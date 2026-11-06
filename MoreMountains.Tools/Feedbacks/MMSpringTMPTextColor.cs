using System;
using TMPro;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000290 RID: 656
	[AddComponentMenu("More Mountains/Springs/MMSpringTMPTextColor")]
	public class MMSpringTMPTextColor : MMSpringColorComponent<TMP_Text>
	{
		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06001277 RID: 4727 RVA: 0x0004FC8A File Offset: 0x0004DE8A
		// (set) Token: 0x06001278 RID: 4728 RVA: 0x0004FC97 File Offset: 0x0004DE97
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
