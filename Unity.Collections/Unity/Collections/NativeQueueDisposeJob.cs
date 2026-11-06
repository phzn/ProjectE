using System;
using Unity.Burst;
using Unity.Jobs;

namespace Unity.Collections
{
	// Token: 0x020000BA RID: 186
	[BurstCompile]
	internal struct NativeQueueDisposeJob : IJob
	{
		// Token: 0x060008E5 RID: 2277 RVA: 0x0001ADF2 File Offset: 0x00018FF2
		public void Execute()
		{
			this.Data.Dispose();
		}

		// Token: 0x040003E6 RID: 998
		public NativeQueueDispose Data;
	}
}
