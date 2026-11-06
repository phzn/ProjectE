using System;
using System.Collections;
using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Invector.Utils
{
	// Token: 0x02000602 RID: 1538
	public static class LoadLevelHelper
	{
		// Token: 0x0600237E RID: 9086 RVA: 0x000C9B14 File Offset: 0x000C7D14
		public static void LoadScene(string _sceneName, string _spawnPointName, vThirdPersonInput tpInput)
		{
			if (!tpInput)
			{
				return;
			}
			LoadLevelHelper.targetCharacter = tpInput;
			LoadLevelHelper.spawnPointName = _spawnPointName;
			LoadLevelHelper.sceneName = _sceneName;
			if (LoadLevelHelper.targetCharacter.tpCamera)
			{
				LoadLevelHelper.targetCharacter.tpCamera.transform.parent = LoadLevelHelper.targetCharacter.transform;
			}
			vISceneLoadListener[] components = LoadLevelHelper.targetCharacter.GetComponents<vISceneLoadListener>();
			for (int i = 0; i < components.Length; i++)
			{
				components[i].OnStartLoadScene(_sceneName);
			}
			LoadLevelHelper.targetCharacter.StartCoroutine(LoadLevelHelper.LoadAsyncScene());
		}

		// Token: 0x0600237F RID: 9087 RVA: 0x000C9B9D File Offset: 0x000C7D9D
		private static IEnumerator LoadAsyncScene()
		{
			if (LoadLevelHelper.targetCharacter == null || LoadLevelHelper.isLoading)
			{
				yield break;
			}
			LoadLevelHelper.isLoading = true;
			Scene currentScene = SceneManager.GetActiveScene();
			if (!currentScene.name.Equals(LoadLevelHelper.sceneName))
			{
				SceneManager.sceneUnloaded += LoadLevelHelper.OnSceneUnloaded;
				AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(LoadLevelHelper.sceneName, LoadSceneMode.Additive);
				while (!asyncLoad.isDone)
				{
					yield return null;
				}
				SceneManager.MoveGameObjectToScene(LoadLevelHelper.targetCharacter.gameObject, SceneManager.GetSceneByName(LoadLevelHelper.sceneName));
				SceneManager.UnloadSceneAsync(currentScene);
				asyncLoad = null;
			}
			else
			{
				LoadLevelHelper.MoveCharacterToSpawnPoint();
			}
			LoadLevelHelper.isLoading = false;
			yield break;
		}

		// Token: 0x06002380 RID: 9088 RVA: 0x000C9BA8 File Offset: 0x000C7DA8
		private static void OnSceneUnloaded(Scene unloadedScene)
		{
			vISceneLoadListener[] components = LoadLevelHelper.targetCharacter.GetComponents<vISceneLoadListener>();
			for (int i = 0; i < components.Length; i++)
			{
				components[i].OnFinishLoadScene(unloadedScene.name);
			}
			LoadLevelHelper.MoveCharacterToSpawnPoint();
			SceneManager.sceneUnloaded -= LoadLevelHelper.OnSceneUnloaded;
		}

		// Token: 0x06002381 RID: 9089 RVA: 0x000C9BF4 File Offset: 0x000C7DF4
		private static void MoveCharacterToSpawnPoint()
		{
			GameObject gameObject = GameObject.Find(LoadLevelHelper.spawnPointName);
			if (gameObject && LoadLevelHelper.targetCharacter)
			{
				LoadLevelHelper.targetCharacter.lockCameraInput = true;
				if (LoadLevelHelper.targetCharacter.tpCamera)
				{
					LoadLevelHelper.targetCharacter.tpCamera.FreezeCamera();
				}
				LoadLevelHelper.targetCharacter.transform.position = gameObject.transform.position;
				LoadLevelHelper.targetCharacter.transform.rotation = gameObject.transform.rotation;
				if (LoadLevelHelper.targetCharacter.tpCamera)
				{
					LoadLevelHelper.targetCharacter.tpCamera.transform.parent = null;
					LoadLevelHelper.targetCharacter.tpCamera.UnFreezeCamera();
				}
				LoadLevelHelper.targetCharacter.lockCameraInput = false;
			}
		}

		// Token: 0x0400238E RID: 9102
		public static vThirdPersonInput targetCharacter;

		// Token: 0x0400238F RID: 9103
		public static string spawnPointName;

		// Token: 0x04002390 RID: 9104
		public static string sceneName;

		// Token: 0x04002391 RID: 9105
		public static bool isLoading;
	}
}
