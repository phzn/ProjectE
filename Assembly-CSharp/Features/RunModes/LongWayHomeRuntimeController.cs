using System;
using System.Collections;
using System.Diagnostics;
using ExhibitionistDemo.UI;
using Features.Achievements;
using Features.Detection;
using Features.GameOver;
using Features.Inventory;
using Features.Loading;
using Features.MainMenu;
using Features.MatchRecap;
using Features.NPC;
using Features.PhoneCompanion;
using Features.Police;
using Features.Rank;
using Features.RenderingPrecision;
using Features.Suspicion;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using MagicaCloth2;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.RunModes
{
	// Token: 0x02000CF5 RID: 3317
	[DefaultExecutionOrder(-800)]
	public sealed class LongWayHomeRuntimeController : MonoBehaviour
	{
		// Token: 0x17000F98 RID: 3992
		// (get) Token: 0x06006058 RID: 24664 RVA: 0x0026390D File Offset: 0x00261B0D
		// (set) Token: 0x06006059 RID: 24665 RVA: 0x00263914 File Offset: 0x00261B14
		public static LongWayHomeRuntimeController Instance { get; private set; }

		// Token: 0x17000F99 RID: 3993
		// (get) Token: 0x0600605A RID: 24666 RVA: 0x0026391C File Offset: 0x00261B1C
		public static bool IsPreparing
		{
			get
			{
				return RunModeService.IsLongWayHome && LongWayHomeRuntimeController.Instance != null && LongWayHomeRuntimeController.Instance.Phase == LongWayHomeRuntimeController.RunPhase.Preparing;
			}
		}

		// Token: 0x17000F9A RID: 3994
		// (get) Token: 0x0600605B RID: 24667 RVA: 0x00263941 File Offset: 0x00261B41
		public static bool IsRunActive
		{
			get
			{
				return RunModeService.IsLongWayHome && LongWayHomeRuntimeController.Instance != null && LongWayHomeRuntimeController.Instance.Phase == LongWayHomeRuntimeController.RunPhase.Running;
			}
		}

		// Token: 0x17000F9B RID: 3995
		// (get) Token: 0x0600605C RID: 24668 RVA: 0x00263966 File Offset: 0x00261B66
		public static bool IsReturningHome
		{
			get
			{
				return LongWayHomeRuntimeController.Instance != null && (LongWayHomeRuntimeController.Instance.Phase == LongWayHomeRuntimeController.RunPhase.Won || LongWayHomeRuntimeController.Instance.Phase == LongWayHomeRuntimeController.RunPhase.Failed);
			}
		}

		// Token: 0x0600605D RID: 24669 RVA: 0x00263993 File Offset: 0x00261B93
		public static string ProloguePoiScene(LongWayHomePrologue prologue)
		{
			if (prologue == LongWayHomePrologue.GymShower)
			{
				return "POI - Mall";
			}
			if (prologue == LongWayHomePrologue.University)
			{
				return "POI - University";
			}
			if (prologue != LongWayHomePrologue.Stadium)
			{
				return null;
			}
			return "POI - Stadium";
		}

		// Token: 0x0600605E RID: 24670 RVA: 0x002639B4 File Offset: 0x00261BB4
		public static ProceduralPoiKind ProloguePoiKind(LongWayHomePrologue prologue)
		{
			if (prologue == LongWayHomePrologue.GymShower)
			{
				return ProceduralPoiKind.Mall;
			}
			if (prologue == LongWayHomePrologue.University)
			{
				return ProceduralPoiKind.University;
			}
			if (prologue != LongWayHomePrologue.Stadium)
			{
				return ProceduralPoiKind.None;
			}
			return ProceduralPoiKind.Stadium;
		}

		// Token: 0x0600605F RID: 24671 RVA: 0x002639CC File Offset: 0x00261BCC
		public static bool IsAwaitingProloguePoi(string sceneName)
		{
			return RunModeService.IsLongWayHomePoiPrologue && sceneName == LongWayHomeRuntimeController.ProloguePoiScene(RunModeService.CurrentLongWayHomePrologue) && LongWayHomeRuntimeController.Instance != null && !LongWayHomeRuntimeController.Instance.PoiPrologueCompleted && (LongWayHomeRuntimeController.Instance.Phase == LongWayHomeRuntimeController.RunPhase.Idle || LongWayHomeRuntimeController.Instance.AwaitingPoiPrologue);
		}

		// Token: 0x17000F9C RID: 3996
		// (get) Token: 0x06006060 RID: 24672 RVA: 0x00263A25 File Offset: 0x00261C25
		public static bool StartsCityAfterPoiPrologue
		{
			get
			{
				return RunModeService.IsLongWayHome && LongWayHomeRuntimeController.Instance != null && LongWayHomeRuntimeController.Instance._poiCityPending;
			}
		}

		// Token: 0x06006061 RID: 24673 RVA: 0x00263A48 File Offset: 0x00261C48
		public static bool TryGetPlannedCitySeed(out int seed)
		{
			bool flag = RunModeService.IsLongWayHome && LongWayHomeRuntimeController.Instance != null && LongWayHomeRuntimeController.Instance._hasPlannedCitySeed;
			seed = (flag ? LongWayHomeRuntimeController.Instance._plannedCitySeed : 0);
			return flag;
		}

		// Token: 0x17000F9D RID: 3997
		// (get) Token: 0x06006062 RID: 24674 RVA: 0x00263A8A File Offset: 0x00261C8A
		// (set) Token: 0x06006063 RID: 24675 RVA: 0x00263A92 File Offset: 0x00261C92
		public LongWayHomeRuntimeController.RunPhase Phase { get; private set; }

		// Token: 0x17000F9E RID: 3998
		// (get) Token: 0x06006064 RID: 24676 RVA: 0x00263A9B File Offset: 0x00261C9B
		// (set) Token: 0x06006065 RID: 24677 RVA: 0x00263AA3 File Offset: 0x00261CA3
		public bool AwaitingPoiPrologue { get; private set; }

		// Token: 0x17000F9F RID: 3999
		// (get) Token: 0x06006066 RID: 24678 RVA: 0x00263AAC File Offset: 0x00261CAC
		// (set) Token: 0x06006067 RID: 24679 RVA: 0x00263AB4 File Offset: 0x00261CB4
		public bool PoiPrologueCompleted { get; private set; }

		// Token: 0x17000FA0 RID: 4000
		// (get) Token: 0x06006068 RID: 24680 RVA: 0x00263ABD File Offset: 0x00261CBD
		public bool HasRoute
		{
			get
			{
				return this._route != null && this._route.IsReady;
			}
		}

		// Token: 0x17000FA1 RID: 4001
		// (get) Token: 0x06006069 RID: 24681 RVA: 0x00263ADA File Offset: 0x00261CDA
		// (set) Token: 0x0600606A RID: 24682 RVA: 0x00263AE2 File Offset: 0x00261CE2
		public bool IsOpeningPrepared { get; private set; }

		// Token: 0x17000FA2 RID: 4002
		// (get) Token: 0x0600606B RID: 24683 RVA: 0x00263AEB File Offset: 0x00261CEB
		// (set) Token: 0x0600606C RID: 24684 RVA: 0x00263AF3 File Offset: 0x00261CF3
		public float StartingDistance { get; private set; }

		// Token: 0x17000FA3 RID: 4003
		// (get) Token: 0x0600606D RID: 24685 RVA: 0x00263AFC File Offset: 0x00261CFC
		public Vector3 HomePosition
		{
			get
			{
				if (!this.HasRoute)
				{
					return Vector3.zero;
				}
				return this._route.HomePosition;
			}
		}

		// Token: 0x17000FA4 RID: 4004
		// (get) Token: 0x0600606E RID: 24686 RVA: 0x00263B17 File Offset: 0x00261D17
		// (set) Token: 0x0600606F RID: 24687 RVA: 0x00263B1F File Offset: 0x00261D1F
		public float ElapsedSeconds { get; private set; }

		// Token: 0x17000FA5 RID: 4005
		// (get) Token: 0x06006070 RID: 24688 RVA: 0x00263B28 File Offset: 0x00261D28
		public float PreparationProgress
		{
			get
			{
				if (!(this._route != null))
				{
					return 0f;
				}
				return this._route.PreparationProgress;
			}
		}

		// Token: 0x06006071 RID: 24689 RVA: 0x00263B49 File Offset: 0x00261D49
		public void SkipNextCityWakeUp()
		{
			this._skipCityWakeUp = true;
		}

		// Token: 0x17000FA6 RID: 4006
		// (get) Token: 0x06006072 RID: 24690 RVA: 0x00263B52 File Offset: 0x00261D52
		public static bool IsPoliceCapturePending
		{
			get
			{
				return RunModeService.IsLongWayHome && LongWayHomeRuntimeController.Instance != null && LongWayHomeRuntimeController.Instance.Phase == LongWayHomeRuntimeController.RunPhase.Lost && LongWayHomeRuntimeController.Instance._policeCaptureWitness != null;
			}
		}

		// Token: 0x06006073 RID: 24691 RVA: 0x00263B87 File Offset: 0x00261D87
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			LongWayHomeRuntimeController.Instance = null;
		}

		// Token: 0x06006074 RID: 24692 RVA: 0x00263B8F File Offset: 0x00261D8F
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void Install()
		{
			if (LongWayHomeRuntimeController.Instance != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("[LongWayHome]");
			Object.DontDestroyOnLoad(gameObject);
			gameObject.AddComponent<LongWayHomeRuntimeController>();
		}

		// Token: 0x06006075 RID: 24693 RVA: 0x00263BB8 File Offset: 0x00261DB8
		private void Awake()
		{
			if (LongWayHomeRuntimeController.Instance != null && LongWayHomeRuntimeController.Instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			LongWayHomeRuntimeController.Instance = this;
			SceneManager.sceneLoaded += this.OnSceneLoaded;
			SceneManager.activeSceneChanged += this.OnActiveSceneChanged;
			this.TryStart(SceneManager.GetActiveScene());
		}

		// Token: 0x06006076 RID: 24694 RVA: 0x00263C1E File Offset: 0x00261E1E
		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			if (this.Phase == LongWayHomeRuntimeController.RunPhase.Idle && LongWayHomeRuntimeController.IsAwaitingProloguePoi(scene.name))
			{
				this.BeginPoiPrologue(scene);
				return;
			}
			this.TryStart(scene);
		}

		// Token: 0x06006077 RID: 24695 RVA: 0x00263C48 File Offset: 0x00261E48
		private void OnActiveSceneChanged(Scene previous, Scene current)
		{
			if (current.name == "CielHome" || current.name == "MainMenu" || current.name == "GameOver - Prison" || current.name == "LongWayHome_RestaurantIntro")
			{
				this.ResetRun();
				return;
			}
			this.TryStart(current);
		}

		// Token: 0x06006078 RID: 24696 RVA: 0x00263CB0 File Offset: 0x00261EB0
		private void TryStart(Scene scene)
		{
			if (!RunModeService.IsLongWayHome || scene.name != "Game - Current")
			{
				return;
			}
			bool flag = this.Phase == LongWayHomeRuntimeController.RunPhase.Running && this._poiCityPending;
			if (this.Phase != LongWayHomeRuntimeController.RunPhase.Idle && !flag)
			{
				return;
			}
			this.Phase = LongWayHomeRuntimeController.RunPhase.Preparing;
			this.IsOpeningPrepared = false;
			ScreenFadeController instance = ScreenFadeController.Instance;
			if (instance != null)
			{
				instance.SetBlack();
			}
			this._opening = base.StartCoroutine(this.PrepareAndWake());
		}

		// Token: 0x06006079 RID: 24697 RVA: 0x00263D28 File Offset: 0x00261F28
		private void BeginPoiPrologue(Scene scene)
		{
			this.Phase = LongWayHomeRuntimeController.RunPhase.Preparing;
			this.AwaitingPoiPrologue = true;
			this._poiPrologue = RunModeService.CurrentLongWayHomePrologue;
			this.IsOpeningPrepared = true;
			this._player = PlayerRegistry.Player;
			this._input = ((this._player != null) ? this._player.GetComponent<vThirdPersonInput>() : null);
			if (this._npcPause == null)
			{
				this._npcPause = NPCSimulationPause.AcquirePause();
			}
			SensoryManager instance = SensoryManager.Instance;
			if (instance != null)
			{
				instance.PushPause("LongWayHome");
			}
			MatchStatsService instance2 = MatchStatsService.Instance;
			if (instance2 != null)
			{
				instance2.SetRunTrackingActive(false);
			}
			ScreenFadeController instance3 = ScreenFadeController.Instance;
			if (instance3 != null)
			{
				instance3.SetBlack();
			}
			this._plannedCitySeed = Random.Range(int.MinValue, int.MaxValue);
			this._hasPlannedCitySeed = true;
			this.StartingDistance = LongWayHomeWorldRoute.SelectStartingDistance(this._plannedCitySeed);
			LongWayHomeLootPolicy.BeginRun();
			LongWayHomeLootPolicy.CacheBodyPaintLoot(Resources.FindObjectsOfTypeAll<InventoryItemSO>());
			if (this._poiPrologue == LongWayHomePrologue.University)
			{
				LongWayHomeUniversitySequence.Install(scene);
			}
			else if (this._poiPrologue == LongWayHomePrologue.Stadium)
			{
				LongWayHomeStadiumSequence.Install(scene);
			}
			else
			{
				LongWayHomeGymShowerSequence.Install(scene);
			}
			Debug.Log(string.Format("[LongWayHome] {0} opening: the run starts in {1}.", this._poiPrologue, scene.name));
		}

		// Token: 0x17000FA7 RID: 4007
		// (get) Token: 0x0600607A RID: 24698 RVA: 0x00263E54 File Offset: 0x00262054
		private string PoiDisplayName
		{
			get
			{
				if (this._poiPrologue == LongWayHomePrologue.University)
				{
					return "University";
				}
				if (this._poiPrologue != LongWayHomePrologue.Stadium)
				{
					return "Mall";
				}
				return "Stadium";
			}
		}

		// Token: 0x0600607B RID: 24699 RVA: 0x00263E79 File Offset: 0x00262079
		private IEnumerator PrepareAndWake()
		{
			Stopwatch preparationClock = Stopwatch.StartNew();
			float deadline = Time.realtimeSinceStartup + 120f;
			int num;
			Vector3 vector;
			while (!SceneBootLoader.CoreReady || SceneManager.GetActiveScene().name != "Game - Current" || GameSystemsBootstrapper.Instance == null || GameSystemsBootstrapper.Instance.InitializedSceneName != "Game - Current" || ChunkManager.Instance == null || !ChunkManager.Instance.isActiveAndEnabled || ChunkManager.Instance.IsPoiTravelSuspended || GameSystemsBootstrapper.Instance.Player == null || GameSystemsBootstrapper.Instance.Player != PlayerRegistry.Player || !ChunkManager.Instance.TryGetSessionSeed(out num) || !LongWayHomeWorldRoute.TryResolveHomeEntrance(ChunkManager.Instance, out vector))
			{
				if (Time.realtimeSinceStartup >= deadline)
				{
					yield return this.AbortOpening("The city or player did not finish loading.");
					yield break;
				}
				yield return null;
			}
			this._player = GameSystemsBootstrapper.Instance.Player;
			long bootstrapMs = preparationClock.ElapsedMilliseconds;
			this._input = this._player.GetComponent<vThirdPersonInput>();
			this._npcPause = NPCSimulationPause.AcquirePause();
			this.HoldPreparation();
			int num2;
			ChunkManager.Instance.TryGetSessionSeed(out num2);
			this.StartingDistance = LongWayHomeWorldRoute.SelectStartingDistance(num2);
			this._route = base.gameObject.AddComponent<LongWayHomeWorldRoute>();
			yield return this._route.Prepare(this._player, this.StartingDistance, delegate(Vector3 position)
			{
				this.TeleportPlayer(position, this._player.rotation);
			}, this._poiCityPending ? LongWayHomeRuntimeController.ProloguePoiKind(this._poiPrologue) : ProceduralPoiKind.None);
			long routeMs = preparationClock.ElapsedMilliseconds;
			if (!this._route.IsReady)
			{
				yield return this.AbortOpening(this._route.Error);
				yield break;
			}
			this.TeleportPlayer(this._route.SpawnPosition, this._route.SpawnRotation);
			while (!WorldOrigin.TryCenterAround(this._player.position, true))
			{
				yield return null;
			}
			long recenterMs = preparationClock.ElapsedMilliseconds;
			deadline = Time.realtimeSinceStartup + 120f;
			while (!GameSystemsBootstrapper.IsRunReady(false) || ChunkManager.Instance == null || !ChunkManager.Instance.IsMapReady || !RankRuntimeController.InitialPresentationReady)
			{
				if (Time.realtimeSinceStartup >= deadline)
				{
					yield return this.AbortOpening("The destination or player presentation did not finish loading.");
					yield break;
				}
				yield return null;
			}
			DistanceTracker distance = this._player.GetComponent<DistanceTracker>();
			long readinessMs = preparationClock.ElapsedMilliseconds;
			if (distance != null)
			{
				distance.SetHomeDestination(this.HomePosition);
			}
			bool poiPrologue = this._poiCityPending;
			bool skipCityWakeUp = this._skipCityWakeUp;
			this._skipCityWakeUp = false;
			LongWayHomeIntroSequence intro = null;
			if (!poiPrologue && !skipCityWakeUp)
			{
				intro = base.gameObject.AddComponent<LongWayHomeIntroSequence>();
				intro.Prepare(this._player);
			}
			long introMs = preparationClock.ElapsedMilliseconds;
			if (intro != null && !intro.IsPrepared)
			{
				yield return this.AbortOpening("The wake-up animation could not be prepared.");
				yield break;
			}
			if (!poiPrologue)
			{
				this._startingLoot = base.gameObject.AddComponent<LongWayHomeStartingLoot>();
				yield return this._startingLoot.Prepare(this._player, this._route.SpawnPosition, this._route.SpawnRotation);
			}
			Debug.Log(string.Format("[LongWayHome] Opening preparation: bootstrap {0}ms, ", bootstrapMs) + string.Format("route {0}ms, recenter {1}ms, ", routeMs - bootstrapMs, recenterMs - routeMs) + string.Format("shared readiness {0}ms, intro {1}ms, ", readinessMs - recenterMs, introMs - readinessMs) + string.Format("loot {0}ms.", preparationClock.ElapsedMilliseconds - introMs));
			if (this._startingLoot != null && !this._startingLoot.IsReady)
			{
				yield return this.AbortOpening(this._startingLoot.Error);
				yield break;
			}
			this.IsOpeningPrepared = true;
			deadline = Time.realtimeSinceStartup + 120f;
			while ((SceneTransitionManager.Instance != null && SceneTransitionManager.Instance.IsTransitioning) || (LoadingScreenController.ExistingInstance != null && LoadingScreenController.ExistingInstance.IsLoadingActive))
			{
				if (Time.realtimeSinceStartup >= deadline)
				{
					yield return this.AbortOpening("The loading screen did not finish its handoff.");
					yield break;
				}
				yield return null;
			}
			if (poiPrologue)
			{
				this.ContinuePoiRunInCity(distance);
				yield break;
			}
			this.ReleaseBodyHold();
			this._introPlaying = true;
			PlayerActivityGate.Pop("LongWayHome");
			vThirdPersonInput input = this._input;
			if (input != null)
			{
				input.SetLockAllInput(false);
			}
			if (intro != null)
			{
				yield return intro.PlayPrepared();
				if (!intro.CompletedSuccessfully)
				{
					yield return this.AbortOpening("The wake-up animation could not be started.");
					yield break;
				}
				Object.Destroy(intro);
			}
			else
			{
				vThirdPersonInput input2 = this._input;
				if (input2 != null)
				{
					input2.SetLockCameraInput(false);
				}
				if (this._input != null && this._input.tpCamera != null && this._input.tpCamera.isInit)
				{
					this._input.tpCamera.ResetAngleWithoutSmooth();
					this._input.tpCamera.CameraMovement(true);
				}
				ScreenFadeController instance = ScreenFadeController.Instance;
				if (instance != null)
				{
					instance.FadeIn(0.35f, null);
				}
			}
			this._introPlaying = false;
			if (distance != null)
			{
				distance.ResetForNewRun();
				distance.SetHomeDestination(this.HomePosition);
			}
			MatchStatsService instance2 = MatchStatsService.Instance;
			if (instance2 != null)
			{
				instance2.SetMockedOverride(false);
				instance2.ResetForNewRun();
			}
			this.ElapsedSeconds = 0f;
			this.Phase = LongWayHomeRuntimeController.RunPhase.Running;
			if (instance2 != null)
			{
				instance2.SetRunTrackingActive(true);
			}
			this.ReleaseHold();
			LongWayHomeBanner.Show(base.transform, "RETURN TO HOME", "Use Maps to reach home. Encounters mean maximum wanted. Avoid police capture.");
			Debug.Log(string.Format("[LongWayHome] Run started. Spawn={0}, home={1}.", this._player.position, this.HomePosition));
			this._opening = null;
			yield break;
		}

		// Token: 0x0600607C RID: 24700 RVA: 0x00263E88 File Offset: 0x00262088
		private void ContinuePoiRunInCity(DistanceTracker distance)
		{
			this._poiCityPending = false;
			this._hasPlannedCitySeed = false;
			if (distance != null)
			{
				distance.ResetForNewRun();
				distance.SetHomeDestination(this.HomePosition);
			}
			this.Phase = LongWayHomeRuntimeController.RunPhase.Running;
			this.ReleaseHold();
			vThirdPersonInput input = this._input;
			if (input != null)
			{
				input.SetLockAllInput(false);
			}
			vThirdPersonInput input2 = this._input;
			if (input2 != null)
			{
				input2.SetLockCameraInput(false);
			}
			MatchStatsService instance = MatchStatsService.Instance;
			if (instance != null)
			{
				instance.SetRunTrackingActive(true);
			}
			ScreenFadeController instance2 = ScreenFadeController.Instance;
			if (instance2 != null)
			{
				instance2.FadeIn(0.6f, null);
			}
			LongWayHomeBanner.Show(base.transform, "RETURN TO HOME", "Use Maps to reach home. Encounters mean maximum wanted. Avoid police capture.");
			Debug.Log(string.Format("[LongWayHome] {0} run continues in the city. Spawn={1}, home={2}.", this._poiPrologue, this._player.position, this.HomePosition));
			this._opening = null;
		}

		// Token: 0x0600607D RID: 24701 RVA: 0x00263F67 File Offset: 0x00262167
		public bool BeginRunFromGymShowerPrologue()
		{
			return this.BeginRunFromPoiPrologue();
		}

		// Token: 0x0600607E RID: 24702 RVA: 0x00263F70 File Offset: 0x00262170
		public bool BeginRunFromPoiPrologue()
		{
			if (!this.AwaitingPoiPrologue || this.Phase != LongWayHomeRuntimeController.RunPhase.Preparing || this._player == null)
			{
				return false;
			}
			this.AwaitingPoiPrologue = false;
			this.PoiPrologueCompleted = true;
			this._poiCityPending = true;
			DistanceTracker component = this._player.GetComponent<DistanceTracker>();
			if (component != null)
			{
				component.ResetForNewRun();
				component.SetHeldHomeDistance(this.StartingDistance);
			}
			MatchStatsService instance = MatchStatsService.Instance;
			if (instance != null)
			{
				instance.SetMockedOverride(false);
				instance.ResetForNewRun();
			}
			this.ElapsedSeconds = 0f;
			this.Phase = LongWayHomeRuntimeController.RunPhase.Running;
			if (instance != null)
			{
				instance.SetRunTrackingActive(true);
			}
			this.ReleaseHold();
			vThirdPersonInput input = this._input;
			if (input != null)
			{
				input.SetLockAllInput(false);
			}
			vThirdPersonInput input2 = this._input;
			if (input2 != null)
			{
				input2.SetLockCameraInput(false);
			}
			LongWayHomeBanner.Show(base.transform, "RETURN TO HOME", (this._poiPrologue == LongWayHomePrologue.University) ? "Get out of the University, then use Maps to reach home. Avoid police capture." : ((this._poiPrologue == LongWayHomePrologue.Stadium) ? "Get out of the Stadium, then use Maps to reach home. Avoid police capture." : "Get out of the Mall, then use Maps to reach home. Avoid police capture."));
			Debug.Log("[LongWayHome] Run started in the " + this.PoiDisplayName + ".");
			return true;
		}

		// Token: 0x0600607F RID: 24703 RVA: 0x00264092 File Offset: 0x00262292
		public void FailGymShowerPrologue(string error)
		{
			this.FailPoiPrologue(error);
		}

		// Token: 0x06006080 RID: 24704 RVA: 0x0026409C File Offset: 0x0026229C
		public void FailPoiPrologue(string error)
		{
			if (!this.AwaitingPoiPrologue || this.Phase != LongWayHomeRuntimeController.RunPhase.Preparing)
			{
				return;
			}
			this.AwaitingPoiPrologue = false;
			if (this._opening != null)
			{
				base.StopCoroutine(this._opening);
			}
			this._opening = base.StartCoroutine(this.AbortOpening(error));
		}

		// Token: 0x06006081 RID: 24705 RVA: 0x002640EC File Offset: 0x002622EC
		private void Update()
		{
			if (LongWayHomeRuntimeController.IsPreparing && !this._introPlaying && !this.AwaitingPoiPrologue)
			{
				this.HoldPreparation();
			}
			if (RunModeService.IsLongWayHome && this.Phase == LongWayHomeRuntimeController.RunPhase.Encounter && !DetectionRecoveryGate.MustWaitForPlayer && this._input != null && ExhibitionEventDirector.Instance != null && !ExhibitionEventDirector.Instance.IsEventActive)
			{
				this.HoldPendingEncounterInput();
			}
			if (!LongWayHomeRuntimeController.IsRunActive)
			{
				return;
			}
			if (GameOverManager.Instance != null && GameOverManager.Instance.IsGameOver)
			{
				this.Phase = LongWayHomeRuntimeController.RunPhase.Lost;
				return;
			}
			if (Time.timeScale <= 0f || DetectionTransitionGate.IsBlocked)
			{
				return;
			}
			this.ElapsedSeconds += Time.deltaTime;
		}

		// Token: 0x06006082 RID: 24706 RVA: 0x002641AC File Offset: 0x002623AC
		private void LateUpdate()
		{
			if (this.Phase == LongWayHomeRuntimeController.RunPhase.Won)
			{
				this.StopTerminalMovement();
				return;
			}
			if (!LongWayHomeRuntimeController.IsRunActive || !this.HasRoute || this._player == null || Time.timeScale <= 0f || SceneManager.GetActiveScene().name != "Game - Current" || DetectionTransitionGate.IsBlocked || (GameOverManager.Instance != null && GameOverManager.Instance.IsGameOver))
			{
				return;
			}
			if (this._route.IsAtHome(this._player.position))
			{
				base.StartCoroutine(this.WinAndReturnHome());
			}
		}

		// Token: 0x06006083 RID: 24707 RVA: 0x00264251 File Offset: 0x00262451
		public static bool TryHandleFullDetection(NPCSenses witness)
		{
			if (!LongWayHomeRuntimeController.CanStartEncounter(witness) || ExhibitionEventDirector.Instance == null)
			{
				return false;
			}
			ExhibitionEventDirector.Instance.TriggerDetection(witness);
			return LongWayHomeRuntimeController.Instance.Phase == LongWayHomeRuntimeController.RunPhase.Encounter;
		}

		// Token: 0x06006084 RID: 24708 RVA: 0x00264284 File Offset: 0x00262484
		public static bool CanStartEncounter(NPCSenses witness)
		{
			if (!RunModeService.IsLongWayHome || LongWayHomeRuntimeController.Instance == null || witness == null || DetectionTransitionGate.IsBlocked || PlayerStealthBridge.IsFullyHidden || witness.BlocksFinalDetectionWhileSeated || LongWayHomeRuntimeController.IsPoliceWitness(witness))
			{
				return false;
			}
			if (LongWayHomeRuntimeController.Instance.Phase == LongWayHomeRuntimeController.RunPhase.Encounter)
			{
				return LongWayHomeRuntimeController.Instance._encounterWitness == witness;
			}
			return LongWayHomeRuntimeController.IsRunActive && witness.CurrentAttention >= 1f && witness.HasLineOfSight;
		}

		// Token: 0x06006085 RID: 24709 RVA: 0x00264308 File Offset: 0x00262508
		public static void MarkEncounterStarted(NPCSenses witness)
		{
			if (!LongWayHomeRuntimeController.CanStartEncounter(witness))
			{
				return;
			}
			LongWayHomeRuntimeController.Instance._encounterWitness = witness;
			LongWayHomeRuntimeController.Instance.Phase = LongWayHomeRuntimeController.RunPhase.Encounter;
			PlayerActivityGate.Push("LongWayHome.Encounter", PlayerActivity.AllPlayerActions);
			if (!DetectionRecoveryGate.MustWaitForPlayer)
			{
				LongWayHomeRuntimeController.Instance.HoldPendingEncounterInput();
			}
		}

		// Token: 0x06006086 RID: 24710 RVA: 0x00264354 File Offset: 0x00262554
		private void HoldPendingEncounterInput()
		{
			if (this._input == null || this._pendingInputHeld)
			{
				return;
			}
			vSwimming componentInChildren = this._input.GetComponentInChildren<vSwimming>(true);
			if (componentInChildren != null && (componentInChildren.inTheWater || componentInChildren.isSwimming || componentInChildren.isUnderWater))
			{
				return;
			}
			this._previousInputLock = this._input.lockInput;
			this._pendingInputHeld = true;
			this._input.SetLockAllInput(true);
		}

		// Token: 0x06006087 RID: 24711 RVA: 0x002643CB File Offset: 0x002625CB
		private void ReleaseEncounterHold()
		{
			PlayerActivityGate.Pop("LongWayHome.Encounter");
			if (this._pendingInputHeld && this._input != null)
			{
				this._input.SetLockAllInput(this._previousInputLock);
			}
			this._pendingInputHeld = false;
		}

		// Token: 0x06006088 RID: 24712 RVA: 0x00264408 File Offset: 0x00262608
		public static void CancelPendingEncounter(NPCSenses witness)
		{
			if (!RunModeService.IsLongWayHome || LongWayHomeRuntimeController.Instance == null || LongWayHomeRuntimeController.Instance.Phase != LongWayHomeRuntimeController.RunPhase.Encounter || LongWayHomeRuntimeController.Instance._encounterWitness != witness)
			{
				return;
			}
			LongWayHomeRuntimeController.Instance.ReleaseEncounterHold();
			LongWayHomeRuntimeController.Instance._encounterWitness = null;
			LongWayHomeRuntimeController.Instance.Phase = LongWayHomeRuntimeController.RunPhase.Running;
		}

		// Token: 0x06006089 RID: 24713 RVA: 0x00264464 File Offset: 0x00262664
		public static bool IsPoliceWitness(NPCSenses witness)
		{
			return witness != null && (SensoryManager.ResolveActorType(witness) == DetectionActorType.Police || witness.GetComponentInParent<PoliceOfficerChaser>() != null || witness.GetComponentInParent<TaserOfficer>() != null);
		}

		// Token: 0x0600608A RID: 24714 RVA: 0x00264498 File Offset: 0x00262698
		public static bool CompleteEncounter()
		{
			if (!RunModeService.IsLongWayHome || LongWayHomeRuntimeController.Instance == null || LongWayHomeRuntimeController.Instance.Phase != LongWayHomeRuntimeController.RunPhase.Encounter)
			{
				return false;
			}
			SuspicionManager instance = SuspicionManager.Instance;
			if (instance != null)
			{
				instance.MaximizeSuspicion();
			}
			LongWayHomeRuntimeController.Instance.ReleaseEncounterHold();
			LongWayHomeRuntimeController.Instance._encounterWitness = null;
			LongWayHomeRuntimeController.Instance.Phase = LongWayHomeRuntimeController.RunPhase.Running;
			return true;
		}

		// Token: 0x0600608B RID: 24715 RVA: 0x002644FC File Offset: 0x002626FC
		public static bool TryBeginPoliceCapture(NPCSenses witness)
		{
			if (witness == null || DetectionTransitionGate.IsBlocked || PlayerStealthBridge.IsFullyHidden)
			{
				return false;
			}
			if (LongWayHomeRuntimeController.IsPoliceCapturePending)
			{
				return LongWayHomeRuntimeController.Instance._policeCaptureWitness == witness;
			}
			if (!LongWayHomeRuntimeController.TryBeginPoliceDefeat())
			{
				return false;
			}
			LongWayHomeRuntimeController.Instance._policeCaptureWitness = witness;
			return true;
		}

		// Token: 0x0600608C RID: 24716 RVA: 0x00264550 File Offset: 0x00262750
		public static bool TryBeginPoliceDefeat()
		{
			if (!RunModeService.IsLongWayHome || LongWayHomeRuntimeController.Instance == null || (LongWayHomeRuntimeController.Instance.Phase != LongWayHomeRuntimeController.RunPhase.Running && LongWayHomeRuntimeController.Instance.Phase != LongWayHomeRuntimeController.RunPhase.Encounter) || DetectionTransitionGate.IsBlocked || PlayerStealthBridge.IsFullyHidden)
			{
				return false;
			}
			LongWayHomeRuntimeController.Instance.Phase = LongWayHomeRuntimeController.RunPhase.Lost;
			ExhibitionEventDirector instance = ExhibitionEventDirector.Instance;
			if (instance != null)
			{
				instance.CancelForSceneTransition();
			}
			LongWayHomeRuntimeController.Instance.ReleaseEncounterHold();
			LongWayHomeRuntimeController.Instance._encounterWitness = null;
			return true;
		}

		// Token: 0x0600608D RID: 24717 RVA: 0x002645CC File Offset: 0x002627CC
		private IEnumerator WinAndReturnHome()
		{
			this.Phase = LongWayHomeRuntimeController.RunPhase.Won;
			AchievementTrackingService.RecordLongWayHomeWin();
			LongWayHomeRuntimeController.ClosePhone();
			this.HoldTerminal();
			this.PlayArrivalDoorSound();
			MatchStatsService instance = MatchStatsService.Instance;
			if (instance != null)
			{
				instance.CompleteRun();
			}
			LongWayHomeBanner.Show(base.transform, "HOME SWEET HOME", string.Format("LONG WAY HOME COMPLETE  •  {0:0}s", this.ElapsedSeconds));
			Debug.Log("[LongWayHome] Completed at the home entrance.");
			yield return new WaitForSecondsRealtime(3.5f);
			this.ReturnHome();
			yield break;
		}

		// Token: 0x0600608E RID: 24718 RVA: 0x002645DB File Offset: 0x002627DB
		private IEnumerator AbortOpening(string error)
		{
			this.Phase = LongWayHomeRuntimeController.RunPhase.Failed;
			this.IsOpeningPrepared = false;
			this._introPlaying = false;
			Debug.LogError("[LongWayHome] Cannot start run: " + error);
			LongWayHomeIntroSequence component = base.GetComponent<LongWayHomeIntroSequence>();
			if (component != null)
			{
				component.enabled = false;
			}
			LongWayHomeWorldRoute route = this._route;
			if (route != null)
			{
				route.Clear();
			}
			LongWayHomeStartingLoot startingLoot = this._startingLoot;
			if (startingLoot != null)
			{
				startingLoot.Clear();
			}
			ScreenFadeController instance = ScreenFadeController.Instance;
			if (instance != null)
			{
				instance.SetClear();
			}
			this.ReleaseHold();
			LongWayHomeBanner.Show(base.transform, "UNABLE TO START", "Returning home. Please try again.");
			yield return new WaitForSecondsRealtime(2f);
			while (SceneTransitionManager.Instance != null && SceneTransitionManager.Instance.IsTransitioning)
			{
				yield return null;
			}
			this.ReturnHome();
			yield break;
		}

		// Token: 0x0600608F RID: 24719 RVA: 0x002645F4 File Offset: 0x002627F4
		public void FailPreparation(string error)
		{
			if (this.Phase != LongWayHomeRuntimeController.RunPhase.Preparing)
			{
				return;
			}
			if (this._opening != null)
			{
				base.StopCoroutine(this._opening);
			}
			if (this._route != null)
			{
				this._route.Clear();
			}
			LongWayHomeIntroSequence component = base.GetComponent<LongWayHomeIntroSequence>();
			if (component != null)
			{
				component.enabled = false;
			}
			this._opening = base.StartCoroutine(this.AbortOpening(error));
		}

		// Token: 0x06006090 RID: 24720 RVA: 0x00264664 File Offset: 0x00262864
		private void HoldPreparation()
		{
			if (this._heldBody == null && this._player != null)
			{
				this._heldBody = this._player.GetComponent<Rigidbody>();
				if (this._heldBody != null)
				{
					this._savedKinematic = this._heldBody.isKinematic;
					this._savedGravity = this._heldBody.useGravity;
					if (!this._heldBody.isKinematic)
					{
						this._heldBody.velocity = Vector3.zero;
						this._heldBody.angularVelocity = Vector3.zero;
					}
					this._heldBody.useGravity = false;
					this._heldBody.isKinematic = true;
				}
			}
			PlayerActivityGate.Push("LongWayHome", PlayerActivity.AllPlayerActions);
			SensoryManager instance = SensoryManager.Instance;
			if (instance != null)
			{
				instance.PushPause("LongWayHome");
			}
			MatchStatsService instance2 = MatchStatsService.Instance;
			if (instance2 != null)
			{
				instance2.SetRunTrackingActive(false);
			}
			if (this._input != null)
			{
				this._input.SetLockAllInput(true);
			}
			ScreenFadeController instance3 = ScreenFadeController.Instance;
			if (instance3 == null)
			{
				return;
			}
			instance3.SetBlack();
		}

		// Token: 0x06006091 RID: 24721 RVA: 0x00264778 File Offset: 0x00262978
		private void HoldTerminal()
		{
			PlayerActivityGate.Push("LongWayHome", PlayerActivity.AllPlayerActions);
			SensoryManager instance = SensoryManager.Instance;
			if (instance != null)
			{
				instance.PushPause("LongWayHome");
			}
			vThirdPersonInput input = this._input;
			if (input != null)
			{
				input.SetLockAllInput(true);
			}
			this.StopTerminalMovement();
			if (this._npcPause == null)
			{
				this._npcPause = NPCSimulationPause.AcquirePause();
			}
		}

		// Token: 0x06006092 RID: 24722 RVA: 0x002647D4 File Offset: 0x002629D4
		private void StopTerminalMovement()
		{
			if (this._input == null || this._input.cc == null)
			{
				return;
			}
			vThirdPersonController cc = this._input.cc;
			cc.StopCharacter();
			cc.isRunning = false;
			cc.ResetInputAnimatorParameters();
			cc.animator.SetBool(vAnimatorParameters.IsRunning, false);
		}

		// Token: 0x06006093 RID: 24723 RVA: 0x00264834 File Offset: 0x00262A34
		private void PlayArrivalDoorSound()
		{
			GameObject gameObject = Resources.Load<GameObject>("LongWayHomeArrivalDoor");
			if (gameObject == null)
			{
				return;
			}
			GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject, base.transform);
			AudioSource component = gameObject2.GetComponent<AudioSource>();
			if (component == null || component.clip == null)
			{
				Object.Destroy(gameObject2);
				return;
			}
			GameAudioRouter.Route(component, GameAudioRouter.Channel.Sfx);
			component.PlayOneShot(component.clip, 0.14f);
			Object.Destroy(gameObject2, component.clip.length + 0.1f);
		}

		// Token: 0x06006094 RID: 24724 RVA: 0x002648B7 File Offset: 0x00262AB7
		private void ReleaseHold()
		{
			this.ReleaseBodyHold();
			PlayerActivityGate.Pop("LongWayHome");
			SensoryManager instance = SensoryManager.Instance;
			if (instance != null)
			{
				instance.PopPause("LongWayHome");
			}
			IDisposable npcPause = this._npcPause;
			if (npcPause != null)
			{
				npcPause.Dispose();
			}
			this._npcPause = null;
		}

		// Token: 0x06006095 RID: 24725 RVA: 0x002648F6 File Offset: 0x00262AF6
		private void ReleaseBodyHold()
		{
			if (this._heldBody == null)
			{
				return;
			}
			this._heldBody.isKinematic = this._savedKinematic;
			this._heldBody.useGravity = this._savedGravity;
			this._heldBody = null;
		}

		// Token: 0x06006096 RID: 24726 RVA: 0x00264930 File Offset: 0x00262B30
		private void TeleportPlayer(Vector3 position, Quaternion rotation)
		{
			this._player.SetPositionAndRotation(position, rotation);
			Rigidbody component = this._player.GetComponent<Rigidbody>();
			if (component != null)
			{
				component.position = position;
				component.rotation = rotation;
				if (!component.isKinematic)
				{
					component.velocity = Vector3.zero;
					component.angularVelocity = Vector3.zero;
				}
			}
			if (this._input != null && this._input.cc != null)
			{
				this._input.cc.verticalVelocity = 0f;
				this._input.cc.heightReached = position.y;
			}
			Physics.SyncTransforms();
			foreach (MagicaCloth magicaCloth in this._player.GetComponentsInChildren<MagicaCloth>(true))
			{
				if (magicaCloth != null && magicaCloth.isActiveAndEnabled)
				{
					magicaCloth.ResetCloth(false);
				}
			}
		}

		// Token: 0x06006097 RID: 24727 RVA: 0x00264A14 File Offset: 0x00262C14
		private static void ClosePhone()
		{
			SmartphoneController smartphoneController = Object.FindObjectOfType<SmartphoneController>(true);
			if (smartphoneController != null && smartphoneController.IsPhoneOut)
			{
				smartphoneController.ExitToGameplay();
			}
		}

		// Token: 0x06006098 RID: 24728 RVA: 0x00264A40 File Offset: 0x00262C40
		private void ReturnHome()
		{
			this.ReleaseHold();
			vThirdPersonInput input = this._input;
			if (input != null)
			{
				input.SetLockAllInput(false);
			}
			if (GameOverManager.Instance != null)
			{
				GameOverManager.Instance.ReturnToHome();
				return;
			}
			if (SceneTransitionManager.Instance != null)
			{
				SceneTransitionManager.Instance.LoadGameplayScene("CielHome");
				return;
			}
			SceneManager.LoadScene("CielHome");
		}

		// Token: 0x06006099 RID: 24729 RVA: 0x00264AA4 File Offset: 0x00262CA4
		private void ResetRun()
		{
			if (this._opening != null)
			{
				base.StopCoroutine(this._opening);
			}
			this._opening = null;
			base.StopAllCoroutines();
			LongWayHomeIntroSequence component = base.GetComponent<LongWayHomeIntroSequence>();
			if (component != null)
			{
				component.enabled = false;
				Object.Destroy(component);
			}
			this.ReleaseEncounterHold();
			if (this._input != null)
			{
				LoadingScreenController existingInstance = LoadingScreenController.ExistingInstance;
				bool flag2;
				bool flag3;
				bool flag = existingInstance != null && existingInstance.TryGetPreLoadingInputLocks(this._input, out flag2, out flag3);
				this._input.SetLockAllInput(flag);
			}
			this.ReleaseHold();
			if (this._startingLoot != null)
			{
				this._startingLoot.Clear();
				Object.Destroy(this._startingLoot);
				this._startingLoot = null;
			}
			LongWayHomeLootPolicy.Clear();
			if (this._route != null)
			{
				this._route.Clear();
				Object.Destroy(this._route);
			}
			foreach (object obj in base.transform)
			{
				Object.Destroy(((Transform)obj).gameObject);
			}
			this._route = null;
			this._player = null;
			this._input = null;
			this._introPlaying = false;
			this.AwaitingPoiPrologue = false;
			this._skipCityWakeUp = false;
			this.PoiPrologueCompleted = false;
			this._poiCityPending = false;
			this._poiPrologue = LongWayHomePrologue.Restaurant;
			this._hasPlannedCitySeed = false;
			this._encounterWitness = null;
			this._policeCaptureWitness = null;
			this.IsOpeningPrepared = false;
			this.StartingDistance = 0f;
			this.Phase = LongWayHomeRuntimeController.RunPhase.Idle;
		}

		// Token: 0x0600609A RID: 24730 RVA: 0x00264C50 File Offset: 0x00262E50
		private void OnDestroy()
		{
			SceneManager.sceneLoaded -= this.OnSceneLoaded;
			SceneManager.activeSceneChanged -= this.OnActiveSceneChanged;
			LongWayHomeStartingLoot startingLoot = this._startingLoot;
			if (startingLoot != null)
			{
				startingLoot.Clear();
			}
			this.ReleaseEncounterHold();
			this.ReleaseHold();
			if (LongWayHomeRuntimeController.Instance == this)
			{
				LongWayHomeLootPolicy.Clear();
				LongWayHomeRuntimeController.Instance = null;
			}
		}

		// Token: 0x0400620B RID: 25099
		private const string CityScene = "Game - Current";

		// Token: 0x0400620C RID: 25100
		public const string GymShowerPrologueScene = "POI - Mall";

		// Token: 0x0400620D RID: 25101
		public const string UniversityPrologueScene = "POI - University";

		// Token: 0x0400620E RID: 25102
		public const string StadiumPrologueScene = "POI - Stadium";

		// Token: 0x0400620F RID: 25103
		private const string HoldReason = "LongWayHome";

		// Token: 0x04006210 RID: 25104
		private const string EncounterHoldReason = "LongWayHome.Encounter";

		// Token: 0x04006218 RID: 25112
		private LongWayHomeWorldRoute _route;

		// Token: 0x04006219 RID: 25113
		private LongWayHomeStartingLoot _startingLoot;

		// Token: 0x0400621A RID: 25114
		private Transform _player;

		// Token: 0x0400621B RID: 25115
		private vThirdPersonInput _input;

		// Token: 0x0400621C RID: 25116
		private Coroutine _opening;

		// Token: 0x0400621D RID: 25117
		private IDisposable _npcPause;

		// Token: 0x0400621E RID: 25118
		private bool _introPlaying;

		// Token: 0x0400621F RID: 25119
		private bool _skipCityWakeUp;

		// Token: 0x04006220 RID: 25120
		private Rigidbody _heldBody;

		// Token: 0x04006221 RID: 25121
		private bool _savedKinematic;

		// Token: 0x04006222 RID: 25122
		private bool _savedGravity;

		// Token: 0x04006223 RID: 25123
		private NPCSenses _encounterWitness;

		// Token: 0x04006224 RID: 25124
		private bool _poiCityPending;

		// Token: 0x04006225 RID: 25125
		private LongWayHomePrologue _poiPrologue;

		// Token: 0x04006226 RID: 25126
		private bool _hasPlannedCitySeed;

		// Token: 0x04006227 RID: 25127
		private int _plannedCitySeed;

		// Token: 0x04006228 RID: 25128
		private NPCSenses _policeCaptureWitness;

		// Token: 0x04006229 RID: 25129
		private bool _pendingInputHeld;

		// Token: 0x0400622A RID: 25130
		private bool _previousInputLock;

		// Token: 0x02000CF6 RID: 3318
		public enum RunPhase
		{
			// Token: 0x0400622C RID: 25132
			Idle,
			// Token: 0x0400622D RID: 25133
			Preparing,
			// Token: 0x0400622E RID: 25134
			Running,
			// Token: 0x0400622F RID: 25135
			Won,
			// Token: 0x04006230 RID: 25136
			Lost,
			// Token: 0x04006231 RID: 25137
			Failed,
			// Token: 0x04006232 RID: 25138
			Encounter
		}
	}
}
