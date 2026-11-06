using System;
using System.Collections.Generic;
using System.Globalization;
using Features.DebugTools;
using Features.Detection;
using Features.PlayerEmotes;
using Invector.vCharacterController;
using ProjectExhibitionist;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace Features.Gym
{
	// Token: 0x020011AE RID: 4526
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(-50)]
	public sealed class GymExerciseStation : MonoBehaviour, IInteractionCandidate
	{
		// Token: 0x17001624 RID: 5668
		// (get) Token: 0x06008ABC RID: 35516 RVA: 0x0038AFED File Offset: 0x003891ED
		public static bool ControllerOwnsShoulders
		{
			get
			{
				return GymExerciseStation._controllerStation != null && GymExerciseStation._controllerStation.isActiveAndEnabled && GymExerciseStation._controllerStation._exerciseActive;
			}
		}

		// Token: 0x06008ABD RID: 35517 RVA: 0x0038B014 File Offset: 0x00389214
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetControllerStation()
		{
			GymExerciseStation._controllerStation = null;
		}

		// Token: 0x17001625 RID: 5669
		// (get) Token: 0x06008ABE RID: 35518 RVA: 0x0038B01C File Offset: 0x0038921C
		public float CandidateSqrDistance
		{
			get
			{
				return this._sqrDistance;
			}
		}

		// Token: 0x17001626 RID: 5670
		// (get) Token: 0x06008ABF RID: 35519 RVA: 0x0038B024 File Offset: 0x00389224
		public bool IsCandidateEligible
		{
			get
			{
				return !this._exerciseActive && !this._ambientOccupied && this._player != null && this._emoteController != null && this._emoteController.CurrentState == GameplayEmoteController.EmoteState.Idle && this.characterAnchor != null && this.SelectedExercise != null && this.HasRequiredPoi && this._sqrDistance <= this.detectionRadius * this.detectionRadius && GameplayEmoteController.CanStartContextualEmote;
			}
		}

		// Token: 0x17001627 RID: 5671
		// (get) Token: 0x06008AC0 RID: 35520 RVA: 0x000FCE88 File Offset: 0x000FB088
		public int CandidateInstanceId
		{
			get
			{
				return base.GetInstanceID();
			}
		}

		// Token: 0x17001628 RID: 5672
		// (get) Token: 0x06008AC1 RID: 35521 RVA: 0x0038B0AA File Offset: 0x003892AA
		public bool IsExerciseActive
		{
			get
			{
				return this._exerciseActive;
			}
		}

		// Token: 0x17001629 RID: 5673
		// (get) Token: 0x06008AC2 RID: 35522 RVA: 0x0038B0B2 File Offset: 0x003892B2
		public bool IsAmbientOccupied
		{
			get
			{
				return this._ambientOccupied;
			}
		}

		// Token: 0x06008AC3 RID: 35523 RVA: 0x0038B0BA File Offset: 0x003892BA
		public void SetAmbientOccupied(bool occupied)
		{
			if (this._ambientOccupied == occupied)
			{
				return;
			}
			this._ambientOccupied = occupied;
			if (occupied)
			{
				this.HideIdlePrompt();
				this.HideOwnedHud();
			}
		}

		// Token: 0x1700162A RID: 5674
		// (get) Token: 0x06008AC4 RID: 35524 RVA: 0x0038B0DC File Offset: 0x003892DC
		private EmoteDefinitionSO SelectedExercise
		{
			get
			{
				if (this.exercises == null || this.exercises.Count == 0 || this._selectedExerciseIndex < 0 || this._selectedExerciseIndex >= this.exercises.Count)
				{
					return null;
				}
				return this.exercises[this._selectedExerciseIndex];
			}
		}

		// Token: 0x1700162B RID: 5675
		// (get) Token: 0x06008AC5 RID: 35525 RVA: 0x0038B12D File Offset: 0x0038932D
		private bool HasRequiredPoi
		{
			get
			{
				return this._poiContext != null && this._poiContext.IsInPoi(this.requiredPoiId);
			}
		}

		// Token: 0x06008AC6 RID: 35526 RVA: 0x0038B150 File Offset: 0x00389350
		private void Awake()
		{
			ControllerContextBindings.Set(this.startInput, "X");
			ControllerContextBindings.Set(this.changeExerciseInput, "Y");
			ControllerContextBindings.Set(this.endInput, "B");
			ControllerContextBindings.Set(this.exerciseCameraInput, "RightStickClick");
			this._treadmillIntensityIndex = Mathf.Clamp(this.treadmillDefaultIntensityIndex, 0, GymExerciseStation.ExerciseIntensityMultipliers.Length - 1);
			this.EnsureValidSelection();
			if (this.promptUI == null)
			{
				this.promptUI = base.GetComponentInChildren<InteractablePromptUI>(true);
			}
			if (this.promptUI != null)
			{
				this.promptUI.MatchGymPlacement(base.transform, 0.008525f, 0.3f);
				this.promptUI.Hide();
			}
			this.BuildRuntimePromptConfig();
		}

		// Token: 0x06008AC7 RID: 35527 RVA: 0x0038B214 File Offset: 0x00389414
		private void OnEnable()
		{
			this._sqrDistance = float.MaxValue;
			this._nextPlayerResolveTime = 0f;
			InteractionCandidateRegistry.Register(this);
			this.ResolvePlayerReferences();
			if (this.beltScroller != null)
			{
				this.beltScroller.SetRunning(false);
			}
		}

		// Token: 0x06008AC8 RID: 35528 RVA: 0x0038B254 File Offset: 0x00389454
		private void OnDisable()
		{
			InteractionCandidateRegistry.Unregister(this);
			FlyCamera.StopExerciseCamera(this);
			this.HideIdlePrompt();
			this.HideOwnedHud();
			if (this._exerciseActive)
			{
				this.RequestEndExercise(true);
				this.FinishExerciseState();
			}
			else if (this.beltScroller != null)
			{
				this.beltScroller.SetRunning(false);
			}
			this.RestoreEquipmentProps();
			this.RestorePlayerPhysics(true);
			this.UnsubscribeFromPlayer();
			this._player = null;
			this._playerInput = null;
			this._playerAnimator = null;
			this._poiContext = null;
			this._sqrDistance = float.MaxValue;
		}

		// Token: 0x06008AC9 RID: 35529 RVA: 0x0038B2E4 File Offset: 0x003894E4
		private void OnDestroy()
		{
			this.RestoreEquipmentProps();
			this.RestorePlayerPhysics(true);
			if (this._runtimeHudConfig != null)
			{
				Object.Destroy(this._runtimeHudConfig);
				this._runtimeHudConfig = null;
			}
			if (this._runtimePromptConfig != null)
			{
				Object.Destroy(this._runtimePromptConfig);
				this._runtimePromptConfig = null;
			}
		}

		// Token: 0x06008ACA RID: 35530 RVA: 0x0038B340 File Offset: 0x00389540
		private void Update()
		{
			if (this._exerciseActive)
			{
				this.UpdateActiveExercise();
				return;
			}
			if (this._ambientOccupied)
			{
				this._sqrDistance = float.MaxValue;
				this.HideIdlePrompt();
				this.HideOwnedHud();
				return;
			}
			if (!this.EnsurePlayerReferences())
			{
				this._sqrDistance = float.MaxValue;
				this.HideIdlePrompt();
				this.HideOwnedHud();
				return;
			}
			Vector3 vector = ((this.characterAnchor != null) ? this.characterAnchor.position : base.transform.position);
			this._sqrDistance = (vector - this._player.position).sqrMagnitude;
			if (!this.IsCandidateEligible || !InteractionCandidateRegistry.IsWinner(this))
			{
				this.HideIdlePrompt();
				return;
			}
			this.ShowIdlePrompt();
			if (this.changeExerciseInput != null && this.changeExerciseInput.GetButtonDown() && this.SelectRelative(1))
			{
				this.RefreshIdlePrompt();
			}
			if (this.startInput != null && this.startInput.GetButtonDown())
			{
				this.TryStartSelectedExercise();
			}
		}

		// Token: 0x06008ACB RID: 35531 RVA: 0x0038B444 File Offset: 0x00389644
		private void UpdateActiveExercise()
		{
			this.PinTreadmillPlayerToAnchor();
			if (this._emoteController == null || !this._emoteController.IsPlaying || this._emoteController.PlayingEmote != this._activeExercise)
			{
				this.FinishExerciseState();
				return;
			}
			this.UpdateExerciseIntensityInput();
			if (this.exerciseCameraInput != null && this.exerciseCameraInput.GetButtonDown())
			{
				FlyCamera.ToggleExerciseCamera(this._player, this);
				if (FlyCamera.IsExerciseCameraOwnedBy(this))
				{
					this.HideOwnedHud();
				}
				else
				{
					this.RefreshOwnedHud(true);
				}
			}
			if (!this.HasRequiredPoi)
			{
				this.RequestEndExercise(false);
				return;
			}
			if (this._endingExercise)
			{
				this.HideOwnedHud();
				return;
			}
			if (FlyCamera.IsExerciseCameraOwnedBy(this))
			{
				this.HideOwnedHud();
				if (ControllerInput.IsActive)
				{
					return;
				}
			}
			else
			{
				this.ShowActiveHud();
			}
			if (this._waitForEndRelease)
			{
				if (this.endInput == null || !this.endInput.GetButton())
				{
					this._waitForEndRelease = false;
				}
				return;
			}
			if (this.endInput != null && this.endInput.GetButtonDown())
			{
				this.RequestEndExercise(false);
			}
		}

		// Token: 0x06008ACC RID: 35532 RVA: 0x0038B54C File Offset: 0x0038974C
		private void LateUpdate()
		{
			this.PinTreadmillPlayerToAnchor();
		}

		// Token: 0x06008ACD RID: 35533 RVA: 0x0038B554 File Offset: 0x00389754
		private void PinTreadmillPlayerToAnchor()
		{
			if (!this._exerciseActive || this._endingExercise || this.beltScroller == null || this._player == null || this.characterAnchor == null)
			{
				return;
			}
			Quaternion quaternion = Quaternion.Euler(0f, this.characterAnchor.eulerAngles.y, 0f);
			Rigidbody rigidbody = ((this._overriddenPlayerBody != null) ? this._overriddenPlayerBody : this._player.GetComponent<Rigidbody>());
			if (rigidbody != null)
			{
				rigidbody.position = this.characterAnchor.position;
				rigidbody.rotation = quaternion;
				rigidbody.velocity = Vector3.zero;
				rigidbody.angularVelocity = Vector3.zero;
			}
			this._player.SetPositionAndRotation(this.characterAnchor.position, quaternion);
		}

		// Token: 0x06008ACE RID: 35534 RVA: 0x0038B62C File Offset: 0x0038982C
		private void SuppressTreadmillRootMotion()
		{
			if (this.beltScroller == null || this._treadmillRootMotionOverridden || this._playerInput == null || this._playerInput.cc == null || this._playerAnimator == null)
			{
				return;
			}
			this._savedTreadmillControllerUseRootMotion = this._playerInput.cc.useRootMotion;
			this._savedTreadmillAnimatorApplyRootMotion = this._playerAnimator.applyRootMotion;
			this._playerInput.cc.useRootMotion = false;
			this._playerAnimator.applyRootMotion = false;
			this._treadmillRootMotionOverridden = true;
		}

		// Token: 0x06008ACF RID: 35535 RVA: 0x0038B6CC File Offset: 0x003898CC
		private void RestoreTreadmillRootMotion()
		{
			if (!this._treadmillRootMotionOverridden)
			{
				return;
			}
			if (this._playerInput != null && this._playerInput.cc != null)
			{
				this._playerInput.cc.useRootMotion = this._savedTreadmillControllerUseRootMotion;
			}
			if (this._playerAnimator != null)
			{
				this._playerAnimator.applyRootMotion = this._savedTreadmillAnimatorApplyRootMotion;
			}
			this._treadmillRootMotionOverridden = false;
		}

		// Token: 0x06008AD0 RID: 35536 RVA: 0x0038B740 File Offset: 0x00389940
		private bool EnsurePlayerReferences()
		{
			if (this._player != null && this._playerInput != null && this._playerAnimator != null && this._emoteController != null && this._poiContext != null)
			{
				return true;
			}
			if (Time.unscaledTime < this._nextPlayerResolveTime)
			{
				return false;
			}
			this._nextPlayerResolveTime = Time.unscaledTime + 1f;
			this.ResolvePlayerReferences();
			return this._player != null && this._playerInput != null && this._playerAnimator != null && this._emoteController != null && this._poiContext != null;
		}

		// Token: 0x06008AD1 RID: 35537 RVA: 0x0038B804 File Offset: 0x00389A04
		private void ResolvePlayerReferences()
		{
			GameObject gameObject = PlayerRegistry.PlayerObject;
			if (gameObject == null)
			{
				vThirdPersonController vThirdPersonController = Object.FindObjectOfType<vThirdPersonController>();
				if (vThirdPersonController != null)
				{
					gameObject = vThirdPersonController.gameObject;
				}
			}
			if (gameObject == null)
			{
				return;
			}
			GameplayEmoteController gameplayEmoteController = gameObject.GetComponent<GameplayEmoteController>() ?? gameObject.GetComponentInChildren<GameplayEmoteController>(true);
			PlayerPoiEmoteContext playerPoiEmoteContext = gameObject.GetComponent<PlayerPoiEmoteContext>() ?? gameObject.GetComponentInChildren<PlayerPoiEmoteContext>(true);
			vThirdPersonInput vThirdPersonInput = gameObject.GetComponent<vThirdPersonInput>() ?? gameObject.GetComponentInChildren<vThirdPersonInput>(true);
			Animator animator = ((vThirdPersonInput != null && vThirdPersonInput.cc != null) ? vThirdPersonInput.cc.animator : gameObject.GetComponentInChildren<Animator>(true));
			SkinDecalController skinDecalController = gameObject.GetComponent<SkinDecalController>() ?? gameObject.GetComponentInChildren<SkinDecalController>(true);
			GymExerciseSweatController gymExerciseSweatController = gameObject.GetComponent<GymExerciseSweatController>() ?? gameObject.GetComponentInChildren<GymExerciseSweatController>(true);
			if (gymExerciseSweatController == null && skinDecalController != null && this.sweatLightDecal != null)
			{
				gymExerciseSweatController = gameObject.AddComponent<GymExerciseSweatController>();
			}
			if (gymExerciseSweatController != null)
			{
				gymExerciseSweatController.Configure(skinDecalController, this.sweatLightDecal, this.sweatDelayMinSeconds, this.sweatDelayMaxSeconds, this.sweatFadeInSeconds, this.sweatPeakOpacity, this.sweatFadeOutSeconds);
			}
			if (this._emoteController != gameplayEmoteController)
			{
				this.UnsubscribeFromPlayer();
				this._emoteController = gameplayEmoteController;
				if (this._emoteController != null)
				{
					this._emoteController.OnEmoteEnded += this.HandleEmoteEnded;
				}
			}
			this._player = gameObject.transform;
			this._playerInput = vThirdPersonInput;
			this._playerAnimator = animator;
			this._poiContext = playerPoiEmoteContext;
			this._sweatController = gymExerciseSweatController;
		}

		// Token: 0x06008AD2 RID: 35538 RVA: 0x0038B997 File Offset: 0x00389B97
		private void UnsubscribeFromPlayer()
		{
			if (this._emoteController != null)
			{
				this._emoteController.OnEmoteEnded -= this.HandleEmoteEnded;
			}
			this._emoteController = null;
			this._sweatController = null;
		}

		// Token: 0x06008AD3 RID: 35539 RVA: 0x0038B9CC File Offset: 0x00389BCC
		private void TryStartSelectedExercise()
		{
			EmoteDefinitionSO selectedExercise = this.SelectedExercise;
			if (selectedExercise == null || !this.IsCandidateEligible || !InteractionCandidateRegistry.IsWinner(this) || !InteractionCandidateRegistry.TryCommitWorldInteraction())
			{
				return;
			}
			Vector3 position = this._player.position;
			Quaternion rotation = this._player.rotation;
			this._entryPosition = position;
			this._entryRotation = rotation;
			this._hasEntryPose = true;
			this.SuspendPlayerPhysics();
			this.SnapPlayerToAnchor();
			if (!this._emoteController.TryPlayContextualEmote(selectedExercise, true))
			{
				this.ApplyPlayerPose(position, rotation);
				this.RestorePlayerPhysics(false);
				this._hasEntryPose = false;
				if (this.debugLogs)
				{
					Debug.LogWarning("[GymExerciseStation] Could not start '" + GymExerciseStation.ExerciseName(selectedExercise) + "'.", this);
				}
				return;
			}
			this._activeExercise = selectedExercise;
			this._exerciseActive = true;
			GymExerciseStation._controllerStation = this;
			this._endingExercise = false;
			this._waitForEndRelease = true;
			this.SuppressTreadmillRootMotion();
			this.HideIdlePrompt();
			this.HideEquipmentProps();
			if (this._sweatController != null)
			{
				this._sweatController.BeginExercise();
			}
			if (this.beltScroller != null)
			{
				this.beltScroller.SetRunning(true);
			}
			this.ApplyExerciseIntensity();
			this.RefreshOwnedHud(true);
			if (this.debugLogs)
			{
				Debug.Log("[GymExerciseStation] Started '" + GymExerciseStation.ExerciseName(selectedExercise) + "'.", this);
			}
		}

		// Token: 0x06008AD4 RID: 35540 RVA: 0x0038BB1C File Offset: 0x00389D1C
		private void RequestEndExercise(bool immediate)
		{
			if (!this._exerciseActive || (this._endingExercise && !immediate))
			{
				return;
			}
			FlyCamera.StopExerciseCamera(this);
			this._endingExercise = true;
			this._waitForEndRelease = false;
			if (this.beltScroller != null)
			{
				this.beltScroller.SetRunning(false);
			}
			this.HideOwnedHud();
			if (this._emoteController != null && this._emoteController.IsPlaying && this._emoteController.PlayingEmote == this._activeExercise)
			{
				if (immediate)
				{
					this._emoteController.ForceEndEmoteImmediate();
				}
				else
				{
					this._emoteController.EndEmote();
				}
			}
			if (this._emoteController == null || !this._emoteController.IsPlaying)
			{
				this.FinishExerciseState();
			}
		}

		// Token: 0x06008AD5 RID: 35541 RVA: 0x0038BBE0 File Offset: 0x00389DE0
		private void HandleEmoteEnded(EmoteDefinitionSO endedEmote)
		{
			if (!this._exerciseActive)
			{
				return;
			}
			if (endedEmote != null && this._activeExercise != null && endedEmote != this._activeExercise)
			{
				return;
			}
			this.FinishExerciseState();
		}

		// Token: 0x06008AD6 RID: 35542 RVA: 0x0038BC18 File Offset: 0x00389E18
		private void FinishExerciseState()
		{
			FlyCamera.StopExerciseCamera(this);
			this.ResetExerciseIntensity();
			if (this.beltScroller != null)
			{
				this.beltScroller.SetRunning(false);
			}
			this.RestoreTreadmillRootMotion();
			this.RestoreEquipmentProps();
			if (this._sweatController != null)
			{
				this._sweatController.EndExercise();
			}
			this.RestorePlayerPhysics(true);
			this.HideOwnedHud();
			this._activeExercise = null;
			this._exerciseActive = false;
			if (GymExerciseStation._controllerStation == this)
			{
				GymExerciseStation._controllerStation = null;
			}
			this._endingExercise = false;
			this._waitForEndRelease = false;
		}

		// Token: 0x06008AD7 RID: 35543 RVA: 0x0038BCAC File Offset: 0x00389EAC
		private void SnapPlayerToAnchor()
		{
			if (this._player == null || this.characterAnchor == null)
			{
				return;
			}
			Quaternion quaternion = Quaternion.Euler(0f, this.characterAnchor.eulerAngles.y, 0f);
			this.ApplyPlayerPose(this.characterAnchor.position, quaternion);
		}

		// Token: 0x06008AD8 RID: 35544 RVA: 0x0038BD08 File Offset: 0x00389F08
		private void ApplyPlayerPose(Vector3 position, Quaternion rotation)
		{
			if (this._player == null)
			{
				return;
			}
			global::CharacterController component = this._player.GetComponent<global::CharacterController>();
			bool flag = component != null && component.enabled;
			if (flag)
			{
				component.enabled = false;
			}
			Rigidbody component2 = this._player.GetComponent<Rigidbody>();
			if (component2 != null)
			{
				if (!component2.isKinematic)
				{
					component2.velocity = Vector3.zero;
					component2.angularVelocity = Vector3.zero;
				}
				component2.position = position;
				component2.rotation = rotation;
			}
			this._player.SetPositionAndRotation(position, rotation);
			if (flag)
			{
				component.enabled = true;
			}
			Physics.SyncTransforms();
		}

		// Token: 0x06008AD9 RID: 35545 RVA: 0x0038BDA8 File Offset: 0x00389FA8
		private void SuspendPlayerPhysics()
		{
			this.RestorePlayerPhysics(false);
			if (this._player == null)
			{
				return;
			}
			this._playerColliderStates.Clear();
			foreach (Collider collider in this._player.GetComponents<Collider>())
			{
				if (!(collider == null) && !collider.isTrigger)
				{
					this._playerColliderStates.Add(new GymExerciseStation.ColliderState
					{
						Collider = collider,
						WasEnabled = collider.enabled
					});
					if (collider.enabled)
					{
						collider.enabled = false;
					}
				}
			}
			this._overriddenPlayerBody = this._player.GetComponent<Rigidbody>();
			if (this._overriddenPlayerBody != null)
			{
				this._savedBodyUseGravity = this._overriddenPlayerBody.useGravity;
				this._savedBodyIsKinematic = this._overriddenPlayerBody.isKinematic;
				this._savedBodyDetectCollisions = this._overriddenPlayerBody.detectCollisions;
				this._overriddenPlayerBody.velocity = Vector3.zero;
				this._overriddenPlayerBody.angularVelocity = Vector3.zero;
				this._overriddenPlayerBody.detectCollisions = false;
				this._overriddenPlayerBody.useGravity = false;
				this._overriddenPlayerBody.isKinematic = true;
			}
			this._playerPhysicsOverridden = true;
		}

		// Token: 0x06008ADA RID: 35546 RVA: 0x0038BEDC File Offset: 0x0038A0DC
		private void RestorePlayerPhysics(bool returnToEntryPose)
		{
			if (!this._playerPhysicsOverridden)
			{
				if (returnToEntryPose)
				{
					this._hasEntryPose = false;
				}
				return;
			}
			if (returnToEntryPose && this._hasEntryPose && this._player != null)
			{
				this.ApplyPlayerPose(this._entryPosition, this._entryRotation);
			}
			if (this._overriddenPlayerBody != null)
			{
				this._overriddenPlayerBody.isKinematic = this._savedBodyIsKinematic;
				this._overriddenPlayerBody.useGravity = this._savedBodyUseGravity;
				this._overriddenPlayerBody.detectCollisions = this._savedBodyDetectCollisions;
			}
			for (int i = 0; i < this._playerColliderStates.Count; i++)
			{
				GymExerciseStation.ColliderState colliderState = this._playerColliderStates[i];
				if (colliderState.Collider != null)
				{
					colliderState.Collider.enabled = colliderState.WasEnabled;
				}
			}
			this._playerColliderStates.Clear();
			this._overriddenPlayerBody = null;
			this._playerPhysicsOverridden = false;
			this._hasEntryPose = false;
			Physics.SyncTransforms();
		}

		// Token: 0x06008ADB RID: 35547 RVA: 0x0038BFD0 File Offset: 0x0038A1D0
		private void EnsureValidSelection()
		{
			if (this.SelectedExercise != null)
			{
				return;
			}
			if (this.exercises == null)
			{
				this.exercises = new List<EmoteDefinitionSO>();
			}
			this._selectedExerciseIndex = 0;
			for (int i = 0; i < this.exercises.Count; i++)
			{
				if (!(this.exercises[i] == null))
				{
					this._selectedExerciseIndex = i;
					return;
				}
			}
		}

		// Token: 0x06008ADC RID: 35548 RVA: 0x0038C038 File Offset: 0x0038A238
		private bool SelectRelative(int direction)
		{
			if (this.exercises == null || this.exercises.Count < 2 || this.CountValidExercises() < 2)
			{
				return false;
			}
			int num = ((direction < 0) ? (-1) : 1);
			int num2 = this._selectedExerciseIndex;
			for (int i = 0; i < this.exercises.Count; i++)
			{
				num2 = (num2 + num + this.exercises.Count) % this.exercises.Count;
				if (!(this.exercises[num2] == null))
				{
					this._selectedExerciseIndex = num2;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008ADD RID: 35549 RVA: 0x0038C0C8 File Offset: 0x0038A2C8
		private int CountValidExercises()
		{
			if (this.exercises == null)
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < this.exercises.Count; i++)
			{
				if (this.exercises[i] != null)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06008ADE RID: 35550 RVA: 0x0038C110 File Offset: 0x0038A310
		private void ShowIdlePrompt()
		{
			if (this.promptUI == null)
			{
				return;
			}
			if (!this._promptPresented || !this.promptUI.gameObject.activeInHierarchy)
			{
				this.RefreshIdlePrompt();
			}
		}

		// Token: 0x06008ADF RID: 35551 RVA: 0x0038C141 File Offset: 0x0038A341
		private void RefreshIdlePrompt()
		{
			if (this.promptUI == null)
			{
				return;
			}
			this.BuildRuntimePromptConfig();
			this.promptUI.Show(this._runtimePromptConfig);
			this._promptPresented = true;
		}

		// Token: 0x06008AE0 RID: 35552 RVA: 0x0038C170 File Offset: 0x0038A370
		private void HideIdlePrompt()
		{
			if (this.promptUI != null)
			{
				this.promptUI.Hide();
			}
			this._promptPresented = false;
		}

		// Token: 0x06008AE1 RID: 35553 RVA: 0x0038C192 File Offset: 0x0038A392
		private void ShowActiveHud()
		{
			if (!this._hudPresented || InteractionHUDController.Instance == null || InteractionHUDController.Instance.CurrentConfig != this._runtimeHudConfig)
			{
				this.RefreshOwnedHud(true);
			}
		}

		// Token: 0x06008AE2 RID: 35554 RVA: 0x0038C1C8 File Offset: 0x0038A3C8
		private void RefreshOwnedHud(bool isPlaying)
		{
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (instance == null)
			{
				return;
			}
			this.BuildRuntimeHudConfig(isPlaying);
			instance.Show(this._runtimeHudConfig);
			this._hudPresented = true;
		}

		// Token: 0x06008AE3 RID: 35555 RVA: 0x0038C200 File Offset: 0x0038A400
		private void HideOwnedHud()
		{
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (instance != null)
			{
				instance.HideIfCurrent(this._runtimeHudConfig);
			}
			this._hudPresented = false;
		}

		// Token: 0x06008AE4 RID: 35556 RVA: 0x0038C230 File Offset: 0x0038A430
		private void BuildRuntimeHudConfig(bool isPlaying)
		{
			if (this._runtimeHudConfig == null)
			{
				this._runtimeHudConfig = ScriptableObject.CreateInstance<InteractionConfigSO>();
			}
			ControllerContextBindings.SetLabel(this._runtimeHudConfig, this.startKeyLabel, "X", false);
			ControllerContextBindings.SetLabel(this._runtimeHudConfig, this.endKeyLabel, "B", false);
			ControllerContextBindings.SetLabel(this._runtimeHudConfig, "F", "Y", false);
			ControllerContextBindings.SetLabel(this._runtimeHudConfig, "K", "R3", false);
			ControllerContextBindings.SetLabel(this._runtimeHudConfig, "CTRL + Wheel", "LB / RB", false);
			string text = GymExerciseStation.ExerciseName(isPlaying ? this._activeExercise : this.SelectedExercise);
			this._runtimeHudConfig.objectName = (string.IsNullOrWhiteSpace(this.stationDisplayName) ? "Gym station" : this.stationDisplayName);
			this._runtimeHudConfig.mainActionName = string.Empty;
			this._runtimeHudConfig.mainKeyDisplayLabel = string.Empty;
			this._runtimeHudConfig.usableActions = this._hudActions.Actions;
			this._hudActions.Begin();
			if (!isPlaying && this.CountValidExercises() == 1 && this.beltScroller == null)
			{
				this._hudActions.Add(this.startKeyLabel, text, "");
				this._hudActions.End();
				return;
			}
			this._hudActions.Add(isPlaying ? "Active" : "Selected", text, "");
			if (isPlaying)
			{
				if (this._cachedHudIntensityIndex != this._treadmillIntensityIndex)
				{
					this._cachedHudIntensityIndex = this._treadmillIntensityIndex;
					this._cachedHudIntensityLabel = string.Concat(new string[]
					{
						"Intensity: ",
						GymExerciseStation.ExerciseIntensityNames[this._treadmillIntensityIndex],
						" (x",
						GymExerciseStation.ExerciseIntensityMultipliers[this._treadmillIntensityIndex].ToString("0.00", CultureInfo.InvariantCulture),
						")"
					});
				}
				this._hudActions.Add("CTRL + Wheel", this._cachedHudIntensityLabel, "");
				this._hudActions.Add("K", "Exercise camera: enter / exit", "");
				this._hudActions.Add(this.endKeyLabel, "End exercise", "");
				this._hudActions.End();
				return;
			}
			if (this.CountValidExercises() > 1)
			{
				this._hudActions.Add("F", "Change exercise", "");
			}
			this._hudActions.Add(this.startKeyLabel, "Start exercise", "");
			this._hudActions.End();
		}

		// Token: 0x06008AE5 RID: 35557 RVA: 0x0038C4C4 File Offset: 0x0038A6C4
		private void BuildRuntimePromptConfig()
		{
			if (this._runtimePromptConfig == null)
			{
				this._runtimePromptConfig = ScriptableObject.CreateInstance<InteractionConfigSO>();
			}
			ControllerContextBindings.SetLabel(this._runtimePromptConfig, "F", "Y", true);
			this._runtimePromptConfig.objectName = (string.IsNullOrWhiteSpace(this.stationDisplayName) ? "Gym station" : this.stationDisplayName);
			this._runtimePromptConfig.objectNameColor = Color.white;
			this._runtimePromptConfig.mainActionName = GymExerciseStation.ExerciseName(this.SelectedExercise);
			this._runtimePromptConfig.mainKeyDisplayLabel = this.startKeyLabel;
			bool flag = this.CountValidExercises() > 1;
			this._runtimePromptConfig.secondaryActionName = (flag ? "Change exercise" : string.Empty);
			this._runtimePromptConfig.secondaryKeyDisplayLabel = (flag ? "F" : string.Empty);
			this._runtimePromptConfig.usableActions.Clear();
		}

		// Token: 0x06008AE6 RID: 35558 RVA: 0x0038C5AA File Offset: 0x0038A7AA
		private static string ExerciseName(EmoteDefinitionSO exercise)
		{
			if (exercise == null)
			{
				return "No exercise configured";
			}
			if (!string.IsNullOrWhiteSpace(exercise.DisplayName))
			{
				return exercise.DisplayName;
			}
			if (!string.IsNullOrWhiteSpace(exercise.EmoteId))
			{
				return exercise.EmoteId;
			}
			return exercise.name;
		}

		// Token: 0x06008AE7 RID: 35559 RVA: 0x0038C5EC File Offset: 0x0038A7EC
		private void UpdateExerciseIntensityInput()
		{
			if (FlyCamera.IsExerciseCameraOwnedBy(this))
			{
				return;
			}
			bool flag = ControllerInput.IsActive || GameplayInput.GetKey(KeyCode.LeftControl) || GameplayInput.GetKey(KeyCode.RightControl);
			if (this._playerInput != null)
			{
				this._playerInput.suppressCameraZoom = flag;
			}
			if (!flag)
			{
				return;
			}
			float num = (ControllerInput.IsActive ? ((ControllerWorldPointer.Down(ControllerButton.RightShoulder) ? 1f : 0f) - (ControllerWorldPointer.Down(ControllerButton.LeftShoulder) ? 1f : 0f)) : GameplayInput.GetAxisRaw("Mouse ScrollWheel"));
			if (Mathf.Approximately(num, 0f))
			{
				return;
			}
			int num2 = Mathf.Clamp(this._treadmillIntensityIndex + ((num > 0f) ? 1 : (-1)), 0, GymExerciseStation.ExerciseIntensityMultipliers.Length - 1);
			if (num2 == this._treadmillIntensityIndex)
			{
				return;
			}
			this._treadmillIntensityIndex = num2;
			this.ApplyExerciseIntensity();
			this.RefreshOwnedHud(true);
		}

		// Token: 0x06008AE8 RID: 35560 RVA: 0x0038C6D0 File Offset: 0x0038A8D0
		private void ApplyExerciseIntensity()
		{
			float num = GymExerciseStation.ExerciseIntensityMultipliers[this._treadmillIntensityIndex];
			if (this._playerAnimator != null)
			{
				this._playerAnimator.SetFloat(GymExerciseStation.EmoteSpeedMultiplierHash, num);
			}
			if (this.beltScroller != null)
			{
				this.beltScroller.SetSpeedMultiplier(num);
			}
		}

		// Token: 0x06008AE9 RID: 35561 RVA: 0x0038C724 File Offset: 0x0038A924
		private void ResetExerciseIntensity()
		{
			if (this._playerAnimator != null)
			{
				this._playerAnimator.SetFloat(GymExerciseStation.EmoteSpeedMultiplierHash, 1f);
			}
			if (this._playerInput != null)
			{
				this._playerInput.suppressCameraZoom = false;
			}
			if (this.beltScroller != null)
			{
				this.beltScroller.SetSpeedMultiplier(1f);
			}
		}

		// Token: 0x06008AEA RID: 35562 RVA: 0x0038C78C File Offset: 0x0038A98C
		private void HideEquipmentProps()
		{
			this.RestoreEquipmentProps();
			if (this.equipmentPropsToHide == null)
			{
				return;
			}
			int num = 0;
			if (this._activeExercise != null && this._activeExercise.PropBindings != null)
			{
				for (int i = 0; i < this._activeExercise.PropBindings.Count; i++)
				{
					EmotePropBinding emotePropBinding = this._activeExercise.PropBindings[i];
					if (emotePropBinding != null && emotePropBinding.EffectPrefab != null)
					{
						num++;
					}
				}
			}
			int num2 = Mathf.Min(this.equipmentPropsToHide.Length, num);
			for (int j = 0; j < num2; j++)
			{
				GameObject gameObject = this.equipmentPropsToHide[j];
				if (!(gameObject == null) && !(gameObject == base.gameObject) && !base.transform.IsChildOf(gameObject.transform))
				{
					bool flag = false;
					for (int k = 0; k < this._hiddenEquipmentProps.Count; k++)
					{
						if (this._hiddenEquipmentProps[k].GameObject == gameObject)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						this._hiddenEquipmentProps.Add(new GymExerciseStation.EquipmentPropState
						{
							GameObject = gameObject,
							WasActiveSelf = gameObject.activeSelf
						});
						if (gameObject.activeSelf)
						{
							gameObject.SetActive(false);
						}
					}
				}
			}
		}

		// Token: 0x06008AEB RID: 35563 RVA: 0x0038C8E8 File Offset: 0x0038AAE8
		private void RestoreEquipmentProps()
		{
			for (int i = this._hiddenEquipmentProps.Count - 1; i >= 0; i--)
			{
				GymExerciseStation.EquipmentPropState equipmentPropState = this._hiddenEquipmentProps[i];
				if (equipmentPropState.GameObject != null && equipmentPropState.GameObject.activeSelf != equipmentPropState.WasActiveSelf)
				{
					equipmentPropState.GameObject.SetActive(equipmentPropState.WasActiveSelf);
				}
			}
			this._hiddenEquipmentProps.Clear();
		}

		// Token: 0x04008D84 RID: 36228
		private const float PlayerResolveRetrySeconds = 1f;

		// Token: 0x04008D85 RID: 36229
		private const string ChangeExerciseKeyLabel = "F";

		// Token: 0x04008D86 RID: 36230
		private static readonly int EmoteSpeedMultiplierHash = Animator.StringToHash("EmoteSpeedMultiplier");

		// Token: 0x04008D87 RID: 36231
		private static readonly string[] ExerciseIntensityNames = new string[] { "Low", "Normal", "High", "Maximum" };

		// Token: 0x04008D88 RID: 36232
		private static readonly float[] ExerciseIntensityMultipliers = new float[] { 0.8f, 1f, 1.25f, 1.5f };

		// Token: 0x04008D89 RID: 36233
		[Header("Exercises")]
		[SerializeField]
		private List<EmoteDefinitionSO> exercises = new List<EmoteDefinitionSO>();

		// Token: 0x04008D8A RID: 36234
		[SerializeField]
		private Transform characterAnchor;

		// Token: 0x04008D8B RID: 36235
		[Header("Availability")]
		[SerializeField]
		[Min(0.25f)]
		private float detectionRadius = 2f;

		// Token: 0x04008D8C RID: 36236
		[SerializeField]
		private string requiredPoiId = "MallGym";

		// Token: 0x04008D8D RID: 36237
		[Header("Treadmill (Optional)")]
		[SerializeField]
		private GymTreadmillBeltScroller beltScroller;

		// Token: 0x04008D8E RID: 36238
		[Header("Equipment Visuals (Optional)")]
		[Tooltip("Physical weights/handles that disappear while the generated held prop is in use.")]
		[SerializeField]
		private GameObject[] equipmentPropsToHide = Array.Empty<GameObject>();

		// Token: 0x04008D8F RID: 36239
		[Header("Exercise Sweat")]
		[SerializeField]
		private SkinDecal sweatLightDecal;

		// Token: 0x04008D90 RID: 36240
		[SerializeField]
		[Min(0f)]
		private float sweatDelayMinSeconds = 10f;

		// Token: 0x04008D91 RID: 36241
		[SerializeField]
		[Min(0f)]
		private float sweatDelayMaxSeconds = 15f;

		// Token: 0x04008D92 RID: 36242
		[SerializeField]
		[Min(0f)]
		private float sweatFadeInSeconds = 12f;

		// Token: 0x04008D93 RID: 36243
		[SerializeField]
		[Range(0f, 1f)]
		private float sweatPeakOpacity = 1f;

		// Token: 0x04008D94 RID: 36244
		[SerializeField]
		[Min(0f)]
		private float sweatFadeOutSeconds = 6f;

		// Token: 0x04008D95 RID: 36245
		[Header("Input")]
		[SerializeField]
		private GenericInput startInput = new GenericInput("E", string.Empty, string.Empty);

		// Token: 0x04008D96 RID: 36246
		[SerializeField]
		private GenericInput changeExerciseInput = new GenericInput("F", string.Empty, string.Empty);

		// Token: 0x04008D97 RID: 36247
		[SerializeField]
		private GenericInput endInput = new GenericInput("X", string.Empty, string.Empty);

		// Token: 0x04008D98 RID: 36248
		[SerializeField]
		private GenericInput exerciseCameraInput = new GenericInput("K", string.Empty, string.Empty);

		// Token: 0x04008D99 RID: 36249
		[Header("Exercise Intensity")]
		[SerializeField]
		[Range(0f, 3f)]
		private int treadmillDefaultIntensityIndex = 1;

		// Token: 0x04008D9A RID: 36250
		[Header("HUD")]
		[SerializeField]
		private string stationDisplayName = "Gym station";

		// Token: 0x04008D9B RID: 36251
		[SerializeField]
		private string startKeyLabel = "E";

		// Token: 0x04008D9C RID: 36252
		[SerializeField]
		private string changeKeyLabel = "F";

		// Token: 0x04008D9D RID: 36253
		[SerializeField]
		private string endKeyLabel = "X";

		// Token: 0x04008D9E RID: 36254
		[Header("World Prompt")]
		[Tooltip("Vehicle-style world-space prompt shown only while this station wins the shared proximity selection.")]
		[SerializeField]
		private InteractablePromptUI promptUI;

		// Token: 0x04008D9F RID: 36255
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04008DA0 RID: 36256
		private Transform _player;

		// Token: 0x04008DA1 RID: 36257
		private vThirdPersonInput _playerInput;

		// Token: 0x04008DA2 RID: 36258
		private Animator _playerAnimator;

		// Token: 0x04008DA3 RID: 36259
		private GameplayEmoteController _emoteController;

		// Token: 0x04008DA4 RID: 36260
		private PlayerPoiEmoteContext _poiContext;

		// Token: 0x04008DA5 RID: 36261
		private GymExerciseSweatController _sweatController;

		// Token: 0x04008DA6 RID: 36262
		private InteractionConfigSO _runtimeHudConfig;

		// Token: 0x04008DA7 RID: 36263
		private InteractionConfigSO _runtimePromptConfig;

		// Token: 0x04008DA8 RID: 36264
		private readonly InteractionActionBuffer _hudActions = new InteractionActionBuffer();

		// Token: 0x04008DA9 RID: 36265
		private int _cachedHudIntensityIndex = -1;

		// Token: 0x04008DAA RID: 36266
		private string _cachedHudIntensityLabel;

		// Token: 0x04008DAB RID: 36267
		private int _selectedExerciseIndex;

		// Token: 0x04008DAC RID: 36268
		private float _sqrDistance = float.MaxValue;

		// Token: 0x04008DAD RID: 36269
		private float _nextPlayerResolveTime;

		// Token: 0x04008DAE RID: 36270
		private bool _hudPresented;

		// Token: 0x04008DAF RID: 36271
		private bool _promptPresented;

		// Token: 0x04008DB0 RID: 36272
		private bool _exerciseActive;

		// Token: 0x04008DB1 RID: 36273
		private static GymExerciseStation _controllerStation;

		// Token: 0x04008DB2 RID: 36274
		private bool _ambientOccupied;

		// Token: 0x04008DB3 RID: 36275
		private bool _endingExercise;

		// Token: 0x04008DB4 RID: 36276
		private bool _waitForEndRelease;

		// Token: 0x04008DB5 RID: 36277
		private EmoteDefinitionSO _activeExercise;

		// Token: 0x04008DB6 RID: 36278
		private int _treadmillIntensityIndex;

		// Token: 0x04008DB7 RID: 36279
		private readonly List<GymExerciseStation.EquipmentPropState> _hiddenEquipmentProps = new List<GymExerciseStation.EquipmentPropState>();

		// Token: 0x04008DB8 RID: 36280
		private readonly List<GymExerciseStation.ColliderState> _playerColliderStates = new List<GymExerciseStation.ColliderState>();

		// Token: 0x04008DB9 RID: 36281
		private Rigidbody _overriddenPlayerBody;

		// Token: 0x04008DBA RID: 36282
		private bool _savedBodyUseGravity;

		// Token: 0x04008DBB RID: 36283
		private bool _savedBodyIsKinematic;

		// Token: 0x04008DBC RID: 36284
		private bool _savedBodyDetectCollisions;

		// Token: 0x04008DBD RID: 36285
		private bool _playerPhysicsOverridden;

		// Token: 0x04008DBE RID: 36286
		private bool _treadmillRootMotionOverridden;

		// Token: 0x04008DBF RID: 36287
		private bool _savedTreadmillAnimatorApplyRootMotion;

		// Token: 0x04008DC0 RID: 36288
		private bool _savedTreadmillControllerUseRootMotion;

		// Token: 0x04008DC1 RID: 36289
		private bool _hasEntryPose;

		// Token: 0x04008DC2 RID: 36290
		private Vector3 _entryPosition;

		// Token: 0x04008DC3 RID: 36291
		private Quaternion _entryRotation;

		// Token: 0x020011AF RID: 4527
		private struct EquipmentPropState
		{
			// Token: 0x04008DC4 RID: 36292
			public GameObject GameObject;

			// Token: 0x04008DC5 RID: 36293
			public bool WasActiveSelf;
		}

		// Token: 0x020011B0 RID: 4528
		private struct ColliderState
		{
			// Token: 0x04008DC6 RID: 36294
			public Collider Collider;

			// Token: 0x04008DC7 RID: 36295
			public bool WasEnabled;
		}
	}
}
