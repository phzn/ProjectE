using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000273 RID: 627
	[AddComponentMenu("More Mountains/Springs/MMSpringAnimatorSpeed")]
	public class MMSpringAnimatorSpeed : MMSpringFloatComponent<Animator>
	{
		// Token: 0x170001CF RID: 463
		// (get) Token: 0x0600121C RID: 4636 RVA: 0x0004F31A File Offset: 0x0004D51A
		// (set) Token: 0x0600121D RID: 4637 RVA: 0x0004F327 File Offset: 0x0004D527
		public override float TargetFloat
		{
			get
			{
				return this.Target.speed;
			}
			set
			{
				this.Target.speed = Mathf.Abs(value);
			}
		}
	}
}
