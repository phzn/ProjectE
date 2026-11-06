using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.XR
{
	// Token: 0x02000017 RID: 23
	[StaticAccessor("XRInputDevices::Get()", StaticAccessorType.Dot)]
	[NativeHeader("Modules/XR/Subsystems/Input/Public/XRInputDevices.h")]
	[NativeConditional("ENABLE_VR")]
	[UsedByNativeCode]
	[StructLayout(LayoutKind.Sequential)]
	public class InputDevices
	{
		// Token: 0x060000B3 RID: 179 RVA: 0x00003CF4 File Offset: 0x00001EF4
		public static InputDevice GetDeviceAtXRNode(XRNode node)
		{
			ulong deviceIdAtXRNode = InputTracking.GetDeviceIdAtXRNode(node);
			return new InputDevice(deviceIdAtXRNode);
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00003D14 File Offset: 0x00001F14
		public static void GetDevicesAtXRNode(XRNode node, List<InputDevice> inputDevices)
		{
			bool flag = inputDevices == null;
			if (flag)
			{
				throw new ArgumentNullException("inputDevices");
			}
			List<ulong> list = new List<ulong>();
			InputTracking.GetDeviceIdsAtXRNode_Internal(node, list);
			inputDevices.Clear();
			foreach (ulong num in list)
			{
				InputDevice inputDevice = new InputDevice(num);
				bool isValid = inputDevice.isValid;
				if (isValid)
				{
					inputDevices.Add(inputDevice);
				}
			}
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00003DA8 File Offset: 0x00001FA8
		public static void GetDevices(List<InputDevice> inputDevices)
		{
			bool flag = inputDevices == null;
			if (flag)
			{
				throw new ArgumentNullException("inputDevices");
			}
			inputDevices.Clear();
			InputDevices.GetDevices_Internal(inputDevices);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00003DD8 File Offset: 0x00001FD8
		[Obsolete("This API has been marked as deprecated and will be removed in future versions. Please use InputDevices.GetDevicesWithCharacteristics instead.")]
		public static void GetDevicesWithRole(InputDeviceRole role, List<InputDevice> inputDevices)
		{
			bool flag = inputDevices == null;
			if (flag)
			{
				throw new ArgumentNullException("inputDevices");
			}
			bool flag2 = InputDevices.s_InputDeviceList == null;
			if (flag2)
			{
				InputDevices.s_InputDeviceList = new List<InputDevice>();
			}
			InputDevices.GetDevices_Internal(InputDevices.s_InputDeviceList);
			inputDevices.Clear();
			foreach (InputDevice inputDevice in InputDevices.s_InputDeviceList)
			{
				bool flag3 = inputDevice.role == role;
				if (flag3)
				{
					inputDevices.Add(inputDevice);
				}
			}
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00003E7C File Offset: 0x0000207C
		public static void GetDevicesWithCharacteristics(InputDeviceCharacteristics desiredCharacteristics, List<InputDevice> inputDevices)
		{
			bool flag = inputDevices == null;
			if (flag)
			{
				throw new ArgumentNullException("inputDevices");
			}
			bool flag2 = InputDevices.s_InputDeviceList == null;
			if (flag2)
			{
				InputDevices.s_InputDeviceList = new List<InputDevice>();
			}
			InputDevices.GetDevices_Internal(InputDevices.s_InputDeviceList);
			inputDevices.Clear();
			foreach (InputDevice inputDevice in InputDevices.s_InputDeviceList)
			{
				bool flag3 = (inputDevice.characteristics & desiredCharacteristics) == desiredCharacteristics;
				if (flag3)
				{
					inputDevices.Add(inputDevice);
				}
			}
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060000B8 RID: 184 RVA: 0x00003F20 File Offset: 0x00002120
		// (remove) Token: 0x060000B9 RID: 185 RVA: 0x00003F54 File Offset: 0x00002154
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<InputDevice> deviceConnected;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060000BA RID: 186 RVA: 0x00003F88 File Offset: 0x00002188
		// (remove) Token: 0x060000BB RID: 187 RVA: 0x00003FBC File Offset: 0x000021BC
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<InputDevice> deviceDisconnected;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060000BC RID: 188 RVA: 0x00003FF0 File Offset: 0x000021F0
		// (remove) Token: 0x060000BD RID: 189 RVA: 0x00004024 File Offset: 0x00002224
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<InputDevice> deviceConfigChanged;

		// Token: 0x060000BE RID: 190 RVA: 0x00004058 File Offset: 0x00002258
		[RequiredByNativeCode]
		private static void InvokeConnectionEvent(ulong deviceId, ConnectionChangeType change)
		{
			switch (change)
			{
			case ConnectionChangeType.Connected:
			{
				bool flag = InputDevices.deviceConnected != null;
				if (flag)
				{
					InputDevices.deviceConnected(new InputDevice(deviceId));
				}
				break;
			}
			case ConnectionChangeType.Disconnected:
			{
				bool flag2 = InputDevices.deviceDisconnected != null;
				if (flag2)
				{
					InputDevices.deviceDisconnected(new InputDevice(deviceId));
				}
				break;
			}
			case ConnectionChangeType.ConfigChange:
			{
				bool flag3 = InputDevices.deviceConfigChanged != null;
				if (flag3)
				{
					InputDevices.deviceConfigChanged(new InputDevice(deviceId));
				}
				break;
			}
			}
		}

		// Token: 0x060000BF RID: 191
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetDevices_Internal([NotNull("ArgumentNullException")] List<InputDevice> inputDevices);

		// Token: 0x060000C0 RID: 192
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool SendHapticImpulse(ulong deviceId, uint channel, float amplitude, float duration);

		// Token: 0x060000C1 RID: 193
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool SendHapticBuffer(ulong deviceId, uint channel, [NotNull("ArgumentNullException")] byte[] buffer);

		// Token: 0x060000C2 RID: 194
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool TryGetHapticCapabilities(ulong deviceId, out HapticCapabilities capabilities);

		// Token: 0x060000C3 RID: 195
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void StopHaptics(ulong deviceId);

		// Token: 0x060000C4 RID: 196
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool TryGetFeatureUsages(ulong deviceId, [NotNull("ArgumentNullException")] List<InputFeatureUsage> featureUsages);

		// Token: 0x060000C5 RID: 197
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool TryGetFeatureValue_bool(ulong deviceId, string usage, out bool value);

		// Token: 0x060000C6 RID: 198
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool TryGetFeatureValue_UInt32(ulong deviceId, string usage, out uint value);

		// Token: 0x060000C7 RID: 199
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool TryGetFeatureValue_float(ulong deviceId, string usage, out float value);

		// Token: 0x060000C8 RID: 200
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool TryGetFeatureValue_Vector2f(ulong deviceId, string usage, out Vector2 value);

		// Token: 0x060000C9 RID: 201
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool TryGetFeatureValue_Vector3f(ulong deviceId, string usage, out Vector3 value);

		// Token: 0x060000CA RID: 202
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool TryGetFeatureValue_Quaternionf(ulong deviceId, string usage, out Quaternion value);

		// Token: 0x060000CB RID: 203
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool TryGetFeatureValue_Custom(ulong deviceId, string usage, [Out] byte[] value);

		// Token: 0x060000CC RID: 204
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool TryGetFeatureValueAtTime_bool(ulong deviceId, string usage, long time, out bool value);

		// Token: 0x060000CD RID: 205
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool TryGetFeatureValueAtTime_UInt32(ulong deviceId, string usage, long time, out uint value);

		// Token: 0x060000CE RID: 206
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool TryGetFeatureValueAtTime_float(ulong deviceId, string usage, long time, out float value);

		// Token: 0x060000CF RID: 207
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool TryGetFeatureValueAtTime_Vector2f(ulong deviceId, string usage, long time, out Vector2 value);

		// Token: 0x060000D0 RID: 208
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool TryGetFeatureValueAtTime_Vector3f(ulong deviceId, string usage, long time, out Vector3 value);

		// Token: 0x060000D1 RID: 209
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool TryGetFeatureValueAtTime_Quaternionf(ulong deviceId, string usage, long time, out Quaternion value);

		// Token: 0x060000D2 RID: 210
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool TryGetFeatureValue_XRHand(ulong deviceId, string usage, out Hand value);

		// Token: 0x060000D3 RID: 211
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool TryGetFeatureValue_XRBone(ulong deviceId, string usage, out Bone value);

		// Token: 0x060000D4 RID: 212
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool TryGetFeatureValue_XREyes(ulong deviceId, string usage, out Eyes value);

		// Token: 0x060000D5 RID: 213
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool IsDeviceValid(ulong deviceId);

		// Token: 0x060000D6 RID: 214
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string GetDeviceName(ulong deviceId);

		// Token: 0x060000D7 RID: 215
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string GetDeviceManufacturer(ulong deviceId);

		// Token: 0x060000D8 RID: 216
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string GetDeviceSerialNumber(ulong deviceId);

		// Token: 0x060000D9 RID: 217
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern InputDeviceCharacteristics GetDeviceCharacteristics(ulong deviceId);

		// Token: 0x060000DA RID: 218 RVA: 0x000040E0 File Offset: 0x000022E0
		internal static InputDeviceRole GetDeviceRole(ulong deviceId)
		{
			InputDeviceCharacteristics deviceCharacteristics = InputDevices.GetDeviceCharacteristics(deviceId);
			bool flag = (deviceCharacteristics & (InputDeviceCharacteristics.HeadMounted | InputDeviceCharacteristics.TrackedDevice)) == (InputDeviceCharacteristics.HeadMounted | InputDeviceCharacteristics.TrackedDevice);
			InputDeviceRole inputDeviceRole;
			if (flag)
			{
				inputDeviceRole = InputDeviceRole.Generic;
			}
			else
			{
				bool flag2 = (deviceCharacteristics & (InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.TrackedDevice | InputDeviceCharacteristics.Left)) == (InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.TrackedDevice | InputDeviceCharacteristics.Left);
				if (flag2)
				{
					inputDeviceRole = InputDeviceRole.LeftHanded;
				}
				else
				{
					bool flag3 = (deviceCharacteristics & (InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.TrackedDevice | InputDeviceCharacteristics.Right)) == (InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.TrackedDevice | InputDeviceCharacteristics.Right);
					if (flag3)
					{
						inputDeviceRole = InputDeviceRole.RightHanded;
					}
					else
					{
						bool flag4 = (deviceCharacteristics & InputDeviceCharacteristics.Controller) == InputDeviceCharacteristics.Controller;
						if (flag4)
						{
							inputDeviceRole = InputDeviceRole.GameController;
						}
						else
						{
							bool flag5 = (deviceCharacteristics & (InputDeviceCharacteristics.TrackedDevice | InputDeviceCharacteristics.TrackingReference)) == (InputDeviceCharacteristics.TrackedDevice | InputDeviceCharacteristics.TrackingReference);
							if (flag5)
							{
								inputDeviceRole = InputDeviceRole.TrackingReference;
							}
							else
							{
								bool flag6 = (deviceCharacteristics & InputDeviceCharacteristics.TrackedDevice) == InputDeviceCharacteristics.TrackedDevice;
								if (flag6)
								{
									inputDeviceRole = InputDeviceRole.HardwareTracker;
								}
								else
								{
									inputDeviceRole = InputDeviceRole.Unknown;
								}
							}
						}
					}
				}
			}
			return inputDeviceRole;
		}

		// Token: 0x040000AC RID: 172
		private static List<InputDevice> s_InputDeviceList;
	}
}
