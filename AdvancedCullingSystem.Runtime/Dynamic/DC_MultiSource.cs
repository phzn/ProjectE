using System;
using System.Collections.Generic;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Dynamic
{
	// Token: 0x02000045 RID: 69
	public class DC_MultiSource : DC_Source
	{
		// Token: 0x06000199 RID: 409 RVA: 0x00007C72 File Offset: 0x00005E72
		private void Awake()
		{
			this._cullingTargets = new List<ICullingTarget>();
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00007C7F File Offset: 0x00005E7F
		public override void SetCullingTarget(ICullingTarget target)
		{
			if (this._cullingTargets == null)
			{
				this._cullingTargets = new List<ICullingTarget>();
			}
			this._cullingTargets.Add(target);
			if (this._visible)
			{
				target.MakeVisible();
				return;
			}
			target.MakeInvisible();
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00007CB5 File Offset: 0x00005EB5
		public override void RemoveCullingTarget(ICullingTarget target)
		{
			this._cullingTargets.Remove(target);
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00007CC4 File Offset: 0x00005EC4
		protected override void OnHitInternal()
		{
			if (this._visible)
			{
				return;
			}
			int i = 0;
			while (i < this._cullingTargets.Count)
			{
				try
				{
					this._cullingTargets[i].MakeVisible();
					i++;
				}
				catch (MissingReferenceException)
				{
					this.RemoveCullingTarget(this._cullingTargets[i]);
				}
			}
			this._visible = true;
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00007D30 File Offset: 0x00005F30
		protected override void OnTimeout()
		{
			int i = 0;
			while (i < this._cullingTargets.Count)
			{
				try
				{
					this._cullingTargets[i].MakeInvisible();
					i++;
				}
				catch (MissingReferenceException)
				{
					this.RemoveCullingTarget(this._cullingTargets[i]);
				}
			}
			this._visible = false;
		}

		// Token: 0x0400011B RID: 283
		private List<ICullingTarget> _cullingTargets;

		// Token: 0x0400011C RID: 284
		private bool _visible;
	}
}
