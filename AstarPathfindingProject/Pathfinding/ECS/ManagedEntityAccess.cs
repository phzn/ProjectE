using System;
using System.Runtime.CompilerServices;
using Unity.Entities;

namespace Pathfinding.ECS
{
	// Token: 0x02000312 RID: 786
	public struct ManagedEntityAccess<T> where T : class, IComponentData
	{
		// Token: 0x06001329 RID: 4905 RVA: 0x00073D78 File Offset: 0x00071F78
		public ManagedEntityAccess(bool readOnly)
		{
			this.entityManager = default(EntityManager);
			this.handle = default(ComponentTypeHandle<T>);
			this.readOnly = readOnly;
		}

		// Token: 0x0600132A RID: 4906 RVA: 0x00073D99 File Offset: 0x00071F99
		public ManagedEntityAccess(EntityManager entityManager, bool readOnly)
		{
			this = new ManagedEntityAccess<T>(readOnly);
			this.Update(entityManager);
		}

		// Token: 0x0600132B RID: 4907 RVA: 0x00073DA9 File Offset: 0x00071FA9
		public void Update(EntityManager entityManager)
		{
			if (this.readOnly)
			{
				entityManager.CompleteDependencyBeforeRO<T>();
			}
			else
			{
				entityManager.CompleteDependencyBeforeRW<T>();
			}
			this.handle = entityManager.GetComponentTypeHandle<T>(this.readOnly);
			this.entityManager = entityManager;
		}

		// Token: 0x1700031B RID: 795
		public T this[EntityStorageInfo storage]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return storage.Chunk.GetManagedComponentAccessor<T>(ref this.handle, this.entityManager)[storage.IndexInChunk];
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				storage.Chunk.GetManagedComponentAccessor<T>(ref this.handle, this.entityManager)[storage.IndexInChunk] = value;
			}
		}

		// Token: 0x04000E0F RID: 3599
		private EntityManager entityManager;

		// Token: 0x04000E10 RID: 3600
		private ComponentTypeHandle<T> handle;

		// Token: 0x04000E11 RID: 3601
		private bool readOnly;
	}
}
