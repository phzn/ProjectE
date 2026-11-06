using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002CE RID: 718
	[AddComponentMenu("More Mountains/Springs/MMSpringColorAdjustmentsSaturation_URP")]
	public class MMSpringColorAdjustmentsSaturation_URP : MMSpringFloatComponent<Volume>
	{
		// Token: 0x0600144C RID: 5196 RVA: 0x00057704 File Offset: 0x00055904
		protected override void Initialization()
		{
			if (this.Target == null)
			{
				this.Target = base.gameObject.GetComponent<Volume>();
			}
			this.Target.profile.TryGet<ColorAdjustments>(out this._colorAdjustments);
			base.Initialization();
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x0600144D RID: 5197 RVA: 0x00057742 File Offset: 0x00055942
		// (set) Token: 0x0600144E RID: 5198 RVA: 0x00057754 File Offset: 0x00055954
		public override float TargetFloat
		{
			get
			{
				return this._colorAdjustments.saturation.value;
			}
			set
			{
				this._colorAdjustments.saturation.Override(value);
			}
		}

		// Token: 0x04001758 RID: 5976
		protected ColorAdjustments _colorAdjustments;
	}
}
