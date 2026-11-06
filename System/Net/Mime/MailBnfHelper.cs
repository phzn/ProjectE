using System;
using System.Text;

namespace System.Net.Mime
{
	// Token: 0x020007CA RID: 1994
	internal static class MailBnfHelper
	{
		// Token: 0x06003FD7 RID: 16343 RVA: 0x000D9B9C File Offset: 0x000D7D9C
		private static bool[] CreateCharactersAllowedInAtoms()
		{
			bool[] array = new bool[128];
			for (int i = 48; i <= 57; i++)
			{
				array[i] = true;
			}
			for (int j = 65; j <= 90; j++)
			{
				array[j] = true;
			}
			for (int k = 97; k <= 122; k++)
			{
				array[k] = true;
			}
			array[33] = true;
			array[35] = true;
			array[36] = true;
			array[37] = true;
			array[38] = true;
			array[39] = true;
			array[42] = true;
			array[43] = true;
			array[45] = true;
			array[47] = true;
			array[61] = true;
			array[63] = true;
			array[94] = true;
			array[95] = true;
			array[96] = true;
			array[123] = true;
			array[124] = true;
			array[125] = true;
			array[126] = true;
			return array;
		}

		// Token: 0x06003FD8 RID: 16344 RVA: 0x000D9C4C File Offset: 0x000D7E4C
		private static bool[] CreateCharactersAllowedInQuotedStrings()
		{
			bool[] array = new bool[128];
			for (int i = 1; i <= 9; i++)
			{
				array[i] = true;
			}
			array[11] = true;
			array[12] = true;
			for (int j = 14; j <= 33; j++)
			{
				array[j] = true;
			}
			for (int k = 35; k <= 91; k++)
			{
				array[k] = true;
			}
			for (int l = 93; l <= 127; l++)
			{
				array[l] = true;
			}
			return array;
		}

		// Token: 0x06003FD9 RID: 16345 RVA: 0x000D9CBC File Offset: 0x000D7EBC
		private static bool[] CreateCharactersAllowedInDomainLiterals()
		{
			bool[] array = new bool[128];
			for (int i = 1; i <= 8; i++)
			{
				array[i] = true;
			}
			array[11] = true;
			array[12] = true;
			for (int j = 14; j <= 31; j++)
			{
				array[j] = true;
			}
			for (int k = 33; k <= 90; k++)
			{
				array[k] = true;
			}
			for (int l = 94; l <= 127; l++)
			{
				array[l] = true;
			}
			return array;
		}

		// Token: 0x06003FDA RID: 16346 RVA: 0x000D9D2C File Offset: 0x000D7F2C
		private static bool[] CreateCharactersAllowedInHeaderNames()
		{
			bool[] array = new bool[128];
			for (int i = 33; i <= 57; i++)
			{
				array[i] = true;
			}
			for (int j = 59; j <= 126; j++)
			{
				array[j] = true;
			}
			return array;
		}

		// Token: 0x06003FDB RID: 16347 RVA: 0x000D9D6C File Offset: 0x000D7F6C
		private static bool[] CreateCharactersAllowedInTokens()
		{
			bool[] array = new bool[128];
			for (int i = 33; i <= 126; i++)
			{
				array[i] = true;
			}
			array[40] = false;
			array[41] = false;
			array[60] = false;
			array[62] = false;
			array[64] = false;
			array[44] = false;
			array[59] = false;
			array[58] = false;
			array[92] = false;
			array[34] = false;
			array[47] = false;
			array[91] = false;
			array[93] = false;
			array[63] = false;
			array[61] = false;
			return array;
		}

		// Token: 0x06003FDC RID: 16348 RVA: 0x000D9DE4 File Offset: 0x000D7FE4
		private static bool[] CreateCharactersAllowedInComments()
		{
			bool[] array = new bool[128];
			for (int i = 1; i <= 8; i++)
			{
				array[i] = true;
			}
			array[11] = true;
			array[12] = true;
			for (int j = 14; j <= 31; j++)
			{
				array[j] = true;
			}
			for (int k = 33; k <= 39; k++)
			{
				array[k] = true;
			}
			for (int l = 42; l <= 91; l++)
			{
				array[l] = true;
			}
			for (int m = 93; m <= 127; m++)
			{
				array[m] = true;
			}
			return array;
		}

