using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000010 RID: 16
	[StaticAccessor("AudioClipBindings", StaticAccessorType.DoubleColon)]
	[NativeHeader("Modules/Audio/Public/ScriptBindings/Audio.bindings.h")]
	public sealed class AudioClip : Object
	{
		// Token: 0x0600002D RID: 45 RVA: 0x000023FB File Offset: 0x000005FB
		private AudioClip()
		{
		}

		// Token: 0x0600002E RID: 46
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetData([NotNull("NullExceptionObject")] AudioClip clip, [Out] float[] data, int numSamples, int samplesOffset);

		// Token: 0x0600002F RID: 47
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SetData([NotNull("NullExceptionObject")] AudioClip clip, float[] data, int numsamples, int samplesOffset);

		// Token: 0x06000030 RID: 48
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern AudioClip Construct_Internal();

		// Token: 0x06000031 RID: 49
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern string GetName();

		// Token: 0x06000032 RID: 50
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void CreateUserSound(string name, int lengthSamples, int channels, int frequency, bool stream);

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000033 RID: 51
		[NativeProperty("LengthSec")]
		public extern float length
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000034 RID: 52
		[NativeProperty("SampleCount")]
		public extern int samples
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000035 RID: 53
		[NativeProperty("ChannelCount")]
		public extern int channels
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000036 RID: 54
		public extern int frequency
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000037 RID: 55
		[Obsolete("Use AudioClip.loadState instead to get more detailed information about the loading process.")]
		public extern bool isReadyToPlay
		{
			[NativeName("ReadyToPlay")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000038 RID: 56
		public extern AudioClipLoadType loadType
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06000039 RID: 57
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool LoadAudioData();

		// Token: 0x0600003A RID: 58
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool UnloadAudioData();

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600003B RID: 59
		public extern bool preloadAudioData
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600003C RID: 60
		public extern bool ambisonic
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600003D RID: 61
		public extern bool loadInBackground
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600003E RID: 62
		public extern AudioDataLoadState loadState
		{
			[NativeMethod(Name = "AudioClipBindings::GetLoadState", HasExplicitThis = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002414 File Offset: 0x00000614
		public bool GetData(float[] data, int offsetSamples)
		{
			bool flag = this.channels <= 0;
			bool flag2;
			if (flag)
			{
				Debug.Log("AudioClip.GetData failed; AudioClip " + this.GetName() + " contains no data");
				flag2 = false;
			}
			else
			{
				int num = ((data != null) ? (data.Length / this.channels) : 0);
				flag2 = AudioClip.GetData(this, data, num, offsetSamples);
			}
			return flag2;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002470 File Offset: 0x00000670
		public bool SetData(float[] data, int offsetSamples)
		{
			bool flag = this.channels <= 0;
			bool flag2;
			if (flag)
			{
				Debug.Log("AudioClip.SetData failed; AudioClip " + this.GetName() + " contains no data");
				flag2 = false;
			}
			else
			{
				bool flag3 = offsetSamples < 0 || offsetSamples >= this.samples;
				if (flag3)
				{
					throw new ArgumentException("AudioClip.SetData failed; invalid offsetSamples");
				}
				bool flag4 = data == null || data.Length == 0;
				if (flag4)
				{
					throw new ArgumentException("AudioClip.SetData failed; invalid data");
				}
				flag2 = AudioClip.SetData(this, data, data.Length / this.channels, offsetSamples);
			}
			return flag2;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002500 File Offset: 0x00000700
		[Obsolete("The _3D argument of AudioClip is deprecated. Use the spatialBlend property of AudioSource instead to morph between 2D and 3D playback.")]
		public static AudioClip Create(string name, int lengthSamples, int channels, int frequency, bool _3D, bool stream)
		{
			return AudioClip.Create(name, lengthSamples, channels, frequency, stream);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002520 File Offset: 0x00000720
		[Obsolete("The _3D argument of AudioClip is deprecated. Use the spatialBlend property of AudioSource instead to morph between 2D and 3D playback.")]
		public static AudioClip Create(string name, int lengthSamples, int channels, int frequency, bool _3D, bool stream, AudioClip.PCMReaderCallback pcmreadercallback)
		{
			return AudioClip.Create(name, lengthSamples, channels, frequency, stream, pcmreadercallback, null);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002540 File Offset: 0x00000740
		[Obsolete("The _3D argument of AudioClip is deprecated. Use the spatialBlend property of AudioSource instead to morph between 2D and 3D playback.")]
		public static AudioClip Create(string name, int lengthSamples, int channels, int frequency, bool _3D, bool stream, AudioClip.PCMReaderCallback pcmreadercallback, AudioClip.PCMSetPositionCallback pcmsetpositioncallback)
		{
			return AudioClip.Create(name, lengthSamples, channels, frequency, stream, pcmreadercallback, pcmsetpositioncallback);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002564 File Offset: 0x00000764
		public static AudioClip Create(string name, int lengthSamples, int channels, int frequency, bool stream)
		{
			return AudioClip.Create(name, lengthSamples, channels, frequency, stream, null, null);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002588 File Offset: 0x00000788
		public static AudioClip Create(string name, int lengthSamples, int channels, int frequency, bool stream, AudioClip.PCMReaderCallback pcmreadercallback)
		{
			return AudioClip.Create(name, lengthSamples, channels, frequency, stream, pcmreadercallback, null);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000025AC File Offset: 0x000007AC
		public static AudioClip Create(string name, int lengthSamples, int channels, int frequency, bool stream, AudioClip.PCMReaderCallback pcmreadercallback, AudioClip.PCMSetPositionCallback pcmsetpositioncallback)
		{
			bool flag = name == null;
			if (flag)
			{
				throw new NullReferenceException();
			}
			bool flag2 = lengthSamples <= 0;
			if (flag2)
			{
				throw new ArgumentException("Length of created clip must be larger than 0");
			}
			bool flag3 = channels <= 0;
			if (flag3)
			{
				throw new ArgumentException("Number of channels in created clip must be greater than 0");
			}
			bool flag4 = frequency <= 0;
			if (flag4)
			{
				throw new ArgumentException("Frequency in created clip must be greater than 0");
			}
			AudioClip audioClip = AudioClip.Construct_Internal();
			bool flag5 = pcmreadercallback != null;
			if (flag5)
			{
				audioClip.m_PCMReaderCallback += pcmreadercallback;
			}
			bool flag6 = pcmsetpositioncallback != null;
			if (flag6)
			{
				audioClip.m_PCMSetPositionCallback += pcmsetpositioncallback;
			}
			audioClip.CreateUserSound(name, lengthSamples, channels, frequency, stream);
			return audioClip;
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000047 RID: 71 RVA: 0x00002650 File Offset: 0x00000850
		// (remove) Token: 0x06000048 RID: 72 RVA: 0x00002688 File Offset: 0x00000888
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private event AudioClip.PCMReaderCallback m_PCMReaderCallback = null;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000049 RID: 73 RVA: 0x000026C0 File Offset: 0x000008C0
		// (remove) Token: 0x0600004A RID: 74 RVA: 0x000026F8 File Offset: 0x000008F8
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private event AudioClip.PCMSetPositionCallback m_PCMSetPositionCallback = null;

		// Token: 0x0600004B RID: 75 RVA: 0x00002730 File Offset: 0x00000930
		[RequiredByNativeCode]
		private void InvokePCMReaderCallback_Internal(float[] data)
		{
			bool flag = this.m_PCMReaderCallback != null;
			if (flag)
			{
				this.m_PCMReaderCallback(data);
			}
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002758 File Offset: 0x00000958
		[RequiredByNativeCode]
		private void InvokePCMSetPositionCallback_Internal(int position)
		{
			bool flag = this.m_PCMSetPositionCallback != null;
			if (flag)
			{
				this.m_PCMSetPositionCallback(position);
			}
		}

		// Token: 0x02000011 RID: 17
		// (Invoke) Token: 0x0600004E RID: 78
		public delegate void PCMReaderCallback(float[] data);

		// Token: 0x02000012 RID: 18
		// (Invoke) Token: 0x06000052 RID: 82
		public delegate void PCMSetPositionCallback(int position);
	}
}
