using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Audio;
using UnityEngine.Bindings;
using UnityEngine.Internal;

namespace UnityEngine
{
	// Token: 0x02000015 RID: 21
	[StaticAccessor("AudioSourceBindings", StaticAccessorType.DoubleColon)]
	[RequireComponent(typeof(Transform))]
	public sealed class AudioSource : AudioBehaviour
	{
		// Token: 0x06000063 RID: 99
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetPitch([NotNull("ArgumentNullException")] AudioSource source);

		// Token: 0x06000064 RID: 100
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetPitch([NotNull("ArgumentNullException")] AudioSource source, float pitch);

		// Token: 0x06000065 RID: 101
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void PlayHelper([NotNull("ArgumentNullException")] AudioSource source, ulong delay);

		// Token: 0x06000066 RID: 102
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Play(double delay);

		// Token: 0x06000067 RID: 103
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void PlayOneShotHelper([NotNull("ArgumentNullException")] AudioSource source, [NotNull("NullExceptionObject")] AudioClip clip, float volumeScale);

		// Token: 0x06000068 RID: 104
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Stop(bool stopOneShots);

		// Token: 0x06000069 RID: 105
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetCustomCurveHelper([NotNull("ArgumentNullException")] AudioSource source, AudioSourceCurveType type, AnimationCurve curve);

		// Token: 0x0600006A RID: 106
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern AnimationCurve GetCustomCurveHelper([NotNull("ArgumentNullException")] AudioSource source, AudioSourceCurveType type);

		// Token: 0x0600006B RID: 107
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetOutputDataHelper([NotNull("ArgumentNullException")] AudioSource source, [Out] float[] samples, int channel);

