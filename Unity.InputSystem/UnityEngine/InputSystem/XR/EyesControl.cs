using System;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

namespace UnityEngine.InputSystem.XR
{
	// Token: 0x02000072 RID: 114
	public class EyesControl : InputControl<Eyes>
	{
		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06000A4F RID: 2639 RVA: 0x0003846E File Offset: 0x0003666E
		// (set) Token: 0x06000A50 RID: 2640 RVA: 0x00038476 File Offset: 0x00036676
		[InputControl(offset = 0U, displayName = "LeftEyePosition")]
		public Vector3Control leftEyePosition { get; set; }

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000A51 RID: 2641 RVA: 0x0003847F File Offset: 0x0003667F
		// (set) Token: 0x06000A52 RID: 2642 RVA: 0x00038487 File Offset: 0x00036687
		[InputControl(offset = 12U, displayName = "LeftEyeRotation")]
		public QuaternionControl leftEyeRotation { get; set; }

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000A53 RID: 2643 RVA: 0x00038490 File Offset: 0x00036690
		// (set) Token: 0x06000A54 RID: 2644 RVA: 0x00038498 File Offset: 0x00036698
		[InputControl(offset = 28U, displayName = "RightEyePosition")]
		public Vector3Control rightEyePosition { get; set; }

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000A55 RID: 2645 RVA: 0x000384A1 File Offset: 0x000366A1
		// (set) Token: 0x06000A56 RID: 2646 RVA: 0x000384A9 File Offset: 0x000366A9
		[InputControl(offset = 40U, displayName = "RightEyeRotation")]
		public QuaternionControl rightEyeRotation { get; set; }

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06000A57 RID: 2647 RVA: 0x000384B2 File Offset: 0x000366B2
		// (set) Token: 0x06000A58 RID: 2648 RVA: 0x000384BA File Offset: 0x000366BA
		[InputControl(offset = 56U, displayName = "FixationPoint")]
		public Vector3Control fixationPoint { get; set; }

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000A59 RID: 2649 RVA: 0x000384C3 File Offset: 0x000366C3
		// (set) Token: 0x06000A5A RID: 2650 RVA: 0x000384CB File Offset: 0x000366CB
		[InputControl(offset = 68U, displayName = "LeftEyeOpenAmount")]
		public AxisControl leftEyeOpenAmount { get; set; }

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000A5B RID: 2651 RVA: 0x000384D4 File Offset: 0x000366D4
		// (set) Token: 0x06000A5C RID: 2652 RVA: 0x000384DC File Offset: 0x000366DC
		[InputControl(offset = 72U, displayName = "RightEyeOpenAmount")]
		public AxisControl rightEyeOpenAmount { get; set; }

		// Token: 0x06000A5D RID: 2653 RVA: 0x000384E8 File Offset: 0x000366E8
		protected override void FinishSetup()
		{
			this.leftEyePosition = base.GetChildControl<Vector3Control>("leftEyePosition");
			this.leftEyeRotation = base.GetChildControl<QuaternionControl>("leftEyeRotation");
			this.rightEyePosition = base.GetChildControl<Vector3Control>("rightEyePosition");
			this.rightEyeRotation = base.GetChildControl<QuaternionControl>("rightEyeRotation");
			this.fixationPoint = base.GetChildControl<Vector3Control>("fixationPoint");
			this.leftEyeOpenAmount = base.GetChildControl<AxisControl>("leftEyeOpenAmount");
			this.rightEyeOpenAmount = base.GetChildControl<AxisControl>("rightEyeOpenAmount");
			base.FinishSetup();
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x00038574 File Offset: 0x00036774
		public unsafe override Eyes ReadUnprocessedValueFromState(void* statePtr)
		{
			return new Eyes
			{
				leftEyePosition = this.leftEyePosition.ReadUnprocessedValueFromStateWithCaching(statePtr),
				leftEyeRotation = this.leftEyeRotation.ReadUnprocessedValueFromStateWithCaching(statePtr),
				rightEyePosition = this.rightEyePosition.ReadUnprocessedValueFromStateWithCaching(statePtr),
				rightEyeRotation = this.rightEyeRotation.ReadUnprocessedValueFromStateWithCaching(statePtr),
				fixationPoint = this.fixationPoint.ReadUnprocessedValueFromStateWithCaching(statePtr),
				leftEyeOpenAmount = this.leftEyeOpenAmount.ReadUnprocessedValueFromStateWithCaching(statePtr),
				rightEyeOpenAmount = this.rightEyeOpenAmount.ReadUnprocessedValueFromStateWithCaching(statePtr)
			};
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x00038610 File Offset: 0x00036810
		public unsafe override void WriteValueIntoState(Eyes value, void* statePtr)
		{
			this.leftEyePosition.WriteValueIntoState(value.leftEyePosition, statePtr);
			this.leftEyeRotation.WriteValueIntoState(value.leftEyeRotation, statePtr);
			this.rightEyePosition.WriteValueIntoState(value.rightEyePosition, statePtr);
			this.rightEyeRotation.WriteValueIntoState(value.rightEyeRotation, statePtr);
			this.fixationPoint.WriteValueIntoState(value.fixationPoint, statePtr);
			this.leftEyeOpenAmount.WriteValueIntoState(value.leftEyeOpenAmount, statePtr);
			this.rightEyeOpenAmount.WriteValueIntoState(value.rightEyeOpenAmount, statePtr);
		}
	}
}
