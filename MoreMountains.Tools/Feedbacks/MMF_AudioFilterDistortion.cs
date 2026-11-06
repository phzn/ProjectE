using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001CF RID: 463
	[AddComponentMenu("")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Audio/Audio Filter Distortion")]
	[FeedbackHelp("This feedback lets you control a distortion audio filter over time. You'll need a MMAudioFilterDistortionShaker on the filter.")]
	public class MMF_AudioFilterDistortion : MMF_Feedback
	{
		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000BB1 RID: 2993 RVA: 0x00033729 File Offset: 0x00031929
		// (set) Token: 0x06000BB2 RID: 2994 RVA: 0x00033737 File Offset: 0x00031937
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

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000BB3 RID: 2995 RVA: 0x00033740 File Offset: 0x00031940
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000BB4 RID: 2996 RVA: 0x00033743 File Offset: 0x00031943
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x00033748 File Offset: 0x00031948
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_AudioFilterDistortion.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			float num2 = 0f;
			float num3 = 0f;
			if (!this.Timing.ConstantIntensity)
			{
				num2 = this.RemapDistortionZero * num;
				num3 = this.RemapDistortionOne * num;
			}
			MMAudioFilterDistortionShakeEvent.Trigger(this.ShakeDistortion, this.FeedbackDuration, num2, num3, this.RelativeDistortion, num, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x000337D4 File Offset: 0x000319D4
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			base.CustomStopFeedback(position, feedbacksIntensity);
			if (!this.Active || !MMF_AudioFilterDistortion.FeedbackTypeAuthorized)
			{
				return;
			}
			MMAudioFilterDistortionShakeEvent.Trigger(this.ShakeDistortion, this.FeedbackDuration, 0f, 0f, false, 1f, null, true, true, true, TimescaleModes.Scaled, true, false);
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x00033824 File Offset: 0x00031A24
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_AudioFilterDistortion.FeedbackTypeAuthorized)
			{
				return;
			}
			MMAudioFilterDistortionShakeEvent.Trigger(this.ShakeDistortion, this.FeedbackDuration, 0f, 0f, false, 1f, null, true, true, true, TimescaleModes.Scaled, false, true);
		}

		// Token: 0x04000D6D RID: 3437
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000D6E RID: 3438
		[MMFInspectorGroup("Distortion Filter", true, 28, false, false)]
		[Tooltip("the duration of the shake, in seconds")]
		public float Duration = 2f;

		// Token: 0x04000D6F RID: 3439
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x04000D70 RID: 3440
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x04000D71 RID: 3441
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeDistortion;

		// Token: 0x04000D72 RID: 3442
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve ShakeDistortion = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04000D73 RID: 3443
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(0f, 1f)]
		public float RemapDistortionZero;

		// Token: 0x04000D74 RID: 3444
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(0f, 1f)]
		public float RemapDistortionOne = 1f;
	}
}
