using System;
using UnityEngine;

namespace GPUInstancerPro
{
	// Token: 0x0200004A RID: 74
	public class GPUIObjectDestroyer : MonoBehaviour
	{
		// Token: 0x0600028E RID: 654 RVA: 0x0000E3F1 File Offset: 0x0000C5F1
		private void OnEnable()
		{
			this._enabledTime = Time.time;
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000E3FE File Offset: 0x0000C5FE
		private void Update()
		{
			if (Time.time - this._enabledTime > this.timeToDestroy)
			{
				Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x040001DF RID: 479
		public float timeToDestroy = 5f;

		// Token: 0x040001E0 RID: 480
		private float _enabledTime;
	}
}
