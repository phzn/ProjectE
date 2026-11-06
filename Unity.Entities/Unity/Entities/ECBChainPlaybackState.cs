using System;

namespace Unity.Entities
{
	// Token: 0x02000108 RID: 264
	internal struct ECBChainPlaybackState
	{
		// Token: 0x040002C7 RID: 711
		public unsafe ECBChunk* Chunk;

		// Token: 0x040002C8 RID: 712
		public int Offset;

		// Token: 0x040002C9 RID: 713
		public int NextSortKey;

		// Token: 0x040002CA RID: 714
		public bool CanBurstPlayback;
	}
}
