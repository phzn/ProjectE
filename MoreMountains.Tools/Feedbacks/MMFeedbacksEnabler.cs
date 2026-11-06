using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001A7 RID: 423
	[AddComponentMenu("")]
	public class MMFeedbacksEnabler : MonoBehaviour
	{
		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000A49 RID: 2633 RVA: 0x0002F275 File Offset: 0x0002D475
		// (set) Token: 0x06000A4A RID: 2634 RVA: 0x0002F27D File Offset: 0x0002D47D
		public MMFeedbacks TargetMMFeedbacks { get; set; }

		// Token: 0x06000A4B RID: 2635 RVA: 0x0002F286 File Offset: 0x0002D486
		protected virtual void OnEnable()
		{
			if (this.TargetMMFeedbacks != null && !this.TargetMMFeedbacks.enabled && this.TargetMMFeedbacks.AutoPlayOnEnable)
			{
				this.TargetMMFeedbacks.enabled = true;
			}
		}
	}
}
