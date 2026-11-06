using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Unity.Entities
{
	// Token: 0x02000290 RID: 656
	[BurstCompile]
	internal struct SegmentSortInt : IJobParallelFor
	{
		// Token: 0x06001160 RID: 4448 RVA: 0x000495B8 File Offset: 0x000477B8
		public void Execute(int index)
		{
			int num = index * this.SegmentWidth;
			int num2 = ((this.Data.Length - num < this.SegmentWidth) ? (this.Data.Length - num) : this.SegmentWidth);
			new NativeSlice<IndexedInt>(this.Data, num, num2).Sort<IndexedInt>();
		}

		// Token: 0x040006A5 RID: 1701
		[NativeDisableParallelForRestriction]
		public NativeArray<IndexedInt> Data;

		// Token: 0x040006A6 RID: 1702
		public int SegmentWidth;
	}
}
