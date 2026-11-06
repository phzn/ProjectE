using System;

namespace Unity.Entities
{
	// Token: 0x0200004B RID: 75
	internal sealed class ArchetypeChunkDebugView
	{
		// Token: 0x0600021A RID: 538 RVA: 0x0000A928 File Offset: 0x00008B28
		public ArchetypeChunkDebugView(ArchetypeChunk ArchetypeChunk)
		{
			this.m_ArchetypeChunk = ArchetypeChunk;
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600021B RID: 539 RVA: 0x0000A937 File Offset: 0x00008B37
		public EntityArchetype Archetype
		{
			get
			{
				return new EntityArchetype(this.m_ArchetypeChunk.Archetype.Archetype);
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600021C RID: 540 RVA: 0x0000A950 File Offset: 0x00008B50
		public unsafe Entity_[] Entities
		{
			get
			{
				ChunkIndex chunk = this.m_ArchetypeChunk.m_Chunk;
				if (chunk == ChunkIndex.Null)
				{
					return null;
				}
				int count = this.m_ArchetypeChunk.Count;
				Entity_[] array = new Entity_[count];
				Entity* chunkComponentDataROPtr = (Entity*)ChunkIterationUtility.GetChunkComponentDataROPtr(this.m_ArchetypeChunk.Archetype.Archetype, chunk, 0);
				for (int i = 0; i < count; i++)
				{
					array[i] = new Entity_(this.m_ArchetypeChunk.m_EntityComponentStore, chunkComponentDataROPtr[i], false);
				}
				return array;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600021D RID: 541 RVA: 0x0000A9E0 File Offset: 0x00008BE0
		public unsafe object[] SharedComponents
		{
			get
			{
				ChunkIndex chunk = this.m_ArchetypeChunk.m_Chunk;
				if (chunk == ChunkIndex.Null)
				{
					return Array.Empty<object>();
				}
				Archetype* archetype = this.m_ArchetypeChunk.Archetype.Archetype;
				object[] array = new object[archetype->NumSharedComponents];
				int typesCount = archetype->TypesCount;
				int num = 0;
				for (int i = 0; i < typesCount; i++)
				{
					ComponentTypeInArchetype componentTypeInArchetype = archetype->Types[i];
					if (componentTypeInArchetype.IsSharedComponent)
					{
						SharedComponentValues sharedComponentValues = archetype->Chunks.GetSharedComponentValues(chunk.ListIndex);
						array[num] = new DebuggerDataAccess(this.m_ArchetypeChunk.m_EntityComponentStore).GetSharedComponentDataBoxed(*sharedComponentValues[num], componentTypeInArchetype.TypeIndex);
						num++;
					}
				}
				return array;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x0600021E RID: 542 RVA: 0x0000AAAD File Offset: 0x00008CAD
		public Entity_ ChunkComponent
		{
			get
			{
				if (this.m_ArchetypeChunk.m_Chunk == ChunkIndex.Null)
				{
					return Entity_.Null;
				}
				return new Entity_(this.m_ArchetypeChunk.m_EntityComponentStore, this.m_ArchetypeChunk.m_Chunk.MetaChunkEntity, false);
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x0600021F RID: 543 RVA: 0x0000AAF0 File Offset: 0x00008CF0
		public unsafe ComponentType_[] ComponentTypes
		{
			get
			{
				ChunkIndex chunk = this.m_ArchetypeChunk.m_Chunk;
				if (chunk == ChunkIndex.Null)
				{
					return null;
				}
				Archetype* archetype = this.m_ArchetypeChunk.Archetype.Archetype;
				ComponentType_[] array = new ComponentType_[archetype->TypesCount];
				for (int i = 0; i < archetype->TypesCount; i++)
				{
					int num = archetype->TypeIndexInArchetypeToMemoryOrderIndex[i];
					ComponentType componentType = archetype->Types[i].ToComponentType();
					array[i].Type = componentType;
					array[i].Version = archetype->Chunks.GetChangeVersion(i, chunk.ListIndex);
					array[i].IsEnableable = TypeManager.IsEnableable(componentType.TypeIndex);
					array[i].NumDisabledEntitiesInChunk = archetype->Chunks.GetChunkDisabledCountForType(num, chunk.ListIndex);
				}
				return array;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000220 RID: 544 RVA: 0x0000ABD8 File Offset: 0x00008DD8
		public unsafe uint OrderVersion
		{
			get
			{
				ChunkIndex chunk = this.m_ArchetypeChunk.m_Chunk;
				if (chunk == ChunkIndex.Null)
				{
					return 0U;
				}
				return this.m_ArchetypeChunk.Archetype.Archetype->Chunks.GetOrderVersion(chunk.ListIndex);
			}
		}

		// Token: 0x0400008D RID: 141
		private ArchetypeChunk m_ArchetypeChunk;
	}
}
