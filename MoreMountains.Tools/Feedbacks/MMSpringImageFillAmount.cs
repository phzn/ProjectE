using System;
using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200027A RID: 634
	[AddComponentMenu("More Mountains/Springs/MMSpringImageFillAmount")]
	public class MMSpringImageFillAmount : MMSpringFloatComponent<Image>
	{
		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06001231 RID: 4657 RVA: 0x0004F43B File Offset: 0x0004D63B
		// (set) Token: 0x06001232 RID: 4658 RVA: 0x0004F448 File Offset: 0x0004D648
		public override float TargetFloat
		{
			get
			{
				return this.Target.fillAmount;
			}
			set
			{
				this.Target.fillAmount = value;
			}
		}
	}
}
