using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace Unity.Entities
{
	// Token: 0x02000168 RID: 360
	[GenerateTestsForBurstCompatibility]
	[BurstCompile]
	internal struct EntityDataAccess : IDisposable
	{
		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060007D0 RID: 2000 RVA: 0x0002A7B3 File Offset: 0x000289B3
		[ExcludeFromBurstCompatTesting("Managed data access")]
		internal ref ManagedEntityDataAccess ManagedEntityDataAccess
		{
			get
			{
				return Unity.Entities.ManagedEntityDataAccess.GetInstance(this.m_ManagedAccessHandle);
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060007D1 RID: 2001 RVA: 0x0002A7C0 File Offset: 0x000289C0
		[ExcludeFromBurstCompatTesting("Managed component store")]
		internal ManagedComponentStore ManagedComponentStore
		{
			get
			{
				return this.ManagedEntityDataAccess.ManagedComponentStore;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060007D2 RID: 2002 RVA: 0x0002A7D0 File Offset: 0x000289D0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal unsafe EntityComponentStore* EntityComponentStore
		{
			get
			{
				fixed (EntityComponentStore* ptr = &this.m_EntityComponentStore)
				{
					return ptr;
				}
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060007D3 RID: 2003 RVA: 0x0002A7E8 File Offset: 0x000289E8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal unsafe EntityQueryManager* EntityQueryManager
		{
			get
			{
				fixed (EntityQueryManager* ptr = &this.m_EntityQueryManager)
				{
					return ptr;
				}
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060007D4 RID: 2004 RVA: 0x0002A800 File Offset: 0x00028A00
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal unsafe ComponentDependencyManager* DependencyManager
		{
			get
			{
				fixed (ComponentDependencyManager* ptr = &this.m_DependencyManager)
				{
					return ptr;
				}
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060007D5 RID: 2005 RVA: 0x0002A818 File Offset: 0x00028A18
		internal unsafe ref UnsafeParallelHashMap<ulong, byte> AliveEntityQueries
		{
			get
			{
				fixed (UntypedUnsafeParallelHashMap* ptr = &this.m_AliveEntityQueries)
				{
					void* ptr2 = (void*)ptr;
					return UnsafeUtility.AsRef<UnsafeParallelHashMap<ulong, byte>>(ptr2);
				}
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060007D6 RID: 2006 RVA: 0x0002A835 File Offset: 0x00028A35
		internal bool IsInExclusiveTransaction
		{
			get
			{
				return this.m_DependencyManager.IsInTransaction;
			}
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x0002A844 File Offset: 0x00028A44
		[ExcludeFromBurstCompatTesting("Takes managed World")]
		public unsafe static void Initialize(EntityDataAccess* self, World world)
		{
			self->m_ManagedReferenceIndexList = new UnsafeList<int>(8, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			self->m_EntityAndSimulateOnlyArchetype = default(EntityArchetype);
			self->m_ManagedAccessHandle = -1;
			*self->AliveEntityQueries = new UnsafeParallelHashMap<ulong, byte>(32, Allocator.Persistent);
			self->m_DependencyManager.OnCreate(world.Unmanaged);
			Unity.Entities.EntityComponentStore.Create(&self->m_EntityComponentStore, world.SequenceNumber, 1024);
			Unity.Entities.EntityQueryManager.Create(&self->m_EntityQueryManager, &self->m_DependencyManager);
			self->m_WorldUnmanaged = world.Unmanaged;
			ManagedEntityDataAccess managedEntityDataAccess = new ManagedEntityDataAccess(world, new ManagedComponentStore(self->EntityComponentStore));
			self->m_ManagedAccessHandle = Unity.Entities.ManagedEntityDataAccess.AllocHandle(managedEntityDataAccess);
			self->m_EntityComponentStore.m_DebugOnlyManagedAccess = self->m_ManagedAccessHandle;
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp).WithOptions(EntityQueryOptions.IncludePrefab | EntityQueryOptions.IncludeDisabledEntities | EntityQueryOptions.IncludeSystems);
			self->m_UniversalQueryWithSystems = self->m_EntityQueryManager.CreateEntityQuery(self, entityQueryBuilder);
			entityQueryBuilder.Reset();
			entityQueryBuilder.WithOptions(EntityQueryOptions.IncludePrefab | EntityQueryOptions.IncludeDisabledEntities);
			self->m_UniversalQuery = self->m_EntityQueryManager.CreateEntityQuery(self, entityQueryBuilder);
			entityQueryBuilder.Reset();
			entityQueryBuilder.WithOptions(EntityQueryOptions.IncludePrefab | EntityQueryOptions.IncludeDisabledEntities | EntityQueryOptions.IncludeMetaChunks);
			self->m_UniversalQueryWithChunks = self->m_EntityQueryManager.CreateEntityQuery(self, entityQueryBuilder);
			entityQueryBuilder.Reset();
			entityQueryBuilder.WithOptions(EntityQueryOptions.IncludePrefab | EntityQueryOptions.IncludeDisabledEntities | EntityQueryOptions.IncludeSystems | EntityQueryOptions.IncludeMetaChunks);
			self->m_UniversalQueryWithChunksAndSystems = self->m_EntityQueryManager.CreateEntityQuery(self, entityQueryBuilder);
			entityQueryBuilder.Dispose();
			if (EntityDataAccess.s_DelegateGCPrevention == null)
			{
				EntityDataAccess.PlaybackManagedDelegate playbackManagedDelegate = new EntityDataAccess.PlaybackManagedDelegate(EntityDataAccess.PlaybackManagedDelegateInMonoWithWrappedExceptions);
				EntityDataAccess.s_DelegateGCPrevention = playbackManagedDelegate;
				*EntityDataAccess.s_ManagedPlaybackTrampoline.Data = Marshal.GetFunctionPointerForDelegate<EntityDataAccess.PlaybackManagedDelegate>(playbackManagedDelegate);
			}
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x0002A9D0 File Offset: 0x00028BD0
		[ExcludeFromBurstCompatTesting("calls managed playback")]
		[MonoPInvokeCallback(typeof(EntityDataAccess.PlaybackManagedDelegate))]
		private unsafe static void PlaybackManagedDelegateInMonoWithWrappedExceptions(IntPtr target)
		{
			try
			{
				((EntityDataAccess*)target.ToPointer())->PlaybackManagedChanges();
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x0002AA04 File Offset: 0x00028C04
		[ExcludeFromBurstCompatTesting("Disposes managed lists")]
		public unsafe void Dispose()
		{
			this.m_ManagedReferenceIndexList.Dispose();
			this.m_UniversalQueryWithSystems.Dispose();
			this.m_UniversalQueryWithChunks.Dispose();
			this.m_UniversalQuery.Dispose();
			this.m_UniversalQueryWithChunksAndSystems.Dispose();
			this.m_UniversalQueryWithSystems = default(EntityQuery);
			this.m_UniversalQueryWithChunks = default(EntityQuery);
			this.m_UniversalQuery = default(EntityQuery);
			this.m_UniversalQueryWithChunksAndSystems = default(EntityQuery);
			NativeArray<ulong> keyArray = this.AliveEntityQueries.GetKeyArray(Allocator.Temp);
			for (int i = 0; i < keyArray.Length; i++)
			{
				EntityQueryImpl* ptr = keyArray[i];
				ptr->Dispose();
				EntityQueryImpl.Free(ptr);
			}
			this.AliveEntityQueries.Dispose();
			*this.AliveEntityQueries = default(UnsafeParallelHashMap<ulong, byte>);
			this.m_DependencyManager.Dispose();
			Unity.Entities.EntityComponentStore.Destroy(this.EntityComponentStore);
			Unity.Entities.EntityQueryManager.Destroy(this.EntityQueryManager);
			this.ManagedEntityDataAccess.ManagedComponentStore.Dispose();
			Unity.Entities.ManagedEntityDataAccess.FreeHandle(this.m_ManagedAccessHandle);
			this.m_ManagedAccessHandle = -1;
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x0002AB0C File Offset: 0x00028D0C
		public unsafe bool Exists(Entity entity)
		{
			return this.EntityComponentStore->Exists(entity);
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x00002287 File Offset: 0x00000487
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void CheckIsStructuralChange()
		{
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x00002287 File Offset: 0x00000487
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void CheckIsAdditiveStructuralChange(UnsafePtrList<Archetype> archetypes)
		{
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x00002287 File Offset: 0x00000487
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private unsafe void CheckIsAdditiveStructuralChange(Archetype* archetype)
		{
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x0002AB1A File Offset: 0x00028D1A
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		internal void AssertMainThread()
		{
			if (this.IsInExclusiveTransaction)
			{
				throw new InvalidOperationException("Must be called from the main thread");
			}
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x0002AB2F File Offset: 0x00028D2F
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		internal void AssertQueryIsValid(EntityQuery query)
		{
			if (!this.IsQueryValid(query))
			{
				throw new ArgumentException("query is invalid or disposed");
			}
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x0002AB45 File Offset: 0x00028D45
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		internal unsafe void AssertQueryHasNoEnableableComponents(EntityQuery query)
		{
			if (query._GetImpl()->_QueryData->HasEnableableComponents != 0)
			{
				throw new ArgumentException("EntityQuery objects with types that implement IEnableableComponent are not currently supported by this operation.");
			}
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x0002AB65 File Offset: 0x00028D65
		public unsafe bool IsQueryValid(EntityQuery query)
		{
			return this.AliveEntityQueries.ContainsKey((ulong)(long)((IntPtr)((void*)query.__impl))) && query.__seqno == query.__impl->_SeqNo;
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x0002AB99 File Offset: 0x00028D99
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void CheckMoreThan1024Components(int count)
		{
			if (count + 1 > 1024)
			{
				throw new ArgumentException("Archetypes can't hold more than 1024 components");
			}
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x0002ABB0 File Offset: 0x00028DB0
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void AssertCountsMatch(int count, int outputCount)
		{
			if (count != outputCount)
			{
				throw new ArgumentException(string.Format("srcEntities.Length ({0}) and outputEntities.Length (({1})) must be the same.", count, outputCount));
			}
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x0002ABD2 File Offset: 0x00028DD2
		public void BeforeStructuralChange()
		{
			if (!this.m_DependencyManager.IsInTransaction)
			{
				this.m_DependencyManager.CompleteAllJobsAndInvalidateArrays();
			}
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x0002ABEC File Offset: 0x00028DEC
		public unsafe EntityComponentStore.ArchetypeChanges BeginStructuralChanges()
		{
			if (!this.IsInExclusiveTransaction)
			{
				this.BeforeStructuralChange();
			}
			this.m_ManagedReferenceIndexList.Clear();
			return this.EntityComponentStore->BeginArchetypeChangeTracking();
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x0002AC12 File Offset: 0x00028E12
		public void PrepareForAdditiveStructuralChanges()
		{
			this.CompleteForAdditiveStructuralChanges();
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x0002AC12 File Offset: 0x00028E12
		public unsafe void PrepareForAdditiveStructuralChanges(Archetype* archetype)
		{
			this.CompleteForAdditiveStructuralChanges();
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x0002AC1C File Offset: 0x00028E1C
		public void PrepareForInstantiateAdditiveStructuralChanges(NativeArray<Entity> entities)
		{
			foreach (Entity entity in entities)
			{
			}
			this.CompleteForAdditiveStructuralChanges();
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x0002AC6C File Offset: 0x00028E6C
		public void PrepareForCopyAdditiveStructuralChanges(NativeArray<Entity> entities)
		{
			foreach (Entity entity in entities)
			{
			}
			this.CompleteForAdditiveStructuralChanges();
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x0002ACBC File Offset: 0x00028EBC
		private unsafe void CompleteForAdditiveStructuralChanges()
		{
			if (this.IsInExclusiveTransaction)
			{
				return;
			}
			if (!this.m_DependencyManager.IsInTransaction)
			{
				this.DependencyManager->CompleteAllJobsAndCheckDeallocateAndThrow();
			}
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x0002ACDF File Offset: 0x00028EDF
		public unsafe EntityComponentStore.ArchetypeChanges BeginAdditiveStructuralChanges()
		{
			this.m_ManagedReferenceIndexList.Clear();
			return this.EntityComponentStore->BeginArchetypeChangeTracking();
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x0002ACF7 File Offset: 0x00028EF7
		public unsafe void CheckIsAdditiveArchetypeStructuralChangePossible(Archetype* archetype)
		{
			bool isInExclusiveTransaction = this.IsInExclusiveTransaction;
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x0002AD00 File Offset: 0x00028F00
		public unsafe void EndStructuralChanges(ref EntityComponentStore.ArchetypeChanges changes)
		{
			if (this.EntityComponentStore->ManagedChangesTracker.CommandBuffer.Length > 0)
			{
				this.PlaybackManagedChanges();
			}
			if (!this.m_ManagedReferenceIndexList.IsEmpty)
			{
				this.ResetManagedReferences();
			}
			if (this.EntityComponentStore->m_Archetypes.Length - changes.StartIndex != 0)
			{
				this.EntityComponentStore->EndArchetypeChangeTracking(changes, this.EntityQueryManager);
			}
			if (this.EntityComponentStore->m_ChunkListChangesTracker.ArchetypeTrackingHead != null)
			{
				this.EntityComponentStore->InvalidateChunkListCacheForChangedArchetypes();
			}
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x0002AD90 File Offset: 0x00028F90
		public unsafe void ResetManagedReferences()
		{
			fixed (EntityDataAccess* ptr = &this)
			{
				ECBInterop.RemoveManagedReferences(ptr, this.m_ManagedReferenceIndexList.Ptr, this.m_ManagedReferenceIndexList.Length);
			}
			this.m_ManagedReferenceIndexList.Clear();
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x0002ADCA File Offset: 0x00028FCA
		public unsafe void DestroyEntityDuringStructuralChange(NativeArray<Entity> entities, in SystemHandle originSystem = null)
		{
			if (entities.Length == 0)
			{
				return;
			}
			StructuralChange.DestroyEntity(this.EntityComponentStore, (Entity*)entities.GetUnsafePtr<Entity>(), entities.Length);
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x0002ADF0 File Offset: 0x00028FF0
		public unsafe void DestroyEntitiesInQueryDuringStructuralChange(EntityQueryImpl* queryImpl, in SystemHandle originSystem = null)
		{
			queryImpl->SyncFilterTypes();
			BufferTypeHandle<LinkedEntityGroup> bufferTypeHandle = this.GetBufferTypeHandle<LinkedEntityGroup>(true);
			StructuralChange.DestroyChunksQuery(this.EntityComponentStore, queryImpl, ref bufferTypeHandle);
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x0002AE19 File Offset: 0x00029019
		internal unsafe void DestroyEntityInternalDuringStructuralChange(Entity* entities, int count, in SystemHandle originSystem = null)
		{
			if (count == 0)
			{
				return;
			}
			StructuralChange.DestroyEntity(this.EntityComponentStore, entities, count);
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x0002AE2C File Offset: 0x0002902C
		internal unsafe EntityArchetype CreateArchetype(ComponentType* types, int count, bool addSimulateComponentIfMissing)
		{
			checked
			{
				ComponentTypeInArchetype* ptr = stackalloc ComponentTypeInArchetype[unchecked((UIntPtr)(count + 2)) * (UIntPtr)sizeof(ComponentTypeInArchetype)];
				int num = EntityDataAccess.FillSortedArchetypeArray(ptr, types, count, addSimulateComponentIfMissing);
				return this.CreateArchetype_Sorted(ptr, num);
			}
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x0002AE5C File Offset: 0x0002905C
		internal unsafe EntityArchetype CreateArchetype_Sorted(ComponentTypeInArchetype* typesInArchetype, int cachedComponentCount)
		{
			EntityArchetype entityArchetype;
			entityArchetype.Archetype = this.EntityComponentStore->GetExistingArchetype(typesInArchetype, cachedComponentCount);
			if (entityArchetype.Archetype != null)
			{
				return entityArchetype;
			}
			entityArchetype.Archetype = this.EntityComponentStore->GetOrCreateArchetype(typesInArchetype, cachedComponentCount);
			return entityArchetype;
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x0002AEA0 File Offset: 0x000290A0
		internal unsafe static int FillSortedArchetypeArray(ComponentTypeInArchetype* dst, ComponentType* requiredComponents, int count, bool addSimulateComponentIfMissing)
		{
			*dst = new ComponentTypeInArchetype(ComponentType.ReadWrite<Entity>());
			bool flag = false;
			for (int i = 0; i < count; i++)
			{
				flag |= requiredComponents[i] == ComponentType.ReadWrite<Simulate>();
				SortingUtilities.InsertSorted(dst, i + 1, requiredComponents[i]);
			}
			if (!flag && addSimulateComponentIfMissing)
			{
				SortingUtilities.InsertSorted(dst, count + 1, ComponentType.ReadWrite<Simulate>());
				return count + 2;
			}
			return count + 1;
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x0002AF20 File Offset: 0x00029120
		public unsafe Entity CreateEntity(in SystemHandle originSystem = null)
		{
			Entity entity;
			StructuralChange.CreateEntity(this.EntityComponentStore, (void*)this.GetEntityAndSimulateArchetype().Archetype, &entity, 1);
			return entity;
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x0002AF48 File Offset: 0x00029148
		public unsafe Entity CreateEntityDuringStructuralChange(EntityArchetype archetype, in SystemHandle originSystem = null)
		{
			Entity entity;
			this.CreateEntityDuringStructuralChange(archetype, &entity, 1, in originSystem);
			return entity;
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x0002AF62 File Offset: 0x00029162
		internal unsafe void CreateEntityDuringStructuralChange(EntityArchetype archetype, Entity* outEntities, int count, in SystemHandle originSystem = null)
		{
			StructuralChange.CreateEntity(this.EntityComponentStore, (void*)archetype.Archetype, outEntities, count);
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x0002AF77 File Offset: 0x00029177
		public unsafe void CreateEntityDuringStructuralChange(EntityArchetype archetype, NativeArray<Entity> entities, in SystemHandle originSystem = null)
		{
			this.CreateEntityDuringStructuralChange(archetype, (Entity*)entities.GetUnsafePtr<Entity>(), entities.Length, in originSystem);
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x0002AF8E File Offset: 0x0002918E
		public unsafe bool AddComponentDuringStructuralChange(Entity entity, ComponentType componentType, in SystemHandle originSystem = null)
		{
			return !this.HasComponent(entity, componentType) && StructuralChange.AddComponentEntity(this.EntityComponentStore, &entity, componentType.TypeIndex);
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x0002AFB0 File Offset: 0x000291B0
		public unsafe void AddMultipleComponentsDuringStructuralChange(Entity entity, in ComponentTypeSet typeSet, in SystemHandle originSystem = null)
		{
			StructuralChange.AddComponentsEntity(this.EntityComponentStore, &entity, in typeSet);
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x0002AFC1 File Offset: 0x000291C1
		public unsafe void AddComponentToQueryDuringStructuralChange(EntityQueryImpl* queryImpl, ComponentType componentType, in SystemHandle originSystem = null)
		{
			if (queryImpl->IsEmptyIgnoreFilter)
			{
				return;
			}
			queryImpl->SyncFilterTypes();
			StructuralChange.AddComponentQuery(this.EntityComponentStore, queryImpl, componentType.TypeIndex);
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x0002AFE4 File Offset: 0x000291E4
		internal unsafe void AddComponentsToQueryDuringStructuralChange(EntityQueryImpl* queryImpl, in ComponentTypeSet typeSet, in SystemHandle originSystem = null)
		{
			if (queryImpl->IsEmptyIgnoreFilter || typeSet.Length == 0)
			{
				return;
			}
			queryImpl->SyncFilterTypes();
			StructuralChange.AddComponentsQuery(this.EntityComponentStore, queryImpl, in typeSet);
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x0002B00C File Offset: 0x0002920C
		public unsafe void AddComponentDuringStructuralChange(NativeArray<Entity> entities, ComponentType componentType, in SystemHandle originSystem = null)
		{
			if (entities.Length == 0)
			{
				return;
			}
			NativeList<EntityBatchInChunk> nativeList;
			if (entities.Length > 10 && this.EntityComponentStore->CreateEntityBatchList(entities, componentType.IsSharedComponent ? 1 : 0, Allocator.Temp, out nativeList))
			{
				StructuralChange.AddComponentEntitiesBatch(this.EntityComponentStore, (UnsafeList<EntityBatchInChunk>*)NativeListUnsafeUtility.GetInternalListDataPtrUnchecked<EntityBatchInChunk>(ref nativeList), componentType.TypeIndex);
				return;
			}
			for (int i = 0; i < entities.Length; i++)
			{
				Entity entity = entities[i];
				StructuralChange.AddComponentEntity(this.EntityComponentStore, &entity, componentType.TypeIndex);
			}
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x0002B09C File Offset: 0x0002929C
		public unsafe void AddMultipleComponentsDuringStructuralChange(NativeArray<Entity> entities, in ComponentTypeSet componentTypeSet, in SystemHandle originSystem = null)
		{
			if (entities.Length == 0 || componentTypeSet.Length == 0)
			{
				return;
			}
			NativeList<EntityBatchInChunk> nativeList;
			if (componentTypeSet.ChunkComponentCount == 0 && entities.Length > 10 && this.EntityComponentStore->CreateEntityBatchList(entities, componentTypeSet.m_masks.SharedComponents, Allocator.Temp, out nativeList))
			{
				StructuralChange.AddComponentsEntitiesBatch(this.EntityComponentStore, (UnsafeList<EntityBatchInChunk>*)NativeListUnsafeUtility.GetInternalListDataPtrUnchecked<EntityBatchInChunk>(ref nativeList), in componentTypeSet);
				return;
			}
			for (int i = 0; i < entities.Length; i++)
			{
				Entity entity = entities[i];
				StructuralChange.AddComponentsEntity(this.EntityComponentStore, &entity, in componentTypeSet);
			}
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x0002B130 File Offset: 0x00029330
		public unsafe void RemoveMultipleComponentsDuringStructuralChange(NativeArray<Entity> entities, in ComponentTypeSet componentTypeSet, in SystemHandle originSystem = null)
		{
			if (entities.Length == 0 || componentTypeSet.Length == 0)
			{
				return;
			}
			NativeList<EntityBatchInChunk> nativeList;
			if (entities.Length > 10 && this.EntityComponentStore->CreateEntityBatchList(entities, 0, Allocator.Temp, out nativeList))
			{
				StructuralChange.RemoveComponentsEntitiesBatch(this.EntityComponentStore, (UnsafeList<EntityBatchInChunk>*)NativeListUnsafeUtility.GetInternalListDataPtrUnchecked<EntityBatchInChunk>(ref nativeList), in componentTypeSet);
				return;
			}
			for (int i = 0; i < entities.Length; i++)
			{
				Entity entity = entities[i];
				StructuralChange.RemoveComponentsEntity(this.EntityComponentStore, &entity, in componentTypeSet);
			}
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x0002B1AF File Offset: 0x000293AF
		public unsafe bool RemoveComponentDuringStructuralChange(Entity entity, ComponentType componentType, in SystemHandle originSystem = null)
		{
			return StructuralChange.RemoveComponentEntity(this.EntityComponentStore, &entity, componentType.TypeIndex);
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x0002B1C5 File Offset: 0x000293C5
		public unsafe void RemoveComponentDuringStructuralChange(Entity entity, in ComponentTypeSet componentTypeSet, in SystemHandle originSystem = null)
		{
			StructuralChange.RemoveComponentsEntity(this.EntityComponentStore, &entity, in componentTypeSet);
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x0002B1D6 File Offset: 0x000293D6
		public unsafe void RemoveComponentFromQueryDuringStructuralChange(EntityQueryImpl* queryImpl, ComponentType componentType, in SystemHandle originSystem = null)
		{
			if (queryImpl->IsEmptyIgnoreFilter)
			{
				return;
			}
			queryImpl->SyncFilterTypes();
			StructuralChange.RemoveComponentQuery(this.EntityComponentStore, queryImpl, componentType.TypeIndex);
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x0002B1F9 File Offset: 0x000293F9
		internal unsafe void RemoveMultipleComponentsFromQueryDuringStructuralChange(EntityQueryImpl* queryImpl, in ComponentTypeSet componentTypeSet, in SystemHandle originSystem = null)
		{
			if (queryImpl->IsEmptyIgnoreFilter || componentTypeSet.Length == 0)
			{
				return;
			}
			queryImpl->SyncFilterTypes();
			StructuralChange.RemoveComponentsQuery(this.EntityComponentStore, queryImpl, in componentTypeSet);
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x0002B21F File Offset: 0x0002941F
		internal unsafe void RemoveComponentDuringStructuralChange(NativeArray<ArchetypeChunk> chunks, ComponentType componentType, in SystemHandle originSystem = null)
		{
			if (chunks.Length == 0)
			{
				return;
			}
			StructuralChange.RemoveComponentChunks(this.EntityComponentStore, (ArchetypeChunk*)chunks.GetUnsafePtr<ArchetypeChunk>(), chunks.Length, componentType.TypeIndex);
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x0002B24C File Offset: 0x0002944C
		public unsafe void RemoveComponentDuringStructuralChange(NativeArray<Entity> entities, ComponentType componentType, in SystemHandle originSystem = null)
		{
			if (entities.Length == 0)
			{
				return;
			}
			NativeList<EntityBatchInChunk> nativeList;
			if (entities.Length > 10 && this.EntityComponentStore->CreateEntityBatchList(entities, 0, Allocator.Temp, out nativeList))
			{
				StructuralChange.RemoveComponentEntitiesBatch(this.EntityComponentStore, (UnsafeList<EntityBatchInChunk>*)NativeListUnsafeUtility.GetInternalListDataPtrUnchecked<EntityBatchInChunk>(ref nativeList), componentType.TypeIndex);
				return;
			}
			for (int i = 0; i < entities.Length; i++)
			{
				Entity entity = entities[i];
				StructuralChange.RemoveComponentEntity(this.EntityComponentStore, &entity, componentType.TypeIndex);
			}
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x0002B2D0 File Offset: 0x000294D0
		public unsafe bool HasComponent(Entity entity, ComponentType type)
		{
			bool flag;
			return this.EntityComponentStore->HasComponent(entity, type, out flag);
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x0002B2EC File Offset: 0x000294EC
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe T GetComponentData<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity) where T : struct, ValueType, IComponentData
		{
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			if (!this.IsInExclusiveTransaction)
			{
				this.DependencyManager->CompleteWriteDependency(typeIndex);
			}
			T t;
			UnsafeUtility.CopyPtrToStructure<T>((void*)this.EntityComponentStore->GetComponentDataWithTypeRO(entity, typeIndex), out t);
			return t;
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x0002B328 File Offset: 0x00029528
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe byte* GetComponentDataRW_AsBytePointer(Entity entity, TypeIndex typeIndex)
		{
			if (!this.IsInExclusiveTransaction)
			{
				this.DependencyManager->CompleteReadAndWriteDependency(typeIndex);
			}
			return this.EntityComponentStore->GetComponentDataWithTypeRW(entity, typeIndex, this.EntityComponentStore->GlobalSystemVersion);
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x0002B356 File Offset: 0x00029556
		public unsafe void* GetComponentDataRawRW(Entity entity, TypeIndex typeIndex)
		{
			return this.EntityComponentStore->GetComponentDataRawRW(entity, typeIndex);
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x0002B365 File Offset: 0x00029565
		internal unsafe void* GetComponentDataRawRWEntityHasComponent(Entity entity, TypeIndex typeIndex)
		{
			return this.EntityComponentStore->GetComponentDataRawRWEntityHasComponent(entity, typeIndex);
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x0002B374 File Offset: 0x00029574
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe void SetComponentData<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity, T componentData, in SystemHandle originSystem = null) where T : struct, ValueType, IComponentData
		{
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			if (!this.IsInExclusiveTransaction)
			{
				this.DependencyManager->CompleteReadAndWriteDependency(typeIndex);
			}
			byte* componentDataWithTypeRW = this.EntityComponentStore->GetComponentDataWithTypeRW(entity, typeIndex, this.EntityComponentStore->GlobalSystemVersion);
			UnsafeUtility.CopyStructureToPtr<T>(ref componentData, (void*)componentDataWithTypeRW);
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x0002B3BC File Offset: 0x000295BC
		public unsafe void SetComponentDataRaw(Entity entity, TypeIndex typeIndex, void* data, int size, in SystemHandle originSystem = null)
		{
			this.EntityComponentStore->SetComponentDataRawEntityHasComponent(entity, typeIndex, data, size);
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x0002B3CE File Offset: 0x000295CE
		public unsafe bool IsComponentEnabled(Entity entity, TypeIndex typeIndex)
		{
			return this.EntityComponentStore->IsComponentEnabled(entity, typeIndex);
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x0002B3DD File Offset: 0x000295DD
		public unsafe bool IsComponentEnabled(Entity entity, TypeIndex typeIndex, ref LookupCache typeLookupCache)
		{
			return this.EntityComponentStore->IsComponentEnabled(entity, typeIndex, ref typeLookupCache);
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x0002B3ED File Offset: 0x000295ED
		public unsafe void SetComponentEnabled(Entity entity, TypeIndex typeIndex, bool value)
		{
			this.EntityComponentStore->SetComponentEnabled(entity, typeIndex, value);
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x0002B3FD File Offset: 0x000295FD
		public unsafe void SetComponentEnabled(Entity entity, TypeIndex typeIndex, bool value, ref LookupCache typeLookupCache)
		{
			this.EntityComponentStore->SetComponentEnabled(entity, typeIndex, value, ref typeLookupCache);
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x0002B40F File Offset: 0x0002960F
		public bool IsEnabled(Entity entity)
		{
			return !this.HasComponent(entity, ComponentType.ReadWrite<Disabled>());
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x0002B420 File Offset: 0x00029620
		public void SetEnabled(Entity entity, bool value)
		{
			if (this.IsEnabled(entity) == value)
			{
				return;
			}
			ComponentType componentType = ComponentType.ReadWrite<Disabled>();
			SystemHandle systemHandle;
			if (this.HasComponent(entity, ComponentType.ReadWrite<LinkedEntityGroup>()))
			{
				TypeManager.GetTypeIndex<LinkedEntityGroup>();
				NativeArray<Entity> nativeArray = this.GetBuffer<LinkedEntityGroup>(entity, false).Reinterpret<Entity>().ToNativeArray(Allocator.TempJob);
				if (value)
				{
					NativeArray<Entity> nativeArray2 = nativeArray;
					ComponentType componentType2 = componentType;
					systemHandle = default(SystemHandle);
					this.RemoveComponentDuringStructuralChange(nativeArray2, componentType2, in systemHandle);
				}
				else
				{
					NativeArray<Entity> nativeArray3 = nativeArray;
					ComponentType componentType3 = componentType;
					systemHandle = default(SystemHandle);
					this.AddComponentDuringStructuralChange(nativeArray3, componentType3, in systemHandle);
				}
				nativeArray.Dispose();
				return;
			}
			if (!value)
			{
				ComponentType componentType4 = componentType;
				systemHandle = default(SystemHandle);
				this.AddComponentDuringStructuralChange(entity, componentType4, in systemHandle);
				return;
			}
			ComponentType componentType5 = componentType;
			systemHandle = default(SystemHandle);
			this.RemoveComponentDuringStructuralChange(entity, componentType5, in systemHandle);
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x0002B4C8 File Offset: 0x000296C8
		public unsafe void AddComponentForLinkedEntityGroup(Entity entity, EntityQueryMask mask, TypeIndex typeIndex, void* data, int componentSize)
		{
			if (!this.HasComponent(entity, ComponentType.ReadWrite<LinkedEntityGroup>()))
			{
				return;
			}
			TypeManager.GetTypeIndex<LinkedEntityGroup>();
			using (NativeArray<Entity> nativeArray = this.GetBuffer<LinkedEntityGroup>(entity, false).Reinterpret<Entity>().ToNativeArray(this.m_WorldUnmanaged.UpdateAllocator.ToAllocator))
			{
				foreach (Entity entity2 in nativeArray)
				{
					if (mask.MatchesIgnoreFilter(entity2))
					{
						Entity entity3 = entity2;
						ComponentType componentType = ComponentType.FromTypeIndex(typeIndex);
						SystemHandle systemHandle = default(SystemHandle);
						this.AddComponentDuringStructuralChange(entity3, componentType, in systemHandle);
						if (componentSize > 0)
						{
							Entity entity4 = entity2;
							systemHandle = default(SystemHandle);
							this.SetComponentDataRaw(entity4, typeIndex, data, componentSize, in systemHandle);
						}
					}
				}
			}
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x0002B5AC File Offset: 0x000297AC
		public unsafe void SetComponentForLinkedEntityGroup(Entity entity, EntityQueryMask mask, TypeIndex typeIndex, void* data, int componentSize)
		{
			if (!this.HasComponent(entity, ComponentType.ReadWrite<LinkedEntityGroup>()))
			{
				return;
			}
			TypeManager.GetTypeIndex<LinkedEntityGroup>();
			using (NativeArray<Entity> nativeArray = this.GetBuffer<LinkedEntityGroup>(entity, false).Reinterpret<Entity>().ToNativeArray(this.m_WorldUnmanaged.UpdateAllocator.ToAllocator))
			{
				foreach (Entity entity2 in nativeArray)
				{
					if (mask.MatchesIgnoreFilter(entity2))
					{
						Entity entity3 = entity2;
						SystemHandle systemHandle = default(SystemHandle);
						this.SetComponentDataRaw(entity3, typeIndex, data, componentSize, in systemHandle);
					}
				}
			}
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x0002B674 File Offset: 0x00029874
		public unsafe void ReplaceComponentForLinkedEntityGroup(Entity entity, TypeIndex typeIndex, void* data, int componentSize)
		{
			if (!this.HasComponent(entity, ComponentType.ReadWrite<LinkedEntityGroup>()))
			{
				return;
			}
			TypeManager.GetTypeIndex<LinkedEntityGroup>();
			using (NativeArray<Entity> nativeArray = this.GetBuffer<LinkedEntityGroup>(entity, false).Reinterpret<Entity>().ToNativeArray(this.m_WorldUnmanaged.UpdateAllocator.ToAllocator))
			{
				foreach (Entity entity2 in nativeArray)
				{
					bool flag;
					if (this.EntityComponentStore->HasComponent(entity2, typeIndex, out flag))
					{
						Entity entity3 = entity2;
						SystemHandle systemHandle = default(SystemHandle);
						this.SetComponentDataRaw(entity3, typeIndex, data, componentSize, in systemHandle);
					}
				}
			}
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x0002B744 File Offset: 0x00029944
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public bool AddSharedComponentDataDuringStructuralChange_Managed<T>(Entity entity, T componentData, in SystemHandle originSystem = null) where T : struct, ISharedComponentData
		{
			bool flag = this.AddComponentDuringStructuralChange(entity, ComponentType.ReadWrite<T>(), in originSystem);
			this.SetSharedComponentData_Managed<T>(entity, componentData, in originSystem);
			return flag;
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x0002B75C File Offset: 0x0002995C
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public unsafe void AddSharedComponentDataDuringStructuralChange_Managed<T>(NativeArray<Entity> entities, T componentData, in SystemHandle originSystem = null) where T : struct, ISharedComponentData
		{
			ComponentType componentType = ComponentType.FromTypeIndex(TypeManager.GetTypeIndex<T>());
			int num = this.InsertSharedComponent<T>(componentData);
			StructuralChange.AddSharedComponentDataIndexWithBurst(this.EntityComponentStore, (Entity*)entities.GetUnsafeReadOnlyPtr<Entity>(), entities.Length, in componentType, num);
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x0002B797 File Offset: 0x00029997
		internal unsafe void AddSharedComponentDataDuringStructuralChange(NativeArray<ArchetypeChunk> chunks, int sharedComponentIndex, ComponentType componentType, in SystemHandle originSystem = null)
		{
			if (chunks.Length == 0)
			{
				return;
			}
			StructuralChange.AddSharedComponentChunks(this.EntityComponentStore, (ArchetypeChunk*)chunks.GetUnsafePtr<ArchetypeChunk>(), chunks.Length, componentType.TypeIndex, sharedComponentIndex);
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x0002B7C2 File Offset: 0x000299C2
		internal unsafe void AddSharedComponentDataToQueryDuringStructuralChange(EntityQueryImpl* queryImpl, int sharedComponentIndex, ComponentType componentType, in SystemHandle originSystem = null)
		{
			if (queryImpl->IsEmptyIgnoreFilter)
			{
				return;
			}
			queryImpl->SyncFilterTypes();
			StructuralChange.AddSharedComponentQuery(this.EntityComponentStore, queryImpl, componentType.TypeIndex, sharedComponentIndex);
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x0002B7C2 File Offset: 0x000299C2
		internal unsafe void AddSharedComponentDataToQueryDuringStructuralChange_Unmanaged(EntityQueryImpl* queryImpl, int sharedComponentIndex, ComponentType componentType, void* componentValue, in SystemHandle originSystem = null)
		{
			if (queryImpl->IsEmptyIgnoreFilter)
			{
				return;
			}
			queryImpl->SyncFilterTypes();
			StructuralChange.AddSharedComponentQuery(this.EntityComponentStore, queryImpl, componentType.TypeIndex, sharedComponentIndex);
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x0002B7E6 File Offset: 0x000299E6
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public void AddSharedComponentDataBoxedDefaultMustBeNullDuringStructuralChange(NativeArray<Entity> entities, TypeIndex typeIndex, int hashCode, object componentData, in SystemHandle originSystem = null)
		{
			if (entities.Length == 0)
			{
				return;
			}
			this.AddComponentDuringStructuralChange(entities, ComponentType.FromTypeIndex(typeIndex), in originSystem);
			this.SetSharedComponentDataBoxedDefaultMustBeNullDuringStructuralChange(entities, typeIndex, hashCode, componentData, in originSystem);
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x0002B810 File Offset: 0x00029A10
		[GenerateTestsForBurstCompatibility]
		public unsafe void AddSharedComponentDataAddrDefaultMustBeNullDuringStructuralChange(NativeArray<Entity> entities, TypeIndex typeIndex, int hashCode, void* componentDataAddr)
		{
			ComponentType componentType = ComponentType.FromTypeIndex(typeIndex);
			SystemHandle systemHandle = default(SystemHandle);
			this.AddComponentDuringStructuralChange(entities, componentType, in systemHandle);
			systemHandle = default(SystemHandle);
			this.SetSharedComponentDataAddrDefaultMustBeNullDuringStructuralChange(entities, typeIndex, hashCode, componentDataAddr, in systemHandle);
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x0002B847 File Offset: 0x00029A47
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public bool AddSharedComponentDataBoxedDefaultMustBeNullDuringStructuralChange(Entity entity, TypeIndex typeIndex, int hashCode, object componentData, in SystemHandle originSystem = null)
		{
			bool flag = this.AddComponentDuringStructuralChange(entity, ComponentType.FromTypeIndex(typeIndex), in originSystem);
			this.SetSharedComponentDataBoxedDefaultMustBeNullDuringStructuralChange(entity, typeIndex, hashCode, componentData, in originSystem);
			return flag;
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x0002B868 File Offset: 0x00029A68
		[GenerateTestsForBurstCompatibility]
		public unsafe bool AddSharedComponentDataDuringStructuralChange_Unmanaged(Entity entity, ComponentType componentType, void* componentData, void* defaultComponentData)
		{
			SystemHandle systemHandle = default(SystemHandle);
			bool flag = this.AddComponentDuringStructuralChange(entity, componentType, in systemHandle);
			TypeIndex typeIndex = componentType.TypeIndex;
			systemHandle = default(SystemHandle);
			this.SetSharedComponentData_Unmanaged(entity, typeIndex, componentData, defaultComponentData, in systemHandle);
			return flag;
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x0002B8A0 File Offset: 0x00029AA0
		[GenerateTestsForBurstCompatibility]
		public unsafe void AddSharedComponentDataDuringStructuralChange_Unmanaged(NativeArray<Entity> entities, ComponentType componentType, void* componentData, void* defaultComponentData, in SystemHandle originSystem = null)
		{
			int num = this.InsertSharedComponent_Unmanaged(componentType.TypeIndex, 0, componentData, defaultComponentData);
			StructuralChange.AddSharedComponentDataIndexWithBurst(this.EntityComponentStore, (Entity*)entities.GetUnsafeReadOnlyPtr<Entity>(), entities.Length, in componentType, num);
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x0002B8D9 File Offset: 0x00029AD9
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleSharedComponentData) })]
		public unsafe void GetAllUniqueSharedComponents_Unmanaged<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(out UnsafeList<T> sharedComponentValues, AllocatorManager.AllocatorHandle allocator) where T : struct, ValueType, ISharedComponentData
		{
			this.EntityComponentStore->GetAllUniqueSharedComponents_Unmanaged<T>(out sharedComponentValues, allocator);
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x0002B8E8 File Offset: 0x00029AE8
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public unsafe object GetSharedComponentDataNonDefaultBoxed(int shareComponentIndex)
		{
			if (Unity.Entities.EntityComponentStore.IsUnmanagedSharedComponentIndex(shareComponentIndex))
			{
				TypeIndex componentTypeFromSharedComponentIndex = Unity.Entities.EntityComponentStore.GetComponentTypeFromSharedComponentIndex(shareComponentIndex);
				return this.EntityComponentStore->GetSharedComponentDataObject_Unmanaged(shareComponentIndex, componentTypeFromSharedComponentIndex);
			}
			return this.ManagedComponentStore.GetSharedComponentDataNonDefaultBoxed(shareComponentIndex);
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x0002B91E File Offset: 0x00029B1E
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public unsafe object GetSharedComponentDataBoxed(int shareComponentIndex, TypeIndex typeIndex)
		{
			if (Unity.Entities.EntityComponentStore.IsUnmanagedSharedComponentIndex(shareComponentIndex))
			{
				return this.EntityComponentStore->GetSharedComponentDataObject_Unmanaged(shareComponentIndex, typeIndex);
			}
			return this.ManagedComponentStore.GetSharedComponentDataBoxed(shareComponentIndex, typeIndex);
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x0002B944 File Offset: 0x00029B44
		public unsafe JobHandle GetCreatedAndDestroyedEntitiesAsync(NativeList<int> state, NativeList<Entity> createdEntities, NativeList<Entity> destroyedEntities)
		{
			JobHandle createdAndDestroyedEntities = Unity.Entities.EntityComponentStore.GetCreatedAndDestroyedEntities(this.EntityComponentStore, state, createdEntities, destroyedEntities, true);
			this.DependencyManager->AddDependency(null, 0, null, 0, createdAndDestroyedEntities);
			return createdAndDestroyedEntities;
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x0002B975 File Offset: 0x00029B75
		public void GetCreatedAndDestroyedEntities(NativeList<int> state, NativeList<Entity> createdEntities, NativeList<Entity> destroyedEntities)
		{
			Unity.Entities.EntityComponentStore.GetCreatedAndDestroyedEntities(this.EntityComponentStore, state, createdEntities, destroyedEntities, false);
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x0002B988 File Offset: 0x00029B88
		[ExcludeFromBurstCompatTesting("Potentially accesses managed component store")]
		public unsafe T GetSharedComponentData<T>(Entity entity) where T : struct, ISharedComponentData
		{
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			int sharedComponentDataIndex = this.EntityComponentStore->GetSharedComponentDataIndex(entity, typeIndex);
			return this.GetSharedComponentData<T>(sharedComponentDataIndex);
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x0002B9B0 File Offset: 0x00029BB0
		[ExcludeFromBurstCompatTesting("Potentially accesses managed component store")]
		public unsafe void SetSharedComponentData_Managed<T>(Entity entity, T componentData, in SystemHandle originSystem = null) where T : struct, ISharedComponentData
		{
			ComponentType componentType = ComponentType.FromTypeIndex(TypeManager.GetTypeIndex<T>());
			int num = this.InsertSharedComponent<T>(componentData);
			this.EntityComponentStore->SetSharedComponentDataIndex(entity, componentType, num);
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x0002B9E0 File Offset: 0x00029BE0
		[ExcludeFromBurstCompatTesting("Potentially accesses managed component store")]
		public unsafe void SetSharedComponentData_Managed<T>(NativeArray<Entity> entities, T componentData, in SystemHandle originSystem = null) where T : struct, ISharedComponentData
		{
			ComponentType componentType = ComponentType.FromTypeIndex(TypeManager.GetTypeIndex<T>());
			int num = this.InsertSharedComponent<T>(componentData);
			StructuralChange.SetSharedComponentDataIndexWithBurst(this.EntityComponentStore, (Entity*)entities.GetUnsafeReadOnlyPtr<Entity>(), entities.Length, in componentType, num);
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x0002BA1C File Offset: 0x00029C1C
		[ExcludeFromBurstCompatTesting("Potentially accesses managed component store")]
		public unsafe void SetSharedComponentData_Managed<T>(ArchetypeChunk chunk, T componentData, in SystemHandle originSystem = null) where T : struct, ISharedComponentData
		{
			ComponentType componentType = ComponentType.FromTypeIndex(TypeManager.GetTypeIndex<T>());
			int num = this.InsertSharedComponent<T>(componentData);
			this.EntityComponentStore->SetSharedComponentDataIndexForChunk(chunk.m_Chunk, chunk.Archetype.Archetype, componentType, num);
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x0002BA5C File Offset: 0x00029C5C
		[ExcludeFromBurstCompatTesting("Potentially accesses managed component store")]
		public unsafe void SetSharedComponentDataBoxedDefaultMustBeNullDuringStructuralChange(Entity entity, TypeIndex typeIndex, int hashCode, object componentData, in SystemHandle originSystem = null)
		{
			int num = 0;
			bool flag = TypeManager.IsManagedSharedComponent(typeIndex);
			if (componentData != null)
			{
				if (flag)
				{
					num = this.ManagedComponentStore.InsertSharedComponentAssumeNonDefault(typeIndex, hashCode, componentData);
				}
				else
				{
					ulong num2;
					byte* ptr = (byte*)UnsafeUtility.PinGCObjectAndGetAddress(componentData, out num2) + TypeManager.ObjectOffset;
					num = this.EntityComponentStore->InsertSharedComponent_Unmanaged(typeIndex, hashCode, (void*)ptr, null);
					UnsafeUtility.ReleaseGCObject(num2);
				}
			}
			ComponentType componentType = ComponentType.FromTypeIndex(typeIndex);
			EntityDataAccess.SetSharedComponentDataIndexWithBurst(this.EntityComponentStore, in entity, in componentType, num);
			this.m_ManagedReferenceIndexList.Add(in num);
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x0002BAD7 File Offset: 0x00029CD7
		[BurstCompile]
		private unsafe static void SetSharedComponentDataIndexWithBurst(EntityComponentStore* ecs, in Entity entity, in ComponentType componentType, int newSharedComponentDataIndex)
		{
			EntityDataAccess.SetSharedComponentDataIndexWithBurst_0000076F$BurstDirectCall.Invoke(ecs, in entity, in componentType, newSharedComponentDataIndex);
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x0002BAE4 File Offset: 0x00029CE4
		[ExcludeFromBurstCompatTesting("Potentially accesses managed component store")]
		public unsafe void SetSharedComponentDataBoxedDefaultMustBeNullDuringStructuralChange(NativeArray<Entity> entities, TypeIndex typeIndex, int hashCode, object componentData, in SystemHandle originSystem = null)
		{
			if (entities.Length == 0)
			{
				return;
			}
			ComponentType componentType = ComponentType.FromTypeIndex(typeIndex);
			int num = 0;
			if (componentData != null)
			{
				num = this.ManagedComponentStore.InsertSharedComponentAssumeNonDefault(typeIndex, hashCode, componentData);
			}
			for (int i = 0; i < entities.Length; i++)
			{
				EntityComponentStore* entityComponentStore = this.EntityComponentStore;
				Entity entity = entities[i];
				EntityDataAccess.SetSharedComponentDataIndexWithBurst(entityComponentStore, in entity, in componentType, num);
			}
			this.m_ManagedReferenceIndexList.Add(in num);
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x0002BB50 File Offset: 0x00029D50
		[GenerateTestsForBurstCompatibility]
		public unsafe void SetSharedComponentDataAddrDefaultMustBeNullDuringStructuralChange(Entity entity, TypeIndex typeIndex, int hashCode, void* componentDataAddr)
		{
			ComponentType componentType = ComponentType.FromTypeIndex(typeIndex);
			int num = 0;
			if (componentDataAddr != null)
			{
				num = this.EntityComponentStore->InsertSharedComponent_Unmanaged(typeIndex, hashCode, componentDataAddr, null);
			}
			EntityDataAccess.SetSharedComponentDataIndexWithBurst(this.EntityComponentStore, in entity, in componentType, num);
			this.m_ManagedReferenceIndexList.Add(in num);
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x0002BB9C File Offset: 0x00029D9C
		[GenerateTestsForBurstCompatibility]
		public unsafe void SetSharedComponentDataAddrDefaultMustBeNullDuringStructuralChange(NativeArray<Entity> entities, TypeIndex typeIndex, int hashCode, void* componentDataAddr, in SystemHandle originSystem = null)
		{
			ComponentType componentType = ComponentType.FromTypeIndex(typeIndex);
			int num = 0;
			if (componentDataAddr != null)
			{
				num = this.EntityComponentStore->InsertSharedComponent_Unmanaged(typeIndex, hashCode, componentDataAddr, null);
			}
			for (int i = 0; i < entities.Length; i++)
			{
				EntityComponentStore* entityComponentStore = this.EntityComponentStore;
				Entity entity = entities[i];
				EntityDataAccess.SetSharedComponentDataIndexWithBurst(entityComponentStore, in entity, in componentType, num);
			}
			this.m_ManagedReferenceIndexList.Add(in num);
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x0002BC04 File Offset: 0x00029E04
		[ExcludeFromBurstCompatTesting("Takes managed list")]
		public unsafe void GetAllUniqueSharedComponents<T>(List<T> sharedComponentValues) where T : struct, ISharedComponentData
		{
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			if (TypeManager.IsManagedSharedComponent(typeIndex))
			{
				this.ManagedComponentStore.GetAllUniqueSharedComponents_Managed<T>(sharedComponentValues);
				return;
			}
			T t = default(T);
			ComponentTypeList componentTypeList;
			UnsafeList<int> unsafeList;
			this.EntityComponentStore->GetAllUniqueSharedComponents_Unmanaged(typeIndex, UnsafeUtility.AddressOf<T>(ref t), out componentTypeList, out unsafeList, Allocator.Temp);
			for (int i = 0; i < componentTypeList.Length; i++)
			{
				T t2 = UnsafeUtility.ReadArrayElement<T>(componentTypeList.Ptr, i);
				sharedComponentValues.Add(t2);
			}
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x0002BC80 File Offset: 0x00029E80
		[ExcludeFromBurstCompatTesting("Takes managed list")]
		public unsafe void GetAllUniqueSharedComponents<T>(List<T> sharedComponentValues, List<int> sharedComponentIndices) where T : struct, ISharedComponentData
		{
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			if (TypeManager.IsManagedSharedComponent(typeIndex))
			{
				this.ManagedComponentStore.GetAllUniqueSharedComponents_Managed<T>(sharedComponentValues, sharedComponentIndices);
				return;
			}
			T t = default(T);
			ComponentTypeList componentTypeList;
			UnsafeList<int> unsafeList;
			this.EntityComponentStore->GetAllUniqueSharedComponents_Unmanaged(typeIndex, UnsafeUtility.AddressOf<T>(ref t), out componentTypeList, out unsafeList, Allocator.Temp);
			for (int i = 0; i < componentTypeList.Length; i++)
			{
				T t2 = UnsafeUtility.ReadArrayElement<T>(componentTypeList.Ptr, i);
				sharedComponentValues.Add(t2);
				sharedComponentIndices.Add(unsafeList[i]);
			}
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x0002BD0C File Offset: 0x00029F0C
		[ExcludeFromBurstCompatTesting("Potentially accesses managed component store")]
		public unsafe int InsertSharedComponent<T>(T newData) where T : struct, ISharedComponentData
		{
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			int num;
			if (TypeManager.IsManagedSharedComponent(typeIndex))
			{
				num = this.ManagedComponentStore.InsertSharedComponent_Managed<T>(newData);
			}
			else
			{
				T t = default(T);
				num = this.EntityComponentStore->InsertSharedComponent_Unmanaged(typeIndex, 0, UnsafeUtility.AddressOf<T>(ref newData), UnsafeUtility.AddressOf<T>(ref t));
			}
			this.m_ManagedReferenceIndexList.Add(in num);
			return num;
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x0002BD68 File Offset: 0x00029F68
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleSharedComponentData) })]
		public unsafe int InsertSharedComponent_Unmanaged<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(T newData) where T : struct, ValueType, ISharedComponentData
		{
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			T t = default(T);
			int num = this.EntityComponentStore->InsertSharedComponent_Unmanaged(typeIndex, 0, UnsafeUtility.AddressOf<T>(ref newData), UnsafeUtility.AddressOf<T>(ref t));
			this.m_ManagedReferenceIndexList.Add(in num);
			return num;
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x0002BDB0 File Offset: 0x00029FB0
		[ExcludeFromBurstCompatTesting("Potentially accesses managed component store")]
		public unsafe int GetSharedComponentVersion<T>(T sharedData) where T : struct
		{
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			if (TypeManager.IsManagedSharedComponent(typeIndex))
			{
				return this.ManagedComponentStore.GetSharedComponentVersion_Managed<T>(sharedData);
			}
			T t = default(T);
			return this.EntityComponentStore->GetSharedComponentVersion_Unmanaged(typeIndex, UnsafeUtility.AddressOf<T>(ref sharedData), UnsafeUtility.AddressOf<T>(ref t));
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x0002BDFC File Offset: 0x00029FFC
		[ExcludeFromBurstCompatTesting("Potentially accesses managed component store")]
		public unsafe T GetSharedComponentData<T>(int sharedComponentIndex) where T : struct
		{
			if (Unity.Entities.EntityComponentStore.IsUnmanagedSharedComponentIndex(sharedComponentIndex))
			{
				T t = default(T);
				if (sharedComponentIndex != 0)
				{
					this.EntityComponentStore->GetSharedComponentData_Unmanaged(sharedComponentIndex, TypeManager.GetTypeIndex<T>(), UnsafeUtility.AddressOf<T>(ref t));
				}
				return t;
			}
			return this.ManagedComponentStore.GetSharedComponentData_Managed<T>(sharedComponentIndex);
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x0002BE42 File Offset: 0x0002A042
		[ExcludeFromBurstCompatTesting("Potentially accesses managed component store")]
		public unsafe void AddSharedComponentReference(int sharedComponentIndex, int numRefs = 1)
		{
			if (Unity.Entities.EntityComponentStore.IsUnmanagedSharedComponentIndex(sharedComponentIndex))
			{
				this.EntityComponentStore->AddSharedComponentReference_Unmanaged(sharedComponentIndex, numRefs);
				return;
			}
			this.ManagedComponentStore.AddSharedComponentReference_Managed(sharedComponentIndex, numRefs);
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x0002BE67 File Offset: 0x0002A067
		[ExcludeFromBurstCompatTesting("Potentially accesses managed component store")]
		public unsafe void RemoveSharedComponentReference(int sharedComponentIndex, int numRefs = 1)
		{
			if (Unity.Entities.EntityComponentStore.IsUnmanagedSharedComponentIndex(sharedComponentIndex))
			{
				this.EntityComponentStore->RemoveSharedComponentReference_Unmanaged(sharedComponentIndex, numRefs);
				return;
			}
			this.ManagedComponentStore.RemoveSharedComponentReference_Managed(sharedComponentIndex, numRefs);
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x0002BE8C File Offset: 0x0002A08C
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public unsafe int GetSharedComponentCount()
		{
			return this.ManagedComponentStore.GetSharedComponentCount() + this.EntityComponentStore->GetUnmanagedSharedComponentCount();
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x0002BEA8 File Offset: 0x0002A0A8
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public unsafe NativeParallelHashMap<int, int> MoveAllSharedComponents(EntityComponentStore* srcEntityComponentStore, ManagedComponentStore srcManagedComponents, AllocatorManager.AllocatorHandle allocator)
		{
			int sharedComponentCount = srcManagedComponents.GetSharedComponentCount();
			NativeParallelHashMap<int, int> nativeParallelHashMap = new NativeParallelHashMap<int, int>(srcEntityComponentStore->GetUnmanagedSharedComponentCount() + sharedComponentCount, allocator);
			this.ManagedComponentStore.MoveAllSharedComponents_Managed(srcManagedComponents, ref nativeParallelHashMap, sharedComponentCount);
			this.EntityComponentStore->MoveAllSharedComponents_Unmanaged(srcEntityComponentStore, ref nativeParallelHashMap);
			return nativeParallelHashMap;
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x0002BEEA File Offset: 0x0002A0EA
		[BurstCompile]
		private static void BuildSharedComponentMapForMoving(ref NativeHashMap<int, int> hashmap, ref NativeArray<ArchetypeChunk> chunks)
		{
			EntityDataAccess.BuildSharedComponentMapForMoving_0000077D$BurstDirectCall.Invoke(ref hashmap, ref chunks);
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x0002BEF4 File Offset: 0x0002A0F4
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public unsafe NativeHashMap<int, int> MoveSharedComponents(EntityComponentStore* srcEntityComponentStore, ManagedComponentStore srcManagedComponents, NativeArray<ArchetypeChunk> chunks, AllocatorManager.AllocatorHandle allocator)
		{
			NativeHashMap<int, int> nativeHashMap = new NativeHashMap<int, int>(64, allocator);
			EntityDataAccess.BuildSharedComponentMapForMoving(ref nativeHashMap, ref chunks);
			NativeKeyValueArrays<int, int> keyValueArrays = nativeHashMap.GetKeyValueArrays(Allocator.Temp);
			for (int i = 0; i < keyValueArrays.Length; i++)
			{
				int num = keyValueArrays.Keys[i];
				int num2 = keyValueArrays.Values[i];
				int num3;
				if (Unity.Entities.EntityComponentStore.IsUnmanagedSharedComponentIndex(num))
				{
					num3 = this.EntityComponentStore->CloneSharedComponentNonDefault(srcEntityComponentStore, num);
					this.EntityComponentStore->AddSharedComponentReference_Unmanaged(num3, num2 - 1);
					srcEntityComponentStore->RemoveSharedComponentReference_Unmanaged(num, num2);
					this.EntityComponentStore->IncrementSharedComponentVersion_Unmanaged(num3);
				}
				else
				{
					num3 = this.ManagedComponentStore.CloneSharedComponentNonDefault(srcManagedComponents, num);
					this.ManagedComponentStore.AddSharedComponentReference_Managed(num3, num2 - 1);
					srcManagedComponents.RemoveSharedComponentReference_Managed(num, num2);
					this.ManagedComponentStore.IncrementSharedComponentVersion_Managed(num3);
				}
				nativeHashMap[num] = num3;
			}
			keyValueArrays.Dispose();
			return nativeHashMap;
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x0002BFE0 File Offset: 0x0002A1E0
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public unsafe int InsertSharedComponentAssumeNonDefault(TypeIndex typeIndex, int hashCode, object sharedComponent)
		{
			if (TypeManager.IsManagedSharedComponent(typeIndex))
			{
				return this.ManagedComponentStore.InsertSharedComponentAssumeNonDefault(typeIndex, hashCode, sharedComponent);
			}
			ulong num;
			byte* ptr = (byte*)UnsafeUtility.PinGCObjectAndGetAddress(sharedComponent, out num) + TypeManager.ObjectOffset;
			int num2 = this.EntityComponentStore->InsertSharedComponent_Unmanaged(typeIndex, hashCode, (void*)ptr, null);
			UnsafeUtility.ReleaseGCObject(num);
			return num2;
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x0002C02C File Offset: 0x0002A22C
		[GenerateTestsForBurstCompatibility]
		public unsafe int InsertSharedComponent_Unmanaged(TypeIndex typeIndex, int hashCode, void* newData, void* defaultValue)
		{
			int num = this.EntityComponentStore->InsertSharedComponent_Unmanaged(typeIndex, hashCode, newData, defaultValue);
			if (num != 0)
			{
				this.m_ManagedReferenceIndexList.Add(in num);
			}
			return num;
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x0002C05C File Offset: 0x0002A25C
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public int InsertSharedComponent_Managed(TypeIndex typeIndex, int hashCode, object newData)
		{
			int num = this.ManagedComponentStore.InsertSharedComponentAssumeNonDefault(typeIndex, hashCode, newData);
			if (num != 0)
			{
				this.m_ManagedReferenceIndexList.Add(in num);
			}
			return num;
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x0002C08C File Offset: 0x0002A28C
		[GenerateTestsForBurstCompatibility]
		public unsafe void SetSharedComponentData_Unmanaged(Entity entity, TypeIndex typeIndex, void* componentData, void* componentDataDefaultValue, in SystemHandle originSystem = null)
		{
			ComponentType componentType = ComponentType.FromTypeIndex(typeIndex);
			int num = this.InsertSharedComponent_Unmanaged(typeIndex, 0, componentData, componentDataDefaultValue);
			this.EntityComponentStore->SetSharedComponentDataIndex(entity, componentType, num);
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x0002C0BC File Offset: 0x0002A2BC
		[GenerateTestsForBurstCompatibility]
		public unsafe void SetSharedComponentData_Unmanaged(NativeArray<Entity> entities, TypeIndex typeIndex, void* componentData, void* componentDataDefaultValue, in SystemHandle originSystem = null)
		{
			ComponentType componentType = ComponentType.FromTypeIndex(typeIndex);
			int num = this.InsertSharedComponent_Unmanaged(typeIndex, 0, componentData, componentDataDefaultValue);
			StructuralChange.SetSharedComponentDataIndexWithBurst(this.EntityComponentStore, (Entity*)entities.GetUnsafeReadOnlyPtr<Entity>(), entities.Length, in componentType, num);
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x0002C0F8 File Offset: 0x0002A2F8
		[GenerateTestsForBurstCompatibility]
		public unsafe void SetSharedComponentData_Unmanaged(ArchetypeChunk chunk, TypeIndex typeIndex, void* componentData, void* componentDataDefaultValue, in SystemHandle originSystem = null)
		{
			ComponentType componentType = ComponentType.FromTypeIndex(typeIndex);
			int num = this.InsertSharedComponent_Unmanaged(typeIndex, 0, componentData, componentDataDefaultValue);
			this.EntityComponentStore->SetSharedComponentDataIndexForChunk(chunk.m_Chunk, chunk.Archetype.Archetype, componentType, num);
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x0002C137 File Offset: 0x0002A337
		public unsafe void SetSharedComponentDataOnQueryDuringStructuralChange(EntityQueryImpl* queryImpl, int sharedComponentIndex, ComponentType componentType, in SystemHandle originSystem = null)
		{
			queryImpl->SyncFilterTypes();
			StructuralChange.SetSharedComponentDataIndexWithBurst(this.EntityComponentStore, queryImpl, in componentType, sharedComponentIndex);
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x0002C137 File Offset: 0x0002A337
		[GenerateTestsForBurstCompatibility]
		public unsafe void SetSharedComponentDataOnQueryDuringStructuralChange_Unmanaged(EntityQueryImpl* queryImpl, int sharedComponentIndex, ComponentType componentType, void* componentData, in SystemHandle originSystem = null)
		{
			queryImpl->SyncFilterTypes();
			StructuralChange.SetSharedComponentDataIndexWithBurst(this.EntityComponentStore, queryImpl, in componentType, sharedComponentIndex);
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x0002C150 File Offset: 0x0002A350
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleSharedComponentData) })]
		public unsafe T GetSharedComponentData_Unmanaged<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity) where T : struct, ValueType, ISharedComponentData
		{
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			int sharedComponentDataIndex = this.EntityComponentStore->GetSharedComponentDataIndex(entity, typeIndex);
			return this.GetSharedComponentData_Unmanaged<T>(sharedComponentDataIndex);
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x0002C178 File Offset: 0x0002A378
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleSharedComponentData) })]
		public unsafe T GetSharedComponentData_Unmanaged<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(int sharedComponentIndex) where T : struct, ValueType
		{
			T t = default(T);
			if (sharedComponentIndex != 0)
			{
				this.EntityComponentStore->GetSharedComponentData_Unmanaged(sharedComponentIndex, TypeManager.GetTypeIndex<T>(), UnsafeUtility.AddressOf<T>(ref t));
			}
			return t;
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x0002C1AC File Offset: 0x0002A3AC
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleSharedComponentData) })]
		public unsafe int GetSharedComponentVersion_Unmanaged<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(T sharedData) where T : struct, ValueType
		{
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			T t = default(T);
			return this.EntityComponentStore->GetSharedComponentVersion_Unmanaged(typeIndex, UnsafeUtility.AddressOf<T>(ref sharedData), UnsafeUtility.AddressOf<T>(ref t));
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x0002C1E4 File Offset: 0x0002A3E4
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public unsafe bool AllSharedComponentReferencesAreFromChunks(EntityComponentStore* entityComponentStore)
		{
			UnsafeParallelHashMap<int, int> unsafeParallelHashMap = new UnsafeParallelHashMap<int, int>(64, Allocator.Temp);
			for (int i = 0; i < entityComponentStore->m_Archetypes.Length; i++)
			{
				Archetype* ptr = *(IntPtr*)(entityComponentStore->m_Archetypes.Ptr + (IntPtr)i * (IntPtr)sizeof(Archetype*) / (IntPtr)sizeof(Archetype*));
				int count = ptr->Chunks.Count;
				for (int j = 0; j < ptr->NumSharedComponents; j++)
				{
					int* sharedComponentValueArrayForType = ptr->Chunks.GetSharedComponentValueArrayForType(j);
					for (int k = 0; k < count; k++)
					{
						int num = sharedComponentValueArrayForType[k];
						int num2;
						if (unsafeParallelHashMap.TryGetValue(num, out num2))
						{
							unsafeParallelHashMap[num] = num2 + 1;
						}
						else
						{
							unsafeParallelHashMap.Add(num, 1);
						}
					}
				}
			}
			return this.ManagedComponentStore.AllSharedComponentReferencesAreFromChunks(unsafeParallelHashMap) && this.EntityComponentStore->AllSharedComponentReferencesAreFromChunks(unsafeParallelHashMap);
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x0002C2C0 File Offset: 0x0002A4C0
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public unsafe void CopySharedComponents(EntityDataAccess* srcEntityDataAccess, int* sharedComponentIndices, int sharedComponentIndicesCount)
		{
			EntityComponentStore* entityComponentStore = srcEntityDataAccess->EntityComponentStore;
			EntityComponentStore* entityComponentStore2 = this.EntityComponentStore;
			ManagedComponentStore managedComponentStore = srcEntityDataAccess->ManagedComponentStore;
			ManagedComponentStore managedComponentStore2 = this.ManagedComponentStore;
			for (int num = 0; num != sharedComponentIndicesCount; num++)
			{
				int num2 = sharedComponentIndices[num];
				if (num2 != 0)
				{
					int num3;
					if (Unity.Entities.EntityComponentStore.IsUnmanagedSharedComponentIndex(num2))
					{
						num3 = entityComponentStore2->CloneSharedComponentNonDefault(entityComponentStore, num2);
					}
					else
					{
						num3 = managedComponentStore2.CloneSharedComponentNonDefault(managedComponentStore, num2);
					}
					sharedComponentIndices[num] = num3;
				}
			}
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x0002C334 File Offset: 0x0002A534
		public unsafe EntityStorageInfo GetStorageInfo(Entity entity)
		{
			EntityInChunk entityInChunk = this.EntityComponentStore->GetEntityInChunk(entity);
			return new EntityStorageInfo
			{
				Chunk = new ArchetypeChunk(entityInChunk.Chunk, this.EntityComponentStore),
				IndexInChunk = entityInChunk.IndexInChunk
			};
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x0002C37C File Offset: 0x0002A57C
		private unsafe void PlaybackManagedChangesMono()
		{
			this.ManagedComponentStore.Playback(ref this.EntityComponentStore->ManagedChangesTracker);
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x0002C394 File Offset: 0x0002A594
		[BurstDiscard]
		private void PlaybackManagedDirectly(ref bool didTheThing)
		{
			didTheThing = true;
			this.PlaybackManagedChangesMono();
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x0002C3A0 File Offset: 0x0002A5A0
		internal unsafe void PlaybackManagedChanges()
		{
			if (this.EntityComponentStore->ManagedChangesTracker.Empty)
			{
				return;
			}
			bool flag = false;
			this.PlaybackManagedDirectly(ref flag);
			if (flag)
			{
				return;
			}
			fixed (EntityDataAccess* ptr = &this)
			{
				void* ptr2 = (void*)ptr;
				new FunctionPointer<EntityDataAccess.PlaybackManagedDelegate>(*EntityDataAccess.s_ManagedPlaybackTrampoline.Data).Invoke((IntPtr)ptr2);
			}
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x0002C3FC File Offset: 0x0002A5FC
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleBufferElement) })]
		public unsafe DynamicBuffer<T> GetBuffer<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity, bool isReadOnly = false) where T : struct, ValueType, IBufferElementData
		{
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			if (!this.IsInExclusiveTransaction)
			{
				if (isReadOnly)
				{
					this.DependencyManager->CompleteWriteDependency(typeIndex);
				}
				else
				{
					this.DependencyManager->CompleteReadAndWriteDependency(typeIndex);
				}
			}
			BufferHeader* ptr;
			if (isReadOnly)
			{
				ptr = (BufferHeader*)this.EntityComponentStore->GetComponentDataWithTypeRO(entity, typeIndex);
			}
			else
			{
				ptr = (BufferHeader*)this.EntityComponentStore->GetComponentDataWithTypeRW(entity, typeIndex, this.EntityComponentStore->GlobalSystemVersion);
			}
			int bufferCapacity = TypeManager.GetTypeInfo(typeIndex).BufferCapacity;
			return new DynamicBuffer<T>(ptr, bufferCapacity);
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x0002C473 File Offset: 0x0002A673
		public unsafe void SetBufferRaw(Entity entity, TypeIndex componentTypeIndex, BufferHeader* tempBuffer, int sizeInChunk, in SystemHandle originSystem = null)
		{
			if (!this.IsInExclusiveTransaction)
			{
				this.DependencyManager->CompleteReadAndWriteDependency(componentTypeIndex);
			}
			this.EntityComponentStore->SetBufferRaw(entity, componentTypeIndex, tempBuffer, sizeInChunk);
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x0002C499 File Offset: 0x0002A699
		public EntityArchetype GetEntityAndSimulateArchetype()
		{
			if (!this.m_EntityAndSimulateOnlyArchetype.Valid)
			{
				this.m_EntityAndSimulateOnlyArchetype = this.CreateArchetype(null, 0, true);
			}
			return this.m_EntityAndSimulateOnlyArchetype;
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x0002C4BE File Offset: 0x0002A6BE
		internal unsafe void InstantiateInternalDuringStructuralChange(Entity srcEntity, Entity* outputEntities, int count, in SystemHandle originSystem = null)
		{
			StructuralChange.InstantiateEntity(this.EntityComponentStore, &srcEntity, outputEntities, count);
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x0002C4D0 File Offset: 0x0002A6D0
		internal unsafe void InstantiateInternalDuringStructuralChange(Entity* srcEntities, Entity* outputEntities, int count, int outputCount, bool removePrefab, in SystemHandle originSystem = null)
		{
			StructuralChange.InstantiateEntities(this.EntityComponentStore, srcEntities, outputEntities, count, removePrefab);
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x0002C4E4 File Offset: 0x0002A6E4
		public unsafe void SwapComponents(ArchetypeChunk leftChunk, int leftIndex, ArchetypeChunk rightChunk, int rightIndex)
		{
			if (!this.IsInExclusiveTransaction)
			{
				this.BeforeStructuralChange();
			}
			uint globalSystemVersion = this.EntityComponentStore->GlobalSystemVersion;
			ChunkDataUtility.SwapComponents(leftChunk.Archetype.Archetype, leftChunk.m_Chunk, leftIndex, rightChunk.Archetype.Archetype, rightChunk.m_Chunk, rightIndex, 1, globalSystemVersion, globalSystemVersion);
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x0002C53A File Offset: 0x0002A73A
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleBufferElement) })]
		public unsafe BufferTypeHandle<T> GetBufferTypeHandle<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(bool isReadOnly) where T : struct, ValueType, IBufferElementData
		{
			return new BufferTypeHandle<T>(isReadOnly, this.EntityComponentStore->GlobalSystemVersion);
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x0002C54D File Offset: 0x0002A74D
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public void GetAllUniqueSharedComponents<T>(List<T> sharedComponentValues, List<int> sharedComponentIndices, List<int> sharedComponentVersions) where T : struct, ISharedComponentData
		{
			this.ManagedComponentStore.GetAllUniqueSharedComponents_Managed<T>(sharedComponentValues, sharedComponentIndices, sharedComponentVersions);
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x0002C55D File Offset: 0x0002A75D
		public bool HasBlobReferences(int sharedComponentIndex)
		{
			return TypeManager.GetTypeInfo(Unity.Entities.EntityComponentStore.GetComponentTypeFromSharedComponentIndex(sharedComponentIndex)).HasBlobAssetRefs;
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x0002C56F File Offset: 0x0002A76F
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public NativeArray<int> MoveSharedComponents(ManagedComponentStore srcManagedComponents, NativeArray<ArchetypeChunk> chunks, AllocatorManager.AllocatorHandle allocator)
		{
			return this.ManagedComponentStore.MoveSharedComponents_Managed(srcManagedComponents, chunks, allocator);
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x0002C57F File Offset: 0x0002A77F
		internal unsafe Entity GetEntityByEntityIndex(int index)
		{
			return this.EntityComponentStore->GetEntityByEntityIndex(index);
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x0002C58D File Offset: 0x0002A78D
		internal unsafe int GetNameIndexByEntityIndex(int index)
		{
			return this.EntityComponentStore->GetEntityNameByEntityIndex(index).Index;
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x0002C5A0 File Offset: 0x0002A7A0
		[ExcludeFromBurstCompatTesting("Returns managed string")]
		public unsafe string GetName(Entity entity)
		{
			return this.EntityComponentStore->GetName(entity);
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x0002C5AE File Offset: 0x0002A7AE
		public unsafe void GetName(Entity entity, out FixedString64Bytes name)
		{
			this.EntityComponentStore->GetName(entity, out name);
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x0002C5BD File Offset: 0x0002A7BD
		public unsafe void SetName(Entity entity, in FixedString64Bytes name)
		{
			this.EntityComponentStore->SetName(entity, in name);
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x0002C5CC File Offset: 0x0002A7CC
		public unsafe void CompleteAllTrackedJobs()
		{
			this.DependencyManager->CompleteAllJobsAndInvalidateArrays();
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x0002C5F8 File Offset: 0x0002A7F8
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void SetSharedComponentDataIndexWithBurst$BurstManaged(EntityComponentStore* ecs, in Entity entity, in ComponentType componentType, int newSharedComponentDataIndex)
		{
			ecs->SetSharedComponentDataIndex(entity, componentType, newSharedComponentDataIndex);
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x0002C610 File Offset: 0x0002A810
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void BuildSharedComponentMapForMoving$BurstManaged(ref NativeHashMap<int, int> hashmap, ref NativeArray<ArchetypeChunk> chunks)
		{
			NativeHashMap<int, int> nativeHashMap = hashmap;
			for (int i = 0; i < chunks.Length; i++)
			{
				ChunkIndex chunk = chunks[i].m_Chunk;
				Archetype* archetype = chunks[i].Archetype.Archetype;
				SharedComponentValues sharedComponentValues = archetype->Chunks.GetSharedComponentValues(chunk.ListIndex);
				for (int j = 0; j < archetype->NumSharedComponents; j++)
				{
					int num = *sharedComponentValues[j];
					int num2;
					if (nativeHashMap.TryGetValue(num, out num2))
					{
						num2 = (nativeHashMap[num] = num2 + 1);
					}
					else
					{
						nativeHashMap.Add(num, 1);
					}
				}
			}
		}

		// Token: 0x04000450 RID: 1104
		internal const int FASTER_TO_BATCH_THRESHOLD = 10;

		// Token: 0x04000451 RID: 1105
		internal const int MANAGED_REFERENCES_DEFAULT = 8;

		// Token: 0x04000452 RID: 1106
		private static readonly SharedStatic<IntPtr> s_ManagedPlaybackTrampoline = SharedStatic<IntPtr>.GetOrCreateUnsafe(0U, -7572400463909285300L, 0L);

		// Token: 0x04000453 RID: 1107
		private static object s_DelegateGCPrevention;

		// Token: 0x04000454 RID: 1108
		[NativeDisableUnsafePtrRestriction]
		private EntityComponentStore m_EntityComponentStore;

		// Token: 0x04000455 RID: 1109
		[NativeDisableUnsafePtrRestriction]
		private EntityQueryManager m_EntityQueryManager;

		// Token: 0x04000456 RID: 1110
		[NativeDisableUnsafePtrRestriction]
		private ComponentDependencyManager m_DependencyManager;

		// Token: 0x04000457 RID: 1111
		[NativeDisableUnsafePtrRestriction]
		private UnsafeList<int> m_ManagedReferenceIndexList;

		// Token: 0x04000458 RID: 1112
		[NativeDisableUnsafePtrRestriction]
		public EntityQuery m_UniversalQuery;

		// Token: 0x04000459 RID: 1113
		[NativeDisableUnsafePtrRestriction]
		public EntityQuery m_UniversalQueryWithChunks;

		// Token: 0x0400045A RID: 1114
		[NativeDisableUnsafePtrRestriction]
		public EntityQuery m_UniversalQueryWithSystems;

		// Token: 0x0400045B RID: 1115
		[NativeDisableUnsafePtrRestriction]
		public EntityQuery m_UniversalQueryWithChunksAndSystems;

		// Token: 0x0400045C RID: 1116
		[NativeDisableUnsafePtrRestriction]
		public EntityQuery m_EntityGuidQuery;

		// Token: 0x0400045D RID: 1117
		[NativeDisableUnsafePtrRestriction]
		public WorldUnmanaged m_WorldUnmanaged;

		// Token: 0x0400045E RID: 1118
		internal const int kBuiltinEntityQueryCount = 5;

		// Token: 0x0400045F RID: 1119
		private int m_ManagedAccessHandle;

		// Token: 0x04000460 RID: 1120
		private EntityArchetype m_EntityAndSimulateOnlyArchetype;

		// Token: 0x04000461 RID: 1121
		private UntypedUnsafeParallelHashMap m_AliveEntityQueries;

		// Token: 0x02000169 RID: 361
		// (Invoke) Token: 0x0600085F RID: 2143
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void PlaybackManagedDelegate(IntPtr self);

		// Token: 0x0200016A RID: 362
		// (Invoke) Token: 0x06000863 RID: 2147
		internal unsafe delegate void SetSharedComponentDataIndexWithBurst_0000076F$PostfixBurstDelegate(EntityComponentStore* ecs, in Entity entity, in ComponentType componentType, int newSharedComponentDataIndex);

		// Token: 0x0200016B RID: 363
		internal static class SetSharedComponentDataIndexWithBurst_0000076F$BurstDirectCall
		{
			// Token: 0x06000866 RID: 2150 RVA: 0x0002C6BC File Offset: 0x0002A8BC
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (EntityDataAccess.SetSharedComponentDataIndexWithBurst_0000076F$BurstDirectCall.Pointer == 0)
				{
					EntityDataAccess.SetSharedComponentDataIndexWithBurst_0000076F$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(EntityDataAccess.SetSharedComponentDataIndexWithBurst_0000076F$BurstDirectCall.DeferredCompilation, methodof(EntityDataAccess.SetSharedComponentDataIndexWithBurst$BurstManaged(Unity.Entities.EntityComponentStore*, ref Entity, ref ComponentType, int)).MethodHandle, typeof(EntityDataAccess.SetSharedComponentDataIndexWithBurst_0000076F$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = EntityDataAccess.SetSharedComponentDataIndexWithBurst_0000076F$BurstDirectCall.Pointer;
			}

			// Token: 0x06000867 RID: 2151 RVA: 0x0002C6E8 File Offset: 0x0002A8E8
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				EntityDataAccess.SetSharedComponentDataIndexWithBurst_0000076F$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000868 RID: 2152 RVA: 0x0002C700 File Offset: 0x0002A900
			public unsafe static void Constructor()
			{
				EntityDataAccess.SetSharedComponentDataIndexWithBurst_0000076F$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(EntityDataAccess.SetSharedComponentDataIndexWithBurst(Unity.Entities.EntityComponentStore*, ref Entity, ref ComponentType, int)).MethodHandle);
			}

			// Token: 0x06000869 RID: 2153 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x0600086A RID: 2154 RVA: 0x0002C711 File Offset: 0x0002A911
			// Note: this type is marked as 'beforefieldinit'.
			static SetSharedComponentDataIndexWithBurst_0000076F$BurstDirectCall()
			{
				EntityDataAccess.SetSharedComponentDataIndexWithBurst_0000076F$BurstDirectCall.Constructor();
			}

			// Token: 0x0600086B RID: 2155 RVA: 0x0002C718 File Offset: 0x0002A918
			public unsafe static void Invoke(EntityComponentStore* ecs, in Entity entity, in ComponentType componentType, int newSharedComponentDataIndex)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = EntityDataAccess.SetSharedComponentDataIndexWithBurst_0000076F$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.EntityComponentStore*,Unity.Entities.Entity&,Unity.Entities.ComponentType&,System.Int32), ecs, ref entity, ref componentType, newSharedComponentDataIndex, functionPointer);
						return;
					}
				}
				EntityDataAccess.SetSharedComponentDataIndexWithBurst$BurstManaged(ecs, in entity, in componentType, newSharedComponentDataIndex);
			}

			// Token: 0x04000462 RID: 1122
			private static IntPtr Pointer;

			// Token: 0x04000463 RID: 1123
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x0200016C RID: 364
		// (Invoke) Token: 0x0600086D RID: 2157
		internal delegate void BuildSharedComponentMapForMoving_0000077D$PostfixBurstDelegate(ref NativeHashMap<int, int> hashmap, ref NativeArray<ArchetypeChunk> chunks);

		// Token: 0x0200016D RID: 365
		internal static class BuildSharedComponentMapForMoving_0000077D$BurstDirectCall
		{
			// Token: 0x06000870 RID: 2160 RVA: 0x0002C74F File Offset: 0x0002A94F
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (EntityDataAccess.BuildSharedComponentMapForMoving_0000077D$BurstDirectCall.Pointer == 0)
				{
					EntityDataAccess.BuildSharedComponentMapForMoving_0000077D$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(EntityDataAccess.BuildSharedComponentMapForMoving_0000077D$BurstDirectCall.DeferredCompilation, methodof(EntityDataAccess.BuildSharedComponentMapForMoving$BurstManaged(ref NativeHashMap<int, int>, ref NativeArray<ArchetypeChunk>)).MethodHandle, typeof(EntityDataAccess.BuildSharedComponentMapForMoving_0000077D$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = EntityDataAccess.BuildSharedComponentMapForMoving_0000077D$BurstDirectCall.Pointer;
			}

			// Token: 0x06000871 RID: 2161 RVA: 0x0002C77C File Offset: 0x0002A97C
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				EntityDataAccess.BuildSharedComponentMapForMoving_0000077D$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000872 RID: 2162 RVA: 0x0002C794 File Offset: 0x0002A994
			public static void Constructor()
			{
				EntityDataAccess.BuildSharedComponentMapForMoving_0000077D$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(EntityDataAccess.BuildSharedComponentMapForMoving(ref NativeHashMap<int, int>, ref NativeArray<ArchetypeChunk>)).MethodHandle);
			}

			// Token: 0x06000873 RID: 2163 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06000874 RID: 2164 RVA: 0x0002C7A5 File Offset: 0x0002A9A5
			// Note: this type is marked as 'beforefieldinit'.
			static BuildSharedComponentMapForMoving_0000077D$BurstDirectCall()
			{
				EntityDataAccess.BuildSharedComponentMapForMoving_0000077D$BurstDirectCall.Constructor();
			}

			// Token: 0x06000875 RID: 2165 RVA: 0x0002C7AC File Offset: 0x0002A9AC
			public static void Invoke(ref NativeHashMap<int, int> hashmap, ref NativeArray<ArchetypeChunk> chunks)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = EntityDataAccess.BuildSharedComponentMapForMoving_0000077D$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Collections.NativeHashMap`2<System.Int32,System.Int32>&,Unity.Collections.NativeArray`1<Unity.Entities.ArchetypeChunk>&), ref hashmap, ref chunks, functionPointer);
						return;
					}
				}
				EntityDataAccess.BuildSharedComponentMapForMoving$BurstManaged(ref hashmap, ref chunks);
			}

			// Token: 0x04000464 RID: 1124
			private static IntPtr Pointer;

			// Token: 0x04000465 RID: 1125
			private static IntPtr DeferredCompilation;
		}
	}
}
