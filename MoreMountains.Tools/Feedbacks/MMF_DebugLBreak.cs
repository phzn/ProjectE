using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000296 RID: 662
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will will force a break, pausing the editor")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.MMTools", null)]
	[FeedbackPath("Debug/Break")]
	public class MMF_DebugLBreak : MMF_Feedback
	{
		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06001297 RID: 4759 RVA: 0x0005016E File Offset: 0x0004E36E
		public override float FeedbackDuration
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06001298 RID: 4760 RVA: 0x00050175 File Offset: 0x0004E375
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_DebugLBreak.FeedbackTypeAuthorized)
			{
				return;
			}
			Debug.Break();
		}

		// Token: 0x040014F1 RID: 5361
		public static bool FeedbackTypeAuthorized = true;
	}
}
