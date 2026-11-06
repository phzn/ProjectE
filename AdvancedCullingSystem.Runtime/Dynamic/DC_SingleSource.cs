using System;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Dynamic
{
	// Token: 0x02000046 RID: 70
	public class DC_SingleSource : DC_Source
	{
		// Token: 0x0600019F RID: 415 RVA: 0x00007D9C File Offset: 0x00005F9C
		public override void SetCullingTarget(ICullingTarget target)
		{
			this._cullingTarget = target;
			this._cullingTarget.MakeInvisible();
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00007DB0 File Offset: 0x00005FB0
		public override void RemoveCullingTarget(ICullingTarget target)
		{
			base.enabled = false;
			Object.Destroy(base.gameObject);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00007DC4 File Offset: 0x00005FC4
		protected override void OnHitInternal()
		{
			if (this._visible)
			{
				return;
			}
			this._cullingTarget.MakeVisible();
			this._visible = true;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00007DE1 File Offset: 0x00005FE1
		protected override void OnTimeout()
		{
			this._cullingTarget.MakeInvisible();
			this._visible = false;
		}

		// Token: 0x0400011D RID: 285
		private ICullingTarget _cullingTarget;

		// Token: 0x0400011E RID: 286
		private bool _visible;
	}
}
