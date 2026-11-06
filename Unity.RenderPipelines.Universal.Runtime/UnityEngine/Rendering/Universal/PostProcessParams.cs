using System;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000B9 RID: 185
	internal struct PostProcessParams
	{
		// Token: 0x060005F4 RID: 1524 RVA: 0x000243F8 File Offset: 0x000225F8
		public static PostProcessParams Create()
		{
			PostProcessParams postProcessParams;
			postProcessParams.blitMaterial = null;
			postProcessParams.requestHDRFormat = GraphicsFormat.None;
			return postProcessParams;
		}

		// Token: 0x040004B9 RID: 1209
		public Material blitMaterial;

		// Token: 0x040004BA RID: 1210
		public GraphicsFormat requestHDRFormat;
	}
}
