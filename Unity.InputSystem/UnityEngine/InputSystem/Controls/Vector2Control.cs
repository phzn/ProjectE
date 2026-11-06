using System;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.Controls
{
	// Token: 0x02000122 RID: 290
	public class Vector2Control : InputControl<Vector2>
	{
		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x0600107D RID: 4221 RVA: 0x0004F5E4 File Offset: 0x0004D7E4
		// (set) Token: 0x0600107E RID: 4222 RVA: 0x0004F5EC File Offset: 0x0004D7EC
		[InputControl(offset = 0U, displayName = "X")]
		public AxisControl x { get; set; }

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x0600107F RID: 4223 RVA: 0x0004F5F5 File Offset: 0x0004D7F5
		// (set) Token: 0x06001080 RID: 4224 RVA: 0x0004F5FD File Offset: 0x0004D7FD
		[InputControl(offset = 4U, displayName = "Y")]
		public AxisControl y { get; set; }

		// Token: 0x06001081 RID: 4225 RVA: 0x0004F606 File Offset: 0x0004D806
		public Vector2Control()
		{
			this.m_StateBlock.format = InputStateBlock.FormatVector2;
		}

		// Token: 0x06001082 RID: 4226 RVA: 0x0004F61E File Offset: 0x0004D81E
		protected override void FinishSetup()
		{
			this.x = base.GetChildControl<AxisControl>("x");
			this.y = base.GetChildControl<AxisControl>("y");
			base.FinishSetup();
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x0004F648 File Offset: 0x0004D848
		public unsafe override Vector2 ReadUnprocessedValueFromState(void* statePtr)
		{
			if (this.m_OptimizedControlDataType == 1447379762)
			{
				return *(Vector2*)((byte*)statePtr + this.m_StateBlock.byteOffset);
			}
			return new Vector2(this.x.ReadUnprocessedValueFromStateWithCaching(statePtr), this.y.ReadUnprocessedValueFromStateWithCaching(statePtr));
		}

		// Token: 0x06001084 RID: 4228 RVA: 0x0004F698 File Offset: 0x0004D898
		public unsafe override void WriteValueIntoState(Vector2 value, void* statePtr)
		{
			if (this.m_OptimizedControlDataType == 1447379762)
			{
				*(Vector2*)((byte*)statePtr + this.m_StateBlock.byteOffset) = value;
				return;
			}
			this.x.WriteValueIntoState(value.x, statePtr);
			this.y.WriteValueIntoState(value.y, statePtr);
		}

		// Token: 0x06001085 RID: 4229 RVA: 0x0004F6F0 File Offset: 0x0004D8F0
		public unsafe override float EvaluateMagnitude(void* statePtr)
		{
			return base.ReadValueFromStateWithCaching(statePtr).magnitude;
		}

		// Token: 0x06001086 RID: 4230 RVA: 0x0004F70C File Offset: 0x0004D90C
		protected override FourCC CalculateOptimizedControlDataType()
		{
			if (this.m_StateBlock.sizeInBits == 64U && this.m_StateBlock.bitOffset == 0U && this.x.optimizedControlDataType == InputStateBlock.FormatFloat && this.y.optimizedControlDataType == InputStateBlock.FormatFloat && this.y.m_StateBlock.byteOffset == this.x.m_StateBlock.byteOffset + 4U)
			{
				return InputStateBlock.FormatVector2;
			}
			return InputStateBlock.FormatInvalid;
		}
	}
}
