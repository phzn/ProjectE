using System;
using System.Collections.Generic;
using Invector.vCharacterController.vActions;
using UnityEngine;

namespace Features.Vehicles.Spawning
{
	// Token: 0x02000B82 RID: 2946
	[DisallowMultipleComponent]
	public sealed class SpawnedVehicleGroundSafety : MonoBehaviour
	{
		// Token: 0x060053D7 RID: 21463 RVA: 0x0020867C File Offset: 0x0020687C
		public static void ApplyWorldOriginShiftToAll(Vector3 delta)
		{
			foreach (SpawnedVehicleGroundSafety spawnedVehicleGroundSafety in SpawnedVehicleGroundSafety.OriginInstances)
			{
				if (spawnedVehicleGroundSafety != null)
				{
					spawnedVehicleGroundSafety.ApplyWorldOriginShift(delta);
				}
			}
		}

		// Token: 0x17000DFC RID: 3580
		// (get) Token: 0x060053D8 RID: 21464 RVA: 0x002086D8 File Offset: 0x002068D8
		// (set) Token: 0x060053D9 RID: 21465 RVA: 0x002086E0 File Offset: 0x002068E0
		public int RecoveryCount { get; private set; }

		// Token: 0x17000DFD RID: 3581
		// (get) Token: 0x060053DA RID: 21466 RVA: 0x002086E9 File Offset: 0x002068E9
		public Vector3 LastSafePosition
		{
			get
			{
				return this._lastSafePosition;
			}
		}

		// Token: 0x060053DB RID: 21467 RVA: 0x002086F1 File Offset: 0x002068F1
		private void Awake()
		{
			SpawnedVehicleGroundSafety.OriginInstances.Add(this);
			this.CacheReferences();
		}

		// Token: 0x060053DC RID: 21468 RVA: 0x00208705 File Offset: 0x00206905
		private void OnDestroy()
		{
			SpawnedVehicleGroundSafety.OriginInstances.Remove(this);
		}

		// Token: 0x060053DD RID: 21469 RVA: 0x00208714 File Offset: 0x00206914
		public void ApplyWorldOriginShift(Vector3 delta)
		{
			if (!this._initialized)
			{
				return;
			}
			this._spawnFallbackPosition += delta;
			this._lastSafePosition += delta;
			this._previousBodyPosition += delta;
		}

		// Token: 0x060053DE RID: 21470 RVA: 0x00208760 File Offset: 0x00206960
		public void Initialize(Vector3 spawnPosition, Quaternion spawnRotation)
		{
			this.Initialize(spawnPosition, spawnRotation, new Vector3(1.35f, 1.95f, 2.65f), -1);
		}

		// Token: 0x060053DF RID: 21471 RVA: 0x00208784 File Offset: 0x00206984
		public void Initialize(Vector3 spawnPosition, Quaternion spawnRotation, Vector3 clearanceSize, LayerMask supportMask)
		{
			SpawnedVehicleGroundSafety.OriginInstances.Add(this);
			this.CacheReferences();
			if (this._body != null)
			{
				this._parkedCollisionDetectionMode = this._body.collisionDetectionMode;
			}
			this._footprintHalfWidth = Mathf.Max(0.15f, clearanceSize.x * 0.42f);
			this._footprintHalfLength = Mathf.Max(0.25f, clearanceSize.z * 0.42f);
			this._supportMask = supportMask;
			this._spawnFallbackPosition = spawnPosition;
			this._spawnFallbackRotation = SpawnedVehicleGroundSafety.FlattenRotation(spawnRotation);
			this._previousBodyPosition = ((this._body != null) ? this._body.position : base.transform.position);
			this._lastSupportedTime = Time.time;
			this._initialized = true;
			Physics.SyncTransforms();
			RaycastHit raycastHit;
			float num;
			if (this.TryFindSupport(this._previousBodyPosition, out raycastHit, out num))
			{
				this.RecordSafePose(this._previousBodyPosition, base.transform.rotation, raycastHit.point.y, num);
				return;
			}
			this._lastSafePosition = spawnPosition;
			this._lastSafeRotation = this._spawnFallbackRotation;
			this._lastSafeGroundOffset = 0.35f;
			this._hasSafePose = false;
		}

