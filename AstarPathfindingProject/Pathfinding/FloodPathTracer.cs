using System;
using Pathfinding.Pooling;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200013B RID: 315
	public class FloodPathTracer : ABPath
	{
		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000A51 RID: 2641 RVA: 0x0001AA07 File Offset: 0x00018C07
		protected override bool hasEndPoint
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x0003753C File Offset: 0x0003573C
		public static FloodPathTracer Construct(Vector3 start, FloodPath flood, OnPathDelegate callback = null)
		{
			FloodPathTracer path = PathPool.GetPath<FloodPathTracer>();
			path.Setup(start, flood, callback);
			return path;
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x0003754C File Offset: 0x0003574C
		protected void Setup(Vector3 start, FloodPath flood, OnPathDelegate callback)
		{
			this.flood = flood;
			this.traversalConstraint = flood.traversalConstraint;
			if (flood == null || flood.PipelineState < PathState.Returning)
			{
				throw new ArgumentException("You must supply a calculated FloodPath to the 'flood' argument");
			}
			base.Setup(start, flood.originalStartPoint, callback);
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x00037588 File Offset: 0x00035788
		public override void RefreshPathEndpoints(Vector3 start, Vector3 end)
		{
			lock (this)
			{
				if (base.PipelineState == PathState.PathQueue || base.PipelineState == PathState.Created)
				{
					this.endPoint = end;
					this.originalEndPoint = end;
				}
			}
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x000375E0 File Offset: 0x000357E0
		protected override void Reset()
		{
			base.Reset();
			this.flood = null;
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x000375F0 File Offset: 0x000357F0
		protected override void Prepare(ref Path.SearchContext ctx)
		{
			PathHandler pathHandler = ctx.pathHandler;
			if (!this.flood.IsValid(pathHandler.nodeStorage))
			{
				base.FailWithError("The flood path is invalid because nodes have been destroyed since it was calculated. Please recalculate the flood path.");
				return;
			}
			NearestNodeConstraint nearestNodeConstraint = base.nearestNodeConstraint;
			NearestNodeConstraint origConstraint = nearestNodeConstraint;
			nearestNodeConstraint.filter = (GraphNode node) => this.flood.HasPathTo(node) && origConstraint.Suitable(node);
			base.FindStartAndEndNodes(ref ctx, ref nearestNodeConstraint);
			uint num = 0U;
			while ((ulong)num < (ulong)((long)pathHandler.numTemporaryNodes))
			{
				uint num2 = pathHandler.temporaryNodeStartIndex + num;
				ref TemporaryNode temporaryNode = ref pathHandler.GetTemporaryNode(num2);
				if (temporaryNode.type == TemporaryNodeType.Start)
				{
					GraphNode node2 = pathHandler.GetNode(temporaryNode.associatedNode);
					bool flag = false;
					uint num3 = 0U;
					while ((ulong)num3 < (ulong)((long)node2.PathNodeVariants))
					{
						if (this.flood.GetParent(node2.NodeIndex + num3) != 0U)
						{
							flag = true;
							this.OnFoundEndNode(ref ctx, node2.NodeIndex + num3, 0U, 0U);
							break;
						}
						num3 += 1U;
					}
					if (!flag)
					{
						base.FailWithError("The flood path did not contain any information about the end node. This should not happen.");
					}
					return;
				}
				num += 1U;
			}
			base.FailWithError("Could not find a valid start node");
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x00037704 File Offset: 0x00035904
		protected override void CalculateStep(ref Path.SearchContext ctx, long targetTick)
		{
			if (base.CompleteState != PathCompleteState.Complete)
			{
				throw new Exception("Something went wrong. At this point the path should be completed");
			}
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x0003771C File Offset: 0x0003591C
		protected override void Trace(ref Path.SearchContext ctx, uint fromPathNodeIndex)
		{
			uint num = fromPathNodeIndex;
			int num2 = 0;
			GraphNode graphNode = null;
			PathHandler pathHandler = ctx.pathHandler;
			while (num != 0U)
			{
				if ((num & 2147483648U) != 0U)
				{
					num = this.flood.GetParent(num & 2147483647U);
				}
				else
				{
					GraphNode node = pathHandler.GetNode(num);
					if (node == null)
					{
						base.FailWithError("A node in the path has been destroyed. The FloodPath needs to be recalculated before you can use a FloodPathTracer.");
						return;
					}
					if (node != graphNode)
					{
						if (!this.traversalConstraint.CanTraverse(node))
						{
							base.FailWithError("A node in the path is no longer walkable. The FloodPath needs to be recalculated before you can use a FloodPathTracer.");
							return;
						}
						this.path.Add(node);
						graphNode = node;
						this.vectorPath.Add((Vector3)node.position);
					}
					uint parent = this.flood.GetParent(num);
					if (parent == num)
					{
						break;
					}
					num = parent;
				}
				num2++;
				if (num2 > 10000)
				{
					Debug.LogWarning("Infinite loop? >10000 node path. Remove this message if you really have that long paths");
					return;
				}
			}
		}

		// Token: 0x040006D2 RID: 1746
		protected FloodPath flood;
	}
}
