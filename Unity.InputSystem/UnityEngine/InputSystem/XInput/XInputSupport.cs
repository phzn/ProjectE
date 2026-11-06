using System;
using UnityEngine.InputSystem.Layouts;

namespace UnityEngine.InputSystem.XInput
{
	// Token: 0x0200007E RID: 126
	internal static class XInputSupport
	{
		// Token: 0x06000A8E RID: 2702 RVA: 0x00038E50 File Offset: 0x00037050
		public static void Initialize()
		{
			InputSystem.RegisterLayout<XInputController>(null, null);
			InputSystem.RegisterLayout<XInputControllerWindows>(null, new InputDeviceMatcher?(default(InputDeviceMatcher).WithInterface("XInput", true)));
		}
	}
}
