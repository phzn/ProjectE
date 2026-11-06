using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace UnityEngine.UIElements
{
	// Token: 0x020002C1 RID: 705
	internal static class StringUtilsExtensions
	{
		// Token: 0x06001465 RID: 5221 RVA: 0x00048B08 File Offset: 0x00046D08
		public static string ToPascalCase(this string text)
		{
			return StringUtilsExtensions.ConvertCase(text, StringUtilsExtensions.NoDelimiter, new Func<char, char>(char.ToUpperInvariant), new Func<char, char>(char.ToUpperInvariant));
		}

		// Token: 0x06001466 RID: 5222 RVA: 0x00048B40 File Offset: 0x00046D40
		public static string ToCamelCase(this string text)
		{
			return StringUtilsExtensions.ConvertCase(text, StringUtilsExtensions.NoDelimiter, new Func<char, char>(char.ToLowerInvariant), new Func<char, char>(char.ToUpperInvariant));
		}

		// Token: 0x06001467 RID: 5223 RVA: 0x00048B78 File Offset: 0x00046D78
		public static string ToKebabCase(this string text)
		{
			return StringUtilsExtensions.ConvertCase(text, '-', new Func<char, char>(char.ToLowerInvariant), new Func<char, char>(char.ToLowerInvariant));
		}

		// Token: 0x06001468 RID: 5224 RVA: 0x00048BAC File Offset: 0x00046DAC
		public static string ToTrainCase(this string text)
		{
			return StringUtilsExtensions.ConvertCase(text, '-', new Func<char, char>(char.ToUpperInvariant), new Func<char, char>(char.ToUpperInvariant));
		}

		// Token: 0x06001469 RID: 5225 RVA: 0x00048BE0 File Offset: 0x00046DE0
		public static string ToSnakeCase(this string text)
		{
			return StringUtilsExtensions.ConvertCase(text, '_', new Func<char, char>(char.ToLowerInvariant), new Func<char, char>(char.ToLowerInvariant));
		}

		// Token: 0x0600146A RID: 5226 RVA: 0x00048C14 File Offset: 0x00046E14
		private static string ConvertCase(string text, char outputWordDelimiter, Func<char, char> startOfStringCaseHandler, Func<char, char> middleStringCaseHandler)
		{
			bool flag = text == null;
			if (flag)
			{
				throw new ArgumentNullException("text");
			}
			StringBuilder stringBuilder = new StringBuilder();
			bool flag2 = true;
			bool flag3 = true;
			bool flag4 = true;
			foreach (char c in text)
			{
				bool flag5 = StringUtilsExtensions.WordDelimiters.Contains(c);
				if (flag5)
				{
					bool flag6 = c == outputWordDelimiter;
					if (flag6)
					{
						stringBuilder.Append(outputWordDelimiter);
						flag4 = false;
					}
					flag3 = true;
				}
				else
				{
					bool flag7 = !char.IsLetterOrDigit(c);
					if (flag7)
					{
						flag2 = true;
						flag3 = true;
					}
					else
					{
						bool flag8 = flag3 || char.IsUpper(c);
						if (flag8)
						{
							bool flag9 = flag2;
							if (flag9)
							{
								stringBuilder.Append(startOfStringCaseHandler(c));
							}
							else
							{
								bool flag10 = flag4 && outputWordDelimiter != StringUtilsExtensions.NoDelimiter;
								if (flag10)
								{
									stringBuilder.Append(outputWordDelimiter);
								}
								stringBuilder.Append(middleStringCaseHandler(c));
								flag4 = true;
							}
							flag2 = false;
							flag3 = false;
						}
						else
						{
							stringBuilder.Append(c);
						}
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600146B RID: 5227 RVA: 0x00048D40 File Offset: 0x00046F40
		public static bool EndsWithIgnoreCaseFast(this string a, string b)
		{
			int num = a.Length - 1;
			int num2 = b.Length - 1;
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			while (num >= 0 && num2 >= 0 && (a[num] == b[num2] || char.ToLower(a[num], invariantCulture) == char.ToLower(b[num2], invariantCulture)))
			{
				num--;
				num2--;
			}
			return num2 < 0;
		}

		// Token: 0x0600146C RID: 5228 RVA: 0x00048DBC File Offset: 0x00046FBC
		public static bool StartsWithIgnoreCaseFast(this string a, string b)
		{
			int length = a.Length;
			int length2 = b.Length;
			int num = 0;
			int num2 = 0;
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			while (num < length && num2 < length2 && (a[num] == b[num2] || char.ToLower(a[num], invariantCulture) == char.ToLower(b[num2], invariantCulture)))
			{
				num++;
				num2++;
			}
			return num2 == length2;
		}

		// Token: 0x0400097E RID: 2430
		private static readonly char NoDelimiter = '\0';

		// Token: 0x0400097F RID: 2431
		private static readonly char[] WordDelimiters = new char[] { ' ', '-', '_' };
	}
}
