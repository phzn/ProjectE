using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Unity.Entities
{
	// Token: 0x0200010D RID: 269
	internal struct EntityCommandBufferData
	{
		// Token: 0x06000418 RID: 1048 RVA: 0x0001760C File Offset: 0x0001580C
		internal unsafe void InitForParallelWriter()
		{
			if (this.m_ThreadedChains != null)
			{
				return;
			}
			int threadIndexCount = JobsUtility.ThreadIndexCount;
			int num = sizeof(EntityCommandBufferChain) * threadIndexCount;
			this.m_ThreadedChains = (EntityCommandBufferChain*)Memory.Unmanaged.Allocate((long)num, 64, this.m_Allocator);
			UnsafeUtility.MemClear((void*)this.m_ThreadedChains, (long)num);
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x00017655 File Offset: 0x00015855
		internal void DestroyForParallelWriter()
		{
			if (this.m_ThreadedChains != null)
			{
				Memory.Unmanaged.Free<EntityCommandBufferChain>(this.m_ThreadedChains, this.m_Allocator);
				this.m_ThreadedChains = null;
			}
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x0001767A File Offset: 0x0001587A
		private unsafe void ResetCreateCommandBatching(EntityCommandBufferChain* chain)
		{
			chain->m_PrevCreateCommand = null;
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x00017684 File Offset: 0x00015884
		private unsafe void ResetEntityCommandBatching(EntityCommandBufferChain* chain)
		{
			chain->m_PrevEntityCommand = null;
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0001768E File Offset: 0x0001588E
		internal unsafe void ResetCommandBatching(EntityCommandBufferChain* chain)
		{
			this.ResetCreateCommandBatching(chain);
			this.ResetEntityCommandBatching(chain);
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x000176A0 File Offset: 0x000158A0
		internal unsafe Entity* CloneAndSearchForDeferredEntities(NativeArray<Entity> entities, out bool containsDeferredEntities)
		{
			Entity* ptr = (Entity*)Memory.Unmanaged.Allocate((long)(entities.Length * sizeof(Entity)), EntityCommandBufferData.ALIGN_64_BIT, this.m_Allocator);
			containsDeferredEntities = false;
			int i = 0;
			int length = entities.Length;
			while (i < length)
			{
				Entity entity = entities[i];
				ptr[i] = entity;
				if (entity.Index < 0)
				{
					containsDeferredEntities = true;
					IL_007D:
					while (i < length)
					{
						ptr[i] = entities[i];
						i++;
					}
					return ptr;
				}
				i++;
			}
			goto IL_007D;
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x00017730 File Offset: 0x00015930
		internal unsafe void AddCreateCommand(EntityCommandBufferChain* chain, int sortKey, ECBCommand op, int index, EntityArchetype archetype, bool batchable)
		{
			if (batchable && chain->m_PrevCreateCommand != null && chain->m_PrevCreateCommand->Archetype == archetype)
			{
				chain->m_PrevCreateCommand->BatchCount++;
				return;
			}
			this.ResetEntityCommandBatching(chain);
			int num = EntityCommandBufferData.Align(sizeof(CreateCommand), EntityCommandBufferData.ALIGN_64_BIT);
			CreateCommand* ptr = (CreateCommand*)this.Reserve(chain, sortKey, num);
			ptr->Header.CommandType = op;
			ptr->Header.TotalSize = sizeof(CreateCommand);
			ptr->Header.SortKey = chain->m_LastSortKey;
			ptr->Archetype = archetype;
			ptr->IdentityIndex = index;
			ptr->BatchCount = 1;
			chain->m_PrevCreateCommand = ptr;
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x000177E0 File Offset: 0x000159E0
		internal unsafe void AddEntityCommand(EntityCommandBufferChain* chain, int sortKey, ECBCommand op, int index, Entity e, bool batchable)
		{
			if (batchable && chain->m_PrevEntityCommand != null && chain->m_PrevEntityCommand->Entity == e)
			{
				chain->m_PrevEntityCommand->BatchCount++;
				return;
			}
			this.ResetCreateCommandBatching(chain);
			int num = EntityCommandBufferData.Align(sizeof(EntityCommand), EntityCommandBufferData.ALIGN_64_BIT);
			EntityCommand* ptr = (EntityCommand*)this.Reserve(chain, sortKey, num);
			ptr->Header.CommandType = op;
			ptr->Header.TotalSize = num;
			ptr->Header.SortKey = chain->m_LastSortKey;
			ptr->Entity = e;
			ptr->IdentityIndex = index;
			ptr->BatchCount = 1;
			chain->m_PrevEntityCommand = ptr;
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x0001788C File Offset: 0x00015A8C
		internal unsafe void AddLinkedEntityGroupComponentCommand<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(EntityCommandBufferChain* chain, int sortKey, ECBCommand op, EntityQueryMask mask, Entity e, T component) where T : struct, ValueType, IComponentData
		{
			ComponentType componentType = ComponentType.ReadWrite<T>();
			if (componentType.IsZeroSized)
			{
				this.AddLinkedEntityGroupTypeCommand(chain, sortKey, op, mask, e, componentType);
				return;
			}
			short num = (short)UnsafeUtility.SizeOf<T>();
			int num2 = EntityCommandBufferData.Align(sizeof(EntityQueryMaskCommand) + (int)num, EntityCommandBufferData.ALIGN_64_BIT);
			this.ResetCommandBatching(chain);
			EntityQueryMaskCommand* ptr = (EntityQueryMaskCommand*)this.Reserve(chain, sortKey, num2);
			ptr->Header.Header.Header.CommandType = op;
			ptr->Header.Header.Header.TotalSize = num2;
			ptr->Header.Header.Header.SortKey = chain->m_LastSortKey;
			ptr->Header.Header.Entity = e;
			ptr->Header.Header.IdentityIndex = 0;
			ptr->Header.Header.BatchCount = 1;
			ptr->Header.ComponentTypeIndex = componentType.TypeIndex;
			ptr->Header.ComponentSize = num;
			ptr->Mask = mask;
			byte* ptr2 = (byte*)(ptr + 1);
			UnsafeUtility.CopyStructureToPtr<T>(ref component, (void*)ptr2);
			ptr->Header.ValueRequiresEntityFixup = (this.RequiresEntityFixUp(ptr2, componentType.TypeIndex) ? 1 : 0);
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x000179B4 File Offset: 0x00015BB4
		internal unsafe void AddLinkedEntityGroupTypeCommand(EntityCommandBufferChain* chain, int sortKey, ECBCommand op, EntityQueryMask mask, Entity e, ComponentType t)
		{
			int num = EntityCommandBufferData.Align(sizeof(EntityQueryMaskCommand), EntityCommandBufferData.ALIGN_64_BIT);
			this.ResetCommandBatching(chain);
			EntityQueryMaskCommand* ptr = (EntityQueryMaskCommand*)this.Reserve(chain, sortKey, num);
			ptr->Header.Header.Header.CommandType = op;
			ptr->Header.Header.Header.TotalSize = num;
			ptr->Header.Header.Header.SortKey = chain->m_LastSortKey;
			ptr->Header.Header.Entity = e;
			ptr->Header.Header.IdentityIndex = 0;
			ptr->Header.Header.BatchCount = 1;
			ptr->Header.ComponentTypeIndex = t.TypeIndex;
			ptr->Header.ComponentSize = 0;
			ptr->Header.ValueRequiresEntityFixup = 0;
			ptr->Mask = mask;
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x00017A90 File Offset: 0x00015C90
		internal unsafe void AddMultipleEntityCommand(EntityCommandBufferChain* chain, int sortKey, ECBCommand op, int firstIndex, int count, Entity e, bool batchable)
		{
			if (batchable && chain->m_PrevEntityCommand != null && chain->m_PrevEntityCommand->Entity == e)
			{
				chain->m_PrevEntityCommand->BatchCount += count;
				return;
			}
			this.ResetCreateCommandBatching(chain);
			int num = EntityCommandBufferData.Align(sizeof(EntityCommand), EntityCommandBufferData.ALIGN_64_BIT);
			EntityCommand* ptr = (EntityCommand*)this.Reserve(chain, sortKey, num);
			ptr->Header.CommandType = op;
			ptr->Header.TotalSize = num;
			ptr->Header.SortKey = chain->m_LastSortKey;
			ptr->Entity = e;
			ptr->IdentityIndex = firstIndex;
			ptr->BatchCount = count;
			chain->m_PrevEntityCommand = null;
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x00017B3C File Offset: 0x00015D3C
		internal unsafe bool RequiresEntityFixUp(byte* data, TypeIndex typeIndex)
		{
			if (!TypeManager.HasEntityReferences(typeIndex))
			{
				return false;
			}
			int num;
			TypeManager.EntityOffsetInfo* entityOffsets = TypeManager.GetEntityOffsets(typeIndex, out num);
			for (int i = 0; i < num; i++)
			{
				if (((Entity*)(data + entityOffsets[i].Offset))->Index < 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x00017B88 File Offset: 0x00015D88
		internal unsafe void AddEntityComponentTypeWithValueCommand<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(EntityCommandBufferChain* chain, int sortKey, ECBCommand op, Entity e, T component) where T : struct, ValueType, IComponentData
		{
			ComponentType componentType = ComponentType.ReadWrite<T>();
			if (componentType.IsZeroSized)
			{
				this.AddEntityComponentTypeWithoutValueCommand(chain, sortKey, op, e, componentType);
				return;
			}
			short num = (short)UnsafeUtility.SizeOf<T>();
			int num2 = EntityCommandBufferData.Align(sizeof(EntityComponentCommand) + (int)num, EntityCommandBufferData.ALIGN_64_BIT);
			this.ResetCommandBatching(chain);
			EntityComponentCommand* ptr = (EntityComponentCommand*)this.Reserve(chain, sortKey, num2);
			ptr->Header.Header.CommandType = op;
			ptr->Header.Header.TotalSize = num2;
			ptr->Header.Header.SortKey = chain->m_LastSortKey;
			ptr->Header.Entity = e;
			ptr->Header.IdentityIndex = 0;
			ptr->Header.BatchCount = 1;
			ptr->ComponentTypeIndex = componentType.TypeIndex;
			ptr->ComponentSize = num;
			byte* ptr2 = (byte*)(ptr + 1);
			UnsafeUtility.CopyStructureToPtr<T>(ref component, (void*)ptr2);
			ptr->ValueRequiresEntityFixup = (this.RequiresEntityFixUp(ptr2, componentType.TypeIndex) ? 1 : 0);
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x00017C7C File Offset: 0x00015E7C
		internal unsafe void UnsafeAddEntityComponentCommand(EntityCommandBufferChain* chain, int sortKey, ECBCommand op, Entity e, TypeIndex typeIndex, int typeSize, void* componentDataPtr)
		{
			int num = EntityCommandBufferData.Align(sizeof(EntityComponentCommand) + typeSize, EntityCommandBufferData.ALIGN_64_BIT);
			this.ResetCommandBatching(chain);
			EntityComponentCommand* ptr = (EntityComponentCommand*)this.Reserve(chain, sortKey, num);
			ptr->Header.Header.CommandType = op;
			ptr->Header.Header.TotalSize = num;
			ptr->Header.Header.SortKey = chain->m_LastSortKey;
			ptr->Header.Entity = e;
			ptr->Header.IdentityIndex = 0;
			ptr->Header.BatchCount = 1;
			ptr->ComponentTypeIndex = typeIndex;
			ptr->ComponentSize = (short)typeSize;
			byte* ptr2 = (byte*)(ptr + 1);
			UnsafeUtility.MemCpy((void*)ptr2, componentDataPtr, (long)typeSize);
			ptr->ValueRequiresEntityFixup = (this.RequiresEntityFixUp(ptr2, typeIndex) ? 1 : 0);
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x00017D48 File Offset: 0x00015F48
		internal unsafe void AddEntityEnabledCommand(EntityCommandBufferChain* chain, int sortKey, ECBCommand op, Entity e, bool value)
		{
			int num = EntityCommandBufferData.Align(sizeof(EntityEnabledCommand), EntityCommandBufferData.ALIGN_64_BIT);
			this.ResetCommandBatching(chain);
			EntityEnabledCommand* ptr = (EntityEnabledCommand*)this.Reserve(chain, sortKey, num);
			ptr->Header.Header.CommandType = op;
			ptr->Header.Header.TotalSize = num;
			ptr->Header.Header.SortKey = chain->m_LastSortKey;
			ptr->Header.Entity = e;
			ptr->Header.IdentityIndex = 0;
			ptr->Header.BatchCount = 1;
			ptr->IsEnabled = (value ? 1 : 0);
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x00017DE4 File Offset: 0x00015FE4
		internal unsafe void AddEntityComponentEnabledCommand(EntityCommandBufferChain* chain, int sortKey, ECBCommand op, Entity e, TypeIndex typeIndex, bool value)
		{
			int num = EntityCommandBufferData.Align(sizeof(EntityComponentEnabledCommand), EntityCommandBufferData.ALIGN_64_BIT);
			this.ResetCommandBatching(chain);
			EntityComponentEnabledCommand* ptr = (EntityComponentEnabledCommand*)this.Reserve(chain, sortKey, num);
			ptr->Header.Header.Header.CommandType = op;
			ptr->Header.Header.Header.TotalSize = num;
			ptr->Header.Header.Header.SortKey = chain->m_LastSortKey;
			ptr->Header.Header.Entity = e;
			ptr->Header.Header.IdentityIndex = 0;
			ptr->Header.Header.BatchCount = 1;
			ptr->ComponentTypeIndex = typeIndex;
			ptr->Header.IsEnabled = (value ? 1 : 0);
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x00017EAC File Offset: 0x000160AC
		internal unsafe void AddEntityNameCommand(EntityCommandBufferChain* chain, int sortKey, ECBCommand op, Entity e, in FixedString64Bytes name)
		{
			int num = EntityCommandBufferData.Align(sizeof(EntityNameCommand), EntityCommandBufferData.ALIGN_64_BIT);
			this.ResetCommandBatching(chain);
			EntityNameCommand* ptr = (EntityNameCommand*)this.Reserve(chain, sortKey, num);
			ptr->Header.Header.CommandType = op;
			ptr->Header.Header.TotalSize = num;
			ptr->Header.Header.SortKey = chain->m_LastSortKey;
			ptr->Header.Entity = e;
			ptr->Header.IdentityIndex = 0;
			ptr->Header.BatchCount = 1;
			ptr->Name = name;
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x00017F48 File Offset: 0x00016148
		internal unsafe BufferHeader* AddEntityBufferCommand<T>(EntityCommandBufferChain* chain, int sortKey, ECBCommand op, Entity e, out int internalCapacity) where T : struct, IBufferElementData
		{
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			readonly ref TypeManager.TypeInfo typeInfo = ref TypeManager.GetTypeInfo<T>();
			int num = EntityCommandBufferData.Align(sizeof(EntityBufferCommand) + typeInfo.SizeInChunk, EntityCommandBufferData.ALIGN_64_BIT);
			this.ResetCommandBatching(chain);
			EntityBufferCommand* ptr = (EntityBufferCommand*)this.Reserve(chain, sortKey, num);
			ptr->Header.Header.CommandType = op;
			ptr->Header.Header.TotalSize = num;
			ptr->Header.Header.SortKey = chain->m_LastSortKey;
			ptr->Header.Entity = e;
			ptr->Header.IdentityIndex = 0;
			ptr->Header.BatchCount = 1;
			ptr->ComponentTypeIndex = typeIndex;
			ptr->ComponentSize = (short)typeInfo.SizeInChunk;
			ptr->ValueRequiresEntityFixup = 0;
			BufferHeader* ptr2 = &ptr->BufferNode.TempBuffer;
			BufferHeader.Initialize(ptr2, typeInfo.BufferCapacity);
			ptr->BufferNode.Prev = chain->m_Cleanup->BufferCleanupList;
			chain->m_Cleanup->BufferCleanupList = &ptr->BufferNode;
			this.m_ForceFullDispose = true;
			internalCapacity = typeInfo.BufferCapacity;
			if (TypeManager.HasEntityReferences(typeIndex))
			{
				ptr->ValueRequiresEntityFixup = 1;
				this.m_BufferWithFixups.Add(1);
			}
			return ptr2;
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0001806E File Offset: 0x0001626E
		internal static int Align(int size, int alignmentPowerOfTwo)
		{
			return (size + alignmentPowerOfTwo - 1) & ~(alignmentPowerOfTwo - 1);
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0001807C File Offset: 0x0001627C
		internal unsafe void AddEntityComponentTypeWithoutValueCommand(EntityCommandBufferChain* chain, int sortKey, ECBCommand op, Entity e, ComponentType t)
		{
			int num = EntityCommandBufferData.Align(sizeof(EntityComponentCommand), EntityCommandBufferData.ALIGN_64_BIT);
			this.ResetCommandBatching(chain);
			EntityComponentCommand* ptr = (EntityComponentCommand*)this.Reserve(chain, sortKey, num);
			ptr->Header.Header.CommandType = op;
			ptr->Header.Header.TotalSize = num;
			ptr->Header.Header.SortKey = chain->m_LastSortKey;
			ptr->Header.Entity = e;
			ptr->Header.IdentityIndex = 0;
			ptr->Header.BatchCount = 1;
			ptr->ComponentTypeIndex = t.TypeIndex;
			ptr->ComponentSize = 0;
			ptr->ValueRequiresEntityFixup = 0;
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x00018124 File Offset: 0x00016324
		internal unsafe void AddEntityComponentTypesCommand(EntityCommandBufferChain* chain, int sortKey, ECBCommand op, Entity e, in ComponentTypeSet t)
		{
			int num = EntityCommandBufferData.Align(sizeof(EntityMultipleComponentsCommand), EntityCommandBufferData.ALIGN_64_BIT);
			this.ResetCommandBatching(chain);
			EntityMultipleComponentsCommand* ptr = (EntityMultipleComponentsCommand*)this.Reserve(chain, sortKey, num);
			ptr->Header.Header.CommandType = op;
			ptr->Header.Header.TotalSize = num;
			ptr->Header.Header.SortKey = chain->m_LastSortKey;
			ptr->Header.Entity = e;
			ptr->Header.IdentityIndex = 0;
			ptr->Header.BatchCount = 1;
			ptr->TypeSet = t;
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x000181C0 File Offset: 0x000163C0
		internal unsafe bool AppendMultipleEntitiesCommand(EntityCommandBufferChain* chain, int sortKey, ECBCommand op, EntityQuery entityQuery)
		{
			NativeArray<Entity> nativeArray = entityQuery.ToEntityArray(this.m_Allocator.ToAllocator);
			if (nativeArray.Length == 0)
			{
				nativeArray.Dispose();
				return false;
			}
			return this.AppendMultipleEntitiesCommand(chain, sortKey, op, (Entity*)nativeArray.GetUnsafeReadOnlyPtr<Entity>(), nativeArray.Length, false);
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x00018210 File Offset: 0x00016410
		internal unsafe bool AppendMultipleEntitiesCommand(EntityCommandBufferChain* chain, int sortKey, ECBCommand op, Entity* entities, int entityCount, bool mayContainDeferredEntities)
		{
			int num = EntityCommandBufferData.Align(sizeof(MultipleEntitiesCommand), EntityCommandBufferData.ALIGN_64_BIT);
			this.ResetCommandBatching(chain);
			MultipleEntitiesCommand* ptr = (MultipleEntitiesCommand*)this.Reserve(chain, sortKey, num);
			ptr->Entities.Ptr = entities;
			ptr->Entities.Prev = chain->m_Cleanup->EntityArraysCleanupList;
			chain->m_Cleanup->EntityArraysCleanupList = &ptr->Entities;
			ptr->EntitiesCount = entityCount;
			ptr->Allocator = this.m_Allocator;
			ptr->SkipDeferredEntityLookup = (mayContainDeferredEntities ? 0 : 1);
			ptr->Header.CommandType = op;
			ptr->Header.TotalSize = num;
			ptr->Header.SortKey = chain->m_LastSortKey;
			return true;
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x000182C0 File Offset: 0x000164C0
		internal unsafe bool AppendMultipleEntitiesComponentCommandWithValue<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(EntityCommandBufferChain* chain, int sortKey, ECBCommand op, EntityQuery entityQuery, T component) where T : struct, ValueType, IComponentData
		{
			NativeArray<Entity> nativeArray = entityQuery.ToEntityArray(this.m_Allocator.ToAllocator);
			if (nativeArray.Length == 0)
			{
				nativeArray.Dispose();
				return false;
			}
			return this.AppendMultipleEntitiesComponentCommandWithValue<T>(chain, sortKey, op, (Entity*)nativeArray.GetUnsafeReadOnlyPtr<Entity>(), nativeArray.Length, false, component);
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x00018310 File Offset: 0x00016510
		internal unsafe bool AppendMultipleEntitiesComponentCommandWithValue<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(EntityCommandBufferChain* chain, int sortKey, ECBCommand op, Entity* entities, int entityCount, bool mayContainDeferredEntities, T component) where T : struct, ValueType, IComponentData
		{
			ComponentType componentType = ComponentType.ReadWrite<T>();
			if (componentType.IsZeroSized)
			{
				return this.AppendMultipleEntitiesComponentCommand(chain, sortKey, op, entities, entityCount, mayContainDeferredEntities, componentType);
			}
			short num = (short)UnsafeUtility.SizeOf<T>();
			int num2 = EntityCommandBufferData.Align(sizeof(MultipleEntitiesComponentCommand) + (int)num, EntityCommandBufferData.ALIGN_64_BIT);
			this.ResetCommandBatching(chain);
			MultipleEntitiesComponentCommand* ptr = (MultipleEntitiesComponentCommand*)this.Reserve(chain, sortKey, num2);
			ptr->Header.Entities.Ptr = entities;
			ptr->Header.Entities.Prev = chain->m_Cleanup->EntityArraysCleanupList;
			chain->m_Cleanup->EntityArraysCleanupList = &ptr->Header.Entities;
			ptr->Header.EntitiesCount = entityCount;
			ptr->Header.Allocator = this.m_Allocator;
			ptr->Header.SkipDeferredEntityLookup = (mayContainDeferredEntities ? 0 : 1);
			ptr->Header.Header.CommandType = op;
			ptr->Header.Header.TotalSize = num2;
			ptr->Header.Header.SortKey = chain->m_LastSortKey;
			ptr->ComponentTypeIndex = componentType.TypeIndex;
			ptr->ComponentSize = num;
			byte* ptr2 = (byte*)(ptr + 1);
			UnsafeUtility.CopyStructureToPtr<T>(ref component, (void*)ptr2);
			ptr->ValueRequiresEntityFixup = (this.RequiresEntityFixUp(ptr2, componentType.TypeIndex) ? 1 : 0);
			return true;
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x00018458 File Offset: 0x00016658
		internal unsafe bool AppendMultipleEntitiesComponentCommandWithObject(EntityCommandBufferChain* chain, int sortKey, ECBCommand op, EntityQuery entityQuery, object boxedComponent, ComponentType ctype)
		{
			NativeArray<Entity> nativeArray = entityQuery.ToEntityArray(this.m_Allocator.ToAllocator);
			if (nativeArray.Length == 0)
			{
				nativeArray.Dispose();
				return false;
			}
			return this.AppendMultipleEntitiesComponentCommandWithObject(chain, sortKey, op, (Entity*)nativeArray.GetUnsafeReadOnlyPtr<Entity>(), nativeArray.Length, false, boxedComponent, ctype);
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x000184AC File Offset: 0x000166AC
		internal unsafe bool AppendMultipleEntitiesComponentCommandWithObject(EntityCommandBufferChain* chain, int sortKey, ECBCommand op, Entity* entities, int entityCount, bool mayContainDeferredEntities, object boxedComponent, ComponentType ctype)
		{
			int num = EntityCommandBufferData.Align(sizeof(MultipleEntitiesComponentCommandWithObject), EntityCommandBufferData.ALIGN_64_BIT);
			this.ResetCommandBatching(chain);
			MultipleEntitiesComponentCommandWithObject* ptr = (MultipleEntitiesComponentCommandWithObject*)this.Reserve(chain, sortKey, num);
			ptr->Header.Entities.Ptr = entities;
			ptr->Header.Entities.Prev = chain->m_Cleanup->EntityArraysCleanupList;
			chain->m_Cleanup->EntityArraysCleanupList = &ptr->Header.Entities;
			ptr->Header.EntitiesCount = entityCount;
			ptr->Header.Allocator = this.m_Allocator;
			ptr->Header.SkipDeferredEntityLookup = (mayContainDeferredEntities ? 0 : 1);
			ptr->Header.Header.CommandType = op;
			ptr->Header.Header.TotalSize = num;
			ptr->Header.Header.SortKey = chain->m_LastSortKey;
			ptr->ComponentTypeIndex = ctype.TypeIndex;
			if (boxedComponent != null)
			{
				ptr->GCNode.BoxedObject = GCHandle.Alloc(boxedComponent);
				ptr->GCNode.Prev = chain->m_Cleanup->CleanupList;
				chain->m_Cleanup->CleanupList = &ptr->GCNode;
			}
			else
			{
				ptr->GCNode.BoxedObject = default(GCHandle);
			}
			return true;
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x000185E8 File Offset: 0x000167E8
		internal unsafe bool AppendEntityQueryComponentCommandWithSharedValue<T>(EntityCommandBufferChain* chain, int sortKey, ECBCommand op, EntityQuery entityQuery, int hashCode, object boxedComponent) where T : struct, ISharedComponentData
		{
			ComponentType componentType = ComponentType.ReadWrite<T>();
			int num = EntityCommandBufferData.Align(sizeof(EntityQueryComponentCommandWithObject), EntityCommandBufferData.ALIGN_64_BIT);
			this.ResetCommandBatching(chain);
			EntityQueryComponentCommandWithObject* ptr = (EntityQueryComponentCommandWithObject*)this.Reserve(chain, sortKey, num);
			ptr->Header.Header.Header.CommandType = op;
			ptr->Header.Header.Header.TotalSize = num;
			ptr->Header.Header.Header.SortKey = chain->m_LastSortKey;
			ptr->Header.Header.QueryImpl = entityQuery._GetImpl();
			ptr->Header.ComponentTypeIndex = componentType.TypeIndex;
			ptr->HashCode = hashCode;
			if (boxedComponent != null)
			{
				ptr->GCNode.BoxedObject = GCHandle.Alloc(boxedComponent);
				ptr->GCNode.Prev = chain->m_Cleanup->CleanupList;
				chain->m_Cleanup->CleanupList = &ptr->GCNode;
			}
			else
			{
				ptr->GCNode.BoxedObject = default(GCHandle);
			}
			return true;
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x000186E8 File Offset: 0x000168E8
		internal unsafe bool AppendMultipleEntitiesComponentCommandWithSharedValue<T>(EntityCommandBufferChain* chain, int sortKey, ECBCommand op, EntityQuery entityQuery, int hashCode, object boxedComponent) where T : struct, ISharedComponentData
		{
			NativeArray<Entity> nativeArray = entityQuery.ToEntityArray(this.m_Allocator.ToAllocator);
			if (nativeArray.Length == 0)
			{
				nativeArray.Dispose();
				return false;
			}
			return this.AppendMultipleEntitiesComponentCommandWithSharedValue<T>(chain, sortKey, op, (Entity*)nativeArray.GetUnsafeReadOnlyPtr<Entity>(), nativeArray.Length, false, hashCode, boxedComponent);
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0001873C File Offset: 0x0001693C
		internal unsafe bool AppendMultipleEntitiesComponentCommandWithSharedValue<T>(EntityCommandBufferChain* chain, int sortKey, ECBCommand op, Entity* entities, int entityCount, bool mayContainDeferredEntities, int hashCode, object boxedComponent) where T : struct, ISharedComponentData
		{
			ComponentType componentType = ComponentType.ReadWrite<T>();
			int num = EntityCommandBufferData.Align(sizeof(MultipleEntitiesComponentCommandWithObject), EntityCommandBufferData.ALIGN_64_BIT);
			this.ResetCommandBatching(chain);
			MultipleEntitiesComponentCommandWithObject* ptr = (MultipleEntitiesComponentCommandWithObject*)this.Reserve(chain, sortKey, num);
			ptr->Header.Entities.Ptr = entities;
			ptr->Header.Entities.Prev = chain->m_Cleanup->EntityArraysCleanupList;
			chain->m_Cleanup->EntityArraysCleanupList = &ptr->Header.Entities;
			ptr->Header.EntitiesCount = entityCount;
			ptr->Header.Allocator = this.m_Allocator;
			ptr->Header.SkipDeferredEntityLookup = (mayContainDeferredEntities ? 0 : 1);
			ptr->Header.Header.CommandType = op;
			ptr->Header.Header.TotalSize = num;
			ptr->Header.Header.SortKey = chain->m_LastSortKey;
			ptr->ComponentTypeIndex = componentType.TypeIndex;
			ptr->HashCode = hashCode;
			if (boxedComponent != null)
			{
				ptr->GCNode.BoxedObject = GCHandle.Alloc(boxedComponent);
				ptr->GCNode.Prev = chain->m_Cleanup->CleanupList;
				chain->m_Cleanup->CleanupList = &ptr->GCNode;
			}
			else
			{
				ptr->GCNode.BoxedObject = default(GCHandle);
			}
			return true;
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x00018884 File Offset: 0x00016A84
		internal unsafe bool AppendMultipleEntitiesCommand_WithUnmanagedSharedValue<T>(EntityCommandBufferChain* chain, int sortKey, ECBCommand op, EntityQuery entityQuery, bool mayContainDeferredEntities, int hashCode, void* componentAddr) where T : struct, ISharedComponentData
		{
			NativeArray<Entity> nativeArray = entityQuery.ToEntityArray(this.m_Allocator.ToAllocator);
			if (nativeArray.Length == 0)
			{
				nativeArray.Dispose();
				return false;
			}
			this.AppendMultipleEntitiesCommand_WithUnmanagedSharedValue<T>(chain, sortKey, op, (Entity*)nativeArray.GetUnsafeReadOnlyPtr<Entity>(), nativeArray.Length, mayContainDeferredEntities, hashCode, componentAddr);
			return true;
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x000188DC File Offset: 0x00016ADC
		internal unsafe bool AppendEntityQueryComponentCommandWithUnmanagedSharedValue<T>(EntityCommandBufferChain* chain, int sortKey, ECBCommand op, EntityQuery entityQuery, int hashCode, void* componentAddr) where T : struct, ISharedComponentData
		{
			ComponentType componentType = ComponentType.ReadWrite<T>();
			int num = UnsafeUtility.SizeOf<T>();
			int num2 = EntityCommandBufferData.Align(sizeof(EntityQueryComponentCommandWithUnmanagedSharedComponent) + num, EntityCommandBufferData.ALIGN_64_BIT);
			this.ResetCommandBatching(chain);
			EntityQueryComponentCommandWithUnmanagedSharedComponent* ptr = (EntityQueryComponentCommandWithUnmanagedSharedComponent*)this.Reserve(chain, sortKey, num2);
			ptr->Header.Header.Header.CommandType = op;
			ptr->Header.Header.Header.TotalSize = num2;
			ptr->Header.Header.Header.SortKey = chain->m_LastSortKey;
			ptr->Header.Header.QueryImpl = entityQuery._GetImpl();
			ptr->Header.ComponentTypeIndex = componentType.TypeIndex;
			ptr->ComponentSize = num;
			ptr->HashCode = hashCode;
			ptr->IsDefault = ((componentAddr == null) ? 1 : 0);
			byte* ptr2 = (byte*)(ptr + 1);
			if (componentAddr != null)
			{
				UnsafeUtility.MemCpy((void*)ptr2, componentAddr, (long)num);
			}
			else
			{
				UnsafeUtility.MemSet((void*)ptr2, 0, (long)num);
			}
			ptr->ValueRequiresEntityFixup = (this.RequiresEntityFixUp(ptr2, componentType.TypeIndex) ? 1 : 0);
			return true;
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x000189EC File Offset: 0x00016BEC
		internal unsafe bool AppendMultipleEntitiesCommand_WithUnmanagedSharedValue<T>(EntityCommandBufferChain* chain, int sortKey, ECBCommand op, Entity* entities, int entityCount, bool mayContainDeferredEntities, int hashCode, void* componentAddr) where T : struct, ISharedComponentData
		{
			ComponentType componentType = ComponentType.ReadWrite<T>();
			int num = UnsafeUtility.SizeOf<T>();
			int num2 = EntityCommandBufferData.Align(sizeof(MultipleEntitiesCommand_WithUnmanagedSharedComponent) + num, EntityCommandBufferData.ALIGN_64_BIT);
			this.ResetCommandBatching(chain);
			MultipleEntitiesCommand_WithUnmanagedSharedComponent* ptr = (MultipleEntitiesCommand_WithUnmanagedSharedComponent*)this.Reserve(chain, sortKey, num2);
			ptr->Header.Entities.Ptr = entities;
			ptr->Header.Entities.Prev = chain->m_Cleanup->EntityArraysCleanupList;
			chain->m_Cleanup->EntityArraysCleanupList = &ptr->Header.Entities;
			ptr->Header.EntitiesCount = entityCount;
			ptr->Header.Allocator = this.m_Allocator;
			ptr->Header.SkipDeferredEntityLookup = (mayContainDeferredEntities ? 0 : 1);
			ptr->Header.Header.CommandType = op;
			ptr->Header.Header.TotalSize = num2;
			ptr->Header.Header.SortKey = chain->m_LastSortKey;
			ptr->ComponentTypeIndex = componentType.TypeIndex;
			ptr->ComponentSize = num;
			ptr->HashCode = hashCode;
			ptr->IsDefault = ((componentAddr == null) ? 1 : 0);
			byte* ptr2 = (byte*)(ptr + 1);
			if (componentAddr != null)
			{
				UnsafeUtility.MemCpy((void*)ptr2, componentAddr, (long)num);
			}
			else
			{
				UnsafeUtility.MemSet((void*)ptr2, 0, (long)num);
			}
			ptr->ValueRequiresEntityFixup = (this.RequiresEntityFixUp(ptr2, componentType.TypeIndex) ? 1 : 0);
			return true;
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00018B44 File Offset: 0x00016D44
		internal unsafe bool AppendEntityQueryCommand(EntityCommandBufferChain* chain, int sortKey, ECBCommand op, EntityQuery entityQuery)
		{
			int num = EntityCommandBufferData.Align(sizeof(EntityQueryCommand), EntityCommandBufferData.ALIGN_64_BIT);
			this.ResetCommandBatching(chain);
			EntityQueryCommand* ptr = (EntityQueryCommand*)this.Reserve(chain, sortKey, num);
			ptr->QueryImpl = entityQuery._GetImpl();
			ptr->Header.CommandType = op;
			ptr->Header.TotalSize = num;
			ptr->Header.SortKey = chain->m_LastSortKey;
			return true;
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00018BAC File Offset: 0x00016DAC
		internal unsafe bool AppendEntityQueryComponentCommand(EntityCommandBufferChain* chain, int sortKey, ECBCommand op, EntityQuery entityQuery, ComponentType t)
		{
			int num = EntityCommandBufferData.Align(sizeof(EntityQueryComponentCommand), EntityCommandBufferData.ALIGN_64_BIT);
			this.ResetCommandBatching(chain);
			EntityQueryComponentCommand* ptr = (EntityQueryComponentCommand*)this.Reserve(chain, sortKey, num);
			ptr->Header.QueryImpl = entityQuery._GetImpl();
			ptr->Header.Header.CommandType = op;
			ptr->Header.Header.TotalSize = num;
			ptr->Header.Header.SortKey = chain->m_LastSortKey;
			ptr->ComponentTypeIndex = t.TypeIndex;
			return true;
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x00018C34 File Offset: 0x00016E34
		internal unsafe bool AppendEntityQueryComponentTypeSetCommand(EntityCommandBufferChain* chain, int sortKey, ECBCommand op, EntityQuery entityQuery, in ComponentTypeSet typeSet)
		{
			int num = EntityCommandBufferData.Align(sizeof(EntityQueryComponentTypeSetCommand), EntityCommandBufferData.ALIGN_64_BIT);
			this.ResetCommandBatching(chain);
			EntityQueryComponentTypeSetCommand* ptr = (EntityQueryComponentTypeSetCommand*)this.Reserve(chain, sortKey, num);
			ptr->Header.QueryImpl = entityQuery._GetImpl();
			ptr->Header.Header.CommandType = op;
			ptr->Header.Header.TotalSize = num;
			ptr->Header.Header.SortKey = chain->m_LastSortKey;
			ptr->TypeSet = typeSet;
			return true;
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00018CBC File Offset: 0x00016EBC
		internal unsafe bool AppendMultipleEntitiesComponentCommand(EntityCommandBufferChain* chain, int sortKey, ECBCommand op, EntityQuery entityQuery, ComponentType t)
		{
			NativeArray<Entity> nativeArray = entityQuery.ToEntityArray(this.m_Allocator.ToAllocator);
			if (nativeArray.Length == 0)
			{
				nativeArray.Dispose();
				return false;
			}
			return this.AppendMultipleEntitiesComponentCommand(chain, sortKey, op, (Entity*)nativeArray.GetUnsafeReadOnlyPtr<Entity>(), nativeArray.Length, false, t);
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00018D0C File Offset: 0x00016F0C
		internal unsafe bool AppendMultipleEntitiesComponentCommand(EntityCommandBufferChain* chain, int sortKey, ECBCommand op, Entity* entities, int entityCount, bool mayContainDeferredEntities, ComponentType t)
		{
			int num = EntityCommandBufferData.Align(sizeof(MultipleEntitiesComponentCommand), EntityCommandBufferData.ALIGN_64_BIT);
			this.ResetCommandBatching(chain);
			MultipleEntitiesComponentCommand* ptr = (MultipleEntitiesComponentCommand*)this.Reserve(chain, sortKey, num);
			ptr->Header.Entities.Ptr = entities;
			ptr->Header.Entities.Prev = chain->m_Cleanup->EntityArraysCleanupList;
			chain->m_Cleanup->EntityArraysCleanupList = &ptr->Header.Entities;
			ptr->Header.EntitiesCount = entityCount;
			ptr->Header.Allocator = this.m_Allocator;
			ptr->Header.SkipDeferredEntityLookup = (mayContainDeferredEntities ? 0 : 1);
			ptr->Header.Header.CommandType = op;
			ptr->Header.Header.TotalSize = num;
			ptr->Header.Header.SortKey = chain->m_LastSortKey;
			ptr->ComponentTypeIndex = t.TypeIndex;
			ptr->ComponentSize = 0;
			ptr->ValueRequiresEntityFixup = 0;
			return true;
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x00018E04 File Offset: 0x00017004
		internal unsafe bool AppendMultipleEntitiesMultipleComponentsCommand(EntityCommandBufferChain* chain, int sortKey, ECBCommand op, EntityQuery entityQuery, in ComponentTypeSet t)
		{
			NativeArray<Entity> nativeArray = entityQuery.ToEntityArray(this.m_Allocator.ToAllocator);
			if (nativeArray.Length == 0)
			{
				nativeArray.Dispose();
				return false;
			}
			return this.AppendMultipleEntitiesMultipleComponentsCommand(chain, sortKey, op, (Entity*)nativeArray.GetUnsafeReadOnlyPtr<Entity>(), nativeArray.Length, false, in t);
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x00018E54 File Offset: 0x00017054
		internal unsafe bool AppendMultipleEntitiesMultipleComponentsCommand(EntityCommandBufferChain* chain, int sortKey, ECBCommand op, Entity* entities, int entityCount, bool mayContainDeferredEntities, in ComponentTypeSet t)
		{
			int num = EntityCommandBufferData.Align(sizeof(MultipleEntitiesAndComponentsCommand), EntityCommandBufferData.ALIGN_64_BIT);
			this.ResetCommandBatching(chain);
			MultipleEntitiesAndComponentsCommand* ptr = (MultipleEntitiesAndComponentsCommand*)this.Reserve(chain, sortKey, num);
			ptr->Header.Entities.Ptr = entities;
			ptr->Header.Entities.Prev = chain->m_Cleanup->EntityArraysCleanupList;
			chain->m_Cleanup->EntityArraysCleanupList = &ptr->Header.Entities;
			ptr->Header.EntitiesCount = entityCount;
			ptr->Header.Allocator = this.m_Allocator;
			ptr->Header.SkipDeferredEntityLookup = (mayContainDeferredEntities ? 0 : 1);
			ptr->Header.Header.CommandType = op;
			ptr->Header.Header.TotalSize = num;
			ptr->Header.Header.SortKey = chain->m_LastSortKey;
			ptr->TypeSet = t;
			return true;
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x00018F40 File Offset: 0x00017140
		internal unsafe void AddEntitySharedComponentCommand<T>(EntityCommandBufferChain* chain, int sortKey, ECBCommand op, Entity e, int hashCode, object boxedObject) where T : struct
		{
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			this.AddEntitySharedComponentCommand(chain, sortKey, op, e, hashCode, typeIndex, boxedObject);
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x00018F64 File Offset: 0x00017164
		internal unsafe void AddEntitySharedComponentCommand(EntityCommandBufferChain* chain, int sortKey, ECBCommand op, Entity e, int hashCode, TypeIndex typeIndex, object boxedObject)
		{
			int num = EntityCommandBufferData.Align(sizeof(EntitySharedComponentCommand), EntityCommandBufferData.ALIGN_64_BIT);
			this.ResetCommandBatching(chain);
			EntitySharedComponentCommand* ptr = (EntitySharedComponentCommand*)this.Reserve(chain, sortKey, num);
			ptr->Header.Header.CommandType = op;
			ptr->Header.Header.TotalSize = num;
			ptr->Header.Header.SortKey = chain->m_LastSortKey;
			ptr->Header.Entity = e;
			ptr->Header.IdentityIndex = 0;
			ptr->Header.BatchCount = 1;
			ptr->ComponentTypeIndex = typeIndex;
			ptr->HashCode = hashCode;
			if (boxedObject != null)
			{
				ptr->GCNode.BoxedObject = GCHandle.Alloc(boxedObject);
				ptr->GCNode.Prev = chain->m_Cleanup->CleanupList;
				chain->m_Cleanup->CleanupList = &ptr->GCNode;
				return;
			}
			ptr->GCNode.BoxedObject = default(GCHandle);
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x00019050 File Offset: 0x00017250
		internal unsafe void AddEntityUnmanagedSharedComponentCommand<T>(EntityCommandBufferChain* chain, int sortKey, ECBCommand op, Entity e, int hashCode, void* componentData) where T : struct
		{
			int num = UnsafeUtility.SizeOf<T>();
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			this.AddEntityUnmanagedSharedComponentCommand(chain, sortKey, op, e, hashCode, typeIndex, num, componentData);
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x0001907C File Offset: 0x0001727C
		internal unsafe void AddEntityUnmanagedSharedComponentCommand(EntityCommandBufferChain* chain, int sortKey, ECBCommand op, Entity e, int hashCode, TypeIndex typeIndex, int typeSize, void* componentData)
		{
			int num = EntityCommandBufferData.Align(sizeof(EntityUnmanagedSharedComponentCommand) + typeSize, EntityCommandBufferData.ALIGN_64_BIT);
			this.ResetCommandBatching(chain);
			EntityUnmanagedSharedComponentCommand* ptr = (EntityUnmanagedSharedComponentCommand*)this.Reserve(chain, sortKey, num);
			ptr->Header.Header.CommandType = op;
			ptr->Header.Header.TotalSize = num;
			ptr->Header.Header.SortKey = chain->m_LastSortKey;
			ptr->Header.Entity = e;
			ptr->Header.IdentityIndex = 0;
			ptr->Header.BatchCount = 1;
			ptr->ComponentTypeIndex = typeIndex;
			ptr->HashCode = hashCode;
			ptr->IsDefault = ((componentData == null) ? 1 : 0);
			byte* ptr2 = (byte*)(ptr + 1);
			if (componentData != null)
			{
				UnsafeUtility.MemCpy((void*)ptr2, componentData, (long)typeSize);
			}
			else
			{
				UnsafeUtility.MemSet((void*)ptr2, 0, (long)typeSize);
			}
			ptr->ValueRequiresEntityFixup = (this.RequiresEntityFixUp(ptr2, typeIndex) ? 1 : 0);
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00019168 File Offset: 0x00017368
		internal unsafe byte* Reserve(EntityCommandBufferChain* chain, int sortKey, int size)
		{
			if (Hint.Unlikely(chain->m_Head == null))
			{
				EntityCommandBufferChain.InitChain(chain, this.m_Allocator);
			}
			if (Hint.Unlikely(sortKey < chain->m_LastSortKey))
			{
				EntityCommandBufferChain* ptr = (EntityCommandBufferChain*)Memory.Unmanaged.Allocate((long)sizeof(EntityCommandBufferChain), EntityCommandBufferData.ALIGN_64_BIT, this.m_Allocator);
				*ptr = *chain;
				EntityCommandBufferChain.InitChain(chain, this.m_Allocator);
				chain->m_NextChain = ptr;
			}
			chain->m_LastSortKey = sortKey;
			if (Hint.Unlikely(chain->m_Tail == null || chain->m_Tail->Capacity < size))
			{
				int num = math.max(this.m_MinimumChunkSize, size);
				ECBChunk* ptr2 = (ECBChunk*)Memory.Unmanaged.Allocate((long)(sizeof(ECBChunk) + num), 16, this.m_Allocator);
				ECBChunk* tail = chain->m_Tail;
				ptr2->Next = null;
				ptr2->Prev = tail;
				ptr2->Used = 0;
				ptr2->Size = num;
				if (tail != null)
				{
					tail->Next = ptr2;
				}
				if (chain->m_Head == null)
				{
					chain->m_Head = ptr2;
					Interlocked.Increment(ref this.m_RecordedChainCount);
				}
				chain->m_Tail = ptr2;
			}
			int num2 = chain->m_Tail->Bump(size);
			return (byte*)(chain->m_Tail + 1 + num2 / sizeof(ECBChunk));
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x000192A8 File Offset: 0x000174A8
		public unsafe DynamicBuffer<T> CreateBufferCommand<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ECBCommand commandType, EntityCommandBufferChain* chain, int sortKey, Entity e) where T : struct, ValueType, IBufferElementData
		{
			int num;
			return new DynamicBuffer<T>(this.AddEntityBufferCommand<T>(chain, sortKey, commandType, e, out num), num);
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x000192C8 File Offset: 0x000174C8
		public unsafe void AppendToBufferCommand<T>(EntityCommandBufferChain* chain, int sortKey, Entity e, T element) where T : struct, IBufferElementData
		{
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			short num = (short)UnsafeUtility.SizeOf<T>();
			int num2 = EntityCommandBufferData.Align(sizeof(EntityComponentCommand) + (int)num, EntityCommandBufferData.ALIGN_64_BIT);
			this.ResetCommandBatching(chain);
			EntityComponentCommand* ptr = (EntityComponentCommand*)this.Reserve(chain, sortKey, num2);
			ptr->Header.Header.CommandType = ECBCommand.AppendToBuffer;
			ptr->Header.Header.TotalSize = num2;
			ptr->Header.Header.SortKey = chain->m_LastSortKey;
			ptr->Header.Entity = e;
			ptr->Header.IdentityIndex = 0;
			ptr->Header.BatchCount = 1;
			ptr->ComponentTypeIndex = typeIndex;
			ptr->ComponentSize = num;
			byte* ptr2 = (byte*)(ptr + 1);
			UnsafeUtility.CopyStructureToPtr<T>(ref element, (void*)ptr2);
			ptr->ValueRequiresEntityFixup = (TypeManager.HasEntityReferences(typeIndex) ? 1 : 0);
		}

		// Token: 0x04000303 RID: 771
		public EntityCommandBufferChain m_MainThreadChain;

		// Token: 0x04000304 RID: 772
		public unsafe EntityCommandBufferChain* m_ThreadedChains;

		// Token: 0x04000305 RID: 773
		public int m_RecordedChainCount;

		// Token: 0x04000306 RID: 774
		public int m_MinimumChunkSize;

		// Token: 0x04000307 RID: 775
		public AllocatorManager.AllocatorHandle m_Allocator;

		// Token: 0x04000308 RID: 776
		public PlaybackPolicy m_PlaybackPolicy;

		// Token: 0x04000309 RID: 777
		public bool m_ShouldPlayback;

		// Token: 0x0400030A RID: 778
		public bool m_DidPlayback;

		// Token: 0x0400030B RID: 779
		public bool m_ForceFullDispose;

		// Token: 0x0400030C RID: 780
		public Entity m_Entity;

		// Token: 0x0400030D RID: 781
		public int m_BufferWithFixupsCount;

		// Token: 0x0400030E RID: 782
		public UnsafeAtomicCounter32 m_BufferWithFixups;

		// Token: 0x0400030F RID: 783
		internal static readonly int ALIGN_64_BIT = 8;

		// Token: 0x04000310 RID: 784
		public int m_CommandBufferID;
	}
}
