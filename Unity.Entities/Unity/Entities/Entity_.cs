using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Unity.Entities
{
	// Token: 0x020002FE RID: 766
	internal struct Entity_
	{
		// Token: 0x060014CE RID: 5326 RVA: 0x0004F5AC File Offset: 0x0004D7AC
		public static Entity_[] ResolveArray(DebuggerDataAccess access, List<Entity> entities)
		{
			Entity_[] array = new Entity_[entities.Count];
			for (int num = 0; num != array.Length; num++)
			{
				array[num] = new Entity_(access, entities[num], false);
			}
			return array;
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060014CF RID: 5327 RVA: 0x0004F5EC File Offset: 0x0004D7EC
		public static Entity_ Null
		{
			get
			{
				Entity_ entity_;
				entity_._Entity = default(Entity);
				entity_._Access = new DebuggerDataAccess(null);
				entity_._IncludeWorldName = false;
				return entity_;
			}
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x0004F61D File Offset: 0x0004D81D
		public Entity_(World world, Entity entity, bool includeWorldName)
		{
			this = new Entity_(new DebuggerDataAccess(world), entity, includeWorldName);
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x0004F62D File Offset: 0x0004D82D
		public unsafe Entity_(EntityComponentStore* store, Entity entity, bool includeWorldName)
		{
			this = new Entity_(new DebuggerDataAccess(store), entity, includeWorldName);
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x0004F63D File Offset: 0x0004D83D
		public Entity_(DebuggerDataAccess access, Entity entity, bool includeWorldName)
		{
			this._Entity = entity;
			this._Access = access;
			this._IncludeWorldName = includeWorldName;
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x0004F654 File Offset: 0x0004D854
		public override string ToString()
		{
			if (this._IncludeWorldName)
			{
				return this._Access.GetDebugNameWithWorld(this._Entity);
			}
			return this._Access.GetDebugNameWithoutWorld(this._Entity);
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060014D4 RID: 5332 RVA: 0x0004F681 File Offset: 0x0004D881
		public EntityArchetype Archetype
		{
			get
			{
				return this._Access.GetArchetype(this._Entity);
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060014D5 RID: 5333 RVA: 0x0004F694 File Offset: 0x0004D894
		public ArchetypeChunk Chunk
		{
			get
			{
				return this._Access.GetChunk(this._Entity);
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x060014D6 RID: 5334 RVA: 0x0004F6A7 File Offset: 0x0004D8A7
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public object[] Components
		{
			get
			{
				return this._Access.GetComponents(this._Entity);
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x060014D7 RID: 5335 RVA: 0x0004F6BA File Offset: 0x0004D8BA
		public World World
		{
			get
			{
				return this._Access.GetWorld();
			}
		}

		// Token: 0x04000795 RID: 1941
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Entity _Entity;

		// Token: 0x04000796 RID: 1942
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private DebuggerDataAccess _Access;

		// Token: 0x04000797 RID: 1943
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool _IncludeWorldName;
	}
}
