using System;
using System.Collections.Generic;
using System.Globalization;
using ExhibitionistDemo.UI;
using Features.Achievements;
using Features.Audio;
using Features.CarHide;
using Features.Detection;
using Features.GameOver;
using Features.Inventory;
using Features.Loading;
using Features.MasturbationSystem;
using Features.Missions;
using Features.PlayerEmotes;
using Features.RenderingPrecision;
using Features.RunModes;
using Features.Score;
using Features.Vehicles.Traffic;
using Invector;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using ProjectExhibitionist.Core.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using VehicleSpawnPlugin;

namespace Features.MatchRecap
{
	// Token: 0x02000FDD RID: 4061
	[DisallowMultipleComponent]
	public class MatchStatsService : MonoBehaviour
	{
		// Token: 0x170013DE RID: 5086
		// (get) Token: 0x06007AA8 RID: 31400 RVA: 0x00321896 File Offset: 0x0031FA96
		// (set) Token: 0x06007AA9 RID: 31401 RVA: 0x0032189D File Offset: 0x0031FA9D
		public static MatchStatsService Instance { get; private set; }

		// Token: 0x170013DF RID: 5087
		// (get) Token: 0x06007AAA RID: 31402 RVA: 0x003218A5 File Offset: 0x0031FAA5
		public float VehicleTravelMeters
		{
			get
			{
				this.SampleVehicleTravel();
				return this._vehicleTravelMeters;
			}
		}

		// Token: 0x170013E0 RID: 5088
		// (get) Token: 0x06007AAB RID: 31403 RVA: 0x003218B3 File Offset: 0x0031FAB3
		private bool CanCollect
		{
			get
			{
				return !this.isMocked && this._runTrackingActive && !this._lifetimeStatsCommitted && !LongWayHomeRuntimeController.IsPreparing && !WardrobeGameplayContext.IsPresentationOnly(this);
			}
		}

		// Token: 0x06007AAC RID: 31404 RVA: 0x003218E0 File Offset: 0x0031FAE0
		private void Awake()
		{
			if (MatchStatsService.Instance != null && MatchStatsService.Instance != this)
			{
				Object.Destroy(this);
				return;
			}
			MatchStatsService.Instance = this;
			if (base.GetComponent<RunScoreDirector>() == null)
			{
				base.gameObject.AddComponent<RunScoreDirector>();
			}
		}

		// Token: 0x06007AAD RID: 31405 RVA: 0x0032192E File Offset: 0x0031FB2E
		private void Start()
		{
			if (!this.isMocked && this._runTrackingActive)
			{
				this.BindLiveSources();
			}
		}

		// Token: 0x06007AAE RID: 31406 RVA: 0x00321946 File Offset: 0x0031FB46
		private void OnDestroy()
		{
			this.UnbindLiveSources();
			this.UnbindFootsteps();
			if (MatchStatsService.Instance == this)
			{
				MatchStatsService.Instance = null;
			}
		}

		// Token: 0x170013E1 RID: 5089
		// (get) Token: 0x06007AAF RID: 31407 RVA: 0x00321967 File Offset: 0x0031FB67
		public bool IsMocked
		{
			get
			{
				return this.isMocked;
			}
		}

		// Token: 0x170013E2 RID: 5090
		// (get) Token: 0x06007AB0 RID: 31408 RVA: 0x0032196F File Offset: 0x0031FB6F
		public bool IsRunTrackingActive
		{
			get
			{
				return this._runTrackingActive;
			}
		}

		// Token: 0x170013E3 RID: 5091
		// (get) Token: 0x06007AB1 RID: 31409 RVA: 0x00321977 File Offset: 0x0031FB77
		public int CurrentPoints
		{
			get
			{
				return this._points;
			}
		}

		// Token: 0x170013E4 RID: 5092
		// (get) Token: 0x06007AB2 RID: 31410 RVA: 0x0032197F File Offset: 0x0031FB7F
		public float PeakDistanceMeters
		{
			get
			{
				return this._peakDistanceMeters;
			}
		}

		// Token: 0x170013E5 RID: 5093
		// (get) Token: 0x06007AB3 RID: 31411 RVA: 0x00321987 File Offset: 0x0031FB87
		public IReadOnlyList<RunScoreAward> ScoreAwards
		{
			get
			{
				return this._scoreAwards;
			}
		}

		// Token: 0x140000D6 RID: 214
		// (add) Token: 0x06007AB4 RID: 31412 RVA: 0x00321990 File Offset: 0x0031FB90
		// (remove) Token: 0x06007AB5 RID: 31413 RVA: 0x003219C8 File Offset: 0x0031FBC8
		public event Action<int, int> PointsChanged;

		// Token: 0x140000D7 RID: 215
		// (add) Token: 0x06007AB6 RID: 31414 RVA: 0x00321A00 File Offset: 0x0031FC00
		// (remove) Token: 0x06007AB7 RID: 31415 RVA: 0x00321A38 File Offset: 0x0031FC38
		public event Action<RunScoreAward> ScoreAwarded;

		// Token: 0x140000D8 RID: 216
		// (add) Token: 0x06007AB8 RID: 31416 RVA: 0x00321A70 File Offset: 0x0031FC70
		// (remove) Token: 0x06007AB9 RID: 31417 RVA: 0x00321AA8 File Offset: 0x0031FCA8
		public event Action<bool> RunTrackingChanged;

		// Token: 0x140000D9 RID: 217
		// (add) Token: 0x06007ABA RID: 31418 RVA: 0x00321AE0 File Offset: 0x0031FCE0
		// (remove) Token: 0x06007ABB RID: 31419 RVA: 0x00321B18 File Offset: 0x0031FD18
		public event Action RunReset;

		// Token: 0x06007ABC RID: 31420 RVA: 0x00321B50 File Offset: 0x0031FD50
		public void SetRunTrackingActive(bool active)
		{
			if (!active)
			{
				this.FinishVehicleRide();
			}
			bool flag = this._runTrackingActive != active;
			this._runTrackingActive = active;
			if (active && !this.isMocked)
			{
				this.BindLiveSources();
			}
			else
			{
				this.UnbindLiveSources();
				this._isRiding = false;
			}
			if (flag)
			{
				Action<bool> runTrackingChanged = this.RunTrackingChanged;
				if (runTrackingChanged == null)
				{
					return;
				}
				runTrackingChanged(active);
			}
		}

		// Token: 0x06007ABD RID: 31421 RVA: 0x00321BAC File Offset: 0x0031FDAC
		public void ResetForNewRun()
		{
			AchievementTrackingService.PrepareDeparture();
			NpcPhotoSessionArchive.Clear();
			RunScoreDirector component = base.GetComponent<RunScoreDirector>();
			if (component != null)
			{
				component.ResetForNewRun();
			}
			int points = this._points;
			this._timesSeen = 0;
			this._timesExhibitioned = 0;
			this._vehicleDistanceMeters = 0f;
			this._vehicleTravelMeters = 0f;
			this._carSurfCount = 0;
			this._points = 0;
			this._peakDistanceMeters = 0f;
			this._homeReturnAwarded = false;
			this._homeReturnBonus = 0;
			this._homeReturnMissionsCompleted = 0;
			this._homeReturnMissionXp = 0f;
			this._homeReturnMissionRewards = null;
			this._scoreAwards.Clear();
			this._visitedPoiIds.Clear();
			this._bpmSum = 0.0;
			this._bpmSampleCount = 0;
			this._nextBpmSampleTime = 0f;
			this._isRiding = false;
			this._rideStartDistance = 0f;
			this._lifetimeStatsCommitted = false;
			this._hasCompletedSnapshot = false;
			this._completedSnapshot = default(MatchStatsSnapshot);
			this._activeVehicle = null;
			this.ResetHealthTelemetry();
			Action<int, int> pointsChanged = this.PointsChanged;
			if (pointsChanged != null)
			{
				pointsChanged(this._points, -points);
			}
			Action runReset = this.RunReset;
			if (runReset == null)
			{
				return;
			}
			runReset();
		}

