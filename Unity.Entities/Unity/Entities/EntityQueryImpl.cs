using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities.Internal;
using Unity.Jobs;

namespace Unity.Entities
{
	// Token: 0x02000214 RID: 532
	[GenerateBurstMonoInterop("EntityQuery")]
	internal struct EntityQueryImpl
	{
		// Token: 0x06000C76 RID: 3190 RVA: 0x00038E44 File Offset: 0x00037044
		internal unsafe void Construct(EntityQueryData* queryData, EntityDataAccess* access, ulong seqno)
		{
			this._Access = access;
			this._QueryData = queryData;
			this._Filter = default(EntityQueryFilter);
			this._SeqNo = seqno;
			fixed (EntityQueryImpl* ptr = &this)
			{
				EntityQueryImpl* ptr2 = ptr;
				access->AliveEntityQueries.Add((ulong)(long)((IntPtr)((void*)ptr2)), 0);
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000C77 RID: 3191 RVA: 0x00038E94 File Offset: 0x00037094
		public unsafe bool IsEmpty
		{
			get
			{
				bool flag = this._QueryData->HasEnableableComponents > 0;
				if (!this._Filter.RequiresMatchesFilter && !flag)
				{
					return this.IsEmptyIgnoreFilter;
				}
				if (this.IsEmptyIgnoreFilter)
				{
					return true;
				}
				this.SyncFilterTypes();
				return ChunkIterationUtility.IsEmpty(ref this, in this._Filter);
			}
		}

		// Token: 0x06000C78 RID: 3192 RVA: 0x00038EE3 File Offset: 0x000370E3
		internal void SyncFilterTypes()
		{
			this.SyncChangeFilterTypes();
			this.SyncEnableableTypes();
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000C79 RID: 3193 RVA: 0x00038EF4 File Offset: 0x000370F4
		public bool IsEmptyIgnoreFilter
		{
			get
			{
				return this.GetMatchingChunkCache().Length == 0;
			}
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x00038F14 File Offset: 0x00037114
		[ExcludeFromBurstCompatTesting("Returns managed array")]
		internal unsafe ComponentType[] GetQueryTypes()
		{
			ComponentType[] array;
			using (NativeList<ComponentType> nativeList = new NativeList<ComponentType>(128, Allocator.Temp))
			{
				for (int i = 0; i < this._QueryData->ArchetypeQueryCount; i++)
				{
					for (int j = 0; j < this._QueryData->ArchetypeQueries[i].AnyCount; j++)
					{
						ComponentType.AccessMode accessMode = (ComponentType.AccessMode)this._QueryData->ArchetypeQueries[i].AnyAccessMode[j];
						Type type = TypeManager.GetType(this._QueryData->ArchetypeQueries[i].Any[j]);
						ComponentType componentType = ((accessMode == ComponentType.AccessMode.ReadOnly) ? ComponentType.ReadOnly(type) : ComponentType.ReadWrite(type));
						nativeList.Add(in componentType);
					}
					for (int k = 0; k < this._QueryData->ArchetypeQueries[i].AllCount; k++)
					{
						ComponentType.AccessMode accessMode2 = (ComponentType.AccessMode)this._QueryData->ArchetypeQueries[i].AllAccessMode[k];
						Type type2 = TypeManager.GetType(this._QueryData->ArchetypeQueries[i].All[k]);
						ComponentType componentType = ((accessMode2 == ComponentType.AccessMode.ReadOnly) ? ComponentType.ReadOnly(type2) : ComponentType.ReadWrite(type2));
						nativeList.Add(in componentType);
					}
					for (int l = 0; l < this._QueryData->ArchetypeQueries[i].NoneCount; l++)
					{
						Type type3 = TypeManager.GetType(this._QueryData->ArchetypeQueries[i].None[l]);
						ComponentType componentType = ComponentType.ReadOnly(type3);
						nativeList.Add(in componentType);
					}
					for (int m = 0; m < this._QueryData->ArchetypeQueries[i].DisabledCount; m++)
					{
						ComponentType.AccessMode accessMode3 = (ComponentType.AccessMode)this._QueryData->ArchetypeQueries[i].DisabledAccessMode[m];
						Type type4 = TypeManager.GetType(this._QueryData->ArchetypeQueries[i].Disabled[m]);
						ComponentType componentType = ((accessMode3 == ComponentType.AccessMode.ReadOnly) ? ComponentType.ReadOnly(type4) : ComponentType.ReadWrite(type4));
						nativeList.Add(in componentType);
					}
					for (int n = 0; n < this._QueryData->ArchetypeQueries[i].AbsentCount; n++)
					{
						Type type5 = TypeManager.GetType(this._QueryData->ArchetypeQueries[i].Absent[n]);
						ComponentType componentType = ComponentType.ReadOnly(type5);
						nativeList.Add(in componentType);
					}
					for (int num = 0; num < this._QueryData->ArchetypeQueries[i].PresentCount; num++)
					{
						ComponentType.AccessMode accessMode4 = (ComponentType.AccessMode)this._QueryData->ArchetypeQueries[i].PresentAccessMode[num];
						Type type6 = TypeManager.GetType(this._QueryData->ArchetypeQueries[i].Present[num]);
						ComponentType componentType = ((accessMode4 == ComponentType.AccessMode.ReadOnly) ? ComponentType.ReadOnly(type6) : ComponentType.ReadWrite(type6));
						nativeList.Add(in componentType);
					}
				}
				using (NativeArray<ComponentType> nativeArray = nativeList.ToArray(Allocator.Temp))
				{
					array = nativeArray.ToArray();
				}
			}
			return array;
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x00039300 File Offset: 0x00037500
		[ExcludeFromBurstCompatTesting("Returns managed array")]
		internal unsafe ComponentType[] GetReadAndWriteTypes()
		{
			ComponentType[] array = new ComponentType[this._QueryData->ReaderTypesCount + this._QueryData->WriterTypesCount];
			int num = 0;
			for (int i = 0; i < this._QueryData->ReaderTypesCount; i++)
			{
				array[num++] = ComponentType.ReadOnly(TypeManager.GetType(this._QueryData->ReaderTypes[i]));
			}
			for (int j = 0; j < this._QueryData->WriterTypesCount; j++)
			{
				array[num++] = TypeManager.GetType(this._QueryData->WriterTypes[j]);
			}
			return array;
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x000393B8 File Offset: 0x000375B8
		public unsafe void Dispose()
		{
			fixed (EntityQueryImpl* ptr = &this)
			{
				EntityQueryImpl* ptr2 = ptr;
				this._Access->AliveEntityQueries.Remove((ulong)(long)((IntPtr)((void*)ptr2)));
				if (this._CachedState.IsAllocated)
				{
					EntityQueryImpl.FreeCachedState(ptr2);
					this._CachedState = default(GCHandle);
				}
				if (this._QueryData != null)
				{
					this.ResetFilter();
				}
				this._Access = null;
				this._QueryData = null;
			}
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x00039428 File Offset: 0x00037628
		private unsafe bool GetIsReadWrite(int indexInEntityQuery)
		{
			return this._QueryData->RequiredComponents[indexInEntityQuery].AccessModeType == ComponentType.AccessMode.ReadWrite;
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x00039448 File Offset: 0x00037648
		public unsafe int CalculateEntityCount()
		{
			this.SyncFilterTypes();
			UnsafeCachedChunkList matchingChunkCache = this.GetMatchingChunkCache();
			return ChunkIterationUtility.CalculateEntityCount(in matchingChunkCache, ref this._QueryData->MatchingArchetypes, ref this._Filter, (int)this._QueryData->HasEnableableComponents);
		}

		// Token: 0x06000C7F RID: 3199 RVA: 0x00039488 File Offset: 0x00037688
		public unsafe int CalculateEntityCountWithoutFiltering()
		{
			EntityQueryFilter entityQueryFilter = default(EntityQueryFilter);
			UnsafeCachedChunkList matchingChunkCache = this.GetMatchingChunkCache();
			return ChunkIterationUtility.CalculateEntityCount(in matchingChunkCache, ref this._QueryData->MatchingArchetypes, ref entityQueryFilter, 0);
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x000394BC File Offset: 0x000376BC
		public unsafe int CalculateChunkCount()
		{
			this.SyncChangeFilterTypes();
			this.SyncEnableableTypes();
			UnsafeCachedChunkList matchingChunkCache = this.GetMatchingChunkCache();
			return ChunkIterationUtility.CalculateChunkCount(in matchingChunkCache, ref this._QueryData->MatchingArchetypes, ref this._Filter, (int)this._QueryData->HasEnableableComponents);
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x00039500 File Offset: 0x00037700
		public unsafe int CalculateChunkCountWithoutFiltering()
		{
			EntityQueryFilter entityQueryFilter = default(EntityQueryFilter);
			UnsafeCachedChunkList matchingChunkCache = this.GetMatchingChunkCache();
			return ChunkIterationUtility.CalculateChunkCount(in matchingChunkCache, ref this._QueryData->MatchingArchetypes, ref entityQueryFilter, 0);
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x00039534 File Offset: 0x00037734
		public unsafe NativeArray<int> CalculateFilteredChunkIndexArray(AllocatorManager.AllocatorHandle allocator)
		{
			this.SyncChangeFilterTypes();
			this.SyncEnableableTypes();
			NativeArray<int> nativeArray = CollectionHelper.CreateNativeArray<int>(this.CalculateChunkCountWithoutFiltering(), allocator, NativeArrayOptions.UninitializedMemory);
			UnsafeCachedChunkList matchingChunkCache = this.GetMatchingChunkCache();
			ChunkIterationUtility.CalculateFilteredChunkIndexArray(in matchingChunkCache, in this._QueryData->MatchingArchetypes, ref this._Filter, (int)this._QueryData->HasEnableableComponents, ref nativeArray);
			return nativeArray;
		}

		// Token: 0x06000C83 RID: 3203 RVA: 0x00039588 File Offset: 0x00037788
		public unsafe NativeArray<int> CalculateFilteredChunkIndexArrayAsync(AllocatorManager.AllocatorHandle allocator, JobHandle additionalInputDep, out JobHandle outJobHandle)
		{
			NativeArray<int> nativeArray = CollectionHelper.CreateNativeArray<int>(this.CalculateChunkCountWithoutFiltering(), allocator, NativeArrayOptions.ClearMemory);
			int enableableComponentTypeIndexCount = this._QueryData->EnableableComponentTypeIndexCount;
			int count = this._Filter.Changed.Count;
			int num = enableableComponentTypeIndexCount + count;
			TypeIndex* ptr;
			int num2;
			checked
			{
				ptr = stackalloc TypeIndex[unchecked((UIntPtr)num) * (UIntPtr)sizeof(TypeIndex)];
				num2 = 0;
			}
			for (int i = 0; i < count; i++)
			{
				ComponentType componentType = this._QueryData->RequiredComponents[*((ref this._Filter.Changed.IndexInEntityQuery.FixedElementField) + (IntPtr)i * 4)];
				ptr[(IntPtr)(num2++) * (IntPtr)sizeof(TypeIndex)] = componentType.TypeIndex;
			}
			for (int j = 0; j < enableableComponentTypeIndexCount; j++)
			{
				ptr[(IntPtr)(num2++) * (IntPtr)sizeof(TypeIndex)] = this._QueryData->EnableableComponentTypeIndices[j];
			}
			JobHandle jobHandle = JobHandle.CombineDependencies(additionalInputDep, this._Access->DependencyManager->GetDependency(ptr, num, null, 0, false));
			FilteredChunkIndexJob filteredChunkIndexJob = new FilteredChunkIndexJob
			{
				CachedChunkList = this.GetMatchingChunkCache(),
				Filter = this._Filter,
				MatchingArchetypes = this._QueryData->MatchingArchetypes,
				OutFilteredChunkIndices = nativeArray,
				QueryIncludesEnableableComponents = (int)this._QueryData->HasEnableableComponents
			};
			outJobHandle = filteredChunkIndexJob.Schedule(jobHandle);
			return nativeArray;
		}

		// Token: 0x06000C84 RID: 3204 RVA: 0x000396FC File Offset: 0x000378FC
		public unsafe NativeArray<int> CalculateBaseEntityIndexArray(AllocatorManager.AllocatorHandle allocator)
		{
			this.SyncChangeFilterTypes();
			this.SyncEnableableTypes();
			NativeArray<int> nativeArray = CollectionHelper.CreateNativeArray<int>(this.CalculateChunkCountWithoutFiltering(), allocator, NativeArrayOptions.UninitializedMemory);
			UnsafeCachedChunkList matchingChunkCache = this.GetMatchingChunkCache();
			ChunkIterationUtility.CalculateBaseEntityIndexArray(in matchingChunkCache, in this._QueryData->MatchingArchetypes, ref this._Filter, (int)this._QueryData->HasEnableableComponents, ref nativeArray);
			return nativeArray;
		}

		// Token: 0x06000C85 RID: 3205 RVA: 0x00039750 File Offset: 0x00037950
		public unsafe NativeArray<int> CalculateBaseEntityIndexArrayAsync(AllocatorManager.AllocatorHandle allocator, JobHandle additionalInputDep, out JobHandle outJobHandle)
		{
			NativeArray<int> nativeArray = CollectionHelper.CreateNativeArray<int>(this.CalculateChunkCountWithoutFiltering(), allocator, NativeArrayOptions.ClearMemory);
			int enableableComponentTypeIndexCount = this._QueryData->EnableableComponentTypeIndexCount;
			int count = this._Filter.Changed.Count;
			int num = enableableComponentTypeIndexCount + count;
			TypeIndex* ptr;
			int num2;
			checked
			{
				ptr = stackalloc TypeIndex[unchecked((UIntPtr)num) * (UIntPtr)sizeof(TypeIndex)];
				num2 = 0;
			}
			for (int i = 0; i < count; i++)
			{
				ComponentType componentType = this._QueryData->RequiredComponents[*((ref this._Filter.Changed.IndexInEntityQuery.FixedElementField) + (IntPtr)i * 4)];
				ptr[(IntPtr)(num2++) * (IntPtr)sizeof(TypeIndex)] = componentType.TypeIndex;
			}
			for (int j = 0; j < enableableComponentTypeIndexCount; j++)
			{
				ptr[(IntPtr)(num2++) * (IntPtr)sizeof(TypeIndex)] = this._QueryData->EnableableComponentTypeIndices[j];
			}
			JobHandle jobHandle = JobHandle.CombineDependencies(additionalInputDep, this._Access->DependencyManager->GetDependency(ptr, num, null, 0, false));
			ChunkBaseEntityIndexJob chunkBaseEntityIndexJob = new ChunkBaseEntityIndexJob
			{
				CachedChunkList = this.GetMatchingChunkCache(),
				Filter = this._Filter,
				MatchingArchetypes = this._QueryData->MatchingArchetypes,
				OutChunkBaseEntityIndices = nativeArray,
				QueryIncludesEnableableComponents = (int)this._QueryData->HasEnableableComponents
			};
			outJobHandle = chunkBaseEntityIndexJob.Schedule(jobHandle);
			return nativeArray;
		}

		// Token: 0x06000C86 RID: 3206 RVA: 0x000398C4 File Offset: 0x00037AC4
		internal unsafe int GetIndexInEntityQuery(TypeIndex componentType)
		{
			int num = 0;
			while (num < this._QueryData->RequiredComponentsCount && this._QueryData->RequiredComponents[num].TypeIndex != componentType)
			{
				num++;
			}
			return num;
		}

		// Token: 0x06000C87 RID: 3207 RVA: 0x0003990C File Offset: 0x00037B0C
		[Obsolete("Remove with CreateArchetypeChunkArrayAsync. (RemovedAfter Entities 1.0)")]
		public unsafe NativeArray<ArchetypeChunk> CreateArchetypeChunkArrayAsync(AllocatorManager.AllocatorHandle allocator, out JobHandle jobhandle)
		{
			JobHandle jobHandle = default(JobHandle);
			int count = this._Filter.Changed.Count;
			if (count > 0)
			{
				TypeIndex* ptr;
				checked
				{
					ptr = stackalloc TypeIndex[unchecked((UIntPtr)count) * (UIntPtr)sizeof(TypeIndex)];
				}
				for (int i = 0; i < count; i++)
				{
					ptr[i] = this._QueryData->RequiredComponents[*((ref this._Filter.Changed.IndexInEntityQuery.FixedElementField) + (IntPtr)i * 4)].TypeIndex;
				}
				jobHandle = this._Access->DependencyManager->GetDependency(ptr, count, null, 0, false);
			}
			return ChunkIterationUtility.CreateArchetypeChunkArrayAsync(this._QueryData->MatchingArchetypes, allocator, out jobhandle, ref this._Filter, jobHandle);
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x000399C4 File Offset: 0x00037BC4
		public unsafe NativeList<ArchetypeChunk> ToArchetypeChunkListAsync(AllocatorManager.AllocatorHandle allocator, JobHandle additionalInputDep, out JobHandle outJobHandle)
		{
			int num = this.CalculateChunkCountWithoutFiltering();
			NativeList<ArchetypeChunk> nativeList = new NativeList<ArchetypeChunk>(num, allocator);
			int enableableComponentTypeIndexCount = this._QueryData->EnableableComponentTypeIndexCount;
			int count = this._Filter.Changed.Count;
			int num2 = enableableComponentTypeIndexCount + count;
			TypeIndex* ptr;
			int num3;
			checked
			{
				ptr = stackalloc TypeIndex[unchecked((UIntPtr)num2) * (UIntPtr)sizeof(TypeIndex)];
				num3 = 0;
			}
			for (int i = 0; i < count; i++)
			{
				ComponentType componentType = this._QueryData->RequiredComponents[*((ref this._Filter.Changed.IndexInEntityQuery.FixedElementField) + (IntPtr)i * 4)];
				ptr[(IntPtr)(num3++) * (IntPtr)sizeof(TypeIndex)] = componentType.TypeIndex;
			}
			for (int j = 0; j < enableableComponentTypeIndexCount; j++)
			{
				ptr[(IntPtr)(num3++) * (IntPtr)sizeof(TypeIndex)] = this._QueryData->EnableableComponentTypeIndices[j];
			}
			JobHandle jobHandle = JobHandle.CombineDependencies(additionalInputDep, this._Access->DependencyManager->GetDependency(ptr, num2, null, 0, false));
			GatherChunksJob gatherChunksJob = new GatherChunksJob
			{
				ChunkCache = this.GetMatchingChunkCache(),
				Filter = this._Filter,
				MatchingArchetypes = this._QueryData->MatchingArchetypes,
				QueryContainsEnableableComponents = (int)this._QueryData->HasEnableableComponents,
				OutFilteredChunksList = new TypelessUnsafeList
				{
					Ptr = (byte*)nativeList.m_ListData->Ptr,
					Length = &nativeList.m_ListData->m_length,
					Capacity = nativeList.m_ListData->Capacity
				}
			};
			outJobHandle = gatherChunksJob.Schedule(jobHandle);
			return nativeList;
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x00039B80 File Offset: 0x00037D80
		public unsafe NativeArray<ArchetypeChunk> ToArchetypeChunkArray(AllocatorManager.AllocatorHandle allocator)
		{
			this.SyncChangeFilterTypes();
			this.SyncEnableableTypes();
			int num = this.CalculateChunkCountWithoutFiltering();
			if (num == 0)
			{
				return CollectionHelper.CreateNativeArray<ArchetypeChunk>(0, allocator, NativeArrayOptions.ClearMemory);
			}
			NativeList<ArchetypeChunk> nativeList = new NativeList<ArchetypeChunk>(num, allocator);
			UnsafeCachedChunkList matchingChunkCache = this.GetMatchingChunkCache();
			ChunkIterationUtility.ToArchetypeChunkList(in matchingChunkCache, in this._QueryData->MatchingArchetypes, ref this._Filter, (int)this._QueryData->HasEnableableComponents, ref nativeList);
			NativeArray<ArchetypeChunk> nativeArray = CollectionHelper.ConvertExistingNativeListToNativeArray<ArchetypeChunk>(ref nativeList, nativeList.Length, nativeList.m_ListData->Allocator);
			AllocatorManager.Free<UnsafeList<ArchetypeChunk>>(allocator, nativeList.m_ListData, 1);
			return nativeArray;
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x00039C08 File Offset: 0x00037E08
		public unsafe void ToArchetypeChunkList(NativeList<ArchetypeChunk> archetypeChunkList)
		{
			this.SyncChangeFilterTypes();
			this.SyncEnableableTypes();
			archetypeChunkList.Clear();
			int num = this.CalculateChunkCountWithoutFiltering();
			if (num == 0)
			{
				return;
			}
			archetypeChunkList.SetCapacity(num);
			UnsafeCachedChunkList matchingChunkCache = this.GetMatchingChunkCache();
			ChunkIterationUtility.ToArchetypeChunkList(in matchingChunkCache, in this._QueryData->MatchingArchetypes, ref this._Filter, (int)this._QueryData->HasEnableableComponents, ref archetypeChunkList);
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x00039C68 File Offset: 0x00037E68
		[GenerateTestsForBurstCompatibility(RequiredUnityDefine = "ENABLE_UNITY_COLLECTIONS_CHECKS", CompileTarget = GenerateTestsForBurstCompatibilityAttribute.BurstCompatibleCompileTarget.Editor)]
		[Obsolete("This method does not correctly handle enableable components, and is generally unsafe. Use ToEntityListAsync() instead. (RemovedAfter Entities 1.0)")]
		public unsafe NativeArray<Entity> ToEntityArrayAsync(AllocatorManager.AllocatorHandle allocator, out JobHandle jobhandle, EntityQuery outer)
		{
			EntityTypeHandle entityTypeHandle = default(EntityTypeHandle);
			return ChunkIterationUtility.CreateEntityArrayAsync(this._QueryData->MatchingArchetypes, allocator, entityTypeHandle, outer, this.CalculateEntityCount(), out jobhandle, this.GetDependency());
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x00039CA0 File Offset: 0x00037EA0
		[GenerateTestsForBurstCompatibility(RequiredUnityDefine = "ENABLE_UNITY_COLLECTIONS_CHECKS", CompileTarget = GenerateTestsForBurstCompatibilityAttribute.BurstCompatibleCompileTarget.Editor)]
		public unsafe NativeList<Entity> ToEntityListAsync(AllocatorManager.AllocatorHandle allocator, EntityQuery outer, JobHandle additionalInputDep, out JobHandle outJobHandle)
		{
			EntityTypeHandle entityTypeHandle = default(EntityTypeHandle);
			int num = this.CalculateEntityCountWithoutFiltering();
			int enableableComponentTypeIndexCount = this._QueryData->EnableableComponentTypeIndexCount;
			int count = this._Filter.Changed.Count;
			int num2 = enableableComponentTypeIndexCount + count;
			TypeIndex* ptr;
			int num3;
			checked
			{
				ptr = stackalloc TypeIndex[unchecked((UIntPtr)num2) * (UIntPtr)sizeof(TypeIndex)];
				num3 = 0;
			}
			for (int i = 0; i < count; i++)
			{
				ComponentType componentType = this._QueryData->RequiredComponents[*((ref this._Filter.Changed.IndexInEntityQuery.FixedElementField) + (IntPtr)i * 4)];
				ptr[(IntPtr)(num3++) * (IntPtr)sizeof(TypeIndex)] = componentType.TypeIndex;
			}
			for (int j = 0; j < enableableComponentTypeIndexCount; j++)
			{
				ptr[(IntPtr)(num3++) * (IntPtr)sizeof(TypeIndex)] = this._QueryData->EnableableComponentTypeIndices[j];
			}
			JobHandle jobHandle = JobHandle.CombineDependencies(additionalInputDep, this._Access->DependencyManager->GetDependency(ptr, num2, null, 0, false));
			return ChunkIterationUtility.CreateEntityListAsync(allocator, entityTypeHandle, outer, num, jobHandle, out outJobHandle);
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x00039DC4 File Offset: 0x00037FC4
		[GenerateTestsForBurstCompatibility(RequiredUnityDefine = "ENABLE_UNITY_COLLECTIONS_CHECKS", CompileTarget = GenerateTestsForBurstCompatibilityAttribute.BurstCompatibleCompileTarget.Editor)]
		public unsafe NativeArray<Entity> ToEntityArray(AllocatorManager.AllocatorHandle allocator, EntityQuery outer)
		{
			int num = this.CalculateEntityCount();
			this._Access->DependencyManager->CompleteWriteDependency(TypeManager.GetTypeIndex<Entity>());
			EntityTypeHandle entityTypeHandle = default(EntityTypeHandle);
			return ChunkIterationUtility.CreateEntityArray(this._QueryData->MatchingArchetypes, allocator, entityTypeHandle, outer, num);
		}

		// Token: 0x06000C8E RID: 3214 RVA: 0x00039E09 File Offset: 0x00038009
		[GenerateTestsForBurstCompatibility(RequiredUnityDefine = "ENABLE_UNITY_COLLECTIONS_CHECKS", CompileTarget = GenerateTestsForBurstCompatibilityAttribute.BurstCompatibleCompileTarget.Editor)]
		internal unsafe void GatherEntitiesToArray(out InternalGatherEntitiesResult result, EntityQuery outer)
		{
			result = default(InternalGatherEntitiesResult);
			result.EntityArray = outer.ToEntityArray(Allocator.TempJob);
			result.EntityBuffer = (Entity*)result.EntityArray.GetUnsafeReadOnlyPtr<Entity>();
			result.EntityCount = result.EntityArray.Length;
		}

		// Token: 0x06000C8F RID: 3215 RVA: 0x00039E47 File Offset: 0x00038047
		internal void ReleaseGatheredEntities(ref InternalGatherEntitiesResult result)
		{
			if (result.EntityArray.IsCreated)
			{
				result.EntityArray.Dispose();
			}
		}

		// Token: 0x06000C90 RID: 3216 RVA: 0x00039E64 File Offset: 0x00038064
		[GenerateTestsForBurstCompatibility(RequiredUnityDefine = "ENABLE_UNITY_COLLECTIONS_CHECKS", CompileTarget = GenerateTestsForBurstCompatibilityAttribute.BurstCompatibleCompileTarget.Editor, GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		[Obsolete("This method does not correctly support enableable components. Use ToComponentDataListAsync() instead. (RemovedAfter Entities 1.0)")]
		public unsafe NativeArray<T> ToComponentDataArrayAsync<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(AllocatorManager.AllocatorHandle allocator, out JobHandle jobhandle, EntityQuery outer) where T : struct, ValueType, IComponentData
		{
			ComponentTypeHandle<T> componentTypeHandle = new ComponentTypeHandle<T>(true, this._Access->EntityComponentStore->GlobalSystemVersion);
			return ChunkIterationUtility.CreateComponentDataArrayAsync<T>(allocator, ref componentTypeHandle, this.CalculateEntityCount(), outer, out jobhandle, this.GetDependency());
		}

		// Token: 0x06000C91 RID: 3217 RVA: 0x00039EA0 File Offset: 0x000380A0
		[GenerateTestsForBurstCompatibility(RequiredUnityDefine = "ENABLE_UNITY_COLLECTIONS_CHECKS", CompileTarget = GenerateTestsForBurstCompatibilityAttribute.BurstCompatibleCompileTarget.Editor, GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe NativeList<T> ToComponentDataListAsync<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(AllocatorManager.AllocatorHandle allocator, EntityQuery outer, JobHandle additionalInputDep, out JobHandle outJobHandle) where T : struct, ValueType, IComponentData
		{
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			ComponentType componentType = ComponentType.ReadOnly<T>();
			DynamicComponentTypeHandle dynamicComponentTypeHandle = new DynamicComponentTypeHandle(componentType, this._Access->EntityComponentStore->GlobalSystemVersion);
			int num = this.CalculateEntityCountWithoutFiltering();
			int enableableComponentTypeIndexCount = this._QueryData->EnableableComponentTypeIndexCount;
			int count = this._Filter.Changed.Count;
			int num2 = enableableComponentTypeIndexCount + count + 1;
			TypeIndex* ptr;
			int num3;
			checked
			{
				ptr = stackalloc TypeIndex[unchecked((UIntPtr)num2) * (UIntPtr)sizeof(TypeIndex)];
				num3 = 0;
			}
			for (int i = 0; i < count; i++)
			{
				ComponentType componentType2 = this._QueryData->RequiredComponents[*((ref this._Filter.Changed.IndexInEntityQuery.FixedElementField) + (IntPtr)i * 4)];
				ptr[(IntPtr)(num3++) * (IntPtr)sizeof(TypeIndex)] = componentType2.TypeIndex;
			}
			for (int j = 0; j < enableableComponentTypeIndexCount; j++)
			{
				ptr[(IntPtr)(num3++) * (IntPtr)sizeof(TypeIndex)] = this._QueryData->EnableableComponentTypeIndices[j];
			}
			ptr[(IntPtr)(num3++) * (IntPtr)sizeof(TypeIndex)] = typeIndex;
			JobHandle jobHandle = JobHandle.CombineDependencies(additionalInputDep, this._Access->DependencyManager->GetDependency(ptr, num2, null, 0, false));
			return ChunkIterationUtility.CreateComponentDataListAsync<T>(allocator, ref dynamicComponentTypeHandle, num, outer, jobHandle, out outJobHandle);
		}

		// Token: 0x06000C92 RID: 3218 RVA: 0x0003A000 File Offset: 0x00038200
		[GenerateTestsForBurstCompatibility(RequiredUnityDefine = "ENABLE_UNITY_COLLECTIONS_CHECKS", CompileTarget = GenerateTestsForBurstCompatibilityAttribute.BurstCompatibleCompileTarget.Editor, GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe NativeArray<T> ToComponentDataArray<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(AllocatorManager.AllocatorHandle allocator, EntityQuery outer) where T : struct, ValueType, IComponentData
		{
			int num = this.CalculateEntityCount();
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			this._Access->DependencyManager->CompleteWriteDependency(typeIndex);
			ComponentTypeHandle<T> componentTypeHandle = new ComponentTypeHandle<T>(true, this._Access->EntityComponentStore->GlobalSystemVersion);
			return ChunkIterationUtility.CreateComponentDataArray<T>(allocator, ref componentTypeHandle, num, outer);
		}

		// Token: 0x06000C93 RID: 3219 RVA: 0x0003A050 File Offset: 0x00038250
		[ExcludeFromBurstCompatTesting("Returns managed array")]
		public unsafe T[] ToComponentDataArray<T>() where T : class, IComponentData, new()
		{
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			new ComponentTypeHandle<T>(true, this._Access->EntityComponentStore->GlobalSystemVersion);
			ManagedComponentStore managedComponentStore = this._Access->ManagedComponentStore;
			UnsafeMatchingArchetypePtrList matchingArchetypes = this._QueryData->MatchingArchetypes;
			T[] array = new T[this.CalculateEntityCount()];
			int num = 0;
			UnsafeChunkCacheIterator unsafeChunkCacheIterator = new UnsafeChunkCacheIterator(in this._Filter, true, this.GetMatchingChunkCache(), matchingArchetypes.Ptr);
			int num2 = -1;
			v128 v = default(v128);
			LookupCache lookupCache = default(LookupCache);
			ArchetypeChunk archetypeChunk;
			int num3;
			byte b;
			while (unsafeChunkCacheIterator.MoveNextChunk(ref num2, out archetypeChunk, out num3, out b, ref v))
			{
				Archetype* archetype = unsafeChunkCacheIterator._CurrentMatchingArchetype->Archetype;
				if (archetype != lookupCache.Archetype)
				{
					lookupCache.Update(archetype, typeIndex);
				}
				int* componentDataRO = (int*)ChunkDataUtility.GetComponentDataRO(archetypeChunk.m_Chunk, archetype, 0, (int)lookupCache.IndexInArchetype);
				if (b == 0)
				{
					for (int i = 0; i < num3; i++)
					{
						array[num++] = (T)((object)managedComponentStore.GetManagedComponent(componentDataRO[i]));
					}
				}
				else
				{
					int num4 = 0;
					int num5;
					while (EnabledBitUtility.TryGetNextRange(v, num4, out num5, out num4))
					{
						int num6 = num4 - num5;
						for (int j = 0; j < num6; j++)
						{
							array[num++] = (T)((object)managedComponentStore.GetManagedComponent(componentDataRO[num5 + j]));
						}
					}
				}
			}
			return array;
		}

		// Token: 0x06000C94 RID: 3220 RVA: 0x0003A1B0 File Offset: 0x000383B0
		[GenerateTestsForBurstCompatibility(RequiredUnityDefine = "ENABLE_UNITY_COLLECTIONS_CHECKS", CompileTarget = GenerateTestsForBurstCompatibilityAttribute.BurstCompatibleCompileTarget.Editor, GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe void CopyFromComponentDataArray<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(NativeArray<T> componentDataArray, EntityQuery outer) where T : struct, ValueType, IComponentData
		{
			this.CalculateEntityCount();
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			this._Access->DependencyManager->CompleteReadAndWriteDependency(typeIndex);
			ComponentTypeHandle<T> componentTypeHandle = new ComponentTypeHandle<T>(false, this._Access->EntityComponentStore->GlobalSystemVersion);
			ChunkIterationUtility.CopyFromComponentDataArray<T>(componentDataArray, ref componentTypeHandle, outer);
		}

		// Token: 0x06000C95 RID: 3221 RVA: 0x0003A1FC File Offset: 0x000383FC
		[GenerateTestsForBurstCompatibility(RequiredUnityDefine = "ENABLE_UNITY_COLLECTIONS_CHECKS", CompileTarget = GenerateTestsForBurstCompatibilityAttribute.BurstCompatibleCompileTarget.Editor, GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		[Obsolete("This method does not correctly support enableable components. Use CopyFromComponentDataListAsync() instead. (RemovedAfter Entities 1.0)")]
		public unsafe void CopyFromComponentDataArrayAsync<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(NativeArray<T> componentDataArray, out JobHandle jobhandle, EntityQuery outer) where T : struct, ValueType, IComponentData
		{
			ComponentTypeHandle<T> componentTypeHandle = new ComponentTypeHandle<T>(false, this._Access->EntityComponentStore->GlobalSystemVersion);
			ChunkIterationUtility.CopyFromComponentDataArrayAsync<T>(this._QueryData->MatchingArchetypes, componentDataArray, ref componentTypeHandle, outer, ref this._Filter, out jobhandle, this.GetDependency());
		}

		// Token: 0x06000C96 RID: 3222 RVA: 0x0003A244 File Offset: 0x00038444
		[GenerateTestsForBurstCompatibility(RequiredUnityDefine = "ENABLE_UNITY_COLLECTIONS_CHECKS", CompileTarget = GenerateTestsForBurstCompatibilityAttribute.BurstCompatibleCompileTarget.Editor, GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe void CopyFromComponentDataListAsync<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(NativeList<T> componentDataList, EntityQuery outer, JobHandle additionalInputDep, out JobHandle outJobHandle) where T : struct, ValueType, IComponentData
		{
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			ComponentType componentType = ComponentType.ReadWrite<T>();
			DynamicComponentTypeHandle dynamicComponentTypeHandle = new DynamicComponentTypeHandle(componentType, this._Access->EntityComponentStore->GlobalSystemVersion);
			int enableableComponentTypeIndexCount = this._QueryData->EnableableComponentTypeIndexCount;
			int count = this._Filter.Changed.Count;
			int num = enableableComponentTypeIndexCount + count;
			TypeIndex* ptr;
			int num2;
			checked
			{
				ptr = stackalloc TypeIndex[unchecked((UIntPtr)num) * (UIntPtr)sizeof(TypeIndex)];
				num2 = 0;
			}
			for (int i = 0; i < count; i++)
			{
				ComponentType componentType2 = this._QueryData->RequiredComponents[*((ref this._Filter.Changed.IndexInEntityQuery.FixedElementField) + (IntPtr)i * 4)];
				ptr[(IntPtr)(num2++) * (IntPtr)sizeof(TypeIndex)] = componentType2.TypeIndex;
			}
			for (int j = 0; j < enableableComponentTypeIndexCount; j++)
			{
				ptr[(IntPtr)(num2++) * (IntPtr)sizeof(TypeIndex)] = this._QueryData->EnableableComponentTypeIndices[j];
			}
			JobHandle jobHandle = JobHandle.CombineDependencies(additionalInputDep, this._Access->DependencyManager->GetDependency(ptr, num, &typeIndex, 1, false));
			ChunkIterationUtility.CopyFromComponentDataListAsync<T>(componentDataList, ref dynamicComponentTypeHandle, outer, jobHandle, out outJobHandle);
		}

		// Token: 0x06000C97 RID: 3223 RVA: 0x0003A380 File Offset: 0x00038580
		public unsafe Entity GetSingletonEntity()
		{
			int num;
			ChunkIndex chunkIndex;
			int num2;
			this.GetSingletonChunkAndEntity(TypeManager.GetTypeIndex<Entity>(), out num, out chunkIndex, out num2);
			Entity* chunkComponentDataROPtr = (Entity*)ChunkIterationUtility.GetChunkComponentDataROPtr(this._Access->EntityComponentStore->GetArchetype(chunkIndex), chunkIndex, 0);
			return *UnsafeUtility.AsRef<Entity>((void*)(chunkComponentDataROPtr + num2));
		}

		// Token: 0x06000C98 RID: 3224 RVA: 0x0003A3CC File Offset: 0x000385CC
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal unsafe void GetSingletonChunkAndEntity(TypeIndex typeIndex, out int outIndexInArchetype, out ChunkIndex outChunk, out int outEntityIndexInChunk)
		{
			if (!this._Filter.RequiresMatchesFilter && this._QueryData->HasEnableableComponents == 0 && this._QueryData->RequiredComponentsCount <= 2 && this._QueryData->RequiredComponents[1].TypeIndex == typeIndex)
			{
				UnsafeCachedChunkList matchingChunkCache = this.GetMatchingChunkCache();
				outChunk = *matchingChunkCache.ChunkIndices;
				int num = *matchingChunkCache.PerChunkMatchingArchetypeIndex->Ptr;
				MatchingArchetype* ptr = *(IntPtr*)(this._QueryData->MatchingArchetypes.Ptr + (IntPtr)num * (IntPtr)sizeof(MatchingArchetype*) / (IntPtr)sizeof(MatchingArchetype*));
				outIndexInArchetype = *((ref ptr->IndexInArchetype.FixedElementField) + 4);
				outEntityIndexInChunk = 0;
				return;
			}
			this.SyncFilterTypes();
			UnsafeCachedChunkList matchingChunkCache2 = this.GetMatchingChunkCache();
			MatchingArchetype* ptr2;
			ChunkIterationUtility.CalculateEntityCountAndSingleton(in matchingChunkCache2, ref this._QueryData->MatchingArchetypes, ref this._Filter, (int)this._QueryData->HasEnableableComponents, out ptr2, out outChunk, out outEntityIndexInChunk);
			int indexInEntityQuery = this.GetIndexInEntityQuery(typeIndex);
			outIndexInArchetype = *((ref ptr2->IndexInArchetype.FixedElementField) + (IntPtr)indexInEntityQuery * 4);
		}

		// Token: 0x06000C99 RID: 3225 RVA: 0x0003A4D4 File Offset: 0x000386D4
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe RefRW<T> GetSingletonRW<[global::System.Runtime.CompilerServices.IsUnmanaged] T>() where T : struct, ValueType, IComponentData
		{
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			int num;
			ChunkIndex chunkIndex;
			int num2;
			this.GetSingletonChunkAndEntity(typeIndex, out num, out chunkIndex, out num2);
			Archetype* archetype = this._Access->EntityComponentStore->GetArchetype(chunkIndex);
			return new RefRW<T>(ChunkDataUtility.GetComponentDataRW(chunkIndex, archetype, num2, num, this._Access->EntityComponentStore->GlobalSystemVersion));
		}

		// Token: 0x06000C9A RID: 3226 RVA: 0x0003A528 File Offset: 0x00038728
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe T GetSingleton<[global::System.Runtime.CompilerServices.IsUnmanaged] T>() where T : struct, ValueType, IComponentData
		{
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			if (!this._Filter.RequiresMatchesFilter && this._QueryData->RequiredComponentsCount <= 2 && this._QueryData->RequiredComponents[1].TypeIndex == typeIndex)
			{
				UnsafeCachedChunkList matchingChunkCache = this.GetMatchingChunkCache();
				ChunkIndex chunkIndex = *matchingChunkCache.ChunkIndices;
				int num = *matchingChunkCache.PerChunkMatchingArchetypeIndex->Ptr;
				MatchingArchetype* ptr = *(IntPtr*)(this._QueryData->MatchingArchetypes.Ptr + (IntPtr)num * (IntPtr)sizeof(MatchingArchetype*) / (IntPtr)sizeof(MatchingArchetype*));
				return *UnsafeUtility.AsRef<T>(ChunkIterationUtility.GetChunkComponentDataROPtr(this._Access->EntityComponentStore->GetArchetype(chunkIndex), chunkIndex, *((ref ptr->IndexInArchetype.FixedElementField) + 4)));
			}
			int num2;
			ChunkIndex chunkIndex2;
			int num3;
			this.GetSingletonChunkAndEntity(typeIndex, out num2, out chunkIndex2, out num3);
			T* chunkComponentDataROPtr = (T*)ChunkIterationUtility.GetChunkComponentDataROPtr(this._Access->EntityComponentStore->GetArchetype(chunkIndex2), chunkIndex2, num2);
			return *UnsafeUtility.AsRef<T>((void*)(chunkComponentDataROPtr + (IntPtr)num3 * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)));
		}

		// Token: 0x06000C9B RID: 3227 RVA: 0x0003A62C File Offset: 0x0003882C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleBufferElement) })]
		public unsafe DynamicBuffer<T> GetSingletonBuffer<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(bool isReadOnly = false) where T : struct, ValueType, IBufferElementData
		{
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			if (isReadOnly)
			{
				this._Access->DependencyManager->CompleteWriteDependencyNoChecks(typeIndex);
			}
			else
			{
				this._Access->DependencyManager->CompleteReadAndWriteDependencyNoChecks(typeIndex);
			}
			int num;
			ChunkIndex chunkIndex;
			int num2;
			this.GetSingletonChunkAndEntity(typeIndex, out num, out chunkIndex, out num2);
			return ChunkIterationUtility.GetChunkBufferAccessor<T>(this._Access->EntityComponentStore->GetArchetype(chunkIndex), chunkIndex, !isReadOnly, num, this._Access->EntityComponentStore->GlobalSystemVersion)[num2];
		}

		// Token: 0x06000C9C RID: 3228 RVA: 0x0003A6AC File Offset: 0x000388AC
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe void SetSingleton<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(T value) where T : struct, ValueType, IComponentData
		{
			*this.GetSingletonRW<T>().ValueRW = value;
		}

		// Token: 0x06000C9D RID: 3229 RVA: 0x0003A6D0 File Offset: 0x000388D0
		public bool TryGetSingleton<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(out T value) where T : struct, ValueType, IComponentData
		{
			bool flag = this.HasSingleton<T>();
			value = (flag ? this.GetSingleton<T>() : default(T));
			return flag;
		}

		// Token: 0x06000C9E RID: 3230 RVA: 0x0003A700 File Offset: 0x00038900
		public bool TryGetSingletonRW<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(out RefRW<T> value) where T : struct, ValueType, IComponentData
		{
			bool flag = this.HasSingleton<T>();
			value = (flag ? this.GetSingletonRW<T>() : default(RefRW<T>));
			return flag;
		}

		// Token: 0x06000C9F RID: 3231 RVA: 0x0003A72F File Offset: 0x0003892F
		public bool HasSingleton<T>()
		{
			return this.CalculateEntityCount() == 1;
		}

		// Token: 0x06000CA0 RID: 3232 RVA: 0x0003A73C File Offset: 0x0003893C
		public bool TryGetSingletonBuffer<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(out DynamicBuffer<T> value, bool isReadOnly = false) where T : struct, ValueType, IBufferElementData
		{
			bool flag = this.HasSingleton<T>();
			value = (flag ? this.GetSingletonBuffer<T>(isReadOnly) : default(DynamicBuffer<T>));
			return flag;
		}

		// Token: 0x06000CA1 RID: 3233 RVA: 0x0003A76C File Offset: 0x0003896C
		public bool TryGetSingletonEntity<T>(out Entity value)
		{
			bool flag = this.HasSingleton<T>();
			value = (flag ? this.GetSingletonEntity() : Entity.Null);
			return flag;
		}

		// Token: 0x06000CA2 RID: 3234 RVA: 0x0003A797 File Offset: 0x00038997
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleEnableableComponent) })]
		public unsafe void SetEnabledBitsOnAllChunks(TypeIndex typeIndex, bool value)
		{
			this._Access->DependencyManager->CompleteReadAndWriteDependency(typeIndex);
			ChunkIterationUtility.SetEnabledBitsOnAllChunks(ref this, typeIndex, value, this._Access->EntityComponentStore->GlobalSystemVersion);
		}

		// Token: 0x06000CA3 RID: 3235 RVA: 0x0003A7C2 File Offset: 0x000389C2
		public bool CompareQuery(in EntityQueryBuilder queryDesc)
		{
			return EntityQueryManager.CompareQuery(in queryDesc, this._QueryData);
		}

		// Token: 0x06000CA4 RID: 3236 RVA: 0x0003A7D0 File Offset: 0x000389D0
		[BurstDiscard]
		[BurstMonoInteropMethod(false)]
		internal unsafe static void _ResetFilter(EntityQueryImpl* self)
		{
			int count = self->_Filter.Shared.Count;
			for (int i = 0; i < count; i++)
			{
				self->_Access->RemoveSharedComponentReference(*((ref self->_Filter.Shared.SharedComponentIndex.FixedElementField) + (IntPtr)i * 4), 1);
			}
			self->_Filter.Changed.Count = 0;
			self->_Filter.Shared.Count = 0;
			self->_Filter.UseOrderFiltering = false;
		}

		// Token: 0x06000CA5 RID: 3237 RVA: 0x0003A850 File Offset: 0x00038A50
		[BurstDiscard]
		[BurstMonoInteropMethod(false)]
		internal unsafe static void _FreeCachedState(EntityQueryImpl* self)
		{
			IDisposable disposable = self->_CachedState.Target as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
			self->_CachedState.Free();
		}

		// Token: 0x06000CA6 RID: 3238 RVA: 0x0003A882 File Offset: 0x00038A82
		public void SetSharedComponentFilter<SharedComponent1>(SharedComponent1 sharedComponent1) where SharedComponent1 : struct, ISharedComponentData
		{
			this.ResetFilter();
			this.AddSharedComponentFilter<SharedComponent1>(sharedComponent1);
		}

		// Token: 0x06000CA7 RID: 3239 RVA: 0x0003A891 File Offset: 0x00038A91
		public void SetSharedComponentFilterUnmanaged<[global::System.Runtime.CompilerServices.IsUnmanaged] SharedComponent1>(SharedComponent1 sharedComponent1) where SharedComponent1 : struct, ValueType, ISharedComponentData
		{
			this.ResetFilter();
			this.AddSharedComponentFilterUnmanaged<SharedComponent1>(sharedComponent1);
		}

		// Token: 0x06000CA8 RID: 3240 RVA: 0x0003A8A0 File Offset: 0x00038AA0
		public unsafe void ResetFilter()
		{
			fixed (EntityQueryImpl* ptr = &this)
			{
				EntityQueryImpl.ResetFilter(ptr);
			}
		}

		// Token: 0x06000CA9 RID: 3241 RVA: 0x0003A8B9 File Offset: 0x00038AB9
		public void SetSharedComponentFilter<SharedComponent1, SharedComponent2>(SharedComponent1 sharedComponent1, SharedComponent2 sharedComponent2) where SharedComponent1 : struct, ISharedComponentData where SharedComponent2 : struct, ISharedComponentData
		{
			this.ResetFilter();
			this.AddSharedComponentFilter<SharedComponent1>(sharedComponent1);
			this.AddSharedComponentFilter<SharedComponent2>(sharedComponent2);
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x0003A8CF File Offset: 0x00038ACF
		public void SetSharedComponentFilterUnmanaged<[global::System.Runtime.CompilerServices.IsUnmanaged] SharedComponent1, [global::System.Runtime.CompilerServices.IsUnmanaged] SharedComponent2>(SharedComponent1 sharedComponent1, SharedComponent2 sharedComponent2) where SharedComponent1 : struct, ValueType, ISharedComponentData where SharedComponent2 : struct, ValueType, ISharedComponentData
		{
			this.ResetFilter();
			this.AddSharedComponentFilterUnmanaged<SharedComponent1>(sharedComponent1);
			this.AddSharedComponentFilterUnmanaged<SharedComponent2>(sharedComponent2);
		}

		// Token: 0x06000CAB RID: 3243 RVA: 0x0003A8E5 File Offset: 0x00038AE5
		public void SetChangedVersionFilter(ComponentType componentType)
		{
			this.ResetFilter();
			this.AddChangedVersionFilter(componentType);
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x0003A8F4 File Offset: 0x00038AF4
		public void SetOrderVersionFilter()
		{
			this.ResetFilter();
			this.AddOrderVersionFilter();
		}

		// Token: 0x06000CAD RID: 3245 RVA: 0x0003A902 File Offset: 0x00038B02
		internal void SetChangedFilterRequiredVersion(uint requiredVersion)
		{
			this._Filter.RequiredChangeVersion = requiredVersion;
		}

		// Token: 0x06000CAE RID: 3246 RVA: 0x0003A910 File Offset: 0x00038B10
		public void SetChangedVersionFilter(ComponentType[] componentType)
		{
			if (componentType.Length > 2)
			{
				throw new ArgumentException(string.Format("EntityQuery.SetFilterChanged accepts a maximum of {0} component array length", 2));
			}
			if (componentType.Length == 0)
			{
				throw new ArgumentException("EntityQuery.SetFilterChanged component array length must be larger than 0");
			}
			this.ResetFilter();
			for (int num = 0; num != componentType.Length; num++)
			{
				this.AddChangedVersionFilter(componentType[num]);
			}
		}

		// Token: 0x06000CAF RID: 3247 RVA: 0x0003A96C File Offset: 0x00038B6C
		public unsafe void AddChangedVersionFilter(ComponentType componentType)
		{
			int count = this._Filter.Changed.Count;
			if (count >= 2)
			{
				throw new ArgumentException(string.Format("EntityQuery accepts a maximum of {0} changed filters.", 2));
			}
			this._Filter.Changed.Count = count + 1;
			*((ref this._Filter.Changed.IndexInEntityQuery.FixedElementField) + (IntPtr)count * 4) = this.GetIndexInEntityQuery(componentType.TypeIndex);
		}

		// Token: 0x06000CB0 RID: 3248 RVA: 0x0003A9E0 File Offset: 0x00038BE0
		public unsafe void AddSharedComponentFilter<SharedComponent>(SharedComponent sharedComponent) where SharedComponent : struct, ISharedComponentData
		{
			int count = this._Filter.Shared.Count;
			if (count >= 2)
			{
				throw new ArgumentException(string.Format("EntityQuery accepts a maximum of {0} shared component filters.", 2));
			}
			this._Filter.Shared.Count = count + 1;
			*((ref this._Filter.Shared.IndexInEntityQuery.FixedElementField) + (IntPtr)count * 4) = this.GetIndexInEntityQuery(TypeManager.GetTypeIndex<SharedComponent>());
			*((ref this._Filter.Shared.SharedComponentIndex.FixedElementField) + (IntPtr)count * 4) = this._Access->InsertSharedComponent<SharedComponent>(sharedComponent);
		}

		// Token: 0x06000CB1 RID: 3249 RVA: 0x0003AA78 File Offset: 0x00038C78
		public unsafe void AddSharedComponentFilterUnmanaged<[global::System.Runtime.CompilerServices.IsUnmanaged] SharedComponent>(SharedComponent sharedComponent) where SharedComponent : struct, ValueType, ISharedComponentData
		{
			int count = this._Filter.Shared.Count;
			if (count >= 2)
			{
				throw new ArgumentException(string.Format("EntityQuery accepts a maximum of {0} shared component filters.", 2));
			}
			this._Filter.Shared.Count = count + 1;
			*((ref this._Filter.Shared.IndexInEntityQuery.FixedElementField) + (IntPtr)count * 4) = this.GetIndexInEntityQuery(TypeManager.GetTypeIndex<SharedComponent>());
			*((ref this._Filter.Shared.SharedComponentIndex.FixedElementField) + (IntPtr)count * 4) = this._Access->InsertSharedComponent_Unmanaged<SharedComponent>(sharedComponent);
		}

		// Token: 0x06000CB2 RID: 3250 RVA: 0x0003AB10 File Offset: 0x00038D10
		public void AddOrderVersionFilter()
		{
			this._Filter.UseOrderFiltering = true;
		}

		// Token: 0x06000CB3 RID: 3251 RVA: 0x0003AB1E File Offset: 0x00038D1E
		public unsafe void CompleteDependency()
		{
			this._Access->DependencyManager->CompleteDependenciesNoChecks(this._QueryData->ReaderTypes, this._QueryData->ReaderTypesCount, this._QueryData->WriterTypes, this._QueryData->WriterTypesCount);
		}

		// Token: 0x06000CB4 RID: 3252 RVA: 0x0003AB5C File Offset: 0x00038D5C
		public unsafe JobHandle GetDependency()
		{
			return this._Access->DependencyManager->GetDependency(this._QueryData->ReaderTypes, this._QueryData->ReaderTypesCount, this._QueryData->WriterTypes, this._QueryData->WriterTypesCount, false);
		}

		// Token: 0x06000CB5 RID: 3253 RVA: 0x0003AB9B File Offset: 0x00038D9B
		public unsafe JobHandle AddDependency(JobHandle job)
		{
			return this._Access->DependencyManager->AddDependency(this._QueryData->ReaderTypes, this._QueryData->ReaderTypesCount, this._QueryData->WriterTypes, this._QueryData->WriterTypesCount, job);
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x0003ABDC File Offset: 0x00038DDC
		public unsafe int GetCombinedComponentOrderVersion(bool includeEntityType)
		{
			int num = 0;
			for (int i = (includeEntityType ? 0 : 1); i < this._QueryData->RequiredComponentsCount; i++)
			{
				num += this._Access->EntityComponentStore->GetComponentTypeOrderVersion(this._QueryData->RequiredComponents[i].TypeIndex);
			}
			return num;
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x0003AC38 File Offset: 0x00038E38
		internal unsafe bool AddReaderWritersToLists(ref UnsafeList<TypeIndex> reading, ref UnsafeList<TypeIndex> writing)
		{
			bool flag = false;
			for (int i = 0; i < this._QueryData->ReaderTypesCount; i++)
			{
				flag |= CalculateReaderWriterDependency.AddReaderTypeIndex(this._QueryData->ReaderTypes[i], ref reading, ref writing);
			}
			for (int j = 0; j < this._QueryData->WriterTypesCount; j++)
			{
				flag |= CalculateReaderWriterDependency.AddWriterTypeIndex(this._QueryData->WriterTypes[j], ref reading, ref writing);
			}
			return flag;
		}

		// Token: 0x06000CB8 RID: 3256 RVA: 0x0003ACBC File Offset: 0x00038EBC
		private unsafe void SyncChangeFilterTypes()
		{
			for (int i = 0; i < this._Filter.Changed.Count; i++)
			{
				ComponentType componentType = this._QueryData->RequiredComponents[*((ref this._Filter.Changed.IndexInEntityQuery.FixedElementField) + (IntPtr)i * 4)];
				this._Access->DependencyManager->CompleteWriteDependency(componentType.TypeIndex);
			}
		}

		// Token: 0x06000CB9 RID: 3257 RVA: 0x0003AD30 File Offset: 0x00038F30
		private unsafe void SyncEnableableTypes()
		{
			TypeIndex* enableableComponentTypeIndices = this._QueryData->EnableableComponentTypeIndices;
			int enableableComponentTypeIndexCount = this._QueryData->EnableableComponentTypeIndexCount;
			for (int i = 0; i < enableableComponentTypeIndexCount; i++)
			{
				this._Access->DependencyManager->CompleteWriteDependency(enableableComponentTypeIndices[i]);
			}
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x0003AD81 File Offset: 0x00038F81
		public bool HasFilter()
		{
			return this._Filter.RequiresMatchesFilter;
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x0003AD90 File Offset: 0x00038F90
		public unsafe bool Debugger_GetData(List<Entity> entities, List<ArchetypeChunk> chunks)
		{
			if (this._QueryData == null)
			{
				return false;
			}
			if (this._QueryData->MatchingArchetypes.Length > 10240)
			{
				return false;
			}
			for (int i = 0; i < this._QueryData->MatchingArchetypes.Length; i++)
			{
				MatchingArchetype* ptr = *(IntPtr*)(this._QueryData->MatchingArchetypes.Ptr + (IntPtr)i * (IntPtr)sizeof(MatchingArchetype*) / (IntPtr)sizeof(MatchingArchetype*));
				Archetype* archetype = ptr->Archetype;
				if (archetype->EntityCount != 0)
				{
					int count = archetype->Chunks.Count;
					if (count > 10240)
					{
						return false;
					}
					for (int j = 0; j < count; j++)
					{
						ChunkIndex chunkIndex = archetype->Chunks[j];
						if (chunkIndex.MatchesFilter(ptr, ref this._Filter))
						{
							Entity* componentDataRO = (Entity*)ChunkDataUtility.GetComponentDataRO(chunkIndex, archetype, 0, 0);
							v128 v;
							ChunkIterationUtility.GetEnabledMask(chunkIndex, ptr, out v);
							if (chunkIndex.Count > 128)
							{
								return false;
							}
							if (EnabledBitUtility.countbits(v) != 0)
							{
								if (chunks != null)
								{
									ArchetypeChunk archetypeChunk = new ArchetypeChunk(chunkIndex, this._Access->EntityComponentStore);
									chunks.Add(archetypeChunk);
								}
								int num = 0;
								int num2;
								while (EnabledBitUtility.TryGetNextRange(v, num, out num2, out num))
								{
									if (entities != null)
									{
										for (int k = num2; k < num; k++)
										{
											entities.Add(componentDataRO[k]);
										}
									}
								}
							}
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x0003AEF0 File Offset: 0x000390F0
		public unsafe EntityQueryMask GetEntityQueryMask()
		{
			EntityComponentStore* entityComponentStore = this._Access->EntityComponentStore;
			return this._Access->EntityQueryManager->GetEntityQueryMask(this._QueryData, entityComponentStore);
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x0003AF20 File Offset: 0x00039120
		public unsafe bool Matches(Entity e)
		{
			EntityComponentStore* entityComponentStore = this._Access->EntityComponentStore;
			EntityQueryMask entityQueryMask = this._QueryData->EntityQueryMask;
			if (Hint.Unlikely(!entityQueryMask.IsCreated()))
			{
				entityQueryMask = this._Access->EntityQueryManager->GetEntityQueryMask(this._QueryData, entityComponentStore);
			}
			if (!entityQueryMask.MatchesIgnoreFilter(e))
			{
				return false;
			}
			bool flag = this.HasFilter();
			bool flag2 = this._QueryData->HasEnableableComponents > 0;
			if (flag || flag2)
			{
				ChunkIndex chunk = entityComponentStore->GetChunk(e);
				Archetype* archetype = entityComponentStore->GetArchetype(chunk);
				MatchingArchetype* ptr = *(IntPtr*)(this._QueryData->MatchingArchetypes.Ptr + (IntPtr)EntityQueryManager.FindMatchingArchetypeIndexForArchetype(ref this._QueryData->MatchingArchetypes, archetype) * (IntPtr)sizeof(MatchingArchetype*) / (IntPtr)sizeof(MatchingArchetype*));
				if (flag)
				{
					this.SyncChangeFilterTypes();
					if (!chunk.MatchesFilter(ptr, ref this._Filter))
					{
						return false;
					}
				}
				if (flag2)
				{
					this.SyncEnableableTypes();
					v128 v;
					ChunkIterationUtility.GetEnabledMask(chunk, ptr, out v);
					int indexInChunk = entityComponentStore->GetEntityInChunk(e).IndexInChunk;
					if (indexInChunk < 64)
					{
						if ((v.ULong0 & (1UL << indexInChunk)) == 0UL)
						{
							return false;
						}
					}
					else if ((v.ULong1 & (1UL << indexInChunk - 64)) == 0UL)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06000CBE RID: 3262 RVA: 0x0003B04C File Offset: 0x0003924C
		public unsafe bool MatchesIgnoreFilter(Entity e)
		{
			EntityComponentStore* entityComponentStore = this._Access->EntityComponentStore;
			EntityQueryMask entityQueryMask = this._QueryData->EntityQueryMask;
			if (Hint.Unlikely(!entityQueryMask.IsCreated()))
			{
				entityQueryMask = this._Access->EntityQueryManager->GetEntityQueryMask(this._QueryData, entityComponentStore);
			}
			return entityQueryMask.MatchesIgnoreFilter(e);
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x0003B0A4 File Offset: 0x000392A4
		internal unsafe EntityQueryDesc[] GetEntityQueryDescs()
		{
			EntityQueryDesc[] array = new EntityQueryDesc[this._QueryData->ArchetypeQueryCount];
			int i = 0;
			int archetypeQueryCount = this._QueryData->ArchetypeQueryCount;
			while (i < archetypeQueryCount)
			{
				ref ArchetypeQuery ptr = ref this._QueryData->ArchetypeQueries[i];
				ComponentType[] array2 = new ComponentType[ptr.AllCount];
				for (int j = 0; j < ptr.AllCount; j++)
				{
					array2[j] = new ComponentType
					{
						TypeIndex = ptr.All[j],
						AccessModeType = (ComponentType.AccessMode)ptr.AllAccessMode[j]
					};
				}
				ComponentType[] array3 = new ComponentType[ptr.AnyCount];
				for (int k = 0; k < ptr.AnyCount; k++)
				{
					array3[k] = new ComponentType
					{
						TypeIndex = ptr.Any[k],
						AccessModeType = (ComponentType.AccessMode)ptr.AnyAccessMode[k]
					};
				}
				ComponentType[] array4 = new ComponentType[ptr.NoneCount];
				for (int l = 0; l < ptr.NoneCount; l++)
				{
					array4[l] = new ComponentType
					{
						TypeIndex = ptr.None[l],
						AccessModeType = (ComponentType.AccessMode)ptr.NoneAccessMode[l]
					};
				}
				ComponentType[] array5 = new ComponentType[ptr.DisabledCount];
				for (int m = 0; m < ptr.DisabledCount; m++)
				{
					array5[m] = new ComponentType
					{
						TypeIndex = ptr.Disabled[m],
						AccessModeType = (ComponentType.AccessMode)ptr.DisabledAccessMode[m]
					};
				}
				ComponentType[] array6 = new ComponentType[ptr.AbsentCount];
				for (int n = 0; n < ptr.AbsentCount; n++)
				{
					array6[n] = new ComponentType
					{
						TypeIndex = ptr.Absent[n],
						AccessModeType = (ComponentType.AccessMode)ptr.AbsentAccessMode[n]
					};
				}
				ComponentType[] array7 = new ComponentType[ptr.PresentCount];
				for (int num = 0; num < ptr.PresentCount; num++)
				{
					array7[num] = new ComponentType
					{
						TypeIndex = ptr.Present[num],
						AccessModeType = (ComponentType.AccessMode)ptr.PresentAccessMode[num]
					};
				}
				array[i] = new EntityQueryDesc
				{
					All = array2,
					Any = array3,
					None = array4,
					Disabled = array5,
					Absent = array6,
					Present = array7,
					Options = ptr.Options
				};
				i++;
			}
			return array;
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x0003B38D File Offset: 0x0003958D
		internal EntityQueryDesc GetEntityQueryDesc()
		{
			return this.GetEntityQueryDescs()[0];
		}

		// Token: 0x06000CC1 RID: 3265 RVA: 0x0003B397 File Offset: 0x00039597
		internal unsafe UnsafeCachedChunkList GetMatchingChunkCache()
		{
			if (Hint.Unlikely(!this._QueryData->IsChunkCacheValid()))
			{
				this.UpdateMatchingChunkCache();
			}
			return this._QueryData->UnsafeGetMatchingChunkCache();
		}

		// Token: 0x06000CC2 RID: 3266 RVA: 0x0003B3BF File Offset: 0x000395BF
		internal unsafe void UpdateMatchingChunkCache()
		{
			this._QueryData->RebuildMatchingChunkCache();
		}

		// Token: 0x06000CC3 RID: 3267 RVA: 0x0003B3CC File Offset: 0x000395CC
		internal unsafe static EntityQueryImpl* Allocate()
		{
			void* ptr = Memory.Unmanaged.Allocate((long)sizeof(EntityQueryImpl), 8, Allocator.Persistent);
			UnsafeUtility.MemClear(ptr, (long)sizeof(EntityQueryImpl));
			return (EntityQueryImpl*)ptr;
		}

		// Token: 0x06000CC4 RID: 3268 RVA: 0x0003B3EE File Offset: 0x000395EE
		internal unsafe static void Free(EntityQueryImpl* impl)
		{
			Memory.Unmanaged.Free<EntityQueryImpl>(impl, Allocator.Persistent);
		}

		// Token: 0x06000CC5 RID: 3269 RVA: 0x00016E6B File Offset: 0x0001506B
		[BurstDiscard]
		private static void CheckDelegate(ref bool useDelegate)
		{
			useDelegate = true;
		}

		// Token: 0x06000CC6 RID: 3270 RVA: 0x0003B3FC File Offset: 0x000395FC
		private static bool UseDelegate()
		{
			bool flag = false;
			EntityQueryImpl.CheckDelegate(ref flag);
			return flag;
		}

		// Token: 0x06000CC7 RID: 3271 RVA: 0x0003B414 File Offset: 0x00039614
		[ExcludeFromBurstCompatTesting("Uses managed delegates")]
		internal unsafe static void Initialize()
		{
			if (EntityQueryImpl.Managed._initialized)
			{
				return;
			}
			EntityQueryImpl.Managed._initialized = true;
			EntityQueryImpl.Managed._dlg_ResetFilter dlg_ResetFilter = new EntityQueryImpl.Managed._dlg_ResetFilter(EntityQueryImpl._wrapper_ResetFilter);
			EntityQueryImpl.Managed._gcDefeat_ResetFilter = dlg_ResetFilter;
			*EntityQueryImpl._bfp_ResetFilter.Data = Marshal.GetFunctionPointerForDelegate<EntityQueryImpl.Managed._dlg_ResetFilter>(dlg_ResetFilter);
			EntityQueryImpl.Managed._dlg_FreeCachedState dlg_FreeCachedState = new EntityQueryImpl.Managed._dlg_FreeCachedState(EntityQueryImpl._wrapper_FreeCachedState);
			EntityQueryImpl.Managed._gcDefeat_FreeCachedState = dlg_FreeCachedState;
			*EntityQueryImpl._bfp_FreeCachedState.Data = Marshal.GetFunctionPointerForDelegate<EntityQueryImpl.Managed._dlg_FreeCachedState>(dlg_FreeCachedState);
		}

		// Token: 0x06000CC8 RID: 3272 RVA: 0x0003B478 File Offset: 0x00039678
		internal unsafe static void ResetFilter(EntityQueryImpl* self)
		{
			if (!EntityQueryImpl.UseDelegate())
			{
				FunctionPointer<EntityQueryImpl.Managed._dlg_ResetFilter> functionPointer = new FunctionPointer<EntityQueryImpl.Managed._dlg_ResetFilter>(*EntityQueryImpl._bfp_ResetFilter.Data);
				functionPointer.Invoke((IntPtr)((void*)self));
				return;
			}
			EntityQueryImpl._ResetFilter(self);
		}

		// Token: 0x06000CC9 RID: 3273 RVA: 0x0003B4B7 File Offset: 0x000396B7
		[MonoPInvokeCallback(typeof(EntityQueryImpl.Managed._dlg_ResetFilter))]
		private unsafe static void _wrapper_ResetFilter(IntPtr self)
		{
			EntityQueryImpl._ResetFilter((EntityQueryImpl*)(void*)self);
		}

		// Token: 0x06000CCA RID: 3274 RVA: 0x0003B4C4 File Offset: 0x000396C4
		internal unsafe static void FreeCachedState(EntityQueryImpl* self)
		{
			if (!EntityQueryImpl.UseDelegate())
			{
				FunctionPointer<EntityQueryImpl.Managed._dlg_FreeCachedState> functionPointer = new FunctionPointer<EntityQueryImpl.Managed._dlg_FreeCachedState>(*EntityQueryImpl._bfp_FreeCachedState.Data);
				functionPointer.Invoke((IntPtr)((void*)self));
				return;
			}
			EntityQueryImpl._FreeCachedState(self);
		}

		// Token: 0x06000CCB RID: 3275 RVA: 0x0003B503 File Offset: 0x00039703
		[MonoPInvokeCallback(typeof(EntityQueryImpl.Managed._dlg_FreeCachedState))]
		private unsafe static void _wrapper_FreeCachedState(IntPtr self)
		{
			EntityQueryImpl._FreeCachedState((EntityQueryImpl*)(void*)self);
		}

		// Token: 0x0400057B RID: 1403
		internal unsafe EntityDataAccess* _Access;

		// Token: 0x0400057C RID: 1404
		internal unsafe EntityQueryData* _QueryData;

		// Token: 0x0400057D RID: 1405
		internal EntityQueryFilter _Filter;

		// Token: 0x0400057E RID: 1406
		internal ulong _SeqNo;

		// Token: 0x0400057F RID: 1407
		internal GCHandle _CachedState;

		// Token: 0x04000580 RID: 1408
		public static readonly SharedStatic<IntPtr> _bfp_ResetFilter = SharedStatic<IntPtr>.GetOrCreateUnsafe(0U, 9112463149797675703L, 0L);

		// Token: 0x04000581 RID: 1409
		public static readonly SharedStatic<IntPtr> _bfp_FreeCachedState = SharedStatic<IntPtr>.GetOrCreateUnsafe(0U, 7257527534780448561L, 0L);

		// Token: 0x02000215 RID: 533
		private static class Managed
		{
			// Token: 0x04000582 RID: 1410
			public static bool _initialized;

			// Token: 0x04000583 RID: 1411
			public static object _gcDefeat_ResetFilter;

			// Token: 0x04000584 RID: 1412
			public static object _gcDefeat_FreeCachedState;

			// Token: 0x02000216 RID: 534
			// (Invoke) Token: 0x06000CCE RID: 3278
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate void _dlg_ResetFilter(IntPtr self);

			// Token: 0x02000217 RID: 535
			// (Invoke) Token: 0x06000CD2 RID: 3282
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate void _dlg_FreeCachedState(IntPtr self);
		}

		// Token: 0x02000218 RID: 536
		private struct TagType_ResetFilter
		{
		}

		// Token: 0x02000219 RID: 537
		private struct TagType_FreeCachedState
		{
		}
	}
}
