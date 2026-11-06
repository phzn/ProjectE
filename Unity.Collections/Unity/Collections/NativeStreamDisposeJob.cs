using System;
using Unity.Burst;
using Unity.Jobs;

namespace Unity.Collections
{
	// Token: 0x020000D1 RID: 209
	[BurstCompile]
	internal struct NativeStreamDisposeJob : IJob
	{
		// Token: 0x06000974 RID: 2420 RVA: 0x0001C5B5 File Offset: 0x0001A7B5
		public void Execute()
		{
			this.Data.Dispose();
		}

		// Token: 0x0400040E RID: 1038
		public NativeStreamDispose Data;
	}
}
