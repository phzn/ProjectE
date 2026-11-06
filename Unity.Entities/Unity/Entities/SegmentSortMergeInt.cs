using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Unity.Entities
{
	// Token: 0x02000291 RID: 657
	[BurstCompile]
	internal struct SegmentSortMergeInt : IJob
	{
		// Token: 0x06001161 RID: 4449 RVA: 0x0004960C File Offset: 0x0004780C
		public unsafe void Execute()
		{
			int length = this.IndexedSourceBuffer.Length;
			if (length == 0)
			{
				return;
			}
			int num = (length + (this.SegmentWidth - 1)) / this.SegmentWidth;
			int* ptr;
			int num2;
			int num3;
			checked
			{
				ptr = stackalloc int[unchecked((UIntPtr)num) * 4];
				num2 = -1;
				num3 = 0;
			}
			for (int i = 0; i < length; i++)
			{
				int num4 = -1;
				IndexedInt indexedInt = default(IndexedInt);
				for (int j = 0; j < num; j++)
				{
					int num5 = j * this.SegmentWidth;
					int num6 = ptr[j];
					int num7 = ((length - num5 < this.SegmentWidth) ? (length - num5) : this.SegmentWidth);
					if (num6 != num7)
					{
						IndexedInt indexedInt2 = this.IndexedSourceBuffer[num5 + num6];
						if (num4 == -1 || indexedInt2.CompareTo(indexedInt) <= 0)
						{
							indexedInt = indexedInt2;
							num4 = j;
						}
					}
				}
				ptr[num4]++;
				this.SourceIndexBySortedSourceIndex[i] = indexedInt.Index;
				if (num2 != -1 && indexedInt.Value.CompareTo(num3) == 0)
				{
					int num8 = num2;
					int num9 = this.SharedIndexCountsBySharedIndex[num8];
					this.SharedIndexCountsBySharedIndex[num8] = num9 + 1;
				}
				else
				{
					num2++;
					num3 = indexedInt.Value;
					this.SortedSourceIndexBySharedIndex.Add(in i);
					int num9 = 1;
					this.SharedIndexCountsBySharedIndex.Add(in num9);
				}
				this.SharedIndicesBySourceIndex[indexedInt.Index] = num2;
			}
		}

		// Token: 0x040006A7 RID: 1703
		[ReadOnly]
		public NativeArray<IndexedInt> IndexedSourceBuffer;

		// Token: 0x040006A8 RID: 1704
		public NativeArray<int> SourceIndexBySortedSourceIndex;

		// Token: 0x040006A9 RID: 1705
		public NativeList<int> SortedSourceIndexBySharedIndex;

		// Token: 0x040006AA RID: 1706
		public NativeList<int> SharedIndexCountsBySharedIndex;

		// Token: 0x040006AB RID: 1707
		public NativeArray<int> SharedIndicesBySourceIndex;

		// Token: 0x040006AC RID: 1708
		public int SegmentWidth;
	}
}
