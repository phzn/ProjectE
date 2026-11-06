using System;
using System.Collections.Generic;
using Features.Detection;
using Features.Inventory.Actions;
using Features.Rank;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using UnityEngine;
using VehicleSpawnPlugin;

namespace Features.Vehicles.Traffic
{
	// Token: 0x02000B9B RID: 2971
	[DisallowMultipleComponent]
	public class CarRoofCarrier : MonoBehaviour
	{
		// Token: 0x06005518 RID: 21784 RVA: 0x00210440 File Offset: 0x0020E640
		public static void ApplyWorldOriginShiftToAll(Vector3 delta)
		{
			foreach (CarRoofCarrier carRoofCarrier in CarRoofCarrier.OriginInstances)
			{
				if (carRoofCarrier != null)
				{
					carRoofCarrier.ApplyWorldOriginShift(delta);
				}
			}
		}

		// Token: 0x06005519 RID: 21785 RVA: 0x0021049C File Offset: 0x0020E69C
		private void OnDestroy()
		{
			CarRoofCarrier.OriginInstances.Remove(this);
		}

		// Token: 0x1400008D RID: 141
		// (add) Token: 0x0600551A RID: 21786 RVA: 0x002104AC File Offset: 0x0020E6AC
		// (remove) Token: 0x0600551B RID: 21787 RVA: 0x002104E0 File Offset: 0x0020E6E0
		public static event Action<VehicleAI, Transform> OnPlayerMounted;

		// Token: 0x1400008E RID: 142
		// (add) Token: 0x0600551C RID: 21788 RVA: 0x00210514 File Offset: 0x0020E714
		// (remove) Token: 0x0600551D RID: 21789 RVA: 0x00210548 File Offset: 0x0020E748
		public static event Action<VehicleAI, Transform> OnPlayerDismounted;

		// Token: 0x17000E39 RID: 3641
		// (get) Token: 0x0600551E RID: 21790 RVA: 0x0021057B File Offset: 0x0020E77B
		// (set) Token: 0x0600551F RID: 21791 RVA: 0x00210582 File Offset: 0x0020E782
		public static bool IsPlayerMounted { get; private set; }

		// Token: 0x17000E3A RID: 3642
		// (get) Token: 0x06005520 RID: 21792 RVA: 0x0021058A File Offset: 0x0020E78A
		public static Transform MountedVehicleRoot
		{
			get
			{
				if (!(CarRoofCarrier._activeCarrier != null) || !CarRoofCarrier._activeCarrier.HasRider)
				{
					return null;
				}
				return CarRoofCarrier._activeCarrier._vehicleRoot;
			}
		}

		// Token: 0x06005521 RID: 21793 RVA: 0x002105B4 File Offset: 0x0020E7B4
		public static bool IsCarSurfing(Transform player)
		{
			return player != null && CarRoofCarrier._activeCarrier != null && CarRoofCarrier._activeCarrier._rider != null && CarRoofCarrier._activeCarrier._rideLease != null && CarRoofCarrier._activeCarrier._rideLease.IsActive && (player == CarRoofCarrier._activeCarrier._rider || player.IsChildOf(CarRoofCarrier._activeCarrier._rider) || CarRoofCarrier._activeCarrier._rider.IsChildOf(player));
		}

		// Token: 0x06005522 RID: 21794 RVA: 0x00210640 File Offset: 0x0020E840
		public static void ResumeAfterExternalPose(Transform player)
		{
			if (!CarRoofCarrier.IsCarSurfing(player))
			{
				return;
			}
			CarRoofCarrier activeCarrier = CarRoofCarrier._activeCarrier;
			activeCarrier._lastVehiclePosition = ((activeCarrier._vehicleRb != null) ? activeCarrier._vehicleRb.position : activeCarrier._vehicleRoot.position);
			activeCarrier._lastVehicleRotation = ((activeCarrier._vehicleRb != null) ? activeCarrier._vehicleRb.rotation : activeCarrier._vehicleRoot.rotation);
		}

		// Token: 0x17000E3B RID: 3643
		// (get) Token: 0x06005523 RID: 21795 RVA: 0x002106B4 File Offset: 0x0020E8B4
		public bool CanStartNewRide
		{
			get
			{
				this.RefreshSpawnGeneration();
				return this._vehicle != null && !this._rideConsumed;
			}
		}

