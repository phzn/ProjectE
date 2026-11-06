using System;
using Unity.Burst.Intrinsics;
using Unity.Jobs.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x020001AA RID: 426
	[JobProducerType(typeof(JobChunkExtensions.JobChunkProducer<>))]
	public interface IJobChunk
	{
		// Token: 0x06000A19 RID: 2585
		void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask);
	}
}
