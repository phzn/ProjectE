using System;
using UnityEngine;

namespace GPUInstancerPro.PrefabModule
{
	// Token: 0x02000018 RID: 24
	[DefaultExecutionOrder(200)]
	[RequireComponent(typeof(GPUIPrefab))]
	public class GPUIPrefabAutoAddRemove : MonoBehaviour
	{
		// Token: 0x0600009C RID: 156 RVA: 0x00005A20 File Offset: 0x00003C20
		private void Reset()
		{
			this.gpuiPrefab = base.GetComponent<GPUIPrefab>();
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00005A2E File Offset: 0x00003C2E
		protected void OnEnable()
		{
			if (this.gpuiPrefab == null)
			{
				this.gpuiPrefab = base.GetComponent<GPUIPrefab>();
			}
			GPUIPrefabManager.AddPrefabInstance(this.gpuiPrefab);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00005A55 File Offset: 0x00003C55
		protected void OnDisable()
		{
			if (this.gpuiPrefab != null)
			{
				this.gpuiPrefab.RemovePrefabInstance();
			}
		}

		// Token: 0x04000079 RID: 121
		[SerializeField]
		[HideInInspector]
		public GPUIPrefab gpuiPrefab;
	}
}