		// Token: 0x06003FDD RID: 16349 RVA: 0x000D9E6C File Offset: 0x000D806C
		internal static bool SkipCFWS(string data, ref int offset)
		{
			int num = 0;
			while (offset < data.Length)
			{
				if (data[offset] > '\u007f')
				{
					throw new FormatException(SR.Format("An invalid character was found in the mail header: '{0}'.", data[offset]));
				}
				if (data[offset] == '\\' && num > 0)
				{
					offset += 2;
				}
				else if (data[offset] == '(')
				{
					num++;
				}
				else if (data[offset] == ')')
				{
					num--;
				}
				else if (data[offset] != ' ' && data[offset] != '\t' && num == 0)
				{
					return true;
				}
				if (num < 0)
				{
					throw new FormatException(SR.Format("An invalid character was found in the mail header: '{0}'.", data[offset]));
				}
				offset++;
			}
			return false;
		}

		// Token: 0x06003FDE RID: 16350 RVA: 0x000D9F38 File Offset: 0x000D8138
		internal static void ValidateHeaderName(string data)
		{
			int i;
			for (i = 0; i < data.Length; i++)
			{
				if ((int)data[i] > MailBnfHelper.Ftext.Length || !MailBnfHelper.Ftext[(int)data[i]])
				{
					throw new FormatException("An invalid character was found in header name.");
				}
			}
			if (i == 0)
			{
				throw new FormatException("An invalid character was found in header name.");
			}
		}

		// Token: 0x06003FDF RID: 16351 RVA: 0x000D9F8E File Offset: 0x000D818E
		internal static string ReadQuotedString(string data, ref int offset, StringBuilder builder)
		{
			return MailBnfHelper.ReadQuotedString(data, ref offset, builder, false, false);
		}

		// Token: 0x06003FE0 RID: 16352 RVA: 0x000D9F9C File Offset: 0x000D819C
		internal static string ReadQuotedString(string data, ref int offset, StringBuilder builder, bool doesntRequireQuotes, bool permitUnicodeInDisplayName)
		{
			if (!doesntRequireQuotes)
			{
				offset++;
			}
			int num = offset;
			StringBuilder stringBuilder = ((builder != null) ? builder : new StringBuilder());
			while (offset < data.Length)
			{
				if (data[offset] == '\\')
				{
					stringBuilder.Append(data, num, offset - num);
					int num2 = offset + 1;
					offset = num2;
					num = num2;
				}
				else if (data[offset] == '"')
				{
					stringBuilder.Append(data, num, offset - num);
					offset++;
					if (builder == null)
					{
						return stringBuilder.ToString();
					}
					return null;
				}
				else if (data[offset] == '=' && data.Length > offset + 3 && data[offset + 1] == '\r' && data[offset + 2] == '\n' && (data[offset + 3] == ' ' || data[offset + 3] == '\t'))
				{
					offset += 3;
				}
				else if (permitUnicodeInDisplayName)
				{
					if ((int)data[offset] <= MailBnfHelper.Ascii7bitMaxValue && !MailBnfHelper.Qtext[(int)data[offset]])
					{
						throw new FormatException(SR.Format("An invalid character was found in the mail header: '{0}'.", data[offset]));
					}
				}
				else if ((int)data[offset] > MailBnfHelper.Ascii7bitMaxValue || !MailBnfHelper.Qtext[(int)data[offset]])
				{
					throw new FormatException(SR.Format("An invalid character was found in the mail header: '{0}'.", data[offset]));
				}
				offset++;
			}
			if (!doesntRequireQuotes)
			{
				throw new FormatException("The mail header is malformed.");
			}
			stringBuilder.Append(data, num, offset - num);
			if (builder == null)
			{
				return stringBuilder.ToString();
			}
			return null;
		}

