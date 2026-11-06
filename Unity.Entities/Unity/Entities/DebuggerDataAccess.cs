using System;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x02000045 RID: 69
	internal struct DebuggerDataAccess
	{
		// Token: 0x060001F7 RID: 503 RVA: 0x0000A0A8 File Offset: 0x000082A8
		internal unsafe DebuggerDataAccess(World world)
		{
			if (world != null && world.IsCreated)
			{
				this.ComponentStore = world.EntityManager.GetUncheckedEntityDataAccess()->EntityComponentStore;
				this.ManagedStore = world.EntityManager.GetUncheckedEntityDataAccess()->ManagedComponentStore;
				return;
			}
			this.ComponentStore = null;
			this.ManagedStore = null;
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000A104 File Offset: 0x00008304
		internal unsafe DebuggerDataAccess(EntityComponentStore* store)
		{
			this.ComponentStore = null;
			this.ManagedStore = null;
			if (store == null)
			{
				return;
			}
			ManagedEntityDataAccess managedEntityDataAccess = ManagedEntityDataAccess.Debugger_GetInstance(store->m_DebugOnlyManagedAccess);
			if (managedEntityDataAccess.ManagedComponentStore == null)
			{
				return;
			}
			if (managedEntityDataAccess.World.EntityManager.Debugger_GetEntityDataAccess() != (EntityDataAccess*)store)
			{
				return;
			}
			this.ComponentStore = store;
			this.ManagedStore = managedEntityDataAccess.ManagedComponentStore;
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x0000A165 File Offset: 0x00008365
		public bool IsCreated
		{
			get
			{
				return this.ComponentStore != null && this.ManagedStore != null;
			}
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000A17C File Offset: 0x0000837C
		public string GetDebugNameWithWorld(Entity entity)
		{
			if (this.ComponentStore != null && !entity.Equals(default(Entity)) && this.IsCreated)
			{
				return string.Format("'{0}' Entity({1}:{2}) {3}", new object[]
				{
					this.GetName(entity),
					entity.Index,
					entity.Version,
					this.GetWorld()
				});
			}
			return entity.ToString();
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000A1FC File Offset: 0x000083FC
		public string GetDebugNameWithoutWorld(Entity entity)
		{
			if (this.ComponentStore != null && !entity.Equals(default(Entity)) && this.IsCreated)
			{
				return string.Format("'{0}' Entity({1}:{2})", this.GetName(entity), entity.Index, entity.Version);
			}
			return entity.ToString();
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000A262 File Offset: 0x00008462
		private string GetName(Entity entity)
		{
			return EntityComponentStore.Debugger_GetName(this.ComponentStore, entity);
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000A270 File Offset: 0x00008470
		private unsafe object GetComponentObject(Entity entity, ComponentType componentType)
		{
			int* ptr = (int*)EntityComponentStore.Debugger_GetComponentDataWithTypeRO(this.ComponentStore, entity, componentType.TypeIndex);
			if (ptr == null)
			{
				return null;
			}
			return this.ManagedStore.Debugger_GetManagedComponent(*ptr);
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000A2A4 File Offset: 0x000084A4
		public unsafe object GetSharedComponentDataBoxed(int shareComponentIndex, TypeIndex typeIndex)
		{
			if (EntityComponentStore.IsUnmanagedSharedComponentIndex(shareComponentIndex))
			{
				return this.ComponentStore->GetSharedComponentDataObject_Unmanaged(shareComponentIndex, typeIndex);
			}
			return this.ManagedStore.GetSharedComponentDataBoxed(shareComponentIndex, typeIndex);
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000A2C9 File Offset: 0x000084C9
		internal bool Exists(Entity entity)
		{
			return EntityComponentStore.Debugger_Exists(this.ComponentStore, entity);
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000A2D7 File Offset: 0x000084D7
		private unsafe static bool SanityCheckArchetype(Archetype* archetype)
		{
			return archetype != null && archetype->TypesCount >= 1 && archetype->TypesCount < 4096 && *archetype->Offsets == 0;
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000A300 File Offset: 0x00008500
		internal unsafe object[] GetComponents(Entity entity)
		{
			if (!EntityComponentStore.Debugger_Exists(this.ComponentStore, entity))
			{
				return null;
			}
			Archetype* archetype = this.ComponentStore->GetArchetype(entity);
			if (!DebuggerDataAccess.SanityCheckArchetype(archetype))
			{
				return null;
			}
			object[] array = new object[archetype->TypesCount - 1];
			for (int i = 1; i < archetype->TypesCount; i++)
			{
				ComponentType componentType = ComponentType.FromTypeIndex(archetype->Types[i].TypeIndex);
				object obj = this.GetComponentBoxedUnchecked(entity, componentType);
				if (obj != null && TypeManager.IsEnableable(componentType.TypeIndex))
				{
					obj = new Component_E(in obj, this.ComponentStore->IsComponentEnabled(entity, componentType.TypeIndex));
				}
				array[i - 1] = obj;
			}
			return array;
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000A3AC File Offset: 0x000085AC
		internal unsafe World GetWorld()
		{
			if (this.ComponentStore == null)
			{
				return null;
			}
			return ManagedEntityDataAccess.GetInstance(this.ComponentStore->m_DebugOnlyManagedAccess)->World;
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000A3D4 File Offset: 0x000085D4
		internal unsafe EntityArchetype GetArchetype(Entity entity)
		{
			if (!EntityComponentStore.Debugger_Exists(this.ComponentStore, entity))
			{
				return default(EntityArchetype);
			}
			Archetype* archetype = this.ComponentStore->GetArchetype(entity);
			if (!DebuggerDataAccess.SanityCheckArchetype(archetype))
			{
				return default(EntityArchetype);
			}
			return new EntityArchetype
			{
				Archetype = archetype
			};
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000A42C File Offset: 0x0000862C
		internal unsafe ArchetypeChunk GetChunk(Entity entity)
		{
			if (!EntityComponentStore.Debugger_Exists(this.ComponentStore, entity))
			{
				return default(ArchetypeChunk);
			}
			return new ArchetypeChunk(this.ComponentStore->GetChunk(entity), this.ComponentStore);
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000A468 File Offset: 0x00008668
		internal unsafe object GetComponentBoxedUnchecked(Entity entity, ComponentType type)
		{
			TypeIndex typeIndex = type.TypeIndex;
			readonly ref TypeManager.TypeInfo typeInfo = ref TypeManager.GetTypeInfo(typeIndex);
			if (typeInfo.Category == TypeManager.TypeCategory.ComponentData)
			{
				if (TypeManager.IsManagedComponent(typeIndex))
				{
					return this.GetComponentObject(entity, type);
				}
				byte* ptr = EntityComponentStore.Debugger_GetComponentDataWithTypeRO(this.ComponentStore, entity, typeIndex);
				return TypeManager.ConstructComponentFromBuffer(typeIndex, (void*)ptr);
			}
			else if (typeInfo.Category == TypeManager.TypeCategory.ISharedComponentData)
			{
				int num = this.ComponentStore->Debugger_GetSharedComponentDataIndex(entity, typeIndex);
				if (num == -1)
				{
					return null;
				}
				return this.GetSharedComponentDataBoxed(num, typeIndex);
			}
			else
			{
				if (typeInfo.Category == TypeManager.TypeCategory.UnityEngineObject)
				{
					return this.GetComponentObject(entity, type);
				}
				if (typeInfo.Category != TypeManager.TypeCategory.BufferData)
				{
					return null;
				}
				BufferHeader* ptr2 = (BufferHeader*)EntityComponentStore.Debugger_GetComponentDataWithTypeRO(this.ComponentStore, entity, typeIndex);
				if (ptr2 == null || ptr2->Length < 0)
				{
					return null;
				}
				int length = ptr2->Length;
				Array array = Array.CreateInstance(TypeManager.GetType(typeIndex), length);
				int elementSize = TypeManager.GetTypeInfo(typeIndex).ElementSize;
				byte* elementPointer = BufferHeader.GetElementPointer(ptr2);
				ulong num2;
				UnsafeUtility.MemCpy(UnsafeUtility.PinGCArrayAndGetDataAddress(array, out num2), (void*)elementPointer, (long)(elementSize * length));
				UnsafeUtility.ReleaseGCObject(num2);
				return array;
			}
		}

		// Token: 0x04000085 RID: 133
		private unsafe EntityComponentStore* ComponentStore;

		// Token: 0x04000086 RID: 134
		private ManagedComponentStore ManagedStore;
	}
}
