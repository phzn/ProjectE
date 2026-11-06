using System;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Dynamic
{
	// Token: 0x0200003B RID: 59
	public class DC_LODGroupTarget : DC_LODGroupTargetBase
	{
		// Token: 0x06000166 RID: 358 RVA: 0x000075F6 File Offset: 0x000057F6
		public DC_LODGroupTarget(LODGroup group, Renderer[] renderers, Bounds bounds)
			: base(group, renderers, bounds)
		{
			this._renderers = base.Renderers;
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00007610 File Offset: 0x00005810
		public override void MakeVisible()
		{
			for (int i = 0; i < this._renderers.Length; i++)
			{
				this._renderers[i].enabled = true;
			}
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00007640 File Offset: 0x00005840
		public override void MakeInvisible()
		{
			for (int i = 0; i < this._renderers.Length; i++)
			{
				this._renderers[i].enabled = false;
			}
		}

		// Token: 0x04000107 RID: 263
		private Renderer[] _renderers;
	}
}
