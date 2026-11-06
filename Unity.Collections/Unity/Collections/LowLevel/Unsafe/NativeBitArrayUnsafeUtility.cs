using System;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x020000FF RID: 255
	[GenerateTestsForBurstCompatibility]
	public static class NativeBitArrayUnsafeUtility
	{
		// Token: 0x06000AE9 RID: 2793 RVA: 0x00021374 File Offset: 0x0001F574
		public unsafe static NativeBitArray ConvertExistingDataToNativeBitArray(void* ptr, int sizeInBytes, AllocatorManager.AllocatorHandle allocator)
		{
			UnsafeBitArray* ptr2 = UnsafeBitArray.Alloc(Allocator.Persistent);
			*ptr2 = new UnsafeBitArray(ptr, sizeInBytes, allocator);
			return new NativeBitArray
			{
				m_BitArray = ptr2,
				m_Allocator = Allocator.Persistent
			};
		}
	}
}
