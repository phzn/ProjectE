using System;
using System.Collections;
using CompassNavigatorPro;
using ExhibitionistDemo.UI;
using Features.Detection;
using Features.Loading;
using Features.MatchRecap;
using Features.NPC;
using Features.PlayerEmotions;
using Features.Police;
using Features.RunModes;
using Features.UI.TimedEffects;
using Features.Vehicles.Spawning;
using Features.WantedPoster;
using ProjectExhibitionist.Core.Rendering;
using ProjectExhibitionist.Core.SceneManagement;
using UnityEngine;
using UnityEngine.AzureSky;
using UnityEngine.SceneManagement;
using VehicleSpawnPlugin;

// Token: 0x02000016 RID: 22
[DefaultExecutionOrder(-1000)]
public class GameSystemsBootstrapper : MonoBehaviour
{
	// Token: 0x17000010 RID: 16
	// (get) Token: 0x06000082 RID: 130 RVA: 0x00005332 File Offset: 0x00003532
	// (set) Token: 0x06000083 RID: 131 RVA: 0x00005339 File Offset: 0x00003539
	public static GameSystemsBootstrapper Instance { get; private set; }

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x06000084 RID: 132 RVA: 0x00005341 File Offset: 0x00003541
	// (set) Token: 0x06000085 RID: 133 RVA: 0x00005348 File Offset: 0x00003548
	public static bool GameplayReady { get; private set; }

	// Token: 0x17000012 RID: 18
	// (get) Token: 0x06000086 RID: 134 RVA: 0x00005350 File Offset: 0x00003550
	// (set) Token: 0x06000087 RID: 135 RVA: 0x00005357 File Offset: 0x00003557
	public static bool GameplayReadyDegraded { get; private set; }

	// Token: 0x17000013 RID: 19
	// (get) Token: 0x06000088 RID: 136 RVA: 0x0000535F File Offset: 0x0000355F
	// (set) Token: 0x06000089 RID: 137 RVA: 0x00005366 File Offset: 0x00003566
	public static bool GameplayReadinessFailed { get; private set; }

	// Token: 0x17000014 RID: 20
	// (get) Token: 0x0600008A RID: 138 RVA: 0x0000536E File Offset: 0x0000356E
	// (set) Token: 0x0600008B RID: 139 RVA: 0x00005375 File Offset: 0x00003575
	public static string LastReadinessDiagnostic { get; private set; }

	// Token: 0x17000015 RID: 21
	// (get) Token: 0x0600008C RID: 140 RVA: 0x0000537D File Offset: 0x0000357D
	public static float InitializationProgress
	{
		get
		{
			if (!(GameSystemsBootstrapper.Instance != null))
			{
				return 0f;
			}
			return GameSystemsBootstrapper.Instance.CalculateInitializationProgress();
		}
	}

	// Token: 0x17000016 RID: 22
	// (get) Token: 0x0600008D RID: 141 RVA: 0x0000539C File Offset: 0x0000359C
	public static int InitializationWorkVersion
	{
		get
		{
			return (int)Math.Min(2147483647L, (long)SceneBootLoader.InitializationWorkVersion + (long)((ChunkManager.Instance != null) ? ChunkManager.Instance.InitializationWorkVersion : 0));
		}
	}

	// Token: 0x0600008E RID: 142 RVA: 0x000053CC File Offset: 0x000035CC
	public static bool IsRunReady(bool requireNPCs = false)
	{
		GameSystemsBootstrapper instance = GameSystemsBootstrapper.Instance;
		return GameSystemsBootstrapper.GameplayReady && instance != null && instance.participatesInRun && (!requireNPCs || instance.enableNPCs);
	}

	// Token: 0x14000001 RID: 1
	// (add) Token: 0x0600008F RID: 143 RVA: 0x00005404 File Offset: 0x00003604
	// (remove) Token: 0x06000090 RID: 144 RVA: 0x00005438 File Offset: 0x00003638
	public static event Action OnGameplayReady;

	// Token: 0x17000017 RID: 23
	// (get) Token: 0x06000091 RID: 145 RVA: 0x0000546B File Offset: 0x0000366B
	public string InitializedSceneName
	{
		get
		{
			return this._lastInitializedScene;
		}
	}

	// Token: 0x17000018 RID: 24
	// (get) Token: 0x06000092 RID: 146 RVA: 0x00005473 File Offset: 0x00003673
	public Transform Player
	{
		get
		{
			return this.player;
		}
	}

	// Token: 0x17000019 RID: 25
	// (get) Token: 0x06000093 RID: 147 RVA: 0x0000547B File Offset: 0x0000367B
	public Camera MainCamera
	{
		get
		{
			return this.mainCamera;
		}
	}

	// Token: 0x1700001A RID: 26
	// (get) Token: 0x06000094 RID: 148 RVA: 0x00005483 File Offset: 0x00003683
	public Transform StaticChunksContainer
	{
		get
		{
			return this.staticChunksContainer;
		}
	}

	// Token: 0x1700001B RID: 27
	// (get) Token: 0x06000095 RID: 149 RVA: 0x0000548B File Offset: 0x0000368B
	public float CellSize
	{
		get
		{
			return this.cellSize;
		}
	}

	// Token: 0x1700001C RID: 28
	// (get) Token: 0x06000096 RID: 150 RVA: 0x00005493 File Offset: 0x00003693
	public bool MinimapEnabled
	{
		get
		{
			return this.enableMinimap;
		}
	}

	// Token: 0x06000097 RID: 151 RVA: 0x0000549C File Offset: 0x0000369C
	private void Awake()
	{
		if (GameSystemsBootstrapper.Instance != null && GameSystemsBootstrapper.Instance != this)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		GameSystemsBootstrapper.Instance = this;
		PlayerRegistry.PlayerChanged += this.OnPlayerChanged;
		this.ApplySceneProfile();
		this.GateManagersByProfile();
	}

	// Token: 0x06000098 RID: 152 RVA: 0x000054F2 File Offset: 0x000036F2
	private void Start()
	{
		base.StartCoroutine(this.InitializeWhenReady());
	}

