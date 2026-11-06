using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000018 RID: 24
	public abstract class MMTriggerFilter : MonoBehaviour
	{
		// Token: 0x06000082 RID: 130 RVA: 0x000031AB File Offset: 0x000013AB
		protected virtual void OnValidate()
		{
			this.TriggerFilter &= TriggerAndCollisionMask.OnAnyTrigger;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000031BF File Offset: 0x000013BF
		protected virtual bool UseEvent(TriggerAndCollisionMask value)
		{
			return (this.TriggerFilter & value) > TriggerAndCollisionMask.IgnoreAll;
		}

		// Token: 0x06000084 RID: 132
		protected abstract void OnTriggerEnter2D_(Collider2D collider);

		// Token: 0x06000085 RID: 133 RVA: 0x000031CC File Offset: 0x000013CC
		private void OnTriggerEnter2D(Collider2D collider)
		{
			if (this.UseEvent(TriggerAndCollisionMask.OnTriggerEnter2D))
			{
				this.OnTriggerEnter2D_(collider);
			}
		}

		// Token: 0x06000086 RID: 134
		protected abstract void OnTriggerExit2D_(Collider2D collider);

		// Token: 0x06000087 RID: 135 RVA: 0x000031DF File Offset: 0x000013DF
		private void OnTriggerExit2D(Collider2D collider)
		{
			if (this.UseEvent(TriggerAndCollisionMask.OnTriggerExit2D))
			{
				this.OnTriggerExit2D_(collider);
			}
		}

		// Token: 0x06000088 RID: 136
		protected abstract void OnTriggerStay2D_(Collider2D collider);

		// Token: 0x06000089 RID: 137 RVA: 0x000031F5 File Offset: 0x000013F5
		private void OnTriggerStay2D(Collider2D collider)
		{
			if (this.UseEvent(TriggerAndCollisionMask.OnTriggerStay2D))
			{
				this.OnTriggerStay2D_(collider);
			}
		}

		// Token: 0x0600008A RID: 138
		protected abstract void OnTriggerEnter_(Collider collider);

		// Token: 0x0600008B RID: 139 RVA: 0x0000320B File Offset: 0x0000140B
		private void OnTriggerEnter(Collider collider)
		{
			if (this.UseEvent(TriggerAndCollisionMask.OnTriggerEnter))
			{
				this.OnTriggerEnter_(collider);
			}
		}

		// Token: 0x0600008C RID: 140
		protected abstract void OnTriggerExit_(Collider collider);

		// Token: 0x0600008D RID: 141 RVA: 0x0000321D File Offset: 0x0000141D
		private void OnTriggerExit(Collider collider)
		{
			if (this.UseEvent(TriggerAndCollisionMask.OnTriggerExit))
			{
				this.OnTriggerExit_(collider);
			}
		}

		// Token: 0x0600008E RID: 142
		protected abstract void OnTriggerStay_(Collider collider);

		// Token: 0x0600008F RID: 143 RVA: 0x0000322F File Offset: 0x0000142F
		private void OnTriggerStay(Collider collider)
		{
			if (this.UseEvent(TriggerAndCollisionMask.OnTriggerStay))
			{
				this.OnTriggerStay_(collider);
			}
		}

		// Token: 0x04000085 RID: 133
		public TriggerAndCollisionMask TriggerFilter = TriggerAndCollisionMask.All;
	}
}
