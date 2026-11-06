using System;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.Controls
{
	// Token: 0x02000118 RID: 280
	public class DiscreteButtonControl : ButtonControl
	{
		// Token: 0x06001022 RID: 4130 RVA: 0x0004E8F8 File Offset: 0x0004CAF8
		protected override void FinishSetup()
		{
			base.FinishSetup();
			if (!base.stateBlock.format.IsIntegerFormat())
			{
				throw new NotSupportedException(string.Format("Non-integer format '{0}' is not supported for DiscreteButtonControl '{1}'", base.stateBlock.format, this));
			}
		}

		// Token: 0x06001023 RID: 4131 RVA: 0x0004E944 File Offset: 0x0004CB44
		public unsafe override float ReadUnprocessedValueFromState(void* statePtr)
		{
			int num = MemoryHelpers.ReadTwosComplementMultipleBitsAsInt((void*)((byte*)statePtr + this.m_StateBlock.byteOffset), this.m_StateBlock.bitOffset, this.m_StateBlock.sizeInBits);
			float num2 = 0f;
			if (this.minValue > this.maxValue)
			{
				if (this.wrapAtValue == this.nullValue)
				{
					this.wrapAtValue = this.minValue;
				}
				if ((num >= this.minValue && num <= this.wrapAtValue) || (num != this.nullValue && num <= this.maxValue))
				{
					num2 = 1f;
				}
			}
			else
			{
				num2 = ((num >= this.minValue && num <= this.maxValue) ? 1f : 0f);
			}
			return base.Preprocess(num2);
		}

		// Token: 0x06001024 RID: 4132 RVA: 0x0004E9FC File Offset: 0x0004CBFC
		public unsafe override void WriteValueIntoState(float value, void* statePtr)
		{
			if (this.writeMode == DiscreteButtonControl.WriteMode.WriteNullAndMaxValue)
			{
				void* ptr = (void*)((byte*)statePtr + this.m_StateBlock.byteOffset);
				int num = ((value >= base.pressPointOrDefault) ? this.maxValue : this.nullValue);
				MemoryHelpers.WriteIntAsTwosComplementMultipleBits(ptr, this.m_StateBlock.bitOffset, this.m_StateBlock.sizeInBits, num);
				return;
			}
			throw new NotSupportedException("Writing value states for DiscreteButtonControl is not supported as a single value may correspond to multiple states");
		}

		// Token: 0x040006BE RID: 1726
		public int minValue;

		// Token: 0x040006BF RID: 1727
		public int maxValue;

		// Token: 0x040006C0 RID: 1728
		public int wrapAtValue;

		// Token: 0x040006C1 RID: 1729
		public int nullValue;

		// Token: 0x040006C2 RID: 1730
		public DiscreteButtonControl.WriteMode writeMode;

		// Token: 0x0200023C RID: 572
		public enum WriteMode
		{
			// Token: 0x04000C57 RID: 3159
			WriteDisabled,
			// Token: 0x04000C58 RID: 3160
			WriteNullAndMaxValue
		}
	}
}
