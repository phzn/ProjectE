using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001D6 RID: 470
	[AddComponentMenu("")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Audio/AudioSource Pitch")]
	[FeedbackHelp("This feedback lets you control the pitch of a target AudioSource over time.")]
	public class MMF_AudioSourcePitch : MMF_Feedback
	{
		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000BEB RID: 3051 RVA: 0x00034256 File Offset: 0x00032456
		// (set) Token: 0x06000BEC RID: 3052 RVA: 0x00034264 File Offset: 0x00032464
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

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000BED RID: 3053 RVA: 0x0003426D File Offset: 0x0003246D
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000BEE RID: 3054 RVA: 0x00034270 File Offset: 0x00032470
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x00034274 File Offset: 0x00032474
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_AudioSourcePitch.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			MMAudioSourcePitchShakeEvent.Trigger(this.PitchTween, this.FeedbackDuration, this.RemapPitchZero, this.RemapPitchOne, this.RelativePitch, num, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x000342E0 File Offset: 0x000324E0
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_AudioSourcePitch.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMAudioSourcePitchShakeEvent.Trigger(this.PitchTween, this.FeedbackDuration, this.RemapPitchZero, this.RemapPitchOne, this.RelativePitch, 1f, null, true, true, true, TimescaleModes.Scaled, true, false);
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x00034334 File Offset: 0x00032534
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_AudioSourcePitch.FeedbackTypeAuthorized)
			{
				return;
			}
			MMAudioSourcePitchShakeEvent.Trigger(this.PitchTween, this.FeedbackDuration, this.RemapPitchZero, this.RemapPitchOne, this.RelativePitch, 1f, null, true, true, true, TimescaleModes.Scaled, false, true);
		}

		// Token: 0x04000DA4 RID: 3492
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000DA5 RID: 3493
		[MMFInspectorGroup("AudioSource Pitch", true, 77, false, false)]
		[Tooltip("the duration of the shake, in seconds")]
		public float Duration = 2f;

		// Token: 0x04000DA6 RID: 3494
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x04000DA7 RID: 3495
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x04000DA8 RID: 3496
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativePitch;

		// Token: 0x04000DA9 RID: 3497
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve PitchTween = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 1f),
			new Keyframe(0.5f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04000DAA RID: 3498
		[Range(-3f, 3f)]
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapPitchZero;

		// Token: 0x04000DAB RID: 3499
		[Range(-3f, 3f)]
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapPitchOne = 1f;
	}
}
