using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Bozo.AnimeCharacters;
using Cinemachine;
using ExhibitionistDemo.Movement;
using Features.Achievements;
using Features.BoobTouch;
using Features.DebugTools;
using Features.Detection.Customization;
using Features.GameOver.Newspaper;
using Features.NPC;
using Features.NPC.StateMachine;
using Features.PlayerEmotes;
using Features.PlayerEmotions;
using Features.Police;
using Features.Rank;
using Features.RunModes;
using Features.TimeFreeze;
using Features.WardrobeCombos;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using Pathfinding;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.Rendering;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.Interactions;
using ProjectExhibitionist.NPCs;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VehicleSpawnPlugin;

namespace Features.Detection
{
	// Token: 0x02001291 RID: 4753
	public class ExhibitionEventDirector : MonoBehaviour
	{
		// Token: 0x17001777 RID: 6007
		// (get) Token: 0x06009361 RID: 37729 RVA: 0x003C8A68 File Offset: 0x003C6C68
		// (set) Token: 0x06009362 RID: 37730 RVA: 0x003C8A6F File Offset: 0x003C6C6F
		public static ExhibitionEventDirector Instance { get; private set; }

		// Token: 0x14000102 RID: 258
		// (add) Token: 0x06009363 RID: 37731 RVA: 0x003C8A78 File Offset: 0x003C6C78
		// (remove) Token: 0x06009364 RID: 37732 RVA: 0x003C8AB0 File Offset: 0x003C6CB0
		public event Action<DetectionActorType> OnPlayerReactionStarted;

		// Token: 0x14000103 RID: 259
		// (add) Token: 0x06009365 RID: 37733 RVA: 0x003C8AE8 File Offset: 0x003C6CE8
		// (remove) Token: 0x06009366 RID: 37734 RVA: 0x003C8B20 File Offset: 0x003C6D20
		public event Action<ChoiceCategory, DetectionActorType> OnPlayerCategoryChosen;

		// Token: 0x14000104 RID: 260
		// (add) Token: 0x06009367 RID: 37735 RVA: 0x003C8B58 File Offset: 0x003C6D58
		// (remove) Token: 0x06009368 RID: 37736 RVA: 0x003C8B90 File Offset: 0x003C6D90
		public event Action OnEncounterResolved;

		// Token: 0x14000105 RID: 261
		// (add) Token: 0x06009369 RID: 37737 RVA: 0x003C8BC8 File Offset: 0x003C6DC8
		// (remove) Token: 0x0600936A RID: 37738 RVA: 0x003C8C00 File Offset: 0x003C6E00
		public event Action OnEncounterStarted;

		// Token: 0x14000106 RID: 262
		// (add) Token: 0x0600936B RID: 37739 RVA: 0x003C8C38 File Offset: 0x003C6E38
		// (remove) Token: 0x0600936C RID: 37740 RVA: 0x003C8C70 File Offset: 0x003C6E70
		public event Action<EmoteDefinitionSO> OnEncounterEmoteStarted;

		// Token: 0x0600936D RID: 37741 RVA: 0x003C8CA5 File Offset: 0x003C6EA5
		public void MarkReactionHandled()
		{
			this._reactionHandledExternally = true;
		}

		// Token: 0x0600936E RID: 37742 RVA: 0x003C8CAE File Offset: 0x003C6EAE
		public void RegisterReactionHold(Func<bool> isDone)
		{
			if (isDone != null)
			{
				this._reactionHolds.Add(isDone);
			}
		}

		// Token: 0x17001778 RID: 6008
		// (get) Token: 0x0600936F RID: 37743 RVA: 0x003C8CBF File Offset: 0x003C6EBF
		public bool IsEventActive
		{
			get
			{
				return this._isEventActive;
			}
		}

		// Token: 0x17001779 RID: 6009
		// (get) Token: 0x06009370 RID: 37744 RVA: 0x003C8CC7 File Offset: 0x003C6EC7
		public bool IsChoosingEncounterCategory
		{
			get
			{
				return this._isEventActive && this._isInChoicePhase && !this._isInEmoteSelectionPhase && this._selectedCategory == null;
			}
		}

		// Token: 0x1700177A RID: 6010
		// (get) Token: 0x06009371 RID: 37745 RVA: 0x003C8CF1 File Offset: 0x003C6EF1
		// (set) Token: 0x06009372 RID: 37746 RVA: 0x003C8CF9 File Offset: 0x003C6EF9
		public bool IsResolutionActionPlaying { get; private set; }

		// Token: 0x1700177B RID: 6011
		// (get) Token: 0x06009373 RID: 37747 RVA: 0x003C8D02 File Offset: 0x003C6F02
		// (set) Token: 0x06009374 RID: 37748 RVA: 0x003C8D0A File Offset: 0x003C6F0A
		public EmoteDefinitionSO ActiveResolutionEmote { get; private set; }

		// Token: 0x1700177C RID: 6012
		// (get) Token: 0x06009375 RID: 37749 RVA: 0x003C8D13 File Offset: 0x003C6F13
		public bool CanSkipEncounter
		{
			get
			{
				return this._isEventActive && !this._encounterResolutionStarted && this.GetCurrentActorType() != DetectionActorType.Police && SensoryManager.ResolveActorType(this._detectingNPC) != DetectionActorType.Police;
			}
		}

		// Token: 0x1700177D RID: 6013
		// (get) Token: 0x06009376 RID: 37750 RVA: 0x003C8D41 File Offset: 0x003C6F41
		public NPCSenses DetectingNPC
		{
			get
			{
				return this._detectingNPC;
			}
		}

		// Token: 0x1700177E RID: 6014
		// (get) Token: 0x06009377 RID: 37751 RVA: 0x003C8D49 File Offset: 0x003C6F49
		public Transform PlayerTransform
		{
			get
			{
				if (!(this._cachedPlayerInput != null))
				{
					return null;
				}
				return this._cachedPlayerInput.transform;
			}
		}

		// Token: 0x1700177F RID: 6015
		// (get) Token: 0x06009378 RID: 37752 RVA: 0x003C8D66 File Offset: 0x003C6F66
		public Transform ActiveInitiatorTransform
		{
			get
			{
				EncounterRequest activeEncounterRequest = this._activeEncounterRequest;
				if (activeEncounterRequest == null)
				{
					return null;
				}
				NPCSenses initiator = activeEncounterRequest.initiator;
				if (initiator == null)
				{
					return null;
				}
				return initiator.transform;
			}
		}

		// Token: 0x17001780 RID: 6016
		// (get) Token: 0x06009379 RID: 37753 RVA: 0x003C8D84 File Offset: 0x003C6F84
		public bool HasPendingEncounter
		{
			get
			{
				return this._pendingRecoveryDetection != null || this._pendingRecoveryEncounter != null;
			}
		}

		// Token: 0x0600937A RID: 37754 RVA: 0x003C8D9F File Offset: 0x003C6F9F
		public bool HasPendingDetection(NPCSenses npc)
		{
			if (!(npc != null))
			{
				return false;
			}
			if (!(npc == this._pendingRecoveryDetection))
			{
				EncounterRequest pendingRecoveryEncounter = this._pendingRecoveryEncounter;
				return npc == ((pendingRecoveryEncounter != null) ? pendingRecoveryEncounter.initiator : null);
			}
			return true;
		}

		// Token: 0x17001781 RID: 6017
		// (get) Token: 0x0600937B RID: 37755 RVA: 0x003C8DD4 File Offset: 0x003C6FD4
		public bool IsGropingChoiceSelected
		{
			get
			{
				return this._selectedEmote != null && this._selectedEmote.SpecialAction == EmoteSpecialAction.StartGropingMode;
			}
		}

		// Token: 0x0600937C RID: 37756 RVA: 0x003C8DF4 File Offset: 0x003C6FF4
		public bool AllowsInteractionControls(InteractionConfigSO config)
		{
			return config != null && this._gropingInterludeOwner != null && this._gropingInterludeOwner.IsExternalInteractorMode && this._gropingInterludeOwner.ModeControlsConfig == config;
		}

		// Token: 0x17001782 RID: 6018
		// (get) Token: 0x0600937D RID: 37757 RVA: 0x003C8E2D File Offset: 0x003C702D
		public bool OwnsPhotoModeInput
		{
			get
			{
				return this._isEventActive && this._waitingForEmotePhoto && this.EnableFlyingCamera && this.FlyingCamera != null;
			}
		}

		// Token: 0x0600937E RID: 37758 RVA: 0x003C8E58 File Offset: 0x003C7058
		private void Awake()
		{
			DetectionFlyingCamera.NormalizePhotoInput(this.FlyingCameraEnterInput);
			ControllerContextBindings.Set(this.SkipInput, "B");
			ControllerContextBindings.Set(this.FlyingCameraEnterInput, "Y");
			if (ExhibitionEventDirector.Instance == null)
			{
				ExhibitionEventDirector.Instance = this;
			}
			else
			{
				Object.Destroy(base.gameObject);
			}
			EncounterInputHints encounterInputHints = base.GetComponent<EncounterInputHints>();
			if (encounterInputHints == null)
			{
				encounterInputHints = base.gameObject.AddComponent<EncounterInputHints>();
			}
			encounterInputHints.Bind(this);
			if (this.PlayerEmotePropHandler == null)
			{
				vThirdPersonInput vThirdPersonInput = Object.FindObjectOfType<vThirdPersonInput>();
				if (vThirdPersonInput != null)
				{
					this.PlayerEmotePropHandler = vThirdPersonInput.GetComponent<EmotePropHandler>();
				}
			}
			if (this.EmoteAudioSource == null)
			{
				vThirdPersonInput vThirdPersonInput2 = Object.FindObjectOfType<vThirdPersonInput>();
				if (vThirdPersonInput2 != null)
				{
					this.EmoteAudioSource = vThirdPersonInput2.GetComponent<AudioSource>();
				}
			}
			if (Camera.main != null)
			{
				this._mainCameraBrain = Camera.main.GetComponent<CinemachineBrain>();
				if (this._mainCameraBrain != null)
				{
					this._mainCameraBrain.enabled = false;
				}
			}
		}

		// Token: 0x0600937F RID: 37759 RVA: 0x003C8F5C File Offset: 0x003C715C
		private void OnDisable()
		{
			this.CancelForSceneTransition();
		}

		// Token: 0x06009380 RID: 37760 RVA: 0x003C8F64 File Offset: 0x003C7164
		public void CancelForSceneTransition()
		{
			base.StopAllCoroutines();
			this.CancelPreReactionPresentation();
			this._encounterPresentationCoroutines.Clear();
			this.ClearCooldownsForCancellation();
			this._sequenceCoroutine = null;
			this._concurrentDialogueCoroutine = null;
			this.EndEncounterDialogue(this._encounterDialogueToken, false);
			this.RestoreExitPresentation(this._pendingExitPresentation);
			this.ClearPendingRecoveryDetection();
			if (this._isEventActive && ExhibitionEventDirector.Instance == this)
			{
				LongWayHomeRuntimeController.CancelPendingEncounter(this._detectingNPC);
				this.AbortEncounterForDisable();
			}
			this.UnlockPlayerInput();
			IDisposable encounterTrafficPause = this._encounterTrafficPause;
			if (encounterTrafficPause != null)
			{
				encounterTrafficPause.Dispose();
			}
			this._encounterTrafficPause = null;
			if (this._choiceCameraAnchor != null)
			{
				this._choiceCameraAnchor.enabled = false;
			}
			this.EndDetectionShotProtection();
			this.SetResolutionActionState(false, null);
			if (ExhibitionEventDirector.Instance == this)
			{
				this.SetDetectionFlowActive(false);
			}
		}

		// Token: 0x06009381 RID: 37761 RVA: 0x003C903C File Offset: 0x003C723C
		private void AbortEncounterForDisable()
		{
			bool flag = !this._detectionCameraReleased;
			if (this.EmoteWheel != null)
			{
				this.EmoteWheel.OnEmoteConfirmed -= this.OnEmoteWheelConfirmed;
				this.EmoteWheel.Close();
			}
			if (this.ChoiceCanvas)
			{
				this.ChoiceCanvas.SetActive(false);
			}
			if (this.ChoiceUIRoot)
			{
				this.ChoiceUIRoot.SetActive(false);
			}
			if (this.PlayerReactionCanvas)
			{
				this.PlayerReactionCanvas.SetActive(false);
			}
			if (this.SkipHintUI)
			{
				this.SkipHintUI.SetActive(false);
			}
			if (this.EmoteHintUI)
			{
				this.EmoteHintUI.SetActive(false);
			}
			if (this.SkipCountdownText)
			{
				this.SkipCountdownText.text = "";
			}
			if (flag)
			{
				if (this.FlyingCamera != null && this.FlyingCamera.IsActive)
				{
					this.FlyingCamera.Deactivate();
				}
				vThirdPersonInput vThirdPersonInput = ((this._cachedPlayerInput != null) ? this._cachedPlayerInput : ((this._playerAnimator != null) ? this._playerAnimator.GetComponent<vThirdPersonInput>() : null));
				PlayerActionAnimator playerAnimator = this._playerAnimator;
				if (playerAnimator != null)
				{
					playerAnimator.CancelCurrentAction();
				}
				if (vThirdPersonInput != null && vThirdPersonInput.cc != null)
				{
					this.RestorePlayerAnimatorLayerWeights(vThirdPersonInput.cc.animator);
				}
				this.DeactivateEmoteCameras();
				this.DeactivatePlayerReactionCameras();
				if (this.NPCReactionCamera != null)
				{
					this.NPCReactionCamera.gameObject.SetActive(false);
				}
				if (this._activeCinematicCamera != null)
				{
					this._activeCinematicCamera.SetActive(false);
				}
				this.RestoreGameplayCameraImmediate();
				ScreenFadeController instance = ScreenFadeController.Instance;
				if (instance != null)
				{
					instance.SetClear();
				}
				this.CleanupActiveProps();
				this.ExitNPCIdleState();
				this.RestartNPCBehavior();
				HUDManager instance2 = HUDManager.Instance;
				if (instance2 != null)
				{
					instance2.ShowHUD();
				}
				Time.timeScale = 1f;
				Time.fixedDeltaTime = 0.02f;
			}
			this.ReleaseDetectionCamera();
			this.UnlockPlayerInput();
			if (this._ownsSensoryPause)
			{
				SensoryManager instance3 = SensoryManager.Instance;
				if (instance3 != null)
				{
					instance3.SetPaused(false);
				}
			}
			this._ownsSensoryPause = false;
			this.UnfreezeAllNPCs();
			this.RestoreStaticEncounterActors();
			DetectionEmotionSource instance4 = DetectionEmotionSource.Instance;
			if (instance4 != null)
			{
				instance4.OnDetectionEnded();
			}
			PlayerEmotionController instance5 = PlayerEmotionController.Instance;
			if (instance5 != null)
			{
				instance5.OnDetectionEnded();
			}
			DetectionAudioManager instance6 = DetectionAudioManager.Instance;
			if (instance6 != null)
			{
				instance6.OnDetectionEnded();
			}
			this.DeactivateEncounterOverlay();
			this._isInChoicePhase = false;
			this._isInEmoteSelectionPhase = false;
			this._selectedAction = null;
			this._selectedEmote = null;
			this._selectedCategory = null;
			this._activeEncounterRequest = null;
			this._detectingNPC = null;
		}

		// Token: 0x06009382 RID: 37762 RVA: 0x003C92F0 File Offset: 0x003C74F0
		private PlayerActionAnimator GetPlayerAnimator()
		{
			if (this._playerAnimator == null && this._cachedPlayerInput != null)
			{
				this._playerAnimator = this._cachedPlayerInput.GetComponent<PlayerActionAnimator>();
			}
			if (this._playerAnimator == null)
			{
				this._playerAnimator = Object.FindObjectOfType<PlayerActionAnimator>();
			}
			return this._playerAnimator;
		}

		// Token: 0x06009383 RID: 37763 RVA: 0x003C9349 File Offset: 0x003C7549
		private void Update()
		{
			this.ResumeDetectionAfterRecovery();
			if (!this.EnableShotProtection)
			{
				this.EndDetectionShotProtection();
			}
			if (this.CanSkipEncounter && this.SkipInput != null && this.SkipInput.GetButtonDown())
			{
				this.TrySkipEncounter();
			}
		}

		// Token: 0x06009384 RID: 37764 RVA: 0x003C9384 File Offset: 0x003C7584
		private void LateUpdate()
		{
			this.ReassertPlayerInputLock();
			EncounterInputHints component = base.GetComponent<EncounterInputHints>();
			if (component != null)
			{
				component.SetSkipHintVisible(this.CanSkipEncounter && !this.IsSkipInputBlocked());
			}
		}

		// Token: 0x06009385 RID: 37765 RVA: 0x003C93C1 File Offset: 0x003C75C1
		private bool IsSkipInputBlocked()
		{
			return GameplayInputGate.BlocksGameplayInput || FlyCameraPhotoMode.AnyOpen || (this.FlyingCamera != null && this.FlyingCamera.IsActive) || Time.timeScale <= 0f;
		}

		// Token: 0x06009386 RID: 37766 RVA: 0x003C9400 File Offset: 0x003C7600
		public void TriggerDetection(NPCSenses npc)
		{
			if (!RunModeService.EncountersEnabled)
			{
				return;
			}
			if (TimeFreezeController.IsAnyActive)
			{
				return;
			}
			if (DetectionInteractionGate.IsBlocked)
			{
				return;
			}
			if (RunModeService.IsLongWayHome && !LongWayHomeRuntimeController.CanStartEncounter(npc))
			{
				return;
			}
			if (npc == null || npc.BlocksFinalDetectionWhileSeated || this._isEventActive || this.IsInCooldown(npc))
			{
				return;
			}
			if (WardrobeComboRuntime.ShouldDisableFinalDetection(SensoryManager.ResolveActorType(npc), npc.transform.position))
			{
				return;
			}
			if (PlayerStealthBridge.IsFullyHidden)
			{
				return;
			}
			if (this._pendingRecoveryDetection != null || this._pendingRecoveryEncounter != null)
			{
				return;
			}
			TroupeDetectionProxy component = npc.GetComponent<TroupeDetectionProxy>();
			if (component == null)
			{
				LongWayHomeRuntimeController.MarkEncounterStarted(npc);
			}
			if (DetectionRecoveryGate.MustWaitForPlayer || DetectionRecoveryGate.IsRecovering(npc.transform))
			{
				this._pendingRecoveryDetection = npc;
				return;
			}
			if (component != null)
			{
				component.HandleDetection(npc);
				return;
			}
			this.TriggerEncounter(new EncounterRequest
			{
				initiator = npc
			});
		}

		// Token: 0x06009387 RID: 37767 RVA: 0x003C94E8 File Offset: 0x003C76E8
		public void TriggerEncounter(EncounterRequest request)
		{
			if (!RunModeService.EncountersEnabled)
			{
				return;
			}
			if (TimeFreezeController.IsAnyActive)
			{
				return;
			}
			if (DetectionInteractionGate.IsBlocked)
			{
				return;
			}
			if (RunModeService.IsLongWayHome)
			{
				if (request != null && ((request.profile != null && request.profile.actorType == DetectionActorType.Police) || LongWayHomeRuntimeController.IsPoliceWitness(request.initiator)))
				{
					if (request.initiator == null || request.initiator.BlocksFinalDetectionWhileSeated || this.IsInCooldown(request.initiator) || WardrobeComboRuntime.ShouldDisableFinalDetection((request.profile != null) ? request.profile.actorType : SensoryManager.ResolveActorType(request.initiator), request.initiator.transform.position))
					{
						return;
					}
					if (this._isEventActive && this._activeEncounterRequest == request)
					{
						return;
					}
					if (!LongWayHomeRuntimeController.TryBeginPoliceCapture(request.initiator))
					{
						return;
					}
				}
				else
				{
					if (!LongWayHomeRuntimeController.CanStartEncounter((request != null) ? request.initiator : null))
					{
						return;
					}
					if (request.profile == null || request.profile.actorType != DetectionActorType.Reporter)
					{
						request = new EncounterRequest
						{
							initiator = request.initiator,
							additionalActors = request.additionalActors,
							onSceneEnded = request.onSceneEnded,
							completeUnderFade = request.completeUnderFade
						};
					}
				}
			}
			if (((request != null) ? request.initiator : null) == null || request.initiator.BlocksFinalDetectionWhileSeated || this._isEventActive || this.IsInCooldown(request.initiator))
			{
				return;
			}
			if (WardrobeComboRuntime.ShouldDisableFinalDetection((request.profile != null) ? request.profile.actorType : SensoryManager.ResolveActorType(request.initiator), request.initiator.transform.position))
			{
				return;
			}
			if (PlayerStealthBridge.IsFullyHidden)
			{
				return;
			}
			if (this._pendingRecoveryDetection != null || this._pendingRecoveryEncounter != null)
			{
				return;
			}
			LongWayHomeRuntimeController.MarkEncounterStarted(request.initiator);
			if (DetectionRecoveryGate.MustWait(request))
			{
				this._pendingRecoveryEncounter = request;
				return;
			}
			if (!DetectionPlayerNormalizer.PrepareForEncounter(request.initiator))
			{
				this._pendingRecoveryEncounter = request;
				this._nextPoolRecoveryRetry = Time.unscaledTime + 0.5f;
				return;
			}
			if (!NPCSeatedEncounter.TryPrepare(request.initiator, PlayerRegistry.Player, out this._seatedEncounter))
			{
				this._pendingRecoveryEncounter = request;
				this._nextPoolRecoveryRetry = Time.unscaledTime + 0.5f;
				return;
			}
			this._phoneEncounter = (request.initiator.UsesStaticPhoneReaction ? request.initiator.StaticPhone : null);
			if (this._phoneEncounter != null)
			{
				this._phoneEncounter.PrepareForEncounter();
			}
			this.SetDetectionFlowActive(true);
			this.LockPlayerInput(Object.FindObjectOfType<vThirdPersonInput>(true));
			this._detectingNPC = request.initiator;
			this._activeEncounterRequest = request;
			this._encounterDialogueSelection = null;
			NPCStateMachine component = request.initiator.GetComponent<NPCStateMachine>();
			if (component != null && component.IsInitialized)
			{
				component.ForceState(NPCStateType.Detected, "Director");
			}
			this._sequenceCoroutine = base.StartCoroutine(this.DetectionSequence());
		}

