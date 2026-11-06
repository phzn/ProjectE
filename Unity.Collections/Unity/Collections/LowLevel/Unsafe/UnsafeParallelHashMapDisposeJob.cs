using System;
using Unity.Burst;
using Unity.Jobs;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x0200012D RID: 301
	[BurstCompile]
	internal struct UnsafeParallelHashMapDisposeJob : IJob
	{
		// Token: 0x06000C9B RID: 3227 RVA: 0x00025D47 File Offset: 0x00023F47
		public void Execute()
		{
			UnsafeParallelHashMapData.DeallocateHashMap(this.Data, this.Allocator);
		}

		// Token: 0x04000507 RID: 1287
		[NativeDisableUnsafePtrRestriction]
		public unsafe UnsafeParallelHashMapData* Data;

		// Token: 0x04000508 RID: 1288
		public AllocatorManager.AllocatorHandle Allocator;
	}
}
