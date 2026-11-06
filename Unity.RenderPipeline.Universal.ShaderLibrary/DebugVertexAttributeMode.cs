using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000004 RID: 4
	[GenerateHLSL(PackingRules.Exact, true, false, false, 1, false, false, false, -1, ".\\Packages\\com.unity.render-pipelines.universal\\ShaderLibrary\\Debug\\DebugViewEnums.cs")]
	public enum DebugVertexAttributeMode
	{
		// Token: 0x0400000F RID: 15
		None,
		// Token: 0x04000010 RID: 16
		Texcoord0,
		// Token: 0x04000011 RID: 17
		Texcoord1,
		// Token: 0x04000012 RID: 18
		Texcoord2,
		// Token: 0x04000013 RID: 19
		Texcoord3,
		// Token: 0x04000014 RID: 20
		Color,
		// Token: 0x04000015 RID: 21
		Tangent,
		// Token: 0x04000016 RID: 22
		Normal
	}
}
