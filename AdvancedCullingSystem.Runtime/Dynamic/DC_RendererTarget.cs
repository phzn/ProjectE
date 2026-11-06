using System;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Dynamic
{
	// Token: 0x0200003E RID: 62
	public class DC_RendererTarget : DC_RendererTargetBase
	{
		// Token: 0x06000176 RID: 374 RVA: 0x00007712 File Offset: 0x00005912
		public DC_RendererTarget(Renderer renderer)
			: base(renderer)
		{
			this._renderer = base.Renderer;
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00007727 File Offset: 0x00005927
		public override void MakeVisible()
		{
			this._renderer.enabled = true;
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00007735 File Offset: 0x00005935
		public override void MakeInvisible()
		{
			this._renderer.enabled = false;
		}

		// Token: 0x0400010D RID: 269
		private Renderer _renderer;
	}
}
