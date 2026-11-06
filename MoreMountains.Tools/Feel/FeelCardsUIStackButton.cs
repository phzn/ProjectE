using System;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.Feel
{
	// Token: 0x02000377 RID: 887
	[AddComponentMenu("")]
	public class FeelCardsUIStackButton : MonoBehaviour
	{
		// Token: 0x06001858 RID: 6232 RVA: 0x0006790C File Offset: 0x00065B0C
		public virtual void Stack()
		{
			bool flag = false;
			using (List<MMFeedbacks>.Enumerator enumerator = this.BlockerFeedbacks.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsPlaying)
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				return;
			}
			MMFeedbacks stackFeedback = this.StackFeedback;
			if (stackFeedback != null)
			{
				stackFeedback.PlayFeedbacks();
			}
			base.gameObject.SetActive(false);
		}

		// Token: 0x04001C54 RID: 7252
		public MMFeedbacks StackFeedback;

		// Token: 0x04001C55 RID: 7253
		public List<MMFeedbacks> BlockerFeedbacks;
	}
}
