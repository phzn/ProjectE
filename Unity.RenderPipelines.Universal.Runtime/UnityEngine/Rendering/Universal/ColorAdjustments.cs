using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000090 RID: 144
	[VolumeComponentMenuForRenderPipeline("Post-processing/Color Adjustments", new Type[] { typeof(UniversalRenderPipeline) })]
	[Serializable]
	public sealed class ColorAdjustments : VolumeComponent, IPostProcessComponent
	{
		// Token: 0x06000566 RID: 1382 RVA: 0x0001F67C File Offset: 0x0001D87C
		public bool IsActive()
		{
			return this.postExposure.value != 0f || this.contrast.value != 0f || this.colorFilter != Color.white || this.hueShift != 0f || this.saturation != 0f;
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x0001F6E3 File Offset: 0x0001D8E3
		public bool IsTileCompatible()
		{
			return true;
		}

		// Token: 0x040003DF RID: 991
		[Tooltip("Adjusts the overall exposure of the scene in EV100. This is applied after HDR effect and right before tonemapping so it won't affect previous effects in the chain.")]
		public FloatParameter postExposure = new FloatParameter(0f, false);

		// Token: 0x040003E0 RID: 992
		[Tooltip("Expands or shrinks the overall range of tonal values.")]
		public ClampedFloatParameter contrast = new ClampedFloatParameter(0f, -100f, 100f, false);

		// Token: 0x040003E1 RID: 993
		[Tooltip("Tint the render by multiplying a color.")]
		public ColorParameter colorFilter = new ColorParameter(Color.white, true, false, true, false);

		// Token: 0x040003E2 RID: 994
		[Tooltip("Shift the hue of all colors.")]
		public ClampedFloatParameter hueShift = new ClampedFloatParameter(0f, -180f, 180f, false);

		// Token: 0x040003E3 RID: 995
		[Tooltip("Pushes the intensity of all colors.")]
		public ClampedFloatParameter saturation = new ClampedFloatParameter(0f, -100f, 100f, false);
	}
}
