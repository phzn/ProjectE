using System;
using Pathfinding.Drawing;
using Unity.Jobs;
using UnityEngine;

// Token: 0x02000063 RID: 99
[DOTSCompilerGenerated]
internal class __JobReflectionRegistrationOutput__17700261659878220535
{
	// Token: 0x060002B6 RID: 694 RVA: 0x00010D0C File Offset: 0x0000EF0C
	public static void CreateJobReflectionData()
	{
		try
		{
			IJobExtensions.EarlyJobInit<GeometryBuilderJob>();
			IJobExtensions.EarlyJobInit<PersistentFilterJob>();
			IJobExtensions.EarlyJobInit<StreamSplitter>();
		}
		catch (Exception ex)
		{
			EarlyInitHelpers.JobReflectionDataCreationFailed(ex);
		}
	}

	// Token: 0x060002B7 RID: 695 RVA: 0x00010D4C File Offset: 0x0000EF4C
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	public static void EarlyInit()
	{
		__JobReflectionRegistrationOutput__17700261659878220535.CreateJobReflectionData();
	}
}
