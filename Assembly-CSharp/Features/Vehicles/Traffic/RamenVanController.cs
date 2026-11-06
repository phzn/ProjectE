using System;
using UnityEngine;
using VehicleSpawnPlugin;

namespace Features.Vehicles.Traffic
{
	// Token: 0x02000BA5 RID: 2981
	[DisallowMultipleComponent]
	[RequireComponent(typeof(VehicleAI))]
	public class RamenVanController : MonoBehaviour
	{
		// Token: 0x06005591 RID: 21905 RVA: 0x0021330C File Offset: 0x0021150C
		private void Awake()
		{
			this._vehicle = base.GetComponent<VehicleAI>();
			this.EnsureJingle();
		}

		// Token: 0x06005592 RID: 21906 RVA: 0x00213320 File Offset: 0x00211520
		private void OnEnable()
		{
			this._state = RamenVanController.StopState.Driving;
			this._weBlocked = false;
			this._nextStopTime = Time.time + this.initialGrace + Random.Range(0f, this.cooldownRandomSlack);
			if (this._jingle != null && this.jingleClip != null && !this._jingle.isPlaying)
			{
				this._jingle.Play();
			}
		}

		// Token: 0x06005593 RID: 21907 RVA: 0x00213394 File Offset: 0x00211594
		private void OnDisable()
		{
			if (this._weBlocked && this._vehicle != null)
			{
				this._vehicle.SetBlocked(false);
			}
			this._weBlocked = false;
			this._state = RamenVanController.StopState.Driving;
			if (this._jingle != null && this._jingle.isPlaying)
			{
				this._jingle.Stop();
			}
		}

		// Token: 0x06005594 RID: 21908 RVA: 0x002133F8 File Offset: 0x002115F8
		private void Update()
		{
			if (this._vehicle == null)
			{
				return;
			}
			switch (this._state)
			{
			case RamenVanController.StopState.Driving:
				if (Time.time >= this._nextStopTime)
				{
					this._vehicle.SetSpeedZoneLimit(0.4f);
					this._state = RamenVanController.StopState.EasingIn;
					return;
				}
				break;
			case RamenVanController.StopState.EasingIn:
				if (this._vehicle.CurrentSpeed <= this.minSpeedBeforeStopBlock)
				{
					this._vehicle.SetBlocked(true);
					this._weBlocked = true;
					this._vehicle.ClearSpeedZoneLimit();
					this._stopEndTime = Time.time + this.stopDuration;
					this._state = RamenVanController.StopState.Stopped;
					if (this.debugLogs)
					{
						Debug.Log(string.Format("[RamenVan] '{0}' stopped roadside for {1:0.#}s.", base.name, this.stopDuration), this);
						return;
					}
				}
				break;
			case RamenVanController.StopState.Stopped:
				if (Time.time >= this._stopEndTime)
				{
					this._vehicle.SetBlocked(false);
					this._weBlocked = false;
					this._state = RamenVanController.StopState.Driving;
					this._nextStopTime = Time.time + this.minCooldownBetweenStops + Random.Range(0f, this.cooldownRandomSlack);
					if (this.debugLogs)
					{
						Debug.Log(string.Format("[RamenVan] '{0}' resumed; next stop in >= {1:0.#}s.", base.name, this.minCooldownBetweenStops), this);
					}
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06005595 RID: 21909 RVA: 0x00213544 File Offset: 0x00211744
		private void EnsureJingle()
		{
			this._jingle = base.GetComponent<AudioSource>();
			if (this._jingle == null)
			{
				this._jingle = base.gameObject.AddComponent<AudioSource>();
			}
			this._jingle.clip = this.jingleClip;
			this._jingle.loop = true;
			this._jingle.playOnAwake = false;
			this._jingle.spatialBlend = 1f;
			this._jingle.rolloffMode = AudioRolloffMode.Linear;
			this._jingle.minDistance = this.jingleMinDistance;
			this._jingle.maxDistance = this.jingleMaxDistance;
			this._jingle.volume = this.jingleVolume;
		}

		// Token: 0x040055C9 RID: 21961
		[Header("Jingle (looping, spatialised)")]
		[Tooltip("Looping jingle clip. Left empty = silent (assign on the prefab). Spatial, short range.")]
		[SerializeField]
		private AudioClip jingleClip;

		// Token: 0x040055CA RID: 21962
		[SerializeField]
		[Range(0f, 1f)]
		private float jingleVolume = 0.5f;

		// Token: 0x040055CB RID: 21963
		[Tooltip("Beyond this distance (m) the jingle is inaudible (audio budget).")]
		[SerializeField]
		private float jingleMaxDistance = 22f;

		// Token: 0x040055CC RID: 21964
		[Tooltip("Below this distance (m) the jingle plays at full volume.")]
		[SerializeField]
		private float jingleMinDistance = 4f;

		// Token: 0x040055CD RID: 21965
		[Header("Roadside stops")]
		[Tooltip("Only stops below this speed (m/s) — never slams to a halt at speed; it eases in via IDM when it enters a stop window and the IDM decelerates it, then we block once slow.")]
		[SerializeField]
		private float minSpeedBeforeStopBlock = 1.5f;

		// Token: 0x040055CE RID: 21966
		[Tooltip("How long a single roadside stop lasts (s).")]
		[SerializeField]
		private float stopDuration = 6f;

		// Token: 0x040055CF RID: 21967
		[Tooltip("Minimum time between the END of one stop and the START of the next (s).")]
		[SerializeField]
		private float minCooldownBetweenStops = 20f;

		// Token: 0x040055D0 RID: 21968
		[Tooltip("Extra random slack added on top of the cooldown (s), so stops are not metronomic.")]
		[SerializeField]
		private float cooldownRandomSlack = 15f;

		// Token: 0x040055D1 RID: 21969
		[Tooltip("How long after spawning before the first stop can happen (s).")]
		[SerializeField]
		private float initialGrace = 8f;

		// Token: 0x040055D2 RID: 21970
		[SerializeField]
		private bool debugLogs;

		// Token: 0x040055D3 RID: 21971
		private VehicleAI _vehicle;

		// Token: 0x040055D4 RID: 21972
		private AudioSource _jingle;

		// Token: 0x040055D5 RID: 21973
		private RamenVanController.StopState _state;

		// Token: 0x040055D6 RID: 21974
		private float _nextStopTime;

		// Token: 0x040055D7 RID: 21975
		private float _stopEndTime;

		// Token: 0x040055D8 RID: 21976
		private bool _weBlocked;

		// Token: 0x02000BA6 RID: 2982
		private enum StopState
		{
			// Token: 0x040055DA RID: 21978
			Driving,
			// Token: 0x040055DB RID: 21979
			EasingIn,
			// Token: 0x040055DC RID: 21980
			Stopped
		}
	}
}
