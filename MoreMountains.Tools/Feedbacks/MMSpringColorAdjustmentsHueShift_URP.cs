using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002CD RID: 717
	[AddComponentMenu("More Mountains/Springs/MMSpringColorAdjustmentsHueShift_URP")]
	public class MMSpringColorAdjustmentsHueShift_URP : MMSpringFloatComponent<Volume>
	{
		// Token: 0x06001448 RID: 5192 RVA: 0x00057699 File Offset: 0x00055899
		protected override void Initialization()
		{
			if (this.Target == null)
			{
				this.Target = base.gameObject.GetComponent<Volume>();
			}
			this.Target.profile.TryGet<ColorAdjustments>(out this._colorAdjustments);
			base.Initialization();
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06001449 RID: 5193 RVA: 0x000576D7 File Offset: 0x000558D7
		// (set) Token: 0x0600144A RID: 5194 RVA: 0x000576E9 File Offset: 0x000558E9
		public override float TargetFloat
		{
			get
			{
				return this._colorAdjustments.hueShift.value;
			}
			set
			{
				this._colorAdjustments.hueShift.Override(value);
			}
		}

		// Token: 0x04001757 RID: 5975
		protected ColorAdjustments _colorAdjustments;
	}
}
