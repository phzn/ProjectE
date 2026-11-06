using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Dinopunch.LivingMud;
using Features.DebugTools;
using Features.Detection;
using Features.MainMenu;
using Features.Score;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using ProjectExhibitionist;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.FirstPerson;
using ProjectExhibitionist.Interactions;
using UnityEngine;
using UnityEngine.Events;

namespace Features.DumpsterHide
{
	// Token: 0x02001236 RID: 4662
	[DefaultExecutionOrder(900)]
	[DisallowMultipleComponent]
	public class DumpsterHideInteractable : MonoBehaviour
	{
		// Token: 0x060090C5 RID: 37061 RVA: 0x003B4234 File Offset: 0x003B2434
		internal Object ReadUniversityGameplayReference(UniversityGameplayField field)
		{
			switch (field)
			{
			case UniversityGameplayField.DumpsterHideInteractable_enterConfig:
				return this.enterConfig;
			case UniversityGameplayField.DumpsterHideInteractable_hiddenHudConfig:
				return this.hiddenHudConfig;
			case UniversityGameplayField.DumpsterHideInteractable_lidCloseSound:
				return this.lidCloseSound;
			case UniversityGameplayField.DumpsterHideInteractable_lidOpenSound:
				return this.lidOpenSound;
			default:
				if (field != UniversityGameplayField.DumpsterHideInteractable_hideBodyPaintDecal)
				{
					throw new InvalidOperationException("Unsupported DumpsterHideInteractable gameplay binding: " + field.ToString());
				}
				return this.hideBodyPaintDecal;
			}
		}

		// Token: 0x060090C6 RID: 37062 RVA: 0x003B42A0 File Offset: 0x003B24A0
		internal void WriteUniversityGameplayReference(UniversityGameplayField field, Object asset)
		{
			switch (field)
			{
			case UniversityGameplayField.DumpsterHideInteractable_enterConfig:
				this.enterConfig = (InteractionConfigSO)asset;
				return;
			case UniversityGameplayField.DumpsterHideInteractable_hiddenHudConfig:
				this.hiddenHudConfig = (InteractionConfigSO)asset;
				return;
			case UniversityGameplayField.DumpsterHideInteractable_lidCloseSound:
				this.lidCloseSound = (AudioClip)asset;
				return;
			case UniversityGameplayField.DumpsterHideInteractable_lidOpenSound:
				this.lidOpenSound = (AudioClip)asset;
				return;
			default:
				if (field != UniversityGameplayField.DumpsterHideInteractable_hideBodyPaintDecal)
				{
					throw new InvalidOperationException("Unsupported DumpsterHideInteractable gameplay binding: " + field.ToString());
				}
				this.hideBodyPaintDecal = (SkinDecal)asset;
				return;
			}
		}

		// Token: 0x17001726 RID: 5926
		// (get) Token: 0x060090C7 RID: 37063 RVA: 0x003B4329 File Offset: 0x003B2529
		public bool HasActiveResidencyInteraction
		{
			get
			{
				return this._hidden || this._exiting || this._entryPending || this._controlsLocked;
			}
		}

		// Token: 0x060090C8 RID: 37064 RVA: 0x003B434C File Offset: 0x003B254C
		private void Awake()
		{
			ControllerContextBindings.SetLabel(this.hiddenHudConfig, "E", "B", false);
			ControllerContextBindings.SetLabel(this.hiddenHudConfig, "G", "R3", false);
			ControllerContextBindings.Set(this.exitInput, "B");
			ControllerContextBindings.Set(this.xrayInput, "RightStickClick");
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
				this.promptUI.MatchVehiclePlacement(base.transform, 0.0055f);
			}
			if (this.dumpsterAnimator == null)
			{
				this.dumpsterAnimator = base.GetComponentInChildren<Animator>(true);
			}
			if (this.lidBone == null)
			{
				this.lidBone = DumpsterHideInteractable.FindDeep(base.transform, "lid1");
			}
			if (this.triggers == null || this.triggers.Length == 0)
			{
				Debug.LogError("[DumpsterHide] No vTriggerGenericAction in the hierarchy of '" + base.name + "'.", this);
			}
			if (this.promptUI != null)
			{
				this.promptUI.Hide();
				this.promptUI.gameObject.SetActive(false);
			}
			this._interactionCandidate = TriggerInteractionCandidate.Attach(base.gameObject, this.triggers, this.promptUI, () => !this._hidden && !this._exiting && !FlyCamera.IsActive && FullBodyHideActivityContext.CanAcquireWithTripodCamera, null);
		}

		// Token: 0x060090C9 RID: 37065 RVA: 0x003B44C2 File Offset: 0x003B26C2
		private void Start()
		{
			if (this.promptUI != null)
			{
				this.promptUI.MatchVehiclePlacement(base.transform, 0.0055f);
			}
		}

		// Token: 0x060090CA RID: 37066 RVA: 0x003B44E9 File Offset: 0x003B26E9
		private void LateUpdate()
		{
			this.HoldPairedPose();
			this.RefreshEnterPrompt();
		}

		// Token: 0x060090CB RID: 37067 RVA: 0x003B44F8 File Offset: 0x003B26F8
		private void RefreshEnterPrompt()
		{
			if (this.promptUI == null)
			{
				return;
			}
			if (this._hidden || this._exiting || !(this.enterConfig != null) || !InteractionCandidateRegistry.IsWinner(this._interactionCandidate))
			{
				if (this.promptUI.IsVisible || InteractablePromptUI.Current == this.promptUI)
				{
					this.promptUI.Hide();
				}
				if (this.promptUI.gameObject.activeSelf)
				{
					this.promptUI.gameObject.SetActive(false);
				}
				return;
			}
			if (!this.promptUI.IsVisible || InteractablePromptUI.Current != this.promptUI)
			{
				this.promptUI.gameObject.SetActive(true);
				this.promptUI.MatchVehiclePlacement(base.transform, 0.0055f);
				this.promptUI.Show(this.enterConfig);
			}
		}

		// Token: 0x17001727 RID: 5927
		// (get) Token: 0x060090CC RID: 37068 RVA: 0x003B45E8 File Offset: 0x003B27E8
		private string HideGateReason
		{
			get
			{
				return "DumpsterHide#" + base.GetInstanceID().ToString();
			}
		}

		// Token: 0x060090CD RID: 37069 RVA: 0x003B460D File Offset: 0x003B280D
		private bool TryAcquireHideContext()
		{
			return FullBodyHideActivityContext.TryAcquire(this.HideGateReason, this, ref this._hideLease, PlayerActivity.AllPlayerActions, true, true);
		}

