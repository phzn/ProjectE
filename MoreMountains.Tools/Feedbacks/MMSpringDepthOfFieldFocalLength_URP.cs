using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002CF RID: 719
	[AddComponentMenu("More Mountains/Springs/MMSpringDepthOfFieldFocalLength_URP")]
	public class MMSpringDepthOfFieldFocalLength_URP : MMSpringFloatComponent<Volume>
	{
		// Token: 0x06001450 RID: 5200 RVA: 0x0005776F File Offset: 0x0005596F
		protected override void Initialization()
		{
			if (this.Target == null)
			{
				this.Target = base.gameObject.GetComponent<Volume>();
			}
			this.Target.profile.TryGet<DepthOfField>(out this._depthOfField);
			base.Initialization();
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06001451 RID: 5201 RVA: 0x000577AD File Offset: 0x000559AD
		// (set) Token: 0x06001452 RID: 5202 RVA: 0x000577BF File Offset: 0x000559BF
		public override float TargetFloat
		{
			get
			{
				return this._depthOfField.focalLength.value;
			}
			set
			{
				this._depthOfField.focalLength.Override(value);
			}
		}

		// Token: 0x04001759 RID: 5977
		protected DepthOfField _depthOfField;
	}
}
