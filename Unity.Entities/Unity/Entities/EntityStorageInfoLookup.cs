using System;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x02000232 RID: 562
	[NativeContainer]
	[NativeContainerIsReadOnly]
	public struct EntityStorageInfoLookup
	{
		// Token: 0x06000DDC RID: 3548 RVA: 0x000429AC File Offset: 0x00040BAC
		internal unsafe EntityStorageInfoLookup(EntityDataAccess* entityDataAccess)
		{
			this.m_EntityDataAccess = entityDataAccess;
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x000429B5 File Offset: 0x00040BB5
		public unsafe void Update(SystemBase system)
		{
			this.Update(ref *system.m_StatePtr);
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x00002287 File Offset: 0x00000487
		public void Update(ref SystemState systemState)
		{
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x000429C3 File Offset: 0x00040BC3
		public unsafe bool Exists(Entity entity)
		{
			return this.m_EntityDataAccess->EntityComponentStore->Exists(entity);
		}

		// Token: 0x17000103 RID: 259
		public unsafe EntityStorageInfo this[Entity entity]
		{
			get
			{
				EntityInChunk entityInChunk = this.m_EntityDataAccess->EntityComponentStore->GetEntityInChunk(entity);
				return new EntityStorageInfo
				{
					Chunk = new ArchetypeChunk(entityInChunk.Chunk, this.m_EntityDataAccess->EntityComponentStore),
					IndexInChunk = entityInChunk.IndexInChunk
				};
			}
		}

		// Token: 0x040005E9 RID: 1513
		[NativeDisableUnsafePtrRestriction]
		private unsafe readonly EntityDataAccess* m_EntityDataAccess;
	}
}
