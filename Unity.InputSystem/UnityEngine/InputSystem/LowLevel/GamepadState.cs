using System;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000C8 RID: 200
	[StructLayout(LayoutKind.Explicit, Size = 28)]
	public struct GamepadState : IInputStateTypeInfo
	{
		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06000D45 RID: 3397 RVA: 0x000447C5 File Offset: 0x000429C5
		public static FourCC Format
		{
			get
			{
				return new FourCC('G', 'P', 'A', 'D');
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06000D46 RID: 3398 RVA: 0x000447D4 File Offset: 0x000429D4
		public FourCC format
		{
			get
			{
				return GamepadState.Format;
			}
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x000447DC File Offset: 0x000429DC
		public GamepadState(params GamepadButton[] buttons)
		{
			this = default(GamepadState);
			if (buttons == null)
			{
				throw new ArgumentNullException("buttons");
			}
			foreach (GamepadButton gamepadButton in buttons)
			{
				uint num = 1U << (int)gamepadButton;
				this.buttons |= num;
			}
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x00044828 File Offset: 0x00042A28
		public GamepadState WithButton(GamepadButton button, bool value = true)
		{
			uint num = 1U << (int)button;
			if (value)
			{
				this.buttons |= num;
			}
			else
			{
				this.buttons &= ~num;
			}
			return this;
		}

		// Token: 0x04000531 RID: 1329
		internal const string ButtonSouthShortDisplayName = "A";

		// Token: 0x04000532 RID: 1330
		internal const string ButtonNorthShortDisplayName = "Y";

		// Token: 0x04000533 RID: 1331
		internal const string ButtonWestShortDisplayName = "X";

		// Token: 0x04000534 RID: 1332
		internal const string ButtonEastShortDisplayName = "B";

		// Token: 0x04000535 RID: 1333
		[InputControl(name = "dpad", layout = "Dpad", usage = "Hatswitch", displayName = "D-Pad", format = "BIT", sizeInBits = 4U, bit = 0U)]
		[InputControl(name = "buttonSouth", layout = "Button", bit = 6U, usages = new string[] { "PrimaryAction", "Submit" }, aliases = new string[] { "a", "cross" }, displayName = "Button South", shortDisplayName = "A")]
		[InputControl(name = "buttonWest", layout = "Button", bit = 7U, usage = "SecondaryAction", aliases = new string[] { "x", "square" }, displayName = "Button West", shortDisplayName = "X")]
		[InputControl(name = "buttonNorth", layout = "Button", bit = 4U, aliases = new string[] { "y", "triangle" }, displayName = "Button North", shortDisplayName = "Y")]
		[InputControl(name = "buttonEast", layout = "Button", bit = 5U, usages = new string[] { "Back", "Cancel" }, aliases = new string[] { "b", "circle" }, displayName = "Button East", shortDisplayName = "B")]
		[InputControl(name = "leftStickPress", layout = "Button", bit = 8U, displayName = "Left Stick Press")]
		[InputControl(name = "rightStickPress", layout = "Button", bit = 9U, displayName = "Right Stick Press")]
		[InputControl(name = "leftShoulder", layout = "Button", bit = 10U, displayName = "Left Shoulder", shortDisplayName = "LB")]
		[InputControl(name = "rightShoulder", layout = "Button", bit = 11U, displayName = "Right Shoulder", shortDisplayName = "RB")]
		[InputControl(name = "start", layout = "Button", bit = 12U, usage = "Menu", displayName = "Start")]
		[InputControl(name = "select", layout = "Button", bit = 13U, displayName = "Select")]
		[FieldOffset(0)]
		public uint buttons;

		// Token: 0x04000536 RID: 1334
		[InputControl(layout = "Stick", usage = "Primary2DMotion", processors = "stickDeadzone", displayName = "Left Stick", shortDisplayName = "LS")]
		[FieldOffset(4)]
		public Vector2 leftStick;

		// Token: 0x04000537 RID: 1335
		[InputControl(layout = "Stick", usage = "Secondary2DMotion", processors = "stickDeadzone", displayName = "Right Stick", shortDisplayName = "RS")]
		[FieldOffset(12)]
		public Vector2 rightStick;

		// Token: 0x04000538 RID: 1336
		[InputControl(layout = "Button", format = "FLT", usage = "SecondaryTrigger", displayName = "Left Trigger", shortDisplayName = "LT")]
		[FieldOffset(20)]
		public float leftTrigger;

		// Token: 0x04000539 RID: 1337
		[InputControl(layout = "Button", format = "FLT", usage = "SecondaryTrigger", displayName = "Right Trigger", shortDisplayName = "RT")]
		[FieldOffset(24)]
		public float rightTrigger;
	}
}
