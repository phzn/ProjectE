using System;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections
{
	// Token: 0x020000C1 RID: 193
	[NativeContainer]
	[GenerateTestsForBurstCompatibility]
	internal struct NativeRingQueueDispose
	{
		// Token: 0x0600090B RID: 2315 RVA: 0x0001B1DE File Offset: 0x000193DE
		public void Dispose()
		{
			UnsafeRingQueue<int>.Free(this.m_QueueData);
		}

		// Token: 0x040003EF RID: 1007
		[NativeDisableUnsafePtrRestriction]
		public unsafe UnsafeRingQueue<int>* m_QueueData;
	}
}
