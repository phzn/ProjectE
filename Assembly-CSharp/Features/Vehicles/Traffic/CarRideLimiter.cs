using System;
using System.Collections;
using Features.CarHide;
using Features.NPC;
using Features.NPC.Audio;
using Features.RunModes;
using Features.Suspicion;
using Pathfinding;
using ProjectExhibitionist.Core.SceneManagement;
using UnityEngine;
using VehicleSpawnPlugin;

namespace Features.Vehicles.Traffic
{
	// Token: 0x02000B92 RID: 2962
	[DisallowMultipleComponent]
	public class CarRideLimiter : MonoBehaviour
	{
		// Token: 0x1400008C RID: 140
		// (add) Token: 0x060054BC RID: 21692 RVA: 0x0020E6D0 File Offset: 0x0020C8D0
		// (remove) Token: 0x060054BD RID: 21693 RVA: 0x0020E704 File Offset: 0x0020C904
		public static event Action<VehicleAI> OnRideLimitReached;

		// Token: 0x17000E23 RID: 3619
		// (get) Token: 0x060054BE RID: 21694 RVA: 0x0020E737 File Offset: 0x0020C937
		public bool IsConfronting
		{
			get
			{
				return this._sequence != null;
			}
		}

		// Token: 0x17000E24 RID: 3620
		// (get) Token: 0x060054BF RID: 21695 RVA: 0x0020E742 File Offset: 0x0020C942
		public bool HasRoofRider
		{
			get
			{
				return this._carrier != null && this._carrier.HasRider;
			}
		}

		// Token: 0x17000E25 RID: 3621
		// (get) Token: 0x060054C0 RID: 21696 RVA: 0x0020E75F File Offset: 0x0020C95F
		public bool IsAbandoned
		{
			get
			{
				return this._abandoned;
			}
		}

		// Token: 0x17000E26 RID: 3622
		// (get) Token: 0x060054C1 RID: 21697 RVA: 0x0020E767 File Offset: 0x0020C967
		public GameObject DriverNpc
		{
			get
			{
				return this._driverNpc;
			}
		}

		// Token: 0x17000E27 RID: 3623
		// (get) Token: 0x060054C2 RID: 21698 RVA: 0x0020E76F File Offset: 0x0020C96F
		public VehicleAI Vehicle
		{
			get
			{
				return this._vehicle;
			}
		}

		// Token: 0x17000E28 RID: 3624
		// (get) Token: 0x060054C3 RID: 21699 RVA: 0x0020E777 File Offset: 0x0020C977
		private DriverPersonalityProfileSO Profile
		{
			get
			{
				if (this.personalityProfile != null)
				{
					return this.personalityProfile;
				}
				if (CarRideLimiter.s_defaultProfile == null)
				{
					CarRideLimiter.s_defaultProfile = ScriptableObject.CreateInstance<DriverPersonalityProfileSO>();
					CarRideLimiter.s_defaultProfile.hideFlags = HideFlags.HideAndDontSave;
				}
				return CarRideLimiter.s_defaultProfile;
			}
		}

		// Token: 0x060054C4 RID: 21700 RVA: 0x0020E7B8 File Offset: 0x0020C9B8
		private void Awake()
		{
			this._vehicle = base.GetComponent<VehicleAI>();
			if (this._vehicle == null)
			{
				this._vehicle = base.GetComponentInParent<VehicleAI>();
			}
			this._carrier = base.GetComponentInChildren<CarRoofCarrier>(true);
			this._carHide = base.GetComponentInChildren<CarHideUnderInteractable>(true);
			this._driverReaction = base.GetComponent<DriverReactionController>();
			foreach (Transform transform in base.GetComponentsInChildren<Transform>(true))
			{
				if (transform.name == "DriverSpawnAnchor")
				{
					this._driverSpawnAnchor = transform;
					return;
				}
			}
		}

		// Token: 0x060054C5 RID: 21701 RVA: 0x0020E845 File Offset: 0x0020CA45
		private void OnEnable()
		{
			CarRoofCarrier.OnPlayerMounted += this.HandleMounted;
			CarRoofCarrier.OnPlayerDismounted += this.HandleDismounted;
		}

		// Token: 0x060054C6 RID: 21702 RVA: 0x0020E86C File Offset: 0x0020CA6C
		private void OnDisable()
		{
			CarRoofCarrier.OnPlayerMounted -= this.HandleMounted;
			CarRoofCarrier.OnPlayerDismounted -= this.HandleDismounted;
			if (this._sequence != null)
			{
				base.StopCoroutine(this._sequence);
				this._sequence = null;
			}
			this.CleanupConfrontation(true);
			this.RestoreCowardBoost();
			this._rideTimeRemaining = -1f;
			this._abandoned = false;
			this._wasHiddenUnder = false;
			this._player = null;
			this._playerResolveFrame = 0;
		}

