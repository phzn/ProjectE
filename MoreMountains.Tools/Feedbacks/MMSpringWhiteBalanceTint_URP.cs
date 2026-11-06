using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002D7 RID: 727
	[AddComponentMenu("More Mountains/Springs/MMSpringWhiteBalanceTint_URP")]
	public class MMSpringWhiteBalanceTint_URP : MMSpringFloatComponent<Volume>
	{
		// Token: 0x06001470 RID: 5232 RVA: 0x00057AC7 File Offset: 0x00055CC7
		protected override void Initialization()
		{
			if (this.Target == null)
			{
				this.Target = base.gameObject.GetComponent<Volume>();
			}
			this.Target.profile.TryGet<WhiteBalance>(out this._whiteBalance);
			base.Initialization();
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06001471 RID: 5233 RVA: 0x00057B05 File Offset: 0x00055D05
		// (set) Token: 0x06001472 RID: 5234 RVA: 0x00057B17 File Offset: 0x00055D17
		public override float TargetFloat
		{
			get
			{
				return this._whiteBalance.tint.value;
			}
			set
			{
				this._whiteBalance.tint.Override(value);
			}
		}

		// Token: 0x04001761 RID: 5985
		protected WhiteBalance _whiteBalance;
	}
}
