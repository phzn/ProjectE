using System;
using Unity.Burst;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs.LowLevel.Unsafe;

namespace Unity.Jobs
{
	// Token: 0x02000010 RID: 16
	public static class IJobParallelForBatchExtensions
	{
		// Token: 0x06000026 RID: 38 RVA: 0x00002483 File Offset: 0x00000683
		public static void EarlyJobInit<T>() where T : struct, IJobParallelForBatch
		{
			IJobParallelForBatchExtensions.JobParallelForBatchProducer<T>.Initialize();
		}

		// Token: 0x06000027 RID: 39 RVA: 0x0000248A File Offset: 0x0000068A
		private unsafe static IntPtr GetReflectionData<T>() where T : struct, IJobParallelForBatch
		{
			IJobParallelForBatchExtensions.JobParallelForBatchProducer<T>.Initialize();
			return *IJobParallelForBatchExtensions.JobParallelForBatchProducer<T>.jobReflectionData.Data;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x0000249C File Offset: 0x0000069C
		public static JobHandle Schedule<T>(this T jobData, int arrayLength, int indicesPerJobCount, JobHandle dependsOn = default(JobHandle)) where T : struct, IJobParallelForBatch
		{
			JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<T>(ref jobData), IJobParallelForBatchExtensions.GetReflectionData<T>(), dependsOn, ScheduleMode.Single);
			return JobsUtility.ScheduleParallelFor(ref jobScheduleParameters, arrayLength, indicesPerJobCount);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000024C8 File Offset: 0x000006C8
		public static JobHandle ScheduleByRef<T>(this T jobData, int arrayLength, int indicesPerJobCount, JobHandle dependsOn = default(JobHandle)) where T : struct, IJobParallelForBatch
		{
			JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<T>(ref jobData), IJobParallelForBatchExtensions.GetReflectionData<T>(), dependsOn, ScheduleMode.Single);
			return JobsUtility.ScheduleParallelFor(ref jobScheduleParameters, arrayLength, indicesPerJobCount);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000024F4 File Offset: 0x000006F4
		public static JobHandle ScheduleParallel<T>(this T jobData, int arrayLength, int indicesPerJobCount, JobHandle dependsOn = default(JobHandle)) where T : struct, IJobParallelForBatch
		{
			JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<T>(ref jobData), IJobParallelForBatchExtensions.GetReflectionData<T>(), dependsOn, ScheduleMode.Batched);
			return JobsUtility.ScheduleParallelFor(ref jobScheduleParameters, arrayLength, indicesPerJobCount);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002520 File Offset: 0x00000720
		public static JobHandle ScheduleParallelByRef<T>(this T jobData, int arrayLength, int indicesPerJobCount, JobHandle dependsOn = default(JobHandle)) where T : struct, IJobParallelForBatch
		{
			JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<T>(ref jobData), IJobParallelForBatchExtensions.GetReflectionData<T>(), dependsOn, ScheduleMode.Batched);
			return JobsUtility.ScheduleParallelFor(ref jobScheduleParameters, arrayLength, indicesPerJobCount);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x0000254A File Offset: 0x0000074A
		public static JobHandle ScheduleBatch<T>(this T jobData, int arrayLength, int indicesPerJobCount, JobHandle dependsOn = default(JobHandle)) where T : struct, IJobParallelForBatch
		{
			return jobData.ScheduleParallel(arrayLength, indicesPerJobCount, dependsOn);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002555 File Offset: 0x00000755
		public static JobHandle ScheduleBatchByRef<T>(this T jobData, int arrayLength, int indicesPerJobCount, JobHandle dependsOn = default(JobHandle)) where T : struct, IJobParallelForBatch
		{
			return (ref jobData).ScheduleParallelByRef<T>(arrayLength, indicesPerJobCount, dependsOn);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002560 File Offset: 0x00000760
		public static void Run<T>(this T jobData, int arrayLength, int indicesPerJobCount) where T : struct, IJobParallelForBatch
		{
			JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<T>(ref jobData), IJobParallelForBatchExtensions.GetReflectionData<T>(), default(JobHandle), ScheduleMode.Run);
			JobsUtility.ScheduleParallelFor(ref jobScheduleParameters, arrayLength, arrayLength);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002594 File Offset: 0x00000794
		public static void RunByRef<T>(this T jobData, int arrayLength, int indicesPerJobCount) where T : struct, IJobParallelForBatch
		{
			JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<T>(ref jobData), IJobParallelForBatchExtensions.GetReflectionData<T>(), default(JobHandle), ScheduleMode.Run);
			JobsUtility.ScheduleParallelFor(ref jobScheduleParameters, arrayLength, arrayLength);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000025C7 File Offset: 0x000007C7
		public static void RunBatch<T>(this T jobData, int arrayLength) where T : struct, IJobParallelForBatch
		{
			jobData.Run(arrayLength, arrayLength);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000025D1 File Offset: 0x000007D1
		public static void RunBatchByRef<T>(this T jobData, int arrayLength) where T : struct, IJobParallelForBatch
		{
			(ref jobData).RunByRef<T>(arrayLength, arrayLength);
		}

		// Token: 0x02000011 RID: 17
		internal struct JobParallelForBatchProducer<T> where T : struct, IJobParallelForBatch
		{
			// Token: 0x06000032 RID: 50 RVA: 0x000025DC File Offset: 0x000007DC
			[BurstDiscard]
			internal unsafe static void Initialize()
			{
				if (*IJobParallelForBatchExtensions.JobParallelForBatchProducer<T>.jobReflectionData.Data == IntPtr.Zero)
				{
					*IJobParallelForBatchExtensions.JobParallelForBatchProducer<T>.jobReflectionData.Data = JobsUtility.CreateJobReflectionData(typeof(T), new IJobParallelForBatchExtensions.JobParallelForBatchProducer<T>.ExecuteJobFunction(IJobParallelForBatchExtensions.JobParallelForBatchProducer<T>.Execute), null, null);
				}
			}

			// Token: 0x06000033 RID: 51 RVA: 0x00002628 File Offset: 0x00000828
			public static void Execute(ref T jobData, IntPtr additionalPtr, IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex)
			{
				int num;
				int num2;
				while (JobsUtility.GetWorkStealingRange(ref ranges, jobIndex, out num, out num2))
				{
					jobData.Execute(num, num2 - num);
				}
			}

			// Token: 0x0400000B RID: 11
			internal static readonly SharedStatic<IntPtr> jobReflectionData = SharedStatic<IntPtr>.GetOrCreate<IJobParallelForBatchExtensions.JobParallelForBatchProducer<T>>(0U);

			// Token: 0x02000012 RID: 18
			// (Invoke) Token: 0x06000036 RID: 54
			internal delegate void ExecuteJobFunction(ref T jobData, IntPtr additionalPtr, IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex);
		}
	}
}
