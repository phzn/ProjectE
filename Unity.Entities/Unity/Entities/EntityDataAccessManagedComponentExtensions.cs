using System;

namespace Unity.Entities
{
	// Token: 0x0200016E RID: 366
	internal static class EntityDataAccessManagedComponentExtensions
	{
		// Token: 0x06000876 RID: 2166 RVA: 0x0002B328 File Offset: 0x00029528
		internal unsafe static int* GetManagedComponentIndex(this EntityDataAccess dataAccess, Entity entity, TypeIndex typeIndex)
		{
			if (!dataAccess.IsInExclusiveTransaction)
			{
				dataAccess.DependencyManager->CompleteReadAndWriteDependency(typeIndex);
			}
			return (int*)dataAccess.EntityComponentStore->GetComponentDataWithTypeRW(entity, typeIndex, dataAccess.EntityComponentStore->GlobalSystemVersion);
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x0002C7E0 File Offset: 0x0002A9E0
		public unsafe static T GetComponentData<T>(this EntityDataAccess dataAccess, Entity entity, ManagedComponentStore managedComponentStore) where T : class, IComponentData, new()
		{
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			int num = *(ref dataAccess).GetManagedComponentIndex(entity, typeIndex);
			return (T)((object)managedComponentStore.GetManagedComponent(num));
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x0002C80C File Offset: 0x0002AA0C
		public unsafe static T GetComponentObject<T>(this EntityDataAccess dataAccess, Entity entity, ComponentType componentType)
		{
			int num = *(ref dataAccess).GetManagedComponentIndex(entity, componentType.TypeIndex);
			return (T)((object)dataAccess.ManagedComponentStore.GetManagedComponent(num));
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x0002C83C File Offset: 0x0002AA3C
		public unsafe static void SetComponentObject(this EntityDataAccess dataAccess, Entity entity, ComponentType componentType, object componentObject, in SystemHandle originSystem = null)
		{
			int* managedComponentIndex = (ref dataAccess).GetManagedComponentIndex(entity, componentType.TypeIndex);
			dataAccess.ManagedComponentStore.UpdateManagedComponentValue(managedComponentIndex, componentObject, ref *dataAccess.EntityComponentStore);
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x0002C86C File Offset: 0x0002AA6C
		public unsafe static void MoveComponentObjectDuringStructuralChange(this EntityDataAccess dataAccess, Entity srcEntity, Entity dstEntity, ComponentType componentType, in SystemHandle originSystem = null)
		{
			if (srcEntity == dstEntity)
			{
				return;
			}
			int* managedComponentIndex = (ref dataAccess).GetManagedComponentIndex(srcEntity, componentType.TypeIndex);
			if (!dataAccess.HasComponent(dstEntity, componentType))
			{
				dataAccess.AddComponentDuringStructuralChange(dstEntity, componentType, in originSystem);
			}
			int* managedComponentIndex2 = (ref dataAccess).GetManagedComponentIndex(dstEntity, componentType.TypeIndex);
			if (*managedComponentIndex2 != 0)
			{
				object managedComponent = dataAccess.ManagedComponentStore.GetManagedComponent(*managedComponentIndex);
				object managedComponent2 = dataAccess.ManagedComponentStore.GetManagedComponent(*managedComponentIndex2);
				if (managedComponent != managedComponent2)
				{
					dataAccess.ManagedComponentStore.UpdateManagedComponentValue(managedComponentIndex2, null, ref *dataAccess.EntityComponentStore);
				}
				else if (*managedComponentIndex != *managedComponentIndex2)
				{
					dataAccess.ManagedComponentStore.SetManagedComponentValue(*managedComponentIndex2, null);
					dataAccess.EntityComponentStore->FreeManagedComponentIndex(*managedComponentIndex2);
				}
			}
			*managedComponentIndex2 = *managedComponentIndex;
			*managedComponentIndex = 0;
			dataAccess.RemoveComponentDuringStructuralChange(srcEntity, componentType, in originSystem);
		}
	}
}
