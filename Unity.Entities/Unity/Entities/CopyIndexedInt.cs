using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Unity.Entities
{
	// Token: 0x0200028F RID: 655
	[BurstCompile]
	internal struct CopyIndexedInt : IJobParallelFor
	{
		// Token: 0x0600115F RID: 4447 RVA: 0x00049578 File Offset: 0x00047778
		public void Execute(int index)
		{
			this.Dst[index] = new IndexedInt
			{
				Value = this.Src[index],
				Index = index
			};
		}

		// Token: 0x040006A3 RID: 1699
		[ReadOnly]
		public NativeArray<int> Src;

		// Token: 0x040006A4 RID: 1700
		public NativeArray<IndexedInt> Dst;
	}
}
