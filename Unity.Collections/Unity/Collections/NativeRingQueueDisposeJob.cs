using System;
using Unity.Burst;
using Unity.Jobs;

namespace Unity.Collections
{
	// Token: 0x020000C2 RID: 194
	[BurstCompile]
	internal struct NativeRingQueueDisposeJob : IJob
	{
		// Token: 0x0600090C RID: 2316 RVA: 0x0001B1EB File Offset: 0x000193EB
		public void Execute()
		{
			this.Data.Dispose();
		}

		// Token: 0x040003F0 RID: 1008
		public NativeRingQueueDispose Data;
	}
}
