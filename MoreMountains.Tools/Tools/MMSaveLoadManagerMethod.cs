using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200013D RID: 317
	public class MMSaveLoadManagerMethod : MonoBehaviour
	{
		// Token: 0x06000721 RID: 1825 RVA: 0x00022C91 File Offset: 0x00020E91
		protected virtual void Awake()
		{
			this.SetSaveLoadMethod();
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x00022C9C File Offset: 0x00020E9C
		public virtual void SetSaveLoadMethod()
		{
			switch (this.SaveLoadMethod)
			{
			case MMSaveLoadManagerMethods.Json:
				this._saveLoadManagerMethod = new MMSaveLoadManagerMethodJson();
				break;
			case MMSaveLoadManagerMethods.JsonEncrypted:
				this._saveLoadManagerMethod = new MMSaveLoadManagerMethodJsonEncrypted();
				((MMSaveLoadManagerEncrypter)this._saveLoadManagerMethod).Key = this.EncryptionKey;
				break;
			case MMSaveLoadManagerMethods.Binary:
				this._saveLoadManagerMethod = new MMSaveLoadManagerMethodBinary();
				break;
			case MMSaveLoadManagerMethods.BinaryEncrypted:
				this._saveLoadManagerMethod = new MMSaveLoadManagerMethodBinaryEncrypted();
				((MMSaveLoadManagerEncrypter)this._saveLoadManagerMethod).Key = this.EncryptionKey;
				break;
			}
			MMSaveLoadManager.SaveLoadMethod = this._saveLoadManagerMethod;
		}

		// Token: 0x040008B2 RID: 2226
		[Header("Save and load method")]
		[MMInformation("This component, on Awake or on demand, will force a SaveLoadMethod on the MMSaveLoadManager, changing the way it saves data to file. This will impact all classes that use the MMSaveLoadManager (unless they change that method before saving or loading).If you change the method, your previously existing data files won't be compatible, you'll need to delete them and start with new ones.", MMInformationAttribute.InformationType.Info, false)]
		[Tooltip("the method to use to save to file")]
		public MMSaveLoadManagerMethods SaveLoadMethod = MMSaveLoadManagerMethods.Binary;

		// Token: 0x040008B3 RID: 2227
		[Tooltip("the key to use to encrypt the file (if using an encryption method)")]
		public string EncryptionKey = "ThisIsTheKey";

		// Token: 0x040008B4 RID: 2228
		protected IMMSaveLoadManagerMethod _saveLoadManagerMethod;
	}
}
