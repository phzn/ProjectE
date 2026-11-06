using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000286 RID: 646
	[AddComponentMenu("More Mountains/Springs/MMSpringSpriteColor")]
	public class MMSpringSpriteColor : MMSpringColorComponent<SpriteRenderer>
	{
		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06001258 RID: 4696 RVA: 0x0004F867 File Offset: 0x0004DA67
		// (set) Token: 0x06001259 RID: 4697 RVA: 0x0004F874 File Offset: 0x0004DA74
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
