using System;
using Pathfinding.Collections;
using Pathfinding.Jobs;
using Pathfinding.Util;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Pathfinding
{
	// Token: 0x020000A0 RID: 160
	internal class GlobalNodeStorage
	{
		// Token: 0x1700011A RID: 282
		// (get) Token: 0x0600056D RID: 1389 RVA: 0x0001A4B3 File Offset: 0x000186B3
		// (set) Token: 0x0600056E RID: 1390 RVA: 0x0001A4BB File Offset: 0x000186BB
		public uint destroyedNodesVersion { get; private set; }

		// Token: 0x0600056F RID: 1391 RVA: 0x0001A4C4 File Offset: 0x000186C4
		public GlobalNodeStorage(AstarPath astar)
		{
			this.astar = astar;
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x0001A52C File Offset: 0x0001872C
		public GraphNode GetNode(uint nodeIndex)
		{
			return this.nodes[(int)nodeIndex];
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x0001A538 File Offset: 0x00018738
		private void DisposeThreadData()
		{
			if (this.pathfindingThreadData.Length != 0)
			{
				for (int i = 0; i < this.pathfindingThreadData.Length; i++)
				{
					this.pathfindingThreadData[i].pathNodes.Free(Allocator.Persistent);
				}
				this.pathfindingThreadData = new GlobalNodeStorage.PathfindingThreadData[0];
			}
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x0001A584 File Offset: 0x00018784
		public void SetThreadCount(int threadCount)
		{
			if (this.pathfindingThreadData.Length != threadCount)
			{
				this.DisposeThreadData();
				this.pathfindingThreadData = new GlobalNodeStorage.PathfindingThreadData[threadCount];
				for (int i = 0; i < this.pathfindingThreadData.Length; i++)
				{
					this.pathfindingThreadData[i].pathNodes = new UnsafeSpan<PathNode>(Allocator.Persistent, (int)(this.reservedPathNodeData + (uint)this.temporaryNodeCount));
					this.pathfindingThreadData[i].pathNodes.Fill(PathNode.Default);
				}
			}
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x0001A600 File Offset: 0x00018800
		public void GrowTemporaryNodeStorage(int threadID)
		{
			GlobalNodeStorage.PathfindingThreadData pathfindingThreadData = this.pathfindingThreadData[threadID];
			int num = pathfindingThreadData.pathNodes.Length - (int)this.reservedPathNodeData;
			this.temporaryNodeCount = Math.Max(this.temporaryNodeCount, num * 8);
			int length = pathfindingThreadData.pathNodes.Length;
			pathfindingThreadData.pathNodes = pathfindingThreadData.pathNodes.Reallocate(Allocator.Persistent, (int)(this.reservedPathNodeData + (uint)this.temporaryNodeCount));
			pathfindingThreadData.pathNodes.Slice(length).Fill(PathNode.Default);
			this.pathfindingThreadData[threadID] = pathfindingThreadData;
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x0001A694 File Offset: 0x00018894
		public void InitializeNode(GraphNode node)
		{
			int pathNodeVariants = node.PathNodeVariants;
			lock (this)
			{
				if (this.nodeIndexPools[pathNodeVariants - 1].Count > 0)
				{
					node.NodeIndex = this.nodeIndexPools[pathNodeVariants - 1].Pop();
				}
				else
				{
					node.NodeIndex = this.nextNodeIndex;
					this.nextNodeIndex += (uint)pathNodeVariants;
					this.ReserveNodeIndices(this.nextNodeIndex);
				}
				for (int i = 0; i < pathNodeVariants; i++)
				{
					checked
					{
						this.nodes[(int)((IntPtr)(unchecked((ulong)node.NodeIndex + (ulong)((long)i))))] = node;
					}
				}
				this.astar.hierarchicalGraph.OnCreatedNode(node);
			}
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x0001A750 File Offset: 0x00018950
		private void ReserveNodeIndices(uint nextNodeIndex)
		{
			if (nextNodeIndex <= this.reservedPathNodeData)
			{
				return;
			}
			this.reservedPathNodeData = math.ceilpow2(nextNodeIndex);
			this.astar.hierarchicalGraph.ReserveNodeIndices(this.reservedPathNodeData);
			int num = this.pathfindingThreadData.Length;
			this.DisposeThreadData();
			this.SetThreadCount(num);
			Memory.Realloc<GraphNode>(ref this.nodes, (int)this.reservedPathNodeData);
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x0001A7B0 File Offset: 0x000189B0
		public void DestroyNode(GraphNode node)
		{
			uint nodeIndex = node.NodeIndex;
			if (nodeIndex == 268435454U)
			{
				return;
			}
			uint destroyedNodesVersion = this.destroyedNodesVersion;
			this.destroyedNodesVersion = destroyedNodesVersion + 1U;
			int pathNodeVariants = node.PathNodeVariants;
			this.nodeIndexPools[pathNodeVariants - 1].Push(nodeIndex);
			for (int i = 0; i < pathNodeVariants; i++)
			{
				checked
				{
					this.nodes[(int)((IntPtr)(unchecked((ulong)nodeIndex + (ulong)((long)i))))] = null;
				}
			}
			for (int j = 0; j < this.pathfindingThreadData.Length; j++)
			{
				GlobalNodeStorage.PathfindingThreadData pathfindingThreadData = this.pathfindingThreadData[j];
				uint num = 0U;
				while ((ulong)num < (ulong)((long)pathNodeVariants))
				{
					pathfindingThreadData.pathNodes[nodeIndex + num].pathID = 0;
					num += 1U;
				}
			}
			this.astar.hierarchicalGraph.OnDestroyedNode(node);
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x0001A870 File Offset: 0x00018A70
		public void OnDisable()
		{
			this.lastAllocationJob.Complete();
			this.nextNodeIndex = 1U;
			this.reservedPathNodeData = 0U;
			for (int i = 0; i < this.nodeIndexPools.Length; i++)
			{
				this.nodeIndexPools[i].Clear();
			}
			this.nodes = new GraphNode[0];
			this.DisposeThreadData();
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x0001A8C8 File Offset: 0x00018AC8
		public JobHandle AllocateNodesJob<T>(T[] result, int count, Func<T> createNode, uint variantsPerNode) where T : GraphNode
		{
			this.lastAllocationJob = new GlobalNodeStorage.JobAllocateNodes<T>
			{
				result = result,
				count = count,
				nodeStorage = this,
				variantsPerNode = variantsPerNode,
				createNode = createNode
			}.ScheduleManaged(this.lastAllocationJob);
			return this.lastAllocationJob;
		}

		// Token: 0x0400036B RID: 875
		private readonly AstarPath astar;

		// Token: 0x0400036C RID: 876
		private JobHandle lastAllocationJob;

		// Token: 0x0400036D RID: 877
		public uint nextNodeIndex = 1U;

		// Token: 0x0400036E RID: 878
		public uint reservedPathNodeData;

		// Token: 0x04000370 RID: 880
		private const int InitialTemporaryNodes = 256;

		// Token: 0x04000371 RID: 881
		private int temporaryNodeCount = 256;

		// Token: 0x04000372 RID: 882
		private readonly GlobalNodeStorage.IndexedStack<uint>[] nodeIndexPools = new GlobalNodeStorage.IndexedStack<uint>[]
		{
			new GlobalNodeStorage.IndexedStack<uint>(),
			new GlobalNodeStorage.IndexedStack<uint>(),
			new GlobalNodeStorage.IndexedStack<uint>()
		};

		// Token: 0x04000373 RID: 883
		public GlobalNodeStorage.PathfindingThreadData[] pathfindingThreadData = new GlobalNodeStorage.PathfindingThreadData[0];

		// Token: 0x04000374 RID: 884
		private GraphNode[] nodes = new GraphNode[0];

		// Token: 0x020000A1 RID: 161
		public struct PathfindingThreadData
		{
			// Token: 0x04000375 RID: 885
			public UnsafeSpan<PathNode> pathNodes;
		}

		// Token: 0x020000A2 RID: 162
		private class IndexedStack<T>
		{
			// Token: 0x1700011B RID: 283
			// (get) Token: 0x06000579 RID: 1401 RVA: 0x0001A91E File Offset: 0x00018B1E
			// (set) Token: 0x0600057A RID: 1402 RVA: 0x0001A926 File Offset: 0x00018B26
			public int Count { get; private set; }

			// Token: 0x0600057B RID: 1403 RVA: 0x0001A930 File Offset: 0x00018B30
			public void Push(T v)
			{
				if (this.Count == this.buffer.Length)
				{
					Memory.Realloc<T>(ref this.buffer, this.buffer.Length * 2);
				}
				this.buffer[this.Count] = v;
				int count = this.Count;
				this.Count = count + 1;
			}

			// Token: 0x0600057C RID: 1404 RVA: 0x0001A984 File Offset: 0x00018B84
			public void Clear()
			{
				this.Count = 0;
			}

			// Token: 0x0600057D RID: 1405 RVA: 0x0001A990 File Offset: 0x00018B90
			public T Pop()
			{
				int count = this.Count;
				this.Count = count - 1;
				return this.buffer[this.Count];
			}

			// Token: 0x0600057E RID: 1406 RVA: 0x0001A9BE File Offset: 0x00018BBE
			public void PopMany(T[] resultBuffer, int popCount)
			{
				if (popCount > this.Count)
				{
					throw new IndexOutOfRangeException();
				}
				Array.Copy(this.buffer, this.Count - popCount, resultBuffer, 0, popCount);
				this.Count -= popCount;
			}

			// Token: 0x04000376 RID: 886
			private T[] buffer = new T[4];
		}

		// Token: 0x020000A3 RID: 163
		private struct JobAllocateNodes<T> : IJob where T : GraphNode
		{
			// Token: 0x1700011C RID: 284
			// (get) Token: 0x06000580 RID: 1408 RVA: 0x0001AA07 File Offset: 0x00018C07
			public bool allowBoundsChecks
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06000581 RID: 1409 RVA: 0x0001AA0C File Offset: 0x00018C0C
			public void Execute()
			{
				HierarchicalGraph hierarchicalGraph = this.nodeStorage.astar.hierarchicalGraph;
				GlobalNodeStorage globalNodeStorage = this.nodeStorage;
				lock (globalNodeStorage)
				{
					GlobalNodeStorage.IndexedStack<uint> indexedStack = this.nodeStorage.nodeIndexPools[(int)(this.variantsPerNode - 1U)];
					uint num = this.nodeStorage.nextNodeIndex;
					uint num2 = 0U;
					while ((ulong)num2 < (ulong)((long)this.count))
					{
						T t = (this.result[(int)num2] = this.createNode());
						if (indexedStack.Count > 0)
						{
							t.NodeIndex = indexedStack.Pop();
						}
						else
						{
							t.NodeIndex = num;
							num += this.variantsPerNode;
						}
						num2 += 1U;
					}
					this.nodeStorage.ReserveNodeIndices(num);
					this.nodeStorage.nextNodeIndex = num;
					for (int i = 0; i < this.count; i++)
					{
						T t2 = this.result[i];
						hierarchicalGraph.AddDirtyNode(t2);
						this.nodeStorage.nodes[(int)t2.NodeIndex] = t2;
					}
				}
			}

			// Token: 0x04000378 RID: 888
			public T[] result;

			// Token: 0x04000379 RID: 889
			public int count;

			// Token: 0x0400037A RID: 890
			public GlobalNodeStorage nodeStorage;

			// Token: 0x0400037B RID: 891
			public uint variantsPerNode;

			// Token: 0x0400037C RID: 892
			public Func<T> createNode;
		}
	}
}
