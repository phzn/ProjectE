using System;
using Features.CielOS.Apps.Maps;
using Features.RunModes;
using ProjectExhibitionist.Core.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectExhibitionist.PoiTravel
{
	// Token: 0x02000944 RID: 2372
	internal static class PoiArrivalAutoInstaller
	{
		// Token: 0x06004097 RID: 16535 RVA: 0x0017F772 File Offset: 0x0017D972
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			SceneManager.sceneLoaded -= PoiArrivalAutoInstaller.OnSceneLoaded;
			PoiArrivalAutoInstaller._catalog = null;
		}

		// Token: 0x06004098 RID: 16536 RVA: 0x0017F78B File Offset: 0x0017D98B
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Register()
		{
			SceneManager.sceneLoaded -= PoiArrivalAutoInstaller.OnSceneLoaded;
			SceneManager.sceneLoaded += PoiArrivalAutoInstaller.OnSceneLoaded;
		}

		// Token: 0x06004099 RID: 16537 RVA: 0x0017F7B0 File Offset: 0x0017D9B0
		private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			if (RunModeService.IsLongWayHome && scene.name == "Game - Current")
			{
				PoiArrivalPresentationRequest.TryConsume(scene.name);
				return;
			}
			if (LongWayHomeRuntimeController.IsAwaitingProloguePoi(scene.name))
			{
				return;
			}
			PoiArrivalAutoInstaller._catalog = ((PoiArrivalAutoInstaller._catalog != null) ? PoiArrivalAutoInstaller._catalog : Resources.Load<PoiArrivalCatalogSO>("PoiArrivalCatalog"));
			PoiArrivalCatalogSO.Zone zone;
			if (PoiArrivalAutoInstaller._catalog == null || !PoiArrivalAutoInstaller._catalog.TryGetZone(scene.name, out zone))
			{
				return;
			}
			string text;
			if (PoiTravelState.TryConsumeNaturalDiscovery(scene.name, out text))
			{
				PoiFastTravelProgressService.RecordNaturalVisit(scene.name, text);
			}
			if (!PoiTravelState.IsPoiSceneName(scene.name) && !PoiArrivalPresentationRequest.TryConsume(scene.name))
			{
				return;
			}
			GameObject gameObject = PoiArrivalAutoInstaller.FindSceneContextHost(scene);
			if (gameObject == null)
			{
				Debug.LogWarning("[PoiArrival] Scene '" + scene.name + "' is catalogued but has no SceneContext.");
				return;
			}
			PoiArrivalPresentation poiArrivalPresentation = gameObject.GetComponent<PoiArrivalPresentation>();
			if (poiArrivalPresentation == null)
			{
				poiArrivalPresentation = gameObject.AddComponent<PoiArrivalPresentation>();
			}
			poiArrivalPresentation.Configure(zone.displayName, zone.voiceLines, PoiArrivalAutoInstaller._catalog.TitleFont, zone.cameraYawOffset, zone.overrideGameplayCameraOrbit, zone.gameplayCameraOrbit, zone.gameplayCameraDistance, zone.cameraReturnDurationOverride, zone.titleHoldDurationOverride);
			poiArrivalPresentation.ConfigureTrafficAudio(zone.trafficPassClip, zone.trafficHornOneClip, zone.trafficHornTwoClip, zone.trafficPresets);
		}

		// Token: 0x0600409A RID: 16538 RVA: 0x0017F918 File Offset: 0x0017DB18
		private static GameObject FindSceneContextHost(Scene scene)
		{
			GameObject[] rootGameObjects = scene.GetRootGameObjects();
			for (int i = 0; i < rootGameObjects.Length; i++)
			{
				SceneContext componentInChildren = rootGameObjects[i].GetComponentInChildren<SceneContext>(true);
				if (componentInChildren != null && componentInChildren.gameObject.scene == scene)
				{
					return componentInChildren.gameObject;
				}
			}
			return null;
		}

		// Token: 0x04004063 RID: 16483
		private const string CatalogResourceName = "PoiArrivalCatalog";

		// Token: 0x04004064 RID: 16484
		private static PoiArrivalCatalogSO _catalog;
	}
}
