using System;
using andywiecko.BurstTriangulator.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x02000031 RID: 49
[DOTSCompilerGenerated]
internal class __JobReflectionRegistrationOutput__7274761993000005315
{
	// Token: 0x06000191 RID: 401 RVA: 0x000095A4 File Offset: 0x000077A4
	public static void CreateJobReflectionData()
	{
		try
		{
			IJobExtensions.EarlyJobInit<TriangulationJob<float, float2, float, TransformFloat, FloatUtils>>();
			IJobExtensions.EarlyJobInit<TriangulationJob<double, double2, double, TransformDouble, DoubleUtils>>();
			IJobExtensions.EarlyJobInit<TriangulationJob<int, int2, long, TransformInt, IntUtils>>();
		}
		catch (Exception ex)
		{
			EarlyInitHelpers.JobReflectionDataCreationFailed(ex);
		}
	}

	// Token: 0x06000192 RID: 402 RVA: 0x000095E4 File Offset: 0x000077E4
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	public static void EarlyInit()
	{
		__JobReflectionRegistrationOutput__7274761993000005315.CreateJobReflectionData();
	}
}
