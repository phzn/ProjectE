using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoreMountains.Tools
{
	// Token: 0x02000105 RID: 261
	public class MMScene
	{
		// Token: 0x060005AB RID: 1451 RVA: 0x0001BD80 File Offset: 0x00019F80
		public static Scene[] GetLoadedScenes()
		{
			int sceneCount = SceneManager.sceneCount;
			List<Scene> list = new List<Scene>(sceneCount);
			for (int i = 0; i < sceneCount; i++)
			{
				Scene sceneAt = SceneManager.GetSceneAt(i);
				if (sceneAt.isLoaded)
				{
					list.Add(sceneAt);
				}
				else
				{
					Debug.LogWarning(sceneAt.name + " NOT LOADED");
				}
			}
			return list.ToArray();
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x0001BDDC File Offset: 0x00019FDC
		public static List<string> GetScenesInBuild()
		{
			List<string> list = new List<string>();
			for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
			{
				string scenePathByBuildIndex = SceneUtility.GetScenePathByBuildIndex(i);
				int num = scenePathByBuildIndex.LastIndexOf("/", StringComparison.Ordinal);
				list.Add(scenePathByBuildIndex.Substring(num + 1, scenePathByBuildIndex.LastIndexOf(".", StringComparison.Ordinal) - num - 1));
			}
			return list;
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x0001BE33 File Offset: 0x0001A033
		public static bool SceneInBuild(string sceneName)
		{
			return MMScene.GetScenesInBuild().Contains(sceneName);
		}
	}
}
