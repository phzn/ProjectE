using System;
using Unity.Burst;
using Unity.Jobs;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x02000113 RID: 275
	[BurstCompile]
	internal struct UnsafeDisposeJob : IJob
	{
		// Token: 0x06000BB9 RID: 3001 RVA: 0x0002362C File Offset: 0x0002182C
		public void Execute()
		{
			AllocatorManager.Free(this.Allocator, this.Ptr);
		}

		// Token: 0x040004C6 RID: 1222
		[NativeDisableUnsafePtrRestriction]
		public unsafe void* Ptr;

		// Token: 0x040004C7 RID: 1223
		public AllocatorManager.AllocatorHandle Allocator;
	}
}
