using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200000D RID: 13
	public class MMInputExecution : MonoBehaviour
	{
		// Token: 0x0600002D RID: 45 RVA: 0x000029A5 File Offset: 0x00000BA5
		protected virtual void Update()
		{
			this.HandleInput();
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000029B0 File Offset: 0x00000BB0
		protected virtual void HandleInput()
		{
			if (this.Bindings == null)
			{
				return;
			}
			foreach (MMInputExecutionBinding mminputExecutionBinding in this.Bindings)
			{
				mminputExecutionBinding.ProcessInput();
			}
		}

		// Token: 0x04000028 RID: 40
		[Header("Bindings")]
		public List<MMInputExecutionBinding> Bindings;
	}
}
