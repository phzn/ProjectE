using System;
using NGS.AdvancedCullingSystem.Static;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Tutorial
{
	// Token: 0x02000430 RID: 1072
	public class DisabledTargetGizmo : MonoBehaviour
	{
		// Token: 0x06001B12 RID: 6930 RVA: 0x000A8DCB File Offset: 0x000A6FCB
		private void Awake()
		{
			this._target = base.GetComponent<CullingTarget>();
			if (this._target == null)
			{
				base.enabled = false;
			}
		}

		// Token: 0x06001B13 RID: 6931 RVA: 0x000A8DF0 File Offset: 0x000A6FF0
		private void OnDrawGizmos()
		{
			if (this._target == null)
			{
				return;
			}
			if (this._target.enabled)
			{
				return;
			}
			Gizmos.color = Color.red;
			Gizmos.DrawWireCube(this._target.Bounds.center, this._target.Bounds.size);
		}

		// Token: 0x04001BEA RID: 7146
		private CullingTarget _target;
	}
}
