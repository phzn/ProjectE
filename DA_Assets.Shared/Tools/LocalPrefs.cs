using System;
using System.IO;

namespace DA_Assets.Tools
{
	// Token: 0x0200000F RID: 15
	public static class LocalPrefs
	{
		// Token: 0x06000037 RID: 55 RVA: 0x00002869 File Offset: 0x00000A69
		static LocalPrefs()
		{
			if (!Directory.Exists(LocalPrefs.prefsFolderPath))
			{
				Directory.CreateDirectory(LocalPrefs.prefsFolderPath);
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002898 File Offset: 0x00000A98
		private static string GetFilePath(string key)
		{
			return Path.Combine(LocalPrefs.prefsFolderPath, key);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000028A8 File Offset: 0x00000AA8
		public static int GetInt(string key, int defaultValue = 0)
		{
			string filePath = LocalPrefs.GetFilePath(key);
			int num;
			if (File.Exists(filePath) && int.TryParse(File.ReadAllText(filePath), out num))
			{
				return num;
			}
			return defaultValue;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000028D6 File Offset: 0x00000AD6
		public static void SetInt(string key, int value)
		{
			File.WriteAllText(LocalPrefs.GetFilePath(key), value.ToString());
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000028EC File Offset: 0x00000AEC
		public static string GetString(string key, string defaultValue = "")
		{
			string filePath = LocalPrefs.GetFilePath(key);
			if (!File.Exists(filePath))
			{
				return defaultValue;
			}
			return File.ReadAllText(filePath);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002910 File Offset: 0x00000B10
		public static void SetString(string key, string value)
		{
			File.WriteAllText(LocalPrefs.GetFilePath(key), value);
		}

		// Token: 0x04000023 RID: 35
		private static readonly string prefsFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LocalPrefs");
	}
}
