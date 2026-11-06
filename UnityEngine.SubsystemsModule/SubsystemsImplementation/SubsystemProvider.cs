using System;

namespace UnityEngine.SubsystemsImplementation
{
	// Token: 0x02000014 RID: 20
	public abstract class SubsystemProvider
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00002CB9 File Offset: 0x00000EB9
		public bool running
		{
			get
			{
				return this.m_Running;
			}
		}

		// Token: 0x04000014 RID: 20
		internal bool m_Running;
	}
}
