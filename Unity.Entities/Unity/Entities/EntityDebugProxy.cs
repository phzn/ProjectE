using System;
using System.Diagnostics;

namespace Unity.Entities
{
	// Token: 0x020002FD RID: 765
	internal class EntityDebugProxy
	{
		// Token: 0x060014C7 RID: 5319 RVA: 0x0004F408 File Offset: 0x0004D608
		public EntityDebugProxy(Entity entity)
		{
			this._Entity = entity;
			this._World = EntityDebugProxy.GetWorld(entity);
		}

		// Token: 0x060014C8 RID: 5320 RVA: 0x0004F424 File Offset: 0x0004D624
		public static string GetDebugName(int index, int version)
		{
			Entity entity = new Entity
			{
				Index = index,
				Version = version
			};
			return new DebuggerDataAccess(EntityDebugProxy.GetWorld(entity)).GetDebugNameWithWorld(entity);
		}

		// Token: 0x060014C9 RID: 5321 RVA: 0x0004F460 File Offset: 0x0004D660
		public unsafe static World GetWorld(Entity entity)
		{
			if (!EntityComponentStore.s_entityStore.Data.Exists(entity))
			{
				return null;
			}
			EntityInChunk entityInChunk = EntityComponentStore.s_entityStore.Data.GetEntityInChunk(entity);
			Archetype* archetype = EntityComponentStore.PerChunkArray.ChunkData[entityInChunk.Chunk].Archetype;
			if (archetype == null)
			{
				return null;
			}
			EntityComponentStore* entityComponentStore = archetype->EntityComponentStore;
			if (entityComponentStore == null)
			{
				return null;
			}
			ulong worldSequenceNumber = entityComponentStore->WorldSequenceNumber;
			foreach (World world in World.All)
			{
				if (worldSequenceNumber == world.SequenceNumber)
				{
					return world;
				}
			}
			return null;
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060014CA RID: 5322 RVA: 0x0004F52C File Offset: 0x0004D72C
		public EntityArchetype Archetype
		{
			get
			{
				return new DebuggerDataAccess(this._World).GetArchetype(this._Entity);
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060014CB RID: 5323 RVA: 0x0004F554 File Offset: 0x0004D754
		public ArchetypeChunk Chunk
		{
			get
			{
				return new DebuggerDataAccess(this._World).GetChunk(this._Entity);
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060014CC RID: 5324 RVA: 0x0004F57C File Offset: 0x0004D77C
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public object[] Components
		{
			get
			{
				return new DebuggerDataAccess(this._World).GetComponents(this._Entity);
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060014CD RID: 5325 RVA: 0x0004F5A2 File Offset: 0x0004D7A2
		public World World
		{
			get
			{
				return this._World;
			}
		}

		// Token: 0x04000793 RID: 1939
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Entity _Entity;

		// Token: 0x04000794 RID: 1940
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private World _World;
	}
}
