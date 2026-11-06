using System;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.Controls
{
	// Token: 0x0200011D RID: 285
	public class QuaternionControl : InputControl<Quaternion>
	{
		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x06001040 RID: 4160 RVA: 0x0004EEEB File Offset: 0x0004D0EB
		// (set) Token: 0x06001041 RID: 4161 RVA: 0x0004EEF3 File Offset: 0x0004D0F3
		[InputControl(displayName = "X")]
		public AxisControl x { get; set; }

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x06001042 RID: 4162 RVA: 0x0004EEFC File Offset: 0x0004D0FC
		// (set) Token: 0x06001043 RID: 4163 RVA: 0x0004EF04 File Offset: 0x0004D104
		[InputControl(displayName = "Y")]
		public AxisControl y { get; set; }

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x06001044 RID: 4164 RVA: 0x0004EF0D File Offset: 0x0004D10D
		// (set) Token: 0x06001045 RID: 4165 RVA: 0x0004EF15 File Offset: 0x0004D115
		[InputControl(displayName = "Z")]
		public AxisControl z { get; set; }

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x06001046 RID: 4166 RVA: 0x0004EF1E File Offset: 0x0004D11E
		// (set) Token: 0x06001047 RID: 4167 RVA: 0x0004EF26 File Offset: 0x0004D126
		[InputControl(displayName = "W")]
		public AxisControl w { get; set; }

		// Token: 0x06001048 RID: 4168 RVA: 0x0004EF2F File Offset: 0x0004D12F
		public QuaternionControl()
		{
			this.m_StateBlock.sizeInBits = 128U;
			this.m_StateBlock.format = InputStateBlock.FormatQuaternion;
		}

		// Token: 0x06001049 RID: 4169 RVA: 0x0004EF58 File Offset: 0x0004D158
		protected override void FinishSetup()
		{
			this.x = base.GetChildControl<AxisControl>("x");
			this.y = base.GetChildControl<AxisControl>("y");
			this.z = base.GetChildControl<AxisControl>("z");
			this.w = base.GetChildControl<AxisControl>("w");
			base.FinishSetup();
		}

		// Token: 0x0600104A RID: 4170 RVA: 0x0004EFB0 File Offset: 0x0004D1B0
		public unsafe override Quaternion ReadUnprocessedValueFromState(void* statePtr)
		{
			if (this.m_OptimizedControlDataType == 1364541780)
			{
				return *(Quaternion*)((byte*)statePtr + this.m_StateBlock.byteOffset);
			}
			return new Quaternion(this.x.ReadValueFromStateWithCaching(statePtr), this.y.ReadValueFromStateWithCaching(statePtr), this.z.ReadValueFromStateWithCaching(statePtr), this.w.ReadUnprocessedValueFromStateWithCaching(statePtr));
		}

		// Token: 0x0600104B RID: 4171 RVA: 0x0004F018 File Offset: 0x0004D218
		public unsafe override void WriteValueIntoState(Quaternion value, void* statePtr)
		{
			if (this.m_OptimizedControlDataType == 1364541780)
			{
				*(Quaternion*)((byte*)statePtr + this.m_StateBlock.byteOffset) = value;
				return;
			}
			this.x.WriteValueIntoState(value.x, statePtr);
			this.y.WriteValueIntoState(value.y, statePtr);
			this.z.WriteValueIntoState(value.z, statePtr);
			this.w.WriteValueIntoState(value.w, statePtr);
		}

		// Token: 0x0600104C RID: 4172 RVA: 0x0004F094 File Offset: 0x0004D294
		protected override FourCC CalculateOptimizedControlDataType()
		{
			if (this.m_StateBlock.sizeInBits == 128U && this.m_StateBlock.bitOffset == 0U && this.x.optimizedControlDataType == InputStateBlock.FormatFloat && this.y.optimizedControlDataType == InputStateBlock.FormatFloat && this.z.optimizedControlDataType == InputStateBlock.FormatFloat && this.w.optimizedControlDataType == InputStateBlock.FormatFloat && this.y.m_StateBlock.byteOffset == this.x.m_StateBlock.byteOffset + 4U && this.z.m_StateBlock.byteOffset == this.x.m_StateBlock.byteOffset + 8U && this.w.m_StateBlock.byteOffset == this.x.m_StateBlock.byteOffset + 12U && this.x.m_ProcessorStack.length == 0 && this.y.m_ProcessorStack.length == 0 && this.z.m_ProcessorStack.length == 0)
			{
				return InputStateBlock.FormatQuaternion;
			}
			return InputStateBlock.FormatInvalid;
		}
	}
}
