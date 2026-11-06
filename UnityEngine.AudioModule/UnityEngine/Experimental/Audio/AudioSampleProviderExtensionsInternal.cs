using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Experimental.Audio
{
	// Token: 0x02000029 RID: 41
	[StaticAccessor("AudioSampleProviderExtensionsBindings", StaticAccessorType.DoubleColon)]
	[NativeHeader("Modules/Audio/Public/ScriptBindings/AudioSampleProviderExtensions.bindings.h")]
	internal static class AudioSampleProviderExtensionsInternal
	{
		// Token: 0x060001C6 RID: 454 RVA: 0x00003500 File Offset: 0x00001700
		public static float GetSpeed(this AudioSampleProvider provider)
		{
			return AudioSampleProviderExtensionsInternal.InternalGetAudioSampleProviderSpeed(provider.id);
		}

		// Token: 0x060001C7 RID: 455
		[NativeMethod(IsThreadSafe = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float InternalGetAudioSampleProviderSpeed(uint providerId);
	}
}
