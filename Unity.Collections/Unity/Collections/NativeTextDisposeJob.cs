using System;
using Unity.Burst;
using Unity.Jobs;

namespace Unity.Collections
{
	// Token: 0x020000D6 RID: 214
	[BurstCompile]
	internal struct NativeTextDisposeJob : IJob
	{
		// Token: 0x060009F1 RID: 2545 RVA: 0x0001D236 File Offset: 0x0001B436
		public void Execute()
		{
			this.Data.Dispose();
		}

		// Token: 0x04000415 RID: 1045
		public NativeTextDispose Data;
	}
}
