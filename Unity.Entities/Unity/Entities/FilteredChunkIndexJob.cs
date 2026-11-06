using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Unity.Entities
{
	// Token: 0x020001D4 RID: 468
	[BurstCompile]
	internal struct FilteredChunkIndexJob : IJob
	{
		// Token: 0x06000B09 RID: 2825 RVA: 0x00034759 File Offset: 0x00032959
		public void Execute()
		{
			ChunkIterationUtility.CalculateFilteredChunkIndexArray(in this.CachedChunkList, in this.MatchingArchetypes, ref this.Filter, this.QueryIncludesEnableableComponents, ref this.OutFilteredChunkIndices);
		}

		// Token: 0x04000500 RID: 1280
		public UnsafeCachedChunkList CachedChunkList;

		// Token: 0x04000501 RID: 1281
		public EntityQueryFilter Filter;

		// Token: 0x04000502 RID: 1282
		[NoAlias]
		public UnsafeMatchingArchetypePtrList MatchingArchetypes;

		// Token: 0x04000503 RID: 1283
		[NoAlias]
		public NativeArray<int> OutFilteredChunkIndices;

		// Token: 0x04000504 RID: 1284
		public int QueryIncludesEnableableComponents;
	}
}