		// Token: 0x060053E0 RID: 21472 RVA: 0x002088B4 File Offset: 0x00206AB4
		public bool PrepareForMount()
		{
			this.CacheReferences();
			if (!this._initialized || this._body == null)
			{
				return true;
			}
			Physics.SyncTransforms();
			Vector3 position = this._body.position;
			float num = Vector3.Angle(this._body.rotation * Vector3.up, Vector3.up);
			RaycastHit raycastHit;
			float num2;
			bool flag = this.HasFullFootprintSupport(position, SpawnedVehicleGroundSafety.FlattenRotation(this._body.rotation), out raycastHit, out num2);
			if (num <= 15f && flag)
			{
				return true;
			}
			Vector3 vector;
			Quaternion quaternion;
			if (!this.TryResolveRecoveryPose(out vector, out quaternion))
			{
				return false;
			}
			this._body.position = vector;
			this._body.rotation = quaternion;
			if (!this._body.isKinematic)
			{
				this._body.velocity = Vector3.zero;
				this._body.angularVelocity = Vector3.zero;
			}
			this._previousBodyPosition = vector;
			Physics.SyncTransforms();
			return true;
		}

		// Token: 0x060053E1 RID: 21473 RVA: 0x0020899C File Offset: 0x00206B9C
		public void PrepareForDynamicSimulation()
		{
			if (this._body == null)
			{
				this._body = base.GetComponent<Rigidbody>();
			}
			if (this._body != null && !this._body.isKinematic && this._body.collisionDetectionMode != CollisionDetectionMode.ContinuousDynamic)
			{
				this._body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
			}
		}

		// Token: 0x060053E2 RID: 21474 RVA: 0x002089F8 File Offset: 0x00206BF8
		public void PrepareForParking()
		{
			if (this._body == null)
			{
				this._body = base.GetComponent<Rigidbody>();
			}
			if (this._body != null && this._body.collisionDetectionMode != this._parkedCollisionDetectionMode)
			{
				this._body.collisionDetectionMode = this._parkedCollisionDetectionMode;
			}
		}

		// Token: 0x060053E3 RID: 21475 RVA: 0x00208A54 File Offset: 0x00206C54
		private void FixedUpdate()
		{
			if (!this._initialized || this._body == null)
			{
				return;
			}
			if (!this.IsMounted())
			{
				this._wasMounted = false;
				this._previousBodyPosition = this._body.position;
				if (this._body.isKinematic)
				{
					this.PrepareForParking();
				}
				return;
			}
			this.PrepareForDynamicSimulation();
			Vector3 position = this._body.position;
			Vector3 vector = Vector3.ProjectOnPlane(position - this._previousBodyPosition, Vector3.up);
			Vector3 vector2 = Vector3.ProjectOnPlane(this._body.velocity, Vector3.up);
			Vector3 vector3 = ((vector2.sqrMagnitude > 0.04f) ? vector2 : vector);
			if (vector3.sqrMagnitude > 0.0004f)
			{
				this._lastTravelDirection = vector3.normalized;
			}
			this._previousBodyPosition = position;
			if (!this._wasMounted)
			{
				this._wasMounted = true;
				this._lastSupportedTime = Time.time;
				this._stableSupportSince = -1f;
				this._nextCheckpointTime = Time.time;
			}
			RaycastHit raycastHit;
			float num;
			if (this.TryFindStableSupport(position, out raycastHit, out num))
			{
				this._lastSupportedTime = Time.time;
				if (this._stableSupportSince < 0f)
				{
					this._stableSupportSince = Time.time;
				}
				if (Mathf.Abs(this._body.velocity.y) <= 0.5f && Time.time - this._stableSupportSince >= 0.15f && Time.time >= this._nextCheckpointTime)
				{
					this.RecordSafePose(position, this._body.rotation, raycastHit.point.y, num);
					this._nextCheckpointTime = Time.time + 0.2f;
				}
				return;
			}
			this._stableSupportSince = -1f;
			if (Time.time < this._recoverySuppressedUntil || !this._hasSafePose)
			{
				return;
			}
			float num2 = Time.time - this._lastSupportedTime;
			float num3 = this._lastSafePosition.y - position.y;
			if (num2 < 0.35f || num3 < 1.5f || this._body.velocity.y > -2.5f)
			{
				return;
			}
			this.RecoverFromFall(position, num3);
		}

