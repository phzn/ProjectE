using System;
using ProjectExhibitionist;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.CensorBars
{
	// Token: 0x020014C4 RID: 5316
	internal static class StreamerCensorRuntime
	{
		// Token: 0x0600A369 RID: 41833 RVA: 0x0043E481 File Offset: 0x0043C681
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void Reset()
		{
			StreamerCensorTarget.Active.Clear();
			StreamerCensorCharacter.Active.Clear();
			SceneManager.sceneLoaded -= StreamerCensorRuntime.OnSceneLoaded;
		}

		// Token: 0x0600A36A RID: 41834 RVA: 0x0043E4A8 File Offset: 0x0043C6A8
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void RegisterScenes()
		{
			SceneManager.sceneLoaded -= StreamerCensorRuntime.OnSceneLoaded;
			SceneManager.sceneLoaded += StreamerCensorRuntime.OnSceneLoaded;
		}

		// Token: 0x0600A36B RID: 41835 RVA: 0x0043E4CC File Offset: 0x0043C6CC
		private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			GameObject[] rootGameObjects = scene.GetRootGameObjects();
			for (int i = 0; i < rootGameObjects.Length; i++)
			{
				StreamerCensorRuntime.BindHierarchy(rootGameObjects[i]);
			}
		}

		// Token: 0x0600A36C RID: 41836 RVA: 0x0043E4F8 File Offset: 0x0043C6F8
		internal static void BindHierarchy(GameObject root)
		{
			ClothingToggleController[] componentsInChildren = root.GetComponentsInChildren<ClothingToggleController>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				StreamerCensorCharacter.Ensure(componentsInChildren[i]);
			}
			Animator[] componentsInChildren2 = root.GetComponentsInChildren<Animator>(true);
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				StreamerCensorCharacter.Ensure(componentsInChildren2[i]);
			}
		}

		// Token: 0x0600A36D RID: 41837 RVA: 0x0043E544 File Offset: 0x0043C744
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void Rebind()
		{
			ClothingToggleController[] array = Object.FindObjectsOfType<ClothingToggleController>(true);
			for (int i = 0; i < array.Length; i++)
			{
				StreamerCensorCharacter.Ensure(array[i]);
			}
			Animator[] array2 = Object.FindObjectsOfType<Animator>(true);
			for (int i = 0; i < array2.Length; i++)
			{
				StreamerCensorCharacter.Ensure(array2[i]);
			}
			foreach (StreamerCensorCharacter streamerCensorCharacter in Object.FindObjectsOfType<StreamerCensorCharacter>())
			{
				if (streamerCensorCharacter.isActiveAndEnabled)
				{
					StreamerCensorCharacter.Active.Add(streamerCensorCharacter);
				}
			}
			foreach (StreamerCensorTarget streamerCensorTarget in Object.FindObjectsOfType<StreamerCensorTarget>())
			{
				if (streamerCensorTarget.isActiveAndEnabled)
				{
					StreamerCensorTarget.Active.Add(streamerCensorTarget);
				}
			}
		}
	}
}
