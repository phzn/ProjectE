using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001D0 RID: 464
	[AddComponentMenu("")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Audio/Audio Filter Echo")]
	[FeedbackHelp("This feedback lets you control an echo audio filter's wet mix value over time. You'll need a MMAudioFilterEchoShaker on your filter.")]
	public class MMF_AudioFilterEcho : MMF_Feedback
	{
		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000BBA RID: 3002 RVA: 0x000338FE File Offset: 0x00031AFE
		// (set) Token: 0x06000BBB RID: 3003 RVA: 0x0003390C File Offset: 0x00031B0C
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

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000BBC RID: 3004 RVA: 0x00033915 File Offset: 0x00031B15
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000BBD RID: 3005 RVA: 0x00033918 File Offset: 0x00031B18
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x0003391C File Offset: 0x00031B1C
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_AudioFilterEcho.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			MMAudioFilterEchoShakeEvent.Trigger(this.ShakeEcho, this.FeedbackDuration, this.RemapEchoZero, this.RemapEchoOne, this.RelativeEcho, num, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x00033988 File Offset: 0x00031B88
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_AudioFilterEcho.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMAudioFilterEchoShakeEvent.Trigger(this.ShakeEcho, this.FeedbackDuration, this.RemapEchoZero, this.RemapEchoOne, this.RelativeEcho, 1f, null, true, true, true, TimescaleModes.Scaled, true, false);
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x000339DC File Offset: 0x00031BDC
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_AudioFilterEcho.FeedbackTypeAuthorized)
			{
				return;
			}
			MMAudioFilterEchoShakeEvent.Trigger(this.ShakeEcho, this.FeedbackDuration, this.RemapEchoZero, this.RemapEchoOne, this.RelativeEcho, 1f, null, true, true, true, TimescaleModes.Scaled, false, true);
		}

		// Token: 0x04000D75 RID: 3445
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000D76 RID: 3446
		[MMFInspectorGroup("Echo Filter", true, 28, false, false)]
		[Tooltip("the duration of the shake, in seconds")]
		public float Duration = 2f;

		// Token: 0x04000D77 RID: 3447
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x04000D78 RID: 3448
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x04000D79 RID: 3449
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeEcho;

		// Token: 0x04000D7A RID: 3450
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve ShakeEcho = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04000D7B RID: 3451
		[Range(0f, 1f)]
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapEchoZero;

		// Token: 0x04000D7C RID: 3452
		[Range(0f, 1f)]
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapEchoOne = 1f;
	}
}
