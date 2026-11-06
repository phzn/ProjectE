using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000276 RID: 630
	[AddComponentMenu("More Mountains/Springs/MMSpringCameraFieldOfView")]
	public class MMSpringCameraFieldOfView : MMSpringFloatComponent<Camera>
	{
		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06001225 RID: 4645 RVA: 0x0004F388 File Offset: 0x0004D588
		// (set) Token: 0x06001226 RID: 4646 RVA: 0x0004F395 File Offset: 0x0004D595
		public override float TargetFloat
		{
			get
			{
				return this.Target.fieldOfView;
			}
			set
			{
				this.Target.fieldOfView = value;
			}
		}
	}
}
