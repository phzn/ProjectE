using System;
using System.Net.Mime;

namespace System.Net.Mail
{
	// Token: 0x020007EC RID: 2028
	internal static class DotAtomReader
	{
		// Token: 0x060040EF RID: 16623 RVA: 0x000DE708 File Offset: 0x000DC908
		internal static int ReadReverse(string data, int index)
		{
			int num = index;
			while (0 <= index && ((int)data[index] > MailBnfHelper.Ascii7bitMaxValue || data[index] == MailBnfHelper.Dot || MailBnfHelper.Atext[(int)data[index]]))
			{
				index--;
			}
			if (num == index)
			{
				throw new FormatException(SR.Format("An invalid character was found in the mail header: '{0}'.", data[index]));
			}
			if (data[index + 1] == MailBnfHelper.Dot)
			{
				throw new FormatException(SR.Format("An invalid character was found in the mail header: '{0}'.", MailBnfHelper.Dot));
			}
			return index;
		}
	}
}
