using System;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Dynamic
{
	// Token: 0x02000040 RID: 64
	public class DC_CullingTargetObserver : MonoBehaviour
	{
		// Token: 0x06000180 RID: 384 RVA: 0x0000777D File Offset: 0x0000597D
		public void Initialize(DC_Source source, ICullingTarget target)
		{
			this._source = source;
			this._target = target;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00007790 File Offset: 0x00005990
		private void OnDestroy()
		{
			if (!base.gameObject.scene.isLoaded)
			{
				return;
			}
			DC_Source source = this._source;
			if (source == null)
			{
				return;
			}
			source.RemoveCullingTarget(this._target);
		}

		// Token: 0x0400010F RID: 271
		private DC_Source _source;

		// Token: 0x04000110 RID: 272
		private ICullingTarget _target;
	}
}