		// Token: 0x06007ABE RID: 31422 RVA: 0x00321CD8 File Offset: 0x0031FED8
		public MatchStatsSnapshot GetSnapshot()
		{
			if (this._hasCompletedSnapshot)
			{
				return this._completedSnapshot;
			}
			MatchStatsSnapshot matchStatsSnapshot;
			if (this.isMocked)
			{
				matchStatsSnapshot = new MatchStatsSnapshot
				{
					DistanceMeters = this.mockDistanceMeters,
					DistanceWithVehiclesMeters = this.mockDistanceWithVehiclesMeters,
					TimesSeen = this.mockTimesSeen,
					TimesExhibitioned = this.mockTimesExhibitioned,
					LocationsVisited = this.mockLocationsVisited,
					AverageHeartbeatBpm = this.mockAverageHeartbeatBpm,
					CarSurfCount = this.mockCarSurfCount,
					Points = Mathf.Max(0, this.mockPoints)
				};
				return matchStatsSnapshot;
			}
			float num = this._vehicleDistanceMeters;
			if (this._isRiding && this.distanceTracker != null)
			{
				float num2 = this.distanceTracker.GetDistance() - this._rideStartDistance;
				if (num2 > 0f)
				{
					num += num2;
				}
			}
			matchStatsSnapshot = new MatchStatsSnapshot
			{
				DistanceMeters = (this._homeReturnAwarded ? this._peakDistanceMeters : ((this.distanceTracker != null) ? this.distanceTracker.GetDistance() : 0f)),
				PeakDistanceMeters = this._peakDistanceMeters,
				HomeReturnBonus = this._homeReturnBonus,
				HomeReturnMissionsCompleted = this._homeReturnMissionsCompleted,
				HomeReturnMissionXp = this._homeReturnMissionXp,
				HomeReturnMissionRewards = this._homeReturnMissionRewards,
				DistanceWithVehiclesMeters = num,
				TimesSeen = this._timesSeen,
				TimesExhibitioned = this._timesExhibitioned,
				LocationsVisited = this._visitedPoiIds.Count,
				AverageHeartbeatBpm = ((this._bpmSampleCount > 0) ? Mathf.RoundToInt((float)(this._bpmSum / (double)this._bpmSampleCount)) : 0),
				CarSurfCount = this._carSurfCount,
				Points = this._points,
				PlayTimeSeconds = this._playTimeSeconds,
				WalkingSeconds = this._walkingSeconds,
				RunningSeconds = this._runningSeconds,
				EmoteSeconds = this._emoteSeconds,
				VehicleSeconds = this._vehicleSeconds,
				CarSeconds = this._carSeconds,
				BikeSeconds = this._bikeSeconds,
				BoardSeconds = this._boardSeconds,
				SkatesSeconds = this._skatesSeconds,
				RoofSeconds = this._roofSeconds,
				Steps = this._steps,
				HealthDays = this.SnapshotHealthDays(),
				WalkingMeters = this._walkingMeters,
				RunningMeters = this._runningMeters
			};
			return matchStatsSnapshot;
		}

		// Token: 0x06007ABF RID: 31423 RVA: 0x00321F5C File Offset: 0x0032015C
		public bool AddPoints(int amount)
		{
			if (this.CanCollect && amount > 0)
			{
				RunModeService.DisqualifyLeaderboardRun("Direct debug points were added. This run cannot submit a score.");
			}
			RunScoreAward runScoreAward;
			return this.CommitPoints(new RunScoreAwardRequest(RunScoreReason.Legacy, amount, "SCORE", null, null, RunScorePresentation.Silent), false, out runScoreAward);
		}

		// Token: 0x06007AC0 RID: 31424 RVA: 0x00321F97 File Offset: 0x00320197
		public bool AwardPoints(RunScoreAwardRequest request, out RunScoreAward award)
		{
			return this.CommitPoints(request, true, out award);
		}

		// Token: 0x06007AC1 RID: 31425 RVA: 0x00321FA4 File Offset: 0x003201A4
		private bool CommitPoints(RunScoreAwardRequest request, bool enforceGameplayPresentationGate, out RunScoreAward award)
		{
			award = default(RunScoreAward);
			if (!this.CanCollect || request.Points <= 0 || this._points == 2147483647)
			{
				return false;
			}
			if (enforceGameplayPresentationGate)
			{
				GameOverManager instance = GameOverManager.Instance;
				if (instance != null && instance.IsGameOver)
				{
					return false;
				}
				ExhibitionEventDirector instance2 = ExhibitionEventDirector.Instance;
				if (instance2 != null && instance2.isActiveAndEnabled && instance2.IsEventActive)
				{
					return false;
				}
			}
			int points = this._points;
			if (request.Points > 2147483647 - this._points || !Enum.IsDefined(typeof(RunScoreReason), request.Reason))
			{
				RunModeService.DisqualifyLeaderboardRun("The run score exceeded its valid range. No score will be submitted.");
			}
			this._points = ((request.Points > int.MaxValue - this._points) ? int.MaxValue : (this._points + request.Points));
			int num = this._points - points;
			if (num <= 0)
			{
				return false;
			}
			award = new RunScoreAward(request, num, this._points, Time.unscaledTime);
			this._scoreAwards.Add(award);
			Action<RunScoreAward> scoreAwarded = this.ScoreAwarded;
			if (scoreAwarded != null)
			{
				scoreAwarded(award);
			}
			Action<int, int> pointsChanged = this.PointsChanged;
			if (pointsChanged != null)
			{
				pointsChanged(this._points, num);
			}
			return true;
		}

		// Token: 0x06007AC2 RID: 31426 RVA: 0x003220EC File Offset: 0x003202EC
		public void RegisterCarSurf()
		{
			if (!this.CanCollect)
			{
				return;
			}
			this._carSurfCount++;
		}

		// Token: 0x06007AC3 RID: 31427 RVA: 0x00322105 File Offset: 0x00320305
		public void AddVehicleDistance(float meters)
		{
			if (this.CanCollect && meters > 0f && !float.IsInfinity(meters))
			{
				this._vehicleDistanceMeters += meters;
			}
		}

		// Token: 0x06007AC4 RID: 31428 RVA: 0x00322130 File Offset: 0x00320330
		public void SetMockedOverride(bool mocked)
		{
			if (this.isMocked == mocked)
			{
				return;
			}
			if (mocked)
			{
				RunModeService.DisqualifyLeaderboardRun("Mock statistics were enabled. This run cannot submit a score.");
			}
			this.isMocked = mocked;
			if (!this.isMocked && this._runTrackingActive)
			{
				if (!this._spottedSubscribed && !this._vehicleSubscribed)
				{
					this.BindLiveSources();
					return;
				}
			}
			else
			{
				this.UnbindLiveSources();
			}
		}

