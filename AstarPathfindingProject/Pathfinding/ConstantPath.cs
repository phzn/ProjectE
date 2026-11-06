using System;
using System.Collections.Generic;
using Pathfinding.Pooling;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000137 RID: 311
	public class ConstantPath : Path
	{
		// Token: 0x06000A35 RID: 2613 RVA: 0x00036FE4 File Offset: 0x000351E4
		public static ConstantPath Construct(Vector3 start, int maxGScore, OnPathDelegate callback = null)
		{
			ConstantPath path = PathPool.GetPath<ConstantPath>();
			path.Setup(start, maxGScore, callback);
			return path;
		}

		// Token: 0x06000A36 RID: 2614 RVA: 0x00036FF4 File Offset: 0x000351F4
		protected void Setup(Vector3 start, int maxGScore, OnPathDelegate callback)
		{
			this.callback = callback;
			this.startPoint = start;
			this.originalStartPoint = this.startPoint;
			this.endingCondition = new EndingConditionDistance(this, maxGScore);
		}

		// Token: 0x06000A37 RID: 2615 RVA: 0x0003701D File Offset: 0x0003521D
		protected override void OnEnterPool()
		{
			base.OnEnterPool();
			if (this.allNodes != null)
			{
				ListPool<GraphNode>.Release(ref this.allNodes);
			}
		}

		// Token: 0x06000A38 RID: 2616 RVA: 0x00037038 File Offset: 0x00035238
		protected override void Reset()
		{
			base.Reset();
			this.allNodes = ListPool<GraphNode>.Claim();
			this.endingCondition = null;
			this.originalStartPoint = Vector3.zero;
			this.startPoint = Vector3.zero;
			this.startNode = null;
			this.heuristic = Heuristic.None;
		}

		// Token: 0x06000A39 RID: 2617 RVA: 0x00037078 File Offset: 0x00035278
		protected override void Prepare(ref Path.SearchContext ctx)
		{
			NNInfo nearest = AstarPath.active.GetNearest(this.startPoint, base.nearestNodeConstraint);
			this.startNode = nearest.node;
			if (this.startNode == null)
			{
				base.FailWithError("Could not find close node to the start point");
				return;
			}
			ctx.pathHandler.AddTemporaryNode(new TemporaryNode
			{
				type = TemporaryNodeType.Start,
				position = (Int3)nearest.position,
				associatedNode = this.startNode.NodeIndex
			});
			ctx.heuristicObjective = new HeuristicObjective(int3.zero, Heuristic.None, 0f);
			ctx.traversalConstraint = this.traversalConstraint;
			ctx.traversalCosts = this.traversalCosts;
			ctx.AddStartNodesToHeap();
		}

		// Token: 0x06000A3A RID: 2618 RVA: 0x00037131 File Offset: 0x00035331
		protected override void OnHeapExhausted(ref Path.SearchContext ctx)
		{
			base.CompleteState = PathCompleteState.Complete;
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x0003713A File Offset: 0x0003533A
		protected override void OnFoundEndNode(ref Path.SearchContext ctx, uint pathNode, uint hScore, uint gScore)
		{
			throw new InvalidOperationException("ConstantPaths do not have any end nodes");
		}

		// Token: 0x06000A3C RID: 2620 RVA: 0x00037148 File Offset: 0x00035348
		public override void OnVisitNode(ref Path.SearchContext ctx, uint pathNode, uint hScore, uint gScore)
		{
			GraphNode node = ctx.pathHandler.GetNode(pathNode);
			if (this.endingCondition.TargetFound(node, hScore, gScore))
			{
				base.CompleteState = PathCompleteState.Complete;
				return;
			}
			if (!ctx.pathHandler.pathNodes[node.NodeIndex].flag2)
			{
				ctx.pathHandler.pathNodes[node.NodeIndex].flag2 = true;
				this.allNodes.Add(node);
			}
		}

		// Token: 0x06000A3D RID: 2621 RVA: 0x000371C0 File Offset: 0x000353C0
		protected override void Cleanup(ref Path.SearchContext ctx)
		{
			base.Cleanup(ref ctx);
			for (int i = 0; i < this.allNodes.Count; i++)
			{
				ctx.pathHandler.pathNodes[this.allNodes[i].NodeIndex].flag2 = false;
			}
		}

		// Token: 0x040006C5 RID: 1733
		public GraphNode startNode;

		// Token: 0x040006C6 RID: 1734
		public Vector3 startPoint;

		// Token: 0x040006C7 RID: 1735
		public Vector3 originalStartPoint;

		// Token: 0x040006C8 RID: 1736
		public List<GraphNode> allNodes;

		// Token: 0x040006C9 RID: 1737
		public PathEndingCondition endingCondition;
	}
}
