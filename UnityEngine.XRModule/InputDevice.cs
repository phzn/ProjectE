using System;
using System.Collections.Generic;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.XR
{
	// Token: 0x02000010 RID: 16
	[NativeConditional("ENABLE_VR")]
	[UsedByNativeCode]
	public struct InputDevice : IEquatable<InputDevice>
	{
		// Token: 0x06000052 RID: 82 RVA: 0x00002F92 File Offset: 0x00001192
		internal InputDevice(ulong deviceId)
		{
			this.m_DeviceId = deviceId;
			this.m_Initialized = true;
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00002FA4 File Offset: 0x000011A4
		private ulong deviceId
		{
			get
			{
				return this.m_Initialized ? this.m_DeviceId : ulong.MaxValue;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000054 RID: 84 RVA: 0x00002FC8 File Offset: 0x000011C8
		public XRInputSubsystem subsystem
		{
			get
			{
				bool flag = InputDevice.s_InputSubsystemCache == null;
				if (flag)
				{
					InputDevice.s_InputSubsystemCache = new List<XRInputSubsystem>();
				}
				bool initialized = this.m_Initialized;
				if (initialized)
				{
					uint num = (uint)(this.m_DeviceId >> 32);
					SubsystemManager.GetSubsystems<XRInputSubsystem>(InputDevice.s_InputSubsystemCache);
					for (int i = 0; i < InputDevice.s_InputSubsystemCache.Count; i++)
					{
						bool flag2 = num == InputDevice.s_InputSubsystemCache[i].GetIndex();
						if (flag2)
						{
							return InputDevice.s_InputSubsystemCache[i];
						}
					}
				}
				return null;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000055 RID: 85 RVA: 0x0000305C File Offset: 0x0000125C
		public bool isValid
		{
			get
			{
				return this.IsValidId() && InputDevices.IsDeviceValid(this.m_DeviceId);
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000056 RID: 86 RVA: 0x00003084 File Offset: 0x00001284
		public string name
		{
			get
			{
				return this.IsValidId() ? InputDevices.GetDeviceName(this.m_DeviceId) : null;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000057 RID: 87 RVA: 0x000030AC File Offset: 0x000012AC
		[Obsolete("This API has been marked as deprecated and will be removed in future versions. Please use InputDevice.characteristics instead.")]
		public InputDeviceRole role
		{
			get
			{
				return this.IsValidId() ? InputDevices.GetDeviceRole(this.m_DeviceId) : InputDeviceRole.Unknown;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000058 RID: 88 RVA: 0x000030D4 File Offset: 0x000012D4
		public string manufacturer
		{
			get
			{
				return this.IsValidId() ? InputDevices.GetDeviceManufacturer(this.m_DeviceId) : null;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000059 RID: 89 RVA: 0x000030FC File Offset: 0x000012FC
		public string serialNumber
		{
			get
			{
				return this.IsValidId() ? InputDevices.GetDeviceSerialNumber(this.m_DeviceId) : null;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00003124 File Offset: 0x00001324
		public InputDeviceCharacteristics characteristics
		{
			get
			{
				return this.IsValidId() ? InputDevices.GetDeviceCharacteristics(this.m_DeviceId) : InputDeviceCharacteristics.None;
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x0000314C File Offset: 0x0000134C
		private bool IsValidId()
		{
			return this.deviceId != ulong.MaxValue;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x0000316C File Offset: 0x0000136C
		public bool SendHapticImpulse(uint channel, float amplitude, float duration = 1f)
		{
			bool flag = !this.IsValidId();
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				bool flag3 = amplitude < 0f;
				if (flag3)
				{
					throw new ArgumentException("Amplitude of SendHapticImpulse cannot be negative.");
				}
				bool flag4 = duration < 0f;
				if (flag4)
				{
					throw new ArgumentException("Duration of SendHapticImpulse cannot be negative.");
				}
				flag2 = InputDevices.SendHapticImpulse(this.m_DeviceId, channel, amplitude, duration);
			}
			return flag2;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000031CC File Offset: 0x000013CC
		public bool SendHapticBuffer(uint channel, byte[] buffer)
		{
			bool flag = !this.IsValidId();
			return !flag && InputDevices.SendHapticBuffer(this.m_DeviceId, channel, buffer);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000031FC File Offset: 0x000013FC
		public bool TryGetHapticCapabilities(out HapticCapabilities capabilities)
		{
			bool flag = this.CheckValidAndSetDefault<HapticCapabilities>(out capabilities);
			return flag && InputDevices.TryGetHapticCapabilities(this.m_DeviceId, out capabilities);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x0000322C File Offset: 0x0000142C
		public void StopHaptics()
		{
			bool flag = this.IsValidId();
			if (flag)
			{
				InputDevices.StopHaptics(this.m_DeviceId);
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003250 File Offset: 0x00001450
		public bool TryGetFeatureUsages(List<InputFeatureUsage> featureUsages)
		{
			bool flag = this.IsValidId();
			return flag && InputDevices.TryGetFeatureUsages(this.m_DeviceId, featureUsages);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x0000327C File Offset: 0x0000147C
		public bool TryGetFeatureValue(InputFeatureUsage<bool> usage, out bool value)
		{
			bool flag = this.CheckValidAndSetDefault<bool>(out value);
			return flag && InputDevices.TryGetFeatureValue_bool(this.m_DeviceId, usage.name, out value);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000032B0 File Offset: 0x000014B0
		public bool TryGetFeatureValue(InputFeatureUsage<uint> usage, out uint value)
		{
			bool flag = this.CheckValidAndSetDefault<uint>(out value);
			return flag && InputDevices.TryGetFeatureValue_UInt32(this.m_DeviceId, usage.name, out value);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000032E4 File Offset: 0x000014E4
		public bool TryGetFeatureValue(InputFeatureUsage<float> usage, out float value)
		{
			bool flag = this.CheckValidAndSetDefault<float>(out value);
			return flag && InputDevices.TryGetFeatureValue_float(this.m_DeviceId, usage.name, out value);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00003318 File Offset: 0x00001518
		public bool TryGetFeatureValue(InputFeatureUsage<Vector2> usage, out Vector2 value)
		{
			bool flag = this.CheckValidAndSetDefault<Vector2>(out value);
			return flag && InputDevices.TryGetFeatureValue_Vector2f(this.m_DeviceId, usage.name, out value);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x0000334C File Offset: 0x0000154C
		public bool TryGetFeatureValue(InputFeatureUsage<Vector3> usage, out Vector3 value)
		{
			bool flag = this.CheckValidAndSetDefault<Vector3>(out value);
			return flag && InputDevices.TryGetFeatureValue_Vector3f(this.m_DeviceId, usage.name, out value);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003380 File Offset: 0x00001580
		public bool TryGetFeatureValue(InputFeatureUsage<Quaternion> usage, out Quaternion value)
		{
			bool flag = this.CheckValidAndSetDefault<Quaternion>(out value);
			return flag && InputDevices.TryGetFeatureValue_Quaternionf(this.m_DeviceId, usage.name, out value);
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000033B4 File Offset: 0x000015B4
		public bool TryGetFeatureValue(InputFeatureUsage<Hand> usage, out Hand value)
		{
			bool flag = this.CheckValidAndSetDefault<Hand>(out value);
			return flag && InputDevices.TryGetFeatureValue_XRHand(this.m_DeviceId, usage.name, out value);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000033E8 File Offset: 0x000015E8
		public bool TryGetFeatureValue(InputFeatureUsage<Bone> usage, out Bone value)
		{
			bool flag = this.CheckValidAndSetDefault<Bone>(out value);
			return flag && InputDevices.TryGetFeatureValue_XRBone(this.m_DeviceId, usage.name, out value);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x0000341C File Offset: 0x0000161C
		public bool TryGetFeatureValue(InputFeatureUsage<Eyes> usage, out Eyes value)
		{
			bool flag = this.CheckValidAndSetDefault<Eyes>(out value);
			return flag && InputDevices.TryGetFeatureValue_XREyes(this.m_DeviceId, usage.name, out value);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00003450 File Offset: 0x00001650
		public bool TryGetFeatureValue(InputFeatureUsage<byte[]> usage, byte[] value)
		{
			bool flag = this.IsValidId();
			return flag && InputDevices.TryGetFeatureValue_Custom(this.m_DeviceId, usage.name, value);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003484 File Offset: 0x00001684
		public bool TryGetFeatureValue(InputFeatureUsage<InputTrackingState> usage, out InputTrackingState value)
		{
			bool flag = this.IsValidId();
			if (flag)
			{
				uint num = 0U;
				bool flag2 = InputDevices.TryGetFeatureValue_UInt32(this.m_DeviceId, usage.name, out num);
				if (flag2)
				{
					value = (InputTrackingState)num;
					return true;
				}
			}
			value = InputTrackingState.None;
			return false;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000034C8 File Offset: 0x000016C8
		public bool TryGetFeatureValue(InputFeatureUsage<bool> usage, DateTime time, out bool value)
		{
			bool flag = this.CheckValidAndSetDefault<bool>(out value);
			return flag && InputDevices.TryGetFeatureValueAtTime_bool(this.m_DeviceId, usage.name, TimeConverter.LocalDateTimeToUnixTimeMilliseconds(time), out value);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003504 File Offset: 0x00001704
		public bool TryGetFeatureValue(InputFeatureUsage<uint> usage, DateTime time, out uint value)
		{
			bool flag = this.CheckValidAndSetDefault<uint>(out value);
			return flag && InputDevices.TryGetFeatureValueAtTime_UInt32(this.m_DeviceId, usage.name, TimeConverter.LocalDateTimeToUnixTimeMilliseconds(time), out value);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003540 File Offset: 0x00001740
		public bool TryGetFeatureValue(InputFeatureUsage<float> usage, DateTime time, out float value)
		{
			bool flag = this.CheckValidAndSetDefault<float>(out value);
			return flag && InputDevices.TryGetFeatureValueAtTime_float(this.m_DeviceId, usage.name, TimeConverter.LocalDateTimeToUnixTimeMilliseconds(time), out value);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x0000357C File Offset: 0x0000177C
		public bool TryGetFeatureValue(InputFeatureUsage<Vector2> usage, DateTime time, out Vector2 value)
		{
			bool flag = this.CheckValidAndSetDefault<Vector2>(out value);
			return flag && InputDevices.TryGetFeatureValueAtTime_Vector2f(this.m_DeviceId, usage.name, TimeConverter.LocalDateTimeToUnixTimeMilliseconds(time), out value);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x000035B8 File Offset: 0x000017B8
		public bool TryGetFeatureValue(InputFeatureUsage<Vector3> usage, DateTime time, out Vector3 value)
		{
			bool flag = this.CheckValidAndSetDefault<Vector3>(out value);
			return flag && InputDevices.TryGetFeatureValueAtTime_Vector3f(this.m_DeviceId, usage.name, TimeConverter.LocalDateTimeToUnixTimeMilliseconds(time), out value);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000035F4 File Offset: 0x000017F4
		public bool TryGetFeatureValue(InputFeatureUsage<Quaternion> usage, DateTime time, out Quaternion value)
		{
			bool flag = this.CheckValidAndSetDefault<Quaternion>(out value);
			return flag && InputDevices.TryGetFeatureValueAtTime_Quaternionf(this.m_DeviceId, usage.name, TimeConverter.LocalDateTimeToUnixTimeMilliseconds(time), out value);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003630 File Offset: 0x00001830
		public bool TryGetFeatureValue(InputFeatureUsage<InputTrackingState> usage, DateTime time, out InputTrackingState value)
		{
			bool flag = this.IsValidId();
			if (flag)
			{
				uint num = 0U;
				bool flag2 = InputDevices.TryGetFeatureValueAtTime_UInt32(this.m_DeviceId, usage.name, TimeConverter.LocalDateTimeToUnixTimeMilliseconds(time), out num);
				if (flag2)
				{
					value = (InputTrackingState)num;
					return true;
				}
			}
			value = InputTrackingState.None;
			return false;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x0000367C File Offset: 0x0000187C
		private bool CheckValidAndSetDefault<T>(out T value)
		{
			value = default(T);
			return this.IsValidId();
		}

		// Token: 0x06000074 RID: 116 RVA: 0x0000369C File Offset: 0x0000189C
		public override bool Equals(object obj)
		{
			bool flag = !(obj is InputDevice);
			return !flag && this.Equals((InputDevice)obj);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x000036D0 File Offset: 0x000018D0
		public bool Equals(InputDevice other)
		{
			return this.deviceId == other.deviceId;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000036F4 File Offset: 0x000018F4
		public override int GetHashCode()
		{
			return this.deviceId.GetHashCode();
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003714 File Offset: 0x00001914
		public static bool operator ==(InputDevice a, InputDevice b)
		{
			return a.Equals(b);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003730 File Offset: 0x00001930
		public static bool operator !=(InputDevice a, InputDevice b)
		{
			return !(a == b);
		}

		// Token: 0x04000099 RID: 153
		private static List<XRInputSubsystem> s_InputSubsystemCache;

		// Token: 0x0400009A RID: 154
		private ulong m_DeviceId;

		// Token: 0x0400009B RID: 155
		private bool m_Initialized;
	}
}
