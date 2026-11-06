using System;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x02000124 RID: 292
	[NativeContainer]
	[GenerateTestsForBurstCompatibility]
	internal struct UnsafeParallelHashMapDataDispose
	{
		// Token: 0x06000C50 RID: 3152 RVA: 0x00024E1D File Offset: 0x0002301D
		public void Dispose()
		{
			UnsafeParallelHashMapData.DeallocateHashMap(this.m_Buffer, this.m_AllocatorLabel);
		}

		// Token: 0x040004F5 RID: 1269
		[NativeDisableUnsafePtrRestriction]
		internal unsafe UnsafeParallelHashMapData* m_Buffer;

		// Token: 0x040004F6 RID: 1270
		internal AllocatorManager.AllocatorHandle m_AllocatorLabel;
	}
}
