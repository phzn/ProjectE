using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002D5 RID: 725
	[AddComponentMenu("More Mountains/Springs/MMSpringVignetteIntensity_URP")]
	public class MMSpringVignetteIntensity_URP : MMSpringFloatComponent<Volume>
	{
		// Token: 0x06001468 RID: 5224 RVA: 0x000579F1 File Offset: 0x00055BF1
		protected override void Initialization()
		{
			if (this.Target == null)
			{
				this.Target = base.gameObject.GetComponent<Volume>();
			}
			this.Target.profile.TryGet<Vignette>(out this._vignette);
			base.Initialization();
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06001469 RID: 5225 RVA: 0x00057A2F File Offset: 0x00055C2F
		// (set) Token: 0x0600146A RID: 5226 RVA: 0x00057A41 File Offset: 0x00055C41
		public override float TargetFloat
		{
			get
			{
				return this._vignette.intensity.value;
			}
			set
			{
				this._vignette.intensity.Override(value);
			}
		}

		// Token: 0x0400175F RID: 5983
		protected Vignette _vignette;
	}
}
