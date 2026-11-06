using System;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

namespace UnityEngine.InputSystem
{
	// Token: 0x02000057 RID: 87
	[InputControlLayout(displayName = "Tracked Device", isGenericTypeOfDevice = true)]
	public class TrackedDevice : InputDevice
	{
		// Token: 0x1700021C RID: 540
		// (get) Token: 0x0600085B RID: 2139 RVA: 0x0002ED6E File Offset: 0x0002CF6E
		// (set) Token: 0x0600085C RID: 2140 RVA: 0x0002ED76 File Offset: 0x0002CF76
		[InputControl(synthetic = true)]
		public IntegerControl trackingState { get; protected set; }

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x0600085D RID: 2141 RVA: 0x0002ED7F File Offset: 0x0002CF7F
		// (set) Token: 0x0600085E RID: 2142 RVA: 0x0002ED87 File Offset: 0x0002CF87
		[InputControl(synthetic = true)]
		public ButtonControl isTracked { get; protected set; }

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x0600085F RID: 2143 RVA: 0x0002ED90 File Offset: 0x0002CF90
		// (set) Token: 0x06000860 RID: 2144 RVA: 0x0002ED98 File Offset: 0x0002CF98
		[InputControl(noisy = true, dontReset = true)]
		public Vector3Control devicePosition { get; protected set; }

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000861 RID: 2145 RVA: 0x0002EDA1 File Offset: 0x0002CFA1
		// (set) Token: 0x06000862 RID: 2146 RVA: 0x0002EDA9 File Offset: 0x0002CFA9
		[InputControl(noisy = true, dontReset = true)]
		public QuaternionControl deviceRotation { get; protected set; }

		// Token: 0x06000863 RID: 2147 RVA: 0x0002EDB4 File Offset: 0x0002CFB4
		protected override void FinishSetup()
		{
			base.FinishSetup();
			this.trackingState = base.GetChildControl<IntegerControl>("trackingState");
			this.isTracked = base.GetChildControl<ButtonControl>("isTracked");
			this.devicePosition = base.GetChildControl<Vector3Control>("devicePosition");
			this.deviceRotation = base.GetChildControl<QuaternionControl>("deviceRotation");
		}
	}
}
