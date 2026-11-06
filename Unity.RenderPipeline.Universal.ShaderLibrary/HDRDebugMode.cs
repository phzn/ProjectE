using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200000E RID: 14
	[GenerateHLSL(PackingRules.Exact, true, false, false, 1, false, false, false, -1, ".\\Packages\\com.unity.render-pipelines.universal\\ShaderLibrary\\Debug\\DebugViewEnums.cs")]
	public enum HDRDebugMode
	{
		// Token: 0x04000047 RID: 71
		None,
		// Token: 0x04000048 RID: 72
		GamutView,
		// Token: 0x04000049 RID: 73
		GamutClip,
		// Token: 0x0400004A RID: 74
		ValuesAbovePaperWhite
	}
}
