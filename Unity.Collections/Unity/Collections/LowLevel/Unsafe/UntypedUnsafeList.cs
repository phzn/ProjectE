using System;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x02000114 RID: 276
	internal struct UntypedUnsafeList
	{
		// Token: 0x040004C8 RID: 1224
		[NativeDisableUnsafePtrRestriction]
		internal unsafe readonly void* Ptr;

		// Token: 0x040004C9 RID: 1225
		internal readonly int m_length;

		// Token: 0x040004CA RID: 1226
		internal readonly int m_capacity;

		// Token: 0x040004CB RID: 1227
		internal readonly AllocatorManager.AllocatorHandle Allocator;

		// Token: 0x040004CC RID: 1228
		internal readonly int padding;
	}
}
