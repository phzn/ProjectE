using System;
using System.Threading;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x020001D9 RID: 473
	[BurstCompile]
	internal struct GatherEntitiesJob : IJobChunk
	{
		// Token: 0x06000B0E RID: 2830 RVA: 0x00034ABC File Offset: 0x00032CBC
		public unsafe void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			int num = this.ChunkBaseEntityIndices[unfilteredChunkIndex];
			Entity* ptr = (Entity*)(this.OutputList.Ptr + (IntPtr)num * (IntPtr)sizeof(Entity));
			Entity* entityDataPtrRO = chunk.GetEntityDataPtrRO(this.EntityTypeHandle);
			int count = chunk.Count;
			int num2 = (useEnabledMask ? EnabledBitUtility.countbits(chunkEnabledMask) : count);
			if (useEnabledMask)
			{
				int num3 = 0;
				int num4 = 0;
				int num5;
				while (EnabledBitUtility.TryGetNextRange(chunkEnabledMask, num3, out num5, out num3))
				{
					int num6 = num3 - num5;
					UnsafeUtility.MemCpy((void*)(ptr + num4), (void*)(entityDataPtrRO + num5), (long)(num6 * sizeof(Entity)));
					num4 += num6;
				}
			}
			else
			{
				UnsafeUtility.MemCpy((void*)ptr, (void*)entityDataPtrRO, (long)(chunk.Count * sizeof(Entity)));
			}
			Interlocked.Add(ref *this.OutputList.Length, num2);
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x00034B90 File Offset: 0x00032D90
		void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
		}

		// Token: 0x04000515 RID: 1301
		[NativeDisableParallelForRestriction]
		public TypelessUnsafeList OutputList;

		// Token: 0x04000516 RID: 1302
		[ReadOnly]
		public EntityTypeHandle EntityTypeHandle;

		// Token: 0x04000517 RID: 1303
		[ReadOnly]
		[DeallocateOnJobCompletion]
		public NativeArray<int> ChunkBaseEntityIndices;
	}
}
