using System;
using Unity.Jobs.LowLevel.Unsafe;

namespace UnityEngine.ParticleSystemJobs
{
	// Token: 0x02000061 RID: 97
	[JobProducerType(typeof(ParticleSystemParallelForBatchJobStruct<>))]
	public interface IJobParticleSystemParallelForBatch
	{
		// Token: 0x06000751 RID: 1873
		void Execute(ParticleSystemJobData jobData, int startIndex, int count);
	}
}
