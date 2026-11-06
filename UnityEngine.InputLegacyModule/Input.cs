using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;

namespace UnityEngine
{
	// Token: 0x02000012 RID: 18
	[NativeHeader("Runtime/Input/InputBindings.h")]
	public class Input
	{
		// Token: 0x0600005E RID: 94 RVA: 0x0000264D File Offset: 0x0000084D
		public static float GetAxis(string axisName)
		{
			return InputUnsafeUtility.GetAxis(axisName);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00002655 File Offset: 0x00000855
		public static float GetAxisRaw(string axisName)
		{
			return InputUnsafeUtility.GetAxisRaw(axisName);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x0000265D File Offset: 0x0000085D
		public static bool GetButton(string buttonName)
		{
			return InputUnsafeUtility.GetButton(buttonName);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00002665 File Offset: 0x00000865
		public static bool GetButtonDown(string buttonName)
		{
			return InputUnsafeUtility.GetButtonDown(buttonName);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x0000266D File Offset: 0x0000086D
		public static bool GetButtonUp(string buttonName)
		{
			return InputUnsafeUtility.GetButtonUp(buttonName);
		}

		// Token: 0x06000063 RID: 99
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetKeyInt(KeyCode key);

		// Token: 0x06000064 RID: 100
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetKeyUpInt(KeyCode key);

		// Token: 0x06000065 RID: 101
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetKeyDownInt(KeyCode key);

		// Token: 0x06000066 RID: 102
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetMouseButton(int button);

		// Token: 0x06000067 RID: 103
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetMouseButtonDown(int button);

		// Token: 0x06000068 RID: 104
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetMouseButtonUp(int button);

		// Token: 0x06000069 RID: 105
		[FreeFunction("ResetInput")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ResetInputAxes();

		// Token: 0x0600006A RID: 106
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string[] GetJoystickNames();

		// Token: 0x0600006B RID: 107 RVA: 0x00002678 File Offset: 0x00000878
		[NativeThrows]
		public static Touch GetTouch(int index)
		{
			Touch touch;
			Input.GetTouch_Injected(index, out touch);
			return touch;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00002690 File Offset: 0x00000890
		[NativeThrows]
		public static PenData GetPenEvent(int index)
		{
			PenData penData;
			Input.GetPenEvent_Injected(index, out penData);
			return penData;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000026A8 File Offset: 0x000008A8
		[NativeThrows]
		public static PenData GetLastPenContactEvent()
		{
			PenData penData;
			Input.GetLastPenContactEvent_Injected(out penData);
			return penData;
		}

		// Token: 0x0600006E RID: 110
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ResetPenEvents();

		// Token: 0x0600006F RID: 111
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ClearLastPenContactEvent();

		// Token: 0x06000070 RID: 112 RVA: 0x000026C0 File Offset: 0x000008C0
		[NativeThrows]
		public static AccelerationEvent GetAccelerationEvent(int index)
		{
			AccelerationEvent accelerationEvent;
			Input.GetAccelerationEvent_Injected(index, out accelerationEvent);
			return accelerationEvent;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000026D6 File Offset: 0x000008D6
		public static bool GetKey(KeyCode key)
		{
			return Input.GetKeyInt(key);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000026DE File Offset: 0x000008DE
		public static bool GetKey(string name)
		{
			return InputUnsafeUtility.GetKeyString(name);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000026E6 File Offset: 0x000008E6
		public static bool GetKeyUp(KeyCode key)
		{
			return Input.GetKeyUpInt(key);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x000026EE File Offset: 0x000008EE
		public static bool GetKeyUp(string name)
		{
			return InputUnsafeUtility.GetKeyUpString(name);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x000026F6 File Offset: 0x000008F6
		public static bool GetKeyDown(KeyCode key)
		{
			return Input.GetKeyDownInt(key);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000026FE File Offset: 0x000008FE
		public static bool GetKeyDown(string name)
		{
			return InputUnsafeUtility.GetKeyDownString(name);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00002706 File Offset: 0x00000906
		[Conditional("UNITY_EDITOR")]
		internal static void SimulateTouch(Touch touch)
		{
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00002709 File Offset: 0x00000909
		[FreeFunction("SimulateTouch")]
		[NativeConditional("UNITY_EDITOR")]
		[Conditional("UNITY_EDITOR")]
		private static void SimulateTouchInternal(Touch touch, long timestamp)
		{
			Input.SimulateTouchInternal_Injected(ref touch, timestamp);
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000079 RID: 121
		// (set) Token: 0x0600007A RID: 122
		public static extern bool simulateMouseWithTouches
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600007B RID: 123
		[NativeThrows]
		public static extern bool anyKey
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600007C RID: 124
		[NativeThrows]
		public static extern bool anyKeyDown
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600007D RID: 125
		[NativeThrows]
		public static extern string inputString
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00002714 File Offset: 0x00000914
		[NativeThrows]
		public static Vector3 mousePosition
		{
			get
			{
				Vector3 vector;
				Input.get_mousePosition_Injected(out vector);
				return vector;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600007F RID: 127 RVA: 0x0000272C File Offset: 0x0000092C
		[NativeThrows]
		public static Vector2 mouseScrollDelta
		{
			get
			{
				Vector2 vector;
				Input.get_mouseScrollDelta_Injected(out vector);
				return vector;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000080 RID: 128
		// (set) Token: 0x06000081 RID: 129
		public static extern IMECompositionMode imeCompositionMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000082 RID: 130
		public static extern string compositionString
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000083 RID: 131
		public static extern bool imeIsSelected
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00002744 File Offset: 0x00000944
		// (set) Token: 0x06000085 RID: 133 RVA: 0x00002759 File Offset: 0x00000959
		public static Vector2 compositionCursorPos
		{
			get
			{
				Vector2 vector;
				Input.get_compositionCursorPos_Injected(out vector);
				return vector;
			}
			set
			{
				Input.set_compositionCursorPos_Injected(ref value);
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000086 RID: 134
		// (set) Token: 0x06000087 RID: 135
		[Obsolete("eatKeyPressOnTextFieldFocus property is deprecated, and only provided to support legacy behavior.")]
		public static extern bool eatKeyPressOnTextFieldFocus
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000088 RID: 136
		public static extern bool mousePresent
		{
			[FreeFunction("GetMousePresent")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000089 RID: 137
		public static extern int penEventCount
		{
			[FreeFunction("GetPenEventCount")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600008A RID: 138
		public static extern int touchCount
		{
			[FreeFunction("GetTouchCount")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600008B RID: 139
		public static extern bool touchPressureSupported
		{
			[FreeFunction("IsTouchPressureSupported")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600008C RID: 140
		public static extern bool stylusTouchSupported
		{
			[FreeFunction("IsStylusTouchSupported")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600008D RID: 141
		public static extern bool touchSupported
		{
			[FreeFunction("IsTouchSupported")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600008E RID: 142
		// (set) Token: 0x0600008F RID: 143
		public static extern bool multiTouchEnabled
		{
			[FreeFunction("IsMultiTouchEnabled")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[FreeFunction("SetMultiTouchEnabled")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000090 RID: 144
		[Obsolete("isGyroAvailable property is deprecated. Please use SystemInfo.supportsGyroscope instead.")]
		public static extern bool isGyroAvailable
		{
			[FreeFunction("IsGyroAvailable")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000091 RID: 145
		public static extern DeviceOrientation deviceOrientation
		{
			[FreeFunction("GetDeviceOrientation")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00002764 File Offset: 0x00000964
		public static Vector3 acceleration
		{
			[FreeFunction("GetAcceleration")]
			get
			{
				Vector3 vector;
				Input.get_acceleration_Injected(out vector);
				return vector;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000093 RID: 147
		// (set) Token: 0x06000094 RID: 148
		public static extern bool compensateSensors
		{
			[FreeFunction("IsCompensatingSensors")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[FreeFunction("SetCompensatingSensors")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000095 RID: 149
		public static extern int accelerationEventCount
		{
			[FreeFunction("GetAccelerationCount")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000096 RID: 150
		// (set) Token: 0x06000097 RID: 151
		public static extern bool backButtonLeavesApp
		{
			[FreeFunction("GetBackButtonLeavesApp")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[FreeFunction("SetBackButtonLeavesApp")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000098 RID: 152 RVA: 0x0000277C File Offset: 0x0000097C
		public static LocationService location
		{
			get
			{
				bool flag = Input.locationServiceInstance == null;
				if (flag)
				{
					Input.locationServiceInstance = new LocationService();
				}
				return Input.locationServiceInstance;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000099 RID: 153 RVA: 0x000027AC File Offset: 0x000009AC
		public static Compass compass
		{
			get
			{
				bool flag = Input.compassInstance == null;
				if (flag)
				{
					Input.compassInstance = new Compass();
				}
				return Input.compassInstance;
			}
		}

		// Token: 0x0600009A RID: 154
		[FreeFunction("GetGyro")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetGyroInternal();

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600009B RID: 155 RVA: 0x000027DC File Offset: 0x000009DC
		public static Gyroscope gyro
		{
			get
			{
				bool flag = Input.s_MainGyro == null;
				if (flag)
				{
					Input.s_MainGyro = new Gyroscope(Input.GetGyroInternal());
				}
				return Input.s_MainGyro;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600009C RID: 156 RVA: 0x00002810 File Offset: 0x00000A10
		public static Touch[] touches
		{
			get
			{
				int touchCount = Input.touchCount;
				Touch[] array = new Touch[touchCount];
				for (int i = 0; i < touchCount; i++)
				{
					array[i] = Input.GetTouch(i);
				}
				return array;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600009D RID: 157 RVA: 0x00002850 File Offset: 0x00000A50
		public static AccelerationEvent[] accelerationEvents
		{
			get
			{
				int accelerationEventCount = Input.accelerationEventCount;
				AccelerationEvent[] array = new AccelerationEvent[accelerationEventCount];
				for (int i = 0; i < accelerationEventCount; i++)
				{
					array[i] = Input.GetAccelerationEvent(i);
				}
				return array;
			}
		}

		// Token: 0x0600009E RID: 158
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool CheckDisabled();

		// Token: 0x060000A0 RID: 160
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetTouch_Injected(int index, out Touch ret);

		// Token: 0x060000A1 RID: 161
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetPenEvent_Injected(int index, out PenData ret);

		// Token: 0x060000A2 RID: 162
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetLastPenContactEvent_Injected(out PenData ret);

		// Token: 0x060000A3 RID: 163
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetAccelerationEvent_Injected(int index, out AccelerationEvent ret);

		// Token: 0x060000A4 RID: 164
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SimulateTouchInternal_Injected(ref Touch touch, long timestamp);

		// Token: 0x060000A5 RID: 165
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void get_mousePosition_Injected(out Vector3 ret);

		// Token: 0x060000A6 RID: 166
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void get_mouseScrollDelta_Injected(out Vector2 ret);

		// Token: 0x060000A7 RID: 167
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void get_compositionCursorPos_Injected(out Vector2 ret);

		// Token: 0x060000A8 RID: 168
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void set_compositionCursorPos_Injected(ref Vector2 value);

		// Token: 0x060000A9 RID: 169
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void get_acceleration_Injected(out Vector3 ret);

		// Token: 0x0400004B RID: 75
		private static LocationService locationServiceInstance;

		// Token: 0x0400004C RID: 76
		private static Compass compassInstance;

		// Token: 0x0400004D RID: 77
		private static Gyroscope s_MainGyro;
	}
}
