using System;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.XR;

namespace Unity.XR.OpenVR
{
	// Token: 0x02000004 RID: 4
	[InputControlLayout(displayName = "OpenVR Headset", hideInUI = true)]
	public class OpenVRHMD : XRHMD
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000004 RID: 4 RVA: 0x000020EA File Offset: 0x000002EA
		// (set) Token: 0x06000005 RID: 5 RVA: 0x000020F2 File Offset: 0x000002F2
		[InputControl(noisy = true)]
		public Vector3Control deviceVelocity { get; protected set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000006 RID: 6 RVA: 0x000020FB File Offset: 0x000002FB
		// (set) Token: 0x06000007 RID: 7 RVA: 0x00002103 File Offset: 0x00000303
		[InputControl(noisy = true)]
		public Vector3Control deviceAngularVelocity { get; protected set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000008 RID: 8 RVA: 0x0000210C File Offset: 0x0000030C
		// (set) Token: 0x06000009 RID: 9 RVA: 0x00002114 File Offset: 0x00000314
		[InputControl(noisy = true)]
		public Vector3Control leftEyeVelocity { get; protected set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000A RID: 10 RVA: 0x0000211D File Offset: 0x0000031D
		// (set) Token: 0x0600000B RID: 11 RVA: 0x00002125 File Offset: 0x00000325
		[InputControl(noisy = true)]
		public Vector3Control leftEyeAngularVelocity { get; protected set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000C RID: 12 RVA: 0x0000212E File Offset: 0x0000032E
		// (set) Token: 0x0600000D RID: 13 RVA: 0x00002136 File Offset: 0x00000336
		[InputControl(noisy = true)]
		public Vector3Control rightEyeVelocity { get; protected set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000E RID: 14 RVA: 0x0000213F File Offset: 0x0000033F
		// (set) Token: 0x0600000F RID: 15 RVA: 0x00002147 File Offset: 0x00000347
		[InputControl(noisy = true)]
		public Vector3Control rightEyeAngularVelocity { get; protected set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000010 RID: 16 RVA: 0x00002150 File Offset: 0x00000350
		// (set) Token: 0x06000011 RID: 17 RVA: 0x00002158 File Offset: 0x00000358
		[InputControl(noisy = true)]
		public Vector3Control centerEyeVelocity { get; protected set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000012 RID: 18 RVA: 0x00002161 File Offset: 0x00000361
		// (set) Token: 0x06000013 RID: 19 RVA: 0x00002169 File Offset: 0x00000369
		[InputControl(noisy = true)]
		public Vector3Control centerEyeAngularVelocity { get; protected set; }

		// Token: 0x06000014 RID: 20 RVA: 0x00002174 File Offset: 0x00000374
		protected override void FinishSetup()
		{
			base.FinishSetup();
			this.deviceVelocity = base.GetChildControl<Vector3Control>("deviceVelocity");
			this.deviceAngularVelocity = base.GetChildControl<Vector3Control>("deviceAngularVelocity");
			this.leftEyeVelocity = base.GetChildControl<Vector3Control>("leftEyeVelocity");
			this.leftEyeAngularVelocity = base.GetChildControl<Vector3Control>("leftEyeAngularVelocity");
			this.rightEyeVelocity = base.GetChildControl<Vector3Control>("rightEyeVelocity");
			this.rightEyeAngularVelocity = base.GetChildControl<Vector3Control>("rightEyeAngularVelocity");
			this.centerEyeVelocity = base.GetChildControl<Vector3Control>("centerEyeVelocity");
			this.centerEyeAngularVelocity = base.GetChildControl<Vector3Control>("centerEyeAngularVelocity");
		}
	}
}
