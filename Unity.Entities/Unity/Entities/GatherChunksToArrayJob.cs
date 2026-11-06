using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace Unity.Entities
{
	// Token: 0x020001D1 RID: 465
	[BurstCompile]
	[Obsolete("Remove with CreateArchetypeChunkArrayAsync. (RemovedAfter Entities 1.0)")]
	internal struct GatherChunksToArrayJob : IJobParallelFor
	{
		// Token: 0x06000B06 RID: 2822 RVA: 0x000345FC File Offset: 0x000327FC
		public unsafe void Execute(int index)
		{
			Archetype* archetype = ((IntPtr*)(this.MatchingArchetypes + (IntPtr)index * (IntPtr)sizeof(MatchingArchetype*) / (IntPtr)sizeof(MatchingArchetype*)))->Archetype;
			int count = archetype->Chunks.Count;
			int num = this.Offsets[index];
			for (int i = 0; i < count; i++)
			{
				ChunkIndex chunkIndex = archetype->Chunks[i];
				this.Chunks[num + i] = new ArchetypeChunk(chunkIndex, this.entityComponentStore);
			}
		}

		// Token: 0x040004F2 RID: 1266
		[NativeDisableUnsafePtrRestriction]
		public unsafe EntityComponentStore* entityComponentStore;

		// Token: 0x040004F3 RID: 1267
		[NativeDisableUnsafePtrRestriction]
		public unsafe MatchingArchetype** MatchingArchetypes;

		// Token: 0x040004F4 RID: 1268
		[DeallocateOnJobCompletion]
		[ReadOnly]
		public NativeArray<int> Offsets;

		// Token: 0x040004F5 RID: 1269
		[NativeDisableParallelForRestriction]
		public NativeArray<ArchetypeChunk> Chunks;
	}
}
