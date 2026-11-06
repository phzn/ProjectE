using System;
using System.Collections.Generic;
using Features.Detection;
using Features.NPC;
using Features.NPC.StateMachine;
using PoolWaterTools;
using UnityEngine;

namespace Features.Police
{
	// Token: 0x02000DFC RID: 3580
	[DisallowMultipleComponent]
	public class TaserBoltProjectile : MonoBehaviour
	{
		// Token: 0x06006A8A RID: 27274 RVA: 0x002AB94C File Offset: 0x002A9B4C
		public void Init(Vector3 direction, float speed, float maxDistance, float hitRadius, LayerMask hitMask, LayerMask obstacleMask, PlayerActionEntry[] hitVariants, bool debugLogs, StealthActor target = null)
		{
			if (direction.sqrMagnitude < 0.0001f)
			{
				direction = base.transform.forward;
			}
			this._direction = direction.normalized;
			this._speed = Mathf.Max(0.1f, speed);
			this._maxDistance = Mathf.Max(0.1f, maxDistance);
			this._hitRadius = Mathf.Max(0.05f, hitRadius);
			this._hitMask = hitMask;
			this._obstacleMask = obstacleMask;
			this._hitVariants = hitVariants;
			this._debugLogs = debugLogs;
			this._targetCapsule = ((target != null) ? target.GetComponent<CapsuleCollider>() : null);
			this._target = ((target != null) ? target.transform : null);
			if (this._direction.sqrMagnitude > 0.0001f)
			{
				base.transform.rotation = Quaternion.LookRotation(this._direction, Vector3.up);
			}
			AudioSource component = base.GetComponent<AudioSource>();
			if (component != null && component.clip != null)
			{
				this._audioTargetVolume = component.volume;
				component.volume = 0f;
				this._audioFadeInTime = 0.12f;
				this._audioFadeInElapsed = 0f;
			}
		}

		// Token: 0x06006A8B RID: 27275 RVA: 0x002ABA84 File Offset: 0x002A9C84
		private void LateUpdate()
		{
			if (NPCSimulationPause.IsPaused || NPCSimulationPause.SimulationScale <= 0.0001f)
			{
				return;
			}
			if (this._audioFadeInTime > 0f && this._audioFadeInElapsed < this._audioFadeInTime)
			{
				this._audioFadeInElapsed += NPCSimulationPause.SimulationDeltaTime;
				AudioSource component = base.GetComponent<AudioSource>();
				if (component != null)
				{
					float num = Mathf.Clamp01(this._audioFadeInElapsed / this._audioFadeInTime);
					component.volume = Mathf.Lerp(0f, this._audioTargetVolume, num);
				}
			}
		}

		// Token: 0x06006A8C RID: 27276 RVA: 0x002ABB0C File Offset: 0x002A9D0C
		private void Update()
		{
			if (this._consumed)
			{
				return;
			}
			if (TaserOfficer.MustAbortAttack || PoolWaterRecovery.IsPlayerInWater(this._target))
			{
				this.Fizzle();
				return;
			}
			if (NPCSimulationPause.IsPaused || NPCSimulationPause.SimulationScale <= 0.0001f)
			{
				return;
			}
			float num = Mathf.Min(this._speed * NPCSimulationPause.SimulationDeltaTime, this._maxDistance - this._traveled);
			Vector3 position = base.transform.position;
			Vector3 vector = position + this._direction * num;
			float positiveInfinity = float.PositiveInfinity;
			if (this._targetCapsule != null && !this._targetCapsule.enabled && (this._hitMask & (1 << this._targetCapsule.gameObject.layer)) != 0 && this._targetCapsule.gameObject.activeInHierarchy)
			{
				TaserBoltProjectile.TrySweepRider(position, vector, this._targetCapsule, this._hitRadius, out positiveInfinity);
			}
			int num2 = this._hitMask | this._obstacleMask;
			int num3 = Physics.SphereCastNonAlloc(position, this._hitRadius, this._direction, TaserBoltProjectile._sweepBuffer, num, num2, QueryTriggerInteraction.Ignore);
			RaycastHit[] array = TaserBoltProjectile._sweepBuffer;
			if (num3 == array.Length)
			{
				array = Physics.SphereCastAll(position, this._hitRadius, this._direction, num, num2, QueryTriggerInteraction.Ignore);
				num3 = array.Length;
			}
			Array.Sort<RaycastHit>(array, 0, num3, TaserBoltProjectile.HitDistanceComparer.Instance);
			int num4 = 0;
			while (num4 < num3 && positiveInfinity >= array[num4].distance)
			{
				if (this.TryHitCollider(array[num4].collider))
				{
					return;
				}
				num4++;
			}
			if (positiveInfinity <= num)
			{
				base.transform.position = position + this._direction * positiveInfinity;
				this.OnHitPlayer();
				return;
			}
			base.transform.position = vector;
			this._traveled += num;
			int num5 = Physics.OverlapSphereNonAlloc(base.transform.position, this._hitRadius, TaserBoltProjectile._hitBuffer, num2, QueryTriggerInteraction.Ignore);
			for (int i = 0; i < num5; i++)
			{
				if (this.TryHitCollider(TaserBoltProjectile._hitBuffer[i]))
				{
					return;
				}
			}
			if (this._traveled >= this._maxDistance)
			{
				this.Fizzle();
			}
		}

