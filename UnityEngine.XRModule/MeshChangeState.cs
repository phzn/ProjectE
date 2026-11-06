using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.XR
{
	// Token: 0x0200002D RID: 45
	[UsedByNativeCode]
	[NativeHeader("Modules/XR/Subsystems/Meshing/XRMeshBindings.h")]
	public enum MeshChangeState
	{
		// Token: 0x0400010E RID: 270
		Added,
		// Token: 0x0400010F RID: 271
		Updated,
		// Token: 0x04000110 RID: 272
		Removed,
		// Token: 0x04000111 RID: 273
		Unchanged
	}
}
