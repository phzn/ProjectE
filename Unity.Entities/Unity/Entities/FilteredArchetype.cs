using System;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x02000075 RID: 117
	public struct FilteredArchetype
	{
		// Token: 0x040000BD RID: 189
		public int EntityCount;

		// Token: 0x040000BE RID: 190
		public UnsafeList<int> PackedEntityIndices;

		// Token: 0x040000BF RID: 191
		public UnsafeList<TypeIndex> TypeIndices;
	}
}
