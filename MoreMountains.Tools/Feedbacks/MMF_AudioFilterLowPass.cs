using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001D2 RID: 466
	[AddComponentMenu("")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Audio/Audio Filter Low Pass")]
	[FeedbackHelp("This feedback lets you control a low pass audio filter over time. You'll need a MMAudioFilterLowPassShaker on your filter.")]
	public class MMF_AudioFilterLowPass : MMF_Feedback
	{
		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000BCC RID: 3020 RVA: 0x00033C6E File Offset: 0x00031E6E
		// (set) Token: 0x06000BCD RID: 3021 RVA: 0x00033C7C File Offset: 0x00031E7C
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

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000BCE RID: 3022 RVA: 0x00033C85 File Offset: 0x00031E85
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000BCF RID: 3023 RVA: 0x00033C88 File Offset: 0x00031E88
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x00033C8C File Offset: 0x00031E8C
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_AudioFilterLowPass.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			MMAudioFilterLowPassShakeEvent.Trigger(this.ShakeLowPass, this.FeedbackDuration, this.RemapLowPassZero, this.RemapLowPassOne, this.RelativeLowPass, num, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x00033CF8 File Offset: 0x00031EF8
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_AudioFilterLowPass.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMAudioFilterLowPassShakeEvent.Trigger(this.ShakeLowPass, this.FeedbackDuration, this.RemapLowPassZero, this.RemapLowPassOne, false, 1f, null, true, true, true, TimescaleModes.Scaled, true, false);
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x00033D48 File Offset: 0x00031F48
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_AudioFilterLowPass.FeedbackTypeAuthorized)
			{
				return;
			}
			MMAudioFilterLowPassShakeEvent.Trigger(this.ShakeLowPass, this.FeedbackDuration, this.RemapLowPassZero, this.RemapLowPassOne, false, 1f, null, true, true, true, TimescaleModes.Scaled, false, true);
		}

		// Token: 0x04000D85 RID: 3461
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000D86 RID: 3462
		[MMFInspectorGroup("Low Pass Filter", true, 28, false, false)]
		[Tooltip("the duration of the shake, in seconds")]
		public float Duration = 2f;

		// Token: 0x04000D87 RID: 3463
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x04000D88 RID: 3464
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x04000D89 RID: 3465
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeLowPass;

		// Token: 0x04000D8A RID: 3466
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve ShakeLowPass = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 1f),
			new Keyframe(0.5f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04000D8B RID: 3467
		[Range(10f, 22000f)]
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapLowPassZero;

		// Token: 0x04000D8C RID: 3468
		[Range(10f, 22000f)]
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapLowPassOne = 10000f;
	}
}