		// Token: 0x06007AC5 RID: 31429 RVA: 0x00322188 File Offset: 0x00320388
		private void BindLiveSources()
		{
			if (!this._spottedSubscribed)
			{
				NPCSenses.OnPlayerSpotted += this.HandlePlayerSpotted;
				this._spottedSubscribed = true;
			}
			if (!this._vehicleSubscribed)
			{
				vVehicleController.OnAnyVehicleMounted += this.HandleVehicleMounted;
				vVehicleController.OnAnyVehicleDismounted += this.HandleVehicleDismounted;
				SkateboardInteractable.OnAnyMounted += this.HandleSkateboardMounted;
				SkateboardInteractable.OnAnyDismounted += this.HandleSkateboardDismounted;
				SimpleRollerSkatesInteractable.OnAnyMounted += this.HandleRollersMounted;
				SimpleRollerSkatesInteractable.OnAnyDismounted += this.HandleRollersDismounted;
				CarRoofCarrier.OnPlayerMounted += this.HandleCarRoofMounted;
				CarRoofCarrier.OnPlayerDismounted += this.HandleCarRoofDismounted;
				CarHideUnderInteractable.OnPlayerMounted += this.HandleCarHideMounted;
				CarHideUnderInteractable.OnPlayerDismounted += this.HandleCarRoofDismounted;
				this._vehicleSubscribed = true;
			}
			this.BindSceneSources();
		}

		// Token: 0x06007AC6 RID: 31430 RVA: 0x00322278 File Offset: 0x00320478
		private void BindSceneSources()
		{
			if (this.distanceTracker == null)
			{
				this.distanceTracker = Object.FindObjectOfType<DistanceTracker>();
			}
			this.BindFootsteps();
			if (this.heartbeatController == null)
			{
				this.heartbeatController = Object.FindObjectOfType<HeartbeatController>();
			}
			if (this.poiContext == null)
			{
				this.poiContext = Object.FindObjectOfType<PlayerPoiEmoteContext>();
			}
			if (this.poiContext != this._boundPoiContext)
			{
				if (this._boundPoiContext != null)
				{
					this._boundPoiContext.OnPoiVisited -= this.HandleZoneEntered;
				}
				this._boundPoiContext = this.poiContext;
				if (this._boundPoiContext != null)
				{
					this._boundPoiContext.OnPoiVisited += this.HandleZoneEntered;
				}
			}
			if (this.emoteController == null)
			{
				this.emoteController = Object.FindObjectOfType<GameplayEmoteController>();
			}
			if (this.emoteController != this._boundEmoteController)
			{
				if (this._boundEmoteController != null)
				{
					this._boundEmoteController.OnEmoteStarted -= this.HandleEmoteStarted;
				}
				this._boundEmoteController = this.emoteController;
				if (this._boundEmoteController != null)
				{
					this._boundEmoteController.OnEmoteStarted += this.HandleEmoteStarted;
				}
			}
			if (GameOverManager.Instance != this._boundGameOver)
			{
				if (this._boundGameOver != null && this._boundGameOver.OnGameOver != null)
				{
					this._boundGameOver.OnGameOver.RemoveListener(new UnityAction<GameOverReason>(this.HandleGameOver));
				}
				this._boundGameOver = GameOverManager.Instance;
				if (this._boundGameOver != null && this._boundGameOver.OnGameOver != null)
				{
					this._boundGameOver.OnGameOver.RemoveListener(new UnityAction<GameOverReason>(this.HandleGameOver));
					this._boundGameOver.OnGameOver.AddListener(new UnityAction<GameOverReason>(this.HandleGameOver));
				}
			}
			if (ExhibitionEventDirector.Instance != this._boundDirector)
			{
				if (this._boundDirector != null)
				{
					this._boundDirector.OnEncounterEmoteStarted -= this.HandleEmoteStarted;
				}
				this._boundDirector = ExhibitionEventDirector.Instance;
				if (this._boundDirector != null)
				{
					this._boundDirector.OnEncounterEmoteStarted += this.HandleEmoteStarted;
				}
			}
			this._nextSourceBindTime = Time.unscaledTime + 0.5f;
		}

		// Token: 0x06007AC7 RID: 31431 RVA: 0x003224E4 File Offset: 0x003206E4
		private void UnbindLiveSources()
		{
			this.UnbindFootsteps();
			if (this._spottedSubscribed)
			{
				NPCSenses.OnPlayerSpotted -= this.HandlePlayerSpotted;
				this._spottedSubscribed = false;
			}
			if (this._vehicleSubscribed)
			{
				vVehicleController.OnAnyVehicleMounted -= this.HandleVehicleMounted;
				vVehicleController.OnAnyVehicleDismounted -= this.HandleVehicleDismounted;
				SkateboardInteractable.OnAnyMounted -= this.HandleSkateboardMounted;
				SkateboardInteractable.OnAnyDismounted -= this.HandleSkateboardDismounted;
				SimpleRollerSkatesInteractable.OnAnyMounted -= this.HandleRollersMounted;
				SimpleRollerSkatesInteractable.OnAnyDismounted -= this.HandleRollersDismounted;
				CarRoofCarrier.OnPlayerMounted -= this.HandleCarRoofMounted;
				CarRoofCarrier.OnPlayerDismounted -= this.HandleCarRoofDismounted;
				CarHideUnderInteractable.OnPlayerMounted -= this.HandleCarHideMounted;
				CarHideUnderInteractable.OnPlayerDismounted -= this.HandleCarRoofDismounted;
				this._vehicleSubscribed = false;
			}
			if (this._boundPoiContext != null)
			{
				this._boundPoiContext.OnPoiVisited -= this.HandleZoneEntered;
				this._boundPoiContext = null;
			}
			if (this._boundEmoteController != null)
			{
				this._boundEmoteController.OnEmoteStarted -= this.HandleEmoteStarted;
				this._boundEmoteController = null;
			}
			if (this._boundGameOver != null)
			{
				if (this._boundGameOver.OnGameOver != null)
				{
					this._boundGameOver.OnGameOver.RemoveListener(new UnityAction<GameOverReason>(this.HandleGameOver));
				}
				this._boundGameOver = null;
			}
			if (this._boundDirector != null)
			{
				this._boundDirector.OnEncounterEmoteStarted -= this.HandleEmoteStarted;
				this._boundDirector = null;
			}
		}

		// Token: 0x06007AC8 RID: 31432 RVA: 0x00322698 File Offset: 0x00320898
		private void Update()
		{
			this.SampleActiveGameplayClock();
			if (!this.CanCollect)
			{
				return;
			}
			this.SampleHealthTelemetry();
			this.SampleVehicleTravel();
			this.SamplePeakDistance();
			if (Time.unscaledTime >= this._nextSourceBindTime && (this.distanceTracker == null || this._stepMotor == null || this.heartbeatController == null || this.poiContext == null || this.emoteController == null || this._boundGameOver != GameOverManager.Instance || this._boundDirector != ExhibitionEventDirector.Instance))
			{
				this.BindSceneSources();
			}
			if (this.heartbeatController != null && Time.time >= this._nextBpmSampleTime)
			{
				this._bpmSum += (double)this.heartbeatController.CurrentBpm;
				this._bpmSampleCount++;
				this._nextBpmSampleTime = Time.time + Mathf.Max(0.05f, this.bpmSampleInterval);
			}
		}

		// Token: 0x06007AC9 RID: 31433 RVA: 0x003227A1 File Offset: 0x003209A1
		private void HandlePlayerSpotted(NPCSenses npc)
		{
			if (this.CanCollect && npc != null && !WardrobeGameplayContext.IsPresentationOnly(npc))
			{
				this._timesSeen++;
			}
		}

		// Token: 0x06007ACA RID: 31434 RVA: 0x003227CA File Offset: 0x003209CA
		private void HandleVehicleMounted(vVehicleController vehicle, vThirdPersonController rider)
		{
			this.BeginVehicleRide(vehicle, (rider != null) ? rider.transform : null);
		}

