using System;
using Unity.Jobs.LowLevel.Unsafe;

namespace UnityEngine.ParticleSystemJobs
{
	// Token: 0x02000060 RID: 96
	[JobProducerType(typeof(ParticleSystemParallelForJobStruct<>))]
	public interface IJobParticleSystemParallelFor
	{
		// Token: 0x06000750 RID: 1872
		void Execute(ParticleSystemJobData jobData, int index);
	}
}
