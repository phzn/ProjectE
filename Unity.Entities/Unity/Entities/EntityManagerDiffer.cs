using System;
using Unity.Collections;

namespace Unity.Entities
{
	// Token: 0x020000B5 RID: 181
	public struct EntityManagerDiffer : IDisposable
	{
		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000358 RID: 856 RVA: 0x00013C6D File Offset: 0x00011E6D
		internal static EntityQueryDesc EntityGuidQueryDesc { get; } = new EntityQueryDesc
		{
			All = new ComponentType[] { typeof(EntityGuid) },
			Options = (EntityQueryOptions.IncludePrefab | EntityQueryOptions.IncludeDisabledEntities)
		};

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000359 RID: 857 RVA: 0x00013C74 File Offset: 0x00011E74
		internal EntityManager ShadowEntityManager
		{
			get
			{
				return this.m_ShadowEntityManager;
			}
		}

		// Token: 0x0600035A RID: 858 RVA: 0x00013C7C File Offset: 0x00011E7C
		public EntityManagerDiffer(EntityManager sourceEntityManager, AllocatorManager.AllocatorHandle allocator, EntityQueryDesc entityQueryDesc = null)
		{
			this.m_CachedComponentChanges = new EntityDiffer.CachedComponentChanges(1024);
			this.m_SourceEntityManager = sourceEntityManager;
			this.m_EntityQueryDesc = entityQueryDesc ?? EntityManagerDiffer.EntityGuidQueryDesc;
			this.m_ShadowWorld = new World(sourceEntityManager.World.Name + " (Shadow)", sourceEntityManager.World.Flags | WorldFlags.Shadow);
			this.m_ShadowEntityManager = this.m_ShadowWorld.EntityManager;
			this.m_BlobAssetCache = new BlobAssetCache(allocator);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x00013D00 File Offset: 0x00011F00
		public void Dispose()
		{
			this.m_CachedComponentChanges.Dispose();
			this.m_SourceEntityManager = default(EntityManager);
			if (this.m_ShadowWorld != null && this.m_ShadowWorld.IsCreated)
			{
				this.m_ShadowWorld.Dispose();
			}
			this.m_BlobAssetCache.Dispose();
			this.m_ShadowWorld = null;
			this.m_ShadowEntityManager = default(EntityManager);
			this.m_EntityQueryDesc = null;
		}

		// Token: 0x0600035C RID: 860 RVA: 0x00013D69 File Offset: 0x00011F69
		public EntityChanges GetChanges(EntityManagerDifferOptions options, AllocatorManager.AllocatorHandle allocator)
		{
			return EntityDiffer.GetChanges(ref this.m_CachedComponentChanges, this.m_SourceEntityManager, this.m_ShadowEntityManager, options, this.m_EntityQueryDesc, this.m_BlobAssetCache, allocator);
		}

		// Token: 0x040001E1 RID: 481
		private World m_ShadowWorld;

		// Token: 0x040001E2 RID: 482
		private EntityDiffer.CachedComponentChanges m_CachedComponentChanges;

		// Token: 0x040001E3 RID: 483
		private EntityManager m_SourceEntityManager;

		// Token: 0x040001E4 RID: 484
		private EntityManager m_ShadowEntityManager;

		// Token: 0x040001E5 RID: 485
		private EntityQueryDesc m_EntityQueryDesc;

		// Token: 0x040001E6 RID: 486
		private BlobAssetCache m_BlobAssetCache;
	}
}
