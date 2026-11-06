using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Playables;

namespace UnityEngine.Audio
{
	// Token: 0x02000033 RID: 51
	[NativeHeader("Runtime/Director/Core/HPlayableOutput.h")]
	[StaticAccessor("AudioPlayableGraphExtensionsBindings", StaticAccessorType.DoubleColon)]
	[NativeHeader("Modules/Audio/Public/ScriptBindings/AudioPlayableGraphExtensions.bindings.h")]
	internal static class AudioPlayableGraphExtensions
	{
		// Token: 0x06000215 RID: 533
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool InternalCreateAudioOutput(ref PlayableGraph graph, string name, out PlayableOutputHandle handle);
	}
}
