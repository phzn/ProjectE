using System;
using Unity.Jobs;
using Unity.Serialization.Json;
using UnityEngine;

// Token: 0x020000AC RID: 172
[DOTSCompilerGenerated]
internal class __JobReflectionRegistrationOutput__14542720655017034712
{
	// Token: 0x06000449 RID: 1097 RVA: 0x00011668 File Offset: 0x0000F868
	public static void CreateJobReflectionData()
	{
		try
		{
			IJobExtensions.EarlyJobInit<JsonSerialization.ReadJob>();
		}
		catch (Exception ex)
		{
			EarlyInitHelpers.JobReflectionDataCreationFailed(ex);
		}
	}

	// Token: 0x0600044A RID: 1098 RVA: 0x0001169C File Offset: 0x0000F89C
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	public static void EarlyInit()
	{
		__JobReflectionRegistrationOutput__14542720655017034712.CreateJobReflectionData();
	}
}
