using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using BoZo.CharacterCreator.MC.StyleSystem;
using Features.Detection;
using Features.Inventory;
using Features.Inventory.Wearable;
using Features.Loading;
using Features.MasturbationSystem;
using Features.WardrobeFeedback;
using Invector.vCamera;
using Invector.vCharacterController;
using MagicaCloth2;
using ProjectExhibitionist;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.SceneManagement;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace Features.RunModes
{
	// Token: 0x02000CC3 RID: 3267
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(-1050)]
	public sealed class LongWayHomeGymShowerSequence : MonoBehaviour, ILongWayHomePoiProloguePresenter
	{
		// Token: 0x17000F50 RID: 3920
		// (get) Token: 0x06005EDB RID: 24283 RVA: 0x002586B7 File Offset: 0x002568B7
		// (set) Token: 0x06005EDC RID: 24284 RVA: 0x002586BF File Offset: 0x002568BF
		public LongWayHomeGymShowerSequence.StoryPhase Phase { get; private set; }

		// Token: 0x17000F51 RID: 3921
		// (get) Token: 0x06005EDD RID: 24285 RVA: 0x002586C8 File Offset: 0x002568C8
		// (set) Token: 0x06005EDE RID: 24286 RVA: 0x002586D0 File Offset: 0x002568D0
		public bool IsReady { get; private set; }

		// Token: 0x17000F52 RID: 3922
		// (get) Token: 0x06005EDF RID: 24287 RVA: 0x002586D9 File Offset: 0x002568D9
		// (set) Token: 0x06005EE0 RID: 24288 RVA: 0x002586E1 File Offset: 0x002568E1
		public bool IsPreparedForReveal { get; private set; }

		// Token: 0x17000F53 RID: 3923
		// (get) Token: 0x06005EE1 RID: 24289 RVA: 0x002586EA File Offset: 0x002568EA
		public bool OwnsDestinationReveal
		{
			get
			{
				return base.isActiveAndEnabled && this.Phase != LongWayHomeGymShowerSequence.StoryPhase.Failed && this.Phase != LongWayHomeGymShowerSequence.StoryPhase.Done;
			}
		}

		// Token: 0x17000F54 RID: 3924
		// (get) Token: 0x06005EE2 RID: 24290 RVA: 0x0025870C File Offset: 0x0025690C
		public Transform RuntimePlayer
		{
			get
			{
				return this._player;
			}
		}

		// Token: 0x17000F55 RID: 3925
		// (get) Token: 0x06005EE3 RID: 24291 RVA: 0x00258714 File Offset: 0x00256914
		public bool CanSkip
		{
			get
			{
				return this.IsReady && this._captured && !this._skipRequested && this.Phase != LongWayHomeGymShowerSequence.StoryPhase.Loading && this.Phase != LongWayHomeGymShowerSequence.StoryPhase.Handoff && this.Phase != LongWayHomeGymShowerSequence.StoryPhase.Done && this.Phase != LongWayHomeGymShowerSequence.StoryPhase.Failed;
			}
		}

		// Token: 0x06005EE4 RID: 24292 RVA: 0x00258764 File Offset: 0x00256964
		public bool TrySkip()
		{
			if (!this.CanSkip)
			{
				return false;
			}
			this._skipRequested = true;
			base.StopAllCoroutines();
			LongWayHomeRestaurantDialogue dialogue = this._dialogue;
			if (dialogue != null)
			{
				dialogue.Abort();
			}
			LongWayHomeCinematicSkip skipHint = this._skipHint;
			if (skipHint != null)
			{
				skipHint.Dispose();
			}
			base.StartCoroutine(this.SkipToGameplay());
			return true;
		}

		// Token: 0x06005EE5 RID: 24293 RVA: 0x002587B8 File Offset: 0x002569B8
		private IEnumerator SkipToGameplay()
		{
			this.Phase = LongWayHomeGymShowerSequence.StoryPhase.Handoff;
			yield return this.FadeScreen(true, this._config.handoffFadeSeconds);
			this._grounded = false;
			this.RemoveGarments(true);
			if (this._wet != null && !this._wetAcquired)
			{
				this._wet.AcquireWet("LongWayHomeGymShower");
				this._wetAcquired = true;
				if (this._bubbles != null)
				{
					this._bubbles.OnBubblesGone += this.ReleaseWet;
				}
			}
			if (this._bubbles != null)
			{
				this._bubbles.ResetToFull();
				this._bubbles.ShowBubbles();
			}
			this.StopShowerEffects();
			Vector3[] exitPath = this._config.exitPath;
			this.TeleportPlayer(exitPath[exitPath.Length - 1], LongWayHomeGymShowerSequence.FacingRotation(exitPath[exitPath.Length - 2], exitPath[exitPath.Length - 1]));
			this.CompleteHandoff();
			yield break;
		}

		// Token: 0x06005EE6 RID: 24294 RVA: 0x002587C7 File Offset: 0x002569C7
		public static LongWayHomeGymShowerSequence GetPresenterForScene(Scene scene)
		{
			if (!(LongWayHomeGymShowerSequence._activePresenter != null) || !LongWayHomeGymShowerSequence._activePresenter.isActiveAndEnabled || !(LongWayHomeGymShowerSequence._activePresenter.gameObject.scene == scene))
			{
				return null;
			}
			return LongWayHomeGymShowerSequence._activePresenter;
		}

		// Token: 0x06005EE7 RID: 24295 RVA: 0x00258800 File Offset: 0x00256A00
		public static LongWayHomeGymShowerSequence Install(Scene scene)
		{
			LongWayHomeGymShowerSequence presenterForScene = LongWayHomeGymShowerSequence.GetPresenterForScene(scene);
			if (presenterForScene != null)
			{
				return presenterForScene;
			}
			GameObject gameObject = new GameObject("Long Way Home - Gym Shower Opening");
			SceneManager.MoveGameObjectToScene(gameObject, scene);
			return gameObject.AddComponent<LongWayHomeGymShowerSequence>();
		}

		// Token: 0x06005EE8 RID: 24296 RVA: 0x00258835 File Offset: 0x00256A35
		public void FailPreparation(string reason)
		{
			if (this.IsPreparedForReveal || !this.OwnsDestinationReveal)
			{
				return;
			}
			base.StopAllCoroutines();
			this.Fail(reason);
		}

		// Token: 0x06005EE9 RID: 24297 RVA: 0x00258855 File Offset: 0x00256A55
		public void PrepareForSceneExit()
		{
			if (this.Phase == LongWayHomeGymShowerSequence.StoryPhase.Done)
			{
				return;
			}
			base.StopAllCoroutines();
			this.ReleasePlayer();
			if (this.Phase != LongWayHomeGymShowerSequence.StoryPhase.Failed)
			{
				this.Phase = LongWayHomeGymShowerSequence.StoryPhase.Handoff;
			}
			base.enabled = false;
		}

		// Token: 0x06005EEA RID: 24298 RVA: 0x00258885 File Offset: 0x00256A85
		private void Awake()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			LongWayHomeGymShowerSequence._activePresenter = this;
			ScreenFadeController instance = ScreenFadeController.Instance;
			if (instance != null)
			{
				instance.SetBlack();
			}
			this._dialogue = base.gameObject.AddComponent<LongWayHomeRestaurantDialogue>();
		}

		// Token: 0x06005EEB RID: 24299 RVA: 0x002588B6 File Offset: 0x00256AB6
		private IEnumerator Start()
		{
			this._config = LongWayHomeGymShowerConfigSO.Load();
			if (this._config == null)
			{
				this.Fail("Missing Resources/LongWayHomeGymShowerConfig.");
				yield break;
			}
			string text;
			if (!this._config.HasRequiredAssets(out text))
			{
				this.Fail("The gym opening config is missing '" + text + "'.");
				yield break;
			}
			string text2;
			if (!this.ResolveStall(out text2))
			{
				this.Fail(text2);
				yield break;
			}
			this.OpenStall();
			LongWayHomeOpeningReadinessWait playerReadiness = new LongWayHomeOpeningReadinessWait(60f);
			while (!this.PlayerReady())
			{
				LongWayHomeOpeningReadinessWait.Result result = playerReadiness.ObservePlayer(base.gameObject.scene);
				if (result == LongWayHomeOpeningReadinessWait.Result.UpstreamFailed)
				{
					yield break;
				}
				if (result == LongWayHomeOpeningReadinessWait.Result.TimedOut)
				{
					this.Fail("The persistent player did not finish initialization in the Mall.");
					yield break;
				}
				yield return null;
			}
			yield return null;
			if (LongWayHomeOpeningReadinessWait.HasUpstreamFailure)
			{
				yield break;
			}
			LoadingReadinessWatchdog activityReadiness = new LoadingReadinessWatchdog(60f);
			while (LongWayHomeOpeningReadinessWait.HasUpstreamFailure || !PlayerActivityGate.TryAcquire("LongWayHomeGymShower#" + base.GetInstanceID().ToString(), this, PlayerActivity.None, PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation | PlayerContextResource.HeldItem, PlayerActivity.AllPlayerActions, out this._lease, false, null))
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
			this._style = this._player.GetComponentInChildren<MCStyleCustomizationService>(true);
			this._clothing = this._player.GetComponentInChildren<ClothingToggleController>(true);
			this._bubbles = this._player.GetComponentInChildren<ShowerBubbleSpawner>(true);
			this._wet = this._player.GetComponentInChildren<CielWetController>(true);
			if (this._animator == null || !this._animator.isHuman || this._input == null || this._body == null || this._style == null)
			{
				this.Fail("The registered player is missing its production rig components.");
				yield break;
			}
			this._gameplayCamera = this._input.tpCamera;
			this._gameplayView = ((this._gameplayCamera != null) ? this._gameplayCamera.GetComponentInChildren<Camera>(true) : Camera.main);
			this.CapturePlayerControl();
			this.CaptureFace();
			this.SuppressCorePresentation();
			WearAdmireAnimation.SetPresentationSuppressed(this, true);
			if (!this.CaptureWardrobe())
			{
				this.Fail("The player's wardrobe could not be isolated for the cinematic.");
				yield break;
			}
			this.RemoveGarments(false);
			this._style.SetNaked(false);
			yield return null;
			WearableItemSO gymOutfit = this._config.gymOutfit;
			if (!this._style.EquipRuntimePreset(gymOutfit.mcStylePartId, gymOutfit.mcStylePreset) || this._style.GetSpawnedRoot(gymOutfit.mcStylePartId) == null)
			{
				this.Fail("The complete gym outfit could not be attached to the player.");
				yield break;
			}
			this._style.SetSlotPhysicallyHidden(gymOutfit.mcStylePartId, false);
			ClothingToggleController clothing = this._clothing;
			if (clothing != null)
			{
				clothing.SetSlotVisible(gymOutfit.mcStylePartId, true);
			}
			yield return null;
			string text3;
			this._photographer = LongWayHomeGymShowerPhotographer.Create(base.gameObject.scene, this._config, out text3);
			if (this._photographer == null)
			{
				this.Fail(text3);
				yield break;
			}
			this._photographer.PhotoTaken += this.OnPhotoTaken;
			if (!this.CreateClothesPile())
			{
				this.Fail("The gym clothes could not be prepared.");
				yield break;
			}
			this.CreateCameras();
			Vector3[] walkInPath = this._config.walkInPath;
			this.TeleportPlayer(walkInPath[0], LongWayHomeGymShowerSequence.FacingRotation(walkInPath[0], walkInPath[1]));
			this.BuildCielGraph(this._config.walkInIdle, this._config.walkInWalk, this._config.walkInPlaybackSpeed);
			this._walkWeight = 1f;
			this.SetWalkWeight();
			this.BeginFace(this._config.walkInFaceAnimation);
			this.ActivateCamera(this._walkInCamera, this._player, Vector3.zero);
			yield return this.WarmPresentation();
			if (this.Phase == LongWayHomeGymShowerSequence.StoryPhase.Failed)
			{
				yield break;
			}
			this._ankleHeight = Mathf.Clamp(this.MinFootHeight() - this._player.position.y, 0.02f, 0.2f);
			this.IsPreparedForReveal = true;
			while ((LoadingScreenController.ExistingInstance != null && LoadingScreenController.ExistingInstance.IsLoadingActive) || (SceneTransitionManager.Instance != null && SceneTransitionManager.Instance.IsTransitioning))
			{
				yield return null;
			}
			this._input.SetLockAllInput(true);
			this._input.SetLockCameraInput(true);
			this.IsReady = true;
			yield return this.PlayStory();
			yield break;
		}

		// Token: 0x06005EEC RID: 24300 RVA: 0x002588C8 File Offset: 0x00256AC8
		private bool PlayerReady()
		{
			if (SceneManager.GetActiveScene() != base.gameObject.scene || !SceneBootLoader.CoreReady || !GameSystemsBootstrapper.GameplayReady || GameSystemsBootstrapper.Instance == null || GameSystemsBootstrapper.Instance.InitializedSceneName != base.gameObject.scene.name || PlayerRegistry.Player == null)
			{
				return false;
			}
			ClothInventorySyncer componentInChildren = PlayerRegistry.Player.GetComponentInChildren<ClothInventorySyncer>(true);
			return componentInChildren == null || componentInChildren.SyncCompleted;
		}

		// Token: 0x06005EED RID: 24301 RVA: 0x00258957 File Offset: 0x00256B57
		private IEnumerator PlayStory()
		{
			this.Phase = LongWayHomeGymShowerSequence.StoryPhase.WalkIn;
			base.StartCoroutine(this.FadeScreen(false, 0.6f));
			yield return this.WalkAlong(this._config.walkInPath, this._config.walkInMetersPerSecond * this._config.walkInPlaybackSpeed, true);
			yield return new WaitForSeconds(0.25f);
			yield return this.FadeScreen(true, this._config.transitionSeconds);
			this.Phase = LongWayHomeGymShowerSequence.StoryPhase.Shower;
			this.RemoveGarments(true);
			this._clothesPile.SetActive(true);
			this.TeleportPlayer(this._standPosition, this._standRotation);
			this.BuildCielGraph(this._config.shower, this._config.exitIdle, 1f);
			this._face.Show(this._config.showerFaceAnimation);
			this._grounded = true;
			this.StartShowerEffects();
			this.ActivateCamera(this._showerCamera, null, Vector3.zero);
			yield return null;
			yield return null;
			yield return this.FadeScreen(false, this._config.transitionSeconds);
			float showerStarted = Time.time;
			yield return new WaitForSeconds(this._config.bubblesAfterSeconds);
			if (this._bubbles != null)
			{
				this._bubbles.ResetToFull();
				this._bubbles.ShowBubbles();
			}
			while (Time.time < showerStarted + this._config.photographerAfterSeconds)
			{
				yield return null;
			}
			this.Phase = LongWayHomeGymShowerSequence.StoryPhase.Photos;
			Vector3[] npcEntryPath = this._config.npcEntryPath;
			this._photographer.Show(npcEntryPath[0], LongWayHomeGymShowerSequence.FacingRotation(npcEntryPath[0], npcEntryPath[1]));
			yield return this.MovePhotographer(npcEntryPath, this._config.npcWalkMetersPerSecond, LongWayHomeGymShowerPhotographer.Pose.Walk);
			yield return this.TurnPhotographer(this._standPosition);
			this._photographer.SetPose(LongWayHomeGymShowerPhotographer.Pose.Photo, 0.3f);
			float photoDeadline = Time.time + 8f;
			while (!this._firstPhotoTaken && Time.time < photoDeadline)
			{
				yield return null;
			}
			if (!this._firstPhotoTaken)
			{
				this.OnPhotoTaken();
			}
			yield return new WaitForSeconds(this._config.dialogueAfterFirstPhotoSeconds);
			this.Phase = LongWayHomeGymShowerSequence.StoryPhase.Dialogue;
			yield return this._dialogue.PlayGymShowerConfrontation(this._photographer.DisplayName);
			if (!this._dialogue.LastDialogueCompleted)
			{
				this.Fail("The gym dialogue was interrupted.");
				yield break;
			}
			this.Phase = LongWayHomeGymShowerSequence.StoryPhase.Theft;
			this.ActivateCamera(this._theftCamera, this._photographer.Transform, Vector3.zero);
			base.StartCoroutine(this.CielNoticesTheft());
			this._photographer.SetPoseSpeed(LongWayHomeGymShowerPhotographer.Pose.Walk, this._config.npcToBenchMetersPerSecond / Mathf.Max(0.1f, this._config.npcWalkMetersPerSecond));
			yield return this.MovePhotographer(new Vector3[]
			{
				this._photographer.Transform.position,
				this._config.npcPickUpMark
			}, this._config.npcToBenchMetersPerSecond, LongWayHomeGymShowerPhotographer.Pose.Walk);
			this._photographer.SetPoseSpeed(LongWayHomeGymShowerPhotographer.Pose.Walk, 1f);
			yield return this.TurnPhotographer(this._clothesPile.transform.position);
			yield return new WaitForSeconds(this._config.npcGrabSeconds);
			this._clothesPile.SetActive(false);
			yield return new WaitForSeconds(0.15f);
			List<Vector3> list = new List<Vector3> { this._photographer.Transform.position };
			list.AddRange(this._config.npcEscapePath);
			yield return this.MovePhotographer(list.ToArray(), this._config.npcRunMetersPerSecond, LongWayHomeGymShowerPhotographer.Pose.Run);
			this._photographer.Hide();
			yield return new WaitForSeconds(0.5f);
			yield return this.FadeScreen(true, this._config.transitionSeconds);
			this.Phase = LongWayHomeGymShowerSequence.StoryPhase.Exit;
			this._grounded = false;
			this.StopShowerEffects();
			Vector3[] exit = this._config.exitPath;
			this.TeleportPlayer(exit[0], LongWayHomeGymShowerSequence.FacingRotation(exit[0], exit[1]));
			this.BuildCielGraph(this._config.exitIdle, this._config.exitWalk, this._config.exitPlaybackSpeed);
			this._walkWeight = 1f;
			this.SetWalkWeight();
			this._face.Show(this._config.embarrassedFaceAnimation);
			this.ActivateCamera(this._exitCamera, this._player, Vector3.zero);
			yield return null;
			base.StartCoroutine(this.FadeScreen(false, this._config.transitionSeconds));
			yield return this.WalkAlong(exit, this._config.exitMetersPerSecond * this._config.exitPlaybackSpeed, true);
			this.Phase = LongWayHomeGymShowerSequence.StoryPhase.Handoff;
			LongWayHomeCinematicSkip skipHint = this._skipHint;
			if (skipHint != null)
			{
				skipHint.Dispose();
			}
			yield return this.FadeScreen(true, this._config.handoffFadeSeconds);
			this.CompleteHandoff();
			yield break;
		}

		// Token: 0x06005EEE RID: 24302 RVA: 0x00258968 File Offset: 0x00256B68
		private void CompleteHandoff()
		{
			this.ReleasePlayer();
			if (this._bubbles != null && this._bubbles.BubblesActive)
			{
				this._bubbles.BeginDissolve();
			}
			if (this._gameplayCamera != null && this._gameplayCamera.isInit)
			{
				this._gameplayCamera.ResetAngleWithoutSmooth();
			}
			LongWayHomeRuntimeController instance = LongWayHomeRuntimeController.Instance;
			if (instance == null || !instance.BeginRunFromGymShowerPrologue())
			{
				Debug.LogWarning("[LongWayHomeGymShower] No prepared Long Way Home run took over the Mall.", this);
			}
			this.Phase = LongWayHomeGymShowerSequence.StoryPhase.Done;
			ScreenFadeController instance2 = ScreenFadeController.Instance;
			if (instance2 == null)
			{
				return;
			}
			instance2.FadeIn(this._config.handoffFadeSeconds, null);
		}

		// Token: 0x06005EEF RID: 24303 RVA: 0x00258A0B File Offset: 0x00256C0B
		private IEnumerator CielNoticesTheft()
		{
			while (this._photographer != null && this._photographer.Transform != null && LongWayHomeGymShowerSequence.PlanarDistance(this._photographer.Transform.position, this._config.npcPickUpMark) > this._config.cielReactsAtNpcDistance)
			{
				yield return null;
			}
			Vector3 vector = this._clothesPile.transform.position - this._player.position;
			vector.y = 0f;
			if (vector.sqrMagnitude < 0.0001f)
			{
				yield break;
			}
			float targetYaw = Quaternion.LookRotation(vector).eulerAngles.y;
			float yaw = this._player.eulerAngles.y;
			float yawVelocity = 0f;
			float blend = 0f;
			while (blend < 1f || Mathf.Abs(Mathf.DeltaAngle(yaw, targetYaw)) > 0.5f)
			{
				blend = Mathf.MoveTowards(blend, 1f, Time.deltaTime / 0.5f);
				this._walkWeight = Mathf.SmoothStep(0f, 1f, blend);
				this.SetWalkWeight();
				yaw = Mathf.SmoothDampAngle(yaw, targetYaw, ref yawVelocity, 0.35f);
				Quaternion quaternion = Quaternion.Euler(0f, yaw, 0f);
				this._player.rotation = quaternion;
				this._body.rotation = quaternion;
				yield return null;
			}
			yield break;
		}

		// Token: 0x06005EF0 RID: 24304 RVA: 0x00258A1C File Offset: 0x00256C1C
		private static float PlanarDistance(Vector3 a, Vector3 b)
		{
			a.y = (b.y = 0f);
			return Vector3.Distance(a, b);
		}

		// Token: 0x06005EF1 RID: 24305 RVA: 0x00258A46 File Offset: 0x00256C46
		private void OnPhotoTaken()
		{
			if (this._firstPhotoTaken)
			{
				return;
			}
			this._firstPhotoTaken = true;
			LongWayHomeGymShowerFaceDriver face = this._face;
			if (face == null)
			{
				return;
			}
			face.BlendTo(this._config.embarrassedClosedMouthFaceAnimation, this._config.faceBlendSeconds);
		}

		// Token: 0x06005EF2 RID: 24306 RVA: 0x00258A80 File Offset: 0x00256C80
		private void BeginFace(LongWayHomeFaceAnimationSO face)
		{
			Transform transform = this._player.Find("3D Model/Ciel_07_05_26_ass/Body");
			SkinnedMeshRenderer skinnedMeshRenderer = ((transform != null) ? transform.GetComponent<SkinnedMeshRenderer>() : null);
			if (skinnedMeshRenderer == null)
			{
				foreach (SkinnedMeshRenderer skinnedMeshRenderer2 in this._player.GetComponentsInChildren<SkinnedMeshRenderer>(true))
				{
					if (skinnedMeshRenderer2.name == "Body")
					{
						skinnedMeshRenderer = skinnedMeshRenderer2;
						break;
					}
				}
			}
			if (this._face == null)
			{
				this._face = base.gameObject.AddComponent<LongWayHomeGymShowerFaceDriver>();
			}
			this._face.Begin((skinnedMeshRenderer != null && skinnedMeshRenderer.transform.parent != null) ? skinnedMeshRenderer.transform.parent.gameObject : null, skinnedMeshRenderer);
			this._face.Show(face);
		}

		// Token: 0x06005EF3 RID: 24307 RVA: 0x00258B58 File Offset: 0x00256D58
		private float MinFootHeight()
		{
			Transform boneTransform = this._animator.GetBoneTransform(HumanBodyBones.LeftFoot);
			Transform boneTransform2 = this._animator.GetBoneTransform(HumanBodyBones.RightFoot);
			if (boneTransform == null || boneTransform2 == null)
			{
				return this._player.position.y + this._ankleHeight;
			}
			return Mathf.Min(boneTransform.position.y, boneTransform2.position.y);
		}

		// Token: 0x06005EF4 RID: 24308 RVA: 0x00258BC4 File Offset: 0x00256DC4
		private bool ResolveStall(out string error)
		{
			error = null;
			ShowerInteraction showerInteraction = null;
			ShowerInteraction showerInteraction2 = null;
			float num = float.MaxValue;
			GameObject[] rootGameObjects = base.gameObject.scene.GetRootGameObjects();
			for (int i = 0; i < rootGameObjects.Length; i++)
			{
				foreach (ShowerInteraction showerInteraction3 in rootGameObjects[i].GetComponentsInChildren<ShowerInteraction>(true))
				{
					Transform transform = LongWayHomeGymShowerSequence.StationRoot(showerInteraction3);
					if (showerInteraction2 == null && transform.parent != null && transform.parent.name == this._config.stallName)
					{
						showerInteraction2 = showerInteraction3;
					}
					float sqrMagnitude = (transform.position - this._config.stallFallbackPosition).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						num = sqrMagnitude;
						showerInteraction = showerInteraction3;
					}
				}
			}
			if (showerInteraction2 != null)
			{
				showerInteraction = showerInteraction2;
			}
			Transform transform2 = ((showerInteraction != null) ? LongWayHomeGymShowerSequence.StationRoot(showerInteraction) : null);
			if (transform2 == null || transform2.parent == null || (transform2.position - this._config.stallFallbackPosition).sqrMagnitude > 25f)
			{
				error = "The Mall gym shower stall was not found.";
				return false;
			}
			this._station = showerInteraction;
			this._stall = transform2.parent;
			Transform transform3 = LongWayHomeGymShowerSequence.FindDeep(transform2, "ShowerStandingPoint");
			this._standPosition = ((transform3 != null) ? transform3.position : transform2.position);
			this._standRotation = ((transform3 != null) ? transform3.rotation : (transform2.rotation * Quaternion.Euler(0f, 180f, 0f)));
			this._standPosition.y = transform2.position.y;
			ParticleSystem[] array = LongWayHomeGymShowerSequence.Field(showerInteraction, "waterParticles") as ParticleSystem[];
			if (array != null)
			{
				foreach (ParticleSystem particleSystem in array)
				{
					if (particleSystem != null)
					{
						this._water.Add(particleSystem);
					}
				}
			}
			foreach (ParticleSystem particleSystem2 in transform2.GetComponentsInChildren<ParticleSystem>(true))
			{
				if (!this._water.Contains(particleSystem2) && (particleSystem2.name == "ShowerWater" || particleSystem2.name == "ShowerSteam"))
				{
					this._water.Add(particleSystem2);
				}
			}
			this._showerAudio = LongWayHomeGymShowerSequence.Field(showerInteraction, "audioSource") as AudioSource;
			this._showerLoop = LongWayHomeGymShowerSequence.Field(showerInteraction, "showerLoopClip") as AudioClip;
			object obj = LongWayHomeGymShowerSequence.Field(showerInteraction, "showerVolume");
			if (obj is float)
			{
				float num2 = (float)obj;
				this._showerVolume = num2;
			}
			Debug.Log(string.Concat(new string[]
			{
				"[LongWayHomeGymShower] Stall '",
				this._stall.name,
				"', stand ",
				(transform3 != null) ? "authored" : "estimated",
				" ",
				string.Format("at {0}, {1} water/steam systems.", this._standPosition, this._water.Count)
			}), this._stall);
			return true;
		}

		// Token: 0x06005EF5 RID: 24309 RVA: 0x00258F18 File Offset: 0x00257118
		private static Transform StationRoot(ShowerInteraction station)
		{
			Transform transform = station.transform;
			while (transform != null)
			{
				if (transform.name == "MallGymShowerStation")
				{
					return transform;
				}
				transform = transform.parent;
			}
			return station.transform;
		}

		// Token: 0x06005EF6 RID: 24310 RVA: 0x00258F58 File Offset: 0x00257158
		private static Transform FindDeep(Transform parent, string childName)
		{
			if (parent.name == childName)
			{
				return parent;
			}
			for (int i = 0; i < parent.childCount; i++)
			{
				Transform transform = LongWayHomeGymShowerSequence.FindDeep(parent.GetChild(i), childName);
				if (transform != null)
				{
					return transform;
				}
			}
			return null;
		}

		// Token: 0x06005EF7 RID: 24311 RVA: 0x00258FA0 File Offset: 0x002571A0
		private static object Field(object target, string name)
		{
			if (target == null)
			{
				return null;
			}
			FieldInfo field = target.GetType().GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (field == null)
			{
				return null;
			}
			return field.GetValue(target);
		}

		// Token: 0x06005EF8 RID: 24312 RVA: 0x00258FC4 File Offset: 0x002571C4
		private void OpenStall()
		{
			if (this._stall == null)
			{
				return;
			}
			this._openLeaves.Clear();
			FieldInfo field = typeof(DoorInteraction).GetField("_isOpen", BindingFlags.Instance | BindingFlags.NonPublic);
			foreach (DoorInteraction doorInteraction in this._stall.GetComponentsInChildren<DoorInteraction>(true))
			{
				if (field != null)
				{
					field.SetValue(doorInteraction, true);
				}
				this.AddOpenLeaf(LongWayHomeGymShowerSequence.Field(doorInteraction, "rotatePivot") as Transform, LongWayHomeGymShowerSequence.Field(doorInteraction, "openAngle"));
				this.AddOpenLeaf(LongWayHomeGymShowerSequence.Field(doorInteraction, "secondaryRotatePivot") as Transform, LongWayHomeGymShowerSequence.Field(doorInteraction, "secondaryOpenAngle"));
			}
			this.KeepStallOpen();
		}

		// Token: 0x06005EF9 RID: 24313 RVA: 0x0025907C File Offset: 0x0025727C
		private void AddOpenLeaf(Transform leaf, object angle)
		{
			if (leaf == null || !(angle is float))
			{
				return;
			}
			float num = (float)angle;
			Quaternion quaternion = Quaternion.Euler(0f, num, 0f);
			this._openLeaves.Add(new KeyValuePair<Transform, Quaternion>(leaf, quaternion));
			MeshRenderer component = leaf.GetComponent<MeshRenderer>();
			if (component == null || !component.isPartOfStaticBatch)
			{
				return;
			}
			Mesh mesh = ((leaf.GetComponent<MeshCollider>() != null) ? leaf.GetComponent<MeshCollider>().sharedMesh : null);
			MeshFilter component2 = leaf.GetComponent<MeshFilter>();
			if (mesh == null && component2 != null && component2.sharedMesh != null && !component2.sharedMesh.name.StartsWith("Combined Mesh", StringComparison.Ordinal))
			{
				mesh = component2.sharedMesh;
			}
			if (mesh == null)
			{
				return;
			}
			GameObject gameObject = new GameObject(leaf.name + " (open, cinematic)");
			gameObject.transform.SetParent(leaf.parent, false);
			gameObject.transform.localPosition = leaf.localPosition;
			gameObject.transform.localRotation = quaternion;
			gameObject.transform.localScale = leaf.localScale;
			gameObject.AddComponent<MeshFilter>().sharedMesh = mesh;
			MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterials = component.sharedMaterials;
			meshRenderer.shadowCastingMode = component.shadowCastingMode;
			meshRenderer.receiveShadows = component.receiveShadows;
			component.enabled = false;
			this._hiddenDoorLeaves.Add(component);
			this._doorCopies.Add(gameObject);
		}

		// Token: 0x06005EFA RID: 24314 RVA: 0x00259204 File Offset: 0x00257404
		private void RestoreStall()
		{
			foreach (GameObject gameObject in this._doorCopies)
			{
				if (gameObject != null)
				{
					Object.Destroy(gameObject);
				}
			}
			this._doorCopies.Clear();
			foreach (MeshRenderer meshRenderer in this._hiddenDoorLeaves)
			{
				if (meshRenderer != null)
				{
					meshRenderer.enabled = true;
				}
			}
			this._hiddenDoorLeaves.Clear();
			foreach (KeyValuePair<Transform, Quaternion> keyValuePair in this._openLeaves)
			{
				if (keyValuePair.Key != null)
				{
					keyValuePair.Key.localRotation = Quaternion.identity;
				}
			}
			this._openLeaves.Clear();
			if (this._stall == null)
			{
				return;
			}
			FieldInfo field = typeof(DoorInteraction).GetField("_isOpen", BindingFlags.Instance | BindingFlags.NonPublic);
			foreach (DoorInteraction doorInteraction in this._stall.GetComponentsInChildren<DoorInteraction>(true))
			{
				if (field != null)
				{
					field.SetValue(doorInteraction, false);
				}
			}
		}

		// Token: 0x06005EFB RID: 24315 RVA: 0x0025938C File Offset: 0x0025758C
		private void KeepStallOpen()
		{
			foreach (KeyValuePair<Transform, Quaternion> keyValuePair in this._openLeaves)
			{
				if (keyValuePair.Key != null)
				{
					keyValuePair.Key.localRotation = keyValuePair.Value;
				}
			}
		}

		// Token: 0x06005EFC RID: 24316 RVA: 0x002593FC File Offset: 0x002575FC
		private void StartShowerEffects()
		{
			if (this._station != null)
			{
				this._station.enabled = false;
			}
			foreach (ParticleSystem particleSystem in this._water)
			{
				if (particleSystem != null)
				{
					particleSystem.Play(true);
				}
			}
			if (this._showerAudio != null && (this._showerLoop != null || this._showerAudio.clip != null))
			{
				if (this._showerLoop != null)
				{
					this._showerAudio.clip = this._showerLoop;
				}
				this._showerAudio.loop = true;
				this._showerAudio.volume = this._showerVolume;
				this._showerAudio.Play();
			}
			if (this._wet != null && !this._wetAcquired)
			{
				this._wet.AcquireWet("LongWayHomeGymShower");
				this._wetAcquired = true;
				if (this._bubbles != null)
				{
					this._bubbles.OnBubblesGone += this.ReleaseWet;
				}
			}
		}

		// Token: 0x06005EFD RID: 24317 RVA: 0x0025953C File Offset: 0x0025773C
		private void StopShowerEffects()
		{
			foreach (ParticleSystem particleSystem in this._water)
			{
				if (particleSystem != null)
				{
					particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
				}
			}
			if (this._showerAudio != null)
			{
				this._showerAudio.Stop();
			}
			if (this._station != null)
			{
				this._station.enabled = true;
			}
		}

		// Token: 0x06005EFE RID: 24318 RVA: 0x002595CC File Offset: 0x002577CC
		private void ReleaseWet()
		{
			if (this._bubbles != null)
			{
				this._bubbles.OnBubblesGone -= this.ReleaseWet;
			}
			if (this._wetAcquired && this._wet != null)
			{
				this._wet.ReleaseWet("LongWayHomeGymShower");
			}
			this._wetAcquired = false;
		}

		// Token: 0x06005EFF RID: 24319 RVA: 0x0025962C File Offset: 0x0025782C
		private bool CreateClothesPile()
		{
			GameObject gameObject = new GameObject("Gym Clothes (staging)");
			gameObject.SetActive(false);
			SceneManager.MoveGameObjectToScene(gameObject, base.gameObject.scene);
			this._clothesPile = new GameObject("Gym Clothes - On Bench");
			this._clothesPile.transform.SetParent(gameObject.transform, false);
			for (int i = 0; i < this._config.clothesPieces.Length; i++)
			{
				GameObject gameObject2 = this._config.clothesPieces[i];
				if (!(gameObject2 == null))
				{
					GameObject gameObject3 = Object.Instantiate<GameObject>(gameObject2, this._clothesPile.transform);
					gameObject3.name = gameObject2.name;
					LongWayHomeGymShowerPhotographer.StripToVisual(gameObject3);
					Vector3[] clothesPieceOffsets = this._config.clothesPieceOffsets;
					gameObject3.transform.localPosition = ((clothesPieceOffsets != null && i < clothesPieceOffsets.Length) ? clothesPieceOffsets[i] : (Vector3.up * 0.03f * (float)i));
					gameObject3.transform.localRotation = Quaternion.Euler(0f, (float)i * 23f, 0f);
				}
			}
			this._clothesPile.SetActive(false);
			this._clothesPile.transform.SetParent(base.transform, false);
			Object.Destroy(gameObject);
			this._clothesPile.transform.SetPositionAndRotation(this._config.clothesPilePosition, Quaternion.Euler(0f, this._config.clothesPileYaw, 0f));
			return this._clothesPile.GetComponentInChildren<Renderer>(true) != null;
		}

		// Token: 0x06005F00 RID: 24320 RVA: 0x002597B0 File Offset: 0x002579B0
		private void CreateCameras()
		{
			this._walkInCamera = this.CreateCamera("Camera - Gym Walk In", this._config.walkInCameraPosition, this._config.followFieldOfView);
			this._showerCamera = this.CreateCamera("Camera - Shower Face", this._standPosition, this._config.showerFieldOfView);
			this._theftCamera = this.CreateCamera("Camera - Stolen Clothes", this._config.theftCameraPosition, this._config.trackingFieldOfView);
			this._exitCamera = this.CreateCamera("Camera - Locker Room Exit", this._config.exitCameraPosition, this._config.followFieldOfView);
		}

		// Token: 0x06005F01 RID: 24321 RVA: 0x00259854 File Offset: 0x00257A54
		private Camera CreateCamera(string cameraName, Vector3 position, float fieldOfView)
		{
			GameObject gameObject = new GameObject(cameraName);
			gameObject.transform.SetParent(base.transform, false);
			gameObject.transform.position = position;
			Camera camera = gameObject.AddComponent<Camera>();
			camera.enabled = false;
			camera.fieldOfView = fieldOfView;
			camera.nearClipPlane = 0.03f;
			camera.farClipPlane = 400f;
			UniversalAdditionalCameraData universalAdditionalCameraData = gameObject.AddComponent<UniversalAdditionalCameraData>();
			if (this._gameplayView != null)
			{
				camera.cullingMask = this._gameplayView.cullingMask;
				camera.clearFlags = this._gameplayView.clearFlags;
				camera.backgroundColor = this._gameplayView.backgroundColor;
				camera.allowHDR = this._gameplayView.allowHDR;
				camera.allowMSAA = this._gameplayView.allowMSAA;
				UniversalAdditionalCameraData component = this._gameplayView.GetComponent<UniversalAdditionalCameraData>();
				if (component != null)
				{
					universalAdditionalCameraData.renderPostProcessing = component.renderPostProcessing;
					universalAdditionalCameraData.antialiasing = component.antialiasing;
					universalAdditionalCameraData.antialiasingQuality = component.antialiasingQuality;
					universalAdditionalCameraData.renderShadows = component.renderShadows;
					universalAdditionalCameraData.volumeLayerMask = component.volumeLayerMask;
					universalAdditionalCameraData.stopNaN = component.stopNaN;
					universalAdditionalCameraData.dithering = component.dithering;
				}
			}
			gameObject.AddComponent<AudioListener>().enabled = false;
			return camera;
		}

		// Token: 0x06005F02 RID: 24322 RVA: 0x00259994 File Offset: 0x00257B94
		private void ActivateCamera(Camera next, Transform trackingTarget, Vector3 followOffset)
		{
			foreach (Camera camera in new Camera[] { this._walkInCamera, this._showerCamera, this._theftCamera, this._exitCamera })
			{
				if (!(camera == null) && !(camera == next))
				{
					camera.enabled = false;
					camera.GetComponent<AudioListener>().enabled = false;
				}
			}
			this._activeCamera = next;
			this._trackingTarget = trackingTarget;
			this._followOffset = followOffset;
			this._follow = followOffset != Vector3.zero;
			if (next == null)
			{
				return;
			}
			next.enabled = true;
			next.GetComponent<AudioListener>().enabled = true;
			this.UpdateCamera(true);
		}

		// Token: 0x06005F03 RID: 24323 RVA: 0x00259A50 File Offset: 0x00257C50
		private void UpdateCamera(bool snap)
		{
			if (this._activeCamera == null)
			{
				return;
			}
			Transform transform = this._activeCamera.transform;
			if (this._activeCamera == this._showerCamera)
			{
				Transform transform2 = ((this._animator != null) ? this._animator.GetBoneTransform(HumanBodyBones.Head) : null);
				if (transform2 == null)
				{
					return;
				}
				Vector3 vector = transform2.position + this._standRotation * this._config.showerCameraLocalOffset;
				Vector3 vector2 = this._standRotation * Vector3.forward;
				float num = Vector3.Dot(vector - this._standPosition, vector2);
				if (num > this._config.showerCameraMaxForward)
				{
					vector -= vector2 * (num - this._config.showerCameraMaxForward);
				}
				transform.position = vector;
				transform.rotation = Quaternion.LookRotation(transform2.position + Vector3.down * 0.03f - vector);
				return;
			}
			else
			{
				if (this._trackingTarget == null)
				{
					return;
				}
				float num2 = (this._follow ? this._config.followCameraLookHeight : 1.2f);
				Vector3 vector3 = this._trackingTarget.position + Vector3.up * num2;
				if (this._follow)
				{
					float y = this._trackingTarget.eulerAngles.y;
					if (snap)
					{
						this._cameraYaw = y;
						this._cameraYawVelocity = 0f;
					}
					else
					{
						this._cameraYaw = Mathf.SmoothDampAngle(this._cameraYaw, y, ref this._cameraYawVelocity, 1.1f);
					}
					Vector3 vector4 = Quaternion.Euler(0f, this._cameraYaw, 0f) * this._followOffset;
					Vector3 vector5 = this._trackingTarget.position + vector4;
					Vector3 vector6 = vector5 - vector3;
					float magnitude = vector6.magnitude;
					float num3 = 1f;
					if (magnitude > 0.01f)
					{
						float num4 = magnitude;
						int num5 = Physics.SphereCastNonAlloc(vector3, 0.18f, vector6 / magnitude, this._cameraHits, magnitude, -1, QueryTriggerInteraction.Ignore);
						for (int i = 0; i < num5; i++)
						{
							if (this._cameraHits[i].collider != null && !this._cameraHits[i].collider.transform.IsChildOf(this._trackingTarget) && this._cameraHits[i].distance > 0f && this._cameraHits[i].distance < num4)
							{
								num4 = this._cameraHits[i].distance;
							}
						}
						if (num4 < magnitude)
						{
							num3 = Mathf.Max(0.35f, num4 - 0.05f) / magnitude;
						}
					}
					if (snap)
					{
						this._cameraReach = num3;
						this._cameraReachVelocity = 0f;
					}
					else
					{
						this._cameraReach = Mathf.SmoothDamp(this._cameraReach, num3, ref this._cameraReachVelocity, (num3 < this._cameraReach) ? 0.12f : 0.7f);
					}
					vector5 = vector3 + vector6 * this._cameraReach;
					if (snap)
					{
						transform.position = vector5;
						this._cameraVelocity = Vector3.zero;
					}
					else
					{
						transform.position = Vector3.SmoothDamp(transform.position, vector5, ref this._cameraVelocity, 0.35f);
					}
				}
				if (snap)
				{
					this._cameraFocus = vector3;
					this._cameraFocusVelocity = Vector3.zero;
				}
				else
				{
					this._cameraFocus = Vector3.SmoothDamp(this._cameraFocus, vector3, ref this._cameraFocusVelocity, 0.25f);
				}
				transform.rotation = Quaternion.LookRotation(this._cameraFocus - transform.position);
				return;
			}
		}

		// Token: 0x06005F04 RID: 24324 RVA: 0x00259E14 File Offset: 0x00258014
		private void BuildCielGraph(AnimationClip idle, AnimationClip walk, float walkPlaybackSpeed)
		{
			if (this._cielGraph.IsValid())
			{
				this._cielGraph.Destroy();
			}
			this._cielGraph = PlayableGraph.Create("Long Way Home - Gym Shower Ciel");
			this._cielGraph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
			AnimationClipPlayable animationClipPlayable = AnimationClipPlayable.Create(this._cielGraph, idle);
			animationClipPlayable.SetApplyFootIK(false);
			this._cielWalk = AnimationClipPlayable.Create(this._cielGraph, (walk != null) ? walk : idle);
			this._cielWalk.SetApplyFootIK(false);
			this._cielWalk.SetSpeed((double)walkPlaybackSpeed);
			this._cielBody = AnimationMixerPlayable.Create(this._cielGraph, 2);
			this._cielGraph.Connect<AnimationClipPlayable, AnimationMixerPlayable>(animationClipPlayable, 0, this._cielBody, 0);
			this._cielGraph.Connect<AnimationClipPlayable, AnimationMixerPlayable>(this._cielWalk, 0, this._cielBody, 1);
			this._walkWeight = 0f;
			this.SetWalkWeight();
			AnimationPlayableOutput.Create(this._cielGraph, "Ciel - Gym Shower", this._animator).SetSourcePlayable(this._cielBody);
			this._cielGraph.Play();
			this._cielGraph.Evaluate(0f);
		}

		// Token: 0x06005F05 RID: 24325 RVA: 0x00259F2F File Offset: 0x0025812F
		private void SetWalkWeight()
		{
			if (!this._cielBody.IsValid<AnimationMixerPlayable>())
			{
				return;
			}
			this._cielBody.SetInputWeight(0, 1f - this._walkWeight);
			this._cielBody.SetInputWeight(1, this._walkWeight);
		}

		// Token: 0x06005F06 RID: 24326 RVA: 0x00259F69 File Offset: 0x00258169
		private IEnumerator WalkAlong(Vector3[] waypoints, float metersPerSecond, bool alreadyMoving = false)
		{
			Vector3[] path = LongWayHomeGymShowerSequence.RoundCorners(waypoints, 4);
			float[] along = LongWayHomeGymShowerSequence.CumulativeLengths(path);
			float total = along[along.Length - 1];
			float travelled = 0f;
			float speed = (alreadyMoving ? metersPerSecond : 0f);
			float speedVelocity = 0f;
			float yaw = this._player.eulerAngles.y;
			float yawVelocity = 0f;
			float clipSpeed = (float)this._cielWalk.GetSpeed<AnimationClipPlayable>();
			if (!alreadyMoving)
			{
				this._cielWalk.SetTime(0.0);
			}
			while (travelled < total - 0.002f)
			{
				float deltaTime = Time.deltaTime;
				float num = LongWayHomeGymShowerSequence.BendDegrees(path, along, travelled, 1.2f);
				float num2 = metersPerSecond * Mathf.Lerp(1f, 0.8f, Mathf.InverseLerp(25f, 100f, num));
				float num3 = metersPerSecond * Mathf.Sqrt(Mathf.Clamp01((total - travelled) / 0.9f));
				float num4 = Mathf.Max(Mathf.Min(num2, num3), 0.15f);
				speed = Mathf.SmoothDamp(speed, num4, ref speedVelocity, 0.28f);
				travelled = Mathf.Min(total, travelled + speed * deltaTime);
				Vector3 vector = LongWayHomeGymShowerSequence.SampleCurve(path, along, travelled);
				vector.y = this._player.position.y;
				Vector3 vector2 = LongWayHomeGymShowerSequence.SampleCurve(path, along, Mathf.Min(total, travelled + 0.75f)) - vector;
				vector2.y = 0f;
				if (vector2.sqrMagnitude > 0.0004f)
				{
					yaw = Mathf.SmoothDampAngle(yaw, Quaternion.LookRotation(vector2).eulerAngles.y, ref yawVelocity, 0.22f);
				}
				Quaternion quaternion = Quaternion.Euler(0f, yaw, 0f);
				this._player.SetPositionAndRotation(vector, quaternion);
				this._body.position = vector;
				this._body.rotation = quaternion;
				float num5 = ((metersPerSecond > 0f) ? Mathf.Clamp01(speed / metersPerSecond) : 0f);
				this._walkWeight = Mathf.Clamp01(num5 * 1.6f);
				this.SetWalkWeight();
				this._cielWalk.SetSpeed((double)(clipSpeed * Mathf.Lerp(0.7f, 1f, num5)));
				yield return null;
			}
			this._cielWalk.SetSpeed((double)clipSpeed);
			while (this._walkWeight > 0f)
			{
				this._walkWeight = Mathf.MoveTowards(this._walkWeight, 0f, Time.deltaTime / 0.25f);
				this.SetWalkWeight();
				yield return null;
			}
			yield break;
		}

		// Token: 0x06005F07 RID: 24327 RVA: 0x00259F90 File Offset: 0x00258190
		private static float[] CumulativeLengths(Vector3[] path)
		{
			float[] array = new float[path.Length];
			for (int i = 1; i < path.Length; i++)
			{
				Vector3 vector = path[i] - path[i - 1];
				vector.y = 0f;
				array[i] = array[i - 1] + vector.magnitude;
			}
			return array;
		}

		// Token: 0x06005F08 RID: 24328 RVA: 0x00259FE8 File Offset: 0x002581E8
		private static Vector3 SampleCurve(Vector3[] path, float[] along, float distance)
		{
			if (distance <= 0f)
			{
				return path[0];
			}
			for (int i = 1; i < path.Length; i++)
			{
				if (distance <= along[i])
				{
					float num = along[i] - along[i - 1];
					return Vector3.Lerp(path[i - 1], path[i], (num > 0f) ? ((distance - along[i - 1]) / num) : 1f);
				}
			}
			return path[path.Length - 1];
		}

		// Token: 0x06005F09 RID: 24329 RVA: 0x0025A05C File Offset: 0x0025825C
		private static float BendDegrees(Vector3[] path, float[] along, float distance, float lookAhead)
		{
			float num = along[along.Length - 1];
			Vector3 vector = LongWayHomeGymShowerSequence.SampleCurve(path, along, Mathf.Min(num, distance + 0.2f)) - LongWayHomeGymShowerSequence.SampleCurve(path, along, distance);
			Vector3 vector2 = LongWayHomeGymShowerSequence.SampleCurve(path, along, Mathf.Min(num, distance + lookAhead + 0.2f)) - LongWayHomeGymShowerSequence.SampleCurve(path, along, Mathf.Min(num, distance + lookAhead));
			vector.y = (vector2.y = 0f);
			if (vector.sqrMagnitude >= 1E-06f && vector2.sqrMagnitude >= 1E-06f)
			{
				return Vector3.Angle(vector, vector2);
			}
			return 0f;
		}

		// Token: 0x06005F0A RID: 24330 RVA: 0x0025A0FE File Offset: 0x002582FE
		private IEnumerator MovePhotographer(Vector3[] path, float metersPerSecond, LongWayHomeGymShowerPhotographer.Pose pose)
		{
			this._photographer.SetPose(pose, 0.2f);
			path = LongWayHomeGymShowerSequence.RoundCorners(path, 3);
			int num;
			for (int i = 1; i < path.Length; i = num + 1)
			{
				while (!this._photographer.MoveTowards(path[i], metersPerSecond, 330f, Time.deltaTime))
				{
					yield return null;
				}
				num = i;
			}
			if (pose != LongWayHomeGymShowerPhotographer.Pose.Run)
			{
				this._photographer.SetPose(LongWayHomeGymShowerPhotographer.Pose.Idle, 0.25f);
			}
			yield break;
		}

		// Token: 0x06005F0B RID: 24331 RVA: 0x0025A122 File Offset: 0x00258322
		private IEnumerator TurnPhotographer(Vector3 point)
		{
			float deadline = Time.time + 1.5f;
			while (!this._photographer.TurnTowards(point, 220f, Time.deltaTime) && Time.time < deadline)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x06005F0C RID: 24332 RVA: 0x0025A138 File Offset: 0x00258338
		private static Vector3[] RoundCorners(Vector3[] path, int iterations = 3)
		{
			if (path == null || path.Length < 3)
			{
				return path;
			}
			List<Vector3> list = new List<Vector3>(path);
			for (int i = 0; i < iterations; i++)
			{
				List<Vector3> list2 = new List<Vector3>(list.Count * 2) { list[0] };
				for (int j = 0; j < list.Count - 1; j++)
				{
					if (j > 0)
					{
						list2.Add(Vector3.Lerp(list[j], list[j + 1], 0.25f));
					}
					if (j < list.Count - 2)
					{
						list2.Add(Vector3.Lerp(list[j], list[j + 1], 0.75f));
					}
				}
				list2.Add(list[list.Count - 1]);
				list = list2;
			}
			return list.ToArray();
		}

		// Token: 0x06005F0D RID: 24333 RVA: 0x0025A204 File Offset: 0x00258404
		private static Quaternion FacingRotation(Vector3 from, Vector3 to)
		{
			Vector3 vector = to - from;
			vector.y = 0f;
			if (vector.sqrMagnitude <= 0.0001f)
			{
				return Quaternion.identity;
			}
			return Quaternion.LookRotation(vector);
		}

		// Token: 0x06005F0E RID: 24334 RVA: 0x0025A240 File Offset: 0x00258440
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
			if (this.CanSkip)
			{
				if (this._skipHint == null)
				{
					this._skipHint = new LongWayHomeCinematicSkip(this);
				}
				this._skipHint.Show();
				if (LongWayHomeCinematicSkip.TryConsume(this))
				{
					this.TrySkip();
				}
			}
			else if (this._captured)
			{
				EscapeInputCoordinator.TryConsume(this);
			}
			LongWayHomeGymShowerPhotographer photographer = this._photographer;
			if (photographer == null)
			{
				return;
			}
			photographer.Tick(Time.deltaTime);
		}

		// Token: 0x06005F0F RID: 24335 RVA: 0x0025A2E0 File Offset: 0x002584E0
		private void LateUpdate()
		{
			if (this._captured)
			{
				this.KeepStallOpen();
			}
			if (!this._captured || this._animator == null)
			{
				return;
			}
			if (this._grounded && this._config != null)
			{
				float num = this.MinFootHeight() - (this._config.showerFloorY + this._ankleHeight);
				if (Mathf.Abs(num) > 0.001f)
				{
					Vector3 vector = this._player.position + Vector3.down * num;
					this._player.position = vector;
					this._body.position = vector;
				}
			}
			for (int i = 1; i < this._animator.layerCount; i++)
			{
				this._animator.SetLayerWeight(i, (float)((i == this._faceLayer) ? 1 : 0));
			}
			if (this._faceAnimator != null && this._faceAnimator.GetInteger(this._faceParameter) != this._neutralFaceIndex)
			{
				this._faceAnimator.SetInteger(this._faceParameter, this._neutralFaceIndex);
			}
			this.UpdateCamera(false);
		}

		// Token: 0x06005F10 RID: 24336 RVA: 0x0025A3F8 File Offset: 0x002585F8
		private void TeleportPlayer(Vector3 position, Quaternion rotation)
		{
			this._player.SetPositionAndRotation(position, rotation);
			this._body.position = position;
			this._body.rotation = rotation;
			if (!this._body.isKinematic)
			{
				this._body.velocity = Vector3.zero;
				this._body.angularVelocity = Vector3.zero;
			}
			Physics.SyncTransforms();
			foreach (MagicaCloth magicaCloth in this._player.GetComponentsInChildren<MagicaCloth>(true))
			{
				if (magicaCloth != null && magicaCloth.isActiveAndEnabled)
				{
					magicaCloth.ResetCloth(false);
				}
			}
		}

		// Token: 0x06005F11 RID: 24337 RVA: 0x0025A493 File Offset: 0x00258693
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

		// Token: 0x06005F12 RID: 24338 RVA: 0x0025A4A4 File Offset: 0x002586A4
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
			this._interpolation = this._body.interpolation;
			this._body.interpolation = RigidbodyInterpolation.None;
			this._body.isKinematic = true;
			this._body.useGravity = false;
			this._body.constraints = RigidbodyConstraints.FreezeAll;
		}

		// Token: 0x06005F13 RID: 24339 RVA: 0x0025A6C4 File Offset: 0x002588C4
		private void CaptureFace()
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
				return;
			}
			this._faceAnimator.SetInteger(this._faceParameter, this._neutralFaceIndex);
		}

		// Token: 0x06005F14 RID: 24340 RVA: 0x0025A814 File Offset: 0x00258A14
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
				foreach (Canvas canvas in gameObject.GetComponentsInChildren<Canvas>(true))
				{
					if (!GameplayHudVisibility.IsProtected(canvas.transform))
					{
						this.Suppress(canvas);
					}
				}
				foreach (AdaptiveHeartbeatDriver adaptiveHeartbeatDriver in gameObject.GetComponentsInChildren<AdaptiveHeartbeatDriver>(true))
				{
					this.Suppress(adaptiveHeartbeatDriver);
				}
				foreach (HeartbeatController heartbeatController in gameObject.GetComponentsInChildren<HeartbeatController>(true))
				{
					this.Suppress(heartbeatController);
				}
				foreach (BreathController breathController in gameObject.GetComponentsInChildren<BreathController>(true))
				{
					this.Suppress(breathController);
				}
			}
		}

		// Token: 0x06005F15 RID: 24341 RVA: 0x0025A970 File Offset: 0x00258B70
		private void Suppress(Behaviour component)
		{
			this._presentation.Add(new LongWayHomeGymShowerSequence.BehaviourState
			{
				Component = component,
				Enabled = component.enabled
			});
			component.enabled = false;
		}

		// Token: 0x06005F16 RID: 24342 RVA: 0x0025A9B0 File Offset: 0x00258BB0
		private bool CaptureWardrobe()
		{
			if (this._wardrobeSnapshot != null)
			{
				return true;
			}
			this._wardrobePresentation = WardrobeGameplayContext.BeginPresentationOverride(this);
			this._wearController = this._player.GetComponentInChildren<WearableUseController>(true);
			this._wearWasEnabled = this._wearController != null && this._wearController.enabled;
			if (this._wearController != null)
			{
				this._wearController.enabled = false;
			}
			bool flag;
			try
			{
				this._wardrobeSnapshot = this._style.CaptureWardrobeState(this._clothing);
				flag = this._wardrobeSnapshot != null;
			}
			catch (Exception ex)
			{
				Debug.LogException(ex, this);
				this.RestoreWardrobe();
				flag = false;
			}
			return flag;
		}

		// Token: 0x06005F17 RID: 24343 RVA: 0x0025AA68 File Offset: 0x00258C68
		private void RemoveGarments(bool includeGymAccessories)
		{
			foreach (MCStylePartDefinitionSO mcstylePartDefinitionSO in this._style.PartDefinitions)
			{
				if (!(mcstylePartDefinitionSO == null) && !mcstylePartDefinitionSO.isBodyAppearance && (mcstylePartDefinitionSO.partId.StartsWith("Outfit.", StringComparison.Ordinal) || (includeGymAccessories && Array.IndexOf<string>(LongWayHomeGymShowerSequence.GymAccessorySlots, mcstylePartDefinitionSO.partId) >= 0)) && (this._style.IsSlotEquipped(mcstylePartDefinitionSO.partId) || this._style.GetSpawnedRoot(mcstylePartDefinitionSO.partId) != null))
				{
					this._style.SetSlotEquipped(mcstylePartDefinitionSO.partId, false);
				}
			}
		}

		// Token: 0x06005F18 RID: 24344 RVA: 0x0025AB40 File Offset: 0x00258D40
		private void RestoreWardrobe()
		{
			MCStyleCustomizationService.WardrobeSnapshot wardrobeSnapshot = this._wardrobeSnapshot;
			this._wardrobeSnapshot = null;
			WearableUseController wearController = this._wearController;
			this._wearController = null;
			try
			{
				if (this._style != null && wardrobeSnapshot != null)
				{
					this._style.RestoreWardrobeState(wardrobeSnapshot);
				}
			}
			finally
			{
				try
				{
					if (wearController != null)
					{
						wearController.enabled = this._wearWasEnabled;
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

		// Token: 0x06005F19 RID: 24345 RVA: 0x0025ABD8 File Offset: 0x00258DD8
		private IEnumerator FadeScreen(bool toBlack, float seconds)
		{
			ScreenFadeController instance = ScreenFadeController.Instance;
			if (instance == null)
			{
				yield return new WaitForSecondsRealtime(seconds);
				yield break;
			}
			bool done = false;
			if (toBlack)
			{
				instance.FadeOut(seconds, delegate
				{
					done = true;
				});
			}
			else
			{
				instance.FadeIn(seconds, delegate
				{
					done = true;
				});
			}
			float deadline = Time.realtimeSinceStartup + seconds + 2f;
			while (!done && Time.realtimeSinceStartup < deadline)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x06005F1A RID: 24346 RVA: 0x0025ABF0 File Offset: 0x00258DF0
		private void Fail(string reason)
		{
			this.Phase = LongWayHomeGymShowerSequence.StoryPhase.Failed;
			Debug.LogError("[LongWayHomeGymShower] " + reason, this);
			this.ReleasePlayer();
			ScreenFadeController instance = ScreenFadeController.Instance;
			if (instance != null)
			{
				instance.SetClear();
			}
			LongWayHomeRuntimeController instance2 = LongWayHomeRuntimeController.Instance;
			if (instance2 == null)
			{
				return;
			}
			instance2.FailGymShowerPrologue(reason);
		}

		// Token: 0x06005F1B RID: 24347 RVA: 0x0025AC3C File Offset: 0x00258E3C
		private void OnDisable()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			base.StopAllCoroutines();
			this.ReleasePlayer();
			if (LongWayHomeGymShowerSequence._activePresenter == this)
			{
				LongWayHomeGymShowerSequence._activePresenter = null;
			}
		}

		// Token: 0x06005F1C RID: 24348 RVA: 0x0025AC68 File Offset: 0x00258E68
		private void OnDestroy()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			this.ReleasePlayer();
			this.ReleaseWet();
			if (this._photographer != null)
			{
				this._photographer.PhotoTaken -= this.OnPhotoTaken;
				this._photographer.Destroy();
				this._photographer = null;
			}
			if (LongWayHomeGymShowerSequence._activePresenter == this)
			{
				LongWayHomeGymShowerSequence._activePresenter = null;
			}
		}

		// Token: 0x06005F1D RID: 24349 RVA: 0x0025ACD0 File Offset: 0x00258ED0
		private void ReleasePlayer()
		{
			LongWayHomeCinematicSkip skipHint = this._skipHint;
			if (skipHint != null)
			{
				skipHint.Dispose();
			}
			this.IsReady = false;
			this._grounded = false;
			LongWayHomeGymShowerFaceDriver face = this._face;
			if (face != null)
			{
				face.Release();
			}
			this.RestoreStall();
			LongWayHomeRestaurantDialogue dialogue = this._dialogue;
			if (dialogue != null)
			{
				dialogue.Abort();
			}
			if (this._cielGraph.IsValid())
			{
				this._cielGraph.Destroy();
			}
			foreach (Camera camera in new Camera[] { this._walkInCamera, this._showerCamera, this._theftCamera, this._exitCamera })
			{
				if (!(camera == null))
				{
					camera.enabled = false;
					AudioListener component = camera.GetComponent<AudioListener>();
					if (component != null)
					{
						component.enabled = false;
					}
				}
			}
			this._activeCamera = null;
			if (this.Phase != LongWayHomeGymShowerSequence.StoryPhase.Done && this.Phase != LongWayHomeGymShowerSequence.StoryPhase.Handoff)
			{
				this.StopShowerEffects();
			}
			if (this._photographer != null)
			{
				this._photographer.Hide();
			}
			if (this._clothesPile != null)
			{
				this._clothesPile.SetActive(false);
			}
			this.RestoreWardrobe();
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
					this._body.interpolation = this._interpolation;
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
			foreach (LongWayHomeGymShowerSequence.BehaviourState behaviourState in this._presentation)
			{
				if (behaviourState.Component != null)
				{
					behaviourState.Component.enabled = behaviourState.Enabled;
				}
			}
			this._presentation.Clear();
			PlayerActivityLease lease = this._lease;
			if (lease != null)
			{
				lease.Dispose();
			}
			this._lease = null;
		}

		// Token: 0x04006026 RID: 24614
		private const string WetOwner = "LongWayHomeGymShower";

		// Token: 0x04006027 RID: 24615
		private const string FaceRendererPath = "3D Model/Ciel_07_05_26_ass/Body";

		// Token: 0x04006028 RID: 24616
		private const float ReadinessTimeout = 60f;

		// Token: 0x04006029 RID: 24617
		private const float TurnDegreesPerSecond = 220f;

		// Token: 0x0400602A RID: 24618
		private const float CielTurnDegreesPerSecond = 160f;

		// Token: 0x0400602B RID: 24619
		private static readonly string[] GymAccessorySlots = new string[] { "Accessory.Wrist", "Accessory.Earphones" };

		// Token: 0x0400602C RID: 24620
		private static LongWayHomeGymShowerSequence _activePresenter;

		// Token: 0x0400602D RID: 24621
		private readonly List<LongWayHomeGymShowerSequence.BehaviourState> _presentation = new List<LongWayHomeGymShowerSequence.BehaviourState>();

		// Token: 0x0400602E RID: 24622
		private readonly List<ParticleSystem> _water = new List<ParticleSystem>();

		// Token: 0x0400602F RID: 24623
		private LongWayHomeGymShowerConfigSO _config;

		// Token: 0x04006030 RID: 24624
		private LongWayHomeRestaurantDialogue _dialogue;

		// Token: 0x04006031 RID: 24625
		private LongWayHomeGymShowerPhotographer _photographer;

		// Token: 0x04006032 RID: 24626
		private Transform _player;

		// Token: 0x04006033 RID: 24627
		private Animator _animator;

		// Token: 0x04006034 RID: 24628
		private vThirdPersonInput _input;

		// Token: 0x04006035 RID: 24629
		private vThirdPersonCamera _gameplayCamera;

		// Token: 0x04006036 RID: 24630
		private Camera _gameplayView;

		// Token: 0x04006037 RID: 24631
		private Rigidbody _body;

		// Token: 0x04006038 RID: 24632
		private MCStyleCustomizationService _style;

		// Token: 0x04006039 RID: 24633
		private ClothingToggleController _clothing;

		// Token: 0x0400603A RID: 24634
		private CoverPoseController _coverPose;

		// Token: 0x0400603B RID: 24635
		private ShowerBubbleSpawner _bubbles;

		// Token: 0x0400603C RID: 24636
		private CielWetController _wet;

		// Token: 0x0400603D RID: 24637
		private PlayerActivityLease _lease;

		// Token: 0x0400603E RID: 24638
		private bool _captured;

		// Token: 0x0400603F RID: 24639
		private bool _inputEnabled;

		// Token: 0x04006040 RID: 24640
		private bool _inputLock;

		// Token: 0x04006041 RID: 24641
		private bool _cameraLock;

		// Token: 0x04006042 RID: 24642
		private bool _cameraFrozen;

		// Token: 0x04006043 RID: 24643
		private bool _coverEnabled;

		// Token: 0x04006044 RID: 24644
		private bool _rootMotion;

		// Token: 0x04006045 RID: 24645
		private bool _kinematic;

		// Token: 0x04006046 RID: 24646
		private bool _gravity;

		// Token: 0x04006047 RID: 24647
		private bool _wetAcquired;

		// Token: 0x04006048 RID: 24648
		private RigidbodyConstraints _constraints;

		// Token: 0x04006049 RID: 24649
		private float[] _layerWeights;

		// Token: 0x0400604A RID: 24650
		private AnimatorCullingMode _culling;

		// Token: 0x0400604B RID: 24651
		private PlayerEmotionController _emotion;

		// Token: 0x0400604C RID: 24652
		private Animator _faceAnimator;

		// Token: 0x0400604D RID: 24653
		private string _faceParameter;

		// Token: 0x0400604E RID: 24654
		private int _faceLayer = -1;

		// Token: 0x0400604F RID: 24655
		private int _savedFaceIndex;

		// Token: 0x04006050 RID: 24656
		private int _neutralFaceIndex;

		// Token: 0x04006051 RID: 24657
		private bool _ownsFaceOverride;

		// Token: 0x04006052 RID: 24658
		private bool _ownsEmotionOverride;

		// Token: 0x04006053 RID: 24659
		private MCStyleCustomizationService.WardrobeSnapshot _wardrobeSnapshot;

		// Token: 0x04006054 RID: 24660
		private WearableUseController _wearController;

		// Token: 0x04006055 RID: 24661
		private bool _wearWasEnabled;

		// Token: 0x04006056 RID: 24662
		private IDisposable _wardrobePresentation;

		// Token: 0x04006057 RID: 24663
		private PlayableGraph _cielGraph;

		// Token: 0x04006058 RID: 24664
		private AnimationMixerPlayable _cielBody;

		// Token: 0x04006059 RID: 24665
		private AnimationClipPlayable _cielWalk;

		// Token: 0x0400605A RID: 24666
		private float _walkWeight;

		// Token: 0x0400605B RID: 24667
		private LongWayHomeGymShowerFaceDriver _face;

		// Token: 0x0400605C RID: 24668
		private float _ankleHeight = 0.09f;

		// Token: 0x0400605D RID: 24669
		private bool _grounded;

		// Token: 0x0400605E RID: 24670
		private RigidbodyInterpolation _interpolation;

		// Token: 0x0400605F RID: 24671
		private readonly List<KeyValuePair<Transform, Quaternion>> _openLeaves = new List<KeyValuePair<Transform, Quaternion>>();

		// Token: 0x04006060 RID: 24672
		private readonly List<GameObject> _doorCopies = new List<GameObject>();

		// Token: 0x04006061 RID: 24673
		private readonly List<MeshRenderer> _hiddenDoorLeaves = new List<MeshRenderer>();

		// Token: 0x04006062 RID: 24674
		private Vector3 _followOffset;

		// Token: 0x04006063 RID: 24675
		private bool _follow;

		// Token: 0x04006064 RID: 24676
		private readonly RaycastHit[] _cameraHits = new RaycastHit[16];

		// Token: 0x04006065 RID: 24677
		private float _cameraYaw;

		// Token: 0x04006066 RID: 24678
		private float _cameraYawVelocity;

		// Token: 0x04006067 RID: 24679
		private float _cameraReach = 1f;

		// Token: 0x04006068 RID: 24680
		private float _cameraReachVelocity;

		// Token: 0x04006069 RID: 24681
		private Vector3 _cameraVelocity;

		// Token: 0x0400606A RID: 24682
		private Vector3 _cameraFocus;

		// Token: 0x0400606B RID: 24683
		private Vector3 _cameraFocusVelocity;

		// Token: 0x0400606C RID: 24684
		private Transform _stall;

		// Token: 0x0400606D RID: 24685
		private ShowerInteraction _station;

		// Token: 0x0400606E RID: 24686
		private Vector3 _standPosition;

		// Token: 0x0400606F RID: 24687
		private Quaternion _standRotation;

		// Token: 0x04006070 RID: 24688
		private AudioSource _showerAudio;

		// Token: 0x04006071 RID: 24689
		private AudioClip _showerLoop;

		// Token: 0x04006072 RID: 24690
		private float _showerVolume = 0.18f;

		// Token: 0x04006073 RID: 24691
		private GameObject _clothesPile;

		// Token: 0x04006074 RID: 24692
		private Camera _walkInCamera;

		// Token: 0x04006075 RID: 24693
		private Camera _showerCamera;

		// Token: 0x04006076 RID: 24694
		private Camera _theftCamera;

		// Token: 0x04006077 RID: 24695
		private Camera _exitCamera;

		// Token: 0x04006078 RID: 24696
		private Camera _activeCamera;

		// Token: 0x04006079 RID: 24697
		private Transform _trackingTarget;

		// Token: 0x0400607A RID: 24698
		private bool _firstPhotoTaken;

		// Token: 0x0400607B RID: 24699
		private bool _skipRequested;

		// Token: 0x0400607C RID: 24700
		private LongWayHomeCinematicSkip _skipHint;

		// Token: 0x02000CC4 RID: 3268
		public enum StoryPhase
		{
			// Token: 0x04006081 RID: 24705
			Loading,
			// Token: 0x04006082 RID: 24706
			WalkIn,
			// Token: 0x04006083 RID: 24707
			Shower,
			// Token: 0x04006084 RID: 24708
			Photos,
			// Token: 0x04006085 RID: 24709
			Dialogue,
			// Token: 0x04006086 RID: 24710
			Theft,
			// Token: 0x04006087 RID: 24711
			Exit,
			// Token: 0x04006088 RID: 24712
			Handoff,
			// Token: 0x04006089 RID: 24713
			Done,
			// Token: 0x0400608A RID: 24714
			Failed
		}

		// Token: 0x02000CC5 RID: 3269
		private struct BehaviourState
		{
			// Token: 0x0400608B RID: 24715
			public Behaviour Component;

			// Token: 0x0400608C RID: 24716
			public bool Enabled;
		}
	}
}
