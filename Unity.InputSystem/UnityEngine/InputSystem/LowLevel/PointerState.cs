using System;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000D5 RID: 213
	internal struct PointerState : IInputStateTypeInfo
	{
		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06000D62 RID: 3426 RVA: 0x00044A53 File Offset: 0x00042C53
		public static FourCC kFormat
		{
			get
			{
				return new FourCC('P', 'T', 'R', ' ');
			}
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x06000D63 RID: 3427 RVA: 0x00044A62 File Offset: 0x00042C62
		public FourCC format
		{
			get
			{
				return PointerState.kFormat;
			}
		}

		// Token: 0x0400056F RID: 1391
		private uint pointerId;

		// Token: 0x04000570 RID: 1392
		[InputControl(layout = "Vector2", displayName = "Position", usage = "Point", dontReset = true)]
		public Vector2 position;

		// Token: 0x04000571 RID: 1393
		[InputControl(layout = "Delta", displayName = "Delta", usage = "Secondary2DMotion")]
		public Vector2 delta;

		// Token: 0x04000572 RID: 1394
		[InputControl(layout = "Analog", displayName = "Pressure", usage = "Pressure", defaultState = 1f)]
		public float pressure;

		// Token: 0x04000573 RID: 1395
		[InputControl(layout = "Vector2", displayName = "Radius", usage = "Radius")]
		public Vector2 radius;

		// Token: 0x04000574 RID: 1396
		[InputControl(name = "press", displayName = "Press", layout = "Button", format = "BIT", bit = 0U)]
		public ushort buttons;

		// Token: 0x04000575 RID: 1397
		[InputControl(name = "displayIndex", layout = "Integer", displayName = "Display Index")]
		public ushort displayIndex;
	}
}
