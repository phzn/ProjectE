using System;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000CA RID: 202
	[StructLayout(LayoutKind.Explicit, Size = 16)]
	internal struct DualMotorRumbleCommand : IInputDeviceCommandInfo
	{
		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06000D49 RID: 3401 RVA: 0x00044864 File Offset: 0x00042A64
		public static FourCC Type
		{
			get
			{
				return new FourCC('R', 'M', 'B', 'L');
			}
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06000D4A RID: 3402 RVA: 0x00044873 File Offset: 0x00042A73
		public FourCC typeStatic
		{
			get
			{
				return DualMotorRumbleCommand.Type;
			}
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x0004487C File Offset: 0x00042A7C
		public static DualMotorRumbleCommand Create(float lowFrequency, float highFrequency)
		{
			return new DualMotorRumbleCommand
			{
				baseCommand = new InputDeviceCommand(DualMotorRumbleCommand.Type, 16),
				lowFrequencyMotorSpeed = lowFrequency,
				highFrequencyMotorSpeed = highFrequency
			};
		}

		// Token: 0x04000553 RID: 1363
		internal const int kSize = 16;

		// Token: 0x04000554 RID: 1364
		[FieldOffset(0)]
		public InputDeviceCommand baseCommand;

		// Token: 0x04000555 RID: 1365
		[FieldOffset(8)]
		public float lowFrequencyMotorSpeed;

		// Token: 0x04000556 RID: 1366
		[FieldOffset(12)]
		public float highFrequencyMotorSpeed;
	}
}
