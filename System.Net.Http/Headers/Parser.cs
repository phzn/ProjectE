using System;
using System.Globalization;
using System.Net.Mail;

namespace System.Net.Http.Headers
{
	// Token: 0x02000054 RID: 84
	internal static class Parser
	{
		// Token: 0x02000055 RID: 85
		public static class Token
		{
			// Token: 0x06000342 RID: 834 RVA: 0x0000B89E File Offset: 0x00009A9E
			public static bool TryParse(string input, out string result)
			{
				if (input != null && Lexer.IsValidToken(input))
				{
					result = input;
					return true;
				}
				result = null;
				return false;
			}

			// Token: 0x06000343 RID: 835 RVA: 0x0000B8B4 File Offset: 0x00009AB4
			public static void Check(string s)
			{
				if (s == null)
				{
					throw new ArgumentNullException();
				}
				if (Lexer.IsValidToken(s))
				{
					return;
				}
				if (s.Length == 0)
				{
					throw new ArgumentException();
				}
				throw new FormatException(s);
			}

			// Token: 0x06000344 RID: 836 RVA: 0x0000B8DC File Offset: 0x00009ADC
			public static bool TryCheck(string s)
			{
				return s != null && Lexer.IsValidToken(s);
			}

			// Token: 0x06000345 RID: 837 RVA: 0x0000B8EC File Offset: 0x00009AEC
			public static void CheckQuotedString(string s)
			{
				if (s == null)
				{
					throw new ArgumentNullException();
				}
				Lexer lexer = new Lexer(s);
				if (lexer.Scan(false) == global::System.Net.Http.Headers.Token.Type.QuotedString && lexer.Scan(false) == global::System.Net.Http.Headers.Token.Type.End)
				{
					return;
				}
				if (s.Length == 0)
				{
					throw new ArgumentException();
				}
				throw new FormatException(s);
			}

			// Token: 0x06000346 RID: 838 RVA: 0x0000B93C File Offset: 0x00009B3C
			public static void CheckComment(string s)
			{
				if (s == null)
				{
					throw new ArgumentNullException();
				}
				string text;
				if (new Lexer(s).ScanCommentOptional(out text))
				{
					return;
				}
				if (s.Length == 0)
				{
					throw new ArgumentException();
				}
				throw new FormatException(s);
			}
		}

		// Token: 0x02000056 RID: 86
		public static class DateTime
		{
			// Token: 0x06000347 RID: 839 RVA: 0x0000B976 File Offset: 0x00009B76
			public static bool TryParse(string input, out DateTimeOffset result)
			{
				return Lexer.TryGetDateValue(input, out result);
			}

			// Token: 0x0400013A RID: 314
			public new static readonly Func<object, string> ToString = (object l) => ((DateTimeOffset)l).ToString("r", CultureInfo.InvariantCulture);
		}

		// Token: 0x02000058 RID: 88
		public static class EmailAddress
		{
			// Token: 0x0600034C RID: 844 RVA: 0x0000B9CC File Offset: 0x00009BCC
			public static bool TryParse(string input, out string result)
			{
				bool flag;
				try
				{
					new MailAddress(input);
					result = input;
					flag = true;
				}
				catch
				{
					result = null;
					flag = false;
				}
				return flag;
			}
		}

		// Token: 0x02000059 RID: 89
		public static class Host
		{
			// Token: 0x0600034D RID: 845 RVA: 0x0000BA00 File Offset: 0x00009C00
			public static bool TryParse(string input, out string result)
			{
				result = input;
				global::System.Uri uri;
				return global::System.Uri.TryCreate("http://u@" + input + "/", UriKind.Absolute, out uri);
			}
		}

		// Token: 0x0200005A RID: 90
		public static class Int
		{
			// Token: 0x0600034E RID: 846 RVA: 0x0000BA28 File Offset: 0x00009C28
			public static bool TryParse(string input, out int result)
			{
				return int.TryParse(input, NumberStyles.None, CultureInfo.InvariantCulture, out result);
			}
		}

		// Token: 0x0200005B RID: 91
		public static class Long
		{
			// Token: 0x0600034F RID: 847 RVA: 0x0000BA37 File Offset: 0x00009C37
			public static bool TryParse(string input, out long result)
			{
				return long.TryParse(input, NumberStyles.None, CultureInfo.InvariantCulture, out result);
			}
		}

		// Token: 0x0200005C RID: 92
		public static class MD5
		{
			// Token: 0x06000350 RID: 848 RVA: 0x0000BA48 File Offset: 0x00009C48
			public static bool TryParse(string input, out byte[] result)
			{
				bool flag;
				try
				{
					result = Convert.FromBase64String(input);
					flag = true;
				}
				catch
				{
					result = null;
					flag = false;
				}
				return flag;
			}

			// Token: 0x0400013C RID: 316
			public new static readonly Func<object, string> ToString = (object l) => Convert.ToBase64String((byte[])l);
		}

		// Token: 0x0200005E RID: 94
		public static class TimeSpanSeconds
		{
			// Token: 0x06000355 RID: 853 RVA: 0x0000BAAC File Offset: 0x00009CAC
			public static bool TryParse(string input, out TimeSpan result)
			{
				int num;
				if (Parser.Int.TryParse(input, out num))
				{
					result = TimeSpan.FromSeconds((double)num);
					return true;
				}
				result = TimeSpan.Zero;
				return false;
			}
		}

		// Token: 0x0200005F RID: 95
		public static class Uri
		{
			// Token: 0x06000356 RID: 854 RVA: 0x0000BADE File Offset: 0x00009CDE
			public static bool TryParse(string input, out global::System.Uri result)
			{
				return global::System.Uri.TryCreate(input, UriKind.RelativeOrAbsolute, out result);
			}

			// Token: 0x06000357 RID: 855 RVA: 0x0000BAE8 File Offset: 0x00009CE8
			public static void Check(string s)
			{
				if (s == null)
				{
					throw new ArgumentNullException();
				}
				global::System.Uri uri;
				if (Parser.Uri.TryParse(s, out uri))
				{
					return;
				}
				if (s.Length == 0)
				{
					throw new ArgumentException();
				}
				throw new FormatException(s);
			}
		}
	}
}
