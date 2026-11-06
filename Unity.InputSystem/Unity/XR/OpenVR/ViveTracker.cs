using System;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

namespace Unity.XR.OpenVR
{
	// Token: 0x02000008 RID: 8
	[InputControlLayout(displayName = "Vive Tracker")]
	public class ViveTracker : TrackedDevice
	{
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00002521 File Offset: 0x00000721
		// (set) Token: 0x06000046 RID: 70 RVA: 0x00002529 File Offset: 0x00000729
		[InputControl(noisy = true)]
		public Vector3Control deviceVelocity { get; protected set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00002532 File Offset: 0x00000732
		// (set) Token: 0x06000048 RID: 72 RVA: 0x0000253A File Offset: 0x0000073A
		[InputControl(noisy = true)]
		public Vector3Control deviceAngularVelocity { get; protected set; }

		// Token: 0x06000049 RID: 73 RVA: 0x00002543 File Offset: 0x00000743
		protected override void FinishSetup()
		{
			base.FinishSetup();
			this.deviceVelocity = base.GetChildControl<Vector3Control>("deviceVelocity");
			this.deviceAngularVelocity = base.GetChildControl<Vector3Control>("deviceAngularVelocity");
		}
	}
}
