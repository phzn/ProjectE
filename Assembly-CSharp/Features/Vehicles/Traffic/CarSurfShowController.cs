using System;
using Features.Detection;
using Features.MatchRecap;
using Features.PlayerEmotes;
using Features.Score;
using UnityEngine;
using VehicleSpawnPlugin;

namespace Features.Vehicles.Traffic
{
	// Token: 0x02000B9C RID: 2972
	[DefaultExecutionOrder(-80)]
	public class CarSurfShowController : MonoBehaviour
	{
		// Token: 0x17000E3D RID: 3645
		// (get) Token: 0x0600553D RID: 21821 RVA: 0x00211534 File Offset: 0x0020F734
		// (set) Token: 0x0600553E RID: 21822 RVA: 0x0021153B File Offset: 0x0020F73B
		public static CarSurfShowController Instance { get; private set; }

		// Token: 0x0600553F RID: 21823 RVA: 0x00211543 File Offset: 0x0020F743
		public void ApplyWorldOriginShift(Vector3 delta)
		{
			this._lastVehiclePosition += delta;
		}

		// Token: 0x06005540 RID: 21824 RVA: 0x00211557 File Offset: 0x0020F757
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void Bootstrap()
		{
			CarRoofCarrier.OnPlayerMounted -= CarSurfShowController.StaticOnMounted;
			CarRoofCarrier.OnPlayerMounted += CarSurfShowController.StaticOnMounted;
		}

		// Token: 0x06005541 RID: 21825 RVA: 0x0021157B File Offset: 0x0020F77B
		private static void StaticOnMounted(VehicleAI vehicle, Transform rider)
		{
			CarSurfShowController.EnsureInstance();
			CarSurfShowController.Instance.HandleMounted(vehicle, rider);
		}

		// Token: 0x06005542 RID: 21826 RVA: 0x0021158E File Offset: 0x0020F78E
		private static void EnsureInstance()
		{
			if (CarSurfShowController.Instance != null)
			{
				return;
			}
			CarSurfShowController.Instance = new GameObject("CarSurfShowController (auto)").AddComponent<CarSurfShowController>();
		}

