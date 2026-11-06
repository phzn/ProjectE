using System;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.XR;

namespace Unity.XR.OpenVR
{
	// Token: 0x02000006 RID: 6
	[InputControlLayout(displayName = "Vive Wand", commonUsages = new string[] { "LeftHand", "RightHand" }, hideInUI = true)]
	public class ViveWand : XRControllerWithRumble
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600002E RID: 46 RVA: 0x000023AA File Offset: 0x000005AA
		// (set) Token: 0x0600002F RID: 47 RVA: 0x000023B2 File Offset: 0x000005B2
		[InputControl]
		public AxisControl grip { get; protected set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000030 RID: 48 RVA: 0x000023BB File Offset: 0x000005BB
		// (set) Token: 0x06000031 RID: 49 RVA: 0x000023C3 File Offset: 0x000005C3
		[InputControl]
		public ButtonControl gripPressed { get; protected set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000032 RID: 50 RVA: 0x000023CC File Offset: 0x000005CC
		// (set) Token: 0x06000033 RID: 51 RVA: 0x000023D4 File Offset: 0x000005D4
		[InputControl]
		public ButtonControl primary { get; protected set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000034 RID: 52 RVA: 0x000023DD File Offset: 0x000005DD
		// (set) Token: 0x06000035 RID: 53 RVA: 0x000023E5 File Offset: 0x000005E5
		[InputControl(aliases = new string[] { "primary2DAxisClick", "joystickOrPadPressed" })]
		public ButtonControl trackpadPressed { get; protected set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000036 RID: 54 RVA: 0x000023EE File Offset: 0x000005EE
		// (set) Token: 0x06000037 RID: 55 RVA: 0x000023F6 File Offset: 0x000005F6
		[InputControl(aliases = new string[] { "primary2DAxisTouch", "joystickOrPadTouched" })]
		public ButtonControl trackpadTouched { get; protected set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000038 RID: 56 RVA: 0x000023FF File Offset: 0x000005FF
		// (set) Token: 0x06000039 RID: 57 RVA: 0x00002407 File Offset: 0x00000607
		[InputControl(aliases = new string[] { "Primary2DAxis" })]
		public Vector2Control trackpad { get; protected set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00002410 File Offset: 0x00000610
		// (set) Token: 0x0600003B RID: 59 RVA: 0x00002418 File Offset: 0x00000618
		[InputControl]
		public AxisControl trigger { get; protected set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00002421 File Offset: 0x00000621
		// (set) Token: 0x0600003D RID: 61 RVA: 0x00002429 File Offset: 0x00000629
		[InputControl]
		public ButtonControl triggerPressed { get; protected set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00002432 File Offset: 0x00000632
		// (set) Token: 0x0600003F RID: 63 RVA: 0x0000243A File Offset: 0x0000063A
		[InputControl(noisy = true)]
		public Vector3Control deviceVelocity { get; protected set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00002443 File Offset: 0x00000643
		// (set) Token: 0x06000041 RID: 65 RVA: 0x0000244B File Offset: 0x0000064B
		[InputControl(noisy = true)]
		public Vector3Control deviceAngularVelocity { get; protected set; }

		// Token: 0x06000042 RID: 66 RVA: 0x00002454 File Offset: 0x00000654
		protected override void FinishSetup()
		{
			base.FinishSetup();
			this.grip = base.GetChildControl<AxisControl>("grip");
			this.primary = base.GetChildControl<ButtonControl>("primary");
			this.gripPressed = base.GetChildControl<ButtonControl>("gripPressed");
			this.trackpadPressed = base.GetChildControl<ButtonControl>("trackpadPressed");
			this.trackpadTouched = base.GetChildControl<ButtonControl>("trackpadTouched");
			this.trackpad = base.GetChildControl<Vector2Control>("trackpad");
			this.trigger = base.GetChildControl<AxisControl>("trigger");
			this.triggerPressed = base.GetChildControl<ButtonControl>("triggerPressed");
			this.deviceVelocity = base.GetChildControl<Vector3Control>("deviceVelocity");
			this.deviceAngularVelocity = base.GetChildControl<Vector3Control>("deviceAngularVelocity");
		}
	}
}
