using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000081 RID: 129
	[VolumeComponentMenuForRenderPipeline("Post-processing/Bloom", new Type[] { typeof(UniversalRenderPipeline) })]
	[Serializable]
	public sealed class Bloom : VolumeComponent, IPostProcessComponent
	{
		// Token: 0x0600050C RID: 1292 RVA: 0x0001DC68 File Offset: 0x0001BE68
		public bool IsActive()
		{
			return this.intensity.value > 0f;
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x0001DC7C File Offset: 0x0001BE7C
		public bool IsTileCompatible()
		{
			return false;
		}

		// Token: 0x0400039A RID: 922
		[Obsolete("This is obsolete, please use maxIterations instead.", false)]
		[Tooltip("The number of final iterations to skip in the effect processing sequence.")]
		public ClampedIntParameter skipIterations = new ClampedIntParameter(1, 0, 16, false);

		// Token: 0x0400039B RID: 923
		[Header("Bloom")]
		[Tooltip("Filters out pixels under this level of brightness. Value is in gamma-space.")]
		public MinFloatParameter threshold = new MinFloatParameter(0.9f, 0f, false);

		// Token: 0x0400039C RID: 924
		[Tooltip("Strength of the bloom filter.")]
		public MinFloatParameter intensity = new MinFloatParameter(0f, 0f, false);

		// Token: 0x0400039D RID: 925
		[Tooltip("Set the radius of the bloom effect.")]
		public ClampedFloatParameter scatter = new ClampedFloatParameter(0.7f, 0f, 1f, false);

		// Token: 0x0400039E RID: 926
		[Tooltip("Set the maximum intensity that Unity uses to calculate Bloom. If pixels in your Scene are more intense than this, URP renders them at their current intensity, but uses this intensity value for the purposes of Bloom calculations.")]
		public MinFloatParameter clamp = new MinFloatParameter(65472f, 0f, false);

		// Token: 0x0400039F RID: 927
		[Tooltip("Use the color picker to select a color for the Bloom effect to tint to.")]
		public ColorParameter tint = new ColorParameter(Color.white, false, false, true, false);

		// Token: 0x040003A0 RID: 928
		[Tooltip("Use bicubic sampling instead of bilinear sampling for the upsampling passes. This is slightly more expensive but helps getting smoother visuals.")]
		public BoolParameter highQualityFiltering = new BoolParameter(false, false);

		// Token: 0x040003A1 RID: 929
		[Tooltip("The starting resolution that this effect begins processing.")]
		[AdditionalProperty]
		public DownscaleParameter downscale = new DownscaleParameter(BloomDownscaleMode.Half, false);

		// Token: 0x040003A2 RID: 930
		[Tooltip("The maximum number of iterations in the effect processing sequence.")]
		[AdditionalProperty]
		public ClampedIntParameter maxIterations = new ClampedIntParameter(6, 2, 8, false);

		// Token: 0x040003A3 RID: 931
		[Header("Lens Dirt")]
		[Tooltip("Dirtiness texture to add smudges or dust to the bloom effect.")]
		public TextureParameter dirtTexture = new TextureParameter(null, false);

		// Token: 0x040003A4 RID: 932
		[Tooltip("Amount of dirtiness.")]
		public MinFloatParameter dirtIntensity = new MinFloatParameter(0f, 0f, false);
	}
}
