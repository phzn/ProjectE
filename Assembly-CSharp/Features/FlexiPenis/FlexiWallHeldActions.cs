using System;
using System.Collections;
using Cinemachine;
using Features.Detection;
using Features.GameOver;
using Features.Inventory;
using Features.Inventory.Placement;
using Features.MasturbationSystem;
using Features.PhoneCompanion;
using Features.PlayerEmotes;
using Features.Rank;
using Features.Vehicles.Traffic;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace Features.FlexiPenis
{
	// Token: 0x020011FC RID: 4604
	public class FlexiWallHeldActions : MonoBehaviour
	{
		// Token: 0x170016B3 RID: 5811
		// (get) Token: 0x06008DF8 RID: 36344 RVA: 0x003A0440 File Offset: 0x0039E640
		private PlayerContextResource RequestedResources
		{
			get
			{
				if (!SmartphoneController.IsStableTripodCameraOwner)
				{
					return PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation | PlayerContextResource.HeldItem;
				}
				return PlayerContextResource.PlayerControl | PlayerContextResource.FullBodyAnimation | PlayerContextResource.HeldItem;
			}
		}

		// Token: 0x170016B4 RID: 5812
		// (get) Token: 0x06008DF9 RID: 36345 RVA: 0x003A044E File Offset: 0x0039E64E
		private bool UsesTripodCamera
		{
			get
			{
				return this._activityLease != null && (this._activityLease.Resources & PlayerContextResource.Camera) == PlayerContextResource.None;
			}
		}

		// Token: 0x06008DFA RID: 36346 RVA: 0x003A046A File Offset: 0x0039E66A
		private void Awake()
		{
			DetectionFlyingCamera.NormalizePhotoInput(this.flyModeInput);
			ControllerContextBindings.Set(this.flyModeInput, "Y");
			ControllerContextBindings.Set(this.stopInput, "B");
		}

		// Token: 0x06008DFB RID: 36347 RVA: 0x003A0498 File Offset: 0x0039E698
		private void Start()
		{
			PlayerInventoryManager instance = PlayerInventoryManager.Instance;
			if (this.actionAnimator == null && instance != null)
			{
				this.actionAnimator = instance.GetComponentInChildren<PlayerActionAnimator>();
			}
			if (this.actionAnimator == null)
			{
				this.actionAnimator = Object.FindObjectOfType<PlayerActionAnimator>();
			}
			if (this.playerInput == null && this.actionAnimator != null)
			{
				this.playerInput = this.actionAnimator.GetComponentInParent<vThirdPersonInput>();
			}
			if (this.playerInput == null)
			{
				this.playerInput = Object.FindObjectOfType<vThirdPersonInput>();
			}
			if (this.playerRoot == null && this.playerInput != null)
			{
				this.playerRoot = this.playerInput.transform;
			}
			if (this.surfaceDetector == null)
			{
				this.surfaceDetector = Object.FindObjectOfType<PlacementSurfaceDetector>();
			}
			if (this.masturbationSpeed == null)
			{
				this.masturbationSpeed = Object.FindObjectOfType<MasturbationSpeedController>();
			}
			if (this.emoteController == null)
			{
				this.emoteController = Object.FindObjectOfType<GameplayEmoteController>();
			}
			if (this.propHandler == null)
			{
				this.propHandler = Object.FindObjectOfType<EmotePropHandler>();
			}
			if (this.thrustSound == null)
			{
				Animator animator = this.ResolveAnimator();
				if (animator != null)
				{
					this.thrustSound = animator.GetComponent<FlexiThrustSound>();
					if (this.thrustSound == null)
					{
						this.thrustSound = animator.gameObject.AddComponent<FlexiThrustSound>();
					}
				}
			}
		}

		// Token: 0x06008DFC RID: 36348 RVA: 0x003A0608 File Offset: 0x0039E808
		private bool IsFlexiActive()
		{
			PlayerInventoryManager instance = PlayerInventoryManager.Instance;
			if (instance == null)
			{
				return false;
			}
			InventoryItemSO activeItem = instance.GetActiveItem();
			return !(activeItem == null) && this.flexiItem != null && activeItem == this.flexiItem;
		}

		// Token: 0x06008DFD RID: 36349 RVA: 0x003A0654 File Offset: 0x0039E854
		private void Update()
		{
			ItemUsageSignal.Set(this, this._phase > FlexiWallHeldActions.Phase.Idle);
			if (!this.IsFlexiActive())
			{
				if (this._phase != FlexiWallHeldActions.Phase.Idle)
				{
					this.AbortToIdle();
				}
				return;
			}
			if (this._phase != FlexiWallHeldActions.Phase.Idle && (this._activityLease == null || !this._activityLease.IsActive || (GameOverManager.Instance != null && GameOverManager.Instance.IsGameOver)))
			{
				this.AbortToIdle();
				return;
			}
			if (TextInputFocus.BlocksGameplayInput || Time.timeScale <= 0f)
			{
				return;
			}
			if (this._phase == FlexiWallHeldActions.Phase.Idle)
			{
				if (this.useInput.GetButtonDown())
				{
					this.TryStartAction();
					return;
				}
			}
			else if (this._phase == FlexiWallHeldActions.Phase.Positioning)
			{
				if (this.stopInput.GetButtonDown())
				{
					this.AbortToIdle();
					return;
				}
			}
			else if (this._phase == FlexiWallHeldActions.Phase.Playing)
			{
				if (!this.HandleCameraInput() && this.stopInput.GetButtonDown())
				{
					this.StopAction();
					return;
				}
				this.TickSpeedAndOrgasm();
				return;
			}
			else if (this._phase == FlexiWallHeldActions.Phase.Stopping && !this.HandleCameraInput() && this.stopInput.GetButtonDown())
			{
				this.AbortToIdle();
			}
		}

		// Token: 0x06008DFE RID: 36350 RVA: 0x003A0764 File Offset: 0x0039E964
		private void TryStartAction()
		{
			FlexiWallHeldActions.<>c__DisplayClass112_0 CS$<>8__locals1 = new FlexiWallHeldActions.<>c__DisplayClass112_0();
			CS$<>8__locals1.<>4__this = this;
			if (this._phase != FlexiWallHeldActions.Phase.Idle || DildoPlacementSystem.AnyPlacing || !PlayerActivityGate.Allows(PlayerActivity.Interact) || PlayerActivityGate.IsResourceOwned(this.RequestedResources) || TextInputFocus.BlocksGameplayInput || Time.timeScale <= 0f)
			{
				return;
			}
			if (!UncoveredBottomRequirement.TryAllow())
			{
				return;
			}
			if (this.playerRoot == null || this.playerInput == null || this.actionAnimator == null || this.actionAnimator.GenericAction == null || this.actionEntry == null || this.actionEntry.TriggerPrefab == null)
			{
				Debug.LogWarning("[FlexiWallHeldActions] Cannot start: missing actionAnimator or actionEntry/TriggerPrefab.");
				return;
			}
			Vector3 vector;
			if (!this.TryDetectWall(out vector, out CS$<>8__locals1.wallNormal, out CS$<>8__locals1.floorPos))
			{
				BlockedActionFeedback.Show("Move close to a wall, face it, then press F to use.");
				if (this.debugLogs)
				{
					Debug.Log("[FlexiWallHeldActions] No valid wall in view — F ignored.");
				}
				return;
			}
			if (!this.TryAcquireActionContext())
			{
				return;
			}
			this._phase = FlexiWallHeldActions.Phase.Positioning;
			FlexiWallHeldActions.<>c__DisplayClass112_0 CS$<>8__locals2 = CS$<>8__locals1;
			int num = this._sessionVersion + 1;
			this._sessionVersion = num;
			CS$<>8__locals2.session = num;
			ItemUsageSignal.Set(this, true);
			if (!this.UsesTripodCamera)
			{
				this._actionUi.Begin(Array.Empty<Transform>());
			}
			if (ScreenFadeController.Instance != null)
			{
				this._entryFade = ScreenFadeController.Instance;
				this._entryFade.FadeOutIn(delegate
				{
					CS$<>8__locals1.<>4__this.CompletePositioning(CS$<>8__locals1.session, CS$<>8__locals1.floorPos, CS$<>8__locals1.wallNormal);
				}, this.fadeOutDuration, this.fadeInDuration, this.fadeHoldDuration, delegate
				{
					if (CS$<>8__locals1.session == CS$<>8__locals1.<>4__this._sessionVersion)
					{
						CS$<>8__locals1.<>4__this._entryFade = null;
					}
				});
				return;
			}
			this.CompletePositioning(CS$<>8__locals1.session, CS$<>8__locals1.floorPos, CS$<>8__locals1.wallNormal);
		}

		// Token: 0x06008DFF RID: 36351 RVA: 0x003A0904 File Offset: 0x0039EB04
		private void CompletePositioning(int session, Vector3 floorPos, Vector3 wallNormal)
		{
			if (session != this._sessionVersion || this._phase != FlexiWallHeldActions.Phase.Positioning || !base.isActiveAndEnabled || this._activityLease == null || !this._activityLease.IsActive || !this.IsFlexiActive())
			{
				return;
			}
			if (!UncoveredBottomRequirement.TryAllow())
			{
				this.AbortToIdle();
				return;
			}
			this.SnapToWall(floorPos, wallNormal);
			this._phase = FlexiWallHeldActions.Phase.Playing;
			this.BeginAction();
		}

		// Token: 0x06008E00 RID: 36352 RVA: 0x003A096C File Offset: 0x0039EB6C
		private bool TryAcquireActionContext()
		{
			if (CarRoofCarrier.IsPlayerMounted)
			{
				return false;
			}
			if (this._activityLease != null && this._activityLease.IsActive)
			{
				return false;
			}
			if (!PlayerActivityGate.TryAcquire("FlexiWall", this, PlayerActivity.Interact, this.RequestedResources, PlayerActivity.AllPlayerActions, out this._activityLease, false, null))
			{
				return false;
			}
			if (this.playerInput == null)
			{
				return true;
			}
			this._savedInputLock = this.playerInput.lockInput;
			this._savedCameraInputLock = this.playerInput.lockCameraInput;
			this._savedForceCameraLock = vThirdPersonInput.ForceCameraLock;
			this._savedSuppressZoom = this.playerInput.suppressCameraZoom;
			this.playerInput.SetLockAllInput(true);
			if (!this.UsesTripodCamera)
			{
				this.playerInput.lockCameraInput = true;
				vThirdPersonInput.ForceCameraLock = true;
			}
			this._heldController = this.playerInput.cc;
			if (this._heldController != null)
			{
				this._savedMovementLock = this._heldController.lockMovement;
				this._savedRotationLock = this._heldController.lockRotation;
				this._savedControllerEnabled = this._heldController.enabled;
				this._heldController.lockMovement = true;
				this._heldController.lockRotation = true;
				this._heldController.input = Vector3.zero;
				this._heldController.inputSmooth = Vector3.zero;
				this._heldController.moveDirection = Vector3.zero;
				if (this._heldController.animator != null && this._heldController.GetComponent<Rigidbody>() != null)
				{
					this._heldController.StopCharacter();
				}
			}
			return true;
		}

		// Token: 0x06008E01 RID: 36353 RVA: 0x003A0B00 File Offset: 0x0039ED00
		private void ReleaseActionContext()
		{
			if (this._activityLease == null)
			{
				return;
			}
			if (this._heldController != null)
			{
				this._heldController.lockMovement = this._savedMovementLock;
				this._heldController.lockRotation = this._savedRotationLock;
				this._heldController.enabled = this._savedControllerEnabled;
				this._heldController = null;
			}
			if (this.playerInput != null)
			{
				this.playerInput.SetLockAllInput(this._savedInputLock);
				if (!this.UsesTripodCamera)
				{
					this.playerInput.lockCameraInput = this._savedCameraInputLock;
					vThirdPersonInput.ForceCameraLock = this._savedForceCameraLock;
				}
				this.playerInput.suppressCameraZoom = this._savedSuppressZoom;
			}
			this._activityLease.Dispose();
			this._activityLease = null;
		}

		// Token: 0x06008E02 RID: 36354 RVA: 0x003A0BC4 File Offset: 0x0039EDC4
		private bool TryDetectWall(out Vector3 wallPos, out Vector3 wallNormal, out Vector3 floorPos)
		{
			wallPos = Vector3.zero;
			wallNormal = Vector3.forward;
			floorPos = Vector3.zero;
			if (this.surfaceDetector == null || Camera.main == null)
			{
				return false;
			}
			Vector3? vector = ((SmartphoneController.IsStableTripodCameraOwner && this.playerRoot != null) ? new Vector3?(this.playerRoot.forward) : null);
			PlacementSurfaceDetector placementSurfaceDetector = this.surfaceDetector;
			Camera main = Camera.main;
			LayerMask layerMask = this.wallMask;
			float num = this.maxWallDistance;
			Transform transform = this.playerRoot;
			Vector3? vector2 = vector;
			PlacementResult placementResult = placementSurfaceDetector.DetectSurface(main, layerMask, num, transform, null, vector2);
			if (!placementResult.IsValid || placementResult.SurfaceType != PlacementSurfaceType.Wall || placementResult.SurfaceTransform != null)
			{
				return false;
			}
			wallPos = placementResult.Position;
			wallNormal = placementResult.HitNormal;
			floorPos = placementResult.FloorPosition;
			return true;
		}

		// Token: 0x06008E03 RID: 36355 RVA: 0x003A0CB8 File Offset: 0x0039EEB8
		private void SnapToWall(Vector3 floorPos, Vector3 wallNormal)
		{
			if (this.playerRoot == null)
			{
				return;
			}
			this._savedPlayerPos = this.playerRoot.position;
			this._savedPlayerRot = this.playerRoot.rotation;
			this._savedPlayerTransform = true;
			vThirdPersonController vThirdPersonController = ((this.playerInput != null) ? this.playerInput.cc : null);
			if (this.disableControllerDuringAlign && vThirdPersonController != null)
			{
				vThirdPersonController.enabled = false;
			}
			Rigidbody component = this.playerRoot.GetComponent<Rigidbody>();
			if (component != null)
			{
				this._hasRigidbody = true;
				this._originalIsKinematic = component.isKinematic;
				component.velocity = Vector3.zero;
				component.angularVelocity = Vector3.zero;
				component.isKinematic = true;
			}
			Vector3 vector = Vector3.ProjectOnPlane(-wallNormal, Vector3.up).normalized;
			if (vector == Vector3.zero)
			{
				vector = this.playerRoot.forward;
			}
			Quaternion quaternion = Quaternion.LookRotation(vector, Vector3.up);
			Vector3 normalized = Vector3.ProjectOnPlane(wallNormal, Vector3.up).normalized;
			Vector3 vector2 = floorPos + normalized * this.distanceFromWall;
			this.playerRoot.rotation = quaternion * Quaternion.Euler(this.wallRotationOffset);
			this.playerRoot.position = vector2 + this.playerRoot.TransformVector(this.wallPositionOffset);
			if (this.disableControllerDuringAlign && vThirdPersonController != null)
			{
				vThirdPersonController.enabled = this._savedControllerEnabled;
			}
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[FlexiWallHeldActions] Snapped to wall at {0}, facing {1}.", vector2, vector));
			}
		}

		// Token: 0x06008E04 RID: 36356 RVA: 0x003A0E64 File Offset: 0x0039F064
		private void BeginAction()
		{
			if (this._activityLease == null || !this._activityLease.IsActive)
			{
				return;
			}
			this._actionStarted = true;
			this.actionAnimator.TriggerAction(this.actionEntry);
			this.SuppressHoldingLayer();
			this.ApplyActionHandOffset();
			Animator animator = this.ResolvePropAnimator();
			if (animator != null)
			{
				animator.Play(this.startState, 0, 0f);
			}
			this._targetSpeed = Mathf.Clamp(this.defaultSpeed, this.minSpeed, this.maxSpeed);
			this._currentSpeed = this._targetSpeed;
			this._maxReached = false;
			this._cumPropFired = false;
			this.HookMeter();
			this.ApplySpeedToAnimator(this._currentSpeed);
			if (this.masturbationSpeed != null)
			{
				this._savedExternalDriver = this.masturbationSpeed.ExternalDriverActive;
				this.masturbationSpeed.ExternalDriverActive = true;
				this._speedDriveStarted = true;
				this.masturbationSpeed.ShowExternalSpeed(this._currentSpeed);
			}
			if (this.propHandler != null && this.proxyEmote != null)
			{
				this.propHandler.SetupProps(this.proxyEmote);
				this._propsStarted = true;
			}
			if (this.emoteController != null && this.proxyEmote != null)
			{
				this.emoteController.NotifyExternalMasturbationStart(this.proxyEmote);
				this._masturbationStarted = true;
			}
			this.ActivateActionCamera();
			if (this.thrustSound != null)
			{
				this.thrustSound.Active = true;
			}
			if (this.orgasmMeter != null && this.orgasmMeter.IsLocked)
			{
				this.HandleOrgasmMaxReached();
			}
			if (this.debugLogs)
			{
				Debug.Log("[FlexiWallHeldActions] Action started (Start -> Loop).");
			}
		}

		// Token: 0x06008E05 RID: 36357 RVA: 0x003A1018 File Offset: 0x0039F218
		private void ActivateActionCamera()
		{
			if (this.UsesTripodCamera)
			{
				return;
			}
			if (this.detectionCameraCtrl == null || this.cameraPresets == null)
			{
				return;
			}
			if (this.detectionCameraCtrl.IsActive || (this.flyingCamera != null && this.flyingCamera.IsActive))
			{
				return;
			}
			this._ownsActionCamera = true;
			this._actionOutputCamera = Camera.main;
			if (this._actionOutputCamera != null && !this._hasSavedCameraState)
			{
				this._savedCameraLocalPosition = this._actionOutputCamera.transform.localPosition;
				this._savedCameraLocalRotation = this._actionOutputCamera.transform.localRotation;
				this._hasSavedCameraState = true;
			}
			this._actionBrain = ((this._actionOutputCamera != null) ? this._actionOutputCamera.GetComponent<CinemachineBrain>() : null);
			this._savedBrainEnabled = this._actionBrain != null && this._actionBrain.enabled;
			if (this._actionBrain != null)
			{
				this._actionBrain.enabled = true;
			}
			this.detectionCameraCtrl.ActivateForEmotePhase(this.cameraPresets, this.playerRoot, this.playerRoot);
		}

		// Token: 0x06008E06 RID: 36358 RVA: 0x003A114C File Offset: 0x0039F34C
		private bool HandleCameraInput()
		{
			return !this.UsesTripodCamera && (this._ownsActionCamera && this.flyingCamera != null) && this.flyingCamera.HandleOwnerInput(this.flyModeInput, ref this._ownsFlyCamera, this.detectionCameraCtrl, (this.playerRoot != null) ? this.playerRoot.position : base.transform.position, this._actionOutputCamera);
		}

		// Token: 0x06008E07 RID: 36359 RVA: 0x003A11C4 File Offset: 0x0039F3C4
		private void StopActionCamera()
		{
			if (this._ownsFlyCamera && this.flyingCamera != null && this.flyingCamera.IsActive)
			{
				this.flyingCamera.Deactivate();
			}
			this._ownsFlyCamera = false;
			if (this._ownsActionCamera && this.detectionCameraCtrl != null && this.detectionCameraCtrl.IsActive)
			{
				this.detectionCameraCtrl.Deactivate();
			}
			if (this._ownsActionCamera && this._actionBrain != null)
			{
				this._actionBrain.enabled = this._savedBrainEnabled;
			}
			if (this._hasSavedCameraState && this._actionOutputCamera != null)
			{
				this._actionOutputCamera.transform.localPosition = this._savedCameraLocalPosition;
				this._actionOutputCamera.transform.localRotation = this._savedCameraLocalRotation;
			}
			this._hasSavedCameraState = false;
			this._ownsActionCamera = false;
			this._actionBrain = null;
			this._actionOutputCamera = null;
			this._actionUi.Restore();
		}

		// Token: 0x06008E08 RID: 36360 RVA: 0x003A12C4 File Offset: 0x0039F4C4
		private void TickSpeedAndOrgasm()
		{
			this.HookMeter();
			bool flag = InventoryControllerInput.SpeedModifierHeld(this.speedModifierInput);
			if (this.playerInput != null)
			{
				this.playerInput.suppressCameraZoom = flag;
			}
			int num = InventoryControllerInput.SpeedStep(flag, ref this._controllerSpeedDirection, ref this._controllerSpeedRepeat);
			if (num != 0)
			{
				float targetSpeed = this._targetSpeed;
				this._targetSpeed = Mathf.Clamp(this._targetSpeed + (float)num * this.speedStepPerNotch, this.minSpeed, this.maxSpeed);
				if (!Mathf.Approximately(targetSpeed, this._targetSpeed) && this.masturbationSpeed != null)
				{
					this.masturbationSpeed.ShowExternalSpeed(this._targetSpeed);
				}
			}
			if (this.speedSmoothing >= 1f)
			{
				this._currentSpeed = this._targetSpeed;
			}
			else
			{
				float num2 = 1f - Mathf.Pow(1f - this.speedSmoothing, Time.deltaTime * 60f);
				this._currentSpeed = Mathf.Lerp(this._currentSpeed, this._targetSpeed, num2);
			}
			this.ApplySpeedToAnimator(this._currentSpeed);
			if (this.orgasmMeter != null && !this._maxReached)
			{
				float num3 = ((this.proxyEmote != null) ? this.proxyEmote.OrgasmProgressPerSecondAtBaseSpeed : 0.05f) * this._currentSpeed * Time.deltaTime;
				if (num3 > 0f)
				{
					this.orgasmMeter.AddProgress(num3);
				}
			}
		}

		// Token: 0x06008E09 RID: 36361 RVA: 0x003A1428 File Offset: 0x0039F628
		private void ApplySpeedToAnimator(float s)
		{
			Animator animator = this.ResolveAnimator();
			if (animator == null)
			{
				return;
			}
			float num2;
			float num3;
			if (s <= this.slowToMediumSpeed)
			{
				float num = Mathf.InverseLerp(this.minSpeed, this.slowToMediumSpeed, s);
				num2 = Mathf.Lerp(0f, 1f, num);
				num3 = Mathf.Lerp(this.clipMultMin, this.clipMultMax, num);
			}
			else if (s <= this.mediumToFastSpeed)
			{
				float num4 = Mathf.InverseLerp(this.slowToMediumSpeed, this.mediumToFastSpeed, s);
				num2 = 1f;
				num3 = Mathf.Lerp(this.clipMultMin, this.clipMultMax, num4);
			}
			else
			{
				float num5 = Mathf.InverseLerp(this.mediumToFastSpeed, this.maxSpeed, s);
				num2 = Mathf.Lerp(1f, 2f, num5);
				num3 = Mathf.Lerp(this.clipMultMin, this.clipMultMax, num5);
			}
			if (!string.IsNullOrEmpty(this.blendParam))
			{
				animator.SetFloat(this.blendParam, num2);
			}
			if (!string.IsNullOrEmpty(this.speedMultParam))
			{
				animator.SetFloat(this.speedMultParam, num3);
			}
			Animator animator2 = this.ResolvePropAnimator();
			if (animator2 != null)
			{
				if (!string.IsNullOrEmpty(this.blendParam))
				{
					animator2.SetFloat(this.blendParam, num2);
				}
				if (!string.IsNullOrEmpty(this.speedMultParam))
				{
					animator2.SetFloat(this.speedMultParam, num3);
				}
			}
		}

		// Token: 0x06008E0A RID: 36362 RVA: 0x003A1578 File Offset: 0x0039F778
		private void HookMeter()
		{
			if (this.orgasmMeter == null)
			{
				this.orgasmMeter = OrgasmMeterController.Instance;
			}
			if (this.orgasmMeter != null && !this._meterHooked)
			{
				this.orgasmMeter.OnMaxReached += this.HandleOrgasmMaxReached;
				this._meterHooked = true;
				if (this._phase == FlexiWallHeldActions.Phase.Playing && this._masturbationStarted && this.orgasmMeter.IsLocked)
				{
					this.HandleOrgasmMaxReached();
				}
			}
		}

		// Token: 0x06008E0B RID: 36363 RVA: 0x003A15F6 File Offset: 0x0039F7F6
		private void UnhookMeter()
		{
			if (this.orgasmMeter != null && this._meterHooked)
			{
				this.orgasmMeter.OnMaxReached -= this.HandleOrgasmMaxReached;
			}
			this._meterHooked = false;
		}

		// Token: 0x06008E0C RID: 36364 RVA: 0x003A162C File Offset: 0x0039F82C
		private void HandleOrgasmMaxReached()
		{
			if (this._maxReached)
			{
				return;
			}
			this._maxReached = true;
			if (this._phase != FlexiWallHeldActions.Phase.Playing)
			{
				return;
			}
			this._phase = FlexiWallHeldActions.Phase.Stopping;
			if (this._stopRoutine != null)
			{
				base.StopCoroutine(this._stopRoutine);
			}
			this._stopRoutine = base.StartCoroutine(this.OrgasmRoutine());
		}

		// Token: 0x06008E0D RID: 36365 RVA: 0x003A1680 File Offset: 0x0039F880
		private Animator ResolveAnimator()
		{
			Animator animator = ((this.actionAnimator != null) ? this.actionAnimator.GetComponentInChildren<Animator>() : null);
			if (animator == null && PlayerInventoryManager.Instance != null)
			{
				animator = PlayerInventoryManager.Instance.PlayerAnimator;
			}
			return animator;
		}

		// Token: 0x06008E0E RID: 36366 RVA: 0x003A16CC File Offset: 0x0039F8CC
		private Animator ResolvePropAnimator()
		{
			if (this._actionStarted)
			{
				if (!(this._handProp != null))
				{
					return null;
				}
				return this._handProp.GetComponentInChildren<Animator>();
			}
			else
			{
				PlayerInventoryManager instance = PlayerInventoryManager.Instance;
				GameObject gameObject = ((instance != null) ? instance.GetSpawnedHandObject() : null);
				if (!(gameObject != null))
				{
					return null;
				}
				return gameObject.GetComponentInChildren<Animator>();
			}
		}

		// Token: 0x06008E0F RID: 36367 RVA: 0x003A1728 File Offset: 0x0039F928
		private void StopPropAnimator()
		{
			Animator animator = this.ResolvePropAnimator();
			if (animator != null && !string.IsNullOrEmpty(this.propIdleState))
			{
				animator.Play(this.propIdleState, 0, 0f);
			}
		}

		// Token: 0x06008E10 RID: 36368 RVA: 0x003A1764 File Offset: 0x0039F964
		private void SuppressHoldingLayer()
		{
			if (this._holdSuppressed)
			{
				return;
			}
			Animator animator = this.ResolveAnimator();
			if (animator == null)
			{
				return;
			}
			if (this._holdLayerIndex < 0)
			{
				this._holdLayerIndex = animator.GetLayerIndex(this.holdingLayerName);
			}
			if (this._holdLayerIndex < 0)
			{
				return;
			}
			this._savedHoldWeight = animator.GetLayerWeight(this._holdLayerIndex);
			animator.SetLayerWeight(this._holdLayerIndex, 0f);
			this._holdSuppressed = true;
		}

		// Token: 0x06008E11 RID: 36369 RVA: 0x003A17DC File Offset: 0x0039F9DC
		private void RestoreHoldingLayer()
		{
			if (!this._holdSuppressed)
			{
				return;
			}
			Animator animator = this.ResolveAnimator();
			if (animator != null && this._holdLayerIndex >= 0)
			{
				animator.SetLayerWeight(this._holdLayerIndex, this._savedHoldWeight);
			}
			this._holdSuppressed = false;
		}

		// Token: 0x06008E12 RID: 36370 RVA: 0x003A1824 File Offset: 0x0039FA24
		private void ApplyActionHandOffset()
		{
			if (this._propOffsetApplied)
			{
				return;
			}
			PlayerInventoryManager instance = PlayerInventoryManager.Instance;
			this._handProp = ((instance != null && instance.GetSpawnedHandObject() != null) ? instance.GetSpawnedHandObject().transform : null);
			if (this._handProp == null)
			{
				return;
			}
			this._savedPropLocalPos = this._handProp.localPosition;
			this._savedPropLocalRot = this._handProp.localRotation;
			this._handProp.localPosition = this.actionHandPositionOffset;
			this._handProp.localRotation = Quaternion.Euler(this.actionHandRotationOffset);
			this._propOffsetApplied = true;
		}

		// Token: 0x06008E13 RID: 36371 RVA: 0x003A18CC File Offset: 0x0039FACC
		private void RestoreHandOffset()
		{
			if (!this._propOffsetApplied)
			{
				return;
			}
			Transform handProp = this._handProp;
			if (handProp != null)
			{
				handProp.localPosition = this._savedPropLocalPos;
				handProp.localRotation = this._savedPropLocalRot;
			}
			this._handProp = null;
			this._propOffsetApplied = false;
		}

		// Token: 0x06008E14 RID: 36372 RVA: 0x003A1918 File Offset: 0x0039FB18
		private void StopAction()
		{
			if (this._phase != FlexiWallHeldActions.Phase.Playing)
			{
				return;
			}
			this._phase = FlexiWallHeldActions.Phase.Stopping;
			if (this._stopRoutine != null)
			{
				base.StopCoroutine(this._stopRoutine);
			}
			this._stopRoutine = base.StartCoroutine(this.StopRoutine());
		}

		// Token: 0x06008E15 RID: 36373 RVA: 0x003A1951 File Offset: 0x0039FB51
		private IEnumerator StopRoutine()
		{
			yield return this.PlayEndAndFinish();
			this._stopRoutine = null;
			yield break;
		}

		// Token: 0x06008E16 RID: 36374 RVA: 0x003A1960 File Offset: 0x0039FB60
		private IEnumerator OrgasmRoutine()
		{
			Animator animator = this.ResolveAnimator();
			if (animator != null && animator.HasState(this.actionLayer, Animator.StringToHash(this.cumState)))
			{
				animator.CrossFadeInFixedTime(this.cumState, this.cumCrossfade, this.actionLayer);
				Animator animator2 = this.ResolvePropAnimator();
				if (animator2 != null)
				{
					animator2.CrossFadeInFixedTime(this.cumState, this.cumCrossfade, 0);
				}
				if (this.playerInput != null)
				{
					this.playerInput.suppressCameraZoom = false;
				}
				if (this.masturbationSpeed != null)
				{
					this.masturbationSpeed.HideExternalSpeed();
				}
				float fxDelay = Mathf.Clamp(this.cumEffectDelay, 0f, this.cumDuration);
				if (fxDelay > 0f)
				{
					yield return new WaitForSeconds(fxDelay);
				}
				this.FireCumProp();
				float num = this.cumDuration - fxDelay;
				if (num > 0f)
				{
					yield return new WaitForSeconds(num);
				}
			}
			yield return this.PlayEndAndFinish();
			this._stopRoutine = null;
			yield break;
		}

		// Token: 0x06008E17 RID: 36375 RVA: 0x003A196F File Offset: 0x0039FB6F
		private void FireCumProp()
		{
			if (this._cumPropFired)
			{
				return;
			}
			this._cumPropFired = true;
			if (this.propHandler != null)
			{
				this.propHandler.ActivateProp(this.cumPropLabel);
			}
		}

		// Token: 0x06008E18 RID: 36376 RVA: 0x003A19A0 File Offset: 0x0039FBA0
		private IEnumerator PlayEndAndFinish()
		{
			Animator anim = this.ResolveAnimator();
			if (anim != null && anim.HasState(this.actionLayer, Animator.StringToHash(this.endState)))
			{
				anim.CrossFadeInFixedTime(this.endState, this.endCrossfade, this.actionLayer);
				Animator animator = this.ResolvePropAnimator();
				if (animator != null)
				{
					animator.CrossFadeInFixedTime(this.endState, this.endCrossfade, 0);
				}
				float guard = 0f;
				while (guard < 1f && !this.IsInState(anim, this.endState))
				{
					guard += Time.deltaTime;
					yield return null;
				}
				float endDeadline = Time.unscaledTime + 15f;
				while (Time.unscaledTime < endDeadline && this.IsInState(anim, this.endState) && anim.GetCurrentAnimatorStateInfo(this.actionLayer).normalizedTime < 0.98f)
				{
					yield return null;
				}
			}
			if (this._entryFade != null)
			{
				this._entryFade.SetClear();
			}
			this._entryFade = null;
			this.StopActionCamera();
			this.FinishInPlace();
			this._phase = FlexiWallHeldActions.Phase.Idle;
			this._sessionVersion++;
			ItemUsageSignal.Set(this, false);
			yield break;
		}

		// Token: 0x06008E19 RID: 36377 RVA: 0x003A19B0 File Offset: 0x0039FBB0
		private void FinishInPlace()
		{
			if (this.thrustSound != null)
			{
				this.thrustSound.Active = false;
			}
			this.StopPropAnimator();
			this.RestoreHoldingLayer();
			this.RestoreHandOffset();
			this.EndMasturbationDrive();
			this.CleanupSpeedAndMeter();
			this.ReleasePhysicsHoldInPlace();
			if (this.actionAnimator != null)
			{
				this.actionAnimator.FinishCurrentAction();
				this.actionAnimator.RestoreOverrideLayer();
			}
			this._actionStarted = false;
			this.ReleaseActionContext();
		}

		// Token: 0x06008E1A RID: 36378 RVA: 0x003A1A2C File Offset: 0x0039FC2C
		private void ReleasePhysicsHoldInPlace()
		{
			if (this._hasRigidbody && this.playerRoot != null)
			{
				Rigidbody component = this.playerRoot.GetComponent<Rigidbody>();
				if (component != null)
				{
					component.isKinematic = this._originalIsKinematic;
				}
			}
			this._hasRigidbody = false;
			this._savedPlayerTransform = false;
		}

		// Token: 0x06008E1B RID: 36379 RVA: 0x003A1A80 File Offset: 0x0039FC80
		private void RestorePlayerTransform()
		{
			if (!this._savedPlayerTransform || this.playerRoot == null)
			{
				this._savedPlayerTransform = false;
				return;
			}
			if (this._hasRigidbody)
			{
				Rigidbody component = this.playerRoot.GetComponent<Rigidbody>();
				if (component != null)
				{
					component.isKinematic = this._originalIsKinematic;
				}
				this._hasRigidbody = false;
			}
			this.playerRoot.position = this._savedPlayerPos;
			this.playerRoot.rotation = this._savedPlayerRot;
			this._savedPlayerTransform = false;
		}

		// Token: 0x06008E1C RID: 36380 RVA: 0x003A1B04 File Offset: 0x0039FD04
		private void AbortToIdle()
		{
			this._sessionVersion++;
			base.StopAllCoroutines();
			this._stopRoutine = null;
			ItemUsageSignal.Set(this, false);
			if (this._entryFade != null)
			{
				this._entryFade.SetClear();
			}
			this._entryFade = null;
			if (this._phase == FlexiWallHeldActions.Phase.Idle && this._activityLease == null)
			{
				return;
			}
			if (this.thrustSound != null)
			{
				this.thrustSound.Active = false;
			}
			if (this._actionStarted)
			{
				this.StopPropAnimator();
			}
			this.StopActionCamera();
			this.RestoreHoldingLayer();
			this.RestoreHandOffset();
			this.EndMasturbationDrive();
			this.CleanupSpeedAndMeter();
			this.RestorePlayerTransform();
			if (this._actionStarted && this.actionAnimator != null)
			{
				this.actionAnimator.CancelCurrentAction();
				this.actionAnimator.RestoreOverrideLayer();
			}
			this._actionStarted = false;
			this.ReleaseActionContext();
			this._phase = FlexiWallHeldActions.Phase.Idle;
		}

		// Token: 0x06008E1D RID: 36381 RVA: 0x003A1BF0 File Offset: 0x0039FDF0
		public void InterruptForEncounter()
		{
			this.AbortToIdle();
		}

		// Token: 0x06008E1E RID: 36382 RVA: 0x003A1BF8 File Offset: 0x0039FDF8
		private void EndMasturbationDrive()
		{
			if (this._masturbationStarted && this.emoteController != null && this.proxyEmote != null)
			{
				this.emoteController.NotifyExternalMasturbationEnd(this.proxyEmote);
			}
			this._masturbationStarted = false;
			if (this._propsStarted && this.propHandler != null)
			{
				this.propHandler.CleanupAllProps();
			}
			this._propsStarted = false;
			if (this._speedDriveStarted && this.masturbationSpeed != null)
			{
				this.masturbationSpeed.ExternalDriverActive = this._savedExternalDriver;
			}
			this._speedDriveStarted = false;
		}

		// Token: 0x06008E1F RID: 36383 RVA: 0x003A1C98 File Offset: 0x0039FE98
		private void CleanupSpeedAndMeter()
		{
			if (this.playerInput != null)
			{
				this.playerInput.suppressCameraZoom = false;
			}
			if (this._actionStarted && this.masturbationSpeed != null)
			{
				this.masturbationSpeed.HideExternalSpeed();
			}
			Animator animator = this.ResolveAnimator();
			if (this._actionStarted && animator != null)
			{
				if (!string.IsNullOrEmpty(this.blendParam))
				{
					animator.SetFloat(this.blendParam, 1f);
				}
				if (!string.IsNullOrEmpty(this.speedMultParam))
				{
					animator.SetFloat(this.speedMultParam, 1f);
				}
			}
			if (this._meterHooked && this.orgasmMeter != null)
			{
				if (this._maxReached)
				{
					this.orgasmMeter.UnlockAndCooldown();
				}
				else
				{
					this.orgasmMeter.ForceCooldown();
				}
			}
			this.UnhookMeter();
			this._targetSpeed = this.defaultSpeed;
			this._currentSpeed = this.defaultSpeed;
		}

		// Token: 0x06008E20 RID: 36384 RVA: 0x003A1D88 File Offset: 0x0039FF88
		private void OnDisable()
		{
			this.AbortToIdle();
			if (this._shown != null && InteractionHUDController.Instance != null)
			{
				InteractionHUDController.Instance.HideIfCurrent(this._shown);
			}
			this._shown = null;
		}

		// Token: 0x06008E21 RID: 36385 RVA: 0x003A1DC3 File Offset: 0x0039FFC3
		private void OnDestroy()
		{
			if (this._tripodLoopConfig == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				Object.Destroy(this._tripodLoopConfig);
				return;
			}
			Object.DestroyImmediate(this._tripodLoopConfig);
		}

		// Token: 0x06008E22 RID: 36386 RVA: 0x003A1DF4 File Offset: 0x0039FFF4
		private InteractionConfigSO ResolveLoopConfig()
		{
			if (!this.UsesTripodCamera || this.loopConfig == null)
			{
				return this.loopConfig;
			}
			if (this._tripodLoopConfig == null)
			{
				this._tripodLoopConfig = ScriptableObject.CreateInstance<InteractionConfigSO>();
				this._tripodLoopConfig.name = "Flexi tripod actions";
				this._tripodLoopConfig.hideFlags = HideFlags.HideAndDontSave;
			}
			this._tripodLoopConfig.usableActions.Clear();
			foreach (UsableAction usableAction in this.loopConfig.usableActions)
			{
				if (usableAction != null && !(usableAction.keyDisplayLabel == "K") && !(usableAction.keyDisplayLabel == "C"))
				{
					this._tripodLoopConfig.usableActions.Add(usableAction);
				}
			}
			ControllerContextBindings.SetInputLabel(this._tripodLoopConfig, "X", this.stopInput);
			ControllerContextBindings.SetLabel(this._tripodLoopConfig, "CTRL + MW", "LT + RS", false);
			return this._tripodLoopConfig;
		}

		// Token: 0x06008E23 RID: 36387 RVA: 0x003A1F14 File Offset: 0x003A0114
		private bool IsInState(Animator anim, string stateName)
		{
			return !anim.IsInTransition(this.actionLayer) && anim.GetCurrentAnimatorStateInfo(this.actionLayer).IsName(stateName);
		}

		// Token: 0x06008E24 RID: 36388 RVA: 0x003A1F48 File Offset: 0x003A0148
		private void LateUpdate()
		{
			if (this._phase != FlexiWallHeldActions.Phase.Idle)
			{
				if (this._heldController != null)
				{
					this._heldController.lockMovement = true;
					this._heldController.lockRotation = true;
				}
				this._actionUi.Refresh();
			}
			ControllerContextBindings.SetInputLabel(this.loopConfig, "X", this.stopInput);
			ControllerContextBindings.SetInputLabel(this.loopConfig, "K", this.flyModeInput);
			if (this.detectionCameraCtrl != null)
			{
				ControllerContextBindings.SetInputLabel(this.loopConfig, "C", this.detectionCameraCtrl.CycleCameraInput);
			}
			ControllerContextBindings.SetLabel(this.loopConfig, "CTRL + MW", "LT + RS", false);
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (instance == null)
			{
				return;
			}
			InteractionConfigSO interactionConfigSO = null;
			if (this._phase != FlexiWallHeldActions.Phase.Idle)
			{
				interactionConfigSO = this.ResolveLoopConfig();
			}
			else if (this.IsFlexiActive() && !CarRoofCarrier.IsPlayerMounted && PlayerActivityGate.Allows(PlayerActivity.Interact) && !PlayerActivityGate.IsResourceOwned(this.RequestedResources) && !DildoPlacementSystem.AnyPlacing && !TextInputFocus.BlocksGameplayInput && Time.timeScale > 0f)
			{
				interactionConfigSO = this.idleConfig;
			}
			if (interactionConfigSO == this._shown)
			{
				if (interactionConfigSO != null && instance.CurrentConfig == null)
				{
					instance.Show(interactionConfigSO);
				}
				return;
			}
			if (interactionConfigSO == null)
			{
				if (this._shown != null)
				{
					instance.HideIfCurrent(this._shown);
				}
			}
			else
			{
				instance.Show(interactionConfigSO);
			}
			this._shown = interactionConfigSO;
		}

		// Token: 0x04009130 RID: 37168
		private const PlayerContextResource ActionResources = PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation | PlayerContextResource.HeldItem;

		// Token: 0x04009131 RID: 37169
		private InteractionConfigSO _tripodLoopConfig;

		// Token: 0x04009132 RID: 37170
		[Header("Item")]
		[Tooltip("The Flexi item: HUD/actions are active only while this is the active held item.")]
		[SerializeField]
		private InventoryItemSO flexiItem;

		// Token: 0x04009133 RID: 37171
		[Header("HUD Configs")]
		[Tooltip("Shown while the Flexi is in hand and idle (F : Use).")]
		[SerializeField]
		private InteractionConfigSO idleConfig;

		// Token: 0x04009134 RID: 37172
		[Tooltip("Shown while the looping action is playing (X : Stop).")]
		[SerializeField]
		private InteractionConfigSO loopConfig;

		// Token: 0x04009135 RID: 37173
		[Header("Action")]
		[Tooltip("PlayerActionEntry referencing the vTriggerGenericAction prefab whose playAnimation is 'Ciel_FlexiWall_Start' (the FlexiWall_LoopEmote prefab).")]
		[SerializeField]
		private PlayerActionEntry actionEntry;

		// Token: 0x04009136 RID: 37174
		[Header("Animator State Names (Base Layer / Actions/Ciel_FlexiWall)")]
		[SerializeField]
		private string endState = "Ciel_FlexiWall_End";

		// Token: 0x04009137 RID: 37175
		[Tooltip("Orgasm climax state played when the orgasm meter fills.")]
		[SerializeField]
		private string cumState = "Ciel_FlexiWall_Cum";

		// Token: 0x04009138 RID: 37176
		[Tooltip("Base Layer index the Flexi states live on (animatorLayer:0).")]
		[SerializeField]
		private int actionLayer;

		// Token: 0x04009139 RID: 37177
		[SerializeField]
		private float endCrossfade = 0.2f;

		// Token: 0x0400913A RID: 37178
		[SerializeField]
		private float cumCrossfade = 0.2f;

		// Token: 0x0400913B RID: 37179
		[Tooltip("How long the cum animation plays before transitioning to the End wind-down (seconds).")]
		[SerializeField]
		private float cumDuration = 12.25f;

		// Token: 0x0400913C RID: 37180
		[Header("Wall Positioning")]
		[Tooltip("Detects the wall via a camera raycast (reused from the placement system). Auto-found.")]
		[SerializeField]
		private PlacementSurfaceDetector surfaceDetector;

		// Token: 0x0400913D RID: 37181
		[Tooltip("Layers the wall raycast may hit. Should include the environment/wall layers.")]
		[SerializeField]
		private LayerMask wallMask = -1;

		// Token: 0x0400913E RID: 37182
		[Tooltip("Max distance of the wall detection ray.")]
		[SerializeField]
		private float maxWallDistance = 4f;

		// Token: 0x0400913F RID: 37183
		[Tooltip("The player root that gets teleported to the wall. Auto-found from the inventory manager.")]
		[SerializeField]
		private Transform playerRoot;

		// Token: 0x04009140 RID: 37184
		[Tooltip("Extra local position offset applied after snapping to the wall (tune so the baked pose lines up with the wall surface).")]
		[SerializeField]
		private Vector3 wallPositionOffset = Vector3.zero;

		// Token: 0x04009141 RID: 37185
		[Tooltip("Extra Euler rotation offset applied after facing the wall (tune the baked pose's facing).")]
		[SerializeField]
		private Vector3 wallRotationOffset = Vector3.zero;

		// Token: 0x04009142 RID: 37186
		[Tooltip("Distance kept from the wall surface (along the wall normal) when snapping Ciel's feet.")]
		[SerializeField]
		private float distanceFromWall = 0.35f;

		// Token: 0x04009143 RID: 37187
		[Tooltip("If true, temporarily disable the character controller while teleporting.")]
		[SerializeField]
		private bool disableControllerDuringAlign = true;

		// Token: 0x04009144 RID: 37188
		[Header("Fade")]
		[SerializeField]
		private float fadeOutDuration = 0.25f;

		// Token: 0x04009145 RID: 37189
		[SerializeField]
		private float fadeInDuration = 0.3f;

		// Token: 0x04009146 RID: 37190
		[SerializeField]
		private float fadeHoldDuration = 0.15f;

		// Token: 0x04009147 RID: 37191
		[Header("Camera (dildo-style, optional — leave null to keep the normal camera)")]
		[Tooltip("Detection camera controller for preset-based cameras during the action.")]
		[SerializeField]
		private DetectionCameraController detectionCameraCtrl;

		// Token: 0x04009148 RID: 37192
		[Tooltip("Preset collection used for the action cameras.")]
		[SerializeField]
		private DetectionCameraPresetCollectionSO cameraPresets;

		// Token: 0x04009149 RID: 37193
		[Tooltip("Free-fly camera toggled with K during the action.")]
		[SerializeField]
		private DetectionFlyingCamera flyingCamera;

		// Token: 0x0400914A RID: 37194
		[Tooltip("UI panel showing camera controls (F fly / C next preset).")]
		[SerializeField]
		private GameObject cameraControlsUI;

		// Token: 0x0400914B RID: 37195
		[SerializeField]
		private GenericInput flyModeInput = new GenericInput("K", "Y", "A");

		// Token: 0x0400914C RID: 37196
		[Header("Masturbation System Integration")]
		[Tooltip("Proxy EmoteDefinitionSO (IsMasturbationEmote=1) driving the SHARED controllers (face, voice/breathing, heartbeat) and carrying the squirt PropBinding. Its OrgasmProgressPerSecondAtBaseSpeed sets the bar speed.")]
		[SerializeField]
		private EmoteDefinitionSO proxyEmote;

		// Token: 0x0400914D RID: 37197
		[SerializeField]
		private GameplayEmoteController emoteController;

		// Token: 0x0400914E RID: 37198
		[SerializeField]
		private EmotePropHandler propHandler;

		// Token: 0x0400914F RID: 37199
		[SerializeField]
		private MasturbationSpeedController masturbationSpeed;

		// Token: 0x04009150 RID: 37200
		[Tooltip("Seconds into the Cum animation at which the squirt + cum vocal fire.")]
		[SerializeField]
		private float cumEffectDelay = 3.24f;

		// Token: 0x04009151 RID: 37201
		[Tooltip("PropBinding label (on the proxy emote) for the squirt/cum effect.")]
		[SerializeField]
		private string cumPropLabel = "Squirt";

		// Token: 0x04009152 RID: 37202
		[Header("Holding Layer")]
		[Tooltip("The HoldingItem layer is disabled while the action plays so the in-hand pose doesn't fight the full-body Flexi clips, then restored when it ends.")]
		[SerializeField]
		private string holdingLayerName = "HoldingItem";

		// Token: 0x04009153 RID: 37203
		[Header("Hand Prop Offset During Action")]
		[SerializeField]
		private Vector3 actionHandPositionOffset = Vector3.zero;

		// Token: 0x04009154 RID: 37204
		[SerializeField]
		private Vector3 actionHandRotationOffset = Vector3.zero;

		// Token: 0x04009155 RID: 37205
		[Header("Speed & Loop Blend (Ctrl + Scroll)")]
		[SerializeField]
		private string blendParam = "FlexiBlend";

		// Token: 0x04009156 RID: 37206
		[SerializeField]
		private string speedMultParam = "FlexiSpeedMult";

		// Token: 0x04009157 RID: 37207
		[SerializeField]
		private GenericInput speedModifierInput = new GenericInput("LeftControl", "LeftBumper", "LeftBumper");

		// Token: 0x04009158 RID: 37208
		[SerializeField]
		private float minSpeed = 0.5f;

		// Token: 0x04009159 RID: 37209
		[SerializeField]
		private float maxSpeed = 2.5f;

		// Token: 0x0400915A RID: 37210
		[SerializeField]
		private float defaultSpeed = 1f;

		// Token: 0x0400915B RID: 37211
		[Min(0.01f)]
		[SerializeField]
		private float speedStepPerNotch = 0.15f;

		// Token: 0x0400915C RID: 37212
		[Range(0f, 1f)]
		[SerializeField]
		private float speedSmoothing = 0.2f;

		// Token: 0x0400915D RID: 37213
		[Header("Speed -> Clip Mapping")]
		[SerializeField]
		private float slowToMediumSpeed = 1f;

		// Token: 0x0400915E RID: 37214
		[SerializeField]
		private float mediumToFastSpeed = 1.6f;

		// Token: 0x0400915F RID: 37215
		[SerializeField]
		private float clipMultMin = 0.85f;

		// Token: 0x04009160 RID: 37216
		[SerializeField]
		private float clipMultMax = 1.4f;

		// Token: 0x04009161 RID: 37217
		[Header("Orgasm Meter")]
		[SerializeField]
		private OrgasmMeterController orgasmMeter;

		// Token: 0x04009162 RID: 37218
		[Header("Input")]
		[SerializeField]
		private GenericInput useInput = new GenericInput("F", "X", "X");

		// Token: 0x04009163 RID: 37219
		[SerializeField]
		private GenericInput stopInput = new GenericInput("X", "B", "B");

		// Token: 0x04009164 RID: 37220
		[Header("Prop Animator (the penis wobble)")]
		[Tooltip("The Flexi clips animate the penis bones (Penis_Dildo_*) which live inside the spawned hand prop (it carries a duplicate of Ciel's skeleton). The prop has its OWN Animator (FlexiProp.controller) that we drive IN SYNC with Ciel so the penis deforms. Resolved at runtime from the spawned hand object; no need to assign.")]
		[SerializeField]
		private string startState = "Ciel_FlexiWall_Start";

		// Token: 0x04009165 RID: 37221
		[Tooltip("Empty rest state on the prop controller; the prop returns here after the action so it stops wobbling while merely held.")]
		[SerializeField]
		private string propIdleState = "Idle";

		// Token: 0x04009166 RID: 37222
		[Header("Thrust SFX (the 'ciaf' dildo sounds, played per thrust via animation events)")]
		[Tooltip("Receiver for the AE_FlexiThrust animation events on the Loop/Cum clips. Auto-found on the player animator's GameObject. Gated ON while the action plays, OFF otherwise.")]
		[SerializeField]
		private FlexiThrustSound thrustSound;

		// Token: 0x04009167 RID: 37223
		[Header("Refs (auto-found if left empty)")]
		[SerializeField]
		private PlayerActionAnimator actionAnimator;

		// Token: 0x04009168 RID: 37224
		[SerializeField]
		private vThirdPersonInput playerInput;

		// Token: 0x04009169 RID: 37225
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x0400916A RID: 37226
		private FlexiWallHeldActions.Phase _phase;

		// Token: 0x0400916B RID: 37227
		private InteractionConfigSO _shown;

		// Token: 0x0400916C RID: 37228
		private Coroutine _stopRoutine;

		// Token: 0x0400916D RID: 37229
		private PlayerActivityLease _activityLease;

		// Token: 0x0400916E RID: 37230
		private readonly ActionUiSuppressionScope _actionUi = new ActionUiSuppressionScope();

		// Token: 0x0400916F RID: 37231
		private int _sessionVersion;

		// Token: 0x04009170 RID: 37232
		private bool _actionStarted;

		// Token: 0x04009171 RID: 37233
		private bool _propsStarted;

		// Token: 0x04009172 RID: 37234
		private bool _speedDriveStarted;

		// Token: 0x04009173 RID: 37235
		private bool _savedExternalDriver;

		// Token: 0x04009174 RID: 37236
		private bool _savedInputLock;

		// Token: 0x04009175 RID: 37237
		private bool _savedCameraInputLock;

		// Token: 0x04009176 RID: 37238
		private bool _savedForceCameraLock;

		// Token: 0x04009177 RID: 37239
		private bool _savedSuppressZoom;

		// Token: 0x04009178 RID: 37240
		private bool _savedMovementLock;

		// Token: 0x04009179 RID: 37241
		private bool _savedRotationLock;

		// Token: 0x0400917A RID: 37242
		private bool _savedControllerEnabled;

		// Token: 0x0400917B RID: 37243
		private vThirdPersonController _heldController;

		// Token: 0x0400917C RID: 37244
		private ScreenFadeController _entryFade;

		// Token: 0x0400917D RID: 37245
		private Camera _actionOutputCamera;

		// Token: 0x0400917E RID: 37246
		private CinemachineBrain _actionBrain;

		// Token: 0x0400917F RID: 37247
		private bool _savedBrainEnabled;

		// Token: 0x04009180 RID: 37248
		private bool _ownsActionCamera;

		// Token: 0x04009181 RID: 37249
		private bool _ownsFlyCamera;

		// Token: 0x04009182 RID: 37250
		private Vector3 _savedPlayerPos;

		// Token: 0x04009183 RID: 37251
		private Quaternion _savedPlayerRot;

		// Token: 0x04009184 RID: 37252
		private bool _savedPlayerTransform;

		// Token: 0x04009185 RID: 37253
		private bool _originalIsKinematic;

		// Token: 0x04009186 RID: 37254
		private bool _hasRigidbody;

		// Token: 0x04009187 RID: 37255
		private Vector3 _savedCameraLocalPosition;

		// Token: 0x04009188 RID: 37256
		private Quaternion _savedCameraLocalRotation;

		// Token: 0x04009189 RID: 37257
		private bool _hasSavedCameraState;

		// Token: 0x0400918A RID: 37258
		private int _holdLayerIndex = -1;

		// Token: 0x0400918B RID: 37259
		private float _savedHoldWeight;

		// Token: 0x0400918C RID: 37260
		private bool _holdSuppressed;

		// Token: 0x0400918D RID: 37261
		private Transform _handProp;

		// Token: 0x0400918E RID: 37262
		private Vector3 _savedPropLocalPos;

		// Token: 0x0400918F RID: 37263
		private Quaternion _savedPropLocalRot;

		// Token: 0x04009190 RID: 37264
		private bool _propOffsetApplied;

		// Token: 0x04009191 RID: 37265
		private float _targetSpeed = 1f;

		// Token: 0x04009192 RID: 37266
		private float _currentSpeed = 1f;

		// Token: 0x04009193 RID: 37267
		private bool _maxReached;

		// Token: 0x04009194 RID: 37268
		private bool _meterHooked;

		// Token: 0x04009195 RID: 37269
		private bool _masturbationStarted;

		// Token: 0x04009196 RID: 37270
		private bool _cumPropFired;

		// Token: 0x04009197 RID: 37271
		private int _controllerSpeedDirection;

		// Token: 0x04009198 RID: 37272
		private float _controllerSpeedRepeat;

		// Token: 0x020011FD RID: 4605
		private enum Phase
		{
			// Token: 0x0400919A RID: 37274
			Idle,
			// Token: 0x0400919B RID: 37275
			Positioning,
			// Token: 0x0400919C RID: 37276
			Playing,
			// Token: 0x0400919D RID: 37277
			Stopping
		}
	}
}
