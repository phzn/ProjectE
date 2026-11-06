using System;

namespace Pathfinding
{
	// Token: 0x020000B1 RID: 177
	public interface ITraversalProvider
	{
		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060005CF RID: 1487 RVA: 0x00019DC2 File Offset: 0x00017FC2
		bool filterDiagonalGridConnections
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x0001C894 File Offset: 0x0001AA94
		bool CanTraverse(ref TraversalConstraint traversalConstraint, GraphNode node)
		{
			return DefaultITraversalProvider.CanTraverse(ref traversalConstraint, node);
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x0001C89D File Offset: 0x0001AA9D
		bool CanTraverse(ref TraversalConstraint traversalConstraint, GraphNode from, GraphNode to)
		{
			return this.CanTraverse(ref traversalConstraint, to);
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x0001C8A7 File Offset: 0x0001AAA7
		uint GetConnectionCost(ref TraversalCosts traversalCosts, GraphNode from, GraphNode to)
		{
			return DefaultITraversalProvider.GetConnectionCost(ref traversalCosts, from, to);
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x0001C8B1 File Offset: 0x0001AAB1
		float GetTraversalCostMultiplier(ref TraversalCosts traversalCosts, GraphNode node)
		{
			return DefaultITraversalProvider.GetTraversalCostMultiplier(ref traversalCosts, node);
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x0001C8BA File Offset: 0x0001AABA
		[Obsolete("Use CanTraverse(ref TraversalConstraint, GraphNode, GraphNode) instead")]
		bool CanTraverse(Path path, GraphNode from, GraphNode to)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x0001C8BA File Offset: 0x0001AABA
		[Obsolete("Use CanTraverse(ref TraversalConstraint, GraphNode) instead")]
		bool CanTraverse(Path path, GraphNode node)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x0001C8BA File Offset: 0x0001AABA
		[Obsolete("Use GetTraversalCostMultiplier(ref TraversalCosts, GraphNode) instead")]
		uint GetTraversalCost(Path path, GraphNode node)
		{
			throw new NotImplementedException();
		}
	}
}
