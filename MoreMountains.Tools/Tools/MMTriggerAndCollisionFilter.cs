using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000017 RID: 23
	public abstract class MMTriggerAndCollisionFilter : MonoBehaviour
	{
		// Token: 0x06000068 RID: 104 RVA: 0x0000309C File Offset: 0x0000129C
		protected virtual bool UseEvent(TriggerAndCollisionMask value)
		{
			return (this.TriggerAndCollisionFilter & value) > TriggerAndCollisionMask.IgnoreAll;
		}

		// Token: 0x06000069 RID: 105
		protected abstract void OnCollisionEnter2D_(Collision2D collision);

		// Token: 0x0600006A RID: 106 RVA: 0x000030A9 File Offset: 0x000012A9
		private void OnCollisionEnter2D(Collision2D collision)
		{
			if (this.UseEvent(TriggerAndCollisionMask.OnCollisionEnter2D))
			{
				this.OnCollisionEnter2D_(collision);
			}
		}

		// Token: 0x0600006B RID: 107
		protected abstract void OnCollisionExit2D_(Collision2D collision);

		// Token: 0x0600006C RID: 108 RVA: 0x000030BF File Offset: 0x000012BF
		private void OnCollisionExit2D(Collision2D collision)
		{
			if (this.UseEvent(TriggerAndCollisionMask.OnCollisionExit2D))
			{
				this.OnCollisionExit2D_(collision);
			}
		}

		// Token: 0x0600006D RID: 109
		protected abstract void OnCollisionStay2D_(Collision2D collision);

		// Token: 0x0600006E RID: 110 RVA: 0x000030D5 File Offset: 0x000012D5
		private void OnCollisionStay2D(Collision2D collision)
		{
			if (this.UseEvent(TriggerAndCollisionMask.OnCollisionStay2D))
			{
				this.OnCollisionStay2D_(collision);
			}
		}

		// Token: 0x0600006F RID: 111
		protected abstract void OnTriggerEnter2D_(Collider2D collider);

		// Token: 0x06000070 RID: 112 RVA: 0x000030EB File Offset: 0x000012EB
		private void OnTriggerEnter2D(Collider2D collider)
		{
			if (this.UseEvent(TriggerAndCollisionMask.OnTriggerEnter2D))
			{
				this.OnTriggerEnter2D_(collider);
			}
		}

		// Token: 0x06000071 RID: 113
		protected abstract void OnTriggerExit2D_(Collider2D collider);

		// Token: 0x06000072 RID: 114 RVA: 0x000030FE File Offset: 0x000012FE
		private void OnTriggerExit2D(Collider2D collider)
		{
			if (this.UseEvent(TriggerAndCollisionMask.OnTriggerExit2D))
			{
				this.OnTriggerExit2D_(collider);
			}
		}

		// Token: 0x06000073 RID: 115
		protected abstract void OnTriggerStay2D_(Collider2D collider);

		// Token: 0x06000074 RID: 116 RVA: 0x00003114 File Offset: 0x00001314
		private void OnTriggerStay2D(Collider2D collider)
		{
			if (this.UseEvent(TriggerAndCollisionMask.OnTriggerStay2D))
			{
				this.OnTriggerStay2D_(collider);
			}
		}

		// Token: 0x06000075 RID: 117
		protected abstract void OnCollisionEnter_(Collision c);

		// Token: 0x06000076 RID: 118 RVA: 0x0000312A File Offset: 0x0000132A
		private void OnCollisionEnter(Collision c)
		{
			if (this.UseEvent(TriggerAndCollisionMask.OnCollisionEnter))
			{
				this.OnCollisionEnter_(c);
			}
		}

		// Token: 0x06000077 RID: 119
		protected abstract void OnCollisionExit_(Collision c);

		// Token: 0x06000078 RID: 120 RVA: 0x0000313C File Offset: 0x0000133C
		private void OnCollisionExit(Collision c)
		{
			if (this.UseEvent(TriggerAndCollisionMask.OnCollisionExit))
			{
				this.OnCollisionExit_(c);
			}
		}

		// Token: 0x06000079 RID: 121
		protected abstract void OnCollisionStay_(Collision c);

		// Token: 0x0600007A RID: 122 RVA: 0x0000314F File Offset: 0x0000134F
		private void OnCollisionStay(Collision c)
		{
			if (this.UseEvent(TriggerAndCollisionMask.OnCollisionStay))
			{
				this.OnCollisionStay_(c);
			}
		}

		// Token: 0x0600007B RID: 123
		protected abstract void OnTriggerEnter_(Collider collider);

		// Token: 0x0600007C RID: 124 RVA: 0x00003162 File Offset: 0x00001362
		private void OnTriggerEnter(Collider collider)
		{
			if (this.UseEvent(TriggerAndCollisionMask.OnTriggerEnter))
			{
				this.OnTriggerEnter_(collider);
			}
		}

		// Token: 0x0600007D RID: 125
		protected abstract void OnTriggerExit_(Collider collider);

		// Token: 0x0600007E RID: 126 RVA: 0x00003174 File Offset: 0x00001374
		private void OnTriggerExit(Collider collider)
		{
			if (this.UseEvent(TriggerAndCollisionMask.OnTriggerExit))
			{
				this.OnTriggerExit_(collider);
			}
		}

		// Token: 0x0600007F RID: 127
		protected abstract void OnTriggerStay_(Collider collider);

		// Token: 0x06000080 RID: 128 RVA: 0x00003186 File Offset: 0x00001386
		private void OnTriggerStay(Collider collider)
		{
			if (this.UseEvent(TriggerAndCollisionMask.OnTriggerStay))
			{
				this.OnTriggerStay_(collider);
			}
		}

		// Token: 0x04000084 RID: 132
		public TriggerAndCollisionMask TriggerAndCollisionFilter = TriggerAndCollisionMask.All;
	}
}
