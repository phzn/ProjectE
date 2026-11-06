using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000003 RID: 3
	[GenerateHLSL(PackingRules.Exact, true, false, false, 1, false, false, false, -1, ".\\Packages\\com.unity.render-pipelines.universal\\ShaderLibrary\\Debug\\DebugViewEnums.cs")]
	public enum DebugMaterialMode
	{
		// Token: 0x04000002 RID: 2
		None,
		// Token: 0x04000003 RID: 3
		Albedo,
		// Token: 0x04000004 RID: 4
		Specular,
		// Token: 0x04000005 RID: 5
		Alpha,
		// Token: 0x04000006 RID: 6
		Smoothness,
		// Token: 0x04000007 RID: 7
		AmbientOcclusion,
		// Token: 0x04000008 RID: 8
		Emission,
		// Token: 0x04000009 RID: 9
		NormalWorldSpace,
		// Token: 0x0400000A RID: 10
		NormalTangentSpace,
		// Token: 0x0400000B RID: 11
		LightingComplexity,
		// Token: 0x0400000C RID: 12
		Metallic,
		// Token: 0x0400000D RID: 13
		SpriteMask
	}
}
