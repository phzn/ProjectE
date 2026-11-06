using System;
using System.Text;

namespace System.Globalization
{
	// Token: 0x0200096F RID: 2415
	internal static class TimeSpanFormat
	{
		// Token: 0x06005552 RID: 21842 RVA: 0x0011E4AC File Offset: 0x0011C6AC
		private unsafe static void AppendNonNegativeInt32(StringBuilder sb, int n, int digits)
		{
			uint num = (uint)n;
			char* ptr = stackalloc char[(UIntPtr)20];
			int num2 = 0;
			do
			{
				uint num3 = num / 10U;
				ptr[(IntPtr)(num2++) * 2] = (char)(num - num3 * 10U + 48U);
				num = num3;
			}
			while (num != 0U);
			for (int i = digits - num2; i > 0; i--)
			{
				sb.Append('0');
			}
			for (int j = num2 - 1; j >= 0; j--)
			{
				sb.Append(ptr[j]);
			}
		}

		// Token: 0x06005553 RID: 21843 RVA: 0x0011E51E File Offset: 0x0011C71E
		internal static string Format(TimeSpan value, string format, IFormatProvider formatProvider)
		{
			return StringBuilderCache.GetStringAndRelease(TimeSpanFormat.FormatToBuilder(value, format, formatProvider));
		}

		// Token: 0x06005554 RID: 21844 RVA: 0x0011E534 File Offset: 0x0011C734
		internal static bool TryFormat(TimeSpan value, Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider formatProvider)
		{
			StringBuilder stringBuilder = TimeSpanFormat.FormatToBuilder(value, format, formatProvider);
			if (stringBuilder.Length <= destination.Length)
			{
				charsWritten = stringBuilder.Length;
				stringBuilder.CopyTo(0, destination, stringBuilder.Length);
				StringBuilderCache.Release(stringBuilder);
				return true;
			}
			StringBuilderCache.Release(stringBuilder);
			charsWritten = 0;
			return false;
		}

		// Token: 0x06005555 RID: 21845 RVA: 0x0011E584 File Offset: 0x0011C784
		private unsafe static StringBuilder FormatToBuilder(TimeSpan value, ReadOnlySpan<char> format, IFormatProvider formatProvider)
		{
			if (format.Length == 0)
			{
				format = "c";
			}
			if (format.Length == 1)
			{
				char c = (char)(*format[0]);
				if (c <= 'T')
				{
					if (c == 'G')
					{
						goto IL_0053;
					}
					if (c != 'T')
					{
						goto IL_0089;
					}
				}
				else if (c != 'c')
				{
					if (c == 'g')
					{
						goto IL_0053;
					}
					if (c != 't')
					{
						goto IL_0089;
					}
				}
				return TimeSpanFormat.FormatStandard(value, true, format, TimeSpanFormat.Pattern.Minimum);
				IL_0053:
				DateTimeFormatInfo instance = DateTimeFormatInfo.GetInstance(formatProvider);
				return TimeSpanFormat.FormatStandard(value, false, (value.Ticks < 0L) ? instance.FullTimeSpanNegativePattern : instance.FullTimeSpanPositivePattern, (c == 'g') ? TimeSpanFormat.Pattern.Minimum : TimeSpanFormat.Pattern.Full);
				IL_0089:
				throw new FormatException("Input string was not in a correct format.");
			}
			return TimeSpanFormat.FormatCustomized(value, format, DateTimeFormatInfo.GetInstance(formatProvider), null);
		}

