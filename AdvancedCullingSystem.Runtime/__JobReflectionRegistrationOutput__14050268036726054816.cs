using System;
using NGS.AdvancedCullingSystem.Static;
using Unity.Jobs;
using UnityEngine;

// Token: 0x0200005B RID: 91
[DOTSCompilerGenerated]
internal class __JobReflectionRegistrationOutput__14050268036726054816
{
	// Token: 0x06000229 RID: 553 RVA: 0x0000965C File Offset: 0x0000785C
	public static void CreateJobReflectionData()
	{
		try
		{
			IJobExtensions.EarlyJobInit<StaticCullingBaker.TreeCreateRaysJob>();
			IJobExtensions.EarlyJobInit<StaticCullingBaker.TreeComputeResultsJob>();
			IJobExtensions.EarlyJobInit<StaticCullingBaker.TargetsCreateRaysJob>();
			IJobExtensions.EarlyJobInit<StaticCullingBaker.TargetsComputeResultsJob>();
		}
		catch (Exception ex)
		{
			EarlyInitHelpers.JobReflectionDataCreationFailed(ex);
		}
	}

	// Token: 0x0600022A RID: 554 RVA: 0x000096A0 File Offset: 0x000078A0
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	public static void EarlyInit()
	{
		__JobReflectionRegistrationOutput__14050268036726054816.CreateJobReflectionData();
	}
}
