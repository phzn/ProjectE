using System;
using Unity.Collections;
using UnityEngine.Scripting;

namespace Unity.Entities
{
	// Token: 0x020000D8 RID: 216
	[DisableAutoCreation]
	internal class EntityPatcherBlobAssetSystem : SystemBase
	{
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060003CE RID: 974 RVA: 0x00016BD8 File Offset: 0x00014DD8
		private static EntityQueryDesc EntityGuidQueryDesc
		{
			get
			{
				EntityQueryDesc entityQueryDesc;
				if ((entityQueryDesc = EntityPatcherBlobAssetSystem.s_EntityGuidQueryDesc) == null)
				{
					entityQueryDesc = (EntityPatcherBlobAssetSystem.s_EntityGuidQueryDesc = new EntityQueryDesc
					{
						All = new ComponentType[] { typeof(EntityGuid) },
						Options = (EntityQueryOptions.IncludePrefab | EntityQueryOptions.IncludeDisabledEntities)
					});
				}
				return entityQueryDesc;
			}
		}

		// Token: 0x060003CF RID: 975 RVA: 0x00016C24 File Offset: 0x00014E24
		[Preserve]
		protected override void OnCreate()
		{
			this.m_BlobAssetBatchPtr = DynamicBlobAssetBatch.Allocate(Allocator.Persistent);
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x00016C37 File Offset: 0x00014E37
		[Preserve]
		protected override void OnDestroy()
		{
			DynamicBlobAssetBatch.Free(this.m_BlobAssetBatchPtr);
			this.m_BlobAssetBatchPtr = null;
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x00016C4C File Offset: 0x00014E4C
		public unsafe void SetFramesToRetainBlobAssets(int framesToRetainBlobAssets)
		{
			this.m_BlobAssetBatchPtr->SetFramesToRetainBlobAssets(framesToRetainBlobAssets);
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00016C5A File Offset: 0x00014E5A
		public unsafe void AllocateBlobAsset(void* ptr, int len, ulong hash)
		{
			this.m_BlobAssetBatchPtr->AllocateBlobAsset(ptr, len, hash);
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x00016C6B File Offset: 0x00014E6B
		public unsafe void ReleaseBlobAsset(EntityManager entityManager, ulong hash)
		{
			this.m_BlobAssetBatchPtr->ReleaseBlobAsset(entityManager, hash);
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x00016C7A File Offset: 0x00014E7A
		public unsafe bool TryGetBlobAsset(ulong hash, out BlobAssetPtr ptr)
		{
			return this.m_BlobAssetBatchPtr->TryGetBlobAsset(hash, out ptr);
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x00016C8C File Offset: 0x00014E8C
		public unsafe void ReleaseUnusedBlobAssets()
		{
			using (NativeArray<ArchetypeChunk> nativeArray = base.EntityManager.CreateEntityQuery(new EntityQueryDesc[] { EntityPatcherBlobAssetSystem.EntityGuidQueryDesc }).ToArchetypeChunkArray(Allocator.TempJob))
			{
				using (EntityDiffer.BlobAssetsWithDistinctHash blobAssetsWithDistinctHash = EntityDiffer.GetBlobAssetsWithDistinctHash(base.EntityManager.GetCheckedEntityDataAccess()->EntityComponentStore, base.EntityManager.GetCheckedEntityDataAccess()->ManagedComponentStore, nativeArray, Allocator.TempJob))
				{
					this.m_BlobAssetBatchPtr->RemoveUnusedBlobAssets(blobAssetsWithDistinctHash.BlobAssetsMap);
				}
			}
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x00002287 File Offset: 0x00000487
		[Preserve]
		protected override void OnUpdate()
		{
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x00016D44 File Offset: 0x00014F44
		[Preserve]
		public EntityPatcherBlobAssetSystem()
		{
		}

		// Token: 0x04000251 RID: 593
		private unsafe DynamicBlobAssetBatch* m_BlobAssetBatchPtr;

		// Token: 0x04000252 RID: 594
		private static EntityQueryDesc s_EntityGuidQueryDesc;
	}
}