		// Token: 0x060053E4 RID: 21476 RVA: 0x00208C68 File Offset: 0x00206E68
		private void RecoverFromFall(Vector3 unsafePosition, float verticalDrop)
		{
			Vector3 vector;
			Quaternion quaternion;
			if (!this.TryResolveRecoveryPose(out vector, out quaternion))
			{
				this._recoverySuppressedUntil = Time.time + 0.75f;
				this._hasSafePose = false;
				Debug.LogError("[SpawnedVehicleGroundSafety] Could not recover '" + base.name + "': no fully supported checkpoint is currently loaded.", this);
				return;
			}
			Vector3 vector2 = vector;
			vector.y += 0.08f;
			this._body.position = vector;
			this._body.rotation = quaternion;
			if (!this._body.isKinematic)
			{
				this._body.velocity = Vector3.zero;
				this._body.angularVelocity = Vector3.zero;
				this._body.WakeUp();
			}
			Physics.SyncTransforms();
			int recoveryCount = this.RecoveryCount;
			this.RecoveryCount = recoveryCount + 1;
			this._lastSafePosition = vector2;
			this._lastSafeRotation = quaternion;
			this._previousBodyPosition = vector;
			this._lastSupportedTime = Time.time;
			this._recoverySuppressedUntil = Time.time + 0.75f;
			this._nextCheckpointTime = this._recoverySuppressedUntil;
			Debug.LogWarning("[SpawnedVehicleGroundSafety] Recovered '" + base.name + "' from a " + string.Format("{0:F2} m unsupported fall ({1} -> {2}).", verticalDrop, unsafePosition, vector), this);
		}

		// Token: 0x060053E5 RID: 21477 RVA: 0x00208DA0 File Offset: 0x00206FA0
		private bool TryResolveRecoveryPose(out Vector3 target, out Quaternion targetRotation)
		{
			targetRotation = (this._hasSafePose ? this._lastSafeRotation : this._spawnFallbackRotation);
			Vector3 vector = (this._hasSafePose ? this._lastSafePosition : this._spawnFallbackPosition);
			if (this.TryMakeSupportedPose(vector, targetRotation, out target))
			{
				return true;
			}
			Vector3 vector2 = ((this._lastTravelDirection.sqrMagnitude > 0.01f) ? (-this._lastTravelDirection.normalized) : (-(targetRotation * Vector3.forward)));
			vector2 = Vector3.ProjectOnPlane(vector2, Vector3.up).normalized;
			Vector3 normalized = Vector3.Cross(Vector3.up, vector2).normalized;
			foreach (float num in new float[] { 0.9f, 1.8f, 2.6999998f })
			{
				Vector3 vector3 = vector + vector2 * num;
				if (this.TryMakeSupportedPose(vector3, targetRotation, out target) || this.TryMakeSupportedPose(vector3 + normalized * num * 0.75f, targetRotation, out target) || this.TryMakeSupportedPose(vector3 - normalized * num * 0.75f, targetRotation, out target))
				{
					return true;
				}
			}
			targetRotation = this._spawnFallbackRotation;
			return this.TryMakeSupportedPose(this._spawnFallbackPosition, targetRotation, out target);
		}

		// Token: 0x060053E6 RID: 21478 RVA: 0x00208F18 File Offset: 0x00207118
		private bool TryMakeSupportedPose(Vector3 candidate, Quaternion rotation, out Vector3 supportedPose)
		{
			supportedPose = candidate;
			RaycastHit raycastHit;
			float num;
			if (!this.HasFullFootprintSupport(candidate, rotation, out raycastHit, out num))
			{
				return false;
			}
			supportedPose.y = raycastHit.point.y + Mathf.Clamp(this._lastSafeGroundOffset, 0.05f, 0.95f);
			return true;
		}

		// Token: 0x060053E7 RID: 21479 RVA: 0x00208F68 File Offset: 0x00207168
		private void RecordSafePose(Vector3 position, Quaternion rotation, float supportY, float groundGap)
		{
			float num = Mathf.Max(0.05f, position.y - supportY);
			if (groundGap >= 0f)
			{
				num = Mathf.Max(num, groundGap);
			}
			this._lastSafeGroundOffset = Mathf.Clamp(num, 0.05f, 0.95f);
			this._lastSafePosition = position;
			this._lastSafePosition.y = supportY + this._lastSafeGroundOffset;
			this._lastSafeRotation = SpawnedVehicleGroundSafety.FlattenRotation(rotation);
			this._hasSafePose = true;
		}

