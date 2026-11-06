using System;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

namespace UnityEngine.InputSystem.XR
{
	// Token: 0x02000065 RID: 101
	[InputControlLayout(isGenericTypeOfDevice = true, displayName = "XR HMD", canRunInBackground = true)]
	public class XRHMD : TrackedDevice
	{
		// Token: 0x17000280 RID: 640
		// (get) Token: 0x060009DF RID: 2527 RVA: 0x0003703F File Offset: 0x0003523F
		// (set) Token: 0x060009E0 RID: 2528 RVA: 0x00037047 File Offset: 0x00035247
		[InputControl(noisy = true)]
		public Vector3Control leftEyePosition { get; protected set; }

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x060009E1 RID: 2529 RVA: 0x00037050 File Offset: 0x00035250
		// (set) Token: 0x060009E2 RID: 2530 RVA: 0x00037058 File Offset: 0x00035258
		[InputControl(noisy = true)]
		public QuaternionControl leftEyeRotation { get; protected set; }

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x060009E3 RID: 2531 RVA: 0x00037061 File Offset: 0x00035261
		// (set) Token: 0x060009E4 RID: 2532 RVA: 0x00037069 File Offset: 0x00035269
		[InputControl(noisy = true)]
		public Vector3Control rightEyePosition { get; protected set; }

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x060009E5 RID: 2533 RVA: 0x00037072 File Offset: 0x00035272
		// (set) Token: 0x060009E6 RID: 2534 RVA: 0x0003707A File Offset: 0x0003527A
		[InputControl(noisy = true)]
		public QuaternionControl rightEyeRotation { get; protected set; }

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x060009E7 RID: 2535 RVA: 0x00037083 File Offset: 0x00035283
		// (set) Token: 0x060009E8 RID: 2536 RVA: 0x0003708B File Offset: 0x0003528B
		[InputControl(noisy = true)]
		public Vector3Control centerEyePosition { get; protected set; }

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x060009E9 RID: 2537 RVA: 0x00037094 File Offset: 0x00035294
		// (set) Token: 0x060009EA RID: 2538 RVA: 0x0003709C File Offset: 0x0003529C
		[InputControl(noisy = true)]
		public QuaternionControl centerEyeRotation { get; protected set; }

		// Token: 0x060009EB RID: 2539 RVA: 0x000370A8 File Offset: 0x000352A8
		protected override void FinishSetup()
		{
			base.FinishSetup();
			this.centerEyePosition = base.GetChildControl<Vector3Control>("centerEyePosition");
			this.centerEyeRotation = base.GetChildControl<QuaternionControl>("centerEyeRotation");
			this.leftEyePosition = base.GetChildControl<Vector3Control>("leftEyePosition");
			this.leftEyeRotation = base.GetChildControl<QuaternionControl>("leftEyeRotation");
			this.rightEyePosition = base.GetChildControl<Vector3Control>("rightEyePosition");
			this.rightEyeRotation = base.GetChildControl<QuaternionControl>("rightEyeRotation");
		}
	}
}
