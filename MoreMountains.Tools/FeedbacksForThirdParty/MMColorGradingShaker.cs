using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000329 RID: 809
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/PostProcessing/MMColorGradingShaker")]
	public class MMColorGradingShaker : MMShaker
	{
		// Token: 0x040019C5 RID: 6597
		public bool RelativeValues = true;

		// Token: 0x040019C6 RID: 6598
		[MMInspectorGroup("Post Exposure", true, 40, false)]
		[Tooltip("the curve used to animate the focus distance value on")]
		public AnimationCurve ShakePostExposure = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040019C7 RID: 6599
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapPostExposureZero;

		// Token: 0x040019C8 RID: 6600
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapPostExposureOne = 1f;

		// Token: 0x040019C9 RID: 6601
		[MMInspectorGroup("Hue Shift", true, 49, false)]
		[Tooltip("the curve used to animate the aperture value on")]
		public AnimationCurve ShakeHueShift = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040019CA RID: 6602
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-180f, 180f)]
		public float RemapHueShiftZero;

		// Token: 0x040019CB RID: 6603
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-180f, 180f)]
		public float RemapHueShiftOne = 180f;

		// Token: 0x040019CC RID: 6604
		[MMInspectorGroup("Saturation", true, 48, false)]
		[Tooltip("the curve used to animate the focal length value on")]
		public AnimationCurve ShakeSaturation = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040019CD RID: 6605
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-100f, 100f)]
		public float RemapSaturationZero;

		// Token: 0x040019CE RID: 6606
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-100f, 100f)]
		public float RemapSaturationOne = 100f;

		// Token: 0x040019CF RID: 6607
		[MMInspectorGroup("Contrast", true, 47, false)]
		[Tooltip("the curve used to animate the focal length value on")]
		public AnimationCurve ShakeContrast = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040019D0 RID: 6608
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-100f, 100f)]
		public float RemapContrastZero;

		// Token: 0x040019D1 RID: 6609
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-100f, 100f)]
		public float RemapContrastOne = 100f;

		// Token: 0x040019D2 RID: 6610
		[MMFInspectorGroup("Color Filter", true, 50, false, false)]
		[Tooltip("if this is true, the color filter will be animated over the gradient below")]
		public bool ShakeColorFilter;

		// Token: 0x040019D3 RID: 6611
		[Tooltip("the gradient to use to animate the color filter over time")]
		[GradientUsage(true)]
		public Gradient ColorFilterGradient;
	}
}