		// Token: 0x060053E8 RID: 21480 RVA: 0x00208FE0 File Offset: 0x002071E0
		private bool TryFindSupport(Vector3 rootPosition, out RaycastHit nearest, out float groundGap)
		{
			int num = Physics.RaycastNonAlloc(rootPosition + Vector3.up * 1.5f, Vector3.down, this._supportHits, 4f, this._supportMask, QueryTriggerInteraction.Ignore);
			nearest = default(RaycastHit);
			float num2 = float.PositiveInfinity;
			for (int i = 0; i < num; i++)
			{
				RaycastHit raycastHit = this._supportHits[i];
				this._supportHits[i] = default(RaycastHit);
				if (!(raycastHit.collider == null) && raycastHit.normal.y >= 0.45f && !raycastHit.collider.transform.IsChildOf(base.transform) && raycastHit.distance < num2)
				{
					num2 = raycastHit.distance;
					nearest = raycastHit;
				}
			}
			if (nearest.collider == null)
			{
				groundGap = float.PositiveInfinity;
				return false;
			}
			groundGap = rootPosition.y - nearest.point.y;
			return groundGap >= -0.02f;
		}

		// Token: 0x060053E9 RID: 21481 RVA: 0x002090E8 File Offset: 0x002072E8
		private bool TryFindStableSupport(Vector3 rootPosition, out RaycastHit centerSupport, out float centerGap)
		{
			Quaternion quaternion = SpawnedVehicleGroundSafety.FlattenRotation(this._body.rotation);
			return this.HasFullFootprintSupport(rootPosition, quaternion, out centerSupport, out centerGap) && Vector3.Angle(this._body.rotation * Vector3.up, centerSupport.normal) <= 25f;
		}

		// Token: 0x060053EA RID: 21482 RVA: 0x00209140 File Offset: 0x00207340
		private bool HasFullFootprintSupport(Vector3 rootPosition, Quaternion rotation, out RaycastHit centerSupport, out float centerGap)
		{
			if (!this.TryFindSupport(rootPosition, out centerSupport, out centerGap) || centerGap > 0.95f)
			{
				return false;
			}
			Vector3 vector = rotation * Vector3.right * this._footprintHalfWidth;
			Vector3 vector2 = rotation * Vector3.forward * this._footprintHalfLength;
			Vector3[] array = new Vector3[]
			{
				rootPosition + vector + vector2,
				rootPosition + vector - vector2,
				rootPosition - vector + vector2,
				rootPosition - vector - vector2
			};
			float num = centerSupport.point.y;
			float num2 = centerSupport.point.y;
			for (int i = 0; i < array.Length; i++)
			{
				RaycastHit raycastHit;
				float num3;
				if (!this.TryFindSupport(array[i], out raycastHit, out num3) || num3 > 0.95f)
				{
					return false;
				}
				num = Mathf.Min(num, raycastHit.point.y);
				num2 = Mathf.Max(num2, raycastHit.point.y);
			}
			return num2 - num <= 0.35f;
		}

		// Token: 0x060053EB RID: 21483 RVA: 0x0020926C File Offset: 0x0020746C
		private bool IsMounted()
		{
			return (this._standardVehicle != null && this._standardVehicle.IsBeingRidden) || (this._skateboard != null && this._skateboard.IsBeingUsed) || (this._rollers != null && this._rollers.IsBeingUsed);
		}

		// Token: 0x060053EC RID: 21484 RVA: 0x002092CC File Offset: 0x002074CC
		private void CacheReferences()
		{
			if (this._body == null)
			{
				this._body = base.GetComponent<Rigidbody>();
			}
			if (this._standardVehicle == null)
			{
				this._standardVehicle = base.GetComponentInChildren<vVehicleController>(true);
			}
			if (this._skateboard == null)
			{
				this._skateboard = base.GetComponentInChildren<SkateboardInteractable>(true);
			}
			if (this._rollers == null)
			{
				this._rollers = base.GetComponentInChildren<SimpleRollerSkatesInteractable>(true);
			}
		}

