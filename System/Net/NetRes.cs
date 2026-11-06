using System;
using System.Globalization;

namespace System.Net
{
	// Token: 0x02000631 RID: 1585
	internal class NetRes
	{
		// Token: 0x0600321C RID: 12828 RVA: 0x0000219B File Offset: 0x0000039B
		private NetRes()
		{
		}

		// Token: 0x0600321D RID: 12829 RVA: 0x000AD738 File Offset: 0x000AB938
		public static string GetWebStatusString(string Res, WebExceptionStatus Status)
		{
			string @string = SR.GetString(WebExceptionMapping.GetWebStatusString(Status));
			string string2 = SR.GetString(Res);
			return string.Format(CultureInfo.CurrentCulture, string2, @string);
		}

		// Token: 0x0600321E RID: 12830 RVA: 0x000AD764 File Offset: 0x000AB964
		public static string GetWebStatusString(WebExceptionStatus Status)
		{
			return SR.GetString(WebExceptionMapping.GetWebStatusString(Status));
		}

		// Token: 0x0600321F RID: 12831 RVA: 0x000AD774 File Offset: 0x000AB974
		public static string GetWebStatusCodeString(HttpStatusCode statusCode, string statusDescription)
		{
			string text = "(";
			int num = (int)statusCode;
			string text2 = text + num.ToString(NumberFormatInfo.InvariantInfo) + ")";
			string text3 = null;
			try
			{
				text3 = SR.GetString("net_httpstatuscode_" + statusCode.ToString(), null);
			}
			catch
			{
			}
			if (text3 != null && text3.Length > 0)
			{
				text2 = text2 + " " + text3;
			}
			else if (statusDescription != null && statusDescription.Length > 0)
			{
				text2 = text2 + " " + statusDescription;
			}
			return text2;
		}

		// Token: 0x06003220 RID: 12832 RVA: 0x000AD80C File Offset: 0x000ABA0C
		public static string GetWebStatusCodeString(FtpStatusCode statusCode, string statusDescription)
		{
			string text = "(";
			int num = (int)statusCode;
			string text2 = text + num.ToString(NumberFormatInfo.InvariantInfo) + ")";
			string text3 = null;
			try
			{
				text3 = SR.GetString("net_ftpstatuscode_" + statusCode.ToString(), null);
			}
			catch
			{
			}
			if (text3 != null && text3.Length > 0)
			{
				text2 = text2 + " " + text3;
			}
			else if (statusDescription != null && statusDescription.Length > 0)
			{
				text2 = text2 + " " + statusDescription;
			}
			return text2;
		}
	}
}
