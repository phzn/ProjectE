using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Dinopunch.LivingMud;
using Features.Detection;
using Features.Inventory;
using Features.Inventory.Placement;
using Features.Missions;
using Features.Rank;
using Features.RunModes;
using Features.Tutorials;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using ProjectExhibitionist;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.FirstPerson;
using ProjectExhibitionist.Interactions;
using UnityEngine;
using UnityEngine.Events;

namespace Features.TrashcanLoot
{
	// Token: 0x02000BE7 RID: 3047
	[DisallowMultipleComponent]
	public class TrashcanLootInteractable : MonoBehaviour
	{
		// Token: 0x0600578B RID: 22411 RVA: 0x00222C88 File Offset: 0x00220E88
		internal Object ReadUniversityGameplayReference(UniversityGameplayField field)
		{
			switch (field)
			{
			case UniversityGameplayField.TrashcanLootInteractable_ejectSFX:
				return this.ejectSFX;
			case UniversityGameplayField.TrashcanLootInteractable_lootBodyPaintDecal:
				return this.lootBodyPaintDecal;
			case UniversityGameplayField.TrashcanLootInteractable_lootProfile:
				return this.lootProfile;
			case UniversityGameplayField.TrashcanLootInteractable_progressUiPrefab:
				return this.progressUiPrefab;
			case UniversityGameplayField.TrashcanLootInteractable_promptConfig:
				return this.promptConfig;
			case UniversityGameplayField.TrashcanLootInteractable_rummageHudConfig:
				return this.rummageHudConfig;
			case UniversityGameplayField.TrashcanLootInteractable_searchLoopSFX:
				return this.searchLoopSFX;
			default:
				throw new InvalidOperationException("Unsupported TrashcanLootInteractable gameplay binding: " + field.ToString());
			}
		}

		// Token: 0x0600578C RID: 22412 RVA: 0x00222D0C File Offset: 0x00220F0C
		internal void WriteUniversityGameplayReference(UniversityGameplayField field, Object asset)
		{
			switch (field)
			{
			case UniversityGameplayField.TrashcanLootInteractable_ejectSFX:
				this.ejectSFX = (AudioClip)asset;
				return;
			case UniversityGameplayField.TrashcanLootInteractable_lootBodyPaintDecal:
				this.lootBodyPaintDecal = (SkinDecal)asset;
				return;
			case UniversityGameplayField.TrashcanLootInteractable_lootProfile:
				this.lootProfile = (TrashcanLootProfileSO)asset;
				return;
			case UniversityGameplayField.TrashcanLootInteractable_progressUiPrefab:
				this.progressUiPrefab = (TrashcanLootProgressUI)asset;
				return;
			case UniversityGameplayField.TrashcanLootInteractable_promptConfig:
				this.promptConfig = (InteractionConfigSO)asset;
				return;
			case UniversityGameplayField.TrashcanLootInteractable_rummageHudConfig:
				this.rummageHudConfig = (InteractionConfigSO)asset;
				return;
			case UniversityGameplayField.TrashcanLootInteractable_searchLoopSFX:
				this.searchLoopSFX = (AudioClip)asset;
				return;
			default:
				throw new InvalidOperationException("Unsupported TrashcanLootInteractable gameplay binding: " + field.ToString());
			}
		}

		// Token: 0x17000E87 RID: 3719
		// (get) Token: 0x0600578D RID: 22413 RVA: 0x00222DB7 File Offset: 0x00220FB7
		public bool IsRummaging
		{
			get
			{
				return this._rummaging;
			}
		}

		// Token: 0x17000E88 RID: 3720
		// (get) Token: 0x0600578E RID: 22414 RVA: 0x00222DBF File Offset: 0x00220FBF
		public bool HasActiveResidencyInteraction
		{
			get
			{
				return this._rummaging || this._pendingStartedByHold || this._rummageLease != null;
			}
		}

		// Token: 0x17000E89 RID: 3721
		// (get) Token: 0x0600578F RID: 22415 RVA: 0x00222DDC File Offset: 0x00220FDC
		public bool IsLooted
		{
			get
			{
				return this._looted;
			}
		}

		// Token: 0x17000E8A RID: 3722
		// (get) Token: 0x06005790 RID: 22416 RVA: 0x00222DE4 File Offset: 0x00220FE4
		private bool HideSiblingBusy
		{
			get
			{
				return this._hideSibling != null && this._hideSibling.IsBusy;
			}
		}

		// Token: 0x17000E8B RID: 3723
		// (get) Token: 0x06005791 RID: 22417 RVA: 0x00222E01 File Offset: 0x00221001
		private bool HasHideInteraction
		{
			get
			{
				return this._hideSibling != null && this._hideSibling.enabled;
			}
		}

		// Token: 0x17000E8C RID: 3724
		// (get) Token: 0x06005792 RID: 22418 RVA: 0x00222E1E File Offset: 0x0022101E
		private static bool LootBlocked
		{
			get
			{
				return GuidedRankGameplayPolicy.AdvancedMechanicsBlocked;
			}
		}

		// Token: 0x17000E8D RID: 3725
		// (get) Token: 0x06005793 RID: 22419 RVA: 0x00222E25 File Offset: 0x00221025
		private bool CanOfferIdlePrompt
		{
			get
			{
				return this.CanOfferIdlePromptForAvailability(TrashcanLootInteractable.LootBlocked);
			}
		}

		// Token: 0x06005794 RID: 22420 RVA: 0x00222E32 File Offset: 0x00221032
		private bool CanOfferIdlePromptForAvailability(bool lootBlocked)
		{
			return !this._rummaging && !this.HideSiblingBusy && (this.HasHideInteraction || (!this._looted && !lootBlocked));
		}

		// Token: 0x06005795 RID: 22421 RVA: 0x00222E60 File Offset: 0x00221060
		private void Awake()
		{
			ControllerContextBindings.SetLabel(this.rummageHudConfig, "F", "Y", false);
			ControllerContextBindings.Set(this.switchCameraInput, "Y");
			if (this.triggers == null || this.triggers.Length == 0)
			{
				this.triggers = base.GetComponentsInChildren<vTriggerGenericAction>(true);
			}
			if (this.promptUI == null)
			{
				this.promptUI = base.GetComponentInChildren<InteractablePromptUI>(true);
			}
			if (this.rummageAnchor == null)
			{
				foreach (Transform transform in base.GetComponentsInChildren<Transform>(true))
				{
					if (transform.name == "RummageAnchor")
					{
						this.rummageAnchor = transform;
						break;
					}
				}
			}
			if (this.binAnimator == null)
			{
				foreach (Transform transform2 in base.GetComponentsInChildren<Transform>(true))
				{
					if (transform2.name == "WasteCanRig")
					{
						this.binAnimator = transform2.GetComponentInChildren<Animator>(true);
						break;
					}
				}
			}
			if (this.rummageCamera != null)
			{
				this.rummageCamera.gameObject.SetActive(false);
			}
			this._hideSibling = base.GetComponent<WasteCanHideInteractable>();
			this._validateListener = new UnityAction<GameObject>(this.HandleValidate);
			this._invalidateListener = new UnityAction<GameObject>(this.HandleInvalidate);
			this._animationStartListener = new UnityAction(this.HandleAnimationStart);
			this._cancelledListener = new UnityAction(this.HandleCancelled);
			if (this.triggers == null || this.triggers.Length == 0)
			{
				Debug.LogError("[TrashcanLoot] No vTriggerGenericAction in the hierarchy of '" + base.name + "'.", this);
			}
			if (this.triggers != null)
			{
				foreach (vTriggerGenericAction vTriggerGenericAction in this.triggers)
				{
					if (vTriggerGenericAction != null && vTriggerGenericAction.actionInput != null)
					{
						vTriggerGenericAction.actionInput.useInput = false;
					}
				}
			}
			if (this.promptUI != null)
			{
				this.promptUI.Hide();
			}
		}

		// Token: 0x06005796 RID: 22422 RVA: 0x00223058 File Offset: 0x00221258
		private void OnDestroy()
		{
			if (this._poiResidencyRegistered)
			{
				TrashcanTierRandomizer.ForgetDestroyedScopedCan(this);
			}
			this.CancelPendingLidReset();
			if (this._tripodHudConfig != null)
			{
				if (Application.isPlaying)
				{
					Object.Destroy(this._tripodHudConfig);
				}
				else
				{
					Object.DestroyImmediate(this._tripodHudConfig);
				}
				this._tripodHudConfig = null;
			}
			if (this._hideOnlyPromptConfig != null)
			{
				if (Application.isPlaying)
				{
					Object.Destroy(this._hideOnlyPromptConfig);
				}
				else
				{
					Object.DestroyImmediate(this._hideOnlyPromptConfig);
				}
				this._hideOnlyPromptConfig = null;
			}
		}

		// Token: 0x06005797 RID: 22423 RVA: 0x002230E4 File Offset: 0x002212E4
		private void OnEnable()
		{
			PoiResidencyStateKey poiResidencyStateKey;
			this._poiResidencyRegistered |= PoiResidencyPlacement.TryGetKey(this, "trashcan-looted", out poiResidencyStateKey);
			TrashcanTierRandomizer.Register(this);
			StreamedWorldPropView component = base.GetComponent<StreamedWorldPropView>();
			if (component == null || !component.ResumingSamePlacement)
			{
				this.ResetLootStateCore(false);
			}
			if (this.triggers == null)
			{
				return;
			}
			foreach (vTriggerGenericAction vTriggerGenericAction in this.triggers)
			{
				if (!(vTriggerGenericAction == null))
				{
					vTriggerGenericAction.OnValidate.AddListener(this._validateListener);
					vTriggerGenericAction.OnInvalidate.AddListener(this._invalidateListener);
					vTriggerGenericAction.OnPlayerExit.AddListener(this._invalidateListener);
					vTriggerGenericAction.OnStartAnimation.AddListener(this._animationStartListener);
					vTriggerGenericAction.OnCancelAction.AddListener(this._cancelledListener);
				}
			}
		}

