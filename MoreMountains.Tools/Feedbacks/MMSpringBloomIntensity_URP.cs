using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002CA RID: 714
	[AddComponentMenu("More Mountains/Springs/MMSpringBloomIntensity_URP")]
	public class MMSpringBloomIntensity_URP : MMSpringFloatComponent<Volume>
	{
		// Token: 0x0600143C RID: 5180 RVA: 0x00057558 File Offset: 0x00055758
		protected override void Initialization()
		{
			if (this.Target == null)
			{
				this.Target = base.gameObject.GetComponent<Volume>();
			}
			this.Target.profile.TryGet<Bloom>(out this._bloom);
			base.Initialization();
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x0600143D RID: 5181 RVA: 0x00057596 File Offset: 0x00055796
		// (set) Token: 0x0600143E RID: 5182 RVA: 0x000575A8 File Offset: 0x000557A8
		public override float TargetFloat
		{
			get
			{
				return this._bloom.intensity.value;
			}
			set
			{
				this._bloom.intensity.Override(value);
			}
		}

		// Token: 0x04001754 RID: 5972
		protected Bloom _bloom;
	}
}
