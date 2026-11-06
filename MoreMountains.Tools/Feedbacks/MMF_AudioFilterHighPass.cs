using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001D1 RID: 465
	[AddComponentMenu("")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Audio/Audio Filter High Pass")]
	[FeedbackHelp("This feedback lets you control a high pass audio filter over time. You'll need a MMAudioFilterHighPassShaker on your filter.")]
	public class MMF_AudioFilterHighPass : MMF_Feedback
	{
		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000BC3 RID: 3011 RVA: 0x00033ABA File Offset: 0x00031CBA
		// (set) Token: 0x06000BC4 RID: 3012 RVA: 0x00033AC8 File Offset: 0x00031CC8
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

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000BC5 RID: 3013 RVA: 0x00033AD1 File Offset: 0x00031CD1
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000BC6 RID: 3014 RVA: 0x00033AD4 File Offset: 0x00031CD4
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000BC7 RID: 3015 RVA: 0x00033AD8 File Offset: 0x00031CD8
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_AudioFilterHighPass.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			MMAudioFilterHighPassShakeEvent.Trigger(this.ShakeHighPass, this.FeedbackDuration, this.RemapHighPassZero, this.RemapHighPassOne, this.RelativeHighPass, num, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
		}

		// Token: 0x06000BC8 RID: 3016 RVA: 0x00033B44 File Offset: 0x00031D44
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_AudioFilterHighPass.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMAudioFilterHighPassShakeEvent.Trigger(this.ShakeHighPass, this.FeedbackDuration, this.RemapHighPassZero, this.RemapHighPassOne, false, 1f, null, true, true, true, TimescaleModes.Scaled, true, false);
		}

		// Token: 0x06000BC9 RID: 3017 RVA: 0x00033B94 File Offset: 0x00031D94
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_AudioFilterHighPass.FeedbackTypeAuthorized)
			{
				return;
			}
			MMAudioFilterHighPassShakeEvent.Trigger(this.ShakeHighPass, this.FeedbackDuration, this.RemapHighPassZero, this.RemapHighPassOne, false, 1f, null, true, true, true, TimescaleModes.Scaled, false, true);
		}

		// Token: 0x04000D7D RID: 3453
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000D7E RID: 3454
		[MMFInspectorGroup("High Pass Filter", true, 28, false, false)]
		[Tooltip("the duration of the shake, in seconds")]
		public float Duration = 2f;

		// Token: 0x04000D7F RID: 3455
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x04000D80 RID: 3456
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x04000D81 RID: 3457
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeHighPass;

		// Token: 0x04000D82 RID: 3458
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve ShakeHighPass = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04000D83 RID: 3459
		[Range(10f, 22000f)]
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapHighPassZero;

		// Token: 0x04000D84 RID: 3460
		[Range(10f, 22000f)]
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapHighPassOne = 10000f;
	}
}