		// Token: 0x06009388 RID: 37768 RVA: 0x003C97E4 File Offset: 0x003C79E4
		private void ResumeDetectionAfterRecovery()
		{
			if (TimeFreezeController.IsAnyActive)
			{
				return;
			}
			if (DetectionInteractionGate.IsGameOver || DetectionTransitionGate.IsBlocked)
			{
				this.ClearPendingRecoveryDetection();
				return;
			}
			bool flag = (RunModeService.IsLongWayHome ? (this._pendingRecoveryDetection != null) : (this._pendingRecoveryDetection != null));
			if (!flag && this._pendingRecoveryEncounter == null)
			{
				return;
			}
			if (Time.unscaledTime < this._nextPoolRecoveryRetry)
			{
				return;
			}
			if (this._isEventActive || NPCSimulationPause.IsPaused || (SensoryManager.Instance != null && SensoryManager.Instance.IsPaused))
			{
				return;
			}
			NPCSenses npcsenses;
			if (!flag)
			{
				EncounterRequest pendingRecoveryEncounter = this._pendingRecoveryEncounter;
				npcsenses = ((pendingRecoveryEncounter != null) ? pendingRecoveryEncounter.initiator : null);
			}
			else
			{
				npcsenses = this._pendingRecoveryDetection;
			}
			NPCSenses npcsenses2 = npcsenses;
			if (npcsenses2 == null || !npcsenses2.gameObject.activeInHierarchy || PlayerStealthBridge.IsFullyHidden)
			{
				this.ClearPendingRecoveryDetection();
				return;
			}
			if (DetectionRecoveryGate.MustWaitForPlayer || DetectionRecoveryGate.IsRecovering(npcsenses2.transform) || (this._pendingRecoveryEncounter != null && DetectionRecoveryGate.MustWait(this._pendingRecoveryEncounter)))
			{
				return;
			}
			NPCSenses pendingRecoveryDetection = this._pendingRecoveryDetection;
			EncounterRequest pendingRecoveryEncounter2 = this._pendingRecoveryEncounter;
			this._pendingRecoveryDetection = null;
			this._pendingRecoveryEncounter = null;
			if (pendingRecoveryDetection != null)
			{
				this.TriggerDetection(pendingRecoveryDetection);
				return;
			}
			this.TriggerEncounter(pendingRecoveryEncounter2);
		}

		// Token: 0x06009389 RID: 37769 RVA: 0x003C9910 File Offset: 0x003C7B10
		private void ClearPendingRecoveryDetection()
		{
			NPCSenses npcsenses;
			if (!(RunModeService.IsLongWayHome ? (this._pendingRecoveryDetection != null) : (this._pendingRecoveryDetection != null)))
			{
				EncounterRequest pendingRecoveryEncounter = this._pendingRecoveryEncounter;
				npcsenses = ((pendingRecoveryEncounter != null) ? pendingRecoveryEncounter.initiator : null);
			}
			else
			{
				npcsenses = this._pendingRecoveryDetection;
			}
			NPCSenses npcsenses2 = npcsenses;
			if (npcsenses2 != null)
			{
				npcsenses2.IsDetected = false;
			}
			if (npcsenses2 != null)
			{
				LongWayHomeRuntimeController.CancelPendingEncounter(npcsenses2);
			}
			this._pendingRecoveryDetection = null;
			this._pendingRecoveryEncounter = null;
		}

		// Token: 0x0600938A RID: 37770 RVA: 0x003C997F File Offset: 0x003C7B7F
		private IEnumerator DetectionSequence()
		{
			this._selectedAction = null;
			this._selectedEmote = null;
			this.SetResolutionActionState(false, null);
			this._selectedCategory = null;
			this._isInEmoteSelectionPhase = false;
			this._isInChoicePhase = false;
			this.ReassertPlayerInputLock();
			UnityEvent<NPCSenses> onDetectionStarted = this.OnDetectionStarted;
			if (onDetectionStarted != null)
			{
				onDetectionStarted.Invoke(this._detectingNPC);
			}
			if (SensoryManager.Instance != null)
			{
				SensoryManager.Instance.SetPaused(true);
				this._ownsSensoryPause = true;
			}
			if (DetectionEmotionSource.Instance != null)
			{
				float num = Vector3.Distance(this._detectingNPC.transform.position, (this._cachedPlayerInput != null) ? this._cachedPlayerInput.transform.position : Vector3.zero);
				DetectionEmotionSource.Instance.OnDetection(num);
			}
			if (PlayerEmotionController.Instance != null)
			{
				PlayerEmotionController.Instance.OnDetectionStarted();
			}
			Action onEncounterStarted = this.OnEncounterStarted;
			if (onEncounterStarted != null)
			{
				onEncounterStarted();
			}
			if (this.GetCurrentActorType() == DetectionActorType.Reporter)
			{
				AchievementTrackingService.RecordReporter();
			}
			EncounterRequest activeEncounterRequest = this._activeEncounterRequest;
			EncounterProfileSO encounterProfileSO = ((activeEncounterRequest != null) ? activeEncounterRequest.profile : null);
			if (encounterProfileSO != null && encounterProfileSO.forceShameSupremeAndFreeze)
			{
				EmotionManager instance = EmotionManager.Instance;
				if (instance != null)
				{
					instance.SetBalance(encounterProfileSO.forcedStartBalance, "PoliceEncounterStart");
					instance.SetFrozen(true);
					Debug.Log(string.Format("[Director] Forced EmotionBalance={0} + froze EmotionManager ", encounterProfileSO.forcedStartBalance) + "(profile '" + encounterProfileSO.name + "')");
				}
				if (PlayerEmotionController.Instance != null)
				{
					PlayerEmotionController.Instance.TriggerExpressionUpdate();
				}
			}
			if (DetectionAudioManager.Instance != null)
			{
				DetectionAudioManager.Instance.OnDetectionStarted();
			}
			Debug.Log("[Director] Detected by " + this._detectingNPC.name + "!");
			this.StopNPCMovement();
			this.FreezeAllNPCsAndSetupSpectators();
			yield return null;
			bool fadeComplete = false;
			if (ScreenFadeController.Instance != null)
			{
				ScreenFadeController.Instance.FadeOut(this.FadeDuration, delegate
				{
					fadeComplete = true;
				});
				while (!fadeComplete)
				{
					yield return null;
				}
			}
			if (HUDManager.Instance != null)
			{
				HUDManager.Instance.HideHUD();
			}
			if (this.GetCurrentActorType() == DetectionActorType.Police && this._cachedPlayerInput != null)
			{
				PoliceDetectionSpacing.TrySeparate(this._detectingNPC, this._cachedPlayerInput.transform, this.PoliceCaptureStandOffDistance);
			}
			this.RotateNPCToFacePlayer();
			this.RotatePlayerToFaceNPC();
			this.PositionAdditionalActors();
			this.ActivateEncounterOverlay();
			this.HideEncounterNPCsOnLayer();
			this.ActivatePlayerFaceCloseUpCamera();
			SimpleDialogueManager instance2 = SimpleDialogueManager.Instance;
			if (instance2 != null)
			{
				instance2.HideImmediate();
			}
			fadeComplete = false;
			if (ScreenFadeController.Instance != null)
			{
				ScreenFadeController.Instance.FadeIn(this.FadeDuration, delegate
				{
					fadeComplete = true;
				});
				while (!fadeComplete)
				{
					yield return null;
				}
			}
			EncounterInputHints component = base.GetComponent<EncounterInputHints>();
			if (component != null)
			{
				component.EmphasizeSkipHint();
			}
			yield return this.StartEncounterPresentation(this.PlayPlayerReactionSequence());
			EncounterRequest activeEncounterRequest2 = this._activeEncounterRequest;
			IEncounterPreReactionPhase preReaction = ((activeEncounterRequest2 != null) ? activeEncounterRequest2.preReactionPhase : null);
			if (preReaction != null)
			{
				this.RestoreEncounterNPCLayers();
				this._preReactionStarted = true;
				CinemachineCore instance3 = CinemachineCore.Instance;
				this._preReactionBrain = ((instance3 != null) ? instance3.GetActiveBrain(0) : null);
				if (this._preReactionBrain != null)
				{
					this._preReactionSavedBlend = this._preReactionBrain.m_DefaultBlend;
					this._preReactionHasSavedBlend = true;
					this._preReactionBrain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, 0f);
				}
				yield return this.StartEncounterPresentation(preReaction.Execute(this));
			}
			this.RestoreEncounterNPCLayers();
			this.ActivateNPCCamera();
			this.DeactivatePlayerReactionCameras();
			if (preReaction != null)
			{
				ExhibitionEventDirector.<>c__DisplayClass182_1 CS$<>8__locals2 = new ExhibitionEventDirector.<>c__DisplayClass182_1();
				CS$<>8__locals2.fadeIn = ScreenFadeController.Instance == null;
				ScreenFadeController instance4 = ScreenFadeController.Instance;
				if (instance4 != null)
				{
					instance4.FadeIn(this.FadeDuration, delegate
					{
						CS$<>8__locals2.fadeIn = true;
					});
				}
				while (!CS$<>8__locals2.fadeIn)
				{
					yield return null;
				}
				this.RestorePreReactionBlend();
				this._preReactionStarted = false;
				CS$<>8__locals2 = null;
			}
			EncounterRequest activeEncounterRequest3 = this._activeEncounterRequest;
			object obj;
			if (activeEncounterRequest3 == null)
			{
				obj = 0;
			}
			else
			{
				EncounterProfileSO profile = activeEncounterRequest3.profile;
				bool? flag = ((profile != null) ? new bool?(profile.skipNPCReaction) : null);
				bool flag2 = true;
				obj = (flag.GetValueOrDefault() == flag2) & (flag != null);
			}
			EncounterRequest activeEncounterRequest4 = this._activeEncounterRequest;
			NPCReactionProfileSO npcreactionProfileSO;
			if (activeEncounterRequest4 == null)
			{
				npcreactionProfileSO = null;
			}
			else
			{
				EncounterProfileSO profile2 = activeEncounterRequest4.profile;
				npcreactionProfileSO = ((profile2 != null) ? profile2.reactionProfileOverride : null);
			}
			NPCReactionProfileSO npcreactionProfileSO2 = npcreactionProfileSO;
			NPCReactionProfileSO npcreactionProfileSO3 = ((npcreactionProfileSO2 != null) ? npcreactionProfileSO2 : this._detectingNPC.ReactionProfile);
			NPCReactionEntry npcreactionEntry2;
			if (!npcreactionProfileSO3)
			{
				NPCReactionEntry npcreactionEntry = new NPCReactionEntry();
				npcreactionEntry.Type = ReactionType.LookAt;
				npcreactionEntry.AnimatorReactionIndex = 0;
				npcreactionEntry2 = npcreactionEntry;
				npcreactionEntry.ReactionDuration = this.ReactionPreCinematicDelay;
			}
			else
			{
				npcreactionEntry2 = npcreactionProfileSO3.GetRandomReaction();
			}
			NPCReactionEntry npcreactionEntry3 = npcreactionEntry2;
			object obj2 = obj;
			if (obj2 == null)
			{
				this.TriggerReactionAnimation(npcreactionEntry3, false);
			}
			else
			{
				Debug.Log("[Director] PHASE 9 skipped — skipNPCReaction=true from profile");
			}
			EncounterRequest activeEncounterRequest5 = this._activeEncounterRequest;
			bool flag3;
			if (activeEncounterRequest5 == null)
			{
				flag3 = false;
			}
			else
			{
				EncounterProfileSO profile3 = activeEncounterRequest5.profile;
				bool? flag = ((profile3 != null) ? new bool?(profile3.startDialogueWithReaction) : null);
				bool flag2 = true;
				flag3 = (flag.GetValueOrDefault() == flag2) & (flag != null);
			}
			bool startDialogueWithReaction = flag3;
			bool concurrentDialogueDone = true;
			if (startDialogueWithReaction && this.UseDialogue)
			{
				Debug.Log("[Director] Starting dialogue concurrently with reaction (Phase 9.5)");
				concurrentDialogueDone = false;
				this._concurrentDialogueCoroutine = this.StartEncounterPresentation(this.WatchCoroutine(this.PlayNPCDialogue(ExhibitionEventDirector.DialoguePhase.Reaction), delegate
				{
					concurrentDialogueDone = true;
					this._concurrentDialogueCoroutine = null;
				}));
			}
			if (obj2 == null)
			{
				float num2 = ((npcreactionEntry3.ReactionDuration > 0f) ? npcreactionEntry3.ReactionDuration : this.ReactionPreCinematicDelay);
				yield return new WaitForSeconds(num2);
			}
			this.TriggerNPCIdleState();
			if (!startDialogueWithReaction && this.UseDialogue)
			{
				string text = "[Director] PHASE 11: UseDialogue={0}, _detectingNPC={1}";
				object obj3 = this.UseDialogue;
				NPCSenses detectingNPC = this._detectingNPC;
				Debug.Log(string.Format(text, obj3, (detectingNPC != null) ? detectingNPC.name : null));
				yield return this.StartEncounterPresentation(this.PlayNPCDialogue(ExhibitionEventDirector.DialoguePhase.Reaction));
			}
			if (startDialogueWithReaction && this.UseDialogue)
			{
				while (!concurrentDialogueDone)
				{
					yield return null;
				}
			}
			this.HideEncounterNPCsOnLayer();
			this.ActivatePlayerChoiceCamera();
			this._isInChoicePhase = true;
			this._selectedCategory = null;
			this._selectedEmote = null;
			this._isInEmoteSelectionPhase = false;
			EncounterRequest activeEncounterRequest6 = this._activeEncounterRequest;
			EncounterProfileSO encounterProfileSO2 = ((activeEncounterRequest6 != null) ? activeEncounterRequest6.profile : null);
			bool skipCategoryChoice = encounterProfileSO2 != null && encounterProfileSO2.skipCategoryChoice;
			if (skipCategoryChoice)
			{
				this._selectedCategory = new ChoiceCategory?(encounterProfileSO2.forcedEmoteCategory);
				Debug.Log(string.Format("[Director] PHASE 13 skipped — forced category '{0}' from profile '{1}'", this._selectedCategory.Value, encounterProfileSO2.name));
			}
			else
			{
				if (this.ChoiceCanvas)
				{
					this.ChoiceCanvas.SetActive(true);
				}
				if (this.ChoiceUIRoot)
				{
					this.ChoiceUIRoot.SetActive(true);
				}
				this.InitializeChoiceButtons();
				UnityEvent onChoicePanelShown = this.OnChoicePanelShown;
				if (onChoicePanelShown != null)
				{
					onChoicePanelShown.Invoke();
				}
				float duration = ((this.SkipHintDuration > 0f) ? this.SkipHintDuration : (this.GlobalConfig ? this.GlobalConfig.DetectionDuration : 3f));
				if (RankRuntimeController.Current != null && RankProgressionService.Config != null)
				{
					duration = Mathf.Max(0.1f, RankProgressionService.Config.choiceTimerSeconds);
				}
				float timer = duration;
				while (timer > 0f && this._isEventActive && this._selectedCategory == null)
				{
					if (FlyCameraPhotoMode.AnyOpen)
					{
						yield return null;
					}
					else
					{
						timer -= Time.unscaledDeltaTime;
						if (this.SkipTimerFill)
						{
							this.SkipTimerFill.fillAmount = timer / duration;
						}
						if (this.SkipCountdownText)
						{
							this.SkipCountdownText.text = string.Format("{0}s", Mathf.CeilToInt(timer));
						}
						foreach (ExhibitionEventDirector.ChoiceActionConfig choiceActionConfig in this.AvailableChoices)
						{
							if (choiceActionConfig.ButtonUI != null && choiceActionConfig.ButtonUI.gameObject.activeInHierarchy)
							{
								choiceActionConfig.ButtonUI.UpdateInput();
							}
						}
						yield return null;
					}
				}
				if (this._selectedCategory == null)
				{
					if (!RunModeService.IsLongWayHome && !(RankRuntimeController.Current != null))
					{
						this._isInChoicePhase = false;
						if (this._isEventActive)
						{
							this.ResolveEvent(null, null);
						}
						yield break;
					}
					this._selectedCategory = new ChoiceCategory?(ChoiceCategory.Shame);
					Debug.Log("[Director] Choice timeout → auto-SHAME.");
				}
			}
			this._isInEmoteSelectionPhase = true;
			if (this.SkipTimerFill)
			{
				this.SkipTimerFill.fillAmount = 0f;
			}
			if (this.SkipCountdownText)
			{
				this.SkipCountdownText.text = "";
			}
			if (!skipCategoryChoice)
			{
				this.ApplyChoiceButtonVisualStates(this._selectedCategory.Value);
				this.HideAllChoiceHints();
			}
			if (this.EmoteWheel != null)
			{
				this.EmoteWheel.OnEmoteConfirmed -= this.OnEmoteWheelConfirmed;
				this.EmoteWheel.OnEmoteConfirmed += this.OnEmoteWheelConfirmed;
				this.EmoteWheel.Open(this._selectedCategory.Value);
				Debug.Log(string.Format("[Director] Emote wheel opened for {0} - waiting for emote selection (no timer)", this._selectedCategory.Value));
				while (this._isEventActive)
				{
					while (this._isEventActive && this._selectedEmote == null)
					{
						foreach (ExhibitionEventDirector.ChoiceActionConfig choiceActionConfig2 in this.AvailableChoices)
						{
							if (choiceActionConfig2.ButtonUI != null)
							{
								choiceActionConfig2.ButtonUI.UpdateInput();
							}
						}
						yield return null;
					}
					if (!this._isEventActive)
					{
						yield break;
					}
					if (this._selectedEmote.IsBlockedByClothing)
					{
						this._selectedEmote = null;
						this.EmoteWheel.ResetConfirmation();
					}
					else
					{
						if (this._selectedCategory != null)
						{
							Action<ChoiceCategory, DetectionActorType> onPlayerCategoryChosen = this.OnPlayerCategoryChosen;
							if (onPlayerCategoryChosen != null)
							{
								onPlayerCategoryChosen(this._selectedCategory.Value, this.GetCurrentActorType());
							}
						}
						if (this.ResolveEventWithEmote(this._selectedEmote))
						{
							yield break;
						}
						this._selectedEmote = null;
						this.EmoteWheel.ResetConfirmation();
					}
				}
				yield break;
			}
			Debug.LogWarning("[Director] EmoteWheel not assigned! Exiting detection.");
			this.ResolveEvent(null, null);
			yield break;
		}

		// Token: 0x0600938B RID: 37771 RVA: 0x003C9990 File Offset: 0x003C7B90
		private void ActivateNPCCamera()
		{
			EncounterRequest activeEncounterRequest = this._activeEncounterRequest;
			DetectionCameraPresetCollectionSO detectionCameraPresetCollectionSO;
			if (activeEncounterRequest == null)
			{
				detectionCameraPresetCollectionSO = null;
			}
			else
			{
				EncounterProfileSO profile = activeEncounterRequest.profile;
				detectionCameraPresetCollectionSO = ((profile != null) ? profile.npcCameraPresetsOverride : null);
			}
			DetectionCameraPresetCollectionSO detectionCameraPresetCollectionSO2 = detectionCameraPresetCollectionSO ?? this.NpcPhaseCameraPresets;
			if (this.DetectionCameraCtrl != null && detectionCameraPresetCollectionSO2 != null && detectionCameraPresetCollectionSO2.Count > 0 && this._detectingNPC != null)
			{
				this.SaveCameraStateIfNeeded();
				Transform transform = ((this._cachedPlayerInput != null) ? this._cachedPlayerInput.transform : null);
				this.DetectionCameraCtrl.ActivateForNPCPhase(detectionCameraPresetCollectionSO2, this._detectingNPC.transform, transform);
				Debug.Log(string.Format("[Director] NPC camera activated via DetectionCameraController ({0} presets)", detectionCameraPresetCollectionSO2.Count));
				return;
			}
			if (this.NPCReactionCamera != null && this._detectingNPC != null)
			{
				Transform transform2 = this.FindDeepChild(this._detectingNPC.transform, this.NPCLookAtBoneName);
				if (transform2 == null)
				{
					transform2 = this._detectingNPC.transform;
				}
				this.NPCReactionCamera.Follow = this._detectingNPC.transform;
				this.NPCReactionCamera.LookAt = transform2;
				this.ProtectDetectionCamera(this.NPCReactionCamera, this._detectingNPC.transform);
				this.NPCReactionCamera.Priority = 20;
				this.NPCReactionCamera.gameObject.SetActive(true);
				this.SaveCameraStateIfNeeded();
				if (this._mainCameraBrain != null)
				{
					this._mainCameraBrain.enabled = true;
				}
				Debug.Log("[Director] Activated legacy NPC camera on " + this._detectingNPC.name + ", LookAt: " + transform2.name);
				return;
			}
			if (this.CinematicCameras != null && this.CinematicCameras.Count > 0)
			{
				this.ActivateCinematicCamera();
				if (this._activeCinematicCamera != null)
				{
					CinemachineVirtualCamera component = this._activeCinematicCamera.GetComponent<CinemachineVirtualCamera>();
					this.ProtectDetectionCamera(component, (component != null) ? (component.LookAt ?? component.Follow) : null);
				}
			}
		}

		// Token: 0x0600938C RID: 37772 RVA: 0x003C9B90 File Offset: 0x003C7D90
		private void SaveCameraStateIfNeeded()
		{
			if (this._hasSavedCameraState)
			{
				return;
			}
			if (this._mainCameraBrain == null || Camera.main == null)
			{
				return;
			}
			this._savedCameraLocalPosition = Camera.main.transform.localPosition;
			this._savedCameraLocalRotation = Camera.main.transform.localRotation;
			this._hasSavedCameraState = true;
		}

		// Token: 0x0600938D RID: 37773 RVA: 0x003C9BF4 File Offset: 0x003C7DF4
		private void ActivatePlayerChoiceCamera()
		{
			if (this.DetectionCameraCtrl != null && this.DetectionCameraCtrl.IsActive)
			{
				this.DetectionCameraCtrl.Deactivate();
				Debug.Log("[Director] NPC camera controller deactivated for choice phase");
			}
			if (this.NPCReactionCamera != null)
			{
				this.NPCReactionCamera.Priority = 5;
			}
			if (this.CinematicCameras != null && this.CinematicCameras.Count > 0)
			{
				this.ActivateCinematicCamera();
				if (this.CinematicCameras.Count > 0)
				{
					GameObject gameObject = this.CinematicCameras[0];
					if (gameObject != null)
					{
						Debug.Log("[Director] Switched to player camera: " + gameObject.name);
					}
				}
			}
		}

		// Token: 0x0600938E RID: 37774 RVA: 0x003C9CA0 File Offset: 0x003C7EA0
		private void ActivatePlayerFaceCloseUpCamera()
		{
			if (this.PlayerFaceCloseUpCamera == null)
			{
				Debug.LogWarning("[Director] PlayerFaceCloseUpCamera not available");
				return;
			}
			this.PlayerFaceCloseUpCamera.Priority = 25;
			this.PlayerFaceCloseUpCamera.gameObject.SetActive(true);
			if (this.PlayerFaceMediumCamera != null)
			{
				this.PlayerFaceMediumCamera.Priority = 5;
				this.PlayerFaceMediumCamera.gameObject.SetActive(false);
			}
			if (this._mainCameraBrain != null)
			{
				if (!this._hasSavedCameraState && Camera.main != null)
				{
					this._savedCameraLocalPosition = Camera.main.transform.localPosition;
					this._savedCameraLocalRotation = Camera.main.transform.localRotation;
					this._hasSavedCameraState = true;
				}
				this._mainCameraBrain.enabled = true;
			}
			Debug.Log("[Director] Activated player face close-up camera");
		}

