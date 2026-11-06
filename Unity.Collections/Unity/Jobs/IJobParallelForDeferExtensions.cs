using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs.LowLevel.Unsafe;

namespace Unity.Jobs
{
	// Token: 0x02000014 RID: 20
	public static class IJobParallelForDeferExtensions
	{
		// Token: 0x0600003A RID: 58 RVA: 0x00002662 File Offset: 0x00000862
		public static void EarlyJobInit<T>() where T : struct, IJobParallelForDefer
		{
			IJobParallelForDeferExtensions.JobParallelForDeferProducer<T>.Initialize();
		}

		// Token: 0x0600003B RID: 59 RVA: 0x0000266C File Offset: 0x0000086C
		public unsafe static JobHandle Schedule<T, [global::System.Runtime.CompilerServices.IsUnmanaged] U>(this T jobData, NativeList<U> list, int innerloopBatchCount, JobHandle dependsOn = default(JobHandle)) where T : struct, IJobParallelForDefer where U : struct, ValueType
		{
			void* ptr = null;
			return IJobParallelForDeferExtensions.ScheduleInternal<T>(ref jobData, innerloopBatchCount, NativeListUnsafeUtility.GetInternalListDataPtrUnchecked<U>(ref list), ptr, dependsOn);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002690 File Offset: 0x00000890
		public unsafe static JobHandle ScheduleByRef<T, [global::System.Runtime.CompilerServices.IsUnmanaged] U>(this T jobData, NativeList<U> list, int innerloopBatchCount, JobHandle dependsOn = default(JobHandle)) where T : struct, IJobParallelForDefer where U : struct, ValueType
		{
			void* ptr = null;
			return IJobParallelForDeferExtensions.ScheduleInternal<T>(ref jobData, innerloopBatchCount, NativeListUnsafeUtility.GetInternalListDataPtrUnchecked<U>(ref list), ptr, dependsOn);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000026B0 File Offset: 0x000008B0
		public unsafe static JobHandle Schedule<T>(this T jobData, int* forEachCount, int innerloopBatchCount, JobHandle dependsOn = default(JobHandle)) where T : struct, IJobParallelForDefer
		{
			byte* ptr = (byte*)(forEachCount - sizeof(void*) / 4);
			return IJobParallelForDeferExtensions.ScheduleInternal<T>(ref jobData, innerloopBatchCount, (void*)ptr, null, dependsOn);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000026D4 File Offset: 0x000008D4
		public unsafe static JobHandle ScheduleByRef<T>(this T jobData, int* forEachCount, int innerloopBatchCount, JobHandle dependsOn = default(JobHandle)) where T : struct, IJobParallelForDefer
		{
			byte* ptr = (byte*)(forEachCount - sizeof(void*) / 4);
			return IJobParallelForDeferExtensions.ScheduleInternal<T>(ref jobData, innerloopBatchCount, (void*)ptr, null, dependsOn);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000026F8 File Offset: 0x000008F8
		private unsafe static JobHandle ScheduleInternal<T>(ref T jobData, int innerloopBatchCount, void* forEachListPtr, void* atomicSafetyHandlePtr, JobHandle dependsOn) where T : struct, IJobParallelForDefer
		{
			IJobParallelForDeferExtensions.JobParallelForDeferProducer<T>.Initialize();
			IntPtr intPtr = *IJobParallelForDeferExtensions.JobParallelForDeferProducer<T>.jobReflectionData.Data;
			JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<T>(ref jobData), intPtr, dependsOn, ScheduleMode.Batched);
			return JobsUtility.ScheduleParallelForDeferArraySize(ref jobScheduleParameters, innerloopBatchCount, forEachListPtr, atomicSafetyHandlePtr);
		}

		// Token: 0x02000015 RID: 21
		internal struct JobParallelForDeferProducer<T> where T : struct, IJobParallelForDefer
		{
			// Token: 0x06000040 RID: 64 RVA: 0x00002734 File Offset: 0x00000934
			[BurstDiscard]
			internal unsafe static void Initialize()
			{
				if (*IJobParallelForDeferExtensions.JobParallelForDeferProducer<T>.jobReflectionData.Data == IntPtr.Zero)
				{
					*IJobParallelForDeferExtensions.JobParallelForDeferProducer<T>.jobReflectionData.Data = JobsUtility.CreateJobReflectionData(typeof(T), new IJobParallelForDeferExtensions.JobParallelForDeferProducer<T>.ExecuteJobFunction(IJobParallelForDeferExtensions.JobParallelForDeferProducer<T>.Execute), null, null);
				}
			}

			// Token: 0x06000041 RID: 65 RVA: 0x00002780 File Offset: 0x00000980
			public static void Execute(ref T jobData, IntPtr additionalPtr, IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex)
			{
				int num;
				int num2;
				while (JobsUtility.GetWorkStealingRange(ref ranges, jobIndex, out num, out num2))
				{
					int num3 = num2;
					for (int i = num; i < num3; i++)
					{
						jobData.Execute(i);
					}
				}
			}

			// Token: 0x0400000C RID: 12
			internal static readonly SharedStatic<IntPtr> jobReflectionData = SharedStatic<IntPtr>.GetOrCreate<IJobParallelForDeferExtensions.JobParallelForDeferProducer<T>>(0U);

			// Token: 0x02000016 RID: 22
			// (Invoke) Token: 0x06000044 RID: 68
			public delegate void ExecuteJobFunction(ref T jobData, IntPtr additionalPtr, IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex);
		}
	}
}
