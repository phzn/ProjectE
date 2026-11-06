using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000014 RID: 20
	[StaticAccessor("AudioListenerBindings", StaticAccessorType.DoubleColon)]
	[RequireComponent(typeof(Transform))]
	public sealed class AudioListener : AudioBehaviour
	{
		// Token: 0x06000056 RID: 86
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetOutputDataHelper([Out] float[] samples, int channel);

		// Token: 0x06000057 RID: 87
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetSpectrumDataHelper([Out] float[] samples, int channel, FFTWindow window);

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000058 RID: 88
		// (set) Token: 0x06000059 RID: 89
		public static extern float volume
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600005A RID: 90
		// (set) Token: 0x0600005B RID: 91
		[NativeProperty("ListenerPause")]
		public static extern bool pause
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600005C RID: 92
		// (set) Token: 0x0600005D RID: 93
		public extern AudioVelocityUpdateMode velocityUpdateMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x0000278C File Offset: 0x0000098C
		[Obsolete("GetOutputData returning a float[] is deprecated, use GetOutputData and pass a pre allocated array instead.")]
		public static float[] GetOutputData(int numSamples, int channel)
		{
			float[] array = new float[numSamples];
			AudioListener.GetOutputDataHelper(array, channel);
			return array;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000027AE File Offset: 0x000009AE
		public static void GetOutputData(float[] samples, int channel)
		{
			AudioListener.GetOutputDataHelper(samples, channel);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000027BC File Offset: 0x000009BC
		[Obsolete("GetSpectrumData returning a float[] is deprecated, use GetSpectrumData and pass a pre allocated array instead.")]
		public static float[] GetSpectrumData(int numSamples, int channel, FFTWindow window)
		{
			float[] array = new float[numSamples];
			AudioListener.GetSpectrumDataHelper(array, channel, window);
			return array;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000027DF File Offset: 0x000009DF
		public static void GetSpectrumData(float[] samples, int channel, FFTWindow window)
		{
			AudioListener.GetSpectrumDataHelper(samples, channel, window);
		}
	}
}