		// Token: 0x06007ACB RID: 31435 RVA: 0x003227E6 File Offset: 0x003209E6
		private void HandleVehicleDismounted(vVehicleController vehicle, vThirdPersonController rider)
		{
			this.EndVehicleRide(vehicle);
		}

		// Token: 0x06007ACC RID: 31436 RVA: 0x003227CA File Offset: 0x003209CA
		private void HandleSkateboardMounted(SkateboardInteractable vehicle, vThirdPersonController rider)
		{
			this.BeginVehicleRide(vehicle, (rider != null) ? rider.transform : null);
		}

		// Token: 0x06007ACD RID: 31437 RVA: 0x003227E6 File Offset: 0x003209E6
		private void HandleSkateboardDismounted(SkateboardInteractable vehicle, vThirdPersonController rider)
		{
			this.EndVehicleRide(vehicle);
		}

		// Token: 0x06007ACE RID: 31438 RVA: 0x003227CA File Offset: 0x003209CA
		private void HandleRollersMounted(SimpleRollerSkatesInteractable vehicle, vThirdPersonController rider)
		{
			this.BeginVehicleRide(vehicle, (rider != null) ? rider.transform : null);
		}

		// Token: 0x06007ACF RID: 31439 RVA: 0x003227E6 File Offset: 0x003209E6
		private void HandleRollersDismounted(SimpleRollerSkatesInteractable vehicle, vThirdPersonController rider)
		{
			this.EndVehicleRide(vehicle);
		}

		// Token: 0x06007AD0 RID: 31440 RVA: 0x003227EF File Offset: 0x003209EF
		private void HandleCarRoofMounted(VehicleAI vehicle, Transform rider)
		{
			this.BeginVehicleRide(vehicle, rider);
		}

		// Token: 0x06007AD1 RID: 31441 RVA: 0x003227E6 File Offset: 0x003209E6
		private void HandleCarRoofDismounted(VehicleAI vehicle, Transform rider)
		{
			this.EndVehicleRide(vehicle);
		}

		// Token: 0x06007AD2 RID: 31442 RVA: 0x003227FA File Offset: 0x003209FA
		private void HandleCarHideMounted(VehicleAI vehicle, Transform rider)
		{
			if (this.BeginVehicleRide(vehicle, rider))
			{
				this._vehicleKind = MatchStatsService.VehicleKind.Car;
			}
		}

		// Token: 0x06007AD3 RID: 31443 RVA: 0x00322810 File Offset: 0x00320A10
		private bool BeginVehicleRide(Object vehicle, Transform rider)
		{
			if (!this.CanCollect || vehicle == null || rider == null || WardrobeGameplayContext.IsPresentationOnly(rider))
			{
				return false;
			}
			if (this.distanceTracker == null)
			{
				this.distanceTracker = rider.GetComponentInChildren<DistanceTracker>();
			}
			if (this.distanceTracker == null || (this.distanceTracker.transform != rider && !this.distanceTracker.transform.IsChildOf(rider)))
			{
				return false;
			}
			if (this._isRiding && this._activeVehicle == vehicle)
			{
				return true;
			}
			this.FinishVehicleRide();
			this._activeVehicle = vehicle;
			this._lastVehicleTravelPosition = this.GetVehicleTravelPosition();
			this._lastVehicleTravelTime = Time.time;
			this._rideStartDistance = this.distanceTracker.GetDistance();
			this._isRiding = true;
			this._vehicleKind = MatchStatsService.ClassifyVehicle(vehicle);
			this.ResetStepBaseline();
			return true;
		}

		// Token: 0x06007AD4 RID: 31444 RVA: 0x003228F8 File Offset: 0x00320AF8
		private void EndVehicleRide(Object vehicle)
		{
			if (this._activeVehicle == vehicle)
			{
				this.FinishVehicleRide();
			}
		}

		// Token: 0x06007AD5 RID: 31445 RVA: 0x00322910 File Offset: 0x00320B10
		private void FinishVehicleRide()
		{
			this.SampleVehicleTravel();
			if (this._isRiding && this.CanCollect && this.distanceTracker != null)
			{
				float num = this.distanceTracker.GetDistance() - this._rideStartDistance;
				if (num > 0f)
				{
					this._vehicleDistanceMeters += num;
				}
			}
			this._isRiding = false;
			this._activeVehicle = null;
			this._vehicleKind = MatchStatsService.VehicleKind.None;
			this.ResetStepBaseline();
		}

		// Token: 0x06007AD6 RID: 31446 RVA: 0x00322988 File Offset: 0x00320B88
		private Vector3 GetVehicleTravelPosition()
		{
			Component component = this._activeVehicle as Component;
			return WorldOrigin.ToLogical((component != null) ? component.transform.position : Vector3.zero);
		}

		// Token: 0x06007AD7 RID: 31447 RVA: 0x003229BC File Offset: 0x00320BBC
		private void SampleVehicleTravel()
		{
			if (!this._isRiding || this._activeVehicle == null)
			{
				return;
			}
			Vector3 vehicleTravelPosition = this.GetVehicleTravelPosition();
			Vector3 vector = vehicleTravelPosition - this._lastVehicleTravelPosition;
			float num = Mathf.Max(0f, Time.time - this._lastVehicleTravelTime);
			this._lastVehicleTravelPosition = vehicleTravelPosition;
			this._lastVehicleTravelTime = Time.time;
			if (!this.CanCollect || MatchStatsService.IsActivityClockPaused())
			{
				return;
			}
			vector.y = 0f;
			float magnitude = vector.magnitude;
			if (!float.IsNaN(magnitude) && !float.IsInfinity(magnitude) && magnitude <= Mathf.Max(25f, num * 80f))
			{
				this._vehicleTravelMeters += magnitude;
			}
		}

		// Token: 0x06007AD8 RID: 31448 RVA: 0x00322A73 File Offset: 0x00320C73
		private void HandleEmoteStarted(EmoteDefinitionSO emote)
		{
			if (this.CanCollect && emote != null && !WardrobeGameplayContext.IsPresentationOnly(this._boundEmoteController))
			{
				this._timesExhibitioned++;
			}
		}

		// Token: 0x06007AD9 RID: 31449 RVA: 0x00322AA1 File Offset: 0x00320CA1
		private void HandleGameOver(GameOverReason reason)
		{
			this.CompleteRun();
		}

		// Token: 0x06007ADA RID: 31450 RVA: 0x00322AAC File Offset: 0x00320CAC
		private void SamplePeakDistance()
		{
			if (!this.CanCollect || this.distanceTracker == null || (GameOverManager.Instance != null && GameOverManager.Instance.IsGameOver))
			{
				return;
			}
			float distance = this.distanceTracker.GetDistance();
			if (!float.IsNaN(distance) && !float.IsInfinity(distance))
			{
				this._peakDistanceMeters = Mathf.Max(this._peakDistanceMeters, distance);
			}
		}

