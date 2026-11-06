using System;

namespace Unity.Entities
{
	// Token: 0x02000103 RID: 259
	internal struct EntityNode
	{
		// Token: 0x040002B4 RID: 692
		public unsafe Entity* Ptr;

		// Token: 0x040002B5 RID: 693
		public unsafe EntityNode* Prev;
	}
}
