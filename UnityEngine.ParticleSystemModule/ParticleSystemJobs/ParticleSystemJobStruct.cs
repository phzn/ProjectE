using System;
using Unity.Burst;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs.LowLevel.Unsafe;

namespace UnityEngine.ParticleSystemJobs
{
	// Token: 0x0200006E RID: 110
	internal struct ParticleSystemJobStruct<T> where T : struct, IJobParticleSystem
	{
		// Token: 0x06000773 RID: 1907 RVA: 0x00006D04 File Offset: 0x00004F04
		[BurstDiscard]
		public unsafe static void Initialize()
		{
			bool flag = *ParticleSystemJobStruct<T>.jobReflectionData.Data == IntPtr.Zero;
			if (flag)
			{
				*ParticleSystemJobStruct<T>.jobReflectionData.Data = JobsUtility.CreateJobReflectionData(typeof(T), new ParticleSystemJobStruct<T>.ExecuteJobFunction(ParticleSystemJobStruct<T>.Execute), null, null);
			}
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x00006D54 File Offset: 0x00004F54
		public unsafe static void Execute(ref T data, IntPtr listDataPtr, IntPtr unusedPtr, ref JobRanges ranges, int jobIndex)
		{
			NativeListData* ptr = (NativeListData*)(void*)listDataPtr;
			NativeParticleData nativeParticleData;
			ParticleSystem.CopyManagedJobData(ptr->system, out nativeParticleData);
			ParticleSystemJobData particleSystemJobData = new ParticleSystemJobData(ref nativeParticleData);
			data.Execute(particleSystemJobData);
		}

		// Token: 0x040001C1 RID: 449
		public static readonly BurstLike.SharedStatic<IntPtr> jobReflectionData = BurstLike.SharedStatic<IntPtr>.GetOrCreate<ParticleSystemJobStruct<T>>(0U);

		// Token: 0x0200006F RID: 111
		// (Invoke) Token: 0x06000777 RID: 1911
		public delegate void ExecuteJobFunction(ref T data, IntPtr listDataPtr, IntPtr unusedPtr, ref JobRanges ranges, int jobIndex);
	}
}
