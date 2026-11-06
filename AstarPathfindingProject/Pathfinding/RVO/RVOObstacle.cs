using System;

namespace Pathfinding.RVO
{
	// Token: 0x020002DA RID: 730
	public abstract class RVOObstacle : VersionedMonoBehaviour
	{
		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06001227 RID: 4647
		protected abstract bool ExecuteInEditor { get; }

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06001228 RID: 4648
		protected abstract bool LocalCoordinates { get; }

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06001229 RID: 4649
		protected abstract bool StaticObstacle { get; }

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x0600122A RID: 4650
		protected abstract float Height { get; }

		// Token: 0x04000D5F RID: 3423
		public RVOObstacle.ObstacleVertexWinding obstacleMode;

		// Token: 0x04000D60 RID: 3424
		public RVOLayer layer = RVOLayer.DefaultObstacle;

		// Token: 0x020002DB RID: 731
		public enum ObstacleVertexWinding
		{
			// Token: 0x04000D62 RID: 3426
			KeepOut,
			// Token: 0x04000D63 RID: 3427
			KeepIn
		}
	}
}
