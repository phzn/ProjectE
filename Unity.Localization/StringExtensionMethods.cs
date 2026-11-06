using System;
using System.Text.RegularExpressions;

namespace UnityEngine.Localization
{
	// Token: 0x02000025 RID: 37
	internal static class StringExtensionMethods
	{
		// Token: 0x06000111 RID: 273 RVA: 0x000049D7 File Offset: 0x00002BD7
		public static string ReplaceWhiteSpaces(this string str, string replacement = "")
		{
			return StringExtensionMethods.s_WhitespaceRegex.Replace(str, replacement);
		}

		// Token: 0x04000041 RID: 65
		private static readonly Regex s_WhitespaceRegex = new Regex("\\s+");
	}
}