		// Token: 0x06003FE1 RID: 16353 RVA: 0x000DA126 File Offset: 0x000D8326
		internal static string ReadParameterAttribute(string data, ref int offset, StringBuilder builder)
		{
			if (!MailBnfHelper.SkipCFWS(data, ref offset))
			{
				return null;
			}
			return MailBnfHelper.ReadToken(data, ref offset, null);
		}

		// Token: 0x06003FE2 RID: 16354 RVA: 0x000DA13C File Offset: 0x000D833C
		internal static string ReadToken(string data, ref int offset, StringBuilder builder)
		{
			int num = offset;
			while (offset < data.Length)
			{
				if ((int)data[offset] > MailBnfHelper.Ascii7bitMaxValue)
				{
					throw new FormatException(SR.Format("An invalid character was found in the mail header: '{0}'.", data[offset]));
				}
				if (!MailBnfHelper.Ttext[(int)data[offset]])
				{
					break;
				}
				offset++;
			}
			if (num == offset)
			{
				throw new FormatException(SR.Format("An invalid character was found in the mail header: '{0}'.", data[offset]));
			}
			return data.Substring(num, offset - num);
		}

		// Token: 0x06003FE3 RID: 16355 RVA: 0x000DA1C8 File Offset: 0x000D83C8
		internal static string GetDateTimeString(DateTime value, StringBuilder builder)
		{
			StringBuilder stringBuilder = ((builder != null) ? builder : new StringBuilder());
			stringBuilder.Append(value.Day);
			stringBuilder.Append(' ');
			stringBuilder.Append(MailBnfHelper.s_months[value.Month]);
			stringBuilder.Append(' ');
			stringBuilder.Append(value.Year);
			stringBuilder.Append(' ');
			if (value.Hour <= 9)
			{
				stringBuilder.Append('0');
			}
			stringBuilder.Append(value.Hour);
			stringBuilder.Append(':');
			if (value.Minute <= 9)
			{
				stringBuilder.Append('0');
			}
			stringBuilder.Append(value.Minute);
			stringBuilder.Append(':');
			if (value.Second <= 9)
			{
				stringBuilder.Append('0');
			}
			stringBuilder.Append(value.Second);
			string text = TimeZoneInfo.Local.GetUtcOffset(value).ToString();
			if (text[0] != '-')
			{
				stringBuilder.Append(" +");
			}
			else
			{
				stringBuilder.Append(' ');
			}
			string[] array = text.Split(MailBnfHelper.s_colonSeparator);
			stringBuilder.Append(array[0]);
			stringBuilder.Append(array[1]);
			if (builder == null)
			{
				return stringBuilder.ToString();
			}
			return null;
		}

		// Token: 0x06003FE4 RID: 16356 RVA: 0x000DA30C File Offset: 0x000D850C
		internal static void GetTokenOrQuotedString(string data, StringBuilder builder, bool allowUnicode)
		{
			int i = 0;
			int num = 0;
			while (i < data.Length)
			{
				if (!MailBnfHelper.CheckForUnicode(data[i], allowUnicode) && (!MailBnfHelper.Ttext[(int)data[i]] || data[i] == ' '))
				{
					builder.Append('"');
					while (i < data.Length)
					{
						if (!MailBnfHelper.CheckForUnicode(data[i], allowUnicode))
						{
							if (MailBnfHelper.IsFWSAt(data, i))
							{
								i += 2;
							}
							else if (!MailBnfHelper.Qtext[(int)data[i]])
							{
								builder.Append(data, num, i - num);
								builder.Append('\\');
								num = i;
							}
						}
						i++;
					}
					builder.Append(data, num, i - num);
					builder.Append('"');
					return;
				}
				i++;
			}
			if (data.Length == 0)
			{
				builder.Append("\"\"");
			}
			builder.Append(data);
		}