		// Token: 0x06007ADB RID: 31451 RVA: 0x00322B18 File Offset: 0x00320D18
		internal bool TryAwardHomeReturn()
		{
			MatchStatsService.<>c__DisplayClass111_0 CS$<>8__locals1 = new MatchStatsService.<>c__DisplayClass111_0();
			if (!this.CanCollect || this._homeReturnAwarded || RunModeService.CurrentMode != RunModeKind.Classic)
			{
				return false;
			}
			this.SamplePeakDistance();
			int num = ClassicHomeReturnPolicy.CalculateBonus(this._peakDistanceMeters);
			this._homeReturnAwarded = true;
			if (num <= 0)
			{
				return true;
			}
			RunScoreAward runScoreAward;
			if (!this.AwardPoints(new RunScoreAwardRequest(RunScoreReason.ReturnedHome, num, "HOME, SAFE", "DISTANCE BONUS", "home-return", RunScorePresentation.External), out runScoreAward))
			{
				this._homeReturnAwarded = false;
				return false;
			}
			this._homeReturnBonus = runScoreAward.Points;
			CS$<>8__locals1.rewards = new List<MissionCompletion>();
			MissionService.Completed += CS$<>8__locals1.<TryAwardHomeReturn>g__Collect|0;
			try
			{
				MissionService.ReportClassicHomeReturn(this._peakDistanceMeters, out this._homeReturnMissionsCompleted, out this._homeReturnMissionXp);
				this._homeReturnMissionRewards = CS$<>8__locals1.rewards.ToArray();
			}
			finally
			{
				MissionService.Completed -= CS$<>8__locals1.<TryAwardHomeReturn>g__Collect|0;
			}
			return true;
		}

		// Token: 0x06007ADC RID: 31452 RVA: 0x00322C04 File Offset: 0x00320E04
		internal bool ValidateCompletedRankedScore(int score, out string reason)
		{
			reason = "The completed score does not match the run record.";
			if (this.isMocked || !this._hasCompletedSnapshot || !this._lifetimeStatsCommitted || this._runTrackingActive || score < 0 || score == 2147483647 || score != this._points || score != this._completedSnapshot.Points)
			{
				return false;
			}
			long num = 0L;
			foreach (RunScoreAward runScoreAward in this._scoreAwards)
			{
				if (runScoreAward.Points <= 0 || runScoreAward.Reason == RunScoreReason.Legacy || !Enum.IsDefined(typeof(RunScoreReason), runScoreAward.Reason) || float.IsNaN(runScoreAward.UnscaledTimestamp) || float.IsInfinity(runScoreAward.UnscaledTimestamp))
				{
					return false;
				}
				num += (long)runScoreAward.Points;
				if (num >= 2147483647L || num != (long)runScoreAward.TotalAfterAward)
				{
					return false;
				}
			}
			if (num != (long)score)
			{
				return false;
			}
			reason = null;
			return true;
		}

		// Token: 0x06007ADD RID: 31453 RVA: 0x00322D18 File Offset: 0x00320F18
		internal bool TryGetCompletedRankedResult(out RankedRunResult result)
		{
			result = default(RankedRunResult);
			string text;
			return this.ValidateCompletedRankedScore(this._completedSnapshot.Points, out text) && RankedRunResult.TryCreate(this._completedSnapshot.DistanceMeters, this._completedSnapshot.Points, out result);
		}

		// Token: 0x06007ADE RID: 31454 RVA: 0x00322D60 File Offset: 0x00320F60
		internal bool ValidateCompletedRankedResult(float distanceMeters, int score, out string reason)
		{
			if (!this.ValidateCompletedRankedScore(score, out reason))
			{
				return false;
			}
			reason = "The completed distance does not match the run record.";
			float distanceMeters2 = this._completedSnapshot.DistanceMeters;
			if (!MatchStatsService.ValidCompletedDistance(distanceMeters) || !MatchStatsService.ValidCompletedDistance(distanceMeters2) || Math.Round((double)distanceMeters, MidpointRounding.ToEven) != Math.Round((double)distanceMeters2, MidpointRounding.ToEven))
			{
				return false;
			}
			reason = null;
			return true;
		}

		// Token: 0x06007ADF RID: 31455 RVA: 0x00322DB4 File Offset: 0x00320FB4
		private static bool ValidCompletedDistance(float meters)
		{
			return !float.IsNaN(meters) && !float.IsInfinity(meters) && meters >= 0f && (double)meters < 2147483647.0;
		}

		// Token: 0x06007AE0 RID: 31456 RVA: 0x00322DE0 File Offset: 0x00320FE0
		public void CompleteRun()
		{
			MatchStatsSnapshot matchStatsSnapshot;
			if (!this.TryCompleteRun(out matchStatsSnapshot))
			{
				return;
			}
			try
			{
				this.FlushActiveGameplayClock();
				PlayerStatsService.CommitRun(matchStatsSnapshot);
				RankedLeaderboardService.TrySubmitCurrentRunResult(matchStatsSnapshot.DistanceMeters, matchStatsSnapshot.Points);
			}
			finally
			{
				RunModeService.CloseRankedRun();
			}
		}

		// Token: 0x06007AE1 RID: 31457 RVA: 0x00322E30 File Offset: 0x00321030
		private bool TryCompleteRun(out MatchStatsSnapshot snapshot)
		{
			snapshot = default(MatchStatsSnapshot);
			if (!this.CanCollect)
			{
				return false;
			}
			this.SampleVehicleTravel();
			this._completedSnapshot = this.GetSnapshot();
			this._hasCompletedSnapshot = true;
			this._lifetimeStatsCommitted = true;
			this.SetRunTrackingActive(false);
			snapshot = this._completedSnapshot;
			return true;
		}

		// Token: 0x06007AE2 RID: 31458 RVA: 0x00322E84 File Offset: 0x00321084
		private void HandleZoneEntered(PoiEmoteProfileSO profile)
		{
			if (!this.CanCollect || WardrobeGameplayContext.IsPresentationOnly(this._boundPoiContext) || profile == null || string.IsNullOrEmpty(profile.PoiId))
			{
				return;
			}
			if (this._visitedPoiIds.Add(profile.PoiId))
			{
				RunScoreAward runScoreAward;
				this.AwardPoints(new RunScoreAwardRequest(RunScoreReason.PoiDiscovered, 400, "NEW LOCATION", profile.PoiId, "poi:" + profile.PoiId, RunScorePresentation.Generic), out runScoreAward);
			}
		}

		// Token: 0x170013E6 RID: 5094
		// (get) Token: 0x06007AE3 RID: 31459 RVA: 0x00322F00 File Offset: 0x00321100
		public int LiveSteps
		{
			get
			{
				return this._steps;
			}
		}

		// Token: 0x170013E7 RID: 5095
		// (get) Token: 0x06007AE4 RID: 31460 RVA: 0x00322F08 File Offset: 0x00321108
		public int LiveBpm
		{
			get
			{
				if (!(this.heartbeatController != null) || !this.CanCollect)
				{
					return 0;
				}
				return Mathf.RoundToInt(this.heartbeatController.CurrentBpm);
			}
		}

		// Token: 0x06007AE5 RID: 31461 RVA: 0x00322F34 File Offset: 0x00321134
		private void ResetHealthTelemetry()
		{
			this._playTimeSeconds = (this._walkingSeconds = (this._runningSeconds = (this._emoteSeconds = 0f)));
			this._vehicleSeconds = (this._carSeconds = (this._bikeSeconds = (this._boardSeconds = (this._skatesSeconds = (this._roofSeconds = 0f)))));
			this._walkingMeters = (this._runningMeters = 0f);
			this._steps = 0;
			this._runHealthDays.Clear();
			this._healthDaysSnapshot = null;
			this._vehicleKind = MatchStatsService.VehicleKind.None;
			this.ResetStepBaseline();
		}

