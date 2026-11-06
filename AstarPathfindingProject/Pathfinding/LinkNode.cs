using System;

namespace Pathfinding
{
	// Token: 0x020000DB RID: 219
	public class LinkNode : PointNode
	{
		// Token: 0x0600074E RID: 1870 RVA: 0x00025566 File Offset: 0x00023766
		public LinkNode()
		{
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x0002556E File Offset: 0x0002376E
		public LinkNode(AstarPath active)
			: base(active)
		{
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x00025577 File Offset: 0x00023777
		public override void RemovePartialConnection(GraphNode node)
		{
			this.linkConcrete.staleConnections = true;
			AstarPath.active.offMeshLinks.DirtyNoSchedule(this.linkSource);
			base.RemovePartialConnection(node);
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x000255A4 File Offset: 0x000237A4
		public unsafe override void Open(ref Path.SearchContext ctx, uint pathNodeIndex, uint gScore)
		{
			if (this.connections == null)
			{
				return;
			}
			PathHandler pathHandler = ctx.pathHandler;
			PathNode pathNode = *pathHandler.pathNodes[pathNodeIndex];
			if (!pathHandler.IsTemporaryNode(pathNode.parentIndex) && pathHandler.GetNode(pathNode.parentIndex).GraphIndex == base.GraphIndex)
			{
				for (int i = 0; i < this.connections.Length; i++)
				{
					GraphNode node = this.connections[i].node;
					if (node.GraphIndex != base.GraphIndex && ctx.traversalConstraint.CanTraverse(this, node))
					{
						uint connectionCost = ctx.traversalCosts.GetConnectionCost(this, node);
						node.OpenAtPoint(ref ctx, pathNodeIndex, this.position, gScore + connectionCost);
					}
				}
				return;
			}
			float traversalCostMultiplier = ctx.traversalCosts.GetTraversalCostMultiplier(this);
			for (int j = 0; j < this.connections.Length; j++)
			{
				GraphNode node2 = this.connections[j].node;
				if (node2.GraphIndex == base.GraphIndex && ctx.traversalConstraint.CanTraverse(this, node2))
				{
					uint num = ctx.traversalCosts.GetConnectionCost(this, node2);
					num += (uint)Math.Round((double)(this.connections[j].cost * 0.5f * (traversalCostMultiplier + ctx.traversalCosts.GetTraversalCostMultiplier(node2))));
					ctx.OpenCandidateConnection(pathNodeIndex, node2.NodeIndex, gScore + num, 0U, node2.position);
				}
			}
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x00025724 File Offset: 0x00023924
		public override void OpenAtPoint(ref Path.SearchContext ctx, uint pathNodeIndex, Int3 pos, uint gScore)
		{
			uint num = (uint)Math.Round((double)((pos - this.position).magnitude * ctx.traversalCosts.GetTraversalCostMultiplier(this)));
			ctx.OpenCandidateConnection(pathNodeIndex, base.NodeIndex, gScore + num, 0U, this.position);
		}

		// Token: 0x040004C6 RID: 1222
		public OffMeshLinks.OffMeshLinkSource linkSource;

		// Token: 0x040004C7 RID: 1223
		public OffMeshLinks.OffMeshLinkConcrete linkConcrete;

		// Token: 0x040004C8 RID: 1224
		public int nodeInGraphIndex;
	}
}
