using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000140 RID: 320
	public class MMSaveLoadManagerMethodJson : IMMSaveLoadManagerMethod
	{
		// Token: 0x0600072A RID: 1834 RVA: 0x00022E3C File Offset: 0x0002103C
		public void Save(object objectToSave, FileStream saveFile)
		{
			string text = JsonUtility.ToJson(objectToSave);
			StreamWriter streamWriter = new StreamWriter(saveFile);
			streamWriter.Write(text);
			streamWriter.Close();
			saveFile.Close();
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x00022E68 File Offset: 0x00021068
		public object Load(Type objectType, FileStream saveFile)
		{
			StreamReader streamReader = new StreamReader(saveFile, Encoding.UTF8);
			object obj = JsonUtility.FromJson(streamReader.ReadToEnd(), objectType);
			streamReader.Close();
			saveFile.Close();
			return obj;
		}
	}
}
