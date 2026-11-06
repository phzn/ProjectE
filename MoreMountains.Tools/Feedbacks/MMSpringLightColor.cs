using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200027B RID: 635
	[AddComponentMenu("More Mountains/Springs/MMSpringLightColor")]
	public class MMSpringLightColor : MMSpringColorComponent<Light>
	{
		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06001234 RID: 4660 RVA: 0x0004F45E File Offset: 0x0004D65E
		// (set) Token: 0x06001235 RID: 4661 RVA: 0x0004F46B File Offset: 0x0004D66B
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
