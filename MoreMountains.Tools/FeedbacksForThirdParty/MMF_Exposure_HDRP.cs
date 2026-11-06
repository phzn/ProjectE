using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x020002F4 RID: 756
	[AddComponentMenu("")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.HDRP", null)]
	[FeedbackHelp("This feedback allows you to control Exposure intensity over time. It requires you have in your scene an object with a Volume with Exposure active, and a MMExposureShaker_HDRP component.")]
	public class MMF_Exposure_HDRP : MMF_Feedback
	{
		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06001551 RID: 5457 RVA: 0x0005B099 File Offset: 0x00059299
		// (set) Token: 0x06001552 RID: 5458 RVA: 0x0005B0A7 File Offset: 0x000592A7
		public override float FeedbackDuration
		{
			get
			{
				return this.ApplyTimeMultiplier(this.Duration);
			}
			set
			{
				this.Duration = value;
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06001553 RID: 5459 RVA: 0x0005B0B0 File Offset: 0x000592B0
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06001554 RID: 5460 RVA: 0x0005B0B3 File Offset: 0x000592B3
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001555 RID: 5461 RVA: 0x0005B0B8 File Offset: 0x000592B8
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Exposure_HDRP.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			MMExposureShakeEvent_HDRP.Trigger(this.FixedExposure, this.FeedbackDuration, this.RemapFixedExposureZero, this.RemapFixedExposureOne, this.RelativeFixedExposure, num, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
		}

		// Token: 0x06001556 RID: 5462 RVA: 0x0005B124 File Offset: 0x00059324
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Exposure_HDRP.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMExposureShakeEvent_HDRP.Trigger(this.FixedExposure, this.FeedbackDuration, this.RemapFixedExposureZero, this.RemapFixedExposureOne, this.RelativeFixedExposure, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, true, false);
		}

		// Token: 0x06001557 RID: 5463 RVA: 0x0005B180 File Offset: 0x00059380
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_Exposure_HDRP.FeedbackTypeAuthorized)
			{
				return;
			}
			MMExposureShakeEvent_HDRP.Trigger(this.FixedExposure, this.FeedbackDuration, this.RemapFixedExposureZero, this.RemapFixedExposureOne, this.RelativeFixedExposure, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, false, true);
		}

		// Token: 0x06001558 RID: 5464 RVA: 0x0005B1D1 File Offset: 0x000593D1
		public override void AutomaticShakerSetup()
		{
		}

		// Token: 0x0400187A RID: 6266
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x0400187B RID: 6267
		[MMFInspectorGroup("Exposure", true, 17, false, false)]
		[Tooltip("the duration of the shake, in seconds")]
		public float Duration = 0.2f;

		// Token: 0x0400187C RID: 6268
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x0400187D RID: 6269
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x0400187E RID: 6270
		[MMFInspectorGroup("Intensity", true, 18, false, false)]
		[Tooltip("the curve to animate the intensity on")]
		public AnimationCurve FixedExposure = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x0400187F RID: 6271
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapFixedExposureZero = 8.5f;

		// Token: 0x04001880 RID: 6272
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapFixedExposureOne = 6f;

		// Token: 0x04001881 RID: 6273
		[Tooltip("whether or not to add to the initial intensity")]
		public bool RelativeFixedExposure;
	}
}
