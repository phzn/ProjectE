using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x0200022C RID: 556
	[NoAlias]
	[BurstCompile]
	internal struct UnsafeCachedChunkList
	{
		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000DC1 RID: 3521 RVA: 0x0004203F File Offset: 0x0004023F
		internal unsafe ChunkIndex* ChunkIndices
		{
			get
			{
				return this.MatchingChunks->Ptr;
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000DC2 RID: 3522 RVA: 0x0004204C File Offset: 0x0004024C
		public unsafe int Length
		{
			get
			{
				return this.MatchingChunks->Length;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000DC3 RID: 3523 RVA: 0x00042059 File Offset: 0x00040259
		public bool IsCacheValid
		{
			get
			{
				return this.CacheValid != 0;
			}
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x00042064 File Offset: 0x00040264
		internal unsafe UnsafeCachedChunkList(EntityComponentStore* entityComponentStore)
		{
			this.EntityComponentStore = entityComponentStore;
			this.MatchingChunks = UnsafeList<ChunkIndex>.Create(0, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			this.PerChunkMatchingArchetypeIndex = UnsafeList<int>.Create(0, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			this.ChunkIndexInArchetype = UnsafeList<int>.Create(0, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			this.CacheValid = 0;
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x000420B8 File Offset: 0x000402B8
		internal unsafe void Append(ChunkIndex* t, int addChunkCount, int matchingArchetypeIndex)
		{
			this.MatchingChunks->AddRange(new UnsafeList<ChunkIndex>(t, addChunkCount));
			for (int i = 0; i < addChunkCount; i++)
			{
				this.PerChunkMatchingArchetypeIndex->Add(in matchingArchetypeIndex);
				this.ChunkIndexInArchetype->Add(in i);
			}
		}

		// Token: 0x06000DC6 RID: 3526 RVA: 0x00042100 File Offset: 0x00040300
		internal void Dispose()
		{
			if (this.MatchingChunks != null)
			{
				UnsafeList<ChunkIndex>.Destroy(this.MatchingChunks);
			}
			if (this.PerChunkMatchingArchetypeIndex != null)
			{
				UnsafeList<int>.Destroy(this.PerChunkMatchingArchetypeIndex);
			}
			if (this.ChunkIndexInArchetype != null)
			{
				UnsafeList<int>.Destroy(this.ChunkIndexInArchetype);
			}
		}

		// Token: 0x06000DC7 RID: 3527 RVA: 0x0004214C File Offset: 0x0004034C
		internal void Invalidate()
		{
			this.CacheValid = 0;
		}

		// Token: 0x06000DC8 RID: 3528 RVA: 0x00042155 File Offset: 0x00040355
		[BurstCompile]
		internal static void Rebuild(ref UnsafeCachedChunkList cache, in EntityQueryData queryData)
		{
			UnsafeCachedChunkList.Rebuild_00000C31$BurstDirectCall.Invoke(ref cache, in queryData);
		}

		// Token: 0x06000DC9 RID: 3529 RVA: 0x00042160 File Offset: 0x00040360
		internal unsafe static void AssertIsConsistent(in UnsafeCachedChunkList cache, in EntityQueryData queryData, bool forceCheckInvalidCache)
		{
			UnsafeCachedChunkList unsafeCachedChunkList;
			if (!forceCheckInvalidCache)
			{
				unsafeCachedChunkList = cache;
				if (!unsafeCachedChunkList.IsCacheValid)
				{
					throw new ArgumentException("Can't check an invalid cache for consistency. Did you call queryData.GetMatchingChunkCache() to access this cache?");
				}
			}
			int length = cache.ChunkIndexInArchetype->Length;
			unsafeCachedChunkList = cache;
			if (length != unsafeCachedChunkList.Length)
			{
				string text = "query chunk cache inconsistency: ChunkIndexInArchetypeLength={0}, cache length={1}";
				object obj = cache.ChunkIndexInArchetype->Length;
				unsafeCachedChunkList = cache;
				throw new ArgumentException(string.Format(text, obj, unsafeCachedChunkList.Length));
			}
			int length2 = cache.PerChunkMatchingArchetypeIndex->Length;
			unsafeCachedChunkList = cache;
			if (length2 != unsafeCachedChunkList.Length)
			{
				string text2 = "query chunk cache inconsistency: PerChunkMatchingArchetypeIndex={0}, cache length={1}";
				object obj2 = cache.PerChunkMatchingArchetypeIndex->Length;
				unsafeCachedChunkList = cache;
				throw new ArgumentException(string.Format(text2, obj2, unsafeCachedChunkList.Length));
			}
			UnsafeMatchingArchetypePtrList unsafeMatchingArchetypePtrList = queryData.MatchingArchetypes;
			int length3 = unsafeMatchingArchetypePtrList.Length;
			unsafeMatchingArchetypePtrList = queryData.MatchingArchetypes;
			MatchingArchetype** ptr = unsafeMatchingArchetypePtrList.Ptr;
			int num = 0;
			for (int i = 0; i < length3; i++)
			{
				Archetype* archetype = ((IntPtr*)(ptr + (IntPtr)i * (IntPtr)sizeof(MatchingArchetype*) / (IntPtr)sizeof(MatchingArchetype*)))->Archetype;
				num += archetype->Chunks.Count;
			}
			int num2 = 0;
			for (int j = 0; j < length3; j++)
			{
				Archetype* archetype2 = ((IntPtr*)(ptr + (IntPtr)j * (IntPtr)sizeof(MatchingArchetype*) / (IntPtr)sizeof(MatchingArchetype*)))->Archetype;
				int count = archetype2->Chunks.Count;
				for (int k = 0; k < count; k++)
				{
					int num3 = num2;
					unsafeCachedChunkList = cache;
					if (num3 >= unsafeCachedChunkList.Length)
					{
						string text3 = "query chunk cache inconsistency: matching archetype chunk {0} exceeds chunk cache length {1}";
						object obj3 = num2;
						unsafeCachedChunkList = cache;
						throw new ArgumentException(string.Format(text3, obj3, unsafeCachedChunkList.Length));
					}
					ChunkIndex chunkIndex = cache.MatchingChunks->Ptr[num2];
					if (chunkIndex != archetype2->Chunks[k])
					{
						throw new ArgumentException(string.Format("query chunk cache inconsistency: cached chunk at index {0} should be at chunkIndex={1} of archetype {2}.\nArchetype has {3} chunks and {4} entities, with types {5}.\nQuery matches {6} archetypes.", new object[]
						{
							num2,
							k,
							j,
							count,
							archetype2->EntityCount,
							archetype2->ToString(),
							length3
						}));
					}
					if (chunkIndex.SequenceNumber != archetype2->Chunks[k].SequenceNumber)
					{
						throw new ArgumentException(string.Format("query chunk cache inconsistency: cached chunk at index {0} has a different sequence number {1} than chunk in matching archetype with sequence number {2}.\nArchetype has {3} chunks and {4} entities, with types {5}.\nQuery matches {6} archetypes.", new object[]
						{
							num2,
							chunkIndex.SequenceNumber,
							archetype2->Chunks[k].SequenceNumber,
							count,
							archetype2->EntityCount,
							archetype2->ToString(),
							length3
						}));
					}
					if (cache.ChunkIndexInArchetype->Ptr[num2] != chunkIndex.ListIndex)
					{
						throw new ArgumentException(string.Format("query chunk cache inconsistency: cached chunk at index {0} thinks it should be at index {1} in archetype {2}'s chunk list, but it's actually at index {3}.\nArchetype has {4} chunks and {5} entities, with types {6}.\nQuery matches {7} archetypes.", new object[]
						{
							num2,
							cache.ChunkIndexInArchetype->Ptr[num2],
							j,
							chunkIndex.ListIndex,
							count,
							archetype2->EntityCount,
							archetype2->ToString(),
							length3
						}));
					}
					if (cache.PerChunkMatchingArchetypeIndex->Ptr[num2] != j)
					{
						throw new ArgumentException(string.Format("query chunk cache inconsistency: cached chunk at index {0} thinks it should be in matching archetype {1}, but it's actually in archetype {2}.\nArchetype has {3} chunks and {4} entities, with types {5}.\nQuery matches {6} archetypes.", new object[]
						{
							num2,
							cache.PerChunkMatchingArchetypeIndex->Ptr[num2],
							j,
							count,
							archetype2->EntityCount,
							archetype2->ToString(),
							length3
						}));
					}
					num2++;
				}
			}
			int num4 = 0;
			ChunkIndex chunkIndex2;
			int num6;
			int num7;
			Archetype* archetype3;
			for (;;)
			{
				int num5 = num4;
				unsafeCachedChunkList = cache;
				if (num5 >= unsafeCachedChunkList.Length)
				{
					goto Block_17;
				}
				chunkIndex2 = cache.MatchingChunks->Ptr[num4];
				num6 = cache.PerChunkMatchingArchetypeIndex->Ptr[num4];
				num7 = cache.ChunkIndexInArchetype->Ptr[num4];
				if (num6 < 0 || num6 >= length3)
				{
					break;
				}
				archetype3 = ((IntPtr*)(ptr + (IntPtr)num6 * (IntPtr)sizeof(MatchingArchetype*) / (IntPtr)sizeof(MatchingArchetype*)))->Archetype;
				if (num7 < 0 || num7 >= archetype3->Chunks.Count)
				{
					goto IL_04AC;
				}
				if (archetype3->Chunks[num7] != chunkIndex2)
				{
					goto Block_15;
				}
				if (archetype3->Chunks[num7].SequenceNumber != chunkIndex2.SequenceNumber)
				{
					goto Block_16;
				}
				num4++;
			}
			throw new ArgumentException(string.Format("query chunk cache inconsistency: matching archetype index {0} for cached chunk {1} is out of range.\nQuery matches {2} archetypes.", num6, num4, length3));
			IL_04AC:
			throw new ArgumentException(string.Format("query chunk cache inconsistency: chunk index in archetype {0} for cached chunk {1} is out of range. Archetype {2} contains {3} chunks.\nArchetype has {4} chunks and {5} entities, with types {6}.\nQuery matches {7} archetypes.", new object[]
			{
				num7,
				num4,
				num6,
				archetype3->Chunks.Count,
				archetype3->Chunks.Count,
				archetype3->EntityCount,
				archetype3->ToString(),
				length3
			}));
			Block_15:
			throw new ArgumentException(string.Format("query chunk cache inconsistency: cached chunk {0} not found at expected chunk index {1} of expected matching archetype {2}.\nArchetype has {3} chunks and {4} entities, with types {5}.\nQuery matches {6} archetypes.", new object[]
			{
				num4,
				num7,
				num6,
				archetype3->Chunks.Count,
				archetype3->EntityCount,
				archetype3->ToString(),
				length3
			}));
			Block_16:
			throw new ArgumentException(string.Format("query chunk cache inconsistency: cached chunk {0} has a different sequence number {1} than chunk in matching archetype with sequence number {2}.\nArchetype has {3} chunks and {4} entities, with types {5}.\nQuery matches {6} archetypes.", new object[]
			{
				num4,
				chunkIndex2.SequenceNumber,
				archetype3->Chunks[num7].SequenceNumber,
				archetype3->Chunks.Count,
				archetype3->EntityCount,
				archetype3->ToString(),
				length3
			}));
			Block_17:
			int num8 = num2;
			unsafeCachedChunkList = cache;
			if (num8 != unsafeCachedChunkList.Length)
			{
				string text4 = "query chunk cache inconsistency: we checked {0} chunks from matching archetypes, but chunk cache length is {1}.";
				object obj4 = num2;
				unsafeCachedChunkList = cache;
				throw new ArgumentException(string.Format(text4, obj4, unsafeCachedChunkList.Length));
			}
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x0004282C File Offset: 0x00040A2C
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void Rebuild$BurstManaged(ref UnsafeCachedChunkList cache, in EntityQueryData queryData)
		{
			cache.MatchingChunks->Clear();
			cache.PerChunkMatchingArchetypeIndex->Clear();
			cache.ChunkIndexInArchetype->Clear();
			UnsafeMatchingArchetypePtrList unsafeMatchingArchetypePtrList = queryData.MatchingArchetypes;
			int length = unsafeMatchingArchetypePtrList.Length;
			unsafeMatchingArchetypePtrList = queryData.MatchingArchetypes;
			MatchingArchetype** ptr = unsafeMatchingArchetypePtrList.Ptr;
			for (int i = 0; i < length; i++)
			{
				Archetype* archetype = ((IntPtr*)(ptr + (IntPtr)i * (IntPtr)sizeof(MatchingArchetype*) / (IntPtr)sizeof(MatchingArchetype*)))->Archetype;
				if (archetype->EntityCount > 0)
				{
					archetype->Chunks.AddToCachedChunkList(ref cache, i, 0);
				}
			}
			cache.CacheValid = 1;
		}

		// Token: 0x040005D2 RID: 1490
		[NoAlias]
		[NativeDisableUnsafePtrRestriction]
		internal unsafe UnsafeList<ChunkIndex>* MatchingChunks;

		// Token: 0x040005D3 RID: 1491
		[NoAlias]
		[NativeDisableUnsafePtrRestriction]
		internal unsafe UnsafeList<int>* PerChunkMatchingArchetypeIndex;

		// Token: 0x040005D4 RID: 1492
		[NoAlias]
		[NativeDisableUnsafePtrRestriction]
		internal unsafe UnsafeList<int>* ChunkIndexInArchetype;

		// Token: 0x040005D5 RID: 1493
		[NoAlias]
		[NativeDisableUnsafePtrRestriction]
		internal unsafe EntityComponentStore* EntityComponentStore;

		// Token: 0x040005D6 RID: 1494
		internal int CacheValid;

		// Token: 0x0200022D RID: 557
		// (Invoke) Token: 0x06000DCC RID: 3532
		internal delegate void Rebuild_00000C31$PostfixBurstDelegate(ref UnsafeCachedChunkList cache, in EntityQueryData queryData);

		// Token: 0x0200022E RID: 558
		internal static class Rebuild_00000C31$BurstDirectCall
		{
			// Token: 0x06000DCF RID: 3535 RVA: 0x000428B7 File Offset: 0x00040AB7
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (UnsafeCachedChunkList.Rebuild_00000C31$BurstDirectCall.Pointer == 0)
				{
					UnsafeCachedChunkList.Rebuild_00000C31$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(UnsafeCachedChunkList.Rebuild_00000C31$BurstDirectCall.DeferredCompilation, methodof(UnsafeCachedChunkList.Rebuild$BurstManaged(ref UnsafeCachedChunkList, ref EntityQueryData)).MethodHandle, typeof(UnsafeCachedChunkList.Rebuild_00000C31$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = UnsafeCachedChunkList.Rebuild_00000C31$BurstDirectCall.Pointer;
			}

			// Token: 0x06000DD0 RID: 3536 RVA: 0x000428E4 File Offset: 0x00040AE4
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				UnsafeCachedChunkList.Rebuild_00000C31$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000DD1 RID: 3537 RVA: 0x000428FC File Offset: 0x00040AFC
			public static void Constructor()
			{
				UnsafeCachedChunkList.Rebuild_00000C31$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(UnsafeCachedChunkList.Rebuild(ref UnsafeCachedChunkList, ref EntityQueryData)).MethodHandle);
			}

			// Token: 0x06000DD2 RID: 3538 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06000DD3 RID: 3539 RVA: 0x0004290D File Offset: 0x00040B0D
			// Note: this type is marked as 'beforefieldinit'.
			static Rebuild_00000C31$BurstDirectCall()
			{
				UnsafeCachedChunkList.Rebuild_00000C31$BurstDirectCall.Constructor();
			}

			// Token: 0x06000DD4 RID: 3540 RVA: 0x00042914 File Offset: 0x00040B14
			public static void Invoke(ref UnsafeCachedChunkList cache, in EntityQueryData queryData)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = UnsafeCachedChunkList.Rebuild_00000C31$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.UnsafeCachedChunkList&,Unity.Entities.EntityQueryData&), ref cache, ref queryData, functionPointer);
						return;
					}
				}
				UnsafeCachedChunkList.Rebuild$BurstManaged(ref cache, in queryData);
			}

			// Token: 0x040005D7 RID: 1495
			private static IntPtr Pointer;

			// Token: 0x040005D8 RID: 1496
			private static IntPtr DeferredCompilation;
		}
	}
}
