using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200027C RID: 636
	[AddComponentMenu("More Mountains/Springs/MMSpringLightIntensity")]
	public class MMSpringLightIntensity : MMSpringFloatComponent<Light>
	{
		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06001237 RID: 4663 RVA: 0x0004F481 File Offset: 0x0004D681
		// (set) Token: 0x06001238 RID: 4664 RVA: 0x0004F48E File Offset: 0x0004D68E
		public override float TargetFloat
		{
			get
			{
				return this.Target.intensity;
			}
			set
			{
				this.Target.intensity = value;
			}
		}
	}
}
