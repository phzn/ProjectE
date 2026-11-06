using System;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x02000213 RID: 531
	public struct EntityQueryMask
	{
		// Token: 0x06000C6E RID: 3182 RVA: 0x00038DA8 File Offset: 0x00036FA8
		internal unsafe EntityQueryMask(byte index, byte mask, EntityComponentStore* entityComponentStore)
		{
			this.Index = index;
			this.Mask = mask;
			this.EntityComponentStore = entityComponentStore;
		}

		// Token: 0x06000C6F RID: 3183 RVA: 0x00038DBF File Offset: 0x00036FBF
		internal bool IsCreated()
		{
			return this.EntityComponentStore != null;
		}

		// Token: 0x06000C70 RID: 3184 RVA: 0x00038DCE File Offset: 0x00036FCE
		public unsafe bool MatchesIgnoreFilter(Entity entity)
		{
			return this.EntityComponentStore->Exists(entity) && this.EntityComponentStore->GetArchetype(entity)->CompareMask(this);
		}

		// Token: 0x06000C71 RID: 3185 RVA: 0x00038DF7 File Offset: 0x00036FF7
		[Obsolete("This method has been renamed to MatchesIgnoreFilter, for clarity. It will return false positives if the query uses chunk filtering or enableable components, and should not be used. (RemovedAfter Entities 1.0)", true)]
		public bool Matches(Entity entity)
		{
			return this.MatchesIgnoreFilter(entity);
		}

		// Token: 0x06000C72 RID: 3186 RVA: 0x00038E00 File Offset: 0x00037000
		public unsafe bool MatchesIgnoreFilter(ArchetypeChunk chunk)
		{
			return chunk.Archetype.Archetype->CompareMask(this);
		}

		// Token: 0x06000C73 RID: 3187 RVA: 0x00038E19 File Offset: 0x00037019
		[Obsolete("This method has been renamed to MatchesIgnoreFilter, for clarity. It will return false positives if the query uses chunk filtering, and should not be used. (RemovedAfter Entities 1.0)", true)]
		public bool Matches(ArchetypeChunk chunk)
		{
			return this.MatchesIgnoreFilter(chunk);
		}

		// Token: 0x06000C74 RID: 3188 RVA: 0x00038E22 File Offset: 0x00037022
		public unsafe bool Matches(EntityArchetype archetype)
		{
			return archetype.Archetype->CompareMask(this);
		}

		// Token: 0x06000C75 RID: 3189 RVA: 0x00038E35 File Offset: 0x00037035
		internal unsafe bool Matches(Archetype* archetype)
		{
			return archetype->CompareMask(this);
		}

		// Token: 0x04000578 RID: 1400
		internal byte Index;

		// Token: 0x04000579 RID: 1401
		internal byte Mask;

		// Token: 0x0400057A RID: 1402
		[NativeDisableUnsafePtrRestriction]
		internal unsafe readonly EntityComponentStore* EntityComponentStore;
	}
}
