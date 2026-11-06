using System;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Burst.Intrinsics;
using Unity.Jobs;

namespace Unity.Entities
{
	// Token: 0x020001D7 RID: 471
	[BurstCompile]
	internal struct GatherChunksJob : IJob
	{
		// Token: 0x06000B0B RID: 2827 RVA: 0x000347A4 File Offset: 0x000329A4
		public unsafe void Execute()
		{
			int num = 0;
			MatchingArchetype** ptr = this.MatchingArchetypes.Ptr;
			bool requiresMatchesFilter = this.Filter.RequiresMatchesFilter;
			bool flag = this.QueryContainsEnableableComponents == 1;
			ChunkIndex* chunkIndices = this.ChunkCache.ChunkIndices;
			int* ptr2 = this.ChunkCache.PerChunkMatchingArchetypeIndex->Ptr;
			int* ptr3 = this.ChunkCache.ChunkIndexInArchetype->Ptr;
			int length = this.ChunkCache.Length;
			int num2 = -1;
			MatchingArchetype* ptr4 = null;
			ChunkIterationUtility.EnabledMaskMatchingArchetypeState enabledMaskMatchingArchetypeState = default(ChunkIterationUtility.EnabledMaskMatchingArchetypeState);
			int* ptr5 = null;
			ArchetypeChunk* ptr6 = (ArchetypeChunk*)this.OutFilteredChunksList.Ptr;
			EntityComponentStore* entityComponentStore = this.ChunkCache.EntityComponentStore;
			if (!requiresMatchesFilter && !flag)
			{
				for (int i = 0; i < length; i++)
				{
					ptr6[i] = new ArchetypeChunk(chunkIndices[i], entityComponentStore);
				}
				num = length;
			}
			else if (flag)
			{
				for (int j = 0; j < length; j++)
				{
					if (Hint.Unlikely(ptr2[j] != num2))
					{
						num2 = ptr2[j];
						ptr4 = *(IntPtr*)(ptr + (IntPtr)num2 * (IntPtr)sizeof(MatchingArchetype*) / (IntPtr)sizeof(MatchingArchetype*));
						ptr5 = ptr4->Archetype->Chunks.GetChunkEntityCountArray();
						enabledMaskMatchingArchetypeState = new ChunkIterationUtility.EnabledMaskMatchingArchetypeState(ptr4);
					}
					int num3 = ptr3[j];
					if (!requiresMatchesFilter || ptr4->ChunkMatchesFilter(num3, ref this.Filter))
					{
						int num4 = ptr5[num3];
						v128 v;
						ChunkIterationUtility.GetEnabledMask(num3, num4, in enabledMaskMatchingArchetypeState, out v);
						if (v.ULong0 != 0UL || v.ULong1 != 0UL)
						{
							ptr6[(IntPtr)(num++) * (IntPtr)sizeof(ArchetypeChunk)] = new ArchetypeChunk(chunkIndices[j], entityComponentStore);
						}
					}
				}
			}
			else
			{
				for (int k = 0; k < length; k++)
				{
					if (Hint.Unlikely(ptr2[k] != num2))
					{
						num2 = ptr2[k];
						ptr4 = *(IntPtr*)(ptr + (IntPtr)num2 * (IntPtr)sizeof(MatchingArchetype*) / (IntPtr)sizeof(MatchingArchetype*));
					}
					int num5 = ptr3[k];
					if (ptr4->ChunkMatchesFilter(num5, ref this.Filter))
					{
						ptr6[(IntPtr)(num++) * (IntPtr)sizeof(ArchetypeChunk)] = new ArchetypeChunk(chunkIndices[k], entityComponentStore);
					}
				}
			}
			*this.OutFilteredChunksList.Length = num;
		}

		// Token: 0x0400050D RID: 1293
		public UnsafeCachedChunkList ChunkCache;

		// Token: 0x0400050E RID: 1294
		public EntityQueryFilter Filter;

		// Token: 0x0400050F RID: 1295
		public UnsafeMatchingArchetypePtrList MatchingArchetypes;

		// Token: 0x04000510 RID: 1296
		public int QueryContainsEnableableComponents;

		// Token: 0x04000511 RID: 1297
		public TypelessUnsafeList OutFilteredChunksList;
	}
}
