using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MoreMountains.Tools
{
	// Token: 0x02000144 RID: 324
	public abstract class MMSaveLoadManagerEncrypter
	{
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000732 RID: 1842 RVA: 0x00022FCC File Offset: 0x000211CC
		// (set) Token: 0x06000733 RID: 1843 RVA: 0x00022FD4 File Offset: 0x000211D4
		public virtual string Key { get; set; } = "yourDefaultKey";

		// Token: 0x06000734 RID: 1844 RVA: 0x00022FE0 File Offset: 0x000211E0
		protected virtual void Encrypt(Stream inputStream, Stream outputStream, string sKey)
		{
			RijndaelManaged rijndaelManaged = new RijndaelManaged();
			Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(sKey, Encoding.ASCII.GetBytes(this._saltText));
			rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
			rijndaelManaged.IV = rfc2898DeriveBytes.GetBytes(rijndaelManaged.BlockSize / 8);
			new CryptoStream(inputStream, rijndaelManaged.CreateEncryptor(), CryptoStreamMode.Read).CopyTo(outputStream);
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x00023048 File Offset: 0x00021248
		protected virtual void Decrypt(Stream inputStream, Stream outputStream, string sKey)
		{
			RijndaelManaged rijndaelManaged = new RijndaelManaged();
			Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(sKey, Encoding.ASCII.GetBytes(this._saltText));
			rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
			rijndaelManaged.IV = rfc2898DeriveBytes.GetBytes(rijndaelManaged.BlockSize / 8);
			new CryptoStream(inputStream, rijndaelManaged.CreateDecryptor(), CryptoStreamMode.Read).CopyTo(outputStream);
		}

		// Token: 0x040008BB RID: 2235
		protected string _saltText = "SaltTextGoesHere";
	}
}
