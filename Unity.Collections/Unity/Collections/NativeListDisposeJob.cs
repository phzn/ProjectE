using System;
using Unity.Burst;
using Unity.Jobs;

namespace Unity.Collections
{
	// Token: 0x0200009D RID: 157
	[BurstCompile]
	[GenerateTestsForBurstCompatibility]
	internal struct NativeListDisposeJob : IJob
	{
		// Token: 0x060007CF RID: 1999 RVA: 0x000183BE File Offset: 0x000165BE
		public void Execute()
		{
			this.Data.Dispose();
		}

		// Token: 0x040003C6 RID: 966
		internal NativeListDispose Data;
	}
}