		// Token: 0x060090CE RID: 37070 RVA: 0x003B4628 File Offset: 0x003B2828
		private bool SetHiddenFreeCameraAvailable(bool available)
		{
			return FullBodyHideActivityContext.SetPhotoModeAvailable(this._hideLease, available);
		}

		// Token: 0x060090CF RID: 37071 RVA: 0x003B4636 File Offset: 0x003B2836
		private void ReleaseHideContext()
		{
			this._entryPending = false;
			FullBodyHideActivityContext.Release(ref this._hideLease);
		}

		// Token: 0x060090D0 RID: 37072 RVA: 0x003B464A File Offset: 0x003B284A
		private void RejectEntryAfterInvectorCommit()
		{
			this._entryRejected = true;
			if (this._rejectedEntryCleanup == null && base.isActiveAndEnabled)
			{
				this._rejectedEntryCleanup = base.StartCoroutine(this.CancelRejectedEntryNextFrame());
			}
		}

		// Token: 0x060090D1 RID: 37073 RVA: 0x003B4675 File Offset: 0x003B2875
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

		// Token: 0x060090D2 RID: 37074 RVA: 0x003B4684 File Offset: 0x003B2884
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

		// Token: 0x060090D3 RID: 37075 RVA: 0x003B475C File Offset: 0x003B295C
		private void OnDisable()
		{
			FlyCamera.StopForActivityLease(this._hideLease);
			this.StopPairedRootMotion();
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
			this.SetInteriorCam(false);
			this.ClearFpOverride();
			this.StopMirror();
			this.ResetDumpsterAnimator();
			this.HideHud();
			if (this.promptUI != null && this.promptUI.gameObject.activeSelf)
			{
				this.promptUI.gameObject.SetActive(false);
			}
			base.StopAllCoroutines();
			this._hidden = false;
			this._exiting = false;
			this.ReleaseHideContext();
		}

