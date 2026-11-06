using System;
using System.Collections;
using System.Collections.Generic;
using Features.DebugTools;
using Features.Detection;
using Features.PlayerEmotes;
using Features.Score;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.FirstPerson;
using ProjectExhibitionist.Interactions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

namespace Features.LockerHide
{
	// Token: 0x020010B4 RID: 4276
	[DefaultExecutionOrder(900)]
	[DisallowMultipleComponent]
	public class LockerHideInteractable : MonoBehaviour
	{
		// Token: 0x06008180 RID: 33152 RVA: 0x0034E874 File Offset: 0x0034CA74
		internal Object ReadUniversityGameplayReference(UniversityGameplayField field)
		{
			switch (field)
			{
			case UniversityGameplayField.LockerHideInteractable_doorCloseSound:
				return this.doorCloseSound;
			case UniversityGameplayField.LockerHideInteractable_doorOpenSound:
				return this.doorOpenSound;
			case UniversityGameplayField.LockerHideInteractable_enterConfig:
				return this.enterConfig;
			case UniversityGameplayField.LockerHideInteractable_hiddenHudConfig:
				return this.hiddenHudConfig;
			case UniversityGameplayField.LockerHideInteractable_lockerEnterAnimation:
				return this.lockerEnterAnimation;
			case UniversityGameplayField.LockerHideInteractable_lockerExitAnimation:
				return this.lockerExitAnimation;
			case UniversityGameplayField.LockerHideInteractable_lockerIdleAnimation:
				return this.lockerIdleAnimation;
			case UniversityGameplayField.LockerHideInteractable_peeAudioClip:
				return this.peeAudioClip;
			case UniversityGameplayField.LockerHideInteractable_peeEffectPrefab:
				return this.peeEffectPrefab;
			default:
				throw new InvalidOperationException("Unsupported LockerHideInteractable gameplay binding: " + field.ToString());
			}
		}

		// Token: 0x06008181 RID: 33153 RVA: 0x0034E90C File Offset: 0x0034CB0C
		internal void WriteUniversityGameplayReference(UniversityGameplayField field, Object asset)
		{
			switch (field)
			{
			case UniversityGameplayField.LockerHideInteractable_doorCloseSound:
				this.doorCloseSound = (AudioClip)asset;
				return;
			case UniversityGameplayField.LockerHideInteractable_doorOpenSound:
				this.doorOpenSound = (AudioClip)asset;
				return;
			case UniversityGameplayField.LockerHideInteractable_enterConfig:
				this.enterConfig = (InteractionConfigSO)asset;
				return;
			case UniversityGameplayField.LockerHideInteractable_hiddenHudConfig:
				this.hiddenHudConfig = (InteractionConfigSO)asset;
				return;
			case UniversityGameplayField.LockerHideInteractable_lockerEnterAnimation:
				this.lockerEnterAnimation = (AnimationClip)asset;
				return;
			case UniversityGameplayField.LockerHideInteractable_lockerExitAnimation:
				this.lockerExitAnimation = (AnimationClip)asset;
				return;
			case UniversityGameplayField.LockerHideInteractable_lockerIdleAnimation:
				this.lockerIdleAnimation = (AnimationClip)asset;
				return;
			case UniversityGameplayField.LockerHideInteractable_peeAudioClip:
				this.peeAudioClip = (AudioClip)asset;
				return;
			case UniversityGameplayField.LockerHideInteractable_peeEffectPrefab:
				this.peeEffectPrefab = (GameObject)asset;
				return;
			default:
				throw new InvalidOperationException("Unsupported LockerHideInteractable gameplay binding: " + field.ToString());
			}
		}

		// Token: 0x17001493 RID: 5267
		// (get) Token: 0x06008182 RID: 33154 RVA: 0x0034E9D9 File Offset: 0x0034CBD9
		public bool HasActiveResidencyInteraction
		{
			get
			{
				return this._hidden || this._exiting || this._entryPending || this._controlsLocked;
			}
		}

		// Token: 0x06008183 RID: 33155 RVA: 0x0034E9FC File Offset: 0x0034CBFC
		private void Awake()
		{
			ControllerContextBindings.SetLabel(this.hiddenHudConfig, "E", "B", false);
			ControllerContextBindings.SetLabel(this.hiddenHudConfig, "G", "R3", false);
			ControllerContextBindings.SetLabel(this.hiddenHudConfig, "H", "Y", false);
			ControllerContextBindings.Set(this.exitInput, "B");
			ControllerContextBindings.Set(this.xrayInput, "RightStickClick");
			ControllerContextBindings.Set(this.peeInput, "Y");
			if (this.triggers == null || this.triggers.Length == 0)
			{
				this.triggers = base.GetComponentsInChildren<vTriggerGenericAction>(true);
			}
			if (this.promptUI == null)
			{
				this.promptUI = base.GetComponentInChildren<InteractablePromptUI>(true);
			}
			if (this.promptUI != null)
			{
				this.promptUI.MatchVehiclePlacement(this.LockerVisualRoot, 0.0055f);
			}
			if (this.triggers == null || this.triggers.Length == 0)
			{
				Debug.LogError("[LockerHide] No vTriggerGenericAction in the hierarchy of '" + base.name + "'.", this);
			}
			if (this.promptUI != null && this.enterConfig != null)
			{
				this.promptUI.Show(this.enterConfig);
				this.promptUI.gameObject.SetActive(false);
			}
			if (this.HasAuthoredLockerAnimations)
			{
				this.lockerIdleAnimation.SampleAnimation(this.lockerAnimationRoot.gameObject, 0f);
			}
			this.InitializeRendererCulling();
			if (this.lockerDoor != null)
			{
				this._doorClosedRootLocalPos = base.transform.InverseTransformPoint(this.lockerDoor.position);
				this._doorClosedRootLocalRot = Quaternion.Inverse(base.transform.rotation) * this.lockerDoor.rotation;
				this._authoredDoorClosedLocalRotation = this.lockerDoor.localRotation;
				this._doorAngle = 0f;
			}
			this._interactionCandidate = TriggerInteractionCandidate.Attach(base.gameObject, this.triggers, this.promptUI, () => !this._hidden && !this._exiting && !FlyCamera.IsActive && FullBodyHideActivityContext.CanAcquireWithTripodCamera, this.enterConfig);
		}

		// Token: 0x06008184 RID: 33156 RVA: 0x0034EC07 File Offset: 0x0034CE07
		private void Start()
		{
			if (this.promptUI != null)
			{
				this.promptUI.MatchVehiclePlacement(this.LockerVisualRoot, 0.0055f);
			}
		}

		// Token: 0x17001494 RID: 5268
		// (get) Token: 0x06008185 RID: 33157 RVA: 0x0034EC30 File Offset: 0x0034CE30
		private string HideGateReason
		{
			get
			{
				return "LockerHide#" + base.GetInstanceID().ToString();
			}
		}

		// Token: 0x06008186 RID: 33158 RVA: 0x0034EC55 File Offset: 0x0034CE55
		private bool TryAcquireHideContext()
		{
			return FullBodyHideActivityContext.TryAcquire(this.HideGateReason, this, ref this._hideLease, PlayerActivity.AllPlayerActions, true, true);
		}

		// Token: 0x06008187 RID: 33159 RVA: 0x0034EC70 File Offset: 0x0034CE70
		private bool SetHiddenFreeCameraAvailable(bool available)
		{
			return FullBodyHideActivityContext.SetPhotoModeAvailable(this._hideLease, available);
		}

		// Token: 0x06008188 RID: 33160 RVA: 0x0034EC7E File Offset: 0x0034CE7E
		private void ReleaseHideContext()
		{
			this._entryPending = false;
			FullBodyHideActivityContext.Release(ref this._hideLease);
		}

		// Token: 0x06008189 RID: 33161 RVA: 0x0034EC92 File Offset: 0x0034CE92
		private void RejectEntryAfterInvectorCommit()
		{
			this._entryRejected = true;
			if (this._rejectedEntryCleanup == null && base.isActiveAndEnabled)
			{
				this._rejectedEntryCleanup = base.StartCoroutine(this.CancelRejectedEntryNextFrame());
			}
		}

		// Token: 0x0600818A RID: 33162 RVA: 0x0034ECBD File Offset: 0x0034CEBD
		private IEnumerator CancelRejectedEntryNextFrame()
		{
			yield return null;
			this._rejectedEntryCleanup = null;
			if (!this._entryRejected)
			{
				yield break;
			}
			if (this._actionAnimator != null)
			{
				this._actionAnimator.CancelCurrentAction();
			}
			else if (this._playerGenericAction != null)
			{
				this._playerGenericAction.CancelAction();
				if (this._playerGenericAction.doingAction)
				{
					this._playerGenericAction.ResetTriggerSettings(true);
				}
			}
			this.LockControls(false);
			this._entryRejected = false;
			yield break;
		}

