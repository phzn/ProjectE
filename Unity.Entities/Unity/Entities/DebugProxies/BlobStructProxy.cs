using System;
using System.Diagnostics;

namespace Unity.Entities.DebugProxies
{
	// Token: 0x020004C2 RID: 1218
	internal struct BlobStructProxy<T> where T : struct
	{
		// Token: 0x06001D4E RID: 7502 RVA: 0x0006640D File Offset: 0x0006460D
		public BlobStructProxy(BlobStruct<T> struc)
		{
			this.m_Struct = struc;
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06001D4F RID: 7503 RVA: 0x00066416 File Offset: 0x00064616
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public BlobMember[] Entries
		{
			get
			{
				return this.m_Struct.Members;
			}
		}

		// Token: 0x04000A88 RID: 2696
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private BlobStruct<T> m_Struct;
	}
}
