using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace NGS.AdvancedCullingSystem.Dynamic
{
	// Token: 0x0200003A RID: 58
	public class DC_LODGroupShadowsTarget : DC_LODGroupTargetBase
	{
		// Token: 0x06000163 RID: 355 RVA: 0x00007569 File Offset: 0x00005769
		public DC_LODGroupShadowsTarget(LODGroup group, Renderer[] renderers, Bounds bounds)
			: base(group, renderers, bounds)
		{
			this._renderers = base.Renderers;
			this._shadowMode = this._renderers[0].shadowCastingMode;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00007594 File Offset: 0x00005794
		public override void MakeVisible()
		{
			for (int i = 0; i < this._renderers.Length; i++)
			{
				this._renderers[i].shadowCastingMode = this._shadowMode;
			}
		}

		// Token: 0x06000165 RID: 357 RVA: 0x000075C8 File Offset: 0x000057C8
		public override void MakeInvisible()
		{
			for (int i = 0; i < this._renderers.Length; i++)
			{
				this._renderers[i].shadowCastingMode = ShadowCastingMode.ShadowsOnly;
			}
		}

		// Token: 0x04000105 RID: 261
		private Renderer[] _renderers;

		// Token: 0x04000106 RID: 262
		private ShadowCastingMode _shadowMode;
	}
}
