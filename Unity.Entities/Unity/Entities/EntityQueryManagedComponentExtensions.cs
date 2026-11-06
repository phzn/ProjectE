using System;

namespace Unity.Entities
{
	// Token: 0x0200021B RID: 539
	public static class EntityQueryManagedComponentExtensions
	{
		// Token: 0x06000D31 RID: 3377 RVA: 0x0003BCAC File Offset: 0x00039EAC
		public unsafe static T GetSingleton<T>(this EntityQuery query) where T : class
		{
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			EntityQueryImpl* ptr = query._GetImpl();
			EntityDataAccess* access = ptr->_Access;
			access->DependencyManager->CompleteWriteDependencyNoChecks(typeIndex);
			int num;
			ChunkIndex chunkIndex;
			int num2;
			ptr->GetSingletonChunkAndEntity(typeIndex, out num, out chunkIndex, out num2);
			Archetype* archetype = query.GetEntityComponentStore()->GetArchetype(chunkIndex);
			int num3 = *(int*)ChunkDataUtility.GetComponentDataRO(chunkIndex, archetype, num2, num);
			return (T)((object)access->ManagedComponentStore.GetManagedComponent(num3));
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x0003BD14 File Offset: 0x00039F14
		public static bool TryGetSingleton<T>(this EntityQuery query, out T value) where T : class
		{
			bool flag = query.HasSingleton<T>();
			value = (flag ? query.GetSingleton<T>() : default(T));
			return flag;
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x0003BD44 File Offset: 0x00039F44
		public unsafe static T GetSingletonRW<T>(this EntityQuery query) where T : class
		{
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			EntityQueryImpl* ptr = query._GetImpl();
			EntityDataAccess* access = ptr->_Access;
			int num;
			ChunkIndex chunkIndex;
			int num2;
			ptr->GetSingletonChunkAndEntity(typeIndex, out num, out chunkIndex, out num2);
			Archetype* archetype = access->EntityComponentStore->GetArchetype(chunkIndex);
			int num3 = *(int*)ChunkDataUtility.GetComponentDataRW(chunkIndex, archetype, num2, num, access->EntityComponentStore->GlobalSystemVersion);
			return (T)((object)access->ManagedComponentStore.GetManagedComponent(num3));
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x0003BDAC File Offset: 0x00039FAC
		public unsafe static void SetSingleton<T>(this EntityQuery query, T value) where T : class
		{
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			EntityQueryImpl* ptr = query._GetImpl();
			EntityDataAccess* access = ptr->_Access;
			access->DependencyManager->CompleteWriteDependencyNoChecks(typeIndex);
			EntityComponentStore* entityComponentStore = access->EntityComponentStore;
			int num;
			ChunkIndex chunkIndex;
			int num2;
			ptr->GetSingletonChunkAndEntity(typeIndex, out num, out chunkIndex, out num2);
			Archetype* archetype = entityComponentStore->GetArchetype(chunkIndex);
			int* componentDataRW = (int*)ChunkDataUtility.GetComponentDataRW(chunkIndex, archetype, num2, num, entityComponentStore->GlobalSystemVersion);
			access->ManagedComponentStore.UpdateManagedComponentValue(componentDataRW, value, ref *entityComponentStore);
		}
	}
}