		// Token: 0x0600938F RID: 37775 RVA: 0x003C9D79 File Offset: 0x003C7F79
		private IEnumerator PlayPlayerReactionSequence()
		{
			if (this.ShowReactionCanvasDuringSequence && this.PlayerReactionCanvas != null)
			{
				this.PlayerReactionCanvas.SetActive(true);
				Debug.Log("[Director] Player reaction canvas activated");
			}
			if (this.PlayerFaceMediumCamera != null)
			{
				this.PlayerFaceMediumCamera.Priority = 30;
				this.PlayerFaceMediumCamera.gameObject.SetActive(true);
				if (this.PlayerFaceCloseUpCamera != null)
				{
					this.PlayerFaceCloseUpCamera.Priority = 5;
				}
				Debug.Log("[Director] Starting camera transition to medium shot");
			}
			this._reactionHandledExternally = false;
			this._reactionHolds.Clear();
			Action<DetectionActorType> onPlayerReactionStarted = this.OnPlayerReactionStarted;
			if (onPlayerReactionStarted != null)
			{
				onPlayerReactionStarted(this.GetCurrentActorType());
			}
			if (this._reactionHandledExternally)
			{
				yield return this.WaitForReactionHolds();
				Debug.Log("[Director] Player reaction handled by external listener");
			}
			else
			{
				yield return new WaitForSeconds(this.PlayerCameraTransitionDuration);
			}
			if (this.PlayerReactionCanvas != null)
			{
				this.PlayerReactionCanvas.SetActive(false);
				Debug.Log("[Director] Player reaction canvas deactivated");
			}
			Debug.Log("[Director] Player reaction sequence complete");
			yield break;
		}

		// Token: 0x06009390 RID: 37776 RVA: 0x003C9D88 File Offset: 0x003C7F88
		private void DeactivatePlayerReactionCameras()
		{
			if (this.PlayerFaceCloseUpCamera != null)
			{
				this.PlayerFaceCloseUpCamera.Priority = 5;
				this.PlayerFaceCloseUpCamera.gameObject.SetActive(false);
			}
			if (this.PlayerFaceMediumCamera != null)
			{
				this.PlayerFaceMediumCamera.Priority = 5;
				this.PlayerFaceMediumCamera.gameObject.SetActive(false);
			}
		}

		// Token: 0x06009391 RID: 37777 RVA: 0x003C9DEB File Offset: 0x003C7FEB
		public DetectionActorType GetCurrentActorType()
		{
			EncounterRequest activeEncounterRequest = this._activeEncounterRequest;
			if (!(((activeEncounterRequest != null) ? activeEncounterRequest.profile : null) != null))
			{
				return SensoryManager.ResolveActorType(this._detectingNPC);
			}
			return this._activeEncounterRequest.profile.actorType;
		}

		// Token: 0x06009392 RID: 37778 RVA: 0x003C9E23 File Offset: 0x003C8023
		private IEnumerator WaitForReactionHolds()
		{
			float elapsed = 0f;
			while (!this.<WaitForReactionHolds>g__AllDone|192_0())
			{
				if (this.ReactionHoldTimeout > 0f && elapsed >= this.ReactionHoldTimeout)
				{
					Debug.LogWarning(string.Format("[Director] Reaction hold timeout ({0}s) — proceeding.", this.ReactionHoldTimeout));
					break;
				}
				elapsed += Time.unscaledDeltaTime;
				yield return null;
			}
			this._reactionHolds.Clear();
			yield break;
		}

		// Token: 0x06009393 RID: 37779 RVA: 0x003C9E34 File Offset: 0x003C8034
		private void TriggerNPCIdleState()
		{
			if (this._detectingNPC == null)
			{
				return;
			}
			this.CleanupActiveProps();
			Debug.Log("[Director] " + this._detectingNPC.name + ": Props deactivated before entering idle state");
			Animator componentInChildren = this._detectingNPC.GetComponentInChildren<Animator>();
			if (componentInChildren != null)
			{
				int num = 1;
				if (num >= 0 && num < componentInChildren.layerCount)
				{
					componentInChildren.SetLayerWeight(num, 1f);
				}
				componentInChildren.SetInteger(ExhibitionEventDirector.ReactionIdleHash, 1);
				Debug.Log("[Director] " + this._detectingNPC.name + " entered special idle state");
			}
		}

		// Token: 0x06009394 RID: 37780 RVA: 0x003C9ED0 File Offset: 0x003C80D0
		private void ExitNPCIdleState()
		{
			if (this._detectingNPC == null)
			{
				return;
			}
			Animator componentInChildren = this._detectingNPC.GetComponentInChildren<Animator>();
			if (componentInChildren != null)
			{
				componentInChildren.SetInteger(ExhibitionEventDirector.ReactionIdleHash, 0);
			}
		}

		// Token: 0x06009395 RID: 37781 RVA: 0x003C9F10 File Offset: 0x003C8110
		private object StartEncounterDialogue(DialogueLine[] lines, Action onComplete)
		{
			SimpleDialogueManager instance = SimpleDialogueManager.Instance;
			if (instance == null)
			{
				Action onComplete2 = onComplete;
				if (onComplete2 != null)
				{
					onComplete2();
				}
				return null;
			}
			object token = new object();
			this._encounterDialogueManager = instance;
			this._encounterDialogueToken = token;
			instance.StartOwnedDialogue(token, lines, delegate
			{
				if (this._encounterDialogueToken == token)
				{
					this._encounterDialogueManager = null;
					this._encounterDialogueToken = null;
				}
				Action onComplete3 = onComplete;
				if (onComplete3 == null)
				{
					return;
				}
				onComplete3();
			});
			return token;
		}

		// Token: 0x06009396 RID: 37782 RVA: 0x003C9F90 File Offset: 0x003C8190
		private bool IsEncounterDialogueActive(object token)
		{
			return token != null && this._encounterDialogueToken == token && this._encounterDialogueManager != null && this._encounterDialogueManager.IsDialogueOwnedBy(token);
		}

		// Token: 0x06009397 RID: 37783 RVA: 0x003C9FBC File Offset: 0x003C81BC
		private object StartExitDialogue(DialogueLine[] lines, Action onComplete)
		{
			object obj = this.StartEncounterDialogue(lines, onComplete);
			SimpleDialogueManager encounterDialogueManager = this._encounterDialogueManager;
			if (encounterDialogueManager != null)
			{
				encounterDialogueManager.SetOwnedLineAutoAdvance(obj, this.ExitDialogueAutoDismissTime);
			}
			return obj;
		}

		// Token: 0x06009398 RID: 37784 RVA: 0x003C9FEC File Offset: 0x003C81EC
		private void EndEncounterDialogue(object token, bool invokeCompletion = true)
		{
			if (token == null || this._encounterDialogueToken != token)
			{
				return;
			}
			SimpleDialogueManager encounterDialogueManager = this._encounterDialogueManager;
			this._encounterDialogueManager = null;
			this._encounterDialogueToken = null;
			if (encounterDialogueManager != null)
			{
				encounterDialogueManager.TryEndDialogue(token, invokeCompletion);
			}
		}

		// Token: 0x06009399 RID: 37785 RVA: 0x003CA030 File Offset: 0x003C8230
		private ExhibitionEventDirector.ExitPresentation BeginExitPresentation(bool isTerminal)
		{
			ExhibitionEventDirector.ExitPresentation exitPresentation = new ExhibitionEventDirector.ExitPresentation();
			exitPresentation.DialogueManager = SimpleDialogueManager.Instance;
			exitPresentation.Hud = HUDManager.Instance;
			exitPresentation.IsTerminal = isTerminal;
			ExhibitionEventDirector.ExitPresentation exitPresentation2 = exitPresentation;
			this._pendingExitPresentation = exitPresentation;
			return exitPresentation2;
		}

		// Token: 0x0600939A RID: 37786 RVA: 0x003CA068 File Offset: 0x003C8268
		private void RestoreExitPresentation(ExhibitionEventDirector.ExitPresentation presentation)
		{
			if (presentation == null || this._pendingExitPresentation != presentation)
			{
				return;
			}
			this._pendingExitPresentation = null;
			if (presentation.IsTerminal)
			{
				return;
			}
			SimpleDialogueManager instance = SimpleDialogueManager.Instance;
			if (instance != null && instance.IsDialogueActive && (instance != presentation.DialogueManager || !instance.IsDialogueOwnedBy(presentation.DialogueToken)))
			{
				return;
			}
			if (presentation.DialogueManager != null && presentation.DialogueManager.IsDialogueActive && !presentation.DialogueManager.IsDialogueOwnedBy(presentation.DialogueToken))
			{
				return;
			}
			if (presentation.Hud != null)
			{
				presentation.Hud.ShowHUD();
			}
		}

		// Token: 0x0600939B RID: 37787 RVA: 0x003CA10E File Offset: 0x003C830E
		private IEnumerator WatchCoroutine(IEnumerator routine, Action onComplete)
		{
			yield return routine;
			if (onComplete != null)
			{
				onComplete();
			}
			yield break;
		}

		// Token: 0x0600939C RID: 37788 RVA: 0x003CA124 File Offset: 0x003C8324
		private IEnumerator PlayNPCDialogue(ExhibitionEventDirector.DialoguePhase phase)
		{
			Debug.Log(string.Format("[Director] PlayNPCDialogue called for phase: {0}", phase));
			if (SimpleDialogueManager.Instance == null)
			{
				Debug.LogWarning("[Director] SimpleDialogueManager.Instance is null!");
				yield break;
			}
			if (this._detectingNPC == null)
			{
				Debug.LogWarning("[Director] _detectingNPC is null!");
				yield break;
			}
			NPCDialogueHolder component = this._detectingNPC.GetComponent<NPCDialogueHolder>();
			DialogueLine[] array = this.ResolveEncounterDialogue(component, phase);
			EncounterRequest activeEncounterRequest = this._activeEncounterRequest;
			DialoguePackSO dialoguePackSO;
			if (activeEncounterRequest == null)
			{
				dialoguePackSO = null;
			}
			else
			{
				EncounterProfileSO profile = activeEncounterRequest.profile;
				dialoguePackSO = ((profile != null) ? profile.multiActorDialogue : null);
			}
			DialoguePackSO dialoguePackSO2 = dialoguePackSO;
			if (dialoguePackSO2 != null && this._encounterDialogueSelection.UsesEncounterPack)
			{
				PackDialogueLine[] array2;
				switch (phase)
				{
				case ExhibitionEventDirector.DialoguePhase.Reaction:
					array2 = dialoguePackSO2.reactionLines;
					break;
				case ExhibitionEventDirector.DialoguePhase.PostShame:
					array2 = dialoguePackSO2.postShameLines;
					break;
				case ExhibitionEventDirector.DialoguePhase.PostExhibit:
					array2 = dialoguePackSO2.postExhibitLines;
					break;
				case ExhibitionEventDirector.DialoguePhase.Exit:
					array2 = dialoguePackSO2.exitLines;
					break;
				default:
					array2 = null;
					break;
				}
				PackDialogueLine[] array3 = array2;
				if (array3 != null && array3.Length != 0)
				{
					DialogueLine[] array4 = this.BuildDialogueFromPack(array3);
					bool packComplete = false;
					object packToken = this.StartEncounterDialogue(array4, delegate
					{
						packComplete = true;
					});
					while (!packComplete && this.IsEncounterDialogueActive(packToken))
					{
						yield return null;
					}
					yield break;
				}
			}
			EncounterRequest activeEncounterRequest2 = this._activeEncounterRequest;
			NPCDialogueProfileSO npcdialogueProfileSO;
			if (activeEncounterRequest2 == null)
			{
				npcdialogueProfileSO = null;
			}
			else
			{
				EncounterProfileSO profile2 = activeEncounterRequest2.profile;
				npcdialogueProfileSO = ((profile2 != null) ? profile2.dialogueOverride : null);
			}
			NPCDialogueProfileSO npcdialogueProfileSO2 = npcdialogueProfileSO;
			string npcdisplayName = this.GetNPCDisplayName(component, npcdialogueProfileSO2);
			Debug.Log(string.Format("[Director] Dialogue lines for {0}: {1}", phase, (array != null) ? array.Length.ToString() : "null"));
			if (array == null || array.Length == 0)
			{
				Debug.LogWarning(string.Format("[Director] No dialogue lines found for phase {0}!", phase));
				yield break;
			}
			DialogueLine[] array5 = this.OverrideDialogueSpeakerName(array, npcdisplayName, ExhibitionEventDirector.SpeakerScope(component));
			Debug.Log(string.Format("[Director] Starting NPC dialogue with {0} lines, speaker: {1}", array5.Length, npcdisplayName));
			bool dialogueComplete = false;
			object dialogueToken = this.StartEncounterDialogue(array5, delegate
			{
				dialogueComplete = true;
			});
			Debug.Log("[Director] Waiting for NPC dialogue to complete...");
			while (!dialogueComplete && this.IsEncounterDialogueActive(dialogueToken))
			{
				yield return null;
			}
			Debug.Log("[Director] NPC dialogue completed!");
			yield break;
		}

		// Token: 0x0600939D RID: 37789 RVA: 0x003CA13C File Offset: 0x003C833C
		private DialogueLine[] ResolveEncounterDialogue(NPCDialogueHolder holder, ExhibitionEventDirector.DialoguePhase phase)
		{
			if (this._encounterDialogueSelection == null)
			{
				string text = ((holder != null) ? holder.NpcIdentityId : null);
				PersonalityType personalityType = ((holder != null) ? holder.Personality : PersonalityType.Neutral);
				Gender gender = ((holder != null) ? holder.Gender : Gender.Male);
				DialogueActorGroup dialogueActorGroup = DialogueResolver.GroupForActor(this.GetCurrentActorType());
				EncounterRequest activeEncounterRequest = this._activeEncounterRequest;
				NPCDialogueProfileSO npcdialogueProfileSO;
				if (activeEncounterRequest == null)
				{
					npcdialogueProfileSO = null;
				}
				else
				{
					EncounterProfileSO profile = activeEncounterRequest.profile;
					npcdialogueProfileSO = ((profile != null) ? profile.dialogueOverride : null);
				}
				this._encounterDialogueSelection = new DialogueResolver.Session(text, personalityType, gender, dialogueActorGroup, npcdialogueProfileSO);
			}
			return this._encounterDialogueSelection.Resolve(this.ToDialogueCategory(phase));
		}

		// Token: 0x0600939E RID: 37790 RVA: 0x003CA1CD File Offset: 0x003C83CD
		private DialogueCategory ToDialogueCategory(ExhibitionEventDirector.DialoguePhase phase)
		{
			switch (phase)
			{
			case ExhibitionEventDirector.DialoguePhase.Reaction:
				return DialogueCategory.Reaction;
			case ExhibitionEventDirector.DialoguePhase.PostShame:
				return DialogueCategory.Shame;
			case ExhibitionEventDirector.DialoguePhase.PostExhibit:
				return DialogueCategory.Exhibit;
			case ExhibitionEventDirector.DialoguePhase.Exit:
				return DialogueCategory.Exit;
			default:
				return DialogueCategory.Reaction;
			}
		}

		// Token: 0x0600939F RID: 37791 RVA: 0x003CA1F0 File Offset: 0x003C83F0
		private string GetNPCDisplayName(NPCDialogueHolder holder, NPCDialogueProfileSO profile)
		{
			if (holder != null)
			{
				if (!string.IsNullOrEmpty(holder.FullName))
				{
					return holder.FullName;
				}
				if (!string.IsNullOrEmpty(holder.FirstName))
				{
					return holder.FirstName;
				}
			}
			if (profile != null && !string.IsNullOrEmpty(profile.npcDisplayName))
			{
				return profile.npcDisplayName;
			}
			return "NPC";
		}

		// Token: 0x060093A0 RID: 37792 RVA: 0x003CA250 File Offset: 0x003C8450
		private static string SpeakerScope(NPCDialogueHolder holder)
		{
			if (!(holder != null) || (string.IsNullOrEmpty(holder.FullName) && string.IsNullOrEmpty(holder.FirstName)))
			{
				return "dialogue_speaker";
			}
			return null;
		}

		// Token: 0x060093A1 RID: 37793 RVA: 0x003CA27C File Offset: 0x003C847C
		private DialogueLine[] OverrideDialogueSpeakerName(DialogueLine[] originalLines, string speakerName, string speakerScope = null)
		{
			if (originalLines == null)
			{
				return null;
			}
			DialogueLine[] array = new DialogueLine[originalLines.Length];
			for (int i = 0; i < originalLines.Length; i++)
			{
				array[i] = new DialogueLine
				{
					speakerName = speakerName,
					text = originalLines[i].text,
					localizationScope = originalLines[i].localizationScope,
					speakerLocalizationScope = speakerScope
				};
			}
			return array;
		}

		// Token: 0x060093A2 RID: 37794 RVA: 0x003CA2D8 File Offset: 0x003C84D8
		private DialogueLine[] BuildDialogueFromPack(PackDialogueLine[] packLines)
		{
			DialogueLine[] array = new DialogueLine[packLines.Length];
			for (int i = 0; i < packLines.Length; i++)
			{
				string text2;
				string text = this.ResolveActorName(packLines[i].role, out text2);
				array[i] = new DialogueLine
				{
					speakerName = text,
					speakerLocalizationScope = text2,
					text = packLines[i].text,
					localizationScope = "dialogue"
				};
			}
			return array;
		}

		// Token: 0x060093A3 RID: 37795 RVA: 0x003CA348 File Offset: 0x003C8548
		private string ResolveActorName(SpeakerRole role, out string speakerScope)
		{
			speakerScope = null;
			NPCSenses npcsenses;
			switch (role)
			{
			case SpeakerRole.Initiator:
				npcsenses = this._detectingNPC;
				break;
			case SpeakerRole.Actor0:
				npcsenses = this.GetAdditionalActor(0);
				break;
			case SpeakerRole.Actor1:
				npcsenses = this.GetAdditionalActor(1);
				break;
			case SpeakerRole.Actor2:
				npcsenses = this.GetAdditionalActor(2);
				break;
			default:
				npcsenses = null;
				break;
			}
			NPCSenses npcsenses2 = npcsenses;
			if (npcsenses2 == null)
			{
				speakerScope = "dialogue_speaker";
				return "Reporter";
			}
			NPCDialogueHolder component = npcsenses2.GetComponent<NPCDialogueHolder>();
			if (component != null)
			{
				if (!string.IsNullOrEmpty(component.FullName))
				{
					return component.FullName;
				}
				if (!string.IsNullOrEmpty(component.FirstName))
				{
					return component.FirstName;
				}
			}
			return npcsenses2.name;
		}

		// Token: 0x060093A4 RID: 37796 RVA: 0x003CA3F0 File Offset: 0x003C85F0
		private NPCSenses GetAdditionalActor(int index)
		{
			EncounterRequest activeEncounterRequest = this._activeEncounterRequest;
			List<NPCSenses> list = ((activeEncounterRequest != null) ? activeEncounterRequest.additionalActors : null);
			if (list == null || index >= list.Count)
			{
				return null;
			}
			return list[index];
		}

		// Token: 0x060093A5 RID: 37797 RVA: 0x003CA425 File Offset: 0x003C8625
		private IEnumerator PlayExitDialogueWithAutoDismiss()
		{
			EncounterRequest activeEncounterRequest = this._activeEncounterRequest;
			bool flag;
			if (activeEncounterRequest == null)
			{
				flag = false;
			}
			else
			{
				EncounterProfileSO profile = activeEncounterRequest.profile;
				bool? flag2 = ((profile != null) ? new bool?(profile.invokeOnSceneEndedDuringFade) : null);
				bool flag3 = true;
				flag = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
			}
			ExhibitionEventDirector.ExitPresentation presentation = this.BeginExitPresentation(flag);
			if (this.UseDialogue && SimpleDialogueManager.Instance != null && this._detectingNPC != null)
			{
				NPCDialogueHolder component = this._detectingNPC.GetComponent<NPCDialogueHolder>();
				EncounterRequest activeEncounterRequest2 = this._activeEncounterRequest;
				NPCDialogueProfileSO npcdialogueProfileSO;
				if (activeEncounterRequest2 == null)
				{
					npcdialogueProfileSO = null;
				}
				else
				{
					EncounterProfileSO profile2 = activeEncounterRequest2.profile;
					npcdialogueProfileSO = ((profile2 != null) ? profile2.dialogueOverride : null);
				}
				NPCDialogueProfileSO npcdialogueProfileSO2 = npcdialogueProfileSO;
				DialogueLine[] array = this.ResolveEncounterDialogue(component, ExhibitionEventDirector.DialoguePhase.Exit);
				EncounterRequest activeEncounterRequest3 = this._activeEncounterRequest;
				DialoguePackSO dialoguePackSO;
				if (activeEncounterRequest3 == null)
				{
					dialoguePackSO = null;
				}
				else
				{
					EncounterProfileSO profile3 = activeEncounterRequest3.profile;
					dialoguePackSO = ((profile3 != null) ? profile3.multiActorDialogue : null);
				}
				DialoguePackSO dialoguePackSO2 = dialoguePackSO;
				bool flag4 = dialoguePackSO2 != null && this._encounterDialogueSelection.UsesEncounterPack && dialoguePackSO2.exitLines != null && dialoguePackSO2.exitLines.Length != 0;
				if (flag4)
				{
					array = this.BuildDialogueFromPack(dialoguePackSO2.exitLines);
				}
				if (array != null && array.Length != 0)
				{
					ExhibitionEventDirector.<>c__DisplayClass213_0 CS$<>8__locals1 = new ExhibitionEventDirector.<>c__DisplayClass213_0();
					string npcdisplayName = this.GetNPCDisplayName(component, npcdialogueProfileSO2);
					DialogueLine[] array2 = (flag4 ? array : this.OverrideDialogueSpeakerName(array, npcdisplayName, ExhibitionEventDirector.SpeakerScope(component)));
					CS$<>8__locals1.dialogueComplete = false;
					object dialogueToken = this.StartExitDialogue(array2, delegate
					{
						CS$<>8__locals1.dialogueComplete = true;
					});
					presentation.DialogueManager = this._encounterDialogueManager;
					presentation.DialogueToken = dialogueToken;
					while (!CS$<>8__locals1.dialogueComplete && this.IsEncounterDialogueActive(dialogueToken))
					{
						yield return null;
					}
					CS$<>8__locals1 = null;
					dialogueToken = null;
				}
			}
			this.RestoreExitPresentation(presentation);
			yield break;
		}

