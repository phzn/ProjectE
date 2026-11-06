using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Burst
{
	// Token: 0x0200001E RID: 30
	internal static class BurstString
	{
		// Token: 0x06000096 RID: 150 RVA: 0x000032B8 File Offset: 0x000014B8
		[BurstString.PreserveAttribute]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void CopyFixedString(byte* dest, int destLength, byte* src, int srcLength)
		{
			int num = ((srcLength > destLength) ? destLength : srcLength);
			*(short*)(dest - 2) = (short)((ushort)num);
			dest[num] = 0;
			UnsafeUtility.MemCpy((void*)dest, (void*)src, (long)num);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x000032E4 File Offset: 0x000014E4
		[BurstString.PreserveAttribute]
		public unsafe static void Format(byte* dest, ref int destIndex, int destLength, byte* src, int srcLength, int formatOptionsRaw)
		{
			BurstString.FormatOptions formatOptions = *(BurstString.FormatOptions*)(&formatOptionsRaw);
			if (BurstString.AlignLeft(dest, ref destIndex, destLength, (int)formatOptions.AlignAndSize, srcLength))
			{
				return;
			}
			int num = destLength - destIndex;
			int num2 = ((srcLength > num) ? num : srcLength);
			if (num2 > 0)
			{
				UnsafeUtility.MemCpy((void*)(dest + destIndex), (void*)src, (long)num2);
				destIndex += num2;
				BurstString.AlignRight(dest, ref destIndex, destLength, (int)formatOptions.AlignAndSize, srcLength);
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00003344 File Offset: 0x00001544
		[BurstString.PreserveAttribute]
		public unsafe static void Format(byte* dest, ref int destIndex, int destLength, float value, int formatOptionsRaw)
		{
			BurstString.FormatOptions formatOptions = *(BurstString.FormatOptions*)(&formatOptionsRaw);
			BurstString.ConvertFloatToString(dest, ref destIndex, destLength, value, formatOptions);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00003364 File Offset: 0x00001564
		[BurstString.PreserveAttribute]
		public unsafe static void Format(byte* dest, ref int destIndex, int destLength, double value, int formatOptionsRaw)
		{
			BurstString.FormatOptions formatOptions = *(BurstString.FormatOptions*)(&formatOptionsRaw);
			BurstString.ConvertDoubleToString(dest, ref destIndex, destLength, value, formatOptions);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00003384 File Offset: 0x00001584
		[BurstString.PreserveAttribute]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public unsafe static void Format(byte* dest, ref int destIndex, int destLength, bool value, int formatOptionsRaw)
		{
			int num = (value ? 4 : 5);
			BurstString.FormatOptions formatOptions = *(BurstString.FormatOptions*)(&formatOptionsRaw);
			if (BurstString.AlignLeft(dest, ref destIndex, destLength, (int)formatOptions.AlignAndSize, num))
			{
				return;
			}
			if (value)
			{
				if (destIndex >= destLength)
				{
					return;
				}
				int num2 = destIndex;
				destIndex = num2 + 1;
				dest[num2] = 84;
				if (destIndex >= destLength)
				{
					return;
				}
				num2 = destIndex;
				destIndex = num2 + 1;
				dest[num2] = 114;
				if (destIndex >= destLength)
				{
					return;
				}
				num2 = destIndex;
				destIndex = num2 + 1;
				dest[num2] = 117;
				if (destIndex >= destLength)
				{
					return;
				}
				num2 = destIndex;
				destIndex = num2 + 1;
				dest[num2] = 101;
			}
			else
			{
				if (destIndex >= destLength)
				{
					return;
				}
				int num2 = destIndex;
				destIndex = num2 + 1;
				dest[num2] = 70;
				if (destIndex >= destLength)
				{
					return;
				}
				num2 = destIndex;
				destIndex = num2 + 1;
				dest[num2] = 97;
				if (destIndex >= destLength)
				{
					return;
				}
				num2 = destIndex;
				destIndex = num2 + 1;
				dest[num2] = 108;
				if (destIndex >= destLength)
				{
					return;
				}
				num2 = destIndex;
				destIndex = num2 + 1;
				dest[num2] = 115;
				if (destIndex >= destLength)
				{
					return;
				}
				num2 = destIndex;
				destIndex = num2 + 1;
				dest[num2] = 101;
			}
			BurstString.AlignRight(dest, ref destIndex, destLength, (int)formatOptions.AlignAndSize, num);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00003480 File Offset: 0x00001680
		[BurstString.PreserveAttribute]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public unsafe static void Format(byte* dest, ref int destIndex, int destLength, char value, int formatOptionsRaw)
		{
			int num = ((value <= '\u007f') ? 1 : ((value <= '߿') ? 2 : 3));
			BurstString.FormatOptions formatOptions = *(BurstString.FormatOptions*)(&formatOptionsRaw);
			if (BurstString.AlignLeft(dest, ref destIndex, destLength, (int)formatOptions.AlignAndSize, 1))
			{
				return;
			}
			if (num == 1)
			{
				if (destIndex >= destLength)
				{
					return;
				}
				int num2 = destIndex;
				destIndex = num2 + 1;
				dest[num2] = (byte)value;
			}
			else if (num == 2)
			{
				if (destIndex >= destLength)
				{
					return;
				}
				int num2 = destIndex;
				destIndex = num2 + 1;
				dest[num2] = (byte)((value >> 6) | 'À');
				if (destIndex >= destLength)
				{
					return;
				}
				num2 = destIndex;
				destIndex = num2 + 1;
				dest[num2] = (byte)((value & '?') | '\u0080');
			}
			else if (num == 3)
			{
				if (value >= '\ud800' && value <= '\udfff')
				{
					if (destIndex >= destLength)
					{
						return;
					}
					int num2 = destIndex;
					destIndex = num2 + 1;
					dest[num2] = 239;
					if (destIndex >= destLength)
					{
						return;
					}
					num2 = destIndex;
					destIndex = num2 + 1;
					dest[num2] = 191;
					if (destIndex >= destLength)
					{
						return;
					}
					num2 = destIndex;
					destIndex = num2 + 1;
					dest[num2] = 189;
				}
				else
				{
					if (destIndex >= destLength)
					{
						return;
					}
					int num2 = destIndex;
					destIndex = num2 + 1;
					dest[num2] = (byte)((value >> 12) | 'à');
					if (destIndex >= destLength)
					{
						return;
					}
					num2 = destIndex;
					destIndex = num2 + 1;
					dest[num2] = (byte)(((value >> 6) & '?') | '\u0080');
					if (destIndex >= destLength)
					{
						return;
					}
					num2 = destIndex;
					destIndex = num2 + 1;
					dest[num2] = (byte)((value & '?') | '\u0080');
				}
			}
			BurstString.AlignRight(dest, ref destIndex, destLength, (int)formatOptions.AlignAndSize, 1);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x000035EB File Offset: 0x000017EB
		[BurstString.PreserveAttribute]
		public unsafe static void Format(byte* dest, ref int destIndex, int destLength, byte value, int formatOptionsRaw)
		{
			BurstString.Format(dest, ref destIndex, destLength, (ulong)value, formatOptionsRaw);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000035EB File Offset: 0x000017EB
		[BurstString.PreserveAttribute]
		public unsafe static void Format(byte* dest, ref int destIndex, int destLength, ushort value, int formatOptionsRaw)
		{
			BurstString.Format(dest, ref destIndex, destLength, (ulong)value, formatOptionsRaw);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000035FC File Offset: 0x000017FC
		[BurstString.PreserveAttribute]
		public unsafe static void Format(byte* dest, ref int destIndex, int destLength, uint value, int formatOptionsRaw)
		{
			BurstString.FormatOptions formatOptions = *(BurstString.FormatOptions*)(&formatOptionsRaw);
			BurstString.ConvertUnsignedIntegerToString(dest, ref destIndex, destLength, (ulong)value, formatOptions);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00003620 File Offset: 0x00001820
		[BurstString.PreserveAttribute]
		public unsafe static void Format(byte* dest, ref int destIndex, int destLength, ulong value, int formatOptionsRaw)
		{
			BurstString.FormatOptions formatOptions = *(BurstString.FormatOptions*)(&formatOptionsRaw);
			BurstString.ConvertUnsignedIntegerToString(dest, ref destIndex, destLength, value, formatOptions);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00003640 File Offset: 0x00001840
		[BurstString.PreserveAttribute]
		public unsafe static void Format(byte* dest, ref int destIndex, int destLength, sbyte value, int formatOptionsRaw)
		{
			BurstString.FormatOptions formatOptions = *(BurstString.FormatOptions*)(&formatOptionsRaw);
			if (formatOptions.Kind == BurstString.NumberFormatKind.Hexadecimal)
			{
				BurstString.ConvertUnsignedIntegerToString(dest, ref destIndex, destLength, (ulong)((byte)value), formatOptions);
				return;
			}
			BurstString.ConvertIntegerToString(dest, ref destIndex, destLength, (long)value, formatOptions);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00003678 File Offset: 0x00001878
		[BurstString.PreserveAttribute]
		public unsafe static void Format(byte* dest, ref int destIndex, int destLength, short value, int formatOptionsRaw)
		{
			BurstString.FormatOptions formatOptions = *(BurstString.FormatOptions*)(&formatOptionsRaw);
			if (formatOptions.Kind == BurstString.NumberFormatKind.Hexadecimal)
			{
				BurstString.ConvertUnsignedIntegerToString(dest, ref destIndex, destLength, (ulong)((ushort)value), formatOptions);
				return;
			}
			BurstString.ConvertIntegerToString(dest, ref destIndex, destLength, (long)value, formatOptions);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000036B0 File Offset: 0x000018B0
		[BurstString.PreserveAttribute]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public unsafe static void Format(byte* dest, ref int destIndex, int destLength, int value, int formatOptionsRaw)
		{
			BurstString.FormatOptions formatOptions = *(BurstString.FormatOptions*)(&formatOptionsRaw);
			if (formatOptions.Kind == BurstString.NumberFormatKind.Hexadecimal)
			{
				BurstString.ConvertUnsignedIntegerToString(dest, ref destIndex, destLength, (ulong)value, formatOptions);
				return;
			}
			BurstString.ConvertIntegerToString(dest, ref destIndex, destLength, (long)value, formatOptions);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000036E8 File Offset: 0x000018E8
		[BurstString.PreserveAttribute]
		public unsafe static void Format(byte* dest, ref int destIndex, int destLength, long value, int formatOptionsRaw)
		{
			BurstString.FormatOptions formatOptions = *(BurstString.FormatOptions*)(&formatOptionsRaw);
			if (formatOptions.Kind == BurstString.NumberFormatKind.Hexadecimal)
			{
				BurstString.ConvertUnsignedIntegerToString(dest, ref destIndex, destLength, (ulong)value, formatOptions);
				return;
			}
			BurstString.ConvertIntegerToString(dest, ref destIndex, destLength, value, formatOptions);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x0000371C File Offset: 0x0000191C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private unsafe static void ConvertUnsignedIntegerToString(byte* dest, ref int destIndex, int destLength, ulong value, BurstString.FormatOptions options)
		{
			uint @base = (uint)options.GetBase();
			if (@base < 2U || @base > 36U)
			{
				return;
			}
			int num = 0;
			ulong num2 = value;
			do
			{
				num2 /= (ulong)@base;
				num++;
			}
			while (num2 != 0UL);
			int num3 = num - 1;
			byte* ptr = stackalloc byte[(UIntPtr)(num + 1)];
			num2 = value;
			do
			{
				ptr[num3--] = BurstString.ValueToIntegerChar((int)(num2 % (ulong)@base), options.Uppercase);
				num2 /= (ulong)@base;
			}
			while (num2 != 0UL);
			ptr[num] = 0;
			BurstString.NumberBuffer numberBuffer = new BurstString.NumberBuffer(BurstString.NumberBufferKind.Integer, ptr, num, num, false);
			BurstString.FormatNumber(dest, ref destIndex, destLength, ref numberBuffer, (int)options.Specifier, options);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000037A4 File Offset: 0x000019A4
		private static int GetLengthIntegerToString(long value, int basis, int zeroPadding)
		{
			int num = 0;
			long num2 = value;
			do
			{
				num2 /= (long)basis;
				num++;
			}
			while (num2 != 0L);
			if (num < zeroPadding)
			{
				num = zeroPadding;
			}
			if (value < 0L)
			{
				num++;
			}
			return num;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000037D4 File Offset: 0x000019D4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private unsafe static void ConvertIntegerToString(byte* dest, ref int destIndex, int destLength, long value, BurstString.FormatOptions options)
		{
			int @base = options.GetBase();
			if (@base < 2 || @base > 36)
			{
				return;
			}
			int num = 0;
			long num2 = value;
			do
			{
				num2 /= (long)@base;
				num++;
			}
			while (num2 != 0L);
			byte* ptr = stackalloc byte[(UIntPtr)(num + 1)];
			num2 = value;
			int num3 = num - 1;
			do
			{
				ptr[num3--] = BurstString.ValueToIntegerChar((int)(num2 % (long)@base), options.Uppercase);
				num2 /= (long)@base;
			}
			while (num2 != 0L);
			ptr[num] = 0;
			BurstString.NumberBuffer numberBuffer = new BurstString.NumberBuffer(BurstString.NumberBufferKind.Integer, ptr, num, num, value < 0L);
			BurstString.FormatNumber(dest, ref destIndex, destLength, ref numberBuffer, (int)options.Specifier, options);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x0000385C File Offset: 0x00001A5C
		private unsafe static void FormatNumber(byte* dest, ref int destIndex, int destLength, ref BurstString.NumberBuffer number, int nMaxDigits, BurstString.FormatOptions options)
		{
			bool flag = number.Kind == BurstString.NumberBufferKind.Float;
			if (number.Kind == BurstString.NumberBufferKind.Integer && options.Kind == BurstString.NumberFormatKind.General && options.Specifier == 0)
			{
				options.Kind = BurstString.NumberFormatKind.Decimal;
			}
			BurstString.NumberFormatKind kind = options.Kind;
			if (kind != BurstString.NumberFormatKind.General && kind - BurstString.NumberFormatKind.Decimal <= 2)
			{
				int num = number.DigitsCount;
				int specifier = (int)options.Specifier;
				int num2 = 0;
				if (num < specifier)
				{
					num2 = specifier - num;
					num = specifier;
				}
				bool flag2 = options.Kind == BurstString.NumberFormatKind.DecimalForceSigned;
				num += ((number.IsNegative || flag2) ? 1 : 0);
				if (BurstString.AlignLeft(dest, ref destIndex, destLength, (int)options.AlignAndSize, num))
				{
					return;
				}
				BurstString.FormatDecimalOrHexadecimal(dest, ref destIndex, destLength, ref number, num2, flag2);
				BurstString.AlignRight(dest, ref destIndex, destLength, (int)options.AlignAndSize, num);
				return;
			}
			else
			{
				if (nMaxDigits < 1)
				{
					nMaxDigits = number.DigitsCount;
				}
				BurstString.RoundNumber(ref number, nMaxDigits, flag);
				int num = BurstString.GetLengthForFormatGeneral(ref number, nMaxDigits);
				if (BurstString.AlignLeft(dest, ref destIndex, destLength, (int)options.AlignAndSize, num))
				{
					return;
				}
				BurstString.FormatGeneral(dest, ref destIndex, destLength, ref number, nMaxDigits, options.Uppercase ? 69 : 101);
				BurstString.AlignRight(dest, ref destIndex, destLength, (int)options.AlignAndSize, num);
				return;
			}
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00003970 File Offset: 0x00001B70
		private unsafe static void FormatDecimalOrHexadecimal(byte* dest, ref int destIndex, int destLength, ref BurstString.NumberBuffer number, int zeroPadding, bool outputPositiveSign)
		{
			if (number.IsNegative)
			{
				if (destIndex >= destLength)
				{
					return;
				}
				int num = destIndex;
				destIndex = num + 1;
				dest[num] = 45;
			}
			else if (outputPositiveSign)
			{
				if (destIndex >= destLength)
				{
					return;
				}
				int num = destIndex;
				destIndex = num + 1;
				dest[num] = 43;
			}
			for (int i = 0; i < zeroPadding; i++)
			{
				if (destIndex >= destLength)
				{
					return;
				}
				int num = destIndex;
				destIndex = num + 1;
				dest[num] = 48;
			}
			int digitsCount = number.DigitsCount;
			byte* digitsPointer = number.GetDigitsPointer();
			for (int j = 0; j < digitsCount; j++)
			{
				if (destIndex >= destLength)
				{
					return;
				}
				int num = destIndex;
				destIndex = num + 1;
				dest[num] = digitsPointer[j];
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00003A09 File Offset: 0x00001C09
		private static byte ValueToIntegerChar(int value, bool uppercase)
		{
			value = ((value < 0) ? (-value) : value);
			if (value <= 9)
			{
				return (byte)(48 + value);
			}
			if (value < 36)
			{
				return (byte)((uppercase ? 65 : 97) + (value - 10));
			}
			return 63;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00003A38 File Offset: 0x00001C38
		private static void OptsSplit(string fullFormat, out string padding, out string format)
		{
			string[] array = fullFormat.Split(BurstString.SplitByColon, StringSplitOptions.RemoveEmptyEntries);
			format = array[0];
			padding = null;
			if (array.Length == 2)
			{
				padding = format;
				format = array[1];
				return;
			}
			if (array.Length != 1)
			{
				throw new ArgumentException(string.Format("Format `{0}` not supported. Invalid number {1} of :. Expecting no more than one.", format, array.Length));
			}
			if (format[0] == ',')
			{
				padding = format;
				format = null;
				return;
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00003AA0 File Offset: 0x00001CA0
		public static BurstString.FormatOptions ParseFormatToFormatOptions(string fullFormat)
		{
			if (string.IsNullOrWhiteSpace(fullFormat))
			{
				return default(BurstString.FormatOptions);
			}
			string text;
			string text2;
			BurstString.OptsSplit(fullFormat, out text, out text2);
			text2 = ((text2 != null) ? text2.Trim() : null);
			text = ((text != null) ? text.Trim() : null);
			int num = 0;
			BurstString.NumberFormatKind numberFormatKind = BurstString.NumberFormatKind.General;
			bool flag = false;
			int num2 = 0;
			if (!string.IsNullOrEmpty(text2))
			{
				char c = text2[0];
				if (c <= 'X')
				{
					if (c == 'D')
					{
						numberFormatKind = BurstString.NumberFormatKind.Decimal;
						goto IL_00BA;
					}
					if (c == 'G')
					{
						numberFormatKind = BurstString.NumberFormatKind.General;
						goto IL_00BA;
					}
					if (c == 'X')
					{
						numberFormatKind = BurstString.NumberFormatKind.Hexadecimal;
						goto IL_00BA;
					}
				}
				else
				{
					if (c == 'd')
					{
						numberFormatKind = BurstString.NumberFormatKind.Decimal;
						flag = true;
						goto IL_00BA;
					}
					if (c == 'g')
					{
						numberFormatKind = BurstString.NumberFormatKind.General;
						flag = true;
						goto IL_00BA;
					}
					if (c == 'x')
					{
						numberFormatKind = BurstString.NumberFormatKind.Hexadecimal;
						flag = true;
						goto IL_00BA;
					}
				}
				throw new ArgumentException("Format `" + text2 + "` not supported. Only G, g, D, d, X, x are supported.");
				IL_00BA:
				if (text2.Length > 1)
				{
					string text3 = text2.Substring(1);
					uint num3;
					if (!uint.TryParse(text3, out num3))
					{
						throw new ArgumentException(string.Concat(new string[] { "Expecting an unsigned integer for specifier `", text2, "` instead of ", text3, "." }));
					}
					num2 = (int)num3;
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				if (text[0] != ',')
				{
					throw new ArgumentException("Invalid padding `" + text + "`, expecting to start with a leading `,` comma.");
				}
				string text4 = text.Substring(1);
				if (!int.TryParse(text4, out num))
				{
					throw new ArgumentException("Expecting an integer for align/size padding `" + text4 + "`.");
				}
			}
			return new BurstString.FormatOptions(numberFormatKind, (sbyte)num, (byte)num2, flag);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00003C1B File Offset: 0x00001E1B
		private unsafe static bool AlignRight(byte* dest, ref int destIndex, int destLength, int align, int length)
		{
			if (align < 0)
			{
				align = -align;
				return BurstString.AlignLeft(dest, ref destIndex, destLength, align, length);
			}
			return false;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00003C34 File Offset: 0x00001E34
		private unsafe static bool AlignLeft(byte* dest, ref int destIndex, int destLength, int align, int length)
		{
			if (align > 0)
			{
				while (length < align)
				{
					if (destIndex >= destLength)
					{
						return true;
					}
					int num = destIndex;
					destIndex = num + 1;
					dest[num] = 32;
					length++;
				}
			}
			return false;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00003C68 File Offset: 0x00001E68
		private unsafe static int GetLengthForFormatGeneral(ref BurstString.NumberBuffer number, int nMaxDigits)
		{
			int num = 0;
			int i = number.Scale;
			bool flag = false;
			if (i > nMaxDigits || i < -3)
			{
				i = 1;
				flag = true;
			}
			byte* ptr = number.GetDigitsPointer();
			if (number.IsNegative)
			{
				num++;
			}
			if (i > 0)
			{
				do
				{
					if (*ptr != 0)
					{
						ptr++;
					}
					num++;
				}
				while (--i > 0);
			}
			else
			{
				num++;
			}
			if (*ptr != 0 || i < 0)
			{
				num++;
				while (i < 0)
				{
					num++;
					i++;
				}
				while (*ptr != 0)
				{
					num++;
					ptr++;
				}
			}
			if (flag)
			{
				num++;
				int num2 = number.Scale - 1;
				if (num2 >= 0)
				{
					num++;
				}
				num += BurstString.GetLengthIntegerToString((long)num2, 10, 2);
			}
			return num;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00003D10 File Offset: 0x00001F10
		[MethodImpl(MethodImplOptions.NoInlining)]
		private unsafe static void FormatGeneral(byte* dest, ref int destIndex, int destLength, ref BurstString.NumberBuffer number, int nMaxDigits, byte expChar)
		{
			int i = number.Scale;
			bool flag = false;
			if (i > nMaxDigits || i < -3)
			{
				i = 1;
				flag = true;
			}
			byte* digitsPointer = number.GetDigitsPointer();
			int num;
			if (number.IsNegative)
			{
				if (destIndex >= destLength)
				{
					return;
				}
				num = destIndex;
				destIndex = num + 1;
				dest[num] = 45;
			}
			if (i > 0)
			{
				while (destIndex < destLength)
				{
					num = destIndex;
					destIndex = num + 1;
					dest[num] = ((*digitsPointer != 0) ? (*(digitsPointer++)) : 48);
					if (--i <= 0)
					{
						goto IL_007C;
					}
				}
				return;
			}
			if (destIndex >= destLength)
			{
				return;
			}
			num = destIndex;
			destIndex = num + 1;
			dest[num] = 48;
			IL_007C:
			if (*digitsPointer != 0 || i < 0)
			{
				if (destIndex >= destLength)
				{
					return;
				}
				num = destIndex;
				destIndex = num + 1;
				dest[num] = 46;
				while (i < 0)
				{
					if (destIndex >= destLength)
					{
						return;
					}
					num = destIndex;
					destIndex = num + 1;
					dest[num] = 48;
					i++;
				}
				while (*digitsPointer != 0)
				{
					if (destIndex >= destLength)
					{
						return;
					}
					num = destIndex;
					destIndex = num + 1;
					dest[num] = *(digitsPointer++);
				}
			}
			if (flag)
			{
				if (destIndex >= destLength)
				{
					return;
				}
				num = destIndex;
				destIndex = num + 1;
				dest[num] = expChar;
				int num2 = number.Scale - 1;
				BurstString.FormatOptions formatOptions = new BurstString.FormatOptions(BurstString.NumberFormatKind.DecimalForceSigned, 0, 2, false);
				BurstString.ConvertIntegerToString(dest, ref destIndex, destLength, (long)num2, formatOptions);
			}
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00003E2C File Offset: 0x0000202C
		private unsafe static void RoundNumber(ref BurstString.NumberBuffer number, int pos, bool isCorrectlyRounded)
		{
			byte* digitsPointer = number.GetDigitsPointer();
			int num = 0;
			while (num < pos && digitsPointer[num] != 0)
			{
				num++;
			}
			if (num == pos && BurstString.ShouldRoundUp(digitsPointer, num, isCorrectlyRounded))
			{
				while (num > 0 && digitsPointer[num - 1] == 57)
				{
					num--;
				}
				if (num > 0)
				{
					byte* ptr = digitsPointer + (num - 1);
					*ptr += 1;
				}
				else
				{
					number.Scale++;
					*digitsPointer = 49;
					num = 1;
				}
			}
			else
			{
				while (num > 0 && digitsPointer[num - 1] == 48)
				{
					num--;
				}
			}
			if (num == 0)
			{
				number.Scale = 0;
			}
			digitsPointer[num] = 0;
			number.DigitsCount = num;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00003EC0 File Offset: 0x000020C0
		private unsafe static bool ShouldRoundUp(byte* dig, int i, bool isCorrectlyRounded)
		{
			byte b = dig[i];
			return b != 0 && !isCorrectlyRounded && b >= 53;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00003EE4 File Offset: 0x000020E4
		private static uint LogBase2(uint val)
		{
			uint num = val >> 24;
			if (num != 0U)
			{
				return (uint)(24 + BurstString.logTable[(int)num]);
			}
			num = val >> 16;
			if (num != 0U)
			{
				return (uint)(16 + BurstString.logTable[(int)num]);
			}
			num = val >> 8;
			if (num != 0U)
			{
				return (uint)(8 + BurstString.logTable[(int)num]);
			}
			return (uint)BurstString.logTable[(int)val];
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00003F30 File Offset: 0x00002130
		private unsafe static int BigInt_Compare(in BurstString.tBigInt lhs, in BurstString.tBigInt rhs)
		{
			int num = lhs.m_length - rhs.m_length;
			if (num != 0)
			{
				return num;
			}
			int i = lhs.m_length - 1;
			while (i >= 0)
			{
				if (*((ref lhs.m_blocks.FixedElementField) + (IntPtr)i * 4) != *((ref rhs.m_blocks.FixedElementField) + (IntPtr)i * 4))
				{
					if (*((ref lhs.m_blocks.FixedElementField) + (IntPtr)i * 4) > *((ref rhs.m_blocks.FixedElementField) + (IntPtr)i * 4))
					{
						return 1;
					}
					return -1;
				}
				else
				{
					i--;
				}
			}
			return 0;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00003FB0 File Offset: 0x000021B0
		private static void BigInt_Add(out BurstString.tBigInt pResult, in BurstString.tBigInt lhs, in BurstString.tBigInt rhs)
		{
			if (lhs.m_length < rhs.m_length)
			{
				BurstString.BigInt_Add_internal(out pResult, in rhs, in lhs);
				return;
			}
			BurstString.BigInt_Add_internal(out pResult, in lhs, in rhs);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00003FD4 File Offset: 0x000021D4
		private unsafe static void BigInt_Add_internal(out BurstString.tBigInt pResult, in BurstString.tBigInt pLarge, in BurstString.tBigInt pSmall)
		{
			int length = pLarge.m_length;
			int length2 = pSmall.m_length;
			pResult.m_length = length;
			ulong num = 0UL;
			fixed (uint* ptr = &pLarge.m_blocks.FixedElementField)
			{
				uint* ptr2 = ptr;
				fixed (uint* ptr3 = &pSmall.m_blocks.FixedElementField)
				{
					uint* ptr4 = ptr3;
					fixed (uint* ptr5 = &pResult.m_blocks.FixedElementField)
					{
						uint* ptr6 = ptr5;
						uint* ptr7 = ptr2;
						uint* ptr8 = ptr4;
						uint* ptr9 = ptr6;
						uint* ptr10 = ptr7 + length;
						uint* ptr11 = ptr8 + length2;
						while (ptr8 != ptr11)
						{
							ulong num2 = num + (ulong)(*ptr7) + (ulong)(*ptr8);
							num = num2 >> 32;
							*ptr9 = (uint)(num2 & (ulong)(-1));
							ptr7++;
							ptr8++;
							ptr9++;
						}
						while (ptr7 != ptr10)
						{
							ulong num3 = num + (ulong)(*ptr7);
							num = num3 >> 32;
							*ptr9 = (uint)(num3 & (ulong)(-1));
							ptr7++;
							ptr9++;
						}
						if (num != 0UL)
						{
							*ptr9 = 1U;
							pResult.m_length = length + 1;
						}
						else
						{
							pResult.m_length = length;
						}
					}
				}
			}
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000040CD File Offset: 0x000022CD
		private static void BigInt_Multiply(out BurstString.tBigInt pResult, in BurstString.tBigInt lhs, in BurstString.tBigInt rhs)
		{
			if (lhs.m_length < rhs.m_length)
			{
				BurstString.BigInt_Multiply_internal(out pResult, in rhs, in lhs);
				return;
			}
			BurstString.BigInt_Multiply_internal(out pResult, in lhs, in rhs);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x000040F0 File Offset: 0x000022F0
		private unsafe static void BigInt_Multiply_internal(out BurstString.tBigInt pResult, in BurstString.tBigInt pLarge, in BurstString.tBigInt pSmall)
		{
			int num = pLarge.m_length + pSmall.m_length;
			for (int i = 0; i < num; i++)
			{
				*((ref pResult.m_blocks.FixedElementField) + (IntPtr)i * 4) = 0U;
			}
			fixed (uint* ptr = &pLarge.m_blocks.FixedElementField)
			{
				uint* ptr2 = ptr;
				uint* ptr3 = ptr2 + pLarge.m_length;
				fixed (uint* ptr4 = &pResult.m_blocks.FixedElementField)
				{
					uint* ptr5 = ptr4;
					fixed (uint* ptr6 = &pSmall.m_blocks.FixedElementField)
					{
						uint* ptr7 = ptr6;
						uint* ptr8 = ptr7 + pSmall.m_length;
						uint* ptr9 = ptr5;
						while (ptr7 != ptr8)
						{
							uint num2 = *ptr7;
							if (num2 != 0U)
							{
								uint* ptr10 = ptr2;
								uint* ptr11 = ptr9;
								ulong num3 = 0UL;
								do
								{
									ulong num4 = (ulong)(*ptr11) + (ulong)(*ptr10) * (ulong)num2 + num3;
									num3 = num4 >> 32;
									*ptr11 = (uint)(num4 & (ulong)(-1));
									ptr10++;
									ptr11++;
								}
								while (ptr10 != ptr3);
								*ptr11 = (uint)(num3 & (ulong)(-1));
							}
							ptr7++;
							ptr9++;
						}
						if (num > 0 && *((ref pResult.m_blocks.FixedElementField) + (IntPtr)(num - 1) * 4) == 0U)
						{
							pResult.m_length = num - 1;
						}
						else
						{
							pResult.m_length = num;
						}
					}
				}
			}
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00004214 File Offset: 0x00002414
		private unsafe static void BigInt_Multiply(out BurstString.tBigInt pResult, in BurstString.tBigInt lhs, uint rhs)
		{
			uint num = 0U;
			fixed (uint* ptr = &pResult.m_blocks.FixedElementField)
			{
				uint* ptr2 = ptr;
				fixed (uint* ptr3 = &lhs.m_blocks.FixedElementField)
				{
					uint* ptr4 = ptr3;
					uint* ptr5 = ptr2;
					uint* ptr6 = ptr4;
					uint* ptr7 = ptr6 + lhs.m_length;
					while (ptr6 != ptr7)
					{
						ulong num2 = (ulong)(*ptr6) * (ulong)rhs + (ulong)num;
						*ptr5 = (uint)(num2 & (ulong)(-1));
						num = (uint)(num2 >> 32);
						ptr6++;
						ptr5++;
					}
					if (num != 0U)
					{
						*ptr5 = num;
						pResult.m_length = lhs.m_length + 1;
					}
					else
					{
						pResult.m_length = lhs.m_length;
					}
				}
			}
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000042AC File Offset: 0x000024AC
		private unsafe static void BigInt_Multiply2(out BurstString.tBigInt pResult, in BurstString.tBigInt input)
		{
			uint num = 0U;
			fixed (uint* ptr = &pResult.m_blocks.FixedElementField)
			{
				uint* ptr2 = ptr;
				fixed (uint* ptr3 = &input.m_blocks.FixedElementField)
				{
					uint* ptr4 = ptr3;
					uint* ptr5 = ptr2;
					uint* ptr6 = ptr4;
					uint* ptr7 = ptr6 + input.m_length;
					while (ptr6 != ptr7)
					{
						uint num2 = *ptr6;
						*ptr5 = (num2 << 1) | num;
						num = num2 >> 31;
						ptr6++;
						ptr5++;
					}
					if (num != 0U)
					{
						*ptr5 = num;
						pResult.m_length = input.m_length + 1;
					}
					else
					{
						pResult.m_length = input.m_length;
					}
				}
			}
		}

		// Token: 0x060000BA RID: 186 RVA: 0x0000433C File Offset: 0x0000253C
		private unsafe static void BigInt_Multiply2(ref BurstString.tBigInt pResult)
		{
			uint num = 0U;
			fixed (uint* ptr = &pResult.m_blocks.FixedElementField)
			{
				uint* ptr2 = ptr;
				uint* ptr3 = ptr2 + pResult.m_length;
				while (ptr2 != ptr3)
				{
					uint num2 = *ptr2;
					*ptr2 = (num2 << 1) | num;
					num = num2 >> 31;
					ptr2++;
				}
				if (num != 0U)
				{
					*ptr2 = num;
					pResult.m_length++;
				}
			}
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00004398 File Offset: 0x00002598
		private unsafe static void BigInt_Multiply10(ref BurstString.tBigInt pResult)
		{
			ulong num = 0UL;
			fixed (uint* ptr = &pResult.m_blocks.FixedElementField)
			{
				uint* ptr2 = ptr;
				uint* ptr3 = ptr2 + pResult.m_length;
				while (ptr2 != ptr3)
				{
					ulong num2 = (ulong)(*ptr2) * 10UL + num;
					*ptr2 = (uint)(num2 & (ulong)(-1));
					num = num2 >> 32;
					ptr2++;
				}
				if (num != 0UL)
				{
					*ptr2 = (uint)num;
					pResult.m_length++;
				}
			}
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000043FC File Offset: 0x000025FC
		private unsafe static BurstString.tBigInt g_PowerOf10_Big(int i)
		{
			BurstString.tBigInt tBigInt;
			if (i == 0)
			{
				tBigInt.m_length = 1;
				tBigInt.m_blocks.FixedElementField = 100000000U;
			}
			else if (i == 1)
			{
				tBigInt.m_length = 2;
				tBigInt.m_blocks.FixedElementField = 1874919424U;
				*((ref tBigInt.m_blocks.FixedElementField) + 4) = 2328306U;
			}
			else if (i == 2)
			{
				tBigInt.m_length = 4;
				tBigInt.m_blocks.FixedElementField = 0U;
				*((ref tBigInt.m_blocks.FixedElementField) + 4) = 2242703233U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)2 * 4) = 762134875U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)3 * 4) = 1262U;
			}
			else if (i == 3)
			{
				tBigInt.m_length = 7;
				tBigInt.m_blocks.FixedElementField = 0U;
				*((ref tBigInt.m_blocks.FixedElementField) + 4) = 0U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)2 * 4) = 3211403009U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)3 * 4) = 1849224548U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)4 * 4) = 3668416493U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)5 * 4) = 3913284084U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)6 * 4) = 1593091U;
			}
			else if (i == 4)
			{
				tBigInt.m_length = 14;
				tBigInt.m_blocks.FixedElementField = 0U;
				*((ref tBigInt.m_blocks.FixedElementField) + 4) = 0U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)2 * 4) = 0U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)3 * 4) = 0U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)4 * 4) = 781532673U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)5 * 4) = 64985353U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)6 * 4) = 253049085U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)7 * 4) = 594863151U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)8 * 4) = 3553621484U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)9 * 4) = 3288652808U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)10 * 4) = 3167596762U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)11 * 4) = 2788392729U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)12 * 4) = 3911132675U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)13 * 4) = 590U;
			}
			else
			{
				tBigInt.m_length = 27;
				tBigInt.m_blocks.FixedElementField = 0U;
				*((ref tBigInt.m_blocks.FixedElementField) + 4) = 0U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)2 * 4) = 0U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)3 * 4) = 0U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)4 * 4) = 0U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)5 * 4) = 0U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)6 * 4) = 0U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)7 * 4) = 0U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)8 * 4) = 2553183233U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)9 * 4) = 3201533787U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)10 * 4) = 3638140786U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)11 * 4) = 303378311U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)12 * 4) = 1809731782U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)13 * 4) = 3477761648U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)14 * 4) = 3583367183U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)15 * 4) = 649228654U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)16 * 4) = 2915460784U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)17 * 4) = 487929380U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)18 * 4) = 1011012442U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)19 * 4) = 1677677582U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)20 * 4) = 3428152256U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)21 * 4) = 1710878487U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)22 * 4) = 1438394610U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)23 * 4) = 2161952759U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)24 * 4) = 4100910556U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)25 * 4) = 1608314830U;
				*((ref tBigInt.m_blocks.FixedElementField) + (IntPtr)26 * 4) = 349175U;
			}
			return tBigInt;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00004910 File Offset: 0x00002B10
		private static void BigInt_Pow10(out BurstString.tBigInt pResult, uint exponent)
		{
			BurstString.tBigInt tBigInt = default(BurstString.tBigInt);
			BurstString.tBigInt tBigInt2 = default(BurstString.tBigInt);
			ref BurstString.tBigInt ptr = ref tBigInt;
			ref BurstString.tBigInt ptr2 = ref tBigInt2;
			uint num = exponent & 7U;
			ptr.SetU32(BurstString.g_PowerOf10_U32[(int)num]);
			exponent >>= 3;
			int num2 = 0;
			while (exponent != 0U)
			{
				if ((exponent & 1U) != 0U)
				{
					ref BurstString.tBigInt ptr3 = ref ptr2;
					ref BurstString.tBigInt ptr4 = ref ptr;
					BurstString.tBigInt tBigInt3 = BurstString.g_PowerOf10_Big(num2);
					BurstString.BigInt_Multiply(out ptr3, in ptr4, in tBigInt3);
					ref BurstString.tBigInt ptr5 = ref ptr;
					ptr = ptr2;
					ptr2 = ptr5;
				}
				num2++;
				exponent >>= 1;
			}
			pResult = ptr;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000049A0 File Offset: 0x00002BA0
		private static void BigInt_MultiplyPow10(out BurstString.tBigInt pResult, in BurstString.tBigInt input, uint exponent)
		{
			BurstString.tBigInt tBigInt = default(BurstString.tBigInt);
			BurstString.tBigInt tBigInt2 = default(BurstString.tBigInt);
			ref BurstString.tBigInt ptr = ref tBigInt;
			ref BurstString.tBigInt ptr2 = ref tBigInt2;
			uint num = exponent & 7U;
			if (num != 0U)
			{
				BurstString.BigInt_Multiply(out ptr, in input, BurstString.g_PowerOf10_U32[(int)num]);
			}
			else
			{
				ptr = input;
			}
			exponent >>= 3;
			int num2 = 0;
			while (exponent != 0U)
			{
				if ((exponent & 1U) != 0U)
				{
					ref BurstString.tBigInt ptr3 = ref ptr2;
					ref BurstString.tBigInt ptr4 = ref ptr;
					BurstString.tBigInt tBigInt3 = BurstString.g_PowerOf10_Big(num2);
					BurstString.BigInt_Multiply(out ptr3, in ptr4, in tBigInt3);
					ref BurstString.tBigInt ptr5 = ref ptr;
					ptr = ptr2;
					ptr2 = ptr5;
				}
				num2++;
				exponent >>= 1;
			}
			pResult = ptr;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00004A40 File Offset: 0x00002C40
		private unsafe static void BigInt_Pow2(out BurstString.tBigInt pResult, uint exponent)
		{
			int num = (int)(exponent / 32U);
			uint num2 = 0U;
			while ((ulong)num2 <= (ulong)((long)num))
			{
				*((ref pResult.m_blocks.FixedElementField) + (IntPtr)((ulong)num2 * 4UL)) = 0U;
				num2 += 1U;
			}
			pResult.m_length = num + 1;
			int num3 = (int)(exponent % 32U);
			*((ref pResult.m_blocks.FixedElementField) + (IntPtr)num * 4) |= 1U << num3;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00004A9C File Offset: 0x00002C9C
		private unsafe static uint BigInt_DivideWithRemainder_MaxQuotient9(ref BurstString.tBigInt pDividend, in BurstString.tBigInt divisor)
		{
			int num = divisor.m_length;
			if (pDividend.m_length < divisor.m_length)
			{
				return 0U;
			}
			fixed (uint* ptr = &divisor.m_blocks.FixedElementField)
			{
				uint* ptr2 = ptr;
				fixed (uint* ptr3 = &pDividend.m_blocks.FixedElementField)
				{
					uint* ptr4 = ptr3;
					uint* ptr5 = ptr2;
					uint* ptr6 = ptr4;
					uint* ptr7 = ptr5 + num - 1;
					uint num2 = *(ptr6 + num - 1) / (*ptr7 + 1U);
					if (num2 != 0U)
					{
						ulong num3 = 0UL;
						ulong num4 = 0UL;
						do
						{
							ulong num5 = (ulong)(*ptr5) * (ulong)num2 + num4;
							num4 = num5 >> 32;
							ulong num6 = (ulong)(*ptr6) - (num5 & (ulong)(-1)) - num3;
							num3 = (num6 >> 32) & 1UL;
							*ptr6 = (uint)(num6 & (ulong)(-1));
							ptr5++;
							ptr6++;
						}
						while (ptr5 == ptr7);
						while (num > 0 && *((ref pDividend.m_blocks.FixedElementField) + (IntPtr)(num - 1) * 4) == 0U)
						{
							num--;
						}
						pDividend.m_length = num;
					}
					if (BurstString.BigInt_Compare(in pDividend, in divisor) >= 0)
					{
						num2 += 1U;
						ptr5 = ptr2;
						ptr6 = ptr4;
						ulong num7 = 0UL;
						do
						{
							ulong num8 = (ulong)(*ptr6) - (ulong)(*ptr5) - num7;
							num7 = (num8 >> 32) & 1UL;
							*ptr6 = (uint)(num8 & (ulong)(-1));
							ptr5++;
							ptr6++;
						}
						while (ptr5 == ptr7);
						while (num > 0 && *((ref pDividend.m_blocks.FixedElementField) + (IntPtr)(num - 1) * 4) == 0U)
						{
							num--;
						}
						pDividend.m_length = num;
					}
					return num2;
				}
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00004BF4 File Offset: 0x00002DF4
		private unsafe static void BigInt_ShiftLeft(ref BurstString.tBigInt pResult, uint shift)
		{
			int num = (int)(shift / 32U);
			int num2 = (int)(shift % 32U);
			int length = pResult.m_length;
			if (num2 == 0)
			{
				fixed (uint* ptr = &pResult.m_blocks.FixedElementField)
				{
					uint* ptr2 = ptr;
					uint* ptr3 = ptr2 + length - 1;
					uint* ptr4 = ptr3 + num;
					while (ptr3 >= ptr2)
					{
						*ptr4 = *ptr3;
						ptr3--;
						ptr4--;
					}
				}
				uint num3 = 0U;
				while ((ulong)num3 < (ulong)((long)num))
				{
					*((ref pResult.m_blocks.FixedElementField) + (IntPtr)((ulong)num3 * 4UL)) = 0U;
					num3 += 1U;
				}
				pResult.m_length += num;
				return;
			}
			int i = length - 1;
			int num4 = length + num;
			pResult.m_length = num4 + 1;
			int num5 = 32 - num2;
			uint num6 = 0U;
			uint num7 = *((ref pResult.m_blocks.FixedElementField) + (IntPtr)i * 4);
			uint num8 = num7 >> num5;
			while (i > 0)
			{
				*((ref pResult.m_blocks.FixedElementField) + (IntPtr)num4 * 4) = num6 | num8;
				num6 = num7 << num2;
				i--;
				num4--;
				num7 = *((ref pResult.m_blocks.FixedElementField) + (IntPtr)i * 4);
				num8 = num7 >> num5;
			}
			*((ref pResult.m_blocks.FixedElementField) + (IntPtr)num4 * 4) = num6 | num8;
			*((ref pResult.m_blocks.FixedElementField) + (IntPtr)(num4 - 1) * 4) = num7 << num2;
			uint num9 = 0U;
			while ((ulong)num9 < (ulong)((long)num))
			{
				*((ref pResult.m_blocks.FixedElementField) + (IntPtr)((ulong)num9 * 4UL)) = 0U;
				num9 += 1U;
			}
			if (*((ref pResult.m_blocks.FixedElementField) + (IntPtr)(pResult.m_length - 1) * 4) == 0U)
			{
				pResult.m_length--;
			}
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00004D94 File Offset: 0x00002F94
		private unsafe static uint Dragon4(ulong mantissa, int exponent, uint mantissaHighBitIdx, bool hasUnequalMargins, BurstString.CutoffMode cutoffMode, uint cutoffNumber, byte* pOutBuffer, uint bufferSize, out int pOutExponent)
		{
			byte* ptr = pOutBuffer;
			if (mantissa == 0UL)
			{
				*ptr = 48;
				pOutExponent = 0;
				return 1U;
			}
			BurstString.tBigInt tBigInt = default(BurstString.tBigInt);
			BurstString.tBigInt tBigInt2 = default(BurstString.tBigInt);
			BurstString.tBigInt tBigInt3 = default(BurstString.tBigInt);
			BurstString.tBigInt tBigInt4 = default(BurstString.tBigInt);
			BurstString.tBigInt* ptr2;
			if (hasUnequalMargins)
			{
				if (exponent > 0)
				{
					tBigInt2.SetU64(4UL * mantissa);
					BurstString.BigInt_ShiftLeft(ref tBigInt2, (uint)exponent);
					tBigInt.SetU32(4U);
					BurstString.BigInt_Pow2(out tBigInt3, (uint)exponent);
					BurstString.BigInt_Pow2(out tBigInt4, (uint)(exponent + 1));
				}
				else
				{
					tBigInt2.SetU64(4UL * mantissa);
					BurstString.BigInt_Pow2(out tBigInt, (uint)(-exponent + 2));
					tBigInt3.SetU32(1U);
					tBigInt4.SetU32(2U);
				}
				ptr2 = &tBigInt4;
			}
			else
			{
				if (exponent > 0)
				{
					tBigInt2.SetU64(2UL * mantissa);
					BurstString.BigInt_ShiftLeft(ref tBigInt2, (uint)exponent);
					tBigInt.SetU32(2U);
					BurstString.BigInt_Pow2(out tBigInt3, (uint)exponent);
				}
				else
				{
					tBigInt2.SetU64(2UL * mantissa);
					BurstString.BigInt_Pow2(out tBigInt, (uint)(-exponent + 1));
					tBigInt3.SetU32(1U);
				}
				ptr2 = &tBigInt3;
			}
			int num = (int)Math.Ceiling((double)(mantissaHighBitIdx + (uint)exponent) * 0.3010299956639812 - 0.69);
			if (cutoffMode == BurstString.CutoffMode.FractionLength && num <= (int)(-(int)cutoffNumber))
			{
				num = (int)(-cutoffNumber + 1U);
			}
			if (num > 0)
			{
				BurstString.tBigInt tBigInt5;
				BurstString.BigInt_MultiplyPow10(out tBigInt5, in tBigInt, (uint)num);
				tBigInt = tBigInt5;
			}
			else if (num < 0)
			{
				BurstString.tBigInt tBigInt6;
				BurstString.BigInt_Pow10(out tBigInt6, (uint)(-(uint)num));
				BurstString.tBigInt tBigInt7;
				BurstString.BigInt_Multiply(out tBigInt7, in tBigInt2, in tBigInt6);
				tBigInt2 = tBigInt7;
				BurstString.BigInt_Multiply(out tBigInt7, in tBigInt3, in tBigInt6);
				tBigInt3 = tBigInt7;
				if (ptr2 != &tBigInt3)
				{
					BurstString.BigInt_Multiply2(out *ptr2, in tBigInt3);
				}
			}
			if (BurstString.BigInt_Compare(in tBigInt2, in tBigInt) >= 0)
			{
				num++;
			}
			else
			{
				BurstString.BigInt_Multiply10(ref tBigInt2);
				BurstString.BigInt_Multiply10(ref tBigInt3);
				if (ptr2 != &tBigInt3)
				{
					BurstString.BigInt_Multiply2(out *ptr2, in tBigInt3);
				}
			}
			int num2 = num - (int)bufferSize;
			switch (cutoffMode)
			{
			case BurstString.CutoffMode.TotalLength:
			{
				int num3 = num - (int)cutoffNumber;
				if (num3 > num2)
				{
					num2 = num3;
				}
				break;
			}
			case BurstString.CutoffMode.FractionLength:
			{
				int num4 = (int)(-(int)cutoffNumber);
				if (num4 > num2)
				{
					num2 = num4;
				}
				break;
			}
			}
			pOutExponent = num - 1;
			uint block = tBigInt.GetBlock(tBigInt.GetLength() - 1);
			if (block < 8U || block > 429496729U)
			{
				uint num5 = BurstString.LogBase2(block);
				uint num6 = (59U - num5) % 32U;
				BurstString.BigInt_ShiftLeft(ref tBigInt, num6);
				BurstString.BigInt_ShiftLeft(ref tBigInt2, num6);
				BurstString.BigInt_ShiftLeft(ref tBigInt3, num6);
				if (ptr2 != &tBigInt3)
				{
					BurstString.BigInt_Multiply2(out *ptr2, in tBigInt3);
				}
			}
			uint num7;
			bool flag;
			bool flag2;
			if (cutoffMode == BurstString.CutoffMode.Unique)
			{
				for (;;)
				{
					num--;
					num7 = BurstString.BigInt_DivideWithRemainder_MaxQuotient9(ref tBigInt2, in tBigInt);
					BurstString.tBigInt tBigInt8;
					BurstString.BigInt_Add(out tBigInt8, in tBigInt2, in *ptr2);
					flag = BurstString.BigInt_Compare(in tBigInt2, in tBigInt3) < 0;
					flag2 = BurstString.BigInt_Compare(in tBigInt8, in tBigInt) > 0;
					if ((flag || flag2) | (num == num2))
					{
						break;
					}
					*ptr = (byte)(48U + num7);
					ptr++;
					BurstString.BigInt_Multiply10(ref tBigInt2);
					BurstString.BigInt_Multiply10(ref tBigInt3);
					if (ptr2 != &tBigInt3)
					{
						BurstString.BigInt_Multiply2(out *ptr2, in tBigInt3);
					}
				}
			}
			else
			{
				flag = false;
				flag2 = false;
				for (;;)
				{
					num--;
					num7 = BurstString.BigInt_DivideWithRemainder_MaxQuotient9(ref tBigInt2, in tBigInt);
					if (tBigInt2.IsZero() | (num == num2))
					{
						break;
					}
					*ptr = (byte)(48U + num7);
					ptr++;
					BurstString.BigInt_Multiply10(ref tBigInt2);
				}
			}
			bool flag3 = flag;
			if (flag == flag2)
			{
				BurstString.BigInt_Multiply2(ref tBigInt2);
				int num8 = BurstString.BigInt_Compare(in tBigInt2, in tBigInt);
				flag3 = num8 < 0;
				if (num8 == 0)
				{
					flag3 = (num7 & 1U) == 0U;
				}
			}
			if (flag3)
			{
				*ptr = (byte)(48U + num7);
				ptr++;
			}
			else if (num7 == 9U)
			{
				while (ptr != pOutBuffer)
				{
					ptr--;
					if (*ptr != 57)
					{
						byte* ptr3 = ptr;
						*ptr3 += 1;
						ptr++;
						goto IL_0368;
					}
				}
				*ptr = 49;
				ptr++;
				pOutExponent++;
			}
			else
			{
				*ptr = (byte)(48U + num7 + 1U);
				ptr++;
			}
			IL_0368:
			return (uint)((long)(ptr - pOutBuffer));
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00005114 File Offset: 0x00003314
		private unsafe static int FormatPositional(byte* pOutBuffer, uint bufferSize, ulong mantissa, int exponent, uint mantissaHighBitIdx, bool hasUnequalMargins, int precision)
		{
			uint num = bufferSize - 1U;
			int num3;
			uint num2;
			if (precision < 0)
			{
				num2 = BurstString.Dragon4(mantissa, exponent, mantissaHighBitIdx, hasUnequalMargins, BurstString.CutoffMode.Unique, 0U, pOutBuffer, num, out num3);
			}
			else
			{
				num2 = BurstString.Dragon4(mantissa, exponent, mantissaHighBitIdx, hasUnequalMargins, BurstString.CutoffMode.FractionLength, (uint)precision, pOutBuffer, num, out num3);
			}
			uint num4 = 0U;
			if (num3 >= 0)
			{
				uint num5 = (uint)(num3 + 1);
				if (num2 < num5)
				{
					if (num5 > num)
					{
						num5 = num;
					}
					while (num2 < num5)
					{
						pOutBuffer[num2] = 48;
						num2 += 1U;
					}
				}
				else if (num2 > num5)
				{
					num4 = num2 - num5;
					uint num6 = num - num5 - 1U;
					if (num4 > num6)
					{
						num4 = num6;
					}
					Unsafe.CopyBlock((void*)(pOutBuffer + num5 + 1), (void*)(pOutBuffer + num5), num4);
					pOutBuffer[num5] = 46;
					num2 = num5 + 1U + num4;
				}
			}
			else
			{
				if (num > 2U)
				{
					uint num7 = (uint)(-num3 - 1);
					uint num8 = num - 2U;
					if (num7 > num8)
					{
						num7 = num8;
					}
					uint num9 = 2U + num7;
					num4 = num2;
					uint num10 = num - num9;
					if (num4 > num10)
					{
						num4 = num10;
					}
					Unsafe.CopyBlock((void*)(pOutBuffer + num9), (void*)pOutBuffer, num4);
					for (uint num11 = 2U; num11 < num9; num11 += 1U)
					{
						pOutBuffer[num11] = 48;
					}
					num4 += num7;
					num2 = num4;
				}
				if (num > 1U)
				{
					pOutBuffer[1] = 46;
					num2 += 1U;
				}
				if (num > 0U)
				{
					*pOutBuffer = 48;
					num2 += 1U;
				}
			}
			if (precision > (int)num4 && num2 < num)
			{
				if (num4 == 0U)
				{
					pOutBuffer[num2++] = 46;
				}
				uint num12 = (uint)((ulong)num2 + (ulong)((long)(precision - (int)num4)));
				if (num12 > num)
				{
					num12 = num;
				}
				while (num2 < num12)
				{
					pOutBuffer[num2] = 48;
					num2 += 1U;
				}
			}
			return (int)num2;
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00005274 File Offset: 0x00003474
		private unsafe static int FormatScientific(byte* pOutBuffer, uint bufferSize, ulong mantissa, int exponent, uint mantissaHighBitIdx, bool hasUnequalMargins, int precision)
		{
			int num2;
			uint num;
			if (precision < 0)
			{
				num = BurstString.Dragon4(mantissa, exponent, mantissaHighBitIdx, hasUnequalMargins, BurstString.CutoffMode.Unique, 0U, pOutBuffer, bufferSize, out num2);
			}
			else
			{
				num = BurstString.Dragon4(mantissa, exponent, mantissaHighBitIdx, hasUnequalMargins, BurstString.CutoffMode.TotalLength, (uint)(precision + 1), pOutBuffer, bufferSize, out num2);
			}
			byte* ptr = pOutBuffer;
			if (bufferSize > 1U)
			{
				ptr++;
				bufferSize -= 1U;
			}
			uint num3 = num - 1U;
			if (num3 > 0U && bufferSize > 1U)
			{
				uint num4 = bufferSize - 2U;
				if (num3 > num4)
				{
					num3 = num4;
				}
				Unsafe.CopyBlock((void*)(ptr + 1), (void*)ptr, num3);
				*ptr = 46;
				ptr += 1U + num3;
				bufferSize -= 1U + num3;
			}
			if (precision > (int)num3 && bufferSize > 1U)
			{
				if (num3 == 0U)
				{
					*ptr = 46;
					ptr++;
					bufferSize -= 1U;
				}
				uint num5 = (uint)((long)precision - (long)((ulong)num3));
				if (num5 > bufferSize - 1U)
				{
					num5 = bufferSize - 1U;
				}
				byte* ptr2 = ptr + num5;
				while (ptr < ptr2)
				{
					*ptr = 48;
					ptr++;
				}
			}
			if (bufferSize > 1U)
			{
				byte* ptr3 = stackalloc byte[(UIntPtr)5];
				*ptr3 = 101;
				if (num2 >= 0)
				{
					ptr3[1] = 43;
				}
				else
				{
					ptr3[1] = 45;
					num2 = -num2;
				}
				uint num6 = (uint)(num2 / 100);
				uint num7 = (uint)(((long)num2 - (long)((ulong)(num6 * 100U))) / 10L);
				uint num8 = (uint)((long)num2 - (long)((ulong)(num6 * 100U)) - (long)((ulong)(num7 * 10U)));
				ptr3[2] = (byte)(48U + num6);
				ptr3[3] = (byte)(48U + num7);
				ptr3[4] = (byte)(48U + num8);
				uint num9 = bufferSize - 1U;
				uint num10 = ((5U < num9) ? 5U : num9);
				Unsafe.CopyBlock((void*)ptr, (void*)ptr3, num10);
				ptr += num10;
				bufferSize -= num10;
			}
			return (int)((long)(ptr - pOutBuffer));
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x000053D8 File Offset: 0x000035D8
		private unsafe static void FormatInfinityNaN(byte* dest, ref int destIndex, int destLength, ulong mantissa, bool isNegative, BurstString.FormatOptions formatOptions)
		{
			int num = ((mantissa == 0UL) ? (8 + (isNegative ? 1 : 0)) : 3);
			int alignAndSize = (int)formatOptions.AlignAndSize;
			if (BurstString.AlignLeft(dest, ref destIndex, destLength, alignAndSize, num))
			{
				return;
			}
			if (mantissa == 0UL)
			{
				if (isNegative)
				{
					if (destIndex >= destLength)
					{
						return;
					}
					int num2 = destIndex;
					destIndex = num2 + 1;
					dest[num2] = 45;
				}
				for (int i = 0; i < 8; i++)
				{
					if (destIndex >= destLength)
					{
						return;
					}
					int num2 = destIndex;
					destIndex = num2 + 1;
					dest[num2] = BurstString.InfinityString[i];
				}
			}
			else
			{
				for (int j = 0; j < 3; j++)
				{
					if (destIndex >= destLength)
					{
						return;
					}
					int num2 = destIndex;
					destIndex = num2 + 1;
					dest[num2] = BurstString.NanString[j];
				}
			}
			BurstString.AlignRight(dest, ref destIndex, destLength, alignAndSize, num);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00005484 File Offset: 0x00003684
		[MethodImpl(MethodImplOptions.NoInlining)]
		private unsafe static void ConvertFloatToString(byte* dest, ref int destIndex, int destLength, float value, BurstString.FormatOptions formatOptions)
		{
			BurstString.tFloatUnion32 tFloatUnion = default(BurstString.tFloatUnion32);
			tFloatUnion.m_floatingPoint = value;
			uint exponent = tFloatUnion.GetExponent();
			uint mantissa = tFloatUnion.GetMantissa();
			if (exponent == 255U)
			{
				BurstString.FormatInfinityNaN(dest, ref destIndex, destLength, (ulong)mantissa, tFloatUnion.IsNegative(), formatOptions);
				return;
			}
			uint num;
			int num2;
			uint num3;
			bool flag;
			if (exponent != 0U)
			{
				num = (uint)(8388608UL | (ulong)mantissa);
				num2 = (int)(exponent - 127U - 23U);
				num3 = 23U;
				flag = exponent != 1U && mantissa == 0U;
			}
			else
			{
				num = mantissa;
				num2 = -149;
				num3 = BurstString.LogBase2(num);
				flag = false;
			}
			int num4 = ((formatOptions.Specifier == 0) ? (-1) : ((int)formatOptions.Specifier));
			int num5 = Math.Max(10, num4 + 1);
			byte* ptr = stackalloc byte[(UIntPtr)num5];
			if (num4 < 0)
			{
				num4 = 7;
			}
			int num7;
			uint num6 = BurstString.Dragon4((ulong)num, num2, num3, flag, BurstString.CutoffMode.TotalLength, (uint)num4, ptr, (uint)(num5 - 1), out num7);
			ptr[num6] = 0;
			bool flag2 = tFloatUnion.IsNegative();
			if (tFloatUnion.m_integer == 2147483648U)
			{
				flag2 = false;
			}
			BurstString.NumberBuffer numberBuffer = new BurstString.NumberBuffer(BurstString.NumberBufferKind.Float, ptr, (int)num6, num7 + 1, flag2);
			BurstString.FormatNumber(dest, ref destIndex, destLength, ref numberBuffer, num4, formatOptions);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00005598 File Offset: 0x00003798
		[MethodImpl(MethodImplOptions.NoInlining)]
		private unsafe static void ConvertDoubleToString(byte* dest, ref int destIndex, int destLength, double value, BurstString.FormatOptions formatOptions)
		{
			BurstString.tFloatUnion64 tFloatUnion = default(BurstString.tFloatUnion64);
			tFloatUnion.m_floatingPoint = value;
			uint exponent = tFloatUnion.GetExponent();
			ulong mantissa = tFloatUnion.GetMantissa();
			if (exponent == 2047U)
			{
				BurstString.FormatInfinityNaN(dest, ref destIndex, destLength, mantissa, tFloatUnion.IsNegative(), formatOptions);
				return;
			}
			ulong num;
			int num2;
			uint num3;
			bool flag;
			if (exponent != 0U)
			{
				num = 4503599627370496UL | mantissa;
				num2 = (int)(exponent - 1023U - 52U);
				num3 = 52U;
				flag = exponent != 1U && mantissa == 0UL;
			}
			else
			{
				num = mantissa;
				num2 = -1074;
				num3 = BurstString.LogBase2((uint)num);
				flag = false;
			}
			int num4 = ((formatOptions.Specifier == 0) ? (-1) : ((int)formatOptions.Specifier));
			int num5 = Math.Max(18, num4 + 1);
			byte* ptr = stackalloc byte[(UIntPtr)num5];
			if (num4 < 0)
			{
				num4 = 15;
			}
			int num7;
			uint num6 = BurstString.Dragon4(num, num2, num3, flag, BurstString.CutoffMode.TotalLength, (uint)num4, ptr, (uint)(num5 - 1), out num7);
			ptr[num6] = 0;
			bool flag2 = tFloatUnion.IsNegative();
			if (tFloatUnion.m_integer == 9223372036854775808UL)
			{
				flag2 = false;
			}
			BurstString.NumberBuffer numberBuffer = new BurstString.NumberBuffer(BurstString.NumberBufferKind.Float, ptr, (int)num6, num7 + 1, flag2);
			BurstString.FormatNumber(dest, ref destIndex, destLength, ref numberBuffer, num4, formatOptions);
		}

		// Token: 0x040000D2 RID: 210
		private static readonly char[] SplitByColon = new char[] { ':' };

		// Token: 0x040000D3 RID: 211
		private static readonly byte[] logTable = new byte[]
		{
			0, 0, 1, 1, 2, 2, 2, 2, 3, 3,
			3, 3, 3, 3, 3, 3, 4, 4, 4, 4,
			4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
			4, 4, 5, 5, 5, 5, 5, 5, 5, 5,
			5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
			5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
			5, 5, 5, 5, 6, 6, 6, 6, 6, 6,
			6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
			6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
			6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
			6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
			6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
			6, 6, 6, 6, 6, 6, 6, 6, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7
		};

		// Token: 0x040000D4 RID: 212
		private static readonly uint[] g_PowerOf10_U32 = new uint[] { 1U, 10U, 100U, 1000U, 10000U, 100000U, 1000000U, 10000000U };

		// Token: 0x040000D5 RID: 213
		private static readonly byte[] InfinityString = new byte[] { 73, 110, 102, 105, 110, 105, 116, 121 };

		// Token: 0x040000D6 RID: 214
		private static readonly byte[] NanString = new byte[] { 78, 97, 78 };

		// Token: 0x040000D7 RID: 215
		private const int SinglePrecision = 9;

		// Token: 0x040000D8 RID: 216
		private const int DoublePrecision = 17;

		// Token: 0x040000D9 RID: 217
		internal const int SingleNumberBufferLength = 10;

		// Token: 0x040000DA RID: 218
		internal const int DoubleNumberBufferLength = 18;

		// Token: 0x040000DB RID: 219
		private const int SinglePrecisionCustomFormat = 7;

		// Token: 0x040000DC RID: 220
		private const int DoublePrecisionCustomFormat = 15;

		// Token: 0x0200001F RID: 31
		internal class PreserveAttribute : Attribute
		{
		}

		// Token: 0x02000020 RID: 32
		private enum NumberBufferKind
		{
			// Token: 0x040000DE RID: 222
			Integer,
			// Token: 0x040000DF RID: 223
			Float
		}

		// Token: 0x02000021 RID: 33
		private struct NumberBuffer
		{
			// Token: 0x060000CA RID: 202 RVA: 0x0000572D File Offset: 0x0000392D
			public unsafe NumberBuffer(BurstString.NumberBufferKind kind, byte* buffer, int digitsCount, int scale, bool isNegative)
			{
				this.Kind = kind;
				this._buffer = buffer;
				this.DigitsCount = digitsCount;
				this.Scale = scale;
				this.IsNegative = isNegative;
			}

			// Token: 0x060000CB RID: 203 RVA: 0x00005754 File Offset: 0x00003954
			public unsafe byte* GetDigitsPointer()
			{
				return this._buffer;
			}

			// Token: 0x040000E0 RID: 224
			private unsafe readonly byte* _buffer;

			// Token: 0x040000E1 RID: 225
			public BurstString.NumberBufferKind Kind;

			// Token: 0x040000E2 RID: 226
			public int DigitsCount;

			// Token: 0x040000E3 RID: 227
			public int Scale;

			// Token: 0x040000E4 RID: 228
			public readonly bool IsNegative;
		}

		// Token: 0x02000022 RID: 34
		public enum NumberFormatKind : byte
		{
			// Token: 0x040000E6 RID: 230
			General,
			// Token: 0x040000E7 RID: 231
			Decimal,
			// Token: 0x040000E8 RID: 232
			DecimalForceSigned,
			// Token: 0x040000E9 RID: 233
			Hexadecimal
		}

		// Token: 0x02000023 RID: 35
		public struct FormatOptions
		{
			// Token: 0x060000CC RID: 204 RVA: 0x0000575C File Offset: 0x0000395C
			public FormatOptions(BurstString.NumberFormatKind kind, sbyte alignAndSize, byte specifier, bool lowercase)
			{
				this = default(BurstString.FormatOptions);
				this.Kind = kind;
				this.AlignAndSize = alignAndSize;
				this.Specifier = specifier;
				this.Lowercase = lowercase;
			}

			// Token: 0x17000017 RID: 23
			// (get) Token: 0x060000CD RID: 205 RVA: 0x00005782 File Offset: 0x00003982
			public bool Uppercase
			{
				get
				{
					return !this.Lowercase;
				}
			}

			// Token: 0x060000CE RID: 206 RVA: 0x00005790 File Offset: 0x00003990
			public unsafe int EncodeToRaw()
			{
				BurstString.FormatOptions formatOptions = this;
				return *(int*)(&formatOptions);
			}

			// Token: 0x060000CF RID: 207 RVA: 0x000057A8 File Offset: 0x000039A8
			public int GetBase()
			{
				if (this.Kind == BurstString.NumberFormatKind.Hexadecimal)
				{
					return 16;
				}
				return 10;
			}

			// Token: 0x060000D0 RID: 208 RVA: 0x000057B8 File Offset: 0x000039B8
			public override string ToString()
			{
				return string.Format("{0}: {1}, {2}: {3}, {4}: {5}, {6}: {7}", new object[] { "Kind", this.Kind, "AlignAndSize", this.AlignAndSize, "Specifier", this.Specifier, "Uppercase", this.Uppercase });
			}

			// Token: 0x040000EA RID: 234
			public BurstString.NumberFormatKind Kind;

			// Token: 0x040000EB RID: 235
			public sbyte AlignAndSize;

			// Token: 0x040000EC RID: 236
			public byte Specifier;

			// Token: 0x040000ED RID: 237
			public bool Lowercase;
		}

		// Token: 0x02000024 RID: 36
		public struct tBigInt
		{
			// Token: 0x060000D1 RID: 209 RVA: 0x0000582D File Offset: 0x00003A2D
			public int GetLength()
			{
				return this.m_length;
			}

			// Token: 0x060000D2 RID: 210 RVA: 0x00005835 File Offset: 0x00003A35
			public unsafe uint GetBlock(int idx)
			{
				return *((ref this.m_blocks.FixedElementField) + (IntPtr)idx * 4);
			}

			// Token: 0x060000D3 RID: 211 RVA: 0x00005848 File Offset: 0x00003A48
			public void SetZero()
			{
				this.m_length = 0;
			}

			// Token: 0x060000D4 RID: 212 RVA: 0x00005851 File Offset: 0x00003A51
			public bool IsZero()
			{
				return this.m_length == 0;
			}

			// Token: 0x060000D5 RID: 213 RVA: 0x0000585C File Offset: 0x00003A5C
			public unsafe void SetU64(ulong val)
			{
				if (val > (ulong)(-1))
				{
					this.m_blocks.FixedElementField = (uint)(val & (ulong)(-1));
					*((ref this.m_blocks.FixedElementField) + 4) = (uint)((val >> 32) & (ulong)(-1));
					this.m_length = 2;
					return;
				}
				if (val != 0UL)
				{
					this.m_blocks.FixedElementField = (uint)(val & (ulong)(-1));
					this.m_length = 1;
					return;
				}
				this.m_length = 0;
			}

			// Token: 0x060000D6 RID: 214 RVA: 0x000058C0 File Offset: 0x00003AC0
			public void SetU32(uint val)
			{
				if (val != 0U)
				{
					this.m_blocks.FixedElementField = val;
					this.m_length = ((val != 0U) ? 1 : 0);
					return;
				}
				this.m_length = 0;
			}

			// Token: 0x060000D7 RID: 215 RVA: 0x000058E7 File Offset: 0x00003AE7
			public uint GetU32()
			{
				if (this.m_length != 0)
				{
					return this.m_blocks.FixedElementField;
				}
				return 0U;
			}

			// Token: 0x040000EE RID: 238
			private const int c_BigInt_MaxBlocks = 35;

			// Token: 0x040000EF RID: 239
			public int m_length;

			// Token: 0x040000F0 RID: 240
			[FixedBuffer(typeof(uint), 35)]
			public BurstString.tBigInt.<m_blocks>e__FixedBuffer m_blocks;

			// Token: 0x02000025 RID: 37
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 140)]
			public struct <m_blocks>e__FixedBuffer
			{
				// Token: 0x040000F1 RID: 241
				public uint FixedElementField;
			}
		}

		// Token: 0x02000026 RID: 38
		public enum CutoffMode
		{
			// Token: 0x040000F3 RID: 243
			Unique,
			// Token: 0x040000F4 RID: 244
			TotalLength,
			// Token: 0x040000F5 RID: 245
			FractionLength
		}

		// Token: 0x02000027 RID: 39
		public enum PrintFloatFormat
		{
			// Token: 0x040000F7 RID: 247
			Positional,
			// Token: 0x040000F8 RID: 248
			Scientific
		}

		// Token: 0x02000028 RID: 40
		[StructLayout(LayoutKind.Explicit)]
		public struct tFloatUnion32
		{
			// Token: 0x060000D8 RID: 216 RVA: 0x000058FF File Offset: 0x00003AFF
			public bool IsNegative()
			{
				return this.m_integer >> 31 > 0U;
			}

			// Token: 0x060000D9 RID: 217 RVA: 0x0000590D File Offset: 0x00003B0D
			public uint GetExponent()
			{
				return (this.m_integer >> 23) & 255U;
			}

			// Token: 0x060000DA RID: 218 RVA: 0x0000591E File Offset: 0x00003B1E
			public uint GetMantissa()
			{
				return this.m_integer & 8388607U;
			}

			// Token: 0x040000F9 RID: 249
			[FieldOffset(0)]
			public float m_floatingPoint;

			// Token: 0x040000FA RID: 250
			[FieldOffset(0)]
			public uint m_integer;
		}

		// Token: 0x02000029 RID: 41
		[StructLayout(LayoutKind.Explicit)]
		public struct tFloatUnion64
		{
			// Token: 0x060000DB RID: 219 RVA: 0x0000592C File Offset: 0x00003B2C
			public bool IsNegative()
			{
				return this.m_integer >> 63 > 0UL;
			}

			// Token: 0x060000DC RID: 220 RVA: 0x0000593B File Offset: 0x00003B3B
			public uint GetExponent()
			{
				return (uint)((this.m_integer >> 52) & 2047UL);
			}

			// Token: 0x060000DD RID: 221 RVA: 0x0000594E File Offset: 0x00003B4E
			public ulong GetMantissa()
			{
				return this.m_integer & 4503599627370495UL;
			}

			// Token: 0x040000FB RID: 251
			[FieldOffset(0)]
			public double m_floatingPoint;

			// Token: 0x040000FC RID: 252
			[FieldOffset(0)]
			public ulong m_integer;
		}
	}
}
