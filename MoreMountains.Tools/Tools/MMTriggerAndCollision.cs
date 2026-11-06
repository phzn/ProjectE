using System;
using UnityEngine;
using UnityEngine.Events;

namespace MoreMountains.Tools
{
	// Token: 0x02000015 RID: 21
	[AddComponentMenu("More Mountains/Tools/Activation/MMTriggerAndCollision")]
	public class MMTriggerAndCollision : MonoBehaviour
	{
		// Token: 0x0600005A RID: 90 RVA: 0x00002EB8 File Offset: 0x000010B8
		protected virtual void OnCollisionEnter2D(Collision2D collision)
		{
			if (this.Collision2DLayerMask.MMContains(collision.gameObject))
			{
				this.OnCollision2DEnterEvent.Invoke();
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002ED8 File Offset: 0x000010D8
		protected virtual void OnCollisionExit2D(Collision2D collision)
		{
			if (this.Collision2DLayerMask.MMContains(collision.gameObject))
			{
				this.OnCollision2DExitEvent.Invoke();
			}
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00002EF8 File Offset: 0x000010F8
		protected virtual void OnCollisionStay2D(Collision2D collision)
		{
			if (this.Collision2DLayerMask.MMContains(collision.gameObject))
			{
				this.OnCollision2DStayEvent.Invoke();
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00002F18 File Offset: 0x00001118
		protected virtual void OnTriggerEnter2D(Collider2D collider)
		{
			if (this.Trigger2DLayerMask.MMContains(collider.gameObject))
			{
				this.OnTrigger2DEnterEvent.Invoke();
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00002F38 File Offset: 0x00001138
		protected virtual void OnTriggerExit2D(Collider2D collider)
		{
			if (this.Trigger2DLayerMask.MMContains(collider.gameObject))
			{
				this.OnTrigger2DExitEvent.Invoke();
			}
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00002F58 File Offset: 0x00001158
		protected virtual void OnTriggerStay2D(Collider2D collider)
		{
			if (this.Trigger2DLayerMask.MMContains(collider.gameObject))
			{
				this.OnTrigger2DStayEvent.Invoke();
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00002F78 File Offset: 0x00001178
		protected virtual void OnCollisionEnter(Collision c)
		{
			if ((this.CollisionLayerMask.value & (1 << c.transform.gameObject.layer)) != 0)
			{
				this.OnCollisionEnterEvent.Invoke();
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00002FA8 File Offset: 0x000011A8
		protected virtual void OnCollisionExit(Collision c)
		{
			if ((this.CollisionLayerMask.value & (1 << c.transform.gameObject.layer)) != 0)
			{
				this.OnCollisionExitEvent.Invoke();
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00002FD8 File Offset: 0x000011D8
		protected virtual void OnCollisionStay(Collision c)
		{
			if ((this.CollisionLayerMask.value & (1 << c.transform.gameObject.layer)) != 0)
			{
				this.OnCollisionStayEvent.Invoke();
			}
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003008 File Offset: 0x00001208
		protected virtual void OnTriggerEnter(Collider collider)
		{
			if (this.TriggerLayerMask.MMContains(collider.gameObject))
			{
				this.OnTriggerEnterEvent.Invoke();
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00003028 File Offset: 0x00001228
		protected virtual void OnTriggerExit(Collider collider)
		{
			if (this.TriggerLayerMask.MMContains(collider.gameObject))
			{
				this.OnTriggerExitEvent.Invoke();
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00003048 File Offset: 0x00001248
		protected virtual void OnTriggerStay(Collider collider)
		{
			if (this.TriggerLayerMask.MMContains(collider.gameObject))
			{
				this.OnTriggerStayEvent.Invoke();
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003068 File Offset: 0x00001268
		protected virtual void Reset()
		{
			this.Collision2DLayerMask = LayerMask.NameToLayer("Everything");
			this.CollisionLayerMask = LayerMask.NameToLayer("Everything");
		}

		// Token: 0x0400005D RID: 93
		public LayerMask CollisionLayerMask;

		// Token: 0x0400005E RID: 94
		public UnityEvent OnCollisionEnterEvent;

		// Token: 0x0400005F RID: 95
		public UnityEvent OnCollisionExitEvent;

		// Token: 0x04000060 RID: 96
		public UnityEvent OnCollisionStayEvent;

		// Token: 0x04000061 RID: 97
		public LayerMask TriggerLayerMask;

		// Token: 0x04000062 RID: 98
		public UnityEvent OnTriggerEnterEvent;

		// Token: 0x04000063 RID: 99
		public UnityEvent OnTriggerExitEvent;

		// Token: 0x04000064 RID: 100
		public UnityEvent OnTriggerStayEvent;

		// Token: 0x04000065 RID: 101
		public LayerMask Collision2DLayerMask;

		// Token: 0x04000066 RID: 102
		public UnityEvent OnCollision2DEnterEvent;

		// Token: 0x04000067 RID: 103
		public UnityEvent OnCollision2DExitEvent;

		// Token: 0x04000068 RID: 104
		public UnityEvent OnCollision2DStayEvent;

		// Token: 0x04000069 RID: 105
		public LayerMask Trigger2DLayerMask;

		// Token: 0x0400006A RID: 106
		public UnityEvent OnTrigger2DEnterEvent;

		// Token: 0x0400006B RID: 107
		public UnityEvent OnTrigger2DExitEvent;

		// Token: 0x0400006C RID: 108
		public UnityEvent OnTrigger2DStayEvent;
	}
}
