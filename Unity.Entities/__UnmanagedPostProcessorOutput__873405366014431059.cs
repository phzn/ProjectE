using System;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

// Token: 0x020004D9 RID: 1241
[BurstCompile]
internal class __UnmanagedPostProcessorOutput__873405366014431059
{
	// Token: 0x06001DDC RID: 7644 RVA: 0x00067E90 File Offset: 0x00066090
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	public static void EarlyInit()
	{
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(BurstCompatibleSystem), BurstRuntime.GetHashCode64<BurstCompatibleSystem>(), null, null, null, null, null, null, "Unity.Entities.BurstCompatibleSystem", 0);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(WorldUpdateAllocatorResetSystem), BurstRuntime.GetHashCode64<WorldUpdateAllocatorResetSystem>(), null, new SystemBaseRegistry.ForwardingFunc(WorldUpdateAllocatorResetSystem.__codegen__OnUpdate), null, null, null, null, "Unity.Entities.WorldUpdateAllocatorResetSystem", 0);
	}
}
