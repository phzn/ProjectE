using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200027D RID: 637
	[AddComponentMenu("More Mountains/Springs/MMSpringLightRange")]
	public class MMSpringLightRange : MMSpringFloatComponent<Light>
	{
		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x0600123A RID: 4666 RVA: 0x0004F4A4 File Offset: 0x0004D6A4
		// (set) Token: 0x0600123B RID: 4667 RVA: 0x0004F4B1 File Offset: 0x0004D6B1
		public override float TargetFloat
		{
			get
			{
				return this.Target.range;
			}
			set
			{
				this.Target.range = value;
			}
		}
	}
}
