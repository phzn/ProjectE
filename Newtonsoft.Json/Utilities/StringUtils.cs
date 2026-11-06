using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200006B RID: 107
	[NullableContext(1)]
	[Nullable(0)]
	internal static class StringUtils
	{
		// Token: 0x060005C5 RID: 1477 RVA: 0x000183F3 File Offset: 0x000165F3
		[NullableContext(2)]
		public static bool IsNullOrEmpty([NotNullWhen(false)] string value)
		{
			return string.IsNullOrEmpty(value);
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x000183FB File Offset: 0x000165FB
		public static string FormatWith(this string format, IFormatProvider provider, [Nullable(2)] object arg0)
		{
			return format.FormatWith(provider, new object[] { arg0 });
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x0001840E File Offset: 0x0001660E
		public static string FormatWith(this string format, IFormatProvider provider, [Nullable(2)] object arg0, [Nullable(2)] object arg1)
		{
			return format.FormatWith(provider, new object[] { arg0, arg1 });
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x00018425 File Offset: 0x00016625
		public static string FormatWith(this string format, IFormatProvider provider, [Nullable(2)] object arg0, [Nullable(2)] object arg1, [Nullable(2)] object arg2)
		{
			return format.FormatWith(provider, new object[] { arg0, arg1, arg2 });
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x00018441 File Offset: 0x00016641
		[NullableContext(2)]
		[return: Nullable(1)]
		public static string FormatWith([Nullable(1)] this string format, [Nullable(1)] IFormatProvider provider, object arg0, object arg1, object arg2, object arg3)
		{
			return format.FormatWith(provider, new object[] { arg0, arg1, arg2, arg3 });
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x00018462 File Offset: 0x00016662
		private static string FormatWith(this string format, IFormatProvider provider, [Nullable(new byte[] { 1, 2 })] params object[] args)
		{
			ValidationUtils.ArgumentNotNull(format, "format");
			return string.Format(provider, format, args);
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x00018478 File Offset: 0x00016678
		public static bool IsWhiteSpace(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (s.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < s.Length; i++)
			{
				if (!char.IsWhiteSpace(s[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x000184BF File Offset: 0x000166BF
		public static StringWriter CreateStringWriter(int capacity)
		{
			return new StringWriter(new StringBuilder(capacity), CultureInfo.InvariantCulture);
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x000184D4 File Offset: 0x000166D4
		public static void ToCharAsUnicode(char c, char[] buffer)
		{
			buffer[0] = '\\';
			buffer[1] = 'u';
			buffer[2] = MathUtils.IntToHex((int)((c >> 12) & '\u000f'));
			buffer[3] = MathUtils.IntToHex((int)((c >> 8) & '\u000f'));
			buffer[4] = MathUtils.IntToHex((int)((c >> 4) & '\u000f'));
			buffer[5] = MathUtils.IntToHex((int)(c & '\u000f'));
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x00018524 File Offset: 0x00016724
		[return: Nullable(2)]
		public static TSource ForgivingCaseSensitiveFind<[Nullable(2)] TSource>(this IEnumerable<TSource> source, Func<TSource, string> valueSelector, string testValue)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (valueSelector == null)
			{
				throw new ArgumentNullException("valueSelector");
			}
			IEnumerable<TSource> enumerable = source.Where<TSource>((TSource s) => string.Equals(valueSelector(s), testValue, StringComparison.OrdinalIgnoreCase));
			if (enumerable.Count<TSource>() <= 1)
			{
				return enumerable.SingleOrDefault<TSource>();
			}
			return source.Where<TSource>((TSource s) => string.Equals(valueSelector(s), testValue, StringComparison.Ordinal)).SingleOrDefault<TSource>();
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x000185A0 File Offset: 0x000167A0
		public static string ToCamelCase(string s)
		{
			if (StringUtils.IsNullOrEmpty(s) || !char.IsUpper(s[0]))
			{
				return s;
			}
			char[] array = s.ToCharArray();
			int num = 0;
			while (num < array.Length && (num != 1 || char.IsUpper(array[num])))
			{
				bool flag = num + 1 < array.Length;
				if (num > 0 && flag && !char.IsUpper(array[num + 1]))
				{
					if (char.IsSeparator(array[num + 1]))
					{
						array[num] = StringUtils.ToLower(array[num]);
						break;
					}
					break;
				}
				else
				{
					array[num] = StringUtils.ToLower(array[num]);
					num++;
				}
			}
			return new string(array);
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x0001862F File Offset: 0x0001682F
		private static char ToLower(char c)
		{
			c = char.ToLower(c, CultureInfo.InvariantCulture);
			return c;
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x0001863F File Offset: 0x0001683F
		public static string ToSnakeCase(string s)
		{
			return StringUtils.ToSeparatedCase(s, '_');
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x00018649 File Offset: 0x00016849
		public static string ToKebabCase(string s)
		{
			return StringUtils.ToSeparatedCase(s, '-');
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x00018654 File Offset: 0x00016854
		private static string ToSeparatedCase(string s, char separator)
		{
			if (StringUtils.IsNullOrEmpty(s))
			{
				return s;
			}
			StringBuilder stringBuilder = new StringBuilder();
			StringUtils.SeparatedCaseState separatedCaseState = StringUtils.SeparatedCaseState.Start;
			for (int i = 0; i < s.Length; i++)
			{
				if (s[i] == ' ')
				{
					if (separatedCaseState != StringUtils.SeparatedCaseState.Start)
					{
						separatedCaseState = StringUtils.SeparatedCaseState.NewWord;
					}
				}
				else if (char.IsUpper(s[i]))
				{
					switch (separatedCaseState)
					{
					case StringUtils.SeparatedCaseState.Lower:
					case StringUtils.SeparatedCaseState.NewWord:
						stringBuilder.Append(separator);
						break;
					case StringUtils.SeparatedCaseState.Upper:
					{
						bool flag = i + 1 < s.Length;
						if (i > 0 && flag)
						{
							char c = s[i + 1];
							if (!char.IsUpper(c) && c != separator)
							{
								stringBuilder.Append(separator);
							}
						}
						break;
					}
					}
					char c2 = char.ToLower(s[i], CultureInfo.InvariantCulture);
					stringBuilder.Append(c2);
					separatedCaseState = StringUtils.SeparatedCaseState.Upper;
				}
				else if (s[i] == separator)
				{
					stringBuilder.Append(separator);
					separatedCaseState = StringUtils.SeparatedCaseState.Start;
				}
				else
				{
					if (separatedCaseState == StringUtils.SeparatedCaseState.NewWord)
					{
						stringBuilder.Append(separator);
					}
					stringBuilder.Append(s[i]);
					separatedCaseState = StringUtils.SeparatedCaseState.Lower;
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x0001875D File Offset: 0x0001695D
		public static bool IsHighSurrogate(char c)
		{
			return char.IsHighSurrogate(c);
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x00018765 File Offset: 0x00016965
		public static bool IsLowSurrogate(char c)
		{
			return char.IsLowSurrogate(c);
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x0001876D File Offset: 0x0001696D
		public static int IndexOf(string s, char c)
		{
			return s.IndexOf(c);
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x00018776 File Offset: 0x00016976
		public static string Replace(string s, string oldValue, string newValue)
		{
			return s.Replace(oldValue, newValue);
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x00018780 File Offset: 0x00016980
		public static bool StartsWith(this string source, char value)
		{
			return source.Length > 0 && source[0] == value;
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x00018797 File Offset: 0x00016997
		public static bool EndsWith(this string source, char value)
		{
			return source.Length > 0 && source[source.Length - 1] == value;
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x000187B8 File Offset: 0x000169B8
		public static string Trim(this string s, int start, int length)
		{
			if (s == null)
			{
				throw new ArgumentNullException();
			}
			if (start < 0)
			{
				throw new ArgumentOutOfRangeException("start");
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length");
			}
			int num = start + length - 1;
			if (num >= s.Length)
			{
				throw new ArgumentOutOfRangeException("length");
			}
			while (start < num)
			{
				if (!char.IsWhiteSpace(s[start]))
				{
					IL_006C:
					while (num >= start && char.IsWhiteSpace(s[num]))
					{
						num--;
					}
					return s.Substring(start, num - start + 1);
				}
				start++;
			}
			goto IL_006C;
		}

		// Token: 0x04000220 RID: 544
		public const string CarriageReturnLineFeed = "\r\n";

		// Token: 0x04000221 RID: 545
		public const string Empty = "";

		// Token: 0x04000222 RID: 546
		public const char CarriageReturn = '\r';

		// Token: 0x04000223 RID: 547
		public const char LineFeed = '\n';

		// Token: 0x04000224 RID: 548
		public const char Tab = '\t';

		// Token: 0x02000196 RID: 406
		[NullableContext(0)]
		private enum SeparatedCaseState
		{
			// Token: 0x04000719 RID: 1817
			Start,
			// Token: 0x0400071A RID: 1818
			Lower,
			// Token: 0x0400071B RID: 1819
			Upper,
			// Token: 0x0400071C RID: 1820
			NewWord
		}
	}
}