		// Token: 0x06005524 RID: 21796 RVA: 0x002106D5 File Offset: 0x0020E8D5
		public bool TryConsumeRide()
		{
			if (!this.CanStartNewRide)
			{
				return false;
			}
			this._rideConsumed = true;
			return true;
		}

		// Token: 0x06005525 RID: 21797 RVA: 0x002106EC File Offset: 0x0020E8EC
		private void RefreshSpawnGeneration()
		{
			if (this._vehicle == null || this._observedSpawnGeneration == this._vehicle.SpawnGeneration)
			{
				return;
			}
			this._observedSpawnGeneration = this._vehicle.SpawnGeneration;
			this._rideConsumed = false;
			this._remountLockUntil = -999f;
		}

		// Token: 0x06005526 RID: 21798 RVA: 0x0021073E File Offset: 0x0020E93E
		public void LockRemount(float seconds)
		{
			this._remountLockUntil = Time.time + seconds;
		}

		// Token: 0x17000E3C RID: 3644
		// (get) Token: 0x06005527 RID: 21799 RVA: 0x0021074D File Offset: 0x0020E94D
		public bool HasRider
		{
			get
			{
				return this._rider != null;
			}
		}

		// Token: 0x06005528 RID: 21800 RVA: 0x0021075B File Offset: 0x0020E95B
		public void ApplyWorldOriginShift(Vector3 delta)
		{
			this._lastVehiclePosition += delta;
		}

		// Token: 0x06005529 RID: 21801 RVA: 0x00210770 File Offset: 0x0020E970
		private void Awake()
		{
			CarRoofCarrier.OriginInstances.Add(this);
			this._vehicle = base.GetComponentInParent<VehicleAI>();
			this._vehicleRoot = ((this._vehicle != null) ? this._vehicle.transform : base.transform.root);
			if (this._vehicleRoot != null)
			{
				this._bodyCollider = this._vehicleRoot.GetComponent<Collider>();
				this._vehicleRb = this._vehicleRoot.GetComponent<Rigidbody>();
			}
			BoxCollider[] components = base.GetComponents<BoxCollider>();
			for (int i = 0; i < components.Length; i++)
			{
				if (components[i] != null && components[i].isTrigger)
				{
					this._roofZone = components[i];
					return;
				}
			}
		}

		// Token: 0x0600552A RID: 21802 RVA: 0x00210828 File Offset: 0x0020EA28
		public bool Mount(Transform riderRoot, bool seat = true)
		{
			if (GuidedRankGameplayPolicy.CarInteractionsBlocked)
			{
				return false;
			}
			if (riderRoot == null || this._vehicle == null)
			{
				return false;
			}
			if (this._rider == riderRoot)
			{
				return true;
			}
			if (!this.CanStartNewRide)
			{
				return false;
			}
			if (!CarInteractionActivityContext.TryAcquire("CarSurf#" + base.GetInstanceID().ToString(), this, ref this._rideLease))
			{
				return false;
			}
			this._rideConsumed = true;
			this._rider = riderRoot;
			this._riderRb = riderRoot.GetComponent<Rigidbody>();
			this._riderInput = riderRoot.GetComponentInChildren<vThirdPersonInput>();
			if (seat)
			{
				this.SeatOnRoof(riderRoot);
			}
			if (riderRoot.parent == this._vehicleRoot)
			{
				riderRoot.SetParent(null, true);
			}
			this._lastVehiclePosition = ((this._vehicleRb != null) ? this._vehicleRb.position : this._vehicleRoot.position);
			this._lastVehicleRotation = ((this._vehicleRb != null) ? this._vehicleRb.rotation : this._vehicleRoot.rotation);
			this._vehicle.HasPassenger = true;
			this._vehicle.PassengerRoot = riderRoot;
			CarRoofCarrier.IsPlayerMounted = true;
			CarRoofCarrier._activeCarrier = this;
			this._rideLocalPosAtMount = this._vehicleRoot.InverseTransformPoint(riderRoot.position);
			this._rideFrameAtMount = Time.frameCount;
			this._emoteCarryActive = false;
			if (this._riderInput != null && !string.IsNullOrEmpty(this.surfCameraState))
			{
				this._riderInput.ChangeCameraState(this.surfCameraState, true);
			}
			Action<VehicleAI, Transform> onPlayerMounted = CarRoofCarrier.OnPlayerMounted;
			if (onPlayerMounted != null)
			{
				onPlayerMounted(this._vehicle, riderRoot);
			}
			if (VspDiag.Climb)
			{
				Debug.Log(string.Format("[ClimbDiag] Mount car='{0}' seatedPos={1:F3} frame={2}", this._vehicleRoot.name, riderRoot.position, Time.frameCount), this);
			}
			return true;
		}

