using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Burst.Intrinsics;

namespace Unity.Entities.Internal
{
	// Token: 0x0200045A RID: 1114
	[EditorBrowsable(EditorBrowsableState.Never)]
	[NoAlias]
	public struct InternalEntityQueryEnumerator : IDisposable
	{
		// Token: 0x06001B9F RID: 7071 RVA: 0x0005F964 File Offset: 0x0005DB64
		[MethodImpl(MethodImplOptions.NoInlining)]
		public unsafe InternalEntityQueryEnumerator(EntityQuery query)
		{
			this.IndexInChunk = -1;
			this._EndIndexInChunk = 0;
			this._UseEnableBitsForChunk = 0;
			this._EnableBitMask = default(v128);
			EntityQueryImpl* ptr = query._GetImpl();
			this._QueryHasFilterOrEnableable = ((ptr->_Filter.RequiresMatchesFilter || ptr->_QueryData->HasEnableableComponents == 1) ? 1 : 0);
			if (this._QueryHasFilterOrEnableable == 1)
			{
				this._ChunkCacheIterator = new UnsafeChunkCacheIterator(in ptr->_Filter, ptr->_QueryData->HasEnableableComponents > 0, ptr->GetMatchingChunkCache(), ptr->_QueryData->MatchingArchetypes.Ptr);
				this._CachedChunkList = default(UnsafeCachedChunkList);
				this._CachedChunkListLength = -1;
				ptr->SyncFilterTypes();
			}
			else
			{
				this._ChunkCacheIterator = default(UnsafeChunkCacheIterator);
				this._CachedChunkList = ptr->GetMatchingChunkCache();
				this._CachedChunkListLength = this._CachedChunkList.Length;
			}
			this._ChunkIndex = -1;
		}

		// Token: 0x06001BA0 RID: 7072 RVA: 0x00002287 File Offset: 0x00000487
		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Dispose()
		{
		}

		// Token: 0x06001BA1 RID: 7073 RVA: 0x0005FA48 File Offset: 0x0005DC48
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool MoveNextHotLoop()
		{
			this.IndexInChunk++;
			return Hint.Likely(this.IndexInChunk < this._EndIndexInChunk) || (this._UseEnableBitsForChunk != 0 && EnabledBitUtility.TryGetNextRange(this._EnableBitMask, this._EndIndexInChunk, out this.IndexInChunk, out this._EndIndexInChunk));
		}

		// Token: 0x06001BA2 RID: 7074 RVA: 0x0005FAA4 File Offset: 0x0005DCA4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe bool MoveNextEntityRange(out bool movedToNewChunk, out ArchetypeChunk chunk, out int entityStartIndex, out int entityEndIndex)
		{
			if (this._QueryHasFilterOrEnableable == 0)
			{
				this._ChunkIndex++;
				if (Hint.Unlikely(this._ChunkIndex >= this._CachedChunkListLength))
				{
					chunk = default(ArchetypeChunk);
					movedToNewChunk = false;
					this._EndIndexInChunk = 0;
					entityStartIndex = -1;
					entityEndIndex = -1;
					return false;
				}
				chunk = new ArchetypeChunk(this._CachedChunkList.ChunkIndices[this._ChunkIndex], this._CachedChunkList.EntityComponentStore);
				movedToNewChunk = true;
				entityStartIndex = 0;
				entityEndIndex = chunk.Count;
				this._EndIndexInChunk = 0;
				return true;
			}
			else
			{
				if (this._UseEnableBitsForChunk != 0 && EnabledBitUtility.TryGetNextRange(this._EnableBitMask, this._EndIndexInChunk, out entityStartIndex, out this._EndIndexInChunk))
				{
					chunk = default(ArchetypeChunk);
					movedToNewChunk = false;
					entityEndIndex = this._EndIndexInChunk;
					return true;
				}
				this._EndIndexInChunk = 0;
				int num;
				if (Hint.Unlikely(!this._ChunkCacheIterator.MoveNextChunk(ref this._ChunkIndex, out chunk, out num, out this._UseEnableBitsForChunk, ref this._EnableBitMask)))
				{
					movedToNewChunk = false;
					entityStartIndex = -1;
					entityEndIndex = -1;
					return false;
				}
				if (Hint.Likely(this._UseEnableBitsForChunk == 0))
				{
					movedToNewChunk = true;
					entityStartIndex = 0;
					entityEndIndex = num;
					return true;
				}
				movedToNewChunk = Hint.Likely(EnabledBitUtility.TryGetNextRange(this._EnableBitMask, this._EndIndexInChunk, out entityStartIndex, out this._EndIndexInChunk));
				entityEndIndex = this._EndIndexInChunk;
				return movedToNewChunk;
			}
		}

		// Token: 0x06001BA3 RID: 7075 RVA: 0x0005FC04 File Offset: 0x0005DE04
		[MethodImpl(MethodImplOptions.NoInlining)]
		public unsafe bool MoveNextColdLoop(out ArchetypeChunk chunk)
		{
			if (this._QueryHasFilterOrEnableable == 0)
			{
				this._ChunkIndex++;
				if (Hint.Unlikely(this._ChunkIndex >= this._CachedChunkList.Length))
				{
					chunk = default(ArchetypeChunk);
					return false;
				}
				chunk = new ArchetypeChunk(this._CachedChunkList.ChunkIndices[this._ChunkIndex], this._CachedChunkList.EntityComponentStore);
				this.IndexInChunk = 0;
				this._EndIndexInChunk = chunk.Count;
				return true;
			}
			else
			{
				int num;
				if (Hint.Unlikely(!this._ChunkCacheIterator.MoveNextChunk(ref this._ChunkIndex, out chunk, out num, out this._UseEnableBitsForChunk, ref this._EnableBitMask)))
				{
					return false;
				}
				if (Hint.Unlikely(this._UseEnableBitsForChunk > 0))
				{
					this._EndIndexInChunk = 0;
					EnabledBitUtility.TryGetNextRange(this._EnableBitMask, this._EndIndexInChunk, out this.IndexInChunk, out this._EndIndexInChunk);
				}
				else
				{
					this.IndexInChunk = 0;
					this._EndIndexInChunk = num;
				}
				return true;
			}
		}

		// Token: 0x06001BA4 RID: 7076 RVA: 0x00002287 File Offset: 0x00000487
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public void CheckDisposed()
		{
		}

		// Token: 0x04000984 RID: 2436
		public int IndexInChunk;

		// Token: 0x04000985 RID: 2437
		private int _EndIndexInChunk;

		// Token: 0x04000986 RID: 2438
		private int _ChunkIndex;

		// Token: 0x04000987 RID: 2439
		private byte _QueryHasFilterOrEnableable;

		// Token: 0x04000988 RID: 2440
		private byte _UseEnableBitsForChunk;

		// Token: 0x04000989 RID: 2441
		private v128 _EnableBitMask;

		// Token: 0x0400098A RID: 2442
		private UnsafeChunkCacheIterator _ChunkCacheIterator;

		// Token: 0x0400098B RID: 2443
		private UnsafeCachedChunkList _CachedChunkList;

		// Token: 0x0400098C RID: 2444
		private int _CachedChunkListLength;
	}
}
