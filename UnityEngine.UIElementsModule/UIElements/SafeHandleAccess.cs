using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020002A1 RID: 673
	internal struct SafeHandleAccess
	{
		// Token: 0x06001348 RID: 4936 RVA: 0x00043666 File Offset: 0x00041866
		public SafeHandleAccess(IntPtr ptr)
		{
			this.m_Handle = ptr;
		}

		// Token: 0x06001349 RID: 4937 RVA: 0x00043670 File Offset: 0x00041870
		public bool IsNull()
		{
			return this.m_Handle == IntPtr.Zero;
		}

		// Token: 0x0600134A RID: 4938 RVA: 0x00043694 File Offset: 0x00041894
		public static implicit operator IntPtr(SafeHandleAccess a)
		{
			bool flag = a.m_Handle == IntPtr.Zero;
			if (flag)
			{
				throw new ArgumentNullException();
			}
			return a.m_Handle;
		}

		// Token: 0x040008BD RID: 2237
		private IntPtr m_Handle;
	}
}
