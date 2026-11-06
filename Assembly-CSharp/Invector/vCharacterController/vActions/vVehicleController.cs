using System;
using Features.DebugTools;
using Features.Detection;
using Features.PhoneCompanion;
using Features.Rank;
using Features.Vehicles.Spawning;
using Invector.vCamera;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Interactions;
using RayznGames.VehicleSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Invector.vCharacterController.vActions
{
	// Token: 0x02000709 RID: 1801
	public class vVehicleController : MonoBehaviour
	{
		// Token: 0x14000051 RID: 81
		// (add) Token: 0x06002E49 RID: 11849 RVA: 0x001044CC File Offset: 0x001026CC
		// (remove) Token: 0x06002E4A RID: 11850 RVA: 0x00104500 File Offset: 0x00102700
		public static event Action<vVehicleController, vThirdPersonController> OnAnyVehicleMounted;

		// Token: 0x14000052 RID: 82
		// (add) Token: 0x06002E4B RID: 11851 RVA: 0x00104534 File Offset: 0x00102734
		// (remove) Token: 0x06002E4C RID: 11852 RVA: 0x00104568 File Offset: 0x00102768
		public static event Action<vVehicleController, vThirdPersonController> OnAnyVehicleDismounted;

		// Token: 0x170008D4 RID: 2260
		// (get) Token: 0x06002E4D RID: 11853 RVA: 0x0010459B File Offset: 0x0010279B
		public bool IsBeingRidden
		{
			get
			{
				return this.isBeingRidden;
			}
		}

		// Token: 0x170008D5 RID: 2261
		// (get) Token: 0x06002E4E RID: 11854 RVA: 0x001045A3 File Offset: 0x001027A3
		public bool IsTransitioning
		{
			get
			{
				return this.rideTransitionInProgress;
			}
		}

		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x06002E4F RID: 11855 RVA: 0x001045AB File Offset: 0x001027AB
		public float MountStabilizationRemaining
		{
			get
			{
				return Mathf.Max(0f, this.mountStabilizationUntil - Time.time);
			}
		}

		// Token: 0x170008D7 RID: 2263
		// (get) Token: 0x06002E50 RID: 11856 RVA: 0x001045C3 File Offset: 0x001027C3
		public vThirdPersonController CurrentRider
		{
			get
			{
				return this.currentRider;
			}
		}

		// Token: 0x170008D8 RID: 2264
		// (get) Token: 0x06002E51 RID: 11857 RVA: 0x001045CB File Offset: 0x001027CB
		public IVehicle Vehicle
		{
			get
			{
				return this.vehicle;
			}
		}

		// Token: 0x170008D9 RID: 2265
		// (get) Token: 0x06002E52 RID: 11858 RVA: 0x001045D3 File Offset: 0x001027D3
		public vVehicleController.VehicleType Type
		{
			get
			{
				return this.vehicleType;
			}
		}

		// Token: 0x170008DA RID: 2266
		// (get) Token: 0x06002E53 RID: 11859 RVA: 0x001045DB File Offset: 0x001027DB
		public IVehicleAnchorProvider AnchorProvider
		{
			get
			{
				return this.anchorProvider;
			}
		}

		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x06002E54 RID: 11860 RVA: 0x001045E3 File Offset: 0x001027E3
		// (set) Token: 0x06002E55 RID: 11861 RVA: 0x001045EB File Offset: 0x001027EB
		public int DiagnosticInputFrame { get; private set; } = -1;

		// Token: 0x170008DC RID: 2268
		// (get) Token: 0x06002E56 RID: 11862 RVA: 0x001045F4 File Offset: 0x001027F4
		// (set) Token: 0x06002E57 RID: 11863 RVA: 0x001045FC File Offset: 0x001027FC
		public int DiagnosticInputCheckFrame { get; private set; } = -1;

		// Token: 0x170008DD RID: 2269
		// (get) Token: 0x06002E58 RID: 11864 RVA: 0x00104605 File Offset: 0x00102805
		// (set) Token: 0x06002E59 RID: 11865 RVA: 0x0010460D File Offset: 0x0010280D
		public float DiagnosticRawThrottle { get; private set; }

		// Token: 0x170008DE RID: 2270
		// (get) Token: 0x06002E5A RID: 11866 RVA: 0x00104616 File Offset: 0x00102816
		// (set) Token: 0x06002E5B RID: 11867 RVA: 0x0010461E File Offset: 0x0010281E
		public float DiagnosticRawSteering { get; private set; }

		// Token: 0x170008DF RID: 2271
		// (get) Token: 0x06002E5C RID: 11868 RVA: 0x00104627 File Offset: 0x00102827
		// (set) Token: 0x06002E5D RID: 11869 RVA: 0x0010462F File Offset: 0x0010282F
		public bool DiagnosticRawBrake { get; private set; }

		// Token: 0x170008E0 RID: 2272
		// (get) Token: 0x06002E5E RID: 11870 RVA: 0x00104638 File Offset: 0x00102838
		// (set) Token: 0x06002E5F RID: 11871 RVA: 0x00104640 File Offset: 0x00102840
		public bool DiagnosticRawSprint { get; private set; }

		// Token: 0x170008E1 RID: 2273
		// (get) Token: 0x06002E60 RID: 11872 RVA: 0x00104649 File Offset: 0x00102849
		// (set) Token: 0x06002E61 RID: 11873 RVA: 0x00104651 File Offset: 0x00102851
		public string DiagnosticInputSuppression { get; private set; } = "not_riding";

		// Token: 0x06002E62 RID: 11874 RVA: 0x0010465C File Offset: 0x0010285C
		private void Awake()
		{
			ControllerContextBindings.Set(this.brakeInput, "A");
			ControllerContextBindings.Set(this.dismountInput, "B");
			ControllerContextBindings.Set(this.sprintInput, "LeftStickClick");
			ControllerContextBindings.Set(this.bellInput, "Y");
			if (this.vehicleType == vVehicleController.VehicleType.RollerSkates)
			{
				this.rollerSkatesVehicle = base.GetComponent<RollerSkatesVehicle>();
				this.vehicle = this.rollerSkatesVehicle;
				this.rollerSkatesAnchorProvider = base.GetComponent<RollerSkatesAnchorProvider>();
			}
			else if (this.vehicleType == vVehicleController.VehicleType.Unicycle)
			{
				this.unicycleVehicle = base.GetComponent<UnicycleVehicle>();
				this.vehicle = this.unicycleVehicle;
			}
			else
			{
				this.vehicle = base.GetComponent<BicycleVehicle>();
			}
			if (this.vehicle == null)
			{
				Debug.LogError(string.Format("[vVehicleController] No IVehicle component found for {0}! Add BicycleVehicle or RollerSkatesVehicle.", this.vehicleType));
				base.enabled = false;
				return;
			}
			this.vehicleRigidbody = base.GetComponent<Rigidbody>();
			this.anchorProvider = this.GetAnchorProvider();
			this.poseDriver = base.GetComponent<VehiclePoseDriver>();
			this.audioController = base.GetComponent<VehicleAudioController>();
			this.rollerbladeAudioController = base.GetComponent<RollerbladeAudioController>();
			this.vehicle.InControl(false);
			this.FreezeVehicle(true);
			if (this.vehicleType == vVehicleController.VehicleType.Bicycle)
			{
				BicycleRideDiagnostics.Attach(this);
			}
			if (this.enableDebugLogs)
			{
				Debug.Log(string.Format("[vVehicleController] Initialized {0} - parked and waiting for rider", this.vehicleType));
			}
		}

		// Token: 0x06002E63 RID: 11875 RVA: 0x001047B0 File Offset: 0x001029B0
		private IVehicleAnchorProvider GetAnchorProvider()
		{
			IVehicleAnchorProvider vehicleAnchorProvider;
			switch (this.vehicleType)
			{
			case vVehicleController.VehicleType.Scooter:
				vehicleAnchorProvider = base.GetComponent<ScooterAnchorProvider>();
				goto IL_004B;
			case vVehicleController.VehicleType.KickScooter:
				vehicleAnchorProvider = base.GetComponent<KickScooterAnchorProvider>();
				goto IL_004B;
			case vVehicleController.VehicleType.RollerSkates:
				vehicleAnchorProvider = base.GetComponent<RollerSkatesAnchorProvider>();
				goto IL_004B;
			case vVehicleController.VehicleType.Unicycle:
				vehicleAnchorProvider = base.GetComponent<UnicycleAnchorProvider>();
				goto IL_004B;
			}
			vehicleAnchorProvider = null;
			IL_004B:
			IVehicleAnchorProvider vehicleAnchorProvider2 = vehicleAnchorProvider;
			if (vehicleAnchorProvider2 == null)
			{
				vehicleAnchorProvider2 = base.GetComponent<BikeAnchorProvider>();
			}
			if (vehicleAnchorProvider2 == null)
			{
				vehicleAnchorProvider2 = base.GetComponent<IVehicleAnchorProvider>();
			}
			return vehicleAnchorProvider2;
		}

		// Token: 0x06002E64 RID: 11876 RVA: 0x00104820 File Offset: 0x00102A20
		private void FreezeVehicle(bool freeze)
		{
			if (this.vehicleRigidbody == null)
			{
				return;
			}
			if (freeze)
			{
				SpawnedVehicleGroundSafety component = base.GetComponent<SpawnedVehicleGroundSafety>();
				if (component != null)
				{
					component.PrepareForParking();
				}
				if (!this.vehicleRigidbody.isKinematic)
				{
					this.vehicleRigidbody.velocity = Vector3.zero;
					this.vehicleRigidbody.angularVelocity = Vector3.zero;
				}
				this.vehicleRigidbody.isKinematic = true;
				this.vehicle.braking = true;
				if (this.enableDebugLogs)
				{
					Debug.Log(string.Format("[vVehicleController] {0} frozen (parked)", this.vehicleType));
					return;
				}
			}
			else
			{
				this.vehicleRigidbody.isKinematic = false;
				SpawnedVehicleGroundSafety component2 = base.GetComponent<SpawnedVehicleGroundSafety>();
				if (component2 != null)
				{
					component2.PrepareForDynamicSimulation();
				}
				this.vehicle.braking = false;
				if (this.enableDebugLogs)
				{
					Debug.Log(string.Format("[vVehicleController] {0} unfrozen (ready to ride)", this.vehicleType));
				}
			}
		}

		// Token: 0x06002E65 RID: 11877 RVA: 0x00104903 File Offset: 0x00102B03
		private void OnDisable()
		{
			this.isSprinting = false;
			this.vehicleSprintInput.Reset();
		}

		// Token: 0x06002E66 RID: 11878 RVA: 0x00104918 File Offset: 0x00102B18
		private void Update()
		{
			if (FlyCamera.IsActive)
			{
				this.isSprinting = false;
				this.vehicleSprintInput.Reset();
				return;
			}
			this.DiagnosticInputCheckFrame = Time.frameCount;
			if (!this.isBeingRidden)
			{
				this.DiagnosticInputSuppression = "not_riding";
				return;
			}
			if (this.mountCooldown > 0f)
			{
				this.mountCooldown -= Time.deltaTime;
			}
			if (this.bellCooldownTimer > 0f)
			{
				this.bellCooldownTimer -= Time.deltaTime;
			}
			this.HandleInput();
			this.HandleBellInput();
			this.CheckDismount();
			if (this.poseDriver != null && this.poseDriver.IsActive)
			{
				this.poseDriver.UpdateFromSpeed(this.GetPoseSpeed(this.vehicle.currentSpeed));
			}
			if (this.vehicleType == vVehicleController.VehicleType.RollerSkates && this.rollerbladeAudioController != null && this.rollerbladeAudioController.IsActive)
			{
				float axis = this.verticalInput.GetAxis();
				this.rollerbladeAudioController.UpdateAudio(this.vehicle.currentSpeed, this.vehicle.braking, axis);
			}
			else if (this.vehicleType != vVehicleController.VehicleType.RollerSkates && this.audioController != null && this.audioController.IsActive)
			{
				float axis2 = this.verticalInput.GetAxis();
				this.audioController.UpdateAudio(this.vehicle.currentSpeed, this.vehicle.braking, axis2);
			}
			if (Mathf.Abs(this.vehicle.currentSpeed - this.lastSpeed) > 0.1f)
			{
				UnityEvent<float> onSpeedChanged = this.OnSpeedChanged;
				if (onSpeedChanged != null)
				{
					onSpeedChanged.Invoke(this.vehicle.currentSpeed);
				}
				this.lastSpeed = this.vehicle.currentSpeed;
			}
		}

		// Token: 0x06002E67 RID: 11879 RVA: 0x00104AD6 File Offset: 0x00102CD6
		private void LateUpdate()
		{
			this.TryRestoreDeferredVehicleCamera();
		}

		// Token: 0x06002E68 RID: 11880 RVA: 0x00104AE0 File Offset: 0x00102CE0
		private void TryRestoreDeferredVehicleCamera()
		{
			if (!this.vehicleCameraPending || !this.isBeingRidden || !this.useInvectorCamera || this.currentRider == null || this.currentRiderInput == null || this.tpCamera == null || this.currentRiderInput.ignoreTpCamera)
			{
				return;
			}
			if (SmartphoneController.IsAnyTripodActive || FlyCamera.IsActive || this.tpCamera.isFreezed || PlayerActivityGate.IsResourceOwned(PlayerContextResource.Camera | PlayerContextResource.ModalUI) || (this.currentRiderInput.firstPersonController != null && this.currentRiderInput.firstPersonController.IsFirstPerson))
			{
				return;
			}
			this.ClaimVehicleCamera();
			this.currentRiderInput.UpdateCameraStates();
			this.tpCamera.SnapToCurrentStateWithoutSmooth(false);
		}

		// Token: 0x06002E69 RID: 11881 RVA: 0x00104BA8 File Offset: 0x00102DA8
		private void ClaimVehicleCamera()
		{
			this.tpCamera.SetMainTarget(base.transform);
			if (!string.IsNullOrEmpty(this.vehicleCameraState))
			{
				this.currentRiderInput.ChangeCameraState(this.vehicleCameraState, true);
			}
			this.vehicleCameraWasClaimed = true;
			this.vehicleCameraPending = false;
			if (this.enableDebugLogs)
			{
				Debug.Log(string.Format("[vVehicleController] Camera switched to {0} target", this.vehicleType));
			}
		}

		// Token: 0x06002E6A RID: 11882 RVA: 0x00104C18 File Offset: 0x00102E18
		private void HandleInput()
		{
			if (VehicleRideActivityContext.BlocksDrivingInput)
			{
				this.DiagnosticInputSuppression = "modal_ui";
				if (this.vehicle != null)
				{
					this.vehicle.verticalInput = 0f;
					this.vehicle.horizontalInput = 0f;
					this.vehicle.braking = true;
				}
				this.isSprinting = false;
				this.vehicleSprintInput.Reset();
				return;
			}
			if (this.currentRiderInput == null || this.currentRiderInput.lockInput)
			{
				this.isSprinting = false;
				this.vehicleSprintInput.Reset();
				this.DiagnosticInputSuppression = ((this.currentRiderInput == null) ? "missing_rider_input" : "rider_input_locked");
				return;
			}
			float axis = this.verticalInput.GetAxis();
			float axis2 = this.horizontalInput.GetAxis();
			bool button = this.brakeInput.GetButton();
			this.isSprinting = this.vehicleSprintInput.Read(this.sprintInput, Mathf.Abs(axis), button);
			this.DiagnosticInputFrame = Time.frameCount;
			this.DiagnosticRawThrottle = axis;
			this.DiagnosticRawSteering = axis2;
			this.DiagnosticRawBrake = button;
			this.DiagnosticRawSprint = this.isSprinting;
			this.DiagnosticInputSuppression = "none";
			float num = (this.isSprinting ? this.sprintMotorMultiplier : 1f);
			this.currentMotorMultiplier = Mathf.MoveTowards(this.currentMotorMultiplier, num, this.sprintTransitionRate * Time.deltaTime);
			BicycleVehicle bicycleVehicle = this.vehicle as BicycleVehicle;
			if (bicycleVehicle != null)
			{
				bicycleVehicle.motorForce = this.originalMotorForce * this.currentMotorMultiplier * this.gameplaySpeedScale;
			}
			UnicycleVehicle unicycleVehicle = this.vehicle as UnicycleVehicle;
			if (unicycleVehicle != null)
			{
				unicycleVehicle.motorForce = this.originalMotorForce * this.currentMotorMultiplier * this.gameplaySpeedScale;
			}
			float num2 = axis * this.gameplaySpeedScale;
			this.vehicle.verticalInput = num2;
			this.vehicle.horizontalInput = axis2;
			bool flag = Mathf.Abs(axis) < 0.01f && !button;
			this.vehicle.braking = button || flag;
			this.vehicle.ConstrainRotation(Time.time < this.mountStabilizationUntil || this.vehicle.OnGround());
			if (this.rollerSkatesAnchorProvider != null)
			{
				this.rollerSkatesAnchorProvider.UpdateWheelRotation(this.vehicle.currentSpeed);
			}
		}

		// Token: 0x06002E6B RID: 11883 RVA: 0x00104E70 File Offset: 0x00103070
		private void HandleBellInput()
		{
			if (PlayerActivityGate.IsResourceOwned(PlayerContextResource.ModalUI))
			{
				return;
			}
			if (this.vehicleType != vVehicleController.VehicleType.Bicycle)
			{
				return;
			}
			if (this.bellCooldownTimer > 0f || this.audioController == null || !this.audioController.IsActive)
			{
				return;
			}
			if (this.bellInput.GetButtonDown())
			{
				this.audioController.PlayHorn();
				this.bellCooldownTimer = this.bellCooldown;
			}
		}

		// Token: 0x06002E6C RID: 11884 RVA: 0x00104EE0 File Offset: 0x001030E0
		private float GetPoseSpeed(float rawSpeed)
		{
			if (!this.compensatePoseSpeed)
			{
				return rawSpeed;
			}
			float num = Mathf.Max(0.01f, this.gameplaySpeedScale);
			return rawSpeed / num;
		}

		// Token: 0x06002E6D RID: 11885 RVA: 0x00104F0C File Offset: 0x0010310C
		private void ApplyRiderRootPose()
		{
			if (this.currentRider == null)
			{
				return;
			}
			Transform seatTransform = this.GetSeatTransform();
			if (this.enableDebugLogs)
			{
				Debug.Log(string.Format("[vVehicleController] ApplyRiderRootPose START - vehicleType={0}", this.vehicleType));
				Debug.Log(string.Format("[vVehicleController] - Vehicle position: {0}", base.transform.position));
				Debug.Log("[vVehicleController] - Seat transform: " + ((seatTransform != null) ? (seatTransform.name + " at " + seatTransform.position.ToString()) : "NULL"));
				Debug.Log(string.Format("[vVehicleController] - Rider BEFORE: {0}", this.currentRider.transform.position));
			}
			if (seatTransform == null)
			{
				Debug.LogWarning("[vVehicleController] Seat is NULL! Cannot position rider.");
				return;
			}
			Vector3 seatPositionOffset = this.riderPositionOffset;
			Quaternion quaternion = Quaternion.Euler(this.riderRotationOffset);
			if (this.enableDebugLogs)
			{
				Debug.Log(string.Format("[vVehicleController] - riderPositionOffset: {0}", this.riderPositionOffset));
			}
			if (this.poseDriver != null && this.poseDriver.PosePreset != null)
			{
				RiderPoseData pose = this.poseDriver.PosePreset.GetPose(VehiclePoseState.Idle);
				if (pose != null)
				{
					seatPositionOffset = pose.seatPositionOffset;
					quaternion = Quaternion.Euler(pose.seatRotationOffset);
					if (this.enableDebugLogs)
					{
						Debug.Log(string.Format("[vVehicleController] - Using PosePreset seatPositionOffset: {0}", seatPositionOffset));
					}
				}
			}
			if (this.vehicleType == vVehicleController.VehicleType.RollerSkates)
			{
				Vector3 position = this.currentRider.transform.position;
				Quaternion rotation = this.currentRider.transform.rotation;
				base.transform.SetParent(this.currentRider.transform, true);
				base.transform.localPosition = Vector3.zero;
				base.transform.localRotation = Quaternion.identity;
				if (this.enableDebugLogs)
				{
					Debug.Log(string.Format("[vVehicleController] RollerSkates: Vehicle parented to rider. Rider stays at {0}", position));
				}
			}
			else
			{
				this.currentRider.transform.SetParent(base.transform, true);
				if (this.enableDebugLogs)
				{
					Debug.Log(string.Format("[vVehicleController] - Rider parented to vehicle. LocalPos after parent: {0}", this.currentRider.transform.localPosition));
				}
				this.currentRider.transform.position = seatTransform.TransformPoint(seatPositionOffset);
				this.currentRider.transform.rotation = seatTransform.rotation * quaternion;
			}
			if (this.enableDebugLogs)
			{
				Debug.Log(string.Format("[vVehicleController] - Rider AFTER: {0} (local: {1})", this.currentRider.transform.position, this.currentRider.transform.localPosition));
			}
		}

		// Token: 0x06002E6E RID: 11886 RVA: 0x001051CC File Offset: 0x001033CC
		private Transform GetSeatTransform()
		{
			if (this.anchorProvider != null)
			{
				Transform anchor = this.anchorProvider.GetAnchor(VehicleAnchorType.Seat);
				if (anchor != null)
				{
					return anchor;
				}
				if (this.vehicleType == vVehicleController.VehicleType.KickScooter)
				{
					Transform anchor2 = this.anchorProvider.GetAnchor(VehicleAnchorType.PelvisReference);
					if (anchor2 != null)
					{
						return anchor2;
					}
				}
			}
			return this.seatPosition;
		}

		// Token: 0x06002E6F RID: 11887 RVA: 0x00105220 File Offset: 0x00103420
		private void CheckDismount()
		{
			if (PlayerActivityGate.IsResourceOwned(PlayerContextResource.ModalUI))
			{
				return;
			}
			if (this.mountCooldown > 0f)
			{
				return;
			}
			if (this.dismountInput.GetButtonDown())
			{
				if (this.CanDismount())
				{
					this.Dismount(false);
					return;
				}
				BicycleRideDiagnostics.Record(this, "dismount.input_rejected.speed", this.currentRider, null);
			}
		}

		// Token: 0x06002E70 RID: 11888 RVA: 0x00105275 File Offset: 0x00103475
		private bool CanDismount()
		{
			return this.vehicle != null && Mathf.Abs(this.vehicle.currentSpeed) < 2f;
		}

		// Token: 0x06002E71 RID: 11889 RVA: 0x00105298 File Offset: 0x00103498
		public void Mount(vThirdPersonController rider)
		{
			if (GuidedRankGameplayPolicy.RideableVehiclesBlocked)
			{
				return;
			}
			if (rider == null || !VehicleRideActivityContext.IsCompatibleWithRider(rider.gameObject, this.vehicleType))
			{
				return;
			}
			BicycleRideDiagnostics.Record(this, "mount.request", rider, null);
			if (this.rideTransitionInProgress || this.isBeingRidden || rider == null)
			{
				BicycleRideDiagnostics.Record(this, "mount.rejected", rider, "transition=" + this.rideTransitionInProgress.ToString() + "; alreadyRidden=" + this.isBeingRidden.ToString());
				Debug.LogWarning(string.Format("[vVehicleController] Mount failed - isBeingRidden:{0}, rider:{1}", this.isBeingRidden, rider));
				return;
			}
			if (this.enableDebugLogs)
			{
				Debug.Log(string.Format("[vVehicleController] === MOUNTING {0} === Rider: {1}", this.vehicleType, rider.name));
			}
			SpawnedVehicleGroundSafety component = base.GetComponent<SpawnedVehicleGroundSafety>();
			if (component != null && !component.PrepareForMount())
			{
				BicycleRideDiagnostics.Record(this, "mount.rejected.unsupported", rider, null);
				Debug.LogWarning(string.Format("[vVehicleController] Mount blocked for {0}: ", this.vehicleType) + "the spawned vehicle no longer has a fully supported pose.", this);
				return;
			}
			this.rideTransitionInProgress = true;
			this.vehicle.ConstrainRotation(true);
			this.mountStabilizationUntil = Time.time + 1f;
			this.currentRider = rider;
			this.currentRiderInput = rider.GetComponent<vThirdPersonInput>();
			this.isBeingRidden = true;
			this.originalRiderParent = this.currentRider.transform.parent;
			this.originalRiderScene = this.currentRider.gameObject.scene;
			this.originalRiderLocalPosition = this.currentRider.transform.localPosition;
			this.originalRiderLocalRotation = this.currentRider.transform.localRotation;
			this.originalRiderLocalScale = this.currentRider.transform.localScale;
			this.originalRiderWorldScale = this.currentRider.transform.lossyScale;
			this.currentRider.lockMovement = true;
			this.currentRider.lockRotation = true;
			this.currentRider.lockAnimMovement = true;
			this.currentRider.lockAnimRotation = true;
			BicycleRideDiagnostics.Record(this, "mount.rider_locked", this.currentRider, null);
			if (this.vehicleType == vVehicleController.VehicleType.RollerSkates)
			{
				this.skatesPrefabVisual = this.GetSkatesPrefabVisual();
				if (this.skatesPrefabVisual != null)
				{
					this.skatesPrefabVisual.SetActive(false);
					if (this.enableDebugLogs)
					{
						Debug.Log("[vVehicleController] Skates prefab visual hidden");
					}
				}
			}
			CapsuleCollider component2 = this.currentRider.GetComponent<CapsuleCollider>();
			if (component2 != null)
			{
				component2.enabled = false;
			}
			Rigidbody component3 = this.currentRider.GetComponent<Rigidbody>();
			if (component3 != null)
			{
				component3.isKinematic = true;
			}
			if (this.GetSeatTransform() != null)
			{
				this.ApplyRiderRootPose();
				if (this.enableDebugLogs)
				{
					Debug.Log(string.Format("[vVehicleController] Rider positioned at {0} seat/stand position", this.vehicleType));
				}
			}
			else
			{
				Debug.LogError("[vVehicleController] Seat/Stand position is NULL! Please assign it in the Inspector.");
			}
			this.riderAnimator = this.currentRider.GetComponent<Animator>();
			if (this.riderAnimator != null)
			{
				this.animatorWasEnabled = this.riderAnimator.enabled;
				this.originalAnimatorController = this.riderAnimator.runtimeAnimatorController;
				if (this.enableDebugLogs)
				{
					Debug.Log("[vVehicleController] Stored Animator state for restoration on dismount");
				}
			}
			BicycleVehicle bicycleVehicle = this.vehicle as BicycleVehicle;
			if (bicycleVehicle != null)
			{
				this.originalMotorForce = bicycleVehicle.motorForce;
			}
			UnicycleVehicle unicycleVehicle = this.vehicle as UnicycleVehicle;
			if (unicycleVehicle != null)
			{
				this.originalMotorForce = unicycleVehicle.motorForce;
			}
			this.currentMotorMultiplier = 1f;
			this.isSprinting = false;
			this.vehicleSprintInput.Reset();
			this.bellCooldownTimer = 0f;
			this.FreezeVehicle(false);
			this.vehicle.InControl(true);
			BicycleRideDiagnostics.Record(this, "mount.physics_enabled", this.currentRider, null);
			this.vehicleCameraWasClaimed = false;
			this.vehicleCameraPending = false;
			if (this.useInvectorCamera && this.currentRiderInput != null && this.currentRiderInput.tpCamera != null)
			{
				this.tpCamera = this.currentRiderInput.tpCamera;
				if (!SmartphoneController.IsAnyTripodActive)
				{
					this.ClaimVehicleCamera();
				}
				else
				{
					this.vehicleCameraPending = true;
					if (this.enableDebugLogs)
					{
						Debug.Log("[vVehicleController] Tripod owns the camera; vehicle camera takeover deferred.", this);
					}
				}
			}
			if (this.poseDriver != null && this.anchorProvider != null)
			{
				this.currentRigController = this.currentRider.GetComponent<CharacterRigController>();
				if (this.currentRigController == null)
				{
					Debug.LogError("[vVehicleController] CharacterRigController not found on rider!");
				}
				else if (this.currentRigController.Initialize())
				{
					this.poseDriver.Initialize(this.anchorProvider, this.currentRigController, this.currentRider.transform);
					this.poseDriver.SetPoseImmediate(VehiclePoseState.Idle);
					if (this.enableDebugLogs)
					{
						Debug.Log("[vVehicleController] Pose system activated");
					}
				}
				else
				{
					Debug.LogError("[vVehicleController] Failed to initialize CharacterRigController");
				}
			}
			if (this.vehicleType == vVehicleController.VehicleType.RollerSkates)
			{
				if (this.rollerbladeAudioController != null)
				{
					this.rollerbladeAudioController.Activate();
					this.SetupRollerbladeAnimationRelay();
				}
			}
			else if (this.audioController != null)
			{
				this.audioController.Activate();
			}
			if (this.vehicleType == vVehicleController.VehicleType.RollerSkates && this.rollerSkatesAnchorProvider != null)
			{
				Animator component4 = this.currentRider.GetComponent<Animator>();
				if (component4 != null)
				{
					Transform boneTransform = component4.GetBoneTransform(HumanBodyBones.LeftFoot);
					Transform boneTransform2 = component4.GetBoneTransform(HumanBodyBones.RightFoot);
					this.rollerSkatesAnchorProvider.AttachSkateVisuals(boneTransform, boneTransform2);
					if (this.enableDebugLogs)
					{
						Debug.Log("[vVehicleController] Roller skate visuals attached to rider feet");
					}
				}
			}
			this.mountCooldown = 0.5f;
			if (CoverPoseController.Instance != null)
			{
				CoverPoseController.Instance.OnVehicleMounted(new VehicleCoverPoseConfig
				{
					allowCover = this.allowCoverWhileRiding,
					armMode = this.coverArmMode,
					anchorProvider = this.anchorProvider,
					poseDriver = this.poseDriver,
					posePreset = ((this.poseDriver != null) ? this.poseDriver.PosePreset : null)
				});
			}
			BicycleRideDiagnostics.Record(this, "mount.callbacks.begin", this.currentRider, null);
			UnityEvent onMount = this.OnMount;
			if (onMount != null)
			{
				onMount.Invoke();
			}
			Action<vVehicleController, vThirdPersonController> onAnyVehicleMounted = vVehicleController.OnAnyVehicleMounted;
			if (onAnyVehicleMounted != null)
			{
				onAnyVehicleMounted(this, this.currentRider);
			}
			this.rideTransitionInProgress = false;
			BicycleRideDiagnostics.Record(this, "mount.complete", this.currentRider, null);
			if (this.enableDebugLogs)
			{
				Debug.Log(string.Format("[vVehicleController] === MOUNT COMPLETE ({0}) ===", this.vehicleType));
			}
		}

		// Token: 0x06002E72 RID: 11890 RVA: 0x00105904 File Offset: 0x00103B04
		public bool Dismount(bool force = false)
		{
			BicycleRideDiagnostics.Record(this, "dismount.request", this.currentRider, "force=" + force.ToString());
			if (this.rideTransitionInProgress || !this.isBeingRidden || this.currentRider == null)
			{
				BicycleRideDiagnostics.Record(this, "dismount.rejected", this.currentRider, "transition=" + this.rideTransitionInProgress.ToString() + "; mounted=" + this.isBeingRidden.ToString());
				Debug.LogWarning("[vVehicleController] Dismount ignored - no rider or transition already active.");
				return false;
			}
			if (!force && !this.CanDismount())
			{
				BicycleRideDiagnostics.Record(this, "dismount.rejected.speed", this.currentRider, null);
				if (this.enableDebugLogs)
				{
					Debug.Log(string.Format("[vVehicleController] Dismount blocked at {0:F1} m/s.", Mathf.Abs(this.vehicle.currentSpeed)));
				}
				return false;
			}
			this.rideTransitionInProgress = true;
			vThirdPersonController vThirdPersonController = this.currentRider;
			if (this.enableDebugLogs)
			{
				Debug.Log(string.Format("[vVehicleController] === DISMOUNTING {0} ===", this.vehicleType));
			}
			if (this.vehicleType == vVehicleController.VehicleType.RollerSkates)
			{
				if (this.rollerSkatesAnchorProvider != null)
				{
					this.rollerSkatesAnchorProvider.DetachSkateVisuals();
					if (this.enableDebugLogs)
					{
						Debug.Log("[vVehicleController] Roller skate visuals detached from rider feet");
					}
				}
				if (this.skatesPrefabVisual != null)
				{
					this.skatesPrefabVisual.SetActive(true);
					this.skatesPrefabVisual = null;
					if (this.enableDebugLogs)
					{
						Debug.Log("[vVehicleController] Skates prefab visual restored");
					}
				}
				Vector3 position = this.currentRider.transform.position;
				Quaternion rotation = this.currentRider.transform.rotation;
				base.transform.SetParent(null, true);
				base.transform.position = position;
				base.transform.rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);
				if (this.enableDebugLogs)
				{
					Debug.Log(string.Format("[vVehicleController] RollerSkates: Vehicle unparented and placed at {0} with rotation {1}", base.transform.position, base.transform.rotation.eulerAngles));
				}
			}
			if (this.poseDriver != null)
			{
				this.poseDriver.Shutdown();
			}
			this.currentRigController = null;
			if (this.riderAnimator != null)
			{
				this.riderAnimator.enabled = this.animatorWasEnabled;
				if (this.riderAnimator.runtimeAnimatorController == null && this.originalAnimatorController != null)
				{
					this.riderAnimator.runtimeAnimatorController = this.originalAnimatorController;
				}
				if (this.enableDebugLogs)
				{
					Debug.Log("[vVehicleController] Rider Animator restored");
				}
			}
			this.riderAnimator = null;
			this.originalAnimatorController = null;
			this.savedLayerWeights = null;
			this.vehicle.InControl(false);
			this.vehicle.verticalInput = 0f;
			this.vehicle.horizontalInput = 0f;
			this.FreezeVehicle(true);
			CapsuleCollider component = this.currentRider.GetComponent<CapsuleCollider>();
			Rigidbody component2 = this.currentRider.GetComponent<Rigidbody>();
			Quaternion quaternion = VehicleDismountSafety.UprightRotation(base.transform, this.currentRider.transform);
			Vector3 vector = VehicleDismountSafety.FindSafeExitPosition(base.transform, this.currentRider.transform, component, 1.15f);
			BicycleRideDiagnostics.Record(this, "dismount.restore_rider.begin", this.currentRider, null);
			Vector3 vector2 = ((!this.originalRiderScene.IsValid() || (this.originalRiderParent != null && this.originalRiderParent.gameObject.scene == this.originalRiderScene)) ? this.originalRiderLocalScale : this.originalRiderWorldScale);
			VehicleDismountSafety.RestoreRiderPhysics(this.currentRider.transform, this.originalRiderParent, vector, quaternion, vector2, component, true, component2, false, this.originalRiderScene);
			if (this.enableDebugLogs)
			{
				Debug.Log(string.Format("[vVehicleController] Rider dismounted at {0}", vector));
			}
			this.currentRider.lockMovement = false;
			this.currentRider.lockRotation = false;
			this.currentRider.lockAnimMovement = false;
			this.currentRider.lockAnimRotation = false;
			this.ClearRollerbladeAnimationRelay();
			if (this.rollerbladeAudioController != null)
			{
				this.rollerbladeAudioController.Deactivate();
			}
			if (this.audioController != null)
			{
				this.audioController.Deactivate();
			}
			BicycleVehicle bicycleVehicle = this.vehicle as BicycleVehicle;
			if (bicycleVehicle != null)
			{
				bicycleVehicle.motorForce = this.originalMotorForce;
			}
			UnicycleVehicle unicycleVehicle = this.vehicle as UnicycleVehicle;
			if (unicycleVehicle != null)
			{
				unicycleVehicle.motorForce = this.originalMotorForce;
			}
			this.currentMotorMultiplier = 1f;
			this.vehicle.InControl(false);
			this.vehicle.verticalInput = 0f;
			this.vehicle.horizontalInput = 0f;
			this.FreezeVehicle(true);
			if (this.vehicleCameraWasClaimed && this.useInvectorCamera && this.tpCamera != null && this.currentRiderInput != null)
			{
				this.tpCamera.SetMainTarget(this.currentRider.transform);
				this.currentRiderInput.ResetCameraState();
			}
			this.vehicleCameraWasClaimed = false;
			this.vehicleCameraPending = false;
			if (CoverPoseController.Instance != null)
			{
				CoverPoseController.Instance.OnVehicleDismounted();
			}
			this.currentRider = null;
			this.originalRiderParent = null;
			this.originalRiderScene = default(Scene);
			this.currentRiderInput = null;
			this.tpCamera = null;
			this.isBeingRidden = false;
			this.isSprinting = false;
			this.vehicleSprintInput.Reset();
			vVehicleInteractable[] componentsInChildren = base.GetComponentsInChildren<vVehicleInteractable>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].ReleaseDismountedRideContext();
			}
			UnityEvent onDismount = this.OnDismount;
			if (onDismount != null)
			{
				onDismount.Invoke();
			}
			Action<vVehicleController, vThirdPersonController> onAnyVehicleDismounted = vVehicleController.OnAnyVehicleDismounted;
			if (onAnyVehicleDismounted != null)
			{
				onAnyVehicleDismounted(this, vThirdPersonController);
			}
			this.rideTransitionInProgress = false;
			BicycleRideDiagnostics.Record(this, "dismount.complete", vThirdPersonController, null);
			if (this.enableDebugLogs)
			{
				Debug.Log(string.Format("[vVehicleController] === DISMOUNT COMPLETE ({0}) ===", this.vehicleType));
			}
			return true;
		}

		// Token: 0x06002E73 RID: 11891 RVA: 0x00105EF2 File Offset: 0x001040F2
		public void ForceDismount()
		{
			if (this.isBeingRidden)
			{
				this.Dismount(true);
			}
		}

		// Token: 0x06002E74 RID: 11892 RVA: 0x00105F04 File Offset: 0x00104104
		private void SetupRollerbladeAnimationRelay()
		{
			if (this.vehicleType != vVehicleController.VehicleType.RollerSkates || this.currentRider == null || this.rollerbladeAudioController == null)
			{
				return;
			}
			this.riderRollerbladeEventRelay = this.currentRider.GetComponent<RollerbladeAnimationEventRelay>();
			if (this.riderRollerbladeEventRelay == null)
			{
				this.riderRollerbladeEventRelay = this.currentRider.gameObject.AddComponent<RollerbladeAnimationEventRelay>();
			}
			this.riderRollerbladeEventRelay.Bind(this.rollerbladeAudioController);
		}

		// Token: 0x06002E75 RID: 11893 RVA: 0x00105F7D File Offset: 0x0010417D
		private void ClearRollerbladeAnimationRelay()
		{
			if (this.riderRollerbladeEventRelay == null || this.rollerbladeAudioController == null)
			{
				return;
			}
			this.riderRollerbladeEventRelay.Unbind(this.rollerbladeAudioController);
			this.riderRollerbladeEventRelay = null;
		}

		// Token: 0x06002E76 RID: 11894 RVA: 0x00105FB4 File Offset: 0x001041B4
		private void OnDrawGizmosSelected()
		{
			Transform seatTransform = this.seatPosition;
			if (Application.isPlaying && this.anchorProvider != null)
			{
				seatTransform = this.GetSeatTransform();
			}
			if (seatTransform != null)
			{
				Gizmos.color = ((this.vehicleType == vVehicleController.VehicleType.KickScooter) ? Color.cyan : Color.green);
				Gizmos.DrawWireSphere(seatTransform.position, 0.2f);
				Gizmos.DrawLine(seatTransform.position, seatTransform.position + seatTransform.forward * 0.5f);
			}
		}

		// Token: 0x06002E77 RID: 11895 RVA: 0x00106038 File Offset: 0x00104238
		private GameObject GetSkatesPrefabVisual()
		{
			Transform transform = base.transform.Find("Visual");
			if (transform == null)
			{
				transform = base.transform.Find("Model");
			}
			if (transform == null)
			{
				transform = base.transform.Find("Skates");
			}
			if (transform == null)
			{
				MeshRenderer componentInChildren = base.GetComponentInChildren<MeshRenderer>();
				if (componentInChildren != null)
				{
					transform = componentInChildren.transform;
				}
			}
			if (transform == null)
			{
				SkinnedMeshRenderer componentInChildren2 = base.GetComponentInChildren<SkinnedMeshRenderer>();
				if (componentInChildren2 != null)
				{
					transform = componentInChildren2.transform;
				}
			}
			if (transform == null)
			{
				return null;
			}
			return transform.gameObject;
		}

		// Token: 0x04002E8B RID: 11915
		[Header("Vehicle Type")]
		[SerializeField]
		private vVehicleController.VehicleType vehicleType;

		// Token: 0x04002E8C RID: 11916
		[Header("Diagnostics")]
		[Tooltip("Verbose vehicle setup and rider-pose logs. Leave disabled during normal play.")]
		[SerializeField]
		private bool enableDebugLogs;

		// Token: 0x04002E8D RID: 11917
		[Header("Input Settings")]
		[Tooltip("Input for acceleration/deceleration")]
		public GenericInput verticalInput = new GenericInput("Vertical", "LeftAnalogVertical", "Vertical");

		// Token: 0x04002E8E RID: 11918
		[Tooltip("Input for steering")]
		public GenericInput horizontalInput = new GenericInput("Horizontal", "LeftAnalogHorizontal", "Horizontal");

		// Token: 0x04002E8F RID: 11919
		[Tooltip("Input for braking")]
		public GenericInput brakeInput = new GenericInput("Space", "X", "X");

		// Token: 0x04002E90 RID: 11920
		[Tooltip("Input to dismount the vehicle")]
		public GenericInput dismountInput = new GenericInput("F", "A", "A");

		// Token: 0x04002E91 RID: 11921
		[Tooltip("Sprint: toggle with LS on controller; hold Shift on keyboard")]
		public GenericInput sprintInput = new GenericInput("LeftShift", "LeftStickClick", "LeftStickClick");

		// Token: 0x04002E92 RID: 11922
		[Tooltip("Input for bicycle bell")]
		public GenericInput bellInput = new GenericInput("B", "Y", "Y");

		// Token: 0x04002E93 RID: 11923
		[Header("Sprint Settings")]
		[Tooltip("Motor force multiplier when sprinting")]
		[Range(1f, 5f)]
		[SerializeField]
		private float sprintMotorMultiplier = 2f;

		// Token: 0x04002E94 RID: 11924
		[Tooltip("How quickly the sprint multiplier transitions")]
		[Range(1f, 20f)]
		[SerializeField]
		private float sprintTransitionRate = 5f;

		// Token: 0x04002E95 RID: 11925
		[Header("Speed Scaling")]
		[Tooltip("Scales gameplay speed without changing pose/animation thresholds")]
		[Range(0.2f, 1f)]
		[SerializeField]
		private float gameplaySpeedScale = 1f;

		// Token: 0x04002E96 RID: 11926
		[Tooltip("If true, pose/animation thresholds keep original behavior while gameplay speed is scaled")]
		[SerializeField]
		private bool compensatePoseSpeed = true;

		// Token: 0x04002E97 RID: 11927
		[Header("Bell Settings")]
		[SerializeField]
		private float bellCooldown = 0.35f;

		// Token: 0x04002E98 RID: 11928
		[Header("Rider Settings")]
		[Tooltip("Transform where the rider sits/stands")]
		public Transform seatPosition;

		// Token: 0x04002E99 RID: 11929
		[Tooltip("Offset from seat position for rider placement")]
		public Vector3 riderPositionOffset = new Vector3(0f, 0f, 0f);

		// Token: 0x04002E9A RID: 11930
		[Tooltip("Rotation offset for rider (degrees)")]
		public Vector3 riderRotationOffset = new Vector3(0f, 0f, 0f);

		// Token: 0x04002E9B RID: 11931
		[Header("Camera Settings")]
		[Tooltip("Camera state name when riding the vehicle")]
		public string vehicleCameraState = "Bike";

		// Token: 0x04002E9C RID: 11932
		[Tooltip("Use Invector's third person camera")]
		public bool useInvectorCamera = true;

		// Token: 0x04002E9D RID: 11933
		[Header("Cover Pose Compatibility")]
		[Tooltip("If true, the player can use cover pose while riding this vehicle (e.g. roller skates). If false, cover pose is blocked.")]
		public bool allowCoverWhileRiding;

		// Token: 0x04002E9E RID: 11934
		[Tooltip("Which arm(s) CoverPose can control while riding this vehicle")]
		[SerializeField]
		private VehicleCoverArmMode coverArmMode;

		// Token: 0x04002E9F RID: 11935
		[Header("Events")]
		public UnityEvent OnMount;

		// Token: 0x04002EA0 RID: 11936
		public UnityEvent OnDismount;

		// Token: 0x04002EA1 RID: 11937
		public UnityEvent<float> OnSpeedChanged;

		// Token: 0x04002EA4 RID: 11940
		private IVehicle vehicle;

		// Token: 0x04002EA5 RID: 11941
		private RollerSkatesVehicle rollerSkatesVehicle;

		// Token: 0x04002EA6 RID: 11942
		private UnicycleVehicle unicycleVehicle;

		// Token: 0x04002EA7 RID: 11943
		private RollerSkatesAnchorProvider rollerSkatesAnchorProvider;

		// Token: 0x04002EA8 RID: 11944
		private vThirdPersonController currentRider;

		// Token: 0x04002EA9 RID: 11945
		private vThirdPersonInput currentRiderInput;

		// Token: 0x04002EAA RID: 11946
		private vThirdPersonCamera tpCamera;

		// Token: 0x04002EAB RID: 11947
		private bool vehicleCameraWasClaimed;

		// Token: 0x04002EAC RID: 11948
		private bool vehicleCameraPending;

		// Token: 0x04002EAD RID: 11949
		private IVehicleAnchorProvider anchorProvider;

		// Token: 0x04002EAE RID: 11950
		private VehiclePoseDriver poseDriver;

		// Token: 0x04002EAF RID: 11951
		private CharacterRigController currentRigController;

		// Token: 0x04002EB0 RID: 11952
		private VehicleAudioController audioController;

		// Token: 0x04002EB1 RID: 11953
		private RollerbladeAudioController rollerbladeAudioController;

		// Token: 0x04002EB2 RID: 11954
		private RollerbladeAnimationEventRelay riderRollerbladeEventRelay;

		// Token: 0x04002EB3 RID: 11955
		private bool isBeingRidden;

		// Token: 0x04002EB4 RID: 11956
		private float lastSpeed;

		// Token: 0x04002EB5 RID: 11957
		private float currentMotorMultiplier = 1f;

		// Token: 0x04002EB6 RID: 11958
		private float originalMotorForce;

		// Token: 0x04002EB7 RID: 11959
		private bool isSprinting;

		// Token: 0x04002EB8 RID: 11960
		private readonly VehicleSprintInput vehicleSprintInput = new VehicleSprintInput();

		// Token: 0x04002EB9 RID: 11961
		private float bellCooldownTimer;

		// Token: 0x04002EBA RID: 11962
		private float mountCooldown;

		// Token: 0x04002EBB RID: 11963
		private bool rideTransitionInProgress;

		// Token: 0x04002EBC RID: 11964
		private const float MOUNT_COOLDOWN_TIME = 0.5f;

		// Token: 0x04002EBD RID: 11965
		private const float MAX_VOLUNTARY_DISMOUNT_SPEED = 2f;

		// Token: 0x04002EBE RID: 11966
		private const float MOUNT_STABILIZATION_TIME = 1f;

		// Token: 0x04002EBF RID: 11967
		private float mountStabilizationUntil;

		// Token: 0x04002EC0 RID: 11968
		private Transform originalRiderParent;

		// Token: 0x04002EC1 RID: 11969
		private Scene originalRiderScene;

		// Token: 0x04002EC2 RID: 11970
		private Vector3 originalRiderLocalPosition;

		// Token: 0x04002EC3 RID: 11971
		private Quaternion originalRiderLocalRotation;

		// Token: 0x04002EC4 RID: 11972
		private Vector3 originalRiderLocalScale;

		// Token: 0x04002EC5 RID: 11973
		private Vector3 originalRiderWorldScale;

		// Token: 0x04002EC6 RID: 11974
		private Animator riderAnimator;

		// Token: 0x04002EC7 RID: 11975
		private bool animatorWasEnabled;

		// Token: 0x04002EC8 RID: 11976
		private RuntimeAnimatorController originalAnimatorController;

		// Token: 0x04002EC9 RID: 11977
		private float[] savedLayerWeights;

		// Token: 0x04002ECA RID: 11978
		private Rigidbody vehicleRigidbody;

		// Token: 0x04002ECB RID: 11979
		private GameObject skatesPrefabVisual;

		// Token: 0x0200070A RID: 1802
		public enum VehicleType
		{
			// Token: 0x04002ED4 RID: 11988
			Bicycle,
			// Token: 0x04002ED5 RID: 11989
			Scooter,
			// Token: 0x04002ED6 RID: 11990
			KickScooter,
			// Token: 0x04002ED7 RID: 11991
			Motorbike,
			// Token: 0x04002ED8 RID: 11992
			RollerSkates,
			// Token: 0x04002ED9 RID: 11993
			Unicycle
		}
	}
}