		// Token: 0x06005798 RID: 22424 RVA: 0x002231B7 File Offset: 0x002213B7
		public void ResetLootState()
		{
			this.ResetLootStateCore(true);
		}

		// Token: 0x06005799 RID: 22425 RVA: 0x002231C0 File Offset: 0x002213C0
		private void ResetLootStateCore(bool forgetPoiState)
		{
			if (this._rummaging)
			{
				return;
			}
			if (forgetPoiState)
			{
				PoiResidencyPlacement.WriteFlag(this, "trashcan-looted", false);
			}
			this.CancelPendingRummageStart(true);
			this._finishingSelf = false;
			this._looted = !forgetPoiState && PoiResidencyPlacement.ReadFlag(this, "trashcan-looted");
			this._holding = false;
			this._holdConsumed = false;
			this._holdTimer = 0f;
			this._startedByHold = false;
			this._endRequested = false;
			this._spawnedCount = 0;
			if (this._legacyTriggerObjectsDisabledForLoot)
			{
				this._legacyTriggerObjectsDisabledForLoot = false;
				if (this.triggers != null)
				{
					foreach (vTriggerGenericAction vTriggerGenericAction in this.triggers)
					{
						if (vTriggerGenericAction != null)
						{
							vTriggerGenericAction.gameObject.SetActive(true);
						}
					}
				}
			}
			this._activePlayer = null;
			this._playerGenericAction = null;
			if (this.promptUI != null)
			{
				this.promptUI.Hide();
			}
			if (this._lidCloseCo != null)
			{
				base.StopCoroutine(this._lidCloseCo);
				this._lidCloseCo = null;
			}
			this.ResetLidPoseWhenReady();
		}

		// Token: 0x0600579A RID: 22426 RVA: 0x002232C8 File Offset: 0x002214C8
		private void OnDisable()
		{
			this.CancelPendingLidReset();
			if (this.triggers != null)
			{
				foreach (vTriggerGenericAction vTriggerGenericAction in this.triggers)
				{
					if (!(vTriggerGenericAction == null))
					{
						vTriggerGenericAction.OnValidate.RemoveListener(this._validateListener);
						vTriggerGenericAction.OnInvalidate.RemoveListener(this._invalidateListener);
						vTriggerGenericAction.OnPlayerExit.RemoveListener(this._invalidateListener);
						vTriggerGenericAction.OnStartAnimation.RemoveListener(this._animationStartListener);
						vTriggerGenericAction.OnCancelAction.RemoveListener(this._cancelledListener);
					}
				}
			}
			if (this._rummaging)
			{
				this.AbortRummage(true);
			}
			else
			{
				this.CancelPendingRummageStart(true);
			}
			this._activePlayer = null;
			this._playerGenericAction = null;
			this.HideRummageHud();
			this.HidePromptPresenter();
		}

		// Token: 0x0600579B RID: 22427 RVA: 0x0022338C File Offset: 0x0022158C
		private void ResetLidPoseWhenReady()
		{
			if (this.binAnimator == null)
			{
				this.CancelPendingLidReset();
				return;
			}
			if (this.binAnimator.isActiveAndEnabled)
			{
				this.CancelPendingLidReset();
				this.RestoreCommittedLidPose();
				return;
			}
			HeavyZoneStreamingProxy componentInParent = base.GetComponentInParent<HeavyZoneStreamingProxy>();
			if (componentInParent == null || componentInParent.IsOperationalReady)
			{
				this.CancelPendingLidReset();
				return;
			}
			if (this._pendingLidResetProxy == componentInParent)
			{
				return;
			}
			this.CancelPendingLidReset();
			this._pendingLidResetProxy = componentInParent;
			componentInParent.OperationalReady += this.HandleHeavyZoneOperationalReadyForLidReset;
		}

		// Token: 0x0600579C RID: 22428 RVA: 0x00223415 File Offset: 0x00221615
		private void HandleHeavyZoneOperationalReadyForLidReset(HeavyZoneStreamingProxy proxy)
		{
			if (proxy != this._pendingLidResetProxy)
			{
				return;
			}
			this.CancelPendingLidReset();
			if (this.binAnimator != null && this.binAnimator.isActiveAndEnabled)
			{
				this.RestoreCommittedLidPose();
			}
		}

		// Token: 0x0600579D RID: 22429 RVA: 0x0022344D File Offset: 0x0022164D
		private void RestoreCommittedLidPose()
		{
			this.binAnimator.Play(this._looted ? "WasteCan_Open" : "WasteCan_Close", 0, this._looted ? 1f : 0f);
		}

		// Token: 0x0600579E RID: 22430 RVA: 0x00223484 File Offset: 0x00221684
		private void CancelPendingLidReset()
		{
			HeavyZoneStreamingProxy pendingLidResetProxy = this._pendingLidResetProxy;
			this._pendingLidResetProxy = null;
			if (pendingLidResetProxy != null)
			{
				pendingLidResetProxy.OperationalReady -= this.HandleHeavyZoneOperationalReadyForLidReset;
			}
		}

		// Token: 0x0600579F RID: 22431 RVA: 0x002234BC File Offset: 0x002216BC
		private void HandleValidate(GameObject player)
		{
			this._activePlayer = player;
			if (this._playerGenericAction == null && player != null)
			{
				this._playerGenericAction = player.GetComponentInChildren<vGenericAction>();
			}
			if (this.CanOfferIdlePrompt && this.promptUI != null)
			{
				this.ShowPrompt();
			}
		}

		// Token: 0x060057A0 RID: 22432 RVA: 0x0022350F File Offset: 0x0022170F
		private void HandleInvalidate(GameObject player)
		{
			this.HidePromptPresenter();
		}

		// Token: 0x060057A1 RID: 22433 RVA: 0x00223518 File Offset: 0x00221718
		private void HandleAnimationStart()
		{
			if (this._rummaging || this._finishingSelf)
			{
				return;
			}
			if (this._looted || TrashcanLootInteractable.LootBlocked)
			{
				this.CancelPendingRummageStart(true);
				return;
			}
			if (!this.TryAcquireRummageContext())
			{
				this.CancelPendingRummageStart(false);
				vGenericAction vGenericAction = ((this._playerGenericAction != null) ? this._playerGenericAction : ((this._actionAnimator != null) ? this._actionAnimator.GenericAction : null));
				this.ResetOwnedGenericAction(vGenericAction);
				return;
			}
			this._rummaging = true;
			this._looted = true;
			this._spawnedCount = 0;
			this._endRequested = false;
			this._finishingSelf = false;
			this._suppressionWarned = false;
			this._startedByHold = this._pendingStartedByHold;
			this._pendingStartedByHold = false;
			this._rummageStartTime = Time.time;
			this.ResolvePlayerRefs();
			this._layerIdxResolved = false;
			if (this._playerAnimator != null)
			{
				this._playerAnimator.ResetTrigger("TrashRummageEnding");
			}
			Transform transform = this.ResolvePlayerTransform();
			if (transform != null)
			{
				this.CaptureEntryPose();
				this._rummageStealthActor = transform.GetComponentInChildren<StealthActor>();
				Transform transform2 = ((this._playerAnimator != null && this._playerAnimator.isHuman) ? (this._playerAnimator.GetBoneTransform(HumanBodyBones.LeftFoot) ?? this._playerAnimator.GetBoneTransform(HumanBodyBones.LeftLowerLeg)) : transform);
				Transform transform3 = ((this._playerAnimator != null && this._playerAnimator.isHuman) ? (this._playerAnimator.GetBoneTransform(HumanBodyBones.RightFoot) ?? this._playerAnimator.GetBoneTransform(HumanBodyBones.RightLowerLeg)) : null);
				StealthActor rummageStealthActor = this._rummageStealthActor;
				if (rummageStealthActor != null)
				{
					rummageStealthActor.SetVisibilityPointOverride(this, transform2, transform3, base.transform);
				}
			}
			if (this._playerGenericAction != null)
			{
				this._playerGenericAction.DisablePlayerGravityAndCollision();
			}
			this.SnapPlayerToAnchor();
			this.LockControls(true);
			this.BindAnchorUpdates();
			if (this._fpController != null && !FullBodyHideActivityContext.IsUsingTripodCamera(this._rummageLease))
			{
				this._fpController.BeginInteractionView(this, true, 80f, true, false, null, null, null);
				this._interactionViewPushed = true;
			}
			if (this._actionAnimator != null)
			{
				this._actionAnimator.DisableEmotionLayers();
				this._emotionPushed = true;
			}
			this.PlayBinOpen();
			this.StartSearchLoop();
			this.ApplyLootBodyPaint();
			this.HidePromptPresenter();
			this.ShowRummageHud();
			if (this._rummageRoutine != null)
			{
				base.StopCoroutine(this._rummageRoutine);
			}
			this._rummageRoutine = base.StartCoroutine(this.RummageRoutine());
			if (this.debugLogs)
			{
				Debug.Log("[TrashcanLoot] Rummage started.", this);
			}
		}

		// Token: 0x060057A2 RID: 22434 RVA: 0x002237AD File Offset: 0x002219AD
		private void Update()
		{
			this.UpdateForLootAvailability(TrashcanLootInteractable.LootBlocked);
		}

