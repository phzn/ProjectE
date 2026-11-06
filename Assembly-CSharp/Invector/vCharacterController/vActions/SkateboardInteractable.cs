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
using ProjectExhibitionist.Interactions;
using RayznGames.VehicleSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Invector.vCharacterController.vActions
{
	// Token: 0x02000700 RID: 1792
	public class SkateboardInteractable : MonoBehaviour, IInteractable, IInteractionCandidate
	{
		// Token: 0x1400004F RID: 79
		// (add) Token: 0x06002DB0 RID: 11696 RVA: 0x000FFC0C File Offset: 0x000FDE0C
		// (remove) Token: 0x06002DB1 RID: 11697 RVA: 0x000FFC40 File Offset: 0x000FDE40
		public static event Action<SkateboardInteractable, vThirdPersonController> OnAnyMounted;

		// Token: 0x14000050 RID: 80
		// (add) Token: 0x06002DB2 RID: 11698 RVA: 0x000FFC74 File Offset: 0x000FDE74
		// (remove) Token: 0x06002DB3 RID: 11699 RVA: 0x000FFCA8 File Offset: 0x000FDEA8
		public static event Action<SkateboardInteractable, vThirdPersonController> OnAnyDismounted;

		// Token: 0x170008B1 RID: 2225
		// (get) Token: 0x06002DB4 RID: 11700 RVA: 0x000FFCDB File Offset: 0x000FDEDB
		public InteractionConfigSO Config
		{
			get
			{
				return this.interactionConfig;
			}
		}

		// Token: 0x170008B2 RID: 2226
		// (get) Token: 0x06002DB5 RID: 11701 RVA: 0x000FFCE3 File Offset: 0x000FDEE3
		public bool IsBeingUsed
		{
			get
			{
				return this.isBeingWorn;
			}
		}

		// Token: 0x06002DB6 RID: 11702 RVA: 0x000FFCEC File Offset: 0x000FDEEC
		public bool CanInteract(GameObject player)
		{
			if (this.isBeingWorn)
			{
				return false;
			}
			vThirdPersonController component = player.GetComponent<vThirdPersonController>();
			return (!(component != null) || !component.lockMovement) && VehicleRideActivityContext.CanAcquire;
		}

		// Token: 0x170008B3 RID: 2227
		// (get) Token: 0x06002DB7 RID: 11703 RVA: 0x000FFCE3 File Offset: 0x000FDEE3
		public bool IsBeingWorn
		{
			get
			{
				return this.isBeingWorn;
			}
		}

		// Token: 0x170008B4 RID: 2228
		// (get) Token: 0x06002DB8 RID: 11704 RVA: 0x000FFD22 File Offset: 0x000FDF22
		public bool IsTransitioning
		{
			get
			{
				return this.rideTransitionInProgress;
			}
		}

		// Token: 0x170008B5 RID: 2229
		// (get) Token: 0x06002DB9 RID: 11705 RVA: 0x000FFD2A File Offset: 0x000FDF2A
		public vThirdPersonController CurrentRider
		{
			get
			{
				return this.currentRider;
			}
		}

		// Token: 0x170008B6 RID: 2230
		// (get) Token: 0x06002DBA RID: 11706 RVA: 0x000FFD32 File Offset: 0x000FDF32
		public float CurrentSpeed
		{
			get
			{
				return this.currentSpeed;
			}
		}

		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x06002DBB RID: 11707 RVA: 0x000FFD3A File Offset: 0x000FDF3A
		public bool InBalance
		{
			get
			{
				return this.inBalance;
			}
		}

		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x06002DBC RID: 11708 RVA: 0x000FFD42 File Offset: 0x000FDF42
		public bool TrickActive
		{
			get
			{
				return this.trickActive;
			}
		}

		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x06002DBD RID: 11709 RVA: 0x000FFD4A File Offset: 0x000FDF4A
		public float TrickCooldownSeconds
		{
			get
			{
				return Mathf.Max(0f, this.trickCooldownSeconds);
			}
		}

		// Token: 0x170008BA RID: 2234
		// (get) Token: 0x06002DBE RID: 11710 RVA: 0x000FFD5C File Offset: 0x000FDF5C
		public float TrickCooldownRemaining
		{
			get
			{
				return Mathf.Max(0f, this.trickCooldownRemaining);
			}
		}

		// Token: 0x06002DBF RID: 11711 RVA: 0x000FFD70 File Offset: 0x000FDF70
		private void Awake()
		{
			ControllerContextBindings.SetLabel(this.interactionConfig, "shift", "L3", false);
			ControllerContextBindings.SetLabel(this.interactionConfig, "space", "A", false);
			ControllerContextBindings.SetLabel(this.interactionConfig, "E", "B", false);
			ControllerContextBindings.SetLabel(this.interactionConfig, "F", "Y", false);
			ControllerContextBindings.SetLabel(this.trickActiveHudConfig, "F", "Y", false);
			ControllerContextBindings.SetLabel(this.trickActiveHudConfig, "space", "A", false);
			ControllerContextBindings.Set(this.brakeInput, "A");
			ControllerContextBindings.Set(this.sprintInput, "LeftStickClick");
			ControllerContextBindings.Set(this.trickInput, "Y");
			if (this.vehicleRigidbody == null)
			{
				this.vehicleRigidbody = base.GetComponent<Rigidbody>();
				if (this.vehicleRigidbody == null)
				{
					Debug.LogError("[Skateboard] No Rigidbody found! Please add and configure one in the Inspector.");
				}
			}
			Collider component = base.GetComponent<Collider>();
			if (component == null)
			{
				Debug.LogWarning("[Skateboard] No Collider found! Please add one in the Inspector.");
				return;
			}
			if (this.vehiclePhysicMaterial != null)
			{
				component.material = this.vehiclePhysicMaterial;
				return;
			}
			if (this.autoCreateZeroFrictionMaterial)
			{
				component.material = new PhysicMaterial("ZeroFriction")
				{
					dynamicFriction = 0f,
					staticFriction = 0f,
					frictionCombine = PhysicMaterialCombine.Minimum,
					bounciness = 0f,
					bounceCombine = PhysicMaterialCombine.Minimum
				};
			}
		}

		// Token: 0x06002DC0 RID: 11712 RVA: 0x000FFEE0 File Offset: 0x000FE0E0
		private void Start()
		{
			this.FreezeVehicle(true);
			this.playerTransform = this._playerLookup.Resolve();
		}

		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x06002DC1 RID: 11713 RVA: 0x000FFEFC File Offset: 0x000FE0FC
		private string RideGateReason
		{
			get
			{
				return "VehicleRide#" + base.GetInstanceID().ToString();
			}
		}

		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x06002DC2 RID: 11714 RVA: 0x000FFF21 File Offset: 0x000FE121
		public float CandidateSqrDistance
		{
			get
			{
				return this._sqrDistToPlayer;
			}
		}

		// Token: 0x170008BD RID: 2237
		// (get) Token: 0x06002DC3 RID: 11715 RVA: 0x000FFF29 File Offset: 0x000FE129
		public bool IsCandidateEligible
		{
			get
			{
				return this.isPlayerInRange && !this.isBeingWorn && this.playerTransform != null && this.CanInteract(this.playerTransform.gameObject);
			}
		}

		// Token: 0x170008BE RID: 2238
		// (get) Token: 0x06002DC4 RID: 11716 RVA: 0x000FCE88 File Offset: 0x000FB088
		public int CandidateInstanceId
		{
			get
			{
				return base.GetInstanceID();
			}
		}

		// Token: 0x06002DC5 RID: 11717 RVA: 0x000FCE90 File Offset: 0x000FB090
		private void OnEnable()
		{
			InteractionCandidateRegistry.Register(this);
		}

		// Token: 0x06002DC6 RID: 11718 RVA: 0x000FFF5C File Offset: 0x000FE15C
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

		// Token: 0x06002DC7 RID: 11719 RVA: 0x000FFFA8 File Offset: 0x000FE1A8
		private void SyncRideGate()
		{
			if (this.isBeingWorn && (this._rideLease == null || !this._rideLease.IsActive))
			{
				if (!VehicleRideActivityContext.TryAcquire(this.RideGateReason, this, ref this._rideLease))
				{
					Debug.LogError("[Skateboard] Ride context was lost and could not be restored; forcing a safe dismount.", this);
					this.ForceCrashDismount();
					return;
				}
			}
			else if (!this.isBeingWorn)
			{
				VehicleRideActivityContext.Release(ref this._rideLease);
			}
		}

		// Token: 0x06002DC8 RID: 11720 RVA: 0x0010000C File Offset: 0x000FE20C
		private void Update()
		{
			if (FlyCamera.IsActive)
			{
				this.isSprinting = false;
				this.vehicleSprintInput.Reset();
				return;
			}
			this.SyncRideGate();
			this.TickTrickCooldown(Time.deltaTime);
			if (this.mountCooldown > 0f)
			{
				this.mountCooldown -= Time.deltaTime;
			}
			if (this.isBeingWorn)
			{
				this.HandleInput();
				this.CheckDismount();
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

		// Token: 0x06002DC9 RID: 11721 RVA: 0x001000BA File Offset: 0x000FE2BA
		private void LateUpdate()
		{
			this.TryRestoreDeferredVehicleCamera();
			this.MaintainRidingHud();
		}

		// Token: 0x06002DCA RID: 11722 RVA: 0x001000C8 File Offset: 0x000FE2C8
		private void FixedUpdate()
		{
			if (!this.isBeingWorn)
			{
				return;
			}
			this.CheckGround();
			this.HandleMovementSimple();
			this.ConstrainVehicleRotation();
			this.UpdateSpeed();
			this.ReapplyBlendShapeOverrides();
		}

		// Token: 0x06002DCB RID: 11723 RVA: 0x001000F4 File Offset: 0x000FE2F4
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
			if (!PlayerActivityGate.IsResourceOwned(PlayerContextResource.ModalUI) && this.trickInput.GetButtonDown() && !this.TryToggleTrick())
			{
				BlockedActionFeedback.Show(string.Format("Trick ready in {0}s", Mathf.CeilToInt(this.TrickCooldownRemaining)));
			}
			if (!(this.mainCamera != null))
			{
				this.moveDirection = base.transform.forward * axis + base.transform.right * axis2;
				this.inputMagnitude = Mathf.Clamp01(this.moveDirection.magnitude);
				if (this.inputMagnitude > 0.01f)
				{
					this.moveDirection.Normalize();
				}
				return;
			}
			Vector3 forward = this.mainCamera.transform.forward;
			Vector3 right = this.mainCamera.transform.right;
			forward.y = 0f;
			right.y = 0f;
			forward.Normalize();
			right.Normalize();
			this.moveDirection = forward * axis + right * axis2;
			this.inputMagnitude = Mathf.Clamp01(this.moveDirection.magnitude);
			if (this.inputMagnitude > 0.01f)
			{
				this.moveDirection.Normalize();
				return;
			}
			this.moveDirection = Vector3.zero;
		}

		// Token: 0x06002DCC RID: 11724 RVA: 0x001002F0 File Offset: 0x000FE4F0
		private bool TryToggleTrick()
		{
			if (!this.trickActive && this.trickCooldownRemaining > 0f)
			{
				return false;
			}
			this.trickActive = !this.trickActive;
			if (this.trickActive)
			{
				this.trickCooldownRemaining = this.TrickCooldownSeconds;
				this.pendingTrickBoost = true;
				this.PresentRidingHud();
				this.LogDebug("Trick STARTED (boost queued)");
			}
			else
			{
				this.PresentRidingHud();
				this.LogDebug("Trick ENDED");
			}
			return true;
		}

		// Token: 0x06002DCD RID: 11725 RVA: 0x00100363 File Offset: 0x000FE563
		private void TickTrickCooldown(float deltaTime)
		{
			if (this.trickCooldownRemaining <= 0f || deltaTime <= 0f)
			{
				return;
			}
			this.trickCooldownRemaining = Mathf.Max(0f, this.trickCooldownRemaining - deltaTime);
		}

		// Token: 0x170008BF RID: 2239
		// (get) Token: 0x06002DCE RID: 11726 RVA: 0x00100393 File Offset: 0x000FE593
		private InteractionConfigSO DesiredRidingHudConfig
		{
			get
			{
				if (!this.trickActive || !(this.trickActiveHudConfig != null))
				{
					return this.interactionConfig;
				}
				return this.trickActiveHudConfig;
			}
		}

		// Token: 0x06002DCF RID: 11727 RVA: 0x001003B8 File Offset: 0x000FE5B8
		private void PresentRidingHud()
		{
			InteractionConfigSO desiredRidingHudConfig = this.DesiredRidingHudConfig;
			if (desiredRidingHudConfig != null)
			{
				InteractionHUDController instance = InteractionHUDController.Instance;
				if (instance == null)
				{
					return;
				}
				instance.Show(desiredRidingHudConfig);
			}
		}

		// Token: 0x06002DD0 RID: 11728 RVA: 0x001003E8 File Offset: 0x000FE5E8
		private void MaintainRidingHud()
		{
			if (!this.isBeingWorn || SmartphoneController.IsPhonePointerActive || SmartphoneController.IsAnySelfieActive || PlayerActivityGate.IsResourceOwned(PlayerContextResource.ModalUI))
			{
				return;
			}
			ExhibitionEventDirector instance = ExhibitionEventDirector.Instance;
			if (instance != null && instance.isActiveAndEnabled && instance.IsEventActive)
			{
				return;
			}
			InteractionHUDController instance2 = InteractionHUDController.Instance;
			InteractionConfigSO desiredRidingHudConfig = this.DesiredRidingHudConfig;
			if (instance2 != null && desiredRidingHudConfig != null && (!instance2.IsShowing || instance2.CurrentConfig != desiredRidingHudConfig))
			{
				instance2.Show(desiredRidingHudConfig);
			}
		}

		// Token: 0x06002DD1 RID: 11729 RVA: 0x00100474 File Offset: 0x000FE674
		private void HideOwnedHud()
		{
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (instance == null)
			{
				return;
			}
			instance.HideIfCurrent(this.interactionConfig);
			if (this.trickActiveHudConfig != null)
			{
				instance.HideIfCurrent(this.trickActiveHudConfig);
			}
		}

		// Token: 0x06002DD2 RID: 11730 RVA: 0x001004BC File Offset: 0x000FE6BC
		private void TriggerNearFall()
		{
			if (this.fallActive || this.trickActive || !this.inBalance)
			{
				return;
			}
			this.fallActive = true;
			this.fallTimer = this.fallDuration;
			if (this.riderAnimator != null)
			{
				this.riderAnimator.SetTrigger(SkateboardInteractable.SkateFallHash);
			}
			PlayerEmotionController instance = PlayerEmotionController.Instance;
			if (instance != null)
			{
				instance.DisableFacialExpressionForAction();
				instance.SetExpression(this.fallFaceExpressionIndex);
				this.holdingFallFace = true;
			}
			this.LogDebug("Near-fall TRIGGERED (random)");
		}

		// Token: 0x06002DD3 RID: 11731 RVA: 0x00100546 File Offset: 0x000FE746
		private void RestoreFallFace()
		{
			if (!this.holdingFallFace)
			{
				return;
			}
			this.holdingFallFace = false;
			PlayerEmotionController instance = PlayerEmotionController.Instance;
			if (instance == null)
			{
				return;
			}
			instance.RestoreFacialExpression();
		}

		// Token: 0x06002DD4 RID: 11732 RVA: 0x00100567 File Offset: 0x000FE767
		private void ScheduleNextRandomFall()
		{
			this.fallIntervalTimer = 0f;
			this.nextFallDelay = this.randomFallMinInterval + Random.Range(0f, this.randomFallExtraRandom);
		}

		// Token: 0x06002DD5 RID: 11733 RVA: 0x00100594 File Offset: 0x000FE794
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
		}

		// Token: 0x06002DD6 RID: 11734 RVA: 0x0010065C File Offset: 0x000FE85C
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

		// Token: 0x06002DD7 RID: 11735 RVA: 0x0010078D File Offset: 0x000FE98D
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

		// Token: 0x06002DD8 RID: 11736 RVA: 0x001007BC File Offset: 0x000FE9BC
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
				this.PresentRidingHud();
			}
		}

		// Token: 0x06002DD9 RID: 11737 RVA: 0x00004F31 File Offset: 0x00003131
		public void OnActionExecuted(string actionName, GameObject player)
		{
		}

		// Token: 0x06002DDA RID: 11738 RVA: 0x00100820 File Offset: 0x000FEA20
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
			this.LogDebug("=== MOUNTING SKATEBOARD === Rider: " + rider.name);
			this.currentRider = rider;
			this.currentRiderInput = rider.GetComponent<vThirdPersonInput>();
			this.riderTransform = rider.transform;
			this.riderAnimator = ((this.playerAnimator != null) ? this.playerAnimator : rider.GetComponent<Animator>());
			this.riderRigidbody = rider.GetComponent<Rigidbody>();
			this.riderCapsule = rider.GetComponent<CapsuleCollider>();
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
			this.currentRider.lockMovement = true;
			this.currentRider.lockRotation = true;
			this.currentRider.lockAnimMovement = true;
			this.currentRider.lockAnimRotation = true;
			if (this.riderCapsule != null)
			{
				this.riderCapsule.enabled = false;
			}
			if (this.riderRigidbody != null)
			{
				this.riderRigidbody.isKinematic = true;
			}
			base.transform.position = this.riderTransform.position;
			base.transform.rotation = Quaternion.Euler(0f, this.riderTransform.eulerAngles.y, 0f);
			this.riderTransform.SetParent(base.transform, true);
			this.riderTransform.localPosition = Vector3.zero;
			this.riderTransform.localRotation = Quaternion.identity;
			if (this.riderAnimator != null)
			{
				this.originalApplyRootMotion = this.riderAnimator.applyRootMotion;
				this.riderAnimator.applyRootMotion = false;
				this.animatorUpdateModeLease = AnimatorNormalUpdateModeLease.Acquire(this.riderAnimator);
				this.originalAnimatorSpeed = this.riderAnimator.speed;
				this.riderAnimator.speed = this.originalAnimatorSpeed * this.animationSpeedMultiplier;
				this.riderAnimator.SetBool(SkateboardInteractable.IsRidingHash, true);
				this.riderAnimator.SetFloat(SkateboardInteractable.VehicleTypeHash, this.vehicleTypeValue);
				this.riderAnimator.SetInteger(SkateboardInteractable.SkateStateHash, 0);
				this.riderAnimator.SetFloat(SkateboardInteractable.SkatePushSpeedHash, this.walkPushClipSpeed);
				this.riderAnimator.SetFloat(SkateboardInteractable.SkateTurnHash, 0f);
			}
			else
			{
				Debug.LogError("[Skateboard] NO ANIMATOR - Cannot set parameters!");
			}
			this.HidePlayerObjects();
			this.ApplyBlendShapeOverrides();
			this.FreezeVehicle(false);
			this.SetupCamera();
			this.mainCamera = Camera.main;
			this.currentSpeed = 0f;
			this.lastSpeed = 0f;
			this.isSprinting = false;
			this.vehicleSprintInput.Reset();
			this.inBalance = false;
			this.trickActive = false;
			this.pendingTrickBoost = false;
			this.fallActive = false;
			this.fallTimer = 0f;
			this.ScheduleNextRandomFall();
			if (this.riderAnimator != null)
			{
				this.riderAnimator.ResetTrigger(SkateboardInteractable.SkateFallHash);
			}
			this.smoothedTurn = 0f;
			this.smoothedTurnVelocity = 0f;
			this.planarVelocity = Vector3.zero;
			this.moveDirection = Vector3.zero;
			this.inputMagnitude = 0f;
			this.inputBrake = false;
			this.mountCooldown = 0.5f;
			this.isBeingWorn = true;
			if (CoverPoseController.Instance != null)
			{
				CoverPoseController.Instance.OnVehicleMounted(new VehicleCoverPoseConfig
				{
					allowCover = this.allowCoverWhileRiding,
					armMode = this.coverArmMode
				});
			}
			UnityEvent onMount = this.OnMount;
			if (onMount != null)
			{
				onMount.Invoke();
			}
			Action<SkateboardInteractable, vThirdPersonController> onAnyMounted = SkateboardInteractable.OnAnyMounted;
			if (onAnyMounted != null)
			{
				onAnyMounted(this, this.currentRider);
			}
			this.rideTransitionInProgress = false;
			this.LogDebug("=== MOUNT COMPLETE ===");
		}

		// Token: 0x06002DDB RID: 11739 RVA: 0x00100C68 File Offset: 0x000FEE68
		private void SetupCamera()
		{
			this.vehicleCameraWasClaimed = false;
			this.vehicleCameraPending = false;
			if (!this.useInvectorCamera || !(this.currentRiderInput != null) || !(this.currentRiderInput.tpCamera != null))
			{
				Debug.LogWarning("[Skateboard] Camera setup SKIPPED - missing references");
				return;
			}
			this.tpCamera = this.currentRiderInput.tpCamera;
			if (SmartphoneController.IsAnyTripodActive)
			{
				this.vehicleCameraPending = true;
				this.LogDebug("Tripod owns the camera; skateboard camera takeover deferred.");
				return;
			}
			this.ClaimVehicleCamera();
		}

		// Token: 0x06002DDC RID: 11740 RVA: 0x00100CE8 File Offset: 0x000FEEE8
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

		// Token: 0x06002DDD RID: 11741 RVA: 0x00100DAD File Offset: 0x000FEFAD
		private void ClaimVehicleCamera()
		{
			this.tpCamera.SetMainTarget(base.transform);
			if (!string.IsNullOrEmpty(this.vehicleCameraState))
			{
				this.currentRiderInput.ChangeCameraState(this.vehicleCameraState, true);
			}
			this.vehicleCameraWasClaimed = true;
			this.vehicleCameraPending = false;
		}

		// Token: 0x06002DDE RID: 11742 RVA: 0x00100DF0 File Offset: 0x000FEFF0
		private bool Dismount(bool force = false)
		{
			if (this.rideTransitionInProgress || !this.isBeingWorn || this.currentRider == null)
			{
				return false;
			}
			if (!force && Mathf.Abs(this.currentSpeed) >= 2f)
			{
				return false;
			}
			this.rideTransitionInProgress = true;
			vThirdPersonController vThirdPersonController = this.currentRider;
			this.LogDebug("=== DISMOUNTING SKATEBOARD ===");
			this.FreezeVehicle(true);
			this.RestorePlayerObjects();
			this.RestoreBlendShapes();
			AnimatorNormalUpdateModeLease animatorNormalUpdateModeLease = this.animatorUpdateModeLease;
			if (animatorNormalUpdateModeLease != null)
			{
				animatorNormalUpdateModeLease.Dispose();
			}
			this.animatorUpdateModeLease = null;
			if (this.riderAnimator != null)
			{
				this.riderAnimator.SetBool(SkateboardInteractable.IsRidingHash, false);
				this.riderAnimator.SetInteger(SkateboardInteractable.SkateStateHash, 0);
				this.riderAnimator.SetFloat(SkateboardInteractable.VehicleTypeHash, 0f);
				this.riderAnimator.speed = this.originalAnimatorSpeed;
				this.riderAnimator.applyRootMotion = this.originalApplyRootMotion;
			}
			Quaternion quaternion = VehicleDismountSafety.UprightRotation(base.transform, this.riderTransform);
			Vector3 vector = VehicleDismountSafety.FindSafeExitPosition(base.transform, this.riderTransform, this.riderCapsule, 1.15f);
			VehicleDismountSafety.RestoreRiderPhysics(this.riderTransform, this.originalRiderParent, vector, quaternion, this.originalRiderLocalScale, this.riderCapsule, this.originalCapsuleEnabled, this.riderRigidbody, this.originalRiderIsKinematic, default(Scene));
			if (this.vehicleCameraWasClaimed && this.useInvectorCamera && this.tpCamera != null && this.currentRiderInput != null)
			{
				this.tpCamera.SetMainTarget(this.riderTransform);
				this.currentRiderInput.ResetCameraState();
			}
			this.vehicleCameraWasClaimed = false;
			this.vehicleCameraPending = false;
			this.HideOwnedHud();
			if (CoverPoseController.Instance != null)
			{
				CoverPoseController.Instance.OnVehicleDismounted();
			}
			this.currentRider.lockMovement = false;
			this.currentRider.lockRotation = false;
			this.currentRider.lockAnimMovement = false;
			this.currentRider.lockAnimRotation = false;
			this.currentRider = null;
			this.currentRiderInput = null;
			this.tpCamera = null;
			this.riderTransform = null;
			this.riderAnimator = null;
			this.riderRigidbody = null;
			this.riderCapsule = null;
			this.originalRiderParent = null;
			this.currentSpeed = 0f;
			this.isGrounded = false;
			this.isBeingWorn = false;
			this.isSprinting = false;
			this.vehicleSprintInput.Reset();
			this.trickActive = false;
			this.pendingTrickBoost = false;
			this.fallActive = false;
			this.fallTimer = 0f;
			this.RestoreFallFace();
			this.moveDirection = Vector3.zero;
			this.inputMagnitude = 0f;
			this.inputBrake = false;
			VehicleRideActivityContext.Release(ref this._rideLease);
			UnityEvent onDismount = this.OnDismount;
			if (onDismount != null)
			{
				onDismount.Invoke();
			}
			Action<SkateboardInteractable, vThirdPersonController> onAnyDismounted = SkateboardInteractable.OnAnyDismounted;
			if (onAnyDismounted != null)
			{
				onAnyDismounted(this, vThirdPersonController);
			}
			this.rideTransitionInProgress = false;
			this.LogDebug("=== DISMOUNT COMPLETE ===");
			return true;
		}

		// Token: 0x06002DDF RID: 11743 RVA: 0x001010D2 File Offset: 0x000FF2D2
		public void ForceCrashDismount()
		{
			this.Dismount(true);
		}

		// Token: 0x06002DE0 RID: 11744 RVA: 0x001010DC File Offset: 0x000FF2DC
		private void HandleMovementSimple()
		{
			if (this.vehicleRigidbody == null || !this.isGrounded)
			{
				return;
			}
			float fixedDeltaTime = Time.fixedDeltaTime;
			this.planarVelocity = this.vehicleRigidbody.velocity;
			this.planarVelocity.y = 0f;
			float num = this.planarVelocity.magnitude;
			bool flag = this.inputMagnitude > 0.01f && !this.inputBrake;
			if (!this.inBalance && num >= this.balanceEnterSpeed)
			{
				this.inBalance = true;
			}
			else if (this.inBalance && num < this.balanceExitSpeed)
			{
				this.inBalance = false;
			}
			if (flag)
			{
				if (num <= this.stopSpeedThreshold)
				{
					base.transform.rotation = Quaternion.LookRotation(this.moveDirection, Vector3.up);
				}
				else
				{
					float num2 = Mathf.Max(0.01f, this.sprintSpeed * this.gameplaySpeedScale);
					float num3 = Mathf.Lerp(this.turnRateLowSpeed, this.turnRateTopSpeed, Mathf.Clamp01(num / num2));
					float num4 = Mathf.Clamp(Vector3.SignedAngle(base.transform.forward, this.moveDirection, Vector3.up), -num3 * fixedDeltaTime, num3 * fixedDeltaTime);
					base.transform.rotation = Quaternion.AngleAxis(num4, Vector3.up) * base.transform.rotation;
				}
			}
			Vector3 forward = base.transform.forward;
			forward.y = 0f;
			forward.Normalize();
			float num5 = (flag ? ((this.isSprinting ? this.sprintSpeed : this.walkSpeed) * this.inputMagnitude * this.gameplaySpeedScale) : 0f);
			if (this.pendingTrickBoost)
			{
				num = Mathf.Max(num, this.trickBoostSpeed * this.gameplaySpeedScale);
				this.pendingTrickBoost = false;
			}
			if (this.fallActive)
			{
				this.fallTimer -= fixedDeltaTime;
				if (this.fallTimer <= 0f)
				{
					this.fallActive = false;
					this.RestoreFallFace();
				}
			}
			else if (this.enableRandomFalls && this.inBalance && !this.trickActive)
			{
				this.fallIntervalTimer += fixedDeltaTime;
				if (this.fallIntervalTimer >= this.nextFallDelay)
				{
					this.TriggerNearFall();
					this.ScheduleNextRandomFall();
				}
			}
			if (this.trickActive)
			{
				num = Mathf.Max(0f, num - this.trickGlideDeceleration * fixedDeltaTime);
			}
			else if (flag && !this.inBalance && num < num5)
			{
				num = Mathf.MoveTowards(num, num5, this.acceleration * fixedDeltaTime);
			}
			else if (flag && this.inBalance)
			{
				num = Mathf.Max(0f, num - this.balanceGlideDeceleration * fixedDeltaTime);
			}
			else
			{
				float num6 = this.rollingFriction * num + this.coastDeceleration;
				if (this.inputBrake)
				{
					num6 += this.brakePower;
				}
				else if (!flag && this.inputMagnitude > 0.01f)
				{
					num6 += this.reverseBrakeDeceleration;
				}
				num = Mathf.Max(0f, num - num6 * fixedDeltaTime);
			}
			Vector3 vector = forward * num;
			this.vehicleRigidbody.velocity = new Vector3(vector.x, this.vehicleRigidbody.velocity.y, vector.z);
			if (this.riderAnimator != null)
			{
				this.riderAnimator.SetBool(SkateboardInteractable.IsRidingHash, true);
				this.riderAnimator.SetFloat(SkateboardInteractable.VehicleTypeHash, this.vehicleTypeValue);
				int num7;
				if (this.trickActive)
				{
					num7 = 3;
				}
				else if (flag)
				{
					num7 = (this.inBalance ? 2 : 1);
				}
				else if (num <= this.stopSpeedThreshold)
				{
					num7 = 0;
				}
				else
				{
					num7 = 2;
				}
				this.riderAnimator.SetInteger(SkateboardInteractable.SkateStateHash, num7);
				this.riderAnimator.SetFloat(SkateboardInteractable.SkatePushSpeedHash, this.isSprinting ? this.sprintPushClipSpeed : this.walkPushClipSpeed);
				float num8 = 0f;
				if (flag && !this.trickActive && !this.fallActive)
				{
					num8 = Mathf.Clamp(Vector3.SignedAngle(base.transform.forward, this.moveDirection, Vector3.up) / this.maxLeanAngle, -1f, 1f);
				}
				this.smoothedTurn = Mathf.SmoothDamp(this.smoothedTurn, num8, ref this.smoothedTurnVelocity, this.leanDampTime, float.PositiveInfinity, fixedDeltaTime);
				this.riderAnimator.SetFloat(SkateboardInteractable.SkateTurnHash, this.smoothedTurn);
			}
		}

		// Token: 0x06002DE1 RID: 11745 RVA: 0x00101534 File Offset: 0x000FF734
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

		// Token: 0x06002DE2 RID: 11746 RVA: 0x001015EA File Offset: 0x000FF7EA
		private void UpdateSpeed()
		{
			if (this.vehicleRigidbody == null)
			{
				return;
			}
			this.currentSpeed = Vector3.Dot(this.vehicleRigidbody.velocity, base.transform.forward);
		}

		// Token: 0x06002DE3 RID: 11747 RVA: 0x0010161C File Offset: 0x000FF81C
		private void CheckGround()
		{
			Vector3 vector = base.transform.position + Vector3.up * 0.1f;
			this.isGrounded = Physics.Raycast(vector, Vector3.down, this.groundCheckDistance + 0.1f, this.groundLayer, QueryTriggerInteraction.Ignore);
		}

		// Token: 0x06002DE4 RID: 11748 RVA: 0x00101674 File Offset: 0x000FF874
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
					}
				}
			}
		}

		// Token: 0x06002DE5 RID: 11749 RVA: 0x00101730 File Offset: 0x000FF930
		private void RestorePlayerObjects()
		{
			foreach (GameObject gameObject in this.hiddenObjects)
			{
				if (gameObject != null)
				{
					gameObject.SetActive(true);
				}
			}
			this.hiddenObjects.Clear();
		}

		// Token: 0x06002DE6 RID: 11750 RVA: 0x00101798 File Offset: 0x000FF998
		private void ApplyBlendShapeOverrides()
		{
			if (this.blendShapeOverrides.Count == 0 || this.riderTransform == null)
			{
				return;
			}
			this.originalBlendShapeValues.Clear();
			this.playerSkinnedMeshRenderers.Clear();
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in this.riderTransform.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				if (skinnedMeshRenderer.sharedMesh != null && skinnedMeshRenderer.sharedMesh.blendShapeCount > 0)
				{
					this.playerSkinnedMeshRenderers.Add(skinnedMeshRenderer);
				}
			}
			foreach (SkateBlendShapeOverride skateBlendShapeOverride in this.blendShapeOverrides)
			{
				if (!string.IsNullOrEmpty(skateBlendShapeOverride.blendShapeName))
				{
					foreach (SkinnedMeshRenderer skinnedMeshRenderer2 in this.playerSkinnedMeshRenderers)
					{
						if (!(skinnedMeshRenderer2 == null) && !(skinnedMeshRenderer2.sharedMesh == null))
						{
							int blendShapeIndex = skinnedMeshRenderer2.sharedMesh.GetBlendShapeIndex(skateBlendShapeOverride.blendShapeName);
							if (blendShapeIndex >= 0)
							{
								this.originalBlendShapeValues[new ValueTuple<int, int>(skinnedMeshRenderer2.GetInstanceID(), blendShapeIndex)] = skinnedMeshRenderer2.GetBlendShapeWeight(blendShapeIndex);
								skinnedMeshRenderer2.SetBlendShapeWeight(blendShapeIndex, skateBlendShapeOverride.targetValue);
								break;
							}
						}
					}
				}
			}
		}

		// Token: 0x06002DE7 RID: 11751 RVA: 0x0010191C File Offset: 0x000FFB1C
		private void ReapplyBlendShapeOverrides()
		{
			if (this.blendShapeOverrides.Count == 0)
			{
				return;
			}
			if (BlendShapeManager.Instance != null)
			{
				foreach (SkateBlendShapeOverride skateBlendShapeOverride in this.blendShapeOverrides)
				{
					if (!string.IsNullOrEmpty(skateBlendShapeOverride.blendShapeName))
					{
						BlendShapeManager.Instance.SetOverride(skateBlendShapeOverride.blendShapeName, skateBlendShapeOverride.targetValue, "Skateboard", BlendShapePriority.Vehicle, false);
					}
				}
			}
		}

		// Token: 0x06002DE8 RID: 11752 RVA: 0x001019B0 File Offset: 0x000FFBB0
		private void RestoreBlendShapes()
		{
			if (BlendShapeManager.Instance != null)
			{
				BlendShapeManager.Instance.RemoveAllOverridesFromSource("Skateboard");
			}
			else if (this.originalBlendShapeValues.Count > 0)
			{
				foreach (KeyValuePair<ValueTuple<int, int>, float> keyValuePair in this.originalBlendShapeValues)
				{
					ValueTuple<int, int> key = keyValuePair.Key;
					int item = key.Item1;
					int item2 = key.Item2;
					foreach (SkinnedMeshRenderer skinnedMeshRenderer in this.playerSkinnedMeshRenderers)
					{
						if (skinnedMeshRenderer != null && skinnedMeshRenderer.GetInstanceID() == item)
						{
							skinnedMeshRenderer.SetBlendShapeWeight(item2, keyValuePair.Value);
							break;
						}
					}
				}
			}
			this.originalBlendShapeValues.Clear();
			this.playerSkinnedMeshRenderers.Clear();
		}

		// Token: 0x06002DE9 RID: 11753 RVA: 0x00101ABC File Offset: 0x000FFCBC
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

		// Token: 0x06002DEA RID: 11754 RVA: 0x00101B38 File Offset: 0x000FFD38
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(base.transform.position, this.interactionDistance);
			Gizmos.color = (this.isGrounded ? Color.green : Color.red);
			Vector3 vector = base.transform.position + Vector3.up * 0.1f;
			Gizmos.DrawLine(vector, vector + Vector3.down * (this.groundCheckDistance + 0.1f));
		}

		// Token: 0x06002DEB RID: 11755 RVA: 0x00101BBE File Offset: 0x000FFDBE
		private void LogDebug(string message)
		{
			if (this.enableDebugLogs)
			{
				Debug.Log("[Skateboard] " + message);
			}
		}

		// Token: 0x04002DB3 RID: 11699
		[Header("Interaction")]
		[SerializeField]
		private InteractionConfigSO interactionConfig;

		// Token: 0x04002DB4 RID: 11700
		[Tooltip("Maximum distance for interaction. Aligned with the clothes pickup radius (1.1) so every interactable engages at the same distance.")]
		[SerializeField]
		private float interactionDistance = 1.1f;

		// Token: 0x04002DB5 RID: 11701
		[Header("Input Settings")]
		[Tooltip("Input to mount/dismount")]
		public GenericInput interactInput = new GenericInput("E", "A", "A");

		// Token: 0x04002DB6 RID: 11702
		[Tooltip("Input for acceleration/deceleration")]
		public GenericInput verticalInput = new GenericInput("Vertical", "LeftAnalogVertical", "Vertical");

		// Token: 0x04002DB7 RID: 11703
		[Tooltip("Input for steering")]
		public GenericInput horizontalInput = new GenericInput("Horizontal", "LeftAnalogHorizontal", "Horizontal");

		// Token: 0x04002DB8 RID: 11704
		[Tooltip("Input for braking")]
		public GenericInput brakeInput = new GenericInput("Space", "X", "X");

		// Token: 0x04002DB9 RID: 11705
		[Tooltip("Sprint: toggle with LS on controller; hold Shift on keyboard")]
		public GenericInput sprintInput = new GenericInput("LeftShift", "LeftStickClick", "LeftStickClick");

		// Token: 0x04002DBA RID: 11706
		[Tooltip("Input to start/stop the skate trick (BO emote): press once to launch, again to exit")]
		public GenericInput trickInput = new GenericInput("F", "B", "B");

		// Token: 0x04002DBB RID: 11707
		[Header("Vehicle Rigidbody")]
		[Tooltip("Rigidbody on this prefab for movement (required)")]
		[SerializeField]
		private Rigidbody vehicleRigidbody;

		// Token: 0x04002DBC RID: 11708
		[Header("Player References (Optional - auto-detected if empty)")]
		[Tooltip("Assign the GameObject with Animator (e.g., Ciel_Invector)")]
		[SerializeField]
		private Animator playerAnimator;

		// Token: 0x04002DBD RID: 11709
		[Header("Movement")]
		[Tooltip("Global multiplier over every push speed cap (quick way to scale the whole vehicle speed).")]
		[Range(0.2f, 2f)]
		[SerializeField]
		private float gameplaySpeedScale = 1f;

		// Token: 0x04002DBE RID: 11710
		[Header("Skate Feel - Smooth Movement")]
		[Tooltip("Cruise speed (m/s) while holding W. Kept BELOW balanceEnterSpeed so plain W stays in the push animation.")]
		[SerializeField]
		private float walkSpeed = 4f;

		// Token: 0x04002DBF RID: 11711
		[Tooltip("Cruise speed (m/s) while holding Shift. Set ABOVE balanceEnterSpeed so sprinting reaches stable balance mode.")]
		[SerializeField]
		private float sprintSpeed = 10f;

		// Token: 0x04002DC0 RID: 11712
		[Tooltip("How fast the board accelerates toward its cruise speed (m/s^2). Tuned so the climb from balanceExitSpeed to balanceEnterSpeed takes ~2 push cycles (push clip 1.43s at 1.5x sprint speed).")]
		[SerializeField]
		private float acceleration = 1.6f;

		// Token: 0x04002DC1 RID: 11713
		[Header("Skate Feel - Balance / Coast")]
		[Tooltip("Speed (m/s) above which the rider enters stable balance (no push anim). This is also the effective top speed: pushing stops here. Reachable only via sprint.")]
		[SerializeField]
		private float balanceEnterSpeed = 7.5f;

		// Token: 0x04002DC2 RID: 11714
		[Tooltip("Speed (m/s) below which balance ends and pushing can resume. Lower than enter = hysteresis; the gap sets how long the glide lasts.")]
		[SerializeField]
		private float balanceExitSpeed = 4.5f;

		// Token: 0x04002DC3 RID: 11715
		[Tooltip("Gentle speed decay (m/s^2) while gliding in stable balance: the board slowly bleeds speed until pushing resumes below balanceExitSpeed. Lower = longer glides in stable idle.")]
		[SerializeField]
		private float balanceGlideDeceleration = 0.35f;

		// Token: 0x04002DC4 RID: 11716
		[Tooltip("Rolling friction coefficient (scaled by speed). LOW = speed persists a long time when coasting.")]
		[SerializeField]
		private float rollingFriction = 0.06f;

		// Token: 0x04002DC5 RID: 11717
		[Tooltip("Constant deceleration floor (m/s^2) so the board eventually comes to a full stop.")]
		[SerializeField]
		private float coastDeceleration = 0.15f;

		// Token: 0x04002DC6 RID: 11718
		[Tooltip("Extra decel when pulling back on the stick (light manual brake, separate from the hard brake button).")]
		[SerializeField]
		private float reverseBrakeDeceleration = 6f;

		// Token: 0x04002DC7 RID: 11719
		[Header("Trick (BO emote)")]
		[Tooltip("Speed the trick launch boosts the board to (m/s, scaled by gameplaySpeedScale). The 'big sprint' of the trick.")]
		[SerializeField]
		private float trickBoostSpeed = 12f;

		// Token: 0x04002DC8 RID: 11720
		[Tooltip("Gentle decay (m/s^2) while holding the trick pose, like the balance glide.")]
		[SerializeField]
		private float trickGlideDeceleration = 0.35f;

		// Token: 0x04002DC9 RID: 11721
		[Tooltip("Optional HUD config shown while the trick is active (e.g. 'exit trick / F'). The main config is restored on exit.")]
		[SerializeField]
		private InteractionConfigSO trickActiveHudConfig;

		// Token: 0x04002DCA RID: 11722
		[Tooltip("Minimum gameplay seconds between two trick starts. Exiting an active trick is always immediate.")]
		[Min(0f)]
		[SerializeField]
		private float trickCooldownSeconds = 20f;

		// Token: 0x04002DCB RID: 11723
		[Header("Skate Feel - Steering")]
		[Tooltip("Turn rate (deg/s) at low speed: tight turns while setting off.")]
		[SerializeField]
		private float turnRateLowSpeed = 160f;

		// Token: 0x04002DCC RID: 11724
		[Tooltip("Turn rate (deg/s) at sprint cruise speed: wider, more stable arcs at speed, like a real board.")]
		[SerializeField]
		private float turnRateTopSpeed = 70f;

		// Token: 0x04002DCD RID: 11725
		[Header("Near Fall (random, balance only)")]
		[Tooltip("How long (s) the near-fall keeps the stumble state. Match the clip length (2s for the _Short clip).")]
		[SerializeField]
		private float fallDuration = 2f;

		// Token: 0x04002DCE RID: 11726
		[Tooltip("Random near-falls while cruising in balance (never during push, idle or trick). Purely visual: no speed loss.")]
		[SerializeField]
		private bool enableRandomFalls = true;

		// Token: 0x04002DCF RID: 11727
		[Tooltip("Minimum seconds of BALANCE riding accumulated between random falls.")]
		[SerializeField]
		private float randomFallMinInterval = 50f;

		// Token: 0x04002DD0 RID: 11728
		[Tooltip("Extra random delay (0..this) added on top of the minimum, so the timing is unpredictable.")]
		[SerializeField]
		private float randomFallExtraRandom = 15f;

		// Token: 0x04002DD1 RID: 11729
		[Tooltip("FaceExpressionIndex shown ONLY while the stumble plays (3 = HornySupreme). Previous face is restored right after.")]
		[SerializeField]
		private int fallFaceExpressionIndex = 3;

		// Token: 0x04002DD2 RID: 11730
		[Header("Lean (turning)")]
		[Tooltip("Steering angle (degrees between board heading and input direction) at which the lean pose reaches full strength.")]
		[SerializeField]
		private float maxLeanAngle = 45f;

		// Token: 0x04002DD3 RID: 11731
		[Tooltip("Smoothing time (s) for the lean blend: higher = softer, lazier lean in/out.")]
		[SerializeField]
		private float leanDampTime = 0.15f;

		// Token: 0x04002DD4 RID: 11732
		[Header("Brake")]
		[SerializeField]
		private float brakePower = 20f;

		// Token: 0x04002DD5 RID: 11733
		[Header("Camera Settings")]
		[Tooltip("Camera state name when riding")]
		[SerializeField]
		private string vehicleCameraState = "Bike";

		// Token: 0x04002DD6 RID: 11734
		[SerializeField]
		private bool useInvectorCamera = true;

		// Token: 0x04002DD7 RID: 11735
		[Header("Cover Pose Compatibility")]
		[Tooltip("If true, the player can use cover pose while riding this vehicle.")]
		[SerializeField]
		private bool allowCoverWhileRiding = true;

		// Token: 0x04002DD8 RID: 11736
		[Tooltip("Which arm(s) CoverPose can control while riding")]
		[SerializeField]
		private VehicleCoverArmMode coverArmMode;

		// Token: 0x04002DD9 RID: 11737
		[Header("Animator Settings")]
		[Tooltip("VehicleType parameter value (4 = Skateboard) that selects the skate branch")]
		[SerializeField]
		private float vehicleTypeValue = 4f;

		// Token: 0x04002DDA RID: 11738
		[Tooltip("Multiplier applied to the rider animator speed while riding (1 = normal). Restored on dismount.")]
		[Range(0.1f, 2f)]
		[SerializeField]
		private float animationSpeedMultiplier = 1f;

		// Token: 0x04002DDB RID: 11739
		[Tooltip("Speed (m/s) below which the board is considered stopped and the rider goes to IDLE.")]
		[SerializeField]
		private float stopSpeedThreshold = 0.15f;

		// Token: 0x04002DDC RID: 11740
		[Header("Push Clip Speed (state machine SkatePushSpeed)")]
		[Tooltip("Push animation playback speed while pushing with W (gentle).")]
		[SerializeField]
		private float walkPushClipSpeed = 1f;

		// Token: 0x04002DDD RID: 11741
		[Tooltip("Push animation playback speed while sprinting with Shift (faster pumping).")]
		[SerializeField]
		private float sprintPushClipSpeed = 1.5f;

		// Token: 0x04002DDE RID: 11742
		[Header("UI")]
		[SerializeField]
		private InteractablePromptUI promptUI;

		// Token: 0x04002DDF RID: 11743
		[Header("Stability Settings")]
		[Tooltip("Ground check distance")]
		[SerializeField]
		private float groundCheckDistance = 0.3f;

		// Token: 0x04002DE0 RID: 11744
		[Tooltip("Layer mask for ground detection")]
		[SerializeField]
		private LayerMask groundLayer = -1;

		// Token: 0x04002DE1 RID: 11745
		[Tooltip("Constrain rotation to prevent tipping")]
		[SerializeField]
		private bool constrainRotation = true;

		// Token: 0x04002DE2 RID: 11746
		[Tooltip("PhysicMaterial for the vehicle collider (use zero friction to prevent wall sticking)")]
		[SerializeField]
		private PhysicMaterial vehiclePhysicMaterial;

		// Token: 0x04002DE3 RID: 11747
		[Tooltip("Auto-create zero friction material if none assigned")]
		[SerializeField]
		private bool autoCreateZeroFrictionMaterial = true;

		// Token: 0x04002DE4 RID: 11748
		[Header("Player Visual Overrides")]
		[Tooltip("Nomi dei GameObject da nascondere sul player mentre usa lo skateboard")]
		[SerializeField]
		private List<string> objectNamesToHide = new List<string>();

		// Token: 0x04002DE5 RID: 11749
		[Tooltip("Blend shapes da modificare mentre si usa lo skateboard")]
		[SerializeField]
		private List<SkateBlendShapeOverride> blendShapeOverrides = new List<SkateBlendShapeOverride>();

		// Token: 0x04002DE6 RID: 11750
		[Header("Debug")]
		[SerializeField]
		private bool enableDebugLogs = true;

		// Token: 0x04002DE7 RID: 11751
		[Header("Events")]
		public UnityEvent OnMount;

		// Token: 0x04002DE8 RID: 11752
		public UnityEvent OnDismount;

		// Token: 0x04002DE9 RID: 11753
		public UnityEvent<float> OnSpeedChanged;

		// Token: 0x04002DEC RID: 11756
		private bool isBeingWorn;

		// Token: 0x04002DED RID: 11757
		private bool rideTransitionInProgress;

		// Token: 0x04002DEE RID: 11758
		private Transform riderTransform;

		// Token: 0x04002DEF RID: 11759
		private Animator riderAnimator;

		// Token: 0x04002DF0 RID: 11760
		private Rigidbody riderRigidbody;

		// Token: 0x04002DF1 RID: 11761
		private CapsuleCollider riderCapsule;

		// Token: 0x04002DF2 RID: 11762
		private Transform originalRiderParent;

		// Token: 0x04002DF3 RID: 11763
		private Vector3 originalRiderLocalScale;

		// Token: 0x04002DF4 RID: 11764
		private AnimatorNormalUpdateModeLease animatorUpdateModeLease;

		// Token: 0x04002DF5 RID: 11765
		private float originalAnimatorSpeed = 1f;

		// Token: 0x04002DF6 RID: 11766
		private vThirdPersonController currentRider;

		// Token: 0x04002DF7 RID: 11767
		private vThirdPersonInput currentRiderInput;

		// Token: 0x04002DF8 RID: 11768
		private vThirdPersonCamera tpCamera;

		// Token: 0x04002DF9 RID: 11769
		private bool vehicleCameraWasClaimed;

		// Token: 0x04002DFA RID: 11770
		private bool vehicleCameraPending;

		// Token: 0x04002DFB RID: 11771
		private bool isPlayerInRange;

		// Token: 0x04002DFC RID: 11772
		private Transform playerTransform;

		// Token: 0x04002DFD RID: 11773
		private readonly VehiclePlayerLookup _playerLookup = new VehiclePlayerLookup();

		// Token: 0x04002DFE RID: 11774
		private bool originalRiderIsKinematic;

		// Token: 0x04002DFF RID: 11775
		private bool originalCapsuleEnabled;

		// Token: 0x04002E00 RID: 11776
		private bool originalApplyRootMotion;

		// Token: 0x04002E01 RID: 11777
		private static readonly int IsRidingHash = Animator.StringToHash("IsRiding");

		// Token: 0x04002E02 RID: 11778
		private static readonly int VehicleTypeHash = Animator.StringToHash("VehicleType");

		// Token: 0x04002E03 RID: 11779
		private static readonly int SkateStateHash = Animator.StringToHash("SkateState");

		// Token: 0x04002E04 RID: 11780
		private static readonly int SkatePushSpeedHash = Animator.StringToHash("SkatePushSpeed");

		// Token: 0x04002E05 RID: 11781
		private static readonly int SkateTurnHash = Animator.StringToHash("SkateTurn");

		// Token: 0x04002E06 RID: 11782
		private static readonly int SkateFallHash = Animator.StringToHash("SkateFall");

		// Token: 0x04002E07 RID: 11783
		private const int SKATE_IDLE = 0;

		// Token: 0x04002E08 RID: 11784
		private const int SKATE_PUSH = 1;

		// Token: 0x04002E09 RID: 11785
		private const int SKATE_BALANCE = 2;

		// Token: 0x04002E0A RID: 11786
		private const int SKATE_TRICK = 3;

		// Token: 0x04002E0B RID: 11787
		private float currentSpeed;

		// Token: 0x04002E0C RID: 11788
		private float lastSpeed;

		// Token: 0x04002E0D RID: 11789
		private bool isSprinting;

		// Token: 0x04002E0E RID: 11790
		private readonly VehicleSprintInput vehicleSprintInput = new VehicleSprintInput();

		// Token: 0x04002E0F RID: 11791
		private bool inBalance;

		// Token: 0x04002E10 RID: 11792
		private bool trickActive;

		// Token: 0x04002E11 RID: 11793
		private bool pendingTrickBoost;

		// Token: 0x04002E12 RID: 11794
		private float trickCooldownRemaining;

		// Token: 0x04002E13 RID: 11795
		private bool fallActive;

		// Token: 0x04002E14 RID: 11796
		private float fallTimer;

		// Token: 0x04002E15 RID: 11797
		private float fallIntervalTimer;

		// Token: 0x04002E16 RID: 11798
		private float nextFallDelay;

		// Token: 0x04002E17 RID: 11799
		private bool holdingFallFace;

		// Token: 0x04002E18 RID: 11800
		private float smoothedTurn;

		// Token: 0x04002E19 RID: 11801
		private float smoothedTurnVelocity;

		// Token: 0x04002E1A RID: 11802
		private Vector3 planarVelocity;

		// Token: 0x04002E1B RID: 11803
		private bool isGrounded;

		// Token: 0x04002E1C RID: 11804
		private Camera mainCamera;

		// Token: 0x04002E1D RID: 11805
		private float mountCooldown;

		// Token: 0x04002E1E RID: 11806
		private const float MOUNT_COOLDOWN_TIME = 0.5f;

		// Token: 0x04002E1F RID: 11807
		private Vector3 moveDirection;

		// Token: 0x04002E20 RID: 11808
		private float inputMagnitude;

		// Token: 0x04002E21 RID: 11809
		private bool inputBrake;

		// Token: 0x04002E22 RID: 11810
		private List<GameObject> hiddenObjects = new List<GameObject>();

		// Token: 0x04002E23 RID: 11811
		private List<SkinnedMeshRenderer> playerSkinnedMeshRenderers = new List<SkinnedMeshRenderer>();

		// Token: 0x04002E24 RID: 11812
		private Dictionary<ValueTuple<int, int>, float> originalBlendShapeValues = new Dictionary<ValueTuple<int, int>, float>();

		// Token: 0x04002E25 RID: 11813
		private float _sqrDistToPlayer = float.MaxValue;

		// Token: 0x04002E26 RID: 11814
		private PlayerActivityLease _rideLease;
	}
}