		// Token: 0x06006A8D RID: 27277 RVA: 0x002ABD40 File Offset: 0x002A9F40
		private bool TryHitCollider(Collider col)
		{
			if (col == null)
			{
				return false;
			}
			int num = 1 << col.gameObject.layer;
			if ((num & this._hitMask) != 0)
			{
				StealthActor componentInParent = col.GetComponentInParent<StealthActor>();
				if (componentInParent != null)
				{
					this._target = componentInParent.transform;
					this.OnHitPlayer();
					return true;
				}
			}
			if (!TaserOfficer.IsPoliceCollider(col) && NPCTaserApplier.TryTaser(col))
			{
				if (this._debugLogs)
				{
					Debug.Log(string.Format("[TaserBolt] Tasered NPC '{0}' after {1:F1}m", col.name, this._traveled));
				}
				this._consumed = true;
				Object.Destroy(base.gameObject);
				return true;
			}
			if ((num & this._obstacleMask) == 0)
			{
				return false;
			}
			if (TaserBoltProjectile.IsLivingEntity(col))
			{
				return false;
			}
			if (this._debugLogs)
			{
				Debug.Log(string.Format("[TaserBolt] Hit obstacle '{0}' (layer={1}) after {2:F1}m", col.name, LayerMask.LayerToName(col.gameObject.layer), this._traveled));
			}
			this.Fizzle();
			return true;
		}

		// Token: 0x06006A8E RID: 27278 RVA: 0x002ABE44 File Offset: 0x002AA044
		internal static bool TrySweepRider(Vector3 start, Vector3 end, CapsuleCollider capsule, float boltRadius, out float distance)
		{
			Transform transform = capsule.transform;
			Vector3 lossyScale = transform.lossyScale;
			lossyScale = new Vector3(Mathf.Abs(lossyScale.x), Mathf.Abs(lossyScale.y), Mathf.Abs(lossyScale.z));
			int direction = capsule.direction;
			float num = capsule.radius * Mathf.Max(lossyScale[(direction + 1) % 3], lossyScale[(direction + 2) % 3]);
			float num2 = Mathf.Max(0f, capsule.height * lossyScale[direction] * 0.5f - num);
			Vector3 vector = ((direction == 0) ? Vector3.right : ((direction == 1) ? Vector3.up : Vector3.forward));
			Vector3 vector2 = transform.TransformDirection(vector) * num2;
			Vector3 vector3 = transform.TransformPoint(capsule.center);
			return TaserBoltProjectile.SweepCapsule(start, end, vector3 - vector2, vector3 + vector2, num + boltRadius, out distance);
		}

