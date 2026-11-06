using System;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x020001D8 RID: 472
	[BurstCompile]
	[Obsolete("Only used by obsolete code paths. (RemovedAfter Entities 1.0)")]
	internal struct GatherEntitiesToArrayJob : IJobChunk
	{
		// Token: 0x06000B0C RID: 2828 RVA: 0x00034A10 File Offset: 0x00032C10
		public unsafe void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			int num = this.ChunkBaseEntityIndices[unfilteredChunkIndex];
			Entity* ptr = (Entity*)(this.Entities + (IntPtr)num * (IntPtr)sizeof(Entity));
			Entity* entityDataPtrRO = chunk.GetEntityDataPtrRO(this.EntityTypeHandle);
			if (useEnabledMask)
			{
				int num2 = 0;
				int num3 = 0;
				int num4;
				while (EnabledBitUtility.TryGetNextRange(chunkEnabledMask, num2, out num4, out num2))
				{
					int num5 = num2 - num4;
					UnsafeUtility.MemCpy((void*)(ptr + num3), (void*)(entityDataPtrRO + num4), (long)(num5 * sizeof(Entity)));
					num3 += num5;
				}
			}
			else
			{
				UnsafeUtility.MemCpy((void*)ptr, (void*)entityDataPtrRO, (long)(chunk.Count * sizeof(Entity)));
			}
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x00034AAE File Offset: 0x00032CAE
		void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
		}

		// Token: 0x04000512 RID: 1298
		[NativeDisableUnsafePtrRestriction]
		public unsafe byte* Entities;

		// Token: 0x04000513 RID: 1299
		[ReadOnly]
		public EntityTypeHandle EntityTypeHandle;

		// Token: 0x04000514 RID: 1300
		[ReadOnly]
		[DeallocateOnJobCompletion]
		public NativeArray<int> ChunkBaseEntityIndices;
	}
}