		// Token: 0x060057A3 RID: 22435 RVA: 0x002237BC File Offset: 0x002219BC
		private void UpdateForLootAvailability(bool lootBlocked)
		{
			if (lootBlocked != this._lastLootAvailabilityBlocked)
			{
				this._lastLootAvailabilityBlocked = lootBlocked;
				this.HidePromptPresenter();
			}
			if (lootBlocked)
			{
				if (this._rummaging)
				{
					this.AbortRummage(true);
				}
				else if (this._pendingStartedByHold)
				{
					this.CancelPendingRummageStart(true);
				}
				this._holding = false;
				this._holdTimer = 0f;
				if (!this.HasHideInteraction || this.HideSiblingBusy)
				{
					this.HidePromptPresenter();
					return;
				}
				this.SelfHealValidation();
				this.SelfHealInvalidation();
				this.RepairMissedTriggerRegistration();
				return;
			}
			else
			{
				if (!this._rummaging)
				{
					this.SelfHealValidation();
					this.SelfHealInvalidation();
					this.RepairMissedTriggerRegistration();
					this.UpdateHoldToStart();
					return;
				}
				if (!this._endRequested && !FullBodyHideActivityContext.IsUsingTripodCamera(this._rummageLease) && this.switchCameraInput != null && this.switchCameraInput.GetButtonDown() && InteractionCandidateRegistry.TryConsumeContextInput(this.switchCameraInput))
				{
					this.SetRummageCamera(!this._rummageCamActive);
				}
				if (this._endRequested || !this._startedByHold)
				{
					return;
				}
				if (this.interactInput != null && !this.interactInput.GetButton())
				{
					this._endRequested = true;
				}
				return;
			}
		}

		// Token: 0x060057A4 RID: 22436 RVA: 0x002238D4 File Offset: 0x00221AD4
		private void RepairMissedTriggerRegistration()
		{
			if (!this.CanOfferIdlePrompt || this._rummaging || Time.time < this._nextTriggerRepairAt)
			{
				return;
			}
			this._nextTriggerRepairAt = Time.time + 0.2f + (float)(base.GetInstanceID() & 15) * 0.01f;
			Transform player = PlayerRegistry.Player;
			if (!(player == null))
			{
				Vector3 vector = player.position - base.transform.position;
				if (vector.sqrMagnitude <= 16f)
				{
					vGenericAction vGenericAction = ((this._playerGenericAction != null) ? this._playerGenericAction : player.GetComponentInChildren<vGenericAction>());
					if (vGenericAction == null || vGenericAction.isLockTriggerEvents || vGenericAction.doingAction)
					{
						return;
					}
					CapsuleCollider component = vGenericAction.GetComponent<CapsuleCollider>();
					if (component == null || !component.enabled || !component.gameObject.activeInHierarchy)
					{
						return;
					}
					this._playerGenericAction = vGenericAction;
					if (this.triggers == null)
					{
						return;
					}
					foreach (vTriggerGenericAction vTriggerGenericAction in this.triggers)
					{
						if (!(vTriggerGenericAction == null) && vTriggerGenericAction.isActiveAndEnabled)
						{
							Collider component2 = vTriggerGenericAction.GetComponent<Collider>();
							float num;
							if (!(component2 == null) && component2.enabled && component2.gameObject.activeInHierarchy && Physics.ComputePenetration(component, component.transform.position, component.transform.rotation, component2, component2.transform.position, component2.transform.rotation, out vector, out num))
							{
								vGenericAction.OnActionEnter(component2);
								vGenericAction.OnActionStay(component2);
							}
						}
					}
					return;
				}
			}
		}

		// Token: 0x060057A5 RID: 22437 RVA: 0x00223A74 File Offset: 0x00221C74
		private void SelfHealValidation()
		{
			if (!this.CanOfferIdlePrompt)
			{
				return;
			}
			if (this.promptUI == null || this.promptUI.gameObject.activeSelf)
			{
				return;
			}
			vGenericAction vGenericAction = this._playerGenericAction;
			if (vGenericAction == null)
			{
				Transform player = PlayerRegistry.Player;
				if (player == null)
				{
					return;
				}
				vGenericAction = player.GetComponentInChildren<vGenericAction>();
				if (vGenericAction == null)
				{
					return;
				}
				this._playerGenericAction = vGenericAction;
			}
			if (vGenericAction.triggerAction == null || !this.IsOwnedTrigger(vGenericAction.triggerAction))
			{
				return;
			}
			this.HandleValidate(vGenericAction.gameObject);
		}

		// Token: 0x060057A6 RID: 22438 RVA: 0x00223B0C File Offset: 0x00221D0C
		private void SelfHealInvalidation()
		{
			if (this._rummaging)
			{
				return;
			}
			if (this.promptUI == null || !this.promptUI.gameObject.activeSelf)
			{
				return;
			}
			vGenericAction playerGenericAction = this._playerGenericAction;
			if (playerGenericAction == null)
			{
				return;
			}
			if (playerGenericAction.triggerAction != null && this.IsOwnedTrigger(playerGenericAction.triggerAction))
			{
				return;
			}
			this.HidePromptPresenter();
		}

		// Token: 0x060057A7 RID: 22439 RVA: 0x00223B78 File Offset: 0x00221D78
		private void UpdateHoldToStart()
		{
			if (TrashcanLootInteractable.LootBlocked)
			{
				return;
			}
			if (this._looted || this.HideSiblingBusy || this._activePlayer == null)
			{
				return;
			}
			if (this.promptUI == null || !this.promptUI.gameObject.activeSelf)
			{
				return;
			}
			if (this.interactInput != null && this.interactInput.GetButtonDown())
			{
				string text;
				if (this.TryGetBusyWarning(out text))
				{
					BlockedActionFeedback.Show(text);
					return;
				}
				this._holding = true;
				this._holdConsumed = false;
				this._holdTimer = 0f;
			}
			if (!this._holding)
			{
				return;
			}
			if (this.interactInput == null || this.interactInput.GetButtonUp())
			{
				this._holding = false;
				return;
			}
			this._holdTimer += Time.deltaTime;
			if (this._holdConsumed || this._holdTimer < this.holdToRummageDuration)
			{
				return;
			}
			if (this._playerAnimator == null)
			{
				this.ResolvePlayerRefs();
			}
			if (this._playerAnimator != null && this._playerAnimator.IsInTransition(0))
			{
				return;
			}
			this._holdConsumed = true;
			this._holding = false;
			if (Time.time - InventoryPickup.LastInteractTime < this.holdToRummageDuration + 0.2f)
			{
				return;
			}
			this.TryStartRummage();
		}

		// Token: 0x060057A8 RID: 22440 RVA: 0x00223CBC File Offset: 0x00221EBC
		private void TryStartRummage()
		{
			if (TrashcanLootInteractable.LootBlocked)
			{
				return;
			}
			if (TutorialService.TryShowFirstUse("looting"))
			{
				return;
			}
			vGenericAction vGenericAction = this._playerGenericAction;
			if (vGenericAction == null && this._activePlayer != null)
			{
				vGenericAction = (this._playerGenericAction = this._activePlayer.GetComponentInChildren<vGenericAction>());
			}
			if (vGenericAction == null)
			{
				return;
			}
			if (vGenericAction.triggerAction == null || !this.IsOwnedTrigger(vGenericAction.triggerAction))
			{
				return;
			}
			if (!vGenericAction.actionConditions)
			{
				return;
			}
			InteractionCandidateRegistry.MarkContextInputConsumed(this.interactInput);
			if (!this.TryAcquireRummageContext())
			{
				BlockedActionFeedback.Show("Stop what you're doing first");
				return;
			}
			this._pendingStartedByHold = true;
			this._finishingSelf = false;
			this.PrepareRummageRootMotion();
			this.CaptureEntryPose();
			int num = this._pendingStartSerial + 1;
			this._pendingStartSerial = num;
			int num2 = num;
			vGenericAction.TriggerActionEvents();
			if (this._rummageLease == null || !this._rummageLease.IsActive)
			{
				this.CancelPendingRummageStart(true);
				return;
			}
			vGenericAction.TriggerAnimation();
			base.StartCoroutine(this.RummageStartWatchdog(vGenericAction, num2));
		}

		// Token: 0x060057A9 RID: 22441 RVA: 0x00223DC4 File Offset: 0x00221FC4
		private bool TryAcquireRummageContext()
		{
			return FullBodyHideActivityContext.TryAcquire("TrashcanRummage#" + base.GetInstanceID().ToString(), this, ref this._rummageLease, PlayerActivity.AllPlayerActions, false, true);
		}

		// Token: 0x060057AA RID: 22442 RVA: 0x00223DFC File Offset: 0x00221FFC
		private void PrepareRummageRootMotion()
		{
			if (!this.useRootMotion || this._rootMotionPushed)
			{
				return;
			}
			this.ResolvePlayerRefs();
			if (this._actionAnimator == null)
			{
				return;
			}
			this._actionAnimator.EnableRootMotionForAction();
			this._rootMotionPushed = true;
		}

		// Token: 0x060057AB RID: 22443 RVA: 0x00223E36 File Offset: 0x00222036
		private IEnumerator RummageStartWatchdog(vGenericAction ga, int startSerial)
		{
			float t = 0f;
			while (t < 2f)
			{
				if (startSerial != this._pendingStartSerial)
				{
					yield break;
				}
				if (this._rummaging)
				{
					yield break;
				}
				if (ga == null || !ga.doingAction)
				{
					this.CancelPendingRummageStart(false);
					yield break;
				}
				t += Time.deltaTime;
				yield return null;
			}
			if (this._rummaging)
			{
				yield break;
			}
			if (startSerial != this._pendingStartSerial)
			{
				yield break;
			}
			if (ga == null)
			{
				this.CancelPendingRummageStart(false);
				yield break;
			}
			this.CompleteRummageStartWatchdog(ga, startSerial);
			yield break;
		}

		// Token: 0x060057AC RID: 22444 RVA: 0x00223E54 File Offset: 0x00222054
		private void CompleteRummageStartWatchdog(vGenericAction ga, int startSerial)
		{
			bool flag = ga.doingAction && ga.playingAnimation;
			if (this._rummaging || startSerial != this._pendingStartSerial)
			{
				return;
			}
			if (ga.doingAction && !flag && this.IsOwnedTrigger(ga.triggerAction))
			{
				Debug.LogWarning("[TrashcanLoot] Azione avviata ma animazione mai partita (race Invector) - FinishAction di recupero.", this);
				ga.FinishAction();
				this.ResetOwnedGenericAction(ga);
			}
			this.CancelPendingRummageStart(false);
		}

