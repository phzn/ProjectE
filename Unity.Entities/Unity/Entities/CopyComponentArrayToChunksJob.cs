using System;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x020001DC RID: 476
	[Obsolete("Use CopyComponentListToChunksJob instead. (RemovedAfter Entities 1.0)")]
	[BurstCompile]
	internal struct CopyComponentArrayToChunksJob : IJobChunk
	{
		// Token: 0x06000B14 RID: 2836 RVA: 0x00034D68 File Offset: 0x00032F68
		public unsafe void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			Archetype* archetype = chunk.Archetype.Archetype;
			int indexInTypeArray = ChunkDataUtility.GetIndexInTypeArray(archetype, this.TypeIndex);
			ushort num = archetype->SizeOfs[indexInTypeArray];
			int num2 = this.ChunkBaseEntityIndices[unfilteredChunkIndex];
			byte* componentDataWithTypeRW = ChunkDataUtility.GetComponentDataWithTypeRW(chunk.m_Chunk, archetype, 0, this.TypeIndex, this.GlobalSystemVersion);
			byte* ptr = this.ComponentData + num2 * (int)num;
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

		// Token: 0x06000B15 RID: 2837 RVA: 0x00034E20 File Offset: 0x00033020
		void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
		}

		// Token: 0x0400051E RID: 1310
		[NativeDisableUnsafePtrRestriction]
		public unsafe byte* ComponentData;

		// Token: 0x0400051F RID: 1311
		public TypeIndex TypeIndex;

		// Token: 0x04000520 RID: 1312
		public uint GlobalSystemVersion;

		// Token: 0x04000521 RID: 1313
		[ReadOnly]
		[DeallocateOnJobCompletion]
		public NativeArray<int> ChunkBaseEntityIndices;
	}
}
