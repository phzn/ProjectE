using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000046 RID: 70
	public enum DynamicResUpscaleFilter : byte
	{
		// Token: 0x04000174 RID: 372
		[Obsolete("Bilinear upscale filter is considered obsolete and is not supported anymore, please use CatmullRom for a very cheap, but blurry filter.", false)]
		Bilinear,
		// Token: 0x04000175 RID: 373
		CatmullRom,
		// Token: 0x04000176 RID: 374
		[Obsolete("Lanczos upscale filter is considered obsolete and is not supported anymore, please use Contrast Adaptive Sharpening for very sharp filter or FidelityFX Super Resolution 1.0.", false)]
		Lanczos,
		// Token: 0x04000177 RID: 375
		ContrastAdaptiveSharpen,
		// Token: 0x04000178 RID: 376
		[InspectorName("FidelityFX Super Resolution 1.0")]
		EdgeAdaptiveScalingUpres,
		// Token: 0x04000179 RID: 377
		[InspectorName("TAA Upscale")]
		TAAU
	}
}
