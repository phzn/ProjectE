using System;
using System.Collections;
using System.Collections.Generic;
using BoZo.CharacterCreator;
using Cinemachine;
using Features.Detection;
using Features.Inventory.Audio;
using Features.Inventory.Placement;
using Features.Loading;
using Features.MasturbationSystem;
using Features.PhoneCompanion;
using Features.PlayerEmotes;
using Features.Vehicles.Traffic;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Interactions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.Inventory.Actions
{
	// Token: 0x02001164 RID: 4452
	public class DildoActionExecutor : MonoBehaviour
	{
		// Token: 0x17001590 RID: 5520
		// (get) Token: 0x060087BA RID: 34746 RVA: 0x00377C36 File Offset: 0x00375E36
		private bool UsesTripodCamera
		{
			get
			{
				return this._activityLease != null && this._activityLease.Resources == (PlayerContextResource.PlayerControl | PlayerContextResource.FullBodyAnimation | PlayerContextResource.HeldItem);
			}
		}

		// Token: 0x17001591 RID: 5521
		// (get) Token: 0x060087BB RID: 34747 RVA: 0x00377C51 File Offset: 0x00375E51
		public bool IsExecuting
		{
			get
			{
				return this._isExecuting;
			}
		}

		// Token: 0x060087BC RID: 34748 RVA: 0x00377C5C File Offset: 0x00375E5C
		public static bool IsAnchoredToMovingSupport(Transform player, Transform support)
		{
			if (player == null || support == null)
			{
				return false;
			}
			foreach (DildoActionExecutor dildoActionExecutor in DildoActionExecutor.MovingSupportOwners)
			{
				if (!(dildoActionExecutor == null) && dildoActionExecutor._hasSavedPlayerState && !(dildoActionExecutor._movingSupport != support))
				{
					Transform transform = dildoActionExecutor.ResolveAlignmentTransform();
					if (transform != null && (transform == player || transform.IsChildOf(player) || player.IsChildOf(transform)))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060087BD RID: 34749 RVA: 0x00377D0C File Offset: 0x00375F0C
		private void Awake()
		{
			DetectionFlyingCamera.NormalizePhotoInput(this.flyModeInput);
			ControllerContextBindings.Set(this.flyModeInput, "Y");
			ControllerContextBindings.Set(this.cancelActionInput, "B");
		}

		// Token: 0x060087BE RID: 34750 RVA: 0x00377D3C File Offset: 0x00375F3C
		private void Start()
		{
			if (this.playerRoot == null && this.playerInput != null)
			{
				this.playerRoot = this.playerInput.transform;
			}
			if (this.playerEmotionController == null)
			{
				this.playerEmotionController = Object.FindObjectOfType<PlayerEmotionController>();
				if (this.debugLogs && this.playerEmotionController != null)
				{
					Debug.Log("[DildoActionExecutor] Auto-found PlayerEmotionController");
				}
			}
		}

		// Token: 0x060087BF RID: 34751 RVA: 0x00377DB0 File Offset: 0x00375FB0
		public void Execute(DildoActionDefinition action, Transform dildoTransform)
		{
			if (action == null || dildoTransform == null || !base.isActiveAndEnabled)
			{
				return;
			}
			if (this._isExecuting)
			{
				return;
			}
			if (!this.CanUsePlacementTarget(dildoTransform))
			{
				return;
			}
			if (!action.TryAllowClothing())
			{
				return;
			}
			if (this.playerActionAnimator == null)
			{
				Debug.LogError("[DildoActionExecutor] PlayerActionAnimator reference is missing!");
				return;
			}
			if (!this.TryAcquireActionContext())
			{
				return;
			}
			if (!base.isActiveAndEnabled)
			{
				this.ReleaseActionContext();
				return;
			}
			this._activeRoutine = base.StartCoroutine(this.ExecuteSequence(action, dildoTransform));
		}

		// Token: 0x060087C0 RID: 34752 RVA: 0x00377E3C File Offset: 0x0037603C
		private bool TryAcquireActionContext()
		{
			if (this._activityLease != null && this._activityLease.IsActive)
			{
				return false;
			}
			if (this.playerActionAnimator != null && this.playerActionAnimator.GenericAction != null && this.playerActionAnimator.GenericAction.doingAction)
			{
				return false;
			}
			PlayerContextResource playerContextResource = (SmartphoneController.IsStableTripodCameraOwner ? (PlayerContextResource.PlayerControl | PlayerContextResource.FullBodyAnimation | PlayerContextResource.HeldItem) : (PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation | PlayerContextResource.HeldItem));
			PlayerActivityLease playerActivityLease;
			if (!PlayerActivityGate.TryAcquire("PlacedDildoAction", this, PlayerActivity.Interact, playerContextResource, PlayerActivity.AllPlayerActions, out playerActivityLease, false, null))
			{
				return false;
			}
			this._activityLease = playerActivityLease;
			return true;
		}

		// Token: 0x060087C1 RID: 34753 RVA: 0x00377EC6 File Offset: 0x003760C6
		private void ReleaseActionContext()
		{
			PlayerActivityLease activityLease = this._activityLease;
			this._activityLease = null;
			if (activityLease == null)
			{
				return;
			}
			activityLease.Dispose();
		}

		// Token: 0x060087C2 RID: 34754 RVA: 0x00377EE0 File Offset: 0x003760E0
		public void CancelAction()
		{
			if (!this._isExecuting && !this._ownsPlayerAction && !this._actionUiScope.IsActive && this._activityLease == null && this._ownedFade == null && !this._hasSavedPlayerState)
			{
				return;
			}
			this._sessionVersion++;
			this.CancelOwnedFade();
			if (this._activeRoutine != null)
			{
				base.StopCoroutine(this._activeRoutine);
				this._activeRoutine = null;
			}
			this.StopOwnedPlayerAction(true);
			this.Cleanup();
		}

		// Token: 0x060087C3 RID: 34755 RVA: 0x00377F65 File Offset: 0x00376165
		private IEnumerator ExecuteSequence(DildoActionDefinition action, Transform dildoTransform)
		{
			DildoActionExecutor.<>c__DisplayClass108_0 CS$<>8__locals1 = new DildoActionExecutor.<>c__DisplayClass108_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.action = action;
			CS$<>8__locals1.dildoTransform = dildoTransform;
			this._isExecuting = true;
			this._currentAction = CS$<>8__locals1.action;
			this._interactionTarget = CS$<>8__locals1.dildoTransform;
			this._tracksInteractionTarget = true;
			DildoActionExecutor.<>c__DisplayClass108_0 CS$<>8__locals2 = CS$<>8__locals1;
			int num = this._sessionVersion + 1;
			this._sessionVersion = num;
			CS$<>8__locals2.sessionVersion = num;
			if (this.playerInput != null)
			{
				this._savedInputLock = this.playerInput.lockInput;
				this._ownsInputLock = true;
				this.playerInput.SetLockAllInput(true);
			}
			this.BeginActionPresentation();
			CS$<>8__locals1.isPositioned = false;
			if (ScreenFadeController.Instance != null)
			{
				ScreenFadeController entryFade = ScreenFadeController.Instance;
				this._ownedFade = entryFade;
				entryFade.FadeOutIn(delegate
				{
					CS$<>8__locals1.isPositioned = CS$<>8__locals1.<>4__this.TryCommitPositioning(CS$<>8__locals1.action, CS$<>8__locals1.dildoTransform, CS$<>8__locals1.sessionVersion);
				}, 0.25f, 0.3f, 0.15f, delegate
				{
					CS$<>8__locals1.<>4__this.CompleteOwnedFade(entryFade, CS$<>8__locals1.sessionVersion);
				});
				yield return new WaitUntil(() => CS$<>8__locals1.isPositioned);
				yield return new WaitForSeconds(0.1f);
			}
			else if (!this.TryCommitPositioning(CS$<>8__locals1.action, CS$<>8__locals1.dildoTransform, CS$<>8__locals1.sessionVersion))
			{
				yield break;
			}
			if (!this.CanContinueSession(CS$<>8__locals1.sessionVersion))
			{
				yield break;
			}
			if (!this.UsesTripodCamera && this.detectionCameraCtrl != null && (CS$<>8__locals1.action.CameraPresets != null || this.defaultEmotePresets != null))
			{
				if (Camera.main != null && !this._hasSavedCameraState)
				{
					this._savedCameraLocalPosition = Camera.main.transform.localPosition;
					this._savedCameraLocalRotation = Camera.main.transform.localRotation;
					this._hasSavedCameraState = true;
					if (this.debugLogs)
					{
						Debug.Log("[DildoActionExecutor] Saved camera state for restore");
					}
				}
				Camera main = Camera.main;
				CinemachineBrain cinemachineBrain = ((main != null) ? main.GetComponent<CinemachineBrain>() : null);
				if (cinemachineBrain != null && this._ownedBrain == null)
				{
					this._ownedBrain = cinemachineBrain;
					this._savedBrainEnabled = cinemachineBrain.enabled;
				}
				if (cinemachineBrain != null && !cinemachineBrain.enabled)
				{
					cinemachineBrain.enabled = true;
					if (this.debugLogs)
					{
						Debug.Log("[DildoActionExecutor] Enabled CinemachineBrain for action cameras");
					}
				}
				if (this.debugLogs)
				{
					Debug.Log("[DildoActionExecutor] Pre-activation: Brain=" + ((cinemachineBrain != null) ? (cinemachineBrain.enabled ? "enabled" : "DISABLED") : "null") + ", Presets=" + ((CS$<>8__locals1.action.CameraPresets != null) ? CS$<>8__locals1.action.CameraPresets.name : ((this.defaultEmotePresets != null) ? this.defaultEmotePresets.name : "null")));
				}
				this.ActivateDetectionCamera();
				if (this.debugLogs)
				{
					Debug.Log(string.Format("[DildoActionExecutor] Camera activation result: IsActive={0}", this.detectionCameraCtrl.IsActive));
				}
			}
			else if (this.debugLogs)
			{
				Debug.LogWarning(string.Concat(new string[]
				{
					"[DildoActionExecutor] Camera NOT activated: detectionCameraCtrl=",
					(this.detectionCameraCtrl != null) ? "exists" : "null",
					", action.CameraPresets=",
					(CS$<>8__locals1.action.CameraPresets != null) ? "exists" : "null",
					", defaultEmotePresets=",
					(this.defaultEmotePresets != null) ? "exists" : "null"
				}));
			}
			if (CS$<>8__locals1.action.PlayerActionEntry != null)
			{
				if (this.debugLogs)
				{
					Debug.Log("[DildoActionExecutor] Playing action: " + CS$<>8__locals1.action.DisplayName);
				}
				vTriggerGenericAction triggerPrefab = CS$<>8__locals1.action.PlayerActionEntry.TriggerPrefab;
				if (triggerPrefab != null)
				{
					this._ownedActionTrigger = triggerPrefab;
					this._originalUseTriggerRotation = triggerPrefab.useTriggerRotation;
					this._originalMatchTarget = triggerPrefab.matchTarget;
					triggerPrefab.useTriggerRotation = false;
					triggerPrefab.matchTarget = null;
				}
				this._ownsPlayerAction = triggerPrefab != null && this.playerActionAnimator.GenericAction != null;
				float num2 = this.playerActionAnimator.TriggerAction(CS$<>8__locals1.action.PlayerActionEntry);
				if (!this.CanContinueSession(CS$<>8__locals1.sessionVersion))
				{
					yield break;
				}
				Animator componentInChildren = CS$<>8__locals1.dildoTransform.GetComponentInChildren<Animator>();
				if (componentInChildren != null)
				{
					if (!string.IsNullOrEmpty(CS$<>8__locals1.action.DildoAnimationParameter))
					{
						if (CS$<>8__locals1.action.UseTriggerForDildo)
						{
							componentInChildren.SetTrigger(CS$<>8__locals1.action.DildoAnimationParameter);
						}
						else
						{
							componentInChildren.SetInteger(CS$<>8__locals1.action.DildoAnimationParameter, CS$<>8__locals1.action.DildoActionId);
						}
					}
					componentInChildren.SetFloat("Excitement", CS$<>8__locals1.action.Excitement);
				}
				this.SetupActionPlayback(CS$<>8__locals1.action, CS$<>8__locals1.dildoTransform);
				this.SetupAudioForAction(CS$<>8__locals1.action, CS$<>8__locals1.dildoTransform);
				if (CS$<>8__locals1.action.DisableFaceLayer)
				{
					this._suppressedFaceLayer = true;
					this.SetFaceLayerEnabled(false);
					if (this.blendShapeController != null)
					{
						this.blendShapeController.SuppressBlendShapeOverrides();
					}
				}
				else
				{
					this.SaveAndApplyFaceExpression(CS$<>8__locals1.action.FaceExpressionIndex);
				}
				if (CS$<>8__locals1.action.IsLoop)
				{
					while (this.cancelActionInput.GetButton())
					{
						yield return null;
					}
					while ((this.HandleCameraInput() || !this.cancelActionInput.GetButtonDown()) && !this._climaxFinished)
					{
						if (CS$<>8__locals1.action.UsesMasturbationVoiceAndMeter && this._activeVoiceController == null && !this._pleasureSoundTriggered && this._pleasureSoundRoutine == null && CS$<>8__locals1.action.PleasureLoopDelay > 0f)
						{
							this._pleasureSoundRoutine = base.StartCoroutine(this.PleasureSoundLoopRoutine(CS$<>8__locals1.action));
						}
						if (!this._climaxPlaying && this.playerActionAnimator != null && !this.playerActionAnimator.GenericAction.doingAction)
						{
							this.playerActionAnimator.TriggerAction(CS$<>8__locals1.action.PlayerActionEntry);
						}
						yield return null;
					}
					this.StopEmoteCamera();
				}
				else if (num2 > 0f)
				{
					yield return new WaitForSeconds(num2);
				}
			}
			if (ScreenFadeController.Instance != null)
			{
				this.CancelOwnedFade();
				this._ownedFade = ScreenFadeController.Instance;
				bool fadeOutDone = false;
				this._ownedFade.FadeOut(0.25f, delegate
				{
					if (CS$<>8__locals1.<>4__this.CanContinueSession(CS$<>8__locals1.sessionVersion))
					{
						fadeOutDone = true;
					}
				});
				yield return new WaitUntil(() => fadeOutDone);
			}
			if (!this.CanContinueSession(CS$<>8__locals1.sessionVersion))
			{
				yield break;
			}
			this.StopOwnedPlayerAction(false);
			this.RestoreOriginalState();
			if (this._ownedFade != null)
			{
				ScreenFadeController exitFade = this._ownedFade;
				bool fadeInDone = false;
				exitFade.FadeIn(0.3f, delegate
				{
					if (!CS$<>8__locals1.<>4__this.CanContinueSession(CS$<>8__locals1.sessionVersion))
					{
						return;
					}
					CS$<>8__locals1.<>4__this.CompleteOwnedFade(exitFade, CS$<>8__locals1.sessionVersion);
					fadeInDone = true;
				});
				yield return new WaitUntil(() => fadeInDone);
			}
			this.Cleanup();
			yield break;
		}

		// Token: 0x060087C4 RID: 34756 RVA: 0x00377F84 File Offset: 0x00376184
		private bool TryCommitPositioning(DildoActionDefinition action, Transform dildoTransform, int sessionVersion)
		{
			if (!this.CanContinueSession(sessionVersion))
			{
				return false;
			}
			if (!this.CanUsePlacementTarget(dildoTransform))
			{
				this.CancelAction();
				return false;
			}
			if (!action.TryAllowClothing())
			{
				this.CancelAction();
				return false;
			}
			this.PositionPlayerWithReparenting(action.PlayerPositionOffset, Quaternion.Euler(action.PlayerRotationOffset), dildoTransform);
			return true;
		}

		// Token: 0x060087C5 RID: 34757 RVA: 0x00377FD8 File Offset: 0x003761D8
		private bool CanUsePlacementTarget(Transform target)
		{
			if (target == null || !target.gameObject.activeInHierarchy)
			{
				return false;
			}
			PlacedDildoInteractable componentInParent = target.GetComponentInParent<PlacedDildoInteractable>();
			InventoryItemSO inventoryItemSO = ((componentInParent != null) ? componentInParent.ItemData : null);
			if (inventoryItemSO == null)
			{
				return true;
			}
			Transform transform = MovingPlacementSupport.Resolve(target);
			return MovingPlacementSupport.Allows(inventoryItemSO, transform) && (!inventoryItemSO.requiresStaticPlacement || !CarRoofCarrier.IsCarSurfing(this.ResolveAlignmentTransform()));
		}

		// Token: 0x060087C6 RID: 34758 RVA: 0x0037804C File Offset: 0x0037624C
		public void ActivateDetectionCamera()
		{
			if (this.UsesTripodCamera || this.detectionCameraCtrl == null || this._currentAction == null)
			{
				return;
			}
			DetectionCameraPresetCollectionSO detectionCameraPresetCollectionSO = ((this._currentAction.CameraPresets != null) ? this._currentAction.CameraPresets : this.defaultEmotePresets);
			if (detectionCameraPresetCollectionSO == null)
			{
				if (this.debugLogs)
				{
					Debug.LogWarning("[DildoActionExecutor] Cannot activate camera - no preset collection (action and default both null)");
				}
				return;
			}
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[DildoActionExecutor] Activating DetectionCamera with {0} presets: {1}", detectionCameraPresetCollectionSO.Count, detectionCameraPresetCollectionSO.name));
			}
			this.detectionCameraCtrl.ActivateForEmotePhase(detectionCameraPresetCollectionSO, this.playerRoot, this._activeDildoTransform);
			this._ownsDetectionCamera = this.detectionCameraCtrl.IsActive;
			string text = "unknown";
			if (this.detectionCameraCtrl.IsActive)
			{
				DetectionCameraPresetEntry preset = detectionCameraPresetCollectionSO.GetPreset(this.detectionCameraCtrl.CurrentPresetIndex);
				text = ((preset != null) ? preset.Label : null) ?? string.Format("preset_{0}", this.detectionCameraCtrl.CurrentPresetIndex);
			}
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[DildoActionExecutor] DetectionCamera activated | IsActive={0}, Preset={1} (index {2}/{3})", new object[]
				{
					this.detectionCameraCtrl.IsActive,
					text,
					this.detectionCameraCtrl.CurrentPresetIndex,
					this.detectionCameraCtrl.ActivePresetCount
				}));
			}
		}

		// Token: 0x060087C7 RID: 34759 RVA: 0x003781BC File Offset: 0x003763BC
		private bool HandleCameraInput()
		{
			if (this.UsesTripodCamera || this.flyingCamera == null)
			{
				return false;
			}
			bool flag = this.flyingCamera.HandleOwnerInput(this.flyModeInput, ref this._ownsFlyingCamera, this.detectionCameraCtrl, (this.playerRoot != null) ? this.playerRoot.position : base.transform.position, (this._ownedBrain != null) ? this._ownedBrain.OutputCamera : Camera.main);
			if (flag)
			{
				this._actionUiScope.Refresh();
			}
			return flag;
		}

		// Token: 0x060087C8 RID: 34760 RVA: 0x00378254 File Offset: 0x00376454
		public void DeactivateDetectionCamera()
		{
			if (this._ownsDetectionCamera && this.detectionCameraCtrl != null && this.detectionCameraCtrl.IsActive)
			{
				if (this.debugLogs)
				{
					Debug.Log("[DildoActionExecutor] Deactivating DetectionCamera...");
				}
				this.detectionCameraCtrl.Deactivate();
				this._ownsDetectionCamera = false;
				if (this.debugLogs)
				{
					Debug.Log(string.Format("[DildoActionExecutor] DetectionCamera deactivated | IsActive={0}", this.detectionCameraCtrl.IsActive));
					return;
				}
			}
			else if (this.debugLogs)
			{
				Debug.Log("[DildoActionExecutor] DeactivateDetectionCamera called but camera not active");
			}
		}

		// Token: 0x060087C9 RID: 34761 RVA: 0x003782E4 File Offset: 0x003764E4
		private void StopEmoteCamera()
		{
			if (this._ownsFlyingCamera && this.flyingCamera != null && this.flyingCamera.IsActive)
			{
				this.flyingCamera.Deactivate();
			}
			this._ownsFlyingCamera = false;
			if (this._ownsDetectionCamera && this.detectionCameraCtrl != null && this.detectionCameraCtrl.IsActive)
			{
				this.detectionCameraCtrl.Deactivate();
			}
			this._ownsDetectionCamera = false;
			if (this._ownedBrain != null)
			{
				this._ownedBrain.enabled = this._savedBrainEnabled;
				this._ownedBrain = null;
			}
			if (this._hasSavedCameraState && Camera.main != null)
			{
				Camera.main.transform.localPosition = this._savedCameraLocalPosition;
				Camera.main.transform.localRotation = this._savedCameraLocalRotation;
				this._hasSavedCameraState = false;
				if (this.debugLogs)
				{
					Debug.Log("[DildoActionExecutor] Camera transform restored to pre-action state");
				}
			}
			if (this._suppressedFaceLayer)
			{
				this._suppressedFaceLayer = false;
				this.SetFaceLayerEnabled(true);
				if (this.blendShapeController != null)
				{
					this.blendShapeController.RestoreBlendShapeOverrides();
				}
			}
			this.RestoreFaceExpression();
		}

		// Token: 0x060087CA RID: 34762 RVA: 0x00378410 File Offset: 0x00376610
		private void SetFaceLayerEnabled(bool enabled)
		{
			PlayerActionAnimator playerActionAnimator = this.playerActionAnimator;
			Animator animator = ((playerActionAnimator != null) ? playerActionAnimator.GetComponent<Animator>() : null);
			if (animator == null && this.playerRoot != null)
			{
				animator = this.playerRoot.GetComponentInChildren<Animator>();
			}
			if (animator != null)
			{
				int layerIndex = animator.GetLayerIndex("Face");
				if (layerIndex >= 0)
				{
					float num = (enabled ? 1f : 0f);
					animator.SetLayerWeight(layerIndex, num);
					if (this.debugLogs)
					{
						Debug.Log(string.Format("[DildoActionExecutor] Face layer {0} (weight={1})", enabled ? "enabled" : "disabled", num));
						return;
					}
				}
				else if (this.debugLogs)
				{
					Debug.LogWarning("[DildoActionExecutor] Face layer not found on player animator");
					return;
				}
			}
			else if (this.debugLogs)
			{
				Debug.LogWarning("[DildoActionExecutor] Player animator not found for Face layer control");
			}
		}

		// Token: 0x060087CB RID: 34763 RVA: 0x003784D8 File Offset: 0x003766D8
		private void SaveAndApplyFaceExpression(int newExpressionIndex)
		{
			PlayerActionAnimator playerActionAnimator = this.playerActionAnimator;
			Animator animator = ((playerActionAnimator != null) ? playerActionAnimator.GetComponent<Animator>() : null);
			if (animator == null && this.playerRoot != null)
			{
				animator = this.playerRoot.GetComponentInChildren<Animator>();
			}
			if (animator != null)
			{
				if (!this._hasSavedFaceExpression)
				{
					this._originalFaceExpressionIndex = animator.GetInteger("FaceExpressionIndex");
					this._hasSavedFaceExpression = true;
					if (this.debugLogs)
					{
						Debug.Log(string.Format("[DildoActionExecutor] Saved original face expression index: {0}", this._originalFaceExpressionIndex));
					}
				}
				if (this.playerEmotionController != null)
				{
					this.playerEmotionController.DisableFacialExpressionForAction();
					if (this.debugLogs)
					{
						Debug.Log("[DildoActionExecutor] Disabled PlayerEmotionController facial expression updates");
					}
				}
				if (newExpressionIndex >= 0)
				{
					animator.SetInteger("FaceExpressionIndex", newExpressionIndex);
					if (this.debugLogs)
					{
						Debug.Log(string.Format("[DildoActionExecutor] Applied face expression index: {0}", newExpressionIndex));
						return;
					}
				}
				else if (this.debugLogs)
				{
					Debug.Log("[DildoActionExecutor] Face expression index is -1, not applying any expression");
					return;
				}
			}
			else if (this.debugLogs)
			{
				Debug.LogWarning("[DildoActionExecutor] Player animator not found for face expression control");
			}
		}

		// Token: 0x060087CC RID: 34764 RVA: 0x003785E8 File Offset: 0x003767E8
		private void RestoreFaceExpression()
		{
			if (!this._hasSavedFaceExpression)
			{
				if (this.debugLogs)
				{
					Debug.Log("[DildoActionExecutor] No saved face expression to restore");
				}
				return;
			}
			PlayerActionAnimator playerActionAnimator = this.playerActionAnimator;
			Animator animator = ((playerActionAnimator != null) ? playerActionAnimator.GetComponent<Animator>() : null);
			if (animator == null && this.playerRoot != null)
			{
				animator = this.playerRoot.GetComponentInChildren<Animator>();
			}
			if (animator != null)
			{
				animator.SetInteger("FaceExpressionIndex", this._originalFaceExpressionIndex);
				if (this.debugLogs)
				{
					Debug.Log(string.Format("[DildoActionExecutor] Restored face expression index: {0}", this._originalFaceExpressionIndex));
				}
				this._hasSavedFaceExpression = false;
				this._originalFaceExpressionIndex = -1;
			}
			else if (this.debugLogs)
			{
				Debug.LogWarning("[DildoActionExecutor] Player animator not found for face expression restoration");
			}
			if (this.playerEmotionController != null)
			{
				this.playerEmotionController.RestoreFacialExpression();
				if (this.debugLogs)
				{
					Debug.Log("[DildoActionExecutor] Restored PlayerEmotionController facial expression updates");
				}
			}
		}

		// Token: 0x060087CD RID: 34765 RVA: 0x003786D0 File Offset: 0x003768D0
		private void PositionPlayerWithReparenting(Vector3 localOffsetPos, Quaternion localOffsetRot, Transform dildoTransform)
		{
			Transform transform = this.ResolveAlignmentTransform();
			if (transform == null)
			{
				return;
			}
			vThirdPersonController vThirdPersonController = ((this.playerInput != null) ? this.playerInput.cc : null);
			this._originalPlayerParent = transform.parent;
			this._originalPlayerScene = transform.gameObject.scene;
			this._originalPlayerWorldPosition = transform.position;
			this._originalPlayerWorldRotation = transform.rotation;
			this._originalPlayerWorldScale = transform.lossyScale;
			this._originalPlayerLocalPosition = transform.localPosition;
			this._originalPlayerLocalRotation = transform.localRotation;
			this._originalPlayerLocalScale = transform.localScale;
			this._hasSavedPlayerState = true;
			this._activeDildoTransform = dildoTransform;
			this._interactionTarget = dildoTransform;
			this._tracksInteractionTarget = true;
			this._originalDildoLocalPosition = dildoTransform.localPosition;
			this._movingSupport = MovingPlacementSupport.Resolve(dildoTransform);
			this._usesMovingSupport = this._movingSupport != null;
			if (this._usesMovingSupport)
			{
				this._originalSupportLocalPosition = this._movingSupport.InverseTransformPoint(transform.position);
				this._originalSupportLocalRotation = Quaternion.Inverse(this._movingSupport.rotation) * transform.rotation;
				this._lastSupportExitPosition = transform.position;
				this._lastSupportExitRotation = transform.rotation;
				DildoActionExecutor.MovingSupportOwners.Add(this);
			}
			if (this.disableControllerDuringAlign && vThirdPersonController != null)
			{
				this._alignmentController = vThirdPersonController;
				this._alignmentControllerWasEnabled = vThirdPersonController.enabled;
				vThirdPersonController.enabled = false;
			}
			Rigidbody component = transform.GetComponent<Rigidbody>();
			this._alignedPlayerBody = component;
			if (component != null)
			{
				this._hasRigidbody = true;
				this._originalIsKinematic = component.isKinematic;
				if (!component.isKinematic)
				{
					component.velocity = Vector3.zero;
					component.angularVelocity = Vector3.zero;
				}
				component.isKinematic = true;
			}
			if (this._currentAction != null && this._currentAction.DildoPositionOffset != Vector3.zero)
			{
				dildoTransform.localPosition += this._currentAction.DildoPositionOffset;
			}
			if (this._usesMovingSupport)
			{
				this._movingPlayerLocalPosition = localOffsetPos;
				PlacedDildoInteractable componentInParent = dildoTransform.GetComponentInParent<PlacedDildoInteractable>();
				if (componentInParent != null && componentInParent.PlacementType == PlacementSurfaceType.Wall)
				{
					this._movingPlayerLocalPosition.y = this._movingPlayerLocalPosition.y + dildoTransform.InverseTransformPoint(transform.position).y;
				}
				this._movingPlayerLocalRotation = localOffsetRot;
				this.FollowMovingPlacement();
			}
			else
			{
				transform.SetParent(dildoTransform);
				transform.localScale = new Vector3(1f / dildoTransform.lossyScale.x, 1f / dildoTransform.lossyScale.y, 1f / dildoTransform.lossyScale.z);
				Vector3 vector = new Vector3(0f, transform.localPosition.y, 0f) + localOffsetPos;
				transform.localPosition = vector;
				transform.localRotation = localOffsetRot;
			}
			if (this.disableControllerDuringAlign && vThirdPersonController != null)
			{
				this._controllerEnableRoutine = base.StartCoroutine(this.ReEnableControllerNextFrame(vThirdPersonController, this._sessionVersion));
			}
		}

		// Token: 0x060087CE RID: 34766 RVA: 0x003789CC File Offset: 0x00376BCC
		private void RestoreOriginalState()
		{
			Transform transform = this.ResolveAlignmentTransform();
			DildoActionExecutor.MovingSupportOwners.Remove(this);
			if (transform == null)
			{
				return;
			}
			if (this._usesMovingSupport && this._movingSupport != null)
			{
				this.CacheMovingSupportExitPose();
			}
			if (this._activeDildoTransform != null)
			{
				Animator componentInChildren = this._activeDildoTransform.GetComponentInChildren<Animator>();
				if (componentInChildren != null && this._currentAction != null && !string.IsNullOrEmpty(this._currentAction.DildoAnimationParameter))
				{
					if (this._currentAction.UseTriggerForDildo)
					{
						componentInChildren.ResetTrigger(this._currentAction.DildoAnimationParameter);
					}
					else
					{
						componentInChildren.SetInteger(this._currentAction.DildoAnimationParameter, 0);
					}
				}
				this._activeDildoTransform.localPosition = this._originalDildoLocalPosition;
				this._activeDildoTransform = null;
			}
			if (this._hasSavedPlayerState)
			{
				bool flag = this._originalPlayerScene.IsValid() && this._originalPlayerScene.isLoaded;
				Transform transform2 = ((this._originalPlayerParent != null) ? this._originalPlayerParent : null);
				if (flag && transform2 != null && transform2.gameObject.scene != this._originalPlayerScene)
				{
					transform2 = null;
				}
				transform.SetParent(transform2);
				if (flag && transform.gameObject.scene != this._originalPlayerScene)
				{
					transform.SetParent(null, true);
					SceneManager.MoveGameObjectToScene(transform.gameObject, this._originalPlayerScene);
				}
				if (this._usesMovingSupport)
				{
					transform.SetPositionAndRotation(this._lastSupportExitPosition, this._lastSupportExitRotation);
				}
				else if (flag && transform2 == null)
				{
					transform.SetPositionAndRotation(this._originalPlayerWorldPosition, this._originalPlayerWorldRotation);
				}
				else
				{
					transform.localPosition = this._originalPlayerLocalPosition;
					transform.localRotation = this._originalPlayerLocalRotation;
				}
				transform.localScale = ((flag && transform2 == null) ? this._originalPlayerWorldScale : this._originalPlayerLocalScale);
				this._originalPlayerParent = null;
				this._originalPlayerScene = default(Scene);
				this._hasSavedPlayerState = false;
			}
			if (this._hasRigidbody)
			{
				Rigidbody component = transform.GetComponent<Rigidbody>();
				if (component != null)
				{
					component.position = transform.position;
					component.rotation = transform.rotation;
					component.isKinematic = this._originalIsKinematic;
				}
				this._hasRigidbody = false;
			}
			if (this._usesMovingSupport)
			{
				CarRoofCarrier.ResumeAfterExternalPose(transform);
			}
			this._alignedPlayerBody = null;
			this._movingSupport = null;
			this._usesMovingSupport = false;
			this.RestoreAlignmentController();
		}

		// Token: 0x060087CF RID: 34767 RVA: 0x00378C3E File Offset: 0x00376E3E
		private void CacheMovingSupportExitPose()
		{
			this._lastSupportExitPosition = this._movingSupport.TransformPoint(this._originalSupportLocalPosition);
			this._lastSupportExitRotation = this._movingSupport.rotation * this._originalSupportLocalRotation;
		}

		// Token: 0x060087D0 RID: 34768 RVA: 0x00378C74 File Offset: 0x00376E74
		private void FollowMovingPlacement()
		{
			if (!this._usesMovingSupport || !this._hasSavedPlayerState)
			{
				return;
			}
			if (this._movingSupport == null || !this._movingSupport.gameObject.activeInHierarchy || this._activeDildoTransform == null || !this._activeDildoTransform.gameObject.activeInHierarchy)
			{
				this.CancelAction();
				return;
			}
			this.CacheMovingSupportExitPose();
			Transform transform = this.ResolveAlignmentTransform();
			Vector3 vector = this._activeDildoTransform.TransformPoint(this._movingPlayerLocalPosition);
			Quaternion quaternion = this._activeDildoTransform.rotation * this._movingPlayerLocalRotation;
			transform.SetPositionAndRotation(vector, quaternion);
			if (this._alignedPlayerBody != null)
			{
				this._alignedPlayerBody.position = vector;
				this._alignedPlayerBody.rotation = quaternion;
			}
		}

		// Token: 0x060087D1 RID: 34769 RVA: 0x00378D3B File Offset: 0x00376F3B
		private void FixedUpdate()
		{
			this.FollowMovingPlacement();
		}

		// Token: 0x060087D2 RID: 34770 RVA: 0x00378D43 File Offset: 0x00376F43
		private IEnumerator ReEnableControllerNextFrame(vThirdPersonController cc, int sessionVersion)
		{
			yield return null;
			this._controllerEnableRoutine = null;
			if (this.CanContinueSession(sessionVersion) && cc == this._alignmentController)
			{
				this.RestoreAlignmentController();
			}
			yield break;
		}

		// Token: 0x060087D3 RID: 34771 RVA: 0x00378D60 File Offset: 0x00376F60
		private void RestoreAlignmentController()
		{
			if (this._controllerEnableRoutine != null)
			{
				base.StopCoroutine(this._controllerEnableRoutine);
				this._controllerEnableRoutine = null;
			}
			if (this._alignmentController != null)
			{
				this._alignmentController.enabled = this._alignmentControllerWasEnabled;
			}
			this._alignmentController = null;
		}

		// Token: 0x060087D4 RID: 34772 RVA: 0x00378DAE File Offset: 0x00376FAE
		private bool CanContinueSession(int sessionVersion)
		{
			return this._isExecuting && base.isActiveAndEnabled && sessionVersion == this._sessionVersion && this._activityLease != null && this._activityLease.IsActive;
		}

		// Token: 0x060087D5 RID: 34773 RVA: 0x00378DDE File Offset: 0x00376FDE
		private void CompleteOwnedFade(ScreenFadeController fade, int sessionVersion)
		{
			if (sessionVersion == this._sessionVersion && this._ownedFade == fade)
			{
				this._ownedFade = null;
			}
		}

		// Token: 0x060087D6 RID: 34774 RVA: 0x00378E00 File Offset: 0x00377000
		private void CancelOwnedFade()
		{
			ScreenFadeController ownedFade = this._ownedFade;
			this._ownedFade = null;
			if (ownedFade != null)
			{
				ownedFade.SetClear();
			}
		}

		// Token: 0x060087D7 RID: 34775 RVA: 0x00378E2C File Offset: 0x0037702C
		private void StopOwnedPlayerAction(bool cancel)
		{
			if (!this._ownsPlayerAction)
			{
				return;
			}
			this._ownsPlayerAction = false;
			if (this.playerActionAnimator == null)
			{
				return;
			}
			vGenericAction genericAction = this.playerActionAnimator.GenericAction;
			if (genericAction != null && genericAction.triggerAction != null && genericAction.triggerAction != this._ownedActionTrigger)
			{
				return;
			}
			if (cancel)
			{
				this.playerActionAnimator.CancelCurrentAction();
			}
			else
			{
				this.playerActionAnimator.FinishCurrentAction();
			}
			this.playerActionAnimator.RestoreOverrideLayer();
		}

		// Token: 0x060087D8 RID: 34776 RVA: 0x00378EB8 File Offset: 0x003770B8
		private void RestoreActionTrigger()
		{
			if (this._ownedActionTrigger == null)
			{
				return;
			}
			this._ownedActionTrigger.useTriggerRotation = this._originalUseTriggerRotation;
			this._ownedActionTrigger.matchTarget = this._originalMatchTarget;
			this._ownedActionTrigger = null;
			this._originalMatchTarget = null;
		}

		// Token: 0x060087D9 RID: 34777 RVA: 0x00378F04 File Offset: 0x00377104
		private Transform ResolveAlignmentTransform()
		{
			if (this.playerRoot != null)
			{
				return this.playerRoot;
			}
			if (this.playerInput != null && this.playerInput.cc != null)
			{
				return this.playerInput.cc.transform;
			}
			return base.transform;
		}

		// Token: 0x060087DA RID: 34778 RVA: 0x00378F5E File Offset: 0x0037715E
		private void BeginActionPresentation()
		{
			if (!this.UsesTripodCamera)
			{
				this._actionUiScope.Begin(Array.Empty<Transform>());
			}
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (instance == null)
			{
				return;
			}
			instance.Show(this.ResolveStopActionConfig());
		}

		// Token: 0x060087DB RID: 34779 RVA: 0x00378F90 File Offset: 0x00377190
		private InteractionConfigSO ResolveStopActionConfig()
		{
			if (this._runtimeStopConfig == null)
			{
				if (this.stopActionConfig != null && this.stopActionConfig.usableActions != null && this.stopActionConfig.usableActions.Count > 0)
				{
					this._runtimeStopConfig = Object.Instantiate<InteractionConfigSO>(this.stopActionConfig);
					ControllerContextBindings.CopyLabels(this.stopActionConfig, this._runtimeStopConfig);
				}
				else
				{
					this._runtimeStopConfig = ScriptableObject.CreateInstance<InteractionConfigSO>();
					this._runtimeStopConfig.objectName = "Dildo";
					this._runtimeStopConfig.mainActionName = "Stop";
					this._runtimeStopConfig.mainKeyDisplayLabel = "X";
					this._runtimeStopConfig.usableActions.Add(new UsableAction
					{
						actionName = "Stop",
						keyDisplayLabel = "X",
						description = "Stop current action"
					});
					this._runtimeStopConfig.usableActions.Add(new UsableAction
					{
						actionName = "Fly camera",
						keyDisplayLabel = "K",
						description = "Toggle fly camera"
					});
					this._runtimeStopConfig.usableActions.Add(new UsableAction
					{
						actionName = "Change camera",
						keyDisplayLabel = "C",
						description = "Next camera preset"
					});
				}
				this._runtimeStopConfig.hideFlags = HideFlags.HideAndDontSave;
			}
			InteractionConfigSO interactionConfigSO = this._runtimeStopConfig;
			if (this.UsesTripodCamera)
			{
				if (this._runtimeTripodStopConfig == null)
				{
					this._runtimeTripodStopConfig = Object.Instantiate<InteractionConfigSO>(this._runtimeStopConfig);
					this._runtimeTripodStopConfig.hideFlags = HideFlags.HideAndDontSave;
					this._runtimeTripodStopConfig.name = "Placed action (Tripod)";
					ControllerContextBindings.CopyLabels(this._runtimeStopConfig, this._runtimeTripodStopConfig);
					this._runtimeTripodStopConfig.usableActions.RemoveAll((UsableAction action) => action != null && (action.keyDisplayLabel == "K" || action.keyDisplayLabel == "C"));
				}
				interactionConfigSO = this._runtimeTripodStopConfig;
			}
			ControllerContextBindings.SetLabel(interactionConfigSO, "X", "B", false);
			ControllerContextBindings.SetInputLabel(interactionConfigSO, "K", this.flyModeInput);
			if (this.detectionCameraCtrl != null)
			{
				ControllerContextBindings.SetInputLabel(interactionConfigSO, "C", this.detectionCameraCtrl.CycleCameraInput);
			}
			ControllerContextBindings.SetLabel(interactionConfigSO, "CTRL + MW", "LT + RS", false);
			bool flag = this._currentAction != null && this._currentAction.UsesMasturbationVoiceAndMeter;
			for (int i = interactionConfigSO.usableActions.Count - 1; i >= 0; i--)
			{
				if (!(interactionConfigSO.usableActions[i].keyDisplayLabel != "CTRL + MW"))
				{
					if (flag)
					{
						return interactionConfigSO;
					}
					interactionConfigSO.usableActions.RemoveAt(i);
				}
			}
			if (flag)
			{
				interactionConfigSO.usableActions.Add(new UsableAction
				{
					actionName = "speed",
					keyDisplayLabel = "CTRL + MW"
				});
			}
			return interactionConfigSO;
		}

		// Token: 0x060087DC RID: 34780 RVA: 0x00379268 File Offset: 0x00377468
		private void Cleanup()
		{
			this._isExecuting = false;
			this._activeRoutine = null;
			this._sessionVersion++;
			this.CancelOwnedFade();
			this.CleanupActionPlayback();
			this.CleanupAudio();
			if (this._hasSavedPlayerState || this._activeDildoTransform != null)
			{
				this.RestoreOriginalState();
			}
			this._interactionTarget = null;
			this._tracksInteractionTarget = false;
			this.RestoreAlignmentController();
			this.RestoreActionTrigger();
			this._currentAction = null;
			if (this._ownsInputLock && this.playerInput != null)
			{
				LoadingScreenController existingInstance = LoadingScreenController.ExistingInstance;
				if (existingInstance == null || !existingInstance.TryReleaseCapturedPlayerInputLock(this.playerInput, this._savedInputLock))
				{
					this.playerInput.SetLockAllInput(this._savedInputLock);
				}
			}
			this._ownsInputLock = false;
			this.StopEmoteCamera();
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (instance != null)
			{
				instance.HideIfCurrent(this._runtimeStopConfig);
			}
			InteractionHUDController instance2 = InteractionHUDController.Instance;
			if (instance2 != null)
			{
				instance2.HideIfCurrent(this._runtimeTripodStopConfig);
			}
			this._actionUiScope.Restore();
			if (this._hasSavedFaceExpression)
			{
				this.RestoreFaceExpression();
			}
			this.ReleaseActionContext();
		}

		// Token: 0x060087DD RID: 34781 RVA: 0x00379384 File Offset: 0x00377584
		private void SetupAudioForAction(DildoActionDefinition action, Transform dildoTransform)
		{
			if (action == null)
			{
				return;
			}
			this.CleanupAudio();
			if (action.UsesMasturbationVoiceAndMeter)
			{
				MasturbationVoiceController masturbationVoiceController = ((this.playerRoot != null) ? this.playerRoot.GetComponentInChildren<MasturbationVoiceController>(true) : base.GetComponentInParent<MasturbationVoiceController>());
				if (masturbationVoiceController != null && masturbationVoiceController.TryBeginExternalSession(this))
				{
					this._activeVoiceController = masturbationVoiceController;
				}
			}
			if (this.debugLogs)
			{
				Debug.Log("[DildoActionExecutor] SetupAudioForAction START | dildoTransform:" + (((dildoTransform != null) ? dildoTransform.name : null) ?? "null"));
			}
			this._activeAudioController = null;
			if (dildoTransform != null)
			{
				this._activeAudioController = dildoTransform.GetComponentInChildren<DildoAudioController>();
				if (this.debugLogs)
				{
					Debug.Log("[DildoActionExecutor] Found on dildo: " + ((this._activeAudioController != null) ? this._activeAudioController.gameObject.name : "null"));
				}
			}
			if (this._activeAudioController == null && this.playerRoot != null)
			{
				this._activeAudioController = this.playerRoot.GetComponentInChildren<DildoAudioController>();
				if (this.debugLogs)
				{
					Debug.Log("[DildoActionExecutor] Found on player: " + ((this._activeAudioController != null) ? this._activeAudioController.gameObject.name : "null"));
				}
			}
			if (this._activeAudioController == null)
			{
				if (this.debugLogs)
				{
					Debug.LogWarning("[DildoActionExecutor] No DildoAudioController found - contact sounds unavailable for this action");
				}
				return;
			}
			if (this.debugLogs)
			{
				Debug.Log("[DildoActionExecutor] Audio controller found: " + this._activeAudioController.gameObject.name);
			}
			this._activeAudioController.ConfigureInteractionClips(action.InteractionSoundClips, action.InteractionVolume);
			this._activeAudioController.ConfigurePleasureClips(action.PleasureSoundClips, action.PleasureVolume);
			this._activeAudioController.Activate();
			if (this.animationEventRelay != null)
			{
				this.animationEventRelay.Bind(this._activeAudioController);
				if (this.debugLogs)
				{
					Debug.Log("[DildoActionExecutor] AnimationEventRelay bound to controller " + this._activeAudioController.gameObject.name);
				}
			}
			else if (this.debugLogs)
			{
				Debug.LogWarning("[DildoActionExecutor] AnimationEventRelay is null - interaction sounds will not work");
			}
			this._pleasureSoundTriggered = false;
			if (action.UsesMasturbationVoiceAndMeter && this._activeVoiceController == null && action.PleasureSoundClips != null && action.PleasureSoundClips.Length != 0)
			{
				this._initialPleasureSoundRoutine = base.StartCoroutine(this.PlayPleasureSoundWithDelay(action));
			}
			if (this.debugLogs)
			{
				string text = "[DildoActionExecutor] Audio system activated. Interaction clips: {0}, Pleasure clips: {1}";
				AudioClip[] interactionSoundClips = action.InteractionSoundClips;
				object obj = ((interactionSoundClips != null) ? interactionSoundClips.Length : 0);
				AudioClip[] pleasureSoundClips = action.PleasureSoundClips;
				Debug.Log(string.Format(text, obj, (pleasureSoundClips != null) ? pleasureSoundClips.Length : 0));
			}
		}

		// Token: 0x060087DE RID: 34782 RVA: 0x0037962D File Offset: 0x0037782D
		private IEnumerator PlayPleasureSoundWithDelay(DildoActionDefinition action)
		{
			yield return new WaitForSeconds(action.PleasureSoundDelay);
			if (this._isExecuting && this._currentAction == action && action.UsesMasturbationVoiceAndMeter && this._activeVoiceController == null && this._activeAudioController != null && !this._pleasureSoundTriggered)
			{
				this._activeAudioController.PlayPleasureSound(action.PleasureSoundClips, action.PleasureVolume);
				this._pleasureSoundTriggered = true;
				if (this.debugLogs)
				{
					Debug.Log("[DildoActionExecutor] Initial pleasure sound played");
				}
			}
			this._initialPleasureSoundRoutine = null;
			yield break;
		}

		// Token: 0x060087DF RID: 34783 RVA: 0x00379643 File Offset: 0x00377843
		private IEnumerator PleasureSoundLoopRoutine(DildoActionDefinition action)
		{
			while (this._isExecuting && this._currentAction == action && action.UsesMasturbationVoiceAndMeter && this._activeVoiceController == null && this._activeAudioController != null && action.PleasureLoopDelay > 0f)
			{
				yield return new WaitForSeconds(action.PleasureLoopDelay);
				if (!this._isExecuting || this._currentAction != action || !action.UsesMasturbationVoiceAndMeter || this._activeVoiceController != null)
				{
					break;
				}
				DildoAudioController activeAudioController = this._activeAudioController;
				if (activeAudioController != null)
				{
					activeAudioController.PlayPleasureSound(action.PleasureSoundClips, action.PleasureVolume);
				}
				if (this.debugLogs)
				{
					Debug.Log(string.Format("[DildoActionExecutor] Loop pleasure sound | Delay:{0:F1}s | Volume:{1:F2}", action.PleasureLoopDelay, action.PleasureVolume));
				}
			}
			this._pleasureSoundRoutine = null;
			yield break;
		}

		// Token: 0x060087E0 RID: 34784 RVA: 0x0037965C File Offset: 0x0037785C
		private void CleanupAudio()
		{
			if (this._activeOrgasmMeter != null)
			{
				this._activeOrgasmMeter.OnMaxReached -= this.HandleOrgasmMaxReached;
				if (this._orgasmMaxReached)
				{
					this._activeOrgasmMeter.UnlockAndCooldown();
				}
				else
				{
					this._activeOrgasmMeter.ForceCooldown();
				}
				this._activeOrgasmMeter = null;
			}
			this._orgasmMaxReached = false;
			if (this._activeVoiceController != null)
			{
				this._activeVoiceController.EndExternalSession(this);
				this._activeVoiceController = null;
			}
			if (this._initialPleasureSoundRoutine != null)
			{
				base.StopCoroutine(this._initialPleasureSoundRoutine);
				this._initialPleasureSoundRoutine = null;
			}
			if (this._pleasureSoundRoutine != null)
			{
				base.StopCoroutine(this._pleasureSoundRoutine);
				this._pleasureSoundRoutine = null;
			}
			if (this.animationEventRelay != null && this._activeAudioController != null)
			{
				this.animationEventRelay.Unbind(this._activeAudioController);
			}
			if (this._activeAudioController != null)
			{
				this._activeAudioController.Deactivate();
				this._activeAudioController = null;
			}
			this._pleasureSoundTriggered = false;
			if (this.debugLogs)
			{
				Debug.Log("[DildoActionExecutor] Audio system cleaned up");
			}
		}

		// Token: 0x060087E1 RID: 34785 RVA: 0x0037977C File Offset: 0x0037797C
		private void SetupActionPlayback(DildoActionDefinition action, Transform prop)
		{
			this.CleanupActionPlayback();
			this._climaxPlaying = (this._climaxFinished = (this._completionEffectFired = false));
			this._playbackSpeed = (this._targetPlaybackSpeed = 1f);
			if (!action.UsesMasturbationVoiceAndMeter)
			{
				return;
			}
			this._playbackAnimator = ((this.playerInput != null && this.playerInput.cc != null) ? this.playerInput.cc.animator : null);
			if (this._playbackAnimator == null && this.playerRoot != null)
			{
				this._playbackAnimator = this.playerRoot.GetComponentInChildren<Animator>(true);
			}
			this._propAnimator = ((prop != null) ? prop.GetComponentInChildren<Animator>() : null);
			this._speedWidget = ((this.playerRoot != null) ? this.playerRoot.GetComponentInChildren<MasturbationSpeedController>(true) : null);
			this._completionProps = ((this.playerRoot != null) ? this.playerRoot.GetComponentInChildren<EmotePropHandler>(true) : null);
			this._hasSpeedParameter = DildoActionExecutor.HasFloatParameter(this._playbackAnimator, "EmoteSpeedMultiplier");
			if (this._hasSpeedParameter)
			{
				this._savedPlaybackSpeedParameter = this._playbackAnimator.GetFloat("EmoteSpeedMultiplier");
			}
			if (this._propAnimator != null)
			{
				this._savedPropAnimatorSpeed = this._propAnimator.speed;
			}
			this._savedSuppressCameraZoom = this.playerInput != null && this.playerInput.suppressCameraZoom;
			this._targetPlaybackSpeed = Mathf.Clamp(action.DefaultPlaybackSpeed, action.MinPlaybackSpeed, action.MaxPlaybackSpeed);
			this._playbackStartFrame = Time.frameCount;
			this._playbackReady = true;
			this.ApplyPlaybackSpeed(this._targetPlaybackSpeed);
			if (this._speedWidget != null)
			{
				this._speedWidget.ShowExternalSpeed(this._targetPlaybackSpeed);
			}
			if (this._completionProps != null && action.MasturbationSettings != null)
			{
				this._completionProps.SetupProps(action.MasturbationSettings);
				this._ownsCompletionProps = true;
			}
		}

		// Token: 0x060087E2 RID: 34786 RVA: 0x00379990 File Offset: 0x00377B90
		private static bool HasFloatParameter(Animator animator, string parameter)
		{
			if (animator == null || animator.runtimeAnimatorController == null)
			{
				return false;
			}
			foreach (AnimatorControllerParameter animatorControllerParameter in animator.parameters)
			{
				if (animatorControllerParameter.type == AnimatorControllerParameterType.Float && animatorControllerParameter.name == parameter)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060087E3 RID: 34787 RVA: 0x003799EC File Offset: 0x00377BEC
		private void ApplyPlaybackSpeed(float value)
		{
			this._playbackSpeed = value;
			if (this._hasSpeedParameter && this._playbackAnimator != null)
			{
				this._playbackAnimator.SetFloat("EmoteSpeedMultiplier", value);
			}
			if (this._propAnimator != null)
			{
				this._propAnimator.speed = value;
			}
		}

		// Token: 0x060087E4 RID: 34788 RVA: 0x00379A44 File Offset: 0x00377C44
		private void TickActionPlayback(float deltaTime)
		{
			if (!this._isExecuting || !this._playbackReady || this._currentAction == null || this._climaxFinished)
			{
				return;
			}
			if (this._climaxPlaying)
			{
				this.TickCompletionPlayback(Mathf.Max(0f, deltaTime));
				return;
			}
			if (TextInputFocus.BlocksGameplayInput || Time.timeScale <= 0f)
			{
				return;
			}
			bool flag = InventoryControllerInput.SpeedModifierHeld(null);
			if (this.playerInput != null)
			{
				this.playerInput.suppressCameraZoom = flag;
			}
			int num = InventoryControllerInput.SpeedStep(flag, ref this._controllerSpeedDirection, ref this._controllerSpeedRepeat);
			if (num != 0)
			{
				this.AdjustPlaybackSpeed(num);
			}
			float num2 = 1f - Mathf.Pow(0.8f, Mathf.Max(0f, deltaTime) * 60f);
			this.ApplyPlaybackSpeed(Mathf.Lerp(this._playbackSpeed, this._targetPlaybackSpeed, num2));
		}

		// Token: 0x060087E5 RID: 34789 RVA: 0x00379B20 File Offset: 0x00377D20
		private void AdjustPlaybackSpeed(int notches)
		{
			if (!this._playbackReady || this._currentAction == null || this._climaxPlaying)
			{
				return;
			}
			this._targetPlaybackSpeed = Mathf.Clamp(this._targetPlaybackSpeed + (float)notches * 0.15f, this._currentAction.MinPlaybackSpeed, this._currentAction.MaxPlaybackSpeed);
			if (this._speedWidget != null)
			{
				this._speedWidget.ShowExternalSpeed(this._targetPlaybackSpeed);
			}
		}

		// Token: 0x060087E6 RID: 34790 RVA: 0x00379B9C File Offset: 0x00377D9C
		private void BeginCompletionPlayback()
		{
			if (this._climaxPlaying || !this._playbackReady || this._currentAction == null || !this._currentAction.UsesMasturbationVoiceAndMeter)
			{
				return;
			}
			this._climaxPlaying = true;
			this._climaxElapsed = 0f;
			this.ApplyPlaybackSpeed(1f);
			if (this.playerInput != null)
			{
				this.playerInput.suppressCameraZoom = false;
			}
			if (this._speedWidget != null)
			{
				this._speedWidget.HideExternalSpeed();
			}
			int num = ((this._currentAction.PlayerActionEntry != null && this._currentAction.PlayerActionEntry.TriggerPrefab != null) ? this._currentAction.PlayerActionEntry.TriggerPrefab.animatorLayer : 0);
			if (this._playbackAnimator == null || this._currentAction.OrgasmAnimationClip == null || string.IsNullOrEmpty(this._currentAction.OrgasmAnimationState) || !this._playbackAnimator.HasState(num, Animator.StringToHash(this._currentAction.OrgasmAnimationState)))
			{
				Debug.LogWarning("[DildoActionExecutor] Missing authored completion state for " + this._currentAction.name);
				this._climaxFinished = true;
				return;
			}
			this._playbackAnimator.CrossFadeInFixedTime(this._currentAction.OrgasmAnimationState, 0.2f, num, 0f);
			if (DildoActionExecutor.HasFloatParameter(this._propAnimator, "Excitement"))
			{
				this._propAnimator.SetFloat("Excitement", 1f);
				AnimatorStateInfo animatorStateInfo = (this._propAnimator.IsInTransition(0) ? this._propAnimator.GetNextAnimatorStateInfo(0) : this._propAnimator.GetCurrentAnimatorStateInfo(0));
				if (animatorStateInfo.fullPathHash != 0)
				{
					this._propAnimator.Play(animatorStateInfo.fullPathHash, 0, 0f);
				}
			}
		}

		// Token: 0x060087E7 RID: 34791 RVA: 0x00379D68 File Offset: 0x00377F68
		private void TickCompletionPlayback(float deltaTime)
		{
			this._climaxElapsed += deltaTime;
			float num = ((this._currentAction.OrgasmAnimationClip != null) ? this._currentAction.OrgasmAnimationClip.length : 0f);
			int num2 = ((this._currentAction.PlayerActionEntry != null && this._currentAction.PlayerActionEntry.TriggerPrefab != null) ? this._currentAction.PlayerActionEntry.TriggerPrefab.animatorLayer : 0);
			int num3 = Animator.StringToHash(this._currentAction.OrgasmAnimationState ?? string.Empty);
			AnimatorStateInfo animatorStateInfo = ((this._playbackAnimator != null) ? this._playbackAnimator.GetCurrentAnimatorStateInfo(num2) : default(AnimatorStateInfo));
			if (this._playbackAnimator != null && this._playbackAnimator.IsInTransition(num2))
			{
				AnimatorStateInfo nextAnimatorStateInfo = this._playbackAnimator.GetNextAnimatorStateInfo(num2);
				if (nextAnimatorStateInfo.fullPathHash == num3 || nextAnimatorStateInfo.shortNameHash == num3)
				{
					animatorStateInfo = nextAnimatorStateInfo;
				}
			}
			bool flag = animatorStateInfo.fullPathHash == num3 || animatorStateInfo.shortNameHash == num3;
			float num4 = (flag ? (Mathf.Max(0f, animatorStateInfo.normalizedTime) * num) : 0f);
			if (flag && !this._completionEffectFired && num4 >= this._currentAction.OrgasmEffectDelay)
			{
				this._completionEffectFired = true;
				if (this._ownsCompletionProps && this._completionProps != null)
				{
					this._completionProps.ActivateProp("Squirt");
				}
			}
			if ((flag && animatorStateInfo.normalizedTime >= 0.98f) || this._climaxElapsed >= num + 3f)
			{
				this._climaxFinished = true;
			}
		}

		// Token: 0x060087E8 RID: 34792 RVA: 0x00379F14 File Offset: 0x00378114
		private void CleanupActionPlayback()
		{
			if (!this._playbackReady)
			{
				return;
			}
			if (this._ownsCompletionProps && this._completionProps != null)
			{
				this._completionProps.CleanupAllProps();
			}
			this._ownsCompletionProps = false;
			if (this._hasSpeedParameter && this._playbackAnimator != null)
			{
				this._playbackAnimator.SetFloat("EmoteSpeedMultiplier", this._savedPlaybackSpeedParameter);
			}
			if (this._propAnimator != null)
			{
				this._propAnimator.speed = this._savedPropAnimatorSpeed;
				if (DildoActionExecutor.HasFloatParameter(this._propAnimator, "Excitement"))
				{
					this._propAnimator.SetFloat("Excitement", 0f);
				}
			}
			if (this._speedWidget != null)
			{
				this._speedWidget.HideExternalSpeed();
			}
			if (this.playerInput != null)
			{
				this.playerInput.suppressCameraZoom = this._savedSuppressCameraZoom;
			}
			this._playbackReady = false;
			this._playbackAnimator = (this._propAnimator = null);
			this._speedWidget = null;
			this._completionProps = null;
			this._playbackSpeed = (this._targetPlaybackSpeed = 1f);
		}

		// Token: 0x060087E9 RID: 34793 RVA: 0x0037A038 File Offset: 0x00378238
		private void Update()
		{
			if (this._isExecuting && (this._activityLease == null || !this._activityLease.IsActive))
			{
				this.CancelAction();
				return;
			}
			if (this._isExecuting && this._tracksInteractionTarget && !this.CanUsePlacementTarget(this._interactionTarget))
			{
				this.CancelAction();
				return;
			}
			this.TickActionPlayback(Time.deltaTime);
			if (Time.frameCount > this._playbackStartFrame)
			{
				this.TickMasturbationProgress(Time.deltaTime);
			}
		}

		// Token: 0x060087EA RID: 34794 RVA: 0x0037A0B1 File Offset: 0x003782B1
		private void LateUpdate()
		{
			this.FollowMovingPlacement();
			if (this._isExecuting)
			{
				this._actionUiScope.Refresh();
				InteractionHUDController instance = InteractionHUDController.Instance;
				if (instance == null)
				{
					return;
				}
				instance.Show(this.ResolveStopActionConfig());
			}
		}

		// Token: 0x060087EB RID: 34795 RVA: 0x0037A0E4 File Offset: 0x003782E4
		private void TickMasturbationProgress(float deltaTime)
		{
			if (!this._isExecuting || !this._playbackReady || this._currentAction == null || !this._currentAction.UsesMasturbationVoiceAndMeter || this._orgasmMaxReached)
			{
				return;
			}
			this.TryBindOrgasmMeter();
			if (this._currentAction.Excitement >= 1f && !this._orgasmMaxReached)
			{
				this.HandleOrgasmMaxReached();
			}
			if (this._activeOrgasmMeter != null && !this._orgasmMaxReached)
			{
				this._activeOrgasmMeter.AddProgress(this._currentAction.OrgasmProgressPerSecond * this._playbackSpeed * Mathf.Max(0f, deltaTime));
			}
		}

		// Token: 0x060087EC RID: 34796 RVA: 0x0037A18C File Offset: 0x0037838C
		private void TryBindOrgasmMeter()
		{
			if (!this._playbackReady || this._currentAction == null || !this._currentAction.UsesMasturbationVoiceAndMeter || this._activeOrgasmMeter != null || OrgasmMeterController.Instance == null)
			{
				return;
			}
			this._activeOrgasmMeter = OrgasmMeterController.Instance;
			this._activeOrgasmMeter.OnMaxReached += this.HandleOrgasmMaxReached;
			if (this._activeOrgasmMeter.Value >= 1f)
			{
				this.HandleOrgasmMaxReached();
			}
		}

		// Token: 0x060087ED RID: 34797 RVA: 0x0037A214 File Offset: 0x00378414
		private void HandleOrgasmMaxReached()
		{
			if (this._currentAction == null || !this._currentAction.UsesMasturbationVoiceAndMeter || this._orgasmMaxReached)
			{
				return;
			}
			this._orgasmMaxReached = true;
			if (this._activeVoiceController != null)
			{
				this._activeVoiceController.PlayExternalEnding(this);
			}
			if (this._playbackReady)
			{
				this.BeginCompletionPlayback();
			}
		}

		// Token: 0x060087EE RID: 34798 RVA: 0x0037A274 File Offset: 0x00378474
		private void OnDisable()
		{
			if (this._isExecuting || this._activityLease != null || this._ownedFade != null || this._hasSavedPlayerState)
			{
				this.CancelAction();
				return;
			}
			this.CleanupAudio();
			this._actionUiScope.Restore();
		}

		// Token: 0x060087EF RID: 34799 RVA: 0x0037A2B4 File Offset: 0x003784B4
		private void OnDestroy()
		{
			if (this._runtimeStopConfig != null)
			{
				Object.Destroy(this._runtimeStopConfig);
			}
			if (this._runtimeTripodStopConfig != null)
			{
				Object.Destroy(this._runtimeTripodStopConfig);
			}
		}

		// Token: 0x04008A48 RID: 35400
		[Header("Explicit References")]
		[Tooltip("The root transform of the player")]
		[SerializeField]
		private Transform playerRoot;

		// Token: 0x04008A49 RID: 35401
		[Tooltip("Reference to the animator that handles actions.")]
		[SerializeField]
		private PlayerActionAnimator playerActionAnimator;

		// Token: 0x04008A4A RID: 35402
		[Tooltip("The input component on the player")]
		[SerializeField]
		private vThirdPersonInput playerInput;

		// Token: 0x04008A4B RID: 35403
		[Tooltip("Reference to the blend shape controller (suppressed when Face layer is disabled)")]
		[SerializeField]
		private CharacterBlendShapeController blendShapeController;

		// Token: 0x04008A4C RID: 35404
		[Tooltip("Reference to the player emotion controller (for facial expression override during actions)")]
		[SerializeField]
		private PlayerEmotionController playerEmotionController;

		// Token: 0x04008A4D RID: 35405
		[Header("Audio System")]
		[Tooltip("Animation event relay for interaction sounds. Must be assigned in inspector.")]
		[SerializeField]
		private DildoAnimationEventRelay animationEventRelay;

		// Token: 0x04008A4E RID: 35406
		[Header("UI & HUD")]
		[Tooltip("Input used to cancel a looping action")]
		[SerializeField]
		private GenericInput cancelActionInput = new GenericInput("X", "B", "B");

		// Token: 0x04008A4F RID: 35407
		[SerializeField]
		private InteractionConfigSO stopActionConfig;

		// Token: 0x04008A50 RID: 35408
		[Tooltip("Main HUD elements to disable during an action and restore after.")]
		[SerializeField]
		private List<GameObject> hudElementsToHide = new List<GameObject>();

		// Token: 0x04008A51 RID: 35409
		[Header("Camera Integration")]
		[Tooltip("Camera controller for preset-based camera switching during actions (like Detection)")]
		[SerializeField]
		private DetectionCameraController detectionCameraCtrl;

		// Token: 0x04008A52 RID: 35410
		[Tooltip("Default camera presets to use when action doesn't specify any")]
		[SerializeField]
		private DetectionCameraPresetCollectionSO defaultEmotePresets;

		// Token: 0x04008A53 RID: 35411
		[Tooltip("Free-fly camera for action viewing (press F to toggle)")]
		[SerializeField]
		private DetectionFlyingCamera flyingCamera;

		// Token: 0x04008A54 RID: 35412
		[Tooltip("UI panel showing camera controls (K for FlyMode, C for next preset)")]
		[SerializeField]
		private GameObject cameraControlsUI;

		// Token: 0x04008A55 RID: 35413
		[Header("Camera Input")]
		[Tooltip("Key to toggle fly camera mode during actions")]
		[SerializeField]
		private GenericInput flyModeInput = new GenericInput("K", "Y", "A");

		// Token: 0x04008A56 RID: 35414
		[Tooltip("Key to cycle to next camera preset")]
		[SerializeField]
		private GenericInput cycleCameraInput = new GenericInput("C", "X", "X");

		// Token: 0x04008A57 RID: 35415
		[Header("Alignment Settings")]
		[SerializeField]
		private bool disableControllerDuringAlign = true;

		// Token: 0x04008A58 RID: 35416
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04008A59 RID: 35417
		private Coroutine _activeRoutine;

		// Token: 0x04008A5A RID: 35418
		private bool _isExecuting;

		// Token: 0x04008A5B RID: 35419
		private PlayerActivityLease _activityLease;

		// Token: 0x04008A5C RID: 35420
		private const PlayerContextResource ActionResources = PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation | PlayerContextResource.HeldItem;

		// Token: 0x04008A5D RID: 35421
		private const PlayerContextResource TripodActionResources = PlayerContextResource.PlayerControl | PlayerContextResource.FullBodyAnimation | PlayerContextResource.HeldItem;

		// Token: 0x04008A5E RID: 35422
		private int _sessionVersion;

		// Token: 0x04008A5F RID: 35423
		private ScreenFadeController _ownedFade;

		// Token: 0x04008A60 RID: 35424
		private bool _ownsPlayerAction;

		// Token: 0x04008A61 RID: 35425
		private vTriggerGenericAction _ownedActionTrigger;

		// Token: 0x04008A62 RID: 35426
		private bool _originalUseTriggerRotation;

		// Token: 0x04008A63 RID: 35427
		private Transform _originalMatchTarget;

		// Token: 0x04008A64 RID: 35428
		private bool _ownsInputLock;

		// Token: 0x04008A65 RID: 35429
		private bool _savedInputLock;

		// Token: 0x04008A66 RID: 35430
		private bool _hasSavedPlayerState;

		// Token: 0x04008A67 RID: 35431
		private Vector3 _originalPlayerLocalScale;

		// Token: 0x04008A68 RID: 35432
		private vThirdPersonController _alignmentController;

		// Token: 0x04008A69 RID: 35433
		private bool _alignmentControllerWasEnabled;

		// Token: 0x04008A6A RID: 35434
		private Coroutine _controllerEnableRoutine;

		// Token: 0x04008A6B RID: 35435
		private InteractionConfigSO _runtimeStopConfig;

		// Token: 0x04008A6C RID: 35436
		private InteractionConfigSO _runtimeTripodStopConfig;

		// Token: 0x04008A6D RID: 35437
		private Transform _originalPlayerParent;

		// Token: 0x04008A6E RID: 35438
		private Scene _originalPlayerScene;

		// Token: 0x04008A6F RID: 35439
		private Vector3 _originalPlayerWorldPosition;

		// Token: 0x04008A70 RID: 35440
		private Quaternion _originalPlayerWorldRotation;

		// Token: 0x04008A71 RID: 35441
		private Vector3 _originalPlayerWorldScale;

		// Token: 0x04008A72 RID: 35442
		private Vector3 _originalPlayerLocalPosition;

		// Token: 0x04008A73 RID: 35443
		private Quaternion _originalPlayerLocalRotation;

		// Token: 0x04008A74 RID: 35444
		private Vector3 _originalDildoLocalPosition;

		// Token: 0x04008A75 RID: 35445
		private Transform _activeDildoTransform;

		// Token: 0x04008A76 RID: 35446
		private Transform _interactionTarget;

		// Token: 0x04008A77 RID: 35447
		private bool _tracksInteractionTarget;

		// Token: 0x04008A78 RID: 35448
		private Transform _movingSupport;

		// Token: 0x04008A79 RID: 35449
		private bool _usesMovingSupport;

		// Token: 0x04008A7A RID: 35450
		private Vector3 _originalSupportLocalPosition;

		// Token: 0x04008A7B RID: 35451
		private Quaternion _originalSupportLocalRotation;

		// Token: 0x04008A7C RID: 35452
		private Vector3 _lastSupportExitPosition;

		// Token: 0x04008A7D RID: 35453
		private Quaternion _lastSupportExitRotation;

		// Token: 0x04008A7E RID: 35454
		private Vector3 _movingPlayerLocalPosition;

		// Token: 0x04008A7F RID: 35455
		private Quaternion _movingPlayerLocalRotation;

		// Token: 0x04008A80 RID: 35456
		private Rigidbody _alignedPlayerBody;

		// Token: 0x04008A81 RID: 35457
		private static readonly HashSet<DildoActionExecutor> MovingSupportOwners = new HashSet<DildoActionExecutor>();

		// Token: 0x04008A82 RID: 35458
		private bool _originalIsKinematic;

		// Token: 0x04008A83 RID: 35459
		private bool _hasRigidbody;

		// Token: 0x04008A84 RID: 35460
		private DildoActionDefinition _currentAction;

		// Token: 0x04008A85 RID: 35461
		private DildoAudioController _activeAudioController;

		// Token: 0x04008A86 RID: 35462
		private MasturbationVoiceController _activeVoiceController;

		// Token: 0x04008A87 RID: 35463
		private OrgasmMeterController _activeOrgasmMeter;

		// Token: 0x04008A88 RID: 35464
		private bool _orgasmMaxReached;

		// Token: 0x04008A89 RID: 35465
		private Coroutine _initialPleasureSoundRoutine;

		// Token: 0x04008A8A RID: 35466
		private Coroutine _pleasureSoundRoutine;

		// Token: 0x04008A8B RID: 35467
		private bool _pleasureSoundTriggered;

		// Token: 0x04008A8C RID: 35468
		private const string PlaybackSpeedParameter = "EmoteSpeedMultiplier";

		// Token: 0x04008A8D RID: 35469
		private Animator _playbackAnimator;

		// Token: 0x04008A8E RID: 35470
		private Animator _propAnimator;

		// Token: 0x04008A8F RID: 35471
		private MasturbationSpeedController _speedWidget;

		// Token: 0x04008A90 RID: 35472
		private EmotePropHandler _completionProps;

		// Token: 0x04008A91 RID: 35473
		private bool _ownsCompletionProps;

		// Token: 0x04008A92 RID: 35474
		private bool _playbackReady;

		// Token: 0x04008A93 RID: 35475
		private bool _hasSpeedParameter;

		// Token: 0x04008A94 RID: 35476
		private bool _climaxPlaying;

		// Token: 0x04008A95 RID: 35477
		private bool _climaxFinished;

		// Token: 0x04008A96 RID: 35478
		private bool _completionEffectFired;

		// Token: 0x04008A97 RID: 35479
		private float _targetPlaybackSpeed = 1f;

		// Token: 0x04008A98 RID: 35480
		private float _playbackSpeed = 1f;

		// Token: 0x04008A99 RID: 35481
		private float _savedPlaybackSpeedParameter = 1f;

		// Token: 0x04008A9A RID: 35482
		private float _savedPropAnimatorSpeed = 1f;

		// Token: 0x04008A9B RID: 35483
		private float _climaxElapsed;

		// Token: 0x04008A9C RID: 35484
		private int _playbackStartFrame;

		// Token: 0x04008A9D RID: 35485
		private bool _savedSuppressCameraZoom;

		// Token: 0x04008A9E RID: 35486
		private Vector3 _savedCameraLocalPosition;

		// Token: 0x04008A9F RID: 35487
		private Quaternion _savedCameraLocalRotation;

		// Token: 0x04008AA0 RID: 35488
		private bool _hasSavedCameraState;

		// Token: 0x04008AA1 RID: 35489
		private CinemachineBrain _ownedBrain;

		// Token: 0x04008AA2 RID: 35490
		private bool _savedBrainEnabled;

		// Token: 0x04008AA3 RID: 35491
		private bool _ownsDetectionCamera;

		// Token: 0x04008AA4 RID: 35492
		private bool _ownsFlyingCamera;

		// Token: 0x04008AA5 RID: 35493
		private int _originalFaceExpressionIndex = -1;

		// Token: 0x04008AA6 RID: 35494
		private bool _hasSavedFaceExpression;

		// Token: 0x04008AA7 RID: 35495
		private bool _suppressedFaceLayer;

		// Token: 0x04008AA8 RID: 35496
		private readonly ActionUiSuppressionScope _actionUiScope = new ActionUiSuppressionScope();

		// Token: 0x04008AA9 RID: 35497
		private int _controllerSpeedDirection;

		// Token: 0x04008AAA RID: 35498
		private float _controllerSpeedRepeat;
	}
}
