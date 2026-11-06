using System;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000141 RID: 321
	public class MMSaveLoadManagerMethodJsonEncrypted : MMSaveLoadManagerEncrypter, IMMSaveLoadManagerMethod
	{
		// Token: 0x0600072D RID: 1837 RVA: 0x00022EA4 File Offset: 0x000210A4
		public void Save(object objectToSave, FileStream saveFile)
		{
			string text = JsonUtility.ToJson(objectToSave);
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (StreamWriter streamWriter = new StreamWriter(memoryStream))
				{
					streamWriter.Write(text);
					streamWriter.Flush();
					memoryStream.Position = 0L;
					this.Encrypt(memoryStream, saveFile, this.Key);
				}
			}
			saveFile.Close();
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x00022F20 File Offset: 0x00021120
		public object Load(Type objectType, FileStream saveFile)
		{
			object obj = null;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (StreamReader streamReader = new StreamReader(memoryStream))
				{
					try
					{
						this.Decrypt(saveFile, memoryStream, this.Key);
					}
					catch (CryptographicException ex)
					{
						Debug.LogError("[MMSaveLoadManager] Encryption key error: " + ex.Message);
						return null;
					}
					memoryStream.Position = 0L;
					obj = JsonUtility.FromJson(streamReader.ReadToEnd(), objectType);
				}
			}
			saveFile.Close();
			return obj;
		}
	}
}
