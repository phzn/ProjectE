using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x020002F1 RID: 753
	[AddComponentMenu("")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.HDRP", null)]
	[FeedbackHelp("This feedback allows you to control chromatic aberration intensity over time. It requires you have in your scene an object with a Volume with Chromatic Aberration active, and a MMChromaticAberrationShaker_HDRP component.")]
	public class MMF_ChromaticAberration_HDRP : MMF_Feedback
	{
		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06001533 RID: 5427 RVA: 0x0005A699 File Offset: 0x00058899
		// (set) Token: 0x06001534 RID: 5428 RVA: 0x0005A6A7 File Offset: 0x000588A7
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

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06001535 RID: 5429 RVA: 0x0005A6B0 File Offset: 0x000588B0
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06001536 RID: 5430 RVA: 0x0005A6B3 File Offset: 0x000588B3
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001537 RID: 5431 RVA: 0x0005A6B8 File Offset: 0x000588B8
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_ChromaticAberration_HDRP.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			MMChromaticAberrationShakeEvent_HDRP.Trigger(this.Intensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, num, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
		}

		// Token: 0x06001538 RID: 5432 RVA: 0x0005A724 File Offset: 0x00058924
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_ChromaticAberration_HDRP.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMChromaticAberrationShakeEvent_HDRP.Trigger(this.Intensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, true, false);
		}

		// Token: 0x06001539 RID: 5433 RVA: 0x0005A780 File Offset: 0x00058980
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_ChromaticAberration_HDRP.FeedbackTypeAuthorized)
			{
				return;
			}
			MMChromaticAberrationShakeEvent_HDRP.Trigger(this.Intensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, false, true);
		}

		// Token: 0x0600153A RID: 5434 RVA: 0x0005A7D1 File Offset: 0x000589D1
		public override void AutomaticShakerSetup()
		{
		}

		// Token: 0x04001844 RID: 6212
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001845 RID: 6213
		[MMFInspectorGroup("Chromatic Aberration", true, 10, false, false)]
		[Tooltip("the duration of the shake, in seconds")]
		public float Duration = 0.2f;

		// Token: 0x04001846 RID: 6214
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x04001847 RID: 6215
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x04001848 RID: 6216
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(0f, 1f)]
		public float RemapIntensityZero;

		// Token: 0x04001849 RID: 6217
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(0f, 1f)]
		public float RemapIntensityOne = 1f;

		// Token: 0x0400184A RID: 6218
		[MMFInspectorGroup("Intensity", true, 11, false, false)]
		[Tooltip("the curve to animate the intensity on")]
		public AnimationCurve Intensity = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x0400184B RID: 6219
		[Tooltip("the multiplier to apply to the intensity curve")]
		[Range(0f, 1f)]
		public float Amplitude = 1f;

		// Token: 0x0400184C RID: 6220
		[Tooltip("whether or not to add to the initial intensity")]
		public bool RelativeIntensity;
	}
}