		// Token: 0x0600552B RID: 21803 RVA: 0x00210A08 File Offset: 0x0020EC08
		private void SeatOnRoof(Transform riderRoot)
		{
			if (this._bodyCollider == null)
			{
				return;
			}
			Bounds bounds = this._bodyCollider.bounds;
			Vector3 position = riderRoot.position;
			Vector3 center = bounds.center;
			float num = bounds.extents.x * 0.8f;
			float num2 = bounds.extents.z * 0.8f;
			position.x = Mathf.Clamp(position.x, center.x - num, center.x + num);
			position.z = Mathf.Clamp(position.z, center.z - num2, center.z + num2);
			position.y = bounds.max.y + this.roofClearance;
			riderRoot.position = position;
			Rigidbody component = riderRoot.GetComponent<Rigidbody>();
			if (component != null && !component.isKinematic)
			{
				component.velocity = Vector3.zero;
			}
			if (VspDiag.Climb)
			{
				Debug.Log(string.Format("[ClimbDiag] SeatOnRoof roofY={0:F3} → seated {1:F3}", bounds.max.y, position), this);
			}
		}

		// Token: 0x0600552C RID: 21804 RVA: 0x00210B24 File Offset: 0x0020ED24
		public void Dismount()
		{
			CarInteractionActivityContext.Release(ref this._rideLease);
			if (this._rider == null)
			{
				return;
			}
			Transform rider = this._rider;
			Vector3 eulerAngles = rider.eulerAngles;
			rider.eulerAngles = new Vector3(0f, eulerAngles.y, 0f);
			if (this._vehicle != null)
			{
				this._vehicle.HasPassenger = false;
				this._vehicle.PassengerRoot = null;
			}
			CarRoofCarrier.IsPlayerMounted = false;
			if (CarRoofCarrier._activeCarrier == this)
			{
				CarRoofCarrier._activeCarrier = null;
			}
			this._lastDismountTime = Time.time;
			if (this._riderInput != null)
			{
				this._riderInput.ResetCameraState();
			}
			if (this._emoteCarryActive)
			{
				this.EndEmoteCarry();
			}
			Action<VehicleAI, Transform> onPlayerDismounted = CarRoofCarrier.OnPlayerDismounted;
			if (onPlayerDismounted != null)
			{
				onPlayerDismounted(this._vehicle, rider);
			}
			if (VspDiag.Climb)
			{
				Debug.Log(string.Format("[ClimbDiag] Dismount car='{0}' frame={1}", (this._vehicleRoot != null) ? this._vehicleRoot.name : base.name, Time.frameCount), this);
			}
			this._rider = null;
			this._riderRb = null;
			this._riderInput = null;
		}

		// Token: 0x0600552D RID: 21805 RVA: 0x00210C54 File Offset: 0x0020EE54
		private void Update()
		{
			if (this._rider == null)
			{
				return;
			}
			if (this.HasExternalPoseOwner())
			{
				return;
			}
			Vector3 vector = this._vehicleRoot.InverseTransformPoint(this._rider.position);
			float magnitude = new Vector2(vector.x, vector.z).magnitude;
			if (VspDiag.Climb)
			{
				Vector3 vector2 = vector - this._rideLocalPosAtMount;
				bool flag = this._riderInput != null && this._riderInput.cc != null && this._riderInput.cc.isGrounded;
				Debug.Log(string.Format("[ClimbDiag][RideDiag] f={0} dt={1} carSpeed={2:F2} localPos={3:F3} driftFromMount={4:F3} horiz={5:F2} grounded={6} riderWorld={7:F2} carWorld={8:F2}", new object[]
				{
					Time.frameCount,
					Time.frameCount - this._rideFrameAtMount,
					this._vehicle.CurrentSpeed,
					vector,
					vector2,
					magnitude,
					flag,
					this._rider.position,
					this._vehicleRoot.position
				}), this);
			}
			if (vector.y < -1f || this.IsOutsideRoofZone())
			{
				this.Dismount();
			}
		}

