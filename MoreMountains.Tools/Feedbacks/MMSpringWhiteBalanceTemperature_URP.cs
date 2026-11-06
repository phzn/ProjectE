using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002D6 RID: 726
	[AddComponentMenu("More Mountains/Springs/MMSpringWhiteBalanceTemperature_URP")]
	public class MMSpringWhiteBalanceTemperature_URP : MMSpringFloatComponent<Volume>
	{
		// Token: 0x0600146C RID: 5228 RVA: 0x00057A5C File Offset: 0x00055C5C
		protected override void Initialization()
		{
			if (this.Target == null)
			{
				this.Target = base.gameObject.GetComponent<Volume>();
			}
			this.Target.profile.TryGet<WhiteBalance>(out this._whiteBalance);
			base.Initialization();
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x0600146D RID: 5229 RVA: 0x00057A9A File Offset: 0x00055C9A
		// (set) Token: 0x0600146E RID: 5230 RVA: 0x00057AAC File Offset: 0x00055CAC
		public override float TargetFloat
		{
			get
			{
				return this._whiteBalance.temperature.value;
			}
			set
			{
				this._whiteBalance.temperature.Override(value);
			}
		}

		// Token: 0x04001760 RID: 5984
		protected WhiteBalance _whiteBalance;
	}
}
