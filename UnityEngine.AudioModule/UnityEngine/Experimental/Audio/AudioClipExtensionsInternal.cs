using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Experimental.Audio
{
	// Token: 0x02000024 RID: 36
	[NativeHeader("Modules/Audio/Public/ScriptBindings/AudioClipExtensions.bindings.h")]
	[NativeHeader("AudioScriptingClasses.h")]
	[NativeHeader("Modules/Audio/Public/AudioClip.h")]
	internal static class AudioClipExtensionsInternal
	{
		// Token: 0x0600017E RID: 382
		[NativeMethod(IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern uint Internal_CreateAudioClipSampleProvider([NotNull("NullExceptionObject")] this AudioClip audioClip, ulong start, long end, bool loop, bool allowDrop, bool loopPointIsStart = false);
	}
}
