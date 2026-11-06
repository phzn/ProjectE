using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MoreMountains.Tools
{
	// Token: 0x02000108 RID: 264
	public static class MMString
	{
		// Token: 0x060005B8 RID: 1464 RVA: 0x0001C0CC File Offset: 0x0001A2CC
		public static string UppercaseFirst(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return string.Empty;
			}
			return char.ToUpper(s[0]).ToString() + s.Substring(1);
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x0001C108 File Offset: 0x0001A308
		public static int RichTextLength(string richText)
		{
			int num = 0;
			bool flag = false;
			richText = richText.Replace("<br>", "-");
			foreach (char c in richText)
			{
				if (c == '<')
				{
					flag = true;
				}
				else if (c == '>')
				{
					flag = false;
				}
				else if (!flag)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x0001C163 File Offset: 0x0001A363
		public static string ToTitleCase(this string title)
		{
			return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title.ToLower());
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x0001C17A File Offset: 0x0001A37A
		public static string RemoveExtraSpaces(this string s)
		{
			return Regex.Replace(s, "\\s+", " ");
		}
	}
}
