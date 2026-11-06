using System;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections
{
	// Token: 0x020000B9 RID: 185
	[NativeContainer]
	[GenerateTestsForBurstCompatibility]
	internal struct NativeQueueDispose
	{
		// Token: 0x060008E4 RID: 2276 RVA: 0x0001ADE5 File Offset: 0x00018FE5
		public void Dispose()
		{
			UnsafeQueue<int>.Free(this.m_QueueData);
		}

		// Token: 0x040003E5 RID: 997
		[NativeDisableUnsafePtrRestriction]
		public unsafe UnsafeQueue<int>* m_QueueData;
	}
}
