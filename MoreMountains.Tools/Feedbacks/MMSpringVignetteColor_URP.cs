using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002D4 RID: 724
	[AddComponentMenu("More Mountains/Springs/MMSpringVignetteColor_URP")]
	public class MMSpringVignetteColor_URP : MMSpringColorComponent<Volume>
	{
		// Token: 0x06001464 RID: 5220 RVA: 0x00057986 File Offset: 0x00055B86
		protected override void Initialization()
		{
			if (this.Target == null)
			{
				this.Target = base.gameObject.GetComponent<Volume>();
			}
			this.Target.profile.TryGet<Vignette>(out this._vignette);
			base.Initialization();
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06001465 RID: 5221 RVA: 0x000579C4 File Offset: 0x00055BC4
		// (set) Token: 0x06001466 RID: 5222 RVA: 0x000579D6 File Offset: 0x00055BD6
		public override Color TargetColor
		{
			get
			{
				return this._vignette.color.value;
			}
			set
			{
				this._vignette.color.Override(value);
			}
		}

		// Token: 0x0400175E RID: 5982
		protected Vignette _vignette;
	}
}
