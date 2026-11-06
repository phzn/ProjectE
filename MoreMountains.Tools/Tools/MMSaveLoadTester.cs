using System;
using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.Tools
{
	// Token: 0x02000146 RID: 326
	public class MMSaveLoadTester : MonoBehaviour
	{
		// Token: 0x06000738 RID: 1848 RVA: 0x000230D3 File Offset: 0x000212D3
		public virtual void Save()
		{
			this.InitializeSaveLoadMethod();
			MMSaveLoadManager.Save(new MMSaveLoadTestObject
			{
				SavedText = this.TargetInputField.text
			}, this.FileName + this.SaveFileExtension, this.FolderName);
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x00023110 File Offset: 0x00021310
		public virtual void Load()
		{
			this.InitializeSaveLoadMethod();
			MMSaveLoadTestObject mmsaveLoadTestObject = (MMSaveLoadTestObject)MMSaveLoadManager.Load(typeof(MMSaveLoadTestObject), this.FileName + this.SaveFileExtension, this.FolderName);
			this.TargetInputField.text = mmsaveLoadTestObject.SavedText;
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x00023160 File Offset: 0x00021360
		protected virtual void Reset()
		{
			MMSaveLoadManager.DeleteSaveFolder(this.FolderName);
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x00023170 File Offset: 0x00021370
		protected virtual void InitializeSaveLoadMethod()
		{
			switch (this.SaveLoadMethod)
			{
			case MMSaveLoadManagerMethods.Json:
				this._saveLoadManagerMethod = new MMSaveLoadManagerMethodJson();
				break;
			case MMSaveLoadManagerMethods.JsonEncrypted:
				this._saveLoadManagerMethod = new MMSaveLoadManagerMethodJsonEncrypted();
				(this._saveLoadManagerMethod as MMSaveLoadManagerEncrypter).Key = this.EncryptionKey;
				break;
			case MMSaveLoadManagerMethods.Binary:
				this._saveLoadManagerMethod = new MMSaveLoadManagerMethodBinary();
				break;
			case MMSaveLoadManagerMethods.BinaryEncrypted:
				this._saveLoadManagerMethod = new MMSaveLoadManagerMethodBinaryEncrypted();
				(this._saveLoadManagerMethod as MMSaveLoadManagerEncrypter).Key = this.EncryptionKey;
				break;
			}
			MMSaveLoadManager.SaveLoadMethod = this._saveLoadManagerMethod;
		}

		// Token: 0x040008BD RID: 2237
		[Header("Bindings")]
		[Tooltip("the text to save")]
		public InputField TargetInputField;

		// Token: 0x040008BE RID: 2238
		[Header("Save settings")]
		[Tooltip("the chosen save method (json, encrypted json, binary, encrypted binary)")]
		public MMSaveLoadManagerMethods SaveLoadMethod = MMSaveLoadManagerMethods.Binary;

		// Token: 0x040008BF RID: 2239
		[Tooltip("the name of the file to save")]
		public string FileName = "TestObject";

		// Token: 0x040008C0 RID: 2240
		[Tooltip("the name of the destination folder")]
		public string FolderName = "MMTest/";

		// Token: 0x040008C1 RID: 2241
		[Tooltip("the extension to use")]
		public string SaveFileExtension = ".testObject";

		// Token: 0x040008C2 RID: 2242
		[Tooltip("the key to use to encrypt the file (if needed)")]
		public string EncryptionKey = "ThisIsTheKey";

		// Token: 0x040008C3 RID: 2243
		[MMInspectorButton("Save")]
		public bool TestSaveButton;

		// Token: 0x040008C4 RID: 2244
		[MMInspectorButton("Load")]
		public bool TestLoadButton;

		// Token: 0x040008C5 RID: 2245
		[MMInspectorButton("Reset")]
		public bool TestResetButton;

		// Token: 0x040008C6 RID: 2246
		protected IMMSaveLoadManagerMethod _saveLoadManagerMethod;
	}
}
