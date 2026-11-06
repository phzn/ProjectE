using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine;

// Token: 0x02000155 RID: 341
[DOTSCompilerGenerated]
internal class __JobReflectionRegistrationOutput__1652832624114795843
{
	// Token: 0x06000DDA RID: 3546 RVA: 0x0002A848 File Offset: 0x00028A48
	public static void CreateJobReflectionData()
	{
		try
		{
			IJobExtensions.EarlyJobInit<CollectionHelper.DummyJob>();
			IJobExtensions.EarlyJobInit<NativeBitArrayDisposeJob>();
			IJobExtensions.EarlyJobInit<NativeHashMapDisposeJob>();
			IJobExtensions.EarlyJobInit<NativeListDisposeJob>();
			IJobExtensions.EarlyJobInit<NativeQueueDisposeJob>();
			IJobExtensions.EarlyJobInit<NativeReferenceDisposeJob>();
			IJobExtensions.EarlyJobInit<NativeRingQueueDisposeJob>();
			IJobExtensions.EarlyJobInit<NativeStream.ConstructJobList>();
			IJobExtensions.EarlyJobInit<NativeStream.ConstructJob>();
			IJobExtensions.EarlyJobInit<NativeStreamDisposeJob>();
			IJobExtensions.EarlyJobInit<NativeTextDisposeJob>();
			IJobExtensions.EarlyJobInit<UnsafeQueueDisposeJob>();
			IJobExtensions.EarlyJobInit<UnsafeDisposeJob>();
			IJobExtensions.EarlyJobInit<UnsafeParallelHashMapDataDisposeJob>();
			IJobExtensions.EarlyJobInit<UnsafeParallelHashMapDisposeJob>();
			IJobExtensions.EarlyJobInit<UnsafeStream.DisposeJob>();
			IJobExtensions.EarlyJobInit<UnsafeStream.ConstructJobList>();
			IJobExtensions.EarlyJobInit<UnsafeStream.ConstructJob>();
		}
		catch (Exception ex)
		{
			EarlyInitHelpers.JobReflectionDataCreationFailed(ex);
		}
	}

	// Token: 0x06000DDB RID: 3547 RVA: 0x0002A8D4 File Offset: 0x00028AD4
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	public static void EarlyInit()
	{
		__JobReflectionRegistrationOutput__1652832624114795843.CreateJobReflectionData();
	}
}
