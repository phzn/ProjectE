using System;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections
{
	// Token: 0x020000F0 RID: 240
	[GenerateTestsForBurstCompatibility]
	internal struct UnsafeQueueDispose
	{
		// Token: 0x06000A72 RID: 2674 RVA: 0x0001F2FB File Offset: 0x0001D4FB
		public void Dispose()
		{
			UnsafeQueueData.DeallocateQueue(this.m_Buffer, this.m_AllocatorLabel);
		}

		// Token: 0x0400046D RID: 1133
		[NativeDisableUnsafePtrRestriction]
		internal unsafe UnsafeQueueData* m_Buffer;

		// Token: 0x0400046E RID: 1134
		internal AllocatorManager.AllocatorHandle m_AllocatorLabel;
	}
}
