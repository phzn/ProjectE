using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000006 RID: 6
	[GenerateHLSL(PackingRules.Exact, true, false, false, 1, false, false, false, -1, ".\\Packages\\com.unity.render-pipelines.universal\\ShaderLibrary\\Debug\\DebugViewEnums.cs")]
	public enum DebugFullScreenMode
	{
		// Token: 0x0400001C RID: 28
		None,
		// Token: 0x0400001D RID: 29
		Depth,
		// Token: 0x0400001E RID: 30
		AdditionalLightsShadowMap,
		// Token: 0x0400001F RID: 31
		MainLightShadowMap,
		// Token: 0x04000020 RID: 32
		ReflectionProbeAtlas
	}
}
