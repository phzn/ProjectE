using System;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections
{
	// Token: 0x0200008D RID: 141
	[NativeContainer]
	[GenerateTestsForBurstCompatibility]
	internal struct NativeHashMapDispose
	{
		// Token: 0x06000729 RID: 1833 RVA: 0x00017628 File Offset: 0x00015828
		internal unsafe void Dispose()
		{
			HashMapHelper<int>* hashMapData = (HashMapHelper<int>*)this.m_HashMapData;
			HashMapHelper<int>.Free(hashMapData);
		}

		// Token: 0x040003B5 RID: 949
		[NativeDisableUnsafePtrRestriction]
		internal unsafe UnsafeHashMap<int, int>* m_HashMapData;

		// Token: 0x040003B6 RID: 950
		internal AllocatorManager.AllocatorHandle m_Allocator;
	}
}
