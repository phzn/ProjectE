using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace DA_Assets.Extensions
{
	// Token: 0x02000035 RID: 53
	public static class StringExtensions
	{
		// Token: 0x060000F4 RID: 244 RVA: 0x0000576C File Offset: 0x0000396C
		public static string RemovePathExtension(this string filePath)
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
			string directoryName = Path.GetDirectoryName(filePath);
			if (!string.IsNullOrEmpty(directoryName))
			{
				return Path.Combine(directoryName, fileNameWithoutExtension).Replace('\\', '/');
			}
			return fileNameWithoutExtension;
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x000057A4 File Offset: 0x000039A4
		public static bool IsValidEmail(this string emailaddress)
		{
			bool flag;
			try
			{
				new MailAddress(emailaddress);
				flag = true;
			}
			catch (FormatException)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x000057D4 File Offset: 0x000039D4
		public static bool IsSpecialNumber(this string value)
		{
			float num;
			return !string.IsNullOrWhiteSpace(value) && Regex.Replace(value, "[\\%\\#\\$\\€\\£\\₴\\¥\\₹\\₩\\₪\\₦\\฿\\₫\\₹\\₺\\₱\\₽]", "").Trim().TryParseFloat(out num);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00005807 File Offset: 0x00003A07
		public static bool IsPhoneNumber(this string value)
		{
			return !string.IsNullOrWhiteSpace(value) && Regex.IsMatch(value, "^\\+?[0-9\\s\\-\\(\\)]+$");
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00005820 File Offset: 0x00003A20
		public static string MakeCharLower(this string str, int charIndex)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			if (charIndex < 0 || charIndex >= str.Length)
			{
				throw new ArgumentOutOfRangeException("charIndex");
			}
			char[] array = str.ToCharArray();
			array[charIndex] = char.ToLower(array[charIndex]);
			return new string(array);
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x0000586C File Offset: 0x00003A6C
		public static string MakeCharUpper(this string str, int charIndex)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			if (charIndex < 0 || charIndex >= str.Length)
			{
				throw new ArgumentOutOfRangeException("charIndex");
			}
			char[] array = str.ToCharArray();
			array[charIndex] = char.ToUpper(array[charIndex]);
			return new string(array);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x000058B8 File Offset: 0x00003AB8
		public static string MakeFirstCharUpper(this string str)
		{
			return char.ToLower(str[0]).ToString() + str.Substring(1);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x000058E8 File Offset: 0x00003AE8
		public static long GetNumberOnly(this string str)
		{
			long num;
			if (long.TryParse(Regex.Replace(str, "[^\\d]", ""), out num))
			{
				return num;
			}
			return 0L;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00005914 File Offset: 0x00003B14
		public static string ReplaceNotNumbers(this string str, char @char)
		{
			char[] array = str.ToCharArray();
			for (int i = 0; i < array.Length; i++)
			{
				if (!char.IsDigit(array[i]))
				{
					array[i] = @char;
				}
			}
			return new string(array);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x0000594A File Offset: 0x00003B4A
		public static string RemoveRepeats(this string str, char @char)
		{
			if (str.IsEmpty())
			{
				return str;
			}
			return Regex.Replace(str, string.Format("{0}+", @char), string.Format("{0}", @char));
		}

		// Token: 0x060000FE RID: 254 RVA: 0x0000597C File Offset: 0x00003B7C
		public static string RemoveRepeatsForNonAlphanumericCharacters(this string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return str;
			}
			return Regex.Replace(str, "([^a-zA-Z0-9])\\1+", "$1");
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00005998 File Offset: 0x00003B98
		public static float GetSizeMB(this string myString)
		{
			float num;
			using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(myString)))
			{
				num = (float)Math.Floor((double)((float)memoryStream.Length / 1048576f * 100f)) / 100f;
			}
			return num;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x000059F4 File Offset: 0x00003BF4
		public static string AddWithNewLine(this object source)
		{
			return "" + Environment.NewLine + ((source != null) ? source.ToString() : null);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00005A18 File Offset: 0x00003C18
		public static bool IsLastChar(this string str, Func<char, bool> condition)
		{
			if (string.IsNullOrWhiteSpace(str))
			{
				return false;
			}
			char c = str.Last<char>();
			return condition(c);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00005A40 File Offset: 0x00003C40
		public static bool IsAllUpper(this string input)
		{
			input = input.RemoveNonLettersAndNonNumbers("");
			for (int i = 0; i < input.Length; i++)
			{
				if (!char.IsUpper(input[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00005A7C File Offset: 0x00003C7C
		public static string RemoveNonNumbers(this string str, string replace)
		{
			if (string.IsNullOrWhiteSpace(str))
			{
				return str;
			}
			return Regex.Replace(str, "[^0-9]", replace);
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00005A94 File Offset: 0x00003C94
		public static string RemoveNonLettersAndNonNumbers(this string str, string replace)
		{
			if (string.IsNullOrWhiteSpace(str))
			{
				return str;
			}
			return Regex.Replace(str, "[^a-zA-Z0-9]", replace);
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00005AAC File Offset: 0x00003CAC
		public static bool IsLetterOrNumber(this char c)
		{
			return Regex.IsMatch(c.ToString(), "[a-zA-Z0-9]");
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00005AC0 File Offset: 0x00003CC0
		public static string ToSnakeCase(this string str)
		{
			if (string.IsNullOrWhiteSpace(str))
			{
				return str;
			}
			if (str.Length < 2)
			{
				return str.ToLowerInvariant();
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(char.ToLowerInvariant(str[0]));
			for (int i = 1; i < str.Length; i++)
			{
				char c = str[i];
				if (char.IsUpper(c))
				{
					stringBuilder.Append('_');
					stringBuilder.Append(char.ToLowerInvariant(c));
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00005B48 File Offset: 0x00003D48
		public static string ToUpperSnakeCase(this string str)
		{
			string text = str.ToSnakeCase();
			if (text.IsEmpty())
			{
				return text;
			}
			return text.ToUpper();
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00005B6C File Offset: 0x00003D6C
		public static string ToPascalCase(this string str)
		{
			Regex regex = new Regex("[^_a-zA-Z0-9]");
			Regex regex2 = new Regex("(?<=\\s)");
			Regex startsWithLowerCaseChar = new Regex("^[a-z]");
			Regex firstCharFollowedByUpperCasesOnly = new Regex("(?<=[A-Z])[A-Z0-9]+$");
			Regex lowerCaseNextToNumber = new Regex("(?<=[0-9])[a-z]");
			Regex upperCaseInside = new Regex("(?<=[A-Z])[A-Z]+?((?=[A-Z][a-z])|(?=[0-9]))");
			return string.Concat(from w in regex.Replace(regex2.Replace(str, "_"), string.Empty).Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries)
				select startsWithLowerCaseChar.Replace(w, (Match m) => m.Value.ToUpper()) into w
				select firstCharFollowedByUpperCasesOnly.Replace(w, (Match m) => m.Value.ToLower()) into w
				select lowerCaseNextToNumber.Replace(w, (Match m) => m.Value.ToUpper()) into w
				select upperCaseInside.Replace(w, (Match m) => m.Value.ToLower()));
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00005C44 File Offset: 0x00003E44
		public static string ToPascalSnakeCase(this string original)
		{
			string[] array = (from word in Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(original, "[^-_a-zA-Z0-9]", " "), "([a-z])([A-Z])", "$1 $2"), "([0-9])([a-zA-Z])", "$1 $2"), "([a-zA-Z])([0-9])", "$1 $2").Replace("-", " ").Replace("_", " "), "\\s+", " ").Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
				select CultureInfo.InvariantCulture.TextInfo.ToTitleCase(word.ToLowerInvariant())).ToArray<string>();
			return string.Join("_", array);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00005D06 File Offset: 0x00003F06
		public static bool IsEmpty(this string value)
		{
			return string.IsNullOrWhiteSpace(value);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00005D0E File Offset: 0x00003F0E
		public static string SubstringSafe(this string value, int maxLength)
		{
			if (value == null || value.Length <= maxLength)
			{
				return value;
			}
			return value.Substring(0, maxLength);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00005D26 File Offset: 0x00003F26
		public static string RemoveHTML(this string text)
		{
			return Regex.Replace(text, "<.*?>", string.Empty);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00005D38 File Offset: 0x00003F38
		public static string RemoveBetween(this string text, string startTag, string endTag)
		{
			return new Regex(string.Format("{0}(.*?){1}", Regex.Escape(startTag), Regex.Escape(endTag)), RegexOptions.RightToLeft).Replace(text, startTag + endTag);
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00005D64 File Offset: 0x00003F64
		public static string GetBetween(this string text, string startTag, string endTag)
		{
			int num = text.IndexOf(startTag) + startTag.Length;
			int num2 = text.LastIndexOf(endTag);
			return text.Substring(num, num2 - num);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00005D92 File Offset: 0x00003F92
		public static string[] SplitByString(this string text, string separator)
		{
			return text.Split(new string[] { separator }, StringSplitOptions.None);
		}

		// Token: 0x04000066 RID: 102
		private const string phonePattern = "^\\+?[0-9\\s\\-\\(\\)]+$";
	}
}
