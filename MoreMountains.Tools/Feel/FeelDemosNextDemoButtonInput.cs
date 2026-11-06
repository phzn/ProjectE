using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.Feel
{
	// Token: 0x0200038E RID: 910
	public class FeelDemosNextDemoButtonInput : MonoBehaviour
	{
		// Token: 0x060018D5 RID: 6357 RVA: 0x00069890 File Offset: 0x00067A90
		protected virtual void Update()
		{
			if (FeelDemosInputHelper.CheckEnterPressedThisFrame())
			{
				MMFeedbacks onInputFeedback = this.OnInputFeedback;
				if (onInputFeedback == null)
				{
					return;
				}
				onInputFeedback.PlayFeedbacks();
			}
		}

		// Token: 0x04001CFA RID: 7418
		public MMFeedbacks OnInputFeedback;
	}
}
