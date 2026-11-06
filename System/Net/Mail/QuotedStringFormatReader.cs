using System;
using System.Net.Mime;

namespace System.Net.Mail
{
	// Token: 0x020007EF RID: 2031
	internal static class QuotedStringFormatReader
	{
		// Token: 0x060040FA RID: 16634 RVA: 0x000DEB90 File Offset: 0x000DCD90
		internal static int ReadReverseQuoted(string data, int index, bool permitUnicode)
		{
			index--;
			for (;;)
			{
				index = WhitespaceReader.ReadFwsReverse(data, index);
				if (index < 0)
				{
					goto IL_006C;
				}
				int num = QuotedPairReader.CountQuotedChars(data, index, permitUnicode);
				if (num > 0)
				{
					index -= num;
				}
				else
				{
					if (data[index] == MailBnfHelper.Quote)
					{
						break;
					}
					if (!QuotedStringFormatReader.IsValidQtext(permitUnicode, data[index]))
					{
						goto Block_4;
					}
					index--;
				}
				if (index < 0)
				{
					goto IL_006C;
				}
			}
			return index - 1;
			Block_4:
			throw new FormatException(SR.Format("An invalid character was found in the mail header: '{0}'.", data[index]));
			IL_006C:
			throw new FormatException(SR.Format("An invalid character was found in the mail header: '{0}'.", MailBnfHelper.Quote));
		}

		// Token: 0x060040FB RID: 16635 RVA: 0x000DEC24 File Offset: 0x000DCE24
		internal static int ReadReverseUnQuoted(string data, int index, bool permitUnicode, bool expectCommaDelimiter)
		{
			for (;;)
			{
				index = WhitespaceReader.ReadFwsReverse(data, index);
				if (index < 0)
				{
					return index;
				}
				int num = QuotedPairReader.CountQuotedChars(data, index, permitUnicode);
				if (num > 0)
				{
					index -= num;
				}
				else
				{
					if (expectCommaDelimiter && data[index] == MailBnfHelper.Comma)
					{
						return index;
					}
					if (!QuotedStringFormatReader.IsValidQtext(permitUnicode, data[index]))
					{
						break;
					}
					index--;
				}
				if (index < 0)
				{
					return index;
				}
			}
			throw new FormatException(SR.Format("An invalid character was found in the mail header: '{0}'.", data[index]));
		}

		// Token: 0x060040FC RID: 16636 RVA: 0x000DEC98 File Offset: 0x000DCE98
		private static bool IsValidQtext(bool allowUnicode, char ch)
		{
			if ((int)ch > MailBnfHelper.Ascii7bitMaxValue)
			{
				return allowUnicode;
			}
			return MailBnfHelper.Qtext[(int)ch];
		}
	}
}
