using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Unity.Entities.DebugProxies
{
	// Token: 0x020004C0 RID: 1216
	internal readonly struct BlobAssetReferenceProxy<[global::System.Runtime.CompilerServices.IsUnmanaged] T> where T : struct, ValueType
	{
		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06001D4B RID: 7499 RVA: 0x0006638E File Offset: 0x0006458E
		public object Value
		{
			get
			{
				return this.m_Root;
			}
		}

		// Token: 0x06001D4C RID: 7500 RVA: 0x00066398 File Offset: 0x00064598
		public unsafe BlobAssetReferenceProxy(BlobAssetReference<T> bar)
		{
			if (!bar.IsCreated)
			{
				this.m_Root = null;
				return;
			}
			if (typeof(T).IsPrimitive)
			{
				this.m_Root = *bar.Value;
				return;
			}
			this.m_Root = new BlobStruct<T>(new IntPtr(bar.GetUnsafePtr()));
		}

		// Token: 0x04000A85 RID: 2693
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly object m_Root;
	}
}
