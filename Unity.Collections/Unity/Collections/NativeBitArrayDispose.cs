using System;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections
{
	// Token: 0x0200008B RID: 139
	[NativeContainer]
	[GenerateTestsForBurstCompatibility]
	internal struct NativeBitArrayDispose
	{
		// Token: 0x06000727 RID: 1831 RVA: 0x00017607 File Offset: 0x00015807
		public void Dispose()
		{
			UnsafeBitArray.Free(this.m_BitArrayData, this.m_Allocator);
		}

		// Token: 0x040003B2 RID: 946
		[NativeDisableUnsafePtrRestriction]
		public unsafe UnsafeBitArray* m_BitArrayData;

		// Token: 0x040003B3 RID: 947
		public AllocatorManager.AllocatorHandle m_Allocator;
	}
}
