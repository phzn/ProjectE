using System;

namespace UnityEngine.ParticleSystemJobs
{
	// Token: 0x02000063 RID: 99
	public static class IJobParticleSystemParallelForExtensions
	{
		// Token: 0x06000754 RID: 1876 RVA: 0x000067A5 File Offset: 0x000049A5
		public static void EarlyJobInit<T>() where T : struct, IJobParticleSystemParallelFor
		{
			ParticleSystemParallelForJobStruct<T>.Initialize();
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x000067B0 File Offset: 0x000049B0
		internal unsafe static IntPtr GetReflectionData<T>() where T : struct, IJobParticleSystemParallelFor
		{
			ParticleSystemParallelForJobStruct<T>.Initialize();
			return *ParticleSystemParallelForJobStruct<T>.jobReflectionData.Data;
		}
	}
}
