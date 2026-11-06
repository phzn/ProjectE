using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000027 RID: 39
	internal struct LightStats
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000172 RID: 370 RVA: 0x0000C644 File Offset: 0x0000A844
		public bool useNormalMap
		{
			get
			{
				return this.totalNormalMapUsage > 0;
			}
		}

		// Token: 0x040000E0 RID: 224
		public int totalLights;

		// Token: 0x040000E1 RID: 225
		public int totalNormalMapUsage;

		// Token: 0x040000E2 RID: 226
		public int totalVolumetricUsage;

		// Token: 0x040000E3 RID: 227
		public uint blendStylesUsed;

		// Token: 0x040000E4 RID: 228
		public uint blendStylesWithLights;
	}
}
