using System;
using Unity.Burst;
using Unity.Jobs;

namespace Unity.Collections
{
	// Token: 0x020000BE RID: 190
	[BurstCompile]
	internal struct NativeReferenceDisposeJob : IJob
	{
		// Token: 0x060008FB RID: 2299 RVA: 0x0001B034 File Offset: 0x00019234
		public void Execute()
		{
			this.Data.Dispose();
		}

		// Token: 0x040003EC RID: 1004
		internal NativeReferenceDispose Data;
	}
}
