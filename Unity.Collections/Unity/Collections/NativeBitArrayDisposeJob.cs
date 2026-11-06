using System;
using Unity.Burst;
using Unity.Jobs;

namespace Unity.Collections
{
	// Token: 0x0200008C RID: 140
	[BurstCompile]
	internal struct NativeBitArrayDisposeJob : IJob
	{
		// Token: 0x06000728 RID: 1832 RVA: 0x0001761A File Offset: 0x0001581A
		public void Execute()
		{
			this.Data.Dispose();
		}

		// Token: 0x040003B4 RID: 948
		public NativeBitArrayDispose Data;
	}
}
