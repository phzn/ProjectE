using System;
using System.Runtime.InteropServices;

namespace Unity.Entities
{
	// Token: 0x02000104 RID: 260
	[StructLayout(LayoutKind.Sequential, Size = 32)]
	internal struct ChainCleanup
	{
		// Token: 0x040002B6 RID: 694
		public unsafe EntityNode* EntityArraysCleanupList;

		// Token: 0x040002B7 RID: 695
		public unsafe BufferHeaderNode* BufferCleanupList;

		// Token: 0x040002B8 RID: 696
		public unsafe EntityComponentGCNode* CleanupList;
	}
}
