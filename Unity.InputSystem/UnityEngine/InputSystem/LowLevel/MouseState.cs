using System;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000D2 RID: 210
	[StructLayout(LayoutKind.Explicit, Size = 30)]
	public struct MouseState : IInputStateTypeInfo
	{
		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06000D5C RID: 3420 RVA: 0x000449A4 File Offset: 0x00042BA4
		public static FourCC Format
		{
			get
			{
				return new FourCC('M', 'O', 'U', 'S');
			}
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x000449B4 File Offset: 0x00042BB4
		public MouseState WithButton(MouseButton button, bool state = true)
		{
			uint num = 1U << (int)button;
			if (state)
			{
				this.buttons |= (ushort)num;
			}
			else
			{
				this.buttons &= (ushort)(~(ushort)num);
			}
			return this;
		}

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06000D5E RID: 3422 RVA: 0x000449F4 File Offset: 0x00042BF4
		public FourCC format
		{
			get
			{
				return MouseState.Format;
			}
		}

		// Token: 0x0400055C RID: 1372
		[InputControl(usage = "Point", dontReset = true)]
		[FieldOffset(0)]
		public Vector2 position;

		// Token: 0x0400055D RID: 1373
		[InputControl(usage = "Secondary2DMotion", layout = "Delta")]
		[FieldOffset(8)]
		public Vector2 delta;

		// Token: 0x0400055E RID: 1374
		[InputControl(displayName = "Scroll", layout = "Delta")]
		[InputControl(name = "scroll/x", aliases = new string[] { "horizontal" }, usage = "ScrollHorizontal", displayName = "Left/Right")]
		[InputControl(name = "scroll/y", aliases = new string[] { "vertical" }, usage = "ScrollVertical", displayName = "Up/Down", shortDisplayName = "Wheel")]
		[FieldOffset(16)]
		public Vector2 scroll;

		// Token: 0x0400055F RID: 1375
		[InputControl(name = "press", useStateFrom = "leftButton", synthetic = true, usages = new string[] { })]
		[InputControl(name = "leftButton", layout = "Button", bit = 0U, usage = "PrimaryAction", displayName = "Left Button", shortDisplayName = "LMB")]
		[InputControl(name = "rightButton", layout = "Button", bit = 1U, usage = "SecondaryAction", displayName = "Right Button", shortDisplayName = "RMB")]
		[InputControl(name = "middleButton", layout = "Button", bit = 2U, displayName = "Middle Button", shortDisplayName = "MMB")]
		[InputControl(name = "forwardButton", layout = "Button", bit = 3U, usage = "Forward", displayName = "Forward")]
		[InputControl(name = "backButton", layout = "Button", bit = 4U, usage = "Back", displayName = "Back")]
		[InputControl(name = "pressure", layout = "Axis", usage = "Pressure", offset = 4294967294U, format = "FLT", sizeInBits = 32U)]
		[InputControl(name = "radius", layout = "Vector2", usage = "Radius", offset = 4294967294U, format = "VEC2", sizeInBits = 64U)]
		[InputControl(name = "pointerId", layout = "Digital", format = "BIT", sizeInBits = 1U, offset = 4294967294U)]
		[FieldOffset(24)]
		public ushort buttons;

		// Token: 0x04000560 RID: 1376
		[InputControl(name = "displayIndex", layout = "Integer", displayName = "Display Index")]
		[FieldOffset(26)]
		public ushort displayIndex;

		// Token: 0x04000561 RID: 1377
		[InputControl(name = "clickCount", layout = "Integer", displayName = "Click Count", synthetic = true)]
		[FieldOffset(28)]
		public ushort clickCount;
	}
}
