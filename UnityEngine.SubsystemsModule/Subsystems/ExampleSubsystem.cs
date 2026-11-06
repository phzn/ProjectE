using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Subsystems
{
	// Token: 0x0200001B RID: 27
	[NativeType(Header = "Modules/Subsystems/Example/ExampleSubsystem.h")]
	[UsedByNativeCode]
	public class ExampleSubsystem : IntegratedSubsystem<ExampleSubsystemDescriptor>
	{
		// Token: 0x0600008F RID: 143
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void PrintExample();

		// Token: 0x06000090 RID: 144
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool GetBool();
	}
}
