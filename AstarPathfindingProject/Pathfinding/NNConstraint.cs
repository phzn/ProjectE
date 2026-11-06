using System;

namespace Pathfinding
{
	// Token: 0x02000069 RID: 105
	[Obsolete("Use NearestNodeConstraint struct instead. Check the upgrade guide for version 5.4 for more info.")]
	public class NNConstraint
	{
		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x0600040A RID: 1034 RVA: 0x00012E2A File Offset: 0x0001102A
		// (set) Token: 0x0600040B RID: 1035 RVA: 0x00012E4D File Offset: 0x0001104D
		[Obsolete("Use distanceMetric = DistanceMetric.ClosestAsSeenFromAbove() instead")]
		public bool distanceXZ
		{
			get
			{
				return this.distanceMetric.isProjectedDistance && this.distanceMetric.distanceScaleAlongProjectionDirection == 0f;
			}
			set
			{
				if (value)
				{
					this.distanceMetric = DistanceMetric.ClosestAsSeenFromAbove();
					return;
				}
				this.distanceMetric = DistanceMetric.Euclidean;
			}
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x00012E69 File Offset: 0x00011069
		public virtual bool SuitableGraph(int graphIndex, NavGraph graph)
		{
			return this.graphMask.Contains((uint)graphIndex);
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x00012E78 File Offset: 0x00011078
		public virtual bool Suitable(GraphNode node)
		{
			return (!this.constrainWalkability || node.Walkable == this.walkable) && (!this.constrainArea || this.area < 0 || (ulong)node.Area == (ulong)((long)this.area)) && (!this.constrainTags || ((this.tags >> (int)node.Tag) & 1) != 0);
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x00012EDF File Offset: 0x000110DF
		public void UseSettings(PathRequestSettings settings)
		{
			this.graphMask = settings.graphMask;
			this.constrainTags = true;
			this.tags = settings.traversableTags;
			this.constrainWalkability = true;
			this.walkable = true;
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x0600040F RID: 1039 RVA: 0x00012F0E File Offset: 0x0001110E
		[Obsolete("Use NNConstraint.Walkable instead. It is equivalent, but the name is more descriptive")]
		public static NNConstraint Default
		{
			get
			{
				return new NNConstraint();
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000410 RID: 1040 RVA: 0x00012F0E File Offset: 0x0001110E
		public static NNConstraint Walkable
		{
			get
			{
				return new NNConstraint();
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000411 RID: 1041 RVA: 0x00012F15 File Offset: 0x00011115
		public static NNConstraint None
		{
			get
			{
				return new NNConstraint
				{
					constrainWalkability = false,
					constrainArea = false,
					constrainTags = false,
					constrainDistance = false,
					graphMask = GraphMask.everything
				};
			}
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x00012F80 File Offset: 0x00011180
		public TraversalConstraint ToTraversalConstraint()
		{
			return new TraversalConstraint
			{
				tags = this.tags,
				graphMask = this.graphMask
			};
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x00012FB0 File Offset: 0x000111B0
		public NearestNodeConstraint ToNearestNodeConstraint()
		{
			return new NearestNodeConstraint
			{
				filter = null,
				distanceMetric = this.distanceMetric,
				tags = (this.constrainTags ? this.tags : (-1)),
				area = (this.constrainArea ? this.area : (-1)),
				graphMask = this.graphMask,
				maxDistanceSqr = (this.constrainDistance ? (-1f) : float.PositiveInfinity),
				walkable = (this.constrainWalkability ? (this.walkable ? NearestNodeConstraint.WalkabilityConstraint.Walkable : NearestNodeConstraint.WalkabilityConstraint.Unwalkable) : NearestNodeConstraint.WalkabilityConstraint.DontCare)
			};
		}

		// Token: 0x04000249 RID: 585
		public GraphMask graphMask = GraphMask.everything;

		// Token: 0x0400024A RID: 586
		public bool constrainArea;

		// Token: 0x0400024B RID: 587
		public int area = -1;

		// Token: 0x0400024C RID: 588
		public DistanceMetric distanceMetric;

		// Token: 0x0400024D RID: 589
		public bool constrainWalkability = true;

		// Token: 0x0400024E RID: 590
		public bool walkable = true;

		// Token: 0x0400024F RID: 591
		public bool constrainTags = true;

		// Token: 0x04000250 RID: 592
		public int tags = -1;

		// Token: 0x04000251 RID: 593
		public bool constrainDistance = true;
	}
}
