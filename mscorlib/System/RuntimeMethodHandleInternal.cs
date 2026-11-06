using System;

namespace System
{
	// Token: 0x02000220 RID: 544
	internal struct RuntimeMethodHandleInternal
	{
		// Token: 0x1700027F RID: 639
		// (get) Token: 0x0600186A RID: 6250 RVA: 0x0005D818 File Offset: 0x0005BA18
		internal static RuntimeMethodHandleInternal EmptyHandle
		{
			get
			{
				return default(RuntimeMethodHandleInternal);
			}
		}

		// Token: 0x0600186B RID: 6251 RVA: 0x0005D82E File Offset: 0x0005BA2E
		internal bool IsNullHandle()
		{
			return this.m_handle.IsNull();
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x0600186C RID: 6252 RVA: 0x0005D83B File Offset: 0x0005BA3B
		internal IntPtr Value
		{
			get
			{
				return this.m_handle;
			}
		}

		// Token: 0x0600186D RID: 6253 RVA: 0x0005D843 File Offset: 0x0005BA43
		internal RuntimeMethodHandleInternal(IntPtr value)
		{
			this.m_handle = value;
		}

		// Token: 0x040016B0 RID: 5808
		internal IntPtr m_handle;
	}
}
