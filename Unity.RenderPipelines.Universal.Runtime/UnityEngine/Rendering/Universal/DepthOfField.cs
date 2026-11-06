using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000094 RID: 148
	[VolumeComponentMenuForRenderPipeline("Post-processing/Depth Of Field", new Type[] { typeof(UniversalRenderPipeline) })]
	[Serializable]
	public sealed class DepthOfField : VolumeComponent, IPostProcessComponent
	{
		// Token: 0x06000570 RID: 1392 RVA: 0x0001FB12 File Offset: 0x0001DD12
		public bool IsActive()
		{
			return this.mode.value != DepthOfFieldMode.Off && SystemInfo.graphicsShaderLevel >= 35 && (this.mode.value != DepthOfFieldMode.Gaussian || SystemInfo.supportedRenderTargetCount > 1);
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x0001FB44 File Offset: 0x0001DD44
		public bool IsTileCompatible()
		{
			return false;
		}

		// Token: 0x040003F2 RID: 1010
		[Tooltip("Use \"Gaussian\" for a faster but non physical depth of field; \"Bokeh\" for a more realistic but slower depth of field.")]
		public DepthOfFieldModeParameter mode = new DepthOfFieldModeParameter(DepthOfFieldMode.Off, false);

		// Token: 0x040003F3 RID: 1011
		[Tooltip("The distance at which the blurring will start.")]
		public MinFloatParameter gaussianStart = new MinFloatParameter(10f, 0f, false);

		// Token: 0x040003F4 RID: 1012
		[Tooltip("The distance at which the blurring will reach its maximum radius.")]
		public MinFloatParameter gaussianEnd = new MinFloatParameter(30f, 0f, false);

		// Token: 0x040003F5 RID: 1013
		[Tooltip("The maximum radius of the gaussian blur. Values above 1 may show under-sampling artifacts.")]
		public ClampedFloatParameter gaussianMaxRadius = new ClampedFloatParameter(1f, 0.5f, 1.5f, false);

		// Token: 0x040003F6 RID: 1014
		[Tooltip("Use higher quality sampling to reduce flickering and improve the overall blur smoothness.")]
		public BoolParameter highQualitySampling = new BoolParameter(false, false);

		// Token: 0x040003F7 RID: 1015
		[Tooltip("The distance to the point of focus.")]
		public MinFloatParameter focusDistance = new MinFloatParameter(10f, 0.1f, false);

		// Token: 0x040003F8 RID: 1016
		[Tooltip("The ratio of aperture (known as f-stop or f-number). The smaller the value is, the shallower the depth of field is.")]
		public ClampedFloatParameter aperture = new ClampedFloatParameter(5.6f, 1f, 32f, false);

		// Token: 0x040003F9 RID: 1017
		[Tooltip("The distance between the lens and the film. The larger the value is, the shallower the depth of field is.")]
		public ClampedFloatParameter focalLength = new ClampedFloatParameter(50f, 1f, 300f, false);

		// Token: 0x040003FA RID: 1018
		[Tooltip("The number of aperture blades.")]
		public ClampedIntParameter bladeCount = new ClampedIntParameter(5, 3, 9, false);

		// Token: 0x040003FB RID: 1019
		[Tooltip("The curvature of aperture blades. The smaller the value is, the more visible aperture blades are. A value of 1 will make the bokeh perfectly circular.")]
		public ClampedFloatParameter bladeCurvature = new ClampedFloatParameter(1f, 0f, 1f, false);

		// Token: 0x040003FC RID: 1020
		[Tooltip("The rotation of aperture blades in degrees.")]
		public ClampedFloatParameter bladeRotation = new ClampedFloatParameter(0f, -180f, 180f, false);
	}
}
