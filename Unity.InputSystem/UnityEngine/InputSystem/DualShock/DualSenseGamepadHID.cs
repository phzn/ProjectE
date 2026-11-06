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
	// Token: 0x020000A3 RID: 163
	[InputControlLayout(stateType = typeof(DualSenseHIDInputReport), displayName = "DualSense HID")]
	public class DualSenseGamepadHID : DualShockGamepad, IEventMerger, IEventPreProcessor, IInputStateCallbackReceiver
	{
		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06000CA3 RID: 3235 RVA: 0x00042E7C File Offset: 0x0004107C
		// (set) Token: 0x06000CA4 RID: 3236 RVA: 0x00042E84 File Offset: 0x00041084
		public ButtonControl leftTriggerButton { get; protected set; }

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06000CA5 RID: 3237 RVA: 0x00042E8D File Offset: 0x0004108D
		// (set) Token: 0x06000CA6 RID: 3238 RVA: 0x00042E95 File Offset: 0x00041095
		public ButtonControl rightTriggerButton { get; protected set; }

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06000CA7 RID: 3239 RVA: 0x00042E9E File Offset: 0x0004109E
		// (set) Token: 0x06000CA8 RID: 3240 RVA: 0x00042EA6 File Offset: 0x000410A6
		public ButtonControl playStationButton { get; protected set; }

		// Token: 0x06000CA9 RID: 3241 RVA: 0x00042EAF File Offset: 0x000410AF
		protected override void FinishSetup()
		{
			this.leftTriggerButton = base.GetChildControl<ButtonControl>("leftTriggerButton");
			this.rightTriggerButton = base.GetChildControl<ButtonControl>("rightTriggerButton");
			this.playStationButton = base.GetChildControl<ButtonControl>("systemButton");
			base.FinishSetup();
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x00042EEA File Offset: 0x000410EA
		public override void PauseHaptics()
		{
			if (this.m_LowFrequencyMotorSpeed == null && this.m_HighFrequenceyMotorSpeed == null)
			{
				return;
			}
			this.SetMotorSpeedsAndLightBarColor(new float?(0f), new float?(0f), this.m_LightBarColor);
		}

		// Token: 0x06000CAB RID: 3243 RVA: 0x00042F28 File Offset: 0x00041128
		public override void ResetHaptics()
		{
			if (this.m_LowFrequencyMotorSpeed == null && this.m_HighFrequenceyMotorSpeed == null)
			{
				return;
			}
			this.m_HighFrequenceyMotorSpeed = null;
			this.m_LowFrequencyMotorSpeed = null;
			this.SetMotorSpeedsAndLightBarColor(this.m_LowFrequencyMotorSpeed, this.m_HighFrequenceyMotorSpeed, this.m_LightBarColor);
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x00042F81 File Offset: 0x00041181
		public override void ResumeHaptics()
		{
			if (this.m_LowFrequencyMotorSpeed == null && this.m_HighFrequenceyMotorSpeed == null)
			{
				return;
			}
			this.SetMotorSpeedsAndLightBarColor(this.m_LowFrequencyMotorSpeed, this.m_HighFrequenceyMotorSpeed, this.m_LightBarColor);
		}

		// Token: 0x06000CAD RID: 3245 RVA: 0x00042FB7 File Offset: 0x000411B7
		public override void SetLightBarColor(Color color)
		{
			this.m_LightBarColor = new Color?(color);
			this.SetMotorSpeedsAndLightBarColor(this.m_LowFrequencyMotorSpeed, this.m_HighFrequenceyMotorSpeed, this.m_LightBarColor);
		}

		// Token: 0x06000CAE RID: 3246 RVA: 0x00042FDE File Offset: 0x000411DE
		public override void SetMotorSpeeds(float lowFrequency, float highFrequency)
		{
			this.m_LowFrequencyMotorSpeed = new float?(lowFrequency);
			this.m_HighFrequenceyMotorSpeed = new float?(highFrequency);
			this.SetMotorSpeedsAndLightBarColor(this.m_LowFrequencyMotorSpeed, this.m_HighFrequenceyMotorSpeed, this.m_LightBarColor);
		}

		// Token: 0x06000CAF RID: 3247 RVA: 0x00043014 File Offset: 0x00041214
		public bool SetMotorSpeedsAndLightBarColor(float? lowFrequency, float? highFrequency, Color? color)
		{
			float num = ((lowFrequency != null) ? lowFrequency.Value : 0f);
			float num2 = ((highFrequency != null) ? highFrequency.Value : 0f);
			Color color2 = ((color != null) ? color.Value : Color.black);
			DualSenseHIDUSBOutputReport dualSenseHIDUSBOutputReport = DualSenseHIDUSBOutputReport.Create(new DualSenseHIDOutputReportPayload
			{
				enableFlags1 = 3,
				enableFlags2 = 4,
				lowFrequencyMotorSpeed = (byte)NumberHelpers.NormalizedFloatToUInt(num, 0U, 255U),
				highFrequencyMotorSpeed = (byte)NumberHelpers.NormalizedFloatToUInt(num2, 0U, 255U),
				redColor = (byte)NumberHelpers.NormalizedFloatToUInt(color2.r, 0U, 255U),
				greenColor = (byte)NumberHelpers.NormalizedFloatToUInt(color2.g, 0U, 255U),
				blueColor = (byte)NumberHelpers.NormalizedFloatToUInt(color2.b, 0U, 255U)
			}, base.hidDescriptor.outputReportSize);
			return base.ExecuteCommand<DualSenseHIDUSBOutputReport>(ref dualSenseHIDUSBOutputReport) >= 0L;
		}

		// Token: 0x06000CB0 RID: 3248 RVA: 0x00043116 File Offset: 0x00041316
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static bool MergeForward(DualSenseGamepadHID.DualSenseHIDUSBInputReport* currentState, DualSenseGamepadHID.DualSenseHIDUSBInputReport* nextState)
		{
			return currentState->buttons0 == nextState->buttons0 && currentState->buttons1 == nextState->buttons1 && currentState->buttons2 == nextState->buttons2;
		}

		// Token: 0x06000CB1 RID: 3249 RVA: 0x00043144 File Offset: 0x00041344
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static bool MergeForward(DualSenseGamepadHID.DualSenseHIDBluetoothInputReport* currentState, DualSenseGamepadHID.DualSenseHIDBluetoothInputReport* nextState)
		{
			return currentState->buttons0 == nextState->buttons0 && currentState->buttons1 == nextState->buttons1 && currentState->buttons2 == nextState->buttons2;
		}

		// Token: 0x06000CB2 RID: 3250 RVA: 0x00043172 File Offset: 0x00041372
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static bool MergeForward(DualSenseGamepadHID.DualSenseHIDMinimalInputReport* currentState, DualSenseGamepadHID.DualSenseHIDMinimalInputReport* nextState)
		{
			return currentState->buttons0 == nextState->buttons0 && currentState->buttons1 == nextState->buttons1 && currentState->buttons2 == nextState->buttons2;
		}

		// Token: 0x06000CB3 RID: 3251 RVA: 0x000431A0 File Offset: 0x000413A0
		unsafe bool IEventMerger.MergeForward(InputEventPtr currentEventPtr, InputEventPtr nextEventPtr)
		{
			if (currentEventPtr.type != 1398030676 || nextEventPtr.type != 1398030676)
			{
				return false;
			}
			StateEvent* ptr = StateEvent.FromUnchecked(currentEventPtr);
			StateEvent* ptr2 = StateEvent.FromUnchecked(nextEventPtr);
			if (ptr->stateFormat != DualSenseGamepadHID.DualSenseHIDGenericInputReport.Format || ptr2->stateFormat != DualSenseGamepadHID.DualSenseHIDGenericInputReport.Format)
			{
				return false;
			}
			if (ptr->stateSizeInBytes != ptr2->stateSizeInBytes)
			{
				return false;
			}
			DualSenseGamepadHID.DualSenseHIDGenericInputReport* state = (DualSenseGamepadHID.DualSenseHIDGenericInputReport*)ptr->state;
			DualSenseGamepadHID.DualSenseHIDGenericInputReport* state2 = (DualSenseGamepadHID.DualSenseHIDGenericInputReport*)ptr2->state;
			if (state->reportId != state2->reportId)
			{
				return false;
			}
			if (state->reportId == 1)
			{
				if ((ulong)ptr->stateSizeInBytes == (ulong)((long)DualSenseGamepadHID.DualSenseHIDMinimalInputReport.ExpectedSize1) || (ulong)ptr->stateSizeInBytes == (ulong)((long)DualSenseGamepadHID.DualSenseHIDMinimalInputReport.ExpectedSize2))
				{
					DualSenseGamepadHID.DualSenseHIDMinimalInputReport* state3 = (DualSenseGamepadHID.DualSenseHIDMinimalInputReport*)ptr->state;
					DualSenseGamepadHID.DualSenseHIDMinimalInputReport* state4 = (DualSenseGamepadHID.DualSenseHIDMinimalInputReport*)ptr2->state;
					return DualSenseGamepadHID.MergeForward(state3, state4);
				}
				DualSenseGamepadHID.DualSenseHIDUSBInputReport* state5 = (DualSenseGamepadHID.DualSenseHIDUSBInputReport*)ptr->state;
				DualSenseGamepadHID.DualSenseHIDUSBInputReport* state6 = (DualSenseGamepadHID.DualSenseHIDUSBInputReport*)ptr2->state;
				return DualSenseGamepadHID.MergeForward(state5, state6);
			}
			else
			{
				if (state->reportId == 49)
				{
					DualSenseGamepadHID.DualSenseHIDBluetoothInputReport* state7 = (DualSenseGamepadHID.DualSenseHIDBluetoothInputReport*)ptr->state;
					DualSenseGamepadHID.DualSenseHIDBluetoothInputReport* state8 = (DualSenseGamepadHID.DualSenseHIDBluetoothInputReport*)ptr2->state;
					return DualSenseGamepadHID.MergeForward(state7, state8);
				}
				return false;
			}
		}

		// Token: 0x06000CB4 RID: 3252 RVA: 0x000432C4 File Offset: 0x000414C4
		unsafe bool IEventPreProcessor.PreProcessEvent(InputEventPtr eventPtr)
		{
			if (eventPtr.type != 1398030676)
			{
				return eventPtr.type != 1145852993;
			}
			StateEvent* ptr = StateEvent.FromUnchecked(eventPtr);
			if (ptr->stateFormat == DualSenseHIDInputReport.Format)
			{
				return true;
			}
			uint stateSizeInBytes = ptr->stateSizeInBytes;
			if (ptr->stateFormat != DualSenseGamepadHID.DualSenseHIDGenericInputReport.Format || (ulong)stateSizeInBytes < (ulong)((long)sizeof(DualSenseHIDInputReport)))
			{
				return false;
			}
			DualSenseGamepadHID.DualSenseHIDGenericInputReport* state = (DualSenseGamepadHID.DualSenseHIDGenericInputReport*)ptr->state;
			if (state->reportId == 1)
			{
				if ((ulong)ptr->stateSizeInBytes == (ulong)((long)DualSenseGamepadHID.DualSenseHIDMinimalInputReport.ExpectedSize1) || (ulong)ptr->stateSizeInBytes == (ulong)((long)DualSenseGamepadHID.DualSenseHIDMinimalInputReport.ExpectedSize2))
				{
					DualSenseHIDInputReport dualSenseHIDInputReport = ((DualSenseGamepadHID.DualSenseHIDMinimalInputReport*)ptr->state)->ToHIDInputReport();
					*(DualSenseHIDInputReport*)ptr->state = dualSenseHIDInputReport;
				}
				else
				{
					DualSenseHIDInputReport dualSenseHIDInputReport2 = ((DualSenseGamepadHID.DualSenseHIDUSBInputReport*)ptr->state)->ToHIDInputReport();
					*(DualSenseHIDInputReport*)ptr->state = dualSenseHIDInputReport2;
				}
				ptr->stateFormat = DualSenseHIDInputReport.Format;
				return true;
			}
			if (state->reportId == 49)
			{
				DualSenseHIDInputReport dualSenseHIDInputReport3 = ((DualSenseGamepadHID.DualSenseHIDBluetoothInputReport*)ptr->state)->ToHIDInputReport();
				*(DualSenseHIDInputReport*)ptr->state = dualSenseHIDInputReport3;
				ptr->stateFormat = DualSenseHIDInputReport.Format;
				return true;
			}
			return false;
		}

		// Token: 0x06000CB5 RID: 3253 RVA: 0x000433E2 File Offset: 0x000415E2
		public void OnNextUpdate()
		{
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x000433E4 File Offset: 0x000415E4
		public unsafe void OnStateEvent(InputEventPtr eventPtr)
		{
			if (eventPtr.type == 1398030676 && eventPtr.stateFormat == DualSenseHIDInputReport.Format)
			{
				DualSenseHIDInputReport* ptr = (DualSenseHIDInputReport*)((byte*)base.currentStatePtr + this.m_StateBlock.byteOffset);
				DualSenseHIDInputReport* state = (DualSenseHIDInputReport*)StateEvent.FromUnchecked(eventPtr)->state;
				if (state->leftStickX >= 120 && state->leftStickX <= 135 && state->leftStickY >= 120 && state->leftStickY <= 135 && state->rightStickX >= 120 && state->rightStickX <= 135 && state->rightStickY >= 120 && state->rightStickY <= 135 && state->leftTrigger == ptr->leftTrigger && state->rightTrigger == ptr->rightTrigger && state->buttons0 == ptr->buttons0 && state->buttons1 == ptr->buttons1 && state->buttons2 == ptr->buttons2)
				{
					InputSystem.s_Manager.DontMakeCurrentlyUpdatingDeviceCurrent();
				}
			}
			InputState.Change(this, eventPtr, InputUpdateType.None);
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x00043507 File Offset: 0x00041707
		public bool GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
		{
			return false;
		}

		// Token: 0x040004A0 RID: 1184
		private float? m_LowFrequencyMotorSpeed;

		// Token: 0x040004A1 RID: 1185
		private float? m_HighFrequenceyMotorSpeed;

		// Token: 0x040004A2 RID: 1186
		protected Color? m_LightBarColor;

		// Token: 0x040004A3 RID: 1187
		private byte outputSequenceId;

		// Token: 0x040004A4 RID: 1188
		internal const byte JitterMaskLow = 120;

		// Token: 0x040004A5 RID: 1189
		internal const byte JitterMaskHigh = 135;

		// Token: 0x020001FD RID: 509
		[StructLayout(LayoutKind.Explicit)]
		internal struct DualSenseHIDGenericInputReport
		{
			// Token: 0x170005A2 RID: 1442
			// (get) Token: 0x060014F3 RID: 5363 RVA: 0x00061F48 File Offset: 0x00060148
			public static FourCC Format
			{
				get
				{
					return new FourCC('H', 'I', 'D', ' ');
				}
			}

			// Token: 0x04000B4B RID: 2891
			[FieldOffset(0)]
			public byte reportId;
		}

		// Token: 0x020001FE RID: 510
		[StructLayout(LayoutKind.Explicit)]
		internal struct DualSenseHIDUSBInputReport
		{
			// Token: 0x060014F4 RID: 5364 RVA: 0x00061F58 File Offset: 0x00060158
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public DualSenseHIDInputReport ToHIDInputReport()
			{
				return new DualSenseHIDInputReport
				{
					leftStickX = this.leftStickX,
					leftStickY = this.leftStickY,
					rightStickX = this.rightStickX,
					rightStickY = this.rightStickY,
					leftTrigger = this.leftTrigger,
					rightTrigger = this.rightTrigger,
					buttons0 = this.buttons0,
					buttons1 = this.buttons1,
					buttons2 = (this.buttons2 & 7)
				};
			}

			// Token: 0x04000B4C RID: 2892
			public const int ExpectedReportId = 1;

			// Token: 0x04000B4D RID: 2893
			[FieldOffset(0)]
			public byte reportId;

			// Token: 0x04000B4E RID: 2894
			[FieldOffset(1)]
			public byte leftStickX;

			// Token: 0x04000B4F RID: 2895
			[FieldOffset(2)]
			public byte leftStickY;

			// Token: 0x04000B50 RID: 2896
			[FieldOffset(3)]
			public byte rightStickX;

			// Token: 0x04000B51 RID: 2897
			[FieldOffset(4)]
			public byte rightStickY;

			// Token: 0x04000B52 RID: 2898
			[FieldOffset(5)]
			public byte leftTrigger;

			// Token: 0x04000B53 RID: 2899
			[FieldOffset(6)]
			public byte rightTrigger;

			// Token: 0x04000B54 RID: 2900
			[FieldOffset(8)]
			public byte buttons0;

			// Token: 0x04000B55 RID: 2901
			[FieldOffset(9)]
			public byte buttons1;

			// Token: 0x04000B56 RID: 2902
			[FieldOffset(10)]
			public byte buttons2;
		}

		// Token: 0x020001FF RID: 511
		[StructLayout(LayoutKind.Explicit)]
		internal struct DualSenseHIDBluetoothInputReport
		{
			// Token: 0x060014F5 RID: 5365 RVA: 0x00061FE8 File Offset: 0x000601E8
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public DualSenseHIDInputReport ToHIDInputReport()
			{
				return new DualSenseHIDInputReport
				{
					leftStickX = this.leftStickX,
					leftStickY = this.leftStickY,
					rightStickX = this.rightStickX,
					rightStickY = this.rightStickY,
					leftTrigger = this.leftTrigger,
					rightTrigger = this.rightTrigger,
					buttons0 = this.buttons0,
					buttons1 = this.buttons1,
					buttons2 = (this.buttons2 & 7)
				};
			}

			// Token: 0x04000B57 RID: 2903
			public const int ExpectedReportId = 49;

			// Token: 0x04000B58 RID: 2904
			[FieldOffset(0)]
			public byte reportId;

			// Token: 0x04000B59 RID: 2905
			[FieldOffset(2)]
			public byte leftStickX;

			// Token: 0x04000B5A RID: 2906
			[FieldOffset(3)]
			public byte leftStickY;

			// Token: 0x04000B5B RID: 2907
			[FieldOffset(4)]
			public byte rightStickX;

			// Token: 0x04000B5C RID: 2908
			[FieldOffset(5)]
			public byte rightStickY;

			// Token: 0x04000B5D RID: 2909
			[FieldOffset(6)]
			public byte leftTrigger;

			// Token: 0x04000B5E RID: 2910
			[FieldOffset(7)]
			public byte rightTrigger;

			// Token: 0x04000B5F RID: 2911
			[FieldOffset(9)]
			public byte buttons0;

			// Token: 0x04000B60 RID: 2912
			[FieldOffset(10)]
			public byte buttons1;

			// Token: 0x04000B61 RID: 2913
			[FieldOffset(11)]
			public byte buttons2;
		}

		// Token: 0x02000200 RID: 512
		[StructLayout(LayoutKind.Explicit)]
		internal struct DualSenseHIDMinimalInputReport
		{
			// Token: 0x060014F6 RID: 5366 RVA: 0x00062078 File Offset: 0x00060278
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public DualSenseHIDInputReport ToHIDInputReport()
			{
				return new DualSenseHIDInputReport
				{
					leftStickX = this.leftStickX,
					leftStickY = this.leftStickY,
					rightStickX = this.rightStickX,
					rightStickY = this.rightStickY,
					leftTrigger = this.leftTrigger,
					rightTrigger = this.rightTrigger,
					buttons0 = this.buttons0,
					buttons1 = this.buttons1,
					buttons2 = (this.buttons2 & 3)
				};
			}

			// Token: 0x04000B62 RID: 2914
			public static int ExpectedSize1 = 10;

			// Token: 0x04000B63 RID: 2915
			public static int ExpectedSize2 = 78;

			// Token: 0x04000B64 RID: 2916
			[FieldOffset(0)]
			public byte reportId;

			// Token: 0x04000B65 RID: 2917
			[FieldOffset(1)]
			public byte leftStickX;

			// Token: 0x04000B66 RID: 2918
			[FieldOffset(2)]
			public byte leftStickY;

			// Token: 0x04000B67 RID: 2919
			[FieldOffset(3)]
			public byte rightStickX;

			// Token: 0x04000B68 RID: 2920
			[FieldOffset(4)]
			public byte rightStickY;

			// Token: 0x04000B69 RID: 2921
			[FieldOffset(5)]
			public byte buttons0;

			// Token: 0x04000B6A RID: 2922
			[FieldOffset(6)]
			public byte buttons1;

			// Token: 0x04000B6B RID: 2923
			[FieldOffset(7)]
			public byte buttons2;

			// Token: 0x04000B6C RID: 2924
			[FieldOffset(8)]
			public byte leftTrigger;

			// Token: 0x04000B6D RID: 2925
			[FieldOffset(9)]
			public byte rightTrigger;
		}
	}
}
