using System;
using System.Collections.Generic;
using Cinemachine;
using Features.DebugTools;
using Features.Detection;
using Features.HideFlash;
using Features.Inventory;
using Features.MainMenu;
using Features.PlayerEmotes;
using Features.PlayerEmotions;
using Features.RenderingPrecision;
using Features.Tutorials;
using Invector.vCamera;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using Invector.vCover;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.Rendering;
using ProjectExhibitionist.Core.SceneManagement;
using RayznGames.VehicleSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace ProjectExhibitionist.FirstPerson
{
	// Token: 0x020009EE RID: 2542
	[DefaultExecutionOrder(1000)]
	public class FirstPersonController : MonoBehaviour
	{
		// Token: 0x17000C31 RID: 3121
		// (get) Token: 0x060047ED RID: 18413 RVA: 0x001B5823 File Offset: 0x001B3A23
		// (set) Token: 0x060047EE RID: 18414 RVA: 0x001B582B File Offset: 0x001B3A2B
		public bool IsFirstPerson { get; private set; }

		// Token: 0x17000C32 RID: 3122
		// (get) Token: 0x060047EF RID: 18415 RVA: 0x001B5834 File Offset: 0x001B3A34
		public InventoryItemSO CarriedCameraItem
		{
			get
			{
				if (this.cameraInventory == null)
				{
					this.cameraInventory = base.GetComponentInChildren<PlayerInventoryManager>(true);
				}
				if (this.cameraInventory == null || !this.cameraInventory.isActiveAndEnabled)
				{
					return null;
				}
				InventoryItemSO activeItem = this.cameraInventory.GetActiveItem();
				if (!(activeItem == null))
				{
					HideFlashItemSO hideFlashItemSO = activeItem as HideFlashItemSO;
					if (hideFlashItemSO == null || !hideFlashItemSO.usesWornDetailAnimator)
					{
						if (this.HasMountedVehicle() || this.IsLocomotionPosture() || this.IsRecoveringFromRagdoll || this.interactionViewOwner != null || this.IsInWallCover() || this.CameraAnchorOverride != null || this.YawClampOverrideCenter != null)
						{
							return null;
						}
						return activeItem;
					}
				}
				return null;
			}
		}

		// Token: 0x17000C33 RID: 3123
		// (get) Token: 0x060047F0 RID: 18416 RVA: 0x001B58F1 File Offset: 0x001B3AF1
		public bool UsesCarriedItemPreset
		{
			get
			{
				return this.CarriedCameraItem != null;
			}
		}

		// Token: 0x17000C34 RID: 3124
		// (get) Token: 0x060047F1 RID: 18417 RVA: 0x001B58FF File Offset: 0x001B3AFF
		public float MovementYaw
		{
			get
			{
				return this.fpMouseX;
			}
		}

		// Token: 0x17000C35 RID: 3125
		// (get) Token: 0x060047F2 RID: 18418 RVA: 0x001B5907 File Offset: 0x001B3B07
		// (set) Token: 0x060047F3 RID: 18419 RVA: 0x001B590F File Offset: 0x001B3B0F
		public bool IsCinematicViewSuspended { get; private set; }

		// Token: 0x17000C36 RID: 3126
		// (get) Token: 0x060047F4 RID: 18420 RVA: 0x001B5918 File Offset: 0x001B3B18
		public bool IsInteractionViewSuspended
		{
			get
			{
				return this.firstPersonSuspensionOwners.Count != 0;
			}
		}

		// Token: 0x17000C37 RID: 3127
		// (get) Token: 0x060047F5 RID: 18421 RVA: 0x001B5928 File Offset: 0x001B3B28
		// (set) Token: 0x060047F6 RID: 18422 RVA: 0x001B5930 File Offset: 0x001B3B30
		public bool SuppressMouseLookForPhotoView { get; set; }

		// Token: 0x17000C38 RID: 3128
		// (get) Token: 0x060047F7 RID: 18423 RVA: 0x001B5939 File Offset: 0x001B3B39
		// (set) Token: 0x060047F8 RID: 18424 RVA: 0x001B5941 File Offset: 0x001B3B41
		public bool ExternalLock { get; set; }

		// Token: 0x17000C39 RID: 3129
		// (get) Token: 0x060047F9 RID: 18425 RVA: 0x001B594A File Offset: 0x001B3B4A
		// (set) Token: 0x060047FA RID: 18426 RVA: 0x001B5952 File Offset: 0x001B3B52
		public Transform CameraAnchorOverride { get; set; }

		// Token: 0x17000C3A RID: 3130
		// (get) Token: 0x060047FB RID: 18427 RVA: 0x001B595B File Offset: 0x001B3B5B
		public bool IsRidingVehicle
		{
			get
			{
				return PlayerActivityGate.IsResourceOwned(PlayerContextResource.Rider) && this.HasMountedVehicle();
			}
		}

		// Token: 0x17000C3B RID: 3131
		// (get) Token: 0x060047FC RID: 18428 RVA: 0x001B596E File Offset: 0x001B3B6E
		// (set) Token: 0x060047FD RID: 18429 RVA: 0x001B5976 File Offset: 0x001B3B76
		public float? YawClampOverrideCenter { get; set; }

		// Token: 0x17000C3C RID: 3132
		// (get) Token: 0x060047FE RID: 18430 RVA: 0x001B597F File Offset: 0x001B3B7F
		// (set) Token: 0x060047FF RID: 18431 RVA: 0x001B5987 File Offset: 0x001B3B87
		public float YawClampOverrideRange { get; set; } = 100f;

		// Token: 0x17000C3D RID: 3133
		// (get) Token: 0x06004800 RID: 18432 RVA: 0x001B5990 File Offset: 0x001B3B90
		public string ReturnCameraState
		{
			get
			{
				return this.savedCameraState;
			}
		}

		// Token: 0x17000C3E RID: 3134
		// (get) Token: 0x06004801 RID: 18433 RVA: 0x001B5998 File Offset: 0x001B3B98
		// (set) Token: 0x06004802 RID: 18434 RVA: 0x001B59A0 File Offset: 0x001B3BA0
		public int CameraPoseFrame { get; private set; } = -1;

		// Token: 0x06004803 RID: 18435 RVA: 0x001B59A9 File Offset: 0x001B3BA9
		private void OnEnable()
		{
			WorldOrigin.Shifted += this.ApplyWorldOriginShift;
			SceneManager.activeSceneChanged += this.ResetViewOnSceneChange;
		}

		// Token: 0x06004804 RID: 18436 RVA: 0x001B59CD File Offset: 0x001B3BCD
		private void ResetViewOnSceneChange(Scene previous, Scene next)
		{
			this.ResetForSceneTransition();
		}

		// Token: 0x06004805 RID: 18437 RVA: 0x001B59D8 File Offset: 0x001B3BD8
		public void ResetForSceneTransition()
		{
			this.firstPersonSuspensionOwners.Clear();
			this.restoreFirstPersonAfterInteraction = false;
			this.ExitFirstPerson();
			this.ExternalLock = false;
			this.CameraAnchorOverride = null;
			this.YawClampOverrideCenter = null;
			this.interactionViewOwner = null;
			this.interactionKeepsHorizonLevel = false;
			this.SuppressMouseLookForPhotoView = false;
		}

		// Token: 0x06004806 RID: 18438 RVA: 0x001B5A2F File Offset: 0x001B3C2F
		private void ApplyWorldOriginShift(Vector3 delta)
		{
			this.wallPeekPosition += delta;
		}

		// Token: 0x06004807 RID: 18439 RVA: 0x001B5A44 File Offset: 0x001B3C44
		private void OnDisable()
		{
			WorldOrigin.Shifted -= this.ApplyWorldOriginShift;
			SceneManager.activeSceneChanged -= this.ResetViewOnSceneChange;
			this.firstPersonSuspensionOwners.Clear();
			this.restoreFirstPersonAfterInteraction = false;
			if (this.IsFirstPerson)
			{
				this.ExitFirstPerson();
			}
		}

		// Token: 0x06004808 RID: 18440 RVA: 0x001B5A94 File Offset: 0x001B3C94
		private void Start()
		{
			this.tpInput = base.GetComponent<vThirdPersonInput>();
			this.cc = base.GetComponent<vThirdPersonController>();
			this.animator = base.GetComponent<Animator>();
			this.ragdollRecovery = base.GetComponent<CielRagdollRecovery>();
			this.wallCover = base.GetComponent<vBasicCoverController>();
			this.emotionController = PlayerEmotionController.Instance;
			if (this.animator != null)
			{
				this.headBone = this.animator.GetBoneTransform(HumanBodyBones.Head);
				this.leftEyeBone = this.animator.GetBoneTransform(HumanBodyBones.LeftEye);
				this.rightEyeBone = this.animator.GetBoneTransform(HumanBodyBones.RightEye);
			}
			this.leftBreastBaseBone = FirstPersonController.FindDescendantByName(base.transform, "c_breast_01.l");
			this.rightBreastBaseBone = FirstPersonController.FindDescendantByName(base.transform, "c_breast_01.r");
			if (this.headBone == null)
			{
				Debug.LogError("[FirstPersonController] Could not find Head bone on Animator. FP mode will not work.");
			}
			if (this.positionReferenceBone == null)
			{
				Debug.LogWarning("[FirstPersonController] positionReferenceBone not assigned; Head will be used if the eye bones are unavailable.");
				this.positionReferenceBone = this.headBone;
			}
		}

		// Token: 0x06004809 RID: 18441 RVA: 0x001B5B98 File Offset: 0x001B3D98
		private void LateUpdate()
		{
			if (GraphicsSettingsRuntimeUI.AnyOpen || Time.timeScale <= 0f)
			{
				return;
			}
			if (!this.IsFirstPerson)
			{
				if (this.headBone != null && !this.IsRecoveringFromRagdoll)
				{
					this.CacheNormalHeadYawBasis(base.transform.eulerAngles.y);
				}
				return;
			}
			this.CacheReferences();
			if (this.tpCamera == null || this.headBone == null || this.mainCamera == null)
			{
				return;
			}
			if (this.UpdateCinematicOwnership())
			{
				return;
			}
			this.firstPersonFieldOfView = this.mainCamera.fieldOfView;
			this.headVisibleForCurrentView = this.ShouldShowHeadForCurrentView();
			if (Time.unscaledTime >= this.nextHeadRendererRefreshTime)
			{
				this.RefreshHeadRenderers(false);
			}
			else
			{
				this.ApplyTrackedHeadVisibility();
			}
			this.EnsureMainCamLocalZero(true);
			if (!this.IsRecoveringFromRagdoll && this.CameraAnchorOverride == null && this.UpdateInteractionCamera())
			{
				this.CameraPoseFrame = Time.frameCount;
				return;
			}
			if (this.YawClampOverrideCenter != null && this.CameraAnchorOverride == null)
			{
				this.UpdateHideSpotCamera();
				this.CameraPoseFrame = Time.frameCount;
				return;
			}
			this.UpdatePresetBlend();
			if (this.CameraAnchorOverride == null && !this.IsRidingVehicle && !this.IsRecoveringFromRagdoll && this.YawClampOverrideCenter == null && this.cc != null && !this.cc.lockMovement && !this.cc.lockRotation)
			{
				this.UpdateCharacterRotation();
			}
			this.UpdateFirstPersonCamera();
			this.CameraPoseFrame = Time.frameCount;
		}

		// Token: 0x0600480A RID: 18442 RVA: 0x001B5D38 File Offset: 0x001B3F38
		private void CacheReferences()
		{
			if (this.tpCamera == null && this.tpInput != null)
			{
				this.tpCamera = this.tpInput.tpCamera;
			}
			if (this.mainCamera == null)
			{
				this.mainCamera = ((this.tpInput != null) ? this.tpInput.cameraMain : null);
				if (this.mainCamera == null)
				{
					this.mainCamera = Camera.main;
				}
			}
			if (this.cinematicBrain == null && this.mainCamera != null)
			{
				this.cinematicBrain = this.mainCamera.GetComponent<CinemachineBrain>();
			}
		}

		// Token: 0x17000C3F RID: 3135
		// (get) Token: 0x0600480B RID: 18443 RVA: 0x001B5DE8 File Offset: 0x001B3FE8
		private bool IsRecoveringFromRagdoll
		{
			get
			{
				return (this.cc != null && this.cc.ragdolled) || (this.ragdollRecovery != null && this.ragdollRecovery.IsRecovering);
			}
		}

		// Token: 0x0600480C RID: 18444 RVA: 0x001B5E22 File Offset: 0x001B4022
		private bool CinematicCameraOwnsView()
		{
			return this.cinematicBrain != null && this.cinematicBrain.isActiveAndEnabled && (this.cinematicBrain.ActiveVirtualCamera != null || this.cinematicBrain.IsBlending);
		}

		// Token: 0x0600480D RID: 18445 RVA: 0x001B5E5C File Offset: 0x001B405C
		private bool UpdateCinematicOwnership()
		{
			if (this.CinematicCameraOwnsView())
			{
				if (!this.IsCinematicViewSuspended)
				{
					this.IsCinematicViewSuspended = true;
					this.ResetRagdollEyeRotation();
					this.suspendedYawOffset = Mathf.DeltaAngle(base.transform.eulerAngles.y, this.fpMouseX);
					if (this.cameraCalibration != null)
					{
						this.cameraCalibration.EndSession();
					}
					Vector3 position = this.mainCamera.transform.position;
					Quaternion rotation = this.mainCamera.transform.rotation;
					this.EnsureMainCamLocalZero(false);
					this.mainCamera.transform.SetPositionAndRotation(position, rotation);
				}
				this.headVisibleForCurrentView = true;
				if (Time.unscaledTime >= this.nextHeadRendererRefreshTime)
				{
					this.RefreshHeadRenderers(false);
				}
				else
				{
					this.ApplyTrackedHeadVisibility();
				}
				return true;
			}
			if (this.IsCinematicViewSuspended)
			{
				this.IsCinematicViewSuspended = false;
				this.fpMouseX = base.transform.eulerAngles.y + this.suspendedYawOffset;
				this.headYInitialized = false;
				this.tpCamera.ChangeState(this.firstPersonStateName, true);
				this.tpCamera.isFreezed = true;
				this.mainCamera.nearClipPlane = this.fpNearClipPlane;
				this.mainCamera.fieldOfView = this.firstPersonFieldOfView;
				Vector3 vector = this.savedMainCamLocalPos;
				Quaternion quaternion = this.savedMainCamLocalRotation;
				this.EnsureMainCamLocalZero(true);
				this.savedMainCamLocalPos = vector;
				this.savedMainCamLocalRotation = quaternion;
			}
			return false;
		}

		// Token: 0x0600480E RID: 18446 RVA: 0x001B5FC0 File Offset: 0x001B41C0
		public void ToggleFirstPerson()
		{
			if (this.IsInteractionViewSuspended)
			{
				return;
			}
			this.CacheReferences();
			if (this.wallCover == null)
			{
				this.wallCover = base.GetComponent<vBasicCoverController>();
			}
			if (this.CinematicCameraOwnsView() && (this.IsFirstPerson || this.wallCover == null || !this.wallCover.CanReleasePeekCameraForFirstPerson(this.mainCamera)))
			{
				return;
			}
			if (this.ExternalLock)
			{
				return;
			}
			if (!this.IsFirstPerson && TutorialService.TryShowFirstUse("perspective"))
			{
				return;
			}
			if (this.IsFirstPerson)
			{
				this.ExitFirstPerson();
				return;
			}
			this.EnterFirstPerson();
		}

		// Token: 0x0600480F RID: 18447 RVA: 0x001B605C File Offset: 0x001B425C
		public void EnterFirstPerson()
		{
			if (this.IsFirstPerson || this.IsInteractionViewSuspended || (SceneTransitionManager.Instance != null && SceneTransitionManager.Instance.IsTransitioning))
			{
				return;
			}
			this.CacheReferences();
			if (this.wallCover == null)
			{
				this.wallCover = base.GetComponent<vBasicCoverController>();
			}
			if (this.CinematicCameraOwnsView() && this.wallCover != null)
			{
				this.wallCover.TryReleasePeekCameraForFirstPerson(this.mainCamera);
			}
			if (this.CinematicCameraOwnsView())
			{
				return;
			}
			if (this.tpCamera == null || this.headBone == null || this.mainCamera == null)
			{
				Debug.LogWarning("[FirstPersonController] Cannot enter FP mode: missing camera or head bone.");
				return;
			}
			this.IsFirstPerson = true;
			this.headVisibleForCurrentView = false;
			this.savedCameraState = this.tpCamera.currentStateName;
			this.savedNearClipPlane = this.mainCamera.nearClipPlane;
			this.firstPersonFieldOfView = this.mainCamera.fieldOfView;
			this.tpCamera.ChangeState(this.firstPersonStateName, true);
			this.tpCamera.isFreezed = true;
			this.SetHeadRenderersToShadowsOnly();
			this.targetPresetBlend = this.ResolveTargetPresetBlend();
			this.currentPresetBlend = this.targetPresetBlend;
			this.mainCamera.nearClipPlane = this.fpNearClipPlane;
			this.EnsureMainCamLocalZero(true);
			this.headYInitialized = false;
			this.ResetRagdollEyeRotation();
			this.wallViewActive = false;
			this.fpMouseX = this.tpCamera.mouseX;
			this.fpMouseY = this.tpCamera.mouseY;
			if (this.YawClampOverrideCenter != null)
			{
				this.hideYawOffset = 0f;
				this.hidePitch = Mathf.Clamp(this.hideSpotEntryPitch, this.hideSpotPitchMin, this.hideSpotPitchMax);
			}
			UnityEvent onEnterFirstPerson = this.OnEnterFirstPerson;
			if (onEnterFirstPerson == null)
			{
				return;
			}
			onEnterFirstPerson.Invoke();
		}

		// Token: 0x06004810 RID: 18448 RVA: 0x001B622C File Offset: 0x001B442C
		public void ExitFirstPerson()
		{
			if (!this.IsFirstPerson)
			{
				return;
			}
			if (this.cameraCalibration != null)
			{
				this.cameraCalibration.EndSession();
			}
			this.IsFirstPerson = false;
			this.IsCinematicViewSuspended = false;
			this.ResetRagdollEyeRotation();
			this.wallViewActive = false;
			this.headVisibleForCurrentView = false;
			this.SuppressMouseLookForPhotoView = false;
			this.CacheReferences();
			if (this.tpCamera != null)
			{
				this.tpCamera.mouseX = this.fpMouseX;
				this.tpCamera.mouseY = this.fpMouseY;
				this.tpCamera.isFreezed = false;
				if (!string.IsNullOrEmpty(this.savedCameraState))
				{
					this.tpCamera.ChangeState(this.savedCameraState, true);
				}
			}
			this.RestoreHeadRenderers();
			this.EnsureMainCamLocalZero(false);
			if (this.mainCamera != null)
			{
				this.mainCamera.nearClipPlane = this.savedNearClipPlane;
			}
			UnityEvent onExitFirstPerson = this.OnExitFirstPerson;
			if (onExitFirstPerson == null)
			{
				return;
			}
			onExitFirstPerson.Invoke();
		}

		// Token: 0x06004811 RID: 18449 RVA: 0x001B6321 File Offset: 0x001B4521
		private void UpdatePresetBlend()
		{
			this.targetPresetBlend = this.ResolveTargetPresetBlend();
			this.currentPresetBlend = Mathf.MoveTowards(this.currentPresetBlend, this.targetPresetBlend, this.presetBlendSpeed * Time.deltaTime);
		}

		// Token: 0x06004812 RID: 18450 RVA: 0x001B6354 File Offset: 0x001B4554
		private float ResolveTargetPresetBlend()
		{
			if (this.HasMountedVehicle() || this.IsLocomotionPosture() || this.IsRecoveringFromRagdoll || this.UsesCarriedItemPreset)
			{
				return 0f;
			}
			if (EmotionManager.Instance != null)
			{
				if (EmotionManager.Instance.CurrentStance != EmotionStance.Embarassed)
				{
					return 0f;
				}
				return 1f;
			}
			else
			{
				if (this.emotionController == null)
				{
					this.emotionController = PlayerEmotionController.Instance;
				}
				if (!(this.emotionController != null))
				{
					return 0f;
				}
				if (this.emotionController.ShameLevel <= this.emotionController.ExhibitionLevel)
				{
					return 0f;
				}
				return 1f;
			}
		}

		// Token: 0x06004813 RID: 18451 RVA: 0x001B6400 File Offset: 0x001B4600
		private bool IsLocomotionPosture()
		{
			if (this.cc != null && this.cc.isCrouching)
			{
				return true;
			}
			if (this.emoteController == null)
			{
				this.emoteController = base.GetComponent<GameplayEmoteController>();
			}
			if (this.emoteController == null || !this.emoteController.IsPlaying || this.emoteController.PlayingEmote == null)
			{
				return false;
			}
			string emoteId = this.emoteController.PlayingEmote.EmoteId;
			return string.Equals(emoteId, "crouch_walk", StringComparison.OrdinalIgnoreCase) || string.Equals(emoteId, "crawl_walk", StringComparison.OrdinalIgnoreCase) || string.Equals(emoteId, "prone_walk", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06004814 RID: 18452 RVA: 0x001B64B0 File Offset: 0x001B46B0
		private bool UsesCheerleaderEyeAnchor()
		{
			if (this.emoteController == null)
			{
				this.emoteController = base.GetComponent<GameplayEmoteController>();
			}
			if (this.emoteController == null || !this.emoteController.IsPlaying || this.emoteController.PlayingEmote == null)
			{
				return false;
			}
			string emoteId = this.emoteController.PlayingEmote.EmoteId;
			return emoteId == "cheerleader_dance" || emoteId == "cheerleader_dance_2" || emoteId == "cheerleader_dance_8";
		}

		// Token: 0x06004815 RID: 18453 RVA: 0x001B6540 File Offset: 0x001B4740
		private void GetViewPresets(out FPCameraPreset from, out FPCameraPreset to, out float blend)
		{
			bool usesCarriedItemPreset = this.UsesCarriedItemPreset;
			from = (usesCarriedItemPreset ? (this.carriedItemPreset ?? this.confidentPreset) : this.confidentPreset);
			to = (usesCarriedItemPreset ? from : this.embarrassedPreset);
			blend = (usesCarriedItemPreset ? 0f : this.currentPresetBlend);
		}

		// Token: 0x06004816 RID: 18454 RVA: 0x001B6594 File Offset: 0x001B4794
		private Vector3 GetEyePosition()
		{
			if (this.IsRecoveringFromRagdoll)
			{
				this.headYInitialized = false;
				return this.GetRagdollEyePosition();
			}
			if (this.UsesCheerleaderEyeAnchor())
			{
				this.headYInitialized = false;
				return this.GetInteractionEyePosition();
			}
			FPCameraPreset fpcameraPreset;
			FPCameraPreset fpcameraPreset2;
			float num;
			this.GetViewPresets(out fpcameraPreset, out fpcameraPreset2, out num);
			float num2 = Mathf.Lerp(fpcameraPreset.eyeHeightOffset, fpcameraPreset2.eyeHeightOffset, num);
			float num3 = Mathf.Lerp(fpcameraPreset.eyeForwardOffset, fpcameraPreset2.eyeForwardOffset, num);
			float num4 = Mathf.Lerp(fpcameraPreset.torsoHorizontalAnchor, fpcameraPreset2.torsoHorizontalAnchor, num);
			float num5 = Mathf.Lerp(fpcameraPreset.torsoForwardOffset, fpcameraPreset2.torsoForwardOffset, num);
			float num6 = Mathf.Lerp(fpcameraPreset.torsoVerticalAnchor, fpcameraPreset2.torsoVerticalAnchor, num);
			float num7 = Mathf.Lerp(fpcameraPreset.torsoHeightOffset, fpcameraPreset2.torsoHeightOffset, num);
			Vector3 vector2;
			if (this.leftEyeBone != null && this.rightEyeBone != null)
			{
				Vector3 vector = (this.leftEyeBone.position + this.rightEyeBone.position) * 0.5f;
				vector2 = vector + base.transform.up * num2 + this.GetPoseFaceForward(vector) * num3;
			}
			else
			{
				Transform transform = ((this.headBone != null) ? this.headBone : this.positionReferenceBone);
				Vector3 normalized = Vector3.ProjectOnPlane(base.transform.forward, base.transform.up).normalized;
				vector2 = transform.position + base.transform.up * (0.06f + num2) + normalized * (0.08f + num3);
			}
			if (num4 > 0.0001f && this.positionReferenceBone != null)
			{
				Vector3 up = base.transform.up;
				Vector3 vector3 = Vector3.ProjectOnPlane(base.transform.forward, up).normalized;
				if (this.leftBreastBaseBone != null && this.rightBreastBaseBone != null)
				{
					Vector3 vector4 = Vector3.ProjectOnPlane((this.leftBreastBaseBone.position + this.rightBreastBaseBone.position) * 0.5f - this.positionReferenceBone.position, up);
					if (vector4.sqrMagnitude > 1E-06f)
					{
						vector3 = vector4.normalized;
					}
				}
				Vector3 vector5 = this.positionReferenceBone.position + vector3 * num5;
				Vector3 vector6 = vector5 + Vector3.Project(vector2 - vector5, up);
				vector2 = Vector3.Lerp(vector2, vector6, num4);
				if (num6 > 0.0001f)
				{
					float num8 = Vector3.Dot(vector2 - this.positionReferenceBone.position, up);
					vector2 += up * ((num7 - num8) * num6);
				}
			}
			float y = base.transform.InverseTransformPoint(vector2).y;
			if (!this.headYInitialized)
			{
				this.smoothedEyeLocalY = y;
				this.headYInitialized = true;
			}
			else
			{
				float num9 = Mathf.Clamp01((1f - this.verticalDamping) * 10f * Time.deltaTime);
				this.smoothedEyeLocalY = Mathf.Lerp(this.smoothedEyeLocalY, y, num9);
			}
			float num10 = Mathf.Lerp(y, this.smoothedEyeLocalY, this.verticalDamping);
			Vector3 vector7 = base.transform.TransformVector(Vector3.up * (num10 - y));
			vector2 += Vector3.ClampMagnitude(vector7, Mathf.Max(0f, this.maximumVerticalLag));
			if (this.HasMountedVehicle() && !this.HasMountedBicycle() && !this.HasMountedSkateboard())
			{
				vector2 = this.ConstrainMountedEyePosition(vector2);
			}
			return vector2;
		}

		// Token: 0x06004817 RID: 18455 RVA: 0x001B6948 File Offset: 0x001B4B48
		private Vector3 GetRagdollEyePosition()
		{
			Vector3 vector = ((this.leftEyeBone != null && this.rightEyeBone != null) ? ((this.leftEyeBone.position + this.rightEyeBone.position) * 0.5f) : this.headBone.position);
			if (this.ragdollHeadCollider == null)
			{
				this.ragdollHeadCollider = this.headBone.GetComponent<SphereCollider>();
			}
			if (this.ragdollHeadCollider == null)
			{
				return vector;
			}
			Transform transform = this.ragdollHeadCollider.transform;
			Vector3 vector2 = transform.TransformPoint(this.ragdollHeadCollider.center);
			Vector3 lossyScale = transform.lossyScale;
			float num = this.ragdollHeadCollider.radius * Mathf.Max(new float[]
			{
				Mathf.Abs(lossyScale.x),
				Mathf.Abs(lossyScale.y),
				Mathf.Abs(lossyScale.z)
			});
			float num2 = ((this.mainCamera != null) ? this.mainCamera.nearClipPlane : this.fpNearClipPlane);
			return vector2 + Vector3.ClampMagnitude(vector - vector2, Mathf.Max(0f, num - Mathf.Max(0.02f, num2 + 0.005f)));
		}

		// Token: 0x06004818 RID: 18456 RVA: 0x001B6A8C File Offset: 0x001B4C8C
		private bool HasMountedVehicle()
		{
			if (this.cc == null || !this.cc.lockMovement || base.transform.parent == null)
			{
				return false;
			}
			Transform parent = base.transform.parent;
			vVehicleController vVehicleController;
			if (parent.TryGetComponent<vVehicleController>(out vVehicleController))
			{
				return vVehicleController.IsBeingRidden && vVehicleController.CurrentRider == this.cc;
			}
			vBikeController vBikeController;
			if (parent.TryGetComponent<vBikeController>(out vBikeController))
			{
				return vBikeController.CurrentRider == this.cc;
			}
			SimpleRollerSkatesInteractable simpleRollerSkatesInteractable;
			if (parent.TryGetComponent<SimpleRollerSkatesInteractable>(out simpleRollerSkatesInteractable))
			{
				return simpleRollerSkatesInteractable.IsBeingWorn && simpleRollerSkatesInteractable.CurrentRider == this.cc;
			}
			SkateboardInteractable skateboardInteractable;
			return parent.TryGetComponent<SkateboardInteractable>(out skateboardInteractable) && skateboardInteractable.IsBeingWorn && skateboardInteractable.CurrentRider == this.cc;
		}

		// Token: 0x06004819 RID: 18457 RVA: 0x001B6B68 File Offset: 0x001B4D68
		private bool HasMountedBicycle()
		{
			if (this.cc == null || !this.cc.lockMovement || base.transform.parent == null)
			{
				return false;
			}
			vVehicleController vVehicleController;
			if (base.transform.parent.TryGetComponent<vVehicleController>(out vVehicleController))
			{
				return vVehicleController.Type == vVehicleController.VehicleType.Bicycle && vVehicleController.IsBeingRidden && vVehicleController.CurrentRider == this.cc;
			}
			vBikeController vBikeController;
			return base.transform.parent.TryGetComponent<vBikeController>(out vBikeController) && vBikeController.IsBeingRidden && vBikeController.CurrentRider == this.cc;
		}

		// Token: 0x0600481A RID: 18458 RVA: 0x001B6C0C File Offset: 0x001B4E0C
		private bool HasMountedSkateboard()
		{
			SkateboardInteractable skateboardInteractable;
			return this.cc != null && this.cc.lockMovement && base.transform.parent != null && base.transform.parent.TryGetComponent<SkateboardInteractable>(out skateboardInteractable) && skateboardInteractable.IsBeingWorn && skateboardInteractable.CurrentRider == this.cc;
		}

		// Token: 0x0600481B RID: 18459 RVA: 0x001B6C78 File Offset: 0x001B4E78
		private Vector3 ConstrainMountedEyePosition(Vector3 eyePosition)
		{
			if (this.positionReferenceBone == null)
			{
				return eyePosition;
			}
			Vector3 up = base.transform.up;
			Vector3 normalized = Vector3.ProjectOnPlane(base.transform.forward, up).normalized;
			Vector3 vector = eyePosition - this.positionReferenceBone.position;
			eyePosition -= normalized * Mathf.Max(0f, Vector3.Dot(vector, normalized) - this.maximumMountedCameraForward);
			eyePosition += up * Mathf.Max(0f, this.minimumMountedCameraHeight - Vector3.Dot(vector, up));
			return eyePosition;
		}

		// Token: 0x0600481C RID: 18460 RVA: 0x001B6D1C File Offset: 0x001B4F1C
		private Vector3 GetPoseFaceForward(Vector3 eyeMid)
		{
			Vector3 vector = ((this.headBone != null) ? (eyeMid - this.headBone.position) : base.transform.forward);
			vector = Vector3.ProjectOnPlane(vector, base.transform.up);
			if (vector.sqrMagnitude < 1E-06f)
			{
				vector = Vector3.ProjectOnPlane(base.transform.forward, base.transform.up);
			}
			return vector.normalized;
		}

		// Token: 0x0600481D RID: 18461 RVA: 0x001B6D9C File Offset: 0x001B4F9C
		private static Transform FindDescendantByName(Transform root, string wantedName)
		{
			if (root == null || string.IsNullOrEmpty(wantedName))
			{
				return null;
			}
			Transform[] componentsInChildren = root.GetComponentsInChildren<Transform>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (string.Equals(componentsInChildren[i].name, wantedName, StringComparison.OrdinalIgnoreCase))
				{
					return componentsInChildren[i];
				}
			}
			return null;
		}

		// Token: 0x0600481E RID: 18462 RVA: 0x001B6DE8 File Offset: 0x001B4FE8
		private void UpdateFirstPersonCamera()
		{
			if (this.CameraAnchorOverride != null)
			{
				this.tpCamera.transform.position = this.CameraAnchorOverride.position;
				this.tpCamera.transform.rotation = this.CameraAnchorOverride.rotation;
				return;
			}
			Vector3 eyePosition = this.GetEyePosition();
			Quaternion eyeRotation = this.GetEyeRotation();
			this.tpCamera.transform.position = eyePosition;
			this.tpCamera.transform.rotation = eyeRotation;
		}

		// Token: 0x0600481F RID: 18463 RVA: 0x001B6E6C File Offset: 0x001B506C
		private void UpdateHideSpotCamera()
		{
			if (this.headBone == null)
			{
				return;
			}
			Vector3 position = this.headBone.position;
			Vector3 vector = ((this.leftEyeBone != null && this.rightEyeBone != null) ? ((this.leftEyeBone.position + this.rightEyeBone.position) * 0.5f) : (position + Vector3.up * 0.06f));
			Vector3 vector2 = vector - position;
			vector2.y = 0f;
			if (vector2.sqrMagnitude < 1E-06f)
			{
				vector2 = -base.transform.forward;
			}
			vector2.Normalize();
			float num = Mathf.Atan2(vector2.x, vector2.z) * 57.29578f;
			Vector3 vector3 = vector + vector2 * this.hideSpotForwardOffset + Vector3.up * this.hideSpotVerticalOffset;
			Quaternion quaternion = Quaternion.Euler(this.hidePitch, num + this.hideYawOffset, 0f);
			this.tpCamera.transform.SetPositionAndRotation(vector3, quaternion);
		}

		// Token: 0x06004820 RID: 18464 RVA: 0x001B6F94 File Offset: 0x001B5194
		private void EnsureMainCamLocalZero(bool wanted)
		{
			if (wanted == this.mainCamLocalZeroed)
			{
				return;
			}
			if (this.mainCamera == null || this.tpCamera == null || this.mainCamera.transform.parent != this.tpCamera.transform)
			{
				return;
			}
			if (wanted)
			{
				this.savedMainCamLocalPos = this.mainCamera.transform.localPosition;
				this.savedMainCamLocalRotation = this.mainCamera.transform.localRotation;
				this.mainCamera.transform.localPosition = Vector3.zero;
				this.mainCamera.transform.localRotation = Quaternion.identity;
			}
			else
			{
				this.mainCamera.transform.localPosition = this.savedMainCamLocalPos;
				this.mainCamera.transform.localRotation = this.savedMainCamLocalRotation;
			}
			this.mainCamLocalZeroed = wanted;
		}

		// Token: 0x06004821 RID: 18465 RVA: 0x001B7078 File Offset: 0x001B5278
		private Quaternion GetEyeRotation()
		{
			FPCameraPreset fpcameraPreset;
			FPCameraPreset fpcameraPreset2;
			float num;
			this.GetViewPresets(out fpcameraPreset, out fpcameraPreset2, out num);
			Vector3 vector = Vector3.Lerp(fpcameraPreset.rotationOffset, fpcameraPreset2.rotationOffset, num);
			if (!this.IsRecoveringFromRagdoll && this.UsesCheerleaderEyeAnchor())
			{
				vector = Vector3.zero;
			}
			float num2 = Mathf.Clamp(this.fpMouseY + vector.x, -89f, 89f);
			Quaternion quaternion = Quaternion.Euler(num2, this.fpMouseX + vector.y, vector.z);
			bool flag = this.IsRecoveringFromRagdoll && this.headBone != null;
			if (flag)
			{
				if (!this.ragdollLookBasisValid)
				{
					this.CacheNormalHeadYawBasis(this.fpMouseX);
				}
				float num3 = Mathf.DeltaAngle(this.ragdollLookReferenceYaw, this.fpMouseX);
				quaternion = this.headBone.rotation * this.ragdollHeadLocalYawBasis * Quaternion.Euler(num2, num3 + vector.y, vector.z);
			}
			else
			{
				this.CacheNormalHeadYawBasis(this.fpMouseX);
			}
			if (!this.eyeRotationInitialized)
			{
				this.lastEyeRotation = ((flag && this.mainCamera != null) ? this.mainCamera.transform.rotation : quaternion);
				this.eyeRotationInitialized = true;
			}
			if (flag != this.ragdollRotationWasRecovering)
			{
				this.ragdollRotationCorrection = this.lastEyeRotation * Quaternion.Inverse(quaternion);
				this.ragdollRotationElapsed = 0f;
				this.ragdollRotationWasRecovering = flag;
			}
			float num4 = (flag ? 0.12f : 0.25f);
			float num5 = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(this.ragdollRotationElapsed / num4));
			this.lastEyeRotation = Quaternion.Slerp(this.ragdollRotationCorrection, Quaternion.identity, num5) * quaternion;
			this.ragdollRotationElapsed = Mathf.Min(num4, this.ragdollRotationElapsed + Time.unscaledDeltaTime);
			return this.lastEyeRotation;
		}

		// Token: 0x06004822 RID: 18466 RVA: 0x001B7258 File Offset: 0x001B5458
		private void CacheNormalHeadYawBasis(float yaw)
		{
			if (this.headBone == null)
			{
				return;
			}
			this.ragdollHeadLocalYawBasis = Quaternion.Inverse(this.headBone.rotation) * Quaternion.Euler(0f, yaw, 0f);
			this.ragdollLookReferenceYaw = yaw;
			this.ragdollLookBasisValid = true;
		}

		// Token: 0x06004823 RID: 18467 RVA: 0x001B72AD File Offset: 0x001B54AD
		private void ResetRagdollEyeRotation()
		{
			this.ragdollRotationWasRecovering = false;
			this.ragdollRotationCorrection = Quaternion.identity;
			this.ragdollRotationElapsed = 1f;
			this.eyeRotationInitialized = false;
		}

		// Token: 0x06004824 RID: 18468 RVA: 0x001B72D3 File Offset: 0x001B54D3
		private void UpdateCharacterRotation()
		{
			if (this.cc == null)
			{
				return;
			}
			this.cc.transform.rotation = Quaternion.Euler(0f, this.fpMouseX, 0f);
		}

		// Token: 0x06004825 RID: 18469 RVA: 0x001B7309 File Offset: 0x001B5509
		public void SuspendForInteraction(Object owner)
		{
			if (owner == null || !this.firstPersonSuspensionOwners.Add(owner))
			{
				return;
			}
			if (this.firstPersonSuspensionOwners.Count != 1)
			{
				return;
			}
			this.restoreFirstPersonAfterInteraction = this.IsFirstPerson;
			this.ExitFirstPerson();
		}

		// Token: 0x06004826 RID: 18470 RVA: 0x001B7344 File Offset: 0x001B5544
		public void ResumeAfterInteraction(Object owner)
		{
			if (!this.firstPersonSuspensionOwners.Remove(owner) || this.IsInteractionViewSuspended)
			{
				return;
			}
			bool flag = this.restoreFirstPersonAfterInteraction;
			this.restoreFirstPersonAfterInteraction = false;
			if (flag)
			{
				this.EnterFirstPerson();
			}
		}

		// Token: 0x06004827 RID: 18471 RVA: 0x001B7374 File Offset: 0x001B5574
		public void BeginInteractionView(Object owner, bool usePoseDirection, float yawHalfRange = 80f, bool invertedPose = false, bool keepHorizonLevel = false, Transform headingReference = null, Vector3? eyeOffset = null, Vector2? pitchLimits = null)
		{
			if (owner == null || this.interactionViewOwner == owner)
			{
				return;
			}
			this.interactionViewOwner = owner;
			this.interactionUsesPoseDirection = usePoseDirection;
			this.interactionHasInvertedPose = invertedPose;
			this.interactionKeepsHorizonLevel = keepHorizonLevel;
			this.interactionHeadingReference = headingReference;
			this.interactionEyeOffset = eyeOffset ?? Vector3.zero;
			this.interactionPitchLimits = pitchLimits;
			this.interactionEntryYaw = base.transform.eulerAngles.y;
			this.interactionHeadLocalBasis = ((this.headBone != null) ? (Quaternion.Inverse(this.headBone.rotation) * Quaternion.Euler(0f, this.interactionEntryYaw, 0f)) : Quaternion.identity);
			this.interactionLookYaw = (this.IsFirstPerson ? Mathf.Clamp(Mathf.DeltaAngle(this.interactionEntryYaw, this.fpMouseX), -80f, 80f) : 0f);
			this.interactionYawHalfRange = Mathf.Clamp(yawHalfRange, 0f, 89f);
			this.interactionLookYaw = Mathf.Clamp(this.interactionLookYaw, -this.interactionYawHalfRange, this.interactionYawHalfRange);
			this.headYInitialized = false;
		}

		// Token: 0x06004828 RID: 18472 RVA: 0x001B74B4 File Offset: 0x001B56B4
		public void EndInteractionView(Object owner)
		{
			if (this.interactionViewOwner != owner)
			{
				return;
			}
			if (this.IsFirstPerson)
			{
				this.fpMouseX = base.transform.eulerAngles.y + this.interactionLookYaw;
			}
			this.interactionViewOwner = null;
			this.interactionKeepsHorizonLevel = false;
			this.interactionHeadingReference = null;
			this.interactionEyeOffset = Vector3.zero;
			this.interactionPitchLimits = null;
			this.headYInitialized = false;
		}

		// Token: 0x06004829 RID: 18473 RVA: 0x001B7528 File Offset: 0x001B5728
		public void ReplaceInteractionReturnCamera(string temporaryState, string restoredState)
		{
			if (this.savedCameraState == temporaryState)
			{
				this.savedCameraState = restoredState;
			}
		}

		// Token: 0x0600482A RID: 18474 RVA: 0x001B753F File Offset: 0x001B573F
		public void RegisterHeadVisibleCamera(Camera camera)
		{
			if (camera != null)
			{
				this.registeredHeadVisibleCameras.Add(camera);
			}
		}

		// Token: 0x0600482B RID: 18475 RVA: 0x001B7557 File Offset: 0x001B5757
		public void UnregisterHeadVisibleCamera(Camera camera)
		{
			if (camera != null)
			{
				this.registeredHeadVisibleCameras.Remove(camera);
			}
		}

		// Token: 0x0600482C RID: 18476 RVA: 0x001B7570 File Offset: 0x001B5770
		private bool IsInWallCover()
		{
			if (this.wallCover == null)
			{
				this.wallCover = base.GetComponent<vBasicCoverController>();
			}
			return this.wallCover != null && this.wallCover.enabled && (this.wallCover.inCover || this.wallCover.goingToCoverPoint);
		}

		// Token: 0x0600482D RID: 18477 RVA: 0x001B75D0 File Offset: 0x001B57D0
		private float GetWallViewYaw()
		{
			if (this.positionReferenceBone != null && this.leftBreastBaseBone != null && this.rightBreastBaseBone != null)
			{
				Vector3 vector = Vector3.ProjectOnPlane((this.leftBreastBaseBone.position + this.rightBreastBaseBone.position) * 0.5f - this.positionReferenceBone.position, Vector3.up);
				if (vector.sqrMagnitude > 0.0009f)
				{
					return Mathf.Atan2(vector.x, vector.z) * 57.29578f;
				}
			}
			if (this.headBone != null && this.leftEyeBone != null && this.rightEyeBone != null)
			{
				Vector3 vector2 = Vector3.ProjectOnPlane(this.GetInteractionEyePosition() - this.headBone.position, Vector3.up);
				if (vector2.sqrMagnitude > 0.0009f)
				{
					return Mathf.Atan2(vector2.x, vector2.z) * 57.29578f;
				}
				if (this.wallViewActive)
				{
					return this.previousWallViewYaw;
				}
			}
			Vector3 vector3 = Vector3.ProjectOnPlane(((this.wallCover != null && this.wallCover.currentCoverPoint != null) ? this.wallCover.currentCoverPoint.transform : base.transform).forward, Vector3.up);
			return Mathf.Atan2(vector3.x, vector3.z) * 57.29578f;
		}

		// Token: 0x0600482E RID: 18478 RVA: 0x001B7750 File Offset: 0x001B5950
		private Vector3 GetInteractionEyePosition()
		{
			if (this.leftEyeBone != null && this.rightEyeBone != null)
			{
				return (this.leftEyeBone.position + this.rightEyeBone.position) * 0.5f;
			}
			return this.headBone.position + base.transform.up * 0.06f + base.transform.forward * 0.08f;
		}

		// Token: 0x0600482F RID: 18479 RVA: 0x001B77E0 File Offset: 0x001B59E0
		private float ClampInteractionPitch(float pitch)
		{
			if (this.interactionPitchLimits != null)
			{
				return Mathf.Clamp(pitch, this.interactionPitchLimits.Value.x, this.interactionPitchLimits.Value.y);
			}
			if (!this.interactionUsesPoseDirection || this.interactionHasInvertedPose || this.headBone == null)
			{
				return Mathf.Clamp(pitch, -70f, 80f);
			}
			float num = Mathf.Asin(Mathf.Clamp(-(this.headBone.rotation * this.interactionHeadLocalBasis * Quaternion.Euler(0f, this.interactionLookYaw, 0f) * Vector3.forward).y, -1f, 1f)) * 57.29578f;
			return Mathf.Clamp(pitch, Mathf.Max(-70f, Mathf.Min(0f, -70f - num)), Mathf.Min(80f, Mathf.Max(0f, 80f - num)));
		}

		// Token: 0x06004830 RID: 18480 RVA: 0x001B78E8 File Offset: 0x001B5AE8
		private bool UpdateInteractionCamera()
		{
			bool flag = this.IsInWallCover();
			if (!flag)
			{
				this.wallViewActive = false;
			}
			if (!flag && this.interactionViewOwner == null)
			{
				return false;
			}
			Vector3 vector = this.GetInteractionEyePosition();
			if (flag)
			{
				this.UpdateWallCoverCamera(vector);
				this.headYInitialized = false;
				return true;
			}
			this.fpMouseY = this.ClampInteractionPitch(this.fpMouseY);
			Quaternion interactionViewBasis = this.GetInteractionViewBasis();
			vector += ((this.interactionHeadingReference != null) ? this.interactionHeadingReference.rotation : interactionViewBasis) * this.interactionEyeOffset;
			Quaternion quaternion = interactionViewBasis * Quaternion.Euler(this.fpMouseY, this.interactionLookYaw, 0f);
			if (this.interactionKeepsHorizonLevel)
			{
				Vector3 vector2 = quaternion * Vector3.forward;
				if (Vector3.Cross(vector2, Vector3.up).sqrMagnitude > 1E-06f)
				{
					quaternion = Quaternion.LookRotation(vector2, Vector3.up);
				}
			}
			this.fpMouseX = quaternion.eulerAngles.y;
			this.tpCamera.transform.SetPositionAndRotation(vector, quaternion);
			this.headYInitialized = false;
			return true;
		}

		// Token: 0x06004831 RID: 18481 RVA: 0x001B7A04 File Offset: 0x001B5C04
		private Quaternion GetInteractionViewBasis()
		{
			if (!this.interactionUsesPoseDirection)
			{
				return Quaternion.Euler(0f, (this.interactionHeadingReference != null) ? this.interactionHeadingReference.eulerAngles.y : this.interactionEntryYaw, 0f);
			}
			return this.headBone.rotation * this.interactionHeadLocalBasis;
		}

		// Token: 0x06004832 RID: 18482 RVA: 0x001B7A68 File Offset: 0x001B5C68
		private Vector3 GetCenteredWallEyePosition(Vector3 eyes, float viewYaw)
		{
			Vector3 vector = ((this.leftBreastBaseBone != null && this.rightBreastBaseBone != null) ? ((this.leftBreastBaseBone.position + this.rightBreastBaseBone.position) * 0.5f) : ((this.positionReferenceBone != null) ? this.positionReferenceBone.position : eyes));
			Quaternion quaternion = Quaternion.Euler(0f, viewYaw, 0f);
			Vector3 vector2 = quaternion * Vector3.right;
			Vector3 vector3 = quaternion * Vector3.forward;
			float num = Mathf.Clamp(Vector3.Dot(vector - eyes, vector2), -Mathf.Max(0f, this.coverMaximumLateralCentering), Mathf.Max(0f, this.coverMaximumLateralCentering));
			float num2 = Mathf.Max(0f, this.coverEyeForwardOffset);
			if (this.leftBreastBaseBone != null && this.rightBreastBaseBone != null)
			{
				num2 = Mathf.Min(num2, Mathf.Max(0f, Vector3.Dot(vector - eyes, vector3)));
			}
			return eyes + vector2 * num + Vector3.up * this.coverEyeHeightOffset + vector3 * num2;
		}

		// Token: 0x06004833 RID: 18483 RVA: 0x001B7BB0 File Offset: 0x001B5DB0
		private void UpdateWallCoverCamera(Vector3 eyes)
		{
			if (!this.wallViewActive)
			{
				this.wallLookYaw = 0f;
				this.wallPeekBlend = 0f;
				this.wallPeekActive = false;
				this.wallPeekRotationBlendInitialized = false;
			}
			float wallViewYaw = this.GetWallViewYaw();
			this.previousWallViewYaw = wallViewYaw;
			Vector3 vector = this.GetCenteredWallEyePosition(eyes, wallViewYaw);
			this.fpMouseY = Mathf.Clamp(this.fpMouseY, -70f, 80f);
			Quaternion quaternion = Quaternion.Euler(this.fpMouseY, wallViewYaw + this.wallLookYaw, 0f);
			Vector3 vector2;
			Vector3 vector3;
			Vector3 vector4;
			bool flag = this.wallCover.TryGetFirstPersonPeekGeometry(out vector2, out vector3, out vector4);
			if (flag)
			{
				if (!this.wallPeekActive)
				{
					this.wallPeekLookYaw = (this.wallPeekLookPitch = 0f);
				}
				vector2.y = vector.y;
				this.wallPeekPosition = vector2 + vector4 * Mathf.Max(0.05f, this.coverPeekPastCorner) + vector3 * Mathf.Max(0.02f, this.coverPeekWallClearance);
				Vector3 vector5 = -vector3 + vector4 * 0.15f;
				float num = Mathf.Atan2(vector5.x, vector5.z) * 57.29578f;
				this.wallPeekRotation = Quaternion.Euler(this.wallPeekLookPitch, num + this.wallPeekLookYaw, 0f);
			}
			this.wallPeekBlend = Mathf.MoveTowards(this.wallPeekBlend, flag ? 1f : 0f, Time.unscaledDeltaTime / Mathf.Max(0.01f, this.coverPeekTransitionTime));
			if (this.wallPeekBlend > 0f)
			{
				float num2 = Mathf.SmoothStep(0f, 1f, this.wallPeekBlend);
				vector = Vector3.Lerp(vector, this.wallPeekPosition, num2);
				quaternion = this.BlendWallPeekRotation(quaternion, num2);
			}
			else
			{
				this.wallPeekRotationBlendInitialized = false;
			}
			this.wallPeekActive = flag;
			this.wallViewActive = true;
			this.fpMouseX = quaternion.eulerAngles.y;
			this.tpCamera.transform.SetPositionAndRotation(vector, quaternion);
		}

		// Token: 0x06004834 RID: 18484 RVA: 0x001B7DBC File Offset: 0x001B5FBC
		private Quaternion BlendWallPeekRotation(Quaternion neutral, float blend)
		{
			Vector3 eulerAngles = neutral.eulerAngles;
			Vector3 eulerAngles2 = this.wallPeekRotation.eulerAngles;
			float num = Mathf.DeltaAngle(eulerAngles.y, eulerAngles2.y);
			if (!this.wallPeekRotationBlendInitialized)
			{
				this.wallPeekYawDelta = num;
				this.wallPeekRotationBlendInitialized = true;
			}
			else
			{
				this.wallPeekYawDelta += Mathf.DeltaAngle(this.wallPeekYawDelta, num);
			}
			return Quaternion.Euler(Mathf.LerpAngle(eulerAngles.x, eulerAngles2.x, blend), eulerAngles.y + this.wallPeekYawDelta * blend, 0f);
		}

		// Token: 0x06004835 RID: 18485 RVA: 0x001B7E4B File Offset: 0x001B604B
		public bool ShouldBlockRoll()
		{
			return this.IsFirstPerson;
		}

		// Token: 0x06004836 RID: 18486 RVA: 0x001B7E54 File Offset: 0x001B6054
		public void FrameWorldPoint(Vector3 point)
		{
			if (!this.IsFirstPerson || this.mainCamera == null || this.IsCinematicViewSuspended || this.CameraAnchorOverride != null)
			{
				return;
			}
			Vector3 vector = point - this.mainCamera.transform.position;
			if (vector.sqrMagnitude < 1E-06f)
			{
				return;
			}
			if (this.interactionViewOwner != null && !this.IsInWallCover())
			{
				Vector3 eulerAngles = (Quaternion.Inverse(this.GetInteractionViewBasis()) * Quaternion.LookRotation(vector, Vector3.up)).eulerAngles;
				this.interactionLookYaw = Mathf.Clamp(Mathf.DeltaAngle(0f, eulerAngles.y), -this.interactionYawHalfRange, this.interactionYawHalfRange);
				this.fpMouseY = this.ClampInteractionPitch(Mathf.DeltaAngle(0f, eulerAngles.x));
				return;
			}
			FPCameraPreset fpcameraPreset;
			FPCameraPreset fpcameraPreset2;
			float num;
			this.GetViewPresets(out fpcameraPreset, out fpcameraPreset2, out num);
			Vector3 vector2 = Vector3.Lerp(fpcameraPreset.rotationOffset, fpcameraPreset2.rotationOffset, num);
			Vector3 eulerAngles2 = Quaternion.LookRotation(vector, Vector3.up).eulerAngles;
			this.fpMouseX = eulerAngles2.y - vector2.y;
			this.fpMouseY = Mathf.Clamp(Mathf.DeltaAngle(0f, eulerAngles2.x) - vector2.x, this.yMinLimit, this.yMaxLimit);
		}

		// Token: 0x06004837 RID: 18487 RVA: 0x001B7FB4 File Offset: 0x001B61B4
		public void RotateCameraFP(float x, float y)
		{
			if (GraphicsSettingsRuntimeUI.AnyOpen || Time.timeScale <= 0f || GameplayInputGate.BlocksGameplayInput)
			{
				return;
			}
			if (this.IsCinematicViewSuspended)
			{
				return;
			}
			if (this.cameraCalibration != null && this.cameraCalibration.IsOpen)
			{
				return;
			}
			if (this.SuppressMouseLookForPhotoView)
			{
				return;
			}
			if (x == 0f && y == 0f && this.tpInput != null && this.tpInput.lockCameraInput && vThirdPersonInput.ForceCameraLock)
			{
				return;
			}
			if (this.interactionViewOwner != null && !this.IsInWallCover())
			{
				this.interactionLookYaw = Mathf.Clamp(this.interactionLookYaw + x * this.fpMouseSensitivityX * GameSettingsService.LookSensitivity, -this.interactionYawHalfRange, this.interactionYawHalfRange);
				this.fpMouseY = this.ClampInteractionPitch(this.fpMouseY - y * this.fpMouseSensitivityY * GameSettingsService.LookSensitivity);
				return;
			}
			if (this.IsInWallCover())
			{
				float num = x * this.fpMouseSensitivityX * GameSettingsService.LookSensitivity;
				float num2 = y * this.fpMouseSensitivityY * GameSettingsService.LookSensitivity;
				Vector3 vector;
				Vector3 vector2;
				Vector3 vector3;
				if (this.wallCover.TryGetFirstPersonPeekGeometry(out vector, out vector2, out vector3))
				{
					this.wallPeekLookYaw = Mathf.Clamp(this.wallPeekLookYaw + num, -60f, 60f);
					this.wallPeekLookPitch = Mathf.Clamp(this.wallPeekLookPitch - num2, -60f, 70f);
					return;
				}
				this.wallLookYaw = Mathf.Clamp(this.wallLookYaw + num, -80f, 80f);
				this.fpMouseY = Mathf.Clamp(this.fpMouseY - num2, -70f, 80f);
				return;
			}
			else
			{
				if (this.YawClampOverrideCenter != null)
				{
					this.hideYawOffset = Mathf.Clamp(this.hideYawOffset + x * this.fpMouseSensitivityX * GameSettingsService.LookSensitivity, -this.hideSpotYawHalfRange, this.hideSpotYawHalfRange);
					this.hidePitch = Mathf.Clamp(this.hidePitch - y * this.fpMouseSensitivityY * GameSettingsService.LookSensitivity, this.hideSpotPitchMin, this.hideSpotPitchMax);
					return;
				}
				this.fpMouseX += x * this.fpMouseSensitivityX * GameSettingsService.LookSensitivity;
				this.fpMouseY -= y * this.fpMouseSensitivityY * GameSettingsService.LookSensitivity;
				if (this.IsRidingVehicle)
				{
					float vehicleForwardYaw = this.GetVehicleForwardYaw();
					float num3 = vehicleForwardYaw - this.vehicleYawLimit;
					float num4 = vehicleForwardYaw + this.vehicleYawLimit;
					this.fpMouseX = FirstPersonController.ClampAngle(this.fpMouseX, num3, num4);
					this.fpMouseY = Mathf.Clamp(this.fpMouseY, this.vehiclePitchMin, this.vehiclePitchMax);
					return;
				}
				this.fpMouseY = Mathf.Clamp(this.fpMouseY, this.yMinLimit, this.yMaxLimit);
				return;
			}
		}

		// Token: 0x06004838 RID: 18488 RVA: 0x001B8268 File Offset: 0x001B6468
		private void SetHeadRenderersToShadowsOnly()
		{
			this.headRendererStates.Clear();
			this.RefreshHeadRenderers(true);
			FirstPersonController.HeadVisibilityOwners.Add(this);
			RenderPipelineManager.beginCameraRendering -= this.OnBeginCameraRendering;
			RenderPipelineManager.endCameraRendering -= this.OnEndCameraRendering;
			RenderPipelineManager.beginCameraRendering += this.OnBeginCameraRendering;
			RenderPipelineManager.endCameraRendering += this.OnEndCameraRendering;
		}

		// Token: 0x06004839 RID: 18489 RVA: 0x001B82D8 File Offset: 0x001B64D8
		public static FirstPersonController.AuxiliaryHeadRenderingScope AllowHeadInAuxiliaryRendering()
		{
			if (FirstPersonController.auxiliaryHeadRenderDepth++ == 0)
			{
				foreach (FirstPersonController firstPersonController in FirstPersonController.HeadVisibilityOwners)
				{
					if (!(firstPersonController == null))
					{
						firstPersonController.auxiliaryHeadStates.Clear();
						foreach (KeyValuePair<Renderer, ShadowCastingMode> keyValuePair in firstPersonController.headRendererStates)
						{
							if (!(keyValuePair.Key == null))
							{
								firstPersonController.auxiliaryHeadStates[keyValuePair.Key] = keyValuePair.Key.shadowCastingMode;
								keyValuePair.Key.shadowCastingMode = keyValuePair.Value;
							}
						}
					}
				}
			}
			return new FirstPersonController.AuxiliaryHeadRenderingScope(true);
		}

		// Token: 0x0600483A RID: 18490 RVA: 0x001B83D0 File Offset: 0x001B65D0
		private void RefreshHeadRenderers(bool force = false)
		{
			if (!this.IsFirstPerson && !force)
			{
				return;
			}
			this.nextHeadRendererRefreshTime = Time.unscaledTime + Mathf.Max(0.05f, this.headRendererRefreshInterval);
			this.staleHeadRenderers.Clear();
			foreach (KeyValuePair<Renderer, ShadowCastingMode> keyValuePair in this.headRendererStates)
			{
				Renderer key = keyValuePair.Key;
				bool flag = this.additionalHeadRenderers != null && Array.IndexOf<Renderer>(this.additionalHeadRenderers, key) >= 0;
				if (key == null || (!flag && (!this.BelongsToPlayerHeadHierarchy(key) || !this.ShouldHideInFirstPerson(key))))
				{
					if (key != null)
					{
						key.shadowCastingMode = keyValuePair.Value;
					}
					this.staleHeadRenderers.Add(key);
				}
			}
			foreach (Renderer renderer in this.staleHeadRenderers)
			{
				this.headRendererStates.Remove(renderer);
			}
			foreach (Renderer renderer2 in base.GetComponentsInChildren<Renderer>(true))
			{
				if (renderer2 != null && this.ShouldHideInFirstPerson(renderer2))
				{
					this.TrackAndHideRenderer(renderer2);
				}
			}
			if (this.headBone != null && !this.headBone.IsChildOf(base.transform))
			{
				foreach (Renderer renderer3 in this.headBone.GetComponentsInChildren<Renderer>(true))
				{
					if (renderer3 != null && this.ShouldHideInFirstPerson(renderer3))
					{
						this.TrackAndHideRenderer(renderer3);
					}
				}
			}
			if (this.additionalHeadRenderers != null)
			{
				for (int k = 0; k < this.additionalHeadRenderers.Length; k++)
				{
					this.TrackAndHideRenderer(this.additionalHeadRenderers[k]);
				}
			}
			this.ApplyTrackedHeadVisibility();
		}

		// Token: 0x0600483B RID: 18491 RVA: 0x001B85D8 File Offset: 0x001B67D8
		private bool BelongsToPlayerHeadHierarchy(Renderer renderer)
		{
			return renderer.transform.IsChildOf(base.transform) || (this.headBone != null && renderer.transform.IsChildOf(this.headBone));
		}

		// Token: 0x0600483C RID: 18492 RVA: 0x001B8610 File Offset: 0x001B6810
		private bool ShouldShowHeadForCurrentView()
		{
			if (this.CinematicCameraOwnsView())
			{
				return true;
			}
			if (this.CameraAnchorOverride == null || this.headBone == null)
			{
				return false;
			}
			float num = Mathf.Max(0.01f, this.externalCameraHeadVisibilityDistance);
			return (this.CameraAnchorOverride.position - this.headBone.position).sqrMagnitude >= num * num;
		}

		// Token: 0x0600483D RID: 18493 RVA: 0x001B8684 File Offset: 0x001B6884
		private void ApplyTrackedHeadVisibility()
		{
			foreach (KeyValuePair<Renderer, ShadowCastingMode> keyValuePair in this.headRendererStates)
			{
				if (keyValuePair.Key != null)
				{
					keyValuePair.Key.shadowCastingMode = (this.headVisibleForCurrentView ? keyValuePair.Value : ShadowCastingMode.ShadowsOnly);
				}
			}
		}

		// Token: 0x0600483E RID: 18494 RVA: 0x001B8700 File Offset: 0x001B6900
		private void TrackAndHideRenderer(Renderer renderer)
		{
			if (renderer == null)
			{
				return;
			}
			if (!this.headRendererStates.ContainsKey(renderer))
			{
				this.headRendererStates.Add(renderer, renderer.shadowCastingMode);
			}
			renderer.shadowCastingMode = (this.headVisibleForCurrentView ? this.headRendererStates[renderer] : ShadowCastingMode.ShadowsOnly);
		}

		// Token: 0x0600483F RID: 18495 RVA: 0x001B8754 File Offset: 0x001B6954
		private bool ShouldHideInFirstPerson(Renderer renderer)
		{
			if (renderer.GetComponentInParent<FirstPersonVisibleProp>(true) != null)
			{
				return false;
			}
			int num = 1 << renderer.gameObject.layer;
			if ((this.headLayer.value & num) != 0)
			{
				return true;
			}
			Transform transform = renderer.transform;
			int num2 = 0;
			while (transform != null && transform != base.transform && num2 < 4)
			{
				if (FirstPersonController.ContainsHeadSemantic(transform.name))
				{
					return true;
				}
				num2++;
				transform = transform.parent;
			}
			Material[] sharedMaterials = renderer.sharedMaterials;
			for (int i = 0; i < sharedMaterials.Length; i++)
			{
				if (sharedMaterials[i] != null && FirstPersonController.ContainsHeadSemantic(sharedMaterials[i].name))
				{
					return true;
				}
			}
			SkinnedMeshRenderer skinnedMeshRenderer = renderer as SkinnedMeshRenderer;
			return skinnedMeshRenderer != null && skinnedMeshRenderer.sharedMesh != null && FirstPersonController.ContainsHeadSemantic(skinnedMeshRenderer.sharedMesh.name);
		}

		// Token: 0x06004840 RID: 18496 RVA: 0x001B883C File Offset: 0x001B6A3C
		private static bool ContainsHeadSemantic(string value)
		{
			return !string.IsNullOrEmpty(value) && (value.IndexOf("head", StringComparison.OrdinalIgnoreCase) >= 0 || value.IndexOf("face", StringComparison.OrdinalIgnoreCase) >= 0 || value.IndexOf("hair", StringComparison.OrdinalIgnoreCase) >= 0 || value.IndexOf("eye", StringComparison.OrdinalIgnoreCase) >= 0 || value.IndexOf("brow", StringComparison.OrdinalIgnoreCase) >= 0 || value.IndexOf("lash", StringComparison.OrdinalIgnoreCase) >= 0 || value.IndexOf("glasses", StringComparison.OrdinalIgnoreCase) >= 0 || value.IndexOf("spectacle", StringComparison.OrdinalIgnoreCase) >= 0 || value.IndexOf("headpin", StringComparison.OrdinalIgnoreCase) >= 0 || value.IndexOf("earring", StringComparison.OrdinalIgnoreCase) >= 0 || value.IndexOf("mask", StringComparison.OrdinalIgnoreCase) >= 0);
		}

		// Token: 0x06004841 RID: 18497 RVA: 0x001B8904 File Offset: 0x001B6B04
		private void RestoreHeadRenderers()
		{
			FirstPersonController.HeadVisibilityOwners.Remove(this);
			this.auxiliaryHeadStates.Clear();
			RenderPipelineManager.beginCameraRendering -= this.OnBeginCameraRendering;
			RenderPipelineManager.endCameraRendering -= this.OnEndCameraRendering;
			foreach (KeyValuePair<Renderer, ShadowCastingMode> keyValuePair in this.headRendererStates)
			{
				if (keyValuePair.Key != null)
				{
					keyValuePair.Key.shadowCastingMode = keyValuePair.Value;
				}
			}
			this.headRendererStates.Clear();
		}

		// Token: 0x06004842 RID: 18498 RVA: 0x001B89B8 File Offset: 0x001B6BB8
		private void OnBeginCameraRendering(ScriptableRenderContext context, Camera cam)
		{
			if (cam == this.mainCamera)
			{
				this.headVisibleForCurrentView = this.ShouldShowHeadForCurrentView();
				this.ApplyTrackedHeadVisibility();
				return;
			}
			if (!this.IsExemptCamera(cam))
			{
				return;
			}
			if (this.IsRegisteredExternalCamera(cam) && this.headBone != null)
			{
				float num = Mathf.Max(0.01f, this.externalCameraHeadVisibilityDistance);
				if ((cam.transform.position - this.headBone.position).sqrMagnitude < num * num)
				{
					foreach (KeyValuePair<Renderer, ShadowCastingMode> keyValuePair in this.headRendererStates)
					{
						if (keyValuePair.Key != null)
						{
							keyValuePair.Key.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
						}
					}
					return;
				}
			}
			foreach (KeyValuePair<Renderer, ShadowCastingMode> keyValuePair2 in this.headRendererStates)
			{
				if (keyValuePair2.Key != null)
				{
					keyValuePair2.Key.shadowCastingMode = keyValuePair2.Value;
				}
			}
		}

		// Token: 0x06004843 RID: 18499 RVA: 0x001B8B00 File Offset: 0x001B6D00
		private void OnEndCameraRendering(ScriptableRenderContext context, Camera cam)
		{
			if (!this.IsExemptCamera(cam))
			{
				return;
			}
			this.ApplyTrackedHeadVisibility();
		}

		// Token: 0x06004844 RID: 18500 RVA: 0x001B8B14 File Offset: 0x001B6D14
		private bool IsExemptCamera(Camera cam)
		{
			if (this.IsRegisteredExternalCamera(cam))
			{
				return true;
			}
			if (this.headVisibleCameras == null)
			{
				return false;
			}
			for (int i = 0; i < this.headVisibleCameras.Length; i++)
			{
				if (this.headVisibleCameras[i] == cam)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004845 RID: 18501 RVA: 0x001B8B5C File Offset: 0x001B6D5C
		private bool IsRegisteredExternalCamera(Camera cam)
		{
			return cam != null && (this.registeredHeadVisibleCameras.Contains(cam) || FlyCameraPhotoMode.IsPhotoCamera(cam));
		}

		// Token: 0x06004846 RID: 18502 RVA: 0x001B8B80 File Offset: 0x001B6D80
		private float GetVehicleForwardYaw()
		{
			if (this.cc == null)
			{
				return this.fpMouseX;
			}
			return ((this.cc.transform.parent != null) ? this.cc.transform.parent : this.cc.transform).eulerAngles.y;
		}

		// Token: 0x06004847 RID: 18503 RVA: 0x001B8BE4 File Offset: 0x001B6DE4
		private static float ClampAngle(float angle, float min, float max)
		{
			float num = Mathf.DeltaAngle(0f, angle - (min + max) * 0.5f);
			float num2 = (max - min) * 0.5f;
			num = Mathf.Clamp(num, -num2, num2);
			return (min + max) * 0.5f + num;
		}

		// Token: 0x040047E5 RID: 18405
		[Header("First Person Settings")]
		[Tooltip("Optional layer assigned to head objects. The modular face/hair are also detected automatically when they remain on Player.")]
		public LayerMask headLayer;

		// Token: 0x040047E6 RID: 18406
		[Tooltip("Name of the CameraState to use in First Person mode")]
		public string firstPersonStateName = "FirstPerson";

		// Token: 0x040047E7 RID: 18407
		[Tooltip("Fallback position reference used only when the humanoid eye bones are unavailable.")]
		public Transform positionReferenceBone;

		// Token: 0x040047E8 RID: 18408
		[Header("Camera Presets")]
		[Tooltip("Camera preset for Confident state (default/clothed/exhibition dominant)")]
		public FPCameraPreset confidentPreset = new FPCameraPreset();

		// Token: 0x040047E9 RID: 18409
		[Tooltip("Camera preset for Embarrassed state (shame dominant)")]
		public FPCameraPreset embarrassedPreset = new FPCameraPreset();

		// Token: 0x040047EA RID: 18410
		[Tooltip("Shared camera preset while carrying an item, independent of Confident or Embarrassed stance.")]
		public FPCameraPreset carriedItemPreset = new FPCameraPreset();

		// Token: 0x040047EB RID: 18411
		[Tooltip("How fast the camera blends between presets")]
		[Range(1f, 20f)]
		public float presetBlendSpeed = 8f;

		// Token: 0x040047EC RID: 18412
		[Header("Head Bob Damping")]
		[Tooltip("How much to dampen vertical head movement from animations (0 = no damping, 1 = fully locked)")]
		[Range(0f, 1f)]
		public float verticalDamping = 0.5f;

		// Token: 0x040047ED RID: 18413
		[Tooltip("Maximum distance in metres that bob damping may move the camera away from the current eye pose.")]
		[Min(0f)]
		public float maximumVerticalLag = 0.025f;

		// Token: 0x040047EE RID: 18414
		[Header("Mouse Sensitivity")]
		[Tooltip("Horizontal mouse sensitivity in FP mode")]
		public float fpMouseSensitivityX = 3f;

		// Token: 0x040047EF RID: 18415
		[Tooltip("Vertical mouse sensitivity in FP mode")]
		public float fpMouseSensitivityY = 3f;

		// Token: 0x040047F0 RID: 18416
		[Tooltip("Vertical look limit (degrees)")]
		public float yMinLimit = -80f;

		// Token: 0x040047F1 RID: 18417
		public float yMaxLimit = 80f;

		// Token: 0x040047F2 RID: 18418
		[Header("Vehicle Camera Limits")]
		[Tooltip("Max horizontal look angle (degrees) relative to vehicle forward when riding")]
		[Range(45f, 170f)]
		public float vehicleYawLimit = 120f;

		// Token: 0x040047F3 RID: 18419
		[Tooltip("Min vertical look angle when riding a vehicle")]
		public float vehiclePitchMin = -40f;

		// Token: 0x040047F4 RID: 18420
		[Tooltip("Max vertical look angle when riding a vehicle")]
		public float vehiclePitchMax = 50f;

		// Token: 0x040047F5 RID: 18421
		[Tooltip("Maximum camera distance in front of the animated chest while mounted. Limits acceleration poses that push the head past the body.")]
		[Min(0f)]
		public float maximumMountedCameraForward = 0.17f;

		// Token: 0x040047F6 RID: 18422
		[Tooltip("Minimum camera height above the animated chest while mounted, keeping the view outside the neck as the rider leans forward.")]
		[Min(0f)]
		public float minimumMountedCameraHeight = 0.16f;

		// Token: 0x040047F7 RID: 18423
		[Header("Wall Cover First Person")]
		[Tooltip("Small height correction from the eyes while against a wall.")]
		public float coverEyeHeightOffset = -0.06f;

		// Token: 0x040047F8 RID: 18424
		[Tooltip("Maximum forward correction along the torso while against a wall, stopping at the chest to preserve crouched framing.")]
		[Min(0f)]
		public float coverEyeForwardOffset = 0.12f;

		// Token: 0x040047F9 RID: 18425
		[Tooltip("Maximum sideways correction that centers the view over the animated chest.")]
		[Min(0f)]
		public float coverMaximumLateralCentering = 0.15f;

		// Token: 0x040047FA RID: 18426
		[Tooltip("Distance beyond the physical corner while peeking, keeping the wall out of the centre of the view.")]
		[Min(0.05f)]
		public float coverPeekPastCorner = 0.22f;

		// Token: 0x040047FB RID: 18427
		[Tooltip("Clearance from the front face of the wall during a peek.")]
		[Min(0.02f)]
		public float coverPeekWallClearance = 0.1f;

		// Token: 0x040047FC RID: 18428
		[Min(0.01f)]
		public float coverPeekTransitionTime = 0.22f;

		// Token: 0x040047FD RID: 18429
		[Header("Head Visibility Exceptions")]
		[Tooltip("Cameras that should still render the head normally in FP mode (e.g. PlayerFaceCam).")]
		public Camera[] headVisibleCameras;

		// Token: 0x040047FE RID: 18430
		[Tooltip("Optional extra renderers to hide. Normally the controller detects face, eyes, hair and head accessories automatically.")]
		public Renderer[] additionalHeadRenderers;

		// Token: 0x040047FF RID: 18431
		[Tooltip("How often to discover head/hair renderers spawned by runtime customization while FP is active.")]
		[Min(0.05f)]
		public float headRendererRefreshInterval = 0.25f;

		// Token: 0x04004800 RID: 18432
		[Tooltip("Minimum distance between the head bone and an external camera anchor before the head becomes visible. This keeps the mesh hidden at the start of a transition from the eye position, then restores it for phone photo/video and tripod framing.")]
		[Min(0.01f)]
		public float externalCameraHeadVisibilityDistance = 0.15f;

		// Token: 0x04004801 RID: 18433
		[Header("Near Clip Plane")]
		[Tooltip("Near clip plane value when in First Person mode")]
		public float fpNearClipPlane = 0.01f;

		// Token: 0x04004802 RID: 18434
		[Header("Hide-spot FP (YawClampOverride active)")]
		[Tooltip("Metres along the FACE direction from the eye midpoint. Positive = toward the door, negative = back into the head. Slightly back + low reads as an over-face shot that frames both her body below and the door in front.")]
		public float hideSpotForwardOffset = 0.02f;

		// Token: 0x04004803 RID: 18435
		[Tooltip("Vertical offset (metres) from the eye midpoint. Negative = lower.")]
		public float hideSpotVerticalOffset = -0.08f;

		// Token: 0x04004804 RID: 18436
		[Tooltip("Hide-spot pitch limit toward LEVEL (positive = down, 0 = level). 25 gives a near-forward view of the door front; the camera doesn't move with the look, so a shallow pitch can't push it through the door.")]
		public float hideSpotPitchMin = 25f;

		// Token: 0x04004805 RID: 18437
		[Tooltip("Hide-spot pitch limit looking DOWN (positive = down). Keep < 90 (Euler flip).")]
		public float hideSpotPitchMax = 89f;

		// Token: 0x04004806 RID: 18438
		[Tooltip("Yaw half-range (degrees) around the face direction. Tiny on purpose — the shot is fixed on her body, just a slight look-around.")]
		public float hideSpotYawHalfRange = 10f;

		// Token: 0x04004807 RID: 18439
		[Tooltip("Pitch the shot OPENS on when entering FP while hidden (the down-at-her-body framing); from there the mouse can rise to hideSpotPitchMin.")]
		public float hideSpotEntryPitch = 80f;

		// Token: 0x04004808 RID: 18440
		[Header("Events")]
		public UnityEvent OnEnterFirstPerson;

		// Token: 0x04004809 RID: 18441
		public UnityEvent OnExitFirstPerson;

		// Token: 0x04004811 RID: 18449
		private vThirdPersonCamera tpCamera;

		// Token: 0x04004812 RID: 18450
		private vThirdPersonInput tpInput;

		// Token: 0x04004813 RID: 18451
		private vThirdPersonController cc;

		// Token: 0x04004814 RID: 18452
		private Camera mainCamera;

		// Token: 0x04004815 RID: 18453
		private CinemachineBrain cinematicBrain;

		// Token: 0x04004816 RID: 18454
		private float firstPersonFieldOfView = 60f;

		// Token: 0x04004817 RID: 18455
		private CielRagdollRecovery ragdollRecovery;

		// Token: 0x04004818 RID: 18456
		private vBasicCoverController wallCover;

		// Token: 0x04004819 RID: 18457
		private bool wallViewActive;

		// Token: 0x0400481A RID: 18458
		private float previousWallViewYaw;

		// Token: 0x0400481B RID: 18459
		private float wallLookYaw;

		// Token: 0x0400481C RID: 18460
		private float wallPeekLookYaw;

		// Token: 0x0400481D RID: 18461
		private float wallPeekLookPitch;

		// Token: 0x0400481E RID: 18462
		private float wallPeekBlend;

		// Token: 0x0400481F RID: 18463
		private bool wallPeekActive;

		// Token: 0x04004820 RID: 18464
		private Vector3 wallPeekPosition;

		// Token: 0x04004821 RID: 18465
		private Quaternion wallPeekRotation;

		// Token: 0x04004822 RID: 18466
		private float wallPeekYawDelta;

		// Token: 0x04004823 RID: 18467
		private bool wallPeekRotationBlendInitialized;

		// Token: 0x04004824 RID: 18468
		private Object interactionViewOwner;

		// Token: 0x04004825 RID: 18469
		private bool interactionUsesPoseDirection;

		// Token: 0x04004826 RID: 18470
		private bool interactionHasInvertedPose;

		// Token: 0x04004827 RID: 18471
		private bool interactionKeepsHorizonLevel;

		// Token: 0x04004828 RID: 18472
		private float interactionEntryYaw;

		// Token: 0x04004829 RID: 18473
		private float interactionLookYaw;

		// Token: 0x0400482A RID: 18474
		private float interactionYawHalfRange;

		// Token: 0x0400482B RID: 18475
		private Transform interactionHeadingReference;

		// Token: 0x0400482C RID: 18476
		private Vector3 interactionEyeOffset;

		// Token: 0x0400482D RID: 18477
		private Vector2? interactionPitchLimits;

		// Token: 0x0400482E RID: 18478
		private Quaternion interactionHeadLocalBasis = Quaternion.identity;

		// Token: 0x0400482F RID: 18479
		private readonly HashSet<Camera> registeredHeadVisibleCameras = new HashSet<Camera>();

		// Token: 0x04004830 RID: 18480
		private readonly HashSet<Object> firstPersonSuspensionOwners = new HashSet<Object>();

		// Token: 0x04004831 RID: 18481
		private bool restoreFirstPersonAfterInteraction;

		// Token: 0x04004832 RID: 18482
		private Animator animator;

		// Token: 0x04004833 RID: 18483
		private PlayerEmotionController emotionController;

		// Token: 0x04004834 RID: 18484
		private PlayerInventoryManager cameraInventory;

		// Token: 0x04004835 RID: 18485
		private GameplayEmoteController emoteController;

		// Token: 0x04004836 RID: 18486
		private Transform headBone;

		// Token: 0x04004837 RID: 18487
		private SphereCollider ragdollHeadCollider;

		// Token: 0x04004838 RID: 18488
		private Transform leftEyeBone;

		// Token: 0x04004839 RID: 18489
		private Transform rightEyeBone;

		// Token: 0x0400483A RID: 18490
		private Transform leftBreastBaseBone;

		// Token: 0x0400483B RID: 18491
		private Transform rightBreastBaseBone;

		// Token: 0x0400483C RID: 18492
		private string savedCameraState;

		// Token: 0x0400483D RID: 18493
		private float savedNearClipPlane;

		// Token: 0x0400483E RID: 18494
		private readonly Dictionary<Renderer, ShadowCastingMode> headRendererStates = new Dictionary<Renderer, ShadowCastingMode>();

		// Token: 0x0400483F RID: 18495
		private readonly List<Renderer> staleHeadRenderers = new List<Renderer>();

		// Token: 0x04004840 RID: 18496
		private float nextHeadRendererRefreshTime;

		// Token: 0x04004841 RID: 18497
		private bool headVisibleForCurrentView;

		// Token: 0x04004842 RID: 18498
		private static readonly HashSet<FirstPersonController> HeadVisibilityOwners = new HashSet<FirstPersonController>();

		// Token: 0x04004843 RID: 18499
		private static int auxiliaryHeadRenderDepth;

		// Token: 0x04004844 RID: 18500
		private readonly Dictionary<Renderer, ShadowCastingMode> auxiliaryHeadStates = new Dictionary<Renderer, ShadowCastingMode>();

		// Token: 0x04004845 RID: 18501
		private float currentPresetBlend;

		// Token: 0x04004846 RID: 18502
		private float targetPresetBlend;

		// Token: 0x04004847 RID: 18503
		private float smoothedEyeLocalY;

		// Token: 0x04004848 RID: 18504
		private bool headYInitialized;

		// Token: 0x04004849 RID: 18505
		private float fpMouseX;

		// Token: 0x0400484A RID: 18506
		private float fpMouseY;

		// Token: 0x0400484B RID: 18507
		private Quaternion ragdollHeadLocalYawBasis = Quaternion.identity;

		// Token: 0x0400484C RID: 18508
		private float ragdollLookReferenceYaw;

		// Token: 0x0400484D RID: 18509
		private bool ragdollLookBasisValid;

		// Token: 0x0400484E RID: 18510
		private bool ragdollRotationWasRecovering;

		// Token: 0x0400484F RID: 18511
		private Quaternion ragdollRotationCorrection = Quaternion.identity;

		// Token: 0x04004850 RID: 18512
		private float ragdollRotationElapsed = 1f;

		// Token: 0x04004851 RID: 18513
		private Quaternion lastEyeRotation = Quaternion.identity;

		// Token: 0x04004852 RID: 18514
		private bool eyeRotationInitialized;

		// Token: 0x04004853 RID: 18515
		private float hideYawOffset;

		// Token: 0x04004854 RID: 18516
		private float hidePitch;

		// Token: 0x04004855 RID: 18517
		private Vector3 savedMainCamLocalPos;

		// Token: 0x04004856 RID: 18518
		private Quaternion savedMainCamLocalRotation;

		// Token: 0x04004857 RID: 18519
		private bool mainCamLocalZeroed;

		// Token: 0x04004858 RID: 18520
		private float suspendedYawOffset;

		// Token: 0x04004859 RID: 18521
		private FirstPersonCameraCalibration cameraCalibration;

		// Token: 0x020009EF RID: 2543
		public struct AuxiliaryHeadRenderingScope : IDisposable
		{
			// Token: 0x0600484A RID: 18506 RVA: 0x001B8E27 File Offset: 0x001B7027
			internal AuxiliaryHeadRenderingScope(bool value)
			{
				this.active = value;
			}

			// Token: 0x0600484B RID: 18507 RVA: 0x001B8E30 File Offset: 0x001B7030
			public void Dispose()
			{
				if (!this.active)
				{
					return;
				}
				this.active = false;
				if (--FirstPersonController.auxiliaryHeadRenderDepth != 0)
				{
					return;
				}
				foreach (FirstPersonController firstPersonController in FirstPersonController.HeadVisibilityOwners)
				{
					if (!(firstPersonController == null))
					{
						foreach (KeyValuePair<Renderer, ShadowCastingMode> keyValuePair in firstPersonController.auxiliaryHeadStates)
						{
							if (keyValuePair.Key != null)
							{
								keyValuePair.Key.shadowCastingMode = keyValuePair.Value;
							}
						}
						firstPersonController.auxiliaryHeadStates.Clear();
					}
				}
			}

			// Token: 0x0400485B RID: 18523
			private bool active;
		}
	}
}