		// Token: 0x0600552E RID: 21806 RVA: 0x00210DA8 File Offset: 0x0020EFA8
		private bool IsOutsideRoofZone()
		{
			if (this._roofZone == null)
			{
				Vector3 vector = this._vehicleRoot.InverseTransformPoint(this._rider.position);
				return new Vector2(vector.x, vector.z).magnitude > this.dismountDistance;
			}
			Vector3 vector2 = this._roofZone.transform.InverseTransformPoint(this._rider.position) - this._roofZone.center;
			Vector3 vector3 = this._roofZone.size * 0.5f;
			return Mathf.Abs(vector2.x) > vector3.x + this.roofFootprintMargin || Mathf.Abs(vector2.z) > vector3.z + this.roofFootprintMargin || vector2.y < -vector3.y - 0.35f;
		}

		// Token: 0x0600552F RID: 21807 RVA: 0x00210E8C File Offset: 0x0020F08C
		private bool IsStandingInRoofZone(Transform rider)
		{
			if (this._roofZone == null || rider == null)
			{
				return false;
			}
			Vector3 vector = this._roofZone.transform.InverseTransformPoint(rider.position) - this._roofZone.center;
			Vector3 vector2 = this._roofZone.size * 0.5f;
			return Mathf.Abs(vector.x) <= vector2.x && Mathf.Abs(vector.z) <= vector2.z && vector.y >= -vector2.y - 0.1f && vector.y <= vector2.y + 0.25f;
		}

		// Token: 0x06005530 RID: 21808 RVA: 0x00210F44 File Offset: 0x0020F144
		private void FixedUpdate()
		{
			if (this._rider == null || this._vehicleRoot == null)
			{
				return;
			}
			if (this.HasExternalPoseOwner())
			{
				if (this._emoteCarryActive)
				{
					this.EndEmoteCarry();
				}
				this._lastVehiclePosition = ((this._vehicleRb != null) ? this._vehicleRb.position : this._vehicleRoot.position);
				this._lastVehicleRotation = ((this._vehicleRb != null) ? this._vehicleRb.rotation : this._vehicleRoot.rotation);
				return;
			}
			if (ExhibitionEventDirector.Instance != null && ExhibitionEventDirector.Instance.IsEventActive)
			{
				if (this._emoteCarryActive)
				{
					this.EndEmoteCarry();
				}
				return;
			}
			this.SyncEmoteCarryMode();
			if (this._emoteCarryActive)
			{
				this.ApplyRigidCarry();
				return;
			}
			this.ApplyCarry();
		}

		// Token: 0x06005531 RID: 21809 RVA: 0x00211020 File Offset: 0x0020F220
		private bool IsRiderInCustomAction()
		{
			return this._riderInput != null && this._riderInput.cc != null && this._riderInput.cc.customAction;
		}

		// Token: 0x06005532 RID: 21810 RVA: 0x00211058 File Offset: 0x0020F258
		private bool HasExternalPoseOwner()
		{
			return this._vehicleRoot != null && this._rider != null && (this._rider.IsChildOf(this._vehicleRoot) || DildoActionExecutor.IsAnchoredToMovingSupport(this._rider, this._vehicleRoot));
		}

		// Token: 0x06005533 RID: 21811 RVA: 0x002110AC File Offset: 0x0020F2AC
		private void SyncEmoteCarryMode()
		{
			if (this._riderInput == null)
			{
				return;
			}
			bool flag = this.IsRiderInCustomAction();
			if (flag && !this._emoteCarryActive)
			{
				this._riderInput.DisableOnAnimatorMove();
				this._riderLocalPos = this._vehicleRoot.InverseTransformPoint(this._rider.position);
				this._riderLocalRot = Quaternion.Inverse(this._vehicleRoot.rotation) * this._rider.rotation;
				this._emoteCarryActive = true;
				return;
			}
			if (!flag && this._emoteCarryActive)
			{
				this.EndEmoteCarry();
			}
		}

