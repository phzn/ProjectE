using System;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

// Token: 0x02000021 RID: 33
[BurstCompile]
internal class __UnmanagedPostProcessorOutput__17410405008362739185
{
	// Token: 0x06000096 RID: 150 RVA: 0x0000426C File Offset: 0x0000246C
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	public static void EarlyInit()
	{
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(ParentSystem), BurstRuntime.GetHashCode64<ParentSystem>(), new SystemBaseRegistry.ForwardingFunc(ParentSystem.__codegen__OnCreate), new SystemBaseRegistry.ForwardingFunc(ParentSystem.__codegen__OnUpdate), null, null, null, new SystemBaseRegistry.ForwardingFunc(ParentSystem.__codegen__OnCreateForCompiler), "Unity.Transforms.ParentSystem", 3);
		SystemBaseRegistry.AddUnmanagedSystemType(typeof(LocalToWorldSystem), BurstRuntime.GetHashCode64<LocalToWorldSystem>(), new SystemBaseRegistry.ForwardingFunc(LocalToWorldSystem.__codegen__OnCreate), new SystemBaseRegistry.ForwardingFunc(LocalToWorldSystem.__codegen__OnUpdate), null, null, null, new SystemBaseRegistry.ForwardingFunc(LocalToWorldSystem.__codegen__OnCreateForCompiler), "Unity.Transforms.LocalToWorldSystem", 3);
	}
}
