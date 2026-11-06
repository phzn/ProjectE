using System;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x020000B3 RID: 179
	public struct NearestNodeConstraint
	{
		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060005DA RID: 1498 RVA: 0x0001C8D4 File Offset: 0x0001AAD4
		// (set) Token: 0x060005DB RID: 1499 RVA: 0x0001C8E1 File Offset: 0x0001AAE1
		public int tags
		{
			get
			{
				return this.traversal.tags;
			}
			set
			{
				this.traversal.tags = value;
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060005DC RID: 1500 RVA: 0x0001C8EF File Offset: 0x0001AAEF
		// (set) Token: 0x060005DD RID: 1501 RVA: 0x0001C8FC File Offset: 0x0001AAFC
		public GraphMask graphMask
		{
			get
			{
				return this.traversal.graphMask;
			}
			set
			{
				this.traversal.graphMask = value;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060005DE RID: 1502 RVA: 0x0001C90A File Offset: 0x0001AB0A
		// (set) Token: 0x060005DF RID: 1503 RVA: 0x0001C917 File Offset: 0x0001AB17
		public Func<GraphNode, bool> filter
		{
			get
			{
				return this.traversal.filter;
			}
			set
			{
				this.traversal.filter = value;
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060005E0 RID: 1504 RVA: 0x0001C925 File Offset: 0x0001AB25
		// (set) Token: 0x060005E1 RID: 1505 RVA: 0x0001C932 File Offset: 0x0001AB32
		public ITraversalProvider traversalProvider
		{
			get
			{
				return this.traversal.traversalProvider;
			}
			set
			{
				this.traversal.traversalProvider = value;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060005E2 RID: 1506 RVA: 0x0001C940 File Offset: 0x0001AB40
		public bool allNodesAreSuitable
		{
			get
			{
				return this.area < 0 && this.tags == -1 && this.graphMask.containsAllGraphs && this.walkable == NearestNodeConstraint.WalkabilityConstraint.DontCare && this.traversal.filterType == TraversalConstraint.FilterType.None;
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060005E3 RID: 1507 RVA: 0x0001C988 File Offset: 0x0001AB88
		// (set) Token: 0x060005E4 RID: 1508 RVA: 0x0001C9BC File Offset: 0x0001ABBC
		public float? maxDistance
		{
			get
			{
				if (this.maxDistanceSqr >= 0f)
				{
					return new float?(Mathf.Sqrt(this.maxDistanceSqr));
				}
				return null;
			}
			set
			{
				this.maxDistanceSqr = ((value != null) ? (value.Value * value.Value) : (-1f));
			}
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x0001C9E3 File Offset: 0x0001ABE3
		internal float maxDistanceSqrOrDefault(AstarPath astar)
		{
			if (this.maxDistanceSqr >= 0f)
			{
				return this.maxDistanceSqr;
			}
			return astar.maxNearestNodeDistanceSqr;
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x0001CA00 File Offset: 0x0001AC00
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe bool Suitable(GraphNode node)
		{
			if (this.area >= 0 && (ulong)node.Area != (ulong)((long)this.area))
			{
				return false;
			}
			if (node.Walkable)
			{
				if (this.walkable == NearestNodeConstraint.WalkabilityConstraint.Unwalkable)
				{
					return false;
				}
			}
			else if (this.walkable == NearestNodeConstraint.WalkabilityConstraint.Walkable)
			{
				return false;
			}
			if (((this.traversal.tags >> (int)node.Tag) & 1) == 0)
			{
				return false;
			}
			TraversalConstraint.FilterType filterType = this.traversal.filterType;
			if (filterType != TraversalConstraint.FilterType.TraversalProvider)
			{
				return filterType != TraversalConstraint.FilterType.Func || (*UnsafeUtility.As<object, Func<GraphNode, bool>>(ref this.traversal.filterObj))(node);
			}
			return UnsafeUtility.As<object, ITraversalProvider>(ref this.traversal.filterObj)->CanTraverse(ref this.traversal, node);
		}

		// Token: 0x040003C4 RID: 964
		public DistanceMetric distanceMetric;

		// Token: 0x040003C5 RID: 965
		public int area;

		// Token: 0x040003C6 RID: 966
		public float maxDistanceSqr;

		// Token: 0x040003C7 RID: 967
		internal TraversalConstraint traversal;

		// Token: 0x040003C8 RID: 968
		public NearestNodeConstraint.WalkabilityConstraint walkable;

		// Token: 0x040003C9 RID: 969
		public static readonly NearestNodeConstraint Walkable = new NearestNodeConstraint
		{
			traversal = TraversalConstraint.None,
			distanceMetric = DistanceMetric.Euclidean,
			area = -1,
			maxDistanceSqr = -1f,
			walkable = NearestNodeConstraint.WalkabilityConstraint.Walkable
		};

		// Token: 0x040003CA RID: 970
		public static readonly NearestNodeConstraint None = new NearestNodeConstraint
		{
			traversal = TraversalConstraint.None,
			distanceMetric = DistanceMetric.Euclidean,
			area = -1,
			maxDistanceSqr = -1f,
			walkable = NearestNodeConstraint.WalkabilityConstraint.DontCare
		};

		// Token: 0x020000B4 RID: 180
		public enum WalkabilityConstraint : byte
		{
			// Token: 0x040003CC RID: 972
			Walkable,
			// Token: 0x040003CD RID: 973
			Unwalkable,
			// Token: 0x040003CE RID: 974
			DontCare
		}
	}
}
