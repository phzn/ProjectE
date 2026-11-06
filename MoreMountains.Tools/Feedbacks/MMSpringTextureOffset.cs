using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000288 RID: 648
	[AddComponentMenu("More Mountains/Springs/MMSpringTextureOffset")]
	public class MMSpringTextureOffset : MMSpringVector2Component<Renderer>
	{
		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x0600125F RID: 4703 RVA: 0x0004FB10 File Offset: 0x0004DD10
		// (set) Token: 0x06001260 RID: 4704 RVA: 0x0004FB22 File Offset: 0x0004DD22
		public override Vector2 TargetVector2
		{
			get
			{
				return this.Target.material.mainTextureOffset;
			}
			set
			{
				this.Target.material.mainTextureOffset = value;
			}
		}
	}
}