		// Token: 0x060057AD RID: 22445 RVA: 0x00223EC0 File Offset: 0x002220C0
		private void CancelPendingRummageStart(bool resetOwnedGenericAction = false)
		{
			this._pendingStartSerial++;
			this._finishingSelf = true;
			if (resetOwnedGenericAction)
			{
				this.ResetOwnedGenericAction(this._playerGenericAction);
			}
			this._pendingStartedByHold = false;
			if (!this._rummaging)
			{
				this._hasPreRummagePose = false;
				vGenericAction vGenericAction = ((this._playerGenericAction != null) ? this._playerGenericAction : ((this._actionAnimator != null) ? this._actionAnimator.GenericAction : null));
				bool flag = vGenericAction != null && vGenericAction.doingAction && !this.IsOwnedTrigger(vGenericAction.triggerAction);
				if (this._rootMotionPushed && this._actionAnimator != null && !flag)
				{
					this._actionAnimator.RestoreRootMotionForAction();
				}
				this._rootMotionPushed = false;
			}
			FullBodyHideActivityContext.Release(ref this._rummageLease);
		}

		// Token: 0x060057AE RID: 22446 RVA: 0x00223F98 File Offset: 0x00222198
		private void ResetOwnedGenericAction(vGenericAction genericAction)
		{
			if (genericAction == null || !genericAction.doingAction || !this.IsOwnedTrigger(genericAction.triggerAction))
			{
				return;
			}
			genericAction.FinishAction();
			if (genericAction.doingAction && this.IsOwnedTrigger(genericAction.triggerAction))
			{
				genericAction.ResetTriggerSettings(true);
			}
		}

		// Token: 0x060057AF RID: 22447 RVA: 0x00223FE8 File Offset: 0x002221E8
		private bool TryGetBusyWarning(out string warning)
		{
			PlayerInventoryManager instance = PlayerInventoryManager.Instance;
			if (instance != null && instance.GetActiveItem() != null)
			{
				warning = "Put away what you're holding first";
				return true;
			}
			warning = null;
			return false;
		}

		// Token: 0x060057B0 RID: 22448 RVA: 0x00224020 File Offset: 0x00222220
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

		// Token: 0x060057B1 RID: 22449 RVA: 0x00224068 File Offset: 0x00222268
		private void LateUpdate()
		{
			this.HoldAtAnchor();
			if (this._rummageCamActive && this.rummageCamera != null && this.aimAtPlayerHead)
			{
				this.AimRummageCamera();
			}
			if (!this._rummaging || !this._emotionPushed)
			{
				return;
			}
			PlayerEmotionController instance = PlayerEmotionController.Instance;
			if (instance != null && !instance.EmotionLayersDisabledForAction)
			{
				if (this._actionAnimator != null)
				{
					this._actionAnimator.DisableEmotionLayers();
				}
				if (!this._suppressionWarned)
				{
					this._suppressionWarned = true;
					Debug.LogWarning("[TrashcanLoot] Emotion-layer suppression was released mid-rummage by another system - re-asserted.", this);
				}
			}
			if (this._playerAnimator == null)
			{
				return;
			}
			if (!this._layerIdxResolved)
			{
				string text = ((instance != null) ? instance.ConfidentLayerName : "Confident");
				string text2 = ((instance != null) ? instance.EmbarrassedLayerName : "Embarassed");
				this._confidentLayerIdx = this._playerAnimator.GetLayerIndex(text);
				this._embarrassedLayerIdx = this._playerAnimator.GetLayerIndex(text2);
				this._layerIdxResolved = true;
			}
			if (this._confidentLayerIdx >= 0 && this._playerAnimator.GetLayerWeight(this._confidentLayerIdx) > 0f)
			{
				this._playerAnimator.SetLayerWeight(this._confidentLayerIdx, 0f);
			}
			if (this._embarrassedLayerIdx >= 0 && this._playerAnimator.GetLayerWeight(this._embarrassedLayerIdx) > 0f)
			{
				this._playerAnimator.SetLayerWeight(this._embarrassedLayerIdx, 0f);
			}
		}

		// Token: 0x060057B2 RID: 22450 RVA: 0x002241D8 File Offset: 0x002223D8
		private void HandleCancelled()
		{
			if (this._finishingSelf)
			{
				return;
			}
			if (!this._rummaging)
			{
				this.CancelPendingRummageStart(false);
				return;
			}
			if (this.debugLogs)
			{
				Debug.Log("[TrashcanLoot] Action cancelled → restoring.", this);
			}
			this.AbortRummage(false);
		}

