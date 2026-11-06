using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.InputSystem.Utilities
{
	// Token: 0x0200013C RID: 316
	internal static class NumberHelpers
	{
		// Token: 0x0600118E RID: 4494 RVA: 0x000542A8 File Offset: 0x000524A8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int AlignToMultipleOf(this int number, int alignment)
		{
			int num = number % alignment;
			if (num == 0)
			{
				return number;
			}
			return number + alignment - num;
		}

		// Token: 0x0600118F RID: 4495 RVA: 0x000542C4 File Offset: 0x000524C4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long AlignToMultipleOf(this long number, long alignment)
		{
			long num = number % alignment;
			if (num == 0L)
			{
				return number;
			}
			return number + alignment - num;
		}

		// Token: 0x06001190 RID: 4496 RVA: 0x000542E0 File Offset: 0x000524E0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint AlignToMultipleOf(this uint number, uint alignment)
		{
			uint num = number % alignment;
			if (num == 0U)
			{
				return number;
			}
			return number + alignment - num;
		}

		// Token: 0x06001191 RID: 4497 RVA: 0x000542FB File Offset: 0x000524FB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Approximately(double a, double b)
		{
			return Math.Abs(b - a) < Math.Max(1E-06 * Math.Max(Math.Abs(a), Math.Abs(b)), 4E-323);
		}

		// Token: 0x06001192 RID: 4498 RVA: 0x00054330 File Offset: 0x00052530
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float IntToNormalizedFloat(int value, int minValue, int maxValue)
		{
			if (value <= minValue)
			{
				return 0f;
			}
			if (value >= maxValue)
			{
				return 1f;
			}
			return (float)(((double)value - (double)minValue) / ((double)maxValue - (double)minValue));
		}

		// Token: 0x06001193 RID: 4499 RVA: 0x00054352 File Offset: 0x00052552
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int NormalizedFloatToInt(float value, int intMinValue, int intMaxValue)
		{
			if (value <= 0f)
			{
				return intMinValue;
			}
			if (value >= 1f)
			{
				return intMaxValue;
			}
			return (int)((double)value * ((double)intMaxValue - (double)intMinValue) + (double)intMinValue);
		}

		// Token: 0x06001194 RID: 4500 RVA: 0x00054374 File Offset: 0x00052574
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float UIntToNormalizedFloat(uint value, uint minValue, uint maxValue)
		{
			if (value <= minValue)
			{
				return 0f;
			}
			if (value >= maxValue)
			{
				return 1f;
			}
			return (float)((value - minValue) / (maxValue - minValue));
		}

		// Token: 0x06001195 RID: 4501 RVA: 0x0005439A File Offset: 0x0005259A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint NormalizedFloatToUInt(float value, uint uintMinValue, uint uintMaxValue)
		{
			if (value <= 0f)
			{
				return uintMinValue;
			}
			if (value >= 1f)
			{
				return uintMaxValue;
			}
			return (uint)((double)value * (uintMaxValue - uintMinValue) + uintMinValue);
		}

		// Token: 0x06001196 RID: 4502 RVA: 0x000543C0 File Offset: 0x000525C0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint RemapUIntBitsToNormalizeFloatToUIntBits(uint value, uint inBitSize, uint outBitSize)
		{
			uint num = (uint)((1L << (int)inBitSize) - 1L);
			uint num2 = (uint)((1L << (int)outBitSize) - 1L);
			return NumberHelpers.NormalizedFloatToUInt(NumberHelpers.UIntToNormalizedFloat(value, 0U, num), 0U, num2);
		}
	}
}