		// Token: 0x0600006C RID: 108
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetSpectrumDataHelper([NotNull("ArgumentNullException")] AudioSource source, [Out] float[] samples, int channel, FFTWindow window);

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600006D RID: 109
		// (set) Token: 0x0600006E RID: 110
		public extern float volume
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600006F RID: 111 RVA: 0x000027F4 File Offset: 0x000009F4
		// (set) Token: 0x06000070 RID: 112 RVA: 0x0000280C File Offset: 0x00000A0C
		public float pitch
		{
			get
			{
				return AudioSource.GetPitch(this);
			}
			set
			{
				AudioSource.SetPitch(this, value);
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000071 RID: 113
		// (set) Token: 0x06000072 RID: 114
		[NativeProperty("SecPosition")]
		public extern float time
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000073 RID: 115
		// (set) Token: 0x06000074 RID: 116
		[NativeProperty("SamplePosition")]
		public extern int timeSamples
		{
			[NativeMethod(IsThreadSafe = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[NativeMethod(IsThreadSafe = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000075 RID: 117
		// (set) Token: 0x06000076 RID: 118
		[NativeProperty("AudioClip")]
		public extern AudioClip clip
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000077 RID: 119
		// (set) Token: 0x06000078 RID: 120
		public extern AudioMixerGroup outputAudioMixerGroup
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00002817 File Offset: 0x00000A17
		[ExcludeFromDocs]
		public void Play()
		{
			AudioSource.PlayHelper(this, 0UL);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00002823 File Offset: 0x00000A23
		public void Play([DefaultValue("0")] ulong delay)
		{
			AudioSource.PlayHelper(this, delay);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x0000282E File Offset: 0x00000A2E
		public void PlayDelayed(float delay)
		{
			this.Play((delay < 0f) ? 0.0 : (-(double)delay));
		}

		// Token: 0x0600007C RID: 124 RVA: 0x0000284E File Offset: 0x00000A4E
		public void PlayScheduled(double time)
		{
			this.Play((time < 0.0) ? 0.0 : time);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00002870 File Offset: 0x00000A70
		[ExcludeFromDocs]
		public void PlayOneShot(AudioClip clip)
		{
			this.PlayOneShot(clip, 1f);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00002880 File Offset: 0x00000A80
		public void PlayOneShot(AudioClip clip, [DefaultValue("1.0F")] float volumeScale)
		{
			bool flag = clip == null;
			if (flag)
			{
				Debug.LogWarning("PlayOneShot was called with a null AudioClip.");
			}
			else
			{
				AudioSource.PlayOneShotHelper(this, clip, volumeScale);
			}
		}

		// Token: 0x0600007F RID: 127
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetScheduledStartTime(double time);

		// Token: 0x06000080 RID: 128
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetScheduledEndTime(double time);

		// Token: 0x06000081 RID: 129 RVA: 0x000028B0 File Offset: 0x00000AB0
		public void Stop()
		{
			this.Stop(true);
		}

		// Token: 0x06000082 RID: 130
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Pause();

		// Token: 0x06000083 RID: 131
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void UnPause();

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000084 RID: 132
		public extern bool isPlaying
		{
			[NativeName("IsPlayingScripting")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000085 RID: 133
		public extern bool isVirtual
		{
			[NativeName("GetLastVirtualState")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000028BB File Offset: 0x00000ABB
		[ExcludeFromDocs]
		public static void PlayClipAtPoint(AudioClip clip, Vector3 position)
		{
			AudioSource.PlayClipAtPoint(clip, position, 1f);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000028CC File Offset: 0x00000ACC
		public static void PlayClipAtPoint(AudioClip clip, Vector3 position, [DefaultValue("1.0F")] float volume)
		{
			GameObject gameObject = new GameObject("One shot audio");
			gameObject.transform.position = position;
			AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
			audioSource.clip = clip;
			audioSource.spatialBlend = 1f;
			audioSource.volume = volume;
			audioSource.Play();
			Object.Destroy(gameObject, clip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000088 RID: 136
		// (set) Token: 0x06000089 RID: 137
		public extern bool loop
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600008A RID: 138
		// (set) Token: 0x0600008B RID: 139
		public extern bool ignoreListenerVolume
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600008C RID: 140
		// (set) Token: 0x0600008D RID: 141
		public extern bool playOnAwake
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600008E RID: 142
		// (set) Token: 0x0600008F RID: 143
		public extern bool ignoreListenerPause
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000090 RID: 144
		// (set) Token: 0x06000091 RID: 145
		public extern AudioVelocityUpdateMode velocityUpdateMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000092 RID: 146
		// (set) Token: 0x06000093 RID: 147
		[NativeProperty("StereoPan")]
		public extern float panStereo
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000094 RID: 148
		// (set) Token: 0x06000095 RID: 149
		[NativeProperty("SpatialBlendMix")]
		public extern float spatialBlend
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000096 RID: 150
		// (set) Token: 0x06000097 RID: 151
		public extern bool spatialize
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000098 RID: 152
		// (set) Token: 0x06000099 RID: 153
		public extern bool spatializePostEffects
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00002951 File Offset: 0x00000B51
		public void SetCustomCurve(AudioSourceCurveType type, AnimationCurve curve)
		{
			AudioSource.SetCustomCurveHelper(this, type, curve);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00002960 File Offset: 0x00000B60
		public AnimationCurve GetCustomCurve(AudioSourceCurveType type)
		{
			return AudioSource.GetCustomCurveHelper(this, type);
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600009C RID: 156
		// (set) Token: 0x0600009D RID: 157
		public extern float reverbZoneMix
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600009E RID: 158
		// (set) Token: 0x0600009F RID: 159
		public extern bool bypassEffects
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000A0 RID: 160
		// (set) Token: 0x060000A1 RID: 161
		public extern bool bypassListenerEffects
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000A2 RID: 162
		// (set) Token: 0x060000A3 RID: 163
		public extern bool bypassReverbZones
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000A4 RID: 164
		// (set) Token: 0x060000A5 RID: 165
		public extern float dopplerLevel
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000A6 RID: 166
		// (set) Token: 0x060000A7 RID: 167
		public extern float spread
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000A8 RID: 168
		// (set) Token: 0x060000A9 RID: 169
		public extern int priority
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000AA RID: 170
		// (set) Token: 0x060000AB RID: 171
		public extern bool mute
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000AC RID: 172
		// (set) Token: 0x060000AD RID: 173
		public extern float minDistance
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000AE RID: 174
		// (set) Token: 0x060000AF RID: 175
		public extern float maxDistance
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000B0 RID: 176
		// (set) Token: 0x060000B1 RID: 177
		public extern AudioRolloffMode rolloffMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x0000297C File Offset: 0x00000B7C
		[Obsolete("GetOutputData returning a float[] is deprecated, use GetOutputData and pass a pre allocated array instead.")]
		public float[] GetOutputData(int numSamples, int channel)
		{
			float[] array = new float[numSamples];
			AudioSource.GetOutputDataHelper(this, array, channel);
			return array;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x0000299F File Offset: 0x00000B9F
		public void GetOutputData(float[] samples, int channel)
		{
			AudioSource.GetOutputDataHelper(this, samples, channel);
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x000029AC File Offset: 0x00000BAC
		[Obsolete("GetSpectrumData returning a float[] is deprecated, use GetSpectrumData and pass a pre allocated array instead.")]
		public float[] GetSpectrumData(int numSamples, int channel, FFTWindow window)
		{
			float[] array = new float[numSamples];
			AudioSource.GetSpectrumDataHelper(this, array, channel, window);
			return array;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000029D0 File Offset: 0x00000BD0
		public void GetSpectrumData(float[] samples, int channel, FFTWindow window)
		{
			AudioSource.GetSpectrumDataHelper(this, samples, channel, window);
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x000029E0 File Offset: 0x00000BE0
		// (set) Token: 0x060000B7 RID: 183 RVA: 0x00002A02 File Offset: 0x00000C02
		[Obsolete("minVolume is not supported anymore. Use min-, maxDistance and rolloffMode instead.", true)]
		public float minVolume
		{
			get
			{
				Debug.LogError("minVolume is not supported anymore. Use min-, maxDistance and rolloffMode instead.");
				return 0f;
			}
			set
			{
				Debug.LogError("minVolume is not supported anymore. Use min-, maxDistance and rolloffMode instead.");
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x00002A10 File Offset: 0x00000C10
		// (set) Token: 0x060000B9 RID: 185 RVA: 0x00002A32 File Offset: 0x00000C32
		[Obsolete("maxVolume is not supported anymore. Use min-, maxDistance and rolloffMode instead.", true)]
		public float maxVolume
		{
			get
			{
				Debug.LogError("maxVolume is not supported anymore. Use min-, maxDistance and rolloffMode instead.");
				return 0f;
			}
			set
			{
				Debug.LogError("maxVolume is not supported anymore. Use min-, maxDistance and rolloffMode instead.");
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000BA RID: 186 RVA: 0x00002A40 File Offset: 0x00000C40
		// (set) Token: 0x060000BB RID: 187 RVA: 0x00002A62 File Offset: 0x00000C62
		[Obsolete("rolloffFactor is not supported anymore. Use min-, maxDistance and rolloffMode instead.", true)]
		public float rolloffFactor
		{
			get
			{
				Debug.LogError("rolloffFactor is not supported anymore. Use min-, maxDistance and rolloffMode instead.");
				return 0f;
			}
			set
			{
				Debug.LogError("rolloffFactor is not supported anymore. Use min-, maxDistance and rolloffMode instead.");
			}
		}

		// Token: 0x060000BC RID: 188
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool SetSpatializerFloat(int index, float value);

		// Token: 0x060000BD RID: 189
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool GetSpatializerFloat(int index, out float value);

		// Token: 0x060000BE RID: 190
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool GetAmbisonicDecoderFloat(int index, out float value);

		// Token: 0x060000BF RID: 191
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool SetAmbisonicDecoderFloat(int index, float value);
	}
}
