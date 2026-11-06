using System;
using System.Text;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace Unity.Entities
{
	// Token: 0x02000081 RID: 129
	[BurstCompile]
	internal static class EntityChangeSetFormatter
	{
		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x0000CDE0 File Offset: 0x0000AFE0
		private static EntityQueryDesc EntityGuidQueryDesc { get; } = new EntityQueryDesc
		{
			All = new ComponentType[] { typeof(EntityGuid) },
			Options = (EntityQueryOptions.IncludePrefab | EntityQueryOptions.IncludeDisabledEntities)
		};

		// Token: 0x060002D3 RID: 723 RVA: 0x0000CDE8 File Offset: 0x0000AFE8
		private unsafe static void GetComponentNameChanges(EntityManager entityManager, EntityChangeSet changeSet, EntityChangeSetFormatter.NameInfoSet nameInfoSet, AllocatorManager.AllocatorHandle allocator)
		{
			EntityQuery entityQuery = entityManager.CreateEntityQuery(new EntityQueryDesc[] { EntityChangeSetFormatter.EntityGuidQueryDesc });
			int num = entityQuery.CalculateEntityCount();
			NativeParallelMultiHashMap<EntityGuid, Entity> nativeParallelMultiHashMap = new NativeParallelMultiHashMap<EntityGuid, Entity>(num, allocator);
			JobHandle jobHandle = new EntityChangeSetFormatter.BuildEntityGuidToEntityHashMap
			{
				EntityTypeHandle = entityManager.GetEntityTypeHandle(),
				ComponentTypeHandle = entityManager.GetComponentTypeHandle<EntityGuid>(true),
				EntityGuidToEntity = nativeParallelMultiHashMap.AsParallelWriter()
			}.ScheduleParallel(entityQuery, default(JobHandle));
			int createdEntityCount = changeSet.CreatedEntityCount;
			int num2 = changeSet.Entities.Length - changeSet.CreatedEntityCount - changeSet.DestroyedEntityCount;
			NativeArray<EntityGuid> subArray = changeSet.Entities.GetSubArray(createdEntityCount, num2);
			NativeArray<FixedString64Bytes> subArray2 = nameInfoSet.Names.GetSubArray(createdEntityCount, num2);
			JobHandle jobHandle2 = new EntityChangeSetFormatter.GatherNamesForComponentChanges
			{
				EntityGuidToEntity = nativeParallelMultiHashMap,
				EntityComponentStore = entityManager.GetCheckedEntityDataAccess()->EntityComponentStore,
				Entities = subArray,
				Names = subArray2
			}.Schedule(num2, 64, jobHandle);
			JobHandle jobHandle3 = new EntityChangeSetFormatter.GatherNamesChangedEntities
			{
				EntityGuidToEntity = nativeParallelMultiHashMap,
				NameChangedEntityGuids = changeSet.NameChangedEntityGuids,
				NameChangedEntities = nameInfoSet.NameChangedEntities
			}.Schedule(changeSet.NameChangedCount, 64, jobHandle);
			jobHandle2.Complete();
			jobHandle3.Complete();
			entityQuery.Dispose();
			nativeParallelMultiHashMap.Dispose();
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000CF4C File Offset: 0x0000B14C
		private static EntityChangeSetFormatter.NameInfoSet BuildEntityNames(EntityManager targetEntityManager, EntityChangeSet changeSet, AllocatorManager.AllocatorHandle allocator)
		{
			int length = changeSet.Entities.Length;
			int nameChangedCount = changeSet.NameChangedCount;
			EntityChangeSetFormatter.NameInfoSet nameInfoSet = new EntityChangeSetFormatter.NameInfoSet(length, nameChangedCount, allocator);
			if (changeSet.CreatedEntityCount > 0)
			{
				NativeArray<FixedString64Bytes>.Copy(changeSet.Names, nameInfoSet.Names, changeSet.CreatedEntityCount);
			}
			EntityChangeSetFormatter.GetComponentNameChanges(targetEntityManager, changeSet, nameInfoSet, allocator);
			if (changeSet.DestroyedEntityCount > 0)
			{
				int num = changeSet.Names.Length - changeSet.DestroyedEntityCount;
				int num2 = changeSet.Entities.Length - changeSet.DestroyedEntityCount;
				NativeArray<FixedString64Bytes>.Copy(changeSet.Names, num, nameInfoSet.Names, num2, changeSet.DestroyedEntityCount);
			}
			return nameInfoSet;
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000CFF8 File Offset: 0x0000B1F8
		internal static string PrintSummary(this EntityChangeSet changeSet, EntityManager targetEntityManager)
		{
			StringBuilder stringBuilder = new StringBuilder();
			changeSet.PrintSummary(targetEntityManager, stringBuilder);
			return stringBuilder.ToString();
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000D01C File Offset: 0x0000B21C
		internal static void PrintSummary(this EntityChangeSet changeSet, EntityManager targetEntityManager, StringBuilder sb)
		{
			sb.AppendLine("Change Summary:");
			if (changeSet.CreatedEntityCount > 0)
			{
				sb.AppendLine("\tEntities created: " + changeSet.CreatedEntityCount.ToString());
			}
			if (changeSet.DestroyedEntityCount > 0)
			{
				sb.AppendLine("\tEntities destroyed: " + changeSet.DestroyedEntityCount.ToString());
			}
			if (changeSet.EntityReferenceChanges.Length > 0)
			{
				sb.AppendLine("\tEntity references changed: " + changeSet.EntityReferenceChanges.Length.ToString());
			}
			if (changeSet.AddComponents.Length > 0)
			{
				sb.AppendLine("\tComponents added: " + changeSet.AddComponents.Length.ToString());
			}
			if (changeSet.AddArchetypes.Length > 0)
			{
				sb.AppendLine("\tComponents added: " + changeSet.AddArchetypes.Length.ToString());
			}
			if (changeSet.RemoveComponents.Length > 0)
			{
				sb.AppendLine("\tComponents removed: " + changeSet.RemoveComponents.Length.ToString());
			}
			if (changeSet.SetComponents.Length > 0)
			{
				sb.AppendLine("\tUnmanaged components changed: " + changeSet.SetComponents.Length.ToString());
			}
			if (changeSet.SetManagedComponents.Length != 0)
			{
				sb.AppendLine("\tManaged components changed: " + changeSet.SetManagedComponents.Length.ToString());
			}
			if (changeSet.SetSharedComponents.Length != 0)
			{
				sb.AppendLine("\tShared components changed: " + changeSet.SetSharedComponents.Length.ToString());
			}
			if (changeSet.CreatedBlobAssets.Length > 0)
			{
				sb.AppendLine("\tBlob assets created: " + changeSet.CreatedBlobAssets.Length.ToString());
			}
			if (changeSet.DestroyedBlobAssets.Length > 0)
			{
				sb.AppendLine("\tBlob assets destroyed: " + changeSet.DestroyedBlobAssets.Length.ToString());
			}
			if (changeSet.BlobAssetReferenceChanges.Length > 0)
			{
				sb.AppendLine("\tBlob asset references changed: " + changeSet.BlobAssetReferenceChanges.Length.ToString());
			}
			if (changeSet.LinkedEntityGroupAdditions.Length > 0)
			{
				sb.AppendLine("\tLinked entity group additions: " + changeSet.LinkedEntityGroupAdditions.Length.ToString());
			}
			if (changeSet.LinkedEntityGroupRemovals.Length > 0)
			{
				sb.AppendLine("\tLinked entity group removals: " + changeSet.LinkedEntityGroupRemovals.Length.ToString());
			}
			sb.AppendLine();
			if (changeSet.CreatedEntityCount > 0)
			{
				sb.AppendLine("Entities created:");
				for (int i = 0; i < changeSet.CreatedEntityCount; i++)
				{
					sb.Append('\t');
					sb.Append(changeSet.Names[i].ToString());
					sb.Append(" - ");
					sb.AppendLine(changeSet.Entities[i].ToString());
				}
				sb.AppendLine();
			}
			if (changeSet.DestroyedEntityCount > 0)
			{
				sb.AppendLine("Entities destroyed:");
				int num = changeSet.Names.Length - changeSet.DestroyedEntityCount;
				int num2 = changeSet.Entities.Length - changeSet.DestroyedEntityCount;
				for (int j = 0; j < changeSet.DestroyedEntityCount; j++)
				{
					sb.Append('\t');
					sb.Append(changeSet.Names[num + j].ToString());
					sb.Append(" - ");
					sb.AppendLine(changeSet.Entities[num2 + j].ToString());
				}
				sb.AppendLine();
			}
			EntityChangeSetFormatter.NameInfoSet nameInfoSet = EntityChangeSetFormatter.BuildEntityNames(targetEntityManager, changeSet, Allocator.TempJob);
			if (changeSet.NameChangedCount > 0)
			{
				sb.AppendLine("Entities name changed:");
				int createdEntityCount = changeSet.CreatedEntityCount;
				for (int k = 0; k < changeSet.NameChangedCount; k++)
				{
					sb.Append('\t');
					sb.Append(changeSet.Names[createdEntityCount + k].ToString());
					sb.Append(" - ");
					sb.AppendLine(nameInfoSet.NameChangedEntities[k].ToString());
				}
				sb.AppendLine();
			}
			if (changeSet.AddComponents.Length > 0)
			{
				sb.AppendLine("Components added:");
				for (int l = 0; l < changeSet.AddComponents.Length; l++)
				{
					EntityChangeSetFormatter.FormatComponentChange(ref changeSet, changeSet.AddComponents[l], nameInfoSet, sb);
				}
				sb.AppendLine();
			}
			if (changeSet.RemoveComponents.Length > 0)
			{
				sb.AppendLine("Components removed:");
				for (int m = 0; m < changeSet.RemoveComponents.Length; m++)
				{
					EntityChangeSetFormatter.FormatComponentChange(ref changeSet, changeSet.RemoveComponents[m], nameInfoSet, sb);
				}
				sb.AppendLine();
			}
			if (changeSet.SetComponents.Length > 0)
			{
				sb.AppendLine("Unmanaged components changed:");
				for (int n = 0; n < changeSet.SetComponents.Length; n++)
				{
					EntityChangeSetFormatter.FormatComponentChange(ref changeSet, changeSet.SetComponents[n].Component, nameInfoSet, sb);
				}
				sb.AppendLine();
			}
			if (changeSet.SetManagedComponents.Length != 0)
			{
				sb.AppendLine("Managed components changed:");
				for (int num3 = 0; num3 < changeSet.SetManagedComponents.Length; num3++)
				{
					EntityChangeSetFormatter.FormatComponentChange(ref changeSet, changeSet.SetManagedComponents[num3].Component, nameInfoSet, sb);
				}
				sb.AppendLine();
			}
			if (changeSet.SetSharedComponents.Length != 0)
			{
				sb.AppendLine("Shared components changed:");
				for (int num4 = 0; num4 < changeSet.SetSharedComponents.Length; num4++)
				{
					EntityChangeSetFormatter.FormatComponentChange(ref changeSet, changeSet.SetSharedComponents[num4].Component, nameInfoSet, sb);
				}
				sb.AppendLine();
			}
			if (changeSet.EntityReferenceChanges.Length > 0)
			{
				sb.AppendLine("Entity references changed:");
				for (int num5 = 0; num5 < changeSet.EntityReferenceChanges.Length; num5++)
				{
					EntityChangeSetFormatter.FormatComponentChange(ref changeSet, changeSet.EntityReferenceChanges[num5].Component, nameInfoSet, sb);
				}
				sb.AppendLine();
			}
			if (changeSet.BlobAssetReferenceChanges.Length > 0)
			{
				sb.AppendLine("Blob asset references changed:");
				for (int num6 = 0; num6 < changeSet.BlobAssetReferenceChanges.Length; num6++)
				{
					EntityChangeSetFormatter.FormatComponentChange(ref changeSet, changeSet.BlobAssetReferenceChanges[num6].Component, nameInfoSet, sb);
				}
				sb.AppendLine();
			}
			if (changeSet.LinkedEntityGroupAdditions.Length > 0)
			{
				sb.AppendLine("Linked entity group additions:");
				for (int num7 = 0; num7 < changeSet.LinkedEntityGroupAdditions.Length; num7++)
				{
					sb.Append('\t');
					LinkedEntityGroupChange linkedEntityGroupChange = changeSet.LinkedEntityGroupAdditions[num7];
					sb.Append(linkedEntityGroupChange.ChildEntityGuid.ToString());
					sb.Append(" added to ");
					linkedEntityGroupChange = changeSet.LinkedEntityGroupAdditions[num7];
					sb.Append(linkedEntityGroupChange.RootEntityGuid.ToString());
					sb.AppendLine();
				}
				sb.AppendLine();
			}
			if (changeSet.LinkedEntityGroupRemovals.Length > 0)
			{
				sb.AppendLine("Linked entity group removals:");
				for (int num8 = 0; num8 < changeSet.LinkedEntityGroupRemovals.Length; num8++)
				{
					sb.Append('\t');
					LinkedEntityGroupChange linkedEntityGroupChange = changeSet.LinkedEntityGroupRemovals[num8];
					sb.Append(linkedEntityGroupChange.ChildEntityGuid.ToString());
					sb.Append(" removed from ");
					linkedEntityGroupChange = changeSet.LinkedEntityGroupRemovals[num8];
					sb.Append(linkedEntityGroupChange.RootEntityGuid.ToString());
					sb.AppendLine();
				}
				sb.AppendLine();
			}
			if (nameInfoSet.IsCreated)
			{
				nameInfoSet.Dispose();
			}
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000D928 File Offset: 0x0000BB28
		private static void FormatComponentChange(ref EntityChangeSet changeSet, PackedComponent c, EntityChangeSetFormatter.NameInfoSet nameInfoSet, StringBuilder sb)
		{
			NativeText.ReadOnly debugTypeName = TypeManager.GetTypeInfo(TypeManager.GetTypeIndexFromStableTypeHash(changeSet.TypeHashes[c.PackedTypeIndex].StableTypeHash)).DebugTypeName;
			sb.Append("\t");
			sb.Append(debugTypeName);
			sb.Append(" - ");
			sb.AppendLine(nameInfoSet.Names[c.PackedEntityIndex].ToString());
		}

		// Token: 0x02000082 RID: 130
		private struct NameInfoSet
		{
			// Token: 0x060002D9 RID: 729 RVA: 0x0000D9EE File Offset: 0x0000BBEE
			internal NameInfoSet(int namesLength, int nameChangedEntitiesLength, AllocatorManager.AllocatorHandle allocator)
			{
				this.Names = new NativeArray<FixedString64Bytes>(namesLength, allocator.ToAllocator, NativeArrayOptions.ClearMemory);
				this.NameChangedEntities = new NativeArray<Entity>(nameChangedEntitiesLength, allocator.ToAllocator, NativeArrayOptions.ClearMemory);
				this.IsCreated = true;
			}

			// Token: 0x060002DA RID: 730 RVA: 0x0000DA1F File Offset: 0x0000BC1F
			internal void Dispose()
			{
				this.Names.Dispose();
				this.NameChangedEntities.Dispose();
				this.IsCreated = false;
			}

			// Token: 0x040000F4 RID: 244
			internal NativeArray<FixedString64Bytes> Names;

			// Token: 0x040000F5 RID: 245
			internal NativeArray<Entity> NameChangedEntities;

			// Token: 0x040000F6 RID: 246
			internal bool IsCreated;
		}

		// Token: 0x02000083 RID: 131
		[BurstCompile]
		private struct BuildEntityGuidToEntityHashMap : IJobChunk
		{
			// Token: 0x060002DB RID: 731 RVA: 0x0000DA40 File Offset: 0x0000BC40
			[BurstCompile]
			public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				NativeArray<EntityGuid> nativeArray = chunk.GetNativeArray<EntityGuid>(ref this.ComponentTypeHandle);
				NativeArray<Entity> nativeArray2 = chunk.GetNativeArray(this.EntityTypeHandle);
				for (int num = 0; num != nativeArray2.Length; num++)
				{
					this.EntityGuidToEntity.Add(nativeArray[num], nativeArray2[num]);
				}
			}

			// Token: 0x060002DC RID: 732 RVA: 0x0000DA94 File Offset: 0x0000BC94
			void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
			}

			// Token: 0x040000F7 RID: 247
			[ReadOnly]
			public ComponentTypeHandle<EntityGuid> ComponentTypeHandle;

			// Token: 0x040000F8 RID: 248
			[ReadOnly]
			public EntityTypeHandle EntityTypeHandle;

			// Token: 0x040000F9 RID: 249
			[WriteOnly]
			public NativeParallelMultiHashMap<EntityGuid, Entity>.ParallelWriter EntityGuidToEntity;
		}

		// Token: 0x02000084 RID: 132
		[BurstCompile]
		private struct GatherNamesForComponentChanges : IJobParallelFor
		{
			// Token: 0x060002DD RID: 733 RVA: 0x0000DAA4 File Offset: 0x0000BCA4
			[BurstCompile]
			public unsafe void Execute(int index)
			{
				EntityGuid entityGuid = this.Entities[index];
				Entity entity;
				NativeParallelMultiHashMapIterator<EntityGuid> nativeParallelMultiHashMapIterator;
				this.EntityGuidToEntity.TryGetFirstValue(entityGuid, out entity, out nativeParallelMultiHashMapIterator);
				FixedString64Bytes fixedString64Bytes;
				this.EntityComponentStore->GetName(entity, out fixedString64Bytes);
				this.Names[index] = fixedString64Bytes;
			}

			// Token: 0x040000FA RID: 250
			[ReadOnly]
			public NativeParallelMultiHashMap<EntityGuid, Entity> EntityGuidToEntity;

			// Token: 0x040000FB RID: 251
			[ReadOnly]
			public NativeArray<EntityGuid> Entities;

			// Token: 0x040000FC RID: 252
			[NativeDisableUnsafePtrRestriction]
			public unsafe EntityComponentStore* EntityComponentStore;

			// Token: 0x040000FD RID: 253
			[WriteOnly]
			public NativeArray<FixedString64Bytes> Names;
		}

		// Token: 0x02000085 RID: 133
		[BurstCompile]
		private struct GatherNamesChangedEntities : IJobParallelFor
		{
			// Token: 0x060002DE RID: 734 RVA: 0x0000DAEC File Offset: 0x0000BCEC
			[BurstCompile]
			public void Execute(int index)
			{
				EntityGuid entityGuid = this.NameChangedEntityGuids[index];
				Entity entity;
				NativeParallelMultiHashMapIterator<EntityGuid> nativeParallelMultiHashMapIterator;
				this.EntityGuidToEntity.TryGetFirstValue(entityGuid, out entity, out nativeParallelMultiHashMapIterator);
				this.NameChangedEntities[index] = entity;
			}

			// Token: 0x040000FE RID: 254
			[ReadOnly]
			public NativeParallelMultiHashMap<EntityGuid, Entity> EntityGuidToEntity;

			// Token: 0x040000FF RID: 255
			[ReadOnly]
			public NativeArray<EntityGuid> NameChangedEntityGuids;

			// Token: 0x04000100 RID: 256
			[WriteOnly]
			public NativeArray<Entity> NameChangedEntities;
		}
	}
}
