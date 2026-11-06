using System;
using Unity.Jobs.LowLevel.Unsafe;

namespace UnityEngine.ParticleSystemJobs
{
	// Token: 0x0200005F RID: 95
	[JobProducerType(typeof(ParticleSystemJobStruct<>))]
	public interface IJobParticleSystem
	{
		// Token: 0x0600074F RID: 1871
		void Execute(ParticleSystemJobData jobData);
	}
}
