using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x02000235 RID: 565
	[NoAlias]
	[GenerateTestsForBurstCompatibility]
	internal struct UnsafeChunkCacheIterator
	{
		// Token: 0x06000DF0 RID: 3568 RVA: 0x00042B14 File Offset: 0x00040D14
		internal unsafe UnsafeChunkCacheIterator(in EntityQueryFilter filter, bool hasEnableableComponents, UnsafeCachedChunkList list, MatchingArchetype** matchingArchetypes)
		{
			this._Chunks = list.ChunkIndices;
			this.Length = list.Length;
			this._EntityComponentStore = list.EntityComponentStore;
			this._MatchingArchetypes = matchingArchetypes;
			this._PerChunkMatchingArchetypeIndex = list.PerChunkMatchingArchetypeIndex->Ptr;
			this._ChunkIndexInArchetype = list.ChunkIndexInArchetype->Ptr;
			this._Filter = filter;
			EntityQueryFilter entityQueryFilter = filter;
			this._RequireMatchesFilter = (entityQueryFilter.RequiresMatchesFilter ? 1 : 0);
			this._QueryHasEnableableComponents = (hasEnableableComponents ? 1 : 0);
			this._CurrentMatchingArchetypeIndex = -1;
			this._CurrentMatchingArchetype = null;
			this._CurrentArchetypeChunkEntityCounts = null;
			this._CurrentArchetypeState = default(ChunkIterationUtility.EnabledMaskMatchingArchetypeState);
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x00042BC8 File Offset: 0x00040DC8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe bool MoveNextChunk(ref int chunkIndexInCache, out ArchetypeChunk outputChunk, out int outputChunkEntityCount, out byte outputUseEnableBits, ref v128 enableBits)
		{
			for (chunkIndexInCache++; chunkIndexInCache < this.Length; chunkIndexInCache++)
			{
				bool flag = false;
				int num = this._ChunkIndexInArchetype[chunkIndexInCache];
				if (Hint.Unlikely(this._CurrentMatchingArchetypeIndex != this._PerChunkMatchingArchetypeIndex[chunkIndexInCache]))
				{
					this._CurrentMatchingArchetypeIndex = this._PerChunkMatchingArchetypeIndex[chunkIndexInCache];
					this._CurrentMatchingArchetype = *(IntPtr*)(this._MatchingArchetypes + (IntPtr)this._CurrentMatchingArchetypeIndex * (IntPtr)sizeof(MatchingArchetype*) / (IntPtr)sizeof(MatchingArchetype*));
					this._CurrentArchetypeChunkEntityCounts = this._CurrentMatchingArchetype->Archetype->Chunks.GetChunkEntityCountArray();
					if (this._QueryHasEnableableComponents != 0)
					{
						this._CurrentArchetypeState = new ChunkIterationUtility.EnabledMaskMatchingArchetypeState(this._CurrentMatchingArchetype);
					}
				}
				if (this._RequireMatchesFilter == 0 || this._CurrentMatchingArchetype->ChunkMatchesFilter(num, ref this._Filter))
				{
					if (this._QueryHasEnableableComponents != 0)
					{
						int num2 = this._CurrentArchetypeChunkEntityCounts[num];
						ChunkIterationUtility.GetEnabledMask(num, num2, in this._CurrentArchetypeState, out enableBits);
						if (enableBits.ULong0 == 0UL && enableBits.ULong1 == 0UL)
						{
							chunkIndexInCache++;
							continue;
						}
						flag = EnabledBitUtility.countbits(enableBits) != num2;
					}
					outputChunkEntityCount = this._CurrentArchetypeChunkEntityCounts[num];
					outputUseEnableBits = (flag ? 1 : 0);
					outputChunk = new ArchetypeChunk(this._Chunks[chunkIndexInCache], this._EntityComponentStore);
					return true;
				}
			}
			outputUseEnableBits = 0;
			outputChunkEntityCount = 0;
			outputChunk = default(ArchetypeChunk);
			return false;
		}

		// Token: 0x040005EC RID: 1516
		[NoAlias]
		public int Length;

		// Token: 0x040005ED RID: 1517
		[NoAlias]
		[NativeDisableUnsafePtrRestriction]
		private unsafe ChunkIndex* _Chunks;

		// Token: 0x040005EE RID: 1518
		[NativeDisableUnsafePtrRestriction]
		[NoAlias]
		private unsafe EntityComponentStore* _EntityComponentStore;

		// Token: 0x040005EF RID: 1519
		[NoAlias]
		private unsafe int* _PerChunkMatchingArchetypeIndex;

		// Token: 0x040005F0 RID: 1520
		[NoAlias]
		private unsafe int* _ChunkIndexInArchetype;

		// Token: 0x040005F1 RID: 1521
		[NoAlias]
		private unsafe MatchingArchetype** _MatchingArchetypes;

		// Token: 0x040005F2 RID: 1522
		private EntityQueryFilter _Filter;

		// Token: 0x040005F3 RID: 1523
		private byte _RequireMatchesFilter;

		// Token: 0x040005F4 RID: 1524
		private byte _QueryHasEnableableComponents;

		// Token: 0x040005F5 RID: 1525
		private int _CurrentMatchingArchetypeIndex;

		// Token: 0x040005F6 RID: 1526
		[NoAlias]
		public unsafe MatchingArchetype* _CurrentMatchingArchetype;

		// Token: 0x040005F7 RID: 1527
		[NoAlias]
		private unsafe int* _CurrentArchetypeChunkEntityCounts;

		// Token: 0x040005F8 RID: 1528
		private ChunkIterationUtility.EnabledMaskMatchingArchetypeState _CurrentArchetypeState;
	}
}
