using System;
using System.Collections;
using System.Diagnostics;
using ExhibitionistDemo.UI;
using Features.DebugTools;
using Features.Detection;
using Features.GameOver;
using Features.Home;
using Features.Inventory.Actions;
using Features.Loading;
using Features.Rank;
using Features.RenderingPrecision;
using Features.RunModes;
using Features.Vehicles.Spawning;
using Invector.vCharacterController;
using ProjectExhibitionist.CharacterCustomization.Core.Runtime;
using ProjectExhibitionist.Core.Rendering;
using ProjectExhibitionist.Diagnostics;
using ProjectExhibitionist.FirstPerson;
using ProjectExhibitionist.Minimap;
using ProjectExhibitionist.PoiTravel;
using RayznGames.VehicleSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectExhibitionist.Core.SceneManagement
{
	// Token: 0x02000A82 RID: 2690
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(-900)]
	public class SceneTransitionManager : MonoBehaviour
	{
		// Token: 0x17000CEC RID: 3308
		// (get) Token: 0x06004C89 RID: 19593 RVA: 0x001DBC80 File Offset: 0x001D9E80
		// (set) Token: 0x06004C8A RID: 19594 RVA: 0x001DBC87 File Offset: 0x001D9E87
		public static SceneTransitionManager Instance { get; private set; }

		// Token: 0x17000CED RID: 3309
		// (get) Token: 0x06004C8B RID: 19595 RVA: 0x001DBC8F File Offset: 0x001D9E8F
		// (set) Token: 0x06004C8C RID: 19596 RVA: 0x001DBC96 File Offset: 0x001D9E96
		public static string PendingGameplayScene { get; set; }

		// Token: 0x17000CEE RID: 3310
		// (get) Token: 0x06004C8D RID: 19597 RVA: 0x001DBC9E File Offset: 0x001D9E9E
		// (set) Token: 0x06004C8E RID: 19598 RVA: 0x001DBCA6 File Offset: 0x001D9EA6
		public bool IsTransitioning { get; private set; }

		// Token: 0x17000CEF RID: 3311
		// (get) Token: 0x06004C8F RID: 19599 RVA: 0x001DBCAF File Offset: 0x001D9EAF
		public bool IsArrivalProtected
		{
			get
			{
				return this.IsTransitioning || this._arrivalProtectionPendingReveal || Time.time < this._arrivalProtectionUntil;
			}
		}

		// Token: 0x17000CF0 RID: 3312
		// (get) Token: 0x06004C90 RID: 19600 RVA: 0x001DBCD0 File Offset: 0x001D9ED0
		// (set) Token: 0x06004C91 RID: 19601 RVA: 0x001DBCD8 File Offset: 0x001D9ED8
		public string CurrentGameplayScene { get; private set; }

		// Token: 0x06004C92 RID: 19602 RVA: 0x001DBCE4 File Offset: 0x001D9EE4
		private void Awake()
		{
			if (SceneTransitionManager.Instance != null && SceneTransitionManager.Instance != this)
			{
				Debug.LogWarning("[SceneTransitionManager] Duplicato su '" + base.name + "', distrutto.");
				Object.Destroy(base.gameObject);
				return;
			}
			SceneTransitionManager.Instance = this;
			this.CurrentGameplayScene = this.ResolveCurrentGameplayScene();
		}

		// Token: 0x06004C93 RID: 19603 RVA: 0x001DBD44 File Offset: 0x001D9F44
		private void Start()
		{
			if (this.CurrentGameplayScene == null)
			{
				string text = SceneTransitionManager.PendingGameplayScene ?? this.defaultGameplayScene;
				SceneTransitionManager.PendingGameplayScene = null;
				if (!string.IsNullOrEmpty(text))
				{
					if (this.enableDebugLogs)
					{
						Debug.Log("[SceneTransitionManager] _Core avviato come base: carico scena di gioco '" + text + "' in additive.");
					}
					this.LoadGameplayScene(text);
				}
			}
		}

		// Token: 0x06004C94 RID: 19604 RVA: 0x001DBD9B File Offset: 0x001D9F9B
		private void OnDestroy()
		{
			if (SceneTransitionManager.Instance == this)
			{
				SceneTransitionManager.Instance = null;
			}
		}

		// Token: 0x06004C95 RID: 19605 RVA: 0x001DBDB0 File Offset: 0x001D9FB0
		private void Update()
		{
			if (!this._arrivalProtectionPendingReveal || this.IsTransitioning)
			{
				return;
			}
			LoadingScreenController existingInstance = LoadingScreenController.ExistingInstance;
			if ((existingInstance != null && existingInstance.IsLoadingActive) || PoiArrivalPresentation.IsPresentingInActiveScene)
			{
				return;
			}
			this._arrivalProtectionPendingReveal = false;
			this._arrivalProtectionUntil = Time.time + Mathf.Max(0f, this.arrivalProtectionDuration);
		}

		// Token: 0x06004C96 RID: 19606 RVA: 0x001DBE10 File Offset: 0x001DA010
		public void LoadGameplayScene(string sceneName)
		{
			if (!RankPoiPolicy.IsDestinationAllowed(sceneName))
			{
				return;
			}
			if (string.IsNullOrEmpty(sceneName))
			{
				Debug.LogError("[SceneTransitionManager] LoadGameplayScene con nome vuoto.");
				return;
			}
			if (sceneName == this.coreSceneName)
			{
				Debug.LogError("[SceneTransitionManager] '" + this.coreSceneName + "' è la scena persistente: non caricarla come scena di gioco.");
				return;
			}
			if (this.IsTransitioning)
			{
				Debug.LogWarning("[SceneTransitionManager] Transizione già in corso, ignoro '" + sceneName + "'.");
				return;
			}
			base.StartCoroutine(this.SwapRoutine(sceneName));
		}

		// Token: 0x06004C97 RID: 19607 RVA: 0x001DBE8E File Offset: 0x001DA08E
		private IEnumerator SwapRoutine(string newScene)
		{
			this.IsTransitioning = true;
			bool completedNormally = false;
			bool startedSwap = false;
			IDisposable navigationPause = null;
			try
			{
				string oldScene = this.ResolveCurrentGameplayScene();
				if (newScene == oldScene)
				{
					if (this.enableDebugLogs)
					{
						Debug.Log("[SceneTransitionManager] '" + newScene + "' è già la scena di gioco attiva. No-op.");
					}
					completedNormally = true;
					yield break;
				}
				startedSwap = true;
				GameSystemsBootstrapper.InvalidateForSceneTransition();
				FlyCamera.StopForContextChange();
				MapPinController.ClearForTransition();
				ExhibitionEventDirector instance = ExhibitionEventDirector.Instance;
				if (instance != null)
				{
					instance.CancelForSceneTransition();
				}
				if (newScene == "CielHome")
				{
					GameOverManager instance2 = GameOverManager.Instance;
					if (instance2 != null)
					{
						instance2.ResetRunState();
					}
				}
				bool destinationIsPoi = PoiTravelState.IsPoiSceneName(newScene);
				bool useTransitionFade = SceneTransitionManager.ShouldUseTransitionFade(oldScene, newScene, this.fadePoiTransitions);
				ScreenFadeController transitionFade = (useTransitionFade ? ScreenFadeController.Instance : null);
				if (transitionFade != null)
				{
					yield return SceneTransitionManager.AnimateFade(transitionFade, true, this.poiFadeOutDuration);
				}
				LoadingScreenController loading = LoadingScreenController.Instance;
				if (loading != null && !loading.IsManualLoadActive)
				{
					loading.BeginManualLoad();
				}
				LongWayHomeRestaurantSequence presenterForScene = LongWayHomeRestaurantSequence.GetPresenterForScene(SceneManager.GetSceneByName(oldScene));
				if (presenterForScene != null)
				{
					presenterForScene.PrepareForSceneExit();
				}
				ILongWayHomePoiProloguePresenter presenterForScene2 = LongWayHomePoiPrologue.GetPresenterForScene(SceneManager.GetSceneByName(oldScene));
				if (presenterForScene2 != null)
				{
					presenterForScene2.PrepareForSceneExit();
				}
				Transform player = PlayerRegistry.Player;
				PlayerRegistry sourceRegistry = ((player != null) ? player.GetComponent<PlayerRegistry>() : null);
				Scene persistentScene = SceneManager.GetSceneByName(this.coreSceneName);
				Scene scene = (string.IsNullOrEmpty(oldScene) ? default(Scene) : SceneManager.GetSceneByName(oldScene));
				SceneTransitionManager.NormalizeSourcePlayerForSceneExit(sourceRegistry, scene, persistentScene);
				if (player != null)
				{
					FirstPersonController component = player.GetComponent<FirstPersonController>();
					if (component != null)
					{
						component.ResetForSceneTransition();
					}
				}
				if (useTransitionFade && transitionFade != null)
				{
					transitionFade.SetBlack();
				}
				AStarGraphManager navigation = AStarGraphManager.Instance;
				while (navigation != null && navigation.IsNavigationInitializing)
				{
					yield return null;
				}
				AStarGraphManager astarGraphManager = navigation;
				navigationPause = ((astarGraphManager != null) ? astarGraphManager.SuspendForSceneTransition() : null);
				if (!SceneTransitionManager.TryRecoverNavigationForTransition())
				{
					yield break;
				}
				while (!WorldOrigin.TryResetForSceneExit())
				{
					if (!SceneTransitionManager.TryRecoverNavigationForTransition())
					{
						yield break;
					}
					yield return null;
				}
				Stopwatch swSwap = Stopwatch.StartNew();
				long msSourceCleanup = 0L;
				long msLoad = 0L;
				long msReady = 0L;
				long msUnload = 0L;
				long msRunRetirement = 0L;
				MemoryReport.Log("swap-start");
				using (IEnumerator sourcePreparation = SceneTransitionManager.PrepareSourceForDestinationLoad(oldScene, destinationIsPoi, this.unloadUnusedAssets && !string.IsNullOrEmpty(oldScene) && oldScene != "LongWayHome_RestaurantIntro" && newScene != "LongWayHome_RestaurantIntro"))
				{
					while (sourcePreparation.MoveNext())
					{
						object obj = sourcePreparation.Current;
						yield return obj;
					}
				}
				msSourceCleanup = swSwap.ElapsedMilliseconds;
				MemoryReport.Log("swap-source-quiesced");
				AsyncOperation loadOp = SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Additive);
				if (loadOp == null)
				{
					Debug.LogError("[SceneTransitionManager] Impossibile caricare '" + newScene + "'. In Build Settings?");
					GameSystemsBootstrapper.FailGameplayReadiness("Scene load returned no operation: '" + newScene + "'.");
					yield break;
				}
				while (!loadOp.isDone)
				{
					LoadingScreenController loadingScreenController = loading;
					if (loadingScreenController != null)
					{
						loadingScreenController.ReportProgress(Mathf.Clamp01(loadOp.progress / 0.9f) * 0.25f);
					}
					yield return null;
				}
				LoadingScreenController loadingScreenController2 = loading;
				if (loadingScreenController2 != null)
				{
					loadingScreenController2.ReportProgress(0.28f);
				}
				msLoad = swSwap.ElapsedMilliseconds;
				MemoryReport.Log("swap-destination-loaded");
				Scene loaded = SceneManager.GetSceneByName(newScene);
				if (loaded.IsValid())
				{
					SceneManager.SetActiveScene(loaded);
					EnvironmentLightingRefresh.RequestForActiveScene();
				}
				this.CurrentGameplayScene = newScene;
				PoiTravelState.NotifySceneArrived(newScene);
				SceneBootLoader.RepositionPlayerToCurrentContext(this.enableDebugLogs);
				PoiTravelState.TryApplyReturnOverride(newScene);
				if (!destinationIsPoi)
				{
					DistanceTracker distanceTracker = Object.FindObjectOfType<DistanceTracker>(true);
					if (distanceTracker != null)
					{
						distanceTracker.SetSceneTrackingPaused(false);
					}
				}
				LoadingScreenController loadingScreenController3 = loading;
				if (loadingScreenController3 != null)
				{
					loadingScreenController3.ReportProgress(0.32f);
				}
				Scene sourceForRetirement = (string.IsNullOrEmpty(oldScene) ? default(Scene) : SceneManager.GetSceneByName(oldScene));
				if (!string.IsNullOrEmpty(oldScene) && oldScene != newScene && sourceForRetirement.IsValid() && sourceForRetirement.isLoaded)
				{
					if (sourceRegistry != null)
					{
						sourceRegistry.TryRestoreSceneBeforeUnload(sourceForRetirement, persistentScene);
					}
					AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(sourceForRetirement);
					while (unloadOp != null && !unloadOp.isDone)
					{
						yield return null;
					}
					unloadOp = null;
				}
				LightProbes.TetrahedralizeAsync();
				msUnload = swSwap.ElapsedMilliseconds;
				MemoryReport.Log("swap-source-unloaded");
				using (IEnumerator poiOpeningCleanup = SceneTransitionManager.PreparePoiOpeningForCityBootstrap(sourceForRetirement, oldScene, newScene, this.unloadUnusedAssets))
				{
					while (poiOpeningCleanup.MoveNext())
					{
						object obj2 = poiOpeningCleanup.Current;
						yield return obj2;
					}
				}
				if (SceneTransitionManager.ShouldRetireCompletedRun(sourceForRetirement, loaded) && ChunkManager.Instance != null)
				{
					long retirementStarted = swSwap.ElapsedMilliseconds;
					MemoryReport.Log("swap-run-retirement-start");
					VehicleSpawnManager vehicleSpawnManager = Object.FindObjectOfType<VehicleSpawnManager>(true);
					if (vehicleSpawnManager != null)
					{
						vehicleSpawnManager.ShutdownForCurrentScene();
					}
					yield return ChunkManager.Instance.RetireCompletedRun();
					MemoryReport.Log("swap-run-retirement-end");
					msRunRetirement = swSwap.ElapsedMilliseconds - retirementStarted;
				}
				if (newScene == "CielHome" && PlayerRegistry.Player != null)
				{
					MainCharacterAppearanceApplier componentInChildren = PlayerRegistry.Player.GetComponentInChildren<MainCharacterAppearanceApplier>(true);
					if (componentInChildren != null)
					{
						componentInChildren.RestoreHomeState();
					}
				}
				if (GameSystemsBootstrapper.Instance != null)
				{
					GameSystemsBootstrapper.Instance.ReinitializeForCurrentScene();
				}
				IDisposable disposable3 = navigationPause;
				if (disposable3 != null)
				{
					disposable3.Dispose();
				}
				navigationPause = null;
				GraphicsSettingsController instance3 = GraphicsSettingsController.Instance;
				if (instance3 != null)
				{
					instance3.ReapplySceneRendering();
				}
				if (newScene == "Game - Current")
				{
					Transform returningPlayer = PlayerRegistry.Player;
					while (returningPlayer != null && !WorldOrigin.TryCenterAround(returningPlayer.position, true))
					{
						if (!SceneTransitionManager.TryRecoverNavigationForTransition())
						{
							yield break;
						}
						yield return null;
					}
					returningPlayer = null;
				}
				double waitStartedAt = Time.realtimeSinceStartupAsDouble;
				LoadingReadinessWatchdog watchdog = new LoadingReadinessWatchdog(Mathf.Max(5f, this.gameplayReadyTimeout));
				while (!GameSystemsBootstrapper.GameplayReady)
				{
					if (GameSystemsBootstrapper.GameplayReadinessFailed)
					{
						yield break;
					}
					float num = (float)(Time.realtimeSinceStartupAsDouble - waitStartedAt);
					ChunkManager instance4 = ChunkManager.Instance;
					LoadingScreenController loadingScreenController4 = loading;
					if (loadingScreenController4 != null)
					{
						loadingScreenController4.ReportProgress((instance4 != null && instance4.IsAwaitingCatalog) ? (0.32f + instance4.CatalogLoadProgress * 0.08f) : (0.28f + GameSystemsBootstrapper.InitializationProgress * 0.66f));
					}
					if (watchdog.Tick(Time.realtimeSinceStartupAsDouble, GameSystemsBootstrapper.InitializationProgress, GameSystemsBootstrapper.InitializationWorkVersion, 0))
					{
						string stall = string.Concat(new string[]
						{
							"SceneTransitionManager '",
							oldScene ?? "<none>",
							"' → '",
							newScene,
							"' ",
							string.Format("stalled {0:0.#}s after {1:0.#}s total", watchdog.StalledSeconds, num)
						});
						if (GameSystemsBootstrapper.ForceCompleteGameplayReadiness(stall))
						{
							break;
						}
						if (GameSystemsBootstrapper.GameplayReadinessFailed)
						{
							yield break;
						}
						LoadingStallMonitor.Report(this, true, () => stall);
					}
					else
					{
						LoadingStallMonitor.Report(this, false, null);
					}
					yield return null;
				}
				LoadingStallMonitor.Report(this, false, null);
				watchdog = null;
				msReady = swSwap.ElapsedMilliseconds;
				MemoryReport.Log("swap-gameplay-ready");
				if (this.unloadUnusedAssets && newScene != "LongWayHome_RestaurantIntro")
				{
					LoadingScreenController loadingScreenController5 = loading;
					if (loadingScreenController5 != null)
					{
						loadingScreenController5.ReportProgress(0.98f);
					}
					HeavyZoneStreamingProxy.ReleaseCachedPayloadAssets(false);
					yield return SceneTransitionManager.UnloadUnusedAssetsRoutine();
				}
				if (loading != null)
				{
					yield return loading.WaitForLongWayHomeOpening();
				}
				LoadingScreenController loadingScreenController6 = loading;
				if (loadingScreenController6 != null)
				{
					loadingScreenController6.ReportProgress(0.99f);
				}
				EnvironmentLightingRefresh.ForceRequestForActiveScene();
				yield return EnvironmentLightingRefresh.WaitUntilReady(12f);
				LongWayHomeRestaurantSequence restaurantPresenter = LongWayHomeRestaurantSequence.GetPresenterForScene(loaded);
				float presentationDeadline = Time.realtimeSinceStartup + Mathf.Max(5f, this.gameplayReadyTimeout);
				while (restaurantPresenter != null && restaurantPresenter.OwnsDestinationReveal && !restaurantPresenter.IsPreparedForReveal)
				{
					if (Time.realtimeSinceStartup >= presentationDeadline)
					{
						restaurantPresenter.FailPreparation("The restaurant presentation did not finish before the loading deadline.");
						break;
					}
					yield return null;
				}
				ILongWayHomePoiProloguePresenter poiPresenter = LongWayHomePoiPrologue.GetPresenterForScene(loaded);
				while (poiPresenter != null && poiPresenter.OwnsDestinationReveal && !poiPresenter.IsPreparedForReveal)
				{
					if (Time.realtimeSinceStartup >= presentationDeadline)
					{
						poiPresenter.FailPreparation("The opening presentation did not finish before the loading deadline.");
						break;
					}
					yield return null;
				}
				Debug.Log(string.Concat(new string[]
				{
					"[SceneSwap] '",
					oldScene ?? "<none>",
					"' → '",
					newScene,
					"': ",
					string.Format("source cleanup {0:F1}s, ", (float)msSourceCleanup / 1000f),
					string.Format("load+activation {0:F1}s, ", (float)(msLoad - msSourceCleanup) / 1000f),
					string.Format("unload vecchia {0:F1}s, ", (float)(msUnload - msLoad) / 1000f),
					string.Format("retirement run {0:F1}s, ", (float)msRunRetirement / 1000f),
					string.Format("gameplayReady {0:F1}s, ", (float)(msReady - msUnload - msRunRetirement) / 1000f),
					string.Format("final cleanup+lighting {0:F1}s, ", (float)(swSwap.ElapsedMilliseconds - msReady) / 1000f),
					string.Format("TOTALE {0:F1}s", (float)swSwap.ElapsedMilliseconds / 1000f)
				}));
				MemoryReport.Log("swap-end");
				if (this.enableDebugLogs)
				{
					Debug.Log(string.Concat(new string[]
					{
						"[SceneTransitionManager] Swap completato: '",
						oldScene ?? "<none>",
						"' → '",
						newScene,
						"'. ",
						string.Format("Scene caricate: {0} (core '{1}' persistente).", SceneManager.sceneCount, this.coreSceneName)
					}));
				}
				bool arrivalOwnsReveal = LongWayHomeRuntimeController.IsPreparing || (destinationIsPoi && SceneTransitionManager.HasActivePoiArrivalPresentation()) || (newScene == "CielHome" && SceneTransitionManager.HasActiveCielHomeIntro()) || (restaurantPresenter != null && restaurantPresenter.OwnsDestinationReveal) || (poiPresenter != null && poiPresenter.OwnsDestinationReveal);
				ScreenFadeController screenFadeController = transitionFade;
				if (screenFadeController != null)
				{
					screenFadeController.SetBlack();
				}
				LoadingScreenController loadingScreenController7 = loading;
				if (loadingScreenController7 != null)
				{
					loadingScreenController7.EndManualLoad();
				}
				yield return SceneTransitionManager.WaitForLoadingToHide(loading);
				if (transitionFade != null && !arrivalOwnsReveal)
				{
					yield return SceneTransitionManager.AnimateFade(transitionFade, false, this.poiFadeInDuration);
				}
				completedNormally = true;
				oldScene = null;
				transitionFade = null;
				loading = null;
				sourceRegistry = null;
				persistentScene = default(Scene);
				navigation = null;
				swSwap = null;
				IEnumerator sourcePreparation = null;
				loadOp = null;
				loaded = default(Scene);
				sourceForRetirement = default(Scene);
				IEnumerator poiOpeningCleanup = null;
				restaurantPresenter = null;
				poiPresenter = null;
			}
			finally
			{
				LoadingStallMonitor.Remove(this);
				try
				{
					IDisposable disposable4 = navigationPause;
					if (disposable4 != null)
					{
						disposable4.Dispose();
					}
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
				}
				if (startedSwap)
				{
					this._arrivalProtectionPendingReveal = true;
				}
				this.IsTransitioning = false;
				if (!completedNormally && startedSwap && Application.isPlaying)
				{
					try
					{
						GameSystemsBootstrapper.FailGameplayReadiness("Scene transition to '" + newScene + "' did not complete.");
						LoadingScreenController existingInstance = LoadingScreenController.ExistingInstance;
						if (existingInstance != null)
						{
							existingInstance.ShowLoadFailure();
						}
					}
					catch (Exception ex2)
					{
						Debug.LogException(ex2);
					}
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06004C98 RID: 19608 RVA: 0x001DBEA4 File Offset: 0x001DA0A4
		private static void NormalizeSourcePlayerForSceneExit(PlayerRegistry sourceRegistry, Scene sourceScene, Scene persistentScene)
		{
			if (sourceRegistry == null || !persistentScene.IsValid() || !persistentScene.isLoaded || sourceRegistry.OriginalScene != persistentScene)
			{
				return;
			}
			Transform transform = sourceRegistry.transform;
			if (transform.gameObject.scene != persistentScene && transform.gameObject.scene != sourceScene)
			{
				return;
			}
			foreach (DildoActionExecutor dildoActionExecutor in transform.GetComponentsInChildren<DildoActionExecutor>(true))
			{
				if (dildoActionExecutor != null && dildoActionExecutor.IsExecuting)
				{
					dildoActionExecutor.CancelAction();
				}
			}
			DetectionPlayerNormalizer.ForceDismount(transform);
			CielRagdollRecovery component = transform.GetComponent<CielRagdollRecovery>();
			vRagdoll component2 = transform.GetComponent<vRagdoll>();
			if (component != null && (component.IsRecovering || (component2 != null && (component2.isActive || component2.state == vRagdoll.RagdollState.blendToAnim))))
			{
				component.InterruptForUnstuck();
			}
		}

		// Token: 0x06004C99 RID: 19609 RVA: 0x001DBF88 File Offset: 0x001DA188
		private static IEnumerator PreparePoiOpeningForCityBootstrap(Scene source, string sourceSceneName, string destinationSceneName, bool releaseUnusedAssets)
		{
			if (!releaseUnusedAssets || destinationSceneName != "Game - Current" || string.IsNullOrEmpty(sourceSceneName) || !LongWayHomeRuntimeController.StartsCityAfterPoiPrologue || (source.IsValid() && source.isLoaded))
			{
				yield break;
			}
			string text = LongWayHomeRuntimeController.ProloguePoiScene(RunModeService.CurrentLongWayHomePrologue);
			if (sourceSceneName != text)
			{
				yield break;
			}
			ChunkManager instance = ChunkManager.Instance;
			if (instance != null && (instance.IsCatalogLoaded || instance.IsReadingCatalog))
			{
				yield break;
			}
			MemoryReport.Log("swap-poi-opening-purge-start");
			HeavyZoneStreamingProxy.ReleaseCachedPayloadAssets(false);
			using (IEnumerator cleanup = SceneTransitionManager.UnloadUnusedAssetsRoutine())
			{
				while (cleanup.MoveNext())
				{
					object obj = cleanup.Current;
					yield return obj;
				}
			}
			MemoryReport.Log("swap-poi-opening-purged");
			yield break;
			yield break;
		}

		// Token: 0x06004C9A RID: 19610 RVA: 0x001DBFAC File Offset: 0x001DA1AC
		private static IEnumerator PrepareSourceForDestinationLoad(string oldScene, bool destinationIsPoi, bool releaseUnusedAssets)
		{
			NPCZoneManager instance = NPCZoneManager.Instance;
			if (instance != null)
			{
				instance.PrepareForSceneTransition();
			}
			if (destinationIsPoi || oldScene == "Game - Current")
			{
				DistanceTracker distanceTracker = Object.FindObjectOfType<DistanceTracker>(true);
				if (distanceTracker != null)
				{
					distanceTracker.SetSceneTrackingPaused(true);
				}
				ChunkManager instance2 = ChunkManager.Instance;
				if (instance2 != null)
				{
					instance2.SuspendForPoiTravel();
				}
			}
			if (!releaseUnusedAssets)
			{
				yield break;
			}
			HeavyZoneStreamingProxy.ReleaseCachedPayloadAssets(false);
			float warningAt = Time.realtimeSinceStartup + 15f;
			bool warned = false;
			while (HeavyZoneStreamingProxy.ActivePayloadAssetLoadCount > 0)
			{
				if (!warned && Time.realtimeSinceStartup >= warningAt)
				{
					warned = true;
					Debug.LogWarning("[SceneTransitionManager] Source payload reads exceeded 15s; waiting for completion before loading the destination.");
				}
				yield return null;
			}
			yield return null;
			HeavyZoneStreamingProxy.ReleaseCachedPayloadAssets(false);
			using (IEnumerator cleanup = SceneTransitionManager.UnloadUnusedAssetsRoutine())
			{
				while (cleanup.MoveNext())
				{
					object obj = cleanup.Current;
					yield return obj;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06004C9B RID: 19611 RVA: 0x001DBFC9 File Offset: 0x001DA1C9
		private static IEnumerator UnloadUnusedAssetsRoutine()
		{
			AsyncOperation cleanup = Resources.UnloadUnusedAssets();
			float warningAt = Time.realtimeSinceStartup + 15f;
			bool warned = false;
			while (!cleanup.isDone)
			{
				if (!warned && Time.realtimeSinceStartup >= warningAt)
				{
					warned = true;
					Debug.LogWarning("[SceneTransitionManager] UnloadUnusedAssets exceeded 15s; keeping the loading screen visible until Unity completes it.");
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x06004C9C RID: 19612 RVA: 0x001DBFD4 File Offset: 0x001DA1D4
		private static bool ShouldRetireCompletedRun(Scene source, Scene destination)
		{
			return destination.IsValid() && destination.isLoaded && destination.name == "CielHome" && SceneManager.GetActiveScene() == destination && (!source.IsValid() || !source.isLoaded);
		}

		// Token: 0x06004C9D RID: 19613 RVA: 0x001DC02C File Offset: 0x001DA22C
		private static bool TryRecoverNavigationForTransition()
		{
			AstarPath active = AstarPath.active;
			if (active == null || !active.IsNavigationSuspended)
			{
				return true;
			}
			active.RequestNavigationRecovery();
			active.FlushWorkItems();
			if (!active.IsNavigationSuspended)
			{
				return true;
			}
			Debug.LogError("[SceneTransitionManager] Scene transition cancelled because navigation could not be recovered. Keeping the loading screen open cannot repair the graph.");
			return false;
		}

		// Token: 0x06004C9E RID: 19614 RVA: 0x001DC073 File Offset: 0x001DA273
		private static bool ShouldUseTransitionFade(string sourceScene, string destinationScene, bool enablePoiFade)
		{
			return destinationScene == "LongWayHome_RestaurantIntro" || (enablePoiFade && SceneTransitionManager.ShouldFadePoiTransition(sourceScene, destinationScene));
		}

		// Token: 0x06004C9F RID: 19615 RVA: 0x001DC090 File Offset: 0x001DA290
		private static bool ShouldFadePoiTransition(string sourceScene, string destinationScene)
		{
			bool flag = PoiTravelState.IsPoiSceneName(sourceScene);
			bool flag2 = PoiTravelState.IsPoiSceneName(destinationScene);
			return flag != flag2;
		}

		// Token: 0x06004CA0 RID: 19616 RVA: 0x001DC0B0 File Offset: 0x001DA2B0
		private static bool HasActivePoiArrivalPresentation()
		{
			PoiArrivalPresentation poiArrivalPresentation = Object.FindObjectOfType<PoiArrivalPresentation>(true);
			return poiArrivalPresentation != null && poiArrivalPresentation.OwnsDestinationReveal;
		}

		// Token: 0x06004CA1 RID: 19617 RVA: 0x001DC0D8 File Offset: 0x001DA2D8
		private static bool HasActiveCielHomeIntro()
		{
			CielHomeIntroSequence cielHomeIntroSequence = Object.FindObjectOfType<CielHomeIntroSequence>(true);
			return cielHomeIntroSequence != null && cielHomeIntroSequence.OwnsDestinationReveal;
		}

		// Token: 0x06004CA2 RID: 19618 RVA: 0x001DC0FD File Offset: 0x001DA2FD
		private static IEnumerator WaitForLoadingToHide(LoadingScreenController loading)
		{
			LoadingReadinessWatchdog fadeWatchdog = null;
			while (loading != null && loading.IsLoadingActive)
			{
				if (loading.IsPreparingReveal)
				{
					fadeWatchdog = null;
				}
				else
				{
					if (fadeWatchdog == null)
					{
						fadeWatchdog = new LoadingReadinessWatchdog(8f);
					}
					if (fadeWatchdog.Tick(Time.realtimeSinceStartupAsDouble, 0f, 0, 0))
					{
						Debug.LogError("[SceneTransitionManager] Loading fade-out missed its safety deadline; forcing the presentation cleanup.");
						loading.ForceHideImmediate();
						yield break;
					}
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x06004CA3 RID: 19619 RVA: 0x001DC10C File Offset: 0x001DA30C
		private static IEnumerator AnimateFade(ScreenFadeController fade, bool toBlack, float duration)
		{
			if (fade == null)
			{
				yield break;
			}
			if (duration <= 0f)
			{
				if (toBlack)
				{
					fade.SetBlack();
				}
				else
				{
					fade.SetClear();
				}
				yield break;
			}
			bool complete = false;
			if (toBlack)
			{
				fade.FadeOut(duration, delegate
				{
					complete = true;
				});
			}
			else
			{
				fade.FadeIn(duration, delegate
				{
					complete = true;
				});
			}
			float deadline = Time.realtimeSinceStartup + duration + 2f;
			while (fade != null && !complete && Time.realtimeSinceStartup < deadline)
			{
				yield return null;
			}
			if (fade != null && !complete)
			{
				Debug.LogError("[SceneTransitionManager] Screen fade callback missed its safety deadline; forcing the requested terminal alpha.");
				if (toBlack)
				{
					fade.SetBlack();
				}
				else
				{
					fade.SetClear();
				}
			}
			yield break;
		}

		// Token: 0x06004CA4 RID: 19620 RVA: 0x001DC12C File Offset: 0x001DA32C
		private string ResolveCurrentGameplayScene()
		{
			Scene activeScene = SceneManager.GetActiveScene();
			if (activeScene.IsValid() && activeScene.name != this.coreSceneName)
			{
				return activeScene.name;
			}
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				Scene sceneAt = SceneManager.GetSceneAt(i);
				if (sceneAt.name != this.coreSceneName)
				{
					return sceneAt.name;
				}
			}
			return null;
		}

		// Token: 0x04004E05 RID: 19973
		[Tooltip("Nome della scena core persistente durante gli swap di una run (senza estensione).")]
		[SerializeField]
		private string coreSceneName = "_Core";

		// Token: 0x04004E06 RID: 19974
		[Tooltip("Scena caricata in additive quando _Core viene avviato da solo (fallback/editor). Sovrascrivibile via PendingGameplayScene.")]
		[SerializeField]
		private string defaultGameplayScene = "Game - Current";

		// Token: 0x04004E07 RID: 19975
		[Tooltip("Libera gli asset non più referenziati prima del caricamento e dopo lo scaricamento della vecchia scena.")]
		[SerializeField]
		private bool unloadUnusedAssets = true;

		// Token: 0x04004E08 RID: 19976
		[Tooltip("Seconds without initialization progress before showing loading recovery. Healthy loading may take longer.")]
		[SerializeField]
		private float gameplayReadyTimeout = 35f;

		// Token: 0x04004E09 RID: 19977
		[Header("POI Transition Fade")]
		[Tooltip("Sfuma il gameplay al nero prima di attraversare il confine mappa/POI e rivela dolcemente la destinazione dopo il loading.")]
		[SerializeField]
		private bool fadePoiTransitions = true;

		// Token: 0x04004E0A RID: 19978
		[SerializeField]
		[Min(0f)]
		private float poiFadeOutDuration = 0.35f;

		// Token: 0x04004E0B RID: 19979
		[SerializeField]
		[Min(0f)]
		private float poiFadeInDuration = 0.45f;

		// Token: 0x04004E0C RID: 19980
		[Tooltip("Seconds of playable time after loading/reveal before NPCs can detect or capture Ciel.")]
		[SerializeField]
		[Min(0f)]
		private float arrivalProtectionDuration = 5f;

		// Token: 0x04004E0D RID: 19981
		private float _arrivalProtectionUntil;

		// Token: 0x04004E0E RID: 19982
		private bool _arrivalProtectionPendingReveal;

		// Token: 0x04004E0F RID: 19983
		[SerializeField]
		private bool enableDebugLogs = true;
	}
}
