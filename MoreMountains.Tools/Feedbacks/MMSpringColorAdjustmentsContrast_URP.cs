using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002CC RID: 716
	[AddComponentMenu("More Mountains/Springs/MMSpringColorAdjustmentsContrast_URP")]
	public class MMSpringColorAdjustmentsContrast_URP : MMSpringFloatComponent<Volume>
	{
		// Token: 0x06001444 RID: 5188 RVA: 0x0005762E File Offset: 0x0005582E
		protected override void Initialization()
		{
			if (this.Target == null)
			{
				this.Target = base.gameObject.GetComponent<Volume>();
			}
			this.Target.profile.TryGet<ColorAdjustments>(out this._colorAdjustments);
			base.Initialization();
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06001445 RID: 5189 RVA: 0x0005766C File Offset: 0x0005586C
		// (set) Token: 0x06001446 RID: 5190 RVA: 0x0005767E File Offset: 0x0005587E
		public override float TargetFloat
		{
			get
			{
				return this._colorAdjustments.contrast.value;
			}
			set
			{
				this._colorAdjustments.contrast.Override(value);
			}
		}

		// Token: 0x04001756 RID: 5974
		protected ColorAdjustments _colorAdjustments;
	}
}
