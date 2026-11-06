using System;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.Controls
{
	// Token: 0x0200011B RID: 283
	public class IntegerControl : InputControl<int>
	{
		// Token: 0x06001037 RID: 4151 RVA: 0x0004ED7F File Offset: 0x0004CF7F
		public IntegerControl()
		{
			this.m_StateBlock.format = InputStateBlock.FormatInt;
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x0004ED97 File Offset: 0x0004CF97
		public unsafe override int ReadUnprocessedValueFromState(void* statePtr)
		{
			if (this.m_OptimizedControlDataType == 1229870112)
			{
				return *(int*)((byte*)statePtr + this.m_StateBlock.byteOffset);
			}
			return this.m_StateBlock.ReadInt(statePtr);
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x0004EDC6 File Offset: 0x0004CFC6
		public unsafe override void WriteValueIntoState(int value, void* statePtr)
		{
			if (this.m_OptimizedControlDataType == 1229870112)
			{
				*(int*)((byte*)statePtr + this.m_StateBlock.byteOffset) = value;
				return;
			}
			this.m_StateBlock.WriteInt(statePtr, value);
		}

		// Token: 0x0600103A RID: 4154 RVA: 0x0004EDF7 File Offset: 0x0004CFF7
		protected override FourCC CalculateOptimizedControlDataType()
		{
			if (this.m_StateBlock.format == InputStateBlock.FormatInt && this.m_StateBlock.sizeInBits == 32U && this.m_StateBlock.bitOffset == 0U)
			{
				return InputStateBlock.FormatInt;
			}
			return InputStateBlock.FormatInvalid;
		}
	}
}
