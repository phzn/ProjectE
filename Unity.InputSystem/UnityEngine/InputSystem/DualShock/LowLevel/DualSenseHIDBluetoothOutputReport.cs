using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.DualShock.LowLevel
{
	// Token: 0x020000AB RID: 171
	[StructLayout(LayoutKind.Explicit, Size = 86)]
	internal struct DualSenseHIDBluetoothOutputReport : IInputDeviceCommandInfo
	{
		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06000CDA RID: 3290 RVA: 0x00043D16 File Offset: 0x00041F16
		public static FourCC Type
		{
			get
			{
				return new FourCC('H', 'I', 'D', 'O');
			}
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06000CDB RID: 3291 RVA: 0x00043D25 File Offset: 0x00041F25
		public FourCC typeStatic
		{
			get
			{
				return DualSenseHIDBluetoothOutputReport.Type;
			}
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x00043D2C File Offset: 0x00041F2C
		public static DualSenseHIDBluetoothOutputReport Create(DualSenseHIDOutputReportPayload payload, byte outputSequenceId, int outputReportSize)
		{
			return new DualSenseHIDBluetoothOutputReport
			{
				baseCommand = new InputDeviceCommand(DualSenseHIDBluetoothOutputReport.Type, 8 + outputReportSize),
				reportId = 49,
				tag1 = (byte)((outputSequenceId & 15) << 4),
				tag2 = 16,
				payload = payload
			};
		}

		// Token: 0x040004C6 RID: 1222
		internal const int kSize = 86;

		// Token: 0x040004C7 RID: 1223
		[FieldOffset(0)]
		public InputDeviceCommand baseCommand;

		// Token: 0x040004C8 RID: 1224
		[FieldOffset(8)]
		public byte reportId;

		// Token: 0x040004C9 RID: 1225
		[FieldOffset(9)]
		public byte tag1;

		// Token: 0x040004CA RID: 1226
		[FieldOffset(10)]
		public byte tag2;

		// Token: 0x040004CB RID: 1227
		[FieldOffset(11)]
		public DualSenseHIDOutputReportPayload payload;

		// Token: 0x040004CC RID: 1228
		[FieldOffset(82)]
		public uint crc32;

		// Token: 0x040004CD RID: 1229
		[FixedBuffer(typeof(byte), 74)]
		[FieldOffset(8)]
		public DualSenseHIDBluetoothOutputReport.<rawData>e__FixedBuffer rawData;

		// Token: 0x02000202 RID: 514
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 74)]
		public struct <rawData>e__FixedBuffer
		{
			// Token: 0x04000B77 RID: 2935
			public byte FixedElementField;
		}
	}
}
