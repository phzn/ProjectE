using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000BF RID: 191
	internal enum DecalTechniqueOption
	{
		// Token: 0x040004DB RID: 1243
		[Tooltip("Automatically selects technique based on build platform.")]
		Automatic,
		// Token: 0x040004DC RID: 1244
		[Tooltip("Renders decals into DBuffer and then applied during opaque rendering. Requires DepthNormal prepass which makes not viable solution for the tile based renderers common on mobile.")]
		[InspectorName("DBuffer")]
		DBuffer,
		// Token: 0x040004DD RID: 1245
		[Tooltip("Renders decals after opaque objects with normal reconstructed from depth. The decals are simply rendered as mesh on top of opaque ones, as result does not support blending per single surface data (etc. normal blending only).")]
		ScreenSpace
	}
}
