using System;
using System.Runtime.CompilerServices;

namespace Pathfinding
{
	// Token: 0x020000B2 RID: 178
	public static class DefaultITraversalProvider
	{
		// Token: 0x060005D7 RID: 1495 RVA: 0x00019DC2 File Offset: 0x00017FC2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool CanTraverse(ref TraversalConstraint traversalConstraint, GraphNode node)
		{
			return true;
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x0001C8C1 File Offset: 0x0001AAC1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float GetTraversalCostMultiplier(ref TraversalCosts traversalCosts, GraphNode node)
		{
			return traversalCosts.GetDefaultTraversalCostMultiplier(node);
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x0001C8CA File Offset: 0x0001AACA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint GetConnectionCost(ref TraversalCosts traversalCosts, GraphNode from, GraphNode to)
		{
			return traversalCosts.GetDefaultConnectionCost(from, to);
		}
	}
}
