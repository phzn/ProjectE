using System;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Scripting;
using UnityEngine.XR;

namespace UnityEngine.InputSystem.XR
{
	// Token: 0x02000064 RID: 100
	[Preserve]
	[InputControlLayout(stateType = typeof(PoseState))]
	public class PoseControl : InputControl<PoseState>
	{
		// Token: 0x1700027A RID: 634
		// (get) Token: 0x060009CE RID: 2510 RVA: 0x00036C37 File Offset: 0x00034E37
		// (set) Token: 0x060009CF RID: 2511 RVA: 0x00036C3F File Offset: 0x00034E3F
		public ButtonControl isTracked { get; set; }

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x060009D0 RID: 2512 RVA: 0x00036C48 File Offset: 0x00034E48
		// (set) Token: 0x060009D1 RID: 2513 RVA: 0x00036C50 File Offset: 0x00034E50
		public IntegerControl trackingState { get; set; }

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x060009D2 RID: 2514 RVA: 0x00036C59 File Offset: 0x00034E59
		// (set) Token: 0x060009D3 RID: 2515 RVA: 0x00036C61 File Offset: 0x00034E61
		public Vector3Control position { get; set; }

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x060009D4 RID: 2516 RVA: 0x00036C6A File Offset: 0x00034E6A
		// (set) Token: 0x060009D5 RID: 2517 RVA: 0x00036C72 File Offset: 0x00034E72
		public QuaternionControl rotation { get; set; }

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x060009D6 RID: 2518 RVA: 0x00036C7B File Offset: 0x00034E7B
		// (set) Token: 0x060009D7 RID: 2519 RVA: 0x00036C83 File Offset: 0x00034E83
		public Vector3Control velocity { get; set; }

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x060009D8 RID: 2520 RVA: 0x00036C8C File Offset: 0x00034E8C
		// (set) Token: 0x060009D9 RID: 2521 RVA: 0x00036C94 File Offset: 0x00034E94
		public Vector3Control angularVelocity { get; set; }

		// Token: 0x060009DA RID: 2522 RVA: 0x00036C9D File Offset: 0x00034E9D
		public PoseControl()
		{
			this.m_StateBlock.format = PoseState.s_Format;
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x00036CB8 File Offset: 0x00034EB8
		protected override void FinishSetup()
		{
			this.isTracked = base.GetChildControl<ButtonControl>("isTracked");
			this.trackingState = base.GetChildControl<IntegerControl>("trackingState");
			this.position = base.GetChildControl<Vector3Control>("position");
			this.rotation = base.GetChildControl<QuaternionControl>("rotation");
			this.velocity = base.GetChildControl<Vector3Control>("velocity");
			this.angularVelocity = base.GetChildControl<Vector3Control>("angularVelocity");
			base.FinishSetup();
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x00036D34 File Offset: 0x00034F34
		public unsafe override PoseState ReadUnprocessedValueFromState(void* statePtr)
		{
			if (this.m_OptimizedControlDataType == 1349481317)
			{
				return *(PoseState*)((byte*)statePtr + this.m_StateBlock.byteOffset);
			}
			return new PoseState
			{
				isTracked = (this.isTracked.ReadUnprocessedValueFromStateWithCaching(statePtr) > 0.5f),
				trackingState = (InputTrackingState)this.trackingState.ReadUnprocessedValueFromStateWithCaching(statePtr),
				position = this.position.ReadUnprocessedValueFromStateWithCaching(statePtr),
				rotation = this.rotation.ReadUnprocessedValueFromStateWithCaching(statePtr),
				velocity = this.velocity.ReadUnprocessedValueFromStateWithCaching(statePtr),
				angularVelocity = this.angularVelocity.ReadUnprocessedValueFromStateWithCaching(statePtr)
			};
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x00036DE8 File Offset: 0x00034FE8
		public unsafe override void WriteValueIntoState(PoseState value, void* statePtr)
		{
			if (this.m_OptimizedControlDataType == 1349481317)
			{
				*(PoseState*)((byte*)statePtr + this.m_StateBlock.byteOffset) = value;
				return;
			}
			this.isTracked.WriteValueIntoState(value.isTracked, statePtr);
			this.trackingState.WriteValueIntoState((uint)value.trackingState, statePtr);
			this.position.WriteValueIntoState(value.position, statePtr);
			this.rotation.WriteValueIntoState(value.rotation, statePtr);
			this.velocity.WriteValueIntoState(value.velocity, statePtr);
			this.angularVelocity.WriteValueIntoState(value.angularVelocity, statePtr);
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x00036E88 File Offset: 0x00035088
		protected override FourCC CalculateOptimizedControlDataType()
		{
			if (this.m_StateBlock.sizeInBits == 480U && this.m_StateBlock.bitOffset == 0U && this.isTracked.optimizedControlDataType == 1113150533 && this.trackingState.optimizedControlDataType == 1229870112 && this.position.optimizedControlDataType == 1447379763 && this.rotation.optimizedControlDataType == 1364541780 && this.velocity.optimizedControlDataType == 1447379763 && this.angularVelocity.optimizedControlDataType == 1447379763 && this.trackingState.m_StateBlock.byteOffset == this.isTracked.m_StateBlock.byteOffset + 4U && this.position.m_StateBlock.byteOffset == this.isTracked.m_StateBlock.byteOffset + 8U && this.rotation.m_StateBlock.byteOffset == this.isTracked.m_StateBlock.byteOffset + 20U && this.velocity.m_StateBlock.byteOffset == this.isTracked.m_StateBlock.byteOffset + 36U && this.angularVelocity.m_StateBlock.byteOffset == this.isTracked.m_StateBlock.byteOffset + 48U)
			{
				return 1349481317;
			}
			return 0;
		}
	}
}