	// Token: 0x06000099 RID: 153 RVA: 0x00005504 File Offset: 0x00003704
	private void GateManagersByProfile()
	{
		GameSystemsBootstrapper.GameplayReady = false;
		GameSystemsBootstrapper.GameplayReadyDegraded = false;
		GameSystemsBootstrapper.GameplayReadinessFailed = false;
		GameSystemsBootstrapper.LastReadinessDiagnostic = null;
		bool flag = this.enableNPCs || this.enableReporters || this.enablePolice;
		GameSystemsBootstrapper.SetEnabled(Object.FindObjectOfType<ChunkManager>(true), this.enableProceduralMap);
		GameSystemsBootstrapper.SetEnabled(Object.FindObjectOfType<NPCZoneManager>(true), this.enableNPCs);
		GameSystemsBootstrapper.SetEnabled(Object.FindObjectOfType<NPCRuntimeSpawner>(true), this.enableNPCs);
		GameSystemsBootstrapper.SetEnabled(Object.FindObjectOfType<ReporterProceduralSpawner>(true), this.enableNPCs && this.enableReporters);
		GameSystemsBootstrapper.SetEnabled(Object.FindObjectOfType<PoliceManager>(true), this.enablePolice);
		GameSystemsBootstrapper.SetEnabled(Object.FindObjectOfType<StreetLightManager>(true), this.enableStreetLights);
		GameSystemsBootstrapper.SetEnabled(Object.FindObjectOfType<AStarGraphManager>(true), flag);
		bool flag2 = this.participatesInRun && this.enableVehicles;
		if (this.vehicleSpawnManager == null)
		{
			this.vehicleSpawnManager = Object.FindObjectOfType<VehicleSpawnManager>(true);
		}
		if (!flag2 && this.vehicleSpawnManager != null)
		{
			this.vehicleSpawnManager.ShutdownForCurrentScene();
		}
		GameSystemsBootstrapper.SetEnabled(this.vehicleSpawnManager, flag2);
		if (this.ambientVehicleSpawner == null)
		{
			this.ambientVehicleSpawner = Object.FindObjectOfType<VehicleSpawner>(true);
		}
		if (this.ambientVehicleSpawner != null)
		{
			this.ambientVehicleSpawner.SetRuntimeActive(flag2);
		}
		bool flag3 = this.enableNPCs || this.enableDetection;
		SensoryManager sensoryManager = Object.FindObjectOfType<SensoryManager>(true);
		GameSystemsBootstrapper.SetEnabled(sensoryManager, flag3);
		if (sensoryManager != null)
		{
			sensoryManager.DisableFinalDetection = !this.enableDetection;
		}
		GameSystemsBootstrapper.SetEnabled(Object.FindObjectOfType<ExhibitionEventDirector>(true), this.enableDetection);
		GameSystemsBootstrapper.SetEnabled(Object.FindObjectOfType<HUDManager>(true), this.enableDetection);
		GameSystemsBootstrapper.SetEnabled(Object.FindObjectOfType<EmotionManager>(true), this.enablePlayerEmotions);
		GameSystemsBootstrapper.SetEnabled(Object.FindObjectOfType<TimedEffectHUDService>(true), true);
		WantedPosterPhotoSystem wantedPosterPhotoSystem = Object.FindObjectOfType<WantedPosterPhotoSystem>(true);
		if (!this.participatesInRun && wantedPosterPhotoSystem != null)
		{
			wantedPosterPhotoSystem.ResetForNewRun();
		}
		GameSystemsBootstrapper.SetEnabled(wantedPosterPhotoSystem, this.participatesInRun);
		GameSystemsBootstrapper.SetEnabled(Object.FindObjectOfType<WantedPosterNpcDirector>(true), this.participatesInRun && this.enableNPCs);
		GameSystemsBootstrapper.SetEnabled(Object.FindObjectOfType<WantedPosterBreakController>(true), this.participatesInRun);
		GameSystemsBootstrapper.SetEnabled(Object.FindObjectOfType<WantedPosterHeldActions>(true), this.participatesInRun);
		this.GateRunOnlyHudAndTracking();
		if (this.enableDebugLogs)
		{
			string text = (flag3 ? (this.enableDetection ? "FULL" : "PRE-DETECTION-ONLY") : "OFF");
			Debug.Log(string.Concat(new string[]
			{
				"[GameSystemsBootstrapper] Manager gated da profilo (Awake): ",
				string.Format("map={0}, npc={1}, reporters={2}, ", this.enableProceduralMap, this.enableNPCs, this.enableReporters),
				string.Format("police={0}, detection={1}, emotions={2}, ", this.enablePolice, this.enableDetection, this.enablePlayerEmotions),
				string.Format("streetLights={0}, vehicles={1}, ", this.enableStreetLights, flag2),
				string.Format("aStar={0}, sensory={1}, ", flag, text),
				string.Format("run={0}.", this.participatesInRun)
			}));
		}
	}

	// Token: 0x0600009A RID: 154 RVA: 0x0000581C File Offset: 0x00003A1C
	private void GateRunOnlyHudAndTracking()
	{
		this.SetCoreSceneObjectActive("Compass", this.participatesInRun);
		this.SetCoreSceneObjectActive("Scores", this.participatesInRun);
		CompassPro[] array = Object.FindObjectsOfType<CompassPro>(true);
		for (int i = 0; i < array.Length; i++)
		{
			array[i].showMiniMap = this.enableMinimap && !this.enableProceduralMap;
		}
		DistanceTracker distanceTracker = Object.FindObjectOfType<DistanceTracker>(true);
		if (distanceTracker != null)
		{
			if (!this.participatesInRun)
			{
				distanceTracker.ResetForNewRun();
			}
			distanceTracker.enabled = this.participatesInRun;
		}
		MatchStatsService instance = MatchStatsService.Instance;
		if (instance != null)
		{
			if (this.participatesInRun)
			{
				instance.SetMockedOverride(false);
			}
			instance.SetRunTrackingActive(this.participatesInRun);
			if (!this.participatesInRun)
			{
				instance.ResetForNewRun();
				instance.SetMockedOverride(true);
			}
		}
	}

