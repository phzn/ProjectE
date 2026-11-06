using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Unity.Jobs
{
	// Token: 0x0200000B RID: 11
	public static class IJobFilterExtensions
	{
		// Token: 0x06000012 RID: 18 RVA: 0x00002176 File Offset: 0x00000376
		public static void EarlyJobInit<T>() where T : struct, IJobFilter
		{
			IJobFilterExtensions.JobFilterProducer<T>.Initialize();
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000217D File Offset: 0x0000037D
		private unsafe static IntPtr GetReflectionData<T>() where T : struct, IJobFilter
		{
			IJobFilterExtensions.JobFilterProducer<T>.Initialize();
			return *IJobFilterExtensions.JobFilterProducer<T>.jobReflectionData.Data;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000218F File Offset: 0x0000038F
		public static JobHandle ScheduleAppend<T>(this T jobData, NativeList<int> indices, int arrayLength, JobHandle dependsOn = default(JobHandle)) where T : struct, IJobFilter
		{
			return (ref jobData).ScheduleAppendByRef<T>(indices, arrayLength, dependsOn);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000219B File Offset: 0x0000039B
		public static JobHandle ScheduleFilter<T>(this T jobData, NativeList<int> indices, JobHandle dependsOn = default(JobHandle)) where T : struct, IJobFilter
		{
			return (ref jobData).ScheduleFilterByRef<T>(indices, dependsOn);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000021A6 File Offset: 0x000003A6
		public static void RunAppend<T>(this T jobData, NativeList<int> indices, int arrayLength) where T : struct, IJobFilter
		{
			(ref jobData).RunAppendByRef<T>(indices, arrayLength);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000021B1 File Offset: 0x000003B1
		public static void RunFilter<T>(this T jobData, NativeList<int> indices) where T : struct, IJobFilter
		{
			(ref jobData).RunFilterByRef<T>(indices);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000021BC File Offset: 0x000003BC
		public static JobHandle ScheduleAppendByRef<T>(this T jobData, NativeList<int> indices, int arrayLength, JobHandle dependsOn = default(JobHandle)) where T : struct, IJobFilter
		{
			IJobFilterExtensions.JobFilterProducer<T>.JobWrapper jobWrapper = new IJobFilterExtensions.JobFilterProducer<T>.JobWrapper
			{
				JobData = jobData,
				outputIndices = indices,
				appendCount = arrayLength
			};
			JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<IJobFilterExtensions.JobFilterProducer<T>.JobWrapper>(ref jobWrapper), IJobFilterExtensions.GetReflectionData<T>(), dependsOn, ScheduleMode.Single);
			return JobsUtility.Schedule(ref jobScheduleParameters);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x0000220C File Offset: 0x0000040C
		public static JobHandle ScheduleFilterByRef<T>(this T jobData, NativeList<int> indices, JobHandle dependsOn = default(JobHandle)) where T : struct, IJobFilter
		{
			IJobFilterExtensions.JobFilterProducer<T>.JobWrapper jobWrapper = new IJobFilterExtensions.JobFilterProducer<T>.JobWrapper
			{
				JobData = jobData,
				outputIndices = indices,
				appendCount = -1
			};
			JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<IJobFilterExtensions.JobFilterProducer<T>.JobWrapper>(ref jobWrapper), IJobFilterExtensions.GetReflectionData<T>(), dependsOn, ScheduleMode.Single);
			return JobsUtility.Schedule(ref jobScheduleParameters);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0000225C File Offset: 0x0000045C
		public static void RunAppendByRef<T>(this T jobData, NativeList<int> indices, int arrayLength) where T : struct, IJobFilter
		{
			IJobFilterExtensions.JobFilterProducer<T>.JobWrapper jobWrapper = new IJobFilterExtensions.JobFilterProducer<T>.JobWrapper
			{
				JobData = jobData,
				outputIndices = indices,
				appendCount = arrayLength
			};
			JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<IJobFilterExtensions.JobFilterProducer<T>.JobWrapper>(ref jobWrapper), IJobFilterExtensions.GetReflectionData<T>(), default(JobHandle), ScheduleMode.Run);
			JobsUtility.Schedule(ref jobScheduleParameters);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000022B8 File Offset: 0x000004B8
		public static void RunFilterByRef<T>(this T jobData, NativeList<int> indices) where T : struct, IJobFilter
		{
			IJobFilterExtensions.JobFilterProducer<T>.JobWrapper jobWrapper = new IJobFilterExtensions.JobFilterProducer<T>.JobWrapper
			{
				JobData = jobData,
				outputIndices = indices,
				appendCount = -1
			};
			JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<IJobFilterExtensions.JobFilterProducer<T>.JobWrapper>(ref jobWrapper), IJobFilterExtensions.GetReflectionData<T>(), default(JobHandle), ScheduleMode.Run);
			JobsUtility.Schedule(ref jobScheduleParameters);
		}

		// Token: 0x0200000C RID: 12
		internal struct JobFilterProducer<T> where T : struct, IJobFilter
		{
			// Token: 0x0600001C RID: 28 RVA: 0x00002314 File Offset: 0x00000514
			[BurstDiscard]
			internal unsafe static void Initialize()
			{
				if (*IJobFilterExtensions.JobFilterProducer<T>.jobReflectionData.Data == IntPtr.Zero)
				{
					*IJobFilterExtensions.JobFilterProducer<T>.jobReflectionData.Data = JobsUtility.CreateJobReflectionData(typeof(IJobFilterExtensions.JobFilterProducer<T>.JobWrapper), typeof(T), new IJobFilterExtensions.JobFilterProducer<T>.ExecuteJobFunction(IJobFilterExtensions.JobFilterProducer<T>.Execute));
				}
			}

			// Token: 0x0600001D RID: 29 RVA: 0x00002368 File Offset: 0x00000568
			public static void Execute(ref IJobFilterExtensions.JobFilterProducer<T>.JobWrapper jobWrapper, IntPtr additionalPtr, IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex)
			{
				if (jobWrapper.appendCount == -1)
				{
					IJobFilterExtensions.JobFilterProducer<T>.ExecuteFilter(ref jobWrapper, bufferRangePatchData);
					return;
				}
				IJobFilterExtensions.JobFilterProducer<T>.ExecuteAppend(ref jobWrapper, bufferRangePatchData);
			}

			// Token: 0x0600001E RID: 30 RVA: 0x00002384 File Offset: 0x00000584
			public unsafe static void ExecuteAppend(ref IJobFilterExtensions.JobFilterProducer<T>.JobWrapper jobWrapper, IntPtr bufferRangePatchData)
			{
				int length = jobWrapper.outputIndices.Length;
				jobWrapper.outputIndices.Capacity = math.max(jobWrapper.appendCount + length, jobWrapper.outputIndices.Capacity);
				int* unsafePtr = jobWrapper.outputIndices.GetUnsafePtr<int>();
				int num = length;
				for (int num2 = 0; num2 != jobWrapper.appendCount; num2++)
				{
					if (jobWrapper.JobData.Execute(num2))
					{
						unsafePtr[num] = num2;
						num++;
					}
				}
				jobWrapper.outputIndices.ResizeUninitialized(num);
			}

			// Token: 0x0600001F RID: 31 RVA: 0x0000240C File Offset: 0x0000060C
			public unsafe static void ExecuteFilter(ref IJobFilterExtensions.JobFilterProducer<T>.JobWrapper jobWrapper, IntPtr bufferRangePatchData)
			{
				int* unsafePtr = jobWrapper.outputIndices.GetUnsafePtr<int>();
				int length = jobWrapper.outputIndices.Length;
				int num = 0;
				for (int num2 = 0; num2 != length; num2++)
				{
					int num3 = unsafePtr[num2];
					if (jobWrapper.JobData.Execute(num3))
					{
						unsafePtr[num] = num3;
						num++;
					}
				}
				jobWrapper.outputIndices.ResizeUninitialized(num);
			}

			// Token: 0x04000007 RID: 7
			internal static readonly SharedStatic<IntPtr> jobReflectionData = SharedStatic<IntPtr>.GetOrCreate<IJobFilterExtensions.JobFilterProducer<T>>(0U);

			// Token: 0x0200000D RID: 13
			public struct JobWrapper
			{
				// Token: 0x04000008 RID: 8
				[NativeDisableParallelForRestriction]
				public NativeList<int> outputIndices;

				// Token: 0x04000009 RID: 9
				public int appendCount;

				// Token: 0x0400000A RID: 10
				public T JobData;
			}

			// Token: 0x0200000E RID: 14
			// (Invoke) Token: 0x06000022 RID: 34
			public delegate void ExecuteJobFunction(ref IJobFilterExtensions.JobFilterProducer<T>.JobWrapper jobWrapper, IntPtr additionalPtr, IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex);
		}
	}
}
