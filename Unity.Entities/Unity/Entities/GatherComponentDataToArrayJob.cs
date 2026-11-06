using System;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x020001DA RID: 474
	[Obsolete("Use GatherComponentDataJob instead. (RemovedAfter Entities 1.0)")]
	[BurstCompile]
	internal struct GatherComponentDataToArrayJob : IJobChunk
	{
		// Token: 0x06000B10 RID: 2832 RVA: 0x00034BA0 File Offset: 0x00032DA0
		public unsafe void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			Archetype* archetype = chunk.Archetype.Archetype;
			int indexInTypeArray = ChunkDataUtility.GetIndexInTypeArray(archetype, this.TypeIndex);
			int num = archetype->Offsets[indexInTypeArray];
			ushort num2 = archetype->SizeOfs[indexInTypeArray];
			int num3 = this.ChunkBaseEntityIndices[unfilteredChunkIndex];
			ChunkIndex chunk2 = chunk.m_Chunk;
			byte* ptr = chunk2.Buffer + num;
			byte* ptr2 = this.ComponentData + num3 * (int)num2;
			if (useEnabledMask)
			{
				int num4 = 0;
				int num5;
				while (EnabledBitUtility.TryGetNextRange(chunkEnabledMask, num4, out num5, out num4))
				{
					int num6 = num4 - num5;
					UnsafeUtility.MemCpy((void*)ptr2, (void*)(ptr + num5 * (int)num2), (long)(num6 * (int)num2));
					ptr2 += num6 * (int)num2;
				}
			}
			else
			{
				UnsafeUtility.MemCpy((void*)ptr2, (void*)ptr, (long)(chunk.Count * (int)num2));
			}
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x00034C5B File Offset: 0x00032E5B
		void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
		}

		// Token: 0x04000518 RID: 1304
		[NativeDisableUnsafePtrRestriction]
		public unsafe byte* ComponentData;

		// Token: 0x04000519 RID: 1305
		public TypeIndex TypeIndex;

		// Token: 0x0400051A RID: 1306
		[ReadOnly]
		[DeallocateOnJobCompletion]
		public NativeArray<int> ChunkBaseEntityIndices;
	}
}
