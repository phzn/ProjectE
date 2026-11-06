using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001EB RID: 491
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will 'hold', or wait, until all previous feedbacks have been executed, and will then pause the execution of your MMFeedbacks sequence, for the specified duration.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Pause/Holding Pause")]
	public class MMF_HoldingPause : MMF_Pause
	{
		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000CAD RID: 3245 RVA: 0x000371F6 File Offset: 0x000353F6
		public override bool HoldingPause
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000CAE RID: 3246 RVA: 0x000371F9 File Offset: 0x000353F9
		// (set) Token: 0x06000CAF RID: 3247 RVA: 0x00037207 File Offset: 0x00035407
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

		// Token: 0x06000CB0 RID: 3248 RVA: 0x00037210 File Offset: 0x00035410
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
