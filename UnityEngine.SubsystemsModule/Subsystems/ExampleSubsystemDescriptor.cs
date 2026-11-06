using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Subsystems
{
	// Token: 0x0200001C RID: 28
	[NativeType(Header = "Modules/Subsystems/Example/ExampleSubsystemDescriptor.h")]
	[UsedByNativeCode]
	public class ExampleSubsystemDescriptor : IntegratedSubsystemDescriptor<ExampleSubsystem>
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000092 RID: 146
		public extern bool supportsEditorMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000093 RID: 147
		public extern bool disableBackbufferMSAA
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000094 RID: 148
		public extern bool stereoscopicBackbuffer
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000095 RID: 149
		public extern bool usePBufferEGL
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
	}
}
