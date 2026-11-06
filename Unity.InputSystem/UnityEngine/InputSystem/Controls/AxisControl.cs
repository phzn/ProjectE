using System;
using System.Runtime.CompilerServices;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Processors;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.Controls
{
	// Token: 0x02000115 RID: 277
	public class AxisControl : InputControl<float>
	{
		// Token: 0x06001004 RID: 4100 RVA: 0x0004E268 File Offset: 0x0004C468
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected float Preprocess(float value)
		{
			if (this.scale)
			{
				value *= this.scaleFactor;
			}
			if (this.clamp == AxisControl.Clamp.ToConstantBeforeNormalize)
			{
				if (value < this.clampMin || value > this.clampMax)
				{
					value = this.clampConstant;
				}
			}
			else if (this.clamp == AxisControl.Clamp.BeforeNormalize)
			{
				value = Mathf.Clamp(value, this.clampMin, this.clampMax);
			}
			if (this.normalize)
			{
				value = NormalizeProcessor.Normalize(value, this.normalizeMin, this.normalizeMax, this.normalizeZero);
			}
			if (this.clamp == AxisControl.Clamp.AfterNormalize)
			{
				value = Mathf.Clamp(value, this.clampMin, this.clampMax);
			}
			if (this.invert)
			{
				value *= -1f;
			}
			return value;
		}

		// Token: 0x06001005 RID: 4101 RVA: 0x0004E31C File Offset: 0x0004C51C
		private float Unpreprocess(float value)
		{
			if (this.invert)
			{
				value *= -1f;
			}
			if (this.normalize)
			{
				value = NormalizeProcessor.Denormalize(value, this.normalizeMin, this.normalizeMax, this.normalizeZero);
			}
			if (this.scale)
			{
				value /= this.scaleFactor;
			}
			return value;
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x0004E36F File Offset: 0x0004C56F
		public AxisControl()
		{
			this.m_StateBlock.format = InputStateBlock.FormatFloat;
		}

		// Token: 0x06001007 RID: 4103 RVA: 0x0004E388 File Offset: 0x0004C588
		protected override void FinishSetup()
		{
			base.FinishSetup();
			if (!base.hasDefaultState && this.normalize && Mathf.Abs(this.normalizeZero) > Mathf.Epsilon)
			{
				this.m_DefaultState = base.stateBlock.FloatToPrimitiveValue(this.normalizeZero);
			}
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x0004E3D8 File Offset: 0x0004C5D8
		public unsafe override float ReadUnprocessedValueFromState(void* statePtr)
		{
			int num = this.m_OptimizedControlDataType;
			if (num != 1113150533)
			{
				if (num == 1179407392)
				{
					return *(float*)((byte*)statePtr + this.m_StateBlock.m_ByteOffset);
				}
				float num2 = base.stateBlock.ReadFloat(statePtr);
				return this.Preprocess(num2);
			}
			else
			{
				if (((byte*)statePtr)[this.m_StateBlock.m_ByteOffset] == 0)
				{
					return 0f;
				}
				return 1f;
			}
		}

		// Token: 0x06001009 RID: 4105 RVA: 0x0004E448 File Offset: 0x0004C648
		public unsafe override void WriteValueIntoState(float value, void* statePtr)
		{
			int num = this.m_OptimizedControlDataType;
			if (num == 1113150533)
			{
				((byte*)statePtr)[this.m_StateBlock.m_ByteOffset] = ((value >= 0.5f) ? 1 : 0);
				return;
			}
			if (num == 1179407392)
			{
				*(float*)((byte*)statePtr + this.m_StateBlock.m_ByteOffset) = value;
				return;
			}
			value = this.Unpreprocess(value);
			base.stateBlock.WriteFloat(statePtr, value);
		}

		// Token: 0x0600100A RID: 4106 RVA: 0x0004E4B8 File Offset: 0x0004C6B8
		public unsafe override bool CompareValue(void* firstStatePtr, void* secondStatePtr)
		{
			float num = base.ReadValueFromState(firstStatePtr);
			float num2 = base.ReadValueFromState(secondStatePtr);
			return !Mathf.Approximately(num, num2);
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x0004E4DD File Offset: 0x0004C6DD
		public unsafe override float EvaluateMagnitude(void* statePtr)
		{
			return this.EvaluateMagnitude(base.ReadValueFromStateWithCaching(statePtr));
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x0004E4EC File Offset: 0x0004C6EC
		private float EvaluateMagnitude(float value)
		{
			if (this.m_MinValue.isEmpty || this.m_MaxValue.isEmpty)
			{
				return Mathf.Abs(value);
			}
			float num = this.m_MinValue.ToSingle(null);
			float num2 = this.m_MaxValue.ToSingle(null);
			float num3 = Mathf.Clamp(value, num, num2);
			if (num >= 0f)
			{
				return NormalizeProcessor.Normalize(num3, num, num2, 0f);
			}
			if (num3 < 0f)
			{
				return NormalizeProcessor.Normalize(Mathf.Abs(num3), 0f, Mathf.Abs(num), 0f);
			}
			return NormalizeProcessor.Normalize(num3, 0f, num2, 0f);
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x0004E588 File Offset: 0x0004C788
		protected override FourCC CalculateOptimizedControlDataType()
		{
			bool flag = this.clamp == AxisControl.Clamp.None && !this.invert && !this.normalize && !this.scale;
			if (flag && this.m_StateBlock.format == InputStateBlock.FormatFloat && this.m_StateBlock.sizeInBits == 32U && this.m_StateBlock.bitOffset == 0U)
			{
				return InputStateBlock.FormatFloat;
			}
			if (flag && this.m_StateBlock.format == InputStateBlock.FormatBit && this.m_StateBlock.sizeInBits == 8U && this.m_StateBlock.bitOffset == 0U)
			{
				return InputStateBlock.FormatByte;
			}
			return InputStateBlock.FormatInvalid;
		}

		// Token: 0x040006A6 RID: 1702
		public AxisControl.Clamp clamp;

		// Token: 0x040006A7 RID: 1703
		public float clampMin;

		// Token: 0x040006A8 RID: 1704
		public float clampMax;

		// Token: 0x040006A9 RID: 1705
		public float clampConstant;

		// Token: 0x040006AA RID: 1706
		public bool invert;

		// Token: 0x040006AB RID: 1707
		public bool normalize;

		// Token: 0x040006AC RID: 1708
		public float normalizeMin;

		// Token: 0x040006AD RID: 1709
		public float normalizeMax;

		// Token: 0x040006AE RID: 1710
		public float normalizeZero;

		// Token: 0x040006AF RID: 1711
		public bool scale;

		// Token: 0x040006B0 RID: 1712
		public float scaleFactor;

		// Token: 0x0200023B RID: 571
		public enum Clamp
		{
			// Token: 0x04000C52 RID: 3154
			None,
			// Token: 0x04000C53 RID: 3155
			BeforeNormalize,
			// Token: 0x04000C54 RID: 3156
			AfterNormalize,
			// Token: 0x04000C55 RID: 3157
			ToConstantBeforeNormalize
		}
	}
}