	// Token: 0x0600009B RID: 155 RVA: 0x000058E8 File Offset: 0x00003AE8
	private void SetCoreSceneObjectActive(string objectName, bool active)
	{
		Scene scene = base.gameObject.scene;
		if (!scene.IsValid() || !scene.isLoaded)
		{
			return;
		}
		GameObject[] rootGameObjects = scene.GetRootGameObjects();
		for (int i = 0; i < rootGameObjects.Length; i++)
		{
			Transform[] componentsInChildren = rootGameObjects[i].GetComponentsInChildren<Transform>(true);
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				if (!(componentsInChildren[j].name != objectName))
				{
					GameObject gameObject = componentsInChildren[j].gameObject;
					if (gameObject.activeSelf != active)
					{
						gameObject.SetActive(active);
					}
					return;
				}
			}
		}
		if (this.enableDebugLogs)
		{
			Debug.LogWarning("[GameSystemsBootstrapper] HUD run-only '" + objectName + "' non trovato nella scena _Core.");
		}
	}

	// Token: 0x0600009C RID: 156 RVA: 0x00005992 File Offset: 0x00003B92
	private static void SetEnabled(Behaviour comp, bool value)
	{
		if (comp != null)
		{
			comp.enabled = value;
		}
	}

	// Token: 0x0600009D RID: 157 RVA: 0x000059A4 File Offset: 0x00003BA4
	public bool GetFeatureEnabled(GameSystemsBootstrapper.RuntimeFeature feature)
	{
		bool flag;
		switch (feature)
		{
		case GameSystemsBootstrapper.RuntimeFeature.ProceduralMap:
			flag = this.enableProceduralMap;
			break;
		case GameSystemsBootstrapper.RuntimeFeature.Vehicles:
			flag = this.enableVehicles;
			break;
		case GameSystemsBootstrapper.RuntimeFeature.NPCs:
			flag = this.enableNPCs;
			break;
		case GameSystemsBootstrapper.RuntimeFeature.Reporters:
			flag = this.enableReporters;
			break;
		case GameSystemsBootstrapper.RuntimeFeature.Police:
			flag = this.enablePolice;
			break;
		case GameSystemsBootstrapper.RuntimeFeature.Detection:
			flag = this.enableDetection;
			break;
		case GameSystemsBootstrapper.RuntimeFeature.PlayerEmotions:
			flag = this.enablePlayerEmotions;
			break;
		case GameSystemsBootstrapper.RuntimeFeature.StreetLights:
			flag = this.enableStreetLights;
			break;
		case GameSystemsBootstrapper.RuntimeFeature.FPSCounter:
			flag = this.enableFPSCounter;
			break;
		default:
			flag = false;
			break;
		}
		return flag;
	}

	// Token: 0x0600009E RID: 158 RVA: 0x00005A34 File Offset: 0x00003C34
	public void SetFeatureEnabled(GameSystemsBootstrapper.RuntimeFeature feature, bool value)
	{
		switch (feature)
		{
		case GameSystemsBootstrapper.RuntimeFeature.ProceduralMap:
			this.enableProceduralMap = value;
			break;
		case GameSystemsBootstrapper.RuntimeFeature.Vehicles:
			this.enableVehicles = value;
			break;
		case GameSystemsBootstrapper.RuntimeFeature.NPCs:
			this.enableNPCs = value;
			break;
		case GameSystemsBootstrapper.RuntimeFeature.Reporters:
			this.enableReporters = value;
			break;
		case GameSystemsBootstrapper.RuntimeFeature.Police:
			this.enablePolice = value;
			break;
		case GameSystemsBootstrapper.RuntimeFeature.Detection:
			this.enableDetection = value;
			break;
		case GameSystemsBootstrapper.RuntimeFeature.PlayerEmotions:
			this.enablePlayerEmotions = value;
			break;
		case GameSystemsBootstrapper.RuntimeFeature.StreetLights:
			this.enableStreetLights = value;
			break;
		case GameSystemsBootstrapper.RuntimeFeature.FPSCounter:
			this.enableFPSCounter = value;
			break;
		}
		this.ApplyFeatureTogglesLive();
	}

	// Token: 0x0600009F RID: 159 RVA: 0x00005AC4 File Offset: 0x00003CC4
	public void ApplyFeatureTogglesLive()
	{
		bool flag = this.enableNPCs || this.enableReporters || this.enablePolice;
		GameSystemsBootstrapper.SetEnabled(Object.FindObjectOfType<ChunkManager>(true), this.enableProceduralMap);
		GameSystemsBootstrapper.SetEnabled(Object.FindObjectOfType<NPCZoneManager>(true), this.enableNPCs);
		GameSystemsBootstrapper.SetEnabled(Object.FindObjectOfType<NPCRuntimeSpawner>(true), this.enableNPCs);
		GameSystemsBootstrapper.SetEnabled(Object.FindObjectOfType<ReporterProceduralSpawner>(true), this.enableNPCs && this.enableReporters);
		GameSystemsBootstrapper.SetEnabled(Object.FindObjectOfType<PoliceManager>(true), this.enablePolice);
		GameSystemsBootstrapper.SetEnabled(Object.FindObjectOfType<StreetLightManager>(true), this.enableStreetLights);
		GameSystemsBootstrapper.SetEnabled(Object.FindObjectOfType<AStarGraphManager>(true), flag);
		bool flag2 = this.participatesInRun && this.enableVehicles;
		if (this.vehicleSpawnManager == null)
		{
			this.vehicleSpawnManager = Object.FindObjectOfType<VehicleSpawnManager>(true);
		}
		if (!flag2 && this.vehicleSpawnManager != null)
		{
			this.vehicleSpawnManager.ShutdownForCurrentScene();
		}
		GameSystemsBootstrapper.SetEnabled(this.vehicleSpawnManager, flag2);
		if (flag2 && this.vehicleSpawnManager != null)
		{
			this.vehicleSpawnManager.ReinitializeForCurrentScene(null, this.enableProceduralMap);
		}
		if (this.ambientVehicleSpawner == null)
		{
			this.ambientVehicleSpawner = Object.FindObjectOfType<VehicleSpawner>(true);
		}
		if (this.ambientVehicleSpawner != null)
		{
			this.ambientVehicleSpawner.SetRuntimeActive(flag2);
		}
		GameSystemsBootstrapper.SetEnabled(Object.FindObjectOfType<FPSCounter>(true), false);
		bool flag3 = this.enableNPCs || this.enableDetection;
		SensoryManager sensoryManager = Object.FindObjectOfType<SensoryManager>(true);
		GameSystemsBootstrapper.SetEnabled(sensoryManager, flag3);
		if (sensoryManager != null)
		{
			sensoryManager.DisableFinalDetection = !this.enableDetection;
		}
		GameSystemsBootstrapper.SetEnabled(Object.FindObjectOfType<ExhibitionEventDirector>(true), this.enableDetection);
		GameSystemsBootstrapper.SetEnabled(Object.FindObjectOfType<HUDManager>(true), this.enableDetection);
		GameSystemsBootstrapper.SetEnabled(Object.FindObjectOfType<EmotionManager>(true), this.enablePlayerEmotions);
		GameSystemsBootstrapper.SetEnabled(Object.FindObjectOfType<TimedEffectHUDService>(true), true);
		GameSystemsBootstrapper.SetEnabled(Object.FindObjectOfType<WantedPosterPhotoSystem>(true), this.participatesInRun);
		GameSystemsBootstrapper.SetEnabled(Object.FindObjectOfType<WantedPosterNpcDirector>(true), this.participatesInRun && this.enableNPCs);
		GameSystemsBootstrapper.SetEnabled(Object.FindObjectOfType<WantedPosterBreakController>(true), this.participatesInRun);
		GameSystemsBootstrapper.SetEnabled(Object.FindObjectOfType<WantedPosterHeldActions>(true), this.participatesInRun);
		if (this.enableDebugLogs)
		{
			Debug.Log(string.Format("[GameSystemsBootstrapper] Toggle live applicati: map={0}, npc={1}, ", this.enableProceduralMap, this.enableNPCs) + string.Format("reporters={0}, police={1}, detection={2}, ", this.enableReporters, this.enablePolice, this.enableDetection) + string.Format("emotions={0}, streetLights={1}, ", this.enablePlayerEmotions, this.enableStreetLights) + string.Format("vehicles={0}, fps={1}.", flag2, this.enableFPSCounter));
		}
	}

	// Token: 0x060000A0 RID: 160 RVA: 0x00005D80 File Offset: 0x00003F80
	public void ReinitializeForCurrentScene()
	{
		Scene activeScene = SceneManager.GetActiveScene();
		string name = activeScene.name;
		if (activeScene.handle == this._initializedSceneHandle && !GameSystemsBootstrapper.GameplayReadinessFailed)
		{
			if (this.enableDebugLogs)
			{
				Debug.Log(string.Concat(new string[]
				{
					"[GameSystemsBootstrapper] ReinitializeForCurrentScene: '",
					name,
					"' già ",
					GameSystemsBootstrapper.GameplayReady ? "inizializzata" : "in inizializzazione",
					", no-op."
				}));
			}
			return;
		}
		bool flag = name == "Game - Current" && (this._lastInitializedScene == "CielHome" || this._lastInitializedScene == "LongWayHome_RestaurantIntro" || this._lastInitializedScene == "MainMenu" || LongWayHomeRuntimeController.StartsCityAfterPoiPrologue);
		if (this._systemsReadyRoutine != null)
		{
			base.StopCoroutine(this._systemsReadyRoutine);
			this._systemsReadyRoutine = null;
		}
		int num = this._readinessGeneration + 1;
		this._readinessGeneration = num;
		int num2 = num;
		this._lastInitializedScene = name;
		this._initializedSceneHandle = activeScene.handle;
		this._sceneSystemsInitialized = false;
		this._startsNewCityRun = flag;
		this.player = PlayerRegistry.Player;
		if (this.mainCamera == null)
		{
			this.mainCamera = Camera.main;
		}
		this.ApplySceneProfile();
		this.GateManagersByProfile();
		this.TryInitializeSceneSystems();
		this._systemsReadyRoutine = base.StartCoroutine(this.WaitForSystemsReady(num2));
		if (this.enableDebugLogs)
		{
			Debug.Log("[GameSystemsBootstrapper] ReinitializeForCurrentScene: profilo riapplicato per la scena attiva.");
		}
	}

	// Token: 0x060000A1 RID: 161 RVA: 0x00005EF8 File Offset: 0x000040F8
	public static void InvalidateForSceneTransition()
	{
		GameSystemsBootstrapper.GameplayReady = false;
		GameSystemsBootstrapper.GameplayReadyDegraded = false;
		GameSystemsBootstrapper.GameplayReadinessFailed = false;
		GameSystemsBootstrapper.LastReadinessDiagnostic = null;
		GameSystemsBootstrapper instance = GameSystemsBootstrapper.Instance;
		if (instance == null)
		{
			return;
		}
		instance._readinessGeneration++;
		if (instance._systemsReadyRoutine != null)
		{
			instance.StopCoroutine(instance._systemsReadyRoutine);
		}
		instance._systemsReadyRoutine = null;
		instance._playerRebindLoadingOwner = null;
		instance._initializedSceneHandle = -1;
		instance._sceneSystemsInitialized = false;
		instance.player = null;
		instance._readinessSpawner = null;
		instance._readinessSpawnerSearchFrame = -1;
	}

	// Token: 0x060000A2 RID: 162 RVA: 0x00005F80 File Offset: 0x00004180
	private bool HasCurrentSceneBinding()
	{
		Scene activeScene = SceneManager.GetActiveScene();
		SceneContext sceneContext = SceneContext.Current;
		return activeScene.IsValid() && activeScene.isLoaded && activeScene.handle == this._initializedSceneHandle && sceneContext != null && sceneContext.gameObject.scene == activeScene;
	}

	// Token: 0x060000A3 RID: 163 RVA: 0x00005FD8 File Offset: 0x000041D8
	private bool HasLivePlayerBinding()
	{
		Transform transform = PlayerRegistry.Player;
		return transform != null && this.player == transform && transform.gameObject.activeInHierarchy && transform.gameObject.scene.isLoaded;
	}

	// Token: 0x060000A4 RID: 164 RVA: 0x00006024 File Offset: 0x00004224
	private void TryInitializeSceneSystems()
	{
		if (this._sceneSystemsInitialized || !this.HasCurrentSceneBinding())
		{
			return;
		}
		this.player = PlayerRegistry.Player;
		if (!this.HasLivePlayerBinding())
		{
			return;
		}
		if (this.mainCamera == null)
		{
			this.mainCamera = Camera.main;
		}
		this.InitializeSystems(this._startsNewCityRun);
		this._startsNewCityRun = false;
		this._sceneSystemsInitialized = true;
	}

	// Token: 0x060000A5 RID: 165 RVA: 0x0000608C File Offset: 0x0000428C
	private void OnPlayerChanged(Transform registered)
	{
		if (this.player == registered)
		{
			return;
		}
		this.player = null;
		GameSystemsBootstrapper.GameplayReady = false;
		this._sceneSystemsInitialized = false;
		if (this._initializedSceneHandle < 0 || GameSystemsBootstrapper.GameplayReadinessFailed || !base.isActiveAndEnabled)
		{
			return;
		}
		if (this._systemsReadyRoutine != null)
		{
			base.StopCoroutine(this._systemsReadyRoutine);
		}
		int num = this._readinessGeneration + 1;
		this._readinessGeneration = num;
		int num2 = num;
		LoadingScreenController loadingScreenController = this.AcquirePlayerRebindLoading();
		this._systemsReadyRoutine = base.StartCoroutine(this.RebindAfterPlayerChange(num2, loadingScreenController));
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x00006114 File Offset: 0x00004314
	private LoadingScreenController AcquirePlayerRebindLoading()
	{
		LoadingScreenController existingInstance = LoadingScreenController.ExistingInstance;
		if (this._playerRebindLoadingOwner != existingInstance)
		{
			this._playerRebindLoadingOwner = null;
		}
		if (existingInstance != null && (this._playerRebindLoadingOwner == existingInstance || !existingInstance.IsLoadingActive))
		{
			this._playerRebindLoadingOwner = existingInstance;
			existingInstance.BeginManualLoad();
		}
		return existingInstance;
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x00006169 File Offset: 0x00004369
	private IEnumerator RebindAfterPlayerChange(int generation, LoadingScreenController loading)
	{
		yield return null;
		if (generation != this._readinessGeneration)
		{
			yield break;
		}
		yield return this.WaitForSystemsReady(generation);
		if (generation != this._readinessGeneration)
		{
			yield break;
		}
		if (GameSystemsBootstrapper.GameplayReadinessFailed)
		{
			this._playerRebindLoadingOwner = null;
			if (loading != null)
			{
				loading.ShowLoadFailure();
			}
			yield break;
		}
		if (loading == null || this._playerRebindLoadingOwner != loading)
		{
			yield break;
		}
		loading.EndManualLoad();
		while (generation == this._readinessGeneration && loading != null && loading.IsLoadingActive)
		{
			yield return null;
		}
		if (generation == this._readinessGeneration && this._playerRebindLoadingOwner == loading)
		{
			this._playerRebindLoadingOwner = null;
		}
		yield break;
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x00006186 File Offset: 0x00004386
	private IEnumerator InitializeWhenReady()
	{
		float elapsed = 0f;
		while (this.player == null)
		{
			this.player = PlayerRegistry.Player;
			if (this.player == null)
			{
				GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
				if (gameObject != null)
				{
					this.player = gameObject.transform;
				}
			}
			if (this.player != null)
			{
				break;
			}
			elapsed += Time.unscaledDeltaTime;
			if (elapsed >= 5f)
			{
				Debug.LogError("[GameSystemsBootstrapper] Player not found dopo " + string.Format("{0}s di attesa! Inizializzazione annullata.", 5f));
				yield break;
			}
			yield return null;
		}
		float gpElapsed = 0f;
		while (SceneManager.GetActiveScene().name == "_Core" || SceneContext.Current == null)
		{
			gpElapsed += Time.unscaledDeltaTime;
			if (gpElapsed >= 60f)
			{
				Debug.LogWarning("[GameSystemsBootstrapper] Nessuna scena di gioco attiva dopo " + string.Format("{0}s: inizializzazione auto annullata.", 60f));
				yield break;
			}
			yield return null;
		}
		if (this.mainCamera == null)
		{
			this.mainCamera = Camera.main;
		}
		this.ReinitializeForCurrentScene();
		yield break;
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x00006195 File Offset: 0x00004395
	private IEnumerator WaitForSystemsReady(int generation)
	{
		float elapsed = 0f;
		double waitStartedAt = Time.realtimeSinceStartupAsDouble;
		float hardDeadline = Mathf.Max(5f, this.readinessSafetyTimeout);
		LoadingReadinessWatchdog watchdog = new LoadingReadinessWatchdog(hardDeadline);
		try
		{
			for (;;)
			{
				if (generation != this._readinessGeneration)
				{
					break;
				}
				if (GameSystemsBootstrapper.GameplayReady || GameSystemsBootstrapper.GameplayReadinessFailed)
				{
					goto IL_00CB;
				}
				this.TryInitializeSceneSystems();
				bool flag = this._sceneSystemsInitialized && this.HasCurrentSceneBinding();
				bool flag2 = this.HasLivePlayerBinding();
				bool flag3 = this.IsRequiredNavigationReady();
				bool mapReady = !this.enableProceduralMap || (ChunkManager.Instance != null && ChunkManager.Instance.IsMapReady);
				NPCZoneManager zoneMgr = ((this.npcZoneManager != null) ? this.npcZoneManager : Object.FindObjectOfType<NPCZoneManager>());
				NPCRuntimeSpawner spawner = ((zoneMgr != null) ? this.GetReadinessSpawner() : null);
				if (this.enableNPCs && zoneMgr != null && zoneMgr.SpawnPoolFailure != null)
				{
					goto Block_12;
				}
				bool flag4 = zoneMgr != null && zoneMgr.IsSpawnPoolReady;
				bool flag5 = zoneMgr != null && (zoneMgr.IsInitialPopulationComplete || NPCSimulationPause.IsPaused);
				bool npcReady = !this.enableNPCs || (zoneMgr != null && flag4 && flag5);
				bool vehiclesReady = !this.participatesInRun || !this.enableVehicles || (this.vehicleSpawnManager != null && this.vehicleSpawnManager.IsInitialSpawnComplete);
				if ((flag && flag2 && flag3) & mapReady & npcReady & vehiclesReady)
				{
					goto Block_20;
				}
				int num = (mapReady ? 1 : 0) | (npcReady ? 2 : 0) | (vehiclesReady ? 4 : 0) | (flag ? 8 : 0) | (flag2 ? 16 : 0) | (flag3 ? 32 : 0);
				elapsed = (float)(Time.realtimeSinceStartupAsDouble - waitStartedAt);
				if (watchdog.Tick(Time.realtimeSinceStartupAsDouble, GameSystemsBootstrapper.InitializationProgress, GameSystemsBootstrapper.InitializationWorkVersion, num))
				{
					if (PlayerRegistry.Player == null)
					{
						goto Block_28;
					}
					LoadingStallMonitor.Report(this, true, delegate
					{
						GameSystemsBootstrapper.LastReadinessDiagnostic = this.BuildReadinessDiagnostic(mapReady, npcReady, vehiclesReady, zoneMgr, spawner);
						return string.Format("no progress for {0:0.#}s (total {1:0.#}s): {2}", hardDeadline, elapsed, GameSystemsBootstrapper.LastReadinessDiagnostic);
					});
				}
				else
				{
					LoadingStallMonitor.Report(this, false, null);
				}
				yield return null;
			}
			yield break;
			IL_00CB:
			yield break;
			Block_12:
			GameSystemsBootstrapper.FailGameplayReadiness("NPC preparation failed: " + CS$<>8__locals2.zoneMgr.SpawnPoolFailure);
			this._systemsReadyRoutine = null;
			yield break;
			Block_20:
			LoadingStallMonitor.Report(this, false, null);
			goto JumpOutOfTryFinally-3;
			Block_28:
			string text = this.BuildReadinessDiagnostic(CS$<>8__locals2.mapReady, CS$<>8__locals2.npcReady, CS$<>8__locals2.vehiclesReady, CS$<>8__locals2.zoneMgr, CS$<>8__locals2.spawner);
			GameSystemsBootstrapper.FailGameplayReadiness(string.Format("no registered player after {0:0.#}s without progress ", CS$<>8__locals2.CS$<>8__locals1.hardDeadline) + string.Format("(total {0:0.#}s): {1}", CS$<>8__locals2.CS$<>8__locals1.elapsed, text));
			this._systemsReadyRoutine = null;
			yield break;
		}
		finally
		{
			LoadingStallMonitor.Remove(this);
		}
		JumpOutOfTryFinally-3:
		this.CompleteGameplayReadiness(generation, false, null);
		yield break;
		yield break;
	}

	// Token: 0x060000AA RID: 170 RVA: 0x000061AC File Offset: 0x000043AC
	public static bool ForceCompleteGameplayReadiness(string source)
	{
		if (GameSystemsBootstrapper.GameplayReady)
		{
			return true;
		}
		if (GameSystemsBootstrapper.GameplayReadinessFailed)
		{
			return false;
		}
		if (GameSystemsBootstrapper.Instance == null || PlayerRegistry.Player == null)
		{
			GameSystemsBootstrapper.FailGameplayReadiness(source + ": " + ((GameSystemsBootstrapper.Instance != null) ? GameSystemsBootstrapper.Instance.GetReadinessDiagnostic() : "GameSystemsBootstrapper missing"));
			return false;
		}
		if (!GameSystemsBootstrapper.Instance.AreRequiredSystemsReady())
		{
			return false;
		}
		GameSystemsBootstrapper.Instance.CompleteGameplayReadiness(GameSystemsBootstrapper.Instance._readinessGeneration, true, source + ": " + GameSystemsBootstrapper.Instance.GetReadinessDiagnostic());
		return GameSystemsBootstrapper.GameplayReady;
	}

	// Token: 0x060000AB RID: 171 RVA: 0x00006252 File Offset: 0x00004452
	public static void FailGameplayReadiness(string diagnostic)
	{
		if (GameSystemsBootstrapper.GameplayReadinessFailed)
		{
			return;
		}
		GameSystemsBootstrapper.GameplayReady = false;
		GameSystemsBootstrapper.GameplayReadyDegraded = false;
		GameSystemsBootstrapper.GameplayReadinessFailed = true;
		GameSystemsBootstrapper.LastReadinessDiagnostic = diagnostic;
		Debug.LogError("[GameSystemsBootstrapper] Required scene preparation failed; gameplay remains gated. " + diagnostic);
	}

	// Token: 0x060000AC RID: 172 RVA: 0x00006284 File Offset: 0x00004484
	private bool IsRequiredNavigationReady()
	{
		return (!this.enableNPCs && !this.enableReporters && !this.enablePolice) || (this.graphManager != null && this.graphManager.IsReady);
	}

	// Token: 0x060000AD RID: 173 RVA: 0x000062BC File Offset: 0x000044BC
	private bool AreRequiredSystemsReady()
	{
		if (!this._sceneSystemsInitialized || !this.HasCurrentSceneBinding() || !this.HasLivePlayerBinding() || !this.IsRequiredNavigationReady())
		{
			return false;
		}
		if (this.enableProceduralMap && (ChunkManager.Instance == null || !ChunkManager.Instance.IsMapReady))
		{
			return false;
		}
		if (this.enableNPCs)
		{
			NPCZoneManager npczoneManager = ((this.npcZoneManager != null) ? this.npcZoneManager : Object.FindObjectOfType<NPCZoneManager>());
			if (npczoneManager == null || !npczoneManager.IsSpawnPoolReady || (!npczoneManager.IsInitialPopulationComplete && !NPCSimulationPause.IsPaused))
			{
				return false;
			}
		}
		return !this.participatesInRun || !this.enableVehicles || (this.vehicleSpawnManager != null && this.vehicleSpawnManager.IsInitialSpawnComplete);
	}

	// Token: 0x060000AE RID: 174 RVA: 0x00006384 File Offset: 0x00004584
	public string GetReadinessDiagnostic()
	{
		bool flag = !this.enableProceduralMap || (ChunkManager.Instance != null && ChunkManager.Instance.IsMapReady);
		NPCZoneManager npczoneManager = ((this.npcZoneManager != null) ? this.npcZoneManager : Object.FindObjectOfType<NPCZoneManager>());
		NPCRuntimeSpawner npcruntimeSpawner = ((npczoneManager != null) ? this.GetReadinessSpawner() : null);
		bool flag2 = npczoneManager != null && npczoneManager.IsSpawnPoolReady;
		bool flag3 = npczoneManager != null && (npczoneManager.IsInitialPopulationComplete || NPCSimulationPause.IsPaused);
		bool flag4 = !this.enableNPCs || (npczoneManager != null && flag2 && flag3);
		bool flag5 = !this.participatesInRun || !this.enableVehicles || (this.vehicleSpawnManager != null && this.vehicleSpawnManager.IsInitialSpawnComplete);
		return this.BuildReadinessDiagnostic(flag, flag4, flag5, npczoneManager, npcruntimeSpawner);
	}

	// Token: 0x060000AF RID: 175 RVA: 0x00006470 File Offset: 0x00004670
	private string BuildReadinessDiagnostic(bool mapReady, bool npcReady, bool vehiclesReady, NPCZoneManager zoneMgr, NPCRuntimeSpawner spawner)
	{
		string text = ((ChunkManager.Instance != null) ? ChunkManager.Instance.GetInitialReadinessDiagnostic() : "ChunkManager=null");
		string text2 = string.Concat(new string[]
		{
			"zoneMgr=",
			(zoneMgr != null) ? "yes" : "no",
			", ",
			string.Format("population={0}, ", zoneMgr != null && zoneMgr.IsInitialPopulationComplete),
			string.Format("count={0}/", (zoneMgr != null) ? zoneMgr.CurrentNPCCount : (-1)),
			string.Format("{0}, ", (zoneMgr != null) ? zoneMgr.MaxNPCCount : (-1)),
			string.Format("pending={0}, ", (zoneMgr != null) ? zoneMgr.PendingSpawns : (-1)),
			string.Format("paused={0}, ", NPCSimulationPause.IsPaused),
			"spawner=",
			(spawner != null) ? "yes" : "no",
			", ",
			string.Format("prePooling={0}, ", spawner != null && spawner.IsPrePooling),
			string.Format("pool={0}, ", zoneMgr != null && zoneMgr.IsSpawnPoolReady),
			"poolFailure='",
			(zoneMgr != null) ? zoneMgr.SpawnPoolFailure : null,
			"', ",
			string.Format("AStar={0}, ", this.IsRequiredNavigationReady()),
			string.Format("AStarScanning={0}", AstarPath.active != null && AstarPath.active.isScanning)
		});
		string text3 = ((this.vehicleSpawnManager != null) ? string.Format("manager=yes, initialSpawn={0}", this.vehicleSpawnManager.IsInitialSpawnComplete) : "manager=no");
		return string.Concat(new string[]
		{
			string.Format("generation={0}, ", this._readinessGeneration),
			"scene='",
			SceneManager.GetActiveScene().name,
			"', ",
			string.Format("sceneBound={0}, player={1}, ", this._sceneSystemsInitialized && this.HasCurrentSceneBinding(), this.HasLivePlayerBinding()),
			string.Format("map={0}, npc={1}, vehicles={2}; ", mapReady, npcReady, vehiclesReady),
			text,
			"; ",
			text2,
			"; ",
			text3
		});
	}

	// Token: 0x060000B0 RID: 176 RVA: 0x0000674C File Offset: 0x0000494C
	private void CompleteGameplayReadiness(int generation, bool degraded, string diagnostic)
	{
		if (generation != this._readinessGeneration || GameSystemsBootstrapper.GameplayReady || GameSystemsBootstrapper.GameplayReadinessFailed || !this.AreRequiredSystemsReady())
		{
			return;
		}
		GameSystemsBootstrapper.GameplayReady = true;
		GameSystemsBootstrapper.GameplayReadyDegraded = degraded;
		GameSystemsBootstrapper.LastReadinessDiagnostic = diagnostic;
		this._systemsReadyRoutine = null;
		if (degraded)
		{
			Debug.LogError("[GameSystemsBootstrapper] GameplayReady = true in DEGRADED mode; required dependencies verified after stalled notification. " + diagnostic);
		}
		else if (this.enableDebugLogs)
		{
			Debug.Log("[GameSystemsBootstrapper] GameplayReady = true (mappa + pool NPC + primo popolamento + veicoli pronti).");
		}
		try
		{
			GraphicsSettingsController instance = GraphicsSettingsController.Instance;
			if (instance != null)
			{
				instance.ReapplyWorld();
			}
		}
		catch (Exception ex)
		{
			Debug.LogException(ex);
			Debug.LogError("[GameSystemsBootstrapper] World rendering refresh failed after readiness; continuing the scene release.");
		}
		GameSystemsBootstrapper.NotifyGameplayReadySafely();
	}

	// Token: 0x060000B1 RID: 177 RVA: 0x000067F4 File Offset: 0x000049F4
	private static void NotifyGameplayReadySafely()
	{
		Action onGameplayReady = GameSystemsBootstrapper.OnGameplayReady;
		if (onGameplayReady == null)
		{
			return;
		}
		foreach (Action action in onGameplayReady.GetInvocationList())
		{
			try
			{
				action();
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
				Debug.LogError("[GameSystemsBootstrapper] An OnGameplayReady subscriber failed; remaining subscribers and transition cleanup will continue.");
			}
		}
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x00006854 File Offset: 0x00004A54
	private NPCRuntimeSpawner GetReadinessSpawner()
	{
		if (this._readinessSpawner != null && this._readinessSpawner.gameObject.activeInHierarchy)
		{
			return this._readinessSpawner;
		}
		if (Application.isPlaying && this._readinessSpawnerSearchFrame == Time.frameCount)
		{
			return null;
		}
		this._readinessSpawnerSearchFrame = Time.frameCount;
		this._readinessSpawner = Object.FindObjectOfType<NPCRuntimeSpawner>();
		return this._readinessSpawner;
	}

	// Token: 0x060000B3 RID: 179 RVA: 0x000068BC File Offset: 0x00004ABC
	private float CalculateInitializationProgress()
	{
		float num = ((!this.enableProceduralMap) ? 1f : ((ChunkManager.Instance != null) ? ChunkManager.Instance.InitialLoadingProgress : 0f));
		float num2 = num;
		if (this.enableNPCs)
		{
			NPCZoneManager npczoneManager = ((this.npcZoneManager != null) ? this.npcZoneManager : Object.FindObjectOfType<NPCZoneManager>());
			NPCRuntimeSpawner npcruntimeSpawner = ((npczoneManager != null) ? this.GetReadinessSpawner() : null);
			float num3 = ((npczoneManager != null && npczoneManager.IsSpawnPoolReady) ? 1f : ((npcruntimeSpawner != null && npcruntimeSpawner.PrePoolTarget > 0) ? Mathf.Clamp01((float)npcruntimeSpawner.PrePooledCount / (float)npcruntimeSpawner.PrePoolTarget) : 0f));
			float num4 = (NPCSimulationPause.IsPaused ? 1f : ((npczoneManager != null) ? npczoneManager.InitialPopulationProgress : 0f));
			num2 = num * 0.7f + num3 * 0.15f + num4 * 0.15f;
		}
		if (!this.participatesInRun || !this.enableVehicles)
		{
			return Mathf.Clamp01(num2);
		}
		float num5 = ((this.vehicleSpawnManager != null && this.vehicleSpawnManager.IsInitialSpawnComplete) ? 1f : 0f);
		return Mathf.Clamp01(num2 * 0.85f + num5 * 0.15f);
	}

	// Token: 0x060000B4 RID: 180 RVA: 0x00006A18 File Offset: 0x00004C18
	private void ApplySceneProfile()
	{
		SceneContext sceneContext = SceneContext.Current;
		if (sceneContext == null)
		{
			this.enableProceduralMap = false;
			this.participatesInRun = false;
			this.enableNPCs = false;
			this.enableVehicles = false;
			this.enableReporters = false;
			this.enablePolice = false;
			this.enableDetection = false;
			this.enablePlayerEmotions = false;
			this.enableStreetLights = false;
			this.enableMinimap = false;
			if (this.enableDebugLogs)
			{
				Debug.Log("[GameSystemsBootstrapper] Nessun SceneContext (contesto non-gameplay): tutti i sistemi gated OFF.");
			}
			return;
		}
		this.staticChunksContainer = sceneContext.NpcSpawnContainer;
		if (this.staticChunksContainer != null && this.enableDebugLogs)
		{
			Debug.Log("[GameSystemsBootstrapper] staticChunksContainer impostato da SceneContext: '" + this.staticChunksContainer.name + "'.");
		}
		SceneProfileSO profile = sceneContext.Profile;
		if (profile == null)
		{
			if (this.enableDebugLogs)
			{
				Debug.Log("[GameSystemsBootstrapper] SceneContext '" + sceneContext.name + "' senza profilo: uso i toggle dell'Inspector.");
			}
			return;
		}
		this.enableProceduralMap = profile.proceduralMap;
		this.participatesInRun = profile.participatesInRun;
		this.enableNPCs = profile.npcs;
		this.enableVehicles = profile.vehicles;
		this.enableReporters = profile.reporters;
		this.enablePolice = profile.police;
		this.enableDetection = profile.detection;
		this.enablePlayerEmotions = profile.playerEmotions;
		this.enableStreetLights = profile.streetLights;
		this.enableMinimap = profile.minimap;
		NPCRuntimeSpawner npcruntimeSpawner = Object.FindObjectOfType<NPCRuntimeSpawner>(true);
		if (npcruntimeSpawner != null)
		{
			npcruntimeSpawner.SetNPCPrefixOverride(profile.npcPrefixOverride);
			if (this.enableDebugLogs && !string.IsNullOrEmpty(profile.npcPrefixOverride))
			{
				Debug.Log("[GameSystemsBootstrapper] NPC prefix override applicato: '" + profile.npcPrefixOverride + "'.");
			}
		}
		if (this.enableDebugLogs)
		{
			Debug.Log(string.Concat(new string[]
			{
				"[GameSystemsBootstrapper] Profilo '",
				profile.displayName,
				"' applicato ",
				string.Format("(map={0}, npc={1}, reporters={2}, ", this.enableProceduralMap, this.enableNPCs, this.enableReporters),
				string.Format("police={0}, detection={1}, emotions={2}, ", this.enablePolice, this.enableDetection, this.enablePlayerEmotions),
				string.Format("streetLights={0}, vehicles={1}, ", this.enableStreetLights, this.enableVehicles),
				string.Format("run={0}).", this.participatesInRun)
			}));
		}
	}

	// Token: 0x060000B5 RID: 181 RVA: 0x00006C8F File Offset: 0x00004E8F
	private void OnDestroy()
	{
		PlayerRegistry.PlayerChanged -= this.OnPlayerChanged;
		if (GameSystemsBootstrapper.Instance == this)
		{
			GameSystemsBootstrapper.InvalidateForSceneTransition();
			GameSystemsBootstrapper.Instance = null;
		}
	}

	// Token: 0x060000B6 RID: 182 RVA: 0x00006CBC File Offset: 0x00004EBC
	private void InitializeSystems(bool startsNewCityRun)
	{
		if (this.proximityProvider == null)
		{
			this.proximityProvider = this.FindOrCreate<PlayerProximityProvider>("PlayerProximityProvider");
		}
		this.proximityProvider.BindPlayer(this.player);
		this.proximityProvider.SetCellSize(this.cellSize);
		if (this.enableDebugLogs)
		{
			Debug.Log("[GameSystemsBootstrapper] PlayerProximityProvider initialized");
		}
		if (this.enableProceduralMap)
		{
			if (this.chunkManager == null)
			{
				this.chunkManager = this.FindOrCreate<ChunkManager>("ChunkManager");
			}
			if (startsNewCityRun)
			{
				this.chunkManager.ResetForNewRun();
			}
			if (this.chunkManager.IsPoiTravelSuspended)
			{
				this.chunkManager.ResumeAfterPoiTravel(this.staticChunksContainer);
			}
			if (this.enableDebugLogs)
			{
				Debug.Log("[GameSystemsBootstrapper] ChunkManager initialized");
			}
		}
		else
		{
			ChunkManager chunkManager = Object.FindObjectOfType<ChunkManager>();
			if (chunkManager != null)
			{
				chunkManager.enabled = false;
			}
			if (this.enableDebugLogs)
			{
				Debug.Log("[GameSystemsBootstrapper] ChunkManager DISABLED (testing mode)");
			}
		}
		if (this.graphManager == null)
		{
			this.graphManager = this.FindOrCreate<AStarGraphManager>("AStarGraphManager");
		}
		if (this.enableNPCs || this.enableReporters || this.enablePolice)
		{
			this.graphManager.ReinitializeForCurrentScene();
		}
		if (this.enableDebugLogs)
		{
			Debug.Log("[GameSystemsBootstrapper] AStarGraphManager initialized");
		}
		bool flag = this.participatesInRun && this.enableVehicles;
		if (this.vehicleSpawnManager == null)
		{
			this.vehicleSpawnManager = Object.FindObjectOfType<VehicleSpawnManager>(true);
		}
		if (flag)
		{
			if (this.vehicleSpawnManager == null)
			{
				this.vehicleSpawnManager = this.FindOrCreate<VehicleSpawnManager>("VehicleSpawnManager");
			}
			this.vehicleSpawnManager.enabled = true;
			this.vehicleSpawnManager.ReinitializeForCurrentScene(null, this.enableProceduralMap);
			if (this.enableDebugLogs)
			{
				Debug.Log("[GameSystemsBootstrapper] VehicleSpawnManager initialized " + string.Format("(chunkMode={0})", this.enableProceduralMap));
			}
		}
		else if (this.vehicleSpawnManager != null)
		{
			this.vehicleSpawnManager.ShutdownForCurrentScene();
			this.vehicleSpawnManager.enabled = false;
			if (this.enableDebugLogs)
			{
				Debug.Log("[GameSystemsBootstrapper] VehicleSpawnManager DISABLED by scene profile");
			}
		}
		if (this.enableNPCs)
		{
			if (this.npcZoneManager == null)
			{
				this.npcZoneManager = this.FindOrCreate<NPCZoneManager>("NPCZoneManager");
			}
			this.npcZoneManager.ReinitializeForPlayer(this.player);
			if (this.enableDebugLogs)
			{
				Debug.Log("[GameSystemsBootstrapper] NPCZoneManager initialized + ri-agganciato al player");
			}
		}
		else
		{
			NPCZoneManager npczoneManager = Object.FindObjectOfType<NPCZoneManager>();
			if (npczoneManager != null)
			{
				npczoneManager.enabled = false;
			}
			NPCRuntimeSpawner npcruntimeSpawner = Object.FindObjectOfType<NPCRuntimeSpawner>();
			if (npcruntimeSpawner != null)
			{
				npcruntimeSpawner.enabled = false;
			}
			if (this.enableDebugLogs)
			{
				Debug.Log("[GameSystemsBootstrapper] NPCZoneManager and NPCRuntimeSpawner DISABLED (testing mode)");
			}
		}
		if (this.enableNPCs && this.enableReporters)
		{
			if (Object.FindObjectOfType<ReporterTroupeManager>() == null)
			{
				Debug.LogWarning("[GameSystemsBootstrapper] enableReporters=true but no ReporterTroupeManager in scene — Reporters will not spawn.");
			}
			else
			{
				this.FindOrCreate<ReporterProceduralSpawner>("ReporterProceduralSpawner");
				if (this.enableDebugLogs)
				{
					Debug.Log("[GameSystemsBootstrapper] ReporterProceduralSpawner initialized");
				}
			}
		}
		else
		{
			ReporterProceduralSpawner reporterProceduralSpawner = Object.FindObjectOfType<ReporterProceduralSpawner>();
			if (reporterProceduralSpawner != null)
			{
				reporterProceduralSpawner.enabled = false;
			}
			if (this.enableDebugLogs)
			{
				Debug.Log("[GameSystemsBootstrapper] ReporterProceduralSpawner DISABLED");
			}
		}
		PoliceManager policeManager = Object.FindObjectOfType<PoliceManager>();
		if (policeManager != null)
		{
			policeManager.enabled = this.enablePolice;
			if (this.enableDebugLogs)
			{
				Debug.Log("[GameSystemsBootstrapper] PoliceManager " + (this.enablePolice ? "ENABLED" : "DISABLED"));
			}
		}
		else if (this.enablePolice && this.enableDebugLogs)
		{
			Debug.LogWarning("[GameSystemsBootstrapper] enablePolice=true but no PoliceManager in scene — police will not spawn.");
		}
		if (this.enableStreetLights)
		{
			if (this.streetLightManager == null)
			{
				this.streetLightManager = this.FindOrCreate<StreetLightManager>("StreetLightManager");
			}
			this.streetLightManager.Initialize(this.player, this.FindAzureSystem(), this.mainCamera);
			if (this.enableDebugLogs)
			{
				Debug.Log("[GameSystemsBootstrapper] StreetLightManager initialized");
			}
		}
		else
		{
			StreetLightManager streetLightManager = Object.FindObjectOfType<StreetLightManager>();
			if (streetLightManager != null)
			{
				streetLightManager.enabled = false;
			}
			if (this.enableDebugLogs)
			{
				Debug.Log("[GameSystemsBootstrapper] StreetLightManager DISABLED (testing mode)");
			}
		}
		FPSCounter fpscounter = Object.FindObjectOfType<FPSCounter>(true);
		if (fpscounter != null)
		{
			fpscounter.SetVisible(false);
			fpscounter.enabled = false;
		}
		if (this.enableDebugLogs)
		{
			Debug.Log("[GameSystemsBootstrapper] Legacy FPSCounter disabled; using PerformanceHudOverlay.");
		}
		if (this.enableDebugLogs)
		{
			Debug.Log("[GameSystemsBootstrapper] All systems initialized!");
		}
	}

	// Token: 0x060000B7 RID: 183 RVA: 0x000070F8 File Offset: 0x000052F8
	private T FindOrCreate<T>(string name) where T : Component
	{
		T t = Object.FindObjectOfType<T>();
		if (t != null)
		{
			return t;
		}
		GameObject gameObject = new GameObject(name);
		gameObject.transform.SetParent(base.transform);
		return gameObject.AddComponent<T>();
	}

	// Token: 0x060000B8 RID: 184 RVA: 0x00007137 File Offset: 0x00005337
	private AzureCoreSystem FindAzureSystem()
	{
		return Object.FindObjectOfType<AzureCoreSystem>();
	}

	// Token: 0x0400005E RID: 94
	private string _lastInitializedScene;

	// Token: 0x0400005F RID: 95
	private Coroutine _systemsReadyRoutine;

	// Token: 0x04000060 RID: 96
	private LoadingScreenController _playerRebindLoadingOwner;

	// Token: 0x04000061 RID: 97
	private int _readinessGeneration;

	// Token: 0x04000062 RID: 98
	private int _initializedSceneHandle = -1;

	// Token: 0x04000063 RID: 99
	private bool _sceneSystemsInitialized;

	// Token: 0x04000064 RID: 100
	private bool _startsNewCityRun;

	// Token: 0x04000065 RID: 101
	private NPCRuntimeSpawner _readinessSpawner;

	// Token: 0x04000066 RID: 102
	private int _readinessSpawnerSearchFrame = -1;

	// Token: 0x04000067 RID: 103
	[Header("Shared References")]
	[SerializeField]
	private Transform player;

	// Token: 0x04000068 RID: 104
	[Tooltip("Camera principale per calcoli FOV (third-person). Se null usa Camera.main.")]
	[SerializeField]
	private Camera mainCamera;

	// Token: 0x04000069 RID: 105
	[Tooltip("Container dei chunk statici pre-spawnati (es. StartingChunks).")]
	[SerializeField]
	private Transform staticChunksContainer;

	// Token: 0x0400006A RID: 106
	[Header("Grid Settings")]
	[SerializeField]
	private float cellSize = 60f;

	// Token: 0x0400006B RID: 107
	[Header("Systems - Auto Created if null")]
	[SerializeField]
	private PlayerProximityProvider proximityProvider;

	// Token: 0x0400006C RID: 108
	[SerializeField]
	private AStarGraphManager graphManager;

	// Token: 0x0400006D RID: 109
	[SerializeField]
	private ChunkManager chunkManager;

	// Token: 0x0400006E RID: 110
	[SerializeField]
	private NPCZoneManager npcZoneManager;

	// Token: 0x0400006F RID: 111
	[SerializeField]
	private StreetLightManager streetLightManager;

	// Token: 0x04000070 RID: 112
	[SerializeField]
	private VehicleSpawnManager vehicleSpawnManager;

	// Token: 0x04000071 RID: 113
	[Tooltip("Spawner del traffico ambientale. Il pool viene creato solo nei profili con veicoli attivi.")]
	[SerializeField]
	private VehicleSpawner ambientVehicleSpawner;

	// Token: 0x04000072 RID: 114
	[SerializeField]
	private FPSCounter fpsCounter;

	// Token: 0x04000073 RID: 115
	[Header("Feature Toggles")]
	[Tooltip("La scena attiva fa parte di una run vera: abilita HUD e tracker di punteggio/distanza.")]
	[SerializeField]
	private bool participatesInRun = true;

	// Token: 0x04000074 RID: 116
	[Tooltip("Disabilita per testare senza generazione procedurale mappa")]
	[SerializeField]
	private bool enableProceduralMap = true;

	// Token: 0x04000075 RID: 117
	[Tooltip("Disabilita per testare senza NPC")]
	[SerializeField]
	private bool enableNPCs = true;

	// Token: 0x04000076 RID: 118
	[Tooltip("Abilita i veicoli parcheggiati generati dai VehicleSpawnPoint della scena/chunk.")]
	[SerializeField]
	private bool enableVehicles;

	// Token: 0x04000077 RID: 119
	[Tooltip("Stack encounter FINALE (ExhibitionEventDirector, HUDManager, TimedEffects). OFF per hub/menu senza gameplay di esibizione (CielHome, MainMenu, Prison). NB: il SensoryManager NON dipende solo da questo flag — con NPC attivi resta acceso anche a detection OFF, in modalità solo pre-detection (DisableFinalDetection): gli NPC vedono/sentono e fanno reazioni passive ma il Director finale non parte.")]
	[SerializeField]
	private bool enableDetection = true;

	// Token: 0x04000078 RID: 120
	[Tooltip("EmotionManager del Player. Sistema del Player, indipendente dalla detection: può restare ON anche in hub/menu (es. CielHome).")]
	[SerializeField]
	private bool enablePlayerEmotions = true;

	// Token: 0x04000079 RID: 121
	[Tooltip("Spawn procedurale delle troupe Reporter nella mappa (probabilistico, max 1 alla volta).")]
	[SerializeField]
	private bool enableReporters = true;

	// Token: 0x0400007A RID: 122
	[Tooltip("Spawn dinamico di poliziotti a seguito della Suspicion (Lv1 = solo officers, Lv2 = patrols con Taser).")]
	[SerializeField]
	private bool enablePolice = true;

	// Token: 0x0400007B RID: 123
	[Tooltip("Disabilita per testare senza luci stradali dinamiche")]
	[SerializeField]
	private bool enableStreetLights = true;

	// Token: 0x0400007C RID: 124
	[Tooltip("Minimappa (Compass Navigator Pro). OFF nei POI con scena dedicata: il refresh della minimappa ri-renderizza la scena dall'alto (TopDownCamera) a ogni movimento del player.")]
	[SerializeField]
	private bool enableMinimap = true;

	// Token: 0x0400007D RID: 125
	[Tooltip("Abilita per mostrare FPS counter in game")]
	[SerializeField]
	private bool enableFPSCounter = true;

	// Token: 0x0400007E RID: 126
	[Header("Debug")]
	[SerializeField]
	private bool enableDebugLogs = true;

	// Token: 0x0400007F RID: 127
	[Tooltip("Seconds without completed preparation work before stalled loading offers recovery. A healthy load may take longer while it continues making progress.")]
	[SerializeField]
	[Min(5f)]
	private float readinessSafetyTimeout = 35f;

	// Token: 0x02000017 RID: 23
	public enum RuntimeFeature
	{
		// Token: 0x04000081 RID: 129
		ProceduralMap,
		// Token: 0x04000082 RID: 130
		Vehicles,
		// Token: 0x04000083 RID: 131
		NPCs,
		// Token: 0x04000084 RID: 132
		Reporters,
		// Token: 0x04000085 RID: 133
		Police,
		// Token: 0x04000086 RID: 134
		Detection,
		// Token: 0x04000087 RID: 135
		PlayerEmotions,
		// Token: 0x04000088 RID: 136
		StreetLights,
		// Token: 0x04000089 RID: 137
		FPSCounter
	}
}
