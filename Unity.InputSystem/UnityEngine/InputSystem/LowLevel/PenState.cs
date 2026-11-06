using System;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000D4 RID: 212
	[StructLayout(LayoutKind.Explicit, Size = 36)]
	public struct PenState : IInputStateTypeInfo
	{
		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06000D5F RID: 3423 RVA: 0x000449FB File Offset: 0x00042BFB
		public static FourCC Format
		{
			get
			{
				return new FourCC('P', 'E', 'N', ' ');
			}
		}

		// Token: 0x06000D60 RID: 3424 RVA: 0x00044A0C File Offset: 0x00042C0C
		public PenState WithButton(PenButton button, bool state = true)
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

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06000D61 RID: 3425 RVA: 0x00044A4C File Offset: 0x00042C4C
		public FourCC format
		{
			get
			{
				return PenState.Format;
			}
		}

		// Token: 0x04000568 RID: 1384
		[InputControl(usage = "Point", dontReset = true)]
		[FieldOffset(0)]
		public Vector2 position;

		// Token: 0x04000569 RID: 1385
		[InputControl(usage = "Secondary2DMotion", layout = "Delta")]
		[FieldOffset(8)]
		public Vector2 delta;

		// Token: 0x0400056A RID: 1386
		[InputControl(layout = "Vector2", displayName = "Tilt", usage = "Tilt")]
		[FieldOffset(16)]
		public Vector2 tilt;

		// Token: 0x0400056B RID: 1387
		[InputControl(layout = "Analog", usage = "Pressure", defaultState = 0f)]
		[FieldOffset(24)]
		public float pressure;

		// Token: 0x0400056C RID: 1388
		[InputControl(layout = "Axis", displayName = "Twist", usage = "Twist")]
		[FieldOffset(28)]
		public float twist;

		// Token: 0x0400056D RID: 1389
		[InputControl(name = "tip", displayName = "Tip", layout = "Button", bit = 0U, usage = "PrimaryAction")]
		[InputControl(name = "press", useStateFrom = "tip", synthetic = true, usages = new string[] { })]
		[InputControl(name = "eraser", displayName = "Eraser", layout = "Button", bit = 1U)]
		[InputControl(name = "inRange", displayName = "In Range?", layout = "Button", bit = 4U, synthetic = true)]
		[InputControl(name = "barrel1", displayName = "Barrel Button #1", layout = "Button", bit = 2U, alias = "barrelFirst", usage = "SecondaryAction")]
		[InputControl(name = "barrel2", displayName = "Barrel Button #2", layout = "Button", bit = 3U, alias = "barrelSecond")]
		[InputControl(name = "barrel3", displayName = "Barrel Button #3", layout = "Button", bit = 5U, alias = "barrelThird")]
		[InputControl(name = "barrel4", displayName = "Barrel Button #4", layout = "Button", bit = 6U, alias = "barrelFourth")]
		[InputControl(name = "radius", layout = "Vector2", format = "VEC2", sizeInBits = 64U, usage = "Radius", offset = 4294967294U)]
		[InputControl(name = "pointerId", layout = "Digital", format = "UINT", sizeInBits = 32U, offset = 4294967294U)]
		[FieldOffset(32)]
		public ushort buttons;

		// Token: 0x0400056E RID: 1390
		[InputControl(name = "displayIndex", displayName = "Display Index", layout = "Integer")]
		[FieldOffset(34)]
		private ushort displayIndex;
	}
}
