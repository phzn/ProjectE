using System;
using System.IO;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200013C RID: 316
	public static class MMSaveLoadManager
	{
		// Token: 0x06000718 RID: 1816 RVA: 0x00022A50 File Offset: 0x00020C50
		private static string DetermineSavePath(string folderName = "MMSaveLoadManager")
		{
			string text;
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				text = Application.persistentDataPath + "/MMData/";
			}
			else
			{
				text = Application.persistentDataPath + "/MMData/";
			}
			return text + folderName + "/";
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x00022A95 File Offset: 0x00020C95
		private static string DetermineSaveFileName(string fileName)
		{
			return fileName;
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x00022A98 File Offset: 0x00020C98
		public static void Save(object saveObject, string fileName, string foldername = "MMSaveLoadManager")
		{
			string text = MMSaveLoadManager.DetermineSavePath(foldername);
			string text2 = MMSaveLoadManager.DetermineSaveFileName(fileName);
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			FileStream fileStream = File.Create(text + text2);
			MMSaveLoadManager.SaveLoadMethod.Save(saveObject, fileStream);
			fileStream.Close();
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x00022AE4 File Offset: 0x00020CE4
		public static object Load(Type objectType, string fileName, string foldername = "MMSaveLoadManager")
		{
			string text = MMSaveLoadManager.DetermineSavePath(foldername);
			string text2 = text + MMSaveLoadManager.DetermineSaveFileName(fileName);
			if (!Directory.Exists(text) || !File.Exists(text2))
			{
				return null;
			}
			FileStream fileStream = File.Open(text2, FileMode.Open, FileAccess.Read, FileShare.Read);
			object obj = MMSaveLoadManager.SaveLoadMethod.Load(objectType, fileStream);
			fileStream.Close();
			return obj;
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x00022B34 File Offset: 0x00020D34
		public static void DeleteSave(string fileName, string folderName = "MMSaveLoadManager")
		{
			string text = MMSaveLoadManager.DetermineSavePath(folderName);
			string text2 = MMSaveLoadManager.DetermineSaveFileName(fileName);
			if (File.Exists(text + text2))
			{
				File.Delete(text + text2);
			}
			if (File.Exists(text + text2 + ".meta"))
			{
				File.Delete(text + text2 + ".meta");
			}
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x00022B90 File Offset: 0x00020D90
		public static void DeleteSaveFolder(string folderName = "MMSaveLoadManager")
		{
			string text = MMSaveLoadManager.DetermineSavePath(folderName);
			if (Directory.Exists(text))
			{
				MMSaveLoadManager.DeleteDirectory(text);
			}
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x00022BB4 File Offset: 0x00020DB4
		public static void DeleteAllSaveFiles()
		{
			string text = MMSaveLoadManager.DetermineSavePath("");
			text = text.Substring(0, text.Length - 1);
			if (text.EndsWith("/"))
			{
				text = text.Substring(0, text.Length - 1);
			}
			if (Directory.Exists(text))
			{
				MMSaveLoadManager.DeleteDirectory(text);
			}
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x00022C08 File Offset: 0x00020E08
		public static void DeleteDirectory(string target_dir)
		{
			string[] files = Directory.GetFiles(target_dir);
			string[] directories = Directory.GetDirectories(target_dir);
			foreach (string text in files)
			{
				File.SetAttributes(text, FileAttributes.Normal);
				File.Delete(text);
			}
			string[] array = directories;
			for (int i = 0; i < array.Length; i++)
			{
				MMSaveLoadManager.DeleteDirectory(array[i]);
			}
			Directory.Delete(target_dir, false);
			if (File.Exists(target_dir + ".meta"))
			{
				File.Delete(target_dir + ".meta");
			}
		}

		// Token: 0x040008AF RID: 2223
		public static IMMSaveLoadManagerMethod SaveLoadMethod = new MMSaveLoadManagerMethodBinary();

		// Token: 0x040008B0 RID: 2224
		private const string _baseFolderName = "/MMData/";

		// Token: 0x040008B1 RID: 2225
		private const string _defaultFolderName = "MMSaveLoadManager";
	}
}
