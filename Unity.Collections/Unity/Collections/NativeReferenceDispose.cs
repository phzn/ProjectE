using System;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections
{
	// Token: 0x020000BD RID: 189
	[NativeContainer]
	internal struct NativeReferenceDispose
	{
		// Token: 0x060008FA RID: 2298 RVA: 0x0001B021 File Offset: 0x00019221
		public void Dispose()
		{
			Memory.Unmanaged.Free(this.m_Data, this.m_AllocatorLabel);
		}

		// Token: 0x040003EA RID: 1002
		[NativeDisableUnsafePtrRestriction]
		internal unsafe void* m_Data;

		// Token: 0x040003EB RID: 1003
		internal AllocatorManager.AllocatorHandle m_AllocatorLabel;
	}
}