		// Token: 0x060054C7 RID: 21703 RVA: 0x0020E8EC File Offset: 0x0020CAEC
		private void HandleMounted(VehicleAI vehicle, Transform rider)
		{
			if (vehicle != this._vehicle)
			{
				return;
			}
			this._player = rider;
			if (this._abandoned)
			{
				return;
			}
			if (this._sequence == null)
			{
				this._rideTimeRemaining = this.maxRideDuration;
				if (this.debugLogs)
				{
					Debug.Log(string.Format("[CarRideLimiter] Timer armed ({0:0}s) on '{1}'.", this.maxRideDuration, base.name), this);
				}
			}
		}

		// Token: 0x060054C8 RID: 21704 RVA: 0x0020E955 File Offset: 0x0020CB55
		private void HandleDismounted(VehicleAI vehicle, Transform rider)
		{
			if (vehicle != this._vehicle)
			{
				return;
			}
			if (this._sequence == null && (this._carHide == null || !this._carHide.IsBusy))
			{
				this._rideTimeRemaining = -1f;
			}
		}

		// Token: 0x060054C9 RID: 21705 RVA: 0x0020E994 File Offset: 0x0020CB94
		public void NotifyHiddenRider(Transform rider)
		{
			this._player = rider;
			this._wasHiddenUnder = true;
			if (this._sequence != null || this._abandoned || this._vehicle == null || !this._vehicle.isActiveAndEnabled || this._vehicle.CurrentTrack == null)
			{
				return;
			}
			this._rideTimeRemaining = Mathf.Max(0.1f, this.maxRideDuration);
		}

		// Token: 0x060054CA RID: 21706 RVA: 0x0020EA04 File Offset: 0x0020CC04
		private void Update()
		{
			if (VehicleSimulationPause.IsPaused || RunModeService.UnlimitedVehicleDuration)
			{
				return;
			}
			if (this._sequence != null || this._abandoned)
			{
				return;
			}
			bool flag = this._carHide != null && this._carHide.IsPlayerHiddenUnder;
			if (flag != this._wasHiddenUnder)
			{
				this._wasHiddenUnder = flag;
				if (!flag && (this._carrier == null || !this._carrier.HasRider))
				{
					this._rideTimeRemaining = -1f;
				}
			}
			if (flag && this._rideTimeRemaining < 0f && this._vehicle != null && this._vehicle.isActiveAndEnabled && (this._vehicle.CurrentTrack != null || (this._carHide.CurrentTravelSpeed < float.PositiveInfinity && this._carHide.CurrentTravelSpeed > 0.5f)) && this.TryResolvePlayer())
			{
				this._rideTimeRemaining = Mathf.Max(0.1f, this.maxRideDuration);
				if (this.debugLogs)
				{
					Debug.Log(string.Format("[CarRideLimiter] Timer armed (hide-under, {0:0}s) on '{1}'.", this.maxRideDuration, base.name), this);
				}
			}
			if (this._rideTimeRemaining < 0f)
			{
				return;
			}
			this._rideTimeRemaining -= Time.deltaTime;
			if (this._rideTimeRemaining > 0f)
			{
				return;
			}
			this._rideTimeRemaining = -1f;
			this._sequence = base.StartCoroutine(this.ConfrontationRoutine());
		}

		// Token: 0x060054CB RID: 21707 RVA: 0x0020EB84 File Offset: 0x0020CD84
		private bool TryResolvePlayer()
		{
			if (this._player != null)
			{
				return true;
			}
			this._player = PlayerRegistry.Player;
			if (this._player != null)
			{
				return true;
			}
			if (Time.frameCount < this._playerResolveFrame)
			{
				return false;
			}
			this._playerResolveFrame = Time.frameCount + 90;
			GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
			if (gameObject != null)
			{
				this._player = gameObject.transform;
			}
			return this._player != null;
		}

		// Token: 0x060054CC RID: 21708 RVA: 0x0020EC08 File Offset: 0x0020CE08
		public void DebugForceTimeout()
		{
			if (this._sequence != null)
			{
				Debug.LogWarning("[CarRideLimiter] DebugForceTimeout: sequence already running.", this);
				return;
			}
			if (this._abandoned)
			{
				Debug.LogWarning("[CarRideLimiter] DebugForceTimeout: car is abandoned.", this);
				return;
			}
			RunModeService.DisqualifyLeaderboardRun("A vehicle encounter was forced with a debug control.");
			this.TryResolvePlayer();
			this._rideTimeRemaining = -1f;
			this._sequence = base.StartCoroutine(this.ConfrontationRoutine());
		}

		// Token: 0x060054CD RID: 21709 RVA: 0x0020EC6B File Offset: 0x0020CE6B
		public void DebugForceTimeout(DriverPersonality personality)
		{
			RunModeService.DisqualifyLeaderboardRun("A driver personality was overridden with a debug control.");
			this.debugForcePersonality = true;
			this.debugPersonality = personality;
			this.DebugForceTimeout();
		}

