using System;
using TMPro;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200028E RID: 654
	[AddComponentMenu("More Mountains/Springs/MMSpringTMPLineSpacing")]
	public class MMSpringTMPLineSpacing : MMSpringFloatComponent<TMP_Text>
	{
		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06001271 RID: 4721 RVA: 0x0004FC1E File Offset: 0x0004DE1E
		// (set) Token: 0x06001272 RID: 4722 RVA: 0x0004FC2B File Offset: 0x0004DE2B
		public override float TargetFloat
		{
			get
			{
				return this.Target.lineSpacing;
			}
			set
			{
				this.Target.lineSpacing = value;
			}
		}
	}
}
