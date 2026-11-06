using System;
using System.Linq;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.HID
{
	// Token: 0x0200009C RID: 156
	public static class HIDSupport
	{
		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06000C29 RID: 3113 RVA: 0x0004178C File Offset: 0x0003F98C
		// (set) Token: 0x06000C2A RID: 3114 RVA: 0x00041798 File Offset: 0x0003F998
		public static ReadOnlyArray<HIDSupport.HIDPageUsage> supportedHIDUsages
		{
			get
			{
				return HIDSupport.s_SupportedHIDUsages;
			}
			set
			{
				HIDSupport.s_SupportedHIDUsages = value.ToArray();
				InputSystem.s_Manager.AddAvailableDevicesThatAreNowRecognized();
				for (int i = 0; i < InputSystem.devices.Count; i++)
				{
					InputDevice inputDevice = InputSystem.devices[i];
					HID hid = inputDevice as HID;
					if (hid != null && !HIDSupport.s_SupportedHIDUsages.Contains(new HIDSupport.HIDPageUsage(hid.hidDescriptor.usagePage, hid.hidDescriptor.usage)))
					{
						InputSystem.RemoveLayout(inputDevice.layout);
						i--;
					}
				}
			}
		}

		// Token: 0x06000C2B RID: 3115 RVA: 0x00041824 File Offset: 0x0003FA24
		internal static void Initialize()
		{
			HIDSupport.s_SupportedHIDUsages = new HIDSupport.HIDPageUsage[]
			{
				new HIDSupport.HIDPageUsage(HID.GenericDesktop.Joystick),
				new HIDSupport.HIDPageUsage(HID.GenericDesktop.Gamepad),
				new HIDSupport.HIDPageUsage(HID.GenericDesktop.MultiAxisController)
			};
			InputSystem.RegisterLayout<HID>(null, null);
			InputSystem.onFindLayoutForDevice += HID.OnFindLayoutForDevice;
		}

		// Token: 0x04000479 RID: 1145
		private static HIDSupport.HIDPageUsage[] s_SupportedHIDUsages;

		// Token: 0x020001F7 RID: 503
		public struct HIDPageUsage
		{
			// Token: 0x060014E2 RID: 5346 RVA: 0x000619C5 File Offset: 0x0005FBC5
			public HIDPageUsage(HID.UsagePage page, int usage)
			{
				this.page = page;
				this.usage = usage;
			}

			// Token: 0x060014E3 RID: 5347 RVA: 0x000619D5 File Offset: 0x0005FBD5
			public HIDPageUsage(HID.GenericDesktop usage)
			{
				this.page = HID.UsagePage.GenericDesktop;
				this.usage = (int)usage;
			}

			// Token: 0x04000B31 RID: 2865
			public HID.UsagePage page;

			// Token: 0x04000B32 RID: 2866
			public int usage;
		}
	}
}
