using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000E0 RID: 224
	internal static class TileSizeExtensions
	{
		// Token: 0x060006EE RID: 1774 RVA: 0x0002989F File Offset: 0x00027A9F
		public static bool IsValid(this TileSize tileSize)
		{
			return tileSize == TileSize._8 || tileSize == TileSize._16 || tileSize == TileSize._32 || tileSize == TileSize._64;
		}
	}
}
