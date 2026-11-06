using System;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.XR;

namespace Unity.XR.OpenVR
{
	// Token: 0x02000005 RID: 5
	[InputControlLayout(displayName = "Windows MR Controller (OpenVR)", commonUsages = new string[] { "LeftHand", "RightHand" }, hideInUI = true)]
	public class OpenVRControllerWMR : XRController
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000016 RID: 22 RVA: 0x00002217 File Offset: 0x00000417
		// (set) Token: 0x06000017 RID: 23 RVA: 0x0000221F File Offset: 0x0000041F
		[InputControl(noisy = true)]
		public Vector3Control deviceVelocity { get; protected set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000018 RID: 24 RVA: 0x00002228 File Offset: 0x00000428
		// (set) Token: 0x06000019 RID: 25 RVA: 0x00002230 File Offset: 0x00000430
		[InputControl(noisy = true)]
		public Vector3Control deviceAngularVelocity { get; protected set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002239 File Offset: 0x00000439
		// (set) Token: 0x0600001B RID: 27 RVA: 0x00002241 File Offset: 0x00000441
		[InputControl(aliases = new string[] { "primary2DAxisClick", "joystickOrPadPressed" })]
		public ButtonControl touchpadClick { get; protected set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600001C RID: 28 RVA: 0x0000224A File Offset: 0x0000044A
		// (set) Token: 0x0600001D RID: 29 RVA: 0x00002252 File Offset: 0x00000452
		[InputControl(aliases = new string[] { "primary2DAxisTouch", "joystickOrPadTouched" })]
		public ButtonControl touchpadTouch { get; protected set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600001E RID: 30 RVA: 0x0000225B File Offset: 0x0000045B
		// (set) Token: 0x0600001F RID: 31 RVA: 0x00002263 File Offset: 0x00000463
		[InputControl]
		public ButtonControl gripPressed { get; protected set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000020 RID: 32 RVA: 0x0000226C File Offset: 0x0000046C
		// (set) Token: 0x06000021 RID: 33 RVA: 0x00002274 File Offset: 0x00000474
		[InputControl]
		public ButtonControl triggerPressed { get; protected set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000022 RID: 34 RVA: 0x0000227D File Offset: 0x0000047D
		// (set) Token: 0x06000023 RID: 35 RVA: 0x00002285 File Offset: 0x00000485
		[InputControl(aliases = new string[] { "primary" })]
		public ButtonControl menu { get; protected set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000024 RID: 36 RVA: 0x0000228E File Offset: 0x0000048E
		// (set) Token: 0x06000025 RID: 37 RVA: 0x00002296 File Offset: 0x00000496
		[InputControl]
		public AxisControl trigger { get; protected set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000026 RID: 38 RVA: 0x0000229F File Offset: 0x0000049F
		// (set) Token: 0x06000027 RID: 39 RVA: 0x000022A7 File Offset: 0x000004A7
		[InputControl]
		public AxisControl grip { get; protected set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000028 RID: 40 RVA: 0x000022B0 File Offset: 0x000004B0
		// (set) Token: 0x06000029 RID: 41 RVA: 0x000022B8 File Offset: 0x000004B8
		[InputControl(aliases = new string[] { "secondary2DAxis" })]
		public Vector2Control touchpad { get; protected set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600002A RID: 42 RVA: 0x000022C1 File Offset: 0x000004C1
		// (set) Token: 0x0600002B RID: 43 RVA: 0x000022C9 File Offset: 0x000004C9
		[InputControl(aliases = new string[] { "primary2DAxis" })]
		public Vector2Control joystick { get; protected set; }

		// Token: 0x0600002C RID: 44 RVA: 0x000022D4 File Offset: 0x000004D4
		protected override void FinishSetup()
		{
			base.FinishSetup();
			this.deviceVelocity = base.GetChildControl<Vector3Control>("deviceVelocity");
			this.deviceAngularVelocity = base.GetChildControl<Vector3Control>("deviceAngularVelocity");
			this.touchpadClick = base.GetChildControl<ButtonControl>("touchpadClick");
			this.touchpadTouch = base.GetChildControl<ButtonControl>("touchpadTouch");
			this.gripPressed = base.GetChildControl<ButtonControl>("gripPressed");
			this.triggerPressed = base.GetChildControl<ButtonControl>("triggerPressed");
			this.menu = base.GetChildControl<ButtonControl>("menu");
			this.trigger = base.GetChildControl<AxisControl>("trigger");
			this.grip = base.GetChildControl<AxisControl>("grip");
			this.touchpad = base.GetChildControl<Vector2Control>("touchpad");
			this.joystick = base.GetChildControl<Vector2Control>("joystick");
		}
	}
}