		// Token: 0x060093A6 RID: 37798 RVA: 0x003CA434 File Offset: 0x003C8634
		private void InitializeChoiceButtons()
		{
			Canvas canvas = ((this.ChoiceUIRoot != null) ? this.ChoiceUIRoot.GetComponentInParent<Canvas>() : null);
			if (canvas == null && this.ChoiceCanvas != null)
			{
				canvas = this.ChoiceCanvas.GetComponent<Canvas>();
			}
			if (canvas != null && canvas.renderMode == RenderMode.WorldSpace)
			{
				this._choiceCameraAnchor = canvas.GetComponent<DetectionChoiceCameraAnchor>();
				if (this._choiceCameraAnchor == null)
				{
					this._choiceCameraAnchor = canvas.gameObject.AddComponent<DetectionChoiceCameraAnchor>();
				}
				this._choiceCameraAnchor.Bind(Camera.main);
				this._choiceCameraAnchor.enabled = true;
			}
			foreach (ExhibitionEventDirector.ChoiceActionConfig choiceActionConfig in this.AvailableChoices)
			{
				if (choiceActionConfig.ButtonUI == null)
				{
					Debug.LogWarning("[Director] Choice '" + choiceActionConfig.Label + "' has no ButtonUI assigned!");
				}
				else if (!this.IsChoiceCategoryAllowedByRank(choiceActionConfig.Category))
				{
					choiceActionConfig.ButtonUI.gameObject.SetActive(false);
				}
				else
				{
					choiceActionConfig.ButtonUI.gameObject.SetActive(true);
					ChoiceCategory capturedCategory = choiceActionConfig.Category;
					ControllerContextBindings.Set(choiceActionConfig.Input, (choiceActionConfig.Category == ChoiceCategory.Exhibit) ? "Y" : "X");
					string text = ((!string.IsNullOrEmpty(choiceActionConfig.DisplayLabel)) ? choiceActionConfig.DisplayLabel : null);
					choiceActionConfig.ButtonUI.Initialize(choiceActionConfig.Input, choiceActionConfig.HoldDuration, text, delegate
					{
						if (!this.IsChoiceCategoryAllowedByRank(capturedCategory))
						{
							return;
						}
						Debug.Log(string.Format("[Director] Category selected: {0}", capturedCategory));
						this._selectedCategory = new ChoiceCategory?(capturedCategory);
					});
				}
			}
		}

		// Token: 0x060093A7 RID: 37799 RVA: 0x003CA5F4 File Offset: 0x003C87F4
		private bool IsChoiceCategoryAllowedByRank(ChoiceCategory category)
		{
			if (RunModeService.IsLongWayHome)
			{
				return category == ChoiceCategory.Shame;
			}
			RankRuntimeController rankRuntimeController = RankRuntimeController.Current;
			return rankRuntimeController == null || category != ChoiceCategory.Exhibit || rankRuntimeController.ChoiceMode == EncounterChoiceMode.ShameOrExhibit;
		}

		// Token: 0x060093A8 RID: 37800 RVA: 0x003CA630 File Offset: 0x003C8830
		private void HideAllChoiceHints()
		{
			foreach (ExhibitionEventDirector.ChoiceActionConfig choiceActionConfig in this.AvailableChoices)
			{
				if (choiceActionConfig.ButtonUI != null)
				{
					choiceActionConfig.ButtonUI.HideHint();
				}
			}
		}

		// Token: 0x060093A9 RID: 37801 RVA: 0x003CA698 File Offset: 0x003C8898
		private void ApplyChoiceButtonVisualStates(ChoiceCategory selectedCategory)
		{
			foreach (ExhibitionEventDirector.ChoiceActionConfig choiceActionConfig in this.AvailableChoices)
			{
				if (!(choiceActionConfig.ButtonUI == null))
				{
					if (choiceActionConfig.Category == selectedCategory)
					{
						choiceActionConfig.ButtonUI.SetSelectedState();
					}
					else
					{
						choiceActionConfig.ButtonUI.SetDimmedState();
					}
				}
			}
		}

		// Token: 0x060093AA RID: 37802 RVA: 0x003CA714 File Offset: 0x003C8914
		private void OnEmoteWheelConfirmed(EmoteDefinitionSO emote)
		{
			if (emote == null)
			{
				return;
			}
			if (!emote.IsBlockedByClothing && this.IsEmoteAllowedInCurrentMode(emote))
			{
				this._selectedEmote = emote;
				Debug.Log("[Director] Emote confirmed from wheel: " + emote.DisplayName);
				return;
			}
			EmoteWheelUI emoteWheel = this.EmoteWheel;
			if (emoteWheel == null)
			{
				return;
			}
			emoteWheel.ResetConfirmation();
		}

		// Token: 0x060093AB RID: 37803 RVA: 0x003CA769 File Offset: 0x003C8969
		private IEnumerator CaptureNewspaperPhotoDelayed()
		{
			if (this.newspaperCaptureDelay > 0f)
			{
				yield return new WaitForSeconds(this.newspaperCaptureDelay);
			}
			yield return new WaitForEndOfFrame();
			if (NewspaperPhotoService.Instance != null)
			{
				NewspaperPhotoService.Instance.CapturePhotoFromMainCamera();
			}
			yield break;
		}

		// Token: 0x060093AC RID: 37804 RVA: 0x003CA778 File Offset: 0x003C8978
		private bool ResolveEventWithEmote(EmoteDefinitionSO emote)
		{
			if (emote != null && (emote.IsBlockedByClothing || !this.IsEmoteAllowedInCurrentMode(emote)))
			{
				return false;
			}
			this._isInEmoteSelectionPhase = false;
			this._isInChoicePhase = false;
			if (emote == null)
			{
				this.ResolveEvent(null, null);
				return true;
			}
			if (emote.SpecialAction == EmoteSpecialAction.StartGropingMode)
			{
				this.ResolveEvent(null, emote);
				return true;
			}
			vTriggerGenericAction vTriggerGenericAction = emote.ResolveTriggerPrefab();
			if (vTriggerGenericAction == null)
			{
				Debug.LogWarning("[Director] Emote '" + emote.name + "' has no valid trigger prefab. Exiting sequence.");
				this.ResolveEvent(null, null);
				return true;
			}
			if (this.PlayerEmotePropHandler != null)
			{
				this.PlayerEmotePropHandler.SetupProps(emote);
				Debug.Log("[Director] Props setup for emote '" + emote.DisplayName + "' via EmotePropHandler");
			}
			this.PlayEmoteAudio(emote);
			PlayerActionEntry playerActionEntry = new PlayerActionEntry
			{
				ActionId = emote.EmoteId,
				DisplayName = emote.DisplayName,
				Duration = emote.Duration,
				TriggerPrefab = vTriggerGenericAction
			};
			this.ResolveEvent(playerActionEntry, emote);
			return true;
		}

		// Token: 0x060093AD RID: 37805 RVA: 0x003CA87E File Offset: 0x003C8A7E
		public bool TrySkipEncounter()
		{
			if (!this.CanSkipEncounter || this.IsSkipInputBlocked())
			{
				return false;
			}
			this.SkipSequence();
			return true;
		}

		// Token: 0x060093AE RID: 37806 RVA: 0x003CA89C File Offset: 0x003C8A9C
		private void SkipSequence()
		{
			if (!this.CanSkipEncounter || this.IsSkipInputBlocked())
			{
				return;
			}
			this._encounterResolutionStarted = true;
			this._encounterSkipped = true;
			this.StopEncounterPresentation();
			this.CancelPreReactionPresentation();
			this._reactionHolds.Clear();
			this._reactionHandledExternally = false;
			UnityEvent onEventSkipped = this.OnEventSkipped;
			if (onEventSkipped != null)
			{
				onEventSkipped.Invoke();
			}
			this.ResolveEvent(null, null);
		}

		// Token: 0x060093AF RID: 37807 RVA: 0x003CA900 File Offset: 0x003C8B00
		private Coroutine StartEncounterPresentation(IEnumerator routine)
		{
			Coroutine coroutine = base.StartCoroutine(routine);
			this._encounterPresentationCoroutines.Add(coroutine);
			return coroutine;
		}

		// Token: 0x060093B0 RID: 37808 RVA: 0x003CA924 File Offset: 0x003C8B24
		private void StopEncounterPresentation()
		{
			if (this._sequenceCoroutine != null)
			{
				base.StopCoroutine(this._sequenceCoroutine);
			}
			this._sequenceCoroutine = null;
			foreach (Coroutine coroutine in this._encounterPresentationCoroutines)
			{
				if (coroutine != null)
				{
					base.StopCoroutine(coroutine);
				}
			}
			this._encounterPresentationCoroutines.Clear();
			this._concurrentDialogueCoroutine = null;
			this.EndEncounterDialogue(this._encounterDialogueToken, false);
			if (this.PlayerReactionCanvas)
			{
				this.PlayerReactionCanvas.SetActive(false);
			}
		}

		// Token: 0x060093B1 RID: 37809 RVA: 0x003CA9D0 File Offset: 0x003C8BD0
		private void CancelPreReactionPresentation()
		{
			if (this._preReactionStarted)
			{
				EncounterRequest activeEncounterRequest = this._activeEncounterRequest;
				IEncounterPreReactionCancellation encounterPreReactionCancellation = ((activeEncounterRequest != null) ? activeEncounterRequest.preReactionPhase : null) as IEncounterPreReactionCancellation;
				if (encounterPreReactionCancellation != null)
				{
					encounterPreReactionCancellation.CancelPreReaction();
				}
			}
			this._preReactionStarted = false;
			this.RestorePreReactionBlend();
		}

		// Token: 0x060093B2 RID: 37810 RVA: 0x003CAA13 File Offset: 0x003C8C13
		private void RestorePreReactionBlend()
		{
			if (this._preReactionHasSavedBlend && this._preReactionBrain != null)
			{
				this._preReactionBrain.m_DefaultBlend = this._preReactionSavedBlend;
			}
			this._preReactionHasSavedBlend = false;
			this._preReactionBrain = null;
		}

		// Token: 0x060093B3 RID: 37811 RVA: 0x003CAA4C File Offset: 0x003C8C4C
		private void ResolveEvent(PlayerActionEntry playerAction, EmoteDefinitionSO emote)
		{
			this._encounterResolutionStarted = true;
			this._isInChoicePhase = false;
			this.SetResolutionActionState(false, null);
			this.StopEncounterPresentation();
			if (this.EmoteWheel != null)
			{
				this.EmoteWheel.OnEmoteConfirmed -= this.OnEmoteWheelConfirmed;
				this.EmoteWheel.Close();
			}
			if (this.ChoiceCanvas)
			{
				this.ChoiceCanvas.SetActive(false);
			}
			if (this.ChoiceUIRoot)
			{
				this.ChoiceUIRoot.SetActive(false);
			}
			if (this.SkipHintUI)
			{
				this.SkipHintUI.SetActive(false);
			}
			EncounterInputHints component = base.GetComponent<EncounterInputHints>();
			if (component != null)
			{
				component.SetSkipHintVisible(false);
			}
			if (this.SkipTimerFill)
			{
				this.SkipTimerFill.fillAmount = 0f;
			}
			if (this.SkipCountdownText)
			{
				this.SkipCountdownText.text = "";
			}
			foreach (ExhibitionEventDirector.ChoiceActionConfig choiceActionConfig in this.AvailableChoices)
			{
				if (choiceActionConfig.ButtonUI != null)
				{
					choiceActionConfig.ButtonUI.ResetState();
				}
			}
			this._selectedEmote = null;
			this._selectedCategory = null;
			this._isInEmoteSelectionPhase = false;
			if (CoverPoseController.Instance != null)
			{
				CoverPoseController.Instance.DeactivateCoverPose();
				Debug.Log("[Director] Cover pose deactivated - player made choice");
			}
			if (DetectionAudioManager.Instance != null)
			{
				DetectionAudioManager.Instance.OnPlayerChoice();
			}
			if (playerAction != null)
			{
				base.StartCoroutine(this.ResolutionSequenceRoutine(playerAction, emote));
				return;
			}
			base.StartCoroutine(this.ExitSequenceRoutine(emote, this._encounterSkipped));
		}

		// Token: 0x060093B4 RID: 37812 RVA: 0x003CAC10 File Offset: 0x003C8E10
		private IEnumerator ResolutionSequenceRoutine(PlayerActionEntry playerAction, EmoteDefinitionSO emote = null)
		{
			Time.timeScale = 1f;
			Time.fixedDeltaTime = 0.02f;
			UnityEvent onTimeResumed = this.OnTimeResumed;
			if (onTimeResumed != null)
			{
				onTimeResumed.Invoke();
			}
			this.UpdateEmotionLevels(playerAction, emote);
			this.ReassertPlayerInputLock();
			vThirdPersonInput cachedPlayerInput = this._cachedPlayerInput;
			Animator animator;
			if (cachedPlayerInput == null)
			{
				animator = null;
			}
			else
			{
				vThirdPersonController cc = cachedPlayerInput.cc;
				animator = ((cc != null) ? cc.animator : null);
			}
			Animator playerAnimator = animator;
			if (playerAnimator != null)
			{
				this.SavePlayerAnimatorLayerWeights(playerAnimator);
			}
			float num = playerAction.Duration;
			PlayerActionAnimator playerAnimator2 = this.GetPlayerAnimator();
			this.SetResolutionActionState(true, emote);
			if (playerAnimator2 != null)
			{
				if (emote != null && emote.MovingUseRootMotion)
				{
					playerAnimator2.EnableRootMotionForAction();
				}
				num = playerAnimator2.TriggerAction(playerAction);
				if (emote != null)
				{
					Action<EmoteDefinitionSO> onEncounterEmoteStarted = this.OnEncounterEmoteStarted;
					if (onEncounterEmoteStarted != null)
					{
						onEncounterEmoteStarted(emote);
					}
				}
				Debug.Log(string.Format("[Director] Playing action '{0}' for {1} seconds", playerAction.ActionId, num));
			}
			if (emote != null && this.PlayerEmotePropHandler != null)
			{
				this.PlayerEmotePropHandler.StartAutoTimers();
				Debug.Log("[Director] Started prop auto timers for emote '" + emote.DisplayName + "'");
			}
			UnityEvent<PlayerActionEntry> onPlayerActionTriggered = this.OnPlayerActionTriggered;
			if (onPlayerActionTriggered != null)
			{
				onPlayerActionTriggered.Invoke(playerAction);
			}
			EncounterRequest activeEncounterRequest = this._activeEncounterRequest;
			bool flag;
			if (activeEncounterRequest == null)
			{
				flag = false;
			}
			else
			{
				EncounterProfileSO profile = activeEncounterRequest.profile;
				bool? flag2 = ((profile != null) ? new bool?(profile.captureNewspaperPhoto) : null);
				bool flag3 = true;
				flag = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
			}
			if (flag && NewspaperPhotoService.Instance != null)
			{
				base.StartCoroutine(this.CaptureNewspaperPhotoDelayed());
			}
			this.ActivateEmoteCameras(emote);
			yield return base.StartCoroutine(this.WaitForEmoteAndSkip(emote, num, playerAnimator));
			Debug.Log("[Director] Player action complete - NPC exiting idle state");
			this.ExitNPCIdleState();
			Debug.Log("[Director] Checking post-action dialogue...");
			if (this.UseDialogue && SimpleDialogueManager.Instance != null)
			{
				ExhibitionEventDirector.DialoguePhase dialoguePhase = this.ResolveDialoguePhase(playerAction, emote);
				yield return base.StartCoroutine(this.PlayNPCDialogue(dialoguePhase));
			}
			Debug.Log("[Director] Starting fade out...");
			if (ScreenFadeController.Instance != null)
			{
				ExhibitionEventDirector.<>c__DisplayClass228_0 CS$<>8__locals1 = new ExhibitionEventDirector.<>c__DisplayClass228_0();
				CS$<>8__locals1.fadeComplete = false;
				ScreenFadeController.Instance.FadeOut(this.FadeDuration, delegate
				{
					CS$<>8__locals1.fadeComplete = true;
				});
				while (!CS$<>8__locals1.fadeComplete)
				{
					yield return null;
				}
				Debug.Log("[Director] Fade out complete");
				CS$<>8__locals1 = null;
			}
			if (playerAnimator != null)
			{
				playerAnimator.speed = 1f;
			}
			PlayerActionAnimator playerAnimator3 = this.GetPlayerAnimator();
			if (playerAnimator3 != null)
			{
				playerAnimator3.FinishCurrentAction();
				playerAnimator3.RestoreRootMotionForAction();
				playerAnimator3.RestoreOverrideLayer();
				Debug.Log("[Director] Emote action finished during fade-to-black");
			}
			this.SetResolutionActionState(false, null);
			if (this.EmoteAudioSource != null && this.EmoteAudioSource.isPlaying)
			{
				this.EmoteAudioSource.Stop();
				this.EmoteAudioSource.clip = null;
				Debug.Log("[Director] Emote audio stopped");
			}
			int num2;
			for (int i = 0; i < 3; i = num2 + 1)
			{
				yield return null;
				num2 = i;
			}
			if (playerAnimator != null)
			{
				this.RestorePlayerAnimatorLayerWeights(playerAnimator);
			}
			this.DeactivateEmoteCameras();
			yield return base.StartCoroutine(this.DeactivateCinematicCameraRoutine());
			if (this.NPCReactionCamera != null)
			{
				this.NPCReactionCamera.gameObject.SetActive(false);
			}
			Debug.Log("[Director] Restarting NPC behavior...");
			this.CleanupActiveProps();
			this.RestartNPCBehavior();
			yield return base.StartCoroutine(this.StartSuccessorUnderFadeRoutine());
			Debug.Log("[Director] Starting fade in...");
			if (ScreenFadeController.Instance != null)
			{
				ExhibitionEventDirector.<>c__DisplayClass228_1 CS$<>8__locals2 = new ExhibitionEventDirector.<>c__DisplayClass228_1();
				CS$<>8__locals2.fadeInComplete = false;
				ScreenFadeController.Instance.FadeIn(this.FadeDuration, delegate
				{
					CS$<>8__locals2.fadeInComplete = true;
				});
				while (!CS$<>8__locals2.fadeInComplete)
				{
					yield return null;
				}
				Debug.Log("[Director] Fade in complete");
				CS$<>8__locals2 = null;
			}
			if (RunModeService.IsLongWayHome)
			{
				yield return base.StartCoroutine(this.PlayExitDialogueWithAutoDismiss());
			}
			else
			{
				base.StartCoroutine(this.PlayExitDialogueWithAutoDismiss());
			}
			Debug.Log("[Director] Starting final cleanup...");
			EncounterRequest activeEncounterRequest2 = this._activeEncounterRequest;
			EncounterProfileSO resolvedProfile = ((activeEncounterRequest2 != null) ? activeEncounterRequest2.profile : null);
			yield return base.StartCoroutine(this.FinalCleanupRoutine());
			this.NotifyProfileEncounterResolved(resolvedProfile);
			yield break;
		}

		// Token: 0x060093B5 RID: 37813 RVA: 0x003CAC2D File Offset: 0x003C8E2D
		private IEnumerator WaitForEmoteAndSkip(EmoteDefinitionSO emote, float actionDuration, Animator playerAnimator)
		{
			DetectionFlyingCamera.NormalizePhotoInput(this.FlyingCameraEnterInput);
			if (this.FlyingCamera == null)
			{
				this.FlyingCamera = Object.FindObjectOfType<DetectionFlyingCamera>();
			}
			bool canUseFlying = this.EnableFlyingCamera && this.FlyingCamera != null;
			EncounterRequest activeEncounterRequest = this._activeEncounterRequest;
			IEncounterMode encounterMode = ((activeEncounterRequest != null) ? activeEncounterRequest.postEmoteMode : null);
			bool canUseEncounterMode = encounterMode != null;
			Debug.Log(string.Format("[Director] WaitForEmoteAndSkip | Emote='{0}' Duration={1:F1}s canUseFlying={2} canUseEncounterMode={3}", new object[]
			{
				((emote != null) ? emote.DisplayName : null) ?? "Legacy",
				actionDuration,
				canUseFlying,
				canUseEncounterMode
			}));
			if (!this.EnableFlyingCamera && this.FlyingCamera != null)
			{
				Debug.LogWarning("[Director][FlyCam] Enter input is currently DISABLED because EnableFlyingCamera is false in the Director inspector");
			}
			if (SimpleDialogueManager.Instance != null)
			{
				float hintDialogueWait = 0f;
				while (SimpleDialogueManager.Instance.IsDialogueActive && hintDialogueWait < 30f && this._isEventActive)
				{
					hintDialogueWait += Time.deltaTime;
					yield return null;
				}
			}
			this._waitingForEmotePhoto = canUseFlying;
			if (this.EmoteHintUI != null)
			{
				this.EmoteHintUI.SetActive(true);
			}
			IEncounterMode encounterMode2 = encounterMode;
			if (((encounterMode2 != null) ? encounterMode2.HintUI : null) != null)
			{
				encounterMode.HintUI.SetActive(true);
			}
			bool hasOneShotFrozen = false;
			float elapsedTime = 0f;
			bool waitForFlyEnterRelease = false;
			float nextFlyEnterAllowedTime = 0f;
			bool waitForEncounterModeRelease = false;
			float nextEncounterModeAllowedTime = 0f;
			for (;;)
			{
				if (FlyCamera.IsActive || TextInputFocus.BlocksGameplayInput || GraphicsSettingsRuntimeUI.AnyOpen)
				{
					yield return null;
				}
				else
				{
					if (emote == null)
					{
						elapsedTime += Time.deltaTime;
						if (elapsedTime >= actionDuration)
						{
							break;
						}
					}
					else if (!emote.IsLoopEmote && !hasOneShotFrozen)
					{
						elapsedTime += Time.deltaTime;
						if (elapsedTime >= actionDuration)
						{
							if (playerAnimator != null)
							{
								playerAnimator.speed = 0f;
								Debug.Log("[Director] One-shot emote '" + emote.DisplayName + "' finished — animator frozen");
							}
							hasOneShotFrozen = true;
						}
					}
					if (!canUseFlying && this.FlyingCameraEnterInput.GetButtonDown())
					{
						Debug.LogWarning("[Director][FlyCam] Enter input detected but FlyCam is disabled. Set EnableFlyingCamera=true on ExhibitionEventDirector");
					}
					if (waitForFlyEnterRelease && !this.FlyingCameraEnterInput.GetButton())
					{
						waitForFlyEnterRelease = false;
					}
					if (canUseFlying && !waitForFlyEnterRelease && Time.unscaledTime >= nextFlyEnterAllowedTime && this.FlyingCameraEnterInput.GetButtonDown())
					{
						Debug.Log("[Director][FlyCam] Enter input detected -> starting FlyMode phase");
						if (this.EmoteHintUI != null)
						{
							this.EmoteHintUI.SetActive(false);
						}
						if (this.DetectionCameraCtrl != null)
						{
							this.DetectionCameraCtrl.OnFlyModeEnter();
						}
						this.RestoreEncounterNPCLayers();
						yield return base.StartCoroutine(this.FlyingCameraPhase());
						this.HideEncounterNPCsOnLayer();
						if (this.EmoteHintUI != null)
						{
							this.EmoteHintUI.SetActive(true);
						}
						if (this.DetectionCameraCtrl != null)
						{
							this.DetectionCameraCtrl.OnFlyModeExit();
						}
						waitForFlyEnterRelease = true;
						nextFlyEnterAllowedTime = Time.unscaledTime + 0.2f;
					}
					if (waitForEncounterModeRelease && !encounterMode.EnterInput.GetButton())
					{
						waitForEncounterModeRelease = false;
					}
					if (canUseEncounterMode && !waitForEncounterModeRelease && Time.unscaledTime >= nextEncounterModeAllowedTime && encounterMode.EnterInput.GetButtonDown())
					{
						if (this.EmoteHintUI != null)
						{
							this.EmoteHintUI.SetActive(false);
						}
						if (encounterMode.HintUI != null)
						{
							encounterMode.HintUI.SetActive(false);
						}
						this._waitingForEmotePhoto = false;
						yield return base.StartCoroutine(encounterMode.Execute(this));
						this._waitingForEmotePhoto = canUseFlying;
						if (this.EmoteHintUI != null)
						{
							this.EmoteHintUI.SetActive(true);
						}
						if (encounterMode.HintUI != null)
						{
							encounterMode.HintUI.SetActive(true);
						}
						waitForEncounterModeRelease = true;
						nextEncounterModeAllowedTime = Time.unscaledTime + 0.2f;
					}
					bool flag = this.FlyingCamera != null && this.FlyingCamera.IsActive;
					IEncounterMode encounterMode3 = encounterMode;
					bool flag2 = encounterMode3 != null && encounterMode3.IsActive;
					if (!flag && !flag2 && this.SkipInput.GetButtonDown())
					{
						break;
					}
					yield return null;
				}
			}
			Debug.Log("[Director] Emote '" + (((emote != null) ? emote.DisplayName : null) ?? "Legacy") + "' finished or skipped by player");
			this._waitingForEmotePhoto = false;
			if (this.EmoteHintUI != null)
			{
				this.EmoteHintUI.SetActive(false);
			}
			IEncounterMode encounterMode4 = encounterMode;
			if (((encounterMode4 != null) ? encounterMode4.HintUI : null) != null)
			{
				encounterMode.HintUI.SetActive(false);
			}
			yield break;
		}

