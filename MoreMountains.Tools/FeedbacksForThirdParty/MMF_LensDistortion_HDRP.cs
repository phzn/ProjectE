using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x020002F6 RID: 758
	[AddComponentMenu("")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.HDRP", null)]
	[FeedbackHelp("This feedback allows you to control HDRP lens distortion intensity over time. It requires you have in your scene an object with a Volume with Lens Distortion active, and a MMLensDistortionShaker_HDRP component.")]
	public class MMF_LensDistortion_HDRP : MMF_Feedback
	{
		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06001565 RID: 5477 RVA: 0x0005B43E File Offset: 0x0005963E
		// (set) Token: 0x06001566 RID: 5478 RVA: 0x0005B44C File Offset: 0x0005964C
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

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06001567 RID: 5479 RVA: 0x0005B455 File Offset: 0x00059655
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06001568 RID: 5480 RVA: 0x0005B458 File Offset: 0x00059658
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001569 RID: 5481 RVA: 0x0005B45C File Offset: 0x0005965C
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_LensDistortion_HDRP.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			MMLensDistortionShakeEvent_HDRP.Trigger(this.Intensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, num, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
		}

		// Token: 0x0600156A RID: 5482 RVA: 0x0005B4C8 File Offset: 0x000596C8
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_LensDistortion_HDRP.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMLensDistortionShakeEvent_HDRP.Trigger(this.Intensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, true, false);
		}

		// Token: 0x0600156B RID: 5483 RVA: 0x0005B524 File Offset: 0x00059724
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_LensDistortion_HDRP.FeedbackTypeAuthorized)
			{
				return;
			}
			MMLensDistortionShakeEvent_HDRP.Trigger(this.Intensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, false, true);
		}

		// Token: 0x0600156C RID: 5484 RVA: 0x0005B575 File Offset: 0x00059775
		public override void AutomaticShakerSetup()
		{
		}

		// Token: 0x0400188A RID: 6282
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x0400188B RID: 6283
		[MMFInspectorGroup("Lens Distortion", true, 22, false, false)]
		[Tooltip("the duration of the shake in seconds")]
		public float Duration = 0.8f;

		// Token: 0x0400188C RID: 6284
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x0400188D RID: 6285
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x0400188E RID: 6286
		[MMFInspectorGroup("Intensity", true, 23, false, false)]
		[Tooltip("whether or not to add to the initial intensity value")]
		public bool RelativeIntensity;

		// Token: 0x0400188F RID: 6287
		[Tooltip("the curve to animate the intensity on")]
		public AnimationCurve Intensity = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.2f, 1f),
			new Keyframe(0.25f, -1f),
			new Keyframe(0.35f, 0.7f),
			new Keyframe(0.4f, -0.7f),
			new Keyframe(0.6f, 0.3f),
			new Keyframe(0.65f, -0.3f),
			new Keyframe(0.8f, 0.1f),
			new Keyframe(0.85f, -0.1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001890 RID: 6288
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-1f, 1f)]
		public float RemapIntensityZero;

		// Token: 0x04001891 RID: 6289
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-1f, 1f)]
		public float RemapIntensityOne = 0.5f;
	}
}
