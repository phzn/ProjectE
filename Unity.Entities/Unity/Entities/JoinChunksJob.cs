using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Unity.Entities
{
	// Token: 0x020001D3 RID: 467
	[BurstCompile]
	[Obsolete("Remove with CreateArchetypeChunkArrayAsync. (RemovedAfter Entities 1.0)")]
	internal struct JoinChunksJob : IJobParallelFor
	{
		// Token: 0x06000B08 RID: 2824 RVA: 0x0003470C File Offset: 0x0003290C
		public void Execute(int index)
		{
			int num = this.DestinationOffsets[index];
			int num2 = this.DestinationOffsets[index + 1] - num;
			if (num2 != 0)
			{
				NativeArray<ArchetypeChunk>.Copy(this.SparseChunks, this.Offsets[index], this.JoinedChunks, num, num2);
			}
		}

		// Token: 0x040004FC RID: 1276
		[DeallocateOnJobCompletion]
		[ReadOnly]
		public NativeArray<int> DestinationOffsets;

		// Token: 0x040004FD RID: 1277
		[DeallocateOnJobCompletion]
		[ReadOnly]
		public NativeArray<ArchetypeChunk> SparseChunks;

		// Token: 0x040004FE RID: 1278
		[DeallocateOnJobCompletion]
		[ReadOnly]
		public NativeArray<int> Offsets;

		// Token: 0x040004FF RID: 1279
		[NativeDisableParallelForRestriction]
		public NativeArray<ArchetypeChunk> JoinedChunks;
	}
}
