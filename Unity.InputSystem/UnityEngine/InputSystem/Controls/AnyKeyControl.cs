using System;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace UnityEngine.InputSystem.Controls
{
	// Token: 0x02000114 RID: 276
	[InputControlLayout(hideInUI = true)]
	public class AnyKeyControl : ButtonControl
	{
		// Token: 0x06001002 RID: 4098 RVA: 0x0004E22B File Offset: 0x0004C42B
		public AnyKeyControl()
		{
			this.m_StateBlock.sizeInBits = 1U;
			this.m_StateBlock.format = InputStateBlock.FormatBit;
		}

		// Token: 0x06001003 RID: 4099 RVA: 0x0004E24F File Offset: 0x0004C44F
		public unsafe override float ReadUnprocessedValueFromState(void* statePtr)
		{
			if (!this.CheckStateIsAtDefault(statePtr, null))
			{
				return 1f;
			}
			return 0f;
		}
	}
}
