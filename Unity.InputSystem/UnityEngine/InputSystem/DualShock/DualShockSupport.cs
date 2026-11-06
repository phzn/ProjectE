using System;
using UnityEngine.InputSystem.Layouts;

namespace UnityEngine.InputSystem.DualShock
{
	// Token: 0x020000A6 RID: 166
	internal static class DualShockSupport
	{
		// Token: 0x06000CD3 RID: 3283 RVA: 0x00043ADC File Offset: 0x00041CDC
		public static void Initialize()
		{
			InputSystem.RegisterLayout<DualShockGamepad>(null, null);
			string text = null;
			InputDeviceMatcher inputDeviceMatcher = default(InputDeviceMatcher);
			inputDeviceMatcher = inputDeviceMatcher.WithInterface("HID", true);
			inputDeviceMatcher = inputDeviceMatcher.WithCapability<int>("vendorId", 1356);
			InputSystem.RegisterLayout<DualSenseGamepadHID>(text, new InputDeviceMatcher?(inputDeviceMatcher.WithCapability<int>("productId", 3570)));
			string text2 = null;
			inputDeviceMatcher = default(InputDeviceMatcher);
			inputDeviceMatcher = inputDeviceMatcher.WithInterface("HID", true);
			inputDeviceMatcher = inputDeviceMatcher.WithCapability<int>("vendorId", 1356);
			InputSystem.RegisterLayout<DualSenseGamepadHID>(text2, new InputDeviceMatcher?(inputDeviceMatcher.WithCapability<int>("productId", 3302)));
			string text3 = null;
			inputDeviceMatcher = default(InputDeviceMatcher);
			inputDeviceMatcher = inputDeviceMatcher.WithInterface("HID", true);
			inputDeviceMatcher = inputDeviceMatcher.WithCapability<int>("vendorId", 1356);
			InputSystem.RegisterLayout<DualShock4GamepadHID>(text3, new InputDeviceMatcher?(inputDeviceMatcher.WithCapability<int>("productId", 2508)));
			inputDeviceMatcher = default(InputDeviceMatcher);
			inputDeviceMatcher = inputDeviceMatcher.WithInterface("HID", true);
			inputDeviceMatcher = inputDeviceMatcher.WithCapability<int>("vendorId", 1356);
			InputSystem.RegisterLayoutMatcher<DualShock4GamepadHID>(inputDeviceMatcher.WithCapability<int>("productId", 1476));
			inputDeviceMatcher = default(InputDeviceMatcher);
			inputDeviceMatcher = inputDeviceMatcher.WithInterface("HID", true);
			inputDeviceMatcher = inputDeviceMatcher.WithManufacturerContains("Sony");
			InputSystem.RegisterLayoutMatcher<DualShock4GamepadHID>(inputDeviceMatcher.WithProduct("Wireless Controller", true));
			string text4 = null;
			inputDeviceMatcher = default(InputDeviceMatcher);
			inputDeviceMatcher = inputDeviceMatcher.WithInterface("HID", true);
			inputDeviceMatcher = inputDeviceMatcher.WithCapability<int>("vendorId", 1356);
			InputSystem.RegisterLayout<DualShock3GamepadHID>(text4, new InputDeviceMatcher?(inputDeviceMatcher.WithCapability<int>("productId", 616)));
			inputDeviceMatcher = default(InputDeviceMatcher);
			inputDeviceMatcher = inputDeviceMatcher.WithInterface("HID", true);
			inputDeviceMatcher = inputDeviceMatcher.WithManufacturerContains("Sony");
			InputSystem.RegisterLayoutMatcher<DualShock3GamepadHID>(inputDeviceMatcher.WithProduct("PLAYSTATION(R)3 Controller", false));
		}
	}
}
