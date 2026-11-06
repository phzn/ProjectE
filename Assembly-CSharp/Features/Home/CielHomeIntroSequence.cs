using System;
using System.Collections;
using System.Collections.Generic;
using Features.Detection;
using Features.Loading;
using Features.WardrobeFeedback;
using Invector.vCamera;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using MagicaCloth2;
using ProjectExhibitionist.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.Home
{
	// Token: 0x02001189 RID: 4489
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(-50)]
	public sealed class CielHomeIntroSequence : MonoBehaviour
	{
		// Token: 0x170015EC RID: 5612
		// (get) Token: 0x0600897B RID: 35195 RVA: 0x0038361B File Offset: 0x0038181B
		public static bool IsAnyRunning
		{
			get
			{
				return CielHomeIntroSequence._activePresentation != null && CielHomeIntroSequence._activePresentation.OwnsDestinationReveal;
			}
		}

		// Token: 0x0600897C RID: 35196 RVA: 0x00383636 File Offset: 0x00381836
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetPresentationOwner()
		{
			CielHomeIntroSequence._activePresentation = null;
		}

		// Token: 0x170015ED RID: 5613
		// (get) Token: 0x0600897D RID: 35197 RVA: 0x0038363E File Offset: 0x0038183E
		public bool OwnsDestinationReveal
		{
			get
			{
				return base.isActiveAndEnabled && (this._destinationRevealPending || this._running);
			}
		}

		// Token: 0x170015EE RID: 5614
		// (get) Token: 0x0600897E RID: 35198 RVA: 0x0038365A File Offset: 0x0038185A
		// (set) Token: 0x0600897F RID: 35199 RVA: 0x00383662 File Offset: 0x00381862
		public CielHomeIntroSequence.WakeVariant LastSelectedVariant { get; private set; }

		// Token: 0x170015EF RID: 5615
		// (get) Token: 0x06008980 RID: 35200 RVA: 0x0038366B File Offset: 0x0038186B
		// (set) Token: 0x06008981 RID: 35201 RVA: 0x00383673 File Offset: 0x00381873
		public bool HasCompleted { get; private set; }

		// Token: 0x170015F0 RID: 5616
		// (get) Token: 0x06008982 RID: 35202 RVA: 0x0038367C File Offset: 0x0038187C
		private string ActivityReason
		{
			get
			{
				return "CielHomeIntro#" + base.GetInstanceID().ToString();
			}
		}

		// Token: 0x06008983 RID: 35203 RVA: 0x003836A4 File Offset: 0x003818A4
		private void Awake()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			if (this.homeAudio == null)
			{
				this.homeAudio = base.GetComponent<CielHomeAudioController>();
			}
			ScreenFadeController instance = ScreenFadeController.Instance;
			if (instance != null)
			{
				instance.SetBlack();
			}
			if (this.hideHud && HUDManager.Instance != null)
			{
				HUDManager.Instance.HideHUD();
				this._hudHidden = true;
			}
		}

		// Token: 0x06008984 RID: 35204 RVA: 0x00383709 File Offset: 0x00381909
		private void OnEnable()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			CielHomeIntroSequence._activePresentation = this;
			if (this._hasStarted)
			{
				return;
			}
			this._destinationRevealPending = true;
		}

		// Token: 0x06008985 RID: 35205 RVA: 0x00383729 File Offset: 0x00381929
		private void Start()
		{
			if (!Application.isPlaying || this._hasStarted)
			{
				return;
			}
			this._hasStarted = true;
			base.StartCoroutine(this.IntroRoutine());
		}

		// Token: 0x06008986 RID: 35206 RVA: 0x0038374F File Offset: 0x0038194F
		private void Update()
		{
			if (this._running)
			{
				EscapeInputCoordinator.TryConsume(this);
			}
		}

		// Token: 0x06008987 RID: 35207 RVA: 0x00383760 File Offset: 0x00381960
		private void LateUpdate()
		{
			if (this._cinematicAnimatorLayersSuppressed)
			{
				this.ForceCinematicAnimatorLayersOff();
			}
		}

		// Token: 0x06008988 RID: 35208 RVA: 0x00383770 File Offset: 0x00381970
		private void OnDisable()
		{
			if (CielHomeIntroSequence._activePresentation == this)
			{
				CielHomeIntroSequence._activePresentation = null;
			}
			if (!Application.isPlaying || CielHomeIntroSequence.IsRuntimeTearingDown())
			{
				this.AbandonState();
				return;
			}
			bool flag = this.OwnsDestinationReveal && SceneManager.GetActiveScene() == base.gameObject.scene;
			this.RestoreControlledState(true);
			this._destinationRevealPending = false;
			this._running = false;
			if (flag)
			{
				ScreenFadeController instance = ScreenFadeController.Instance;
				if (instance == null)
				{
					return;
				}
				instance.SetClear();
			}
		}

		// Token: 0x06008989 RID: 35209 RVA: 0x003837EB File Offset: 0x003819EB
		private void OnDestroy()
		{
			if (CielHomeIntroSequence._activePresentation == this)
			{
				CielHomeIntroSequence._activePresentation = null;
			}
			if (CielHomeIntroSequence.IsRuntimeTearingDown())
			{
				this.AbandonState();
				return;
			}
			this.RestoreControlledState(true);
		}

		// Token: 0x0600898A RID: 35210 RVA: 0x00383815 File Offset: 0x00381A15
		private IEnumerator IntroRoutine()
		{
			this._running = true;
			this._stateRestored = false;
			while (ScreenFadeController.Instance == null)
			{
				yield return null;
			}
			ScreenFadeController fade = ScreenFadeController.Instance;
			fade.SetBlack();
			yield return this.WaitForGameplayReadiness();
			LoadingScreenController loading = LoadingScreenController.Instance;
			while (loading != null && loading.IsLoadingActive)
			{
				fade.SetBlack();
				yield return null;
			}
			yield return null;
			fade.SetBlack();
			yield return this.AcquireCinematicControl();
			if (!this.ResolvePlayerReferences() || !this.HasValidConfiguration())
			{
				Debug.LogError("[CielHomeIntro] Missing player or authored bindings; using safe reveal.", this);
				yield return this.RevealFallback(fade);
				yield break;
			}
			this.LastSelectedVariant = CielHomeIntroSequence.SelectWakeVariant(this.randomizeWakeVariant, this.fixedWakeVariant, Random.Range(0, 2));
			Transform transform = ((this.LastSelectedVariant == CielHomeIntroSequence.WakeVariant.Left) ? this.wakeAnchorLeft : this.wakeAnchorRight);
			string stateName = ((this.LastSelectedVariant == CielHomeIntroSequence.WakeVariant.Left) ? this.wakeLeftState : this.wakeRightState);
			vTriggerGenericAction action = ((this.LastSelectedVariant == CielHomeIntroSequence.WakeVariant.Left) ? this.wakeLeftAction : this.wakeRightAction);
			Transform[] path = ((this.LastSelectedVariant == CielHomeIntroSequence.WakeVariant.Left) ? this.cameraPathLeft : this.cameraPathRight);
			this.AcquireCinematicPresentation();
			this.TeleportPlayer(transform);
			yield return this.PrepareCinematicCamera(path);
			if (this.settleDelay > 0f)
			{
				yield return new WaitForSecondsRealtime(this.settleDelay);
			}
			this._actionAnimator.EnableRootMotionForAction();
			this._actionAnimator.TriggerAction(new PlayerActionEntry
			{
				ActionId = stateName,
				DisplayName = stateName,
				Duration = this.actionTimeout,
				TriggerPrefab = action
			});
			this._actionStarted = true;
			yield return this.WaitForWakeState(stateName, path);
			this.AlignWakePoseToBed();
			this.BindMattressHairCollider();
			yield return null;
			yield return null;
			this.ApplyCameraAlongPath(path, 0f);
			fade.FadeIn(this.openingFadeDuration, null);
			Coroutine cameraTravel = base.StartCoroutine(this.CameraTravel(path));
			yield return this.WaitForWakeEndAndFade(stateName, fade);
			if (cameraTravel != null)
			{
				base.StopCoroutine(cameraTravel);
			}
			this.CommitWakeEndpointForGameplay();
			this.UnbindMattressHairCollider();
			this.ReleaseCinematicPresentation();
			yield return this.RestoreGameplayCamera();
			if (this.blackHandoffHold > 0f)
			{
				yield return new WaitForSeconds(this.blackHandoffHold);
			}
			bool revealComplete = false;
			fade.FadeIn(this.gameplayFadeDuration, delegate
			{
				revealComplete = true;
			});
			float revealDeadline = Time.realtimeSinceStartup + this.gameplayFadeDuration + 2f;
			while (!revealComplete && Time.realtimeSinceStartup < revealDeadline)
			{
				yield return null;
			}
			this.RestoreControlledState(false);
			this._destinationRevealPending = false;
			this._running = false;
			this.HasCompleted = true;
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[CielHomeIntro] Complete ({0}, '{1}').", this.LastSelectedVariant, stateName), this);
			}
			yield break;
		}

		// Token: 0x0600898B RID: 35211 RVA: 0x00383824 File Offset: 0x00381A24
		private IEnumerator WaitForGameplayReadiness()
		{
			while (SceneManager.GetActiveScene() != base.gameObject.scene || !GameSystemsBootstrapper.GameplayReady)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x0600898C RID: 35212 RVA: 0x00383833 File Offset: 0x00381A33
		private IEnumerator AcquireCinematicControl()
		{
			float deadline = Time.realtimeSinceStartup + 1.5f;
			while (this._activityLease == null && Time.realtimeSinceStartup < deadline)
			{
				PlayerActivityGate.TryAcquire(this.ActivityReason, this, PlayerActivity.None, PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation, PlayerActivity.AllPlayerActions, out this._activityLease, false, null);
				if (this._activityLease == null)
				{
					yield return null;
				}
			}
			float playerDeadline = Time.realtimeSinceStartup + 4f;
			while (!this.ResolvePlayerReferences() && Time.realtimeSinceStartup < playerDeadline)
			{
				yield return null;
			}
			if (this._playerInput != null)
			{
				if (this._thirdPersonCamera == null)
				{
					this._thirdPersonCamera = Object.FindObjectOfType<vThirdPersonCamera>();
				}
				if (this._thirdPersonCamera != null)
				{
					this._previousCameraFreeze = this._thirdPersonCamera.isFreezed;
					this._cameraFreezeStateCaptured = true;
				}
				this._previousInputLock = this._playerInput.lockInput;
				this._previousCameraInputLock = this._playerInput.lockCameraInput;
				this._inputCaptured = true;
				this._playerInput.SetLockAllInput(true);
				this._playerInput.SetLockCameraInput(true);
				if (this._playerInput.cc != null)
				{
					this._playerInput.cc.input = Vector2.zero;
					this._playerInput.cc.isSprinting = false;
				}
			}
			if (this._coverPose != null)
			{
				this._coverPoseWasEnabled = this._coverPose.enabled;
				this._coverPose.enabled = false;
			}
			if (this.hideHud && HUDManager.Instance != null)
			{
				HUDManager.Instance.HideHUD();
				this._hudHidden = true;
			}
			yield break;
		}

		// Token: 0x0600898D RID: 35213 RVA: 0x00383844 File Offset: 0x00381A44
		private bool ResolvePlayerReferences()
		{
			if (this._playerInput == null)
			{
				this._playerInput = Object.FindObjectOfType<vThirdPersonInput>();
			}
			if (this._playerInput == null || this._playerInput.cc == null || this._playerInput.cc.animator == null)
			{
				return false;
			}
			this._animator = this._playerInput.cc.animator;
			if (this._actionAnimator == null)
			{
				this._actionAnimator = this._playerInput.GetComponent<PlayerActionAnimator>();
			}
			if (this._actionAnimator == null)
			{
				this._actionAnimator = this._playerInput.GetComponentInChildren<PlayerActionAnimator>(true);
			}
			if (this._actionAnimator == null || this._actionAnimator.GenericAction == null)
			{
				return false;
			}
			if (this._playerRigidbody == null)
			{
				this._playerRigidbody = this._playerInput.GetComponent<Rigidbody>();
			}
			if (this._coverPose == null)
			{
				this._coverPose = this._playerInput.GetComponentInChildren<CoverPoseController>(true);
			}
			return true;
		}

		// Token: 0x0600898E RID: 35214 RVA: 0x00383960 File Offset: 0x00381B60
		private bool HasValidConfiguration()
		{
			return this.wakeAnchorLeft != null && this.wakeAnchorRight != null && this.gameplayExitAnchor != null && this.wakeLeftAction != null && this.wakeRightAction != null && !string.IsNullOrEmpty(this.wakeLeftState) && !string.IsNullOrEmpty(this.wakeRightState);
		}

		// Token: 0x0600898F RID: 35215 RVA: 0x003839D0 File Offset: 0x00381BD0
		private void TeleportPlayer(Transform anchor)
		{
			this._playerInput.transform.SetPositionAndRotation(anchor.position, anchor.rotation);
			if (this._playerRigidbody != null)
			{
				this._playerRigidbody.velocity = Vector3.zero;
				this._playerRigidbody.angularVelocity = Vector3.zero;
				this._playerRigidbody.position = anchor.position;
				this._playerRigidbody.rotation = anchor.rotation;
			}
			Physics.SyncTransforms();
		}

		// Token: 0x06008990 RID: 35216 RVA: 0x00383A50 File Offset: 0x00381C50
		private void AlignWakePoseToBed()
		{
			if (this._playerInput == null || this._animator == null || !this._animator.isHuman || this.bedReference == null)
			{
				return;
			}
			Transform boneTransform = this._animator.GetBoneTransform(HumanBodyBones.Head);
			Transform boneTransform2 = this._animator.GetBoneTransform(HumanBodyBones.LeftFoot);
			Transform boneTransform3 = this._animator.GetBoneTransform(HumanBodyBones.RightFoot);
			if (boneTransform == null || boneTransform2 == null || boneTransform3 == null)
			{
				return;
			}
			Vector3 vector = (boneTransform2.position + boneTransform3.position) * 0.5f;
			Vector3 vector2 = (boneTransform.position + vector) * 0.5f;
			Vector3 vector3 = this.bedReference.position + this.bedReference.right * this.wakePoseLongitudinalOffset + this.bedReference.forward * this.wakePoseLateralOffset + Vector3.up * this.wakePoseHeightAboveBedOrigin;
			Vector3 vector4 = this._playerInput.transform.position + vector3 - vector2;
			this._playerInput.transform.position = vector4;
			if (this._playerRigidbody != null)
			{
				this._playerRigidbody.position = vector4;
				if (!this._playerRigidbody.isKinematic)
				{
					this._playerRigidbody.velocity = Vector3.zero;
					this._playerRigidbody.angularVelocity = Vector3.zero;
				}
			}
			Physics.SyncTransforms();
		}

		// Token: 0x06008991 RID: 35217 RVA: 0x00383BE4 File Offset: 0x00381DE4
		private void AcquireCinematicPresentation()
		{
			WearAdmireAnimation.SetPresentationSuppressed(this, true);
			if (this._animator == null || this._cinematicAnimatorLayersSuppressed)
			{
				return;
			}
			this._previousAnimatorCullingMode = this._animator.cullingMode;
			this._animatorCullingCaptured = true;
			this._animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
			this._savedAnimatorLayerWeights = new float[this._animator.layerCount];
			for (int i = 0; i < this._animator.layerCount; i++)
			{
				this._savedAnimatorLayerWeights[i] = this._animator.GetLayerWeight(i);
			}
			this._cinematicAnimatorLayersSuppressed = true;
			this.ForceCinematicAnimatorLayersOff();
			this._emotionController = ((this._playerInput != null) ? this._playerInput.GetComponent<PlayerEmotionController>() : PlayerEmotionController.Instance);
			if (this._emotionController != null && !this._emotionController.IsFacialExpressionOverriddenForAction)
			{
				this._emotionController.DisableFacialExpressionForAction();
				this._ownsFacialExpressionOverride = true;
			}
		}

		// Token: 0x06008992 RID: 35218 RVA: 0x00383CD4 File Offset: 0x00381ED4
		private void ForceCinematicAnimatorLayersOff()
		{
			if (this._animator == null)
			{
				return;
			}
			for (int i = 1; i < this._animator.layerCount; i++)
			{
				this._animator.SetLayerWeight(i, 0f);
			}
		}

		// Token: 0x06008993 RID: 35219 RVA: 0x00383D18 File Offset: 0x00381F18
		private void ReleaseCinematicPresentation()
		{
			this._cinematicAnimatorLayersSuppressed = false;
			if (this._animator != null && this._savedAnimatorLayerWeights != null)
			{
				int num = Mathf.Min(this._animator.layerCount, this._savedAnimatorLayerWeights.Length);
				for (int i = 1; i < num; i++)
				{
					this._animator.SetLayerWeight(i, this._savedAnimatorLayerWeights[i]);
				}
			}
			this._savedAnimatorLayerWeights = null;
			if (this._animatorCullingCaptured && this._animator != null)
			{
				this._animator.cullingMode = this._previousAnimatorCullingMode;
			}
			this._animatorCullingCaptured = false;
			if (this._ownsFacialExpressionOverride && this._emotionController != null)
			{
				this._emotionController.RestoreFacialExpression();
			}
			this._ownsFacialExpressionOverride = false;
			this._emotionController = null;
			WearAdmireAnimation.SetPresentationSuppressed(this, false);
		}

		// Token: 0x06008994 RID: 35220 RVA: 0x00383DE6 File Offset: 0x00381FE6
		private IEnumerator PrepareCinematicCamera(Transform[] path)
		{
			this._thirdPersonCamera = Object.FindObjectOfType<vThirdPersonCamera>();
			if (this._thirdPersonCamera == null)
			{
				yield break;
			}
			if (!this._cameraFreezeStateCaptured)
			{
				this._previousCameraFreeze = this._thirdPersonCamera.isFreezed;
				this._cameraFreezeStateCaptured = true;
			}
			this._thirdPersonCamera.isFreezed = false;
			this._thirdPersonCamera.RemoveLockTarget();
			this._thirdPersonCamera.ResetTarget();
			this._thirdPersonCamera.ResetAngleWithoutSmooth();
			if (!this._thirdPersonCamera.SnapToCurrentStateWithoutSmooth(true))
			{
				Debug.LogWarning("[CielHomeIntro] Gameplay camera was not ready for the opening snap.", this);
			}
			Physics.SyncTransforms();
			yield return new WaitForFixedUpdate();
			yield return null;
			this._previousCameraPosition = this._thirdPersonCamera.transform.position;
			this._previousCameraRotation = this._thirdPersonCamera.transform.rotation;
			this._cameraCaptured = true;
			this._thirdPersonCamera.isFreezed = true;
			this._renderCamera = Camera.main;
			if (this._renderCamera != null)
			{
				this._previousFieldOfView = this._renderCamera.fieldOfView;
				this._renderCamera.fieldOfView = this.openingFieldOfView;
			}
			this.ApplyCameraAlongPath(path, 0f);
			yield break;
		}

		// Token: 0x06008995 RID: 35221 RVA: 0x00383DFC File Offset: 0x00381FFC
		private IEnumerator WaitForWakeState(string stateName, Transform[] path)
		{
			int stateHash = Animator.StringToHash(stateName);
			float deadline = Time.realtimeSinceStartup + 3f;
			while (Time.realtimeSinceStartup < deadline && this._animator != null)
			{
				AnimatorStateInfo currentAnimatorStateInfo = this._animator.GetCurrentAnimatorStateInfo(0);
				bool flag = currentAnimatorStateInfo.shortNameHash == stateHash || currentAnimatorStateInfo.IsName(stateName);
				this.ApplyCameraAlongPath(path, 0f);
				if (flag && currentAnimatorStateInfo.normalizedTime > 0f)
				{
					yield return null;
					if (this._animator != null)
					{
						this._animator.Update(0f);
					}
					yield break;
				}
				yield return null;
			}
			Debug.LogWarning("[CielHomeIntro] State '" + stateName + "' missed the reveal sync timeout.", this);
			yield break;
		}

		// Token: 0x06008996 RID: 35222 RVA: 0x00383E19 File Offset: 0x00382019
		private IEnumerator WaitForWakeEndAndFade(string stateName, ScreenFadeController fade)
		{
			int stateHash = Animator.StringToHash(stateName);
			float deadline = Time.realtimeSinceStartup + Mathf.Max(1f, this.actionTimeout);
			bool fadeStarted = false;
			bool fadeComplete = false;
			bool wakeSoundPlayed = false;
			Action <>9__1;
			while (Time.realtimeSinceStartup < deadline && this._animator != null)
			{
				AnimatorStateInfo currentAnimatorStateInfo = this._animator.GetCurrentAnimatorStateInfo(0);
				if (currentAnimatorStateInfo.shortNameHash == stateHash || currentAnimatorStateInfo.IsName(stateName))
				{
					if (!wakeSoundPlayed && currentAnimatorStateInfo.normalizedTime >= this.wakeSoundNormalizedTime)
					{
						wakeSoundPlayed = true;
						CielHomeAudioController cielHomeAudioController = this.homeAudio;
						if (cielHomeAudioController != null)
						{
							cielHomeAudioController.PlayWakeUpFromBed();
						}
					}
					if (!fadeStarted && currentAnimatorStateInfo.normalizedTime >= this.fadeOutNormalizedTime)
					{
						fadeStarted = true;
						float num = this.handoffFadeDuration;
						Action action;
						if ((action = <>9__1) == null)
						{
							action = (<>9__1 = delegate
							{
								fadeComplete = true;
							});
						}
						fade.FadeOut(num, action);
					}
					if (currentAnimatorStateInfo.normalizedTime >= this.finishNormalizedTime)
					{
						break;
					}
				}
				yield return null;
			}
			if (!fadeStarted)
			{
				fade.FadeOut(this.handoffFadeDuration, delegate
				{
					fadeComplete = true;
				});
			}
			float fadeDeadline = Time.realtimeSinceStartup + this.handoffFadeDuration + 2f;
			while (!fadeComplete && Time.realtimeSinceStartup < fadeDeadline)
			{
				yield return null;
			}
			if (!fadeComplete)
			{
				fade.SetBlack();
			}
			yield break;
		}

		// Token: 0x06008997 RID: 35223 RVA: 0x00383E36 File Offset: 0x00382036
		private IEnumerator CameraTravel(Transform[] path)
		{
			float elapsed = 0f;
			while (elapsed < this.cameraTravelDuration && this._thirdPersonCamera != null)
			{
				elapsed += Time.unscaledDeltaTime;
				float num = Mathf.Clamp01(elapsed / Mathf.Max(0.01f, this.cameraTravelDuration));
				float num2 = ((this.cameraTravelCurve != null) ? Mathf.Clamp01(this.cameraTravelCurve.Evaluate(num)) : CielHomeIntroSequence.SmoothStep(num));
				this.ApplyCameraAlongPath(path, num2);
				if (this._renderCamera != null)
				{
					this._renderCamera.fieldOfView = Mathf.Lerp(this.openingFieldOfView, this.endingFieldOfView, CielHomeIntroSequence.SmoothStep(num2));
				}
				yield return null;
			}
			this.ApplyCameraAlongPath(path, 1f);
			yield break;
		}

		// Token: 0x06008998 RID: 35224 RVA: 0x00383E4C File Offset: 0x0038204C
		private void ApplyCameraAlongPath(Transform[] path, float t)
		{
			if (this._thirdPersonCamera == null || path == null || path.Length == 0)
			{
				return;
			}
			Vector3 cinematicLookTarget = this.GetCinematicLookTarget(t);
			Vector3 vector;
			if (path.Length == 1)
			{
				vector = path[0].localPosition;
			}
			else if (path.Length == 2)
			{
				vector = Vector3.Lerp(path[0].localPosition, path[1].localPosition, Mathf.Clamp01(t));
			}
			else
			{
				vector = CielHomeIntroSequence.EvaluateThreeBeatPath(path[0].localPosition, path[path.Length / 2].localPosition, path[path.Length - 1].localPosition, t);
			}
			Vector3 vector2 = cinematicLookTarget + vector;
			Vector3 vector3 = this.ResolveCameraCollision(cinematicLookTarget, vector2);
			if (Vector3.Distance(cinematicLookTarget, vector3) < this.minimumCameraDistance)
			{
				Vector3 vector4 = vector2 - cinematicLookTarget;
				if (vector4.sqrMagnitude > 0.0001f)
				{
					vector3 = cinematicLookTarget + vector4.normalized * this.minimumCameraDistance;
				}
			}
			Vector3 vector5 = cinematicLookTarget - vector3;
			Quaternion quaternion = ((vector5.sqrMagnitude > 0.0001f) ? Quaternion.LookRotation(vector5.normalized, Vector3.up) : this._thirdPersonCamera.transform.rotation);
			this._thirdPersonCamera.transform.SetPositionAndRotation(vector3, quaternion);
		}

		// Token: 0x06008999 RID: 35225 RVA: 0x00383F74 File Offset: 0x00382174
		private Vector3 GetCinematicLookTarget(float progress)
		{
			Vector3 lookTarget = this.GetLookTarget();
			if (this._playerInput == null)
			{
				return lookTarget;
			}
			float num = CielHomeIntroSequence.SmoothStep(Mathf.InverseLerp(0.55f, 1f, progress));
			Vector3 vector = this._playerInput.transform.position + this._playerInput.transform.up * 0.8f;
			return Vector3.Lerp(lookTarget, vector, num);
		}

		// Token: 0x0600899A RID: 35226 RVA: 0x00383FE8 File Offset: 0x003821E8
		private Vector3 GetLookTarget()
		{
			if (this._animator != null && this._animator.isHuman)
			{
				Transform boneTransform = this._animator.GetBoneTransform(HumanBodyBones.Head);
				if (boneTransform != null)
				{
					return boneTransform.position;
				}
			}
			if (this._playerInput != null)
			{
				return this._playerInput.transform.position + Vector3.up * this.fallbackLookHeight;
			}
			if (!(this.bedReference != null))
			{
				return base.transform.position;
			}
			return this.bedReference.position + Vector3.up * this.fallbackLookHeight;
		}

		// Token: 0x0600899B RID: 35227 RVA: 0x0038409C File Offset: 0x0038229C
		private Vector3 ResolveCameraCollision(Vector3 target, Vector3 desired)
		{
			Vector3 vector = desired - target;
			float magnitude = vector.magnitude;
			if (magnitude <= 0.001f)
			{
				return desired;
			}
			vector /= magnitude;
			RaycastHit[] array = Physics.SphereCastAll(target, this.cameraCollisionRadius, vector, magnitude, -1, QueryTriggerInteraction.Ignore);
			float num = magnitude;
			for (int i = 0; i < array.Length; i++)
			{
				Transform transform = ((array[i].collider != null) ? array[i].collider.transform : null);
				if (!(transform == null) && array[i].distance > this.minimumCameraDistance && (!(this._playerInput != null) || (!(transform == this._playerInput.transform) && !transform.IsChildOf(this._playerInput.transform))))
				{
					string text = LayerMask.LayerToName(transform.gameObject.layer);
					if (!(text == "Player") && !(text == "BodyPart") && !(text == "PlayerClothing") && (!(this._thirdPersonCamera != null) || !transform.IsChildOf(this._thirdPersonCamera.transform)))
					{
						num = Mathf.Min(num, array[i].distance);
					}
				}
			}
			return target + vector * Mathf.Max(0.05f, num - this.cameraCollisionPadding);
		}

		// Token: 0x0600899C RID: 35228 RVA: 0x00384214 File Offset: 0x00382414
		private void FinishWakeAction()
		{
			if (!this._actionStarted)
			{
				return;
			}
			PlayerActionAnimator actionAnimator = this._actionAnimator;
			if (actionAnimator != null)
			{
				actionAnimator.FinishCurrentAction();
			}
			PlayerActionAnimator actionAnimator2 = this._actionAnimator;
			if (actionAnimator2 != null)
			{
				actionAnimator2.RestoreRootMotionForAction();
			}
			PlayerActionAnimator actionAnimator3 = this._actionAnimator;
			if (actionAnimator3 != null)
			{
				actionAnimator3.RestoreOverrideLayer();
			}
			this._actionStarted = false;
		}

		// Token: 0x0600899D RID: 35229 RVA: 0x00384264 File Offset: 0x00382464
		private void CommitWakeEndpointForGameplay()
		{
			if (this._playerInput == null)
			{
				this.FinishWakeAction();
				return;
			}
			Quaternion rotation = this._playerInput.transform.rotation;
			Vector3 vector = this.ResolveWakeGameplayPosition(this._playerInput.transform.position, rotation);
			this.FinishWakeAction();
			if (this._animator != null)
			{
				this._animator.applyRootMotion = false;
				this._animator.Update(0f);
			}
			this._playerInput.transform.SetPositionAndRotation(vector, rotation);
			if (this._playerRigidbody != null)
			{
				this._playerRigidbody.position = vector;
				this._playerRigidbody.rotation = rotation;
				if (!this._playerRigidbody.isKinematic)
				{
					this._playerRigidbody.velocity = Vector3.zero;
					this._playerRigidbody.angularVelocity = Vector3.zero;
				}
			}
			Physics.SyncTransforms();
		}

		// Token: 0x0600899E RID: 35230 RVA: 0x0038434C File Offset: 0x0038254C
		private Vector3 ResolveWakeGameplayPosition(Vector3 endpoint, Quaternion rotation)
		{
			if (this.gameplayExitAnchor != null)
			{
				endpoint.y = this.gameplayExitAnchor.position.y;
			}
			bool flag = false;
			Bounds bounds = default(Bounds);
			if (this.bedReference != null)
			{
				foreach (Collider collider in this.bedReference.GetComponentsInChildren<Collider>(true))
				{
					if (collider.enabled && !collider.isTrigger && collider.gameObject.activeInHierarchy)
					{
						if (!flag)
						{
							bounds = collider.bounds;
						}
						else
						{
							bounds.Encapsulate(collider.bounds);
						}
						flag = true;
					}
				}
			}
			if (!flag)
			{
				if (!(this.gameplayExitAnchor != null))
				{
					return endpoint;
				}
				return this.gameplayExitAnchor.position;
			}
			else
			{
				CapsuleCollider component = this._playerInput.GetComponent<CapsuleCollider>();
				Vector3 lossyScale = this._playerInput.transform.lossyScale;
				float num = ((component != null) ? (component.radius * Mathf.Max(Mathf.Abs(lossyScale.x), Mathf.Abs(lossyScale.z))) : 0.2f);
				Vector3 vector = endpoint + ((component != null) ? (rotation * Vector3.Scale(component.center, lossyScale)) : Vector3.zero);
				bounds.Expand(new Vector3(2f * (num + 0.08f), 0f, 2f * (num + 0.08f)));
				Vector3 min = bounds.min;
				Vector3 max = bounds.max;
				if (vector.x < min.x || vector.x > max.x || vector.z < min.z || vector.z > max.z)
				{
					return endpoint;
				}
				Vector3 normalized = Vector3.ProjectOnPlane(rotation * Vector3.forward, Vector3.up).normalized;
				if (normalized.sqrMagnitude >= 0.001f)
				{
					float num2 = ((Mathf.Abs(normalized.x) > 0.0001f) ? ((((normalized.x > 0f) ? max.x : min.x) - vector.x) / normalized.x) : float.PositiveInfinity);
					float num3 = ((Mathf.Abs(normalized.z) > 0.0001f) ? ((((normalized.z > 0f) ? max.z : min.z) - vector.z) / normalized.z) : float.PositiveInfinity);
					return endpoint + normalized * (Mathf.Min(num2, num3) + 0.001f);
				}
				if (!(this.gameplayExitAnchor != null))
				{
					return endpoint;
				}
				return this.gameplayExitAnchor.position;
			}
		}

		// Token: 0x0600899F RID: 35231 RVA: 0x00384610 File Offset: 0x00382810
		private void BindMattressHairCollider()
		{
			if (this.mattressHairCollider == null || this._playerInput == null)
			{
				return;
			}
			Transform root = this._playerInput.transform.root;
			this._hairClothScratch.Clear();
			root.GetComponentsInChildren<MagicaCloth>(true, this._hairClothScratch);
			for (int i = 0; i < this._hairClothScratch.Count; i++)
			{
				MagicaCloth magicaCloth = this._hairClothScratch[i];
				if (!(magicaCloth == null) && this.IsBelowRuntimeHairRoot(magicaCloth.transform, root) && magicaCloth.SerializeData != null && magicaCloth.SerializeData.colliderCollisionConstraint != null)
				{
					List<ColliderComponent> list = magicaCloth.SerializeData.colliderCollisionConstraint.colliderList;
					if (list == null)
					{
						list = new List<ColliderComponent>();
						magicaCloth.SerializeData.colliderCollisionConstraint.colliderList = list;
					}
					if (this._boundHairCloths.Add(magicaCloth))
					{
						ColliderCollisionConstraint.SerializeData colliderCollisionConstraint = magicaCloth.SerializeData.colliderCollisionConstraint;
						bool flag = list.Contains(this.mattressHairCollider);
						this._hairCollisionRestoreStates[magicaCloth] = new CielHomeIntroSequence.HairCollisionRestoreState
						{
							Mode = colliderCollisionConstraint.mode,
							Friction = colliderCollisionConstraint.friction,
							MattressColliderWasPresent = flag
						};
						bool flag2 = !flag || colliderCollisionConstraint.mode != ColliderCollisionConstraint.Mode.Edge || !Mathf.Approximately(colliderCollisionConstraint.friction, 0f);
						if (!flag)
						{
							list.Add(this.mattressHairCollider);
						}
						colliderCollisionConstraint.mode = ColliderCollisionConstraint.Mode.Edge;
						colliderCollisionConstraint.friction = 0f;
						if (flag2 && magicaCloth.isActiveAndEnabled)
						{
							magicaCloth.SetParameterChange();
							magicaCloth.ResetCloth(false);
						}
					}
				}
			}
			this._hairClothScratch.Clear();
			if (this.debugLogs)
			{
				Debug.Log("[CielHomeIntro] Mattress collider bound to " + string.Format("{0} hair cloth(s).", this._boundHairCloths.Count), this);
			}
		}

		// Token: 0x060089A0 RID: 35232 RVA: 0x003847F4 File Offset: 0x003829F4
		private void UnbindMattressHairCollider()
		{
			if (this._boundHairCloths.Count == 0)
			{
				return;
			}
			foreach (MagicaCloth magicaCloth in this._boundHairCloths)
			{
				if (!(magicaCloth == null) && magicaCloth.SerializeData != null && magicaCloth.SerializeData.colliderCollisionConstraint != null)
				{
					ColliderCollisionConstraint.SerializeData colliderCollisionConstraint = magicaCloth.SerializeData.colliderCollisionConstraint;
					CielHomeIntroSequence.HairCollisionRestoreState hairCollisionRestoreState;
					bool flag = this._hairCollisionRestoreStates.TryGetValue(magicaCloth, out hairCollisionRestoreState);
					List<ColliderComponent> colliderList = colliderCollisionConstraint.colliderList;
					bool flag2 = false;
					if (flag)
					{
						if (!hairCollisionRestoreState.MattressColliderWasPresent && colliderList != null && this.mattressHairCollider != null)
						{
							flag2 |= colliderList.Remove(this.mattressHairCollider);
						}
						flag2 |= colliderCollisionConstraint.mode != hairCollisionRestoreState.Mode || !Mathf.Approximately(colliderCollisionConstraint.friction, hairCollisionRestoreState.Friction);
						colliderCollisionConstraint.mode = hairCollisionRestoreState.Mode;
						colliderCollisionConstraint.friction = hairCollisionRestoreState.Friction;
					}
					else if (colliderList != null && this.mattressHairCollider != null)
					{
						flag2 = colliderList.Remove(this.mattressHairCollider);
					}
					if (flag2 && magicaCloth.isActiveAndEnabled)
					{
						magicaCloth.SetParameterChange();
						magicaCloth.ResetCloth(true);
					}
				}
			}
			this._boundHairCloths.Clear();
			this._hairCollisionRestoreStates.Clear();
		}

		// Token: 0x060089A1 RID: 35233 RVA: 0x00384970 File Offset: 0x00382B70
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

		// Token: 0x060089A2 RID: 35234 RVA: 0x003849AC File Offset: 0x00382BAC
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

		// Token: 0x060089A3 RID: 35235 RVA: 0x00384A40 File Offset: 0x00382C40
		private IEnumerator RestoreGameplayCamera()
		{
			if (this._renderCamera != null)
			{
				this._renderCamera.fieldOfView = this._previousFieldOfView;
			}
			if (this._thirdPersonCamera != null)
			{
				this._thirdPersonCamera.RemoveLockTarget();
				if (this._previousCameraFreeze)
				{
					this.RestoreCapturedCameraPose();
					this._thirdPersonCamera.isFreezed = true;
				}
				else
				{
					this._thirdPersonCamera.isFreezed = false;
					this._thirdPersonCamera.ResetTarget();
					this._thirdPersonCamera.ResetAngleWithoutSmooth();
					if (!this._thirdPersonCamera.SnapToCurrentStateWithoutSmooth(true))
					{
						Debug.LogWarning("[CielHomeIntro] Gameplay camera was not ready for the hand-off snap.", this);
					}
					Physics.SyncTransforms();
					yield return new WaitForFixedUpdate();
					if (this._thirdPersonCamera != null && !this._thirdPersonCamera.isFreezed)
					{
						this._thirdPersonCamera.SnapToCurrentStateWithoutSmooth(true);
						Physics.SyncTransforms();
					}
					yield return new WaitForEndOfFrame();
				}
			}
			this._cameraCaptured = false;
			yield break;
		}

		// Token: 0x060089A4 RID: 35236 RVA: 0x00384A50 File Offset: 0x00382C50
		private void RestoreCapturedCameraPose()
		{
			if (!this._cameraCaptured || this._thirdPersonCamera == null)
			{
				return;
			}
			Rigidbody selfRigidbody = this._thirdPersonCamera.selfRigidbody;
			selfRigidbody.position = this._previousCameraPosition;
			selfRigidbody.rotation = this._previousCameraRotation;
			this._thirdPersonCamera.transform.SetPositionAndRotation(this._previousCameraPosition, this._previousCameraRotation);
			Physics.SyncTransforms();
		}

		// Token: 0x060089A5 RID: 35237 RVA: 0x00384AB8 File Offset: 0x00382CB8
		private void RestoreControlledState(bool resetGameplayCamera)
		{
			if (this._stateRestored)
			{
				return;
			}
			this._stateRestored = true;
			this.FinishWakeAction();
			this.UnbindMattressHairCollider();
			this.ReleaseCinematicPresentation();
			if (resetGameplayCamera)
			{
				if (this._renderCamera != null)
				{
					this._renderCamera.fieldOfView = this._previousFieldOfView;
				}
				if (this._cameraCaptured && this._thirdPersonCamera != null)
				{
					this.RestoreCapturedCameraPose();
					this._thirdPersonCamera.isFreezed = this._previousCameraFreeze;
				}
			}
			this._cameraCaptured = false;
			this._cameraFreezeStateCaptured = false;
			if (this._coverPose != null)
			{
				this._coverPose.enabled = this._coverPoseWasEnabled;
			}
			if (this._inputCaptured && this._playerInput != null)
			{
				this._playerInput.SetLockAllInput(this._previousInputLock);
				this._playerInput.SetLockCameraInput(this._previousCameraInputLock);
			}
			this._inputCaptured = false;
			PlayerActivityLease activityLease = this._activityLease;
			if (activityLease != null)
			{
				activityLease.Dispose();
			}
			this._activityLease = null;
			if (this._hudHidden && HUDManager.Instance != null)
			{
				HUDManager.Instance.ShowHUD();
			}
			this._hudHidden = false;
		}

		// Token: 0x060089A6 RID: 35238 RVA: 0x00384BE0 File Offset: 0x00382DE0
		private IEnumerator RevealFallback(ScreenFadeController fade)
		{
			bool complete = false;
			fade.FadeIn(this.gameplayFadeDuration, delegate
			{
				complete = true;
			});
			float deadline = Time.realtimeSinceStartup + this.gameplayFadeDuration + 2f;
			while (!complete && Time.realtimeSinceStartup < deadline)
			{
				yield return null;
			}
			this.RestoreControlledState(true);
			this._destinationRevealPending = false;
			this._running = false;
			this.HasCompleted = true;
			yield break;
		}

		// Token: 0x060089A7 RID: 35239 RVA: 0x00384BF8 File Offset: 0x00382DF8
		private void AbandonState()
		{
			WearAdmireAnimation.SetPresentationSuppressed(this, false);
			this._boundHairCloths.Clear();
			this._cinematicAnimatorLayersSuppressed = false;
			this._animatorCullingCaptured = false;
			this._savedAnimatorLayerWeights = null;
			this._ownsFacialExpressionOverride = false;
			this._emotionController = null;
			this._destinationRevealPending = false;
			this._running = false;
			this._actionStarted = false;
			this._inputCaptured = false;
			this._cameraCaptured = false;
			this._hudHidden = false;
			this._activityLease = null;
		}

		// Token: 0x060089A8 RID: 35240 RVA: 0x001800C0 File Offset: 0x0017E2C0
		private static bool IsRuntimeTearingDown()
		{
			return !Application.isPlaying;
		}

		// Token: 0x060089A9 RID: 35241 RVA: 0x00384C6B File Offset: 0x00382E6B
		public static CielHomeIntroSequence.WakeVariant SelectWakeVariant(bool randomize, CielHomeIntroSequence.WakeVariant fixedVariant, int randomValue)
		{
			if (!randomize)
			{
				return fixedVariant;
			}
			if ((randomValue & 1) != 0)
			{
				return CielHomeIntroSequence.WakeVariant.Right;
			}
			return CielHomeIntroSequence.WakeVariant.Left;
		}

		// Token: 0x060089AA RID: 35242 RVA: 0x00384C7A File Offset: 0x00382E7A
		public static Vector3 EvaluateThreeBeatPath(Vector3 start, Vector3 middle, Vector3 end, float t)
		{
			t = Mathf.Clamp01(t);
			if (t <= 0.5f)
			{
				return Vector3.Lerp(start, middle, CielHomeIntroSequence.SmoothStep(t * 2f));
			}
			return Vector3.Lerp(middle, end, CielHomeIntroSequence.SmoothStep((t - 0.5f) * 2f));
		}

		// Token: 0x060089AB RID: 35243 RVA: 0x001811C8 File Offset: 0x0017F3C8
		private static float SmoothStep(float t)
		{
			t = Mathf.Clamp01(t);
			return t * t * (3f - 2f * t);
		}

		// Token: 0x04008C0E RID: 35854
		public const float WakeSoundAdvanceSeconds = 1f;

		// Token: 0x04008C0F RID: 35855
		public const float DefaultWakeSoundNormalizedTime = 0.4977778f;

		// Token: 0x04008C10 RID: 35856
		[Header("Wake-up placement")]
		[SerializeField]
		private Transform bedReference;

		// Token: 0x04008C11 RID: 35857
		[SerializeField]
		private Transform wakeAnchorLeft;

		// Token: 0x04008C12 RID: 35858
		[SerializeField]
		private Transform wakeAnchorRight;

		// Token: 0x04008C13 RID: 35859
		[SerializeField]
		private Transform gameplayExitAnchor;

		// Token: 0x04008C14 RID: 35860
		[SerializeField]
		[Min(0f)]
		private float wakePoseHeightAboveBedOrigin = 0.61f;

		// Token: 0x04008C15 RID: 35861
		[SerializeField]
		private float wakePoseLongitudinalOffset = 0.22f;

		// Token: 0x04008C16 RID: 35862
		[SerializeField]
		private float wakePoseLateralOffset = -0.3f;

		// Token: 0x04008C17 RID: 35863
		[Header("Mattress hair collision (Magica Cloth 2)")]
		[SerializeField]
		private ColliderComponent mattressHairCollider;

		// Token: 0x04008C18 RID: 35864
		[SerializeField]
		private string runtimeHairRootName = "Hair";

		// Token: 0x04008C19 RID: 35865
		[Header("Invector actions")]
		[SerializeField]
		private vTriggerGenericAction wakeLeftAction;

		// Token: 0x04008C1A RID: 35866
		[SerializeField]
		private vTriggerGenericAction wakeRightAction;

		// Token: 0x04008C1B RID: 35867
		[SerializeField]
		private string wakeLeftState = "CielHomeWakeUpLeft";

		// Token: 0x04008C1C RID: 35868
		[SerializeField]
		private string wakeRightState = "CielHomeWakeUpRight";

		// Token: 0x04008C1D RID: 35869
		[SerializeField]
		[Min(1f)]
		private float actionTimeout = 9f;

		// Token: 0x04008C1E RID: 35870
		[SerializeField]
		[Range(0.5f, 1f)]
		private float fadeOutNormalizedTime = 0.86f;

		// Token: 0x04008C1F RID: 35871
		[SerializeField]
		[Range(0.7f, 1f)]
		private float finishNormalizedTime = 0.995f;

		// Token: 0x04008C20 RID: 35872
		[Header("Wake-up audio sync")]
		[SerializeField]
		private CielHomeAudioController homeAudio;

		// Token: 0x04008C21 RID: 35873
		[Tooltip("One real-time second before 3:20 in the 30 fps, six-second source animation played at 0.68 speed (approximately 3:00 source time).")]
		[SerializeField]
		[Range(0f, 1f)]
		private float wakeSoundNormalizedTime = 0.4977778f;

		// Token: 0x04008C22 RID: 35874
		[Header("Cinematic camera")]
		[SerializeField]
		private Transform[] cameraPathLeft;

		// Token: 0x04008C23 RID: 35875
		[SerializeField]
		private Transform[] cameraPathRight;

		// Token: 0x04008C24 RID: 35876
		[SerializeField]
		[Min(0.1f)]
		private float cameraTravelDuration = 4.9f;

		// Token: 0x04008C25 RID: 35877
		[SerializeField]
		private AnimationCurve cameraTravelCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		// Token: 0x04008C26 RID: 35878
		[SerializeField]
		[Min(20f)]
		private float openingFieldOfView = 43f;

		// Token: 0x04008C27 RID: 35879
		[SerializeField]
		[Min(20f)]
		private float endingFieldOfView = 54f;

		// Token: 0x04008C28 RID: 35880
		[SerializeField]
		[Min(0.05f)]
		private float cameraCollisionRadius = 0.18f;

		// Token: 0x04008C29 RID: 35881
		[SerializeField]
		[Min(0f)]
		private float cameraCollisionPadding = 0.1f;

		// Token: 0x04008C2A RID: 35882
		[SerializeField]
		[Min(0.2f)]
		private float minimumCameraDistance = 0.7f;

		// Token: 0x04008C2B RID: 35883
		[SerializeField]
		[Min(0f)]
		private float fallbackLookHeight = 1.05f;

		// Token: 0x04008C2C RID: 35884
		[Header("Fade choreography")]
		[SerializeField]
		[Min(0f)]
		private float settleDelay = 0.08f;

		// Token: 0x04008C2D RID: 35885
		[SerializeField]
		[Min(0f)]
		private float openingFadeDuration = 1.15f;

		// Token: 0x04008C2E RID: 35886
		[SerializeField]
		[Min(0f)]
		private float handoffFadeDuration = 0.45f;

		// Token: 0x04008C2F RID: 35887
		[SerializeField]
		[Min(0f)]
		private float blackHandoffHold = 0.12f;

		// Token: 0x04008C30 RID: 35888
		[SerializeField]
		[Min(0f)]
		private float gameplayFadeDuration = 0.72f;

		// Token: 0x04008C31 RID: 35889
		[Header("Presentation")]
		[SerializeField]
		private bool randomizeWakeVariant = true;

		// Token: 0x04008C32 RID: 35890
		[SerializeField]
		private CielHomeIntroSequence.WakeVariant fixedWakeVariant;

		// Token: 0x04008C33 RID: 35891
		[SerializeField]
		private bool hideHud = true;

		// Token: 0x04008C34 RID: 35892
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04008C35 RID: 35893
		private PlayerActivityLease _activityLease;

		// Token: 0x04008C36 RID: 35894
		private vThirdPersonInput _playerInput;

		// Token: 0x04008C37 RID: 35895
		private PlayerActionAnimator _actionAnimator;

		// Token: 0x04008C38 RID: 35896
		private Animator _animator;

		// Token: 0x04008C39 RID: 35897
		private Rigidbody _playerRigidbody;

		// Token: 0x04008C3A RID: 35898
		private CoverPoseController _coverPose;

		// Token: 0x04008C3B RID: 35899
		private bool _coverPoseWasEnabled;

		// Token: 0x04008C3C RID: 35900
		private bool _inputCaptured;

		// Token: 0x04008C3D RID: 35901
		private bool _previousInputLock;

		// Token: 0x04008C3E RID: 35902
		private bool _previousCameraInputLock;

		// Token: 0x04008C3F RID: 35903
		private bool _hudHidden;

		// Token: 0x04008C40 RID: 35904
		private vThirdPersonCamera _thirdPersonCamera;

		// Token: 0x04008C41 RID: 35905
		private bool _cameraCaptured;

		// Token: 0x04008C42 RID: 35906
		private bool _cameraFreezeStateCaptured;

		// Token: 0x04008C43 RID: 35907
		private bool _previousCameraFreeze;

		// Token: 0x04008C44 RID: 35908
		private Vector3 _previousCameraPosition;

		// Token: 0x04008C45 RID: 35909
		private Quaternion _previousCameraRotation;

		// Token: 0x04008C46 RID: 35910
		private Camera _renderCamera;

		// Token: 0x04008C47 RID: 35911
		private float _previousFieldOfView;

		// Token: 0x04008C48 RID: 35912
		private bool _hasStarted;

		// Token: 0x04008C49 RID: 35913
		private bool _destinationRevealPending;

		// Token: 0x04008C4A RID: 35914
		private bool _running;

		// Token: 0x04008C4B RID: 35915
		private bool _actionStarted;

		// Token: 0x04008C4C RID: 35916
		private bool _stateRestored;

		// Token: 0x04008C4D RID: 35917
		private float[] _savedAnimatorLayerWeights;

		// Token: 0x04008C4E RID: 35918
		private bool _cinematicAnimatorLayersSuppressed;

		// Token: 0x04008C4F RID: 35919
		private AnimatorCullingMode _previousAnimatorCullingMode;

		// Token: 0x04008C50 RID: 35920
		private bool _animatorCullingCaptured;

		// Token: 0x04008C51 RID: 35921
		private PlayerEmotionController _emotionController;

		// Token: 0x04008C52 RID: 35922
		private bool _ownsFacialExpressionOverride;

		// Token: 0x04008C53 RID: 35923
		private readonly HashSet<MagicaCloth> _boundHairCloths = new HashSet<MagicaCloth>();

		// Token: 0x04008C54 RID: 35924
		private readonly Dictionary<MagicaCloth, CielHomeIntroSequence.HairCollisionRestoreState> _hairCollisionRestoreStates = new Dictionary<MagicaCloth, CielHomeIntroSequence.HairCollisionRestoreState>();

		// Token: 0x04008C55 RID: 35925
		private readonly List<MagicaCloth> _hairClothScratch = new List<MagicaCloth>();

		// Token: 0x04008C56 RID: 35926
		private static CielHomeIntroSequence _activePresentation;

		// Token: 0x0200118A RID: 4490
		public enum WakeVariant
		{
			// Token: 0x04008C5A RID: 35930
			Left,
			// Token: 0x04008C5B RID: 35931
			Right
		}

		// Token: 0x0200118B RID: 4491
		private struct HairCollisionRestoreState
		{
			// Token: 0x04008C5C RID: 35932
			public ColliderCollisionConstraint.Mode Mode;

			// Token: 0x04008C5D RID: 35933
			public float Friction;

			// Token: 0x04008C5E RID: 35934
			public bool MattressColliderWasPresent;
		}
	}
}
