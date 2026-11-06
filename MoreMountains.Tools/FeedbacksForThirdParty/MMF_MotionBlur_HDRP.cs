using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x020002F7 RID: 759
	[AddComponentMenu("")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.HDRP", null)]
	[FeedbackHelp("This feedback allows you to control motion blur intensity over time. It requires you have in your scene an object with a Volume with MotionBlur active, and a MMMotionBlurShaker_HDRP component.")]
	public class MMF_MotionBlur_HDRP : MMF_Feedback
	{
		// Token: 0x17000275 RID: 629
		// (get) Token: 0x0600156F RID: 5487 RVA: 0x0005B6A6 File Offset: 0x000598A6
		// (set) Token: 0x06001570 RID: 5488 RVA: 0x0005B6B4 File Offset: 0x000598B4
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

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06001571 RID: 5489 RVA: 0x0005B6BD File Offset: 0x000598BD
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06001572 RID: 5490 RVA: 0x0005B6C0 File Offset: 0x000598C0
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001573 RID: 5491 RVA: 0x0005B6C4 File Offset: 0x000598C4
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_MotionBlur_HDRP.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			MMMotionBlurShakeEvent_HDRP.Trigger(this.Intensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, num, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
		}

		// Token: 0x06001574 RID: 5492 RVA: 0x0005B730 File Offset: 0x00059930
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_MotionBlur_HDRP.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMMotionBlurShakeEvent_HDRP.Trigger(this.Intensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, true, false);
		}

		// Token: 0x06001575 RID: 5493 RVA: 0x0005B78C File Offset: 0x0005998C
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_MotionBlur_HDRP.FeedbackTypeAuthorized)
			{
				return;
			}
			MMMotionBlurShakeEvent_HDRP.Trigger(this.Intensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, false, true);
		}

		// Token: 0x06001576 RID: 5494 RVA: 0x0005B7DD File Offset: 0x000599DD
		public override void AutomaticShakerSetup()
		{
		}

		// Token: 0x04001892 RID: 6290
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001893 RID: 6291
		[MMFInspectorGroup("Motion Blur", true, 24, false, false)]
		[Tooltip("the duration of the shake, in seconds")]
		public float Duration = 0.2f;

		// Token: 0x04001894 RID: 6292
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x04001895 RID: 6293
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x04001896 RID: 6294
		[MMFInspectorGroup("Intensity", true, 25, false, false)]
		[Tooltip("the curve to animate the intensity on")]
		public AnimationCurve Intensity = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001897 RID: 6295
		[Tooltip("the value to which to remap the curve's zero to")]
		public float RemapIntensityZero;

		// Token: 0x04001898 RID: 6296
		[Tooltip("the value to which to remap the curve's one to")]
		public float RemapIntensityOne = 1000f;

		// Token: 0x04001899 RID: 6297
		[Tooltip("whether or not to add to the initial intensity")]
		public bool RelativeIntensity;
	}
}
