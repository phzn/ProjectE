using System;
using GPUInstancerPro.TerrainModule;
using Unity.Jobs;
using UnityEngine;

// Token: 0x02000021 RID: 33
[DOTSCompilerGenerated]
internal class __JobReflectionRegistrationOutput__14655760061624130192
{
	// Token: 0x06000179 RID: 377 RVA: 0x0000AED8 File Offset: 0x000090D8
	public static void CreateJobReflectionData()
	{
		try
		{
			IJobParallelForExtensions.EarlyJobInit<GPUIInfiniteTerrainGenerator.CreateHeightmapArrayJob>();
		}
		catch (Exception ex)
		{
			EarlyInitHelpers.JobReflectionDataCreationFailed(ex);
		}
	}

	// Token: 0x0600017A RID: 378 RVA: 0x0000AF0C File Offset: 0x0000910C
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	public static void EarlyInit()
	{
		__JobReflectionRegistrationOutput__14655760061624130192.CreateJobReflectionData();
	}
}
