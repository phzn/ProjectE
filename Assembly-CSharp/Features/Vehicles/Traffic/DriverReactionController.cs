using System;
using Features.Detection;
using Features.PlayerEmotions;
using UnityEngine;
using VehicleSpawnPlugin;

namespace Features.Vehicles.Traffic
{
	// Token: 0x02000BA3 RID: 2979
	[DisallowMultipleComponent]
	public class DriverReactionController : MonoBehaviour
	{
		// Token: 0x17000E41 RID: 3649
		// (get) Token: 0x0600557E RID: 21886 RVA: 0x0021296B File Offset: 0x00210B6B
		public AudioClip[] HornClips
		{
			get
			{
				return this.hornClips;
			}
		}

		// Token: 0x0600557F RID: 21887 RVA: 0x00212973 File Offset: 0x00210B73
		private void Awake()
		{
			this._vehicle = base.GetComponent<VehicleAI>();
			if (this._vehicle == null)
			{
				this._vehicle = base.GetComponentInParent<VehicleAI>();
			}
			this._lights = base.GetComponent<VehicleLightsController>();
		}

		// Token: 0x06005580 RID: 21888 RVA: 0x002129A7 File Offset: 0x00210BA7
		private void OnDisable()
		{
			if (this._gawking && this._vehicle != null)
			{
				this._vehicle.ClearSpeedZoneLimit();
			}
			this._gawking = false;
			this._gawkClearTime = -1f;
			this._simulationPauseActive = false;
		}

		// Token: 0x06005581 RID: 21889 RVA: 0x002129E4 File Offset: 0x00210BE4
		private void Update()
		{
			if (this.TrackSimulationPause())
			{
				return;
			}
			if (this._vehicle == null)
			{
				return;
			}
			if (this._gawking && Time.time >= this._gawkClearTime)
			{
				this._vehicle.ClearSpeedZoneLimit();
				this._gawking = false;
			}
			if (Time.time < this._nextCheckTime)
			{
				return;
			}
			this._nextCheckTime = Time.time + this.checkInterval;
			this.UpdateQueueHonk();
			if (Time.time < this._reactReadyTime)
			{
				return;
			}
			if (this._vehicle.CurrentSpeed < this.minSpeedToReact)
			{
				return;
			}
			if (!this.TryResolvePlayer())
			{
				return;
			}
			if (!PlayerExposureState.IsExposed)
			{
				return;
			}
			if (this.IsPlayerInCone())
			{
				this.React();
			}
		}

		// Token: 0x06005582 RID: 21890 RVA: 0x00212A98 File Offset: 0x00210C98
		private bool TrackSimulationPause()
		{
			if (VehicleSimulationPause.IsPaused)
			{
				if (!this._simulationPauseActive)
				{
					this._simulationPauseActive = true;
					this._simulationPauseStartedAt = Time.time;
				}
				return true;
			}
			if (!this._simulationPauseActive)
			{
				return false;
			}
			float num = Mathf.Max(0f, Time.time - this._simulationPauseStartedAt);
			DriverReactionController.ShiftDeadline(ref this._nextCheckTime, num);
			DriverReactionController.ShiftDeadline(ref this._reactReadyTime, num);
			DriverReactionController.ShiftDeadline(ref this._gawkClearTime, num);
			DriverReactionController.ShiftDeadline(ref this._stoppedSince, num);
			DriverReactionController.ShiftDeadline(ref this._queueHonkReadyTime, num);
			this._simulationPauseActive = false;
			return false;
		}

		// Token: 0x06005583 RID: 21891 RVA: 0x00212B2D File Offset: 0x00210D2D
		private static void ShiftDeadline(ref float timestamp, float heldDuration)
		{
			if (timestamp >= 0f)
			{
				timestamp += heldDuration;
			}
		}

