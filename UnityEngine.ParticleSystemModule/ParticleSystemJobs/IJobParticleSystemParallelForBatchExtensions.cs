using System;

namespace UnityEngine.ParticleSystemJobs
{
	// Token: 0x02000064 RID: 100
	public static class IJobParticleSystemParallelForBatchExtensions
	{
		// Token: 0x06000756 RID: 1878 RVA: 0x000067D5 File Offset: 0x000049D5
		public static void EarlyJobInit<T>() where T : struct, IJobParticleSystemParallelForBatch
		{
			ParticleSystemParallelForBatchJobStruct<T>.Initialize();
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x000067E0 File Offset: 0x000049E0
		internal unsafe static IntPtr GetReflectionData<T>() where T : struct, IJobParticleSystemParallelForBatch
		{
			ParticleSystemParallelForBatchJobStruct<T>.Initialize();
			return *ParticleSystemParallelForBatchJobStruct<T>.jobReflectionData.Data;
		}
	}
}
