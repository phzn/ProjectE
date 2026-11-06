using System;
using UnityEngine;

namespace GPUInstancerPro
{
	// Token: 0x0200005E RID: 94
	[ExecuteInEditMode]
	[DefaultExecutionOrder(-1000)]
	public class GPUIRuntimeSettingsOverwrite : MonoBehaviour
	{
		// Token: 0x06000363 RID: 867 RVA: 0x00012826 File Offset: 0x00010A26
		private void OnEnable()
		{
			GPUIRuntimeSettings.OverwriteSettings(this.runtimeSettingsOverwrite);
		}

		// Token: 0x04000273 RID: 627
		public GPUIRuntimeSettings runtimeSettingsOverwrite;
	}
}
