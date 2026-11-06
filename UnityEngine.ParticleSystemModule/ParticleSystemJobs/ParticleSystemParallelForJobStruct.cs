using System;
using Unity.Burst;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs.LowLevel.Unsafe;

namespace UnityEngine.ParticleSystemJobs
{
	// Token: 0x02000070 RID: 112
	internal struct ParticleSystemParallelForJobStruct<T> where T : struct, IJobParticleSystemParallelFor
	{
		// Token: 0x0600077A RID: 1914 RVA: 0x00006D9C File Offset: 0x00004F9C
		[BurstDiscard]
		public unsafe static void Initialize()
		{
			bool flag = *ParticleSystemParallelForJobStruct<T>.jobReflectionData.Data == IntPtr.Zero;
			if (flag)
			{
				*ParticleSystemParallelForJobStruct<T>.jobReflectionData.Data = JobsUtility.CreateJobReflectionData(typeof(T), new ParticleSystemParallelForJobStruct<T>.ExecuteJobFunction(ParticleSystemParallelForJobStruct<T>.Execute), null, null);
			}
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x00006DEC File Offset: 0x00004FEC
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
				for (int i = num; i < num2; i++)
				{
					data.Execute(particleSystemJobData, i);
				}
			}
		}

		// Token: 0x040001C2 RID: 450
		public static readonly BurstLike.SharedStatic<IntPtr> jobReflectionData = BurstLike.SharedStatic<IntPtr>.GetOrCreate<ParticleSystemParallelForJobStruct<T>>(0U);

		// Token: 0x02000071 RID: 113
		// (Invoke) Token: 0x0600077E RID: 1918
		public delegate void ExecuteJobFunction(ref T data, IntPtr listDataPtr, IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex);
	}
}
