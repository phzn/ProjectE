using System;
using Features.GameOver;
using ProjectExhibitionist.Core.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.Rank
{
	// Token: 0x02000DA0 RID: 3488
	public static class RankBootstrap
	{
		// Token: 0x060066B6 RID: 26294 RVA: 0x00290FDF File Offset: 0x0028F1DF
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void Init()
		{
			if (!RankBootstrap._subscribed)
			{
				GameSystemsBootstrapper.OnGameplayReady += RankBootstrap.HandleGameplayReady;
				RankBootstrap._subscribed = true;
			}
			if (GameSystemsBootstrapper.GameplayReady)
			{
				RankBootstrap.HandleGameplayReady();
			}
		}

		// Token: 0x060066B7 RID: 26295 RVA: 0x0029100C File Offset: 0x0028F20C
		private static void HandleGameplayReady()
		{
			SceneContext sceneContext = SceneContext.Current;
			if (sceneContext != null && sceneContext.Profile != null && !sceneContext.Profile.participatesInRun)
			{
				RankRuntimeController rankRuntimeController = RankBootstrap.FindOutsideRunDebugHost();
				foreach (RankRuntimeController rankRuntimeController2 in Object.FindObjectsOfType<RankRuntimeController>(true))
				{
					if (!(rankRuntimeController2 == null) && !(rankRuntimeController2 == rankRuntimeController) && rankRuntimeController2.gameObject.name == "[RankSystem]")
					{
						Object.Destroy(rankRuntimeController2.gameObject);
					}
				}
				if (rankRuntimeController != null)
				{
					rankRuntimeController.ApplyCurrentRank(false);
					Debug.Log("[RankBootstrap] [RankSystem] debug mantenuto nella scena fuori-run.");
				}
				return;
			}
			GameObject gameObject = GameObject.Find("[RankSystem]");
			RankRuntimeController rankRuntimeController3 = ((gameObject != null) ? gameObject.GetComponent<RankRuntimeController>() : null);
			if (rankRuntimeController3 != null)
			{
				rankRuntimeController3.ApplyCurrentRank(false);
				return;
			}
			GameObject gameObject2 = ((gameObject != null) ? gameObject : new GameObject("[RankSystem]"));
			RankBootstrap.MoveToCoreScene(gameObject2);
			if (gameObject2.GetComponent<RankRunTracker>() == null)
			{
				gameObject2.AddComponent<RankRunTracker>();
			}
			if (gameObject2.GetComponent<RankRuntimeController>() == null)
			{
				gameObject2.AddComponent<RankRuntimeController>();
			}
			Debug.Log("[RankBootstrap] GO [RankSystem] creato per la run.");
		}

		// Token: 0x060066B8 RID: 26296 RVA: 0x00291148 File Offset: 0x0028F348
		private static RankRuntimeController FindOutsideRunDebugHost()
		{
			RankRuntimeController[] array = Object.FindObjectsOfType<RankRuntimeController>(true);
			Scene activeScene = SceneManager.GetActiveScene();
			RankRuntimeController rankRuntimeController = null;
			foreach (RankRuntimeController rankRuntimeController2 in array)
			{
				if (!(rankRuntimeController2 == null) && rankRuntimeController2.KeepAliveOutsideRun && !(rankRuntimeController2.gameObject.name != "[RankSystem]"))
				{
					if (rankRuntimeController2.gameObject.scene == activeScene)
					{
						return rankRuntimeController2;
					}
					if (rankRuntimeController == null)
					{
						rankRuntimeController = rankRuntimeController2;
					}
				}
			}
			return rankRuntimeController;
		}

		// Token: 0x060066B9 RID: 26297 RVA: 0x002911C8 File Offset: 0x0028F3C8
		private static void MoveToCoreScene(GameObject go)
		{
			Scene scene = ((GameOverManager.Instance != null) ? GameOverManager.Instance.gameObject.scene : SceneManager.GetSceneByName("_Core"));
			if (scene.IsValid() && scene.isLoaded && go.scene != scene)
			{
				SceneManager.MoveGameObjectToScene(go, scene);
			}
		}

		// Token: 0x04006930 RID: 26928
		private const string RankSystemName = "[RankSystem]";

		// Token: 0x04006931 RID: 26929
		private static bool _subscribed;
	}
}
