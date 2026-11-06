using System;
using UnityEngine;

namespace ProjectExhibitionist.Minimap
{
	// Token: 0x020009BA RID: 2490
	internal struct MiniMapTileReference
	{
		// Token: 0x06004601 RID: 17921 RVA: 0x001A7D00 File Offset: 0x001A5F00
		public static MiniMapTileReference From(ProceduralMiniMapStyleAtlas.Entry entry)
		{
			return new MiniMapTileReference
			{
				valid = true,
				uv = entry.uv,
				worldSize = entry.worldSize,
				centerOffset = entry.centerOffset
			};
		}

		// Token: 0x0400459C RID: 17820
		public bool valid;

		// Token: 0x0400459D RID: 17821
		public Rect uv;

		// Token: 0x0400459E RID: 17822
		public Vector2 worldSize;

		// Token: 0x0400459F RID: 17823
		public Vector2 centerOffset;
	}
}