		// Token: 0x0600818B RID: 33163 RVA: 0x0034ECCC File Offset: 0x0034CECC
		private void OnEnable()
		{
			ContainerFloorAlignment.Request(base.gameObject);
			if (this._interactionCandidate != null)
			{
				this._interactionCandidate.enabled = true;
			}
			if (this.triggers == null)
			{
				return;
			}
			foreach (vTriggerGenericAction vTriggerGenericAction in this.triggers)
			{
				if (!(vTriggerGenericAction == null))
				{
					vTriggerGenericAction.OnValidate.AddListener(new UnityAction<GameObject>(this.HandleValidate));
					vTriggerGenericAction.OnInvalidate.AddListener(new UnityAction<GameObject>(this.HandleInvalidate));
					vTriggerGenericAction.OnPlayerExit.AddListener(new UnityAction<GameObject>(this.HandleInvalidate));
					vTriggerGenericAction.OnStartAnimation.AddListener(new UnityAction(this.HandleAnimationStart));
					vTriggerGenericAction.OnCancelAction.AddListener(new UnityAction(this.HandleCancelled));
				}
			}
		}

		// Token: 0x0600818C RID: 33164 RVA: 0x0034EDA4 File Offset: 0x0034CFA4
		private void OnDisable()
		{
			FlyCamera.StopForActivityLease(this._hideLease);
			this.StopPairedPose();
			this.StopExitRoutine();
			this._entryPoseCaptured = false;
			if (this._interactionCandidate != null)
			{
				this._interactionCandidate.enabled = false;
			}
			this._entryRejected = false;
			if (this._rejectedEntryCleanup != null)
			{
				base.StopCoroutine(this._rejectedEntryCleanup);
				this._rejectedEntryCleanup = null;
			}
			if (this.triggers != null)
			{
				foreach (vTriggerGenericAction vTriggerGenericAction in this.triggers)
				{
					if (!(vTriggerGenericAction == null))
					{
						vTriggerGenericAction.OnValidate.RemoveListener(new UnityAction<GameObject>(this.HandleValidate));
						vTriggerGenericAction.OnInvalidate.RemoveListener(new UnityAction<GameObject>(this.HandleInvalidate));
						vTriggerGenericAction.OnPlayerExit.RemoveListener(new UnityAction<GameObject>(this.HandleInvalidate));
						vTriggerGenericAction.OnStartAnimation.RemoveListener(new UnityAction(this.HandleAnimationStart));
						vTriggerGenericAction.OnCancelAction.RemoveListener(new UnityAction(this.HandleCancelled));
					}
				}
			}
			if (this._hideLease != null && this._playerGenericAction != null && this._playerGenericAction.doingAction && this.IsOwnedTrigger(this._playerGenericAction.triggerAction))
			{
				this._playerGenericAction.CancelAction();
			}
			if (this._genericActionSubscribed && this._playerGenericAction != null)
			{
				this._playerGenericAction.OnStartAction.RemoveListener(new UnityAction<vTriggerGenericAction>(this.HandlePlayerActionStart));
				this._genericActionSubscribed = false;
			}
			if (this._hidden || this._rootMotionPushed || this._emotionPushed)
			{
				this.RestorePlayerAfterExit();
			}
			this.LockControls(false);
			PlayerStealthBridge.SetHiddenSource(this, false);
			this.SetXray(false, true);
			this.ClearFpOverride();
			this.StopPee();
			this.HideHud();
			if (this.promptUI != null && this.promptUI.gameObject.activeSelf)
			{
				this.promptUI.gameObject.SetActive(false);
			}
			base.StopAllCoroutines();
			if (this.HasAuthoredLockerAnimations)
			{
				this.RestoreAuthoredLockerPose(false);
			}
			this._hidden = false;
			this._exiting = false;
			this.ReleaseHideContext();
		}

		// Token: 0x0600818D RID: 33165 RVA: 0x0034EFCB File Offset: 0x0034D1CB
		private void OnDestroy()
		{
			if (this._coveredHiddenHudConfig == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				Object.Destroy(this._coveredHiddenHudConfig);
				return;
			}
			Object.DestroyImmediate(this._coveredHiddenHudConfig);
		}

		// Token: 0x0600818E RID: 33166 RVA: 0x0034EFFC File Offset: 0x0034D1FC
		private void Update()
		{
			this.RecoverPendingEntry();
			if (!this._hidden || this._exiting)
			{
				return;
			}
			bool isBottomPhysicallyCovered = PlayerExposureState.IsBottomPhysicallyCovered;
			if (isBottomPhysicallyCovered != this._hudBottomCovered)
			{
				this.ShowHud();
			}
			if (isBottomPhysicallyCovered && this._peeing)
			{
				this.StopPee();
			}
			if (FlyCamera.IsActive)
			{
				return;
			}
			if (PlayerActivityGate.IsResourceOwned(PlayerContextResource.ModalUI))
			{
				return;
			}
			if (Time.time - this._hiddenSince < 0.75f)
			{
				return;
			}
			if (this.HasAuthoredLockerAnimations && !this.IsHiddenIdle())
			{
				return;
			}
			if (ControllerContextBindings.ExitPressed(this.exitInput) && InteractionCandidateRegistry.TryConsumeContextInput(this.exitInput))
			{
				this.BeginExit();
				return;
			}
			if (this.xrayInput != null && this.xrayInput.GetButtonDown() && InteractionCandidateRegistry.TryConsumeContextInput(this.xrayInput))
			{
				this.SetXray(!this._xrayOn, false);
				return;
			}
			if (this.peeInput != null && this.peeInput.GetButtonDown() && InteractionCandidateRegistry.TryConsumeContextInput(this.peeInput))
			{
				if (this._peeing)
				{
					this.StopPee();
					return;
				}
				if (UncoveredBottomRequirement.TryAllow())
				{
					this._peeRoutine = base.StartCoroutine(this.PeeRoutine());
				}
			}
		}

		// Token: 0x0600818F RID: 33167 RVA: 0x0034F11C File Offset: 0x0034D31C
		private void LateUpdate()
		{
			this.HoldPairedPose();
			if (!this.HasAuthoredLockerAnimations || (!this._hidden && !this._exiting) || this._playerAnimator == null)
			{
				return;
			}
			AnimatorStateInfo animatorStateInfo = this._playerAnimator.GetCurrentAnimatorStateInfo(0);
			if (this._playerAnimator.IsInTransition(0))
			{
				AnimatorStateInfo nextAnimatorStateInfo = this._playerAnimator.GetNextAnimatorStateInfo(0);
				if (this.ResolveLockerAnimation(nextAnimatorStateInfo) != null)
				{
					animatorStateInfo = nextAnimatorStateInfo;
				}
			}
			AnimationClip animationClip = this.ResolveLockerAnimation(animatorStateInfo);
			if (animationClip == null)
			{
				return;
			}
			this.SetOffscreenSkinning(!animatorStateInfo.IsName("Ciel_LockerHide_Idle"));
			float num = (animatorStateInfo.IsName("Ciel_LockerHide_Idle") ? Mathf.Repeat(animatorStateInfo.normalizedTime, 1f) : Mathf.Clamp01(animatorStateInfo.normalizedTime));
			animationClip.SampleAnimation(this.lockerAnimationRoot.gameObject, num * animationClip.length);
			this.UpdateAuthoredDoorAudio(true);
		}

		// Token: 0x17001495 RID: 5269
		// (get) Token: 0x06008190 RID: 33168 RVA: 0x0034F204 File Offset: 0x0034D404
		private bool HasAuthoredLockerAnimations
		{
			get
			{
				return this.lockerAnimationRoot != null && this.lockerEnterAnimation != null && this.lockerIdleAnimation != null && this.lockerExitAnimation != null;
			}
		}

		// Token: 0x17001496 RID: 5270
		// (get) Token: 0x06008191 RID: 33169 RVA: 0x0034F23E File Offset: 0x0034D43E
		private Transform LockerVisualRoot
		{
			get
			{
				if (!(this.lockerAnimationRoot != null))
				{
					return base.transform;
				}
				return this.lockerAnimationRoot;
			}
		}

		// Token: 0x06008192 RID: 33170 RVA: 0x0034F25C File Offset: 0x0034D45C
		private void InitializeRendererCulling()
		{
			if (this._animatedRenderers != null || !this.HasAuthoredLockerAnimations)
			{
				return;
			}
			this._animatedRenderers = this.lockerAnimationRoot.GetComponentsInChildren<SkinnedMeshRenderer>(true);
			this._closedRendererBounds = new Bounds[this._animatedRenderers.Length];
			for (int i = 0; i < this._animatedRenderers.Length; i++)
			{
				this._closedRendererBounds[i] = this._animatedRenderers[i].localBounds;
				this._animatedRenderers[i].updateWhenOffscreen = false;
			}
			this._offscreenSkinningEnabled = false;
		}

		// Token: 0x06008193 RID: 33171 RVA: 0x0034F2E0 File Offset: 0x0034D4E0
		private void SetOffscreenSkinning(bool moving)
		{
			this.InitializeRendererCulling();
			if (this._animatedRenderers == null || this._offscreenSkinningEnabled == moving)
			{
				return;
			}
			this._offscreenSkinningEnabled = moving;
			for (int i = 0; i < this._animatedRenderers.Length; i++)
			{
				SkinnedMeshRenderer skinnedMeshRenderer = this._animatedRenderers[i];
				if (!(skinnedMeshRenderer == null))
				{
					skinnedMeshRenderer.updateWhenOffscreen = moving;
					if (!moving)
					{
						skinnedMeshRenderer.localBounds = this._closedRendererBounds[i];
					}
				}
			}
		}

