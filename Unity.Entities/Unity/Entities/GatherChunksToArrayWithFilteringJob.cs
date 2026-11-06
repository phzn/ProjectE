using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace Unity.Entities
{
	// Token: 0x020001D2 RID: 466
	[BurstCompile]
	[Obsolete("Remove with CreateArchetypeChunkArrayAsync. (RemovedAfter Entities 1.0)")]
	internal struct GatherChunksToArrayWithFilteringJob : IJobParallelFor
	{
		// Token: 0x06000B07 RID: 2823 RVA: 0x00034670 File Offset: 0x00032870
		public unsafe void Execute(int index)
		{
			int num = 0;
			MatchingArchetype* ptr = *(IntPtr*)(this.MatchingArchetypes + (IntPtr)index * (IntPtr)sizeof(MatchingArchetype*) / (IntPtr)sizeof(MatchingArchetype*));
			Archetype* archetype = ptr->Archetype;
			int count = archetype->Chunks.Count;
			int num2 = this.Offsets[index];
			ArchetypeChunkData chunks = archetype->Chunks;
			for (int i = 0; i < count; i++)
			{
				if (ptr->ChunkMatchesFilter(i, ref this.Filter))
				{
					this.SparseChunks[num2 + num++] = new ArchetypeChunk(chunks[i], this.entityComponentStore);
				}
			}
			this.FilteredCounts[index] = num;
		}

		// Token: 0x040004F6 RID: 1270
		[NativeDisableUnsafePtrRestriction]
		public unsafe EntityComponentStore* entityComponentStore;

		// Token: 0x040004F7 RID: 1271
		[NativeDisableUnsafePtrRestriction]
		public unsafe MatchingArchetype** MatchingArchetypes;

		// Token: 0x040004F8 RID: 1272
		public EntityQueryFilter Filter;

		// Token: 0x040004F9 RID: 1273
		[ReadOnly]
		public NativeArray<int> Offsets;

		// Token: 0x040004FA RID: 1274
		public NativeArray<int> FilteredCounts;

		// Token: 0x040004FB RID: 1275
		[NativeDisableParallelForRestriction]
		public NativeArray<ArchetypeChunk> SparseChunks;
	}
}
