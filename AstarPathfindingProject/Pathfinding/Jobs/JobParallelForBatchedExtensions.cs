using System;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;

namespace Pathfinding.Jobs
{
	// Token: 0x02000183 RID: 387
	internal static class JobParallelForBatchedExtensions
	{
		// Token: 0x06000BC9 RID: 3017 RVA: 0x00041600 File Offset: 0x0003F800
		public static JobHandle ScheduleBatch<T>(this T jobData, int arrayLength, int minIndicesPerJobCount, JobHandle dependsOn = default(JobHandle)) where T : struct, IJobParallelForBatched
		{
			ScheduleMode scheduleMode = ScheduleMode.Batched;
			JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<T>(ref jobData), JobParallelForBatchedExtensions.ParallelForBatchJobStruct<T>.Initialize(), dependsOn, scheduleMode);
			return JobsUtility.ScheduleParallelFor(ref jobScheduleParameters, arrayLength, minIndicesPerJobCount);
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x00041630 File Offset: 0x0003F830
		public static void RunBatch<T>(this T jobData, int arrayLength) where T : struct, IJobParallelForBatched
		{
			JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<T>(ref jobData), JobParallelForBatchedExtensions.ParallelForBatchJobStruct<T>.Initialize(), default(JobHandle), ScheduleMode.Run);
			JobsUtility.ScheduleParallelFor(ref jobScheduleParameters, arrayLength, arrayLength);
		}

		// Token: 0x02000184 RID: 388
		internal struct ParallelForBatchJobStruct<T> where T : struct, IJobParallelForBatched
		{
			// Token: 0x06000BCB RID: 3019 RVA: 0x00041664 File Offset: 0x0003F864
			public static IntPtr Initialize()
			{
				if (JobParallelForBatchedExtensions.ParallelForBatchJobStruct<T>.jobReflectionData == IntPtr.Zero)
				{
					JobParallelForBatchedExtensions.ParallelForBatchJobStruct<T>.jobReflectionData = JobsUtility.CreateJobReflectionData(typeof(T), new JobParallelForBatchedExtensions.ParallelForBatchJobStruct<T>.ExecuteJobFunction(JobParallelForBatchedExtensions.ParallelForBatchJobStruct<T>.Execute), null, null);
				}
				return JobParallelForBatchedExtensions.ParallelForBatchJobStruct<T>.jobReflectionData;
			}

			// Token: 0x06000BCC RID: 3020 RVA: 0x000416A0 File Offset: 0x0003F8A0
			public static void Execute(ref T jobData, IntPtr additionalPtr, IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex)
			{
				int num;
				int num2;
				while (JobsUtility.GetWorkStealingRange(ref ranges, jobIndex, out num, out num2))
				{
					jobData.Execute(num, num2 - num);
				}
			}

			// Token: 0x040007BE RID: 1982
			public static IntPtr jobReflectionData;

			// Token: 0x02000185 RID: 389
			// (Invoke) Token: 0x06000BCE RID: 3022
			public delegate void ExecuteJobFunction(ref T data, IntPtr additionalPtr, IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex);
		}
	}
}
