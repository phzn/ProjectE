using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000277 RID: 631
	[AddComponentMenu("More Mountains/Springs/MMSpringCameraOrthographicSize")]
	public class MMSpringCameraOrthographicSize : MMSpringFloatComponent<Camera>
	{
		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06001228 RID: 4648 RVA: 0x0004F3AB File Offset: 0x0004D5AB
		// (set) Token: 0x06001229 RID: 4649 RVA: 0x0004F3B8 File Offset: 0x0004D5B8
		public override float TargetFloat
		{
			get
			{
				return this.Target.orthographicSize;
			}
			set
			{
				this.Target.orthographicSize = value;
			}
		}
	}
}
