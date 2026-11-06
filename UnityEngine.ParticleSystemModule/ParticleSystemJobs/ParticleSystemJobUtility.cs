using System;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;

namespace UnityEngine.ParticleSystemJobs
{
	// Token: 0x02000065 RID: 101
	internal static class ParticleSystemJobUtility
	{
		// Token: 0x06000758 RID: 1880 RVA: 0x00006808 File Offset: 0x00004A08
		internal static JobsUtility.JobScheduleParameters CreateScheduleParams<T>(ref T jobData, ParticleSystem ps, JobHandle dependsOn, IntPtr jobReflectionData) where T : struct
		{
			dependsOn = JobHandle.CombineDependencies(ps.GetManagedJobHandle(), dependsOn);
			return new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<T>(ref jobData), jobReflectionData, dependsOn, ScheduleMode.Batched);
		}
	}
}
