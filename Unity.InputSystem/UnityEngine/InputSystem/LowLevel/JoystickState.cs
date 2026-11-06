using System;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000D0 RID: 208
	internal struct JoystickState : IInputStateTypeInfo
	{
		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06000D52 RID: 3410 RVA: 0x000448B5 File Offset: 0x00042AB5
		public static FourCC kFormat
		{
			get
			{
				return new FourCC('J', 'O', 'Y', ' ');
			}
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06000D53 RID: 3411 RVA: 0x000448C4 File Offset: 0x00042AC4
		public FourCC format
		{
			get
			{
				return JoystickState.kFormat;
			}
		}

		// Token: 0x04000557 RID: 1367
		[InputControl(name = "trigger", displayName = "Trigger", layout = "Button", usages = new string[] { "PrimaryTrigger", "PrimaryAction", "Submit" }, bit = 4U)]
		public int buttons;

		// Token: 0x04000558 RID: 1368
		[InputControl(displayName = "Stick", layout = "Stick", usage = "Primary2DMotion", processors = "stickDeadzone")]
		public Vector2 stick;

		// Token: 0x0200020E RID: 526
		public enum Button
		{
			// Token: 0x04000B8D RID: 2957
			HatSwitchUp,
			// Token: 0x04000B8E RID: 2958
			HatSwitchDown,
			// Token: 0x04000B8F RID: 2959
			HatSwitchLeft,
			// Token: 0x04000B90 RID: 2960
			HatSwitchRight,
			// Token: 0x04000B91 RID: 2961
			Trigger
		}
	}
}
