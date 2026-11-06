using System;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.XR;

namespace UnityEngine.XR.WindowsMR.Input
{
	// Token: 0x02000015 RID: 21
	[InputControlLayout(displayName = "Windows MR Controller", commonUsages = new string[] { "LeftHand", "RightHand" }, hideInUI = true)]
	public class WMRSpatialController : XRControllerWithRumble
	{
		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060000FE RID: 254 RVA: 0x000030DF File Offset: 0x000012DF
		// (set) Token: 0x060000FF RID: 255 RVA: 0x000030E7 File Offset: 0x000012E7
		[InputControl(aliases = new string[] { "Primary2DAxis", "thumbstickaxes" })]
		public Vector2Control joystick { get; protected set; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000100 RID: 256 RVA: 0x000030F0 File Offset: 0x000012F0
		// (set) Token: 0x06000101 RID: 257 RVA: 0x000030F8 File Offset: 0x000012F8
		[InputControl(aliases = new string[] { "Secondary2DAxis", "touchpadaxes" })]
		public Vector2Control touchpad { get; protected set; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000102 RID: 258 RVA: 0x00003101 File Offset: 0x00001301
		// (set) Token: 0x06000103 RID: 259 RVA: 0x00003109 File Offset: 0x00001309
		[InputControl(aliases = new string[] { "gripaxis" })]
		public AxisControl grip { get; protected set; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000104 RID: 260 RVA: 0x00003112 File Offset: 0x00001312
		// (set) Token: 0x06000105 RID: 261 RVA: 0x0000311A File Offset: 0x0000131A
		[InputControl(aliases = new string[] { "gripbutton" })]
		public ButtonControl gripPressed { get; protected set; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000106 RID: 262 RVA: 0x00003123 File Offset: 0x00001323
		// (set) Token: 0x06000107 RID: 263 RVA: 0x0000312B File Offset: 0x0000132B
		[InputControl(aliases = new string[] { "Primary", "menubutton" })]
		public ButtonControl menu { get; protected set; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000108 RID: 264 RVA: 0x00003134 File Offset: 0x00001334
		// (set) Token: 0x06000109 RID: 265 RVA: 0x0000313C File Offset: 0x0000133C
		[InputControl(aliases = new string[] { "triggeraxis" })]
		public AxisControl trigger { get; protected set; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00003145 File Offset: 0x00001345
		// (set) Token: 0x0600010B RID: 267 RVA: 0x0000314D File Offset: 0x0000134D
		[InputControl(aliases = new string[] { "triggerbutton" })]
		public ButtonControl triggerPressed { get; protected set; }

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600010C RID: 268 RVA: 0x00003156 File Offset: 0x00001356
		// (set) Token: 0x0600010D RID: 269 RVA: 0x0000315E File Offset: 0x0000135E
		[InputControl(aliases = new string[] { "thumbstickpressed" })]
		public ButtonControl joystickClicked { get; protected set; }

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600010E RID: 270 RVA: 0x00003167 File Offset: 0x00001367
		// (set) Token: 0x0600010F RID: 271 RVA: 0x0000316F File Offset: 0x0000136F
		[InputControl(aliases = new string[] { "joystickorpadpressed", "touchpadpressed" })]
		public ButtonControl touchpadClicked { get; protected set; }

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000110 RID: 272 RVA: 0x00003178 File Offset: 0x00001378
		// (set) Token: 0x06000111 RID: 273 RVA: 0x00003180 File Offset: 0x00001380
		[InputControl(aliases = new string[] { "joystickorpadtouched", "touchpadtouched" })]
		public ButtonControl touchpadTouched { get; protected set; }

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000112 RID: 274 RVA: 0x00003189 File Offset: 0x00001389
		// (set) Token: 0x06000113 RID: 275 RVA: 0x00003191 File Offset: 0x00001391
		[InputControl(noisy = true, aliases = new string[] { "gripVelocity" })]
		public Vector3Control deviceVelocity { get; protected set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000114 RID: 276 RVA: 0x0000319A File Offset: 0x0000139A
		// (set) Token: 0x06000115 RID: 277 RVA: 0x000031A2 File Offset: 0x000013A2
		[InputControl(noisy = true, aliases = new string[] { "gripAngularVelocity" })]
		public Vector3Control deviceAngularVelocity { get; protected set; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000116 RID: 278 RVA: 0x000031AB File Offset: 0x000013AB
		// (set) Token: 0x06000117 RID: 279 RVA: 0x000031B3 File Offset: 0x000013B3
		[InputControl(noisy = true)]
		public AxisControl batteryLevel { get; protected set; }

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000118 RID: 280 RVA: 0x000031BC File Offset: 0x000013BC
		// (set) Token: 0x06000119 RID: 281 RVA: 0x000031C4 File Offset: 0x000013C4
		[InputControl(noisy = true)]
		public AxisControl sourceLossRisk { get; protected set; }

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600011A RID: 282 RVA: 0x000031CD File Offset: 0x000013CD
		// (set) Token: 0x0600011B RID: 283 RVA: 0x000031D5 File Offset: 0x000013D5
		[InputControl(noisy = true)]
		public Vector3Control sourceLossMitigationDirection { get; protected set; }

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600011C RID: 284 RVA: 0x000031DE File Offset: 0x000013DE
		// (set) Token: 0x0600011D RID: 285 RVA: 0x000031E6 File Offset: 0x000013E6
		[InputControl(noisy = true)]
		public Vector3Control pointerPosition { get; protected set; }

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600011E RID: 286 RVA: 0x000031EF File Offset: 0x000013EF
		// (set) Token: 0x0600011F RID: 287 RVA: 0x000031F7 File Offset: 0x000013F7
		[InputControl(noisy = true, aliases = new string[] { "PointerOrientation" })]
		public QuaternionControl pointerRotation { get; protected set; }

		// Token: 0x06000120 RID: 288 RVA: 0x00003200 File Offset: 0x00001400
		protected override void FinishSetup()
		{
			base.FinishSetup();
			this.joystick = base.GetChildControl<Vector2Control>("joystick");
			this.trigger = base.GetChildControl<AxisControl>("trigger");
			this.touchpad = base.GetChildControl<Vector2Control>("touchpad");
			this.grip = base.GetChildControl<AxisControl>("grip");
			this.gripPressed = base.GetChildControl<ButtonControl>("gripPressed");
			this.menu = base.GetChildControl<ButtonControl>("menu");
			this.joystickClicked = base.GetChildControl<ButtonControl>("joystickClicked");
			this.triggerPressed = base.GetChildControl<ButtonControl>("triggerPressed");
			this.touchpadClicked = base.GetChildControl<ButtonControl>("touchpadClicked");
			this.touchpadTouched = base.GetChildControl<ButtonControl>("touchPadTouched");
			this.deviceVelocity = base.GetChildControl<Vector3Control>("deviceVelocity");
			this.deviceAngularVelocity = base.GetChildControl<Vector3Control>("deviceAngularVelocity");
			this.batteryLevel = base.GetChildControl<AxisControl>("batteryLevel");
			this.sourceLossRisk = base.GetChildControl<AxisControl>("sourceLossRisk");
			this.sourceLossMitigationDirection = base.GetChildControl<Vector3Control>("sourceLossMitigationDirection");
			this.pointerPosition = base.GetChildControl<Vector3Control>("pointerPosition");
			this.pointerRotation = base.GetChildControl<QuaternionControl>("pointerRotation");
		}
	}
}
