using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002CB RID: 715
	[AddComponentMenu("More Mountains/Springs/MMSpringChromaticAberrationIntensity_URP")]
	public class MMSpringChromaticAberrationIntensity_URP : MMSpringFloatComponent<Volume>
	{
		// Token: 0x06001440 RID: 5184 RVA: 0x000575C3 File Offset: 0x000557C3
		protected override void Initialization()
		{
			if (this.Target == null)
			{
				this.Target = base.gameObject.GetComponent<Volume>();
			}
			this.Target.profile.TryGet<ChromaticAberration>(out this._chromaticAberration);
			base.Initialization();
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06001441 RID: 5185 RVA: 0x00057601 File Offset: 0x00055801
		// (set) Token: 0x06001442 RID: 5186 RVA: 0x00057613 File Offset: 0x00055813
		public override float TargetFloat
		{
			get
			{
				return this._chromaticAberration.intensity.value;
			}
			set
			{
				this._chromaticAberration.intensity.Override(value);
			}
		}

		// Token: 0x04001755 RID: 5973
		protected ChromaticAberration _chromaticAberration;
	}
}
