using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace NGS.AdvancedCullingSystem.Dynamic
{
	// Token: 0x0200003D RID: 61
	public class DC_RendererShadowsTarget : DC_RendererTargetBase
	{
		// Token: 0x06000173 RID: 371 RVA: 0x000076CB File Offset: 0x000058CB
		public DC_RendererShadowsTarget(Renderer renderer)
			: base(renderer)
		{
			this._renderer = base.Renderer;
			this._shadowMode = this._renderer.shadowCastingMode;
		}

		// Token: 0x06000174 RID: 372 RVA: 0x000076F1 File Offset: 0x000058F1
		public override void MakeVisible()
		{
			this._renderer.shadowCastingMode = this._shadowMode;
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00007704 File Offset: 0x00005904
		public override void MakeInvisible()
		{
			this._renderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
		}

		// Token: 0x0400010B RID: 267
		private Renderer _renderer;

		// Token: 0x0400010C RID: 268
		private ShadowCastingMode _shadowMode;
	}
}