		// Token: 0x06007AE6 RID: 31462 RVA: 0x00322FDC File Offset: 0x003211DC
		private void BindFootsteps()
		{
			if (this.distanceTracker == null || (this._stepDistanceTracker == this.distanceTracker && this._stepMotor != null))
			{
				return;
			}
			this.UnbindFootsteps();
			this._stepDistanceTracker = this.distanceTracker;
			this._stepMotor = this.distanceTracker.GetComponentInParent<vThirdPersonController>(true);
			if (this._stepMotor == null)
			{
				return;
			}
			this._stepTransform = this._stepMotor.transform;
			this._stepInput = this._stepTransform.GetComponent<vThirdPersonInput>();
			this._footstepController = this._stepTransform.GetComponentInChildren<FootstepController>(true);
			if (this._footstepController != null)
			{
				this._footstepController.Stepped += this.RecordPlayerFootstep;
			}
			else
			{
				this._footTriggers = this._stepTransform.GetComponentsInChildren<vFootStepTrigger>(true);
				for (int i = 0; i < this._footTriggers.Length; i++)
				{
					UnityEvent onStep = this._footTriggers[i].OnStep;
					if (onStep != null)
					{
						onStep.AddListener(new UnityAction(this.RecordPlayerFootstep));
					}
				}
			}
			WorldOrigin.Shifted += this.ShiftStepBaseline;
			this.ResetStepBaseline();
		}

		// Token: 0x06007AE7 RID: 31463 RVA: 0x00323108 File Offset: 0x00321308
		private void UnbindFootsteps()
		{
			if (this._footstepController != null)
			{
				this._footstepController.Stepped -= this.RecordPlayerFootstep;
			}
			if (this._footTriggers != null)
			{
				for (int i = 0; i < this._footTriggers.Length; i++)
				{
					if (this._footTriggers[i] != null)
					{
						UnityEvent onStep = this._footTriggers[i].OnStep;
						if (onStep != null)
						{
							onStep.RemoveListener(new UnityAction(this.RecordPlayerFootstep));
						}
					}
				}
			}
			WorldOrigin.Shifted -= this.ShiftStepBaseline;
			this._footTriggers = null;
			this._footstepController = null;
			this._stepDistanceTracker = null;
			this._stepTransform = null;
			this._stepMotor = null;
			this._stepInput = null;
			this.ResetStepBaseline();
		}

		// Token: 0x06007AE8 RID: 31464 RVA: 0x003231CA File Offset: 0x003213CA
		private void ResetStepBaseline()
		{
			this._hasStepBaseline = false;
			this._hasMovementBaseline = false;
			this._movementElapsed = 0f;
		}

		// Token: 0x06007AE9 RID: 31465 RVA: 0x003231E5 File Offset: 0x003213E5
		private void ShiftStepBaseline(Vector3 shift)
		{
			if (this._hasStepBaseline)
			{
				this._lastStepPosition += shift;
			}
			if (this._hasMovementBaseline)
			{
				this._lastMovementPosition += shift;
			}
		}

		// Token: 0x06007AEA RID: 31466 RVA: 0x0032321C File Offset: 0x0032141C
		private void RecordPlayerFootstep()
		{
			if (!this.CanRecordOnFootMovement())
			{
				this._hasStepBaseline = false;
				return;
			}
			Vector3 position = this._stepTransform.position;
			float time = Time.time;
			if (this._hasStepBaseline)
			{
				Vector3 vector = position - this._lastStepPosition;
				vector.y = 0f;
				if (MatchStatsService.IsPhysicalStep(vector.magnitude, time - this._lastStepTime))
				{
					if (this._steps < 2147483647)
					{
						this._steps++;
					}
					AchievementTrackingService.AddStep();
					this.RecordStepDay();
				}
			}
			this._lastStepPosition = position;
			this._lastStepTime = time;
			this._hasStepBaseline = true;
		}

		// Token: 0x06007AEB RID: 31467 RVA: 0x003232BD File Offset: 0x003214BD
		public static bool IsPhysicalStep(float meters, float secondsSincePrevious)
		{
			return secondsSincePrevious >= 0.05f && secondsSincePrevious <= 2f && meters >= 0.18f && meters <= 4f && !float.IsNaN(meters);
		}

		// Token: 0x06007AEC RID: 31468 RVA: 0x003232EC File Offset: 0x003214EC
		private void SampleHealthTelemetry()
		{
			if (MatchStatsService.IsActivityClockPaused())
			{
				this.ResetStepBaseline();
				return;
			}
			float num = Mathf.Clamp(Time.deltaTime, 0f, 0.5f);
			this.SampleOnFootMovement(num);
			this.RecordActivityTime(num);
		}

		// Token: 0x06007AED RID: 31469 RVA: 0x0032332C File Offset: 0x0032152C
		private static bool IsActivityClockPaused()
		{
			return Time.timeScale <= 0f || (LoadingScreenController.ExistingInstance != null && LoadingScreenController.ExistingInstance.IsLoadingActive) || (SceneTransitionManager.Instance != null && SceneTransitionManager.Instance.IsTransitioning);
		}

		// Token: 0x170013E8 RID: 5096
		// (get) Token: 0x06007AEE RID: 31470 RVA: 0x0032337C File Offset: 0x0032157C
		private bool IsEmoteActivityPlaying
		{
			get
			{
				return (this.emoteController != null && this.emoteController.IsPlaying) || (this._boundDirector != null && this._boundDirector.IsResolutionActionPlaying && this._boundDirector.ActiveResolutionEmote != null);
			}
		}

		// Token: 0x06007AEF RID: 31471 RVA: 0x003233D4 File Offset: 0x003215D4
		private void RecordActivityTime(float elapsed)
		{
			if (!this.CanCollect || MatchStatsService.IsActivityClockPaused() || elapsed <= 0f || float.IsNaN(elapsed) || float.IsInfinity(elapsed))
			{
				return;
			}
			this._playTimeSeconds += elapsed;
			if (this._isRiding)
			{
				this._vehicleSeconds += elapsed;
				switch (this._vehicleKind)
				{
				case MatchStatsService.VehicleKind.Car:
					this._carSeconds += elapsed;
					break;
				case MatchStatsService.VehicleKind.Bike:
					this._bikeSeconds += elapsed;
					break;
				case MatchStatsService.VehicleKind.Board:
					this._boardSeconds += elapsed;
					break;
				case MatchStatsService.VehicleKind.Skates:
					this._skatesSeconds += elapsed;
					break;
				case MatchStatsService.VehicleKind.Roof:
					this._roofSeconds += elapsed;
					break;
				}
			}
			if (this.IsEmoteActivityPlaying)
			{
				this._emoteSeconds += elapsed;
				this.ResetStepBaseline();
			}
		}

		// Token: 0x06007AF0 RID: 31472 RVA: 0x003234C0 File Offset: 0x003216C0
		private bool CanRecordOnFootMovement()
		{
			return this.CanCollect && !MatchStatsService.IsActivityClockPaused() && !this._isRiding && this._stepTransform != null && this.distanceTracker != null && !this.distanceTracker.IsSceneTrackingPaused && this._stepMotor != null && this._stepMotor.enabled && this._stepMotor.isGrounded && this._stepMotor.inputMagnitude >= 0.1f && !this._stepMotor.lockMovement && !this._stepMotor.lockAnimMovement && !this._stepMotor.customAction && !this._stepMotor.ragdolled && (this._stepInput == null || (this._stepInput.enabled && !this._stepInput.lockInput && !this._stepInput.lockMoveInput)) && !this.IsEmoteActivityPlaying;
		}

