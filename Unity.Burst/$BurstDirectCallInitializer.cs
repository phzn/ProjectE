using System;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using UnityEngine;

// Token: 0x02000064 RID: 100
internal static class $BurstDirectCallInitializer
{
	// Token: 0x06000E5C RID: 3676 RVA: 0x00013C7F File Offset: 0x00011E7F
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	private static void Initialize()
	{
		BurstCompiler.Initialize$BurstCompilerHelper_IsBurstEnabled_00000145$BurstDirectCall();
		X86.DoSetCSRTrampoline_00000129$BurstDirectCall.Initialize();
		X86.DoGetCSRTrampoline_0000012A$BurstDirectCall.Initialize();
	}
}
