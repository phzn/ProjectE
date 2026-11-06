using System;
using Unity.Collections;
using UnityEngine;

// Token: 0x02000156 RID: 342
internal static class $BurstDirectCallInitializer
{
	// Token: 0x06000DDC RID: 3548 RVA: 0x0002A8DB File Offset: 0x00028ADB
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	private static void Initialize()
	{
		AllocatorManager.Initialize$StackAllocator_Try_000000AB$BurstDirectCall();
		AllocatorManager.Initialize$SlabAllocator_Try_000000B9$BurstDirectCall();
		AutoFreeAllocator.Try_000000E3$BurstDirectCall.Initialize();
		RewindableAllocator.Try_000009E0$BurstDirectCall.Initialize();
		xxHash3.Hash64Long_00000A6B$BurstDirectCall.Initialize();
		xxHash3.Hash128Long_00000A72$BurstDirectCall.Initialize();
	}
}
