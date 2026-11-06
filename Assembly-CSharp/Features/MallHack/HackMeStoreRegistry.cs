using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.MallHack
{
	// Token: 0x02001021 RID: 4129
	public static class HackMeStoreRegistry
	{
		// Token: 0x06007D2C RID: 32044 RVA: 0x00334858 File Offset: 0x00332A58
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetRuntimeState()
		{
			if (HackMeStoreRegistry._subscribed)
			{
				SceneManager.sceneLoaded -= HackMeStoreRegistry.HandleSceneLoaded;
			}
			HackMeStoreRegistry._subscribed = false;
			HackMeStoreRegistry._completedInitialScan = false;
		}

		// Token: 0x06007D2D RID: 32045 RVA: 0x0033487E File Offset: 0x00332A7E
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void Initialize()
		{
			if (!HackMeStoreRegistry._subscribed)
			{
				SceneManager.sceneLoaded += HackMeStoreRegistry.HandleSceneLoaded;
				HackMeStoreRegistry._subscribed = true;
			}
			HackMeStoreRegistry.EnsureLoadedScenesConfigured();
		}

		// Token: 0x06007D2E RID: 32046 RVA: 0x003348A3 File Offset: 0x00332AA3
		private static void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			HackMeStoreRegistry.EnsureSceneConfigured(scene);
			HackMeStoreRegistry._completedInitialScan = true;
		}

		// Token: 0x06007D2F RID: 32047 RVA: 0x003348B4 File Offset: 0x00332AB4
		public static void EnsureLoadedScenesConfigured()
		{
			if (HackMeStoreRegistry._completedInitialScan && HackMeStoreZone.ActiveZones.Count > 0)
			{
				return;
			}
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				HackMeStoreRegistry.EnsureSceneConfigured(SceneManager.GetSceneAt(i));
			}
			HackMeStoreRegistry._completedInitialScan = true;
		}

		// Token: 0x06007D30 RID: 32048 RVA: 0x003348F8 File Offset: 0x00332AF8
		public static HackMeStoreZone FindStoreContaining(Vector3 worldPosition)
		{
			HackMeStoreZone hackMeStoreZone = null;
			float num = float.MaxValue;
			for (int i = HackMeStoreZone.ActiveZones.Count - 1; i >= 0; i--)
			{
				HackMeStoreZone hackMeStoreZone2 = HackMeStoreZone.ActiveZones[i];
				if (hackMeStoreZone2 == null)
				{
					HackMeStoreZone.ActiveZones.RemoveAt(i);
				}
				else if (hackMeStoreZone2.gameObject.activeInHierarchy && hackMeStoreZone2.Contains(worldPosition))
				{
					float approximateVolume = hackMeStoreZone2.ApproximateVolume;
					if (hackMeStoreZone == null || approximateVolume < num)
					{
						hackMeStoreZone = hackMeStoreZone2;
						num = approximateVolume;
					}
				}
			}
			return hackMeStoreZone;
		}

		// Token: 0x06007D31 RID: 32049 RVA: 0x00334978 File Offset: 0x00332B78
		private static void EnsureSceneConfigured(Scene scene)
		{
			if (!scene.IsValid() || !scene.isLoaded)
			{
				return;
			}
			GameObject[] rootGameObjects = scene.GetRootGameObjects();
			for (int i = 0; i < rootGameObjects.Length; i++)
			{
				HackMeStoreRegistry.ConfigureDescendants(rootGameObjects[i].transform, false);
			}
		}

		// Token: 0x06007D32 RID: 32050 RVA: 0x003349BC File Offset: 0x00332BBC
		private static void ConfigureDescendants(Transform current, bool alreadyInsideStore)
		{
			if (current == null)
			{
				return;
			}
			if (!alreadyInsideStore && current.name.StartsWith("Area_Electronic", StringComparison.OrdinalIgnoreCase))
			{
				if (current.GetComponent<HackMeStoreZone>() == null)
				{
					current.gameObject.AddComponent<HackMeStoreZone>().RecalculateBounds();
				}
				alreadyInsideStore = true;
			}
			for (int i = 0; i < current.childCount; i++)
			{
				HackMeStoreRegistry.ConfigureDescendants(current.GetChild(i), alreadyInsideStore);
			}
		}

		// Token: 0x0400811B RID: 33051
		private const string ElectronicsStorePrefix = "Area_Electronic";

		// Token: 0x0400811C RID: 33052
		private static bool _subscribed;

		// Token: 0x0400811D RID: 33053
		private static bool _completedInitialScan;
	}
}
