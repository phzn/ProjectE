using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

namespace Unity.Entities
{
	// Token: 0x02000140 RID: 320
	[BurstCompile]
	internal struct EntityComponentStore
	{
		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000671 RID: 1649 RVA: 0x00021ECC File Offset: 0x000200CC
		public ulong WorldSequenceNumber
		{
			get
			{
				return this.m_WorldSequenceNumber;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000672 RID: 1650 RVA: 0x00021ED4 File Offset: 0x000200D4
		// (set) Token: 0x06000673 RID: 1651 RVA: 0x00021EF9 File Offset: 0x000200F9
		public unsafe static ulong TotalChunkAddressSpaceInBytes
		{
			get
			{
				if (*EntityComponentStore.s_TotalChunkAddressSpaceInBytes.Data <= 0UL)
				{
					return EntityComponentStore.DefaultChunkAddressSpaceInBytes;
				}
				return *EntityComponentStore.s_TotalChunkAddressSpaceInBytes.Data - 1UL;
			}
			set
			{
				*EntityComponentStore.s_TotalChunkAddressSpaceInBytes.Data = value + 1UL;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000674 RID: 1652 RVA: 0x00021F0A File Offset: 0x0002010A
		public EntityNameStoreAccess NameStoreAccess
		{
			get
			{
				return this.m_NameStoreAccess;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000675 RID: 1653 RVA: 0x00021F12 File Offset: 0x00020112
		public ulong NameChangeBitsSequenceNum
		{
			get
			{
				return this.m_NameStoreAccess.NameChangeBitsSequenceNum;
			}
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x00021F1F File Offset: 0x0002011F
		public ulong IncNameChangeBitsVersion()
		{
			return this.m_NameStoreAccess.IncNameChangeBitsVersion();
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x00021F2C File Offset: 0x0002012C
		public void SetNameChangeBitsVersion(ulong nameChangeBitsVersion)
		{
			this.m_NameStoreAccess.SetNameChangeBitsVersion(nameChangeBitsVersion);
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x00021F3A File Offset: 0x0002013A
		public EntityName GetEntityNameByEntityIndex(int index)
		{
			return this.m_NameStoreAccess.GetEntityNameByEntityIndex(index);
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x00021F48 File Offset: 0x00020148
		public unsafe void CopyAndUpdateNameByEntity(EntityComponentStore* fromEntityComponentStore, NativeArray<ArchetypeChunk> srcChunks, NativeArray<EntityRemapUtility.EntityRemapInfo> remap)
		{
			int i = 0;
			int length = srcChunks.Length;
			while (i < length)
			{
				ChunkIndex chunk = srcChunks[i].m_Chunk;
				Entity* buffer = (Entity*)chunk.Buffer;
				int count = chunk.Count;
				for (int j = 0; j < count; j++)
				{
					Entity entity = buffer[j];
					Entity entity2 = EntityRemapUtility.RemapEntity(ref remap, entity);
					EntityName entityName = this.m_NameStoreAccess.GetEntityName(entity);
					this.m_NameStoreAccess.SetEntityName(entity2, entityName);
				}
				i++;
			}
			ulong num = this.IncNameChangeBitsVersion();
			ulong num2 = fromEntityComponentStore->IncNameChangeBitsVersion();
			ulong num3 = math.max(num, num2);
			this.SetNameChangeBitsVersion(num3);
			fromEntityComponentStore->SetNameChangeBitsVersion(num3);
			this.m_NameStoreAccess.ResetEntitiesWithNamesSet();
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600067A RID: 1658 RVA: 0x00022005 File Offset: 0x00020205
		public int EntityOrderVersion
		{
			get
			{
				return this.GetComponentTypeOrderVersion(this.m_EntityType);
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600067B RID: 1659 RVA: 0x00022013 File Offset: 0x00020213
		public uint GlobalSystemVersion
		{
			get
			{
				return this.m_GlobalSystemVersion;
			}
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x0002201B File Offset: 0x0002021B
		public void IncrementGlobalSystemVersion(in SystemHandle handle = null)
		{
			ChangeVersionUtility.IncrementGlobalSystemVersion(ref this.m_GlobalSystemVersion);
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x00022028 File Offset: 0x00020228
		public void SetGlobalSystemVersion(uint value, in SystemHandle handle = null)
		{
			this.m_GlobalSystemVersion = value;
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600067E RID: 1662 RVA: 0x00022031 File Offset: 0x00020231
		internal bool IsIntentionallyInconsistent
		{
			get
			{
				return this.m_IntentionallyInconsistent == 1;
			}
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x00002287 File Offset: 0x00000487
		private void ResizeUnmanagedArrays(long oldValue, long newValue)
		{
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x0002203C File Offset: 0x0002023C
		public Entity GetEntityByEntityIndex(int index)
		{
			return EntityComponentStore.s_entityStore.Data.GetEntityByEntityIndex(index);
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x00022050 File Offset: 0x00020250
		public unsafe static void Create(EntityComponentStore* entities, ulong worldSequenceNumber, int newCapacity = 1024)
		{
			EntityComponentStore.PerChunkArray.Initialize();
			EntityComponentStore.ChunkStore.Initialize();
			UnsafeUtility.MemClear((void*)entities, (long)sizeof(EntityComponentStore));
			entities->m_NameStoreAccess = new EntityNameStoreAccess(entities);
			entities->m_GlobalSystemVersion = 1U;
			entities->m_ComponentTypeOrderVersion = Memory.Unmanaged.Array.Allocate<int>(8192L, Allocator.Persistent);
			Memory.Array.Clear<int>(entities->m_ComponentTypeOrderVersion, 8192L);
			entities->m_ArchetypeChunkAllocator = new BlockAllocator(AllocatorManager.Persistent, 16777216);
			entities->m_TypeLookup = default(ArchetypeListMap);
			entities->m_TypeLookup.Init(16);
			entities->m_WorldSequenceNumber = worldSequenceNumber;
			entities->m_NextChunkSequenceNumber = worldSequenceNumber << 32;
			entities->m_Archetypes = new UnsafePtrList<Archetype>(0, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			entities->ManagedChangesTracker = default(ManagedDeferredCommands);
			entities->ManagedChangesTracker.Init(entities);
			entities->m_SharedComponentVersion = 0;
			entities->m_ManagedComponentIndex = 1;
			entities->m_ManagedComponentIndexCapacity = 64;
			entities->m_ManagedComponentFreeIndex = new UnsafeAppendBuffer(1024, 16, Allocator.Persistent);
			entities->m_SharedComponentGlobalVersion = 1;
			entities->m_UnmanagedSharedComponentCount = 0;
			entities->m_UnmanagedSharedComponentTypes = new UnsafeList<TypeIndex>(64, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			entities->m_UnmanagedSharedComponentsByType = new UnsafeList<ComponentTypeList>(TypeManager.GetTypeCount() + 1, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			entities->m_UnmanagedSharedComponentInfo = new UnsafeList<UnsafeList<EntityComponentStore.SharedComponentInfo>>(TypeManager.GetTypeCount(), Allocator.Persistent, NativeArrayOptions.ClearMemory);
			entities->m_HashLookup = new UnsafeParallelMultiHashMap<ulong, int>(128, Allocator.Persistent);
			entities->m_LinkedGroupType = TypeManager.GetTypeIndex<LinkedEntityGroup>();
			entities->m_ChunkHeaderType = TypeManager.GetTypeIndex<ChunkHeader>();
			entities->m_PrefabType = TypeManager.GetTypeIndex<Prefab>();
			entities->m_OmitLinkedEntityGroupFromPrefabInstanceType = TypeManager.GetTypeIndex<OmitLinkedEntityGroupFromPrefabInstance>();
			entities->m_CleanupEntityType = TypeManager.GetTypeIndex<CleanupEntity>();
			entities->m_DisabledType = TypeManager.GetTypeIndex<Disabled>();
			entities->m_EntityType = TypeManager.GetTypeIndex<Entity>();
			entities->m_SystemInstanceType = TypeManager.GetTypeIndex<SystemInstance>();
			entities->m_ChunkHeaderComponentType = ComponentType.ReadWrite<ChunkHeader>();
			entities->m_EntityComponentType = ComponentType.ReadWrite<Entity>();
			entities->m_SimulateComponentType = ComponentType.ReadWrite<Simulate>();
			entities->InitializeTypeManagerPointers();
			entities->m_ChunkListChangesTracker = default(EntityComponentStore.ChunkListChanges);
			entities->m_ChunkListChangesTracker.Init();
			UnsafeUtility.SizeOf<BufferHeader>();
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x0002224B File Offset: 0x0002044B
		internal void InitializeTypeManagerPointers()
		{
			this.m_TypeInfos = TypeManager.GetTypeInfoPointer();
			this.m_EntityOffsetInfos = TypeManager.GetEntityOffsetsPointer();
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x00022263 File Offset: 0x00020463
		public readonly ref TypeManager.TypeInfo GetTypeInfo(TypeIndex typeIndex)
		{
			return this.m_TypeInfos + typeIndex.Index;
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x0002227B File Offset: 0x0002047B
		public unsafe TypeManager.EntityOffsetInfo* GetEntityOffsets(in TypeManager.TypeInfo typeInfo)
		{
			return this.m_EntityOffsetInfos + typeInfo.EntityOffsetStartIndex;
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x00022292 File Offset: 0x00020492
		public unsafe TypeIndex ChunkComponentToNormalTypeIndex(TypeIndex typeIndex)
		{
			return this.m_TypeInfos[typeIndex.Index].TypeIndex;
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x000222AF File Offset: 0x000204AF
		public unsafe static void Destroy(EntityComponentStore* entityComponentStore)
		{
			entityComponentStore->Dispose();
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x000222B8 File Offset: 0x000204B8
		private unsafe void Dispose()
		{
			this.m_NameStoreAccess.Dispose();
			if (this.m_ComponentTypeOrderVersion != null)
			{
				Memory.Unmanaged.Free<int>(this.m_ComponentTypeOrderVersion, Allocator.Persistent);
				this.m_ComponentTypeOrderVersion = null;
			}
			for (int i = 0; i < this.m_Archetypes.Length; i++)
			{
				Archetype* ptr = *(IntPtr*)(this.m_Archetypes.Ptr + (IntPtr)i * (IntPtr)sizeof(Archetype*) / (IntPtr)sizeof(Archetype*));
				for (int num = 0; num != ptr->Chunks.Count; num++)
				{
					ChunkIndex chunkIndex = ptr->Chunks[num];
					ChunkDataUtility.DeallocateBuffers(ptr, chunkIndex);
					int count = chunkIndex.Count;
					EntityComponentStore.s_entityStore.Data.DeallocateEntities((Entity*)chunkIndex.Buffer, count);
					EntityComponentStore.s_chunkStore.Data.FreeContiguousChunks(ptr->Chunks[num], 1);
				}
				ptr->Chunks.Dispose();
				ptr->ChunksWithEmptySlots.Dispose();
				ptr->FreeChunksBySharedComponents.Dispose();
				ptr->MatchingQueryData.Dispose();
			}
			this.m_Archetypes.Dispose();
			this.m_TypeLookup.Dispose();
			this.m_ArchetypeChunkAllocator.Dispose();
			this.ManagedChangesTracker.Dispose();
			this.m_ManagedComponentFreeIndex.Dispose();
			AspectTypeInfoManager.Dispose();
			for (int j = 0; j < this.m_UnmanagedSharedComponentsByType.Length; j++)
			{
				if (this.m_UnmanagedSharedComponentsByType[j].IsCreated)
				{
					TypeManager.TypeInfo typeInfo = TypeManager.GetTypeInfoPointer()[j];
					TypeIndex typeIndex = typeInfo.TypeIndex;
					if (typeIndex.IsRefCounted)
					{
						int typeSize = typeInfo.TypeSize;
						UnsafeList<EntityComponentStore.SharedComponentInfo> unsafeList = this.m_UnmanagedSharedComponentInfo.Ptr[(IntPtr)j * (IntPtr)sizeof(UnsafeList<EntityComponentStore.SharedComponentInfo>) / (IntPtr)sizeof(UnsafeList<EntityComponentStore.SharedComponentInfo>)];
						if (unsafeList.IsCreated)
						{
							for (int k = 1; k < math.min(this.m_UnmanagedSharedComponentsByType[j].Length, unsafeList.Length); k++)
							{
								if (unsafeList[k].RefCount > 0)
								{
									TypeManager.CallIRefCounted_Release(typeIndex, (IntPtr)this.m_UnmanagedSharedComponentsByType[j].Ptr + k * typeSize);
								}
							}
						}
					}
					this.m_UnmanagedSharedComponentsByType[j].Dispose();
				}
			}
			this.m_UnmanagedSharedComponentsByType.Dispose();
			for (int l = 0; l < this.m_UnmanagedSharedComponentInfo.Capacity; l++)
			{
				if (this.m_UnmanagedSharedComponentInfo.Ptr[(IntPtr)l * (IntPtr)sizeof(UnsafeList<EntityComponentStore.SharedComponentInfo>) / (IntPtr)sizeof(UnsafeList<EntityComponentStore.SharedComponentInfo>)].IsCreated)
				{
					this.m_UnmanagedSharedComponentInfo.Ptr[(IntPtr)l * (IntPtr)sizeof(UnsafeList<EntityComponentStore.SharedComponentInfo>) / (IntPtr)sizeof(UnsafeList<EntityComponentStore.SharedComponentInfo>)].Dispose();
				}
			}
			this.m_UnmanagedSharedComponentInfo.Dispose();
			this.m_UnmanagedSharedComponentTypes.Dispose();
			this.m_HashLookup.Dispose();
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x00022588 File Offset: 0x00020788
		private unsafe void ResetSharedComponentData()
		{
			for (int i = 0; i < this.m_UnmanagedSharedComponentsByType.Length; i++)
			{
				if (this.m_UnmanagedSharedComponentsByType[i].IsCreated)
				{
					this.m_UnmanagedSharedComponentsByType[i].Clear();
					this.m_UnmanagedSharedComponentsByType.Ptr[i].Length = 1;
				}
			}
			for (int j = 0; j < this.m_UnmanagedSharedComponentInfo.Capacity; j++)
			{
				if (this.m_UnmanagedSharedComponentInfo.Ptr[(IntPtr)j * (IntPtr)sizeof(UnsafeList<EntityComponentStore.SharedComponentInfo>) / (IntPtr)sizeof(UnsafeList<EntityComponentStore.SharedComponentInfo>)].IsCreated)
				{
					this.m_UnmanagedSharedComponentInfo.Ptr[(IntPtr)j * (IntPtr)sizeof(UnsafeList<EntityComponentStore.SharedComponentInfo>) / (IntPtr)sizeof(UnsafeList<EntityComponentStore.SharedComponentInfo>)].Clear();
					ref UnsafeList<EntityComponentStore.SharedComponentInfo> ptr = ref this.m_UnmanagedSharedComponentInfo.Ptr[(IntPtr)j * (IntPtr)sizeof(UnsafeList<EntityComponentStore.SharedComponentInfo>) / (IntPtr)sizeof(UnsafeList<EntityComponentStore.SharedComponentInfo>)];
					EntityComponentStore.SharedComponentInfo sharedComponentInfo = default(EntityComponentStore.SharedComponentInfo);
					sharedComponentInfo.RefCount = 1;
					sharedComponentInfo.ComponentType = -1;
					sharedComponentInfo.Version = 1;
					sharedComponentInfo.HashCode = 0;
					ptr.Add(in sharedComponentInfo);
				}
			}
			this.m_HashLookup.Clear();
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x00022690 File Offset: 0x00020890
		public unsafe void FreeAllEntities()
		{
			UnsafePtrList<Archetype> archetypes = this.m_Archetypes;
			int i = 0;
			int length = archetypes.Length;
			while (i < length)
			{
				Archetype* ptr = *(IntPtr*)(this.m_Archetypes.Ptr + (IntPtr)i * (IntPtr)sizeof(Archetype*) / (IntPtr)sizeof(Archetype*));
				int j = 0;
				int count = ptr->Chunks.Count;
				while (j < count)
				{
					ChunkIndex chunkIndex = ptr->Chunks[j];
					Entity* buffer = (Entity*)chunkIndex.Buffer;
					int count2 = chunkIndex.Count;
					EntityComponentStore.s_entityStore.Data.DeallocateEntities(buffer, count2);
					j++;
				}
				i++;
			}
			this.m_NameStoreAccess.ResetEntitiesWithNamesSet();
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x0002272C File Offset: 0x0002092C
		public unsafe void FreeEntities(ChunkIndex chunk)
		{
			int count = chunk.Count;
			Entity* buffer = (Entity*)chunk.Buffer;
			EntityComponentStore.s_entityStore.Data.DeallocateEntities(buffer, count);
			this.m_NameStoreAccess.RemoveEntityWithNameSet(buffer, count);
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x00022768 File Offset: 0x00020968
		public string GetName(Entity entity)
		{
			if (!this.Exists(entity))
			{
				return "ENTITY_NOT_FOUND";
			}
			return this.m_NameStoreAccess.GetEntityName(entity).ToString();
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x000227A0 File Offset: 0x000209A0
		[GenerateTestsForBurstCompatibility]
		public void GetName(Entity entity, out FixedString64Bytes name)
		{
			if (!this.Exists(entity))
			{
				name = "ENTITY_NOT_FOUND";
				return;
			}
			name = default(FixedString64Bytes);
			this.m_NameStoreAccess.GetEntityName(entity).ToFixedString(ref name);
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x000227E4 File Offset: 0x000209E4
		public unsafe static string Debugger_GetName(EntityComponentStore* store, Entity entity)
		{
			if (store != null && store->m_NameStoreAccess.IsCreated && !EntityComponentStore.Debugger_Exists(store, entity))
			{
				return "ENTITY_NOT_FOUND";
			}
			return store->m_NameStoreAccess.GetEntityName(entity).ToString();
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x0002282C File Offset: 0x00020A2C
		[GenerateTestsForBurstCompatibility]
		public void SetName(Entity entity, in FixedString64Bytes name)
		{
			if (!this.Exists(entity))
			{
				return;
			}
			EntityName entityName = default(EntityName);
			entityName.SetFixedString(in name);
			this.m_NameStoreAccess.SetEntityName(entity, entityName);
			this.m_NameStoreAccess.AddEntityWithNameSet(entity);
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x0002286C File Offset: 0x00020A6C
		public void CopyName(Entity dstEntity, Entity srcEntity)
		{
			EntityName entityName = this.m_NameStoreAccess.GetEntityName(srcEntity);
			this.m_NameStoreAccess.SetEntityName(dstEntity, entityName);
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x00022894 File Offset: 0x00020A94
		public unsafe Archetype* GetArchetype(Entity entity)
		{
			ChunkIndex chunk = this.GetChunk(entity);
			return this.GetArchetype(chunk);
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x000228B0 File Offset: 0x00020AB0
		public ChunkIndex GetChunk(Entity entity)
		{
			return EntityComponentStore.s_entityStore.Data.GetEntityInChunk(entity).Chunk;
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x000228C7 File Offset: 0x00020AC7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe Archetype* GetArchetype(ChunkIndex chunk)
		{
			return EntityComponentStore.PerChunkArray.ChunkData[chunk].Archetype;
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x000228E2 File Offset: 0x00020AE2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void SetArchetype(ChunkIndex chunk, Archetype* archetype)
		{
			EntityComponentStore.PerChunkArray.ChunkData[chunk].Archetype = archetype;
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x000228FE File Offset: 0x00020AFE
		public void SetEntityInChunk(Entity entity, EntityInChunk entityInChunk)
		{
			EntityComponentStore.s_entityStore.Data.SetEntityInChunk(entity, entityInChunk);
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x00022911 File Offset: 0x00020B11
		public EntityInChunk GetEntityInChunk(Entity entity)
		{
			return EntityComponentStore.s_entityStore.Data.GetEntityInChunk(entity);
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x00022924 File Offset: 0x00020B24
		public unsafe void IncrementComponentTypeOrderVersion(Archetype* archetype)
		{
			for (int i = 0; i < archetype->TypesCount; i++)
			{
				TypeIndex typeIndex = archetype->Types[i].TypeIndex;
				this.m_ComponentTypeOrderVersion[typeIndex.Index]++;
			}
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x00022970 File Offset: 0x00020B70
		public unsafe bool Exists(Entity entity)
		{
			if (!EntityComponentStore.s_entityStore.Data.Exists(entity))
			{
				return false;
			}
			ChunkIndex chunk = this.GetChunk(entity);
			return this.GetArchetype(chunk)->EntityComponentStore->WorldSequenceNumber == this.WorldSequenceNumber;
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x000229B2 File Offset: 0x00020BB2
		public unsafe static bool Debugger_Exists(EntityComponentStore* store, Entity entity)
		{
			return EntityComponentStore.s_entityStore.Data.Exists(entity);
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x000229C4 File Offset: 0x00020BC4
		public unsafe int GetComponentTypeOrderVersion(TypeIndex typeIndex)
		{
			return this.m_ComponentTypeOrderVersion[typeIndex.Index];
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x000229D8 File Offset: 0x00020BD8
		public bool HasComponent(Entity entity, TypeIndex type, out bool entityExists)
		{
			entityExists = this.Exists(entity);
			return !Hint.Unlikely(!entityExists) && ChunkDataUtility.GetIndexInTypeArray(this.GetArchetype(entity), type) != -1;
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x00022A04 File Offset: 0x00020C04
		internal unsafe bool HasComponent(Entity entity, TypeIndex type, ref LookupCache cache, out bool entityExists)
		{
			entityExists = this.Exists(entity);
			if (Hint.Unlikely(!entityExists))
			{
				return false;
			}
			Archetype* archetype = this.GetArchetype(entity);
			if (Hint.Unlikely(archetype != cache.Archetype))
			{
				cache.Update(archetype, type);
			}
			return cache.IndexInArchetype != -1;
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x00022A59 File Offset: 0x00020C59
		public bool HasComponent(Entity entity, ComponentType type, out bool entityExists)
		{
			entityExists = this.Exists(entity);
			return !Hint.Unlikely(!entityExists) && ChunkDataUtility.GetIndexInTypeArray(this.GetArchetype(entity), type.TypeIndex) != -1;
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x00022A8C File Offset: 0x00020C8C
		public unsafe void SetChunkComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(NativeArray<ArchetypeChunk> chunks, T componentData) where T : struct, ValueType, IComponentData
		{
			TypeIndex typeIndex = TypeManager.MakeChunkComponentTypeIndex(ComponentType.ReadWrite<T>().TypeIndex);
			ArchetypeChunk* unsafePtr = (ArchetypeChunk*)chunks.GetUnsafePtr<ArchetypeChunk>();
			this.SetChunkComponent(unsafePtr, chunks.Length, (void*)(&componentData), typeIndex);
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x00022AC4 File Offset: 0x00020CC4
		public unsafe void SetChunkComponent(ArchetypeChunk* chunks, int chunkCount, void* componentData, TypeIndex componentTypeIndex)
		{
			if (ComponentType.FromTypeIndex(componentTypeIndex).IsZeroSized)
			{
				return;
			}
			for (int i = 0; i < chunkCount; i++)
			{
				ChunkIndex chunk = chunks[i].m_Chunk;
				void* componentDataWithTypeRW = (void*)this.GetComponentDataWithTypeRW(chunk.MetaChunkEntity, componentTypeIndex, this.m_GlobalSystemVersion);
				int sizeInChunk = this.GetTypeInfo(componentTypeIndex).SizeInChunk;
				UnsafeUtility.MemCpy(componentDataWithTypeRW, componentData, (long)sizeInChunk);
			}
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x00022B2C File Offset: 0x00020D2C
		public void GetChunk(Entity entity, out ChunkIndex chunk, out int chunkIndex)
		{
			EntityInChunk entityInChunk = this.GetEntityInChunk(entity);
			chunk = entityInChunk.Chunk;
			chunkIndex = entityInChunk.IndexInChunk;
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x00022B58 File Offset: 0x00020D58
		public unsafe byte* GetComponentDataWithTypeRO(Entity entity, TypeIndex typeIndex)
		{
			EntityInChunk entityInChunk = this.GetEntityInChunk(entity);
			ChunkIndex chunk = entityInChunk.Chunk;
			int indexInChunk = entityInChunk.IndexInChunk;
			Archetype* archetype = this.GetArchetype(chunk);
			return ChunkDataUtility.GetComponentDataWithTypeRO(chunk, archetype, indexInChunk, typeIndex);
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x00022B8C File Offset: 0x00020D8C
		public unsafe static byte* Debugger_GetComponentDataWithTypeRO(EntityComponentStore* store, Entity entity, TypeIndex typeIndex)
		{
			if (!EntityComponentStore.Debugger_Exists(store, entity))
			{
				return null;
			}
			EntityInChunk entityInChunk = store->GetEntityInChunk(entity);
			ChunkIndex chunk = entityInChunk.Chunk;
			int indexInChunk = entityInChunk.IndexInChunk;
			Archetype* archetype = store->GetArchetype(chunk);
			int indexInTypeArray = ChunkDataUtility.GetIndexInTypeArray(archetype, typeIndex);
			if (indexInTypeArray == -1)
			{
				return null;
			}
			return ChunkDataUtility.GetComponentDataRO(chunk, archetype, indexInChunk, indexInTypeArray);
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x00022BDC File Offset: 0x00020DDC
		public unsafe byte* GetComponentDataWithTypeRW(Entity entity, TypeIndex typeIndex, uint globalVersion)
		{
			EntityInChunk entityInChunk = this.GetEntityInChunk(entity);
			ChunkIndex chunk = entityInChunk.Chunk;
			int indexInChunk = entityInChunk.IndexInChunk;
			Archetype* archetype = this.GetArchetype(chunk);
			return ChunkDataUtility.GetComponentDataWithTypeRW(chunk, archetype, indexInChunk, typeIndex, globalVersion);
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x00022C10 File Offset: 0x00020E10
		public unsafe byte* GetComponentDataWithTypeRO(Entity entity, TypeIndex typeIndex, ref LookupCache cache)
		{
			EntityInChunk entityInChunk = this.GetEntityInChunk(entity);
			return ChunkDataUtility.GetComponentDataWithTypeRO(entityInChunk.Chunk, this.GetArchetype(entityInChunk.Chunk), entityInChunk.IndexInChunk, typeIndex, ref cache);
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x00022C44 File Offset: 0x00020E44
		public unsafe byte* GetOptionalComponentDataWithTypeRO(Entity entity, TypeIndex typeIndex, ref LookupCache cache)
		{
			EntityInChunk entityInChunk = this.GetEntityInChunk(entity);
			return ChunkDataUtility.GetOptionalComponentDataWithTypeRO(entityInChunk.Chunk, this.GetArchetype(entityInChunk.Chunk), entityInChunk.IndexInChunk, typeIndex, ref cache);
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x00022C78 File Offset: 0x00020E78
		public unsafe byte* GetComponentDataWithTypeRW(Entity entity, TypeIndex typeIndex, uint globalVersion, ref LookupCache cache)
		{
			EntityInChunk entityInChunk = this.GetEntityInChunk(entity);
			return ChunkDataUtility.GetComponentDataWithTypeRW(entityInChunk.Chunk, this.GetArchetype(entityInChunk.Chunk), entityInChunk.IndexInChunk, typeIndex, globalVersion, ref cache);
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x00022CB0 File Offset: 0x00020EB0
		public unsafe byte* GetOptionalComponentDataWithTypeRW(Entity entity, TypeIndex typeIndex, uint globalVersion, ref LookupCache cache)
		{
			EntityInChunk entityInChunk = this.GetEntityInChunk(entity);
			return ChunkDataUtility.GetOptionalComponentDataWithTypeRW(entityInChunk.Chunk, this.GetArchetype(entityInChunk.Chunk), entityInChunk.IndexInChunk, typeIndex, globalVersion, ref cache);
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x00022CE6 File Offset: 0x00020EE6
		public unsafe void* GetComponentDataRawRW(Entity entity, TypeIndex typeIndex)
		{
			return this.GetComponentDataRawRWEntityHasComponent(entity, typeIndex);
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x00022CF0 File Offset: 0x00020EF0
		internal unsafe void* GetComponentDataRawRWEntityHasComponent(Entity entity, TypeIndex typeIndex)
		{
			return (void*)this.GetComponentDataWithTypeRW(entity, typeIndex, this.GlobalSystemVersion);
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x00022D00 File Offset: 0x00020F00
		public unsafe void SetComponentDataRawEntityHasComponent(Entity entity, TypeIndex typeIndex, void* data, int size)
		{
			UnsafeUtility.MemCpy((void*)this.GetComponentDataWithTypeRW(entity, typeIndex, this.GlobalSystemVersion), data, (long)size);
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x00022D19 File Offset: 0x00020F19
		public unsafe void SetBufferRaw(Entity entity, TypeIndex componentTypeIndex, BufferHeader* tempBuffer, int sizeInChunk)
		{
			byte* componentDataWithTypeRW = this.GetComponentDataWithTypeRW(entity, componentTypeIndex, this.GlobalSystemVersion);
			BufferHeader.Destroy((BufferHeader*)componentDataWithTypeRW);
			UnsafeUtility.MemCpy((void*)componentDataWithTypeRW, (void*)tempBuffer, (long)sizeInChunk);
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x00022D38 File Offset: 0x00020F38
		[GenerateTestsForBurstCompatibility]
		public unsafe int GetSharedComponentDataIndex(Entity entity, TypeIndex typeIndex)
		{
			Archetype* archetype = this.GetArchetype(entity);
			int indexInTypeArray = ChunkDataUtility.GetIndexInTypeArray(archetype, typeIndex);
			SharedComponentValues sharedComponentValues = archetype->Chunks.GetSharedComponentValues(this.GetChunk(entity).ListIndex);
			int num = indexInTypeArray - (int)archetype->FirstSharedComponent;
			return *sharedComponentValues[num];
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x00022D80 File Offset: 0x00020F80
		public unsafe int Debugger_GetSharedComponentDataIndex(Entity entity, TypeIndex typeIndex)
		{
			Archetype* archetype = this.GetArchetype(entity);
			int indexInTypeArray = ChunkDataUtility.GetIndexInTypeArray(archetype, typeIndex);
			if (indexInTypeArray == -1)
			{
				return -1;
			}
			SharedComponentValues sharedComponentValues = archetype->Chunks.GetSharedComponentValues(this.GetChunk(entity).ListIndex);
			int num = indexInTypeArray - (int)archetype->FirstSharedComponent;
			return *sharedComponentValues[num];
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x00022DD4 File Offset: 0x00020FD4
		public unsafe void AddExistingEntitiesInChunk(ChunkIndex chunk)
		{
			Archetype* archetype = this.GetArchetype(chunk);
			int i = 0;
			int count = chunk.Count;
			while (i < count)
			{
				Entity* componentDataRO = (Entity*)ChunkDataUtility.GetComponentDataRO(chunk, archetype, i, 0);
				EntityComponentStore.s_entityStore.Data.SetEntityInChunk(*componentDataRO, new EntityInChunk
				{
					Chunk = chunk,
					IndexInChunk = i
				});
				EntityComponentStore.s_entityStore.Data.SetEntityVersion(*componentDataRO, componentDataRO->Version);
				i++;
			}
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x00022E54 File Offset: 0x00021054
		public unsafe void AllocateEntitiesForRemapping(EntityComponentStore* srcEntityComponentStore, ref NativeArray<EntityRemapUtility.EntityRemapInfo> entityRemapping)
		{
			UnsafePtrList<Archetype> archetypes = srcEntityComponentStore->m_Archetypes;
			int i = 0;
			int length = archetypes.Length;
			while (i < length)
			{
				Archetype* ptr = *(IntPtr*)(archetypes.Ptr + (IntPtr)i * (IntPtr)sizeof(Archetype*) / (IntPtr)sizeof(Archetype*));
				int j = 0;
				int count = ptr->Chunks.Count;
				while (j < count)
				{
					ChunkIndex chunkIndex = ptr->Chunks[j];
					this.AllocateEntitiesForRemapping(chunkIndex, srcEntityComponentStore, ref entityRemapping);
					j++;
				}
				i++;
			}
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x00022EC8 File Offset: 0x000210C8
		public unsafe void AllocateEntitiesForRemapping(ChunkIndex chunk, EntityComponentStore* srcComponentStore, ref NativeArray<EntityRemapUtility.EntityRemapInfo> entityRemapping)
		{
			Entity* buffer = (Entity*)chunk.Buffer;
			int count = chunk.Count;
			Entity* ptr;
			checked
			{
				ptr = stackalloc Entity[unchecked((UIntPtr)128) * (UIntPtr)sizeof(Entity)];
				EntityComponentStore.s_entityStore.Data.AllocateEntities(ptr, count, ChunkIndex.Null, 0);
			}
			for (int i = 0; i < count; i++)
			{
				EntityRemapUtility.AddEntityRemapping(ref entityRemapping, buffer[i], ptr[i]);
				this.CopyName(ptr[i], buffer[i]);
			}
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x00022F64 File Offset: 0x00021164
		public unsafe void RemapChunk(ChunkIndex chunk, int baseIndex, int count, ref NativeArray<EntityRemapUtility.EntityRemapInfo> entityRemapping)
		{
			Entity* ptr = (Entity*)(chunk.Buffer + (IntPtr)baseIndex * (IntPtr)sizeof(Entity));
			for (int num = 0; num != count; num++)
			{
				Entity* ptr2 = ptr + num;
				Entity entity = EntityRemapUtility.RemapEntity(ref entityRemapping, *ptr2);
				*ptr2 = entity;
				this.SetEntityInChunk(entity, new EntityInChunk
				{
					Chunk = chunk,
					IndexInChunk = num
				});
			}
			if (chunk.MetaChunkEntity != Entity.Null)
			{
				chunk.MetaChunkEntity = EntityRemapUtility.RemapEntity(ref entityRemapping, chunk.MetaChunkEntity);
			}
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x00022FF9 File Offset: 0x000211F9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static bool IsUnmanagedSharedComponentIndex(int sharedComponentIndex)
		{
			return sharedComponentIndex == 0 || (sharedComponentIndex & int.MinValue) != 0;
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x0002300A File Offset: 0x0002120A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static TypeIndex GetComponentTypeFromSharedComponentIndex(int sharedComponentIndex)
		{
			return TypeManager.GetTypeInfoPointer()[(sharedComponentIndex & int.MaxValue) >> 16].TypeIndex;
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x00023029 File Offset: 0x00021229
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static int GetElementIndexFromSharedComponentIndex(int sharedComponentIndex)
		{
			return sharedComponentIndex & 65535;
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x00023032 File Offset: 0x00021232
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static int BuildUnmanagedSharedComponentDataIndex(int elementIndex, TypeIndex typeIndex)
		{
			return int.MinValue | (typeIndex.Value << 16) | elementIndex;
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x00023048 File Offset: 0x00021248
		private unsafe ComponentTypeList* CheckGetSharedComponentList(TypeIndex typeIndex)
		{
			int index = typeIndex.Index;
			ComponentTypeList* ptr = this.m_UnmanagedSharedComponentsByType.Ptr + index;
			if (!ptr->IsCreated)
			{
				int typeSize = TypeManager.GetTypeInfo(typeIndex).TypeSize;
				*ptr = new ComponentTypeList(typeSize, 16, 16 * typeSize, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				ptr->Length = 1;
				this.m_UnmanagedSharedComponentsByType.Length = Math.Max(this.m_UnmanagedSharedComponentsByType.Length, index + 1);
			}
			return ptr;
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x000230C8 File Offset: 0x000212C8
		private unsafe UnsafeList<EntityComponentStore.SharedComponentInfo>* CheckGetSharedComponentInfo(TypeIndex typeIndex)
		{
			int index = typeIndex.Index;
			UnsafeList<EntityComponentStore.SharedComponentInfo>* ptr = this.m_UnmanagedSharedComponentInfo.Ptr + (IntPtr)index * (IntPtr)sizeof(UnsafeList<EntityComponentStore.SharedComponentInfo>) / (IntPtr)sizeof(UnsafeList<EntityComponentStore.SharedComponentInfo>);
			if (!ptr->IsCreated)
			{
				*ptr = new UnsafeList<EntityComponentStore.SharedComponentInfo>(16, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				ref UnsafeList<EntityComponentStore.SharedComponentInfo> ptr2 = ref *ptr;
				EntityComponentStore.SharedComponentInfo sharedComponentInfo = default(EntityComponentStore.SharedComponentInfo);
				sharedComponentInfo.RefCount = 1;
				sharedComponentInfo.HashCode = 0;
				sharedComponentInfo.Version = 1;
				sharedComponentInfo.ComponentType = -1;
				ptr2.Add(in sharedComponentInfo);
				this.m_UnmanagedSharedComponentTypes.Add(in typeIndex);
			}
			return ptr;
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x0002314E File Offset: 0x0002134E
		private unsafe int FindSharedComponentIndex(TypeIndex typeIndex, int hashCode, void* newData, void* defaultValue, out UnsafeList<EntityComponentStore.SharedComponentInfo>* infos, out ComponentTypeList* components)
		{
			components = this.CheckGetSharedComponentList(typeIndex);
			infos = this.CheckGetSharedComponentInfo(typeIndex);
			if (newData == null)
			{
				return 0;
			}
			if (defaultValue != null && TypeManager.SharedComponentEquals(newData, defaultValue, typeIndex))
			{
				return 0;
			}
			return this.FindNonDefaultSharedComponentIndex(typeIndex, hashCode, newData, out infos, out components);
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x0002318C File Offset: 0x0002138C
		private unsafe int FindNonDefaultSharedComponentIndex(TypeIndex typeIndex, int hashCode, void* newData, out UnsafeList<EntityComponentStore.SharedComponentInfo>* infos, out ComponentTypeList* components)
		{
			if (hashCode == 0)
			{
				hashCode = TypeManager.SharedComponentGetHashCode(newData, typeIndex);
			}
			int num;
			NativeParallelMultiHashMapIterator<ulong> nativeParallelMultiHashMapIterator;
			if (!this.m_HashLookup.TryGetFirstValue(EntityComponentStore.GetSharedComponentHashKey(typeIndex, hashCode), out num, out nativeParallelMultiHashMapIterator))
			{
				infos = (IntPtr)((UIntPtr)0);
				components = (IntPtr)((UIntPtr)0);
				return -1;
			}
			int typeSize = TypeManager.GetTypeInfo(typeIndex).TypeSize;
			components = this.CheckGetSharedComponentList(typeIndex);
			infos = this.CheckGetSharedComponentInfo(typeIndex);
			while (!TypeManager.SharedComponentEquals((void*)((byte*)components.Ptr + num * typeSize), newData, typeIndex))
			{
				if (!this.m_HashLookup.TryGetNextValue(out num, ref nativeParallelMultiHashMapIterator))
				{
					return -1;
				}
			}
			return num;
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x00023212 File Offset: 0x00021412
		internal unsafe static ulong GetSharedComponentHashKey(TypeIndex typeIndex, int hashCode)
		{
			return ((ulong)TypeManager.GetTypeInfoPointer()[typeIndex.Index].TypeIndex << 32) | (ulong)hashCode;
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x0002323C File Offset: 0x0002143C
		internal unsafe int CloneSharedComponentNonDefault(EntityComponentStore* srcComponentStore, int sharedComponentIndex)
		{
			TypeIndex componentTypeFromSharedComponentIndex = EntityComponentStore.GetComponentTypeFromSharedComponentIndex(sharedComponentIndex);
			void* sharedComponentDataAddr_Unmanaged = srcComponentStore->GetSharedComponentDataAddr_Unmanaged(sharedComponentIndex, componentTypeFromSharedComponentIndex);
			int elementIndexFromSharedComponentIndex = EntityComponentStore.GetElementIndexFromSharedComponentIndex(sharedComponentIndex);
			UnsafeList<EntityComponentStore.SharedComponentInfo>* ptr = srcComponentStore->CheckGetSharedComponentInfo(componentTypeFromSharedComponentIndex);
			return this.InsertSharedComponent_Unmanaged(componentTypeFromSharedComponentIndex, ptr->Ptr[elementIndexFromSharedComponentIndex].HashCode, sharedComponentDataAddr_Unmanaged, null);
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x00023288 File Offset: 0x00021488
		internal unsafe int InsertSharedComponent_Unmanaged(TypeIndex typeIndex, int hashCode, void* data, void* defaultValue)
		{
			if (hashCode == 0)
			{
				hashCode = ((data != null) ? TypeManager.SharedComponentGetHashCode(data, typeIndex) : 0);
			}
			UnsafeList<EntityComponentStore.SharedComponentInfo>* ptr;
			ComponentTypeList* ptr2;
			int num = this.FindSharedComponentIndex(typeIndex, hashCode, data, defaultValue, out ptr, out ptr2);
			if (num == 0)
			{
				return 0;
			}
			if (num != -1)
			{
				ptr->Ptr[num].RefCount++;
				return EntityComponentStore.BuildUnmanagedSharedComponentDataIndex(num, typeIndex);
			}
			if (typeIndex.IsRefCounted)
			{
				TypeManager.CallIRefCounted_Retain(typeIndex, (IntPtr)data);
			}
			int typeSize = TypeManager.GetTypeInfo(typeIndex).TypeSize;
			ptr = this.CheckGetSharedComponentInfo(typeIndex);
			ptr2 = this.CheckGetSharedComponentList(typeIndex);
			EntityComponentStore.SharedComponentInfo sharedComponentInfo = default(EntityComponentStore.SharedComponentInfo);
			sharedComponentInfo.RefCount = 1;
			sharedComponentInfo.HashCode = hashCode;
			sharedComponentInfo.ComponentType = typeIndex.Value;
			int sharedComponentVersion = this.m_SharedComponentVersion;
			this.m_SharedComponentVersion = sharedComponentVersion + 1;
			sharedComponentInfo.Version = sharedComponentVersion;
			EntityComponentStore.SharedComponentInfo sharedComponentInfo2 = sharedComponentInfo;
			num = ptr->Ptr->ComponentType;
			if (num != -1)
			{
				ptr->Ptr->ComponentType = ptr->Ptr[num].ComponentType;
				UnsafeUtility.MemCpy((void*)((byte*)ptr2->Ptr + num * typeSize), data, (long)typeSize);
				UnsafeUtility.WriteArrayElement<EntityComponentStore.SharedComponentInfo>((void*)ptr->Ptr, num, sharedComponentInfo2);
			}
			else
			{
				num = ptr2->Length;
				ptr2->Resize(typeSize, 16, ptr2->Length + 1, NativeArrayOptions.UninitializedMemory);
				UnsafeUtility.MemCpy((void*)((byte*)ptr2->Ptr + num * typeSize), data, (long)typeSize);
				ptr->Add(in sharedComponentInfo2);
			}
			this.m_HashLookup.Add(EntityComponentStore.GetSharedComponentHashKey(typeIndex, hashCode), num);
			this.m_UnmanagedSharedComponentCount++;
			return EntityComponentStore.BuildUnmanagedSharedComponentDataIndex(num, typeIndex);
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x00023406 File Offset: 0x00021606
		[GenerateTestsForBurstCompatibility]
		internal int GetUnmanagedSharedComponentCount()
		{
			return this.m_UnmanagedSharedComponentCount;
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x00023410 File Offset: 0x00021610
		[GenerateTestsForBurstCompatibility]
		internal unsafe void AddSharedComponentReference_Unmanaged(int sharedComponentIndex, int numRefs = 1)
		{
			int elementIndexFromSharedComponentIndex = EntityComponentStore.GetElementIndexFromSharedComponentIndex(sharedComponentIndex);
			if (elementIndexFromSharedComponentIndex == 0)
			{
				return;
			}
			TypeIndex componentTypeFromSharedComponentIndex = EntityComponentStore.GetComponentTypeFromSharedComponentIndex(sharedComponentIndex);
			this.CheckGetSharedComponentInfo(componentTypeFromSharedComponentIndex)->Ptr[elementIndexFromSharedComponentIndex].RefCount += numRefs;
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x00023450 File Offset: 0x00021650
		internal unsafe void GetSharedComponentData_Unmanaged(int sharedComponentIndex, TypeIndex typeIndex, void* destinationAddress)
		{
			int typeSize = TypeManager.GetTypeInfo(typeIndex).TypeSize;
			void* sharedComponentDataAddr_Unmanaged = this.GetSharedComponentDataAddr_Unmanaged(sharedComponentIndex, typeIndex);
			UnsafeUtility.MemCpy(destinationAddress, sharedComponentDataAddr_Unmanaged, (long)typeSize);
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x0002347C File Offset: 0x0002167C
		internal unsafe void* GetSharedComponentDataAddr_Unmanaged(int sharedComponentIndex, TypeIndex typeIndex)
		{
			if (typeIndex == TypeIndex.Null)
			{
				return null;
			}
			int elementIndexFromSharedComponentIndex = EntityComponentStore.GetElementIndexFromSharedComponentIndex(sharedComponentIndex);
			int index = typeIndex.Index;
			if (sharedComponentIndex == 0)
			{
				this.CheckGetSharedComponentList(typeIndex);
			}
			ref ComponentTypeList ptr = ref this.m_UnmanagedSharedComponentsByType.Ptr[index];
			int typeSize = TypeManager.GetTypeInfo(typeIndex).TypeSize;
			return (void*)((byte*)ptr.Ptr + elementIndexFromSharedComponentIndex * typeSize);
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x000234E0 File Offset: 0x000216E0
		internal unsafe object GetSharedComponentDataObject_Unmanaged(int sharedComponentIndex, TypeIndex typeIndex)
		{
			void* sharedComponentDataAddr_Unmanaged = this.GetSharedComponentDataAddr_Unmanaged(sharedComponentIndex, typeIndex);
			return TypeManager.ConstructComponentFromBuffer(typeIndex, sharedComponentDataAddr_Unmanaged);
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x00023500 File Offset: 0x00021700
		[GenerateTestsForBurstCompatibility]
		internal unsafe void RemoveSharedComponentReference_Unmanaged(int sharedComponentIndex, int numRefs = 1)
		{
			TypeIndex componentTypeFromSharedComponentIndex = EntityComponentStore.GetComponentTypeFromSharedComponentIndex(sharedComponentIndex);
			int elementIndexFromSharedComponentIndex = EntityComponentStore.GetElementIndexFromSharedComponentIndex(sharedComponentIndex);
			if (elementIndexFromSharedComponentIndex == 0)
			{
				return;
			}
			UnsafeList<EntityComponentStore.SharedComponentInfo>* ptr = this.CheckGetSharedComponentInfo(componentTypeFromSharedComponentIndex);
			if ((ptr->Ptr[elementIndexFromSharedComponentIndex].RefCount -= numRefs) != 0)
			{
				return;
			}
			if (componentTypeFromSharedComponentIndex.IsRefCounted)
			{
				void* sharedComponentDataAddr_Unmanaged = this.GetSharedComponentDataAddr_Unmanaged(sharedComponentIndex, componentTypeFromSharedComponentIndex);
				TypeManager.CallIRefCounted_Release(componentTypeFromSharedComponentIndex, (IntPtr)sharedComponentDataAddr_Unmanaged);
			}
			this.IncrementSharedComponentVersion_Unmanaged(0);
			int hashCode = ptr->Ptr[elementIndexFromSharedComponentIndex].HashCode;
			ptr->Ptr[elementIndexFromSharedComponentIndex].ComponentType = ptr->Ptr->ComponentType;
			ptr->Ptr->ComponentType = elementIndexFromSharedComponentIndex;
			int num;
			NativeParallelMultiHashMapIterator<ulong> nativeParallelMultiHashMapIterator;
			if (this.m_HashLookup.TryGetFirstValue(EntityComponentStore.GetSharedComponentHashKey(componentTypeFromSharedComponentIndex, hashCode), out num, out nativeParallelMultiHashMapIterator))
			{
				while (num != elementIndexFromSharedComponentIndex)
				{
					if (!this.m_HashLookup.TryGetNextValue(out num, ref nativeParallelMultiHashMapIterator))
					{
						return;
					}
				}
				this.m_HashLookup.Remove(nativeParallelMultiHashMapIterator);
				this.m_UnmanagedSharedComponentCount--;
				return;
			}
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x000235FC File Offset: 0x000217FC
		[GenerateTestsForBurstCompatibility]
		internal unsafe void IncrementSharedComponentVersion_Unmanaged(int sharedComponentIndex)
		{
			int num = this.m_SharedComponentVersion + 1;
			this.m_SharedComponentVersion = num;
			int num2 = num;
			if (sharedComponentIndex == 0)
			{
				this.m_SharedComponentGlobalVersion = num2;
				return;
			}
			TypeIndex componentTypeFromSharedComponentIndex = EntityComponentStore.GetComponentTypeFromSharedComponentIndex(sharedComponentIndex);
			int elementIndexFromSharedComponentIndex = EntityComponentStore.GetElementIndexFromSharedComponentIndex(sharedComponentIndex);
			this.CheckGetSharedComponentInfo(componentTypeFromSharedComponentIndex)->Ptr[elementIndexFromSharedComponentIndex].Version = num2;
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x00023650 File Offset: 0x00021850
		internal unsafe int GetSharedComponentVersion_Unmanaged(TypeIndex typeIndex, void* sharedData, void* defaultData)
		{
			UnsafeList<EntityComponentStore.SharedComponentInfo>* ptr;
			ComponentTypeList* ptr2;
			int num = this.FindSharedComponentIndex(typeIndex, 0, sharedData, defaultData, out ptr, out ptr2);
			if (num <= 0)
			{
				return this.m_SharedComponentGlobalVersion;
			}
			return ptr->Ptr[num].Version;
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x0002368C File Offset: 0x0002188C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleSharedComponentData) })]
		internal unsafe void GetAllUniqueSharedComponents_Unmanaged<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(out UnsafeList<T> sharedComponentValues, AllocatorManager.AllocatorHandle allocator) where T : struct, ValueType, ISharedComponentData
		{
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			T t = default(T);
			ComponentTypeList* ptr = this.CheckGetSharedComponentList(typeIndex);
			UnsafeList<EntityComponentStore.SharedComponentInfo>* ptr2 = this.CheckGetSharedComponentInfo(typeIndex);
			int typeSize = TypeManager.GetTypeInfo<T>().TypeSize;
			sharedComponentValues = new UnsafeList<T>(ptr->Length, allocator, NativeArrayOptions.UninitializedMemory);
			sharedComponentValues.Add(in t);
			int i = 1;
			int num = 1;
			while (i < ptr2->Length)
			{
				if (ptr2->Ptr[i].RefCount > 0)
				{
					UnsafeUtility.MemCpy((void*)(sharedComponentValues.Ptr + num * typeSize / sizeof(T)), (void*)((byte*)ptr->Ptr + i * typeSize), (long)typeSize);
					int length = sharedComponentValues.Length;
					sharedComponentValues.Length = length + 1;
					num++;
				}
				i++;
			}
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x00023748 File Offset: 0x00021948
		[GenerateTestsForBurstCompatibility]
		internal unsafe void GetAllUniqueSharedComponents_Unmanaged(TypeIndex typeIndex, void* defaultValue, out ComponentTypeList sharedComponentValues, out UnsafeList<int> sharedComponentIndices, AllocatorManager.AllocatorHandle allocator)
		{
			ComponentTypeList* ptr = this.CheckGetSharedComponentList(typeIndex);
			UnsafeList<EntityComponentStore.SharedComponentInfo>* ptr2 = this.CheckGetSharedComponentInfo(typeIndex);
			int typeSize = TypeManager.GetTypeInfo(typeIndex).TypeSize;
			sharedComponentValues = new ComponentTypeList(typeSize, 16, ptr->Length * typeSize, allocator, NativeArrayOptions.UninitializedMemory);
			sharedComponentIndices = new UnsafeList<int>(ptr->Length, allocator, NativeArrayOptions.UninitializedMemory);
			UnsafeUtility.MemCpy(sharedComponentValues.Ptr, defaultValue, (long)typeSize);
			sharedComponentValues.Length++;
			int num = 0;
			sharedComponentIndices.Add(in num);
			int i = 1;
			int num2 = 1;
			while (i < ptr2->Length)
			{
				if (ptr2->Ptr[i].RefCount > 0)
				{
					UnsafeUtility.MemCpy((void*)((byte*)sharedComponentValues.Ptr + num2 * typeSize), (void*)((byte*)ptr->Ptr + i * typeSize), (long)typeSize);
					sharedComponentValues.Length++;
					num = EntityComponentStore.BuildUnmanagedSharedComponentDataIndex(i, typeIndex);
					sharedComponentIndices.Add(in num);
					num2++;
				}
				i++;
			}
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x00023834 File Offset: 0x00021A34
		internal unsafe void MoveAllSharedComponents_Unmanaged(EntityComponentStore* srcEntityComponentStore, ref NativeParallelHashMap<int, int> result)
		{
			int length = srcEntityComponentStore->m_UnmanagedSharedComponentTypes.Length;
			for (int i = 0; i < length; i++)
			{
				TypeIndex typeIndex = srcEntityComponentStore->m_UnmanagedSharedComponentTypes[i];
				int typeSize = TypeManager.GetTypeInfo(typeIndex).TypeSize;
				ComponentTypeList* ptr = srcEntityComponentStore->CheckGetSharedComponentList(typeIndex);
				UnsafeList<EntityComponentStore.SharedComponentInfo>* ptr2 = srcEntityComponentStore->CheckGetSharedComponentInfo(typeIndex);
				for (int j = 1; j < ptr->Length; j++)
				{
					int num = this.InsertSharedComponent_Unmanaged(new TypeIndex
					{
						Value = typeIndex.Index
					}, ptr2->Ptr[j].HashCode, (void*)((byte*)ptr->Ptr + j * typeSize), null);
					this.AddSharedComponentReference_Unmanaged(num, ptr2->Ptr[j].RefCount - 1);
					this.IncrementSharedComponentVersion_Unmanaged(num);
					result.Add(EntityComponentStore.BuildUnmanagedSharedComponentDataIndex(j, typeIndex), num);
				}
			}
			srcEntityComponentStore->ResetSharedComponentData();
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x0002392C File Offset: 0x00021B2C
		public unsafe bool AllSharedComponentReferencesAreFromChunks(UnsafeParallelHashMap<int, int> refCountMap)
		{
			int length = this.m_UnmanagedSharedComponentTypes.Length;
			for (int i = 0; i < length; i++)
			{
				TypeIndex typeIndex = this.m_UnmanagedSharedComponentTypes[i];
				UnsafeList<EntityComponentStore.SharedComponentInfo>* ptr = this.CheckGetSharedComponentInfo(typeIndex);
				for (int j = 1; j < ptr->Length; j++)
				{
					int num = EntityComponentStore.BuildUnmanagedSharedComponentDataIndex(j, typeIndex);
					int num2;
					if (refCountMap.TryGetValue(num, out num2) && ptr->Ptr[j].RefCount != num2)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x000239AF File Offset: 0x00021BAF
		[BurstCompile]
		private unsafe static void EntityBatchFromEntityChunkDataShared(EntityComponentStore* ecs, in EntityInChunk* chunkData, int chunkCount, EntityBatchInChunk* entityBatchList, int* currentbatchIndex, int nSharedComponentsToAdd, int* foundError)
		{
			EntityComponentStore.EntityBatchFromEntityChunkDataShared_00000639$BurstDirectCall.Invoke(ecs, in chunkData, chunkCount, entityBatchList, currentbatchIndex, nSharedComponentsToAdd, foundError);
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x000239C0 File Offset: 0x00021BC0
		[BurstCompile]
		private unsafe static void SortEntityInChunk(EntityInChunk* entityInChunks, int count)
		{
			EntityComponentStore.SortEntityInChunk_0000063A$BurstDirectCall.Invoke(entityInChunks, count);
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x000239C9 File Offset: 0x00021BC9
		[BurstCompile]
		private unsafe static void GatherEntityInChunkForEntities(Entity* Entities, EntityInChunk* EntityChunkData, int numEntities)
		{
			EntityComponentStore.GatherEntityInChunkForEntities_0000063B$BurstDirectCall.Invoke(Entities, EntityChunkData, numEntities);
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x000239D4 File Offset: 0x00021BD4
		internal unsafe bool CreateEntityBatchList(NativeArray<Entity> entities, int nSharedComponentsToAdd, AllocatorManager.AllocatorHandle allocator, out NativeList<EntityBatchInChunk> entityBatchList)
		{
			if (entities.Length == 0)
			{
				entityBatchList = default(NativeList<EntityBatchInChunk>);
				return false;
			}
			NativeArray<EntityInChunk> nativeArray = new NativeArray<EntityInChunk>(entities.Length, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
			EntityComponentStore.GatherEntityInChunkForEntities((Entity*)entities.GetUnsafeReadOnlyPtr<Entity>(), (EntityInChunk*)nativeArray.GetUnsafePtr<EntityInChunk>(), entities.Length);
			EntityComponentStore.SortEntityInChunk((EntityInChunk*)nativeArray.GetUnsafePtr<EntityInChunk>(), nativeArray.Length);
			entityBatchList = new NativeList<EntityBatchInChunk>(nativeArray.Length, allocator);
			entityBatchList.Length = nativeArray.Length;
			int num = 0;
			int num2 = 0;
			fixed (EntityComponentStore* ptr = &this)
			{
				EntityComponentStore* ptr2 = ptr;
				EntityInChunk* unsafePtr = (EntityInChunk*)nativeArray.GetUnsafePtr<EntityInChunk>();
				EntityComponentStore.EntityBatchFromEntityChunkDataShared(ptr2, in unsafePtr, nativeArray.Length, entityBatchList.GetUnsafePtr<EntityBatchInChunk>(), &num2, nSharedComponentsToAdd, &num);
			}
			entityBatchList.Length = num2;
			nativeArray.Dispose();
			if (num != 0)
			{
				entityBatchList.Dispose();
				entityBatchList = default(NativeList<EntityBatchInChunk>);
				return false;
			}
			return true;
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x00023AA8 File Offset: 0x00021CA8
		public ulong AllocateSequenceNumber()
		{
			this.m_NextChunkSequenceNumber += 1UL;
			return this.m_NextChunkSequenceNumber;
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x00023ABF File Offset: 0x00021CBF
		public static int AllocateContiguousChunk(int requestedCount, out ChunkIndex chunk, out int actualCount)
		{
			return EntityComponentStore.s_chunkStore.Data.AllocateContiguousChunks(out chunk, requestedCount, out actualCount);
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x00023AD3 File Offset: 0x00021CD3
		public static int FreeContiguousChunks(ChunkIndex firstChunk, int count)
		{
			return EntityComponentStore.s_chunkStore.Data.FreeContiguousChunks(firstChunk, count);
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x00023AE8 File Offset: 0x00021CE8
		public unsafe ChunkIndex AllocateChunk()
		{
			ChunkIndex chunkIndex;
			int num;
			if (EntityComponentStore.s_chunkStore.Data.AllocateContiguousChunks(out chunkIndex, 1, out num) != EntityComponentStore.ChunkStore.kErrorNone)
			{
				throw new InvalidOperationException("ChunkStore.AllocateContiguousChunks failed");
			}
			if (this.useMemoryInitPattern != 0)
			{
				UnsafeUtility.MemSet((void*)chunkIndex.GetPtr(), this.memoryInitPattern, 16384L);
			}
			return chunkIndex;
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x00023B3C File Offset: 0x00021D3C
		public void FreeChunk(ChunkIndex chunk)
		{
			EntityComponentStore.s_chunkStore.Data.FreeContiguousChunks(chunk, 1);
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x00023B50 File Offset: 0x00021D50
		public unsafe Archetype* GetExistingArchetype(ComponentTypeInArchetype* typesSorted, int count)
		{
			return this.m_TypeLookup.TryGet(typesSorted, count);
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x00023B60 File Offset: 0x00021D60
		private unsafe void ChunkAllocate<T>(void* pointer, int count = 1) where T : struct
		{
			*(IntPtr*)pointer = this.m_ArchetypeChunkAllocator.Allocate(UnsafeUtility.SizeOf<T>() * count, UnsafeUtility.AlignOf<T>());
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x00023B88 File Offset: 0x00021D88
		internal static int GetComponentArraySize(int componentSize, int entityCount)
		{
			return CollectionHelper.Align(componentSize * entityCount, 64);
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x00023B94 File Offset: 0x00021D94
		private unsafe static int CalculateSpaceRequirement(ushort* componentSizes, int componentCount, int entityCount)
		{
			int num = 0;
			for (int i = 0; i < componentCount; i++)
			{
				num += EntityComponentStore.GetComponentArraySize((int)componentSizes[i], entityCount);
			}
			return num;
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x00023BC0 File Offset: 0x00021DC0
		private unsafe static int CalculateChunkCapacity(int bufferSize, ushort* componentSizes, int count)
		{
			int num = 0;
			for (int i = 0; i < count; i++)
			{
				num += (int)componentSizes[i];
			}
			int num2 = bufferSize / num;
			while (EntityComponentStore.CalculateSpaceRequirement(componentSizes, count, num2) > bufferSize)
			{
				num2--;
			}
			return num2;
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x00023BFC File Offset: 0x00021DFC
		private unsafe static ulong CalculateStableHash(ComponentTypeInArchetype* types, int* typeMemoryOrder, int count)
		{
			ulong num = 1UL;
			for (int i = 0; i < count; i++)
			{
				ulong stableTypeHash = TypeManager.GetTypeInfo(types[typeMemoryOrder[i]].TypeIndex).StableTypeHash;
				num = TypeHash.CombineFNV1A64(num, stableTypeHash);
			}
			return num;
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x00023C42 File Offset: 0x00021E42
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void ThrowIfComponentDataSizeIsLargerThanShortMaxValue(int sizeInChunk)
		{
			if (sizeInChunk > 32767)
			{
				throw new ArgumentException(string.Format("Component Data sizes may not be larger than {0}", short.MaxValue));
			}
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x00023C68 File Offset: 0x00021E68
		internal unsafe Archetype* CreateArchetype(ComponentTypeInArchetype* types, int count)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int* ptr;
			int num5;
			checked
			{
				ptr = stackalloc int[unchecked((UIntPtr)count) * 4];
				num5 = 0;
			}
			for (int i = 0; i < count; i++)
			{
				readonly ref TypeManager.TypeInfo typeInfo = ref this.GetTypeInfo(types[i].TypeIndex);
				if (typeInfo.Category == TypeManager.TypeCategory.ISharedComponentData)
				{
					num4++;
				}
				else if (TypeManager.IsManagedComponent(types[i].TypeIndex))
				{
					num3++;
				}
				else if (typeInfo.BufferCapacity >= 0)
				{
					num2 += typeInfo.EntityOffsetCount;
				}
				else if (typeInfo.SizeInChunk > 0)
				{
					num += typeInfo.EntityOffsetCount;
				}
				if (types[i].IsEnableable)
				{
					ptr[(IntPtr)(num5++) * 4] = i;
				}
			}
			Archetype* ptr2 = null;
			this.ChunkAllocate<Archetype>((void*)(&ptr2), 1);
			this.ChunkAllocate<ComponentTypeInArchetype>((void*)(&ptr2->Types), count);
			this.ChunkAllocate<int>((void*)(&ptr2->EnableableTypeIndexInArchetype), num5);
			this.ChunkAllocate<int>((void*)(&ptr2->Offsets), count);
			this.ChunkAllocate<int>((void*)(&ptr2->SizeOfs), count);
			this.ChunkAllocate<int>((void*)(&ptr2->BufferCapacities), count);
			this.ChunkAllocate<int>((void*)(&ptr2->TypeMemoryOrderIndexToIndexInArchetype), count);
			this.ChunkAllocate<int>((void*)(&ptr2->TypeIndexInArchetypeToMemoryOrderIndex), count);
			this.ChunkAllocate<EntityRemapUtility.EntityPatchInfo>((void*)(&ptr2->ScalarEntityPatches), num);
			this.ChunkAllocate<EntityRemapUtility.BufferEntityPatchInfo>((void*)(&ptr2->BufferEntityPatches), num2);
			ptr2->TypesCount = count;
			Memory.Array.Copy<ComponentTypeInArchetype>(ptr2->Types, types, (long)count);
			ptr2->EnableableTypesCount = num5;
			Memory.Array.Copy<int>(ptr2->EnableableTypeIndexInArchetype, ptr, (long)num5);
			ptr2->EntityCount = 0;
			ptr2->ChunksWithEmptySlots = new UnsafeList<ChunkIndex>(0, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			ptr2->MatchingQueryData = new UnsafeList<IntPtr>(0, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			ptr2->NextChangedArchetype = null;
			ptr2->InstantiateArchetype = null;
			ptr2->CopyArchetype = null;
			ptr2->MetaChunkArchetype = null;
			ptr2->CleanupResidueArchetype = null;
			ptr2->Flags = (ArchetypeFlags)0;
			short num6 = (short)count;
			do
			{
				ptr2->FirstChunkComponent = num6;
			}
			while (types[num6 -= 1].IsChunkComponent);
			num6 += 1;
			do
			{
				ptr2->FirstSharedComponent = num6;
			}
			while (types[num6 -= 1].IsSharedComponent);
			num6 += 1;
			do
			{
				ptr2->FirstTagComponent = num6;
			}
			while (types[num6 -= 1].IsZeroSized);
			num6 += 1;
			do
			{
				ptr2->FirstManagedComponent = num6;
			}
			while (types[num6 -= 1].IsManagedComponent);
			num6 += 1;
			do
			{
				ptr2->FirstBufferComponent = num6;
			}
			while (types[num6 -= 1].IsBuffer);
			ulong num7 = 0UL;
			for (int j = 0; j < count; j++)
			{
				TypeIndex typeIndex = types[j].TypeIndex;
				readonly ref TypeManager.TypeInfo typeInfo2 = ref this.GetTypeInfo(typeIndex);
				num7 |= typeInfo2.BloomFilterMask;
				if (typeIndex == this.m_DisabledType)
				{
					ptr2->Flags |= ArchetypeFlags.Disabled;
				}
				if (typeIndex == this.m_SystemInstanceType)
				{
					ptr2->Flags |= ArchetypeFlags.HasSystemInstanceComponents;
				}
				if (typeIndex == this.m_PrefabType)
				{
					ptr2->Flags |= ArchetypeFlags.Prefab;
				}
				if (typeIndex == this.m_ChunkHeaderType)
				{
					ptr2->Flags |= ArchetypeFlags.HasChunkHeader;
				}
				if (typeInfo2.HasBlobAssetRefs)
				{
					ptr2->Flags |= ArchetypeFlags.HasBlobAssetRefs;
				}
				if (!types[j].IsChunkComponent && types[j].IsManagedComponent && typeInfo2.Category == TypeManager.TypeCategory.UnityEngineObject)
				{
					ptr2->Flags |= ArchetypeFlags.HasCompanionComponents;
				}
				if (types[j].IsManagedComponent && TypeManager.HasEntityReferences(typeIndex))
				{
					ptr2->Flags |= ArchetypeFlags.HasManagedEntityRefs;
				}
				if (typeInfo2.HasWeakAssetRefs)
				{
					ptr2->Flags |= ArchetypeFlags.HasWeakAssetRefs;
				}
				if (typeInfo2.HasUnityObjectRefs)
				{
					ptr2->Flags |= ArchetypeFlags.HasUnityObjectRefs;
				}
			}
			ptr2->BloomFilterMask = num7;
			if (ptr2->NumManagedComponents > 0)
			{
				ptr2->Flags |= ArchetypeFlags.HasManagedComponents;
			}
			if (ptr2->NumBufferComponents > 0)
			{
				ptr2->Flags |= ArchetypeFlags.HasBufferComponents;
			}
			int num8 = 16320;
			ptr2->ScalarEntityPatchCount = num;
			ptr2->BufferEntityPatchCount = num2;
			int num9 = 128;
			for (int k = 0; k < count; k++)
			{
				readonly ref TypeManager.TypeInfo typeInfo3 = ref this.GetTypeInfo(types[k].TypeIndex);
				if (k < ptr2->NonZeroSizedTypesCount)
				{
					ptr2->SizeOfs[k] = (ushort)typeInfo3.SizeInChunk;
					ptr2->BufferCapacities[k] = typeInfo3.BufferCapacity;
				}
				else
				{
					ptr2->SizeOfs[k] = 0;
					ptr2->BufferCapacities[k] = 0;
				}
				num9 = math.min(num9, typeInfo3.MaximumChunkCapacity);
			}
			ptr2->ChunkCapacity = math.min(num9, EntityComponentStore.CalculateChunkCapacity(num8, ptr2->SizeOfs, ptr2->NonZeroSizedTypesCount));
			ptr2->Chunks = new ArchetypeChunkData(count, num4);
			ptr2->InstanceSize = 0;
			ptr2->InstanceSizeWithOverhead = 0;
			for (int l = 0; l < ptr2->NonZeroSizedTypesCount; l++)
			{
				ptr2->InstanceSize += (int)ptr2->SizeOfs[l];
				ptr2->InstanceSizeWithOverhead += EntityComponentStore.GetComponentArraySize((int)ptr2->SizeOfs[l], 1);
			}
			EntityComponentStore.<>c__DisplayClass166_0 CS$<>8__locals1;
			checked
			{
				ulong* ptr3 = stackalloc ulong[unchecked((UIntPtr)count) * 8];
				CS$<>8__locals1.memoryOrderings = ptr3;
				int* ptr4 = stackalloc int[unchecked((UIntPtr)count) * 4];
				CS$<>8__locals1.typeFlags = ptr4;
			}
			for (int m = 0; m < count; m++)
			{
				TypeIndex typeIndex2 = types[m].TypeIndex;
				CS$<>8__locals1.memoryOrderings[m] = this.GetTypeInfo(typeIndex2).MemoryOrdering;
				CS$<>8__locals1.typeFlags[m] = typeIndex2.Flags;
			}
			for (int n = 0; n < count; n++)
			{
				int num10 = n;
				while (num10 > 1 && EntityComponentStore.<CreateArchetype>g__MemoryOrderCompare|166_0(n, ptr2->TypeMemoryOrderIndexToIndexInArchetype[num10 - 1], ref CS$<>8__locals1))
				{
					ptr2->TypeMemoryOrderIndexToIndexInArchetype[num10] = ptr2->TypeMemoryOrderIndexToIndexInArchetype[num10 - 1];
					num10--;
				}
				ptr2->TypeMemoryOrderIndexToIndexInArchetype[num10] = n;
			}
			for (int num11 = 0; num11 < count; num11++)
			{
				ptr2->TypeIndexInArchetypeToMemoryOrderIndex[ptr2->TypeMemoryOrderIndexToIndexInArchetype[num11]] = num11;
			}
			int num12 = 0;
			for (int num13 = 0; num13 < count; num13++)
			{
				int num14 = ptr2->TypeMemoryOrderIndexToIndexInArchetype[num13];
				ushort num15 = ptr2->SizeOfs[num14];
				ptr2->Offsets[num14] = num12;
				num12 += EntityComponentStore.GetComponentArraySize((int)num15, ptr2->ChunkCapacity);
			}
			EntityRemapUtility.EntityPatchInfo* ptr5 = ptr2->ScalarEntityPatches;
			EntityRemapUtility.BufferEntityPatchInfo* ptr6 = ptr2->BufferEntityPatches;
			for (int num16 = 0; num16 != count; num16++)
			{
				readonly ref TypeManager.TypeInfo typeInfo4 = ref this.GetTypeInfo(types[num16].TypeIndex);
				TypeManager.EntityOffsetInfo* entityOffsets = this.GetEntityOffsets(in typeInfo4);
				int entityOffsetCount = typeInfo4.EntityOffsetCount;
				if (typeInfo4.BufferCapacity >= 0)
				{
					ptr6 = EntityRemapUtility.AppendBufferEntityPatches(ptr6, entityOffsets, entityOffsetCount, ptr2->Offsets[num16], (int)ptr2->SizeOfs[num16], typeInfo4.ElementSize);
				}
				else if (!TypeManager.IsManagedComponent(typeInfo4.TypeIndex) && typeInfo4.SizeInChunk > 0)
				{
					ptr5 = EntityRemapUtility.AppendEntityPatches(ptr5, entityOffsets, entityOffsetCount, ptr2->Offsets[num16], (int)ptr2->SizeOfs[num16]);
				}
			}
			ptr2->ScalarEntityPatchCount = num;
			ptr2->BufferEntityPatchCount = num2;
			UnsafeUtility.MemClear((void*)(&ptr2->QueryMaskArray.FixedElementField), 128L);
			this.m_Archetypes.Add((void*)ptr2);
			ptr2->FreeChunksBySharedComponents = default(ChunkListMap);
			ptr2->FreeChunksBySharedComponents.Init(ptr2, 16);
			this.m_TypeLookup.Add(ptr2);
			if (this.ArchetypeCleanupComplete(ptr2))
			{
				ptr2->Flags |= ArchetypeFlags.CleanupComplete;
			}
			if (this.ArchetypeCleanupNeeded(ptr2))
			{
				ptr2->Flags |= ArchetypeFlags.CleanupNeeded;
			}
			fixed (EntityComponentStore* ptr7 = &this)
			{
				EntityComponentStore* ptr8 = ptr7;
				ptr2->EntityComponentStore = ptr8;
			}
			ptr2->StableHash = EntityComponentStore.CalculateStableHash(types, ptr2->TypeMemoryOrderIndexToIndexInArchetype, count);
			return ptr2;
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x000244F7 File Offset: 0x000226F7
		private unsafe bool ArchetypeCleanupComplete(Archetype* archetype)
		{
			return archetype->TypesCount == 2 && archetype->Types[1].TypeIndex == this.m_CleanupEntityType;
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x00024524 File Offset: 0x00022724
		private unsafe bool ArchetypeCleanupNeeded(Archetype* archetype)
		{
			for (int i = 1; i < archetype->TypesCount; i++)
			{
				ComponentTypeInArchetype componentTypeInArchetype = archetype->Types[i];
				if (componentTypeInArchetype.IsCleanupComponent)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x00024564 File Offset: 0x00022764
		public unsafe int CountEntities()
		{
			int num = 0;
			for (int i = 0; i < this.m_Archetypes.Length; i++)
			{
				Archetype* ptr = *(IntPtr*)(this.m_Archetypes.Ptr + (IntPtr)i * (IntPtr)sizeof(Archetype*) / (IntPtr)sizeof(Archetype*));
				num += ptr->EntityCount;
			}
			return num;
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x000245AC File Offset: 0x000227AC
		public EntityComponentStore.ArchetypeChanges BeginArchetypeChangeTracking()
		{
			this.m_ArchetypeTrackingVersion += 1U;
			return new EntityComponentStore.ArchetypeChanges
			{
				StartIndex = this.m_Archetypes.Length,
				ArchetypeTrackingVersion = this.m_ArchetypeTrackingVersion
			};
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x000245F0 File Offset: 0x000227F0
		public unsafe void EndArchetypeChangeTracking(EntityComponentStore.ArchetypeChanges changes, EntityQueryManager* queries)
		{
			if (this.m_Archetypes.Length - changes.StartIndex == 0)
			{
				return;
			}
			UnsafePtrList<Archetype> unsafePtrList = new UnsafePtrList<Archetype>(this.m_Archetypes.Ptr + (IntPtr)changes.StartIndex * (IntPtr)sizeof(Archetype*) / (IntPtr)sizeof(Archetype*), this.m_Archetypes.Length - changes.StartIndex);
			queries->AddAdditionalArchetypes(unsafePtrList);
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x0002464C File Offset: 0x0002284C
		public unsafe void InvalidateChunkListCacheForChangedArchetypes()
		{
			Archetype* nextChangedArchetype;
			for (Archetype* ptr = this.m_ChunkListChangesTracker.ArchetypeTrackingHead; ptr != null; ptr = nextChangedArchetype)
			{
				int length = ptr->MatchingQueryData.Length;
				for (int i = 0; i < length; i++)
				{
					EntityQueryData* ptr2 = (EntityQueryData*)(void*)ptr->MatchingQueryData.Ptr[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)];
					ptr2->InvalidateChunkCache();
				}
				nextChangedArchetype = ptr->NextChangedArchetype;
				ptr->NextChangedArchetype = null;
			}
			this.m_ChunkListChangesTracker.ArchetypeTrackingHead = null;
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060006DF RID: 1759 RVA: 0x000246C6 File Offset: 0x000228C6
		public int ManagedComponentIndexUsedCount
		{
			get
			{
				return this.m_ManagedComponentIndex - 1 - this.m_ManagedComponentFreeIndex.Length / 4;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060006E0 RID: 1760 RVA: 0x000246DE File Offset: 0x000228DE
		public int ManagedComponentFreeCount
		{
			get
			{
				return this.m_ManagedComponentIndexCapacity - this.m_ManagedComponentIndex + this.m_ManagedComponentFreeIndex.Length / 4;
			}
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x000246FB File Offset: 0x000228FB
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public void AssertNoQueuedManagedDeferredCommands()
		{
			bool empty = this.ManagedChangesTracker.Empty;
			this.ManagedChangesTracker.Reset();
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x00024714 File Offset: 0x00022914
		public unsafe void DeallocateManagedComponents(ChunkIndex chunk, int indexInChunk, int batchCount)
		{
			Archetype* archetype = this.GetArchetype(chunk);
			if (archetype->NumManagedComponents == 0)
			{
				return;
			}
			short firstManagedComponent = archetype->FirstManagedComponent;
			int numManagedComponents = archetype->NumManagedComponents;
			int num = this.ManagedChangesTracker.BeginFreeManagedComponentCommand();
			for (int i = 0; i < numManagedComponents; i++)
			{
				int num2 = i + (int)firstManagedComponent;
				int* componentDataRO = (int*)ChunkDataUtility.GetComponentDataRO(chunk, archetype, 0, num2);
				for (int j = 0; j < batchCount; j++)
				{
					int num3 = componentDataRO[j + indexInChunk];
					if (num3 != 0)
					{
						this.FreeManagedComponentIndex(num3);
						this.ManagedChangesTracker.AddToFreeManagedComponentCommand(num3);
					}
				}
			}
			this.ManagedChangesTracker.EndDeallocateManagedComponentCommand(num);
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x000247B0 File Offset: 0x000229B0
		public int GrowManagedComponentCapacity(int count)
		{
			return this.m_ManagedComponentIndexCapacity += math.max(this.m_ManagedComponentIndexCapacity / 2, count);
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x000247DC File Offset: 0x000229DC
		public void ReserveManagedComponentIndices(int count)
		{
			int managedComponentFreeCount = this.ManagedComponentFreeCount;
			if (managedComponentFreeCount >= count)
			{
				return;
			}
			int num = this.GrowManagedComponentCapacity(count - managedComponentFreeCount);
			this.ManagedChangesTracker.SetManagedComponentCapacity(num);
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x0002480C File Offset: 0x00022A0C
		public int AllocateManagedComponentIndex()
		{
			if (!this.m_ManagedComponentFreeIndex.IsEmpty)
			{
				return this.m_ManagedComponentFreeIndex.Pop<int>();
			}
			if (this.m_ManagedComponentIndex == this.m_ManagedComponentIndexCapacity)
			{
				this.m_ManagedComponentIndexCapacity += this.m_ManagedComponentIndexCapacity / 2;
				this.ManagedChangesTracker.SetManagedComponentCapacity(this.m_ManagedComponentIndexCapacity);
			}
			int managedComponentIndex = this.m_ManagedComponentIndex;
			this.m_ManagedComponentIndex = managedComponentIndex + 1;
			return managedComponentIndex;
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x00024878 File Offset: 0x00022A78
		public unsafe void AllocateManagedComponentIndices(int* dst, int count)
		{
			int num = this.m_ManagedComponentFreeIndex.Length / 4;
			if (num >= count)
			{
				int num2 = num - count;
				UnsafeUtility.MemCpy((void*)dst, (void*)(this.m_ManagedComponentFreeIndex.Ptr + (IntPtr)num2 * 4), (long)(count * 4));
				this.m_ManagedComponentFreeIndex.Length = num2 * 4;
				return;
			}
			UnsafeUtility.MemCpy((void*)dst, (void*)this.m_ManagedComponentFreeIndex.Ptr, (long)(num * 4));
			this.m_ManagedComponentFreeIndex.Length = 0;
			this.ReserveManagedComponentIndices(count - num);
			for (int i = num; i < count; i++)
			{
				ref int ptr = ref dst[i];
				int managedComponentIndex = this.m_ManagedComponentIndex;
				this.m_ManagedComponentIndex = managedComponentIndex + 1;
				ptr = managedComponentIndex;
			}
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x00024912 File Offset: 0x00022B12
		public void FreeManagedComponentIndex(int index)
		{
			this.m_ManagedComponentFreeIndex.Add<int>(index);
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x00024920 File Offset: 0x00022B20
		public bool AddComponent(Entity entity, ComponentType type)
		{
			ChunkIndex chunkWithEmptySlotsWithAddedComponent = this.GetChunkWithEmptySlotsWithAddedComponent(entity, type);
			if (chunkWithEmptySlotsWithAddedComponent == ChunkIndex.Null)
			{
				return false;
			}
			this.Move(entity, chunkWithEmptySlotsWithAddedComponent);
			return true;
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x00024950 File Offset: 0x00022B50
		public unsafe void AddComponent(Entity entity, in ComponentTypeSet componentTypeSet)
		{
			ChunkIndex chunk = this.GetChunk(entity);
			Archetype* archetype = this.GetArchetype(chunk);
			Archetype* archetypeWithAddedComponents = this.GetArchetypeWithAddedComponents(archetype, in componentTypeSet);
			if (archetypeWithAddedComponents == archetype)
			{
				return;
			}
			EntityComponentStore.ArchetypeChunkFilter archetypeChunkFilterWithAddedComponents = this.GetArchetypeChunkFilterWithAddedComponents(chunk, archetypeWithAddedComponents);
			this.Move(entity, ref archetypeChunkFilterWithAddedComponents);
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x00024990 File Offset: 0x00022B90
		public bool RemoveComponent(Entity entity, ComponentType type)
		{
			ChunkIndex chunkWithEmptySlotsWithRemovedComponent = this.GetChunkWithEmptySlotsWithRemovedComponent(entity, type);
			if (chunkWithEmptySlotsWithRemovedComponent == ChunkIndex.Null)
			{
				return false;
			}
			this.Move(entity, chunkWithEmptySlotsWithRemovedComponent);
			return true;
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x000249C0 File Offset: 0x00022BC0
		public unsafe void RemoveComponent(Entity entity, in ComponentTypeSet componentTypeSet)
		{
			if (Hint.Unlikely(!this.Exists(entity)))
			{
				return;
			}
			ChunkIndex chunk = this.GetChunk(entity);
			Archetype* archetype = this.GetArchetype(chunk);
			Archetype* archetypeWithRemovedComponents = this.GetArchetypeWithRemovedComponents(archetype, in componentTypeSet);
			if (archetypeWithRemovedComponents == archetype)
			{
				return;
			}
			EntityComponentStore.ArchetypeChunkFilter archetypeChunkFilterWithRemovedComponents = this.GetArchetypeChunkFilterWithRemovedComponents(chunk, archetypeWithRemovedComponents);
			this.Move(entity, ref archetypeChunkFilterWithRemovedComponents);
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x00024A10 File Offset: 0x00022C10
		private bool AddComponent(EntityBatchInChunk entityBatchInChunk, ComponentType componentType, int sharedComponentIndex = 0)
		{
			ChunkIndex chunk = entityBatchInChunk.Chunk;
			EntityComponentStore.ArchetypeChunkFilter archetypeChunkFilter;
			if (!this.GetArchetypeChunkFilterWithAddedComponent(chunk, componentType, sharedComponentIndex, out archetypeChunkFilter))
			{
				return false;
			}
			this.Move(entityBatchInChunk, ref archetypeChunkFilter);
			return true;
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x00024A40 File Offset: 0x00022C40
		private unsafe bool AddComponents(EntityBatchInChunk entityBatchInChunk, in ComponentTypeSet componentTypeSet)
		{
			ChunkIndex chunk = entityBatchInChunk.Chunk;
			Archetype* archetype = this.GetArchetype(chunk);
			Archetype* archetypeWithAddedComponents = this.GetArchetypeWithAddedComponents(archetype, in componentTypeSet);
			if (archetypeWithAddedComponents == archetype)
			{
				return false;
			}
			EntityComponentStore.ArchetypeChunkFilter archetypeChunkFilterWithAddedComponents = this.GetArchetypeChunkFilterWithAddedComponents(chunk, archetypeWithAddedComponents);
			if (archetypeChunkFilterWithAddedComponents.Archetype == null)
			{
				return false;
			}
			this.Move(entityBatchInChunk, ref archetypeChunkFilterWithAddedComponents);
			return true;
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x00024A8C File Offset: 0x00022C8C
		private bool RemoveComponent(EntityBatchInChunk entityBatchInChunk, ComponentType componentType)
		{
			ChunkIndex chunk = entityBatchInChunk.Chunk;
			EntityComponentStore.ArchetypeChunkFilter archetypeChunkFilter;
			if (!this.GetArchetypeChunkFilterWithRemovedComponent(chunk, componentType, out archetypeChunkFilter))
			{
				return false;
			}
			this.Move(entityBatchInChunk, ref archetypeChunkFilter);
			return true;
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x00024AB8 File Offset: 0x00022CB8
		private unsafe bool RemoveComponents(EntityBatchInChunk entityBatchInChunk, in ComponentTypeSet componentTypeSet)
		{
			ChunkIndex chunk = entityBatchInChunk.Chunk;
			Archetype* archetype = this.GetArchetype(chunk);
			Archetype* archetypeWithRemovedComponents = this.GetArchetypeWithRemovedComponents(archetype, in componentTypeSet);
			if (archetypeWithRemovedComponents == archetype)
			{
				return false;
			}
			EntityComponentStore.ArchetypeChunkFilter archetypeChunkFilterWithRemovedComponents = this.GetArchetypeChunkFilterWithRemovedComponents(chunk, archetypeWithRemovedComponents);
			if (archetypeChunkFilterWithRemovedComponents.Archetype == null)
			{
				return false;
			}
			this.Move(entityBatchInChunk, ref archetypeChunkFilterWithRemovedComponents);
			return true;
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x00024B04 File Offset: 0x00022D04
		public unsafe void AddComponent(ArchetypeChunk* chunks, int chunkCount, ComponentType componentType, int sharedComponentIndex = 0)
		{
			Archetype* ptr = null;
			Archetype* ptr2 = null;
			int num = 0;
			for (int i = 0; i < chunkCount; i++)
			{
				ChunkIndex chunk = chunks[i].m_Chunk;
				Archetype* archetype = chunks[i].Archetype.Archetype;
				if (ptr != archetype)
				{
					ptr2 = this.GetArchetypeWithAddedComponent(archetype, componentType, &num);
					ptr = archetype;
				}
				if (ptr2 != null)
				{
					EntityComponentStore.ArchetypeChunkFilter archetypeChunkFilterWithAddedComponent = this.GetArchetypeChunkFilterWithAddedComponent(archetype, chunk.ListIndex, ptr2, num, componentType, sharedComponentIndex);
					this.Move(chunk, ref archetypeChunkFilterWithAddedComponent);
				}
			}
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x00024B88 File Offset: 0x00022D88
		public unsafe void AddComponent(EntityQueryImpl* queryImpl, ComponentType componentType, int sharedComponentIndex = 0)
		{
			UnsafeChunkCacheIterator unsafeChunkCacheIterator = new UnsafeChunkCacheIterator(in queryImpl->_Filter, queryImpl->_QueryData->HasEnableableComponents > 0, queryImpl->GetMatchingChunkCache(), queryImpl->_QueryData->MatchingArchetypes.Ptr);
			int num = -1;
			v128 v = default(v128);
			int length = unsafeChunkCacheIterator.Length;
			using (UnsafeList<EntityComponentStore.ChunkAndEnabledMask> unsafeList = new UnsafeList<EntityComponentStore.ChunkAndEnabledMask>(length, Allocator.TempJob, NativeArrayOptions.UninitializedMemory))
			{
				ArchetypeChunk archetypeChunk;
				int num2;
				byte b;
				while (unsafeChunkCacheIterator.MoveNextChunk(ref num, out archetypeChunk, out num2, out b, ref v))
				{
					unsafeList.AddNoResize(new EntityComponentStore.ChunkAndEnabledMask
					{
						Chunk = archetypeChunk.m_Chunk,
						EnabledMask = v,
						ChunkEntityCount = num2,
						UseEnabledMask = b
					});
				}
				Archetype* ptr = null;
				Archetype* ptr2 = null;
				int num3 = 0;
				EntityBatchInChunk* ptr3;
				int i;
				int length2;
				checked
				{
					ptr3 = stackalloc EntityBatchInChunk[unchecked((UIntPtr)128) * (UIntPtr)sizeof(EntityBatchInChunk)];
					i = 0;
					length2 = unsafeList.Length;
				}
				while (i < length2)
				{
					ChunkIndex chunk = unsafeList[i].Chunk;
					Archetype* archetype = this.GetArchetype(chunk);
					if (Hint.Unlikely(ptr != archetype))
					{
						ptr = archetype;
						ptr2 = this.GetArchetypeWithAddedComponent(archetype, componentType, &num3);
					}
					if (ptr2 == null)
					{
						if (Hint.Unlikely(componentType.IsSharedComponent))
						{
							this.SetSharedComponentDataIndexForChunk(chunk, archetype, componentType, sharedComponentIndex, unsafeList[i].UseEnabledMask, unsafeList[i].EnabledMask, ptr3);
						}
					}
					else
					{
						EntityComponentStore.ArchetypeChunkFilter archetypeChunkFilterWithAddedComponent = this.GetArchetypeChunkFilterWithAddedComponent(ptr, chunk.ListIndex, ptr2, num3, componentType, sharedComponentIndex);
						if (unsafeList[i].UseEnabledMask == 0)
						{
							this.Move(chunk, ref archetypeChunkFilterWithAddedComponent);
						}
						else
						{
							int num4 = 0;
							int num5 = 0;
							v = unsafeList[i].EnabledMask;
							int num6;
							while (EnabledBitUtility.TryGetNextRange(v, num5, out num6, out num5))
							{
								ptr3[(IntPtr)(num4++) * (IntPtr)sizeof(EntityBatchInChunk)] = new EntityBatchInChunk
								{
									Chunk = chunk,
									Count = num5 - num6,
									StartIndex = num6
								};
							}
							for (int j = num4 - 1; j >= 0; j--)
							{
								this.Move(ptr3[j], ref archetypeChunkFilterWithAddedComponent);
							}
						}
					}
					i++;
				}
			}
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x00024DE4 File Offset: 0x00022FE4
		public unsafe void SetSharedComponentDataIndexForChunk(ChunkIndex chunk, Archetype* chunkArchetype, ComponentType componentType, int dstSharedComponentDataIndex)
		{
			EntityComponentStore.ArchetypeChunkFilter archetypeChunkFilter;
			if (!this.GetArchetypeChunkFilterWithChangedSharedComponent(chunk, componentType, dstSharedComponentDataIndex, out archetypeChunkFilter))
			{
				return;
			}
			SharedComponentValues sharedComponentValues = &archetypeChunkFilter.SharedComponentValues.FixedElementField;
			ChunkDataUtility.SetSharedComponentDataIndex(chunk, chunkArchetype, in sharedComponentValues, componentType.TypeIndex);
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x00024E24 File Offset: 0x00023024
		private unsafe void SetSharedComponentDataIndexForChunk(ChunkIndex chunk, Archetype* chunkArchetype, ComponentType componentType, int sharedComponentIndex, byte useEnabledMask, v128 chunkEnabledMask, EntityBatchInChunk* chunkBatches)
		{
			EntityComponentStore.ArchetypeChunkFilter archetypeChunkFilter;
			if (!this.GetArchetypeChunkFilterWithChangedSharedComponent(chunk, componentType, sharedComponentIndex, out archetypeChunkFilter))
			{
				return;
			}
			if (useEnabledMask == 0)
			{
				SharedComponentValues sharedComponentValues = &archetypeChunkFilter.SharedComponentValues.FixedElementField;
				ChunkDataUtility.SetSharedComponentDataIndex(chunk, chunkArchetype, in sharedComponentValues, componentType.TypeIndex);
				return;
			}
			int num = 0;
			int num2 = 0;
			int num3;
			while (EnabledBitUtility.TryGetNextRange(chunkEnabledMask, num2, out num3, out num2))
			{
				chunkBatches[num++] = new EntityBatchInChunk
				{
					Chunk = chunk,
					Count = num2 - num3,
					StartIndex = num3
				};
			}
			for (int i = num - 1; i >= 0; i--)
			{
				EntityBatchInChunk entityBatchInChunk = chunkBatches[i];
				SharedComponentValues sharedComponentValues = &archetypeChunkFilter.SharedComponentValues.FixedElementField;
				ChunkDataUtility.SetSharedComponentDataIndex(entityBatchInChunk, chunkArchetype, in sharedComponentValues, componentType.TypeIndex);
			}
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x00024EFC File Offset: 0x000230FC
		public unsafe void AddComponents(EntityQueryImpl* queryImpl, in ComponentTypeSet componentTypeSet)
		{
			UnsafeChunkCacheIterator unsafeChunkCacheIterator = new UnsafeChunkCacheIterator(in queryImpl->_Filter, queryImpl->_QueryData->HasEnableableComponents > 0, queryImpl->GetMatchingChunkCache(), queryImpl->_QueryData->MatchingArchetypes.Ptr);
			int num = -1;
			v128 v = default(v128);
			int length = unsafeChunkCacheIterator.Length;
			using (UnsafeList<EntityComponentStore.ChunkAndEnabledMask> unsafeList = new UnsafeList<EntityComponentStore.ChunkAndEnabledMask>(length, Allocator.TempJob, NativeArrayOptions.UninitializedMemory))
			{
				ArchetypeChunk archetypeChunk;
				int num2;
				byte b;
				while (unsafeChunkCacheIterator.MoveNextChunk(ref num, out archetypeChunk, out num2, out b, ref v))
				{
					unsafeList.AddNoResize(new EntityComponentStore.ChunkAndEnabledMask
					{
						Chunk = archetypeChunk.m_Chunk,
						EnabledMask = v,
						ChunkEntityCount = num2,
						UseEnabledMask = b
					});
				}
				Archetype* ptr = null;
				Archetype* ptr2 = null;
				EntityBatchInChunk* ptr3;
				int i;
				int length2;
				checked
				{
					ptr3 = stackalloc EntityBatchInChunk[unchecked((UIntPtr)128) * (UIntPtr)sizeof(EntityBatchInChunk)];
					i = 0;
					length2 = unsafeList.Length;
				}
				while (i < length2)
				{
					ChunkIndex chunk = unsafeList[i].Chunk;
					Archetype* archetype = this.GetArchetype(chunk);
					if (Hint.Unlikely(ptr != archetype))
					{
						ptr = archetype;
						ptr2 = this.GetArchetypeWithAddedComponents(archetype, in componentTypeSet);
					}
					if (!Hint.Unlikely(ptr == ptr2))
					{
						EntityComponentStore.ArchetypeChunkFilter archetypeChunkFilterWithAddedComponents = this.GetArchetypeChunkFilterWithAddedComponents(chunk, ptr2);
						if (unsafeList[i].UseEnabledMask == 0)
						{
							this.Move(chunk, ref archetypeChunkFilterWithAddedComponents);
						}
						else
						{
							int num3 = 0;
							int num4 = 0;
							v = unsafeList[i].EnabledMask;
							int num5;
							while (EnabledBitUtility.TryGetNextRange(v, num4, out num5, out num4))
							{
								ptr3[(IntPtr)(num3++) * (IntPtr)sizeof(EntityBatchInChunk)] = new EntityBatchInChunk
								{
									Chunk = chunk,
									Count = num4 - num5,
									StartIndex = num5
								};
							}
							for (int j = num3 - 1; j >= 0; j--)
							{
								this.Move(ptr3[j], ref archetypeChunkFilterWithAddedComponents);
							}
						}
					}
					i++;
				}
			}
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x00025114 File Offset: 0x00023314
		public unsafe void RemoveComponent(ArchetypeChunk* chunks, int chunkCount, ComponentType componentType)
		{
			Archetype* ptr = null;
			Archetype* ptr2 = null;
			int num = 0;
			for (int i = 0; i < chunkCount; i++)
			{
				ChunkIndex chunk = chunks[i].m_Chunk;
				Archetype* archetype = chunks[i].Archetype.Archetype;
				if (ptr != archetype)
				{
					ptr2 = this.GetArchetypeWithRemovedComponent(archetype, componentType, &num);
					ptr = archetype;
				}
				if (ptr2 != archetype)
				{
					EntityComponentStore.ArchetypeChunkFilter archetypeChunkFilterWithRemovedComponent = this.GetArchetypeChunkFilterWithRemovedComponent(chunk, ptr2, num, componentType);
					this.Move(chunk, ref archetypeChunkFilterWithRemovedComponent);
				}
			}
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x00025190 File Offset: 0x00023390
		public unsafe void RemoveComponent(EntityQueryImpl* queryImpl, ComponentType componentType)
		{
			UnsafeChunkCacheIterator unsafeChunkCacheIterator = new UnsafeChunkCacheIterator(in queryImpl->_Filter, queryImpl->_QueryData->HasEnableableComponents > 0, queryImpl->GetMatchingChunkCache(), queryImpl->_QueryData->MatchingArchetypes.Ptr);
			int num = -1;
			v128 v = default(v128);
			int length = unsafeChunkCacheIterator.Length;
			using (UnsafeList<EntityComponentStore.ChunkAndEnabledMask> unsafeList = new UnsafeList<EntityComponentStore.ChunkAndEnabledMask>(length, Allocator.TempJob, NativeArrayOptions.UninitializedMemory))
			{
				ArchetypeChunk archetypeChunk;
				int num2;
				byte b;
				while (unsafeChunkCacheIterator.MoveNextChunk(ref num, out archetypeChunk, out num2, out b, ref v))
				{
					unsafeList.AddNoResize(new EntityComponentStore.ChunkAndEnabledMask
					{
						Chunk = archetypeChunk.m_Chunk,
						EnabledMask = v,
						ChunkEntityCount = num2,
						UseEnabledMask = b
					});
				}
				Archetype* ptr = null;
				Archetype* ptr2 = null;
				int num3 = 0;
				EntityBatchInChunk* ptr3;
				int i;
				int length2;
				checked
				{
					ptr3 = stackalloc EntityBatchInChunk[unchecked((UIntPtr)128) * (UIntPtr)sizeof(EntityBatchInChunk)];
					i = 0;
					length2 = unsafeList.Length;
				}
				while (i < length2)
				{
					ChunkIndex chunk = unsafeList[i].Chunk;
					Archetype* archetype = this.GetArchetype(chunk);
					if (Hint.Unlikely(ptr != archetype))
					{
						ptr = archetype;
						ptr2 = this.GetArchetypeWithRemovedComponent(archetype, componentType, &num3);
					}
					if (!Hint.Unlikely(ptr2 == ptr))
					{
						EntityComponentStore.ArchetypeChunkFilter archetypeChunkFilterWithRemovedComponent = this.GetArchetypeChunkFilterWithRemovedComponent(chunk, ptr2, num3, componentType);
						if (unsafeList[i].UseEnabledMask == 0)
						{
							this.Move(chunk, ref archetypeChunkFilterWithRemovedComponent);
						}
						else
						{
							int num4 = 0;
							int num5 = 0;
							v = unsafeList[i].EnabledMask;
							int num6;
							while (EnabledBitUtility.TryGetNextRange(v, num5, out num6, out num5))
							{
								ptr3[(IntPtr)(num4++) * (IntPtr)sizeof(EntityBatchInChunk)] = new EntityBatchInChunk
								{
									Chunk = chunk,
									Count = num5 - num6,
									StartIndex = num6
								};
							}
							for (int j = num4 - 1; j >= 0; j--)
							{
								this.Move(ptr3[j], ref archetypeChunkFilterWithRemovedComponent);
							}
						}
					}
					i++;
				}
			}
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x000253B0 File Offset: 0x000235B0
		public unsafe void RemoveComponents(EntityQueryImpl* queryImpl, in ComponentTypeSet componentTypeSet)
		{
			UnsafeChunkCacheIterator unsafeChunkCacheIterator = new UnsafeChunkCacheIterator(in queryImpl->_Filter, queryImpl->_QueryData->HasEnableableComponents > 0, queryImpl->GetMatchingChunkCache(), queryImpl->_QueryData->MatchingArchetypes.Ptr);
			int num = -1;
			v128 v = default(v128);
			int length = unsafeChunkCacheIterator.Length;
			using (UnsafeList<EntityComponentStore.ChunkAndEnabledMask> unsafeList = new UnsafeList<EntityComponentStore.ChunkAndEnabledMask>(length, Allocator.TempJob, NativeArrayOptions.UninitializedMemory))
			{
				ArchetypeChunk archetypeChunk;
				int num2;
				byte b;
				while (unsafeChunkCacheIterator.MoveNextChunk(ref num, out archetypeChunk, out num2, out b, ref v))
				{
					unsafeList.AddNoResize(new EntityComponentStore.ChunkAndEnabledMask
					{
						Chunk = archetypeChunk.m_Chunk,
						EnabledMask = v,
						ChunkEntityCount = num2,
						UseEnabledMask = b
					});
				}
				Archetype* ptr = null;
				Archetype* ptr2 = null;
				EntityBatchInChunk* ptr3;
				int i;
				int length2;
				checked
				{
					ptr3 = stackalloc EntityBatchInChunk[unchecked((UIntPtr)128) * (UIntPtr)sizeof(EntityBatchInChunk)];
					i = 0;
					length2 = unsafeList.Length;
				}
				while (i < length2)
				{
					ChunkIndex chunk = unsafeList[i].Chunk;
					Archetype* archetype = this.GetArchetype(chunk);
					if (Hint.Unlikely(ptr != archetype))
					{
						ptr = archetype;
						ptr2 = this.GetArchetypeWithRemovedComponents(archetype, in componentTypeSet);
					}
					if (!Hint.Unlikely(ptr2 == ptr))
					{
						EntityComponentStore.ArchetypeChunkFilter archetypeChunkFilterWithRemovedComponents = this.GetArchetypeChunkFilterWithRemovedComponents(chunk, ptr2);
						if (unsafeList[i].UseEnabledMask == 0)
						{
							this.Move(chunk, ref archetypeChunkFilterWithRemovedComponents);
						}
						else
						{
							int num3 = 0;
							int num4 = 0;
							v = unsafeList[i].EnabledMask;
							int num5;
							while (EnabledBitUtility.TryGetNextRange(v, num4, out num5, out num4))
							{
								ptr3[(IntPtr)(num3++) * (IntPtr)sizeof(EntityBatchInChunk)] = new EntityBatchInChunk
								{
									Chunk = chunk,
									Count = num4 - num5,
									StartIndex = num5
								};
							}
							for (int j = num3 - 1; j >= 0; j--)
							{
								this.Move(ptr3[j], ref archetypeChunkFilterWithRemovedComponents);
							}
						}
					}
					i++;
				}
			}
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x000255C8 File Offset: 0x000237C8
		public unsafe void AddComponent(UnsafeList<EntityBatchInChunk>* sortedEntityBatchList, ComponentType type, int existingSharedComponentIndex)
		{
			for (int i = sortedEntityBatchList->Length - 1; i >= 0; i--)
			{
				this.AddComponent(sortedEntityBatchList->Ptr[i], type, existingSharedComponentIndex);
			}
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x00025608 File Offset: 0x00023808
		public unsafe void AddComponents(UnsafeList<EntityBatchInChunk>* sortedEntityBatchList, in ComponentTypeSet typeSet)
		{
			for (int i = sortedEntityBatchList->Length - 1; i >= 0; i--)
			{
				this.AddComponents(sortedEntityBatchList->Ptr[i], in typeSet);
			}
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x00025648 File Offset: 0x00023848
		public unsafe void RemoveComponents(UnsafeList<EntityBatchInChunk>* sortedEntityBatchList, in ComponentTypeSet typeSet)
		{
			for (int i = sortedEntityBatchList->Length - 1; i >= 0; i--)
			{
				this.RemoveComponents(sortedEntityBatchList->Ptr[i], in typeSet);
			}
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x00025688 File Offset: 0x00023888
		public unsafe void RemoveComponent(UnsafeList<EntityBatchInChunk>* sortedEntityBatchList, ComponentType type)
		{
			for (int i = sortedEntityBatchList->Length - 1; i >= 0; i--)
			{
				this.RemoveComponent(sortedEntityBatchList->Ptr[i], type);
			}
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x000256C8 File Offset: 0x000238C8
		public unsafe void SetSharedComponentDataIndex(Entity entity, ComponentType componentType, int dstSharedComponentDataIndex)
		{
			EntityComponentStore.ArchetypeChunkFilter archetypeChunkFilter;
			if (!this.GetArchetypeChunkFilterWithChangedSharedComponent(this.GetChunk(entity), componentType, dstSharedComponentDataIndex, out archetypeChunkFilter))
			{
				return;
			}
			Archetype* archetype = archetypeChunkFilter.Archetype;
			SharedComponentValues sharedComponentValues = &archetypeChunkFilter.SharedComponentValues.FixedElementField;
			ChunkDataUtility.SetSharedComponentDataIndex(entity, archetype, in sharedComponentValues, componentType.TypeIndex);
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x00025710 File Offset: 0x00023910
		public unsafe void SetSharedComponentDataIndex(EntityQueryImpl* queryImpl, ComponentType componentType, int dstSharedComponentDataIndex)
		{
			UnsafeChunkCacheIterator unsafeChunkCacheIterator = new UnsafeChunkCacheIterator(in queryImpl->_Filter, queryImpl->_QueryData->HasEnableableComponents > 0, queryImpl->GetMatchingChunkCache(), queryImpl->_QueryData->MatchingArchetypes.Ptr);
			int num = -1;
			v128 v = default(v128);
			int length = unsafeChunkCacheIterator.Length;
			using (UnsafeList<EntityComponentStore.ChunkAndEnabledMask> unsafeList = new UnsafeList<EntityComponentStore.ChunkAndEnabledMask>(length, Allocator.TempJob, NativeArrayOptions.UninitializedMemory))
			{
				ArchetypeChunk archetypeChunk;
				int num2;
				byte b;
				while (unsafeChunkCacheIterator.MoveNextChunk(ref num, out archetypeChunk, out num2, out b, ref v))
				{
					unsafeList.AddNoResize(new EntityComponentStore.ChunkAndEnabledMask
					{
						Chunk = archetypeChunk.m_Chunk,
						EnabledMask = v,
						ChunkEntityCount = num2,
						UseEnabledMask = b
					});
				}
				EntityBatchInChunk* ptr;
				int i;
				int length2;
				checked
				{
					ptr = stackalloc EntityBatchInChunk[unchecked((UIntPtr)128) * (UIntPtr)sizeof(EntityBatchInChunk)];
					i = 0;
					length2 = unsafeList.Length;
				}
				while (i < length2)
				{
					ChunkIndex chunk = unsafeList[i].Chunk;
					Archetype* archetype = this.GetArchetype(chunk);
					this.SetSharedComponentDataIndexForChunk(chunk, archetype, componentType, dstSharedComponentDataIndex, unsafeList[i].UseEnabledMask, unsafeList[i].EnabledMask, ptr);
					i++;
				}
			}
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x0002584C File Offset: 0x00023A4C
		public unsafe void Move(Entity entity, Archetype* dstArchetype)
		{
			EntityComponentStore.ArchetypeChunkFilter archetypeChunkFilterWithChangedArchetype = this.GetArchetypeChunkFilterWithChangedArchetype(this.GetChunk(entity), dstArchetype);
			if (archetypeChunkFilterWithChangedArchetype.Archetype == null)
			{
				return;
			}
			this.Move(entity, ref archetypeChunkFilterWithChangedArchetype);
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x0002587C File Offset: 0x00023A7C
		public unsafe void Move(Entity entity, Archetype* archetype, SharedComponentValues sharedComponentValues)
		{
			EntityComponentStore.ArchetypeChunkFilter archetypeChunkFilter = new EntityComponentStore.ArchetypeChunkFilter(archetype, sharedComponentValues);
			this.Move(entity, ref archetypeChunkFilter);
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x0002589C File Offset: 0x00023A9C
		public unsafe void Move(ChunkIndex srcChunk, Archetype* archetype, SharedComponentValues sharedComponentValues)
		{
			EntityComponentStore.ArchetypeChunkFilter archetypeChunkFilter = new EntityComponentStore.ArchetypeChunkFilter(archetype, sharedComponentValues);
			this.Move(srcChunk, ref archetypeChunkFilter);
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x000258BC File Offset: 0x00023ABC
		public unsafe void MoveAndSetChangeVersion(EntityBatchInChunk batch, Archetype* archetype, SharedComponentValues sharedComponentValues, TypeIndex typeIndex)
		{
			EntityComponentStore.ArchetypeChunkFilter archetypeChunkFilter = new EntityComponentStore.ArchetypeChunkFilter(archetype, sharedComponentValues);
			this.MoveAndSetChangeVersion(batch, ref archetypeChunkFilter, typeIndex);
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x000258E0 File Offset: 0x00023AE0
		private void Move(Entity entity, ChunkIndex dstChunk)
		{
			EntityInChunk entityInChunk = this.GetEntityInChunk(entity);
			ChunkIndex chunk = entityInChunk.Chunk;
			int indexInChunk = entityInChunk.IndexInChunk;
			EntityBatchInChunk entityBatchInChunk = new EntityBatchInChunk
			{
				Chunk = chunk,
				Count = 1,
				StartIndex = indexInChunk
			};
			this.Move(in entityBatchInChunk, dstChunk);
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x00025930 File Offset: 0x00023B30
		private void Move(Entity entity, ref EntityComponentStore.ArchetypeChunkFilter archetypeChunkFilter)
		{
			EntityInChunk entityInChunk = this.GetEntityInChunk(entity);
			EntityBatchInChunk entityBatchInChunk = new EntityBatchInChunk
			{
				Chunk = entityInChunk.Chunk,
				Count = 1,
				StartIndex = entityInChunk.IndexInChunk
			};
			this.Move(entityBatchInChunk, ref archetypeChunkFilter);
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x0002597C File Offset: 0x00023B7C
		private unsafe void Move(ChunkIndex srcChunk, ref EntityComponentStore.ArchetypeChunkFilter archetypeChunkFilter)
		{
			Archetype* archetype = this.GetArchetype(srcChunk);
			if (archetypeChunkFilter.Archetype->CleanupComplete)
			{
				ChunkDataUtility.Deallocate(archetype, srcChunk);
				return;
			}
			if (ChunkDataUtility.AreLayoutCompatible(archetype, archetypeChunkFilter.Archetype))
			{
				fixed (int* ptr = &archetypeChunkFilter.SharedComponentValues.FixedElementField)
				{
					int* ptr2 = ptr;
					ChunkDataUtility.ChangeArchetypeInPlace(archetype, srcChunk, archetypeChunkFilter.Archetype, ptr2);
				}
				return;
			}
			EntityBatchInChunk entityBatchInChunk = new EntityBatchInChunk
			{
				Chunk = srcChunk,
				Count = srcChunk.Count,
				StartIndex = 0
			};
			this.Move(entityBatchInChunk, ref archetypeChunkFilter);
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x00025A08 File Offset: 0x00023C08
		private unsafe void Move(EntityBatchInChunk entityBatchInChunk, ref EntityComponentStore.ArchetypeChunkFilter archetypeChunkFilter)
		{
			bool cleanupComplete = archetypeChunkFilter.Archetype->CleanupComplete;
			ChunkIndex chunk = entityBatchInChunk.Chunk;
			int i = entityBatchInChunk.Count;
			int startIndex = entityBatchInChunk.StartIndex;
			if (i == chunk.Count && cleanupComplete)
			{
				ChunkDataUtility.Deallocate(this.GetArchetype(chunk), chunk);
				return;
			}
			while (i > 0)
			{
				ChunkIndex chunkWithEmptySlots = this.GetChunkWithEmptySlots(ref archetypeChunkFilter);
				EntityBatchInChunk entityBatchInChunk2 = default(EntityBatchInChunk);
				entityBatchInChunk2.Chunk = chunk;
				entityBatchInChunk2.Count = i;
				entityBatchInChunk2.StartIndex = startIndex;
				int num = this.Move(in entityBatchInChunk2, chunkWithEmptySlots);
				i -= num;
			}
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x00025A90 File Offset: 0x00023C90
		private unsafe void MoveAndSetChangeVersion(EntityBatchInChunk entityBatchInChunk, ref EntityComponentStore.ArchetypeChunkFilter archetypeChunkFilter, TypeIndex typeIndex)
		{
			bool cleanupComplete = archetypeChunkFilter.Archetype->CleanupComplete;
			ChunkIndex chunk = entityBatchInChunk.Chunk;
			int i = entityBatchInChunk.Count;
			int startIndex = entityBatchInChunk.StartIndex;
			if (i == chunk.Count && cleanupComplete)
			{
				ChunkDataUtility.Deallocate(this.GetArchetype(chunk), chunk);
				return;
			}
			Archetype* archetype = archetypeChunkFilter.Archetype;
			int indexInTypeArray = ChunkDataUtility.GetIndexInTypeArray(archetype, typeIndex);
			while (i > 0)
			{
				ChunkIndex chunkWithEmptySlots = this.GetChunkWithEmptySlots(ref archetypeChunkFilter);
				EntityBatchInChunk entityBatchInChunk2 = default(EntityBatchInChunk);
				entityBatchInChunk2.Chunk = chunk;
				entityBatchInChunk2.Count = i;
				entityBatchInChunk2.StartIndex = startIndex;
				int num = this.Move(in entityBatchInChunk2, chunkWithEmptySlots);
				i -= num;
				archetype->Chunks.SetChangeVersion(indexInTypeArray, chunkWithEmptySlots.ListIndex, this.GlobalSystemVersion);
			}
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x00025B48 File Offset: 0x00023D48
		private unsafe int Move(in EntityBatchInChunk srcBatch, ChunkIndex dstChunk)
		{
			ChunkIndex chunk = srcBatch.Chunk;
			Archetype* archetype = this.GetArchetype(chunk);
			Archetype* archetype2 = this.GetArchetype(dstChunk);
			int num = math.min(archetype2->ChunkCapacity - dstChunk.Count, srcBatch.Count);
			int num2 = srcBatch.StartIndex + srcBatch.Count - num;
			EntityBatchInChunk entityBatchInChunk = new EntityBatchInChunk
			{
				Chunk = chunk,
				StartIndex = num2,
				Count = num
			};
			ChunkDataUtility.Clone(archetype, in entityBatchInChunk, archetype2, dstChunk);
			fixed (EntityComponentStore* ptr = &this)
			{
				ChunkDataUtility.Remove(ptr, in entityBatchInChunk);
			}
			return num;
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x00025BD4 File Offset: 0x00023DD4
		internal bool IsComponentEnabled(Entity entity, TypeIndex typeIndex)
		{
			EntityInChunk entityInChunk = this.GetEntityInChunk(entity);
			int indexInTypeArray = ChunkDataUtility.GetIndexInTypeArray(this.GetArchetype(entityInChunk.Chunk), typeIndex);
			return this.IsComponentEnabled(entityInChunk.Chunk, entityInChunk.IndexInChunk, indexInTypeArray);
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x00025C10 File Offset: 0x00023E10
		internal unsafe bool IsComponentEnabled(Entity entity, TypeIndex typeIndex, ref LookupCache typeLookupCache)
		{
			EntityInChunk entityInChunk = this.GetEntityInChunk(entity);
			Archetype* archetype = this.GetArchetype(entityInChunk.Chunk);
			if (Hint.Unlikely(archetype != typeLookupCache.Archetype))
			{
				typeLookupCache.Update(archetype, typeIndex);
			}
			return this.IsComponentEnabled(entityInChunk.Chunk, entityInChunk.IndexInChunk, (int)typeLookupCache.IndexInArchetype);
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x00025C68 File Offset: 0x00023E68
		internal unsafe bool IsComponentEnabled(ChunkIndex chunk, int indexInChunk, int typeIndexInArchetype)
		{
			Archetype* archetype = this.GetArchetype(chunk);
			return ChunkDataUtility.GetEnabledRefRO(chunk, archetype, typeIndexInArchetype).IsSet(indexInChunk);
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x00025C90 File Offset: 0x00023E90
		internal void SetComponentEnabled(Entity entity, TypeIndex typeIndex, bool value)
		{
			EntityInChunk entityInChunk = this.GetEntityInChunk(entity);
			int indexInTypeArray = ChunkDataUtility.GetIndexInTypeArray(this.GetArchetype(entityInChunk.Chunk), typeIndex);
			this.SetComponentEnabled(entityInChunk.Chunk, entityInChunk.IndexInChunk, indexInTypeArray, value);
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x00025CCC File Offset: 0x00023ECC
		internal unsafe void SetComponentEnabled(Entity entity, TypeIndex typeIndex, bool value, ref LookupCache typeLookupCache)
		{
			EntityInChunk entityInChunk = this.GetEntityInChunk(entity);
			Archetype* archetype = this.GetArchetype(entityInChunk.Chunk);
			if (Hint.Unlikely(archetype != typeLookupCache.Archetype))
			{
				typeLookupCache.Update(archetype, typeIndex);
			}
			this.SetComponentEnabled(entityInChunk.Chunk, entityInChunk.IndexInChunk, (int)typeLookupCache.IndexInArchetype, value);
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x00025D28 File Offset: 0x00023F28
		internal unsafe void SetComponentEnabled(ChunkIndex chunk, int indexInChunk, int typeIndexInArchetype, bool value)
		{
			Archetype* archetype = this.GetArchetype(chunk);
			Archetype archetype2 = *archetype;
			int* ptr;
			ref UnsafeBitArray enabledRefRW = ChunkDataUtility.GetEnabledRefRW(chunk, archetype, typeIndexInArchetype, this.GlobalSystemVersion, out ptr);
			int num = indexInChunk / 64;
			ulong* ptr2 = enabledRefRW.Ptr + num;
			int num2 = indexInChunk - num * 64;
			long num3 = 1L << num2;
			long num4 = (long)(*ptr2);
			long num5;
			long num6;
			do
			{
				num5 = math.select(num4 & ~num3, num4 | num3, value);
				num6 = num4;
				num4 = Interlocked.CompareExchange(UnsafeUtility.AsRef<long>((void*)ptr2), num5, num6);
			}
			while (num6 != num4);
			if (num4 == num5)
			{
				return;
			}
			int num7 = math.select(1, -1, value);
			Interlocked.Add(UnsafeUtility.AsRef<int>((void*)ptr), num7);
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x00025DD4 File Offset: 0x00023FD4
		public unsafe ulong* GetEnabledRawRW(Entity entity, TypeIndex typeIndex, ref LookupCache typeLookupCache, uint globalSystemVersion, out int indexInBitField, out int* ptrChunkDisabledCount)
		{
			EntityInChunk entityInChunk = this.GetEntityInChunk(entity);
			Archetype* archetype = this.GetArchetype(entityInChunk.Chunk);
			indexInBitField = entityInChunk.IndexInChunk;
			if (Hint.Unlikely(archetype != typeLookupCache.Archetype))
			{
				typeLookupCache.Update(archetype, typeIndex);
			}
			return ChunkDataUtility.GetEnabledRefRW(entityInChunk.Chunk, archetype, (int)typeLookupCache.IndexInArchetype, globalSystemVersion, out ptrChunkDisabledCount).Ptr;
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x00025E38 File Offset: 0x00024038
		public unsafe ulong* GetEnabledRawRO(Entity entity, TypeIndex typeIndex, ref LookupCache typeLookupCache, out int indexInBitField, out int* ptrChunkDisabledCount)
		{
			EntityInChunk entityInChunk = this.GetEntityInChunk(entity);
			Archetype* archetype = this.GetArchetype(entityInChunk.Chunk);
			indexInBitField = entityInChunk.IndexInChunk;
			if (Hint.Unlikely(archetype != typeLookupCache.Archetype))
			{
				typeLookupCache.Update(archetype, typeIndex);
			}
			int num = archetype->TypeIndexInArchetypeToMemoryOrderIndex[typeLookupCache.IndexInArchetype];
			ptrChunkDisabledCount = archetype->Chunks.GetPointerToChunkDisabledCountForType(num, entityInChunk.Chunk.ListIndex);
			return ChunkDataUtility.GetEnabledRefRO(entityInChunk.Chunk, archetype, (int)typeLookupCache.IndexInArchetype).Ptr;
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x00025EC4 File Offset: 0x000240C4
		internal unsafe void AllocateEntities(Archetype* arch, ChunkIndex chunk, int baseIndex, int count, Entity* outputEntities)
		{
			Entity* ptr = (Entity*)(chunk.Buffer + (IntPtr)baseIndex * (IntPtr)sizeof(Entity));
			EntityComponentStore.s_entityStore.Data.AllocateEntities(ptr, count, chunk, baseIndex);
			if (outputEntities != null)
			{
				UnsafeUtility.MemCpy((void*)outputEntities, (void*)ptr, (long)(count * sizeof(Entity)));
			}
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x00025F10 File Offset: 0x00024110
		internal unsafe void DeallocateDataEntitiesInChunk(ChunkIndex chunk, Archetype* archetype, int indexInChunk, int batchCount)
		{
			this.DeallocateBuffers(chunk, indexInChunk, batchCount);
			this.DeallocateManagedComponents(chunk, indexInChunk, batchCount);
			Entity* ptr = (Entity*)(chunk.Buffer + (IntPtr)indexInChunk * (IntPtr)sizeof(Entity));
			EntityComponentStore.s_entityStore.Data.DeallocateEntities(ptr, batchCount);
			int num = Math.Min(batchCount, chunk.Count - indexInChunk - batchCount);
			ChunkDataUtility.RemoveFromEnabledBitsHierarchicalData(chunk, archetype, indexInChunk, batchCount);
			if (num == 0)
			{
				ChunkDataUtility.ClearPaddingBits(chunk, archetype, indexInChunk, batchCount);
				return;
			}
			Entity* ptr2 = (Entity*)(chunk.Buffer + (IntPtr)(chunk.Count - num) * (IntPtr)sizeof(Entity));
			for (int num2 = 0; num2 != num; num2++)
			{
				EntityInChunk entityInChunk = this.GetEntityInChunk(ptr2[num2]);
				entityInChunk.IndexInChunk = indexInChunk + num2;
				this.SetEntityInChunk(ptr2[num2], entityInChunk);
			}
			int num3 = chunk.Count - num;
			fixed (EntityComponentStore* ptr3 = &this)
			{
				ChunkDataUtility.Copy(ptr3, chunk, num3, chunk, indexInChunk, num);
			}
			ChunkDataUtility.CloneEnabledBits(chunk, archetype, num3, chunk, archetype, indexInChunk, num);
			int num4 = chunk.Count - batchCount;
			ChunkDataUtility.ClearPaddingBits(chunk, archetype, num4, batchCount);
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x0002602C File Offset: 0x0002422C
		private unsafe void DeallocateBuffers(ChunkIndex chunk, int indexInChunk, int batchCount)
		{
			Archetype* archetype = this.GetArchetype(chunk);
			byte* buffer = chunk.Buffer;
			int i = 0;
			int typesCount = archetype->TypesCount;
			while (i < typesCount)
			{
				ComponentTypeInArchetype componentTypeInArchetype = archetype->Types[i];
				if (componentTypeInArchetype.IsBuffer)
				{
					byte* ptr = buffer + archetype->Offsets[i];
					ushort num = archetype->SizeOfs[i];
					for (int j = 0; j < batchCount; j++)
					{
						BufferHeader.Destroy((BufferHeader*)(ptr + (int)num * (indexInChunk + j)));
					}
				}
				i++;
			}
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x000260BC File Offset: 0x000242BC
		public unsafe Archetype* GetOrCreateArchetype(ComponentTypeInArchetype* inTypesSorted, int count)
		{
			Archetype* ptr = this.GetExistingArchetype(inTypesSorted, count);
			if (ptr != null)
			{
				return ptr;
			}
			ptr = this.CreateArchetype(inTypesSorted, count);
			ComponentTypeInArchetype* ptr2;
			checked
			{
				ptr2 = stackalloc ComponentTypeInArchetype[unchecked((UIntPtr)(count + 1)) * (UIntPtr)sizeof(ComponentTypeInArchetype)];
				ptr->InstantiateArchetype = this.CreateInstanceArchetype(inTypesSorted, count, ptr2, ptr, true);
				ptr->CopyArchetype = this.CreateInstanceArchetype(inTypesSorted, count, ptr2, ptr, false);
				Archetype* instantiateArchetype = ptr->InstantiateArchetype;
			}
			UIntPtr uintPtr = (UIntPtr)0;
			Archetype* copyArchetype = ptr->CopyArchetype;
			UIntPtr uintPtr2 = (UIntPtr)0;
			if (ptr->CleanupNeeded)
			{
				ComponentTypeInArchetype componentTypeInArchetype = new ComponentTypeInArchetype(ComponentType.FromTypeIndex(this.m_CleanupEntityType));
				bool flag = false;
				*ptr2 = *inTypesSorted;
				int num = 1;
				for (int i = 1; i < ptr->TypesCount; i++)
				{
					ComponentTypeInArchetype componentTypeInArchetype2 = ptr->Types[i];
					if (componentTypeInArchetype2.IsCleanupComponent)
					{
						if (!flag && componentTypeInArchetype < ptr->Types[i])
						{
							ptr2[(IntPtr)(num++) * (IntPtr)sizeof(ComponentTypeInArchetype)] = componentTypeInArchetype;
							flag = true;
						}
						ptr2[(IntPtr)(num++) * (IntPtr)sizeof(ComponentTypeInArchetype)] = ptr->Types[i];
					}
				}
				if (!flag)
				{
					ptr2[(IntPtr)(num++) * (IntPtr)sizeof(ComponentTypeInArchetype)] = componentTypeInArchetype;
				}
				Archetype* orCreateArchetype = this.GetOrCreateArchetype(ptr2, num);
				ptr->CleanupResidueArchetype = orCreateArchetype;
			}
			if (count > 1)
			{
				*ptr2 = new ComponentTypeInArchetype(this.m_EntityComponentType);
				int num2 = 1;
				for (int j = 1; j < count; j++)
				{
					ComponentTypeInArchetype componentTypeInArchetype3 = inTypesSorted[j];
					if (inTypesSorted[j].IsChunkComponent)
					{
						ComponentType componentType = new ComponentType
						{
							TypeIndex = this.ChunkComponentToNormalTypeIndex(componentTypeInArchetype3.TypeIndex)
						};
						SortingUtilities.InsertSorted(ptr2, num2++, componentType);
					}
				}
				if (num2 > 1)
				{
					SortingUtilities.InsertSorted(ptr2, num2++, this.m_ChunkHeaderComponentType);
					ptr->MetaChunkArchetype = this.GetOrCreateArchetype(ptr2, num2);
				}
			}
			return ptr;
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x000262CC File Offset: 0x000244CC
		private unsafe Archetype* CreateInstanceArchetype(ComponentTypeInArchetype* inTypesSorted, int count, ComponentTypeInArchetype* types, Archetype* srcArchetype, bool removePrefab)
		{
			UnsafeUtility.MemCpy((void*)types, (void*)inTypesSorted, (long)(sizeof(ComponentTypeInArchetype) * count));
			bool flag = false;
			int num = 0;
			int num2 = -1;
			int num3 = -1;
			for (int i = 0; i < srcArchetype->TypesCount; i++)
			{
				ComponentTypeInArchetype componentTypeInArchetype = srcArchetype->Types[i];
				flag |= componentTypeInArchetype.TypeIndex == this.m_CleanupEntityType;
				if (componentTypeInArchetype.IsCleanupComponent || (removePrefab && componentTypeInArchetype.TypeIndex == this.m_PrefabType))
				{
					num++;
				}
				else
				{
					types[i - num] = srcArchetype->Types[i];
				}
				if (removePrefab)
				{
					if (componentTypeInArchetype.TypeIndex == this.m_LinkedGroupType)
					{
						num2 = i - num;
					}
					else if (componentTypeInArchetype.TypeIndex == this.m_OmitLinkedEntityGroupFromPrefabInstanceType)
					{
						num3 = i - num;
					}
				}
			}
			if (flag)
			{
				return null;
			}
			if (num2 >= 0 && num3 >= 0)
			{
				int j = math.min(num2, num3);
				int num4 = math.max(num2, num3) - 1;
				while (j < num4)
				{
					types[j] = types[j + 1];
					j++;
				}
				int k = math.max(num2, num3) - 1;
				int num5 = count - num - 2;
				while (k < num5)
				{
					types[k] = types[k + 2];
					k++;
				}
				num += 2;
			}
			if (num <= 0)
			{
				return srcArchetype;
			}
			return this.GetOrCreateArchetype(types, count - num);
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x0002647D File Offset: 0x0002467D
		private ChunkIndex GetChunkWithEmptySlotsWithAddedComponent(Entity entity, ComponentType componentType)
		{
			if (!this.Exists(entity))
			{
				return ChunkIndex.Null;
			}
			return this.GetChunkWithEmptySlotsWithAddedComponent(this.GetChunk(entity), componentType, 0);
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x000264A0 File Offset: 0x000246A0
		private ChunkIndex GetChunkWithEmptySlotsWithAddedComponent(ChunkIndex srcChunk, ComponentType componentType, int sharedComponentIndex = 0)
		{
			EntityComponentStore.ArchetypeChunkFilter archetypeChunkFilter;
			if (!this.GetArchetypeChunkFilterWithAddedComponent(srcChunk, componentType, sharedComponentIndex, out archetypeChunkFilter))
			{
				return ChunkIndex.Null;
			}
			return this.GetChunkWithEmptySlots(ref archetypeChunkFilter);
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x000264C8 File Offset: 0x000246C8
		private ChunkIndex GetChunkWithEmptySlotsWithRemovedComponent(Entity entity, ComponentType componentType)
		{
			if (!this.Exists(entity))
			{
				return ChunkIndex.Null;
			}
			return this.GetChunkWithEmptySlotsWithRemovedComponent(this.GetChunk(entity), componentType);
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x000264E8 File Offset: 0x000246E8
		private ChunkIndex GetChunkWithEmptySlotsWithRemovedComponent(ChunkIndex srcChunk, ComponentType componentType)
		{
			EntityComponentStore.ArchetypeChunkFilter archetypeChunkFilter;
			if (!this.GetArchetypeChunkFilterWithRemovedComponent(srcChunk, componentType, out archetypeChunkFilter))
			{
				return ChunkIndex.Null;
			}
			return this.GetChunkWithEmptySlots(ref archetypeChunkFilter);
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x00026510 File Offset: 0x00024710
		private unsafe ChunkIndex GetChunkWithEmptySlots(ref EntityComponentStore.ArchetypeChunkFilter archetypeChunkFilter)
		{
			Archetype* archetype = archetypeChunkFilter.Archetype;
			fixed (int* ptr = &archetypeChunkFilter.SharedComponentValues.FixedElementField)
			{
				int* ptr2 = ptr;
				ChunkIndex chunkIndex = archetype->GetExistingChunkWithEmptySlots(ptr2);
				if (chunkIndex == ChunkIndex.Null)
				{
					chunkIndex = this.GetCleanChunk(archetype, ptr2);
				}
				return chunkIndex;
			}
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x0002655C File Offset: 0x0002475C
		private unsafe EntityComponentStore.ArchetypeChunkFilter GetArchetypeChunkFilterWithChangedArchetype(ChunkIndex srcChunk, Archetype* dstArchetype)
		{
			Archetype* archetype = this.GetArchetype(srcChunk);
			EntityComponentStore.ArchetypeChunkFilter archetypeChunkFilter = default(EntityComponentStore.ArchetypeChunkFilter);
			archetypeChunkFilter.Archetype = dstArchetype;
			SharedComponentValues sharedComponentValues = archetype->Chunks.GetSharedComponentValues(srcChunk.ListIndex);
			EntityComponentStore.BuildSharedComponentIndicesWithChangedArchetype(archetype, dstArchetype, sharedComponentValues, &archetypeChunkFilter.SharedComponentValues.FixedElementField);
			return archetypeChunkFilter;
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x000265A8 File Offset: 0x000247A8
		private unsafe bool GetArchetypeChunkFilterWithChangedSharedComponent(ChunkIndex srcChunk, ComponentType componentType, int dstSharedComponentIndex, out EntityComponentStore.ArchetypeChunkFilter outFilter)
		{
			TypeIndex typeIndex = componentType.TypeIndex;
			Archetype* archetype = this.GetArchetype(srcChunk);
			int indexInTypeArray = ChunkDataUtility.GetIndexInTypeArray(archetype, typeIndex);
			SharedComponentValues sharedComponentValues = archetype->Chunks.GetSharedComponentValues(srcChunk.ListIndex);
			int num = indexInTypeArray - (int)archetype->FirstSharedComponent;
			int num2 = *sharedComponentValues[num];
			if (dstSharedComponentIndex == num2)
			{
				outFilter = default(EntityComponentStore.ArchetypeChunkFilter);
				return false;
			}
			outFilter = default(EntityComponentStore.ArchetypeChunkFilter);
			outFilter.Archetype = archetype;
			for (int i = 0; i < archetype->NumSharedComponents; i++)
			{
				*((ref outFilter.SharedComponentValues.FixedElementField) + (IntPtr)i * 4) = *sharedComponentValues[i];
			}
			*((ref outFilter.SharedComponentValues.FixedElementField) + (IntPtr)num * 4) = dstSharedComponentIndex;
			return true;
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x00026658 File Offset: 0x00024858
		private unsafe EntityComponentStore.ArchetypeChunkFilter GetArchetypeChunkFilterWithAddedComponent(Archetype* srcArchetype, int srcChunkListIndex, Archetype* dstArchetype, int indexInTypeArray, ComponentType componentType, int sharedComponentIndex)
		{
			EntityComponentStore.ArchetypeChunkFilter archetypeChunkFilter = default(EntityComponentStore.ArchetypeChunkFilter);
			archetypeChunkFilter.Archetype = dstArchetype;
			SharedComponentValues sharedComponentValues = srcArchetype->Chunks.GetSharedComponentValues(srcChunkListIndex);
			if (componentType.IsSharedComponent)
			{
				EntityComponentStore.BuildSharedComponentIndicesWithAddedComponent(indexInTypeArray - (int)dstArchetype->FirstSharedComponent, sharedComponentIndex, dstArchetype->NumSharedComponents, sharedComponentValues, &archetypeChunkFilter.SharedComponentValues.FixedElementField);
			}
			else
			{
				for (int i = 0; i < srcArchetype->NumSharedComponents; i++)
				{
					*((ref archetypeChunkFilter.SharedComponentValues.FixedElementField) + (IntPtr)i * 4) = *sharedComponentValues[i];
				}
			}
			return archetypeChunkFilter;
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x000266E0 File Offset: 0x000248E0
		private unsafe bool GetArchetypeChunkFilterWithAddedComponent(ChunkIndex srcChunk, ComponentType componentType, int sharedComponentIndex, out EntityComponentStore.ArchetypeChunkFilter outFilter)
		{
			Archetype* archetype = this.GetArchetype(srcChunk);
			int num = 0;
			Archetype* archetypeWithAddedComponent = this.GetArchetypeWithAddedComponent(archetype, componentType, &num);
			if (archetypeWithAddedComponent == null)
			{
				outFilter = default(EntityComponentStore.ArchetypeChunkFilter);
				return false;
			}
			outFilter = this.GetArchetypeChunkFilterWithAddedComponent(archetype, srcChunk.ListIndex, archetypeWithAddedComponent, num, componentType, sharedComponentIndex);
			return true;
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x0002672C File Offset: 0x0002492C
		private unsafe EntityComponentStore.ArchetypeChunkFilter GetArchetypeChunkFilterWithAddedComponents(ChunkIndex srcChunk, Archetype* dstArchetype)
		{
			Archetype* archetype = this.GetArchetype(srcChunk);
			EntityComponentStore.ArchetypeChunkFilter archetypeChunkFilter = default(EntityComponentStore.ArchetypeChunkFilter);
			archetypeChunkFilter.Archetype = dstArchetype;
			int numSharedComponents = archetype->NumSharedComponents;
			SharedComponentValues sharedComponentValues = archetype->Chunks.GetSharedComponentValues(srcChunk.ListIndex);
			if (dstArchetype->NumSharedComponents > numSharedComponents)
			{
				EntityComponentStore.BuildSharedComponentIndicesWithAddedComponents(archetype, dstArchetype, sharedComponentValues, &archetypeChunkFilter.SharedComponentValues.FixedElementField);
			}
			else
			{
				for (int i = 0; i < numSharedComponents; i++)
				{
					*((ref archetypeChunkFilter.SharedComponentValues.FixedElementField) + (IntPtr)i * 4) = *sharedComponentValues[i];
				}
			}
			return archetypeChunkFilter;
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x000267BC File Offset: 0x000249BC
		private unsafe Archetype* GetArchetypeWithAddedComponents(Archetype* srcArchetype, in ComponentTypeSet componentTypeSet)
		{
			ComponentTypeInArchetype* types = srcArchetype->Types;
			int num = srcArchetype->TypesCount + componentTypeSet.Length;
			ComponentTypeInArchetype* ptr;
			checked
			{
				ptr = stackalloc ComponentTypeInArchetype[unchecked((UIntPtr)num) * (UIntPtr)sizeof(ComponentTypeInArchetype)];
			}
			int i = srcArchetype->TypesCount - 1;
			int j = componentTypeSet.Length - 1;
			int num2 = num;
			while (j >= 0)
			{
				ComponentTypeInArchetype componentTypeInArchetype = types[i];
				ComponentTypeInArchetype componentTypeInArchetype2 = new ComponentTypeInArchetype(componentTypeSet.GetTypeIndex(j));
				if (componentTypeInArchetype.TypeIndex > componentTypeInArchetype2.TypeIndex)
				{
					ptr[(IntPtr)(--num2) * (IntPtr)sizeof(ComponentTypeInArchetype)] = componentTypeInArchetype;
					i--;
				}
				else
				{
					if (componentTypeInArchetype.TypeIndex == componentTypeInArchetype2.TypeIndex)
					{
						i--;
					}
					ptr[(IntPtr)(--num2) * (IntPtr)sizeof(ComponentTypeInArchetype)] = componentTypeInArchetype2;
					j--;
				}
			}
			while (i >= 0)
			{
				ptr[(IntPtr)(--num2) * (IntPtr)sizeof(ComponentTypeInArchetype)] = types[(IntPtr)(i--) * (IntPtr)sizeof(ComponentTypeInArchetype)];
			}
			int num3 = num2;
			if (num3 == componentTypeSet.Length)
			{
				return srcArchetype;
			}
			return this.GetOrCreateArchetype(ptr + num3, num - num3);
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x000268F8 File Offset: 0x00024AF8
		private unsafe EntityComponentStore.ArchetypeChunkFilter GetArchetypeChunkFilterWithRemovedComponent(ChunkIndex srcChunk, Archetype* dstArchetype, int indexInTypeArray, ComponentType componentType)
		{
			Archetype* archetype = this.GetArchetype(srcChunk);
			EntityComponentStore.ArchetypeChunkFilter archetypeChunkFilter = default(EntityComponentStore.ArchetypeChunkFilter);
			archetypeChunkFilter.Archetype = dstArchetype;
			SharedComponentValues sharedComponentValues = archetype->Chunks.GetSharedComponentValues(srcChunk.ListIndex);
			if (componentType.IsSharedComponent)
			{
				EntityComponentStore.BuildSharedComponentIndicesWithRemovedComponent(indexInTypeArray - (int)archetype->FirstSharedComponent, dstArchetype->NumSharedComponents, sharedComponentValues, &archetypeChunkFilter.SharedComponentValues.FixedElementField);
			}
			else
			{
				for (int i = 0; i < archetype->NumSharedComponents; i++)
				{
					*((ref archetypeChunkFilter.SharedComponentValues.FixedElementField) + (IntPtr)i * 4) = *sharedComponentValues[i];
				}
			}
			return archetypeChunkFilter;
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x0002698C File Offset: 0x00024B8C
		private unsafe bool GetArchetypeChunkFilterWithRemovedComponent(ChunkIndex srcChunk, ComponentType componentType, out EntityComponentStore.ArchetypeChunkFilter outFilter)
		{
			Archetype* archetype = this.GetArchetype(srcChunk);
			int num = 0;
			Archetype* archetypeWithRemovedComponent = this.GetArchetypeWithRemovedComponent(archetype, componentType, &num);
			if (archetypeWithRemovedComponent == archetype)
			{
				outFilter = default(EntityComponentStore.ArchetypeChunkFilter);
				return false;
			}
			outFilter = this.GetArchetypeChunkFilterWithRemovedComponent(srcChunk, archetypeWithRemovedComponent, num, componentType);
			return true;
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x000269D0 File Offset: 0x00024BD0
		private unsafe EntityComponentStore.ArchetypeChunkFilter GetArchetypeChunkFilterWithRemovedComponents(ChunkIndex srcChunk, Archetype* dstArchetype)
		{
			Archetype* archetype = this.GetArchetype(srcChunk);
			EntityComponentStore.ArchetypeChunkFilter archetypeChunkFilter = default(EntityComponentStore.ArchetypeChunkFilter);
			archetypeChunkFilter.Archetype = dstArchetype;
			int numSharedComponents = archetype->NumSharedComponents;
			SharedComponentValues sharedComponentValues = archetype->Chunks.GetSharedComponentValues(srcChunk.ListIndex);
			if (dstArchetype->NumSharedComponents < numSharedComponents)
			{
				EntityComponentStore.BuildSharedComponentIndicesWithRemovedComponents(archetype, dstArchetype, sharedComponentValues, &archetypeChunkFilter.SharedComponentValues.FixedElementField);
			}
			else
			{
				for (int i = 0; i < numSharedComponents; i++)
				{
					*((ref archetypeChunkFilter.SharedComponentValues.FixedElementField) + (IntPtr)i * 4) = *sharedComponentValues[i];
				}
			}
			return archetypeChunkFilter;
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x00026A5D File Offset: 0x00024C5D
		private unsafe static void BuildSharedComponentIndicesWithAddedComponent(int indexOfNewSharedComponent, int value, int newCount, SharedComponentValues srcSharedComponentValues, int* dstSharedComponentValues)
		{
			srcSharedComponentValues.CopyTo(dstSharedComponentValues, 0, indexOfNewSharedComponent);
			dstSharedComponentValues[indexOfNewSharedComponent] = value;
			srcSharedComponentValues.CopyTo(dstSharedComponentValues + indexOfNewSharedComponent + 1, indexOfNewSharedComponent, newCount - indexOfNewSharedComponent - 1);
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x00026A89 File Offset: 0x00024C89
		private unsafe static void BuildSharedComponentIndicesWithRemovedComponent(int indexOfRemovedSharedComponent, int newCount, SharedComponentValues srcSharedComponentValues, int* dstSharedComponentValues)
		{
			srcSharedComponentValues.CopyTo(dstSharedComponentValues, 0, indexOfRemovedSharedComponent);
			srcSharedComponentValues.CopyTo(dstSharedComponentValues + indexOfRemovedSharedComponent, indexOfRemovedSharedComponent + 1, newCount - indexOfRemovedSharedComponent);
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x00026AA8 File Offset: 0x00024CA8
		private unsafe static void BuildSharedComponentIndicesWithAddedComponents(Archetype* srcArchetype, Archetype* dstArchetype, SharedComponentValues srcSharedComponentValues, int* dstSharedComponentValues)
		{
			int firstSharedComponent = (int)srcArchetype->FirstSharedComponent;
			int firstSharedComponent2 = (int)dstArchetype->FirstSharedComponent;
			int numSharedComponents = srcArchetype->NumSharedComponents;
			int numSharedComponents2 = dstArchetype->NumSharedComponents;
			int num = numSharedComponents - 1;
			for (int i = numSharedComponents2 - 1; i >= 0; i--)
			{
				if (dstArchetype->Types[i + firstSharedComponent2] == srcArchetype->Types[num + firstSharedComponent])
				{
					dstSharedComponentValues[i] = *srcSharedComponentValues[num--];
				}
				else
				{
					dstSharedComponentValues[i] = 0;
				}
			}
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x00026B3C File Offset: 0x00024D3C
		private unsafe static void BuildSharedComponentIndicesWithChangedArchetype(Archetype* srcArchetype, Archetype* dstArchetype, SharedComponentValues srcSharedComponentValues, int* dstSharedComponentValues)
		{
			int firstSharedComponent = (int)srcArchetype->FirstSharedComponent;
			int firstSharedComponent2 = (int)dstArchetype->FirstSharedComponent;
			int numSharedComponents = srcArchetype->NumSharedComponents;
			int numSharedComponents2 = dstArchetype->NumSharedComponents;
			int num = 0;
			int i = 0;
			while (i < numSharedComponents2)
			{
				if (num >= numSharedComponents)
				{
					break;
				}
				TypeIndex typeIndex = srcArchetype->Types[num + firstSharedComponent].TypeIndex;
				TypeIndex typeIndex2 = dstArchetype->Types[i + firstSharedComponent2].TypeIndex;
				if (typeIndex == typeIndex2)
				{
					dstSharedComponentValues[i++] = *srcSharedComponentValues[num++];
				}
				else if (typeIndex2 > typeIndex)
				{
					num++;
				}
				else
				{
					dstSharedComponentValues[i++] = 0;
				}
			}
			while (i < numSharedComponents2)
			{
				dstSharedComponentValues[i] = 0;
				i++;
			}
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x00026C10 File Offset: 0x00024E10
		private unsafe Archetype* GetArchetypeWithAddedComponent(Archetype* archetype, ComponentType addedComponentType, int* indexInTypeArray = null)
		{
			ComponentTypeInArchetype componentTypeInArchetype = new ComponentTypeInArchetype(addedComponentType);
			ComponentTypeInArchetype* ptr;
			int i;
			checked
			{
				ptr = stackalloc ComponentTypeInArchetype[unchecked((UIntPtr)(archetype->TypesCount + 1)) * (UIntPtr)sizeof(ComponentTypeInArchetype)];
				i = 0;
			}
			while (i < archetype->TypesCount && archetype->Types[i] < componentTypeInArchetype)
			{
				ptr[i] = archetype->Types[i];
				i++;
			}
			if (indexInTypeArray != null)
			{
				*indexInTypeArray = i;
			}
			if (i != archetype->TypesCount && archetype->Types[i] == componentTypeInArchetype)
			{
				return null;
			}
			ptr[i] = componentTypeInArchetype;
			while (i < archetype->TypesCount)
			{
				ptr[i + 1] = archetype->Types[i];
				i++;
			}
			return this.GetOrCreateArchetype(ptr, archetype->TypesCount + 1);
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x00026D14 File Offset: 0x00024F14
		private unsafe Archetype* GetArchetypeWithRemovedComponent(Archetype* archetype, ComponentType addedComponentType, int* indexInOldTypeArray = null)
		{
			ComponentTypeInArchetype componentTypeInArchetype = new ComponentTypeInArchetype(addedComponentType);
			ComponentTypeInArchetype* ptr;
			int num;
			checked
			{
				ptr = stackalloc ComponentTypeInArchetype[unchecked((UIntPtr)archetype->TypesCount) * (UIntPtr)sizeof(ComponentTypeInArchetype)];
				num = 0;
			}
			for (int i = 0; i < archetype->TypesCount; i++)
			{
				if (archetype->Types[i].TypeIndex == componentTypeInArchetype.TypeIndex)
				{
					if (indexInOldTypeArray != null)
					{
						*indexInOldTypeArray = i;
					}
					num++;
				}
				else
				{
					ptr[i - num] = archetype->Types[i];
				}
			}
			return this.GetOrCreateArchetype(ptr, archetype->TypesCount - num);
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x00026DB4 File Offset: 0x00024FB4
		private unsafe Archetype* GetArchetypeWithRemovedComponents(Archetype* archetype, in ComponentTypeSet typeSetToRemove)
		{
			ComponentTypeInArchetype* ptr;
			int num;
			checked
			{
				ptr = stackalloc ComponentTypeInArchetype[unchecked((UIntPtr)archetype->TypesCount) * (UIntPtr)sizeof(ComponentTypeInArchetype)];
				num = 0;
			}
			for (int i = 0; i < archetype->TypesCount; i++)
			{
				TypeIndex typeIndex = archetype->Types[i].TypeIndex;
				bool flag = false;
				for (int j = 0; j < typeSetToRemove.Length; j++)
				{
					if (typeIndex == typeSetToRemove.GetTypeIndex(j))
					{
						num++;
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					ptr[i - num] = archetype->Types[i];
				}
			}
			if (num == 0)
			{
				return archetype;
			}
			return this.GetOrCreateArchetype(ptr, archetype->TypesCount - num);
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x00026E6C File Offset: 0x0002506C
		private unsafe static void BuildSharedComponentIndicesWithRemovedComponents(Archetype* srcArchetype, Archetype* dstArchetype, SharedComponentValues srcSharedComponentValues, int* dstSharedComponentValues)
		{
			int firstSharedComponent = (int)srcArchetype->FirstSharedComponent;
			int firstSharedComponent2 = (int)dstArchetype->FirstSharedComponent;
			int numSharedComponents = srcArchetype->NumSharedComponents;
			int numSharedComponents2 = dstArchetype->NumSharedComponents;
			for (int i = 0; i < numSharedComponents2; i++)
			{
				ComponentTypeInArchetype componentTypeInArchetype = dstArchetype->Types[firstSharedComponent2 + i];
				int num = 0;
				for (int j = 0; j < numSharedComponents; j++)
				{
					if (srcArchetype->Types[firstSharedComponent + j] == componentTypeInArchetype)
					{
						num = j;
						break;
					}
				}
				dstSharedComponentValues[i] = *srcSharedComponentValues[num];
			}
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x00026F0C File Offset: 0x0002510C
		public unsafe void CreateEntities(Archetype* archetype, Entity* entities, int count)
		{
			EntityComponentStore.ArchetypeChunkFilter archetypeChunkFilter = default(EntityComponentStore.ArchetypeChunkFilter);
			archetypeChunkFilter.Archetype = archetype;
			while (count != 0)
			{
				ChunkIndex chunkWithEmptySlots = this.GetChunkWithEmptySlots(ref archetypeChunkFilter);
				int num = archetype->ChunkCapacity - chunkWithEmptySlots.Count;
				int num2 = math.min(count, num);
				ChunkDataUtility.Allocate(archetype, chunkWithEmptySlots, entities, num2);
				count -= num2;
				if (entities != null)
				{
					entities += num2;
				}
			}
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x00026F70 File Offset: 0x00025170
		public unsafe void DestroyEntities(EntityQueryImpl* queryImpl, ref BufferTypeHandle<LinkedEntityGroup> linkedEntityGroupTypeHandle)
		{
			UnsafeChunkCacheIterator unsafeChunkCacheIterator = new UnsafeChunkCacheIterator(in queryImpl->_Filter, queryImpl->_QueryData->HasEnableableComponents > 0, queryImpl->GetMatchingChunkCache(), queryImpl->_QueryData->MatchingArchetypes.Ptr);
			int num = -1;
			v128 v = default(v128);
			int length = unsafeChunkCacheIterator.Length;
			using (UnsafeList<EntityComponentStore.ChunkAndEnabledMask> unsafeList = new UnsafeList<EntityComponentStore.ChunkAndEnabledMask>(length, Allocator.Temp, NativeArrayOptions.UninitializedMemory))
			{
				ArchetypeChunk archetypeChunk;
				int num2;
				byte b;
				while (unsafeChunkCacheIterator.MoveNextChunk(ref num, out archetypeChunk, out num2, out b, ref v))
				{
					unsafeList.AddNoResize(new EntityComponentStore.ChunkAndEnabledMask
					{
						Chunk = archetypeChunk.m_Chunk,
						EnabledMask = v,
						ChunkEntityCount = num2,
						UseEnabledMask = b
					});
				}
				EntityBatchInChunk* ptr;
				int i;
				int length2;
				checked
				{
					ptr = stackalloc EntityBatchInChunk[unchecked((UIntPtr)128) * (UIntPtr)sizeof(EntityBatchInChunk)];
					i = 0;
					length2 = unsafeList.Length;
				}
				while (i < length2)
				{
					ChunkIndex chunk = unsafeList[i].Chunk;
					if (unsafeList[i].UseEnabledMask == 0)
					{
						EntityBatchInChunk entityBatchInChunk = default(EntityBatchInChunk);
						entityBatchInChunk.Chunk = chunk;
						entityBatchInChunk.StartIndex = 0;
						entityBatchInChunk.Count = unsafeList[i].ChunkEntityCount;
						this.DestroyBatch(in entityBatchInChunk);
					}
					else
					{
						int num3 = 0;
						int num4 = 0;
						v = unsafeList[i].EnabledMask;
						int num5;
						while (EnabledBitUtility.TryGetNextRange(v, num4, out num5, out num4))
						{
							ref EntityBatchInChunk ptr2 = ref ptr[(IntPtr)(num3++) * (IntPtr)sizeof(EntityBatchInChunk)];
							EntityBatchInChunk entityBatchInChunk = new EntityBatchInChunk
							{
								Chunk = chunk,
								Count = num4 - num5,
								StartIndex = num5
							};
							ptr2 = entityBatchInChunk;
						}
						for (int j = num3 - 1; j >= 0; j--)
						{
							this.DestroyBatch(in ptr[j]);
						}
					}
					i++;
				}
			}
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x00027160 File Offset: 0x00025360
		public unsafe void DestroyEntities(Entity* entities, int count)
		{
			int num = 0;
			UnsafeList<Entity> unsafeList = new UnsafeList<Entity>(0, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
			int maxValue = int.MaxValue;
			int num2 = 0;
			while (num != count)
			{
				EntityBatchInChunk firstEntityBatchInChunk = this.GetFirstEntityBatchInChunk(entities + num, count - num);
				ChunkIndex chunk = firstEntityBatchInChunk.Chunk;
				int count2 = firstEntityBatchInChunk.Count;
				int startIndex = firstEntityBatchInChunk.StartIndex;
				if (chunk == ChunkIndex.Null)
				{
					num += count2;
				}
				else
				{
					this.AddToDestroyList(chunk, startIndex, count2, count, ref unsafeList, ref maxValue, ref num2);
					EntityBatchInChunk entityBatchInChunk = default(EntityBatchInChunk);
					entityBatchInChunk.Chunk = chunk;
					entityBatchInChunk.StartIndex = startIndex;
					entityBatchInChunk.Count = count2;
					this.DestroyBatch(in entityBatchInChunk);
					num += count2;
				}
			}
			if (!unsafeList.IsEmpty)
			{
				Entity* ptr = unsafeList.Ptr;
				if (maxValue == num2)
				{
					Entity* ptr2 = (Entity*)Memory.Unmanaged.Allocate((long)(unsafeList.Length * sizeof(Entity)), 16, Allocator.TempJob);
					int num3 = unsafeList.Length / maxValue;
					for (int num4 = 0; num4 != num3; num4++)
					{
						for (int num5 = 0; num5 != maxValue; num5++)
						{
							ptr2[num5 * num3 + num4] = ptr[num4 * maxValue + num5];
						}
					}
					this.DestroyEntities(ptr2, unsafeList.Length);
					Memory.Unmanaged.Free<Entity>(ptr2, Allocator.TempJob);
				}
				else
				{
					this.DestroyEntities(ptr, unsafeList.Length);
				}
			}
			unsafeList.Dispose();
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x000272E0 File Offset: 0x000254E0
		public unsafe ChunkIndex GetCleanChunkNoMetaChunk(Archetype* archetype, SharedComponentValues sharedComponentValues)
		{
			ChunkIndex chunkIndex = this.AllocateChunk();
			ChunkDataUtility.AddEmptyChunk(archetype, chunkIndex, sharedComponentValues);
			return chunkIndex;
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x00027300 File Offset: 0x00025500
		public unsafe ChunkIndex GetCleanChunk(Archetype* archetype, SharedComponentValues sharedComponentValues)
		{
			ChunkIndex chunkIndex = this.AllocateChunk();
			ChunkDataUtility.AddEmptyChunk(archetype, chunkIndex, sharedComponentValues);
			if (archetype->MetaChunkArchetype != null)
			{
				this.CreateMetaEntityForChunk(chunkIndex);
			}
			return chunkIndex;
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x00027330 File Offset: 0x00025530
		public unsafe void InstantiateEntities(Entity srcEntity, Entity* outputEntities, int instanceCount)
		{
			bool flag;
			if (this.HasComponent(srcEntity, this.m_LinkedGroupType, out flag))
			{
				BufferHeader* componentDataWithTypeRO = (BufferHeader*)this.GetComponentDataWithTypeRO(srcEntity, this.m_LinkedGroupType);
				Entity* elementPointer = (Entity*)BufferHeader.GetElementPointer(componentDataWithTypeRO);
				int length = componentDataWithTypeRO->Length;
				this.InstantiateEntitiesGroup(elementPointer, length, outputEntities, true, instanceCount, true);
				return;
			}
			this.InstantiateEntitiesOne(srcEntity, outputEntities, instanceCount, null, 0, true);
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x00027385 File Offset: 0x00025585
		public unsafe void InstantiateEntities(Entity* srcEntity, Entity* outputEntities, int entityCount, bool removePrefab)
		{
			this.InstantiateEntitiesGroup(srcEntity, entityCount, outputEntities, false, 1, removePrefab);
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x00027394 File Offset: 0x00025594
		internal unsafe void DestroyMetaChunkEntity(Entity entity)
		{
			this.RemoveComponent(entity, this.m_ChunkHeaderComponentType);
			this.DestroyEntities(&entity, 1);
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x000273B0 File Offset: 0x000255B0
		internal unsafe void CreateMetaEntityForChunk(ChunkIndex chunk)
		{
			fixed (EntityComponentStore* ptr = &this)
			{
				EntityComponentStore* ptr2 = ptr;
				Archetype* archetype = ptr2->GetArchetype(chunk);
				Entity @null = Entity.Null;
				this.CreateEntities(archetype->MetaChunkArchetype, &@null, 1);
				chunk.MetaChunkEntity = @null;
				ChunkHeader* componentDataWithTypeRW = (ChunkHeader*)this.GetComponentDataWithTypeRW(@null, this.m_ChunkHeaderType, this.GlobalSystemVersion);
				componentDataWithTypeRW->ArchetypeChunk = new ArchetypeChunk(chunk, ptr2);
			}
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x00027410 File Offset: 0x00025610
		private unsafe void AddToDestroyList(ChunkIndex chunk, int indexInChunk, int batchCount, int inputDestroyCount, ref UnsafeList<Entity> entitiesList, ref int minBufferLength, ref int maxBufferLength)
		{
			Archetype* archetype = this.GetArchetype(chunk);
			int indexInTypeArray = ChunkDataUtility.GetIndexInTypeArray(archetype, this.m_LinkedGroupType);
			if (indexInTypeArray != -1)
			{
				byte* componentDataWithTypeRO = ChunkDataUtility.GetComponentDataWithTypeRO(chunk, archetype, indexInChunk, this.m_LinkedGroupType);
				ushort num = archetype->SizeOfs[indexInTypeArray];
				for (int num2 = 0; num2 != batchCount; num2++)
				{
					BufferHeader* ptr = (BufferHeader*)(componentDataWithTypeRO + (int)num * num2);
					int num3 = ptr->Length - 1;
					if (num3 > 0)
					{
						Entity* ptr2 = (Entity*)(BufferHeader.GetElementPointer(ptr) + sizeof(Entity));
						if (entitiesList.Capacity == 0)
						{
							entitiesList.SetCapacity(inputDestroyCount * num3);
						}
						entitiesList.AddRange((void*)ptr2, num3);
						minBufferLength = math.min(minBufferLength, num3);
						maxBufferLength = math.max(maxBufferLength, num3);
					}
				}
			}
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x000274C8 File Offset: 0x000256C8
		private unsafe void DestroyBatch(in EntityBatchInChunk batch)
		{
			ChunkIndex chunk = batch.Chunk;
			Archetype* archetype = this.GetArchetype(chunk);
			if (!archetype->CleanupNeeded)
			{
				ChunkDataUtility.Deallocate(archetype, in batch);
				return;
			}
			int startIndex = batch.StartIndex;
			int count = batch.Count;
			Archetype* cleanupResidueArchetype = archetype->CleanupResidueArchetype;
			if (archetype == cleanupResidueArchetype)
			{
				return;
			}
			EntityComponentStore.ArchetypeChunkFilter archetypeChunkFilter = default(EntityComponentStore.ArchetypeChunkFilter);
			archetypeChunkFilter.Archetype = cleanupResidueArchetype;
			SharedComponentValues sharedComponentValues = archetype->Chunks.GetSharedComponentValues(chunk.ListIndex);
			if (this.RequiresBuildingResidueSharedComponentIndices(archetype, archetypeChunkFilter.Archetype))
			{
				this.BuildResidueSharedComponentIndices(archetype, archetypeChunkFilter.Archetype, sharedComponentValues, &archetypeChunkFilter.SharedComponentValues.FixedElementField);
			}
			else
			{
				sharedComponentValues.CopyTo(&archetypeChunkFilter.SharedComponentValues.FixedElementField, 0, archetype->NumSharedComponents);
			}
			if (count == chunk.Count)
			{
				this.Move(chunk, ref archetypeChunkFilter);
				return;
			}
			this.Move(new EntityBatchInChunk
			{
				Chunk = chunk,
				StartIndex = startIndex,
				Count = count
			}, ref archetypeChunkFilter);
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x000275BD File Offset: 0x000257BD
		private unsafe bool RequiresBuildingResidueSharedComponentIndices(Archetype* srcArchetype, Archetype* dstArchetype)
		{
			return dstArchetype->NumSharedComponents > 0 && dstArchetype->NumSharedComponents != srcArchetype->NumSharedComponents;
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x000275DC File Offset: 0x000257DC
		private unsafe void BuildResidueSharedComponentIndices(Archetype* srcArchetype, Archetype* dstArchetype, SharedComponentValues srcSharedComponentValues, int* dstSharedComponentValues)
		{
			int firstSharedComponent = (int)srcArchetype->FirstSharedComponent;
			int firstSharedComponent2 = (int)dstArchetype->FirstSharedComponent;
			int numSharedComponents = dstArchetype->NumSharedComponents;
			int num = 0;
			int i = 0;
			while (i < numSharedComponents)
			{
				ComponentTypeInArchetype componentTypeInArchetype = dstArchetype->Types[i + firstSharedComponent2];
				while (componentTypeInArchetype != srcArchetype->Types[num + firstSharedComponent])
				{
					num++;
				}
				dstSharedComponentValues[i] = *srcSharedComponentValues[num];
				i++;
				num++;
			}
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x00027668 File Offset: 0x00025868
		private unsafe int InstantiateEntitiesOne(Entity srcEntity, Entity* outputEntities, int instanceCount, EntityComponentStore.InstantiateRemapChunk* remapChunks, int remapChunksCount, bool removePrefab)
		{
			EntityInChunk entityInChunk = this.GetEntityInChunk(srcEntity);
			Archetype* archetype = this.GetArchetype(entityInChunk.Chunk);
			Archetype* ptr = (removePrefab ? archetype->InstantiateArchetype : archetype->CopyArchetype);
			EntityComponentStore.ArchetypeChunkFilter archetypeChunkFilter = default(EntityComponentStore.ArchetypeChunkFilter);
			archetypeChunkFilter.Archetype = ptr;
			SharedComponentValues sharedComponentValues = archetype->Chunks.GetSharedComponentValues(entityInChunk.Chunk.ListIndex);
			if (this.RequiresBuildingResidueSharedComponentIndices(archetype, ptr))
			{
				this.BuildResidueSharedComponentIndices(archetype, ptr, sharedComponentValues, &archetypeChunkFilter.SharedComponentValues.FixedElementField);
			}
			else
			{
				sharedComponentValues.CopyTo(&archetypeChunkFilter.SharedComponentValues.FixedElementField, 0, ptr->NumSharedComponents);
			}
			int num3;
			for (int num = 0; num != instanceCount; num += num3)
			{
				ChunkIndex chunkWithEmptySlots = this.GetChunkWithEmptySlots(ref archetypeChunkFilter);
				int count = chunkWithEmptySlots.Count;
				int num2 = ptr->ChunkCapacity - count;
				num3 = math.min(instanceCount - num, num2);
				ChunkDataUtility.AllocateClone(ptr, chunkWithEmptySlots, outputEntities + num, num3, srcEntity);
				if (remapChunks != null)
				{
					remapChunks[remapChunksCount].Chunk = chunkWithEmptySlots;
					remapChunks[remapChunksCount].IndexInChunk = count;
					remapChunks[remapChunksCount].AllocatedCount = num3;
					remapChunks[remapChunksCount].InstanceBeginIndex = num;
					remapChunksCount++;
				}
			}
			return remapChunksCount;
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x000277BC File Offset: 0x000259BC
		private unsafe void InstantiateEntitiesGroup(Entity* srcEntities, int srcEntityCount, Entity* outputRootEntities, bool outputRootEntityOnly, int instanceCount, bool removePrefab)
		{
			int num = srcEntityCount * instanceCount;
			int num2 = sizeof(Entity) * num + sizeof(EntityComponentStore.InstantiateRemapChunk) * num + sizeof(Entity) * instanceCount;
			byte* ptr;
			if (num2 > 16384)
			{
				ptr = (byte*)Memory.Unmanaged.Allocate((long)num2, 16, Allocator.Temp);
			}
			else
			{
				ptr = stackalloc byte[(UIntPtr)num2];
			}
			Entity* ptr2 = (Entity*)ptr;
			EntityComponentStore.InstantiateRemapChunk* ptr3 = (EntityComponentStore.InstantiateRemapChunk*)(ptr2 + num);
			Entity* ptr4 = (Entity*)(ptr3 + num);
			int num3 = 0;
			for (int num4 = 0; num4 != srcEntityCount; num4++)
			{
				Entity entity = srcEntities[num4];
				num3 = this.InstantiateEntitiesOne(entity, ptr4, instanceCount, ptr3, num3, removePrefab);
				for (int num5 = 0; num5 != instanceCount; num5++)
				{
					ptr2[num5 * srcEntityCount + num4] = ptr4[num5];
				}
				if (outputRootEntityOnly)
				{
					if (num4 == 0)
					{
						for (int num6 = 0; num6 != instanceCount; num6++)
						{
							outputRootEntities[num6] = ptr4[num6];
						}
					}
				}
				else
				{
					for (int num7 = 0; num7 != instanceCount; num7++)
					{
						outputRootEntities[num7 * srcEntityCount + num4] = ptr4[num7];
					}
				}
			}
			for (int num8 = 0; num8 != num3; num8++)
			{
				ChunkIndex chunk = ptr3[num8].Chunk;
				Archetype* archetype = this.GetArchetype(chunk);
				int allocatedCount = ptr3[num8].AllocatedCount;
				int indexInChunk = ptr3[num8].IndexInChunk;
				int instanceBeginIndex = ptr3[num8].InstanceBeginIndex;
				Entity* ptr5 = ptr2 + instanceBeginIndex * srcEntityCount;
				EntityRemapUtility.PatchEntitiesForPrefab(archetype->ScalarEntityPatches + 1, archetype->ScalarEntityPatchCount - 1, archetype->BufferEntityPatches, archetype->BufferEntityPatchCount, chunk.Buffer, indexInChunk, allocatedCount, srcEntities, ptr5, srcEntityCount);
				if (archetype->HasManagedEntityRefs)
				{
					this.ManagedChangesTracker.PatchEntitiesForPrefab(archetype, chunk, indexInChunk, allocatedCount, srcEntities, ptr5, srcEntityCount, Allocator.Temp);
				}
			}
			if (num2 > 16384)
			{
				Memory.Unmanaged.Free<byte>(ptr, Allocator.Temp);
			}
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x00027A0C File Offset: 0x00025C0C
		private unsafe EntityBatchInChunk GetFirstEntityBatchInChunk(Entity* entities, int count)
		{
			EntityInChunk entityInChunk = (this.Exists(*entities) ? this.GetEntityInChunk(*entities) : default(EntityInChunk));
			ChunkIndex chunk = entityInChunk.Chunk;
			int indexInChunk = entityInChunk.IndexInChunk;
			int i;
			for (i = 0; i < count; i++)
			{
				Entity entity = entities[i];
				entityInChunk = (this.Exists(entity) ? this.GetEntityInChunk(entity) : default(EntityInChunk));
				if (entityInChunk.Chunk != chunk || entityInChunk.IndexInChunk != indexInChunk + i)
				{
					break;
				}
			}
			return new EntityBatchInChunk
			{
				Chunk = chunk,
				Count = i,
				StartIndex = indexInChunk
			};
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x00027AC8 File Offset: 0x00025CC8
		public unsafe static JobHandle GetCreatedAndDestroyedEntities(EntityComponentStore* store, NativeList<int> state, NativeList<Entity> createdEntities, NativeList<Entity> destroyedEntities, bool async)
		{
			JobHandle jobHandle = default(JobHandle);
			EntityComponentStore.GetCreatedAndDestroyedEntities(store, ref state, ref createdEntities, ref destroyedEntities, ref jobHandle, async);
			return jobHandle;
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x00027AEE File Offset: 0x00025CEE
		[BurstCompile]
		private unsafe static void GetCreatedAndDestroyedEntities(EntityComponentStore* store, ref NativeList<int> state, ref NativeList<Entity> createdEntities, ref NativeList<Entity> destroyedEntities, ref JobHandle jobHandle, bool async)
		{
			EntityComponentStore.GetCreatedAndDestroyedEntities_000006AD$BurstDirectCall.Invoke(store, ref state, ref createdEntities, ref destroyedEntities, ref jobHandle, async);
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x00027B00 File Offset: 0x00025D00
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public unsafe void CheckInternalConsistency(object[] managedComponentData)
		{
			UnsafeBitArray unsafeBitArray = new UnsafeBitArray(this.m_ManagedComponentIndex, Allocator.Temp, NativeArrayOptions.ClearMemory);
			UnsafeParallelHashSet<ulong> unsafeParallelHashSet = new UnsafeParallelHashSet<ulong>(512, Allocator.Temp);
			for (int i = this.m_ManagedComponentIndex; i < managedComponentData.Length; i++)
			{
			}
			fixed (EntityComponentStore* ptr = &this)
			{
				EntityComponentStore* ptr2 = ptr;
			}
			int num = 0;
			for (int j = 0; j < this.m_Archetypes.Length; j++)
			{
				Archetype* ptr3 = *(IntPtr*)(this.m_Archetypes.Ptr + (IntPtr)j * (IntPtr)sizeof(Archetype*) / (IntPtr)sizeof(Archetype*));
				int firstManagedComponent = (int)ptr3->FirstManagedComponent;
				int managedComponentsEnd = ptr3->ManagedComponentsEnd;
				for (int k = 0; k < ptr3->TypesCount; k++)
				{
					int num2 = ptr3->TypeIndexInArchetypeToMemoryOrderIndex[k];
				}
				int num3 = 0;
				int chunkCapacity = ptr3->ChunkCapacity;
				for (int l = 0; l < ptr3->Chunks.Count; l++)
				{
					ChunkIndex chunkIndex = ptr3->Chunks[l];
					int count = chunkIndex.Count;
					Entity* buffer = (Entity*)chunkIndex.Buffer;
					if (chunkIndex.SequenceNumber == 0UL)
					{
						throw new ArgumentException("Sequence number must not be 0");
					}
					if (!unsafeParallelHashSet.Add(chunkIndex.SequenceNumber))
					{
						throw new ArgumentException("Sequence number must be unique");
					}
					if (count < chunkCapacity)
					{
						int numSharedComponents = ptr3->NumSharedComponents;
					}
					num3 += count;
					bool hasChunkHeader = ptr3->HasChunkHeader;
					Entity metaChunkEntity = chunkIndex.MetaChunkEntity;
					if (metaChunkEntity != Entity.Null)
					{
						TypeIndex typeIndex = TypeManager.GetTypeIndex<ChunkHeader>();
						ChunkHeader chunkHeader = *(ChunkHeader*)this.GetComponentDataWithTypeRO(metaChunkEntity, typeIndex);
						this.GetChunk(metaChunkEntity);
					}
					for (int m = firstManagedComponent; m < managedComponentsEnd; m++)
					{
						int* ptr4 = (int*)(chunkIndex.Buffer + ptr3->Offsets[m]);
						int n = 0;
						int num4 = count;
						while (n < num4)
						{
							int num5 = ptr4[n];
							if (num5 != 0)
							{
								managedComponentData[num5].GetType() != TypeManager.GetType(ptr3->Types[m].TypeIndex);
								unsafeBitArray.Set(num5, true);
							}
							n++;
						}
					}
				}
				num += num3;
			}
			unsafeParallelHashSet.Dispose();
			for (int num6 = 0; num6 < this.m_ManagedComponentIndex; num6++)
			{
			}
			int* ptr5 = (int*)this.m_ManagedComponentFreeIndex.Ptr;
			int num7 = this.m_ManagedComponentFreeIndex.Length / 4;
			for (int num8 = 0; num8 < num7; num8++)
			{
				int num9 = ptr5[num8];
				unsafeBitArray.Set(num9, true);
			}
			unsafeBitArray.Dispose();
			EntityComponentStore.s_entityStore.Data.IntegrityCheck();
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x00002287 File Offset: 0x00000487
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public unsafe static void AssertAllEntitiesCopied(EntityComponentStore* lhs, EntityComponentStore* rhs)
		{
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x00027D94 File Offset: 0x00025F94
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public void ValidateEntity(Entity entity)
		{
			if (entity.Index < 0)
			{
				throw new ArgumentException(string.Format("All entities created using EntityCommandBuffer.CreateEntity must be realized via playback(). One of the entities is still deferred (Index: {0}).", entity.Index));
			}
			int num = 134217728;
			if (entity.Index >= num)
			{
				throw new ArgumentException("An Entity index is higher than the valid range. This means the entity.Index got corrupted or incorrectly assigned and it may not be used.");
			}
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x00027DE0 File Offset: 0x00025FE0
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private unsafe void AssertArchetypeComponents(ComponentTypeInArchetype* types, int count)
		{
			if (count < 1)
			{
				throw new ArgumentException("Invalid component count");
			}
			if (count >= 32767)
			{
				throw new ArgumentException(string.Format("Archetypes can have a maximum of {0} components.", short.MaxValue));
			}
			if (types->TypeIndex == TypeIndex.Null)
			{
				throw new ArgumentException("Component type may not be null");
			}
			if (types->TypeIndex != this.m_EntityType)
			{
				throw new ArgumentException("The Entity ID must always be the first component");
			}
			for (int i = 1; i < count; i++)
			{
				if (types[i - 1].TypeIndex == types[i].TypeIndex)
				{
					throw new ArgumentException(string.Format("It is not allowed to have two components of the same type on the same entity. ({0} and {1})", types[i - 1], types[i]));
				}
				if (this.GetTypeInfo(types[i].TypeIndex).SizeInChunk > 65535)
				{
					throw new ArgumentException(string.Format("IComponentData {0} is too large. SizeOf may not be larger than {1}", types[i], ushort.MaxValue));
				}
			}
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x00027F24 File Offset: 0x00026124
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public unsafe void AssertCanCreateArchetype(ComponentType* componentTypes, int componentTypeCount)
		{
			int num = EntityComponentStore.GetComponentArraySize(this.GetTypeInfo(this.m_EntityType)->SizeInChunk, 1);
			for (int i = 0; i < componentTypeCount; i++)
			{
				int componentArraySize = EntityComponentStore.GetComponentArraySize(this.GetTypeInfo(componentTypes[i].TypeIndex)->SizeInChunk, 1);
				num += componentArraySize;
			}
			int num2 = 16320;
			if (num > num2)
			{
				throw new ArgumentException(string.Format("Archetype too large to fit in chunk. Instance size {0} bytes.  Maximum chunk size {1}. Components in new archetype: {2}", num, num2, EntityComponentStore.AggregateNewArchetypesComponentTypes(componentTypes, componentTypeCount)));
			}
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x00002287 File Offset: 0x00000487
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssertEntityExists(Entity entity)
		{
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x00027FB4 File Offset: 0x000261B4
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public unsafe void AssertEntitiesExist(Entity* entities, int count)
		{
			for (int i = 0; i < count; i++)
			{
				Entity* ptr = entities + i;
				if (!EntityComponentStore.s_entityStore.Data.Exists(*ptr))
				{
					throw new ArgumentException("An EntityManager command is operating on an invalid entity. This usually means that the Entity has already been destroyed or was never created. This can happen when a subscene is opened or closed between when a EntityCommandBuffer has been recorded and played back. This can invalidate recorded Entities." + EntityComponentStore.AppendDestroyedEntityRecordError(*ptr));
				}
			}
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x0002800C File Offset: 0x0002620C
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public unsafe void AssertValidEntities(Entity* entities, int count)
		{
			for (int i = 0; i < count; i++)
			{
				IntPtr intPtr = (IntPtr)i;
			}
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x00028028 File Offset: 0x00026228
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public void AssertEntityHasComponent(Entity entity, ComponentType componentType)
		{
			bool flag;
			if (this.HasComponent(entity, componentType, out flag))
			{
				return;
			}
			if (!flag)
			{
				throw new ArgumentException("The entity does not exist." + EntityComponentStore.AppendDestroyedEntityRecordError(entity));
			}
			throw new ArgumentException(string.Format("A component with type:{0} has not been added to the entity.", componentType) + EntityComponentStore.AppendRemovedComponentRecordError(entity, componentType));
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x0002807C File Offset: 0x0002627C
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public void AssertEntityHasComponent(NativeArray<Entity> entities, ComponentType componentType)
		{
			int i = 0;
			while (i < entities.Length)
			{
				Entity entity = entities[i];
				bool flag;
				if (!this.HasComponent(entity, componentType, out flag))
				{
					if (!flag)
					{
						throw new ArgumentException("The entity does not exist." + EntityComponentStore.AppendDestroyedEntityRecordError(entity));
					}
					throw new ArgumentException(string.Format("A component with type:{0} has not been added to the entity.", componentType) + EntityComponentStore.AppendRemovedComponentRecordError(entity, componentType));
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x00002287 File Offset: 0x00000487
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public void AssertEntityHasComponent(Entity entity, TypeIndex componentTypeIndex)
		{
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x000280EC File Offset: 0x000262EC
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public void AssertEntityHasComponent(Entity entity, TypeIndex componentTypeIndex, ref LookupCache typeLookupCache)
		{
			bool flag;
			if (Hint.Likely(this.HasComponent(entity, componentTypeIndex, ref typeLookupCache, out flag)))
			{
				return;
			}
			if (Hint.Unlikely(!flag))
			{
				throw new ArgumentException("The entity does not exist." + EntityComponentStore.AppendDestroyedEntityRecordError(entity));
			}
			throw new ArgumentException(string.Format("A component with type:{0} has not been added to the entity.", componentTypeIndex) + EntityComponentStore.AppendRemovedComponentRecordError(entity, ComponentType.FromTypeIndex(componentTypeIndex)));
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x00028154 File Offset: 0x00026354
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public unsafe void AssertNonEmptyArchetypesHaveComponent(UnsafeMatchingArchetypePtrList archetypeList, ComponentType componentType)
		{
			int length = archetypeList.Length;
			MatchingArchetype** ptr = archetypeList.Ptr;
			TypeIndex typeIndex = componentType.TypeIndex;
			for (int i = 0; i < length; i++)
			{
				if (Hint.Unlikely(((IntPtr*)(ptr + (IntPtr)i * (IntPtr)sizeof(MatchingArchetype*) / (IntPtr)sizeof(MatchingArchetype*)))->Archetype->EntityCount > 0 && ChunkDataUtility.GetIndexInTypeArray(((IntPtr*)(ptr + (IntPtr)i * (IntPtr)sizeof(MatchingArchetype*) / (IntPtr)sizeof(MatchingArchetype*)))->Archetype, typeIndex) == -1))
				{
					throw new ArgumentException(string.Format("Archetype does not have component {0}", componentType));
				}
			}
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x000281D7 File Offset: 0x000263D7
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public void AssertComponentIsUnmanaged(TypeIndex componentTypeIndex)
		{
			if (TypeManager.IsManagedType(componentTypeIndex))
			{
				throw new ArgumentException("Component type is managed. Can not get pointer to data");
			}
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x000281EC File Offset: 0x000263EC
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public unsafe void AssertCanAddComponent(Archetype* archetype, ComponentType componentType)
		{
			if (componentType == this.m_EntityComponentType)
			{
				throw new ArgumentException("Cannot add Entity as a component.");
			}
			if (componentType.IsSharedComponent && archetype->NumSharedComponents == 16)
			{
				throw new InvalidOperationException(string.Format("Cannot add more than {0} SharedComponent's to a single Archetype. Attempting to add '{1}'. Archetype already contains types ({2}).", 16, componentType, EntityComponentStore.AggregateArchetypeComponentTypes(archetype)));
			}
			int componentArraySize = EntityComponentStore.GetComponentArraySize(this.GetTypeInfo(componentType.TypeIndex)->SizeInChunk, 1);
			int num = archetype->InstanceSizeWithOverhead + componentArraySize;
			int num2 = 16320;
			if (num > num2)
			{
				throw new InvalidOperationException(string.Format("Entity archetype component data is too large. Previous archetype size per instance {0} bytes. Attempting to add component '{1}' with size {2} bytes. Maximum chunk size {3}.", new object[]
				{
					archetype->InstanceSizeWithOverhead,
					componentType,
					componentArraySize,
					num2
				}));
			}
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x000282B8 File Offset: 0x000264B8
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public unsafe void AssertCanAddComponents(Archetype* archetype, in ComponentTypeSet componentTypeSet)
		{
			int num = 0;
			for (int i = 0; i < componentTypeSet.Length; i++)
			{
				ComponentType componentType = componentTypeSet.GetComponentType(i);
				if (componentType == this.m_EntityComponentType)
				{
					throw new ArgumentException("Cannot add Entity as a component.");
				}
				TypeManager.TypeInfo typeInfo = *this.GetTypeInfo(componentType.TypeIndex);
				num += EntityComponentStore.GetComponentArraySize(typeInfo.SizeInChunk, 1);
			}
			if (componentTypeSet.m_masks.SharedComponents + archetype->NumSharedComponents > 16)
			{
				throw new InvalidOperationException(string.Format("Cannot add more than {0} SharedComponent's to a single Archetype. Attempting to add types {1}. Archetype already contains types ({2}).", 16, EntityComponentStore.AggregateComponentTypes(in componentTypeSet), EntityComponentStore.AggregateArchetypeComponentTypes(archetype)));
			}
			int num2 = archetype->InstanceSizeWithOverhead + num;
			int num3 = 16320;
			if (num2 > num3)
			{
				throw new InvalidOperationException(string.Format("Entity archetype component data is too large. Previous archetype size per instance {0} bytes. Attempting to add multiple components ({1}) with a combined size {2} bytes. Maximum chunk size {3}. Archetype already contains types ({4}).", new object[]
				{
					archetype->InstanceSizeWithOverhead,
					EntityComponentStore.AggregateComponentTypes(in componentTypeSet),
					num,
					num3,
					EntityComponentStore.AggregateArchetypeComponentTypes(archetype)
				}));
			}
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x000283B0 File Offset: 0x000265B0
		private static string AggregateComponentTypes(in ComponentTypeSet componentTypeSet)
		{
			string text = string.Format("{0}: ", componentTypeSet.Length);
			for (int i = 0; i < componentTypeSet.Length; i++)
			{
				ComponentType componentType = componentTypeSet.GetComponentType(i);
				text += string.Format("'{0}'", componentType);
				if (i < componentTypeSet.Length - 1)
				{
					text += ", ";
				}
			}
			return text;
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x0002841C File Offset: 0x0002661C
		private unsafe static string AggregateArchetypeComponentTypes(Archetype* archetype)
		{
			int typesCount = archetype->TypesCount;
			string text = string.Format("{0}: ", typesCount);
			for (int i = 0; i < typesCount; i++)
			{
				text += string.Format("'{0}'", TypeManager.GetType(archetype->Types[i].TypeIndex));
				if (i < typesCount - 1)
				{
					text += ", ";
				}
			}
			return text;
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x0002848C File Offset: 0x0002668C
		private unsafe static string AggregateNewArchetypesComponentTypes(ComponentType* componentTypes, int componentTypeCount)
		{
			string text = string.Format("{0}: ", componentTypeCount);
			for (int i = 0; i < componentTypeCount; i++)
			{
				TypeIndex typeIndex = componentTypes[i].TypeIndex;
				text += string.Format("'{0}'", TypeManager.GetType(typeIndex));
				if (i < componentTypeCount - 1)
				{
					text += ", ";
				}
			}
			return text;
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x000284F1 File Offset: 0x000266F1
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public void AssertCanAddComponent(Entity entity, ComponentType componentType)
		{
			if (!this.Exists(entity))
			{
				throw new InvalidOperationException("The entity does not exist." + EntityComponentStore.AppendDestroyedEntityRecordError(entity));
			}
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x00002287 File Offset: 0x00000487
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public void AssertCanAddComponent(Entity entity, TypeIndex componentType)
		{
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x00028514 File Offset: 0x00026714
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public unsafe void AssertCanAddComponent(EntityQueryImpl* queryImpl, ComponentType componentType)
		{
			fixed (EntityComponentStore* ptr = &this)
			{
				EntityComponentStore* ptr2 = ptr;
				if (Hint.Unlikely(queryImpl->_Access->EntityComponentStore != ptr2))
				{
					throw new InvalidOperationException("Provided query belongs to a different World.");
				}
			}
			UnsafeMatchingArchetypePtrList matchingArchetypes = queryImpl->_QueryData->MatchingArchetypes;
			int length = matchingArchetypes.Length;
			MatchingArchetype** ptr3 = matchingArchetypes.Ptr;
			for (int i = 0; i < length; i++)
			{
			}
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x0002857C File Offset: 0x0002677C
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public unsafe void AssertCanAddComponents(UnsafeMatchingArchetypePtrList archetypeList, in ComponentTypeSet componentTypeSet)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < componentTypeSet.Length; i++)
			{
				ComponentType componentType = componentTypeSet.GetComponentType(i);
				if (componentType == this.m_EntityComponentType)
				{
					throw new ArgumentException("Cannot add Entity as a component.");
				}
				if (componentType.IsSharedComponent)
				{
					num++;
				}
				num2 += EntityComponentStore.GetComponentArraySize(this.GetTypeInfo(componentType.TypeIndex).SizeInChunk, 1);
			}
			int length = archetypeList.Length;
			MatchingArchetype** ptr = archetypeList.Ptr;
			for (int j = 0; j < length; j++)
			{
				Archetype* archetype = ((IntPtr*)(ptr + (IntPtr)j * (IntPtr)sizeof(MatchingArchetype*) / (IntPtr)sizeof(MatchingArchetype*)))->Archetype;
				if (archetype->NumSharedComponents + num > 16)
				{
					throw new InvalidOperationException(string.Format("Cannot add more than {0} SharedComponent's to a single Archetype. Attempting to add types {1}. Archetype already contains types ({2}).", 16, EntityComponentStore.AggregateComponentTypes(in componentTypeSet), EntityComponentStore.AggregateArchetypeComponentTypes(archetype)));
				}
				if (archetype->InstanceSizeWithOverhead + num2 > 16320)
				{
					throw new InvalidOperationException(string.Format("Entity archetype component data is too large. Previous archetype size per instance {0} bytes. Attempting to add components {1} with total size {2} bytes. Maximum chunk size {3}. Archetype already contains types ({4}).", new object[]
					{
						archetype->InstanceSizeWithOverhead,
						EntityComponentStore.AggregateComponentTypes(in componentTypeSet),
						num2,
						16320,
						EntityComponentStore.AggregateArchetypeComponentTypes(archetype)
					}));
				}
			}
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x000284F1 File Offset: 0x000266F1
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public void AssertCanAddComponents(Entity entity, in ComponentTypeSet typeSet)
		{
			if (!this.Exists(entity))
			{
				throw new InvalidOperationException("The entity does not exist." + EntityComponentStore.AppendDestroyedEntityRecordError(entity));
			}
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x000286B8 File Offset: 0x000268B8
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public void AssertCanAddComponents(NativeArray<Entity> entities, in ComponentTypeSet typeSet)
		{
			for (int i = 0; i < entities.Length; i++)
			{
				Entity entity = entities[i];
			}
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x000286E0 File Offset: 0x000268E0
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public void AssertCanAddComponent(NativeArray<Entity> entities, ComponentType type)
		{
			for (int i = 0; i < entities.Length; i++)
			{
				Entity entity = entities[i];
			}
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x00028708 File Offset: 0x00026908
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public unsafe void AssertCanAddComponent(NativeList<EntityBatchInChunk> batches, ComponentType componentType)
		{
			Archetype* ptr = null;
			for (int i = 0; i < batches.Length; i++)
			{
				Archetype* archetype = this.GetArchetype(batches[i].Chunk);
				if (archetype != ptr)
				{
					UIntPtr uintPtr = (UIntPtr)0;
					ptr = archetype;
				}
			}
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x00028749 File Offset: 0x00026949
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public void AssertCanRemoveComponent(ComponentType componentType)
		{
			if (componentType == this.m_EntityComponentType)
			{
				throw new ArgumentException("Cannot remove Entity as a component. Use DestroyEntity if you want to delete Entity and all associated components.");
			}
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x00028764 File Offset: 0x00026964
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public void AssertCanRemoveComponents(in ComponentTypeSet typeSet)
		{
			for (int i = 0; i < typeSet.Length; i++)
			{
			}
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x00028784 File Offset: 0x00026984
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private unsafe void AssertWillDestroyAllInLinkedEntityGroup(UnsafeList<EntityComponentStore.ChunkAndEnabledMask> chunksToProcess, ref BufferTypeHandle<LinkedEntityGroup> linkedGroupTypeHandle, bool queryHasEnableableComponents)
		{
			UnsafeList<EntityComponentStore.ChunkAndEnabledMask> unsafeList = new UnsafeList<EntityComponentStore.ChunkAndEnabledMask>(chunksToProcess.Length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			unsafeList.CopyFrom(in chunksToProcess);
			unsafeList.Sort<EntityComponentStore.ChunkAndEnabledMask>();
			UnsafeList<ChunkIndex> unsafeList2 = new UnsafeList<ChunkIndex>(unsafeList.Length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			int i = 0;
			int length = unsafeList.Length;
			while (i < length)
			{
				unsafeList2.AddNoResize(unsafeList[i].Chunk);
				i++;
			}
			fixed (EntityComponentStore* ptr = &this)
			{
				EntityComponentStore* ptr2 = ptr;
				int j = 0;
				int length2 = chunksToProcess.Length;
				while (j < length2)
				{
					ArchetypeChunk archetypeChunk = new ArchetypeChunk
					{
						m_Chunk = chunksToProcess[j].Chunk,
						m_EntityComponentStore = ptr2
					};
					if (archetypeChunk.Has<LinkedEntityGroup>(ref linkedGroupTypeHandle))
					{
						BufferAccessor<LinkedEntityGroup> bufferAccessor = archetypeChunk.GetBufferAccessor<LinkedEntityGroup>(ref linkedGroupTypeHandle);
						int k = 0;
						int length3 = bufferAccessor.Length;
						while (k < length3)
						{
							DynamicBuffer<LinkedEntityGroup> dynamicBuffer = bufferAccessor[k];
							Entity* unsafeReadOnlyPtr = (Entity*)dynamicBuffer.GetUnsafeReadOnlyPtr();
							int l = 0;
							int length4 = dynamicBuffer.Length;
							while (l < length4)
							{
								Entity entity = unsafeReadOnlyPtr[l];
								if (this.Exists(entity))
								{
									EntityInChunk entityInChunk = this.GetEntityInChunk(entity);
									int num = unsafeList2.BinarySearch(entityInChunk.Chunk);
									if (Hint.Likely(num >= 0))
									{
										if (queryHasEnableableComponents && unsafeList[num].UseEnabledMask != 0)
										{
											EntityComponentStore.ChunkAndEnabledMask chunkAndEnabledMask = unsafeList[num];
											if (Hint.Unlikely((EnabledBitUtility.ShiftRight(in chunkAndEnabledMask.EnabledMask, entityInChunk.IndexInChunk).ULong0 & 1UL) == 0UL))
											{
												chunkAndEnabledMask = chunksToProcess[j];
												Entity* buffer = (Entity*)chunkAndEnabledMask.Chunk.Buffer;
											}
										}
									}
									else
									{
										EntityComponentStore.ChunkAndEnabledMask chunkAndEnabledMask = chunksToProcess[j];
										Entity* buffer2 = (Entity*)chunkAndEnabledMask.Chunk.Buffer;
									}
								}
								l++;
							}
							k++;
						}
					}
					j++;
				}
			}
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x00028974 File Offset: 0x00026B74
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public void ThrowDestroyEntityError(Entity errorEntity, Entity errorReferencedEntity)
		{
			this.ThrowDestroyEntityErrorFancy(errorEntity, errorReferencedEntity);
			throw new ArgumentException("DestroyEntity(EntityQuery query) is destroying an entity which contains a LinkedEntityGroup with an entity not included in the query. If you want to destroy entities using a query all linked entities must be contained in the query.. For more detail, disable Burst compilation.");
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x00028988 File Offset: 0x00026B88
		[BurstDiscard]
		private void ThrowDestroyEntityErrorFancy(Entity errorEntity, Entity errorReferencedEntity)
		{
			string text = (this.Exists(errorEntity) ? this.GetName(errorEntity) : "(Deleted)");
			string text2 = (this.Exists(errorReferencedEntity) ? this.GetName(errorReferencedEntity) : "(Deleted)");
			throw new ArgumentException(string.Format("DestroyEntity(EntityQuery query) is destroying entity {0} '{1}' which contains a LinkedEntityGroup and the entity {2} '{3}' in that group is not included in the query. If you want to destroy entities using a query all linked entities must be contained in the query..", new object[] { errorEntity, text, errorReferencedEntity, text2 }));
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x000289F4 File Offset: 0x00026BF4
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public unsafe static void AssertArchetypeDoesNotRemoveCleanupComponents(Archetype* src, Archetype* dst)
		{
			int i = 0;
			int num = 0;
			while (i < src->TypesCount)
			{
				if (num >= dst->TypesCount)
				{
					break;
				}
				TypeIndex typeIndex = src->Types[i].TypeIndex;
				TypeIndex typeIndex2 = dst->Types[num].TypeIndex;
				if (typeIndex == typeIndex2)
				{
					i++;
					num++;
				}
				else if (typeIndex2 > typeIndex)
				{
					if (src->Types[i].IsCleanupComponent)
					{
						throw new ArgumentException(string.Format("Cleanup components may not be removed via SetArchetype: {0}", src->Types[i]));
					}
					i++;
				}
				else
				{
					num++;
				}
			}
			while (i < src->TypesCount)
			{
				if (src->Types[i].IsCleanupComponent)
				{
					throw new ArgumentException(string.Format("Cleanup components may not be removed via SetArchetype: {0}", src->Types[i]));
				}
				i++;
			}
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x00028B04 File Offset: 0x00026D04
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public unsafe void CheckCanAddChunkComponent(NativeArray<ArchetypeChunk> chunkArray, ComponentType componentType, ref bool result)
		{
			ArchetypeChunk* unsafeReadOnlyPtr = (ArchetypeChunk*)chunkArray.GetUnsafeReadOnlyPtr<ArchetypeChunk>();
			for (int i = 0; i < chunkArray.Length; i++)
			{
				ArchetypeChunk archetypeChunk = unsafeReadOnlyPtr[i];
				if (ChunkDataUtility.GetIndexInTypeArray(unsafeReadOnlyPtr[i].Archetype.Archetype, componentType.TypeIndex) != -1)
				{
					result = false;
					return;
				}
			}
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x00028B64 File Offset: 0x00026D64
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public unsafe void AssertCanInstantiateEntities(Entity srcEntity, Entity* outputEntities, int instanceCount)
		{
			bool flag;
			if (this.HasComponent(srcEntity, this.m_LinkedGroupType, out flag))
			{
				BufferHeader* componentDataWithTypeRO = (BufferHeader*)this.GetComponentDataWithTypeRO(srcEntity, this.m_LinkedGroupType);
				Entity* elementPointer = (Entity*)BufferHeader.GetElementPointer(componentDataWithTypeRO);
				int length = componentDataWithTypeRO->Length;
				if (length == 0 || *elementPointer != srcEntity)
				{
					throw new ArgumentException("LinkedEntityGroup[0] must always be the Entity itself.");
				}
				for (int i = 0; i < length; i++)
				{
					if (!this.Exists(elementPointer[i]))
					{
						throw new ArgumentException(string.Format("The srcEntity's LinkedEntityGroup references an entity that is invalid. (Entity at index {0} on the LinkedEntityGroup.)", i));
					}
					if (this.GetArchetype(elementPointer[i])->InstantiateArchetype == null)
					{
						throw new ArgumentException(string.Format("The srcEntity's LinkedEntityGroup references an entity that has already been destroyed. (Entity at index {0} on the LinkedEntityGroup. Only cleanup components are left on the entity)", i));
					}
				}
				return;
			}
			else
			{
				if (!flag)
				{
					throw new ArgumentException("srcEntity is not a valid entity");
				}
				if (this.GetArchetype(srcEntity)->InstantiateArchetype == null)
				{
					throw new ArgumentException("srcEntity is not instantiable because it has already been destroyed. (Only cleanup components are left on it)");
				}
				return;
			}
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x00028C60 File Offset: 0x00026E60
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public unsafe void AssertCanInstantiateEntities(Entity* srcEntity, int entityCount, bool removePrefab)
		{
			for (int i = 0; i < entityCount; i++)
			{
				if (!this.Exists(srcEntity[i]))
				{
					throw new ArgumentException(string.Format("The srcEntity[{0}] references an entity that is invalid.", i) + EntityComponentStore.AppendDestroyedEntityRecordError(*srcEntity));
				}
				Archetype* archetype = this.GetArchetype(srcEntity[i]);
				if ((removePrefab ? archetype->InstantiateArchetype : archetype->CopyArchetype) == null)
				{
					throw new ArgumentException(string.Format("The srcEntity[{0}] references an entity that has already been destroyed. (Only cleanup components are left on the entity)", i));
				}
			}
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x00028CFF File Offset: 0x00026EFF
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public unsafe static void AssertValidEntityQuery(EntityQuery query, EntityComponentStore* store)
		{
			EntityComponentStore* entityComponentStore = query._GetImpl()->_Access->EntityComponentStore;
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x00028D15 File Offset: 0x00026F15
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public unsafe static void AssertValidEntityQuery(EntityComponentStore* queryStore, EntityComponentStore* store)
		{
			if (queryStore == store)
			{
				return;
			}
			if (queryStore == null)
			{
				throw new InvalidOperationException("The EntityQuery has been disposed and can no longer be used.");
			}
			throw new InvalidOperationException("EntityQuery is associated with a different world");
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x00002287 File Offset: 0x00000487
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public unsafe static void AssertValidArchetype(EntityComponentStore* queryStore, EntityArchetype archetype)
		{
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x00028D38 File Offset: 0x00026F38
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public unsafe static void AssertPaddingBitsAreZeroForArchetype(Archetype* archetype)
		{
			for (int i = 0; i < archetype->Chunks.Count; i++)
			{
				ChunkIndex chunkIndex = archetype->Chunks[i];
				for (int j = 0; j < archetype->EnableableTypesCount; j++)
				{
					int num = archetype->EnableableTypeIndexInArchetype[j];
					int num2 = archetype->TypeIndexInArchetypeToMemoryOrderIndex[num];
					v128 v = *archetype->Chunks.GetComponentEnabledMaskArrayForTypeInChunk(num2, i);
					EnabledBitUtility.countbits(EnabledBitUtility.ShiftRight(in v, chunkIndex.Count));
				}
			}
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x00028DC0 File Offset: 0x00026FC0
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public unsafe static void AssertEnabledBitsHierarchyIsCorrectForArchetype(Archetype* archetype)
		{
			for (int i = 0; i < archetype->Chunks.Count; i++)
			{
				ChunkIndex chunkIndex = archetype->Chunks[i];
				for (int j = 0; j < archetype->EnableableTypesCount; j++)
				{
					int num = archetype->EnableableTypeIndexInArchetype[j];
					int num2 = archetype->TypeIndexInArchetypeToMemoryOrderIndex[num];
					v128 v = *archetype->Chunks.GetComponentEnabledMaskArrayForTypeInChunk(num2, i);
					int chunkDisabledCountForType = archetype->Chunks.GetChunkDisabledCountForType(num2, i);
					int num3 = chunkIndex.Count - EnabledBitUtility.countbits(v);
				}
			}
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x00028E57 File Offset: 0x00027057
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		internal void AssertNotZeroSizedComponent(TypeIndex typeIndex)
		{
			if (Hint.Unlikely(typeIndex.IsZeroSized))
			{
				throw new ArgumentException(string.Format("This operation can not be called with a zero sized component ({0}).", typeIndex));
			}
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x00028E80 File Offset: 0x00027080
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		internal static void AssertComponentSizeMatches(TypeIndex typeIndex, int size)
		{
			int sizeInChunk = TypeManager.GetTypeInfo(typeIndex).SizeInChunk;
			if (Hint.Unlikely(sizeInChunk != size))
			{
				throw new ArgumentException(string.Format("Provided data size={0} for type {1} does not match expected SizeInChunk={2}. (This should never happen; please use \"Help -> Report A Bug\" to inform the DOTS team)", size, typeIndex, sizeInChunk));
			}
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x00028EC9 File Offset: 0x000270C9
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		internal static void AssertComponentEnableable(TypeIndex typeIndex)
		{
			if (Hint.Unlikely(!typeIndex.IsEnableable))
			{
				throw new ArgumentException(string.Format("Component Enabled Bits APIs (SetComponentEnabled, IsComponentEnabled, etc) can not be called with a component type ({0}) that does not implement IEnableableComponent", typeIndex));
			}
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x00028EF2 File Offset: 0x000270F2
		internal static string AppendDestroyedEntityRecordError(Entity e)
		{
			return null;
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x00028EF2 File Offset: 0x000270F2
		internal static string AppendRemovedComponentRecordError(Entity e, ComponentType type)
		{
			return null;
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x00028F68 File Offset: 0x00027168
		[CompilerGenerated]
		internal unsafe static bool <CreateArchetype>g__MemoryOrderCompare|166_0(int lhs, int rhs, ref EntityComponentStore.<>c__DisplayClass166_0 A_2)
		{
			if (A_2.typeFlags[lhs] == A_2.typeFlags[rhs])
			{
				return A_2.memoryOrderings[lhs] < A_2.memoryOrderings[rhs];
			}
			return A_2.typeFlags[lhs] < A_2.typeFlags[rhs];
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x00028FC4 File Offset: 0x000271C4
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void EntityBatchFromEntityChunkDataShared$BurstManaged(EntityComponentStore* ecs, in EntityInChunk* chunkData, int chunkCount, EntityBatchInChunk* entityBatchList, int* currentbatchIndex, int nSharedComponentsToAdd, int* foundError)
		{
			*foundError = 0;
			int i = 0;
			EntityBatchInChunk entityBatchInChunk = new EntityBatchInChunk
			{
				Chunk = (chunkData + (IntPtr)i * (IntPtr)sizeof(EntityInChunk)).Chunk,
				StartIndex = (chunkData + (IntPtr)i * (IntPtr)sizeof(EntityInChunk)).IndexInChunk,
				Count = 1
			};
			i++;
			while (i < chunkCount)
			{
				if ((chunkData + (IntPtr)i * (IntPtr)sizeof(EntityInChunk)).Equals(*(chunkData + (IntPtr)(i - 1) * (IntPtr)sizeof(EntityInChunk))))
				{
					i++;
				}
				else
				{
					ChunkIndex chunk = (chunkData + (IntPtr)i * (IntPtr)sizeof(EntityInChunk)).Chunk;
					int indexInChunk = (chunkData + (IntPtr)i * (IntPtr)sizeof(EntityInChunk)).IndexInChunk;
					bool flag = chunk != entityBatchInChunk.Chunk;
					bool flag2 = indexInChunk != entityBatchInChunk.StartIndex + entityBatchInChunk.Count;
					if ((flag || flag2) && entityBatchInChunk.Chunk != ChunkIndex.Null)
					{
						Archetype* archetype = ecs->GetArchetype(entityBatchInChunk.Chunk);
						if (nSharedComponentsToAdd + archetype->NumSharedComponents > 16)
						{
							*foundError = 1;
							return;
						}
						entityBatchList[*currentbatchIndex] = entityBatchInChunk;
						(*currentbatchIndex)++;
						if (*currentbatchIndex > chunkCount)
						{
							*foundError = 1;
							return;
						}
						entityBatchInChunk = new EntityBatchInChunk
						{
							Chunk = chunk,
							StartIndex = indexInChunk,
							Count = 1
						};
					}
					else
					{
						entityBatchInChunk = new EntityBatchInChunk
						{
							Chunk = entityBatchInChunk.Chunk,
							StartIndex = entityBatchInChunk.StartIndex,
							Count = entityBatchInChunk.Count + 1
						};
					}
					i++;
				}
			}
			if (entityBatchInChunk.Chunk == ChunkIndex.Null)
			{
				return;
			}
			if (nSharedComponentsToAdd + ecs->GetArchetype(entityBatchInChunk.Chunk)->NumSharedComponents > 16)
			{
				*foundError = 1;
				return;
			}
			entityBatchList[*currentbatchIndex] = entityBatchInChunk;
			(*currentbatchIndex)++;
			if (*currentbatchIndex > chunkCount)
			{
				*foundError = 1;
			}
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x000291AE File Offset: 0x000273AE
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void SortEntityInChunk$BurstManaged(EntityInChunk* entityInChunks, int count)
		{
			NativeSortExtension.Sort<EntityInChunk>(entityInChunks, count);
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x000291B8 File Offset: 0x000273B8
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void GatherEntityInChunkForEntities$BurstManaged(Entity* Entities, EntityInChunk* EntityChunkData, int numEntities)
		{
			for (int i = 0; i < numEntities; i++)
			{
				Entity entity = Entities[i];
				EntityChunkData[i] = EntityComponentStore.s_entityStore.Data.GetEntityInChunk(entity);
			}
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x00029204 File Offset: 0x00027404
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void GetCreatedAndDestroyedEntities$BurstManaged(EntityComponentStore* store, ref NativeList<int> state, ref NativeList<Entity> createdEntities, ref NativeList<Entity> destroyedEntities, ref JobHandle jobHandle, bool async)
		{
			NativeList<int> nativeList = new NativeList<int>(async ? Allocator.TempJob : Allocator.Temp);
			UnsafePtrList<Archetype> archetypes = store->m_Archetypes;
			int i = 0;
			int length = archetypes.Length;
			while (i < length)
			{
				Archetype* ptr = *(IntPtr*)(store->m_Archetypes.Ptr + (IntPtr)i * (IntPtr)sizeof(Archetype*) / (IntPtr)sizeof(Archetype*));
				if (!ptr->HasChunkHeader)
				{
					ArchetypeChunkData chunks = ptr->Chunks;
					int j = 0;
					int count = chunks.Count;
					while (j < count)
					{
						ChunkIndex chunkIndex = chunks[j];
						Entity* buffer = (Entity*)chunkIndex.Buffer;
						nativeList.AddRange((void*)buffer, chunkIndex.Count * 2);
						j++;
					}
				}
				i++;
			}
			EntityComponentStore.GetCreatedAndDestroyedEntitiesJob getCreatedAndDestroyedEntitiesJob = new EntityComponentStore.GetCreatedAndDestroyedEntitiesJob
			{
				OldState = state,
				NewState = nativeList,
				CreatedEntities = createdEntities,
				DestroyedEntities = destroyedEntities
			};
			if (!async)
			{
				getCreatedAndDestroyedEntitiesJob.Execute();
				return;
			}
			jobHandle = getCreatedAndDestroyedEntitiesJob.Schedule(default(JobHandle));
			jobHandle = nativeList.Dispose(jobHandle);
			jobHandle.Complete();
		}

		// Token: 0x04000397 RID: 919
		private const int kUnmanagedSharedComponentIndexFlag = -2147483648;

		// Token: 0x04000398 RID: 920
		[NativeDisableUnsafePtrRestriction]
		private unsafe int* m_ComponentTypeOrderVersion;

		// Token: 0x04000399 RID: 921
		private BlockAllocator m_ArchetypeChunkAllocator;

		// Token: 0x0400039A RID: 922
		internal UnsafePtrList<Archetype> m_Archetypes;

		// Token: 0x0400039B RID: 923
		private ArchetypeListMap m_TypeLookup;

		// Token: 0x0400039C RID: 924
		internal int m_ManagedComponentIndex;

		// Token: 0x0400039D RID: 925
		internal int m_ManagedComponentIndexCapacity;

		// Token: 0x0400039E RID: 926
		internal UnsafeAppendBuffer m_ManagedComponentFreeIndex;

		// Token: 0x0400039F RID: 927
		internal ManagedDeferredCommands ManagedChangesTracker;

		// Token: 0x040003A0 RID: 928
		internal int m_SharedComponentVersion;

		// Token: 0x040003A1 RID: 929
		internal int m_SharedComponentGlobalVersion;

		// Token: 0x040003A2 RID: 930
		private int m_UnmanagedSharedComponentCount;

		// Token: 0x040003A3 RID: 931
		internal UnsafeList<ComponentTypeList> m_UnmanagedSharedComponentsByType;

		// Token: 0x040003A4 RID: 932
		private UnsafeList<TypeIndex> m_UnmanagedSharedComponentTypes;

		// Token: 0x040003A5 RID: 933
		private UnsafeList<UnsafeList<EntityComponentStore.SharedComponentInfo>> m_UnmanagedSharedComponentInfo;

		// Token: 0x040003A6 RID: 934
		private UnsafeParallelMultiHashMap<ulong, int> m_HashLookup;

		// Token: 0x040003A7 RID: 935
		internal EntityComponentStore.ChunkListChanges m_ChunkListChangesTracker;

		// Token: 0x040003A8 RID: 936
		private ulong m_WorldSequenceNumber;

		// Token: 0x040003A9 RID: 937
		private ulong m_NextChunkSequenceNumber;

		// Token: 0x040003AA RID: 938
		private uint m_GlobalSystemVersion;

		// Token: 0x040003AB RID: 939
		private int m_IntentionallyInconsistent;

		// Token: 0x040003AC RID: 940
		private uint m_ArchetypeTrackingVersion;

		// Token: 0x040003AD RID: 941
		private TypeIndex m_LinkedGroupType;

		// Token: 0x040003AE RID: 942
		private TypeIndex m_ChunkHeaderType;

		// Token: 0x040003AF RID: 943
		private TypeIndex m_PrefabType;

		// Token: 0x040003B0 RID: 944
		private TypeIndex m_OmitLinkedEntityGroupFromPrefabInstanceType;

		// Token: 0x040003B1 RID: 945
		private TypeIndex m_CleanupEntityType;

		// Token: 0x040003B2 RID: 946
		private TypeIndex m_DisabledType;

		// Token: 0x040003B3 RID: 947
		private TypeIndex m_EntityType;

		// Token: 0x040003B4 RID: 948
		private TypeIndex m_SystemInstanceType;

		// Token: 0x040003B5 RID: 949
		private ComponentType m_ChunkHeaderComponentType;

		// Token: 0x040003B6 RID: 950
		private ComponentType m_EntityComponentType;

		// Token: 0x040003B7 RID: 951
		private ComponentType m_SimulateComponentType;

		// Token: 0x040003B8 RID: 952
		private unsafe TypeManager.TypeInfo* m_TypeInfos;

		// Token: 0x040003B9 RID: 953
		private unsafe TypeManager.EntityOffsetInfo* m_EntityOffsetInfos;

		// Token: 0x040003BA RID: 954
		internal int m_DebugOnlyManagedAccess;

		// Token: 0x040003BB RID: 955
		internal byte memoryInitPattern;

		// Token: 0x040003BC RID: 956
		internal byte useMemoryInitPattern;

		// Token: 0x040003BD RID: 957
		private const int kMaximumEmptyChunksInPool = 16;

		// Token: 0x040003BE RID: 958
		private const int kDefaultCapacity = 1024;

		// Token: 0x040003BF RID: 959
		internal const int kMaxSharedComponentCount = 16;

		// Token: 0x040003C0 RID: 960
		private static readonly SharedStatic<ulong> s_TotalChunkAddressSpaceInBytes = SharedStatic<ulong>.GetOrCreateUnsafe(0U, 6266345168485942189L, 0L);

		// Token: 0x040003C1 RID: 961
		private static readonly ulong DefaultChunkAddressSpaceInBytes = 1073741824UL;

		// Token: 0x040003C2 RID: 962
		public EntityNameStoreAccess m_NameStoreAccess;

		// Token: 0x040003C3 RID: 963
		internal const long k_MaximumEntitiesPerWorld = 134217728L;

		// Token: 0x040003C4 RID: 964
		internal const int kUnmanagedSharedElementIndexMask = 65535;

		// Token: 0x040003C5 RID: 965
		internal const int kUnmanagedSharedTypeIndexBitOffset = 16;

		// Token: 0x040003C6 RID: 966
		internal static readonly SharedStatic<EntityComponentStore.ChunkStore> s_chunkStore = SharedStatic<EntityComponentStore.ChunkStore>.GetOrCreateUnsafe(0U, -2093332447932307698L, 0L);

		// Token: 0x040003C7 RID: 967
		private const string k_JournalingDisabledMsg = "Entities Journaling may be able to help determine more information. Please enable Entities Journaling for a more helpful error message.";

		// Token: 0x040003C8 RID: 968
		internal static readonly SharedStatic<EntityComponentStore.EntityStore> s_entityStore = SharedStatic<EntityComponentStore.EntityStore>.GetOrCreateUnsafe(0U, 1063811976437696451L, 0L);

		// Token: 0x02000141 RID: 321
		private struct SharedComponentInfo
		{
			// Token: 0x040003C9 RID: 969
			public int RefCount;

			// Token: 0x040003CA RID: 970
			public int ComponentType;

			// Token: 0x040003CB RID: 971
			public int Version;

			// Token: 0x040003CC RID: 972
			public int HashCode;
		}

		// Token: 0x02000142 RID: 322
		private struct AddressSpaceTagType
		{
		}

		// Token: 0x02000143 RID: 323
		[BurstCompile]
		internal struct PerChunkArray
		{
			// Token: 0x06000771 RID: 1905 RVA: 0x00029328 File Offset: 0x00027528
			[BurstDiscard]
			internal unsafe static void Initialize()
			{
				if (EntityComponentStore.PerChunkArray.StaticIdentifier.Ref.Data.m_PerChunkData == null)
				{
					int num = sizeof(EntityComponentStore.PerChunkArray.PerChunkData) * EntityComponentStore.ChunkStore.kMaximumChunkCount;
					EntityComponentStore.PerChunkArray.PerChunkData* ptr = (EntityComponentStore.PerChunkArray.PerChunkData*)Memory.Unmanaged.Allocate((long)num, 64, Allocator.Persistent);
					UnsafeUtility.MemClear((void*)ptr, (long)num);
					EntityComponentStore.PerChunkArray.StaticIdentifier.Ref.Data.m_PerChunkData = ptr;
					AppDomain.CurrentDomain.DomainUnload += delegate(object _, EventArgs _)
					{
						EntityComponentStore.PerChunkArray.<Initialize>g__Shutdown|2_0();
					};
					AppDomain.CurrentDomain.ProcessExit += delegate(object _, EventArgs _)
					{
						EntityComponentStore.PerChunkArray.<Initialize>g__Shutdown|2_0();
					};
				}
			}

			// Token: 0x170000A9 RID: 169
			// (get) Token: 0x06000772 RID: 1906 RVA: 0x000293D2 File Offset: 0x000275D2
			public unsafe static EntityComponentStore.PerChunkArray.PerChunkData* ChunkData
			{
				get
				{
					return EntityComponentStore.PerChunkArray.StaticIdentifier.Ref.Data.m_PerChunkData;
				}
			}

			// Token: 0x06000773 RID: 1907 RVA: 0x000293E3 File Offset: 0x000275E3
			[CompilerGenerated]
			internal static void <Initialize>g__Shutdown|2_0()
			{
				Memory.Unmanaged.Free<EntityComponentStore.PerChunkArray.PerChunkData>(EntityComponentStore.PerChunkArray.StaticIdentifier.Ref.Data.m_PerChunkData, Allocator.Persistent);
				EntityComponentStore.PerChunkArray.StaticIdentifier.Ref.Data.m_PerChunkData = null;
			}

			// Token: 0x040003CD RID: 973
			private unsafe EntityComponentStore.PerChunkArray.PerChunkData* m_PerChunkData;

			// Token: 0x02000144 RID: 324
			public struct PerChunkData
			{
				// Token: 0x040003CE RID: 974
				public unsafe Archetype* Archetype;

				// Token: 0x040003CF RID: 975
				public int EntityCount;

				// Token: 0x040003D0 RID: 976
				public int ListIndex;
			}

			// Token: 0x02000145 RID: 325
			private sealed class StaticIdentifier
			{
				// Token: 0x040003D1 RID: 977
				internal static readonly SharedStatic<EntityComponentStore.PerChunkArray> Ref = SharedStatic<EntityComponentStore.PerChunkArray>.GetOrCreateUnsafe(0U, -2301534937205546864L, 0L);
			}
		}

		// Token: 0x02000147 RID: 327
		private struct Ulong16
		{
			// Token: 0x040003D5 RID: 981
			private ulong p00;

			// Token: 0x040003D6 RID: 982
			private ulong p01;

			// Token: 0x040003D7 RID: 983
			private ulong p02;

			// Token: 0x040003D8 RID: 984
			private ulong p03;

			// Token: 0x040003D9 RID: 985
			private ulong p04;

			// Token: 0x040003DA RID: 986
			private ulong p05;

			// Token: 0x040003DB RID: 987
			private ulong p06;

			// Token: 0x040003DC RID: 988
			private ulong p07;

			// Token: 0x040003DD RID: 989
			private ulong p08;

			// Token: 0x040003DE RID: 990
			private ulong p09;

			// Token: 0x040003DF RID: 991
			private ulong p10;

			// Token: 0x040003E0 RID: 992
			private ulong p11;

			// Token: 0x040003E1 RID: 993
			private ulong p12;

			// Token: 0x040003E2 RID: 994
			private ulong p13;

			// Token: 0x040003E3 RID: 995
			private ulong p14;

			// Token: 0x040003E4 RID: 996
			private ulong p15;
		}

		// Token: 0x02000148 RID: 328
		private struct Ulong256
		{
			// Token: 0x170000AA RID: 170
			// (get) Token: 0x0600077A RID: 1914 RVA: 0x00029442 File Offset: 0x00027642
			// (set) Token: 0x0600077B RID: 1915 RVA: 0x00002287 File Offset: 0x00000487
			public int Length
			{
				get
				{
					return 256;
				}
				set
				{
				}
			}

			// Token: 0x0600077C RID: 1916 RVA: 0x0002944C File Offset: 0x0002764C
			public unsafe ref long ElementAt(int index)
			{
				fixed (EntityComponentStore.Ulong16* ptr = &this.p00)
				{
					return UnsafeUtility.AsRef<long>((void*)((byte*)ptr + (IntPtr)index * 8));
				}
			}

			// Token: 0x040003E5 RID: 997
			private EntityComponentStore.Ulong16 p00;

			// Token: 0x040003E6 RID: 998
			private EntityComponentStore.Ulong16 p01;

			// Token: 0x040003E7 RID: 999
			private EntityComponentStore.Ulong16 p02;

			// Token: 0x040003E8 RID: 1000
			private EntityComponentStore.Ulong16 p03;

			// Token: 0x040003E9 RID: 1001
			private EntityComponentStore.Ulong16 p04;

			// Token: 0x040003EA RID: 1002
			private EntityComponentStore.Ulong16 p05;

			// Token: 0x040003EB RID: 1003
			private EntityComponentStore.Ulong16 p06;

			// Token: 0x040003EC RID: 1004
			private EntityComponentStore.Ulong16 p07;

			// Token: 0x040003ED RID: 1005
			private EntityComponentStore.Ulong16 p08;

			// Token: 0x040003EE RID: 1006
			private EntityComponentStore.Ulong16 p09;

			// Token: 0x040003EF RID: 1007
			private EntityComponentStore.Ulong16 p10;

			// Token: 0x040003F0 RID: 1008
			private EntityComponentStore.Ulong16 p11;

			// Token: 0x040003F1 RID: 1009
			private EntityComponentStore.Ulong16 p12;

			// Token: 0x040003F2 RID: 1010
			private EntityComponentStore.Ulong16 p13;

			// Token: 0x040003F3 RID: 1011
			private EntityComponentStore.Ulong16 p14;

			// Token: 0x040003F4 RID: 1012
			private EntityComponentStore.Ulong16 p15;
		}

		// Token: 0x02000149 RID: 329
		private struct Ulong4096
		{
			// Token: 0x040003F5 RID: 1013
			private EntityComponentStore.Ulong256 p00;

			// Token: 0x040003F6 RID: 1014
			private EntityComponentStore.Ulong256 p01;

			// Token: 0x040003F7 RID: 1015
			private EntityComponentStore.Ulong256 p02;

			// Token: 0x040003F8 RID: 1016
			private EntityComponentStore.Ulong256 p03;

			// Token: 0x040003F9 RID: 1017
			private EntityComponentStore.Ulong256 p04;

			// Token: 0x040003FA RID: 1018
			private EntityComponentStore.Ulong256 p05;

			// Token: 0x040003FB RID: 1019
			private EntityComponentStore.Ulong256 p06;

			// Token: 0x040003FC RID: 1020
			private EntityComponentStore.Ulong256 p07;

			// Token: 0x040003FD RID: 1021
			private EntityComponentStore.Ulong256 p08;

			// Token: 0x040003FE RID: 1022
			private EntityComponentStore.Ulong256 p09;

			// Token: 0x040003FF RID: 1023
			private EntityComponentStore.Ulong256 p10;

			// Token: 0x04000400 RID: 1024
			private EntityComponentStore.Ulong256 p11;

			// Token: 0x04000401 RID: 1025
			private EntityComponentStore.Ulong256 p12;

			// Token: 0x04000402 RID: 1026
			private EntityComponentStore.Ulong256 p13;

			// Token: 0x04000403 RID: 1027
			private EntityComponentStore.Ulong256 p14;

			// Token: 0x04000404 RID: 1028
			private EntityComponentStore.Ulong256 p15;
		}

		// Token: 0x0200014A RID: 330
		private struct Ulong16384 : IIndexable<long>
		{
			// Token: 0x170000AB RID: 171
			// (get) Token: 0x0600077D RID: 1917 RVA: 0x0002946C File Offset: 0x0002766C
			// (set) Token: 0x0600077E RID: 1918 RVA: 0x00002287 File Offset: 0x00000487
			public int Length
			{
				get
				{
					return 16384;
				}
				set
				{
				}
			}

			// Token: 0x0600077F RID: 1919 RVA: 0x00029474 File Offset: 0x00027674
			public unsafe ref long ElementAt(int index)
			{
				fixed (EntityComponentStore.Ulong4096* ptr = &this.p00)
				{
					return UnsafeUtility.AsRef<long>((void*)((byte*)ptr + (IntPtr)index * 8));
				}
			}

			// Token: 0x04000405 RID: 1029
			private EntityComponentStore.Ulong4096 p00;

			// Token: 0x04000406 RID: 1030
			private EntityComponentStore.Ulong4096 p01;

			// Token: 0x04000407 RID: 1031
			private EntityComponentStore.Ulong4096 p02;

			// Token: 0x04000408 RID: 1032
			private EntityComponentStore.Ulong4096 p03;
		}

		// Token: 0x0200014B RID: 331
		internal struct ChunkStore : IDisposable
		{
			// Token: 0x06000780 RID: 1920 RVA: 0x00029494 File Offset: 0x00027694
			public unsafe static void Initialize()
			{
				*EntityComponentStore.s_chunkStore.Data.m_chunkInUse.ElementAt(0) = -1L;
				*EntityComponentStore.s_chunkStore.Data.m_megachunkIsFull.ElementAt(0) = 1L;
			}

			// Token: 0x06000781 RID: 1921 RVA: 0x000294C6 File Offset: 0x000276C6
			private int AllocationFailed(int offset, int count)
			{
				if (ConcurrentMask.TryFree<EntityComponentStore.Ulong16384>(ref this.m_chunkInUse, offset, count) == -1)
				{
					return EntityComponentStore.ChunkStore.kErrorChunkAlreadyFreed;
				}
				return EntityComponentStore.ChunkStore.kErrorAllocationFailed;
			}

			// Token: 0x06000782 RID: 1922 RVA: 0x000294E4 File Offset: 0x000276E4
			internal unsafe Chunk* GetChunkPointer(int chunkIndex)
			{
				int num = chunkIndex >> EntityComponentStore.ChunkStore.log2ChunksPerMegachunk;
				int num2 = chunkIndex & (EntityComponentStore.ChunkStore.chunksPerMegachunk - 1);
				byte* ptr = *this.m_megachunk.ElementAt(num);
				return (Chunk*)(ptr + (num2 << EntityComponentStore.ChunkStore.Log2ChunkSizeInBytesRoundedUpToPow2));
			}

			// Token: 0x06000783 RID: 1923 RVA: 0x00029524 File Offset: 0x00027724
			public unsafe int AllocateContiguousChunks(out ChunkIndex value, int requestedCount, out int actualCount)
			{
				int num = 0;
				while (num < this.m_megachunkIsFull.Length && *this.m_megachunkIsFull.ElementAt(num) == -1L)
				{
					num++;
				}
				int num2 = num << 6;
				actualCount = math.min(EntityComponentStore.ChunkStore.chunksPerMegachunk, requestedCount);
				value = ChunkIndex.Null;
				while (actualCount > 0)
				{
					int i = 0;
					while (i < EntityComponentStore.ChunkStore.megachunksInUniverse)
					{
						int num3 = (num2 + i) & (EntityComponentStore.ChunkStore.megachunksInUniverse - 1);
						long num4 = *this.m_chunkInUse.ElementAt(num3);
						long num5;
						int num6;
						long num8;
						for (;;)
						{
							num5 = num4;
							int num7;
							if (num5 == -1L || !ConcurrentMask.foundAtLeastThisManyConsecutiveZeroes(num5, actualCount, out num6, out num7))
							{
								break;
							}
							long num9;
							num8 = (num9 = num5 | ConcurrentMask.MakeMask(num6, actualCount));
							if (num5 == 0L)
							{
								num9 = -1L;
							}
							num4 = Interlocked.CompareExchange(this.m_chunkInUse.ElementAt(num3), num9, num5);
							if (num4 == num5)
							{
								goto Block_5;
							}
						}
						i++;
						continue;
						Block_5:
						int num10 = (num3 << EntityComponentStore.ChunkStore.log2ChunksPerMegachunk) + num6;
						if (num5 == 0L)
						{
							long num11 = Memory.Unmanaged.Allocate((long)EntityComponentStore.ChunkStore.MegachunkSizeInBytes, 64, Allocator.Persistent);
							if (num11 == 0L)
							{
								return this.AllocationFailed(num10, actualCount);
							}
							Interlocked.Exchange(this.m_megachunk.ElementAt(num3), num11);
							Interlocked.Exchange(this.m_chunkInUse.ElementAt(num3), num8);
						}
						if (num8 == -1L)
						{
							ConcurrentMask.AtomicOr(this.m_megachunkIsFull.ElementAt(num3 >> 6), 1L << num3);
						}
						value = new ChunkIndex(num10);
						return EntityComponentStore.ChunkStore.kErrorNone;
					}
					actualCount >>= 1;
				}
				return EntityComponentStore.ChunkStore.kErrorNoChunksAvailable;
			}

			// Token: 0x06000784 RID: 1924 RVA: 0x000296A9 File Offset: 0x000278A9
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[Conditional("UNITY_DOTS_DEBUG")]
			private void ThrowChunkAlreadyMarkedAsFree(ChunkIndex chunk)
			{
				throw new ArgumentException(string.Format("Chunk index {0} (address {1:x8}) already marked as free", chunk, chunk.GetPtr()));
			}

			// Token: 0x06000785 RID: 1925 RVA: 0x000296D4 File Offset: 0x000278D4
			public unsafe int FreeContiguousChunks(ChunkIndex firstChunk, int count)
			{
				int num = firstChunk >> EntityComponentStore.ChunkStore.log2ChunksPerMegachunk;
				long num2 = ConcurrentMask.MakeMask(firstChunk & (EntityComponentStore.ChunkStore.chunksPerMegachunk - 1), count);
				long num3 = *this.m_chunkInUse.ElementAt(num);
				long num5;
				for (;;)
				{
					long num4 = num3;
					if ((num4 & num2) != num2)
					{
						break;
					}
					num5 = num4 & ~num2;
					if (num5 == 0L)
					{
						num5 = -1L;
					}
					num3 = Interlocked.CompareExchange(this.m_chunkInUse.ElementAt(num), num5, num4);
					if (num3 == num4)
					{
						goto Block_3;
					}
				}
				return EntityComponentStore.ChunkStore.kErrorChunkAlreadyMarkedFree;
				Block_3:
				if (num5 == -1L)
				{
					byte* ptr = Interlocked.Exchange(this.m_megachunk.ElementAt(num), 0L);
					Interlocked.Exchange(this.m_chunkInUse.ElementAt(num), 0L);
					Memory.Unmanaged.Free<byte>(ptr, Allocator.Persistent);
				}
				ConcurrentMask.AtomicAnd(this.m_megachunkIsFull.ElementAt(num >> 6), ~(1L << num));
				return EntityComponentStore.ChunkStore.kErrorNone;
			}

			// Token: 0x06000786 RID: 1926 RVA: 0x000297A8 File Offset: 0x000279A8
			public unsafe void Dispose()
			{
				for (int i = 0; i < EntityComponentStore.ChunkStore.megachunksInUniverse; i++)
				{
					void* ptr = *this.m_megachunk.ElementAt(i);
					if (ptr != null)
					{
						Memory.Unmanaged.Free(ptr, Allocator.Persistent);
					}
				}
				this = default(EntityComponentStore.ChunkStore);
			}

			// Token: 0x04000409 RID: 1033
			private EntityComponentStore.Ulong16384 m_megachunk;

			// Token: 0x0400040A RID: 1034
			private EntityComponentStore.Ulong16384 m_chunkInUse;

			// Token: 0x0400040B RID: 1035
			private EntityComponentStore.Ulong256 m_megachunkIsFull;

			// Token: 0x0400040C RID: 1036
			private static readonly int log2ChunksPerMegachunk = 6;

			// Token: 0x0400040D RID: 1037
			private static readonly int chunksPerMegachunk = 1 << EntityComponentStore.ChunkStore.log2ChunksPerMegachunk;

			// Token: 0x0400040E RID: 1038
			private static readonly int log2MegachunksInUniverse = 14;

			// Token: 0x0400040F RID: 1039
			private static readonly int megachunksInUniverse = 1 << EntityComponentStore.ChunkStore.log2MegachunksInUniverse;

			// Token: 0x04000410 RID: 1040
			public static readonly int kMaximumChunkCount = 1 << EntityComponentStore.ChunkStore.log2ChunksPerMegachunk + EntityComponentStore.ChunkStore.log2MegachunksInUniverse;

			// Token: 0x04000411 RID: 1041
			public static readonly int kErrorNone = 0;

			// Token: 0x04000412 RID: 1042
			public static readonly int kErrorAllocationFailed = -1;

			// Token: 0x04000413 RID: 1043
			public static readonly int kErrorChunkAlreadyFreed = -2;

			// Token: 0x04000414 RID: 1044
			public static readonly int kErrorChunkAlreadyMarkedFree = -3;

			// Token: 0x04000415 RID: 1045
			public static readonly int kErrorChunkNotFound = -4;

			// Token: 0x04000416 RID: 1046
			public static readonly int kErrorNoChunksAvailable = -5;

			// Token: 0x04000417 RID: 1047
			private static readonly int ChunkSizeInBytesRoundedUpToPow2 = math.ceilpow2(16384);

			// Token: 0x04000418 RID: 1048
			private static readonly int Log2ChunkSizeInBytesRoundedUpToPow2 = math.tzcnt(EntityComponentStore.ChunkStore.ChunkSizeInBytesRoundedUpToPow2);

			// Token: 0x04000419 RID: 1049
			private static readonly int MegachunkSizeInBytes = 1 << EntityComponentStore.ChunkStore.Log2ChunkSizeInBytesRoundedUpToPow2 + 6;
		}

		// Token: 0x0200014C RID: 332
		public struct ArchetypeChanges
		{
			// Token: 0x0400041A RID: 1050
			public int StartIndex;

			// Token: 0x0400041B RID: 1051
			public uint ArchetypeTrackingVersion;
		}

		// Token: 0x0200014D RID: 333
		internal struct ChunkListChanges
		{
			// Token: 0x06000788 RID: 1928 RVA: 0x00029890 File Offset: 0x00027A90
			public void Init()
			{
				this.ArchetypeTrackingHead = null;
			}

			// Token: 0x06000789 RID: 1929 RVA: 0x0002989A File Offset: 0x00027A9A
			public unsafe void TrackArchetype(Archetype* archetype)
			{
				if (archetype->NextChangedArchetype == null)
				{
					archetype->NextChangedArchetype = this.ArchetypeTrackingHead;
					this.ArchetypeTrackingHead = archetype;
				}
			}

			// Token: 0x0400041C RID: 1052
			public unsafe Archetype* ArchetypeTrackingHead;
		}

		// Token: 0x0200014E RID: 334
		private struct ChunkAndEnabledMask : IComparable<EntityComponentStore.ChunkAndEnabledMask>
		{
			// Token: 0x0600078A RID: 1930 RVA: 0x000298B9 File Offset: 0x00027AB9
			public int CompareTo(EntityComponentStore.ChunkAndEnabledMask other)
			{
				return this.Chunk.CompareTo(other.Chunk);
			}

			// Token: 0x0400041D RID: 1053
			public v128 EnabledMask;

			// Token: 0x0400041E RID: 1054
			public ChunkIndex Chunk;

			// Token: 0x0400041F RID: 1055
			public int ChunkEntityCount;

			// Token: 0x04000420 RID: 1056
			public byte UseEnabledMask;
		}

		// Token: 0x0200014F RID: 335
		private struct ArchetypeChunkFilter
		{
			// Token: 0x0600078B RID: 1931 RVA: 0x000298CC File Offset: 0x00027ACC
			public unsafe ArchetypeChunkFilter(Archetype* archetype, int* sharedComponentValues)
			{
				this.Archetype = archetype;
				for (int i = 0; i < archetype->NumSharedComponents; i++)
				{
					*((ref this.SharedComponentValues.FixedElementField) + (IntPtr)i * 4) = sharedComponentValues[i];
				}
			}

			// Token: 0x0600078C RID: 1932 RVA: 0x0002990C File Offset: 0x00027B0C
			public unsafe ArchetypeChunkFilter(Archetype* archetype, SharedComponentValues sharedComponentValues)
			{
				this.Archetype = archetype;
				for (int i = 0; i < archetype->NumSharedComponents; i++)
				{
					*((ref this.SharedComponentValues.FixedElementField) + (IntPtr)i * 4) = *sharedComponentValues[i];
				}
			}

			// Token: 0x04000421 RID: 1057
			public unsafe Archetype* Archetype;

			// Token: 0x04000422 RID: 1058
			[FixedBuffer(typeof(int), 16)]
			public EntityComponentStore.ArchetypeChunkFilter.<SharedComponentValues>e__FixedBuffer SharedComponentValues;

			// Token: 0x02000150 RID: 336
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 64)]
			public struct <SharedComponentValues>e__FixedBuffer
			{
				// Token: 0x04000423 RID: 1059
				public int FixedElementField;
			}
		}

		// Token: 0x02000151 RID: 337
		private struct InstantiateRemapChunk
		{
			// Token: 0x04000424 RID: 1060
			public ChunkIndex Chunk;

			// Token: 0x04000425 RID: 1061
			public int IndexInChunk;

			// Token: 0x04000426 RID: 1062
			public int AllocatedCount;

			// Token: 0x04000427 RID: 1063
			public int InstanceBeginIndex;
		}

		// Token: 0x02000152 RID: 338
		[BurstCompile]
		private struct GetCreatedAndDestroyedEntitiesJob : IJob
		{
			// Token: 0x0600078D RID: 1933 RVA: 0x0002994C File Offset: 0x00027B4C
			public void Execute()
			{
				this.CreatedEntities.Clear();
				this.DestroyedEntities.Clear();
				NativeArray<Entity> nativeArray = this.OldState.AsArray().Reinterpret<Entity>(4);
				NativeArray<Entity> nativeArray2 = this.NewState.AsArray().Reinterpret<Entity>(4);
				nativeArray2.Sort<Entity>();
				int num = 0;
				int num2 = 0;
				int length = nativeArray.Length;
				int length2 = nativeArray2.Length;
				while (num != length)
				{
					if (num2 == length2)
					{
						if (num != length)
						{
							this.DestroyedEntities.AddRange(nativeArray.GetSubArray(num, length - num));
						}
						IL_011B:
						this.OldState.CopyFrom(in this.NewState);
						return;
					}
					Entity entity = nativeArray[num];
					int num3 = entity.CompareTo(nativeArray2[num2]);
					if (num3 == 0)
					{
						num++;
						num2++;
					}
					else if (num3 < 0)
					{
						entity = nativeArray[num];
						this.DestroyedEntities.Add(in entity);
						num++;
					}
					else
					{
						entity = nativeArray2[num2];
						this.CreatedEntities.Add(in entity);
						num2++;
					}
				}
				if (num2 != length2)
				{
					this.CreatedEntities.AddRange(nativeArray2.GetSubArray(num2, length2 - num2));
					goto IL_011B;
				}
				goto IL_011B;
			}

			// Token: 0x04000428 RID: 1064
			public NativeList<int> OldState;

			// Token: 0x04000429 RID: 1065
			public NativeList<int> NewState;

			// Token: 0x0400042A RID: 1066
			public NativeList<Entity> CreatedEntities;

			// Token: 0x0400042B RID: 1067
			public NativeList<Entity> DestroyedEntities;
		}

		// Token: 0x02000153 RID: 339
		internal struct EntityStore : IDisposable
		{
			// Token: 0x0600078E RID: 1934 RVA: 0x00029A88 File Offset: 0x00027C88
			public unsafe void IntegrityCheck(int blockIndex)
			{
				EntityComponentStore.EntityStore.DataBlock* ptr = *((ref this.m_DataBlocks.FixedElementField) + (IntPtr)blockIndex * 8);
				if (ptr == null)
				{
					if (*((ref this.m_EntityCount.FixedElementField) + (IntPtr)blockIndex * 4) != 0)
					{
						Debug.Log(string.Format("block index {0} pointer {1:X16} count {2}", blockIndex, ptr, *((ref this.m_EntityCount.FixedElementField) + (IntPtr)blockIndex * 4)));
					}
					return;
				}
				ulong* ptr2 = &ptr->allocated.FixedElementField;
				int* ptr3 = &ptr->versions.FixedElementField;
				for (int i = 0; i < 8192; i++)
				{
					ulong num = ptr2[i / 64];
					int num2 = i % 64;
					int num3 = ptr3[i];
				}
			}

			// Token: 0x0600078F RID: 1935 RVA: 0x00029B38 File Offset: 0x00027D38
			public void IntegrityCheck()
			{
				for (int i = 0; i < 16384; i++)
				{
					this.IntegrityCheck(i);
				}
			}

			// Token: 0x170000AC RID: 172
			// (get) Token: 0x06000790 RID: 1936 RVA: 0x00029B5C File Offset: 0x00027D5C
			public unsafe int DebugOnlyThreadUnsafeEntityCount
			{
				get
				{
					int num = 0;
					for (int i = 0; i < 16384; i++)
					{
						num += *((ref this.m_EntityCount.FixedElementField) + (IntPtr)i * 4);
					}
					return num;
				}
			}

			// Token: 0x06000791 RID: 1937 RVA: 0x00029B90 File Offset: 0x00027D90
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[Conditional("UNITY_DOTS_DEBUG")]
			private unsafe void DebugOnlyThrowIfEntityDoesntExist(Entity entity, EntityComponentStore.EntityStore.DataBlock* block, int indexInBlock)
			{
				EntityComponentStore.EntityStore.<>c__DisplayClass13_0 CS$<>8__locals1;
				CS$<>8__locals1.block = block;
				CS$<>8__locals1.indexInBlock = indexInBlock;
				if (CS$<>8__locals1.block == null || EntityComponentStore.EntityStore.<DebugOnlyThrowIfEntityDoesntExist>g__MissingInBitmask|13_0(ref CS$<>8__locals1))
				{
					throw new ArgumentException("All entities passed to EntityManager must exist. One of the entities has already been destroyed or was never created. " + EntityComponentStore.AppendDestroyedEntityRecordError(entity));
				}
			}

			// Token: 0x06000792 RID: 1938 RVA: 0x00029BD8 File Offset: 0x00027DD8
			internal unsafe Entity GetEntityByEntityIndex(int index)
			{
				int num = index / 8192;
				int num2 = index % 8192;
				EntityComponentStore.EntityStore.DataBlock* ptr = *((ref this.m_DataBlocks.FixedElementField) + (IntPtr)num * 8);
				if (ptr == null)
				{
					return Entity.Null;
				}
				int num3 = *((ref ptr->versions.FixedElementField) + (IntPtr)num2 * 4);
				if ((num3 & 1) != 0)
				{
					return new Entity
					{
						Index = index,
						Version = num3
					};
				}
				return Entity.Null;
			}

			// Token: 0x06000793 RID: 1939 RVA: 0x00029C4C File Offset: 0x00027E4C
			internal unsafe bool Exists(Entity entity)
			{
				int num = entity.Index / 8192;
				int num2 = entity.Index % 8192;
				EntityComponentStore.EntityStore.DataBlock* ptr = *((ref this.m_DataBlocks.FixedElementField) + (IntPtr)num * 8);
				return ptr != null && (entity.Version & 1) != 0 && *((ref ptr->versions.FixedElementField) + (IntPtr)num2 * 4) == entity.Version;
			}

			// Token: 0x06000794 RID: 1940 RVA: 0x00029CB4 File Offset: 0x00027EB4
			public unsafe void SetEntityInChunk(Entity entity, EntityInChunk entityInChunk)
			{
				int num = entity.Index / 8192;
				int num2 = entity.Index % 8192;
				EntityComponentStore.EntityStore.DataBlock* ptr = *((ref this.m_DataBlocks.FixedElementField) + (IntPtr)num * 8);
				((EntityInChunk*)(&ptr->entityInChunk.FixedElementField))[num2] = entityInChunk;
			}

			// Token: 0x06000795 RID: 1941 RVA: 0x00029D0C File Offset: 0x00027F0C
			public unsafe void SetEntityVersion(Entity entity, int version)
			{
				int num = entity.Index / 8192;
				int num2 = entity.Index % 8192;
				EntityComponentStore.EntityStore.DataBlock* ptr = *((ref this.m_DataBlocks.FixedElementField) + (IntPtr)num * 8);
				*((ref ptr->versions.FixedElementField) + (IntPtr)num2 * 4) = version;
			}

			// Token: 0x06000796 RID: 1942 RVA: 0x00029D58 File Offset: 0x00027F58
			public unsafe EntityInChunk GetEntityInChunk(Entity entity)
			{
				int num = entity.Index / 8192;
				int num2 = entity.Index % 8192;
				EntityComponentStore.EntityStore.DataBlock* ptr = *((ref this.m_DataBlocks.FixedElementField) + (IntPtr)num * 8);
				if (ptr == null)
				{
					return EntityInChunk.Null;
				}
				bool flag = *((ref ptr->allocated.FixedElementField) + (IntPtr)(num2 / 64) * 8) != 0UL;
				ulong num3 = 1UL << num2 % 64;
				if (((flag ? 1UL : 0UL) & num3) == 0UL)
				{
					return EntityInChunk.Null;
				}
				return ((EntityInChunk*)(&ptr->entityInChunk.FixedElementField))[num2];
			}

			// Token: 0x06000797 RID: 1943 RVA: 0x00029DE1 File Offset: 0x00027FE1
			internal unsafe void AllocateEntities(NativeArray<Entity> entities)
			{
				this.AllocateEntities((Entity*)entities.GetUnsafePtr<Entity>(), entities.Length, ChunkIndex.Null, 0);
			}

			// Token: 0x06000798 RID: 1944 RVA: 0x00029DFC File Offset: 0x00027FFC
			internal unsafe void AllocateEntities(Entity* entities, int entityCount)
			{
				this.AllocateEntities(entities, entityCount, ChunkIndex.Null, 0);
			}

			// Token: 0x06000799 RID: 1945 RVA: 0x00029E0C File Offset: 0x0002800C
			internal unsafe void AllocateEntities(Entity* entities, int totalCount, ChunkIndex chunkIndex, int firstEntityInChunkIndex)
			{
				int num = firstEntityInChunkIndex;
				for (int i = 0; i < 16384; i++)
				{
					int num2 = Interlocked.Add((ref this.m_EntityCount.FixedElementField) + (IntPtr)i * 4, 0);
					if (num2 != -1 && num2 != 8192)
					{
						int num3 = 8192 - num2;
						int num4 = math.min(num3, totalCount);
						if (Interlocked.CompareExchange((ref this.m_EntityCount.FixedElementField) + (IntPtr)i * 4, -1, num2) == num2)
						{
							EntityComponentStore.EntityStore.DataBlock* ptr = *((ref this.m_DataBlocks.FixedElementField) + (IntPtr)i * 8);
							if (ptr == null)
							{
								ptr = (EntityComponentStore.EntityStore.DataBlock*)Memory.Unmanaged.Allocate(132096L, 8, Allocator.Persistent);
								UnsafeUtility.MemClear((void*)ptr, 132096L);
								*((ref this.m_DataBlocks.FixedElementField) + (IntPtr)i * 8) = ptr;
							}
							int j = math.min(num3, num4);
							ulong* ptr2 = &ptr->allocated.FixedElementField;
							int* ptr3 = &ptr->versions.FixedElementField;
							ulong* ptr4 = &ptr->entityInChunk.FixedElementField;
							int num5 = i * 8192;
							while (j > 0)
							{
								for (int k = 0; k < 128; k++)
								{
									if (ptr2[k] != 18446744073709551615UL)
									{
										for (int l = 0; l < 64; l++)
										{
											int num6 = k * 64 + l;
											ulong num7 = 1UL << num6 % 64;
											if ((ptr2[k] & num7) == 0UL)
											{
												ptr2[k] |= num7;
												*entities = new Entity
												{
													Index = num5 + num6,
													Version = ++ptr3[num6]
												};
												if (chunkIndex != ChunkIndex.Null)
												{
													*(EntityInChunk*)(ptr4 + (IntPtr)num6 * (IntPtr)sizeof(EntityInChunk) / 8) = new EntityInChunk
													{
														Chunk = chunkIndex,
														IndexInChunk = num
													};
												}
												else
												{
													ptr4[num6] = 0UL;
												}
												entities++;
												num++;
												j--;
												if (j == 0)
												{
													break;
												}
											}
										}
										if (j == 0)
										{
											break;
										}
									}
								}
							}
							Interlocked.CompareExchange((ref this.m_EntityCount.FixedElementField) + (IntPtr)i * 4, num2 + num4, -1);
							totalCount -= num4;
							if (totalCount == 0)
							{
								return;
							}
						}
					}
				}
				throw new InvalidOperationException("Could not find a data block for entity allocation.");
			}

			// Token: 0x0600079A RID: 1946 RVA: 0x0002A062 File Offset: 0x00028262
			internal unsafe void DeallocateEntities(NativeArray<Entity> entities)
			{
				this.DeallocateEntities((Entity*)entities.GetUnsafePtr<Entity>(), entities.Length);
			}

			// Token: 0x0600079B RID: 1947 RVA: 0x0002A078 File Offset: 0x00028278
			internal unsafe void DeallocateEntities(Entity* entities, int count)
			{
				int i = 0;
				while (i < count)
				{
					int num = i;
					int index = entities[i].Index;
					int num2 = index % 8192;
					int num3 = index / 8192;
					i++;
					while (i < count && entities[i].Index / 8192 == num3)
					{
						int num4 = entities[i].Index % 8192;
						if (num4 != num2 + 1)
						{
							break;
						}
						num2 = num4;
						i++;
					}
					int num5 = i;
					int num6 = index + (num5 - num);
					int num7 = Interlocked.Add((ref this.m_EntityCount.FixedElementField) + (IntPtr)num3 * 4, 0);
					if (num7 != 0)
					{
						for (;;)
						{
							if (num7 != -1)
							{
								int num8 = Interlocked.CompareExchange((ref this.m_EntityCount.FixedElementField) + (IntPtr)num3 * 4, -1, num7);
								if (num8 == num7)
								{
									break;
								}
								num7 = num8;
							}
							else
							{
								num7 = Interlocked.Add((ref this.m_EntityCount.FixedElementField) + (IntPtr)num3 * 4, 0);
							}
						}
						if (num7 == 0)
						{
							Interlocked.CompareExchange((ref this.m_EntityCount.FixedElementField) + (IntPtr)i * 4, 0, -1);
						}
						else
						{
							EntityComponentStore.EntityStore.DataBlock* ptr = *((ref this.m_DataBlocks.FixedElementField) + (IntPtr)num3 * 8);
							ulong* ptr2 = &ptr->allocated.FixedElementField;
							int* ptr3 = &ptr->versions.FixedElementField;
							int j = index;
							int num9 = num;
							while (j < num6)
							{
								int num10 = j % 8192;
								if (ptr3[num10] == entities[num9].Version)
								{
									ulong num11 = 1UL << num10 % 64;
									ptr3[num10]++;
									ptr2[num10 / 64] &= ulong.MaxValue ^ num11;
									*((ref ptr->nameByEntityIndex.FixedElementField) + (IntPtr)num10 * 4) = 0;
									num7--;
								}
								j++;
								num9++;
							}
							Interlocked.CompareExchange((ref this.m_EntityCount.FixedElementField) + (IntPtr)num3 * 4, num7, -1);
						}
					}
				}
			}

			// Token: 0x0600079C RID: 1948 RVA: 0x0002A26C File Offset: 0x0002846C
			public unsafe void Dispose()
			{
				for (int i = 0; i < 16384; i++)
				{
					void* ptr = *((ref this.m_DataBlocks.FixedElementField) + (IntPtr)i * 8);
					if (ptr != null)
					{
						Memory.Unmanaged.Free(ptr, Allocator.Persistent);
					}
				}
				this = default(EntityComponentStore.EntityStore);
			}

			// Token: 0x0600079D RID: 1949 RVA: 0x0002A2B4 File Offset: 0x000284B4
			internal EntityName GetEntityName(Entity entity)
			{
				return this.GetEntityName(entity.Index);
			}

			// Token: 0x0600079E RID: 1950 RVA: 0x0002A2C4 File Offset: 0x000284C4
			internal unsafe EntityName GetEntityName(int index)
			{
				int num = index / 8192;
				int num2 = index % 8192;
				EntityComponentStore.EntityStore.DataBlock* ptr = *((ref this.m_DataBlocks.FixedElementField) + (IntPtr)num * 8);
				if (ptr == null)
				{
					return default(EntityName);
				}
				bool flag = *((ref ptr->allocated.FixedElementField) + (IntPtr)(num2 / 64) * 8) != 0UL;
				ulong num3 = 1UL << num2 % 64;
				if (((flag ? 1UL : 0UL) & num3) == 0UL)
				{
					return default(EntityName);
				}
				int num4 = *((ref ptr->nameByEntityIndex.FixedElementField) + (IntPtr)num2 * 4);
				return new EntityName
				{
					Index = num4
				};
			}

			// Token: 0x0600079F RID: 1951 RVA: 0x0002A358 File Offset: 0x00028558
			public unsafe void SetEntityName(Entity entity, EntityName name)
			{
				int num = entity.Index / 8192;
				int num2 = entity.Index % 8192;
				EntityComponentStore.EntityStore.DataBlock* ptr = *((ref this.m_DataBlocks.FixedElementField) + (IntPtr)num * 8);
				*((ref ptr->nameByEntityIndex.FixedElementField) + (IntPtr)num2 * 4) = name.Index;
			}

			// Token: 0x060007A0 RID: 1952 RVA: 0x0002A3AC File Offset: 0x000285AC
			[CompilerGenerated]
			internal unsafe static bool <DebugOnlyThrowIfEntityDoesntExist>g__MissingInBitmask|13_0(ref EntityComponentStore.EntityStore.<>c__DisplayClass13_0 A_0)
			{
				ulong num = *((ref A_0.block->allocated.FixedElementField) + (IntPtr)(A_0.indexInBlock / 64) * 8);
				ulong num2 = 1UL << A_0.indexInBlock % 64;
				return (num & num2) == 0UL;
			}

			// Token: 0x0400042C RID: 1068
			private const int k_EntitiesInBlock = 8192;

			// Token: 0x0400042D RID: 1069
			private const int k_BlockSize = 132096;

			// Token: 0x0400042E RID: 1070
			private const int k_BlockCount = 16384;

			// Token: 0x0400042F RID: 1071
			private const int k_BlockBusy = -1;

			// Token: 0x04000430 RID: 1072
			internal const int MaximumTheoreticalAmountOfEntities = 134217728;

			// Token: 0x04000431 RID: 1073
			[FixedBuffer(typeof(ulong), 16384)]
			private EntityComponentStore.EntityStore.<m_DataBlocks>e__FixedBuffer m_DataBlocks;

			// Token: 0x04000432 RID: 1074
			[FixedBuffer(typeof(int), 16384)]
			private EntityComponentStore.EntityStore.<m_EntityCount>e__FixedBuffer m_EntityCount;

			// Token: 0x02000154 RID: 340
			internal struct BurstStaticIdentifier
			{
			}

			// Token: 0x02000155 RID: 341
			private struct DataBlock
			{
				// Token: 0x04000433 RID: 1075
				[FixedBuffer(typeof(ulong), 128)]
				public EntityComponentStore.EntityStore.DataBlock.<allocated>e__FixedBuffer allocated;

				// Token: 0x04000434 RID: 1076
				[FixedBuffer(typeof(ulong), 8192)]
				public EntityComponentStore.EntityStore.DataBlock.<entityInChunk>e__FixedBuffer entityInChunk;

				// Token: 0x04000435 RID: 1077
				[FixedBuffer(typeof(int), 8192)]
				public EntityComponentStore.EntityStore.DataBlock.<versions>e__FixedBuffer versions;

				// Token: 0x04000436 RID: 1078
				[FixedBuffer(typeof(int), 8192)]
				public EntityComponentStore.EntityStore.DataBlock.<nameByEntityIndex>e__FixedBuffer nameByEntityIndex;

				// Token: 0x02000156 RID: 342
				[CompilerGenerated]
				[UnsafeValueType]
				[StructLayout(LayoutKind.Sequential, Size = 1024)]
				public struct <allocated>e__FixedBuffer
				{
					// Token: 0x04000437 RID: 1079
					public ulong FixedElementField;
				}

				// Token: 0x02000157 RID: 343
				[CompilerGenerated]
				[UnsafeValueType]
				[StructLayout(LayoutKind.Sequential, Size = 65536)]
				public struct <entityInChunk>e__FixedBuffer
				{
					// Token: 0x04000438 RID: 1080
					public ulong FixedElementField;
				}

				// Token: 0x02000158 RID: 344
				[CompilerGenerated]
				[UnsafeValueType]
				[StructLayout(LayoutKind.Sequential, Size = 32768)]
				public struct <nameByEntityIndex>e__FixedBuffer
				{
					// Token: 0x04000439 RID: 1081
					public int FixedElementField;
				}

				// Token: 0x02000159 RID: 345
				[CompilerGenerated]
				[UnsafeValueType]
				[StructLayout(LayoutKind.Sequential, Size = 32768)]
				public struct <versions>e__FixedBuffer
				{
					// Token: 0x0400043A RID: 1082
					public int FixedElementField;
				}
			}

			// Token: 0x0200015B RID: 347
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 131072)]
			public struct <m_DataBlocks>e__FixedBuffer
			{
				// Token: 0x0400043D RID: 1085
				public ulong FixedElementField;
			}

			// Token: 0x0200015C RID: 348
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 65536)]
			public struct <m_EntityCount>e__FixedBuffer
			{
				// Token: 0x0400043E RID: 1086
				public int FixedElementField;
			}
		}

		// Token: 0x0200015E RID: 350
		// (Invoke) Token: 0x060007A2 RID: 1954
		internal unsafe delegate void EntityBatchFromEntityChunkDataShared_00000639$PostfixBurstDelegate(EntityComponentStore* ecs, in EntityInChunk* chunkData, int chunkCount, EntityBatchInChunk* entityBatchList, int* currentbatchIndex, int nSharedComponentsToAdd, int* foundError);

		// Token: 0x0200015F RID: 351
		internal static class EntityBatchFromEntityChunkDataShared_00000639$BurstDirectCall
		{
			// Token: 0x060007A5 RID: 1957 RVA: 0x0002A3ED File Offset: 0x000285ED
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (EntityComponentStore.EntityBatchFromEntityChunkDataShared_00000639$BurstDirectCall.Pointer == 0)
				{
					EntityComponentStore.EntityBatchFromEntityChunkDataShared_00000639$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(EntityComponentStore.EntityBatchFromEntityChunkDataShared_00000639$BurstDirectCall.DeferredCompilation, methodof(EntityComponentStore.EntityBatchFromEntityChunkDataShared$BurstManaged(EntityComponentStore*, ref EntityInChunk*, int, EntityBatchInChunk*, int*, int, int*)).MethodHandle, typeof(EntityComponentStore.EntityBatchFromEntityChunkDataShared_00000639$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = EntityComponentStore.EntityBatchFromEntityChunkDataShared_00000639$BurstDirectCall.Pointer;
			}

			// Token: 0x060007A6 RID: 1958 RVA: 0x0002A41C File Offset: 0x0002861C
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				EntityComponentStore.EntityBatchFromEntityChunkDataShared_00000639$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x060007A7 RID: 1959 RVA: 0x0002A434 File Offset: 0x00028634
			public unsafe static void Constructor()
			{
				EntityComponentStore.EntityBatchFromEntityChunkDataShared_00000639$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(EntityComponentStore.EntityBatchFromEntityChunkDataShared(EntityComponentStore*, ref EntityInChunk*, int, EntityBatchInChunk*, int*, int, int*)).MethodHandle);
			}

			// Token: 0x060007A8 RID: 1960 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x060007A9 RID: 1961 RVA: 0x0002A445 File Offset: 0x00028645
			// Note: this type is marked as 'beforefieldinit'.
			static EntityBatchFromEntityChunkDataShared_00000639$BurstDirectCall()
			{
				EntityComponentStore.EntityBatchFromEntityChunkDataShared_00000639$BurstDirectCall.Constructor();
			}

			// Token: 0x060007AA RID: 1962 RVA: 0x0002A44C File Offset: 0x0002864C
			public unsafe static void Invoke(EntityComponentStore* ecs, in EntityInChunk* chunkData, int chunkCount, EntityBatchInChunk* entityBatchList, int* currentbatchIndex, int nSharedComponentsToAdd, int* foundError)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = EntityComponentStore.EntityBatchFromEntityChunkDataShared_00000639$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.EntityComponentStore*,Unity.Entities.EntityInChunk*&,System.Int32,Unity.Entities.EntityBatchInChunk*,System.Int32*,System.Int32,System.Int32*), ecs, ref chunkData, chunkCount, entityBatchList, currentbatchIndex, nSharedComponentsToAdd, foundError, functionPointer);
						return;
					}
				}
				EntityComponentStore.EntityBatchFromEntityChunkDataShared$BurstManaged(ecs, in chunkData, chunkCount, entityBatchList, currentbatchIndex, nSharedComponentsToAdd, foundError);
			}

			// Token: 0x04000441 RID: 1089
			private static IntPtr Pointer;

			// Token: 0x04000442 RID: 1090
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000160 RID: 352
		// (Invoke) Token: 0x060007AC RID: 1964
		internal unsafe delegate void SortEntityInChunk_0000063A$PostfixBurstDelegate(EntityInChunk* entityInChunks, int count);

		// Token: 0x02000161 RID: 353
		internal static class SortEntityInChunk_0000063A$BurstDirectCall
		{
			// Token: 0x060007AF RID: 1967 RVA: 0x0002A48F File Offset: 0x0002868F
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (EntityComponentStore.SortEntityInChunk_0000063A$BurstDirectCall.Pointer == 0)
				{
					EntityComponentStore.SortEntityInChunk_0000063A$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(EntityComponentStore.SortEntityInChunk_0000063A$BurstDirectCall.DeferredCompilation, methodof(EntityComponentStore.SortEntityInChunk$BurstManaged(EntityInChunk*, int)).MethodHandle, typeof(EntityComponentStore.SortEntityInChunk_0000063A$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = EntityComponentStore.SortEntityInChunk_0000063A$BurstDirectCall.Pointer;
			}

			// Token: 0x060007B0 RID: 1968 RVA: 0x0002A4BC File Offset: 0x000286BC
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				EntityComponentStore.SortEntityInChunk_0000063A$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x060007B1 RID: 1969 RVA: 0x0002A4D4 File Offset: 0x000286D4
			public unsafe static void Constructor()
			{
				EntityComponentStore.SortEntityInChunk_0000063A$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(EntityComponentStore.SortEntityInChunk(EntityInChunk*, int)).MethodHandle);
			}

			// Token: 0x060007B2 RID: 1970 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x060007B3 RID: 1971 RVA: 0x0002A4E5 File Offset: 0x000286E5
			// Note: this type is marked as 'beforefieldinit'.
			static SortEntityInChunk_0000063A$BurstDirectCall()
			{
				EntityComponentStore.SortEntityInChunk_0000063A$BurstDirectCall.Constructor();
			}

			// Token: 0x060007B4 RID: 1972 RVA: 0x0002A4EC File Offset: 0x000286EC
			public unsafe static void Invoke(EntityInChunk* entityInChunks, int count)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = EntityComponentStore.SortEntityInChunk_0000063A$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.EntityInChunk*,System.Int32), entityInChunks, count, functionPointer);
						return;
					}
				}
				EntityComponentStore.SortEntityInChunk$BurstManaged(entityInChunks, count);
			}

			// Token: 0x04000443 RID: 1091
			private static IntPtr Pointer;

			// Token: 0x04000444 RID: 1092
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000162 RID: 354
		// (Invoke) Token: 0x060007B6 RID: 1974
		internal unsafe delegate void GatherEntityInChunkForEntities_0000063B$PostfixBurstDelegate(Entity* Entities, EntityInChunk* EntityChunkData, int numEntities);

		// Token: 0x02000163 RID: 355
		internal static class GatherEntityInChunkForEntities_0000063B$BurstDirectCall
		{
			// Token: 0x060007B9 RID: 1977 RVA: 0x0002A51F File Offset: 0x0002871F
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (EntityComponentStore.GatherEntityInChunkForEntities_0000063B$BurstDirectCall.Pointer == 0)
				{
					EntityComponentStore.GatherEntityInChunkForEntities_0000063B$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(EntityComponentStore.GatherEntityInChunkForEntities_0000063B$BurstDirectCall.DeferredCompilation, methodof(EntityComponentStore.GatherEntityInChunkForEntities$BurstManaged(Entity*, EntityInChunk*, int)).MethodHandle, typeof(EntityComponentStore.GatherEntityInChunkForEntities_0000063B$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = EntityComponentStore.GatherEntityInChunkForEntities_0000063B$BurstDirectCall.Pointer;
			}

			// Token: 0x060007BA RID: 1978 RVA: 0x0002A54C File Offset: 0x0002874C
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				EntityComponentStore.GatherEntityInChunkForEntities_0000063B$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x060007BB RID: 1979 RVA: 0x0002A564 File Offset: 0x00028764
			public unsafe static void Constructor()
			{
				EntityComponentStore.GatherEntityInChunkForEntities_0000063B$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(EntityComponentStore.GatherEntityInChunkForEntities(Entity*, EntityInChunk*, int)).MethodHandle);
			}

			// Token: 0x060007BC RID: 1980 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x060007BD RID: 1981 RVA: 0x0002A575 File Offset: 0x00028775
			// Note: this type is marked as 'beforefieldinit'.
			static GatherEntityInChunkForEntities_0000063B$BurstDirectCall()
			{
				EntityComponentStore.GatherEntityInChunkForEntities_0000063B$BurstDirectCall.Constructor();
			}

			// Token: 0x060007BE RID: 1982 RVA: 0x0002A57C File Offset: 0x0002877C
			public unsafe static void Invoke(Entity* Entities, EntityInChunk* EntityChunkData, int numEntities)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = EntityComponentStore.GatherEntityInChunkForEntities_0000063B$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.Entity*,Unity.Entities.EntityInChunk*,System.Int32), Entities, EntityChunkData, numEntities, functionPointer);
						return;
					}
				}
				EntityComponentStore.GatherEntityInChunkForEntities$BurstManaged(Entities, EntityChunkData, numEntities);
			}

			// Token: 0x04000445 RID: 1093
			private static IntPtr Pointer;

			// Token: 0x04000446 RID: 1094
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000164 RID: 356
		// (Invoke) Token: 0x060007C0 RID: 1984
		internal unsafe delegate void GetCreatedAndDestroyedEntities_000006AD$PostfixBurstDelegate(EntityComponentStore* store, ref NativeList<int> state, ref NativeList<Entity> createdEntities, ref NativeList<Entity> destroyedEntities, ref JobHandle jobHandle, bool async);

		// Token: 0x02000165 RID: 357
		internal static class GetCreatedAndDestroyedEntities_000006AD$BurstDirectCall
		{
			// Token: 0x060007C3 RID: 1987 RVA: 0x0002A5B1 File Offset: 0x000287B1
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (EntityComponentStore.GetCreatedAndDestroyedEntities_000006AD$BurstDirectCall.Pointer == 0)
				{
					EntityComponentStore.GetCreatedAndDestroyedEntities_000006AD$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(EntityComponentStore.GetCreatedAndDestroyedEntities_000006AD$BurstDirectCall.DeferredCompilation, methodof(EntityComponentStore.GetCreatedAndDestroyedEntities$BurstManaged(EntityComponentStore*, ref NativeList<int>, ref NativeList<Entity>, ref NativeList<Entity>, ref JobHandle, bool)).MethodHandle, typeof(EntityComponentStore.GetCreatedAndDestroyedEntities_000006AD$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = EntityComponentStore.GetCreatedAndDestroyedEntities_000006AD$BurstDirectCall.Pointer;
			}

			// Token: 0x060007C4 RID: 1988 RVA: 0x0002A5E0 File Offset: 0x000287E0
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				EntityComponentStore.GetCreatedAndDestroyedEntities_000006AD$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x060007C5 RID: 1989 RVA: 0x0002A5F8 File Offset: 0x000287F8
			public unsafe static void Constructor()
			{
				EntityComponentStore.GetCreatedAndDestroyedEntities_000006AD$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(EntityComponentStore.GetCreatedAndDestroyedEntities(EntityComponentStore*, ref NativeList<int>, ref NativeList<Entity>, ref NativeList<Entity>, ref JobHandle, bool)).MethodHandle);
			}

			// Token: 0x060007C6 RID: 1990 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x060007C7 RID: 1991 RVA: 0x0002A609 File Offset: 0x00028809
			// Note: this type is marked as 'beforefieldinit'.
			static GetCreatedAndDestroyedEntities_000006AD$BurstDirectCall()
			{
				EntityComponentStore.GetCreatedAndDestroyedEntities_000006AD$BurstDirectCall.Constructor();
			}

			// Token: 0x060007C8 RID: 1992 RVA: 0x0002A610 File Offset: 0x00028810
			public unsafe static void Invoke(EntityComponentStore* store, ref NativeList<int> state, ref NativeList<Entity> createdEntities, ref NativeList<Entity> destroyedEntities, ref JobHandle jobHandle, bool async)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = EntityComponentStore.GetCreatedAndDestroyedEntities_000006AD$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.EntityComponentStore*,Unity.Collections.NativeList`1<System.Int32>&,Unity.Collections.NativeList`1<Unity.Entities.Entity>&,Unity.Collections.NativeList`1<Unity.Entities.Entity>&,Unity.Jobs.JobHandle&,System.Boolean), store, ref state, ref createdEntities, ref destroyedEntities, ref jobHandle, async, functionPointer);
						return;
					}
				}
				EntityComponentStore.GetCreatedAndDestroyedEntities$BurstManaged(store, ref state, ref createdEntities, ref destroyedEntities, ref jobHandle, async);
			}

			// Token: 0x04000447 RID: 1095
			private static IntPtr Pointer;

			// Token: 0x04000448 RID: 1096
			private static IntPtr DeferredCompilation;
		}
	}
}
