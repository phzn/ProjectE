using System;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.XR;

namespace Unity.XR.Oculus.Input
{
	// Token: 0x02000010 RID: 16
	[InputControlLayout(displayName = "GearVR Controller", commonUsages = new string[] { "LeftHand", "RightHand" }, hideInUI = true)]
	public class GearVRTrackedController : XRController
	{
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00002D22 File Offset: 0x00000F22
		// (set) Token: 0x060000C4 RID: 196 RVA: 0x00002D2A File Offset: 0x00000F2A
		[InputControl]
		public Vector2Control touchpad { get; protected set; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x00002D33 File Offset: 0x00000F33
		// (set) Token: 0x060000C6 RID: 198 RVA: 0x00002D3B File Offset: 0x00000F3B
		[InputControl]
		public AxisControl trigger { get; protected set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00002D44 File Offset: 0x00000F44
		// (set) Token: 0x060000C8 RID: 200 RVA: 0x00002D4C File Offset: 0x00000F4C
		[InputControl]
		public ButtonControl back { get; protected set; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x00002D55 File Offset: 0x00000F55
		// (set) Token: 0x060000CA RID: 202 RVA: 0x00002D5D File Offset: 0x00000F5D
		[InputControl]
		public ButtonControl triggerPressed { get; protected set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060000CB RID: 203 RVA: 0x00002D66 File Offset: 0x00000F66
		// (set) Token: 0x060000CC RID: 204 RVA: 0x00002D6E File Offset: 0x00000F6E
		[InputControl]
		public ButtonControl touchpadClicked { get; protected set; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060000CD RID: 205 RVA: 0x00002D77 File Offset: 0x00000F77
		// (set) Token: 0x060000CE RID: 206 RVA: 0x00002D7F File Offset: 0x00000F7F
		[InputControl]
		public ButtonControl touchpadTouched { get; protected set; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060000CF RID: 207 RVA: 0x00002D88 File Offset: 0x00000F88
		// (set) Token: 0x060000D0 RID: 208 RVA: 0x00002D90 File Offset: 0x00000F90
		[InputControl(noisy = true)]
		public Vector3Control deviceAngularVelocity { get; protected set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x00002D99 File Offset: 0x00000F99
		// (set) Token: 0x060000D2 RID: 210 RVA: 0x00002DA1 File Offset: 0x00000FA1
		[InputControl(noisy = true)]
		public Vector3Control deviceAcceleration { get; protected set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x00002DAA File Offset: 0x00000FAA
		// (set) Token: 0x060000D4 RID: 212 RVA: 0x00002DB2 File Offset: 0x00000FB2
		[InputControl(noisy = true)]
		public Vector3Control deviceAngularAcceleration { get; protected set; }

		// Token: 0x060000D5 RID: 213 RVA: 0x00002DBC File Offset: 0x00000FBC
		protected override void FinishSetup()
		{
			base.FinishSetup();
			this.touchpad = base.GetChildControl<Vector2Control>("touchpad");
			this.trigger = base.GetChildControl<AxisControl>("trigger");
			this.back = base.GetChildControl<ButtonControl>("back");
			this.triggerPressed = base.GetChildControl<ButtonControl>("triggerPressed");
			this.touchpadClicked = base.GetChildControl<ButtonControl>("touchpadClicked");
			this.touchpadTouched = base.GetChildControl<ButtonControl>("touchpadTouched");
			this.deviceAngularVelocity = base.GetChildControl<Vector3Control>("deviceAngularVelocity");
			this.deviceAcceleration = base.GetChildControl<Vector3Control>("deviceAcceleration");
			this.deviceAngularAcceleration = base.GetChildControl<Vector3Control>("deviceAngularAcceleration");
		}
	}
}
