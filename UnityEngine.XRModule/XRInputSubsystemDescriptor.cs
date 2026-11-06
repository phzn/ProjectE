using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.XR
{
	// Token: 0x02000026 RID: 38
	[UsedByNativeCode]
	[NativeType(Header = "Modules/XR/Subsystems/Input/XRInputSubsystemDescriptor.h")]
	[NativeConditional("ENABLE_XR")]
	[NativeHeader("Modules/XR/XRPrefix.h")]
	public class XRInputSubsystemDescriptor : IntegratedSubsystemDescriptor<XRInputSubsystem>
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000134 RID: 308
		[NativeConditional("ENABLE_XR")]
		public extern bool disablesLegacyInput
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
	}
}
