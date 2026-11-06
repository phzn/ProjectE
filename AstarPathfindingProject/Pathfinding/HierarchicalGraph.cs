using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Pathfinding.Collections;
using Pathfinding.Drawing;
using Pathfinding.Pooling;
using Pathfinding.Sync;
using Pathfinding.Util;
using Unity.Collections;
using Unity.Jobs;
using Unity.Profiling;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x020000AA RID: 170
	public class HierarchicalGraph
	{
		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060005A3 RID: 1443 RVA: 0x0001B2A9 File Offset: 0x000194A9
		// (set) Token: 0x060005A4 RID: 1444 RVA: 0x0001B2B1 File Offset: 0x000194B1
		public int version { get; private set; }

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060005A5 RID: 1445 RVA: 0x0001B2BA File Offset: 0x000194BA
		// (set) Token: 0x060005A6 RID: 1446 RVA: 0x0001B2C2 File Offset: 0x000194C2
		public bool isValid { get; private set; } = true;

		// Token: 0x060005A7 RID: 1447 RVA: 0x0001B2CC File Offset: 0x000194CC
		internal void OnDisable()
		{
			this.rwLock.WriteSync().Unlock();
			this.navmeshEdges.Dispose();
			if (this.gcHandle.IsAllocated)
			{
				this.gcHandle.Free();
			}
			if (this.connectionAllocator.IsCreated)
			{
				this.numHierarchicalNodes.Dispose();
				this.connectionAllocator.Dispose();
				this.connectionAllocations.Dispose();
				this.bounds.Dispose();
				this.dirtiedHierarchicalNodes.Dispose();
				this.dirtyNodes.Dispose();
				this.children = null;
				this.areas = null;
				this.dirty = null;
				this.versions = null;
				this.freeNodeIndices.Clear();
			}
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x0001B385 File Offset: 0x00019585
		public int GetHierarchicalNodeVersion(int index)
		{
			return (index * 71237) ^ this.versions[index];
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x0001B398 File Offset: 0x00019598
		public HierarchicalGraph.HierarhicalNodeData GetHierarhicalNodeData(out RWLock.ReadLockAsync readLock)
		{
			if (!this.isValid)
			{
				throw new InvalidOperationException("Navigation is suspended after a failed graph rebuild.", this.recalculationFailure);
			}
			readLock = this.rwLock.Read();
			return new HierarchicalGraph.HierarhicalNodeData
			{
				connectionAllocator = this.connectionAllocator,
				connectionAllocations = this.connectionAllocations,
				bounds = this.bounds
			};
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x0001B400 File Offset: 0x00019600
		internal HierarchicalGraph(GlobalNodeStorage nodeStorage)
		{
			this.nodeStorage = nodeStorage;
			this.navmeshEdges = new NavmeshEdges();
			this.navmeshEdges.hierarchicalGraph = this;
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0001B470 File Offset: 0x00019670
		public void OnEnable()
		{
			if (!this.connectionAllocator.IsCreated)
			{
				this.isValid = true;
				this.recalculationFailure = null;
				this.gcHandle = GCHandle.Alloc(this);
				this.connectionAllocator = new SlabAllocator<int>(1024, Allocator.Persistent);
				this.connectionAllocations = new NativeList<int>(0, Allocator.Persistent);
				this.bounds = new NativeList<Bounds>(0, Allocator.Persistent);
				this.numHierarchicalNodes = new NativeReference<int>(0, Allocator.Persistent);
				this.dirtiedHierarchicalNodes = new NativeList<int>(0, Allocator.Persistent);
				this.dirtyNodes = new HierarchicalBitset(1024, Allocator.Persistent);
				this.children = new List<GraphNode>[]
				{
					new List<GraphNode>()
				};
				this.areas = new int[1];
				this.dirty = new byte[1];
				this.versions = new int[1];
				this.freeNodeIndices.Clear();
			}
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x0001B559 File Offset: 0x00019759
		internal void OnCreatedNode(GraphNode node)
		{
			this.AddDirtyNode(node);
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x0001B562 File Offset: 0x00019762
		internal void OnDestroyedNode(GraphNode node)
		{
			this.dirty[node.HierarchicalNodeIndex] = 1;
			this.dirtyNodes.Reset((int)node.NodeIndex);
			node.IsHierarchicalNodeDirty = false;
			this.anyNodesDestroyed = true;
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x0001B594 File Offset: 0x00019794
		public void AddDirtyNode(GraphNode node)
		{
			if (!node.IsHierarchicalNodeDirty)
			{
				if (!this.dirtyNodes.IsCreated || node.Destroyed)
				{
					return;
				}
				this.dirtyNodes.Set((int)node.NodeIndex);
				this.dirty[node.HierarchicalNodeIndex] = 1;
				node.IsHierarchicalNodeDirty = true;
			}
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x0001B5E5 File Offset: 0x000197E5
		public void ReserveNodeIndices(uint nodeIndexCount)
		{
			this.dirtyNodes.Capacity = Mathf.Max(this.dirtyNodes.Capacity, (int)nodeIndexCount);
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060005B0 RID: 1456 RVA: 0x0001B603 File Offset: 0x00019803
		// (set) Token: 0x060005B1 RID: 1457 RVA: 0x0001B60B File Offset: 0x0001980B
		public int NumConnectedComponents { get; private set; }

		// Token: 0x060005B2 RID: 1458 RVA: 0x0001B614 File Offset: 0x00019814
		public uint GetConnectedComponent(int hierarchicalNodeIndex)
		{
			if (!this.isValid)
			{
				return 0U;
			}
			return (uint)this.areas[hierarchicalNodeIndex];
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x0001B628 File Offset: 0x00019828
		public void RecalculateIfNecessary()
		{
			this.JobRecalculateIfNecessary(default(JobHandle)).Complete();
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x0001B64C File Offset: 0x0001984C
		private void RepairInvalidConnections()
		{
			int removed = 0;
			AstarPath.active.data.GetNodes(delegate(GraphNode node)
			{
				if (node == null || node.Destroyed)
				{
					throw new InvalidOperationException("A graph contains a null or destroyed node.");
				}
				this.invalidConnections.Clear();
				List<GraphNode> list = this.invalidConnections;
				node.GetConnections<List<GraphNode>>(delegate(GraphNode other, ref List<GraphNode> invalid)
				{
					if (other == null || other.Destroyed)
					{
						invalid.Add(other);
					}
				}, ref list, 48);
				for (int i = 0; i < this.invalidConnections.Count; i++)
				{
					node.RemovePartialConnection(this.invalidConnections[i]);
					int removed2 = removed;
					removed = removed2 + 1;
				}
				if (this.invalidConnections.Count > 0)
				{
					this.AddDirtyNode(node);
					node.GetConnections(delegate(GraphNode other)
					{
						if (other == null || other.Destroyed)
						{
							throw new InvalidOperationException("A graph node could not remove an invalid connection.");
						}
					}, 48);
				}
			});
			this.invalidConnections.Clear();
			if (removed > 0)
			{
				Debug.LogWarning("A* repaired " + removed.ToString() + " dangling graph connection(s) before rebuilding navigation.");
			}
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x0001B6BB File Offset: 0x000198BB
		internal void ReportRecalculationFailure(Exception exception)
		{
			Interlocked.CompareExchange<Exception>(ref this.recalculationFailure, exception, null);
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x0001B6CC File Offset: 0x000198CC
		internal bool TryRecover()
		{
			bool flag;
			try
			{
				using (this.rwLock.WriteSync())
				{
					this.RepairInvalidConnections();
					this.navmeshEdges.Dispose();
					this.connectionAllocator.Clear();
					this.freeNodeIndices.Clear();
					this.dirtyNodes.Clear();
					this.dirtiedHierarchicalNodes.Clear();
					this.temporaryQueue.Clear();
					this.temporaryStack.Clear();
					this.currentConnections.Clear();
					for (int i = this.children.Length - 1; i >= 0; i--)
					{
						this.children[i].Clear();
						this.areas[i] = 0;
						this.dirty[i] = 0;
						this.versions[i]++;
						if (i < this.connectionAllocations.Length)
						{
							this.connectionAllocations[i] = -2;
						}
						if (i < this.bounds.Length)
						{
							this.bounds[i] = default(Bounds);
						}
						if (i > 0)
						{
							this.freeNodeIndices.PushEnd(i);
						}
					}
					AstarPath.active.data.GetNodes(delegate(GraphNode node)
					{
						node.HierarchicalNodeIndex = 0;
						node.IsHierarchicalNodeDirty = false;
						this.AddDirtyNode(node);
					});
					this.anyNodesDestroyed = true;
					this.recalculationFailure = null;
					this.isValid = true;
				}
				this.RecalculateIfNecessary();
				flag = this.isValid;
			}
			catch (Exception ex)
			{
				this.isValid = false;
				this.ReportRecalculationFailure(ex);
				AstarPath.active.SuspendNavigation(ex);
				flag = false;
			}
			return flag;
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x0001B884 File Offset: 0x00019A84
		public JobHandle JobRecalculateIfNecessary(JobHandle dependsOn = default(JobHandle))
		{
			JobHandle jobHandle;
			using (HierarchicalGraph.MapBoostHierarchyMarker.Auto())
			{
				if (!this.connectionAllocator.IsCreated)
				{
					throw new InvalidOperationException("The hierarchical graph has not been initialized. Please call OnEnable before using it.");
				}
				if (!this.isValid)
				{
					jobHandle = dependsOn;
				}
				else if (!this.dirtyNodes.IsEmpty || this.anyNodesDestroyed)
				{
					RWLock.WriteLockAsync writeLockAsync = this.rwLock.Write();
					JobHandle jobHandle2 = JobHandle.CombineDependencies(writeLockAsync.dependency, dependsOn);
					try
					{
						using (HierarchicalGraph.MapBoostPriorFenceMarker.Auto())
						{
							jobHandle2.Complete();
						}
						using (HierarchicalGraph.MapBoostRepairMarker.Auto())
						{
							this.RepairInvalidConnections();
						}
						this.recalculationFailure = null;
						using (HierarchicalGraph.MapBoostComponentsMarker.Auto())
						{
							HierarchicalGraph.JobRecalculateComponents jobRecalculateComponents = default(HierarchicalGraph.JobRecalculateComponents);
							jobRecalculateComponents.hGraphGC = this.gcHandle;
							jobRecalculateComponents.connectionAllocations = this.connectionAllocations;
							jobRecalculateComponents.bounds = this.bounds;
							jobRecalculateComponents.dirtiedHierarchicalNodes = this.dirtiedHierarchicalNodes;
							jobRecalculateComponents.numHierarchicalNodes = this.numHierarchicalNodes;
							jobRecalculateComponents.Execute();
						}
						jobHandle2 = this.navmeshEdges.RecalculateObstacles(this.dirtiedHierarchicalNodes, this.numHierarchicalNodes, default(JobHandle));
						using (HierarchicalGraph.MapBoostObstacleFenceMarker.Auto())
						{
							jobHandle2.Complete();
						}
						if (this.recalculationFailure != null)
						{
							throw new InvalidOperationException("Navmesh obstacle rebuild failed.", this.recalculationFailure);
						}
					}
					catch (Exception ex)
					{
						this.isValid = false;
						this.ReportRecalculationFailure(ex);
						AstarPath.active.SuspendNavigation(ex);
					}
					finally
					{
						writeLockAsync.UnlockAfter(jobHandle2);
					}
					jobHandle = jobHandle2;
				}
				else
				{
					jobHandle = dependsOn;
				}
			}
			return jobHandle;
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x0001BB0C File Offset: 0x00019D0C
		public void RecalculateAll()
		{
			RWLock.LockSync lockSync = this.rwLock.WriteSync();
			AstarPath.active.data.GetNodes(new Action<GraphNode>(this.AddDirtyNode));
			lockSync.Unlock();
			this.RecalculateIfNecessary();
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x0001BB50 File Offset: 0x00019D50
		public unsafe void OnDrawGizmos(DrawingData gizmos, bool renderInGame)
		{
			if (!this.isValid)
			{
				return;
			}
			NodeHasher nodeHasher = new NodeHasher(AstarPath.active);
			nodeHasher.Add<int>(this.gizmoVersion);
			if (!gizmos.Draw(nodeHasher))
			{
				RWLock.LockSync lockSync = this.rwLock.ReadSync();
				try
				{
					using (CommandBuilder builder = gizmos.GetBuilder(nodeHasher, default(RedrawScope), renderInGame))
					{
						for (int i = 0; i < this.areas.Length; i++)
						{
							if (this.children[i].Count > 0)
							{
								builder.WireBox(this.bounds[i].center, this.bounds[i].size);
								UnsafeSpan<int> span = this.connectionAllocator.GetSpan(this.connectionAllocations[i]);
								for (int j = 0; j < span.Length; j++)
								{
									if (*span[j] > i)
									{
										builder.Line(this.bounds[i].center, this.bounds[*span[j]].center, Color.black);
									}
								}
							}
						}
					}
				}
				finally
				{
					lockSync.Unlock();
				}
			}
		}

		// Token: 0x0400038E RID: 910
		private static readonly ProfilerMarker MapBoostHierarchyMarker = new ProfilerMarker("MapBoost.Nav.Hierarchy");

		// Token: 0x0400038F RID: 911
		private static readonly ProfilerMarker MapBoostPriorFenceMarker = new ProfilerMarker("MapBoost.Nav.HierarchyPriorFence");

		// Token: 0x04000390 RID: 912
		private static readonly ProfilerMarker MapBoostRepairMarker = new ProfilerMarker("MapBoost.Nav.RepairConnections");

		// Token: 0x04000391 RID: 913
		private static readonly ProfilerMarker MapBoostComponentsMarker = new ProfilerMarker("MapBoost.Nav.Components");

		// Token: 0x04000392 RID: 914
		private static readonly ProfilerMarker MapBoostObstacleFenceMarker = new ProfilerMarker("MapBoost.Nav.ObstacleFence");

		// Token: 0x04000393 RID: 915
		private const int Tiling = 16;

		// Token: 0x04000394 RID: 916
		private const int MaxChildrenPerNode = 256;

		// Token: 0x04000395 RID: 917
		private const int MinChildrenPerNode = 128;

		// Token: 0x04000396 RID: 918
		private GlobalNodeStorage nodeStorage;

		// Token: 0x04000397 RID: 919
		internal List<GraphNode>[] children;

		// Token: 0x04000398 RID: 920
		internal NativeList<int> connectionAllocations;

		// Token: 0x04000399 RID: 921
		internal SlabAllocator<int> connectionAllocator;

		// Token: 0x0400039A RID: 922
		private NativeList<int> dirtiedHierarchicalNodes;

		// Token: 0x0400039B RID: 923
		private int[] areas;

		// Token: 0x0400039C RID: 924
		private byte[] dirty;

		// Token: 0x0400039D RID: 925
		private int[] versions;

		// Token: 0x0400039E RID: 926
		private bool anyNodesDestroyed;

		// Token: 0x0400039F RID: 927
		internal NativeList<Bounds> bounds;

		// Token: 0x040003A0 RID: 928
		private NativeReference<int> numHierarchicalNodes;

		// Token: 0x040003A1 RID: 929
		internal GCHandle gcHandle;

		// Token: 0x040003A3 RID: 931
		public NavmeshEdges navmeshEdges;

		// Token: 0x040003A5 RID: 933
		private Exception recalculationFailure;

		// Token: 0x040003A6 RID: 934
		private readonly List<GraphNode> invalidConnections = new List<GraphNode>();

		// Token: 0x040003A7 RID: 935
		private Queue<GraphNode> temporaryQueue = new Queue<GraphNode>();

		// Token: 0x040003A8 RID: 936
		private List<int> currentConnections = new List<int>();

		// Token: 0x040003A9 RID: 937
		private Stack<int> temporaryStack = new Stack<int>();

		// Token: 0x040003AA RID: 938
		private HierarchicalBitset dirtyNodes;

		// Token: 0x040003AB RID: 939
		private CircularBuffer<int> freeNodeIndices;

		// Token: 0x040003AC RID: 940
		private int gizmoVersion;

		// Token: 0x040003AD RID: 941
		private RWLock rwLock = new RWLock();

		// Token: 0x020000AB RID: 171
		public struct HierarhicalNodeData
		{
			// Token: 0x040003AF RID: 943
			[ReadOnly]
			public SlabAllocator<int> connectionAllocator;

			// Token: 0x040003B0 RID: 944
			[ReadOnly]
			public NativeList<int> connectionAllocations;

			// Token: 0x040003B1 RID: 945
			[ReadOnly]
			public NativeList<Bounds> bounds;
		}

		// Token: 0x020000AC RID: 172
		private struct JobRecalculateComponents : IJob
		{
			// Token: 0x060005BC RID: 1468 RVA: 0x0001BD5C File Offset: 0x00019F5C
			private void Grow(HierarchicalGraph graph)
			{
				List<GraphNode>[] array = new List<GraphNode>[Math.Min(131072, Math.Max(64, graph.children.Length * 2))];
				int[] array2 = new int[array.Length];
				byte[] array3 = new byte[array.Length];
				int[] array4 = new int[array.Length];
				this.numHierarchicalNodes.Value = array.Length;
				graph.children.CopyTo(array, 0);
				graph.areas.CopyTo(array2, 0);
				graph.dirty.CopyTo(array3, 0);
				graph.versions.CopyTo(array4, 0);
				this.bounds.Resize(array.Length, NativeArrayOptions.UninitializedMemory);
				this.connectionAllocations.Resize(array.Length, NativeArrayOptions.ClearMemory);
				for (int i = array.Length - 1; i >= graph.children.Length; i--)
				{
					array[i] = ListPool<GraphNode>.Claim(256);
					this.connectionAllocations[i] = -2;
					if (i > 0)
					{
						graph.freeNodeIndices.PushEnd(i);
					}
				}
				this.connectionAllocations[0] = -2;
				graph.children = array;
				graph.areas = array2;
				graph.dirty = array3;
				graph.versions = array4;
			}

			// Token: 0x060005BD RID: 1469 RVA: 0x0001BE7C File Offset: 0x0001A07C
			private int GetHierarchicalNodeIndex(HierarchicalGraph graph)
			{
				if (graph.freeNodeIndices.Length == 0)
				{
					if (graph.children.Length == 131072)
					{
						if (!this.warnedAboutTooManyHierarchicalNodes)
						{
							Debug.LogError("Max number of hierarchical nodes in the graphs reached. Your graphs must be extremely fragmented. Pathfinding may not work correctly.\nYour graphs likely have too many connected components. They are visualized as differently colored regions when using A* Inspector -> Settings -> Graph Coloring = Areas");
							this.warnedAboutTooManyHierarchicalNodes = true;
						}
						return 0;
					}
					this.Grow(graph);
				}
				return graph.freeNodeIndices.PopEnd();
			}

			// Token: 0x060005BE RID: 1470 RVA: 0x0001BED4 File Offset: 0x0001A0D4
			private unsafe void RemoveHierarchicalNode(HierarchicalGraph hGraph, int hierarchicalNode, bool removeAdjacentSmallNodes)
			{
				hGraph.freeNodeIndices.PushEnd(hierarchicalNode);
				hGraph.versions[hierarchicalNode]++;
				int num = this.connectionAllocations[hierarchicalNode];
				UnsafeSpan<int> unsafeSpan = hGraph.connectionAllocator.GetSpan(num);
				for (int i = 0; i < unsafeSpan.Length; i++)
				{
					int num2 = *unsafeSpan[i];
					if (hGraph.dirty[num2] == 0)
					{
						if (removeAdjacentSmallNodes && hGraph.children[num2].Count < 128)
						{
							hGraph.dirty[num2] = 2;
							this.RemoveHierarchicalNode(hGraph, num2, false);
							unsafeSpan = hGraph.connectionAllocator.GetSpan(num);
						}
						else
						{
							SlabAllocator<int>.List list = hGraph.connectionAllocator.GetList(this.connectionAllocations[num2]);
							(ref list).Remove(hierarchicalNode);
							this.connectionAllocations[num2] = list.allocationIndex;
							unsafeSpan = hGraph.connectionAllocator.GetSpan(num);
						}
					}
				}
				hGraph.connectionAllocator.Free(num);
				this.connectionAllocations[hierarchicalNode] = -2;
				List<GraphNode> list2 = hGraph.children[hierarchicalNode];
				byte b = hGraph.dirty[hierarchicalNode];
				for (int j = 0; j < list2.Count; j++)
				{
					if (!list2[j].Destroyed)
					{
						hGraph.AddDirtyNode(list2[j]);
					}
				}
				hGraph.dirty[hierarchicalNode] = b;
				list2.ClearFast<GraphNode>();
			}

			// Token: 0x060005BF RID: 1471 RVA: 0x0001C038 File Offset: 0x0001A238
			[Conditional("CHECK_INVARIANTS")]
			private unsafe void CheckConnectionInvariants()
			{
				HierarchicalGraph hierarchicalGraph = (HierarchicalGraph)this.hGraphGC.Target;
				int length = this.connectionAllocations.Length;
				for (int i = 0; i < this.connectionAllocations.Length; i++)
				{
					if (this.connectionAllocations[i] != -2)
					{
						UnsafeSpan<int> span = hierarchicalGraph.connectionAllocator.GetSpan(this.connectionAllocations[i]);
						for (int j = 0; j < span.Length; j++)
						{
							if (!hierarchicalGraph.connectionAllocator.GetSpan(this.connectionAllocations[*span[j]]).Contains(i))
							{
								throw new Exception("Connections are not bidirectional");
							}
						}
					}
				}
			}

			// Token: 0x060005C0 RID: 1472 RVA: 0x0001C0E8 File Offset: 0x0001A2E8
			[Conditional("CHECK_INVARIANTS")]
			private void CheckPreUpdateInvariants()
			{
				HierarchicalGraph hierarchicalGraph = (HierarchicalGraph)this.hGraphGC.Target;
				int length = this.connectionAllocations.Length;
				for (int i = 0; i < this.connectionAllocations.Length; i++)
				{
					if (this.connectionAllocations[i] != -2)
					{
						List<GraphNode> list = hierarchicalGraph.children[i];
						for (int j = 0; j < list.Count; j++)
						{
							bool destroyed = list[j].Destroyed;
						}
					}
				}
			}

			// Token: 0x060005C1 RID: 1473 RVA: 0x0001C164 File Offset: 0x0001A364
			[Conditional("CHECK_INVARIANTS")]
			private void CheckChildInvariants()
			{
				HierarchicalGraph hierarchicalGraph = (HierarchicalGraph)this.hGraphGC.Target;
				int length = this.connectionAllocations.Length;
				for (int i = 0; i < this.connectionAllocations.Length; i++)
				{
					if (this.connectionAllocations[i] != -2)
					{
						List<GraphNode> list = hierarchicalGraph.children[i];
						for (int j = 0; j < list.Count; j++)
						{
						}
					}
				}
			}

			// Token: 0x060005C2 RID: 1474 RVA: 0x0001C1D0 File Offset: 0x0001A3D0
			private void FindHierarchicalNodeChildren(HierarchicalGraph hGraph, int hierarchicalNode, GraphNode startNode)
			{
				hGraph.versions[hierarchicalNode]++;
				Queue<GraphNode> temporaryQueue = hGraph.temporaryQueue;
				HierarchicalGraph.JobRecalculateComponents.Context context2 = new HierarchicalGraph.JobRecalculateComponents.Context
				{
					children = hGraph.children[hierarchicalNode],
					hierarchicalNodeIndex = hierarchicalNode,
					connections = hGraph.currentConnections,
					graphindex = startNode.GraphIndex,
					queue = temporaryQueue
				};
				context2.connections.Clear();
				context2.children.Add(startNode);
				context2.queue.Enqueue(startNode);
				startNode.HierarchicalNodeIndex = hierarchicalNode;
				GraphNode.NodeActionWithData<HierarchicalGraph.JobRecalculateComponents.Context> nodeActionWithData = delegate(GraphNode neighbour, ref HierarchicalGraph.JobRecalculateComponents.Context context)
				{
					if (neighbour == null || neighbour.Destroyed)
					{
						throw new InvalidOperationException("A connection became invalid after navigation preflight.");
					}
					int hierarchicalNodeIndex = neighbour.HierarchicalNodeIndex;
					if (hierarchicalNodeIndex == 0)
					{
						if (context.children.Count < 256 && neighbour.Walkable && neighbour.GraphIndex == context.graphindex)
						{
							neighbour.HierarchicalNodeIndex = context.hierarchicalNodeIndex;
							context.queue.Enqueue(neighbour);
							context.children.Add(neighbour);
							return;
						}
					}
					else if (hierarchicalNodeIndex != context.hierarchicalNodeIndex && !context.connections.Contains(hierarchicalNodeIndex))
					{
						context.connections.Add(hierarchicalNodeIndex);
					}
				};
				while (temporaryQueue.Count > 0)
				{
					temporaryQueue.Dequeue().GetConnections<HierarchicalGraph.JobRecalculateComponents.Context>(nodeActionWithData, ref context2, 48);
				}
				if (hGraph.currentConnections.Count > 4096)
				{
					throw new Exception("Too many connections for a single hierarchical node. Do you have thousands of off-mesh links in a single location?");
				}
				for (int i = 0; i < hGraph.currentConnections.Count; i++)
				{
					int num = hGraph.currentConnections[i];
					int num2 = this.connectionAllocations[num];
					SlabAllocator<int>.List list = hGraph.connectionAllocator.GetList(num2);
					list.Add(hierarchicalNode);
					this.connectionAllocations[num] = list.allocationIndex;
				}
				this.connectionAllocations[hierarchicalNode] = hGraph.connectionAllocator.Allocate(hGraph.currentConnections);
				temporaryQueue.Clear();
			}

			// Token: 0x060005C3 RID: 1475 RVA: 0x0001C340 File Offset: 0x0001A540
			private unsafe void FloodFill(HierarchicalGraph hGraph)
			{
				int[] areas = hGraph.areas;
				for (int i = 0; i < areas.Length; i++)
				{
					areas[i] = 0;
				}
				Stack<int> temporaryStack = hGraph.temporaryStack;
				int num = 0;
				for (int j = 1; j < areas.Length; j++)
				{
					if (areas[j] == 0 && this.connectionAllocations[j] != -2)
					{
						num++;
						areas[j] = num;
						temporaryStack.Push(j);
						while (temporaryStack.Count > 0)
						{
							int num2 = temporaryStack.Pop();
							UnsafeSpan<int> span = hGraph.connectionAllocator.GetSpan(this.connectionAllocations[num2]);
							for (int k = span.Length - 1; k >= 0; k--)
							{
								int num3 = *span[k];
								if (areas[num3] != num)
								{
									areas[num3] = num;
									temporaryStack.Push(num3);
								}
							}
						}
					}
				}
				hGraph.NumConnectedComponents = Math.Max(1, num + 1);
				int version = hGraph.version;
				hGraph.version = version + 1;
			}

			// Token: 0x060005C4 RID: 1476 RVA: 0x0001C438 File Offset: 0x0001A638
			public void Execute()
			{
				try
				{
					this.ExecuteInternal();
				}
				finally
				{
				}
			}

			// Token: 0x060005C5 RID: 1477 RVA: 0x0001C460 File Offset: 0x0001A660
			private unsafe void ExecuteInternal()
			{
				HierarchicalGraph hierarchicalGraph = this.hGraphGC.Target as HierarchicalGraph;
				byte[] dirty = hierarchicalGraph.dirty;
				int length = hierarchicalGraph.freeNodeIndices.Length;
				for (int i = 1; i < dirty.Length; i++)
				{
					if (dirty[i] == 1)
					{
						this.RemoveHierarchicalNode(hierarchicalGraph, i, true);
					}
				}
				for (int j = 1; j < dirty.Length; j++)
				{
					dirty[j] = 0;
				}
				using (NativeArray<int> nativeArray = new NativeArray<int>(512, Allocator.Temp, NativeArrayOptions.UninitializedMemory))
				{
					GlobalNodeStorage nodeStorage = hierarchicalGraph.nodeStorage;
					foreach (UnsafeSpan<int> unsafeSpan in hierarchicalGraph.dirtyNodes.GetIterator(nativeArray.AsUnsafeSpan<int>()))
					{
						for (int k = 0; k < unsafeSpan.Length; k++)
						{
							GraphNode node = nodeStorage.GetNode((uint)(*unsafeSpan[k]));
							node.IsHierarchicalNodeDirty = false;
							node.HierarchicalNodeIndex = 0;
						}
					}
					this.dirtiedHierarchicalNodes.Clear();
					foreach (UnsafeSpan<int> unsafeSpan2 in hierarchicalGraph.dirtyNodes.GetIterator(nativeArray.AsUnsafeSpan<int>()))
					{
						for (int l = 0; l < unsafeSpan2.Length; l++)
						{
							GraphNode node2 = nodeStorage.GetNode((uint)(*unsafeSpan2[l]));
							if (!node2.Destroyed && node2.HierarchicalNodeIndex == 0 && node2.Walkable)
							{
								int hierarchicalNodeIndex = this.GetHierarchicalNodeIndex(hierarchicalGraph);
								if (hierarchicalNodeIndex != 0)
								{
									this.FindHierarchicalNodeChildren(hierarchicalGraph, hierarchicalNodeIndex, node2);
									this.dirtiedHierarchicalNodes.Add(in hierarchicalNodeIndex);
								}
							}
						}
					}
					for (int m = length; m < hierarchicalGraph.freeNodeIndices.Length; m++)
					{
						int num = hierarchicalGraph.freeNodeIndices[m];
						this.dirtiedHierarchicalNodes.Add(in num);
					}
					hierarchicalGraph.dirtyNodes.Clear();
					hierarchicalGraph.anyNodesDestroyed = false;
					this.FloodFill(hierarchicalGraph);
					hierarchicalGraph.gizmoVersion++;
				}
			}

			// Token: 0x040003B2 RID: 946
			public GCHandle hGraphGC;

			// Token: 0x040003B3 RID: 947
			public NativeList<int> connectionAllocations;

			// Token: 0x040003B4 RID: 948
			public NativeList<Bounds> bounds;

			// Token: 0x040003B5 RID: 949
			public NativeList<int> dirtiedHierarchicalNodes;

			// Token: 0x040003B6 RID: 950
			public NativeReference<int> numHierarchicalNodes;

			// Token: 0x040003B7 RID: 951
			private bool warnedAboutTooManyHierarchicalNodes;

			// Token: 0x020000AD RID: 173
			private struct Context
			{
				// Token: 0x040003B8 RID: 952
				public List<GraphNode> children;

				// Token: 0x040003B9 RID: 953
				public int hierarchicalNodeIndex;

				// Token: 0x040003BA RID: 954
				public List<int> connections;

				// Token: 0x040003BB RID: 955
				public uint graphindex;

				// Token: 0x040003BC RID: 956
				public Queue<GraphNode> queue;
			}
		}
	}
}