		// Token: 0x060093B6 RID: 37814 RVA: 0x003CAC51 File Offset: 0x003C8E51
		private IEnumerator FlyingCameraPhase()
		{
			if (this.FlyingCamera == null)
			{
				Debug.LogError("[Director][FlyCam] FlyingCameraPhase called but FlyingCamera reference is NULL");
				yield break;
			}
			Vector3 vector = ((this._cachedPlayerInput != null) ? this._cachedPlayerInput.transform.position : ((Camera.main != null) ? Camera.main.transform.position : Vector3.zero));
			this.ReassertPlayerInputLock();
			Debug.Log(string.Format("[Director][FlyCam] Activating FlyMode | origin={0}", vector));
			this.FlyingCamera.Activate(vector, null, null);
			Debug.Log(string.Format("[Director][FlyCam] Flying camera phase started | IsActive={0}", this.FlyingCamera.IsActive));
			bool escapeRequested = false;
			while (this.FlyingCamera.IsActive && this.FlyingCamera.IsExitInputPressed())
			{
				if (this.FlyingCamera.WasEscapePressed())
				{
					escapeRequested = true;
					break;
				}
				yield return null;
			}
			float earliestExitTime = Time.unscaledTime + 0.1f;
			Debug.Log("[Director][FlyCam] Exit input armed after release + 100ms grace");
			while (this.FlyingCamera.IsActive && !escapeRequested && !this.FlyingCamera.WasEscapePressed() && (Time.unscaledTime < earliestExitTime || !this.FlyingCamera.WasExitPressed()))
			{
				yield return null;
			}
			this.FlyingCamera.Deactivate();
			this.ReassertPlayerInputLock();
			Debug.Log("[Director][FlyCam] Flying camera phase ended");
			yield break;
		}

		// Token: 0x060093B7 RID: 37815 RVA: 0x003CAC60 File Offset: 0x003C8E60
		private IEnumerator ExitSequenceRoutine(EmoteDefinitionSO resolutionEmote = null, bool resolveSkippedEncounter = false)
		{
			EncounterRequest activeEncounterRequest = this._activeEncounterRequest;
			EncounterProfileSO resolvedProfile = ((activeEncounterRequest != null) ? activeEncounterRequest.profile : null);
			if (resolutionEmote != null && resolutionEmote.SpecialAction == EmoteSpecialAction.StartGropingMode)
			{
				yield return base.StartCoroutine(this.GropingInterludeRoutine(resolutionEmote));
				yield break;
			}
			if (Time.timeScale != 1f)
			{
				Time.timeScale = 1f;
				Time.fixedDeltaTime = 0.02f;
				UnityEvent onTimeResumed = this.OnTimeResumed;
				if (onTimeResumed != null)
				{
					onTimeResumed.Invoke();
				}
			}
			this.ExitNPCIdleState();
			if (ScreenFadeController.Instance != null)
			{
				ExhibitionEventDirector.<>c__DisplayClass231_0 CS$<>8__locals1 = new ExhibitionEventDirector.<>c__DisplayClass231_0();
				CS$<>8__locals1.fadeComplete = false;
				ScreenFadeController.Instance.FadeOut(this.FadeDuration, delegate
				{
					CS$<>8__locals1.fadeComplete = true;
				});
				while (!CS$<>8__locals1.fadeComplete)
				{
					yield return null;
				}
				CS$<>8__locals1 = null;
			}
			this.DeactivatePlayerReactionCameras();
			this.DeactivateEmoteCameras();
			yield return this.DeactivateCinematicCameraRoutine();
			if (this.NPCReactionCamera != null)
			{
				this.NPCReactionCamera.gameObject.SetActive(false);
			}
			this.CleanupActiveProps();
			this.RestartNPCBehavior();
			yield return this.StartSuccessorUnderFadeRoutine();
			if (ScreenFadeController.Instance != null)
			{
				ExhibitionEventDirector.<>c__DisplayClass231_1 CS$<>8__locals2 = new ExhibitionEventDirector.<>c__DisplayClass231_1();
				CS$<>8__locals2.fadeInComplete = false;
				ScreenFadeController.Instance.FadeIn(this.FadeDuration, delegate
				{
					CS$<>8__locals2.fadeInComplete = true;
				});
				while (!CS$<>8__locals2.fadeInComplete)
				{
					yield return null;
				}
				CS$<>8__locals2 = null;
			}
			this.UnlockPlayerInput();
			if (resolveSkippedEncounter)
			{
				this.RestoreExitPresentation(this.BeginExitPresentation(false));
			}
			else if (RunModeService.IsLongWayHome)
			{
				yield return this.PlayExitDialogueWithAutoDismiss();
			}
			else
			{
				base.StartCoroutine(this.PlayExitDialogueWithAutoDismiss());
			}
			yield return this.FinalCleanupRoutine();
			if (resolveSkippedEncounter || RunModeService.IsLongWayHome)
			{
				this.NotifyProfileEncounterResolved(resolvedProfile);
			}
			yield break;
		}

		// Token: 0x060093B8 RID: 37816 RVA: 0x003CAC7D File Offset: 0x003C8E7D
		private IEnumerator GropingInterludeRoutine(EmoteDefinitionSO resolutionEmote)
		{
			if (Time.timeScale != 1f)
			{
				Time.timeScale = 1f;
				Time.fixedDeltaTime = 0.02f;
				UnityEvent onTimeResumed = this.OnTimeResumed;
				if (onTimeResumed != null)
				{
					onTimeResumed.Invoke();
				}
			}
			this.UpdateEmotionLevels(new PlayerActionEntry
			{
				ActionId = resolutionEmote.EmoteId,
				DisplayName = resolutionEmote.DisplayName,
				Duration = 0f,
				TriggerPrefab = null
			}, resolutionEmote);
			this.DeactivateEmoteCameras();
			yield return base.StartCoroutine(this.DeactivateCinematicCameraRoutine());
			if (this.NPCReactionCamera != null)
			{
				this.NPCReactionCamera.gameObject.SetActive(false);
			}
			this.ReassertPlayerInputLock();
			if (HUDManager.Instance != null)
			{
				HUDManager.Instance.ShowHUD();
			}
			GropingModeController groping = Object.FindObjectOfType<GropingModeController>(true);
			if (groping != null)
			{
				this._gropingInterludeOwner = groping.GetComponent<BoobTouchModeController>();
				try
				{
					float enterDeadline = Time.unscaledTime + 8.5f;
					while (groping != null && !groping.IsActive && this.IsEventActive && Time.unscaledTime < enterDeadline && !this.TryEnterGropingInterlude(groping))
					{
						yield return null;
					}
					if (groping != null && groping.IsActive)
					{
						Debug.Log("[Director] Detection interaction active; encounter remains suspended.");
					}
					else
					{
						Debug.LogWarning("[Director] Detection interaction could not acquire control before timeout.");
					}
					while (groping != null && groping.IsActive)
					{
						yield return null;
					}
				}
				finally
				{
					this._gropingInterludeOwner = null;
				}
			}
			else
			{
				Debug.LogWarning("[Director] 'Wanna touch?' confirmed, but no GropingModeController was found on the player.");
			}
			if (ScreenFadeController.Instance != null)
			{
				ExhibitionEventDirector.<>c__DisplayClass232_0 CS$<>8__locals1 = new ExhibitionEventDirector.<>c__DisplayClass232_0();
				CS$<>8__locals1.fadeComplete = false;
				ScreenFadeController.Instance.FadeOut(this.FadeDuration, delegate
				{
					CS$<>8__locals1.fadeComplete = true;
				});
				while (!CS$<>8__locals1.fadeComplete)
				{
					yield return null;
				}
				CS$<>8__locals1 = null;
			}
			this.ExitNPCIdleState();
			this.CleanupActiveProps();
			this.RestartNPCBehavior();
			yield return base.StartCoroutine(this.StartSuccessorUnderFadeRoutine());
			if (ScreenFadeController.Instance != null)
			{
				ExhibitionEventDirector.<>c__DisplayClass232_1 CS$<>8__locals2 = new ExhibitionEventDirector.<>c__DisplayClass232_1();
				CS$<>8__locals2.fadeInComplete = false;
				ScreenFadeController.Instance.FadeIn(this.FadeDuration, delegate
				{
					CS$<>8__locals2.fadeInComplete = true;
				});
				while (!CS$<>8__locals2.fadeInComplete)
				{
					yield return null;
				}
				CS$<>8__locals2 = null;
			}
			if (RunModeService.IsLongWayHome)
			{
				yield return base.StartCoroutine(this.PlayExitDialogueWithAutoDismiss());
			}
			else
			{
				base.StartCoroutine(this.PlayExitDialogueWithAutoDismiss());
			}
			yield return base.StartCoroutine(this.FinalCleanupRoutine());
			this.NotifyEncounterResolved();
			yield break;
			yield break;
		}

		// Token: 0x060093B9 RID: 37817 RVA: 0x003CAC94 File Offset: 0x003C8E94
		private bool IsEmoteAllowedInCurrentMode(EmoteDefinitionSO emote)
		{
			if (RunModeService.IsLongWayHome && (emote.Category != ChoiceCategory.Shame || emote.SpecialAction != EmoteSpecialAction.None))
			{
				if (emote.Category == ChoiceCategory.Police)
				{
					EncounterRequest activeEncounterRequest = this._activeEncounterRequest;
					if (((activeEncounterRequest != null) ? activeEncounterRequest.profile : null) != null)
					{
						return this._activeEncounterRequest.profile.actorType == DetectionActorType.Police;
					}
				}
				return false;
			}
			return true;
		}

		// Token: 0x060093BA RID: 37818 RVA: 0x003CACF2 File Offset: 0x003C8EF2
		private void NotifyEncounterResolved()
		{
			LongWayHomeRuntimeController.CompleteEncounter();
			Action onEncounterResolved = this.OnEncounterResolved;
			if (onEncounterResolved == null)
			{
				return;
			}
			onEncounterResolved();
		}

		// Token: 0x060093BB RID: 37819 RVA: 0x003CAD0A File Offset: 0x003C8F0A
		private void NotifyProfileEncounterResolved(EncounterProfileSO profile)
		{
			if (profile != null && profile.actorType == DetectionActorType.Police)
			{
				return;
			}
			this.NotifyEncounterResolved();
		}

		// Token: 0x060093BC RID: 37820 RVA: 0x003CAD28 File Offset: 0x003C8F28
		private bool TryEnterGropingInterlude(GropingModeController groping)
		{
			if (!this.IsEventActive || groping == null || !groping.isActiveAndEnabled)
			{
				return false;
			}
			vThirdPersonInput cachedPlayerInput = this._cachedPlayerInput;
			bool ownsPlayerInputLock = this._ownsPlayerInputLock;
			bool flag = false;
			bool flag2;
			try
			{
				this.UnlockPlayerInput();
				PlayerActivityGate.Pop("DetectionFlow");
				flag = this.IsEventActive && groping.EnterMode();
				flag2 = flag;
			}
			finally
			{
				if (this.IsEventActive)
				{
					if (!flag && ownsPlayerInputLock)
					{
						this.LockPlayerInput(cachedPlayerInput);
					}
					PlayerActivityGate.Push("DetectionFlow", PlayerActivity.Cover | PlayerActivity.Interact | PlayerActivity.Inventory | PlayerActivity.ArmsPose);
				}
			}
			return flag2;
		}

		// Token: 0x060093BD RID: 37821 RVA: 0x003CADC0 File Offset: 0x003C8FC0
		private IEnumerator CleanupAndExitRoutine()
		{
			this.EndDetectionShotProtection();
			this.SetResolutionActionState(false, null);
			if (this._detectingNPC != null)
			{
				AStarNPCController component = this._detectingNPC.GetComponent<AStarNPCController>();
				if (component != null)
				{
					component.SetAction(NPCInterestPoint.ActionType.None);
				}
			}
			if (this._detectingNPC != null)
			{
				Animator componentInChildren = this._detectingNPC.GetComponentInChildren<Animator>();
				if (componentInChildren != null)
				{
					componentInChildren.SetInteger(ExhibitionEventDirector.ReactionTypeHash, 0);
					componentInChildren.SetTrigger(ExhibitionEventDirector.ReactionTriggerHash);
				}
			}
			yield return new WaitForSeconds(0.3f);
			this.CleanupActiveProps();
			this.ExitNPCIdleState();
			if (this._detectingNPC != null)
			{
				this.RestartNPCBehavior();
			}
			if (this._detectingNPC != null)
			{
				this.AddCooldown(this._detectingNPC);
				this._detectingNPC.CurrentAttention = 0f;
				this._detectingNPC.IsDetected = false;
				this._detectingNPC.CurrentDetectionSource = DetectionSource.None;
			}
			this.SetDetectionFlowActive(false);
			this._isInChoicePhase = false;
			this._isInEmoteSelectionPhase = false;
			this._selectedAction = null;
			this._selectedEmote = null;
			this._selectedCategory = null;
			UnityEvent onSequenceComplete = this.OnSequenceComplete;
			if (onSequenceComplete != null)
			{
				onSequenceComplete.Invoke();
			}
			Debug.Log("[Director] Detection sequence complete.");
			this.DeactivateEncounterOverlay();
			EncounterRequest activeEncounterRequest = this._activeEncounterRequest;
			if (activeEncounterRequest != null)
			{
				Action onSceneEnded = activeEncounterRequest.onSceneEnded;
				if (onSceneEnded != null)
				{
					onSceneEnded();
				}
			}
			this._activeEncounterRequest = null;
			this._detectingNPC = null;
			yield break;
		}

		// Token: 0x060093BE RID: 37822 RVA: 0x003CADCF File Offset: 0x003C8FCF
		private IEnumerator FinalCleanupRoutine()
		{
			this.EndDetectionShotProtection();
			this.SetResolutionActionState(false, null);
			this.UnlockPlayerInput();
			Debug.Log("[Director] Player input unlocked in FinalCleanupRoutine");
			if (this._detectingNPC != null)
			{
				this.AddCooldown(this._detectingNPC);
				this._detectingNPC.CurrentAttention = 0f;
				this._detectingNPC.IsDetected = false;
				this._detectingNPC.CurrentDetectionSource = DetectionSource.None;
			}
			if (SensoryManager.Instance != null)
			{
				SensoryManager.Instance.SetPaused(false);
				this._ownsSensoryPause = false;
			}
			this.UnfreezeAllNPCs();
			this.RestoreStaticEncounterActors();
			if (DetectionEmotionSource.Instance != null)
			{
				DetectionEmotionSource.Instance.OnDetectionEnded();
				Debug.Log("[Director] EmotionManager notified - decay and continuous sources re-enabled");
			}
			if (PlayerEmotionController.Instance != null)
			{
				PlayerEmotionController.Instance.OnDetectionEnded();
			}
			if (DetectionAudioManager.Instance != null)
			{
				DetectionAudioManager.Instance.OnDetectionEnded();
			}
			this.SetDetectionFlowActive(false);
			this._isInChoicePhase = false;
			this._isInEmoteSelectionPhase = false;
			this._selectedAction = null;
			this._selectedEmote = null;
			this._selectedCategory = null;
			UnityEvent onSequenceComplete = this.OnSequenceComplete;
			if (onSequenceComplete != null)
			{
				onSequenceComplete.Invoke();
			}
			Debug.Log("[Director] Detection sequence complete.");
			this.DeactivateEncounterOverlay();
			EncounterRequest activeEncounterRequest = this._activeEncounterRequest;
			if (activeEncounterRequest != null)
			{
				Action onSceneEnded = activeEncounterRequest.onSceneEnded;
				if (onSceneEnded != null)
				{
					onSceneEnded();
				}
			}
			this._activeEncounterRequest = null;
			this._detectingNPC = null;
			yield break;
		}

		// Token: 0x060093BF RID: 37823 RVA: 0x003CADDE File Offset: 0x003C8FDE
		private void SetResolutionActionState(bool playing, EmoteDefinitionSO emote)
		{
			this.IsResolutionActionPlaying = playing;
			this.ActiveResolutionEmote = (playing ? emote : null);
		}

		// Token: 0x060093C0 RID: 37824 RVA: 0x003CADF4 File Offset: 0x003C8FF4
		private void RestoreStaticEncounterActors()
		{
			NPCSeatedEncounter seatedEncounter = this._seatedEncounter;
			if (seatedEncounter != null)
			{
				seatedEncounter.Restore();
			}
			this._seatedEncounter = null;
			if (this._phoneEncounter != null)
			{
				this._phoneEncounter.RestoreAfterEncounter();
			}
			this._phoneEncounter = null;
		}

		// Token: 0x060093C1 RID: 37825 RVA: 0x003CAE30 File Offset: 0x003C9030
		private void SetDetectionFlowActive(bool active)
		{
			this._waitingForEmotePhoto = false;
			this._isEventActive = active;
			this._gropingInterludeOwner = null;
			if (!active)
			{
				this.UnlockPlayerInput();
				EncounterInputHints component = base.GetComponent<EncounterInputHints>();
				if (component != null)
				{
					component.SetSkipHintVisible(false);
				}
				IDisposable encounterTrafficPause = this._encounterTrafficPause;
				if (encounterTrafficPause != null)
				{
					encounterTrafficPause.Dispose();
				}
				this._encounterTrafficPause = null;
				PlayerActivityGate.Pop("DetectionFlow");
				return;
			}
			this._encounterResolutionStarted = false;
			this._encounterSkipped = false;
			this._preReactionStarted = false;
			if (this.SkipCountdownText)
			{
				this.SkipCountdownText.text = "";
			}
			if (this.SkipTimerFill)
			{
				this.SkipTimerFill.fillAmount = 0f;
			}
			this._pendingExitPresentation = null;
			if (this._encounterTrafficPause == null)
			{
				this._encounterTrafficPause = VehicleSimulationPause.AcquirePause();
			}
			this._detectionCameraReleased = false;
			PlayerActivityGate.Push("DetectionFlow", PlayerActivity.Cover | PlayerActivity.Interact | PlayerActivity.Inventory | PlayerActivity.ArmsPose);
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (instance == null)
			{
				return;
			}
			instance.Hide();
		}

		// Token: 0x060093C2 RID: 37826 RVA: 0x003CAF21 File Offset: 0x003C9121
		private ExhibitionEventDirector.DialoguePhase ResolveDialoguePhase(PlayerActionEntry playerAction, EmoteDefinitionSO emote)
		{
			if (emote != null)
			{
				if (emote.Category != ChoiceCategory.Shame)
				{
					return ExhibitionEventDirector.DialoguePhase.PostExhibit;
				}
				return ExhibitionEventDirector.DialoguePhase.PostShame;
			}
			else
			{
				if (playerAction != null && playerAction.ActionId.ToLower().Contains("shame"))
				{
					return ExhibitionEventDirector.DialoguePhase.PostShame;
				}
				return ExhibitionEventDirector.DialoguePhase.PostExhibit;
			}
		}

		// Token: 0x060093C3 RID: 37827 RVA: 0x003CAF58 File Offset: 0x003C9158
		private void UpdateEmotionLevels(PlayerActionEntry playerAction, EmoteDefinitionSO emote = null)
		{
			if (playerAction == null)
			{
				return;
			}
			ChoiceCategory? choiceCategory = null;
			if (emote != null)
			{
				choiceCategory = new ChoiceCategory?(emote.Category);
			}
			else
			{
				string text = playerAction.ActionId.ToLower();
				if (text.Contains("exhibit"))
				{
					choiceCategory = new ChoiceCategory?(ChoiceCategory.Exhibit);
				}
				else if (text.Contains("shame"))
				{
					choiceCategory = new ChoiceCategory?(ChoiceCategory.Shame);
				}
			}
			EncounterRequest activeEncounterRequest = this._activeEncounterRequest;
			EncounterProfileSO encounterProfileSO = ((activeEncounterRequest != null) ? activeEncounterRequest.profile : null);
			if (encounterProfileSO != null && encounterProfileSO.overrideEmotionImpactCategory)
			{
				choiceCategory = new ChoiceCategory?(encounterProfileSO.forcedEmotionImpactCategory);
				Debug.Log(string.Format("[Director] Emotion impact category overridden by profile '{0}' → {1}", encounterProfileSO.name, choiceCategory));
			}
			if (DetectionEmotionSource.Instance != null)
			{
				ChoiceCategory? choiceCategory2 = choiceCategory;
				ChoiceCategory choiceCategory3 = ChoiceCategory.Exhibit;
				if ((choiceCategory2.GetValueOrDefault() == choiceCategory3) & (choiceCategory2 != null))
				{
					DetectionEmotionSource.Instance.OnPlayerChoice(PlayerChoiceType.Expose, 0f);
					Debug.Log("[Director] Player chose Exhibit - EmotionBalance updated via EmotionManager");
				}
				else
				{
					choiceCategory2 = choiceCategory;
					choiceCategory3 = ChoiceCategory.Shame;
					if ((choiceCategory2.GetValueOrDefault() == choiceCategory3) & (choiceCategory2 != null))
					{
						DetectionEmotionSource.Instance.OnPlayerChoice(PlayerChoiceType.Shame, 0f);
						Debug.Log("[Director] Player chose Shame - EmotionBalance updated via EmotionManager");
					}
					else
					{
						DetectionEmotionSource.Instance.OnPlayerChoice(PlayerChoiceType.Flee, 0f);
						Debug.Log("[Director] Player chose Flee/Skip - EmotionBalance updated via EmotionManager");
					}
				}
			}
			else if (PlayerEmotionController.Instance != null && choiceCategory != null)
			{
				if (choiceCategory.Value == ChoiceCategory.Exhibit)
				{
					PlayerEmotionController.Instance.OnPlayerActionSelected(ChoiceCategory.Exhibit, this.ExhibitLevelIncrease, this.ExhibitShameDecrease);
				}
				else
				{
					PlayerEmotionController.Instance.OnPlayerActionSelected(ChoiceCategory.Shame, this.ShameLevelIncrease, this.ShameExhibitDecrease);
				}
			}
			if (PlayerEmotionController.Instance != null)
			{
				PlayerEmotionController.Instance.TriggerExpressionUpdate();
			}
		}

