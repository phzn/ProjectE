using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x0200001D RID: 29
	[StaticAccessor("GetAudioManager()", StaticAccessorType.Dot)]
	public sealed class Microphone
	{
		// Token: 0x06000130 RID: 304
		[NativeMethod(IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetMicrophoneDeviceIDFromName(string name);

		// Token: 0x06000131 RID: 305
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern AudioClip StartRecord(int deviceID, bool loop, float lengthSec, int frequency);

		// Token: 0x06000132 RID: 306
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void EndRecord(int deviceID);

		// Token: 0x06000133 RID: 307
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsRecording(int deviceID);

		// Token: 0x06000134 RID: 308
		[NativeMethod(IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetRecordPosition(int deviceID);

		// Token: 0x06000135 RID: 309
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetDeviceCaps(int deviceID, out int minFreq, out int maxFreq);

		// Token: 0x06000136 RID: 310 RVA: 0x00002B18 File Offset: 0x00000D18
		public static AudioClip Start(string deviceName, bool loop, int lengthSec, int frequency)
		{
			int microphoneDeviceIDFromName = Microphone.GetMicrophoneDeviceIDFromName(deviceName);
			bool flag = microphoneDeviceIDFromName == -1;
			if (flag)
			{
				throw new ArgumentException("Couldn't acquire device ID for device name " + deviceName);
			}
			bool flag2 = lengthSec <= 0;
			if (flag2)
			{
				throw new ArgumentException("Length of recording must be greater than zero seconds (was: " + lengthSec.ToString() + " seconds)");
			}
			bool flag3 = lengthSec > 3600;
			if (flag3)
			{
				throw new ArgumentException("Length of recording must be less than one hour (was: " + lengthSec.ToString() + " seconds)");
			}
			bool flag4 = frequency <= 0;
			if (flag4)
			{
				throw new ArgumentException("Frequency of recording must be greater than zero (was: " + frequency.ToString() + " Hz)");
			}
			return Microphone.StartRecord(microphoneDeviceIDFromName, loop, (float)lengthSec, frequency);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00002BD0 File Offset: 0x00000DD0
		public static void End(string deviceName)
		{
			int microphoneDeviceIDFromName = Microphone.GetMicrophoneDeviceIDFromName(deviceName);
			bool flag = microphoneDeviceIDFromName == -1;
			if (!flag)
			{
				Microphone.EndRecord(microphoneDeviceIDFromName);
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000138 RID: 312
		public static extern string[] devices
		{
			[NativeName("GetRecordDevices")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000139 RID: 313
		internal static extern bool isAnyDeviceRecording
		{
			[NativeName("IsAnyRecordDeviceActive")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00002BF8 File Offset: 0x00000DF8
		public static bool IsRecording(string deviceName)
		{
			int microphoneDeviceIDFromName = Microphone.GetMicrophoneDeviceIDFromName(deviceName);
			bool flag = microphoneDeviceIDFromName == -1;
			return !flag && Microphone.IsRecording(microphoneDeviceIDFromName);
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00002C24 File Offset: 0x00000E24
		public static int GetPosition(string deviceName)
		{
			int microphoneDeviceIDFromName = Microphone.GetMicrophoneDeviceIDFromName(deviceName);
			bool flag = microphoneDeviceIDFromName == -1;
			int num;
			if (flag)
			{
				num = 0;
			}
			else
			{
				num = Microphone.GetRecordPosition(microphoneDeviceIDFromName);
			}
			return num;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00002C50 File Offset: 0x00000E50
		public static void GetDeviceCaps(string deviceName, out int minFreq, out int maxFreq)
		{
			minFreq = 0;
			maxFreq = 0;
			int microphoneDeviceIDFromName = Microphone.GetMicrophoneDeviceIDFromName(deviceName);
			bool flag = microphoneDeviceIDFromName == -1;
			if (!flag)
			{
				Microphone.GetDeviceCaps(microphoneDeviceIDFromName, out minFreq, out maxFreq);
			}
		}
	}
}