		// Token: 0x060054CE RID: 21710 RVA: 0x0020EC8B File Offset: 0x0020CE8B
		private DriverPersonality ResolvePersonality()
		{
			if (this.debugForcePersonality)
			{
				RunModeService.DisqualifyLeaderboardRun("A driver personality was overridden with a debug control.");
				return this.debugPersonality;
			}
			if (!(this.personalityProfile != null))
			{
				return DriverPersonality.Angry;
			}
			return this.personalityProfile.Pick();
		}

		// Token: 0x060054CF RID: 21711 RVA: 0x0020ECC1 File Offset: 0x0020CEC1
		private IEnumerator ConfrontationRoutine()
		{
			yield return CarRideLimiter.WaitForVehicleSimulationResume();
			DriverPersonality personality = this.ResolvePersonality();
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[CarRideLimiter] Ride limit hit on '{0}' — confrontation starts (personality={1}).", base.name, personality), this);
			}
			if (this._vehicle != null)
			{
				this._vehicle.HasPassenger = true;
				this._passengerFlagByUs = true;
			}
			if (this.doubleHornClip != null)
			{
				this.PlayHornClip(this.doubleHornClip);
				yield return CarRideLimiter.WaitForVehicleSimulationSeconds(this.honkGap * 2f);
			}
			else
			{
				this.Honk();
				yield return CarRideLimiter.WaitForVehicleSimulationSeconds(this.honkGap);
				this.Honk();
			}
			if (this._vehicle != null)
			{
				this._vehicle.SetInteractionBlocked(this, true);
				this._blockedByUs = true;
			}
			float stopTimeRemaining = Mathf.Max(0f, this.stopWaitTimeout);
			while (this._vehicle != null && this._vehicle.CurrentSpeed > 0.3f && stopTimeRemaining > 0f)
			{
				if (VehicleSimulationPause.IsPaused)
				{
					yield return null;
				}
				else
				{
					stopTimeRemaining -= Time.deltaTime;
					yield return null;
				}
			}
			yield return CarRideLimiter.WaitForVehicleSimulationResume();
			if (this._vehicle != null && this._vehicle.CurrentSpeed > 0.3f)
			{
				this._vehicle.SetInitialSpeed(0f);
			}
			if (this._carrier != null && this._carrier.HasRider)
			{
				Action<VehicleAI> onRideLimitReached = CarRideLimiter.OnRideLimitReached;
				if (onRideLimitReached != null)
				{
					onRideLimitReached(this._vehicle);
				}
			}
			if (this._carHide != null && this._carHide.IsPlayerHiddenUnder)
			{
				if (this.debugLogs)
				{
					Debug.Log("[CarRideLimiter] Player hidden under the car — forcing exit.", this);
				}
				this._carHide.ForceExit();
			}
			while (this._carHide != null && this._carHide.IsBusy)
			{
				yield return CarRideLimiter.WaitForVehicleSimulationResume();
				if (this._carHide.IsPlayerHiddenUnder)
				{
					this._carHide.ForceExit();
				}
				yield return null;
			}
			switch (personality)
			{
			case DriverPersonality.Scared:
				yield return this.ScaredRoutine();
				break;
			case DriverPersonality.Coward:
				yield return this.CowardRoutine();
				break;
			case DriverPersonality.Gawker:
				yield return this.DriverOutRoutine(personality);
				break;
			case DriverPersonality.CallPolice:
				yield return this.DriverOutRoutine(personality);
				break;
			default:
				yield return this.DriverOutRoutine(DriverPersonality.Angry);
				break;
			}
			this._sequence = null;
			yield break;
		}

