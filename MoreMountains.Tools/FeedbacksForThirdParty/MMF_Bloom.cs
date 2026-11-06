using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x0200031B RID: 795
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback allows you to control bloom intensity and threshold over time. It requires you have in your scene an object with a PostProcessVolume with Bloom active, and a MMBloomShaker component.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.PostProcessing", null)]
	public class MMF_Bloom : MMF_Feedback
	{
		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06001606 RID: 5638 RVA: 0x0005D82A File Offset: 0x0005BA2A
		// (set) Token: 0x06001607 RID: 5639 RVA: 0x0005D838 File Offset: 0x0005BA38
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

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06001608 RID: 5640 RVA: 0x0005D841 File Offset: 0x0005BA41
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06001609 RID: 5641 RVA: 0x0005D844 File Offset: 0x0005BA44
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600160A RID: 5642 RVA: 0x0005D848 File Offset: 0x0005BA48
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Bloom.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			MMBloomShakeEvent.Trigger(this.ShakeIntensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.ShakeThreshold, this.RemapThresholdZero, this.RemapThresholdOne, this.RelativeValues, num, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
		}

		// Token: 0x0600160B RID: 5643 RVA: 0x0005D8C4 File Offset: 0x0005BAC4
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Bloom.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMBloomShakeEvent.Trigger(this.ShakeIntensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.ShakeThreshold, this.RemapThresholdZero, this.RemapThresholdOne, this.RelativeValues, 1f, null, true, true, true, TimescaleModes.Scaled, true, false);
		}

		// Token: 0x0600160C RID: 5644 RVA: 0x0005D92C File Offset: 0x0005BB2C
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_Bloom.FeedbackTypeAuthorized)
			{
				return;
			}
			MMBloomShakeEvent.Trigger(this.ShakeIntensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.ShakeThreshold, this.RemapThresholdZero, this.RemapThresholdOne, this.RelativeValues, 1f, null, true, true, true, TimescaleModes.Scaled, false, true);
		}

		// Token: 0x0600160D RID: 5645 RVA: 0x0005D98A File Offset: 0x0005BB8A
		public override void AutomaticShakerSetup()
		{
		}

		// Token: 0x0400194F RID: 6479
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001950 RID: 6480
		[MMFInspectorGroup("Bloom", true, 41, false, false)]
		[Tooltip("the duration of the feedback, in seconds")]
		public float ShakeDuration = 0.2f;

		// Token: 0x04001951 RID: 6481
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x04001952 RID: 6482
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x04001953 RID: 6483
		[Tooltip("whether or not to add to the initial intensity")]
		public bool RelativeValues = true;

		// Token: 0x04001954 RID: 6484
		[MMFInspectorGroup("Intensity", true, 42, false, false)]
		[Tooltip("the curve to animate the intensity on")]
		public AnimationCurve ShakeIntensity = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001955 RID: 6485
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapIntensityZero;

		// Token: 0x04001956 RID: 6486
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapIntensityOne = 10f;

		// Token: 0x04001957 RID: 6487
		[MMFInspectorGroup("Threshold", true, 43, false, false)]
		[Tooltip("the curve to animate the threshold on")]
		public AnimationCurve ShakeThreshold = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001958 RID: 6488
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapThresholdZero;

		// Token: 0x04001959 RID: 6489
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapThresholdOne;
	}
}
