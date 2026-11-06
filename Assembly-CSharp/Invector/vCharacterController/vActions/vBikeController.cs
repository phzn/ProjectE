using System;
using Features.DebugTools;
using Features.Detection;
using Features.PhoneCompanion;
using Features.Rank;
using Invector.vCamera;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Interactions;
using RayznGames.VehicleSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vCharacterController.vActions
{
	// Token: 0x02000703 RID: 1795
	[RequireComponent(typeof(BicycleVehicle))]
	public class vBikeController : MonoBehaviour
	{
		// Token: 0x170008C0 RID: 2240
		// (get) Token: 0x06002DF7 RID: 11767 RVA: 0x0010228B File Offset: 0x0010048B
		public bool IsBeingRidden
		{
			get
			{
				return this.isBeingRidden;
			}
		}

		// Token: 0x170008C1 RID: 2241
		// (get) Token: 0x06002DF8 RID: 11768 RVA: 0x00102293 File Offset: 0x00100493
		public vThirdPersonController CurrentRider
		{
			get
			{
				return this.currentRider;
			}
		}

		// Token: 0x170008C2 RID: 2242
		// (get) Token: 0x06002DF9 RID: 11769 RVA: 0x0010229B File Offset: 0x0010049B
		public BicycleVehicle Bicycle
		{
			get
			{
				return this.bicycle;
			}
		}

		// Token: 0x170008C3 RID: 2243
		// (get) Token: 0x06002DFA RID: 11770 RVA: 0x001022A3 File Offset: 0x001004A3
		// (set) Token: 0x06002DFB RID: 11771 RVA: 0x001022AB File Offset: 0x001004AB
		public int DiagnosticInputFrame { get; private set; } = -1;

		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x06002DFC RID: 11772 RVA: 0x001022B4 File Offset: 0x001004B4
		// (set) Token: 0x06002DFD RID: 11773 RVA: 0x001022BC File Offset: 0x001004BC
		public int DiagnosticInputCheckFrame { get; private set; } = -1;

		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x06002DFE RID: 11774 RVA: 0x001022C5 File Offset: 0x001004C5
		// (set) Token: 0x06002DFF RID: 11775 RVA: 0x001022CD File Offset: 0x001004CD
		public float DiagnosticRawThrottle { get; private set; }

		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x06002E00 RID: 11776 RVA: 0x001022D6 File Offset: 0x001004D6
		// (set) Token: 0x06002E01 RID: 11777 RVA: 0x001022DE File Offset: 0x001004DE
		public float DiagnosticRawSteering { get; private set; }

		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x06002E02 RID: 11778 RVA: 0x001022E7 File Offset: 0x001004E7
		// (set) Token: 0x06002E03 RID: 11779 RVA: 0x001022EF File Offset: 0x001004EF
		public bool DiagnosticRawBrake { get; private set; }

		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x06002E04 RID: 11780 RVA: 0x001022F8 File Offset: 0x001004F8
		// (set) Token: 0x06002E05 RID: 11781 RVA: 0x00102300 File Offset: 0x00100500
		public bool DiagnosticRawSprint { get; private set; }

		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x06002E06 RID: 11782 RVA: 0x00102309 File Offset: 0x00100509
		// (set) Token: 0x06002E07 RID: 11783 RVA: 0x00102311 File Offset: 0x00100511
		public string DiagnosticInputSuppression { get; private set; } = "not_riding";

		// Token: 0x06002E08 RID: 11784 RVA: 0x0010231C File Offset: 0x0010051C
		private void Awake()
		{
			ControllerContextBindings.Set(this.brakeInput, "A");
			ControllerContextBindings.Set(this.dismountInput, "B");
			ControllerContextBindings.Set(this.sprintInput, "LeftStickClick");
			ControllerContextBindings.Set(this.bellInput, "Y");
			this.bicycle = base.GetComponent<BicycleVehicle>();
			this.bikeRigidbody = base.GetComponent<Rigidbody>();
			this.anchorProvider = base.GetComponent<BikeAnchorProvider>();
			this.poseDriver = base.GetComponent<VehiclePoseDriver>();
			this.audioController = base.GetComponent<VehicleAudioController>();
			this.bicycle.InControl(false);
			this.FreezeBike(true);
			BicycleRideDiagnostics.Attach(this);
			Debug.Log("[vBikeController] Initialized - bike is parked and waiting for rider");
		}

		// Token: 0x06002E09 RID: 11785 RVA: 0x001023C8 File Offset: 0x001005C8
		private void FreezeBike(bool freeze)
		{
			if (this.bikeRigidbody == null)
			{
				return;
			}
			if (freeze)
			{
				this.bikeRigidbody.isKinematic = true;
				this.bicycle.braking = true;
				Debug.Log("[vBikeController] Bike frozen (parked)");
				return;
			}
			this.bikeRigidbody.isKinematic = false;
			this.bicycle.braking = false;
			Debug.Log("[vBikeController] Bike unfrozen (ready to ride)");
		}

		// Token: 0x06002E0A RID: 11786 RVA: 0x0010242C File Offset: 0x0010062C
		private void OnDisable()
		{
			this.isSprinting = false;
			this.vehicleSprintInput.Reset();
		}

		// Token: 0x06002E0B RID: 11787 RVA: 0x00102440 File Offset: 0x00100640
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
				this.poseDriver.UpdateFromSpeed(this.GetPoseSpeed(this.bicycle.currentSpeed));
			}
			if (this.audioController != null && this.audioController.IsActive)
			{
				float axis = this.verticalInput.GetAxis();
				this.audioController.UpdateAudio(this.bicycle.currentSpeed, this.bicycle.braking, axis);
			}
			if (Mathf.Abs(this.bicycle.currentSpeed - this.lastSpeed) > 0.1f)
			{
				UnityEvent<float> onSpeedChanged = this.OnSpeedChanged;
				if (onSpeedChanged != null)
				{
					onSpeedChanged.Invoke(this.bicycle.currentSpeed);
				}
				this.lastSpeed = this.bicycle.currentSpeed;
			}
		}

		// Token: 0x06002E0C RID: 11788 RVA: 0x001025A1 File Offset: 0x001007A1
		private void LateUpdate()
		{
			this.TryRestoreDeferredVehicleCamera();
		}

		// Token: 0x06002E0D RID: 11789 RVA: 0x001025AC File Offset: 0x001007AC
		private void TryRestoreDeferredVehicleCamera()
		{
			if (!this.vehicleCameraPending || !this.isBeingRidden || !this.useInvectorCamera || this.currentRider == null || this.currentRiderInput == null || this.tpCamera == null)
			{
				return;
			}
			if (SmartphoneController.IsAnyTripodActive || FlyCamera.IsActive || this.tpCamera.isFreezed || this.currentRiderInput.ignoreTpCamera || PlayerActivityGate.IsResourceOwned(PlayerContextResource.Camera | PlayerContextResource.ModalUI) || (this.currentRiderInput.firstPersonController != null && this.currentRiderInput.firstPersonController.IsFirstPerson))
			{
				return;
			}
			this.ClaimVehicleCamera();
			this.currentRiderInput.UpdateCameraStates();
			this.tpCamera.SnapToCurrentStateWithoutSmooth(false);
		}

		// Token: 0x06002E0E RID: 11790 RVA: 0x00102674 File Offset: 0x00100874
		private void ClaimVehicleCamera()
		{
			this.tpCamera.SetMainTarget(base.transform);
			if (!string.IsNullOrEmpty(this.bikeCameraState))
			{
				this.currentRiderInput.ChangeCameraState(this.bikeCameraState, true);
			}
			this.vehicleCameraWasClaimed = true;
			this.vehicleCameraPending = false;
			Debug.Log("[vBikeController] Camera switched to bike target");
		}

		// Token: 0x06002E0F RID: 11791 RVA: 0x001026CC File Offset: 0x001008CC
		private void HandleInput()
		{
			if (VehicleRideActivityContext.BlocksDrivingInput)
			{
				this.DiagnosticInputSuppression = "modal_ui";
				if (this.bicycle != null)
				{
					this.bicycle.verticalInput = 0f;
					this.bicycle.horizontalInput = 0f;
					this.bicycle.braking = true;
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
			this.bicycle.motorForce = this.originalMotorForce * this.currentMotorMultiplier * this.gameplaySpeedScale;
			float num2 = axis * this.gameplaySpeedScale;
			this.bicycle.verticalInput = num2;
			this.bicycle.horizontalInput = axis2;
			bool flag = Mathf.Abs(axis) < 0.01f && !button;
			this.bicycle.braking = button || flag;
			this.bicycle.ConstrainRotation(this.bicycle.OnGround());
		}

		// Token: 0x06002E10 RID: 11792 RVA: 0x001028B0 File Offset: 0x00100AB0
		private void HandleBellInput()
		{
			if (PlayerActivityGate.IsResourceOwned(PlayerContextResource.ModalUI))
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

		// Token: 0x06002E11 RID: 11793 RVA: 0x00102914 File Offset: 0x00100B14
		private float GetPoseSpeed(float rawSpeed)
		{
			if (!this.compensatePoseSpeed)
			{
				return rawSpeed;
			}
			float num = Mathf.Max(0.01f, this.gameplaySpeedScale);
			return rawSpeed / num;
		}

		// Token: 0x06002E12 RID: 11794 RVA: 0x00102940 File Offset: 0x00100B40
		private void ApplyRiderRootPose()
		{
			if (this.currentRider == null)
			{
				return;
			}
			Transform transform = ((this.anchorProvider != null && this.anchorProvider.SeatPosition != null) ? this.anchorProvider.SeatPosition : this.seatPosition);
			if (transform == null)
			{
				return;
			}
			Vector3 seatPositionOffset = this.riderPositionOffset;
			Quaternion quaternion = Quaternion.Euler(this.riderRotationOffset);
			if (this.poseDriver != null && this.poseDriver.PosePreset != null)
			{
				RiderPoseData pose = this.poseDriver.PosePreset.GetPose(VehiclePoseState.Idle);
				if (pose != null)
				{
					seatPositionOffset = pose.seatPositionOffset;
					quaternion = Quaternion.Euler(pose.seatRotationOffset);
				}
			}
			this.currentRider.transform.SetParent(base.transform, true);
			this.currentRider.transform.position = transform.TransformPoint(seatPositionOffset);
			this.currentRider.transform.rotation = transform.rotation * quaternion;
		}

		// Token: 0x06002E13 RID: 11795 RVA: 0x00102A44 File Offset: 0x00100C44
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
					this.Dismount();
					return;
				}
				BicycleRideDiagnostics.Record(this, "dismount.input_rejected.speed", this.currentRider, null);
			}
		}

		// Token: 0x06002E14 RID: 11796 RVA: 0x00102A97 File Offset: 0x00100C97
		private bool CanDismount()
		{
			return this.bicycle.currentSpeed < 2f;
		}

		// Token: 0x06002E15 RID: 11797 RVA: 0x00102AAC File Offset: 0x00100CAC
		public void Mount(vThirdPersonController rider)
		{
			if (rider == null || !VehicleRideActivityContext.IsCompatibleWithRider(rider.gameObject, vVehicleController.VehicleType.Bicycle))
			{
				return;
			}
			if (GuidedRankGameplayPolicy.RideableVehiclesBlocked)
			{
				return;
			}
			BicycleRideDiagnostics.Record(this, "mount.request", rider, null);
			if (this.isBeingRidden || rider == null)
			{
				BicycleRideDiagnostics.Record(this, "mount.rejected", rider, null);
				Debug.LogWarning(string.Format("[vBikeController] Mount failed - isBeingRidden:{0}, rider:{1}", this.isBeingRidden, rider));
				return;
			}
			Debug.Log("[vBikeController] === MOUNTING BIKE === Rider: " + rider.name);
			this.currentRider = rider;
			this.currentRiderInput = rider.GetComponent<vThirdPersonInput>();
			this.isBeingRidden = true;
			this.originalRiderParent = this.currentRider.transform.parent;
			this.originalRiderLocalPosition = this.currentRider.transform.localPosition;
			this.originalRiderLocalRotation = this.currentRider.transform.localRotation;
			this.originalRiderLocalScale = this.currentRider.transform.localScale;
			this.currentRider.lockMovement = true;
			this.currentRider.lockRotation = true;
			this.currentRider.lockAnimMovement = true;
			this.currentRider.lockAnimRotation = true;
			CapsuleCollider component = this.currentRider.GetComponent<CapsuleCollider>();
			if (component != null)
			{
				component.enabled = false;
			}
			Rigidbody component2 = this.currentRider.GetComponent<Rigidbody>();
			if (component2 != null)
			{
				component2.isKinematic = true;
			}
			if (this.seatPosition != null)
			{
				this.ApplyRiderRootPose();
				Debug.Log("[vBikeController] Rider root pose applied at seat");
			}
			else
			{
				Debug.LogError("[vBikeController] SeatPosition is NULL! Please assign it in the Inspector.");
			}
			this.riderAnimator = this.currentRider.GetComponent<Animator>();
			if (this.riderAnimator != null)
			{
				this.animatorWasEnabled = this.riderAnimator.enabled;
				this.originalAnimatorController = this.riderAnimator.runtimeAnimatorController;
				Debug.Log("[vBikeController] Stored Animator state for restoration on dismount");
			}
			this.originalMotorForce = this.bicycle.motorForce;
			this.currentMotorMultiplier = 1f;
			this.isSprinting = false;
			this.vehicleSprintInput.Reset();
			this.bellCooldownTimer = 0f;
			this.FreezeBike(false);
			this.bicycle.InControl(true);
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
					Debug.Log("[vBikeController] Tripod owns the camera; bike camera takeover deferred.");
				}
			}
			if (this.poseDriver != null && this.anchorProvider != null)
			{
				this.currentRigController = this.currentRider.GetComponent<CharacterRigController>();
				if (this.currentRigController == null)
				{
					Debug.LogError("[vBikeController] CharacterRigController not found on rider! Please add it to the character prefab.");
				}
				else if (this.currentRigController.Initialize())
				{
					this.poseDriver.Initialize(this.anchorProvider, this.currentRigController, this.currentRider.transform);
					this.poseDriver.SetPoseImmediate(VehiclePoseState.Idle);
					Debug.Log("[vBikeController] New pose system activated");
				}
				else
				{
					Debug.LogError("[vBikeController] Failed to initialize CharacterRigController");
				}
			}
			if (this.audioController != null)
			{
				this.audioController.Activate();
				Debug.Log("[vBikeController] Audio system activated");
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
			UnityEvent onMount = this.OnMount;
			if (onMount != null)
			{
				onMount.Invoke();
			}
			BicycleRideDiagnostics.Record(this, "mount.complete", this.currentRider, null);
			Debug.Log("[vBikeController] === MOUNT COMPLETE ===");
		}

		// Token: 0x06002E16 RID: 11798 RVA: 0x00102EA8 File Offset: 0x001010A8
		public void Dismount()
		{
			BicycleRideDiagnostics.Record(this, "dismount.request", this.currentRider, null);
			if (!this.isBeingRidden || this.currentRider == null)
			{
				BicycleRideDiagnostics.Record(this, "dismount.rejected", this.currentRider, null);
				Debug.LogWarning("[vBikeController] Dismount failed - no rider");
				return;
			}
			Debug.Log("[vBikeController] === DISMOUNTING BIKE ===");
			if (this.poseDriver != null)
			{
				this.poseDriver.Shutdown();
				Debug.Log("[vBikeController] Pose system shutdown");
			}
			this.currentRigController = null;
			if (this.riderAnimator != null)
			{
				this.riderAnimator.enabled = this.animatorWasEnabled;
				if (this.riderAnimator.runtimeAnimatorController == null && this.originalAnimatorController != null)
				{
					this.riderAnimator.runtimeAnimatorController = this.originalAnimatorController;
				}
				Debug.Log("[vBikeController] Rider Animator restored");
			}
			this.riderAnimator = null;
			this.originalAnimatorController = null;
			this.savedLayerWeights = null;
			Vector3 vector = base.transform.position - base.transform.right * 1f;
			vector.y = base.transform.position.y;
			BicycleRideDiagnostics.Record(this, "dismount.restore_rider.begin", this.currentRider, null);
			this.currentRider.transform.SetParent(this.originalRiderParent, false);
			this.currentRider.transform.localPosition = this.originalRiderLocalPosition;
			this.currentRider.transform.localRotation = this.originalRiderLocalRotation;
			this.currentRider.transform.localScale = this.originalRiderLocalScale;
			this.currentRider.transform.position = vector;
			this.currentRider.transform.rotation = Quaternion.LookRotation(base.transform.forward);
			Debug.Log(string.Format("[vBikeController] Rider dismounted at {0}", vector));
			this.currentRider.lockMovement = false;
			this.currentRider.lockRotation = false;
			this.currentRider.lockAnimMovement = false;
			this.currentRider.lockAnimRotation = false;
			CapsuleCollider component = this.currentRider.GetComponent<CapsuleCollider>();
			if (component != null)
			{
				component.enabled = true;
			}
			Rigidbody component2 = this.currentRider.GetComponent<Rigidbody>();
			if (component2 != null)
			{
				component2.isKinematic = false;
			}
			if (this.audioController != null)
			{
				this.audioController.Deactivate();
				Debug.Log("[vBikeController] Audio system deactivated");
			}
			this.bicycle.motorForce = this.originalMotorForce;
			this.currentMotorMultiplier = 1f;
			this.bicycle.InControl(false);
			this.bicycle.verticalInput = 0f;
			this.bicycle.horizontalInput = 0f;
			this.FreezeBike(true);
			if (this.vehicleCameraWasClaimed && this.useInvectorCamera && this.tpCamera != null && this.currentRiderInput != null)
			{
				this.tpCamera.SetMainTarget(this.currentRider.transform);
				this.currentRiderInput.ResetCameraState();
				Debug.Log("[vBikeController] Camera reset to player");
			}
			this.vehicleCameraWasClaimed = false;
			this.vehicleCameraPending = false;
			if (CoverPoseController.Instance != null)
			{
				CoverPoseController.Instance.OnVehicleDismounted();
			}
			UnityEvent onDismount = this.OnDismount;
			if (onDismount != null)
			{
				onDismount.Invoke();
			}
			this.currentRider = null;
			this.currentRiderInput = null;
			this.tpCamera = null;
			this.isBeingRidden = false;
			this.isSprinting = false;
			this.vehicleSprintInput.Reset();
			vBikeInteractable[] componentsInChildren = base.GetComponentsInChildren<vBikeInteractable>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].ReleaseDismountedRideContext();
			}
			BicycleRideDiagnostics.Record(this, "dismount.complete", null, null);
			Debug.Log("[vBikeController] === DISMOUNT COMPLETE ===");
		}

		// Token: 0x06002E17 RID: 11799 RVA: 0x0010325B File Offset: 0x0010145B
		public void ForceDismount()
		{
			if (this.isBeingRidden)
			{
				this.Dismount();
			}
		}

		// Token: 0x06002E18 RID: 11800 RVA: 0x0010326C File Offset: 0x0010146C
		private void OnDrawGizmosSelected()
		{
			if (this.seatPosition != null)
			{
				Gizmos.color = Color.green;
				Gizmos.DrawWireSphere(this.seatPosition.position, 0.2f);
				Gizmos.DrawLine(this.seatPosition.position, this.seatPosition.position + this.seatPosition.forward * 0.5f);
			}
		}

		// Token: 0x04002E37 RID: 11831
		[Header("Input Settings")]
		[Tooltip("Input for acceleration/deceleration")]
		public GenericInput verticalInput = new GenericInput("Vertical", "LeftAnalogVertical", "Vertical");

		// Token: 0x04002E38 RID: 11832
		[Tooltip("Input for steering")]
		public GenericInput horizontalInput = new GenericInput("Horizontal", "LeftAnalogHorizontal", "Horizontal");

		// Token: 0x04002E39 RID: 11833
		[Tooltip("Input for braking")]
		public GenericInput brakeInput = new GenericInput("Space", "X", "X");

		// Token: 0x04002E3A RID: 11834
		[Tooltip("Input to dismount the bike")]
		public GenericInput dismountInput = new GenericInput("F", "A", "A");

		// Token: 0x04002E3B RID: 11835
		[Tooltip("Sprint: toggle with LS on controller; hold Shift on keyboard")]
		public GenericInput sprintInput = new GenericInput("LeftShift", "LeftStickClick", "LeftStickClick");

		// Token: 0x04002E3C RID: 11836
		[Tooltip("Input for bicycle bell")]
		public GenericInput bellInput = new GenericInput("B", "Y", "Y");

		// Token: 0x04002E3D RID: 11837
		[Header("Sprint Settings")]
		[Tooltip("Motor force multiplier when sprinting")]
		[Range(1f, 5f)]
		[SerializeField]
		private float sprintMotorMultiplier = 2f;

		// Token: 0x04002E3E RID: 11838
		[Tooltip("How quickly the sprint multiplier transitions")]
		[Range(1f, 20f)]
		[SerializeField]
		private float sprintTransitionRate = 5f;

		// Token: 0x04002E3F RID: 11839
		[Header("Speed Scaling")]
		[Tooltip("Scales vehicle gameplay speed without changing pose/animation thresholds")]
		[Range(0.2f, 1f)]
		[SerializeField]
		private float gameplaySpeedScale = 1f;

		// Token: 0x04002E40 RID: 11840
		[Tooltip("If true, pose/animation thresholds keep original behavior while gameplay speed is scaled")]
		[SerializeField]
		private bool compensatePoseSpeed = true;

		// Token: 0x04002E41 RID: 11841
		[Header("Bell Settings")]
		[SerializeField]
		private float bellCooldown = 0.35f;

		// Token: 0x04002E42 RID: 11842
		[Header("Rider Settings")]
		[Tooltip("Transform where the rider sits")]
		public Transform seatPosition;

		// Token: 0x04002E43 RID: 11843
		[Tooltip("Transform for the rider to look at (forward direction)")]
		public Transform lookAtTarget;

		// Token: 0x04002E44 RID: 11844
		[Tooltip("Offset from seat position for rider placement")]
		public Vector3 riderPositionOffset = new Vector3(0f, 0f, 0f);

		// Token: 0x04002E45 RID: 11845
		[Tooltip("Rotation offset for rider (degrees)")]
		public Vector3 riderRotationOffset = new Vector3(0f, 0f, 0f);

		// Token: 0x04002E46 RID: 11846
		[Header("Camera Settings")]
		[Tooltip("Camera state name when riding the bike")]
		public string bikeCameraState = "Bike";

		// Token: 0x04002E47 RID: 11847
		[Tooltip("Use Invector's third person camera")]
		public bool useInvectorCamera = true;

		// Token: 0x04002E48 RID: 11848
		[Header("Cover Pose Compatibility")]
		[Tooltip("If true, the player can use cover pose while riding this vehicle. If false, cover pose is blocked.")]
		public bool allowCoverWhileRiding;

		// Token: 0x04002E49 RID: 11849
		[Tooltip("Which arm(s) CoverPose can control while riding this vehicle")]
		[SerializeField]
		private VehicleCoverArmMode coverArmMode;

		// Token: 0x04002E4A RID: 11850
		[Header("Events")]
		public UnityEvent OnMount;

		// Token: 0x04002E4B RID: 11851
		public UnityEvent OnDismount;

		// Token: 0x04002E4C RID: 11852
		public UnityEvent<float> OnSpeedChanged;

		// Token: 0x04002E4D RID: 11853
		private BicycleVehicle bicycle;

		// Token: 0x04002E4E RID: 11854
		private vThirdPersonController currentRider;

		// Token: 0x04002E4F RID: 11855
		private vThirdPersonInput currentRiderInput;

		// Token: 0x04002E50 RID: 11856
		private vThirdPersonCamera tpCamera;

		// Token: 0x04002E51 RID: 11857
		private bool vehicleCameraWasClaimed;

		// Token: 0x04002E52 RID: 11858
		private bool vehicleCameraPending;

		// Token: 0x04002E53 RID: 11859
		private BikeAnchorProvider anchorProvider;

		// Token: 0x04002E54 RID: 11860
		private VehiclePoseDriver poseDriver;

		// Token: 0x04002E55 RID: 11861
		private CharacterRigController currentRigController;

		// Token: 0x04002E56 RID: 11862
		private VehicleAudioController audioController;

		// Token: 0x04002E57 RID: 11863
		private bool isBeingRidden;

		// Token: 0x04002E58 RID: 11864
		private float lastSpeed;

		// Token: 0x04002E59 RID: 11865
		private float currentMotorMultiplier = 1f;

		// Token: 0x04002E5A RID: 11866
		private float originalMotorForce;

		// Token: 0x04002E5B RID: 11867
		private bool isSprinting;

		// Token: 0x04002E5C RID: 11868
		private readonly VehicleSprintInput vehicleSprintInput = new VehicleSprintInput();

		// Token: 0x04002E5D RID: 11869
		private float bellCooldownTimer;

		// Token: 0x04002E5E RID: 11870
		private float mountCooldown;

		// Token: 0x04002E5F RID: 11871
		private const float MOUNT_COOLDOWN_TIME = 0.5f;

		// Token: 0x04002E60 RID: 11872
		private Transform originalRiderParent;

		// Token: 0x04002E61 RID: 11873
		private Vector3 originalRiderLocalPosition;

		// Token: 0x04002E62 RID: 11874
		private Quaternion originalRiderLocalRotation;

		// Token: 0x04002E63 RID: 11875
		private Vector3 originalRiderLocalScale;

		// Token: 0x04002E64 RID: 11876
		private Animator riderAnimator;

		// Token: 0x04002E65 RID: 11877
		private bool animatorWasEnabled;

		// Token: 0x04002E66 RID: 11878
		private RuntimeAnimatorController originalAnimatorController;

		// Token: 0x04002E67 RID: 11879
		private float[] savedLayerWeights;

		// Token: 0x04002E6F RID: 11887
		private Rigidbody bikeRigidbody;
	}
}
