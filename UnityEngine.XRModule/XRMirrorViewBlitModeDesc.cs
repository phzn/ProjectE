using System;
using UnityEngine.Bindings;

namespace UnityEngine.XR
{
	// Token: 0x02000022 RID: 34
	[NativeType(Header = "Modules/XR/Subsystems/Display/XRDisplaySubsystemDescriptor.h")]
	[NativeHeader("Modules/XR/XRPrefix.h")]
	public struct XRMirrorViewBlitModeDesc
	{
		// Token: 0x040000E6 RID: 230
		public int blitMode;

		// Token: 0x040000E7 RID: 231
		public string blitModeDesc;
	}
}
