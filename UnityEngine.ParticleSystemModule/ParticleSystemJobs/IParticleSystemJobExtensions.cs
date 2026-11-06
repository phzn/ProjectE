using System;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;

namespace UnityEngine.ParticleSystemJobs
{
	// Token: 0x02000066 RID: 102
	public static class IParticleSystemJobExtensions
	{
		// Token: 0x06000759 RID: 1881 RVA: 0x00006838 File Offset: 0x00004A38
		public static JobHandle Schedule<T>(this T jobData, ParticleSystem ps, JobHandle dependsOn = default(JobHandle)) where T : struct, IJobParticleSystem
		{
			bool flag = ParticleSystem.UserJobCanBeScheduled();
			if (flag)
			{
				JobsUtility.JobScheduleParameters jobScheduleParameters = ParticleSystemJobUtility.CreateScheduleParams<T>(ref jobData, ps, dependsOn, IJobParticleSystemExtensions.GetReflectionData<T>());
				JobHandle jobHandle = ParticleSystem.ScheduleManagedJob(ref jobScheduleParameters, ps.GetManagedJobData());
				ps.SetManagedJobHandle(jobHandle);
				return jobHandle;
			}
			throw new InvalidOperationException(IParticleSystemJobExtensions.k_UserJobScheduledOutsideOfCallbackErrorMsg);
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x00006888 File Offset: 0x00004A88
		public static JobHandle Schedule<T>(this T jobData, ParticleSystem ps, int minIndicesPerJobCount, JobHandle dependsOn = default(JobHandle)) where T : struct, IJobParticleSystemParallelFor
		{
			bool flag = ParticleSystem.UserJobCanBeScheduled();
			if (flag)
			{
				JobsUtility.JobScheduleParameters jobScheduleParameters = ParticleSystemJobUtility.CreateScheduleParams<T>(ref jobData, ps, dependsOn, IJobParticleSystemParallelForExtensions.GetReflectionData<T>());
				JobHandle jobHandle = JobsUtility.ScheduleParallelForDeferArraySize(ref jobScheduleParameters, minIndicesPerJobCount, ps.GetManagedJobData(), null);
				ps.SetManagedJobHandle(jobHandle);
				return jobHandle;
			}
			throw new InvalidOperationException(IParticleSystemJobExtensions.k_UserJobScheduledOutsideOfCallbackErrorMsg);
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x000068DC File Offset: 0x00004ADC
		public static JobHandle ScheduleBatch<T>(this T jobData, ParticleSystem ps, int innerLoopBatchCount, JobHandle dependsOn = default(JobHandle)) where T : struct, IJobParticleSystemParallelForBatch
		{
			bool flag = ParticleSystem.UserJobCanBeScheduled();
			if (flag)
			{
				JobsUtility.JobScheduleParameters jobScheduleParameters = ParticleSystemJobUtility.CreateScheduleParams<T>(ref jobData, ps, dependsOn, IJobParticleSystemParallelForBatchExtensions.GetReflectionData<T>());
				JobHandle jobHandle = JobsUtility.ScheduleParallelForDeferArraySize(ref jobScheduleParameters, innerLoopBatchCount, ps.GetManagedJobData(), null);
				ps.SetManagedJobHandle(jobHandle);
				return jobHandle;
			}
			throw new InvalidOperationException(IParticleSystemJobExtensions.k_UserJobScheduledOutsideOfCallbackErrorMsg);
		}

		// Token: 0x04000193 RID: 403
		private static readonly string k_UserJobScheduledOutsideOfCallbackErrorMsg = "Particle System jobs can only be scheduled in MonoBehaviour.OnParticleUpdateJobScheduled()";
	}
}
