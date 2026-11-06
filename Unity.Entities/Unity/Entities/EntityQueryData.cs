using System;

namespace Unity.Entities
{
	// Token: 0x0200022F RID: 559
	internal struct EntityQueryData : IDisposable
	{
		// Token: 0x06000DD5 RID: 3541 RVA: 0x00042947 File Offset: 0x00040B47
		internal unsafe void CreateMatchingChunkCache(EntityComponentStore* ecs)
		{
			this.MatchingChunkCache = new UnsafeCachedChunkList(ecs);
		}

		// Token: 0x06000DD6 RID: 3542 RVA: 0x00042955 File Offset: 0x00040B55
		internal void RebuildMatchingChunkCache()
		{
			UnsafeCachedChunkList.Rebuild(ref this.MatchingChunkCache, in this);
		}

		// Token: 0x06000DD7 RID: 3543 RVA: 0x00042963 File Offset: 0x00040B63
		internal UnsafeCachedChunkList UnsafeGetMatchingChunkCache()
		{
			return this.MatchingChunkCache;
		}

		// Token: 0x06000DD8 RID: 3544 RVA: 0x0004296B File Offset: 0x00040B6B
		internal void CheckChunkListCacheConsistency(bool forceCheckInvalidCache)
		{
			UnsafeCachedChunkList.AssertIsConsistent(in this.MatchingChunkCache, in this, forceCheckInvalidCache);
		}

		// Token: 0x06000DD9 RID: 3545 RVA: 0x0004297A File Offset: 0x00040B7A
		internal void InvalidateChunkCache()
		{
			this.MatchingChunkCache.Invalidate();
		}

		// Token: 0x06000DDA RID: 3546 RVA: 0x00042987 File Offset: 0x00040B87
		internal bool IsChunkCacheValid()
		{
			return this.MatchingChunkCache.IsCacheValid;
		}

		// Token: 0x06000DDB RID: 3547 RVA: 0x00042994 File Offset: 0x00040B94
		public void Dispose()
		{
			this.MatchingArchetypes.Dispose();
			this.MatchingChunkCache.Dispose();
		}

		// Token: 0x040005D9 RID: 1497
		public unsafe ComponentType* RequiredComponents;

		// Token: 0x040005DA RID: 1498
		public int RequiredComponentsCount;

		// Token: 0x040005DB RID: 1499
		public unsafe TypeIndex* ReaderTypes;

		// Token: 0x040005DC RID: 1500
		public int ReaderTypesCount;

		// Token: 0x040005DD RID: 1501
		public unsafe TypeIndex* WriterTypes;

		// Token: 0x040005DE RID: 1502
		public int WriterTypesCount;

		// Token: 0x040005DF RID: 1503
		public unsafe TypeIndex* EnableableComponentTypeIndices;

		// Token: 0x040005E0 RID: 1504
		public int EnableableComponentTypeIndexCount;

		// Token: 0x040005E1 RID: 1505
		public unsafe ArchetypeQuery* ArchetypeQueries;

		// Token: 0x040005E2 RID: 1506
		public int ArchetypeQueryCount;

		// Token: 0x040005E3 RID: 1507
		public EntityQueryMask EntityQueryMask;

		// Token: 0x040005E4 RID: 1508
		public UnsafeMatchingArchetypePtrList MatchingArchetypes;

		// Token: 0x040005E5 RID: 1509
		private UnsafeCachedChunkList MatchingChunkCache;

		// Token: 0x040005E6 RID: 1510
		public byte HasEnableableComponents;
	}
}