		// Token: 0x060090D4 RID: 37076 RVA: 0x003B497C File Offset: 0x003B2B7C
		private void Update()
		{
			this.RecoverPendingEntry();
			this.UpdateLidSoundWatcher();
			if (!this._hidden || this._exiting)
			{
				return;
			}
			if (PlayerActivityGate.IsResourceOwned(PlayerContextResource.ModalUI))
			{
				return;
			}
			if (FlyCamera.IsActive)
			{
				return;
			}
			if (this._camOn)
			{
				this.UpdateInteriorCamYaw();
			}
			if (Time.time - this._hiddenSince < 0.75f)
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
			}
		}

		// Token: 0x060090D5 RID: 37077 RVA: 0x003B4A2D File Offset: 0x003B2C2D
		private void HandleValidate(GameObject player)
		{
			this._activePlayer = player;
			this.EnsureGenericActionSubscription((player != null) ? player.GetComponentInChildren<vGenericAction>() : null);
			this.RefreshEnterPrompt();
		}

		// Token: 0x060090D6 RID: 37078 RVA: 0x003B4A54 File Offset: 0x003B2C54
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

		// Token: 0x060090D7 RID: 37079 RVA: 0x003B4AEC File Offset: 0x003B2CEC
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
			firingTrigger.crossFadeTransition = 0f;
			this.LockControls(true);
			if (this._playerGenericAction != null)
			{
				this._playerGenericAction.DisablePlayerGravityAndCollision();
			}
			this.ResolveEnterStart();
			firingTrigger.matchTarget = null;
			firingTrigger.useTriggerRotation = false;
			this.SnapPlayerToEnterStart();
			this.StartPairedRootMotion();
			this.BeginFirstPersonView();
			if (this.debugLogs)
			{
				Debug.Log("[DumpsterHide] E pressed → snapped to EnterStart.", this);
			}
		}

		// Token: 0x060090D8 RID: 37080 RVA: 0x003B4BD4 File Offset: 0x003B2DD4
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
			Debug.LogWarning("[DumpsterHide] Entry animation did not start; restoring player controls.", this);
			this.CancelHideAndRestoreEntry(true);
		}

		// Token: 0x060090D9 RID: 37081 RVA: 0x003B4C9C File Offset: 0x003B2E9C
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

		// Token: 0x060090DA RID: 37082 RVA: 0x003B4CE2 File Offset: 0x003B2EE2
		private void HandleInvalidate(GameObject player)
		{
			if (!this._hidden && this.promptUI != null)
			{
				this.promptUI.gameObject.SetActive(false);
			}
		}

		// Token: 0x060090DB RID: 37083 RVA: 0x003B4D0C File Offset: 0x003B2F0C
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
			this.ApplyHideBodyPaint();
			if (this._playerAnimator != null)
			{
				this._playerAnimator.ResetTrigger("DumpsterHideEnding");
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
			if (this._rootMotionInput == null)
			{
				this.StartPairedRootMotion();
			}
			if (this.promptUI != null && this.promptUI.gameObject.activeSelf)
			{
				this.promptUI.gameObject.SetActive(false);
			}
			this.LockControls(true);
			this.BeginFirstPersonView();
			this.StartMirror();
			if (this._idleWatch != null)
			{
				base.StopCoroutine(this._idleWatch);
			}
			this._idleWatch = base.StartCoroutine(this.OnReachedHiddenIdle());
			this.ShowHud();
			if (this.debugLogs)
			{
				Debug.Log("[DumpsterHide] Hiding: paired Enter clip started.", this);
			}
		}

		// Token: 0x060090DC RID: 37084 RVA: 0x003B4E58 File Offset: 0x003B3058
		private void ApplyHideBodyPaint()
		{
			Transform transform = this.ResolvePlayerTransform();
			GarbageBodyPaint.TryApply((transform != null) ? transform.gameObject : null, this.hideBodyPaintDecal);
		}

		// Token: 0x060090DD RID: 37085 RVA: 0x003B4E8A File Offset: 0x003B308A
		public bool InterruptForEncounter()
		{
			if (!this._hidden && !this._exiting && this._hideLease == null)
			{
				return false;
			}
			this.CancelHideAndRestoreEntry(false);
			return true;
		}

		// Token: 0x060090DE RID: 37086 RVA: 0x003B4EB0 File Offset: 0x003B30B0
		private void CancelHideAndRestoreEntry(bool restoreLocomotion = false)
		{
			FlyCamera.StopForActivityLease(this._hideLease);
			base.StopAllCoroutines();
			Transform transform = this.ResolvePlayerTransform();
			if (transform != null && this._entryPoseCaptured)
			{
				DumpsterHideInteractable.ApplyPlayerPose(transform, this._entryPosition, this._entryRotation);
			}
			if (restoreLocomotion && this._playerAnimator != null)
			{
				int num = DumpsterHideInteractable.ResolveLocomotionStateHash(this._playerAnimator);
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

		// Token: 0x060090DF RID: 37087 RVA: 0x003B4F8C File Offset: 0x003B318C
		private void HandleCancelled()
		{
			FlyCamera.StopForActivityLease(this._hideLease);
			this.StopPairedRootMotion();
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
				Debug.Log("[DumpsterHide] Action cancelled → restoring.", this);
			}
			if (this._idleWatch != null)
			{
				base.StopCoroutine(this._idleWatch);
				this._idleWatch = null;
			}
			this.RestorePlayerAfterExit();
			this.HideHud();
			this.LockControls(false);
			PlayerStealthBridge.SetHiddenSource(this, false);
			this.SetXray(false, true);
			this.SetInteriorCam(false);
			this.ClearFpOverride();
			this.StopMirror();
			this.ResetDumpsterAnimator();
			this._hidden = false;
			this._exiting = false;
			this.ReleaseHideContext();
		}

		// Token: 0x060090E0 RID: 37088 RVA: 0x003B5060 File Offset: 0x003B3260
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

		// Token: 0x060090E1 RID: 37089 RVA: 0x003B50AC File Offset: 0x003B32AC
		private void BeginFirstPersonView()
		{
			if (this._fpController == null || FullBodyHideActivityContext.IsUsingTripodCamera(this._hideLease))
			{
				return;
			}
			this._fpController.BeginInteractionView(this, true, this.fpLookHalfRange, false, true, null, null, null);
			this._ownsFirstPersonView = true;
			Transform transform = this.ResolveEnterStart();
			if (transform == null)
			{
				return;
			}
			this._fpController.YawClampOverrideCenter = new float?(transform.eulerAngles.y + 180f);
			this._fpController.YawClampOverrideRange = this.fpLookHalfRange;
		}

		// Token: 0x060090E2 RID: 37090 RVA: 0x003B5148 File Offset: 0x003B3348
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

		// Token: 0x060090E3 RID: 37091 RVA: 0x003B51E0 File Offset: 0x003B33E0
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
			if (this._playerAnimator == null || !this._playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Ciel_DumpsterHide_Idle"))
			{
				return;
			}
			if (!this.SetHiddenFreeCameraAvailable(false))
			{
				return;
			}
			this._exiting = true;
			if (this._idleWatch != null)
			{
				base.StopCoroutine(this._idleWatch);
				this._idleWatch = null;
			}
			this.HideHud();
			PlayerStealthBridge.SetHiddenSource(this, false);
			this.SetXray(false, true);
			this.SetInteriorCam(false);
			if (this.debugLogs)
			{
				Debug.Log("[DumpsterHide] Exit requested → playing get-out clip.", this);
			}
			if (this._playerAnimator != null)
			{
				this._playerAnimator.SetTrigger("DumpsterHideEnding");
			}
			this.CapturePairedPose();
			base.StartCoroutine(this.ExitRoutine());
		}

		// Token: 0x060090E4 RID: 37092 RVA: 0x003B52BC File Offset: 0x003B34BC
		private IEnumerator ExitRoutine()
		{
			float t0 = Time.time;
			bool reachedExit = false;
			while (this._hidden && this._exiting && this._playerAnimator != null && Time.time - t0 < 2f)
			{
				if (this._playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Ciel_DumpsterHide_Exit"))
				{
					reachedExit = true;
					break;
				}
				yield return null;
			}
			if (!this._hidden || !this._exiting)
			{
				yield break;
			}
			if (!reachedExit)
			{
				Debug.LogWarning("[DumpsterHide] Exit state did not start; restoring the entry pose.", this);
				this.CancelHideAndRestoreEntry(true);
				yield break;
			}
			t0 = Time.time;
			bool completedExit = false;
			while (this._hidden && this._exiting && this._playerAnimator != null && Time.time - t0 < 15f)
			{
				AnimatorStateInfo currentAnimatorStateInfo = this._playerAnimator.GetCurrentAnimatorStateInfo(0);
				if (!currentAnimatorStateInfo.IsName("Ciel_DumpsterHide_Exit") || currentAnimatorStateInfo.normalizedTime >= 1f)
				{
					completedExit = true;
					break;
				}
				yield return null;
			}
			if (!this._hidden || !this._exiting)
			{
				yield break;
			}
			if (!completedExit)
			{
				Debug.LogWarning("[DumpsterHide] Exit animation timed out; restoring the entry pose.", this);
				this.CancelHideAndRestoreEntry(true);
				yield break;
			}
			if (!Mathf.Approximately(this.exitYawFlipDegrees, 0f))
			{
				Transform transform = this.ResolvePlayerTransform();
				if (transform != null)
				{
					Quaternion quaternion = Quaternion.Euler(0f, transform.eulerAngles.y + this.exitYawFlipDegrees, 0f);
					DumpsterHideInteractable.ApplyPlayerPose(transform, transform.position, quaternion);
				}
			}
			if (this._playerAnimator != null)
			{
				int num = DumpsterHideInteractable.ResolveLocomotionStateHash(this._playerAnimator);
				if (num != 0)
				{
					this._playerAnimator.Play(num, 0, 0f);
				}
			}
			if (this._actionAnimator != null && this._actionAnimator.GenericAction != null && this._actionAnimator.GenericAction.doingAction)
			{
				this._actionAnimator.GenericAction.FinishAction();
			}
			this.OnExitAnimationFinished();
			yield break;
		}

		// Token: 0x060090E5 RID: 37093 RVA: 0x003B52CC File Offset: 0x003B34CC
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

		// Token: 0x060090E6 RID: 37094 RVA: 0x003B5324 File Offset: 0x003B3524
		private void OnExitAnimationFinished()
		{
			this.ClearFpOverride();
			this.RestorePlayerAfterExit();
			this.LockControls(false);
			this._hidden = false;
			this._exiting = false;
			this.ReleaseHideContext();
			this.StopMirror();
			this.ResetDumpsterAnimator();
			this.RefreshEnterPrompt();
			if (this.debugLogs)
			{
				Debug.Log("[DumpsterHide] Exit complete: control returned to locomotion.", this);
			}
		}

		// Token: 0x060090E7 RID: 37095 RVA: 0x003B5380 File Offset: 0x003B3580
		private void RestorePlayerAfterExit()
		{
			this.StopPairedRootMotion();
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

		// Token: 0x060090E8 RID: 37096 RVA: 0x003B53EC File Offset: 0x003B35EC
		private void StartPairedRootMotion()
		{
			if (this._rootMotionInput != null)
			{
				this._rootMotionInput.onAnimatorMove -= this.ApplyPairedRootMotionScale;
			}
			this._rootMotionInput = null;
			if (!this.useRootMotion || !this._rootMotionPushed || this._playerAnimator == null || !this._playerAnimator.isHuman)
			{
				return;
			}
			Transform boneTransform = this._playerAnimator.GetBoneTransform(HumanBodyBones.Hips);
			float num = Mathf.Abs(this._playerAnimator.transform.lossyScale.x);
			if (boneTransform == null || num <= Mathf.Epsilon)
			{
				return;
			}
			this._pairedRootMotionScale = Mathf.Abs(boneTransform.lossyScale.x) / num;
			Transform transform = this.ResolvePlayerTransform();
			this._rootMotionInput = ((transform != null) ? transform.GetComponent<vThirdPersonInput>() : null);
			if (this._rootMotionInput == null)
			{
				this._rootMotionInput = this._playerAnimator.GetComponentInParent<vThirdPersonInput>();
			}
			if (this._rootMotionInput != null)
			{
				if (!this._pairedPoseCaptured)
				{
					this.CapturePairedPose();
				}
				this._rootMotionInput.onAnimatorMove += this.ApplyPairedRootMotionScale;
			}
		}

		// Token: 0x060090E9 RID: 37097 RVA: 0x003B5514 File Offset: 0x003B3714
		private void CapturePairedPose()
		{
			Transform transform = this.ResolvePlayerTransform();
			if (transform == null)
			{
				return;
			}
			this._pairedLocalPosition = base.transform.InverseTransformPoint(transform.position);
			this._pairedLocalRotation = Quaternion.Inverse(base.transform.rotation) * transform.rotation;
			this._pairedPoseCaptured = true;
		}

		// Token: 0x060090EA RID: 37098 RVA: 0x003B5571 File Offset: 0x003B3771
		private void StopPairedRootMotion()
		{
			if (this._rootMotionInput != null)
			{
				this._rootMotionInput.onAnimatorMove -= this.ApplyPairedRootMotionScale;
			}
			this._rootMotionInput = null;
			this._pairedRootMotionScale = 1f;
			this._pairedPoseCaptured = false;
		}

		// Token: 0x060090EB RID: 37099 RVA: 0x003B55B4 File Offset: 0x003B37B4
		private void HoldPairedPose()
		{
			if (!this._pairedPoseCaptured || (!this._hidden && !this._entryPending))
			{
				return;
			}
			Transform transform = this.ResolvePlayerTransform();
			if (transform != null)
			{
				this.UpdateAuthoredEntryPose(transform);
				DumpsterHideInteractable.ApplyPlayerPose(transform, base.transform.TransformPoint(this._pairedLocalPosition), base.transform.rotation * this._pairedLocalRotation);
			}
		}

		// Token: 0x060090EC RID: 37100 RVA: 0x003B5620 File Offset: 0x003B3820
		private void ApplyPairedRootMotionScale()
		{
			if ((!this._hidden && !this._entryPending) || !this.useRootMotion || !this._rootMotionPushed || this._playerAnimator == null || this._rootMotionInput == null || this._rootMotionInput.cc == null || !this._rootMotionInput.cc.enabled)
			{
				return;
			}
			AnimatorStateInfo animatorStateInfo;
			string text = this.CurrentOrNextHideState(out animatorStateInfo);
			if (text == null && !this._entryPending)
			{
				return;
			}
			Transform transform = this.ResolvePlayerTransform();
			if (transform == null)
			{
				return;
			}
			if (this.UpdateAuthoredEntryPose(transform))
			{
				return;
			}
			bool flag = text != null && text != "Ciel_DumpsterHide_Idle";
			Vector3 vector = (flag ? this._playerAnimator.deltaPosition : Vector3.zero);
			Quaternion quaternion = (flag ? this._playerAnimator.deltaRotation : Quaternion.identity);
			this.ApplyPairedDelta(transform, vector, quaternion);
		}

		// Token: 0x060090ED RID: 37101 RVA: 0x003B5708 File Offset: 0x003B3908
		private bool UpdateAuthoredEntryPose(Transform player)
		{
			AnimatorStateInfo animatorStateInfo;
			string text = this.CurrentOrNextHideState(out animatorStateInfo);
			if (text == "Ciel_DumpsterHide_Idle" && this._playerAnimator.IsInTransition(0))
			{
				AnimatorStateInfo currentAnimatorStateInfo = this._playerAnimator.GetCurrentAnimatorStateInfo(0);
				if (currentAnimatorStateInfo.IsName("Ciel_DumpsterHide_Enter") || currentAnimatorStateInfo.IsName("Ciel_DumpsterHide_Open"))
				{
					return this.ApplyEntryPoseAtPhase(player, currentAnimatorStateInfo.normalizedTime);
				}
			}
			return (!(text != "Ciel_DumpsterHide_Enter") || !(text != "Ciel_DumpsterHide_Open") || !(text != "Ciel_DumpsterHide_Idle")) && this.ApplyEntryPoseAtPhase(player, (text == "Ciel_DumpsterHide_Idle") ? 1f : animatorStateInfo.normalizedTime);
		}

		// Token: 0x060090EE RID: 37102 RVA: 0x003B57BC File Offset: 0x003B39BC
		private bool ApplyEntryPoseAtPhase(Transform player, float phase)
		{
			Transform transform = this.ResolveEnterStart();
			if (transform == null || this.hideTarget == null)
			{
				return false;
			}
			if (this._entryTrajectory == null)
			{
				this._entryTrajectory = Resources.Load<DumpsterRootTrajectory>("DumpsterRootTrajectory");
			}
			if (this._entryTrajectory == null || !this._entryTrajectory.IsValid)
			{
				return false;
			}
			phase = Mathf.Clamp01(phase);
			this._pairedLocalPosition = this._entryTrajectory.EvaluateEntry(phase, base.transform.InverseTransformPoint(transform.position), base.transform.InverseTransformPoint(this.hideTarget.position));
			this._pairedLocalRotation = Quaternion.Inverse(base.transform.rotation) * Quaternion.Slerp(Quaternion.Euler(0f, transform.eulerAngles.y, 0f), Quaternion.Euler(0f, this.hideTarget.eulerAngles.y, 0f), phase);
			this._pairedPoseCaptured = true;
			DumpsterHideInteractable.ApplyPlayerPose(player, base.transform.TransformPoint(this._pairedLocalPosition), base.transform.rotation * this._pairedLocalRotation);
			return true;
		}

		// Token: 0x060090EF RID: 37103 RVA: 0x003B58F4 File Offset: 0x003B3AF4
		private void ApplyPairedDelta(Transform player, Vector3 delta, Quaternion turn)
		{
			delta.x *= this._pairedRootMotionScale;
			delta.z *= this._pairedRootMotionScale;
			this._pairedLocalPosition += base.transform.InverseTransformVector(delta);
			this._pairedLocalRotation *= turn;
			DumpsterHideInteractable.ApplyPlayerPose(player, base.transform.TransformPoint(this._pairedLocalPosition), base.transform.rotation * this._pairedLocalRotation);
		}

		// Token: 0x060090F0 RID: 37104 RVA: 0x003B597F File Offset: 0x003B3B7F
		private void StartMirror()
		{
			this.StopMirror();
			this._mirroredDumpState = null;
			if (this.dumpsterAnimator != null && this._playerAnimator != null)
			{
				this._mirrorRoutine = base.StartCoroutine(this.MirrorDumpsterRoutine());
			}
		}

		// Token: 0x060090F1 RID: 37105 RVA: 0x003B59BC File Offset: 0x003B3BBC
		private void StopMirror()
		{
			if (this._mirrorRoutine != null)
			{
				base.StopCoroutine(this._mirrorRoutine);
				this._mirrorRoutine = null;
			}
		}

		// Token: 0x060090F2 RID: 37106 RVA: 0x003B59D9 File Offset: 0x003B3BD9
		private IEnumerator MirrorDumpsterRoutine()
		{
			while (this._playerAnimator != null && this.dumpsterAnimator != null)
			{
				AnimatorStateInfo animatorStateInfo;
				string text = this.CurrentOrNextHideState(out animatorStateInfo);
				string text2 = null;
				if (text == "Ciel_DumpsterHide_Open" || text == "Ciel_DumpsterHide_Enter")
				{
					text2 = "Enter";
				}
				else if (text == "Ciel_DumpsterHide_Idle")
				{
					text2 = "Idle";
				}
				else if (text == "Ciel_DumpsterHide_Exit")
				{
					text2 = "Exit";
				}
				if (text2 != null)
				{
					bool flag = text2 != this._mirroredDumpState;
					float num = Mathf.Max(0f, animatorStateInfo.normalizedTime);
					if (text2 == "Idle")
					{
						num = Mathf.Repeat(num, 1f);
					}
					else
					{
						num = Mathf.Clamp01(num);
					}
					this.dumpsterAnimator.Play(text2, 0, num);
					this._mirroredDumpState = text2;
					if (flag && this.debugLogs)
					{
						Debug.Log("[DumpsterHide] Mirror: dumpster -> " + text2, this);
					}
				}
				yield return null;
			}
			this._mirrorRoutine = null;
			yield break;
		}

		// Token: 0x060090F3 RID: 37107 RVA: 0x003B59E8 File Offset: 0x003B3BE8
		private string CurrentOrNextHideState(out AnimatorStateInfo info)
		{
			info = default(AnimatorStateInfo);
			if (this._playerAnimator == null)
			{
				return null;
			}
			if (this._playerAnimator.IsInTransition(0))
			{
				AnimatorStateInfo nextAnimatorStateInfo = this._playerAnimator.GetNextAnimatorStateInfo(0);
				string text = DumpsterHideInteractable.MatchHideState(nextAnimatorStateInfo);
				if (text != null)
				{
					info = nextAnimatorStateInfo;
					return text;
				}
			}
			info = this._playerAnimator.GetCurrentAnimatorStateInfo(0);
			return DumpsterHideInteractable.MatchHideState(info);
		}

		// Token: 0x060090F4 RID: 37108 RVA: 0x003B5A58 File Offset: 0x003B3C58
		private static string MatchHideState(AnimatorStateInfo info)
		{
			if (info.IsName("Ciel_DumpsterHide_Open"))
			{
				return "Ciel_DumpsterHide_Open";
			}
			if (info.IsName("Ciel_DumpsterHide_Enter"))
			{
				return "Ciel_DumpsterHide_Enter";
			}
			if (info.IsName("Ciel_DumpsterHide_Idle"))
			{
				return "Ciel_DumpsterHide_Idle";
			}
			if (info.IsName("Ciel_DumpsterHide_Exit"))
			{
				return "Ciel_DumpsterHide_Exit";
			}
			return null;
		}

		// Token: 0x060090F5 RID: 37109 RVA: 0x003B5AB8 File Offset: 0x003B3CB8
		private void ResetDumpsterAnimator()
		{
			if (this.dumpsterAnimator == null || !this.dumpsterAnimator.isActiveAndEnabled)
			{
				return;
			}
			this.dumpsterAnimator.CrossFadeInFixedTime("Closed", Mathf.Max(0.1f, this.lidResetBlend), 0, 0f);
			this._mirroredDumpState = "Closed";
		}

		// Token: 0x060090F6 RID: 37110 RVA: 0x003B5B14 File Offset: 0x003B3D14
		private void PlayLidSound(AudioClip clip)
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
			AudioSource.PlayClipAtPoint(clip, base.transform.position);
		}

		// Token: 0x060090F7 RID: 37111 RVA: 0x003B5B74 File Offset: 0x003B3D74
		private void UpdateLidSoundWatcher()
		{
			if (this.lidBone == null)
			{
				return;
			}
			if (this._lidRefCaptured)
			{
				float num = Quaternion.Angle(this.lidBone.localRotation, this._lidShutRefRot);
				bool flag = (this._lidSoundOpen ? (num > this.lidSoundAngleThreshold * 0.75f) : (num > this.lidSoundAngleThreshold));
				if (flag != this._lidSoundOpen)
				{
					if (Time.time - this._lastLidSoundTime > 0.5f)
					{
						this.PlayLidSound(flag ? this.lidOpenSound : this.lidCloseSound);
						this._lastLidSoundTime = Time.time;
					}
					this._lidSoundOpen = flag;
				}
				return;
			}
			if (this.dumpsterAnimator == null || !this.dumpsterAnimator.isActiveAndEnabled || Time.frameCount < 2)
			{
				return;
			}
			this._lidShutRefRot = this.lidBone.localRotation;
			this._lidRefCaptured = true;
			this._lidSoundOpen = false;
		}

		// Token: 0x060090F8 RID: 37112 RVA: 0x003B5C5C File Offset: 0x003B3E5C
		private static Transform FindDeep(Transform root, string childName)
		{
			for (int i = 0; i < root.childCount; i++)
			{
				Transform child = root.GetChild(i);
				if (child.name == childName)
				{
					return child;
				}
				Transform transform = DumpsterHideInteractable.FindDeep(child, childName);
				if (transform != null)
				{
					return transform;
				}
			}
			return null;
		}

		// Token: 0x060090F9 RID: 37113 RVA: 0x003B5CA6 File Offset: 0x003B3EA6
		private IEnumerator OnReachedHiddenIdle()
		{
			float timeout = Time.time + 12f;
			bool reachedIdle = false;
			while (this._hidden && !this._exiting && Time.time < timeout)
			{
				if (this._playerAnimator != null && this._playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Ciel_DumpsterHide_Idle"))
				{
					reachedIdle = true;
					break;
				}
				yield return null;
			}
			if (!reachedIdle && this._hidden && !this._exiting)
			{
				Debug.LogWarning("[DumpsterHide] Hidden idle did not start; restoring the entry pose.", this);
				this.CancelHideAndRestoreEntry(true);
				yield break;
			}
			if (reachedIdle && this._hidden && !this._exiting)
			{
				this.SetHiddenFreeCameraAvailable(true);
				PlayerStealthBridge.SetHiddenSource(this, true);
				ScoreGameplaySignals.NotifyHidingStarted(ScoreHidingSpotKind.Dumpster, this);
			}
			this._idleWatch = null;
			yield break;
		}

		// Token: 0x060090FA RID: 37114 RVA: 0x003B5CB8 File Offset: 0x003B3EB8
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
			DumpsterHideInteractable.ApplyPlayerPose(transform2, position, quaternion);
			this.CapturePairedPose();
		}

		// Token: 0x060090FB RID: 37115 RVA: 0x003B5D40 File Offset: 0x003B3F40
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

		// Token: 0x060090FC RID: 37116 RVA: 0x003B5D94 File Offset: 0x003B3F94
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

		// Token: 0x060090FD RID: 37117 RVA: 0x003B5E3A File Offset: 0x003B403A
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

		// Token: 0x060090FE RID: 37118 RVA: 0x003B5E6C File Offset: 0x003B406C
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

		// Token: 0x060090FF RID: 37119 RVA: 0x003B5F24 File Offset: 0x003B4124
		private void ShowHud()
		{
			if (this.hiddenHudConfig != null && InteractionHUDController.Instance != null)
			{
				InteractionHUDController.Instance.Show(this.hiddenHudConfig);
			}
		}

		// Token: 0x06009100 RID: 37120 RVA: 0x00170130 File Offset: 0x0016E330
		private void HideHud()
		{
			if (InteractionHUDController.Instance != null)
			{
				InteractionHUDController.Instance.Hide();
			}
		}

		// Token: 0x06009101 RID: 37121 RVA: 0x003B5F54 File Offset: 0x003B4154
		private void SetInteriorCam(bool on)
		{
			if (on && FullBodyHideActivityContext.IsUsingTripodCamera(this._hideLease))
			{
				return;
			}
			if (on == this._camOn)
			{
				return;
			}
			if (on && this.interiorVCam == null)
			{
				return;
			}
			if (on)
			{
				if (this._mainCamT == null && Camera.main != null)
				{
					this._mainCamT = Camera.main.transform;
				}
				if (this._mainCamT != null && !this._mainCamLocalSaved)
				{
					this._mainCamLocalPos = this._mainCamT.localPosition;
					this._mainCamLocalRot = this._mainCamT.localRotation;
					this._mainCamLocalSaved = true;
				}
				if (this.brain == null && Camera.main != null)
				{
					this.brain = Camera.main.GetComponent<CinemachineBrain>();
					if (this.brain == null)
					{
						this.brain = Camera.main.gameObject.AddComponent<CinemachineBrain>();
					}
				}
				if (this.brain != null)
				{
					this.brain.enabled = true;
				}
				this._vcamBaseLocalRot = this.interiorVCam.transform.localRotation;
				this._vcamBaseSaved = true;
				this._camYawOffset = 0f;
				this._camPitchOffset = 0f;
				this._vcamLastWrittenRot = this._vcamBaseLocalRot;
				this.interiorVCam.Priority = this.cameraLivePriority;
				this._camOn = true;
				if (this.debugLogs)
				{
					Debug.Log("[DumpsterHide] Interior cam ON.", this);
					return;
				}
			}
			else
			{
				if (this.interiorVCam != null)
				{
					this.interiorVCam.Priority = 0;
					if (this._vcamBaseSaved)
					{
						this.interiorVCam.transform.localRotation = this._vcamBaseLocalRot;
					}
					Debug.Log("[DumpsterHide] Interior cam OFF — framing: localPos=" + this.interiorVCam.transform.localPosition.ToString("F3") + " localEuler=" + this.interiorVCam.transform.localEulerAngles.ToString("F2"), this);
				}
				if (this.brain != null)
				{
					this.brain.enabled = false;
				}
				if (this._mainCamT != null && this._mainCamLocalSaved)
				{
					this._mainCamT.localPosition = this._mainCamLocalPos;
					this._mainCamT.localRotation = this._mainCamLocalRot;
				}
				this._mainCamLocalSaved = false;
				this._camOn = false;
				if (this.debugLogs)
				{
					Debug.Log("[DumpsterHide] Interior cam OFF.", this);
				}
			}
		}

		// Token: 0x06009102 RID: 37122 RVA: 0x003B61D0 File Offset: 0x003B43D0
		private void UpdateInteriorCamYaw()
		{
			if (this.interiorVCam == null || !this._vcamBaseSaved)
			{
				return;
			}
			Quaternion localRotation = this.interiorVCam.transform.localRotation;
			if (Quaternion.Angle(localRotation, this._vcamLastWrittenRot) > 0.05f)
			{
				this._vcamBaseLocalRot = localRotation;
				this._camYawOffset = 0f;
				this._camPitchOffset = 0f;
			}
			Vector2 vector = new Vector2(GameplayInput.GetAxis("Mouse X"), GameplayInput.GetAxis("Mouse Y"));
			if (!ControllerInput.IsActive)
			{
				vector = GameSettingsService.ApplyMouseLookInversion(vector);
			}
			this._camYawOffset = Mathf.Clamp(this._camYawOffset + vector.x * this.camYawSensitivity, -this.camYawHalfRange, this.camYawHalfRange);
			this._camPitchOffset = Mathf.Clamp(this._camPitchOffset - vector.y * this.camPitchSensitivity, -this.camPitchHalfRange, this.camPitchHalfRange);
			this._vcamLastWrittenRot = Quaternion.Euler(0f, this._camYawOffset, 0f) * this._vcamBaseLocalRot * Quaternion.Euler(this._camPitchOffset, 0f, 0f);
			this.interiorVCam.transform.localRotation = this._vcamLastWrittenRot;
		}

		// Token: 0x06009103 RID: 37123 RVA: 0x003B630C File Offset: 0x003B450C
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

		// Token: 0x06009104 RID: 37124 RVA: 0x003B635C File Offset: 0x003B455C
		private void BuildXrayInstances()
		{
			Renderer[] componentsInChildren = base.GetComponentsInChildren<Renderer>(true);
			List<Renderer> list = new List<Renderer>();
			foreach (Renderer renderer in componentsInChildren)
			{
				if (renderer is MeshRenderer || renderer is SkinnedMeshRenderer)
				{
					list.Add(renderer);
				}
			}
			this._xrayRenderers = list.ToArray();
			this._xrayOriginalMats = new Material[this._xrayRenderers.Length][];
			for (int j = 0; j < this._xrayRenderers.Length; j++)
			{
				this._xrayOriginalMats[j] = this._xrayRenderers[j].sharedMaterials;
				foreach (Material material in this._xrayRenderers[j].materials)
				{
					DumpsterHideInteractable.MakeMaterialTransparent(material);
					this._xrayInstances.Add(material);
				}
			}
		}

		// Token: 0x06009105 RID: 37125 RVA: 0x003B6428 File Offset: 0x003B4628
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

		// Token: 0x06009106 RID: 37126 RVA: 0x003B6550 File Offset: 0x003B4750
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

		// Token: 0x06009107 RID: 37127 RVA: 0x003B65B5 File Offset: 0x003B47B5
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

		// Token: 0x06009108 RID: 37128 RVA: 0x003B65D4 File Offset: 0x003B47D4
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

		// Token: 0x06009109 RID: 37129 RVA: 0x003B6670 File Offset: 0x003B4870
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

		// Token: 0x0600910A RID: 37130 RVA: 0x003B6720 File Offset: 0x003B4920
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
				if (material != null)
				{
					Object.Destroy(material);
				}
			}
			this._xrayInstances.Clear();
			this._xrayRenderers = null;
			this._xrayOriginalMats = null;
		}

		// Token: 0x040094E1 RID: 38113
		private const string OpenStateName = "Ciel_DumpsterHide_Open";

		// Token: 0x040094E2 RID: 38114
		private const string EnterStateName = "Ciel_DumpsterHide_Enter";

		// Token: 0x040094E3 RID: 38115
		private const string IdleStateName = "Ciel_DumpsterHide_Idle";

		// Token: 0x040094E4 RID: 38116
		private const string ExitStateName = "Ciel_DumpsterHide_Exit";

		// Token: 0x040094E5 RID: 38117
		private const string EndingTrigger = "DumpsterHideEnding";

		// Token: 0x040094E6 RID: 38118
		private const string DumpClosedState = "Closed";

		// Token: 0x040094E7 RID: 38119
		private const string DumpEnterState = "Enter";

		// Token: 0x040094E8 RID: 38120
		private const string DumpIdleState = "Idle";

		// Token: 0x040094E9 RID: 38121
		private const string DumpExitState = "Exit";

		// Token: 0x040094EA RID: 38122
		[Header("References")]
		[Tooltip("vTriggerGenericAction(s) that drive the hide. Auto-resolved from children if empty.")]
		[SerializeField]
		private vTriggerGenericAction[] triggers;

		// Token: 0x040094EB RID: 38123
		[Tooltip("World-space prompt shown when the player is in range. Auto-resolved from children.")]
		[SerializeField]
		private InteractablePromptUI promptUI;

		// Token: 0x040094EC RID: 38124
		[Tooltip("The single snap point for every approach. The paired Enter trajectory is evaluated from this anchor at the animation's absolute phase.")]
		[SerializeField]
		private Transform enterStart;

		// Token: 0x040094ED RID: 38125
		[Tooltip("Endpoint of the Enter trajectory and root of the hidden idle. There is no separate repositioning after entry.")]
		[SerializeField]
		private Transform hideTarget;

		// Token: 0x040094EE RID: 38126
		[Tooltip("Animator above the Dumpster child, matching the Objz clip paths Dumpster/root/lid1 and Dumpster/root/lid2. Auto-resolved if empty.")]
		[SerializeField]
		private Animator dumpsterAnimator;

		// Token: 0x040094EF RID: 38127
		[Tooltip("Timed mud applied on entry when enabled in Game Settings. Uses the rummage decal.")]
		[SerializeField]
		private SkinDecal hideBodyPaintDecal;

		// Token: 0x040094F0 RID: 38128
		[Header("UI Config")]
		[Tooltip("Prompt text + key shown while OUTSIDE ('Dumpster' / 'Hide' / E). Required.")]
		[SerializeField]
		private InteractionConfigSO enterConfig;

		// Token: 0x040094F1 RID: 38129
		[Tooltip("Right-side HUD config shown WHILE HIDDEN (Exit / E, X-Ray / G). Required for the HUD.")]
		[SerializeField]
		private InteractionConfigSO hiddenHudConfig;

		// Token: 0x040094F2 RID: 38130
		[Header("Exit input")]
		[Tooltip("Input pressed to get out of the dumpster while hidden.")]
		[SerializeField]
		private GenericInput exitInput = new GenericInput("E", "A", "A");

		// Token: 0x040094F3 RID: 38131
		[Header("Exit facing")]
		[Tooltip("Optional extra yaw at the cut to locomotion. Keep zero for the new Char clips: the paired Enter and Exit clips return to the original facing.")]
		[SerializeField]
		private float exitYawFlipDegrees;

		// Token: 0x040094F4 RID: 38132
		[Header("First person while hidden")]
		[Tooltip("Half-range (degrees) the FP look can swing around the way OUT while hidden.")]
		[SerializeField]
		private float fpLookHalfRange = 100f;

		// Token: 0x040094F5 RID: 38133
		[Header("X-ray (see through the dumpster while hidden)")]
		[Tooltip("Toggles the dumpster transparency while hidden. G is free in this context (T = emote wheel, V = first person).")]
		[SerializeField]
		private GenericInput xrayInput = new GenericInput("G", "RightStickClick", "RightStickClick");

		// Token: 0x040094F6 RID: 38134
		[SerializeField]
		[Range(0f, 1f)]
		private float xrayOpacity = 0.3f;

		// Token: 0x040094F7 RID: 38135
		[SerializeField]
		private float xrayFadeDuration = 0.25f;

		// Token: 0x040094F8 RID: 38136
		[Header("Lid reset")]
		[Tooltip("Crossfade seconds back to the authored Closed pose after exit or cancellation.")]
		[SerializeField]
		private float lidResetBlend = 0.7f;

		// Token: 0x040094F9 RID: 38137
		[Header("Lid sound sync")]
		[Tooltip("The lid BONE (root/lid1). The open/close sounds are edge-triggered from this bone's ACTUAL rotation every frame — perfectly synced to whatever clip or blend is moving the lid, no guessed timings. Auto-resolved by name.")]
		[SerializeField]
		private Transform lidBone;

		// Token: 0x040094FA RID: 38138
		[Tooltip("Degrees away from the shut pose beyond which the lid counts as OPEN. The open creak fires crossing it outward, the close clack crossing it back in.")]
		[SerializeField]
		private float lidSoundAngleThreshold = 8f;

		// Token: 0x040094FB RID: 38139
		[Header("Interior side camera (legacy cleanup)")]
		[Tooltip("Dedicated VCam parked inside the dumpster on one side, framing Ciel. Static (no Follow/LookAt): the toggle only raises its priority, BedSleep-style.")]
		[SerializeField]
		private CinemachineVirtualCamera interiorVCam;

		// Token: 0x040094FC RID: 38140
		[Tooltip("CinemachineBrain on the main camera — enabled only while the interior cam is live (Invector drives the camera the rest of the time). Auto-resolved/added.")]
		[SerializeField]
		private CinemachineBrain brain;

		// Token: 0x040094FD RID: 38141
		[Tooltip("Priority while live (must beat any other VCam).")]
		[SerializeField]
		private int cameraLivePriority = 100;

		// Token: 0x040094FE RID: 38142
		[Tooltip("Degrees the interior cam can yaw left/right around its authored framing (mouse X). Fixed surveillance-style shot — it never moves.")]
		[SerializeField]
		private float camYawHalfRange = 30f;

		// Token: 0x040094FF RID: 38143
		[Tooltip("Yaw degrees per unit of mouse X.")]
		[SerializeField]
		private float camYawSensitivity = 3f;

		// Token: 0x04009500 RID: 38144
		[Tooltip("Degrees the cam can pitch up/down around the authored framing (mouse Y) — kept small on purpose.")]
		[SerializeField]
		private float camPitchHalfRange = 12f;

		// Token: 0x04009501 RID: 38145
		[Tooltip("Pitch degrees per unit of mouse Y.")]
		[SerializeField]
		private float camPitchSensitivity = 2.5f;

		// Token: 0x04009502 RID: 38146
		[Header("Audio")]
		[Tooltip("Played when the paired Enter or Exit animation starts opening the lid.")]
		[SerializeField]
		private AudioClip lidOpenSound;

		// Token: 0x04009503 RID: 38147
		[Tooltip("Played when the animated lid reaches its closed pose.")]
		[SerializeField]
		private AudioClip lidCloseSound;

		// Token: 0x04009504 RID: 38148
		[Tooltip("3D source on the dumpster. Auto-resolved from this object if empty.")]
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x04009505 RID: 38149
		[Header("Root motion")]
		[Tooltip("The hide clips carry XZ translation (approach + climb). Enable root motion for the duration so Ciel physically moves into / out of the dumpster.")]
		[SerializeField]
		private bool useRootMotion = true;

		// Token: 0x04009506 RID: 38150
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04009507 RID: 38151
		private GameObject _activePlayer;

		// Token: 0x04009508 RID: 38152
		private PlayerActionAnimator _actionAnimator;

		// Token: 0x04009509 RID: 38153
		private Animator _playerAnimator;

		// Token: 0x0400950A RID: 38154
		private bool _hidden;

		// Token: 0x0400950B RID: 38155
		private bool _exiting;

		// Token: 0x0400950C RID: 38156
		private bool _rootMotionPushed;

		// Token: 0x0400950D RID: 38157
		private bool _emotionPushed;

		// Token: 0x0400950E RID: 38158
		private vThirdPersonInput _rootMotionInput;

		// Token: 0x0400950F RID: 38159
		private float _pairedRootMotionScale = 1f;

		// Token: 0x04009510 RID: 38160
		private Vector3 _pairedLocalPosition;

		// Token: 0x04009511 RID: 38161
		private Quaternion _pairedLocalRotation;

		// Token: 0x04009512 RID: 38162
		private bool _pairedPoseCaptured;

		// Token: 0x04009513 RID: 38163
		private DumpsterRootTrajectory _entryTrajectory;

		// Token: 0x04009514 RID: 38164
		private float _hiddenSince;

		// Token: 0x04009515 RID: 38165
		private bool _entryRejected;

		// Token: 0x04009516 RID: 38166
		private bool _entryPending;

		// Token: 0x04009517 RID: 38167
		private float _entryRequestedAt;

		// Token: 0x04009518 RID: 38168
		private Coroutine _rejectedEntryCleanup;

		// Token: 0x04009519 RID: 38169
		private PlayerActivityLease _hideLease;

		// Token: 0x0400951A RID: 38170
		private bool _ownsFirstPersonView;

		// Token: 0x0400951B RID: 38171
		private TriggerInteractionCandidate _interactionCandidate;

		// Token: 0x0400951C RID: 38172
		private vGenericAction _playerGenericAction;

		// Token: 0x0400951D RID: 38173
		private bool _genericActionSubscribed;

		// Token: 0x0400951E RID: 38174
		private vThirdPersonController _lockedController;

		// Token: 0x0400951F RID: 38175
		private bool _controlsLocked;

		// Token: 0x04009520 RID: 38176
		private FirstPersonController _fpController;

		// Token: 0x04009521 RID: 38177
		private Coroutine _mirrorRoutine;

		// Token: 0x04009522 RID: 38178
		private Coroutine _idleWatch;

		// Token: 0x04009523 RID: 38179
		private string _mirroredDumpState;

		// Token: 0x04009524 RID: 38180
		private Quaternion _lidShutRefRot;

		// Token: 0x04009525 RID: 38181
		private bool _lidRefCaptured;

		// Token: 0x04009526 RID: 38182
		private bool _lidSoundOpen;

		// Token: 0x04009527 RID: 38183
		private float _lastLidSoundTime;

		// Token: 0x04009528 RID: 38184
		private bool _camOn;

		// Token: 0x04009529 RID: 38185
		private Transform _mainCamT;

		// Token: 0x0400952A RID: 38186
		private Vector3 _mainCamLocalPos;

		// Token: 0x0400952B RID: 38187
		private Quaternion _mainCamLocalRot;

		// Token: 0x0400952C RID: 38188
		private bool _mainCamLocalSaved;

		// Token: 0x0400952D RID: 38189
		private Quaternion _vcamBaseLocalRot;

		// Token: 0x0400952E RID: 38190
		private bool _vcamBaseSaved;

		// Token: 0x0400952F RID: 38191
		private float _camYawOffset;

		// Token: 0x04009530 RID: 38192
		private float _camPitchOffset;

		// Token: 0x04009531 RID: 38193
		private Quaternion _vcamLastWrittenRot;

		// Token: 0x04009532 RID: 38194
		private bool _xrayOn;

		// Token: 0x04009533 RID: 38195
		private Coroutine _xrayFade;

		// Token: 0x04009534 RID: 38196
		private Renderer[] _xrayRenderers;

		// Token: 0x04009535 RID: 38197
		private Material[][] _xrayOriginalMats;

		// Token: 0x04009536 RID: 38198
		private readonly List<Material> _xrayInstances = new List<Material>();

		// Token: 0x04009537 RID: 38199
		private bool _entryPoseCaptured;

		// Token: 0x04009538 RID: 38200
		private Vector3 _entryPosition;

		// Token: 0x04009539 RID: 38201
		private Quaternion _entryRotation;
	}
}
