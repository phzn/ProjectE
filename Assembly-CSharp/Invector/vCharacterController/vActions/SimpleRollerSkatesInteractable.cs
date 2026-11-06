using System;
using System.Collections.Generic;
using BoZo.CharacterCreator;
using Features.DebugTools;
using Features.Detection;
using Features.PhoneCompanion;
using Features.Rank;
using Features.Tutorials;
using Features.Vehicles.Spawning;
using Invector.vCamera;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.FirstPerson;
using ProjectExhibitionist.Interactions;
using RayznGames.VehicleSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Invector.vCharacterController.vActions
{
	// Token: 0x020006FE RID: 1790
	public class SimpleRollerSkatesInteractable : MonoBehaviour, IInteractable, IInteractionCandidate
	{
		// Token: 0x1400004D RID: 77
		// (add) Token: 0x06002D72 RID: 11634 RVA: 0x000FCD48 File Offset: 0x000FAF48
		// (remove) Token: 0x06002D73 RID: 11635 RVA: 0x000FCD7C File Offset: 0x000FAF7C
		public static event Action<SimpleRollerSkatesInteractable, vThirdPersonController> OnAnyMounted;

		// Token: 0x1400004E RID: 78
		// (add) Token: 0x06002D74 RID: 11636 RVA: 0x000FCDB0 File Offset: 0x000FAFB0
		// (remove) Token: 0x06002D75 RID: 11637 RVA: 0x000FCDE4 File Offset: 0x000FAFE4
		public static event Action<SimpleRollerSkatesInteractable, vThirdPersonController> OnAnyDismounted;

		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x06002D76 RID: 11638 RVA: 0x000FCE17 File Offset: 0x000FB017
		public InteractionConfigSO Config
		{
			get
			{
				return this.interactionConfig;
			}
		}

		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x06002D77 RID: 11639 RVA: 0x000FCE1F File Offset: 0x000FB01F
		public bool IsBeingUsed
		{
			get
			{
				return this.isBeingWorn;
			}
		}

		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x06002D78 RID: 11640 RVA: 0x000FCE28 File Offset: 0x000FB028
		private string RideGateReason
		{
			get
			{
				return "VehicleRide#" + base.GetInstanceID().ToString();
			}
		}

		// Token: 0x170008AA RID: 2218
		// (get) Token: 0x06002D79 RID: 11641 RVA: 0x000FCE4D File Offset: 0x000FB04D
		public float CandidateSqrDistance
		{
			get
			{
				return this._sqrDistToPlayer;
			}
		}

		// Token: 0x170008AB RID: 2219
		// (get) Token: 0x06002D7A RID: 11642 RVA: 0x000FCE55 File Offset: 0x000FB055
		public bool IsCandidateEligible
		{
			get
			{
				return this.isPlayerInRange && !this.isBeingWorn && this.playerTransform != null && this.CanInteract(this.playerTransform.gameObject);
			}
		}

		// Token: 0x170008AC RID: 2220
		// (get) Token: 0x06002D7B RID: 11643 RVA: 0x000FCE88 File Offset: 0x000FB088
		public int CandidateInstanceId
		{
			get
			{
				return base.GetInstanceID();
			}
		}

		// Token: 0x06002D7C RID: 11644 RVA: 0x000FCE90 File Offset: 0x000FB090
		private void OnEnable()
		{
			InteractionCandidateRegistry.Register(this);
		}

		// Token: 0x06002D7D RID: 11645 RVA: 0x000FCE98 File Offset: 0x000FB098
		private void OnDisable()
		{
			this.isSprinting = false;
			this.vehicleSprintInput.Reset();
			if (this.isBeingWorn && this.currentRider != null)
			{
				this.ForceCrashDismount();
			}
			InteractionCandidateRegistry.Unregister(this);
			VehicleRideActivityContext.Release(ref this._rideLease);
		}

		// Token: 0x06002D7E RID: 11646 RVA: 0x000FCEE4 File Offset: 0x000FB0E4
		private void SyncRideGate()
		{
			if (this.isBeingWorn && (this._rideLease == null || !this._rideLease.IsActive))
			{
				if (!VehicleRideActivityContext.TryAcquire(this.RideGateReason, this, ref this._rideLease))
				{
					Debug.LogError("[SimpleRollerSkates] Ride context was lost and could not be restored; forcing a safe dismount.", this);
					this.ForceCrashDismount();
					return;
				}
			}
			else if (!this.isBeingWorn)
			{
				VehicleRideActivityContext.Release(ref this._rideLease);
			}
		}

		// Token: 0x06002D7F RID: 11647 RVA: 0x000FCF48 File Offset: 0x000FB148
		public bool CanInteract(GameObject player)
		{
			if (this.isBeingWorn)
			{
				return false;
			}
			vThirdPersonController component = player.GetComponent<vThirdPersonController>();
			return (!(component != null) || !component.lockMovement) && VehicleRideActivityContext.CanAcquire;
		}

		// Token: 0x170008AD RID: 2221
		// (get) Token: 0x06002D80 RID: 11648 RVA: 0x000FCE1F File Offset: 0x000FB01F
		public bool IsBeingWorn
		{
			get
			{
				return this.isBeingWorn;
			}
		}

		// Token: 0x170008AE RID: 2222
		// (get) Token: 0x06002D81 RID: 11649 RVA: 0x000FCF7E File Offset: 0x000FB17E
		public bool IsTransitioning
		{
			get
			{
				return this.rideTransitionInProgress;
			}
		}

		// Token: 0x170008AF RID: 2223
		// (get) Token: 0x06002D82 RID: 11650 RVA: 0x000FCF86 File Offset: 0x000FB186
		public vThirdPersonController CurrentRider
		{
			get
			{
				return this.currentRider;
			}
		}

		// Token: 0x170008B0 RID: 2224
		// (get) Token: 0x06002D83 RID: 11651 RVA: 0x000FCF8E File Offset: 0x000FB18E
		public float CurrentSpeed
		{
			get
			{
				return this.currentSpeed;
			}
		}

		// Token: 0x06002D84 RID: 11652 RVA: 0x000FCF98 File Offset: 0x000FB198
		private void Awake()
		{
			ControllerContextBindings.SetLabel(this.interactionConfig, "shift", "L3", false);
			ControllerContextBindings.SetLabel(this.interactionConfig, "space", "A", false);
			ControllerContextBindings.SetLabel(this.interactionConfig, "E", "B", false);
			ControllerContextBindings.Set(this.brakeInput, "A");
			ControllerContextBindings.Set(this.sprintInput, "LeftStickClick");
			if (this.vehicleRigidbody == null)
			{
				this.vehicleRigidbody = base.GetComponent<Rigidbody>();
				if (this.vehicleRigidbody == null)
				{
					Debug.LogError("[SimpleRollerSkates] No Rigidbody found! Please add and configure one in the Inspector.");
				}
			}
			Collider component = base.GetComponent<Collider>();
			if (component == null)
			{
				Debug.LogWarning("[SimpleRollerSkates] No Collider found! Please add one in the Inspector.");
			}
			else if (this.vehiclePhysicMaterial != null)
			{
				component.material = this.vehiclePhysicMaterial;
			}
			else if (this.autoCreateZeroFrictionMaterial)
			{
				PhysicMaterial physicMaterial = new PhysicMaterial("ZeroFriction")
				{
					dynamicFriction = 0f,
					staticFriction = 0f,
					frictionCombine = PhysicMaterialCombine.Minimum,
					bounciness = 0f,
					bounceCombine = PhysicMaterialCombine.Minimum
				};
				component.material = physicMaterial;
				this.LogDebug("Auto-created zero friction PhysicMaterial for collider");
			}
			this.rollerbladeAudioController = base.GetComponent<RollerbladeAudioController>();
		}

		// Token: 0x06002D85 RID: 11653 RVA: 0x000FD0D4 File Offset: 0x000FB2D4
		private void Start()
		{
			this.LogDebug("=".PadRight(50, '='));
			this.LogDebug("=== INITIALIZING ===");
			this.LogDebug("Vehicle Rigidbody: " + ((this.vehicleRigidbody != null) ? "FOUND" : "NULL - WILL BE ADDED"));
			this.LogDebug("Left Skate Prefab: " + ((this.leftSkatePrefab != null) ? this.leftSkatePrefab.name : "NULL - ASSIGN IN INSPECTOR!"));
			this.LogDebug("Right Skate Prefab: " + ((this.rightSkatePrefab != null) ? this.rightSkatePrefab.name : "NULL - ASSIGN IN INSPECTOR!"));
			if (this.skatesPrefabVisual == null)
			{
				Transform transform = base.transform.Find("Visual");
				if (transform != null)
				{
					this.skatesPrefabVisual = transform.gameObject;
				}
			}
			this.FreezeVehicle(true);
			this.playerTransform = this._playerLookup.Resolve();
			this.LogDebug("=== INITIALIZATION COMPLETE ===");
		}

		// Token: 0x06002D86 RID: 11654 RVA: 0x000FD1E4 File Offset: 0x000FB3E4
		private void Update()
		{
			if (FlyCamera.IsActive)
			{
				this.isSprinting = false;
				this.vehicleSprintInput.Reset();
				return;
			}
			this.SyncRideGate();
			if (this.mountCooldown > 0f)
			{
				this.mountCooldown -= Time.deltaTime;
			}
			if (this.isBeingWorn)
			{
				if (!SmartphoneController.IsPhonePointerActive && !SmartphoneController.IsAnySelfieActive && !PlayerActivityGate.IsResourceOwned(PlayerContextResource.ModalUI))
				{
					InteractionHUDController instance = InteractionHUDController.Instance;
					if (instance != null && (!instance.IsShowing || instance.CurrentConfig != this.interactionConfig))
					{
						instance.Show(this.interactionConfig);
					}
				}
				this.HandleInput();
				this.CheckDismount();
				this.UpdateSkateOffsets();
				if (Mathf.Abs(this.currentSpeed - this.lastSpeed) > 0.1f)
				{
					UnityEvent<float> onSpeedChanged = this.OnSpeedChanged;
					if (onSpeedChanged != null)
					{
						onSpeedChanged.Invoke(this.currentSpeed);
					}
					this.lastSpeed = this.currentSpeed;
					return;
				}
			}
			else
			{
				this.CheckPlayerProximity();
			}
		}

		// Token: 0x06002D87 RID: 11655 RVA: 0x000FD2DD File Offset: 0x000FB4DD
		private void LateUpdate()
		{
			this.TryRestoreDeferredVehicleCamera();
		}

		// Token: 0x06002D88 RID: 11656 RVA: 0x000FD2E8 File Offset: 0x000FB4E8
		private void FixedUpdate()
		{
			if (!this.isBeingWorn)
			{
				return;
			}
			this.frameCounter++;
			this.CheckGround();
			this.HandleMovementSimple();
			this.ConstrainVehicleRotation();
			this.UpdateSpeed();
			if (this.rollerbladeAudioController != null && this.rollerbladeAudioController.IsActive)
			{
				this.rollerbladeAudioController.UpdateAudio(this.currentSpeed, this.inputBrake, this.inputMagnitude);
			}
			this.ReapplyBlendShapeOverrides();
		}

		// Token: 0x06002D89 RID: 11657 RVA: 0x000FD364 File Offset: 0x000FB564
		private void HandleInput()
		{
			if ((this.currentRiderInput != null && this.currentRiderInput.lockInput) || VehicleRideActivityContext.BlocksDrivingInput)
			{
				this.inputMagnitude = 0f;
				this.moveDirection = Vector3.zero;
				this.inputBrake = true;
				this.isSprinting = false;
				this.vehicleSprintInput.Reset();
				return;
			}
			float axis = this.verticalInput.GetAxis();
			float axis2 = this.horizontalInput.GetAxis();
			this.inputBrake = this.brakeInput.GetButton();
			this.isSprinting = this.vehicleSprintInput.Read(this.sprintInput, new Vector2(axis2, axis).magnitude, this.inputBrake);
			float num = (this.isSprinting ? this.sprintSpeedMultiplier : 1f);
			this.currentSpeedMultiplier = Mathf.MoveTowards(this.currentSpeedMultiplier, num, this.sprintTransitionRate * Time.deltaTime);
			this.UpdateMovementInput(axis, axis2);
		}

		// Token: 0x06002D8A RID: 11658 RVA: 0x000FD454 File Offset: 0x000FB654
		private void UpdateMovementInput(float rawVertical, float rawHorizontal)
		{
			this.inputMagnitude = Mathf.Clamp01(new Vector2(rawHorizontal, rawVertical).magnitude);
			if (this.inputMagnitude <= 0.01f)
			{
				this.moveDirection = Vector3.zero;
				return;
			}
			Vector3 vector;
			if (this.useCameraRelativeInput && this.riderFirstPerson != null && this.riderFirstPerson.IsFirstPerson)
			{
				vector = Quaternion.Euler(0f, this.riderFirstPerson.MovementYaw, 0f) * Vector3.forward;
			}
			else
			{
				vector = SimpleRollerSkatesInteractable.GetPlanarMovementForward((this.useCameraRelativeInput && this.mainCamera != null) ? this.mainCamera.transform : base.transform);
			}
			Vector3 vector2 = Vector3.Cross(Vector3.up, vector);
			this.moveDirection = (vector * rawVertical + vector2 * rawHorizontal).normalized;
		}

		// Token: 0x06002D8B RID: 11659 RVA: 0x000FD53C File Offset: 0x000FB73C
		private static Vector3 GetPlanarMovementForward(Transform reference)
		{
			Vector3 vector = Vector3.ProjectOnPlane(reference.right, Vector3.up);
			if (vector.sqrMagnitude > 0.0001f)
			{
				return Vector3.Cross(vector.normalized, Vector3.up);
			}
			return Vector3.ProjectOnPlane(reference.forward, Vector3.up).normalized;
		}

		// Token: 0x06002D8C RID: 11660 RVA: 0x000FD594 File Offset: 0x000FB794
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
				this.LogDebug("Vehicle FROZEN (kinematic)");
				return;
			}
			this.vehicleRigidbody.isKinematic = false;
			this.vehicleRigidbody.velocity = Vector3.zero;
			this.vehicleRigidbody.angularVelocity = Vector3.zero;
			SpawnedVehicleGroundSafety component2 = base.GetComponent<SpawnedVehicleGroundSafety>();
			if (component2 != null)
			{
				component2.PrepareForDynamicSimulation();
			}
			this.vehicleRigidbody.drag = 0f;
			this.vehicleRigidbody.angularDrag = 0.5f;
			this.LogDebug("Vehicle UNFROZEN (dynamic)");
		}

		// Token: 0x06002D8D RID: 11661 RVA: 0x000FD674 File Offset: 0x000FB874
		private void CheckPlayerProximity()
		{
			this.playerTransform = this._playerLookup.Resolve();
			if (this.playerTransform == null)
			{
				return;
			}
			bool flag = this.isPlayerInRange;
			this._sqrDistToPlayer = (base.transform.position - this.playerTransform.position).sqrMagnitude;
			this.isPlayerInRange = this._sqrDistToPlayer <= this.interactionDistance * this.interactionDistance;
			bool flag2 = this.isPlayerInRange && InteractionCandidateRegistry.IsWinner(this) && PlayerActivityGate.Allows(PlayerActivity.Interact);
			this.isPlayerInRange = flag2;
			if (flag2 && !flag)
			{
				if (this.CanInteract(this.playerTransform.gameObject) && this.promptUI != null)
				{
					this.promptUI.Show(this.interactionConfig);
				}
			}
			else if (!flag2 && flag && this.promptUI != null)
			{
				this.promptUI.Hide();
			}
			if (this.isPlayerInRange && this.interactInput.GetButtonDown() && this.CanInteract(this.playerTransform.gameObject))
			{
				this.OnInteract(this.playerTransform.gameObject);
			}
		}

		// Token: 0x06002D8E RID: 11662 RVA: 0x000FD7A5 File Offset: 0x000FB9A5
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
			if (ControllerContextBindings.ExitPressed(this.interactInput))
			{
				this.Dismount(false);
			}
		}

		// Token: 0x06002D8F RID: 11663 RVA: 0x000FD7D4 File Offset: 0x000FB9D4
		public void OnInteract(GameObject player)
		{
			if (GuidedRankGameplayPolicy.RideableVehiclesBlocked)
			{
				return;
			}
			if (TutorialService.TryShowFirstUse("vehicles"))
			{
				return;
			}
			vThirdPersonController component = player.GetComponent<vThirdPersonController>();
			if (component == null)
			{
				return;
			}
			this.Mount(component);
			if (this.isBeingWorn)
			{
				if (this.promptUI != null)
				{
					this.promptUI.Hide();
				}
				InteractionHUDController instance = InteractionHUDController.Instance;
				if (instance == null)
				{
					return;
				}
				instance.Show(this.interactionConfig);
			}
		}

		// Token: 0x06002D90 RID: 11664 RVA: 0x00004F31 File Offset: 0x00003131
		public void OnActionExecuted(string actionName, GameObject player)
		{
		}

		// Token: 0x06002D91 RID: 11665 RVA: 0x000FD844 File Offset: 0x000FBA44
		private void Mount(vThirdPersonController rider)
		{
			if (rider == null || this.isBeingWorn || this.rideTransitionInProgress)
			{
				this.LogDebug(string.Format("Mount ABORTED - rider={0}, isBeingWorn={1}", rider, this.isBeingWorn));
				return;
			}
			if (!VehicleRideActivityContext.TryAcquire(this.RideGateReason, this, ref this._rideLease))
			{
				BlockedActionFeedback.Show("Stop what you're doing first");
				return;
			}
			this.rideTransitionInProgress = true;
			InteractionCandidateRegistry.MarkInteract();
			this.LogDebug("=".PadRight(50, '='));
			this.LogDebug("=== MOUNTING ROLLER SKATES === Rider: " + rider.name);
			this.LogDebug("=".PadRight(50, '='));
			this.currentRider = rider;
			this.currentRiderInput = rider.GetComponent<vThirdPersonInput>();
			this.riderFirstPerson = rider.GetComponent<FirstPersonController>();
			this.riderTransform = rider.transform;
			this.LogDebug(string.Format("Rider Transform: {0} at position {1}", this.riderTransform.name, this.riderTransform.position));
			this.LogDebug("Rider Input: " + ((this.currentRiderInput != null) ? "FOUND" : "NULL - WARNING!"));
			if (this.playerAnimator != null)
			{
				this.riderAnimator = this.playerAnimator;
				this.LogDebug("Using INSPECTOR-assigned Animator: " + this.riderAnimator.gameObject.name);
			}
			else
			{
				this.riderAnimator = rider.GetComponent<Animator>();
				this.LogDebug("Auto-detected Animator: " + ((this.riderAnimator != null) ? this.riderAnimator.gameObject.name : "NULL"));
			}
			this.riderRigidbody = rider.GetComponent<Rigidbody>();
			this.riderCapsule = rider.GetComponent<CapsuleCollider>();
			this.LogDebug("Rider Rigidbody: " + ((this.riderRigidbody != null) ? "FOUND" : "NULL"));
			this.LogDebug("Rider CapsuleCollider: " + ((this.riderCapsule != null) ? "FOUND" : "NULL"));
			this.originalRiderParent = this.riderTransform.parent;
			this.originalRiderLocalScale = this.riderTransform.localScale;
			if (this.riderRigidbody != null)
			{
				this.originalRiderIsKinematic = this.riderRigidbody.isKinematic;
			}
			if (this.riderCapsule != null)
			{
				this.originalCapsuleEnabled = this.riderCapsule.enabled;
			}
			this.LogDebug("Original rider parent: " + ((this.originalRiderParent != null) ? this.originalRiderParent.name : "NULL (root)"));
			this.LogDebug(string.Format("Original rider isKinematic: {0}", this.originalRiderIsKinematic));
			this.LogDebug("Locking Invector character controller...");
			this.currentRider.lockMovement = true;
			this.currentRider.lockRotation = true;
			this.currentRider.lockAnimMovement = true;
			this.currentRider.lockAnimRotation = true;
			this.LogDebug("Invector locks applied");
			if (this.riderCapsule != null)
			{
				this.riderCapsule.enabled = false;
				this.LogDebug("Rider CapsuleCollider DISABLED");
			}
			if (this.riderRigidbody != null)
			{
				this.riderRigidbody.isKinematic = true;
				this.LogDebug("Rider Rigidbody set to KINEMATIC");
			}
			base.transform.position = this.riderTransform.position;
			base.transform.rotation = Quaternion.Euler(0f, this.riderTransform.eulerAngles.y, 0f);
			this.LogDebug(string.Format("Vehicle positioned at rider: {0}", base.transform.position));
			this.riderTransform.SetParent(base.transform, true);
			this.riderTransform.localPosition = Vector3.zero;
			this.riderTransform.localRotation = Quaternion.identity;
			this.LogDebug(string.Format("Rider parented to vehicle. Rider local pos: {0}", this.riderTransform.localPosition));
			if (this.riderAnimator != null)
			{
				this.leftFootBone = this.riderAnimator.GetBoneTransform(this.leftSkateAttachBone);
				this.rightFootBone = this.riderAnimator.GetBoneTransform(this.rightSkateAttachBone);
				this.LogDebug(string.Format("Skate attach bones - Left: {0} ({1}), Right: {2} ({3})", new object[]
				{
					this.leftSkateAttachBone,
					(this.leftFootBone != null) ? this.leftFootBone.name : "NULL",
					this.rightSkateAttachBone,
					(this.rightFootBone != null) ? this.rightFootBone.name : "NULL"
				}));
				this.originalApplyRootMotion = this.riderAnimator.applyRootMotion;
				this.riderAnimator.applyRootMotion = false;
				this.LogDebug(string.Format("Root Motion DISABLED (was: {0})", this.originalApplyRootMotion));
				this.riderAnimator.SetBool(SimpleRollerSkatesInteractable.IsRidingHash, true);
				this.riderAnimator.SetFloat(SimpleRollerSkatesInteractable.VehicleTypeHash, this.vehicleTypeValue);
				this.riderAnimator.SetFloat(SimpleRollerSkatesInteractable.VehicleSpeedHash, 0f);
				this.LogDebug(string.Format("Animator params set - IsRiding:true, VehicleType:{0}, VehicleSpeed:0", this.vehicleTypeValue));
			}
			else
			{
				Debug.LogError("[SimpleRollerSkates] NO ANIMATOR - Cannot find foot bones or set parameters!");
			}
			if (this.skatesPrefabVisual != null)
			{
				this.skatesPrefabVisual.SetActive(false);
				this.LogDebug("Prefab visual HIDDEN");
			}
			this.AttachSkateVisuals();
			this.HidePlayerObjects();
			this.ApplyBlendShapeOverrides();
			this.FreezeVehicle(false);
			if (this.rollerbladeAudioController != null)
			{
				this.rollerbladeAudioController.Activate();
				this.SetupRollerbladeAnimationRelay();
			}
			this.SetupCamera();
			this.mainCamera = Camera.main;
			this.currentSpeed = 0f;
			this.lastSpeed = 0f;
			this.currentSpeedMultiplier = 1f;
			this.isSprinting = false;
			this.vehicleSprintInput.Reset();
			this.moveDirection = Vector3.zero;
			this.inputMagnitude = 0f;
			this.inputBrake = false;
			this.targetLeanAngle = 0f;
			this.currentLeanAngle = 0f;
			this.mountCooldown = 0.5f;
			this.isBeingWorn = true;
			if (CoverPoseController.Instance != null)
			{
				CoverPoseController.Instance.OnVehicleMounted(new VehicleCoverPoseConfig
				{
					allowCover = this.allowCoverWhileRiding,
					armMode = this.coverArmMode,
					poseDriver = null,
					posePreset = null
				});
			}
			UnityEvent onMount = this.OnMount;
			if (onMount != null)
			{
				onMount.Invoke();
			}
			Action<SimpleRollerSkatesInteractable, vThirdPersonController> onAnyMounted = SimpleRollerSkatesInteractable.OnAnyMounted;
			if (onAnyMounted != null)
			{
				onAnyMounted(this, this.currentRider);
			}
			this.rideTransitionInProgress = false;
			this.LogDebug("=".PadRight(50, '='));
			this.LogDebug("=== MOUNT COMPLETE ===");
			this.LogDebug("=".PadRight(50, '='));
		}

		// Token: 0x06002D92 RID: 11666 RVA: 0x000FDF2C File Offset: 0x000FC12C
		private void SetupCamera()
		{
			this.vehicleCameraWasClaimed = false;
			this.vehicleCameraPending = false;
			this.LogDebug("--- Setting up camera ---");
			this.LogDebug(string.Format("useInvectorCamera: {0}", this.useInvectorCamera));
			this.LogDebug("currentRiderInput: " + ((this.currentRiderInput != null) ? "FOUND" : "NULL"));
			if (this.currentRiderInput != null)
			{
				this.LogDebug("currentRiderInput.tpCamera: " + ((this.currentRiderInput.tpCamera != null) ? "FOUND" : "NULL"));
			}
			if (!this.useInvectorCamera || !(this.currentRiderInput != null) || !(this.currentRiderInput.tpCamera != null))
			{
				Debug.LogWarning("[SimpleRollerSkates] Camera setup SKIPPED - missing references");
				return;
			}
			this.tpCamera = this.currentRiderInput.tpCamera;
			if (SmartphoneController.IsAnyTripodActive)
			{
				this.vehicleCameraPending = true;
				this.LogDebug("Tripod owns the camera; roller-skates camera takeover deferred.");
				return;
			}
			this.ClaimVehicleCamera();
		}

		// Token: 0x06002D93 RID: 11667 RVA: 0x000FE03C File Offset: 0x000FC23C
		private void TryRestoreDeferredVehicleCamera()
		{
			if (!this.vehicleCameraPending || !this.isBeingWorn || !this.useInvectorCamera || this.currentRider == null || this.currentRiderInput == null || this.tpCamera == null)
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

		// Token: 0x06002D94 RID: 11668 RVA: 0x000FE104 File Offset: 0x000FC304
		private void ClaimVehicleCamera()
		{
			this.LogDebug("Setting camera target to VEHICLE: " + base.transform.name);
			this.tpCamera.SetMainTarget(base.transform);
			if (!string.IsNullOrEmpty(this.vehicleCameraState))
			{
				this.LogDebug("Changing camera state to: '" + this.vehicleCameraState + "'");
				this.currentRiderInput.ChangeCameraState(this.vehicleCameraState, true);
			}
			else
			{
				this.LogDebug("No vehicleCameraState specified, keeping current camera state");
			}
			this.vehicleCameraWasClaimed = true;
			this.vehicleCameraPending = false;
			string text = "Camera setup complete - Target: ";
			Transform currentTarget = this.tpCamera.currentTarget;
			this.LogDebug(text + ((currentTarget != null) ? currentTarget.name : null) + ", State: " + this.vehicleCameraState);
		}

		// Token: 0x06002D95 RID: 11669 RVA: 0x000FE1C4 File Offset: 0x000FC3C4
		private bool Dismount(bool force = false)
		{
			if (this.rideTransitionInProgress || !this.isBeingWorn || this.currentRider == null)
			{
				this.LogDebug(string.Format("Dismount ABORTED - isBeingWorn={0}, currentRider={1}", this.isBeingWorn, this.currentRider));
				return false;
			}
			if (!force && Mathf.Abs(this.currentSpeed) >= 2f)
			{
				return false;
			}
			this.rideTransitionInProgress = true;
			vThirdPersonController vThirdPersonController = this.currentRider;
			this.LogDebug("=".PadRight(50, '='));
			this.LogDebug("=== DISMOUNTING ROLLER SKATES ===");
			this.LogDebug("=".PadRight(50, '='));
			this.FreezeVehicle(true);
			if (this.rollerbladeAudioController != null)
			{
				this.ClearRollerbladeAnimationRelay();
				this.rollerbladeAudioController.Deactivate();
			}
			this.DetachSkateVisuals();
			this.RestorePlayerObjects();
			this.RestoreBlendShapes();
			if (this.riderAnimator != null)
			{
				this.riderAnimator.SetBool(SimpleRollerSkatesInteractable.IsRidingHash, false);
				this.riderAnimator.SetFloat(SimpleRollerSkatesInteractable.VehicleSpeedHash, 0f);
				this.riderAnimator.applyRootMotion = this.originalApplyRootMotion;
				this.LogDebug(string.Format("Animator params reset - IsRiding:false, VehicleSpeed:0, RootMotion restored to: {0}", this.originalApplyRootMotion));
			}
			Vector3 position = this.riderTransform.position;
			Quaternion quaternion = VehicleDismountSafety.UprightRotation(base.transform, this.riderTransform);
			Vector3 vector = VehicleDismountSafety.FindSafeExitPosition(base.transform, this.riderTransform, this.riderCapsule, 1.15f);
			this.LogDebug(string.Format("Rider world position before unparent: {0}", vector));
			VehicleDismountSafety.RestoreRiderPhysics(this.riderTransform, this.originalRiderParent, vector, quaternion, this.originalRiderLocalScale, this.riderCapsule, this.originalCapsuleEnabled, this.riderRigidbody, this.originalRiderIsKinematic, default(Scene));
			this.LogDebug("Rider unparented. New parent: " + ((this.originalRiderParent != null) ? this.originalRiderParent.name : "NULL (root)"));
			base.transform.position = position;
			base.transform.rotation = quaternion;
			this.LogDebug(string.Format("Prefab dropped at position: {0}", base.transform.position));
			if (this.skatesPrefabVisual != null)
			{
				this.skatesPrefabVisual.SetActive(true);
				this.LogDebug("Prefab visual SHOWN");
			}
			this.LogDebug("Unlocking Invector character controller...");
			this.currentRider.lockMovement = false;
			this.currentRider.lockRotation = false;
			this.currentRider.lockAnimMovement = false;
			this.currentRider.lockAnimRotation = false;
			this.LogDebug("Invector locks released");
			if (this.vehicleCameraWasClaimed && this.useInvectorCamera && this.tpCamera != null && this.currentRiderInput != null)
			{
				this.LogDebug("Resetting camera target to rider: " + this.riderTransform.name);
				this.tpCamera.SetMainTarget(this.riderTransform);
				this.currentRiderInput.ResetCameraState();
				this.LogDebug("Camera state reset to default");
			}
			this.vehicleCameraWasClaimed = false;
			this.vehicleCameraPending = false;
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (instance != null)
			{
				instance.HideIfCurrent(this.interactionConfig);
			}
			if (CoverPoseController.Instance != null)
			{
				CoverPoseController.Instance.OnVehicleDismounted();
			}
			string name = this.currentRider.name;
			this.currentRider = null;
			this.currentRiderInput = null;
			this.riderFirstPerson = null;
			this.tpCamera = null;
			this.riderTransform = null;
			this.riderAnimator = null;
			this.riderRigidbody = null;
			this.riderCapsule = null;
			this.leftFootBone = null;
			this.rightFootBone = null;
			this.originalRiderParent = null;
			this.currentSpeed = 0f;
			this.isGrounded = false;
			this.isBeingWorn = false;
			this.isSprinting = false;
			this.vehicleSprintInput.Reset();
			this.moveDirection = Vector3.zero;
			this.inputMagnitude = 0f;
			this.inputBrake = false;
			this.targetLeanAngle = 0f;
			this.currentLeanAngle = 0f;
			VehicleRideActivityContext.Release(ref this._rideLease);
			UnityEvent onDismount = this.OnDismount;
			if (onDismount != null)
			{
				onDismount.Invoke();
			}
			Action<SimpleRollerSkatesInteractable, vThirdPersonController> onAnyDismounted = SimpleRollerSkatesInteractable.OnAnyDismounted;
			if (onAnyDismounted != null)
			{
				onAnyDismounted(this, vThirdPersonController);
			}
			this.rideTransitionInProgress = false;
			this.LogDebug("=".PadRight(50, '='));
			this.LogDebug("=== DISMOUNT COMPLETE === (Rider: " + name + ")");
			this.LogDebug("=".PadRight(50, '='));
			return true;
		}

		// Token: 0x06002D96 RID: 11670 RVA: 0x000FE638 File Offset: 0x000FC838
		public void ForceCrashDismount()
		{
			this.Dismount(true);
		}

		// Token: 0x06002D97 RID: 11671 RVA: 0x000FE644 File Offset: 0x000FC844
		private void HandleMovementSimple()
		{
			if (this.vehicleRigidbody == null || !this.isGrounded)
			{
				return;
			}
			float magnitude = this.vehicleRigidbody.velocity.magnitude;
			if (this.inputMagnitude > 0.01f && !this.inputBrake)
			{
				Quaternion quaternion = Quaternion.LookRotation(this.moveDirection, Vector3.up);
				float num = 10f;
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, quaternion, num * Time.fixedDeltaTime);
				float num2 = this.inputMagnitude * this.maxSpeed * this.currentSpeedMultiplier * this.gameplaySpeedScale - magnitude;
				float num3 = this.acceleration * Time.fixedDeltaTime;
				if (num2 > 0f)
				{
					float num4 = Mathf.Min(num2, num3);
					this.vehicleRigidbody.velocity = this.moveDirection * (magnitude + num4);
				}
				else
				{
					float num5 = Mathf.Max(num2, -num3 * 2f);
					this.vehicleRigidbody.velocity = this.moveDirection * (magnitude + num5);
				}
			}
			else if (this.inputBrake && magnitude > 0.1f)
			{
				float num6 = this.brakePower * Time.fixedDeltaTime;
				float num7 = Mathf.Max(0f, magnitude - num6);
				this.vehicleRigidbody.velocity = this.vehicleRigidbody.velocity.normalized * num7;
			}
			else if (magnitude > 0.1f)
			{
				float num8 = this.deceleration * Time.fixedDeltaTime;
				float num9 = Mathf.Max(0f, magnitude - num8);
				this.vehicleRigidbody.velocity = this.vehicleRigidbody.velocity.normalized * num9;
			}
			else
			{
				this.vehicleRigidbody.velocity = Vector3.zero;
			}
			if (this.riderAnimator != null)
			{
				float num10 = this.maxSpeed;
				float num11 = this.maxSpeed * this.sprintSpeedMultiplier;
				float num12;
				if (magnitude <= num10)
				{
					num12 = magnitude / num10 * 0.5f;
				}
				else
				{
					float num13 = num11 - num10;
					float num14 = ((num13 > 0f) ? ((magnitude - num10) / num13) : 1f);
					num12 = 0.5f + Mathf.Clamp01(num14) * 0.5f;
				}
				num12 = Mathf.Clamp01(num12);
				this.riderAnimator.SetFloat(SimpleRollerSkatesInteractable.VehicleSpeedHash, num12);
			}
		}

		// Token: 0x06002D98 RID: 11672 RVA: 0x000FE89C File Offset: 0x000FCA9C
		private void ConstrainVehicleRotation()
		{
			if (!this.constrainRotation)
			{
				return;
			}
			Vector3 eulerAngles = base.transform.eulerAngles;
			float num = eulerAngles.x;
			float num2 = eulerAngles.z;
			if (num > 180f)
			{
				num -= 360f;
			}
			if (num2 > 180f)
			{
				num2 -= 360f;
			}
			if (Mathf.Abs(num) > 0.5f || Mathf.Abs(num2) > 0.5f)
			{
				base.transform.rotation = Quaternion.Euler(0f, eulerAngles.y, 0f);
				Vector3 angularVelocity = this.vehicleRigidbody.angularVelocity;
				this.vehicleRigidbody.angularVelocity = new Vector3(0f, angularVelocity.y, 0f);
			}
		}

		// Token: 0x06002D99 RID: 11673 RVA: 0x000FE952 File Offset: 0x000FCB52
		private void UpdateSpeed()
		{
			if (this.vehicleRigidbody == null)
			{
				return;
			}
			this.currentSpeed = Vector3.Dot(this.vehicleRigidbody.velocity, base.transform.forward);
		}

		// Token: 0x06002D9A RID: 11674 RVA: 0x000FE984 File Offset: 0x000FCB84
		private void CheckGround()
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(base.transform.position + Vector3.up * 0.1f, Vector3.down, out raycastHit, this.groundCheckDistance + 0.1f, this.groundLayer, QueryTriggerInteraction.Ignore))
			{
				this.isGrounded = true;
				this.groundNormal = raycastHit.normal;
				return;
			}
			this.isGrounded = false;
			this.groundNormal = Vector3.up;
		}

		// Token: 0x06002D9B RID: 11675 RVA: 0x000FEA00 File Offset: 0x000FCC00
		private void AttachSkateVisuals()
		{
			Debug.Log("[SimpleRollerSkates] AttachSkateVisuals called");
			Debug.Log("[SimpleRollerSkates] - leftSkatePrefab: " + ((this.leftSkatePrefab != null) ? this.leftSkatePrefab.name : "NULL"));
			Debug.Log("[SimpleRollerSkates] - rightSkatePrefab: " + ((this.rightSkatePrefab != null) ? this.rightSkatePrefab.name : "NULL"));
			Debug.Log("[SimpleRollerSkates] - leftFootBone: " + ((this.leftFootBone != null) ? this.leftFootBone.name : "NULL"));
			Debug.Log("[SimpleRollerSkates] - rightFootBone: " + ((this.rightFootBone != null) ? this.rightFootBone.name : "NULL"));
			if (this.leftSkatePrefab != null && this.leftFootBone != null)
			{
				this.leftSkateInstance = Object.Instantiate<GameObject>(this.leftSkatePrefab, this.leftFootBone);
				Debug.Log("[SimpleRollerSkates] LEFT SKATE ATTACHED to " + this.leftFootBone.name);
				Debug.Log(string.Format("[SimpleRollerSkates] LEFT FOOT BONE rotation: {0}", this.leftFootBone.rotation.eulerAngles));
			}
			else
			{
				Debug.LogError(string.Format("[SimpleRollerSkates] FAILED to attach left skate! Prefab={0}, FootBone={1}", this.leftSkatePrefab != null, this.leftFootBone != null));
			}
			if (this.rightSkatePrefab != null && this.rightFootBone != null)
			{
				this.rightSkateInstance = Object.Instantiate<GameObject>(this.rightSkatePrefab, this.rightFootBone);
				Debug.Log("[SimpleRollerSkates] RIGHT SKATE ATTACHED to " + this.rightFootBone.name);
				Debug.Log(string.Format("[SimpleRollerSkates] RIGHT FOOT BONE rotation: {0}", this.rightFootBone.rotation.eulerAngles));
			}
			else
			{
				Debug.LogError(string.Format("[SimpleRollerSkates] FAILED to attach right skate! Prefab={0}, FootBone={1}", this.rightSkatePrefab != null, this.rightFootBone != null));
			}
			this.UpdateSkateOffsets();
			Debug.Log(string.Format("[SimpleRollerSkates] Skate instances created: Left={0}, Right={1}", this.leftSkateInstance != null, this.rightSkateInstance != null));
		}

		// Token: 0x06002D9C RID: 11676 RVA: 0x000FEC58 File Offset: 0x000FCE58
		private void UpdateSkateOffsets()
		{
			if (this.leftSkateInstance != null)
			{
				this.leftSkateInstance.transform.localPosition = this.leftSkatePositionOffset;
				if (this.useWorldAlignedRotation)
				{
					float y = base.transform.eulerAngles.y;
					this.leftSkateInstance.transform.rotation = Quaternion.Euler(this.leftSkateRotationOffset.x, y + this.leftSkateRotationOffset.y, this.leftSkateRotationOffset.z);
				}
				else
				{
					this.leftSkateInstance.transform.localRotation = Quaternion.Euler(this.leftSkateRotationOffset);
				}
			}
			if (this.rightSkateInstance != null)
			{
				this.rightSkateInstance.transform.localPosition = this.rightSkatePositionOffset;
				if (this.useWorldAlignedRotation)
				{
					float y2 = base.transform.eulerAngles.y;
					this.rightSkateInstance.transform.rotation = Quaternion.Euler(this.rightSkateRotationOffset.x, y2 + this.rightSkateRotationOffset.y, this.rightSkateRotationOffset.z);
					return;
				}
				this.rightSkateInstance.transform.localRotation = Quaternion.Euler(this.rightSkateRotationOffset);
			}
		}

		// Token: 0x06002D9D RID: 11677 RVA: 0x000FED90 File Offset: 0x000FCF90
		private void DetachSkateVisuals()
		{
			if (this.leftSkateInstance != null)
			{
				Object.Destroy(this.leftSkateInstance);
				this.leftSkateInstance = null;
			}
			if (this.rightSkateInstance != null)
			{
				Object.Destroy(this.rightSkateInstance);
				this.rightSkateInstance = null;
			}
			Debug.Log("[SimpleRollerSkates] Skate visuals detached");
		}

		// Token: 0x06002D9E RID: 11678 RVA: 0x000FEDE8 File Offset: 0x000FCFE8
		private void HidePlayerObjects()
		{
			this.hiddenObjects.Clear();
			if (this.objectNamesToHide.Count == 0 || this.riderTransform == null)
			{
				return;
			}
			foreach (string text in this.objectNamesToHide)
			{
				if (!string.IsNullOrEmpty(text))
				{
					Transform transform = this.FindChildRecursive(this.riderTransform, text);
					if (transform != null && transform.gameObject.activeSelf)
					{
						transform.gameObject.SetActive(false);
						this.hiddenObjects.Add(transform.gameObject);
						this.LogDebug("Hidden player object: '" + text + "'");
					}
					else if (transform == null)
					{
						this.LogDebug("WARNING: Object '" + text + "' not found on player");
					}
				}
			}
			if (this.hiddenObjects.Count > 0)
			{
				this.LogDebug(string.Format("Total hidden objects: {0}", this.hiddenObjects.Count));
			}
		}

		// Token: 0x06002D9F RID: 11679 RVA: 0x000FEF10 File Offset: 0x000FD110
		private void RestorePlayerObjects()
		{
			foreach (GameObject gameObject in this.hiddenObjects)
			{
				if (gameObject != null)
				{
					gameObject.SetActive(true);
				}
			}
			if (this.hiddenObjects.Count > 0)
			{
				this.LogDebug(string.Format("Restored {0} hidden objects", this.hiddenObjects.Count));
			}
			this.hiddenObjects.Clear();
		}

		// Token: 0x06002DA0 RID: 11680 RVA: 0x000FEFA8 File Offset: 0x000FD1A8
		private void ApplyBlendShapeOverrides()
		{
			if (this.blendShapeOverrides.Count == 0 || this.riderTransform == null)
			{
				return;
			}
			this.originalBlendShapeValues.Clear();
			this.playerSkinnedMeshRenderers.Clear();
			this.FindAllPlayerSkinnedMeshRenderers();
			if (this.playerSkinnedMeshRenderers.Count == 0)
			{
				this.LogDebug("WARNING: No SkinnedMeshRenderer found on player for blend shapes");
				return;
			}
			this.LogDebug(string.Format("Found {0} SkinnedMeshRenderers with blend shapes", this.playerSkinnedMeshRenderers.Count));
			int num = 0;
			foreach (SkateBlendShapeOverride skateBlendShapeOverride in this.blendShapeOverrides)
			{
				if (!string.IsNullOrEmpty(skateBlendShapeOverride.blendShapeName))
				{
					bool flag = false;
					foreach (SkinnedMeshRenderer skinnedMeshRenderer in this.playerSkinnedMeshRenderers)
					{
						if (!(skinnedMeshRenderer == null) && !(skinnedMeshRenderer.sharedMesh == null))
						{
							int blendShapeIndex = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(skateBlendShapeOverride.blendShapeName);
							if (blendShapeIndex >= 0)
							{
								ValueTuple<int, int> valueTuple = new ValueTuple<int, int>(skinnedMeshRenderer.GetInstanceID(), blendShapeIndex);
								this.originalBlendShapeValues[valueTuple] = skinnedMeshRenderer.GetBlendShapeWeight(blendShapeIndex);
								skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, skateBlendShapeOverride.targetValue);
								this.LogDebug(string.Format("BlendShape '{0}' set to {1} on '{2}'", skateBlendShapeOverride.blendShapeName, skateBlendShapeOverride.targetValue, skinnedMeshRenderer.name));
								flag = true;
								num++;
								break;
							}
						}
					}
					if (!flag)
					{
						this.LogDebug("WARNING: BlendShape '" + skateBlendShapeOverride.blendShapeName + "' not found on any renderer");
					}
				}
			}
			if (num > 0)
			{
				this.LogDebug(string.Format("Applied {0} blend shape overrides", num));
			}
		}

		// Token: 0x06002DA1 RID: 11681 RVA: 0x000FF1AC File Offset: 0x000FD3AC
		private void ReapplyBlendShapeOverrides()
		{
			if (this.blendShapeOverrides.Count == 0)
			{
				return;
			}
			if (BlendShapeManager.Instance != null)
			{
				using (List<SkateBlendShapeOverride>.Enumerator enumerator = this.blendShapeOverrides.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						SkateBlendShapeOverride skateBlendShapeOverride = enumerator.Current;
						if (!string.IsNullOrEmpty(skateBlendShapeOverride.blendShapeName))
						{
							BlendShapeManager.Instance.SetOverride(skateBlendShapeOverride.blendShapeName, skateBlendShapeOverride.targetValue, "RollerSkates", BlendShapePriority.Vehicle, false);
						}
					}
					return;
				}
			}
			if (this.playerSkinnedMeshRenderers.Count == 0)
			{
				return;
			}
			foreach (SkateBlendShapeOverride skateBlendShapeOverride2 in this.blendShapeOverrides)
			{
				if (!string.IsNullOrEmpty(skateBlendShapeOverride2.blendShapeName))
				{
					foreach (SkinnedMeshRenderer skinnedMeshRenderer in this.playerSkinnedMeshRenderers)
					{
						if (!(skinnedMeshRenderer == null) && !(skinnedMeshRenderer.sharedMesh == null))
						{
							int blendShapeIndex = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(skateBlendShapeOverride2.blendShapeName);
							if (blendShapeIndex >= 0)
							{
								skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, skateBlendShapeOverride2.targetValue);
								break;
							}
						}
					}
				}
			}
		}

		// Token: 0x06002DA2 RID: 11682 RVA: 0x000FF31C File Offset: 0x000FD51C
		private void RestoreBlendShapes()
		{
			if (BlendShapeManager.Instance != null)
			{
				BlendShapeManager.Instance.RemoveAllOverridesFromSource("RollerSkates");
				this.LogDebug("Removed blend shape overrides from BlendShapeManager");
			}
			else
			{
				if (this.playerSkinnedMeshRenderers.Count == 0 || this.originalBlendShapeValues.Count == 0)
				{
					return;
				}
				int num = 0;
				foreach (KeyValuePair<ValueTuple<int, int>, float> keyValuePair in this.originalBlendShapeValues)
				{
					ValueTuple<int, int> key = keyValuePair.Key;
					int item = key.Item1;
					int item2 = key.Item2;
					float value = keyValuePair.Value;
					foreach (SkinnedMeshRenderer skinnedMeshRenderer in this.playerSkinnedMeshRenderers)
					{
						if (skinnedMeshRenderer != null && skinnedMeshRenderer.GetInstanceID() == item)
						{
							skinnedMeshRenderer.SetBlendShapeWeight(item2, value);
							num++;
							break;
						}
					}
				}
				this.LogDebug(string.Format("Restored {0} blend shapes (legacy)", num));
			}
			this.originalBlendShapeValues.Clear();
			this.playerSkinnedMeshRenderers.Clear();
		}

		// Token: 0x06002DA3 RID: 11683 RVA: 0x000FF460 File Offset: 0x000FD660
		private void FindAllPlayerSkinnedMeshRenderers()
		{
			this.playerSkinnedMeshRenderers.Clear();
			if (this.riderTransform == null)
			{
				return;
			}
			if (this.autoFindSkinnedMeshRenderer)
			{
				foreach (SkinnedMeshRenderer skinnedMeshRenderer in this.riderTransform.GetComponentsInChildren<SkinnedMeshRenderer>(true))
				{
					if (skinnedMeshRenderer.sharedMesh != null && skinnedMeshRenderer.sharedMesh.blendShapeCount > 0)
					{
						this.playerSkinnedMeshRenderers.Add(skinnedMeshRenderer);
						this.LogDebug(string.Format("Found SkinnedMeshRenderer: '{0}' with {1} blend shapes", skinnedMeshRenderer.name, skinnedMeshRenderer.sharedMesh.blendShapeCount));
					}
				}
				return;
			}
			Transform transform = this.FindChildRecursive(this.riderTransform, this.skinnedMeshRendererName);
			if (transform != null)
			{
				SkinnedMeshRenderer component = transform.GetComponent<SkinnedMeshRenderer>();
				if (component != null && component.sharedMesh != null && component.sharedMesh.blendShapeCount > 0)
				{
					this.playerSkinnedMeshRenderers.Add(component);
					this.LogDebug("Found SkinnedMeshRenderer by name: '" + this.skinnedMeshRendererName + "'");
				}
			}
		}

		// Token: 0x06002DA4 RID: 11684 RVA: 0x000FF574 File Offset: 0x000FD774
		private Transform FindChildRecursive(Transform parent, string name)
		{
			if (parent.name.Equals(name, StringComparison.OrdinalIgnoreCase))
			{
				return parent;
			}
			foreach (object obj in parent)
			{
				Transform transform = (Transform)obj;
				Transform transform2 = this.FindChildRecursive(transform, name);
				if (transform2 != null)
				{
					return transform2;
				}
			}
			return null;
		}

		// Token: 0x06002DA5 RID: 11685 RVA: 0x000FF5F0 File Offset: 0x000FD7F0
		private void SetupRollerbladeAnimationRelay()
		{
			if (this.currentRider == null || this.rollerbladeAudioController == null)
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

		// Token: 0x06002DA6 RID: 11686 RVA: 0x000FF660 File Offset: 0x000FD860
		private void ClearRollerbladeAnimationRelay()
		{
			if (this.riderRollerbladeEventRelay == null || this.rollerbladeAudioController == null)
			{
				return;
			}
			this.riderRollerbladeEventRelay.Unbind(this.rollerbladeAudioController);
			this.riderRollerbladeEventRelay = null;
		}

		// Token: 0x06002DA7 RID: 11687 RVA: 0x000FF697 File Offset: 0x000FD897
		private void OnDestroy()
		{
			this.ClearRollerbladeAnimationRelay();
		}

		// Token: 0x06002DA8 RID: 11688 RVA: 0x000FF6A0 File Offset: 0x000FD8A0
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(base.transform.position, this.interactionDistance);
			Gizmos.color = (this.isGrounded ? Color.green : Color.red);
			Vector3 vector = base.transform.position + Vector3.up * 0.1f;
			Gizmos.DrawLine(vector, vector + Vector3.down * (this.groundCheckDistance + 0.1f));
		}

		// Token: 0x06002DA9 RID: 11689 RVA: 0x000FF726 File Offset: 0x000FD926
		private void LogDebug(string message)
		{
			if (this.enableDebugLogs)
			{
				Debug.Log("[SimpleRollerSkates] " + message);
			}
		}

		// Token: 0x04002D3C RID: 11580
		[Header("Interaction")]
		[SerializeField]
		private InteractionConfigSO interactionConfig;

		// Token: 0x04002D3D RID: 11581
		[Tooltip("Maximum distance for interaction. Aligned with the clothes pickup radius (1.1) so every interactable engages at the same distance.")]
		[SerializeField]
		private float interactionDistance = 1.1f;

		// Token: 0x04002D3E RID: 11582
		[Header("Input Settings")]
		[Tooltip("Input to mount/dismount")]
		public GenericInput interactInput = new GenericInput("E", "A", "A");

		// Token: 0x04002D3F RID: 11583
		[Tooltip("Input for acceleration/deceleration")]
		public GenericInput verticalInput = new GenericInput("Vertical", "LeftAnalogVertical", "Vertical");

		// Token: 0x04002D40 RID: 11584
		[Tooltip("Input for steering")]
		public GenericInput horizontalInput = new GenericInput("Horizontal", "LeftAnalogHorizontal", "Horizontal");

		// Token: 0x04002D41 RID: 11585
		[Tooltip("Input for braking")]
		public GenericInput brakeInput = new GenericInput("Space", "X", "X");

		// Token: 0x04002D42 RID: 11586
		[Tooltip("Sprint: toggle with LS on controller; hold Shift on keyboard")]
		public GenericInput sprintInput = new GenericInput("LeftShift", "LeftStickClick", "LeftStickClick");

		// Token: 0x04002D43 RID: 11587
		[Header("Vehicle Rigidbody")]
		[Tooltip("Rigidbody on this prefab for movement (required)")]
		[SerializeField]
		private Rigidbody vehicleRigidbody;

		// Token: 0x04002D44 RID: 11588
		[Header("Player References (Optional - auto-detected if empty)")]
		[Tooltip("Assign the GameObject with Animator (e.g., Ciel_Invector)")]
		[SerializeField]
		private Animator playerAnimator;

		// Token: 0x04002D45 RID: 11589
		[Header("Skate Prefabs")]
		[SerializeField]
		private GameObject leftSkatePrefab;

		// Token: 0x04002D46 RID: 11590
		[SerializeField]
		private GameObject rightSkatePrefab;

		// Token: 0x04002D47 RID: 11591
		[Header("Left Skate Offset")]
		[SerializeField]
		private Vector3 leftSkatePositionOffset = new Vector3(0f, -0.05f, 0f);

		// Token: 0x04002D48 RID: 11592
		[SerializeField]
		private Vector3 leftSkateRotationOffset = Vector3.zero;

		// Token: 0x04002D49 RID: 11593
		[Header("Right Skate Offset")]
		[SerializeField]
		private Vector3 rightSkatePositionOffset = new Vector3(0f, -0.05f, 0f);

		// Token: 0x04002D4A RID: 11594
		[SerializeField]
		private Vector3 rightSkateRotationOffset = Vector3.zero;

		// Token: 0x04002D4B RID: 11595
		[Header("Skate Rotation Mode")]
		[Tooltip("If true, skates maintain world-aligned rotation (flat on ground) instead of following foot rotation")]
		[SerializeField]
		private bool useWorldAlignedRotation = true;

		// Token: 0x04002D4C RID: 11596
		[Header("Skate Attachment")]
		[Tooltip("Which bone to attach skates to. Toes may be more stable than Foot for some rigs.")]
		[SerializeField]
		private HumanBodyBones leftSkateAttachBone = HumanBodyBones.LeftFoot;

		// Token: 0x04002D4D RID: 11597
		[SerializeField]
		private HumanBodyBones rightSkateAttachBone = HumanBodyBones.RightFoot;

		// Token: 0x04002D4E RID: 11598
		[Header("Movement")]
		[SerializeField]
		private float maxSpeed = 15f;

		// Token: 0x04002D4F RID: 11599
		[Tooltip("Scales gameplay speed without altering animator threshold normalization")]
		[Range(0.2f, 1f)]
		[SerializeField]
		private float gameplaySpeedScale = 1f;

		// Token: 0x04002D50 RID: 11600
		[Header("Sprint Settings")]
		[Tooltip("Speed multiplier when sprinting (holding Shift)")]
		[Range(1f, 3f)]
		[SerializeField]
		private float sprintSpeedMultiplier = 1.5f;

		// Token: 0x04002D51 RID: 11601
		[Tooltip("How quickly the sprint multiplier transitions")]
		[Range(1f, 20f)]
		[SerializeField]
		private float sprintTransitionRate = 5f;

		// Token: 0x04002D52 RID: 11602
		[SerializeField]
		private float acceleration = 10f;

		// Token: 0x04002D53 RID: 11603
		[SerializeField]
		private float deceleration = 2f;

		// Token: 0x04002D54 RID: 11604
		[SerializeField]
		private float brakePower = 20f;

		// Token: 0x04002D55 RID: 11605
		[Range(0.01f, 1f)]
		[SerializeField]
		private float turnSmoothing = 0.1f;

		// Token: 0x04002D56 RID: 11606
		[Range(0.01f, 1f)]
		[SerializeField]
		private float inputSmoothing = 0.1f;

		// Token: 0x04002D57 RID: 11607
		[Header("Rotation Settings")]
		[Tooltip("VelocitÃ\u00a0 di rotazione quando fermo (gradi/secondo)")]
		[SerializeField]
		private float stationaryTurnSpeed = 280f;

		// Token: 0x04002D58 RID: 11608
		[Tooltip("VelocitÃ\u00a0 di rotazione in movimento (gradi/secondo)")]
		[SerializeField]
		private float movingTurnSpeed = 150f;

		// Token: 0x04002D59 RID: 11609
		[Tooltip("VelocitÃ\u00a0 minima per considerare il personaggio in movimento")]
		[SerializeField]
		private float movementThreshold = 0.5f;

		// Token: 0x04002D5A RID: 11610
		[Tooltip("Quanto ridurre la rotazione alla velocitÃ\u00a0 massima (0-0.8)")]
		[Range(0f, 0.8f)]
		[SerializeField]
		private float highSpeedTurnReduction = 0.4f;

		// Token: 0x04002D5B RID: 11611
		[Header("Leaning")]
		[SerializeField]
		private float maxLeanAngle = 25f;

		// Token: 0x04002D5C RID: 11612
		[Range(0.01f, 1f)]
		[SerializeField]
		private float leanSmoothing = 0.1f;

		// Token: 0x04002D5D RID: 11613
		[SerializeField]
		private Transform leanRoot;

		// Token: 0x04002D5E RID: 11614
		[Header("Camera Settings")]
		[Tooltip("Camera state name when riding")]
		[SerializeField]
		private string vehicleCameraState = "Bike";

		// Token: 0x04002D5F RID: 11615
		[SerializeField]
		private bool useInvectorCamera = true;

		// Token: 0x04002D60 RID: 11616
		[Tooltip("Use camera-relative input like Invector locomotion (W moves towards camera forward)")]
		[SerializeField]
		private bool useCameraRelativeInput = true;

		// Token: 0x04002D61 RID: 11617
		[Header("Cover Pose Compatibility")]
		[Tooltip("If true, the player can use cover pose while riding this vehicle (e.g. roller skates allow it, bikes don't).")]
		[SerializeField]
		private bool allowCoverWhileRiding = true;

		// Token: 0x04002D62 RID: 11618
		[Tooltip("Which arm(s) CoverPose can control while riding these skates")]
		[SerializeField]
		private VehicleCoverArmMode coverArmMode;

		// Token: 0x04002D63 RID: 11619
		[Header("Animator Settings")]
		[Tooltip("VehicleType parameter value (2 = RollerSkates)")]
		[SerializeField]
		private float vehicleTypeValue = 2f;

		// Token: 0x04002D64 RID: 11620
		[Header("Visual")]
		[SerializeField]
		private GameObject skatesPrefabVisual;

		// Token: 0x04002D65 RID: 11621
		[Header("UI")]
		[SerializeField]
		private InteractablePromptUI promptUI;

		// Token: 0x04002D66 RID: 11622
		[Header("Stability Settings")]
		[Tooltip("Ground check distance")]
		[SerializeField]
		private float groundCheckDistance = 0.3f;

		// Token: 0x04002D67 RID: 11623
		[Tooltip("Layer mask for ground detection")]
		[SerializeField]
		private LayerMask groundLayer = -1;

		// Token: 0x04002D68 RID: 11624
		[Tooltip("Constrain rotation to prevent tipping")]
		[SerializeField]
		private bool constrainRotation = true;

		// Token: 0x04002D69 RID: 11625
		[Tooltip("PhysicMaterial for the vehicle collider (use zero friction to prevent wall sticking)")]
		[SerializeField]
		private PhysicMaterial vehiclePhysicMaterial;

		// Token: 0x04002D6A RID: 11626
		[Tooltip("Auto-create zero friction material if none assigned")]
		[SerializeField]
		private bool autoCreateZeroFrictionMaterial = true;

		// Token: 0x04002D6B RID: 11627
		[Header("Player Visual Overrides")]
		[Tooltip("Nomi dei GameObject da nascondere sul player quando indossa i pattini (es. 'Shoes', 'LeftShoe'). Cerca ricorsivamente nella gerarchia del player.")]
		[SerializeField]
		private List<string> objectNamesToHide = new List<string>();

		// Token: 0x04002D6C RID: 11628
		[Tooltip("Blend shapes da modificare quando si indossano i pattini (es. piedi in posizione tacchi)")]
		[SerializeField]
		private List<SkateBlendShapeOverride> blendShapeOverrides = new List<SkateBlendShapeOverride>();

		// Token: 0x04002D6D RID: 11629
		[Tooltip("Se true, cerca il SkinnedMeshRenderer automaticamente sul player. Altrimenti usa quello specificato.")]
		[SerializeField]
		private bool autoFindSkinnedMeshRenderer = true;

		// Token: 0x04002D6E RID: 11630
		[Tooltip("Nome del SkinnedMeshRenderer da usare per i blend shapes (se autoFind Ã\u00a8 false)")]
		[SerializeField]
		private string skinnedMeshRendererName = "Body";

		// Token: 0x04002D6F RID: 11631
		[Header("Debug")]
		[SerializeField]
		private bool enableDebugLogs = true;

		// Token: 0x04002D70 RID: 11632
		[SerializeField]
		private bool enablePeriodicLogs;

		// Token: 0x04002D71 RID: 11633
		[Tooltip("Log every N frames when periodic logs enabled")]
		[SerializeField]
		private int periodicLogInterval = 60;

		// Token: 0x04002D72 RID: 11634
		[Header("Events")]
		public UnityEvent OnMount;

		// Token: 0x04002D73 RID: 11635
		public UnityEvent OnDismount;

		// Token: 0x04002D74 RID: 11636
		public UnityEvent<float> OnSpeedChanged;

		// Token: 0x04002D77 RID: 11639
		private bool isBeingWorn;

		// Token: 0x04002D78 RID: 11640
		private bool rideTransitionInProgress;

		// Token: 0x04002D79 RID: 11641
		private Transform riderTransform;

		// Token: 0x04002D7A RID: 11642
		private Animator riderAnimator;

		// Token: 0x04002D7B RID: 11643
		private Rigidbody riderRigidbody;

		// Token: 0x04002D7C RID: 11644
		private CapsuleCollider riderCapsule;

		// Token: 0x04002D7D RID: 11645
		private Transform originalRiderParent;

		// Token: 0x04002D7E RID: 11646
		private Vector3 originalRiderLocalScale;

		// Token: 0x04002D7F RID: 11647
		private Transform leftFootBone;

		// Token: 0x04002D80 RID: 11648
		private Transform rightFootBone;

		// Token: 0x04002D81 RID: 11649
		private GameObject leftSkateInstance;

		// Token: 0x04002D82 RID: 11650
		private GameObject rightSkateInstance;

		// Token: 0x04002D83 RID: 11651
		private vThirdPersonController currentRider;

		// Token: 0x04002D84 RID: 11652
		private vThirdPersonInput currentRiderInput;

		// Token: 0x04002D85 RID: 11653
		private FirstPersonController riderFirstPerson;

		// Token: 0x04002D86 RID: 11654
		private vThirdPersonCamera tpCamera;

		// Token: 0x04002D87 RID: 11655
		private bool vehicleCameraWasClaimed;

		// Token: 0x04002D88 RID: 11656
		private bool vehicleCameraPending;

		// Token: 0x04002D89 RID: 11657
		private RollerbladeAudioController rollerbladeAudioController;

		// Token: 0x04002D8A RID: 11658
		private RollerbladeAnimationEventRelay riderRollerbladeEventRelay;

		// Token: 0x04002D8B RID: 11659
		private bool isPlayerInRange;

		// Token: 0x04002D8C RID: 11660
		private Transform playerTransform;

		// Token: 0x04002D8D RID: 11661
		private readonly VehiclePlayerLookup _playerLookup = new VehiclePlayerLookup();

		// Token: 0x04002D8E RID: 11662
		private bool originalRiderIsKinematic;

		// Token: 0x04002D8F RID: 11663
		private bool originalCapsuleEnabled;

		// Token: 0x04002D90 RID: 11664
		private bool originalApplyRootMotion;

		// Token: 0x04002D91 RID: 11665
		private static readonly int IsRidingHash = Animator.StringToHash("IsRiding");

		// Token: 0x04002D92 RID: 11666
		private static readonly int VehicleTypeHash = Animator.StringToHash("VehicleType");

		// Token: 0x04002D93 RID: 11667
		private static readonly int VehicleSpeedHash = Animator.StringToHash("VehicleSpeed");

		// Token: 0x04002D94 RID: 11668
		private float currentSpeed;

		// Token: 0x04002D95 RID: 11669
		private float lastSpeed;

		// Token: 0x04002D96 RID: 11670
		private float targetSpeed;

		// Token: 0x04002D97 RID: 11671
		private float currentSpeedMultiplier = 1f;

		// Token: 0x04002D98 RID: 11672
		private bool isSprinting;

		// Token: 0x04002D99 RID: 11673
		private readonly VehicleSprintInput vehicleSprintInput = new VehicleSprintInput();

		// Token: 0x04002D9A RID: 11674
		private float currentLeanAngle;

		// Token: 0x04002D9B RID: 11675
		private bool isGrounded;

		// Token: 0x04002D9C RID: 11676
		private Camera mainCamera;

		// Token: 0x04002D9D RID: 11677
		private Vector3 groundNormal = Vector3.up;

		// Token: 0x04002D9E RID: 11678
		private float mountCooldown;

		// Token: 0x04002D9F RID: 11679
		private const float MOUNT_COOLDOWN_TIME = 0.5f;

		// Token: 0x04002DA0 RID: 11680
		private int frameCounter;

		// Token: 0x04002DA1 RID: 11681
		private List<GameObject> hiddenObjects = new List<GameObject>();

		// Token: 0x04002DA2 RID: 11682
		private Dictionary<ValueTuple<int, int>, float> originalBlendShapeValues = new Dictionary<ValueTuple<int, int>, float>();

		// Token: 0x04002DA3 RID: 11683
		private List<SkinnedMeshRenderer> playerSkinnedMeshRenderers = new List<SkinnedMeshRenderer>();

		// Token: 0x04002DA4 RID: 11684
		private float _sqrDistToPlayer = float.MaxValue;

		// Token: 0x04002DA5 RID: 11685
		private PlayerActivityLease _rideLease;

		// Token: 0x04002DA6 RID: 11686
		private Vector3 moveDirection;

		// Token: 0x04002DA7 RID: 11687
		private float inputMagnitude;

		// Token: 0x04002DA8 RID: 11688
		private bool inputBrake;

		// Token: 0x04002DA9 RID: 11689
		private float targetLeanAngle;
	}
}
