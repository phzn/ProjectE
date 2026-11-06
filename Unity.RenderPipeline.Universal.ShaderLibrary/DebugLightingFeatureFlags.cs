using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200000F RID: 15
	[GenerateHLSL(PackingRules.Exact, true, false, false, 1, false, false, false, -1, ".\\Packages\\com.unity.render-pipelines.universal\\ShaderLibrary\\Debug\\DebugViewEnums.cs")]
	[Flags]
	public enum DebugLightingFeatureFlags
	{
		// Token: 0x0400004C RID: 76
		None = 0,
		// Token: 0x0400004D RID: 77
		GlobalIllumination = 1,
		// Token: 0x0400004E RID: 78
		MainLight = 2,
		// Token: 0x0400004F RID: 79
		AdditionalLights = 4,
		// Token: 0x04000050 RID: 80
		VertexLighting = 8,
		// Token: 0x04000051 RID: 81
		Emission = 16,
		// Token: 0x04000052 RID: 82
		AmbientOcclusion = 32
	}
}