		// Token: 0x060057B3 RID: 22451 RVA: 0x0022420D File Offset: 0x0022240D
		private IEnumerator RummageRoutine()
		{
			TrashcanLootInteractable.<>c__DisplayClass140_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.loot = ((this.lootProfile != null) ? this.lootProfile.RollLoot() : new List<TrashcanLootProfileSO.RolledLoot>());
			if (this.lootProfile == null)
			{
				Debug.LogWarning("[TrashcanLoot] No loot profile assigned - rummage yields nothing.", this);
			}
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[TrashcanLoot] Rolled {0} item(s): ", CS$<>8__locals1.loot.Count) + string.Join(", ", CS$<>8__locals1.loot.ConvertAll<string>((TrashcanLootProfileSO.RolledLoot i) => string.Format("{0}({1})", i.item.itemName, i.rarity))), this);
			}
			this.ShowProgressUI(CS$<>8__locals1.loot.Count);
			CS$<>8__locals1.spawned = 0;
			float startShare = this.progressStartPhaseShare;
			float t0 = Time.time;
			while (this._playerAnimator != null && Time.time - t0 < 8f && !this._endRequested)
			{
				AnimatorStateInfo currentAnimatorStateInfo = this._playerAnimator.GetCurrentAnimatorStateInfo(0);
				if (currentAnimatorStateInfo.IsName("Ciel_Rummage_Loop"))
				{
					break;
				}
				if (currentAnimatorStateInfo.IsName("Ciel_Rummage_Start"))
				{
					float num = Mathf.Clamp01(currentAnimatorStateInfo.normalizedTime) * startShare;
					if (this._progressUI != null)
					{
						this._progressUI.SetProgress(num);
					}
					this.<RummageRoutine>g__EjectUpTo|140_1(num, ref CS$<>8__locals1);
				}
				yield return null;
			}
			float elapsed = 0f;
			while (elapsed < this.rummageDuration && this._rummaging && !this._endRequested)
			{
				elapsed += Time.deltaTime;
				float num2 = startShare + elapsed / this.rummageDuration * (1f - startShare);
				if (this._progressUI != null)
				{
					this._progressUI.SetProgress(num2);
				}
				this.<RummageRoutine>g__EjectUpTo|140_1(num2, ref CS$<>8__locals1);
				yield return null;
			}
			if (this._rummaging && !this._endRequested)
			{
				this.<RummageRoutine>g__EjectUpTo|140_1(1f, ref CS$<>8__locals1);
			}
			this.StopSearchLoop();
			if (this._progressUI != null)
			{
				if (this._endRequested)
				{
					this._progressUI.FadeOut(0.2f);
				}
				else
				{
					this._progressUI.Complete();
				}
			}
			if (!this._rummaging)
			{
				yield break;
			}
			if (this._playerAnimator != null)
			{
				if (this._endRequested)
				{
					this._playerAnimator.CrossFadeInFixedTime("Ciel_Rummage_Stop", 0.15f, 0);
				}
				else
				{
					this._playerAnimator.SetTrigger("TrashRummageEnding");
				}
			}
			if (this._spawnedCount == 0)
			{
				this.PlayBinClose();
			}
			t0 = Time.time;
			while (this._playerAnimator != null && Time.time - t0 < 3f && !this._playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Ciel_Rummage_Stop"))
			{
				yield return null;
			}
			t0 = Time.time;
			while (this._playerAnimator != null && Time.time - t0 < 10f && this._playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Ciel_Rummage_Stop"))
			{
				yield return null;
			}
			this.FinishRummage();
			yield break;
		}

		// Token: 0x060057B4 RID: 22452 RVA: 0x0022421C File Offset: 0x0022241C
		private void ShowProgressUI(int lootCount)
		{
			if (this._progressUI == null)
			{
				Vector3 vector = this.ProgressUiLocalPosition();
				if (this.progressUiPrefab != null)
				{
					this._progressUI = Object.Instantiate<TrashcanLootProgressUI>(this.progressUiPrefab, base.transform);
					this._progressUI.transform.localPosition = vector;
					this._progressUI.gameObject.SetActive(false);
				}
				else
				{
					this._progressUI = TrashcanLootProgressUI.Create(base.transform, vector);
				}
			}
			this._progressUI.Show(lootCount);
		}

		// Token: 0x060057B5 RID: 22453 RVA: 0x002242A8 File Offset: 0x002224A8
		private Vector3 ProgressUiLocalPosition()
		{
			Bounds bounds = default(Bounds);
			bool flag = true;
			foreach (Renderer renderer in base.GetComponentsInChildren<Renderer>(true))
			{
				if (renderer is MeshRenderer || renderer is SkinnedMeshRenderer)
				{
					if (flag)
					{
						bounds = renderer.bounds;
						flag = false;
					}
					else
					{
						bounds.Encapsulate(renderer.bounds);
					}
				}
			}
			if (flag)
			{
				return new Vector3(0f, this.progressUiHeight, 0f);
			}
			Vector3 vector = base.transform.InverseTransformPoint(bounds.center);
			return new Vector3(vector.x, this.progressUiHeight, vector.z);
		}

		// Token: 0x060057B6 RID: 22454 RVA: 0x00224354 File Offset: 0x00222554
		private void CaptureEntryPose()
		{
			if (this._hasPreRummagePose)
			{
				return;
			}
			Transform transform = this.ResolvePlayerTransform();
			if (transform == null)
			{
				return;
			}
			this._preRummagePosition = transform.position;
			this._preRummageRotation = transform.rotation;
			this._hasPreRummagePose = true;
		}

		// Token: 0x060057B7 RID: 22455 RVA: 0x0022439C File Offset: 0x0022259C
		public bool InterruptForEncounter()
		{
			if (!this._rummaging && this._rummageLease == null)
			{
				return false;
			}
			Transform transform = this.ResolvePlayerTransform();
			bool hasPreRummagePose = this._hasPreRummagePose;
			Vector3 preRummagePosition = this._preRummagePosition;
			Quaternion preRummageRotation = this._preRummageRotation;
			if (this._rummaging)
			{
				this.AbortRummage(true);
			}
			else
			{
				this.CancelPendingRummageStart(true);
			}
			this.ResetOwnedGenericAction(this._playerGenericAction);
			this._holding = false;
			this._holdConsumed = true;
			if (transform != null && hasPreRummagePose)
			{
				TrashcanLootInteractable.ApplyPlayerPose(transform, preRummagePosition, preRummageRotation);
			}
			return true;
		}

		// Token: 0x060057B8 RID: 22456 RVA: 0x00224420 File Offset: 0x00222620
		private void FinishRummage()
		{
			this._finishingSelf = true;
			this._pendingStartSerial++;
			this._pendingStartedByHold = false;
			this._rummaging = false;
			this.ResetOwnedGenericAction((this._actionAnimator != null) ? this._actionAnimator.GenericAction : this._playerGenericAction);
			this.RestorePlayer();
			this._rummageRoutine = null;
			if (this._spawnedCount > 0)
			{
				MissionGameplaySignals.NotifyTrashcanLooted(this._spawnedCount);
			}
			this.ConcludeInteraction();
			if (this.debugLogs)
			{
				Debug.Log("[TrashcanLoot] Rummage complete.", this);
			}
		}

		// Token: 0x060057B9 RID: 22457 RVA: 0x002244B4 File Offset: 0x002226B4
		private void AbortRummage(bool finishInvectorAction)
		{
			if (this._rummageRoutine != null)
			{
				base.StopCoroutine(this._rummageRoutine);
				this._rummageRoutine = null;
			}
			if (this._progressUI != null)
			{
				this._progressUI.Hide();
			}
			this._finishingSelf = true;
			this._pendingStartSerial++;
			this._pendingStartedByHold = false;
			this._rummaging = false;
			if (finishInvectorAction)
			{
				this.ResetOwnedGenericAction((this._actionAnimator != null) ? this._actionAnimator.GenericAction : this._playerGenericAction);
			}
			this.RestorePlayer();
			if (this._spawnedCount > 0)
			{
				MissionGameplaySignals.NotifyTrashcanLooted(this._spawnedCount);
			}
			this.ConcludeInteraction();
		}

		// Token: 0x060057BA RID: 22458 RVA: 0x00224564 File Offset: 0x00222764
		private void ConcludeInteraction()
		{
			bool flag = this._spawnedCount > 0;
			PoiResidencyPlacement.WriteFlag(this, "trashcan-looted", flag && !this.infiniteLootForTesting);
			if (!flag)
			{
				this._looted = false;
				this.PlayBinClose();
			}
			if (flag && !this.infiniteLootForTesting)
			{
				this.DisableInteraction();
				return;
			}
			this._looted = false;
			if (this._activePlayer != null && this.promptUI != null)
			{
				this.ShowPrompt();
			}
		}

		// Token: 0x060057BB RID: 22459 RVA: 0x002245E0 File Offset: 0x002227E0
		public void PlaySearchLoopForSibling()
		{
			this.StartSearchLoop();
		}

		// Token: 0x060057BC RID: 22460 RVA: 0x002245E8 File Offset: 0x002227E8
		public void StopSearchLoopForSibling()
		{
			this.StopSearchLoop();
		}

		// Token: 0x060057BD RID: 22461 RVA: 0x002245F0 File Offset: 0x002227F0
		private void StartSearchLoop()
		{
			if (this.searchLoopSFX == null || this.searchLoopVolume <= 0f)
			{
				return;
			}
			if (this._loopSource == null)
			{
				this._loopSource = base.gameObject.AddComponent<AudioSource>();
				this._loopSource.playOnAwake = false;
				this._loopSource.loop = true;
				this._loopSource.spatialBlend = 1f;
				this._loopSource.maxDistance = 12f;
				this._loopSource.rolloffMode = AudioRolloffMode.Linear;
			}
			if (this._loopFadeCo != null)
			{
				base.StopCoroutine(this._loopFadeCo);
				this._loopFadeCo = null;
			}
			this._loopSource.clip = this.searchLoopSFX;
			this._loopSource.time = Random.Range(0f, Mathf.Max(0f, this.searchLoopSFX.length * 0.8f));
			this._loopSource.volume = 0f;
			this._loopSource.Play();
			this._loopFadeCo = base.StartCoroutine(this.FadeLoop(this.searchLoopVolume, this.searchLoopFadeIn, false));
		}

		// Token: 0x060057BE RID: 22462 RVA: 0x00224714 File Offset: 0x00222914
		private void StopSearchLoop()
		{
			if (this._loopSource == null || !this._loopSource.isPlaying)
			{
				return;
			}
			if (this._loopFadeCo != null)
			{
				base.StopCoroutine(this._loopFadeCo);
				this._loopFadeCo = null;
			}
			if (!base.isActiveAndEnabled)
			{
				this._loopSource.Stop();
				return;
			}
			this._loopFadeCo = base.StartCoroutine(this.FadeLoop(0f, this.searchLoopFadeOut, true));
		}

		// Token: 0x060057BF RID: 22463 RVA: 0x0022478A File Offset: 0x0022298A
		private IEnumerator FadeLoop(float target, float duration, bool stopAtEnd)
		{
			float from = this._loopSource.volume;
			float t = 0f;
			while (t < duration && this._loopSource != null)
			{
				t += Time.deltaTime;
				this._loopSource.volume = Mathf.Lerp(from, target, Mathf.Clamp01(t / Mathf.Max(0.01f, duration)));
				yield return null;
			}
			if (this._loopSource != null)
			{
				this._loopSource.volume = target;
				if (stopAtEnd)
				{
					this._loopSource.Stop();
				}
			}
			this._loopFadeCo = null;
			yield break;
		}

		// Token: 0x060057C0 RID: 22464 RVA: 0x002247B0 File Offset: 0x002229B0
		private void RestorePlayer()
		{
			this.UnbindAnchorUpdates();
			StealthActor rummageStealthActor = this._rummageStealthActor;
			if (rummageStealthActor != null)
			{
				rummageStealthActor.ClearVisibilityPointOverride(this);
			}
			this._rummageStealthActor = null;
			this._hasPreRummagePose = false;
			if (this._progressUI != null)
			{
				this._progressUI.Hide();
			}
			this.StopSearchLoop();
			this.SetRummageCamera(false);
			if (this._interactionViewPushed && this._fpController != null)
			{
				this._fpController.EndInteractionView(this);
			}
			this._interactionViewPushed = false;
			this.HideRummageHud();
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
			this.LockControls(false);
			FullBodyHideActivityContext.Release(ref this._rummageLease);
		}

		// Token: 0x060057C1 RID: 22465 RVA: 0x0022489C File Offset: 0x00222A9C
		private void DisableInteraction()
		{
			this._holding = false;
			this._holdConsumed = true;
			this.HidePromptPresenter();
			if (this.HasHideInteraction)
			{
				if (this._hideSibling.IsPlayerInRange)
				{
					this.ShowPrompt();
				}
				return;
			}
			this._legacyTriggerObjectsDisabledForLoot = true;
			if (this.triggers != null)
			{
				foreach (vTriggerGenericAction vTriggerGenericAction in this.triggers)
				{
					if (vTriggerGenericAction != null)
					{
						vTriggerGenericAction.gameObject.SetActive(false);
					}
				}
			}
		}

		// Token: 0x060057C2 RID: 22466 RVA: 0x00224918 File Offset: 0x00222B18
		private void ShowPrompt()
		{
			if (this.promptUI == null)
			{
				return;
			}
			if (this.promptUI.gameObject != base.gameObject && !this.promptUI.gameObject.activeSelf)
			{
				this.promptUI.gameObject.SetActive(true);
			}
			InteractionConfigSO interactionConfigSO = this.ResolveIdlePromptConfig();
			if (!(interactionConfigSO != null))
			{
				if (this.promptUI.gameObject != base.gameObject)
				{
					this.promptUI.gameObject.SetActive(true);
				}
				return;
			}
			this.ResolvePromptObjectNameColor();
			if (this._hasPromptObjectNameColorOverride)
			{
				this.promptUI.Show(interactionConfigSO, this._promptObjectNameColor);
				return;
			}
			this.promptUI.Show(interactionConfigSO);
		}

		// Token: 0x060057C3 RID: 22467 RVA: 0x002249D8 File Offset: 0x00222BD8
		private void HidePromptPresenter()
		{
			if (this.promptUI == null)
			{
				return;
			}
			this.promptUI.Hide();
			if (this.promptUI.gameObject != base.gameObject && this.promptUI.gameObject.activeSelf)
			{
				this.promptUI.gameObject.SetActive(false);
			}
		}

		// Token: 0x060057C4 RID: 22468 RVA: 0x00224A3A File Offset: 0x00222C3A
		private InteractionConfigSO ResolveIdlePromptConfig()
		{
			return this.ResolveIdlePromptConfigForAvailability(TrashcanLootInteractable.LootBlocked);
		}

		// Token: 0x060057C5 RID: 22469 RVA: 0x00224A48 File Offset: 0x00222C48
		private InteractionConfigSO ResolveIdlePromptConfigForAvailability(bool lootBlocked)
		{
			if ((!this._looted && !lootBlocked) || !this.HasHideInteraction || this.promptConfig == null)
			{
				return this.promptConfig;
			}
			if (this._hideOnlyPromptConfig != null)
			{
				return this._hideOnlyPromptConfig;
			}
			this._hideOnlyPromptConfig = Object.Instantiate<InteractionConfigSO>(this.promptConfig);
			this._hideOnlyPromptConfig.hideFlags = HideFlags.HideAndDontSave;
			this._hideOnlyPromptConfig.mainActionName = this.promptConfig.secondaryActionName;
			this._hideOnlyPromptConfig.mainKeyDisplayLabel = this.promptConfig.secondaryKeyDisplayLabel;
			this._hideOnlyPromptConfig.secondaryActionName = string.Empty;
			this._hideOnlyPromptConfig.secondaryKeyDisplayLabel = string.Empty;
			return this._hideOnlyPromptConfig;
		}

		// Token: 0x060057C6 RID: 22470 RVA: 0x00224B04 File Offset: 0x00222D04
		private void ResolvePromptObjectNameColor()
		{
			if (this._promptObjectNameColorResolved)
			{
				return;
			}
			this._promptObjectNameColorResolved = true;
			RaritySettingsSO instance = RaritySettingsSO.Instance;
			ItemRarity itemRarity = ((this.lootProfile != null) ? this.lootProfile.trashcanRarity : ItemRarity.Common);
			if (instance == null || !instance.ColorLabel(itemRarity))
			{
				return;
			}
			this._promptObjectNameColor = instance.GetColor(itemRarity);
			this._hasPromptObjectNameColorOverride = true;
		}

		// Token: 0x060057C7 RID: 22471 RVA: 0x00224B6B File Offset: 0x00222D6B
		private void ShowRummageHud()
		{
			this._shownRummageHudConfig = this.ResolveRummageHudConfig();
			if (this._shownRummageHudConfig != null && InteractionHUDController.Instance != null)
			{
				InteractionHUDController.Instance.Show(this._shownRummageHudConfig);
			}
		}

		// Token: 0x060057C8 RID: 22472 RVA: 0x00224BA4 File Offset: 0x00222DA4
		private InteractionConfigSO ResolveRummageHudConfig()
		{
			if (this.rummageHudConfig == null || !FullBodyHideActivityContext.IsUsingTripodCamera(this._rummageLease))
			{
				return this.rummageHudConfig;
			}
			if (this._tripodHudConfig == null)
			{
				this._tripodHudConfig = Object.Instantiate<InteractionConfigSO>(this.rummageHudConfig);
				this._tripodHudConfig.hideFlags = HideFlags.HideAndDontSave;
				this._tripodHudConfig.name = this.rummageHudConfig.name + " (Tripod)";
				List<UsableAction> usableActions = this._tripodHudConfig.usableActions;
				if (usableActions != null)
				{
					usableActions.RemoveAll(delegate(UsableAction action)
					{
						if (action != null)
						{
							string keyDisplayLabel = action.keyDisplayLabel;
							GenericInput genericInput = this.switchCameraInput;
							return string.Equals(keyDisplayLabel, ((genericInput != null) ? genericInput.keyboard : null) ?? "F", StringComparison.OrdinalIgnoreCase);
						}
						return false;
					});
				}
				ControllerContextBindings.CopyLabels(this.rummageHudConfig, this._tripodHudConfig);
			}
			return this._tripodHudConfig;
		}

		// Token: 0x060057C9 RID: 22473 RVA: 0x00224C5C File Offset: 0x00222E5C
		private void HideRummageHud()
		{
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (instance != null)
			{
				instance.HideIfCurrent((this._shownRummageHudConfig != null) ? this._shownRummageHudConfig : this.rummageHudConfig);
			}
			this._shownRummageHudConfig = null;
		}

		// Token: 0x060057CA RID: 22474 RVA: 0x00224CA4 File Offset: 0x00222EA4
		private void EjectLoot(TrashcanLootProfileSO.RolledLoot roll, int index, int total)
		{
			if (TrashcanLootInteractable.LootBlocked)
			{
				return;
			}
			InventoryItemSO item = roll.item;
			if (item == null)
			{
				return;
			}
			if (RunModeService.IsLongWayHome && !LongWayHomeLootPolicy.IsAllowedTrashLoot(item))
			{
				return;
			}
			InventoryDropController instance = InventoryDropController.Instance;
			if (instance == null)
			{
				Debug.LogWarning("[TrashcanLoot] No InventoryDropController in scene - cannot eject loot.", this);
				return;
			}
			Vector3 vector = ((this.rummageAnchor != null) ? (this.rummageAnchor.position - base.transform.position) : base.transform.forward);
			vector.y = 0f;
			vector = ((vector.sqrMagnitude > 0.001f) ? vector.normalized : Vector3.forward);
			Vector3 vector2 = Vector3.Cross(Vector3.up, vector);
			float num = ((float)index - (float)(total - 1) * 0.5f) * this.lootSpacing;
			Vector3 vector3 = base.transform.position + vector * (this.lootForwardDistance + Random.Range(-this.lootJitter, this.lootJitter)) + vector2 * (num + Random.Range(-this.lootJitter, this.lootJitter)) + Vector3.up * this.ejectHeight;
			if (this.infiniteLootForTesting)
			{
				RunModeService.DisqualifyLeaderboardRun("Loot was generated with an infinite loot debug setting.");
			}
			instance.SpawnDropAt(item, vector3, roll.rarity, roll.variantIndex, 1f, true, false);
			this._spawnedCount++;
			if (this.ejectSFX != null)
			{
				if (this.audioSource == null)
				{
					this.audioSource = base.GetComponent<AudioSource>();
				}
				if (this.audioSource != null)
				{
					this.audioSource.PlayOneShot(this.ejectSFX);
				}
				else
				{
					AudioSource.PlayClipAtPoint(this.ejectSFX, base.transform.position);
				}
			}
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[TrashcanLoot] Ejected {0} ({1}).", item.itemName, roll.rarity), this);
			}
		}

