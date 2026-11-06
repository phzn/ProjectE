using System;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

namespace Unity.XR.Oculus.Input
{
	// Token: 0x0200000E RID: 14
	[InputControlLayout(displayName = "Oculus Remote", hideInUI = true)]
	public class OculusRemote : InputDevice
	{
		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x00002C58 File Offset: 0x00000E58
		// (set) Token: 0x060000B6 RID: 182 RVA: 0x00002C60 File Offset: 0x00000E60
		[InputControl]
		public ButtonControl back { get; protected set; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x00002C69 File Offset: 0x00000E69
		// (set) Token: 0x060000B8 RID: 184 RVA: 0x00002C71 File Offset: 0x00000E71
		[InputControl]
		public ButtonControl start { get; protected set; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x00002C7A File Offset: 0x00000E7A
		// (set) Token: 0x060000BA RID: 186 RVA: 0x00002C82 File Offset: 0x00000E82
		[InputControl]
		public Vector2Control touchpad { get; protected set; }

		// Token: 0x060000BB RID: 187 RVA: 0x00002C8B File Offset: 0x00000E8B
		protected override void FinishSetup()
		{
			base.FinishSetup();
			this.back = base.GetChildControl<ButtonControl>("back");
			this.start = base.GetChildControl<ButtonControl>("start");
			this.touchpad = base.GetChildControl<Vector2Control>("touchpad");
		}
	}
}
