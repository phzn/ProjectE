using System;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.XR;

namespace Unity.XR.Oculus.Input
{
	// Token: 0x0200000C RID: 12
	[InputControlLayout(displayName = "Oculus Touch Controller", commonUsages = new string[] { "LeftHand", "RightHand" }, hideInUI = true)]
	public class OculusTouchController : XRControllerWithRumble
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600008B RID: 139 RVA: 0x000029A4 File Offset: 0x00000BA4
		// (set) Token: 0x0600008C RID: 140 RVA: 0x000029AC File Offset: 0x00000BAC
		[InputControl(aliases = new string[] { "Primary2DAxis", "Joystick" })]
		public Vector2Control thumbstick { get; protected set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600008D RID: 141 RVA: 0x000029B5 File Offset: 0x00000BB5
		// (set) Token: 0x0600008E RID: 142 RVA: 0x000029BD File Offset: 0x00000BBD
		[InputControl]
		public AxisControl trigger { get; protected set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600008F RID: 143 RVA: 0x000029C6 File Offset: 0x00000BC6
		// (set) Token: 0x06000090 RID: 144 RVA: 0x000029CE File Offset: 0x00000BCE
		[InputControl]
		public AxisControl grip { get; protected set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000091 RID: 145 RVA: 0x000029D7 File Offset: 0x00000BD7
		// (set) Token: 0x06000092 RID: 146 RVA: 0x000029DF File Offset: 0x00000BDF
		[InputControl(aliases = new string[] { "A", "X", "Alternate" })]
		public ButtonControl primaryButton { get; protected set; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000093 RID: 147 RVA: 0x000029E8 File Offset: 0x00000BE8
		// (set) Token: 0x06000094 RID: 148 RVA: 0x000029F0 File Offset: 0x00000BF0
		[InputControl(aliases = new string[] { "B", "Y", "Primary" })]
		public ButtonControl secondaryButton { get; protected set; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000095 RID: 149 RVA: 0x000029F9 File Offset: 0x00000BF9
		// (set) Token: 0x06000096 RID: 150 RVA: 0x00002A01 File Offset: 0x00000C01
		[InputControl(aliases = new string[] { "GripButton" })]
		public ButtonControl gripPressed { get; protected set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00002A0A File Offset: 0x00000C0A
		// (set) Token: 0x06000098 RID: 152 RVA: 0x00002A12 File Offset: 0x00000C12
		[InputControl]
		public ButtonControl start { get; protected set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000099 RID: 153 RVA: 0x00002A1B File Offset: 0x00000C1B
		// (set) Token: 0x0600009A RID: 154 RVA: 0x00002A23 File Offset: 0x00000C23
		[InputControl(aliases = new string[] { "JoystickOrPadPressed", "thumbstickClick" })]
		public ButtonControl thumbstickClicked { get; protected set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600009B RID: 155 RVA: 0x00002A2C File Offset: 0x00000C2C
		// (set) Token: 0x0600009C RID: 156 RVA: 0x00002A34 File Offset: 0x00000C34
		[InputControl(aliases = new string[] { "ATouched", "XTouched", "ATouch", "XTouch" })]
		public ButtonControl primaryTouched { get; protected set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600009D RID: 157 RVA: 0x00002A3D File Offset: 0x00000C3D
		// (set) Token: 0x0600009E RID: 158 RVA: 0x00002A45 File Offset: 0x00000C45
		[InputControl(aliases = new string[] { "BTouched", "YTouched", "BTouch", "YTouch" })]
		public ButtonControl secondaryTouched { get; protected set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00002A4E File Offset: 0x00000C4E
		// (set) Token: 0x060000A0 RID: 160 RVA: 0x00002A56 File Offset: 0x00000C56
		[InputControl(aliases = new string[] { "indexTouch", "indexNearTouched" })]
		public AxisControl triggerTouched { get; protected set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x00002A5F File Offset: 0x00000C5F
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x00002A67 File Offset: 0x00000C67
		[InputControl(aliases = new string[] { "indexButton", "indexTouched" })]
		public ButtonControl triggerPressed { get; protected set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x00002A70 File Offset: 0x00000C70
		// (set) Token: 0x060000A4 RID: 164 RVA: 0x00002A78 File Offset: 0x00000C78
		[InputControl(aliases = new string[] { "JoystickOrPadTouched", "thumbstickTouch" })]
		[InputControl(name = "trackingState", layout = "Integer", aliases = new string[] { "controllerTrackingState" })]
		[InputControl(name = "isTracked", layout = "Button", aliases = new string[] { "ControllerIsTracked" })]
		[InputControl(name = "devicePosition", layout = "Vector3", aliases = new string[] { "controllerPosition" })]
		[InputControl(name = "deviceRotation", layout = "Quaternion", aliases = new string[] { "controllerRotation" })]
		public ButtonControl thumbstickTouched { get; protected set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x00002A81 File Offset: 0x00000C81
		// (set) Token: 0x060000A6 RID: 166 RVA: 0x00002A89 File Offset: 0x00000C89
		[InputControl(noisy = true, aliases = new string[] { "controllerVelocity" })]
		public Vector3Control deviceVelocity { get; protected set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x00002A92 File Offset: 0x00000C92
		// (set) Token: 0x060000A8 RID: 168 RVA: 0x00002A9A File Offset: 0x00000C9A
		[InputControl(noisy = true, aliases = new string[] { "controllerAngularVelocity" })]
		public Vector3Control deviceAngularVelocity { get; protected set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x00002AA3 File Offset: 0x00000CA3
		// (set) Token: 0x060000AA RID: 170 RVA: 0x00002AAB File Offset: 0x00000CAB
		[InputControl(noisy = true, aliases = new string[] { "controllerAcceleration" })]
		public Vector3Control deviceAcceleration { get; protected set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000AB RID: 171 RVA: 0x00002AB4 File Offset: 0x00000CB4
		// (set) Token: 0x060000AC RID: 172 RVA: 0x00002ABC File Offset: 0x00000CBC
		[InputControl(noisy = true, aliases = new string[] { "controllerAngularAcceleration" })]
		public Vector3Control deviceAngularAcceleration { get; protected set; }

		// Token: 0x060000AD RID: 173 RVA: 0x00002AC8 File Offset: 0x00000CC8
		protected override void FinishSetup()
		{
			base.FinishSetup();
			this.thumbstick = base.GetChildControl<Vector2Control>("thumbstick");
			this.trigger = base.GetChildControl<AxisControl>("trigger");
			this.triggerTouched = base.GetChildControl<AxisControl>("triggerTouched");
			this.grip = base.GetChildControl<AxisControl>("grip");
			this.primaryButton = base.GetChildControl<ButtonControl>("primaryButton");
			this.secondaryButton = base.GetChildControl<ButtonControl>("secondaryButton");
			this.gripPressed = base.GetChildControl<ButtonControl>("gripPressed");
			this.start = base.GetChildControl<ButtonControl>("start");
			this.thumbstickClicked = base.GetChildControl<ButtonControl>("thumbstickClicked");
			this.primaryTouched = base.GetChildControl<ButtonControl>("primaryTouched");
			this.secondaryTouched = base.GetChildControl<ButtonControl>("secondaryTouched");
			this.thumbstickTouched = base.GetChildControl<ButtonControl>("thumbstickTouched");
			this.triggerPressed = base.GetChildControl<ButtonControl>("triggerPressed");
			this.deviceVelocity = base.GetChildControl<Vector3Control>("deviceVelocity");
			this.deviceAngularVelocity = base.GetChildControl<Vector3Control>("deviceAngularVelocity");
			this.deviceAcceleration = base.GetChildControl<Vector3Control>("deviceAcceleration");
			this.deviceAngularAcceleration = base.GetChildControl<Vector3Control>("deviceAngularAcceleration");
		}
	}
}
