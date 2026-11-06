using System;
using UnityEngine.InputSystem.LowLevel;

namespace UnityEngine.InputSystem.Controls
{
	// Token: 0x02000119 RID: 281
	public class DoubleControl : InputControl<double>
	{
		// Token: 0x06001026 RID: 4134 RVA: 0x0004EA66 File Offset: 0x0004CC66
		public DoubleControl()
		{
			this.m_StateBlock.format = InputStateBlock.FormatDouble;
		}

		// Token: 0x06001027 RID: 4135 RVA: 0x0004EA7E File Offset: 0x0004CC7E
		public unsafe override double ReadUnprocessedValueFromState(void* statePtr)
		{
			return this.m_StateBlock.ReadDouble(statePtr);
		}

		// Token: 0x06001028 RID: 4136 RVA: 0x0004EA8C File Offset: 0x0004CC8C
		public unsafe override void WriteValueIntoState(double value, void* statePtr)
		{
			this.m_StateBlock.WriteDouble(statePtr, value);
		}
	}
}
