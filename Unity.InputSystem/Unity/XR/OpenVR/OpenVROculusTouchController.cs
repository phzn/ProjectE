using System;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.XR;

namespace Unity.XR.OpenVR
{
	// Token: 0x0200000A RID: 10
	[InputControlLayout(displayName = "Oculus Touch Controller (OpenVR)", commonUsages = new string[] { "LeftHand", "RightHand" }, hideInUI = true)]
	public class OpenVROculusTouchController : XRControllerWithRumble
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000057 RID: 87 RVA: 0x0000263C File Offset: 0x0000083C
		// (set) Token: 0x06000058 RID: 88 RVA: 0x00002644 File Offset: 0x00000844
		[InputControl]
		public Vector2Control thumbstick { get; protected set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000059 RID: 89 RVA: 0x0000264D File Offset: 0x0000084D
		// (set) Token: 0x0600005A RID: 90 RVA: 0x00002655 File Offset: 0x00000855
		[InputControl]
		public AxisControl trigger { get; protected set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600005B RID: 91 RVA: 0x0000265E File Offset: 0x0000085E
		// (set) Token: 0x0600005C RID: 92 RVA: 0x00002666 File Offset: 0x00000866
		[InputControl]
		public AxisControl grip { get; protected set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600005D RID: 93 RVA: 0x0000266F File Offset: 0x0000086F
		// (set) Token: 0x0600005E RID: 94 RVA: 0x00002677 File Offset: 0x00000877
		[InputControl(aliases = new string[] { "Alternate" })]
		public ButtonControl primaryButton { get; protected set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00002680 File Offset: 0x00000880
		// (set) Token: 0x06000060 RID: 96 RVA: 0x00002688 File Offset: 0x00000888
		[InputControl(aliases = new string[] { "Primary" })]
		public ButtonControl secondaryButton { get; protected set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000061 RID: 97 RVA: 0x00002691 File Offset: 0x00000891
		// (set) Token: 0x06000062 RID: 98 RVA: 0x00002699 File Offset: 0x00000899
		[InputControl]
		public ButtonControl gripPressed { get; protected set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000063 RID: 99 RVA: 0x000026A2 File Offset: 0x000008A2
		// (set) Token: 0x06000064 RID: 100 RVA: 0x000026AA File Offset: 0x000008AA
		[InputControl]
		public ButtonControl triggerPressed { get; protected set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000065 RID: 101 RVA: 0x000026B3 File Offset: 0x000008B3
		// (set) Token: 0x06000066 RID: 102 RVA: 0x000026BB File Offset: 0x000008BB
		[InputControl(aliases = new string[] { "primary2DAxisClicked" })]
		public ButtonControl thumbstickClicked { get; protected set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000067 RID: 103 RVA: 0x000026C4 File Offset: 0x000008C4
		// (set) Token: 0x06000068 RID: 104 RVA: 0x000026CC File Offset: 0x000008CC
		[InputControl(aliases = new string[] { "primary2DAxisTouch" })]
		public ButtonControl thumbstickTouched { get; protected set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000069 RID: 105 RVA: 0x000026D5 File Offset: 0x000008D5
		// (set) Token: 0x0600006A RID: 106 RVA: 0x000026DD File Offset: 0x000008DD
		[InputControl(noisy = true)]
		public Vector3Control deviceVelocity { get; protected set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600006B RID: 107 RVA: 0x000026E6 File Offset: 0x000008E6
		// (set) Token: 0x0600006C RID: 108 RVA: 0x000026EE File Offset: 0x000008EE
		[InputControl(noisy = true)]
		public Vector3Control deviceAngularVelocity { get; protected set; }

		// Token: 0x0600006D RID: 109 RVA: 0x000026F8 File Offset: 0x000008F8
		protected override void FinishSetup()
		{
			base.FinishSetup();
			this.thumbstick = base.GetChildControl<Vector2Control>("thumbstick");
			this.trigger = base.GetChildControl<AxisControl>("trigger");
			this.grip = base.GetChildControl<AxisControl>("grip");
			this.primaryButton = base.GetChildControl<ButtonControl>("primaryButton");
			this.secondaryButton = base.GetChildControl<ButtonControl>("secondaryButton");
			this.gripPressed = base.GetChildControl<ButtonControl>("gripPressed");
			this.thumbstickClicked = base.GetChildControl<ButtonControl>("thumbstickClicked");
			this.thumbstickTouched = base.GetChildControl<ButtonControl>("thumbstickTouched");
			this.triggerPressed = base.GetChildControl<ButtonControl>("triggerPressed");
			this.deviceVelocity = base.GetChildControl<Vector3Control>("deviceVelocity");
			this.deviceAngularVelocity = base.GetChildControl<Vector3Control>("deviceAngularVelocity");
		}
	}
}
