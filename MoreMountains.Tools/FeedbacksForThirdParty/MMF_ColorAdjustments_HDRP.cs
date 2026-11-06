using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x020002F2 RID: 754
	[AddComponentMenu("")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.HDRP", null)]
	[FeedbackHelp("This feedback allows you to control color adjustments' post exposure, hue shift, saturation and contrast over time. It requires you have in your scene an object with a Volume with Color Adjustments active, and a MMColorAdjustmentsShaker_HDRP component.")]
	public class MMF_ColorAdjustments_HDRP : MMF_Feedback
	{
		// Token: 0x17000266 RID: 614
		// (get) Token: 0x0600153D RID: 5437 RVA: 0x0005A871 File Offset: 0x00058A71
		// (set) Token: 0x0600153E RID: 5438 RVA: 0x0005A87F File Offset: 0x00058A7F
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

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x0600153F RID: 5439 RVA: 0x0005A888 File Offset: 0x00058A88
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06001540 RID: 5440 RVA: 0x0005A88B File Offset: 0x00058A8B
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001541 RID: 5441 RVA: 0x0005A890 File Offset: 0x00058A90
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_ColorAdjustments_HDRP.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			MMColorAdjustmentsShakeEvent_HDRP.Trigger(this.ShakePostExposure, this.RemapPostExposureZero, this.RemapPostExposureOne, this.ShakeHueShift, this.RemapHueShiftZero, this.RemapHueShiftOne, this.ShakeSaturation, this.RemapSaturationZero, this.RemapSaturationOne, this.ShakeContrast, this.RemapContrastZero, this.RemapContrastOne, this.ColorFilterMode, this.ColorFilterGradient, this.ColorFilterDestination, this.ColorFilterCurve, this.FeedbackDuration, this.RelativeIntensity, num, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
		}

		// Token: 0x06001542 RID: 5442 RVA: 0x0005A948 File Offset: 0x00058B48
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_ColorAdjustments_HDRP.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMColorAdjustmentsShakeEvent_HDRP.Trigger(this.ShakePostExposure, this.RemapPostExposureZero, this.RemapPostExposureOne, this.ShakeHueShift, this.RemapHueShiftZero, this.RemapHueShiftOne, this.ShakeSaturation, this.RemapSaturationZero, this.RemapSaturationOne, this.ShakeContrast, this.RemapContrastZero, this.RemapContrastOne, this.ColorFilterMode, this.ColorFilterGradient, this.ColorFilterDestination, this.ColorFilterCurve, this.FeedbackDuration, this.RelativeIntensity, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, true, false);
		}

		// Token: 0x06001543 RID: 5443 RVA: 0x0005A9F0 File Offset: 0x00058BF0
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_ColorAdjustments_HDRP.FeedbackTypeAuthorized)
			{
				return;
			}
			MMColorAdjustmentsShakeEvent_HDRP.Trigger(this.ShakePostExposure, this.RemapPostExposureZero, this.RemapPostExposureOne, this.ShakeHueShift, this.RemapHueShiftZero, this.RemapHueShiftOne, this.ShakeSaturation, this.RemapSaturationZero, this.RemapSaturationOne, this.ShakeContrast, this.RemapContrastZero, this.RemapContrastOne, this.ColorFilterMode, this.ColorFilterGradient, this.ColorFilterDestination, this.ColorFilterCurve, this.FeedbackDuration, this.RelativeIntensity, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, false, true);
		}

		// Token: 0x06001544 RID: 5444 RVA: 0x0005AA8F File Offset: 0x00058C8F
		public override void AutomaticShakerSetup()
		{
		}

		// Token: 0x0400184D RID: 6221
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x0400184E RID: 6222
		[MMFInspectorGroup("Color Grading", true, 16, false, false)]
		[Tooltip("the duration of the shake, in seconds")]
		public float ShakeDuration = 1f;

		// Token: 0x0400184F RID: 6223
		[Tooltip("whether or not to add to the initial intensity")]
		public bool RelativeIntensity = true;

		// Token: 0x04001850 RID: 6224
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x04001851 RID: 6225
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x04001852 RID: 6226
		[MMFInspectorGroup("Post Exposure", true, 15, false, false)]
		[Tooltip("the curve used to animate the focus distance value on")]
		public AnimationCurve ShakePostExposure = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001853 RID: 6227
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapPostExposureZero;

		// Token: 0x04001854 RID: 6228
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapPostExposureOne = 1f;

		// Token: 0x04001855 RID: 6229
		[MMFInspectorGroup("Hue Shift", true, 14, false, false)]
		[Tooltip("the curve used to animate the aperture value on")]
		public AnimationCurve ShakeHueShift = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001856 RID: 6230
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-180f, 180f)]
		public float RemapHueShiftZero;

		// Token: 0x04001857 RID: 6231
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-180f, 180f)]
		public float RemapHueShiftOne = 180f;

		// Token: 0x04001858 RID: 6232
		[MMFInspectorGroup("Saturation", true, 13, false, false)]
		[Tooltip("the curve used to animate the focal length value on")]
		public AnimationCurve ShakeSaturation = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001859 RID: 6233
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-100f, 100f)]
		public float RemapSaturationZero;

		// Token: 0x0400185A RID: 6234
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-100f, 100f)]
		public float RemapSaturationOne = 100f;

		// Token: 0x0400185B RID: 6235
		[MMFInspectorGroup("Contrast", true, 12, false, false)]
		[Tooltip("the curve used to animate the focal length value on")]
		public AnimationCurve ShakeContrast = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x0400185C RID: 6236
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-100f, 100f)]
		public float RemapContrastZero;

		// Token: 0x0400185D RID: 6237
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-100f, 100f)]
		public float RemapContrastOne = 100f;

		// Token: 0x0400185E RID: 6238
		[Header("Color Filter")]
		[Tooltip("the selected color filter mode :None : nothing will happen,gradient : evaluates the color over time on that gradient, from left to right,interpolate : lerps from the current color to the destination one ")]
		public MMColorAdjustmentsShaker_HDRP.ColorFilterModes ColorFilterMode;

		// Token: 0x0400185F RID: 6239
		[Tooltip("the gradient to use to animate the color filter over time")]
		[MMFEnumCondition("ColorFilterMode", new int[] { 1 })]
		[GradientUsage(true)]
		public Gradient ColorFilterGradient;

		// Token: 0x04001860 RID: 6240
		[Tooltip("the destination color when in interpolate mode")]
		[MMFEnumCondition("ColorFilterMode", new int[] { 2 })]
		public Color ColorFilterDestination = Color.yellow;

		// Token: 0x04001861 RID: 6241
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
