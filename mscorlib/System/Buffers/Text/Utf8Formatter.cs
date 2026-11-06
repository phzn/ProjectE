using System;
using System.Buffers.Binary;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Buffers.Text
{
	// Token: 0x02000AF6 RID: 2806
	public static class Utf8Formatter
	{
		// Token: 0x06006419 RID: 25625 RVA: 0x0014F468 File Offset: 0x0014D668
		public unsafe static bool TryFormat(bool value, Span<byte> destination, out int bytesWritten, StandardFormat format = default(StandardFormat))
		{
			char symbolOrDefault = FormattingHelpers.GetSymbolOrDefault(in format, 'G');
			if (value)
			{
				if (symbolOrDefault == 'G')
				{
					if (!BinaryPrimitives.TryWriteUInt32BigEndian(destination, 1416787301U))
					{
						goto IL_007E;
					}
				}
				else
				{
					if (symbolOrDefault != 'l')
					{
						goto IL_0083;
					}
					if (!BinaryPrimitives.TryWriteUInt32BigEndian(destination, 1953658213U))
					{
						goto IL_007E;
					}
				}
				bytesWritten = 4;
				return true;
			}
			if (symbolOrDefault == 'G')
			{
				if (4 >= destination.Length)
				{
					goto IL_007E;
				}
				BinaryPrimitives.WriteUInt32BigEndian(destination, 1180789875U);
			}
			else
			{
				if (symbolOrDefault != 'l')
				{
					goto IL_0083;
				}
				if (4 >= destination.Length)
				{
					goto IL_007E;
				}
				BinaryPrimitives.WriteUInt32BigEndian(destination, 1717660787U);
			}
			*destination[4] = 101;
			bytesWritten = 5;
			return true;
			IL_007E:
			bytesWritten = 0;
			return false;
			IL_0083:
			return ThrowHelper.TryFormatThrowFormatException(out bytesWritten);
		}

		// Token: 0x0600641A RID: 25626 RVA: 0x0014F500 File Offset: 0x0014D700
		private unsafe static bool TryFormatDateTimeG(DateTime value, TimeSpan offset, Span<byte> destination, out int bytesWritten)
		{
			int num = 19;
			if (offset != Utf8Constants.s_nullUtcOffset)
			{
				num += 7;
			}
			if (destination.Length < num)
			{
				bytesWritten = 0;
				return false;
			}
			bytesWritten = num;
			ref byte ptr = ref destination[18];
			FormattingHelpers.WriteTwoDecimalDigits((uint)value.Month, destination, 0);
			*destination[2] = 47;
			FormattingHelpers.WriteTwoDecimalDigits((uint)value.Day, destination, 3);
			*destination[5] = 47;
			FormattingHelpers.WriteFourDecimalDigits((uint)value.Year, destination, 6);
			*destination[10] = 32;
			FormattingHelpers.WriteTwoDecimalDigits((uint)value.Hour, destination, 11);
			*destination[13] = 58;
			FormattingHelpers.WriteTwoDecimalDigits((uint)value.Minute, destination, 14);
			*destination[16] = 58;
			FormattingHelpers.WriteTwoDecimalDigits((uint)value.Second, destination, 17);
			if (offset != Utf8Constants.s_nullUtcOffset)
			{
				byte b;
				if (offset < default(TimeSpan))
				{
					b = 45;
					offset = TimeSpan.FromTicks(-offset.Ticks);
				}
				else
				{
					b = 43;
				}
				FormattingHelpers.WriteTwoDecimalDigits((uint)offset.Minutes, destination, 24);
				*destination[23] = 58;
				FormattingHelpers.WriteTwoDecimalDigits((uint)offset.Hours, destination, 21);
				*destination[20] = b;
				*destination[19] = 32;
			}
			return true;
		}

		// Token: 0x0600641B RID: 25627 RVA: 0x0014F648 File Offset: 0x0014D848
		private unsafe static bool TryFormatDateTimeL(DateTime value, Span<byte> destination, out int bytesWritten)
		{
			if (28 >= destination.Length)
			{
				bytesWritten = 0;
				return false;
			}
			uint num = Utf8Formatter.DayAbbreviationsLowercase[(int)value.DayOfWeek];
			*destination[0] = (byte)num;
			num >>= 8;
			*destination[1] = (byte)num;
			num >>= 8;
			*destination[2] = (byte)num;
			*destination[3] = 44;
			*destination[4] = 32;
			FormattingHelpers.WriteTwoDecimalDigits((uint)value.Day, destination, 5);
			*destination[7] = 32;
			uint num2 = Utf8Formatter.MonthAbbreviationsLowercase[value.Month - 1];
			*destination[8] = (byte)num2;
			num2 >>= 8;
			*destination[9] = (byte)num2;
			num2 >>= 8;
			*destination[10] = (byte)num2;
			*destination[11] = 32;
			FormattingHelpers.WriteFourDecimalDigits((uint)value.Year, destination, 12);
			*destination[16] = 32;
			FormattingHelpers.WriteTwoDecimalDigits((uint)value.Hour, destination, 17);
			*destination[19] = 58;
			FormattingHelpers.WriteTwoDecimalDigits((uint)value.Minute, destination, 20);
			*destination[22] = 58;
			FormattingHelpers.WriteTwoDecimalDigits((uint)value.Second, destination, 23);
			*destination[25] = 32;
			*destination[26] = 103;
			*destination[27] = 109;
			*destination[28] = 116;
			bytesWritten = 29;
			return true;
		}

		// Token: 0x0600641C RID: 25628 RVA: 0x0014F7A8 File Offset: 0x0014D9A8
		private unsafe static bool TryFormatDateTimeO(DateTime value, TimeSpan offset, Span<byte> destination, out int bytesWritten)
		{
			int num = 27;
			DateTimeKind dateTimeKind = DateTimeKind.Local;
			if (offset == Utf8Constants.s_nullUtcOffset)
			{
				dateTimeKind = value.Kind;
				if (dateTimeKind == DateTimeKind.Local)
				{
					offset = TimeZoneInfo.Local.GetUtcOffset(value);
					num += 6;
				}
				else if (dateTimeKind == DateTimeKind.Utc)
				{
					num++;
				}
			}
			else
			{
				num += 6;
			}
			if (destination.Length < num)
			{
				bytesWritten = 0;
				return false;
			}
			bytesWritten = num;
			ref byte ptr = ref destination[26];
			FormattingHelpers.WriteFourDecimalDigits((uint)value.Year, destination, 0);
			*destination[4] = 45;
			FormattingHelpers.WriteTwoDecimalDigits((uint)value.Month, destination, 5);
			*destination[7] = 45;
			FormattingHelpers.WriteTwoDecimalDigits((uint)value.Day, destination, 8);
			*destination[10] = 84;
			FormattingHelpers.WriteTwoDecimalDigits((uint)value.Hour, destination, 11);
			*destination[13] = 58;
			FormattingHelpers.WriteTwoDecimalDigits((uint)value.Minute, destination, 14);
			*destination[16] = 58;
			FormattingHelpers.WriteTwoDecimalDigits((uint)value.Second, destination, 17);
			*destination[19] = 46;
			FormattingHelpers.WriteDigits((uint)(value.Ticks % 10000000L), destination.Slice(20, 7));
			if (dateTimeKind == DateTimeKind.Local)
			{
				byte b;
				if (offset < default(TimeSpan))
				{
					b = 45;
					offset = TimeSpan.FromTicks(-offset.Ticks);
				}
				else
				{
					b = 43;
				}
				FormattingHelpers.WriteTwoDecimalDigits((uint)offset.Minutes, destination, 31);
				*destination[30] = 58;
				FormattingHelpers.WriteTwoDecimalDigits((uint)offset.Hours, destination, 28);
				*destination[27] = b;
			}
			else if (dateTimeKind == DateTimeKind.Utc)
			{
				*destination[27] = 90;
			}
			return true;
		}

		// Token: 0x0600641D RID: 25629 RVA: 0x0014F940 File Offset: 0x0014DB40
		private unsafe static bool TryFormatDateTimeR(DateTime value, Span<byte> destination, out int bytesWritten)
		{
			if (28 >= destination.Length)
			{
				bytesWritten = 0;
				return false;
			}
			uint num = Utf8Formatter.DayAbbreviations[(int)value.DayOfWeek];
			*destination[0] = (byte)num;
			num >>= 8;
			*destination[1] = (byte)num;
			num >>= 8;
			*destination[2] = (byte)num;
			*destination[3] = 44;
			*destination[4] = 32;
			FormattingHelpers.WriteTwoDecimalDigits((uint)value.Day, destination, 5);
			*destination[7] = 32;
			uint num2 = Utf8Formatter.MonthAbbreviations[value.Month - 1];
			*destination[8] = (byte)num2;
			num2 >>= 8;
			*destination[9] = (byte)num2;
			num2 >>= 8;
			*destination[10] = (byte)num2;
			*destination[11] = 32;
			FormattingHelpers.WriteFourDecimalDigits((uint)value.Year, destination, 12);
			*destination[16] = 32;
			FormattingHelpers.WriteTwoDecimalDigits((uint)value.Hour, destination, 17);
			*destination[19] = 58;
			FormattingHelpers.WriteTwoDecimalDigits((uint)value.Minute, destination, 20);
			*destination[22] = 58;
			FormattingHelpers.WriteTwoDecimalDigits((uint)value.Second, destination, 23);
			*destination[25] = 32;
			*destination[26] = 71;
			*destination[27] = 77;
			*destination[28] = 84;
			bytesWritten = 29;
			return true;
		}

		// Token: 0x0600641E RID: 25630 RVA: 0x0014FAA0 File Offset: 0x0014DCA0
		public static bool TryFormat(DateTimeOffset value, Span<byte> destination, out int bytesWritten, StandardFormat format = default(StandardFormat))
		{
			TimeSpan timeSpan = Utf8Constants.s_nullUtcOffset;
			char c = format.Symbol;
			if (format.IsDefault)
			{
				c = 'G';
				timeSpan = value.Offset;
			}
			if (c <= 'O')
			{
				if (c == 'G')
				{
					return Utf8Formatter.TryFormatDateTimeG(value.DateTime, timeSpan, destination, out bytesWritten);
				}
				if (c == 'O')
				{
					return Utf8Formatter.TryFormatDateTimeO(value.DateTime, value.Offset, destination, out bytesWritten);
				}
			}
			else
			{
				if (c == 'R')
				{
					return Utf8Formatter.TryFormatDateTimeR(value.UtcDateTime, destination, out bytesWritten);
				}
				if (c == 'l')
				{
					return Utf8Formatter.TryFormatDateTimeL(value.UtcDateTime, destination, out bytesWritten);
				}
			}
			return ThrowHelper.TryFormatThrowFormatException(out bytesWritten);
		}

		// Token: 0x0600641F RID: 25631 RVA: 0x0014FB38 File Offset: 0x0014DD38
		public static bool TryFormat(DateTime value, Span<byte> destination, out int bytesWritten, StandardFormat format = default(StandardFormat))
		{
			char symbolOrDefault = FormattingHelpers.GetSymbolOrDefault(in format, 'G');
			if (symbolOrDefault <= 'O')
			{
				if (symbolOrDefault == 'G')
				{
					return Utf8Formatter.TryFormatDateTimeG(value, Utf8Constants.s_nullUtcOffset, destination, out bytesWritten);
				}
				if (symbolOrDefault == 'O')
				{
					return Utf8Formatter.TryFormatDateTimeO(value, Utf8Constants.s_nullUtcOffset, destination, out bytesWritten);
				}
			}
			else
			{
				if (symbolOrDefault == 'R')
				{
					return Utf8Formatter.TryFormatDateTimeR(value, destination, out bytesWritten);
				}
				if (symbolOrDefault == 'l')
				{
					return Utf8Formatter.TryFormatDateTimeL(value, destination, out bytesWritten);
				}
			}
			return ThrowHelper.TryFormatThrowFormatException(out bytesWritten);
		}

		// Token: 0x06006420 RID: 25632 RVA: 0x0014FBA0 File Offset: 0x0014DDA0
		private unsafe static bool TryFormatDecimalE(ref NumberBuffer number, Span<byte> destination, out int bytesWritten, byte precision, byte exponentSymbol)
		{
			int scale = number.Scale;
			ReadOnlySpan<byte> readOnlySpan = number.Digits;
			int num = (int)((number.IsNegative ? 1 : 0) + 1 + ((precision == 0) ? 0 : (precision + 1)) + 2 + 3);
			if (destination.Length < num)
			{
				bytesWritten = 0;
				return false;
			}
			int num2 = 0;
			int num3 = 0;
			if (number.IsNegative)
			{
				*destination[num2++] = 45;
			}
			byte b = *readOnlySpan[num3];
			int num4;
			if (b == 0)
			{
				*destination[num2++] = 48;
				num4 = 0;
			}
			else
			{
				*destination[num2++] = b;
				num3++;
				num4 = scale - 1;
			}
			if (precision > 0)
			{
				*destination[num2++] = 46;
				for (int i = 0; i < (int)precision; i++)
				{
					byte b2 = *readOnlySpan[num3];
					if (b2 == 0)
					{
						while (i++ < (int)precision)
						{
							*destination[num2++] = 48;
						}
						break;
					}
					*destination[num2++] = b2;
					num3++;
				}
			}
			*destination[num2++] = exponentSymbol;
			if (num4 >= 0)
			{
				*destination[num2++] = 43;
			}
			else
			{
				*destination[num2++] = 45;
				num4 = -num4;
			}
			*destination[num2++] = 48;
			*destination[num2++] = (byte)(num4 / 10 + 48);
			*destination[num2++] = (byte)(num4 % 10 + 48);
			bytesWritten = num;
			return true;
		}

		// Token: 0x06006421 RID: 25633 RVA: 0x0014FD24 File Offset: 0x0014DF24
		private unsafe static bool TryFormatDecimalF(ref NumberBuffer number, Span<byte> destination, out int bytesWritten, byte precision)
		{
			int scale = number.Scale;
			ReadOnlySpan<byte> readOnlySpan = number.Digits;
			int num = (number.IsNegative ? 1 : 0) + ((scale <= 0) ? 1 : scale) + (int)((precision == 0) ? 0 : (precision + 1));
			if (destination.Length < num)
			{
				bytesWritten = 0;
				return false;
			}
			int i = 0;
			int num2 = 0;
			if (number.IsNegative)
			{
				*destination[num2++] = 45;
			}
			if (scale <= 0)
			{
				*destination[num2++] = 48;
			}
			else
			{
				while (i < scale)
				{
					byte b = *readOnlySpan[i];
					if (b == 0)
					{
						int num3 = scale - i;
						for (int j = 0; j < num3; j++)
						{
							*destination[num2++] = 48;
						}
						break;
					}
					*destination[num2++] = b;
					i++;
				}
			}
			if (precision > 0)
			{
				*destination[num2++] = 46;
				int k = 0;
				if (scale < 0)
				{
					int num4 = Math.Min((int)precision, -scale);
					for (int l = 0; l < num4; l++)
					{
						*destination[num2++] = 48;
					}
					k += num4;
				}
				while (k < (int)precision)
				{
					byte b2 = *readOnlySpan[i];
					if (b2 == 0)
					{
						while (k++ < (int)precision)
						{
							*destination[num2++] = 48;
						}
						break;
					}
					*destination[num2++] = b2;
					i++;
					k++;
				}
			}
			bytesWritten = num;
			return true;
		}

		// Token: 0x06006422 RID: 25634 RVA: 0x0014FEA0 File Offset: 0x0014E0A0
		private unsafe static bool TryFormatDecimalG(ref NumberBuffer number, Span<byte> destination, out int bytesWritten)
		{
			int scale = number.Scale;
			ReadOnlySpan<byte> readOnlySpan = number.Digits;
			int numDigits = number.NumDigits;
			bool flag = scale < numDigits;
			int num;
			if (flag)
			{
				num = numDigits + 1;
				if (scale <= 0)
				{
					num += 1 + -scale;
				}
			}
			else
			{
				num = ((scale <= 0) ? 1 : scale);
			}
			if (number.IsNegative)
			{
				num++;
			}
			if (destination.Length < num)
			{
				bytesWritten = 0;
				return false;
			}
			int i = 0;
			int num2 = 0;
			if (number.IsNegative)
			{
				*destination[num2++] = 45;
			}
			if (scale <= 0)
			{
				*destination[num2++] = 48;
			}
			else
			{
				while (i < scale)
				{
					byte b = *readOnlySpan[i];
					if (b == 0)
					{
						int num3 = scale - i;
						for (int j = 0; j < num3; j++)
						{
							*destination[num2++] = 48;
						}
						break;
					}
					*destination[num2++] = b;
					i++;
				}
			}
			if (flag)
			{
				*destination[num2++] = 46;
				if (scale < 0)
				{
					int num4 = -scale;
					for (int k = 0; k < num4; k++)
					{
						*destination[num2++] = 48;
					}
				}
				byte b2;
				while ((b2 = *readOnlySpan[i++]) != 0)
				{
					*destination[num2++] = b2;
				}
			}
			bytesWritten = num;
			return true;
		}

		// Token: 0x06006423 RID: 25635 RVA: 0x00150004 File Offset: 0x0014E204
		public unsafe static bool TryFormat(decimal value, Span<byte> destination, out int bytesWritten, StandardFormat format = default(StandardFormat))
		{
			if (format.IsDefault)
			{
				format = 'G';
			}
			char symbol = format.Symbol;
			switch (symbol)
			{
			case 'E':
				goto IL_00DD;
			case 'F':
				goto IL_0097;
			case 'G':
				break;
			default:
				switch (symbol)
				{
				case 'e':
					goto IL_00DD;
				case 'f':
					goto IL_0097;
				case 'g':
					break;
				default:
					return ThrowHelper.TryFormatThrowFormatException(out bytesWritten);
				}
				break;
			}
			if (format.Precision != 255)
			{
				throw new NotSupportedException("The 'G' format combined with a precision is not supported.");
			}
			NumberBuffer numberBuffer = default(NumberBuffer);
			Number.DecimalToNumber(value, ref numberBuffer);
			if (*numberBuffer.Digits[0] == 0)
			{
				numberBuffer.IsNegative = false;
			}
			return Utf8Formatter.TryFormatDecimalG(ref numberBuffer, destination, out bytesWritten);
			IL_0097:
			NumberBuffer numberBuffer2 = default(NumberBuffer);
			Number.DecimalToNumber(value, ref numberBuffer2);
			byte b = ((format.Precision == byte.MaxValue) ? 2 : format.Precision);
			Number.RoundNumber(ref numberBuffer2, numberBuffer2.Scale + (int)b);
			return Utf8Formatter.TryFormatDecimalF(ref numberBuffer2, destination, out bytesWritten, b);
			IL_00DD:
			NumberBuffer numberBuffer3 = default(NumberBuffer);
			Number.DecimalToNumber(value, ref numberBuffer3);
			byte b2 = ((format.Precision == byte.MaxValue) ? 6 : format.Precision);
			Number.RoundNumber(ref numberBuffer3, (int)(b2 + 1));
			return Utf8Formatter.TryFormatDecimalE(ref numberBuffer3, destination, out bytesWritten, b2, (byte)format.Symbol);
		}

		// Token: 0x06006424 RID: 25636 RVA: 0x0015013D File Offset: 0x0014E33D
		public static bool TryFormat(double value, Span<byte> destination, out int bytesWritten, StandardFormat format = default(StandardFormat))
		{
			return Utf8Formatter.TryFormatFloatingPoint<double>(value, destination, out bytesWritten, format);
		}

		// Token: 0x06006425 RID: 25637 RVA: 0x00150148 File Offset: 0x0014E348
		public static bool TryFormat(float value, Span<byte> destination, out int bytesWritten, StandardFormat format = default(StandardFormat))
		{
			return Utf8Formatter.TryFormatFloatingPoint<float>(value, destination, out bytesWritten, format);
		}

		// Token: 0x06006426 RID: 25638 RVA: 0x00150154 File Offset: 0x0014E354
		private unsafe static bool TryFormatFloatingPoint<T>(T value, Span<byte> destination, out int bytesWritten, StandardFormat format) where T : IFormattable
		{
			if (format.IsDefault)
			{
				format = 'G';
			}
			char symbol = format.Symbol;
			switch (symbol)
			{
			case 'E':
			case 'F':
				goto IL_0066;
			case 'G':
				break;
			default:
				switch (symbol)
				{
				case 'e':
				case 'f':
					goto IL_0066;
				case 'g':
					break;
				default:
					return ThrowHelper.TryFormatThrowFormatException(out bytesWritten);
				}
				break;
			}
			if (format.Precision != 255)
			{
				throw new NotSupportedException("The 'G' format combined with a precision is not supported.");
			}
			IL_0066:
			string text = format.ToString();
			string text2 = value.ToString(text, CultureInfo.InvariantCulture);
			int length = text2.Length;
			if (length > destination.Length)
			{
				bytesWritten = 0;
				return false;
			}
			for (int i = 0; i < length; i++)
			{
				*destination[i] = (byte)text2[i];
			}
			bytesWritten = length;
			return true;
		}

		// Token: 0x06006427 RID: 25639 RVA: 0x00150228 File Offset: 0x0014E428
		public unsafe static bool TryFormat(Guid value, Span<byte> destination, out int bytesWritten, StandardFormat format = default(StandardFormat))
		{
			char symbolOrDefault = FormattingHelpers.GetSymbolOrDefault(in format, 'D');
			int num;
			if (symbolOrDefault <= 'D')
			{
				if (symbolOrDefault == 'B')
				{
					num = -2139260122;
					goto IL_004B;
				}
				if (symbolOrDefault == 'D')
				{
					num = -2147483612;
					goto IL_004B;
				}
			}
			else
			{
				if (symbolOrDefault == 'N')
				{
					num = 32;
					goto IL_004B;
				}
				if (symbolOrDefault == 'P')
				{
					num = -2144786394;
					goto IL_004B;
				}
			}
			return ThrowHelper.TryFormatThrowFormatException(out bytesWritten);
			IL_004B:
			if ((int)((byte)num) > destination.Length)
			{
				bytesWritten = 0;
				return false;
			}
			bytesWritten = (int)((byte)num);
			num >>= 8;
			if ((byte)num != 0)
			{
				*destination[0] = (byte)num;
				destination = destination.Slice(1);
			}
			num >>= 8;
			Utf8Formatter.DecomposedGuid decomposedGuid = default(Utf8Formatter.DecomposedGuid);
			decomposedGuid.Guid = value;
			ref byte ptr = ref destination[8];
			FormattingHelpers.WriteHexByte(decomposedGuid.Byte03, destination, 0, FormattingHelpers.HexCasing.Lowercase);
			FormattingHelpers.WriteHexByte(decomposedGuid.Byte02, destination, 2, FormattingHelpers.HexCasing.Lowercase);
			FormattingHelpers.WriteHexByte(decomposedGuid.Byte01, destination, 4, FormattingHelpers.HexCasing.Lowercase);
			FormattingHelpers.WriteHexByte(decomposedGuid.Byte00, destination, 6, FormattingHelpers.HexCasing.Lowercase);
			if (num < 0)
			{
				*destination[8] = 45;
				destination = destination.Slice(9);
			}
			else
			{
				destination = destination.Slice(8);
			}
			ref byte ptr2 = ref destination[4];
			FormattingHelpers.WriteHexByte(decomposedGuid.Byte05, destination, 0, FormattingHelpers.HexCasing.Lowercase);
			FormattingHelpers.WriteHexByte(decomposedGuid.Byte04, destination, 2, FormattingHelpers.HexCasing.Lowercase);
			if (num < 0)
			{
				*destination[4] = 45;
				destination = destination.Slice(5);
			}
			else
			{
				destination = destination.Slice(4);
			}
			ref byte ptr3 = ref destination[4];
			FormattingHelpers.WriteHexByte(decomposedGuid.Byte07, destination, 0, FormattingHelpers.HexCasing.Lowercase);
			FormattingHelpers.WriteHexByte(decomposedGuid.Byte06, destination, 2, FormattingHelpers.HexCasing.Lowercase);
			if (num < 0)
			{
				*destination[4] = 45;
				destination = destination.Slice(5);
			}
			else
			{
				destination = destination.Slice(4);
			}
			ref byte ptr4 = ref destination[4];
			FormattingHelpers.WriteHexByte(decomposedGuid.Byte08, destination, 0, FormattingHelpers.HexCasing.Lowercase);
			FormattingHelpers.WriteHexByte(decomposedGuid.Byte09, destination, 2, FormattingHelpers.HexCasing.Lowercase);
			if (num < 0)
			{
				*destination[4] = 45;
				destination = destination.Slice(5);
			}
			else
			{
				destination = destination.Slice(4);
			}
			ref byte ptr5 = ref destination[11];
			FormattingHelpers.WriteHexByte(decomposedGuid.Byte10, destination, 0, FormattingHelpers.HexCasing.Lowercase);
			FormattingHelpers.WriteHexByte(decomposedGuid.Byte11, destination, 2, FormattingHelpers.HexCasing.Lowercase);
			FormattingHelpers.WriteHexByte(decomposedGuid.Byte12, destination, 4, FormattingHelpers.HexCasing.Lowercase);
			FormattingHelpers.WriteHexByte(decomposedGuid.Byte13, destination, 6, FormattingHelpers.HexCasing.Lowercase);
			FormattingHelpers.WriteHexByte(decomposedGuid.Byte14, destination, 8, FormattingHelpers.HexCasing.Lowercase);
			FormattingHelpers.WriteHexByte(decomposedGuid.Byte15, destination, 10, FormattingHelpers.HexCasing.Lowercase);
			if ((byte)num != 0)
			{
				*destination[12] = (byte)num;
			}
			return true;
		}

		// Token: 0x06006428 RID: 25640 RVA: 0x001504BC File Offset: 0x0014E6BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool TryFormatInt64D(long value, byte precision, Span<byte> destination, out int bytesWritten)
		{
			bool flag = false;
			if (value < 0L)
			{
				flag = true;
				value = -value;
			}
			return Utf8Formatter.TryFormatUInt64D((ulong)value, precision, destination, flag, out bytesWritten);
		}

		// Token: 0x06006429 RID: 25641 RVA: 0x001504E0 File Offset: 0x0014E6E0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool TryFormatInt64Default(long value, Span<byte> destination, out int bytesWritten)
		{
			if (value < 10L)
			{
				return Utf8Formatter.TryFormatUInt32SingleDigit((uint)value, destination, out bytesWritten);
			}
			if (IntPtr.Size == 8)
			{
				return Utf8Formatter.TryFormatInt64MultipleDigits(value, destination, out bytesWritten);
			}
			if (value <= 2147483647L && value >= -2147483648L)
			{
				return Utf8Formatter.TryFormatInt32MultipleDigits((int)value, destination, out bytesWritten);
			}
			if (value <= 4294967295000000000L && value >= -4294967295000000000L)
			{
				if (value >= 0L)
				{
					return Utf8Formatter.TryFormatUInt64LessThanBillionMaxUInt((ulong)value, destination, out bytesWritten);
				}
				return Utf8Formatter.TryFormatInt64MoreThanNegativeBillionMaxUInt(-value, destination, out bytesWritten);
			}
			else
			{
				if (value >= 0L)
				{
					return Utf8Formatter.TryFormatUInt64MoreThanBillionMaxUInt((ulong)value, destination, out bytesWritten);
				}
				return Utf8Formatter.TryFormatInt64LessThanNegativeBillionMaxUInt(-value, destination, out bytesWritten);
			}
		}

		// Token: 0x0600642A RID: 25642 RVA: 0x00150571 File Offset: 0x0014E771
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool TryFormatInt32Default(int value, Span<byte> destination, out int bytesWritten)
		{
			if (value < 10)
			{
				return Utf8Formatter.TryFormatUInt32SingleDigit((uint)value, destination, out bytesWritten);
			}
			return Utf8Formatter.TryFormatInt32MultipleDigits(value, destination, out bytesWritten);
		}

		// Token: 0x0600642B RID: 25643 RVA: 0x0015058C File Offset: 0x0014E78C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static bool TryFormatInt32MultipleDigits(int value, Span<byte> destination, out int bytesWritten)
		{
			if (value >= 0)
			{
				return Utf8Formatter.TryFormatUInt32MultipleDigits((uint)value, destination, out bytesWritten);
			}
			value = -value;
			int num = FormattingHelpers.CountDigits((uint)value);
			if (num >= destination.Length)
			{
				bytesWritten = 0;
				return false;
			}
			*destination[0] = 45;
			bytesWritten = num + 1;
			FormattingHelpers.WriteDigits((uint)value, destination.Slice(1, num));
			return true;
		}

		// Token: 0x0600642C RID: 25644 RVA: 0x001505E0 File Offset: 0x0014E7E0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static bool TryFormatInt64MultipleDigits(long value, Span<byte> destination, out int bytesWritten)
		{
			if (value >= 0L)
			{
				return Utf8Formatter.TryFormatUInt64MultipleDigits((ulong)value, destination, out bytesWritten);
			}
			value = -value;
			int num = FormattingHelpers.CountDigits((ulong)value);
			if (num >= destination.Length)
			{
				bytesWritten = 0;
				return false;
			}
			*destination[0] = 45;
			bytesWritten = num + 1;
			FormattingHelpers.WriteDigits((ulong)value, destination.Slice(1, num));
			return true;
		}

		// Token: 0x0600642D RID: 25645 RVA: 0x00150638 File Offset: 0x0014E838
		private unsafe static bool TryFormatInt64MoreThanNegativeBillionMaxUInt(long value, Span<byte> destination, out int bytesWritten)
		{
			uint num = (uint)(value / 1000000000L);
			uint num2 = (uint)(value - (long)((ulong)(num * 1000000000U)));
			int num3 = FormattingHelpers.CountDigits(num);
			int num4 = num3 + 9;
			if (num4 >= destination.Length)
			{
				bytesWritten = 0;
				return false;
			}
			*destination[0] = 45;
			bytesWritten = num4 + 1;
			FormattingHelpers.WriteDigits(num, destination.Slice(1, num3));
			FormattingHelpers.WriteDigits(num2, destination.Slice(num3 + 1, 9));
			return true;
		}

		// Token: 0x0600642E RID: 25646 RVA: 0x001506A8 File Offset: 0x0014E8A8
		private unsafe static bool TryFormatInt64LessThanNegativeBillionMaxUInt(long value, Span<byte> destination, out int bytesWritten)
		{
			ulong num = (ulong)(value / 1000000000L);
			uint num2 = (uint)(value - (long)(num * 1000000000UL));
			uint num3 = (uint)(num / 1000000000UL);
			uint num4 = (uint)(num - (ulong)(num3 * 1000000000U));
			int num5 = FormattingHelpers.CountDigits(num3);
			int num6 = num5 + 18;
			if (num6 >= destination.Length)
			{
				bytesWritten = 0;
				return false;
			}
			*destination[0] = 45;
			bytesWritten = num6 + 1;
			FormattingHelpers.WriteDigits(num3, destination.Slice(1, num5));
			FormattingHelpers.WriteDigits(num4, destination.Slice(num5 + 1, 9));
			FormattingHelpers.WriteDigits(num2, destination.Slice(num5 + 1 + 9, 9));
			return true;
		}

		// Token: 0x0600642F RID: 25647 RVA: 0x0015074C File Offset: 0x0014E94C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool TryFormatInt64N(long value, byte precision, Span<byte> destination, out int bytesWritten)
		{
			bool flag = false;
			if (value < 0L)
			{
				flag = true;
				value = -value;
			}
			return Utf8Formatter.TryFormatUInt64N((ulong)value, precision, destination, flag, out bytesWritten);
		}

		// Token: 0x06006430 RID: 25648 RVA: 0x00150770 File Offset: 0x0014E970
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool TryFormatInt64(long value, ulong mask, Span<byte> destination, out int bytesWritten, StandardFormat format)
		{
			if (format.IsDefault)
			{
				return Utf8Formatter.TryFormatInt64Default(value, destination, out bytesWritten);
			}
			char symbol = format.Symbol;
			if (symbol <= 'X')
			{
				if (symbol <= 'G')
				{
					if (symbol == 'D')
					{
						goto IL_0083;
					}
					if (symbol != 'G')
					{
						goto IL_00C9;
					}
				}
				else
				{
					if (symbol == 'N')
					{
						goto IL_0093;
					}
					if (symbol != 'X')
					{
						goto IL_00C9;
					}
					return Utf8Formatter.TryFormatUInt64X((ulong)(value & (long)mask), format.Precision, false, destination, out bytesWritten);
				}
			}
			else if (symbol <= 'g')
			{
				if (symbol == 'd')
				{
					goto IL_0083;
				}
				if (symbol != 'g')
				{
					goto IL_00C9;
				}
			}
			else
			{
				if (symbol == 'n')
				{
					goto IL_0093;
				}
				if (symbol != 'x')
				{
					goto IL_00C9;
				}
				return Utf8Formatter.TryFormatUInt64X((ulong)(value & (long)mask), format.Precision, true, destination, out bytesWritten);
			}
			if (format.HasPrecision)
			{
				throw new NotSupportedException("The 'G' format combined with a precision is not supported.");
			}
			return Utf8Formatter.TryFormatInt64D(value, format.Precision, destination, out bytesWritten);
			IL_0083:
			return Utf8Formatter.TryFormatInt64D(value, format.Precision, destination, out bytesWritten);
			IL_0093:
			return Utf8Formatter.TryFormatInt64N(value, format.Precision, destination, out bytesWritten);
			IL_00C9:
			return ThrowHelper.TryFormatThrowFormatException(out bytesWritten);
		}

		// Token: 0x06006431 RID: 25649 RVA: 0x0015084C File Offset: 0x0014EA4C
		private unsafe static bool TryFormatUInt64D(ulong value, byte precision, Span<byte> destination, bool insertNegationSign, out int bytesWritten)
		{
			int num = FormattingHelpers.CountDigits(value);
			int num2 = (int)((precision == byte.MaxValue) ? 0 : precision) - num;
			if (num2 < 0)
			{
				num2 = 0;
			}
			int num3 = num + num2;
			if (insertNegationSign)
			{
				num3++;
			}
			if (num3 > destination.Length)
			{
				bytesWritten = 0;
				return false;
			}
			bytesWritten = num3;
			if (insertNegationSign)
			{
				*destination[0] = 45;
				destination = destination.Slice(1);
			}
			if (num2 > 0)
			{
				FormattingHelpers.FillWithAsciiZeros(destination.Slice(0, num2));
			}
			FormattingHelpers.WriteDigits(value, destination.Slice(num2, num));
			return true;
		}

		// Token: 0x06006432 RID: 25650 RVA: 0x001508D0 File Offset: 0x0014EAD0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool TryFormatUInt64Default(ulong value, Span<byte> destination, out int bytesWritten)
		{
			if (value < 10UL)
			{
				return Utf8Formatter.TryFormatUInt32SingleDigit((uint)value, destination, out bytesWritten);
			}
			if (IntPtr.Size == 8)
			{
				return Utf8Formatter.TryFormatUInt64MultipleDigits(value, destination, out bytesWritten);
			}
			if (value <= (ulong)(-1))
			{
				return Utf8Formatter.TryFormatUInt32MultipleDigits((uint)value, destination, out bytesWritten);
			}
			if (value <= 4294967295000000000UL)
			{
				return Utf8Formatter.TryFormatUInt64LessThanBillionMaxUInt(value, destination, out bytesWritten);
			}
			return Utf8Formatter.TryFormatUInt64MoreThanBillionMaxUInt(value, destination, out bytesWritten);
		}

		// Token: 0x06006433 RID: 25651 RVA: 0x0015092A File Offset: 0x0014EB2A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool TryFormatUInt32Default(uint value, Span<byte> destination, out int bytesWritten)
		{
			if (value < 10U)
			{
				return Utf8Formatter.TryFormatUInt32SingleDigit(value, destination, out bytesWritten);
			}
			return Utf8Formatter.TryFormatUInt32MultipleDigits(value, destination, out bytesWritten);
		}

		// Token: 0x06006434 RID: 25652 RVA: 0x00150942 File Offset: 0x0014EB42
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static bool TryFormatUInt32SingleDigit(uint value, Span<byte> destination, out int bytesWritten)
		{
			if (destination.Length == 0)
			{
				bytesWritten = 0;
				return false;
			}
			*destination[0] = (byte)(48U + value);
			bytesWritten = 1;
			return true;
		}

		// Token: 0x06006435 RID: 25653 RVA: 0x00150964 File Offset: 0x0014EB64
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool TryFormatUInt32MultipleDigits(uint value, Span<byte> destination, out int bytesWritten)
		{
			int num = FormattingHelpers.CountDigits(value);
			if (num > destination.Length)
			{
				bytesWritten = 0;
				return false;
			}
			bytesWritten = num;
			FormattingHelpers.WriteDigits(value, destination.Slice(0, num));
			return true;
		}

		// Token: 0x06006436 RID: 25654 RVA: 0x0015099A File Offset: 0x0014EB9A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static bool TryFormatUInt64SingleDigit(ulong value, Span<byte> destination, out int bytesWritten)
		{
			if (destination.Length == 0)
			{
				bytesWritten = 0;
				return false;
			}
			*destination[0] = (byte)(48UL + value);
			bytesWritten = 1;
			return true;
		}

		// Token: 0x06006437 RID: 25655 RVA: 0x001509C0 File Offset: 0x0014EBC0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool TryFormatUInt64MultipleDigits(ulong value, Span<byte> destination, out int bytesWritten)
		{
			int num = FormattingHelpers.CountDigits(value);
			if (num > destination.Length)
			{
				bytesWritten = 0;
				return false;
			}
			bytesWritten = num;
			FormattingHelpers.WriteDigits(value, destination.Slice(0, num));
			return true;
		}

		// Token: 0x06006438 RID: 25656 RVA: 0x001509F8 File Offset: 0x0014EBF8
		private static bool TryFormatUInt64LessThanBillionMaxUInt(ulong value, Span<byte> destination, out int bytesWritten)
		{
			uint num = (uint)(value / 1000000000UL);
			uint num2 = (uint)(value - (ulong)(num * 1000000000U));
			int num3 = FormattingHelpers.CountDigits(num);
			int num4 = num3 + 9;
			if (num4 > destination.Length)
			{
				bytesWritten = 0;
				return false;
			}
			bytesWritten = num4;
			FormattingHelpers.WriteDigits(num, destination.Slice(0, num3));
			FormattingHelpers.WriteDigits(num2, destination.Slice(num3, 9));
			return true;
		}

		// Token: 0x06006439 RID: 25657 RVA: 0x00150A5C File Offset: 0x0014EC5C
		private static bool TryFormatUInt64MoreThanBillionMaxUInt(ulong value, Span<byte> destination, out int bytesWritten)
		{
			ulong num = value / 1000000000UL;
			uint num2 = (uint)(value - num * 1000000000UL);
			uint num3 = (uint)(num / 1000000000UL);
			uint num4 = (uint)(num - (ulong)(num3 * 1000000000U));
			int num5 = FormattingHelpers.CountDigits(num3);
			int num6 = num5 + 18;
			if (num6 > destination.Length)
			{
				bytesWritten = 0;
				return false;
			}
			bytesWritten = num6;
			FormattingHelpers.WriteDigits(num3, destination.Slice(0, num5));
			FormattingHelpers.WriteDigits(num4, destination.Slice(num5, 9));
			FormattingHelpers.WriteDigits(num2, destination.Slice(num5 + 9, 9));
			return true;
		}

		// Token: 0x0600643A RID: 25658 RVA: 0x00150AF0 File Offset: 0x0014ECF0
		private unsafe static bool TryFormatUInt64N(ulong value, byte precision, Span<byte> destination, bool insertNegationSign, out int bytesWritten)
		{
			int num = FormattingHelpers.CountDigits(value);
			int num2 = (num - 1) / 3;
			int num3 = (int)((precision == byte.MaxValue) ? 2 : precision);
			int num4 = num + num2;
			if (num3 > 0)
			{
				num4 += num3 + 1;
			}
			if (insertNegationSign)
			{
				num4++;
			}
			if (num4 > destination.Length)
			{
				bytesWritten = 0;
				return false;
			}
			bytesWritten = num4;
			if (insertNegationSign)
			{
				*destination[0] = 45;
				destination = destination.Slice(1);
			}
			FormattingHelpers.WriteDigitsWithGroupSeparator(value, destination.Slice(0, num + num2));
			if (num3 > 0)
			{
				*destination[num + num2] = 46;
				FormattingHelpers.FillWithAsciiZeros(destination.Slice(num + num2 + 1, num3));
			}
			return true;
		}

		// Token: 0x0600643B RID: 25659 RVA: 0x00150B90 File Offset: 0x0014ED90
		private unsafe static bool TryFormatUInt64X(ulong value, byte precision, bool useLower, Span<byte> destination, out int bytesWritten)
		{
			int num = FormattingHelpers.CountHexDigits(value);
			int num2 = ((precision == byte.MaxValue) ? num : Math.Max((int)precision, num));
			if (destination.Length < num2)
			{
				bytesWritten = 0;
				return false;
			}
			bytesWritten = num2;
			string text = (useLower ? "0123456789abcdef" : "0123456789ABCDEF");
			while (--num2 < destination.Length)
			{
				*destination[num2] = (byte)text[(int)value & 15];
				value >>= 4;
			}
			return true;
		}

		// Token: 0x0600643C RID: 25660 RVA: 0x00150C08 File Offset: 0x0014EE08
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool TryFormatUInt64(ulong value, Span<byte> destination, out int bytesWritten, StandardFormat format)
		{
			if (format.IsDefault)
			{
				return Utf8Formatter.TryFormatUInt64Default(value, destination, out bytesWritten);
			}
			char symbol = format.Symbol;
			if (symbol <= 'X')
			{
				if (symbol <= 'G')
				{
					if (symbol == 'D')
					{
						goto IL_0084;
					}
					if (symbol != 'G')
					{
						goto IL_00C8;
					}
				}
				else
				{
					if (symbol == 'N')
					{
						goto IL_0095;
					}
					if (symbol != 'X')
					{
						goto IL_00C8;
					}
					return Utf8Formatter.TryFormatUInt64X(value, format.Precision, false, destination, out bytesWritten);
				}
			}
			else if (symbol <= 'g')
			{
				if (symbol == 'd')
				{
					goto IL_0084;
				}
				if (symbol != 'g')
				{
					goto IL_00C8;
				}
			}
			else
			{
				if (symbol == 'n')
				{
					goto IL_0095;
				}
				if (symbol != 'x')
				{
					goto IL_00C8;
				}
				return Utf8Formatter.TryFormatUInt64X(value, format.Precision, true, destination, out bytesWritten);
			}
			if (format.HasPrecision)
			{
				throw new NotSupportedException("The 'G' format combined with a precision is not supported.");
			}
			return Utf8Formatter.TryFormatUInt64D(value, format.Precision, destination, false, out bytesWritten);
			IL_0084:
			return Utf8Formatter.TryFormatUInt64D(value, format.Precision, destination, false, out bytesWritten);
			IL_0095:
			return Utf8Formatter.TryFormatUInt64N(value, format.Precision, destination, false, out bytesWritten);
			IL_00C8:
			return ThrowHelper.TryFormatThrowFormatException(out bytesWritten);
		}

		// Token: 0x0600643D RID: 25661 RVA: 0x00150CE3 File Offset: 0x0014EEE3
		public static bool TryFormat(byte value, Span<byte> destination, out int bytesWritten, StandardFormat format = default(StandardFormat))
		{
			return Utf8Formatter.TryFormatUInt64((ulong)value, destination, out bytesWritten, format);
		}

		// Token: 0x0600643E RID: 25662 RVA: 0x00150CEF File Offset: 0x0014EEEF
		[CLSCompliant(false)]
		public static bool TryFormat(sbyte value, Span<byte> destination, out int bytesWritten, StandardFormat format = default(StandardFormat))
		{
			return Utf8Formatter.TryFormatInt64((long)value, 255UL, destination, out bytesWritten, format);
		}

		// Token: 0x0600643F RID: 25663 RVA: 0x00150CE3 File Offset: 0x0014EEE3
		[CLSCompliant(false)]
		public static bool TryFormat(ushort value, Span<byte> destination, out int bytesWritten, StandardFormat format = default(StandardFormat))
		{
			return Utf8Formatter.TryFormatUInt64((ulong)value, destination, out bytesWritten, format);
		}

		// Token: 0x06006440 RID: 25664 RVA: 0x00150D01 File Offset: 0x0014EF01
		public static bool TryFormat(short value, Span<byte> destination, out int bytesWritten, StandardFormat format = default(StandardFormat))
		{
			return Utf8Formatter.TryFormatInt64((long)value, 65535UL, destination, out bytesWritten, format);
		}

		// Token: 0x06006441 RID: 25665 RVA: 0x00150CE3 File Offset: 0x0014EEE3
		[CLSCompliant(false)]
		public static bool TryFormat(uint value, Span<byte> destination, out int bytesWritten, StandardFormat format = default(StandardFormat))
		{
			return Utf8Formatter.TryFormatUInt64((ulong)value, destination, out bytesWritten, format);
		}

		// Token: 0x06006442 RID: 25666 RVA: 0x00150D13 File Offset: 0x0014EF13
		public static bool TryFormat(int value, Span<byte> destination, out int bytesWritten, StandardFormat format = default(StandardFormat))
		{
			return Utf8Formatter.TryFormatInt64((long)value, (ulong)(-1), destination, out bytesWritten, format);
		}

		// Token: 0x06006443 RID: 25667 RVA: 0x00150D21 File Offset: 0x0014EF21
		[CLSCompliant(false)]
		public static bool TryFormat(ulong value, Span<byte> destination, out int bytesWritten, StandardFormat format = default(StandardFormat))
		{
			return Utf8Formatter.TryFormatUInt64(value, destination, out bytesWritten, format);
		}

		// Token: 0x06006444 RID: 25668 RVA: 0x00150D2C File Offset: 0x0014EF2C
		public static bool TryFormat(long value, Span<byte> destination, out int bytesWritten, StandardFormat format = default(StandardFormat))
		{
			return Utf8Formatter.TryFormatInt64(value, ulong.MaxValue, destination, out bytesWritten, format);
		}

		// Token: 0x06006445 RID: 25669 RVA: 0x00150D3C File Offset: 0x0014EF3C
		public unsafe static bool TryFormat(TimeSpan value, Span<byte> destination, out int bytesWritten, StandardFormat format = default(StandardFormat))
		{
			char c = FormattingHelpers.GetSymbolOrDefault(in format, 'c');
			if (c <= 'T')
			{
				if (c == 'G')
				{
					goto IL_0036;
				}
				if (c != 'T')
				{
					goto IL_002F;
				}
			}
			else
			{
				if (c == 'c' || c == 'g')
				{
					goto IL_0036;
				}
				if (c != 't')
				{
					goto IL_002F;
				}
			}
			c = 'c';
			goto IL_0036;
			IL_002F:
			return ThrowHelper.TryFormatThrowFormatException(out bytesWritten);
			IL_0036:
			int num = 8;
			long num2 = value.Ticks;
			uint num3;
			ulong num4;
			if (num2 < 0L)
			{
				num2 = -num2;
				if (num2 < 0L)
				{
					num3 = 4775808U;
					num4 = 922337203685UL;
					goto IL_0082;
				}
			}
			ulong num5;
			num4 = FormattingHelpers.DivMod((ulong)Math.Abs(value.Ticks), 10000000UL, out num5);
			num3 = (uint)num5;
			IL_0082:
			int num6 = 0;
			if (c == 'c')
			{
				if (num3 != 0U)
				{
					num6 = 7;
				}
			}
			else if (c == 'G')
			{
				num6 = 7;
			}
			else if (num3 != 0U)
			{
				num6 = 7 - FormattingHelpers.CountDecimalTrailingZeros(num3, out num3);
			}
			if (num6 != 0)
			{
				num += num6 + 1;
			}
			ulong num7 = 0UL;
			ulong num8 = 0UL;
			if (num4 > 0UL)
			{
				num7 = FormattingHelpers.DivMod(num4, 60UL, out num8);
			}
			ulong num9 = 0UL;
			ulong num10 = 0UL;
			if (num7 > 0UL)
			{
				num9 = FormattingHelpers.DivMod(num7, 60UL, out num10);
			}
			uint num11 = 0U;
			uint num12 = 0U;
			if (num9 > 0UL)
			{
				num11 = FormattingHelpers.DivMod((uint)num9, 24U, out num12);
			}
			int num13 = 2;
			if (num12 < 10U && c == 'g')
			{
				num13--;
				num--;
			}
			int num14 = 0;
			if (num11 == 0U)
			{
				if (c == 'G')
				{
					num += 2;
					num14 = 1;
				}
			}
			else
			{
				num14 = FormattingHelpers.CountDigits(num11);
				num += num14 + 1;
			}
			if (value.Ticks < 0L)
			{
				num++;
			}
			if (destination.Length < num)
			{
				bytesWritten = 0;
				return false;
			}
			bytesWritten = num;
			int num15 = 0;
			if (value.Ticks < 0L)
			{
				*destination[num15++] = 45;
			}
			if (num14 > 0)
			{
				FormattingHelpers.WriteDigits(num11, destination.Slice(num15, num14));
				num15 += num14;
				*destination[num15++] = ((c == 'c') ? 46 : 58);
			}
			FormattingHelpers.WriteDigits(num12, destination.Slice(num15, num13));
			num15 += num13;
			*destination[num15++] = 58;
			FormattingHelpers.WriteDigits((uint)num10, destination.Slice(num15, 2));
			num15 += 2;
			*destination[num15++] = 58;
			FormattingHelpers.WriteDigits((uint)num8, destination.Slice(num15, 2));
			num15 += 2;
			if (num6 > 0)
			{
				*destination[num15++] = 46;
				FormattingHelpers.WriteDigits(num3, destination.Slice(num15, num6));
				num15 += num6;
			}
			return true;
		}

		// Token: 0x04003AAA RID: 15018
		private const byte TimeMarker = 84;

		// Token: 0x04003AAB RID: 15019
		private const byte UtcMarker = 90;

		// Token: 0x04003AAC RID: 15020
		private const byte GMT1 = 71;

		// Token: 0x04003AAD RID: 15021
		private const byte GMT2 = 77;

		// Token: 0x04003AAE RID: 15022
		private const byte GMT3 = 84;

		// Token: 0x04003AAF RID: 15023
		private const byte GMT1Lowercase = 103;

		// Token: 0x04003AB0 RID: 15024
		private const byte GMT2Lowercase = 109;

		// Token: 0x04003AB1 RID: 15025
		private const byte GMT3Lowercase = 116;

		// Token: 0x04003AB2 RID: 15026
		private static readonly uint[] DayAbbreviations = new uint[] { 7238995U, 7237453U, 6649172U, 6579543U, 7694420U, 6910534U, 7627091U };

		// Token: 0x04003AB3 RID: 15027
		private static readonly uint[] DayAbbreviationsLowercase = new uint[] { 7239027U, 7237485U, 6649204U, 6579575U, 7694452U, 6910566U, 7627123U };

		// Token: 0x04003AB4 RID: 15028
		private static readonly uint[] MonthAbbreviations = new uint[]
		{
			7233866U, 6448454U, 7496013U, 7499841U, 7954765U, 7238986U, 7107914U, 6780225U, 7365971U, 7627599U,
			7761742U, 6513988U
		};

		// Token: 0x04003AB5 RID: 15029
		private static readonly uint[] MonthAbbreviationsLowercase = new uint[]
		{
			7233898U, 6448486U, 7496045U, 7499873U, 7954797U, 7239018U, 7107946U, 6780257U, 7366003U, 7627631U,
			7761774U, 6514020U
		};

		// Token: 0x04003AB6 RID: 15030
		private const byte OpenBrace = 123;

		// Token: 0x04003AB7 RID: 15031
		private const byte CloseBrace = 125;

		// Token: 0x04003AB8 RID: 15032
		private const byte OpenParen = 40;

		// Token: 0x04003AB9 RID: 15033
		private const byte CloseParen = 41;

		// Token: 0x04003ABA RID: 15034
		private const byte Dash = 45;

		// Token: 0x02000AF7 RID: 2807
		[StructLayout(LayoutKind.Explicit)]
		private struct DecomposedGuid
		{
			// Token: 0x04003ABB RID: 15035
			[FieldOffset(0)]
			public Guid Guid;

			// Token: 0x04003ABC RID: 15036
			[FieldOffset(0)]
			public byte Byte00;

			// Token: 0x04003ABD RID: 15037
			[FieldOffset(1)]
			public byte Byte01;

			// Token: 0x04003ABE RID: 15038
			[FieldOffset(2)]
			public byte Byte02;

			// Token: 0x04003ABF RID: 15039
			[FieldOffset(3)]
			public byte Byte03;

			// Token: 0x04003AC0 RID: 15040
			[FieldOffset(4)]
			public byte Byte04;

			// Token: 0x04003AC1 RID: 15041
			[FieldOffset(5)]
			public byte Byte05;

			// Token: 0x04003AC2 RID: 15042
			[FieldOffset(6)]
			public byte Byte06;

			// Token: 0x04003AC3 RID: 15043
			[FieldOffset(7)]
			public byte Byte07;

			// Token: 0x04003AC4 RID: 15044
			[FieldOffset(8)]
			public byte Byte08;

			// Token: 0x04003AC5 RID: 15045
			[FieldOffset(9)]
			public byte Byte09;

			// Token: 0x04003AC6 RID: 15046
			[FieldOffset(10)]
			public byte Byte10;

			// Token: 0x04003AC7 RID: 15047
			[FieldOffset(11)]
			public byte Byte11;

			// Token: 0x04003AC8 RID: 15048
			[FieldOffset(12)]
			public byte Byte12;

			// Token: 0x04003AC9 RID: 15049
			[FieldOffset(13)]
			public byte Byte13;

			// Token: 0x04003ACA RID: 15050
			[FieldOffset(14)]
			public byte Byte14;

			// Token: 0x04003ACB RID: 15051
			[FieldOffset(15)]
			public byte Byte15;
		}
	}
}