		// Token: 0x060093C4 RID: 37828 RVA: 0x003CB110 File Offset: 0x003C9310
		private void LockPlayerInput(vThirdPersonInput input)
		{
			if (this._ownsPlayerInputLock)
			{
				this.ReassertPlayerInputLock();
				return;
			}
			if (input == null)
			{
				return;
			}
			AutoLocomotionController component = input.GetComponent<AutoLocomotionController>();
			if (component != null)
			{
				component.CancelAutoLocomotion();
			}
			this._cachedPlayerInput = input;
			this._lockedPlayerController = input.cc;
			this._savedPlayerInputLock = input.lockInput;
			this._savedPlayerMoveInputLock = input.lockMoveInput;
			this._savedPlayerMoveDirectionLock = input.lockUpdateMoveDirection;
			if (this._lockedPlayerController != null)
			{
				this._savedPlayerMovementLock = this._lockedPlayerController.lockMovement;
				this._savedPlayerRotationLock = this._lockedPlayerController.lockRotation;
				this._lockedPlayerController.isSprinting = false;
				Rigidbody component2 = this._lockedPlayerController.GetComponent<Rigidbody>();
				if (component2 != null && !component2.isKinematic)
				{
					component2.velocity = Vector3.zero;
					component2.angularVelocity = Vector3.zero;
				}
				Animator animator = this._lockedPlayerController.animator;
				if (animator != null)
				{
					animator.SetFloat("InputHorizontal", 0f);
					animator.SetFloat("InputVertical", 0f);
					animator.SetFloat("InputMagnitude", 0f);
				}
			}
			this._ownsPlayerInputLock = true;
			this.ReassertPlayerInputLock();
		}

		// Token: 0x060093C5 RID: 37829 RVA: 0x003CB244 File Offset: 0x003C9444
		private void ReassertPlayerInputLock()
		{
			if (!this._ownsPlayerInputLock)
			{
				return;
			}
			if (this._cachedPlayerInput != null)
			{
				this._cachedPlayerInput.lockInput = true;
				this._cachedPlayerInput.lockMoveInput = true;
				this._cachedPlayerInput.lockUpdateMoveDirection = true;
			}
			if (this._lockedPlayerController == null)
			{
				return;
			}
			this._lockedPlayerController.lockMovement = true;
			this._lockedPlayerController.lockRotation = true;
			this._lockedPlayerController.input = Vector3.zero;
			this._lockedPlayerController.inputSmooth = Vector3.zero;
			this._lockedPlayerController.moveDirection = Vector3.zero;
		}

		// Token: 0x060093C6 RID: 37830 RVA: 0x003CB2E4 File Offset: 0x003C94E4
		private void UnlockPlayerInput()
		{
			if (!this._ownsPlayerInputLock)
			{
				return;
			}
			this._ownsPlayerInputLock = false;
			if (this._cachedPlayerInput != null)
			{
				this._cachedPlayerInput.lockInput = this._savedPlayerInputLock;
				this._cachedPlayerInput.lockMoveInput = this._savedPlayerMoveInputLock;
				this._cachedPlayerInput.lockUpdateMoveDirection = this._savedPlayerMoveDirectionLock;
			}
			if (this._lockedPlayerController != null)
			{
				this._lockedPlayerController.lockMovement = this._savedPlayerMovementLock;
				this._lockedPlayerController.lockRotation = this._savedPlayerRotationLock;
			}
			this._lockedPlayerController = null;
			this._cachedPlayerInput = null;
		}

		// Token: 0x060093C7 RID: 37831 RVA: 0x003CB380 File Offset: 0x003C9580
		private void RestartNPCBehavior()
		{
			if (this._detectingNPC == null)
			{
				return;
			}
			this._detectingNPC.IsDetected = false;
			this._detectingNPC.CurrentAttention = 0f;
			this._detectingNPC.CurrentDetectionSource = DetectionSource.None;
			if (!(this._phoneEncounter != null))
			{
				NPCSeatedEncounter seatedEncounter = this._seatedEncounter;
				if (seatedEncounter == null || !seatedEncounter.RestoresSeatedPose)
				{
					NPCStateMachine component = this._detectingNPC.GetComponent<NPCStateMachine>();
					if (component != null)
					{
						component.ForceState(NPCStateType.Idle, "Director");
					}
					else
					{
						AStarNPCController component2 = this._detectingNPC.GetComponent<AStarNPCController>();
						if (component2 != null)
						{
							component2.UnlockNavigation(false);
						}
					}
					this.DisableAlertLayers();
					NPCWanderController component3 = this._detectingNPC.GetComponent<NPCWanderController>();
					if (component3 != null)
					{
						component3.ForceStop();
						component3.enabled = true;
						Debug.Log("[Director] " + this._detectingNPC.name + ": WanderController restarted for new POI (IsDetected reset)");
					}
					else
					{
						this.ResumeNPCMovement();
					}
					this.RestoreStaticEncounterActors();
					return;
				}
			}
			NPCStateMachine component4 = this._detectingNPC.GetComponent<NPCStateMachine>();
			if (component4 != null && component4.IsInitialized)
			{
				component4.ForceState(NPCStateType.Idle, "Director");
			}
			AStarNPCController component5 = this._detectingNPC.GetComponent<AStarNPCController>();
			if (component5 != null)
			{
				component5.UnlockNavigation(false);
			}
			this.RestoreStaticEncounterActors();
		}

		// Token: 0x060093C8 RID: 37832 RVA: 0x003CB4CC File Offset: 0x003C96CC
		private void CleanupActiveProps()
		{
			if (this.PlayerEmotePropHandler != null)
			{
				this.PlayerEmotePropHandler.CleanupAllProps();
				Debug.Log("[Director] Props cleaned up via EmotePropHandler (new emote system)");
			}
			if (this._detectingNPC != null)
			{
				NPCEquipmentManager component = this._detectingNPC.GetComponent<NPCEquipmentManager>();
				if (component != null)
				{
					component.DeactivateAllProps();
					Debug.Log("[Director] " + this._detectingNPC.name + ": Props deactivated via NPCEquipmentManager");
				}
			}
			if (this._activeProps != null && this._activeProps.Count > 0)
			{
				foreach (GameObject gameObject in this._activeProps)
				{
					if (gameObject)
					{
						gameObject.SetActive(false);
					}
				}
				this._activeProps.Clear();
				Debug.Log("[Director] Props cleaned up via legacy manual list");
			}
		}

