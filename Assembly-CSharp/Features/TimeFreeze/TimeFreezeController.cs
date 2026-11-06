using System;
using System.Collections;
using Features.DebugTools;
using Features.Detection;
using Features.GameOver;
using Features.Inventory;
using Features.Inventory.Wearable;
using Features.MainMenu;
using Features.Police;
using Features.TrashcanLoot;
using Features.UI.TimedEffects;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Interactions;
using ProjectExhibitionist.UI;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Features.TimeFreeze
{
	// Token: 0x02000C13 RID: 3091
	[DisallowMultipleComponent]
	public sealed class TimeFreezeController : MonoBehaviour
	{
		// Token: 0x17000EBC RID: 3772
		// (get) Token: 0x06005974 RID: 22900 RVA: 0x0022F8E4 File Offset: 0x0022DAE4
		public static bool IsAnyActive
		{
			get
			{
				return TimeFreezeController.s_anyActive;
			}
		}

		// Token: 0x17000EBD RID: 3773
		// (get) Token: 0x06005975 RID: 22901 RVA: 0x0022F8EB File Offset: 0x0022DAEB
		public static bool OwnsContextInput
		{
			get
			{
				return TimeFreezeController.s_anyActive || TimeFreezeController.s_resumeInputFrame == Time.frameCount;
			}
		}

		// Token: 0x17000EBE RID: 3774
		// (get) Token: 0x06005976 RID: 22902 RVA: 0x0022F902 File Offset: 0x0022DB02
		public bool IsActive
		{
			get
			{
				return this._active;
			}
		}

		// Token: 0x06005977 RID: 22903 RVA: 0x0022F90A File Offset: 0x0022DB0A
		public bool IsActiveFor(WearableItemSO item)
		{
			return this._active && item != null && this._activeItem == item;
		}

		// Token: 0x06005978 RID: 22904 RVA: 0x0022F928 File Offset: 0x0022DB28
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStaticState()
		{
			TimeFreezeController.s_anyActive = false;
			TimeFreezeController.s_resumeInputFrame = -1;
		}

		// Token: 0x06005979 RID: 22905 RVA: 0x0022F938 File Offset: 0x0022DB38
		private void Awake()
		{
			this._settings = Resources.Load<TimeFreezeSettingsSO>("TimeFreezeSettings");
			this._braceletDefinition = Resources.Load<WearableItemSO>("Wardrobe/Cloth_GymSet_MeterBand");
			this._wearableUse = base.GetComponent<WearableUseController>();
			if (this._wearableUse == null)
			{
				this._wearableUse = base.GetComponentInChildren<WearableUseController>(true);
			}
			if (this._wearableUse == null)
			{
				this._wearableUse = base.GetComponentInParent<WearableUseController>(true);
			}
			this._actionAnimator = base.GetComponent<PlayerActionAnimator>();
			if (this._actionAnimator == null)
			{
				this._actionAnimator = base.GetComponentInChildren<PlayerActionAnimator>(true);
			}
			if (this._actionAnimator == null)
			{
				this._actionAnimator = base.GetComponentInParent<PlayerActionAnimator>(true);
			}
			this._thirdPersonInput = base.GetComponent<vThirdPersonInput>();
			if (this._thirdPersonInput == null)
			{
				this._thirdPersonInput = base.GetComponentInChildren<vThirdPersonInput>(true);
			}
			if (this._thirdPersonInput == null)
			{
				this._thirdPersonInput = base.GetComponentInParent<vThirdPersonInput>(true);
			}
			if (this._thirdPersonInput != null && this._thirdPersonInput.cc != null)
			{
				this._animator = this._thirdPersonInput.cc.animator;
			}
			if (this._animator == null)
			{
				this._animator = base.GetComponentInChildren<Animator>(true);
			}
			this.EnsureRuntimeEffects();
		}

		// Token: 0x0600597A RID: 22906 RVA: 0x0022FA83 File Offset: 0x0022DC83
		private void OnEnable()
		{
			if (this._wearableUse != null)
			{
				this._wearableUse.OnItemUnworn += this.HandleItemUnworn;
			}
		}

		// Token: 0x0600597B RID: 22907 RVA: 0x0022FAAA File Offset: 0x0022DCAA
		private void OnDisable()
		{
			if (this._wearableUse != null)
			{
				this._wearableUse.OnItemUnworn -= this.HandleItemUnworn;
			}
			this.AbortAndRestore();
		}

		// Token: 0x0600597C RID: 22908 RVA: 0x0022FAD7 File Offset: 0x0022DCD7
		private void OnDestroy()
		{
			if (this._wearableUse != null)
			{
				this._wearableUse.OnItemUnworn -= this.HandleItemUnworn;
			}
			this.AbortAndRestore();
			this.DestroyRuntimeEffects();
		}

		// Token: 0x0600597D RID: 22909 RVA: 0x0022FB0C File Offset: 0x0022DD0C
		public bool TryToggle(WearableItemSO item)
		{
			if (this._lastAcceptedToggleFrame == Time.frameCount)
			{
				return false;
			}
			if (!this._active)
			{
				bool flag = this.TryActivate(item);
				if (flag)
				{
					this._lastAcceptedToggleFrame = Time.frameCount;
				}
				return flag;
			}
			if (item != this._activeItem || this._closing || this._stopRequested)
			{
				return false;
			}
			this._stopRequested = true;
			this._lastAcceptedToggleFrame = Time.frameCount;
			TimeFreezeController.s_resumeInputFrame = Time.frameCount;
			WearableUseController wearableUse = this._wearableUse;
			if (wearableUse != null)
			{
				wearableUse.RefreshFocusedActionsHud();
			}
			return true;
		}

		// Token: 0x0600597E RID: 22910 RVA: 0x0022FB90 File Offset: 0x0022DD90
		public bool TryActivate(WearableItemSO item)
		{
			if (!this.CanActivate(item))
			{
				return false;
			}
			if (!this.PrepareActivationLayer())
			{
				ToastService.Show("The bracelet arms layer is not configured.", "Time Freeze", null, -1f, "gameplay_residuals");
				return false;
			}
			if (!PlayerActivityGate.TryAcquire("Gym Time Freeze", this, PlayerActivity.Interact, PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.FullBodyAnimation | PlayerContextResource.HeldItem | PlayerContextResource.WorldSimulation, PlayerActivity.Cover | PlayerActivity.Flash | PlayerActivity.Interact | PlayerActivity.Undress | PlayerActivity.LingerieOpen | PlayerActivity.Inventory | PlayerActivity.Phone | PlayerActivity.DebugFly, out this._activityLease, false, null))
			{
				this.ReleasePreparedActivationLayer();
				ToastService.Show("Time Freeze cannot be used during another action.", "Time Freeze", null, -1f, "gameplay_residuals");
				return false;
			}
			if (!PlayerActivityGate.TryUpdate(this._activityLease, PlayerActivity.None, PlayerContextResource.WorldSimulation, PlayerActivity.Cover | PlayerActivity.Flash | PlayerActivity.Interact | PlayerActivity.Undress | PlayerActivity.LingerieOpen | PlayerActivity.Inventory | PlayerActivity.Phone | PlayerActivity.DebugFly))
			{
				this._activityLease.Dispose();
				this._activityLease = null;
				this.ReleasePreparedActivationLayer();
				ToastService.Show("Time Freeze cannot reserve the world simulation.", "Time Freeze", null, -1f, "gameplay_residuals");
				return false;
			}
			this._stopRequested = false;
			this._closing = false;
			this._hardFreezeCommitted = false;
			this._cycleChargeConsumed = false;
			this._active = true;
			TimeFreezeController.s_anyActive = true;
			this._activeItem = item;
			this._sequenceRoutine = base.StartCoroutine(this.RunTimeFreeze());
			WearableUseController wearableUse = this._wearableUse;
			if (wearableUse != null)
			{
				wearableUse.RefreshFocusedActionsHud();
			}
			return true;
		}

		// Token: 0x0600597F RID: 22911 RVA: 0x0022FCB0 File Offset: 0x0022DEB0
		private bool CanActivate(WearableItemSO item)
		{
			if (this._active || TimeFreezeController.s_anyActive || item == null)
			{
				return false;
			}
			if (TimeFreezeController.IsDetectionBlockingActivation())
			{
				return false;
			}
			if (TimeFreezeController.IsContextInputReserved(item))
			{
				return false;
			}
			if (this._settings == null || this._settings.activationClip == null)
			{
				Debug.LogError("[TimeFreeze] Missing Resources/TimeFreezeSettings or activation clip.", this);
				return false;
			}
			if (this._wearableUse == null || !this._wearableUse.IsWorn(item))
			{
				return false;
			}
			if (!this._wearableUse.HasWornInventoryDurability(item, 1f))
			{
				ToastService.Show("The bracelet has no Time Freeze charges left.", "Time Freeze", null, -1f, "gameplay_residuals");
				return false;
			}
			if (InteractionCandidateRegistry.HasEligibleCandidate)
			{
				return false;
			}
			if (FlyCamera.IsActive || Time.timeScale <= 0.01f || AudioListener.pause)
			{
				return false;
			}
			if (this._thirdPersonInput == null || this._thirdPersonInput.cc == null || this._animator == null)
			{
				Debug.LogError("[TimeFreeze] Player locomotion/Animator components are unavailable.", this);
				return false;
			}
			vThirdPersonController cc = this._thirdPersonInput.cc;
			return cc.isGrounded && !cc.ragdolled && !cc.customAction && !cc.isRolling && !cc.lockMovement && !cc.lockAnimMovement && !cc.lockAnimRotation && (!(this._actionAnimator != null) || !(this._actionAnimator.GenericAction != null) || (!this._actionAnimator.GenericAction.doingAction && this._actionAnimator.GenericAction.actionConditions));
		}

		// Token: 0x06005980 RID: 22912 RVA: 0x0022FE54 File Offset: 0x0022E054
		private static bool IsDetectionBlockingActivation()
		{
			ExhibitionEventDirector instance = ExhibitionEventDirector.Instance;
			if (instance != null && (instance.IsEventActive || instance.HasPendingEncounter))
			{
				return true;
			}
			GameOverManager instance2 = GameOverManager.Instance;
			if (instance2 != null && (instance2.IsGameOver || instance2.HasPendingCapture))
			{
				return true;
			}
			PlayerStunExecutor instance3 = PlayerStunExecutor.Instance;
			return instance3 != null && instance3.IsStunned;
		}

		// Token: 0x06005981 RID: 22913 RVA: 0x0022FEBC File Offset: 0x0022E0BC
		private static bool IsContextInputReserved(WearableItemSO item)
		{
			if (InteractionCandidateRegistry.HasEligibleCandidate)
			{
				return true;
			}
			if (item == null || item.wearActions == null)
			{
				return false;
			}
			for (int i = 0; i < item.wearActions.Count; i++)
			{
				WearableActionDefinition wearableActionDefinition = item.wearActions[i];
				if (!(wearableActionDefinition == null) && wearableActionDefinition.ActionType == WearableActionType.TimeFreeze && (InteractionCandidateRegistry.IsInventoryInputReserved(wearableActionDefinition.Input) || WasteCanHideInteractable.ReservesContextInput(wearableActionDefinition.Input)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005982 RID: 22914 RVA: 0x0022FF37 File Offset: 0x0022E137
		private IEnumerator RunTimeFreeze()
		{
			this.EnsureRuntimeEffects();
			Transform transform = ((this._thirdPersonInput != null) ? this._thirdPersonInput.transform : base.transform.root);
			this._world = new TimeFreezeWorldSnapshot(transform, this._settings.minimumWorldPitch);
			this._world.Capture();
			this._worldRamp = new TimeFreezeWorldRamp(transform);
			this._worldRamp.Capture();
			this.PlayTransition(false);
			this.StartArmGesture(1f);
			yield return TimeFreezeController.WaitUnscaled(this._settings.activationDelay);
			yield return this.RampWorld(1f, 0f, this._settings.slowDownDuration);
			this._worldRamp.RefreshDynamicParticipants(true);
			this._worldRamp.SetFactor(1f);
			this._world.Freeze();
			this._hardFreezeCommitted = true;
			this._worldRamp.SetFactor(0f);
			float num = Mathf.Min(60f, Mathf.Max(0f, this._settings.frozenDuration));
			float remainingDuration = num;
			this.PlayLockCue();
			this.StartFrozenHum();
			while (this._actionActive)
			{
				yield return null;
			}
			if (this._activityLease != null && this._activityLease.IsActive && !PlayerActivityGate.TryUpdate(this._activityLease, PlayerActivity.None, PlayerContextResource.WorldSimulation, PlayerActivity.Cover | PlayerActivity.Flash | PlayerActivity.Interact | PlayerActivity.Undress | PlayerActivity.LingerieOpen | PlayerActivity.Inventory | PlayerActivity.Phone))
			{
				Debug.LogWarning("[TimeFreeze] Could not enable the hard-freeze player activity profile.", this);
			}
			this.RegisterHud();
			while (!this._stopRequested && remainingDuration > 0f)
			{
				if (!GameplayDurationPause.IsPaused)
				{
					remainingDuration = Mathf.Max(0f, remainingDuration - Time.unscaledDeltaTime);
				}
				TimedEffectHUDService instance = TimedEffectHUDService.Instance;
				if (instance != null)
				{
					instance.SetRemainingTime("GymTimeFreeze", remainingDuration);
				}
				yield return null;
			}
			bool manualResumeGesture = TimeFreezeController.ShouldReplayArmGestureOnResume(this._stopRequested);
			this._closing = true;
			if (this._activityLease != null && this._activityLease.IsActive && !PlayerActivityGate.TryUpdate(this._activityLease, PlayerActivity.None, PlayerContextResource.WorldSimulation, PlayerActivity.Cover | PlayerActivity.Flash | PlayerActivity.Interact | PlayerActivity.Undress | PlayerActivity.LingerieOpen | PlayerActivity.Inventory | PlayerActivity.Phone | PlayerActivity.DebugFly))
			{
				Debug.LogWarning("[TimeFreeze] Could not restore the transition activity profile.", this);
			}
			FlyCamera.StopGlobalPause();
			if (manualResumeGesture)
			{
				if (this.PrepareActivationLayer())
				{
					this.StartArmGesture(TimeFreezeController.CalculateResumeGesturePlaybackSpeed(this._settings));
				}
				else
				{
					Debug.LogWarning("[TimeFreeze] The manual resume gesture could not be prepared; resuming without it.", this);
				}
			}
			this.UnregisterHud();
			this.FadeOutFrozenHum();
			this._world.SetAudioSlowFactor(0f);
			this._world.Thaw(this._settings.resumeDuration);
			this._worldRamp.RefreshDynamicParticipants(true);
			this._worldRamp.SetFactor(0f);
			this.PlayTransition(true);
			yield return this.RampWorld(0f, 1f, this._settings.resumeDuration);
			this._world.RestoreAll();
			this._world = null;
			this._worldRamp.Restore();
			this._worldRamp = null;
			this.SetVisualWeight(0f);
			this.StopFrozenHumImmediate();
			if (manualResumeGesture)
			{
				while (this._actionActive)
				{
					yield return null;
				}
			}
			this.FinishNormally();
			yield break;
		}

		// Token: 0x06005983 RID: 22915 RVA: 0x000F5D4D File Offset: 0x000F3F4D
		private static bool ShouldReplayArmGestureOnResume(bool stopRequested)
		{
			return stopRequested;
		}

		// Token: 0x06005984 RID: 22916 RVA: 0x0022FF48 File Offset: 0x0022E148
		private static float CalculateResumeGesturePlaybackSpeed(TimeFreezeSettingsSO settings)
		{
			if (settings == null || settings.activationClip == null)
			{
				return 1f;
			}
			float num = Mathf.Clamp(settings.activationFadeOutNormalizedTime, 0.1f, 0.95f);
			float num2 = settings.activationClip.length * num;
			float num3 = Mathf.Max(0.05f, settings.resumeDuration - settings.activationBlendOutDuration);
			return Mathf.Clamp(num2 / num3, 0.25f, 4f);
		}

		// Token: 0x06005985 RID: 22917 RVA: 0x0022FFBE File Offset: 0x0022E1BE
		private IEnumerator RampWorld(float from, float to, float duration)
		{
			float elapsed = 0f;
			duration = Mathf.Max(0.01f, duration);
			while (elapsed < duration)
			{
				elapsed += Time.unscaledDeltaTime;
				float num = Mathf.Clamp01(elapsed / duration);
				float num2 = num * num * (3f - 2f * num);
				float num3 = Mathf.LerpUnclamped(from, to, num2);
				TimeFreezeWorldRamp worldRamp = this._worldRamp;
				if (worldRamp != null)
				{
					worldRamp.SetFactor(num3);
				}
				TimeFreezeWorldSnapshot world = this._world;
				if (world != null)
				{
					world.SetAudioSlowFactor(num3);
				}
				this.SetVisualWeight(1f - num3);
				yield return null;
			}
			TimeFreezeWorldRamp worldRamp2 = this._worldRamp;
			if (worldRamp2 != null)
			{
				worldRamp2.SetFactor(to);
			}
			TimeFreezeWorldSnapshot world2 = this._world;
			if (world2 != null)
			{
				world2.SetAudioSlowFactor(to);
			}
			this.SetVisualWeight(1f - to);
			yield break;
		}

		// Token: 0x06005986 RID: 22918 RVA: 0x0022FFE2 File Offset: 0x0022E1E2
		private static IEnumerator WaitUnscaled(float seconds)
		{
			float elapsed = 0f;
			while (elapsed < seconds)
			{
				elapsed += Time.unscaledDeltaTime;
				yield return null;
			}
			yield break;
		}

		// Token: 0x06005987 RID: 22919 RVA: 0x0022FFF4 File Offset: 0x0022E1F4
		private bool PrepareActivationLayer()
		{
			this.ReleasePreparedActivationLayer();
			if (this._animator == null || this._animator.runtimeAnimatorController == null || this._settings == null)
			{
				return false;
			}
			string activationLayerName = this._settings.activationLayerName;
			string activationStateName = this._settings.activationStateName;
			if (string.IsNullOrWhiteSpace(activationLayerName) || string.IsNullOrWhiteSpace(activationStateName))
			{
				return false;
			}
			this._activationLayerIndex = this._animator.GetLayerIndex(activationLayerName);
			if (this._activationLayerIndex < 0)
			{
				Debug.LogError("[TimeFreeze] Animator layer '" + activationLayerName + "' was not found.", this);
				this.ReleasePreparedActivationLayer();
				return false;
			}
			this._activationStateFullHash = Animator.StringToHash(activationLayerName + "." + activationStateName);
			this._activationStateShortHash = Animator.StringToHash(activationStateName);
			if (!this._animator.HasState(this._activationLayerIndex, this._activationStateFullHash) && !this._animator.HasState(this._activationLayerIndex, this._activationStateShortHash))
			{
				Debug.LogError(string.Concat(new string[] { "[TimeFreeze] Animator state '", activationLayerName, ".", activationStateName, "' was not found." }), this);
				this.ReleasePreparedActivationLayer();
				return false;
			}
			int num = Animator.StringToHash(activationLayerName + ".Empty");
			int num2 = Animator.StringToHash("Empty");
			this._activationEmptyStateHash = (this._animator.HasState(this._activationLayerIndex, num) ? num : (this._animator.HasState(this._activationLayerIndex, num2) ? num2 : 0));
			this._savedActivationLayerWeight = this._animator.GetLayerWeight(this._activationLayerIndex);
			this._activationLayerPrepared = true;
			return true;
		}

		// Token: 0x06005988 RID: 22920 RVA: 0x0023019C File Offset: 0x0022E39C
		private void StartArmGesture(float playbackSpeed = 1f)
		{
			if (!this._activationLayerPrepared || this._animator == null || this._settings == null)
			{
				return;
			}
			int num = (this._animator.HasState(this._activationLayerIndex, this._activationStateFullHash) ? this._activationStateFullHash : this._activationStateShortHash);
			this._activationPoseHeld = false;
			this._armGesturePlaybackSpeed = Mathf.Clamp(playbackSpeed, 0.25f, 4f);
			this._animator.SetFloat(this._activationSpeedParameterHash, this._armGesturePlaybackSpeed);
			this._animator.SetLayerWeight(this._activationLayerIndex, this._savedActivationLayerWeight);
			this._animator.Play(num, this._activationLayerIndex, 0f);
			this._actionActive = true;
			this._actionRoutine = base.StartCoroutine(this.RunArmGesture());
		}

		// Token: 0x06005989 RID: 22921 RVA: 0x00230271 File Offset: 0x0022E471
		private IEnumerator RunArmGesture()
		{
			yield return this.BlendActivationLayerWeight(this._savedActivationLayerWeight, 1f, this._settings.activationBlendInDuration);
			double watchdog = Time.realtimeSinceStartupAsDouble + (double)(this._settings.activationClip.length / Mathf.Max(0.25f, this._armGesturePlaybackSpeed)) + (double)this._settings.slowDownDuration + 2.0;
			bool enteredState = false;
			float fadeOutAt = Mathf.Clamp(this._settings.activationFadeOutNormalizedTime, 0.1f, 0.95f);
			while (Time.realtimeSinceStartupAsDouble < watchdog)
			{
				this.UpdateArmGestureSpeed();
				if (this._animator == null)
				{
					break;
				}
				AnimatorStateInfo currentAnimatorStateInfo = this._animator.GetCurrentAnimatorStateInfo(this._activationLayerIndex);
				if (currentAnimatorStateInfo.fullPathHash == this._activationStateFullHash || currentAnimatorStateInfo.shortNameHash == this._activationStateShortHash)
				{
					enteredState = true;
					if (currentAnimatorStateInfo.normalizedTime >= fadeOutAt)
					{
						break;
					}
				}
				else if (enteredState)
				{
					break;
				}
				yield return null;
			}
			this._activationPoseHeld = enteredState;
			if (this._animator != null && this._activationLayerPrepared && this._activationPoseHeld)
			{
				this._animator.SetFloat(this._activationSpeedParameterHash, 0f);
			}
			float num = ((this._animator != null) ? this._animator.GetLayerWeight(this._activationLayerIndex) : 1f);
			yield return this.BlendActivationLayerWeight(num, this._savedActivationLayerWeight, this._settings.activationBlendOutDuration);
			this._actionRoutine = null;
			this.RestoreArmGesture();
			yield break;
		}

		// Token: 0x0600598A RID: 22922 RVA: 0x00230280 File Offset: 0x0022E480
		private IEnumerator BlendActivationLayerWeight(float from, float to, float duration)
		{
			if (this._animator == null || !this._activationLayerPrepared)
			{
				yield break;
			}
			if (duration <= 0.001f)
			{
				this._animator.SetLayerWeight(this._activationLayerIndex, to);
				yield break;
			}
			float elapsed = 0f;
			while (elapsed < duration && this._animator != null && this._activationLayerPrepared)
			{
				elapsed += Time.unscaledDeltaTime;
				float num = Mathf.Clamp01(elapsed / duration);
				this._animator.SetLayerWeight(this._activationLayerIndex, Mathf.LerpUnclamped(from, to, num * num * (3f - 2f * num)));
				this.UpdateArmGestureSpeed();
				yield return null;
			}
			if (this._animator != null && this._activationLayerPrepared)
			{
				this._animator.SetLayerWeight(this._activationLayerIndex, to);
			}
			yield break;
		}

		// Token: 0x0600598B RID: 22923 RVA: 0x002302A4 File Offset: 0x0022E4A4
		private void UpdateArmGestureSpeed()
		{
			if (this._animator == null || !this._activationLayerPrepared)
			{
				return;
			}
			this._animator.SetFloat(this._activationSpeedParameterHash, this._activationPoseHeld ? 0f : this._armGesturePlaybackSpeed);
		}

		// Token: 0x0600598C RID: 22924 RVA: 0x002302E4 File Offset: 0x0022E4E4
		private void RestoreArmGesture()
		{
			if (this._animator != null && this._activationLayerPrepared && this._activationLayerIndex >= 0)
			{
				this._animator.SetFloat(this._activationSpeedParameterHash, 1f);
				if (this._activationEmptyStateHash != 0)
				{
					this._animator.Play(this._activationEmptyStateHash, this._activationLayerIndex, 0f);
				}
				this._animator.SetLayerWeight(this._activationLayerIndex, this._savedActivationLayerWeight);
			}
			this._activationPoseHeld = false;
			this._armGesturePlaybackSpeed = 1f;
			this._actionActive = false;
			this.ReleasePreparedActivationLayer();
		}

		// Token: 0x0600598D RID: 22925 RVA: 0x00230380 File Offset: 0x0022E580
		private void ReleasePreparedActivationLayer()
		{
			this._activationLayerIndex = -1;
			this._activationStateFullHash = 0;
			this._activationStateShortHash = 0;
			this._activationEmptyStateHash = 0;
			this._savedActivationLayerWeight = 0f;
			this._activationPoseHeld = false;
			this._armGesturePlaybackSpeed = 1f;
			this._activationLayerPrepared = false;
		}

		// Token: 0x0600598E RID: 22926 RVA: 0x002303CD File Offset: 0x0022E5CD
		private void HandleItemUnworn(WearableItemSO item)
		{
			if (this._active && item == this._activeItem)
			{
				this.AbortAndRestore();
			}
		}

		// Token: 0x0600598F RID: 22927 RVA: 0x002303EC File Offset: 0x0022E5EC
		private void FinishNormally()
		{
			WearableItemSO activeItem = this._activeItem;
			this._sequenceRoutine = null;
			PlayerActivityLease activityLease = this._activityLease;
			if (activityLease != null)
			{
				activityLease.Dispose();
			}
			this._activityLease = null;
			this._activeItem = null;
			this._active = false;
			TimeFreezeController.s_anyActive = false;
			if (this._hardFreezeCommitted)
			{
				this.ConsumeCycleChargeOnce(activeItem);
			}
			this._stopRequested = false;
			this._closing = false;
			this._hardFreezeCommitted = false;
			WearableUseController wearableUse = this._wearableUse;
			if (wearableUse == null)
			{
				return;
			}
			wearableUse.RefreshFocusedActionsHud();
		}

		// Token: 0x06005990 RID: 22928 RVA: 0x00230468 File Offset: 0x0022E668
		private void AbortAndRestore()
		{
			if (this._cleaningUp || (!this._active && this._world == null && this._worldRamp == null && !this._actionActive))
			{
				return;
			}
			this._cleaningUp = true;
			WearableItemSO activeItem = this._activeItem;
			bool flag = this._hardFreezeCommitted && !this._cycleChargeConsumed;
			if (this._sequenceRoutine != null)
			{
				base.StopCoroutine(this._sequenceRoutine);
			}
			if (this._actionRoutine != null)
			{
				base.StopCoroutine(this._actionRoutine);
			}
			this._sequenceRoutine = null;
			this._actionRoutine = null;
			this.RestoreArmGesture();
			TimeFreezeWorldSnapshot world = this._world;
			if (world != null)
			{
				world.RestoreAll();
			}
			this._world = null;
			TimeFreezeWorldRamp worldRamp = this._worldRamp;
			if (worldRamp != null)
			{
				worldRamp.Restore();
			}
			this._worldRamp = null;
			this.StopFrozenHumImmediate();
			if (this._transitionSource != null)
			{
				this._transitionSource.Stop();
			}
			this.UnregisterHud();
			this.SetVisualWeight(0f);
			PlayerActivityLease activityLease = this._activityLease;
			if (activityLease != null)
			{
				activityLease.Dispose();
			}
			this._activityLease = null;
			this._activeItem = null;
			this._active = false;
			TimeFreezeController.s_anyActive = false;
			if (flag)
			{
				this.ConsumeCycleChargeOnce(activeItem);
			}
			this._stopRequested = false;
			this._closing = false;
			this._hardFreezeCommitted = false;
			this._cycleChargeConsumed = false;
			WearableUseController wearableUse = this._wearableUse;
			if (wearableUse != null)
			{
				wearableUse.RefreshFocusedActionsHud();
			}
			this._cleaningUp = false;
		}

		// Token: 0x06005991 RID: 22929 RVA: 0x002305C8 File Offset: 0x0022E7C8
		private void ConsumeCycleChargeOnce(WearableItemSO item)
		{
			if (this._cycleChargeConsumed || item == null)
			{
				return;
			}
			this._cycleChargeConsumed = true;
			float num;
			if (this._wearableUse == null || !this._wearableUse.TryConsumeWornInventoryDurability(item, 1f, out num))
			{
				Debug.LogError("[TimeFreeze] The completed cycle could not consume its bracelet charge.", this);
				return;
			}
			if (num > 0f)
			{
				ToastService.ShowFormat("gameplay_residuals", "{0} Time Freeze charge(s) remaining.", new object[] { Mathf.CeilToInt(num) }, "Gym Time Bracelet", null, -1f);
			}
		}

		// Token: 0x06005992 RID: 22930 RVA: 0x00230654 File Offset: 0x0022E854
		private void EnsureRuntimeEffects()
		{
			if (this._runtimeRoot == null)
			{
				this._runtimeRoot = new GameObject("[Gym Time Freeze Runtime]")
				{
					hideFlags = HideFlags.DontSave
				};
				this._runtimeRoot.transform.SetParent(base.transform, false);
			}
			if (this._transitionSource == null)
			{
				this._transitionSource = this._runtimeRoot.AddComponent<AudioSource>();
				TimeFreezeController.ConfigureAudioSource(this._transitionSource, false);
			}
			if (this._humSource == null)
			{
				this._humSource = this._runtimeRoot.AddComponent<AudioSource>();
				TimeFreezeController.ConfigureAudioSource(this._humSource, true);
			}
			if (this._settings != null && this._stopClip == null)
			{
				this._stopClip = TimeFreezeController.CreateTransitionClip("GymTimeFreeze_Stop", this._settings.activationDelay + this._settings.slowDownDuration, true, this._settings.activationDelay);
				this._resumeClip = TimeFreezeController.CreateTransitionClip("GymTimeFreeze_Resume", this._settings.resumeDuration, false, 0f);
				this._lockClip = TimeFreezeController.CreateLockClip();
				this._humClip = TimeFreezeController.CreateHumClip();
			}
			this.EnsurePostProcessing();
		}

		// Token: 0x06005993 RID: 22931 RVA: 0x00230780 File Offset: 0x0022E980
		private static void ConfigureAudioSource(AudioSource source, bool loop)
		{
			source.playOnAwake = false;
			source.loop = loop;
			source.spatialBlend = 0f;
			source.ignoreListenerPause = true;
			source.ignoreListenerVolume = false;
			source.dopplerLevel = 0f;
			GameAudioRouter.Route(source, GameAudioRouter.Channel.Sfx);
		}

		// Token: 0x06005994 RID: 22932 RVA: 0x002307BC File Offset: 0x0022E9BC
		private void PlayTransition(bool resuming)
		{
			if (this._transitionSource == null || this._settings == null)
			{
				return;
			}
			this._transitionSource.Stop();
			this._transitionSource.clip = (resuming ? this._resumeClip : this._stopClip);
			this._transitionSource.volume = this._settings.transitionVolume * (resuming ? this._settings.resumeVolumeMultiplier : 1f);
			this._transitionSource.pitch = 1f;
			if (this._transitionSource.clip != null)
			{
				this._transitionSource.Play();
			}
		}

		// Token: 0x06005995 RID: 22933 RVA: 0x00230868 File Offset: 0x0022EA68
		private void PlayLockCue()
		{
			if (this._transitionSource == null || this._lockClip == null || this._settings == null)
			{
				return;
			}
			this._transitionSource.PlayOneShot(this._lockClip, this._settings.lockCueVolume);
		}

		// Token: 0x06005996 RID: 22934 RVA: 0x002308BC File Offset: 0x0022EABC
		private void StartFrozenHum()
		{
			if (this._humSource == null || this._humClip == null || this._settings == null)
			{
				return;
			}
			if (this._humFadeRoutine != null)
			{
				base.StopCoroutine(this._humFadeRoutine);
			}
			this._humSource.clip = this._humClip;
			this._humSource.volume = 0f;
			this._humSource.Play();
			this._humFadeRoutine = base.StartCoroutine(this.FadeHum(0f, this._settings.frozenHumVolume, this._settings.frozenHumFadeDuration, false));
		}

		// Token: 0x06005997 RID: 22935 RVA: 0x00230964 File Offset: 0x0022EB64
		private void FadeOutFrozenHum()
		{
			if (this._humSource == null || !this._humSource.isPlaying || this._settings == null)
			{
				return;
			}
			if (this._humFadeRoutine != null)
			{
				base.StopCoroutine(this._humFadeRoutine);
			}
			this._humFadeRoutine = base.StartCoroutine(this.FadeHum(this._humSource.volume, 0f, this._settings.frozenHumFadeDuration, true));
		}

		// Token: 0x06005998 RID: 22936 RVA: 0x002309DD File Offset: 0x0022EBDD
		private IEnumerator FadeHum(float from, float to, float duration, bool stopAfter)
		{
			if (this._humSource == null)
			{
				yield break;
			}
			if (duration <= 0.001f)
			{
				this._humSource.volume = to;
			}
			else
			{
				float elapsed = 0f;
				while (elapsed < duration && this._humSource != null)
				{
					elapsed += Time.unscaledDeltaTime;
					float num = Mathf.Clamp01(elapsed / duration);
					float num2 = num * num * (3f - 2f * num);
					this._humSource.volume = Mathf.LerpUnclamped(from, to, num2);
					yield return null;
				}
				if (this._humSource != null)
				{
					this._humSource.volume = to;
				}
			}
			if (stopAfter && this._humSource != null)
			{
				this._humSource.Stop();
			}
			this._humFadeRoutine = null;
			yield break;
		}

		// Token: 0x06005999 RID: 22937 RVA: 0x00230A0C File Offset: 0x0022EC0C
		private void StopFrozenHumImmediate()
		{
			if (this._humFadeRoutine != null)
			{
				base.StopCoroutine(this._humFadeRoutine);
			}
			this._humFadeRoutine = null;
			if (this._humSource == null)
			{
				return;
			}
			this._humSource.Stop();
			this._humSource.volume = 0f;
		}

		// Token: 0x0600599A RID: 22938 RVA: 0x00230A60 File Offset: 0x0022EC60
		private static AudioClip CreateTransitionClip(string clipName, float seconds, bool descending, float preRoll)
		{
			seconds = Mathf.Max(0.2f, seconds);
			preRoll = Mathf.Clamp(preRoll, 0f, seconds - 0.05f);
			int num = Mathf.Max(1, Mathf.CeilToInt(seconds * 44100f));
			float[] array = new float[num];
			Random random = new Random(descending ? 1931 : 3191);
			double num2 = 0.0;
			float num3 = 0f;
			for (int i = 0; i < num; i++)
			{
				float num4 = (float)i / 44100f;
				float num5 = Mathf.Max(0.05f, seconds - preRoll);
				float num6 = Mathf.Clamp01((num4 - preRoll) / num5);
				float num7 = num6 * num6 * (3f - 2f * num6);
				float num8 = (descending ? 680f : 55f);
				float num9 = (descending ? 55f : 520f);
				float num10 = num8 * Mathf.Pow(num9 / num8, num7);
				num2 += 6.283185307179586 * (double)num10 / 44100.0;
				float num11 = (float)(random.NextDouble() * 2.0 - 1.0);
				float num12 = (descending ? Mathf.Lerp(0.13f, 0.018f, num7) : Mathf.Lerp(0.018f, 0.11f, num7));
				num3 = Mathf.Lerp(num3, num11, num12);
				float num13 = Mathf.Clamp01(num4 / 0.03f);
				num13 = num13 * num13 * (3f - 2f * num13);
				float num14 = Mathf.Clamp01((seconds - num4) / 0.2f);
				num14 = num14 * num14 * (3f - 2f * num14);
				float num15 = (descending ? Mathf.Lerp(0.2f, 1f, num7) : 1f);
				float num16 = Mathf.Sin((float)num2) * 0.3f;
				float num17 = Mathf.Sin((float)num2 * 1.997f) * 0.045f;
				float num18 = Mathf.Sin((float)num2 * 0.5f) * 0.09f;
				float num19 = num16 + num17 + num18 + num3 * 0.13f;
				array[i] = Mathf.Clamp(num19 * num13 * num14 * num15, -0.68f, 0.68f);
			}
			AudioClip audioClip = AudioClip.Create(clipName, num, 1, 44100, false);
			audioClip.hideFlags = HideFlags.DontSave;
			audioClip.SetData(array, 0);
			return audioClip;
		}

		// Token: 0x0600599B RID: 22939 RVA: 0x00230CB4 File Offset: 0x0022EEB4
		private static AudioClip CreateLockClip()
		{
			int num = Mathf.CeilToInt(8820f);
			float[] array = new float[num];
			double num2 = 0.0;
			for (int i = 0; i < num; i++)
			{
				float num3 = (float)i / 44100f;
				float num4 = Mathf.Clamp01(num3 / 0.2f);
				float num5 = Mathf.Lerp(920f, 360f, num4 * num4);
				num2 += 6.283185307179586 * (double)num5 / 44100.0;
				float num6 = Mathf.Clamp01(num3 / 0.008f);
				float num7 = (1f - num4) * (1f - num4);
				float num8 = Mathf.Sin((float)num2) * 0.24f;
				float num9 = Mathf.Sin(389.5575f * num3) * 0.14f;
				array[i] = (num8 + num9) * num6 * num7;
			}
			AudioClip audioClip = AudioClip.Create("GymTimeFreeze_Lock", num, 1, 44100, false);
			audioClip.hideFlags = HideFlags.DontSave;
			audioClip.SetData(array, 0);
			return audioClip;
		}

		// Token: 0x0600599C RID: 22940 RVA: 0x00230DB0 File Offset: 0x0022EFB0
		private static AudioClip CreateHumClip()
		{
			float[] array = new float[88200];
			for (int i = 0; i < 88200; i++)
			{
				float num = (float)i / 44100f;
				float num2 = 0.76f + Mathf.Sin(3.1415927f * num) * 0.06f;
				array[i] = (Mathf.Sin(345.5752f * num) * 0.38f + Mathf.Sin(691.1504f * num) * 0.12f + Mathf.Sin(1382.3008f * num) * 0.025f) * num2;
			}
			AudioClip audioClip = AudioClip.Create("GymTimeFreeze_Hum", 88200, 1, 44100, false);
			audioClip.hideFlags = HideFlags.DontSave;
			audioClip.SetData(array, 0);
			return audioClip;
		}

		// Token: 0x0600599D RID: 22941 RVA: 0x00230E60 File Offset: 0x0022F060
		private void EnsurePostProcessing()
		{
			if (this._runtimeRoot == null || this._volume != null || this._settings == null)
			{
				return;
			}
			this._volume = this._runtimeRoot.AddComponent<Volume>();
			this._volume.isGlobal = true;
			this._volume.priority = 10000f;
			this._volume.weight = 0f;
			this._volumeProfile = ScriptableObject.CreateInstance<VolumeProfile>();
			this._volumeProfile.name = "GymTimeFreeze_RuntimeVolume";
			this._volumeProfile.hideFlags = HideFlags.DontSave;
			this._volume.sharedProfile = this._volumeProfile;
			this._colorAdjustments = this._volumeProfile.Add<ColorAdjustments>(true);
			this._colorAdjustments.saturation.overrideState = true;
			this._colorAdjustments.saturation.value = this._settings.frozenSaturation;
			this._vignette = this._volumeProfile.Add<Vignette>(true);
			this._vignette.intensity.overrideState = true;
			this._vignette.intensity.value = this._settings.frozenVignette;
			this._vignette.smoothness.overrideState = true;
			this._vignette.smoothness.value = 0.45f;
			this._chromaticAberration = this._volumeProfile.Add<ChromaticAberration>(true);
			this._chromaticAberration.intensity.overrideState = true;
			this._chromaticAberration.intensity.value = this._settings.frozenChromaticAberration;
		}

		// Token: 0x0600599E RID: 22942 RVA: 0x00230FEE File Offset: 0x0022F1EE
		private void SetVisualWeight(float weight)
		{
			if (this._volume != null)
			{
				this._volume.weight = Mathf.Clamp01(weight);
			}
		}

		// Token: 0x0600599F RID: 22943 RVA: 0x00231010 File Offset: 0x0022F210
		private void RegisterHud()
		{
			TimedEffectHUDService instance = TimedEffectHUDService.Instance;
			if (instance == null || this._settings == null)
			{
				return;
			}
			instance.RegisterEffect(new TimedEffectData
			{
				EffectId = "GymTimeFreeze",
				DisplayName = "TIME FROZEN",
				Icon = ((this._braceletDefinition != null) ? this._braceletDefinition.icon : ((this._activeItem != null) ? this._activeItem.icon : null)),
				TotalDuration = Mathf.Min(60f, Mathf.Max(1f, this._settings.frozenDuration)),
				AccentColor = new Color(0.42f, 0.72f, 1f, 1f)
			});
			this._hudRegistered = true;
		}

		// Token: 0x060059A0 RID: 22944 RVA: 0x002310E4 File Offset: 0x0022F2E4
		private void UnregisterHud()
		{
			if (!this._hudRegistered)
			{
				return;
			}
			TimedEffectHUDService instance = TimedEffectHUDService.Instance;
			if (instance != null)
			{
				instance.UnregisterEffect("GymTimeFreeze");
			}
			this._hudRegistered = false;
		}

		// Token: 0x060059A1 RID: 22945 RVA: 0x0023110C File Offset: 0x0022F30C
		private void DestroyRuntimeEffects()
		{
			if (this._stopClip != null)
			{
				Object.Destroy(this._stopClip);
			}
			if (this._resumeClip != null)
			{
				Object.Destroy(this._resumeClip);
			}
			if (this._lockClip != null)
			{
				Object.Destroy(this._lockClip);
			}
			if (this._humClip != null)
			{
				Object.Destroy(this._humClip);
			}
			if (this._volumeProfile != null)
			{
				Object.Destroy(this._volumeProfile);
			}
			if (this._runtimeRoot != null)
			{
				Object.Destroy(this._runtimeRoot);
			}
			this._stopClip = null;
			this._resumeClip = null;
			this._lockClip = null;
			this._humClip = null;
			this._volumeProfile = null;
			this._runtimeRoot = null;
		}

		// Token: 0x04005A15 RID: 23061
		private const string SettingsResourcePath = "TimeFreezeSettings";

		// Token: 0x04005A16 RID: 23062
		private const string BraceletResourcePath = "Wardrobe/Cloth_GymSet_MeterBand";

		// Token: 0x04005A17 RID: 23063
		private const string HudEffectId = "GymTimeFreeze";

		// Token: 0x04005A18 RID: 23064
		private const string ActivityReason = "Gym Time Freeze";

		// Token: 0x04005A19 RID: 23065
		private const string ActivationEmptyStateName = "Empty";

		// Token: 0x04005A1A RID: 23066
		private const string ActivationSpeedParameterName = "TimeFreezeArmsSpeed";

		// Token: 0x04005A1B RID: 23067
		private const PlayerContextResource ActivationResources = PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.FullBodyAnimation | PlayerContextResource.HeldItem | PlayerContextResource.WorldSimulation;

		// Token: 0x04005A1C RID: 23068
		private const PlayerActivity PlayerCompatibleActivities = PlayerActivity.Emote | PlayerActivity.RapidHUD | PlayerActivity.StanceSwitch | PlayerActivity.Crouch | PlayerActivity.ArmsPose;

		// Token: 0x04005A1D RID: 23069
		private const PlayerActivity HardFreezeCompatibleActivities = PlayerActivity.Emote | PlayerActivity.RapidHUD | PlayerActivity.StanceSwitch | PlayerActivity.DebugFly | PlayerActivity.Crouch | PlayerActivity.ArmsPose;

		// Token: 0x04005A1E RID: 23070
		private const PlayerActivity TransitionBlockedActivities = PlayerActivity.Cover | PlayerActivity.Flash | PlayerActivity.Interact | PlayerActivity.Undress | PlayerActivity.LingerieOpen | PlayerActivity.Inventory | PlayerActivity.Phone | PlayerActivity.DebugFly;

		// Token: 0x04005A1F RID: 23071
		private const PlayerActivity FrozenBlockedActivities = PlayerActivity.Cover | PlayerActivity.Flash | PlayerActivity.Interact | PlayerActivity.Undress | PlayerActivity.LingerieOpen | PlayerActivity.Inventory | PlayerActivity.Phone;

		// Token: 0x04005A20 RID: 23072
		private static bool s_anyActive;

		// Token: 0x04005A21 RID: 23073
		private static int s_resumeInputFrame = -1;

		// Token: 0x04005A22 RID: 23074
		private TimeFreezeSettingsSO _settings;

		// Token: 0x04005A23 RID: 23075
		private WearableItemSO _braceletDefinition;

		// Token: 0x04005A24 RID: 23076
		private WearableUseController _wearableUse;

		// Token: 0x04005A25 RID: 23077
		private PlayerActionAnimator _actionAnimator;

		// Token: 0x04005A26 RID: 23078
		private vThirdPersonInput _thirdPersonInput;

		// Token: 0x04005A27 RID: 23079
		private Animator _animator;

		// Token: 0x04005A28 RID: 23080
		private WearableItemSO _activeItem;

		// Token: 0x04005A29 RID: 23081
		private PlayerActivityLease _activityLease;

		// Token: 0x04005A2A RID: 23082
		private TimeFreezeWorldSnapshot _world;

		// Token: 0x04005A2B RID: 23083
		private TimeFreezeWorldRamp _worldRamp;

		// Token: 0x04005A2C RID: 23084
		private Coroutine _sequenceRoutine;

		// Token: 0x04005A2D RID: 23085
		private Coroutine _actionRoutine;

		// Token: 0x04005A2E RID: 23086
		private int _activationLayerIndex = -1;

		// Token: 0x04005A2F RID: 23087
		private int _activationStateFullHash;

		// Token: 0x04005A30 RID: 23088
		private int _activationStateShortHash;

		// Token: 0x04005A31 RID: 23089
		private int _activationEmptyStateHash;

		// Token: 0x04005A32 RID: 23090
		private readonly int _activationSpeedParameterHash = Animator.StringToHash("TimeFreezeArmsSpeed");

		// Token: 0x04005A33 RID: 23091
		private float _savedActivationLayerWeight;

		// Token: 0x04005A34 RID: 23092
		private bool _activationLayerPrepared;

		// Token: 0x04005A35 RID: 23093
		private bool _actionActive;

		// Token: 0x04005A36 RID: 23094
		private bool _activationPoseHeld;

		// Token: 0x04005A37 RID: 23095
		private float _armGesturePlaybackSpeed = 1f;

		// Token: 0x04005A38 RID: 23096
		private bool _active;

		// Token: 0x04005A39 RID: 23097
		private bool _cleaningUp;

		// Token: 0x04005A3A RID: 23098
		private bool _hudRegistered;

		// Token: 0x04005A3B RID: 23099
		private bool _stopRequested;

		// Token: 0x04005A3C RID: 23100
		private bool _closing;

		// Token: 0x04005A3D RID: 23101
		private bool _hardFreezeCommitted;

		// Token: 0x04005A3E RID: 23102
		private bool _cycleChargeConsumed;

		// Token: 0x04005A3F RID: 23103
		private int _lastAcceptedToggleFrame = -1;

		// Token: 0x04005A40 RID: 23104
		private GameObject _runtimeRoot;

		// Token: 0x04005A41 RID: 23105
		private AudioSource _transitionSource;

		// Token: 0x04005A42 RID: 23106
		private AudioSource _humSource;

		// Token: 0x04005A43 RID: 23107
		private AudioClip _stopClip;

		// Token: 0x04005A44 RID: 23108
		private AudioClip _resumeClip;

		// Token: 0x04005A45 RID: 23109
		private AudioClip _lockClip;

		// Token: 0x04005A46 RID: 23110
		private AudioClip _humClip;

		// Token: 0x04005A47 RID: 23111
		private Coroutine _humFadeRoutine;

		// Token: 0x04005A48 RID: 23112
		private Volume _volume;

		// Token: 0x04005A49 RID: 23113
		private VolumeProfile _volumeProfile;

		// Token: 0x04005A4A RID: 23114
		private ColorAdjustments _colorAdjustments;

		// Token: 0x04005A4B RID: 23115
		private Vignette _vignette;

		// Token: 0x04005A4C RID: 23116
		private ChromaticAberration _chromaticAberration;
	}
}
