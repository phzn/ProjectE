using System;
using System.Runtime.InteropServices;

namespace Unity.Entities
{
	// Token: 0x02000101 RID: 257
	internal struct EntityComponentGCNode
	{
		// Token: 0x040002B0 RID: 688
		public GCHandle BoxedObject;

		// Token: 0x040002B1 RID: 689
		public unsafe EntityComponentGCNode* Prev;
	}
}
