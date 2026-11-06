using System;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000E2 RID: 226
	[StructLayout(LayoutKind.Explicit, Size = 20)]
	public struct DeviceResetEvent : IInputEventTypeInfo
	{
		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06000DA7 RID: 3495 RVA: 0x00045160 File Offset: 0x00043360
		public FourCC typeStatic
		{
			get
			{
				return 1146245972;
			}
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x0004516C File Offset: 0x0004336C
		public static DeviceResetEvent Create(int deviceId, bool hardReset = false, double time = -1.0)
		{
			DeviceResetEvent deviceResetEvent = new DeviceResetEvent
			{
				baseEvent = new InputEvent(1146245972, 20, deviceId, time)
			};
			deviceResetEvent.hardReset = hardReset;
			return deviceResetEvent;
		}

		// Token: 0x040005A4 RID: 1444
		public const int Type = 1146245972;

		// Token: 0x040005A5 RID: 1445
		[FieldOffset(0)]
		public InputEvent baseEvent;

		// Token: 0x040005A6 RID: 1446
		[FieldOffset(8)]
		public bool hardReset;
	}
}
