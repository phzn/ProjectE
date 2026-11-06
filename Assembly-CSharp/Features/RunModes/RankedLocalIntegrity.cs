using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Features.RunModes
{
	// Token: 0x02000D5E RID: 3422
	public static class RankedLocalIntegrity
	{
		// Token: 0x1700106B RID: 4203
		// (get) Token: 0x0600648A RID: 25738 RVA: 0x0028363B File Offset: 0x0028183B
		private static string KeyPath
		{
			get
			{
				return Path.Combine(Application.persistentDataPath, "RankedIntegrity", "install-key-v1.bin");
			}
		}

		// Token: 0x0600648B RID: 25739 RVA: 0x00283654 File Offset: 0x00281854
		public static bool TrySign(string purpose, string payload, out string seal)
		{
			seal = null;
			if (string.IsNullOrEmpty(purpose) || payload == null)
			{
				return false;
			}
			bool flag;
			try
			{
				byte[] key = RankedLocalIntegrity.GetKey(true);
				if (key == null)
				{
					flag = false;
				}
				else
				{
					using (HMACSHA256 hmacsha = new HMACSHA256(key))
					{
						seal = Convert.ToBase64String(hmacsha.ComputeHash(Encoding.UTF8.GetBytes(purpose + "\n" + payload)));
					}
					flag = true;
				}
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is CryptographicException)
			{
				Debug.LogWarning("[Ranked] Unable to protect the local record. Online submission is disabled for this record.");
				flag = false;
			}
			return flag;
		}

		// Token: 0x0600648C RID: 25740 RVA: 0x00283714 File Offset: 0x00281914
		public static bool Verify(string purpose, string payload, string seal)
		{
			if (string.IsNullOrEmpty(purpose) || payload == null || string.IsNullOrEmpty(seal) || seal.Length != 44)
			{
				return false;
			}
			bool flag;
			try
			{
				byte[] key = RankedLocalIntegrity.GetKey(false);
				if (key == null)
				{
					flag = false;
				}
				else
				{
					byte[] array = Convert.FromBase64String(seal);
					byte[] array2;
					using (HMACSHA256 hmacsha = new HMACSHA256(key))
					{
						array2 = hmacsha.ComputeHash(Encoding.UTF8.GetBytes(purpose + "\n" + payload));
					}
					if (array.Length != array2.Length)
					{
						flag = false;
					}
					else
					{
						int num = 0;
						for (int i = 0; i < array2.Length; i++)
						{
							num |= (int)(array[i] ^ array2[i]);
						}
						flag = num == 0;
					}
				}
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is CryptographicException || ex is FormatException)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x0600648D RID: 25741 RVA: 0x00283824 File Offset: 0x00281A24
		private static byte[] GetKey(bool create)
		{
			if (RankedLocalIntegrity._key != null)
			{
				return RankedLocalIntegrity._key;
			}
			string keyPath = RankedLocalIntegrity.KeyPath;
			if (File.Exists(keyPath))
			{
				if (new FileInfo(keyPath).Length != 32L)
				{
					return null;
				}
				return RankedLocalIntegrity._key = File.ReadAllBytes(keyPath);
			}
			else
			{
				if (!create)
				{
					return null;
				}
				Directory.CreateDirectory(Path.GetDirectoryName(keyPath));
				byte[] array = new byte[32];
				using (RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create())
				{
					randomNumberGenerator.GetBytes(array);
				}
				string text = keyPath + ".tmp";
				using (FileStream fileStream = new FileStream(text, FileMode.Create, FileAccess.Write, FileShare.None))
				{
					fileStream.Write(array, 0, array.Length);
					fileStream.Flush(true);
				}
				File.Move(text, keyPath);
				return RankedLocalIntegrity._key = array;
			}
		}

		// Token: 0x0400675E RID: 26462
		private static byte[] _key;
	}
}
