using System;

namespace UnityEngine.Rendering.Universal.Internal
{
	// Token: 0x02000128 RID: 296
	internal enum StencilUsage
	{
		// Token: 0x040008CF RID: 2255
		UserMask = 15,
		// Token: 0x040008D0 RID: 2256
		StencilLight,
		// Token: 0x040008D1 RID: 2257
		MaterialMask = 96,
		// Token: 0x040008D2 RID: 2258
		MaterialUnlit = 0,
		// Token: 0x040008D3 RID: 2259
		MaterialLit = 32,
		// Token: 0x040008D4 RID: 2260
		MaterialSimpleLit = 64
	}
}
