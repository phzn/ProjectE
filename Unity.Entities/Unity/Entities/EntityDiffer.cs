using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Profiling;
using Unity.Properties;
using UnityEngine;

namespace Unity.Entities
{
	// Token: 0x02000088 RID: 136
	[BurstCompile]
	internal static class EntityDiffer
	{
		// Token: 0x060002E3 RID: 739 RVA: 0x0000DB6D File Offset: 0x0000BD6D
		internal static string[] CollectImportantProfilerMarkerStrings()
		{
			return new string[] { EntityDiffer.s_GetChangesProfilerMarkerStr };
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000DB80 File Offset: 0x0000BD80
		internal unsafe static EntityChanges GetChanges(ref EntityDiffer.CachedComponentChanges cachedComponentChanges, EntityManager srcEntityManager, EntityManager dstEntityManager, EntityManagerDifferOptions options, EntityQueryDesc entityQueryDesc, BlobAssetCache blobAssetCache, AllocatorManager.AllocatorHandle allocator)
		{
			EntityChanges entityChanges = default(EntityChanges);
			using (EntityDiffer.s_GetChangesProfilerMarker.Auto())
			{
				if (options == EntityManagerDifferOptions.None)
				{
					return entityChanges;
				}
				srcEntityManager.CompleteAllTrackedJobs();
				dstEntityManager.CompleteAllTrackedJobs();
				EntityQuery entityQuery = srcEntityManager.CreateEntityQuery(new EntityQueryDesc[] { entityQueryDesc });
				EntityQuery entityQuery2 = dstEntityManager.CreateEntityQuery(new EntityQueryDesc[] { entityQueryDesc });
				int num = entityQuery.CalculateChunkCountWithoutFiltering();
				JobHandle jobHandle;
				using (NativeList<ArchetypeChunk> nativeList = entityQuery.ToArchetypeChunkListAsync(Allocator.TempJob, out jobHandle))
				{
					JobHandle jobHandle2;
					using (NativeList<ArchetypeChunk> nativeList2 = entityQuery2.ToArchetypeChunkListAsync(Allocator.TempJob, out jobHandle2))
					{
						JobHandle jobHandle3 = default(JobHandle);
						if (EntityDiffer.CheckOption(options, EntityManagerDifferOptions.ClearMissingReferences))
						{
							EntityDiffer.ClearMissingReferences(srcEntityManager, nativeList, out jobHandle3, jobHandle);
						}
						JobHandle jobHandle4 = JobHandle.CombineDependencies(jobHandle, jobHandle2, jobHandle3);
						JobHandle jobHandle5;
						using (EntityDiffer.ArchetypeChunkChanges archetypeChunkChanges = EntityDiffer.GetArchetypeChunkChanges(nativeList, nativeList2, num, Allocator.TempJob, out jobHandle5, jobHandle4))
						{
							jobHandle5.Complete();
							JobHandle jobHandle6;
							using (NativeArray<EntityDiffer.EntityInChunkWithGuid> sortedEntitiesInChunk = EntityDiffer.GetSortedEntitiesInChunk(srcEntityManager, archetypeChunkChanges.CreatedSrcChunks, Allocator.TempJob, out jobHandle6, default(JobHandle)))
							{
								JobHandle jobHandle7;
								using (NativeArray<EntityDiffer.EntityInChunkWithGuid> sortedEntitiesInChunk2 = EntityDiffer.GetSortedEntitiesInChunk(dstEntityManager, archetypeChunkChanges.DestroyedDstChunks, Allocator.TempJob, out jobHandle7, default(JobHandle)))
								{
									using (EntityDiffer.BlobAssetsWithDistinctHash blobAssetsWithDistinctHash = EntityDiffer.GetBlobAssetsWithDistinctHash(srcEntityManager.GetCheckedEntityDataAccess()->EntityComponentStore, srcEntityManager.GetCheckedEntityDataAccess()->ManagedComponentStore, nativeList.AsArray(), Allocator.TempJob))
									{
										using (NativeList<BlobAssetPtr> nativeList3 = blobAssetCache.BlobAssetBatch->ToNativeList(Allocator.TempJob))
										{
											NativeList<DuplicateEntityGuid> nativeList4 = default(NativeList<DuplicateEntityGuid>);
											EntityDiffer.EntityInChunkChanges entityInChunkChanges = default(EntityDiffer.EntityInChunkChanges);
											EntityDiffer.EntityInChunkChanges entityInChunkChanges2 = default(EntityDiffer.EntityInChunkChanges);
											EntityDiffer.BlobAssetChanges blobAssetChanges = default(EntityDiffer.BlobAssetChanges);
											EntityDiffer.BlobAssetChanges blobAssetChanges2 = default(EntityDiffer.BlobAssetChanges);
											try
											{
												JobHandle jobHandle8 = default(JobHandle);
												JobHandle jobHandle9 = default(JobHandle);
												JobHandle jobHandle10 = default(JobHandle);
												if (EntityDiffer.CheckOption(options, EntityManagerDifferOptions.ValidateUniqueEntityGuid))
												{
													nativeList4 = EntityDiffer.GetDuplicateEntityGuids(sortedEntitiesInChunk, Allocator.TempJob, out jobHandle8, jobHandle6);
												}
												if (EntityDiffer.CheckOption(options, EntityManagerDifferOptions.IncludeForwardChangeSet))
												{
													JobHandle jobHandle11;
													entityInChunkChanges = EntityDiffer.GetEntityInChunkChanges(srcEntityManager, dstEntityManager, sortedEntitiesInChunk, sortedEntitiesInChunk2, Allocator.TempJob, out jobHandle11, JobHandle.CombineDependencies(jobHandle6, jobHandle7));
													jobHandle11.Complete();
													cachedComponentChanges.ForwardComponentChanges.ResizeAndClear(entityInChunkChanges.CreatedEntities.Length + entityInChunkChanges.ModifiedEntities.Length + entityInChunkChanges.DestroyedEntities.Length);
													JobHandle jobHandle12 = default(JobHandle);
													EntityDiffer.GetComponentChanges(ref cachedComponentChanges.ForwardComponentChanges, entityInChunkChanges, (options & EntityManagerDifferOptions.UseReferentialEquality) > EntityManagerDifferOptions.None, default(NativeParallelHashMap<BlobAssetPtr, BlobAssetPtr>), blobAssetCache.BlobAssetRemap, Allocator.TempJob, out jobHandle12, default(JobHandle));
													JobHandle jobHandle13;
													blobAssetChanges = EntityDiffer.GetBlobAssetChanges(blobAssetsWithDistinctHash.BlobAssets, nativeList3, Allocator.TempJob, out jobHandle13, default(JobHandle));
													jobHandle9 = JobHandle.CombineDependencies(jobHandle12, jobHandle13);
												}
												if (EntityDiffer.CheckOption(options, EntityManagerDifferOptions.IncludeReverseChangeSet))
												{
													JobHandle jobHandle14;
													entityInChunkChanges2 = EntityDiffer.GetEntityInChunkChanges(dstEntityManager, srcEntityManager, sortedEntitiesInChunk2, sortedEntitiesInChunk, Allocator.TempJob, out jobHandle14, JobHandle.CombineDependencies(jobHandle6, jobHandle7));
													jobHandle14.Complete();
													jobHandle14 = default(JobHandle);
													cachedComponentChanges.ReverseComponentChanges.ResizeAndClear(entityInChunkChanges2.CreatedEntities.Length + entityInChunkChanges2.ModifiedEntities.Length + entityInChunkChanges2.DestroyedEntities.Length);
													JobHandle jobHandle15;
													EntityDiffer.GetComponentChanges(ref cachedComponentChanges.ReverseComponentChanges, entityInChunkChanges2, (options & EntityManagerDifferOptions.UseReferentialEquality) > EntityManagerDifferOptions.None, blobAssetCache.BlobAssetRemap, default(NativeParallelHashMap<BlobAssetPtr, BlobAssetPtr>), Allocator.TempJob, out jobHandle15, jobHandle14);
													JobHandle jobHandle16;
													blobAssetChanges2 = EntityDiffer.GetBlobAssetChanges(nativeList3, blobAssetsWithDistinctHash.BlobAssets, Allocator.TempJob, out jobHandle16, default(JobHandle));
													jobHandle10 = JobHandle.CombineDependencies(jobHandle15, jobHandle16);
												}
												JobHandle jobHandle17;
												using (NativeList<JobHandle> nativeList5 = new NativeList<JobHandle>(5, Allocator.Temp))
												{
													nativeList5.Add(in jobHandle3);
													nativeList5.Add(in jobHandle8);
													if (EntityDiffer.CheckOption(options, EntityManagerDifferOptions.IncludeForwardChangeSet) || EntityDiffer.CheckOption(options, EntityManagerDifferOptions.IncludeReverseChangeSet))
													{
														nativeList5.Add(in jobHandle9);
														nativeList5.Add(in jobHandle10);
													}
													else
													{
														nativeList5.Add(in jobHandle6);
														nativeList5.Add(in jobHandle7);
													}
													jobHandle17 = JobHandle.CombineDependencies(nativeList5.AsArray());
												}
												jobHandle17.Complete();
												if (nativeList4.IsCreated && nativeList4.Length > 0)
												{
													throw new DuplicateEntityGuidException(nativeList4.AsArray().ToArray());
												}
												EntityChangeSet entityChangeSet = EntityDiffer.CreateEntityChangeSet(entityInChunkChanges, cachedComponentChanges.ForwardComponentChanges, blobAssetChanges, allocator);
												EntityChangeSet entityChangeSet2 = EntityDiffer.CreateEntityChangeSet(entityInChunkChanges2, cachedComponentChanges.ReverseComponentChanges, blobAssetChanges2, allocator);
												entityChanges = new EntityChanges(entityChangeSet, entityChangeSet2);
												if (EntityDiffer.CheckOption(options, EntityManagerDifferOptions.FastForwardShadowWorld))
												{
													EntityDiffer.CopyAndReplaceChunks(srcEntityManager, dstEntityManager, entityQuery2, archetypeChunkChanges, default(NativeArray<EntityRemapUtility.EntityRemapInfo>));
													EntityDiffer.UpdateBlobAssetCache(blobAssetCache, blobAssetsWithDistinctHash.BlobAssets, nativeList3);
												}
											}
											catch (Exception ex)
											{
												Debug.LogException(ex);
												throw ex;
											}
											finally
											{
												if (nativeList4.IsCreated)
												{
													nativeList4.Dispose();
												}
												if (entityInChunkChanges.IsCreated)
												{
													entityInChunkChanges.Dispose();
												}
												if (entityInChunkChanges2.IsCreated)
												{
													entityInChunkChanges2.Dispose();
												}
												if (blobAssetChanges.IsCreated)
												{
													blobAssetChanges.Dispose();
												}
												if (blobAssetChanges2.IsCreated)
												{
													blobAssetChanges2.Dispose();
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return entityChanges;
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000E1A8 File Offset: 0x0000C3A8
		internal unsafe static void PrecomputeBlobAssetCache(EntityManager entityManager, EntityQueryDesc entityQueryDesc, BlobAssetCache blobAssetCache)
		{
			entityManager.CompleteAllTrackedJobs();
			using (NativeArray<ArchetypeChunk> nativeArray = entityManager.CreateEntityQuery(new EntityQueryDesc[] { entityQueryDesc }).ToArchetypeChunkArray(Allocator.TempJob))
			{
				using (EntityDiffer.BlobAssetsWithDistinctHash blobAssetsWithDistinctHash = EntityDiffer.GetBlobAssetsWithDistinctHash(entityManager.GetCheckedEntityDataAccess()->EntityComponentStore, entityManager.GetCheckedEntityDataAccess()->ManagedComponentStore, nativeArray, Allocator.TempJob))
				{
					using (NativeList<BlobAssetPtr> nativeList = new NativeList<BlobAssetPtr>(Allocator.TempJob))
					{
						EntityDiffer.UpdateBlobAssetCache(blobAssetCache, blobAssetsWithDistinctHash.BlobAssets, nativeList);
					}
				}
			}
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000E270 File Offset: 0x0000C470
		private unsafe static void UpdateBlobAssetCache(BlobAssetCache blobAssetCache, NativeList<BlobAssetPtr> srcBlobAssets, NativeList<BlobAssetPtr> dstBlobAssets)
		{
			DynamicBlobAssetBatch* blobAssetBatch = blobAssetCache.BlobAssetBatch;
			NativeParallelHashMap<BlobAssetPtr, BlobAssetPtr> blobAssetRemap = blobAssetCache.BlobAssetRemap;
			using (NativeList<BlobAssetPtr> nativeList = new NativeList<BlobAssetPtr>(1, Allocator.TempJob))
			{
				using (NativeList<BlobAssetPtr> nativeList2 = new NativeList<BlobAssetPtr>(1, Allocator.TempJob))
				{
					using (NativeList<BlobAssetPtr> nativeList3 = new NativeList<BlobAssetPtr>(1, Allocator.TempJob))
					{
						new EntityDiffer.GatherCreatedAndDestroyedBlobAssets
						{
							CreatedBlobAssets = nativeList,
							DestroyedBlobAssets = nativeList2,
							AfterBlobAssets = srcBlobAssets,
							BeforeBlobAssets = dstBlobAssets,
							SameHashDifferentAddressBlobAssets = nativeList3
						}.Run<EntityDiffer.GatherCreatedAndDestroyedBlobAssets>();
						NativeList<BlobAssetPtr> nativeList4 = new NativeList<BlobAssetPtr>(Allocator.TempJob);
						for (int i = 0; i < nativeList2.Length; i++)
						{
							BlobAssetPtr blobAssetPtr;
							if (!blobAssetBatch->TryGetBlobAsset(nativeList2[i].Hash, out blobAssetPtr))
							{
								throw new Exception(string.Format("Failed to destroy a BlobAsset to the shadow world. A BlobAsset with the Hash=[{0}] does not exists.", nativeList[i].Header->Hash));
							}
							ref DynamicBlobAssetBatch ptr = ref *blobAssetBatch;
							blobAssetPtr = nativeList2[i];
							ptr.ReleaseBlobAssetImmediately(blobAssetPtr.Hash);
							foreach (KeyValue<BlobAssetPtr, BlobAssetPtr> keyValue in blobAssetRemap)
							{
								blobAssetPtr = nativeList2[i];
								if (blobAssetPtr.Data == keyValue.Value.Data)
								{
									blobAssetPtr = keyValue.Key;
									nativeList4.Add(in blobAssetPtr);
									break;
								}
							}
						}
						NativeArray<ValueTuple<BlobAssetPtr, BlobAssetPtr>> nativeArray = new NativeArray<ValueTuple<BlobAssetPtr, BlobAssetPtr>>(nativeList3.Length, Allocator.Temp, NativeArrayOptions.ClearMemory);
						int num = 0;
						foreach (BlobAssetPtr blobAssetPtr2 in nativeList3)
						{
							foreach (KeyValue<BlobAssetPtr, BlobAssetPtr> keyValue2 in blobAssetRemap)
							{
								if (keyValue2.Value.Hash == blobAssetPtr2.Hash)
								{
									BlobAssetPtr blobAssetPtr = keyValue2.Key;
									if (blobAssetPtr.Data != blobAssetPtr2.Data)
									{
										blobAssetPtr = keyValue2.Key;
										nativeList4.Add(in blobAssetPtr);
										nativeArray[num] = new ValueTuple<BlobAssetPtr, BlobAssetPtr>(blobAssetPtr2, *keyValue2.Value);
										num++;
										break;
									}
									break;
								}
							}
						}
						foreach (BlobAssetPtr blobAssetPtr3 in nativeList4)
						{
							blobAssetRemap.Remove(blobAssetPtr3);
						}
						nativeList4.Dispose();
						for (int j = 0; j < num; j++)
						{
							blobAssetRemap.Add(nativeArray[j].Item1, nativeArray[j].Item2);
						}
						for (int k = 0; k < nativeList.Length; k++)
						{
							BlobAssetPtr blobAssetPtr;
							if (blobAssetBatch->TryGetBlobAsset(nativeList[k].Header->Hash, out blobAssetPtr))
							{
								throw new Exception(string.Format("Failed to copy a BlobAsset to the shadow world. A BlobAsset with the Hash=[{0}] already exists.", nativeList[k].Header->Hash));
							}
							ref DynamicBlobAssetBatch ptr2 = ref *blobAssetBatch;
							blobAssetPtr = nativeList[k];
							void* data = blobAssetPtr.Data;
							blobAssetPtr = nativeList[k];
							BlobAssetPtr blobAssetPtr4 = ptr2.AllocateBlobAsset(data, blobAssetPtr.Length, nativeList[k].Header->Hash);
							blobAssetRemap.Add(nativeList[k], blobAssetPtr4);
						}
						if (nativeList2.Length > 0 || nativeList.Length > 0)
						{
							blobAssetBatch->SortByHash();
						}
					}
				}
			}
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000E6C0 File Offset: 0x0000C8C0
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void CheckEntityGuidComponent(EntityQueryDesc entityQueryDesc)
		{
			foreach (ComponentType componentType in entityQueryDesc.All)
			{
				if (componentType.GetManagedType() == typeof(EntityGuid))
				{
					return;
				}
			}
			throw new ArgumentException("EntityDiffer custom query requires an EntityGuid component in the All filter.");
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000E710 File Offset: 0x0000C910
		private static JobHandle CombineDependencies(JobHandle job1, JobHandle job2, JobHandle job3)
		{
			NativeArray<JobHandle> nativeArray = new NativeArray<JobHandle>(3, Allocator.Temp, NativeArrayOptions.ClearMemory);
			nativeArray[0] = job1;
			nativeArray[1] = job2;
			nativeArray[2] = job2;
			NativeArray<JobHandle> nativeArray2 = nativeArray;
			JobHandle jobHandle = JobHandle.CombineDependencies(nativeArray2);
			nativeArray2.Dispose();
			return jobHandle;
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000E751 File Offset: 0x0000C951
		private static bool CheckOption(EntityManagerDifferOptions options, EntityManagerDifferOptions option)
		{
			return (options & option) == option;
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000E75C File Offset: 0x0000C95C
		internal static EntityDiffer.ArchetypeChunkChanges GetArchetypeChunkChanges(NativeList<ArchetypeChunk> srcChunks, NativeList<ArchetypeChunk> dstChunks, int maxSrcChunkCount, AllocatorManager.AllocatorHandle allocator, out JobHandle jobHandle, JobHandle dependsOn = default(JobHandle))
		{
			EntityDiffer.ArchetypeChunkChanges archetypeChunkChanges = new EntityDiffer.ArchetypeChunkChanges(allocator);
			NativeParallelHashMap<ulong, ArchetypeChunk> nativeParallelHashMap = new NativeParallelHashMap<ulong, ArchetypeChunk>(maxSrcChunkCount, Allocator.TempJob);
			JobHandle jobHandle2 = new EntityDiffer.BuildChunkSequenceNumberMap
			{
				Chunks = srcChunks,
				ChunksBySequenceNumber = nativeParallelHashMap.AsParallelWriter()
			}.Schedule(srcChunks, 64, dependsOn);
			JobHandle jobHandle3 = new EntityDiffer.GatherArchetypeChunkChanges
			{
				SrcChunks = srcChunks,
				DstChunks = dstChunks,
				SrcChunksBySequenceNumber = nativeParallelHashMap,
				CreatedChunks = archetypeChunkChanges.CreatedSrcChunks.Chunks,
				CreatedChunkFlags = archetypeChunkChanges.CreatedSrcChunks.Flags,
				CreatedChunkEntityCounts = archetypeChunkChanges.CreatedSrcChunks.EntityCounts,
				DestroyedChunks = archetypeChunkChanges.DestroyedDstChunks.Chunks,
				DestroyedChunkFlags = archetypeChunkChanges.DestroyedDstChunks.Flags,
				DestroyedChunkEntityCounts = archetypeChunkChanges.DestroyedDstChunks.EntityCounts
			}.Schedule(jobHandle2);
			jobHandle = nativeParallelHashMap.Dispose(jobHandle3);
			return archetypeChunkChanges;
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000E850 File Offset: 0x0000CA50
		internal unsafe static EntityDiffer.BlobAssetsWithDistinctHash GetBlobAssetsWithDistinctHash(EntityComponentStore* entityComponentStore, ManagedComponentStore managedComponentStore, NativeArray<ArchetypeChunk> chunks, AllocatorManager.AllocatorHandle allocator)
		{
			EntityDiffer.BlobAssetsWithDistinctHash blobAssetsWithDistinctHash = new EntityDiffer.BlobAssetsWithDistinctHash(allocator);
			TypeManager.TypeInfo* typeInfoPointer = TypeManager.GetTypeInfoPointer();
			TypeManager.EntityOffsetInfo* blobAssetRefOffsetsPointer = TypeManager.GetBlobAssetRefOffsetsPointer();
			ManagedObjectBlobs managedObjectBlobs = new ManagedObjectBlobs();
			NativeList<BlobAssetPtr> nativeList = new NativeList<BlobAssetPtr>(16, Allocator.Temp);
			NativeParallelHashMap<BlobAssetPtr, int> nativeParallelHashMap = new NativeParallelHashMap<BlobAssetPtr, int>(16, Allocator.Temp);
			for (int i = 0; i < chunks.Length; i++)
			{
				ChunkIndex chunk = chunks[i].m_Chunk;
				Archetype* archetype = entityComponentStore->GetArchetype(chunk);
				if (archetype->HasBlobAssetRefs)
				{
					int typesCount = archetype->TypesCount;
					int count = chunks[i].Count;
					for (int j = 0; j < typesCount; j++)
					{
						int num = archetype->TypeMemoryOrderIndexToIndexInArchetype[j];
						ComponentTypeInArchetype componentTypeInArchetype = archetype->Types[num];
						if (!componentTypeInArchetype.IsZeroSized)
						{
							ref TypeManager.TypeInfo ptr = ref typeInfoPointer[componentTypeInArchetype.TypeIndex.Index];
							if (ptr.HasBlobAssetRefs)
							{
								byte* buffer = chunk.Buffer;
								int num2 = archetype->Offsets[num];
								byte* ptr2 = buffer + num2;
								if (componentTypeInArchetype.IsManagedComponent)
								{
									ushort num3 = archetype->SizeOfs[num];
									byte* ptr3 = ptr2 + (int)num3 * count;
									for (byte* ptr4 = ptr2; ptr4 < ptr3; ptr4 += num3)
									{
										int num4 = *(int*)ptr4;
										object managedComponent = managedComponentStore.GetManagedComponent(num4);
										if (managedComponent != null)
										{
											managedObjectBlobs.GatherBlobAssetReferences(managedComponent, nativeList, nativeParallelHashMap);
										}
									}
								}
								else
								{
									int blobAssetRefOffsetCount = ptr.BlobAssetRefOffsetCount;
									TypeManager.EntityOffsetInfo* ptr5 = blobAssetRefOffsetsPointer + ptr.BlobAssetRefOffsetStartIndex;
									if (componentTypeInArchetype.IsBuffer)
									{
										BufferHeader* ptr6 = (BufferHeader*)ptr2;
										ushort num5 = archetype->SizeOfs[num];
										int elementSize = ptr.ElementSize;
										for (int k = 0; k < count; k++)
										{
											byte* elementPointer = BufferHeader.GetElementPointer(ptr6);
											byte* ptr7 = elementPointer + ptr6->Length * elementSize;
											for (byte* ptr8 = elementPointer; ptr8 < ptr7; ptr8 += elementSize)
											{
												EntityDiffer.AddBlobAssetsWithDistinctHash(ptr8, ptr5, blobAssetRefOffsetCount, blobAssetsWithDistinctHash);
											}
											ptr6 += (int)num5 / sizeof(BufferHeader);
										}
									}
									else
									{
										ushort num6 = archetype->SizeOfs[num];
										byte* ptr9 = ptr2 + (int)num6 * count;
										for (byte* ptr10 = ptr2; ptr10 < ptr9; ptr10 += num6)
										{
											EntityDiffer.AddBlobAssetsWithDistinctHash(ptr10, ptr5, blobAssetRefOffsetCount, blobAssetsWithDistinctHash);
										}
									}
								}
							}
						}
					}
					SharedComponentValues sharedComponentValues = archetype->Chunks.GetSharedComponentValues(chunk.ListIndex);
					for (int l = 0; l < archetype->NumSharedComponents; l++)
					{
						int num7 = *sharedComponentValues[l];
						if (TypeManager.GetTypeInfo(EntityComponentStore.GetComponentTypeFromSharedComponentIndex(num7)).HasBlobAssetRefs && EntityComponentStore.IsUnmanagedSharedComponentIndex(num7))
						{
							TypeIndex componentTypeFromSharedComponentIndex = EntityComponentStore.GetComponentTypeFromSharedComponentIndex(num7);
							readonly ref TypeManager.TypeInfo typeInfo = ref TypeManager.GetTypeInfo(componentTypeFromSharedComponentIndex);
							int blobAssetRefOffsetCount2 = typeInfo.BlobAssetRefOffsetCount;
							if (blobAssetRefOffsetCount2 > 0)
							{
								byte* sharedComponentDataAddr_Unmanaged = (byte*)entityComponentStore->GetSharedComponentDataAddr_Unmanaged(num7, componentTypeFromSharedComponentIndex);
								TypeManager.EntityOffsetInfo* blobAssetRefOffsets = TypeManager.GetBlobAssetRefOffsets(in typeInfo);
								EntityDiffer.AddBlobAssetsWithDistinctHash(sharedComponentDataAddr_Unmanaged, blobAssetRefOffsets, blobAssetRefOffsetCount2, blobAssetsWithDistinctHash);
							}
						}
					}
				}
			}
			for (int m = 0; m < chunks.Length; m++)
			{
				ChunkIndex chunk2 = chunks[m].m_Chunk;
				Archetype* archetype2 = entityComponentStore->GetArchetype(chunk2);
				SharedComponentValues sharedComponentValues2 = archetype2->Chunks.GetSharedComponentValues(chunk2.ListIndex);
				for (int n = 0; n < archetype2->NumSharedComponents; n++)
				{
					int num8 = *sharedComponentValues2[n];
					if (TypeManager.GetTypeInfo(EntityComponentStore.GetComponentTypeFromSharedComponentIndex(num8)).HasBlobAssetRefs && !EntityComponentStore.IsUnmanagedSharedComponentIndex(num8))
					{
						object sharedComponentDataNonDefaultBoxed = managedComponentStore.GetSharedComponentDataNonDefaultBoxed(num8);
						managedObjectBlobs.GatherBlobAssetReferences(sharedComponentDataNonDefaultBoxed, nativeList, nativeParallelHashMap);
					}
				}
			}
			int num9 = 0;
			while (num9 < nativeList.Length)
			{
				BlobAssetPtr blobAssetPtr = nativeList[num9];
				try
				{
					default(BlobAssetReferenceData).m_Ptr = (byte*)blobAssetPtr.Data;
				}
				catch (Exception)
				{
					goto IL_03AF;
				}
				goto IL_03A6;
				IL_03AF:
				num9++;
				continue;
				IL_03A6:
				blobAssetsWithDistinctHash.TryAdd(blobAssetPtr);
				goto IL_03AF;
			}
			nativeList.Dispose();
			nativeParallelHashMap.Dispose();
			new EntityDiffer.SortBlobAssetPtr
			{
				Array = blobAssetsWithDistinctHash.BlobAssets.AsDeferredJobArray()
			}.Run<EntityDiffer.SortBlobAssetPtr>();
			return blobAssetsWithDistinctHash;
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000EC60 File Offset: 0x0000CE60
		private unsafe static void AddBlobAssetsWithDistinctHash(byte* componentData, TypeManager.EntityOffsetInfo* blobAssetRefOffsets, int blobAssetRefCount, EntityDiffer.BlobAssetsWithDistinctHash blobAssets)
		{
			for (int i = 0; i < blobAssetRefCount; i++)
			{
				int offset = blobAssetRefOffsets[i].Offset;
				BlobAssetReferenceData* ptr = (BlobAssetReferenceData*)(componentData + offset);
				if (ptr->m_Ptr != null)
				{
					try
					{
					}
					catch (Exception)
					{
						goto IL_003B;
					}
					blobAssets.TryAdd(new BlobAssetPtr(ptr->Header));
				}
				IL_003B:;
			}
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000ECC0 File Offset: 0x0000CEC0
		private static EntityDiffer.BlobAssetChanges GetBlobAssetChanges(NativeList<BlobAssetPtr> afterBlobAssets, NativeList<BlobAssetPtr> beforeBlobAssets, AllocatorManager.AllocatorHandle allocator, out JobHandle jobHandle, JobHandle dependsOn = default(JobHandle))
		{
			EntityDiffer.BlobAssetChanges blobAssetChanges = new EntityDiffer.BlobAssetChanges(allocator);
			NativeList<BlobAssetPtr> nativeList = new NativeList<BlobAssetPtr>(1, Allocator.TempJob);
			NativeList<BlobAssetPtr> nativeList2 = new NativeList<BlobAssetPtr>(1, Allocator.TempJob);
			NativeList<BlobAssetPtr> nativeList3 = new NativeList<BlobAssetPtr>(1, Allocator.TempJob);
			jobHandle = new EntityDiffer.GatherCreatedAndDestroyedBlobAssets
			{
				AfterBlobAssets = afterBlobAssets,
				BeforeBlobAssets = beforeBlobAssets,
				CreatedBlobAssets = nativeList,
				DestroyedBlobAssets = nativeList2,
				SameHashDifferentAddressBlobAssets = nativeList3
			}.Schedule(dependsOn);
			jobHandle = new EntityDiffer.GatherBlobAssetChanges
			{
				CreatedBlobAssets = nativeList,
				DestroyedBlobAssets = nativeList2,
				CreatedBlobAssetChanges = blobAssetChanges.CreatedBlobAssets,
				DestroyedBlobAssetChanges = blobAssetChanges.DestroyedBlobAssets,
				BlobAssetData = blobAssetChanges.BlobAssetData
			}.Schedule(jobHandle);
			jobHandle = JobHandle.CombineDependencies(nativeList.Dispose(jobHandle), nativeList2.Dispose(jobHandle), nativeList3.Dispose(jobHandle));
			return blobAssetChanges;
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000EDC4 File Offset: 0x0000CFC4
		private static void ClearMissingReferences(EntityManager entityManager, NativeList<ArchetypeChunk> chunks, out JobHandle jobHandle, JobHandle dependsOn)
		{
			jobHandle = new EntityDiffer.ClearMissingReferencesJob
			{
				Chunks = chunks
			}.Schedule(chunks, 64, dependsOn);
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000EDF4 File Offset: 0x0000CFF4
		private unsafe static bool TryGetEntityGuidComponent(EntityComponentStore* ecs, Entity entity, TypeIndex entityGuidTypeIndex, out EntityGuid entityGuid)
		{
			entityGuid = default(EntityGuid);
			bool flag;
			if (!ecs->HasComponent(entity, entityGuidTypeIndex, out flag))
			{
				return false;
			}
			entityGuid = *(EntityGuid*)ecs->GetComponentDataWithTypeRO(entity, entityGuidTypeIndex);
			return true;
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000EE2C File Offset: 0x0000D02C
		internal static void GatherLinkedEntityGroupChanges(EntityGuid entityGuid, NativeArray<EntityGuid> beforeLinkedEntityGroup, NativeArray<EntityGuid> afterLinkedEntityGroup, ref NativeList<LinkedEntityGroupChange> additions, ref NativeList<LinkedEntityGroupChange> removals)
		{
			beforeLinkedEntityGroup.Sort<EntityGuid>();
			afterLinkedEntityGroup.Sort<EntityGuid>();
			int i = 0;
			int j = 0;
			int length = beforeLinkedEntityGroup.Length;
			int length2 = afterLinkedEntityGroup.Length;
			while (i < length)
			{
				if (j >= length2)
				{
					break;
				}
				EntityGuid entityGuid2 = beforeLinkedEntityGroup[i];
				EntityGuid entityGuid3 = afterLinkedEntityGroup[j];
				int num = entityGuid2.CompareTo(entityGuid3);
				if (num == 0)
				{
					i++;
					j++;
				}
				else if (num > 0)
				{
					LinkedEntityGroupChange linkedEntityGroupChange = default(LinkedEntityGroupChange);
					linkedEntityGroupChange.RootEntityGuid = entityGuid;
					linkedEntityGroupChange.ChildEntityGuid = entityGuid3;
					additions.Add(in linkedEntityGroupChange);
					j++;
				}
				else if (num < 0)
				{
					LinkedEntityGroupChange linkedEntityGroupChange = default(LinkedEntityGroupChange);
					linkedEntityGroupChange.RootEntityGuid = entityGuid;
					linkedEntityGroupChange.ChildEntityGuid = entityGuid2;
					removals.Add(in linkedEntityGroupChange);
					i++;
				}
			}
			while (i < length)
			{
				LinkedEntityGroupChange linkedEntityGroupChange = default(LinkedEntityGroupChange);
				linkedEntityGroupChange.RootEntityGuid = entityGuid;
				linkedEntityGroupChange.ChildEntityGuid = beforeLinkedEntityGroup[i++];
				removals.Add(in linkedEntityGroupChange);
			}
			while (j < length2)
			{
				LinkedEntityGroupChange linkedEntityGroupChange = default(LinkedEntityGroupChange);
				linkedEntityGroupChange.RootEntityGuid = entityGuid;
				linkedEntityGroupChange.ChildEntityGuid = afterLinkedEntityGroup[j++];
				additions.Add(in linkedEntityGroupChange);
			}
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000EF50 File Offset: 0x0000D150
		private unsafe static EntityDiffer.GatherComponentChangesReadOnlyData InitGatherComponentChangesReadOnlyData(EntityDiffer.EntityInChunkChanges entityChanges, EntityDiffer.ComponentChanges componentChanges, bool useReferentialEquivalence, NativeParallelHashMap<BlobAssetPtr, BlobAssetPtr> afterBlobAssetRemap, NativeParallelHashMap<BlobAssetPtr, BlobAssetPtr> beforeBlobAssetRemap)
		{
			return new EntityDiffer.GatherComponentChangesReadOnlyData
			{
				EntityGuidTypeIndex = TypeManager.GetTypeIndex<EntityGuid>(),
				LinkedEntityGroupTypeIndex = TypeManager.GetTypeIndex<LinkedEntityGroup>(),
				TypeInfo = TypeManager.GetTypeInfoPointer(),
				EntityOffsets = TypeManager.GetEntityOffsetsPointer(),
				BlobAssetRefOffsets = TypeManager.GetBlobAssetRefOffsetsPointer(),
				AfterEntityComponentStore = entityChanges.AfterEntityManager.GetCheckedEntityDataAccess()->EntityComponentStore,
				BeforeEntityComponentStore = entityChanges.BeforeEntityManager.GetCheckedEntityDataAccess()->EntityComponentStore,
				CreatedEntities = entityChanges.CreatedEntities,
				ModifiedEntities = entityChanges.ModifiedEntities,
				Entities = componentChanges.Entities,
				ComponentTypes = componentChanges.ComponentTypes,
				AfterBlobAssetRemap = afterBlobAssetRemap,
				BeforeBlobAssetRemap = beforeBlobAssetRemap,
				UseReferentialEquivalence = (useReferentialEquivalence ? 1 : 0)
			};
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000F02C File Offset: 0x0000D22C
		private static EntityDiffer.GatherComponentChangesOutput InitGatherComponentChangesOutput(EntityDiffer.ComponentChanges componentChanges)
		{
			return new EntityDiffer.GatherComponentChangesOutput
			{
				AddComponents = componentChanges.AddComponents,
				AddArchetypes = componentChanges.AddArchetypes,
				RemoveComponents = componentChanges.RemoveComponents,
				SetComponents = componentChanges.SetComponents,
				EntityReferencePatches = componentChanges.EntityReferenceChanges,
				BlobAssetReferenceChanges = componentChanges.BlobAssetReferenceChanges,
				LinkedEntityGroupAdditions = componentChanges.LinkedEntityGroupAdditions,
				LinkedEntityGroupRemovals = componentChanges.LinkedEntityGroupRemovals,
				ComponentData = componentChanges.ComponentData,
				SharedComponentChanges = componentChanges.SharedComponentChanges,
				ManagedComponentChanges = componentChanges.ManagedComponentChanges,
				NonSerializableComponents = new NativeList<EntityDiffer.NonSerializableDebugInfo>(16, Allocator.TempJob)
			};
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000F0E8 File Offset: 0x0000D2E8
		private unsafe static EntityDiffer.ComponentChanges GetComponentChanges(ref EntityDiffer.ComponentChanges componentChanges, EntityDiffer.EntityInChunkChanges entityChanges, bool useReferentialEquivalence, NativeParallelHashMap<BlobAssetPtr, BlobAssetPtr> afterBlobAssetRemap, NativeParallelHashMap<BlobAssetPtr, BlobAssetPtr> beforeBlobAssetRemap, AllocatorManager.AllocatorHandle allocator, out JobHandle jobHandle, JobHandle dependsOn = default(JobHandle))
		{
			dependsOn.Complete();
			dependsOn = default(JobHandle);
			int num = entityChanges.CreatedEntities.Length + entityChanges.ModifiedEntities.Length;
			EntityDiffer.GatherComponentChangesReadOnlyData gatherComponentChangesReadOnlyData = EntityDiffer.InitGatherComponentChangesReadOnlyData(entityChanges, componentChanges, useReferentialEquivalence, afterBlobAssetRemap, beforeBlobAssetRemap);
			EntityDiffer.GatherComponentChangesOutput gatherComponentChangesOutput = EntityDiffer.InitGatherComponentChangesOutput(componentChanges);
			new EntityDiffer.GatherComponentChangesBuildPacked
			{
				CreatedEntities = entityChanges.CreatedEntities,
				ModifiedEntities = entityChanges.ModifiedEntities,
				TypeInfo = TypeManager.GetTypeInfoPointer(),
				Entities = gatherComponentChangesReadOnlyData.Entities,
				ComponentTypes = gatherComponentChangesReadOnlyData.ComponentTypes,
				AfterEntityComponentStore = entityChanges.AfterEntityManager.GetCheckedEntityDataAccess()->EntityComponentStore,
				BeforeEntityComponentStore = entityChanges.BeforeEntityManager.GetCheckedEntityDataAccess()->EntityComponentStore
			}.Run<EntityDiffer.GatherComponentChangesBuildPacked>();
			if (num >= 1)
			{
				new EntityDiffer.GatherComponentChangesJob
				{
					ReadData = gatherComponentChangesReadOnlyData,
					OutputData = gatherComponentChangesOutput,
					AfterEntityComponentStore = entityChanges.AfterEntityManager.GetCheckedEntityDataAccess()->EntityComponentStore,
					BeforeEntityComponentStore = entityChanges.BeforeEntityManager.GetCheckedEntityDataAccess()->EntityComponentStore
				}.Run<EntityDiffer.GatherComponentChangesJob>();
			}
			gatherComponentChangesOutput.NonSerializableComponents.Dispose();
			jobHandle = default(JobHandle);
			return componentChanges;
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000F22C File Offset: 0x0000D42C
		private unsafe static EntityChangeSet CreateEntityChangeSet(EntityDiffer.EntityInChunkChanges entityInChunkChanges, EntityDiffer.ComponentChanges componentChanges, EntityDiffer.BlobAssetChanges blobAssetChanges, AllocatorManager.AllocatorHandle allocator)
		{
			if (!entityInChunkChanges.IsCreated || !componentChanges.IsCreated || !blobAssetChanges.IsCreated)
			{
				return default(EntityChangeSet);
			}
			UnsafeAppendBuffer unsafeAppendBuffer;
			PackedSharedComponentDataChange[] changedSharedComponents = EntityDiffer.GetChangedSharedComponents(componentChanges.Entities, componentChanges.ComponentTypes, componentChanges.SharedComponentChanges, componentChanges.BlobAssetReferenceChanges, entityInChunkChanges.BeforeEntityManager.GetCheckedEntityDataAccess()->EntityComponentStore, entityInChunkChanges.AfterEntityManager.GetCheckedEntityDataAccess()->EntityComponentStore, entityInChunkChanges.BeforeEntityManager.GetCheckedEntityDataAccess()->ManagedComponentStore, entityInChunkChanges.AfterEntityManager.GetCheckedEntityDataAccess()->ManagedComponentStore, allocator, out unsafeAppendBuffer);
			PackedManagedComponentDataChange[] changedManagedComponents = EntityDiffer.GetChangedManagedComponents(componentChanges.Entities, componentChanges.ComponentTypes, componentChanges.ManagedComponentChanges, componentChanges.EntityReferenceChanges, componentChanges.BlobAssetReferenceChanges, entityInChunkChanges.AfterEntityManager.GetCheckedEntityDataAccess()->EntityComponentStore, entityInChunkChanges.BeforeEntityManager.GetCheckedEntityDataAccess()->ManagedComponentStore, entityInChunkChanges.AfterEntityManager.GetCheckedEntityDataAccess()->ManagedComponentStore);
			NativeList<EntityGuid> list = componentChanges.Entities.List;
			for (int i = 0; i < entityInChunkChanges.DestroyedEntities.Length; i++)
			{
				list.Add(in entityInChunkChanges.DestroyedEntities[i].EntityGuid);
			}
			EntityDiffer.NameChangeSet entityNames = EntityDiffer.GetEntityNames(entityInChunkChanges.CreatedEntities, entityInChunkChanges.DestroyedEntities, entityInChunkChanges.NameModifiedEntities, entityInChunkChanges.AfterEntityManager, entityInChunkChanges.BeforeEntityManager, allocator);
			int length = entityInChunkChanges.CreatedEntities.Length;
			int length2 = entityInChunkChanges.DestroyedEntities.Length;
			int nameChangedCount = entityNames.NameChangedCount;
			NativeList<EntityGuid> list2 = componentChanges.Entities.List;
			NativeArray<EntityGuid> nativeArray = list2.ToArray(allocator);
			NativeList<ComponentTypeHash> list3 = componentChanges.ComponentTypes.List;
			return new EntityChangeSet(length, length2, nameChangedCount, nativeArray, list3.ToArray(allocator), entityNames.Names, entityNames.NameChangedEntityGuids, componentChanges.AddComponents.ToArray(allocator), EntityDiffer.DeepCopyToArray(componentChanges.AddArchetypes, allocator), componentChanges.RemoveComponents.ToArray(allocator), componentChanges.SetComponents.ToArray(allocator), componentChanges.ComponentData.ToArray(allocator), componentChanges.EntityReferenceChanges.ToArray(allocator), componentChanges.BlobAssetReferenceChanges.ToArray(allocator), changedManagedComponents, changedSharedComponents, unsafeAppendBuffer, componentChanges.LinkedEntityGroupAdditions.ToArray(allocator), componentChanges.LinkedEntityGroupRemovals.ToArray(allocator), blobAssetChanges.CreatedBlobAssets.ToArray(allocator), blobAssetChanges.DestroyedBlobAssets.ToArray(allocator), blobAssetChanges.BlobAssetData.ToArray(allocator));
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000F4B8 File Offset: 0x0000D6B8
		private static NativeArray<FilteredArchetype> DeepCopyToArray(NativeList<FilteredArchetype> addArchetype, AllocatorManager.AllocatorHandle allocator)
		{
			int num = 0;
			using (NativeArray<FilteredArchetype>.Enumerator enumerator = addArchetype.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.EntityCount == 0)
					{
						break;
					}
					num++;
				}
			}
			NativeArray<FilteredArchetype> nativeArray = CollectionHelper.CreateNativeArray<FilteredArchetype>(num, allocator, NativeArrayOptions.ClearMemory);
			for (int i = 0; i < num; i++)
			{
				FilteredArchetype filteredArchetype = addArchetype[i];
				UnsafeList<TypeIndex> unsafeList = new UnsafeList<TypeIndex>(filteredArchetype.TypeIndices.Length, allocator, NativeArrayOptions.UninitializedMemory);
				filteredArchetype = addArchetype[i];
				UnsafeList<int> unsafeList2 = new UnsafeList<int>(filteredArchetype.PackedEntityIndices.Length, allocator, NativeArrayOptions.UninitializedMemory);
				filteredArchetype = addArchetype[i];
				unsafeList.CopyFrom(in filteredArchetype.TypeIndices);
				filteredArchetype = addArchetype[i];
				unsafeList2.CopyFrom(in filteredArchetype.PackedEntityIndices);
				int num2 = i;
				filteredArchetype = new FilteredArchetype
				{
					EntityCount = addArchetype[i].EntityCount,
					TypeIndices = unsafeList,
					PackedEntityIndices = unsafeList2
				};
				nativeArray[num2] = filteredArchetype;
			}
			return nativeArray;
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000F5D0 File Offset: 0x0000D7D0
		private unsafe static PackedSharedComponentDataChange[] GetChangedSharedComponents(EntityDiffer.PackedEntityGuidsCollection packedEntityCollection, EntityDiffer.PackedCollection<ComponentTypeHash> packedStableTypeHashCollection, NativeList<EntityDiffer.DeferredSharedComponentChange> changes, NativeList<BlobAssetReferenceChange> blobAssetReferencePatches, EntityComponentStore* beforeEntityComponentStore, EntityComponentStore* afterEntityComponentStore, ManagedComponentStore beforeManagedComponentStore, ManagedComponentStore afterManagedComponentStore, AllocatorManager.AllocatorHandle allocator, out UnsafeAppendBuffer unmanagedSharedComponentData)
		{
			if (changes.Length == 0)
			{
				unmanagedSharedComponentData = default(UnsafeAppendBuffer);
				return EntityDiffer.s_EmptySetSharedComponentDiff;
			}
			List<PackedSharedComponentDataChange> list = new List<PackedSharedComponentDataChange>();
			unmanagedSharedComponentData = new UnsafeAppendBuffer(0, 8, allocator);
			EntityDiffer.ManagedObjectPatches managedObjectPatches = new EntityDiffer.ManagedObjectPatches(afterEntityComponentStore);
			for (int i = 0; i < changes.Length; i++)
			{
				EntityDiffer.DeferredSharedComponentChange deferredSharedComponentChange = changes[i];
				readonly ref TypeManager.TypeInfo typeInfo = ref TypeManager.GetTypeInfo(deferredSharedComponentChange.TypeIndex);
				object obj = null;
				void* ptr = null;
				if (deferredSharedComponentChange.AfterSharedComponentIndex != 0 || deferredSharedComponentChange.BeforeSharedComponentIndex != 0)
				{
					if (deferredSharedComponentChange.AfterSharedComponentIndex != 0)
					{
						if (EntityComponentStore.IsUnmanagedSharedComponentIndex(deferredSharedComponentChange.AfterSharedComponentIndex))
						{
							ptr = afterEntityComponentStore->GetSharedComponentDataAddr_Unmanaged(deferredSharedComponentChange.AfterSharedComponentIndex, deferredSharedComponentChange.TypeIndex);
						}
						else
						{
							obj = afterManagedComponentStore.GetSharedComponentDataBoxed(deferredSharedComponentChange.AfterSharedComponentIndex, deferredSharedComponentChange.TypeIndex);
						}
					}
					if (deferredSharedComponentChange.BeforeSharedComponentIndex != -1 && deferredSharedComponentChange.AfterSharedComponentIndex != 0)
					{
						void* ptr2 = null;
						if (EntityComponentStore.IsUnmanagedSharedComponentIndex(deferredSharedComponentChange.BeforeSharedComponentIndex))
						{
							ptr2 = beforeEntityComponentStore->GetSharedComponentDataAddr_Unmanaged(deferredSharedComponentChange.BeforeSharedComponentIndex, deferredSharedComponentChange.TypeIndex);
							if (ptr != null && TypeManager.SharedComponentEquals(ptr2, ptr, deferredSharedComponentChange.TypeIndex))
							{
								goto IL_02D1;
							}
						}
						else if (TypeManager.Equals(beforeManagedComponentStore.GetSharedComponentDataBoxed(deferredSharedComponentChange.BeforeSharedComponentIndex, deferredSharedComponentChange.TypeIndex), obj, deferredSharedComponentChange.TypeIndex))
						{
							goto IL_02D1;
						}
					}
					int num = packedEntityCollection.Get(deferredSharedComponentChange.EntityGuid, 0, 0);
					int orAdd = packedStableTypeHashCollection.GetOrAdd(new ComponentTypeHash
					{
						StableTypeHash = typeInfo.StableTypeHash
					});
					PackedComponent packedComponent = new PackedComponent
					{
						PackedEntityIndex = num,
						PackedTypeIndex = orAdd
					};
					if (ptr != null)
					{
						obj = TypeManager.ConstructComponentFromBuffer(deferredSharedComponentChange.TypeIndex, ptr);
					}
					IRefCounted refCounted = obj as IRefCounted;
					if (refCounted != null)
					{
						refCounted.Retain();
					}
					if (obj != null && (typeInfo.HasBlobAssetRefs || TypeManager.HasEntityReferences(deferredSharedComponentChange.TypeIndex)))
					{
						if (EntityComponentStore.IsUnmanagedSharedComponentIndex(deferredSharedComponentChange.BeforeSharedComponentIndex))
						{
							TypeManager.EntityOffsetInfo* blobAssetRefOffsets = TypeManager.GetBlobAssetRefOffsets(in typeInfo);
							for (int j = 0; j < typeInfo.BlobAssetRefOffsetCount; j++)
							{
								int offset = blobAssetRefOffsets[j].Offset;
								BlobAssetReferenceData* ptr3 = (BlobAssetReferenceData*)((byte*)ptr + offset);
								ulong num2 = 0UL;
								if (ptr3->m_Ptr != null)
								{
									num2 = ptr3->Header->Hash;
								}
								BlobAssetReferenceChange blobAssetReferenceChange = default(BlobAssetReferenceChange);
								blobAssetReferenceChange.Component = packedComponent;
								blobAssetReferenceChange.Offset = offset;
								blobAssetReferenceChange.Value = num2;
								blobAssetReferencePatches.Add(in blobAssetReferenceChange);
							}
						}
						else
						{
							managedObjectPatches.ExtractPatches(ref obj, packedComponent, default(NativeList<EntityReferenceChange>), blobAssetReferencePatches);
						}
					}
					PackedSharedComponentDataChange packedSharedComponentDataChange = new PackedSharedComponentDataChange
					{
						Component = packedComponent,
						BoxedSharedValue = ((ptr != null) ? null : obj),
						UnmanagedSharedValueDataOffsetWithManagedFlag = -1
					};
					if (ptr != null)
					{
						packedSharedComponentDataChange.UnmanagedSharedValueDataOffsetWithManagedFlag = unmanagedSharedComponentData.Length;
						packedSharedComponentDataChange.UnmanagedSharedValueDataOffsetWithManagedFlag &= int.MaxValue;
						unmanagedSharedComponentData.Add(ptr, typeInfo.TypeSize);
					}
					list.Add(packedSharedComponentDataChange);
				}
				IL_02D1:;
			}
			return list.ToArray();
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000F8C8 File Offset: 0x0000DAC8
		private unsafe static PackedManagedComponentDataChange[] GetChangedManagedComponents(EntityDiffer.PackedEntityGuidsCollection packedEntityCollection, EntityDiffer.PackedCollection<ComponentTypeHash> packedStableTypeHashCollection, NativeList<EntityDiffer.DeferredManagedComponentChange> changes, NativeList<EntityReferenceChange> entityReferencePatches, NativeList<BlobAssetReferenceChange> blobAssetReferencePatches, EntityComponentStore* afterEntityComponentStore, ManagedComponentStore beforeManagedComponentStore, ManagedComponentStore afterManagedComponentStore)
		{
			if (changes.Length == 0)
			{
				return EntityDiffer.s_EmptySetManagedComponentDiff;
			}
			List<PackedManagedComponentDataChange> list = new List<PackedManagedComponentDataChange>();
			ManagedObjectClone managedObjectClone = new ManagedObjectClone();
			EntityDiffer.ManagedObjectPatches managedObjectPatches = new EntityDiffer.ManagedObjectPatches(afterEntityComponentStore);
			for (int i = 0; i < changes.Length; i++)
			{
				EntityDiffer.DeferredManagedComponentChange deferredManagedComponentChange = changes[i];
				readonly ref TypeManager.TypeInfo typeInfo = ref TypeManager.GetTypeInfo(deferredManagedComponentChange.TypeIndex);
				if (typeInfo.Category != TypeManager.TypeCategory.UnityEngineObject)
				{
					object obj = afterManagedComponentStore.GetManagedComponent(deferredManagedComponentChange.AfterManagedComponentIndex);
					if (deferredManagedComponentChange.BeforeManagedComponentIndex <= -1 || !TypeManager.Equals(beforeManagedComponentStore.GetManagedComponent(deferredManagedComponentChange.BeforeManagedComponentIndex), obj, deferredManagedComponentChange.TypeIndex))
					{
						int num = packedEntityCollection.Get(deferredManagedComponentChange.EntityGuid, 1, i);
						int orAdd = packedStableTypeHashCollection.GetOrAdd(new ComponentTypeHash
						{
							StableTypeHash = typeInfo.StableTypeHash
						});
						PackedComponent packedComponent = new PackedComponent
						{
							PackedEntityIndex = num,
							PackedTypeIndex = orAdd
						};
						obj = managedObjectClone.Clone(obj);
						if (obj != null && (typeInfo.HasBlobAssetRefs || TypeManager.HasEntityReferences(deferredManagedComponentChange.TypeIndex)))
						{
							managedObjectPatches.ExtractPatches(ref obj, packedComponent, entityReferencePatches, blobAssetReferencePatches);
						}
						list.Add(new PackedManagedComponentDataChange
						{
							Component = packedComponent,
							BoxedValue = obj
						});
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000FA1C File Offset: 0x0000DC1C
		[GenerateTestsForBurstCompatibility(RequiredUnityDefine = "!DOTS_DISABLE_DEBUG_NAMES")]
		internal unsafe static EntityDiffer.NameChangeSet GetEntityNames(NativeList<EntityDiffer.CreatedEntity> createdEntities, NativeList<EntityDiffer.DestroyedEntity> destroyedEntities, NativeList<EntityDiffer.NameModifiedEntity> nameModifiedEntities, EntityManager afterEntityManager, EntityManager beforeEntityManager, AllocatorManager.AllocatorHandle allocator)
		{
			int num = createdEntities.Length + destroyedEntities.Length + nameModifiedEntities.Length;
			num += afterEntityManager.GetCheckedEntityDataAccess()->EntityComponentStore->NameStoreAccess.CountEntitiesWithNamesSet();
			if (num == 0)
			{
				return new EntityDiffer.NameChangeSet(0, allocator);
			}
			EntityDiffer.NameChangeSet nameChangeSet = new EntityDiffer.NameChangeSet(num, allocator);
			int num2 = 0;
			new EntityDiffer.GetEntityNamesJob
			{
				EntityGuidTypeIndex = TypeManager.GetTypeIndex<EntityGuid>(),
				NameChanges = nameChangeSet,
				NameChangeCount = &num2,
				CreatedEntities = createdEntities,
				DestroyedEntities = destroyedEntities,
				NameModifiedEntities = nameModifiedEntities,
				NameStoreAccess = afterEntityManager.GetCheckedEntityDataAccess()->EntityComponentStore->NameStoreAccess,
				AfterEntityComponentStore = afterEntityManager.GetCheckedEntityDataAccess()->EntityComponentStore,
				BeforeEntityComponentStore = beforeEntityManager.GetCheckedEntityDataAccess()->EntityComponentStore
			}.Run<EntityDiffer.GetEntityNamesJob>();
			nameChangeSet.NameChangedCount = num2;
			return nameChangeSet;
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000FB04 File Offset: 0x0000DD04
		internal unsafe static void CopyAndReplaceChunks(EntityManager srcEntityManager, EntityManager dstEntityManager, EntityQuery dstEntityQuery, EntityDiffer.ArchetypeChunkChanges archetypeChunkChanges, NativeArray<EntityRemapUtility.EntityRemapInfo> remap = default(NativeArray<EntityRemapUtility.EntityRemapInfo>))
		{
			EntityDataAccess* checkedEntityDataAccess = dstEntityManager.GetCheckedEntityDataAccess();
			EntityDataAccess* checkedEntityDataAccess2 = srcEntityManager.GetCheckedEntityDataAccess();
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->EntityComponentStore->BeginArchetypeChangeTracking();
			EntityDiffer.DestroyChunks(dstEntityManager, archetypeChunkChanges.DestroyedDstChunks.Chunks);
			if (!remap.IsCreated)
			{
				EntityDiffer.CloneAndAddChunks(srcEntityManager, dstEntityManager, archetypeChunkChanges.CreatedSrcChunks.Chunks, default(NativeList<ArchetypeChunk>));
			}
			else
			{
				using (NativeList<ArchetypeChunk> nativeList = new NativeList<ArchetypeChunk>(Allocator.TempJob))
				{
					EntityDiffer.CloneAndAddChunks(srcEntityManager, dstEntityManager, archetypeChunkChanges.CreatedSrcChunks.Chunks, nativeList);
					NativeList<ArchetypeChunk> chunks = archetypeChunkChanges.CreatedSrcChunks.Chunks;
					int i = 0;
					int length = nativeList.Length;
					while (i < length)
					{
						ChunkIndex chunk = chunks[i].m_Chunk;
						Entity* buffer = (Entity*)chunk.Buffer;
						ChunkIndex chunk2 = nativeList[i].m_Chunk;
						Entity* buffer2 = (Entity*)chunk2.Buffer;
						int j = 0;
						int count = chunk.Count;
						while (j < count)
						{
							EntityRemapUtility.AddEntityRemapping(ref remap, buffer[j], buffer2[j]);
							j++;
						}
						i++;
					}
				}
			}
			checkedEntityDataAccess->EntityComponentStore->EndArchetypeChangeTracking(archetypeChanges, checkedEntityDataAccess->EntityQueryManager);
			checkedEntityDataAccess2->EntityComponentStore->InvalidateChunkListCacheForChangedArchetypes();
			checkedEntityDataAccess->EntityComponentStore->InvalidateChunkListCacheForChangedArchetypes();
			using (NativeArray<ArchetypeChunk> nativeArray = dstEntityQuery.ToArchetypeChunkArray(Allocator.TempJob))
			{
				foreach (ArchetypeChunk archetypeChunk in nativeArray)
				{
					ChunkIndex chunk3 = archetypeChunk.m_Chunk;
					Entity metaChunkEntity = chunk3.MetaChunkEntity;
					if (metaChunkEntity != Entity.Null && dstEntityManager.Exists(metaChunkEntity))
					{
						dstEntityManager.SetComponentData<ChunkHeader>(metaChunkEntity, new ChunkHeader
						{
							ArchetypeChunk = archetypeChunk
						});
					}
				}
			}
			ref EntityComponentStore entityComponentStore = ref *checkedEntityDataAccess2->EntityComponentStore;
			SystemHandle systemHandle = default(SystemHandle);
			entityComponentStore.IncrementGlobalSystemVersion(in systemHandle);
			ref EntityComponentStore entityComponentStore2 = ref *checkedEntityDataAccess->EntityComponentStore;
			systemHandle = default(SystemHandle);
			entityComponentStore2.IncrementGlobalSystemVersion(in systemHandle);
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000FD44 File Offset: 0x0000DF44
		private unsafe static void DestroyChunks(EntityManager entityManager, NativeList<ArchetypeChunk> chunks)
		{
			new EntityDiffer.DestroyChunksJob
			{
				EntityManager = entityManager,
				Chunks = chunks
			}.Run<EntityDiffer.DestroyChunksJob>();
			EntityDataAccess* checkedEntityDataAccess = entityManager.GetCheckedEntityDataAccess();
			EntityComponentStore* entityComponentStore = checkedEntityDataAccess->EntityComponentStore;
			checkedEntityDataAccess->ManagedComponentStore.Playback(ref entityComponentStore->ManagedChangesTracker);
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000FD90 File Offset: 0x0000DF90
		private unsafe static void CloneAndAddChunks(EntityManager srcEntityManager, EntityManager dstEntityManager, NativeList<ArchetypeChunk> chunks, NativeList<ArchetypeChunk> clonedList = default(NativeList<ArchetypeChunk>))
		{
			NativeList<int> nativeList = new NativeList<int>(128, Allocator.TempJob);
			new EntityDiffer.CollectSharedComponentIndices
			{
				Chunks = chunks,
				SharedComponentIndices = nativeList
			}.Run<EntityDiffer.CollectSharedComponentIndices>();
			EntityDataAccess* checkedEntityDataAccess = srcEntityManager.GetCheckedEntityDataAccess();
			EntityDataAccess* checkedEntityDataAccess2 = dstEntityManager.GetCheckedEntityDataAccess();
			ManagedComponentStore managedComponentStore = checkedEntityDataAccess->ManagedComponentStore;
			ManagedComponentStore managedComponentStore2 = checkedEntityDataAccess2->ManagedComponentStore;
			NativeArray<int> nativeArray = new NativeArray<int>(nativeList.AsArray(), Allocator.TempJob);
			checkedEntityDataAccess2->CopySharedComponents(checkedEntityDataAccess, (int*)nativeArray.GetUnsafeReadOnlyPtr<int>(), nativeArray.Length);
			NativeArray<ArchetypeChunk> nativeArray2;
			if (clonedList.IsCreated)
			{
				clonedList.Resize(chunks.Length, NativeArrayOptions.UninitializedMemory);
				nativeArray2 = clonedList.AsArray();
			}
			else
			{
				nativeArray2 = new NativeArray<ArchetypeChunk>(chunks.Length, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
			}
			new EntityDiffer.CreateNewChunks
			{
				Chunks = chunks,
				ClonedChunks = nativeArray2,
				DstEntityManager = dstEntityManager,
				SrcSharedComponentIndices = nativeList.AsArray(),
				DstSharedComponentIndices = nativeArray
			}.Run<EntityDiffer.CreateNewChunks>();
			JobHandle jobHandle = new EntityDiffer.CopyChunkBuffers
			{
				Chunks = chunks,
				ClonedChunks = nativeArray2
			}.Schedule(chunks.Length, default(JobHandle));
			JobHandle.ScheduleBatchedJobs();
			nativeList.Dispose();
			jobHandle.Complete();
			NativeArray<EntityRemapUtility.EntityRemapInfo> nativeArray3 = srcEntityManager.CreateEntityRemapArray(Allocator.TempJob);
			dstEntityManager.DuplicateEntitiesForDiffer(nativeArray3, chunks.AsArray(), nativeArray2);
			checkedEntityDataAccess2->EntityComponentStore->CopyAndUpdateNameByEntity(checkedEntityDataAccess->EntityComponentStore, chunks.AsArray(), nativeArray3);
			managedComponentStore2.Playback(ref checkedEntityDataAccess2->EntityComponentStore->ManagedChangesTracker);
			for (int i = 0; i < nativeArray.Length; i++)
			{
				checkedEntityDataAccess2->RemoveSharedComponentReference(nativeArray[i], 1);
			}
			nativeArray.Dispose();
			jobHandle.Complete();
			for (int j = 0; j < nativeArray2.Length; j++)
			{
				ChunkIndex chunk = chunks[j].m_Chunk;
				ChunkIndex chunk2 = nativeArray2[j].m_Chunk;
				Archetype* archetype = checkedEntityDataAccess->EntityComponentStore->GetArchetype(chunk);
				Archetype* archetype2 = checkedEntityDataAccess2->EntityComponentStore->GetArchetype(chunk2);
				int count = chunk.Count;
				ChunkDataUtility.CloneEnabledBits(chunk, archetype, 0, chunk2, archetype2, 0, count);
				ChunkDataUtility.UpdateChunkDisabledEntityCounts(chunk2, archetype2);
			}
			for (int k = 0; k < nativeArray2.Length; k++)
			{
				ChunkIndex chunk3 = nativeArray2[k].m_Chunk;
				Archetype* archetype3 = checkedEntityDataAccess2->EntityComponentStore->GetArchetype(chunk3);
				int numManagedComponents = archetype3->NumManagedComponents;
				bool hasCompanionComponents = archetype3->HasCompanionComponents;
				for (int l = 0; l < numManagedComponents; l++)
				{
					int num = l + (int)archetype3->FirstManagedComponent;
					int num2 = archetype3->Offsets[num];
					int* ptr = (int*)(chunk3.Buffer + num2);
					int count2 = chunk3.Count;
					if (hasCompanionComponents && TypeManager.GetTypeInfo(archetype3->Types[num].TypeIndex).Category == TypeManager.TypeCategory.UnityEngineObject)
					{
						UnsafeUtility.MemSet((void*)ptr, 0, (long)(4 * count2));
					}
					else
					{
						managedComponentStore2.CloneManagedComponentsFromDifferentWorld(ptr, count2, managedComponentStore, ref *checkedEntityDataAccess2->EntityComponentStore);
					}
				}
				chunk3.MetaChunkEntity = EntityRemapUtility.RemapEntity(ref nativeArray3, chunk3.MetaChunkEntity);
			}
			dstEntityManager.RemapEntitiesForDiffer(nativeArray3, chunks.AsArray(), nativeArray2);
			managedComponentStore2.Playback(ref checkedEntityDataAccess2->EntityComponentStore->ManagedChangesTracker);
			nativeArray3.Dispose();
			new EntityDiffer.PatchAndAddClonedChunks
			{
				SrcChunks = chunks.AsArray(),
				DstChunks = nativeArray2,
				SrcEntityComponentStore = checkedEntityDataAccess->EntityComponentStore,
				DstEntityComponentStore = checkedEntityDataAccess2->EntityComponentStore
			}.Schedule(chunks.Length, 64, default(JobHandle)).Complete();
			if (!clonedList.IsCreated)
			{
				nativeArray2.Dispose();
			}
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0001014C File Offset: 0x0000E34C
		private static NativeArray<EntityDiffer.EntityInChunkWithGuid> GetSortedEntitiesInChunk(EntityManager entityManager, EntityDiffer.ArchetypeChunkChangeSet archetypeChunkChangeSet, AllocatorManager.AllocatorHandle allocator, out JobHandle jobHandle, JobHandle dependsOn = default(JobHandle))
		{
			NativeArray<EntityDiffer.EntityInChunkWithGuid> nativeArray = new NativeArray<EntityDiffer.EntityInChunkWithGuid>(archetypeChunkChangeSet.TotalEntityCount, allocator.ToAllocator, NativeArrayOptions.UninitializedMemory);
			JobHandle jobHandle2 = new EntityDiffer.GatherEntityInChunkWithGuid
			{
				EntityGuidTypeIndex = TypeManager.GetTypeIndex<EntityGuid>(),
				EntityTypeIndex = TypeManager.GetTypeIndex<Entity>(),
				Chunks = archetypeChunkChangeSet.Chunks,
				Flags = archetypeChunkChangeSet.Flags,
				EntityCounts = archetypeChunkChangeSet.EntityCounts,
				Entities = nativeArray
			}.Schedule(archetypeChunkChangeSet.Chunks.Length, 64, dependsOn);
			JobHandle jobHandle3 = new EntityDiffer.SortEntityInChunk
			{
				Array = nativeArray
			}.Schedule(jobHandle2);
			jobHandle = jobHandle3;
			return nativeArray;
		}

		// Token: 0x060002FD RID: 765 RVA: 0x000101F8 File Offset: 0x0000E3F8
		private static EntityDiffer.EntityInChunkChanges GetEntityInChunkChanges(EntityManager afterEntityManager, EntityManager beforeEntityManager, NativeArray<EntityDiffer.EntityInChunkWithGuid> afterEntities, NativeArray<EntityDiffer.EntityInChunkWithGuid> beforeEntities, AllocatorManager.AllocatorHandle allocator, out JobHandle jobHandle, JobHandle dependsOn = default(JobHandle))
		{
			EntityDiffer.EntityInChunkChanges entityInChunkChanges = new EntityDiffer.EntityInChunkChanges(afterEntityManager, beforeEntityManager, allocator);
			jobHandle = new EntityDiffer.GatherEntityChanges
			{
				AfterEntities = afterEntities,
				BeforeEntities = beforeEntities,
				CreatedEntities = entityInChunkChanges.CreatedEntities,
				ModifiedEntities = entityInChunkChanges.ModifiedEntities,
				DestroyedEntities = entityInChunkChanges.DestroyedEntities,
				NameModifiedEntities = entityInChunkChanges.NameModifiedEntities
			}.Schedule(dependsOn);
			return entityInChunkChanges;
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0001026C File Offset: 0x0000E46C
		private static NativeList<DuplicateEntityGuid> GetDuplicateEntityGuids(NativeArray<EntityDiffer.EntityInChunkWithGuid> sortedEntitiesWithGuid, AllocatorManager.AllocatorHandle allocator, out JobHandle jobHandle, JobHandle dependsOn = default(JobHandle))
		{
			NativeList<DuplicateEntityGuid> nativeList = new NativeList<DuplicateEntityGuid>(1, allocator);
			jobHandle = new EntityDiffer.BuildEntityGuidToEntity
			{
				SortedEntitiesWithGuid = sortedEntitiesWithGuid,
				Duplicates = nativeList
			}.Schedule(dependsOn);
			return nativeList;
		}

		// Token: 0x0400010B RID: 267
		private static string s_GetChangesProfilerMarkerStr = "GetChanges";

		// Token: 0x0400010C RID: 268
		private static ProfilerMarker s_GetChangesProfilerMarker = new ProfilerMarker(EntityDiffer.s_GetChangesProfilerMarkerStr);

		// Token: 0x0400010D RID: 269
		private static ProfilerMarker s_CreateEntityChangeSetProfilerMarker = new ProfilerMarker("CreateEntityChangeSet");

		// Token: 0x0400010E RID: 270
		private static ProfilerMarker s_GetEntityNamesProfilerMarker = new ProfilerMarker("GetEntityNames");

		// Token: 0x0400010F RID: 271
		private static ProfilerMarker s_GetChangedManagedComponentsProfilerMarker = new ProfilerMarker("GetChangedManagedComponents");

		// Token: 0x04000110 RID: 272
		private static ProfilerMarker s_GetChangedSharedComponentsProfilerMarker = new ProfilerMarker("GetChangedSharedComponents");

		// Token: 0x04000111 RID: 273
		private static ProfilerMarker s_CopyAndReplaceChunksProfilerMarker = new ProfilerMarker("CopyAndReplaceChunks");

		// Token: 0x04000112 RID: 274
		private static ProfilerMarker s_DestroyChunksProfilerMarker = new ProfilerMarker("DestroyChunks");

		// Token: 0x04000113 RID: 275
		private static ProfilerMarker s_CloneAndAddChunksProfilerMarker = new ProfilerMarker("CloneAndAddChunks");

		// Token: 0x04000114 RID: 276
		private static ProfilerMarker s_GetBlobAssetsWithDistinctHash = new ProfilerMarker("GetBlobAssetsWithDistinctHash");

		// Token: 0x04000115 RID: 277
		public const int ComponentChangesBatchCount = 128;

		// Token: 0x04000116 RID: 278
		private static readonly PackedSharedComponentDataChange[] s_EmptySetSharedComponentDiff = new PackedSharedComponentDataChange[0];

		// Token: 0x04000117 RID: 279
		private static readonly PackedManagedComponentDataChange[] s_EmptySetManagedComponentDiff = new PackedManagedComponentDataChange[0];

		// Token: 0x04000118 RID: 280
		private static readonly ProfilerMarker s_PlaybackManagedChangesMarker = new ProfilerMarker("PlaybackManagedChanges");

		// Token: 0x04000119 RID: 281
		private static readonly ProfilerMarker s_CopySharedComponentsMarker = new ProfilerMarker("CopySharedComponents");

		// Token: 0x0400011A RID: 282
		private static readonly ProfilerMarker s_CopyManagedComponentsMarker = new ProfilerMarker("CopyManagedComponents");

		// Token: 0x02000089 RID: 137
		public struct CachedComponentChanges
		{
			// Token: 0x06000300 RID: 768 RVA: 0x00010389 File Offset: 0x0000E589
			public CachedComponentChanges(int count)
			{
				this.ForwardComponentChanges = new EntityDiffer.ComponentChanges(count);
				this.ReverseComponentChanges = new EntityDiffer.ComponentChanges(count);
			}

			// Token: 0x06000301 RID: 769 RVA: 0x000103A3 File Offset: 0x0000E5A3
			public void Dispose()
			{
				this.ForwardComponentChanges.Dispose();
				this.ReverseComponentChanges.Dispose();
			}

			// Token: 0x0400011B RID: 283
			public EntityDiffer.ComponentChanges ForwardComponentChanges;

			// Token: 0x0400011C RID: 284
			public EntityDiffer.ComponentChanges ReverseComponentChanges;
		}

		// Token: 0x0200008A RID: 138
		[Flags]
		internal enum ArchetypeChunkChangeFlags
		{
			// Token: 0x0400011E RID: 286
			None = 0,
			// Token: 0x0400011F RID: 287
			Cloned = 1
		}

		// Token: 0x0200008B RID: 139
		internal readonly struct ArchetypeChunkChanges : IDisposable
		{
			// Token: 0x06000302 RID: 770 RVA: 0x000103BB File Offset: 0x0000E5BB
			public ArchetypeChunkChanges(AllocatorManager.AllocatorHandle allocator)
			{
				this.CreatedSrcChunks = new EntityDiffer.ArchetypeChunkChangeSet(allocator);
				this.DestroyedDstChunks = new EntityDiffer.ArchetypeChunkChangeSet(allocator);
			}

			// Token: 0x06000303 RID: 771 RVA: 0x000103D8 File Offset: 0x0000E5D8
			public void Dispose()
			{
				this.CreatedSrcChunks.Dispose();
				this.DestroyedDstChunks.Dispose();
			}

			// Token: 0x04000120 RID: 288
			public readonly EntityDiffer.ArchetypeChunkChangeSet CreatedSrcChunks;

			// Token: 0x04000121 RID: 289
			public readonly EntityDiffer.ArchetypeChunkChangeSet DestroyedDstChunks;
		}

		// Token: 0x0200008C RID: 140
		internal struct ArchetypeChunkChangeSet : IDisposable
		{
			// Token: 0x17000086 RID: 134
			// (get) Token: 0x06000304 RID: 772 RVA: 0x00010404 File Offset: 0x0000E604
			public int TotalEntityCount
			{
				get
				{
					if (this.EntityCounts.Length <= 0)
					{
						return 0;
					}
					return this.EntityCounts[this.EntityCounts.Length - 1];
				}
			}

			// Token: 0x06000305 RID: 773 RVA: 0x0001043C File Offset: 0x0000E63C
			public ArchetypeChunkChangeSet(AllocatorManager.AllocatorHandle allocator)
			{
				this.Chunks = new NativeList<ArchetypeChunk>(allocator);
				this.Flags = new NativeList<EntityDiffer.ArchetypeChunkChangeFlags>(allocator);
				this.EntityCounts = new NativeList<int>(allocator);
			}

			// Token: 0x06000306 RID: 774 RVA: 0x00010464 File Offset: 0x0000E664
			public void Dispose()
			{
				this.Chunks.Dispose();
				this.Flags.Dispose();
				this.EntityCounts.Dispose();
			}

			// Token: 0x04000122 RID: 290
			public readonly NativeList<ArchetypeChunk> Chunks;

			// Token: 0x04000123 RID: 291
			public readonly NativeList<EntityDiffer.ArchetypeChunkChangeFlags> Flags;

			// Token: 0x04000124 RID: 292
			public readonly NativeList<int> EntityCounts;
		}

		// Token: 0x0200008D RID: 141
		[BurstCompile]
		private struct BuildChunkSequenceNumberMap : IJobParallelForDefer
		{
			// Token: 0x06000307 RID: 775 RVA: 0x0001049C File Offset: 0x0000E69C
			public void Execute(int index)
			{
				this.ChunksBySequenceNumber.TryAdd(this.Chunks[index].m_Chunk.SequenceNumber, this.Chunks[index]);
			}

			// Token: 0x04000125 RID: 293
			[ReadOnly]
			public NativeList<ArchetypeChunk> Chunks;

			// Token: 0x04000126 RID: 294
			[WriteOnly]
			public NativeParallelHashMap<ulong, ArchetypeChunk>.ParallelWriter ChunksBySequenceNumber;
		}

		// Token: 0x0200008E RID: 142
		[BurstCompile]
		private struct GatherArchetypeChunkChanges : IJob
		{
			// Token: 0x06000308 RID: 776 RVA: 0x000104DC File Offset: 0x0000E6DC
			public void Execute()
			{
				NativeParallelHashMap<ulong, int> nativeParallelHashMap = new NativeParallelHashMap<ulong, int>(1, Allocator.Temp);
				int num = 0;
				int num2 = 0;
				for (int i = 0; i < this.DstChunks.Length; i++)
				{
					ArchetypeChunk archetypeChunk = this.DstChunks[i];
					ArchetypeChunk archetypeChunk2 = default(ArchetypeChunk);
					this.SrcChunksBySequenceNumber.TryGetValue(archetypeChunk.m_Chunk.SequenceNumber, out archetypeChunk2);
					if (archetypeChunk2.m_Chunk == ChunkIndex.Null)
					{
						this.DestroyedChunks.Add(in archetypeChunk);
						EntityDiffer.ArchetypeChunkChangeFlags archetypeChunkChangeFlags = EntityDiffer.ArchetypeChunkChangeFlags.None;
						this.DestroyedChunkFlags.Add(in archetypeChunkChangeFlags);
						this.DestroyedChunkEntityCounts.Add(in num2);
						num2 += archetypeChunk.m_Chunk.Count;
					}
					else
					{
						if (EntityDiffer.GatherArchetypeChunkChanges.ChunksAreDifferent(archetypeChunk2.Archetype.Archetype, archetypeChunk2.m_Chunk, archetypeChunk.Archetype.Archetype, archetypeChunk.m_Chunk))
						{
							this.DestroyedChunks.Add(in archetypeChunk);
							EntityDiffer.ArchetypeChunkChangeFlags archetypeChunkChangeFlags = EntityDiffer.ArchetypeChunkChangeFlags.Cloned;
							this.DestroyedChunkFlags.Add(in archetypeChunkChangeFlags);
							this.DestroyedChunkEntityCounts.Add(in num2);
							num2 += archetypeChunk.m_Chunk.Count;
							this.CreatedChunks.Add(in archetypeChunk2);
							archetypeChunkChangeFlags = EntityDiffer.ArchetypeChunkChangeFlags.Cloned;
							this.CreatedChunkFlags.Add(in archetypeChunkChangeFlags);
							this.CreatedChunkEntityCounts.Add(in num);
							num += archetypeChunk2.m_Chunk.Count;
						}
						nativeParallelHashMap.TryAdd(archetypeChunk2.m_Chunk.SequenceNumber, 1);
					}
				}
				for (int j = 0; j < this.SrcChunks.Length; j++)
				{
					ArchetypeChunk archetypeChunk3 = this.SrcChunks[j];
					int num3;
					if (!nativeParallelHashMap.TryGetValue(archetypeChunk3.m_Chunk.SequenceNumber, out num3))
					{
						this.CreatedChunks.Add(in archetypeChunk3);
						EntityDiffer.ArchetypeChunkChangeFlags archetypeChunkChangeFlags = EntityDiffer.ArchetypeChunkChangeFlags.Cloned;
						this.CreatedChunkFlags.Add(in archetypeChunkChangeFlags);
						this.CreatedChunkEntityCounts.Add(in num);
						num += archetypeChunk3.m_Chunk.Count;
					}
				}
				this.CreatedChunkEntityCounts.Add(in num);
				this.DestroyedChunkEntityCounts.Add(in num2);
			}

			// Token: 0x06000309 RID: 777 RVA: 0x000106DF File Offset: 0x0000E8DF
			private unsafe static bool ChunksAreDifferent(Archetype* srcArchetype, ChunkIndex srcChunk, Archetype* dstArchetype, ChunkIndex dstChunk)
			{
				return true;
			}

			// Token: 0x04000127 RID: 295
			[ReadOnly]
			public NativeList<ArchetypeChunk> SrcChunks;

			// Token: 0x04000128 RID: 296
			[ReadOnly]
			public NativeList<ArchetypeChunk> DstChunks;

			// Token: 0x04000129 RID: 297
			[ReadOnly]
			public NativeParallelHashMap<ulong, ArchetypeChunk> SrcChunksBySequenceNumber;

			// Token: 0x0400012A RID: 298
			[WriteOnly]
			public NativeList<ArchetypeChunk> CreatedChunks;

			// Token: 0x0400012B RID: 299
			[WriteOnly]
			public NativeList<EntityDiffer.ArchetypeChunkChangeFlags> CreatedChunkFlags;

			// Token: 0x0400012C RID: 300
			[WriteOnly]
			public NativeList<int> CreatedChunkEntityCounts;

			// Token: 0x0400012D RID: 301
			[WriteOnly]
			public NativeList<ArchetypeChunk> DestroyedChunks;

			// Token: 0x0400012E RID: 302
			[WriteOnly]
			public NativeList<EntityDiffer.ArchetypeChunkChangeFlags> DestroyedChunkFlags;

			// Token: 0x0400012F RID: 303
			[WriteOnly]
			public NativeList<int> DestroyedChunkEntityCounts;
		}

		// Token: 0x0200008F RID: 143
		private readonly struct BlobAssetChanges : IDisposable
		{
			// Token: 0x0600030A RID: 778 RVA: 0x000106E2 File Offset: 0x0000E8E2
			public BlobAssetChanges(AllocatorManager.AllocatorHandle allocator)
			{
				this.CreatedBlobAssets = new NativeList<BlobAssetChange>(1, allocator);
				this.DestroyedBlobAssets = new NativeList<ulong>(1, allocator);
				this.BlobAssetData = new NativeList<byte>(1, allocator);
				this.IsCreated = true;
			}

			// Token: 0x0600030B RID: 779 RVA: 0x00010714 File Offset: 0x0000E914
			public void Dispose()
			{
				this.CreatedBlobAssets.Dispose();
				this.DestroyedBlobAssets.Dispose();
				this.BlobAssetData.Dispose();
			}

			// Token: 0x04000130 RID: 304
			public readonly NativeList<BlobAssetChange> CreatedBlobAssets;

			// Token: 0x04000131 RID: 305
			public readonly NativeList<ulong> DestroyedBlobAssets;

			// Token: 0x04000132 RID: 306
			public readonly NativeList<byte> BlobAssetData;

			// Token: 0x04000133 RID: 307
			public readonly bool IsCreated;
		}

		// Token: 0x02000090 RID: 144
		internal struct BlobAssetsWithDistinctHash : IDisposable
		{
			// Token: 0x0600030C RID: 780 RVA: 0x0001074B File Offset: 0x0000E94B
			public BlobAssetsWithDistinctHash(AllocatorManager.AllocatorHandle allocator)
			{
				this.BlobAssets = new NativeList<BlobAssetPtr>(1, allocator);
				this.BlobAssetsMap = new NativeParallelHashMap<ulong, int>(1, allocator);
			}

			// Token: 0x0600030D RID: 781 RVA: 0x00010767 File Offset: 0x0000E967
			public void Dispose()
			{
				this.BlobAssets.Dispose();
				this.BlobAssetsMap.Dispose();
			}

			// Token: 0x0600030E RID: 782 RVA: 0x00010780 File Offset: 0x0000E980
			public unsafe void TryAdd(BlobAssetPtr blobAssetPtr)
			{
				int num;
				if (this.BlobAssetsMap.TryGetValue(blobAssetPtr.Header->Hash, out num))
				{
					return;
				}
				this.BlobAssetsMap.TryAdd(blobAssetPtr.Header->Hash, this.BlobAssets.Length);
				BlobAssetPtr blobAssetPtr2 = new BlobAssetPtr(blobAssetPtr.Header);
				this.BlobAssets.Add(in blobAssetPtr2);
			}

			// Token: 0x04000134 RID: 308
			public NativeList<BlobAssetPtr> BlobAssets;

			// Token: 0x04000135 RID: 309
			public NativeParallelHashMap<ulong, int> BlobAssetsMap;
		}

		// Token: 0x02000091 RID: 145
		[BurstCompile]
		private struct SortBlobAssetPtr : IJob
		{
			// Token: 0x0600030F RID: 783 RVA: 0x000107E4 File Offset: 0x0000E9E4
			public void Execute()
			{
				this.Array.Sort(default(BlobAssetPtrHashComparer));
			}

			// Token: 0x04000136 RID: 310
			public NativeArray<BlobAssetPtr> Array;
		}

		// Token: 0x02000092 RID: 146
		[BurstCompile]
		private struct GatherCreatedAndDestroyedBlobAssets : IJob
		{
			// Token: 0x06000310 RID: 784 RVA: 0x00010808 File Offset: 0x0000EA08
			public void Execute()
			{
				int i = 0;
				int j = 0;
				BlobAssetPtrHashComparer blobAssetPtrHashComparer = default(BlobAssetPtrHashComparer);
				while (i < this.AfterBlobAssets.Length)
				{
					if (j >= this.BeforeBlobAssets.Length)
					{
						break;
					}
					BlobAssetPtr blobAssetPtr = this.AfterBlobAssets[i];
					BlobAssetPtr blobAssetPtr2 = this.BeforeBlobAssets[j];
					int num = blobAssetPtrHashComparer.Compare(blobAssetPtr, blobAssetPtr2);
					if (num < 0)
					{
						this.CreatedBlobAssets.Add(in blobAssetPtr);
						i++;
					}
					else if (num == 0)
					{
						i++;
						j++;
						if (blobAssetPtr2.Data != blobAssetPtr.Data)
						{
							this.SameHashDifferentAddressBlobAssets.Add(in blobAssetPtr);
						}
					}
					else
					{
						j++;
						this.DestroyedBlobAssets.Add(in blobAssetPtr2);
					}
				}
				while (i < this.AfterBlobAssets.Length)
				{
					BlobAssetPtr blobAssetPtr3 = this.AfterBlobAssets[i++];
					this.CreatedBlobAssets.Add(in blobAssetPtr3);
				}
				while (j < this.BeforeBlobAssets.Length)
				{
					BlobAssetPtr blobAssetPtr3 = this.BeforeBlobAssets[j++];
					this.DestroyedBlobAssets.Add(in blobAssetPtr3);
				}
			}

			// Token: 0x04000137 RID: 311
			[ReadOnly]
			public NativeList<BlobAssetPtr> AfterBlobAssets;

			// Token: 0x04000138 RID: 312
			[ReadOnly]
			public NativeList<BlobAssetPtr> BeforeBlobAssets;

			// Token: 0x04000139 RID: 313
			[WriteOnly]
			public NativeList<BlobAssetPtr> CreatedBlobAssets;

			// Token: 0x0400013A RID: 314
			[WriteOnly]
			public NativeList<BlobAssetPtr> DestroyedBlobAssets;

			// Token: 0x0400013B RID: 315
			[WriteOnly]
			public NativeList<BlobAssetPtr> SameHashDifferentAddressBlobAssets;
		}

		// Token: 0x02000093 RID: 147
		[BurstCompile]
		private struct GatherBlobAssetChanges : IJob
		{
			// Token: 0x06000311 RID: 785 RVA: 0x0001091C File Offset: 0x0000EB1C
			public unsafe void Execute()
			{
				int num = 0;
				for (int i = 0; i < this.CreatedBlobAssets.Length; i++)
				{
					int length = this.CreatedBlobAssets[i].Header->Length;
					BlobAssetChange blobAssetChange = default(BlobAssetChange);
					blobAssetChange.Length = length;
					blobAssetChange.Hash = this.CreatedBlobAssets[i].Header->Hash;
					this.CreatedBlobAssetChanges.Add(in blobAssetChange);
					num += length;
				}
				for (int j = 0; j < this.DestroyedBlobAssets.Length; j++)
				{
					this.DestroyedBlobAssetChanges.Add(in this.DestroyedBlobAssets[j].Header->Hash);
				}
				this.BlobAssetData.Capacity = num;
				for (int k = 0; k < this.CreatedBlobAssets.Length; k++)
				{
					this.BlobAssetData.AddRange(this.CreatedBlobAssets[k].Data, this.CreatedBlobAssets[k].Length);
				}
			}

			// Token: 0x0400013C RID: 316
			[ReadOnly]
			public NativeList<BlobAssetPtr> CreatedBlobAssets;

			// Token: 0x0400013D RID: 317
			[ReadOnly]
			public NativeList<BlobAssetPtr> DestroyedBlobAssets;

			// Token: 0x0400013E RID: 318
			[WriteOnly]
			public NativeList<BlobAssetChange> CreatedBlobAssetChanges;

			// Token: 0x0400013F RID: 319
			[WriteOnly]
			public NativeList<ulong> DestroyedBlobAssetChanges;

			// Token: 0x04000140 RID: 320
			public NativeList<byte> BlobAssetData;
		}

		// Token: 0x02000094 RID: 148
		[BurstCompile]
		private struct ClearMissingReferencesJob : IJobParallelForDefer
		{
			// Token: 0x06000312 RID: 786 RVA: 0x00010A30 File Offset: 0x0000EC30
			public void Execute(int index)
			{
				ChunkIndex chunk = this.Chunks[index].m_Chunk;
				ChunkDataUtility.ClearMissingReferences(this.Chunks[index].Archetype.Archetype, chunk);
			}

			// Token: 0x04000141 RID: 321
			[ReadOnly]
			public NativeList<ArchetypeChunk> Chunks;
		}

		// Token: 0x02000095 RID: 149
		public struct DeferredSharedComponentChange
		{
			// Token: 0x04000142 RID: 322
			public EntityGuid EntityGuid;

			// Token: 0x04000143 RID: 323
			public TypeIndex TypeIndex;

			// Token: 0x04000144 RID: 324
			public int BeforeSharedComponentIndex;

			// Token: 0x04000145 RID: 325
			public int AfterSharedComponentIndex;
		}

		// Token: 0x02000096 RID: 150
		public struct DeferredManagedComponentChange
		{
			// Token: 0x04000146 RID: 326
			public EntityGuid EntityGuid;

			// Token: 0x04000147 RID: 327
			public TypeIndex TypeIndex;

			// Token: 0x04000148 RID: 328
			public int AfterManagedComponentIndex;

			// Token: 0x04000149 RID: 329
			public int BeforeManagedComponentIndex;
		}

		// Token: 0x02000097 RID: 151
		internal struct NameChangeSet : IDisposable
		{
			// Token: 0x06000313 RID: 787 RVA: 0x00010A6E File Offset: 0x0000EC6E
			public NameChangeSet(int length, AllocatorManager.AllocatorHandle allocator)
			{
				this.Names = CollectionHelper.CreateNativeArray<FixedString64Bytes>(length, allocator, NativeArrayOptions.ClearMemory);
				this.NameChangedEntityGuids = CollectionHelper.CreateNativeArray<EntityGuid>(length, allocator, NativeArrayOptions.ClearMemory);
				this.NameChangedCount = 0;
			}

			// Token: 0x06000314 RID: 788 RVA: 0x00010A94 File Offset: 0x0000EC94
			public void Dispose()
			{
				this.Names.Dispose();
				this.NameChangedEntityGuids.Dispose();
			}

			// Token: 0x0400014A RID: 330
			public readonly NativeArray<FixedString64Bytes> Names;

			// Token: 0x0400014B RID: 331
			public readonly NativeArray<EntityGuid> NameChangedEntityGuids;

			// Token: 0x0400014C RID: 332
			public int NameChangedCount;
		}

		// Token: 0x02000098 RID: 152
		public struct ComponentChanges : IDisposable
		{
			// Token: 0x06000315 RID: 789 RVA: 0x00010AC0 File Offset: 0x0000ECC0
			public ComponentChanges(int count)
			{
				this.Entities = new EntityDiffer.PackedEntityGuidsCollection(count, Allocator.Persistent);
				this.ComponentTypes = new EntityDiffer.PackedCollection<ComponentTypeHash>(count, Allocator.Persistent);
				this.AddComponents = new NativeList<PackedComponent>(count * 16, Allocator.Persistent);
				this.AddArchetypes = new NativeList<FilteredArchetype>(count, Allocator.Persistent);
				this.RemoveComponents = new NativeList<PackedComponent>(count, Allocator.Persistent);
				this.SetComponents = new NativeList<PackedComponentDataChange>(count * 8, Allocator.Persistent);
				this.LinkedEntityGroupAdditions = new NativeList<LinkedEntityGroupChange>(count, Allocator.Persistent);
				this.LinkedEntityGroupRemovals = new NativeList<LinkedEntityGroupChange>(count, Allocator.Persistent);
				this.EntityReferenceChanges = new NativeList<EntityReferenceChange>(count, Allocator.Persistent);
				this.BlobAssetReferenceChanges = new NativeList<BlobAssetReferenceChange>(count, Allocator.Persistent);
				this.ComponentData = new NativeList<byte>(count * 256, Allocator.Persistent);
				this.SharedComponentChanges = new NativeList<EntityDiffer.DeferredSharedComponentChange>(count * 16, Allocator.Persistent);
				this.ManagedComponentChanges = new NativeList<EntityDiffer.DeferredManagedComponentChange>(count, Allocator.Persistent);
				this.IsCreated = true;
				this.ReserveSize = count;
				for (int i = 0; i < count; i++)
				{
					FilteredArchetype filteredArchetype = default(FilteredArchetype);
					filteredArchetype.EntityCount = 0;
					filteredArchetype.PackedEntityIndices = new UnsafeList<int>(1, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
					filteredArchetype.TypeIndices = new UnsafeList<TypeIndex>(1, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
					this.AddArchetypes.Add(in filteredArchetype);
				}
			}

			// Token: 0x06000316 RID: 790 RVA: 0x00010C24 File Offset: 0x0000EE24
			public void ResizeAndClear(int count)
			{
				if (count > this.ReserveSize)
				{
					this.Dispose();
					this = new EntityDiffer.ComponentChanges(count);
					this.ReserveSize = count;
					return;
				}
				this.Clear();
			}

			// Token: 0x06000317 RID: 791 RVA: 0x00010C50 File Offset: 0x0000EE50
			public void Clear()
			{
				this.Entities.Clear();
				this.ComponentTypes.Clear();
				if (this.AddComponents.IsCreated)
				{
					this.AddComponents.Clear();
				}
				if (this.AddArchetypes.IsCreated)
				{
					for (int i = 0; i < this.AddArchetypes.Length; i++)
					{
						ref FilteredArchetype ptr = ref this.AddArchetypes.ElementAt(i);
						ptr.EntityCount = 0;
						ptr.TypeIndices.Clear();
						ptr.PackedEntityIndices.Clear();
					}
				}
				if (this.RemoveComponents.IsCreated)
				{
					this.RemoveComponents.Clear();
				}
				if (this.SetComponents.IsCreated)
				{
					this.SetComponents.Clear();
				}
				if (this.LinkedEntityGroupAdditions.IsCreated)
				{
					this.LinkedEntityGroupAdditions.Clear();
				}
				if (this.LinkedEntityGroupRemovals.IsCreated)
				{
					this.LinkedEntityGroupRemovals.Clear();
				}
				if (this.EntityReferenceChanges.IsCreated)
				{
					this.EntityReferenceChanges.Clear();
				}
				if (this.BlobAssetReferenceChanges.IsCreated)
				{
					this.BlobAssetReferenceChanges.Clear();
				}
				if (this.ComponentData.IsCreated)
				{
					this.ComponentData.Clear();
				}
				if (this.SharedComponentChanges.IsCreated)
				{
					this.SharedComponentChanges.Clear();
				}
				if (this.ManagedComponentChanges.IsCreated)
				{
					this.ManagedComponentChanges.Clear();
				}
			}

			// Token: 0x06000318 RID: 792 RVA: 0x00010DE0 File Offset: 0x0000EFE0
			public void Dispose()
			{
				this.Entities.Dispose();
				this.ComponentTypes.Dispose();
				if (this.AddComponents.IsCreated)
				{
					this.AddComponents.Dispose();
				}
				if (this.AddArchetypes.IsCreated)
				{
					foreach (FilteredArchetype filteredArchetype in this.AddArchetypes)
					{
						UnsafeList<TypeIndex> typeIndices = filteredArchetype.TypeIndices;
						typeIndices.Dispose();
						UnsafeList<int> packedEntityIndices = filteredArchetype.PackedEntityIndices;
						packedEntityIndices.Dispose();
					}
					this.AddArchetypes.Dispose();
				}
				if (this.RemoveComponents.IsCreated)
				{
					this.RemoveComponents.Dispose();
				}
				if (this.SetComponents.IsCreated)
				{
					this.SetComponents.Dispose();
				}
				if (this.LinkedEntityGroupAdditions.IsCreated)
				{
					this.LinkedEntityGroupAdditions.Dispose();
				}
				if (this.LinkedEntityGroupRemovals.IsCreated)
				{
					this.LinkedEntityGroupRemovals.Dispose();
				}
				if (this.EntityReferenceChanges.IsCreated)
				{
					this.EntityReferenceChanges.Dispose();
				}
				if (this.BlobAssetReferenceChanges.IsCreated)
				{
					this.BlobAssetReferenceChanges.Dispose();
				}
				if (this.ComponentData.IsCreated)
				{
					this.ComponentData.Dispose();
				}
				if (this.SharedComponentChanges.IsCreated)
				{
					this.SharedComponentChanges.Dispose();
				}
				if (this.ManagedComponentChanges.IsCreated)
				{
					this.ManagedComponentChanges.Dispose();
				}
			}

			// Token: 0x0400014D RID: 333
			public readonly EntityDiffer.PackedEntityGuidsCollection Entities;

			// Token: 0x0400014E RID: 334
			public readonly EntityDiffer.PackedCollection<ComponentTypeHash> ComponentTypes;

			// Token: 0x0400014F RID: 335
			public readonly NativeList<PackedComponent> AddComponents;

			// Token: 0x04000150 RID: 336
			public readonly NativeList<FilteredArchetype> AddArchetypes;

			// Token: 0x04000151 RID: 337
			public readonly NativeList<PackedComponent> RemoveComponents;

			// Token: 0x04000152 RID: 338
			public readonly NativeList<PackedComponentDataChange> SetComponents;

			// Token: 0x04000153 RID: 339
			public readonly NativeList<LinkedEntityGroupChange> LinkedEntityGroupAdditions;

			// Token: 0x04000154 RID: 340
			public readonly NativeList<LinkedEntityGroupChange> LinkedEntityGroupRemovals;

			// Token: 0x04000155 RID: 341
			public readonly NativeList<EntityReferenceChange> EntityReferenceChanges;

			// Token: 0x04000156 RID: 342
			public readonly NativeList<BlobAssetReferenceChange> BlobAssetReferenceChanges;

			// Token: 0x04000157 RID: 343
			public readonly NativeList<byte> ComponentData;

			// Token: 0x04000158 RID: 344
			public readonly NativeList<EntityDiffer.DeferredSharedComponentChange> SharedComponentChanges;

			// Token: 0x04000159 RID: 345
			public readonly NativeList<EntityDiffer.DeferredManagedComponentChange> ManagedComponentChanges;

			// Token: 0x0400015A RID: 346
			public readonly bool IsCreated;

			// Token: 0x0400015B RID: 347
			private int ReserveSize;
		}

		// Token: 0x02000099 RID: 153
		public struct PackedEntityGuidsCollection : IDisposable
		{
			// Token: 0x06000319 RID: 793 RVA: 0x00010FA0 File Offset: 0x0000F1A0
			public PackedEntityGuidsCollection(int capacity, AllocatorManager.AllocatorHandle label)
			{
				this.List = new NativeList<EntityGuid>(capacity, label);
				this.AddedCount = new NativeReference<int>(label, NativeArrayOptions.ClearMemory);
			}

			// Token: 0x0600031A RID: 794 RVA: 0x00010FBC File Offset: 0x0000F1BC
			public void ResizeUninitialized(int count)
			{
				this.List.ResizeUninitialized(count);
			}

			// Token: 0x0600031B RID: 795 RVA: 0x00010FCA File Offset: 0x0000F1CA
			public void Clear()
			{
				if (this.List.IsCreated)
				{
					this.List.Clear();
				}
			}

			// Token: 0x0600031C RID: 796 RVA: 0x00010FE4 File Offset: 0x0000F1E4
			public void Dispose()
			{
				if (this.List.IsCreated)
				{
					this.List.Dispose();
				}
				if (this.AddedCount.IsCreated)
				{
					this.AddedCount.Dispose();
				}
			}

			// Token: 0x0600031D RID: 797 RVA: 0x00011018 File Offset: 0x0000F218
			public int BinarySearchRange(EntityGuid key, int startIndex, int endIndex, int entryHint)
			{
				if (startIndex == endIndex)
				{
					return -1;
				}
				int num = startIndex + entryHint;
				if (num >= 0 && num < endIndex)
				{
					EntityGuid entityGuid = this.List[num];
					if ((in key) == (in entityGuid))
					{
						return num;
					}
				}
				int i = startIndex;
				int num2 = endIndex - 1;
				if (i == num2)
				{
					EntityGuid entityGuid = this.List[0];
					if ((in key) == (in entityGuid))
					{
						return 0;
					}
				}
				while (i <= num2)
				{
					int num3 = i + (num2 - i) / 2;
					int num4 = this.List[num3].CompareTo(key);
					if (num4 == 0)
					{
						return num3;
					}
					if (num4 > 0)
					{
						num2 = num3 - 1;
					}
					else
					{
						i = num3 + 1;
					}
				}
				return -1;
			}

			// Token: 0x0600031E RID: 798 RVA: 0x000110B8 File Offset: 0x0000F2B8
			public int Get(EntityGuid value, int tableHint, int entryHint)
			{
				int value2 = this.AddedCount.Value;
				int num;
				if (tableHint == 0)
				{
					num = this.BinarySearchRange(value, 0, value2, entryHint);
					if (num == -1)
					{
						num = this.BinarySearchRange(value, value2, this.List.Length, entryHint);
					}
				}
				else
				{
					num = this.BinarySearchRange(value, value2, this.List.Length, entryHint);
					if (num == -1)
					{
						num = this.BinarySearchRange(value, 0, value2, entryHint);
					}
				}
				if (num == -1)
				{
					throw new Exception(string.Format("Unable to find the correct for {0} - index, ranges: {1} - {2}", value, value2, this.List.Length));
				}
				return num;
			}

			// Token: 0x0400015C RID: 348
			[NativeDisableContainerSafetyRestriction]
			public NativeList<EntityGuid> List;

			// Token: 0x0400015D RID: 349
			public NativeReference<int> AddedCount;
		}

		// Token: 0x0200009A RID: 154
		public struct PackedCollection<[global::System.Runtime.CompilerServices.IsUnmanaged] T> : IDisposable where T : struct, ValueType, IEquatable<T>
		{
			// Token: 0x0600031F RID: 799 RVA: 0x00011151 File Offset: 0x0000F351
			public PackedCollection(int capacity, AllocatorManager.AllocatorHandle label)
			{
				this.List = new NativeList<T>(capacity, label);
				this.Lookup = new NativeParallelHashMap<T, int>(capacity, label);
			}

			// Token: 0x06000320 RID: 800 RVA: 0x0001116D File Offset: 0x0000F36D
			public void ResizeUninitialized(int count)
			{
				this.List.ResizeUninitialized(count);
				this.Lookup.Clear();
			}

			// Token: 0x06000321 RID: 801 RVA: 0x00011186 File Offset: 0x0000F386
			public void Clear()
			{
				if (this.List.IsCreated)
				{
					this.List.Clear();
				}
				if (this.Lookup.IsCreated)
				{
					this.Lookup.Clear();
				}
			}

			// Token: 0x06000322 RID: 802 RVA: 0x000111B8 File Offset: 0x0000F3B8
			public void Dispose()
			{
				if (this.List.IsCreated)
				{
					this.List.Dispose();
				}
				if (this.Lookup.IsCreated)
				{
					this.Lookup.Dispose();
				}
			}

			// Token: 0x06000323 RID: 803 RVA: 0x000111EC File Offset: 0x0000F3EC
			public int GetOrAdd(T value)
			{
				int length;
				if (this.Lookup.TryGetValue(value, out length))
				{
					return length;
				}
				length = this.List.Length;
				this.List.Add(in value);
				this.Lookup.TryAdd(value, length);
				return length;
			}

			// Token: 0x06000324 RID: 804 RVA: 0x00011234 File Offset: 0x0000F434
			public int Get(T value)
			{
				int num;
				if (this.Lookup.TryGetValue(value, out num))
				{
					return num;
				}
				return -1;
			}

			// Token: 0x0400015E RID: 350
			[NativeDisableContainerSafetyRestriction]
			public NativeList<T> List;

			// Token: 0x0400015F RID: 351
			[NativeDisableContainerSafetyRestriction]
			public NativeParallelHashMap<T, int> Lookup;
		}

		// Token: 0x0200009B RID: 155
		[BurstCompile]
		private struct GatherComponentChangesBuildPacked : IJob
		{
			// Token: 0x06000325 RID: 805 RVA: 0x00011254 File Offset: 0x0000F454
			private unsafe void AddStableTypeHash(TypeIndex typeIndex)
			{
				ComponentTypeFlags componentTypeFlags = ComponentTypeFlags.None;
				if (typeIndex.IsChunkComponent)
				{
					componentTypeFlags |= ComponentTypeFlags.ChunkComponent;
				}
				ulong stableTypeHash = this.TypeInfo[typeIndex.Index].StableTypeHash;
				this.ComponentTypes.GetOrAdd(new ComponentTypeHash
				{
					StableTypeHash = stableTypeHash,
					Flags = componentTypeFlags
				});
			}

			// Token: 0x06000326 RID: 806 RVA: 0x000112B4 File Offset: 0x0000F4B4
			public unsafe void Execute()
			{
				UnsafeParallelHashSet<ulong> unsafeParallelHashSet = new UnsafeParallelHashSet<ulong>(128, Allocator.Temp);
				for (int i = 0; i < this.CreatedEntities.Length; i++)
				{
					EntityGuid entityGuid = this.CreatedEntities[i].EntityGuid;
					ChunkIndex chunk = this.CreatedEntities[i].AfterEntityInChunk.Chunk;
					Archetype* archetype = this.AfterEntityComponentStore->GetArchetype(chunk);
					int typesCount = archetype->TypesCount;
					this.Entities.List.Add(in entityGuid);
					if (unsafeParallelHashSet.Add((ulong)(long)((IntPtr)((void*)archetype))))
					{
						for (int j = 1; j < typesCount; j++)
						{
							ComponentTypeInArchetype componentTypeInArchetype = archetype->Types[j];
							if (!componentTypeInArchetype.IsCleanupComponent && !componentTypeInArchetype.IsBakeOnlyType && !componentTypeInArchetype.IsChunkComponent)
							{
								this.AddStableTypeHash(componentTypeInArchetype.TypeIndex);
							}
						}
					}
				}
				this.Entities.AddedCount.Value = this.Entities.List.Length;
				for (int k = 0; k < this.ModifiedEntities.Length; k++)
				{
					EntityDiffer.ModifiedEntity modifiedEntity = this.ModifiedEntities[k];
					EntityGuid entityGuid2 = modifiedEntity.EntityGuid;
					ChunkIndex chunk2 = modifiedEntity.AfterEntityInChunk.Chunk;
					Archetype* archetype2 = this.AfterEntityComponentStore->GetArchetype(chunk2);
					int typesCount2 = archetype2->TypesCount;
					ChunkIndex chunk3 = modifiedEntity.BeforeEntityInChunk.Chunk;
					Archetype* archetype3 = this.BeforeEntityComponentStore->GetArchetype(chunk3);
					int typesCount3 = archetype3->TypesCount;
					this.Entities.List.Add(in entityGuid2);
					if (unsafeParallelHashSet.Add((ulong)(long)((IntPtr)((void*)archetype2))))
					{
						for (int l = 1; l < typesCount2; l++)
						{
							ComponentTypeInArchetype componentTypeInArchetype2 = archetype2->Types[l];
							if (!componentTypeInArchetype2.IsCleanupComponent && !componentTypeInArchetype2.IsBakeOnlyType && !componentTypeInArchetype2.IsChunkComponent)
							{
								this.AddStableTypeHash(componentTypeInArchetype2.TypeIndex);
							}
						}
					}
					if (unsafeParallelHashSet.Add((ulong)(long)((IntPtr)((void*)archetype3))))
					{
						for (int m = 1; m < typesCount3; m++)
						{
							ComponentTypeInArchetype componentTypeInArchetype3 = archetype3->Types[m];
							if (!componentTypeInArchetype3.IsCleanupComponent && !componentTypeInArchetype3.IsBakeOnlyType && !componentTypeInArchetype3.IsChunkComponent)
							{
								TypeIndex typeIndex = componentTypeInArchetype3.TypeIndex;
								if (-1 == ChunkDataUtility.GetIndexInTypeArray(archetype2, typeIndex))
								{
									this.AddStableTypeHash(typeIndex);
								}
							}
						}
					}
				}
				unsafeParallelHashSet.Dispose();
			}

			// Token: 0x04000160 RID: 352
			[ReadOnly]
			public NativeList<EntityDiffer.CreatedEntity> CreatedEntities;

			// Token: 0x04000161 RID: 353
			[ReadOnly]
			public NativeList<EntityDiffer.ModifiedEntity> ModifiedEntities;

			// Token: 0x04000162 RID: 354
			[ReadOnly]
			[NativeDisableUnsafePtrRestriction]
			public unsafe TypeManager.TypeInfo* TypeInfo;

			// Token: 0x04000163 RID: 355
			[NativeDisableUnsafePtrRestriction]
			public unsafe EntityComponentStore* AfterEntityComponentStore;

			// Token: 0x04000164 RID: 356
			[NativeDisableUnsafePtrRestriction]
			public unsafe EntityComponentStore* BeforeEntityComponentStore;

			// Token: 0x04000165 RID: 357
			public EntityDiffer.PackedEntityGuidsCollection Entities;

			// Token: 0x04000166 RID: 358
			public EntityDiffer.PackedCollection<ComponentTypeHash> ComponentTypes;
		}

		// Token: 0x0200009C RID: 156
		private struct GatherComponentChanges
		{
			// Token: 0x06000327 RID: 807 RVA: 0x00011540 File Offset: 0x0000F740
			public unsafe void ProcessRange(int index)
			{
				int length = this.ReadData.CreatedEntities.Length;
				int num = 128;
				if (index < length)
				{
					int num2 = math.min(length, index + 128);
					for (int i = index; i < num2; i++)
					{
						EntityGuid entityGuid = this.ReadData.CreatedEntities[i].EntityGuid;
						EntityInChunk afterEntityInChunk = this.ReadData.CreatedEntities[i].AfterEntityInChunk;
						ChunkIndex chunk = afterEntityInChunk.Chunk;
						Archetype* archetype = this.AfterEntityComponentStore->GetArchetype(chunk);
						int typesCount = archetype->TypesCount;
						ulong stableHash = archetype->StableHash;
						int num3 = this.ReadData.Entities.Get(entityGuid, 0, i);
						int num4;
						if (this.AddedArchetypes.TryGetValue(stableHash, out num4))
						{
							FilteredArchetype filteredArchetype = this.CacheData.AddArchetypes[num4];
							filteredArchetype.PackedEntityIndices.Add(in num3);
							filteredArchetype.EntityCount++;
							this.CacheData.AddArchetypes[num4] = filteredArchetype;
						}
						else
						{
							UnsafeList<TypeIndex> unsafeList = new UnsafeList<TypeIndex>(typesCount - 1, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
							for (int j = 1; j < typesCount; j++)
							{
								ComponentTypeInArchetype componentTypeInArchetype = archetype->Types[j];
								if (!componentTypeInArchetype.IsCleanupComponent && !componentTypeInArchetype.IsBakeOnlyType && !componentTypeInArchetype.IsChunkComponent)
								{
									this.ValidateTypeForSerialization(componentTypeInArchetype, entityGuid);
									unsafeList.Add(in componentTypeInArchetype.TypeIndex);
								}
							}
							this.AddedArchetypes.Add(stableHash, this.CacheData.AddArchetypes.Length);
							FilteredArchetype filteredArchetype2 = default(FilteredArchetype);
							filteredArchetype2.EntityCount = 1;
							filteredArchetype2.TypeIndices = unsafeList;
							filteredArchetype2.PackedEntityIndices = new UnsafeList<int>(16, Allocator.TempJob, NativeArrayOptions.UninitializedMemory) { in num3 };
							this.CacheData.AddArchetypes.Add(in filteredArchetype2);
						}
						for (int k = 1; k < typesCount; k++)
						{
							ComponentTypeInArchetype componentTypeInArchetype2 = archetype->Types[k];
							if (!componentTypeInArchetype2.IsCleanupComponent && !componentTypeInArchetype2.IsBakeOnlyType && !componentTypeInArchetype2.IsChunkComponent)
							{
								this.AddComponentData(chunk, archetype, componentTypeInArchetype2, k, afterEntityInChunk.IndexInChunk, entityGuid, 0, i, false);
							}
						}
					}
					this.AddedArchetypes.Clear();
					num = index + 128 - num2;
					if (num <= 0)
					{
						return;
					}
					index = 0;
				}
				else
				{
					index -= length;
				}
				int num5 = math.min(index + num, this.ReadData.ModifiedEntities.Length);
				for (int l = index; l < num5; l++)
				{
					EntityDiffer.ModifiedEntity modifiedEntity = this.ReadData.ModifiedEntities[l];
					EntityGuid entityGuid2 = modifiedEntity.EntityGuid;
					EntityInChunk afterEntityInChunk2 = modifiedEntity.AfterEntityInChunk;
					ChunkIndex chunk2 = afterEntityInChunk2.Chunk;
					Archetype* archetype2 = this.AfterEntityComponentStore->GetArchetype(chunk2);
					int typesCount2 = archetype2->TypesCount;
					EntityInChunk beforeEntityInChunk = modifiedEntity.BeforeEntityInChunk;
					ChunkIndex chunk3 = beforeEntityInChunk.Chunk;
					Archetype* archetype3 = this.BeforeEntityComponentStore->GetArchetype(chunk3);
					int typesCount3 = archetype3->TypesCount;
					for (int m = 1; m < typesCount2; m++)
					{
						ComponentTypeInArchetype componentTypeInArchetype3 = archetype2->Types[m];
						if (!componentTypeInArchetype3.IsCleanupComponent && !componentTypeInArchetype3.IsBakeOnlyType && !componentTypeInArchetype3.IsChunkComponent)
						{
							TypeIndex typeIndex = componentTypeInArchetype3.TypeIndex;
							int indexInTypeArray = ChunkDataUtility.GetIndexInTypeArray(archetype3, typeIndex);
							if (-1 == indexInTypeArray)
							{
								this.ValidateTypeForSerialization(componentTypeInArchetype3, entityGuid2);
								this.AddComponentData(chunk2, archetype2, componentTypeInArchetype3, m, afterEntityInChunk2.IndexInChunk, entityGuid2, 1, l, true);
							}
							else
							{
								if (!componentTypeInArchetype3.IsManagedComponent && modifiedEntity.CanCompareChunkVersions)
								{
									uint changeVersion = archetype2->Chunks.GetChangeVersion(m, chunk2.ListIndex);
									uint changeVersion2 = archetype3->Chunks.GetChangeVersion(indexInTypeArray, chunk3.ListIndex);
									if (changeVersion == changeVersion2)
									{
										goto IL_03E9;
									}
								}
								this.SetComponentData(chunk2, archetype2, componentTypeInArchetype3, m, afterEntityInChunk2.IndexInChunk, chunk3, archetype3, indexInTypeArray, beforeEntityInChunk.IndexInChunk, entityGuid2, l);
							}
						}
						IL_03E9:;
					}
					for (int n = 1; n < typesCount3; n++)
					{
						ComponentTypeInArchetype componentTypeInArchetype4 = archetype3->Types[n];
						if (!componentTypeInArchetype4.IsCleanupComponent && !componentTypeInArchetype4.IsBakeOnlyType && !componentTypeInArchetype4.IsChunkComponent)
						{
							TypeIndex typeIndex2 = componentTypeInArchetype4.TypeIndex;
							if (-1 == ChunkDataUtility.GetIndexInTypeArray(archetype2, typeIndex2))
							{
								PackedComponent packedComponent = this.PackComponent(entityGuid2, typeIndex2, 1, l);
								this.CacheData.RemoveComponents.Add(in packedComponent);
							}
						}
					}
				}
			}

			// Token: 0x06000328 RID: 808 RVA: 0x000119D0 File Offset: 0x0000FBD0
			private unsafe void AddComponentData(ChunkIndex afterChunk, Archetype* afterArchetype, ComponentTypeInArchetype afterTypeInArchetype, int afterIndexInTypeArray, int afterEntityIndexInChunk, EntityGuid entityGuid, int tableHint, int entryHint, bool addComponent)
			{
				PackedComponent packedComponent = this.PackComponent(entityGuid, afterTypeInArchetype.TypeIndex, tableHint, entryHint);
				byte* buffer = afterChunk.Buffer;
				if (addComponent)
				{
					this.CacheData.AddComponents.Add(in packedComponent);
				}
				if (afterTypeInArchetype.IsSharedComponent)
				{
					int num = afterIndexInTypeArray - (int)this.AfterEntityComponentStore->GetArchetype(afterChunk)->FirstSharedComponent;
					int sharedComponentValue = afterArchetype->Chunks.GetSharedComponentValue(num, afterChunk.ListIndex);
					this.AddendSharedComponentData(entityGuid, afterTypeInArchetype.TypeIndex, sharedComponentValue, -1);
					return;
				}
				if (afterTypeInArchetype.IsManagedComponent)
				{
					int num2 = *(int*)(buffer + afterArchetype->Offsets[afterIndexInTypeArray] + (IntPtr)afterEntityIndexInChunk * 4);
					this.AppendManagedComponentData(entityGuid, afterTypeInArchetype.TypeIndex, num2, -1);
					return;
				}
				int num3 = -1;
				if (afterTypeInArchetype.IsEnableable)
				{
					num3 = (ChunkDataUtility.GetEnabledRefRO(afterChunk, afterArchetype, afterIndexInTypeArray).IsSet(afterEntityIndexInChunk) ? (-1) : 0);
				}
				if (afterTypeInArchetype.IsZeroSized)
				{
					if (num3 >= 0)
					{
						this.AppendEmptyComponentData(packedComponent, num3);
					}
					return;
				}
				if (!afterTypeInArchetype.IsBuffer)
				{
					TypeManager.TypeInfo* ptr = this.ReadData.TypeInfo + afterTypeInArchetype.TypeIndex.Index;
					ushort num4 = afterArchetype->SizeOfs[afterIndexInTypeArray];
					byte* ptr2 = buffer + afterArchetype->Offsets[afterIndexInTypeArray] + afterEntityIndexInChunk * (int)num4;
					this.AppendComponentData(packedComponent, (void*)ptr2, (int)num4, num3);
					this.ExtractPatches(ptr, packedComponent, ptr2, 1);
					return;
				}
				ushort num5 = afterArchetype->SizeOfs[afterIndexInTypeArray];
				BufferHeader* ptr3 = (BufferHeader*)(buffer + afterArchetype->Offsets[afterIndexInTypeArray] + afterEntityIndexInChunk * (int)num5);
				int length = ptr3->Length;
				if (length == 0)
				{
					if (num3 >= 0)
					{
						this.AppendEmptyComponentData(packedComponent, num3);
					}
					return;
				}
				byte* elementPointer = BufferHeader.GetElementPointer(ptr3);
				if (afterTypeInArchetype.TypeIndex == this.ReadData.LinkedEntityGroupTypeIndex)
				{
					for (int i = 0; i < length; i++)
					{
						Entity entity = *(Entity*)(elementPointer + (IntPtr)i * (IntPtr)sizeof(Entity));
						EntityGuid entityGuid2 = this.GetEntityGuid(this.ReadData.AfterEntityComponentStore, this.ReadData.EntityGuidTypeIndex, entity);
						LinkedEntityGroupChange linkedEntityGroupChange = default(LinkedEntityGroupChange);
						linkedEntityGroupChange.RootEntityGuid = entityGuid;
						linkedEntityGroupChange.ChildEntityGuid = entityGuid2;
						this.CacheData.LinkedEntityGroupAdditions.Add(in linkedEntityGroupChange);
					}
					return;
				}
				TypeManager.TypeInfo* ptr4 = this.ReadData.TypeInfo + afterTypeInArchetype.TypeIndex.Index;
				this.AppendComponentData(packedComponent, (void*)elementPointer, ptr4->ElementSize * length, num3);
				this.ExtractPatches(ptr4, packedComponent, elementPointer, length);
			}

			// Token: 0x06000329 RID: 809 RVA: 0x00011C4D File Offset: 0x0000FE4D
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[Conditional("UNITY_DOTS_DEBUG")]
			private unsafe static void AssertArchetypeSizeOfsMatch(Archetype* beforeArchetype, int beforeIndexInTypeArray, Archetype* afterArchetype, int afterIndexInTypeArray)
			{
				if (beforeArchetype->SizeOfs[beforeIndexInTypeArray] != afterArchetype->SizeOfs[afterIndexInTypeArray])
				{
					throw new Exception("Archetype->SizeOfs do not match");
				}
			}

			// Token: 0x0600032A RID: 810 RVA: 0x00011C74 File Offset: 0x0000FE74
			private unsafe void SetComponentData(ChunkIndex afterChunk, Archetype* afterArchetype, ComponentTypeInArchetype afterTypeInArchetype, int afterIndexInTypeArray, int afterEntityIndexInChunk, ChunkIndex beforeChunk, Archetype* beforeArchetype, int beforeIndexInTypeArray, int beforeEntityIndexInChunk, EntityGuid entityGuid, int entryHint)
			{
				byte* buffer = afterChunk.Buffer;
				byte* buffer2 = beforeChunk.Buffer;
				if (afterTypeInArchetype.IsSharedComponent)
				{
					int num = beforeIndexInTypeArray - (int)this.BeforeEntityComponentStore->GetArchetype(beforeChunk)->FirstSharedComponent;
					int sharedComponentValue = beforeArchetype->Chunks.GetSharedComponentValue(num, beforeChunk.ListIndex);
					int num2 = afterIndexInTypeArray - (int)this.AfterEntityComponentStore->GetArchetype(afterChunk)->FirstSharedComponent;
					int sharedComponentValue2 = afterArchetype->Chunks.GetSharedComponentValue(num2, afterChunk.ListIndex);
					this.AddendSharedComponentData(entityGuid, afterTypeInArchetype.TypeIndex, sharedComponentValue2, sharedComponentValue);
					return;
				}
				if (afterTypeInArchetype.IsManagedComponent)
				{
					int num3 = *(int*)(buffer + afterArchetype->Offsets[afterIndexInTypeArray] + (IntPtr)afterEntityIndexInChunk * 4);
					int num4 = *(int*)(buffer2 + beforeArchetype->Offsets[beforeIndexInTypeArray] + (IntPtr)beforeEntityIndexInChunk * 4);
					this.AppendManagedComponentData(entityGuid, afterTypeInArchetype.TypeIndex, num3, num4);
					return;
				}
				int num5 = -1;
				if (afterTypeInArchetype.IsEnableable)
				{
					this.AreEnableableComponentsEqual(afterChunk, afterArchetype, afterIndexInTypeArray, afterEntityIndexInChunk, beforeChunk, beforeArchetype, beforeIndexInTypeArray, beforeEntityIndexInChunk, out num5);
				}
				if (afterTypeInArchetype.IsZeroSized)
				{
					if (num5 >= 0)
					{
						PackedComponent packedComponent = this.PackComponent(entityGuid, afterTypeInArchetype.TypeIndex, 1, entryHint);
						this.AppendEmptyComponentData(packedComponent, num5);
					}
					return;
				}
				if (afterTypeInArchetype.IsBuffer)
				{
					BufferHeader* ptr = (BufferHeader*)(buffer2 + beforeArchetype->Offsets[beforeIndexInTypeArray] + beforeEntityIndexInChunk * (int)beforeArchetype->SizeOfs[beforeIndexInTypeArray]);
					byte* elementPointer = BufferHeader.GetElementPointer(ptr);
					int length = ptr->Length;
					BufferHeader* ptr2 = (BufferHeader*)(buffer + afterArchetype->Offsets[afterIndexInTypeArray] + afterEntityIndexInChunk * (int)afterArchetype->SizeOfs[afterIndexInTypeArray]);
					byte* elementPointer2 = BufferHeader.GetElementPointer(ptr2);
					int length2 = ptr2->Length;
					if (afterTypeInArchetype.TypeIndex == this.ReadData.LinkedEntityGroupTypeIndex)
					{
						LinkedEntityGroup* ptr3 = (LinkedEntityGroup*)elementPointer;
						LinkedEntityGroup* ptr4 = (LinkedEntityGroup*)elementPointer2;
						NativeArray<EntityGuid> nativeArray = new NativeArray<EntityGuid>(length, Allocator.Temp, NativeArrayOptions.ClearMemory);
						NativeArray<EntityGuid> nativeArray2 = new NativeArray<EntityGuid>(length2, Allocator.Temp, NativeArrayOptions.ClearMemory);
						for (int i = 0; i < length; i++)
						{
							EntityGuid entityGuid2 = this.GetEntityGuid(this.ReadData.BeforeEntityComponentStore, this.ReadData.EntityGuidTypeIndex, ptr3[i].Value);
							nativeArray[i] = entityGuid2;
						}
						for (int j = 0; j < length2; j++)
						{
							EntityGuid entityGuid3 = this.GetEntityGuid(this.ReadData.AfterEntityComponentStore, this.ReadData.EntityGuidTypeIndex, ptr4[j].Value);
							nativeArray2[j] = entityGuid3;
						}
						EntityDiffer.GatherLinkedEntityGroupChanges(entityGuid, nativeArray, nativeArray2, ref this.CacheData.LinkedEntityGroupAdditions, ref this.CacheData.LinkedEntityGroupRemovals);
						return;
					}
					TypeManager.TypeInfo* ptr5 = this.ReadData.TypeInfo + afterTypeInArchetype.TypeIndex.Index;
					if (length2 != length || !this.AreComponentsEqual(ptr5, elementPointer, elementPointer2, length2) || num5 >= 0)
					{
						PackedComponent packedComponent2 = this.PackComponent(entityGuid, afterTypeInArchetype.TypeIndex, 1, entryHint);
						this.AppendComponentData(packedComponent2, (void*)elementPointer2, ptr5->ElementSize * length2, num5);
						this.ExtractPatches(ptr5, packedComponent2, elementPointer2, length2);
						return;
					}
				}
				else
				{
					byte* ptr6 = buffer2 + beforeArchetype->Offsets[beforeIndexInTypeArray] + (int)beforeArchetype->SizeOfs[beforeIndexInTypeArray] * beforeEntityIndexInChunk;
					byte* ptr7 = buffer + afterArchetype->Offsets[afterIndexInTypeArray] + (int)afterArchetype->SizeOfs[afterIndexInTypeArray] * afterEntityIndexInChunk;
					TypeManager.TypeInfo* ptr8 = this.ReadData.TypeInfo + afterTypeInArchetype.TypeIndex.Index;
					if (!this.AreComponentsEqual(ptr8, ptr6, ptr7, 1) || num5 >= 0)
					{
						PackedComponent packedComponent3 = this.PackComponent(entityGuid, afterTypeInArchetype.TypeIndex, 1, entryHint);
						this.ExtractPatches(ptr8, packedComponent3, ptr7, 1);
						this.AppendComponentData(packedComponent3, (void*)ptr7, (int)beforeArchetype->SizeOfs[beforeIndexInTypeArray], num5);
					}
				}
			}

			// Token: 0x0600032B RID: 811 RVA: 0x0001203C File Offset: 0x0001023C
			private void ValidateTypeForSerialization(ComponentTypeInArchetype afterTypeInArchetype, EntityGuid entityGuid)
			{
				if (!afterTypeInArchetype.IsChunkSerializable)
				{
					bool flag = afterTypeInArchetype.IsSharedComponent && afterTypeInArchetype.TypeIndex.HasEntityReferences;
					EntityDiffer.NonSerializableDebugInfo nonSerializableDebugInfo = default(EntityDiffer.NonSerializableDebugInfo);
					nonSerializableDebugInfo.OriginatingId = entityGuid.OriginatingId;
					nonSerializableDebugInfo.OriginatingSubId = entityGuid.OriginatingSubId;
					nonSerializableDebugInfo.TypeIndex = afterTypeInArchetype.TypeIndex;
					nonSerializableDebugInfo.SharedComponent = (flag ? 1 : 0);
					this.CacheData.NonSerializableComponents.Add(in nonSerializableDebugInfo);
				}
			}

			// Token: 0x0600032C RID: 812 RVA: 0x000120C0 File Offset: 0x000102C0
			private unsafe static bool ShouldSkip(int offset, ref int nextOffsetIndex, int offsetCount, TypeManager.EntityOffsetInfo* offsets)
			{
				while (nextOffsetIndex < offsetCount)
				{
					int num = offset - offsets[nextOffsetIndex].Offset;
					if (num < 0)
					{
						return false;
					}
					if (num == 0)
					{
						return true;
					}
					nextOffsetIndex++;
				}
				return false;
			}

			// Token: 0x0600032D RID: 813 RVA: 0x000120FC File Offset: 0x000102FC
			private unsafe bool AreEnableableComponentsEqual(ChunkIndex afterChunk, Archetype* afterArchetype, int afterIndexInTypeArray, int afterEntityIndexInChunk, ChunkIndex beforeChunk, Archetype* beforeArchetype, int beforeIndexInTypeArray, int beforeEntityIndexInChunk, out int enabled)
			{
				bool flag = ChunkDataUtility.GetEnabledRefRO(afterChunk, afterArchetype, afterIndexInTypeArray).IsSet(afterEntityIndexInChunk);
				bool flag2 = ChunkDataUtility.GetEnabledRefRO(beforeChunk, beforeArchetype, beforeIndexInTypeArray).IsSet(beforeEntityIndexInChunk);
				bool flag3 = flag == flag2;
				enabled = (flag3 ? (-1) : (flag ? 1 : 0));
				return flag3;
			}

			// Token: 0x0600032E RID: 814 RVA: 0x0001214C File Offset: 0x0001034C
			private unsafe bool AreComponentsEqual(TypeManager.TypeInfo* typeInfo, byte* beforeAddress, byte* afterAddress, int elementCount)
			{
				int elementSize = typeInfo->ElementSize;
				if (this.ReadData.UseReferentialEquivalence == 0 || (typeInfo->EntityOffsetCount == 0 && typeInfo->BlobAssetRefOffsetCount == 0))
				{
					return UnsafeUtility.MemCmp((void*)beforeAddress, (void*)afterAddress, (long)(elementCount * elementSize)) == 0 && !this.BlobAssetHashesAreDifferent(typeInfo, beforeAddress, afterAddress, elementCount);
				}
				if (this.BlobAssetHashesAreDifferent(typeInfo, beforeAddress, afterAddress, elementCount))
				{
					return false;
				}
				if (this.EntityGuidsAreDifferent(typeInfo, beforeAddress, afterAddress, elementCount))
				{
					return false;
				}
				TypeManager.EntityOffsetInfo* ptr = this.ReadData.BlobAssetRefOffsets + typeInfo->BlobAssetRefOffsetStartIndex;
				TypeManager.EntityOffsetInfo* ptr2 = this.ReadData.EntityOffsets + typeInfo->EntityOffsetStartIndex;
				int blobAssetRefOffsetCount = typeInfo->BlobAssetRefOffsetCount;
				int entityOffsetCount = typeInfo->EntityOffsetCount;
				byte* ptr3 = stackalloc byte[(UIntPtr)elementSize];
				int num = 0;
				int num2 = 0;
				int i = 0;
				while (i < elementSize)
				{
					if (EntityDiffer.GatherComponentChanges.ShouldSkip(i, ref num, entityOffsetCount, ptr2))
					{
						UnsafeUtility.MemSet((void*)(ptr3 + i), 0, (long)sizeof(Entity));
						i += sizeof(Entity);
					}
					else if (EntityDiffer.GatherComponentChanges.ShouldSkip(i, ref num2, blobAssetRefOffsetCount, ptr))
					{
						UnsafeUtility.MemSet((void*)(ptr3 + i), 0, (long)sizeof(BlobAssetReferenceData));
						i += sizeof(BlobAssetReferenceData);
					}
					else
					{
						ptr3[i] = byte.MaxValue;
						i++;
					}
				}
				for (int j = 0; j < elementCount; j++)
				{
					for (int k = 0; k < elementSize; k++)
					{
						if (ptr3[k] != 0 && beforeAddress[k] != afterAddress[k])
						{
							return false;
						}
					}
				}
				return true;
			}

			// Token: 0x0600032F RID: 815 RVA: 0x000122BC File Offset: 0x000104BC
			private unsafe bool BlobAssetHashesAreDifferent(TypeManager.TypeInfo* typeInfo, byte* beforeAddress, byte* afterAddress, int elementCount)
			{
				if (typeInfo->BlobAssetRefOffsetCount == 0)
				{
					return false;
				}
				TypeManager.EntityOffsetInfo* ptr = this.ReadData.BlobAssetRefOffsets + typeInfo->BlobAssetRefOffsetStartIndex;
				int num = 0;
				int elementSize = typeInfo->ElementSize;
				int blobAssetRefOffsetCount = typeInfo->BlobAssetRefOffsetCount;
				for (int i = 0; i < elementCount; i++)
				{
					for (int j = 0; j < blobAssetRefOffsetCount; j++)
					{
						int num2 = num + ptr[j].Offset;
						BlobAssetReferenceData* ptr2 = (BlobAssetReferenceData*)(beforeAddress + num2);
						BlobAssetReferenceData* ptr3 = (BlobAssetReferenceData*)(afterAddress + num2);
						if (EntityDiffer.GatherComponentChanges.GetBlobAssetHash(this.ReadData.BeforeBlobAssetRemap, ptr2) != EntityDiffer.GatherComponentChanges.GetBlobAssetHash(this.ReadData.AfterBlobAssetRemap, ptr3))
						{
							return true;
						}
					}
					num += elementSize;
				}
				return false;
			}

			// Token: 0x06000330 RID: 816 RVA: 0x00012370 File Offset: 0x00010570
			private unsafe bool EntityGuidsAreDifferent(TypeManager.TypeInfo* typeInfo, byte* beforeAddress, byte* afterAddress, int elementCount)
			{
				if (typeInfo->EntityOffsetCount == 0)
				{
					return false;
				}
				TypeManager.EntityOffsetInfo* ptr = this.ReadData.EntityOffsets + typeInfo->EntityOffsetStartIndex;
				int num = 0;
				int elementSize = typeInfo->ElementSize;
				int entityOffsetCount = typeInfo->EntityOffsetCount;
				for (int i = 0; i < elementCount; i++)
				{
					for (int j = 0; j < entityOffsetCount; j++)
					{
						int num2 = num + ptr[j].Offset;
						Entity entity = *(Entity*)(afterAddress + num2);
						Entity entity2 = *(Entity*)(beforeAddress + num2);
						EntityGuid entityGuid;
						EntityDiffer.TryGetEntityGuidComponent(this.ReadData.BeforeEntityComponentStore, entity2, this.ReadData.EntityGuidTypeIndex, out entityGuid);
						EntityGuid entityGuid2;
						EntityDiffer.TryGetEntityGuidComponent(this.ReadData.AfterEntityComponentStore, entity, this.ReadData.EntityGuidTypeIndex, out entityGuid2);
						if (!entityGuid.Equals(entityGuid2))
						{
							return true;
						}
					}
					num += elementSize;
				}
				return false;
			}

			// Token: 0x06000331 RID: 817 RVA: 0x00012459 File Offset: 0x00010659
			private unsafe void ExtractPatches(TypeManager.TypeInfo* typeInfo, PackedComponent component, byte* afterAddress, int elementCount)
			{
				this.ExtractEntityReferencePatches(typeInfo, component, afterAddress, elementCount);
				this.ExtractBlobAssetReferencePatches(typeInfo, component, afterAddress, elementCount);
			}

			// Token: 0x06000332 RID: 818 RVA: 0x00012474 File Offset: 0x00010674
			private unsafe void ExtractEntityReferencePatches(TypeManager.TypeInfo* typeInfo, PackedComponent component, byte* afterAddress, int elementCount)
			{
				if (typeInfo->EntityOffsetCount == 0)
				{
					return;
				}
				TypeManager.EntityOffsetInfo* ptr = this.ReadData.EntityOffsets + typeInfo->EntityOffsetStartIndex;
				int num = 0;
				int elementSize = typeInfo->ElementSize;
				int entityOffsetCount = typeInfo->EntityOffsetCount;
				for (int i = 0; i < elementCount; i++)
				{
					for (int j = 0; j < entityOffsetCount; j++)
					{
						int num2 = num + ptr[j].Offset;
						Entity entity = *(Entity*)(afterAddress + num2);
						EntityGuid entityGuid;
						EntityDiffer.TryGetEntityGuidComponent(this.ReadData.AfterEntityComponentStore, entity, this.ReadData.EntityGuidTypeIndex, out entityGuid);
						EntityReferenceChange entityReferenceChange = default(EntityReferenceChange);
						entityReferenceChange.Component = component;
						entityReferenceChange.Offset = num2;
						entityReferenceChange.Value = entityGuid;
						this.CacheData.EntityReferencePatches.Add(in entityReferenceChange);
					}
					num += elementSize;
				}
			}

			// Token: 0x06000333 RID: 819 RVA: 0x00012554 File Offset: 0x00010754
			private unsafe void ExtractBlobAssetReferencePatches(TypeManager.TypeInfo* typeInfo, PackedComponent component, byte* afterAddress, int elementCount)
			{
				if (typeInfo->BlobAssetRefOffsetCount == 0)
				{
					return;
				}
				TypeManager.EntityOffsetInfo* ptr = this.ReadData.BlobAssetRefOffsets + typeInfo->BlobAssetRefOffsetStartIndex;
				int num = 0;
				int elementSize = typeInfo->ElementSize;
				int blobAssetRefOffsetCount = typeInfo->BlobAssetRefOffsetCount;
				for (int i = 0; i < elementCount; i++)
				{
					for (int j = 0; j < blobAssetRefOffsetCount; j++)
					{
						int num2 = num + ptr[j].Offset;
						BlobAssetReferenceData* ptr2 = (BlobAssetReferenceData*)(afterAddress + num2);
						ulong blobAssetHash = EntityDiffer.GatherComponentChanges.GetBlobAssetHash(this.ReadData.AfterBlobAssetRemap, ptr2);
						BlobAssetReferenceChange blobAssetReferenceChange = default(BlobAssetReferenceChange);
						blobAssetReferenceChange.Component = component;
						blobAssetReferenceChange.Offset = num2;
						blobAssetReferenceChange.Value = blobAssetHash;
						this.CacheData.BlobAssetReferenceChanges.Add(in blobAssetReferenceChange);
					}
					num += elementSize;
				}
			}

			// Token: 0x06000334 RID: 820 RVA: 0x00012620 File Offset: 0x00010820
			private unsafe static ulong GetBlobAssetHash(NativeParallelHashMap<BlobAssetPtr, BlobAssetPtr> remap, BlobAssetReferenceData* blobAssetReferenceData)
			{
				if (blobAssetReferenceData->m_Ptr == null)
				{
					return 0UL;
				}
				BlobAssetPtr blobAssetPtr;
				if (remap.IsCreated && remap.TryGetValue(new BlobAssetPtr((BlobAssetHeader*)(blobAssetReferenceData->m_Ptr - sizeof(BlobAssetHeader))), out blobAssetPtr))
				{
					return blobAssetPtr.Hash;
				}
				return blobAssetReferenceData->Header->Hash;
			}

			// Token: 0x06000335 RID: 821 RVA: 0x00012674 File Offset: 0x00010874
			private unsafe void AppendComponentData(PackedComponent component, void* ptr, int sizeOf)
			{
				PackedComponentDataChange packedComponentDataChange = default(PackedComponentDataChange);
				packedComponentDataChange.Component = component;
				packedComponentDataChange.Offset = 0;
				packedComponentDataChange.Size = sizeOf;
				packedComponentDataChange.Enabled = -1;
				this.CacheData.SetComponents.Add(in packedComponentDataChange);
				this.CacheData.ComponentData.AddRange(ptr, sizeOf);
			}

			// Token: 0x06000336 RID: 822 RVA: 0x000126D0 File Offset: 0x000108D0
			private unsafe void AppendComponentData(PackedComponent component, void* ptr, int sizeOf, int enabled)
			{
				PackedComponentDataChange packedComponentDataChange = default(PackedComponentDataChange);
				packedComponentDataChange.Component = component;
				packedComponentDataChange.Offset = 0;
				packedComponentDataChange.Size = sizeOf;
				packedComponentDataChange.Enabled = enabled;
				this.CacheData.SetComponents.Add(in packedComponentDataChange);
				this.CacheData.ComponentData.AddRange(ptr, sizeOf);
			}

			// Token: 0x06000337 RID: 823 RVA: 0x0001272C File Offset: 0x0001092C
			private void AppendEmptyComponentData(PackedComponent component, int enabled)
			{
				PackedComponentDataChange packedComponentDataChange = default(PackedComponentDataChange);
				packedComponentDataChange.Component = component;
				packedComponentDataChange.Offset = 0;
				packedComponentDataChange.Size = 0;
				packedComponentDataChange.Enabled = enabled;
				this.CacheData.SetComponents.Add(in packedComponentDataChange);
			}

			// Token: 0x06000338 RID: 824 RVA: 0x00012774 File Offset: 0x00010974
			private void AddendSharedComponentData(EntityGuid entityGuid, TypeIndex typeIndex, int afterSharedComponentIndex, int beforeSharedComponentIndex = -1)
			{
				EntityDiffer.DeferredSharedComponentChange deferredSharedComponentChange = default(EntityDiffer.DeferredSharedComponentChange);
				deferredSharedComponentChange.EntityGuid = entityGuid;
				deferredSharedComponentChange.TypeIndex = typeIndex;
				deferredSharedComponentChange.AfterSharedComponentIndex = afterSharedComponentIndex;
				deferredSharedComponentChange.BeforeSharedComponentIndex = beforeSharedComponentIndex;
				this.CacheData.SharedComponentChanges.Add(in deferredSharedComponentChange);
			}

			// Token: 0x06000339 RID: 825 RVA: 0x000127BC File Offset: 0x000109BC
			private void AppendManagedComponentData(EntityGuid entityGuid, TypeIndex typeIndex, int afterManagedComponentIndex, int beforeManagedComponentIndex = -1)
			{
				EntityDiffer.DeferredManagedComponentChange deferredManagedComponentChange = default(EntityDiffer.DeferredManagedComponentChange);
				deferredManagedComponentChange.EntityGuid = entityGuid;
				deferredManagedComponentChange.TypeIndex = typeIndex;
				deferredManagedComponentChange.AfterManagedComponentIndex = afterManagedComponentIndex;
				deferredManagedComponentChange.BeforeManagedComponentIndex = beforeManagedComponentIndex;
				this.CacheData.ManagedComponentChanges.Add(in deferredManagedComponentChange);
			}

			// Token: 0x0600033A RID: 826 RVA: 0x00012804 File Offset: 0x00010A04
			private unsafe void AppendEnableableComponentData(PackedComponent component, void* ptr, int sizeOf)
			{
				PackedComponentDataChange packedComponentDataChange = default(PackedComponentDataChange);
				packedComponentDataChange.Component = component;
				packedComponentDataChange.Offset = 0;
				packedComponentDataChange.Size = sizeOf;
				this.CacheData.SetComponents.Add(in packedComponentDataChange);
				this.CacheData.ComponentData.AddRange(ptr, sizeOf);
			}

			// Token: 0x0600033B RID: 827 RVA: 0x00012858 File Offset: 0x00010A58
			private unsafe PackedComponent PackComponent(EntityGuid entityGuid, TypeIndex typeIndex, int tableHint, int entryHint)
			{
				ComponentTypeFlags componentTypeFlags = ComponentTypeFlags.None;
				if (typeIndex.IsChunkComponent)
				{
					componentTypeFlags |= ComponentTypeFlags.ChunkComponent;
				}
				ulong stableTypeHash = this.ReadData.TypeInfo[typeIndex.Index].StableTypeHash;
				int num = this.ReadData.Entities.Get(entityGuid, tableHint, entryHint);
				int num2 = this.ReadData.ComponentTypes.Get(new ComponentTypeHash
				{
					StableTypeHash = stableTypeHash,
					Flags = componentTypeFlags
				});
				if (num2 == -1)
				{
					throw new Exception(string.Format("ComponentTypes: Unable to find {0}:{1}", stableTypeHash, componentTypeFlags));
				}
				return new PackedComponent
				{
					PackedEntityIndex = num,
					PackedTypeIndex = num2
				};
			}

			// Token: 0x0600033C RID: 828 RVA: 0x0001290F File Offset: 0x00010B0F
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[Conditional("UNITY_DOTS_DEBUG")]
			private void ThrowOnEntityGuidMissing()
			{
				throw new Exception("LinkedEntityGroup child is missing an EntityGuid component.");
			}

			// Token: 0x0600033D RID: 829 RVA: 0x0001291C File Offset: 0x00010B1C
			private unsafe EntityGuid GetEntityGuid(EntityComponentStore* entityComponentStore, TypeIndex entityGuidTypeIndex, Entity entity)
			{
				EntityGuid entityGuid;
				EntityDiffer.TryGetEntityGuidComponent(entityComponentStore, entity, entityGuidTypeIndex, out entityGuid);
				return entityGuid;
			}

			// Token: 0x0600033E RID: 830 RVA: 0x00012935 File Offset: 0x00010B35
			private void WriteArray<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(NativeList<T> dest, ref NativeList<T> src) where T : struct, ValueType
			{
				if (src.IsEmpty)
				{
					return;
				}
				dest.AddRange(src.AsArray());
				src.Clear();
			}

			// Token: 0x0600033F RID: 831 RVA: 0x00012954 File Offset: 0x00010B54
			private void WriteArrayDeepCopy(NativeList<FilteredArchetype> dest, ref NativeList<FilteredArchetype> src, ref int currentIndex)
			{
				if (src.IsEmpty)
				{
					return;
				}
				for (int i = 0; i < src.Length; i++)
				{
					int num = currentIndex + i;
					ref FilteredArchetype ptr = ref dest.ElementAt(num);
					ptr.EntityCount += src[i].EntityCount;
					ptr.TypeIndices.AddRange(src[i].TypeIndices);
					ptr.PackedEntityIndices.AddRange(src[i].PackedEntityIndices);
					FilteredArchetype filteredArchetype = src[i];
					filteredArchetype.TypeIndices.Dispose();
					filteredArchetype = src[i];
					filteredArchetype.PackedEntityIndices.Dispose();
				}
				currentIndex += src.Length;
				src.Clear();
			}

			// Token: 0x06000340 RID: 832 RVA: 0x00012A08 File Offset: 0x00010C08
			public void WriteToOutputData(ref int archteypeCount)
			{
				this.WriteArray<PackedComponent>(this.OutputData.AddComponents, ref this.CacheData.AddComponents);
				this.WriteArrayDeepCopy(this.OutputData.AddArchetypes, ref this.CacheData.AddArchetypes, ref archteypeCount);
				this.WriteArray<PackedComponentDataChange>(this.OutputData.SetComponents, ref this.CacheData.SetComponents);
				this.WriteArray<PackedComponent>(this.OutputData.RemoveComponents, ref this.CacheData.RemoveComponents);
				this.WriteArray<EntityReferenceChange>(this.OutputData.EntityReferencePatches, ref this.CacheData.EntityReferencePatches);
				this.WriteArray<BlobAssetReferenceChange>(this.OutputData.BlobAssetReferenceChanges, ref this.CacheData.BlobAssetReferenceChanges);
				this.WriteArray<LinkedEntityGroupChange>(this.OutputData.LinkedEntityGroupAdditions, ref this.CacheData.LinkedEntityGroupAdditions);
				this.WriteArray<LinkedEntityGroupChange>(this.OutputData.LinkedEntityGroupRemovals, ref this.CacheData.LinkedEntityGroupRemovals);
				this.WriteArray<byte>(this.OutputData.ComponentData, ref this.CacheData.ComponentData);
				this.WriteArray<EntityDiffer.DeferredSharedComponentChange>(this.OutputData.SharedComponentChanges, ref this.CacheData.SharedComponentChanges);
				this.WriteArray<EntityDiffer.DeferredManagedComponentChange>(this.OutputData.ManagedComponentChanges, ref this.CacheData.ManagedComponentChanges);
				this.WriteArray<EntityDiffer.NonSerializableDebugInfo>(this.OutputData.NonSerializableComponents, ref this.CacheData.NonSerializableComponents);
			}

			// Token: 0x04000167 RID: 359
			public EntityDiffer.GatherComponentChangesReadOnlyData ReadData;

			// Token: 0x04000168 RID: 360
			public EntityDiffer.GatherComponentChangesWriteOnlyData CacheData;

			// Token: 0x04000169 RID: 361
			public EntityDiffer.GatherComponentChangesOutput OutputData;

			// Token: 0x0400016A RID: 362
			public NativeHashMap<ulong, int> AddedArchetypes;

			// Token: 0x0400016B RID: 363
			public unsafe EntityComponentStore* AfterEntityComponentStore;

			// Token: 0x0400016C RID: 364
			public unsafe EntityComponentStore* BeforeEntityComponentStore;
		}

		// Token: 0x0200009D RID: 157
		private struct NonSerializableDebugInfo
		{
			// Token: 0x0400016D RID: 365
			public int OriginatingId;

			// Token: 0x0400016E RID: 366
			public int OriginatingSubId;

			// Token: 0x0400016F RID: 367
			public TypeIndex TypeIndex;

			// Token: 0x04000170 RID: 368
			public int SharedComponent;
		}

		// Token: 0x0200009E RID: 158
		private struct GatherComponentChangesOutput
		{
			// Token: 0x04000171 RID: 369
			[WriteOnly]
			public NativeList<PackedComponent> AddComponents;

			// Token: 0x04000172 RID: 370
			[WriteOnly]
			public NativeList<FilteredArchetype> AddArchetypes;

			// Token: 0x04000173 RID: 371
			[WriteOnly]
			public NativeList<PackedComponentDataChange> SetComponents;

			// Token: 0x04000174 RID: 372
			[WriteOnly]
			public NativeList<PackedComponent> RemoveComponents;

			// Token: 0x04000175 RID: 373
			[WriteOnly]
			public NativeList<EntityReferenceChange> EntityReferencePatches;

			// Token: 0x04000176 RID: 374
			[WriteOnly]
			public NativeList<BlobAssetReferenceChange> BlobAssetReferenceChanges;

			// Token: 0x04000177 RID: 375
			[WriteOnly]
			public NativeList<LinkedEntityGroupChange> LinkedEntityGroupAdditions;

			// Token: 0x04000178 RID: 376
			[WriteOnly]
			public NativeList<LinkedEntityGroupChange> LinkedEntityGroupRemovals;

			// Token: 0x04000179 RID: 377
			[WriteOnly]
			public NativeList<byte> ComponentData;

			// Token: 0x0400017A RID: 378
			[WriteOnly]
			public NativeList<EntityDiffer.DeferredSharedComponentChange> SharedComponentChanges;

			// Token: 0x0400017B RID: 379
			[WriteOnly]
			public NativeList<EntityDiffer.DeferredManagedComponentChange> ManagedComponentChanges;

			// Token: 0x0400017C RID: 380
			[WriteOnly]
			public NativeList<EntityDiffer.NonSerializableDebugInfo> NonSerializableComponents;
		}

		// Token: 0x0200009F RID: 159
		private struct GatherComponentChangesReadOnlyData
		{
			// Token: 0x0400017D RID: 381
			public TypeIndex EntityGuidTypeIndex;

			// Token: 0x0400017E RID: 382
			public TypeIndex LinkedEntityGroupTypeIndex;

			// Token: 0x0400017F RID: 383
			[ReadOnly]
			[NativeDisableUnsafePtrRestriction]
			public unsafe TypeManager.TypeInfo* TypeInfo;

			// Token: 0x04000180 RID: 384
			[ReadOnly]
			[NativeDisableUnsafePtrRestriction]
			public unsafe TypeManager.EntityOffsetInfo* EntityOffsets;

			// Token: 0x04000181 RID: 385
			[ReadOnly]
			[NativeDisableUnsafePtrRestriction]
			public unsafe TypeManager.EntityOffsetInfo* BlobAssetRefOffsets;

			// Token: 0x04000182 RID: 386
			[ReadOnly]
			[NativeDisableUnsafePtrRestriction]
			public unsafe EntityComponentStore* AfterEntityComponentStore;

			// Token: 0x04000183 RID: 387
			[ReadOnly]
			[NativeDisableUnsafePtrRestriction]
			public unsafe EntityComponentStore* BeforeEntityComponentStore;

			// Token: 0x04000184 RID: 388
			[ReadOnly]
			public NativeList<EntityDiffer.CreatedEntity> CreatedEntities;

			// Token: 0x04000185 RID: 389
			[ReadOnly]
			public NativeList<EntityDiffer.ModifiedEntity> ModifiedEntities;

			// Token: 0x04000186 RID: 390
			[ReadOnly]
			public EntityDiffer.PackedEntityGuidsCollection Entities;

			// Token: 0x04000187 RID: 391
			[ReadOnly]
			public EntityDiffer.PackedCollection<ComponentTypeHash> ComponentTypes;

			// Token: 0x04000188 RID: 392
			[ReadOnly]
			[NativeDisableContainerSafetyRestriction]
			public NativeParallelHashMap<BlobAssetPtr, BlobAssetPtr> AfterBlobAssetRemap;

			// Token: 0x04000189 RID: 393
			[ReadOnly]
			[NativeDisableContainerSafetyRestriction]
			public NativeParallelHashMap<BlobAssetPtr, BlobAssetPtr> BeforeBlobAssetRemap;

			// Token: 0x0400018A RID: 394
			[ReadOnly]
			public byte UseReferentialEquivalence;
		}

		// Token: 0x020000A0 RID: 160
		private struct GatherComponentChangesWriteOnlyData
		{
			// Token: 0x0400018B RID: 395
			[WriteOnly]
			public NativeList<PackedComponent> AddComponents;

			// Token: 0x0400018C RID: 396
			[WriteOnly]
			public NativeList<FilteredArchetype> AddArchetypes;

			// Token: 0x0400018D RID: 397
			[WriteOnly]
			public NativeList<PackedComponentDataChange> SetComponents;

			// Token: 0x0400018E RID: 398
			[WriteOnly]
			public NativeList<PackedComponent> RemoveComponents;

			// Token: 0x0400018F RID: 399
			[WriteOnly]
			public NativeList<EntityReferenceChange> EntityReferencePatches;

			// Token: 0x04000190 RID: 400
			[WriteOnly]
			public NativeList<BlobAssetReferenceChange> BlobAssetReferenceChanges;

			// Token: 0x04000191 RID: 401
			[WriteOnly]
			public NativeList<LinkedEntityGroupChange> LinkedEntityGroupAdditions;

			// Token: 0x04000192 RID: 402
			[WriteOnly]
			public NativeList<LinkedEntityGroupChange> LinkedEntityGroupRemovals;

			// Token: 0x04000193 RID: 403
			[WriteOnly]
			public NativeList<byte> ComponentData;

			// Token: 0x04000194 RID: 404
			[WriteOnly]
			public NativeList<EntityDiffer.DeferredSharedComponentChange> SharedComponentChanges;

			// Token: 0x04000195 RID: 405
			[WriteOnly]
			public NativeList<EntityDiffer.DeferredManagedComponentChange> ManagedComponentChanges;

			// Token: 0x04000196 RID: 406
			[WriteOnly]
			public NativeList<EntityDiffer.NonSerializableDebugInfo> NonSerializableComponents;
		}

		// Token: 0x020000A1 RID: 161
		[BurstCompile]
		private struct GatherComponentChangesJob : IJob
		{
			// Token: 0x06000341 RID: 833 RVA: 0x00012B68 File Offset: 0x00010D68
			public void Execute()
			{
				int num = 2048;
				int num2 = 128;
				int num3 = 128;
				int num4 = 1024;
				int num5 = 128;
				int num6 = 128;
				int num7 = 128;
				int num8 = 128;
				int num9 = 32768;
				int num10 = 2048;
				int num11 = 128;
				int num12 = 128;
				EntityDiffer.GatherComponentChangesWriteOnlyData gatherComponentChangesWriteOnlyData = new EntityDiffer.GatherComponentChangesWriteOnlyData
				{
					AddComponents = new NativeList<PackedComponent>(num, Allocator.Temp),
					AddArchetypes = new NativeList<FilteredArchetype>(num2, Allocator.Temp),
					RemoveComponents = new NativeList<PackedComponent>(num3, Allocator.Temp),
					SetComponents = new NativeList<PackedComponentDataChange>(num4, Allocator.Temp),
					LinkedEntityGroupAdditions = new NativeList<LinkedEntityGroupChange>(num5, Allocator.Temp),
					LinkedEntityGroupRemovals = new NativeList<LinkedEntityGroupChange>(num6, Allocator.Temp),
					BlobAssetReferenceChanges = new NativeList<BlobAssetReferenceChange>(num7, Allocator.Temp),
					EntityReferencePatches = new NativeList<EntityReferenceChange>(num8, Allocator.Temp),
					ComponentData = new NativeList<byte>(num9, Allocator.Temp),
					SharedComponentChanges = new NativeList<EntityDiffer.DeferredSharedComponentChange>(num10, Allocator.Temp),
					ManagedComponentChanges = new NativeList<EntityDiffer.DeferredManagedComponentChange>(num11, Allocator.Temp),
					NonSerializableComponents = new NativeList<EntityDiffer.NonSerializableDebugInfo>(num12, Allocator.Temp)
				};
				NativeHashMap<ulong, int> nativeHashMap = new NativeHashMap<ulong, int>(16, Allocator.Temp);
				EntityDiffer.GatherComponentChanges gatherComponentChanges = new EntityDiffer.GatherComponentChanges
				{
					ReadData = this.ReadData,
					CacheData = gatherComponentChangesWriteOnlyData,
					OutputData = this.OutputData,
					AddedArchetypes = nativeHashMap,
					AfterEntityComponentStore = this.AfterEntityComponentStore,
					BeforeEntityComponentStore = this.BeforeEntityComponentStore
				};
				int num13 = this.ReadData.CreatedEntities.Length + this.ReadData.ModifiedEntities.Length;
				int num14 = 0;
				for (int i = 0; i < num13; i += 128)
				{
					gatherComponentChanges.ProcessRange(i);
					gatherComponentChanges.WriteToOutputData(ref num14);
				}
				gatherComponentChanges.CacheData.ManagedComponentChanges.Dispose();
				gatherComponentChanges.CacheData.SharedComponentChanges.Dispose();
				gatherComponentChanges.CacheData.ComponentData.Dispose();
				gatherComponentChanges.CacheData.EntityReferencePatches.Dispose();
				gatherComponentChanges.CacheData.BlobAssetReferenceChanges.Dispose();
				gatherComponentChanges.CacheData.LinkedEntityGroupRemovals.Dispose();
				gatherComponentChanges.CacheData.LinkedEntityGroupAdditions.Dispose();
				gatherComponentChanges.CacheData.SetComponents.Dispose();
				gatherComponentChanges.CacheData.RemoveComponents.Dispose();
				gatherComponentChanges.CacheData.AddComponents.Dispose();
				foreach (FilteredArchetype filteredArchetype in gatherComponentChanges.CacheData.AddArchetypes)
				{
					UnsafeList<TypeIndex> typeIndices = filteredArchetype.TypeIndices;
					typeIndices.Dispose();
					UnsafeList<int> packedEntityIndices = filteredArchetype.PackedEntityIndices;
					packedEntityIndices.Dispose();
				}
				gatherComponentChanges.CacheData.AddArchetypes.Dispose();
				gatherComponentChanges.CacheData.NonSerializableComponents.Dispose();
				nativeHashMap.Dispose();
			}

			// Token: 0x04000197 RID: 407
			public EntityDiffer.GatherComponentChangesReadOnlyData ReadData;

			// Token: 0x04000198 RID: 408
			public EntityDiffer.GatherComponentChangesOutput OutputData;

			// Token: 0x04000199 RID: 409
			[NativeDisableUnsafePtrRestriction]
			public unsafe EntityComponentStore* AfterEntityComponentStore;

			// Token: 0x0400019A RID: 410
			[NativeDisableUnsafePtrRestriction]
			public unsafe EntityComponentStore* BeforeEntityComponentStore;
		}

		// Token: 0x020000A2 RID: 162
		[BurstCompile]
		private struct GetEntityNamesJob : IJob
		{
			// Token: 0x06000342 RID: 834 RVA: 0x00012EA8 File Offset: 0x000110A8
			private unsafe bool TryGetEntityGuid(EntityComponentStore* entityComponentStoreEntity, Entity entity, out EntityGuid entityGuid)
			{
				entityGuid = default(EntityGuid);
				return entityComponentStoreEntity->Exists(entity) && EntityDiffer.TryGetEntityGuidComponent(entityComponentStoreEntity, entity, this.EntityGuidTypeIndex, out entityGuid);
			}

			// Token: 0x06000343 RID: 835 RVA: 0x00012ED0 File Offset: 0x000110D0
			[BurstCompile]
			public unsafe void Execute()
			{
				FixedString64Bytes* unsafeReadOnlyPtr = (FixedString64Bytes*)this.NameChanges.Names.GetUnsafeReadOnlyPtr<FixedString64Bytes>();
				EntityGuid* unsafeReadOnlyPtr2 = (EntityGuid*)this.NameChanges.NameChangedEntityGuids.GetUnsafeReadOnlyPtr<EntityGuid>();
				int num = this.CreatedEntities.Length + this.NameModifiedEntities.Length + this.DestroyedEntities.Length;
				UnsafeParallelHashSet<EntityGuid> unsafeParallelHashSet = new UnsafeParallelHashSet<EntityGuid>(num, Allocator.TempJob);
				int num2 = 0;
				int num3 = 0;
				for (int i = 0; i < this.CreatedEntities.Length; i++)
				{
					Entity entityFromEntityInChunk = ChunkDataUtility.GetEntityFromEntityInChunk(this.AfterEntityComponentStore->GetArchetype(this.CreatedEntities[i].AfterEntityInChunk.Chunk), this.CreatedEntities[i].AfterEntityInChunk);
					this.AfterEntityComponentStore->GetName(entityFromEntityInChunk, out unsafeReadOnlyPtr[(IntPtr)(num2++) * (IntPtr)sizeof(FixedString64Bytes)]);
					unsafeParallelHashSet.Add(this.CreatedEntities[i].EntityGuid);
				}
				for (int j = 0; j < this.NameModifiedEntities.Length; j++)
				{
					Entity entity = this.NameModifiedEntities[j].Entity;
					this.AfterEntityComponentStore->GetName(entity, out unsafeReadOnlyPtr[(IntPtr)(num2++) * (IntPtr)sizeof(FixedString64Bytes)]);
					unsafeReadOnlyPtr2[(IntPtr)(num3++) * (IntPtr)sizeof(EntityGuid)] = this.NameModifiedEntities[j].EntityGuid;
					unsafeParallelHashSet.Add(this.NameModifiedEntities[j].EntityGuid);
				}
				if (this.AfterEntityComponentStore->NameChangeBitsSequenceNum == this.BeforeEntityComponentStore->NameChangeBitsSequenceNum)
				{
					foreach (Entity entity2 in this.AfterEntityComponentStore->NameStoreAccess.GetEntityWithNameSetRO())
					{
						EntityGuid entityGuid;
						if (this.TryGetEntityGuid(this.AfterEntityComponentStore, entity2, out entityGuid) && !unsafeParallelHashSet.Contains(entityGuid))
						{
							this.AfterEntityComponentStore->GetName(entity2, out unsafeReadOnlyPtr[(IntPtr)(num2++) * (IntPtr)sizeof(FixedString64Bytes)]);
							unsafeReadOnlyPtr2[(IntPtr)(num3++) * (IntPtr)sizeof(EntityGuid)] = entityGuid;
						}
					}
				}
				for (int k = 0; k < this.DestroyedEntities.Length; k++)
				{
					Entity entityFromEntityInChunk2 = ChunkDataUtility.GetEntityFromEntityInChunk(this.BeforeEntityComponentStore->GetArchetype(this.DestroyedEntities[k].BeforeEntityInChunk.Chunk), this.DestroyedEntities[k].BeforeEntityInChunk);
					this.BeforeEntityComponentStore->GetName(entityFromEntityInChunk2, out unsafeReadOnlyPtr[(IntPtr)(num2++) * (IntPtr)sizeof(FixedString64Bytes)]);
				}
				*this.NameChangeCount = num3;
				unsafeParallelHashSet.Dispose();
			}

			// Token: 0x0400019B RID: 411
			public TypeIndex EntityGuidTypeIndex;

			// Token: 0x0400019C RID: 412
			public EntityDiffer.NameChangeSet NameChanges;

			// Token: 0x0400019D RID: 413
			public NativeList<EntityDiffer.CreatedEntity> CreatedEntities;

			// Token: 0x0400019E RID: 414
			public NativeList<EntityDiffer.DestroyedEntity> DestroyedEntities;

			// Token: 0x0400019F RID: 415
			public NativeList<EntityDiffer.NameModifiedEntity> NameModifiedEntities;

			// Token: 0x040001A0 RID: 416
			public EntityNameStoreAccess NameStoreAccess;

			// Token: 0x040001A1 RID: 417
			[NativeDisableUnsafePtrRestriction]
			public unsafe int* NameChangeCount;

			// Token: 0x040001A2 RID: 418
			[NativeDisableUnsafePtrRestriction]
			public unsafe EntityComponentStore* AfterEntityComponentStore;

			// Token: 0x040001A3 RID: 419
			[NativeDisableUnsafePtrRestriction]
			public unsafe EntityComponentStore* BeforeEntityComponentStore;
		}

		// Token: 0x020000A3 RID: 163
		private class ManagedObjectPatches : PropertyVisitor, IVisitPropertyAdapter<Entity>, IPropertyVisitorAdapter, IVisitPropertyAdapter<BlobAssetReferenceData>
		{
			// Token: 0x06000344 RID: 836 RVA: 0x0001319C File Offset: 0x0001139C
			public unsafe ManagedObjectPatches(EntityComponentStore* entityComponentStore)
			{
				this.m_EntityComponentStore = entityComponentStore;
				base.AddAdapter(this);
			}

			// Token: 0x06000345 RID: 837 RVA: 0x000131B4 File Offset: 0x000113B4
			public void ExtractPatches(ref object value, PackedComponent component, NativeList<EntityReferenceChange> entityReferencePatches, NativeList<BlobAssetReferenceChange> blobAssetReferencePatches)
			{
				this.m_Component = component;
				this.m_EntityReferencePatches = entityReferencePatches;
				this.m_BlobAssetReferencePatches = blobAssetReferencePatches;
				this.m_EntityReferencePatchId = 0;
				this.m_BlobAssetReferencePatchId = 0;
				VisitReturnCode visitReturnCode;
				PropertyContainer.TryAccept<object>(this, ref value, out visitReturnCode, default(VisitParameters));
			}

			// Token: 0x06000346 RID: 838 RVA: 0x000131F8 File Offset: 0x000113F8
			protected override void VisitProperty<TContainer, TValue>(Property<TContainer, TValue> property, ref TContainer container, ref TValue value)
			{
				if (typeof(Object).IsAssignableFrom(typeof(TValue)))
				{
					return;
				}
				base.VisitProperty<TContainer, TValue>(property, ref container, ref value);
			}

			// Token: 0x06000347 RID: 839 RVA: 0x00013220 File Offset: 0x00011420
			unsafe void IVisitPropertyAdapter<Entity>.Visit<TContainer>(in VisitContext<TContainer, Entity> context, ref TContainer container, ref Entity value)
			{
				if (!this.m_EntityReferencePatches.IsCreated)
				{
					return;
				}
				EntityGuid entityGuid = default(EntityGuid);
				bool flag;
				if (this.m_EntityComponentStore->HasComponent(value, TypeManager.GetTypeIndex<EntityGuid>(), out flag))
				{
					entityGuid = *(EntityGuid*)this.m_EntityComponentStore->GetComponentDataWithTypeRO(value, TypeManager.GetTypeIndex<EntityGuid>());
				}
				value = new Entity
				{
					Index = this.m_EntityReferencePatchId,
					Version = -1
				};
				EntityReferenceChange entityReferenceChange = default(EntityReferenceChange);
				entityReferenceChange.Component = this.m_Component;
				int entityReferencePatchId = this.m_EntityReferencePatchId;
				this.m_EntityReferencePatchId = entityReferencePatchId + 1;
				entityReferenceChange.Offset = entityReferencePatchId;
				entityReferenceChange.Value = entityGuid;
				this.m_EntityReferencePatches.Add(in entityReferenceChange);
			}

			// Token: 0x06000348 RID: 840 RVA: 0x000132E4 File Offset: 0x000114E4
			unsafe void IVisitPropertyAdapter<BlobAssetReferenceData>.Visit<TContainer>(in VisitContext<TContainer, BlobAssetReferenceData> context, ref TContainer container, ref BlobAssetReferenceData value)
			{
				if (!this.m_BlobAssetReferencePatches.IsCreated)
				{
					return;
				}
				ulong num = 0UL;
				if (value.m_Ptr != null)
				{
					num = value.Header->Hash;
				}
				value.m_Align8Union = (long)this.m_BlobAssetReferencePatchId;
				BlobAssetReferenceChange blobAssetReferenceChange = default(BlobAssetReferenceChange);
				blobAssetReferenceChange.Component = this.m_Component;
				int blobAssetReferencePatchId = this.m_BlobAssetReferencePatchId;
				this.m_BlobAssetReferencePatchId = blobAssetReferencePatchId + 1;
				blobAssetReferenceChange.Offset = blobAssetReferencePatchId;
				blobAssetReferenceChange.Value = num;
				this.m_BlobAssetReferencePatches.Add(in blobAssetReferenceChange);
			}

			// Token: 0x040001A4 RID: 420
			private unsafe readonly EntityComponentStore* m_EntityComponentStore;

			// Token: 0x040001A5 RID: 421
			private PackedComponent m_Component;

			// Token: 0x040001A6 RID: 422
			private NativeList<EntityReferenceChange> m_EntityReferencePatches;

			// Token: 0x040001A7 RID: 423
			private NativeList<BlobAssetReferenceChange> m_BlobAssetReferencePatches;

			// Token: 0x040001A8 RID: 424
			private int m_EntityReferencePatchId;

			// Token: 0x040001A9 RID: 425
			private int m_BlobAssetReferencePatchId;
		}

		// Token: 0x020000A4 RID: 164
		[BurstCompile]
		private struct PatchAndAddClonedChunks : IJobParallelFor
		{
			// Token: 0x06000349 RID: 841 RVA: 0x00013368 File Offset: 0x00011568
			public unsafe void Execute(int index)
			{
				ChunkIndex chunk = this.SrcChunks[index].m_Chunk;
				ChunkIndex chunk2 = this.DstChunks[index].m_Chunk;
				Archetype* archetype = this.SrcEntityComponentStore->GetArchetype(chunk);
				Archetype* archetype2 = this.DstEntityComponentStore->GetArchetype(chunk2);
				ChunkDataUtility.CloneChangeVersions(archetype, chunk.ListIndex, archetype2, chunk2.ListIndex, false);
				this.DstEntityComponentStore->AddExistingEntitiesInChunk(chunk2);
			}

			// Token: 0x040001AA RID: 426
			[ReadOnly]
			public NativeArray<ArchetypeChunk> SrcChunks;

			// Token: 0x040001AB RID: 427
			[ReadOnly]
			public NativeArray<ArchetypeChunk> DstChunks;

			// Token: 0x040001AC RID: 428
			[NativeDisableUnsafePtrRestriction]
			public unsafe EntityComponentStore* SrcEntityComponentStore;

			// Token: 0x040001AD RID: 429
			[NativeDisableUnsafePtrRestriction]
			public unsafe EntityComponentStore* DstEntityComponentStore;
		}

		// Token: 0x020000A5 RID: 165
		[BurstCompile]
		private struct DestroyChunksJob : IJob
		{
			// Token: 0x0600034A RID: 842 RVA: 0x000133D4 File Offset: 0x000115D4
			public unsafe void Execute()
			{
				EntityComponentStore* entityComponentStore = this.EntityManager.GetCheckedEntityDataAccess()->EntityComponentStore;
				for (int i = 0; i < this.Chunks.Length; i++)
				{
					ChunkIndex chunk = this.Chunks[i].m_Chunk;
					Archetype* archetype = this.Chunks[i].Archetype.Archetype;
					int count = chunk.Count;
					ChunkDataUtility.DeallocateBuffers(archetype, chunk);
					entityComponentStore->DeallocateManagedComponents(chunk, 0, count);
					archetype->EntityCount -= chunk.Count;
					entityComponentStore->FreeEntities(chunk);
					ChunkDataUtility.SetChunkCountKeepMetaChunk(archetype, chunk, 0);
				}
			}

			// Token: 0x040001AE RID: 430
			public EntityManager EntityManager;

			// Token: 0x040001AF RID: 431
			public NativeList<ArchetypeChunk> Chunks;
		}

		// Token: 0x020000A6 RID: 166
		[BurstCompile]
		private struct CollectSharedComponentIndices : IJob
		{
			// Token: 0x0600034B RID: 843 RVA: 0x0001346C File Offset: 0x0001166C
			public void Execute()
			{
				UnsafeParallelHashSet<int> unsafeParallelHashSet = new UnsafeParallelHashSet<int>(128, Allocator.Temp);
				for (int i = 0; i < this.Chunks.Length; i++)
				{
					this.HandleChunk(this.Chunks[i].Archetype.Archetype, this.Chunks[i].m_Chunk, ref unsafeParallelHashSet);
				}
				this.SharedComponentIndices.AddRange(unsafeParallelHashSet.ToNativeArray(Allocator.Temp));
			}

			// Token: 0x0600034C RID: 844 RVA: 0x000134EC File Offset: 0x000116EC
			private unsafe void HandleChunk(Archetype* srcArchetype, ChunkIndex srcChunk, ref UnsafeParallelHashSet<int> indices)
			{
				int numSharedComponents = srcArchetype->NumSharedComponents;
				int* ptr;
				checked
				{
					ptr = stackalloc int[unchecked((UIntPtr)srcArchetype->NumSharedComponents) * 4];
					srcArchetype->Chunks.GetSharedComponentValues(srcChunk.ListIndex).CopyTo(ptr, 0, srcArchetype->NumSharedComponents);
				}
				for (int i = 0; i < numSharedComponents; i++)
				{
					indices.Add(ptr[i]);
				}
			}

			// Token: 0x040001B0 RID: 432
			public NativeList<ArchetypeChunk> Chunks;

			// Token: 0x040001B1 RID: 433
			public NativeList<int> SharedComponentIndices;
		}

		// Token: 0x020000A7 RID: 167
		[BurstCompile]
		private struct CreateNewChunks : IJob
		{
			// Token: 0x0600034D RID: 845 RVA: 0x00013548 File Offset: 0x00011748
			public unsafe void Execute()
			{
				EntityComponentStore* entityComponentStore = this.DstEntityManager.GetCheckedEntityDataAccess()->EntityComponentStore;
				UnsafeParallelHashMap<int, int> unsafeParallelHashMap = new UnsafeParallelHashMap<int, int>(this.SrcSharedComponentIndices.Length, Allocator.Temp);
				for (int i = 0; i < this.SrcSharedComponentIndices.Length; i++)
				{
					unsafeParallelHashMap.Add(this.SrcSharedComponentIndices[i], this.DstSharedComponentIndices[i]);
				}
				for (int j = 0; j < this.Chunks.Length; j++)
				{
					this.HandleChunk(j, entityComponentStore, unsafeParallelHashMap);
				}
			}

			// Token: 0x0600034E RID: 846 RVA: 0x000135D4 File Offset: 0x000117D4
			private unsafe void HandleChunk(int idx, EntityComponentStore* dstEntityComponentStore, UnsafeParallelHashMap<int, int> sharedComponentRemap)
			{
				ChunkIndex chunk = this.Chunks[idx].m_Chunk;
				Archetype* archetype = this.Chunks[idx].Archetype.Archetype;
				int numSharedComponents = archetype->NumSharedComponents;
				int* ptr;
				checked
				{
					ptr = stackalloc int[unchecked((UIntPtr)numSharedComponents) * 4];
					archetype->Chunks.GetSharedComponentValues(chunk.ListIndex).CopyTo(ptr, 0, numSharedComponents);
				}
				for (int i = 0; i < numSharedComponents; i++)
				{
					ptr[i] = sharedComponentRemap[ptr[i]];
				}
				Archetype* orCreateArchetype = dstEntityComponentStore->GetOrCreateArchetype(archetype->Types, archetype->TypesCount);
				ChunkIndex cleanChunkNoMetaChunk = dstEntityComponentStore->GetCleanChunkNoMetaChunk(orCreateArchetype, ptr);
				cleanChunkNoMetaChunk.MetaChunkEntity = chunk.MetaChunkEntity;
				int count = chunk.Count;
				ChunkDataUtility.SetChunkCountKeepMetaChunk(orCreateArchetype, cleanChunkNoMetaChunk, count);
				orCreateArchetype->EntityCount += count;
				cleanChunkNoMetaChunk.SequenceNumber = chunk.SequenceNumber;
				this.ClonedChunks[idx] = new ArchetypeChunk
				{
					m_Chunk = cleanChunkNoMetaChunk
				};
			}

			// Token: 0x040001B2 RID: 434
			public NativeList<ArchetypeChunk> Chunks;

			// Token: 0x040001B3 RID: 435
			public NativeArray<ArchetypeChunk> ClonedChunks;

			// Token: 0x040001B4 RID: 436
			public EntityManager DstEntityManager;

			// Token: 0x040001B5 RID: 437
			[ReadOnly]
			public NativeArray<int> SrcSharedComponentIndices;

			// Token: 0x040001B6 RID: 438
			[ReadOnly]
			public NativeArray<int> DstSharedComponentIndices;
		}

		// Token: 0x020000A8 RID: 168
		[BurstCompile]
		private struct CopyChunkBuffers : IJobFor
		{
			// Token: 0x0600034F RID: 847 RVA: 0x000136E4 File Offset: 0x000118E4
			public unsafe void Execute(int index)
			{
				ChunkIndex chunk = this.Chunks[index].m_Chunk;
				ChunkIndex chunk2 = this.ClonedChunks[index].m_Chunk;
				UnsafeUtility.MemCpy((void*)chunk2.Buffer, (void*)chunk.Buffer, 16320L);
				BufferHeader.PatchAfterCloningChunk(this.Chunks[index].Archetype.Archetype, chunk2.Buffer, chunk2.Count);
			}

			// Token: 0x040001B7 RID: 439
			[ReadOnly]
			public NativeList<ArchetypeChunk> Chunks;

			// Token: 0x040001B8 RID: 440
			[ReadOnly]
			public NativeArray<ArchetypeChunk> ClonedChunks;
		}

		// Token: 0x020000A9 RID: 169
		internal struct CreatedEntity
		{
			// Token: 0x040001B9 RID: 441
			public EntityGuid EntityGuid;

			// Token: 0x040001BA RID: 442
			public EntityInChunk AfterEntityInChunk;
		}

		// Token: 0x020000AA RID: 170
		internal struct DestroyedEntity
		{
			// Token: 0x040001BB RID: 443
			public EntityGuid EntityGuid;

			// Token: 0x040001BC RID: 444
			public EntityInChunk BeforeEntityInChunk;
		}

		// Token: 0x020000AB RID: 171
		private struct ModifiedEntity
		{
			// Token: 0x040001BD RID: 445
			public EntityGuid EntityGuid;

			// Token: 0x040001BE RID: 446
			public EntityInChunk BeforeEntityInChunk;

			// Token: 0x040001BF RID: 447
			public EntityInChunk AfterEntityInChunk;

			// Token: 0x040001C0 RID: 448
			public bool CanCompareChunkVersions;
		}

		// Token: 0x020000AC RID: 172
		internal struct NameModifiedEntity
		{
			// Token: 0x040001C1 RID: 449
			public EntityGuid EntityGuid;

			// Token: 0x040001C2 RID: 450
			public Entity Entity;
		}

		// Token: 0x020000AD RID: 173
		private readonly struct EntityInChunkChanges : IDisposable
		{
			// Token: 0x06000350 RID: 848 RVA: 0x0001375C File Offset: 0x0001195C
			public EntityInChunkChanges(EntityManager afterEntityManager, EntityManager beforeEntityManager, AllocatorManager.AllocatorHandle allocator)
			{
				this.AfterEntityManager = afterEntityManager;
				this.BeforeEntityManager = beforeEntityManager;
				this.CreatedEntities = new NativeList<EntityDiffer.CreatedEntity>(16, allocator);
				this.ModifiedEntities = new NativeList<EntityDiffer.ModifiedEntity>(16, allocator);
				this.DestroyedEntities = new NativeList<EntityDiffer.DestroyedEntity>(16, allocator);
				this.NameModifiedEntities = new NativeList<EntityDiffer.NameModifiedEntity>(16, allocator);
				this.IsCreated = true;
			}

			// Token: 0x06000351 RID: 849 RVA: 0x000137B8 File Offset: 0x000119B8
			public void Dispose()
			{
				this.CreatedEntities.Dispose();
				this.ModifiedEntities.Dispose();
				this.DestroyedEntities.Dispose();
				this.NameModifiedEntities.Dispose();
			}

			// Token: 0x040001C3 RID: 451
			public readonly EntityManager AfterEntityManager;

			// Token: 0x040001C4 RID: 452
			public readonly EntityManager BeforeEntityManager;

			// Token: 0x040001C5 RID: 453
			public readonly NativeList<EntityDiffer.CreatedEntity> CreatedEntities;

			// Token: 0x040001C6 RID: 454
			public readonly NativeList<EntityDiffer.ModifiedEntity> ModifiedEntities;

			// Token: 0x040001C7 RID: 455
			public readonly NativeList<EntityDiffer.DestroyedEntity> DestroyedEntities;

			// Token: 0x040001C8 RID: 456
			public readonly NativeList<EntityDiffer.NameModifiedEntity> NameModifiedEntities;

			// Token: 0x040001C9 RID: 457
			public readonly bool IsCreated;
		}

		// Token: 0x020000AE RID: 174
		private struct EntityInChunkWithGuid
		{
			// Token: 0x040001CA RID: 458
			public EntityInChunk EntityInChunk;

			// Token: 0x040001CB RID: 459
			public EntityGuid EntityGuid;

			// Token: 0x040001CC RID: 460
			public Entity Entity;

			// Token: 0x040001CD RID: 461
			public int NameIndex;

			// Token: 0x040001CE RID: 462
			public EntityDiffer.ArchetypeChunkChangeFlags Flags;
		}

		// Token: 0x020000AF RID: 175
		private struct EntityInChunkWithGuidComparer : IComparer<EntityDiffer.EntityInChunkWithGuid>
		{
			// Token: 0x06000352 RID: 850 RVA: 0x000137FD File Offset: 0x000119FD
			public int Compare(EntityDiffer.EntityInChunkWithGuid x, EntityDiffer.EntityInChunkWithGuid y)
			{
				return x.EntityGuid.CompareTo(y.EntityGuid);
			}
		}

		// Token: 0x020000B0 RID: 176
		[BurstCompile]
		private struct GatherEntityInChunkWithGuid : IJobParallelFor
		{
			// Token: 0x06000353 RID: 851 RVA: 0x00013814 File Offset: 0x00011A14
			public unsafe void Execute(int index)
			{
				ChunkIndex chunk = this.Chunks[index].m_Chunk;
				EntityDiffer.ArchetypeChunkChangeFlags archetypeChunkChangeFlags = this.Flags[index];
				int num = this.EntityCounts[index];
				byte* buffer = chunk.Buffer;
				Archetype* archetype = this.Chunks[index].Archetype.Archetype;
				int indexInTypeArray = ChunkDataUtility.GetIndexInTypeArray(archetype, this.EntityGuidTypeIndex);
				EntityGuid* ptr = (EntityGuid*)(buffer + archetype->Offsets[indexInTypeArray]);
				int indexInTypeArray2 = ChunkDataUtility.GetIndexInTypeArray(archetype, this.EntityTypeIndex);
				Entity* ptr2 = (Entity*)(buffer + archetype->Offsets[indexInTypeArray2]);
				int num2 = num;
				int i = 0;
				int count = chunk.Count;
				while (i < count)
				{
					Entity entity = ptr2[i];
					int num3 = 0;
					this.Entities[num2++] = new EntityDiffer.EntityInChunkWithGuid
					{
						EntityInChunk = new EntityInChunk
						{
							Chunk = chunk,
							IndexInChunk = i
						},
						EntityGuid = ptr[i],
						Entity = ptr2[i],
						NameIndex = num3,
						Flags = archetypeChunkChangeFlags
					};
					i++;
				}
			}

			// Token: 0x040001CF RID: 463
			public TypeIndex EntityGuidTypeIndex;

			// Token: 0x040001D0 RID: 464
			public TypeIndex EntityTypeIndex;

			// Token: 0x040001D1 RID: 465
			[ReadOnly]
			public NativeList<ArchetypeChunk> Chunks;

			// Token: 0x040001D2 RID: 466
			[ReadOnly]
			public NativeList<EntityDiffer.ArchetypeChunkChangeFlags> Flags;

			// Token: 0x040001D3 RID: 467
			[ReadOnly]
			public NativeList<int> EntityCounts;

			// Token: 0x040001D4 RID: 468
			[NativeDisableParallelForRestriction]
			[WriteOnly]
			public NativeArray<EntityDiffer.EntityInChunkWithGuid> Entities;
		}

		// Token: 0x020000B1 RID: 177
		[BurstCompile]
		private struct SortEntityInChunk : IJob
		{
			// Token: 0x06000354 RID: 852 RVA: 0x00013964 File Offset: 0x00011B64
			public void Execute()
			{
				this.Array.Sort(default(EntityDiffer.EntityInChunkWithGuidComparer));
			}

			// Token: 0x040001D5 RID: 469
			public NativeArray<EntityDiffer.EntityInChunkWithGuid> Array;
		}

		// Token: 0x020000B2 RID: 178
		[BurstCompile]
		private struct GatherEntityChanges : IJob
		{
			// Token: 0x06000355 RID: 853 RVA: 0x00013988 File Offset: 0x00011B88
			public void Execute()
			{
				int i = 0;
				int j = 0;
				while (j < this.BeforeEntities.Length)
				{
					if (i >= this.AfterEntities.Length)
					{
						break;
					}
					EntityDiffer.EntityInChunkWithGuid entityInChunkWithGuid = this.BeforeEntities[j];
					EntityDiffer.EntityInChunkWithGuid entityInChunkWithGuid2 = this.AfterEntities[i];
					int num = entityInChunkWithGuid.EntityGuid.CompareTo(entityInChunkWithGuid2.EntityGuid);
					if (num < 0)
					{
						EntityDiffer.DestroyedEntity destroyedEntity = default(EntityDiffer.DestroyedEntity);
						destroyedEntity.EntityGuid = entityInChunkWithGuid.EntityGuid;
						destroyedEntity.BeforeEntityInChunk = entityInChunkWithGuid.EntityInChunk;
						this.DestroyedEntities.Add(in destroyedEntity);
						j++;
					}
					else if (num == 0)
					{
						EntityDiffer.ModifiedEntity modifiedEntity = default(EntityDiffer.ModifiedEntity);
						modifiedEntity.EntityGuid = entityInChunkWithGuid2.EntityGuid;
						modifiedEntity.AfterEntityInChunk = entityInChunkWithGuid2.EntityInChunk;
						modifiedEntity.BeforeEntityInChunk = entityInChunkWithGuid.EntityInChunk;
						modifiedEntity.CanCompareChunkVersions = (entityInChunkWithGuid.Flags & entityInChunkWithGuid2.Flags & EntityDiffer.ArchetypeChunkChangeFlags.Cloned) == EntityDiffer.ArchetypeChunkChangeFlags.Cloned;
						this.ModifiedEntities.Add(in modifiedEntity);
						if (entityInChunkWithGuid2.NameIndex != entityInChunkWithGuid.NameIndex)
						{
							EntityDiffer.NameModifiedEntity nameModifiedEntity = default(EntityDiffer.NameModifiedEntity);
							nameModifiedEntity.EntityGuid = entityInChunkWithGuid2.EntityGuid;
							nameModifiedEntity.Entity = entityInChunkWithGuid2.Entity;
							this.NameModifiedEntities.Add(in nameModifiedEntity);
						}
						i++;
						j++;
					}
					else
					{
						EntityDiffer.CreatedEntity createdEntity = default(EntityDiffer.CreatedEntity);
						createdEntity.EntityGuid = entityInChunkWithGuid2.EntityGuid;
						createdEntity.AfterEntityInChunk = entityInChunkWithGuid2.EntityInChunk;
						this.CreatedEntities.Add(in createdEntity);
						i++;
					}
				}
				while (j < this.BeforeEntities.Length)
				{
					EntityDiffer.EntityInChunkWithGuid entityInChunkWithGuid3 = this.BeforeEntities[j];
					EntityDiffer.DestroyedEntity destroyedEntity = default(EntityDiffer.DestroyedEntity);
					destroyedEntity.EntityGuid = entityInChunkWithGuid3.EntityGuid;
					destroyedEntity.BeforeEntityInChunk = entityInChunkWithGuid3.EntityInChunk;
					this.DestroyedEntities.Add(in destroyedEntity);
					j++;
				}
				while (i < this.AfterEntities.Length)
				{
					EntityDiffer.EntityInChunkWithGuid entityInChunkWithGuid4 = this.AfterEntities[i];
					EntityDiffer.CreatedEntity createdEntity = default(EntityDiffer.CreatedEntity);
					createdEntity.EntityGuid = entityInChunkWithGuid4.EntityGuid;
					createdEntity.AfterEntityInChunk = entityInChunkWithGuid4.EntityInChunk;
					this.CreatedEntities.Add(in createdEntity);
					i++;
				}
			}

			// Token: 0x040001D6 RID: 470
			[ReadOnly]
			public NativeArray<EntityDiffer.EntityInChunkWithGuid> BeforeEntities;

			// Token: 0x040001D7 RID: 471
			[ReadOnly]
			public NativeArray<EntityDiffer.EntityInChunkWithGuid> AfterEntities;

			// Token: 0x040001D8 RID: 472
			[WriteOnly]
			public NativeList<EntityDiffer.CreatedEntity> CreatedEntities;

			// Token: 0x040001D9 RID: 473
			[WriteOnly]
			public NativeList<EntityDiffer.ModifiedEntity> ModifiedEntities;

			// Token: 0x040001DA RID: 474
			[WriteOnly]
			public NativeList<EntityDiffer.DestroyedEntity> DestroyedEntities;

			// Token: 0x040001DB RID: 475
			[WriteOnly]
			public NativeList<EntityDiffer.NameModifiedEntity> NameModifiedEntities;
		}

		// Token: 0x020000B3 RID: 179
		[BurstCompile]
		private struct BuildEntityGuidToEntity : IJob
		{
			// Token: 0x06000356 RID: 854 RVA: 0x00013BA4 File Offset: 0x00011DA4
			public void Execute()
			{
				if (this.SortedEntitiesWithGuid.Length == 0)
				{
					return;
				}
				EntityGuid entityGuid = this.SortedEntitiesWithGuid[0].EntityGuid;
				for (int i = 1; i < this.SortedEntitiesWithGuid.Length; i++)
				{
					EntityGuid entityGuid2 = this.SortedEntitiesWithGuid[i].EntityGuid;
					if ((in entityGuid2) == (in entityGuid))
					{
						int num = 1;
						while (++i < this.SortedEntitiesWithGuid.Length)
						{
							EntityGuid entityGuid3 = this.SortedEntitiesWithGuid[i].EntityGuid;
							if ((in entityGuid3) != (in entityGuid))
							{
								i--;
								break;
							}
							num++;
						}
						DuplicateEntityGuid duplicateEntityGuid = new DuplicateEntityGuid(entityGuid2, num);
						this.Duplicates.Add(in duplicateEntityGuid);
					}
					entityGuid = entityGuid2;
				}
			}

			// Token: 0x040001DC RID: 476
			[ReadOnly]
			public NativeArray<EntityDiffer.EntityInChunkWithGuid> SortedEntitiesWithGuid;

			// Token: 0x040001DD RID: 477
			[WriteOnly]
			public NativeList<DuplicateEntityGuid> Duplicates;
		}
	}
}