		// Token: 0x06007AF1 RID: 31473 RVA: 0x003235D8 File Offset: 0x003217D8
		private void SampleOnFootMovement(float elapsed)
		{
			if (!this.CanRecordOnFootMovement() || elapsed <= 0f)
			{
				this.ResetStepBaseline();
				return;
			}
			Vector3 position = this._stepTransform.position;
			if (this._hasMovementBaseline)
			{
				this._movementElapsed = Mathf.Min(this._movementElapsed + elapsed, Mathf.Max(0.1f, elapsed));
				Vector3 vector = position - this._lastMovementPosition;
				vector.y = 0f;
				float magnitude = vector.magnitude;
				if (float.IsNaN(magnitude) || float.IsInfinity(magnitude) || magnitude > Mathf.Max(0.5f, elapsed * 20f))
				{
					this._hasStepBaseline = false;
					this._movementElapsed = 0f;
				}
				else if (magnitude > 0.0001f)
				{
					AchievementTrackingService.AddTravel(magnitude);
					this.RecordTravelDay(magnitude, this._stepMotor.isRunning || this._stepMotor.isSprinting);
					if (this._stepMotor.isRunning || this._stepMotor.isSprinting)
					{
						this._runningMeters += magnitude;
						this._runningSeconds += this._movementElapsed;
					}
					else
					{
						this._walkingMeters += magnitude;
						this._walkingSeconds += this._movementElapsed;
					}
					this._movementElapsed = 0f;
				}
			}
			this._lastMovementPosition = position;
			this._hasMovementBaseline = true;
			if (!this._hasStepBaseline)
			{
				this._lastStepPosition = position;
				this._lastStepTime = Time.time;
				this._hasStepBaseline = true;
			}
		}

		// Token: 0x06007AF2 RID: 31474 RVA: 0x00323758 File Offset: 0x00321958
		private void SampleActiveGameplayClock()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			this._activeGameElapsed += Mathf.Min(Time.unscaledDeltaTime, 0.5f);
			if (this._activeGameElapsed < 0.25f)
			{
				return;
			}
			if (MatchStatsService.IsActivityClockPaused() || GameplayDurationPause.IsPaused || LongWayHomeRuntimeController.IsPreparing)
			{
				this._activeGameElapsed = 0f;
				return;
			}
			string name = SceneManager.GetActiveScene().name;
			if (name != "CielHome" && name != "Game" && !this.CanCollect)
			{
				this._activeGameElapsed = 0f;
				return;
			}
			this.FlushActiveGameplayClock();
		}

		// Token: 0x06007AF3 RID: 31475 RVA: 0x003237FA File Offset: 0x003219FA
		private void FlushActiveGameplayClock()
		{
			if (this._activeGameElapsed <= 0f)
			{
				return;
			}
			PlayerStatsService.AddActiveGameplaySeconds(this._activeGameElapsed);
			this._activeGameElapsed = 0f;
		}

		// Token: 0x06007AF4 RID: 31476 RVA: 0x00323820 File Offset: 0x00321A20
		private void OnApplicationPause(bool paused)
		{
			if (!Application.isPlaying || !paused)
			{
				return;
			}
			this.FlushActiveGameplayClock();
			PlayerStatsService.FlushActiveGameplayTime();
		}