		// Token: 0x06005584 RID: 21892 RVA: 0x00212B40 File Offset: 0x00210D40
		private void UpdateQueueHonk()
		{
			if (!this.queueHonkEnabled)
			{
				return;
			}
			if (this._vehicle.CurrentSpeed > 0.3f || this._vehicle.IsBlocked)
			{
				this._stoppedSince = -1f;
				return;
			}
			if (this._stoppedSince < 0f)
			{
				this._stoppedSince = Time.time;
				return;
			}
			if (Time.time - this._stoppedSince < this.queueHonkAfterSeconds)
			{
				return;
			}
			if (Time.time < this._queueHonkReadyTime)
			{
				return;
			}
			if (!this.TryResolvePlayer())
			{
				return;
			}
			if ((base.transform.position - this._player.position).sqrMagnitude > this.queueHonkPlayerRange * this.queueHonkPlayerRange)
			{
				return;
			}
			if (Random.value > this.queueHonkChance)
			{
				return;
			}
			this._queueHonkReadyTime = Time.time + this.queueHonkCooldown + Random.Range(0f, 3f);
			this.Honk();
			if (this.debugLogs)
			{
				Debug.Log("[DriverReaction] '" + base.name + "' protest honk (queued).", this);
			}
		}

		// Token: 0x06005585 RID: 21893 RVA: 0x00212C54 File Offset: 0x00210E54
		private bool TryResolvePlayer()
		{
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

		// Token: 0x06005586 RID: 21894 RVA: 0x00212CBC File Offset: 0x00210EBC
		private bool IsPlayerInCone()
		{
			Vector3 vector = this._player.position - base.transform.position;
			vector.y = 0f;
			float sqrMagnitude = vector.sqrMagnitude;
			float num = ((this._lights != null && VehicleLightsManager.Instance != null && VehicleLightsManager.Instance.IsNight) ? (this.reactRange + this.nightRangeBonus) : this.reactRange);
			if (sqrMagnitude > num * num || sqrMagnitude < 0.0001f)
			{
				return false;
			}
			Vector3 travelDirection = this._vehicle.TravelDirection;
			travelDirection.y = 0f;
			return travelDirection.sqrMagnitude >= 0.0001f && Vector3.Dot(travelDirection.normalized, vector / Mathf.Sqrt(sqrMagnitude)) >= Mathf.Cos(this.coneHalfAngle * 0.017453292f);
		}

		// Token: 0x06005587 RID: 21895 RVA: 0x00212DA0 File Offset: 0x00210FA0
		private void React()
		{
			this._reactReadyTime = Time.time + this.reactionCooldown;
			this.Honk();
			if (this._lights != null)
			{
				this._lights.FlashHeadlights(this.flashDuration);
			}
			this._vehicle.SetSpeedZoneLimit(this.gawkSpeedLimit);
			this._gawking = true;
			this._gawkClearTime = Time.time + this.gawkDuration;
			if (EmotionManager.Instance != null && this.emotionDelta != 0f)
			{
				EmotionManager.Instance.ApplyDelta(this.emotionDelta, "DriverReaction", EmotionSourceCategory.Environment);
			}
			if (this.debugLogs)
			{
				Debug.Log("[DriverReaction] '" + base.name + "' reacted to exposed player.", this);
			}
		}

		// Token: 0x06005588 RID: 21896 RVA: 0x00212E64 File Offset: 0x00211064
		private void Honk()
		{
			if (this.hornClips == null || this.hornClips.Length == 0)
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
			AudioClip audioClip = this.hornClips[Random.Range(0, this.hornClips.Length)];
			if (audioClip != null)
			{
				this._hornSource.PlayOneShot(audioClip, this.hornVolume);
			}
		}

		// Token: 0x0400559B RID: 21915
		[Header("Detection cone")]
		[Tooltip("Max distance (m) at which the driver reacts to the exposed player.")]
		[SerializeField]
		private float reactRange = 12f;

		// Token: 0x0400559C RID: 21916
		[Tooltip("Half-angle (deg) of the forward cone. Player must be within it.")]
		[SerializeField]
		private float coneHalfAngle = 40f;

		// Token: 0x0400559D RID: 21917
		[Tooltip("Extra reach (m) added to the cone at night, when the headlight beam sweeps the player.")]
		[SerializeField]
		private float nightRangeBonus = 6f;

		// Token: 0x0400559E RID: 21918
		[Tooltip("Minimum car speed (m/s) to react. A parked/blocked car doesn't honk.")]
		[SerializeField]
		private float minSpeedToReact = 1f;

		// Token: 0x0400559F RID: 21919
		[Header("Timing")]
		[Tooltip("Seconds between cone checks (throttle, never per-frame).")]
		[SerializeField]
		private float checkInterval = 0.35f;

		// Token: 0x040055A0 RID: 21920
		[Tooltip("Cooldown (s) after a reaction before this car can react again.")]
		[SerializeField]
		private float reactionCooldown = 6f;

		// Token: 0x040055A1 RID: 21921
		[Header("Slow-down 'to look'")]
		[Tooltip("Speed limit (m/s) forced while the driver gawks.")]
		[SerializeField]
		private float gawkSpeedLimit = 3f;

		// Token: 0x040055A2 RID: 21922
		[Tooltip("How long the slow-down lasts (s).")]
		[SerializeField]
		private float gawkDuration = 2f;

		// Token: 0x040055A3 RID: 21923
		[Header("Horn")]
		[Tooltip("2-3 horn clips; one is picked at random per honk. Left empty = no audio (assign in prefab).")]
		[SerializeField]
		private AudioClip[] hornClips;

		// Token: 0x040055A4 RID: 21924
		[SerializeField]
		[Range(0f, 1f)]
		private float hornVolume = 0.7f;

		// Token: 0x040055A5 RID: 21925
		[SerializeField]
		private float hornMaxDistance = 25f;

		// Token: 0x040055A6 RID: 21926
		[Header("Queue honk")]
		[Tooltip("Honk in protest when stuck in a queue near the player (e.g. behind an abandoned car). Only when NOT intentionally blocked (zones/junction waits stay silent).")]
		[SerializeField]
		private bool queueHonkEnabled = true;

		// Token: 0x040055A7 RID: 21927
		[Tooltip("Seconds fully stopped before protest honks may start (courtesy stops last 2s and never reach this).")]
		[SerializeField]
		private float queueHonkAfterSeconds = 3.5f;

		// Token: 0x040055A8 RID: 21928
		[Tooltip("Chance per check (0.35s cadence) to honk once eligible.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float queueHonkChance = 0.35f;

		// Token: 0x040055A9 RID: 21929
		[Tooltip("Cooldown (s) between protest honks of this car (plus 0-3s jitter).")]
		[SerializeField]
		private float queueHonkCooldown = 9f;

		// Token: 0x040055AA RID: 21930
		[Tooltip("Only honk if the player is within this range (m) — nobody hears it otherwise.")]
		[SerializeField]
		private float queueHonkPlayerRange = 35f;

		// Token: 0x040055AB RID: 21931
		[Header("Emotion")]
		[Tooltip("Balance delta applied to the player on reaction (positive = exhibition/confident side).")]
		[SerializeField]
		private float emotionDelta = 3f;

		// Token: 0x040055AC RID: 21932
		[Header("Headlight flash")]
		[SerializeField]
		private float flashDuration = 0.6f;

		// Token: 0x040055AD RID: 21933
		[SerializeField]
		private bool debugLogs;

		// Token: 0x040055AE RID: 21934
		private VehicleAI _vehicle;

		// Token: 0x040055AF RID: 21935
		private VehicleLightsController _lights;

		// Token: 0x040055B0 RID: 21936
		private AudioSource _hornSource;

		// Token: 0x040055B1 RID: 21937
		private Transform _player;

		// Token: 0x040055B2 RID: 21938
		private int _playerResolveFrame;

		// Token: 0x040055B3 RID: 21939
		private float _nextCheckTime;

		// Token: 0x040055B4 RID: 21940
		private float _reactReadyTime;

		// Token: 0x040055B5 RID: 21941
		private float _gawkClearTime = -1f;

		// Token: 0x040055B6 RID: 21942
		private bool _gawking;

		// Token: 0x040055B7 RID: 21943
		private float _stoppedSince = -1f;

		// Token: 0x040055B8 RID: 21944
		private float _queueHonkReadyTime;

		// Token: 0x040055B9 RID: 21945
		private bool _simulationPauseActive;

		// Token: 0x040055BA RID: 21946
		private float _simulationPauseStartedAt;
	}
}
