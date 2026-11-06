using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001D7 RID: 471
	[AddComponentMenu("")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Audio/AudioSource Stereo Pan")]
	[FeedbackHelp("This feedback lets you control the stereo pan of a target AudioSource over time.")]
	public class MMF_AudioSourceStereoPan : MMF_Feedback
	{
		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000BF4 RID: 3060 RVA: 0x00034412 File Offset: 0x00032612
		// (set) Token: 0x06000BF5 RID: 3061 RVA: 0x00034420 File Offset: 0x00032620
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

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000BF6 RID: 3062 RVA: 0x00034429 File Offset: 0x00032629
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000BF7 RID: 3063 RVA: 0x0003442C File Offset: 0x0003262C
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x00034430 File Offset: 0x00032630
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_AudioSourceStereoPan.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			MMAudioSourceStereoPanShakeEvent.Trigger(this.ShakeStereoPan, this.FeedbackDuration, this.RemapStereoPanZero, this.RemapStereoPanOne, this.RelativeStereoPan, num, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
		}

		// Token: 0x06000BF9 RID: 3065 RVA: 0x0003449C File Offset: 0x0003269C
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_AudioSourceStereoPan.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMAudioSourceStereoPanShakeEvent.Trigger(this.ShakeStereoPan, this.FeedbackDuration, this.RemapStereoPanZero, this.RemapStereoPanOne, false, 1f, null, true, true, true, TimescaleModes.Scaled, true, false);
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x000344EC File Offset: 0x000326EC
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_AudioSourceStereoPan.FeedbackTypeAuthorized)
			{
				return;
			}
			MMAudioSourceStereoPanShakeEvent.Trigger(this.ShakeStereoPan, this.FeedbackDuration, this.RemapStereoPanZero, this.RemapStereoPanOne, false, 1f, null, true, true, true, TimescaleModes.Scaled, false, true);
		}

		// Token: 0x04000DAC RID: 3500
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000DAD RID: 3501
		[MMFInspectorGroup("AudioSource Stereo Pan", true, 48, false, false)]
		[Tooltip("the duration of the shake, in seconds")]
		public float Duration = 2f;

		// Token: 0x04000DAE RID: 3502
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x04000DAF RID: 3503
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x04000DB0 RID: 3504
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeStereoPan;

		// Token: 0x04000DB1 RID: 3505
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve ShakeStereoPan = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(0.6f, -1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04000DB2 RID: 3506
		[Range(-1f, 1f)]
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapStereoPanZero;

		// Token: 0x04000DB3 RID: 3507
		[Range(-1f, 1f)]
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapStereoPanOne = 1f;
	}
}
