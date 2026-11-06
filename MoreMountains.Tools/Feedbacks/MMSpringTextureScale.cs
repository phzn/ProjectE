using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000289 RID: 649
	[AddComponentMenu("More Mountains/Springs/MMSpringTextureScale")]
	public class MMSpringTextureScale : MMSpringVector2Component<Renderer>
	{
		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06001262 RID: 4706 RVA: 0x0004FB3D File Offset: 0x0004DD3D
		// (set) Token: 0x06001263 RID: 4707 RVA: 0x0004FB4F File Offset: 0x0004DD4F
		public override Vector2 TargetVector2
		{
			get
			{
				return this.Target.material.mainTextureScale;
			}
			set
			{
				this.Target.material.mainTextureScale = value;
			}
		}
	}
}
