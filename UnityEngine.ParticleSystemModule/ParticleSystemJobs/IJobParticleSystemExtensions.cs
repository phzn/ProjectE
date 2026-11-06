using System;

namespace UnityEngine.ParticleSystemJobs
{
	// Token: 0x02000062 RID: 98
	public static class IJobParticleSystemExtensions
	{
		// Token: 0x06000752 RID: 1874 RVA: 0x00006775 File Offset: 0x00004975
		public static void EarlyJobInit<T>() where T : struct, IJobParticleSystem
		{
			ParticleSystemJobStruct<T>.Initialize();
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x00006780 File Offset: 0x00004980
		internal unsafe static IntPtr GetReflectionData<T>() where T : struct, IJobParticleSystem
		{
			ParticleSystemJobStruct<T>.Initialize();
			return *ParticleSystemJobStruct<T>.jobReflectionData.Data;
		}
	}
}
