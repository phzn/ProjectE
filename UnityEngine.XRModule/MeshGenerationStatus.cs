using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.XR
{
	// Token: 0x02000028 RID: 40
	[NativeHeader("Modules/XR/Subsystems/Meshing/XRMeshBindings.h")]
	[RequiredByNativeCode]
	public enum MeshGenerationStatus
	{
		// Token: 0x040000F5 RID: 245
		Success,
		// Token: 0x040000F6 RID: 246
		InvalidMeshId,
		// Token: 0x040000F7 RID: 247
		GenerationAlreadyInProgress,
		// Token: 0x040000F8 RID: 248
		Canceled,
		// Token: 0x040000F9 RID: 249
		UnknownError
	}
}
