using System;
using System.Collections.Generic;
using Pathfinding.Pooling;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200013A RID: 314
	public class FloodPath : Path
	{
		// Token: 0x06000A44 RID: 2628 RVA: 0x00037284 File Offset: 0x00035484
		public bool HasPathTo(GraphNode node)
		{
			if (this.parents != null)
			{
				uint num = 0U;
				while ((ulong)num < (ulong)((long)node.PathNodeVariants))
				{
					if (this.parents.ContainsKey(node.NodeIndex + num))
					{
						return true;
					}
					num += 1U;
				}
			}
			return false;
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x000372C4 File Offset: 0x000354C4
		internal bool IsValid(GlobalNodeStorage nodeStorage)
		{
			return nodeStorage.destroyedNodesVersion == this.validationHash;
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x000372D4 File Offset: 0x000354D4
		public uint GetParent(uint node)
		{
			uint num;
			if (!this.parents.TryGetValue(node, out num))
			{
				return 0U;
			}
			return num;
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x00037303 File Offset: 0x00035503
		public static FloodPath Construct(Vector3 start, OnPathDelegate callback = null)
		{
			FloodPath path = PathPool.GetPath<FloodPath>();
			path.Setup(start, callback);
			return path;
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x00037312 File Offset: 0x00035512
		public static FloodPath Construct(GraphNode start, OnPathDelegate callback = null)
		{
			if (start == null)
			{
				throw new ArgumentNullException("start");
			}
			FloodPath path = PathPool.GetPath<FloodPath>();
			path.Setup(start, callback);
			return path;
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x0003732F File Offset: 0x0003552F
		protected void Setup(Vector3 start, OnPathDelegate callback)
		{
			this.callback = callback;
			this.originalStartPoint = start;
			this.startPoint = start;
			this.heuristic = Heuristic.None;
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x0003734D File Offset: 0x0003554D
		protected void Setup(GraphNode start, OnPathDelegate callback)
		{
			this.callback = callback;
			this.originalStartPoint = (Vector3)start.position;
			this.startNode = start;
			this.startPoint = (Vector3)start.position;
			this.heuristic = Heuristic.None;
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x00037386 File Offset: 0x00035586
		protected override void Reset()
		{
			base.Reset();
			this.originalStartPoint = Vector3.zero;
			this.startPoint = Vector3.zero;
			this.startNode = null;
			this.parents = new Dictionary<uint, uint>();
			this.saveParents = true;
			this.validationHash = 0U;
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x000373C4 File Offset: 0x000355C4
		protected override void Prepare(ref Path.SearchContext ctx)
		{
			if (this.startNode == null)
			{
				NNInfo nearest = AstarPath.active.GetNearest(this.originalStartPoint, base.nearestNodeConstraint);
				this.startPoint = nearest.position;
				this.startNode = nearest.node;
			}
			else
			{
				if (this.startNode.Destroyed)
				{
					base.FailWithError("Start node has been destroyed");
					return;
				}
				this.startPoint = (Vector3)this.startNode.position;
			}
			if (this.startNode == null)
			{
				base.FailWithError("Couldn't find a close node to the start point");
				return;
			}
			ctx.pathHandler.AddTemporaryNode(new TemporaryNode
			{
				type = TemporaryNodeType.Start,
				position = (Int3)this.startPoint,
				associatedNode = this.startNode.NodeIndex
			});
			ctx.heuristicObjective = new HeuristicObjective(int3.zero, Heuristic.None, 0f);
			ctx.traversalConstraint = this.traversalConstraint;
			ctx.traversalCosts = this.traversalCosts;
			ctx.AddStartNodesToHeap();
			this.validationHash = ctx.pathHandler.nodeStorage.destroyedNodesVersion;
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x00037131 File Offset: 0x00035331
		protected override void OnHeapExhausted(ref Path.SearchContext ctx)
		{
			base.CompleteState = PathCompleteState.Complete;
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x000374D8 File Offset: 0x000356D8
		protected override void OnFoundEndNode(ref Path.SearchContext ctx, uint pathNode, uint hScore, uint gScore)
		{
			throw new InvalidOperationException("FloodPaths do not have any end nodes");
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x000374E4 File Offset: 0x000356E4
		public override void OnVisitNode(ref Path.SearchContext ctx, uint pathNode, uint hScore, uint gScore)
		{
			if (this.saveParents)
			{
				uint parentIndex = ctx.pathHandler.pathNodes[pathNode].parentIndex;
				this.parents[pathNode] = parentIndex | (ctx.pathHandler.IsTemporaryNode(parentIndex) ? 2147483648U : 0U);
			}
		}

		// Token: 0x040006CB RID: 1739
		public Vector3 originalStartPoint;

		// Token: 0x040006CC RID: 1740
		public Vector3 startPoint;

		// Token: 0x040006CD RID: 1741
		public GraphNode startNode;

		// Token: 0x040006CE RID: 1742
		public bool saveParents = true;

		// Token: 0x040006CF RID: 1743
		protected Dictionary<uint, uint> parents;

		// Token: 0x040006D0 RID: 1744
		private uint validationHash;

		// Token: 0x040006D1 RID: 1745
		public const uint TemporaryNodeBit = 2147483648U;
	}
}