		// Token: 0x06006A8F RID: 27279 RVA: 0x002ABF2C File Offset: 0x002AA12C
		private static bool SweepCapsule(Vector3 start, Vector3 end, Vector3 a, Vector3 b, float radius, out float distance)
		{
			distance = float.PositiveInfinity;
			Vector3 vector = b - a;
			Vector3 vector2 = end - start;
			Vector3 vector3 = start - a;
			float sqrMagnitude = vector.sqrMagnitude;
			float magnitude = vector2.magnitude;
			float num = ((sqrMagnitude > 0f) ? Mathf.Clamp01(Vector3.Dot(vector3, vector) / sqrMagnitude) : 0f);
			if ((start - (a + num * vector)).sqrMagnitude <= radius * radius)
			{
				distance = 0f;
				return true;
			}
			if (magnitude < 1E-05f)
			{
				return false;
			}
			Vector3 vector4 = vector2 / magnitude;
			float num2 = Vector3.Dot(vector, vector4);
			float num3 = Vector3.Dot(vector, vector3);
			float num4 = Vector3.Dot(vector3, vector4);
			float num5 = sqrMagnitude - num2 * num2;
			float num6 = sqrMagnitude * num4 - num3 * num2;
			float num7 = sqrMagnitude * (vector3.sqrMagnitude - radius * radius) - num3 * num3;
			float num8 = num6 * num6 - num5 * num7;
			if (num5 > 1E-05f && num8 >= 0f)
			{
				float num9 = (-num6 - Mathf.Sqrt(num8)) / num5;
				float num10 = num3 + num9 * num2;
				if (num9 >= 0f && num10 >= 0f && num10 <= sqrMagnitude)
				{
					distance = num9;
				}
			}
			distance = Mathf.Min(distance, TaserBoltProjectile.RaySphere(start - a, vector4, radius));
			distance = Mathf.Min(distance, TaserBoltProjectile.RaySphere(start - b, vector4, radius));
			if (distance <= magnitude)
			{
				return true;
			}
			distance = float.PositiveInfinity;
			return false;
		}

		// Token: 0x06006A90 RID: 27280 RVA: 0x002AC0B4 File Offset: 0x002AA2B4
		private static float RaySphere(Vector3 origin, Vector3 direction, float radius)
		{
			float num = Vector3.Dot(origin, direction);
			float num2 = num * num - origin.sqrMagnitude + radius * radius;
			if (num2 < 0f)
			{
				return float.PositiveInfinity;
			}
			float num3 = -num - Mathf.Sqrt(num2);
			if (num3 < 0f)
			{
				return float.PositiveInfinity;
			}
			return num3;
		}

		// Token: 0x06006A91 RID: 27281 RVA: 0x002AC100 File Offset: 0x002AA300
		private static bool IsLivingEntity(Collider col)
		{
			return !(col == null) && (col.GetComponentInParent<StealthActor>() != null || col.GetComponentInParent<NPCStateMachine>() != null || col.GetComponentInParent<NPCSenses>() != null || TaserOfficer.IsPoliceCollider(col));
		}

		// Token: 0x06006A92 RID: 27282 RVA: 0x002AC153 File Offset: 0x002AA353
		private void OnHitPlayer()
		{
			if (this._consumed)
			{
				return;
			}
			this._consumed = true;
			this.TryApplyPlayerHit();
			this.DestroyWithFade();
		}

		// Token: 0x06006A93 RID: 27283 RVA: 0x002AC174 File Offset: 0x002AA374
		private bool TryApplyPlayerHit()
		{
			if (TaserOfficer.MustAbortAttack)
			{
				return false;
			}
			PlayerStunExecutor instance = PlayerStunExecutor.Instance;
			if (PoolWaterRecovery.IsPlayerInWater((this._target != null) ? this._target : ((instance != null) ? instance.transform : null)))
			{
				return false;
			}
			if (this._hitVariants != null && this._hitVariants.Length != 0 && instance != null)
			{
				PlayerActionEntry playerActionEntry = this._hitVariants[Random.Range(0, this._hitVariants.Length)];
				if (this._debugLogs)
				{
					Debug.Log(string.Format("[TaserBolt] HIT player → stun '{0}' after {1:F1}m", (playerActionEntry != null) ? playerActionEntry.DisplayName : "<null>", this._traveled));
				}
				instance.PlayStun(playerActionEntry);
				return instance.IsStunned;
			}
			if (this._debugLogs)
			{
				Debug.LogWarning("[TaserBolt] HIT player but hitVariants empty or PlayerStunExecutor missing.");
			}
			return false;
		}

