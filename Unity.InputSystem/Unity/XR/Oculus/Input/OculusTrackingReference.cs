using System;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

namespace Unity.XR.Oculus.Input
{
	// Token: 0x0200000D RID: 13
	public class OculusTrackingReference : TrackedDevice
	{
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00002C04 File Offset: 0x00000E04
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x00002C0C File Offset: 0x00000E0C
		[InputControl(aliases = new string[] { "trackingReferenceTrackingState" })]
		public new IntegerControl trackingState { get; protected set; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x00002C15 File Offset: 0x00000E15
		// (set) Token: 0x060000B2 RID: 178 RVA: 0x00002C1D File Offset: 0x00000E1D
		[InputControl(aliases = new string[] { "trackingReferenceIsTracked" })]
		public new ButtonControl isTracked { get; protected set; }

		// Token: 0x060000B3 RID: 179 RVA: 0x00002C26 File Offset: 0x00000E26
		protected override void FinishSetup()
		{
			base.FinishSetup();
			this.trackingState = base.GetChildControl<IntegerControl>("trackingState");
			this.isTracked = base.GetChildControl<ButtonControl>("isTracked");
		}
	}
}
