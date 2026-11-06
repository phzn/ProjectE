using System;
using System.Collections;
using System.Collections.Generic;
using Bozo.AnimeCharacters;
using BoZo.CharacterCreator.MC.StyleSystem;
using Features.Detection;
using Features.Inventory;
using Features.Inventory.Wearable;
using Features.Loading;
using Features.NPC;
using Features.NPC.Audio;
using Features.NPCs;
using Features.WardrobeFeedback;
using Invector.vCamera;
using Invector.vCharacterController;
using MagicaCloth2;
using ProjectExhibitionist;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.Rendering;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.Interactions;
using ProjectExhibitionist.NPCs;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.AzureSky;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.Timeline;

namespace Features.RunModes
{
	// Token: 0x02000CE5 RID: 3301
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(-1050)]
	public sealed class LongWayHomeRestaurantSequence : MonoBehaviour
	{
		// Token: 0x06005FCE RID: 24526 RVA: 0x0025F13C File Offset: 0x0025D33C
		private void UpdateDepartureCameraHint()
		{
			bool canSwitchDepartureCamera = this.CanSwitchDepartureCamera;
			bool canSkip = this.CanSkip;
			if (!canSwitchDepartureCamera && !canSkip)
			{
				this.ReleaseDepartureCameraHint();
				return;
			}
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (this._departureCameraHud == instance && this._departureCameraHintConfig != null)
			{
				if (this._hintShowsCamera != canSwitchDepartureCamera || this._hintShowsSkip != canSkip)
				{
					this.UpdateCinematicHintActions(canSwitchDepartureCamera, canSkip);
					instance.ShowSupplemental(this, this._departureCameraHintConfig);
				}
				return;
			}
			this.ReleaseDepartureCameraHint();
			if (instance == null)
			{
				return;
			}
			this._departureCameraHud = instance;
			this._departureCameraHintConfig = ScriptableObject.CreateInstance<InteractionConfigSO>();
			this._departureCameraHintConfig.name = "LongWayHomeRestaurantCameraHints_Runtime";
			this._departureCameraHintConfig.hideFlags = HideFlags.HideAndDontSave;
			this._departureCameraHintConfig.objectName = "Camera";
			this.UpdateCinematicHintActions(canSwitchDepartureCamera, canSkip);
			this._departureCameraHudCanvas = instance.GetComponent<Canvas>();
			this._departureCameraHudCanvasCreated = this._departureCameraHudCanvas == null;
			if (this._departureCameraHudCanvasCreated)
			{
				this._departureCameraHudCanvas = instance.gameObject.AddComponent<Canvas>();
				this._departureCameraHudCanvas.hideFlags = HideFlags.DontSave;
			}
			this._departureCameraHudCanvasEnabled = this._departureCameraHudCanvas.enabled;
			this._departureCameraHudOverrideSorting = this._departureCameraHudCanvas.overrideSorting;
			this._departureCameraHudSortingOrder = this._departureCameraHudCanvas.sortingOrder;
			instance.ShowSupplemental(this, this._departureCameraHintConfig);
			this._departureCameraHudCanvas.enabled = true;
			this._departureCameraHudCanvas.overrideSorting = true;
			this._departureCameraHudCanvas.sortingOrder = 30;
			GameplayHudVisibility.RegisterCanvas(this._departureCameraHudCanvas);
		}

		// Token: 0x06005FCF RID: 24527 RVA: 0x0025F2BC File Offset: 0x0025D4BC
		private void UpdateCinematicHintActions(bool cameraHint, bool skipHint)
		{
			this._hintShowsCamera = cameraHint;
			this._hintShowsSkip = skipHint;
			this._departureCameraHintConfig.usableActions.Clear();
			if (skipHint)
			{
				LongWayHomeCinematicSkip.AddAction(this._departureCameraHintConfig);
			}
			if (cameraHint)
			{
				this._departureCameraHintConfig.usableActions.Add(new UsableAction
				{
					keyDisplayLabel = "F",
					actionName = "Switch camera"
				});
			}
		}

		// Token: 0x06005FD0 RID: 24528 RVA: 0x0025F324 File Offset: 0x0025D524
		private void ReleaseDepartureCameraHint()
		{
			if (this._departureCameraHud != null)
			{
				this._departureCameraHud.HideSupplemental(this);
			}
			this._departureCameraHud = null;
			if (this._departureCameraHudCanvas != null)
			{
				if (this._departureCameraHudCanvasCreated)
				{
					this._departureCameraHudCanvas.enabled = false;
					LongWayHomeRestaurantSequence.DestroyDepartureCameraHintObject(this._departureCameraHudCanvas);
				}
				else
				{
					this._departureCameraHudCanvas.overrideSorting = this._departureCameraHudOverrideSorting;
					this._departureCameraHudCanvas.sortingOrder = this._departureCameraHudSortingOrder;
					this._departureCameraHudCanvas.enabled = this._departureCameraHudCanvasEnabled;
				}
			}
			this._departureCameraHudCanvas = null;
			this._departureCameraHudCanvasCreated = false;
			if (this._departureCameraHintConfig != null)
			{
				LongWayHomeRestaurantSequence.DestroyDepartureCameraHintObject(this._departureCameraHintConfig);
			}
			this._departureCameraHintConfig = null;
		}

		// Token: 0x06005FD1 RID: 24529 RVA: 0x00027EE2 File Offset: 0x000260E2
		private static void DestroyDepartureCameraHintObject(Object target)
		{
			if (Application.isPlaying)
			{
				Object.Destroy(target);
				return;
			}
			Object.DestroyImmediate(target);
		}

		// Token: 0x17000F82 RID: 3970
		// (get) Token: 0x06005FD2 RID: 24530 RVA: 0x0025F3E4 File Offset: 0x0025D5E4
		private bool CanSwitchDepartureCamera
		{
			get
			{
				return this._departureCameraSwitchUnlocked && this.IsReady && this.departureCamera != null && this.departureFaceCamera != null && (this.Phase == LongWayHomeRestaurantSequence.StoryPhase.DrunkPause || this.Phase == LongWayHomeRestaurantSequence.StoryPhase.Walking || this.Phase == LongWayHomeRestaurantSequence.StoryPhase.AtExit);
			}
		}

		// Token: 0x06005FD3 RID: 24531 RVA: 0x0025F43C File Offset: 0x0025D63C
		private void InitializeDepartureCameraSwitch()
		{
			this.ReleaseDepartureCameraSwitch();
			if (this._player == null || this.departureFaceCamera == null || this.departureFaceCameraRig == null)
			{
				return;
			}
			this._departureCameraRigPosition = this.departureFaceCameraRig.localPosition;
			this._departureCameraRigRotation = this.departureFaceCameraRig.localRotation;
			this._departureCameraRigCaptured = true;
			this._departureFaceHead = ((this._animator != null && this._animator.isHuman) ? this._animator.GetBoneTransform(HumanBodyBones.Head) : null);
			Transform transform = ((this.departureFaceCameraReference != null) ? this.departureFaceCameraReference : ((this.playerMark != null) ? this.playerMark : this._player));
			Quaternion rotation = transform.rotation;
			Vector3 vector = transform.position;
			if (this.departureFaceCameraReference == null)
			{
				vector += rotation * new Vector3(0f, 1.61f, 0f);
			}
			Quaternion quaternion = Quaternion.Inverse(rotation);
			this._departureCameraPositionOffset = quaternion * (this.departureFaceCameraRig.position - vector);
			this._departureCameraRotationOffset = quaternion * this.departureFaceCameraRig.rotation;
			this.UpdateDepartureCameraPose();
		}

		// Token: 0x06005FD4 RID: 24532 RVA: 0x0025F583 File Offset: 0x0025D783
		private void UnlockDepartureCameraSwitch()
		{
			if (this.Phase != LongWayHomeRestaurantSequence.StoryPhase.Walking || !this._departureCameraRigCaptured)
			{
				return;
			}
			this._departureCameraSwitchUnlocked = true;
			this.UpdateDepartureCameraHint();
		}

		// Token: 0x06005FD5 RID: 24533 RVA: 0x0025F5A4 File Offset: 0x0025D7A4
		private void UpdateDepartureCameraSwitchInput()
		{
			this.UpdateDepartureCameraHint();
			if (!this.CanSwitchDepartureCamera)
			{
				return;
			}
			bool flag = ControllerInput.IsActive && !ControllerInput.UiInputConsumedThisFrame && !TextInputFocus.HasFocusedField && !GraphicsSettingsRuntimeUI.AnyOpen && ControllerInput.GetKeyDown(KeyCode.F);
			if (Time.timeScale > 0f && (Input.GetKeyDown(KeyCode.F) || flag))
			{
				if (flag)
				{
					ControllerInput.ConsumeUIInput();
				}
				this.TryToggleDepartureCamera();
			}
		}

		// Token: 0x06005FD6 RID: 24534 RVA: 0x0025F610 File Offset: 0x0025D810
		private bool TryToggleDepartureCamera()
		{
			if (!this.CanSwitchDepartureCamera)
			{
				return false;
			}
			this.UpdateDepartureCameraPose();
			this._usingDepartureFaceCamera = !this._usingDepartureFaceCamera;
			this.SetDepartureCamera(!this._usingDepartureFaceCamera);
			this.SetFaceCameraEnabled(this._usingDepartureFaceCamera);
			this.UpdateDepartureCameraHint();
			return true;
		}

		// Token: 0x06005FD7 RID: 24535 RVA: 0x0025F660 File Offset: 0x0025D860
		private void UpdateDepartureCameraPose()
		{
			if (!this._departureCameraRigCaptured || this.departureFaceCameraRig == null || this._player == null)
			{
				return;
			}
			if (this._departureCameraHasAppliedPose)
			{
				Quaternion quaternion = Quaternion.Inverse(this._departureCameraLastFollowRotation);
				Vector3 vector = this.departureFaceCameraRig.position - this._departureCameraLastPosition;
				if (vector.sqrMagnitude > 1E-08f)
				{
					this._departureCameraPositionOffset += quaternion * vector;
				}
				if (!this.departureFaceCameraRig.rotation.Equals(this._departureCameraLastRotation))
				{
					this._departureCameraRotationOffset = quaternion * this.departureFaceCameraRig.rotation;
				}
			}
			Vector3 vector2 = ((this._departureFaceHead != null) ? this._departureFaceHead.position : this._player.TransformPoint(new Vector3(0f, 1.51f, 0f)));
			this.departureFaceCameraRig.SetPositionAndRotation(vector2 + this._player.TransformDirection(this.departureFaceTargetOffset) + this._player.rotation * this._departureCameraPositionOffset, this._player.rotation * this._departureCameraRotationOffset);
			this._departureCameraLastPosition = this.departureFaceCameraRig.position;
			this._departureCameraLastRotation = this.departureFaceCameraRig.rotation;
			this._departureCameraLastFollowRotation = this._player.rotation;
			this._departureCameraHasAppliedPose = true;
		}

		// Token: 0x06005FD8 RID: 24536 RVA: 0x0025F7DD File Offset: 0x0025D9DD
		private void LockDepartureCameraSwitch()
		{
			this._departureCameraSwitchUnlocked = false;
			this.UpdateDepartureCameraHint();
		}

		// Token: 0x06005FD9 RID: 24537 RVA: 0x0025F7EC File Offset: 0x0025D9EC
		private void SetFaceCameraEnabled(bool active)
		{
			if (this.departureFaceCamera == null)
			{
				return;
			}
			this.departureFaceCamera.enabled = active;
			AudioListener component = this.departureFaceCamera.GetComponent<AudioListener>();
			if (component != null)
			{
				component.enabled = active;
			}
		}

		// Token: 0x06005FDA RID: 24538 RVA: 0x0025F830 File Offset: 0x0025DA30
		private void ReleaseDepartureCameraSwitch()
		{
			this.LockDepartureCameraSwitch();
			this.SetFaceCameraEnabled(false);
			this._usingDepartureFaceCamera = false;
			this._departureFaceHead = null;
			this._departureCameraHasAppliedPose = false;
			if (this._departureCameraRigCaptured && this.departureFaceCameraRig != null)
			{
				this.departureFaceCameraRig.localPosition = this._departureCameraRigPosition;
				this.departureFaceCameraRig.localRotation = this._departureCameraRigRotation;
			}
			this._departureCameraRigCaptured = false;
		}

		// Token: 0x17000F83 RID: 3971
		// (get) Token: 0x06005FDB RID: 24539 RVA: 0x0025F89E File Offset: 0x0025DA9E
		public Transform RuntimePlayer
		{
			get
			{
				return this._player;
			}
		}

		// Token: 0x17000F84 RID: 3972
		// (get) Token: 0x06005FDC RID: 24540 RVA: 0x0025F8A6 File Offset: 0x0025DAA6
		// (set) Token: 0x06005FDD RID: 24541 RVA: 0x0025F8AE File Offset: 0x0025DAAE
		public bool IsReady { get; private set; }

		// Token: 0x17000F85 RID: 3973
		// (get) Token: 0x06005FDE RID: 24542 RVA: 0x0025F8B7 File Offset: 0x0025DAB7
		// (set) Token: 0x06005FDF RID: 24543 RVA: 0x0025F8BF File Offset: 0x0025DABF
		public bool IsPreparedForReveal { get; private set; }

		// Token: 0x17000F86 RID: 3974
		// (get) Token: 0x06005FE0 RID: 24544 RVA: 0x0025F8C8 File Offset: 0x0025DAC8
		public bool OwnsDestinationReveal
		{
			get
			{
				return base.isActiveAndEnabled && this.Phase != LongWayHomeRestaurantSequence.StoryPhase.Failed;
			}
		}

		// Token: 0x17000F87 RID: 3975
		// (get) Token: 0x06005FE1 RID: 24545 RVA: 0x0025F8E0 File Offset: 0x0025DAE0
		public PlayableDirector Director
		{
			get
			{
				return this.director;
			}
		}

		// Token: 0x06005FE2 RID: 24546 RVA: 0x0025F8E8 File Offset: 0x0025DAE8
		public static LongWayHomeRestaurantSequence GetPresenterForScene(Scene scene)
		{
			if (!(LongWayHomeRestaurantSequence._activePresenter != null) || !LongWayHomeRestaurantSequence._activePresenter.isActiveAndEnabled || !(LongWayHomeRestaurantSequence._activePresenter.gameObject.scene == scene))
			{
				return null;
			}
			return LongWayHomeRestaurantSequence._activePresenter;
		}

		// Token: 0x06005FE3 RID: 24547 RVA: 0x0025F921 File Offset: 0x0025DB21
		public void FailPreparation(string reason)
		{
			if (this.IsPreparedForReveal || !this.OwnsDestinationReveal)
			{
				return;
			}
			base.StopAllCoroutines();
			this.Fail(reason);
		}

		// Token: 0x06005FE4 RID: 24548 RVA: 0x0025F944 File Offset: 0x0025DB44
		private void Awake()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			LongWayHomeRestaurantSequence._activePresenter = this;
			this.CaptureCinematicPhotoReactions();
			ScreenFadeController instance = ScreenFadeController.Instance;
			if (instance != null)
			{
				instance.SetBlack();
			}
			if (this.cinematicCamera != null)
			{
				this.cinematicCamera.enabled = false;
				this._cinematicListener = this.cinematicCamera.GetComponent<AudioListener>();
				if (this._cinematicListener != null)
				{
					this._cinematicListener.enabled = false;
				}
			}
			if (this.director != null)
			{
				this.director.playOnAwake = false;
				this.director.Stop();
			}
			if (this.editorPreview != null)
			{
				this.editorPreview.SetActive(false);
			}
			this.SetDepartureCamera(false);
			this.ReleaseDepartureCameraSwitch();
		}

		// Token: 0x06005FE5 RID: 24549 RVA: 0x0025FA06 File Offset: 0x0025DC06
		private IEnumerator Start()
		{
			if (this.director == null || this.cielTrack == null || this.playerMark == null || this.cinematicCamera == null || this.drinkTemplate == null || this.tableOutfit == null || !this.HasStoryBindings())
			{
				this.Fail("Missing authored scene bindings.");
				yield break;
			}
			string text;
			if (!this.PrepareRestaurantPopulation(out text))
			{
				this.Fail(text);
				yield break;
			}
			LongWayHomeOpeningReadinessWait playerReadiness = new LongWayHomeOpeningReadinessWait(this.readinessTimeout);
			while (!this.PlayerReady())
			{
				LongWayHomeOpeningReadinessWait.Result result = playerReadiness.ObservePlayer(base.gameObject.scene);
				if (result == LongWayHomeOpeningReadinessWait.Result.UpstreamFailed)
				{
					yield break;
				}
				if (result == LongWayHomeOpeningReadinessWait.Result.TimedOut)
				{
					this.Fail("The persistent player did not finish initialization.");
					yield break;
				}
				yield return null;
			}
			yield return null;
			if (LongWayHomeOpeningReadinessWait.HasUpstreamFailure)
			{
				yield break;
			}
			LoadingReadinessWatchdog activityReadiness = new LoadingReadinessWatchdog(this.readinessTimeout);
			while (LongWayHomeOpeningReadinessWait.HasUpstreamFailure || !PlayerActivityGate.TryAcquire("LongWayHomeRestaurant#" + base.GetInstanceID().ToString(), this, PlayerActivity.None, PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation | PlayerContextResource.HeldItem, PlayerActivity.AllPlayerActions, out this._lease, false, null))
			{
				if (LongWayHomeOpeningReadinessWait.HasUpstreamFailure)
				{
					yield break;
				}
				if (activityReadiness.Tick(Time.realtimeSinceStartupAsDouble, 0f, 0, 0))
				{
					this.Fail("Another player activity still owns cinematic control.");
					yield break;
				}
				yield return null;
			}
			this._player = PlayerRegistry.Player;
			this._animator = this._player.GetComponent<Animator>();
			this._input = this._player.GetComponent<vThirdPersonInput>();
			this._body = this._player.GetComponent<Rigidbody>();
			MCStyleCustomizationService style = this._player.GetComponentInChildren<MCStyleCustomizationService>(true);
			if (this._animator == null || !this._animator.isHuman || this._input == null || this._body == null || style == null)
			{
				this.Fail("The registered player is missing its production rig components.");
				yield break;
			}
			this.CapturePlayerControl();
			this.CaptureBaseFace();
			this.SuppressCorePresentation();
			WearAdmireAnimation.SetPresentationSuppressed(this, true);
			if (!this.CaptureCinematicWardrobe())
			{
				this.Fail("The player's wardrobe could not be isolated for the cinematic.");
				yield break;
			}
			foreach (MCStylePartDefinitionSO mcstylePartDefinitionSO in style.PartDefinitions)
			{
				if (mcstylePartDefinitionSO != null && !mcstylePartDefinitionSO.isBodyAppearance && mcstylePartDefinitionSO.partId.StartsWith("Outfit.", StringComparison.Ordinal) && (style.IsSlotEquipped(mcstylePartDefinitionSO.partId) || style.GetSpawnedRoot(mcstylePartDefinitionSO.partId) != null))
				{
					style.SetSlotEquipped(mcstylePartDefinitionSO.partId, false);
				}
			}
			style.SetNaked(false);
			yield return null;
			if (!style.EquipRuntimePreset(this.tableOutfit.mcStylePartId, this.tableOutfit.mcStylePreset) || style.GetSpawnedRoot(this.tableOutfit.mcStylePartId) == null)
			{
				this.Fail("The complete university outfit could not be attached to the player.");
				yield break;
			}
			style.SetSlotPhysicallyHidden(this.tableOutfit.mcStylePartId, false);
			ClothingToggleController componentInChildren = this._player.GetComponentInChildren<ClothingToggleController>(true);
			if (componentInChildren != null)
			{
				componentInChildren.SetSlotVisible(this.tableOutfit.mcStylePartId, true);
			}
			yield return null;
			this._player.SetPositionAndRotation(this.playerMark.position, this.playerMark.rotation);
			this._body.position = this.playerMark.position;
			this._body.rotation = this.playerMark.rotation;
			this._body.velocity = Vector3.zero;
			this._body.angularVelocity = Vector3.zero;
			Physics.SyncTransforms();
			Transform boneTransform = this._animator.GetBoneTransform(HumanBodyBones.LeftHand);
			this._drink = Object.Instantiate<GameObject>(this.drinkTemplate, boneTransform, false);
			this._drink.name = "Drink - Ciel (authored grip)";
			this._drink.SetActive(true);
			this._previewBinding = this.director.GetGenericBinding(this.cielTrack);
			this.director.SetGenericBinding(this.cielTrack, this._animator);
			this.director.RebuildGraph();
			this.director.time = 0.0;
			this.director.Evaluate();
			foreach (MagicaCloth magicaCloth in this._player.GetComponentsInChildren<MagicaCloth>(true))
			{
				if (magicaCloth != null && magicaCloth.isActiveAndEnabled)
				{
					magicaCloth.ResetCloth(false);
				}
			}
			this.cinematicCamera.enabled = true;
			if (this._cinematicListener != null)
			{
				this._cinematicListener.enabled = true;
			}
			this.director.Play();
			yield return this.WarmPresentation();
			if (this.Phase == LongWayHomeRestaurantSequence.StoryPhase.Failed)
			{
				yield break;
			}
			this.IsPreparedForReveal = true;
			while ((LoadingScreenController.ExistingInstance != null && LoadingScreenController.ExistingInstance.IsLoadingActive) || (SceneTransitionManager.Instance != null && SceneTransitionManager.Instance.IsTransitioning))
			{
				yield return null;
			}
			this._input.SetLockAllInput(true);
			this._input.SetLockCameraInput(true);
			this.IsReady = true;
			ScreenFadeController instance = ScreenFadeController.Instance;
			if (instance != null)
			{
				instance.FadeIn(0.35f, null);
			}
			this.ambience.Begin(0.6f);
			yield return this.PlayRestaurantStory();
			yield break;
		}

		// Token: 0x06005FE6 RID: 24550 RVA: 0x0025FA18 File Offset: 0x0025DC18
		private bool PlayerReady()
		{
			if (SceneManager.GetActiveScene() != base.gameObject.scene || !SceneBootLoader.CoreReady || !GameSystemsBootstrapper.GameplayReady || GameSystemsBootstrapper.Instance == null || GameSystemsBootstrapper.Instance.InitializedSceneName != base.gameObject.scene.name || PlayerRegistry.Player == null)
			{
				return false;
			}
			ClothInventorySyncer componentInChildren = PlayerRegistry.Player.GetComponentInChildren<ClothInventorySyncer>(true);
			return componentInChildren == null || componentInChildren.SyncCompleted;
		}

		// Token: 0x06005FE7 RID: 24551 RVA: 0x0025FAA7 File Offset: 0x0025DCA7
		private IEnumerator WarmPresentation()
		{
			MagicaCloth[] cloths = this._player.GetComponentsInChildren<MagicaCloth>(true);
			float deadline = Time.realtimeSinceStartup + 12f;
			int settledFrames = 0;
			for (;;)
			{
				yield return null;
				bool flag = true;
				foreach (MagicaCloth magicaCloth in cloths)
				{
					if (magicaCloth != null && magicaCloth.isActiveAndEnabled && !magicaCloth.IsValid())
					{
						flag = false;
						break;
					}
				}
				settledFrames = (flag ? (settledFrames + 1) : 0);
				if (Time.realtimeSinceStartup >= deadline)
				{
					break;
				}
				if (settledFrames >= 6)
				{
					goto Block_6;
				}
			}
			this.Fail("The cinematic character physics did not finish preparing.");
			yield break;
			Block_6:
			float renderDeadline = Time.realtimeSinceStartup + 4f;
			int smoothFrames = 0;
			do
			{
				yield return null;
				smoothFrames = ((Time.unscaledDeltaTime <= 0.05f) ? (smoothFrames + 1) : 0);
			}
			while (smoothFrames < 6 && Time.realtimeSinceStartup < renderDeadline);
			yield break;
		}

		// Token: 0x06005FE8 RID: 24552 RVA: 0x0025FAB8 File Offset: 0x0025DCB8
		private void CapturePlayerControl()
		{
			this._captured = true;
			this._inputEnabled = this._input.enabled;
			this._inputLock = this._input.lockInput;
			this._cameraLock = this._input.lockCameraInput;
			LoadingScreenController existingInstance = LoadingScreenController.ExistingInstance;
			bool flag;
			bool flag2;
			if (existingInstance != null && existingInstance.TryGetPreLoadingInputLocks(this._input, out flag, out flag2))
			{
				this._inputLock = flag;
				this._cameraLock = flag2;
			}
			this._gameplayCamera = this._input.tpCamera;
			if (this._gameplayCamera != null)
			{
				this._cameraFrozen = this._gameplayCamera.isFreezed;
			}
			this._input.SetLockAllInput(true);
			this._input.SetLockCameraInput(true);
			if (this._input.cc != null)
			{
				this._input.cc.input = Vector2.zero;
				this._input.cc.isSprinting = false;
			}
			this._input.enabled = false;
			this._coverPose = this._player.GetComponentInChildren<CoverPoseController>(true);
			if (this._coverPose != null)
			{
				this._coverEnabled = this._coverPose.enabled;
				this._coverPose.enabled = false;
			}
			this._rootMotion = this._animator.applyRootMotion;
			this._culling = this._animator.cullingMode;
			this._animator.applyRootMotion = false;
			this._animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
			this._layerWeights = new float[this._animator.layerCount];
			for (int i = 0; i < this._layerWeights.Length; i++)
			{
				this._layerWeights[i] = this._animator.GetLayerWeight(i);
			}
			this._kinematic = this._body.isKinematic;
			this._gravity = this._body.useGravity;
			this._constraints = this._body.constraints;
			this._body.isKinematic = false;
			this._body.useGravity = true;
			this._body.constraints = RigidbodyConstraints.FreezeAll;
		}

		// Token: 0x06005FE9 RID: 24553 RVA: 0x0025FCCC File Offset: 0x0025DECC
		private void SuppressCorePresentation()
		{
			Scene sceneByName = SceneManager.GetSceneByName("_Core");
			if (!sceneByName.IsValid() || !sceneByName.isLoaded)
			{
				return;
			}
			foreach (GameObject gameObject in sceneByName.GetRootGameObjects())
			{
				foreach (Camera camera in gameObject.GetComponentsInChildren<Camera>(true))
				{
					this.Suppress(camera);
				}
				foreach (AudioListener audioListener in gameObject.GetComponentsInChildren<AudioListener>(true))
				{
					this.Suppress(audioListener);
				}
				foreach (AzureCoreSystem azureCoreSystem in gameObject.GetComponentsInChildren<AzureCoreSystem>(true))
				{
					this.Suppress(azureCoreSystem);
				}
				foreach (Light light in gameObject.GetComponentsInChildren<Light>(true))
				{
					this.Suppress(light);
				}
				foreach (Volume volume in gameObject.GetComponentsInChildren<Volume>(true))
				{
					this.Suppress(volume);
				}
				foreach (Canvas canvas in gameObject.GetComponentsInChildren<Canvas>(true))
				{
					if (!GameplayHudVisibility.IsProtected(canvas.transform))
					{
						this.Suppress(canvas);
					}
				}
				foreach (AudioSource audioSource in gameObject.GetComponentsInChildren<AudioSource>(true))
				{
					if (!audioSource.mute)
					{
						this._mutedAudio.Add(audioSource);
						audioSource.mute = true;
					}
				}
			}
			this._previousSky = RenderSettings.skybox;
			this._previousAmbientMode = RenderSettings.ambientMode;
			this._previousAmbient = RenderSettings.ambientLight;
			this._previousFog = RenderSettings.fog;
			this._capturedRenderSettings = true;
			RenderSettings.skybox = this.authoredSky;
			RenderSettings.ambientMode = this.authoredAmbientMode;
			RenderSettings.ambientLight = this.authoredAmbient;
			RenderSettings.fog = this.authoredFog;
		}

		// Token: 0x06005FEA RID: 24554 RVA: 0x0025FEC8 File Offset: 0x0025E0C8
		private void Suppress(Behaviour component)
		{
			this._presentation.Add(new LongWayHomeRestaurantSequence.BehaviourState
			{
				Component = component,
				Enabled = component.enabled
			});
			component.enabled = false;
		}

		// Token: 0x06005FEB RID: 24555 RVA: 0x0025FF08 File Offset: 0x0025E108
		private void LateUpdate()
		{
			this.UpdateDepartureLookAt();
			this.UpdateDepartureCameraPose();
			if (!this._captured || this._animator == null)
			{
				return;
			}
			for (int i = 1; i < this._animator.layerCount; i++)
			{
				this._animator.SetLayerWeight(i, (float)((i == this._faceLayer) ? 1 : 0));
			}
			if (this._faceAnimator != null && this._faceAnimator.GetInteger(this._faceParameter) != this._neutralFaceIndex)
			{
				this._faceAnimator.SetInteger(this._faceParameter, this._neutralFaceIndex);
			}
		}

		// Token: 0x06005FEC RID: 24556 RVA: 0x0025FFA8 File Offset: 0x0025E1A8
		private void BeginDrunkFace()
		{
			if (!this._captured || this._player == null)
			{
				return;
			}
			if (this._drunkFaceOverride == null)
			{
				this._drunkFaceOverride = base.gameObject.AddComponent<LongWayHomeRestaurantFaceOverride>();
			}
			this._drunkFaceOverride.Begin(this._player, this.drunkFaceBlendSeconds);
		}

		// Token: 0x06005FED RID: 24557 RVA: 0x00260004 File Offset: 0x0025E204
		private void CaptureBaseFace()
		{
			this._emotion = this._player.GetComponentInChildren<PlayerEmotionController>(true);
			this._faceAnimator = ((this._emotion != null && this._emotion.PlayerAnimator != null) ? this._emotion.PlayerAnimator : this._animator);
			this._faceParameter = ((this._emotion != null && !string.IsNullOrEmpty(this._emotion.ExpressionParamName)) ? this._emotion.ExpressionParamName : "FaceExpressionIndex");
			this._neutralFaceIndex = ((this._emotion != null) ? this._emotion.NeutralExpressionIndex : 0);
			this._savedFaceIndex = this._faceAnimator.GetInteger(this._faceParameter);
			this._faceLayer = this._animator.GetLayerIndex("Face");
			if (this._emotion != null)
			{
				this._emotion.DisableEmotionLayersForAction();
				this._ownsEmotionOverride = true;
				this._ownsFaceOverride = !this._emotion.IsFacialExpressionOverriddenForAction;
				if (this._ownsFaceOverride)
				{
					this._emotion.DisableFacialExpressionForAction();
				}
				this._emotion.SetExpression(this._neutralFaceIndex);
			}
			else
			{
				this._faceAnimator.SetInteger(this._faceParameter, this._neutralFaceIndex);
			}
			if (this._faceLayer >= 0)
			{
				this._animator.SetLayerWeight(this._faceLayer, 1f);
				this._animator.Play("Face.Base Face", this._faceLayer, 0f);
			}
		}

		// Token: 0x06005FEE RID: 24558 RVA: 0x00260190 File Offset: 0x0025E390
		private void Update()
		{
			if (!this.IsPreparedForReveal && this.OwnsDestinationReveal && SceneManager.GetActiveScene() == base.gameObject.scene)
			{
				ScreenFadeController instance = ScreenFadeController.Instance;
				if (instance != null)
				{
					instance.SetBlack();
				}
			}
			if (this.CanSkip && LongWayHomeCinematicSkip.TryConsume(this))
			{
				this.TrySkip();
			}
			else if (this.IsReady)
			{
				EscapeInputCoordinator.TryConsume(this);
			}
			this.UpdateDepartureCameraSwitchInput();
		}

		// Token: 0x06005FEF RID: 24559 RVA: 0x00260202 File Offset: 0x0025E402
		private void Fail(string reason)
		{
			this.Phase = LongWayHomeRestaurantSequence.StoryPhase.Failed;
			Debug.LogError("[LongWayHomeRestaurant] " + reason, this);
			this.ReleasePlayer();
			ScreenFadeController instance = ScreenFadeController.Instance;
			if (instance == null)
			{
				return;
			}
			instance.SetClear();
		}

		// Token: 0x06005FF0 RID: 24560 RVA: 0x00260234 File Offset: 0x0025E434
		private void OnDisable()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			base.StopAllCoroutines();
			this.ReleasePlayer();
			if (this.Phase != LongWayHomeRestaurantSequence.StoryPhase.Handoff && SceneManager.GetActiveScene() == base.gameObject.scene)
			{
				ScreenFadeController instance = ScreenFadeController.Instance;
				if (instance != null)
				{
					instance.SetClear();
				}
			}
			if (LongWayHomeRestaurantSequence._activePresenter == this)
			{
				LongWayHomeRestaurantSequence._activePresenter = null;
			}
		}

		// Token: 0x06005FF1 RID: 24561 RVA: 0x00260298 File Offset: 0x0025E498
		public void PrepareForSceneExit()
		{
			this.Phase = LongWayHomeRestaurantSequence.StoryPhase.Handoff;
			base.StopAllCoroutines();
			this.ReleasePlayer();
			base.enabled = false;
		}

		// Token: 0x06005FF2 RID: 24562 RVA: 0x002602B4 File Offset: 0x0025E4B4
		private void ReleasePlayer()
		{
			this.StopCinematicPhotoReactions();
			this.IsReady = false;
			this.IsPreparedForReveal = false;
			this.ReleaseStory();
			if (this.cinematicCamera != null)
			{
				this.cinematicCamera.enabled = false;
			}
			if (this._cinematicListener != null)
			{
				this._cinematicListener.enabled = false;
			}
			if (this.director != null)
			{
				this.director.Stop();
				if (this.cielTrack != null && this._previewBinding != null)
				{
					this.director.SetGenericBinding(this.cielTrack, this._previewBinding);
				}
			}
			this.ReleaseRestaurantPopulation();
			if (this._drink != null)
			{
				Object.Destroy(this._drink);
				this._drink = null;
			}
			LongWayHomeRestaurantFaceOverride drunkFaceOverride = this._drunkFaceOverride;
			if (drunkFaceOverride != null)
			{
				drunkFaceOverride.Release();
			}
			this.RestoreCinematicWardrobe();
			WearAdmireAnimation.SetPresentationSuppressed(this, false);
			if (this._faceAnimator != null)
			{
				if (this._ownsFaceOverride && this._emotion != null)
				{
					this._emotion.RestoreFacialExpression();
				}
				else
				{
					this._faceAnimator.SetInteger(this._faceParameter, this._savedFaceIndex);
				}
			}
			this._ownsFaceOverride = false;
			this._faceAnimator = null;
			if (this._captured)
			{
				if (this._animator != null)
				{
					this._animator.applyRootMotion = this._rootMotion;
					this._animator.cullingMode = this._culling;
					int num = 1;
					while (num < this._animator.layerCount && num < this._layerWeights.Length)
					{
						this._animator.SetLayerWeight(num, this._layerWeights[num]);
						num++;
					}
					if (this._animator.isActiveAndEnabled && this._animator.isInitialized)
					{
						this._animator.Update(0f);
					}
				}
				if (this._body != null)
				{
					this._body.constraints = this._constraints;
					this._body.useGravity = this._gravity;
					this._body.isKinematic = this._kinematic;
					if (!this._body.isKinematic)
					{
						this._body.velocity = Vector3.zero;
						this._body.angularVelocity = Vector3.zero;
					}
				}
				if (this._coverPose != null)
				{
					this._coverPose.enabled = this._coverEnabled;
				}
				if (this._input != null)
				{
					LoadingScreenController existingInstance = LoadingScreenController.ExistingInstance;
					bool flag2;
					bool flag3;
					bool flag = existingInstance != null && existingInstance.TryGetPreLoadingInputLocks(this._input, out flag2, out flag3);
					this._input.SetLockAllInput(flag || this._inputLock);
					this._input.SetLockCameraInput(flag || this._cameraLock);
					this._input.enabled = this._inputEnabled;
				}
				if (this._gameplayCamera != null)
				{
					this._gameplayCamera.isFreezed = this._cameraFrozen;
				}
				this._captured = false;
			}
			if (this._ownsEmotionOverride && this._emotion != null)
			{
				this._emotion.RestoreEmotionLayers();
			}
			this._ownsEmotionOverride = false;
			foreach (LongWayHomeRestaurantSequence.BehaviourState behaviourState in this._presentation)
			{
				if (behaviourState.Component != null)
				{
					behaviourState.Component.enabled = behaviourState.Enabled;
				}
			}
			this._presentation.Clear();
			foreach (AudioSource audioSource in this._mutedAudio)
			{
				if (audioSource != null)
				{
					audioSource.mute = false;
				}
			}
			this._mutedAudio.Clear();
			if (this._capturedRenderSettings)
			{
				if (SceneManager.GetActiveScene() == base.gameObject.scene)
				{
					RenderSettings.skybox = this._previousSky;
					RenderSettings.ambientMode = this._previousAmbientMode;
					RenderSettings.ambientLight = this._previousAmbient;
					RenderSettings.fog = this._previousFog;
				}
				this._capturedRenderSettings = false;
				this._previousSky = null;
			}
			PlayerActivityLease lease = this._lease;
			if (lease != null)
			{
				lease.Dispose();
			}
			this._lease = null;
		}

		// Token: 0x17000F88 RID: 3976
		// (get) Token: 0x06005FF3 RID: 24563 RVA: 0x00260718 File Offset: 0x0025E918
		// (set) Token: 0x06005FF4 RID: 24564 RVA: 0x00260720 File Offset: 0x0025E920
		public LongWayHomeRestaurantSequence.StoryPhase Phase { get; private set; }

		// Token: 0x17000F89 RID: 3977
		// (get) Token: 0x06005FF5 RID: 24565 RVA: 0x00260729 File Offset: 0x0025E929
		public bool CanSkip
		{
			get
			{
				return this.IsReady && this._captured && !this._skipRequested && this.Phase != LongWayHomeRestaurantSequence.StoryPhase.Loading && this.Phase != LongWayHomeRestaurantSequence.StoryPhase.Handoff && this.Phase != LongWayHomeRestaurantSequence.StoryPhase.Failed;
			}
		}

		// Token: 0x06005FF6 RID: 24566 RVA: 0x00260764 File Offset: 0x0025E964
		public bool TrySkip()
		{
			if (!this.CanSkip)
			{
				return false;
			}
			this._skipRequested = true;
			base.StopAllCoroutines();
			LongWayHomeRestaurantDialogue longWayHomeRestaurantDialogue = this.dialogue;
			if (longWayHomeRestaurantDialogue != null)
			{
				longWayHomeRestaurantDialogue.Abort();
			}
			this.StopCinematicPhotoReactions();
			this.StopDepartureFootsteps();
			this.LockDepartureCameraSwitch();
			base.StartCoroutine(this.HandoffToCity(true));
			return true;
		}

		// Token: 0x06005FF7 RID: 24567 RVA: 0x002607BC File Offset: 0x0025E9BC
		private bool HasStoryBindings()
		{
			return this.dialogue != null && this.ambience != null && this.departureCamera != null && this.exitMark != null && this.departureApproachMark != null && this.leftPauseMark != null && this.drunkIdle != null && this.drunkWalk != null && this.departureFace != null && this.departureBra != null && this.departurePanties != null;
		}

		// Token: 0x06005FF8 RID: 24568 RVA: 0x00260866 File Offset: 0x0025EA66
		private IEnumerator PlayRestaurantStory()
		{
			this.Phase = LongWayHomeRestaurantSequence.StoryPhase.TableDialogue;
			yield return this.dialogue.PlayTableConversation(new Action(this.BeginDrunkFace));
			if (!this.dialogue.LastDialogueCompleted)
			{
				this.Fail("Restaurant dialogue was interrupted.");
				yield break;
			}
			this.Phase = LongWayHomeRestaurantSequence.StoryPhase.FadeToDeparture;
			yield return this.FadeScreen(true);
			if (!this.PrepareDeparture())
			{
				this.Fail("The departure outfit could not be applied.");
				yield break;
			}
			this.Phase = LongWayHomeRestaurantSequence.StoryPhase.DrunkReveal;
			this.AllowDeparturePhotos();
			yield return null;
			yield return this.FadeScreen(false);
			yield return new WaitForSeconds(0.55f);
			this.Phase = LongWayHomeRestaurantSequence.StoryPhase.Walking;
			this.BeginDepartureLookAt();
			Coroutine reaction = base.StartCoroutine(this.dialogue.PlayUndressingReaction());
			yield return this.WalkToExit();
			this.Phase = LongWayHomeRestaurantSequence.StoryPhase.AtExit;
			yield return reaction;
			if (!this.dialogue.LastDialogueCompleted)
			{
				this.Fail("Departure dialogue was interrupted.");
				yield break;
			}
			yield return new WaitForSeconds(0.25f);
			yield return this.HandoffToCity(false);
			yield break;
		}

		// Token: 0x06005FF9 RID: 24569 RVA: 0x00260875 File Offset: 0x0025EA75
		private IEnumerator HandoffToCity(bool skipWakeUp)
		{
			if (SceneTransitionManager.Instance == null || SceneTransitionManager.Instance.IsTransitioning || !Application.CanStreamedLevelBeLoaded(this.runSceneName))
			{
				this.Fail("The Long Way Home gameplay scene is unavailable.");
				yield break;
			}
			string text;
			if (!RunModeService.IsLongWayHome && !RunModeService.TryPrepareRun(RunModeKind.LongWayHome, null, out text))
			{
				this.Fail("Cannot prepare Long Way Home: " + text);
				yield break;
			}
			this.LockDepartureCameraSwitch();
			Coroutine audioFade = base.StartCoroutine(this.ambience.FadeOut(this.transitionSeconds));
			yield return this.FadeScreen(true);
			yield return audioFade;
			this.Phase = LongWayHomeRestaurantSequence.StoryPhase.Handoff;
			if (skipWakeUp)
			{
				LongWayHomeRuntimeController instance = LongWayHomeRuntimeController.Instance;
				if (instance != null)
				{
					instance.SkipNextCityWakeUp();
				}
			}
			this.ReleasePlayer();
			SceneTransitionManager.Instance.LoadGameplayScene(this.runSceneName);
			yield break;
		}

		// Token: 0x06005FFA RID: 24570 RVA: 0x0026088C File Offset: 0x0025EA8C
		private bool PrepareDeparture()
		{
			MCStyleCustomizationService componentInChildren = this._player.GetComponentInChildren<MCStyleCustomizationService>(true);
			if (!componentInChildren.SetSlotEquipped(this.tableOutfit.mcStylePartId, false) || !componentInChildren.SetSlotEquipped(this.departureBra.mcStylePartId, false) || !this.EquipUnderwear(componentInChildren, this.departurePanties))
			{
				return false;
			}
			if (this._drink != null)
			{
				Object.Destroy(this._drink);
				this._drink = null;
			}
			this.director.Stop();
			this.director.ClearGenericBinding(this.cielTrack);
			this.director.RebuildGraph();
			this.director.Play();
			Vector3 vector = this.departureApproachMark.position - this._player.position;
			vector.y = 0f;
			if (vector.sqrMagnitude > 0.001f)
			{
				this._player.rotation = Quaternion.LookRotation(vector);
				this._body.rotation = this._player.rotation;
			}
			this.CreateDepartureGraph();
			this.cinematicCamera.enabled = false;
			if (this._cinematicListener != null)
			{
				this._cinematicListener.enabled = false;
			}
			this.SetDepartureCamera(true);
			this.InitializeDepartureCameraSwitch();
			return true;
		}

		// Token: 0x06005FFB RID: 24571 RVA: 0x002609C8 File Offset: 0x0025EBC8
		private bool EquipUnderwear(MCStyleCustomizationService style, WearableItemSO item)
		{
			if (!style.EquipRuntimePreset(item.mcStylePartId, item.mcStylePreset) || style.GetSpawnedRoot(item.mcStylePartId) == null)
			{
				return false;
			}
			style.SetSlotPhysicallyHidden(item.mcStylePartId, false);
			ClothingToggleController componentInChildren = this._player.GetComponentInChildren<ClothingToggleController>(true);
			if (componentInChildren != null)
			{
				componentInChildren.SetSlotVisible(item.mcStylePartId, true);
			}
			return true;
		}

		// Token: 0x06005FFC RID: 24572 RVA: 0x00260A2C File Offset: 0x0025EC2C
		private void CreateDepartureGraph()
		{
			this._departureGraph = PlayableGraph.Create("Long Way Home - Drunk Departure");
			this._departureGraph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
			AnimationClipPlayable animationClipPlayable = AnimationClipPlayable.Create(this._departureGraph, this.drunkIdle);
			animationClipPlayable.SetApplyFootIK(false);
			this._walkPlayable = AnimationClipPlayable.Create(this._departureGraph, this.drunkWalk);
			this._walkPlayable.SetApplyFootIK(false);
			this._walkPlayable.SetSpeed((double)this.walkPlaybackSpeed);
			this._locomotionMixer = AnimationMixerPlayable.Create(this._departureGraph, 2);
			this._departureGraph.Connect<AnimationClipPlayable, AnimationMixerPlayable>(animationClipPlayable, 0, this._locomotionMixer, 0);
			this._departureGraph.Connect<AnimationClipPlayable, AnimationMixerPlayable>(this._walkPlayable, 0, this._locomotionMixer, 1);
			this._locomotionMixer.SetInputWeight(0, 1f);
			this._locomotionMixer.SetInputWeight(1, 0f);
			AnimationClipPlayable animationClipPlayable2 = AnimationClipPlayable.Create(this._departureGraph, this.departureFace);
			animationClipPlayable2.SetApplyFootIK(false);
			AnimationLayerMixerPlayable animationLayerMixerPlayable = AnimationLayerMixerPlayable.Create(this._departureGraph, 2);
			this._departureGraph.Connect<AnimationMixerPlayable, AnimationLayerMixerPlayable>(this._locomotionMixer, 0, animationLayerMixerPlayable, 0);
			this._departureGraph.Connect<AnimationClipPlayable, AnimationLayerMixerPlayable>(animationClipPlayable2, 0, animationLayerMixerPlayable, 1);
			animationLayerMixerPlayable.SetInputWeight(0, 1f);
			animationLayerMixerPlayable.SetInputWeight(1, 1f);
			AnimationPlayableOutput.Create(this._departureGraph, "Ciel - Drunk", this._animator).SetSourcePlayable(animationLayerMixerPlayable);
			this._departureGraph.Play();
			this._departureGraph.Evaluate(0f);
		}

		// Token: 0x06005FFD RID: 24573 RVA: 0x00260BA2 File Offset: 0x0025EDA2
		private IEnumerator WalkToExit()
		{
			this._walkPlayable.SetTime(0.0);
			this._walkWeight = 0f;
			this.UnlockDepartureCameraSwitch();
			this.BeginDepartureFootsteps();
			yield return this.WalkToPoint(this.departureApproachMark.position, false);
			yield return this.WalkToPoint(this.leftPauseMark.position, true);
			this.StopDepartureFootsteps();
			this.Phase = LongWayHomeRestaurantSequence.StoryPhase.DrunkPause;
			yield return new WaitForSeconds(this.leftPauseSeconds);
			this.Phase = LongWayHomeRestaurantSequence.StoryPhase.Walking;
			this._walkPlayable.SetTime(0.0);
			this.BeginDepartureFootsteps();
			yield return this.WalkToPoint(this.exitMark.position, true);
			this.StopDepartureFootsteps();
			yield break;
		}

		// Token: 0x06005FFE RID: 24574 RVA: 0x00260BB1 File Offset: 0x0025EDB1
		private IEnumerator WalkToPoint(Vector3 target, bool stopAtEnd)
		{
			target.y = this._player.position.y;
			while (Vector3.Distance(this._player.position, target) > 0.015f)
			{
				float deltaTime = Time.deltaTime;
				float num = Vector3.Distance(this._player.position, target);
				float num2 = (stopAtEnd ? Mathf.Clamp01(num / 0.2f) : 1f);
				this._walkWeight = Mathf.MoveTowards(this._walkWeight, num2, deltaTime / 0.35f);
				this.SetWalkWeight();
				Vector3 vector = target - this._player.position;
				vector.y = 0f;
				if (vector.sqrMagnitude > 0.0001f)
				{
					Quaternion quaternion = Quaternion.RotateTowards(this._player.rotation, Quaternion.LookRotation(vector), this.departureTurnDegreesPerSecond * deltaTime);
					this._player.rotation = quaternion;
					this._body.rotation = quaternion;
				}
				Vector3 vector2 = Vector3.MoveTowards(this._player.position, target, this.walkMetersPerSecond * this.walkPlaybackSpeed * this._walkWeight * deltaTime);
				this._body.position = vector2;
				this._player.position = vector2;
				this.UpdateDepartureFootsteps();
				yield return null;
			}
			this._body.position = target;
			this._player.position = target;
			while (stopAtEnd && this._walkWeight > 0f)
			{
				this._walkWeight = Mathf.MoveTowards(this._walkWeight, 0f, Time.deltaTime / 0.25f);
				this.SetWalkWeight();
				yield return null;
			}
			yield break;
		}

		// Token: 0x06005FFF RID: 24575 RVA: 0x00260BCE File Offset: 0x0025EDCE
		private void SetWalkWeight()
		{
			this._locomotionMixer.SetInputWeight(0, 1f - this._walkWeight);
			this._locomotionMixer.SetInputWeight(1, this._walkWeight);
		}

		// Token: 0x06006000 RID: 24576 RVA: 0x00260BFA File Offset: 0x0025EDFA
		private IEnumerator FadeScreen(bool toBlack)
		{
			ScreenFadeController instance = ScreenFadeController.Instance;
			if (instance == null)
			{
				yield return new WaitForSecondsRealtime(this.transitionSeconds);
				yield break;
			}
			bool done = false;
			if (toBlack)
			{
				instance.FadeOut(this.transitionSeconds, delegate
				{
					done = true;
				});
			}
			else
			{
				instance.FadeIn(this.transitionSeconds, delegate
				{
					done = true;
				});
			}
			while (!done)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x06006001 RID: 24577 RVA: 0x00260C10 File Offset: 0x0025EE10
		private void SetDepartureCamera(bool enabled)
		{
			if (this.departureCamera == null)
			{
				return;
			}
			this.departureCamera.enabled = enabled;
			AudioListener component = this.departureCamera.GetComponent<AudioListener>();
			if (component != null)
			{
				component.enabled = enabled;
			}
		}

		// Token: 0x06006002 RID: 24578 RVA: 0x00260C54 File Offset: 0x0025EE54
		private void ReleaseStory()
		{
			this.ReleaseDepartureLookAt();
			this.StopDepartureFootsteps();
			LongWayHomeRestaurantDialogue longWayHomeRestaurantDialogue = this.dialogue;
			if (longWayHomeRestaurantDialogue != null)
			{
				longWayHomeRestaurantDialogue.Abort();
			}
			RestaurantAmbienceLoop restaurantAmbienceLoop = this.ambience;
			if (restaurantAmbienceLoop != null)
			{
				restaurantAmbienceLoop.StopImmediate();
			}
			this.ReleaseDepartureCameraSwitch();
			this.SetDepartureCamera(false);
			if (this._departureGraph.IsValid())
			{
				this._departureGraph.Destroy();
			}
		}

		// Token: 0x06006003 RID: 24579 RVA: 0x00260CB4 File Offset: 0x0025EEB4
		private void BeginDepartureFootsteps()
		{
			this.StopDepartureFootsteps();
			if (this._player == null || this.drunkWalk == null || this.departureFootstepClips == null || this.departureFootstepClips.Length == 0)
			{
				return;
			}
			if (this._departureFootstepSource == null)
			{
				GameObject gameObject = new GameObject("Audio - Ciel Departure Footsteps");
				gameObject.transform.SetParent(base.transform, false);
				this._departureFootstepSource = gameObject.AddComponent<AudioSource>();
				this._departureFootstepSource.playOnAwake = false;
				this._departureFootstepSource.loop = false;
				this._departureFootstepSource.spatialBlend = 0f;
				this._departureFootstepSource.dopplerLevel = 0f;
				this._departureFootstepSource.pitch = 1f;
				this._departureFootstepSource.volume = 1f;
			}
			foreach (AudioClip audioClip in this.departureFootstepClips)
			{
				if (audioClip != null && audioClip.loadState == AudioDataLoadState.Unloaded)
				{
					audioClip.LoadAudioData();
				}
			}
			this._previousFootstepWalkTime = (this._walkPlayable.IsValid<AnimationClipPlayable>() ? this._walkPlayable.GetTime<AnimationClipPlayable>() : 0.0);
			this._previousFootstepPosition = this._player.position;
			this._departureFootstepsRunning = true;
		}

		// Token: 0x06006004 RID: 24580 RVA: 0x00260DFC File Offset: 0x0025EFFC
		private void UpdateDepartureFootsteps()
		{
			if (!this._departureFootstepsRunning)
			{
				return;
			}
			if (this._player == null || this.drunkWalk == null || !this._walkPlayable.IsValid<AnimationClipPlayable>())
			{
				this.StopDepartureFootsteps();
				return;
			}
			if (!this.ConsumeDepartureFootContact(this._walkPlayable.GetTime<AnimationClipPlayable>(), this._player.position, (double)this.drunkWalk.length, this._walkWeight))
			{
				return;
			}
			if (this._departureFootstepSource == null || this.departureFootstepClips == null)
			{
				return;
			}
			for (int i = 0; i < this.departureFootstepClips.Length; i++)
			{
				this._lastDepartureFootstepClip = (this._lastDepartureFootstepClip + 1) % this.departureFootstepClips.Length;
				AudioClip audioClip = this.departureFootstepClips[this._lastDepartureFootstepClip];
				if (!(audioClip == null))
				{
					this._departureFootstepSource.PlayOneShot(audioClip, this.departureFootstepVolume);
					this._departureFootstepsPlayed++;
					return;
				}
			}
		}

		// Token: 0x06006005 RID: 24581 RVA: 0x00260EEC File Offset: 0x0025F0EC
		private bool ConsumeDepartureFootContact(double time, Vector3 position, double duration, float walkWeight)
		{
			double previousFootstepWalkTime = this._previousFootstepWalkTime;
			Vector3 vector = position - this._previousFootstepPosition;
			vector.y = 0f;
			this._previousFootstepWalkTime = time;
			this._previousFootstepPosition = position;
			return walkWeight > 0.25f && vector.sqrMagnitude > 1E-08f && (LongWayHomeRestaurantSequence.CrossedDepartureFootContact(previousFootstepWalkTime, time, duration, this.leftFootContactPhase) || LongWayHomeRestaurantSequence.CrossedDepartureFootContact(previousFootstepWalkTime, time, duration, this.rightFootContactPhase));
		}

		// Token: 0x06006006 RID: 24582 RVA: 0x00260F60 File Offset: 0x0025F160
		private static bool CrossedDepartureFootContact(double previous, double current, double duration, float phase)
		{
			if (duration <= 0.0 || double.IsNaN(duration) || double.IsInfinity(duration) || double.IsNaN(previous) || double.IsInfinity(previous) || double.IsNaN(current) || double.IsInfinity(current) || current <= previous || current - previous > duration)
			{
				return false;
			}
			double num = (double)phase - Math.Floor((double)phase);
			return Math.Floor(current / duration - num) > Math.Floor(previous / duration - num);
		}

		// Token: 0x06006007 RID: 24583 RVA: 0x00260FD6 File Offset: 0x0025F1D6
		private void StopDepartureFootsteps()
		{
			this._departureFootstepsRunning = false;
			if (this._departureFootstepSource != null)
			{
				this._departureFootstepSource.Stop();
			}
		}

		// Token: 0x06006008 RID: 24584 RVA: 0x00260FF8 File Offset: 0x0025F1F8
		private void BeginDepartureLookAt()
		{
			this.ReleaseDepartureLookAt();
			if (this._animator == null || this.director == null || this.director.playableAsset == null)
			{
				return;
			}
			this._departureGazeTarget = this._animator.GetBoneTransform(HumanBodyBones.Head);
			if (this._departureGazeTarget == null)
			{
				this._departureGazeTarget = this._player;
			}
			HashSet<Animator> hashSet = new HashSet<Animator>();
			foreach (PlayableBinding playableBinding in this.director.playableAsset.outputs)
			{
				AnimationTrack animationTrack = playableBinding.sourceObject as AnimationTrack;
				if (animationTrack != null && !(animationTrack == this.cielTrack))
				{
					Object genericBinding = this.director.GetGenericBinding(animationTrack);
					Animator animator = genericBinding as Animator;
					if (animator == null)
					{
						GameObject gameObject = genericBinding as GameObject;
						if (gameObject != null)
						{
							animator = gameObject.GetComponent<Animator>();
						}
					}
					if (!(animator == null) && !(animator == this._animator) && animator.isHuman && !(animator.gameObject.scene != base.gameObject.scene) && hashSet.Add(animator) && (!(this.editorPreview != null) || !animator.transform.IsChildOf(this.editorPreview.transform)))
					{
						Transform boneTransform = animator.GetBoneTransform(HumanBodyBones.Head);
						if (!(boneTransform == null))
						{
							this._departureGazeActors.Add(new LongWayHomeRestaurantSequence.DepartureGazeActor
							{
								Animator = animator,
								Neck = new LongWayHomeRestaurantSequence.DepartureGazeBone(animator.GetBoneTransform(HumanBodyBones.Neck)),
								Head = new LongWayHomeRestaurantSequence.DepartureGazeBone(boneTransform),
								LeftEye = new LongWayHomeRestaurantSequence.DepartureGazeBone(LongWayHomeRestaurantSequence.FindDepartureEye(animator, HumanBodyBones.LeftEye, "eye_l")),
								RightEye = new LongWayHomeRestaurantSequence.DepartureGazeBone(LongWayHomeRestaurantSequence.FindDepartureEye(animator, HumanBodyBones.RightEye, "eye_r")),
								HeadForwardLocal = boneTransform.InverseTransformDirection(animator.transform.forward),
								HeadUpLocal = boneTransform.InverseTransformDirection(animator.transform.up)
							});
						}
					}
				}
			}
		}

		// Token: 0x06006009 RID: 24585 RVA: 0x00261258 File Offset: 0x0025F458
		private void UpdateDepartureLookAt()
		{
			if (this._departureGazeTarget == null)
			{
				return;
			}
			float deltaTime = Time.deltaTime;
			float num = 1f - Mathf.Exp(-this.departureLookSmoothing * deltaTime);
			float num2 = 1f - Mathf.Exp(-12f * deltaTime);
			Vector3 position = this._departureGazeTarget.position;
			foreach (LongWayHomeRestaurantSequence.DepartureGazeActor departureGazeActor in this._departureGazeActors)
			{
				if (!(departureGazeActor.Animator == null) && !(departureGazeActor.Head.Bone == null))
				{
					departureGazeActor.Neck.ReadAnimatedPose();
					departureGazeActor.Head.ReadAnimatedPose();
					departureGazeActor.LeftEye.ReadAnimatedPose();
					departureGazeActor.RightEye.ReadAnimatedPose();
					Transform transform = departureGazeActor.Animator.transform;
					Vector2 vector = LongWayHomeRestaurantSequence.DepartureLookAngles(transform.InverseTransformDirection(position - departureGazeActor.Head.Bone.position), this.departureHeadYawLimit, this.departureHeadPitchLimit);
					departureGazeActor.HeadAngles = Vector2.Lerp(departureGazeActor.HeadAngles, vector, num);
					float num3 = ((departureGazeActor.Neck.Bone != null) ? 0.25f : 0f);
					departureGazeActor.Neck.ApplyWorldOffset(LongWayHomeRestaurantSequence.DepartureLookOffset(transform.rotation, departureGazeActor.HeadAngles * num3));
					departureGazeActor.Head.ApplyWorldOffset(LongWayHomeRestaurantSequence.DepartureLookOffset(transform.rotation, departureGazeActor.HeadAngles * (1f - num3)));
					Quaternion quaternion = Quaternion.LookRotation(departureGazeActor.Head.Bone.TransformDirection(departureGazeActor.HeadForwardLocal), departureGazeActor.Head.Bone.TransformDirection(departureGazeActor.HeadUpLocal));
					Vector3 vector2 = departureGazeActor.Head.Bone.position;
					if (departureGazeActor.LeftEye.Bone != null && departureGazeActor.RightEye.Bone != null)
					{
						vector2 = (departureGazeActor.LeftEye.Bone.position + departureGazeActor.RightEye.Bone.position) * 0.5f;
					}
					Vector3 vector3 = Quaternion.Inverse(quaternion) * (position - vector2);
					departureGazeActor.EyeAngles = Vector2.Lerp(departureGazeActor.EyeAngles, LongWayHomeRestaurantSequence.DepartureLookAngles(vector3, 20f, 12f), num2);
					Quaternion quaternion2 = LongWayHomeRestaurantSequence.DepartureLookOffset(quaternion, departureGazeActor.EyeAngles);
					departureGazeActor.LeftEye.ApplyWorldOffset(quaternion2);
					departureGazeActor.RightEye.ApplyWorldOffset(quaternion2);
				}
			}
		}

		// Token: 0x0600600A RID: 24586 RVA: 0x0026152C File Offset: 0x0025F72C
		private static Vector2 DepartureLookAngles(Vector3 direction, float yawLimit, float pitchLimit)
		{
			if (direction.sqrMagnitude < 1E-06f)
			{
				return Vector2.zero;
			}
			float num = Mathf.Atan2(direction.x, direction.z) * 57.29578f;
			float num2 = Mathf.Sqrt(direction.x * direction.x + direction.z * direction.z);
			float num3 = -Mathf.Atan2(direction.y, num2) * 57.29578f;
			return new Vector2(Mathf.Clamp(num, -yawLimit, yawLimit), Mathf.Clamp(num3, -pitchLimit, pitchLimit));
		}

		// Token: 0x0600600B RID: 24587 RVA: 0x002615AF File Offset: 0x0025F7AF
		private static Quaternion DepartureLookOffset(Quaternion frame, Vector2 angles)
		{
			return frame * Quaternion.Euler(angles.y, angles.x, 0f) * Quaternion.Inverse(frame);
		}

		// Token: 0x0600600C RID: 24588 RVA: 0x002615D8 File Offset: 0x0025F7D8
		private static Transform FindDepartureEye(Animator actor, HumanBodyBones humanoidBone, string fallbackName)
		{
			Transform boneTransform = actor.GetBoneTransform(humanoidBone);
			if (boneTransform != null)
			{
				return boneTransform;
			}
			foreach (Transform transform in actor.GetComponentsInChildren<Transform>(true))
			{
				if (transform.name == fallbackName)
				{
					return transform;
				}
			}
			return null;
		}

		// Token: 0x0600600D RID: 24589 RVA: 0x00261624 File Offset: 0x0025F824
		private void ReleaseDepartureLookAt()
		{
			foreach (LongWayHomeRestaurantSequence.DepartureGazeActor departureGazeActor in this._departureGazeActors)
			{
				departureGazeActor.Neck.Release();
				departureGazeActor.Head.Release();
				departureGazeActor.LeftEye.Release();
				departureGazeActor.RightEye.Release();
			}
			this._departureGazeActors.Clear();
			this._departureGazeTarget = null;
		}

		// Token: 0x0600600E RID: 24590 RVA: 0x002616AC File Offset: 0x0025F8AC
		private void CaptureCinematicPhotoReactions()
		{
			this._cinematicPhotoReactions.Clear();
			GameObject[] rootGameObjects = base.gameObject.scene.GetRootGameObjects();
			for (int i = 0; i < rootGameObjects.Length; i++)
			{
				foreach (SeatedRandomReaction seatedRandomReaction in rootGameObjects[i].GetComponentsInChildren<SeatedRandomReaction>(true))
				{
					this._cinematicPhotoReactions.Add(seatedRandomReaction);
					seatedRandomReaction.SetPresentationCaptureAllowed(this, false);
				}
			}
		}

		// Token: 0x0600600F RID: 24591 RVA: 0x00261720 File Offset: 0x0025F920
		private void AllowDeparturePhotos()
		{
			foreach (SeatedRandomReaction seatedRandomReaction in this._cinematicPhotoReactions)
			{
				if (seatedRandomReaction != null)
				{
					seatedRandomReaction.SetPresentationCaptureAllowed(this, true);
				}
			}
		}

		// Token: 0x06006010 RID: 24592 RVA: 0x00261780 File Offset: 0x0025F980
		private void StopCinematicPhotoReactions()
		{
			foreach (SeatedRandomReaction seatedRandomReaction in this._cinematicPhotoReactions)
			{
				if (!(seatedRandomReaction == null))
				{
					seatedRandomReaction.SetPresentationCaptureAllowed(this, false);
					foreach (SmartphoneFlash smartphoneFlash in seatedRandomReaction.GetComponentsInChildren<SmartphoneFlash>(true))
					{
						smartphoneFlash.StopFlash();
						AudioSource component = smartphoneFlash.GetComponent<AudioSource>();
						if (component != null)
						{
							component.Stop();
						}
					}
				}
			}
		}

		// Token: 0x06006011 RID: 24593 RVA: 0x00261814 File Offset: 0x0025FA14
		private bool PrepareRestaurantPopulation(out string error)
		{
			error = null;
			NPCRosterSnapshot currentSnapshot = NPCRosterSession.GetCurrentSnapshot();
			HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
			foreach (Animator animator in this.FindRestaurantPopulationActors())
			{
				SkinnedMeshRenderer skinnedMeshRenderer = LongWayHomeRestaurantSequence.FindRestaurantSkin(animator);
				string text = LongWayHomeRestaurantSequence.RestaurantResourceName(skinnedMeshRenderer);
				NPCRosterEntry npcrosterEntry = LongWayHomeRestaurantSequence.ChooseRestaurantNpc(currentSnapshot, text, hashSet, (string resource) => NPCBakedResourceIndex.LoadPrefab(resource) != null);
				if (npcrosterEntry == null || skinnedMeshRenderer == null)
				{
					error = "The selected NPC population has no compatible restaurant cast.";
					return false;
				}
				GameObject gameObject = NPCBakedResourceIndex.LoadPrefab(npcrosterEntry.resourceName);
				CharacterObject characterObject = NPCBakedResourceIndex.LoadCharacter(npcrosterEntry.resourceName);
				Animator animator2 = ((gameObject != null) ? gameObject.GetComponent<Animator>() : null);
				SkinnedMeshRenderer skinnedMeshRenderer2 = LongWayHomeRestaurantSequence.FindRestaurantSkin(animator2);
				if (!(animator2 == null) && animator2.isHuman && !(skinnedMeshRenderer2 == null))
				{
					bool flag;
					if (characterObject == null)
					{
						flag = null != null;
					}
					else
					{
						CharacterData data = characterObject.data;
						flag = ((data != null) ? data.generalInfo : null) != null;
					}
					if (flag && characterObject.data.generalInfo.gender == npcrosterEntry.gender)
					{
						LongWayHomeRestaurantSequence.RestaurantNpcAppearance restaurantNpcAppearance = LongWayHomeRestaurantSequence.CaptureRestaurantAppearance(animator, skinnedMeshRenderer);
						this._restaurantNpcAppearances.Add(restaurantNpcAppearance);
						if (LongWayHomeRestaurantSequence.RestaurantSkinNeedsRetargeting(skinnedMeshRenderer, skinnedMeshRenderer2))
						{
							try
							{
								LongWayHomeRestaurantSequence.ApplyRestaurantAppearance(restaurantNpcAppearance, animator2, skinnedMeshRenderer2);
							}
							catch (Exception ex)
							{
								error = "The selected restaurant NPC could not be retargeted: " + ex.Message;
								return false;
							}
						}
						if (restaurantNpcAppearance.Dialogue != null)
						{
							restaurantNpcAppearance.Dialogue.Initialize(characterObject.data.generalInfo);
							restaurantNpcAppearance.Dialogue.SetNpcIdentity(npcrosterEntry.presetId);
							restaurantNpcAppearance.Dialogue.PersonalityProfile = restaurantNpcAppearance.PersonalityProfile;
						}
						NPCReactionAudioPlayer component = animator.GetComponent<NPCReactionAudioPlayer>();
						if (component != null)
						{
							component.UpdateGender(npcrosterEntry.gender);
						}
						hashSet.Add(npcrosterEntry.presetId);
						continue;
					}
				}
				error = "The selected restaurant NPC is missing its baked character rig.";
				return false;
			}
			return true;
		}

		// Token: 0x06006012 RID: 24594 RVA: 0x00261A58 File Offset: 0x0025FC58
		private List<Animator> FindRestaurantPopulationActors()
		{
			List<Animator> list = new List<Animator>();
			if (this.director != null && this.director.playableAsset != null)
			{
				foreach (PlayableBinding playableBinding in this.director.playableAsset.outputs)
				{
					AnimationTrack animationTrack = playableBinding.sourceObject as AnimationTrack;
					if (animationTrack != null && !(animationTrack == this.cielTrack))
					{
						Object genericBinding = this.director.GetGenericBinding(animationTrack);
						Animator animator = genericBinding as Animator;
						if (animator == null)
						{
							GameObject gameObject = genericBinding as GameObject;
							if (gameObject != null)
							{
								animator = gameObject.GetComponent<Animator>();
							}
						}
						this.AddRestaurantActor(list, animator);
					}
				}
			}
			GameObject[] rootGameObjects = base.gameObject.scene.GetRootGameObjects();
			for (int i = 0; i < rootGameObjects.Length; i++)
			{
				foreach (SeatedRandomReaction seatedRandomReaction in rootGameObjects[i].GetComponentsInChildren<SeatedRandomReaction>(true))
				{
					this.AddRestaurantActor(list, seatedRandomReaction.GetComponent<Animator>());
				}
			}
			return list;
		}

		// Token: 0x06006013 RID: 24595 RVA: 0x00261B94 File Offset: 0x0025FD94
		private void AddRestaurantActor(List<Animator> actors, Animator actor)
		{
			if (actor == null || !actor.isHuman || actor.gameObject.scene != base.gameObject.scene || actors.Contains(actor) || actor == this._animator || (this.editorPreview != null && actor.transform.IsChildOf(this.editorPreview.transform)))
			{
				return;
			}
			actors.Add(actor);
		}

		// Token: 0x06006014 RID: 24596 RVA: 0x00261C14 File Offset: 0x0025FE14
		private static NPCRosterEntry ChooseRestaurantNpc(NPCRosterSnapshot roster, string preferred, HashSet<string> used, Func<string, bool> hasPrefab)
		{
			if (((roster != null) ? roster.entries : null) == null || hasPrefab == null)
			{
				return null;
			}
			foreach (NPCRosterEntry npcrosterEntry in roster.entries)
			{
				if (LongWayHomeRestaurantSequence.IsRestaurantEntryEligible(roster, npcrosterEntry) && npcrosterEntry.resourceName == preferred && (used == null || !used.Contains(npcrosterEntry.presetId)) && hasPrefab(npcrosterEntry.resourceName))
				{
					return npcrosterEntry;
				}
			}
			foreach (NPCRosterEntry npcrosterEntry2 in roster.entries)
			{
				if (LongWayHomeRestaurantSequence.IsRestaurantEntryEligible(roster, npcrosterEntry2) && (used == null || !used.Contains(npcrosterEntry2.presetId)) && hasPrefab(npcrosterEntry2.resourceName))
				{
					return npcrosterEntry2;
				}
			}
			foreach (NPCRosterEntry npcrosterEntry3 in roster.entries)
			{
				if (LongWayHomeRestaurantSequence.IsRestaurantEntryEligible(roster, npcrosterEntry3) && hasPrefab(npcrosterEntry3.resourceName))
				{
					return npcrosterEntry3;
				}
			}
			return null;
		}

		// Token: 0x06006015 RID: 24597 RVA: 0x00261D74 File Offset: 0x0025FF74
		private static bool IsRestaurantEntryEligible(NPCRosterSnapshot roster, NPCRosterEntry entry)
		{
			return entry != null && !entry.isCustom && !string.IsNullOrEmpty(entry.resourceName) && NPCRosterSession.MatchesPopulation(roster.populationMode, entry.gender);
		}

		// Token: 0x06006016 RID: 24598 RVA: 0x00261DA4 File Offset: 0x0025FFA4
		private static SkinnedMeshRenderer FindRestaurantSkin(Animator actor)
		{
			if (actor == null)
			{
				return null;
			}
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in actor.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				if (skinnedMeshRenderer.sharedMesh != null && skinnedMeshRenderer.sharedMesh.name.StartsWith("NPC_", StringComparison.Ordinal) && skinnedMeshRenderer.sharedMesh.name.EndsWith("_Mesh", StringComparison.Ordinal))
				{
					return skinnedMeshRenderer;
				}
			}
			return null;
		}

		// Token: 0x06006017 RID: 24599 RVA: 0x00261E18 File Offset: 0x00260018
		private static string RestaurantResourceName(SkinnedMeshRenderer skin)
		{
			if (!(((skin != null) ? skin.sharedMesh : null) != null) || !skin.sharedMesh.name.EndsWith("_Mesh", StringComparison.Ordinal))
			{
				return null;
			}
			return skin.sharedMesh.name.Substring(0, skin.sharedMesh.name.Length - 5);
		}

		// Token: 0x06006018 RID: 24600 RVA: 0x00261E78 File Offset: 0x00260078
		private static bool RestaurantSkinNeedsRetargeting(SkinnedMeshRenderer skin, SkinnedMeshRenderer sourceSkin)
		{
			if (skin.sharedMesh != sourceSkin.sharedMesh || skin.rootBone == null)
			{
				return true;
			}
			Transform[] bones = skin.bones;
			Transform[] bones2 = sourceSkin.bones;
			if (bones.Length != bones2.Length)
			{
				return true;
			}
			for (int i = 0; i < bones.Length; i++)
			{
				if (bones[i] == null || bones2[i] == null || bones[i].name != bones2[i].name)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006019 RID: 24601 RVA: 0x00261F00 File Offset: 0x00260100
		private static LongWayHomeRestaurantSequence.RestaurantNpcAppearance CaptureRestaurantAppearance(Animator actor, SkinnedMeshRenderer skin)
		{
			LongWayHomeRestaurantSequence.RestaurantNpcAppearance restaurantNpcAppearance = new LongWayHomeRestaurantSequence.RestaurantNpcAppearance
			{
				Actor = actor,
				Avatar = actor.avatar,
				Renderer = skin,
				Mesh = skin.sharedMesh,
				Materials = skin.sharedMaterials,
				Bones = skin.bones,
				RootBone = skin.rootBone,
				Bounds = skin.localBounds,
				Shapes = new float[skin.sharedMesh.blendShapeCount],
				Dialogue = actor.GetComponent<NPCDialogueHolder>()
			};
			for (int i = 0; i < restaurantNpcAppearance.Shapes.Length; i++)
			{
				restaurantNpcAppearance.Shapes[i] = skin.GetBlendShapeWeight(i);
			}
			if (restaurantNpcAppearance.Dialogue != null)
			{
				restaurantNpcAppearance.Info = new CharacterGeneralInfo
				{
					characterName = restaurantNpcAppearance.Dialogue.FirstName,
					surname = restaurantNpcAppearance.Dialogue.Surname,
					gender = restaurantNpcAppearance.Dialogue.Gender,
					personality = restaurantNpcAppearance.Dialogue.Personality,
					occupation = restaurantNpcAppearance.Dialogue.Occupation
				};
				restaurantNpcAppearance.Identity = restaurantNpcAppearance.Dialogue.NpcIdentityId;
				restaurantNpcAppearance.PersonalityProfile = restaurantNpcAppearance.Dialogue.PersonalityProfile;
			}
			return restaurantNpcAppearance;
		}

		// Token: 0x0600601A RID: 24602 RVA: 0x00262044 File Offset: 0x00260244
		private static void ApplyRestaurantAppearance(LongWayHomeRestaurantSequence.RestaurantNpcAppearance state, Animator sourceActor, SkinnedMeshRenderer sourceSkin)
		{
			Dictionary<Transform, Transform> dictionary = new Dictionary<Transform, Transform>();
			Transform transform = sourceActor.transform;
			dictionary[transform] = state.Actor.transform;
			Dictionary<Transform, Transform> dictionary2 = dictionary;
			Transform transform2 = sourceActor.transform.Find("armature");
			if (transform2 == null)
			{
				throw new InvalidOperationException("Restaurant NPC has no baked armature.");
			}
			LongWayHomeRestaurantSequence.CopyRestaurantBone(transform2, state.Actor.transform, state, dictionary2);
			Transform[] bones = sourceSkin.bones;
			Transform[] array = new Transform[bones.Length];
			for (int i = 0; i < bones.Length; i++)
			{
				if (bones[i] != null && !dictionary2.TryGetValue(bones[i], out array[i]))
				{
					throw new InvalidOperationException("Restaurant NPC skin bone is outside its armature.");
				}
			}
			Transform transform3;
			if (sourceSkin.rootBone == null)
			{
				transform3 = null;
			}
			else if (!dictionary2.TryGetValue(sourceSkin.rootBone, out transform3))
			{
				throw new InvalidOperationException("Restaurant NPC skin root is outside its armature.");
			}
			state.Renderer.sharedMesh = sourceSkin.sharedMesh;
			state.Renderer.sharedMaterials = sourceSkin.sharedMaterials;
			state.Renderer.bones = array;
			state.Renderer.rootBone = transform3;
			state.Renderer.localBounds = sourceSkin.localBounds;
			for (int j = 0; j < sourceSkin.sharedMesh.blendShapeCount; j++)
			{
				state.Renderer.SetBlendShapeWeight(j, sourceSkin.GetBlendShapeWeight(j));
			}
			state.Actor.avatar = sourceActor.avatar;
			state.Actor.Rebind();
			NPCSimpleLookAt component = state.Actor.GetComponent<NPCSimpleLookAt>();
			if (component == null)
			{
				return;
			}
			component.ReInitializeAfterMerge();
		}

		// Token: 0x0600601B RID: 24603 RVA: 0x002621D0 File Offset: 0x002603D0
		private static void CopyRestaurantBone(Transform source, Transform parent, LongWayHomeRestaurantSequence.RestaurantNpcAppearance state, Dictionary<Transform, Transform> mapping)
		{
			if (source.GetComponent<NPCProp>() != null || source.name == "Smartphone")
			{
				return;
			}
			Transform transform = parent.Find(source.name);
			if (transform == null)
			{
				transform = new GameObject(source.name).transform;
				transform.SetParent(parent, false);
				state.AddedBones.Add(transform.gameObject);
			}
			else
			{
				state.Poses.Add(new LongWayHomeRestaurantSequence.RestaurantBonePose
				{
					Bone = transform,
					Position = transform.localPosition,
					Rotation = transform.localRotation,
					Scale = transform.localScale
				});
			}
			transform.localPosition = source.localPosition;
			transform.localRotation = source.localRotation;
			transform.localScale = source.localScale;
			mapping.Add(source, transform);
			for (int i = 0; i < source.childCount; i++)
			{
				LongWayHomeRestaurantSequence.CopyRestaurantBone(source.GetChild(i), transform, state, mapping);
			}
		}

		// Token: 0x0600601C RID: 24604 RVA: 0x002622C8 File Offset: 0x002604C8
		private void ReleaseRestaurantPopulation()
		{
			foreach (LongWayHomeRestaurantSequence.RestaurantNpcAppearance restaurantNpcAppearance in this._restaurantNpcAppearances)
			{
				LongWayHomeRestaurantSequence.RestoreRestaurantAppearance(restaurantNpcAppearance);
			}
			this._restaurantNpcAppearances.Clear();
		}

		// Token: 0x0600601D RID: 24605 RVA: 0x00262324 File Offset: 0x00260524
		private static void RestoreRestaurantAppearance(LongWayHomeRestaurantSequence.RestaurantNpcAppearance state)
		{
			if (state.Actor == null)
			{
				return;
			}
			if (state.Renderer != null)
			{
				state.Renderer.sharedMesh = state.Mesh;
				state.Renderer.sharedMaterials = state.Materials;
				state.Renderer.bones = state.Bones;
				state.Renderer.rootBone = state.RootBone;
				state.Renderer.localBounds = state.Bounds;
				for (int i = 0; i < state.Shapes.Length; i++)
				{
					state.Renderer.SetBlendShapeWeight(i, state.Shapes[i]);
				}
			}
			state.Actor.avatar = state.Avatar;
			for (int j = state.AddedBones.Count - 1; j >= 0; j--)
			{
				if (state.AddedBones[j] != null)
				{
					state.AddedBones[j].SetActive(false);
					if (Application.isPlaying)
					{
						Object.Destroy(state.AddedBones[j]);
					}
					else
					{
						Object.DestroyImmediate(state.AddedBones[j]);
					}
				}
			}
			foreach (LongWayHomeRestaurantSequence.RestaurantBonePose restaurantBonePose in state.Poses)
			{
				if (restaurantBonePose.Bone != null)
				{
					restaurantBonePose.Bone.localPosition = restaurantBonePose.Position;
					restaurantBonePose.Bone.localRotation = restaurantBonePose.Rotation;
					restaurantBonePose.Bone.localScale = restaurantBonePose.Scale;
				}
			}
			if (state.Dialogue != null)
			{
				state.Dialogue.Initialize(state.Info);
				state.Dialogue.SetNpcIdentity(state.Identity);
				state.Dialogue.PersonalityProfile = state.PersonalityProfile;
				NPCReactionAudioPlayer component = state.Actor.GetComponent<NPCReactionAudioPlayer>();
				if (component == null)
				{
					return;
				}
				component.UpdateGender(state.Info.gender);
			}
		}

		// Token: 0x0600601E RID: 24606 RVA: 0x00262528 File Offset: 0x00260728
		private bool CaptureCinematicWardrobe()
		{
			if (this._wardrobeSnapshot != null)
			{
				return true;
			}
			if (this._player == null)
			{
				return false;
			}
			this._wardrobeService = this._player.GetComponentInChildren<MCStyleCustomizationService>(true);
			if (this._wardrobeService == null)
			{
				return false;
			}
			this._wardrobePresentation = WardrobeGameplayContext.BeginPresentationOverride(this);
			this._wardrobeWearController = this._player.GetComponentInChildren<WearableUseController>(true);
			this._wardrobeWearWasEnabled = this._wardrobeWearController != null && this._wardrobeWearController.enabled;
			if (this._wardrobeWearController != null)
			{
				this._wardrobeWearController.enabled = false;
			}
			bool flag;
			try
			{
				this._wardrobeSnapshot = this._wardrobeService.CaptureWardrobeState(this._player.GetComponentInChildren<ClothingToggleController>(true));
				flag = true;
			}
			catch
			{
				this.RestoreCinematicWardrobe();
				throw;
			}
			return flag;
		}

		// Token: 0x0600601F RID: 24607 RVA: 0x00262608 File Offset: 0x00260808
		private void RestoreCinematicWardrobe()
		{
			MCStyleCustomizationService.WardrobeSnapshot wardrobeSnapshot = this._wardrobeSnapshot;
			this._wardrobeSnapshot = null;
			WearableUseController wardrobeWearController = this._wardrobeWearController;
			this._wardrobeWearController = null;
			try
			{
				if (this._wardrobeService != null && wardrobeSnapshot != null)
				{
					this._wardrobeService.RestoreWardrobeState(wardrobeSnapshot);
				}
			}
			finally
			{
				this._wardrobeService = null;
				try
				{
					if (wardrobeWearController != null)
					{
						wardrobeWearController.enabled = this._wardrobeWearWasEnabled;
					}
				}
				finally
				{
					IDisposable wardrobePresentation = this._wardrobePresentation;
					if (wardrobePresentation != null)
					{
						wardrobePresentation.Dispose();
					}
					this._wardrobePresentation = null;
				}
			}
		}

		// Token: 0x0400613C RID: 24892
		private InteractionHUDController _departureCameraHud;

		// Token: 0x0400613D RID: 24893
		private InteractionConfigSO _departureCameraHintConfig;

		// Token: 0x0400613E RID: 24894
		private Canvas _departureCameraHudCanvas;

		// Token: 0x0400613F RID: 24895
		private bool _departureCameraHudCanvasCreated;

		// Token: 0x04006140 RID: 24896
		private bool _departureCameraHudCanvasEnabled;

		// Token: 0x04006141 RID: 24897
		private bool _departureCameraHudOverrideSorting;

		// Token: 0x04006142 RID: 24898
		private int _departureCameraHudSortingOrder;

		// Token: 0x04006143 RID: 24899
		private bool _hintShowsCamera;

		// Token: 0x04006144 RID: 24900
		private bool _hintShowsSkip;

		// Token: 0x04006145 RID: 24901
		[Header("Departure camera switch")]
		[SerializeField]
		private Camera departureFaceCamera;

		// Token: 0x04006146 RID: 24902
		[Tooltip("Follows Ciel while preserving the position and rotation authored on this rig. The child Camera remains editable too.")]
		[SerializeField]
		private Transform departureFaceCameraRig;

		// Token: 0x04006147 RID: 24903
		[Tooltip("Reference pose under Ciel - Mark. The rig's framing is measured relative to this marker before following the runtime player.")]
		[SerializeField]
		private Transform departureFaceCameraReference;

		// Token: 0x04006148 RID: 24904
		[SerializeField]
		private Vector3 departureFaceTargetOffset = new Vector3(0f, 0.1f, 0f);

		// Token: 0x04006149 RID: 24905
		private Transform _departureFaceHead;

		// Token: 0x0400614A RID: 24906
		private bool _departureCameraSwitchUnlocked;

		// Token: 0x0400614B RID: 24907
		private bool _usingDepartureFaceCamera;

		// Token: 0x0400614C RID: 24908
		private bool _departureCameraRigCaptured;

		// Token: 0x0400614D RID: 24909
		private Vector3 _departureCameraRigPosition;

		// Token: 0x0400614E RID: 24910
		private Quaternion _departureCameraRigRotation;

		// Token: 0x0400614F RID: 24911
		private Vector3 _departureCameraPositionOffset;

		// Token: 0x04006150 RID: 24912
		private Vector3 _departureCameraLastPosition;

		// Token: 0x04006151 RID: 24913
		private Quaternion _departureCameraRotationOffset;

		// Token: 0x04006152 RID: 24914
		private Quaternion _departureCameraLastRotation;

		// Token: 0x04006153 RID: 24915
		private Quaternion _departureCameraLastFollowRotation;

		// Token: 0x04006154 RID: 24916
		private bool _departureCameraHasAppliedPose;

		// Token: 0x04006155 RID: 24917
		[Header("Authored scene — preserve these transforms")]
		[SerializeField]
		private PlayableDirector director;

		// Token: 0x04006156 RID: 24918
		[SerializeField]
		private AnimationTrack cielTrack;

		// Token: 0x04006157 RID: 24919
		[SerializeField]
		private Transform playerMark;

		// Token: 0x04006158 RID: 24920
		[SerializeField]
		private Camera cinematicCamera;

		// Token: 0x04006159 RID: 24921
		[SerializeField]
		private GameObject editorPreview;

		// Token: 0x0400615A RID: 24922
		[Tooltip("The authored bottle under the preview's left hand. Its local grip is copied verbatim.")]
		[SerializeField]
		private GameObject drinkTemplate;

		// Token: 0x0400615B RID: 24923
		[FormerlySerializedAs("gymOutfit")]
		[SerializeField]
		private WearableItemSO tableOutfit;

		// Token: 0x0400615C RID: 24924
		[SerializeField]
		[Min(5f)]
		private float readinessTimeout = 60f;

		// Token: 0x0400615D RID: 24925
		[Header("Restaurant lighting (independent of the source scene)")]
		[SerializeField]
		private Material authoredSky;

		// Token: 0x0400615E RID: 24926
		[SerializeField]
		private AmbientMode authoredAmbientMode = AmbientMode.Flat;

		// Token: 0x0400615F RID: 24927
		[SerializeField]
		private Color authoredAmbient = new Color(0.34f, 0.35f, 0.4f);

		// Token: 0x04006160 RID: 24928
		[SerializeField]
		private bool authoredFog;

		// Token: 0x04006161 RID: 24929
		private readonly List<LongWayHomeRestaurantSequence.BehaviourState> _presentation = new List<LongWayHomeRestaurantSequence.BehaviourState>();

		// Token: 0x04006162 RID: 24930
		private readonly List<AudioSource> _mutedAudio = new List<AudioSource>();

		// Token: 0x04006163 RID: 24931
		private Transform _player;

		// Token: 0x04006164 RID: 24932
		private Animator _animator;

		// Token: 0x04006165 RID: 24933
		private vThirdPersonInput _input;

		// Token: 0x04006166 RID: 24934
		private vThirdPersonCamera _gameplayCamera;

		// Token: 0x04006167 RID: 24935
		private Rigidbody _body;

		// Token: 0x04006168 RID: 24936
		private CoverPoseController _coverPose;

		// Token: 0x04006169 RID: 24937
		private PlayerActivityLease _lease;

		// Token: 0x0400616A RID: 24938
		private GameObject _drink;

		// Token: 0x0400616B RID: 24939
		private bool _captured;

		// Token: 0x0400616C RID: 24940
		private bool _inputEnabled;

		// Token: 0x0400616D RID: 24941
		private bool _inputLock;

		// Token: 0x0400616E RID: 24942
		private bool _cameraLock;

		// Token: 0x0400616F RID: 24943
		private bool _cameraFrozen;

		// Token: 0x04006170 RID: 24944
		private bool _coverEnabled;

		// Token: 0x04006171 RID: 24945
		private bool _rootMotion;

		// Token: 0x04006172 RID: 24946
		private bool _kinematic;

		// Token: 0x04006173 RID: 24947
		private bool _gravity;

		// Token: 0x04006174 RID: 24948
		private RigidbodyConstraints _constraints;

		// Token: 0x04006175 RID: 24949
		private float[] _layerWeights;

		// Token: 0x04006176 RID: 24950
		private AnimatorCullingMode _culling;

		// Token: 0x04006177 RID: 24951
		private Object _previewBinding;

		// Token: 0x04006178 RID: 24952
		private AudioListener _cinematicListener;

		// Token: 0x04006179 RID: 24953
		private PlayerEmotionController _emotion;

		// Token: 0x0400617A RID: 24954
		private Animator _faceAnimator;

		// Token: 0x0400617B RID: 24955
		private string _faceParameter;

		// Token: 0x0400617C RID: 24956
		private int _faceLayer = -1;

		// Token: 0x0400617D RID: 24957
		private int _savedFaceIndex;

		// Token: 0x0400617E RID: 24958
		private int _neutralFaceIndex;

		// Token: 0x0400617F RID: 24959
		private bool _ownsFaceOverride;

		// Token: 0x04006180 RID: 24960
		private bool _ownsEmotionOverride;

		// Token: 0x04006181 RID: 24961
		private LongWayHomeRestaurantFaceOverride _drunkFaceOverride;

		// Token: 0x04006182 RID: 24962
		private bool _capturedRenderSettings;

		// Token: 0x04006183 RID: 24963
		private Material _previousSky;

		// Token: 0x04006184 RID: 24964
		private AmbientMode _previousAmbientMode;

		// Token: 0x04006185 RID: 24965
		private Color _previousAmbient;

		// Token: 0x04006186 RID: 24966
		private bool _previousFog;

		// Token: 0x04006187 RID: 24967
		private static LongWayHomeRestaurantSequence _activePresenter;

		// Token: 0x0400618B RID: 24971
		[Header("Restaurant story")]
		[SerializeField]
		private LongWayHomeRestaurantDialogue dialogue;

		// Token: 0x0400618C RID: 24972
		[SerializeField]
		private RestaurantAmbienceLoop ambience;

		// Token: 0x0400618D RID: 24973
		[SerializeField]
		private Camera departureCamera;

		// Token: 0x0400618E RID: 24974
		[SerializeField]
		private Transform exitMark;

		// Token: 0x0400618F RID: 24975
		[SerializeField]
		private Transform departureApproachMark;

		// Token: 0x04006190 RID: 24976
		[SerializeField]
		private Transform leftPauseMark;

		// Token: 0x04006191 RID: 24977
		[SerializeField]
		[Min(0f)]
		private float leftPauseSeconds = 6f;

		// Token: 0x04006192 RID: 24978
		[SerializeField]
		private AnimationClip drunkIdle;

		// Token: 0x04006193 RID: 24979
		[SerializeField]
		private AnimationClip drunkWalk;

		// Token: 0x04006194 RID: 24980
		[SerializeField]
		private AnimationClip departureFace;

		// Token: 0x04006195 RID: 24981
		[Tooltip("Time to blend into blush 2 and closed eyes when the drinking warning starts.")]
		[SerializeField]
		[Min(0f)]
		private float drunkFaceBlendSeconds = 0.8f;

		// Token: 0x04006196 RID: 24982
		[Tooltip("Identifies the bra slot to unequip after removing the table outfit.")]
		[FormerlySerializedAs("universityBra")]
		[SerializeField]
		private WearableItemSO departureBra;

		// Token: 0x04006197 RID: 24983
		[FormerlySerializedAs("universityPanties")]
		[SerializeField]
		private WearableItemSO departurePanties;

		// Token: 0x04006198 RID: 24984
		[Tooltip("Travel speed at normal clip speed, matched to the short steps of the in-place drunk walk.")]
		[SerializeField]
		[Min(0.1f)]
		private float walkMetersPerSecond = 0.78f;

		// Token: 0x04006199 RID: 24985
		[SerializeField]
		[Range(0.5f, 1.5f)]
		private float walkPlaybackSpeed = 0.65f;

		// Token: 0x0400619A RID: 24986
		[SerializeField]
		[Min(30f)]
		private float departureTurnDegreesPerSecond = 120f;

		// Token: 0x0400619B RID: 24987
		[SerializeField]
		[Min(0.1f)]
		private float transitionSeconds = 0.7f;

		// Token: 0x0400619C RID: 24988
		[SerializeField]
		private string runSceneName = "Game - Current";

		// Token: 0x0400619D RID: 24989
		private PlayableGraph _departureGraph;

		// Token: 0x0400619E RID: 24990
		private AnimationMixerPlayable _locomotionMixer;

		// Token: 0x0400619F RID: 24991
		private AnimationClipPlayable _walkPlayable;

		// Token: 0x040061A0 RID: 24992
		private float _walkWeight;

		// Token: 0x040061A1 RID: 24993
		private bool _skipRequested;

		// Token: 0x040061A2 RID: 24994
		[Header("Departure footsteps")]
		[Tooltip("Reuse Ciel's existing footstep recordings; this source belongs to the restaurant scene.")]
		[SerializeField]
		private AudioClip[] departureFootstepClips;

		// Token: 0x040061A3 RID: 24995
		[SerializeField]
		[Range(0f, 1f)]
		private float departureFootstepVolume = 0.32f;

		// Token: 0x040061A4 RID: 24996
		[Tooltip("Normalized contact phase in the looping drunk walk. Left foot lands at the loop boundary.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float leftFootContactPhase;

		// Token: 0x040061A5 RID: 24997
		[Tooltip("Right foot reaches the floor around 0.65 seconds in the 1.3-second source clip.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float rightFootContactPhase = 0.5f;

		// Token: 0x040061A6 RID: 24998
		private AudioSource _departureFootstepSource;

		// Token: 0x040061A7 RID: 24999
		private bool _departureFootstepsRunning;

		// Token: 0x040061A8 RID: 25000
		private double _previousFootstepWalkTime;

		// Token: 0x040061A9 RID: 25001
		private Vector3 _previousFootstepPosition;

		// Token: 0x040061AA RID: 25002
		private int _lastDepartureFootstepClip = -1;

		// Token: 0x040061AB RID: 25003
		private int _departureFootstepsPlayed;

		// Token: 0x040061AC RID: 25004
		[Header("Friends watching Ciel leave")]
		[SerializeField]
		[Range(30f, 90f)]
		private float departureHeadYawLimit = 75f;

		// Token: 0x040061AD RID: 25005
		[SerializeField]
		[Range(10f, 40f)]
		private float departureHeadPitchLimit = 25f;

		// Token: 0x040061AE RID: 25006
		[SerializeField]
		[Range(2f, 15f)]
		private float departureLookSmoothing = 7f;

		// Token: 0x040061AF RID: 25007
		private readonly List<LongWayHomeRestaurantSequence.DepartureGazeActor> _departureGazeActors = new List<LongWayHomeRestaurantSequence.DepartureGazeActor>(2);

		// Token: 0x040061B0 RID: 25008
		private Transform _departureGazeTarget;

		// Token: 0x040061B1 RID: 25009
		private readonly List<SeatedRandomReaction> _cinematicPhotoReactions = new List<SeatedRandomReaction>();

		// Token: 0x040061B2 RID: 25010
		private readonly List<LongWayHomeRestaurantSequence.RestaurantNpcAppearance> _restaurantNpcAppearances = new List<LongWayHomeRestaurantSequence.RestaurantNpcAppearance>(4);

		// Token: 0x040061B3 RID: 25011
		private MCStyleCustomizationService _wardrobeService;

		// Token: 0x040061B4 RID: 25012
		private MCStyleCustomizationService.WardrobeSnapshot _wardrobeSnapshot;

		// Token: 0x040061B5 RID: 25013
		private WearableUseController _wardrobeWearController;

		// Token: 0x040061B6 RID: 25014
		private bool _wardrobeWearWasEnabled;

		// Token: 0x040061B7 RID: 25015
		private IDisposable _wardrobePresentation;

		// Token: 0x02000CE6 RID: 3302
		private struct BehaviourState
		{
			// Token: 0x040061B8 RID: 25016
			public Behaviour Component;

			// Token: 0x040061B9 RID: 25017
			public bool Enabled;
		}

		// Token: 0x02000CE7 RID: 3303
		public enum StoryPhase
		{
			// Token: 0x040061BB RID: 25019
			Loading,
			// Token: 0x040061BC RID: 25020
			TableDialogue,
			// Token: 0x040061BD RID: 25021
			FadeToDeparture,
			// Token: 0x040061BE RID: 25022
			DrunkReveal,
			// Token: 0x040061BF RID: 25023
			Walking,
			// Token: 0x040061C0 RID: 25024
			AtExit,
			// Token: 0x040061C1 RID: 25025
			Handoff,
			// Token: 0x040061C2 RID: 25026
			Failed,
			// Token: 0x040061C3 RID: 25027
			DrunkPause
		}

		// Token: 0x02000CE8 RID: 3304
		private sealed class DepartureGazeBone
		{
			// Token: 0x06006021 RID: 24609 RVA: 0x002627CC File Offset: 0x002609CC
			public DepartureGazeBone(Transform bone)
			{
				this.Bone = bone;
				if (bone != null)
				{
					this.AnimatedLocal = bone.localRotation;
				}
			}

			// Token: 0x06006022 RID: 24610 RVA: 0x002627F0 File Offset: 0x002609F0
			public void ReadAnimatedPose()
			{
				if (this.Bone == null)
				{
					return;
				}
				if (this.HasOffset && Quaternion.Angle(this.Bone.localRotation, this.WrittenLocal) < 0.001f)
				{
					this.Bone.localRotation = this.AnimatedLocal;
				}
				this.AnimatedLocal = this.Bone.localRotation;
				this.HasOffset = false;
			}

			// Token: 0x06006023 RID: 24611 RVA: 0x0026285C File Offset: 0x00260A5C
			public void ApplyWorldOffset(Quaternion offset)
			{
				if (this.Bone == null)
				{
					return;
				}
				this.Bone.rotation = offset * this.Bone.rotation;
				this.WrittenLocal = this.Bone.localRotation;
				this.HasOffset = true;
			}

			// Token: 0x06006024 RID: 24612 RVA: 0x002628AC File Offset: 0x00260AAC
			public void Release()
			{
				if (this.Bone != null && this.HasOffset && Quaternion.Angle(this.Bone.localRotation, this.WrittenLocal) < 0.001f)
				{
					this.Bone.localRotation = this.AnimatedLocal;
				}
				this.HasOffset = false;
			}

			// Token: 0x040061C4 RID: 25028
			public Transform Bone;

			// Token: 0x040061C5 RID: 25029
			public Quaternion AnimatedLocal;

			// Token: 0x040061C6 RID: 25030
			public Quaternion WrittenLocal;

			// Token: 0x040061C7 RID: 25031
			public bool HasOffset;
		}

		// Token: 0x02000CE9 RID: 3305
		private sealed class DepartureGazeActor
		{
			// Token: 0x040061C8 RID: 25032
			public Animator Animator;

			// Token: 0x040061C9 RID: 25033
			public LongWayHomeRestaurantSequence.DepartureGazeBone Neck;

			// Token: 0x040061CA RID: 25034
			public LongWayHomeRestaurantSequence.DepartureGazeBone Head;

			// Token: 0x040061CB RID: 25035
			public LongWayHomeRestaurantSequence.DepartureGazeBone LeftEye;

			// Token: 0x040061CC RID: 25036
			public LongWayHomeRestaurantSequence.DepartureGazeBone RightEye;

			// Token: 0x040061CD RID: 25037
			public Vector3 HeadForwardLocal;

			// Token: 0x040061CE RID: 25038
			public Vector3 HeadUpLocal;

			// Token: 0x040061CF RID: 25039
			public Vector2 HeadAngles;

			// Token: 0x040061D0 RID: 25040
			public Vector2 EyeAngles;
		}

		// Token: 0x02000CEA RID: 3306
		private sealed class RestaurantBonePose
		{
			// Token: 0x040061D1 RID: 25041
			public Transform Bone;

			// Token: 0x040061D2 RID: 25042
			public Vector3 Position;

			// Token: 0x040061D3 RID: 25043
			public Vector3 Scale;

			// Token: 0x040061D4 RID: 25044
			public Quaternion Rotation;
		}

		// Token: 0x02000CEB RID: 3307
		private sealed class RestaurantNpcAppearance
		{
			// Token: 0x040061D5 RID: 25045
			public Animator Actor;

			// Token: 0x040061D6 RID: 25046
			public Avatar Avatar;

			// Token: 0x040061D7 RID: 25047
			public SkinnedMeshRenderer Renderer;

			// Token: 0x040061D8 RID: 25048
			public Mesh Mesh;

			// Token: 0x040061D9 RID: 25049
			public Material[] Materials;

			// Token: 0x040061DA RID: 25050
			public Transform[] Bones;

			// Token: 0x040061DB RID: 25051
			public Transform RootBone;

			// Token: 0x040061DC RID: 25052
			public Bounds Bounds;

			// Token: 0x040061DD RID: 25053
			public float[] Shapes;

			// Token: 0x040061DE RID: 25054
			public readonly List<LongWayHomeRestaurantSequence.RestaurantBonePose> Poses = new List<LongWayHomeRestaurantSequence.RestaurantBonePose>();

			// Token: 0x040061DF RID: 25055
			public readonly List<GameObject> AddedBones = new List<GameObject>();

			// Token: 0x040061E0 RID: 25056
			public NPCDialogueHolder Dialogue;

			// Token: 0x040061E1 RID: 25057
			public CharacterGeneralInfo Info;

			// Token: 0x040061E2 RID: 25058
			public NPCPersonalityProfileSO PersonalityProfile;

			// Token: 0x040061E3 RID: 25059
			public string Identity;
		}
	}
}
