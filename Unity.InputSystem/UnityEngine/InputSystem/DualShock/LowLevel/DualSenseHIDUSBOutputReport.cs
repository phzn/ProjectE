using System;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.DualShock.LowLevel
{
	// Token: 0x020000AA RID: 170
	[StructLayout(LayoutKind.Explicit, Size = 56)]
	internal struct DualSenseHIDUSBOutputReport : IInputDeviceCommandInfo
	{
		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06000CD7 RID: 3287 RVA: 0x00043CC4 File Offset: 0x00041EC4
		public static FourCC Type
		{
			get
			{
				return new FourCC('H', 'I', 'D', 'O');
			}
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06000CD8 RID: 3288 RVA: 0x00043CD3 File Offset: 0x00041ED3
		public FourCC typeStatic
		{
			get
			{
				return DualSenseHIDUSBOutputReport.Type;
			}
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x00043CDC File Offset: 0x00041EDC
		public static DualSenseHIDUSBOutputReport Create(DualSenseHIDOutputReportPayload payload, int outputReportSize)
		{
			return new DualSenseHIDUSBOutputReport
			{
				baseCommand = new InputDeviceCommand(DualSenseHIDUSBOutputReport.Type, 8 + outputReportSize),
				reportId = 2,
				payload = payload
			};
		}

		// Token: 0x040004C2 RID: 1218
		internal const int kSize = 56;

		// Token: 0x040004C3 RID: 1219
		[FieldOffset(0)]
		public InputDeviceCommand baseCommand;

		// Token: 0x040004C4 RID: 1220
		[FieldOffset(8)]
		public byte reportId;

		// Token: 0x040004C5 RID: 1221
		[FieldOffset(9)]
		public DualSenseHIDOutputReportPayload payload;
	}
}
