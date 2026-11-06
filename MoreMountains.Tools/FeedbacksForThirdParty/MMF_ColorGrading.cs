using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x0200031D RID: 797
	[AddComponentMenu("")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.PostProcessing", null)]
	[FeedbackHelp("This feedback allows you to control color grading post exposure, hue shift, saturation and contrast over time. It requires you have in your scene an object with a PostProcessVolume with Color Grading active, and a MMColorGradingShaker component.")]
	public class MMF_ColorGrading : MMF_Feedback
	{
		// Token: 0x17000288 RID: 648
		// (get) Token: 0x0600161A RID: 5658 RVA: 0x0005DC45 File Offset: 0x0005BE45
		// (set) Token: 0x0600161B RID: 5659 RVA: 0x0005DC53 File Offset: 0x0005BE53
		public override float FeedbackDuration
		{
			get
			{
				return this.ApplyTimeMultiplier(this.ShakeDuration);
			}
			set
			{
				this.ShakeDuration = value;
			}
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x0600161C RID: 5660 RVA: 0x0005DC5C File Offset: 0x0005BE5C
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x0600161D RID: 5661 RVA: 0x0005DC5F File Offset: 0x0005BE5F
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600161E RID: 5662 RVA: 0x0005DC64 File Offset: 0x0005BE64
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_ColorGrading.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			MMColorGradingShakeEvent.Trigger(this.ShakePostExposure, this.RemapPostExposureZero, this.RemapPostExposureOne, this.ShakeHueShift, this.RemapHueShiftZero, this.RemapHueShiftOne, this.ShakeSaturation, this.RemapSaturationZero, this.RemapSaturationOne, this.ShakeContrast, this.RemapContrastZero, this.RemapContrastOne, this.ShakeColorFilter, this.ColorFilterGradient, this.FeedbackDuration, this.RelativeIntensity, num, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
		}

		// Token: 0x0600161F RID: 5663 RVA: 0x0005DD10 File Offset: 0x0005BF10
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_ColorGrading.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMColorGradingShakeEvent.Trigger(this.ShakePostExposure, this.RemapPostExposureZero, this.RemapPostExposureOne, this.ShakeHueShift, this.RemapHueShiftZero, this.RemapHueShiftOne, this.ShakeSaturation, this.RemapSaturationZero, this.RemapSaturationOne, this.ShakeContrast, this.RemapContrastZero, this.RemapContrastOne, this.ShakeColorFilter, this.ColorFilterGradient, this.FeedbackDuration, false, 1f, null, true, true, true, TimescaleModes.Scaled, true, false);
		}

		// Token: 0x06001620 RID: 5664 RVA: 0x0005DDA4 File Offset: 0x0005BFA4
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_ColorGrading.FeedbackTypeAuthorized)
			{
				return;
			}
			MMColorGradingShakeEvent.Trigger(this.ShakePostExposure, this.RemapPostExposureZero, this.RemapPostExposureOne, this.ShakeHueShift, this.RemapHueShiftZero, this.RemapHueShiftOne, this.ShakeSaturation, this.RemapSaturationZero, this.RemapSaturationOne, this.ShakeContrast, this.RemapContrastZero, this.RemapContrastOne, this.ShakeColorFilter, this.ColorFilterGradient, this.FeedbackDuration, false, 1f, null, true, true, true, TimescaleModes.Scaled, false, true);
		}

		// Token: 0x06001621 RID: 5665 RVA: 0x0005DE2D File Offset: 0x0005C02D
		public override void AutomaticShakerSetup()
		{
		}

		// Token: 0x04001963 RID: 6499
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001964 RID: 6500
		[MMFInspectorGroup("Color Grading", true, 46, false, false)]
		[Tooltip("the duration of the shake, in seconds")]
		public float ShakeDuration = 1f;

		// Token: 0x04001965 RID: 6501
		[Tooltip("whether or not to add to the initial intensity")]
		public bool RelativeIntensity = true;

		// Token: 0x04001966 RID: 6502
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x04001967 RID: 6503
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x04001968 RID: 6504
		[MMFInspectorGroup("Post Exposure", true, 47, false, false)]
		[Tooltip("the curve used to animate the focus distance value on")]
		public AnimationCurve ShakePostExposure = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001969 RID: 6505
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapPostExposureZero;

		// Token: 0x0400196A RID: 6506
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapPostExposureOne = 1f;

		// Token: 0x0400196B RID: 6507
		[MMFInspectorGroup("Hue Shift", true, 48, false, false)]
		[Tooltip("the curve used to animate the aperture value on")]
		public AnimationCurve ShakeHueShift = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x0400196C RID: 6508
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-180f, 180f)]
		public float RemapHueShiftZero;

		// Token: 0x0400196D RID: 6509
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-180f, 180f)]
		public float RemapHueShiftOne = 180f;

		// Token: 0x0400196E RID: 6510
		[MMFInspectorGroup("Saturation", true, 49, false, false)]
		[Tooltip("the curve used to animate the focal length value on")]
		public AnimationCurve ShakeSaturation = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x0400196F RID: 6511
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-100f, 100f)]
		public float RemapSaturationZero;

		// Token: 0x04001970 RID: 6512
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-100f, 100f)]
		public float RemapSaturationOne = 100f;

		// Token: 0x04001971 RID: 6513
		[MMFInspectorGroup("Contrast", true, 50, false, false)]
		[Tooltip("the curve used to animate the focal length value on")]
		public AnimationCurve ShakeContrast = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001972 RID: 6514
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-100f, 100f)]
		public float RemapContrastZero;

		// Token: 0x04001973 RID: 6515
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-100f, 100f)]
		public float RemapContrastOne = 100f;

		// Token: 0x04001974 RID: 6516
		[MMFInspectorGroup("Color Filter", true, 50, false, false)]
		[Tooltip("if this is true, the color filter will be animated over the gradient below")]
		public bool ShakeColorFilter;

		// Token: 0x04001975 RID: 6517
		[Tooltip("the gradient to use to animate the color filter over time")]
		[GradientUsage(true)]
		public Gradient ColorFilterGradient;
	}
}
