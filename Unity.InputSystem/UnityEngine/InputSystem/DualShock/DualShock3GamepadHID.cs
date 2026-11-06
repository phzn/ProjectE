using System;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.DualShock.LowLevel;
using UnityEngine.InputSystem.Layouts;

namespace UnityEngine.InputSystem.DualShock
{
	// Token: 0x020000A5 RID: 165
	[InputControlLayout(stateType = typeof(DualShock3HIDInputReport), hideInUI = true, displayName = "PS3 Controller")]
	public class DualShock3GamepadHID : DualShockGamepad
	{
		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06000CCB RID: 3275 RVA: 0x00043A66 File Offset: 0x00041C66
		// (set) Token: 0x06000CCC RID: 3276 RVA: 0x00043A6E File Offset: 0x00041C6E
		public ButtonControl leftTriggerButton { get; protected set; }

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06000CCD RID: 3277 RVA: 0x00043A77 File Offset: 0x00041C77
		// (set) Token: 0x06000CCE RID: 3278 RVA: 0x00043A7F File Offset: 0x00041C7F
		public ButtonControl rightTriggerButton { get; protected set; }

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06000CCF RID: 3279 RVA: 0x00043A88 File Offset: 0x00041C88
		// (set) Token: 0x06000CD0 RID: 3280 RVA: 0x00043A90 File Offset: 0x00041C90
		public ButtonControl playStationButton { get; protected set; }

		// Token: 0x06000CD1 RID: 3281 RVA: 0x00043A99 File Offset: 0x00041C99
		protected override void FinishSetup()
		{
			this.leftTriggerButton = base.GetChildControl<ButtonControl>("leftTriggerButton");
			this.rightTriggerButton = base.GetChildControl<ButtonControl>("rightTriggerButton");
			this.playStationButton = base.GetChildControl<ButtonControl>("systemButton");
			base.FinishSetup();
		}
	}
}
