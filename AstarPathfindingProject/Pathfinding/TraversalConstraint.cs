using System;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;

namespace Pathfinding
{
	// Token: 0x020000C5 RID: 197
	public struct TraversalConstraint
	{
		// Token: 0x17000147 RID: 327
		// (get) Token: 0x0600068B RID: 1675 RVA: 0x0001EDD4 File Offset: 0x0001CFD4
		// (set) Token: 0x0600068C RID: 1676 RVA: 0x0001EDE1 File Offset: 0x0001CFE1
		public ITraversalProvider traversalProvider
		{
			get
			{
				return this.filterObj as ITraversalProvider;
			}
			set
			{
				this.filterObj = value;
				this.filterType = ((value != null) ? TraversalConstraint.FilterType.TraversalProvider : TraversalConstraint.FilterType.None);
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x0600068D RID: 1677 RVA: 0x0001EDF7 File Offset: 0x0001CFF7
		// (set) Token: 0x0600068E RID: 1678 RVA: 0x0001EE04 File Offset: 0x0001D004
		public Func<GraphNode, bool> filter
		{
			get
			{
				return this.filterObj as Func<GraphNode, bool>;
			}
			set
			{
				this.filterObj = value;
				this.filterType = ((value != null) ? TraversalConstraint.FilterType.Func : TraversalConstraint.FilterType.None);
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x0600068F RID: 1679 RVA: 0x0001EE1A File Offset: 0x0001D01A
		public unsafe bool filterDiagonalGridConnections
		{
			get
			{
				return this.filterType != TraversalConstraint.FilterType.TraversalProvider || UnsafeUtility.As<object, ITraversalProvider>(ref this.filterObj)->filterDiagonalGridConnections;
			}
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x0001EE38 File Offset: 0x0001D038
		public TraversalConstraint(Func<GraphNode, bool> filter)
		{
			this = TraversalConstraint.None;
			this.filter = filter;
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x0001EE4C File Offset: 0x0001D04C
		public TraversalConstraint(ITraversalProvider traversalProvider)
		{
			this = TraversalConstraint.None;
			this.traversalProvider = traversalProvider;
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x0001EE60 File Offset: 0x0001D060
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe bool CanTraverse(GraphNode node)
		{
			if (!this.CanTraverseSkipUserFilter(node))
			{
				return false;
			}
			TraversalConstraint.FilterType filterType = this.filterType;
			if (filterType != TraversalConstraint.FilterType.TraversalProvider)
			{
				return filterType != TraversalConstraint.FilterType.Func || (*UnsafeUtility.As<object, Func<GraphNode, bool>>(ref this.filterObj))(node);
			}
			return UnsafeUtility.As<object, ITraversalProvider>(ref this.filterObj)->CanTraverse(ref this, node);
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x0001EEB4 File Offset: 0x0001D0B4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe bool CanTraverse(GraphNode from, GraphNode to)
		{
			if (!this.CanTraverseSkipUserFilter(to))
			{
				return false;
			}
			TraversalConstraint.FilterType filterType = this.filterType;
			if (filterType != TraversalConstraint.FilterType.TraversalProvider)
			{
				return filterType != TraversalConstraint.FilterType.Func || (*UnsafeUtility.As<object, Func<GraphNode, bool>>(ref this.filterObj))(to);
			}
			return UnsafeUtility.As<object, ITraversalProvider>(ref this.filterObj)->CanTraverse(ref this, from, to);
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x0001EF06 File Offset: 0x0001D106
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool CanTraverseSkipUserFilter(GraphNode node)
		{
			return node.Walkable && ((this.tags >> (int)node.Tag) & 1) != 0;
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x0001EF28 File Offset: 0x0001D128
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public NearestNodeConstraint ToNearestNodeConstraint()
		{
			return new NearestNodeConstraint
			{
				traversal = this,
				distanceMetric = DistanceMetric.Euclidean,
				area = -1,
				maxDistanceSqr = -1f,
				walkable = NearestNodeConstraint.WalkabilityConstraint.Walkable
			};
		}

		// Token: 0x04000439 RID: 1081
		internal object filterObj;

		// Token: 0x0400043A RID: 1082
		public int tags;

		// Token: 0x0400043B RID: 1083
		public GraphMask graphMask;

		// Token: 0x0400043C RID: 1084
		internal TraversalConstraint.FilterType filterType;

		// Token: 0x0400043D RID: 1085
		public static readonly TraversalConstraint None = new TraversalConstraint
		{
			tags = -1,
			graphMask = GraphMask.everything
		};

		// Token: 0x020000C6 RID: 198
		internal enum FilterType
		{
			// Token: 0x0400043F RID: 1087
			None,
			// Token: 0x04000440 RID: 1088
			TraversalProvider,
			// Token: 0x04000441 RID: 1089
			Func
		}
	}
}
