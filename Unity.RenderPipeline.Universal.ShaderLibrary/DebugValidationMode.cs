using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200000B RID: 11
	[GenerateHLSL(PackingRules.Exact, true, false, false, 1, false, false, false, -1, ".\\Packages\\com.unity.render-pipelines.universal\\ShaderLibrary\\Debug\\DebugViewEnums.cs")]
	public enum DebugValidationMode
	{
		// Token: 0x04000036 RID: 54
		None,
		// Token: 0x04000037 RID: 55
		[InspectorName("Highlight NaN, Inf and Negative Values")]
		HighlightNanInfNegative,
		// Token: 0x04000038 RID: 56
		[InspectorName("Highlight Values Outside Range")]
		HighlightOutsideOfRange
	}
}
