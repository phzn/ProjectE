using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002D1 RID: 721
	[AddComponentMenu("More Mountains/Springs/MMSpringMotionBlurIntensity_URP")]
	public class MMSpringMotionBlurIntensity_URP : MMSpringFloatComponent<Volume>
	{
		// Token: 0x06001458 RID: 5208 RVA: 0x00057845 File Offset: 0x00055A45
		protected override void Initialization()
		{
			if (this.Target == null)
			{
				this.Target = base.gameObject.GetComponent<Volume>();
			}
			this.Target.profile.TryGet<MotionBlur>(out this._motionBlur);
			base.Initialization();
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06001459 RID: 5209 RVA: 0x00057883 File Offset: 0x00055A83
		// (set) Token: 0x0600145A RID: 5210 RVA: 0x00057895 File Offset: 0x00055A95
		public override float TargetFloat
		{
			get
			{
				return this._motionBlur.intensity.value;
			}
			set
			{
				this._motionBlur.intensity.Override(value);
			}
		}

		// Token: 0x0400175B RID: 5979
		protected MotionBlur _motionBlur;
	}
}