		// Token: 0x060093C9 RID: 37833 RVA: 0x003CB5C0 File Offset: 0x003C97C0
		private void TriggerReactionAnimation(NPCReactionEntry reaction, bool clearPreviousProps = false)
		{
			if (this._detectingNPC == null || reaction == null)
			{
				return;
			}
			Animator componentInChildren = this._detectingNPC.GetComponentInChildren<Animator>();
			if (componentInChildren != null)
			{
				int num = 4;
				int num2 = 1;
				if (num >= 0 && num < componentInChildren.layerCount)
				{
					componentInChildren.SetLayerWeight(num, 0f);
					Debug.Log("[Director] Forced AlertBehavior layer to 0 for " + this._detectingNPC.name + " during reaction");
				}
				if (num2 >= 0 && num2 < componentInChildren.layerCount)
				{
					componentInChildren.SetLayerWeight(num2, 1f);
					Debug.Log("[Director] Enabled Reactions layer (weight=1) for " + this._detectingNPC.name);
				}
			}
			NPCEquipmentManager component = this._detectingNPC.GetComponent<NPCEquipmentManager>();
			if (component != null)
			{
				if (clearPreviousProps)
				{
					component.DeactivateAllProps();
				}
				if (reaction.PropsToActivate == null || reaction.PropsToActivate.Count <= 0)
				{
					goto IL_01E8;
				}
				using (List<string>.Enumerator enumerator = reaction.PropsToActivate.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string text = enumerator.Current;
						if (!string.IsNullOrEmpty(text))
						{
							component.ActivateAndLockProp(text);
							Debug.Log(string.Format("[Director] Activated prop '{0}' for reaction {1}", text, reaction.Type));
						}
					}
					goto IL_01E8;
				}
			}
			if (clearPreviousProps)
			{
				this.CleanupActiveProps();
			}
			if (reaction.PropsToActivate != null && reaction.PropsToActivate.Count > 0)
			{
				foreach (string text2 in reaction.PropsToActivate)
				{
					if (!string.IsNullOrEmpty(text2))
					{
						Transform transform = this.FindDeepChild(this._detectingNPC.transform, text2);
						if (transform != null)
						{
							this.ActivatePropWithParents(transform);
							this._activeProps.Add(transform.gameObject);
							Debug.Log(string.Format("[Director] Activated prop '{0}' (legacy) for reaction {1}", text2, reaction.Type));
						}
					}
				}
			}
			IL_01E8:
			if (componentInChildren != null)
			{
				componentInChildren.SetInteger(ExhibitionEventDirector.ReactionTypeHash, reaction.AnimatorReactionIndex);
				componentInChildren.SetTrigger(ExhibitionEventDirector.ReactionTriggerHash);
				NPCDialogueHolder component2 = this._detectingNPC.GetComponent<NPCDialogueHolder>();
				if (component2 != null && component2.PersonalityProfile != null)
				{
					int expressionForReaction = component2.GetExpressionForReaction(reaction.Type);
					string expressionParamName = component2.PersonalityProfile.expressionParamName;
					if (!string.IsNullOrEmpty(expressionParamName))
					{
						componentInChildren.SetInteger(expressionParamName, expressionForReaction);
						Debug.Log(string.Format("[Director] Set NPC facial expression: {0}={1} for reaction {2}", expressionParamName, expressionForReaction, reaction.Type));
					}
				}
			}
		}

		// Token: 0x060093CA RID: 37834 RVA: 0x003CB874 File Offset: 0x003C9A74
		private bool IsInCooldown(NPCSenses npc)
		{
			return this._cooldownList.Contains(npc);
		}

		// Token: 0x060093CB RID: 37835 RVA: 0x003CB882 File Offset: 0x003C9A82
		private void AddCooldown(NPCSenses npc)
		{
			if (npc == null)
			{
				return;
			}
			npc.IsInCooldown = true;
			this._cooldownList.Add(npc);
			base.StartCoroutine(this.RemoveCooldownRoutine(npc));
		}

		// Token: 0x060093CC RID: 37836 RVA: 0x003CB8B0 File Offset: 0x003C9AB0
		private void ClearCooldownsForCancellation()
		{
			for (int i = 0; i < this._cooldownList.Count; i++)
			{
				NPCSenses npcsenses = this._cooldownList[i];
				if (npcsenses != null)
				{
					npcsenses.IsInCooldown = false;
				}
			}
			this._cooldownList.Clear();
		}

		// Token: 0x060093CD RID: 37837 RVA: 0x003CB8FB File Offset: 0x003C9AFB
		private IEnumerator RemoveCooldownRoutine(NPCSenses npc)
		{
			float num = ((this.GlobalConfig != null) ? this.GlobalConfig.CooldownTime : 10f);
			yield return new WaitForSeconds(num);
			this._cooldownList.Remove(npc);
			if (npc != null)
			{
				npc.IsInCooldown = false;
				UnityEvent<NPCSenses> onCooldownEnded = this.OnCooldownEnded;
				if (onCooldownEnded != null)
				{
					onCooldownEnded.Invoke(npc);
				}
			}
			yield break;
		}

		// Token: 0x060093CE RID: 37838 RVA: 0x003CB914 File Offset: 0x003C9B14
		private void RotateNPCToFacePlayer()
		{
			if (this._detectingNPC == null || this._cachedPlayerInput == null)
			{
				return;
			}
			Transform transform = this._detectingNPC.transform;
			Vector3 vector = this._cachedPlayerInput.transform.position - transform.position;
			vector.y = 0f;
			if (vector.sqrMagnitude > 0.01f)
			{
				transform.rotation = Quaternion.LookRotation(vector);
				Debug.Log("[Director] " + this._detectingNPC.name + " rotated to face player (instant)");
			}
		}

		// Token: 0x060093CF RID: 37839 RVA: 0x003CB9AC File Offset: 0x003C9BAC
		private void RotatePlayerToFaceNPC()
		{
			if (this._cachedPlayerInput == null || this._detectingNPC == null)
			{
				return;
			}
			Transform transform = this._cachedPlayerInput.transform;
			Vector3 vector = this._detectingNPC.transform.position - transform.position;
			vector.y = 0f;
			if (vector.sqrMagnitude > 0.01f)
			{
				transform.rotation = Quaternion.LookRotation(vector);
				Debug.Log("[Director] Player rotated to face " + this._detectingNPC.name + " (instant)");
			}
		}

		// Token: 0x060093D0 RID: 37840 RVA: 0x003CBA43 File Offset: 0x003C9C43
		private IEnumerator TurnNPCTowardsPlayer()
		{
			if (this._detectingNPC == null)
			{
				yield break;
			}
			Transform npcTransform = this._detectingNPC.transform;
			float maxTurnTime = 2f;
			float elapsed = 0f;
			while (elapsed < maxTurnTime)
			{
				if (this._detectingNPC == null)
				{
					yield break;
				}
				Vector3 vector = this._detectingNPC.LastKnownPlayerPosition - npcTransform.position;
				vector.y = 0f;
				if (vector.sqrMagnitude < 0.01f)
				{
					break;
				}
				Quaternion quaternion = Quaternion.LookRotation(vector);
				if (Quaternion.Angle(npcTransform.rotation, quaternion) <= 2f)
				{
					npcTransform.rotation = quaternion;
					break;
				}
				npcTransform.rotation = Quaternion.RotateTowards(npcTransform.rotation, quaternion, this.TurnSpeed * Time.deltaTime);
				elapsed += Time.deltaTime;
				yield return null;
			}
			if (this._detectingNPC != null)
			{
				Vector3 vector2 = this._detectingNPC.LastKnownPlayerPosition - npcTransform.position;
				vector2.y = 0f;
				if (vector2.sqrMagnitude > 0.01f)
				{
					npcTransform.rotation = Quaternion.LookRotation(vector2);
				}
			}
			Debug.Log("[Director] " + this._detectingNPC.name + " finished turning to face player.");
			yield break;
		}

		// Token: 0x060093D1 RID: 37841 RVA: 0x003CBA54 File Offset: 0x003C9C54
		private void ActivateCinematicCamera()
		{
			if (this.CinematicCameras == null || this.CinematicCameras.Count == 0)
			{
				return;
			}
			if (Camera.main == null)
			{
				return;
			}
			if (this._anchorCamera == null)
			{
				GameObject gameObject = new GameObject("CM_GameplayAnchor");
				this._anchorCamera = gameObject.AddComponent<CinemachineVirtualCamera>();
				this._anchorCamera.Priority = 10;
				this._anchorCamera.m_Lens.FieldOfView = Camera.main.fieldOfView;
			}
			if (Camera.main.transform.parent != null)
			{
				this._anchorCamera.transform.SetParent(Camera.main.transform.parent);
				this._anchorCamera.transform.localPosition = Camera.main.transform.localPosition;
				this._anchorCamera.transform.localRotation = Camera.main.transform.localRotation;
			}
			else
			{
				this._anchorCamera.transform.position = Camera.main.transform.position;
				this._anchorCamera.transform.rotation = Camera.main.transform.rotation;
			}
			this._anchorCamera.gameObject.SetActive(true);
			if (this._mainCameraBrain == null)
			{
				this._mainCameraBrain = Camera.main.GetComponent<CinemachineBrain>();
			}
			if (this._mainCameraBrain != null)
			{
				if (!this._hasSavedCameraState)
				{
					this._savedCameraLocalPosition = Camera.main.transform.localPosition;
					this._savedCameraLocalRotation = Camera.main.transform.localRotation;
					this._hasSavedCameraState = true;
				}
				if (this._mainCameraBrain.m_DefaultBlend.m_Time != this.CameraBlendTime)
				{
					this._mainCameraBrain.m_DefaultBlend.m_Time = this.CameraBlendTime;
				}
				this._mainCameraBrain.enabled = true;
				int num;
				if (this.RandomizeCameraSelection && this.CinematicCameras.Count > 1)
				{
					do
					{
						num = Random.Range(0, this.CinematicCameras.Count);
					}
					while (num == this._lastCameraIndex);
				}
				else
				{
					num = (this._lastCameraIndex + 1) % this.CinematicCameras.Count;
				}
				this._lastCameraIndex = num;
				this._activeCinematicCamera = this.CinematicCameras[num];
				if (this._activeCinematicCamera != null)
				{
					CinemachineVirtualCamera component = this._activeCinematicCamera.GetComponent<CinemachineVirtualCamera>();
					if (component != null)
					{
						component.Priority = 20;
					}
					this._activeCinematicCamera.SetActive(true);
					Debug.Log("[Director] Blending to cinematic camera: " + this._activeCinematicCamera.name);
				}
				return;
			}
			Debug.LogWarning("[Director] CinemachineBrain NOT found! Transition will be abrupt.");
		}

		// Token: 0x060093D2 RID: 37842 RVA: 0x003CBCF6 File Offset: 0x003C9EF6
		private IEnumerator DeactivateCinematicCameraRoutine()
		{
			this.ReleaseDetectionCamera();
			if (this._activeCinematicCamera != null)
			{
				this._activeCinematicCamera.SetActive(false);
				Debug.Log("[Director] Deactivated cinematic camera, returning to anchor...");
			}
			yield return new WaitForSecondsRealtime(this.CameraBlendTime);
			this.RestoreGameplayCameraImmediate();
			yield break;
		}

		// Token: 0x060093D3 RID: 37843 RVA: 0x003CBD08 File Offset: 0x003C9F08
		private void RestoreGameplayCameraImmediate()
		{
			if (this._mainCameraBrain != null)
			{
				this._mainCameraBrain.enabled = false;
			}
			if (this._hasSavedCameraState && Camera.main != null)
			{
				Camera.main.transform.localPosition = this._savedCameraLocalPosition;
				Camera.main.transform.localRotation = this._savedCameraLocalRotation;
				this._hasSavedCameraState = false;
				Debug.Log("[Director] Camera transform restored to pre-sequence state.");
			}
			if (this._anchorCamera != null)
			{
				this._anchorCamera.gameObject.SetActive(false);
			}
		}

		// Token: 0x060093D4 RID: 37844 RVA: 0x003CBDA0 File Offset: 0x003C9FA0
		private void StopNPCMovement()
		{
			if (this._detectingNPC == null)
			{
				return;
			}
			AStarNPCController component = this._detectingNPC.GetComponent<AStarNPCController>();
			if (component != null)
			{
				this._wasNavAgentActive = component.follower != null && !component.follower.isStopped;
				component.Stop();
				component.ClearSpeedOverride();
			}
		}

		// Token: 0x060093D5 RID: 37845 RVA: 0x003CBE04 File Offset: 0x003CA004
		private void ResumeNPCMovement()
		{
			if (this._detectingNPC == null)
			{
				return;
			}
			AStarNPCController component = this._detectingNPC.GetComponent<AStarNPCController>();
			if (component != null)
			{
				component.ExitManualMode(false);
				if (this._wasNavAgentActive && component.follower != null)
				{
					component.follower.isStopped = false;
				}
			}
		}

		// Token: 0x060093D6 RID: 37846 RVA: 0x003CBE60 File Offset: 0x003CA060
		private void DisableAlertLayers()
		{
			if (this._detectingNPC == null)
			{
				return;
			}
			Animator componentInChildren = this._detectingNPC.GetComponentInChildren<Animator>();
			if (componentInChildren == null)
			{
				return;
			}
			int num = 4;
			int num2 = 1;
			if (num >= 0 && num < componentInChildren.layerCount)
			{
				componentInChildren.SetLayerWeight(num, 0f);
			}
			if (num2 >= 0 && num2 < componentInChildren.layerCount)
			{
				componentInChildren.SetLayerWeight(num2, 0f);
			}
			componentInChildren.SetFloat("Speed", 0f);
			Debug.Log("[Director] " + this._detectingNPC.name + ": Alert layers disabled, Speed reset to 0");
		}

		// Token: 0x060093D7 RID: 37847 RVA: 0x003CBEF8 File Offset: 0x003CA0F8
		private void ReenableAlertBehaviorLayer()
		{
			if (this._detectingNPC == null)
			{
				return;
			}
			Animator componentInChildren = this._detectingNPC.GetComponentInChildren<Animator>();
			if (componentInChildren == null)
			{
				return;
			}
			int num = 4;
			if (num < 0 || num >= componentInChildren.layerCount)
			{
				return;
			}
			componentInChildren.SetLayerWeight(num, 1f);
			Debug.Log("[Director] " + this._detectingNPC.name + ": AlertBehavior layer re-enabled for future HeadLook/Searching");
		}

		// Token: 0x060093D8 RID: 37848 RVA: 0x003CBF68 File Offset: 0x003CA168
		private Transform FindDeepChild(Transform parent, string name)
		{
			Transform transform = parent.Find(name);
			if (transform != null)
			{
				return transform;
			}
			foreach (object obj in parent)
			{
				Transform transform2 = (Transform)obj;
				transform = this.FindDeepChild(transform2, name);
				if (transform != null)
				{
					return transform;
				}
			}
			return null;
		}

		// Token: 0x060093D9 RID: 37849 RVA: 0x003CBFE4 File Offset: 0x003CA1E4
		private void ActivatePropWithParents(Transform propTransform)
		{
			if (propTransform == null)
			{
				return;
			}
			propTransform.gameObject.SetActive(true);
			Transform transform = propTransform.parent;
			Transform transform2 = ((this._detectingNPC != null) ? this._detectingNPC.transform : null);
			while (transform != null && transform != transform2)
			{
				if (!transform.gameObject.activeSelf)
				{
					transform.gameObject.SetActive(true);
					Debug.Log("[Director] Activated parent '" + transform.name + "' for prop visibility");
				}
				transform = transform.parent;
			}
		}

		// Token: 0x060093DA RID: 37850 RVA: 0x003CC07C File Offset: 0x003CA27C
		private string GetTransformPath(Transform t)
		{
			if (t == null)
			{
				return "null";
			}
			string text = t.name;
			while (t.parent != null)
			{
				t = t.parent;
				text = t.name + "/" + text;
			}
			return text;
		}

		// Token: 0x060093DB RID: 37851 RVA: 0x003CC0CC File Offset: 0x003CA2CC
		private void SavePlayerAnimatorLayerWeights(Animator animator)
		{
			if (animator == null)
			{
				return;
			}
			this._savedAnimatorLayerWeights.Clear();
			for (int i = 0; i < animator.layerCount; i++)
			{
				if (!ExhibitionEventDirector.ShouldSkipLayerWeightSnapshot(animator.GetLayerName(i)))
				{
					float layerWeight = animator.GetLayerWeight(i);
					this._savedAnimatorLayerWeights[i] = layerWeight;
				}
			}
			Debug.Log(string.Format("[Director] Saved {0} animator layer weights", this._savedAnimatorLayerWeights.Count));
		}

		// Token: 0x060093DC RID: 37852 RVA: 0x003CC144 File Offset: 0x003CA344
		private void RestorePlayerAnimatorLayerWeights(Animator animator)
		{
			if (animator == null || this._savedAnimatorLayerWeights.Count == 0)
			{
				return;
			}
			foreach (KeyValuePair<int, float> keyValuePair in this._savedAnimatorLayerWeights)
			{
				if (keyValuePair.Key < animator.layerCount && !ExhibitionEventDirector.ShouldSkipLayerWeightSnapshot(animator.GetLayerName(keyValuePair.Key)))
				{
					animator.SetLayerWeight(keyValuePair.Key, keyValuePair.Value);
				}
			}
			Debug.Log(string.Format("[Director] Restored {0} animator layer weights", this._savedAnimatorLayerWeights.Count));
			this._savedAnimatorLayerWeights.Clear();
		}

		// Token: 0x060093DD RID: 37853 RVA: 0x003CC208 File Offset: 0x003CA408
		private static bool ShouldSkipLayerWeightSnapshot(string layerName)
		{
			if (string.IsNullOrEmpty(layerName))
			{
				return false;
			}
			string text = layerName.ToLowerInvariant();
			return text.Contains("face") || text.Contains("mouth");
		}

		// Token: 0x060093DE RID: 37854 RVA: 0x003CC240 File Offset: 0x003CA440
		private void FreezeAllNPCsAndSetupSpectators()
		{
			this._frozenNPCs.Clear();
			this._spectatorNPCs.Clear();
			this._savedWanderControllerStates.Clear();
			this._savedFollowerStates.Clear();
			Vector3 vector = ((this._cachedPlayerInput != null) ? this._cachedPlayerInput.transform.position : ((this._detectingNPC != null) ? this._detectingNPC.LastKnownPlayerPosition : Vector3.zero));
			foreach (NPCSenses npcsenses in Object.FindObjectsOfType<NPCSenses>())
			{
				if (!(npcsenses == null) && !(npcsenses == this._detectingNPC))
				{
					NPCWanderController component = npcsenses.GetComponent<NPCWanderController>();
					if (component != null)
					{
						this._savedWanderControllerStates[npcsenses] = component.enabled;
						if (this.FreezeAllNPCsDuringDetection)
						{
							component.enabled = false;
						}
					}
					AStarNPCController component2 = npcsenses.GetComponent<AStarNPCController>();
					if (component2 != null && component2.follower != null)
					{
						this._savedFollowerStates[npcsenses] = !component2.follower.isStopped;
						if (this.FreezeAllNPCsDuringDetection)
						{
							component2.Stop();
							component2.ClearSpeedOverride();
						}
					}
					npcsenses.CurrentAttention = 0f;
					npcsenses.CurrentDetectionSource = DetectionSource.None;
					this._frozenNPCs.Add(npcsenses);
					if (Vector3.Distance(npcsenses.transform.position, vector) <= this.SpectatorRadius)
					{
						this._spectatorNPCs.Add(npcsenses);
					}
				}
			}
			AttentionUIController.GlobalForceHideAll = true;
			Debug.Log(string.Format("[Director] Frozen {0} NPCs, {1} spectators within {2}m radius", this._frozenNPCs.Count, this._spectatorNPCs.Count, this.SpectatorRadius));
			this.SetSpectatorFlashVolume(this.SpectatorFlashVolumeMultiplier);
			if (this.SpectatorsFacePlayer)
			{
				this.SetupSpectatorReactions(vector);
			}
		}

		// Token: 0x060093DF RID: 37855 RVA: 0x003CC41C File Offset: 0x003CA61C
		private void SetSpectatorFlashVolume(float volumeMultiplier)
		{
			foreach (NPCSenses npcsenses in this._spectatorNPCs)
			{
				if (!(npcsenses == null))
				{
					SmartphoneFlash componentInChildren = npcsenses.GetComponentInChildren<SmartphoneFlash>();
					if (componentInChildren != null)
					{
						componentInChildren.SetVolumeMultiplier(volumeMultiplier);
					}
				}
			}
			Debug.Log(string.Format("[Director] Set spectator flash volume to {0:P0}", volumeMultiplier));
		}

		// Token: 0x060093E0 RID: 37856 RVA: 0x003CC4A0 File Offset: 0x003CA6A0
		private void ResetSpectatorFlashVolume()
		{
			foreach (NPCSenses npcsenses in this._spectatorNPCs)
			{
				if (!(npcsenses == null))
				{
					SmartphoneFlash componentInChildren = npcsenses.GetComponentInChildren<SmartphoneFlash>();
					if (componentInChildren != null)
					{
						componentInChildren.ResetVolumeMultiplier();
					}
				}
			}
			Debug.Log("[Director] Reset spectator flash volume to normal");
		}

		// Token: 0x060093E1 RID: 37857 RVA: 0x003CC518 File Offset: 0x003CA718
		private void SetupSpectatorReactions(Vector3 playerPosition)
		{
			foreach (NPCSenses npcsenses in this._spectatorNPCs)
			{
				if (!(npcsenses == null))
				{
					EncounterRequest activeEncounterRequest = this._activeEncounterRequest;
					bool flag;
					if (activeEncounterRequest == null)
					{
						flag = false;
					}
					else
					{
						List<NPCSenses> additionalActors = activeEncounterRequest.additionalActors;
						bool? flag2 = ((additionalActors != null) ? new bool?(additionalActors.Contains(npcsenses)) : null);
						bool flag3 = true;
						flag = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
					}
					if (!flag && !npcsenses.UsesStaticPhoneReaction && !npcsenses.IsSeatedObserver)
					{
						Vector3 vector = playerPosition - npcsenses.transform.position;
						vector.y = 0f;
						if (vector.sqrMagnitude > 0.01f)
						{
							npcsenses.transform.rotation = Quaternion.LookRotation(vector);
						}
						NPCDialogueHolder component = npcsenses.GetComponent<NPCDialogueHolder>();
						NPCReactionProfileSO npcreactionProfileSO = ((component != null) ? component.GetReactionProfile() : null) ?? npcsenses.ReactionProfile;
						Animator componentInChildren = npcsenses.GetComponentInChildren<Animator>();
						if (componentInChildren != null)
						{
							int num = 1;
							if (num >= 0 && num < componentInChildren.layerCount)
							{
								componentInChildren.SetLayerWeight(num, 1f);
							}
							int num2 = this.SpectatorDefaultReactionIndex;
							if (npcreactionProfileSO != null)
							{
								NPCReactionEntry randomReaction = npcreactionProfileSO.GetRandomReaction();
								if (randomReaction != null)
								{
									num2 = randomReaction.AnimatorReactionIndex;
									if (((component != null) ? component.PersonalityProfile : null) != null)
									{
										int expressionForReaction = component.GetExpressionForReaction(randomReaction.Type);
										string expressionParamName = component.PersonalityProfile.expressionParamName;
										if (!string.IsNullOrEmpty(expressionParamName))
										{
											componentInChildren.SetInteger(expressionParamName, expressionForReaction);
										}
									}
									this.ActivateSpectatorProps(npcsenses, randomReaction);
								}
							}
							if (!string.IsNullOrEmpty(this.SpectatorReactionTypeParam))
							{
								componentInChildren.SetInteger(this.SpectatorReactionTypeParam, num2);
							}
							if (!string.IsNullOrEmpty(this.SpectatorReactionTrigger))
							{
								componentInChildren.SetTrigger(this.SpectatorReactionTrigger);
							}
							Debug.Log(string.Format("[Director] Spectator {0} facing player with reaction index {1}, Reactions layer enabled", npcsenses.name, num2));
						}
					}
				}
			}
		}

		// Token: 0x060093E2 RID: 37858 RVA: 0x003CC740 File Offset: 0x003CA940
		private void ActivateSpectatorProps(NPCSenses spectator, NPCReactionEntry reaction)
		{
			if (spectator == null || reaction == null)
			{
				return;
			}
			if (reaction.PropsToActivate == null || reaction.PropsToActivate.Count == 0)
			{
				return;
			}
			NPCEquipmentManager component = spectator.GetComponent<NPCEquipmentManager>();
			if (component != null)
			{
				using (List<string>.Enumerator enumerator = reaction.PropsToActivate.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string text = enumerator.Current;
						if (!string.IsNullOrEmpty(text))
						{
							component.ActivateAndLockProp(text);
							Debug.Log(string.Concat(new string[] { "[Director] Spectator ", spectator.name, ": Activated prop '", text, "'" }));
						}
					}
					return;
				}
			}
			foreach (string text2 in reaction.PropsToActivate)
			{
				if (!string.IsNullOrEmpty(text2))
				{
					Transform transform = this.FindDeepChild(spectator.transform, text2);
					if (transform != null)
					{
						this.ActivatePropWithParents(transform);
						Debug.Log(string.Concat(new string[] { "[Director] Spectator ", spectator.name, ": Activated prop '", text2, "' (legacy)" }));
					}
				}
			}
		}

		// Token: 0x060093E3 RID: 37859 RVA: 0x003CC8A0 File Offset: 0x003CAAA0
		private void UnfreezeAllNPCs()
		{
			foreach (NPCSenses npcsenses in this._frozenNPCs)
			{
				if (!(npcsenses == null))
				{
					NPCWanderController component = npcsenses.GetComponent<NPCWanderController>();
					bool flag;
					if (component != null && this._savedWanderControllerStates.TryGetValue(npcsenses, out flag))
					{
						component.enabled = flag;
					}
					AStarNPCController component2 = npcsenses.GetComponent<AStarNPCController>();
					bool flag2;
					if (component2 != null && component2.follower != null && (this._savedFollowerStates.TryGetValue(npcsenses, out flag2) && flag2))
					{
						component2.follower.isStopped = false;
					}
					if (this._spectatorNPCs.Contains(npcsenses) && !npcsenses.UsesStaticPhoneReaction && !npcsenses.IsSeatedObserver)
					{
						Animator componentInChildren = npcsenses.GetComponentInChildren<Animator>();
						if (componentInChildren != null)
						{
							int num = 1;
							if (num >= 0 && num < componentInChildren.layerCount)
							{
								componentInChildren.SetLayerWeight(num, 0f);
							}
							if (!string.IsNullOrEmpty(this.SpectatorReactionTypeParam))
							{
								componentInChildren.SetInteger(this.SpectatorReactionTypeParam, 0);
							}
							NPCDialogueHolder component3 = npcsenses.GetComponent<NPCDialogueHolder>();
							if (((component3 != null) ? component3.PersonalityProfile : null) != null)
							{
								string expressionParamName = component3.PersonalityProfile.expressionParamName;
								int neutralExpressionIndex = component3.PersonalityProfile.neutralExpressionIndex;
								if (!string.IsNullOrEmpty(expressionParamName))
								{
									componentInChildren.SetInteger(expressionParamName, neutralExpressionIndex);
								}
							}
						}
						NPCEquipmentManager component4 = npcsenses.GetComponent<NPCEquipmentManager>();
						if (component4 != null)
						{
							component4.DeactivateAllProps();
						}
					}
				}
			}
			if (!(PerpWalkController.Instance != null) || !PerpWalkController.Instance.IsActive)
			{
				AttentionUIController.GlobalForceHideAll = false;
			}
			this.ResetSpectatorFlashVolume();
			Debug.Log(string.Format("[Director] Unfrozen {0} NPCs", this._frozenNPCs.Count));
			this._frozenNPCs.Clear();
			this._spectatorNPCs.Clear();
			this._savedWanderControllerStates.Clear();
			this._savedFollowerStates.Clear();
		}

		// Token: 0x060093E4 RID: 37860 RVA: 0x003CCAC4 File Offset: 0x003CACC4
		public void ProtectDetectionCamera(CinemachineVirtualCamera camera, Transform subject)
		{
			if (!this.EnableShotProtection || !this.IsEventActive || this._detectionCameraReleased)
			{
				this.EndDetectionShotProtection();
				return;
			}
			if (camera == null || subject == null)
			{
				return;
			}
			if (this._shotProtection == null)
			{
				this._shotProtection = base.GetComponent<DetectionShotProtection>();
			}
			if (this._shotProtection == null)
			{
				this._shotProtection = base.gameObject.AddComponent<DetectionShotProtection>();
			}
			this._shotProtection.Begin(Camera.main);
			this._shotProtection.IncludeProtectedActor(this.PlayerTransform);
			this._shotProtection.IncludeActor((this._detectingNPC != null) ? this._detectingNPC.transform : null);
			EncounterRequest activeEncounterRequest = this._activeEncounterRequest;
			if (((activeEncounterRequest != null) ? activeEncounterRequest.additionalActors : null) != null)
			{
				foreach (NPCSenses npcsenses in this._activeEncounterRequest.additionalActors)
				{
					if (npcsenses != null)
					{
						this._shotProtection.IncludeActor(npcsenses.transform);
					}
				}
			}
			bool flag = subject == this.PlayerTransform && (camera == this.PlayerFaceCloseUpCamera || camera == this.PlayerFaceMediumCamera);
			this._shotProtection.RegisterCamera(camera, subject, flag);
		}

		// Token: 0x060093E5 RID: 37861 RVA: 0x003CCC34 File Offset: 0x003CAE34
		public void HideEncounterNPCsOnLayer()
		{
			if (!this.EnableShotProtection || !this.IsEventActive || this._detectionCameraReleased)
			{
				this.EndDetectionShotProtection();
				return;
			}
			Transform transform;
			if (!(this._cachedPlayerInput != null))
			{
				vThirdPersonInput vThirdPersonInput = Object.FindObjectOfType<vThirdPersonInput>();
				transform = ((vThirdPersonInput != null) ? vThirdPersonInput.transform : null);
			}
			else
			{
				transform = this._cachedPlayerInput.transform;
			}
			Transform transform2 = transform;
			this.ProtectDetectionCamera(this.PlayerFaceCloseUpCamera, transform2);
			this.ProtectDetectionCamera(this.PlayerFaceMediumCamera, transform2);
			if (this.CinematicCameras != null)
			{
				foreach (GameObject gameObject in this.CinematicCameras)
				{
					if (gameObject != null)
					{
						this.ProtectDetectionCamera(gameObject.GetComponent<CinemachineVirtualCamera>(), transform2);
					}
				}
			}
			if (this._shotProtection != null)
			{
				this._shotProtection.Begin(Camera.main);
			}
		}

		// Token: 0x060093E6 RID: 37862 RVA: 0x003CCD24 File Offset: 0x003CAF24
		public void RestoreEncounterNPCLayers()
		{
			if (this._shotProtection != null)
			{
				this._shotProtection.Suspend();
			}
		}

		// Token: 0x060093E7 RID: 37863 RVA: 0x003CCD3F File Offset: 0x003CAF3F
		private IEnumerator StartSuccessorUnderFadeRoutine()
		{
			EncounterRequest activeEncounterRequest = this._activeEncounterRequest;
			if (activeEncounterRequest != null && activeEncounterRequest.completeUnderFade)
			{
				Action onSceneEnded = activeEncounterRequest.onSceneEnded;
				activeEncounterRequest.onSceneEnded = null;
				if (onSceneEnded != null)
				{
					onSceneEnded();
				}
				yield return null;
				yield break;
			}
			bool flag;
			if (activeEncounterRequest == null)
			{
				flag = true;
			}
			else
			{
				EncounterProfileSO profile = activeEncounterRequest.profile;
				bool? flag2 = ((profile != null) ? new bool?(profile.invokeOnSceneEndedDuringFade) : null);
				bool flag3 = true;
				flag = !((flag2.GetValueOrDefault() == flag3) & (flag2 != null));
			}
			if (flag)
			{
				yield break;
			}
			this.ReleaseDetectionCamera();
			this.UnlockPlayerInput();
			Action onSceneEnded2 = activeEncounterRequest.onSceneEnded;
			activeEncounterRequest.onSceneEnded = null;
			if (onSceneEnded2 != null)
			{
				onSceneEnded2();
			}
			yield return null;
			float num = ((this.DetectionCameraCtrl != null) ? this.DetectionCameraCtrl.ActivePresetBlendDuration : 0f);
			if (num > 0f)
			{
				yield return new WaitForSecondsRealtime(num);
			}
			DetectionCameraController detectionCameraCtrl = this.DetectionCameraCtrl;
			if (detectionCameraCtrl != null)
			{
				detectionCameraCtrl.PrepareActiveCameraForReveal();
			}
			yield return null;
			yield break;
		}

		// Token: 0x060093E8 RID: 37864 RVA: 0x003CCD4E File Offset: 0x003CAF4E
		private void ReleaseDetectionCamera()
		{
			this._detectionCameraReleased = true;
			this.EndDetectionShotProtection();
		}

		// Token: 0x060093E9 RID: 37865 RVA: 0x003CCD5D File Offset: 0x003CAF5D
		private void EndDetectionShotProtection()
		{
			if (this._shotProtection != null)
			{
				this._shotProtection.EndProtection();
			}
		}

		// Token: 0x060093EA RID: 37866 RVA: 0x003CCD78 File Offset: 0x003CAF78
		private void PlayEmoteAudio(EmoteDefinitionSO emote)
		{
			if (((emote != null) ? emote.EmoteAudioClip : null) == null || this.EmoteAudioSource == null)
			{
				return;
			}
			this.EmoteAudioSource.clip = emote.EmoteAudioClip;
			this.EmoteAudioSource.volume = emote.AudioVolume;
			this.EmoteAudioSource.Play();
			Debug.Log(string.Format("[Director] Playing audio for emote '{0}' (volume: {1})", emote.DisplayName, emote.AudioVolume));
		}

		// Token: 0x060093EB RID: 37867 RVA: 0x003CCDF8 File Offset: 0x003CAFF8
		private void ActivateEmoteCameras(EmoteDefinitionSO emote)
		{
			if (this.DetectionCameraCtrl == null)
			{
				return;
			}
			DetectionCameraPresetCollectionSO detectionCameraPresetCollectionSO = ((emote != null) ? emote.DetectionCameraPresets : null) ?? this.DefaultEmoteCameraPresets;
			if (detectionCameraPresetCollectionSO == null || detectionCameraPresetCollectionSO.Count == 0)
			{
				Debug.Log("[Director] No emote camera presets configured — keeping current camera.");
				return;
			}
			Transform transform;
			if (!(this._cachedPlayerInput != null))
			{
				vThirdPersonInput vThirdPersonInput = Object.FindObjectOfType<vThirdPersonInput>();
				transform = ((vThirdPersonInput != null) ? vThirdPersonInput.transform : null);
			}
			else
			{
				transform = this._cachedPlayerInput.transform;
			}
			Transform transform2 = transform;
			if (transform2 == null)
			{
				Debug.LogWarning("[Director] Cannot activate emote cameras — player transform not found.");
				return;
			}
			Transform transform3 = ((this._detectingNPC != null) ? this._detectingNPC.transform : null);
			this.DetectionCameraCtrl.ActivateForEmotePhase(detectionCameraPresetCollectionSO, transform2, transform3);
			string text = ((((emote != null) ? emote.DetectionCameraPresets : null) != null) ? ("per-emote '" + emote.DisplayName + "'") : "default");
			Debug.Log(string.Format("[Director] Emote cameras activated ({0}, {1} presets)", text, detectionCameraPresetCollectionSO.Count));
		}

		// Token: 0x060093EC RID: 37868 RVA: 0x003CCEFF File Offset: 0x003CB0FF
		private void DeactivateEmoteCameras()
		{
			if (this.DetectionCameraCtrl != null && this.DetectionCameraCtrl.IsActive)
			{
				this.DetectionCameraCtrl.Deactivate();
				Debug.Log("[Director] Emote cameras deactivated");
			}
		}

		// Token: 0x060093ED RID: 37869 RVA: 0x003CCF34 File Offset: 0x003CB134
		private void PositionAdditionalActors()
		{
			EncounterRequest activeEncounterRequest = this._activeEncounterRequest;
			List<NPCSenses> list = ((activeEncounterRequest != null) ? activeEncounterRequest.additionalActors : null);
			if (list == null || list.Count == 0)
			{
				return;
			}
			EncounterProfileSO profile = this._activeEncounterRequest.profile;
			List<EncounterProfileSO.ActorPositionSlot> list2 = ((profile != null) ? profile.actorSlots : null);
			Vector3 vector = ((this._cachedPlayerInput != null) ? this._cachedPlayerInput.transform.position : Vector3.zero);
			for (int i = 0; i < list.Count; i++)
			{
				NPCSenses npcsenses = list[i];
				if (!(npcsenses == null))
				{
					Vector3 vector2 = ((list2 != null && i < list2.Count) ? list2[i].positionOffset : new Vector3((float)(i + 1) * 1.8f, 0f, 0.4f));
					Vector3 vector3 = this._detectingNPC.transform.position + this._detectingNPC.transform.TransformDirection(vector2);
					FollowerEntity component = npcsenses.GetComponent<FollowerEntity>();
					if (component != null && component.entityExists)
					{
						component.Teleport(vector3, true);
					}
					npcsenses.transform.position = vector3;
					if (list2 == null || i >= list2.Count || list2[i].lookAtPlayer)
					{
						Vector3 vector4 = vector - vector3;
						vector4.y = 0f;
						if (vector4.sqrMagnitude > 0.001f)
						{
							npcsenses.transform.rotation = Quaternion.LookRotation(vector4.normalized);
						}
					}
					int num = ((list2 != null && i < list2.Count) ? list2[i].upperBodyActionIndex : 0);
					if (num > 0)
					{
						AStarNPCController component2 = npcsenses.GetComponent<AStarNPCController>();
						if (component2 != null)
						{
							component2.SetAction((NPCInterestPoint.ActionType)num);
						}
					}
					FollowerEntity component3 = npcsenses.GetComponent<FollowerEntity>();
					if (component3 != null)
					{
						component3.isStopped = true;
					}
					Debug.Log(string.Format("[Director] Additional actor '{0}' positioned at {1}", npcsenses.name, vector3));
				}
			}
		}

		// Token: 0x060093EE RID: 37870 RVA: 0x003CD124 File Offset: 0x003CB324
		private void ActivateEncounterOverlay()
		{
			EncounterRequest activeEncounterRequest = this._activeEncounterRequest;
			EncounterProfileSO encounterProfileSO = ((activeEncounterRequest != null) ? activeEncounterRequest.profile : null);
			if (encounterProfileSO == null)
			{
				return;
			}
			if (encounterProfileSO.uiOverlayPrefab != null && this._overlayInstance == null)
			{
				this._overlayInstance = Object.Instantiate<GameObject>(encounterProfileSO.uiOverlayPrefab);
				Canvas[] componentsInChildren = this._overlayInstance.GetComponentsInChildren<Canvas>(true);
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					GameplayHudVisibility.RegisterCanvas(componentsInChildren[i]);
				}
				Debug.Log("[Director] Encounter overlay activated: " + encounterProfileSO.uiOverlayPrefab.name);
			}
			if (encounterProfileSO.postProcessOverride != null && this._postProcessVolume == null)
			{
				GameObject gameObject = new GameObject("EncounterPostProcess");
				this._postProcessVolume = gameObject.AddComponent<Volume>();
				this._postProcessVolume.isGlobal = true;
				this._postProcessVolume.priority = 100f;
				this._postProcessVolume.profile = encounterProfileSO.postProcessOverride;
				this._postProcessVolume.weight = encounterProfileSO.postProcessWeight;
				Debug.Log("[Director] Encounter post-process activated: " + encounterProfileSO.postProcessOverride.name);
			}
		}

		// Token: 0x060093EF RID: 37871 RVA: 0x003CD244 File Offset: 0x003CB444
		private void DeactivateEncounterOverlay()
		{
			if (this._overlayInstance != null)
			{
				Object.Destroy(this._overlayInstance);
				this._overlayInstance = null;
			}
			if (this._postProcessVolume != null)
			{
				Object.Destroy(this._postProcessVolume.gameObject);
				this._postProcessVolume = null;
			}
		}

		// Token: 0x060093F2 RID: 37874 RVA: 0x003CD4AC File Offset: 0x003CB6AC
		[CompilerGenerated]
		private bool <WaitForReactionHolds>g__AllDone|192_0()
		{
			for (int i = 0; i < this._reactionHolds.Count; i++)
			{
				Func<bool> func = this._reactionHolds[i];
				if (func != null && !func())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04009868 RID: 39016
		private const string DetectionInteractionGateReason = "DetectionFlow";

		// Token: 0x04009869 RID: 39017
		private IDisposable _encounterTrafficPause;

		// Token: 0x0400986A RID: 39018
		private bool _ownsSensoryPause;

		// Token: 0x0400986B RID: 39019
		private const PlayerActivity DetectionInteractionGateMask = PlayerActivity.Cover | PlayerActivity.Interact | PlayerActivity.Inventory | PlayerActivity.ArmsPose;

		// Token: 0x0400986D RID: 39021
		[Header("Time Config")]
		public DetectionConfigSO GlobalConfig;

		// Token: 0x0400986E RID: 39022
		[Tooltip("Delay in seconds after reaction starts before showing UI/SlowMo")]
		public float ReactionPreCinematicDelay = 2f;

		// Token: 0x0400986F RID: 39023
		[Header("Fade Settings")]
		[Tooltip("Duration of fade transitions")]
		public float FadeDuration = 0.5f;

		// Token: 0x04009870 RID: 39024
		[Header("Turn Settings")]
		[Tooltip("How fast the NPC turns to face the player (degrees/sec)")]
		public float TurnSpeed = 180f;

		// Token: 0x04009871 RID: 39025
		[Tooltip("Angle threshold to consider 'facing' the player")]
		public float FacingThreshold = 10f;

		// Token: 0x04009872 RID: 39026
		[Header("UI References")]
		public GameObject ChoiceCanvas;

		// Token: 0x04009873 RID: 39027
		public Image SkipTimerFill;

		// Token: 0x04009874 RID: 39028
		public TextMeshProUGUI SkipCountdownText;

		// Token: 0x04009875 RID: 39029
		[Header("Skip Hint")]
		[Tooltip("UI element showing the skip hint (e.g., 'Press SPACE to skip')")]
		public GameObject SkipHintUI;

		// Token: 0x04009876 RID: 39030
		[Tooltip("Total duration of the detection sequence (time available for choice)")]
		public float SkipHintDuration = 10f;

		// Token: 0x04009877 RID: 39031
		[Header("Input Config")]
		public GenericInput SkipInput = new GenericInput("Space", "A", "A");

		// Token: 0x04009878 RID: 39032
		[Header("Emote Hint")]
		[FormerlySerializedAs("EmoteSkipHintUI")]
		[FormerlySerializedAs("EndEmoteHintUI")]
		[Tooltip("UI shown during emote playback while in normal mode (hidden during FlyMode)")]
		public GameObject EmoteHintUI;

		// Token: 0x04009879 RID: 39033
		[Header("Flying Camera")]
		[Tooltip("Reference to the DetectionFlyingCamera component in the scene")]
		public DetectionFlyingCamera FlyingCamera;

		// Token: 0x0400987A RID: 39034
		[Tooltip("If true, the player can enter flying camera mode after emote playback")]
		public bool EnableFlyingCamera;

		// Token: 0x0400987B RID: 39035
		[Tooltip("Input used to enter flying camera mode when prompted")]
		public GenericInput FlyingCameraEnterInput = new GenericInput("K", "Y", "Y");

		// Token: 0x0400987C RID: 39036
		[Header("Detection Camera System")]
		[Tooltip("Reference to the DetectionCameraController in the scene")]
		public DetectionCameraController DetectionCameraCtrl;

		// Token: 0x0400987D RID: 39037
		[Tooltip("Camera presets used during NPC reaction phase (Phase 8-11). First preset is the default NPC camera.")]
		public DetectionCameraPresetCollectionSO NpcPhaseCameraPresets;

		// Token: 0x0400987E RID: 39038
		[Tooltip("Default camera presets used during emote playback phase (Phase after choice). Overridden by per-emote DetectionCameraPresets if configured.")]
		public DetectionCameraPresetCollectionSO DefaultEmoteCameraPresets;

		// Token: 0x0400987F RID: 39039
		[Header("Cinemachine Camera (Legacy)")]
		[Tooltip("Legacy: List of virtual cameras for cinematic shots. Superseded by DetectionCameraController.")]
		public List<GameObject> CinematicCameras = new List<GameObject>();

		// Token: 0x04009880 RID: 39040
		[Tooltip("Legacy: If true, randomly select a camera from the list")]
		public bool RandomizeCameraSelection = true;

		// Token: 0x04009881 RID: 39041
		[Tooltip("Blend time for camera transitions")]
		public float CameraBlendTime = 0.5f;

		// Token: 0x04009882 RID: 39042
		[Header("Choice Configuration")]
		public List<ExhibitionEventDirector.ChoiceActionConfig> AvailableChoices = new List<ExhibitionEventDirector.ChoiceActionConfig>();

		// Token: 0x04009883 RID: 39043
		[Tooltip("Root GameObject to activate/deactivate for showing choice UI")]
		public GameObject ChoiceUIRoot;

		// Token: 0x04009884 RID: 39044
		[Header("Emote Wheel")]
		[Tooltip("Reference to the EmoteWheelUI component in the scene")]
		public EmoteWheelUI EmoteWheel;

		// Token: 0x04009885 RID: 39045
		[Tooltip("Collection of emotes available per category")]
		public EmoteCollectionSO EmoteCollection;

		// Token: 0x04009886 RID: 39046
		[Header("NPC Cameras")]
		[Tooltip("Virtual camera focused on NPC for reaction phase")]
		public CinemachineVirtualCamera NPCReactionCamera;

		// Token: 0x04009887 RID: 39047
		[Header("Police Capture Spacing")]
		[Tooltip("Preferred distance from Ciel when the capturing officer starts too close. A short retreat is attempted during the black screen only when navigation and physical space are clear. Set to 0 to disable.")]
		[Min(0f)]
		public float PoliceCaptureStandOffDistance = 1.5f;

		// Token: 0x04009888 RID: 39048
		[Header("Player Reaction Cameras")]
		[Tooltip("Virtual camera for close-up on player's face (first shot after fade)")]
		public CinemachineVirtualCamera PlayerFaceCloseUpCamera;

		// Token: 0x04009889 RID: 39049
		[Tooltip("Virtual camera for medium shot of player's face (pulls back from close-up)")]
		public CinemachineVirtualCamera PlayerFaceMediumCamera;

		// Token: 0x0400988A RID: 39050
		[Tooltip("Duration of the camera transition from close-up to medium shot")]
		public float PlayerCameraTransitionDuration = 2.5f;

		// Token: 0x0400988B RID: 39051
		[Header("Player Reaction UI")]
		[Tooltip("Canvas to show during player reaction (e.g., with animated GIF/effect)")]
		public GameObject PlayerReactionCanvas;

		// Token: 0x0400988C RID: 39052
		[Tooltip("If true, the reaction canvas will be shown during the entire player reaction sequence")]
		public bool ShowReactionCanvasDuringSequence = true;

		// Token: 0x0400988D RID: 39053
		[Header("Dialogue")]
		[Tooltip("If true, uses dialogue system during detection")]
		public bool UseDialogue = true;

		// Token: 0x0400988E RID: 39054
		[Tooltip("Reading time per Exit line after its typewriter finishes (0 = manual advance only)")]
		public float ExitDialogueAutoDismissTime = 5f;

		// Token: 0x0400988F RID: 39055
		[Header("Spectator NPCs")]
		[Tooltip("Radius around the player where NPCs will react as spectators during detection")]
		public float SpectatorRadius = 15f;

		// Token: 0x04009890 RID: 39056
		[Tooltip("If true, all spawned NPCs will be frozen during detection (not just spectators)")]
		public bool FreezeAllNPCsDuringDetection = true;

		// Token: 0x04009891 RID: 39057
		[Tooltip("If true, spectator NPCs will turn to face the player during detection")]
		public bool SpectatorsFacePlayer = true;

		// Token: 0x04009892 RID: 39058
		[Tooltip("Animator trigger for spectator reaction (e.g., 'ReactionTrigger')")]
		public string SpectatorReactionTrigger = "ReactionTrigger";

		// Token: 0x04009893 RID: 39059
		[Tooltip("Animator int parameter for spectator reaction type (e.g., 'ReactionType')")]
		public string SpectatorReactionTypeParam = "ReactionType";

		// Token: 0x04009894 RID: 39060
		[Tooltip("Default reaction type index for spectators (0 = LookAt)")]
		public int SpectatorDefaultReactionIndex;

		// Token: 0x04009895 RID: 39061
		[Header("Spectator Audio")]
		[Tooltip("Volume multiplier for spectator flash sounds during detection (0 = muted, 1 = normal)")]
		[Range(0f, 1f)]
		public float SpectatorFlashVolumeMultiplier = 0.15f;

		// Token: 0x04009896 RID: 39062
		[Header("Detection Shot Protection")]
		[Tooltip("Adapts detection shots around nearby obstacles and hides NPCs only while they block the subject.")]
		public bool EnableShotProtection = true;

		// Token: 0x04009897 RID: 39063
		[HideInInspector]
		public string npcDetectionHiddenLayerName = "NPCDetectionHidden";

		// Token: 0x04009898 RID: 39064
		[Header("Events")]
		public UnityEvent<NPCSenses> OnDetectionStarted;

		// Token: 0x04009899 RID: 39065
		public UnityEvent OnTimeSlowed;

		// Token: 0x0400989A RID: 39066
		public UnityEvent OnTimeResumed;

		// Token: 0x0400989B RID: 39067
		public UnityEvent<PlayerActionEntry> OnPlayerActionTriggered;

		// Token: 0x0400989C RID: 39068
		public UnityEvent OnEventSkipped;

		// Token: 0x0400989D RID: 39069
		public UnityEvent OnSkipHintShown;

		// Token: 0x0400989E RID: 39070
		public UnityEvent OnChoicePanelShown;

		// Token: 0x0400989F RID: 39071
		public UnityEvent<NPCSenses> OnCooldownEnded;

		// Token: 0x040098A0 RID: 39072
		public UnityEvent OnSequenceComplete;

		// Token: 0x040098A6 RID: 39078
		private bool _reactionHandledExternally;

		// Token: 0x040098A7 RID: 39079
		private readonly List<Func<bool>> _reactionHolds = new List<Func<bool>>();

		// Token: 0x040098A8 RID: 39080
		[Tooltip("Timeout di sicurezza (s) per l'attesa degli hold di reaction esterni: se un listener non sblocca mai il suo hold, la sequenza prosegue comunque dopo questo tempo. 0 o negativo = nessun timeout (attesa indefinita, sconsigliato).")]
		public float ReactionHoldTimeout = 20f;

		// Token: 0x040098A9 RID: 39081
		[Header("Newspaper Capture")]
		[Tooltip("Ritardo (s) dopo l'avvio dell'emote prima di catturare la foto per il giornale, cosi' la posa e' formata. Usato solo se il profilo ha captureNewspaperPhoto = true.")]
		[Min(0f)]
		public float newspaperCaptureDelay = 0.25f;

		// Token: 0x040098AA RID: 39082
		private bool _isEventActive;

		// Token: 0x040098AD RID: 39085
		private bool _isInChoicePhase;

		// Token: 0x040098AE RID: 39086
		private bool _encounterResolutionStarted;

		// Token: 0x040098AF RID: 39087
		private bool _encounterSkipped;

		// Token: 0x040098B0 RID: 39088
		private NPCSenses _detectingNPC;

		// Token: 0x040098B1 RID: 39089
		private NPCSeatedEncounter _seatedEncounter;

		// Token: 0x040098B2 RID: 39090
		private StaticPhoneNPC _phoneEncounter;

		// Token: 0x040098B3 RID: 39091
		private vThirdPersonInput _cachedPlayerInput;

		// Token: 0x040098B4 RID: 39092
		private vThirdPersonController _lockedPlayerController;

		// Token: 0x040098B5 RID: 39093
		private bool _ownsPlayerInputLock;

		// Token: 0x040098B6 RID: 39094
		private bool _savedPlayerInputLock;

		// Token: 0x040098B7 RID: 39095
		private bool _savedPlayerMoveInputLock;

		// Token: 0x040098B8 RID: 39096
		private bool _savedPlayerMoveDirectionLock;

		// Token: 0x040098B9 RID: 39097
		private bool _savedPlayerMovementLock;

		// Token: 0x040098BA RID: 39098
		private bool _savedPlayerRotationLock;

		// Token: 0x040098BB RID: 39099
		private PlayerActionAnimator _playerAnimator;

		// Token: 0x040098BC RID: 39100
		private List<NPCSenses> _cooldownList = new List<NPCSenses>();

		// Token: 0x040098BD RID: 39101
		private Coroutine _sequenceCoroutine;

		// Token: 0x040098BE RID: 39102
		private Coroutine _concurrentDialogueCoroutine;

		// Token: 0x040098BF RID: 39103
		private readonly List<Coroutine> _encounterPresentationCoroutines = new List<Coroutine>();

		// Token: 0x040098C0 RID: 39104
		private bool _preReactionStarted;

		// Token: 0x040098C1 RID: 39105
		private CinemachineBrain _preReactionBrain;

		// Token: 0x040098C2 RID: 39106
		private CinemachineBlendDefinition _preReactionSavedBlend;

		// Token: 0x040098C3 RID: 39107
		private bool _preReactionHasSavedBlend;

		// Token: 0x040098C4 RID: 39108
		private SimpleDialogueManager _encounterDialogueManager;

		// Token: 0x040098C5 RID: 39109
		private object _encounterDialogueToken;

		// Token: 0x040098C6 RID: 39110
		private ExhibitionEventDirector.ExitPresentation _pendingExitPresentation;

		// Token: 0x040098C7 RID: 39111
		private EncounterRequest _activeEncounterRequest;

		// Token: 0x040098C8 RID: 39112
		private DialogueResolver.Session _encounterDialogueSelection;

		// Token: 0x040098C9 RID: 39113
		private EncounterRequest _pendingRecoveryEncounter;

		// Token: 0x040098CA RID: 39114
		private NPCSenses _pendingRecoveryDetection;

		// Token: 0x040098CB RID: 39115
		private float _nextPoolRecoveryRetry;

		// Token: 0x040098CC RID: 39116
		private GameObject _overlayInstance;

		// Token: 0x040098CD RID: 39117
		private Volume _postProcessVolume;

		// Token: 0x040098CE RID: 39118
		private PlayerActionEntry _selectedAction;

		// Token: 0x040098CF RID: 39119
		private EmoteDefinitionSO _selectedEmote;

		// Token: 0x040098D0 RID: 39120
		private BoobTouchModeController _gropingInterludeOwner;

		// Token: 0x040098D1 RID: 39121
		private ChoiceCategory? _selectedCategory;

		// Token: 0x040098D2 RID: 39122
		private bool _isInEmoteSelectionPhase;

		// Token: 0x040098D3 RID: 39123
		private bool _waitingForEmotePhoto;

		// Token: 0x040098D4 RID: 39124
		private Dictionary<int, float> _savedAnimatorLayerWeights = new Dictionary<int, float>();

		// Token: 0x040098D5 RID: 39125
		private List<NPCSenses> _frozenNPCs = new List<NPCSenses>();

		// Token: 0x040098D6 RID: 39126
		private DetectionShotProtection _shotProtection;

		// Token: 0x040098D7 RID: 39127
		private bool _detectionCameraReleased;

		// Token: 0x040098D8 RID: 39128
		private DetectionChoiceCameraAnchor _choiceCameraAnchor;

		// Token: 0x040098D9 RID: 39129
		private List<NPCSenses> _spectatorNPCs = new List<NPCSenses>();

		// Token: 0x040098DA RID: 39130
		private Dictionary<NPCSenses, bool> _savedWanderControllerStates = new Dictionary<NPCSenses, bool>();

		// Token: 0x040098DB RID: 39131
		private Dictionary<NPCSenses, bool> _savedFollowerStates = new Dictionary<NPCSenses, bool>();

		// Token: 0x040098DC RID: 39132
		private CinemachineBrain _mainCameraBrain;

		// Token: 0x040098DD RID: 39133
		private Vector3 _savedCameraLocalPosition;

		// Token: 0x040098DE RID: 39134
		private Quaternion _savedCameraLocalRotation;

		// Token: 0x040098DF RID: 39135
		private bool _hasSavedCameraState;

		// Token: 0x040098E0 RID: 39136
		private CinemachineVirtualCamera _anchorCamera;

		// Token: 0x040098E1 RID: 39137
		[Header("Emote System Integration")]
		[Tooltip("Reference to the EmotePropHandler on the player (auto-found if null)")]
		public EmotePropHandler PlayerEmotePropHandler;

		// Token: 0x040098E2 RID: 39138
		[Tooltip("AudioSource for playing emote sounds (auto-found if null)")]
		public AudioSource EmoteAudioSource;

		// Token: 0x040098E3 RID: 39139
		[Header("Emotion Level Changes")]
		[Tooltip("Amount to increase Exhibition level when player chooses Exhibit action")]
		public float ExhibitLevelIncrease = 10f;

		// Token: 0x040098E4 RID: 39140
		[Tooltip("Amount to decrease Shame level when player chooses Exhibit action")]
		public float ExhibitShameDecrease = 5f;

		// Token: 0x040098E5 RID: 39141
		[Tooltip("Amount to increase Shame level when player chooses Shame action")]
		public float ShameLevelIncrease = 5f;

		// Token: 0x040098E6 RID: 39142
		[Tooltip("Amount to decrease Exhibition level when player chooses Shame action")]
		public float ShameExhibitDecrease = 2f;

		// Token: 0x040098E7 RID: 39143
		[Header("Camera Positioning")]
		[Tooltip("Transform to use as LookAt target for NPC (e.g., Head bone). If null, uses NPC transform.")]
		public string NPCLookAtBoneName = "Head";

		// Token: 0x040098E8 RID: 39144
		[Tooltip("Transform to use as LookAt target for Player (e.g., Head bone). If null, uses Player transform.")]
		public string PlayerLookAtBoneName = "Head";

		// Token: 0x040098E9 RID: 39145
		private static readonly int ReactionIdleHash = Animator.StringToHash("ReactionIdle");

		// Token: 0x040098EA RID: 39146
		private static readonly int ReactionTypeHash = Animator.StringToHash("ReactionType");

		// Token: 0x040098EB RID: 39147
		private static readonly int ReactionTriggerHash = Animator.StringToHash("ReactionTrigger");

		// Token: 0x040098EC RID: 39148
		private List<GameObject> _activeProps = new List<GameObject>();

		// Token: 0x040098ED RID: 39149
		private bool _wasNavAgentActive;

		// Token: 0x040098EE RID: 39150
		private GameObject _activeCinematicCamera;

		// Token: 0x040098EF RID: 39151
		private int _lastCameraIndex = -1;

		// Token: 0x02001292 RID: 4754
		private sealed class ExitPresentation
		{
			// Token: 0x040098F0 RID: 39152
			public SimpleDialogueManager DialogueManager;

			// Token: 0x040098F1 RID: 39153
			public object DialogueToken;

			// Token: 0x040098F2 RID: 39154
			public HUDManager Hud;

			// Token: 0x040098F3 RID: 39155
			public bool IsTerminal;
		}

		// Token: 0x02001293 RID: 4755
		[Serializable]
		public class ChoiceActionConfig
		{
			// Token: 0x040098F4 RID: 39156
			public string Label;

			// Token: 0x040098F5 RID: 39157
			public ChoiceCategory Category;

			// Token: 0x040098F6 RID: 39158
			[Tooltip("Legacy keyboard alias. Runtime instructions now format the current Input binding for each device.")]
			public string DisplayLabel;

			// Token: 0x040098F7 RID: 39159
			public GenericInput Input = new GenericInput("Q", "X", "X");

			// Token: 0x040098F8 RID: 39160
			[Range(0f, 2f)]
			public float HoldDuration;

			// Token: 0x040098F9 RID: 39161
			[Tooltip("Action ID for this choice (e.g. 'Shame', 'Exhibit'). Used for legacy paths.")]
			public string PlayerActionId;

			// Token: 0x040098FA RID: 39162
			[Tooltip("Reference to the fixed ChoiceButtonUI in the scene")]
			public ChoiceButtonUI ButtonUI;
		}

		// Token: 0x02001294 RID: 4756
		private enum DialoguePhase
		{
			// Token: 0x040098FC RID: 39164
			Reaction,
			// Token: 0x040098FD RID: 39165
			PostShame,
			// Token: 0x040098FE RID: 39166
			PostExhibit,
			// Token: 0x040098FF RID: 39167
			Exit
		}
	}
}