		// Token: 0x06003FE5 RID: 16357 RVA: 0x000DA3EC File Offset: 0x000D85EC
		private static bool CheckForUnicode(char ch, bool allowUnicode)
		{
			if ((int)ch < MailBnfHelper.Ascii7bitMaxValue)
			{
				return false;
			}
			if (!allowUnicode)
			{
				throw new FormatException(SR.Format("An invalid character was found in the mail header: '{0}'.", ch));
			}
			return true;
		}

		// Token: 0x06003FE6 RID: 16358 RVA: 0x000DA412 File Offset: 0x000D8612
		internal static bool IsAllowedWhiteSpace(char c)
		{
			return c == MailBnfHelper.Tab || c == MailBnfHelper.Space || c == MailBnfHelper.CR || c == MailBnfHelper.LF;
		}

		// Token: 0x06003FE7 RID: 16359 RVA: 0x000DA438 File Offset: 0x000D8638
		internal static bool HasCROrLF(string data)
		{
			for (int i = 0; i < data.Length; i++)
			{
				if (data[i] == '\r' || data[i] == '\n')
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003FE8 RID: 16360 RVA: 0x000DA470 File Offset: 0x000D8670
		internal static bool IsFWSAt(string data, int index)
		{
			return data[index] == MailBnfHelper.CR && index + 2 < data.Length && data[index + 1] == MailBnfHelper.LF && (data[index + 2] == MailBnfHelper.Space || data[index + 2] == MailBnfHelper.Tab);
		}

		// Token: 0x04002637 RID: 9783
		internal static readonly bool[] Atext = MailBnfHelper.CreateCharactersAllowedInAtoms();

		// Token: 0x04002638 RID: 9784
		internal static readonly bool[] Qtext = MailBnfHelper.CreateCharactersAllowedInQuotedStrings();

		// Token: 0x04002639 RID: 9785
		internal static readonly bool[] Dtext = MailBnfHelper.CreateCharactersAllowedInDomainLiterals();

		// Token: 0x0400263A RID: 9786
		internal static readonly bool[] Ftext = MailBnfHelper.CreateCharactersAllowedInHeaderNames();

		// Token: 0x0400263B RID: 9787
		internal static readonly bool[] Ttext = MailBnfHelper.CreateCharactersAllowedInTokens();

		// Token: 0x0400263C RID: 9788
		internal static readonly bool[] Ctext = MailBnfHelper.CreateCharactersAllowedInComments();

		// Token: 0x0400263D RID: 9789
		internal static readonly int Ascii7bitMaxValue = 127;

		// Token: 0x0400263E RID: 9790
		internal static readonly char Quote = '"';

		// Token: 0x0400263F RID: 9791
		internal static readonly char Space = ' ';

		// Token: 0x04002640 RID: 9792
		internal static readonly char Tab = '\t';

		// Token: 0x04002641 RID: 9793
		internal static readonly char CR = '\r';

		// Token: 0x04002642 RID: 9794
		internal static readonly char LF = '\n';

		// Token: 0x04002643 RID: 9795
		internal static readonly char StartComment = '(';

		// Token: 0x04002644 RID: 9796
		internal static readonly char EndComment = ')';

		// Token: 0x04002645 RID: 9797
		internal static readonly char Backslash = '\\';

		// Token: 0x04002646 RID: 9798
		internal static readonly char At = '@';

		// Token: 0x04002647 RID: 9799
		internal static readonly char EndAngleBracket = '>';

		// Token: 0x04002648 RID: 9800
		internal static readonly char StartAngleBracket = '<';

		// Token: 0x04002649 RID: 9801
		internal static readonly char StartSquareBracket = '[';

		// Token: 0x0400264A RID: 9802
		internal static readonly char EndSquareBracket = ']';

		// Token: 0x0400264B RID: 9803
		internal static readonly char Comma = ',';

		// Token: 0x0400264C RID: 9804
		internal static readonly char Dot = '.';

		// Token: 0x0400264D RID: 9805
		private static readonly char[] s_colonSeparator = new char[] { ':' };

		// Token: 0x0400264E RID: 9806
		private static string[] s_months = new string[]
		{
			null, "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep",
			"Oct", "Nov", "Dec"
		};
	}
}
