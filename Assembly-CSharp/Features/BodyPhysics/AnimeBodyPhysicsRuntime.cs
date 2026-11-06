using System;
using BoZo.CharacterCreator.MC.StyleSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.BodyPhysics
{
	// Token: 0x02001503 RID: 5379
	internal static class AnimeBodyPhysicsRuntime
	{
		// Token: 0x0600A622 RID: 42530 RVA: 0x00456E42 File Offset: 0x00455042
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void RegisterScenes()
		{
			SceneManager.sceneLoaded -= AnimeBodyPhysicsRuntime.OnSceneLoaded;
			SceneManager.sceneLoaded += AnimeBodyPhysicsRuntime.OnSceneLoaded;
		}

		// Token: 0x0600A623 RID: 42531 RVA: 0x00456E68 File Offset: 0x00455068
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void BindLoaded()
		{
			WardrobeBodyPhysicsController[] array = Object.FindObjectsOfType<WardrobeBodyPhysicsController>(true);
			for (int i = 0; i < array.Length; i++)
			{
				AnimeBodyPhysicsRuntime.Bind(array[i].gameObject);
			}
		}

		// Token: 0x0600A624 RID: 42532 RVA: 0x00456E98 File Offset: 0x00455098
		private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			GameObject[] rootGameObjects = scene.GetRootGameObjects();
			for (int i = 0; i < rootGameObjects.Length; i++)
			{
				WardrobeBodyPhysicsController[] componentsInChildren = rootGameObjects[i].GetComponentsInChildren<WardrobeBodyPhysicsController>(true);
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					AnimeBodyPhysicsRuntime.Bind(componentsInChildren[j].gameObject);
				}
			}
		}

		// Token: 0x0600A625 RID: 42533 RVA: 0x00456EE0 File Offset: 0x004550E0
		private static void Bind(GameObject owner)
		{
			AnimeBodyPhysics.Ensure(owner);
			OpenGarmentBodyPhysics.Ensure(owner);
		}
	}
}
