using System;
using Cinemachine;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x020002E2 RID: 738
	[AddComponentMenu("")]
	[FeedbackPath("Camera/Cinemachine Impulse Clear")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.Cinemachine", null)]
	[FeedbackHelp("This feedback lets you trigger a Cinemachine Impulse clear, stopping instantly any impulse that may be playing.")]
	public class MMF_CinemachineImpulseClear : MMF_Feedback
	{
		// Token: 0x060014AC RID: 5292 RVA: 0x000588AC File Offset: 0x00056AAC
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_CinemachineImpulseClear.FeedbackTypeAuthorized)
			{
				return;
			}
			CinemachineImpulseManager.Instance.Clear();
		}

		// Token: 0x040017A7 RID: 6055
		public static bool FeedbackTypeAuthorized = true;
	}
}
