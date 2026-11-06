using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000FF RID: 255
	public struct PunctualLightData
	{
		// Token: 0x040006D3 RID: 1747
		public Vector3 wsPos;

		// Token: 0x040006D4 RID: 1748
		public float radius;

		// Token: 0x040006D5 RID: 1749
		public Vector4 color;

		// Token: 0x040006D6 RID: 1750
		public Vector4 attenuation;

		// Token: 0x040006D7 RID: 1751
		public Vector3 spotDirection;

		// Token: 0x040006D8 RID: 1752
		public int flags;

		// Token: 0x040006D9 RID: 1753
		public Vector4 occlusionProbeInfo;

		// Token: 0x040006DA RID: 1754
		public uint layerMask;
	}
}
