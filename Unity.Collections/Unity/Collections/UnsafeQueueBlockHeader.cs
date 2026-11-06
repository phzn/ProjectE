using System;

namespace Unity.Collections
{
	// Token: 0x020000EA RID: 234
	internal struct UnsafeQueueBlockHeader
	{
		// Token: 0x0400045B RID: 1115
		public unsafe UnsafeQueueBlockHeader* m_NextBlock;

		// Token: 0x0400045C RID: 1116
		public int m_NumItems;
	}
}
