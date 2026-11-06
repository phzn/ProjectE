using System;
using System.Runtime.InteropServices;
using Unity.Collections;

namespace Unity.Entities
{
	// Token: 0x02000105 RID: 261
	[StructLayout(LayoutKind.Sequential, Size = 64)]
	internal struct EntityCommandBufferChain
	{
		// Token: 0x0600040C RID: 1036 RVA: 0x00017228 File Offset: 0x00015428
		internal unsafe static void InitChain(EntityCommandBufferChain* chain, AllocatorManager.AllocatorHandle allocator)
		{
			chain->m_Cleanup = (ChainCleanup*)Memory.Unmanaged.Allocate((long)sizeof(ChainCleanup), sizeof(ChainCleanup), allocator);
			chain->m_Cleanup->CleanupList = null;
			chain->m_Cleanup->BufferCleanupList = null;
			chain->m_Cleanup->EntityArraysCleanupList = null;
			chain->m_Tail = null;
			chain->m_Head = null;
			chain->m_PrevCreateCommand = null;
			chain->m_PrevEntityCommand = null;
			chain->m_LastSortKey = -1;
			chain->m_NextChain = null;
			chain->m_CanBurstPlayback = true;
		}

		// Token: 0x040002B9 RID: 697
		public unsafe ECBChunk* m_Tail;

		// Token: 0x040002BA RID: 698
		public unsafe ECBChunk* m_Head;

		// Token: 0x040002BB RID: 699
		public unsafe ChainCleanup* m_Cleanup;

		// Token: 0x040002BC RID: 700
		public unsafe CreateCommand* m_PrevCreateCommand;

		// Token: 0x040002BD RID: 701
		public unsafe EntityCommand* m_PrevEntityCommand;

		// Token: 0x040002BE RID: 702
		public unsafe EntityCommandBufferChain* m_NextChain;

		// Token: 0x040002BF RID: 703
		public int m_LastSortKey;

		// Token: 0x040002C0 RID: 704
		public bool m_CanBurstPlayback;
	}
}
