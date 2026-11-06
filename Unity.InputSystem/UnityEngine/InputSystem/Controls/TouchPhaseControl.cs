using System;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace UnityEngine.InputSystem.Controls
{
	// Token: 0x02000120 RID: 288
	[InputControlLayout(hideInUI = true)]
	public class TouchPhaseControl : InputControl<TouchPhase>
	{
		// Token: 0x06001076 RID: 4214 RVA: 0x0004F4EA File Offset: 0x0004D6EA
		public TouchPhaseControl()
		{
			this.m_StateBlock.format = InputStateBlock.FormatInt;
		}

		// Token: 0x06001077 RID: 4215 RVA: 0x0004F504 File Offset: 0x0004D704
		public unsafe override TouchPhase ReadUnprocessedValueFromState(void* statePtr)
		{
			return (TouchPhase)base.stateBlock.ReadInt(statePtr);
		}

		// Token: 0x06001078 RID: 4216 RVA: 0x0004F520 File Offset: 0x0004D720
		public unsafe override void WriteValueIntoState(TouchPhase value, void* statePtr)
		{
			*(int*)((byte*)statePtr + this.m_StateBlock.byteOffset) = (int)value;
		}
	}
}