		// Token: 0x06008194 RID: 33172 RVA: 0x0034F350 File Offset: 0x0034D550
		private bool IsHiddenIdle()
		{
			return this._playerAnimator != null && this._playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Ciel_LockerHide_Idle");
		}

		// Token: 0x06008195 RID: 33173 RVA: 0x0034F388 File Offset: 0x0034D588
		private AnimationClip ResolveLockerAnimation(AnimatorStateInfo state)
		{
			if (state.IsName("Ciel_LockerHide_Enter"))
			{
				return this.lockerEnterAnimation;
			}
			if (state.IsName("Ciel_LockerHide_Idle"))
			{
				return this.lockerIdleAnimation;
			}
			if (state.IsName("Ciel_LockerHide_Exit"))
			{
				return this.lockerExitAnimation;
			}
			return null;
		}

		// Token: 0x06008196 RID: 33174 RVA: 0x0034F3D5 File Offset: 0x0034D5D5
		private void RestoreAuthoredLockerPose(bool playAudio)
		{
			if (!this.HasAuthoredLockerAnimations)
			{
				return;
			}
			this.lockerIdleAnimation.SampleAnimation(this.lockerAnimationRoot.gameObject, 0f);
			this.SetOffscreenSkinning(false);
			this.UpdateAuthoredDoorAudio(playAudio);
		}

		// Token: 0x06008197 RID: 33175 RVA: 0x0034F40C File Offset: 0x0034D60C
		private void UpdateAuthoredDoorAudio(bool playAudio)
		{
			if (this.lockerDoor == null)
			{
				return;
			}
			bool flag = Quaternion.Angle(this._authoredDoorClosedLocalRotation, this.lockerDoor.localRotation) > 0.5f;
			if (playAudio)
			{
				if (flag && !this._doorIsOpen)
				{
					this.PlayDoorSound(this.doorOpenSound);
				}
				else if (!flag && this._doorIsOpen)
				{
					this.PlayDoorSound(this.doorCloseSound);
				}
			}
			this._doorIsOpen = flag;
		}

		// Token: 0x06008198 RID: 33176 RVA: 0x0034F480 File Offset: 0x0034D680
		private void HandleValidate(GameObject player)
		{
			this._activePlayer = player;
			this.EnsureGenericActionSubscription((player != null) ? player.GetComponentInChildren<vGenericAction>() : null);
			if (!this._hidden && !this._exiting && this.promptUI != null)
			{
				this.promptUI.MatchVehiclePlacement(this.LockerVisualRoot, 0.0055f);
				this.promptUI.gameObject.SetActive(true);
				this.promptUI.Show(this.enterConfig);
			}
		}

		// Token: 0x06008199 RID: 33177 RVA: 0x0034F504 File Offset: 0x0034D704
		private void EnsureGenericActionSubscription(vGenericAction playerGenericAction)
		{
			if (playerGenericAction == this._playerGenericAction && this._genericActionSubscribed)
			{
				return;
			}
			if (this._genericActionSubscribed && this._playerGenericAction != null)
			{
				this._playerGenericAction.OnStartAction.RemoveListener(new UnityAction<vTriggerGenericAction>(this.HandlePlayerActionStart));
				this._genericActionSubscribed = false;
			}
			this._playerGenericAction = playerGenericAction;
			if (this._playerGenericAction != null)
			{
				this._playerGenericAction.OnStartAction.AddListener(new UnityAction<vTriggerGenericAction>(this.HandlePlayerActionStart));
				this._genericActionSubscribed = true;
			}
		}

		// Token: 0x0600819A RID: 33178 RVA: 0x0034F59C File Offset: 0x0034D79C
		private void HandlePlayerActionStart(vTriggerGenericAction firingTrigger)
		{
			if (!this.IsOwnedTrigger(firingTrigger))
			{
				return;
			}
			InteractionCandidateRegistry.MarkContextInputConsumed(firingTrigger.actionInput);
			if (!this.TryAcquireHideContext())
			{
				this.RejectEntryAfterInvectorCommit();
				return;
			}
			this._entryRejected = false;
			this._entryPending = true;
			this._entryRequestedAt = Time.time;
			this.ResolvePlayerRefs();
			if (this.useRootMotion && !this._rootMotionPushed && this._actionAnimator != null)
			{
				this._actionAnimator.EnableRootMotionForAction();
				this._rootMotionPushed = true;
			}
			this.LockControls(true);
			if (this._playerGenericAction != null)
			{
				this._playerGenericAction.DisablePlayerGravityAndCollision();
			}
			if (this.HasAuthoredLockerAnimations)
			{
				this.ResolveEnterStart();
				firingTrigger.crossFadeTransition = 0f;
				firingTrigger.matchTarget = null;
				firingTrigger.useTriggerRotation = false;
			}
			this.SnapPlayerToEnterStart();
			this.StartPairedPose();
			this.BeginFirstPersonView();
			if (this.debugLogs)
			{
				Debug.Log("[LockerHide] E pressed → snapped to EnterStart.", this);
			}
		}

		// Token: 0x0600819B RID: 33179 RVA: 0x0034F68C File Offset: 0x0034D88C
		private void RecoverPendingEntry()
		{
			if (!this._entryPending || this._hidden || Time.time - this._entryRequestedAt < 2f)
			{
				return;
			}
			PlayerActivityLease hideLease = this._hideLease;
			vGenericAction playerGenericAction = this._playerGenericAction;
			if (playerGenericAction != null && playerGenericAction.doingAction && this.IsOwnedTrigger(playerGenericAction.triggerAction))
			{
				bool playingAnimation = playerGenericAction.playingAnimation;
				if (!this._entryPending || this._hidden || this._hideLease != hideLease)
				{
					return;
				}
			}
			if (playerGenericAction != null && playerGenericAction.doingAction && !this.IsOwnedTrigger(playerGenericAction.triggerAction))
			{
				this._rootMotionPushed = false;
				this._entryPoseCaptured = false;
				this.ReleaseHideContext();
				return;
			}
			Debug.LogWarning("[LockerHide] Entry animation did not start; restoring player controls.", this);
			this.CancelHideAndRestoreEntry(true);
		}

