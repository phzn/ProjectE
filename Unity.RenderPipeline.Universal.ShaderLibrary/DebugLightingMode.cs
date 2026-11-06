using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200000D RID: 13
	[GenerateHLSL(PackingRules.Exact, true, false, false, 1, false, false, false, -1, ".\\Packages\\com.unity.render-pipelines.universal\\ShaderLibrary\\Debug\\DebugViewEnums.cs")]
	public enum DebugLightingMode
	{
		// Token: 0x04000040 RID: 64
		None,
		// Token: 0x04000041 RID: 65
		ShadowCascades,
		// Token: 0x04000042 RID: 66
		LightingWithoutNormalMaps,
		// Token: 0x04000043 RID: 67
		LightingWithNormalMaps,
		// Token: 0x04000044 RID: 68
		Reflections,
		// Token: 0x04000045 RID: 69
		ReflectionsWithSmoothness
	}
}
