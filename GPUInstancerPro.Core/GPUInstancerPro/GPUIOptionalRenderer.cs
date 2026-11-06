using System;
using UnityEngine;

namespace GPUInstancerPro
{
	// Token: 0x0200005C RID: 92
	[DefaultExecutionOrder(-310)]
	[DisallowMultipleComponent]
	public class GPUIOptionalRenderer : MonoBehaviour
	{
		// Token: 0x0600035F RID: 863 RVA: 0x000127FE File Offset: 0x000109FE
		private void OnEnable()
		{
			GPUIPrefabBase gpuiprefabBase = this.prefabBase;
			if (gpuiprefabBase == null)
			{
				return;
			}
			gpuiprefabBase.SetOptionalRendererEnabled(this, true);
		}

		// Token: 0x06000360 RID: 864 RVA: 0x00012812 File Offset: 0x00010A12
		private void OnDisable()
		{
			GPUIPrefabBase gpuiprefabBase = this.prefabBase;
			if (gpuiprefabBase == null)
			{
				return;
			}
			gpuiprefabBase.SetOptionalRendererEnabled(this, false);
		}

		// Token: 0x04000271 RID: 625
		[SerializeField]
		[HideInInspector]
		public GPUIPrefabBase prefabBase;

		// Token: 0x04000272 RID: 626
		[SerializeField]
		[HideInInspector]
		public int optionalRendererNo;
	}
}
