using System;
using Features.Detection;
using Features.Suspicion;
using UnityEngine;
using VehicleSpawnPlugin;

namespace Features.Vehicles.Traffic
{
	// Token: 0x02000BA4 RID: 2980
	[DisallowMultipleComponent]
	public class PoliceVehicleController : MonoBehaviour
	{
		// Token: 0x0600558A RID: 21898 RVA: 0x00213000 File Offset: 0x00211200
		private void Awake()
		{
			this._vehicle = base.GetComponent<VehicleAI>();
			if (this._vehicle == null)
			{
				this._vehicle = base.GetComponentInParent<VehicleAI>();
			}
			this._lights = base.GetComponent<VehicleLightsController>();
		}

		// Token: 0x0600558B RID: 21899 RVA: 0x00213034 File Offset: 0x00211234
		private void OnEnable()
		{
			this._seedReadyTime = Time.time + this.checkInterval;
			this._player = null;
		}

		// Token: 0x0600558C RID: 21900 RVA: 0x00213050 File Offset: 0x00211250
		private void Update()
		{
			if (this._vehicle == null)
			{
				return;
			}
			if (Time.time < this._nextCheckTime)
			{
				return;
			}
			this._nextCheckTime = Time.time + this.checkInterval;
			if (Time.time < this._seedReadyTime)
			{
				return;
			}
			if (this._vehicle.CurrentSpeed < this.minSpeedToNotice)
			{
				return;
			}
			if (SuspicionManager.Instance == null)
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
				this.ApplySeed();
			}
		}

		// Token: 0x0600558D RID: 21901 RVA: 0x002130DC File Offset: 0x002112DC
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

		// Token: 0x0600558E RID: 21902 RVA: 0x00213144 File Offset: 0x00211344
		private bool IsPlayerInCone()
		{
			Vector3 vector = this._player.position - base.transform.position;
			vector.y = 0f;
			float sqrMagnitude = vector.sqrMagnitude;
			float num = ((this._lights != null && VehicleLightsManager.Instance != null && VehicleLightsManager.Instance.IsNight) ? (this.noticeRange + this.nightRangeBonus) : this.noticeRange);
			if (sqrMagnitude > num * num || sqrMagnitude < 0.0001f)
			{
				return false;
			}
			Vector3 travelDirection = this._vehicle.TravelDirection;
			travelDirection.y = 0f;
			return travelDirection.sqrMagnitude >= 0.0001f && Vector3.Dot(travelDirection.normalized, vector / Mathf.Sqrt(sqrMagnitude)) >= Mathf.Cos(this.coneHalfAngle * 0.017453292f);
		}

		// Token: 0x0600558F RID: 21903 RVA: 0x00213228 File Offset: 0x00211428
		private void ApplySeed()
		{
			this._seedReadyTime = Time.time + this.seedCooldown;
			if (this.suspicionDelta > 0f)
			{
				SuspicionManager.Instance.AddSuspicion(this.suspicionDelta, null);
			}
			if (this._lights != null)
			{
				this._lights.FlashHeadlights(0.6f);
			}
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[PoliceVehicle] '{0}' seeded +{1} suspicion (exposed player in cone).", base.name, this.suspicionDelta), this);
			}
		}

		// Token: 0x040055BB RID: 21947
		[Header("Detection cone")]
		[Tooltip("Max distance (m) at which the cruiser notices the exposed player.")]
		[SerializeField]
		private float noticeRange = 14f;

		// Token: 0x040055BC RID: 21948
		[Tooltip("Half-angle (deg) of the forward cone. Player must be within it.")]
		[SerializeField]
		private float coneHalfAngle = 40f;

		// Token: 0x040055BD RID: 21949
		[Tooltip("Extra reach (m) at night (headlight beam sweeps the player).")]
		[SerializeField]
		private float nightRangeBonus = 6f;

		// Token: 0x040055BE RID: 21950
		[Tooltip("Minimum cruiser speed (m/s) to notice. A parked/blocked cruiser doesn't seed.")]
		[SerializeField]
		private float minSpeedToNotice = 0.5f;

		// Token: 0x040055BF RID: 21951
		[Header("Timing")]
		[Tooltip("Seconds between cone checks (throttle, never per-frame).")]
		[SerializeField]
		private float checkInterval = 0.4f;

		// Token: 0x040055C0 RID: 21952
		[Tooltip("Cooldown (s) after a seed before this cruiser can seed again.")]
		[SerializeField]
		private float seedCooldown = 8f;

		// Token: 0x040055C1 RID: 21953
		[Header("Suspicion")]
		[Tooltip("Suspicion added to SuspicionManager when the cruiser notices the exposed player. Small on purpose — this is a seed, not a full detection.")]
		[SerializeField]
		private float suspicionDelta = 1f;

		// Token: 0x040055C2 RID: 21954
		[SerializeField]
		private bool debugLogs;

		// Token: 0x040055C3 RID: 21955
		private VehicleAI _vehicle;

		// Token: 0x040055C4 RID: 21956
		private VehicleLightsController _lights;

		// Token: 0x040055C5 RID: 21957
		private Transform _player;

		// Token: 0x040055C6 RID: 21958
		private int _playerResolveFrame;

		// Token: 0x040055C7 RID: 21959
		private float _nextCheckTime;

		// Token: 0x040055C8 RID: 21960
		private float _seedReadyTime;
	}
}
