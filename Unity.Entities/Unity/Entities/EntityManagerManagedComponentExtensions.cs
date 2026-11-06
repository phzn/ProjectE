using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x02000186 RID: 390
	public static class EntityManagerManagedComponentExtensions
	{
		// Token: 0x060009B6 RID: 2486 RVA: 0x00030E1B File Offset: 0x0002F01B
		public static Entity CreateSingleton<T>(this EntityManager manager, FixedString64Bytes name = default(FixedString64Bytes)) where T : class, IComponentData, new()
		{
			return manager.CreateSingletonEntityInternal<T>(name);
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x00030E24 File Offset: 0x0002F024
		public static Entity CreateSingleton<T>(this EntityManager manager, T componentData, FixedString64Bytes name = default(FixedString64Bytes)) where T : class, IComponentData, new()
		{
			Entity entity = manager.CreateSingletonEntityInternal<T>(name);
			manager.SetComponentData(entity, componentData);
			return entity;
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x00030E44 File Offset: 0x0002F044
		private unsafe static Entity CreateSingletonEntityInternal<T>(this EntityManager manager, FixedString64Bytes name = default(FixedString64Bytes)) where T : class, IComponentData, new()
		{
			ComponentType componentType = ComponentType.ReadWrite<T>();
			Entity entity = manager.CreateEntity(manager.CreateArchetype(&componentType, 1));
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			if ((in name) == (in fixedString32Bytes))
			{
				FixedString128Bytes fixedString128Bytes = componentType.ToFixedString();
				(ref name).CopyFromTruncated<FixedString64Bytes, FixedString128Bytes>(in fixedString128Bytes);
			}
			manager.GetCheckedEntityDataAccess()->SetName(entity, in name);
			return entity;
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x00030EA0 File Offset: 0x0002F0A0
		public unsafe static T GetComponentData<T>(this EntityManager manager, Entity entity) where T : class, IComponentData, new()
		{
			EntityDataAccess* checkedEntityDataAccess = manager.GetCheckedEntityDataAccess();
			return (ref *checkedEntityDataAccess).GetComponentData<T>(entity, checkedEntityDataAccess->ManagedComponentStore);
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x00030EC4 File Offset: 0x0002F0C4
		public unsafe static T GetComponentData<T>(this EntityManager manager, SystemHandle system) where T : class, IComponentData, new()
		{
			EntityDataAccess* checkedEntityDataAccess = manager.GetCheckedEntityDataAccess();
			return (ref *checkedEntityDataAccess).GetComponentData<T>(system.m_Entity, checkedEntityDataAccess->ManagedComponentStore);
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x00030EEC File Offset: 0x0002F0EC
		public static void SetComponentData<T>(this EntityManager manager, Entity entity, T componentData) where T : class, IComponentData, new()
		{
			ComponentType componentType = ComponentType.ReadWrite<T>();
			manager.SetComponentObject(entity, componentType, componentData);
		}

		// Token: 0x060009BC RID: 2492 RVA: 0x00030F10 File Offset: 0x0002F110
		public unsafe static void SetComponentData<T>(this EntityManager manager, SystemHandle system, T componentData) where T : class, IComponentData, new()
		{
			ref EntityDataAccess checkedEntityDataAccess = ref *manager.GetCheckedEntityDataAccess();
			ComponentType componentType = ComponentType.ReadWrite<T>();
			Entity entity = system.m_Entity;
			ComponentType componentType2 = componentType;
			object obj = componentData;
			SystemHandle systemHandle = default(SystemHandle);
			(ref checkedEntityDataAccess).SetComponentObject(entity, componentType2, obj, in systemHandle);
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x00030F48 File Offset: 0x0002F148
		public static T GetChunkComponentData<T>(this EntityManager manager, ArchetypeChunk chunk) where T : class, IComponentData, new()
		{
			Entity metaChunkEntity = chunk.m_Chunk.MetaChunkEntity;
			return manager.GetComponentData<T>(metaChunkEntity);
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x00030F6C File Offset: 0x0002F16C
		public unsafe static T GetChunkComponentData<T>(this EntityManager manager, Entity entity) where T : class, IComponentData, new()
		{
			EntityDataAccess* checkedEntityDataAccess = manager.GetCheckedEntityDataAccess();
			Entity metaChunkEntity = checkedEntityDataAccess->EntityComponentStore->GetChunk(entity).MetaChunkEntity;
			return (ref *checkedEntityDataAccess).GetComponentData<T>(metaChunkEntity, checkedEntityDataAccess->ManagedComponentStore);
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x00030FA4 File Offset: 0x0002F1A4
		public static void SetChunkComponentData<T>(this EntityManager manager, ArchetypeChunk chunk, T componentValue) where T : class, IComponentData, new()
		{
			Entity metaChunkEntity = chunk.m_Chunk.MetaChunkEntity;
			manager.SetComponentData(metaChunkEntity, componentValue);
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x00030FC8 File Offset: 0x0002F1C8
		public static void AddComponentData<T>(this EntityManager manager, Entity entity, T componentData) where T : class, IComponentData, new()
		{
			ComponentType componentType = ComponentType.ReadWrite<T>();
			manager.AddComponent(entity, componentType);
			manager.SetComponentData(entity, componentData);
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x00030FED File Offset: 0x0002F1ED
		public static void AddComponentData<T>(this EntityManager manager, SystemHandle system, T componentData) where T : class, IComponentData, new()
		{
			manager.AddComponentData(system.m_Entity, componentData);
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x00030FFC File Offset: 0x0002F1FC
		public unsafe static void MoveComponent<T>(this EntityManager manager, Entity src, Entity dst) where T : class, IComponentData, new()
		{
			EntityDataAccess* checkedEntityDataAccess = manager.GetCheckedEntityDataAccess();
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginStructuralChanges();
			ComponentType componentType = ComponentType.ReadWrite<T>();
			SystemHandle systemHandle = default(SystemHandle);
			(ref *checkedEntityDataAccess).MoveComponentObjectDuringStructuralChange(src, dst, componentType, in systemHandle);
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x00031034 File Offset: 0x0002F234
		public static void AddChunkComponentData<T>(this EntityManager manager, Entity entity) where T : class, IComponentData, new()
		{
			manager.AddComponent(entity, ComponentType.ChunkComponent<T>());
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x00031044 File Offset: 0x0002F244
		public unsafe static void AddChunkComponentData<T>(this EntityManager manager, EntityQuery entityQuery, T componentData) where T : class, IComponentData, new()
		{
			EntityDataAccess* checkedEntityDataAccess = manager.GetCheckedEntityDataAccess();
			EntityComponentStore* entityComponentStore = checkedEntityDataAccess->EntityComponentStore;
			using (NativeArray<ArchetypeChunk> nativeArray = entityQuery.ToArchetypeChunkArray(Allocator.TempJob))
			{
				if (nativeArray.Length > 0)
				{
					ComponentType componentType = ComponentType.FromTypeIndex(TypeManager.MakeChunkComponentTypeIndex(ComponentType.ReadWrite<T>().TypeIndex));
					EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginStructuralChanges();
					StructuralChange.AddComponentChunks(entityComponentStore, (ArchetypeChunk*)nativeArray.GetUnsafePtr<ArchetypeChunk>(), nativeArray.Length, componentType.TypeIndex);
					checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
					manager.SetChunkComponent(nativeArray, componentData);
				}
			}
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x000310E0 File Offset: 0x0002F2E0
		private static void SetChunkComponent<T>(this EntityManager manager, NativeArray<ArchetypeChunk> chunks, T componentData) where T : class, IComponentData, new()
		{
			for (int i = 0; i < chunks.Length; i++)
			{
				ChunkIndex chunk = chunks[i].m_Chunk;
				manager.SetComponentData(chunk.MetaChunkEntity, componentData);
			}
		}
	}
}
