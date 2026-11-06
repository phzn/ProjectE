using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200009D RID: 157
	[VolumeComponentMenuForRenderPipeline("Post-processing/Motion Blur", new Type[] { typeof(UniversalRenderPipeline) })]
	[Serializable]
	public sealed class MotionBlur : VolumeComponent, IPostProcessComponent
	{
		// Token: 0x0600057E RID: 1406 RVA: 0x0001FECA File Offset: 0x0001E0CA
		public bool IsActive()
		{
			return this.intensity.value > 0f && this.mode.value == MotionBlurMode.CameraOnly;
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x0001FEEE File Offset: 0x0001E0EE
		public bool IsTileCompatible()
		{
			return false;
		}

		// Token: 0x0400041C RID: 1052
		[Tooltip("The motion blur technique to use. If you don't need object motion blur, CameraOnly will result in better performance.")]
		public MotionBlurModeParameter mode = new MotionBlurModeParameter(MotionBlurMode.CameraOnly, false);

		// Token: 0x0400041D RID: 1053
		[Tooltip("The quality of the effect. Lower presets will result in better performance at the expense of visual quality.")]
		public MotionBlurQualityParameter quality = new MotionBlurQualityParameter(MotionBlurQuality.Low, false);

		// Token: 0x0400041E RID: 1054
		[Tooltip("The strength of the motion blur filter. Acts as a multiplier for velocities.")]
		public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 1f, false);

		// Token: 0x0400041F RID: 1055
		[Tooltip("Sets the maximum length, as a fraction of the screen's full resolution, that the velocity resulting from Camera rotation can have. Lower values will improve performance.")]
		public ClampedFloatParameter clamp = new ClampedFloatParameter(0.05f, 0f, 0.2f, false);
	}
}