		// Token: 0x06005556 RID: 21846 RVA: 0x0011E634 File Offset: 0x0011C834
		private static StringBuilder FormatStandard(TimeSpan value, bool isInvariant, ReadOnlySpan<char> format, TimeSpanFormat.Pattern pattern)
		{
			StringBuilder stringBuilder = StringBuilderCache.Acquire(16);
			int num = (int)(value.Ticks / 864000000000L);
			long num2 = value.Ticks % 864000000000L;
			if (value.Ticks < 0L)
			{
				num = -num;
				num2 = -num2;
			}
			int num3 = (int)(num2 / 36000000000L % 24L);
			int num4 = (int)(num2 / 600000000L % 60L);
			int num5 = (int)(num2 / 10000000L % 60L);
			int num6 = (int)(num2 % 10000000L);
			TimeSpanFormat.FormatLiterals formatLiterals;
			if (isInvariant)
			{
				formatLiterals = ((value.Ticks < 0L) ? TimeSpanFormat.NegativeInvariantFormatLiterals : TimeSpanFormat.PositiveInvariantFormatLiterals);
			}
			else
			{
				formatLiterals = default(TimeSpanFormat.FormatLiterals);
				formatLiterals.Init(format, pattern == TimeSpanFormat.Pattern.Full);
			}
			if (num6 != 0)
			{
				num6 = (int)((long)num6 / TimeSpanParse.Pow10(7 - formatLiterals.ff));
			}
			stringBuilder.Append(formatLiterals.Start);
			if (pattern == TimeSpanFormat.Pattern.Full || num != 0)
			{
				stringBuilder.Append(num);
				stringBuilder.Append(formatLiterals.DayHourSep);
			}
			TimeSpanFormat.AppendNonNegativeInt32(stringBuilder, num3, formatLiterals.hh);
			stringBuilder.Append(formatLiterals.HourMinuteSep);
			TimeSpanFormat.AppendNonNegativeInt32(stringBuilder, num4, formatLiterals.mm);
			stringBuilder.Append(formatLiterals.MinuteSecondSep);
			TimeSpanFormat.AppendNonNegativeInt32(stringBuilder, num5, formatLiterals.ss);
			if (!isInvariant && pattern == TimeSpanFormat.Pattern.Minimum)
			{
				int num7 = formatLiterals.ff;
				while (num7 > 0 && num6 % 10 == 0)
				{
					num6 /= 10;
					num7--;
				}
				if (num7 > 0)
				{
					stringBuilder.Append(formatLiterals.SecondFractionSep);
					stringBuilder.Append(num6.ToString(DateTimeFormat.fixedNumberFormats[num7 - 1], CultureInfo.InvariantCulture));
				}
			}
			else if (pattern == TimeSpanFormat.Pattern.Full || num6 != 0)
			{
				stringBuilder.Append(formatLiterals.SecondFractionSep);
				TimeSpanFormat.AppendNonNegativeInt32(stringBuilder, num6, formatLiterals.ff);
			}
			stringBuilder.Append(formatLiterals.End);
			return stringBuilder;
		}

		// Token: 0x06005557 RID: 21847 RVA: 0x0011E808 File Offset: 0x0011CA08
		private unsafe static StringBuilder FormatCustomized(TimeSpan value, ReadOnlySpan<char> format, DateTimeFormatInfo dtfi, StringBuilder result)
		{
			bool flag = false;
			if (result == null)
			{
				result = StringBuilderCache.Acquire(16);
				flag = true;
			}
			int num = (int)(value.Ticks / 864000000000L);
			long num2 = value.Ticks % 864000000000L;
			if (value.Ticks < 0L)
			{
				num = -num;
				num2 = -num2;
			}
			int num3 = (int)(num2 / 36000000000L % 24L);
			int num4 = (int)(num2 / 600000000L % 60L);
			int num5 = (int)(num2 / 10000000L % 60L);
			int num6 = (int)(num2 % 10000000L);
			int i = 0;
			while (i < format.Length)
			{
				char c = (char)(*format[i]);
				int num8;
				if (c <= 'F')
				{
					if (c <= '%')
					{
						if (c != '"')
						{
							if (c != '%')
							{
								goto IL_02B5;
							}
							int num7 = DateTimeFormat.ParseNextChar(format, i);
							if (num7 >= 0 && num7 != 37)
							{
								char c2 = (char)num7;
								ReadOnlySpan<char> readOnlySpan = new ReadOnlySpan<char>((void*)(&c2), 1);
								TimeSpanFormat.FormatCustomized(value, readOnlySpan, dtfi, result);
								num8 = 2;
								goto IL_02C9;
							}
							goto IL_02B5;
						}
					}
					else if (c != '\'')
					{
						if (c != 'F')
						{
							goto IL_02B5;
						}
						num8 = DateTimeFormat.ParseRepeatPattern(format, i, c);
						if (num8 > 7)
						{
							goto IL_02B5;
						}
						long num9 = (long)num6;
						num9 /= TimeSpanParse.Pow10(7 - num8);
						int num10 = num8;
						while (num10 > 0 && num9 % 10L == 0L)
						{
							num9 /= 10L;
							num10--;
						}
						if (num10 > 0)
						{
							result.Append(num9.ToString(DateTimeFormat.fixedNumberFormats[num10 - 1], CultureInfo.InvariantCulture));
							goto IL_02C9;
						}
						goto IL_02C9;
					}
					num8 = DateTimeFormat.ParseQuoteString(format, i, result);
				}
				else if (c <= 'h')
				{
					if (c != '\\')
					{
						switch (c)
						{
						case 'd':
							num8 = DateTimeFormat.ParseRepeatPattern(format, i, c);
							if (num8 > 8)
							{
								goto IL_02B5;
							}
							DateTimeFormat.FormatDigits(result, num, num8, true);
							break;
						case 'e':
						case 'g':
							goto IL_02B5;
						case 'f':
						{
							num8 = DateTimeFormat.ParseRepeatPattern(format, i, c);
							if (num8 > 7)
							{
								goto IL_02B5;
							}
							long num9 = (long)num6;
							result.Append((num9 / TimeSpanParse.Pow10(7 - num8)).ToString(DateTimeFormat.fixedNumberFormats[num8 - 1], CultureInfo.InvariantCulture));
							break;
						}
						case 'h':
							num8 = DateTimeFormat.ParseRepeatPattern(format, i, c);
							if (num8 > 2)
							{
								goto IL_02B5;
							}
							DateTimeFormat.FormatDigits(result, num3, num8);
							break;
						default:
							goto IL_02B5;
						}
					}
					else
					{
						int num7 = DateTimeFormat.ParseNextChar(format, i);
						if (num7 < 0)
						{
							goto IL_02B5;
						}
						result.Append((char)num7);
						num8 = 2;
					}
				}
				else if (c != 'm')
				{
					if (c != 's')
					{
						goto IL_02B5;
					}
					num8 = DateTimeFormat.ParseRepeatPattern(format, i, c);
					if (num8 > 2)
					{
						goto IL_02B5;
					}
					DateTimeFormat.FormatDigits(result, num5, num8);
				}
				else
				{
					num8 = DateTimeFormat.ParseRepeatPattern(format, i, c);
					if (num8 > 2)
					{
						goto IL_02B5;
					}
					DateTimeFormat.FormatDigits(result, num4, num8);
				}
				IL_02C9:
				i += num8;
				continue;
				IL_02B5:
				if (flag)
				{
					StringBuilderCache.Release(result);
				}
				throw new FormatException("Input string was not in a correct format.");
			}
			return result;
		}

