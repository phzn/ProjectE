using System;
using System.Collections;
using System.Collections.Generic;
using BoZo.CharacterCreator.MC.StyleSystem;
using Dinopunch.LivingMud;
using Features.DebugTools;
using Features.Detection;
using Features.Inventory;
using Features.Rank;
using Features.Score;
using Features.TimeFreeze;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using MagicaCloth2;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.FirstPerson;
using ProjectExhibitionist.Interactions;
using UnityEngine;
using UnityEngine.Events;

namespace Features.TrashcanLoot
{
	// Token: 0x02000BFA RID: 3066
	[DisallowMultipleComponent]
	public class WasteCanHideInteractable : MonoBehaviour
	{
		// Token: 0x06005857 RID: 22615 RVA: 0x002289E5 File Offset: 0x00226BE5
		internal Object ReadUniversityGameplayReference(UniversityGameplayField field)
		{
			if (field == UniversityGameplayField.WasteCanHideInteractable_hiddenHudConfig)
			{
				return this.hiddenHudConfig;
			}
			throw new InvalidOperationException("Unsupported WasteCanHideInteractable gameplay binding: " + field.ToString());
		}

		// Token: 0x06005858 RID: 22616 RVA: 0x00228A0F File Offset: 0x00226C0F
		internal void WriteUniversityGameplayReference(UniversityGameplayField field, Object asset)
		{
			if (field == UniversityGameplayField.WasteCanHideInteractable_hiddenHudConfig)
			{
				this.hiddenHudConfig = (InteractionConfigSO)asset;
				return;
			}
			throw new InvalidOperationException("Unsupported WasteCanHideInteractable gameplay binding: " + field.ToString());
		}

		// Token: 0x17000E9C RID: 3740
		// (get) Token: 0x06005859 RID: 22617 RVA: 0x00228A3F File Offset: 0x00226C3F
		public bool IsBusy
		{
			get
			{
				return this._state > WasteCanHideInteractable.HideState.None;
			}
		}

		// Token: 0x0600585A RID: 22618 RVA: 0x00228A4C File Offset: 0x00226C4C
		public bool InterruptForEncounter()
		{
			if (!this.IsBusy)
			{
				return false;
			}
			if (this._controller != null)
			{
				this._controller.transform.SetPositionAndRotation(this._entryPosition, this._entryRotation);
				if (this._controller._rigidbody != null)
				{
					this._controller._rigidbody.position = this._entryPosition;
					this._controller._rigidbody.rotation = this._entryRotation;
				}
			}
			this.ForceAbort();
			return true;
		}

		// Token: 0x17000E9D RID: 3741
		// (get) Token: 0x0600585B RID: 22619 RVA: 0x00228AD3 File Offset: 0x00226CD3
		public bool IsPlayerInRange
		{
			get
			{
				return this._playerInRange;
			}
		}

		// Token: 0x0600585C RID: 22620 RVA: 0x00228ADB File Offset: 0x00226CDB
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStaticState()
		{
			WasteCanHideInteractable.InRangeCans.Clear();
			WasteCanHideInteractable._sharedPlayerGenericAction = null;
		}

