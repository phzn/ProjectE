using System;

namespace UnityEngine.TerrainUtils
{
	// Token: 0x0200001A RID: 26
	public readonly struct TerrainTileCoord
	{
		// Token: 0x060001AA RID: 426 RVA: 0x00004510 File Offset: 0x00002710
		public TerrainTileCoord(int tileX, int tileZ)
		{
			this.tileX = tileX;
			this.tileZ = tileZ;
		}

		// Token: 0x04000076 RID: 118
		public readonly int tileX;

		// Token: 0x04000077 RID: 119
		public readonly int tileZ;
	}
}