		// Token: 0x040034E4 RID: 13540
		internal static readonly TimeSpanFormat.FormatLiterals PositiveInvariantFormatLiterals = TimeSpanFormat.FormatLiterals.InitInvariant(false);

		// Token: 0x040034E5 RID: 13541
		internal static readonly TimeSpanFormat.FormatLiterals NegativeInvariantFormatLiterals = TimeSpanFormat.FormatLiterals.InitInvariant(true);

		// Token: 0x02000970 RID: 2416
		internal enum Pattern
		{
			// Token: 0x040034E7 RID: 13543
			None,
			// Token: 0x040034E8 RID: 13544
			Minimum,
			// Token: 0x040034E9 RID: 13545
			Full
		}

		// Token: 0x02000971 RID: 2417
		internal struct FormatLiterals
		{
			// Token: 0x17000E2D RID: 3629
			// (get) Token: 0x06005559 RID: 21849 RVA: 0x0011EB0C File Offset: 0x0011CD0C
			internal string Start
			{
				get
				{
					return this._literals[0];
				}
			}

			// Token: 0x17000E2E RID: 3630
			// (get) Token: 0x0600555A RID: 21850 RVA: 0x0011EB16 File Offset: 0x0011CD16
			internal string DayHourSep
			{
				get
				{
					return this._literals[1];
				}
			}

			// Token: 0x17000E2F RID: 3631
			// (get) Token: 0x0600555B RID: 21851 RVA: 0x0011EB20 File Offset: 0x0011CD20
			internal string HourMinuteSep
			{
				get
				{
					return this._literals[2];
				}
			}

			// Token: 0x17000E30 RID: 3632
			// (get) Token: 0x0600555C RID: 21852 RVA: 0x0011EB2A File Offset: 0x0011CD2A
			internal string MinuteSecondSep
			{
				get
				{
					return this._literals[3];
				}
			}

			// Token: 0x17000E31 RID: 3633
			// (get) Token: 0x0600555D RID: 21853 RVA: 0x0011EB34 File Offset: 0x0011CD34
			internal string SecondFractionSep
			{
				get
				{
					return this._literals[4];
				}
			}

			// Token: 0x17000E32 RID: 3634
			// (get) Token: 0x0600555E RID: 21854 RVA: 0x0011EB3E File Offset: 0x0011CD3E
			internal string End
			{
				get
				{
					return this._literals[5];
				}
			}

