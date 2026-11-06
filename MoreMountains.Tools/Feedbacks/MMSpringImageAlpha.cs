using System;
using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000278 RID: 632
	[AddComponentMenu("More Mountains/Springs/MMSpringImageAlpha")]
	public class MMSpringImageAlpha : MMSpringFloatComponent<Image>
	{
		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x0600122B RID: 4651 RVA: 0x0004F3CE File Offset: 0x0004D5CE
		// (set) Token: 0x0600122C RID: 4652 RVA: 0x0004F3E0 File Offset: 0x0004D5E0
		public override float TargetFloat
		{
			get
			{
				return this.Target.color.a;
			}
			set
			{
				this._color = this.Target.color;
				this._color.a = value;
				this.Target.color = this._color;
			}
		}

		// Token: 0x040014D0 RID: 5328
		protected Color _color;
	}
}
