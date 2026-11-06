using System;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Dynamic
{
	// Token: 0x0200003F RID: 63
	public abstract class DC_RendererTargetBase : ICullingTarget
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000179 RID: 377 RVA: 0x00007743 File Offset: 0x00005943
		public GameObject GameObject
		{
			get
			{
				return this.Renderer.gameObject;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600017A RID: 378 RVA: 0x00007750 File Offset: 0x00005950
		public Bounds Bounds
		{
			get
			{
				return this.Renderer.bounds;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600017B RID: 379 RVA: 0x0000775D File Offset: 0x0000595D
		// (set) Token: 0x0600017C RID: 380 RVA: 0x00007765 File Offset: 0x00005965
		private protected Renderer Renderer { protected get; private set; }

		// Token: 0x0600017D RID: 381 RVA: 0x0000776E File Offset: 0x0000596E
		public DC_RendererTargetBase(Renderer renderer)
		{
			this.Renderer = renderer;
		}

		// Token: 0x0600017E RID: 382
		public abstract void MakeInvisible();

		// Token: 0x0600017F RID: 383
		public abstract void MakeVisible();
	}
}
