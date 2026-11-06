using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MoreMountains.Tools
{
	// Token: 0x0200013E RID: 318
	public class MMSaveLoadManagerMethodBinary : IMMSaveLoadManagerMethod
	{
		// Token: 0x06000724 RID: 1828 RVA: 0x00022D4B File Offset: 0x00020F4B
		public void Save(object objectToSave, FileStream saveFile)
		{
			new BinaryFormatter().Serialize(saveFile, objectToSave);
			saveFile.Close();
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x00022D5F File Offset: 0x00020F5F
		public object Load(Type objectType, FileStream saveFile)
		{
			object obj = new BinaryFormatter().Deserialize(saveFile);
			saveFile.Close();
			return obj;
		}
	}
}
