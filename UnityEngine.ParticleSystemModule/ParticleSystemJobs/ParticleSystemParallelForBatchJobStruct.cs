using System;
using Unity.Burst;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs.LowLevel.Unsafe;

namespace UnityEngine.ParticleSystemJobs
{
	// Token: 0x02000072 RID: 114
	internal struct ParticleSystemParallelForBatchJobStruct<T> where T : struct, IJobParticleSystemParallelForBatch
	{
		// Token: 0x06000781 RID: 1921 RVA: 0x00006E6C File Offset: 0x0000506C
		[BurstDiscard]
		public unsafe static void Initialize()
		{
			bool flag = *ParticleSystemParallelForBatchJobStruct<T>.jobReflectionData.Data == IntPtr.Zero;
			if (flag)
			{
				*ParticleSystemParallelForBatchJobStruct<T>.jobReflectionData.Data = JobsUtility.CreateJobReflectionData(typeof(T), new ParticleSystemParallelForBatchJobStruct<T>.ExecuteJobFunction(ParticleSystemParallelForBatchJobStruct<T>.Execute), null, null);
			}
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x00006EBC File Offset: 0x000050BC
		public unsafe static void Execute(ref T data, IntPtr listDataPtr, IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex)
		{
			NativeListData* ptr = (NativeListData*)(void*)listDataPtr;
			NativeParticleData nativeParticleData;
			ParticleSystem.CopyManagedJobData(ptr->system, out nativeParticleData);
			ParticleSystemJobData particleSystemJobData = new ParticleSystemJobData(ref nativeParticleData);
			for (;;)
			{
				int num;
				int num2;
				bool flag = !JobsUtility.GetWorkStealingRange(ref ranges, jobIndex, out num, out num2);
				if (flag)
				{
					break;
				}
				data.Execute(particleSystemJobData, num, num2 - num);
			}
		}

		// Token: 0x040001C3 RID: 451
		public static readonly BurstLike.SharedStatic<IntPtr> jobReflectionData = BurstLike.SharedStatic<IntPtr>.GetOrCreate<ParticleSystemParallelForBatchJobStruct<T>>(0U);

		// Token: 0x02000073 RID: 115
		// (Invoke) Token: 0x06000785 RID: 1925
		public delegate void ExecuteJobFunction(ref T data, IntPtr listDataPtr, IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex);
	}
}
