using System;
using GPUInstancerPro.PrefabModule;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

// Token: 0x0200001F RID: 31
[DOTSCompilerGenerated]
internal class __JobReflectionRegistrationOutput__6368090013526615711
{
	// Token: 0x060000B0 RID: 176 RVA: 0x00005BBC File Offset: 0x00003DBC
	public static void CreateJobReflectionData()
	{
		try
		{
			IJobParallelForTransformExtensions.EarlyJobInit<GPUIAutoUpdateTransformsJob>();
		}
		catch (Exception ex)
		{
			EarlyInitHelpers.JobReflectionDataCreationFailed(ex);
		}
	}

	// Token: 0x060000B1 RID: 177 RVA: 0x00005BF0 File Offset: 0x00003DF0
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	public static void EarlyInit()
	{
		__JobReflectionRegistrationOutput__6368090013526615711.CreateJobReflectionData();
	}
}
