using System;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.Controls
{
	// Token: 0x02000121 RID: 289
	[InputControlLayout(hideInUI = true)]
	public class TouchPressControl : ButtonControl
	{
		// Token: 0x06001079 RID: 4217 RVA: 0x0004F534 File Offset: 0x0004D734
		protected override void FinishSetup()
		{
			base.FinishSetup();
			if (!base.stateBlock.format.IsIntegerFormat())
			{
				throw new NotSupportedException(string.Format("Non-integer format '{0}' is not supported for TouchButtonControl '{1}'", base.stateBlock.format, this));
			}
		}

		// Token: 0x0600107A RID: 4218 RVA: 0x0004F580 File Offset: 0x0004D780
		public unsafe override float ReadUnprocessedValueFromState(void* statePtr)
		{
			TouchPhase touchPhase = (TouchPhase)MemoryHelpers.ReadMultipleBitsAsUInt((void*)((byte*)statePtr + this.m_StateBlock.byteOffset), this.m_StateBlock.bitOffset, this.m_StateBlock.sizeInBits);
			float num = 0f;
			if (touchPhase == TouchPhase.Began || touchPhase == TouchPhase.Stationary || touchPhase == TouchPhase.Moved)
			{
				num = 1f;
			}
			return base.Preprocess(num);
		}

		// Token: 0x0600107B RID: 4219 RVA: 0x0004F5D5 File Offset: 0x0004D7D5
		public unsafe override void WriteValueIntoState(float value, void* statePtr)
		{
			throw new NotSupportedException();
		}
	}
}