		// Token: 0x060054D0 RID: 21712 RVA: 0x0020ECD0 File Offset: 0x0020CED0
		private IEnumerator DriverOutRoutine(DriverPersonality personality)
		{
			yield return CarRideLimiter.WaitForVehicleSimulationResume();
			this.SpawnDriver();
			int face = ((personality == DriverPersonality.Gawker) ? 1 : 2);
			yield return CarRideLimiter.WaitForVehicleSimulationFrames(2);
			this.ApplyDriverFace(face);
			yield return CarRideLimiter.WaitForVehicleSimulationSeconds(0.5f);
			this.ApplyDriverFace(face);
			DriverPersonalityProfileSO profile = this.Profile;
			float suspicionTimeRemaining = -1f;
			if (this._driverNpc != null && personality == DriverPersonality.Gawker)
			{
				this.StartDriverPhoneReaction(1, profile.PhonePropName);
				yield return CarRideLimiter.WaitForVehicleSimulationSeconds(Mathf.Max(1f, profile.PhotoSeconds));
				this.EndDriverPhoneReaction(profile.PhonePropName);
			}
			else if (this._driverNpc != null && personality == DriverPersonality.CallPolice)
			{
				this.StartDriverPhoneReaction(2, profile.PhonePropName);
				if (SuspicionManager.Instance != null)
				{
					SuspicionManager.Instance.AddSuspicion(profile.SuspicionSeed, null);
				}
				suspicionTimeRemaining = profile.SuspicionRepeatInterval;
				if (this.debugLogs)
				{
					Debug.Log(string.Format("[CarRideLimiter] CallPolice driver seeded suspicion +{0}.", profile.SuspicionSeed), this);
				}
			}
			float awayDuration = 0f;
			for (;;)
			{
				if (VehicleSimulationPause.IsPaused)
				{
					yield return null;
				}
				else
				{
					float deltaTime = Time.deltaTime;
					if (suspicionTimeRemaining > 0f)
					{
						suspicionTimeRemaining -= deltaTime;
						if (suspicionTimeRemaining <= 0f)
						{
							suspicionTimeRemaining = profile.SuspicionRepeatInterval;
							if (SuspicionManager.Instance != null && this._player != null && this._driverNpc != null && (this._player.position - this._driverNpc.transform.position).sqrMagnitude <= profile.SuspicionRepeatRange * profile.SuspicionRepeatRange)
							{
								SuspicionManager.Instance.AddSuspicion(profile.SuspicionRepeatDelta, null);
							}
						}
					}
					if (this.IsPlayerAway(this.leaveDistance))
					{
						awayDuration += deltaTime;
						if (awayDuration >= this.leaveHoldSeconds)
						{
							break;
						}
					}
					else
					{
						awayDuration = 0f;
					}
					yield return null;
				}
			}
			if (personality == DriverPersonality.CallPolice)
			{
				this.EndDriverPhoneReaction(profile.PhonePropName);
			}
			GameObject departingDriver = this._driverNpc;
			this.DespawnDriver(false);
			float driverGoneTimeRemaining = 4f;
			while (departingDriver != null && departingDriver.activeInHierarchy && driverGoneTimeRemaining > 0f)
			{
				if (VehicleSimulationPause.IsPaused)
				{
					yield return null;
				}
				else
				{
					driverGoneTimeRemaining -= Time.deltaTime;
					yield return null;
				}
			}
			yield return CarRideLimiter.WaitForVehicleSimulationResume();
			this.ReleaseVehicle();
			if (this._carrier != null)
			{
				this._carrier.LockRemount(this.remountLockSeconds);
			}
			yield break;
		}

		// Token: 0x060054D1 RID: 21713 RVA: 0x0020ECE6 File Offset: 0x0020CEE6
		private IEnumerator ScaredRoutine()
		{
			yield return CarRideLimiter.WaitForVehicleSimulationResume();
			this.SpawnDriver();
			yield return CarRideLimiter.WaitForVehicleSimulationFrames(2);
			this.ApplyDriverFace(5);
			DriverPersonalityProfileSO profile = this.Profile;
			this.PlayScream(profile);
			yield return CarRideLimiter.WaitForVehicleSimulationSeconds(0.9f);
			this.ApplyDriverFace(5);
			if (this._driverNpc != null)
			{
				Vector3 position = this._driverNpc.transform.position;
				Vector3 vector = ((this._player != null) ? (position - this._player.position) : (-base.transform.forward));
				vector.y = 0f;
				if (vector.sqrMagnitude < 0.001f)
				{
					vector = -base.transform.forward;
				}
				Vector3 vector2 = position + vector.normalized * Mathf.Max(10f, profile.FleeDistance);
				ScaredDriverFleeRunner.Begin(this._driverNpc, CarRideLimiter.s_spawner, vector2, profile.FleeSpeed, profile.FleeDespawnSeconds);
				this._driverNpc = null;
				if (this.debugLogs)
				{
					Debug.Log(string.Format("[CarRideLimiter] Scared driver flees to {0} — car abandoned.", vector2), this);
				}
			}
			this.AbandonCar();
			yield break;
		}

		// Token: 0x060054D2 RID: 21714 RVA: 0x0020ECF5 File Offset: 0x0020CEF5
		private IEnumerator CowardRoutine()
		{
			yield return CarRideLimiter.WaitForVehicleSimulationResume();
			DriverPersonalityProfileSO profile = this.Profile;
			float awayDuration = 0f;
			for (;;)
			{
				if (VehicleSimulationPause.IsPaused)
				{
					yield return null;
				}
				else
				{
					if (this.IsPlayerAway(Mathf.Max(1f, profile.CowardLeaveDistance)))
					{
						awayDuration += Time.deltaTime;
						if (awayDuration >= 0.5f)
						{
							break;
						}
					}
					else
					{
						awayDuration = 0f;
					}
					yield return null;
				}
			}
			this.Honk();
			if (this._vehicle != null && this._accelRestore < 0f)
			{
				this._accelRestore = this._vehicle.maxAcceleration;
				this._vehicle.maxAcceleration = Mathf.Max(this._accelRestore, profile.CowardBoostAcceleration);
				base.StartCoroutine(this.RestoreCowardBoostAfter(Mathf.Max(0.5f, profile.CowardBoostSeconds)));
			}
			this.ReleaseVehicle();
			if (this._carrier != null)
			{
				this._carrier.LockRemount(this.remountLockSeconds);
			}
			yield break;
		}

