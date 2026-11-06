using System;
using Unity.Burst;
using Unity.Jobs;

namespace Unity.Collections
{
	// Token: 0x0200008E RID: 142
	[BurstCompile]
	internal struct NativeHashMapDisposeJob : IJob
	{
		// Token: 0x0600072A RID: 1834 RVA: 0x00017642 File Offset: 0x00015842
		public void Execute()
		{
			this.Data.Dispose();
		}

		// Token: 0x040003B7 RID: 951
		internal NativeHashMapDispose Data;
	}
}
