using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000302 RID: 770
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/PostProcessing/MMColorAdjustmentsShaker_HDRP")]
	public class MMColorAdjustmentsShaker_HDRP : MMShaker
	{
		// Token: 0x040018D2 RID: 6354
		public bool RelativeValues = true;

		// Token: 0x040018D3 RID: 6355
		[MMInspectorGroup("Post Exposure", true, 44, false)]
		[Tooltip("the curve used to animate the focus distance value on")]
		public AnimationCurve ShakePostExposure = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040018D4 RID: 6356
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapPostExposureZero;

		// Token: 0x040018D5 RID: 6357
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapPostExposureOne = 1f;

		// Token: 0x040018D6 RID: 6358
		[MMInspectorGroup("Hue Shift", true, 45, false)]
		[Tooltip("the curve used to animate the aperture value on")]
		public AnimationCurve ShakeHueShift = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040018D7 RID: 6359
		[Range(-180f, 180f)]
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapHueShiftZero;

		// Token: 0x040018D8 RID: 6360
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-180f, 180f)]
		public float RemapHueShiftOne = 180f;

		// Token: 0x040018D9 RID: 6361
		[MMInspectorGroup("Saturation", true, 46, false)]
		[Tooltip("the curve used to animate the focal length value on")]
		public AnimationCurve ShakeSaturation = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040018DA RID: 6362
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-100f, 100f)]
		public float RemapSaturationZero;

		// Token: 0x040018DB RID: 6363
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-100f, 100f)]
		public float RemapSaturationOne = 100f;

		// Token: 0x040018DC RID: 6364
		[MMInspectorGroup("Contrast", true, 47, false)]
		[Tooltip("the curve used to animate the focal length value on")]
		public AnimationCurve ShakeContrast = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040018DD RID: 6365
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-100f, 100f)]
		public float RemapContrastZero;

		// Token: 0x040018DE RID: 6366
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-100f, 100f)]
		public float RemapContrastOne = 100f;

		// Token: 0x040018DF RID: 6367
		[MMInspectorGroup("Color Filter", true, 48, false)]
		[Tooltip("the color filter mode to work with (none, over a gradient, or interpolate to a destination color")]
		public MMColorAdjustmentsShaker_HDRP.ColorFilterModes ColorFilterMode;

		// Token: 0x040018E0 RID: 6368
		[Tooltip("the gradient over which to modify the color filter")]
		[MMFEnumCondition("ColorFilterMode", new int[] { 1 })]
		[GradientUsage(true)]
		public Gradient ColorFilterGradient;

		// Token: 0x040018E1 RID: 6369
		[Tooltip("the destination color to match when in Interpolate mode")]
		[MMFEnumCondition("ColorFilterMode", new int[] { 2 })]
		public Color ColorFilterDestination = Color.yellow;

		// Token: 0x040018E2 RID: 6370
		[Tooltip("the curve over which to interpolate the color filter")]
		[MMFEnumCondition("ColorFilterMode", new int[] { 2 })]
		public AnimationCurve ColorFilterCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x02000563 RID: 1379
		public enum ColorFilterModes
		{
			// Token: 0x04002496 RID: 9366
			None,
			// Token: 0x04002497 RID: 9367
			Gradient,
			// Token: 0x04002498 RID: 9368
			Interpolate
		}
	}
}