		// Token: 0x060054D3 RID: 21715 RVA: 0x0020ED04 File Offset: 0x0020CF04
		private IEnumerator RestoreCowardBoostAfter(float seconds)
		{
			yield return CarRideLimiter.WaitForVehicleSimulationSeconds(seconds);
			this.RestoreCowardBoost();
			yield break;
		}

		// Token: 0x060054D4 RID: 21716 RVA: 0x0020ED1A File Offset: 0x0020CF1A
		private static IEnumerator WaitForVehicleSimulationResume()
		{
			while (VehicleSimulationPause.IsPaused)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x060054D5 RID: 21717 RVA: 0x0020ED22 File Offset: 0x0020CF22
		private static IEnumerator WaitForVehicleSimulationSeconds(float seconds)
		{
			float remaining = Mathf.Max(0f, seconds);
			while (remaining > 0f || VehicleSimulationPause.IsPaused)
			{
				yield return null;
				if (!VehicleSimulationPause.IsPaused)
				{
					remaining -= Time.deltaTime;
				}
			}
			yield break;
		}

		// Token: 0x060054D6 RID: 21718 RVA: 0x0020ED31 File Offset: 0x0020CF31
		private static IEnumerator WaitForVehicleSimulationFrames(int frameCount)
		{
			int remaining = Mathf.Max(0, frameCount);
			while (remaining > 0 || VehicleSimulationPause.IsPaused)
			{
				yield return null;
				if (!VehicleSimulationPause.IsPaused && remaining > 0)
				{
					int num = remaining;
					remaining = num - 1;
				}
			}
			yield break;
		}

		// Token: 0x060054D7 RID: 21719 RVA: 0x0020ED40 File Offset: 0x0020CF40
		private void RestoreCowardBoost()
		{
			if (this._accelRestore >= 0f && this._vehicle != null)
			{
				this._vehicle.maxAcceleration = this._accelRestore;
			}
			this._accelRestore = -1f;
		}

		// Token: 0x060054D8 RID: 21720 RVA: 0x0020ED7C File Offset: 0x0020CF7C
		private bool IsPlayerAway(float distance)
		{
			return (this._carrier == null || !this._carrier.HasRider) && (this._carHide == null || !this._carHide.IsBusy) && this._player != null && (this._player.position - base.transform.position).sqrMagnitude > distance * distance;
		}

		// Token: 0x060054D9 RID: 21721 RVA: 0x0020EDF8 File Offset: 0x0020CFF8
		private void AbandonCar()
		{
			this._abandoned = true;
			if (this._passengerFlagByUs && this._vehicle != null)
			{
				this._vehicle.HasPassenger = this._carrier != null && this._carrier.HasRider;
				this._passengerFlagByUs = false;
			}
			this._blockedByUs = false;
			if (this._vehicle != null)
			{
				this._vehicle.SetAbandoned(true);
				this._vehicle.SetInteractionBlocked(this, false);
			}
			if (CarRideLimiter.s_vehicleSpawner == null)
			{
				CarRideLimiter.s_vehicleSpawner = Object.FindObjectOfType<VehicleSpawner>();
			}
			if (CarRideLimiter.s_vehicleSpawner != null && this._vehicle != null)
			{
				CarRideLimiter.s_vehicleSpawner.AbandonUntilUnseen(this._vehicle.gameObject);
				return;
			}
			if (this.debugLogs)
			{
				Debug.LogWarning("[CarRideLimiter] No VehicleSpawner found — abandoned car will only be reclaimed by chunk unload.", this);
			}
		}

		// Token: 0x060054DA RID: 21722 RVA: 0x0020EED9 File Offset: 0x0020D0D9
		private void CleanupConfrontation(bool hardDespawn)
		{
			this.DespawnDriver(hardDespawn);
			this.ReleaseVehicle();
		}

		// Token: 0x060054DB RID: 21723 RVA: 0x0020EEE8 File Offset: 0x0020D0E8
		private void DespawnDriver(bool hard)
		{
			if (this._driverNpc == null)
			{
				return;
			}
			if (CarRideLimiter.s_spawner != null)
			{
				CarRideLimiter.s_spawner.DespawnNPC(this._driverNpc, hard);
			}
			else
			{
				Object.Destroy(this._driverNpc);
			}
			this._driverNpc = null;
		}

		// Token: 0x060054DC RID: 21724 RVA: 0x0020EF38 File Offset: 0x0020D138
		private void ReleaseVehicle()
		{
			if (this._blockedByUs && this._vehicle != null)
			{
				this._vehicle.SetInteractionBlocked(this, false);
			}
			this._blockedByUs = false;
			if (this._passengerFlagByUs && this._vehicle != null)
			{
				this._vehicle.HasPassenger = (this._carrier != null && this._carrier.HasRider) || (this._carHide != null && this._carHide.IsBusy);
				if (!this._vehicle.HasPassenger)
				{
					this._vehicle.PassengerRoot = null;
				}
				this._passengerFlagByUs = false;
			}
		}

		// Token: 0x060054DD RID: 21725 RVA: 0x0020EFEC File Offset: 0x0020D1EC
		private void SpawnDriver()
		{
			if (CarRideLimiter.s_spawner == null)
			{
				CarRideLimiter.s_spawner = Object.FindObjectOfType<NPCRuntimeSpawner>();
			}
			if (CarRideLimiter.s_spawner == null)
			{
				if (this.debugLogs)
				{
					Debug.LogWarning("[CarRideLimiter] No NPCRuntimeSpawner in scene — skipping driver spawn.", this);
				}
				return;
			}
			Vector3 vector = ((this._driverSpawnAnchor != null) ? this._driverSpawnAnchor.position : this.ResolveDoorWorldPos());
			vector = this.SnapToWalkable(vector);
			Vector3 vector2 = ((this._player != null) ? (this._player.position - vector) : base.transform.forward);
			vector2.y = 0f;
			Quaternion quaternion = ((vector2.sqrMagnitude > 0.0001f) ? Quaternion.LookRotation(vector2.normalized) : base.transform.rotation);
			this._driverNpc = CarRideLimiter.s_spawner.SpawnNPCSync(CarRideLimiter.s_spawner.GetRandomCharacterData(), vector, quaternion, null, false);
			if (this._driverNpc == null)
			{
				if (this.debugLogs)
				{
					Debug.LogWarning("[CarRideLimiter] Driver spawn failed.", this);
				}
				return;
			}
			NPCWanderController component = this._driverNpc.GetComponent<NPCWanderController>();
			if (component != null)
			{
				component.enabled = false;
			}
			AStarNPCController component2 = this._driverNpc.GetComponent<AStarNPCController>();
			if (component2 != null)
			{
				component2.SetPosture(NPCInterestPoint.PostureType.Standing);
				component2.SetAction(NPCInterestPoint.ActionType.None);
			}
			NPCSimpleLookAt component3 = this._driverNpc.GetComponent<NPCSimpleLookAt>();
			if (component3 != null && this._player != null)
			{
				component3.StartLookingAt(this._player);
			}
			if (this.debugLogs)
			{
				Debug.Log(string.Concat(new string[]
				{
					"[CarRideLimiter] Driver '",
					this._driverNpc.name,
					"' spawned at door of '",
					base.name,
					"'."
				}), this);
			}
		}

		// Token: 0x060054DE RID: 21726 RVA: 0x0020F1BC File Offset: 0x0020D3BC
		private Vector3 SnapToWalkable(Vector3 pos)
		{
			AstarPath active = AstarPath.active;
			NNInfo? nninfo = ((active != null) ? new NNInfo?(active.GetNearest(pos)) : null);
			if (((nninfo != null) ? nninfo.GetValueOrDefault().node : null) != null && nninfo.Value.node.Walkable)
			{
				Vector3 position = nninfo.Value.position;
				if ((position - pos).sqrMagnitude <= 36f)
				{
					return position;
				}
			}
			return pos;
		}

		// Token: 0x060054DF RID: 21727 RVA: 0x0020F23C File Offset: 0x0020D43C
		private Vector3 ResolveDoorWorldPos()
		{
			Transform transform = ((this._vehicle != null) ? this._vehicle.transform : base.transform);
			Collider component = transform.GetComponent<Collider>();
			if (component == null)
			{
				return transform.TransformPoint(this.doorLocalOffset);
			}
			float num = ((this.doorLocalOffset.x < 0f) ? (-1f) : 1f);
			Vector3 right = transform.right;
			Bounds bounds = component.bounds;
			float num2 = Mathf.Abs(bounds.extents.x * right.x) + Mathf.Abs(bounds.extents.y * right.y) + Mathf.Abs(bounds.extents.z * right.z);
			Vector3 vector = bounds.center + right * num * (num2 + this.doorSideClearance) + transform.forward * this.doorLocalOffset.z;
			vector.y = transform.position.y;
			return vector;
		}

		// Token: 0x060054E0 RID: 21728 RVA: 0x0020F354 File Offset: 0x0020D554
		private void ApplyDriverFace(int faceExpressionIndex)
		{
			if (this._driverNpc == null)
			{
				return;
			}
			Animator componentInChildren = this._driverNpc.GetComponentInChildren<Animator>();
			if (componentInChildren != null)
			{
				componentInChildren.SetInteger("FaceExpressionIndex", faceExpressionIndex);
			}
		}

		// Token: 0x060054E1 RID: 21729 RVA: 0x0020F394 File Offset: 0x0020D594
		private void StartDriverPhoneReaction(int reactionType, string propName)
		{
			if (this._driverNpc == null)
			{
				return;
			}
			Animator componentInChildren = this._driverNpc.GetComponentInChildren<Animator>();
			if (componentInChildren != null)
			{
				int layerIndex = componentInChildren.GetLayerIndex("Reactions");
				if (layerIndex >= 0)
				{
					componentInChildren.SetLayerWeight(layerIndex, 1f);
				}
				componentInChildren.SetInteger(CarRideLimiter.ReactionTypeHash, reactionType);
				componentInChildren.SetTrigger(CarRideLimiter.ReactionTriggerHash);
			}
			if (!string.IsNullOrEmpty(propName))
			{
				NPCEquipmentManager component = this._driverNpc.GetComponent<NPCEquipmentManager>();
				if (component != null)
				{
					component.ActivateAndLockProp(propName);
				}
			}
		}

		// Token: 0x060054E2 RID: 21730 RVA: 0x0020F41C File Offset: 0x0020D61C
		private void EndDriverPhoneReaction(string propName)
		{
			if (this._driverNpc == null)
			{
				return;
			}
			Animator componentInChildren = this._driverNpc.GetComponentInChildren<Animator>();
			if (componentInChildren != null)
			{
				componentInChildren.SetInteger(CarRideLimiter.ReactionTypeHash, 0);
			}
			if (!string.IsNullOrEmpty(propName))
			{
				NPCEquipmentManager component = this._driverNpc.GetComponent<NPCEquipmentManager>();
				if (component != null)
				{
					component.DeactivateProp(propName);
				}
			}
		}

		// Token: 0x060054E3 RID: 21731 RVA: 0x0020F480 File Offset: 0x0020D680
		private void PlayScream(DriverPersonalityProfileSO profile)
		{
			if (this._driverNpc == null)
			{
				return;
			}
			if (profile.ScreamClips != null && profile.ScreamClips.Length != 0)
			{
				AudioClip audioClip = profile.ScreamClips[Random.Range(0, profile.ScreamClips.Length)];
				if (audioClip != null)
				{
					AudioSource.PlayClipAtPoint(audioClip, this._driverNpc.transform.position, profile.ScreamVolume);
					return;
				}
			}
			NPCReactionAudioPlayer component = this._driverNpc.GetComponent<NPCReactionAudioPlayer>();
			if (component != null)
			{
				component.PlayStopAndStareAudio();
			}
		}

		// Token: 0x060054E4 RID: 21732 RVA: 0x0020F504 File Offset: 0x0020D704
		private void Honk()
		{
			AudioClip[] array = ((this._driverReaction != null) ? this._driverReaction.HornClips : null);
			if (array == null || array.Length == 0)
			{
				return;
			}
			this.PlayHornClip(array[Random.Range(0, array.Length)]);
		}

		// Token: 0x060054E5 RID: 21733 RVA: 0x0020F548 File Offset: 0x0020D748
		private void PlayHornClip(AudioClip clip)
		{
			if (clip == null)
			{
				return;
			}
			if (this._hornSource == null)
			{
				this._hornSource = base.gameObject.AddComponent<AudioSource>();
				this._hornSource.playOnAwake = false;
				this._hornSource.spatialBlend = 1f;
				this._hornSource.rolloffMode = AudioRolloffMode.Linear;
				this._hornSource.maxDistance = this.hornMaxDistance;
				this._hornSource.volume = this.hornVolume;
			}
			this._hornSource.PlayOneShot(clip, this.hornVolume);
		}

		// Token: 0x040054EF RID: 21743
		[Header("Ride limit")]
		[Tooltip("Max seconds the player may ride this car's roof before the driver reacts. Arms on the first successful roof mount for this spawned car.")]
		[SerializeField]
		private float maxRideDuration = 20f;

		// Token: 0x040054F0 RID: 21744
		[Header("Personality")]
		[Tooltip("Global weighted personality table (designer decision 2026-07-17: one shared asset). Null = always Angry with default tuning.")]
		[SerializeField]
		private DriverPersonalityProfileSO personalityProfile;

		// Token: 0x040054F1 RID: 21745
		[Tooltip("Debug: skip the roll and always play this personality.")]
		[SerializeField]
		private bool debugForcePersonality;

		// Token: 0x040054F2 RID: 21746
		[SerializeField]
		private DriverPersonality debugPersonality;

		// Token: 0x040054F3 RID: 21747
		[Header("Honk")]
		[Tooltip("Optional dedicated 'double honk' clip (double_horn.mp3): played ONCE at the ride limit instead of two separate random honks. Empty = legacy two-honk behaviour.")]
		[SerializeField]
		private AudioClip doubleHornClip;

		// Token: 0x040054F4 RID: 21748
		[Tooltip("Seconds between the two honks.")]
		[SerializeField]
		private float honkGap = 0.45f;

		// Token: 0x040054F5 RID: 21749
		[SerializeField]
		[Range(0f, 1f)]
		private float hornVolume = 0.8f;

		// Token: 0x040054F6 RID: 21750
		[SerializeField]
		private float hornMaxDistance = 25f;

		// Token: 0x040054F7 RID: 21751
		[Header("Driver")]
		[Tooltip("Fallback door position in the CAR's local space, used only when the car has no body collider to measure. Otherwise: X sign = door side, Z = forward offset, and the lateral distance comes from the body bounds + doorSideClearance.")]
		[SerializeField]
		private Vector3 doorLocalOffset = new Vector3(0.95f, 0f, 0.35f);

		// Token: 0x040054F8 RID: 21752
		[Tooltip("Clearance (m) between the body side and the spawned driver, so he steps out BESIDE the door instead of inside the bodywork.")]
		[SerializeField]
		private float doorSideClearance = 0.6f;

		// Token: 0x040054F9 RID: 21753
		[Tooltip("Max seconds to wait for the car to actually stop before spawning the driver.")]
		[SerializeField]
		private float stopWaitTimeout = 4f;

		// Token: 0x040054FA RID: 21754
		[Header("Confrontation end")]
		[Tooltip("The player must be at least this far (m) from the car…")]
		[SerializeField]
		private float leaveDistance = 6f;

		// Token: 0x040054FB RID: 21755
		[Tooltip("…for this long (s) before the driver lets it go and gets back in.")]
		[SerializeField]
		private float leaveHoldSeconds = 2f;

		// Token: 0x040054FC RID: 21756
		[Tooltip("Seconds the roof refuses presence-mounts after the driver drives off.")]
		[SerializeField]
		private float remountLockSeconds = 30f;

		// Token: 0x040054FD RID: 21757
		[SerializeField]
		private bool debugLogs;

		// Token: 0x040054FE RID: 21758
		private VehicleAI _vehicle;

		// Token: 0x040054FF RID: 21759
		private CarRoofCarrier _carrier;

		// Token: 0x04005500 RID: 21760
		private CarHideUnderInteractable _carHide;

		// Token: 0x04005501 RID: 21761
		private DriverReactionController _driverReaction;

		// Token: 0x04005502 RID: 21762
		private AudioSource _hornSource;

		// Token: 0x04005503 RID: 21763
		private static NPCRuntimeSpawner s_spawner;

		// Token: 0x04005504 RID: 21764
		private static VehicleSpawner s_vehicleSpawner;

		// Token: 0x04005505 RID: 21765
		private static DriverPersonalityProfileSO s_defaultProfile;

		// Token: 0x04005506 RID: 21766
		private Transform _player;

		// Token: 0x04005507 RID: 21767
		private int _playerResolveFrame;

		// Token: 0x04005508 RID: 21768
		private bool _wasHiddenUnder;

		// Token: 0x04005509 RID: 21769
		private float _rideTimeRemaining = -1f;

		// Token: 0x0400550A RID: 21770
		private Coroutine _sequence;

		// Token: 0x0400550B RID: 21771
		private GameObject _driverNpc;

		// Token: 0x0400550C RID: 21772
		private bool _blockedByUs;

		// Token: 0x0400550D RID: 21773
		private bool _passengerFlagByUs;

		// Token: 0x0400550E RID: 21774
		private bool _abandoned;

		// Token: 0x0400550F RID: 21775
		private float _accelRestore = -1f;

		// Token: 0x04005510 RID: 21776
		private static readonly int ReactionTypeHash = Animator.StringToHash("ReactionType");

		// Token: 0x04005511 RID: 21777
		private static readonly int ReactionTriggerHash = Animator.StringToHash("ReactionTrigger");

		// Token: 0x04005512 RID: 21778
		private const int FaceNeutral = 0;

		// Token: 0x04005513 RID: 21779
		private const int FaceSurprised = 1;

		// Token: 0x04005514 RID: 21780
		private const int FaceAngry = 2;

		// Token: 0x04005515 RID: 21781
		private const int FaceScared = 5;

		// Token: 0x04005516 RID: 21782
		private const int ReactionTakePhoto = 1;

		// Token: 0x04005517 RID: 21783
		private const int ReactionTakeVideo = 2;

		// Token: 0x04005518 RID: 21784
		private const string DriverSpawnAnchorName = "DriverSpawnAnchor";

		// Token: 0x04005519 RID: 21785
		private Transform _driverSpawnAnchor;
	}
}
