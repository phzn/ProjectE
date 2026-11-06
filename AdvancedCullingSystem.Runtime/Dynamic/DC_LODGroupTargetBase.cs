using System;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Dynamic
{
	// Token: 0x0200003C RID: 60
	public abstract class DC_LODGroupTargetBase : ICullingTarget
	{
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000169 RID: 361 RVA: 0x0000766E File Offset: 0x0000586E
		public GameObject GameObject
		{
			get
			{
				return this.Group.gameObject;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600016A RID: 362 RVA: 0x0000767B File Offset: 0x0000587B
		// (set) Token: 0x0600016B RID: 363 RVA: 0x00007683 File Offset: 0x00005883
		public Bounds Bounds { get; private set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600016C RID: 364 RVA: 0x0000768C File Offset: 0x0000588C
		// (set) Token: 0x0600016D RID: 365 RVA: 0x00007694 File Offset: 0x00005894
		private protected LODGroup Group { protected get; private set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600016E RID: 366 RVA: 0x0000769D File Offset: 0x0000589D
		// (set) Token: 0x0600016F RID: 367 RVA: 0x000076A5 File Offset: 0x000058A5
		private protected Renderer[] Renderers { protected get; private set; }

		// Token: 0x06000170 RID: 368 RVA: 0x000076AE File Offset: 0x000058AE
		public DC_LODGroupTargetBase(LODGroup group, Renderer[] renderers, Bounds bounds)
		{
			this.Group = group;
			this.Renderers = renderers;
			this.Bounds = bounds;
		}

		// Token: 0x06000171 RID: 369
		public abstract void MakeVisible();

		// Token: 0x06000172 RID: 370
		public abstract void MakeInvisible();
	}
}
