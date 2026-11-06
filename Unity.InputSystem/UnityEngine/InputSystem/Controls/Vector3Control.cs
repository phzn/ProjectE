using System;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.Controls
{
	// Token: 0x02000123 RID: 291
	public class Vector3Control : InputControl<Vector3>
	{
		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x06001087 RID: 4231 RVA: 0x0004F792 File Offset: 0x0004D992
		// (set) Token: 0x06001088 RID: 4232 RVA: 0x0004F79A File Offset: 0x0004D99A
		[InputControl(offset = 0U, displayName = "X")]
		public AxisControl x { get; set; }

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x06001089 RID: 4233 RVA: 0x0004F7A3 File Offset: 0x0004D9A3
		// (set) Token: 0x0600108A RID: 4234 RVA: 0x0004F7AB File Offset: 0x0004D9AB
		[InputControl(offset = 4U, displayName = "Y")]
		public AxisControl y { get; set; }

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x0600108B RID: 4235 RVA: 0x0004F7B4 File Offset: 0x0004D9B4
		// (set) Token: 0x0600108C RID: 4236 RVA: 0x0004F7BC File Offset: 0x0004D9BC
		[InputControl(offset = 8U, displayName = "Z")]
		public AxisControl z { get; set; }

		// Token: 0x0600108D RID: 4237 RVA: 0x0004F7C5 File Offset: 0x0004D9C5
		public Vector3Control()
		{
			this.m_StateBlock.format = InputStateBlock.FormatVector3;
		}

		// Token: 0x0600108E RID: 4238 RVA: 0x0004F7DD File Offset: 0x0004D9DD
		protected override void FinishSetup()
		{
			this.x = base.GetChildControl<AxisControl>("x");
			this.y = base.GetChildControl<AxisControl>("y");
			this.z = base.GetChildControl<AxisControl>("z");
			base.FinishSetup();
		}

		// Token: 0x0600108F RID: 4239 RVA: 0x0004F818 File Offset: 0x0004DA18
		public unsafe override Vector3 ReadUnprocessedValueFromState(void* statePtr)
		{
			if (this.m_OptimizedControlDataType == 1447379763)
			{
				return *(Vector3*)((byte*)statePtr + this.m_StateBlock.byteOffset);
			}
			return new Vector3(this.x.ReadUnprocessedValueFromStateWithCaching(statePtr), this.y.ReadUnprocessedValueFromStateWithCaching(statePtr), this.z.ReadUnprocessedValueFromStateWithCaching(statePtr));
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x0004F874 File Offset: 0x0004DA74
		public unsafe override void WriteValueIntoState(Vector3 value, void* statePtr)
		{
			if (this.m_OptimizedControlDataType == 1447379763)
			{
				*(Vector3*)((byte*)statePtr + this.m_StateBlock.byteOffset) = value;
				return;
			}
			this.x.WriteValueIntoState(value.x, statePtr);
			this.y.WriteValueIntoState(value.y, statePtr);
			this.z.WriteValueIntoState(value.z, statePtr);
		}

		// Token: 0x06001091 RID: 4241 RVA: 0x0004F8E0 File Offset: 0x0004DAE0
		public unsafe override float EvaluateMagnitude(void* statePtr)
		{
			return base.ReadValueFromStateWithCaching(statePtr).magnitude;
		}

		// Token: 0x06001092 RID: 4242 RVA: 0x0004F8FC File Offset: 0x0004DAFC
		protected override FourCC CalculateOptimizedControlDataType()
		{
			if (this.m_StateBlock.sizeInBits == 96U && this.m_StateBlock.bitOffset == 0U && this.x.optimizedControlDataType == InputStateBlock.FormatFloat && this.y.optimizedControlDataType == InputStateBlock.FormatFloat && this.z.optimizedControlDataType == InputStateBlock.FormatFloat && this.y.m_StateBlock.byteOffset == this.x.m_StateBlock.byteOffset + 4U && this.z.m_StateBlock.byteOffset == this.x.m_StateBlock.byteOffset + 8U)
			{
				return InputStateBlock.FormatVector3;
			}
			return InputStateBlock.FormatInvalid;
		}
	}
}
