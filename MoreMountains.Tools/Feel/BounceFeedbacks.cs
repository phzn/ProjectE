using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.Feel
{
	// Token: 0x02000372 RID: 882
	[AddComponentMenu("")]
	public class BounceFeedbacks : MonoBehaviour
	{
		// Token: 0x06001845 RID: 6213 RVA: 0x000671A4 File Offset: 0x000653A4
		public virtual void PlayCharge()
		{
			MMFeedbacks chargeFeedbacks = this.ChargeFeedbacks;
			if (chargeFeedbacks == null)
			{
				return;
			}
			chargeFeedbacks.PlayFeedbacks();
		}

		// Token: 0x06001846 RID: 6214 RVA: 0x000671B6 File Offset: 0x000653B6
		public virtual void PlayJump()
		{
			MMFeedbacks jumpFeedbacks = this.JumpFeedbacks;
			if (jumpFeedbacks == null)
			{
				return;
			}
			jumpFeedbacks.PlayFeedbacks();
		}

		// Token: 0x06001847 RID: 6215 RVA: 0x000671C8 File Offset: 0x000653C8
		public virtual void PlayLanding()
		{
			MMFeedbacks landingFeedbacks = this.LandingFeedbacks;
			if (landingFeedbacks == null)
			{
				return;
			}
			landingFeedbacks.PlayFeedbacks();
		}

		// Token: 0x04001C24 RID: 7204
		public MMFeedbacks ChargeFeedbacks;

		// Token: 0x04001C25 RID: 7205
		public MMFeedbacks JumpFeedbacks;

		// Token: 0x04001C26 RID: 7206
		public MMFeedbacks LandingFeedbacks;
	}
}