		// Token: 0x060057CB RID: 22475 RVA: 0x00224EA4 File Offset: 0x002230A4
		public void ApplyLootBodyPaint()
		{
			if (!GarbageBodyPaint.TryApply((PlayerRegistry.Player != null) ? PlayerRegistry.Player.gameObject : this._activePlayer, this.lootBodyPaintDecal))
			{
				return;
			}
			if (this.debugLogs)
			{
				Debug.Log("[TrashcanLoot] Loot body-paint applied (" + ((this.lootBodyPaintDecal != null) ? this.lootBodyPaintDecal.name : "controller default") + ", 30s hold + decay).", this);
			}
		}

		// Token: 0x060057CC RID: 22476 RVA: 0x00224F1C File Offset: 0x0022311C
		private void SetRummageCamera(bool on)
		{
			if (on && FullBodyHideActivityContext.IsUsingTripodCamera(this._rummageLease))
			{
				return;
			}
			if (on == this._rummageCamActive)
			{
				return;
			}
			if (on)
			{
				if (this.rummageCamera == null)
				{
					foreach (Camera camera in base.GetComponentsInChildren<Camera>(true))
					{
						if (!(camera.name != "TrashcanRummageCamera"))
						{
							this.rummageCamera = camera;
							break;
						}
					}
				}
				if (this.rummageCamera == null)
				{
					this.rummageCamera = this.CreateRummageCamera();
				}
				if (this.rummageCamera == null)
				{
					return;
				}
				if (this._fpController != null)
				{
					this._fpController.RegisterHeadVisibleCamera(this.rummageCamera);
				}
				this._disabledMainCamera = Camera.main;
				if (this._disabledMainCamera != null)
				{
					this._disabledMainCamera.enabled = false;
				}
				this.rummageCamera.gameObject.SetActive(true);
				this.rummageCamera.enabled = true;
				this._rummageCamActive = true;
				if (this.aimAtPlayerHead)
				{
					this.AimRummageCamera();
				}
				if (this.debugLogs)
				{
					Debug.Log("[TrashcanLoot] Camera interna ON.", this);
					return;
				}
			}
			else
			{
				this._rummageCamActive = false;
				if (this.rummageCamera != null)
				{
					this.rummageCamera.gameObject.SetActive(false);
				}
				if (this._fpController != null)
				{
					this._fpController.UnregisterHeadVisibleCamera(this.rummageCamera);
				}
				if (this._disabledMainCamera != null)
				{
					this._disabledMainCamera.enabled = true;
				}
				this._disabledMainCamera = null;
				if (this.debugLogs)
				{
					Debug.Log("[TrashcanLoot] Camera interna OFF.", this);
				}
			}
		}

		// Token: 0x060057CD RID: 22477 RVA: 0x002250BC File Offset: 0x002232BC
		private Camera CreateRummageCamera()
		{
			GameObject gameObject = new GameObject("TrashcanRummageCamera");
			gameObject.transform.SetParent(base.transform, false);
			gameObject.transform.localPosition = this.cameraLocalPosition;
			gameObject.transform.localRotation = Quaternion.Euler(this.cameraLocalEuler);
			Camera camera = gameObject.AddComponent<Camera>();
			camera.fieldOfView = this.cameraFieldOfView;
			camera.nearClipPlane = 0.01f;
			camera.farClipPlane = 80f;
			gameObject.SetActive(false);
			return camera;
		}

		// Token: 0x060057CE RID: 22478 RVA: 0x0022513C File Offset: 0x0022333C
		private void AimRummageCamera()
		{
			Transform transform = null;
			if (this._playerAnimator != null && this._playerAnimator.isHuman)
			{
				transform = this._playerAnimator.GetBoneTransform(HumanBodyBones.Head);
			}
			Vector3 vector;
			if (transform != null)
			{
				vector = transform.position;
			}
			else
			{
				Transform transform2 = this.ResolvePlayerTransform();
				if (transform2 == null)
				{
					return;
				}
				vector = transform2.position + Vector3.up * 1.45f;
			}
			Vector3 vector2 = vector - this.rummageCamera.transform.position;
			if (vector2.sqrMagnitude < 0.0001f)
			{
				return;
			}
			this.rummageCamera.transform.rotation = Quaternion.LookRotation(vector2.normalized, -base.transform.forward);
		}

