using System;
using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000279 RID: 633
	[AddComponentMenu("More Mountains/Springs/MMSpringImageColor")]
	public class MMSpringImageColor : MMSpringColorComponent<Image>
	{
		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x0600122E RID: 4654 RVA: 0x0004F418 File Offset: 0x0004D618
		// (set) Token: 0x0600122F RID: 4655 RVA: 0x0004F425 File Offset: 0x0004D625
		public override Color TargetColor
		{
			get
			{
				return this.Target.color;
			}
			set
			{
				this.Target.color = value;
			}
		}
	}
}
