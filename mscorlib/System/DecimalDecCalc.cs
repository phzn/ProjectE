using System;

namespace System
{
	// Token: 0x020001C3 RID: 451
	internal static class DecimalDecCalc
	{
		// Token: 0x06001342 RID: 4930 RVA: 0x0004D990 File Offset: 0x0004BB90
		private static uint D32DivMod1E9(uint hi32, ref uint lo32)
		{
			ulong num = ((ulong)hi32 << 32) | (ulong)lo32;
			lo32 = (uint)(num / 1000000000UL);
			return (uint)(num % 1000000000UL);
		}

		// Token: 0x06001343 RID: 4931 RVA: 0x0004D9BB File Offset: 0x0004BBBB
		internal static uint DecDivMod1E9(ref MutableDecimal value)
		{
			return DecimalDecCalc.D32DivMod1E9(DecimalDecCalc.D32DivMod1E9(DecimalDecCalc.D32DivMod1E9(0U, ref value.High), ref value.Mid), ref value.Low);
		}

		// Token: 0x06001344 RID: 4932 RVA: 0x0004D9DF File Offset: 0x0004BBDF
		internal static void DecAddInt32(ref MutableDecimal value, uint i)
		{
			if (DecimalDecCalc.D32AddCarry(ref value.Low, i) && DecimalDecCalc.D32AddCarry(ref value.Mid, 1U))
			{
				DecimalDecCalc.D32AddCarry(ref value.High, 1U);
			}
		}

		// Token: 0x06001345 RID: 4933 RVA: 0x0004DA0C File Offset: 0x0004BC0C
		private static bool D32AddCarry(ref uint value, uint i)
		{
			uint num = value;
			uint num2 = num + i;
			value = num2;
			return num2 < num || num2 < i;
		}

		// Token: 0x06001346 RID: 4934 RVA: 0x0004DA30 File Offset: 0x0004BC30
		internal static void DecMul10(ref MutableDecimal value)
		{
			MutableDecimal mutableDecimal = value;
			DecimalDecCalc.DecShiftLeft(ref value);
			DecimalDecCalc.DecShiftLeft(ref value);
			DecimalDecCalc.DecAdd(ref value, mutableDecimal);
			DecimalDecCalc.DecShiftLeft(ref value);
		}

		// Token: 0x06001347 RID: 4935 RVA: 0x0004DA60 File Offset: 0x0004BC60
		private static void DecShiftLeft(ref MutableDecimal value)
		{
			uint num = (((value.Low & 2147483648U) != 0U) ? 1U : 0U);
			uint num2 = (((value.Mid & 2147483648U) != 0U) ? 1U : 0U);
			value.Low <<= 1;
			value.Mid = (value.Mid << 1) | num;
			value.High = (value.High << 1) | num2;
		}

		// Token: 0x06001348 RID: 4936 RVA: 0x0004DAC4 File Offset: 0x0004BCC4
		private static void DecAdd(ref MutableDecimal value, MutableDecimal d)
		{
			if (DecimalDecCalc.D32AddCarry(ref value.Low, d.Low) && DecimalDecCalc.D32AddCarry(ref value.Mid, 1U))
			{
				DecimalDecCalc.D32AddCarry(ref value.High, 1U);
			}
			if (DecimalDecCalc.D32AddCarry(ref value.Mid, d.Mid))
			{
				DecimalDecCalc.D32AddCarry(ref value.High, 1U);
			}
			DecimalDecCalc.D32AddCarry(ref value.High, d.High);
		}
	}
}
