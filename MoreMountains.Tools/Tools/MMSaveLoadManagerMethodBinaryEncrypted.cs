using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200013F RID: 319
	public class MMSaveLoadManagerMethodBinaryEncrypted : MMSaveLoadManagerEncrypter, IMMSaveLoadManagerMethod
	{
		// Token: 0x06000727 RID: 1831 RVA: 0x00022D7C File Offset: 0x00020F7C
		public void Save(object objectToSave, FileStream saveFile)
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			MemoryStream memoryStream = new MemoryStream();
			binaryFormatter.Serialize(memoryStream, objectToSave);
			memoryStream.Position = 0L;
			this.Encrypt(memoryStream, saveFile, this.Key);
			saveFile.Flush();
			memoryStream.Close();
			saveFile.Close();
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x00022DC4 File Offset: 0x00020FC4
		public object Load(Type objectType, FileStream saveFile)
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			MemoryStream memoryStream = new MemoryStream();
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
			object obj = binaryFormatter.Deserialize(memoryStream);
			memoryStream.Close();
			saveFile.Close();
			return obj;
		}
	}
}
