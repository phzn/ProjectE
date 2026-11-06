using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002D2 RID: 722
	[AddComponentMenu("More Mountains/Springs/MMSpringPaniniProjectionDistance_URP")]
	public class MMSpringPaniniProjectionDistance_URP : MMSpringFloatComponent<Volume>
	{
		// Token: 0x0600145C RID: 5212 RVA: 0x000578B0 File Offset: 0x00055AB0
		protected override void Initialization()
		{
			if (this.Target == null)
			{
				this.Target = base.gameObject.GetComponent<Volume>();
			}
			this.Target.profile.TryGet<PaniniProjection>(out this._paniniProjection);
			base.Initialization();
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x0600145D RID: 5213 RVA: 0x000578EE File Offset: 0x00055AEE
		// (set) Token: 0x0600145E RID: 5214 RVA: 0x00057900 File Offset: 0x00055B00
		public override float TargetFloat
		{
			get
			{
				return this._paniniProjection.distance.value;
			}
			set
			{
				this._paniniProjection.distance.Override(value);
			}
		}

		// Token: 0x0400175C RID: 5980
		protected PaniniProjection _paniniProjection;
	}
}
