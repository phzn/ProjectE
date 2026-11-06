using System;
using System.Collections.Generic;
using Pathfinding.Collections;
using Pathfinding.Drawing;
using Pathfinding.Pooling;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Graphs.Util
{
	// Token: 0x0200019D RID: 413
	[Serializable]
	public class EuclideanEmbedding
	{
		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000C1B RID: 3099 RVA: 0x00042A67 File Offset: 0x00040C67
		// (set) Token: 0x06000C1C RID: 3100 RVA: 0x00042A6F File Offset: 0x00040C6F
		public NativeArray<uint> costs { get; private set; }

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000C1D RID: 3101 RVA: 0x00042A78 File Offset: 0x00040C78
		// (set) Token: 0x06000C1E RID: 3102 RVA: 0x00042A80 File Offset: 0x00040C80
		public int pivotCount { get; private set; }

		// Token: 0x06000C1F RID: 3103 RVA: 0x00042A89 File Offset: 0x00040C89
		private uint GetRandom()
		{
			this.rval = 12820163U * this.rval + 1140671485U;
			return this.rval;
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x00042AAC File Offset: 0x00040CAC
		public void OnDisable()
		{
			if (this.costs.IsCreated)
			{
				this.costs.Dispose();
			}
			this.costs = default(NativeArray<uint>);
			this.pivotCount = 0;
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x00042AF0 File Offset: 0x00040CF0
		public unsafe static uint GetHeuristic(UnsafeSpan<uint> costs, uint pivotCount, uint nodeIndex1, uint nodeIndex2)
		{
			uint num = 0U;
			if ((ulong)nodeIndex1 < (ulong)((long)costs.Length) && (ulong)nodeIndex2 < (ulong)((long)costs.Length))
			{
				for (uint num2 = 0U; num2 < pivotCount; num2 += 1U)
				{
					uint num3 = *costs[nodeIndex1 * pivotCount + num2];
					uint num4 = *costs[nodeIndex2 * pivotCount + num2];
					if (num3 != 4294967295U && num4 != 4294967295U)
					{
						uint num5 = (uint)math.abs((int)(num3 - num4));
						if (num5 > num)
						{
							num = num5;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x00042B5C File Offset: 0x00040D5C
		private void GetClosestWalkableNodesToChildrenRecursively(Transform tr, List<GraphNode> nodes)
		{
			NearestNodeConstraint walkable = NearestNodeConstraint.Walkable;
			foreach (object obj in tr)
			{
				Transform transform = (Transform)obj;
				NNInfo nearest = AstarPath.active.GetNearest(transform.position, walkable);
				if (nearest.node != null && nearest.node.Walkable)
				{
					nodes.Add(nearest.node);
				}
				this.GetClosestWalkableNodesToChildrenRecursively(transform, nodes);
			}
		}

		// Token: 0x06000C23 RID: 3107 RVA: 0x00042BF0 File Offset: 0x00040DF0
		private void PickNRandomNodes(int count, List<GraphNode> buffer)
		{
			int n = 0;
			NavGraph[] graphs = AstarPath.active.graphs;
			if (graphs == null)
			{
				return;
			}
			Action<GraphNode> <>9__0;
			for (int i = 0; i < graphs.Length; i++)
			{
				if (graphs[i] != null)
				{
					NavGraph navGraph = graphs[i];
					Action<GraphNode> action;
					if ((action = <>9__0) == null)
					{
						action = (<>9__0 = delegate(GraphNode node)
						{
							if (!node.Destroyed && node.Walkable)
							{
								int j = n;
								n = j + 1;
								if ((ulong)this.GetRandom() % (ulong)((long)n) < (ulong)((long)count))
								{
									if (buffer.Count < count)
									{
										buffer.Add(node);
										return;
									}
									buffer[n % buffer.Count] = node;
								}
							}
						});
					}
					navGraph.GetNodes(action);
				}
			}
		}

		// Token: 0x06000C24 RID: 3108 RVA: 0x00042C68 File Offset: 0x00040E68
		private GraphNode PickAnyWalkableNode()
		{
			NavGraph[] graphs = AstarPath.active.graphs;
			GraphNode graphNode = null;
			for (int i = 0; i < graphs.Length; i++)
			{
				if (graphs[i] != null)
				{
					graphs[i].GetNodes<GraphNode>(delegate(GraphNode node, ref GraphNode first)
					{
						if (node != null && node.Walkable && first == null)
						{
							first = node;
						}
					}, ref graphNode);
				}
			}
			return graphNode;
		}

		// Token: 0x06000C25 RID: 3109 RVA: 0x00042CC0 File Offset: 0x00040EC0
		public void RecalculatePivots()
		{
			if (this.mode == HeuristicOptimizationMode.None)
			{
				this.pivotCount = 0;
				this.pivots = null;
				return;
			}
			this.rval = (uint)this.seed;
			List<GraphNode> list = ListPool<GraphNode>.Claim();
			switch (this.mode)
			{
			case HeuristicOptimizationMode.Random:
				this.PickNRandomNodes(this.spreadOutCount, list);
				break;
			case HeuristicOptimizationMode.RandomSpreadOut:
			{
				if (this.pivotPointRoot != null)
				{
					this.GetClosestWalkableNodesToChildrenRecursively(this.pivotPointRoot, list);
				}
				if (list.Count == 0)
				{
					GraphNode graphNode = this.PickAnyWalkableNode();
					if (graphNode == null)
					{
						ListPool<GraphNode>.Release(ref list);
						this.pivots = new GraphNode[0];
						return;
					}
					list.Add(graphNode);
				}
				int num = this.spreadOutCount - list.Count;
				for (int i = 0; i < num; i++)
				{
					list.Add(null);
				}
				break;
			}
			case HeuristicOptimizationMode.Custom:
				if (this.pivotPointRoot == null)
				{
					throw new Exception("heuristicOptimizationMode is HeuristicOptimizationMode.Custom, but no 'customHeuristicOptimizationPivotsRoot' is set");
				}
				this.GetClosestWalkableNodesToChildrenRecursively(this.pivotPointRoot, list);
				break;
			default:
				throw new Exception("Invalid HeuristicOptimizationMode: " + this.mode.ToString());
			}
			this.pivots = list.ToArray();
			ListPool<GraphNode>.Release(ref list);
		}

		// Token: 0x06000C26 RID: 3110 RVA: 0x00042DF8 File Offset: 0x00040FF8
		public void RecalculateCosts()
		{
			if (this.pivots == null)
			{
				this.RecalculatePivots();
			}
			if (this.mode == HeuristicOptimizationMode.None)
			{
				return;
			}
			this.RecalculateCostsInner();
		}

		// Token: 0x06000C27 RID: 3111 RVA: 0x00042E18 File Offset: 0x00041018
		private void RecalculateCostsInner()
		{
			this.pivotCount = 0;
			for (int i = 0; i < this.pivots.Length; i++)
			{
				if (this.pivots[i] != null && (this.pivots[i].Destroyed || !this.pivots[i].Walkable))
				{
					throw new Exception("Invalid pivot nodes (destroyed or unwalkable)");
				}
			}
			if (this.mode != HeuristicOptimizationMode.RandomSpreadOut)
			{
				for (int j = 0; j < this.pivots.Length; j++)
				{
					if (this.pivots[j] == null)
					{
						throw new Exception("Invalid pivot nodes (null)");
					}
				}
			}
			this.pivotCount = this.pivots.Length;
			Action<int> startCostCalculation = null;
			int numComplete = 0;
			uint nextNodeIndex = AstarPath.active.nodeStorage.nextNodeIndex;
			if (this.costs.IsCreated)
			{
				this.costs.Dispose();
			}
			this.costs = new NativeArray<uint>((int)(nextNodeIndex * (uint)this.pivotCount), Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.costs.AsUnsafeSpan<uint>().Fill(uint.MaxValue);
			startCostCalculation = delegate(int pivotIndex)
			{
				GraphNode graphNode = this.pivots[pivotIndex];
				EuclideanEmbedding.EuclideanEmbeddingSearchPath path = EuclideanEmbedding.EuclideanEmbeddingSearchPath.Construct(this.costs.AsUnsafeSpan<uint>(), (uint)this.pivotCount, (uint)pivotIndex, graphNode);
				path.immediateCallback = delegate(Path _)
				{
					if (this.mode == HeuristicOptimizationMode.RandomSpreadOut && pivotIndex < this.pivots.Length - 1)
					{
						if (this.pivots[pivotIndex + 1] == null)
						{
							this.pivots[pivotIndex + 1] = path.furthestNode;
							if (path.furthestNode == null)
							{
								Debug.LogError("Failed generating random pivot points for heuristic optimizations");
								return;
							}
						}
						startCostCalculation(pivotIndex + 1);
					}
					int numComplete2 = numComplete;
					numComplete = numComplete2 + 1;
					if (numComplete == this.pivotCount)
					{
						this.ApplyGridGraphEndpointSpecialCase();
					}
				};
				AstarPath.StartPath(path, true, true);
			};
			if (this.mode != HeuristicOptimizationMode.RandomSpreadOut)
			{
				for (int k = 0; k < this.pivots.Length; k++)
				{
					startCostCalculation(k);
				}
			}
			else if (this.pivots.Length != 0)
			{
				startCostCalculation(0);
			}
			this.dirty = false;
		}

		// Token: 0x06000C28 RID: 3112 RVA: 0x00042F80 File Offset: 0x00041180
		private unsafe void ApplyGridGraphEndpointSpecialCase()
		{
			UnsafeSpan<uint> unsafeSpan = this.costs.AsUnsafeSpan<uint>();
			NavGraph[] graphs = AstarPath.active.graphs;
			for (int i = 0; i < graphs.Length; i++)
			{
				GridGraph gridGraph = graphs[i] as GridGraph;
				if (gridGraph != null)
				{
					GridNodeBase[] nodes = gridGraph.nodes;
					int num = ((gridGraph.neighbours == NumNeighbours.Four) ? 4 : ((gridGraph.neighbours == NumNeighbours.Eight) ? 8 : 6));
					for (int j = 0; j < gridGraph.depth; j++)
					{
						for (int k = 0; k < gridGraph.width; k++)
						{
							GridNodeBase gridNodeBase = nodes[j * gridGraph.width + k];
							if (!gridNodeBase.Walkable)
							{
								uint num2 = gridNodeBase.NodeIndex * (uint)this.pivotCount;
								for (int l = 0; l < this.pivotCount; l++)
								{
									*unsafeSpan[num2 + (uint)l] = uint.MaxValue;
								}
								for (int m = 0; m < num; m++)
								{
									int num3;
									int num4;
									if (gridGraph.neighbours == NumNeighbours.Six)
									{
										num3 = k + GridGraph.neighbourXOffsets[GridGraph.hexagonNeighbourIndices[m]];
										num4 = j + GridGraph.neighbourZOffsets[GridGraph.hexagonNeighbourIndices[m]];
									}
									else
									{
										num3 = k + GridGraph.neighbourXOffsets[m];
										num4 = j + GridGraph.neighbourZOffsets[m];
									}
									if (num3 >= 0 && num4 >= 0 && num3 < gridGraph.width && num4 < gridGraph.depth)
									{
										GridNodeBase gridNodeBase2 = gridGraph.nodes[num4 * gridGraph.width + num3];
										if (gridNodeBase2.Walkable)
										{
											uint num5 = 0U;
											while ((ulong)num5 < (ulong)((long)this.pivotCount))
											{
												uint num6 = *unsafeSpan[gridNodeBase2.NodeIndex * (uint)this.pivotCount + num5] + (uint)gridGraph.neighbourCosts[m];
												*unsafeSpan[num2 + num5] = Math.Min(*unsafeSpan[num2 + num5], num6);
												num5 += 1U;
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000C29 RID: 3113 RVA: 0x00043174 File Offset: 0x00041374
		public void OnDrawGizmos()
		{
			if (this.pivots != null)
			{
				for (int i = 0; i < this.pivots.Length; i++)
				{
					if (this.pivots[i] != null && !this.pivots[i].Destroyed)
					{
						Draw.SolidBox((Vector3)this.pivots[i].position, Vector3.one, new Color(0.62352943f, 0.36862746f, 0.7607843f, 0.8f));
					}
				}
			}
		}

		// Token: 0x040007FC RID: 2044
		public HeuristicOptimizationMode mode;

		// Token: 0x040007FD RID: 2045
		public int seed;

		// Token: 0x040007FE RID: 2046
		public Transform pivotPointRoot;

		// Token: 0x040007FF RID: 2047
		public int spreadOutCount = 10;

		// Token: 0x04000800 RID: 2048
		[NonSerialized]
		public bool dirty;

		// Token: 0x04000803 RID: 2051
		private GraphNode[] pivots;

		// Token: 0x04000804 RID: 2052
		private const uint ra = 12820163U;

		// Token: 0x04000805 RID: 2053
		private const uint rc = 1140671485U;

		// Token: 0x04000806 RID: 2054
		private uint rval;

		// Token: 0x0200019E RID: 414
		private class EuclideanEmbeddingSearchPath : Path
		{
			// Token: 0x06000C2B RID: 3115 RVA: 0x00043204 File Offset: 0x00041404
			public static EuclideanEmbedding.EuclideanEmbeddingSearchPath Construct(UnsafeSpan<uint> costs, uint costIndexStride, uint pivotIndex, GraphNode startNode)
			{
				EuclideanEmbedding.EuclideanEmbeddingSearchPath path = PathPool.GetPath<EuclideanEmbedding.EuclideanEmbeddingSearchPath>();
				path.costs = costs;
				path.costIndexStride = costIndexStride;
				path.pivotIndex = pivotIndex;
				path.startNode = startNode;
				path.furthestNodeScore = 0U;
				path.furthestNode = null;
				return path;
			}

			// Token: 0x06000C2C RID: 3116 RVA: 0x00043235 File Offset: 0x00041435
			protected override void OnFoundEndNode(ref Path.SearchContext ctx, uint pathNode, uint hScore, uint gScore)
			{
				throw new InvalidOperationException();
			}

			// Token: 0x06000C2D RID: 3117 RVA: 0x00037131 File Offset: 0x00035331
			protected override void OnHeapExhausted(ref Path.SearchContext ctx)
			{
				base.CompleteState = PathCompleteState.Complete;
			}

			// Token: 0x06000C2E RID: 3118 RVA: 0x0004323C File Offset: 0x0004143C
			public unsafe override void OnVisitNode(ref Path.SearchContext ctx, uint pathNode, uint hScore, uint gScore)
			{
				PathHandler pathHandler = ctx.pathHandler;
				if (!pathHandler.IsTemporaryNode(pathNode))
				{
					GraphNode node = pathHandler.GetNode(pathNode);
					uint num = node.NodeIndex * this.costIndexStride;
					*this.costs[num + this.pivotIndex] = math.min(*this.costs[num + this.pivotIndex], gScore);
					uint num2 = uint.MaxValue;
					int num3 = 0;
					while ((long)num3 <= (long)((ulong)this.pivotIndex))
					{
						num2 = math.min(num2, *this.costs[num + (uint)num3]);
						num3++;
					}
					if (num2 > this.furthestNodeScore || this.furthestNode == null)
					{
						this.furthestNodeScore = num2;
						this.furthestNode = node;
					}
				}
			}

			// Token: 0x06000C2F RID: 3119 RVA: 0x000432F0 File Offset: 0x000414F0
			protected override void Prepare(ref Path.SearchContext ctx)
			{
				ctx.pathHandler.AddTemporaryNode(new TemporaryNode
				{
					associatedNode = this.startNode.NodeIndex,
					position = this.startNode.position,
					type = TemporaryNodeType.Start
				});
				ctx.heuristicObjective = new HeuristicObjective(0, Heuristic.None, 0f);
				ctx.traversalConstraint = this.traversalConstraint;
				ctx.traversalCosts = this.traversalCosts;
				ctx.MarkNodesAdjacentToTemporaryEndNodes();
				ctx.AddStartNodesToHeap();
			}

			// Token: 0x04000807 RID: 2055
			public UnsafeSpan<uint> costs;

			// Token: 0x04000808 RID: 2056
			public uint costIndexStride;

			// Token: 0x04000809 RID: 2057
			public uint pivotIndex;

			// Token: 0x0400080A RID: 2058
			public GraphNode startNode;

			// Token: 0x0400080B RID: 2059
			public uint furthestNodeScore;

			// Token: 0x0400080C RID: 2060
			public GraphNode furthestNode;
		}
	}
}
