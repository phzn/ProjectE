using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000097 RID: 151
	[VolumeComponentMenuForRenderPipeline("Post-processing/Film Grain", new Type[] { typeof(UniversalRenderPipeline) })]
	[Serializable]
	public sealed class FilmGrain : VolumeComponent, IPostProcessComponent
	{
		// Token: 0x06000574 RID: 1396 RVA: 0x0001FC58 File Offset: 0x0001DE58
		public bool IsActive()
		{
			return this.intensity.value > 0f && (this.type.value != FilmGrainLookup.Custom || this.texture.value != null);
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x0001FC90 File Offset: 0x0001DE90
		public bool IsTileCompatible()
		{
			return true;
		}

		// Token: 0x04000409 RID: 1033
		[Tooltip("The type of grain to use. You can select a preset or provide your own texture by selecting Custom.")]
		public FilmGrainLookupParameter type = new FilmGrainLookupParameter(FilmGrainLookup.Thin1, false);

		// Token: 0x0400040A RID: 1034
		[Tooltip("Use the slider to set the strength of the Film Grain effect.")]
		public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 1f, false);

		// Token: 0x0400040B RID: 1035
		[Tooltip("Controls the noisiness response curve based on scene luminance. Higher values mean less noise in light areas.")]
		public ClampedFloatParameter response = new ClampedFloatParameter(0.8f, 0f, 1f, false);

		// Token: 0x0400040C RID: 1036
		[Tooltip("A tileable texture to use for the grain. The neutral value is 0.5 where no grain is applied.")]
		public NoInterpTextureParameter texture = new NoInterpTextureParameter(null, false);
	}
}
