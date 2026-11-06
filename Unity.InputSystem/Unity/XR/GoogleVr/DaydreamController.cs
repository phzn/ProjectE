using System;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.XR;

namespace Unity.XR.GoogleVr
{
	// Token: 0x02000012 RID: 18
	[InputControlLayout(displayName = "Daydream Controller", commonUsages = new string[] { "LeftHand", "RightHand" }, hideInUI = true)]
	public class DaydreamController : XRController
	{
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x00002E78 File Offset: 0x00001078
		// (set) Token: 0x060000D9 RID: 217 RVA: 0x00002E80 File Offset: 0x00001080
		[InputControl]
		public Vector2Control touchpad { get; protected set; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060000DA RID: 218 RVA: 0x00002E89 File Offset: 0x00001089
		// (set) Token: 0x060000DB RID: 219 RVA: 0x00002E91 File Offset: 0x00001091
		[InputControl]
		public ButtonControl volumeUp { get; protected set; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060000DC RID: 220 RVA: 0x00002E9A File Offset: 0x0000109A
		// (set) Token: 0x060000DD RID: 221 RVA: 0x00002EA2 File Offset: 0x000010A2
		[InputControl]
		public ButtonControl recentered { get; protected set; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060000DE RID: 222 RVA: 0x00002EAB File Offset: 0x000010AB
		// (set) Token: 0x060000DF RID: 223 RVA: 0x00002EB3 File Offset: 0x000010B3
		[InputControl]
		public ButtonControl volumeDown { get; protected set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00002EBC File Offset: 0x000010BC
		// (set) Token: 0x060000E1 RID: 225 RVA: 0x00002EC4 File Offset: 0x000010C4
		[InputControl]
		public ButtonControl recentering { get; protected set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00002ECD File Offset: 0x000010CD
		// (set) Token: 0x060000E3 RID: 227 RVA: 0x00002ED5 File Offset: 0x000010D5
		[InputControl]
		public ButtonControl app { get; protected set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x00002EDE File Offset: 0x000010DE
		// (set) Token: 0x060000E5 RID: 229 RVA: 0x00002EE6 File Offset: 0x000010E6
		[InputControl]
		public ButtonControl home { get; protected set; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x00002EEF File Offset: 0x000010EF
		// (set) Token: 0x060000E7 RID: 231 RVA: 0x00002EF7 File Offset: 0x000010F7
		[InputControl]
		public ButtonControl touchpadClicked { get; protected set; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x00002F00 File Offset: 0x00001100
		// (set) Token: 0x060000E9 RID: 233 RVA: 0x00002F08 File Offset: 0x00001108
		[InputControl]
		public ButtonControl touchpadTouched { get; protected set; }

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060000EA RID: 234 RVA: 0x00002F11 File Offset: 0x00001111
		// (set) Token: 0x060000EB RID: 235 RVA: 0x00002F19 File Offset: 0x00001119
		[InputControl(noisy = true)]
		public Vector3Control deviceVelocity { get; protected set; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060000EC RID: 236 RVA: 0x00002F22 File Offset: 0x00001122
		// (set) Token: 0x060000ED RID: 237 RVA: 0x00002F2A File Offset: 0x0000112A
		[InputControl(noisy = true)]
		public Vector3Control deviceAcceleration { get; protected set; }

		// Token: 0x060000EE RID: 238 RVA: 0x00002F34 File Offset: 0x00001134
		protected override void FinishSetup()
		{
			base.FinishSetup();
			this.touchpad = base.GetChildControl<Vector2Control>("touchpad");
			this.volumeUp = base.GetChildControl<ButtonControl>("volumeUp");
			this.recentered = base.GetChildControl<ButtonControl>("recentered");
			this.volumeDown = base.GetChildControl<ButtonControl>("volumeDown");
			this.recentering = base.GetChildControl<ButtonControl>("recentering");
			this.app = base.GetChildControl<ButtonControl>("app");
			this.home = base.GetChildControl<ButtonControl>("home");
			this.touchpadClicked = base.GetChildControl<ButtonControl>("touchpadClicked");
			this.touchpadTouched = base.GetChildControl<ButtonControl>("touchpadTouched");
			this.deviceVelocity = base.GetChildControl<Vector3Control>("deviceVelocity");
			this.deviceAcceleration = base.GetChildControl<Vector3Control>("deviceAcceleration");
		}
	}
}
