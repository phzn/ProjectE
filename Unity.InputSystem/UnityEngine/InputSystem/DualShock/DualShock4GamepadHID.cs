using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.DualShock.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.DualShock
{
	// Token: 0x020000A4 RID: 164
	[InputControlLayout(stateType = typeof(DualShock4HIDInputReport), hideInUI = true, isNoisy = true)]
	public class DualShock4GamepadHID : DualShockGamepad, IEventPreProcessor, IInputStateCallbackReceiver
	{
		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06000CB9 RID: 3257 RVA: 0x00043512 File Offset: 0x00041712
		// (set) Token: 0x06000CBA RID: 3258 RVA: 0x0004351A File Offset: 0x0004171A
		public ButtonControl leftTriggerButton { get; protected set; }

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06000CBB RID: 3259 RVA: 0x00043523 File Offset: 0x00041723
		// (set) Token: 0x06000CBC RID: 3260 RVA: 0x0004352B File Offset: 0x0004172B
		public ButtonControl rightTriggerButton { get; protected set; }

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06000CBD RID: 3261 RVA: 0x00043534 File Offset: 0x00041734
		// (set) Token: 0x06000CBE RID: 3262 RVA: 0x0004353C File Offset: 0x0004173C
		public ButtonControl playStationButton { get; protected set; }

		// Token: 0x06000CBF RID: 3263 RVA: 0x00043545 File Offset: 0x00041745
		protected override void FinishSetup()
		{
			this.leftTriggerButton = base.GetChildControl<ButtonControl>("leftTriggerButton");
			this.rightTriggerButton = base.GetChildControl<ButtonControl>("rightTriggerButton");
			this.playStationButton = base.GetChildControl<ButtonControl>("systemButton");
			base.FinishSetup();
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x00043580 File Offset: 0x00041780
		public override void PauseHaptics()
		{
			if (this.m_LowFrequencyMotorSpeed == null && this.m_HighFrequenceyMotorSpeed == null && this.m_LightBarColor == null)
			{
				return;
			}
			DualShockHIDOutputReport dualShockHIDOutputReport = DualShockHIDOutputReport.Create(base.hidDescriptor.outputReportSize);
			dualShockHIDOutputReport.SetMotorSpeeds(0f, 0f);
			if (this.m_LightBarColor != null)
			{
				dualShockHIDOutputReport.SetColor(Color.black);
			}
			base.ExecuteCommand<DualShockHIDOutputReport>(ref dualShockHIDOutputReport);
		}

		// Token: 0x06000CC1 RID: 3265 RVA: 0x000435FC File Offset: 0x000417FC
		public override void ResetHaptics()
		{
			if (this.m_LowFrequencyMotorSpeed == null && this.m_HighFrequenceyMotorSpeed == null && this.m_LightBarColor == null)
			{
				return;
			}
			DualShockHIDOutputReport dualShockHIDOutputReport = DualShockHIDOutputReport.Create(base.hidDescriptor.outputReportSize);
			dualShockHIDOutputReport.SetMotorSpeeds(0f, 0f);
			if (this.m_LightBarColor != null)
			{
				dualShockHIDOutputReport.SetColor(Color.black);
			}
			base.ExecuteCommand<DualShockHIDOutputReport>(ref dualShockHIDOutputReport);
			this.m_HighFrequenceyMotorSpeed = null;
			this.m_LowFrequencyMotorSpeed = null;
			this.m_LightBarColor = null;
		}

		// Token: 0x06000CC2 RID: 3266 RVA: 0x0004369C File Offset: 0x0004189C
		public override void ResumeHaptics()
		{
			if (this.m_LowFrequencyMotorSpeed == null && this.m_HighFrequenceyMotorSpeed == null && this.m_LightBarColor == null)
			{
				return;
			}
			DualShockHIDOutputReport dualShockHIDOutputReport = DualShockHIDOutputReport.Create(base.hidDescriptor.outputReportSize);
			if (this.m_LowFrequencyMotorSpeed != null || this.m_HighFrequenceyMotorSpeed != null)
			{
				dualShockHIDOutputReport.SetMotorSpeeds(this.m_LowFrequencyMotorSpeed.Value, this.m_HighFrequenceyMotorSpeed.Value);
			}
			if (this.m_LightBarColor != null)
			{
				dualShockHIDOutputReport.SetColor(this.m_LightBarColor.Value);
			}
			base.ExecuteCommand<DualShockHIDOutputReport>(ref dualShockHIDOutputReport);
		}

		// Token: 0x06000CC3 RID: 3267 RVA: 0x00043744 File Offset: 0x00041944
		public override void SetLightBarColor(Color color)
		{
			DualShockHIDOutputReport dualShockHIDOutputReport = DualShockHIDOutputReport.Create(base.hidDescriptor.outputReportSize);
			dualShockHIDOutputReport.SetColor(color);
			base.ExecuteCommand<DualShockHIDOutputReport>(ref dualShockHIDOutputReport);
			this.m_LightBarColor = new Color?(color);
		}

		// Token: 0x06000CC4 RID: 3268 RVA: 0x00043780 File Offset: 0x00041980
		public override void SetMotorSpeeds(float lowFrequency, float highFrequency)
		{
			DualShockHIDOutputReport dualShockHIDOutputReport = DualShockHIDOutputReport.Create(base.hidDescriptor.outputReportSize);
			dualShockHIDOutputReport.SetMotorSpeeds(lowFrequency, highFrequency);
			base.ExecuteCommand<DualShockHIDOutputReport>(ref dualShockHIDOutputReport);
			this.m_LowFrequencyMotorSpeed = new float?(lowFrequency);
			this.m_HighFrequenceyMotorSpeed = new float?(highFrequency);
		}

		// Token: 0x06000CC5 RID: 3269 RVA: 0x000437C8 File Offset: 0x000419C8
		public bool SetMotorSpeedsAndLightBarColor(float lowFrequency, float highFrequency, Color color)
		{
			DualShockHIDOutputReport dualShockHIDOutputReport = DualShockHIDOutputReport.Create(base.hidDescriptor.outputReportSize);
			dualShockHIDOutputReport.SetMotorSpeeds(lowFrequency, highFrequency);
			dualShockHIDOutputReport.SetColor(color);
			long num = base.ExecuteCommand<DualShockHIDOutputReport>(ref dualShockHIDOutputReport);
			this.m_LowFrequencyMotorSpeed = new float?(lowFrequency);
			this.m_HighFrequenceyMotorSpeed = new float?(highFrequency);
			this.m_LightBarColor = new Color?(color);
			return num >= 0L;
		}

		// Token: 0x06000CC6 RID: 3270 RVA: 0x0004382C File Offset: 0x00041A2C
		unsafe bool IEventPreProcessor.PreProcessEvent(InputEventPtr eventPtr)
		{
			if (eventPtr.type != 1398030676)
			{
				return eventPtr.type != 1145852993;
			}
			StateEvent* ptr = StateEvent.FromUnchecked(eventPtr);
			if (ptr->stateFormat == DualShock4HIDInputReport.Format)
			{
				return true;
			}
			uint stateSizeInBytes = ptr->stateSizeInBytes;
			if (ptr->stateFormat != DualShock4GamepadHID.DualShock4HIDGenericInputReport.Format || (ulong)stateSizeInBytes < (ulong)((long)sizeof(DualShock4GamepadHID.DualShock4HIDGenericInputReport)))
			{
				return false;
			}
			byte* state = (byte*)ptr->state;
			byte b = *state;
			if (b != 1)
			{
				if (b - 17 > 8)
				{
					return false;
				}
				if ((state[1] & 128) == 0)
				{
					return false;
				}
				if ((ulong)stateSizeInBytes < (ulong)((long)(sizeof(DualShock4GamepadHID.DualShock4HIDGenericInputReport) + 3)))
				{
					return false;
				}
				DualShock4HIDInputReport dualShock4HIDInputReport = ((DualShock4GamepadHID.DualShock4HIDGenericInputReport*)(state + 3))->ToHIDInputReport();
				*(DualShock4HIDInputReport*)ptr->state = dualShock4HIDInputReport;
				ptr->stateFormat = DualShock4HIDInputReport.Format;
				return true;
			}
			else
			{
				if ((ulong)stateSizeInBytes < (ulong)((long)(sizeof(DualShock4GamepadHID.DualShock4HIDGenericInputReport) + 1)))
				{
					return false;
				}
				DualShock4HIDInputReport dualShock4HIDInputReport2 = ((DualShock4GamepadHID.DualShock4HIDGenericInputReport*)(state + 1))->ToHIDInputReport();
				*(DualShock4HIDInputReport*)ptr->state = dualShock4HIDInputReport2;
				ptr->stateFormat = DualShock4HIDInputReport.Format;
				return true;
			}
		}

		// Token: 0x06000CC7 RID: 3271 RVA: 0x00043935 File Offset: 0x00041B35
		public void OnNextUpdate()
		{
		}

		// Token: 0x06000CC8 RID: 3272 RVA: 0x00043938 File Offset: 0x00041B38
		public unsafe void OnStateEvent(InputEventPtr eventPtr)
		{
			if (eventPtr.type == 1398030676 && eventPtr.stateFormat == DualShock4HIDInputReport.Format)
			{
				DualShock4HIDInputReport* ptr = (DualShock4HIDInputReport*)((byte*)base.currentStatePtr + this.m_StateBlock.byteOffset);
				DualShock4HIDInputReport* state = (DualShock4HIDInputReport*)StateEvent.FromUnchecked(eventPtr)->state;
				if (state->leftStickX >= 120 && state->leftStickX <= 135 && state->leftStickY >= 120 && state->leftStickY <= 135 && state->rightStickX >= 120 && state->rightStickX <= 135 && state->rightStickY >= 120 && state->rightStickY <= 135 && state->leftTrigger == ptr->leftTrigger && state->rightTrigger == ptr->rightTrigger && state->buttons1 == ptr->buttons1 && state->buttons2 == ptr->buttons2 && state->buttons3 == ptr->buttons3)
				{
					InputSystem.s_Manager.DontMakeCurrentlyUpdatingDeviceCurrent();
				}
			}
			InputState.Change(this, eventPtr, InputUpdateType.None);
		}

		// Token: 0x06000CC9 RID: 3273 RVA: 0x00043A5B File Offset: 0x00041C5B
		public bool GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
		{
			return false;
		}

		// Token: 0x040004A9 RID: 1193
		private float? m_LowFrequencyMotorSpeed;

		// Token: 0x040004AA RID: 1194
		private float? m_HighFrequenceyMotorSpeed;

		// Token: 0x040004AB RID: 1195
		private Color? m_LightBarColor;

		// Token: 0x040004AC RID: 1196
		internal const byte JitterMaskLow = 120;

		// Token: 0x040004AD RID: 1197
		internal const byte JitterMaskHigh = 135;

		// Token: 0x02000201 RID: 513
		[StructLayout(LayoutKind.Explicit)]
		internal struct DualShock4HIDGenericInputReport
		{
			// Token: 0x170005A3 RID: 1443
			// (get) Token: 0x060014F8 RID: 5368 RVA: 0x00062116 File Offset: 0x00060316
			public static FourCC Format
			{
				get
				{
					return new FourCC('H', 'I', 'D', ' ');
				}
			}

			// Token: 0x060014F9 RID: 5369 RVA: 0x00062128 File Offset: 0x00060328
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public DualShock4HIDInputReport ToHIDInputReport()
			{
				return new DualShock4HIDInputReport
				{
					leftStickX = this.leftStickX,
					leftStickY = this.leftStickY,
					rightStickX = this.rightStickX,
					rightStickY = this.rightStickY,
					leftTrigger = this.leftTrigger,
					rightTrigger = this.rightTrigger,
					buttons1 = this.buttons0,
					buttons2 = this.buttons1,
					buttons3 = this.buttons2
				};
			}

			// Token: 0x04000B6E RID: 2926
			[FieldOffset(0)]
			public byte leftStickX;

			// Token: 0x04000B6F RID: 2927
			[FieldOffset(1)]
			public byte leftStickY;

			// Token: 0x04000B70 RID: 2928
			[FieldOffset(2)]
			public byte rightStickX;

			// Token: 0x04000B71 RID: 2929
			[FieldOffset(3)]
			public byte rightStickY;

			// Token: 0x04000B72 RID: 2930
			[FieldOffset(4)]
			public byte buttons0;

			// Token: 0x04000B73 RID: 2931
			[FieldOffset(5)]
			public byte buttons1;

			// Token: 0x04000B74 RID: 2932
			[FieldOffset(6)]
			public byte buttons2;

			// Token: 0x04000B75 RID: 2933
			[FieldOffset(7)]
			public byte leftTrigger;

			// Token: 0x04000B76 RID: 2934
			[FieldOffset(8)]
			public byte rightTrigger;
		}
	}
}
