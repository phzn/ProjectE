using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x020002EF RID: 751
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback allows you to control bloom intensity and threshold over time. It requires you have in your scene an object with a Volume with Bloom active, and a MMBloomShaker_HDRP component.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.HDRP", null)]
	public class MMF_Bloom_HDRP : MMF_Feedback
	{
		// Token: 0x1700025D RID: 605
		// (get) Token: 0x0600151F RID: 5407 RVA: 0x0005A13D File Offset: 0x0005833D
		// (set) Token: 0x06001520 RID: 5408 RVA: 0x0005A14B File Offset: 0x0005834B
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

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06001521 RID: 5409 RVA: 0x0005A154 File Offset: 0x00058354
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06001522 RID: 5410 RVA: 0x0005A157 File Offset: 0x00058357
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001523 RID: 5411 RVA: 0x0005A15C File Offset: 0x0005835C
		protected override void CustomPlayFeedback(Vector3 position, float attenuation = 1f)
		{
			if (!this.Active || !MMF_Bloom_HDRP.FeedbackTypeAuthorized)
			{
				return;
			}
			attenuation = this.ComputeIntensity(attenuation, position);
			MMBloomShakeEvent_HDRP.Trigger(this.ShakeIntensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.ShakeThreshold, this.RemapThresholdZero, this.RemapThresholdOne, this.RelativeValues, attenuation, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
		}

		// Token: 0x06001524 RID: 5412 RVA: 0x0005A1DC File Offset: 0x000583DC
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Bloom_HDRP.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMBloomShakeEvent_HDRP.Trigger(this.ShakeIntensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.ShakeThreshold, this.RemapThresholdZero, this.RemapThresholdOne, this.RelativeValues, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, true, false);
		}

		// Token: 0x06001525 RID: 5413 RVA: 0x0005A248 File Offset: 0x00058448
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_Bloom_HDRP.FeedbackTypeAuthorized)
			{
				return;
			}
			MMBloomShakeEvent_HDRP.Trigger(this.ShakeIntensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.ShakeThreshold, this.RemapThresholdZero, this.RemapThresholdOne, this.RelativeValues, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, false, true);
		}

		// Token: 0x06001526 RID: 5414 RVA: 0x0005A2AB File Offset: 0x000584AB
		public override void AutomaticShakerSetup()
		{
		}

		// Token: 0x0400182B RID: 6187
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x0400182C RID: 6188
		[MMFInspectorGroup("Bloom", true, 3, false, false)]
		[Tooltip("the duration of the feedback, in seconds")]
		public float ShakeDuration = 0.2f;

		// Token: 0x0400182D RID: 6189
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x0400182E RID: 6190
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x0400182F RID: 6191
		[Tooltip("whether or not to add to the initial intensity")]
		public bool RelativeValues = true;

		// Token: 0x04001830 RID: 6192
		[MMFInspectorGroup("Intensity", true, 4, false, false)]
		[Tooltip("the curve to animate the intensity on")]
		public AnimationCurve ShakeIntensity = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001831 RID: 6193
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapIntensityZero;

		// Token: 0x04001832 RID: 6194
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapIntensityOne = 1f;

		// Token: 0x04001833 RID: 6195
		[MMFInspectorGroup("Threshold", true, 5, false, false)]
		[Tooltip("the curve to animate the threshold on")]
		public AnimationCurve ShakeThreshold = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001834 RID: 6196
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapThresholdZero;

		// Token: 0x04001835 RID: 6197
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapThresholdOne;
	}
}
