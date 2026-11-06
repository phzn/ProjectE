using System;
using UnityEngine;

namespace GPUInstancerPro.TerrainModule
{
	// Token: 0x0200000E RID: 14
	public class GPUIBakedDetailTextureModifier : MonoBehaviour
	{
		// Token: 0x060000C5 RID: 197 RVA: 0x00005803 File Offset: 0x00003A03
		public void SetBakedDetailTexture0(Texture2D texture)
		{
			if (this.terrainBuiltin != null)
			{
				this.terrainBuiltin.SetBakedDetailTexture(0, texture);
			}
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00005820 File Offset: 0x00003A20
		public void SetBakedDetailTexture1(Texture2D texture)
		{
			if (this.terrainBuiltin != null)
			{
				this.terrainBuiltin.SetBakedDetailTexture(1, texture);
			}
		}

		// Token: 0x0400007C RID: 124
		public GPUITerrainBuiltin terrainBuiltin;
	}
}
