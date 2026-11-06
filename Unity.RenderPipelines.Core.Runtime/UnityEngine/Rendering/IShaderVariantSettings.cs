using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020000A9 RID: 169
	public interface IShaderVariantSettings
	{
		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600054C RID: 1356
		// (set) Token: 0x0600054D RID: 1357
		ShaderVariantLogLevel shaderVariantLogLevel { get; set; }

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x0600054E RID: 1358
		// (set) Token: 0x0600054F RID: 1359
		bool exportShaderVariants { get; set; }
	}
}
