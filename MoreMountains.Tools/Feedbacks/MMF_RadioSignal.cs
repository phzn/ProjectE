using System;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002AC RID: 684
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you trigger a play on a target MMRadioSignal (usually used by a MMRadioBroadcaster to emit a value that can then be listened to by MMRadioReceivers. From this feedback you can also specify a duration, timescale and multiplier.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.MMTools", null)]
	[FeedbackPath("GameObject/MMRadioSignal")]
	public class MMF_RadioSignal : MMF_Feedback
	{
		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06001336 RID: 4918 RVA: 0x00052E7F File Offset: 0x0005107F
		public override float FeedbackDuration
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06001337 RID: 4919 RVA: 0x00052E86 File Offset: 0x00051086
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06001338 RID: 4920 RVA: 0x00052E89 File Offset: 0x00051089
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001339 RID: 4921 RVA: 0x00052E8C File Offset: 0x0005108C
		protected override void AutomateTargetAcquisition()
		{
			this.TargetSignal = this.FindAutomatedTarget<MMRadioSignal>();
		}

		// Token: 0x0600133A RID: 4922 RVA: 0x00052E9C File Offset: 0x0005109C
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (this.Active && MMF_RadioSignal.FeedbackTypeAuthorized && this.TargetSignal != null)
			{
				float num = this.ComputeIntensity(feedbacksIntensity, position);
				this.TargetSignal.Duration = this.Duration;
				this.TargetSignal.GlobalMultiplier = this.GlobalMultiplier * num;
				this.TargetSignal.TimeScale = this.TimeScale;
				this.TargetSignal.StartShaking();
			}
		}

		// Token: 0x0600133B RID: 4923 RVA: 0x00052F0F File Offset: 0x0005110F
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			base.CustomStopFeedback(position, feedbacksIntensity);
			if (this.Active && this.TargetSignal != null)
			{
				this.TargetSignal.Stop();
			}
		}

		// Token: 0x0600133C RID: 4924 RVA: 0x00052F3A File Offset: 0x0005113A
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_RadioSignal.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.TargetSignal != null)
			{
				this.TargetSignal.Stop();
				this.TargetSignal.ApplyLevel(0f);
			}
		}

		// Token: 0x04001626 RID: 5670
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001627 RID: 5671
		[MMFInspectorGroup("Radio Signal", true, 72, false, false)]
		[Tooltip("The target MMRadioSignal to trigger")]
		public MMRadioSignal TargetSignal;

		// Token: 0x04001628 RID: 5672
		[Tooltip("the timescale to operate on")]
		public MMRadioSignal.TimeScales TimeScale;

		// Token: 0x04001629 RID: 5673
		[Tooltip("the duration of the shake, in seconds")]
		public float Duration = 1f;

		// Token: 0x0400162A RID: 5674
		[Tooltip("a global multiplier to apply to the end result of the combination")]
		public float GlobalMultiplier = 1f;
	}
}
