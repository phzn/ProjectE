using System;
using Unity.Burst;
using Unity.Jobs;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x02000125 RID: 293
	[BurstCompile]
	internal struct UnsafeParallelHashMapDataDisposeJob : IJob
	{
		// Token: 0x06000C51 RID: 3153 RVA: 0x00024E30 File Offset: 0x00023030
		public void Execute()
		{
			this.Data.Dispose();
		}

		// Token: 0x040004F7 RID: 1271
		internal UnsafeParallelHashMapDataDispose Data;
	}
}
