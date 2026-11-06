using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000A6 RID: 166
	[VolumeComponentMenuForRenderPipeline("Post-processing/Tonemapping", new Type[] { typeof(UniversalRenderPipeline) })]
	[Serializable]
	public sealed class Tonemapping : VolumeComponent, IPostProcessComponent
	{
		// Token: 0x0600058C RID: 1420 RVA: 0x00020172 File Offset: 0x0001E372
		public bool IsActive()
		{
			return this.mode.value > TonemappingMode.None;
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x00020182 File Offset: 0x0001E382
		public bool IsTileCompatible()
		{
			return true;
		}

		// Token: 0x04000437 RID: 1079
		[Tooltip("Select a tonemapping algorithm to use for the color grading process.")]
		public TonemappingModeParameter mode = new TonemappingModeParameter(TonemappingMode.None, false);

		// Token: 0x04000438 RID: 1080
		[AdditionalProperty]
		[Tooltip("Specifies the range reduction mode used when HDR output is enabled and Neutral tonemapping is enabled.")]
		public NeutralRangeReductionModeParameter neutralHDRRangeReductionMode = new NeutralRangeReductionModeParameter(NeutralRangeReductionMode.BT2390, false);

		// Token: 0x04000439 RID: 1081
		[Tooltip("Use the ACES preset for HDR displays.")]
		public HDRACESPresetParameter acesPreset = new HDRACESPresetParameter(HDRACESPreset.ACES1000Nits, false);

		// Token: 0x0400043A RID: 1082
		[Tooltip("Specify how much hue to preserve. Values closer to 0 are likely to preserve hue. As values get closer to 1, Unity doesn't correct hue shifts.")]
		public ClampedFloatParameter hueShiftAmount = new ClampedFloatParameter(0f, 0f, 1f, false);

		// Token: 0x0400043B RID: 1083
		[Tooltip("Enable to use values detected from the output device as paper white. When enabled, output images might differ between SDR and HDR. For best accuracy, set this value manually.")]
		public BoolParameter detectPaperWhite = new BoolParameter(false, false);

		// Token: 0x0400043C RID: 1084
		[Tooltip("The reference brightness of a paper white surface. This property determines the maximum brightness of UI. The brightness of the scene is scaled relative to this value. The value is in nits.")]
		public ClampedFloatParameter paperWhite = new ClampedFloatParameter(300f, 0f, 400f, false);

		// Token: 0x0400043D RID: 1085
		[Tooltip("Enable to use the minimum and maximum brightness values detected from the output device. For best accuracy, considering calibrating these values manually.")]
		public BoolParameter detectBrightnessLimits = new BoolParameter(true, false);

		// Token: 0x0400043E RID: 1086
		[Tooltip("The minimum brightness of the screen (in nits). This value is assumed to be 0.005f with ACES Tonemap.")]
		public ClampedFloatParameter minNits = new ClampedFloatParameter(0.005f, 0f, 50f, false);

		// Token: 0x0400043F RID: 1087
		[Tooltip("The maximum brightness of the screen (in nits). This value is defined by the preset when using ACES Tonemap.")]
		public ClampedFloatParameter maxNits = new ClampedFloatParameter(1000f, 0f, 5000f, false);
	}
}