		// Token: 0x060057CF RID: 22479 RVA: 0x00225204 File Offset: 0x00223404
		private void PlayBinOpen()
		{
			if (this.binAnimator == null)
			{
				return;
			}
			if (this._lidCloseCo != null)
			{
				base.StopCoroutine(this._lidCloseCo);
				this._lidCloseCo = null;
			}
			this.binAnimator.Play("WasteCan_Open", 0, 0f);
		}

		// Token: 0x060057D0 RID: 22480 RVA: 0x00225254 File Offset: 0x00223454
		private void PlayBinClose()
		{
			if (this.binAnimator == null)
			{
				return;
			}
			AnimatorStateInfo currentAnimatorStateInfo = this.binAnimator.GetCurrentAnimatorStateInfo(0);
			if (currentAnimatorStateInfo.IsName("WasteCan_Close"))
			{
				return;
			}
			bool flag = currentAnimatorStateInfo.IsName("WasteCan_Open");
			float num = (flag ? Mathf.Clamp01(currentAnimatorStateInfo.normalizedTime) : 1f);
			float num2 = ((flag && currentAnimatorStateInfo.length > 0.01f) ? currentAnimatorStateInfo.length : 2f);
			if (this._lidCloseCo != null)
			{
				base.StopCoroutine(this._lidCloseCo);
			}
			if (!base.isActiveAndEnabled)
			{
				this.binAnimator.Play("WasteCan_Close", 0, 0f);
				return;
			}
			this._lidCloseCo = base.StartCoroutine(this.ScrubBinClosed(num, num2));
		}

		// Token: 0x060057D1 RID: 22481 RVA: 0x00225315 File Offset: 0x00223515
		private IEnumerator ScrubBinClosed(float fromNt, float clipLen)
		{
			float nt = fromNt;
			while (nt > 0f && this.binAnimator != null)
			{
				nt -= Time.deltaTime / Mathf.Max(0.1f, clipLen);
				this.binAnimator.Play("WasteCan_Close", 0, Mathf.Max(0f, nt));
				yield return null;
			}
			this._lidCloseCo = null;
			yield break;
		}

		// Token: 0x060057D2 RID: 22482 RVA: 0x00225334 File Offset: 0x00223534
		private void SnapPlayerToAnchor()
		{
			if (this.rummageAnchor == null)
			{
				return;
			}
			Transform transform = this.ResolvePlayerTransform();
			if (transform == null)
			{
				return;
			}
			Quaternion quaternion = Quaternion.Euler(0f, this.rummageAnchor.eulerAngles.y, 0f);
			TrashcanLootInteractable.ApplyPlayerPose(transform, this.rummageAnchor.position, quaternion);
		}

		// Token: 0x060057D3 RID: 22483 RVA: 0x00225393 File Offset: 0x00223593
		private void HoldAtAnchor()
		{
			if (!this._rummaging || this.useRootMotion)
			{
				return;
			}
			this.SnapPlayerToAnchor();
		}

		// Token: 0x060057D4 RID: 22484 RVA: 0x002253AC File Offset: 0x002235AC
		private void BindAnchorUpdates()
		{
			this.UnbindAnchorUpdates();
			if (this.useRootMotion)
			{
				return;
			}
			Transform transform = this.ResolvePlayerTransform();
			this._anchoredInput = ((transform != null) ? transform.GetComponent<vThirdPersonInput>() : null);
			if (this._anchoredInput == null)
			{
				return;
			}
			this._anchoredInput.onAnimatorMove += this.HoldAtAnchor;
			this._anchoredInput.onLateUpdate += this.HoldAtAnchor;
		}

		// Token: 0x060057D5 RID: 22485 RVA: 0x00225424 File Offset: 0x00223624
		private void UnbindAnchorUpdates()
		{
			if (this._anchoredInput != null)
			{
				this._anchoredInput.onAnimatorMove -= this.HoldAtAnchor;
				this._anchoredInput.onLateUpdate -= this.HoldAtAnchor;
			}
			this._anchoredInput = null;
		}

		// Token: 0x060057D6 RID: 22486 RVA: 0x00225474 File Offset: 0x00223674
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

		// Token: 0x060057D7 RID: 22487 RVA: 0x002254C8 File Offset: 0x002236C8
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

		// Token: 0x060057D8 RID: 22488 RVA: 0x0022555D File Offset: 0x0022375D
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

		// Token: 0x060057D9 RID: 22489 RVA: 0x00225590 File Offset: 0x00223790
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

		// Token: 0x060057DB RID: 22491 RVA: 0x00225760 File Offset: 0x00223960
		[CompilerGenerated]
		private void <RummageRoutine>g__EjectUpTo|140_1(float progress, ref TrashcanLootInteractable.<>c__DisplayClass140_0 A_2)
		{
			while (A_2.spawned < A_2.loot.Count && progress >= ((float)A_2.spawned + 1f) / (float)A_2.loot.Count)
			{
				this.EjectLoot(A_2.loot[A_2.spawned], A_2.spawned, A_2.loot.Count);
				if (this._progressUI != null)
				{
					this._progressUI.MarkReward(A_2.spawned);
				}
				int spawned = A_2.spawned;
				A_2.spawned = spawned + 1;
			}
		}

		// Token: 0x040057EC RID: 22508
		private const string StartStateName = "Ciel_Rummage_Start";

		// Token: 0x040057ED RID: 22509
		private const string LoopStateName = "Ciel_Rummage_Loop";

		// Token: 0x040057EE RID: 22510
		private const string StopStateName = "Ciel_Rummage_Stop";

		// Token: 0x040057EF RID: 22511
		private const string EndingTrigger = "TrashRummageEnding";

		// Token: 0x040057F0 RID: 22512
		private const string GateReason = "TrashcanRummage";

		// Token: 0x040057F1 RID: 22513
		[Header("References")]
		[Tooltip("vTriggerGenericAction(s) that drive the rummage. Auto-resolved from children if empty.")]
		[SerializeField]
		private vTriggerGenericAction[] triggers;

		// Token: 0x040057F2 RID: 22514
		[Tooltip("World-space prompt shown when the player is in range. Auto-resolved from children.")]
		[SerializeField]
		private InteractablePromptUI promptUI;

		// Token: 0x040057F3 RID: 22515
		[Tooltip("Full position and facing for the rummage, including the authored floor height. Auto-resolved from a child named 'RummageAnchor'.")]
		[SerializeField]
		private Transform rummageAnchor;

		// Token: 0x040057F4 RID: 22516
		[Header("Loot")]
		[Tooltip("Tier profile: loot table, item count and rarity guarantee for this can.")]
		[SerializeField]
		private TrashcanLootProfileSO lootProfile;

		// Token: 0x040057F5 RID: 22517
		[Header("UI Config")]
		[Tooltip("Prompt template ('Trashcan' / 'Rummage' / E). Cloned per instance so the label can be tinted with the can's tier color without touching the shared asset.")]
		[SerializeField]
		private InteractionConfigSO promptConfig;

		// Token: 0x040057F6 RID: 22518
		[Tooltip("Right-side shared interaction HUD shown while searching. It should contain the camera/view-switch action (F), matching the vehicle interaction HUD style.")]
		[SerializeField]
		private InteractionConfigSO rummageHudConfig;

		// Token: 0x040057F7 RID: 22519
		[Header("Lid")]
		[Tooltip("Animator on the Waste_Can FBX instance root (under the WasteCanRig wrapper - it must sit exactly there: the clip paths start at the NESTED 'Waste_Can' node inside the FBX). Plays the authored lid-open clip (A_WasteCan_Enter_OBJ, same length as Ciel's Enter clip and started the same frame, so the two stay in sync). On a LOOTED can (>=1 item ejected) the lid stays open; if nothing came out it swings back shut via the reverse Close state. Auto-resolved from under a child named 'WasteCanRig'.")]
		[SerializeField]
		private Animator binAnimator;

		// Token: 0x040057F8 RID: 22520
		[Header("Rummage")]
		[Tooltip("Seconds the rummage loop lasts (loot is spread evenly across this window).")]
		[SerializeField]
		private float rummageDuration = 7f;

		// Token: 0x040057F9 RID: 22521
		[Tooltip("The dive clips are authored in place; enable only if the Start clip carries baked root translation that should move Ciel toward the can.")]
		[SerializeField]
		private bool useRootMotion;

		// Token: 0x040057FA RID: 22522
		[Header("Loot ejection")]
		[Tooltip("How far IN FRONT of the can (the player/anchor side) ejected items land.")]
		[SerializeField]
		private float lootForwardDistance = 1.35f;

		// Token: 0x040057FB RID: 22523
		[Tooltip("Lateral spacing between ejected items - they land in a tidy row, slightly apart from each other, centered on the can's front.")]
		[SerializeField]
		private float lootSpacing = 0.5f;

		// Token: 0x040057FC RID: 22524
		[Tooltip("Small random offset so the row doesn't look machine-placed.")]
		[SerializeField]
		private float lootJitter = 0.08f;

		// Token: 0x040057FD RID: 22525
		[Tooltip("Height above the can pivot where ejected items appear before falling.")]
		[SerializeField]
		private float ejectHeight = 1.65f;

		// Token: 0x040057FE RID: 22526
		[Tooltip("Sound played each time an item pops out of the can (the inventory's own feedback click, so drops sound consistent everywhere).")]
		[SerializeField]
		private AudioClip ejectSFX;

		// Token: 0x040057FF RID: 22527
		[Tooltip("Looping rustle played while the search is running (start to stop).")]
		[SerializeField]
		private AudioClip searchLoopSFX;

		// Token: 0x04005800 RID: 22528
		[Range(0f, 1f)]
		[SerializeField]
		private float searchLoopVolume = 0.6f;

