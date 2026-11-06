using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000E5 RID: 229
	public enum AntialiasingMode
	{
		// Token: 0x040005AA RID: 1450
		[InspectorName("No Anti-aliasing")]
		None,
		// Token: 0x040005AB RID: 1451
		[InspectorName("Fast Approximate Anti-aliasing (FXAA)")]
		FastApproximateAntialiasing,
		// Token: 0x040005AC RID: 1452
		[InspectorName("Subpixel Morphological Anti-aliasing (SMAA)")]
		SubpixelMorphologicalAntiAliasing,
		// Token: 0x040005AD RID: 1453
		[InspectorName("Temporal Anti-aliasing (TAA)")]
		TemporalAntiAliasing
	}
}
