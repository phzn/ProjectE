using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000315 RID: 789
	[AddComponentMenu("")]
	[FeedbackPath("Haptics/Haptics DEPRECATED!")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.NiceVibrations", null)]
	[FeedbackHelp("This feedback has been deprecated, and is just here to avoid errors in case you were to update from an old version. Use any of the new haptic feedbacks instead.")]
	public class MMF_Haptics : MMF_Feedback
	{
		// Token: 0x060015EF RID: 5615 RVA: 0x0005D35E File Offset: 0x0005B55E
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Haptics.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.OutputDeprecationWarning)
			{
				Debug.LogWarning(this.Owner.name + " : the haptic feedback on this object is using the old version of Nice Vibrations, and won't work anymore. Replace it with any of the new haptic feedbacks.");
			}
		}

		// Token: 0x0400192A RID: 6442
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x0400192B RID: 6443
		[Header("Deprecated Feedback")]
		public bool OutputDeprecationWarning = true;
	}
}
