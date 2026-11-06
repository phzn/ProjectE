using System;
using GPUInstancerPro;
using Unity.Jobs;
using UnityEngine;

// Token: 0x02000072 RID: 114
[DOTSCompilerGenerated]
internal class __JobReflectionRegistrationOutput__2710939216705767182
{
	// Token: 0x0600045F RID: 1119 RVA: 0x00018CE4 File Offset: 0x00016EE4
	public static void CreateJobReflectionData()
	{
		try
		{
			IJobParallelForExtensions.EarlyJobInit<GPUINoGOUpdates.MatrixDataGeneratorJob>();
			IJobParallelForExtensions.EarlyJobInit<GPUINoGOUpdates.ColorDataGeneratorJob>();
		}
		catch (Exception ex)
		{
			EarlyInitHelpers.JobReflectionDataCreationFailed(ex);
		}
	}

	// Token: 0x06000460 RID: 1120 RVA: 0x00018D20 File Offset: 0x00016F20
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	public static void EarlyInit()
	{
		__JobReflectionRegistrationOutput__2710939216705767182.CreateJobReflectionData();
	}
}
