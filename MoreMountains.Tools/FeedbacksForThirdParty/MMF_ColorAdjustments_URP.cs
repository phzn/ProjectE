using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x0200034A RID: 842
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback allows you to control color adjustments' post exposure, hue shift, saturation and contrast over time. It requires you have in your scene an object with a Volume with Color Adjustments active, and a MMColorAdjustmentsShaker_URP component.")]
	[FeedbackPath("PostProcess/Color Adjustments URP")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.URP", null)]
	public class MMF_ColorAdjustments_URP : MMF_Feedback
	{
		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06001702 RID: 5890 RVA: 0x00061439 File Offset: 0x0005F639
		// (set) Token: 0x06001703 RID: 5891 RVA: 0x00061447 File Offset: 0x0005F647
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

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06001704 RID: 5892 RVA: 0x00061450 File Offset: 0x0005F650
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06001705 RID: 5893 RVA: 0x00061453 File Offset: 0x0005F653
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001706 RID: 5894 RVA: 0x00061458 File Offset: 0x0005F658
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_ColorAdjustments_URP.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			MMColorAdjustmentsShakeEvent_URP.Trigger(this.ShakePostExposure, this.RemapPostExposureZero, this.RemapPostExposureOne, this.ShakeHueShift, this.RemapHueShiftZero, this.RemapHueShiftOne, this.ShakeSaturation, this.RemapSaturationZero, this.RemapSaturationOne, this.ShakeContrast, this.RemapContrastZero, this.RemapContrastOne, this.ColorFilterMode, this.ColorFilterGradient, this.ColorFilterDestination, this.ColorFilterCurve, this.FeedbackDuration, this.RelativeIntensity, num, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
		}

		// Token: 0x06001707 RID: 5895 RVA: 0x00061510 File Offset: 0x0005F710
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_ColorAdjustments_URP.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMColorAdjustmentsShakeEvent_URP.Trigger(this.ShakePostExposure, this.RemapPostExposureZero, this.RemapPostExposureOne, this.ShakeHueShift, this.RemapHueShiftZero, this.RemapHueShiftOne, this.ShakeSaturation, this.RemapSaturationZero, this.RemapSaturationOne, this.ShakeContrast, this.RemapContrastZero, this.RemapContrastOne, this.ColorFilterMode, this.ColorFilterGradient, this.ColorFilterDestination, this.ColorFilterCurve, this.FeedbackDuration, this.RelativeIntensity, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, true, false);
		}

		// Token: 0x06001708 RID: 5896 RVA: 0x000615B8 File Offset: 0x0005F7B8
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_ColorAdjustments_URP.FeedbackTypeAuthorized)
			{
				return;
			}
			MMColorAdjustmentsShakeEvent_URP.Trigger(this.ShakePostExposure, this.RemapPostExposureZero, this.RemapPostExposureOne, this.ShakeHueShift, this.RemapHueShiftZero, this.RemapHueShiftOne, this.ShakeSaturation, this.RemapSaturationZero, this.RemapSaturationOne, this.ShakeContrast, this.RemapContrastZero, this.RemapContrastOne, this.ColorFilterMode, this.ColorFilterGradient, this.ColorFilterDestination, this.ColorFilterCurve, this.FeedbackDuration, this.RelativeIntensity, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, false, true);
		}

		// Token: 0x06001709 RID: 5897 RVA: 0x00061657 File Offset: 0x0005F857
		public override void AutomaticShakerSetup()
		{
		}

		// Token: 0x04001A59 RID: 6745
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001A5A RID: 6746
		[MMFInspectorGroup("Color Grading", true, 43, false, false)]
		[Tooltip("the duration of the shake, in seconds")]
		public float ShakeDuration = 1f;

		// Token: 0x04001A5B RID: 6747
		[Tooltip("whether or not to add to the initial intensity")]
		public bool RelativeIntensity = true;

		// Token: 0x04001A5C RID: 6748
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x04001A5D RID: 6749
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x04001A5E RID: 6750
		[MMFInspectorGroup("Post Exposure", true, 48, false, false)]
		[Tooltip("the curve used to animate the focus distance value on")]
		public AnimationCurve ShakePostExposure = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001A5F RID: 6751
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapPostExposureZero;

		// Token: 0x04001A60 RID: 6752
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapPostExposureOne = 1f;

		// Token: 0x04001A61 RID: 6753
		[MMFInspectorGroup("Hue Shift", true, 47, false, false)]
		[Tooltip("the curve used to animate the aperture value on")]
		public AnimationCurve ShakeHueShift = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001A62 RID: 6754
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-180f, 180f)]
		public float RemapHueShiftZero;

		// Token: 0x04001A63 RID: 6755
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-180f, 180f)]
		public float RemapHueShiftOne = 180f;

		// Token: 0x04001A64 RID: 6756
		[MMFInspectorGroup("Saturation", true, 46, false, false)]
		[Tooltip("the curve used to animate the focal length value on")]
		public AnimationCurve ShakeSaturation = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001A65 RID: 6757
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-100f, 100f)]
		public float RemapSaturationZero;

		// Token: 0x04001A66 RID: 6758
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-100f, 100f)]
		public float RemapSaturationOne = 100f;

		// Token: 0x04001A67 RID: 6759
		[MMFInspectorGroup("Contrast", true, 45, false, false)]
		[Tooltip("the curve used to animate the focal length value on")]
		public AnimationCurve ShakeContrast = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001A68 RID: 6760
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-100f, 100f)]
		public float RemapContrastZero;

		// Token: 0x04001A69 RID: 6761
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-100f, 100f)]
		public float RemapContrastOne = 100f;

		// Token: 0x04001A6A RID: 6762
		[MMFInspectorGroup("Color Filter", true, 44, false, false)]
		[Tooltip("the selected color filter mode :None : nothing will happen,gradient : evaluates the color over time on that gradient, from left to right,interpolate : lerps from the current color to the destination one ")]
		public MMColorAdjustmentsShaker_URP.ColorFilterModes ColorFilterMode;

		// Token: 0x04001A6B RID: 6763
		[Tooltip("the gradient to use to animate the color filter over time")]
		[MMFEnumCondition("ColorFilterMode", new int[] { 1 })]
		[GradientUsage(true)]
		public Gradient ColorFilterGradient;

		// Token: 0x04001A6C RID: 6764
		[Tooltip("the destination color when in interpolate mode")]
		[MMFEnumCondition("ColorFilterMode", new int[] { 2 })]
		public Color ColorFilterDestination = Color.yellow;

		// Token: 0x04001A6D RID: 6765
		[Tooltip("the curve to use when interpolating towards the destination color")]
		[MMFEnumCondition("ColorFilterMode", new int[] { 2 })]
		public AnimationCurve ColorFilterCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});
	}
}