		// Token: 0x06005534 RID: 21812 RVA: 0x00211140 File Offset: 0x0020F340
		private void ApplyRigidCarry()
		{
			Vector3 vector = this._vehicleRoot.TransformPoint(this._riderLocalPos);
			Quaternion quaternion = this._vehicleRoot.rotation * this._riderLocalRot;
			if (this._riderRb != null && !this._riderRb.isKinematic)
			{
				this._riderRb.velocity = Vector3.zero;
				this._riderRb.position = vector;
				this._riderRb.rotation = quaternion;
				return;
			}
			this._rider.position = vector;
			this._rider.rotation = quaternion;
		}

		// Token: 0x06005535 RID: 21813 RVA: 0x002111D4 File Offset: 0x0020F3D4
		private void EndEmoteCarry()
		{
			if (this._riderInput != null)
			{
				this._riderInput.EnableOnAnimatorMove();
			}
			this._lastVehiclePosition = ((this._vehicleRb != null) ? this._vehicleRb.position : this._vehicleRoot.position);
			this._lastVehicleRotation = ((this._vehicleRb != null) ? this._vehicleRb.rotation : this._vehicleRoot.rotation);
			this._emoteCarryActive = false;
		}

		// Token: 0x06005536 RID: 21814 RVA: 0x0021125C File Offset: 0x0020F45C
		private void ApplyCarry()
		{
			Vector3 vector = ((this._vehicleRb != null) ? this._vehicleRb.position : this._vehicleRoot.position);
			Quaternion quaternion = ((this._vehicleRb != null) ? this._vehicleRb.rotation : this._vehicleRoot.rotation);
			Quaternion quaternion2 = quaternion * Quaternion.Inverse(this._lastVehicleRotation);
			Vector3 vector2 = vector + quaternion2 * (this._rider.position - this._lastVehiclePosition);
			if (this._riderRb != null && !this._riderRb.isKinematic)
			{
				this._riderRb.position = vector2;
				this._riderRb.rotation = quaternion2 * this._riderRb.rotation;
			}
			else
			{
				this._rider.position = vector2;
				this._rider.rotation = quaternion2 * this._rider.rotation;
			}
			this._lastVehiclePosition = vector;
			this._lastVehicleRotation = quaternion;
		}

		// Token: 0x06005537 RID: 21815 RVA: 0x00211367 File Offset: 0x0020F567
		private void OnDisable()
		{
			if (this._rider != null)
			{
				this.Dismount();
			}
			CarInteractionActivityContext.Release(ref this._rideLease);
		}

		// Token: 0x06005538 RID: 21816 RVA: 0x00211388 File Offset: 0x0020F588
		private void OnTriggerEnter(Collider other)
		{
			this.TryMountByPresence(other);
		}

		// Token: 0x06005539 RID: 21817 RVA: 0x00211388 File Offset: 0x0020F588
		private void OnTriggerStay(Collider other)
		{
			this.TryMountByPresence(other);
		}

		// Token: 0x0600553A RID: 21818 RVA: 0x00211394 File Offset: 0x0020F594
		private void TryMountByPresence(Collider other)
		{
			if (GuidedRankGameplayPolicy.CarInteractionsBlocked)
			{
				return;
			}
			if (!this.CanStartNewRide)
			{
				return;
			}
			if (!CarInteractionActivityContext.HasFreeHands)
			{
				return;
			}
			if (this._rider != null)
			{
				return;
			}
			if (other == null || !other.CompareTag("Player"))
			{
				return;
			}
			if (Time.time - this._lastDismountTime < this.remountCooldown)
			{
				return;
			}
			if (Time.time < this._remountLockUntil)
			{
				return;
			}
			Rigidbody attachedRigidbody = other.attachedRigidbody;
			if (attachedRigidbody == null)
			{
				return;
			}
			Transform transform = attachedRigidbody.transform;
			if (!this.IsStandingInRoofZone(transform))
			{
				return;
			}
			vThirdPersonInput componentInChildren = transform.GetComponentInChildren<vThirdPersonInput>();
			if (componentInChildren != null && componentInChildren.cc != null && componentInChildren.cc.customAction)
			{
				return;
			}
			if (CarRoofCarrier._activeCarrier != null && CarRoofCarrier._activeCarrier != this)
			{
				CarRoofCarrier._activeCarrier.Dismount();
			}
			if (this.Mount(transform, false) && VspDiag.Climb)
			{
				Debug.Log(string.Format("[ClimbDiag] Mount-by-presence car='{0}' rider='{1}' frame={2}", (this._vehicleRoot != null) ? this._vehicleRoot.name : base.name, transform.name, Time.frameCount), this);
			}
		}

