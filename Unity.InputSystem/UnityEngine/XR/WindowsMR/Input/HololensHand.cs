using System;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.XR;

namespace UnityEngine.XR.WindowsMR.Input
{
	// Token: 0x02000014 RID: 20
	[InputControlLayout(displayName = "HoloLens Hand", commonUsages = new string[] { "LeftHand", "RightHand" }, hideInUI = true)]
	public class HololensHand : XRController
	{
		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x0000303C File Offset: 0x0000123C
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x00003044 File Offset: 0x00001244
		[InputControl(noisy = true, aliases = new string[] { "gripVelocity" })]
		public Vector3Control deviceVelocity { get; protected set; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x0000304D File Offset: 0x0000124D
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x00003055 File Offset: 0x00001255
		[InputControl(aliases = new string[] { "triggerbutton" })]
		public ButtonControl airTap { get; protected set; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x0000305E File Offset: 0x0000125E
		// (set) Token: 0x060000F9 RID: 249 RVA: 0x00003066 File Offset: 0x00001266
		[InputControl(noisy = true)]
		public AxisControl sourceLossRisk { get; protected set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060000FA RID: 250 RVA: 0x0000306F File Offset: 0x0000126F
		// (set) Token: 0x060000FB RID: 251 RVA: 0x00003077 File Offset: 0x00001277
		[InputControl(noisy = true)]
		public Vector3Control sourceLossMitigationDirection { get; protected set; }

		// Token: 0x060000FC RID: 252 RVA: 0x00003080 File Offset: 0x00001280
		protected override void FinishSetup()
		{
			base.FinishSetup();
			this.airTap = base.GetChildControl<ButtonControl>("airTap");
			this.deviceVelocity = base.GetChildControl<Vector3Control>("deviceVelocity");
			this.sourceLossRisk = base.GetChildControl<AxisControl>("sourceLossRisk");
			this.sourceLossMitigationDirection = base.GetChildControl<Vector3Control>("sourceLossMitigationDirection");
		}
	}
}
