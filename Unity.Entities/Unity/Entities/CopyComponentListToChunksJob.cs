using System;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x020001DD RID: 477
	[BurstCompile]
	internal struct CopyComponentListToChunksJob : IJobChunk
	{
		// Token: 0x06000B16 RID: 2838 RVA: 0x00034E30 File Offset: 0x00033030
		public unsafe void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			Archetype* archetype = chunk.Archetype.Archetype;
			int indexInTypeArray = ChunkDataUtility.GetIndexInTypeArray(archetype, this.TypeHandle.m_TypeIndex);
			ushort num = archetype->SizeOfs[indexInTypeArray];
			int num2 = this.ChunkBaseEntityIndices[unfilteredChunkIndex];
			byte* componentDataWithTypeRW = ChunkDataUtility.GetComponentDataWithTypeRW(chunk.m_Chunk, archetype, 0, this.TypeHandle.m_TypeIndex, this.TypeHandle.GlobalSystemVersion);
			byte* ptr = this.InputList.Ptr + num2 * (int)num;
			int count = chunk.Count;
			if (useEnabledMask)
			{
				EnabledBitUtility.countbits(chunkEnabledMask);
			}
			if (useEnabledMask)
			{
				int num3 = 0;
				int num4;
				while (EnabledBitUtility.TryGetNextRange(chunkEnabledMask, num3, out num4, out num3))
				{
					int num5 = num3 - num4;
					UnsafeUtility.MemCpy((void*)(componentDataWithTypeRW + num4 * (int)num), (void*)ptr, (long)(num5 * (int)num));
					ptr += num5 * (int)num;
				}
			}
			else
			{
				UnsafeUtility.MemCpy((void*)componentDataWithTypeRW, (void*)ptr, (long)(chunk.Count * (int)num));
			}
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x00034F14 File Offset: 0x00033114
		void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
		}

		// Token: 0x04000522 RID: 1314
		[ReadOnly]
		[NativeDisableParallelForRestriction]
		public TypelessUnsafeList InputList;

		// Token: 0x04000523 RID: 1315
		public DynamicComponentTypeHandle TypeHandle;

		// Token: 0x04000524 RID: 1316
		[ReadOnly]
		[DeallocateOnJobCompletion]
		public NativeArray<int> ChunkBaseEntityIndices;
	}
}
