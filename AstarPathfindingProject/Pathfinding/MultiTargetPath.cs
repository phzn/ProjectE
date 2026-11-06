using System;
using System.Collections.Generic;
using System.Text;
using Pathfinding.Pooling;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200013D RID: 317
	public class MultiTargetPath : ABPath
	{
		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000A5C RID: 2652 RVA: 0x0003780F File Offset: 0x00035A0F
		// (set) Token: 0x06000A5D RID: 2653 RVA: 0x00037817 File Offset: 0x00035A17
		public bool inverted { get; protected set; }

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000A5E RID: 2654 RVA: 0x0001AA07 File Offset: 0x00018C07
		public override bool endPointKnownBeforeCalculation
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x00037836 File Offset: 0x00035A36
		public static MultiTargetPath Construct(Vector3[] startPoints, Vector3 target, OnPathDelegate[] callbackDelegates, OnPathDelegate callback = null)
		{
			MultiTargetPath multiTargetPath = MultiTargetPath.Construct(target, startPoints, callbackDelegates, callback);
			multiTargetPath.inverted = true;
			return multiTargetPath;
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x00037848 File Offset: 0x00035A48
		public static MultiTargetPath Construct(Vector3 start, Vector3[] targets, OnPathDelegate[] callbackDelegates, OnPathDelegate callback = null)
		{
			MultiTargetPath path = PathPool.GetPath<MultiTargetPath>();
			path.Setup(start, targets, callbackDelegates, callback);
			return path;
		}

		// Token: 0x06000A62 RID: 2658 RVA: 0x0003785C File Offset: 0x00035A5C
		public override void RefreshPathEndpoints(Vector3 start, Vector3 end)
		{
			lock (this)
			{
				if (base.PipelineState == PathState.PathQueue || base.PipelineState == PathState.Created)
				{
					this.originalStartPoint = (this.startPoint = (this.inverted ? end : start));
				}
			}
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x000378C0 File Offset: 0x00035AC0
		protected void Setup(Vector3 start, Vector3[] targets, OnPathDelegate[] callbackDelegates, OnPathDelegate callback)
		{
			this.inverted = false;
			this.callback = callback;
			this.callbacks = callbackDelegates;
			if (this.callbacks != null && this.callbacks.Length != targets.Length)
			{
				throw new ArgumentException("The targets array must have the same length as the callbackDelegates array");
			}
			this.targetPoints = targets;
			this.originalStartPoint = start;
			this.startPoint = start;
			if (targets.Length == 0)
			{
				base.FailWithError("No targets were assigned to the MultiTargetPath");
				return;
			}
			this.endPoint = targets[0];
			this.originalTargetPoints = new Vector3[this.targetPoints.Length];
			for (int i = 0; i < this.targetPoints.Length; i++)
			{
				this.originalTargetPoints[i] = this.targetPoints[i];
			}
		}

		// Token: 0x06000A64 RID: 2660 RVA: 0x00037973 File Offset: 0x00035B73
		protected override void Reset()
		{
			base.Reset();
			this.pathsForAll = true;
			this.chosenTarget = -1;
			this.inverted = true;
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x00037990 File Offset: 0x00035B90
		protected override void OnEnterPool()
		{
			if (this.vectorPaths != null)
			{
				for (int i = 0; i < this.vectorPaths.Length; i++)
				{
					if (this.vectorPaths[i] != null)
					{
						ListPool<Vector3>.Release(this.vectorPaths[i]);
					}
				}
			}
			this.vectorPaths = null;
			this.vectorPath = null;
			if (this.nodePaths != null)
			{
				for (int j = 0; j < this.nodePaths.Length; j++)
				{
					if (this.nodePaths[j] != null)
					{
						ListPool<GraphNode>.Release(this.nodePaths[j]);
					}
				}
			}
			this.nodePaths = null;
			this.path = null;
			this.callbacks = null;
			this.targetNodes = null;
			this.targetsFound = null;
			this.targetPathCosts = null;
			this.targetPoints = null;
			this.originalTargetPoints = null;
			base.OnEnterPool();
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x00037A50 File Offset: 0x00035C50
		private void ChooseShortestPath()
		{
			this.chosenTarget = -1;
			if (this.nodePaths != null)
			{
				uint num = uint.MaxValue;
				for (int i = 0; i < this.nodePaths.Length; i++)
				{
					if (this.nodePaths[i] != null)
					{
						uint num2 = this.targetPathCosts[i];
						if (num2 < num)
						{
							this.chosenTarget = i;
							num = num2;
						}
					}
				}
			}
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x00037AA4 File Offset: 0x00035CA4
		private void SetPathParametersForReturn(int target)
		{
			this.path = this.nodePaths[target];
			this.vectorPath = this.vectorPaths[target];
			if (this.inverted)
			{
				this.startPoint = this.targetPoints[target];
				this.originalStartPoint = this.originalTargetPoints[target];
			}
			else
			{
				this.endPoint = this.targetPoints[target];
				this.originalEndPoint = this.originalTargetPoints[target];
			}
			this.cost = ((this.path != null) ? this.targetPathCosts[target] : 0U);
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x00037B38 File Offset: 0x00035D38
		protected override void ReturnPath()
		{
			if (base.error)
			{
				if (this.callbacks != null)
				{
					for (int i = 0; i < this.callbacks.Length; i++)
					{
						if (this.callbacks[i] != null)
						{
							this.callbacks[i](this);
						}
					}
				}
				if (this.callback != null)
				{
					this.callback(this);
				}
				return;
			}
			bool flag = false;
			if (this.inverted)
			{
				this.endPoint = this.startPoint;
				this.originalEndPoint = this.originalStartPoint;
			}
			for (int j = 0; j < this.nodePaths.Length; j++)
			{
				if (this.nodePaths[j] != null)
				{
					this.completeState = PathCompleteState.Complete;
					flag = true;
				}
				else
				{
					this.completeState = PathCompleteState.Error;
				}
				if (this.callbacks != null && this.callbacks[j] != null)
				{
					this.SetPathParametersForReturn(j);
					this.callbacks[j](this);
					this.vectorPaths[j] = this.vectorPath;
				}
			}
			if (flag)
			{
				this.completeState = PathCompleteState.Complete;
				this.SetPathParametersForReturn(this.chosenTarget);
			}
			else
			{
				this.completeState = PathCompleteState.Error;
			}
			if (this.callback != null)
			{
				this.callback(this);
			}
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x00037C50 File Offset: 0x00035E50
		protected void RebuildOpenList(ref Path.SearchContext ctx)
		{
			PathHandler pathHandler = ctx.pathHandler;
			BinaryHeap heap = pathHandler.heap;
			if (heap.tieBreaking != BinaryHeap.TieBreaking.HScore)
			{
				return;
			}
			for (int i = 0; i < heap.numberOfItems; i++)
			{
				uint pathNodeIndex = heap.GetPathNodeIndex(i);
				Int3 @int;
				if (pathHandler.IsTemporaryNode(pathNodeIndex))
				{
					@int = pathHandler.GetTemporaryNode(pathNodeIndex).position;
				}
				else
				{
					@int = pathHandler.GetNode(pathNodeIndex).DecodeVariantPosition(pathNodeIndex, pathHandler.pathNodes[pathNodeIndex].fractionAlongEdge);
				}
				uint num = (uint)ctx.heuristicObjective.Calculate((int3)@int, 0U);
				heap.SetH(i, num);
			}
			pathHandler.heap.Rebuild(pathHandler.pathNodes);
		}

		// Token: 0x06000A6A RID: 2666 RVA: 0x00037CF8 File Offset: 0x00035EF8
		protected override void Prepare(ref Path.SearchContext ctx)
		{
			NearestNodeConstraint nearestNodeConstraint = base.nearestNodeConstraint;
			NNInfo nearest = AstarPath.active.GetNearest(this.startPoint, nearestNodeConstraint);
			GraphNode node = nearest.node;
			if (this.endingCondition != null)
			{
				base.FailWithError("Multi target paths cannot use custom ending conditions");
				return;
			}
			if (node == null)
			{
				base.FailWithError("Could not find start node for multi target path");
				return;
			}
			ctx.pathHandler.AddTemporaryNode(new TemporaryNode
			{
				associatedNode = nearest.node.NodeIndex,
				position = (Int3)nearest.position,
				type = TemporaryNodeType.Start
			});
			nearestNodeConstraint.area = (int)node.Area;
			this.vectorPaths = new List<Vector3>[this.targetPoints.Length];
			this.nodePaths = new List<GraphNode>[this.targetPoints.Length];
			this.targetNodes = new GraphNode[this.targetPoints.Length];
			this.targetsFound = new bool[this.targetPoints.Length];
			this.targetPathCosts = new uint[this.targetPoints.Length];
			this.targetNodeCount = 0;
			bool flag = false;
			for (int i = 0; i < this.targetPoints.Length; i++)
			{
				Vector3 vector = this.targetPoints[i];
				NNInfo nearest2 = AstarPath.active.GetNearest(vector, nearestNodeConstraint);
				this.targetNodes[i] = nearest2.node;
				this.targetPoints[i] = nearest2.position;
				if (nearest2.node != null)
				{
					flag = true;
					this.targetNodeCount++;
					if (!this.EndPointGridGraphSpecialCase(ref ctx, nearest2.node, vector, i))
					{
						ctx.pathHandler.AddTemporaryNode(new TemporaryNode
						{
							associatedNode = nearest2.node.NodeIndex,
							position = (Int3)nearest2.position,
							targetIndex = i,
							type = TemporaryNodeType.End
						});
					}
				}
				else
				{
					this.targetsFound[i] = true;
				}
			}
			this.startPoint = nearest.position;
			if (!flag)
			{
				base.FailWithError("Couldn't find a valid node close to the any of the end points");
				return;
			}
			ctx.traversalConstraint = this.traversalConstraint;
			ctx.traversalCosts = this.traversalCosts;
			ctx.MarkNodesAdjacentToTemporaryEndNodes();
			ctx.AddStartNodesToHeap();
			this.RecalculateHTarget(ref ctx);
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x00037F2C File Offset: 0x0003612C
		private void RecalculateHTarget(ref Path.SearchContext ctx)
		{
			if (this.pathsForAll)
			{
				int3 @int = ctx.FirstTemporaryEndNode();
				ctx.heuristicObjective = new HeuristicObjective(@int, @int, this.heuristic, this.heuristicScale, 0U, null);
			}
			else
			{
				int3 int2;
				int3 int3;
				ctx.TemporaryEndNodesBoundingBox(out int2, out int3);
				ctx.heuristicObjective = new HeuristicObjective(int2, int3, this.heuristic, this.heuristicScale, 0U, null);
			}
			this.RebuildOpenList(ref ctx);
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x00037F91 File Offset: 0x00036191
		protected override void Cleanup(ref Path.SearchContext ctx)
		{
			this.ChooseShortestPath();
			base.Cleanup(ref ctx);
		}

		// Token: 0x06000A6D RID: 2669 RVA: 0x00037131 File Offset: 0x00035331
		protected override void OnHeapExhausted(ref Path.SearchContext ctx)
		{
			base.CompleteState = PathCompleteState.Complete;
		}

		// Token: 0x06000A6E RID: 2670 RVA: 0x00037FA0 File Offset: 0x000361A0
		protected override void OnFoundEndNode(ref Path.SearchContext ctx, uint pathNode, uint hScore, uint gScore)
		{
			if (!ctx.pathHandler.IsTemporaryNode(pathNode))
			{
				base.FailWithError("Expected the end node to be a temporary node. Cannot determine which path it belongs to. This could happen if you are using a custom ending condition for the path.");
				return;
			}
			int targetIndex = ctx.pathHandler.GetTemporaryNode(pathNode).targetIndex;
			if (this.targetsFound[targetIndex])
			{
				throw new ArgumentException("This target has already been found");
			}
			this.Trace(ref ctx, pathNode);
			this.vectorPaths[targetIndex] = this.vectorPath;
			this.nodePaths[targetIndex] = this.path;
			this.vectorPath = ListPool<Vector3>.Claim();
			this.path = ListPool<GraphNode>.Claim();
			this.targetsFound[targetIndex] = true;
			this.targetPathCosts[targetIndex] = gScore;
			this.targetNodeCount--;
			uint num = 0U;
			while ((ulong)num < (ulong)((long)ctx.pathHandler.numTemporaryNodes))
			{
				uint num2 = ctx.pathHandler.temporaryNodeStartIndex + num;
				ref TemporaryNode temporaryNode = ref ctx.pathHandler.GetTemporaryNode(num2);
				if (temporaryNode.type == TemporaryNodeType.End && temporaryNode.targetIndex == targetIndex)
				{
					temporaryNode.type = TemporaryNodeType.Ignore;
				}
				num += 1U;
			}
			if (!this.pathsForAll)
			{
				base.CompleteState = PathCompleteState.Complete;
				this.targetNodeCount = 0;
				return;
			}
			if (this.targetNodeCount <= 0)
			{
				base.CompleteState = PathCompleteState.Complete;
				return;
			}
			this.RecalculateHTarget(ref ctx);
		}

		// Token: 0x06000A6F RID: 2671 RVA: 0x000380C4 File Offset: 0x000362C4
		protected override void Trace(ref Path.SearchContext ctx, uint pathNodeIndex)
		{
			base.Trace(ref ctx, pathNodeIndex, !this.inverted);
		}

		// Token: 0x06000A70 RID: 2672 RVA: 0x000380D8 File Offset: 0x000362D8
		protected override void DebugString(StringBuilder text, PathLog logMode)
		{
			if (logMode == PathLog.None || (!base.error && logMode == PathLog.OnlyErrors))
			{
				return;
			}
			base.DebugStringPrefix(logMode, text);
			if (!base.error)
			{
				text.Append("\nShortest path was ");
				text.Append((this.chosenTarget == -1) ? "undefined" : this.nodePaths[this.chosenTarget].Count.ToString());
				text.Append(" nodes long");
				if (logMode == PathLog.Heavy)
				{
					text.Append("\nPaths (").Append(this.targetsFound.Length).Append("):");
					for (int i = 0; i < this.targetsFound.Length; i++)
					{
						text.Append("\n\n\tPath ").Append(i).Append(" Found: ")
							.Append(this.targetsFound[i]);
						if (this.nodePaths[i] != null)
						{
							text.Append("\n\t\tLength: ");
							text.Append(this.nodePaths[i].Count);
						}
					}
					text.Append("\nStart Node");
					text.Append("\n\tPoint: ");
					Vector3 endPoint = this.endPoint;
					text.Append(endPoint.ToString());
					text.Append("\n\tGraph: ");
					text.Append(base.startNode.GraphIndex);
				}
			}
			base.DebugStringSuffix(logMode, text);
		}

		// Token: 0x040006D5 RID: 1749
		public OnPathDelegate[] callbacks;

		// Token: 0x040006D6 RID: 1750
		public GraphNode[] targetNodes;

		// Token: 0x040006D7 RID: 1751
		protected int targetNodeCount;

		// Token: 0x040006D8 RID: 1752
		public bool[] targetsFound;

		// Token: 0x040006D9 RID: 1753
		public uint[] targetPathCosts;

		// Token: 0x040006DA RID: 1754
		public Vector3[] targetPoints;

		// Token: 0x040006DB RID: 1755
		public Vector3[] originalTargetPoints;

		// Token: 0x040006DC RID: 1756
		public List<Vector3>[] vectorPaths;

		// Token: 0x040006DD RID: 1757
		public List<GraphNode>[] nodePaths;

		// Token: 0x040006DE RID: 1758
		public bool pathsForAll = true;

		// Token: 0x040006DF RID: 1759
		public int chosenTarget = -1;
	}
}
