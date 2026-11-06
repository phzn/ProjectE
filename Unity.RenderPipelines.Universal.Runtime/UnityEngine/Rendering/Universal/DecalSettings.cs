using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000C3 RID: 195
	[Serializable]
	internal class DecalSettings
	{
		// Token: 0x040004E4 RID: 1252
		public DecalTechniqueOption technique;

		// Token: 0x040004E5 RID: 1253
		public float maxDrawDistance = 1000f;

		// Token: 0x040004E6 RID: 1254
		public bool decalLayers;

		// Token: 0x040004E7 RID: 1255
		public DBufferSettings dBufferSettings;

		// Token: 0x040004E8 RID: 1256
		public DecalScreenSpaceSettings screenSpaceSettings;
	}
}
