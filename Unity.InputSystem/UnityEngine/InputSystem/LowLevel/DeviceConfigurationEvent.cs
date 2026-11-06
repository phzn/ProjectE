using System;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000E0 RID: 224
	[StructLayout(LayoutKind.Explicit, Size = 20)]
	public struct DeviceConfigurationEvent : IInputEventTypeInfo
	{
		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06000DA1 RID: 3489 RVA: 0x000450B8 File Offset: 0x000432B8
		public FourCC typeStatic
		{
			get
			{
				return 1145259591;
			}
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x000450C4 File Offset: 0x000432C4
		public unsafe InputEventPtr ToEventPtr()
		{
			fixed (DeviceConfigurationEvent* ptr = &this)
			{
				return new InputEventPtr((InputEvent*)ptr);
			}
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x000450DC File Offset: 0x000432DC
		public static DeviceConfigurationEvent Create(int deviceId, double time)
		{
			return new DeviceConfigurationEvent
			{
				baseEvent = new InputEvent(1145259591, 20, deviceId, time)
			};
		}

		// Token: 0x040005A0 RID: 1440
		public const int Type = 1145259591;

		// Token: 0x040005A1 RID: 1441
		[FieldOffset(0)]
		public InputEvent baseEvent;
	}
}