		// Token: 0x04005540 RID: 21824
		private static readonly HashSet<CarRoofCarrier> OriginInstances = new HashSet<CarRoofCarrier>();

		// Token: 0x04005544 RID: 21828
		private static CarRoofCarrier _activeCarrier;

		// Token: 0x04005545 RID: 21829
		[Header("Camera")]
		[Tooltip("Camera state entered while surfing on the roof (must exist in the player's CameraStateList).")]
		[SerializeField]
		private string surfCameraState = "CarSurf";

		// Token: 0x04005546 RID: 21830
		[Header("Seating")]
		[Tooltip("Extra clearance (m) above the roof surface when seating the rider.")]
		[SerializeField]
		private float roofClearance = 0.05f;

		// Token: 0x04005547 RID: 21831
		[Header("Dismount")]
		[Tooltip("Fallback radius (m) from the car centre used only when the car has no body collider to measure a footprint against.")]
		[SerializeField]
		private float dismountDistance = 4f;

		// Token: 0x04005548 RID: 21832
		[Tooltip("Extra horizontal slack (m) added around the RoofZone trigger box before the rider counts as off the roof. Keep ≈ the player capsule radius so the edge of the roof still counts as aboard without fighting the mount-by-presence trigger.")]
		[SerializeField]
		private float roofFootprintMargin = 0.4f;

		// Token: 0x04005549 RID: 21833
		[Header("Mount by presence (2026-07-16)")]
		[Tooltip("Seconds after a dismount before the roof trigger may mount the player again (anti flip-flop while he is still inside the zone).")]
		[SerializeField]
		private float remountCooldown = 0.5f;

		// Token: 0x0400554A RID: 21834
		private VehicleAI _vehicle;

		// Token: 0x0400554B RID: 21835
		private Transform _vehicleRoot;

		// Token: 0x0400554C RID: 21836
		private Collider _bodyCollider;

		// Token: 0x0400554D RID: 21837
		private Rigidbody _vehicleRb;

		// Token: 0x0400554E RID: 21838
		private BoxCollider _roofZone;

		// Token: 0x0400554F RID: 21839
		private Transform _rider;

		// Token: 0x04005550 RID: 21840
		private Rigidbody _riderRb;

		// Token: 0x04005551 RID: 21841
		private vThirdPersonInput _riderInput;

		// Token: 0x04005552 RID: 21842
		private PlayerActivityLease _rideLease;

		// Token: 0x04005553 RID: 21843
		private Vector3 _rideLocalPosAtMount;

		// Token: 0x04005554 RID: 21844
		private int _rideFrameAtMount;

		// Token: 0x04005555 RID: 21845
		private Vector3 _lastVehiclePosition;

		// Token: 0x04005556 RID: 21846
		private Quaternion _lastVehicleRotation;

		// Token: 0x04005557 RID: 21847
		private bool _emoteCarryActive;

		// Token: 0x04005558 RID: 21848
		private Vector3 _riderLocalPos;

		// Token: 0x04005559 RID: 21849
		private Quaternion _riderLocalRot;

		// Token: 0x0400555A RID: 21850
		private float _lastDismountTime = -999f;

		// Token: 0x0400555B RID: 21851
		private float _remountLockUntil = -999f;

		// Token: 0x0400555C RID: 21852
		private uint _observedSpawnGeneration;

		// Token: 0x0400555D RID: 21853
		private bool _rideConsumed;
	}
}
