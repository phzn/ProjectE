using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x020002F8 RID: 760
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback allows you to control Panini Projection distance and crop to fit over time. It requires you have in your scene an object with a Volume with PaniniProjection active, and a MMPaniniProjectionShaker_HDRP component.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.HDRP", null)]
	public class MMF_PaniniProjection_HDRP : MMF_Feedback
	{
		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06001579 RID: 5497 RVA: 0x0005B872 File Offset: 0x00059A72
		// (set) Token: 0x0600157A RID: 5498 RVA: 0x0005B880 File Offset: 0x00059A80
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

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x0600157B RID: 5499 RVA: 0x0005B889 File Offset: 0x00059A89
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x0600157C RID: 5500 RVA: 0x0005B88C File Offset: 0x00059A8C
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600157D RID: 5501 RVA: 0x0005B890 File Offset: 0x00059A90
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_PaniniProjection_HDRP.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			MMPaniniProjectionShakeEvent_HDRP.Trigger(this.ShakeDistance, this.FeedbackDuration, this.RemapDistanceZero, this.RemapDistanceOne, this.RelativeDistance, num, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
		}

		// Token: 0x0600157E RID: 5502 RVA: 0x0005B8FC File Offset: 0x00059AFC
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_PaniniProjection_HDRP.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMPaniniProjectionShakeEvent_HDRP.Trigger(this.ShakeDistance, this.FeedbackDuration, this.RemapDistanceZero, this.RemapDistanceOne, this.RelativeDistance, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, true, false);
		}

		// Token: 0x0600157F RID: 5503 RVA: 0x0005B958 File Offset: 0x00059B58
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_PaniniProjection_HDRP.FeedbackTypeAuthorized)
			{
				return;
			}
			MMPaniniProjectionShakeEvent_HDRP.Trigger(this.ShakeDistance, this.FeedbackDuration, this.RemapDistanceZero, this.RemapDistanceOne, this.RelativeDistance, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, false, true);
		}

		// Token: 0x06001580 RID: 5504 RVA: 0x0005B9A9 File Offset: 0x00059BA9
		public override void AutomaticShakerSetup()
		{
		}

		// Token: 0x0400189A RID: 6298
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x0400189B RID: 6299
		[MMFInspectorGroup("Panini Projection", true, 26, false, false)]
		[Tooltip("the duration of the shake, in seconds")]
		public float Duration = 0.2f;

		// Token: 0x0400189C RID: 6300
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x0400189D RID: 6301
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x0400189E RID: 6302
		[MMFInspectorGroup("Distance", true, 27, false, false)]
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeDistance;

		// Token: 0x0400189F RID: 6303
		[Tooltip("the curve used to animate the distance value on")]
		public AnimationCurve ShakeDistance = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040018A0 RID: 6304
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(0f, 1f)]
		public float RemapDistanceZero;

		// Token: 0x040018A1 RID: 6305
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(0f, 1f)]
		public float RemapDistanceOne = 1f;
	}
}
