using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Audio;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x0200001E RID: 30
	[NativeType(Header = "Modules/Audio/Public/ScriptBindings/AudioRenderer.bindings.h")]
	public class AudioRenderer
	{
		// Token: 0x0600013E RID: 318 RVA: 0x00002C80 File Offset: 0x00000E80
		public static bool Start()
		{
			return AudioRenderer.Internal_AudioRenderer_Start();
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00002C98 File Offset: 0x00000E98
		public static bool Stop()
		{
			return AudioRenderer.Internal_AudioRenderer_Stop();
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00002CB0 File Offset: 0x00000EB0
		public static int GetSampleCountForCaptureFrame()
		{
			return AudioRenderer.Internal_AudioRenderer_GetSampleCountForCaptureFrame();
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00002CC8 File Offset: 0x00000EC8
		internal static bool AddMixerGroupSink(AudioMixerGroup mixerGroup, NativeArray<float> buffer, bool excludeFromMix)
		{
			return AudioRenderer.Internal_AudioRenderer_AddMixerGroupSink(mixerGroup, buffer.GetUnsafePtr<float>(), buffer.Length, excludeFromMix);
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00002CF0 File Offset: 0x00000EF0
		public static bool Render(NativeArray<float> buffer)
		{
			return AudioRenderer.Internal_AudioRenderer_Render(buffer.GetUnsafePtr<float>(), buffer.Length);
		}

		// Token: 0x06000143 RID: 323
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool Internal_AudioRenderer_Start();

		// Token: 0x06000144 RID: 324
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool Internal_AudioRenderer_Stop();

		// Token: 0x06000145 RID: 325
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int Internal_AudioRenderer_GetSampleCountForCaptureFrame();

		// Token: 0x06000146 RID: 326
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern bool Internal_AudioRenderer_AddMixerGroupSink(AudioMixerGroup mixerGroup, void* ptr, int length, bool excludeFromMix);

		// Token: 0x06000147 RID: 327
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern bool Internal_AudioRenderer_Render(void* ptr, int length);
	}
}
