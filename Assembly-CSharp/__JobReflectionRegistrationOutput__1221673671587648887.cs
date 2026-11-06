using System;
using Features.Detection;
using NPCJobs;
using Unity.Jobs;
using UnityEngine;

// Token: 0x02001553 RID: 5459
[DOTSCompilerGenerated]
internal class __JobReflectionRegistrationOutput__1221673671587648887
{
	// Token: 0x0600A86E RID: 43118 RVA: 0x00464A40 File Offset: 0x00462C40
	public static void CreateJobReflectionData()
	{
		try
		{
			IJobParallelForExtensions.EarlyJobInit<ChunkManager.BakeMeshCollidersJob>();
			IJobParallelForExtensions.EarlyJobInit<ChunkVisibilityJob>();
			IJobParallelForExtensions.EarlyJobInit<FindFreeCellsJob>();
			IJobExtensions.EarlyJobInit<SortFreeCellsJob>();
			IJobExtensions.EarlyJobInit<FilterChangedChunksJob>();
			IJobParallelForExtensions.EarlyJobInit<NPCZoneCheckJob>();
			IJobParallelForExtensions.EarlyJobInit<ChunkZoneCheckJob>();
			IJobExtensions.EarlyJobInit<FindClosestNPCsJob>();
			IJobParallelForExtensions.EarlyJobInit<UpdateNPCPositionsJob>();
			IJobParallelForExtensions.EarlyJobInit<SensoryManager.DetectionJob>();
		}
		catch (Exception ex)
		{
			EarlyInitHelpers.JobReflectionDataCreationFailed(ex);
		}
	}

	// Token: 0x0600A86F RID: 43119 RVA: 0x00464AA4 File Offset: 0x00462CA4
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	public static void EarlyInit()
	{
		__JobReflectionRegistrationOutput__1221673671587648887.CreateJobReflectionData();
	}
}