		// Token: 0x060053ED RID: 21485 RVA: 0x00209344 File Offset: 0x00207544
		private static Quaternion FlattenRotation(Quaternion rotation)
		{
			Vector3 vector = Vector3.ProjectOnPlane(rotation * Vector3.forward, Vector3.up);
			if (vector.sqrMagnitude <= 0.0001f)
			{
				return Quaternion.identity;
			}
			return Quaternion.LookRotation(vector.normalized, Vector3.up);
		}

		// Token: 0x04005440 RID: 21568
		private static readonly HashSet<SpawnedVehicleGroundSafety> OriginInstances = new HashSet<SpawnedVehicleGroundSafety>();

		// Token: 0x04005441 RID: 21569
		private const float ProbeStartHeight = 1.5f;

		// Token: 0x04005442 RID: 21570
		private const float ProbeDistance = 4f;

		// Token: 0x04005443 RID: 21571
		private const float MaximumStableGroundGap = 1.35f;

		// Token: 0x04005444 RID: 21572
		private const float MaximumCheckpointGroundGap = 0.95f;

		// Token: 0x04005445 RID: 21573
		private const float MinimumSupportNormalY = 0.45f;

		// Token: 0x04005446 RID: 21574
		private const float UnsupportedGraceSeconds = 0.35f;

		// Token: 0x04005447 RID: 21575
		private const float RecoveryDrop = 1.5f;

		// Token: 0x04005448 RID: 21576
		private const float MinimumRecoveryFallSpeed = 2.5f;

		// Token: 0x04005449 RID: 21577
		private const float RecoveryRetreat = 0.9f;

		// Token: 0x0400544A RID: 21578
		private const float RecoveryLift = 0.08f;

		// Token: 0x0400544B RID: 21579
		private const float RecoveryCooldownSeconds = 0.75f;

		// Token: 0x0400544C RID: 21580
		private const float StableCheckpointInterval = 0.2f;

		// Token: 0x0400544D RID: 21581
		private const float StableCheckpointDwell = 0.15f;

		// Token: 0x0400544E RID: 21582
		private const float MaximumCheckpointTilt = 25f;

		// Token: 0x0400544F RID: 21583
		private const float MaximumCheckpointVerticalSpeed = 0.5f;

		// Token: 0x04005450 RID: 21584
		private const float MaximumFootprintHeightDelta = 0.35f;

		// Token: 0x04005451 RID: 21585
		private readonly RaycastHit[] _supportHits = new RaycastHit[24];

		// Token: 0x04005452 RID: 21586
		private Rigidbody _body;

		// Token: 0x04005453 RID: 21587
		private vVehicleController _standardVehicle;

		// Token: 0x04005454 RID: 21588
		private SkateboardInteractable _skateboard;

		// Token: 0x04005455 RID: 21589
		private SimpleRollerSkatesInteractable _rollers;

		// Token: 0x04005456 RID: 21590
		private bool _initialized;

		// Token: 0x04005457 RID: 21591
		private bool _wasMounted;

		// Token: 0x04005458 RID: 21592
		private bool _hasSafePose;

		// Token: 0x04005459 RID: 21593
		private Vector3 _spawnFallbackPosition;

		// Token: 0x0400545A RID: 21594
		private Quaternion _spawnFallbackRotation;

		// Token: 0x0400545B RID: 21595
		private Vector3 _lastSafePosition;

		// Token: 0x0400545C RID: 21596
		private Quaternion _lastSafeRotation;

		// Token: 0x0400545D RID: 21597
		private float _lastSafeGroundOffset;

		// Token: 0x0400545E RID: 21598
		private float _footprintHalfWidth = 0.57f;

		// Token: 0x0400545F RID: 21599
		private float _footprintHalfLength = 1.11f;

		// Token: 0x04005460 RID: 21600
		private LayerMask _supportMask = -1;

		// Token: 0x04005461 RID: 21601
		private CollisionDetectionMode _parkedCollisionDetectionMode;

		// Token: 0x04005462 RID: 21602
		private Vector3 _lastTravelDirection;

		// Token: 0x04005463 RID: 21603
		private Vector3 _previousBodyPosition;

		// Token: 0x04005464 RID: 21604
		private float _lastSupportedTime;

		// Token: 0x04005465 RID: 21605
		private float _stableSupportSince = -1f;

		// Token: 0x04005466 RID: 21606
		private float _nextCheckpointTime;

		// Token: 0x04005467 RID: 21607
		private float _recoverySuppressedUntil;
	}
}
