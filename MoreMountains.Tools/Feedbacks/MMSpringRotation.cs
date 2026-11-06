using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000282 RID: 642
	[AddComponentMenu("More Mountains/Springs/MMSpringRotation")]
	public class MMSpringRotation : MMSpringVector3Component<Transform>
	{
		// Token: 0x170001DE RID: 478
		// (get) Token: 0x0600124A RID: 4682 RVA: 0x0004F5E4 File Offset: 0x0004D7E4
		// (set) Token: 0x0600124B RID: 4683 RVA: 0x0004F620 File Offset: 0x0004D820
		public override Vector3 TargetVector3
		{
			get
			{
				if (this.Space != MMSpringRotation.Spaces.Local)
				{
					return this.Target.rotation.eulerAngles;
				}
				return this.Target.localRotation.eulerAngles;
			}
			set
			{
				if (this.Space == MMSpringRotation.Spaces.Local)
				{
					this.Target.localRotation = Quaternion.Euler(value);
					return;
				}
				this.Target.rotation = Quaternion.Euler(value);
			}
		}

		// Token: 0x040014D2 RID: 5330
		[MMInspectorGroup("Target", true, 17, false)]
		public MMSpringRotation.Spaces Space = MMSpringRotation.Spaces.World;

		// Token: 0x02000529 RID: 1321
		public enum Spaces
		{
			// Token: 0x040023AF RID: 9135
			Local,
			// Token: 0x040023B0 RID: 9136
			World
		}
	}
}
