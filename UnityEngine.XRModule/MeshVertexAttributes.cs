using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.XR
{
	// Token: 0x0200002B RID: 43
	[Flags]
	[UsedByNativeCode]
	[NativeHeader("Modules/XR/Subsystems/Meshing/XRMeshBindings.h")]
	public enum MeshVertexAttributes
	{
		// Token: 0x04000105 RID: 261
		None = 0,
		// Token: 0x04000106 RID: 262
		Normals = 1,
		// Token: 0x04000107 RID: 263
		Tangents = 2,
		// Token: 0x04000108 RID: 264
		UVs = 4,
		// Token: 0x04000109 RID: 265
		Colors = 8
	}
}