		// Token: 0x06005543 RID: 21827 RVA: 0x002115B2 File Offset: 0x0020F7B2
		private void Awake()
		{
			if (CarSurfShowController.Instance != null && CarSurfShowController.Instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			CarSurfShowController.Instance = this;
		}

		// Token: 0x06005544 RID: 21828 RVA: 0x002115E0 File Offset: 0x0020F7E0
		private void OnEnable()
		{
			CarRoofCarrier.OnPlayerDismounted += this.HandleDismounted;
			CarRideLimiter.OnRideLimitReached += this.HandleRideLimitReached;
		}

		// Token: 0x06005545 RID: 21829 RVA: 0x00211604 File Offset: 0x0020F804
		private void OnDisable()
		{
			CarRoofCarrier.OnPlayerDismounted -= this.HandleDismounted;
			CarRideLimiter.OnRideLimitReached -= this.HandleRideLimitReached;
			this.UnbindEmotes();
			this.ClearBoost();
		}

		// Token: 0x06005546 RID: 21830 RVA: 0x00211634 File Offset: 0x0020F834
		private void HandleMounted(VehicleAI vehicle, Transform rider)
		{
			this._ridingVehicle = vehicle;
			this._rider = rider;
			this._riderStealth = CarSurfShowController.ResolveStealth(rider);
			this._validThisRide = false;
			this._roofEmotePerformed = false;
			this._roofEmoteAwarded = false;
			this._fullRideAwarded = false;
			this._movingExposedSeconds = 0f;
			this._movingExposedMeters = 0f;
			this._lastVehiclePosition = ((vehicle != null) ? vehicle.transform.position : Vector3.zero);
			this._nextExposureCheck = 0f;
			this._rideSequence++;
			this.BindEmotes(rider);
		}

		// Token: 0x06005547 RID: 21831 RVA: 0x002116D0 File Offset: 0x0020F8D0
		private void HandleDismounted(VehicleAI vehicle, Transform rider)
		{
			if (rider != this._rider && this._rider != null)
			{
				return;
			}
			this.ClearBoost();
			this.UnbindEmotes();
			this._ridingVehicle = null;
			this._rider = null;
			this._riderStealth = null;
			this._validThisRide = false;
		}

		// Token: 0x06005548 RID: 21832 RVA: 0x00211724 File Offset: 0x0020F924
		private static StealthActor ResolveStealth(Transform rider)
		{
			StealthActor stealthActor = ((rider != null) ? rider.GetComponentInChildren<StealthActor>() : null);
			if (stealthActor == null && SensoryManager.Instance != null)
			{
				stealthActor = SensoryManager.Instance.GetPlayer();
			}
			return stealthActor;
		}

		// Token: 0x06005549 RID: 21833 RVA: 0x00211768 File Offset: 0x0020F968
		private void Update()
		{
			if (this._rider == null)
			{
				return;
			}
			bool isExposed = PlayerExposureState.IsExposed;
			if (Time.time >= this._nextExposureCheck)
			{
				this._nextExposureCheck = Time.time + 0.4f;
				if (isExposed)
				{
					this.ApplyShow();
				}
				else
				{
					this.ClearBoost();
				}
			}
			if (this._ridingVehicle == null)
			{
				return;
			}
			Vector3 position = this._ridingVehicle.transform.position;
			float num = Vector3.Distance(position, this._lastVehiclePosition);
			this._lastVehiclePosition = position;
			if (!isExposed || VehicleSimulationPause.IsPaused)
			{
				return;
			}
			if (this._ridingVehicle.CurrentSpeed <= 0.5f && (num <= 0.02f || num >= 8f))
			{
				return;
			}
			this._movingExposedSeconds += Time.deltaTime;
			if (num < 8f)
			{
				this._movingExposedMeters += num;
			}
			if (!this._validThisRide && (this._movingExposedSeconds >= 4f || this._movingExposedMeters >= 30f))
			{
				this.ValidateRide();
			}
		}

		// Token: 0x0600554A RID: 21834 RVA: 0x00211874 File Offset: 0x0020FA74
		private void ApplyShow()
		{
			if (this._activeModifier != null || this._riderStealth == null)
			{
				return;
			}
			this._activeModifier = new StealthModifier
			{
				SourceName = "CarSurfExposed",
				VisibilityMultiplier = 2f,
				NoiseMultiplier = 1f
			};
			this._riderStealth.AddModifier(this._activeModifier);
		}

		// Token: 0x0600554B RID: 21835 RVA: 0x002118D8 File Offset: 0x0020FAD8
		private void ValidateRide()
		{
			MatchStatsService instance = MatchStatsService.Instance;
			RunScoreAward runScoreAward;
			if (instance == null || !instance.AwardPoints(new RunScoreAwardRequest(RunScoreReason.CarSurf, 500, "CAR SURF", string.Empty, this.RideGroupKey, RunScorePresentation.Generic), out runScoreAward))
			{
				return;
			}
			this._validThisRide = true;
			instance.RegisterCarSurf();
			this.TryAwardRoofEmote();
		}

		// Token: 0x0600554C RID: 21836 RVA: 0x00211930 File Offset: 0x0020FB30
		private void BindEmotes(Transform rider)
		{
			this.UnbindEmotes();
			if (rider != null)
			{
				this._emotes = rider.GetComponentInChildren<GameplayEmoteController>();
			}
			if (this._emotes == null)
			{
				this._emotes = Object.FindObjectOfType<GameplayEmoteController>();
			}
			if (this._emotes != null)
			{
				this._emotes.OnEmoteStarted += this.HandleEmoteStarted;
			}
		}

		// Token: 0x0600554D RID: 21837 RVA: 0x00211996 File Offset: 0x0020FB96
		private void UnbindEmotes()
		{
			if (this._emotes != null)
			{
				this._emotes.OnEmoteStarted -= this.HandleEmoteStarted;
			}
			this._emotes = null;
		}

		// Token: 0x0600554E RID: 21838 RVA: 0x002119C4 File Offset: 0x0020FBC4
		private void HandleEmoteStarted(EmoteDefinitionSO emote)
		{
			if (this._rider == null || !PlayerExposureState.IsExposed)
			{
				return;
			}
			this._roofEmotePerformed = true;
			this.TryAwardRoofEmote();
		}

		// Token: 0x0600554F RID: 21839 RVA: 0x002119EC File Offset: 0x0020FBEC
		private void TryAwardRoofEmote()
		{
			if (!this._validThisRide || !this._roofEmotePerformed || this._roofEmoteAwarded)
			{
				return;
			}
			MatchStatsService instance = MatchStatsService.Instance;
			RunScoreAward runScoreAward;
			if (instance != null && instance.AwardPoints(new RunScoreAwardRequest(RunScoreReason.CarSurfEmote, 200, "CAR SURF", "ROOF EMOTE", this.RideGroupKey, RunScorePresentation.Generic), out runScoreAward))
			{
				this._roofEmoteAwarded = true;
			}
		}

		// Token: 0x06005550 RID: 21840 RVA: 0x00211A50 File Offset: 0x0020FC50
		private void HandleRideLimitReached(VehicleAI vehicle)
		{
			if (vehicle != this._ridingVehicle || !this._validThisRide || this._fullRideAwarded)
			{
				return;
			}
			MatchStatsService instance = MatchStatsService.Instance;
			RunScoreAward runScoreAward;
			if (instance != null && instance.AwardPoints(new RunScoreAwardRequest(RunScoreReason.CarSurfFullRide, 250, "CAR SURF", "FULL RIDE", this.RideGroupKey, RunScorePresentation.Generic), out runScoreAward))
			{
				this._fullRideAwarded = true;
			}
		}

		// Token: 0x17000E3E RID: 3646
		// (get) Token: 0x06005551 RID: 21841 RVA: 0x00211AB8 File Offset: 0x0020FCB8
		private string RideGroupKey
		{
			get
			{
				return "car-surf:" + this._rideSequence.ToString();
			}
		}

		// Token: 0x06005552 RID: 21842 RVA: 0x00211ACF File Offset: 0x0020FCCF
		private void ClearBoost()
		{
			if (this._activeModifier != null && this._riderStealth != null)
			{
				this._riderStealth.RemoveModifier(this._activeModifier);
			}
			this._activeModifier = null;
		}

		// Token: 0x06005553 RID: 21843 RVA: 0x00211AFF File Offset: 0x0020FCFF
		private void OnDestroy()
		{
			if (CarSurfShowController.Instance == this)
			{
				CarSurfShowController.Instance = null;
			}
		}

		// Token: 0x0400555E RID: 21854
		private const string ModifierSource = "CarSurfExposed";

		// Token: 0x0400555F RID: 21855
		private const float VisibilityBoost = 2f;

		// Token: 0x04005560 RID: 21856
		private const float ExposureCheckInterval = 0.4f;

		// Token: 0x04005561 RID: 21857
		private const float ValidRideSeconds = 4f;

		// Token: 0x04005562 RID: 21858
		private const float ValidRideMeters = 30f;

		// Token: 0x04005564 RID: 21860
		private VehicleAI _ridingVehicle;

		// Token: 0x04005565 RID: 21861
		private Transform _rider;

		// Token: 0x04005566 RID: 21862
		private StealthActor _riderStealth;

		// Token: 0x04005567 RID: 21863
		private StealthModifier _activeModifier;

		// Token: 0x04005568 RID: 21864
		private GameplayEmoteController _emotes;

		// Token: 0x04005569 RID: 21865
		private bool _validThisRide;

		// Token: 0x0400556A RID: 21866
		private bool _roofEmotePerformed;

		// Token: 0x0400556B RID: 21867
		private bool _roofEmoteAwarded;

		// Token: 0x0400556C RID: 21868
		private bool _fullRideAwarded;

		// Token: 0x0400556D RID: 21869
		private float _movingExposedSeconds;

		// Token: 0x0400556E RID: 21870
		private float _movingExposedMeters;

		// Token: 0x0400556F RID: 21871
		private float _nextExposureCheck;

		// Token: 0x04005570 RID: 21872
		private Vector3 _lastVehiclePosition;

		// Token: 0x04005571 RID: 21873
		private int _rideSequence;
	}
}
