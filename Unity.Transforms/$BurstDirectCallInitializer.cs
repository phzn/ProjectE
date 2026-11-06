using System;
using Unity.Transforms;
using UnityEngine;

// Token: 0x02000023 RID: 35
internal static class $BurstDirectCallInitializer
{
	// Token: 0x06000099 RID: 153 RVA: 0x0000435B File Offset: 0x0000255B
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	private static void Initialize()
	{
		ParentSystem.__codegen__OnCreate_0000000E$BurstDirectCall.Initialize();
		ParentSystem.__codegen__OnUpdate_0000000F$BurstDirectCall.Initialize();
		LocalToWorldSystem.__codegen__OnCreate_00000022$BurstDirectCall.Initialize();
		LocalToWorldSystem.__codegen__OnUpdate_00000023$BurstDirectCall.Initialize();
		TransformHelpers.ComputeWorldTransformMatrix_0000005E$BurstDirectCall.Initialize();
	}
}