		// Token: 0x06007AF5 RID: 31477 RVA: 0x00323838 File Offset: 0x00321A38
		private void OnApplicationQuit()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			this.FlushActiveGameplayClock();
			PlayerStatsService.FlushActiveGameplayTime();
		}

		// Token: 0x06007AF6 RID: 31478 RVA: 0x00323850 File Offset: 0x00321A50
		private static MatchStatsService.VehicleKind ClassifyVehicle(Object vehicle)
		{
			if (vehicle is SkateboardInteractable)
			{
				return MatchStatsService.VehicleKind.Board;
			}
			if (vehicle is SimpleRollerSkatesInteractable)
			{
				return MatchStatsService.VehicleKind.Skates;
			}
			vVehicleController vVehicleController = vehicle as vVehicleController;
			if (vVehicleController != null)
			{
				vVehicleController.VehicleType type = vVehicleController.Type;
				if (type - vVehicleController.VehicleType.Scooter <= 2)
				{
					return MatchStatsService.VehicleKind.Car;
				}
				if (type == vVehicleController.VehicleType.RollerSkates)
				{
					return MatchStatsService.VehicleKind.Skates;
				}
				return MatchStatsService.VehicleKind.Bike;
			}
			else
			{
				string name = vehicle.GetType().Name;
				if (name == "VehicleAI")
				{
					return MatchStatsService.VehicleKind.Roof;
				}
				if (!name.Contains("Bike") && !name.Contains("Cycle"))
				{
					return MatchStatsService.VehicleKind.Car;
				}
				return MatchStatsService.VehicleKind.Bike;
			}
		}

		// Token: 0x06007AF7 RID: 31479 RVA: 0x003238CC File Offset: 0x00321ACC
		private HealthDayData CurrentHealthDay()
		{
			string text = DateTime.Today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
			foreach (HealthDayData healthDayData in this._runHealthDays)
			{
				if (healthDayData.day == text)
				{
					return healthDayData;
				}
			}
			HealthDayData healthDayData2 = new HealthDayData
			{
				day = text
			};
			this._runHealthDays.Add(healthDayData2);
			return healthDayData2;
		}

		// Token: 0x06007AF8 RID: 31480 RVA: 0x00323964 File Offset: 0x00321B64
		private void RecordStepDay()
		{
			this._healthDaysSnapshot = null;
			HealthDayData healthDayData = this.CurrentHealthDay();
			if (healthDayData.steps < 2147483647)
			{
				healthDayData.steps++;
			}
		}

		// Token: 0x06007AF9 RID: 31481 RVA: 0x0032399C File Offset: 0x00321B9C
		private void RecordTravelDay(float meters, bool running)
		{
			this._healthDaysSnapshot = null;
			HealthDayData healthDayData = this.CurrentHealthDay();
			if (running)
			{
				healthDayData.runningMeters += meters;
				return;
			}
			healthDayData.walkingMeters += meters;
		}

		// Token: 0x06007AFA RID: 31482 RVA: 0x003239D8 File Offset: 0x00321BD8
		private HealthDayData[] SnapshotHealthDays()
		{
			if (this._healthDaysSnapshot != null)
			{
				return this._healthDaysSnapshot;
			}
			if (this._runHealthDays.Count == 0)
			{
				return this._healthDaysSnapshot = Array.Empty<HealthDayData>();
			}
			HealthDayData[] array = new HealthDayData[this._runHealthDays.Count];
			for (int i = 0; i < array.Length; i++)
			{
				HealthDayData healthDayData = this._runHealthDays[i];
				array[i] = new HealthDayData
				{
					day = healthDayData.day,
					steps = healthDayData.steps,
					walkingMeters = healthDayData.walkingMeters,
					runningMeters = healthDayData.runningMeters
				};
			}
			return this._healthDaysSnapshot = array;
		}

		// Token: 0x04007E0A RID: 32266
		[Header("Behavior")]
		[Tooltip("Quando true, GetSnapshot() ritorna i mock values qui sotto e i tracker reali NON vengono agganciati. Mettere a false per usare i sistemi di gioco reali.")]
		[SerializeField]
		private bool isMocked;

		// Token: 0x04007E0B RID: 32267
		[Header("Mock values (used while IsMocked = true)")]
		[SerializeField]
		private float mockDistanceMeters = 534f;

		// Token: 0x04007E0C RID: 32268
		[SerializeField]
		private float mockDistanceWithVehiclesMeters = 150f;

		// Token: 0x04007E0D RID: 32269
		[SerializeField]
		private int mockTimesSeen = 14;

		// Token: 0x04007E0E RID: 32270
		[SerializeField]
		private int mockTimesExhibitioned = 21;

		// Token: 0x04007E0F RID: 32271
		[SerializeField]
		private int mockLocationsVisited = 3;

		// Token: 0x04007E10 RID: 32272
		[SerializeField]
		private int mockAverageHeartbeatBpm = 96;

		// Token: 0x04007E11 RID: 32273
		[SerializeField]
		private int mockCarSurfCount = 2;

		// Token: 0x04007E12 RID: 32274
		[SerializeField]
		private int mockPoints = 16000;

		// Token: 0x04007E13 RID: 32275
		[Header("Live wiring (used while IsMocked = false)")]
		[Tooltip("Tracker della distanza sul player. Se null, viene cercato in scena all'avvio.")]
		[SerializeField]
		private DistanceTracker distanceTracker;

		// Token: 0x04007E14 RID: 32276
		[Tooltip("Controller del battito da cui campionare i BPM per la media. Se null, viene cercato in scena.")]
		[SerializeField]
		private HeartbeatController heartbeatController;

		// Token: 0x04007E15 RID: 32277
		[Tooltip("Context POI sul player per contare le location distinte. Se null, viene cercato in scena.")]
		[SerializeField]
		private PlayerPoiEmoteContext poiContext;

		// Token: 0x04007E16 RID: 32278
		[Tooltip("Controller emote sul player da cui contare le esecuzioni. Se null, viene cercato in scena.")]
		[SerializeField]
		private GameplayEmoteController emoteController;

		// Token: 0x04007E17 RID: 32279
		[Tooltip("Intervallo (s) di campionamento dei BPM per la media. 0.5s e' piu' che sufficiente.")]
		[SerializeField]
		private float bpmSampleInterval = 0.5f;

		// Token: 0x04007E18 RID: 32280
		private int _timesSeen;

		// Token: 0x04007E19 RID: 32281
		private int _timesExhibitioned;

		// Token: 0x04007E1A RID: 32282
		private float _vehicleDistanceMeters;

		// Token: 0x04007E1B RID: 32283
		private int _carSurfCount;

		// Token: 0x04007E1C RID: 32284
		private int _points;

		// Token: 0x04007E1D RID: 32285
		private float _peakDistanceMeters;

		// Token: 0x04007E1E RID: 32286
		private bool _homeReturnAwarded;

		// Token: 0x04007E1F RID: 32287
		private int _homeReturnBonus;

		// Token: 0x04007E20 RID: 32288
		private int _homeReturnMissionsCompleted;

		// Token: 0x04007E21 RID: 32289
		private float _homeReturnMissionXp;

		// Token: 0x04007E22 RID: 32290
		private MissionCompletion[] _homeReturnMissionRewards;

		// Token: 0x04007E23 RID: 32291
		private readonly HashSet<string> _visitedPoiIds = new HashSet<string>();

		// Token: 0x04007E24 RID: 32292
		private readonly List<RunScoreAward> _scoreAwards = new List<RunScoreAward>(32);

		// Token: 0x04007E25 RID: 32293
		private double _bpmSum;

		// Token: 0x04007E26 RID: 32294
		private int _bpmSampleCount;

		// Token: 0x04007E27 RID: 32295
		private float _nextBpmSampleTime;

		// Token: 0x04007E28 RID: 32296
		private PlayerPoiEmoteContext _boundPoiContext;

		// Token: 0x04007E29 RID: 32297
		private GameplayEmoteController _boundEmoteController;

		// Token: 0x04007E2A RID: 32298
		private GameOverManager _boundGameOver;

		// Token: 0x04007E2B RID: 32299
		private ExhibitionEventDirector _boundDirector;

		// Token: 0x04007E2C RID: 32300
		private bool _spottedSubscribed;

		// Token: 0x04007E2D RID: 32301
		private bool _vehicleSubscribed;

		// Token: 0x04007E2E RID: 32302
		private bool _lifetimeStatsCommitted;

		// Token: 0x04007E2F RID: 32303
		private bool _runTrackingActive;

		// Token: 0x04007E30 RID: 32304
		private bool _hasCompletedSnapshot;

		// Token: 0x04007E31 RID: 32305
		private MatchStatsSnapshot _completedSnapshot;

		// Token: 0x04007E32 RID: 32306
		private float _nextSourceBindTime;

		// Token: 0x04007E33 RID: 32307
		private bool _isRiding;

		// Token: 0x04007E34 RID: 32308
		private float _rideStartDistance;

		// Token: 0x04007E35 RID: 32309
		private Object _activeVehicle;

		// Token: 0x04007E36 RID: 32310
		private float _vehicleTravelMeters;

		// Token: 0x04007E37 RID: 32311
		private Vector3 _lastVehicleTravelPosition;

		// Token: 0x04007E38 RID: 32312
		private float _lastVehicleTravelTime;

		// Token: 0x04007E3D RID: 32317
		private MatchStatsService.VehicleKind _vehicleKind;

		// Token: 0x04007E3E RID: 32318
		private vThirdPersonController _stepMotor;

		// Token: 0x04007E3F RID: 32319
		private vThirdPersonInput _stepInput;

		// Token: 0x04007E40 RID: 32320
		private DistanceTracker _stepDistanceTracker;

		// Token: 0x04007E41 RID: 32321
		private FootstepController _footstepController;

		// Token: 0x04007E42 RID: 32322
		private vFootStepTrigger[] _footTriggers;

		// Token: 0x04007E43 RID: 32323
		private Transform _stepTransform;

		// Token: 0x04007E44 RID: 32324
		private Vector3 _lastStepPosition;

		// Token: 0x04007E45 RID: 32325
		private Vector3 _lastMovementPosition;

		// Token: 0x04007E46 RID: 32326
		private bool _hasMovementBaseline;

		// Token: 0x04007E47 RID: 32327
		private float _movementElapsed;

		// Token: 0x04007E48 RID: 32328
		private float _lastStepTime;

		// Token: 0x04007E49 RID: 32329
		private bool _hasStepBaseline;

		// Token: 0x04007E4A RID: 32330
		private float _activeGameElapsed;

		// Token: 0x04007E4B RID: 32331
		private float _playTimeSeconds;

		// Token: 0x04007E4C RID: 32332
		private float _walkingSeconds;

		// Token: 0x04007E4D RID: 32333
		private float _runningSeconds;

		// Token: 0x04007E4E RID: 32334
		private float _emoteSeconds;

		// Token: 0x04007E4F RID: 32335
		private float _vehicleSeconds;

		// Token: 0x04007E50 RID: 32336
		private float _carSeconds;

		// Token: 0x04007E51 RID: 32337
		private float _bikeSeconds;

		// Token: 0x04007E52 RID: 32338
		private float _boardSeconds;

		// Token: 0x04007E53 RID: 32339
		private float _skatesSeconds;

		// Token: 0x04007E54 RID: 32340
		private float _roofSeconds;

		// Token: 0x04007E55 RID: 32341
		private float _walkingMeters;

		// Token: 0x04007E56 RID: 32342
		private float _runningMeters;

		// Token: 0x04007E57 RID: 32343
		private int _steps;

		// Token: 0x04007E58 RID: 32344
		private readonly List<HealthDayData> _runHealthDays = new List<HealthDayData>();

		// Token: 0x04007E59 RID: 32345
		private HealthDayData[] _healthDaysSnapshot;

		// Token: 0x02000FDE RID: 4062
		private enum VehicleKind
		{
			// Token: 0x04007E5B RID: 32347
			None,
			// Token: 0x04007E5C RID: 32348
			Car,
			// Token: 0x04007E5D RID: 32349
			Bike,
			// Token: 0x04007E5E RID: 32350
			Board,
			// Token: 0x04007E5F RID: 32351
			Skates,
			// Token: 0x04007E60 RID: 32352
			Roof
		}
	}
}
