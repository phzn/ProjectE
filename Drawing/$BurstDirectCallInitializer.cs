using System;
using Pathfinding.Drawing;
using UnityEngine;

// Token: 0x02000064 RID: 100
internal static class $BurstDirectCallInitializer
{
	// Token: 0x060002B8 RID: 696 RVA: 0x00010D53 File Offset: 0x0000EF53
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	private static void Initialize()
	{
		CommandBuilder.Initialize$JobWireMesh_WireMesh_000000D1$BurstDirectCall();
		CommandBuilder.Initialize$JobWireMesh_Execute_000000D2$BurstDirectCall();
		DrawingData.Initialize$BuilderData_AnyBuffersWrittenTo_000001EA$BurstDirectCall();
		DrawingData.Initialize$BuilderData_ResetAllBuffers_000001EB$BurstDirectCall();
	}
}
