using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.XR
{
	// Token: 0x02000023 RID: 35
	[NativeType(Header = "Modules/XR/Subsystems/Display/XRDisplaySubsystemDescriptor.h")]
	[UsedByNativeCode]
	public class XRDisplaySubsystemDescriptor : IntegratedSubsystemDescriptor<XRDisplaySubsystem>
	{
		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600011F RID: 287
		[NativeConditional("ENABLE_XR")]
		public extern bool disablesLegacyVr
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000120 RID: 288
		[NativeConditional("ENABLE_XR")]
		public extern bool enableBackBufferMSAA
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06000121 RID: 289
		[NativeConditional("ENABLE_XR")]
		[NativeMethod("TryGetAvailableMirrorModeCount")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetAvailableMirrorBlitModeCount();

		// Token: 0x06000122 RID: 290
		[NativeConditional("ENABLE_XR")]
		[NativeMethod("TryGetMirrorModeByIndex")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void GetMirrorBlitModeByIndex(int index, out XRMirrorViewBlitModeDesc mode);
	}
}
