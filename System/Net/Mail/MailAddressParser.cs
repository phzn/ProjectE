using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;

namespace System.Net.Mail
{
	// Token: 0x020007ED RID: 2029
	internal static class MailAddressParser
	{
		// Token: 0x060040F0 RID: 16624 RVA: 0x000DE798 File Offset: 0x000DC998
		internal static MailAddress ParseAddress(string data)
		{
			int num = data.Length - 1;
			return MailAddressParser.ParseAddress(data, false, ref num);
		}

		// Token: 0x060040F1 RID: 16625 RVA: 0x000DE7B8 File Offset: 0x000DC9B8
		internal static List<MailAddress> ParseMultipleAddresses(string data)
		{
			List<MailAddress> list = new List<MailAddress>();
			for (int i = data.Length - 1; i >= 0; i--)
			{
				list.Insert(0, MailAddressParser.ParseAddress(data, true, ref i));
			}
			return list;
		}

		// Token: 0x060040F2 RID: 16626 RVA: 0x000DE7F0 File Offset: 0x000DC9F0
		private static MailAddress ParseAddress(string data, bool expectMultipleAddresses, ref int index)
		{
			index = MailAddressParser.ReadCfwsAndThrowIfIncomplete(data, index);
			bool flag = false;
			if (data[index] == MailBnfHelper.EndAngleBracket)
			{
				flag = true;
				index--;
			}
			string text = MailAddressParser.ParseDomain(data, ref index);
			if (data[index] != MailBnfHelper.At)
			{
				throw new FormatException("The specified string is not in the form required for an e-mail address.");
			}
			index--;
			string text2 = MailAddressParser.ParseLocalPart(data, ref index, flag, expectMultipleAddresses);
			if (flag)
			{
				if (index < 0 || data[index] != MailBnfHelper.StartAngleBracket)
				{
					throw new FormatException(SR.Format("An invalid character was found in the mail header: '{0}'.", (index >= 0) ? data[index] : MailBnfHelper.EndAngleBracket));
				}
				index--;
				index = WhitespaceReader.ReadFwsReverse(data, index);
			}
			string text3;
			if (index >= 0 && (!expectMultipleAddresses || data[index] != MailBnfHelper.Comma))
			{
				text3 = MailAddressParser.ParseDisplayName(data, ref index, expectMultipleAddresses);
			}
			else
			{
				text3 = string.Empty;
			}
			return new MailAddress(text3, text2, text);
		}

		// Token: 0x060040F3 RID: 16627 RVA: 0x000DE8DA File Offset: 0x000DCADA
		private static int ReadCfwsAndThrowIfIncomplete(string data, int index)
		{
			index = WhitespaceReader.ReadCfwsReverse(data, index);
			if (index < 0)
			{
				throw new FormatException("The specified string is not in the form required for an e-mail address.");
			}
			return index;
		}

		// Token: 0x060040F4 RID: 16628 RVA: 0x000DE8F8 File Offset: 0x000DCAF8
		private static string ParseDomain(string data, ref int index)
		{
			index = MailAddressParser.ReadCfwsAndThrowIfIncomplete(data, index);
			int num = index;
			if (data[index] == MailBnfHelper.EndSquareBracket)
			{
				index = DomainLiteralReader.ReadReverse(data, index);
			}
			else
			{
				index = DotAtomReader.ReadReverse(data, index);
			}
			string text = data.Substring(index + 1, num - index);
			index = MailAddressParser.ReadCfwsAndThrowIfIncomplete(data, index);
			return MailAddressParser.NormalizeOrThrow(text);
		}

		// Token: 0x060040F5 RID: 16629 RVA: 0x000DE954 File Offset: 0x000DCB54
		private static string ParseLocalPart(string data, ref int index, bool expectAngleBracket, bool expectMultipleAddresses)
		{
			index = MailAddressParser.ReadCfwsAndThrowIfIncomplete(data, index);
			int num = index;
			if (data[index] == MailBnfHelper.Quote)
			{
				index = QuotedStringFormatReader.ReadReverseQuoted(data, index, true);
			}
			else
			{
				index = DotAtomReader.ReadReverse(data, index);
				if (index >= 0 && !MailBnfHelper.IsAllowedWhiteSpace(data[index]) && data[index] != MailBnfHelper.EndComment && (!expectAngleBracket || data[index] != MailBnfHelper.StartAngleBracket) && (!expectMultipleAddresses || data[index] != MailBnfHelper.Comma) && data[index] != MailBnfHelper.Quote)
				{
					throw new FormatException(SR.Format("An invalid character was found in the mail header: '{0}'.", data[index]));
				}
			}
			string text = data.Substring(index + 1, num - index);
			index = WhitespaceReader.ReadCfwsReverse(data, index);
			return MailAddressParser.NormalizeOrThrow(text);
		}

		// Token: 0x060040F6 RID: 16630 RVA: 0x000DEA24 File Offset: 0x000DCC24
		private static string ParseDisplayName(string data, ref int index, bool expectMultipleAddresses)
		{
			int num = WhitespaceReader.ReadCfwsReverse(data, index);
			string text;
			if (num >= 0 && data[num] == MailBnfHelper.Quote)
			{
				index = QuotedStringFormatReader.ReadReverseQuoted(data, num, true);
				int num2 = index + 2;
				text = data.Substring(num2, num - num2);
				index = WhitespaceReader.ReadCfwsReverse(data, index);
				if (index >= 0 && (!expectMultipleAddresses || data[index] != MailBnfHelper.Comma))
				{
					throw new FormatException(SR.Format("An invalid character was found in the mail header: '{0}'.", data[index]));
				}
			}
			else
			{
				int num3 = index;
				index = QuotedStringFormatReader.ReadReverseUnQuoted(data, index, true, expectMultipleAddresses);
				text = data.SubstringTrim(index + 1, num3 - index);
			}
			return MailAddressParser.NormalizeOrThrow(text);
		}

		// Token: 0x060040F7 RID: 16631 RVA: 0x000DEAC8 File Offset: 0x000DCCC8
		internal static string NormalizeOrThrow(string input)
		{
			string text;
			try
			{
				text = input.Normalize(NormalizationForm.FormC);
			}
			catch (ArgumentException ex)
			{
				throw new FormatException("The specified string is not in the form required for an e-mail address.", ex);
			}
			return text;
		}
	}
}
