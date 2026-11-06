using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace Unity.Entities
{
	// Token: 0x020001DF RID: 479
	[BurstCompile]
	[GenerateTestsForBurstCompatibility]
	internal struct ChunkIterationUtility
	{
		// Token: 0x06000B1B RID: 2843 RVA: 0x00035068 File Offset: 0x00033268
		[Obsolete("Remove with CreateArchetypeChunkArrayAsync. (RemovedAfter Entities 1.0)")]
		public unsafe static NativeArray<ArchetypeChunk> CreateArchetypeChunkArrayAsync(UnsafeMatchingArchetypePtrList matchingArchetypes, AllocatorManager.AllocatorHandle allocator, out JobHandle jobHandle, ref EntityQueryFilter filter, JobHandle dependsOn = default(JobHandle))
		{
			int length = matchingArchetypes.Length;
			NativeArray<int> nativeArray = new NativeArray<int>(length, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
			int num = 0;
			MatchingArchetype** ptr = matchingArchetypes.Ptr;
			for (int i = 0; i < length; i++)
			{
				Archetype* archetype = ((IntPtr*)(ptr + (IntPtr)i * (IntPtr)sizeof(MatchingArchetype*) / (IntPtr)sizeof(MatchingArchetype*)))->Archetype;
				nativeArray[i] = num;
				num += archetype->Chunks.Count;
			}
			if (!filter.RequiresMatchesFilter)
			{
				NativeArray<ArchetypeChunk> nativeArray2 = CollectionHelper.CreateNativeArray<ArchetypeChunk>(num, allocator, NativeArrayOptions.UninitializedMemory);
				GatherChunksToArrayJob gatherChunksToArrayJob = new GatherChunksToArrayJob
				{
					MatchingArchetypes = matchingArchetypes.Ptr,
					entityComponentStore = matchingArchetypes.entityComponentStore,
					Offsets = nativeArray,
					Chunks = nativeArray2
				};
				jobHandle = gatherChunksToArrayJob.Schedule(length, 1, dependsOn);
				return nativeArray2;
			}
			NativeArray<int> nativeArray3 = new NativeArray<int>(length + 1, Allocator.TempJob, NativeArrayOptions.ClearMemory);
			NativeArray<ArchetypeChunk> nativeArray4 = new NativeArray<ArchetypeChunk>(num, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
			new GatherChunksToArrayWithFilteringJob
			{
				MatchingArchetypes = matchingArchetypes.Ptr,
				Filter = filter,
				Offsets = nativeArray,
				FilteredCounts = nativeArray3,
				SparseChunks = nativeArray4,
				entityComponentStore = matchingArchetypes.entityComponentStore
			}.Schedule(length, 1, dependsOn).Complete();
			int num2 = 0;
			for (int j = 0; j < length; j++)
			{
				int num3 = nativeArray3[j];
				nativeArray3[j] = num2;
				num2 += num3;
			}
			nativeArray3[length] = num2;
			NativeArray<ArchetypeChunk> nativeArray5 = CollectionHelper.CreateNativeArray<ArchetypeChunk>(num2, allocator, NativeArrayOptions.UninitializedMemory);
			jobHandle = new JoinChunksJob
			{
				DestinationOffsets = nativeArray3,
				SparseChunks = nativeArray4,
				Offsets = nativeArray,
				JoinedChunks = nativeArray5
			}.Schedule(length, 1, default(JobHandle));
			return nativeArray5;
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x0003522C File Offset: 0x0003342C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static void ToChunkList<TGatherer>(in UnsafeCachedChunkList cachedChunkList, in UnsafeMatchingArchetypePtrList matchingArchetypes, ref EntityQueryFilter filter, int hasEnableableComponents, ref TGatherer outChunks) where TGatherer : struct, ChunkIterationUtility.IChunkGatherer
		{
			UnsafeCachedChunkList unsafeCachedChunkList = cachedChunkList;
			ChunkIndex* chunkIndices = unsafeCachedChunkList.ChunkIndices;
			UnsafeMatchingArchetypePtrList unsafeMatchingArchetypePtrList = matchingArchetypes;
			MatchingArchetype** ptr = unsafeMatchingArchetypePtrList.Ptr;
			bool requiresMatchesFilter = filter.RequiresMatchesFilter;
			bool flag = hasEnableableComponents != 0;
			int* ptr2 = cachedChunkList.PerChunkMatchingArchetypeIndex->Ptr;
			int* ptr3 = cachedChunkList.ChunkIndexInArchetype->Ptr;
			unsafeCachedChunkList = cachedChunkList;
			int length = unsafeCachedChunkList.Length;
			int num = -1;
			MatchingArchetype* ptr4 = null;
			ChunkIterationUtility.EnabledMaskMatchingArchetypeState enabledMaskMatchingArchetypeState = default(ChunkIterationUtility.EnabledMaskMatchingArchetypeState);
			int* ptr5 = null;
			if (!requiresMatchesFilter && !flag)
			{
				for (int i = 0; i < length; i++)
				{
					outChunks.AddNoResize(chunkIndices[i]);
				}
				return;
			}
			if (flag)
			{
				for (int j = 0; j < length; j++)
				{
					if (Hint.Unlikely(ptr2[j] != num))
					{
						num = ptr2[j];
						ptr4 = *(IntPtr*)(ptr + (IntPtr)num * (IntPtr)sizeof(MatchingArchetype*) / (IntPtr)sizeof(MatchingArchetype*));
						ptr5 = ptr4->Archetype->Chunks.GetChunkEntityCountArray();
						enabledMaskMatchingArchetypeState = new ChunkIterationUtility.EnabledMaskMatchingArchetypeState(ptr4);
					}
					int num2 = ptr3[j];
					if (!requiresMatchesFilter || ptr4->ChunkMatchesFilter(num2, ref filter))
					{
						int num3 = ptr5[num2];
						v128 v;
						ChunkIterationUtility.GetEnabledMask(num2, num3, in enabledMaskMatchingArchetypeState, out v);
						if (v.ULong0 != 0UL || v.ULong1 != 0UL)
						{
							outChunks.AddNoResize(chunkIndices[j]);
						}
					}
				}
				return;
			}
			for (int k = 0; k < length; k++)
			{
				if (Hint.Unlikely(ptr2[k] != num))
				{
					num = ptr2[k];
					ptr4 = *(IntPtr*)(ptr + (IntPtr)num * (IntPtr)sizeof(MatchingArchetype*) / (IntPtr)sizeof(MatchingArchetype*));
				}
				int num4 = ptr3[k];
				if (ptr4->ChunkMatchesFilter(num4, ref filter))
				{
					outChunks.AddNoResize(chunkIndices[k]);
				}
			}
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x0003541D File Offset: 0x0003361D
		[BurstCompile]
		public static void ToArchetypeChunkList(in UnsafeCachedChunkList cachedChunkList, in UnsafeMatchingArchetypePtrList matchingArchetypes, ref EntityQueryFilter filter, int hasEnableableComponents, ref NativeList<ArchetypeChunk> outChunks)
		{
			ChunkIterationUtility.ToArchetypeChunkList_00000A41$BurstDirectCall.Invoke(in cachedChunkList, in matchingArchetypes, ref filter, hasEnableableComponents, ref outChunks);
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x0003542A File Offset: 0x0003362A
		[BurstCompile]
		public static void ToChunkIndexList(in UnsafeCachedChunkList cachedChunkList, in UnsafeMatchingArchetypePtrList matchingArchetypes, ref EntityQueryFilter filter, int hasEnableableComponents, ref NativeList<ChunkIndex> outChunkIndices)
		{
			ChunkIterationUtility.ToChunkIndexList_00000A42$BurstDirectCall.Invoke(in cachedChunkList, in matchingArchetypes, ref filter, hasEnableableComponents, ref outChunkIndices);
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x00035437 File Offset: 0x00033637
		[BurstCompile]
		public unsafe static void GatherEntitiesWithFilter(Entity* entities, ref EntityQueryFilter filter, in UnsafeCachedChunkList cache, ref UnsafeMatchingArchetypePtrList matchingArchetypePtrList)
		{
			ChunkIterationUtility.GatherEntitiesWithFilter_00000A43$BurstDirectCall.Invoke(entities, ref filter, in cache, ref matchingArchetypePtrList);
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x00035442 File Offset: 0x00033642
		[BurstCompile]
		public unsafe static void GatherEntitiesWithoutFilter(Entity* entities, in UnsafeCachedChunkList cachedChunkList, in UnsafeMatchingArchetypePtrList matchingArchetypes)
		{
			ChunkIterationUtility.GatherEntitiesWithoutFilter_00000A44$BurstDirectCall.Invoke(entities, in cachedChunkList, in matchingArchetypes);
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x0003544C File Offset: 0x0003364C
		public unsafe static NativeArray<Entity> CreateEntityArray(UnsafeMatchingArchetypePtrList matchingArchetypes, AllocatorManager.AllocatorHandle allocator, EntityTypeHandle typeHandle, EntityQuery entityQuery, int entityCount)
		{
			UnsafeCachedChunkList matchingChunkCache = entityQuery.__impl->GetMatchingChunkCache();
			NativeArray<Entity> nativeArray = CollectionHelper.CreateNativeArray<Entity>(entityCount, allocator, NativeArrayOptions.UninitializedMemory);
			bool flag = entityQuery.HasFilter();
			bool flag2 = entityQuery.__impl->_QueryData->HasEnableableComponents > 0;
			if (!flag && !flag2)
			{
				ChunkIterationUtility.GatherEntitiesWithoutFilter((Entity*)nativeArray.GetUnsafePtr<Entity>(), in matchingChunkCache, in matchingArchetypes);
			}
			else
			{
				EntityQueryFilter filter = entityQuery.__impl->_Filter;
				ChunkIterationUtility.GatherEntitiesWithFilter((Entity*)nativeArray.GetUnsafePtr<Entity>(), ref filter, in matchingChunkCache, ref matchingArchetypes);
			}
			return nativeArray;
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x000354C0 File Offset: 0x000336C0
		[Obsolete("Use CreateEntityListAsync instead. (RemovedAfter Entities 1.0)")]
		public unsafe static NativeArray<Entity> CreateEntityArrayAsync(UnsafeMatchingArchetypePtrList matchingArchetypes, AllocatorManager.AllocatorHandle allocator, EntityTypeHandle typeHandle, EntityQuery entityQuery, int entityCount, out JobHandle outJobHandle, JobHandle dependsOn)
		{
			NativeArray<Entity> nativeArray = CollectionHelper.CreateNativeArray<Entity>(entityCount, allocator, NativeArrayOptions.UninitializedMemory);
			JobHandle jobHandle;
			NativeArray<int> nativeArray2 = entityQuery.CalculateBaseEntityIndexArrayAsync(Allocator.TempJob, dependsOn, out jobHandle);
			GatherEntitiesToArrayJob gatherEntitiesToArrayJob = new GatherEntitiesToArrayJob
			{
				ChunkBaseEntityIndices = nativeArray2,
				EntityTypeHandle = typeHandle,
				Entities = (byte*)nativeArray.GetUnsafePtr<Entity>()
			};
			outJobHandle = gatherEntitiesToArrayJob.ScheduleParallel(entityQuery, jobHandle);
			return nativeArray;
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x00035524 File Offset: 0x00033724
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe static NativeList<Entity> CreateEntityListAsync(AllocatorManager.AllocatorHandle allocator, EntityTypeHandle typeHandle, EntityQuery entityQuery, int maxEntityCount, JobHandle dependsOn, out JobHandle outJobHandle)
		{
			NativeList<Entity> nativeList = new NativeList<Entity>(maxEntityCount, allocator);
			JobHandle jobHandle;
			NativeArray<int> nativeArray = entityQuery.CalculateBaseEntityIndexArrayAsync(Allocator.TempJob, dependsOn, out jobHandle);
			GatherEntitiesJob gatherEntitiesJob = new GatherEntitiesJob
			{
				ChunkBaseEntityIndices = nativeArray,
				OutputList = new TypelessUnsafeList
				{
					Ptr = (byte*)nativeList.m_ListData->Ptr,
					Length = &nativeList.m_ListData->m_length,
					Capacity = nativeList.m_ListData->Capacity
				},
				EntityTypeHandle = typeHandle
			};
			outJobHandle = (ref gatherEntitiesJob).ScheduleParallelByRef<GatherEntitiesJob>(entityQuery, jobHandle);
			return nativeList;
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x000355C0 File Offset: 0x000337C0
		[BurstCompile]
		public unsafe static void GatherComponentDataWithFilter(byte* componentData, TypeIndex typeIndex, in UnsafeCachedChunkList cache, in UnsafeMatchingArchetypePtrList matchingArchetypePtrList, ref EntityQueryFilter filter)
		{
			ChunkIterationUtility.GatherComponentDataWithFilter_00000A48$BurstDirectCall.Invoke(componentData, typeIndex, in cache, in matchingArchetypePtrList, ref filter);
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x000355CD File Offset: 0x000337CD
		[BurstCompile]
		public unsafe static void GatherComponentDataWithoutFilter(byte* componentData, TypeIndex typeIndex, in UnsafeCachedChunkList cachedChunkList, in UnsafeMatchingArchetypePtrList matchingArchetypePtrList)
		{
			ChunkIterationUtility.GatherComponentDataWithoutFilter_00000A49$BurstDirectCall.Invoke(componentData, typeIndex, in cachedChunkList, in matchingArchetypePtrList);
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x000355D8 File Offset: 0x000337D8
		[Obsolete("Use CreateComponentDataListAsync instead. (RemovedAfter Entities 1.0)")]
		public unsafe static NativeArray<T> CreateComponentDataArrayAsync<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(AllocatorManager.AllocatorHandle allocator, ref ComponentTypeHandle<T> typeHandle, int entityCount, EntityQuery entityQuery, out JobHandle outJobHandle, JobHandle dependsOn) where T : struct, ValueType, IComponentData
		{
			NativeArray<T> nativeArray = CollectionHelper.CreateNativeArray<T>(entityCount, allocator, NativeArrayOptions.UninitializedMemory);
			JobHandle jobHandle;
			NativeArray<int> nativeArray2 = entityQuery.CalculateBaseEntityIndexArrayAsync(Allocator.TempJob, dependsOn, out jobHandle);
			GatherComponentDataToArrayJob gatherComponentDataToArrayJob = new GatherComponentDataToArrayJob
			{
				ComponentData = (byte*)nativeArray.GetUnsafePtr<T>(),
				TypeIndex = typeHandle.m_TypeIndex,
				ChunkBaseEntityIndices = nativeArray2
			};
			outJobHandle = gatherComponentDataToArrayJob.ScheduleParallel(entityQuery, jobHandle);
			return nativeArray;
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x00035640 File Offset: 0x00033840
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe static NativeList<T> CreateComponentDataListAsync<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(AllocatorManager.AllocatorHandle allocator, ref DynamicComponentTypeHandle typeHandle, int maxEntityCount, EntityQuery entityQuery, JobHandle dependsOn, out JobHandle outJobHandle) where T : struct, ValueType, IComponentData
		{
			NativeList<T> nativeList = new NativeList<T>(maxEntityCount, allocator);
			JobHandle jobHandle;
			NativeArray<int> nativeArray = entityQuery.CalculateBaseEntityIndexArrayAsync(Allocator.TempJob, dependsOn, out jobHandle);
			GatherComponentDataJob gatherComponentDataJob = new GatherComponentDataJob
			{
				OutputList = new TypelessUnsafeList
				{
					Ptr = (byte*)nativeList.m_ListData->Ptr,
					Length = &nativeList.m_ListData->m_length,
					Capacity = nativeList.m_ListData->Capacity
				},
				TypeHandle = typeHandle,
				ChunkBaseEntityIndices = nativeArray
			};
			outJobHandle = (ref gatherComponentDataJob).ScheduleParallelByRef<GatherComponentDataJob>(entityQuery, jobHandle);
			return nativeList;
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x000356E4 File Offset: 0x000338E4
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe static NativeArray<T> CreateComponentDataArray<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(AllocatorManager.AllocatorHandle allocator, ref ComponentTypeHandle<T> typeHandle, int entityCount, EntityQuery entityQuery) where T : struct, ValueType, IComponentData
		{
			UnsafeCachedChunkList matchingChunkCache = entityQuery.__impl->GetMatchingChunkCache();
			UnsafeMatchingArchetypePtrList matchingArchetypes = entityQuery.__impl->_QueryData->MatchingArchetypes;
			NativeArray<T> nativeArray = CollectionHelper.CreateNativeArray<T>(entityCount, allocator, NativeArrayOptions.UninitializedMemory);
			bool flag = entityQuery.HasFilter();
			bool flag2 = entityQuery.__impl->_QueryData->HasEnableableComponents > 0;
			if (!flag && !flag2)
			{
				ChunkIterationUtility.GatherComponentDataWithoutFilter((byte*)nativeArray.GetUnsafePtr<T>(), typeHandle.m_TypeIndex, in matchingChunkCache, in matchingArchetypes);
			}
			else
			{
				EntityQueryFilter filter = entityQuery.__impl->_Filter;
				ChunkIterationUtility.GatherComponentDataWithFilter((byte*)nativeArray.GetUnsafePtr<T>(), typeHandle.m_TypeIndex, in matchingChunkCache, in matchingArchetypes, ref filter);
			}
			return nativeArray;
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x00035773 File Offset: 0x00033973
		[BurstCompile]
		private unsafe static void CopyComponentArrayToChunksWithFilter(byte* componentData, TypeIndex typeIndex, ref UnsafeMatchingArchetypePtrList matchingArchetypePtrList, ref EntityQueryFilter filter, in UnsafeCachedChunkList cache, uint globalSystemVersion)
		{
			ChunkIterationUtility.CopyComponentArrayToChunksWithFilter_00000A4D$BurstDirectCall.Invoke(componentData, typeIndex, ref matchingArchetypePtrList, ref filter, in cache, globalSystemVersion);
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x00035782 File Offset: 0x00033982
		[BurstCompile]
		public unsafe static void CopyComponentArrayToChunksWithoutFilter(byte* componentData, TypeIndex typeIndex, in UnsafeCachedChunkList cachedChunkList, in UnsafeMatchingArchetypePtrList matchingArchetypePtrList, uint globalSystemVersion)
		{
			ChunkIterationUtility.CopyComponentArrayToChunksWithoutFilter_00000A4E$BurstDirectCall.Invoke(componentData, typeIndex, in cachedChunkList, in matchingArchetypePtrList, globalSystemVersion);
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x00035790 File Offset: 0x00033990
		[Obsolete("Use CopyFromComponentDataListAsync instead. (RemovedAfter Entities 1.0)")]
		public unsafe static void CopyFromComponentDataArrayAsync<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(UnsafeMatchingArchetypePtrList matchingArchetypes, NativeArray<T> componentDataArray, ref ComponentTypeHandle<T> typeHandle, EntityQuery entityQuery, ref EntityQueryFilter filter, out JobHandle jobHandle, JobHandle dependsOn) where T : struct, ValueType, IComponentData
		{
			JobHandle jobHandle2;
			NativeArray<int> nativeArray = entityQuery.CalculateBaseEntityIndexArrayAsync(Allocator.TempJob, dependsOn, out jobHandle2);
			CopyComponentArrayToChunksJob copyComponentArrayToChunksJob = new CopyComponentArrayToChunksJob
			{
				ComponentData = (byte*)componentDataArray.GetUnsafePtr<T>(),
				TypeIndex = typeHandle.m_TypeIndex,
				GlobalSystemVersion = typeHandle.GlobalSystemVersion,
				ChunkBaseEntityIndices = nativeArray
			};
			jobHandle = copyComponentArrayToChunksJob.ScheduleParallel(entityQuery, jobHandle2);
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x000357F8 File Offset: 0x000339F8
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe static void CopyFromComponentDataListAsync<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(NativeList<T> componentDataList, ref DynamicComponentTypeHandle typeHandle, EntityQuery entityQuery, JobHandle dependsOn, out JobHandle outJobHandle) where T : struct, ValueType, IComponentData
		{
			JobHandle jobHandle;
			NativeArray<int> nativeArray = entityQuery.CalculateBaseEntityIndexArrayAsync(Allocator.TempJob, dependsOn, out jobHandle);
			CopyComponentListToChunksJob copyComponentListToChunksJob = new CopyComponentListToChunksJob
			{
				InputList = new TypelessUnsafeList
				{
					Ptr = (byte*)componentDataList.m_ListData->Ptr,
					Length = &componentDataList.m_ListData->m_length,
					Capacity = componentDataList.m_ListData->Capacity
				},
				TypeHandle = typeHandle,
				ChunkBaseEntityIndices = nativeArray
			};
			outJobHandle = (ref copyComponentListToChunksJob).ScheduleParallelByRef<CopyComponentListToChunksJob>(entityQuery, jobHandle);
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x00035890 File Offset: 0x00033A90
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe static void CopyFromComponentDataArray<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(NativeArray<T> componentDataArray, ref ComponentTypeHandle<T> typeHandle, EntityQuery entityQuery) where T : struct, ValueType, IComponentData
		{
			UnsafeMatchingArchetypePtrList matchingArchetypes = entityQuery.__impl->_QueryData->MatchingArchetypes;
			UnsafeCachedChunkList matchingChunkCache = entityQuery.__impl->GetMatchingChunkCache();
			bool flag = entityQuery.HasFilter();
			bool flag2 = entityQuery.__impl->_QueryData->HasEnableableComponents > 0;
			if (!flag && !flag2)
			{
				ChunkIterationUtility.CopyComponentArrayToChunksWithoutFilter((byte*)componentDataArray.GetUnsafePtr<T>(), typeHandle.m_TypeIndex, in matchingChunkCache, in matchingArchetypes, typeHandle.GlobalSystemVersion);
				return;
			}
			EntityQueryFilter filter = entityQuery.__impl->_Filter;
			ChunkIterationUtility.CopyComponentArrayToChunksWithFilter((byte*)componentDataArray.GetUnsafePtr<T>(), typeHandle.m_TypeIndex, ref matchingArchetypes, ref filter, in matchingChunkCache, typeHandle.GlobalSystemVersion);
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x0003591F File Offset: 0x00033B1F
		[BurstCompile]
		public static int CalculateChunkCount(in UnsafeCachedChunkList cachedChunkList, ref UnsafeMatchingArchetypePtrList matchingArchetypes, ref EntityQueryFilter filter, int queryHasEnableableComponents)
		{
			return ChunkIterationUtility.CalculateChunkCount_00000A52$BurstDirectCall.Invoke(in cachedChunkList, ref matchingArchetypes, ref filter, queryHasEnableableComponents);
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x0003592A File Offset: 0x00033B2A
		[BurstCompile]
		public static bool IsEmpty(ref EntityQueryImpl queryImpl, in EntityQueryFilter filter)
		{
			return ChunkIterationUtility.IsEmpty_00000A53$BurstDirectCall.Invoke(ref queryImpl, in filter);
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x00035933 File Offset: 0x00033B33
		[BurstCompile]
		public static void CalculateFilteredChunkIndexArray(in UnsafeCachedChunkList cachedChunkList, in UnsafeMatchingArchetypePtrList matchingArchetypes, ref EntityQueryFilter filter, int hasEnableableComponents, ref NativeArray<int> outFilteredChunkIndices)
		{
			ChunkIterationUtility.CalculateFilteredChunkIndexArray_00000A54$BurstDirectCall.Invoke(in cachedChunkList, in matchingArchetypes, ref filter, hasEnableableComponents, ref outFilteredChunkIndices);
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x00035940 File Offset: 0x00033B40
		[BurstCompile]
		public static void CalculateBaseEntityIndexArray(in UnsafeCachedChunkList cachedChunkList, in UnsafeMatchingArchetypePtrList matchingArchetypes, ref EntityQueryFilter filter, int hasEnableableComponents, ref NativeArray<int> outChunkBaseEntityIndices)
		{
			ChunkIterationUtility.CalculateBaseEntityIndexArray_00000A55$BurstDirectCall.Invoke(in cachedChunkList, in matchingArchetypes, ref filter, hasEnableableComponents, ref outChunkBaseEntityIndices);
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x0003594D File Offset: 0x00033B4D
		[BurstCompile]
		public static int CalculateEntityCount(in UnsafeCachedChunkList cachedChunkList, ref UnsafeMatchingArchetypePtrList matchingArchetypes, ref EntityQueryFilter filter, int hasEnableableComponents)
		{
			return ChunkIterationUtility.CalculateEntityCount_00000A56$BurstDirectCall.Invoke(in cachedChunkList, ref matchingArchetypes, ref filter, hasEnableableComponents);
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x00035958 File Offset: 0x00033B58
		[BurstCompile]
		public unsafe static int CalculateEntityCountAndSingleton(in UnsafeCachedChunkList cachedChunkList, ref UnsafeMatchingArchetypePtrList matchingArchetypes, ref EntityQueryFilter filter, int hasEnableableComponents, out MatchingArchetype* firstMatchArchetype, out ChunkIndex firstMatchChunk, out int firstMatchEntityIndexInChunk)
		{
			return ChunkIterationUtility.CalculateEntityCountAndSingleton_00000A57$BurstDirectCall.Invoke(in cachedChunkList, ref matchingArchetypes, ref filter, hasEnableableComponents, out firstMatchArchetype, out firstMatchChunk, out firstMatchEntityIndexInChunk);
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x0003596C File Offset: 0x00033B6C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleBufferElement) })]
		internal unsafe static BufferAccessor<T> GetChunkBufferAccessor<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Archetype* archetype, ChunkIndex chunk, bool isWriting, int typeIndexInArchetype, uint systemVersion) where T : struct, ValueType, IBufferElementData
		{
			int num = archetype->BufferCapacities[typeIndexInArchetype];
			byte* ptr = ((!isWriting) ? ChunkDataUtility.GetComponentDataRO(chunk, archetype, 0, typeIndexInArchetype) : ChunkDataUtility.GetComponentDataRW(chunk, archetype, 0, typeIndexInArchetype, systemVersion));
			int count = chunk.Count;
			int num2 = (int)archetype->SizeOfs[typeIndexInArchetype];
			return new BufferAccessor<T>(ptr, count, num2, num);
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x000359BC File Offset: 0x00033BBC
		internal unsafe static void* GetChunkComponentDataROPtr(Archetype* archetype, ChunkIndex chunk, int indexInArchetype)
		{
			return (void*)(chunk.Buffer + archetype->Offsets[indexInArchetype]);
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x000359D4 File Offset: 0x00033BD4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void GetEnabledMask(int chunkIndexInArchetype, int chunkEntityCount, in ChunkIterationUtility.EnabledMaskMatchingArchetypeState archetypeState, out v128 enabledMask)
		{
			MatchingArchetype* matchingArchetype = archetypeState.MatchingArchetype;
			int enableableComponentsCount_All = matchingArchetype->EnableableComponentsCount_All;
			int enableableComponentsCount_None = matchingArchetype->EnableableComponentsCount_None;
			int enableableComponentsCount_Any = matchingArchetype->EnableableComponentsCount_Any;
			int enableableComponentsCount_Disabled = matchingArchetype->EnableableComponentsCount_Disabled;
			int* matchingArchetypeAllComponentMemoryOrderIndices = archetypeState.MatchingArchetypeAllComponentMemoryOrderIndices;
			int* matchingArchetypeNoneComponentMemoryOrderIndices = archetypeState.MatchingArchetypeNoneComponentMemoryOrderIndices;
			int* matchingArchetypeAnyComponentMemoryOrderIndices = archetypeState.MatchingArchetypeAnyComponentMemoryOrderIndices;
			int* matchingArchetypeDisabledComponentMemoryOrderIndices = archetypeState.MatchingArchetypeDisabledComponentMemoryOrderIndices;
			v128* ptr = archetypeState.ArchetypeEnabledBits + archetypeState.ArchetypeEnabledBitsPerChunkOffset * chunkIndexInArchetype;
			enabledMask = new v128(ulong.MaxValue);
			if (Hint.Unlikely(enableableComponentsCount_All == 0))
			{
				v128 v = new v128(ulong.MaxValue);
				enabledMask = EnabledBitUtility.ShiftRight(in v, 128 - chunkEntityCount);
			}
			if (X86.Sse2.IsSse2Supported)
			{
				for (int i = 0; i < enableableComponentsCount_All; i++)
				{
					int num = matchingArchetypeAllComponentMemoryOrderIndices[i];
					enabledMask = X86.Sse2.and_si128(ptr[num], enabledMask);
				}
				if (enableableComponentsCount_Any > 0)
				{
					v128 v2 = ptr[*matchingArchetypeAnyComponentMemoryOrderIndices];
					for (int j = 1; j < enableableComponentsCount_Any; j++)
					{
						int num2 = matchingArchetypeAnyComponentMemoryOrderIndices[j];
						v2 = X86.Sse2.or_si128(ptr[num2], v2);
					}
					enabledMask = X86.Sse2.and_si128(v2, enabledMask);
				}
				for (int k = 0; k < enableableComponentsCount_None; k++)
				{
					int num3 = matchingArchetypeNoneComponentMemoryOrderIndices[k];
					enabledMask = X86.Sse2.andnot_si128(ptr[num3], enabledMask);
				}
				for (int l = 0; l < enableableComponentsCount_Disabled; l++)
				{
					int num4 = matchingArchetypeDisabledComponentMemoryOrderIndices[l];
					enabledMask = X86.Sse2.andnot_si128(ptr[num4], enabledMask);
				}
				return;
			}
			if (Arm.Neon.IsNeonSupported)
			{
				for (int m = 0; m < enableableComponentsCount_All; m++)
				{
					int num5 = matchingArchetypeAllComponentMemoryOrderIndices[m];
					enabledMask = Arm.Neon.vandq_u64(ptr[num5], enabledMask);
				}
				if (enableableComponentsCount_Any > 0)
				{
					v128 v3 = ptr[*matchingArchetypeAnyComponentMemoryOrderIndices];
					for (int n = 1; n < enableableComponentsCount_Any; n++)
					{
						int num6 = matchingArchetypeAnyComponentMemoryOrderIndices[n];
						v3 = Arm.Neon.vorrq_u64(ptr[num6], v3);
					}
					enabledMask = Arm.Neon.vandq_u64(v3, enabledMask);
				}
				for (int num7 = 0; num7 < enableableComponentsCount_None; num7++)
				{
					int num8 = matchingArchetypeNoneComponentMemoryOrderIndices[num7];
					enabledMask = Arm.Neon.vandq_u64(Arm.Neon.vmvnq_u32(ptr[num8]), enabledMask);
				}
				for (int num9 = 0; num9 < enableableComponentsCount_Disabled; num9++)
				{
					int num10 = matchingArchetypeDisabledComponentMemoryOrderIndices[num9];
					enabledMask = Arm.Neon.vandq_u64(Arm.Neon.vmvnq_u32(ptr[num10]), enabledMask);
				}
				return;
			}
			for (int num11 = 0; num11 < enableableComponentsCount_All; num11++)
			{
				int num12 = matchingArchetypeAllComponentMemoryOrderIndices[num11];
				enabledMask.ULong0 = ptr[num12].ULong0 & enabledMask.ULong0;
				enabledMask.ULong1 = ptr[num12].ULong1 & enabledMask.ULong1;
			}
			if (enableableComponentsCount_Any > 0)
			{
				v128 v4 = new v128(0);
				for (int num13 = 0; num13 < enableableComponentsCount_Any; num13++)
				{
					int num14 = matchingArchetypeAnyComponentMemoryOrderIndices[num13];
					v4.ULong0 = ptr[num14].ULong0 | v4.ULong0;
					v4.ULong1 = ptr[num14].ULong1 | v4.ULong1;
				}
				enabledMask.ULong0 = v4.ULong0 & enabledMask.ULong0;
				enabledMask.ULong1 = v4.ULong1 & enabledMask.ULong1;
			}
			for (int num15 = 0; num15 < enableableComponentsCount_None; num15++)
			{
				int num16 = matchingArchetypeNoneComponentMemoryOrderIndices[num15];
				enabledMask.ULong0 = ~ptr[num16].ULong0 & enabledMask.ULong0;
				enabledMask.ULong1 = ~ptr[num16].ULong1 & enabledMask.ULong1;
			}
			for (int num17 = 0; num17 < enableableComponentsCount_Disabled; num17++)
			{
				int num18 = matchingArchetypeDisabledComponentMemoryOrderIndices[num17];
				enabledMask.ULong0 = ~ptr[num18].ULong0 & enabledMask.ULong0;
				enabledMask.ULong1 = ~ptr[num18].ULong1 & enabledMask.ULong1;
			}
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x00035E6B File Offset: 0x0003406B
		[BurstCompile]
		public unsafe static void GetEnabledMask(ChunkIndex chunk, MatchingArchetype* matchingArchetype, out v128 enabledMask)
		{
			ChunkIterationUtility.GetEnabledMask_00000A5B$BurstDirectCall.Invoke(chunk, matchingArchetype, out enabledMask);
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x00035E78 File Offset: 0x00034078
		internal unsafe static void GetEnabledMaskNoBurstForTests(ChunkIndex chunk, MatchingArchetype* matchingArchetype, out v128 enabledMask)
		{
			int listIndex = chunk.ListIndex;
			int count = chunk.Count;
			ChunkIterationUtility.EnabledMaskMatchingArchetypeState enabledMaskMatchingArchetypeState = new ChunkIterationUtility.EnabledMaskMatchingArchetypeState(matchingArchetype);
			ChunkIterationUtility.GetEnabledMask(listIndex, count, in enabledMaskMatchingArchetypeState, out enabledMask);
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x00035EA2 File Offset: 0x000340A2
		[BurstCompile]
		public static void SetEnabledBitsOnAllChunks(ref EntityQueryImpl queryImpl, TypeIndex typeIndex, bool value, uint globalSystemVersion)
		{
			ChunkIterationUtility.SetEnabledBitsOnAllChunks_00000A5D$BurstDirectCall.Invoke(ref queryImpl, typeIndex, value, globalSystemVersion);
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x00035EB0 File Offset: 0x000340B0
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void ToArchetypeChunkList$BurstManaged(in UnsafeCachedChunkList cachedChunkList, in UnsafeMatchingArchetypePtrList matchingArchetypes, ref EntityQueryFilter filter, int hasEnableableComponents, ref NativeList<ArchetypeChunk> outChunks)
		{
			ChunkIterationUtility.ArchetypeChunkListGatherer archetypeChunkListGatherer = new ChunkIterationUtility.ArchetypeChunkListGatherer
			{
				ArchetypeChunks = outChunks,
				Ecs = cachedChunkList.EntityComponentStore
			};
			ChunkIterationUtility.ToChunkList<ChunkIterationUtility.ArchetypeChunkListGatherer>(in cachedChunkList, in matchingArchetypes, ref filter, hasEnableableComponents, ref archetypeChunkListGatherer);
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x00035EF0 File Offset: 0x000340F0
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void ToChunkIndexList$BurstManaged(in UnsafeCachedChunkList cachedChunkList, in UnsafeMatchingArchetypePtrList matchingArchetypes, ref EntityQueryFilter filter, int hasEnableableComponents, ref NativeList<ChunkIndex> outChunkIndices)
		{
			ChunkIterationUtility.ChunkIndexListGatherer chunkIndexListGatherer = new ChunkIterationUtility.ChunkIndexListGatherer
			{
				ChunkIndices = outChunkIndices
			};
			ChunkIterationUtility.ToChunkList<ChunkIterationUtility.ChunkIndexListGatherer>(in cachedChunkList, in matchingArchetypes, ref filter, hasEnableableComponents, ref chunkIndexListGatherer);
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x00035F20 File Offset: 0x00034120
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void GatherEntitiesWithFilter$BurstManaged(Entity* entities, ref EntityQueryFilter filter, in UnsafeCachedChunkList cache, ref UnsafeMatchingArchetypePtrList matchingArchetypePtrList)
		{
			Entity* ptr = entities;
			UnsafeChunkCacheIterator unsafeChunkCacheIterator = new UnsafeChunkCacheIterator(in filter, true, cache, matchingArchetypePtrList.Ptr);
			int num = -1;
			v128 v = default(v128);
			ArchetypeChunk archetypeChunk;
			int num2;
			byte b;
			while (unsafeChunkCacheIterator.MoveNextChunk(ref num, out archetypeChunk, out num2, out b, ref v))
			{
				Entity* buffer = (Entity*)archetypeChunk.m_Chunk.Buffer;
				if (b == 0)
				{
					UnsafeUtility.MemCpy((void*)ptr, (void*)buffer, (long)(num2 * sizeof(Entity)));
					ptr += num2;
				}
				else
				{
					int num3 = 0;
					int num4;
					while (EnabledBitUtility.TryGetNextRange(v, num3, out num4, out num3))
					{
						int num5 = num3 - num4;
						UnsafeUtility.MemCpy((void*)ptr, (void*)(buffer + num4), (long)(num5 * sizeof(Entity)));
						ptr += num5;
					}
				}
			}
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x00035FD8 File Offset: 0x000341D8
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void GatherEntitiesWithoutFilter$BurstManaged(Entity* entities, in UnsafeCachedChunkList cachedChunkList, in UnsafeMatchingArchetypePtrList matchingArchetypes)
		{
			EntityQueryFilter entityQueryFilter = default(EntityQueryFilter);
			bool flag = false;
			UnsafeCachedChunkList unsafeCachedChunkList = cachedChunkList;
			UnsafeMatchingArchetypePtrList unsafeMatchingArchetypePtrList = matchingArchetypes;
			UnsafeChunkCacheIterator unsafeChunkCacheIterator = new UnsafeChunkCacheIterator(in entityQueryFilter, flag, unsafeCachedChunkList, unsafeMatchingArchetypePtrList.Ptr);
			int num = -1;
			v128 v = default(v128);
			Entity* ptr = entities;
			ArchetypeChunk archetypeChunk;
			int num2;
			byte b;
			while (unsafeChunkCacheIterator.MoveNextChunk(ref num, out archetypeChunk, out num2, out b, ref v))
			{
				Entity* buffer = (Entity*)archetypeChunk.m_Chunk.Buffer;
				UnsafeUtility.MemCpy((void*)ptr, (void*)buffer, (long)(num2 * sizeof(Entity)));
				ptr += num2;
			}
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x00036058 File Offset: 0x00034258
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void GatherComponentDataWithFilter$BurstManaged(byte* componentData, TypeIndex typeIndex, in UnsafeCachedChunkList cache, in UnsafeMatchingArchetypePtrList matchingArchetypePtrList, ref EntityQueryFilter filter)
		{
			byte* ptr = componentData;
			bool flag = true;
			UnsafeCachedChunkList unsafeCachedChunkList = cache;
			UnsafeMatchingArchetypePtrList unsafeMatchingArchetypePtrList = matchingArchetypePtrList;
			UnsafeChunkCacheIterator unsafeChunkCacheIterator = new UnsafeChunkCacheIterator(in filter, flag, unsafeCachedChunkList, unsafeMatchingArchetypePtrList.Ptr);
			int num = -1;
			v128 v = default(v128);
			LookupCache lookupCache = default(LookupCache);
			ArchetypeChunk archetypeChunk;
			int num2;
			byte b;
			while (unsafeChunkCacheIterator.MoveNextChunk(ref num, out archetypeChunk, out num2, out b, ref v))
			{
				Archetype* archetype = unsafeChunkCacheIterator._CurrentMatchingArchetype->Archetype;
				if (archetype != lookupCache.Archetype)
				{
					lookupCache.Update(archetype, typeIndex);
				}
				byte* ptr2 = archetypeChunk.m_Chunk.Buffer + lookupCache.ComponentOffset;
				if (b == 0)
				{
					UnsafeUtility.MemCpy((void*)ptr, (void*)ptr2, (long)(num2 * (int)lookupCache.ComponentSizeOf));
					ptr += num2 * (int)lookupCache.ComponentSizeOf;
				}
				else
				{
					int num3 = 0;
					int num4;
					while (EnabledBitUtility.TryGetNextRange(v, num3, out num4, out num3))
					{
						int num5 = num3 - num4;
						UnsafeUtility.MemCpy((void*)ptr, (void*)(ptr2 + num4 * (int)lookupCache.ComponentSizeOf), (long)(num5 * (int)lookupCache.ComponentSizeOf));
						ptr += num5 * (int)lookupCache.ComponentSizeOf;
					}
				}
			}
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x00036154 File Offset: 0x00034354
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void GatherComponentDataWithoutFilter$BurstManaged(byte* componentData, TypeIndex typeIndex, in UnsafeCachedChunkList cachedChunkList, in UnsafeMatchingArchetypePtrList matchingArchetypePtrList)
		{
			EntityQueryFilter entityQueryFilter = default(EntityQueryFilter);
			bool flag = false;
			UnsafeCachedChunkList unsafeCachedChunkList = cachedChunkList;
			UnsafeMatchingArchetypePtrList unsafeMatchingArchetypePtrList = matchingArchetypePtrList;
			UnsafeChunkCacheIterator unsafeChunkCacheIterator = new UnsafeChunkCacheIterator(in entityQueryFilter, flag, unsafeCachedChunkList, unsafeMatchingArchetypePtrList.Ptr);
			int num = -1;
			v128 v = default(v128);
			LookupCache lookupCache = default(LookupCache);
			byte* ptr = componentData;
			ArchetypeChunk archetypeChunk;
			int num2;
			byte b;
			while (unsafeChunkCacheIterator.MoveNextChunk(ref num, out archetypeChunk, out num2, out b, ref v))
			{
				Archetype* archetype = unsafeChunkCacheIterator._CurrentMatchingArchetype->Archetype;
				if (archetype != lookupCache.Archetype)
				{
					lookupCache.Update(archetype, typeIndex);
				}
				byte* ptr2 = archetypeChunk.m_Chunk.Buffer + lookupCache.ComponentOffset;
				int num3 = (int)lookupCache.ComponentSizeOf * num2;
				UnsafeUtility.MemCpy((void*)ptr, (void*)ptr2, (long)num3);
				ptr += num3;
			}
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x00036204 File Offset: 0x00034404
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void CopyComponentArrayToChunksWithFilter$BurstManaged(byte* componentData, TypeIndex typeIndex, ref UnsafeMatchingArchetypePtrList matchingArchetypePtrList, ref EntityQueryFilter filter, in UnsafeCachedChunkList cache, uint globalSystemVersion)
		{
			byte* ptr = componentData;
			UnsafeChunkCacheIterator unsafeChunkCacheIterator = new UnsafeChunkCacheIterator(in filter, true, cache, matchingArchetypePtrList.Ptr);
			int num = -1;
			v128 v = default(v128);
			LookupCache lookupCache = default(LookupCache);
			ArchetypeChunk archetypeChunk;
			int num2;
			byte b;
			while (unsafeChunkCacheIterator.MoveNextChunk(ref num, out archetypeChunk, out num2, out b, ref v))
			{
				Archetype* archetype = unsafeChunkCacheIterator._CurrentMatchingArchetype->Archetype;
				byte* componentDataWithTypeRW = ChunkDataUtility.GetComponentDataWithTypeRW(archetypeChunk.m_Chunk, archetype, 0, typeIndex, globalSystemVersion, ref lookupCache);
				if (b == 0)
				{
					UnsafeUtility.MemCpy((void*)componentDataWithTypeRW, (void*)ptr, (long)(num2 * (int)lookupCache.ComponentSizeOf));
					ptr += num2 * (int)lookupCache.ComponentSizeOf;
				}
				else
				{
					int num3 = 0;
					int num4;
					while (EnabledBitUtility.TryGetNextRange(v, num3, out num4, out num3))
					{
						int num5 = num3 - num4;
						UnsafeUtility.MemCpy((void*)(componentDataWithTypeRW + num4 * (int)lookupCache.ComponentSizeOf), (void*)ptr, (long)(num5 * (int)lookupCache.ComponentSizeOf));
						ptr += num5 * (int)lookupCache.ComponentSizeOf;
					}
				}
			}
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x000362E0 File Offset: 0x000344E0
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void CopyComponentArrayToChunksWithoutFilter$BurstManaged(byte* componentData, TypeIndex typeIndex, in UnsafeCachedChunkList cachedChunkList, in UnsafeMatchingArchetypePtrList matchingArchetypePtrList, uint globalSystemVersion)
		{
			EntityQueryFilter entityQueryFilter = default(EntityQueryFilter);
			bool flag = false;
			UnsafeCachedChunkList unsafeCachedChunkList = cachedChunkList;
			UnsafeMatchingArchetypePtrList unsafeMatchingArchetypePtrList = matchingArchetypePtrList;
			UnsafeChunkCacheIterator unsafeChunkCacheIterator = new UnsafeChunkCacheIterator(in entityQueryFilter, flag, unsafeCachedChunkList, unsafeMatchingArchetypePtrList.Ptr);
			int num = -1;
			v128 v = default(v128);
			LookupCache lookupCache = default(LookupCache);
			byte* ptr = componentData;
			ArchetypeChunk archetypeChunk;
			int num2;
			byte b;
			while (unsafeChunkCacheIterator.MoveNextChunk(ref num, out archetypeChunk, out num2, out b, ref v))
			{
				Archetype* archetype = unsafeChunkCacheIterator._CurrentMatchingArchetype->Archetype;
				void* componentDataWithTypeRW = (void*)ChunkDataUtility.GetComponentDataWithTypeRW(archetypeChunk.m_Chunk, archetype, 0, typeIndex, globalSystemVersion, ref lookupCache);
				int num3 = (int)lookupCache.ComponentSizeOf * num2;
				UnsafeUtility.MemCpy(componentDataWithTypeRW, (void*)ptr, (long)num3);
				ptr += num3;
			}
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x0003637C File Offset: 0x0003457C
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static int CalculateChunkCount$BurstManaged(in UnsafeCachedChunkList cachedChunkList, ref UnsafeMatchingArchetypePtrList matchingArchetypes, ref EntityQueryFilter filter, int queryHasEnableableComponents)
		{
			int num = 0;
			MatchingArchetype** ptr = matchingArchetypes.Ptr;
			bool requiresMatchesFilter = filter.RequiresMatchesFilter;
			bool flag = queryHasEnableableComponents == 1;
			int* ptr2 = cachedChunkList.PerChunkMatchingArchetypeIndex->Ptr;
			int* ptr3 = cachedChunkList.ChunkIndexInArchetype->Ptr;
			UnsafeCachedChunkList unsafeCachedChunkList = cachedChunkList;
			int length = unsafeCachedChunkList.Length;
			int num2 = -1;
			MatchingArchetype* ptr4 = null;
			ChunkIterationUtility.EnabledMaskMatchingArchetypeState enabledMaskMatchingArchetypeState = default(ChunkIterationUtility.EnabledMaskMatchingArchetypeState);
			int* ptr5 = null;
			if (!requiresMatchesFilter && !flag)
			{
				unsafeCachedChunkList = cachedChunkList;
				num = unsafeCachedChunkList.Length;
			}
			else if (flag)
			{
				for (int i = 0; i < length; i++)
				{
					if (Hint.Unlikely(ptr2[i] != num2))
					{
						num2 = ptr2[i];
						ptr4 = *(IntPtr*)(ptr + (IntPtr)num2 * (IntPtr)sizeof(MatchingArchetype*) / (IntPtr)sizeof(MatchingArchetype*));
						ptr5 = ptr4->Archetype->Chunks.GetChunkEntityCountArray();
						enabledMaskMatchingArchetypeState = new ChunkIterationUtility.EnabledMaskMatchingArchetypeState(ptr4);
					}
					int num3 = ptr3[i];
					if (!requiresMatchesFilter || ptr4->ChunkMatchesFilter(num3, ref filter))
					{
						int num4 = ptr5[num3];
						v128 v;
						ChunkIterationUtility.GetEnabledMask(num3, num4, in enabledMaskMatchingArchetypeState, out v);
						if (v.ULong0 != 0UL || v.ULong1 != 0UL)
						{
							num++;
						}
					}
				}
			}
			else
			{
				for (int j = 0; j < length; j++)
				{
					if (Hint.Unlikely(ptr2[j] != num2))
					{
						num2 = ptr2[j];
						ptr4 = *(IntPtr*)(ptr + (IntPtr)num2 * (IntPtr)sizeof(MatchingArchetype*) / (IntPtr)sizeof(MatchingArchetype*));
						ptr5 = ptr4->Archetype->Chunks.GetChunkEntityCountArray();
					}
					int num5 = ptr3[j];
					if (!requiresMatchesFilter || ptr4->ChunkMatchesFilter(num5, ref filter))
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x00036520 File Offset: 0x00034720
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static bool IsEmpty$BurstManaged(ref EntityQueryImpl queryImpl, in EntityQueryFilter filter)
		{
			UnsafeChunkCacheIterator unsafeChunkCacheIterator = new UnsafeChunkCacheIterator(in filter, queryImpl._QueryData->HasEnableableComponents > 0, queryImpl.GetMatchingChunkCache(), queryImpl._QueryData->MatchingArchetypes.Ptr);
			int num = -1;
			v128 v = default(v128);
			ArchetypeChunk archetypeChunk;
			int num2;
			byte b;
			return !unsafeChunkCacheIterator.MoveNextChunk(ref num, out archetypeChunk, out num2, out b, ref v);
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x00036578 File Offset: 0x00034778
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void CalculateFilteredChunkIndexArray$BurstManaged(in UnsafeCachedChunkList cachedChunkList, in UnsafeMatchingArchetypePtrList matchingArchetypes, ref EntityQueryFilter filter, int hasEnableableComponents, ref NativeArray<int> outFilteredChunkIndices)
		{
			int num = 0;
			UnsafeMatchingArchetypePtrList unsafeMatchingArchetypePtrList = matchingArchetypes;
			MatchingArchetype** ptr = unsafeMatchingArchetypePtrList.Ptr;
			bool requiresMatchesFilter = filter.RequiresMatchesFilter;
			bool flag = hasEnableableComponents == 1;
			int* ptr2 = cachedChunkList.PerChunkMatchingArchetypeIndex->Ptr;
			int* ptr3 = cachedChunkList.ChunkIndexInArchetype->Ptr;
			UnsafeCachedChunkList unsafeCachedChunkList = cachedChunkList;
			int length = unsafeCachedChunkList.Length;
			int num2 = -1;
			MatchingArchetype* ptr4 = null;
			ChunkIterationUtility.EnabledMaskMatchingArchetypeState enabledMaskMatchingArchetypeState = default(ChunkIterationUtility.EnabledMaskMatchingArchetypeState);
			int* ptr5 = null;
			if (!requiresMatchesFilter && !flag)
			{
				for (int i = 0; i < length; i++)
				{
					outFilteredChunkIndices[i] = i;
				}
				return;
			}
			if (flag)
			{
				for (int j = 0; j < length; j++)
				{
					outFilteredChunkIndices[j] = -1;
					if (Hint.Unlikely(ptr2[j] != num2))
					{
						num2 = ptr2[j];
						ptr4 = *(IntPtr*)(ptr + (IntPtr)num2 * (IntPtr)sizeof(MatchingArchetype*) / (IntPtr)sizeof(MatchingArchetype*));
						ptr5 = ptr4->Archetype->Chunks.GetChunkEntityCountArray();
						enabledMaskMatchingArchetypeState = new ChunkIterationUtility.EnabledMaskMatchingArchetypeState(ptr4);
					}
					int num3 = ptr3[j];
					if (!requiresMatchesFilter || ptr4->ChunkMatchesFilter(num3, ref filter))
					{
						int num4 = ptr5[num3];
						v128 v;
						ChunkIterationUtility.GetEnabledMask(num3, num4, in enabledMaskMatchingArchetypeState, out v);
						if (v.ULong0 != 0UL || v.ULong1 != 0UL)
						{
							outFilteredChunkIndices[j] = num++;
						}
					}
				}
				return;
			}
			for (int k = 0; k < length; k++)
			{
				outFilteredChunkIndices[k] = -1;
				if (Hint.Unlikely(ptr2[k] != num2))
				{
					num2 = ptr2[k];
					ptr4 = *(IntPtr*)(ptr + (IntPtr)num2 * (IntPtr)sizeof(MatchingArchetype*) / (IntPtr)sizeof(MatchingArchetype*));
				}
				int num5 = ptr3[k];
				if (ptr4->ChunkMatchesFilter(num5, ref filter))
				{
					outFilteredChunkIndices[k] = num++;
				}
			}
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x0003673C File Offset: 0x0003493C
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void CalculateBaseEntityIndexArray$BurstManaged(in UnsafeCachedChunkList cachedChunkList, in UnsafeMatchingArchetypePtrList matchingArchetypes, ref EntityQueryFilter filter, int hasEnableableComponents, ref NativeArray<int> outChunkBaseEntityIndices)
		{
			int num = 0;
			UnsafeMatchingArchetypePtrList unsafeMatchingArchetypePtrList = matchingArchetypes;
			MatchingArchetype** ptr = unsafeMatchingArchetypePtrList.Ptr;
			bool requiresMatchesFilter = filter.RequiresMatchesFilter;
			bool flag = hasEnableableComponents == 1;
			int* ptr2 = cachedChunkList.PerChunkMatchingArchetypeIndex->Ptr;
			int* ptr3 = cachedChunkList.ChunkIndexInArchetype->Ptr;
			UnsafeCachedChunkList unsafeCachedChunkList = cachedChunkList;
			int length = unsafeCachedChunkList.Length;
			int num2 = -1;
			MatchingArchetype* ptr4 = null;
			ChunkIterationUtility.EnabledMaskMatchingArchetypeState enabledMaskMatchingArchetypeState = default(ChunkIterationUtility.EnabledMaskMatchingArchetypeState);
			int* ptr5 = null;
			if (!requiresMatchesFilter && !flag)
			{
				for (int i = 0; i < length; i++)
				{
					outChunkBaseEntityIndices[i] = num;
					if (Hint.Unlikely(ptr2[i] != num2))
					{
						num2 = ptr2[i];
						ptr5 = ((IntPtr*)(ptr + (IntPtr)num2 * (IntPtr)sizeof(MatchingArchetype*) / (IntPtr)sizeof(MatchingArchetype*)))->Archetype->Chunks.GetChunkEntityCountArray();
					}
					int num3 = ptr3[i];
					num += ptr5[num3];
				}
				return;
			}
			if (flag)
			{
				for (int j = 0; j < length; j++)
				{
					outChunkBaseEntityIndices[j] = num;
					if (Hint.Unlikely(ptr2[j] != num2))
					{
						num2 = ptr2[j];
						ptr4 = *(IntPtr*)(ptr + (IntPtr)num2 * (IntPtr)sizeof(MatchingArchetype*) / (IntPtr)sizeof(MatchingArchetype*));
						ptr5 = ptr4->Archetype->Chunks.GetChunkEntityCountArray();
						enabledMaskMatchingArchetypeState = new ChunkIterationUtility.EnabledMaskMatchingArchetypeState(ptr4);
					}
					int num4 = ptr3[j];
					if (!requiresMatchesFilter || ptr4->ChunkMatchesFilter(num4, ref filter))
					{
						int num5 = ptr5[num4];
						v128 v;
						ChunkIterationUtility.GetEnabledMask(num4, num5, in enabledMaskMatchingArchetypeState, out v);
						num += EnabledBitUtility.countbits(v);
					}
				}
				return;
			}
			for (int k = 0; k < length; k++)
			{
				outChunkBaseEntityIndices[k] = num;
				if (Hint.Unlikely(ptr2[k] != num2))
				{
					num2 = ptr2[k];
					ptr4 = *(IntPtr*)(ptr + (IntPtr)num2 * (IntPtr)sizeof(MatchingArchetype*) / (IntPtr)sizeof(MatchingArchetype*));
					ptr5 = ptr4->Archetype->Chunks.GetChunkEntityCountArray();
				}
				int num6 = ptr3[k];
				if (ptr4->ChunkMatchesFilter(num6, ref filter))
				{
					int num7 = ptr5[num6];
					num += num7;
				}
			}
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x00036958 File Offset: 0x00034B58
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static int CalculateEntityCount$BurstManaged(in UnsafeCachedChunkList cachedChunkList, ref UnsafeMatchingArchetypePtrList matchingArchetypes, ref EntityQueryFilter filter, int hasEnableableComponents)
		{
			int num = 0;
			bool requiresMatchesFilter = filter.RequiresMatchesFilter;
			bool flag = hasEnableableComponents == 1;
			MatchingArchetype** ptr = matchingArchetypes.Ptr;
			if (!requiresMatchesFilter && !flag)
			{
				int length = matchingArchetypes.Length;
				for (int i = 0; i < length; i++)
				{
					num += ((IntPtr*)(ptr + (IntPtr)i * (IntPtr)sizeof(MatchingArchetype*) / (IntPtr)sizeof(MatchingArchetype*)))->Archetype->EntityCount;
				}
				return num;
			}
			int* ptr2 = cachedChunkList.PerChunkMatchingArchetypeIndex->Ptr;
			int* ptr3 = cachedChunkList.ChunkIndexInArchetype->Ptr;
			UnsafeCachedChunkList unsafeCachedChunkList = cachedChunkList;
			int length2 = unsafeCachedChunkList.Length;
			int num2 = -1;
			MatchingArchetype* ptr4 = null;
			ChunkIterationUtility.EnabledMaskMatchingArchetypeState enabledMaskMatchingArchetypeState = default(ChunkIterationUtility.EnabledMaskMatchingArchetypeState);
			int* ptr5 = null;
			if (flag)
			{
				for (int j = 0; j < length2; j++)
				{
					if (Hint.Unlikely(ptr2[j] != num2))
					{
						num2 = ptr2[j];
						ptr4 = *(IntPtr*)(ptr + (IntPtr)num2 * (IntPtr)sizeof(MatchingArchetype*) / (IntPtr)sizeof(MatchingArchetype*));
						ptr5 = ptr4->Archetype->Chunks.GetChunkEntityCountArray();
						enabledMaskMatchingArchetypeState = new ChunkIterationUtility.EnabledMaskMatchingArchetypeState(ptr4);
					}
					int num3 = ptr3[j];
					if (!requiresMatchesFilter || ptr4->ChunkMatchesFilter(num3, ref filter))
					{
						int num4 = ptr5[num3];
						v128 v;
						ChunkIterationUtility.GetEnabledMask(num3, num4, in enabledMaskMatchingArchetypeState, out v);
						num += EnabledBitUtility.countbits(v);
					}
				}
			}
			else
			{
				for (int k = 0; k < length2; k++)
				{
					if (Hint.Unlikely(ptr2[k] != num2))
					{
						num2 = ptr2[k];
						ptr4 = *(IntPtr*)(ptr + (IntPtr)num2 * (IntPtr)sizeof(MatchingArchetype*) / (IntPtr)sizeof(MatchingArchetype*));
						ptr5 = ptr4->Archetype->Chunks.GetChunkEntityCountArray();
					}
					int num5 = ptr3[k];
					if (!requiresMatchesFilter || ptr4->ChunkMatchesFilter(num5, ref filter))
					{
						int num6 = ptr5[num5];
						num += num6;
					}
				}
			}
			return num;
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x00036B1C File Offset: 0x00034D1C
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static int CalculateEntityCountAndSingleton$BurstManaged(in UnsafeCachedChunkList cachedChunkList, ref UnsafeMatchingArchetypePtrList matchingArchetypes, ref EntityQueryFilter filter, int hasEnableableComponents, out MatchingArchetype* firstMatchArchetype, out ChunkIndex firstMatchChunk, out int firstMatchEntityIndexInChunk)
		{
			firstMatchArchetype = (IntPtr)((UIntPtr)0);
			firstMatchChunk = ChunkIndex.Null;
			firstMatchEntityIndexInChunk = -1;
			int num = 0;
			bool requiresMatchesFilter = filter.RequiresMatchesFilter;
			bool flag = hasEnableableComponents == 1;
			MatchingArchetype** ptr = matchingArchetypes.Ptr;
			if (!requiresMatchesFilter && !flag)
			{
				int length = matchingArchetypes.Length;
				for (int i = 0; i < length; i++)
				{
					int entityCount = ((IntPtr*)(ptr + (IntPtr)i * (IntPtr)sizeof(MatchingArchetype*) / (IntPtr)sizeof(MatchingArchetype*)))->Archetype->EntityCount;
					num += entityCount;
					if (Hint.Unlikely(firstMatchArchetype == (IntPtr)((UIntPtr)0) && entityCount > 0))
					{
						firstMatchArchetype = *(IntPtr*)(ptr + (IntPtr)i * (IntPtr)sizeof(MatchingArchetype*) / (IntPtr)sizeof(MatchingArchetype*));
						firstMatchChunk = firstMatchArchetype.Archetype->Chunks[0];
						firstMatchEntityIndexInChunk = 0;
					}
				}
				return num;
			}
			int* ptr2 = cachedChunkList.PerChunkMatchingArchetypeIndex->Ptr;
			int* ptr3 = cachedChunkList.ChunkIndexInArchetype->Ptr;
			UnsafeCachedChunkList unsafeCachedChunkList = cachedChunkList;
			int length2 = unsafeCachedChunkList.Length;
			int num2 = -1;
			MatchingArchetype* ptr4 = null;
			ChunkIterationUtility.EnabledMaskMatchingArchetypeState enabledMaskMatchingArchetypeState = default(ChunkIterationUtility.EnabledMaskMatchingArchetypeState);
			int* ptr5 = null;
			if (flag)
			{
				for (int j = 0; j < length2; j++)
				{
					if (Hint.Unlikely(ptr2[j] != num2))
					{
						num2 = ptr2[j];
						ptr4 = *(IntPtr*)(ptr + (IntPtr)num2 * (IntPtr)sizeof(MatchingArchetype*) / (IntPtr)sizeof(MatchingArchetype*));
						ptr5 = ptr4->Archetype->Chunks.GetChunkEntityCountArray();
						enabledMaskMatchingArchetypeState = new ChunkIterationUtility.EnabledMaskMatchingArchetypeState(ptr4);
					}
					int num3 = ptr3[j];
					if (!requiresMatchesFilter || ptr4->ChunkMatchesFilter(num3, ref filter))
					{
						int num4 = ptr5[num3];
						v128 v;
						ChunkIterationUtility.GetEnabledMask(num3, num4, in enabledMaskMatchingArchetypeState, out v);
						int num5 = EnabledBitUtility.countbits(v);
						num += num5;
						if (Hint.Unlikely(firstMatchArchetype == (IntPtr)((UIntPtr)0) && num5 > 0))
						{
							firstMatchArchetype = ptr4;
							firstMatchChunk = firstMatchArchetype.Archetype->Chunks[num3];
							firstMatchEntityIndexInChunk = EnabledBitUtility.tzcnt_u128(v);
						}
					}
				}
			}
			else
			{
				for (int k = 0; k < length2; k++)
				{
					if (Hint.Unlikely(ptr2[k] != num2))
					{
						num2 = ptr2[k];
						ptr4 = *(IntPtr*)(ptr + (IntPtr)num2 * (IntPtr)sizeof(MatchingArchetype*) / (IntPtr)sizeof(MatchingArchetype*));
						ptr5 = ptr4->Archetype->Chunks.GetChunkEntityCountArray();
					}
					int num6 = ptr3[k];
					if (!requiresMatchesFilter || ptr4->ChunkMatchesFilter(num6, ref filter))
					{
						int num7 = ptr5[num6];
						num += num7;
						if (Hint.Unlikely(firstMatchArchetype == (IntPtr)((UIntPtr)0) && num7 > 0))
						{
							firstMatchArchetype = ptr4;
							firstMatchChunk = firstMatchArchetype.Archetype->Chunks[num6];
							firstMatchEntityIndexInChunk = 0;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x00036DC8 File Offset: 0x00034FC8
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void GetEnabledMask$BurstManaged(ChunkIndex chunk, MatchingArchetype* matchingArchetype, out v128 enabledMask)
		{
			int listIndex = chunk.ListIndex;
			int count = chunk.Count;
			ChunkIterationUtility.EnabledMaskMatchingArchetypeState enabledMaskMatchingArchetypeState = new ChunkIterationUtility.EnabledMaskMatchingArchetypeState(matchingArchetype);
			ChunkIterationUtility.GetEnabledMask(listIndex, count, in enabledMaskMatchingArchetypeState, out enabledMask);
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x00036DF4 File Offset: 0x00034FF4
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void SetEnabledBitsOnAllChunks$BurstManaged(ref EntityQueryImpl queryImpl, TypeIndex typeIndex, bool value, uint globalSystemVersion)
		{
			UnsafeCachedChunkList matchingChunkCache = queryImpl.GetMatchingChunkCache();
			int length = matchingChunkCache.Length;
			ChunkIndex* chunkIndices = matchingChunkCache.ChunkIndices;
			int* ptr = matchingChunkCache.ChunkIndexInArchetype->Ptr;
			MatchingArchetype** ptr2 = queryImpl._QueryData->MatchingArchetypes.Ptr;
			int* ptr3 = matchingChunkCache.PerChunkMatchingArchetypeIndex->Ptr;
			bool requiresMatchesFilter = queryImpl._Filter.RequiresMatchesFilter;
			int num = -1;
			MatchingArchetype* ptr4 = null;
			v128* ptr5 = null;
			int* ptr6 = null;
			int num2 = 0;
			int* ptr7 = null;
			uint* ptr8 = null;
			for (int i = 0; i < length; i++)
			{
				ChunkIndex chunkIndex = chunkIndices[i];
				if (Hint.Unlikely(ptr3[i] != num))
				{
					num = ptr3[i];
					ptr4 = *(IntPtr*)(ptr2 + (IntPtr)num * (IntPtr)sizeof(MatchingArchetype*) / (IntPtr)sizeof(MatchingArchetype*));
					Archetype* archetype = ptr4->Archetype;
					ptr7 = archetype->Chunks.GetChunkEntityCountArray();
					int indexInTypeArray = ChunkDataUtility.GetIndexInTypeArray(archetype, typeIndex);
					int num3 = archetype->TypeIndexInArchetypeToMemoryOrderIndex[indexInTypeArray];
					ptr5 = archetype->Chunks.GetComponentEnabledMaskArrayForTypeInChunk(num3, 0);
					ptr6 = archetype->Chunks.GetPointerToChunkDisabledCountForType(num3, 0);
					num2 = archetype->Chunks.GetComponentEnabledBitsSizePerChunk() / sizeof(v128);
					ptr8 = archetype->Chunks.GetChangeVersionArrayForType(indexInTypeArray);
				}
				if (!requiresMatchesFilter || chunkIndex.MatchesFilter(ptr4, ref queryImpl._Filter))
				{
					int num4 = ptr[i];
					int num5 = ptr7[num4];
					ptr8[num4] = globalSystemVersion;
					if (value)
					{
						if (num5 < 64)
						{
							ptr5[num2 * num4] = new v128((1UL << num5) - 1UL, 0UL);
						}
						else if (num5 < 128)
						{
							ptr5[num2 * num4] = new v128(ulong.MaxValue, (1UL << num5 - 64) - 1UL);
						}
						else
						{
							ptr5[num2 * num4] = new v128(ulong.MaxValue, ulong.MaxValue);
						}
						ptr6[num2 * num4] = 0;
					}
					else
					{
						ptr5[num2 * num4] = default(v128);
						ptr6[num2 * num4] = num5;
					}
				}
			}
		}

		// Token: 0x020001E0 RID: 480
		private interface IChunkGatherer
		{
			// Token: 0x06000B4A RID: 2890
			void AddNoResize(ChunkIndex chunkIndex);
		}

		// Token: 0x020001E1 RID: 481
		private struct ArchetypeChunkListGatherer : ChunkIterationUtility.IChunkGatherer
		{
			// Token: 0x06000B4B RID: 2891 RVA: 0x00037028 File Offset: 0x00035228
			public void AddNoResize(ChunkIndex chunkIndex)
			{
				this.ArchetypeChunks.AddNoResize(new ArchetypeChunk(chunkIndex, this.Ecs));
			}

			// Token: 0x0400052A RID: 1322
			public NativeList<ArchetypeChunk> ArchetypeChunks;

			// Token: 0x0400052B RID: 1323
			public unsafe EntityComponentStore* Ecs;
		}

		// Token: 0x020001E2 RID: 482
		private struct ChunkIndexListGatherer : ChunkIterationUtility.IChunkGatherer
		{
			// Token: 0x06000B4C RID: 2892 RVA: 0x00037041 File Offset: 0x00035241
			public void AddNoResize(ChunkIndex chunkIndex)
			{
				this.ChunkIndices.AddNoResize(chunkIndex);
			}

			// Token: 0x0400052C RID: 1324
			public NativeList<ChunkIndex> ChunkIndices;
		}

		// Token: 0x020001E3 RID: 483
		[NoAlias]
		internal struct EnabledMaskMatchingArchetypeState
		{
			// Token: 0x06000B4D RID: 2893 RVA: 0x00037050 File Offset: 0x00035250
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe EnabledMaskMatchingArchetypeState(MatchingArchetype* matchingArchetype)
			{
				ArchetypeChunkData chunks = matchingArchetype->Archetype->Chunks;
				this.MatchingArchetype = matchingArchetype;
				this.MatchingArchetypeAllComponentMemoryOrderIndices = matchingArchetype->EnableableTypeMemoryOrderInArchetype_All;
				this.MatchingArchetypeNoneComponentMemoryOrderIndices = matchingArchetype->EnableableTypeMemoryOrderInArchetype_None;
				this.MatchingArchetypeAnyComponentMemoryOrderIndices = matchingArchetype->EnableableTypeMemoryOrderInArchetype_Any;
				this.MatchingArchetypeDisabledComponentMemoryOrderIndices = matchingArchetype->EnableableTypeMemoryOrderInArchetype_Disabled;
				this.ArchetypeEnabledBits = chunks.GetComponentEnabledMaskArrayForChunk(0);
				this.ArchetypeEnabledBitsPerChunkOffset = chunks.GetComponentEnabledBitsSizePerChunk() / sizeof(v128);
			}

			// Token: 0x0400052D RID: 1325
			[NoAlias]
			public unsafe MatchingArchetype* MatchingArchetype;

			// Token: 0x0400052E RID: 1326
			[NoAlias]
			public unsafe int* MatchingArchetypeAllComponentMemoryOrderIndices;

			// Token: 0x0400052F RID: 1327
			[NoAlias]
			public unsafe int* MatchingArchetypeAnyComponentMemoryOrderIndices;

			// Token: 0x04000530 RID: 1328
			[NoAlias]
			public unsafe int* MatchingArchetypeNoneComponentMemoryOrderIndices;

			// Token: 0x04000531 RID: 1329
			[NoAlias]
			public unsafe int* MatchingArchetypeDisabledComponentMemoryOrderIndices;

			// Token: 0x04000532 RID: 1330
			[NoAlias]
			public unsafe v128* ArchetypeEnabledBits;

			// Token: 0x04000533 RID: 1331
			public int ArchetypeEnabledBitsPerChunkOffset;
		}

		// Token: 0x020001E4 RID: 484
		// (Invoke) Token: 0x06000B4F RID: 2895
		internal delegate void ToArchetypeChunkList_00000A41$PostfixBurstDelegate(in UnsafeCachedChunkList cachedChunkList, in UnsafeMatchingArchetypePtrList matchingArchetypes, ref EntityQueryFilter filter, int hasEnableableComponents, ref NativeList<ArchetypeChunk> outChunks);

		// Token: 0x020001E5 RID: 485
		internal static class ToArchetypeChunkList_00000A41$BurstDirectCall
		{
			// Token: 0x06000B52 RID: 2898 RVA: 0x000370C2 File Offset: 0x000352C2
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (ChunkIterationUtility.ToArchetypeChunkList_00000A41$BurstDirectCall.Pointer == 0)
				{
					ChunkIterationUtility.ToArchetypeChunkList_00000A41$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(ChunkIterationUtility.ToArchetypeChunkList_00000A41$BurstDirectCall.DeferredCompilation, methodof(ChunkIterationUtility.ToArchetypeChunkList$BurstManaged(ref UnsafeCachedChunkList, ref UnsafeMatchingArchetypePtrList, ref EntityQueryFilter, int, ref NativeList<ArchetypeChunk>)).MethodHandle, typeof(ChunkIterationUtility.ToArchetypeChunkList_00000A41$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = ChunkIterationUtility.ToArchetypeChunkList_00000A41$BurstDirectCall.Pointer;
			}

			// Token: 0x06000B53 RID: 2899 RVA: 0x000370F0 File Offset: 0x000352F0
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				ChunkIterationUtility.ToArchetypeChunkList_00000A41$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000B54 RID: 2900 RVA: 0x00037108 File Offset: 0x00035308
			public static void Constructor()
			{
				ChunkIterationUtility.ToArchetypeChunkList_00000A41$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(ChunkIterationUtility.ToArchetypeChunkList(ref UnsafeCachedChunkList, ref UnsafeMatchingArchetypePtrList, ref EntityQueryFilter, int, ref NativeList<ArchetypeChunk>)).MethodHandle);
			}

			// Token: 0x06000B55 RID: 2901 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06000B56 RID: 2902 RVA: 0x00037119 File Offset: 0x00035319
			// Note: this type is marked as 'beforefieldinit'.
			static ToArchetypeChunkList_00000A41$BurstDirectCall()
			{
				ChunkIterationUtility.ToArchetypeChunkList_00000A41$BurstDirectCall.Constructor();
			}

			// Token: 0x06000B57 RID: 2903 RVA: 0x00037120 File Offset: 0x00035320
			public static void Invoke(in UnsafeCachedChunkList cachedChunkList, in UnsafeMatchingArchetypePtrList matchingArchetypes, ref EntityQueryFilter filter, int hasEnableableComponents, ref NativeList<ArchetypeChunk> outChunks)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = ChunkIterationUtility.ToArchetypeChunkList_00000A41$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.UnsafeCachedChunkList&,Unity.Entities.UnsafeMatchingArchetypePtrList&,Unity.Entities.EntityQueryFilter&,System.Int32,Unity.Collections.NativeList`1<Unity.Entities.ArchetypeChunk>&), ref cachedChunkList, ref matchingArchetypes, ref filter, hasEnableableComponents, ref outChunks, functionPointer);
						return;
					}
				}
				ChunkIterationUtility.ToArchetypeChunkList$BurstManaged(in cachedChunkList, in matchingArchetypes, ref filter, hasEnableableComponents, ref outChunks);
			}

			// Token: 0x04000534 RID: 1332
			private static IntPtr Pointer;

			// Token: 0x04000535 RID: 1333
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020001E6 RID: 486
		// (Invoke) Token: 0x06000B59 RID: 2905
		internal delegate void ToChunkIndexList_00000A42$PostfixBurstDelegate(in UnsafeCachedChunkList cachedChunkList, in UnsafeMatchingArchetypePtrList matchingArchetypes, ref EntityQueryFilter filter, int hasEnableableComponents, ref NativeList<ChunkIndex> outChunkIndices);

		// Token: 0x020001E7 RID: 487
		internal static class ToChunkIndexList_00000A42$BurstDirectCall
		{
			// Token: 0x06000B5C RID: 2908 RVA: 0x0003715B File Offset: 0x0003535B
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (ChunkIterationUtility.ToChunkIndexList_00000A42$BurstDirectCall.Pointer == 0)
				{
					ChunkIterationUtility.ToChunkIndexList_00000A42$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(ChunkIterationUtility.ToChunkIndexList_00000A42$BurstDirectCall.DeferredCompilation, methodof(ChunkIterationUtility.ToChunkIndexList$BurstManaged(ref UnsafeCachedChunkList, ref UnsafeMatchingArchetypePtrList, ref EntityQueryFilter, int, ref NativeList<ChunkIndex>)).MethodHandle, typeof(ChunkIterationUtility.ToChunkIndexList_00000A42$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = ChunkIterationUtility.ToChunkIndexList_00000A42$BurstDirectCall.Pointer;
			}

			// Token: 0x06000B5D RID: 2909 RVA: 0x00037188 File Offset: 0x00035388
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				ChunkIterationUtility.ToChunkIndexList_00000A42$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000B5E RID: 2910 RVA: 0x000371A0 File Offset: 0x000353A0
			public static void Constructor()
			{
				ChunkIterationUtility.ToChunkIndexList_00000A42$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(ChunkIterationUtility.ToChunkIndexList(ref UnsafeCachedChunkList, ref UnsafeMatchingArchetypePtrList, ref EntityQueryFilter, int, ref NativeList<ChunkIndex>)).MethodHandle);
			}

			// Token: 0x06000B5F RID: 2911 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06000B60 RID: 2912 RVA: 0x000371B1 File Offset: 0x000353B1
			// Note: this type is marked as 'beforefieldinit'.
			static ToChunkIndexList_00000A42$BurstDirectCall()
			{
				ChunkIterationUtility.ToChunkIndexList_00000A42$BurstDirectCall.Constructor();
			}

			// Token: 0x06000B61 RID: 2913 RVA: 0x000371B8 File Offset: 0x000353B8
			public static void Invoke(in UnsafeCachedChunkList cachedChunkList, in UnsafeMatchingArchetypePtrList matchingArchetypes, ref EntityQueryFilter filter, int hasEnableableComponents, ref NativeList<ChunkIndex> outChunkIndices)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = ChunkIterationUtility.ToChunkIndexList_00000A42$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.UnsafeCachedChunkList&,Unity.Entities.UnsafeMatchingArchetypePtrList&,Unity.Entities.EntityQueryFilter&,System.Int32,Unity.Collections.NativeList`1<Unity.Entities.ChunkIndex>&), ref cachedChunkList, ref matchingArchetypes, ref filter, hasEnableableComponents, ref outChunkIndices, functionPointer);
						return;
					}
				}
				ChunkIterationUtility.ToChunkIndexList$BurstManaged(in cachedChunkList, in matchingArchetypes, ref filter, hasEnableableComponents, ref outChunkIndices);
			}

			// Token: 0x04000536 RID: 1334
			private static IntPtr Pointer;

			// Token: 0x04000537 RID: 1335
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020001E8 RID: 488
		// (Invoke) Token: 0x06000B63 RID: 2915
		internal unsafe delegate void GatherEntitiesWithFilter_00000A43$PostfixBurstDelegate(Entity* entities, ref EntityQueryFilter filter, in UnsafeCachedChunkList cache, ref UnsafeMatchingArchetypePtrList matchingArchetypePtrList);

		// Token: 0x020001E9 RID: 489
		internal static class GatherEntitiesWithFilter_00000A43$BurstDirectCall
		{
			// Token: 0x06000B66 RID: 2918 RVA: 0x000371F3 File Offset: 0x000353F3
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (ChunkIterationUtility.GatherEntitiesWithFilter_00000A43$BurstDirectCall.Pointer == 0)
				{
					ChunkIterationUtility.GatherEntitiesWithFilter_00000A43$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(ChunkIterationUtility.GatherEntitiesWithFilter_00000A43$BurstDirectCall.DeferredCompilation, methodof(ChunkIterationUtility.GatherEntitiesWithFilter$BurstManaged(Entity*, ref EntityQueryFilter, ref UnsafeCachedChunkList, ref UnsafeMatchingArchetypePtrList)).MethodHandle, typeof(ChunkIterationUtility.GatherEntitiesWithFilter_00000A43$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = ChunkIterationUtility.GatherEntitiesWithFilter_00000A43$BurstDirectCall.Pointer;
			}

			// Token: 0x06000B67 RID: 2919 RVA: 0x00037220 File Offset: 0x00035420
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				ChunkIterationUtility.GatherEntitiesWithFilter_00000A43$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000B68 RID: 2920 RVA: 0x00037238 File Offset: 0x00035438
			public unsafe static void Constructor()
			{
				ChunkIterationUtility.GatherEntitiesWithFilter_00000A43$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(ChunkIterationUtility.GatherEntitiesWithFilter(Entity*, ref EntityQueryFilter, ref UnsafeCachedChunkList, ref UnsafeMatchingArchetypePtrList)).MethodHandle);
			}

			// Token: 0x06000B69 RID: 2921 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06000B6A RID: 2922 RVA: 0x00037249 File Offset: 0x00035449
			// Note: this type is marked as 'beforefieldinit'.
			static GatherEntitiesWithFilter_00000A43$BurstDirectCall()
			{
				ChunkIterationUtility.GatherEntitiesWithFilter_00000A43$BurstDirectCall.Constructor();
			}

			// Token: 0x06000B6B RID: 2923 RVA: 0x00037250 File Offset: 0x00035450
			public unsafe static void Invoke(Entity* entities, ref EntityQueryFilter filter, in UnsafeCachedChunkList cache, ref UnsafeMatchingArchetypePtrList matchingArchetypePtrList)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = ChunkIterationUtility.GatherEntitiesWithFilter_00000A43$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.Entity*,Unity.Entities.EntityQueryFilter&,Unity.Entities.UnsafeCachedChunkList&,Unity.Entities.UnsafeMatchingArchetypePtrList&), entities, ref filter, ref cache, ref matchingArchetypePtrList, functionPointer);
						return;
					}
				}
				ChunkIterationUtility.GatherEntitiesWithFilter$BurstManaged(entities, ref filter, in cache, ref matchingArchetypePtrList);
			}

			// Token: 0x04000538 RID: 1336
			private static IntPtr Pointer;

			// Token: 0x04000539 RID: 1337
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020001EA RID: 490
		// (Invoke) Token: 0x06000B6D RID: 2925
		internal unsafe delegate void GatherEntitiesWithoutFilter_00000A44$PostfixBurstDelegate(Entity* entities, in UnsafeCachedChunkList cachedChunkList, in UnsafeMatchingArchetypePtrList matchingArchetypes);

		// Token: 0x020001EB RID: 491
		internal static class GatherEntitiesWithoutFilter_00000A44$BurstDirectCall
		{
			// Token: 0x06000B70 RID: 2928 RVA: 0x00037287 File Offset: 0x00035487
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (ChunkIterationUtility.GatherEntitiesWithoutFilter_00000A44$BurstDirectCall.Pointer == 0)
				{
					ChunkIterationUtility.GatherEntitiesWithoutFilter_00000A44$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(ChunkIterationUtility.GatherEntitiesWithoutFilter_00000A44$BurstDirectCall.DeferredCompilation, methodof(ChunkIterationUtility.GatherEntitiesWithoutFilter$BurstManaged(Entity*, ref UnsafeCachedChunkList, ref UnsafeMatchingArchetypePtrList)).MethodHandle, typeof(ChunkIterationUtility.GatherEntitiesWithoutFilter_00000A44$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = ChunkIterationUtility.GatherEntitiesWithoutFilter_00000A44$BurstDirectCall.Pointer;
			}

			// Token: 0x06000B71 RID: 2929 RVA: 0x000372B4 File Offset: 0x000354B4
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				ChunkIterationUtility.GatherEntitiesWithoutFilter_00000A44$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000B72 RID: 2930 RVA: 0x000372CC File Offset: 0x000354CC
			public unsafe static void Constructor()
			{
				ChunkIterationUtility.GatherEntitiesWithoutFilter_00000A44$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(ChunkIterationUtility.GatherEntitiesWithoutFilter(Entity*, ref UnsafeCachedChunkList, ref UnsafeMatchingArchetypePtrList)).MethodHandle);
			}

			// Token: 0x06000B73 RID: 2931 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06000B74 RID: 2932 RVA: 0x000372DD File Offset: 0x000354DD
			// Note: this type is marked as 'beforefieldinit'.
			static GatherEntitiesWithoutFilter_00000A44$BurstDirectCall()
			{
				ChunkIterationUtility.GatherEntitiesWithoutFilter_00000A44$BurstDirectCall.Constructor();
			}

			// Token: 0x06000B75 RID: 2933 RVA: 0x000372E4 File Offset: 0x000354E4
			public unsafe static void Invoke(Entity* entities, in UnsafeCachedChunkList cachedChunkList, in UnsafeMatchingArchetypePtrList matchingArchetypes)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = ChunkIterationUtility.GatherEntitiesWithoutFilter_00000A44$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.Entity*,Unity.Entities.UnsafeCachedChunkList&,Unity.Entities.UnsafeMatchingArchetypePtrList&), entities, ref cachedChunkList, ref matchingArchetypes, functionPointer);
						return;
					}
				}
				ChunkIterationUtility.GatherEntitiesWithoutFilter$BurstManaged(entities, in cachedChunkList, in matchingArchetypes);
			}

			// Token: 0x0400053A RID: 1338
			private static IntPtr Pointer;

			// Token: 0x0400053B RID: 1339
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020001EC RID: 492
		// (Invoke) Token: 0x06000B77 RID: 2935
		internal unsafe delegate void GatherComponentDataWithFilter_00000A48$PostfixBurstDelegate(byte* componentData, TypeIndex typeIndex, in UnsafeCachedChunkList cache, in UnsafeMatchingArchetypePtrList matchingArchetypePtrList, ref EntityQueryFilter filter);

		// Token: 0x020001ED RID: 493
		internal static class GatherComponentDataWithFilter_00000A48$BurstDirectCall
		{
			// Token: 0x06000B7A RID: 2938 RVA: 0x00037319 File Offset: 0x00035519
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (ChunkIterationUtility.GatherComponentDataWithFilter_00000A48$BurstDirectCall.Pointer == 0)
				{
					ChunkIterationUtility.GatherComponentDataWithFilter_00000A48$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(ChunkIterationUtility.GatherComponentDataWithFilter_00000A48$BurstDirectCall.DeferredCompilation, methodof(ChunkIterationUtility.GatherComponentDataWithFilter$BurstManaged(byte*, TypeIndex, ref UnsafeCachedChunkList, ref UnsafeMatchingArchetypePtrList, ref EntityQueryFilter)).MethodHandle, typeof(ChunkIterationUtility.GatherComponentDataWithFilter_00000A48$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = ChunkIterationUtility.GatherComponentDataWithFilter_00000A48$BurstDirectCall.Pointer;
			}

			// Token: 0x06000B7B RID: 2939 RVA: 0x00037348 File Offset: 0x00035548
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				ChunkIterationUtility.GatherComponentDataWithFilter_00000A48$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000B7C RID: 2940 RVA: 0x00037360 File Offset: 0x00035560
			public unsafe static void Constructor()
			{
				ChunkIterationUtility.GatherComponentDataWithFilter_00000A48$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(ChunkIterationUtility.GatherComponentDataWithFilter(byte*, TypeIndex, ref UnsafeCachedChunkList, ref UnsafeMatchingArchetypePtrList, ref EntityQueryFilter)).MethodHandle);
			}

			// Token: 0x06000B7D RID: 2941 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06000B7E RID: 2942 RVA: 0x00037371 File Offset: 0x00035571
			// Note: this type is marked as 'beforefieldinit'.
			static GatherComponentDataWithFilter_00000A48$BurstDirectCall()
			{
				ChunkIterationUtility.GatherComponentDataWithFilter_00000A48$BurstDirectCall.Constructor();
			}

			// Token: 0x06000B7F RID: 2943 RVA: 0x00037378 File Offset: 0x00035578
			public unsafe static void Invoke(byte* componentData, TypeIndex typeIndex, in UnsafeCachedChunkList cache, in UnsafeMatchingArchetypePtrList matchingArchetypePtrList, ref EntityQueryFilter filter)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = ChunkIterationUtility.GatherComponentDataWithFilter_00000A48$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(System.Byte*,Unity.Entities.TypeIndex,Unity.Entities.UnsafeCachedChunkList&,Unity.Entities.UnsafeMatchingArchetypePtrList&,Unity.Entities.EntityQueryFilter&), componentData, typeIndex, ref cache, ref matchingArchetypePtrList, ref filter, functionPointer);
						return;
					}
				}
				ChunkIterationUtility.GatherComponentDataWithFilter$BurstManaged(componentData, typeIndex, in cache, in matchingArchetypePtrList, ref filter);
			}

			// Token: 0x0400053C RID: 1340
			private static IntPtr Pointer;

			// Token: 0x0400053D RID: 1341
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020001EE RID: 494
		// (Invoke) Token: 0x06000B81 RID: 2945
		internal unsafe delegate void GatherComponentDataWithoutFilter_00000A49$PostfixBurstDelegate(byte* componentData, TypeIndex typeIndex, in UnsafeCachedChunkList cachedChunkList, in UnsafeMatchingArchetypePtrList matchingArchetypePtrList);

		// Token: 0x020001EF RID: 495
		internal static class GatherComponentDataWithoutFilter_00000A49$BurstDirectCall
		{
			// Token: 0x06000B84 RID: 2948 RVA: 0x000373B3 File Offset: 0x000355B3
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (ChunkIterationUtility.GatherComponentDataWithoutFilter_00000A49$BurstDirectCall.Pointer == 0)
				{
					ChunkIterationUtility.GatherComponentDataWithoutFilter_00000A49$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(ChunkIterationUtility.GatherComponentDataWithoutFilter_00000A49$BurstDirectCall.DeferredCompilation, methodof(ChunkIterationUtility.GatherComponentDataWithoutFilter$BurstManaged(byte*, TypeIndex, ref UnsafeCachedChunkList, ref UnsafeMatchingArchetypePtrList)).MethodHandle, typeof(ChunkIterationUtility.GatherComponentDataWithoutFilter_00000A49$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = ChunkIterationUtility.GatherComponentDataWithoutFilter_00000A49$BurstDirectCall.Pointer;
			}

			// Token: 0x06000B85 RID: 2949 RVA: 0x000373E0 File Offset: 0x000355E0
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				ChunkIterationUtility.GatherComponentDataWithoutFilter_00000A49$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000B86 RID: 2950 RVA: 0x000373F8 File Offset: 0x000355F8
			public unsafe static void Constructor()
			{
				ChunkIterationUtility.GatherComponentDataWithoutFilter_00000A49$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(ChunkIterationUtility.GatherComponentDataWithoutFilter(byte*, TypeIndex, ref UnsafeCachedChunkList, ref UnsafeMatchingArchetypePtrList)).MethodHandle);
			}

			// Token: 0x06000B87 RID: 2951 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06000B88 RID: 2952 RVA: 0x00037409 File Offset: 0x00035609
			// Note: this type is marked as 'beforefieldinit'.
			static GatherComponentDataWithoutFilter_00000A49$BurstDirectCall()
			{
				ChunkIterationUtility.GatherComponentDataWithoutFilter_00000A49$BurstDirectCall.Constructor();
			}

			// Token: 0x06000B89 RID: 2953 RVA: 0x00037410 File Offset: 0x00035610
			public unsafe static void Invoke(byte* componentData, TypeIndex typeIndex, in UnsafeCachedChunkList cachedChunkList, in UnsafeMatchingArchetypePtrList matchingArchetypePtrList)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = ChunkIterationUtility.GatherComponentDataWithoutFilter_00000A49$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(System.Byte*,Unity.Entities.TypeIndex,Unity.Entities.UnsafeCachedChunkList&,Unity.Entities.UnsafeMatchingArchetypePtrList&), componentData, typeIndex, ref cachedChunkList, ref matchingArchetypePtrList, functionPointer);
						return;
					}
				}
				ChunkIterationUtility.GatherComponentDataWithoutFilter$BurstManaged(componentData, typeIndex, in cachedChunkList, in matchingArchetypePtrList);
			}

			// Token: 0x0400053E RID: 1342
			private static IntPtr Pointer;

			// Token: 0x0400053F RID: 1343
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020001F0 RID: 496
		// (Invoke) Token: 0x06000B8B RID: 2955
		internal unsafe delegate void CopyComponentArrayToChunksWithFilter_00000A4D$PostfixBurstDelegate(byte* componentData, TypeIndex typeIndex, ref UnsafeMatchingArchetypePtrList matchingArchetypePtrList, ref EntityQueryFilter filter, in UnsafeCachedChunkList cache, uint globalSystemVersion);

		// Token: 0x020001F1 RID: 497
		internal static class CopyComponentArrayToChunksWithFilter_00000A4D$BurstDirectCall
		{
			// Token: 0x06000B8E RID: 2958 RVA: 0x00037447 File Offset: 0x00035647
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (ChunkIterationUtility.CopyComponentArrayToChunksWithFilter_00000A4D$BurstDirectCall.Pointer == 0)
				{
					ChunkIterationUtility.CopyComponentArrayToChunksWithFilter_00000A4D$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(ChunkIterationUtility.CopyComponentArrayToChunksWithFilter_00000A4D$BurstDirectCall.DeferredCompilation, methodof(ChunkIterationUtility.CopyComponentArrayToChunksWithFilter$BurstManaged(byte*, TypeIndex, ref UnsafeMatchingArchetypePtrList, ref EntityQueryFilter, ref UnsafeCachedChunkList, uint)).MethodHandle, typeof(ChunkIterationUtility.CopyComponentArrayToChunksWithFilter_00000A4D$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = ChunkIterationUtility.CopyComponentArrayToChunksWithFilter_00000A4D$BurstDirectCall.Pointer;
			}

			// Token: 0x06000B8F RID: 2959 RVA: 0x00037474 File Offset: 0x00035674
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				ChunkIterationUtility.CopyComponentArrayToChunksWithFilter_00000A4D$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000B90 RID: 2960 RVA: 0x0003748C File Offset: 0x0003568C
			public unsafe static void Constructor()
			{
				ChunkIterationUtility.CopyComponentArrayToChunksWithFilter_00000A4D$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(ChunkIterationUtility.CopyComponentArrayToChunksWithFilter(byte*, TypeIndex, ref UnsafeMatchingArchetypePtrList, ref EntityQueryFilter, ref UnsafeCachedChunkList, uint)).MethodHandle);
			}

			// Token: 0x06000B91 RID: 2961 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06000B92 RID: 2962 RVA: 0x0003749D File Offset: 0x0003569D
			// Note: this type is marked as 'beforefieldinit'.
			static CopyComponentArrayToChunksWithFilter_00000A4D$BurstDirectCall()
			{
				ChunkIterationUtility.CopyComponentArrayToChunksWithFilter_00000A4D$BurstDirectCall.Constructor();
			}

			// Token: 0x06000B93 RID: 2963 RVA: 0x000374A4 File Offset: 0x000356A4
			public unsafe static void Invoke(byte* componentData, TypeIndex typeIndex, ref UnsafeMatchingArchetypePtrList matchingArchetypePtrList, ref EntityQueryFilter filter, in UnsafeCachedChunkList cache, uint globalSystemVersion)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = ChunkIterationUtility.CopyComponentArrayToChunksWithFilter_00000A4D$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(System.Byte*,Unity.Entities.TypeIndex,Unity.Entities.UnsafeMatchingArchetypePtrList&,Unity.Entities.EntityQueryFilter&,Unity.Entities.UnsafeCachedChunkList&,System.UInt32), componentData, typeIndex, ref matchingArchetypePtrList, ref filter, ref cache, globalSystemVersion, functionPointer);
						return;
					}
				}
				ChunkIterationUtility.CopyComponentArrayToChunksWithFilter$BurstManaged(componentData, typeIndex, ref matchingArchetypePtrList, ref filter, in cache, globalSystemVersion);
			}

			// Token: 0x04000540 RID: 1344
			private static IntPtr Pointer;

			// Token: 0x04000541 RID: 1345
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020001F2 RID: 498
		// (Invoke) Token: 0x06000B95 RID: 2965
		internal unsafe delegate void CopyComponentArrayToChunksWithoutFilter_00000A4E$PostfixBurstDelegate(byte* componentData, TypeIndex typeIndex, in UnsafeCachedChunkList cachedChunkList, in UnsafeMatchingArchetypePtrList matchingArchetypePtrList, uint globalSystemVersion);

		// Token: 0x020001F3 RID: 499
		internal static class CopyComponentArrayToChunksWithoutFilter_00000A4E$BurstDirectCall
		{
			// Token: 0x06000B98 RID: 2968 RVA: 0x000374E3 File Offset: 0x000356E3
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (ChunkIterationUtility.CopyComponentArrayToChunksWithoutFilter_00000A4E$BurstDirectCall.Pointer == 0)
				{
					ChunkIterationUtility.CopyComponentArrayToChunksWithoutFilter_00000A4E$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(ChunkIterationUtility.CopyComponentArrayToChunksWithoutFilter_00000A4E$BurstDirectCall.DeferredCompilation, methodof(ChunkIterationUtility.CopyComponentArrayToChunksWithoutFilter$BurstManaged(byte*, TypeIndex, ref UnsafeCachedChunkList, ref UnsafeMatchingArchetypePtrList, uint)).MethodHandle, typeof(ChunkIterationUtility.CopyComponentArrayToChunksWithoutFilter_00000A4E$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = ChunkIterationUtility.CopyComponentArrayToChunksWithoutFilter_00000A4E$BurstDirectCall.Pointer;
			}

			// Token: 0x06000B99 RID: 2969 RVA: 0x00037510 File Offset: 0x00035710
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				ChunkIterationUtility.CopyComponentArrayToChunksWithoutFilter_00000A4E$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000B9A RID: 2970 RVA: 0x00037528 File Offset: 0x00035728
			public unsafe static void Constructor()
			{
				ChunkIterationUtility.CopyComponentArrayToChunksWithoutFilter_00000A4E$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(ChunkIterationUtility.CopyComponentArrayToChunksWithoutFilter(byte*, TypeIndex, ref UnsafeCachedChunkList, ref UnsafeMatchingArchetypePtrList, uint)).MethodHandle);
			}

			// Token: 0x06000B9B RID: 2971 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06000B9C RID: 2972 RVA: 0x00037539 File Offset: 0x00035739
			// Note: this type is marked as 'beforefieldinit'.
			static CopyComponentArrayToChunksWithoutFilter_00000A4E$BurstDirectCall()
			{
				ChunkIterationUtility.CopyComponentArrayToChunksWithoutFilter_00000A4E$BurstDirectCall.Constructor();
			}

			// Token: 0x06000B9D RID: 2973 RVA: 0x00037540 File Offset: 0x00035740
			public unsafe static void Invoke(byte* componentData, TypeIndex typeIndex, in UnsafeCachedChunkList cachedChunkList, in UnsafeMatchingArchetypePtrList matchingArchetypePtrList, uint globalSystemVersion)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = ChunkIterationUtility.CopyComponentArrayToChunksWithoutFilter_00000A4E$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(System.Byte*,Unity.Entities.TypeIndex,Unity.Entities.UnsafeCachedChunkList&,Unity.Entities.UnsafeMatchingArchetypePtrList&,System.UInt32), componentData, typeIndex, ref cachedChunkList, ref matchingArchetypePtrList, globalSystemVersion, functionPointer);
						return;
					}
				}
				ChunkIterationUtility.CopyComponentArrayToChunksWithoutFilter$BurstManaged(componentData, typeIndex, in cachedChunkList, in matchingArchetypePtrList, globalSystemVersion);
			}

			// Token: 0x04000542 RID: 1346
			private static IntPtr Pointer;

			// Token: 0x04000543 RID: 1347
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020001F4 RID: 500
		// (Invoke) Token: 0x06000B9F RID: 2975
		internal delegate int CalculateChunkCount_00000A52$PostfixBurstDelegate(in UnsafeCachedChunkList cachedChunkList, ref UnsafeMatchingArchetypePtrList matchingArchetypes, ref EntityQueryFilter filter, int queryHasEnableableComponents);

		// Token: 0x020001F5 RID: 501
		internal static class CalculateChunkCount_00000A52$BurstDirectCall
		{
			// Token: 0x06000BA2 RID: 2978 RVA: 0x0003757B File Offset: 0x0003577B
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (ChunkIterationUtility.CalculateChunkCount_00000A52$BurstDirectCall.Pointer == 0)
				{
					ChunkIterationUtility.CalculateChunkCount_00000A52$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(ChunkIterationUtility.CalculateChunkCount_00000A52$BurstDirectCall.DeferredCompilation, methodof(ChunkIterationUtility.CalculateChunkCount$BurstManaged(ref UnsafeCachedChunkList, ref UnsafeMatchingArchetypePtrList, ref EntityQueryFilter, int)).MethodHandle, typeof(ChunkIterationUtility.CalculateChunkCount_00000A52$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = ChunkIterationUtility.CalculateChunkCount_00000A52$BurstDirectCall.Pointer;
			}

			// Token: 0x06000BA3 RID: 2979 RVA: 0x000375A8 File Offset: 0x000357A8
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				ChunkIterationUtility.CalculateChunkCount_00000A52$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000BA4 RID: 2980 RVA: 0x000375C0 File Offset: 0x000357C0
			public static void Constructor()
			{
				ChunkIterationUtility.CalculateChunkCount_00000A52$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(ChunkIterationUtility.CalculateChunkCount(ref UnsafeCachedChunkList, ref UnsafeMatchingArchetypePtrList, ref EntityQueryFilter, int)).MethodHandle);
			}

			// Token: 0x06000BA5 RID: 2981 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06000BA6 RID: 2982 RVA: 0x000375D1 File Offset: 0x000357D1
			// Note: this type is marked as 'beforefieldinit'.
			static CalculateChunkCount_00000A52$BurstDirectCall()
			{
				ChunkIterationUtility.CalculateChunkCount_00000A52$BurstDirectCall.Constructor();
			}

			// Token: 0x06000BA7 RID: 2983 RVA: 0x000375D8 File Offset: 0x000357D8
			public static int Invoke(in UnsafeCachedChunkList cachedChunkList, ref UnsafeMatchingArchetypePtrList matchingArchetypes, ref EntityQueryFilter filter, int queryHasEnableableComponents)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = ChunkIterationUtility.CalculateChunkCount_00000A52$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(Unity.Entities.UnsafeCachedChunkList&,Unity.Entities.UnsafeMatchingArchetypePtrList&,Unity.Entities.EntityQueryFilter&,System.Int32), ref cachedChunkList, ref matchingArchetypes, ref filter, queryHasEnableableComponents, functionPointer);
					}
				}
				return ChunkIterationUtility.CalculateChunkCount$BurstManaged(in cachedChunkList, ref matchingArchetypes, ref filter, queryHasEnableableComponents);
			}

			// Token: 0x04000544 RID: 1348
			private static IntPtr Pointer;

			// Token: 0x04000545 RID: 1349
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020001F6 RID: 502
		// (Invoke) Token: 0x06000BA9 RID: 2985
		internal delegate bool IsEmpty_00000A53$PostfixBurstDelegate(ref EntityQueryImpl queryImpl, in EntityQueryFilter filter);

		// Token: 0x020001F7 RID: 503
		internal static class IsEmpty_00000A53$BurstDirectCall
		{
			// Token: 0x06000BAC RID: 2988 RVA: 0x0003760F File Offset: 0x0003580F
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (ChunkIterationUtility.IsEmpty_00000A53$BurstDirectCall.Pointer == 0)
				{
					ChunkIterationUtility.IsEmpty_00000A53$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(ChunkIterationUtility.IsEmpty_00000A53$BurstDirectCall.DeferredCompilation, methodof(ChunkIterationUtility.IsEmpty$BurstManaged(ref EntityQueryImpl, ref EntityQueryFilter)).MethodHandle, typeof(ChunkIterationUtility.IsEmpty_00000A53$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = ChunkIterationUtility.IsEmpty_00000A53$BurstDirectCall.Pointer;
			}

			// Token: 0x06000BAD RID: 2989 RVA: 0x0003763C File Offset: 0x0003583C
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				ChunkIterationUtility.IsEmpty_00000A53$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000BAE RID: 2990 RVA: 0x00037654 File Offset: 0x00035854
			public static void Constructor()
			{
				ChunkIterationUtility.IsEmpty_00000A53$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(ChunkIterationUtility.IsEmpty(ref EntityQueryImpl, ref EntityQueryFilter)).MethodHandle);
			}

			// Token: 0x06000BAF RID: 2991 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06000BB0 RID: 2992 RVA: 0x00037665 File Offset: 0x00035865
			// Note: this type is marked as 'beforefieldinit'.
			static IsEmpty_00000A53$BurstDirectCall()
			{
				ChunkIterationUtility.IsEmpty_00000A53$BurstDirectCall.Constructor();
			}

			// Token: 0x06000BB1 RID: 2993 RVA: 0x0003766C File Offset: 0x0003586C
			public static bool Invoke(ref EntityQueryImpl queryImpl, in EntityQueryFilter filter)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = ChunkIterationUtility.IsEmpty_00000A53$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Boolean(Unity.Entities.EntityQueryImpl&,Unity.Entities.EntityQueryFilter&), ref queryImpl, ref filter, functionPointer);
					}
				}
				return ChunkIterationUtility.IsEmpty$BurstManaged(ref queryImpl, in filter);
			}

			// Token: 0x04000546 RID: 1350
			private static IntPtr Pointer;

			// Token: 0x04000547 RID: 1351
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020001F8 RID: 504
		// (Invoke) Token: 0x06000BB3 RID: 2995
		internal delegate void CalculateFilteredChunkIndexArray_00000A54$PostfixBurstDelegate(in UnsafeCachedChunkList cachedChunkList, in UnsafeMatchingArchetypePtrList matchingArchetypes, ref EntityQueryFilter filter, int hasEnableableComponents, ref NativeArray<int> outFilteredChunkIndices);

		// Token: 0x020001F9 RID: 505
		internal static class CalculateFilteredChunkIndexArray_00000A54$BurstDirectCall
		{
			// Token: 0x06000BB6 RID: 2998 RVA: 0x0003769F File Offset: 0x0003589F
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (ChunkIterationUtility.CalculateFilteredChunkIndexArray_00000A54$BurstDirectCall.Pointer == 0)
				{
					ChunkIterationUtility.CalculateFilteredChunkIndexArray_00000A54$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(ChunkIterationUtility.CalculateFilteredChunkIndexArray_00000A54$BurstDirectCall.DeferredCompilation, methodof(ChunkIterationUtility.CalculateFilteredChunkIndexArray$BurstManaged(ref UnsafeCachedChunkList, ref UnsafeMatchingArchetypePtrList, ref EntityQueryFilter, int, ref NativeArray<int>)).MethodHandle, typeof(ChunkIterationUtility.CalculateFilteredChunkIndexArray_00000A54$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = ChunkIterationUtility.CalculateFilteredChunkIndexArray_00000A54$BurstDirectCall.Pointer;
			}

			// Token: 0x06000BB7 RID: 2999 RVA: 0x000376CC File Offset: 0x000358CC
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				ChunkIterationUtility.CalculateFilteredChunkIndexArray_00000A54$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000BB8 RID: 3000 RVA: 0x000376E4 File Offset: 0x000358E4
			public static void Constructor()
			{
				ChunkIterationUtility.CalculateFilteredChunkIndexArray_00000A54$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(ChunkIterationUtility.CalculateFilteredChunkIndexArray(ref UnsafeCachedChunkList, ref UnsafeMatchingArchetypePtrList, ref EntityQueryFilter, int, ref NativeArray<int>)).MethodHandle);
			}

			// Token: 0x06000BB9 RID: 3001 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06000BBA RID: 3002 RVA: 0x000376F5 File Offset: 0x000358F5
			// Note: this type is marked as 'beforefieldinit'.
			static CalculateFilteredChunkIndexArray_00000A54$BurstDirectCall()
			{
				ChunkIterationUtility.CalculateFilteredChunkIndexArray_00000A54$BurstDirectCall.Constructor();
			}

			// Token: 0x06000BBB RID: 3003 RVA: 0x000376FC File Offset: 0x000358FC
			public static void Invoke(in UnsafeCachedChunkList cachedChunkList, in UnsafeMatchingArchetypePtrList matchingArchetypes, ref EntityQueryFilter filter, int hasEnableableComponents, ref NativeArray<int> outFilteredChunkIndices)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = ChunkIterationUtility.CalculateFilteredChunkIndexArray_00000A54$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.UnsafeCachedChunkList&,Unity.Entities.UnsafeMatchingArchetypePtrList&,Unity.Entities.EntityQueryFilter&,System.Int32,Unity.Collections.NativeArray`1<System.Int32>&), ref cachedChunkList, ref matchingArchetypes, ref filter, hasEnableableComponents, ref outFilteredChunkIndices, functionPointer);
						return;
					}
				}
				ChunkIterationUtility.CalculateFilteredChunkIndexArray$BurstManaged(in cachedChunkList, in matchingArchetypes, ref filter, hasEnableableComponents, ref outFilteredChunkIndices);
			}

			// Token: 0x04000548 RID: 1352
			private static IntPtr Pointer;

			// Token: 0x04000549 RID: 1353
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020001FA RID: 506
		// (Invoke) Token: 0x06000BBD RID: 3005
		internal delegate void CalculateBaseEntityIndexArray_00000A55$PostfixBurstDelegate(in UnsafeCachedChunkList cachedChunkList, in UnsafeMatchingArchetypePtrList matchingArchetypes, ref EntityQueryFilter filter, int hasEnableableComponents, ref NativeArray<int> outChunkBaseEntityIndices);

		// Token: 0x020001FB RID: 507
		internal static class CalculateBaseEntityIndexArray_00000A55$BurstDirectCall
		{
			// Token: 0x06000BC0 RID: 3008 RVA: 0x00037737 File Offset: 0x00035937
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (ChunkIterationUtility.CalculateBaseEntityIndexArray_00000A55$BurstDirectCall.Pointer == 0)
				{
					ChunkIterationUtility.CalculateBaseEntityIndexArray_00000A55$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(ChunkIterationUtility.CalculateBaseEntityIndexArray_00000A55$BurstDirectCall.DeferredCompilation, methodof(ChunkIterationUtility.CalculateBaseEntityIndexArray$BurstManaged(ref UnsafeCachedChunkList, ref UnsafeMatchingArchetypePtrList, ref EntityQueryFilter, int, ref NativeArray<int>)).MethodHandle, typeof(ChunkIterationUtility.CalculateBaseEntityIndexArray_00000A55$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = ChunkIterationUtility.CalculateBaseEntityIndexArray_00000A55$BurstDirectCall.Pointer;
			}

			// Token: 0x06000BC1 RID: 3009 RVA: 0x00037764 File Offset: 0x00035964
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				ChunkIterationUtility.CalculateBaseEntityIndexArray_00000A55$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000BC2 RID: 3010 RVA: 0x0003777C File Offset: 0x0003597C
			public static void Constructor()
			{
				ChunkIterationUtility.CalculateBaseEntityIndexArray_00000A55$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(ChunkIterationUtility.CalculateBaseEntityIndexArray(ref UnsafeCachedChunkList, ref UnsafeMatchingArchetypePtrList, ref EntityQueryFilter, int, ref NativeArray<int>)).MethodHandle);
			}

			// Token: 0x06000BC3 RID: 3011 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06000BC4 RID: 3012 RVA: 0x0003778D File Offset: 0x0003598D
			// Note: this type is marked as 'beforefieldinit'.
			static CalculateBaseEntityIndexArray_00000A55$BurstDirectCall()
			{
				ChunkIterationUtility.CalculateBaseEntityIndexArray_00000A55$BurstDirectCall.Constructor();
			}

			// Token: 0x06000BC5 RID: 3013 RVA: 0x00037794 File Offset: 0x00035994
			public static void Invoke(in UnsafeCachedChunkList cachedChunkList, in UnsafeMatchingArchetypePtrList matchingArchetypes, ref EntityQueryFilter filter, int hasEnableableComponents, ref NativeArray<int> outChunkBaseEntityIndices)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = ChunkIterationUtility.CalculateBaseEntityIndexArray_00000A55$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.UnsafeCachedChunkList&,Unity.Entities.UnsafeMatchingArchetypePtrList&,Unity.Entities.EntityQueryFilter&,System.Int32,Unity.Collections.NativeArray`1<System.Int32>&), ref cachedChunkList, ref matchingArchetypes, ref filter, hasEnableableComponents, ref outChunkBaseEntityIndices, functionPointer);
						return;
					}
				}
				ChunkIterationUtility.CalculateBaseEntityIndexArray$BurstManaged(in cachedChunkList, in matchingArchetypes, ref filter, hasEnableableComponents, ref outChunkBaseEntityIndices);
			}

			// Token: 0x0400054A RID: 1354
			private static IntPtr Pointer;

			// Token: 0x0400054B RID: 1355
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020001FC RID: 508
		// (Invoke) Token: 0x06000BC7 RID: 3015
		internal delegate int CalculateEntityCount_00000A56$PostfixBurstDelegate(in UnsafeCachedChunkList cachedChunkList, ref UnsafeMatchingArchetypePtrList matchingArchetypes, ref EntityQueryFilter filter, int hasEnableableComponents);

		// Token: 0x020001FD RID: 509
		internal static class CalculateEntityCount_00000A56$BurstDirectCall
		{
			// Token: 0x06000BCA RID: 3018 RVA: 0x000377CF File Offset: 0x000359CF
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (ChunkIterationUtility.CalculateEntityCount_00000A56$BurstDirectCall.Pointer == 0)
				{
					ChunkIterationUtility.CalculateEntityCount_00000A56$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(ChunkIterationUtility.CalculateEntityCount_00000A56$BurstDirectCall.DeferredCompilation, methodof(ChunkIterationUtility.CalculateEntityCount$BurstManaged(ref UnsafeCachedChunkList, ref UnsafeMatchingArchetypePtrList, ref EntityQueryFilter, int)).MethodHandle, typeof(ChunkIterationUtility.CalculateEntityCount_00000A56$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = ChunkIterationUtility.CalculateEntityCount_00000A56$BurstDirectCall.Pointer;
			}

			// Token: 0x06000BCB RID: 3019 RVA: 0x000377FC File Offset: 0x000359FC
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				ChunkIterationUtility.CalculateEntityCount_00000A56$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000BCC RID: 3020 RVA: 0x00037814 File Offset: 0x00035A14
			public static void Constructor()
			{
				ChunkIterationUtility.CalculateEntityCount_00000A56$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(ChunkIterationUtility.CalculateEntityCount(ref UnsafeCachedChunkList, ref UnsafeMatchingArchetypePtrList, ref EntityQueryFilter, int)).MethodHandle);
			}

			// Token: 0x06000BCD RID: 3021 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06000BCE RID: 3022 RVA: 0x00037825 File Offset: 0x00035A25
			// Note: this type is marked as 'beforefieldinit'.
			static CalculateEntityCount_00000A56$BurstDirectCall()
			{
				ChunkIterationUtility.CalculateEntityCount_00000A56$BurstDirectCall.Constructor();
			}

			// Token: 0x06000BCF RID: 3023 RVA: 0x0003782C File Offset: 0x00035A2C
			public static int Invoke(in UnsafeCachedChunkList cachedChunkList, ref UnsafeMatchingArchetypePtrList matchingArchetypes, ref EntityQueryFilter filter, int hasEnableableComponents)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = ChunkIterationUtility.CalculateEntityCount_00000A56$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(Unity.Entities.UnsafeCachedChunkList&,Unity.Entities.UnsafeMatchingArchetypePtrList&,Unity.Entities.EntityQueryFilter&,System.Int32), ref cachedChunkList, ref matchingArchetypes, ref filter, hasEnableableComponents, functionPointer);
					}
				}
				return ChunkIterationUtility.CalculateEntityCount$BurstManaged(in cachedChunkList, ref matchingArchetypes, ref filter, hasEnableableComponents);
			}

			// Token: 0x0400054C RID: 1356
			private static IntPtr Pointer;

			// Token: 0x0400054D RID: 1357
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020001FE RID: 510
		// (Invoke) Token: 0x06000BD1 RID: 3025
		internal unsafe delegate int CalculateEntityCountAndSingleton_00000A57$PostfixBurstDelegate(in UnsafeCachedChunkList cachedChunkList, ref UnsafeMatchingArchetypePtrList matchingArchetypes, ref EntityQueryFilter filter, int hasEnableableComponents, out MatchingArchetype* firstMatchArchetype, out ChunkIndex firstMatchChunk, out int firstMatchEntityIndexInChunk);

		// Token: 0x020001FF RID: 511
		internal static class CalculateEntityCountAndSingleton_00000A57$BurstDirectCall
		{
			// Token: 0x06000BD4 RID: 3028 RVA: 0x00037863 File Offset: 0x00035A63
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (ChunkIterationUtility.CalculateEntityCountAndSingleton_00000A57$BurstDirectCall.Pointer == 0)
				{
					ChunkIterationUtility.CalculateEntityCountAndSingleton_00000A57$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(ChunkIterationUtility.CalculateEntityCountAndSingleton_00000A57$BurstDirectCall.DeferredCompilation, methodof(ChunkIterationUtility.CalculateEntityCountAndSingleton$BurstManaged(ref UnsafeCachedChunkList, ref UnsafeMatchingArchetypePtrList, ref EntityQueryFilter, int, ref MatchingArchetype*, ref ChunkIndex, ref int)).MethodHandle, typeof(ChunkIterationUtility.CalculateEntityCountAndSingleton_00000A57$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = ChunkIterationUtility.CalculateEntityCountAndSingleton_00000A57$BurstDirectCall.Pointer;
			}

			// Token: 0x06000BD5 RID: 3029 RVA: 0x00037890 File Offset: 0x00035A90
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				ChunkIterationUtility.CalculateEntityCountAndSingleton_00000A57$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000BD6 RID: 3030 RVA: 0x000378A8 File Offset: 0x00035AA8
			public unsafe static void Constructor()
			{
				ChunkIterationUtility.CalculateEntityCountAndSingleton_00000A57$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(ChunkIterationUtility.CalculateEntityCountAndSingleton(ref UnsafeCachedChunkList, ref UnsafeMatchingArchetypePtrList, ref EntityQueryFilter, int, ref MatchingArchetype*, ref ChunkIndex, ref int)).MethodHandle);
			}

			// Token: 0x06000BD7 RID: 3031 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06000BD8 RID: 3032 RVA: 0x000378B9 File Offset: 0x00035AB9
			// Note: this type is marked as 'beforefieldinit'.
			static CalculateEntityCountAndSingleton_00000A57$BurstDirectCall()
			{
				ChunkIterationUtility.CalculateEntityCountAndSingleton_00000A57$BurstDirectCall.Constructor();
			}

			// Token: 0x06000BD9 RID: 3033 RVA: 0x000378C0 File Offset: 0x00035AC0
			public unsafe static int Invoke(in UnsafeCachedChunkList cachedChunkList, ref UnsafeMatchingArchetypePtrList matchingArchetypes, ref EntityQueryFilter filter, int hasEnableableComponents, out MatchingArchetype* firstMatchArchetype, out ChunkIndex firstMatchChunk, out int firstMatchEntityIndexInChunk)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = ChunkIterationUtility.CalculateEntityCountAndSingleton_00000A57$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(Unity.Entities.UnsafeCachedChunkList&,Unity.Entities.UnsafeMatchingArchetypePtrList&,Unity.Entities.EntityQueryFilter&,System.Int32,Unity.Entities.MatchingArchetype*&,Unity.Entities.ChunkIndex&,System.Int32&), ref cachedChunkList, ref matchingArchetypes, ref filter, hasEnableableComponents, ref firstMatchArchetype, ref firstMatchChunk, ref firstMatchEntityIndexInChunk, functionPointer);
					}
				}
				return ChunkIterationUtility.CalculateEntityCountAndSingleton$BurstManaged(in cachedChunkList, ref matchingArchetypes, ref filter, hasEnableableComponents, out firstMatchArchetype, out firstMatchChunk, out firstMatchEntityIndexInChunk);
			}

			// Token: 0x0400054E RID: 1358
			private static IntPtr Pointer;

			// Token: 0x0400054F RID: 1359
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000200 RID: 512
		// (Invoke) Token: 0x06000BDB RID: 3035
		internal unsafe delegate void GetEnabledMask_00000A5B$PostfixBurstDelegate(ChunkIndex chunk, MatchingArchetype* matchingArchetype, out v128 enabledMask);

		// Token: 0x02000201 RID: 513
		internal static class GetEnabledMask_00000A5B$BurstDirectCall
		{
			// Token: 0x06000BDE RID: 3038 RVA: 0x00037903 File Offset: 0x00035B03
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (ChunkIterationUtility.GetEnabledMask_00000A5B$BurstDirectCall.Pointer == 0)
				{
					ChunkIterationUtility.GetEnabledMask_00000A5B$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(ChunkIterationUtility.GetEnabledMask_00000A5B$BurstDirectCall.DeferredCompilation, methodof(ChunkIterationUtility.GetEnabledMask$BurstManaged(ChunkIndex, MatchingArchetype*, ref v128)).MethodHandle, typeof(ChunkIterationUtility.GetEnabledMask_00000A5B$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = ChunkIterationUtility.GetEnabledMask_00000A5B$BurstDirectCall.Pointer;
			}

			// Token: 0x06000BDF RID: 3039 RVA: 0x00037930 File Offset: 0x00035B30
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				ChunkIterationUtility.GetEnabledMask_00000A5B$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000BE0 RID: 3040 RVA: 0x00037948 File Offset: 0x00035B48
			public unsafe static void Constructor()
			{
				ChunkIterationUtility.GetEnabledMask_00000A5B$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(ChunkIterationUtility.GetEnabledMask(ChunkIndex, MatchingArchetype*, ref v128)).MethodHandle);
			}

			// Token: 0x06000BE1 RID: 3041 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06000BE2 RID: 3042 RVA: 0x00037959 File Offset: 0x00035B59
			// Note: this type is marked as 'beforefieldinit'.
			static GetEnabledMask_00000A5B$BurstDirectCall()
			{
				ChunkIterationUtility.GetEnabledMask_00000A5B$BurstDirectCall.Constructor();
			}

			// Token: 0x06000BE3 RID: 3043 RVA: 0x00037960 File Offset: 0x00035B60
			public unsafe static void Invoke(ChunkIndex chunk, MatchingArchetype* matchingArchetype, out v128 enabledMask)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = ChunkIterationUtility.GetEnabledMask_00000A5B$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.ChunkIndex,Unity.Entities.MatchingArchetype*,Unity.Burst.Intrinsics.v128&), chunk, matchingArchetype, ref enabledMask, functionPointer);
						return;
					}
				}
				ChunkIterationUtility.GetEnabledMask$BurstManaged(chunk, matchingArchetype, out enabledMask);
			}

			// Token: 0x04000550 RID: 1360
			private static IntPtr Pointer;

			// Token: 0x04000551 RID: 1361
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000202 RID: 514
		// (Invoke) Token: 0x06000BE5 RID: 3045
		internal delegate void SetEnabledBitsOnAllChunks_00000A5D$PostfixBurstDelegate(ref EntityQueryImpl queryImpl, TypeIndex typeIndex, bool value, uint globalSystemVersion);

		// Token: 0x02000203 RID: 515
		internal static class SetEnabledBitsOnAllChunks_00000A5D$BurstDirectCall
		{
			// Token: 0x06000BE8 RID: 3048 RVA: 0x00037995 File Offset: 0x00035B95
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (ChunkIterationUtility.SetEnabledBitsOnAllChunks_00000A5D$BurstDirectCall.Pointer == 0)
				{
					ChunkIterationUtility.SetEnabledBitsOnAllChunks_00000A5D$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(ChunkIterationUtility.SetEnabledBitsOnAllChunks_00000A5D$BurstDirectCall.DeferredCompilation, methodof(ChunkIterationUtility.SetEnabledBitsOnAllChunks$BurstManaged(ref EntityQueryImpl, TypeIndex, bool, uint)).MethodHandle, typeof(ChunkIterationUtility.SetEnabledBitsOnAllChunks_00000A5D$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = ChunkIterationUtility.SetEnabledBitsOnAllChunks_00000A5D$BurstDirectCall.Pointer;
			}

			// Token: 0x06000BE9 RID: 3049 RVA: 0x000379C4 File Offset: 0x00035BC4
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				ChunkIterationUtility.SetEnabledBitsOnAllChunks_00000A5D$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000BEA RID: 3050 RVA: 0x000379DC File Offset: 0x00035BDC
			public static void Constructor()
			{
				ChunkIterationUtility.SetEnabledBitsOnAllChunks_00000A5D$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(ChunkIterationUtility.SetEnabledBitsOnAllChunks(ref EntityQueryImpl, TypeIndex, bool, uint)).MethodHandle);
			}

			// Token: 0x06000BEB RID: 3051 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06000BEC RID: 3052 RVA: 0x000379ED File Offset: 0x00035BED
			// Note: this type is marked as 'beforefieldinit'.
			static SetEnabledBitsOnAllChunks_00000A5D$BurstDirectCall()
			{
				ChunkIterationUtility.SetEnabledBitsOnAllChunks_00000A5D$BurstDirectCall.Constructor();
			}

			// Token: 0x06000BED RID: 3053 RVA: 0x000379F4 File Offset: 0x00035BF4
			public static void Invoke(ref EntityQueryImpl queryImpl, TypeIndex typeIndex, bool value, uint globalSystemVersion)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = ChunkIterationUtility.SetEnabledBitsOnAllChunks_00000A5D$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.EntityQueryImpl&,Unity.Entities.TypeIndex,System.Boolean,System.UInt32), ref queryImpl, typeIndex, value, globalSystemVersion, functionPointer);
						return;
					}
				}
				ChunkIterationUtility.SetEnabledBitsOnAllChunks$BurstManaged(ref queryImpl, typeIndex, value, globalSystemVersion);
			}

			// Token: 0x04000552 RID: 1362
			private static IntPtr Pointer;

			// Token: 0x04000553 RID: 1363
			private static IntPtr DeferredCompilation;
		}
	}
}
