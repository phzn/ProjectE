using System;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000C6 RID: 198
	[StructLayout(LayoutKind.Explicit, Size = 12)]
	public struct SetSamplingFrequencyCommand : IInputDeviceCommandInfo
	{
		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06000D3F RID: 3391 RVA: 0x00044735 File Offset: 0x00042935
		public static FourCC Type
		{
			get
			{
				return new FourCC('S', 'S', 'P', 'L');
			}
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06000D40 RID: 3392 RVA: 0x00044744 File Offset: 0x00042944
		public FourCC typeStatic
		{
			get
			{
				return SetSamplingFrequencyCommand.Type;
			}
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x0004474C File Offset: 0x0004294C
		public static SetSamplingFrequencyCommand Create(float frequency)
		{
			return new SetSamplingFrequencyCommand
			{
				baseCommand = new InputDeviceCommand(SetSamplingFrequencyCommand.Type, 12),
				frequency = frequency
			};
		}

		// Token: 0x0400052B RID: 1323
		internal const int kSize = 12;

		// Token: 0x0400052C RID: 1324
		[FieldOffset(0)]
		public InputDeviceCommand baseCommand;

		// Token: 0x0400052D RID: 1325
		[FieldOffset(8)]
		public float frequency;
	}
}
