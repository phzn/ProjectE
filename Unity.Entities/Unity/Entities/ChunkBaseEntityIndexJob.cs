using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Unity.Entities
{
	// Token: 0x020001D5 RID: 469
	[BurstCompile]
	internal struct ChunkBaseEntityIndexJob : IJob
	{
		// Token: 0x06000B0A RID: 2826 RVA: 0x0003477E File Offset: 0x0003297E
		public void Execute()
		{
			ChunkIterationUtility.CalculateBaseEntityIndexArray(in this.CachedChunkList, in this.MatchingArchetypes, ref this.Filter, this.QueryIncludesEnableableComponents, ref this.OutChunkBaseEntityIndices);
		}

		// Token: 0x04000505 RID: 1285
		public UnsafeCachedChunkList CachedChunkList;

		// Token: 0x04000506 RID: 1286
		public EntityQueryFilter Filter;

		// Token: 0x04000507 RID: 1287
		[NoAlias]
		public UnsafeMatchingArchetypePtrList MatchingArchetypes;

		// Token: 0x04000508 RID: 1288
		[NoAlias]
		public NativeArray<int> OutChunkBaseEntityIndices;

		// Token: 0x04000509 RID: 1289
		public int QueryIncludesEnableableComponents;
	}
}
