using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectExhibitionist.Minimap
{
	// Token: 0x020009AC RID: 2476
	internal sealed class MiniMapChunkVisual
	{
		// Token: 0x0400454B RID: 17739
		public Vector3 center;

		// Token: 0x0400454C RID: 17740
		public float yaw;

		// Token: 0x0400454D RID: 17741
		public Vector2 size;

		// Token: 0x0400454E RID: 17742
		public ChunkShape shape;

		// Token: 0x0400454F RID: 17743
		public int stableHash;

		// Token: 0x04004550 RID: 17744
		public bool previewOnly;

		// Token: 0x04004551 RID: 17745
		public MiniMapTileReference tile;

		// Token: 0x04004552 RID: 17746
		public readonly List<MiniMapZoneVisual> zones = new List<MiniMapZoneVisual>(4);
	}
}
