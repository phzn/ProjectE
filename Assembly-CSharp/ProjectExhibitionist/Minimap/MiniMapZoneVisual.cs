using System;
using UnityEngine;

namespace ProjectExhibitionist.Minimap
{
	// Token: 0x020009AD RID: 2477
	internal struct MiniMapZoneVisual
	{
		// Token: 0x04004553 RID: 17747
		public Vector3 center;

		// Token: 0x04004554 RID: 17748
		public float yaw;

		// Token: 0x04004555 RID: 17749
		public Vector2 size;

		// Token: 0x04004556 RID: 17750
		public MiniMapZoneKind kind;

		// Token: 0x04004557 RID: 17751
		public int stableHash;

		// Token: 0x04004558 RID: 17752
		public MiniMapTileReference tile;
	}
}
