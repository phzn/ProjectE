using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x02000293 RID: 659
	[BurstCompile]
	internal static class StructuralChange
	{
		// Token: 0x0600116F RID: 4463 RVA: 0x000499BA File Offset: 0x00047BBA
		[BurstCompile]
		public unsafe static void AddComponentEntitiesBatch(EntityComponentStore* entityComponentStore, UnsafeList<EntityBatchInChunk>* entityBatchList, TypeIndex typeIndex)
		{
			StructuralChange.AddComponentEntitiesBatch_00000FAA$BurstDirectCall.Invoke(entityComponentStore, entityBatchList, typeIndex);
		}

		// Token: 0x06001170 RID: 4464 RVA: 0x000499C4 File Offset: 0x00047BC4
		[BurstCompile]
		public unsafe static void AddComponentsEntitiesBatch(EntityComponentStore* entityComponentStore, UnsafeList<EntityBatchInChunk>* entityBatchList, in ComponentTypeSet typeSet)
		{
			StructuralChange.AddComponentsEntitiesBatch_00000FAB$BurstDirectCall.Invoke(entityComponentStore, entityBatchList, in typeSet);
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x000499CE File Offset: 0x00047BCE
		[BurstCompile]
		public unsafe static bool AddComponentEntity(EntityComponentStore* entityComponentStore, Entity* entity, TypeIndex typeIndex)
		{
			return StructuralChange.AddComponentEntity_00000FAC$BurstDirectCall.Invoke(entityComponentStore, entity, typeIndex);
		}

		// Token: 0x06001172 RID: 4466 RVA: 0x000499D8 File Offset: 0x00047BD8
		[BurstCompile]
		public unsafe static void AddComponentsEntity(EntityComponentStore* entityComponentStore, Entity* entity, in ComponentTypeSet typeSet)
		{
			StructuralChange.AddComponentsEntity_00000FAD$BurstDirectCall.Invoke(entityComponentStore, entity, in typeSet);
		}

		// Token: 0x06001173 RID: 4467 RVA: 0x000499E2 File Offset: 0x00047BE2
		[BurstCompile]
		public unsafe static void AddComponentChunks(EntityComponentStore* entityComponentStore, ArchetypeChunk* chunks, int chunkCount, TypeIndex typeIndex)
		{
			StructuralChange.AddComponentChunks_00000FAE$BurstDirectCall.Invoke(entityComponentStore, chunks, chunkCount, typeIndex);
		}

		// Token: 0x06001174 RID: 4468 RVA: 0x000499ED File Offset: 0x00047BED
		[BurstCompile]
		public unsafe static void AddComponentQuery(EntityComponentStore* entityComponentStore, EntityQueryImpl* queryImpl, TypeIndex typeIndex)
		{
			StructuralChange.AddComponentQuery_00000FAF$BurstDirectCall.Invoke(entityComponentStore, queryImpl, typeIndex);
		}

		// Token: 0x06001175 RID: 4469 RVA: 0x000499F7 File Offset: 0x00047BF7
		[BurstCompile]
		public unsafe static void AddComponentsQuery(EntityComponentStore* entityComponentStore, EntityQueryImpl* queryImpl, in ComponentTypeSet typeSet)
		{
			StructuralChange.AddComponentsQuery_00000FB0$BurstDirectCall.Invoke(entityComponentStore, queryImpl, in typeSet);
		}

		// Token: 0x06001176 RID: 4470 RVA: 0x00049A01 File Offset: 0x00047C01
		[BurstCompile]
		public unsafe static bool RemoveComponentEntity(EntityComponentStore* entityComponentStore, Entity* entity, TypeIndex typeIndex)
		{
			return StructuralChange.RemoveComponentEntity_00000FB1$BurstDirectCall.Invoke(entityComponentStore, entity, typeIndex);
		}

		// Token: 0x06001177 RID: 4471 RVA: 0x00049A0B File Offset: 0x00047C0B
		[BurstCompile]
		public unsafe static void RemoveComponentsEntity(EntityComponentStore* entityComponentStore, Entity* entity, in ComponentTypeSet typeSet)
		{
			StructuralChange.RemoveComponentsEntity_00000FB2$BurstDirectCall.Invoke(entityComponentStore, entity, in typeSet);
		}

		// Token: 0x06001178 RID: 4472 RVA: 0x00049A15 File Offset: 0x00047C15
		[BurstCompile]
		public unsafe static void RemoveComponentEntitiesBatch(EntityComponentStore* entityComponentStore, UnsafeList<EntityBatchInChunk>* entityBatchList, TypeIndex typeIndex)
		{
			StructuralChange.RemoveComponentEntitiesBatch_00000FB3$BurstDirectCall.Invoke(entityComponentStore, entityBatchList, typeIndex);
		}

		// Token: 0x06001179 RID: 4473 RVA: 0x00049A1F File Offset: 0x00047C1F
		[BurstCompile]
		public unsafe static void RemoveComponentsEntitiesBatch(EntityComponentStore* entityComponentStore, UnsafeList<EntityBatchInChunk>* entityBatchList, in ComponentTypeSet typeSet)
		{
			StructuralChange.RemoveComponentsEntitiesBatch_00000FB4$BurstDirectCall.Invoke(entityComponentStore, entityBatchList, in typeSet);
		}

		// Token: 0x0600117A RID: 4474 RVA: 0x00049A29 File Offset: 0x00047C29
		[BurstCompile]
		public unsafe static void RemoveComponentChunks(EntityComponentStore* entityComponentStore, ArchetypeChunk* chunks, int chunkCount, TypeIndex typeIndex)
		{
			StructuralChange.RemoveComponentChunks_00000FB5$BurstDirectCall.Invoke(entityComponentStore, chunks, chunkCount, typeIndex);
		}

		// Token: 0x0600117B RID: 4475 RVA: 0x00049A34 File Offset: 0x00047C34
		[BurstCompile]
		public unsafe static void RemoveComponentQuery(EntityComponentStore* entityComponentStore, EntityQueryImpl* queryImpl, TypeIndex typeIndex)
		{
			StructuralChange.RemoveComponentQuery_00000FB6$BurstDirectCall.Invoke(entityComponentStore, queryImpl, typeIndex);
		}

		// Token: 0x0600117C RID: 4476 RVA: 0x00049A3E File Offset: 0x00047C3E
		[BurstCompile]
		public unsafe static void RemoveComponentsQuery(EntityComponentStore* entityComponentStore, EntityQueryImpl* queryImpl, in ComponentTypeSet typeSet)
		{
			StructuralChange.RemoveComponentsQuery_00000FB7$BurstDirectCall.Invoke(entityComponentStore, queryImpl, in typeSet);
		}

		// Token: 0x0600117D RID: 4477 RVA: 0x00049A48 File Offset: 0x00047C48
		[BurstCompile]
		public unsafe static void AddSharedComponentDataIndexWithBurst(EntityComponentStore* entityComponentStore, Entity* entities, int entityCount, in ComponentType componentType, int newSharedComponentDataIndex)
		{
			StructuralChange.AddSharedComponentDataIndexWithBurst_00000FB8$BurstDirectCall.Invoke(entityComponentStore, entities, entityCount, in componentType, newSharedComponentDataIndex);
		}

		// Token: 0x0600117E RID: 4478 RVA: 0x00049A55 File Offset: 0x00047C55
		[BurstCompile]
		public unsafe static void SetSharedComponentDataIndexWithBurst(EntityComponentStore* entityComponentStore, Entity* entities, int entityCount, in ComponentType componentType, int newSharedComponentDataIndex)
		{
			StructuralChange.SetSharedComponentDataIndexWithBurst_00000FB9$BurstDirectCall.Invoke(entityComponentStore, entities, entityCount, in componentType, newSharedComponentDataIndex);
		}

		// Token: 0x0600117F RID: 4479 RVA: 0x00049A62 File Offset: 0x00047C62
		[BurstCompile]
		public unsafe static void SetSharedComponentDataIndexWithBurst(EntityComponentStore* entityComponentStore, EntityQueryImpl* queryImpl, in ComponentType componentType, int newSharedComponentDataIndex)
		{
			StructuralChange.SetSharedComponentDataIndexWithBurst_00000FBA$BurstDirectCall.Invoke(entityComponentStore, queryImpl, in componentType, newSharedComponentDataIndex);
		}

		// Token: 0x06001180 RID: 4480 RVA: 0x00049A6D File Offset: 0x00047C6D
		[BurstCompile]
		public unsafe static void AddSharedComponentChunks(EntityComponentStore* entityComponentStore, ArchetypeChunk* chunks, int chunkCount, TypeIndex componentTypeIndex, int sharedComponentIndex)
		{
			StructuralChange.AddSharedComponentChunks_00000FBB$BurstDirectCall.Invoke(entityComponentStore, chunks, chunkCount, componentTypeIndex, sharedComponentIndex);
		}

		// Token: 0x06001181 RID: 4481 RVA: 0x00049A7A File Offset: 0x00047C7A
		[BurstCompile]
		public unsafe static void AddSharedComponentQuery(EntityComponentStore* entityComponentStore, EntityQueryImpl* queryImpl, TypeIndex componentTypeIndex, int sharedComponentIndex)
		{
			StructuralChange.AddSharedComponentQuery_00000FBC$BurstDirectCall.Invoke(entityComponentStore, queryImpl, componentTypeIndex, sharedComponentIndex);
		}

		// Token: 0x06001182 RID: 4482 RVA: 0x00049A85 File Offset: 0x00047C85
		[BurstCompile]
		public unsafe static void MoveEntityArchetype(EntityComponentStore* entityComponentStore, Entity* entity, void* dstArchetype)
		{
			StructuralChange.MoveEntityArchetype_00000FBD$BurstDirectCall.Invoke(entityComponentStore, entity, dstArchetype);
		}

		// Token: 0x06001183 RID: 4483 RVA: 0x00049A8F File Offset: 0x00047C8F
		[BurstCompile]
		public unsafe static void SetChunkComponent(EntityComponentStore* entityComponentStore, ArchetypeChunk* chunks, int chunkCount, void* componentData, TypeIndex componentTypeIndex)
		{
			StructuralChange.SetChunkComponent_00000FBE$BurstDirectCall.Invoke(entityComponentStore, chunks, chunkCount, componentData, componentTypeIndex);
		}

		// Token: 0x06001184 RID: 4484 RVA: 0x00049A9C File Offset: 0x00047C9C
		[BurstCompile]
		public unsafe static void CreateEntity(EntityComponentStore* entityComponentStore, void* archetype, Entity* outEntities, int count)
		{
			StructuralChange.CreateEntity_00000FBF$BurstDirectCall.Invoke(entityComponentStore, archetype, outEntities, count);
		}

		// Token: 0x06001185 RID: 4485 RVA: 0x00049AA7 File Offset: 0x00047CA7
		[BurstCompile]
		public unsafe static void DestroyEntity(EntityComponentStore* entityComponentStore, Entity* entities, int count)
		{
			StructuralChange.DestroyEntity_00000FC0$BurstDirectCall.Invoke(entityComponentStore, entities, count);
		}

		// Token: 0x06001186 RID: 4486 RVA: 0x00049AB1 File Offset: 0x00047CB1
		[BurstCompile]
		public unsafe static void DestroyChunksQuery(EntityComponentStore* entityComponentStore, EntityQueryImpl* queryImpl, ref BufferTypeHandle<LinkedEntityGroup> linkedEntityGroupTypeHandle)
		{
			StructuralChange.DestroyChunksQuery_00000FC1$BurstDirectCall.Invoke(entityComponentStore, queryImpl, ref linkedEntityGroupTypeHandle);
		}

		// Token: 0x06001187 RID: 4487 RVA: 0x00049ABB File Offset: 0x00047CBB
		[BurstCompile]
		public unsafe static void InstantiateEntity(EntityComponentStore* entityComponentStore, Entity* srcEntity, Entity* outputEntities, int instanceCount)
		{
			StructuralChange.InstantiateEntity_00000FC2$BurstDirectCall.Invoke(entityComponentStore, srcEntity, outputEntities, instanceCount);
		}

		// Token: 0x06001188 RID: 4488 RVA: 0x00049AC6 File Offset: 0x00047CC6
		[BurstCompile]
		public unsafe static void InstantiateEntities(EntityComponentStore* entityComponentStore, Entity* srcEntities, Entity* outputEntities, int entityCount, bool removePrefab)
		{
			StructuralChange.InstantiateEntities_00000FC3$BurstDirectCall.Invoke(entityComponentStore, srcEntities, outputEntities, entityCount, removePrefab);
		}

		// Token: 0x06001189 RID: 4489 RVA: 0x00049AD3 File Offset: 0x00047CD3
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void AddComponentEntitiesBatch$BurstManaged(EntityComponentStore* entityComponentStore, UnsafeList<EntityBatchInChunk>* entityBatchList, TypeIndex typeIndex)
		{
			entityComponentStore->AddComponent(entityBatchList, ComponentType.FromTypeIndex(typeIndex), 0);
		}

		// Token: 0x0600118A RID: 4490 RVA: 0x00049AE3 File Offset: 0x00047CE3
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void AddComponentsEntitiesBatch$BurstManaged(EntityComponentStore* entityComponentStore, UnsafeList<EntityBatchInChunk>* entityBatchList, in ComponentTypeSet typeSet)
		{
			entityComponentStore->AddComponents(entityBatchList, in typeSet);
		}

		// Token: 0x0600118B RID: 4491 RVA: 0x00049AED File Offset: 0x00047CED
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static bool AddComponentEntity$BurstManaged(EntityComponentStore* entityComponentStore, Entity* entity, TypeIndex typeIndex)
		{
			return entityComponentStore->AddComponent(*entity, ComponentType.FromTypeIndex(typeIndex));
		}

		// Token: 0x0600118C RID: 4492 RVA: 0x00049B01 File Offset: 0x00047D01
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void AddComponentsEntity$BurstManaged(EntityComponentStore* entityComponentStore, Entity* entity, in ComponentTypeSet typeSet)
		{
			entityComponentStore->AddComponent(*entity, in typeSet);
		}

		// Token: 0x0600118D RID: 4493 RVA: 0x00049B10 File Offset: 0x00047D10
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void AddComponentChunks$BurstManaged(EntityComponentStore* entityComponentStore, ArchetypeChunk* chunks, int chunkCount, TypeIndex typeIndex)
		{
			entityComponentStore->AddComponent(chunks, chunkCount, ComponentType.FromTypeIndex(typeIndex), 0);
		}

		// Token: 0x0600118E RID: 4494 RVA: 0x00049B21 File Offset: 0x00047D21
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void AddComponentQuery$BurstManaged(EntityComponentStore* entityComponentStore, EntityQueryImpl* queryImpl, TypeIndex typeIndex)
		{
			entityComponentStore->AddComponent(queryImpl, ComponentType.FromTypeIndex(typeIndex), 0);
		}

		// Token: 0x0600118F RID: 4495 RVA: 0x00049B31 File Offset: 0x00047D31
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void AddComponentsQuery$BurstManaged(EntityComponentStore* entityComponentStore, EntityQueryImpl* queryImpl, in ComponentTypeSet typeSet)
		{
			entityComponentStore->AddComponents(queryImpl, in typeSet);
		}

		// Token: 0x06001190 RID: 4496 RVA: 0x00049B3B File Offset: 0x00047D3B
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static bool RemoveComponentEntity$BurstManaged(EntityComponentStore* entityComponentStore, Entity* entity, TypeIndex typeIndex)
		{
			return entityComponentStore->RemoveComponent(*entity, ComponentType.FromTypeIndex(typeIndex));
		}

		// Token: 0x06001191 RID: 4497 RVA: 0x00049B4F File Offset: 0x00047D4F
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void RemoveComponentsEntity$BurstManaged(EntityComponentStore* entityComponentStore, Entity* entity, in ComponentTypeSet typeSet)
		{
			entityComponentStore->RemoveComponent(*entity, in typeSet);
		}

		// Token: 0x06001192 RID: 4498 RVA: 0x00049B5E File Offset: 0x00047D5E
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void RemoveComponentEntitiesBatch$BurstManaged(EntityComponentStore* entityComponentStore, UnsafeList<EntityBatchInChunk>* entityBatchList, TypeIndex typeIndex)
		{
			entityComponentStore->RemoveComponent(entityBatchList, ComponentType.FromTypeIndex(typeIndex));
		}

		// Token: 0x06001193 RID: 4499 RVA: 0x00049B6D File Offset: 0x00047D6D
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void RemoveComponentsEntitiesBatch$BurstManaged(EntityComponentStore* entityComponentStore, UnsafeList<EntityBatchInChunk>* entityBatchList, in ComponentTypeSet typeSet)
		{
			entityComponentStore->RemoveComponents(entityBatchList, in typeSet);
		}

		// Token: 0x06001194 RID: 4500 RVA: 0x00049B77 File Offset: 0x00047D77
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void RemoveComponentChunks$BurstManaged(EntityComponentStore* entityComponentStore, ArchetypeChunk* chunks, int chunkCount, TypeIndex typeIndex)
		{
			entityComponentStore->RemoveComponent(chunks, chunkCount, ComponentType.FromTypeIndex(typeIndex));
		}

		// Token: 0x06001195 RID: 4501 RVA: 0x00049B87 File Offset: 0x00047D87
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void RemoveComponentQuery$BurstManaged(EntityComponentStore* entityComponentStore, EntityQueryImpl* queryImpl, TypeIndex typeIndex)
		{
			entityComponentStore->RemoveComponent(queryImpl, ComponentType.FromTypeIndex(typeIndex));
		}

		// Token: 0x06001196 RID: 4502 RVA: 0x00049B96 File Offset: 0x00047D96
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void RemoveComponentsQuery$BurstManaged(EntityComponentStore* entityComponentStore, EntityQueryImpl* queryImpl, in ComponentTypeSet typeSet)
		{
			entityComponentStore->RemoveComponents(queryImpl, in typeSet);
		}

		// Token: 0x06001197 RID: 4503 RVA: 0x00049BA0 File Offset: 0x00047DA0
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void AddSharedComponentDataIndexWithBurst$BurstManaged(EntityComponentStore* entityComponentStore, Entity* entities, int entityCount, in ComponentType componentType, int newSharedComponentDataIndex)
		{
			for (int i = 0; i < entityCount; i++)
			{
				entityComponentStore->AddComponent(entities[i], componentType);
				entityComponentStore->SetSharedComponentDataIndex(entities[i], componentType, newSharedComponentDataIndex);
			}
		}

		// Token: 0x06001198 RID: 4504 RVA: 0x00049BF4 File Offset: 0x00047DF4
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void SetSharedComponentDataIndexWithBurst$BurstManaged(EntityComponentStore* entityComponentStore, Entity* entities, int entityCount, in ComponentType componentType, int newSharedComponentDataIndex)
		{
			for (int i = 0; i < entityCount; i++)
			{
				entityComponentStore->SetSharedComponentDataIndex(entities[i], componentType, newSharedComponentDataIndex);
			}
		}

		// Token: 0x06001199 RID: 4505 RVA: 0x00049C2B File Offset: 0x00047E2B
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void SetSharedComponentDataIndexWithBurst$BurstManaged(EntityComponentStore* entityComponentStore, EntityQueryImpl* queryImpl, in ComponentType componentType, int newSharedComponentDataIndex)
		{
			entityComponentStore->SetSharedComponentDataIndex(queryImpl, componentType, newSharedComponentDataIndex);
		}

		// Token: 0x0600119A RID: 4506 RVA: 0x00049C3B File Offset: 0x00047E3B
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void AddSharedComponentChunks$BurstManaged(EntityComponentStore* entityComponentStore, ArchetypeChunk* chunks, int chunkCount, TypeIndex componentTypeIndex, int sharedComponentIndex)
		{
			entityComponentStore->AddComponent(chunks, chunkCount, ComponentType.FromTypeIndex(componentTypeIndex), sharedComponentIndex);
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x00049C4D File Offset: 0x00047E4D
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void AddSharedComponentQuery$BurstManaged(EntityComponentStore* entityComponentStore, EntityQueryImpl* queryImpl, TypeIndex componentTypeIndex, int sharedComponentIndex)
		{
			entityComponentStore->AddComponent(queryImpl, ComponentType.FromTypeIndex(componentTypeIndex), sharedComponentIndex);
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x00049C5D File Offset: 0x00047E5D
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void MoveEntityArchetype$BurstManaged(EntityComponentStore* entityComponentStore, Entity* entity, void* dstArchetype)
		{
			entityComponentStore->Move(*entity, (Archetype*)dstArchetype);
		}

		// Token: 0x0600119D RID: 4509 RVA: 0x00049C6C File Offset: 0x00047E6C
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void SetChunkComponent$BurstManaged(EntityComponentStore* entityComponentStore, ArchetypeChunk* chunks, int chunkCount, void* componentData, TypeIndex componentTypeIndex)
		{
			entityComponentStore->SetChunkComponent(chunks, chunkCount, componentData, componentTypeIndex);
		}

		// Token: 0x0600119E RID: 4510 RVA: 0x00049C79 File Offset: 0x00047E79
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void CreateEntity$BurstManaged(EntityComponentStore* entityComponentStore, void* archetype, Entity* outEntities, int count)
		{
			entityComponentStore->CreateEntities((Archetype*)archetype, outEntities, count);
		}

		// Token: 0x0600119F RID: 4511 RVA: 0x00049C84 File Offset: 0x00047E84
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void DestroyEntity$BurstManaged(EntityComponentStore* entityComponentStore, Entity* entities, int count)
		{
			entityComponentStore->DestroyEntities(entities, count);
		}

		// Token: 0x060011A0 RID: 4512 RVA: 0x00049C8E File Offset: 0x00047E8E
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void DestroyChunksQuery$BurstManaged(EntityComponentStore* entityComponentStore, EntityQueryImpl* queryImpl, ref BufferTypeHandle<LinkedEntityGroup> linkedEntityGroupTypeHandle)
		{
			entityComponentStore->DestroyEntities(queryImpl, ref linkedEntityGroupTypeHandle);
		}

		// Token: 0x060011A1 RID: 4513 RVA: 0x00049C98 File Offset: 0x00047E98
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void InstantiateEntity$BurstManaged(EntityComponentStore* entityComponentStore, Entity* srcEntity, Entity* outputEntities, int instanceCount)
		{
			entityComponentStore->InstantiateEntities(*srcEntity, outputEntities, instanceCount);
		}

		// Token: 0x060011A2 RID: 4514 RVA: 0x00049CA8 File Offset: 0x00047EA8
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void InstantiateEntities$BurstManaged(EntityComponentStore* entityComponentStore, Entity* srcEntities, Entity* outputEntities, int entityCount, bool removePrefab)
		{
			entityComponentStore->InstantiateEntities(srcEntities, outputEntities, entityCount, removePrefab);
		}

		// Token: 0x02000294 RID: 660
		// (Invoke) Token: 0x060011A4 RID: 4516
		internal unsafe delegate void AddComponentEntitiesBatch_00000FAA$PostfixBurstDelegate(EntityComponentStore* entityComponentStore, UnsafeList<EntityBatchInChunk>* entityBatchList, TypeIndex typeIndex);

		// Token: 0x02000295 RID: 661
		internal static class AddComponentEntitiesBatch_00000FAA$BurstDirectCall
		{
			// Token: 0x060011A7 RID: 4519 RVA: 0x00049CB5 File Offset: 0x00047EB5
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (StructuralChange.AddComponentEntitiesBatch_00000FAA$BurstDirectCall.Pointer == 0)
				{
					StructuralChange.AddComponentEntitiesBatch_00000FAA$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(StructuralChange.AddComponentEntitiesBatch_00000FAA$BurstDirectCall.DeferredCompilation, methodof(StructuralChange.AddComponentEntitiesBatch$BurstManaged(EntityComponentStore*, UnsafeList<EntityBatchInChunk>*, TypeIndex)).MethodHandle, typeof(StructuralChange.AddComponentEntitiesBatch_00000FAA$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = StructuralChange.AddComponentEntitiesBatch_00000FAA$BurstDirectCall.Pointer;
			}

			// Token: 0x060011A8 RID: 4520 RVA: 0x00049CE4 File Offset: 0x00047EE4
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				StructuralChange.AddComponentEntitiesBatch_00000FAA$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x060011A9 RID: 4521 RVA: 0x00049CFC File Offset: 0x00047EFC
			public unsafe static void Constructor()
			{
				StructuralChange.AddComponentEntitiesBatch_00000FAA$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(StructuralChange.AddComponentEntitiesBatch(EntityComponentStore*, UnsafeList<EntityBatchInChunk>*, TypeIndex)).MethodHandle);
			}

			// Token: 0x060011AA RID: 4522 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x060011AB RID: 4523 RVA: 0x00049D0D File Offset: 0x00047F0D
			// Note: this type is marked as 'beforefieldinit'.
			static AddComponentEntitiesBatch_00000FAA$BurstDirectCall()
			{
				StructuralChange.AddComponentEntitiesBatch_00000FAA$BurstDirectCall.Constructor();
			}

			// Token: 0x060011AC RID: 4524 RVA: 0x00049D14 File Offset: 0x00047F14
			public unsafe static void Invoke(EntityComponentStore* entityComponentStore, UnsafeList<EntityBatchInChunk>* entityBatchList, TypeIndex typeIndex)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = StructuralChange.AddComponentEntitiesBatch_00000FAA$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.EntityComponentStore*,Unity.Collections.LowLevel.Unsafe.UnsafeList`1<Unity.Entities.EntityBatchInChunk>*,Unity.Entities.TypeIndex), entityComponentStore, entityBatchList, typeIndex, functionPointer);
						return;
					}
				}
				StructuralChange.AddComponentEntitiesBatch$BurstManaged(entityComponentStore, entityBatchList, typeIndex);
			}

			// Token: 0x040006B2 RID: 1714
			private static IntPtr Pointer;

			// Token: 0x040006B3 RID: 1715
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000296 RID: 662
		// (Invoke) Token: 0x060011AE RID: 4526
		internal unsafe delegate void AddComponentsEntitiesBatch_00000FAB$PostfixBurstDelegate(EntityComponentStore* entityComponentStore, UnsafeList<EntityBatchInChunk>* entityBatchList, in ComponentTypeSet typeSet);

		// Token: 0x02000297 RID: 663
		internal static class AddComponentsEntitiesBatch_00000FAB$BurstDirectCall
		{
			// Token: 0x060011B1 RID: 4529 RVA: 0x00049D49 File Offset: 0x00047F49
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (StructuralChange.AddComponentsEntitiesBatch_00000FAB$BurstDirectCall.Pointer == 0)
				{
					StructuralChange.AddComponentsEntitiesBatch_00000FAB$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(StructuralChange.AddComponentsEntitiesBatch_00000FAB$BurstDirectCall.DeferredCompilation, methodof(StructuralChange.AddComponentsEntitiesBatch$BurstManaged(EntityComponentStore*, UnsafeList<EntityBatchInChunk>*, ref ComponentTypeSet)).MethodHandle, typeof(StructuralChange.AddComponentsEntitiesBatch_00000FAB$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = StructuralChange.AddComponentsEntitiesBatch_00000FAB$BurstDirectCall.Pointer;
			}

			// Token: 0x060011B2 RID: 4530 RVA: 0x00049D78 File Offset: 0x00047F78
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				StructuralChange.AddComponentsEntitiesBatch_00000FAB$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x060011B3 RID: 4531 RVA: 0x00049D90 File Offset: 0x00047F90
			public unsafe static void Constructor()
			{
				StructuralChange.AddComponentsEntitiesBatch_00000FAB$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(StructuralChange.AddComponentsEntitiesBatch(EntityComponentStore*, UnsafeList<EntityBatchInChunk>*, ref ComponentTypeSet)).MethodHandle);
			}

			// Token: 0x060011B4 RID: 4532 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x060011B5 RID: 4533 RVA: 0x00049DA1 File Offset: 0x00047FA1
			// Note: this type is marked as 'beforefieldinit'.
			static AddComponentsEntitiesBatch_00000FAB$BurstDirectCall()
			{
				StructuralChange.AddComponentsEntitiesBatch_00000FAB$BurstDirectCall.Constructor();
			}

			// Token: 0x060011B6 RID: 4534 RVA: 0x00049DA8 File Offset: 0x00047FA8
			public unsafe static void Invoke(EntityComponentStore* entityComponentStore, UnsafeList<EntityBatchInChunk>* entityBatchList, in ComponentTypeSet typeSet)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = StructuralChange.AddComponentsEntitiesBatch_00000FAB$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.EntityComponentStore*,Unity.Collections.LowLevel.Unsafe.UnsafeList`1<Unity.Entities.EntityBatchInChunk>*,Unity.Entities.ComponentTypeSet&), entityComponentStore, entityBatchList, ref typeSet, functionPointer);
						return;
					}
				}
				StructuralChange.AddComponentsEntitiesBatch$BurstManaged(entityComponentStore, entityBatchList, in typeSet);
			}

			// Token: 0x040006B4 RID: 1716
			private static IntPtr Pointer;

			// Token: 0x040006B5 RID: 1717
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000298 RID: 664
		// (Invoke) Token: 0x060011B8 RID: 4536
		internal unsafe delegate bool AddComponentEntity_00000FAC$PostfixBurstDelegate(EntityComponentStore* entityComponentStore, Entity* entity, TypeIndex typeIndex);

		// Token: 0x02000299 RID: 665
		internal static class AddComponentEntity_00000FAC$BurstDirectCall
		{
			// Token: 0x060011BB RID: 4539 RVA: 0x00049DDD File Offset: 0x00047FDD
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (StructuralChange.AddComponentEntity_00000FAC$BurstDirectCall.Pointer == 0)
				{
					StructuralChange.AddComponentEntity_00000FAC$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(StructuralChange.AddComponentEntity_00000FAC$BurstDirectCall.DeferredCompilation, methodof(StructuralChange.AddComponentEntity$BurstManaged(EntityComponentStore*, Entity*, TypeIndex)).MethodHandle, typeof(StructuralChange.AddComponentEntity_00000FAC$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = StructuralChange.AddComponentEntity_00000FAC$BurstDirectCall.Pointer;
			}

			// Token: 0x060011BC RID: 4540 RVA: 0x00049E0C File Offset: 0x0004800C
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				StructuralChange.AddComponentEntity_00000FAC$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x060011BD RID: 4541 RVA: 0x00049E24 File Offset: 0x00048024
			public unsafe static void Constructor()
			{
				StructuralChange.AddComponentEntity_00000FAC$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(StructuralChange.AddComponentEntity(EntityComponentStore*, Entity*, TypeIndex)).MethodHandle);
			}

			// Token: 0x060011BE RID: 4542 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x060011BF RID: 4543 RVA: 0x00049E35 File Offset: 0x00048035
			// Note: this type is marked as 'beforefieldinit'.
			static AddComponentEntity_00000FAC$BurstDirectCall()
			{
				StructuralChange.AddComponentEntity_00000FAC$BurstDirectCall.Constructor();
			}

			// Token: 0x060011C0 RID: 4544 RVA: 0x00049E3C File Offset: 0x0004803C
			public unsafe static bool Invoke(EntityComponentStore* entityComponentStore, Entity* entity, TypeIndex typeIndex)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = StructuralChange.AddComponentEntity_00000FAC$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Boolean(Unity.Entities.EntityComponentStore*,Unity.Entities.Entity*,Unity.Entities.TypeIndex), entityComponentStore, entity, typeIndex, functionPointer);
					}
				}
				return StructuralChange.AddComponentEntity$BurstManaged(entityComponentStore, entity, typeIndex);
			}

			// Token: 0x040006B6 RID: 1718
			private static IntPtr Pointer;

			// Token: 0x040006B7 RID: 1719
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x0200029A RID: 666
		// (Invoke) Token: 0x060011C2 RID: 4546
		internal unsafe delegate void AddComponentsEntity_00000FAD$PostfixBurstDelegate(EntityComponentStore* entityComponentStore, Entity* entity, in ComponentTypeSet typeSet);

		// Token: 0x0200029B RID: 667
		internal static class AddComponentsEntity_00000FAD$BurstDirectCall
		{
			// Token: 0x060011C5 RID: 4549 RVA: 0x00049E71 File Offset: 0x00048071
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (StructuralChange.AddComponentsEntity_00000FAD$BurstDirectCall.Pointer == 0)
				{
					StructuralChange.AddComponentsEntity_00000FAD$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(StructuralChange.AddComponentsEntity_00000FAD$BurstDirectCall.DeferredCompilation, methodof(StructuralChange.AddComponentsEntity$BurstManaged(EntityComponentStore*, Entity*, ref ComponentTypeSet)).MethodHandle, typeof(StructuralChange.AddComponentsEntity_00000FAD$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = StructuralChange.AddComponentsEntity_00000FAD$BurstDirectCall.Pointer;
			}

			// Token: 0x060011C6 RID: 4550 RVA: 0x00049EA0 File Offset: 0x000480A0
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				StructuralChange.AddComponentsEntity_00000FAD$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x060011C7 RID: 4551 RVA: 0x00049EB8 File Offset: 0x000480B8
			public unsafe static void Constructor()
			{
				StructuralChange.AddComponentsEntity_00000FAD$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(StructuralChange.AddComponentsEntity(EntityComponentStore*, Entity*, ref ComponentTypeSet)).MethodHandle);
			}

			// Token: 0x060011C8 RID: 4552 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x060011C9 RID: 4553 RVA: 0x00049EC9 File Offset: 0x000480C9
			// Note: this type is marked as 'beforefieldinit'.
			static AddComponentsEntity_00000FAD$BurstDirectCall()
			{
				StructuralChange.AddComponentsEntity_00000FAD$BurstDirectCall.Constructor();
			}

			// Token: 0x060011CA RID: 4554 RVA: 0x00049ED0 File Offset: 0x000480D0
			public unsafe static void Invoke(EntityComponentStore* entityComponentStore, Entity* entity, in ComponentTypeSet typeSet)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = StructuralChange.AddComponentsEntity_00000FAD$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.EntityComponentStore*,Unity.Entities.Entity*,Unity.Entities.ComponentTypeSet&), entityComponentStore, entity, ref typeSet, functionPointer);
						return;
					}
				}
				StructuralChange.AddComponentsEntity$BurstManaged(entityComponentStore, entity, in typeSet);
			}

			// Token: 0x040006B8 RID: 1720
			private static IntPtr Pointer;

			// Token: 0x040006B9 RID: 1721
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x0200029C RID: 668
		// (Invoke) Token: 0x060011CC RID: 4556
		internal unsafe delegate void AddComponentChunks_00000FAE$PostfixBurstDelegate(EntityComponentStore* entityComponentStore, ArchetypeChunk* chunks, int chunkCount, TypeIndex typeIndex);

		// Token: 0x0200029D RID: 669
		internal static class AddComponentChunks_00000FAE$BurstDirectCall
		{
			// Token: 0x060011CF RID: 4559 RVA: 0x00049F05 File Offset: 0x00048105
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (StructuralChange.AddComponentChunks_00000FAE$BurstDirectCall.Pointer == 0)
				{
					StructuralChange.AddComponentChunks_00000FAE$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(StructuralChange.AddComponentChunks_00000FAE$BurstDirectCall.DeferredCompilation, methodof(StructuralChange.AddComponentChunks$BurstManaged(EntityComponentStore*, ArchetypeChunk*, int, TypeIndex)).MethodHandle, typeof(StructuralChange.AddComponentChunks_00000FAE$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = StructuralChange.AddComponentChunks_00000FAE$BurstDirectCall.Pointer;
			}

			// Token: 0x060011D0 RID: 4560 RVA: 0x00049F34 File Offset: 0x00048134
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				StructuralChange.AddComponentChunks_00000FAE$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x060011D1 RID: 4561 RVA: 0x00049F4C File Offset: 0x0004814C
			public unsafe static void Constructor()
			{
				StructuralChange.AddComponentChunks_00000FAE$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(StructuralChange.AddComponentChunks(EntityComponentStore*, ArchetypeChunk*, int, TypeIndex)).MethodHandle);
			}

			// Token: 0x060011D2 RID: 4562 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x060011D3 RID: 4563 RVA: 0x00049F5D File Offset: 0x0004815D
			// Note: this type is marked as 'beforefieldinit'.
			static AddComponentChunks_00000FAE$BurstDirectCall()
			{
				StructuralChange.AddComponentChunks_00000FAE$BurstDirectCall.Constructor();
			}

			// Token: 0x060011D4 RID: 4564 RVA: 0x00049F64 File Offset: 0x00048164
			public unsafe static void Invoke(EntityComponentStore* entityComponentStore, ArchetypeChunk* chunks, int chunkCount, TypeIndex typeIndex)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = StructuralChange.AddComponentChunks_00000FAE$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.EntityComponentStore*,Unity.Entities.ArchetypeChunk*,System.Int32,Unity.Entities.TypeIndex), entityComponentStore, chunks, chunkCount, typeIndex, functionPointer);
						return;
					}
				}
				StructuralChange.AddComponentChunks$BurstManaged(entityComponentStore, chunks, chunkCount, typeIndex);
			}

			// Token: 0x040006BA RID: 1722
			private static IntPtr Pointer;

			// Token: 0x040006BB RID: 1723
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x0200029E RID: 670
		// (Invoke) Token: 0x060011D6 RID: 4566
		internal unsafe delegate void AddComponentQuery_00000FAF$PostfixBurstDelegate(EntityComponentStore* entityComponentStore, EntityQueryImpl* queryImpl, TypeIndex typeIndex);

		// Token: 0x0200029F RID: 671
		internal static class AddComponentQuery_00000FAF$BurstDirectCall
		{
			// Token: 0x060011D9 RID: 4569 RVA: 0x00049F9B File Offset: 0x0004819B
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (StructuralChange.AddComponentQuery_00000FAF$BurstDirectCall.Pointer == 0)
				{
					StructuralChange.AddComponentQuery_00000FAF$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(StructuralChange.AddComponentQuery_00000FAF$BurstDirectCall.DeferredCompilation, methodof(StructuralChange.AddComponentQuery$BurstManaged(EntityComponentStore*, EntityQueryImpl*, TypeIndex)).MethodHandle, typeof(StructuralChange.AddComponentQuery_00000FAF$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = StructuralChange.AddComponentQuery_00000FAF$BurstDirectCall.Pointer;
			}

			// Token: 0x060011DA RID: 4570 RVA: 0x00049FC8 File Offset: 0x000481C8
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				StructuralChange.AddComponentQuery_00000FAF$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x060011DB RID: 4571 RVA: 0x00049FE0 File Offset: 0x000481E0
			public unsafe static void Constructor()
			{
				StructuralChange.AddComponentQuery_00000FAF$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(StructuralChange.AddComponentQuery(EntityComponentStore*, EntityQueryImpl*, TypeIndex)).MethodHandle);
			}

			// Token: 0x060011DC RID: 4572 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x060011DD RID: 4573 RVA: 0x00049FF1 File Offset: 0x000481F1
			// Note: this type is marked as 'beforefieldinit'.
			static AddComponentQuery_00000FAF$BurstDirectCall()
			{
				StructuralChange.AddComponentQuery_00000FAF$BurstDirectCall.Constructor();
			}

			// Token: 0x060011DE RID: 4574 RVA: 0x00049FF8 File Offset: 0x000481F8
			public unsafe static void Invoke(EntityComponentStore* entityComponentStore, EntityQueryImpl* queryImpl, TypeIndex typeIndex)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = StructuralChange.AddComponentQuery_00000FAF$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.EntityComponentStore*,Unity.Entities.EntityQueryImpl*,Unity.Entities.TypeIndex), entityComponentStore, queryImpl, typeIndex, functionPointer);
						return;
					}
				}
				StructuralChange.AddComponentQuery$BurstManaged(entityComponentStore, queryImpl, typeIndex);
			}

			// Token: 0x040006BC RID: 1724
			private static IntPtr Pointer;

			// Token: 0x040006BD RID: 1725
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020002A0 RID: 672
		// (Invoke) Token: 0x060011E0 RID: 4576
		internal unsafe delegate void AddComponentsQuery_00000FB0$PostfixBurstDelegate(EntityComponentStore* entityComponentStore, EntityQueryImpl* queryImpl, in ComponentTypeSet typeSet);

		// Token: 0x020002A1 RID: 673
		internal static class AddComponentsQuery_00000FB0$BurstDirectCall
		{
			// Token: 0x060011E3 RID: 4579 RVA: 0x0004A02D File Offset: 0x0004822D
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (StructuralChange.AddComponentsQuery_00000FB0$BurstDirectCall.Pointer == 0)
				{
					StructuralChange.AddComponentsQuery_00000FB0$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(StructuralChange.AddComponentsQuery_00000FB0$BurstDirectCall.DeferredCompilation, methodof(StructuralChange.AddComponentsQuery$BurstManaged(EntityComponentStore*, EntityQueryImpl*, ref ComponentTypeSet)).MethodHandle, typeof(StructuralChange.AddComponentsQuery_00000FB0$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = StructuralChange.AddComponentsQuery_00000FB0$BurstDirectCall.Pointer;
			}

			// Token: 0x060011E4 RID: 4580 RVA: 0x0004A05C File Offset: 0x0004825C
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				StructuralChange.AddComponentsQuery_00000FB0$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x060011E5 RID: 4581 RVA: 0x0004A074 File Offset: 0x00048274
			public unsafe static void Constructor()
			{
				StructuralChange.AddComponentsQuery_00000FB0$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(StructuralChange.AddComponentsQuery(EntityComponentStore*, EntityQueryImpl*, ref ComponentTypeSet)).MethodHandle);
			}

			// Token: 0x060011E6 RID: 4582 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x060011E7 RID: 4583 RVA: 0x0004A085 File Offset: 0x00048285
			// Note: this type is marked as 'beforefieldinit'.
			static AddComponentsQuery_00000FB0$BurstDirectCall()
			{
				StructuralChange.AddComponentsQuery_00000FB0$BurstDirectCall.Constructor();
			}

			// Token: 0x060011E8 RID: 4584 RVA: 0x0004A08C File Offset: 0x0004828C
			public unsafe static void Invoke(EntityComponentStore* entityComponentStore, EntityQueryImpl* queryImpl, in ComponentTypeSet typeSet)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = StructuralChange.AddComponentsQuery_00000FB0$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.EntityComponentStore*,Unity.Entities.EntityQueryImpl*,Unity.Entities.ComponentTypeSet&), entityComponentStore, queryImpl, ref typeSet, functionPointer);
						return;
					}
				}
				StructuralChange.AddComponentsQuery$BurstManaged(entityComponentStore, queryImpl, in typeSet);
			}

			// Token: 0x040006BE RID: 1726
			private static IntPtr Pointer;

			// Token: 0x040006BF RID: 1727
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020002A2 RID: 674
		// (Invoke) Token: 0x060011EA RID: 4586
		internal unsafe delegate bool RemoveComponentEntity_00000FB1$PostfixBurstDelegate(EntityComponentStore* entityComponentStore, Entity* entity, TypeIndex typeIndex);

		// Token: 0x020002A3 RID: 675
		internal static class RemoveComponentEntity_00000FB1$BurstDirectCall
		{
			// Token: 0x060011ED RID: 4589 RVA: 0x0004A0C1 File Offset: 0x000482C1
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (StructuralChange.RemoveComponentEntity_00000FB1$BurstDirectCall.Pointer == 0)
				{
					StructuralChange.RemoveComponentEntity_00000FB1$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(StructuralChange.RemoveComponentEntity_00000FB1$BurstDirectCall.DeferredCompilation, methodof(StructuralChange.RemoveComponentEntity$BurstManaged(EntityComponentStore*, Entity*, TypeIndex)).MethodHandle, typeof(StructuralChange.RemoveComponentEntity_00000FB1$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = StructuralChange.RemoveComponentEntity_00000FB1$BurstDirectCall.Pointer;
			}

			// Token: 0x060011EE RID: 4590 RVA: 0x0004A0F0 File Offset: 0x000482F0
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				StructuralChange.RemoveComponentEntity_00000FB1$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x060011EF RID: 4591 RVA: 0x0004A108 File Offset: 0x00048308
			public unsafe static void Constructor()
			{
				StructuralChange.RemoveComponentEntity_00000FB1$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(StructuralChange.RemoveComponentEntity(EntityComponentStore*, Entity*, TypeIndex)).MethodHandle);
			}

			// Token: 0x060011F0 RID: 4592 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x060011F1 RID: 4593 RVA: 0x0004A119 File Offset: 0x00048319
			// Note: this type is marked as 'beforefieldinit'.
			static RemoveComponentEntity_00000FB1$BurstDirectCall()
			{
				StructuralChange.RemoveComponentEntity_00000FB1$BurstDirectCall.Constructor();
			}

			// Token: 0x060011F2 RID: 4594 RVA: 0x0004A120 File Offset: 0x00048320
			public unsafe static bool Invoke(EntityComponentStore* entityComponentStore, Entity* entity, TypeIndex typeIndex)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = StructuralChange.RemoveComponentEntity_00000FB1$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Boolean(Unity.Entities.EntityComponentStore*,Unity.Entities.Entity*,Unity.Entities.TypeIndex), entityComponentStore, entity, typeIndex, functionPointer);
					}
				}
				return StructuralChange.RemoveComponentEntity$BurstManaged(entityComponentStore, entity, typeIndex);
			}

			// Token: 0x040006C0 RID: 1728
			private static IntPtr Pointer;

			// Token: 0x040006C1 RID: 1729
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020002A4 RID: 676
		// (Invoke) Token: 0x060011F4 RID: 4596
		internal unsafe delegate void RemoveComponentsEntity_00000FB2$PostfixBurstDelegate(EntityComponentStore* entityComponentStore, Entity* entity, in ComponentTypeSet typeSet);

		// Token: 0x020002A5 RID: 677
		internal static class RemoveComponentsEntity_00000FB2$BurstDirectCall
		{
			// Token: 0x060011F7 RID: 4599 RVA: 0x0004A155 File Offset: 0x00048355
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (StructuralChange.RemoveComponentsEntity_00000FB2$BurstDirectCall.Pointer == 0)
				{
					StructuralChange.RemoveComponentsEntity_00000FB2$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(StructuralChange.RemoveComponentsEntity_00000FB2$BurstDirectCall.DeferredCompilation, methodof(StructuralChange.RemoveComponentsEntity$BurstManaged(EntityComponentStore*, Entity*, ref ComponentTypeSet)).MethodHandle, typeof(StructuralChange.RemoveComponentsEntity_00000FB2$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = StructuralChange.RemoveComponentsEntity_00000FB2$BurstDirectCall.Pointer;
			}

			// Token: 0x060011F8 RID: 4600 RVA: 0x0004A184 File Offset: 0x00048384
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				StructuralChange.RemoveComponentsEntity_00000FB2$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x060011F9 RID: 4601 RVA: 0x0004A19C File Offset: 0x0004839C
			public unsafe static void Constructor()
			{
				StructuralChange.RemoveComponentsEntity_00000FB2$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(StructuralChange.RemoveComponentsEntity(EntityComponentStore*, Entity*, ref ComponentTypeSet)).MethodHandle);
			}

			// Token: 0x060011FA RID: 4602 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x060011FB RID: 4603 RVA: 0x0004A1AD File Offset: 0x000483AD
			// Note: this type is marked as 'beforefieldinit'.
			static RemoveComponentsEntity_00000FB2$BurstDirectCall()
			{
				StructuralChange.RemoveComponentsEntity_00000FB2$BurstDirectCall.Constructor();
			}

			// Token: 0x060011FC RID: 4604 RVA: 0x0004A1B4 File Offset: 0x000483B4
			public unsafe static void Invoke(EntityComponentStore* entityComponentStore, Entity* entity, in ComponentTypeSet typeSet)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = StructuralChange.RemoveComponentsEntity_00000FB2$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.EntityComponentStore*,Unity.Entities.Entity*,Unity.Entities.ComponentTypeSet&), entityComponentStore, entity, ref typeSet, functionPointer);
						return;
					}
				}
				StructuralChange.RemoveComponentsEntity$BurstManaged(entityComponentStore, entity, in typeSet);
			}

			// Token: 0x040006C2 RID: 1730
			private static IntPtr Pointer;

			// Token: 0x040006C3 RID: 1731
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020002A6 RID: 678
		// (Invoke) Token: 0x060011FE RID: 4606
		internal unsafe delegate void RemoveComponentEntitiesBatch_00000FB3$PostfixBurstDelegate(EntityComponentStore* entityComponentStore, UnsafeList<EntityBatchInChunk>* entityBatchList, TypeIndex typeIndex);

		// Token: 0x020002A7 RID: 679
		internal static class RemoveComponentEntitiesBatch_00000FB3$BurstDirectCall
		{
			// Token: 0x06001201 RID: 4609 RVA: 0x0004A1E9 File Offset: 0x000483E9
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (StructuralChange.RemoveComponentEntitiesBatch_00000FB3$BurstDirectCall.Pointer == 0)
				{
					StructuralChange.RemoveComponentEntitiesBatch_00000FB3$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(StructuralChange.RemoveComponentEntitiesBatch_00000FB3$BurstDirectCall.DeferredCompilation, methodof(StructuralChange.RemoveComponentEntitiesBatch$BurstManaged(EntityComponentStore*, UnsafeList<EntityBatchInChunk>*, TypeIndex)).MethodHandle, typeof(StructuralChange.RemoveComponentEntitiesBatch_00000FB3$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = StructuralChange.RemoveComponentEntitiesBatch_00000FB3$BurstDirectCall.Pointer;
			}

			// Token: 0x06001202 RID: 4610 RVA: 0x0004A218 File Offset: 0x00048418
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				StructuralChange.RemoveComponentEntitiesBatch_00000FB3$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06001203 RID: 4611 RVA: 0x0004A230 File Offset: 0x00048430
			public unsafe static void Constructor()
			{
				StructuralChange.RemoveComponentEntitiesBatch_00000FB3$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(StructuralChange.RemoveComponentEntitiesBatch(EntityComponentStore*, UnsafeList<EntityBatchInChunk>*, TypeIndex)).MethodHandle);
			}

			// Token: 0x06001204 RID: 4612 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06001205 RID: 4613 RVA: 0x0004A241 File Offset: 0x00048441
			// Note: this type is marked as 'beforefieldinit'.
			static RemoveComponentEntitiesBatch_00000FB3$BurstDirectCall()
			{
				StructuralChange.RemoveComponentEntitiesBatch_00000FB3$BurstDirectCall.Constructor();
			}

			// Token: 0x06001206 RID: 4614 RVA: 0x0004A248 File Offset: 0x00048448
			public unsafe static void Invoke(EntityComponentStore* entityComponentStore, UnsafeList<EntityBatchInChunk>* entityBatchList, TypeIndex typeIndex)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = StructuralChange.RemoveComponentEntitiesBatch_00000FB3$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.EntityComponentStore*,Unity.Collections.LowLevel.Unsafe.UnsafeList`1<Unity.Entities.EntityBatchInChunk>*,Unity.Entities.TypeIndex), entityComponentStore, entityBatchList, typeIndex, functionPointer);
						return;
					}
				}
				StructuralChange.RemoveComponentEntitiesBatch$BurstManaged(entityComponentStore, entityBatchList, typeIndex);
			}

			// Token: 0x040006C4 RID: 1732
			private static IntPtr Pointer;

			// Token: 0x040006C5 RID: 1733
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020002A8 RID: 680
		// (Invoke) Token: 0x06001208 RID: 4616
		internal unsafe delegate void RemoveComponentsEntitiesBatch_00000FB4$PostfixBurstDelegate(EntityComponentStore* entityComponentStore, UnsafeList<EntityBatchInChunk>* entityBatchList, in ComponentTypeSet typeSet);

		// Token: 0x020002A9 RID: 681
		internal static class RemoveComponentsEntitiesBatch_00000FB4$BurstDirectCall
		{
			// Token: 0x0600120B RID: 4619 RVA: 0x0004A27D File Offset: 0x0004847D
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (StructuralChange.RemoveComponentsEntitiesBatch_00000FB4$BurstDirectCall.Pointer == 0)
				{
					StructuralChange.RemoveComponentsEntitiesBatch_00000FB4$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(StructuralChange.RemoveComponentsEntitiesBatch_00000FB4$BurstDirectCall.DeferredCompilation, methodof(StructuralChange.RemoveComponentsEntitiesBatch$BurstManaged(EntityComponentStore*, UnsafeList<EntityBatchInChunk>*, ref ComponentTypeSet)).MethodHandle, typeof(StructuralChange.RemoveComponentsEntitiesBatch_00000FB4$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = StructuralChange.RemoveComponentsEntitiesBatch_00000FB4$BurstDirectCall.Pointer;
			}

			// Token: 0x0600120C RID: 4620 RVA: 0x0004A2AC File Offset: 0x000484AC
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				StructuralChange.RemoveComponentsEntitiesBatch_00000FB4$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600120D RID: 4621 RVA: 0x0004A2C4 File Offset: 0x000484C4
			public unsafe static void Constructor()
			{
				StructuralChange.RemoveComponentsEntitiesBatch_00000FB4$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(StructuralChange.RemoveComponentsEntitiesBatch(EntityComponentStore*, UnsafeList<EntityBatchInChunk>*, ref ComponentTypeSet)).MethodHandle);
			}

			// Token: 0x0600120E RID: 4622 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x0600120F RID: 4623 RVA: 0x0004A2D5 File Offset: 0x000484D5
			// Note: this type is marked as 'beforefieldinit'.
			static RemoveComponentsEntitiesBatch_00000FB4$BurstDirectCall()
			{
				StructuralChange.RemoveComponentsEntitiesBatch_00000FB4$BurstDirectCall.Constructor();
			}

			// Token: 0x06001210 RID: 4624 RVA: 0x0004A2DC File Offset: 0x000484DC
			public unsafe static void Invoke(EntityComponentStore* entityComponentStore, UnsafeList<EntityBatchInChunk>* entityBatchList, in ComponentTypeSet typeSet)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = StructuralChange.RemoveComponentsEntitiesBatch_00000FB4$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.EntityComponentStore*,Unity.Collections.LowLevel.Unsafe.UnsafeList`1<Unity.Entities.EntityBatchInChunk>*,Unity.Entities.ComponentTypeSet&), entityComponentStore, entityBatchList, ref typeSet, functionPointer);
						return;
					}
				}
				StructuralChange.RemoveComponentsEntitiesBatch$BurstManaged(entityComponentStore, entityBatchList, in typeSet);
			}

			// Token: 0x040006C6 RID: 1734
			private static IntPtr Pointer;

			// Token: 0x040006C7 RID: 1735
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020002AA RID: 682
		// (Invoke) Token: 0x06001212 RID: 4626
		internal unsafe delegate void RemoveComponentChunks_00000FB5$PostfixBurstDelegate(EntityComponentStore* entityComponentStore, ArchetypeChunk* chunks, int chunkCount, TypeIndex typeIndex);

		// Token: 0x020002AB RID: 683
		internal static class RemoveComponentChunks_00000FB5$BurstDirectCall
		{
			// Token: 0x06001215 RID: 4629 RVA: 0x0004A311 File Offset: 0x00048511
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (StructuralChange.RemoveComponentChunks_00000FB5$BurstDirectCall.Pointer == 0)
				{
					StructuralChange.RemoveComponentChunks_00000FB5$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(StructuralChange.RemoveComponentChunks_00000FB5$BurstDirectCall.DeferredCompilation, methodof(StructuralChange.RemoveComponentChunks$BurstManaged(EntityComponentStore*, ArchetypeChunk*, int, TypeIndex)).MethodHandle, typeof(StructuralChange.RemoveComponentChunks_00000FB5$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = StructuralChange.RemoveComponentChunks_00000FB5$BurstDirectCall.Pointer;
			}

			// Token: 0x06001216 RID: 4630 RVA: 0x0004A340 File Offset: 0x00048540
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				StructuralChange.RemoveComponentChunks_00000FB5$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06001217 RID: 4631 RVA: 0x0004A358 File Offset: 0x00048558
			public unsafe static void Constructor()
			{
				StructuralChange.RemoveComponentChunks_00000FB5$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(StructuralChange.RemoveComponentChunks(EntityComponentStore*, ArchetypeChunk*, int, TypeIndex)).MethodHandle);
			}

			// Token: 0x06001218 RID: 4632 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06001219 RID: 4633 RVA: 0x0004A369 File Offset: 0x00048569
			// Note: this type is marked as 'beforefieldinit'.
			static RemoveComponentChunks_00000FB5$BurstDirectCall()
			{
				StructuralChange.RemoveComponentChunks_00000FB5$BurstDirectCall.Constructor();
			}

			// Token: 0x0600121A RID: 4634 RVA: 0x0004A370 File Offset: 0x00048570
			public unsafe static void Invoke(EntityComponentStore* entityComponentStore, ArchetypeChunk* chunks, int chunkCount, TypeIndex typeIndex)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = StructuralChange.RemoveComponentChunks_00000FB5$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.EntityComponentStore*,Unity.Entities.ArchetypeChunk*,System.Int32,Unity.Entities.TypeIndex), entityComponentStore, chunks, chunkCount, typeIndex, functionPointer);
						return;
					}
				}
				StructuralChange.RemoveComponentChunks$BurstManaged(entityComponentStore, chunks, chunkCount, typeIndex);
			}

			// Token: 0x040006C8 RID: 1736
			private static IntPtr Pointer;

			// Token: 0x040006C9 RID: 1737
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020002AC RID: 684
		// (Invoke) Token: 0x0600121C RID: 4636
		internal unsafe delegate void RemoveComponentQuery_00000FB6$PostfixBurstDelegate(EntityComponentStore* entityComponentStore, EntityQueryImpl* queryImpl, TypeIndex typeIndex);

		// Token: 0x020002AD RID: 685
		internal static class RemoveComponentQuery_00000FB6$BurstDirectCall
		{
			// Token: 0x0600121F RID: 4639 RVA: 0x0004A3A7 File Offset: 0x000485A7
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (StructuralChange.RemoveComponentQuery_00000FB6$BurstDirectCall.Pointer == 0)
				{
					StructuralChange.RemoveComponentQuery_00000FB6$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(StructuralChange.RemoveComponentQuery_00000FB6$BurstDirectCall.DeferredCompilation, methodof(StructuralChange.RemoveComponentQuery$BurstManaged(EntityComponentStore*, EntityQueryImpl*, TypeIndex)).MethodHandle, typeof(StructuralChange.RemoveComponentQuery_00000FB6$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = StructuralChange.RemoveComponentQuery_00000FB6$BurstDirectCall.Pointer;
			}

			// Token: 0x06001220 RID: 4640 RVA: 0x0004A3D4 File Offset: 0x000485D4
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				StructuralChange.RemoveComponentQuery_00000FB6$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06001221 RID: 4641 RVA: 0x0004A3EC File Offset: 0x000485EC
			public unsafe static void Constructor()
			{
				StructuralChange.RemoveComponentQuery_00000FB6$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(StructuralChange.RemoveComponentQuery(EntityComponentStore*, EntityQueryImpl*, TypeIndex)).MethodHandle);
			}

			// Token: 0x06001222 RID: 4642 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06001223 RID: 4643 RVA: 0x0004A3FD File Offset: 0x000485FD
			// Note: this type is marked as 'beforefieldinit'.
			static RemoveComponentQuery_00000FB6$BurstDirectCall()
			{
				StructuralChange.RemoveComponentQuery_00000FB6$BurstDirectCall.Constructor();
			}

			// Token: 0x06001224 RID: 4644 RVA: 0x0004A404 File Offset: 0x00048604
			public unsafe static void Invoke(EntityComponentStore* entityComponentStore, EntityQueryImpl* queryImpl, TypeIndex typeIndex)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = StructuralChange.RemoveComponentQuery_00000FB6$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.EntityComponentStore*,Unity.Entities.EntityQueryImpl*,Unity.Entities.TypeIndex), entityComponentStore, queryImpl, typeIndex, functionPointer);
						return;
					}
				}
				StructuralChange.RemoveComponentQuery$BurstManaged(entityComponentStore, queryImpl, typeIndex);
			}

			// Token: 0x040006CA RID: 1738
			private static IntPtr Pointer;

			// Token: 0x040006CB RID: 1739
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020002AE RID: 686
		// (Invoke) Token: 0x06001226 RID: 4646
		internal unsafe delegate void RemoveComponentsQuery_00000FB7$PostfixBurstDelegate(EntityComponentStore* entityComponentStore, EntityQueryImpl* queryImpl, in ComponentTypeSet typeSet);

		// Token: 0x020002AF RID: 687
		internal static class RemoveComponentsQuery_00000FB7$BurstDirectCall
		{
			// Token: 0x06001229 RID: 4649 RVA: 0x0004A439 File Offset: 0x00048639
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (StructuralChange.RemoveComponentsQuery_00000FB7$BurstDirectCall.Pointer == 0)
				{
					StructuralChange.RemoveComponentsQuery_00000FB7$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(StructuralChange.RemoveComponentsQuery_00000FB7$BurstDirectCall.DeferredCompilation, methodof(StructuralChange.RemoveComponentsQuery$BurstManaged(EntityComponentStore*, EntityQueryImpl*, ref ComponentTypeSet)).MethodHandle, typeof(StructuralChange.RemoveComponentsQuery_00000FB7$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = StructuralChange.RemoveComponentsQuery_00000FB7$BurstDirectCall.Pointer;
			}

			// Token: 0x0600122A RID: 4650 RVA: 0x0004A468 File Offset: 0x00048668
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				StructuralChange.RemoveComponentsQuery_00000FB7$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600122B RID: 4651 RVA: 0x0004A480 File Offset: 0x00048680
			public unsafe static void Constructor()
			{
				StructuralChange.RemoveComponentsQuery_00000FB7$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(StructuralChange.RemoveComponentsQuery(EntityComponentStore*, EntityQueryImpl*, ref ComponentTypeSet)).MethodHandle);
			}

			// Token: 0x0600122C RID: 4652 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x0600122D RID: 4653 RVA: 0x0004A491 File Offset: 0x00048691
			// Note: this type is marked as 'beforefieldinit'.
			static RemoveComponentsQuery_00000FB7$BurstDirectCall()
			{
				StructuralChange.RemoveComponentsQuery_00000FB7$BurstDirectCall.Constructor();
			}

			// Token: 0x0600122E RID: 4654 RVA: 0x0004A498 File Offset: 0x00048698
			public unsafe static void Invoke(EntityComponentStore* entityComponentStore, EntityQueryImpl* queryImpl, in ComponentTypeSet typeSet)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = StructuralChange.RemoveComponentsQuery_00000FB7$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.EntityComponentStore*,Unity.Entities.EntityQueryImpl*,Unity.Entities.ComponentTypeSet&), entityComponentStore, queryImpl, ref typeSet, functionPointer);
						return;
					}
				}
				StructuralChange.RemoveComponentsQuery$BurstManaged(entityComponentStore, queryImpl, in typeSet);
			}

			// Token: 0x040006CC RID: 1740
			private static IntPtr Pointer;

			// Token: 0x040006CD RID: 1741
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020002B0 RID: 688
		// (Invoke) Token: 0x06001230 RID: 4656
		internal unsafe delegate void AddSharedComponentDataIndexWithBurst_00000FB8$PostfixBurstDelegate(EntityComponentStore* entityComponentStore, Entity* entities, int entityCount, in ComponentType componentType, int newSharedComponentDataIndex);

		// Token: 0x020002B1 RID: 689
		internal static class AddSharedComponentDataIndexWithBurst_00000FB8$BurstDirectCall
		{
			// Token: 0x06001233 RID: 4659 RVA: 0x0004A4CD File Offset: 0x000486CD
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (StructuralChange.AddSharedComponentDataIndexWithBurst_00000FB8$BurstDirectCall.Pointer == 0)
				{
					StructuralChange.AddSharedComponentDataIndexWithBurst_00000FB8$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(StructuralChange.AddSharedComponentDataIndexWithBurst_00000FB8$BurstDirectCall.DeferredCompilation, methodof(StructuralChange.AddSharedComponentDataIndexWithBurst$BurstManaged(EntityComponentStore*, Entity*, int, ref ComponentType, int)).MethodHandle, typeof(StructuralChange.AddSharedComponentDataIndexWithBurst_00000FB8$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = StructuralChange.AddSharedComponentDataIndexWithBurst_00000FB8$BurstDirectCall.Pointer;
			}

			// Token: 0x06001234 RID: 4660 RVA: 0x0004A4FC File Offset: 0x000486FC
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				StructuralChange.AddSharedComponentDataIndexWithBurst_00000FB8$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06001235 RID: 4661 RVA: 0x0004A514 File Offset: 0x00048714
			public unsafe static void Constructor()
			{
				StructuralChange.AddSharedComponentDataIndexWithBurst_00000FB8$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(StructuralChange.AddSharedComponentDataIndexWithBurst(EntityComponentStore*, Entity*, int, ref ComponentType, int)).MethodHandle);
			}

			// Token: 0x06001236 RID: 4662 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06001237 RID: 4663 RVA: 0x0004A525 File Offset: 0x00048725
			// Note: this type is marked as 'beforefieldinit'.
			static AddSharedComponentDataIndexWithBurst_00000FB8$BurstDirectCall()
			{
				StructuralChange.AddSharedComponentDataIndexWithBurst_00000FB8$BurstDirectCall.Constructor();
			}

			// Token: 0x06001238 RID: 4664 RVA: 0x0004A52C File Offset: 0x0004872C
			public unsafe static void Invoke(EntityComponentStore* entityComponentStore, Entity* entities, int entityCount, in ComponentType componentType, int newSharedComponentDataIndex)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = StructuralChange.AddSharedComponentDataIndexWithBurst_00000FB8$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.EntityComponentStore*,Unity.Entities.Entity*,System.Int32,Unity.Entities.ComponentType&,System.Int32), entityComponentStore, entities, entityCount, ref componentType, newSharedComponentDataIndex, functionPointer);
						return;
					}
				}
				StructuralChange.AddSharedComponentDataIndexWithBurst$BurstManaged(entityComponentStore, entities, entityCount, in componentType, newSharedComponentDataIndex);
			}

			// Token: 0x040006CE RID: 1742
			private static IntPtr Pointer;

			// Token: 0x040006CF RID: 1743
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020002B2 RID: 690
		// (Invoke) Token: 0x0600123A RID: 4666
		internal unsafe delegate void SetSharedComponentDataIndexWithBurst_00000FB9$PostfixBurstDelegate(EntityComponentStore* entityComponentStore, Entity* entities, int entityCount, in ComponentType componentType, int newSharedComponentDataIndex);

		// Token: 0x020002B3 RID: 691
		internal static class SetSharedComponentDataIndexWithBurst_00000FB9$BurstDirectCall
		{
			// Token: 0x0600123D RID: 4669 RVA: 0x0004A567 File Offset: 0x00048767
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (StructuralChange.SetSharedComponentDataIndexWithBurst_00000FB9$BurstDirectCall.Pointer == 0)
				{
					StructuralChange.SetSharedComponentDataIndexWithBurst_00000FB9$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(StructuralChange.SetSharedComponentDataIndexWithBurst_00000FB9$BurstDirectCall.DeferredCompilation, methodof(StructuralChange.SetSharedComponentDataIndexWithBurst$BurstManaged(EntityComponentStore*, Entity*, int, ref ComponentType, int)).MethodHandle, typeof(StructuralChange.SetSharedComponentDataIndexWithBurst_00000FB9$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = StructuralChange.SetSharedComponentDataIndexWithBurst_00000FB9$BurstDirectCall.Pointer;
			}

			// Token: 0x0600123E RID: 4670 RVA: 0x0004A594 File Offset: 0x00048794
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				StructuralChange.SetSharedComponentDataIndexWithBurst_00000FB9$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600123F RID: 4671 RVA: 0x0004A5AC File Offset: 0x000487AC
			public unsafe static void Constructor()
			{
				StructuralChange.SetSharedComponentDataIndexWithBurst_00000FB9$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(StructuralChange.SetSharedComponentDataIndexWithBurst(EntityComponentStore*, Entity*, int, ref ComponentType, int)).MethodHandle);
			}

			// Token: 0x06001240 RID: 4672 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06001241 RID: 4673 RVA: 0x0004A5BD File Offset: 0x000487BD
			// Note: this type is marked as 'beforefieldinit'.
			static SetSharedComponentDataIndexWithBurst_00000FB9$BurstDirectCall()
			{
				StructuralChange.SetSharedComponentDataIndexWithBurst_00000FB9$BurstDirectCall.Constructor();
			}

			// Token: 0x06001242 RID: 4674 RVA: 0x0004A5C4 File Offset: 0x000487C4
			public unsafe static void Invoke(EntityComponentStore* entityComponentStore, Entity* entities, int entityCount, in ComponentType componentType, int newSharedComponentDataIndex)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = StructuralChange.SetSharedComponentDataIndexWithBurst_00000FB9$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.EntityComponentStore*,Unity.Entities.Entity*,System.Int32,Unity.Entities.ComponentType&,System.Int32), entityComponentStore, entities, entityCount, ref componentType, newSharedComponentDataIndex, functionPointer);
						return;
					}
				}
				StructuralChange.SetSharedComponentDataIndexWithBurst$BurstManaged(entityComponentStore, entities, entityCount, in componentType, newSharedComponentDataIndex);
			}

			// Token: 0x040006D0 RID: 1744
			private static IntPtr Pointer;

			// Token: 0x040006D1 RID: 1745
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020002B4 RID: 692
		// (Invoke) Token: 0x06001244 RID: 4676
		internal unsafe delegate void SetSharedComponentDataIndexWithBurst_00000FBA$PostfixBurstDelegate(EntityComponentStore* entityComponentStore, EntityQueryImpl* queryImpl, in ComponentType componentType, int newSharedComponentDataIndex);

		// Token: 0x020002B5 RID: 693
		internal static class SetSharedComponentDataIndexWithBurst_00000FBA$BurstDirectCall
		{
			// Token: 0x06001247 RID: 4679 RVA: 0x0004A5FF File Offset: 0x000487FF
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (StructuralChange.SetSharedComponentDataIndexWithBurst_00000FBA$BurstDirectCall.Pointer == 0)
				{
					StructuralChange.SetSharedComponentDataIndexWithBurst_00000FBA$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(StructuralChange.SetSharedComponentDataIndexWithBurst_00000FBA$BurstDirectCall.DeferredCompilation, methodof(StructuralChange.SetSharedComponentDataIndexWithBurst$BurstManaged(EntityComponentStore*, EntityQueryImpl*, ref ComponentType, int)).MethodHandle, typeof(StructuralChange.SetSharedComponentDataIndexWithBurst_00000FBA$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = StructuralChange.SetSharedComponentDataIndexWithBurst_00000FBA$BurstDirectCall.Pointer;
			}

			// Token: 0x06001248 RID: 4680 RVA: 0x0004A62C File Offset: 0x0004882C
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				StructuralChange.SetSharedComponentDataIndexWithBurst_00000FBA$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06001249 RID: 4681 RVA: 0x0004A644 File Offset: 0x00048844
			public unsafe static void Constructor()
			{
				StructuralChange.SetSharedComponentDataIndexWithBurst_00000FBA$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(StructuralChange.SetSharedComponentDataIndexWithBurst(EntityComponentStore*, EntityQueryImpl*, ref ComponentType, int)).MethodHandle);
			}

			// Token: 0x0600124A RID: 4682 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x0600124B RID: 4683 RVA: 0x0004A655 File Offset: 0x00048855
			// Note: this type is marked as 'beforefieldinit'.
			static SetSharedComponentDataIndexWithBurst_00000FBA$BurstDirectCall()
			{
				StructuralChange.SetSharedComponentDataIndexWithBurst_00000FBA$BurstDirectCall.Constructor();
			}

			// Token: 0x0600124C RID: 4684 RVA: 0x0004A65C File Offset: 0x0004885C
			public unsafe static void Invoke(EntityComponentStore* entityComponentStore, EntityQueryImpl* queryImpl, in ComponentType componentType, int newSharedComponentDataIndex)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = StructuralChange.SetSharedComponentDataIndexWithBurst_00000FBA$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.EntityComponentStore*,Unity.Entities.EntityQueryImpl*,Unity.Entities.ComponentType&,System.Int32), entityComponentStore, queryImpl, ref componentType, newSharedComponentDataIndex, functionPointer);
						return;
					}
				}
				StructuralChange.SetSharedComponentDataIndexWithBurst$BurstManaged(entityComponentStore, queryImpl, in componentType, newSharedComponentDataIndex);
			}

			// Token: 0x040006D2 RID: 1746
			private static IntPtr Pointer;

			// Token: 0x040006D3 RID: 1747
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020002B6 RID: 694
		// (Invoke) Token: 0x0600124E RID: 4686
		internal unsafe delegate void AddSharedComponentChunks_00000FBB$PostfixBurstDelegate(EntityComponentStore* entityComponentStore, ArchetypeChunk* chunks, int chunkCount, TypeIndex componentTypeIndex, int sharedComponentIndex);

		// Token: 0x020002B7 RID: 695
		internal static class AddSharedComponentChunks_00000FBB$BurstDirectCall
		{
			// Token: 0x06001251 RID: 4689 RVA: 0x0004A693 File Offset: 0x00048893
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (StructuralChange.AddSharedComponentChunks_00000FBB$BurstDirectCall.Pointer == 0)
				{
					StructuralChange.AddSharedComponentChunks_00000FBB$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(StructuralChange.AddSharedComponentChunks_00000FBB$BurstDirectCall.DeferredCompilation, methodof(StructuralChange.AddSharedComponentChunks$BurstManaged(EntityComponentStore*, ArchetypeChunk*, int, TypeIndex, int)).MethodHandle, typeof(StructuralChange.AddSharedComponentChunks_00000FBB$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = StructuralChange.AddSharedComponentChunks_00000FBB$BurstDirectCall.Pointer;
			}

			// Token: 0x06001252 RID: 4690 RVA: 0x0004A6C0 File Offset: 0x000488C0
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				StructuralChange.AddSharedComponentChunks_00000FBB$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06001253 RID: 4691 RVA: 0x0004A6D8 File Offset: 0x000488D8
			public unsafe static void Constructor()
			{
				StructuralChange.AddSharedComponentChunks_00000FBB$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(StructuralChange.AddSharedComponentChunks(EntityComponentStore*, ArchetypeChunk*, int, TypeIndex, int)).MethodHandle);
			}

			// Token: 0x06001254 RID: 4692 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06001255 RID: 4693 RVA: 0x0004A6E9 File Offset: 0x000488E9
			// Note: this type is marked as 'beforefieldinit'.
			static AddSharedComponentChunks_00000FBB$BurstDirectCall()
			{
				StructuralChange.AddSharedComponentChunks_00000FBB$BurstDirectCall.Constructor();
			}

			// Token: 0x06001256 RID: 4694 RVA: 0x0004A6F0 File Offset: 0x000488F0
			public unsafe static void Invoke(EntityComponentStore* entityComponentStore, ArchetypeChunk* chunks, int chunkCount, TypeIndex componentTypeIndex, int sharedComponentIndex)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = StructuralChange.AddSharedComponentChunks_00000FBB$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.EntityComponentStore*,Unity.Entities.ArchetypeChunk*,System.Int32,Unity.Entities.TypeIndex,System.Int32), entityComponentStore, chunks, chunkCount, componentTypeIndex, sharedComponentIndex, functionPointer);
						return;
					}
				}
				StructuralChange.AddSharedComponentChunks$BurstManaged(entityComponentStore, chunks, chunkCount, componentTypeIndex, sharedComponentIndex);
			}

			// Token: 0x040006D4 RID: 1748
			private static IntPtr Pointer;

			// Token: 0x040006D5 RID: 1749
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020002B8 RID: 696
		// (Invoke) Token: 0x06001258 RID: 4696
		internal unsafe delegate void AddSharedComponentQuery_00000FBC$PostfixBurstDelegate(EntityComponentStore* entityComponentStore, EntityQueryImpl* queryImpl, TypeIndex componentTypeIndex, int sharedComponentIndex);

		// Token: 0x020002B9 RID: 697
		internal static class AddSharedComponentQuery_00000FBC$BurstDirectCall
		{
			// Token: 0x0600125B RID: 4699 RVA: 0x0004A72B File Offset: 0x0004892B
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (StructuralChange.AddSharedComponentQuery_00000FBC$BurstDirectCall.Pointer == 0)
				{
					StructuralChange.AddSharedComponentQuery_00000FBC$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(StructuralChange.AddSharedComponentQuery_00000FBC$BurstDirectCall.DeferredCompilation, methodof(StructuralChange.AddSharedComponentQuery$BurstManaged(EntityComponentStore*, EntityQueryImpl*, TypeIndex, int)).MethodHandle, typeof(StructuralChange.AddSharedComponentQuery_00000FBC$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = StructuralChange.AddSharedComponentQuery_00000FBC$BurstDirectCall.Pointer;
			}

			// Token: 0x0600125C RID: 4700 RVA: 0x0004A758 File Offset: 0x00048958
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				StructuralChange.AddSharedComponentQuery_00000FBC$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600125D RID: 4701 RVA: 0x0004A770 File Offset: 0x00048970
			public unsafe static void Constructor()
			{
				StructuralChange.AddSharedComponentQuery_00000FBC$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(StructuralChange.AddSharedComponentQuery(EntityComponentStore*, EntityQueryImpl*, TypeIndex, int)).MethodHandle);
			}

			// Token: 0x0600125E RID: 4702 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x0600125F RID: 4703 RVA: 0x0004A781 File Offset: 0x00048981
			// Note: this type is marked as 'beforefieldinit'.
			static AddSharedComponentQuery_00000FBC$BurstDirectCall()
			{
				StructuralChange.AddSharedComponentQuery_00000FBC$BurstDirectCall.Constructor();
			}

			// Token: 0x06001260 RID: 4704 RVA: 0x0004A788 File Offset: 0x00048988
			public unsafe static void Invoke(EntityComponentStore* entityComponentStore, EntityQueryImpl* queryImpl, TypeIndex componentTypeIndex, int sharedComponentIndex)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = StructuralChange.AddSharedComponentQuery_00000FBC$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.EntityComponentStore*,Unity.Entities.EntityQueryImpl*,Unity.Entities.TypeIndex,System.Int32), entityComponentStore, queryImpl, componentTypeIndex, sharedComponentIndex, functionPointer);
						return;
					}
				}
				StructuralChange.AddSharedComponentQuery$BurstManaged(entityComponentStore, queryImpl, componentTypeIndex, sharedComponentIndex);
			}

			// Token: 0x040006D6 RID: 1750
			private static IntPtr Pointer;

			// Token: 0x040006D7 RID: 1751
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020002BA RID: 698
		// (Invoke) Token: 0x06001262 RID: 4706
		internal unsafe delegate void MoveEntityArchetype_00000FBD$PostfixBurstDelegate(EntityComponentStore* entityComponentStore, Entity* entity, void* dstArchetype);

		// Token: 0x020002BB RID: 699
		internal static class MoveEntityArchetype_00000FBD$BurstDirectCall
		{
			// Token: 0x06001265 RID: 4709 RVA: 0x0004A7BF File Offset: 0x000489BF
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (StructuralChange.MoveEntityArchetype_00000FBD$BurstDirectCall.Pointer == 0)
				{
					StructuralChange.MoveEntityArchetype_00000FBD$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(StructuralChange.MoveEntityArchetype_00000FBD$BurstDirectCall.DeferredCompilation, methodof(StructuralChange.MoveEntityArchetype$BurstManaged(EntityComponentStore*, Entity*, void*)).MethodHandle, typeof(StructuralChange.MoveEntityArchetype_00000FBD$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = StructuralChange.MoveEntityArchetype_00000FBD$BurstDirectCall.Pointer;
			}

			// Token: 0x06001266 RID: 4710 RVA: 0x0004A7EC File Offset: 0x000489EC
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				StructuralChange.MoveEntityArchetype_00000FBD$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06001267 RID: 4711 RVA: 0x0004A804 File Offset: 0x00048A04
			public unsafe static void Constructor()
			{
				StructuralChange.MoveEntityArchetype_00000FBD$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(StructuralChange.MoveEntityArchetype(EntityComponentStore*, Entity*, void*)).MethodHandle);
			}

			// Token: 0x06001268 RID: 4712 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06001269 RID: 4713 RVA: 0x0004A815 File Offset: 0x00048A15
			// Note: this type is marked as 'beforefieldinit'.
			static MoveEntityArchetype_00000FBD$BurstDirectCall()
			{
				StructuralChange.MoveEntityArchetype_00000FBD$BurstDirectCall.Constructor();
			}

			// Token: 0x0600126A RID: 4714 RVA: 0x0004A81C File Offset: 0x00048A1C
			public unsafe static void Invoke(EntityComponentStore* entityComponentStore, Entity* entity, void* dstArchetype)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = StructuralChange.MoveEntityArchetype_00000FBD$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.EntityComponentStore*,Unity.Entities.Entity*,System.Void*), entityComponentStore, entity, dstArchetype, functionPointer);
						return;
					}
				}
				StructuralChange.MoveEntityArchetype$BurstManaged(entityComponentStore, entity, dstArchetype);
			}

			// Token: 0x040006D8 RID: 1752
			private static IntPtr Pointer;

			// Token: 0x040006D9 RID: 1753
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020002BC RID: 700
		// (Invoke) Token: 0x0600126C RID: 4716
		internal unsafe delegate void SetChunkComponent_00000FBE$PostfixBurstDelegate(EntityComponentStore* entityComponentStore, ArchetypeChunk* chunks, int chunkCount, void* componentData, TypeIndex componentTypeIndex);

		// Token: 0x020002BD RID: 701
		internal static class SetChunkComponent_00000FBE$BurstDirectCall
		{
			// Token: 0x0600126F RID: 4719 RVA: 0x0004A851 File Offset: 0x00048A51
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (StructuralChange.SetChunkComponent_00000FBE$BurstDirectCall.Pointer == 0)
				{
					StructuralChange.SetChunkComponent_00000FBE$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(StructuralChange.SetChunkComponent_00000FBE$BurstDirectCall.DeferredCompilation, methodof(StructuralChange.SetChunkComponent$BurstManaged(EntityComponentStore*, ArchetypeChunk*, int, void*, TypeIndex)).MethodHandle, typeof(StructuralChange.SetChunkComponent_00000FBE$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = StructuralChange.SetChunkComponent_00000FBE$BurstDirectCall.Pointer;
			}

			// Token: 0x06001270 RID: 4720 RVA: 0x0004A880 File Offset: 0x00048A80
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				StructuralChange.SetChunkComponent_00000FBE$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06001271 RID: 4721 RVA: 0x0004A898 File Offset: 0x00048A98
			public unsafe static void Constructor()
			{
				StructuralChange.SetChunkComponent_00000FBE$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(StructuralChange.SetChunkComponent(EntityComponentStore*, ArchetypeChunk*, int, void*, TypeIndex)).MethodHandle);
			}

			// Token: 0x06001272 RID: 4722 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06001273 RID: 4723 RVA: 0x0004A8A9 File Offset: 0x00048AA9
			// Note: this type is marked as 'beforefieldinit'.
			static SetChunkComponent_00000FBE$BurstDirectCall()
			{
				StructuralChange.SetChunkComponent_00000FBE$BurstDirectCall.Constructor();
			}

			// Token: 0x06001274 RID: 4724 RVA: 0x0004A8B0 File Offset: 0x00048AB0
			public unsafe static void Invoke(EntityComponentStore* entityComponentStore, ArchetypeChunk* chunks, int chunkCount, void* componentData, TypeIndex componentTypeIndex)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = StructuralChange.SetChunkComponent_00000FBE$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.EntityComponentStore*,Unity.Entities.ArchetypeChunk*,System.Int32,System.Void*,Unity.Entities.TypeIndex), entityComponentStore, chunks, chunkCount, componentData, componentTypeIndex, functionPointer);
						return;
					}
				}
				StructuralChange.SetChunkComponent$BurstManaged(entityComponentStore, chunks, chunkCount, componentData, componentTypeIndex);
			}

			// Token: 0x040006DA RID: 1754
			private static IntPtr Pointer;

			// Token: 0x040006DB RID: 1755
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020002BE RID: 702
		// (Invoke) Token: 0x06001276 RID: 4726
		internal unsafe delegate void CreateEntity_00000FBF$PostfixBurstDelegate(EntityComponentStore* entityComponentStore, void* archetype, Entity* outEntities, int count);

		// Token: 0x020002BF RID: 703
		internal static class CreateEntity_00000FBF$BurstDirectCall
		{
			// Token: 0x06001279 RID: 4729 RVA: 0x0004A8EB File Offset: 0x00048AEB
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (StructuralChange.CreateEntity_00000FBF$BurstDirectCall.Pointer == 0)
				{
					StructuralChange.CreateEntity_00000FBF$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(StructuralChange.CreateEntity_00000FBF$BurstDirectCall.DeferredCompilation, methodof(StructuralChange.CreateEntity$BurstManaged(EntityComponentStore*, void*, Entity*, int)).MethodHandle, typeof(StructuralChange.CreateEntity_00000FBF$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = StructuralChange.CreateEntity_00000FBF$BurstDirectCall.Pointer;
			}

			// Token: 0x0600127A RID: 4730 RVA: 0x0004A918 File Offset: 0x00048B18
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				StructuralChange.CreateEntity_00000FBF$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600127B RID: 4731 RVA: 0x0004A930 File Offset: 0x00048B30
			public unsafe static void Constructor()
			{
				StructuralChange.CreateEntity_00000FBF$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(StructuralChange.CreateEntity(EntityComponentStore*, void*, Entity*, int)).MethodHandle);
			}

			// Token: 0x0600127C RID: 4732 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x0600127D RID: 4733 RVA: 0x0004A941 File Offset: 0x00048B41
			// Note: this type is marked as 'beforefieldinit'.
			static CreateEntity_00000FBF$BurstDirectCall()
			{
				StructuralChange.CreateEntity_00000FBF$BurstDirectCall.Constructor();
			}

			// Token: 0x0600127E RID: 4734 RVA: 0x0004A948 File Offset: 0x00048B48
			public unsafe static void Invoke(EntityComponentStore* entityComponentStore, void* archetype, Entity* outEntities, int count)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = StructuralChange.CreateEntity_00000FBF$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.EntityComponentStore*,System.Void*,Unity.Entities.Entity*,System.Int32), entityComponentStore, archetype, outEntities, count, functionPointer);
						return;
					}
				}
				StructuralChange.CreateEntity$BurstManaged(entityComponentStore, archetype, outEntities, count);
			}

			// Token: 0x040006DC RID: 1756
			private static IntPtr Pointer;

			// Token: 0x040006DD RID: 1757
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020002C0 RID: 704
		// (Invoke) Token: 0x06001280 RID: 4736
		internal unsafe delegate void DestroyEntity_00000FC0$PostfixBurstDelegate(EntityComponentStore* entityComponentStore, Entity* entities, int count);

		// Token: 0x020002C1 RID: 705
		internal static class DestroyEntity_00000FC0$BurstDirectCall
		{
			// Token: 0x06001283 RID: 4739 RVA: 0x0004A97F File Offset: 0x00048B7F
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (StructuralChange.DestroyEntity_00000FC0$BurstDirectCall.Pointer == 0)
				{
					StructuralChange.DestroyEntity_00000FC0$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(StructuralChange.DestroyEntity_00000FC0$BurstDirectCall.DeferredCompilation, methodof(StructuralChange.DestroyEntity$BurstManaged(EntityComponentStore*, Entity*, int)).MethodHandle, typeof(StructuralChange.DestroyEntity_00000FC0$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = StructuralChange.DestroyEntity_00000FC0$BurstDirectCall.Pointer;
			}

			// Token: 0x06001284 RID: 4740 RVA: 0x0004A9AC File Offset: 0x00048BAC
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				StructuralChange.DestroyEntity_00000FC0$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06001285 RID: 4741 RVA: 0x0004A9C4 File Offset: 0x00048BC4
			public unsafe static void Constructor()
			{
				StructuralChange.DestroyEntity_00000FC0$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(StructuralChange.DestroyEntity(EntityComponentStore*, Entity*, int)).MethodHandle);
			}

			// Token: 0x06001286 RID: 4742 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06001287 RID: 4743 RVA: 0x0004A9D5 File Offset: 0x00048BD5
			// Note: this type is marked as 'beforefieldinit'.
			static DestroyEntity_00000FC0$BurstDirectCall()
			{
				StructuralChange.DestroyEntity_00000FC0$BurstDirectCall.Constructor();
			}

			// Token: 0x06001288 RID: 4744 RVA: 0x0004A9DC File Offset: 0x00048BDC
			public unsafe static void Invoke(EntityComponentStore* entityComponentStore, Entity* entities, int count)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = StructuralChange.DestroyEntity_00000FC0$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.EntityComponentStore*,Unity.Entities.Entity*,System.Int32), entityComponentStore, entities, count, functionPointer);
						return;
					}
				}
				StructuralChange.DestroyEntity$BurstManaged(entityComponentStore, entities, count);
			}

			// Token: 0x040006DE RID: 1758
			private static IntPtr Pointer;

			// Token: 0x040006DF RID: 1759
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020002C2 RID: 706
		// (Invoke) Token: 0x0600128A RID: 4746
		internal unsafe delegate void DestroyChunksQuery_00000FC1$PostfixBurstDelegate(EntityComponentStore* entityComponentStore, EntityQueryImpl* queryImpl, ref BufferTypeHandle<LinkedEntityGroup> linkedEntityGroupTypeHandle);

		// Token: 0x020002C3 RID: 707
		internal static class DestroyChunksQuery_00000FC1$BurstDirectCall
		{
			// Token: 0x0600128D RID: 4749 RVA: 0x0004AA11 File Offset: 0x00048C11
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (StructuralChange.DestroyChunksQuery_00000FC1$BurstDirectCall.Pointer == 0)
				{
					StructuralChange.DestroyChunksQuery_00000FC1$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(StructuralChange.DestroyChunksQuery_00000FC1$BurstDirectCall.DeferredCompilation, methodof(StructuralChange.DestroyChunksQuery$BurstManaged(EntityComponentStore*, EntityQueryImpl*, ref BufferTypeHandle<LinkedEntityGroup>)).MethodHandle, typeof(StructuralChange.DestroyChunksQuery_00000FC1$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = StructuralChange.DestroyChunksQuery_00000FC1$BurstDirectCall.Pointer;
			}

			// Token: 0x0600128E RID: 4750 RVA: 0x0004AA40 File Offset: 0x00048C40
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				StructuralChange.DestroyChunksQuery_00000FC1$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600128F RID: 4751 RVA: 0x0004AA58 File Offset: 0x00048C58
			public unsafe static void Constructor()
			{
				StructuralChange.DestroyChunksQuery_00000FC1$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(StructuralChange.DestroyChunksQuery(EntityComponentStore*, EntityQueryImpl*, ref BufferTypeHandle<LinkedEntityGroup>)).MethodHandle);
			}

			// Token: 0x06001290 RID: 4752 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06001291 RID: 4753 RVA: 0x0004AA69 File Offset: 0x00048C69
			// Note: this type is marked as 'beforefieldinit'.
			static DestroyChunksQuery_00000FC1$BurstDirectCall()
			{
				StructuralChange.DestroyChunksQuery_00000FC1$BurstDirectCall.Constructor();
			}

			// Token: 0x06001292 RID: 4754 RVA: 0x0004AA70 File Offset: 0x00048C70
			public unsafe static void Invoke(EntityComponentStore* entityComponentStore, EntityQueryImpl* queryImpl, ref BufferTypeHandle<LinkedEntityGroup> linkedEntityGroupTypeHandle)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = StructuralChange.DestroyChunksQuery_00000FC1$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.EntityComponentStore*,Unity.Entities.EntityQueryImpl*,Unity.Entities.BufferTypeHandle`1<Unity.Entities.LinkedEntityGroup>&), entityComponentStore, queryImpl, ref linkedEntityGroupTypeHandle, functionPointer);
						return;
					}
				}
				StructuralChange.DestroyChunksQuery$BurstManaged(entityComponentStore, queryImpl, ref linkedEntityGroupTypeHandle);
			}

			// Token: 0x040006E0 RID: 1760
			private static IntPtr Pointer;

			// Token: 0x040006E1 RID: 1761
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020002C4 RID: 708
		// (Invoke) Token: 0x06001294 RID: 4756
		internal unsafe delegate void InstantiateEntity_00000FC2$PostfixBurstDelegate(EntityComponentStore* entityComponentStore, Entity* srcEntity, Entity* outputEntities, int instanceCount);

		// Token: 0x020002C5 RID: 709
		internal static class InstantiateEntity_00000FC2$BurstDirectCall
		{
			// Token: 0x06001297 RID: 4759 RVA: 0x0004AAA5 File Offset: 0x00048CA5
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (StructuralChange.InstantiateEntity_00000FC2$BurstDirectCall.Pointer == 0)
				{
					StructuralChange.InstantiateEntity_00000FC2$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(StructuralChange.InstantiateEntity_00000FC2$BurstDirectCall.DeferredCompilation, methodof(StructuralChange.InstantiateEntity$BurstManaged(EntityComponentStore*, Entity*, Entity*, int)).MethodHandle, typeof(StructuralChange.InstantiateEntity_00000FC2$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = StructuralChange.InstantiateEntity_00000FC2$BurstDirectCall.Pointer;
			}

			// Token: 0x06001298 RID: 4760 RVA: 0x0004AAD4 File Offset: 0x00048CD4
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				StructuralChange.InstantiateEntity_00000FC2$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06001299 RID: 4761 RVA: 0x0004AAEC File Offset: 0x00048CEC
			public unsafe static void Constructor()
			{
				StructuralChange.InstantiateEntity_00000FC2$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(StructuralChange.InstantiateEntity(EntityComponentStore*, Entity*, Entity*, int)).MethodHandle);
			}

			// Token: 0x0600129A RID: 4762 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x0600129B RID: 4763 RVA: 0x0004AAFD File Offset: 0x00048CFD
			// Note: this type is marked as 'beforefieldinit'.
			static InstantiateEntity_00000FC2$BurstDirectCall()
			{
				StructuralChange.InstantiateEntity_00000FC2$BurstDirectCall.Constructor();
			}

			// Token: 0x0600129C RID: 4764 RVA: 0x0004AB04 File Offset: 0x00048D04
			public unsafe static void Invoke(EntityComponentStore* entityComponentStore, Entity* srcEntity, Entity* outputEntities, int instanceCount)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = StructuralChange.InstantiateEntity_00000FC2$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.EntityComponentStore*,Unity.Entities.Entity*,Unity.Entities.Entity*,System.Int32), entityComponentStore, srcEntity, outputEntities, instanceCount, functionPointer);
						return;
					}
				}
				StructuralChange.InstantiateEntity$BurstManaged(entityComponentStore, srcEntity, outputEntities, instanceCount);
			}

			// Token: 0x040006E2 RID: 1762
			private static IntPtr Pointer;

			// Token: 0x040006E3 RID: 1763
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020002C6 RID: 710
		// (Invoke) Token: 0x0600129E RID: 4766
		internal unsafe delegate void InstantiateEntities_00000FC3$PostfixBurstDelegate(EntityComponentStore* entityComponentStore, Entity* srcEntities, Entity* outputEntities, int entityCount, bool removePrefab);

		// Token: 0x020002C7 RID: 711
		internal static class InstantiateEntities_00000FC3$BurstDirectCall
		{
			// Token: 0x060012A1 RID: 4769 RVA: 0x0004AB3B File Offset: 0x00048D3B
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (StructuralChange.InstantiateEntities_00000FC3$BurstDirectCall.Pointer == 0)
				{
					StructuralChange.InstantiateEntities_00000FC3$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(StructuralChange.InstantiateEntities_00000FC3$BurstDirectCall.DeferredCompilation, methodof(StructuralChange.InstantiateEntities$BurstManaged(EntityComponentStore*, Entity*, Entity*, int, bool)).MethodHandle, typeof(StructuralChange.InstantiateEntities_00000FC3$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = StructuralChange.InstantiateEntities_00000FC3$BurstDirectCall.Pointer;
			}

			// Token: 0x060012A2 RID: 4770 RVA: 0x0004AB68 File Offset: 0x00048D68
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				StructuralChange.InstantiateEntities_00000FC3$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x060012A3 RID: 4771 RVA: 0x0004AB80 File Offset: 0x00048D80
			public unsafe static void Constructor()
			{
				StructuralChange.InstantiateEntities_00000FC3$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(StructuralChange.InstantiateEntities(EntityComponentStore*, Entity*, Entity*, int, bool)).MethodHandle);
			}

			// Token: 0x060012A4 RID: 4772 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x060012A5 RID: 4773 RVA: 0x0004AB91 File Offset: 0x00048D91
			// Note: this type is marked as 'beforefieldinit'.
			static InstantiateEntities_00000FC3$BurstDirectCall()
			{
				StructuralChange.InstantiateEntities_00000FC3$BurstDirectCall.Constructor();
			}

			// Token: 0x060012A6 RID: 4774 RVA: 0x0004AB98 File Offset: 0x00048D98
			public unsafe static void Invoke(EntityComponentStore* entityComponentStore, Entity* srcEntities, Entity* outputEntities, int entityCount, bool removePrefab)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = StructuralChange.InstantiateEntities_00000FC3$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.EntityComponentStore*,Unity.Entities.Entity*,Unity.Entities.Entity*,System.Int32,System.Boolean), entityComponentStore, srcEntities, outputEntities, entityCount, removePrefab, functionPointer);
						return;
					}
				}
				StructuralChange.InstantiateEntities$BurstManaged(entityComponentStore, srcEntities, outputEntities, entityCount, removePrefab);
			}

			// Token: 0x040006E4 RID: 1764
			private static IntPtr Pointer;

			// Token: 0x040006E5 RID: 1765
			private static IntPtr DeferredCompilation;
		}
	}
}