		// Token: 0x0600819C RID: 33180 RVA: 0x0034F754 File Offset: 0x0034D954
		private bool IsOwnedTrigger(vTriggerGenericAction t)
		{
			if (t == null || this.triggers == null)
			{
				return false;
			}
			for (int i = 0; i < this.triggers.Length; i++)
			{
				if (this.triggers[i] == t)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600819D RID: 33181 RVA: 0x0034F79A File Offset: 0x0034D99A
		private void HandleInvalidate(GameObject player)
		{
			if (!this._hidden && this.promptUI != null)
			{
				this.promptUI.gameObject.SetActive(false);
			}
		}

		// Token: 0x0600819E RID: 33182 RVA: 0x0034F7C4 File Offset: 0x0034D9C4
		private void HandleAnimationStart()
		{
			if (this._hidden)
			{
				return;
			}
			if (this._entryRejected || !this.TryAcquireHideContext())
			{
				this.RejectEntryAfterInvectorCommit();
				return;
			}
			this._entryPending = false;
			this._hidden = true;
			this._exiting = false;
			this._hiddenSince = Time.time;
			this.ResolvePlayerRefs();
			if (this._playerAnimator != null)
			{
				this._playerAnimator.ResetTrigger("LockerHideEnding");
			}
			if (this.useRootMotion && !this._emotionPushed && this._actionAnimator != null)
			{
				this._actionAnimator.DisableEmotionLayers();
				this._emotionPushed = true;
			}
			if (!this._entryPoseCaptured)
			{
				this.SnapPlayerToEnterStart();
			}
			if (this._pairedPoseInput == null)
			{
				this.StartPairedPose();
			}
			if (this.promptUI != null && this.promptUI.gameObject.activeSelf)
			{
				this.promptUI.gameObject.SetActive(false);
			}
			this.LockControls(true);
			this.BeginFirstPersonView();
			this.StartDoorDriver("Ciel_LockerHide_Enter", this.enterDoorOpenStart, this.enterDoorOpenEnd, this.enterDoorCloseStart, this.enterDoorCloseEnd);
			if (this._doorCloseWatch != null)
			{
				base.StopCoroutine(this._doorCloseWatch);
			}
			this._doorCloseWatch = base.StartCoroutine(this.CloseDoorWhenHiddenIdle());
			this.ShowHud();
			if (this.debugLogs)
			{
				Debug.Log("[LockerHide] Hiding: get-in clip started.", this);
			}
		}

		// Token: 0x0600819F RID: 33183 RVA: 0x0034F927 File Offset: 0x0034DB27
		public bool InterruptForEncounter()
		{
			if (!this._hidden && !this._exiting && this._hideLease == null)
			{
				return false;
			}
			this.CancelHideAndRestoreEntry(false);
			return true;
		}

		// Token: 0x060081A0 RID: 33184 RVA: 0x0034F94C File Offset: 0x0034DB4C
		private void CancelHideAndRestoreEntry(bool restoreLocomotion = false)
		{
			FlyCamera.StopForActivityLease(this._hideLease);
			base.StopAllCoroutines();
			Transform transform = this.ResolvePlayerTransform();
			if (transform != null && this._entryPoseCaptured)
			{
				LockerHideInteractable.ApplyPlayerPose(transform, this._entryPosition, this._entryRotation);
			}
			if (restoreLocomotion && this._playerAnimator != null)
			{
				int num = LockerHideInteractable.ResolveLocomotionStateHash(this._playerAnimator);
				if (num != 0)
				{
					this._playerAnimator.Play(num, 0, 0f);
				}
			}
			if (this._playerGenericAction != null && this._playerGenericAction.doingAction && this.IsOwnedTrigger(this._playerGenericAction.triggerAction))
			{
				this._playerGenericAction.CancelAction();
				if (this._playerGenericAction.doingAction)
				{
					this._playerGenericAction.ResetTriggerSettings(true);
				}
			}
			this.HandleCancelled();
			this._entryPoseCaptured = false;
		}

		// Token: 0x060081A1 RID: 33185 RVA: 0x0034FA28 File Offset: 0x0034DC28
		private void HandleCancelled()
		{
			FlyCamera.StopForActivityLease(this._hideLease);
			this.StopPairedPose();
			this.StopExitRoutine();
			this._entryRejected = false;
			if (!this._hidden)
			{
				if (this._rootMotionPushed)
				{
					this.RestorePlayerAfterExit();
				}
				this.LockControls(false);
				this.ClearFpOverride();
				this._entryPoseCaptured = false;
				this.ReleaseHideContext();
				return;
			}
			if (this.debugLogs)
			{
				Debug.Log("[LockerHide] Action cancelled → restoring.", this);
			}
			if (this._doorCloseWatch != null)
			{
				base.StopCoroutine(this._doorCloseWatch);
				this._doorCloseWatch = null;
			}
			this.RestorePlayerAfterExit();
			this.HideHud();
			this.CloseDoor();
			this.LockControls(false);
			PlayerStealthBridge.SetHiddenSource(this, false);
			this.SetXray(false, true);
			this.ClearFpOverride();
			this.StopPee();
			this._hidden = false;
			this._exiting = false;
			this.ReleaseHideContext();
		}

		// Token: 0x060081A2 RID: 33186 RVA: 0x0034FAFC File Offset: 0x0034DCFC
		private void ClearFpOverride()
		{
			if (!this._ownsFirstPersonView)
			{
				return;
			}
			this._ownsFirstPersonView = false;
			if (this._fpController != null)
			{
				this._fpController.YawClampOverrideCenter = null;
				this._fpController.EndInteractionView(this);
			}
		}

		// Token: 0x060081A3 RID: 33187 RVA: 0x0034FB48 File Offset: 0x0034DD48
		private void BeginFirstPersonView()
		{
			if (this._fpController == null || FullBodyHideActivityContext.IsUsingTripodCamera(this._hideLease))
			{
				return;
			}
			this._fpController.BeginInteractionView(this, true, this.fpLookHalfRange, false, false, null, null, null);
			this._ownsFirstPersonView = true;
			Transform transform = this.ResolveEnterStart();
			if (transform == null)
			{
				return;
			}
			this._fpController.YawClampOverrideCenter = new float?(transform.eulerAngles.y + 180f);
			this._fpController.YawClampOverrideRange = this.fpLookHalfRange;
		}

		// Token: 0x060081A4 RID: 33188 RVA: 0x0034FBE4 File Offset: 0x0034DDE4
		private void LockControls(bool locked)
		{
			if (locked == this._controlsLocked)
			{
				return;
			}
			if (locked)
			{
				Transform transform = this.ResolvePlayerTransform();
				this._lockedController = ((transform != null) ? transform.GetComponent<vThirdPersonController>() : null);
				if (this._lockedController != null)
				{
					this._lockedController.lockMovement = true;
					this._lockedController.lockRotation = true;
				}
			}
			else
			{
				if (this._lockedController != null)
				{
					this._lockedController.lockMovement = false;
					this._lockedController.lockRotation = false;
				}
				this._lockedController = null;
			}
			this._controlsLocked = locked;
		}

		// Token: 0x060081A5 RID: 33189 RVA: 0x0034FC7C File Offset: 0x0034DE7C
		private void BeginExit()
		{
			if (!this._hidden || this._exiting)
			{
				return;
			}
			if (FlyCamera.IsActive)
			{
				return;
			}
			if (!this.SetHiddenFreeCameraAvailable(false))
			{
				return;
			}
			this._exiting = true;
			if (this._doorCloseWatch != null)
			{
				base.StopCoroutine(this._doorCloseWatch);
				this._doorCloseWatch = null;
			}
			this.HideHud();
			PlayerStealthBridge.SetHiddenSource(this, false);
			this.SetXray(false, true);
			this.StopPee();
			this.StartDoorDriver("Ciel_LockerHide_Exit", this.exitDoorOpenStart, this.exitDoorOpenEnd, -1f, -1f);
			if (this.debugLogs)
			{
				Debug.Log("[LockerHide] Exit requested → playing get-out clip.", this);
			}
			if (this._playerAnimator != null)
			{
				this._playerAnimator.SetTrigger("LockerHideEnding");
			}
			this._exitRoutine = base.StartCoroutine(this.ExitRoutine());
		}

		// Token: 0x060081A6 RID: 33190 RVA: 0x0034FD4D File Offset: 0x0034DF4D
		private void StopExitRoutine()
		{
			if (this._exitRoutine == null)
			{
				return;
			}
			base.StopCoroutine(this._exitRoutine);
			this._exitRoutine = null;
		}

		// Token: 0x060081A7 RID: 33191 RVA: 0x0034FD6B File Offset: 0x0034DF6B
		private IEnumerator ExitRoutine()
		{
			float t0 = Time.time;
			while (this._hidden && this._exiting && this._playerAnimator != null && Time.time - t0 < 2f && !this._playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Ciel_LockerHide_Exit"))
			{
				yield return null;
			}
			t0 = Time.time;
			while (this._hidden && this._exiting && this._playerAnimator != null && Time.time - t0 < 8f)
			{
				AnimatorStateInfo currentAnimatorStateInfo = this._playerAnimator.GetCurrentAnimatorStateInfo(0);
				if (!currentAnimatorStateInfo.IsName("Ciel_LockerHide_Exit") || currentAnimatorStateInfo.normalizedTime >= this.exitAnimationCompletionNormalizedTime)
				{
					break;
				}
				yield return null;
			}
			if (!this._hidden || !this._exiting)
			{
				this._exitRoutine = null;
				yield break;
			}
			if (this.HasAuthoredLockerAnimations)
			{
				this.lockerExitAnimation.SampleAnimation(this.lockerAnimationRoot.gameObject, this.lockerExitAnimation.length);
				this.UpdateAuthoredDoorAudio(true);
			}
			Transform transform = this.ResolvePlayerTransform();
			if (transform != null && !Mathf.Approximately(this.exitFacingYawOffset, 0f))
			{
				Quaternion quaternion = Quaternion.Euler(0f, transform.eulerAngles.y + this.exitFacingYawOffset, 0f);
				LockerHideInteractable.ApplyPlayerPose(transform, transform.position, quaternion);
			}
			if (this._playerAnimator != null)
			{
				int num = LockerHideInteractable.ResolveLocomotionStateHash(this._playerAnimator);
				if (num != 0)
				{
					this._playerAnimator.Play(num, 0, 0f);
				}
			}
			this._exitRoutine = null;
			if (this._actionAnimator != null && this._actionAnimator.GenericAction != null && this._actionAnimator.GenericAction.doingAction)
			{
				this._actionAnimator.GenericAction.FinishAction();
			}
			this.OnExitAnimationFinished();
			yield break;
		}

		// Token: 0x060081A8 RID: 33192 RVA: 0x0034FD7C File Offset: 0x0034DF7C
		private static int ResolveLocomotionStateHash(Animator animator)
		{
			string[] array = new string[] { "Free Movement", "Locomotion", "Idle", "Grounded" };
			for (int i = 0; i < array.Length; i++)
			{
				int num = Animator.StringToHash(array[i]);
				if (animator.HasState(0, num))
				{
					return num;
				}
			}
			return 0;
		}

		// Token: 0x060081A9 RID: 33193 RVA: 0x0034FDD4 File Offset: 0x0034DFD4
		private void OnExitAnimationFinished()
		{
			this.ClearFpOverride();
			this.RestorePlayerAfterExit();
			this.LockControls(false);
			this._hidden = false;
			this._exiting = false;
			this.ReleaseHideContext();
			this.CloseDoor();
			if (this._activePlayer != null && this.promptUI != null && this.enterConfig != null)
			{
				this.promptUI.MatchVehiclePlacement(this.LockerVisualRoot, 0.0055f);
				this.promptUI.gameObject.SetActive(true);
			}
			if (this.debugLogs)
			{
				Debug.Log("[LockerHide] Exit complete: control returned to locomotion.", this);
			}
		}

		// Token: 0x060081AA RID: 33194 RVA: 0x0034FE74 File Offset: 0x0034E074
		private void RestorePlayerAfterExit()
		{
			this.StopPairedPose();
			this._entryPoseCaptured = false;
			if (this._rootMotionPushed && this._actionAnimator != null)
			{
				this._actionAnimator.RestoreRootMotionForAction();
			}
			this._rootMotionPushed = false;
			if (this._emotionPushed && this._actionAnimator != null)
			{
				this._actionAnimator.RestoreOverrideLayer();
			}
			this._emotionPushed = false;
		}

		// Token: 0x060081AB RID: 33195 RVA: 0x0034FEE0 File Offset: 0x0034E0E0
		private void SnapPlayerToEnterStart()
		{
			Transform transform = this.ResolveEnterStart();
			Transform transform2 = this.ResolvePlayerTransform();
			if (transform == null || transform2 == null)
			{
				return;
			}
			if (!this._entryPoseCaptured)
			{
				this._entryPosition = transform2.position;
				this._entryRotation = transform2.rotation;
				this._entryPoseCaptured = true;
			}
			Vector3 position = transform.position;
			Quaternion quaternion = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
			LockerHideInteractable.ApplyPlayerPose(transform2, position, quaternion);
		}

		// Token: 0x060081AC RID: 33196 RVA: 0x0034FF60 File Offset: 0x0034E160
		private void StartPairedPose()
		{
			this.StopPairedPose();
			if (!this.HasAuthoredLockerAnimations)
			{
				return;
			}
			Transform transform = this.ResolvePlayerTransform();
			this._pairedPoseInput = ((transform != null) ? transform.GetComponent<vThirdPersonInput>() : null);
			if (this._pairedPoseInput != null)
			{
				this._pairedPoseInput.onAnimatorMove += this.HoldPairedPose;
			}
		}

		// Token: 0x060081AD RID: 33197 RVA: 0x0034FFC0 File Offset: 0x0034E1C0
		private void StopPairedPose()
		{
			if (this._pairedPoseInput != null)
			{
				this._pairedPoseInput.onAnimatorMove -= this.HoldPairedPose;
			}
			this._pairedPoseInput = null;
		}

		// Token: 0x060081AE RID: 33198 RVA: 0x0034FFF0 File Offset: 0x0034E1F0
		private void HoldPairedPose()
		{
			if (!this._entryPoseCaptured || (!this._entryPending && !this._hidden) || !this.HasAuthoredLockerAnimations)
			{
				return;
			}
			Transform transform = this.ResolveEnterStart();
			Transform transform2 = this.ResolvePlayerTransform();
			if (transform != null && transform2 != null)
			{
				LockerHideInteractable.ApplyPlayerPose(transform2, transform.position, Quaternion.Euler(0f, transform.eulerAngles.y, 0f));
			}
		}

		// Token: 0x060081AF RID: 33199 RVA: 0x00350064 File Offset: 0x0034E264
		private static void ApplyPlayerPose(Transform playerT, Vector3 pos, Quaternion rot)
		{
			playerT.SetPositionAndRotation(pos, rot);
			Rigidbody component = playerT.GetComponent<Rigidbody>();
			if (component != null)
			{
				component.position = pos;
				component.rotation = rot;
				if (!component.isKinematic)
				{
					component.velocity = Vector3.zero;
					component.angularVelocity = Vector3.zero;
				}
			}
		}

		// Token: 0x060081B0 RID: 33200 RVA: 0x003500B8 File Offset: 0x0034E2B8
		private Transform ResolveEnterStart()
		{
			if (this.enterStart != null)
			{
				return this.enterStart;
			}
			foreach (Transform transform in base.GetComponentsInChildren<Transform>(true))
			{
				if (transform.name == "EnterStart")
				{
					this.enterStart = transform;
					return transform;
				}
			}
			if (this.triggers != null)
			{
				foreach (vTriggerGenericAction vTriggerGenericAction in this.triggers)
				{
					if (vTriggerGenericAction != null && vTriggerGenericAction.matchTarget != null)
					{
						return this.enterStart = vTriggerGenericAction.matchTarget;
					}
				}
			}
			return null;
		}

		// Token: 0x060081B1 RID: 33201 RVA: 0x0035015E File Offset: 0x0034E35E
		private Transform ResolvePlayerTransform()
		{
			if (PlayerRegistry.Player != null)
			{
				return PlayerRegistry.Player;
			}
			if (!(this._activePlayer != null))
			{
				return null;
			}
			return this._activePlayer.transform;
		}

		// Token: 0x060081B2 RID: 33202 RVA: 0x00350190 File Offset: 0x0034E390
		private void ResolvePlayerRefs()
		{
			GameObject gameObject = ((PlayerRegistry.Player != null) ? PlayerRegistry.Player.gameObject : this._activePlayer);
			if (gameObject != null)
			{
				this._actionAnimator = gameObject.GetComponentInChildren<PlayerActionAnimator>();
				this._playerAnimator = gameObject.GetComponentInChildren<Animator>();
				this._fpController = gameObject.GetComponentInChildren<FirstPersonController>();
			}
			if (this._actionAnimator == null)
			{
				this._actionAnimator = Object.FindObjectOfType<PlayerActionAnimator>();
			}
			if (this._playerAnimator == null && this._actionAnimator != null && this._actionAnimator.GenericAction != null)
			{
				this._playerAnimator = this._actionAnimator.GenericAction.GetComponentInChildren<Animator>();
			}
		}

		// Token: 0x060081B3 RID: 33203 RVA: 0x00350248 File Offset: 0x0034E448
		private void ShowHud()
		{
			if (this.hiddenHudConfig != null && InteractionHUDController.Instance != null)
			{
				this._hudBottomCovered = PlayerExposureState.IsBottomPhysicallyCovered;
				InteractionHUDController.Instance.Show(this._hudBottomCovered ? this.GetCoveredHiddenHudConfig() : this.hiddenHudConfig);
			}
		}

		// Token: 0x060081B4 RID: 33204 RVA: 0x0035029C File Offset: 0x0034E49C
		private InteractionConfigSO GetCoveredHiddenHudConfig()
		{
			if (this._coveredHiddenHudConfig != null)
			{
				return this._coveredHiddenHudConfig;
			}
			this._coveredHiddenHudConfig = ScriptableObject.CreateInstance<InteractionConfigSO>();
			ControllerContextBindings.CopyLabels(this.hiddenHudConfig, this._coveredHiddenHudConfig);
			this._coveredHiddenHudConfig.hideFlags = HideFlags.DontSave;
			this._coveredHiddenHudConfig.objectName = this.hiddenHudConfig.objectName;
			this._coveredHiddenHudConfig.mainActionName = this.hiddenHudConfig.mainActionName;
			this._coveredHiddenHudConfig.mainKeyDisplayLabel = this.hiddenHudConfig.mainKeyDisplayLabel;
			foreach (UsableAction usableAction in this.hiddenHudConfig.usableActions)
			{
				if (usableAction != null && !string.Equals(usableAction.keyDisplayLabel, "H", StringComparison.OrdinalIgnoreCase))
				{
					this._coveredHiddenHudConfig.usableActions.Add(usableAction);
				}
			}
			return this._coveredHiddenHudConfig;
		}

		// Token: 0x060081B5 RID: 33205 RVA: 0x00170130 File Offset: 0x0016E330
		private void HideHud()
		{
			if (InteractionHUDController.Instance != null)
			{
				InteractionHUDController.Instance.Hide();
			}
		}

		// Token: 0x060081B6 RID: 33206 RVA: 0x0035039C File Offset: 0x0034E59C
		private void StartDoorDriver(string stateName, float openStart, float openEnd, float closeStart, float closeEnd)
		{
			if (this.HasAuthoredLockerAnimations)
			{
				return;
			}
			if (this.lockerDoor == null || this._playerAnimator == null)
			{
				return;
			}
			if (this._doorDriver != null)
			{
				base.StopCoroutine(this._doorDriver);
			}
			if (this._doorRoutine != null)
			{
				base.StopCoroutine(this._doorRoutine);
				this._doorRoutine = null;
			}
			this._doorDriver = base.StartCoroutine(this.DriveDoorWithState(stateName, openStart, openEnd, closeStart, closeEnd));
		}

		// Token: 0x060081B7 RID: 33207 RVA: 0x00350417 File Offset: 0x0034E617
		private IEnumerator DriveDoorWithState(string stateName, float openStart, float openEnd, float closeStart, float closeEnd)
		{
			float t0 = Time.time;
			while (this._playerAnimator != null && Time.time - t0 < 2f)
			{
				if (this._playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
				{
					break;
				}
				yield return null;
			}
			while (this._playerAnimator != null)
			{
				AnimatorStateInfo currentAnimatorStateInfo = this._playerAnimator.GetCurrentAnimatorStateInfo(0);
				if (!currentAnimatorStateInfo.IsName(stateName))
				{
					break;
				}
				float num = Mathf.Clamp01(currentAnimatorStateInfo.normalizedTime);
				float num2 = LockerHideInteractable.Ramp(num, openStart, openEnd);
				if (closeStart >= 0f)
				{
					num2 -= LockerHideInteractable.Ramp(num, closeStart, closeEnd);
				}
				this._doorAngle = this.doorOpenAngle * Mathf.Clamp01(num2);
				this.ApplyDoorAngle(this._doorAngle);
				if (num >= 1f)
				{
					break;
				}
				yield return null;
			}
			this._doorDriver = null;
			yield break;
		}

		// Token: 0x060081B8 RID: 33208 RVA: 0x0035044B File Offset: 0x0034E64B
		private static float Ramp(float t, float a, float b)
		{
			if (b > a)
			{
				return Mathf.SmoothStep(0f, 1f, Mathf.Clamp01((t - a) / (b - a)));
			}
			if (t < a)
			{
				return 0f;
			}
			return 1f;
		}

		// Token: 0x060081B9 RID: 33209 RVA: 0x0035047C File Offset: 0x0034E67C
		private void CloseDoor()
		{
			if (this.HasAuthoredLockerAnimations)
			{
				this.RestoreAuthoredLockerPose(true);
				return;
			}
			this.AnimateDoor(0f);
		}

		// Token: 0x060081BA RID: 33210 RVA: 0x00350499 File Offset: 0x0034E699
		private void OpenDoor()
		{
			this.AnimateDoor(this.doorOpenAngle);
		}

		// Token: 0x060081BB RID: 33211 RVA: 0x003504A8 File Offset: 0x0034E6A8
		private void AnimateDoor(float targetAngle)
		{
			if (this.HasAuthoredLockerAnimations)
			{
				return;
			}
			if (this.lockerDoor == null)
			{
				return;
			}
			if (this._doorDriver != null)
			{
				base.StopCoroutine(this._doorDriver);
				this._doorDriver = null;
			}
			if (this._doorRoutine != null)
			{
				base.StopCoroutine(this._doorRoutine);
			}
			this._doorRoutine = base.StartCoroutine(this.SwingDoor(targetAngle));
		}

		// Token: 0x060081BC RID: 33212 RVA: 0x0035050F File Offset: 0x0034E70F
		private IEnumerator SwingDoor(float targetAngle)
		{
			while (this.lockerDoor != null && !Mathf.Approximately(this._doorAngle, targetAngle))
			{
				this._doorAngle = Mathf.MoveTowards(this._doorAngle, targetAngle, this.doorSpeed * Time.deltaTime);
				this.ApplyDoorAngle(this._doorAngle);
				yield return null;
			}
			this._doorRoutine = null;
			yield break;
		}

		// Token: 0x060081BD RID: 33213 RVA: 0x00350528 File Offset: 0x0034E728
		private void ApplyDoorAngle(float angle)
		{
			if (this.lockerDoor == null)
			{
				return;
			}
			Vector3 vector = base.transform.TransformPoint(this._doorClosedRootLocalPos);
			Quaternion quaternion = base.transform.rotation * this._doorClosedRootLocalRot;
			Vector3 vector2 = ((this.doorHinge != null) ? this.doorHinge.position : vector);
			Quaternion quaternion2 = Quaternion.AngleAxis(angle, Vector3.up);
			this.lockerDoor.SetPositionAndRotation(vector2 + quaternion2 * (vector - vector2), quaternion2 * quaternion);
			bool flag = Mathf.Abs(angle) > 0.5f;
			if (flag && !this._doorIsOpen)
			{
				this.PlayDoorSound(this.doorOpenSound);
			}
			else if (!flag && this._doorIsOpen)
			{
				this.PlayDoorSound(this.doorCloseSound);
			}
			this._doorIsOpen = flag;
		}

		// Token: 0x060081BE RID: 33214 RVA: 0x00350604 File Offset: 0x0034E804
		private IEnumerator PeeRoutine()
		{
			if (PlayerExposureState.IsBottomPhysicallyCovered)
			{
				yield break;
			}
			if (this.peeEffectPrefab == null || this._playerAnimator == null)
			{
				yield break;
			}
			Transform hips = this._playerAnimator.GetBoneTransform(HumanBodyBones.Hips);
			if (hips == null)
			{
				yield break;
			}
			this._peeing = true;
			if (this.peeStartDelay > 0f)
			{
				yield return new WaitForSeconds(this.peeStartDelay);
			}
			if (PlayerExposureState.IsBottomPhysicallyCovered)
			{
				this.StopPee();
				yield break;
			}
			this._peeInstance = Object.Instantiate<GameObject>(this.peeEffectPrefab, hips);
			this._peeInstance.transform.localPosition = this.peeLocalPositionOffset;
			this._peeInstance.transform.localRotation = Quaternion.Euler(this.peeLocalRotationOffset);
			foreach (AudioSource audioSource in this._peeInstance.GetComponentsInChildren<AudioSource>(true))
			{
				if (audioSource != null)
				{
					audioSource.enabled = false;
				}
			}
			this._peeInstance.SetActive(true);
			foreach (ParticleSystem particleSystem in this._peeInstance.GetComponentsInChildren<ParticleSystem>(true))
			{
				if (!(particleSystem == null))
				{
					particleSystem.collision.enabled = false;
					particleSystem.Play(true);
				}
			}
			foreach (VisualEffect visualEffect in this._peeInstance.GetComponentsInChildren<VisualEffect>(true))
			{
				if (visualEffect != null)
				{
					visualEffect.Play();
				}
			}
			if (this.peeAudioClip != null)
			{
				AudioSource audioSource2 = this._peeInstance.AddComponent<AudioSource>();
				audioSource2.clip = this.peeAudioClip;
				audioSource2.volume = this.peeAudioVolume;
				audioSource2.loop = true;
				audioSource2.playOnAwake = false;
				audioSource2.spatialBlend = 1f;
				audioSource2.rolloffMode = AudioRolloffMode.Linear;
				audioSource2.minDistance = 1f;
				audioSource2.maxDistance = 15f;
				audioSource2.Play();
			}
			this._peeRoutine = null;
			yield break;
		}

		// Token: 0x060081BF RID: 33215 RVA: 0x00350614 File Offset: 0x0034E814
		private void StopPee()
		{
			if (this._peeRoutine != null)
			{
				base.StopCoroutine(this._peeRoutine);
				this._peeRoutine = null;
			}
			if (this._peeInstance != null)
			{
				Object.Destroy(this._peeInstance);
				this._peeInstance = null;
			}
			this._peeing = false;
		}

		// Token: 0x060081C0 RID: 33216 RVA: 0x00350664 File Offset: 0x0034E864
		private void SetXray(bool on, bool instant)
		{
			if (on == this._xrayOn)
			{
				return;
			}
			this._xrayOn = on;
			if (on)
			{
				if (this._xrayInstances.Count == 0)
				{
					this.BuildXrayInstances();
				}
				this.StartXrayFade(this.xrayOpacity, instant, false);
				return;
			}
			this.StartXrayFade(1f, instant, true);
		}

		// Token: 0x060081C1 RID: 33217 RVA: 0x003506B4 File Offset: 0x0034E8B4
		private void BuildXrayInstances()
		{
			List<Renderer> list = new List<Renderer>();
			foreach (Renderer renderer in base.GetComponentsInChildren<Renderer>(true))
			{
				if (renderer.enabled && (renderer is MeshRenderer || renderer is SkinnedMeshRenderer) && (!(this.promptUI != null) || !renderer.transform.IsChildOf(this.promptUI.transform)))
				{
					list.Add(renderer);
				}
			}
			this._xrayRenderers = list.ToArray();
			this._xrayOriginalMats = new Material[this._xrayRenderers.Length][];
			for (int j = 0; j < this._xrayRenderers.Length; j++)
			{
				this._xrayOriginalMats[j] = this._xrayRenderers[j].sharedMaterials;
				Material[] array = new Material[this._xrayOriginalMats[j].Length];
				for (int k = 0; k < array.Length; k++)
				{
					if (!(this._xrayOriginalMats[j][k] == null))
					{
						Material material = new Material(this._xrayOriginalMats[j][k]);
						LockerHideInteractable.MakeMaterialTransparent(material);
						this._xrayInstances.Add(material);
						array[k] = material;
					}
				}
				this._xrayRenderers[j].sharedMaterials = array;
			}
		}

		// Token: 0x060081C2 RID: 33218 RVA: 0x003507F0 File Offset: 0x0034E9F0
		private static void MakeMaterialTransparent(Material m)
		{
			if (m.HasProperty("_Surface"))
			{
				m.SetFloat("_Surface", 1f);
				m.SetOverrideTag("RenderType", "Transparent");
				if (m.HasProperty("_SrcBlend"))
				{
					m.SetInt("_SrcBlend", 5);
				}
				if (m.HasProperty("_DstBlend"))
				{
					m.SetInt("_DstBlend", 10);
				}
				if (m.HasProperty("_ZWrite"))
				{
					m.SetInt("_ZWrite", 0);
				}
				m.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
				m.renderQueue = 3000;
				return;
			}
			m.SetOverrideTag("RenderType", "Transparent");
			if (m.HasProperty("_SrcBlend"))
			{
				m.SetInt("_SrcBlend", 5);
			}
			if (m.HasProperty("_DstBlend"))
			{
				m.SetInt("_DstBlend", 10);
			}
			if (m.HasProperty("_ZWrite"))
			{
				m.SetInt("_ZWrite", 0);
			}
			m.DisableKeyword("_ALPHATEST_ON");
			m.EnableKeyword("_ALPHABLEND_ON");
			m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			m.renderQueue = 3000;
		}

		// Token: 0x060081C3 RID: 33219 RVA: 0x00350918 File Offset: 0x0034EB18
		private void StartXrayFade(float target, bool instant, bool restoreWhenDone)
		{
			if (this._xrayFade != null)
			{
				base.StopCoroutine(this._xrayFade);
				this._xrayFade = null;
			}
			if (instant || this.xrayFadeDuration <= 0f || !base.isActiveAndEnabled)
			{
				this.WriteXrayAlpha(target);
				if (restoreWhenDone)
				{
					this.RestoreXrayMaterials();
				}
				return;
			}
			this._xrayFade = base.StartCoroutine(this.FadeXray(target, restoreWhenDone));
		}

		// Token: 0x060081C4 RID: 33220 RVA: 0x0035097D File Offset: 0x0034EB7D
		private IEnumerator FadeXray(float target, bool restoreWhenDone)
		{
			float start = this.ReadXrayAlpha();
			float t = 0f;
			while (t < this.xrayFadeDuration)
			{
				t += Time.deltaTime;
				this.WriteXrayAlpha(Mathf.Lerp(start, target, t / this.xrayFadeDuration));
				yield return null;
			}
			this.WriteXrayAlpha(target);
			if (restoreWhenDone)
			{
				this.RestoreXrayMaterials();
			}
			this._xrayFade = null;
			yield break;
		}

		// Token: 0x060081C5 RID: 33221 RVA: 0x0035099C File Offset: 0x0034EB9C
		private float ReadXrayAlpha()
		{
			foreach (Material material in this._xrayInstances)
			{
				if (!(material == null))
				{
					if (material.HasProperty("_BaseColor"))
					{
						return material.GetColor("_BaseColor").a;
					}
					if (material.HasProperty("_Color"))
					{
						return material.GetColor("_Color").a;
					}
				}
			}
			return 1f;
		}

		// Token: 0x060081C6 RID: 33222 RVA: 0x00350A38 File Offset: 0x0034EC38
		private void WriteXrayAlpha(float a)
		{
			foreach (Material material in this._xrayInstances)
			{
				if (!(material == null))
				{
					if (material.HasProperty("_BaseColor"))
					{
						Color color = material.GetColor("_BaseColor");
						color.a = a;
						material.SetColor("_BaseColor", color);
					}
					if (material.HasProperty("_Color"))
					{
						Color color2 = material.GetColor("_Color");
						color2.a = a;
						material.SetColor("_Color", color2);
					}
				}
			}
		}

		// Token: 0x060081C7 RID: 33223 RVA: 0x00350AE8 File Offset: 0x0034ECE8
		private void RestoreXrayMaterials()
		{
			if (this._xrayRenderers != null && this._xrayOriginalMats != null)
			{
				for (int i = 0; i < this._xrayRenderers.Length; i++)
				{
					if (this._xrayRenderers[i] != null && this._xrayOriginalMats[i] != null)
					{
						this._xrayRenderers[i].sharedMaterials = this._xrayOriginalMats[i];
					}
				}
			}
			foreach (Material material in this._xrayInstances)
			{
				if (!(material == null))
				{
					if (Application.isPlaying)
					{
						Object.Destroy(material);
					}
					else
					{
						Object.DestroyImmediate(material);
					}
				}
			}
			this._xrayInstances.Clear();
			this._xrayRenderers = null;
			this._xrayOriginalMats = null;
		}

		// Token: 0x060081C8 RID: 33224 RVA: 0x00350BC0 File Offset: 0x0034EDC0
		private void PlayDoorSound(AudioClip clip)
		{
			if (clip == null)
			{
				return;
			}
			if (this.audioSource == null)
			{
				this.audioSource = base.GetComponent<AudioSource>();
			}
			if (this.audioSource != null)
			{
				this.audioSource.PlayOneShot(clip);
				return;
			}
			if (this.lockerDoor != null)
			{
				AudioSource.PlayClipAtPoint(clip, this.lockerDoor.position);
			}
		}

		// Token: 0x060081C9 RID: 33225 RVA: 0x00350C2B File Offset: 0x0034EE2B
		private IEnumerator CloseDoorWhenHiddenIdle()
		{
			float timeout = Time.time + 12f;
			bool reachedIdle = false;
			while (this._hidden && !this._exiting && this._playerAnimator != null && Time.time < timeout)
			{
				if (this._playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Ciel_LockerHide_Idle"))
				{
					reachedIdle = true;
					break;
				}
				yield return null;
			}
			if (!reachedIdle && this._hidden && !this._exiting)
			{
				Debug.LogWarning("[LockerHide] Hidden idle did not start; restoring the entry pose.", this);
				this.CancelHideAndRestoreEntry(true);
				yield break;
			}
			if (reachedIdle && this._hidden && !this._exiting)
			{
				this.CloseDoor();
				this.SetHiddenFreeCameraAvailable(true);
				PlayerStealthBridge.SetHiddenSource(this, true);
				ScoreGameplaySignals.NotifyHidingStarted(ScoreHidingSpotKind.Locker, this);
			}
			this._doorCloseWatch = null;
			yield break;
		}

		// Token: 0x040084BA RID: 33978
		private const string EnterStateName = "Ciel_LockerHide_Enter";

		// Token: 0x040084BB RID: 33979
		private const string IdleStateName = "Ciel_LockerHide_Idle";

		// Token: 0x040084BC RID: 33980
		private const string ExitStateName = "Ciel_LockerHide_Exit";

		// Token: 0x040084BD RID: 33981
		private const string EndingTrigger = "LockerHideEnding";

		// Token: 0x040084BE RID: 33982
		[Header("References")]
		[Tooltip("vTriggerGenericAction(s) that drive the hide animation. Auto-resolved from this GameObject / children if empty.")]
		[SerializeField]
		private vTriggerGenericAction[] triggers;

		// Token: 0x040084BF RID: 33983
		[Tooltip("World-space prompt shown when the player is in range. Auto-resolved from children.")]
		[SerializeField]
		private InteractablePromptUI promptUI;

		// Token: 0x040084C0 RID: 33984
		[Tooltip("For paired animations this is the common player/object choreography origin. For legacy clips: one measured net-root-travel in front of the hide spot, facing INTO the locker, so the clip's root motion does the whole walk and lands exactly centred. Auto-resolved by name.")]
		[SerializeField]
		private Transform enterStart;

		// Token: 0x040084C1 RID: 33985
		[Header("UI Config")]
		[Tooltip("Prompt text + key shown while OUTSIDE (e.g. 'Locker' / 'E' / 'Nasconditi'). Required.")]
		[SerializeField]
		private InteractionConfigSO enterConfig;

		// Token: 0x040084C2 RID: 33986
		[Tooltip("Config for the shared right-side HUD shown WHILE HIDDEN. Its usableActions must contain at least the 'Esci' entry (keyDisplayLabel + actionName). Required for the HUD.")]
		[SerializeField]
		private InteractionConfigSO hiddenHudConfig;

		// Token: 0x040084C3 RID: 33987
		[Header("Exit input")]
		[Tooltip("Input pressed to get out of the locker while hidden.")]
		[SerializeField]
		private GenericInput exitInput = new GenericInput("E", "A", "A");

		// Token: 0x040084C4 RID: 33988
		[Header("Locker door")]
		[Tooltip("Optional door transform. Its AUTHORED pose is the CLOSED state; it swings open around the HINGE (doorHinge node, world-up axis) during the get-in / get-out clips and closes again while hidden. Null to skip.")]
		[SerializeField]
		private Transform lockerDoor;

		// Token: 0x040084C5 RID: 33989
		[Tooltip("Hinge point the door swings around (vertical axis through this position). The door mesh pivot is NOT on the hinge — rotating in place detaches the panel. Placed by the setup tool on the edge opposite the lock; falls back to the door pivot.")]
		[SerializeField]
		private Transform doorHinge;

		// Token: 0x040084C6 RID: 33990
		[Tooltip("Degrees the door swings open around the hinge. Flip the sign if it clips the wall.")]
		[SerializeField]
		private float doorOpenAngle = 90f;

		// Token: 0x040084C7 RID: 33991
		[SerializeField]
		private float doorSpeed = 300f;

		// Token: 0x040084C8 RID: 33992
		[Header("Authored locker animations")]
		[Tooltip("Root relative to which the OBJ clips bind their Locker hierarchy. When all three clips are assigned, their authored motion replaces the procedural door swing.")]
		[SerializeField]
		private Transform lockerAnimationRoot;

		// Token: 0x040084C9 RID: 33993
		[SerializeField]
		private AnimationClip lockerEnterAnimation;

		// Token: 0x040084CA RID: 33994
		[SerializeField]
		private AnimationClip lockerIdleAnimation;

		// Token: 0x040084CB RID: 33995
		[SerializeField]
		private AnimationClip lockerExitAnimation;

		// Token: 0x040084CC RID: 33996
		[Header("Door sync (normalized time of the get-in / get-out clips)")]
		[Tooltip("Get-in clip: door starts opening here (0..1) — match it to the hand reaching out.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float enterDoorOpenStart = 0.05f;

		// Token: 0x040084CD RID: 33997
		[Tooltip("Get-in clip: door is fully open here.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float enterDoorOpenEnd = 0.28f;

		// Token: 0x040084CE RID: 33998
		[Tooltip("Get-in clip: door starts closing here (she's inside, pulling it shut).")]
		[Range(0f, 1f)]
		[SerializeField]
		private float enterDoorCloseStart = 0.7f;

		// Token: 0x040084CF RID: 33999
		[Tooltip("Get-in clip: door is fully closed here.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float enterDoorCloseEnd = 0.93f;

		// Token: 0x040084D0 RID: 34000
		[Tooltip("Get-out clip: door starts opening here (hand pushes it).")]
		[Range(0f, 1f)]
		[SerializeField]
		private float exitDoorOpenStart = 0.02f;

		// Token: 0x040084D1 RID: 34001
		[Tooltip("Get-out clip: door is fully open here.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float exitDoorOpenEnd = 0.25f;

		// Token: 0x040084D2 RID: 34002
		[Header("First person while hidden")]
		[Tooltip("Half-range (degrees) the FP look can swing around the locker's OUT direction while hidden. The transform faces the back wall (pose-baked 180°), so without an override the FP clamp centres the view into the wall.")]
		[SerializeField]
		private float fpLookHalfRange = 100f;

		// Token: 0x040084D3 RID: 34003
		[Header("X-ray (see through the locker while hidden)")]
		[Tooltip("Input that toggles the locker transparency while hidden inside. NOT V (first-person view) or T (emote wheel) — G is free in this context.")]
		[SerializeField]
		private GenericInput xrayInput = new GenericInput("G", "RightStickClick", "RightStickClick");

		// Token: 0x040084D4 RID: 34004
		[Tooltip("Locker opacity (0..1) while x-ray is ON — lower = more see-through.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float xrayOpacity = 0.3f;

		// Token: 0x040084D5 RID: 34005
		[Tooltip("Seconds to fade the opacity in/out.")]
		[SerializeField]
		private float xrayFadeDuration = 0.25f;

		// Token: 0x040084D6 RID: 34006
		[Header("Pee (while hidden)")]
		[Tooltip("Input that makes Ciel pee while hidden inside (keeps the hide idle pose).")]
		[SerializeField]
		private GenericInput peeInput = new GenericInput("H", "LeftStickClick", "LeftStickClick");

		// Token: 0x040084D7 RID: 34007
		[Tooltip("Pee VFX prefab — wired by the setup tool from the StandPee emote's PropBinding (Elixir_Normal), so it stays identical to the stand-pee effect.")]
		[SerializeField]
		private GameObject peeEffectPrefab;

		// Token: 0x040084D8 RID: 34008
		[Tooltip("Local offset from the Hips bone. HAND-TUNED in play mode for the locker hide pose (2026-07-12) — do NOT reset to the StandPee binding values.")]
		[SerializeField]
		private Vector3 peeLocalPositionOffset = new Vector3(0.0009303628f, -0.0009779186f, 0.04639858f);

		// Token: 0x040084D9 RID: 34009
		[SerializeField]
		private Vector3 peeLocalRotationOffset = new Vector3(-50.795f, 0.465f, -0.902f);

		// Token: 0x040084DA RID: 34010
		[Tooltip("Delay before the stream appears after pressing the key.")]
		[SerializeField]
		private float peeStartDelay = 0.5f;

		// Token: 0x040084DB RID: 34011
		[SerializeField]
		private AudioClip peeAudioClip;

		// Token: 0x040084DC RID: 34012
		[SerializeField]
		[Range(0f, 1f)]
		private float peeAudioVolume = 0.2f;

		// Token: 0x040084DD RID: 34013
		[Header("Audio")]
		[Tooltip("Played when the door STARTS swinging open.")]
		[SerializeField]
		private AudioClip doorOpenSound;

		// Token: 0x040084DE RID: 34014
		[Tooltip("Played the moment the door shuts.")]
		[SerializeField]
		private AudioClip doorCloseSound;

		// Token: 0x040084DF RID: 34015
		[Tooltip("3D source on the locker. Auto-resolved from this object if empty.")]
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x040084E0 RID: 34016
		[Header("Root motion")]
		[Tooltip("Own the animator root-motion settings and emotion layers during the action. Paired clips bake their movement into the body pose around EnterStart; legacy clips move the player root. Keep enabled for both configurations.")]
		[SerializeField]
		private bool useRootMotion = true;

		// Token: 0x040084E1 RID: 34017
		[Tooltip("Facing correction when the exit clip returns to locomotion. The original CupboardHide clips need 180 degrees; the new paired Locker clips need zero.")]
		[SerializeField]
		private float exitFacingYawOffset = 180f;

		// Token: 0x040084E2 RID: 34018
		[Tooltip("How much of the exit clip must play before physics and movement are restored. Use 1 for the new paired clips so the door and character finish together.")]
		[Range(0.9f, 1f)]
		[SerializeField]
		private float exitAnimationCompletionNormalizedTime = 0.9f;

		// Token: 0x040084E3 RID: 34019
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x040084E4 RID: 34020
		private GameObject _activePlayer;

		// Token: 0x040084E5 RID: 34021
		private PlayerActionAnimator _actionAnimator;

		// Token: 0x040084E6 RID: 34022
		private Animator _playerAnimator;

		// Token: 0x040084E7 RID: 34023
		private bool _hidden;

		// Token: 0x040084E8 RID: 34024
		private bool _exiting;

		// Token: 0x040084E9 RID: 34025
		private bool _rootMotionPushed;

		// Token: 0x040084EA RID: 34026
		private bool _emotionPushed;

		// Token: 0x040084EB RID: 34027
		private vThirdPersonInput _pairedPoseInput;

		// Token: 0x040084EC RID: 34028
		private float _hiddenSince;

		// Token: 0x040084ED RID: 34029
		private bool _entryRejected;

		// Token: 0x040084EE RID: 34030
		private bool _entryPending;

		// Token: 0x040084EF RID: 34031
		private float _entryRequestedAt;

		// Token: 0x040084F0 RID: 34032
		private Coroutine _rejectedEntryCleanup;

		// Token: 0x040084F1 RID: 34033
		private Coroutine _exitRoutine;

		// Token: 0x040084F2 RID: 34034
		private PlayerActivityLease _hideLease;

		// Token: 0x040084F3 RID: 34035
		private bool _ownsFirstPersonView;

		// Token: 0x040084F4 RID: 34036
		private TriggerInteractionCandidate _interactionCandidate;

		// Token: 0x040084F5 RID: 34037
		private vGenericAction _playerGenericAction;

		// Token: 0x040084F6 RID: 34038
		private bool _genericActionSubscribed;

		// Token: 0x040084F7 RID: 34039
		private vThirdPersonController _lockedController;

		// Token: 0x040084F8 RID: 34040
		private bool _controlsLocked;

		// Token: 0x040084F9 RID: 34041
		private FirstPersonController _fpController;

		// Token: 0x040084FA RID: 34042
		private bool _peeing;

		// Token: 0x040084FB RID: 34043
		private Coroutine _peeRoutine;

		// Token: 0x040084FC RID: 34044
		private GameObject _peeInstance;

		// Token: 0x040084FD RID: 34045
		private InteractionConfigSO _coveredHiddenHudConfig;

		// Token: 0x040084FE RID: 34046
		private bool _hudBottomCovered;

		// Token: 0x040084FF RID: 34047
		private Vector3 _doorClosedRootLocalPos;

		// Token: 0x04008500 RID: 34048
		private Quaternion _doorClosedRootLocalRot;

		// Token: 0x04008501 RID: 34049
		private float _doorAngle;

		// Token: 0x04008502 RID: 34050
		private bool _doorIsOpen;

		// Token: 0x04008503 RID: 34051
		private Quaternion _authoredDoorClosedLocalRotation;

		// Token: 0x04008504 RID: 34052
		private bool _xrayOn;

		// Token: 0x04008505 RID: 34053
		private Coroutine _xrayFade;

		// Token: 0x04008506 RID: 34054
		private Renderer[] _xrayRenderers;

		// Token: 0x04008507 RID: 34055
		private Material[][] _xrayOriginalMats;

		// Token: 0x04008508 RID: 34056
		private readonly List<Material> _xrayInstances = new List<Material>();

		// Token: 0x04008509 RID: 34057
		private Coroutine _doorRoutine;

		// Token: 0x0400850A RID: 34058
		private Coroutine _doorCloseWatch;

		// Token: 0x0400850B RID: 34059
		private Coroutine _doorDriver;

		// Token: 0x0400850C RID: 34060
		private SkinnedMeshRenderer[] _animatedRenderers;

		// Token: 0x0400850D RID: 34061
		private Bounds[] _closedRendererBounds;

		// Token: 0x0400850E RID: 34062
		private bool _offscreenSkinningEnabled;

		// Token: 0x0400850F RID: 34063
		private bool _entryPoseCaptured;

		// Token: 0x04008510 RID: 34064
		private Vector3 _entryPosition;

		// Token: 0x04008511 RID: 34065
		private Quaternion _entryRotation;
	}
}