		// Token: 0x04005801 RID: 22529
		[Tooltip("Seconds the search loop fades in when the rummage starts.")]
		[SerializeField]
		private float searchLoopFadeIn = 0.35f;

		// Token: 0x04005802 RID: 22530
		[Tooltip("Seconds the search loop fades out when the search ends or is released.")]
		[SerializeField]
		private float searchLoopFadeOut = 0.45f;

		// Token: 0x04005803 RID: 22531
		[Tooltip("3D source on the can. Auto-resolved from this object if empty.")]
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x04005804 RID: 22532
		[Header("Hold to loot")]
		[Tooltip("The rummage runs WHILE this key is held: hold past the start threshold to begin, keep holding for the whole search, release to stop instantly (crossfade straight to the Stop clip, remaining rolls lost). The tap stays reserved for the item pickups around the can; the Invector trigger's own input is disabled at Awake.")]
		[SerializeField]
		private GenericInput interactInput = new GenericInput("E", "A", "A");

		// Token: 0x04005805 RID: 22533
		[Tooltip("Seconds the key must be held before the rummage begins (distinguishes the hold from a pickup tap).")]
		[SerializeField]
		private float holdToRummageDuration = 0.25f;

		// Token: 0x04005806 RID: 22534
		[Header("Rummage Camera")]
		[Tooltip("Camera dentro il cassonetto che inquadra Ciel dal basso mentre fruga (lei in alto, camera sul fondo del bidone). Switchabile col tasto camera del CarHide mentre si tiene premuto E. Auto-risolta da un figlio 'TrashcanRummageCamera'; se assente viene creata a runtime sul fondo del bidone (bounds del mesh).")]
		[SerializeField]
		private Camera rummageCamera;

		// Token: 0x04005807 RID: 22535
		[Tooltip("Stesso input dello switch camera sotto le auto (F/Y): alterna camera default e camera interna finche' il rummage e' in corso.")]
		[SerializeField]
		private GenericInput switchCameraInput = new GenericInput("F", "Y", "Y");

		// Token: 0x04005808 RID: 22536
		[Tooltip("Posa LOCALE (figlia del root del cassonetto) della camera auto-creata: valori tarati a mano in Play sul bidone (fondo, guarda in su verso l'apertura).")]
		[SerializeField]
		private Vector3 cameraLocalPosition = new Vector3(0.016f, 0.206f, 0.018f);

		// Token: 0x04005809 RID: 22537
		[SerializeField]
		private Vector3 cameraLocalEuler = new Vector3(-78.19f, -1.787f, 1.9f);

		// Token: 0x0400580A RID: 22538
		[Tooltip("FOV della camera interna (tarato a mano).")]
		[SerializeField]
		private float cameraFieldOfView = 43.1f;

		// Token: 0x0400580B RID: 22539
		[Tooltip("Se attivo la camera IGNORA la rotazione autorata e segue la testa di Ciel ogni frame. Off di default: il framing fisso tarato a mano e' quello buono.")]
		[SerializeField]
		private bool aimAtPlayerHead;

		// Token: 0x0400580C RID: 22540
		[Header("Progress UI")]
		[Tooltip("Authorable progress bar prefab (built by Tools/Trashcan Loot/Build Progress UI Prefab, restyle it freely). If empty, an identical procedural fallback is built at runtime.")]
		[SerializeField]
		private TrashcanLootProgressUI progressUiPrefab;

		// Token: 0x0400580D RID: 22541
		[Tooltip("Can-local height of the runtime progress bar (fill + one rarity marker per reward at its drop time).")]
		[SerializeField]
		private float progressUiHeight = 1.45f;

		// Token: 0x0400580E RID: 22542
		[Tooltip("Share of the bar dedicated to the lean-in (Start clip) phase, so the fill moves from the very first held frame and the whole action reads as ONE operation. The loot window fills the remaining share.")]
		[Range(0.05f, 0.6f)]
		[SerializeField]
		private float progressStartPhaseShare = 0.35f;

		// Token: 0x0400580F RID: 22543
		[Header("Testing")]
		[Tooltip("TESTING: the can never empties - after each rummage it becomes lootable again, so anchor/loot tuning can be iterated endlessly. Turn OFF for the real one-shot behavior before release.")]
		[SerializeField]
		private bool infiniteLootForTesting;

		// Token: 0x04005810 RID: 22544
		[Header("Loot Body-Paint")]
		[Tooltip("SkinDecal applicato sul player quando la lattina viene lootata (piena opacità 30s poi decay, via il MudBodyPaintRuntimeController esistente). Assegna Mud_02. Se vuoto usa il decal configurato sul controller (la pozza di fango).")]
		[SerializeField]
		private SkinDecal lootBodyPaintDecal;

		// Token: 0x04005811 RID: 22545
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04005812 RID: 22546
		private GameObject _activePlayer;

		// Token: 0x04005813 RID: 22547
		private PlayerActionAnimator _actionAnimator;

		// Token: 0x04005814 RID: 22548
		private Animator _playerAnimator;

		// Token: 0x04005815 RID: 22549
		private bool _looted;

		// Token: 0x04005816 RID: 22550
		private int _spawnedCount;

		// Token: 0x04005817 RID: 22551
		private bool _rummaging;

		// Token: 0x04005818 RID: 22552
		private StealthActor _rummageStealthActor;

		// Token: 0x04005819 RID: 22553
		private Vector3 _preRummagePosition;

		// Token: 0x0400581A RID: 22554
		private Quaternion _preRummageRotation;

		// Token: 0x0400581B RID: 22555
		private bool _hasPreRummagePose;

		// Token: 0x0400581C RID: 22556
		private bool _endRequested;

		// Token: 0x0400581D RID: 22557
		private bool _finishingSelf;

		// Token: 0x0400581E RID: 22558
		private bool _suppressionWarned;

		// Token: 0x0400581F RID: 22559
		private float _rummageStartTime;

		// Token: 0x04005820 RID: 22560
		private AudioSource _loopSource;

		// Token: 0x04005821 RID: 22561
		private Coroutine _loopFadeCo;

		// Token: 0x04005822 RID: 22562
		private bool _holding;

		// Token: 0x04005823 RID: 22563
		private bool _holdConsumed;

		// Token: 0x04005824 RID: 22564
		private float _holdTimer;

		// Token: 0x04005825 RID: 22565
		private bool _startedByHold;

		// Token: 0x04005826 RID: 22566
		private bool _pendingStartedByHold;

		// Token: 0x04005827 RID: 22567
		private vGenericAction _playerGenericAction;

		// Token: 0x04005828 RID: 22568
		private TrashcanLootProgressUI _progressUI;

		// Token: 0x04005829 RID: 22569
		private bool _rootMotionPushed;

		// Token: 0x0400582A RID: 22570
		private bool _emotionPushed;

		// Token: 0x0400582B RID: 22571
		private PlayerActivityLease _rummageLease;

		// Token: 0x0400582C RID: 22572
		private Coroutine _rummageRoutine;

		// Token: 0x0400582D RID: 22573
		private int _pendingStartSerial;

		// Token: 0x0400582E RID: 22574
		private int _confidentLayerIdx = -1;

		// Token: 0x0400582F RID: 22575
		private int _embarrassedLayerIdx = -1;

		// Token: 0x04005830 RID: 22576
		private bool _layerIdxResolved;

		// Token: 0x04005831 RID: 22577
		private vThirdPersonController _lockedController;

		// Token: 0x04005832 RID: 22578
		private bool _controlsLocked;

		// Token: 0x04005833 RID: 22579
		private vThirdPersonInput _anchoredInput;

		// Token: 0x04005834 RID: 22580
		private FirstPersonController _fpController;

		// Token: 0x04005835 RID: 22581
		private bool _rummageCamActive;

		// Token: 0x04005836 RID: 22582
		private Camera _disabledMainCamera;

		// Token: 0x04005837 RID: 22583
		private bool _interactionViewPushed;

		// Token: 0x04005838 RID: 22584
		private InteractionConfigSO _tripodHudConfig;

		// Token: 0x04005839 RID: 22585
		private InteractionConfigSO _shownRummageHudConfig;

		// Token: 0x0400583A RID: 22586
		private Color _promptObjectNameColor = Color.white;

		// Token: 0x0400583B RID: 22587
		private bool _hasPromptObjectNameColorOverride;

		// Token: 0x0400583C RID: 22588
		private bool _promptObjectNameColorResolved;

		// Token: 0x0400583D RID: 22589
		private InteractionConfigSO _hideOnlyPromptConfig;

		// Token: 0x0400583E RID: 22590
		private bool _legacyTriggerObjectsDisabledForLoot;

		// Token: 0x0400583F RID: 22591
		private UnityAction<GameObject> _validateListener;

		// Token: 0x04005840 RID: 22592
		private UnityAction<GameObject> _invalidateListener;

		// Token: 0x04005841 RID: 22593
		private UnityAction _animationStartListener;

		// Token: 0x04005842 RID: 22594
		private UnityAction _cancelledListener;

		// Token: 0x04005843 RID: 22595
		private WasteCanHideInteractable _hideSibling;

		// Token: 0x04005844 RID: 22596
		private bool _lastLootAvailabilityBlocked;

		// Token: 0x04005845 RID: 22597
		private float _nextTriggerRepairAt;

		// Token: 0x04005846 RID: 22598
		private HeavyZoneStreamingProxy _pendingLidResetProxy;

		// Token: 0x04005847 RID: 22599
		private bool _poiResidencyRegistered;

		// Token: 0x04005848 RID: 22600
		internal bool PoiResidencyTierPrepared;

		// Token: 0x04005849 RID: 22601
		private const string BinOpenStateName = "WasteCan_Open";

		// Token: 0x0400584A RID: 22602
		private const string BinCloseStateName = "WasteCan_Close";

		// Token: 0x0400584B RID: 22603
		private Coroutine _lidCloseCo;
	}
}
