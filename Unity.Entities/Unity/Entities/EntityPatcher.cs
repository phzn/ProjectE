using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Profiling;
using Unity.Properties;

namespace Unity.Entities
{
	// Token: 0x020000B6 RID: 182
	[BurstCompile]
	public static class EntityPatcher
	{
		// Token: 0x0600035E RID: 862 RVA: 0x00013DD2 File Offset: 0x00011FD2
		internal static string[] CollectImportantProfilerMarkerStrings()
		{
			return new string[] { EntityPatcher.s_ApplyChangeSetProfilerMarkerStr };
		}

		// Token: 0x0600035F RID: 863 RVA: 0x00013DE4 File Offset: 0x00011FE4
		public static void ApplyChangeSet(EntityManager entityManager, EntityChangeSet changeSet)
		{
			if (!changeSet.IsCreated)
			{
				return;
			}
			using (EntityQuery entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<EntityGuid>().WithOptions(EntityQueryOptions.IncludePrefab | EntityQueryOptions.IncludeDisabledEntities).Build(entityManager))
			{
				using (EntityQuery entityQuery2 = new EntityQueryBuilder(Allocator.Temp).WithAll<EntityGuid, Prefab>().WithOptions(EntityQueryOptions.IncludePrefab | EntityQueryOptions.IncludeDisabledEntities).Build(entityManager))
				{
					using (EntityQuery entityQuery3 = new EntityQueryBuilder(Allocator.Temp).WithAll<EntityGuid, LinkedEntityGroup>().WithOptions(EntityQueryOptions.IncludePrefab | EntityQueryOptions.IncludeDisabledEntities).Build(entityManager))
					{
						int num = entityQuery.CalculateEntityCount();
						NativeParallelMultiHashMap<int, Entity> nativeParallelMultiHashMap = new NativeParallelMultiHashMap<int, Entity>(num, Allocator.TempJob);
						try
						{
							NativeArray<ComponentType> nativeArray = new NativeArray<ComponentType>(changeSet.TypeHashes.Length, Allocator.TempJob, NativeArrayOptions.ClearMemory);
							try
							{
								NativeParallelMultiHashMap<EntityGuid, Entity> nativeParallelMultiHashMap2 = new NativeParallelMultiHashMap<EntityGuid, Entity>(num, Allocator.TempJob);
								try
								{
									NativeParallelHashMap<Entity, EntityGuid> nativeParallelHashMap = new NativeParallelHashMap<Entity, EntityGuid>(entityQuery.CalculateEntityCount(), Allocator.TempJob);
									try
									{
										EntityPatcher.BuildEntityLookups(entityManager, entityQuery, nativeParallelMultiHashMap2, nativeParallelHashMap);
										EntityPatcher.BuildPackedLookups(changeSet, nativeParallelMultiHashMap2, nativeParallelMultiHashMap, nativeArray);
										EntityPatcher.ApplyDestroyEntities(in entityManager, in changeSet.Entities, in nativeParallelMultiHashMap, in nativeParallelMultiHashMap2, changeSet.DestroyedEntityCount);
										EntityPatcher.ApplyCreateEntitiesWithArchetypes(in entityManager, in nativeParallelMultiHashMap, in changeSet.AddArchetypes);
										EntityPatcher.ApplyRemoveComponents(in entityManager, in changeSet.RemoveComponents, in changeSet.Entities, in nativeParallelMultiHashMap, in nativeArray);
										EntityPatcher.ApplyAddComponents(in entityManager, in changeSet.AddComponents, in changeSet.Entities, in nativeParallelMultiHashMap, in nativeArray);
										EntityPatcher.ApplySetSharedComponents(entityManager, changeSet.SetSharedComponents, changeSet.UnmanagedSharedComponentData, changeSet.Entities, nativeParallelMultiHashMap, nativeArray);
										EntityPatcher.ApplySetManagedComponents(entityManager, changeSet.SetManagedComponents, changeSet.Entities, nativeParallelMultiHashMap, nativeArray);
										EntityPatcher.ApplySetComponents(entityManager, changeSet.SetComponents, changeSet.ComponentData, changeSet.Entities, nativeParallelMultiHashMap, nativeArray, nativeParallelMultiHashMap2, nativeParallelHashMap);
										EntityPatcher.ApplyEntityNames(entityManager, changeSet.Names, changeSet.NameChangedEntityGuids, nativeParallelMultiHashMap, nativeParallelMultiHashMap2, changeSet.CreatedEntityCount, changeSet.NameChangedCount);
										int num2 = EntityPatcher.CalculateLinkedEntityGroupEntitiesLength(entityManager, entityQuery3);
										NativeParallelHashMap<EntityGuid, Entity> nativeParallelHashMap2 = new NativeParallelHashMap<EntityGuid, Entity>(entityQuery2.CalculateEntityCount(), Allocator.TempJob);
										try
										{
											NativeParallelHashMap<Entity, Entity> nativeParallelHashMap3 = new NativeParallelHashMap<Entity, Entity>(num2, Allocator.TempJob);
											try
											{
												EntityPatcher.BuildPrefabAndLinkedEntityGroupLookups(entityManager, entityQuery, entityQuery2, entityQuery3, nativeParallelHashMap2, nativeParallelHashMap3);
												EntityPatcher.ApplyLinkedEntityGroupRemovals(in entityManager, in changeSet.LinkedEntityGroupRemovals, in changeSet.Entities, in nativeParallelMultiHashMap, in nativeParallelMultiHashMap2, in nativeParallelHashMap, in nativeParallelHashMap3);
												EntityPatcher.ApplyLinkedEntityGroupAdditions(in entityManager, in changeSet.LinkedEntityGroupAdditions, in changeSet.Entities, in nativeParallelMultiHashMap, in nativeParallelMultiHashMap2, in nativeParallelHashMap, in nativeParallelHashMap2, in nativeParallelHashMap3);
												EntityPatcher.ApplyEntityPatches(entityManager, changeSet.EntityReferenceChanges, changeSet.Entities, nativeParallelMultiHashMap, nativeArray, nativeParallelMultiHashMap2, nativeParallelHashMap, nativeParallelHashMap2, nativeParallelHashMap3);
											}
											finally
											{
												((IDisposable)nativeParallelHashMap3).Dispose();
											}
										}
										finally
										{
											((IDisposable)nativeParallelHashMap2).Dispose();
										}
										EntityPatcher.ApplyBlobAssetChanges(entityManager, changeSet.Entities, nativeParallelMultiHashMap, nativeArray, changeSet.CreatedBlobAssets, changeSet.BlobAssetData, changeSet.DestroyedBlobAssets, changeSet.BlobAssetReferenceChanges);
									}
									finally
									{
										((IDisposable)nativeParallelHashMap).Dispose();
									}
								}
								finally
								{
									((IDisposable)nativeParallelMultiHashMap2).Dispose();
								}
							}
							finally
							{
								((IDisposable)nativeArray).Dispose();
							}
						}
						finally
						{
							((IDisposable)nativeParallelMultiHashMap).Dispose();
						}
					}
				}
			}
		}

		// Token: 0x06000360 RID: 864 RVA: 0x000141C4 File Offset: 0x000123C4
		private static void BuildEntityLookups(EntityManager entityManager, EntityQuery entityQuery, NativeParallelMultiHashMap<EntityGuid, Entity> entityGuidToEntity, NativeParallelHashMap<Entity, EntityGuid> entityToEntityGuid)
		{
			JobHandle jobHandle = new EntityPatcher.BuildComponentToEntityMultiHashMap
			{
				EntityTypeHandle = entityManager.GetEntityTypeHandle(),
				ComponentTypeHandle = entityManager.GetComponentTypeHandle<EntityGuid>(true),
				ComponentToEntity = entityGuidToEntity.AsParallelWriter()
			}.ScheduleParallel(entityQuery, default(JobHandle));
			JobHandle jobHandle2 = new EntityPatcher.BuildEntityToComponentHashMap
			{
				EntityTypeHandle = entityManager.GetEntityTypeHandle(),
				EntityGuidComponentTypeHandle = entityManager.GetComponentTypeHandle<EntityGuid>(true),
				EntityToEntityGuid = entityToEntityGuid.AsParallelWriter()
			}.ScheduleParallel(entityQuery, default(JobHandle));
			JobHandle.CombineDependencies(jobHandle, jobHandle2).Complete();
		}

		// Token: 0x06000361 RID: 865 RVA: 0x00014268 File Offset: 0x00012468
		private static void BuildPrefabAndLinkedEntityGroupLookups(EntityManager entityManager, EntityQuery entityQuery, EntityQuery prefabQuery, EntityQuery linkedEntityGroupQuery, NativeParallelHashMap<EntityGuid, Entity> entityGuidToPrefab, NativeParallelHashMap<Entity, Entity> entityToLinkedEntityGroupRoot)
		{
			JobHandle jobHandle = new EntityPatcher.BuildComponentToEntityHashMap
			{
				EntityTypeHandle = entityManager.GetEntityTypeHandle(),
				ComponentTypeHandle = entityManager.GetComponentTypeHandle<EntityGuid>(true),
				ComponentToEntity = entityGuidToPrefab.AsParallelWriter()
			}.ScheduleParallel(prefabQuery, default(JobHandle));
			JobHandle jobHandle2 = new EntityPatcher.BuildLinkedEntityGroupHashMap
			{
				EntityToLinkedEntityGroupRoot = entityToLinkedEntityGroupRoot.AsParallelWriter(),
				LinkedEntityGroupTypeHandle = entityManager.GetBufferTypeHandle<LinkedEntityGroup>(true)
			}.ScheduleParallel(linkedEntityGroupQuery, default(JobHandle));
			JobHandle.CombineDependencies(jobHandle, jobHandle2).Complete();
		}

		// Token: 0x06000362 RID: 866 RVA: 0x000142FC File Offset: 0x000124FC
		private static void BuildPackedLookups(EntityChangeSet changeSet, NativeParallelMultiHashMap<EntityGuid, Entity> entityGuidToEntity, NativeParallelMultiHashMap<int, Entity> packedEntities, NativeArray<ComponentType> packedTypes)
		{
			JobHandle jobHandle = new EntityPatcher.BuildPackedEntityLookupJob
			{
				StartIndex = changeSet.CreatedEntityCount,
				EntityGuids = changeSet.Entities,
				EntityGuidToEntity = entityGuidToEntity,
				PackedEntities = packedEntities.AsParallelWriter()
			}.Schedule(changeSet.Entities.Length - changeSet.CreatedEntityCount, 64, default(JobHandle));
			JobHandle jobHandle2 = new EntityPatcher.BuildPackedTypeLookupJob
			{
				TypeHashes = changeSet.TypeHashes,
				PackedTypes = packedTypes
			}.Schedule(changeSet.TypeHashes.Length, 64, default(JobHandle));
			JobHandle.CombineDependencies(jobHandle, jobHandle2).Complete();
		}

		// Token: 0x06000363 RID: 867 RVA: 0x000143B4 File Offset: 0x000125B4
		[GenerateTestsForBurstCompatibility]
		[BurstCompile]
		internal static void ApplyCreateEntitiesWithArchetypes(in EntityManager entityManager, in NativeParallelMultiHashMap<int, Entity> packedEntities, in NativeArray<FilteredArchetype> createdArchetypes)
		{
			EntityPatcher.ApplyCreateEntitiesWithArchetypes_0000032C$BurstDirectCall.Invoke(in entityManager, in packedEntities, in createdArchetypes);
		}

		// Token: 0x06000364 RID: 868 RVA: 0x000143C0 File Offset: 0x000125C0
		[GenerateTestsForBurstCompatibility(RequiredUnityDefine = "UNITY_EDITOR && !DOTS_DISABLE_DEBUG_NAMES", CompileTarget = GenerateTestsForBurstCompatibilityAttribute.BurstCompatibleCompileTarget.Editor)]
		internal static void ApplyEntityNames(EntityManager entityManager, NativeArray<FixedString64Bytes> names, NativeArray<EntityGuid> nameChangedEntityGuids, NativeParallelMultiHashMap<int, Entity> packedEntities, NativeParallelMultiHashMap<EntityGuid, Entity> entityGuidToEntity, int createdEntityCount, int nameChangedCount)
		{
			if (names.Length == 0)
			{
				return;
			}
			new EntityPatcher.ApplyNamesJob
			{
				EntityManager = entityManager,
				Names = names,
				NameChangeEntityGuids = nameChangedEntityGuids,
				CreatedEntityCount = createdEntityCount,
				NameChangedCount = nameChangedCount,
				PackedEntities = packedEntities,
				EntityGuidToEntity = entityGuidToEntity
			}.Run<EntityPatcher.ApplyNamesJob>();
		}

		// Token: 0x06000365 RID: 869 RVA: 0x00014420 File Offset: 0x00012620
		[GenerateTestsForBurstCompatibility]
		[BurstCompile]
		internal static void ApplyDestroyEntities(in EntityManager entityManager, in NativeArray<EntityGuid> entities, in NativeParallelMultiHashMap<int, Entity> packedEntities, in NativeParallelMultiHashMap<EntityGuid, Entity> entityGuidToEntity, int destroyedEntityCount)
		{
			EntityPatcher.ApplyDestroyEntities_0000032E$BurstDirectCall.Invoke(in entityManager, in entities, in packedEntities, in entityGuidToEntity, destroyedEntityCount);
		}

		// Token: 0x06000366 RID: 870 RVA: 0x00014430 File Offset: 0x00012630
		private unsafe static void ApplyAddComponents(in EntityManager entityManager, in NativeArray<PackedComponent> addComponents, in NativeArray<EntityGuid> packedEntityGuids, in NativeParallelMultiHashMap<int, Entity> packedEntities, in NativeArray<ComponentType> packedTypes)
		{
			NativeArray<PackedComponent> nativeArray = addComponents;
			NativeList<EntityPatcher.SetComponentError> nativeList = new NativeList<EntityPatcher.SetComponentError>(nativeArray.Length, Allocator.TempJob);
			EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob, PlaybackPolicy.SinglePlayback);
			EntityPatcher.ApplyAddComponentsJob applyAddComponentsJob = default(EntityPatcher.ApplyAddComponentsJob);
			EntityManager entityManager2 = entityManager;
			applyAddComponentsJob.store = entityManager2.GetCheckedEntityDataAccess()->EntityComponentStore;
			applyAddComponentsJob.AddComponents = addComponents;
			applyAddComponentsJob.PackedEntities = packedEntities;
			applyAddComponentsJob.PackedEntityGuids = packedEntityGuids;
			applyAddComponentsJob.PackedTypes = packedTypes;
			applyAddComponentsJob.LinkedEntityGroupTypeIndex = TypeManager.GetTypeIndex<LinkedEntityGroup>();
			applyAddComponentsJob.ComponentAlreadyExists = nativeList.AsParallelWriter();
			applyAddComponentsJob.ecb = entityCommandBuffer.AsParallelWriter();
			EntityPatcher.ApplyAddComponentsJob applyAddComponentsJob2 = applyAddComponentsJob;
			nativeArray = addComponents;
			applyAddComponentsJob2.ScheduleParallel(nativeArray.Length, 32, default(JobHandle)).Complete();
			for (int i = 0; i < nativeList.Length; i++)
			{
				EntityPatcher.SetComponentError setComponentError = nativeList[i];
				Debug.LogWarning(string.Format("AddComponent({0}, {1}) but the component already exists.", setComponentError.Guid, setComponentError.ComponentType));
			}
			entityCommandBuffer.Playback(entityManager);
			entityCommandBuffer.Dispose();
			nativeList.Dispose();
		}

