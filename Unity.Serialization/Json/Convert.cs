using System;
using System.Runtime.InteropServices;

namespace Unity.Serialization.Json
{
	// Token: 0x0200006F RID: 111
	internal static class Convert
	{
		// Token: 0x06000292 RID: 658 RVA: 0x0000B430 File Offset: 0x00009630
		public unsafe static Convert.ParseError StrToInt64(char* c, int len, out long output)
		{
			long num = 0L;
			int num2 = 1;
			int num3 = 0;
			if (c[num3] == '+' || c[num3] == '-')
			{
				if (c[num3] == '-')
				{
					num2 = -1;
				}
				num3++;
			}
			while (num3 < len && Convert.IsDigit(c[num3]))
			{
				if (num > 1844674407370955161L)
				{
					output = 0L;
					return Convert.ParseError.Overflow;
				}
				num *= 10L;
				num += (long)(c[num3] - '0');
				num3++;
			}
			if (num2 == -1)
			{
				num = -num;
				if (num > 0L)
				{
					output = 0L;
					return Convert.ParseError.Overflow;
				}
			}
			else if (num < 0L)
			{
				output = 0L;
				return Convert.ParseError.Overflow;
			}
			output = num;
			return Convert.ParseError.None;
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000B4CC File Offset: 0x000096CC
		public unsafe static Convert.ParseError StrToUInt64(char* c, int len, out ulong output)
		{
			ulong num = 0UL;
			int num2 = 0;
			if (c[num2] == '+' || c[num2] == '-')
			{
				if (c[num2] == '-')
				{
					output = 0UL;
					return Convert.ParseError.Underflow;
				}
				num2++;
			}
			while (num2 < len && Convert.IsDigit(c[num2]))
			{
				if (num > 1844674407370955161UL)
				{
					output = 0UL;
					return Convert.ParseError.Overflow;
				}
				num *= 10UL;
				ulong num3 = num + (ulong)((long)(c[num2] - '0'));
				if (num3 < num)
				{
					output = 0UL;
					return Convert.ParseError.Overflow;
				}
				num = num3;
				num2++;
			}
			output = num;
			return Convert.ParseError.None;
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000B558 File Offset: 0x00009758
		public unsafe static Convert.ParseError StrToFloat32(char* c, int len, out float output)
		{
			if (Convert.MatchesNaN(c, len))
			{
				output = float.NaN;
				return Convert.ParseError.None;
			}
			output = 0f;
			float num = 0f;
			int num2 = 1;
			int num3 = 0;
			if (c[num3] == '+' || c[num3] == '-')
			{
				if (c[num3] == '-')
				{
					num2 = -1;
				}
				num3++;
			}
			if (Convert.MatchesInfinity(c + num3, len - num3))
			{
				output = ((num2 == -1) ? float.NegativeInfinity : float.PositiveInfinity);
				return Convert.ParseError.None;
			}
			ulong num4 = 0UL;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			while (num3 < len && Convert.IsDigit(c[num3]))
			{
				num7++;
				if (num5 < 9)
				{
					ulong num8 = num4 * 10UL + (ulong)((long)(c[num3] - '0'));
					if (num8 > num4)
					{
						num5++;
					}
					num4 = num8;
				}
				else
				{
					num6--;
				}
				num3++;
			}
			if (num3 < len && c[num3] == '.')
			{
				num3++;
				while (num3 < len && Convert.IsDigit(c[num3]))
				{
					num7++;
					if (num5 < 9)
					{
						ulong num9 = num4 * 10UL + (ulong)((long)(c[num3] - '0'));
						if (num9 > num4)
						{
							num5++;
						}
						num4 = num9;
						num6++;
					}
					num3++;
				}
			}
			if (num7 == 0)
			{
				return Convert.ParseError.Syntax;
			}
			int num10 = 0;
			int num11 = 1;
			if (num3 < len && (c[num3] | ' ') == 'e')
			{
				num3++;
				if (num3 < len)
				{
					if (c[num3] == '+')
					{
						num3++;
					}
					else if (c[num3] == '-')
					{
						num11 = -1;
						num3++;
					}
				}
				int num12 = 0;
				while (num3 < len && Convert.IsDigit(c[num3]))
				{
					num12++;
					num10 = num10 * 10 + (int)(c[num3] - '0');
					if (num10 > 38)
					{
						if (num11 == 1)
						{
							return Convert.ParseError.Overflow;
						}
						return Convert.ParseError.Underflow;
					}
					else
					{
						num3++;
					}
				}
				if (num12 == 0)
				{
					return Convert.ParseError.Syntax;
				}
			}
			num10 = num10 * num11 - num6;
			Convert.ParseError parseError = Convert.Base10ToBase2(ref num, num4, num10);
			if (parseError != Convert.ParseError.None)
			{
				return parseError;
			}
			output = num * (float)num2;
			return Convert.ParseError.None;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000B738 File Offset: 0x00009938
		private static Convert.ParseError Base10ToBase2(ref float output, ulong mantissa10, int exponent10)
		{
			if (mantissa10 == 0UL)
			{
				output = 0f;
				return Convert.ParseError.None;
			}
			if (exponent10 == 0)
			{
				output = mantissa10;
				return Convert.ParseError.None;
			}
			int num = exponent10;
			ulong num2 = mantissa10;
			while (exponent10 > 0)
			{
				while ((num2 & 16140901064495857664UL) != 0UL)
				{
					num2 >>= 1;
					num++;
				}
				num2 *= 5UL;
				exponent10--;
			}
			while (exponent10 < 0)
			{
				while ((num2 & 9223372036854775808UL) == 0UL)
				{
					num2 <<= 1;
					num--;
				}
				num2 /= 5UL;
				exponent10++;
			}
			Convert.UintFloatUnion uintFloatUnion = new Convert.UintFloatUnion
			{
				floatValue = num2
			};
			int num3 = (int)(((uintFloatUnion.uintValue >> 23) & 255U) - 127U);
			num3 += num;
			if (num3 > 128)
			{
				return Convert.ParseError.Overflow;
			}
			if (num3 < -127)
			{
				return Convert.ParseError.Underflow;
			}
			uintFloatUnion.uintValue = (uintFloatUnion.uintValue & 2155872255U) | (uint)((uint)(num3 + 127) << 23);
			output = uintFloatUnion.floatValue;
			return Convert.ParseError.None;
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000B807 File Offset: 0x00009A07
		public unsafe static bool IsSigned(char* c, int len)
		{
			return len != 0 && *c == '-';
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000B814 File Offset: 0x00009A14
		public unsafe static bool IsIntegral(char* c, int len)
		{
			if (len == 0)
			{
				return false;
			}
			int i = 0;
			if (c[i] == '-' || c[i] == '+')
			{
				i++;
			}
			while (i < len)
			{
				if (!Convert.IsDigit(c[i]))
				{
					return false;
				}
				i++;
			}
			return true;
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000B860 File Offset: 0x00009A60
		public unsafe static bool IsDecimal(char* c, int len)
		{
			if (len == 0)
			{
				return false;
			}
			int i = 0;
			if (c[i] == '-' || c[i] == '+')
			{
				i++;
			}
			bool flag = false;
			while (i < len)
			{
				if (c[i] == '.' || (c[i] | ' ') == 'e')
				{
					flag = true;
				}
				else if (c[i] != '-' && c[i] != '+' && !Convert.IsDigit(c[i]))
				{
					return false;
				}
				i++;
			}
			return flag;
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000B8DE File Offset: 0x00009ADE
		private static bool IsDigit(char c)
		{
			return c >= '0' && c <= '9';
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000B8F0 File Offset: 0x00009AF0
		internal unsafe static bool MatchesNaN(char* c, int len)
		{
			IntPtr intPtr = stackalloc byte[(UIntPtr)6];
			*intPtr = 110;
			*(intPtr + 2) = 97;
			*(intPtr + (IntPtr)2 * 2) = 110;
			char* ptr = intPtr;
			return Convert.Matches(c, len, ptr, 3);
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000B920 File Offset: 0x00009B20
		internal unsafe static bool MatchesNull(char* c, int len)
		{
			IntPtr intPtr = stackalloc byte[(UIntPtr)8];
			*intPtr = 110;
			*(intPtr + 2) = 117;
			*(intPtr + (IntPtr)2 * 2) = 108;
			*(intPtr + (IntPtr)3 * 2) = 108;
			char* ptr = intPtr;
			return Convert.Matches(c, len, ptr, 4);
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000B958 File Offset: 0x00009B58
		internal unsafe static bool MatchesInfinity(char* c, int len)
		{
			IntPtr intPtr = stackalloc byte[(UIntPtr)16];
			*intPtr = 105;
			*(intPtr + 2) = 110;
			*(intPtr + (IntPtr)2 * 2) = 102;
			*(intPtr + (IntPtr)3 * 2) = 105;
			*(intPtr + (IntPtr)4 * 2) = 110;
			*(intPtr + (IntPtr)5 * 2) = 105;
			*(intPtr + (IntPtr)6 * 2) = 116;
			*(intPtr + (IntPtr)7 * 2) = 121;
			char* ptr = intPtr;
			return Convert.Matches(c, len, ptr, 8);
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000B9B4 File Offset: 0x00009BB4
		internal unsafe static bool MatchesTrue(char* c, int len)
		{
			IntPtr intPtr = stackalloc byte[(UIntPtr)8];
			*intPtr = 116;
			*(intPtr + 2) = 114;
			*(intPtr + (IntPtr)2 * 2) = 117;
			*(intPtr + (IntPtr)3 * 2) = 101;
			char* ptr = intPtr;
			return Convert.Matches(c, len, ptr, 4);
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000B9EC File Offset: 0x00009BEC
		internal unsafe static bool MatchesFalse(char* c, int len)
		{
			IntPtr intPtr = stackalloc byte[(UIntPtr)10];
			*intPtr = 102;
			*(intPtr + 2) = 97;
			*(intPtr + (IntPtr)2 * 2) = 108;
			*(intPtr + (IntPtr)3 * 2) = 115;
			*(intPtr + (IntPtr)4 * 2) = 101;
			char* ptr = intPtr;
			return Convert.Matches(c, len, ptr, 5);
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000BA30 File Offset: 0x00009C30
		internal unsafe static bool Matches(char* inputAnyCase, int inputLen, char* expectedLowerCase, int expectedLen)
		{
			if (inputLen != expectedLen)
			{
				return false;
			}
			for (int i = 0; i < inputLen; i++)
			{
				if ((inputAnyCase[i] | ' ') != expectedLowerCase[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04000151 RID: 337
		private const ulong k_ULongMaxDiv10 = 1844674407370955161UL;

		// Token: 0x02000070 RID: 112
		public enum ParseError
		{
			// Token: 0x04000153 RID: 339
			None,
			// Token: 0x04000154 RID: 340
			Syntax,
			// Token: 0x04000155 RID: 341
			Overflow,
			// Token: 0x04000156 RID: 342
			Underflow
		}

		// Token: 0x02000071 RID: 113
		[StructLayout(LayoutKind.Explicit)]
		private struct UintFloatUnion
		{
			// Token: 0x04000157 RID: 343
			[FieldOffset(0)]
			public uint uintValue;

			// Token: 0x04000158 RID: 344
			[FieldOffset(0)]
			public float floatValue;
		}
	}
}
