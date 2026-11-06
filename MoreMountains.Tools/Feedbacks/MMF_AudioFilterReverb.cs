using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001D3 RID: 467
	[AddComponentMenu("")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Audio/Audio Filter Reverb")]
	[FeedbackHelp("This feedback lets you control a low pass audio filter over time. You'll need a MMAudioFilterReverbShaker on your filter.")]
	public class MMF_AudioFilterReverb : MMF_Feedback
	{
		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000BD5 RID: 3029 RVA: 0x00033E22 File Offset: 0x00032022
		// (set) Token: 0x06000BD6 RID: 3030 RVA: 0x00033E30 File Offset: 0x00032030
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

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000BD7 RID: 3031 RVA: 0x00033E39 File Offset: 0x00032039
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000BD8 RID: 3032 RVA: 0x00033E3C File Offset: 0x0003203C
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000BD9 RID: 3033 RVA: 0x00033E40 File Offset: 0x00032040
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_AudioFilterReverb.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			MMAudioFilterReverbShakeEvent.Trigger(this.ShakeReverb, this.FeedbackDuration, this.RemapReverbZero, this.RemapReverbOne, this.RelativeReverb, num, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
		}

		// Token: 0x06000BDA RID: 3034 RVA: 0x00033EAC File Offset: 0x000320AC
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_AudioFilterReverb.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMAudioFilterReverbShakeEvent.Trigger(this.ShakeReverb, this.FeedbackDuration, this.RemapReverbZero, this.RemapReverbOne, false, 1f, null, true, true, true, TimescaleModes.Scaled, true, false);
		}

		// Token: 0x06000BDB RID: 3035 RVA: 0x00033EFC File Offset: 0x000320FC
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_AudioFilterReverb.FeedbackTypeAuthorized)
			{
				return;
			}
			MMAudioFilterReverbShakeEvent.Trigger(this.ShakeReverb, this.FeedbackDuration, this.RemapReverbZero, this.RemapReverbOne, false, 1f, null, true, true, true, TimescaleModes.Scaled, false, true);
		}

		// Token: 0x04000D8D RID: 3469
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000D8E RID: 3470
		[MMFInspectorGroup("Reverb Filter", true, 28, false, false)]
		[Tooltip("the duration of the shake, in seconds")]
		public float Duration = 2f;

		// Token: 0x04000D8F RID: 3471
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x04000D90 RID: 3472
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x04000D91 RID: 3473
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeReverb;

		// Token: 0x04000D92 RID: 3474
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve ShakeReverb = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04000D93 RID: 3475
		[Range(-10000f, 2000f)]
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapReverbZero = -10000f;

		// Token: 0x04000D94 RID: 3476
		[Range(-10000f, 2000f)]
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapReverbOne = 2000f;
	}
}
