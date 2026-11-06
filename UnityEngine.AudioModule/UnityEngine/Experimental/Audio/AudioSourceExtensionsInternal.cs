using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Experimental.Audio
{
	// Token: 0x0200002A RID: 42
	[NativeHeader("AudioScriptingClasses.h")]
	[NativeHeader("Modules/Audio/Public/AudioSource.h")]
	[NativeHeader("Modules/Audio/Public/ScriptBindings/AudioSourceExtensions.bindings.h")]
	internal static class AudioSourceExtensionsInternal
	{
		// Token: 0x060001C8 RID: 456 RVA: 0x0000351D File Offset: 0x0000171D
		public static void RegisterSampleProvider(this AudioSource source, AudioSampleProvider provider)
		{
			AudioSourceExtensionsInternal.Internal_RegisterSampleProviderWithAudioSource(source, provider.id);
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000352D File Offset: 0x0000172D
		public static void UnregisterSampleProvider(this AudioSource source, AudioSampleProvider provider)
		{
			AudioSourceExtensionsInternal.Internal_UnregisterSampleProviderFromAudioSource(source, provider.id);
		}

		// Token: 0x060001CA RID: 458
		[NativeMethod(IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_RegisterSampleProviderWithAudioSource([NotNull("NullExceptionObject")] AudioSource source, uint providerId);

		// Token: 0x060001CB RID: 459
		[NativeMethod(IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_UnregisterSampleProviderFromAudioSource([NotNull("NullExceptionObject")] AudioSource source, uint providerId);
	}
}
