using System;
using Unity.Burst.Intrinsics;

namespace Unity.Entities
{
	// Token: 0x020001B0 RID: 432
	public interface IJobEntityChunkBeginEnd
	{
		// Token: 0x06000A2B RID: 2603
		bool OnChunkBegin(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask);

		// Token: 0x06000A2C RID: 2604
		void OnChunkEnd(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask, bool chunkWasExecuted);
	}
}