		// Token: 0x0600585D RID: 22621 RVA: 0x00228AF0 File Offset: 0x00226CF0
		private void Awake()
		{
			ControllerContextBindings.SetLabel(this.hiddenHudConfig, "F", "B", false);
			ControllerContextBindings.SetLabel(this.hiddenHudConfig, "G", "Y", false);
			ControllerContextBindings.SetLabel(this.hiddenHudConfig, "H", "R3", false);
			ControllerContextBindings.Set(this.exitInput, "B");
			ControllerContextBindings.Set(this.xrayInput, "RightStickClick");
			ControllerContextBindings.Set(this.switchCameraInput, "Y");
			this.CacheEventCallbacks();
			if (this.rangeTriggers == null || this.rangeTriggers.Length == 0)
			{
				this.rangeTriggers = base.GetComponentsInChildren<vTriggerGenericAction>(true);
			}
			if (this.promptUI == null)
			{
				this.promptUI = base.GetComponentInChildren<InteractablePromptUI>(true);
			}
			if (this.hideAnchor == null)
			{
				foreach (Transform transform in base.GetComponentsInChildren<Transform>(true))
				{
					if (transform.name == "RummageAnchor")
					{
						this.hideAnchor = transform;
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
			if (this.hairContainmentRoot == null)
			{
				foreach (Transform transform3 in base.GetComponentsInChildren<Transform>(true))
				{
					if (transform3.name == "HairContainmentColliders")
					{
						this.hairContainmentRoot = transform3;
						break;
					}
				}
			}
			if (this.hideCamera == null)
			{
				foreach (Camera camera in base.GetComponentsInChildren<Camera>(true))
				{
					if (camera.name == "WasteCanHideCamera")
					{
						this.hideCamera = camera;
						break;
					}
				}
			}
			this._lootSibling = base.GetComponent<TrashcanLootInteractable>();
			if (this.rangeTriggers == null || this.rangeTriggers.Length == 0)
			{
				Debug.LogError("[WasteCanHide] No vTriggerGenericAction under '" + base.name + "' for range detection.", this);
			}
			if (this.hideAnchor == null)
			{
				Debug.LogError("[WasteCanHide] No 'RummageAnchor' child found on '" + base.name + "'.", this);
			}
		}

		// Token: 0x0600585E RID: 22622 RVA: 0x00228D2A File Offset: 0x00226F2A
		private void OnEnable()
		{
			this._playerInRange = false;
			this._activePlayer = null;
			this._playerGenericAction = null;
			WasteCanHideInteractable.InRangeCans.Remove(this);
			this.SubscribeRangeEvents();
			this._acceptRangeEvents = true;
		}

		// Token: 0x0600585F RID: 22623 RVA: 0x00228D5A File Offset: 0x00226F5A
		private void CacheEventCallbacks()
		{
			if (this._validateCallback == null)
			{
				this._validateCallback = new UnityAction<GameObject>(this.HandleValidate);
			}
			if (this._invalidateCallback == null)
			{
				this._invalidateCallback = new UnityAction<GameObject>(this.HandleInvalidate);
			}
		}

		// Token: 0x06005860 RID: 22624 RVA: 0x00228D90 File Offset: 0x00226F90
		private void SubscribeRangeEvents()
		{
			if (this._rangeEventsSubscribed || this.rangeTriggers == null)
			{
				return;
			}
			this.CacheEventCallbacks();
			for (int i = 0; i < this.rangeTriggers.Length; i++)
			{
				vTriggerGenericAction vTriggerGenericAction = this.rangeTriggers[i];
				if (!(vTriggerGenericAction == null))
				{
					vTriggerGenericAction.OnValidate.AddListener(this._validateCallback);
					vTriggerGenericAction.OnInvalidate.AddListener(this._invalidateCallback);
					vTriggerGenericAction.OnPlayerExit.AddListener(this._invalidateCallback);
				}
			}
			this._rangeEventsSubscribed = true;
		}

		// Token: 0x06005861 RID: 22625 RVA: 0x00228E14 File Offset: 0x00227014
		private void OnDisable()
		{
			this._acceptRangeEvents = false;
			this._playerInRange = false;
			WasteCanHideInteractable.InRangeCans.Remove(this);
			if (this._state != WasteCanHideInteractable.HideState.None)
			{
				this.ForceAbort();
			}
			else
			{
				if (this._containedHairCloths.Count != 0)
				{
					this.RemoveHairContainment(false);
				}
				if (this._hairBlendCo != null || this._hairBlendTargets.Count != 0 || this._hairStyleService != null)
				{
					this.RestoreHairBlendImmediately();
				}
				if (this._hairPhysicsRestoreCo != null || this._exitDisabledHairCloths.Count != 0)
				{
					this.RestoreExitHairPhysicsImmediately();
				}
				FullBodyHideActivityContext.Release(ref this._hideLease);
			}
			this._activePlayer = null;
			this._playerGenericAction = null;
		}

		// Token: 0x06005862 RID: 22626 RVA: 0x00228EBC File Offset: 0x002270BC
		private void OnDestroy()
		{
			this._acceptRangeEvents = false;
			WasteCanHideInteractable.InRangeCans.Remove(this);
			this.UnsubscribeRangeEvents();
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
		}

		// Token: 0x06005863 RID: 22627 RVA: 0x00228F18 File Offset: 0x00227118
		private void UnsubscribeRangeEvents()
		{
			if (!this._rangeEventsSubscribed)
			{
				return;
			}
			this._rangeEventsSubscribed = false;
			if (this.rangeTriggers == null)
			{
				return;
			}
			this.CacheEventCallbacks();
			for (int i = 0; i < this.rangeTriggers.Length; i++)
			{
				vTriggerGenericAction vTriggerGenericAction = this.rangeTriggers[i];
				if (!(vTriggerGenericAction == null))
				{
					vTriggerGenericAction.OnValidate.RemoveListener(this._validateCallback);
					vTriggerGenericAction.OnInvalidate.RemoveListener(this._invalidateCallback);
					vTriggerGenericAction.OnPlayerExit.RemoveListener(this._invalidateCallback);
				}
			}
		}

		// Token: 0x06005864 RID: 22628 RVA: 0x00228F9C File Offset: 0x0022719C
		private void Update()
		{
			if (FlyCamera.IsActive)
			{
				return;
			}
			if (this._state == WasteCanHideInteractable.HideState.None)
			{
				this.SynchronizeRangeState();
			}
			WasteCanHideInteractable.HideState state = this._state;
			if (state != WasteCanHideInteractable.HideState.None)
			{
				if (state != WasteCanHideInteractable.HideState.Hidden)
				{
					return;
				}
				if (!PlayerActivityGate.IsResourceOwned(PlayerContextResource.ModalUI))
				{
					if (!FullBodyHideActivityContext.IsUsingTripodCamera(this._hideLease) && this.switchCameraInput != null && this.switchCameraInput.GetButtonDown() && InteractionCandidateRegistry.TryConsumeContextInput(this.switchCameraInput))
					{
						this.SetHideCamera(!this._hideCamActive);
					}
					if (this.xrayInput != null && this.xrayInput.GetButtonDown() && InteractionCandidateRegistry.TryConsumeContextInput(this.xrayInput))
					{
						this.SetXray(!this._xrayOn, false);
					}
					if (Time.time - this._hiddenSince < this.exitInputGrace)
					{
						return;
					}
					if (this.exitInput != null && this.exitInput.GetButtonDown() && InteractionCandidateRegistry.TryConsumeContextInput(this.exitInput))
					{
						this.BeginExit();
					}
				}
			}
			else if (!TimeFreezeController.OwnsContextInput && (!(this._lootSibling != null) || !this._lootSibling.IsRummaging) && this._playerInRange && this.hideInput != null && this.hideInput.GetButtonDown() && this.IsBestCandidate())
			{
				this.TryBeginHide();
				return;
			}
		}

		// Token: 0x06005865 RID: 22629 RVA: 0x002290E8 File Offset: 0x002272E8
		private void HandleValidate(GameObject player)
		{
			if (!this._acceptRangeEvents)
			{
				return;
			}
			this._playerInRange = true;
			this._activePlayer = player;
			if (this._playerGenericAction == null && player != null)
			{
				this._playerGenericAction = player.GetComponentInChildren<vGenericAction>();
				if (this._playerGenericAction != null)
				{
					WasteCanHideInteractable._sharedPlayerGenericAction = this._playerGenericAction;
				}
			}
			if (!WasteCanHideInteractable.InRangeCans.Contains(this))
			{
				WasteCanHideInteractable.InRangeCans.Add(this);
			}
		}

		// Token: 0x06005866 RID: 22630 RVA: 0x00229160 File Offset: 0x00227360
		private void HandleInvalidate(GameObject player)
		{
			if (!this._acceptRangeEvents)
			{
				return;
			}
			this._playerInRange = false;
			this._activePlayer = null;
			WasteCanHideInteractable.InRangeCans.Remove(this);
		}

		// Token: 0x06005867 RID: 22631 RVA: 0x00229188 File Offset: 0x00227388
		private void SynchronizeRangeState()
		{
			vGenericAction vGenericAction = this._playerGenericAction;
			if (vGenericAction == null)
			{
				vGenericAction = WasteCanHideInteractable.ResolveSharedPlayerGenericAction();
			}
			vTriggerGenericAction vTriggerGenericAction = ((vGenericAction != null) ? vGenericAction.triggerAction : null);
			if (!(vTriggerGenericAction != null) || !vTriggerGenericAction.enabled || !vTriggerGenericAction.gameObject.activeInHierarchy || !this.IsOwnedTrigger(vTriggerGenericAction))
			{
				if (this._playerInRange)
				{
					this.HandleInvalidate(this._activePlayer);
				}
				return;
			}
			this._playerGenericAction = vGenericAction;
			GameObject gameObject = vGenericAction.gameObject;
			if (!this._playerInRange || this._activePlayer != gameObject)
			{
				this.HandleValidate(gameObject);
				return;
			}
			if (!WasteCanHideInteractable.InRangeCans.Contains(this))
			{
				WasteCanHideInteractable.InRangeCans.Add(this);
			}
		}

		// Token: 0x06005868 RID: 22632 RVA: 0x00229244 File Offset: 0x00227444
		private static vGenericAction ResolveSharedPlayerGenericAction()
		{
			if (WasteCanHideInteractable._sharedPlayerGenericAction != null)
			{
				return WasteCanHideInteractable._sharedPlayerGenericAction;
			}
			Transform player = PlayerRegistry.Player;
			if (player != null)
			{
				WasteCanHideInteractable._sharedPlayerGenericAction = player.GetComponentInChildren<vGenericAction>();
			}
			return WasteCanHideInteractable._sharedPlayerGenericAction;
		}

		// Token: 0x06005869 RID: 22633 RVA: 0x00229284 File Offset: 0x00227484
		public static bool ReservesContextInput(GenericInput input)
		{
			string text = InteractionCandidateRegistry.ActiveBinding(input);
			if (string.IsNullOrEmpty(text) || TimeFreezeController.OwnsContextInput)
			{
				return false;
			}
			vGenericAction vGenericAction = WasteCanHideInteractable.ResolveSharedPlayerGenericAction();
			vTriggerGenericAction vTriggerGenericAction = ((vGenericAction != null) ? vGenericAction.triggerAction : null);
			if (vTriggerGenericAction == null || !vTriggerGenericAction.enabled || !vTriggerGenericAction.gameObject.activeInHierarchy)
			{
				return false;
			}
			WasteCanHideInteractable componentInParent = vTriggerGenericAction.GetComponentInParent<WasteCanHideInteractable>();
			if (componentInParent == null || !componentInParent.isActiveAndEnabled || !componentInParent.IsOwnedTrigger(vTriggerGenericAction) || !string.Equals(text, InteractionCandidateRegistry.ActiveBinding(componentInParent.hideInput), StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			componentInParent.SynchronizeRangeState();
			return componentInParent._state == WasteCanHideInteractable.HideState.None && componentInParent._playerInRange && componentInParent.IsBestCandidate();
		}

		// Token: 0x0600586A RID: 22634 RVA: 0x00229338 File Offset: 0x00227538
		private bool IsOwnedTrigger(vTriggerGenericAction trigger)
		{
			if (trigger == null || this.rangeTriggers == null)
			{
				return false;
			}
			for (int i = 0; i < this.rangeTriggers.Length; i++)
			{
				if (this.rangeTriggers[i] == trigger)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600586B RID: 22635 RVA: 0x00229380 File Offset: 0x00227580
		private bool IsBestCandidate()
		{
			Transform transform = this.ResolvePlayerTransform();
			if (transform == null)
			{
				return false;
			}
			float num = this.DistanceToPlayer(transform);
			if (num > this.maxStartDistance)
			{
				return false;
			}
			foreach (WasteCanHideInteractable wasteCanHideInteractable in WasteCanHideInteractable.InRangeCans)
			{
				if (!(wasteCanHideInteractable == null) && !(wasteCanHideInteractable == this) && wasteCanHideInteractable.isActiveAndEnabled && wasteCanHideInteractable.DistanceToPlayer(transform) < num)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600586C RID: 22636 RVA: 0x00229420 File Offset: 0x00227620
		private float DistanceToPlayer(Transform player)
		{
			Vector3 vector = ((this.hideAnchor != null) ? this.hideAnchor.position : base.transform.position);
			return Vector3.Distance(player.position, vector);
		}

		// Token: 0x0600586D RID: 22637 RVA: 0x00229460 File Offset: 0x00227660
		private void TryBeginHide()
		{
			if (FlyCamera.IsActive)
			{
				return;
			}
			if (TimeFreezeController.OwnsContextInput)
			{
				return;
			}
			InteractionCandidateRegistry.MarkContextInputConsumed(this.hideInput);
			if (this._lootSibling != null && this._lootSibling.IsRummaging)
			{
				return;
			}
			string text;
			if (this.TryGetBusyWarning(out text))
			{
				BlockedActionFeedback.Show(text);
				return;
			}
			if (!this.CanStartHide())
			{
				return;
			}
			if (!this.TryAcquireHideContext())
			{
				BlockedActionFeedback.Show("Stop what you're doing first");
				return;
			}
			this.BeginHide();
		}

		// Token: 0x0600586E RID: 22638 RVA: 0x002294D8 File Offset: 0x002276D8
		private bool TryAcquireHideContext()
		{
			return FullBodyHideActivityContext.TryAcquire("WasteCanHide#" + base.GetInstanceID().ToString(), this, ref this._hideLease, PlayerActivity.AllPlayerActions, true, true);
		}

		// Token: 0x0600586F RID: 22639 RVA: 0x00229510 File Offset: 0x00227710
		private bool CanStartHide()
		{
			this.ResolvePlayerRefs();
			if (this._controller == null || this._playerAnimator == null)
			{
				return false;
			}
			if (this._controller.customAction || this._controller.isJumping || this._controller.ragdolled)
			{
				return false;
			}
			if (!this._controller.isGrounded)
			{
				return false;
			}
			vGenericAction vGenericAction = ((this._actionAnimator != null) ? this._actionAnimator.GenericAction : null);
			return (!(vGenericAction != null) || !vGenericAction.doingAction) && (!(this._lootSibling != null) || !this._lootSibling.IsRummaging) && this.hideAnchor != null;
		}

		// Token: 0x06005870 RID: 22640 RVA: 0x002295D4 File Offset: 0x002277D4
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

		// Token: 0x06005871 RID: 22641 RVA: 0x0022960C File Offset: 0x0022780C
		private void BeginHide()
		{
			this._entryPosition = this._controller.transform.position;
			this._entryRotation = this._controller.transform.rotation;
			this.RestoreHairBlendImmediately();
			this.RestoreExitHairPhysicsImmediately();
			this._state = WasteCanHideInteractable.HideState.Entering;
			this.PrepareHairBlendTargets();
			this._playerAnimator.ResetTrigger("WasteCanHideEnding");
			this.SetPlayerPhysicsDisabled(true);
			this.LockControls(true);
			if (this._fpController != null && !FullBodyHideActivityContext.IsUsingTripodCamera(this._hideLease))
			{
				this._fpController.BeginInteractionView(this, true, 80f, true, false, null, null, null);
				this._interactionViewPushed = true;
			}
			if (this._actionAnimator != null)
			{
				this._actionAnimator.DisableEmotionLayers();
				this._emotionPushed = true;
				if (this.useRootMotion)
				{
					this._actionAnimator.EnableRootMotionForAction();
					this._rootMotionPushed = true;
				}
			}
			this.BeginForcedLayerWeights();
			this.SnapPlayerToAnchor();
			if (this.promptUI != null)
			{
				this.promptUI.Hide();
				if (this.promptUI.gameObject != base.gameObject && this.promptUI.gameObject.activeSelf)
				{
					this.promptUI.gameObject.SetActive(false);
				}
			}
			this._playerAnimator.CrossFadeInFixedTime("Ciel_WasteCanHide_Enter", this.enterBlendDuration, 0);
			this.ApplyHideBodyPaint();
			this.PlayBinState("WasteCan_Open");
			if (this._lootSibling != null)
			{
				this._lootSibling.PlaySearchLoopForSibling();
			}
			if (this._sequence != null)
			{
				base.StopCoroutine(this._sequence);
			}
			this._sequence = base.StartCoroutine(this.EnterRoutine());
			if (this.debugLogs)
			{
				Debug.Log("[WasteCanHide] F pressed - hide enter started.", this);
			}
		}

		// Token: 0x06005872 RID: 22642 RVA: 0x002297D9 File Offset: 0x002279D9
		private void ApplyHideBodyPaint()
		{
			if (this._lootSibling != null)
			{
				this._lootSibling.ApplyLootBodyPaint();
				return;
			}
			if (this._controller != null)
			{
				GarbageBodyPaint.TryApply(this._controller.gameObject, null);
			}
		}

		// Token: 0x06005873 RID: 22643 RVA: 0x00229815 File Offset: 0x00227A15
		private IEnumerator EnterRoutine()
		{
			float t0 = Time.time;
			bool hairFadeStarted = false;
			bool reachedIdle = false;
			while (this._state == WasteCanHideInteractable.HideState.Entering && this._playerAnimator != null && Time.time - t0 < 20f)
			{
				AnimatorStateInfo currentAnimatorStateInfo = this._playerAnimator.GetCurrentAnimatorStateInfo(0);
				AnimatorStateInfo animatorStateInfo = (this._playerAnimator.IsInTransition(0) ? this._playerAnimator.GetNextAnimatorStateInfo(0) : default(AnimatorStateInfo));
				if (currentAnimatorStateInfo.IsName("Ciel_WasteCanHide_Idle"))
				{
					reachedIdle = true;
					break;
				}
				if (!hairFadeStarted && (currentAnimatorStateInfo.IsName("Ciel_WasteCanHide_HideEnter") || animatorStateInfo.IsName("Ciel_WasteCanHide_HideEnter")))
				{
					this.FadeHairBlend(true);
					hairFadeStarted = true;
					this.BindHairContainment(false);
					this.PlayBinState("WasteCan_HideEnter");
				}
				yield return null;
			}
			if (this._state != WasteCanHideInteractable.HideState.Entering)
			{
				yield break;
			}
			if (!reachedIdle)
			{
				this.InterruptForEncounter();
				yield break;
			}
			if (!hairFadeStarted)
			{
				this.FadeHairBlend(true);
			}
			this._state = WasteCanHideInteractable.HideState.Hidden;
			this._hiddenSince = Time.time;
			this.SetHiddenFreeCameraAvailable(true);
			ScoreGameplaySignals.NotifyHidingStarted(ScoreHidingSpotKind.Trashcan, this);
			if (this._lootSibling != null)
			{
				this._lootSibling.StopSearchLoopForSibling();
			}
			this.BindHairContainment(true);
			PlayerStealthBridge.SetHiddenSource(this, true);
			this.ShowHud();
			this._sequence = null;
			if (this.debugLogs)
			{
				Debug.Log("[WasteCanHide] Hidden inside the can.", this);
			}
			yield break;
		}

		// Token: 0x06005874 RID: 22644 RVA: 0x00229824 File Offset: 0x00227A24
		private bool SetHiddenFreeCameraAvailable(bool available)
		{
			return FullBodyHideActivityContext.SetPhotoModeAvailable(this._hideLease, available);
		}

		// Token: 0x06005875 RID: 22645 RVA: 0x00229834 File Offset: 0x00227A34
		private void BeginExit()
		{
			if (this._state != WasteCanHideInteractable.HideState.Hidden || FlyCamera.IsActive)
			{
				return;
			}
			InteractionCandidateRegistry.MarkContextInputConsumed(this.exitInput);
			if (!this.SetHiddenFreeCameraAvailable(false))
			{
				return;
			}
			this._state = WasteCanHideInteractable.HideState.Exiting;
			this.SetHideCamera(false);
			this.SetXray(false, false);
			this.HideHud();
			if (this._lootSibling != null)
			{
				this._lootSibling.PlaySearchLoopForSibling();
			}
			this._exitAnimationComplete = false;
			this._hairRevealComplete = false;
			this.DisableHairPhysicsForExit();
			this.RemoveHairContainment(true);
			PlayerStealthBridge.SetHiddenSource(this, false);
			this._playerAnimator.SetTrigger("WasteCanHideEnding");
			if (this._sequence != null)
			{
				base.StopCoroutine(this._sequence);
			}
			this._sequence = base.StartCoroutine(this.ExitRoutine());
			if (this.debugLogs)
			{
				Debug.Log("[WasteCanHide] F pressed - climbing out.", this);
			}
		}

		// Token: 0x06005876 RID: 22646 RVA: 0x00229907 File Offset: 0x00227B07
		private IEnumerator ExitRoutine()
		{
			float t0 = Time.time;
			bool sawHideExit = false;
			while (Time.time - t0 < 3f)
			{
				AnimatorStateInfo currentAnimatorStateInfo = this._playerAnimator.GetCurrentAnimatorStateInfo(0);
				AnimatorStateInfo nextAnimatorStateInfo = this._playerAnimator.GetNextAnimatorStateInfo(0);
				if (currentAnimatorStateInfo.IsName("Ciel_WasteCanHide_HideExit") || nextAnimatorStateInfo.IsName("Ciel_WasteCanHide_HideExit"))
				{
					this.PlayBinState("WasteCan_HideExit");
					sawHideExit = true;
					break;
				}
				yield return null;
			}
			if (!sawHideExit)
			{
				Debug.LogWarning("[WasteCanHide] HideExit state never reached (stale trigger?) - finishing anyway.", this);
			}
			if (sawHideExit)
			{
				t0 = Time.time;
				while (Time.time - t0 < 20f && !this._playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Ciel_WasteCanHide_Exit"))
				{
					yield return null;
				}
				this.FadeHairBlend(false);
				t0 = Time.time;
				while (Time.time - t0 < 20f)
				{
					if (!this._playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Ciel_WasteCanHide_Exit"))
					{
						break;
					}
					yield return null;
				}
			}
			else
			{
				this.FadeHairBlend(false);
			}
			this.FinishHide();
			this._sequence = null;
			yield break;
		}

		// Token: 0x06005877 RID: 22647 RVA: 0x00229918 File Offset: 0x00227B18
		private void FinishHide()
		{
			this.CrossFadeBinToClosed();
			if (this._lootSibling != null)
			{
				this._lootSibling.StopSearchLoopForSibling();
			}
			this.RemoveHairContainment(false);
			this.RestorePlayerState();
			this._state = WasteCanHideInteractable.HideState.None;
			this._exitAnimationComplete = true;
			this.TryRestoreHairPhysicsAfterExit();
			if (this.debugLogs)
			{
				Debug.Log("[WasteCanHide] Exit complete - control returned.", this);
			}
		}

		// Token: 0x06005878 RID: 22648 RVA: 0x00229978 File Offset: 0x00227B78
		private void RestorePlayerState()
		{
			if (this._interactionViewPushed && this._fpController != null)
			{
				this._fpController.EndInteractionView(this);
			}
			this._interactionViewPushed = false;
			this.SetPlayerPhysicsDisabled(false);
			this.LockControls(false);
			this.EndForcedLayerWeights();
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
			FullBodyHideActivityContext.Release(ref this._hideLease);
		}

		// Token: 0x06005879 RID: 22649 RVA: 0x00229A20 File Offset: 0x00227C20
		private void ForceAbort()
		{
			FlyCamera.StopForActivityLease(this._hideLease);
			Debug.LogWarning(string.Format("[WasteCanHide] ForceAbort! state={0}", this._state), this);
			if (this._sequence != null)
			{
				base.StopCoroutine(this._sequence);
				this._sequence = null;
			}
			PlayerStealthBridge.SetHiddenSource(this, false);
			this.SetHideCamera(false);
			this.SetXray(false, true);
			this.HideHud();
			if (this._lootSibling != null)
			{
				this._lootSibling.StopSearchLoopForSibling();
			}
			this.CrossFadeBinToClosed();
			this.RemoveHairContainment(false);
			this.RestoreHairBlendImmediately();
			this.RestoreExitHairPhysicsImmediately();
			this.RestorePlayerState();
			if (this._playerAnimator != null)
			{
				int num = WasteCanHideInteractable.ResolveLocomotionStateHash(this._playerAnimator);
				if (num != 0)
				{
					this._playerAnimator.Play(num, 0, 0f);
				}
			}
			this._state = WasteCanHideInteractable.HideState.None;
		}

		// Token: 0x0600587A RID: 22650 RVA: 0x00229AF8 File Offset: 0x00227CF8
		private void PrepareHairBlendTargets()
		{
			this.RestoreHairBlendImmediately();
			Transform transform = this.ResolvePlayerTransform();
			if (transform == null || string.IsNullOrEmpty(this.hideHairBlendShapeName))
			{
				return;
			}
			this._hairStyleService = transform.GetComponentInParent<MCStyleCustomizationService>(true) ?? transform.GetComponentInChildren<MCStyleCustomizationService>(true);
			if (this._hairStyleService != null)
			{
				this._hairStyleService.OnPhysicalSwapCompleted += this.HandleConcealedHairAdded;
				this._hairStyleService.OnPhysicalRemoved += this.HandleConcealedHairRemoved;
			}
			GameObject gameObject = ((this._hairStyleService != null) ? this._hairStyleService.GetSpawnedRoot("Head.Hair") : null);
			this.AddHairBlendTargets((gameObject != null) ? gameObject.transform : transform);
			if (this._hairBlendTargets.Count == 0 && this.debugLogs)
			{
				Debug.LogWarning("[WasteCanHide] No runtime hair renderer contains blendshape '" + this.hideHairBlendShapeName + "'.", this);
			}
		}

		// Token: 0x0600587B RID: 22651 RVA: 0x00229BE8 File Offset: 0x00227DE8
		private void AddHairBlendTargets(Transform root)
		{
			this._hairBlendSearch.Clear();
			root.GetComponentsInChildren<SkinnedMeshRenderer>(true, this._hairBlendSearch);
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in this._hairBlendSearch)
			{
				if (!(skinnedMeshRenderer == null) && !(skinnedMeshRenderer.sharedMesh == null))
				{
					Mesh sharedMesh = skinnedMeshRenderer.sharedMesh;
					int blendShapeIndex = sharedMesh.GetBlendShapeIndex(this.hideHairBlendShapeName);
					if (blendShapeIndex >= 0)
					{
						bool flag = false;
						using (List<WasteCanHideInteractable.HairBlendTarget>.Enumerator enumerator2 = this._hairBlendTargets.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								if (enumerator2.Current.Renderer == skinnedMeshRenderer)
								{
									flag = true;
									break;
								}
							}
						}
						if (!flag)
						{
							int blendShapeIndex2 = sharedMesh.GetBlendShapeIndex("RainCoat_HoodDown_HairFit");
							this._hairBlendTargets.Add(new WasteCanHideInteractable.HairBlendTarget
							{
								Renderer = skinnedMeshRenderer,
								Mesh = sharedMesh,
								ShapeIndex = blendShapeIndex,
								RestoreWeight = skinnedMeshRenderer.GetBlendShapeWeight(blendShapeIndex),
								HoodDownIndex = blendShapeIndex2,
								RestoreHoodDownWeight = ((blendShapeIndex2 >= 0) ? skinnedMeshRenderer.GetBlendShapeWeight(blendShapeIndex2) : 0f)
							});
						}
					}
				}
			}
		}

		// Token: 0x0600587C RID: 22652 RVA: 0x00229D40 File Offset: 0x00227F40
		private void HandleConcealedHairAdded(MCStylePartDefinitionSO definition, GameObject root)
		{
			if (definition == null || definition.partId != "Head.Hair" || root == null)
			{
				return;
			}
			this.AddHairBlendTargets(root.transform);
			this.ApplyHairConcealAmount(this._hairConcealAmount);
			if (this._containedHairCloths.Count > 0)
			{
				this.BindHairContainment(this._state == WasteCanHideInteractable.HideState.Hidden);
			}
		}

		// Token: 0x0600587D RID: 22653 RVA: 0x00229DA8 File Offset: 0x00227FA8
		private void HandleConcealedHairRemoved(MCStylePartDefinitionSO definition, GameObject root)
		{
			if (definition == null || definition.partId != "Head.Hair" || root == null)
			{
				return;
			}
			this._hairBlendTargets.RemoveAll((WasteCanHideInteractable.HairBlendTarget target) => target.Renderer == null || target.Renderer.transform.IsChildOf(root.transform));
		}

		// Token: 0x0600587E RID: 22654 RVA: 0x00229E04 File Offset: 0x00228004
		private void FadeHairBlend(bool hidden)
		{
			if (this._hairBlendTargets.Count == 0)
			{
				this.PrepareHairBlendTargets();
			}
			if (this._hairBlendTargets.Count == 0)
			{
				if (!hidden)
				{
					this.RestoreHairBlendImmediately();
					this._hairRevealComplete = true;
					this.TryRestoreHairPhysicsAfterExit();
				}
				return;
			}
			if (this._hairBlendCo != null)
			{
				base.StopCoroutine(this._hairBlendCo);
			}
			if (!hidden)
			{
				this._hairRevealComplete = false;
			}
			this._hairBlendCo = base.StartCoroutine(this.FadeHairBlendRoutine(hidden));
		}

		// Token: 0x0600587F RID: 22655 RVA: 0x00229E79 File Offset: 0x00228079
		private IEnumerator FadeHairBlendRoutine(bool hidden)
		{
			float startAmount = this._hairConcealAmount;
			float duration = Mathf.Max(0.01f, hidden ? this.hideHairBlendDuration : this.showHairBlendDuration);
			float elapsed = 0f;
			while (elapsed < duration)
			{
				elapsed += Time.deltaTime;
				float num = Mathf.Clamp01(elapsed / duration);
				float num2 = Mathf.SmoothStep(0f, 1f, num);
				this.ApplyHairBlendFrame(hidden, startAmount, num2);
				yield return null;
			}
			this.ApplyHairBlendFrame(hidden, startAmount, 1f);
			this._hairBlendCo = null;
			if (!hidden)
			{
				this.RestoreHairBlendImmediately();
				this._hairRevealComplete = true;
				this.TryRestoreHairPhysicsAfterExit();
			}
			yield break;
		}

		// Token: 0x06005880 RID: 22656 RVA: 0x00229E8F File Offset: 0x0022808F
		private void ApplyHairBlendFrame(bool hidden, float startAmount, float t)
		{
			this.ApplyHairConcealAmount(Mathf.LerpUnclamped(startAmount, hidden ? 1f : 0f, t));
		}

		// Token: 0x06005881 RID: 22657 RVA: 0x00229EB0 File Offset: 0x002280B0
		private void ApplyHairConcealAmount(float amount)
		{
			this._hairConcealAmount = Mathf.Clamp01(amount);
			foreach (WasteCanHideInteractable.HairBlendTarget hairBlendTarget in this._hairBlendTargets)
			{
				if (!(hairBlendTarget.Renderer == null) && !(hairBlendTarget.Renderer.sharedMesh != hairBlendTarget.Mesh))
				{
					hairBlendTarget.Renderer.SetBlendShapeWeight(hairBlendTarget.ShapeIndex, Mathf.LerpUnclamped(hairBlendTarget.RestoreWeight, 100f, this._hairConcealAmount));
					if (hairBlendTarget.HoodDownIndex >= 0)
					{
						hairBlendTarget.Renderer.SetBlendShapeWeight(hairBlendTarget.HoodDownIndex, Mathf.LerpUnclamped(hairBlendTarget.RestoreHoodDownWeight, 0f, this._hairConcealAmount));
					}
				}
			}
		}

		// Token: 0x06005882 RID: 22658 RVA: 0x00229F8C File Offset: 0x0022818C
		private void RestoreHairBlendImmediately()
		{
			if (this._hairBlendCo != null)
			{
				base.StopCoroutine(this._hairBlendCo);
				this._hairBlendCo = null;
			}
			foreach (WasteCanHideInteractable.HairBlendTarget hairBlendTarget in this._hairBlendTargets)
			{
				if (!(hairBlendTarget.Renderer == null) && !(hairBlendTarget.Renderer.sharedMesh != hairBlendTarget.Mesh))
				{
					hairBlendTarget.Renderer.SetBlendShapeWeight(hairBlendTarget.ShapeIndex, hairBlendTarget.RestoreWeight);
					if (hairBlendTarget.HoodDownIndex >= 0)
					{
						hairBlendTarget.Renderer.SetBlendShapeWeight(hairBlendTarget.HoodDownIndex, hairBlendTarget.RestoreHoodDownWeight);
					}
				}
			}
			this._hairBlendTargets.Clear();
			this._hairConcealAmount = 0f;
			if (this._hairStyleService != null)
			{
				this._hairStyleService.OnPhysicalSwapCompleted -= this.HandleConcealedHairAdded;
				this._hairStyleService.OnPhysicalRemoved -= this.HandleConcealedHairRemoved;
				this._hairStyleService = null;
			}
		}

		// Token: 0x06005883 RID: 22659 RVA: 0x0022A0AC File Offset: 0x002282AC
		private void DisableHairPhysicsForExit()
		{
			this.RestoreExitHairPhysicsImmediately();
			this._hairClothScratch.Clear();
			this.CollectRuntimeHairCloths(this._hairClothScratch);
			foreach (MagicaCloth magicaCloth in this._hairClothScratch)
			{
				if (!(magicaCloth == null) && magicaCloth.enabled)
				{
					this._exitDisabledHairCloths.Add(magicaCloth);
					magicaCloth.enabled = false;
				}
			}
			this._hairClothScratch.Clear();
		}

		// Token: 0x06005884 RID: 22660 RVA: 0x0022A148 File Offset: 0x00228348
		private void TryRestoreHairPhysicsAfterExit()
		{
			if (!this._exitAnimationComplete || !this._hairRevealComplete)
			{
				return;
			}
			if (this._hairPhysicsRestoreCo == null)
			{
				this._hairPhysicsRestoreCo = base.StartCoroutine(this.RestoreExitHairPhysicsRoutine());
			}
		}

		// Token: 0x06005885 RID: 22661 RVA: 0x0022A175 File Offset: 0x00228375
		private IEnumerator RestoreExitHairPhysicsRoutine()
		{
			foreach (MagicaCloth magicaCloth in this._exitDisabledHairCloths)
			{
				if (magicaCloth != null)
				{
					magicaCloth.enabled = true;
				}
			}
			yield return null;
			yield return null;
			foreach (MagicaCloth magicaCloth2 in this._exitDisabledHairCloths)
			{
				if (!(magicaCloth2 == null) && magicaCloth2.isActiveAndEnabled)
				{
					magicaCloth2.SetParameterChange();
					magicaCloth2.ResetCloth(false);
				}
			}
			this._exitDisabledHairCloths.Clear();
			this._exitAnimationComplete = false;
			this._hairRevealComplete = false;
			this._hairPhysicsRestoreCo = null;
			yield break;
		}

		// Token: 0x06005886 RID: 22662 RVA: 0x0022A184 File Offset: 0x00228384
		private void RestoreExitHairPhysicsImmediately()
		{
			if (this._hairPhysicsRestoreCo != null)
			{
				base.StopCoroutine(this._hairPhysicsRestoreCo);
				this._hairPhysicsRestoreCo = null;
			}
			foreach (MagicaCloth magicaCloth in this._exitDisabledHairCloths)
			{
				if (!(magicaCloth == null))
				{
					magicaCloth.enabled = true;
					if (magicaCloth.isActiveAndEnabled)
					{
						magicaCloth.ResetCloth(false);
					}
				}
			}
			this._exitDisabledHairCloths.Clear();
			this._exitAnimationComplete = false;
			this._hairRevealComplete = false;
		}

		// Token: 0x06005887 RID: 22663 RVA: 0x0022A224 File Offset: 0x00228424
		private void BindHairContainment(bool includeCeiling)
		{
			if (this.hairContainmentRoot == null)
			{
				if (this.debugLogs)
				{
					Debug.LogWarning("[WasteCanHide] Hair containment root is missing.", this);
				}
				return;
			}
			ColliderComponent[] containmentColliders = this.GetContainmentColliders();
			if (containmentColliders == null || containmentColliders.Length == 0)
			{
				if (this.debugLogs)
				{
					Debug.LogWarning("[WasteCanHide] No Magica colliders found in the containment root.", this);
				}
				return;
			}
			this._hairClothScratch.Clear();
			this.CollectRuntimeHairCloths(this._hairClothScratch);
			if (this._hairClothScratch.Count == 0)
			{
				if (this.debugLogs)
				{
					Debug.LogWarning("[WasteCanHide] Runtime hair root '" + this.runtimeHairRootName + "' has no MagicaCloth.", this);
				}
				return;
			}
			foreach (MagicaCloth magicaCloth in this._hairClothScratch)
			{
				if (!(magicaCloth == null) && magicaCloth.SerializeData != null && magicaCloth.SerializeData.colliderCollisionConstraint != null)
				{
					List<ColliderComponent> list = magicaCloth.SerializeData.colliderCollisionConstraint.colliderList;
					if (list == null)
					{
						list = new List<ColliderComponent>();
						magicaCloth.SerializeData.colliderCollisionConstraint.colliderList = list;
					}
					bool flag = false;
					foreach (ColliderComponent colliderComponent in containmentColliders)
					{
						if (!(colliderComponent == null) && (!(colliderComponent.name == "HairContainment_Ceiling") || includeCeiling) && !list.Contains(colliderComponent))
						{
							list.Add(colliderComponent);
							flag = true;
						}
					}
					this._containedHairCloths.Add(magicaCloth);
					WasteCanHideInteractable.NotifyHairContainmentChanged(magicaCloth, flag, this.resetHairAfterContainmentChange);
				}
			}
			this._hairClothScratch.Clear();
		}

		// Token: 0x06005888 RID: 22664 RVA: 0x0022A3D0 File Offset: 0x002285D0
		private void RemoveHairContainment(bool ceilingOnly)
		{
			if (this._containedHairCloths.Count == 0 || this.hairContainmentRoot == null)
			{
				return;
			}
			ColliderComponent[] containmentColliders = this.GetContainmentColliders();
			if (containmentColliders == null || containmentColliders.Length == 0)
			{
				return;
			}
			this._hairClothScratch.Clear();
			foreach (MagicaCloth magicaCloth in this._containedHairCloths)
			{
				if (magicaCloth != null)
				{
					this._hairClothScratch.Add(magicaCloth);
				}
			}
			this.CollectRuntimeHairCloths(this._hairClothScratch);
			foreach (MagicaCloth magicaCloth2 in this._hairClothScratch)
			{
				if (!(magicaCloth2 == null) && magicaCloth2.SerializeData != null && magicaCloth2.SerializeData.colliderCollisionConstraint != null)
				{
					List<ColliderComponent> colliderList = magicaCloth2.SerializeData.colliderCollisionConstraint.colliderList;
					if (colliderList != null)
					{
						bool flag = false;
						foreach (ColliderComponent colliderComponent in containmentColliders)
						{
							if (!(colliderComponent == null) && (!ceilingOnly || !(colliderComponent.name != "HairContainment_Ceiling")))
							{
								flag |= colliderList.Remove(colliderComponent);
							}
						}
						WasteCanHideInteractable.NotifyHairContainmentChanged(magicaCloth2, flag, false);
					}
				}
			}
			if (!ceilingOnly)
			{
				this._containedHairCloths.Clear();
			}
			this._hairClothScratch.Clear();
		}

		// Token: 0x06005889 RID: 22665 RVA: 0x0022A55C File Offset: 0x0022875C
		private ColliderComponent[] GetContainmentColliders()
		{
			if (this._cachedContainmentColliders == null && this.hairContainmentRoot != null)
			{
				this._cachedContainmentColliders = this.hairContainmentRoot.GetComponentsInChildren<ColliderComponent>(true);
			}
			return this._cachedContainmentColliders;
		}

		// Token: 0x0600588A RID: 22666 RVA: 0x0022A58C File Offset: 0x0022878C
		private void CollectRuntimeHairCloths(HashSet<MagicaCloth> output)
		{
			Transform transform = this.ResolvePlayerTransform();
			if (transform == null || output == null)
			{
				return;
			}
			Transform root = transform.root;
			this._runtimeHairClothScratch.Clear();
			root.GetComponentsInChildren<MagicaCloth>(true, this._runtimeHairClothScratch);
			for (int i = 0; i < this._runtimeHairClothScratch.Count; i++)
			{
				MagicaCloth magicaCloth = this._runtimeHairClothScratch[i];
				if (magicaCloth != null && this.IsBelowRuntimeHairRoot(magicaCloth.transform, root))
				{
					output.Add(magicaCloth);
				}
			}
			this._runtimeHairClothScratch.Clear();
		}

		// Token: 0x0600588B RID: 22667 RVA: 0x0022A61C File Offset: 0x0022881C
		private bool IsBelowRuntimeHairRoot(Transform candidate, Transform playerRoot)
		{
			Transform transform = candidate;
			while (transform != null)
			{
				if (this.IsRuntimeHairRoot(transform.name))
				{
					return true;
				}
				if (transform == playerRoot)
				{
					break;
				}
				transform = transform.parent;
			}
			return false;
		}

		// Token: 0x0600588C RID: 22668 RVA: 0x0022A658 File Offset: 0x00228858
		private bool IsRuntimeHairRoot(string candidateName)
		{
			if (string.IsNullOrEmpty(candidateName) || string.IsNullOrEmpty(this.runtimeHairRootName))
			{
				return false;
			}
			if (string.Equals(candidateName, this.runtimeHairRootName, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
			if (!candidateName.EndsWith("(Clone)", StringComparison.Ordinal))
			{
				return false;
			}
			int num = candidateName.Length - "(Clone)".Length;
			while (num > 0 && char.IsWhiteSpace(candidateName[num - 1]))
			{
				num--;
			}
			return num == this.runtimeHairRootName.Length && string.Compare(candidateName, 0, this.runtimeHairRootName, 0, num, StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x0600588D RID: 22669 RVA: 0x0022A6EC File Offset: 0x002288EC
		private static void NotifyHairContainmentChanged(MagicaCloth cloth, bool changed, bool resetSimulation)
		{
			if (!changed || cloth == null || !cloth.isActiveAndEnabled)
			{
				return;
			}
			cloth.SetParameterChange();
			cloth.ResetCloth(!resetSimulation);
		}

		// Token: 0x0600588E RID: 22670 RVA: 0x0022A714 File Offset: 0x00228914
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
			if (this._xrayInstances.Count == 0)
			{
				return;
			}
			this.StartXrayFade(1f, instant, true);
		}

		// Token: 0x0600588F RID: 22671 RVA: 0x0022A774 File Offset: 0x00228974
		private void BuildXrayInstances()
		{
			List<Renderer> list = new List<Renderer>();
			foreach (Renderer renderer in base.GetComponentsInChildren<Renderer>(true))
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
					WasteCanHideInteractable.MakeMaterialTransparent(material);
					this._xrayInstances.Add(material);
				}
			}
		}

		// Token: 0x06005890 RID: 22672 RVA: 0x0022A840 File Offset: 0x00228A40
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

		// Token: 0x06005891 RID: 22673 RVA: 0x0022A968 File Offset: 0x00228B68
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

		// Token: 0x06005892 RID: 22674 RVA: 0x0022A9CD File Offset: 0x00228BCD
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

		// Token: 0x06005893 RID: 22675 RVA: 0x0022A9EC File Offset: 0x00228BEC
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

		// Token: 0x06005894 RID: 22676 RVA: 0x0022AA88 File Offset: 0x00228C88
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

		// Token: 0x06005895 RID: 22677 RVA: 0x0022AB38 File Offset: 0x00228D38
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

		// Token: 0x06005896 RID: 22678 RVA: 0x0022AC00 File Offset: 0x00228E00
		private void SetHideCamera(bool on)
		{
			if (on && FullBodyHideActivityContext.IsUsingTripodCamera(this._hideLease))
			{
				return;
			}
			if (on == this._hideCamActive)
			{
				return;
			}
			if (on)
			{
				if (this.hideCamera == null)
				{
					this.hideCamera = this.CreateHideCamera();
				}
				if (this.hideCamera == null)
				{
					return;
				}
				if (this._fpController != null)
				{
					this._fpController.RegisterHeadVisibleCamera(this.hideCamera);
				}
				this._disabledMainCamera = Camera.main;
				if (this._disabledMainCamera != null)
				{
					this._disabledMainCamera.enabled = false;
				}
				this.hideCamera.gameObject.SetActive(true);
				this.hideCamera.enabled = true;
				this._hideCamActive = true;
				if (this.debugLogs)
				{
					Debug.Log("[WasteCanHide] Camera interna ON.", this);
					return;
				}
			}
			else
			{
				FlyCamera.StopForGameplayCamera(this.hideCamera);
				this._hideCamActive = false;
				if (this.hideCamera != null)
				{
					this.hideCamera.gameObject.SetActive(false);
				}
				if (this._fpController != null)
				{
					this._fpController.UnregisterHeadVisibleCamera(this.hideCamera);
				}
				if (this._disabledMainCamera != null)
				{
					this._disabledMainCamera.enabled = true;
				}
				this._disabledMainCamera = null;
				if (this.debugLogs)
				{
					Debug.Log("[WasteCanHide] Camera interna OFF.", this);
				}
			}
		}

		// Token: 0x06005897 RID: 22679 RVA: 0x0022AD5C File Offset: 0x00228F5C
		private Camera CreateHideCamera()
		{
			GameObject gameObject = new GameObject("WasteCanHideCamera");
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

		// Token: 0x06005898 RID: 22680 RVA: 0x0022ADDC File Offset: 0x00228FDC
		private void PlayBinState(string stateName)
		{
			if (this.binAnimator == null)
			{
				return;
			}
			this.binAnimator.Play(stateName, 0, 0f);
		}

		// Token: 0x06005899 RID: 22681 RVA: 0x0022ADFF File Offset: 0x00228FFF
		private void CrossFadeBinToClosed()
		{
			if (this.binAnimator == null)
			{
				return;
			}
			this.binAnimator.CrossFadeInFixedTime("WasteCan_Closed", this.binCloseBlend, 0);
		}

		// Token: 0x0600589A RID: 22682 RVA: 0x0022AE28 File Offset: 0x00229028
		private void BeginForcedLayerWeights()
		{
			if (this._playerAnimator == null || this._layersForced)
			{
				return;
			}
			this._savedLayerWeights = new float[this._playerAnimator.layerCount];
			for (int i = 1; i < this._playerAnimator.layerCount; i++)
			{
				this._savedLayerWeights[i] = this._playerAnimator.GetLayerWeight(i);
			}
			this._layersForced = true;
		}

		// Token: 0x0600589B RID: 22683 RVA: 0x0022AE94 File Offset: 0x00229094
		private void LateUpdate()
		{
			this.HoldAtAnchor();
			if (!this._layersForced || this._playerAnimator == null)
			{
				return;
			}
			for (int i = 1; i < this._playerAnimator.layerCount; i++)
			{
				if (!(this._playerAnimator.GetLayerName(i) == "Face"))
				{
					this._playerAnimator.SetLayerWeight(i, 0f);
				}
			}
		}

		// Token: 0x0600589C RID: 22684 RVA: 0x0022AF00 File Offset: 0x00229100
		private void EndForcedLayerWeights()
		{
			if (!this._layersForced)
			{
				return;
			}
			this._layersForced = false;
			if (this._playerAnimator == null || this._savedLayerWeights == null)
			{
				return;
			}
			int num = Mathf.Min(this._playerAnimator.layerCount, this._savedLayerWeights.Length);
			for (int i = 1; i < num; i++)
			{
				if (!(this._playerAnimator.GetLayerName(i) == "Face"))
				{
					this._playerAnimator.SetLayerWeight(i, this._savedLayerWeights[i]);
				}
			}
			this._savedLayerWeights = null;
		}

		// Token: 0x0600589D RID: 22685 RVA: 0x0022AF8C File Offset: 0x0022918C
		private void SnapPlayerToAnchor()
		{
			if (this.hideAnchor == null)
			{
				return;
			}
			Transform transform = this.ResolvePlayerTransform();
			if (transform == null)
			{
				return;
			}
			Quaternion quaternion = Quaternion.Euler(0f, this.hideAnchor.eulerAngles.y, 0f);
			WasteCanHideInteractable.ApplyPlayerPose(transform, this.hideAnchor.position, quaternion);
		}

		// Token: 0x0600589E RID: 22686 RVA: 0x0022AFEB File Offset: 0x002291EB
		private void HoldAtAnchor()
		{
			if (this._state == WasteCanHideInteractable.HideState.None || this.useRootMotion)
			{
				return;
			}
			this.SnapPlayerToAnchor();
		}

		// Token: 0x0600589F RID: 22687 RVA: 0x0022B004 File Offset: 0x00229204
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

		// Token: 0x060058A0 RID: 22688 RVA: 0x0022B058 File Offset: 0x00229258
		private void SetPlayerPhysicsDisabled(bool disabled)
		{
			if (this._controller == null || disabled == this._physicsPushed)
			{
				return;
			}
			if (disabled)
			{
				this._controller._rigidbody.useGravity = false;
				if (!this._controller._rigidbody.isKinematic)
				{
					this._controller._rigidbody.velocity = Vector3.zero;
				}
				this._controller._rigidbody.isKinematic = true;
				this._controller._capsuleCollider.isTrigger = true;
			}
			else
			{
				this._controller._rigidbody.useGravity = true;
				this._controller._rigidbody.isKinematic = false;
				this._controller._capsuleCollider.isTrigger = false;
			}
			this._physicsPushed = disabled;
		}

		// Token: 0x060058A1 RID: 22689 RVA: 0x0022B116 File Offset: 0x00229316
		private void LockControls(bool locked)
		{
			if (this._controller == null || locked == this._controlsLocked)
			{
				return;
			}
			this._controller.lockMovement = locked;
			this._controller.lockRotation = locked;
			this._controlsLocked = locked;
		}

		// Token: 0x060058A2 RID: 22690 RVA: 0x0022B14F File Offset: 0x0022934F
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

		// Token: 0x060058A3 RID: 22691 RVA: 0x0022B180 File Offset: 0x00229380
		private void ResolvePlayerRefs()
		{
			GameObject gameObject = ((PlayerRegistry.Player != null) ? PlayerRegistry.Player.gameObject : this._activePlayer);
			if (gameObject != null)
			{
				this._actionAnimator = gameObject.GetComponentInChildren<PlayerActionAnimator>();
				this._playerAnimator = gameObject.GetComponentInChildren<Animator>();
				this._controller = gameObject.GetComponentInChildren<vThirdPersonController>();
				this._fpController = gameObject.GetComponentInChildren<FirstPersonController>();
			}
			if (this._actionAnimator == null)
			{
				this._actionAnimator = Object.FindObjectOfType<PlayerActionAnimator>();
			}
			if (this._actionAnimator != null && this._actionAnimator.GenericAction != null)
			{
				if (this._playerAnimator == null)
				{
					this._playerAnimator = this._actionAnimator.GenericAction.GetComponentInChildren<Animator>();
				}
				if (this._controller == null)
				{
					this._controller = this._actionAnimator.GenericAction.GetComponent<vThirdPersonController>();
				}
			}
		}

		// Token: 0x060058A4 RID: 22692 RVA: 0x0022B268 File Offset: 0x00229468
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

		// Token: 0x060058A5 RID: 22693 RVA: 0x0022B2C0 File Offset: 0x002294C0
		private void ShowHud()
		{
			this._shownHudConfig = this.ResolveHiddenHudConfig();
			if (this._shownHudConfig != null && InteractionHUDController.Instance != null)
			{
				InteractionHUDController.Instance.Show(this._shownHudConfig);
			}
		}

		// Token: 0x060058A6 RID: 22694 RVA: 0x0022B2FC File Offset: 0x002294FC
		private InteractionConfigSO ResolveHiddenHudConfig()
		{
			if (this.hiddenHudConfig == null || !FullBodyHideActivityContext.IsUsingTripodCamera(this._hideLease))
			{
				return this.hiddenHudConfig;
			}
			if (this._tripodHudConfig == null)
			{
				this._tripodHudConfig = Object.Instantiate<InteractionConfigSO>(this.hiddenHudConfig);
				this._tripodHudConfig.hideFlags = HideFlags.HideAndDontSave;
				this._tripodHudConfig.name = this.hiddenHudConfig.name + " (Tripod)";
				List<UsableAction> usableActions = this._tripodHudConfig.usableActions;
				if (usableActions != null)
				{
					usableActions.RemoveAll(delegate(UsableAction action)
					{
						if (action != null)
						{
							string keyDisplayLabel = action.keyDisplayLabel;
							GenericInput genericInput = this.switchCameraInput;
							return string.Equals(keyDisplayLabel, ((genericInput != null) ? genericInput.keyboard : null) ?? "G", StringComparison.OrdinalIgnoreCase);
						}
						return false;
					});
				}
				ControllerContextBindings.CopyLabels(this.hiddenHudConfig, this._tripodHudConfig);
				ControllerContextBindings.SetLabel(this._tripodHudConfig, "F", "B", false);
				ControllerContextBindings.SetLabel(this._tripodHudConfig, "H", "R3", false);
			}
			return this._tripodHudConfig;
		}

		// Token: 0x060058A7 RID: 22695 RVA: 0x0022B3E0 File Offset: 0x002295E0
		private void HideHud()
		{
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (instance != null)
			{
				instance.HideIfCurrent((this._shownHudConfig != null) ? this._shownHudConfig : this.hiddenHudConfig);
			}
			this._shownHudConfig = null;
		}

		// Token: 0x040058D6 RID: 22742
		private const string EnterStateName = "Ciel_WasteCanHide_Enter";

		// Token: 0x040058D7 RID: 22743
		private const string HideEnterStateName = "Ciel_WasteCanHide_HideEnter";

		// Token: 0x040058D8 RID: 22744
		private const string IdleStateName = "Ciel_WasteCanHide_Idle";

		// Token: 0x040058D9 RID: 22745
		private const string HideExitStateName = "Ciel_WasteCanHide_HideExit";

		// Token: 0x040058DA RID: 22746
		private const string ExitStateName = "Ciel_WasteCanHide_Exit";

		// Token: 0x040058DB RID: 22747
		private const string EndingTrigger = "WasteCanHideEnding";

		// Token: 0x040058DC RID: 22748
		private const string GateReason = "WasteCanHide";

		// Token: 0x040058DD RID: 22749
		private const string BinOpenStateName = "WasteCan_Open";

		// Token: 0x040058DE RID: 22750
		private const string BinClosedStateName = "WasteCan_Closed";

		// Token: 0x040058DF RID: 22751
		private const string BinHideEnterStateName = "WasteCan_HideEnter";

		// Token: 0x040058E0 RID: 22752
		private const string BinHideExitStateName = "WasteCan_HideExit";

		// Token: 0x040058E1 RID: 22753
		private const string HairContainmentRootName = "HairContainmentColliders";

		// Token: 0x040058E2 RID: 22754
		private const string HairContainmentCeilingName = "HairContainment_Ceiling";

		// Token: 0x040058E3 RID: 22755
		private const string HideCameraName = "WasteCanHideCamera";

		// Token: 0x040058E4 RID: 22756
		[Header("References")]
		[Tooltip("Trigger zones used ONLY for range detection (when can F be pressed) — the same rummage triggers. Auto-resolved from children at Awake.")]
		[SerializeField]
		private vTriggerGenericAction[] rangeTriggers;

		// Token: 0x040058E5 RID: 22757
		[Tooltip("Shared world prompt (the one TrashcanLootInteractable drives). Only needed so it can be hidden while the hide runs. Auto-resolved from children.")]
		[SerializeField]
		private InteractablePromptUI promptUI;

		// Token: 0x040058E6 RID: 22758
		[Tooltip("Anchor in front of the can, facing it — the SAME RummageAnchor the rummage snaps to, so the shared Enter clip plays from its perfectly tuned spot. Auto-resolved from a child named 'RummageAnchor'.")]
		[SerializeField]
		private Transform hideAnchor;

		// Token: 0x040058E7 RID: 22759
		[Tooltip("Animator on the Waste_Can FBX instance (under WasteCanRig) — the lid/can animator the rummage uses. Auto-resolved like TrashcanLootInteractable does.")]
		[SerializeField]
		private Animator binAnimator;

		// Token: 0x040058E8 RID: 22760
		[Header("Hair containment (Magica Cloth 2)")]
		[Tooltip("Container of the inward-facing Magica plane colliders that seal the inside of the bin. Auto-resolved from a child named 'HairContainmentColliders'.")]
		[SerializeField]
		private Transform hairContainmentRoot;

		// Token: 0x040058E9 RID: 22761
		[Tooltip("Runtime outfit root to target. The Ciel hair prefab is spawned with this name.")]
		[SerializeField]
		private string runtimeHairRootName = "Hair";

		// Token: 0x040058EA RID: 22762
		[Tooltip("Reset the hair simulation after changing the environment collider list. This clears the large velocity spike caused by climbing into the bin.")]
		[SerializeField]
		private bool resetHairAfterContainmentChange = true;

		// Token: 0x040058EB RID: 22763
		[Header("Hair concealment")]
		[Tooltip("Blendshape on the runtime Hair prefab used to tuck every strand away while Ciel climbs into the bin.")]
		[SerializeField]
		private string hideHairBlendShapeName = "RainCoat_HideHair";

		// Token: 0x040058EC RID: 22764
		[Tooltip("Seconds used to conceal the hair on entry.")]
		[Min(0.01f)]
		[SerializeField]
		private float hideHairBlendDuration = 1f;

		// Token: 0x040058ED RID: 22765
		[Tooltip("Seconds used to reveal the hair near the end of exit. Slightly slower than entry for a gentle visual reveal while Magica Cloth remains frozen.")]
		[Min(0.01f)]
		[SerializeField]
		private float showHairBlendDuration = 1.5f;

		// Token: 0x040058EE RID: 22766
		[Header("UI Config")]
		[Tooltip("Config for the right-side HUD shown WHILE HIDDEN (needs the 'Exit' / F usable action). Wired by the setup tool to WasteCanHideHudConfig.")]
		[SerializeField]
		private InteractionConfigSO hiddenHudConfig;

		// Token: 0x040058EF RID: 22767
		[Header("Input")]
		[Tooltip("Pressed while in range to hide inside the can.")]
		[SerializeField]
		private GenericInput hideInput = new GenericInput("F", "X", "X");

		// Token: 0x040058F0 RID: 22768
		[Tooltip("Pressed while hidden to climb back out.")]
		[SerializeField]
		private GenericInput exitInput = new GenericInput("F", "X", "X");

		// Token: 0x040058F1 RID: 22769
		[Tooltip("Seconds after reaching the hidden idle before the exit key is accepted (the F press that entered must not leak into the exit).")]
		[SerializeField]
		private float exitInputGrace = 0.5f;

		// Token: 0x040058F2 RID: 22770
		[Tooltip("Sanity check sulla F: questo bidone parte solo se il player e' davvero entro questo raggio dall'anchor E se e' il candidato piu' vicino tra i bidoni in range. Ogni istanza ascolta i PROPRI trigger: un flag range stantio (chunk pooled disabilitato senza evento di uscita) non deve teletrasportare il player su un bidone dall'altra parte della mappa.")]
		[SerializeField]
		private float maxStartDistance = 3f;

		// Token: 0x040058F3 RID: 22771
		[Header("X-ray (see through the can while hidden)")]
		[Tooltip("Toggla la trasparenza del bidone mentre e' nascosta dentro (stessa feature dell'armadietto). F = exit, G = camera interna: H e' libero in questo contesto.")]
		[SerializeField]
		private GenericInput xrayInput = new GenericInput("H", "RightStickClick", "RightStickClick");

		// Token: 0x040058F4 RID: 22772
		[Tooltip("Opacita' del bidone (0..1) con l'x-ray ON - piu' basso = piu' trasparente.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float xrayOpacity = 0.3f;

		// Token: 0x040058F5 RID: 22773
		[Tooltip("Secondi del fade di opacita' in/out.")]
		[SerializeField]
		private float xrayFadeDuration = 0.25f;

		// Token: 0x040058F6 RID: 22774
		[Header("Hide Camera")]
		[Tooltip("Camera dentro il cassonetto che inquadra Ciel mentre e' nascosta. Stesso pattern della rummage camera: main camera DISABILITATA (solo il componente, l'AudioListener resta) e ripristinata al toggle-off o all'uscita. Auto-risolta da un figlio 'WasteCanHideCamera'; se assente viene creata a runtime con la posa locale serializzata qui sotto.")]
		[SerializeField]
		private Camera hideCamera;

		// Token: 0x040058F7 RID: 22775
		[Tooltip("Premuto mentre e' nascosta: alterna la camera principale e quella interna.")]
		[SerializeField]
		private GenericInput switchCameraInput = new GenericInput("G", "Y", "Y");

		// Token: 0x040058F8 RID: 22776
		[Tooltip("Posa LOCALE (figlia del root del cassonetto) della camera auto-creata: valori tarati a mano in Play (alto nel bidone, guarda in giu' verso Ciel accucciata).")]
		[SerializeField]
		private Vector3 cameraLocalPosition = new Vector3(-0.0778f, 0.8881f, 0.2727f);

		// Token: 0x040058F9 RID: 22777
		[SerializeField]
		private Vector3 cameraLocalEuler = new Vector3(140f, 0f, 0f);

		// Token: 0x040058FA RID: 22778
		[Tooltip("FOV della camera interna (tarato a mano).")]
		[SerializeField]
		private float cameraFieldOfView = 47.2f;

		// Token: 0x040058FB RID: 22779
		[Header("Animation")]
		[Tooltip("The WasteCan char clips are authored in place (root XZ baked into the pose, same as the rummage — useRootMotion=0 on the prefabs). Enable only if the hide clips get re-exported with real root translation.")]
		[SerializeField]
		private bool useRootMotion;

		// Token: 0x040058FC RID: 22780
		[Tooltip("Crossfade into the Enter state.")]
		[SerializeField]
		private float enterBlendDuration = 0.15f;

		// Token: 0x040058FD RID: 22781
		[Tooltip("Blend used to settle the can back on its Closed pose after the exit.")]
		[SerializeField]
		private float binCloseBlend = 0.4f;

		// Token: 0x040058FE RID: 22782
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x040058FF RID: 22783
		private WasteCanHideInteractable.HideState _state;

		// Token: 0x04005900 RID: 22784
		private bool _playerInRange;

		// Token: 0x04005901 RID: 22785
		private GameObject _activePlayer;

		// Token: 0x04005902 RID: 22786
		private float _hiddenSince;

		// Token: 0x04005903 RID: 22787
		private bool _hideCamActive;

		// Token: 0x04005904 RID: 22788
		private Camera _disabledMainCamera;

		// Token: 0x04005905 RID: 22789
		private bool _interactionViewPushed;

		// Token: 0x04005906 RID: 22790
		private InteractionConfigSO _tripodHudConfig;

		// Token: 0x04005907 RID: 22791
		private InteractionConfigSO _shownHudConfig;

		// Token: 0x04005908 RID: 22792
		private static readonly List<WasteCanHideInteractable> InRangeCans = new List<WasteCanHideInteractable>();

		// Token: 0x04005909 RID: 22793
		private static vGenericAction _sharedPlayerGenericAction;

		// Token: 0x0400590A RID: 22794
		private bool _xrayOn;

		// Token: 0x0400590B RID: 22795
		private Coroutine _xrayFade;

		// Token: 0x0400590C RID: 22796
		private Renderer[] _xrayRenderers;

		// Token: 0x0400590D RID: 22797
		private Material[][] _xrayOriginalMats;

		// Token: 0x0400590E RID: 22798
		private readonly List<Material> _xrayInstances = new List<Material>();

		// Token: 0x0400590F RID: 22799
		private PlayerActionAnimator _actionAnimator;

		// Token: 0x04005910 RID: 22800
		private Animator _playerAnimator;

		// Token: 0x04005911 RID: 22801
		private vThirdPersonController _controller;

		// Token: 0x04005912 RID: 22802
		private FirstPersonController _fpController;

		// Token: 0x04005913 RID: 22803
		private vGenericAction _playerGenericAction;

		// Token: 0x04005914 RID: 22804
		private TrashcanLootInteractable _lootSibling;

		// Token: 0x04005915 RID: 22805
		private bool _physicsPushed;

		// Token: 0x04005916 RID: 22806
		private bool _rootMotionPushed;

		// Token: 0x04005917 RID: 22807
		private bool _emotionPushed;

		// Token: 0x04005918 RID: 22808
		private bool _controlsLocked;

		// Token: 0x04005919 RID: 22809
		private PlayerActivityLease _hideLease;

		// Token: 0x0400591A RID: 22810
		private float[] _savedLayerWeights;

		// Token: 0x0400591B RID: 22811
		private bool _layersForced;

		// Token: 0x0400591C RID: 22812
		private Coroutine _sequence;

		// Token: 0x0400591D RID: 22813
		private Coroutine _hairBlendCo;

		// Token: 0x0400591E RID: 22814
		private Coroutine _hairPhysicsRestoreCo;

		// Token: 0x0400591F RID: 22815
		private readonly HashSet<MagicaCloth> _containedHairCloths = new HashSet<MagicaCloth>();

		// Token: 0x04005920 RID: 22816
		private readonly HashSet<MagicaCloth> _exitDisabledHairCloths = new HashSet<MagicaCloth>();

		// Token: 0x04005921 RID: 22817
		private readonly List<WasteCanHideInteractable.HairBlendTarget> _hairBlendTargets = new List<WasteCanHideInteractable.HairBlendTarget>();

		// Token: 0x04005922 RID: 22818
		private readonly List<SkinnedMeshRenderer> _hairBlendSearch = new List<SkinnedMeshRenderer>();

		// Token: 0x04005923 RID: 22819
		private MCStyleCustomizationService _hairStyleService;

		// Token: 0x04005924 RID: 22820
		private float _hairConcealAmount;

		// Token: 0x04005925 RID: 22821
		private const string HoodDownHairFitShape = "RainCoat_HoodDown_HairFit";

		// Token: 0x04005926 RID: 22822
		private readonly HashSet<MagicaCloth> _hairClothScratch = new HashSet<MagicaCloth>();

		// Token: 0x04005927 RID: 22823
		private readonly List<MagicaCloth> _runtimeHairClothScratch = new List<MagicaCloth>();

		// Token: 0x04005928 RID: 22824
		private ColliderComponent[] _cachedContainmentColliders;

		// Token: 0x04005929 RID: 22825
		private bool _exitAnimationComplete;

		// Token: 0x0400592A RID: 22826
		private bool _hairRevealComplete;

		// Token: 0x0400592B RID: 22827
		private UnityAction<GameObject> _validateCallback;

		// Token: 0x0400592C RID: 22828
		private UnityAction<GameObject> _invalidateCallback;

		// Token: 0x0400592D RID: 22829
		private bool _rangeEventsSubscribed;

		// Token: 0x0400592E RID: 22830
		private bool _acceptRangeEvents;

		// Token: 0x0400592F RID: 22831
		private Vector3 _entryPosition;

		// Token: 0x04005930 RID: 22832
		private Quaternion _entryRotation;

		// Token: 0x02000BFB RID: 3067
		private enum HideState
		{
			// Token: 0x04005932 RID: 22834
			None,
			// Token: 0x04005933 RID: 22835
			Entering,
			// Token: 0x04005934 RID: 22836
			Hidden,
			// Token: 0x04005935 RID: 22837
			Exiting
		}

		// Token: 0x02000BFC RID: 3068
		private sealed class HairBlendTarget
		{
			// Token: 0x04005936 RID: 22838
			public SkinnedMeshRenderer Renderer;

			// Token: 0x04005937 RID: 22839
			public Mesh Mesh;

			// Token: 0x04005938 RID: 22840
			public int ShapeIndex;

			// Token: 0x04005939 RID: 22841
			public float RestoreWeight;

			// Token: 0x0400593A RID: 22842
			public int HoodDownIndex;

			// Token: 0x0400593B RID: 22843
			public float RestoreHoodDownWeight;
		}
	}
}
