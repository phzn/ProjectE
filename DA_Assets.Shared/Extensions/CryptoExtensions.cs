using System;
using System.Security.Cryptography;
using System.Text;

namespace DA_Assets.Extensions
{
	// Token: 0x0200002A RID: 42
	public static class CryptoExtensions
	{
		// Token: 0x0600008B RID: 139 RVA: 0x0000405C File Offset: 0x0000225C
		public static string CreateShortGuid(this string value, out string result)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				value = Guid.NewGuid().ToString().Split('-', StringSplitOptions.None)[0];
			}
			result = value;
			return value;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00004094 File Offset: 0x00002294
		public static byte[] GetSHA1Hash(this string value)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(value);
			byte[] array;
			using (SHA1 sha = SHA1.Create())
			{
				array = sha.ComputeHash(bytes);
			}
			return array;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000040D8 File Offset: 0x000022D8
		public static string ToHEX(this byte[] bytes)
		{
			StringBuilder stringBuilder = new StringBuilder(bytes.Length * 2);
			foreach (byte b in bytes)
			{
				stringBuilder.Append(b.ToString("X2"));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00004120 File Offset: 0x00002320
		public static string Base64Decode(this string base64EncodedData)
		{
			byte[] array = Convert.FromBase64String(base64EncodedData);
			return Encoding.UTF8.GetString(array);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x0000413F File Offset: 0x0000233F
		public static string Base64Encode(this string plainText)
		{
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
		}
	}
}
