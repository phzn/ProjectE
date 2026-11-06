using System;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000F9 RID: 249
	public static class InputState
	{
		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x06000EA4 RID: 3748 RVA: 0x000478FE File Offset: 0x00045AFE
		public static InputUpdateType currentUpdateType
		{
			get
			{
				return InputUpdate.s_LatestUpdateType;
			}
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x06000EA5 RID: 3749 RVA: 0x00047905 File Offset: 0x00045B05
		public static uint updateCount
		{
			get
			{
				return InputUpdate.s_UpdateStepCount;
			}
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x06000EA6 RID: 3750 RVA: 0x0004790C File Offset: 0x00045B0C
		public static double currentTime
		{
			get
			{
				return InputRuntime.s_Instance.currentTime - InputRuntime.s_CurrentTimeOffsetToRealtimeSinceStartup;
			}
		}

		// Token: 0x14000027 RID: 39
		// (add) Token: 0x06000EA7 RID: 3751 RVA: 0x0004791E File Offset: 0x00045B1E
		// (remove) Token: 0x06000EA8 RID: 3752 RVA: 0x0004792B File Offset: 0x00045B2B
		public static event Action<InputDevice, InputEventPtr> onChange
		{
			add
			{
				InputSystem.s_Manager.onDeviceStateChange += value;
			}
			remove
			{
				InputSystem.s_Manager.onDeviceStateChange -= value;
			}
		}

		// Token: 0x06000EA9 RID: 3753 RVA: 0x00047938 File Offset: 0x00045B38
		public unsafe static void Change(InputDevice device, InputEventPtr eventPtr, InputUpdateType updateType = InputUpdateType.None)
		{
			if (device == null)
			{
				throw new ArgumentNullException("device");
			}
			if (!eventPtr.valid)
			{
				throw new ArgumentNullException("eventPtr");
			}
			FourCC type = eventPtr.type;
			FourCC fourCC;
			if (type == 1398030676)
			{
				fourCC = StateEvent.FromUnchecked(eventPtr)->stateFormat;
			}
			else
			{
				if (!(type == 1145852993))
				{
					return;
				}
				fourCC = DeltaStateEvent.FromUnchecked(eventPtr)->stateFormat;
			}
			if (fourCC != device.stateBlock.format)
			{
				throw new ArgumentException(string.Format("State format {0} from event does not match state format {1} of device {2}", fourCC, device.stateBlock.format, device), "eventPtr");
			}
			InputSystem.s_Manager.UpdateState(device, eventPtr, (updateType != InputUpdateType.None) ? updateType : InputSystem.s_Manager.defaultUpdateType);
		}

		// Token: 0x06000EAA RID: 3754 RVA: 0x00047A17 File Offset: 0x00045C17
		public static void Change<TState>(InputControl control, TState state, InputUpdateType updateType = InputUpdateType.None, InputEventPtr eventPtr = default(InputEventPtr)) where TState : struct
		{
			InputState.Change<TState>(control, ref state, updateType, eventPtr);
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x00047A24 File Offset: 0x00045C24
		public unsafe static void Change<TState>(InputControl control, ref TState state, InputUpdateType updateType = InputUpdateType.None, InputEventPtr eventPtr = default(InputEventPtr)) where TState : struct
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			if (control.stateBlock.bitOffset != 0U || control.stateBlock.sizeInBits % 8U != 0U)
			{
				throw new ArgumentException(string.Format("Cannot change state of bitfield control '{0}' using this method", control), "control");
			}
			InputDevice device = control.device;
			long num = Math.Min((long)UnsafeUtility.SizeOf<TState>(), (long)((ulong)control.m_StateBlock.alignedSizeInBytes));
			void* ptr = UnsafeUtility.AddressOf<TState>(ref state);
			uint num2 = control.stateBlock.byteOffset - device.stateBlock.byteOffset;
			InputSystem.s_Manager.UpdateState(device, (updateType != InputUpdateType.None) ? updateType : InputSystem.s_Manager.defaultUpdateType, ptr, num2, (uint)num, eventPtr.valid ? eventPtr.internalTime : InputRuntime.s_Instance.currentTime, eventPtr);
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x00047AFC File Offset: 0x00045CFC
		public static bool IsIntegerFormat(this FourCC format)
		{
			return format == InputStateBlock.FormatBit || format == InputStateBlock.FormatInt || format == InputStateBlock.FormatByte || format == InputStateBlock.FormatShort || format == InputStateBlock.FormatSBit || format == InputStateBlock.FormatUInt || format == InputStateBlock.FormatUShort || format == InputStateBlock.FormatLong || format == InputStateBlock.FormatULong;
		}

		// Token: 0x06000EAD RID: 3757 RVA: 0x00047B80 File Offset: 0x00045D80
		public static void AddChangeMonitor(InputControl control, IInputStateChangeMonitor monitor, long monitorIndex = -1L, uint groupIndex = 0U)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			if (monitor == null)
			{
				throw new ArgumentNullException("monitor");
			}
			if (!control.device.added)
			{
				throw new ArgumentException(string.Format("Device for control '{0}' has not been added to system", control));
			}
			InputSystem.s_Manager.AddStateChangeMonitor(control, monitor, monitorIndex, groupIndex);
		}

		// Token: 0x06000EAE RID: 3758 RVA: 0x00047BD8 File Offset: 0x00045DD8
		public static IInputStateChangeMonitor AddChangeMonitor(InputControl control, Action<InputControl, double, InputEventPtr, long> valueChangeCallback, int monitorIndex = -1, Action<InputControl, double, long, int> timerExpiredCallback = null)
		{
			if (valueChangeCallback == null)
			{
				throw new ArgumentNullException("valueChangeCallback");
			}
			InputState.StateChangeMonitorDelegate stateChangeMonitorDelegate = new InputState.StateChangeMonitorDelegate
			{
				valueChangeCallback = valueChangeCallback,
				timerExpiredCallback = timerExpiredCallback
			};
			InputState.AddChangeMonitor(control, stateChangeMonitorDelegate, (long)monitorIndex, 0U);
			return stateChangeMonitorDelegate;
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x00047C12 File Offset: 0x00045E12
		public static void RemoveChangeMonitor(InputControl control, IInputStateChangeMonitor monitor, long monitorIndex = -1L)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			if (monitor == null)
			{
				throw new ArgumentNullException("monitor");
			}
			InputSystem.s_Manager.RemoveStateChangeMonitor(control, monitor, monitorIndex);
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x00047C3D File Offset: 0x00045E3D
		public static void AddChangeMonitorTimeout(InputControl control, IInputStateChangeMonitor monitor, double time, long monitorIndex = -1L, int timerIndex = -1)
		{
			if (monitor == null)
			{
				throw new ArgumentNullException("monitor");
			}
			InputSystem.s_Manager.AddStateChangeMonitorTimeout(control, monitor, time, monitorIndex, timerIndex);
		}

		// Token: 0x06000EB1 RID: 3761 RVA: 0x00047C5D File Offset: 0x00045E5D
		public static void RemoveChangeMonitorTimeout(IInputStateChangeMonitor monitor, long monitorIndex = -1L, int timerIndex = -1)
		{
			if (monitor == null)
			{
				throw new ArgumentNullException("monitor");
			}
			InputSystem.s_Manager.RemoveStateChangeMonitorTimeout(monitor, monitorIndex, timerIndex);
		}

		// Token: 0x02000223 RID: 547
		private class StateChangeMonitorDelegate : IInputStateChangeMonitor
		{
			// Token: 0x0600153B RID: 5435 RVA: 0x00062DB0 File Offset: 0x00060FB0
			public void NotifyControlStateChanged(InputControl control, double time, InputEventPtr eventPtr, long monitorIndex)
			{
				this.valueChangeCallback(control, time, eventPtr, monitorIndex);
			}

			// Token: 0x0600153C RID: 5436 RVA: 0x00062DC2 File Offset: 0x00060FC2
			public void NotifyTimerExpired(InputControl control, double time, long monitorIndex, int timerIndex)
			{
				Action<InputControl, double, long, int> action = this.timerExpiredCallback;
				if (action == null)
				{
					return;
				}
				action(control, time, monitorIndex, timerIndex);
			}

			// Token: 0x04000BC2 RID: 3010
			public Action<InputControl, double, InputEventPtr, long> valueChangeCallback;

			// Token: 0x04000BC3 RID: 3011
			public Action<InputControl, double, long, int> timerExpiredCallback;
		}
	}
}
