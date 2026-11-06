using System;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Tutorial
{
	// Token: 0x0200042F RID: 1071
	public class DisabledRendererGizmo : MonoBehaviour
	{
		// Token: 0x06001B0F RID: 6927 RVA: 0x000A8D47 File Offset: 0x000A6F47
		private void Awake()
		{
			this._target = base.GetComponent<Renderer>();
			if (this._target == null)
			{
				base.enabled = false;
			}
		}

		// Token: 0x06001B10 RID: 6928 RVA: 0x000A8D6C File Offset: 0x000A6F6C
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
			Gizmos.DrawWireCube(this._target.bounds.center, this._target.bounds.size);
		}

		// Token: 0x04001BE9 RID: 7145
		private Renderer _target;
	}
}
