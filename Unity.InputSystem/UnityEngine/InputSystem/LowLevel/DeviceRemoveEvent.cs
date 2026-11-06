using System;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000E1 RID: 225
	[StructLayout(LayoutKind.Explicit, Size = 20)]
	public struct DeviceRemoveEvent : IInputEventTypeInfo
	{
		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06000DA4 RID: 3492 RVA: 0x0004510C File Offset: 0x0004330C
		public FourCC typeStatic
		{
			get
			{
				return 1146242381;
			}
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x00045118 File Offset: 0x00043318
		public unsafe InputEventPtr ToEventPtr()
		{
			fixed (DeviceRemoveEvent* ptr = &this)
			{
				return new InputEventPtr((InputEvent*)ptr);
			}
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x00045130 File Offset: 0x00043330
		public static DeviceRemoveEvent Create(int deviceId, double time = -1.0)
		{
			return new DeviceRemoveEvent
			{
				baseEvent = new InputEvent(1146242381, 20, deviceId, time)
			};
		}

		// Token: 0x040005A2 RID: 1442
		public const int Type = 1146242381;

		// Token: 0x040005A3 RID: 1443
		[FieldOffset(0)]
		public InputEvent baseEvent;
	}
}
