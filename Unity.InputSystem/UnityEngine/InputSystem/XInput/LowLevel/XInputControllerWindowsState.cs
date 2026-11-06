using System;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.XInput.LowLevel
{
	// Token: 0x0200007F RID: 127
	[StructLayout(LayoutKind.Explicit, Size = 4)]
	internal struct XInputControllerWindowsState : IInputStateTypeInfo
	{
		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000A8F RID: 2703 RVA: 0x00038E8B File Offset: 0x0003708B
		public FourCC format
		{
			get
			{
				return new FourCC('X', 'I', 'N', 'P');
			}
		}

		// Token: 0x06000A90 RID: 2704 RVA: 0x00038E9A File Offset: 0x0003709A
		public XInputControllerWindowsState WithButton(XInputControllerWindowsState.Button button)
		{
			this.buttons |= (ushort)(1 << (int)button);
			return this;
		}

		// Token: 0x040003B4 RID: 948
		[InputControl(name = "dpad", layout = "Dpad", sizeInBits = 4U, bit = 0U)]
		[InputControl(name = "dpad/up", bit = 0U)]
		[InputControl(name = "dpad/down", bit = 1U)]
		[InputControl(name = "dpad/left", bit = 2U)]
		[InputControl(name = "dpad/right", bit = 3U)]
		[InputControl(name = "start", bit = 4U, displayName = "Start")]
		[InputControl(name = "select", bit = 5U, displayName = "Select")]
		[InputControl(name = "leftStickPress", bit = 6U)]
		[InputControl(name = "rightStickPress", bit = 7U)]
		[InputControl(name = "leftShoulder", bit = 8U)]
		[InputControl(name = "rightShoulder", bit = 9U)]
		[InputControl(name = "buttonSouth", bit = 12U, displayName = "A")]
		[InputControl(name = "buttonEast", bit = 13U, displayName = "B")]
		[InputControl(name = "buttonWest", bit = 14U, displayName = "X")]
		[InputControl(name = "buttonNorth", bit = 15U, displayName = "Y")]
		[FieldOffset(0)]
		public ushort buttons;

		// Token: 0x040003B5 RID: 949
		[InputControl(name = "leftTrigger", format = "BYTE")]
		[FieldOffset(2)]
		public byte leftTrigger;

		// Token: 0x040003B6 RID: 950
		[InputControl(name = "rightTrigger", format = "BYTE")]
		[FieldOffset(3)]
		public byte rightTrigger;

		// Token: 0x040003B7 RID: 951
		[InputControl(name = "leftStick", layout = "Stick", format = "VC2S")]
		[InputControl(name = "leftStick/x", offset = 0U, format = "SHRT", parameters = "clamp=false,invert=false,normalize=false")]
		[InputControl(name = "leftStick/left", offset = 0U, format = "SHRT")]
		[InputControl(name = "leftStick/right", offset = 0U, format = "SHRT")]
		[InputControl(name = "leftStick/y", offset = 2U, format = "SHRT", parameters = "clamp=false,invert=false,normalize=false")]
		[InputControl(name = "leftStick/up", offset = 2U, format = "SHRT")]
		[InputControl(name = "leftStick/down", offset = 2U, format = "SHRT")]
		[FieldOffset(4)]
		public short leftStickX;

		// Token: 0x040003B8 RID: 952
		[FieldOffset(6)]
		public short leftStickY;

		// Token: 0x040003B9 RID: 953
		[InputControl(name = "rightStick", layout = "Stick", format = "VC2S")]
		[InputControl(name = "rightStick/x", offset = 0U, format = "SHRT", parameters = "clamp=false,invert=false,normalize=false")]
		[InputControl(name = "rightStick/left", offset = 0U, format = "SHRT")]
		[InputControl(name = "rightStick/right", offset = 0U, format = "SHRT")]
		[InputControl(name = "rightStick/y", offset = 2U, format = "SHRT", parameters = "clamp=false,invert=false,normalize=false")]
		[InputControl(name = "rightStick/up", offset = 2U, format = "SHRT")]
		[InputControl(name = "rightStick/down", offset = 2U, format = "SHRT")]
		[FieldOffset(8)]
		public short rightStickX;

		// Token: 0x040003BA RID: 954
		[FieldOffset(10)]
		public short rightStickY;

		// Token: 0x020001CE RID: 462
		public enum Button
		{
			// Token: 0x04000982 RID: 2434
			DPadUp,
			// Token: 0x04000983 RID: 2435
			DPadDown,
			// Token: 0x04000984 RID: 2436
			DPadLeft,
			// Token: 0x04000985 RID: 2437
			DPadRight,
			// Token: 0x04000986 RID: 2438
			Start,
			// Token: 0x04000987 RID: 2439
			Select,
			// Token: 0x04000988 RID: 2440
			LeftThumbstickPress,
			// Token: 0x04000989 RID: 2441
			RightThumbstickPress,
			// Token: 0x0400098A RID: 2442
			LeftShoulder,
			// Token: 0x0400098B RID: 2443
			RightShoulder,
			// Token: 0x0400098C RID: 2444
			A = 12,
			// Token: 0x0400098D RID: 2445
			B,
			// Token: 0x0400098E RID: 2446
			X,
			// Token: 0x0400098F RID: 2447
			Y
		}
	}
}
