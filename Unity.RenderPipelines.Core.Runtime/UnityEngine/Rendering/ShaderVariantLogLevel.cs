using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020000A8 RID: 168
	public enum ShaderVariantLogLevel
	{
		// Token: 0x040003C2 RID: 962
		[Tooltip("No shader variants are logged")]
		Disabled,
		// Token: 0x040003C3 RID: 963
		[Tooltip("Only shaders that are compatible with SRPs (e.g., URP, HDRP) are logged")]
		OnlySRPShaders,
		// Token: 0x040003C4 RID: 964
		[Tooltip("All shader variants are logged")]
		AllShaders
	}
}
