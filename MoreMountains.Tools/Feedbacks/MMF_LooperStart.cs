using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001F5 RID: 501
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback can act as a pause but also as a start point for your loops. Add a FeedbackLooper below this (and after a few feedbacks) and your MMFeedbacks will loop between both.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Loop/Looper Start")]
	public class MMF_LooperStart : MMF_Pause
	{
		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000D14 RID: 3348 RVA: 0x00038C43 File Offset: 0x00036E43
		public override bool LooperStart
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000D15 RID: 3349 RVA: 0x00038C46 File Offset: 0x00036E46
		// (set) Token: 0x06000D16 RID: 3350 RVA: 0x00038C54 File Offset: 0x00036E54
		public override float FeedbackDuration
		{
			get
			{
				return this.ApplyTimeMultiplier(this.PauseDuration);
			}
			set
			{
				this.PauseDuration = value;
			}
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x00038C5D File Offset: 0x00036E5D
		protected virtual void Reset()
		{
			this.PauseDuration = 0f;
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x00038C6A File Offset: 0x00036E6A
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (this.Active)
			{
				this.ProcessNewPauseDuration();
				this.Owner.StartCoroutine(this.PlayPause());
			}
		}
	}
}
