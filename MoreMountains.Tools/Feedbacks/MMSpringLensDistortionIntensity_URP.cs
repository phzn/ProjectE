using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002D0 RID: 720
	[AddComponentMenu("More Mountains/Springs/MMSpringLensDistortionIntensity_URP")]
	public class MMSpringLensDistortionIntensity_URP : MMSpringFloatComponent<Volume>
	{
		// Token: 0x06001454 RID: 5204 RVA: 0x000577DA File Offset: 0x000559DA
		protected override void Initialization()
		{
			if (this.Target == null)
			{
				this.Target = base.gameObject.GetComponent<Volume>();
			}
			this.Target.profile.TryGet<LensDistortion>(out this._lensDistortion);
			base.Initialization();
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06001455 RID: 5205 RVA: 0x00057818 File Offset: 0x00055A18
		// (set) Token: 0x06001456 RID: 5206 RVA: 0x0005782A File Offset: 0x00055A2A
		public override float TargetFloat
		{
			get
			{
				return this._lensDistortion.intensity.value;
			}
			set
			{
				this._lensDistortion.intensity.Override(value);
			}
		}

		// Token: 0x0400175A RID: 5978
		protected LensDistortion _lensDistortion;
	}
}
