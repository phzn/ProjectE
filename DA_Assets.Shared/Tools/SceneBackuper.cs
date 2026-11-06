using System;
using System.IO;
using System.Linq;
using DA_Assets.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DA_Assets.Tools
{
	// Token: 0x02000015 RID: 21
	public class SceneBackuper
	{
		// Token: 0x0600004A RID: 74 RVA: 0x00002F58 File Offset: 0x00001158
		public static bool TryBackupActiveScene()
		{
			bool flag2;
			try
			{
				Scene activeScene = SceneManager.GetActiveScene();
				bool flag = File.Exists(activeScene.path);
				string text = activeScene.name;
				if (text.IsEmpty())
				{
					text = "Untitled";
				}
				int num = 0;
				string text2 = text;
				if (text.Contains("-"))
				{
					string[] array = text.Split('-', StringSplitOptions.None);
					int num2;
					if (array.Length > 1 && int.TryParse(array.Last<string>(), out num2))
					{
						num = num2;
						text2 = string.Join("-", array.Take<string>(array.Length - 1));
					}
				}
				string backupsPath = SceneBackuper.GetBackupsPath();
				backupsPath.CreateFolderIfNotExists();
				string[] files = Directory.GetFiles(backupsPath, text2 + "-*.unity");
				if (files.Length != 0)
				{
					string[] array2 = files;
					for (int i = 0; i < array2.Length; i++)
					{
						string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(array2[i]);
						if (fileNameWithoutExtension.Contains("-"))
						{
							string[] array3 = fileNameWithoutExtension.Split('-', StringSplitOptions.None);
							int num3;
							if (array3.Length > 1 && int.TryParse(array3.Last<string>(), out num3) && num3 > num)
							{
								num = num3;
							}
						}
					}
				}
				num++;
				string text3 = string.Format("{0}-{1}.unity", text2, num);
				string text4 = Path.Combine(backupsPath, text3);
				if (flag)
				{
					File.Copy(activeScene.path, text4);
				}
				Debug.Log("A backup of your scene is created at the path:\r\n" + text4);
				flag2 = true;
			}
			catch (Exception ex)
			{
				Debug.LogError("Error creating scene backup:\r\n" + ex.Message);
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x000030E8 File Offset: 0x000012E8
		public static string GetBackupsPath()
		{
			return Path.Combine("Library", "Backup", "Scenes");
		}

		// Token: 0x0600004C RID: 76 RVA: 0x000030FE File Offset: 0x000012FE
		public static void MakeActiveSceneDirty()
		{
			bool isPlaying = Application.isPlaying;
		}
	}
}
