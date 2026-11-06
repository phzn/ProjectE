using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001CB RID: 459
	public class MMMiniPoolableObject : MonoBehaviour
	{
		// Token: 0x14000017 RID: 23
		// (add) Token: 0x06000B8A RID: 2954 RVA: 0x00032E8C File Offset: 0x0003108C
		// (remove) Token: 0x06000B8B RID: 2955 RVA: 0x00032EC4 File Offset: 0x000310C4
		public event MMMiniPoolableObject.Events OnSpawnComplete;

		// Token: 0x06000B8C RID: 2956 RVA: 0x00032EF9 File Offset: 0x000310F9
		public virtual void Destroy()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x00032F07 File Offset: 0x00031107
		protected virtual void OnEnable()
		{
			if (this.LifeTime > 0f)
			{
				base.Invoke("Destroy", this.LifeTime);
			}
		}

		// Token: 0x06000B8E RID: 2958 RVA: 0x00032F27 File Offset: 0x00031127
		protected virtual void OnDisable()
		{
			base.CancelInvoke();
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x00032F2F File Offset: 0x0003112F
		public virtual void TriggerOnSpawnComplete()
		{
			if (this.OnSpawnComplete != null)
			{
				this.OnSpawnComplete();
			}
		}

		// Token: 0x04000D30 RID: 3376
		public float LifeTime;

		// Token: 0x020004A4 RID: 1188
		// (Invoke) Token: 0x06001B6B RID: 7019
		public delegate void Events();
	}
}
