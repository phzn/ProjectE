using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200027F RID: 639
	[AddComponentMenu("More Mountains/Springs/MMSpringPosition")]
	public class MMSpringPosition : MMSpringVector3Component<Transform>
	{
		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06001241 RID: 4673 RVA: 0x0004F54A File Offset: 0x0004D74A
		// (set) Token: 0x06001242 RID: 4674 RVA: 0x0004F56B File Offset: 0x0004D76B
		public override Vector3 TargetVector3
		{
			get
			{
				if (this.Space != MMSpringPosition.Spaces.Local)
				{
					return this.Target.position;
				}
				return this.Target.localPosition;
			}
			set
			{
				if (this.Space == MMSpringPosition.Spaces.Local)
				{
					this.Target.localPosition = value;
					return;
				}
				this.Target.position = value;
			}
		}

		// Token: 0x040014D1 RID: 5329
		[MMInspectorGroup("Target", true, 17, false)]
		public MMSpringPosition.Spaces Space = MMSpringPosition.Spaces.World;

		// Token: 0x02000528 RID: 1320
		public enum Spaces
		{
			// Token: 0x040023AC RID: 9132
			Local,
			// Token: 0x040023AD RID: 9133
			World
		}
	}
}
