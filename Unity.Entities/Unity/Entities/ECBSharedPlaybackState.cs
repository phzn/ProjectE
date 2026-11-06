using System;

namespace Unity.Entities
{
	// Token: 0x02000106 RID: 262
	internal struct ECBSharedPlaybackState
	{
		// Token: 0x040002C1 RID: 705
		public unsafe Entity* CreateEntityBatch;

		// Token: 0x040002C2 RID: 706
		public unsafe ECBSharedPlaybackState.BufferWithFixUp* BuffersWithFixUp;

		// Token: 0x040002C3 RID: 707
		public int CreatedEntityCount;

		// Token: 0x040002C4 RID: 708
		public int LastBuffer;

		// Token: 0x040002C5 RID: 709
		public int CommandBufferID;

		// Token: 0x02000107 RID: 263
		public struct BufferWithFixUp
		{
			// Token: 0x040002C6 RID: 710
			public unsafe EntityBufferCommand* cmd;
		}
	}
}
