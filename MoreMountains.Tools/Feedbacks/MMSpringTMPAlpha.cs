using System;
using TMPro;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200028A RID: 650
	[AddComponentMenu("More Mountains/Springs/MMSpringTMPAlpha")]
	public class MMSpringTMPAlpha : MMSpringFloatComponent<TMP_Text>
	{
		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06001265 RID: 4709 RVA: 0x0004FB6A File Offset: 0x0004DD6A
		// (set) Token: 0x06001266 RID: 4710 RVA: 0x0004FB77 File Offset: 0x0004DD77
		public override float TargetFloat
		{
			get
			{
				return this.Target.alpha;
			}
			set
			{
				this.Target.alpha = value;
			}
		}
	}
}
