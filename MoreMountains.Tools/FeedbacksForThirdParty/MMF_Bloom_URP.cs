using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000347 RID: 839
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback allows you to control bloom intensity and threshold over time. It requires you have in your scene an object with a Volume with Bloom active, and a MMBloomShaker_URP component.")]
	[FeedbackPath("PostProcess/Bloom URP")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.URP", null)]
	public class MMF_Bloom_URP : MMF_Feedback
	{
		// Token: 0x1700029F RID: 671
		// (get) Token: 0x060016E4 RID: 5860 RVA: 0x00060D07 File Offset: 0x0005EF07
		// (set) Token: 0x060016E5 RID: 5861 RVA: 0x00060D15 File Offset: 0x0005EF15
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

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x060016E6 RID: 5862 RVA: 0x00060D1E File Offset: 0x0005EF1E
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x060016E7 RID: 5863 RVA: 0x00060D21 File Offset: 0x0005EF21
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060016E8 RID: 5864 RVA: 0x00060D24 File Offset: 0x0005EF24
		protected override void CustomPlayFeedback(Vector3 position, float attenuation = 1f)
		{
			if (!this.Active || !MMF_Bloom_URP.FeedbackTypeAuthorized)
			{
				return;
			}
			attenuation = this.ComputeIntensity(attenuation, position);
			MMBloomShakeEvent_URP.Trigger(this.ShakeIntensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.ShakeThreshold, this.RemapThresholdZero, this.RemapThresholdOne, this.RelativeValues, attenuation, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
		}

		// Token: 0x060016E9 RID: 5865 RVA: 0x00060DA4 File Offset: 0x0005EFA4
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Bloom_URP.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMBloomShakeEvent_URP.Trigger(this.ShakeIntensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.ShakeThreshold, this.RemapThresholdZero, this.RemapThresholdOne, this.RelativeValues, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, true, false);
		}

		// Token: 0x060016EA RID: 5866 RVA: 0x00060E10 File Offset: 0x0005F010
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_Bloom_URP.FeedbackTypeAuthorized)
			{
				return;
			}
			MMBloomShakeEvent_URP.Trigger(this.ShakeIntensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.ShakeThreshold, this.RemapThresholdZero, this.RemapThresholdOne, this.RelativeValues, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, false, true);
		}

		// Token: 0x060016EB RID: 5867 RVA: 0x00060E73 File Offset: 0x0005F073
		public override void AutomaticShakerSetup()
		{
		}

		// Token: 0x04001A37 RID: 6711
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001A38 RID: 6712
		[MMFInspectorGroup("Bloom", true, 41, false, false)]
		[Tooltip("the duration of the feedback, in seconds")]
		public float ShakeDuration = 0.2f;

		// Token: 0x04001A39 RID: 6713
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x04001A3A RID: 6714
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x04001A3B RID: 6715
		[Tooltip("whether or not to add to the initial intensity")]
		public bool RelativeValues = true;

		// Token: 0x04001A3C RID: 6716
		[MMFInspectorGroup("Intensity", true, 42, false, false)]
		[Tooltip("the curve to animate the intensity on")]
		public AnimationCurve ShakeIntensity = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001A3D RID: 6717
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapIntensityZero;

		// Token: 0x04001A3E RID: 6718
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapIntensityOne = 1f;

		// Token: 0x04001A3F RID: 6719
		[MMFInspectorGroup("Threshold", true, 43, false, false)]
		[Tooltip("the curve to animate the threshold on")]
		public AnimationCurve ShakeThreshold = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001A40 RID: 6720
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapThresholdZero;

		// Token: 0x04001A41 RID: 6721
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapThresholdOne;
	}
}
