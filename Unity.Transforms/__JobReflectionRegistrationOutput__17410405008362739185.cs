using System;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

// Token: 0x02000022 RID: 34
[DOTSCompilerGenerated]
internal class __JobReflectionRegistrationOutput__17410405008362739185
{
	// Token: 0x06000097 RID: 151 RVA: 0x00004304 File Offset: 0x00002504
	public static void CreateJobReflectionData()
	{
		try
		{
			JobChunkExtensions.EarlyJobInit<ParentSystem.GatherChangedParents>();
			IJobExtensions.EarlyJobInit<ParentSystem.FindMissingChild>();
			IJobExtensions.EarlyJobInit<ParentSystem.FixupChangedChildren>();
			IJobExtensions.EarlyJobInit<ParentSystem.GatherChildEntities>();
			JobChunkExtensions.EarlyJobInit<LocalToWorldSystem.ComputeWorldSpaceLocalToWorldJob>();
			IJobParallelForDeferExtensions.EarlyJobInit<LocalToWorldSystem.ComputeHierarchyLocalToWorldJob>();
		}
		catch (Exception ex)
		{
			EarlyInitHelpers.JobReflectionDataCreationFailed(ex);
		}
	}

	// Token: 0x06000098 RID: 152 RVA: 0x00004354 File Offset: 0x00002554
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	public static void EarlyInit()
	{
		__JobReflectionRegistrationOutput__17410405008362739185.CreateJobReflectionData();
	}
}
