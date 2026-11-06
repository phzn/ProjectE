using System;
using System.Diagnostics;

namespace Unity.Entities.DebugProxies
{
	// Token: 0x020004C3 RID: 1219
	internal struct BlobPtrProxy<T> where T : struct
	{
		// Token: 0x06001D50 RID: 7504 RVA: 0x00066423 File Offset: 0x00064623
		public BlobPtrProxy(BlobPtrDebug<T> ptr)
		{
			this.m_Ptr = ptr;
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06001D51 RID: 7505 RVA: 0x0006642C File Offset: 0x0006462C
		public object Value
		{
			get
			{
				return this.m_Ptr.Value;
			}
		}

		// Token: 0x04000A89 RID: 2697
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private BlobPtrDebug<T> m_Ptr;
	}
}
