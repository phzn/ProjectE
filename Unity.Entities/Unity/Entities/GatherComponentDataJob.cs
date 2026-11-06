using System;
using System.Threading;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x020001DB RID: 475
	[BurstCompile]
	internal struct GatherComponentDataJob : IJobChunk
	{
		// Token: 0x06000B12 RID: 2834 RVA: 0x00034C68 File Offset: 0x00032E68
		public unsafe void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			Archetype* archetype = chunk.Archetype.Archetype;
			int indexInTypeArray = ChunkDataUtility.GetIndexInTypeArray(archetype, this.TypeHandle.m_TypeIndex);
			ushort num = archetype->SizeOfs[indexInTypeArray];
			int num2 = this.ChunkBaseEntityIndices[unfilteredChunkIndex];
			byte* ptr = this.OutputList.Ptr + num2 * (int)num;
			byte* componentDataWithTypeRO = ChunkDataUtility.GetComponentDataWithTypeRO(chunk.m_Chunk, archetype, 0, this.TypeHandle.m_TypeIndex);
			int count = chunk.Count;
			int num3 = (useEnabledMask ? EnabledBitUtility.countbits(chunkEnabledMask) : count);
			if (useEnabledMask)
			{
				int num4 = 0;
				int num5;
				while (EnabledBitUtility.TryGetNextRange(chunkEnabledMask, num4, out num5, out num4))
				{
					int num6 = num4 - num5;
					UnsafeUtility.MemCpy((void*)ptr, (void*)(componentDataWithTypeRO + num5 * (int)num), (long)(num6 * (int)num));
					ptr += num6 * (int)num;
				}
			}
			else
			{
				UnsafeUtility.MemCpy((void*)ptr, (void*)componentDataWithTypeRO, (long)(chunk.Count * (int)num));
			}
			Interlocked.Add(ref *this.OutputList.Length, num3);
		}

		// Token: 0x06000B13 RID: 2835 RVA: 0x00034D59 File Offset: 0x00032F59
		void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
		}

		// Token: 0x0400051B RID: 1307
		[NativeDisableParallelForRestriction]
		public TypelessUnsafeList OutputList;

		// Token: 0x0400051C RID: 1308
		[ReadOnly]
		public DynamicComponentTypeHandle TypeHandle;

		// Token: 0x0400051D RID: 1309
		[ReadOnly]
		[DeallocateOnJobCompletion]
		public NativeArray<int> ChunkBaseEntityIndices;
	}
}
