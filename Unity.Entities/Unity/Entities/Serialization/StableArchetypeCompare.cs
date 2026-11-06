using System;
using System.Collections.Generic;

namespace Unity.Entities.Serialization
{
	// Token: 0x0200038C RID: 908
	internal struct StableArchetypeCompare : IComparer<IntPtr>
	{
		// Token: 0x06001824 RID: 6180 RVA: 0x0005A4C4 File Offset: 0x000586C4
		public unsafe int Compare(IntPtr x, IntPtr y)
		{
			ulong stableHash = ((Archetype*)(void*)x)->StableHash;
			ulong stableHash2 = ((Archetype*)(void*)y)->StableHash;
			return stableHash.CompareTo(stableHash2);
		}
	}
}
