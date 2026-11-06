using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000BD RID: 189
	internal enum DecalSurfaceData
	{
		// Token: 0x040004D2 RID: 1234
		[Tooltip("Decals will affect only base color and emission.")]
		Albedo,
		// Token: 0x040004D3 RID: 1235
		[Tooltip("Decals will affect only base color, normal and emission.")]
		AlbedoNormal,
		// Token: 0x040004D4 RID: 1236
		[Tooltip("Decals will affect base color, normal, metallic, ambient occlusion, smoothness and emission.")]
		AlbedoNormalMAOS
	}
}
