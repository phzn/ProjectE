using System;

namespace Unity.Entities
{
	// Token: 0x02000102 RID: 258
	internal struct BufferHeaderNode
	{
		// Token: 0x040002B2 RID: 690
		public unsafe BufferHeaderNode* Prev;

		// Token: 0x040002B3 RID: 691
		public BufferHeader TempBuffer;
	}
}
