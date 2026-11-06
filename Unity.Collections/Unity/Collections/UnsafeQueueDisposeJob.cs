using System;
using Unity.Burst;
using Unity.Jobs;

namespace Unity.Collections
{
	// Token: 0x020000F1 RID: 241
	[BurstCompile]
	internal struct UnsafeQueueDisposeJob : IJob
	{
		// Token: 0x06000A73 RID: 2675 RVA: 0x0001F30E File Offset: 0x0001D50E
		public void Execute()
		{
			this.Data.Dispose();
		}

		// Token: 0x0400046F RID: 1135
		public UnsafeQueueDispose Data;
	}
}
