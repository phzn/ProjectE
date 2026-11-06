using System;

namespace Pathfinding
{
	// Token: 0x02000068 RID: 104
	public class NNConstraintPathProxy
	{
		// Token: 0x06000402 RID: 1026 RVA: 0x00012D78 File Offset: 0x00010F78
		internal NNConstraintPathProxy(Path path)
		{
			this.path = path;
			this.pathID = (int)path.pathID;
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x00012D93 File Offset: 0x00010F93
		private void Validate()
		{
			if ((int)this.path.pathID != this.pathID)
			{
				throw new InvalidOperationException("Trying to change traversability constraint for a path that no longer exists. Check out the migration guide for version 5.4 for more info.");
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000404 RID: 1028 RVA: 0x00012DB3 File Offset: 0x00010FB3
		// (set) Token: 0x06000405 RID: 1029 RVA: 0x00012DC5 File Offset: 0x00010FC5
		public GraphMask graphMask
		{
			get
			{
				return this.path.traversalConstraint.graphMask;
			}
			set
			{
				this.Validate();
				this.path.traversalConstraint.graphMask = value;
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000406 RID: 1030 RVA: 0x00012DDE File Offset: 0x00010FDE
		// (set) Token: 0x06000407 RID: 1031 RVA: 0x00012DF0 File Offset: 0x00010FF0
		public int tags
		{
			get
			{
				return this.path.traversalConstraint.tags;
			}
			set
			{
				this.Validate();
				this.path.traversalConstraint.tags = value;
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000408 RID: 1032 RVA: 0x00012E09 File Offset: 0x00011009
		// (set) Token: 0x06000409 RID: 1033 RVA: 0x00012E16 File Offset: 0x00011016
		public DistanceMetric distanceMetric
		{
			get
			{
				return this.path.nearestNodeDistanceMetric;
			}
			set
			{
				this.Validate();
				this.path.nearestNodeDistanceMetric = value;
			}
		}

		// Token: 0x04000247 RID: 583
		private readonly Path path;

		// Token: 0x04000248 RID: 584
		private readonly int pathID;
	}
}
