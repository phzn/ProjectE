using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Pathfinding.Collections;
using Pathfinding.Drawing;
using Pathfinding.RVO;
using Pathfinding.Sync;
using Pathfinding.Util;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200007A RID: 122
	[BurstCompile]
	public class NavmeshEdges
	{
		// Token: 0x0600045C RID: 1116 RVA: 0x000152A0 File Offset: 0x000134A0
		public void Dispose()
		{
			this.rwLock.WriteSync().Unlock();
			this.obstacleData.Dispose();
			if (this.allocationLock.IsCreated)
			{
				this.allocationLock.Dispose();
			}
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x000152E3 File Offset: 0x000134E3
		private void Init()
		{
			this.obstacleData.Init(Allocator.Persistent);
			if (!this.allocationLock.IsCreated)
			{
				this.allocationLock = new NativeReference<SpinLock>(Allocator.Persistent, NativeArrayOptions.ClearMemory);
			}
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00015310 File Offset: 0x00013510
		public JobHandle RecalculateObstacles(NativeList<int> dirtyHierarchicalNodes, NativeReference<int> numHierarchicalNodes, JobHandle dependency)
		{
			JobHandle jobHandle2;
			using (NavmeshEdges.MapBoostScheduleMarker.Auto())
			{
				this.Init();
				RWLock.WriteLockAsync writeLockAsync = this.rwLock.Write();
				JobHandle jobHandle = JobHandle.CombineDependencies(dependency, writeLockAsync.dependency);
				try
				{
					jobHandle = new NavmeshEdges.JobResizeObstacles
					{
						numHierarchicalNodes = numHierarchicalNodes,
						obstacles = this.obstacleData.obstacles
					}.Schedule(jobHandle);
					jobHandle = new NavmeshEdges.JobCalculateObstacles
					{
						hGraphGC = this.hierarchicalGraph.gcHandle,
						obstacleVertices = this.obstacleData.obstacleVertices,
						obstacleVertexGroups = this.obstacleData.obstacleVertexGroups,
						obstacles = this.obstacleData.obstacles.AsDeferredJobArray(),
						bounds = this.hierarchicalGraph.bounds.AsDeferredJobArray(),
						dirtyHierarchicalNodes = dirtyHierarchicalNodes,
						allocationLock = this.allocationLock
					}.ScheduleBatch(32, 1, jobHandle);
					this.gizmoVersion++;
					jobHandle2 = jobHandle;
				}
				catch
				{
					jobHandle.Complete();
					throw;
				}
				finally
				{
					writeLockAsync.UnlockAfter(jobHandle);
				}
			}
			return jobHandle2;
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00015460 File Offset: 0x00013660
		public unsafe void OnDrawGizmos(DrawingData gizmos, bool renderInGame)
		{
			if (!this.hierarchicalGraph.isValid)
			{
				return;
			}
			if (!this.obstacleData.obstacleVertices.IsCreated)
			{
				return;
			}
			NodeHasher nodeHasher = new NodeHasher(AstarPath.active);
			nodeHasher.Add<int>(12314127);
			nodeHasher.Add<int>(this.gizmoVersion);
			if (!gizmos.Draw(nodeHasher))
			{
				RWLock.LockSync lockSync = this.rwLock.ReadSync();
				try
				{
					using (CommandBuilder builder = gizmos.GetBuilder(nodeHasher, default(RedrawScope), renderInGame))
					{
						for (int i = 1; i < this.obstacleData.obstacles.Length; i++)
						{
							UnmanagedObstacle unmanagedObstacle = this.obstacleData.obstacles[i];
							UnsafeSpan<float3> span = this.obstacleData.obstacleVertices.GetSpan(unmanagedObstacle.verticesAllocation);
							UnsafeSpan<ObstacleVertexGroup> span2 = this.obstacleData.obstacleVertexGroups.GetSpan(unmanagedObstacle.groupsAllocation);
							int num = 0;
							for (int j = 0; j < span2.Length; j++)
							{
								ObstacleVertexGroup obstacleVertexGroup = *span2[j];
								builder.PushLineWidth(2f, true);
								for (int k = 0; k < obstacleVertexGroup.vertexCount - 1; k++)
								{
									builder.ArrowRelativeSizeHead(*span[num + k], *span[num + k + 1], new float3(0f, 1f, 0f), 0.05f, Color.black);
								}
								if (obstacleVertexGroup.type == ObstacleType.Loop)
								{
									builder.Arrow(*span[num + obstacleVertexGroup.vertexCount - 1], *span[num], new float3(0f, 1f, 0f), 0.05f, Color.black);
								}
								builder.PopLineWidth();
								num += obstacleVertexGroup.vertexCount;
								builder.WireBox(0.5f * (obstacleVertexGroup.boundsMn + obstacleVertexGroup.boundsMx), obstacleVertexGroup.boundsMx - obstacleVertexGroup.boundsMn, Color.white);
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

		// Token: 0x06000460 RID: 1120 RVA: 0x000156E0 File Offset: 0x000138E0
		public NavmeshEdges.NavmeshBorderData GetNavmeshEdgeData(out RWLock.CombinedReadLockAsync readLock)
		{
			if (!this.hierarchicalGraph.isValid)
			{
				throw new InvalidOperationException("Navigation is suspended after a failed graph rebuild.");
			}
			this.Init();
			RWLock.ReadLockAsync readLockAsync = this.rwLock.Read();
			RWLock.ReadLockAsync readLockAsync2;
			HierarchicalGraph.HierarhicalNodeData hierarhicalNodeData = this.hierarchicalGraph.GetHierarhicalNodeData(out readLockAsync2);
			readLock = new RWLock.CombinedReadLockAsync(readLockAsync, readLockAsync2);
			return new NavmeshEdges.NavmeshBorderData
			{
				hierarhicalNodeData = hierarhicalNodeData,
				obstacleData = this.obstacleData
			};
		}

		// Token: 0x040002B6 RID: 694
		private static readonly ProfilerMarker MapBoostScheduleMarker = new ProfilerMarker("MapBoost.Nav.ObstacleSchedule");

		// Token: 0x040002B7 RID: 695
		public SimulatorBurst.ObstacleData obstacleData;

		// Token: 0x040002B8 RID: 696
		private NativeReference<SpinLock> allocationLock;

		// Token: 0x040002B9 RID: 697
		private const int JobRecalculateObstaclesBatchCount = 32;

		// Token: 0x040002BA RID: 698
		private RWLock rwLock = new RWLock();

		// Token: 0x040002BB RID: 699
		public HierarchicalGraph hierarchicalGraph;

		// Token: 0x040002BC RID: 700
		private int gizmoVersion;

		// Token: 0x0200007B RID: 123
		[BurstCompile]
		private struct JobResizeObstacles : IJob
		{
			// Token: 0x06000463 RID: 1123 RVA: 0x00015774 File Offset: 0x00013974
			public void Execute()
			{
				int length = this.obstacles.Length;
				int value = this.numHierarchicalNodes.Value;
				this.obstacles.Resize(value, NativeArrayOptions.UninitializedMemory);
				for (int i = length; i < this.obstacles.Length; i++)
				{
					this.obstacles[i] = new UnmanagedObstacle
					{
						verticesAllocation = -1,
						groupsAllocation = -1
					};
				}
				if (this.obstacles.Length > 0)
				{
					this.obstacles[0] = new UnmanagedObstacle
					{
						verticesAllocation = -2,
						groupsAllocation = -2
					};
				}
			}

			// Token: 0x040002BD RID: 701
			public NativeList<UnmanagedObstacle> obstacles;

			// Token: 0x040002BE RID: 702
			public NativeReference<int> numHierarchicalNodes;
		}

		// Token: 0x0200007C RID: 124
		private struct JobCalculateObstacles : IJobParallelForBatch
		{
			// Token: 0x06000464 RID: 1124 RVA: 0x00015814 File Offset: 0x00013A14
			public void Execute(int startIndex, int count)
			{
				HierarchicalGraph hierarchicalGraph = this.hGraphGC.Target as HierarchicalGraph;
				try
				{
					this.ExecuteBatch(hierarchicalGraph, startIndex, count);
				}
				catch (Exception ex)
				{
					hierarchicalGraph.ReportRecalculationFailure(ex);
				}
			}

			// Token: 0x06000465 RID: 1125 RVA: 0x00015858 File Offset: 0x00013A58
			private void ExecuteBatch(HierarchicalGraph hGraph, int startIndex, int count)
			{
				int num = (this.dirtyHierarchicalNodes.Length + 32 - 1) / 32;
				startIndex *= num;
				count *= num;
				int num2 = math.min(startIndex + count, this.dirtyHierarchicalNodes.Length);
				using (NativeList<RVOObstacleCache.ObstacleSegment> nativeList = new NativeList<RVOObstacleCache.ObstacleSegment>(Allocator.Temp))
				{
					for (int i = startIndex; i < num2; i++)
					{
						nativeList.Clear();
						int num3 = this.dirtyHierarchicalNodes[i];
						this.CalculateBoundingBox(hGraph, num3);
						this.CalculateObstacles(hGraph, num3, this.obstacleVertexGroups, this.obstacleVertices, this.obstacles, nativeList);
					}
				}
			}

			// Token: 0x06000466 RID: 1126 RVA: 0x0001590C File Offset: 0x00013B0C
			private void CalculateBoundingBox(HierarchicalGraph hGraph, int hierarchicalNode)
			{
				List<GraphNode> list = hGraph.children[hierarchicalNode];
				using (NavmeshEdges.JobCalculateObstacles.MarkerBBox.Auto())
				{
					Bounds bounds = default(Bounds);
					if (list.Count != 0)
					{
						if (list[0] is TriangleMeshNode)
						{
							Int3 @int = new Int3(int.MaxValue, int.MaxValue, int.MaxValue);
							Int3 int2 = new Int3(int.MinValue, int.MinValue, int.MinValue);
							for (int i = 0; i < list.Count; i++)
							{
								Int3 int3;
								Int3 int4;
								Int3 int5;
								(list[i] as TriangleMeshNode).GetVertices(out int3, out int4, out int5);
								@int = Int3.Min(Int3.Min(Int3.Min(@int, int3), int4), int5);
								int2 = Int3.Max(Int3.Max(Int3.Max(int2, int3), int4), int5);
							}
							bounds.SetMinMax((Vector3)@int, (Vector3)int2);
						}
						else
						{
							Int3 int6 = new Int3(int.MaxValue, int.MaxValue, int.MaxValue);
							Int3 int7 = new Int3(int.MinValue, int.MinValue, int.MinValue);
							for (int j = 0; j < list.Count; j++)
							{
								GraphNode graphNode = list[j];
								int6 = Int3.Min(int6, graphNode.position);
								int7 = Int3.Max(int7, graphNode.position);
							}
							if (list[0] is GridNodeBase)
							{
								float num;
								if (list[0] is LevelGridNode)
								{
									num = LevelGridNode.GetGridGraph(list[0].GraphIndex).nodeSize;
								}
								else
								{
									num = GridNode.GetGridGraph(list[0].GraphIndex).nodeSize;
								}
								Vector3 vector = num * 0.70710677f * Vector3.one;
								bounds.SetMinMax((Vector3)int6 - vector, (Vector3)int7 + vector);
							}
							else
							{
								bounds.SetMinMax((Vector3)int6, (Vector3)int7);
							}
						}
					}
					this.bounds[hierarchicalNode] = bounds;
				}
			}

			// Token: 0x06000467 RID: 1127 RVA: 0x00015B3C File Offset: 0x00013D3C
			private unsafe void CalculateObstacles(HierarchicalGraph hGraph, int hierarchicalNode, SlabAllocator<ObstacleVertexGroup> obstacleVertexGroups, SlabAllocator<float3> obstacleVertices, NativeArray<UnmanagedObstacle> obstacles, NativeList<RVOObstacleCache.ObstacleSegment> edgesScratch)
			{
				using (NavmeshEdges.JobCalculateObstacles.MarkerObstacles.Auto())
				{
					RVOObstacleCache.CollectContours(hGraph.children[hierarchicalNode], edgesScratch);
					UnmanagedObstacle unmanagedObstacle = obstacles[hierarchicalNode];
					ref SpinLock ptr = ref UnsafeUtility.AsRef<SpinLock>((void*)this.allocationLock.GetUnsafePtr<SpinLock>());
					if (unmanagedObstacle.groupsAllocation != -1)
					{
						ptr.Lock();
						try
						{
							obstacleVertices.Free(unmanagedObstacle.verticesAllocation);
							obstacleVertexGroups.Free(unmanagedObstacle.groupsAllocation);
						}
						finally
						{
							ptr.Unlock();
						}
					}
					List<GraphNode> list = hGraph.children[hierarchicalNode];
					bool flag = true;
					NativeMovementPlane nativeMovementPlane;
					if (list.Count > 0)
					{
						if (list[0] is GridNodeBase)
						{
							nativeMovementPlane = new NativeMovementPlane((list[0].Graph as GridGraph).transform.rotation);
						}
						else if (list[0] is TriangleMeshNode)
						{
							NavmeshBase navmeshBase = list[0].Graph as NavmeshBase;
							nativeMovementPlane = new NativeMovementPlane(navmeshBase.transform.rotation);
							flag = navmeshBase.RecalculateNormals;
						}
						else
						{
							nativeMovementPlane = new NativeMovementPlane(quaternion.identity);
							flag = false;
						}
					}
					else
					{
						nativeMovementPlane = default(NativeMovementPlane);
					}
					UnsafeSpan<RVOObstacleCache.ObstacleSegment> unsafeSpan = edgesScratch.AsUnsafeSpan<RVOObstacleCache.ObstacleSegment>();
					RVOObstacleCache.TraceContours(ref unsafeSpan, ref nativeMovementPlane, hierarchicalNode, (UnmanagedObstacle*)obstacles.GetUnsafePtr<UnmanagedObstacle>(), ref obstacleVertices, ref obstacleVertexGroups, ref ptr, flag);
				}
			}

			// Token: 0x040002BF RID: 703
			public GCHandle hGraphGC;

			// Token: 0x040002C0 RID: 704
			public SlabAllocator<float3> obstacleVertices;

			// Token: 0x040002C1 RID: 705
			public SlabAllocator<ObstacleVertexGroup> obstacleVertexGroups;

			// Token: 0x040002C2 RID: 706
			[NativeDisableParallelForRestriction]
			public NativeArray<UnmanagedObstacle> obstacles;

			// Token: 0x040002C3 RID: 707
			[NativeDisableParallelForRestriction]
			public NativeArray<Bounds> bounds;

			// Token: 0x040002C4 RID: 708
			[ReadOnly]
			public NativeList<int> dirtyHierarchicalNodes;

			// Token: 0x040002C5 RID: 709
			[NativeDisableParallelForRestriction]
			public NativeReference<SpinLock> allocationLock;

			// Token: 0x040002C6 RID: 710
			private static readonly ProfilerMarker MarkerBBox = new ProfilerMarker("HierarchicalBBox");

			// Token: 0x040002C7 RID: 711
			private static readonly ProfilerMarker MarkerObstacles = new ProfilerMarker("CalculateObstacles");

			// Token: 0x040002C8 RID: 712
			private static readonly ProfilerMarker MarkerCollect = new ProfilerMarker("Collect");

			// Token: 0x040002C9 RID: 713
			private static readonly ProfilerMarker MarkerTrace = new ProfilerMarker("Trace");
		}

		// Token: 0x0200007D RID: 125
		public struct NavmeshBorderData
		{
			// Token: 0x06000469 RID: 1129 RVA: 0x00015D0C File Offset: 0x00013F0C
			public static NavmeshEdges.NavmeshBorderData CreateEmpty(Allocator allocator)
			{
				return new NavmeshEdges.NavmeshBorderData
				{
					hierarhicalNodeData = new HierarchicalGraph.HierarhicalNodeData
					{
						connectionAllocator = default(SlabAllocator<int>),
						connectionAllocations = new NativeList<int>(0, allocator),
						bounds = new NativeList<Bounds>(0, allocator)
					},
					obstacleData = new SimulatorBurst.ObstacleData
					{
						obstacleVertexGroups = default(SlabAllocator<ObstacleVertexGroup>),
						obstacleVertices = default(SlabAllocator<float3>),
						obstacles = new NativeList<UnmanagedObstacle>(0, allocator)
					}
				};
			}

			// Token: 0x0600046A RID: 1130 RVA: 0x00015DA4 File Offset: 0x00013FA4
			public void DisposeEmpty(JobHandle dependsOn)
			{
				if (this.hierarhicalNodeData.connectionAllocator.IsCreated)
				{
					throw new InvalidOperationException("NavmeshEdgeData was not empty");
				}
				this.hierarhicalNodeData.connectionAllocations.Dispose(dependsOn);
				this.hierarhicalNodeData.bounds.Dispose(dependsOn);
				this.obstacleData.obstacles.Dispose(dependsOn);
			}

			// Token: 0x0600046B RID: 1131 RVA: 0x00015E04 File Offset: 0x00014004
			private unsafe static void GetHierarchicalNodesInRangeRec(int hierarchicalNode, Bounds bounds, SlabAllocator<int> connectionAllocator, [NoAlias] NativeList<int> connectionAllocations, NativeList<Bounds> nodeBounds, [NoAlias] NativeList<int> indices)
			{
				indices.Add(in hierarchicalNode);
				UnsafeSpan<int> span = connectionAllocator.GetSpan(connectionAllocations[hierarchicalNode]);
				for (int i = 0; i < span.Length; i++)
				{
					int num = *span[i];
					if (nodeBounds[num].Intersects(bounds) && !indices.Contains(num))
					{
						NavmeshEdges.NavmeshBorderData.GetHierarchicalNodesInRangeRec(num, bounds, connectionAllocator, connectionAllocations, nodeBounds, indices);
					}
				}
			}

			// Token: 0x0600046C RID: 1132 RVA: 0x00015E70 File Offset: 0x00014070
			private unsafe static void ConvertObstaclesToEdges(ref SimulatorBurst.ObstacleData obstacleData, NativeList<int> obstacleIndices, Bounds localBounds, NativeList<float2> edgeBuffer, NativeMovementPlane movementPlane)
			{
				Bounds bounds = movementPlane.ToWorld(localBounds);
				ToPlaneMatrix toPlaneMatrix = movementPlane.AsWorldToPlaneMatrix();
				float3 @float = bounds.min;
				float3 float2 = bounds.max;
				float3 float3 = localBounds.min;
				float3 float4 = localBounds.max;
				int num = 0;
				for (int i = 0; i < obstacleIndices.Length; i++)
				{
					UnmanagedObstacle unmanagedObstacle = obstacleData.obstacles[obstacleIndices[i]];
					num += obstacleData.obstacleVertices.GetSpan(unmanagedObstacle.verticesAllocation).Length;
				}
				edgeBuffer.ResizeUninitialized(num * 3);
				int num2 = 0;
				for (int j = 0; j < obstacleIndices.Length; j++)
				{
					UnmanagedObstacle unmanagedObstacle2 = obstacleData.obstacles[obstacleIndices[j]];
					if (unmanagedObstacle2.verticesAllocation != -1)
					{
						UnsafeSpan<float3> span = obstacleData.obstacleVertices.GetSpan(unmanagedObstacle2.verticesAllocation);
						UnsafeSpan<ObstacleVertexGroup> span2 = obstacleData.obstacleVertexGroups.GetSpan(unmanagedObstacle2.groupsAllocation);
						int num3 = 0;
						for (int k = 0; k < span2.Length; k++)
						{
							ObstacleVertexGroup obstacleVertexGroup = *span2[k];
							if (!math.all((obstacleVertexGroup.boundsMx >= @float) & (obstacleVertexGroup.boundsMn <= float2)))
							{
								num3 += obstacleVertexGroup.vertexCount;
							}
							else
							{
								bool flag = obstacleVertexGroup.type == ObstacleType.Loop;
								int num4 = num3 + (flag ? (obstacleVertexGroup.vertexCount - 1) : 0);
								for (int l = num3 + (flag ? 0 : 1); l < num3 + obstacleVertexGroup.vertexCount; l++)
								{
									float3 float5 = *span[num4];
									float3 float6 = *span[l];
									float3 float7 = math.min(float5, float6);
									if (math.all((math.max(float5, float6) >= @float) & (float7 <= float2)))
									{
										float3 float8 = toPlaneMatrix.ToXZPlane(float5);
										float3 float9 = toPlaneMatrix.ToXZPlane(float6);
										float7 = math.min(float8, float9);
										if (math.all((math.max(float8, float9) >= float3) & (float7 <= float4)))
										{
											edgeBuffer[num2++] = float8.xz;
											edgeBuffer[num2++] = float9.xz;
										}
									}
									num4 = l;
								}
								num3 += obstacleVertexGroup.vertexCount;
							}
						}
					}
				}
				edgeBuffer.Length = num2;
			}

			// Token: 0x0600046D RID: 1133 RVA: 0x00016110 File Offset: 0x00014310
			public unsafe GraphHitInfo GetClosestEdge(int hierarchicalNode, float3 position, NativeList<int> scratchBuffer)
			{
				GraphHitInfo graphHitInfo = default(GraphHitInfo);
				graphHitInfo.origin = position;
				graphHitInfo.point = Vector3.positiveInfinity;
				if (!this.obstacleData.obstacleVertices.IsCreated)
				{
					return graphHitInfo;
				}
				if (hierarchicalNode < 0 || hierarchicalNode >= this.hierarhicalNodeData.connectionAllocations.Length || this.hierarhicalNodeData.connectionAllocations[hierarchicalNode] == -2)
				{
					return graphHitInfo;
				}
				NativeList<int> nativeList = scratchBuffer;
				nativeList.Clear();
				nativeList.Add(in hierarchicalNode);
				int i = 0;
				float num = float.PositiveInfinity;
				while (i < nativeList.Length)
				{
					int num2 = nativeList[i++];
					if (this.hierarhicalNodeData.bounds[num2].SqrDistance(position) <= num)
					{
						UnmanagedObstacle unmanagedObstacle = this.obstacleData.obstacles[num2];
						if (unmanagedObstacle.verticesAllocation != -1)
						{
							UnsafeSpan<float3> span = this.obstacleData.obstacleVertices.GetSpan(unmanagedObstacle.verticesAllocation);
							UnsafeSpan<ObstacleVertexGroup> span2 = this.obstacleData.obstacleVertexGroups.GetSpan(unmanagedObstacle.groupsAllocation);
							int num3 = 0;
							for (int j = 0; j < span2.Length; j++)
							{
								ObstacleVertexGroup obstacleVertexGroup = *span2[j];
								if (math.lengthsq(math.min(math.max(position, obstacleVertexGroup.boundsMn), obstacleVertexGroup.boundsMx) - position) < num)
								{
									bool flag = obstacleVertexGroup.type == ObstacleType.Loop;
									int num4 = num3 + (flag ? (obstacleVertexGroup.vertexCount - 1) : 0);
									for (int k = num3 + (flag ? 0 : 1); k < num3 + obstacleVertexGroup.vertexCount; k++)
									{
										float3 @float = *span[num4];
										float3 float2 = *span[k];
										float num5 = math.clamp(VectorMath.ClosestPointOnLineFactor(@float, float2, position), 0f, 1f);
										float3 float3 = math.lerp(@float, float2, num5);
										float num6 = math.lengthsq(float3 - position);
										if (num6 < num)
										{
											num = num6;
											graphHitInfo.point = float3;
											graphHitInfo.tangent = float2 - @float;
											graphHitInfo.tangentOrigin = @float;
										}
										num4 = k;
									}
								}
								num3 += obstacleVertexGroup.vertexCount;
							}
						}
						UnsafeSpan<int> span3 = this.hierarhicalNodeData.connectionAllocator.GetSpan(this.hierarhicalNodeData.connectionAllocations[num2]);
						for (int l = 0; l < span3.Length; l++)
						{
							int num7 = *span3[l];
							if (!nativeList.Contains(num7))
							{
								nativeList.Add(in num7);
							}
						}
					}
				}
				return graphHitInfo;
			}

			// Token: 0x0600046E RID: 1134 RVA: 0x000163D4 File Offset: 0x000145D4
			public void GetObstaclesInRange(int hierarchicalNode, Bounds bounds, NativeList<int> obstacleIndexBuffer)
			{
				obstacleIndexBuffer.Clear();
				if (!this.obstacleData.obstacleVertices.IsCreated)
				{
					return;
				}
				if (hierarchicalNode < 0 || hierarchicalNode >= this.hierarhicalNodeData.connectionAllocations.Length || this.hierarhicalNodeData.connectionAllocations[hierarchicalNode] == -2)
				{
					return;
				}
				NavmeshEdges.NavmeshBorderData.GetHierarchicalNodesInRangeRec(hierarchicalNode, bounds, this.hierarhicalNodeData.connectionAllocator, this.hierarhicalNodeData.connectionAllocations, this.hierarhicalNodeData.bounds, obstacleIndexBuffer);
			}

			// Token: 0x0600046F RID: 1135 RVA: 0x00016451 File Offset: 0x00014651
			public void GetEdgesInRange(int hierarchicalNode, Bounds localBounds, NativeList<float2> edgeBuffer, NativeList<int> scratchBuffer, NativeMovementPlane movementPlane)
			{
				this.GetObstaclesInRange(hierarchicalNode, movementPlane.ToWorld(localBounds), scratchBuffer);
				NavmeshEdges.NavmeshBorderData.ConvertObstaclesToEdges(ref this.obstacleData, scratchBuffer, localBounds, edgeBuffer, movementPlane);
			}

			// Token: 0x040002CA RID: 714
			public HierarchicalGraph.HierarhicalNodeData hierarhicalNodeData;

			// Token: 0x040002CB RID: 715
			public SimulatorBurst.ObstacleData obstacleData;
		}
	}
}
