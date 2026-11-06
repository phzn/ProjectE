using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002D3 RID: 723
	[AddComponentMenu("More Mountains/Springs/MMSpringVignetteCenter_URP")]
	public class MMSpringVignetteCenter_URP : MMSpringVector2Component<Volume>
	{
		// Token: 0x06001460 RID: 5216 RVA: 0x0005791B File Offset: 0x00055B1B
		protected override void Initialization()
		{
			if (this.Target == null)
			{
				this.Target = base.gameObject.GetComponent<Volume>();
			}
			this.Target.profile.TryGet<Vignette>(out this._vignette);
			base.Initialization();
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06001461 RID: 5217 RVA: 0x00057959 File Offset: 0x00055B59
		// (set) Token: 0x06001462 RID: 5218 RVA: 0x0005796B File Offset: 0x00055B6B
		public override Vector2 TargetVector2
		{
			get
			{
				return this._vignette.center.value;
			}
			set
			{
				this._vignette.center.Override(value);
			}
		}

		// Token: 0x0400175D RID: 5981
		protected Vignette _vignette;
	}
}
