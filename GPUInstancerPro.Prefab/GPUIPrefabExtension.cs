using System;
using UnityEngine;

namespace GPUInstancerPro.PrefabModule
{
	// Token: 0x02000005 RID: 5
	public abstract class GPUIPrefabExtension : MonoBehaviour
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000B RID: 11 RVA: 0x00002210 File Offset: 0x00000410
		public Transform CachedTransform
		{
			get
			{
				return this.gpuiPrefab.CachedTransform;
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000221D File Offset: 0x0000041D
		protected virtual void Start()
		{
			if (this.gpuiPrefab == null)
			{
				this.gpuiPrefab = base.GetComponent<GPUIPrefab>();
			}
		}

		// Token: 0x0400000A RID: 10
		[SerializeField]
		[HideInInspector]
		public GPUIPrefab gpuiPrefab;
	}
}
