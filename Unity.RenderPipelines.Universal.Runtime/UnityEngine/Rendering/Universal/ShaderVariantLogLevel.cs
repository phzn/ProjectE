using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200007F RID: 127
	[Obsolete("This is obsolete, UnityEngine.Rendering.ShaderVariantLogLevel instead.", false)]
	public enum ShaderVariantLogLevel
	{
		// Token: 0x0400036D RID: 877
		Disabled,
		// Token: 0x0400036E RID: 878
		[InspectorName("Only URP Shaders")]
		OnlyUniversalRPShaders,
		// Token: 0x0400036F RID: 879
		[InspectorName("All Shaders")]
		AllShaders
	}
}
