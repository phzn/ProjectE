using System;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x02000166 RID: 358
	internal struct LookupCache
	{
		// Token: 0x060007C9 RID: 1993 RVA: 0x0002A650 File Offset: 0x00028850
		public unsafe void Update(Archetype* archetype, TypeIndex typeIndex)
		{
			ChunkDataUtility.GetIndexInTypeArray(archetype, typeIndex, ref this.IndexInArchetype);
			this.ComponentOffset = ((this.IndexInArchetype == -1) ? 0 : archetype->Offsets[this.IndexInArchetype]);
			this.ComponentSizeOf = ((this.IndexInArchetype == -1) ? 0 : archetype->SizeOfs[this.IndexInArchetype]);
			this.Archetype = archetype;
		}

		// Token: 0x04000449 RID: 1097
		[NativeDisableUnsafePtrRestriction]
		public unsafe Archetype* Archetype;

		// Token: 0x0400044A RID: 1098
		public int ComponentOffset;

		// Token: 0x0400044B RID: 1099
		public ushort ComponentSizeOf;

		// Token: 0x0400044C RID: 1100
		public short IndexInArchetype;
	}
}
