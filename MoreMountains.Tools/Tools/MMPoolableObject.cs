using System;
using UnityEngine;
using UnityEngine.Events;

namespace MoreMountains.Tools
{
	// Token: 0x02000110 RID: 272
	[AddComponentMenu("More Mountains/Tools/Object Pool/MMPoolableObject")]
	public class MMPoolableObject : MMObjectBounds
	{
		// Token: 0x14000012 RID: 18
		// (add) Token: 0x060005ED RID: 1517 RVA: 0x0001D3B4 File Offset: 0x0001B5B4
		// (remove) Token: 0x060005EE RID: 1518 RVA: 0x0001D3EC File Offset: 0x0001B5EC
		public event MMPoolableObject.Events OnSpawnComplete;

		// Token: 0x060005EF RID: 1519 RVA: 0x0001D421 File Offset: 0x0001B621
		public virtual void Destroy()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x0001D42F File Offset: 0x0001B62F
		protected virtual void Update()
		{
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x0001D434 File Offset: 0x0001B634
		protected virtual void OnEnable()
		{
			this.Size = this.GetBounds().extents * 2f;
			if (this.LifeTime > 0f)
			{
				base.Invoke("Destroy", this.LifeTime);
			}
			UnityEvent executeOnEnable = this.ExecuteOnEnable;
			if (executeOnEnable == null)
			{
				return;
			}
			executeOnEnable.Invoke();
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x0001D48D File Offset: 0x0001B68D
		protected virtual void OnDisable()
		{
			UnityEvent executeOnDisable = this.ExecuteOnDisable;
			if (executeOnDisable != null)
			{
				executeOnDisable.Invoke();
			}
			base.CancelInvoke();
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x0001D4A6 File Offset: 0x0001B6A6
		public virtual void TriggerOnSpawnComplete()
		{
			MMPoolableObject.Events onSpawnComplete = this.OnSpawnComplete;
			if (onSpawnComplete == null)
			{
				return;
			}
			onSpawnComplete();
		}

		// Token: 0x0400071A RID: 1818
		[Header("Events")]
		public UnityEvent ExecuteOnEnable;

		// Token: 0x0400071B RID: 1819
		public UnityEvent ExecuteOnDisable;

		// Token: 0x0400071D RID: 1821
		[Header("Poolable Object")]
		public float LifeTime;

		// Token: 0x02000433 RID: 1075
		// (Invoke) Token: 0x06001A26 RID: 6694
		public delegate void Events();
	}
}