			// Token: 0x0600555F RID: 21855 RVA: 0x0011EB48 File Offset: 0x0011CD48
			internal static TimeSpanFormat.FormatLiterals InitInvariant(bool isNegative)
			{
				TimeSpanFormat.FormatLiterals formatLiterals = new TimeSpanFormat.FormatLiterals
				{
					_literals = new string[6]
				};
				formatLiterals._literals[0] = (isNegative ? "-" : string.Empty);
				formatLiterals._literals[1] = ".";
				formatLiterals._literals[2] = ":";
				formatLiterals._literals[3] = ":";
				formatLiterals._literals[4] = ".";
				formatLiterals._literals[5] = string.Empty;
				formatLiterals.AppCompatLiteral = ":.";
				formatLiterals.dd = 2;
				formatLiterals.hh = 2;
				formatLiterals.mm = 2;
				formatLiterals.ss = 2;
				formatLiterals.ff = 7;
				return formatLiterals;
			}

			// Token: 0x06005560 RID: 21856 RVA: 0x0011EBF8 File Offset: 0x0011CDF8
			internal unsafe void Init(ReadOnlySpan<char> format, bool useInvariantFieldLengths)
			{
				this.dd = (this.hh = (this.mm = (this.ss = (this.ff = 0))));
				this._literals = new string[6];
				for (int i = 0; i < this._literals.Length; i++)
				{
					this._literals[i] = string.Empty;
				}
				StringBuilder stringBuilder = StringBuilderCache.Acquire(16);
				bool flag = false;
				char c = '\'';
				int num = 0;
				int j = 0;
				while (j < format.Length)
				{
					char c2 = (char)(*format[j]);
					if (c2 <= 'F')
					{
						if (c2 <= '%')
						{
							if (c2 != '"')
							{
								if (c2 != '%')
								{
									goto IL_01C5;
								}
								goto IL_01C5;
							}
						}
						else if (c2 != '\'')
						{
							if (c2 != 'F')
							{
								goto IL_01C5;
							}
							goto IL_01B0;
						}
						if (flag && c == (char)(*format[j]))
						{
							if (num < 0 || num > 5)
							{
								return;
							}
							this._literals[num] = stringBuilder.ToString();
							stringBuilder.Length = 0;
							flag = false;
						}
						else if (!flag)
						{
							c = (char)(*format[j]);
							flag = true;
						}
					}
					else if (c2 <= 'h')
					{
						if (c2 != '\\')
						{
							switch (c2)
							{
							case 'd':
								if (!flag)
								{
									num = 1;
									this.dd++;
								}
								break;
							case 'e':
							case 'g':
								goto IL_01C5;
							case 'f':
								goto IL_01B0;
							case 'h':
								if (!flag)
								{
									num = 2;
									this.hh++;
								}
								break;
							default:
								goto IL_01C5;
							}
						}
						else
						{
							if (flag)
							{
								goto IL_01C5;
							}
							j++;
						}
					}
					else if (c2 != 'm')
					{
						if (c2 != 's')
						{
							goto IL_01C5;
						}
						if (!flag)
						{
							num = 4;
							this.ss++;
						}
					}
					else if (!flag)
					{
						num = 3;
						this.mm++;
					}
					IL_01D6:
					j++;
					continue;
					IL_01B0:
					if (!flag)
					{
						num = 5;
						this.ff++;
						goto IL_01D6;
					}
					goto IL_01D6;
					IL_01C5:
					stringBuilder.Append((char)(*format[j]));
					goto IL_01D6;
				}
				this.AppCompatLiteral = this.MinuteSecondSep + this.SecondFractionSep;
				if (useInvariantFieldLengths)
				{
					this.dd = 2;
					this.hh = 2;
					this.mm = 2;
					this.ss = 2;
					this.ff = 7;
				}
				else
				{
					if (this.dd < 1 || this.dd > 2)
					{
						this.dd = 2;
					}
					if (this.hh < 1 || this.hh > 2)
					{
						this.hh = 2;
					}
					if (this.mm < 1 || this.mm > 2)
					{
						this.mm = 2;
					}
					if (this.ss < 1 || this.ss > 2)
					{
						this.ss = 2;
					}
					if (this.ff < 1 || this.ff > 7)
					{
						this.ff = 7;
					}
				}
				StringBuilderCache.Release(stringBuilder);
			}

			// Token: 0x040034EA RID: 13546
			internal string AppCompatLiteral;

			// Token: 0x040034EB RID: 13547
			internal int dd;

			// Token: 0x040034EC RID: 13548
			internal int hh;

			// Token: 0x040034ED RID: 13549
			internal int mm;

			// Token: 0x040034EE RID: 13550
			internal int ss;

			// Token: 0x040034EF RID: 13551
			internal int ff;

			// Token: 0x040034F0 RID: 13552
			private string[] _literals;
		}
	}
}
