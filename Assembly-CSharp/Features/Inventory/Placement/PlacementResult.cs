using System;
using UnityEngine;

namespace Features.Inventory.Placement
{
	// Token: 0x0200114B RID: 4427
	public struct PlacementResult
	{
		// Token: 0x17001554 RID: 5460
		// (get) Token: 0x0600868A RID: 34442 RVA: 0x0036F2FC File Offset: 0x0036D4FC
		public static PlacementResult Invalid
		{
			get
			{
				return new PlacementResult
				{
					IsValid = false,
					SurfaceType = PlacementSurfaceType.Invalid
				};
			}
		}

		// Token: 0x04008929 RID: 35113
		public bool IsValid;

		// Token: 0x0400892A RID: 35114
		public PlacementSurfaceType SurfaceType;

		// Token: 0x0400892B RID: 35115
		public Vector3 Position;

		// Token: 0x0400892C RID: 35116
		public Quaternion Rotation;

		// Token: 0x0400892D RID: 35117
		public Vector3 HitNormal;

		// Token: 0x0400892E RID: 35118
		public Vector3 FloorPosition;

		// Token: 0x0400892F RID: 35119
		public Vector3 HitPosition;

		// Token: 0x04008930 RID: 35120
		public Collider HitCollider;

		// Token: 0x04008931 RID: 35121
		public Transform SurfaceTransform;
	}
}
