using System;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Dynamic
{
	// Token: 0x02000038 RID: 56
	public class DC_CustomTarget : ICullingTarget
	{
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600015B RID: 347 RVA: 0x000074DF File Offset: 0x000056DF
		// (set) Token: 0x0600015C RID: 348 RVA: 0x000074E7 File Offset: 0x000056E7
		public GameObject GameObject { get; private set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600015D RID: 349 RVA: 0x000074F0 File Offset: 0x000056F0
		// (set) Token: 0x0600015E RID: 350 RVA: 0x000074F8 File Offset: 0x000056F8
		public Bounds Bounds { get; private set; }

		// Token: 0x0600015F RID: 351 RVA: 0x00007501 File Offset: 0x00005701
		public DC_CustomTarget(GameObject go, Bounds bounds, DC_CustomTargetEvent onVisible, DC_CustomTargetEvent onInvisible)
		{
			this.GameObject = go;
			this.Bounds = bounds;
			this._onVisible = ((onVisible != null) ? onVisible : new DC_CustomTargetEvent());
			this._onInvisible = ((onInvisible != null) ? onInvisible : new DC_CustomTargetEvent());
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0000753B File Offset: 0x0000573B
		public void MakeVisible()
		{
			DC_CustomTargetEvent onVisible = this._onVisible;
			if (onVisible == null)
			{
				return;
			}
			onVisible.Invoke(this);
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0000754E File Offset: 0x0000574E
		public void MakeInvisible()
		{
			DC_CustomTargetEvent onInvisible = this._onInvisible;
			if (onInvisible == null)
			{
				return;
			}
			onInvisible.Invoke(this);
		}

		// Token: 0x04000103 RID: 259
		private DC_CustomTargetEvent _onVisible;

		// Token: 0x04000104 RID: 260
		private DC_CustomTargetEvent _onInvisible;
	}
}