		// Token: 0x06000367 RID: 871 RVA: 0x00014572 File Offset: 0x00012772
		[GenerateTestsForBurstCompatibility]
		[BurstCompile]
		internal static void ApplyRemoveComponents(in EntityManager entityManager, in NativeArray<PackedComponent> removeComponents, in NativeArray<EntityGuid> packedEntityGuids, in NativeParallelMultiHashMap<int, Entity> packedEntities, in NativeArray<ComponentType> packedTypes)
		{
			EntityPatcher.ApplyRemoveComponents_00000330$BurstDirectCall.Invoke(in entityManager, in removeComponents, in packedEntityGuids, in packedEntities, in packedTypes);
		}

		// Token: 0x06000368 RID: 872 RVA: 0x00014580 File Offset: 0x00012780
		private unsafe static void ApplySetSharedComponents(EntityManager entityManager, PackedSharedComponentDataChange[] sharedComponentDataChanges, UnsafeAppendBuffer unmanagedSharedComponentData, NativeArray<EntityGuid> packedEntityGuid, NativeParallelMultiHashMap<int, Entity> packedEntities, NativeArray<ComponentType> packedTypes)
		{
			NativeArray<EntityPatcher.UnmanagedPackedComponentData> nativeArray = new NativeArray<EntityPatcher.UnmanagedPackedComponentData>(sharedComponentDataChanges.Length, Allocator.TempJob, NativeArrayOptions.ClearMemory);
			int num = 0;
			for (int i = 0; i < sharedComponentDataChanges.Length; i++)
			{
				PackedSharedComponentDataChange packedSharedComponentDataChange = sharedComponentDataChanges[i];
				int unmanagedSharedValueDataOffsetWithManagedFlag = packedSharedComponentDataChange.UnmanagedSharedValueDataOffsetWithManagedFlag;
				PackedComponent component = packedSharedComponentDataChange.Component;
				nativeArray[i] = new EntityPatcher.UnmanagedPackedComponentData
				{
					offset = unmanagedSharedValueDataOffsetWithManagedFlag,
					packedComponent = component
				};
				if ((unmanagedSharedValueDataOffsetWithManagedFlag & -2147483648) != 0)
				{
					num += packedEntities.CountValuesForKey(component.PackedEntityIndex);
				}
			}
			NativeList<EntityPatcher.SetComponentError> nativeList = new NativeList<EntityPatcher.SetComponentError>(sharedComponentDataChanges.Length, Allocator.TempJob);
			NativeList<EntityPatcher.SetComponentError> nativeList2 = new NativeList<EntityPatcher.SetComponentError>(sharedComponentDataChanges.Length, Allocator.TempJob);
			NativeList<EntityPatcher.ManagedComponentData> nativeList3 = new NativeList<EntityPatcher.ManagedComponentData>(num, Allocator.TempJob);
			EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob, PlaybackPolicy.SinglePlayback);
			new EntityPatcher.ApplySetSharedComponentsJob
			{
				store = entityManager.GetCheckedEntityDataAccess()->EntityComponentStore,
				UnmanagedPackedComponents = nativeArray,
				UnmanagedSharedComponentData = unmanagedSharedComponentData,
				PackedEntities = packedEntities,
				PackedEntityGuids = packedEntityGuid,
				PackedTypes = packedTypes,
				ComponentDoesNotExist = nativeList2.AsParallelWriter(),
				EntityDoesNotExist = nativeList.AsParallelWriter(),
				SetManagedComponents = nativeList3.AsParallelWriter(),
				ecb = entityCommandBuffer.AsParallelWriter()
			}.ScheduleParallel(nativeArray.Length, 64, default(JobHandle)).Complete();
			nativeArray.Dispose();
			for (int j = 0; j < nativeList3.Length; j++)
			{
				EntityPatcher.ManagedComponentData managedComponentData = nativeList3[j];
				entityCommandBuffer.UnsafeSetSharedComponentManagedNonDefault(managedComponentData.entity, sharedComponentDataChanges[managedComponentData.packedIndex].BoxedSharedValue, managedComponentData.typeIndex);
			}
			for (int k = 0; k < nativeList.Length; k++)
			{
				EntityPatcher.SetComponentError setComponentError = nativeList[k];
				Debug.LogWarning(string.Format("SetComponent<{0}>({1}) but entity does not exist.", setComponentError.ComponentType, setComponentError.Guid));
			}
			for (int l = 0; l < nativeList2.Length; l++)
			{
				EntityPatcher.SetComponentError setComponentError2 = nativeList2[l];
				Debug.LogWarning(string.Format("SetComponent<{0}>({1}) but component does not exist.", setComponentError2.ComponentType, setComponentError2.Guid));
			}
			entityCommandBuffer.Playback(entityManager);
			entityCommandBuffer.Dispose();
			nativeList.Dispose();
			nativeList2.Dispose();
			nativeList3.Dispose();
		}

		// Token: 0x06000369 RID: 873 RVA: 0x000147E0 File Offset: 0x000129E0
		private static void ApplySetManagedComponents(EntityManager entityManager, PackedManagedComponentDataChange[] managedComponentDataChanges, NativeArray<EntityGuid> packedEntityGuid, NativeParallelMultiHashMap<int, Entity> packedEntities, NativeArray<ComponentType> packedTypes)
		{
			ManagedObjectClone managedObjectClone = new ManagedObjectClone();
			foreach (PackedManagedComponentDataChange packedManagedComponentDataChange in managedComponentDataChanges)
			{
				PackedComponent component = packedManagedComponentDataChange.Component;
				Entity entity;
				NativeParallelMultiHashMapIterator<int> nativeParallelMultiHashMapIterator;
				if (packedEntities.TryGetFirstValue(component.PackedEntityIndex, out entity, out nativeParallelMultiHashMapIterator))
				{
					ComponentType componentType = packedTypes[component.PackedTypeIndex];
					do
					{
						if (!entityManager.Exists(entity))
						{
							Debug.LogWarning(string.Format("SetComponent<{0}>({1}) but entity does not exist.", componentType, packedEntityGuid[component.PackedEntityIndex]));
						}
						else if (!entityManager.HasComponent(entity, componentType))
						{
							Debug.LogWarning(string.Format("SetComponent<{0}>({1}) but component does not exist.", componentType, packedEntityGuid[component.PackedEntityIndex]));
						}
						else
						{
							object obj = managedObjectClone.Clone(packedManagedComponentDataChange.BoxedValue);
							entityManager.SetComponentObject(entity, componentType, obj);
						}
					}
					while (packedEntities.TryGetNextValue(out entity, ref nativeParallelMultiHashMapIterator));
				}
			}
		}

		// Token: 0x0600036A RID: 874 RVA: 0x000148D4 File Offset: 0x00012AD4
		private static void ApplySetComponents(EntityManager entityManager, NativeArray<PackedComponentDataChange> changes, NativeArray<byte> payload, NativeArray<EntityGuid> packedEntityGuids, NativeParallelMultiHashMap<int, Entity> packedEntities, NativeArray<ComponentType> packedTypes, NativeParallelMultiHashMap<EntityGuid, Entity> entityGuidToEntity, NativeParallelHashMap<Entity, EntityGuid> entityToEntityGuid)
		{
			entityManager.BeforeStructuralChange();
			NativeList<EntityPatcher.SetComponentError> nativeList = new NativeList<EntityPatcher.SetComponentError>(Allocator.TempJob);
			NativeList<EntityPatcher.SetComponentError> nativeList2 = new NativeList<EntityPatcher.SetComponentError>(Allocator.TempJob);
			NativeList<Entity> nativeList3 = new NativeList<Entity>(Allocator.TempJob);
			new EntityPatcher.ApplySetComponentsJob
			{
				Changes = changes,
				EntitiesWithCompanionLink = nativeList3,
				ComponentDoesNotExist = nativeList2,
				EntityDoesNotExist = nativeList,
				EntityGuidToEntity = entityGuidToEntity,
				EntityManager = entityManager,
				EntityToEntityGuid = entityToEntityGuid,
				PackedEntities = packedEntities,
				PackedEntityGuids = packedEntityGuids,
				PackedTypes = packedTypes,
				Payload = payload
			}.Run<EntityPatcher.ApplySetComponentsJob>();
			for (int i = 0; i < nativeList.Length; i++)
			{
				EntityPatcher.SetComponentError setComponentError = nativeList[i];
				Debug.LogWarning(string.Format("SetComponent<{0}>({1}) but entity does not exist.", setComponentError.ComponentType, setComponentError.Guid));
			}
			for (int j = 0; j < nativeList2.Length; j++)
			{
				EntityPatcher.SetComponentError setComponentError2 = nativeList2[j];
				Debug.LogWarning(string.Format("SetComponent<{0}>({1}) but component does not exist.", setComponentError2.ComponentType, setComponentError2.Guid));
			}
			if (nativeList3.Length > 0)
			{
				ManagedComponentStore.AssignCompanionComponentsToCompanionGameObjects(entityManager, nativeList3.AsArray());
			}
			nativeList.Dispose();
			nativeList2.Dispose();
			nativeList3.Dispose();
		}

		// Token: 0x0600036B RID: 875 RVA: 0x00014A3C File Offset: 0x00012C3C
		[GenerateTestsForBurstCompatibility]
		[BurstCompile]
		internal static void ApplyUnmanagedEntityPatches(in EntityManager entityManager, in NativeArray<EntityReferenceChange> changes, in NativeArray<EntityGuid> packedEntityGuids, in NativeParallelMultiHashMap<int, Entity> packedEntities, in NativeArray<ComponentType> packedTypes, in NativeParallelMultiHashMap<EntityGuid, Entity> entityGuidToEntity, in NativeParallelHashMap<Entity, EntityGuid> entityToEntityGuid, in NativeParallelHashMap<EntityGuid, Entity> entityGuidToPrefab, in NativeParallelHashMap<Entity, Entity> entityToLinkedEntityGroupRoot, in NativeParallelMultiHashMap<int, EntityPatcher.EntityTargetPair> entityTargets)
		{
			EntityPatcher.ApplyUnmanagedEntityPatches_00000334$BurstDirectCall.Invoke(in entityManager, in changes, in packedEntityGuids, in packedEntities, in packedTypes, in entityGuidToEntity, in entityToEntityGuid, in entityGuidToPrefab, in entityToLinkedEntityGroupRoot, in entityTargets);
		}

		// Token: 0x0600036C RID: 876 RVA: 0x00014A60 File Offset: 0x00012C60
		private static void ApplyManagedEntityPatches(EntityManager entityManager, NativeArray<EntityReferenceChange> changes, NativeArray<EntityGuid> packedEntityGuids, NativeParallelMultiHashMap<int, Entity> packedEntities, NativeArray<ComponentType> packedTypes, NativeParallelMultiHashMap<int, EntityPatcher.EntityTargetPair> entityTargets)
		{
			NativeParallelMultiHashMap<EntityPatcher.EntityComponentPair, EntityPatcher.ManagedObjectEntityReferencePatch> nativeParallelMultiHashMap = new NativeParallelMultiHashMap<EntityPatcher.EntityComponentPair, EntityPatcher.ManagedObjectEntityReferencePatch>(changes.Length, Allocator.Temp);
			for (int i = 0; i < changes.Length; i++)
			{
				EntityPatcher.EntityTargetPair entityTargetPair;
				NativeParallelMultiHashMapIterator<int> nativeParallelMultiHashMapIterator;
				if (entityTargets.TryGetFirstValue(i, out entityTargetPair, out nativeParallelMultiHashMapIterator))
				{
					EntityReferenceChange entityReferenceChange = changes[i];
					PackedComponent component = entityReferenceChange.Component;
					ComponentType componentType = packedTypes[component.PackedTypeIndex];
					int offset = entityReferenceChange.Offset;
					do
					{
						nativeParallelMultiHashMap.Add(new EntityPatcher.EntityComponentPair
						{
							Entity = entityTargetPair.Entity,
							Component = componentType
						}, new EntityPatcher.ManagedObjectEntityReferencePatch
						{
							Id = offset,
							TargetEntity = entityTargetPair.TargetEntity
						});
					}
					while (entityTargets.TryGetNextValue(out entityTargetPair, ref nativeParallelMultiHashMapIterator));
				}
			}
			EntityPatcher.ManagedObjectEntityReferencePatcher managedObjectEntityReferencePatcher = new EntityPatcher.ManagedObjectEntityReferencePatcher();
			using (NativeArray<EntityPatcher.EntityComponentPair> keyArray = nativeParallelMultiHashMap.GetKeyArray(Allocator.Temp))
			{
				keyArray.Sort<EntityPatcher.EntityComponentPair>();
				int num = keyArray.Unique<EntityPatcher.EntityComponentPair>();
				for (int j = 0; j < num; j++)
				{
					EntityPatcher.EntityComponentPair entityComponentPair = keyArray[j];
					NativeParallelMultiHashMap<EntityPatcher.EntityComponentPair, EntityPatcher.ManagedObjectEntityReferencePatch>.Enumerator valuesForKey = nativeParallelMultiHashMap.GetValuesForKey(entityComponentPair);
					if (entityComponentPair.Component.IsManagedComponent)
					{
						object componentObject = entityManager.GetComponentObject<object>(entityComponentPair.Entity, entityComponentPair.Component);
						managedObjectEntityReferencePatcher.ApplyPatches(ref componentObject, valuesForKey);
					}
					else if (entityComponentPair.Component.IsSharedComponent)
					{
						object sharedComponentData = entityManager.GetSharedComponentData(entityComponentPair.Entity, entityComponentPair.Component.TypeIndex);
						managedObjectEntityReferencePatcher.ApplyPatches(ref sharedComponentData, valuesForKey);
						entityManager.SetSharedComponentDataBoxedDefaultMustBeNull(entityComponentPair.Entity, entityComponentPair.Component.TypeIndex, sharedComponentData);
					}
					valuesForKey.Dispose();
				}
			}
			nativeParallelMultiHashMap.Dispose();
		}

		// Token: 0x0600036D RID: 877 RVA: 0x00014C24 File Offset: 0x00012E24
		private static void ApplyEntityPatches(EntityManager entityManager, NativeArray<EntityReferenceChange> changes, NativeArray<EntityGuid> packedEntityGuids, NativeParallelMultiHashMap<int, Entity> packedEntities, NativeArray<ComponentType> packedTypes, NativeParallelMultiHashMap<EntityGuid, Entity> entityGuidToEntity, NativeParallelHashMap<Entity, EntityGuid> entityToEntityGuid, NativeParallelHashMap<EntityGuid, Entity> entityGuidToPrefab, NativeParallelHashMap<Entity, Entity> entityToLinkedEntityGroupRoot)
		{
			if (changes.Length == 0)
			{
				return;
			}
			NativeParallelMultiHashMap<int, EntityPatcher.EntityTargetPair> nativeParallelMultiHashMap = new NativeParallelMultiHashMap<int, EntityPatcher.EntityTargetPair>(changes.Length, Allocator.Temp);
			EntityPatcher.ApplyUnmanagedEntityPatches(in entityManager, in changes, in packedEntityGuids, in packedEntities, in packedTypes, in entityGuidToEntity, in entityToEntityGuid, in entityGuidToPrefab, in entityToLinkedEntityGroupRoot, in nativeParallelMultiHashMap);
			EntityPatcher.ApplyManagedEntityPatches(entityManager, changes, packedEntityGuids, packedEntities, packedTypes, nativeParallelMultiHashMap);
			nativeParallelMultiHashMap.Dispose();
		}

		// Token: 0x0600036E RID: 878 RVA: 0x00014C7B File Offset: 0x00012E7B
		[GenerateTestsForBurstCompatibility]
		[BurstCompile]
		internal static void ApplyLinkedEntityGroupAdditions(in EntityManager entityManager, in NativeArray<LinkedEntityGroupChange> linkedEntityGroupChanges, in NativeArray<EntityGuid> packedEntityGuids, in NativeParallelMultiHashMap<int, Entity> packedEntities, in NativeParallelMultiHashMap<EntityGuid, Entity> entityGuidToEntity, in NativeParallelHashMap<Entity, EntityGuid> entityToEntityGuid, in NativeParallelHashMap<EntityGuid, Entity> entityGuidToPrefab, in NativeParallelHashMap<Entity, Entity> entityToLinkedEntityGroupRoot)
		{
			EntityPatcher.ApplyLinkedEntityGroupAdditions_00000337$BurstDirectCall.Invoke(in entityManager, in linkedEntityGroupChanges, in packedEntityGuids, in packedEntities, in entityGuidToEntity, in entityToEntityGuid, in entityGuidToPrefab, in entityToLinkedEntityGroupRoot);
		}

		// Token: 0x0600036F RID: 879 RVA: 0x00014C8E File Offset: 0x00012E8E
		[GenerateTestsForBurstCompatibility]
		[BurstCompile]
		internal static void ApplyLinkedEntityGroupRemovals(in EntityManager entityManager, in NativeArray<LinkedEntityGroupChange> linkedEntityGroupChanges, in NativeArray<EntityGuid> packedEntityGuids, in NativeParallelMultiHashMap<int, Entity> packedEntities, in NativeParallelMultiHashMap<EntityGuid, Entity> entityGuidToEntity, in NativeParallelHashMap<Entity, EntityGuid> entityToEntityGuid, in NativeParallelHashMap<Entity, Entity> entityToLinkedEntityGroupRoot)
		{
			EntityPatcher.ApplyLinkedEntityGroupRemovals_00000338$BurstDirectCall.Invoke(in entityManager, in linkedEntityGroupChanges, in packedEntityGuids, in packedEntities, in entityGuidToEntity, in entityToEntityGuid, in entityToLinkedEntityGroupRoot);
		}

		// Token: 0x06000370 RID: 880 RVA: 0x00014CA0 File Offset: 0x00012EA0
		private unsafe static int CalculateLinkedEntityGroupEntitiesLength(EntityManager entityManager, EntityQuery linkedEntityGroupQuery)
		{
			int num = 0;
			using (NativeArray<ArchetypeChunk> nativeArray = linkedEntityGroupQuery.ToArchetypeChunkArray(Allocator.TempJob))
			{
				new EntityPatcher.CalculateLinkedEntityGroupEntitiesLengthJob
				{
					Count = &num,
					Chunks = nativeArray,
					LinkedEntityGroupTypeHandle = entityManager.GetBufferTypeHandle<LinkedEntityGroup>(true)
				}.Schedule(default(JobHandle)).Complete();
			}
			return num;
		}

		// Token: 0x06000371 RID: 881 RVA: 0x00014D20 File Offset: 0x00012F20
		private unsafe static void ApplyBlobAssetChanges(EntityManager entityManager, NativeArray<EntityGuid> packedEntityGuids, NativeParallelMultiHashMap<int, Entity> packedEntities, NativeArray<ComponentType> packedTypes, NativeArray<BlobAssetChange> createdBlobAssets, NativeArray<byte> createdBlobAssetData, NativeArray<ulong> destroyedBlobAssets, NativeArray<BlobAssetReferenceChange> blobAssetReferenceChanges)
		{
			if (createdBlobAssets.Length == 0 && blobAssetReferenceChanges.Length == 0 && destroyedBlobAssets.Length == 0)
			{
				return;
			}
			NativeParallelMultiHashMap<EntityPatcher.EntityComponentPair, EntityPatcher.ManagedObjectBlobAssetReferencePatch> nativeParallelMultiHashMap = new NativeParallelMultiHashMap<EntityPatcher.EntityComponentPair, EntityPatcher.ManagedObjectBlobAssetReferencePatch>(blobAssetReferenceChanges.Length, Allocator.Temp);
			EntityPatcherBlobAssetSystem orCreateSystemManaged = entityManager.World.GetOrCreateSystemManaged<EntityPatcherBlobAssetSystem>();
			byte* ptr = (byte*)createdBlobAssetData.GetUnsafePtr<byte>();
			for (int i = 0; i < createdBlobAssets.Length; i++)
			{
				BlobAssetPtr blobAssetPtr;
				if (!orCreateSystemManaged.TryGetBlobAsset(createdBlobAssets[i].Hash, out blobAssetPtr))
				{
					orCreateSystemManaged.AllocateBlobAsset((void*)ptr, createdBlobAssets[i].Length, createdBlobAssets[i].Hash);
				}
				ptr += createdBlobAssets[i].Length;
			}
			for (int j = 0; j < destroyedBlobAssets.Length; j++)
			{
				orCreateSystemManaged.ReleaseBlobAsset(entityManager, destroyedBlobAssets[j]);
			}
			for (int k = 0; k < blobAssetReferenceChanges.Length; k++)
			{
				PackedComponent component = blobAssetReferenceChanges[k].Component;
				ComponentType componentType = packedTypes[component.PackedTypeIndex];
				int offset = blobAssetReferenceChanges[k].Offset;
				BlobAssetReferenceData blobAssetReferenceData = default(BlobAssetReferenceData);
				BlobAssetPtr blobAssetPtr2;
				if (orCreateSystemManaged.TryGetBlobAsset(blobAssetReferenceChanges[k].Value, out blobAssetPtr2))
				{
					blobAssetReferenceData = new BlobAssetReferenceData
					{
						m_Ptr = (byte*)blobAssetPtr2.Data
					};
				}
				Entity entity;
				NativeParallelMultiHashMapIterator<int> nativeParallelMultiHashMapIterator;
				if (packedEntities.TryGetFirstValue(component.PackedEntityIndex, out entity, out nativeParallelMultiHashMapIterator))
				{
					do
					{
						if (!entityManager.Exists(entity))
						{
							Debug.LogWarning(string.Format("ApplyBlobAssetReferencePatches<{0}>({1}) but entity to patch does not exist.", componentType, packedEntityGuids[component.PackedEntityIndex]));
						}
						else if (!entityManager.HasComponent(entity, componentType))
						{
							Debug.LogWarning(string.Format("ApplyBlobAssetReferencePatches<{0}>({1}) but component in entity to patch does not exist.", componentType, packedEntityGuids[component.PackedEntityIndex]));
						}
						else if (componentType.IsBuffer)
						{
							byte* bufferRawRW = (byte*)entityManager.GetBufferRawRW(entity, componentType.TypeIndex);
							UnsafeUtility.MemCpy((void*)(bufferRawRW + offset), (void*)(&blobAssetReferenceData), (long)sizeof(BlobAssetReferenceData));
						}
						else if (componentType.IsManagedComponent || componentType.IsSharedComponent)
						{
							nativeParallelMultiHashMap.Add(new EntityPatcher.EntityComponentPair
							{
								Entity = entity,
								Component = componentType
							}, new EntityPatcher.ManagedObjectBlobAssetReferencePatch
							{
								Id = offset,
								Target = blobAssetReferenceChanges[k].Value
							});
						}
						else
						{
							byte* componentDataRawRW = (byte*)entityManager.GetComponentDataRawRW(entity, componentType.TypeIndex);
							UnsafeUtility.MemCpy((void*)(componentDataRawRW + offset), (void*)(&blobAssetReferenceData), (long)sizeof(BlobAssetReferenceData));
						}
					}
					while (packedEntities.TryGetNextValue(out entity, ref nativeParallelMultiHashMapIterator));
				}
			}
			EntityPatcher.ManagedObjectBlobAssetReferencePatcher managedObjectBlobAssetReferencePatcher = new EntityPatcher.ManagedObjectBlobAssetReferencePatcher(orCreateSystemManaged);
			using (NativeArray<EntityPatcher.EntityComponentPair> keyArray = nativeParallelMultiHashMap.GetKeyArray(Allocator.Temp))
			{
				keyArray.Sort<EntityPatcher.EntityComponentPair>();
				int num = keyArray.Unique<EntityPatcher.EntityComponentPair>();
				for (int l = 0; l < num; l++)
				{
					EntityPatcher.EntityComponentPair entityComponentPair = keyArray[l];
					NativeParallelMultiHashMap<EntityPatcher.EntityComponentPair, EntityPatcher.ManagedObjectBlobAssetReferencePatch>.Enumerator valuesForKey = nativeParallelMultiHashMap.GetValuesForKey(entityComponentPair);
					if (entityComponentPair.Component.IsManagedComponent)
					{
						object componentObject = entityManager.GetComponentObject<object>(entityComponentPair.Entity, entityComponentPair.Component);
						managedObjectBlobAssetReferencePatcher.ApplyPatches(ref componentObject, valuesForKey);
					}
					else if (entityComponentPair.Component.TypeIndex.IsManagedSharedComponent)
					{
						object sharedComponentData = entityManager.GetSharedComponentData(entityComponentPair.Entity, entityComponentPair.Component.TypeIndex);
						managedObjectBlobAssetReferencePatcher.ApplyPatches(ref sharedComponentData, valuesForKey);
						entityManager.SetSharedComponentDataBoxedDefaultMustBeNull(entityComponentPair.Entity, entityComponentPair.Component.TypeIndex, sharedComponentData);
					}
					else if (entityComponentPair.Component.IsSharedComponent && !entityComponentPair.Component.TypeIndex.IsManagedSharedComponent)
					{
						EntityDataAccess* checkedEntityDataAccess = entityManager.GetCheckedEntityDataAccess();
						EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginStructuralChanges();
						int sharedComponentDataIndex = checkedEntityDataAccess->EntityComponentStore->GetSharedComponentDataIndex(entityComponentPair.Entity, entityComponentPair.Component.TypeIndex);
						byte* sharedComponentDataAddr_Unmanaged = (byte*)checkedEntityDataAccess->EntityComponentStore->GetSharedComponentDataAddr_Unmanaged(sharedComponentDataIndex, entityComponentPair.Component.TypeIndex);
						foreach (EntityPatcher.ManagedObjectBlobAssetReferencePatch managedObjectBlobAssetReferencePatch in valuesForKey)
						{
							int id = managedObjectBlobAssetReferencePatch.Id;
							BlobAssetPtr blobAssetPtr3;
							BlobAssetReferenceData blobAssetReferenceData2;
							if (orCreateSystemManaged.TryGetBlobAsset(managedObjectBlobAssetReferencePatch.Target, out blobAssetPtr3))
							{
								blobAssetReferenceData2 = new BlobAssetReferenceData
								{
									m_Ptr = (byte*)blobAssetPtr3.Data
								};
							}
							UnsafeUtility.MemCpy((void*)(sharedComponentDataAddr_Unmanaged + id), (void*)(&blobAssetReferenceData2), (long)sizeof(BlobAssetReferenceData));
						}
						int num2 = 0;
						if (sharedComponentDataAddr_Unmanaged != null)
						{
							num2 = TypeManager.GetHashCode((void*)sharedComponentDataAddr_Unmanaged, entityComponentPair.Component.TypeIndex);
						}
						checkedEntityDataAccess->SetSharedComponentDataAddrDefaultMustBeNullDuringStructuralChange(entityComponentPair.Entity, entityComponentPair.Component.TypeIndex, num2, (void*)sharedComponentDataAddr_Unmanaged);
						checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
					}
					valuesForKey.Dispose();
				}
			}
			nativeParallelMultiHashMap.Dispose();
			orCreateSystemManaged.ReleaseUnusedBlobAssets();
		}

		// Token: 0x06000373 RID: 883 RVA: 0x00015348 File Offset: 0x00013548
		[GenerateTestsForBurstCompatibility]
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void ApplyCreateEntitiesWithArchetypes$BurstManaged(in EntityManager entityManager, in NativeParallelMultiHashMap<int, Entity> packedEntities, in NativeArray<FilteredArchetype> createdArchetypes)
		{
			int num = 0;
			for (;;)
			{
				int num2 = num;
				NativeArray<FilteredArchetype> nativeArray = createdArchetypes;
				if (num2 >= nativeArray.Length)
				{
					break;
				}
				nativeArray = createdArchetypes;
				FilteredArchetype filteredArchetype = nativeArray[num];
				NativeArray<ComponentType> nativeArray2 = new NativeArray<ComponentType>(filteredArchetype.TypeIndices.Length, Allocator.Temp, NativeArrayOptions.ClearMemory);
				for (int i = 0; i < nativeArray2.Length; i++)
				{
					nativeArray2[i] = ComponentType.ReadOnly(filteredArchetype.TypeIndices[i]);
				}
				EntityManager entityManager2 = entityManager;
				EntityArchetype entityArchetype = entityManager2.CreateArchetypeWithoutSimulateComponent(nativeArray2);
				using (NativeArray<Entity> nativeArray3 = new NativeArray<Entity>(filteredArchetype.EntityCount, Allocator.Temp, NativeArrayOptions.ClearMemory))
				{
					entityManager2 = entityManager;
					entityManager2.CreateEntity(entityArchetype, nativeArray3);
					for (int j = 0; j < filteredArchetype.EntityCount; j++)
					{
						NativeParallelMultiHashMap<int, Entity> nativeParallelMultiHashMap = packedEntities;
						nativeParallelMultiHashMap.Add(filteredArchetype.PackedEntityIndices[j], nativeArray3[j]);
					}
				}
				num++;
			}
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0001545C File Offset: 0x0001365C
		[GenerateTestsForBurstCompatibility]
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void ApplyDestroyEntities$BurstManaged(in EntityManager entityManager, in NativeArray<EntityGuid> entities, in NativeParallelMultiHashMap<int, Entity> packedEntities, in NativeParallelMultiHashMap<EntityGuid, Entity> entityGuidToEntity, int destroyedEntityCount)
		{
			ComponentType componentType = ComponentType.ReadOnly<LinkedEntityGroup>();
			NativeArray<EntityGuid> nativeArray = entities;
			int num = nativeArray.Length - destroyedEntityCount;
			for (;;)
			{
				int num2 = num;
				nativeArray = entities;
				if (num2 >= nativeArray.Length)
				{
					break;
				}
				NativeParallelMultiHashMap<int, Entity> nativeParallelMultiHashMap = packedEntities;
				Entity entity;
				NativeParallelMultiHashMapIterator<int> nativeParallelMultiHashMapIterator;
				if (nativeParallelMultiHashMap.TryGetFirstValue(num, out entity, out nativeParallelMultiHashMapIterator))
				{
					do
					{
						NativeParallelMultiHashMap<EntityGuid, Entity> nativeParallelMultiHashMap2 = entityGuidToEntity;
						nativeArray = entities;
						nativeParallelMultiHashMap2.Remove(nativeArray[num], entity);
						EntityManager entityManager2 = entityManager;
						if (entityManager2.Exists(entity))
						{
							entityManager2 = entityManager;
							entityManager2.RemoveComponent(entity, componentType);
							entityManager2 = entityManager;
							entityManager2.DestroyEntity(entity);
						}
						nativeParallelMultiHashMap = packedEntities;
					}
					while (nativeParallelMultiHashMap.TryGetNextValue(out entity, ref nativeParallelMultiHashMapIterator));
				}
				num++;
			}
		}

		// Token: 0x06000375 RID: 885 RVA: 0x00015518 File Offset: 0x00013718
		[GenerateTestsForBurstCompatibility]
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void ApplyRemoveComponents$BurstManaged(in EntityManager entityManager, in NativeArray<PackedComponent> removeComponents, in NativeArray<EntityGuid> packedEntityGuids, in NativeParallelMultiHashMap<int, Entity> packedEntities, in NativeArray<ComponentType> packedTypes)
		{
			TypeIndex typeIndex = TypeManager.GetTypeIndex<EntityGuid>();
			int num = 0;
			for (;;)
			{
				int num2 = num;
				NativeArray<PackedComponent> nativeArray = removeComponents;
				if (num2 >= nativeArray.Length)
				{
					break;
				}
				nativeArray = removeComponents;
				PackedComponent packedComponent = nativeArray[num];
				NativeParallelMultiHashMap<int, Entity> nativeParallelMultiHashMap = packedEntities;
				Entity entity;
				NativeParallelMultiHashMapIterator<int> nativeParallelMultiHashMapIterator;
				if (nativeParallelMultiHashMap.TryGetFirstValue(packedComponent.PackedEntityIndex, out entity, out nativeParallelMultiHashMapIterator))
				{
					NativeArray<ComponentType> nativeArray2 = packedTypes;
					ComponentType componentType = nativeArray2[packedComponent.PackedTypeIndex];
					do
					{
						componentType.TypeIndex == typeIndex;
						EntityManager entityManager2 = entityManager;
						if (entityManager2.HasComponent(entity, componentType))
						{
							entityManager2 = entityManager;
							entityManager2.RemoveComponent(entity, componentType);
						}
						else
						{
							string text = "RemoveComponent({0}, {1}) but the component was already removed.";
							NativeArray<EntityGuid> nativeArray3 = packedEntityGuids;
							Debug.LogWarning(string.Format(text, nativeArray3[packedComponent.PackedEntityIndex], componentType));
						}
						nativeParallelMultiHashMap = packedEntities;
					}
					while (nativeParallelMultiHashMap.TryGetNextValue(out entity, ref nativeParallelMultiHashMapIterator));
				}
				num++;
			}
		}

		// Token: 0x06000376 RID: 886 RVA: 0x00015610 File Offset: 0x00013810
		[GenerateTestsForBurstCompatibility]
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void ApplyUnmanagedEntityPatches$BurstManaged(in EntityManager entityManager, in NativeArray<EntityReferenceChange> changes, in NativeArray<EntityGuid> packedEntityGuids, in NativeParallelMultiHashMap<int, Entity> packedEntities, in NativeArray<ComponentType> packedTypes, in NativeParallelMultiHashMap<EntityGuid, Entity> entityGuidToEntity, in NativeParallelHashMap<Entity, EntityGuid> entityToEntityGuid, in NativeParallelHashMap<EntityGuid, Entity> entityGuidToPrefab, in NativeParallelHashMap<Entity, Entity> entityToLinkedEntityGroupRoot, in NativeParallelMultiHashMap<int, EntityPatcher.EntityTargetPair> entityTargets)
		{
			int num = 0;
			for (;;)
			{
				int num2 = num;
				NativeArray<EntityReferenceChange> nativeArray = changes;
				if (num2 >= nativeArray.Length)
				{
					break;
				}
				nativeArray = changes;
				EntityReferenceChange entityReferenceChange = nativeArray[num];
				PackedComponent component = entityReferenceChange.Component;
				NativeArray<ComponentType> nativeArray2 = packedTypes;
				ComponentType componentType = nativeArray2[component.PackedTypeIndex];
				EntityGuid value = entityReferenceChange.Value;
				int offset = entityReferenceChange.Offset;
				bool flag = false;
				Entity entity;
				if (value.Equals(default(EntityGuid)))
				{
					entity = Entity.Null;
					goto IL_00C3;
				}
				NativeParallelMultiHashMap<EntityGuid, Entity> nativeParallelMultiHashMap = entityGuidToEntity;
				NativeParallelMultiHashMapIterator<EntityGuid> nativeParallelMultiHashMapIterator;
				if (nativeParallelMultiHashMap.TryGetFirstValue(value, out entity, out nativeParallelMultiHashMapIterator))
				{
					nativeParallelMultiHashMap = entityGuidToEntity;
					Entity entity2;
					flag = nativeParallelMultiHashMap.TryGetNextValue(out entity2, ref nativeParallelMultiHashMapIterator);
					goto IL_00C3;
				}
				string text = "PatchEntities<{0}>({1}) but entity with guid-to-patch-to does not exist.";
				object obj = componentType;
				NativeArray<EntityGuid> nativeArray3 = packedEntityGuids;
				Debug.LogWarning(string.Format(text, obj, nativeArray3[component.PackedEntityIndex]));
				IL_030A:
				num++;
				continue;
				IL_00C3:
				NativeParallelMultiHashMap<int, Entity> nativeParallelMultiHashMap2 = packedEntities;
				Entity entity3;
				NativeParallelMultiHashMapIterator<int> nativeParallelMultiHashMapIterator2;
				if (!nativeParallelMultiHashMap2.TryGetFirstValue(component.PackedEntityIndex, out entity3, out nativeParallelMultiHashMapIterator2))
				{
					goto IL_030A;
				}
				for (;;)
				{
					EntityManager entityManager2 = entityManager;
					if (!entityManager2.Exists(entity3))
					{
						string text2 = "PatchEntities<{0}>({1}) but entity to patch does not exist.";
						object obj2 = componentType;
						nativeArray3 = packedEntityGuids;
						Debug.LogWarning(string.Format(text2, obj2, nativeArray3[component.PackedEntityIndex]));
					}
					else
					{
						entityManager2 = entityManager;
						if (!entityManager2.HasComponent(entity3, componentType))
						{
							string text3 = "PatchEntities<{0}>({1}) but component in entity to patch does not exist.";
							object obj3 = componentType;
							nativeArray3 = packedEntityGuids;
							Debug.LogWarning(string.Format(text3, obj3, nativeArray3[component.PackedEntityIndex]));
						}
						else
						{
							if (flag)
							{
								entity = Entity.Null;
								NativeParallelHashMap<Entity, Entity> nativeParallelHashMap = entityToLinkedEntityGroupRoot;
								Entity entity4;
								if (nativeParallelHashMap.TryGetValue(entity3, out entity4))
								{
									entityManager2 = entityManager;
									DynamicBuffer<LinkedEntityGroup> buffer = entityManager2.GetBuffer<LinkedEntityGroup>(entity4, false);
									for (int i = 0; i < buffer.Length; i++)
									{
										NativeParallelHashMap<Entity, EntityGuid> nativeParallelHashMap2 = entityToEntityGuid;
										EntityGuid entityGuid;
										if (nativeParallelHashMap2.TryGetValue(buffer[i].Value, out entityGuid) && entityGuid.Equals(value))
										{
											entity = buffer[i].Value;
											break;
										}
									}
								}
								else
								{
									NativeParallelHashMap<EntityGuid, Entity> nativeParallelHashMap3 = entityGuidToPrefab;
									if (!nativeParallelHashMap3.TryGetValue(value, out entity))
									{
										string text4 = "PatchEntities<{0}>({1}) but 2+ entities for GUID of entity-to-patch-to, and no root for entity-to-patch is, so we can't disambiguate.";
										object obj4 = componentType;
										nativeArray3 = packedEntityGuids;
										Debug.LogWarning(string.Format(text4, obj4, nativeArray3[component.PackedEntityIndex]));
										goto IL_02F2;
									}
								}
							}
							if (componentType.IsBuffer)
							{
								entityManager2 = entityManager;
								byte* bufferRawRW = (byte*)entityManager2.GetBufferRawRW(entity3, componentType.TypeIndex);
								UnsafeUtility.MemCpy((void*)(bufferRawRW + offset), (void*)(&entity), (long)sizeof(Entity));
							}
							else if (componentType.IsManagedComponent || componentType.TypeIndex.IsManagedSharedComponent)
							{
								NativeParallelMultiHashMap<int, EntityPatcher.EntityTargetPair> nativeParallelMultiHashMap3 = entityTargets;
								nativeParallelMultiHashMap3.Add(num, new EntityPatcher.EntityTargetPair
								{
									Entity = entity3,
									TargetEntity = entity
								});
							}
							else
							{
								entityManager2 = entityManager;
								byte* componentDataRawRW = (byte*)entityManager2.GetComponentDataRawRW(entity3, componentType.TypeIndex);
								UnsafeUtility.MemCpy((void*)(componentDataRawRW + offset), (void*)(&entity), (long)sizeof(Entity));
							}
						}
					}
					IL_02F2:
					nativeParallelMultiHashMap2 = packedEntities;
					if (!nativeParallelMultiHashMap2.TryGetNextValue(out entity3, ref nativeParallelMultiHashMapIterator2))
					{
						goto IL_030A;
					}
				}
			}
		}

		// Token: 0x06000377 RID: 887 RVA: 0x00015940 File Offset: 0x00013B40
		[GenerateTestsForBurstCompatibility]
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void ApplyLinkedEntityGroupAdditions$BurstManaged(in EntityManager entityManager, in NativeArray<LinkedEntityGroupChange> linkedEntityGroupChanges, in NativeArray<EntityGuid> packedEntityGuids, in NativeParallelMultiHashMap<int, Entity> packedEntities, in NativeParallelMultiHashMap<EntityGuid, Entity> entityGuidToEntity, in NativeParallelHashMap<Entity, EntityGuid> entityToEntityGuid, in NativeParallelHashMap<EntityGuid, Entity> entityGuidToPrefab, in NativeParallelHashMap<Entity, Entity> entityToLinkedEntityGroupRoot)
		{
			using (NativeList<EntityPatcher.Child> nativeList = new NativeList<EntityPatcher.Child>(Allocator.TempJob))
			{
				int num = 0;
				for (;;)
				{
					int num2 = num;
					NativeArray<LinkedEntityGroupChange> nativeArray = linkedEntityGroupChanges;
					if (num2 >= nativeArray.Length)
					{
						break;
					}
					nativeArray = linkedEntityGroupChanges;
					LinkedEntityGroupChange linkedEntityGroupChange = nativeArray[num];
					NativeParallelHashMap<EntityGuid, Entity> nativeParallelHashMap = entityGuidToPrefab;
					Entity entity;
					if (nativeParallelHashMap.TryGetValue(linkedEntityGroupChange.ChildEntityGuid, out entity))
					{
						NativeParallelMultiHashMap<EntityGuid, Entity> nativeParallelMultiHashMap = entityGuidToEntity;
						Entity entity2;
						NativeParallelMultiHashMapIterator<EntityGuid> nativeParallelMultiHashMapIterator;
						if (nativeParallelMultiHashMap.TryGetFirstValue(linkedEntityGroupChange.RootEntityGuid, out entity2, out nativeParallelMultiHashMapIterator))
						{
							do
							{
								EntityManager entityManager2 = entityManager;
								if (entityManager2.HasComponent<Prefab>(entity2))
								{
									entityManager2 = entityManager;
									entityManager2.GetBuffer<LinkedEntityGroup>(entity2, false).Add(entity);
									NativeParallelHashMap<Entity, Entity> nativeParallelHashMap2 = entityToLinkedEntityGroupRoot;
									nativeParallelHashMap2.TryAdd(entity, entity2);
								}
								else
								{
									entityManager2 = entityManager;
									Entity entity3 = entityManager2.Instantiate(entity);
									entityManager2 = entityManager;
									entityManager2.GetBuffer<LinkedEntityGroup>(entity2, false).Add(entity3);
									EntityPatcher.Child child = default(EntityPatcher.Child);
									child.RootEntity = entity2;
									child.ChildEntity = entity3;
									child.ChildEntityGuid = linkedEntityGroupChange.ChildEntityGuid;
									nativeList.Add(in child);
								}
								nativeParallelMultiHashMap = entityGuidToEntity;
							}
							while (nativeParallelMultiHashMap.TryGetNextValue(out entity2, ref nativeParallelMultiHashMapIterator));
						}
						else
						{
							Debug.LogWarning("Tried to add a child to a linked entity group, but root entity didn't exist in destination world.");
						}
					}
					else
					{
						NativeParallelMultiHashMap<EntityGuid, Entity> nativeParallelMultiHashMap = entityGuidToEntity;
						Entity entity4;
						NativeParallelMultiHashMapIterator<EntityGuid> nativeParallelMultiHashMapIterator2;
						bool flag = nativeParallelMultiHashMap.TryGetFirstValue(linkedEntityGroupChange.RootEntityGuid, out entity4, out nativeParallelMultiHashMapIterator2);
						nativeParallelMultiHashMap = entityGuidToEntity;
						Entity entity5;
						NativeParallelMultiHashMapIterator<EntityGuid> nativeParallelMultiHashMapIterator3;
						bool flag2 = nativeParallelMultiHashMap.TryGetFirstValue(linkedEntityGroupChange.ChildEntityGuid, out entity5, out nativeParallelMultiHashMapIterator3);
						if (!flag)
						{
							Debug.LogWarning("Failed to add a linked child. The specified root entity was not found in the destination world.");
						}
						else if (!flag2)
						{
							Debug.LogWarning("Failed to add a linked child. The specified child entity was not found in the destination world.");
						}
						else
						{
							nativeParallelMultiHashMap = entityGuidToEntity;
							Entity entity6;
							bool flag3 = nativeParallelMultiHashMap.TryGetNextValue(out entity6, ref nativeParallelMultiHashMapIterator2);
							nativeParallelMultiHashMap = entityGuidToEntity;
							bool flag4 = nativeParallelMultiHashMap.TryGetNextValue(out entity6, ref nativeParallelMultiHashMapIterator3);
							if (flag3)
							{
								Debug.LogWarning("Failed to add a linked child. Multiple instances of the root entity were found in the destination world.");
							}
							else if (flag4)
							{
								Debug.LogWarning("Failed to add a linked child. Multiple instances of the child entity were found in the destination world.");
							}
							else
							{
								EntityManager entityManager2 = entityManager;
								entityManager2.GetBuffer<LinkedEntityGroup>(entity4, false).Add(entity5);
								NativeParallelHashMap<Entity, Entity> nativeParallelHashMap2 = entityToLinkedEntityGroupRoot;
								nativeParallelHashMap2.TryAdd(entity5, entity4);
							}
						}
					}
					num++;
				}
				int i = 0;
				while (i < nativeList.Length)
				{
					EntityPatcher.Child child2 = nativeList[i];
					int num3 = 0;
					for (;;)
					{
						int num4 = num3;
						NativeArray<EntityGuid> nativeArray2 = packedEntityGuids;
						if (num4 >= nativeArray2.Length)
						{
							break;
						}
						nativeArray2 = packedEntityGuids;
						if (nativeArray2[num3].Equals(child2.ChildEntityGuid))
						{
							goto Block_11;
						}
						num3++;
					}
					IL_029F:
					NativeParallelHashMap<Entity, EntityGuid> nativeParallelHashMap3 = entityToEntityGuid;
					nativeParallelHashMap3.TryAdd(child2.ChildEntity, child2.ChildEntityGuid);
					NativeParallelMultiHashMap<EntityGuid, Entity> nativeParallelMultiHashMap = entityGuidToEntity;
					nativeParallelMultiHashMap.Add(child2.ChildEntityGuid, child2.ChildEntity);
					NativeParallelHashMap<Entity, Entity> nativeParallelHashMap2 = entityToLinkedEntityGroupRoot;
					nativeParallelHashMap2.TryAdd(child2.ChildEntity, child2.RootEntity);
					i++;
					continue;
					Block_11:
					NativeParallelMultiHashMap<int, Entity> nativeParallelMultiHashMap2 = packedEntities;
					nativeParallelMultiHashMap2.Add(num3, child2.ChildEntity);
					goto IL_029F;
				}
			}
		}

		// Token: 0x06000378 RID: 888 RVA: 0x00015C88 File Offset: 0x00013E88
		[GenerateTestsForBurstCompatibility]
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void ApplyLinkedEntityGroupRemovals$BurstManaged(in EntityManager entityManager, in NativeArray<LinkedEntityGroupChange> linkedEntityGroupChanges, in NativeArray<EntityGuid> packedEntityGuids, in NativeParallelMultiHashMap<int, Entity> packedEntities, in NativeParallelMultiHashMap<EntityGuid, Entity> entityGuidToEntity, in NativeParallelHashMap<Entity, EntityGuid> entityToEntityGuid, in NativeParallelHashMap<Entity, Entity> entityToLinkedEntityGroupRoot)
		{
			using (NativeList<EntityPatcher.Child> nativeList = new NativeList<EntityPatcher.Child>(Allocator.TempJob))
			{
				int num = 0;
				for (;;)
				{
					int num2 = num;
					NativeArray<LinkedEntityGroupChange> nativeArray = linkedEntityGroupChanges;
					if (num2 >= nativeArray.Length)
					{
						break;
					}
					nativeArray = linkedEntityGroupChanges;
					LinkedEntityGroupChange linkedEntityGroupChange = nativeArray[num];
					NativeParallelMultiHashMap<EntityGuid, Entity> nativeParallelMultiHashMap = entityGuidToEntity;
					Entity entity;
					NativeParallelMultiHashMapIterator<EntityGuid> nativeParallelMultiHashMapIterator;
					if (nativeParallelMultiHashMap.TryGetFirstValue(linkedEntityGroupChange.RootEntityGuid, out entity, out nativeParallelMultiHashMapIterator))
					{
						do
						{
							EntityManager entityManager2 = entityManager;
							DynamicBuffer<LinkedEntityGroup> buffer = entityManager2.GetBuffer<LinkedEntityGroup>(entity, false);
							for (int i = 0; i < buffer.Length; i++)
							{
								Entity value = buffer[i].Value;
								NativeParallelHashMap<Entity, EntityGuid> nativeParallelHashMap = entityToEntityGuid;
								EntityGuid entityGuid;
								if (nativeParallelHashMap.TryGetValue(value, out entityGuid) && entityGuid.Equals(linkedEntityGroupChange.ChildEntityGuid))
								{
									buffer.RemoveAt(i);
									EntityPatcher.Child child = default(EntityPatcher.Child);
									child.RootEntity = entity;
									child.ChildEntity = value;
									child.ChildEntityGuid = linkedEntityGroupChange.ChildEntityGuid;
									nativeList.Add(in child);
									break;
								}
							}
							nativeParallelMultiHashMap = entityGuidToEntity;
						}
						while (nativeParallelMultiHashMap.TryGetNextValue(out entity, ref nativeParallelMultiHashMapIterator));
					}
					num++;
				}
				for (int j = 0; j < nativeList.Length; j++)
				{
					NativeParallelHashMap<Entity, Entity> nativeParallelHashMap2 = entityToLinkedEntityGroupRoot;
					nativeParallelHashMap2.Remove(nativeList[j].ChildEntity);
				}
			}
		}

		// Token: 0x040001E7 RID: 487
		private static string s_ApplyChangeSetProfilerMarkerStr = "EntityPatcher.ApplyChangeSet";

		// Token: 0x040001E8 RID: 488
		private static ProfilerMarker s_ApplyChangeSetProfilerMarker = new ProfilerMarker(EntityPatcher.s_ApplyChangeSetProfilerMarkerStr);

		// Token: 0x040001E9 RID: 489
		private static ProfilerMarker s_BuildEntityLookupsProfilerMarker = new ProfilerMarker("EntityPatcher.BuildEntityLookups");

		// Token: 0x040001EA RID: 490
		private static ProfilerMarker s_BuildPackedLookupsProfilerMarker = new ProfilerMarker("EntityPatcher.BuildPackedLookups");

		// Token: 0x040001EB RID: 491
		private static ProfilerMarker s_ApplyDestroyEntitiesProfilerMarker = new ProfilerMarker("EntityPatcher.ApplyDestroyEntities");

		// Token: 0x040001EC RID: 492
		private static ProfilerMarker s_ApplyCreateEntitiesProfilerMarker = new ProfilerMarker("EntityPatcher.ApplyCreateEntities");

		// Token: 0x040001ED RID: 493
		private static ProfilerMarker s_ApplyEntityNamesProfilerMarker = new ProfilerMarker("EntityPatcher.ApplyEntityNames");

		// Token: 0x040001EE RID: 494
		private static ProfilerMarker s_ApplyRemoveComponentsProfilerMarker = new ProfilerMarker("EntityPatcher.ApplyRemoveComponents");

		// Token: 0x040001EF RID: 495
		private static ProfilerMarker s_ApplyAddComponentsProfilerMarker = new ProfilerMarker("EntityPatcher.ApplyAddComponents");

		// Token: 0x040001F0 RID: 496
		private static ProfilerMarker s_ApplySetSharedComponentsProfilerMarker = new ProfilerMarker("EntityPatcher.ApplySetSharedComponents");

		// Token: 0x040001F1 RID: 497
		private static ProfilerMarker s_ApplySetManagedComponentsProfilerMarker = new ProfilerMarker("EntityPatcher.ApplySetManagedComponents");

		// Token: 0x040001F2 RID: 498
		private static ProfilerMarker s_ApplySetComponentsProfilerMarker = new ProfilerMarker("EntityPatcher.ApplySetComponents");

		// Token: 0x040001F3 RID: 499
		private static ProfilerMarker s_BuildPrefabAndLinkedEntityGroupLookupsProfilerMarker = new ProfilerMarker("EntityPatcher.BuildPrefabAndLinkedEntityGroupLookups");

		// Token: 0x040001F4 RID: 500
		private static ProfilerMarker s_ApplyLinkedEntityGroupRemovalsProfilerMarker = new ProfilerMarker("EntityPatcher.ApplyLinkedEntityGroupRemovals");

		// Token: 0x040001F5 RID: 501
		private static ProfilerMarker s_ApplyLinkedEntityGroupAdditionsProfilerMarker = new ProfilerMarker("EntityPatcher.ApplyLinkedEntityGroupAdditions");

		// Token: 0x040001F6 RID: 502
		private static ProfilerMarker s_ApplyEntityPatchesProfilerMarker = new ProfilerMarker("EntityPatcher.ApplyEntityPatches");

		// Token: 0x040001F7 RID: 503
		private static ProfilerMarker s_ApplyBlobAssetChangesProfilerMarker = new ProfilerMarker("EntityPatcher.ApplyBlobAssetChanges");

		// Token: 0x020000B7 RID: 183
		[BurstCompile]
		private struct BuildComponentToEntityMultiHashMap : IJobChunk
		{
			// Token: 0x06000379 RID: 889 RVA: 0x00015E04 File Offset: 0x00014004
			public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				NativeArray<EntityGuid> nativeArray = chunk.GetNativeArray<EntityGuid>(ref this.ComponentTypeHandle);
				NativeArray<Entity> nativeArray2 = chunk.GetNativeArray(this.EntityTypeHandle);
				for (int num = 0; num != nativeArray2.Length; num++)
				{
					this.ComponentToEntity.Add(nativeArray[num], nativeArray2[num]);
				}
			}

			// Token: 0x0600037A RID: 890 RVA: 0x00015E58 File Offset: 0x00014058
			void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
			}

			// Token: 0x040001F8 RID: 504
			[ReadOnly]
			public ComponentTypeHandle<EntityGuid> ComponentTypeHandle;

			// Token: 0x040001F9 RID: 505
			[ReadOnly]
			public EntityTypeHandle EntityTypeHandle;

			// Token: 0x040001FA RID: 506
			[WriteOnly]
			public NativeParallelMultiHashMap<EntityGuid, Entity>.ParallelWriter ComponentToEntity;
		}

		// Token: 0x020000B8 RID: 184
		[BurstCompile]
		private struct BuildComponentToEntityHashMap : IJobChunk
		{
			// Token: 0x0600037B RID: 891 RVA: 0x00015E68 File Offset: 0x00014068
			public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				NativeArray<EntityGuid> nativeArray = chunk.GetNativeArray<EntityGuid>(ref this.ComponentTypeHandle);
				NativeArray<Entity> nativeArray2 = chunk.GetNativeArray(this.EntityTypeHandle);
				for (int num = 0; num != nativeArray2.Length; num++)
				{
					this.ComponentToEntity.TryAdd(nativeArray[num], nativeArray2[num]);
				}
			}

			// Token: 0x0600037C RID: 892 RVA: 0x00015EBD File Offset: 0x000140BD
			void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
			}

			// Token: 0x040001FB RID: 507
			[ReadOnly]
			public ComponentTypeHandle<EntityGuid> ComponentTypeHandle;

			// Token: 0x040001FC RID: 508
			[ReadOnly]
			public EntityTypeHandle EntityTypeHandle;

			// Token: 0x040001FD RID: 509
			[WriteOnly]
			public NativeParallelHashMap<EntityGuid, Entity>.ParallelWriter ComponentToEntity;
		}

		// Token: 0x020000B9 RID: 185
		[BurstCompile]
		private struct BuildEntityToComponentHashMap : IJobChunk
		{
			// Token: 0x0600037D RID: 893 RVA: 0x00015ECC File Offset: 0x000140CC
			public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				NativeArray<EntityGuid> nativeArray = chunk.GetNativeArray<EntityGuid>(ref this.EntityGuidComponentTypeHandle);
				NativeArray<Entity> nativeArray2 = chunk.GetNativeArray(this.EntityTypeHandle);
				for (int num = 0; num != nativeArray2.Length; num++)
				{
					this.EntityToEntityGuid.TryAdd(nativeArray2[num], nativeArray[num]);
				}
			}

			// Token: 0x0600037E RID: 894 RVA: 0x00015F21 File Offset: 0x00014121
			void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
			}

			// Token: 0x040001FE RID: 510
			[ReadOnly]
			public ComponentTypeHandle<EntityGuid> EntityGuidComponentTypeHandle;

			// Token: 0x040001FF RID: 511
			[ReadOnly]
			public EntityTypeHandle EntityTypeHandle;

			// Token: 0x04000200 RID: 512
			[WriteOnly]
			public NativeParallelHashMap<Entity, EntityGuid>.ParallelWriter EntityToEntityGuid;
		}

		// Token: 0x020000BA RID: 186
		[BurstCompile]
		private struct CalculateLinkedEntityGroupEntitiesLengthJob : IJob
		{
			// Token: 0x0600037F RID: 895 RVA: 0x00015F30 File Offset: 0x00014130
			public unsafe void Execute()
			{
				int num = 0;
				for (int i = 0; i < this.Chunks.Length; i++)
				{
					BufferAccessor<LinkedEntityGroup> bufferAccessor = this.Chunks[i].GetBufferAccessor<LinkedEntityGroup>(ref this.LinkedEntityGroupTypeHandle);
					for (int j = 0; j < bufferAccessor.Length; j++)
					{
						num += bufferAccessor[j].Length;
					}
				}
				*this.Count = num;
			}

			// Token: 0x04000201 RID: 513
			[NativeDisableUnsafePtrRestriction]
			public unsafe int* Count;

			// Token: 0x04000202 RID: 514
			[ReadOnly]
			public NativeArray<ArchetypeChunk> Chunks;

			// Token: 0x04000203 RID: 515
			[ReadOnly]
			public BufferTypeHandle<LinkedEntityGroup> LinkedEntityGroupTypeHandle;
		}

		// Token: 0x020000BB RID: 187
		[BurstCompile]
		private struct BuildLinkedEntityGroupHashMap : IJobChunk
		{
			// Token: 0x06000380 RID: 896 RVA: 0x00015FA4 File Offset: 0x000141A4
			public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				BufferAccessor<LinkedEntityGroup> bufferAccessor = chunk.GetBufferAccessor<LinkedEntityGroup>(ref this.LinkedEntityGroupTypeHandle);
				for (int num = 0; num != bufferAccessor.Length; num++)
				{
					DynamicBuffer<LinkedEntityGroup> dynamicBuffer = bufferAccessor[num];
					for (int num2 = 0; num2 != dynamicBuffer.Length; num2++)
					{
						this.EntityToLinkedEntityGroupRoot.TryAdd(dynamicBuffer[num2].Value, dynamicBuffer[0].Value);
					}
				}
			}

			// Token: 0x06000381 RID: 897 RVA: 0x00016011 File Offset: 0x00014211
			void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
			}

			// Token: 0x04000204 RID: 516
			[WriteOnly]
			public NativeParallelHashMap<Entity, Entity>.ParallelWriter EntityToLinkedEntityGroupRoot;

			// Token: 0x04000205 RID: 517
			[ReadOnly]
			public BufferTypeHandle<LinkedEntityGroup> LinkedEntityGroupTypeHandle;
		}

		// Token: 0x020000BC RID: 188
		[BurstCompile]
		private struct BuildPackedEntityLookupJob : IJobParallelFor
		{
			// Token: 0x06000382 RID: 898 RVA: 0x00016020 File Offset: 0x00014220
			public void Execute(int index)
			{
				EntityGuid entityGuid = this.EntityGuids[index + this.StartIndex];
				Entity entity;
				NativeParallelMultiHashMapIterator<EntityGuid> nativeParallelMultiHashMapIterator;
				if (this.EntityGuidToEntity.TryGetFirstValue(entityGuid, out entity, out nativeParallelMultiHashMapIterator))
				{
					do
					{
						this.PackedEntities.Add(index + this.StartIndex, entity);
					}
					while (this.EntityGuidToEntity.TryGetNextValue(out entity, ref nativeParallelMultiHashMapIterator));
				}
			}

			// Token: 0x04000206 RID: 518
			public int StartIndex;

			// Token: 0x04000207 RID: 519
			[ReadOnly]
			public NativeArray<EntityGuid> EntityGuids;

			// Token: 0x04000208 RID: 520
			[ReadOnly]
			public NativeParallelMultiHashMap<EntityGuid, Entity> EntityGuidToEntity;

			// Token: 0x04000209 RID: 521
			[WriteOnly]
			public NativeParallelMultiHashMap<int, Entity>.ParallelWriter PackedEntities;
		}

		// Token: 0x020000BD RID: 189
		private struct BuildPackedTypeLookupJob : IJobParallelFor
		{
			// Token: 0x06000383 RID: 899 RVA: 0x00016078 File Offset: 0x00014278
			public void Execute(int index)
			{
				ComponentTypeHash componentTypeHash = this.TypeHashes[index];
				Type type = TypeManager.GetType(TypeManager.GetTypeIndexFromStableTypeHash(componentTypeHash.StableTypeHash));
				ComponentType componentType;
				if ((componentTypeHash.Flags & ComponentTypeFlags.ChunkComponent) == ComponentTypeFlags.ChunkComponent)
				{
					componentType = ComponentType.ChunkComponent(type);
				}
				else
				{
					componentType = new ComponentType(type, ComponentType.AccessMode.ReadWrite);
				}
				this.PackedTypes[index] = componentType;
			}

			// Token: 0x0400020A RID: 522
			[ReadOnly]
			public NativeArray<ComponentTypeHash> TypeHashes;

			// Token: 0x0400020B RID: 523
			[WriteOnly]
			public NativeArray<ComponentType> PackedTypes;
		}

		// Token: 0x020000BE RID: 190
		[BurstCompile]
		internal struct ApplyNamesJob : IJob
		{
			// Token: 0x06000384 RID: 900 RVA: 0x000160CC File Offset: 0x000142CC
			[BurstCompile]
			public unsafe void Execute()
			{
				FixedString64Bytes* unsafeReadOnlyPtr = (FixedString64Bytes*)this.Names.GetUnsafeReadOnlyPtr<FixedString64Bytes>();
				for (int i = 0; i < this.CreatedEntityCount; i++)
				{
					FixedString64Bytes fixedString64Bytes = unsafeReadOnlyPtr[i];
					Entity entity;
					NativeParallelMultiHashMapIterator<int> nativeParallelMultiHashMapIterator;
					if (this.PackedEntities.TryGetFirstValue(i, out entity, out nativeParallelMultiHashMapIterator))
					{
						this.EntityManager.SetName(entity, fixedString64Bytes);
					}
				}
				for (int j = 0; j < this.NameChangedCount; j++)
				{
					int num = j + this.CreatedEntityCount;
					FixedString64Bytes fixedString64Bytes2 = unsafeReadOnlyPtr[num];
					EntityGuid entityGuid = this.NameChangeEntityGuids[j];
					Entity entity2;
					NativeParallelMultiHashMapIterator<EntityGuid> nativeParallelMultiHashMapIterator2;
					if (this.EntityGuidToEntity.TryGetFirstValue(entityGuid, out entity2, out nativeParallelMultiHashMapIterator2))
					{
						this.EntityManager.SetName(entity2, fixedString64Bytes2);
					}
				}
			}

			// Token: 0x0400020C RID: 524
			public EntityManager EntityManager;

			// Token: 0x0400020D RID: 525
			public NativeArray<FixedString64Bytes> Names;

			// Token: 0x0400020E RID: 526
			public NativeArray<EntityGuid> NameChangeEntityGuids;

			// Token: 0x0400020F RID: 527
			public int NameChangedCount;

			// Token: 0x04000210 RID: 528
			public int CreatedEntityCount;

			// Token: 0x04000211 RID: 529
			public NativeParallelMultiHashMap<int, Entity> PackedEntities;

			// Token: 0x04000212 RID: 530
			public NativeParallelMultiHashMap<EntityGuid, Entity> EntityGuidToEntity;
		}

		// Token: 0x020000BF RID: 191
		[BurstCompile]
		private struct ApplyAddComponentsJob : IJobFor
		{
			// Token: 0x06000385 RID: 901 RVA: 0x0001618C File Offset: 0x0001438C
			public unsafe void Execute(int index)
			{
				PackedComponent packedComponent = this.AddComponents[index];
				Entity entity;
				NativeParallelMultiHashMapIterator<int> nativeParallelMultiHashMapIterator;
				if (!this.PackedEntities.TryGetFirstValue(packedComponent.PackedEntityIndex, out entity, out nativeParallelMultiHashMapIterator))
				{
					return;
				}
				ComponentType componentType = this.PackedTypes[packedComponent.PackedTypeIndex];
				do
				{
					bool flag;
					if (!this.store->HasComponent(entity, componentType.TypeIndex, out flag))
					{
						this.ecb.AddComponent(index, entity, componentType);
					}
					else
					{
						this.ComponentAlreadyExists.AddNoResize(new EntityPatcher.SetComponentError
						{
							ComponentType = componentType,
							Guid = this.PackedEntityGuids[packedComponent.PackedEntityIndex]
						});
					}
				}
				while (this.PackedEntities.TryGetNextValue(out entity, ref nativeParallelMultiHashMapIterator));
			}

			// Token: 0x04000213 RID: 531
			[NativeDisableUnsafePtrRestriction]
			public unsafe EntityComponentStore* store;

			// Token: 0x04000214 RID: 532
			[ReadOnly]
			public NativeArray<PackedComponent> AddComponents;

			// Token: 0x04000215 RID: 533
			[ReadOnly]
			public NativeArray<EntityGuid> PackedEntityGuids;

			// Token: 0x04000216 RID: 534
			[ReadOnly]
			public NativeParallelMultiHashMap<int, Entity> PackedEntities;

			// Token: 0x04000217 RID: 535
			[ReadOnly]
			public NativeArray<ComponentType> PackedTypes;

			// Token: 0x04000218 RID: 536
			public int LinkedEntityGroupTypeIndex;

			// Token: 0x04000219 RID: 537
			public NativeList<EntityPatcher.SetComponentError>.ParallelWriter ComponentAlreadyExists;

			// Token: 0x0400021A RID: 538
			public EntityCommandBuffer.ParallelWriter ecb;
		}

		// Token: 0x020000C0 RID: 192
		private struct ManagedComponentData
		{
			// Token: 0x06000386 RID: 902 RVA: 0x0001623C File Offset: 0x0001443C
			public ManagedComponentData(int packedIndex, TypeIndex typeIndex, Entity entity)
			{
				this.packedIndex = packedIndex;
				this.typeIndex = typeIndex;
				this.entity = entity;
			}

			// Token: 0x0400021B RID: 539
			public int packedIndex;

			// Token: 0x0400021C RID: 540
			public TypeIndex typeIndex;

			// Token: 0x0400021D RID: 541
			public Entity entity;
		}

		// Token: 0x020000C1 RID: 193
		private struct UnmanagedPackedComponentData
		{
			// Token: 0x0400021E RID: 542
			public int offset;

			// Token: 0x0400021F RID: 543
			public PackedComponent packedComponent;
		}

		// Token: 0x020000C2 RID: 194
		[BurstCompile]
		private struct ApplySetSharedComponentsJob : IJobFor
		{
			// Token: 0x06000387 RID: 903 RVA: 0x00016254 File Offset: 0x00014454
			public unsafe void Execute(int index)
			{
				PackedComponent packedComponent = this.UnmanagedPackedComponents[index].packedComponent;
				Entity entity;
				NativeParallelMultiHashMapIterator<int> nativeParallelMultiHashMapIterator;
				if (!this.PackedEntities.TryGetFirstValue(packedComponent.PackedEntityIndex, out entity, out nativeParallelMultiHashMapIterator))
				{
					return;
				}
				ComponentType componentType = this.PackedTypes[packedComponent.PackedTypeIndex];
				do
				{
					bool flag;
					if (!this.store->HasComponent(entity, componentType, out flag))
					{
						if (flag)
						{
							this.ComponentDoesNotExist.AddNoResize(new EntityPatcher.SetComponentError
							{
								ComponentType = componentType,
								Guid = this.PackedEntityGuids[packedComponent.PackedEntityIndex]
							});
						}
						else
						{
							this.EntityDoesNotExist.AddNoResize(new EntityPatcher.SetComponentError
							{
								ComponentType = componentType,
								Guid = this.PackedEntityGuids[packedComponent.PackedEntityIndex]
							});
						}
					}
					else
					{
						int offset = this.UnmanagedPackedComponents[index].offset;
						if ((offset & -2147483648) != 0)
						{
							this.SetManagedComponents.AddNoResize(new EntityPatcher.ManagedComponentData(index, componentType.TypeIndex, entity));
						}
						else
						{
							byte* ptr = ((offset != int.MaxValue) ? (this.UnmanagedSharedComponentData.Ptr + offset) : null);
							this.ecb.UnsafeSetSharedComponentNonDefault(0, entity, (void*)ptr, componentType.TypeIndex);
						}
					}
				}
				while (this.PackedEntities.TryGetNextValue(out entity, ref nativeParallelMultiHashMapIterator));
			}

			// Token: 0x04000220 RID: 544
			[NativeDisableUnsafePtrRestriction]
			public unsafe EntityComponentStore* store;

			// Token: 0x04000221 RID: 545
			[ReadOnly]
			public NativeArray<EntityPatcher.UnmanagedPackedComponentData> UnmanagedPackedComponents;

			// Token: 0x04000222 RID: 546
			public UnsafeAppendBuffer UnmanagedSharedComponentData;

			// Token: 0x04000223 RID: 547
			[ReadOnly]
			public NativeArray<EntityGuid> PackedEntityGuids;

			// Token: 0x04000224 RID: 548
			[ReadOnly]
			public NativeParallelMultiHashMap<int, Entity> PackedEntities;

			// Token: 0x04000225 RID: 549
			[ReadOnly]
			public NativeArray<ComponentType> PackedTypes;

			// Token: 0x04000226 RID: 550
			public NativeList<EntityPatcher.SetComponentError>.ParallelWriter EntityDoesNotExist;

			// Token: 0x04000227 RID: 551
			public NativeList<EntityPatcher.SetComponentError>.ParallelWriter ComponentDoesNotExist;

			// Token: 0x04000228 RID: 552
			public NativeList<EntityPatcher.ManagedComponentData>.ParallelWriter SetManagedComponents;

			// Token: 0x04000229 RID: 553
			public EntityCommandBuffer.ParallelWriter ecb;
		}

		// Token: 0x020000C3 RID: 195
		private struct SetComponentError
		{
			// Token: 0x0400022A RID: 554
			public ComponentType ComponentType;

			// Token: 0x0400022B RID: 555
			public EntityGuid Guid;
		}

		// Token: 0x020000C4 RID: 196
		[BurstCompile]
		private struct ApplySetComponentsJob : IJob
		{
			// Token: 0x06000388 RID: 904 RVA: 0x000163A0 File Offset: 0x000145A0
			public unsafe void Execute()
			{
				TypeIndex typeIndex = TypeManager.GetTypeIndex<EntityGuid>();
				TypeIndex companionLinkTypeIndex = ManagedComponentStore.CompanionLinkTypeIndex;
				long num = 0L;
				for (int i = 0; i < this.Changes.Length; i++)
				{
					PackedComponentDataChange packedComponentDataChange = this.Changes[i];
					PackedComponent component = packedComponentDataChange.Component;
					ComponentType componentType = this.PackedTypes[component.PackedTypeIndex];
					int size = packedComponentDataChange.Size;
					byte* ptr = (byte*)this.Payload.GetUnsafeReadOnlyPtr<byte>() + num;
					ComponentTypeInArchetype componentTypeInArchetype = new ComponentTypeInArchetype(componentType);
					Entity entity;
					NativeParallelMultiHashMapIterator<int> nativeParallelMultiHashMapIterator;
					if (this.PackedEntities.TryGetFirstValue(component.PackedEntityIndex, out entity, out nativeParallelMultiHashMapIterator))
					{
						do
						{
							if (!this.EntityManager.Exists(entity))
							{
								EntityPatcher.SetComponentError setComponentError = default(EntityPatcher.SetComponentError);
								setComponentError.ComponentType = componentType;
								setComponentError.Guid = this.PackedEntityGuids[component.PackedEntityIndex];
								this.EntityDoesNotExist.Add(in setComponentError);
							}
							else if (!this.EntityManager.HasComponent(entity, componentType))
							{
								EntityPatcher.SetComponentError setComponentError = default(EntityPatcher.SetComponentError);
								setComponentError.ComponentType = componentType;
								setComponentError.Guid = this.PackedEntityGuids[component.PackedEntityIndex];
								this.ComponentDoesNotExist.Add(in setComponentError);
							}
							else
							{
								if (packedComponentDataChange.Enabled > -1)
								{
									this.EntityManager.SetEnableableComponent(entity, componentType.TypeIndex, packedComponentDataChange.Enabled == 1);
								}
								if (!componentTypeInArchetype.IsZeroSized)
								{
									if (componentTypeInArchetype.IsBuffer)
									{
										int elementSize = TypeManager.GetTypeInfo(componentTypeInArchetype.TypeIndex).ElementSize;
										int num2 = size / elementSize;
										BufferHeader* componentDataRawRW = (BufferHeader*)this.EntityManager.GetComponentDataRawRW(entity, componentType.TypeIndex);
										BufferHeader.Assign(componentDataRawRW, ptr, num2, elementSize, 16, false, 0);
									}
									else
									{
										byte* componentDataRawRW2 = (byte*)this.EntityManager.GetComponentDataRawRW(entity, componentType.TypeIndex);
										if (componentTypeInArchetype.TypeIndex == typeIndex)
										{
											EntityGuid entityGuid;
											UnsafeUtility.MemCpy((void*)(&entityGuid), (void*)componentDataRawRW2, (long)sizeof(EntityGuid));
											if (!entityGuid.Equals(default(EntityGuid)))
											{
												this.EntityGuidToEntity.Remove(entityGuid, entity);
											}
											UnsafeUtility.MemCpy((void*)(&entityGuid), (void*)(ptr + packedComponentDataChange.Offset), (long)size);
											this.EntityGuidToEntity.Add(entityGuid, entity);
											this.EntityToEntityGuid.TryAdd(entity, entityGuid);
										}
										if (componentTypeInArchetype.TypeIndex == companionLinkTypeIndex)
										{
											this.EntitiesWithCompanionLink.Add(in entity);
										}
										UnsafeUtility.MemCpy((void*)(componentDataRawRW2 + packedComponentDataChange.Offset), (void*)ptr, (long)size);
									}
								}
							}
						}
						while (this.PackedEntities.TryGetNextValue(out entity, ref nativeParallelMultiHashMapIterator));
					}
					num += (long)size;
				}
			}

			// Token: 0x0400022C RID: 556
			public EntityManager EntityManager;

			// Token: 0x0400022D RID: 557
			[ReadOnly]
			public NativeArray<PackedComponentDataChange> Changes;

			// Token: 0x0400022E RID: 558
			[ReadOnly]
			public NativeArray<byte> Payload;

			// Token: 0x0400022F RID: 559
			[ReadOnly]
			public NativeArray<EntityGuid> PackedEntityGuids;

			// Token: 0x04000230 RID: 560
			[ReadOnly]
			public NativeParallelMultiHashMap<int, Entity> PackedEntities;

			// Token: 0x04000231 RID: 561
			[ReadOnly]
			public NativeArray<ComponentType> PackedTypes;

			// Token: 0x04000232 RID: 562
			public NativeParallelMultiHashMap<EntityGuid, Entity> EntityGuidToEntity;

			// Token: 0x04000233 RID: 563
			public NativeParallelHashMap<Entity, EntityGuid> EntityToEntityGuid;

			// Token: 0x04000234 RID: 564
			public NativeList<EntityPatcher.SetComponentError> EntityDoesNotExist;

			// Token: 0x04000235 RID: 565
			public NativeList<EntityPatcher.SetComponentError> ComponentDoesNotExist;

			// Token: 0x04000236 RID: 566
			public NativeList<Entity> EntitiesWithCompanionLink;
		}

		// Token: 0x020000C5 RID: 197
		internal struct ManagedObjectEntityReferencePatch
		{
			// Token: 0x04000237 RID: 567
			public int Id;

			// Token: 0x04000238 RID: 568
			public Entity TargetEntity;
		}

		// Token: 0x020000C6 RID: 198
		internal struct ManagedObjectBlobAssetReferencePatch
		{
			// Token: 0x04000239 RID: 569
			public int Id;

			// Token: 0x0400023A RID: 570
			public ulong Target;
		}

		// Token: 0x020000C7 RID: 199
		internal struct EntityComponentPair : IEquatable<EntityPatcher.EntityComponentPair>, IComparable<EntityPatcher.EntityComponentPair>
		{
			// Token: 0x06000389 RID: 905 RVA: 0x00016631 File Offset: 0x00014831
			public bool Equals(EntityPatcher.EntityComponentPair other)
			{
				return this.Entity == other.Entity && this.Component == other.Component;
			}

			// Token: 0x0600038A RID: 906 RVA: 0x0001665C File Offset: 0x0001485C
			public int CompareTo(EntityPatcher.EntityComponentPair other)
			{
				int num = this.Entity.CompareTo(other.Entity);
				if (num != 0)
				{
					return num;
				}
				return this.Component.TypeIndex.CompareTo(other.Component.TypeIndex);
			}

			// Token: 0x0400023B RID: 571
			public Entity Entity;

			// Token: 0x0400023C RID: 572
			public ComponentType Component;
		}

		// Token: 0x020000C8 RID: 200
		internal struct EntityTargetPair
		{
			// Token: 0x0600038B RID: 907 RVA: 0x0001669D File Offset: 0x0001489D
			public EntityTargetPair(Entity entity, Entity targetEntity)
			{
				this.Entity = entity;
				this.TargetEntity = targetEntity;
			}

			// Token: 0x0400023D RID: 573
			public Entity Entity;

			// Token: 0x0400023E RID: 574
			public Entity TargetEntity;
		}

		// Token: 0x020000C9 RID: 201
		private struct Child
		{
			// Token: 0x0400023F RID: 575
			public Entity RootEntity;

			// Token: 0x04000240 RID: 576
			public Entity ChildEntity;

			// Token: 0x04000241 RID: 577
			public EntityGuid ChildEntityGuid;
		}

		// Token: 0x020000CA RID: 202
		private class ManagedObjectEntityReferencePatcher : PropertyVisitor, IVisitPropertyAdapter<Entity>, IPropertyVisitorAdapter
		{
			// Token: 0x0600038C RID: 908 RVA: 0x000166AD File Offset: 0x000148AD
			public ManagedObjectEntityReferencePatcher()
			{
				base.AddAdapter(this);
			}

			// Token: 0x0600038D RID: 909 RVA: 0x000166BC File Offset: 0x000148BC
			public void ApplyPatches(ref object obj, NativeParallelMultiHashMap<EntityPatcher.EntityComponentPair, EntityPatcher.ManagedObjectEntityReferencePatch>.Enumerator patches)
			{
				this.Patches = patches;
				PropertyContainer.Accept<object>(this, ref obj, default(VisitParameters));
			}

			// Token: 0x0600038E RID: 910 RVA: 0x000166E0 File Offset: 0x000148E0
			void IVisitPropertyAdapter<Entity>.Visit<TContainer>(in VisitContext<TContainer, Entity> context, ref TContainer container, ref Entity value)
			{
				NativeParallelMultiHashMap<EntityPatcher.EntityComponentPair, EntityPatcher.ManagedObjectEntityReferencePatch>.Enumerator patches = this.Patches;
				foreach (EntityPatcher.ManagedObjectEntityReferencePatch managedObjectEntityReferencePatch in patches)
				{
					if (value.Index == managedObjectEntityReferencePatch.Id)
					{
						value = managedObjectEntityReferencePatch.TargetEntity;
						break;
					}
				}
			}

			// Token: 0x04000242 RID: 578
			private NativeParallelMultiHashMap<EntityPatcher.EntityComponentPair, EntityPatcher.ManagedObjectEntityReferencePatch>.Enumerator Patches;
		}

		// Token: 0x020000CB RID: 203
		private class ManagedObjectBlobAssetReferencePatcher : PropertyVisitor, IVisitPropertyAdapter<BlobAssetReferenceData>, IPropertyVisitorAdapter
		{
			// Token: 0x0600038F RID: 911 RVA: 0x0001674C File Offset: 0x0001494C
			public ManagedObjectBlobAssetReferencePatcher(EntityPatcherBlobAssetSystem entityPatcherBlobAssetSystem)
			{
				this.m_EntityPatcherBlobAssetSystem = entityPatcherBlobAssetSystem;
				base.AddAdapter(this);
			}

			// Token: 0x06000390 RID: 912 RVA: 0x00016764 File Offset: 0x00014964
			public void ApplyPatches(ref object obj, NativeParallelMultiHashMap<EntityPatcher.EntityComponentPair, EntityPatcher.ManagedObjectBlobAssetReferencePatch>.Enumerator patches)
			{
				this.Patches = patches;
				PropertyContainer.Accept<object>(this, ref obj, default(VisitParameters));
			}

			// Token: 0x06000391 RID: 913 RVA: 0x00016788 File Offset: 0x00014988
			unsafe void IVisitPropertyAdapter<BlobAssetReferenceData>.Visit<TContainer>(in VisitContext<TContainer, BlobAssetReferenceData> context, ref TContainer container, ref BlobAssetReferenceData value)
			{
				NativeParallelMultiHashMap<EntityPatcher.EntityComponentPair, EntityPatcher.ManagedObjectBlobAssetReferencePatch>.Enumerator patches = this.Patches;
				foreach (EntityPatcher.ManagedObjectBlobAssetReferencePatch managedObjectBlobAssetReferencePatch in patches)
				{
					if (value.m_Align8Union == (long)managedObjectBlobAssetReferencePatch.Id)
					{
						BlobAssetPtr blobAssetPtr;
						if (this.m_EntityPatcherBlobAssetSystem.TryGetBlobAsset(managedObjectBlobAssetReferencePatch.Target, out blobAssetPtr))
						{
							value = new BlobAssetReferenceData
							{
								m_Ptr = (byte*)blobAssetPtr.Data
							};
							break;
						}
						value = default(BlobAssetReferenceData);
						break;
					}
				}
			}

			// Token: 0x04000243 RID: 579
			private EntityPatcherBlobAssetSystem m_EntityPatcherBlobAssetSystem;

			// Token: 0x04000244 RID: 580
			private NativeParallelMultiHashMap<EntityPatcher.EntityComponentPair, EntityPatcher.ManagedObjectBlobAssetReferencePatch>.Enumerator Patches;
		}

		// Token: 0x020000CC RID: 204
		// (Invoke) Token: 0x06000393 RID: 915
		internal delegate void ApplyCreateEntitiesWithArchetypes_0000032C$PostfixBurstDelegate(in EntityManager entityManager, in NativeParallelMultiHashMap<int, Entity> packedEntities, in NativeArray<FilteredArchetype> createdArchetypes);

		// Token: 0x020000CD RID: 205
		internal static class ApplyCreateEntitiesWithArchetypes_0000032C$BurstDirectCall
		{
			// Token: 0x06000396 RID: 918 RVA: 0x00016824 File Offset: 0x00014A24
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (EntityPatcher.ApplyCreateEntitiesWithArchetypes_0000032C$BurstDirectCall.Pointer == 0)
				{
					EntityPatcher.ApplyCreateEntitiesWithArchetypes_0000032C$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(EntityPatcher.ApplyCreateEntitiesWithArchetypes_0000032C$BurstDirectCall.DeferredCompilation, methodof(EntityPatcher.ApplyCreateEntitiesWithArchetypes$BurstManaged(ref EntityManager, ref NativeParallelMultiHashMap<int, Entity>, ref NativeArray<FilteredArchetype>)).MethodHandle, typeof(EntityPatcher.ApplyCreateEntitiesWithArchetypes_0000032C$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = EntityPatcher.ApplyCreateEntitiesWithArchetypes_0000032C$BurstDirectCall.Pointer;
			}

			// Token: 0x06000397 RID: 919 RVA: 0x00016850 File Offset: 0x00014A50
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				EntityPatcher.ApplyCreateEntitiesWithArchetypes_0000032C$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000398 RID: 920 RVA: 0x00016868 File Offset: 0x00014A68
			public static void Constructor()
			{
				EntityPatcher.ApplyCreateEntitiesWithArchetypes_0000032C$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(EntityPatcher.ApplyCreateEntitiesWithArchetypes(ref EntityManager, ref NativeParallelMultiHashMap<int, Entity>, ref NativeArray<FilteredArchetype>)).MethodHandle);
			}

			// Token: 0x06000399 RID: 921 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x0600039A RID: 922 RVA: 0x00016879 File Offset: 0x00014A79
			// Note: this type is marked as 'beforefieldinit'.
			static ApplyCreateEntitiesWithArchetypes_0000032C$BurstDirectCall()
			{
				EntityPatcher.ApplyCreateEntitiesWithArchetypes_0000032C$BurstDirectCall.Constructor();
			}

			// Token: 0x0600039B RID: 923 RVA: 0x00016880 File Offset: 0x00014A80
			public static void Invoke(in EntityManager entityManager, in NativeParallelMultiHashMap<int, Entity> packedEntities, in NativeArray<FilteredArchetype> createdArchetypes)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = EntityPatcher.ApplyCreateEntitiesWithArchetypes_0000032C$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.EntityManager&,Unity.Collections.NativeParallelMultiHashMap`2<System.Int32,Unity.Entities.Entity>&,Unity.Collections.NativeArray`1<Unity.Entities.FilteredArchetype>&), ref entityManager, ref packedEntities, ref createdArchetypes, functionPointer);
						return;
					}
				}
				EntityPatcher.ApplyCreateEntitiesWithArchetypes$BurstManaged(in entityManager, in packedEntities, in createdArchetypes);
			}

			// Token: 0x04000245 RID: 581
			private static IntPtr Pointer;

			// Token: 0x04000246 RID: 582
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020000CE RID: 206
		// (Invoke) Token: 0x0600039D RID: 925
		internal delegate void ApplyDestroyEntities_0000032E$PostfixBurstDelegate(in EntityManager entityManager, in NativeArray<EntityGuid> entities, in NativeParallelMultiHashMap<int, Entity> packedEntities, in NativeParallelMultiHashMap<EntityGuid, Entity> entityGuidToEntity, int destroyedEntityCount);

		// Token: 0x020000CF RID: 207
		internal static class ApplyDestroyEntities_0000032E$BurstDirectCall
		{
			// Token: 0x060003A0 RID: 928 RVA: 0x000168B5 File Offset: 0x00014AB5
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (EntityPatcher.ApplyDestroyEntities_0000032E$BurstDirectCall.Pointer == 0)
				{
					EntityPatcher.ApplyDestroyEntities_0000032E$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(EntityPatcher.ApplyDestroyEntities_0000032E$BurstDirectCall.DeferredCompilation, methodof(EntityPatcher.ApplyDestroyEntities$BurstManaged(ref EntityManager, ref NativeArray<EntityGuid>, ref NativeParallelMultiHashMap<int, Entity>, ref NativeParallelMultiHashMap<EntityGuid, Entity>, int)).MethodHandle, typeof(EntityPatcher.ApplyDestroyEntities_0000032E$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = EntityPatcher.ApplyDestroyEntities_0000032E$BurstDirectCall.Pointer;
			}

			// Token: 0x060003A1 RID: 929 RVA: 0x000168E4 File Offset: 0x00014AE4
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				EntityPatcher.ApplyDestroyEntities_0000032E$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x060003A2 RID: 930 RVA: 0x000168FC File Offset: 0x00014AFC
			public static void Constructor()
			{
				EntityPatcher.ApplyDestroyEntities_0000032E$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(EntityPatcher.ApplyDestroyEntities(ref EntityManager, ref NativeArray<EntityGuid>, ref NativeParallelMultiHashMap<int, Entity>, ref NativeParallelMultiHashMap<EntityGuid, Entity>, int)).MethodHandle);
			}

			// Token: 0x060003A3 RID: 931 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x060003A4 RID: 932 RVA: 0x0001690D File Offset: 0x00014B0D
			// Note: this type is marked as 'beforefieldinit'.
			static ApplyDestroyEntities_0000032E$BurstDirectCall()
			{
				EntityPatcher.ApplyDestroyEntities_0000032E$BurstDirectCall.Constructor();
			}

			// Token: 0x060003A5 RID: 933 RVA: 0x00016914 File Offset: 0x00014B14
			public static void Invoke(in EntityManager entityManager, in NativeArray<EntityGuid> entities, in NativeParallelMultiHashMap<int, Entity> packedEntities, in NativeParallelMultiHashMap<EntityGuid, Entity> entityGuidToEntity, int destroyedEntityCount)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = EntityPatcher.ApplyDestroyEntities_0000032E$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.EntityManager&,Unity.Collections.NativeArray`1<Unity.Entities.EntityGuid>&,Unity.Collections.NativeParallelMultiHashMap`2<System.Int32,Unity.Entities.Entity>&,Unity.Collections.NativeParallelMultiHashMap`2<Unity.Entities.EntityGuid,Unity.Entities.Entity>&,System.Int32), ref entityManager, ref entities, ref packedEntities, ref entityGuidToEntity, destroyedEntityCount, functionPointer);
						return;
					}
				}
				EntityPatcher.ApplyDestroyEntities$BurstManaged(in entityManager, in entities, in packedEntities, in entityGuidToEntity, destroyedEntityCount);
			}

			// Token: 0x04000247 RID: 583
			private static IntPtr Pointer;

			// Token: 0x04000248 RID: 584
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020000D0 RID: 208
		// (Invoke) Token: 0x060003A7 RID: 935
		internal delegate void ApplyRemoveComponents_00000330$PostfixBurstDelegate(in EntityManager entityManager, in NativeArray<PackedComponent> removeComponents, in NativeArray<EntityGuid> packedEntityGuids, in NativeParallelMultiHashMap<int, Entity> packedEntities, in NativeArray<ComponentType> packedTypes);

		// Token: 0x020000D1 RID: 209
		internal static class ApplyRemoveComponents_00000330$BurstDirectCall
		{
			// Token: 0x060003AA RID: 938 RVA: 0x0001694F File Offset: 0x00014B4F
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (EntityPatcher.ApplyRemoveComponents_00000330$BurstDirectCall.Pointer == 0)
				{
					EntityPatcher.ApplyRemoveComponents_00000330$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(EntityPatcher.ApplyRemoveComponents_00000330$BurstDirectCall.DeferredCompilation, methodof(EntityPatcher.ApplyRemoveComponents$BurstManaged(ref EntityManager, ref NativeArray<PackedComponent>, ref NativeArray<EntityGuid>, ref NativeParallelMultiHashMap<int, Entity>, ref NativeArray<ComponentType>)).MethodHandle, typeof(EntityPatcher.ApplyRemoveComponents_00000330$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = EntityPatcher.ApplyRemoveComponents_00000330$BurstDirectCall.Pointer;
			}

			// Token: 0x060003AB RID: 939 RVA: 0x0001697C File Offset: 0x00014B7C
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				EntityPatcher.ApplyRemoveComponents_00000330$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x060003AC RID: 940 RVA: 0x00016994 File Offset: 0x00014B94
			public static void Constructor()
			{
				EntityPatcher.ApplyRemoveComponents_00000330$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(EntityPatcher.ApplyRemoveComponents(ref EntityManager, ref NativeArray<PackedComponent>, ref NativeArray<EntityGuid>, ref NativeParallelMultiHashMap<int, Entity>, ref NativeArray<ComponentType>)).MethodHandle);
			}

			// Token: 0x060003AD RID: 941 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x060003AE RID: 942 RVA: 0x000169A5 File Offset: 0x00014BA5
			// Note: this type is marked as 'beforefieldinit'.
			static ApplyRemoveComponents_00000330$BurstDirectCall()
			{
				EntityPatcher.ApplyRemoveComponents_00000330$BurstDirectCall.Constructor();
			}

			// Token: 0x060003AF RID: 943 RVA: 0x000169AC File Offset: 0x00014BAC
			public static void Invoke(in EntityManager entityManager, in NativeArray<PackedComponent> removeComponents, in NativeArray<EntityGuid> packedEntityGuids, in NativeParallelMultiHashMap<int, Entity> packedEntities, in NativeArray<ComponentType> packedTypes)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = EntityPatcher.ApplyRemoveComponents_00000330$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.EntityManager&,Unity.Collections.NativeArray`1<Unity.Entities.PackedComponent>&,Unity.Collections.NativeArray`1<Unity.Entities.EntityGuid>&,Unity.Collections.NativeParallelMultiHashMap`2<System.Int32,Unity.Entities.Entity>&,Unity.Collections.NativeArray`1<Unity.Entities.ComponentType>&), ref entityManager, ref removeComponents, ref packedEntityGuids, ref packedEntities, ref packedTypes, functionPointer);
						return;
					}
				}
				EntityPatcher.ApplyRemoveComponents$BurstManaged(in entityManager, in removeComponents, in packedEntityGuids, in packedEntities, in packedTypes);
			}

			// Token: 0x04000249 RID: 585
			private static IntPtr Pointer;

			// Token: 0x0400024A RID: 586
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020000D2 RID: 210
		// (Invoke) Token: 0x060003B1 RID: 945
		internal delegate void ApplyUnmanagedEntityPatches_00000334$PostfixBurstDelegate(in EntityManager entityManager, in NativeArray<EntityReferenceChange> changes, in NativeArray<EntityGuid> packedEntityGuids, in NativeParallelMultiHashMap<int, Entity> packedEntities, in NativeArray<ComponentType> packedTypes, in NativeParallelMultiHashMap<EntityGuid, Entity> entityGuidToEntity, in NativeParallelHashMap<Entity, EntityGuid> entityToEntityGuid, in NativeParallelHashMap<EntityGuid, Entity> entityGuidToPrefab, in NativeParallelHashMap<Entity, Entity> entityToLinkedEntityGroupRoot, in NativeParallelMultiHashMap<int, EntityPatcher.EntityTargetPair> entityTargets);

		// Token: 0x020000D3 RID: 211
		internal static class ApplyUnmanagedEntityPatches_00000334$BurstDirectCall
		{
			// Token: 0x060003B4 RID: 948 RVA: 0x000169E7 File Offset: 0x00014BE7
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (EntityPatcher.ApplyUnmanagedEntityPatches_00000334$BurstDirectCall.Pointer == 0)
				{
					EntityPatcher.ApplyUnmanagedEntityPatches_00000334$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(EntityPatcher.ApplyUnmanagedEntityPatches_00000334$BurstDirectCall.DeferredCompilation, methodof(EntityPatcher.ApplyUnmanagedEntityPatches$BurstManaged(ref EntityManager, ref NativeArray<EntityReferenceChange>, ref NativeArray<EntityGuid>, ref NativeParallelMultiHashMap<int, Entity>, ref NativeArray<ComponentType>, ref NativeParallelMultiHashMap<EntityGuid, Entity>, ref NativeParallelHashMap<Entity, EntityGuid>, ref NativeParallelHashMap<EntityGuid, Entity>, ref NativeParallelHashMap<Entity, Entity>, ref NativeParallelMultiHashMap<int, EntityPatcher.EntityTargetPair>)).MethodHandle, typeof(EntityPatcher.ApplyUnmanagedEntityPatches_00000334$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = EntityPatcher.ApplyUnmanagedEntityPatches_00000334$BurstDirectCall.Pointer;
			}

			// Token: 0x060003B5 RID: 949 RVA: 0x00016A14 File Offset: 0x00014C14
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				EntityPatcher.ApplyUnmanagedEntityPatches_00000334$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x060003B6 RID: 950 RVA: 0x00016A2C File Offset: 0x00014C2C
			public static void Constructor()
			{
				EntityPatcher.ApplyUnmanagedEntityPatches_00000334$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(EntityPatcher.ApplyUnmanagedEntityPatches(ref EntityManager, ref NativeArray<EntityReferenceChange>, ref NativeArray<EntityGuid>, ref NativeParallelMultiHashMap<int, Entity>, ref NativeArray<ComponentType>, ref NativeParallelMultiHashMap<EntityGuid, Entity>, ref NativeParallelHashMap<Entity, EntityGuid>, ref NativeParallelHashMap<EntityGuid, Entity>, ref NativeParallelHashMap<Entity, Entity>, ref NativeParallelMultiHashMap<int, EntityPatcher.EntityTargetPair>)).MethodHandle);
			}

			// Token: 0x060003B7 RID: 951 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x060003B8 RID: 952 RVA: 0x00016A3D File Offset: 0x00014C3D
			// Note: this type is marked as 'beforefieldinit'.
			static ApplyUnmanagedEntityPatches_00000334$BurstDirectCall()
			{
				EntityPatcher.ApplyUnmanagedEntityPatches_00000334$BurstDirectCall.Constructor();
			}

			// Token: 0x060003B9 RID: 953 RVA: 0x00016A44 File Offset: 0x00014C44
			public static void Invoke(in EntityManager entityManager, in NativeArray<EntityReferenceChange> changes, in NativeArray<EntityGuid> packedEntityGuids, in NativeParallelMultiHashMap<int, Entity> packedEntities, in NativeArray<ComponentType> packedTypes, in NativeParallelMultiHashMap<EntityGuid, Entity> entityGuidToEntity, in NativeParallelHashMap<Entity, EntityGuid> entityToEntityGuid, in NativeParallelHashMap<EntityGuid, Entity> entityGuidToPrefab, in NativeParallelHashMap<Entity, Entity> entityToLinkedEntityGroupRoot, in NativeParallelMultiHashMap<int, EntityPatcher.EntityTargetPair> entityTargets)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = EntityPatcher.ApplyUnmanagedEntityPatches_00000334$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.EntityManager&,Unity.Collections.NativeArray`1<Unity.Entities.EntityReferenceChange>&,Unity.Collections.NativeArray`1<Unity.Entities.EntityGuid>&,Unity.Collections.NativeParallelMultiHashMap`2<System.Int32,Unity.Entities.Entity>&,Unity.Collections.NativeArray`1<Unity.Entities.ComponentType>&,Unity.Collections.NativeParallelMultiHashMap`2<Unity.Entities.EntityGuid,Unity.Entities.Entity>&,Unity.Collections.NativeParallelHashMap`2<Unity.Entities.Entity,Unity.Entities.EntityGuid>&,Unity.Collections.NativeParallelHashMap`2<Unity.Entities.EntityGuid,Unity.Entities.Entity>&,Unity.Collections.NativeParallelHashMap`2<Unity.Entities.Entity,Unity.Entities.Entity>&,Unity.Collections.NativeParallelMultiHashMap`2<System.Int32,Unity.Entities.EntityPatcher/EntityTargetPair>&), ref entityManager, ref changes, ref packedEntityGuids, ref packedEntities, ref packedTypes, ref entityGuidToEntity, ref entityToEntityGuid, ref entityGuidToPrefab, ref entityToLinkedEntityGroupRoot, ref entityTargets, functionPointer);
						return;
					}
				}
				EntityPatcher.ApplyUnmanagedEntityPatches$BurstManaged(in entityManager, in changes, in packedEntityGuids, in packedEntities, in packedTypes, in entityGuidToEntity, in entityToEntityGuid, in entityGuidToPrefab, in entityToLinkedEntityGroupRoot, in entityTargets);
			}

			// Token: 0x0400024B RID: 587
			private static IntPtr Pointer;

			// Token: 0x0400024C RID: 588
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020000D4 RID: 212
		// (Invoke) Token: 0x060003BB RID: 955
		internal delegate void ApplyLinkedEntityGroupAdditions_00000337$PostfixBurstDelegate(in EntityManager entityManager, in NativeArray<LinkedEntityGroupChange> linkedEntityGroupChanges, in NativeArray<EntityGuid> packedEntityGuids, in NativeParallelMultiHashMap<int, Entity> packedEntities, in NativeParallelMultiHashMap<EntityGuid, Entity> entityGuidToEntity, in NativeParallelHashMap<Entity, EntityGuid> entityToEntityGuid, in NativeParallelHashMap<EntityGuid, Entity> entityGuidToPrefab, in NativeParallelHashMap<Entity, Entity> entityToLinkedEntityGroupRoot);

		// Token: 0x020000D5 RID: 213
		internal static class ApplyLinkedEntityGroupAdditions_00000337$BurstDirectCall
		{
			// Token: 0x060003BE RID: 958 RVA: 0x00016A93 File Offset: 0x00014C93
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (EntityPatcher.ApplyLinkedEntityGroupAdditions_00000337$BurstDirectCall.Pointer == 0)
				{
					EntityPatcher.ApplyLinkedEntityGroupAdditions_00000337$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(EntityPatcher.ApplyLinkedEntityGroupAdditions_00000337$BurstDirectCall.DeferredCompilation, methodof(EntityPatcher.ApplyLinkedEntityGroupAdditions$BurstManaged(ref EntityManager, ref NativeArray<LinkedEntityGroupChange>, ref NativeArray<EntityGuid>, ref NativeParallelMultiHashMap<int, Entity>, ref NativeParallelMultiHashMap<EntityGuid, Entity>, ref NativeParallelHashMap<Entity, EntityGuid>, ref NativeParallelHashMap<EntityGuid, Entity>, ref NativeParallelHashMap<Entity, Entity>)).MethodHandle, typeof(EntityPatcher.ApplyLinkedEntityGroupAdditions_00000337$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = EntityPatcher.ApplyLinkedEntityGroupAdditions_00000337$BurstDirectCall.Pointer;
			}

			// Token: 0x060003BF RID: 959 RVA: 0x00016AC0 File Offset: 0x00014CC0
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				EntityPatcher.ApplyLinkedEntityGroupAdditions_00000337$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x060003C0 RID: 960 RVA: 0x00016AD8 File Offset: 0x00014CD8
			public static void Constructor()
			{
				EntityPatcher.ApplyLinkedEntityGroupAdditions_00000337$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(EntityPatcher.ApplyLinkedEntityGroupAdditions(ref EntityManager, ref NativeArray<LinkedEntityGroupChange>, ref NativeArray<EntityGuid>, ref NativeParallelMultiHashMap<int, Entity>, ref NativeParallelMultiHashMap<EntityGuid, Entity>, ref NativeParallelHashMap<Entity, EntityGuid>, ref NativeParallelHashMap<EntityGuid, Entity>, ref NativeParallelHashMap<Entity, Entity>)).MethodHandle);
			}

			// Token: 0x060003C1 RID: 961 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x060003C2 RID: 962 RVA: 0x00016AE9 File Offset: 0x00014CE9
			// Note: this type is marked as 'beforefieldinit'.
			static ApplyLinkedEntityGroupAdditions_00000337$BurstDirectCall()
			{
				EntityPatcher.ApplyLinkedEntityGroupAdditions_00000337$BurstDirectCall.Constructor();
			}

			// Token: 0x060003C3 RID: 963 RVA: 0x00016AF0 File Offset: 0x00014CF0
			public static void Invoke(in EntityManager entityManager, in NativeArray<LinkedEntityGroupChange> linkedEntityGroupChanges, in NativeArray<EntityGuid> packedEntityGuids, in NativeParallelMultiHashMap<int, Entity> packedEntities, in NativeParallelMultiHashMap<EntityGuid, Entity> entityGuidToEntity, in NativeParallelHashMap<Entity, EntityGuid> entityToEntityGuid, in NativeParallelHashMap<EntityGuid, Entity> entityGuidToPrefab, in NativeParallelHashMap<Entity, Entity> entityToLinkedEntityGroupRoot)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = EntityPatcher.ApplyLinkedEntityGroupAdditions_00000337$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.EntityManager&,Unity.Collections.NativeArray`1<Unity.Entities.LinkedEntityGroupChange>&,Unity.Collections.NativeArray`1<Unity.Entities.EntityGuid>&,Unity.Collections.NativeParallelMultiHashMap`2<System.Int32,Unity.Entities.Entity>&,Unity.Collections.NativeParallelMultiHashMap`2<Unity.Entities.EntityGuid,Unity.Entities.Entity>&,Unity.Collections.NativeParallelHashMap`2<Unity.Entities.Entity,Unity.Entities.EntityGuid>&,Unity.Collections.NativeParallelHashMap`2<Unity.Entities.EntityGuid,Unity.Entities.Entity>&,Unity.Collections.NativeParallelHashMap`2<Unity.Entities.Entity,Unity.Entities.Entity>&), ref entityManager, ref linkedEntityGroupChanges, ref packedEntityGuids, ref packedEntities, ref entityGuidToEntity, ref entityToEntityGuid, ref entityGuidToPrefab, ref entityToLinkedEntityGroupRoot, functionPointer);
						return;
					}
				}
				EntityPatcher.ApplyLinkedEntityGroupAdditions$BurstManaged(in entityManager, in linkedEntityGroupChanges, in packedEntityGuids, in packedEntities, in entityGuidToEntity, in entityToEntityGuid, in entityGuidToPrefab, in entityToLinkedEntityGroupRoot);
			}

			// Token: 0x0400024D RID: 589
			private static IntPtr Pointer;

			// Token: 0x0400024E RID: 590
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020000D6 RID: 214
		// (Invoke) Token: 0x060003C5 RID: 965
		internal delegate void ApplyLinkedEntityGroupRemovals_00000338$PostfixBurstDelegate(in EntityManager entityManager, in NativeArray<LinkedEntityGroupChange> linkedEntityGroupChanges, in NativeArray<EntityGuid> packedEntityGuids, in NativeParallelMultiHashMap<int, Entity> packedEntities, in NativeParallelMultiHashMap<EntityGuid, Entity> entityGuidToEntity, in NativeParallelHashMap<Entity, EntityGuid> entityToEntityGuid, in NativeParallelHashMap<Entity, Entity> entityToLinkedEntityGroupRoot);

		// Token: 0x020000D7 RID: 215
		internal static class ApplyLinkedEntityGroupRemovals_00000338$BurstDirectCall
		{
			// Token: 0x060003C8 RID: 968 RVA: 0x00016B37 File Offset: 0x00014D37
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (EntityPatcher.ApplyLinkedEntityGroupRemovals_00000338$BurstDirectCall.Pointer == 0)
				{
					EntityPatcher.ApplyLinkedEntityGroupRemovals_00000338$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(EntityPatcher.ApplyLinkedEntityGroupRemovals_00000338$BurstDirectCall.DeferredCompilation, methodof(EntityPatcher.ApplyLinkedEntityGroupRemovals$BurstManaged(ref EntityManager, ref NativeArray<LinkedEntityGroupChange>, ref NativeArray<EntityGuid>, ref NativeParallelMultiHashMap<int, Entity>, ref NativeParallelMultiHashMap<EntityGuid, Entity>, ref NativeParallelHashMap<Entity, EntityGuid>, ref NativeParallelHashMap<Entity, Entity>)).MethodHandle, typeof(EntityPatcher.ApplyLinkedEntityGroupRemovals_00000338$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = EntityPatcher.ApplyLinkedEntityGroupRemovals_00000338$BurstDirectCall.Pointer;
			}

			// Token: 0x060003C9 RID: 969 RVA: 0x00016B64 File Offset: 0x00014D64
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				EntityPatcher.ApplyLinkedEntityGroupRemovals_00000338$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x060003CA RID: 970 RVA: 0x00016B7C File Offset: 0x00014D7C
			public static void Constructor()
			{
				EntityPatcher.ApplyLinkedEntityGroupRemovals_00000338$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(EntityPatcher.ApplyLinkedEntityGroupRemovals(ref EntityManager, ref NativeArray<LinkedEntityGroupChange>, ref NativeArray<EntityGuid>, ref NativeParallelMultiHashMap<int, Entity>, ref NativeParallelMultiHashMap<EntityGuid, Entity>, ref NativeParallelHashMap<Entity, EntityGuid>, ref NativeParallelHashMap<Entity, Entity>)).MethodHandle);
			}

			// Token: 0x060003CB RID: 971 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x060003CC RID: 972 RVA: 0x00016B8D File Offset: 0x00014D8D
			// Note: this type is marked as 'beforefieldinit'.
			static ApplyLinkedEntityGroupRemovals_00000338$BurstDirectCall()
			{
				EntityPatcher.ApplyLinkedEntityGroupRemovals_00000338$BurstDirectCall.Constructor();
			}

			// Token: 0x060003CD RID: 973 RVA: 0x00016B94 File Offset: 0x00014D94
			public static void Invoke(in EntityManager entityManager, in NativeArray<LinkedEntityGroupChange> linkedEntityGroupChanges, in NativeArray<EntityGuid> packedEntityGuids, in NativeParallelMultiHashMap<int, Entity> packedEntities, in NativeParallelMultiHashMap<EntityGuid, Entity> entityGuidToEntity, in NativeParallelHashMap<Entity, EntityGuid> entityToEntityGuid, in NativeParallelHashMap<Entity, Entity> entityToLinkedEntityGroupRoot)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = EntityPatcher.ApplyLinkedEntityGroupRemovals_00000338$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.EntityManager&,Unity.Collections.NativeArray`1<Unity.Entities.LinkedEntityGroupChange>&,Unity.Collections.NativeArray`1<Unity.Entities.EntityGuid>&,Unity.Collections.NativeParallelMultiHashMap`2<System.Int32,Unity.Entities.Entity>&,Unity.Collections.NativeParallelMultiHashMap`2<Unity.Entities.EntityGuid,Unity.Entities.Entity>&,Unity.Collections.NativeParallelHashMap`2<Unity.Entities.Entity,Unity.Entities.EntityGuid>&,Unity.Collections.NativeParallelHashMap`2<Unity.Entities.Entity,Unity.Entities.Entity>&), ref entityManager, ref linkedEntityGroupChanges, ref packedEntityGuids, ref packedEntities, ref entityGuidToEntity, ref entityToEntityGuid, ref entityToLinkedEntityGroupRoot, functionPointer);
						return;
					}
				}
				EntityPatcher.ApplyLinkedEntityGroupRemovals$BurstManaged(in entityManager, in linkedEntityGroupChanges, in packedEntityGuids, in packedEntities, in entityGuidToEntity, in entityToEntityGuid, in entityToLinkedEntityGroupRoot);
			}

			// Token: 0x0400024F RID: 591
			private static IntPtr Pointer;

			// Token: 0x04000250 RID: 592
			private static IntPtr DeferredCompilation;
		}
	}
}