		// Token: 0x06006A94 RID: 27284 RVA: 0x002AC244 File Offset: 0x002AA444
		private void Fizzle()
		{
			this._consumed = true;
			this.DestroyWithFade();
		}

		// Token: 0x06006A95 RID: 27285 RVA: 0x002AC254 File Offset: 0x002AA454
		private void DestroyWithFade()
		{
			AudioSource component = base.GetComponent<AudioSource>();
			if (component != null && component.isPlaying)
			{
				GameObject gameObject = new GameObject("TaserBoltAudioFade");
				gameObject.transform.position = base.transform.position;
				AudioSource audioSource = gameObject.AddComponent<AudioSource>();
				audioSource.clip = component.clip;
				audioSource.loop = false;
				audioSource.spatialBlend = component.spatialBlend;
				audioSource.rolloffMode = component.rolloffMode;
				audioSource.minDistance = component.minDistance;
				audioSource.maxDistance = component.maxDistance;
				audioSource.volume = component.volume;
				audioSource.dopplerLevel = component.dopplerLevel;
				audioSource.spread = component.spread;
				audioSource.priority = component.priority;
				audioSource.pitch = component.pitch;
				audioSource.timeSamples = component.timeSamples;
				audioSource.Play();
				gameObject.AddComponent<AudioFadeOutAndDestroy>().Begin(audioSource, 0.18f);
			}
			Object.Destroy(base.gameObject);
		}

		// Token: 0x06006A96 RID: 27286 RVA: 0x002AC354 File Offset: 0x002AA554
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = new Color(0.4f, 0.8f, 1f, 0.5f);
			Gizmos.DrawWireSphere(base.transform.position, (this._hitRadius > 0f) ? this._hitRadius : 0.4f);
		}

		// Token: 0x04006D39 RID: 27961
		private Vector3 _direction;

		// Token: 0x04006D3A RID: 27962
		private float _speed;

		// Token: 0x04006D3B RID: 27963
		private float _maxDistance;

		// Token: 0x04006D3C RID: 27964
		private float _hitRadius;

		// Token: 0x04006D3D RID: 27965
		private LayerMask _hitMask;

		// Token: 0x04006D3E RID: 27966
		private LayerMask _obstacleMask;

		// Token: 0x04006D3F RID: 27967
		private PlayerActionEntry[] _hitVariants;

		// Token: 0x04006D40 RID: 27968
		private bool _debugLogs;

		// Token: 0x04006D41 RID: 27969
		private CapsuleCollider _targetCapsule;

		// Token: 0x04006D42 RID: 27970
		private Transform _target;

		// Token: 0x04006D43 RID: 27971
		private float _traveled;

		// Token: 0x04006D44 RID: 27972
		private bool _consumed;

		// Token: 0x04006D45 RID: 27973
		private static readonly Collider[] _hitBuffer = new Collider[8];

		// Token: 0x04006D46 RID: 27974
		private static readonly RaycastHit[] _sweepBuffer = new RaycastHit[32];

		// Token: 0x04006D47 RID: 27975
		private float _audioTargetVolume;

		// Token: 0x04006D48 RID: 27976
		private float _audioFadeInTime;

		// Token: 0x04006D49 RID: 27977
		private float _audioFadeInElapsed;

		// Token: 0x02000DFD RID: 3581
		private sealed class HitDistanceComparer : IComparer<RaycastHit>
		{
			// Token: 0x06006A99 RID: 27289 RVA: 0x002AC3C4 File Offset: 0x002AA5C4
			public int Compare(RaycastHit a, RaycastHit b)
			{
				return a.distance.CompareTo(b.distance);
			}

			// Token: 0x04006D4A RID: 27978
			public static readonly TaserBoltProjectile.HitDistanceComparer Instance = new TaserBoltProjectile.HitDistanceComparer();
		}
	}
}
