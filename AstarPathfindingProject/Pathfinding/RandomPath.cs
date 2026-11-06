using System;
using Pathfinding.Pooling;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200013E RID: 318
	public class RandomPath : ABPath
	{
		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000A71 RID: 2673 RVA: 0x0001AA07 File Offset: 0x00018C07
		protected override bool hasEndPoint
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000A72 RID: 2674 RVA: 0x0001AA07 File Offset: 0x00018C07
		public override bool endPointKnownBeforeCalculation
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x0003823C File Offset: 0x0003643C
		protected override void Reset()
		{
			base.Reset();
			this.searchLength = 5000;
			this.spread = 5000;
			this.aimStrength = 0f;
			this.chosenPathNodeIndex = uint.MaxValue;
			this.maxGScorePathNodeIndex = uint.MaxValue;
			this.chosenPathNodeGScore = 0U;
			this.maxGScore = 0U;
			this.aim = Vector3.zero;
			this.nodesEvaluatedRep = 0;
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x000382BC File Offset: 0x000364BC
		public static RandomPath Construct(Vector3 start, int length, OnPathDelegate callback = null)
		{
			RandomPath path = PathPool.GetPath<RandomPath>();
			path.Setup(start, length, callback);
			return path;
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x000382CD File Offset: 0x000364CD
		protected RandomPath Setup(Vector3 start, int length, OnPathDelegate callback)
		{
			this.callback = callback;
			this.searchLength = length;
			this.originalStartPoint = start;
			this.originalEndPoint = Vector3.zero;
			this.startPoint = start;
			this.endPoint = Vector3.zero;
			return this;
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x00038304 File Offset: 0x00036504
		public override void RefreshPathEndpoints(Vector3 start, Vector3 end)
		{
			lock (this)
			{
				if (base.PipelineState == PathState.PathQueue || base.PipelineState == PathState.Created)
				{
					this.startPoint = start;
					this.originalStartPoint = start;
				}
			}
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x0003835C File Offset: 0x0003655C
		protected override void ReturnPath()
		{
			if (this.path != null && this.path.Count > 0)
			{
				this.originalEndPoint = this.endPoint;
			}
			if (this.callback != null)
			{
				this.callback(this);
			}
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x00038394 File Offset: 0x00036594
		protected override void Prepare(ref Path.SearchContext ctx)
		{
			NNInfo nearest = AstarPath.active.GetNearest(this.startPoint, base.nearestNodeConstraint);
			this.startPoint = nearest.position;
			this.endPoint = this.startPoint;
			if (nearest.node == null)
			{
				base.FailWithError("Couldn't find close nodes to the start point");
				return;
			}
			this.heuristicScale = this.aimStrength;
			ctx.pathHandler.AddTemporaryNode(new TemporaryNode
			{
				type = TemporaryNodeType.Start,
				position = (Int3)nearest.position,
				associatedNode = nearest.node.NodeIndex
			});
			ctx.heuristicObjective = new HeuristicObjective((int3)((Int3)this.aim), this.heuristic, this.heuristicScale);
			ctx.traversalConstraint = this.traversalConstraint;
			ctx.traversalCosts = this.traversalCosts;
			ctx.AddStartNodesToHeap();
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x00038478 File Offset: 0x00036678
		protected override void OnHeapExhausted(ref Path.SearchContext ctx)
		{
			if (this.chosenPathNodeIndex == 4294967295U && this.maxGScorePathNodeIndex != 4294967295U)
			{
				this.chosenPathNodeIndex = this.maxGScorePathNodeIndex;
				this.chosenPathNodeGScore = this.maxGScore;
			}
			if (this.chosenPathNodeIndex != 4294967295U)
			{
				this.OnFoundEndNode(ref ctx, this.chosenPathNodeIndex, 0U, this.chosenPathNodeGScore);
				return;
			}
			base.FailWithError("Not a single node found to search");
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x000384D8 File Offset: 0x000366D8
		protected override void OnFoundEndNode(ref Path.SearchContext ctx, uint pathNode, uint hScore, uint gScore)
		{
			if (ctx.pathHandler.IsTemporaryNode(pathNode))
			{
				base.OnFoundEndNode(ref ctx, pathNode, hScore, gScore);
				return;
			}
			GraphNode node = ctx.pathHandler.GetNode(pathNode);
			this.endPoint = node.RandomPointOnSurface();
			this.cost = gScore;
			base.CompleteState = PathCompleteState.Complete;
			this.Trace(ref ctx, pathNode);
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x00038530 File Offset: 0x00036730
		public override void OnVisitNode(ref Path.SearchContext ctx, uint pathNode, uint hScore, uint gScore)
		{
			if (base.CompleteState != PathCompleteState.NotCalculated)
			{
				return;
			}
			if ((ulong)gScore >= (ulong)((long)this.searchLength))
			{
				if ((ulong)gScore > (ulong)((long)(this.searchLength + this.spread)))
				{
					if (this.chosenPathNodeIndex == 4294967295U)
					{
						this.chosenPathNodeIndex = pathNode;
						this.chosenPathNodeGScore = gScore;
					}
					this.OnFoundEndNode(ref ctx, this.chosenPathNodeIndex, 0U, this.chosenPathNodeGScore);
					return;
				}
				this.nodesEvaluatedRep++;
				if (this.rnd.NextDouble() <= (double)(1f / (float)this.nodesEvaluatedRep))
				{
					this.chosenPathNodeIndex = pathNode;
					this.chosenPathNodeGScore = gScore;
					return;
				}
			}
			else if (gScore > this.maxGScore)
			{
				this.maxGScore = gScore;
				this.maxGScorePathNodeIndex = pathNode;
			}
		}

		// Token: 0x040006E1 RID: 1761
		public int searchLength;

		// Token: 0x040006E2 RID: 1762
		public int spread = 5000;

		// Token: 0x040006E3 RID: 1763
		public float aimStrength;

		// Token: 0x040006E4 RID: 1764
		private uint chosenPathNodeIndex;

		// Token: 0x040006E5 RID: 1765
		private uint chosenPathNodeGScore;

		// Token: 0x040006E6 RID: 1766
		private uint maxGScorePathNodeIndex;

		// Token: 0x040006E7 RID: 1767
		private uint maxGScore;

		// Token: 0x040006E8 RID: 1768
		public Vector3 aim;

		// Token: 0x040006E9 RID: 1769
		private int nodesEvaluatedRep;

		// Token: 0x040006EA RID: 1770
		private readonly global::System.Random rnd = new global::System.Random();
	}
}
