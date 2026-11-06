using System;

namespace UnityEngine.EventSystems
{
	// Token: 0x0200004E RID: 78
	public abstract class AbstractEventData
	{
		// Token: 0x06000533 RID: 1331 RVA: 0x000180EB File Offset: 0x000162EB
		public virtual void Reset()
		{
			this.m_Used = false;
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x000180F4 File Offset: 0x000162F4
		public virtual void Use()
		{
			this.m_Used = true;
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000535 RID: 1333 RVA: 0x000180FD File Offset: 0x000162FD
		public virtual bool used
		{
			get
			{
				return this.m_Used;
			}
		}

		// Token: 0x040001AE RID: 430
		protected bool m_Used;
	}
}
