using System;
using System.Collections;
using System.Collections.Generic;
using DA_Assets.DAG;
using Features.Audio;
using Features.CielVoice;
using Features.Detection;
using Features.Loading;
using Features.Localization;
using Features.MainMenu;
using Features.RunModes;
using Invector.vCamera;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectExhibitionist.PoiTravel
{
	// Token: 0x02000949 RID: 2377
	[DisallowMultipleComponent]
	public sealed class PoiArrivalPresentation : MonoBehaviour
	{
		// Token: 0x17000AF0 RID: 2800
		// (get) Token: 0x060040A5 RID: 16549 RVA: 0x0017FA50 File Offset: 0x0017DC50
		public static bool IsPresentingInActiveScene
		{
			get
			{
				return PoiArrivalPresentation.IsPresentingInScene(SceneManager.GetActiveScene());
			}
		}

		// Token: 0x060040A6 RID: 16550 RVA: 0x0017FA5C File Offset: 0x0017DC5C
		public static bool IsPresentingInScene(Scene scene)
		{
			foreach (PoiArrivalPresentation poiArrivalPresentation in PoiArrivalPresentation.EnabledPresentations)
			{
				if (poiArrivalPresentation != null && poiArrivalPresentation.gameObject.scene == scene && poiArrivalPresentation.OwnsDestinationReveal)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x17000AF1 RID: 2801
		// (get) Token: 0x060040A7 RID: 16551 RVA: 0x0017FAD4 File Offset: 0x0017DCD4
		public bool OwnsDestinationReveal
		{
			get
			{
				return base.isActiveAndEnabled && (this._destinationRevealPending || this._presentationActive);
			}
		}

		// Token: 0x17000AF2 RID: 2802
		// (get) Token: 0x060040A8 RID: 16552 RVA: 0x0017FAF0 File Offset: 0x0017DCF0
		private string DetectionPauseReason
		{
			get
			{
				return "PoiArrival#" + base.GetInstanceID().ToString();
			}
		}

		// Token: 0x17000AF3 RID: 2803
		// (get) Token: 0x060040A9 RID: 16553 RVA: 0x0017FB18 File Offset: 0x0017DD18
		private string ActivityReason
		{
			get
			{
				return "PoiArrival.Cinematic#" + base.GetInstanceID().ToString();
			}
		}

		// Token: 0x060040AA RID: 16554 RVA: 0x0017FB3D File Offset: 0x0017DD3D
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetVoiceHistory()
		{
			PoiArrivalPresentation.LastVoiceByZone.Clear();
			PoiArrivalPresentation.LastTrafficPresetByZone.Clear();
			PoiArrivalPresentation.EnabledPresentations.Clear();
		}

		// Token: 0x060040AB RID: 16555 RVA: 0x0017FB5D File Offset: 0x0017DD5D
		private void Awake()
		{
			this.BuildTitleUI();
			if (!this._titleOnly)
			{
				this.BuildPostProcessing();
			}
		}

		// Token: 0x060040AC RID: 16556 RVA: 0x0017FB73 File Offset: 0x0017DD73
		private void OnEnable()
		{
			if (this._titleOnly)
			{
				return;
			}
			if (this.IsClaimedByLongWayHomePrologue())
			{
				return;
			}
			PoiArrivalPresentation.EnabledPresentations.Add(this);
			if (!this._hasStarted)
			{
				this._destinationRevealPending = true;
			}
		}

		// Token: 0x060040AD RID: 16557 RVA: 0x0017FBA4 File Offset: 0x0017DDA4
		private void Start()
		{
			if (!this._titleOnly && !this._hasStarted && this.IsClaimedByLongWayHomePrologue())
			{
				this._hasStarted = true;
				this._destinationRevealPending = false;
				base.enabled = false;
				return;
			}
			if (!this._titleOnly && !this._hasStarted)
			{
				base.StartCoroutine(this.PresentationRoutine());
			}
		}

		// Token: 0x060040AE RID: 16558 RVA: 0x0017FBFC File Offset: 0x0017DDFC
		private bool IsClaimedByLongWayHomePrologue()
		{
			return LongWayHomeRuntimeController.IsAwaitingProloguePoi(base.gameObject.scene.name);
		}

		// Token: 0x060040AF RID: 16559 RVA: 0x0017FC21 File Offset: 0x0017DE21
		private void Update()
		{
			if (this._presentationActive)
			{
				EscapeInputCoordinator.TryConsume(this);
			}
		}

		// Token: 0x060040B0 RID: 16560 RVA: 0x0017FC34 File Offset: 0x0017DE34
		private void OnDisable()
		{
			WeatherAudioShelterRuntime.SetArrivalVoiceDucking(this, false);
			if (this._titleOnly)
			{
				base.StopAllCoroutines();
				if (this._titleGroup != null)
				{
					this._titleGroup.gameObject.SetActive(false);
				}
				return;
			}
			PoiArrivalPresentation.EnabledPresentations.Remove(this);
			base.StopAllCoroutines();
			this.StopTrafficAudio();
			bool flag = (this._destinationRevealPending || this._presentationActive) && SceneManager.GetActiveScene() == base.gameObject.scene;
			bool flag2 = PoiArrivalPresentation.IsRuntimeTearingDown();
			this._destinationRevealPending = false;
			if (!flag2 && this._arrivalVoice != null && this._arrivalVoiceController != null && this._arrivalVoiceController.CurrentVoice == this._arrivalVoice)
			{
				this._arrivalVoiceController.Stop();
			}
			this._arrivalVoiceController = null;
			this._arrivalSubtitleUi = null;
			this._arrivalVoiceStarted = false;
			this._voicePlayRequestFinished = true;
			if (flag2)
			{
				this.AbandonCinematicState();
			}
			else
			{
				this.RestoreCinematicState();
			}
			this.ReleaseDetectionPause();
			if (flag && !flag2)
			{
				ScreenFadeController instance = ScreenFadeController.Instance;
				if (instance == null)
				{
					return;
				}
				instance.SetClear();
			}
		}

		// Token: 0x060040B1 RID: 16561 RVA: 0x0017FD4C File Offset: 0x0017DF4C
		private void OnDestroy()
		{
			if (this._titleOnly)
			{
				return;
			}
			if (PoiArrivalPresentation.IsRuntimeTearingDown())
			{
				this.AbandonCinematicState();
			}
			else
			{
				this.RestoreCinematicState();
			}
			this.ReleaseDetectionPause();
			if (this._runtimeProfile != null)
			{
				Object.Destroy(this._runtimeProfile);
			}
		}

		// Token: 0x060040B2 RID: 16562 RVA: 0x0017FD8B File Offset: 0x0017DF8B
		private IEnumerator PresentationRoutine()
		{
			this._hasStarted = true;
			this._presentationActive = true;
			while (ScreenFadeController.Instance == null)
			{
				yield return null;
			}
			ScreenFadeController fade = ScreenFadeController.Instance;
			fade.SetBlack();
			this.AcquireDetectionPause();
			while (SceneManager.GetActiveScene() != base.gameObject.scene || !GameSystemsBootstrapper.GameplayReady)
			{
				if (!this._ownsDetectionPause)
				{
					this.AcquireDetectionPause();
				}
				yield return null;
			}
			LoadingScreenController loading = LoadingScreenController.Instance;
			while (loading != null && loading.IsLoadingActive)
			{
				yield return null;
			}
			yield return null;
			CielVoiceSubtitleUI cielVoiceSubtitleUI = CielVoiceSubtitleUI.Instance;
			if (cielVoiceSubtitleUI == null)
			{
				cielVoiceSubtitleUI = Object.FindObjectOfType<CielVoiceSubtitleUI>(true);
			}
			this._uiSuppression.Suppress(new Transform[]
			{
				this._titleCanvas,
				fade.transform,
				(cielVoiceSubtitleUI != null) ? cielVoiceSubtitleUI.transform : null
			});
			if (!this._ownsDetectionPause)
			{
				this.AcquireDetectionPause();
			}
			base.StartCoroutine(this.ReleaseDetectionAfterDelay());
			yield return this.AcquireCinematicControl();
			yield return this.PrepareCinematicCamera();
			Vector3 vector = Vector3.zero;
			if (this._cameraStateCaptured)
			{
				vector = this.ResolveFrontCameraPosition();
				this.ApplyCameraPose(vector, this.GetLookRotation(vector));
			}
			fade.FadeIn(this.revealDuration, null);
			Coroutine titleRoutine = base.StartCoroutine(this.AnimateTitleAndPostFx());
			CielVoiceSO selected = this.PickVoiceLine();
			this._arrivalVoice = selected;
			base.StartCoroutine(this.PlayVoiceAfterDelay(selected));
			if (this._cameraStateCaptured)
			{
				yield return this.HoldFrontShot(vector);
				yield return this.ReturnToGameplayCamera();
			}
			else
			{
				float num = Mathf.Max(this.frontShotDuration + this.cameraReturnDuration, this.revealDuration);
				yield return this.WaitWhileRefreshingUi(num);
			}
			if (titleRoutine != null)
			{
				yield return titleRoutine;
			}
			this.RestoreCameraAndInputState();
			yield return this.WaitForArrivalSubtitleToFinish();
			this._uiSuppression.Restore();
			this._presentationActive = false;
			this._destinationRevealPending = false;
			if (this.enableDebugLogs)
			{
				Debug.Log(string.Concat(new string[]
				{
					"[PoiArrival] Presented '",
					this.zoneDisplayName,
					"' with voice '",
					(selected != null) ? selected.ResolvedId : "<none>",
					"'."
				}), this);
			}
			yield break;
		}

		// Token: 0x060040B3 RID: 16563 RVA: 0x0017FD9A File Offset: 0x0017DF9A
		private IEnumerator AcquireCinematicControl()
		{
			float deadline = Time.realtimeSinceStartup + 1f;
			while (this._activityLease == null && Time.realtimeSinceStartup < deadline)
			{
				PlayerActivityGate.TryAcquire(this.ActivityReason, this, PlayerActivity.None, PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera, PlayerActivity.AllPlayerActions, out this._activityLease, false, null);
				if (this._activityLease == null)
				{
					yield return null;
				}
			}
			Transform player = PlayerRegistry.Player;
			this._playerInput = ((player != null) ? player.GetComponent<vThirdPersonInput>() : null);
			if (this._playerInput == null)
			{
				this._playerInput = Object.FindObjectOfType<vThirdPersonInput>(true);
			}
			if (this._playerInput != null)
			{
				this._previousInputLock = this._playerInput.lockInput;
				this._previousCameraInputLock = this._playerInput.lockCameraInput;
				this._inputStateCaptured = true;
				this._playerInput.SetLockAllInput(true);
				this._playerInput.SetLockCameraInput(true);
			}
			if (this._activityLease == null && this.enableDebugLogs)
			{
				Debug.LogWarning("[PoiArrival] Camera/player activity lease unavailable; using the Invector input lock only.", this);
			}
			yield break;
		}

		// Token: 0x060040B4 RID: 16564 RVA: 0x0017FDA9 File Offset: 0x0017DFA9
		private IEnumerator PrepareCinematicCamera()
		{
			this._thirdPersonCamera = ((this._playerInput != null) ? this._playerInput.tpCamera : null);
			if (this._thirdPersonCamera == null)
			{
				this._thirdPersonCamera = Object.FindObjectOfType<vThirdPersonCamera>(true);
			}
			if (this._thirdPersonCamera == null || this._thirdPersonCamera.mainTarget == null)
			{
				yield break;
			}
			this._cameraTarget = this._thirdPersonCamera.mainTarget;
			this._thirdPersonCamera.ResetAngleWithoutSmooth();
			if (this.overrideGameplayCameraOrbit)
			{
				this._thirdPersonCamera.mouseX = this.gameplayCameraOrbit.x;
				this._thirdPersonCamera.mouseY = this.gameplayCameraOrbit.y;
				this._thirdPersonCamera.SetZoomWithoutSmooth(this.gameplayCameraDistance);
			}
			yield return null;
			this._previousCameraFreeze = this._thirdPersonCamera.isFreezed;
			this._gameplayCameraMouseX = this._thirdPersonCamera.mouseX;
			this._gameplayCameraMouseY = this._thirdPersonCamera.mouseY;
			this._gameplayCameraPosition = this._thirdPersonCamera.transform.position;
			this._gameplayCameraRotation = this._thirdPersonCamera.transform.rotation;
			this._cameraStateCaptured = true;
			if (!this._previousCameraFreeze)
			{
				this._thirdPersonCamera.FreezeCamera();
				this._cameraFrozenByPresentation = true;
			}
			yield break;
		}

		// Token: 0x060040B5 RID: 16565 RVA: 0x0017FDB8 File Offset: 0x0017DFB8
		private IEnumerator HoldFrontShot(Vector3 startPosition)
		{
			if (this.frontShotDuration <= 0f)
			{
				yield break;
			}
			float elapsed = 0f;
			while (elapsed < this.frontShotDuration && this._thirdPersonCamera != null && this._cameraTarget != null)
			{
				elapsed += Time.unscaledDeltaTime;
				float num = PoiArrivalPresentation.SmoothStep(Mathf.Clamp01(elapsed / this.frontShotDuration));
				Vector3 cameraPivot = this.GetCameraPivot();
				Vector3 normalized = (cameraPivot - startPosition).normalized;
				Vector3 vector = startPosition + normalized * (this.frontPushIn * num);
				Vector3 vector2 = this.ResolveCameraCollision(cameraPivot, vector);
				if (!this.HasMinimumCameraDistance(cameraPivot, vector2))
				{
					vector2 = startPosition;
				}
				this.ApplyCameraPose(vector2, this.GetLookRotation(vector2));
				this._uiSuppression.Refresh();
				yield return null;
			}
			yield break;
		}

		// Token: 0x060040B6 RID: 16566 RVA: 0x0017FDCE File Offset: 0x0017DFCE
		private IEnumerator ReturnToGameplayCamera()
		{
			if (this._thirdPersonCamera == null || this._cameraTarget == null)
			{
				yield break;
			}
			Vector3 vector = this.GetCameraPivot();
			Vector3 startOffset = this._thirdPersonCamera.transform.position - vector;
			Vector3 targetOffset = this._gameplayCameraPosition - vector;
			Vector3 startHorizontal = Vector3.ProjectOnPlane(startOffset, Vector3.up);
			Vector3 targetHorizontal = Vector3.ProjectOnPlane(targetOffset, Vector3.up);
			if (startHorizontal.sqrMagnitude < 0.001f || targetHorizontal.sqrMagnitude < 0.001f)
			{
				startHorizontal = Vector3.forward;
				targetHorizontal = Vector3.back;
			}
			float signedAngle = Vector3.SignedAngle(startHorizontal, targetHorizontal, Vector3.up);
			if (Mathf.Abs(signedAngle) > 175f)
			{
				signedAngle = (this.orbitClockwise ? 180f : (-180f));
			}
			float effectiveReturnDuration = this.ResolveCameraReturnDuration();
			float elapsed = 0f;
			while (elapsed < effectiveReturnDuration && this._thirdPersonCamera != null && this._cameraTarget != null)
			{
				elapsed += Time.unscaledDeltaTime;
				float num = PoiArrivalPresentation.SmootherStep(Mathf.Clamp01(elapsed / effectiveReturnDuration));
				vector = this.GetCameraPivot();
				Vector3 vector2 = Quaternion.AngleAxis(signedAngle * num, Vector3.up) * startHorizontal.normalized;
				float num2 = Mathf.Lerp(startHorizontal.magnitude, targetHorizontal.magnitude, num);
				float num3 = Mathf.Lerp(startOffset.y, targetOffset.y, num) + Mathf.Sin(3.1415927f * num) * this.orbitLift;
				Vector3 vector3 = vector + vector2 * num2 + Vector3.up * num3;
				Vector3 vector4 = this.ResolveCameraCollision(vector, vector3);
				if (!this.HasMinimumCameraDistance(vector, vector4))
				{
					vector4 = this._thirdPersonCamera.transform.position;
				}
				Quaternion quaternion = Quaternion.Slerp(this.GetLookRotation(vector4), this._gameplayCameraRotation, PoiArrivalPresentation.SmoothStep(Mathf.InverseLerp(0.65f, 1f, num)));
				this.ApplyCameraPose(vector4, quaternion);
				this._uiSuppression.Refresh();
				yield return null;
			}
			this.ApplyCameraPose(this._gameplayCameraPosition, this._gameplayCameraRotation);
			yield break;
		}

		// Token: 0x060040B7 RID: 16567 RVA: 0x0017FDDD File Offset: 0x0017DFDD
		private IEnumerator PlayVoiceAfterDelay(CielVoiceSO selected)
		{
			if (selected == null || !this._presentationActive || !base.isActiveAndEnabled)
			{
				this._voicePlayRequestFinished = true;
				yield break;
			}
			for (int i = 0; i < selected.Clips.Length; i++)
			{
				AudioClip audioClip = selected.Clips[i];
				if (audioClip != null && audioClip.loadState == AudioDataLoadState.Unloaded)
				{
					audioClip.LoadAudioData();
				}
			}
			if (this.voiceDelay > 0f)
			{
				yield return new WaitForSecondsRealtime(this.voiceDelay);
			}
			float voiceControllerDeadline = Time.realtimeSinceStartup + 3f;
			while (this._presentationActive && base.isActiveAndEnabled && Time.realtimeSinceStartup < voiceControllerDeadline)
			{
				CielVoiceController controller = CielVoiceController.Instance;
				if (controller != null && controller.IsReadyToPlay && PoiArrivalPresentation.IsVoiceAudioLoaded(selected) && controller.PlayWithSubtitleSyncedToAudio(selected, 0.75f))
				{
					this._arrivalVoiceController = controller;
					WeatherAudioShelterRuntime.SetArrivalVoiceDucking(this, true);
					yield return null;
					if (!this._presentationActive || !base.isActiveAndEnabled)
					{
						yield break;
					}
					if (controller != null && controller.CurrentVoice == selected && controller.IsSpeaking)
					{
						this._arrivalVoiceStarted = true;
						this._arrivalSubtitleUi = controller.ActiveSubtitleUI;
						if (this._arrivalSubtitleUi != null)
						{
							this._uiSuppression.AddProtectedRoot(this._arrivalSubtitleUi.transform);
						}
						base.StartCoroutine(this.DuckWeatherUntilVoiceEnds(controller, selected));
						break;
					}
					WeatherAudioShelterRuntime.SetArrivalVoiceDucking(this, false);
				}
				yield return null;
				controller = null;
			}
			if (!this._arrivalVoiceStarted && this._presentationActive && base.isActiveAndEnabled)
			{
				Debug.LogWarning(string.Concat(new string[]
				{
					"[PoiArrival] Could not start '",
					selected.ResolvedId,
					"' in '",
					base.gameObject.scene.name,
					"' after waiting for the active voice source/audio."
				}), this);
			}
			this._voicePlayRequestFinished = true;
			yield break;
		}

		// Token: 0x060040B8 RID: 16568 RVA: 0x0017FDF3 File Offset: 0x0017DFF3
		private IEnumerator DuckWeatherUntilVoiceEnds(CielVoiceController controller, CielVoiceSO voice)
		{
			try
			{
				while (controller != null && controller.CurrentVoice == voice && controller.IsSpeaking)
				{
					yield return null;
				}
			}
			finally
			{
				WeatherAudioShelterRuntime.SetArrivalVoiceDucking(this, false);
			}
			yield break;
			yield break;
		}

		// Token: 0x060040B9 RID: 16569 RVA: 0x0017FE10 File Offset: 0x0017E010
		private static bool IsVoiceAudioLoaded(CielVoiceSO voice)
		{
			if (voice == null || !voice.HasClips)
			{
				return false;
			}
			for (int i = 0; i < voice.Clips.Length; i++)
			{
				if (voice.Clips[i] != null && voice.Clips[i].loadState != AudioDataLoadState.Loaded)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060040BA RID: 16570 RVA: 0x0017FE68 File Offset: 0x0017E068
		private float ResolveCameraReturnDuration()
		{
			float num = 0f;
			if (this._arrivalVoiceStarted && this._arrivalVoiceController != null)
			{
				AudioSource voiceSource = this._arrivalVoiceController.VoiceSource;
				if (voiceSource != null && voiceSource.clip != null && voiceSource.isPlaying)
				{
					num = Mathf.Max(0f, voiceSource.clip.length - voiceSource.time) / Mathf.Max(0.01f, Mathf.Abs(voiceSource.pitch));
				}
			}
			if (num <= 0f)
			{
				float num2 = PoiArrivalPresentation.EstimateVoiceDuration(this._arrivalVoice);
				float num3 = Mathf.Max(0f, this.frontShotDuration - this.voiceDelay);
				num = Mathf.Max(0f, num2 - num3);
			}
			return Mathf.Max(this.cameraReturnDuration, num - this.cameraFinishBeforeVoiceEnd);
		}

		// Token: 0x060040BB RID: 16571 RVA: 0x0017FF3C File Offset: 0x0017E13C
		private static float EstimateVoiceDuration(CielVoiceSO voice)
		{
			if (voice == null || voice.Clips == null)
			{
				return 0f;
			}
			float num = 0f;
			for (int i = 0; i < voice.Clips.Length; i++)
			{
				if (voice.Clips[i] != null)
				{
					num = Mathf.Max(num, voice.Clips[i].length);
				}
			}
			return num / Mathf.Max(0.01f, Mathf.Abs(voice.Pitch));
		}

		// Token: 0x060040BC RID: 16572 RVA: 0x0017FFB4 File Offset: 0x0017E1B4
		private IEnumerator WaitForArrivalSubtitleToFinish()
		{
			float requestDeadline = Time.realtimeSinceStartup + this.voiceDelay + 4f;
			while (!this._voicePlayRequestFinished && Time.realtimeSinceStartup < requestDeadline)
			{
				this._uiSuppression.Refresh();
				yield return null;
			}
			if (!this._arrivalVoiceStarted)
			{
				yield break;
			}
			float playbackDeadline = Time.realtimeSinceStartup + Mathf.Max(4f, PoiArrivalPresentation.EstimateVoiceDuration(this._arrivalVoice) + 3f);
			if (this._arrivalSubtitleUi != null && this._arrivalSubtitleUi.IsVisible)
			{
				while (this._arrivalSubtitleUi != null && this._arrivalSubtitleUi.IsVisible)
				{
					if (Time.realtimeSinceStartup >= playbackDeadline)
					{
						break;
					}
					this._uiSuppression.Refresh();
					yield return null;
				}
			}
			else
			{
				while (this._arrivalVoiceController != null && this._arrivalVoiceController.CurrentVoice == this._arrivalVoice && this._arrivalVoiceController.IsSpeaking && Time.realtimeSinceStartup < playbackDeadline)
				{
					this._uiSuppression.Refresh();
					yield return null;
				}
			}
			yield break;
		}

		// Token: 0x060040BD RID: 16573 RVA: 0x0017FFC4 File Offset: 0x0017E1C4
		private void AcquireDetectionPause()
		{
			if (this._ownsDetectionPause)
			{
				return;
			}
			this._pausedSensoryManager = ((SensoryManager.Instance != null) ? SensoryManager.Instance : Object.FindObjectOfType<SensoryManager>(true));
			if (this._pausedSensoryManager == null)
			{
				return;
			}
			this._pausedSensoryManager.PushPause(this.DetectionPauseReason);
			this._ownsDetectionPause = true;
		}

		// Token: 0x060040BE RID: 16574 RVA: 0x00180021 File Offset: 0x0017E221
		private IEnumerator ReleaseDetectionAfterDelay()
		{
			if (!this._ownsDetectionPause)
			{
				yield break;
			}
			float deadline = Time.realtimeSinceStartup + this.detectionEnableDelay;
			while (Time.realtimeSinceStartup < deadline || this._uiSuppression.IsActive)
			{
				yield return null;
			}
			this.ReleaseDetectionPause();
			yield break;
		}

		// Token: 0x060040BF RID: 16575 RVA: 0x00180030 File Offset: 0x0017E230
		private void ReleaseDetectionPause()
		{
			if (!this._ownsDetectionPause)
			{
				return;
			}
			if (this._pausedSensoryManager != null)
			{
				this._pausedSensoryManager.PopPause(this.DetectionPauseReason);
			}
			this._pausedSensoryManager = null;
			this._ownsDetectionPause = false;
		}

		// Token: 0x060040C0 RID: 16576 RVA: 0x00180068 File Offset: 0x0017E268
		private void RestoreCinematicState()
		{
			this._presentationActive = false;
			this.RestoreCameraAndInputState();
			this._uiSuppression.Restore();
		}

		// Token: 0x060040C1 RID: 16577 RVA: 0x00180082 File Offset: 0x0017E282
		private void AbandonCinematicState()
		{
			this._presentationActive = false;
			this._cameraStateCaptured = false;
			this._cameraFrozenByPresentation = false;
			this._thirdPersonCamera = null;
			this._cameraTarget = null;
			this._inputStateCaptured = false;
			this._playerInput = null;
			this._uiSuppression.Abandon();
		}

		// Token: 0x060040C2 RID: 16578 RVA: 0x001800C0 File Offset: 0x0017E2C0
		private static bool IsRuntimeTearingDown()
		{
			return !Application.isPlaying;
		}

		// Token: 0x060040C3 RID: 16579 RVA: 0x001800CC File Offset: 0x0017E2CC
		private void RestoreCameraAndInputState()
		{
			this.StopTrafficAudio();
			if (this._cameraStateCaptured && this._thirdPersonCamera != null)
			{
				this.ApplyCameraPose(this._gameplayCameraPosition, this._gameplayCameraRotation);
				this._thirdPersonCamera.mouseX = this._gameplayCameraMouseX;
				this._thirdPersonCamera.mouseY = this._gameplayCameraMouseY;
				if (this._cameraFrozenByPresentation)
				{
					this._thirdPersonCamera.UnFreezeCamera();
				}
				else
				{
					this._thirdPersonCamera.isFreezed = this._previousCameraFreeze;
				}
			}
			this._cameraStateCaptured = false;
			this._cameraFrozenByPresentation = false;
			this._thirdPersonCamera = null;
			this._cameraTarget = null;
			if (this._inputStateCaptured && this._playerInput != null)
			{
				this._playerInput.SetLockAllInput(this._previousInputLock);
				this._playerInput.SetLockCameraInput(this._previousCameraInputLock);
			}
			this._inputStateCaptured = false;
			this._playerInput = null;
			PlayerActivityLease activityLease = this._activityLease;
			if (activityLease != null)
			{
				activityLease.Dispose();
			}
			this._activityLease = null;
		}

		// Token: 0x060040C4 RID: 16580 RVA: 0x001801CC File Offset: 0x0017E3CC
		private Vector3 ResolveFrontCameraPosition()
		{
			Vector3 vector = Vector3.ProjectOnPlane(this._cameraTarget.forward, Vector3.up).normalized;
			if (vector.sqrMagnitude < 0.001f)
			{
				vector = Vector3.forward;
			}
			Vector3 normalized = Vector3.Cross(Vector3.up, vector).normalized;
			Vector3 vector2 = vector * this.frontDistance + normalized * this.frontSideOffset;
			vector2 = Quaternion.AngleAxis(this.frontYawOffset, Vector3.up) * vector2;
			float num = Mathf.Max(this.minimumCameraDistance + this.frontPushIn, vector2.magnitude);
			Vector3 vector3 = ((vector2.sqrMagnitude > 0.001f) ? vector2.normalized : vector);
			float num2 = this.cameraHeight - this.lookAtHeight;
			Vector3 cameraPivot = this.GetCameraPivot();
			Vector3 vector4 = this._gameplayCameraPosition;
			float num3 = float.NegativeInfinity;
			for (int i = 0; i < PoiArrivalPresentation.FrontCameraSearchAngles.Length; i++)
			{
				float num4 = PoiArrivalPresentation.FrontCameraSearchAngles[i];
				Vector3 vector5 = Quaternion.AngleAxis(num4, Vector3.up) * vector3;
				Vector3 vector6 = cameraPivot + vector5 * num + Vector3.up * num2;
				Vector3 vector7 = this.ResolveCameraCollision(cameraPivot, vector6);
				float num5 = Vector3.Distance(cameraPivot, vector7);
				if (num5 >= this.minimumCameraDistance + this.frontPushIn)
				{
					float num6 = Mathf.Abs(num4) / 180f * 0.65f;
					float num7 = num5 - num6;
					if (num7 > num3)
					{
						num3 = num7;
						vector4 = vector7;
					}
				}
			}
			if (num3 > float.NegativeInfinity)
			{
				return vector4;
			}
			Vector3 vector8 = this.ResolveCameraCollision(cameraPivot, this._gameplayCameraPosition);
			if (!this.HasMinimumCameraDistance(cameraPivot, vector8))
			{
				return this._gameplayCameraPosition;
			}
			return vector8;
		}

		// Token: 0x060040C5 RID: 16581 RVA: 0x00180390 File Offset: 0x0017E590
		private Vector3 GetCameraPivot()
		{
			if (!(this._cameraTarget != null))
			{
				return Vector3.zero;
			}
			return this._cameraTarget.position + Vector3.up * this.lookAtHeight;
		}

		// Token: 0x060040C6 RID: 16582 RVA: 0x001803C8 File Offset: 0x0017E5C8
		private Quaternion GetLookRotation(Vector3 cameraPosition)
		{
			Vector3 vector = this.GetCameraPivot() - cameraPosition;
			if (vector.sqrMagnitude <= 0.0001f)
			{
				return Quaternion.identity;
			}
			return Quaternion.LookRotation(vector.normalized, Vector3.up);
		}

		// Token: 0x060040C7 RID: 16583 RVA: 0x00180408 File Offset: 0x0017E608
		private Vector3 ResolveCameraCollision(Vector3 pivot, Vector3 desired)
		{
			Vector3 vector = desired - pivot;
			float magnitude = vector.magnitude;
			if (magnitude <= 0.001f)
			{
				return desired;
			}
			vector /= magnitude;
			RaycastHit[] array = Physics.SphereCastAll(pivot, this.collisionRadius, vector, magnitude, -1, QueryTriggerInteraction.Ignore);
			float num = magnitude;
			for (int i = 0; i < array.Length; i++)
			{
				Collider collider = array[i].collider;
				if (!(collider == null))
				{
					Transform transform = collider.transform;
					if ((!(this._cameraTarget != null) || (!(transform == this._cameraTarget) && !transform.IsChildOf(this._cameraTarget))) && (!(this._thirdPersonCamera != null) || !transform.IsChildOf(this._thirdPersonCamera.transform)))
					{
						num = Mathf.Min(num, array[i].distance);
					}
				}
			}
			return pivot + vector * Mathf.Max(0.05f, num - this.collisionPadding);
		}

		// Token: 0x060040C8 RID: 16584 RVA: 0x00180504 File Offset: 0x0017E704
		private bool HasMinimumCameraDistance(Vector3 pivot, Vector3 cameraPosition)
		{
			return Vector3.Distance(pivot, cameraPosition) >= this.minimumCameraDistance;
		}

		// Token: 0x060040C9 RID: 16585 RVA: 0x00180518 File Offset: 0x0017E718
		private void ApplyCameraPose(Vector3 position, Quaternion rotation)
		{
			if (this._thirdPersonCamera == null)
			{
				return;
			}
			Rigidbody selfRigidbody = this._thirdPersonCamera.selfRigidbody;
			selfRigidbody.position = position;
			selfRigidbody.rotation = rotation;
			this._thirdPersonCamera.transform.SetPositionAndRotation(position, rotation);
		}

		// Token: 0x060040CA RID: 16586 RVA: 0x00180553 File Offset: 0x0017E753
		private IEnumerator WaitWhileRefreshingUi(float duration)
		{
			float elapsed = 0f;
			while (elapsed < duration)
			{
				elapsed += Time.unscaledDeltaTime;
				this._uiSuppression.Refresh();
				yield return null;
			}
			yield break;
		}

		// Token: 0x060040CB RID: 16587 RVA: 0x00180569 File Offset: 0x0017E769
		private IEnumerator AnimateTitleAndPostFx()
		{
			if (this._titleGroup == null)
			{
				yield break;
			}
			this._titleGroup.gameObject.SetActive(true);
			this._titleGroup.alpha = 0f;
			if (!this._titleOnly)
			{
				this.StartTrafficAudio();
			}
			if (this._arrivalVolume != null)
			{
				this._arrivalVolume.weight = 0f;
			}
			yield return this.AnimatePhase(0f, 1f, this.titleFadeIn, true, true);
			float minimumHoldDeadline = Time.realtimeSinceStartup + this.titleHold;
			float maximumHoldDeadline = Time.realtimeSinceStartup + Mathf.Max(this.titleHold + 4f, this.voiceDelay + PoiArrivalPresentation.EstimateVoiceDuration(this._arrivalVoice) + 3f);
			float postFxElapsed = 0f;
			while (Time.realtimeSinceStartup < maximumHoldDeadline && (Time.realtimeSinceStartup < minimumHoldDeadline || !this._voicePlayRequestFinished || (this._arrivalVoiceStarted && this._arrivalVoiceController != null && this._arrivalVoiceController.CurrentVoice == this._arrivalVoice && this._arrivalVoiceController.IsSpeaking)))
			{
				postFxElapsed += Time.unscaledDeltaTime;
				if (this._arrivalVolume != null)
				{
					this._arrivalVolume.weight = 1f - PoiArrivalPresentation.SmoothStep(postFxElapsed / 1.8f);
				}
				this._uiSuppression.Refresh();
				yield return null;
			}
			yield return this.AnimatePhase(1f, 0f, this.titleFadeOut, false, false);
			this.StopTrafficAudio();
			this._titleGroup.alpha = 0f;
			this._titleGroup.gameObject.SetActive(false);
			if (this._arrivalVolume != null)
			{
				this._arrivalVolume.weight = 0f;
			}
			yield break;
		}

		// Token: 0x060040CC RID: 16588 RVA: 0x00180578 File Offset: 0x0017E778
		public void ConfigureTrafficAudio(AudioClip passClip, AudioClip hornOne, AudioClip hornTwo, PoiArrivalCatalogSO.TrafficPreset[] presets = null)
		{
			this.trafficPassClip = passClip;
			this.trafficHornOneClip = hornOne;
			this.trafficHornTwoClip = hornTwo;
			this.trafficPresets = presets;
			if (passClip != null)
			{
				passClip.LoadAudioData();
			}
			if (hornOne != null)
			{
				hornOne.LoadAudioData();
			}
			if (hornTwo != null)
			{
				hornTwo.LoadAudioData();
			}
			if (presets == null)
			{
				return;
			}
			foreach (PoiArrivalCatalogSO.TrafficPreset trafficPreset in presets)
			{
				if (((trafficPreset != null) ? trafficPreset.cues : null) != null)
				{
					foreach (PoiArrivalCatalogSO.TrafficCue trafficCue in trafficPreset.cues)
					{
						if (((trafficCue != null) ? trafficCue.clip : null) != null && trafficCue.clip.loadState == AudioDataLoadState.Unloaded)
						{
							trafficCue.clip.LoadAudioData();
						}
					}
				}
			}
		}

		// Token: 0x060040CD RID: 16589 RVA: 0x0018064C File Offset: 0x0017E84C
		private PoiArrivalCatalogSO.TrafficPreset PickTrafficPreset()
		{
			if (this.trafficPresets == null)
			{
				return null;
			}
			int num2;
			int num = (PoiArrivalPresentation.LastTrafficPresetByZone.TryGetValue(this.zoneDisplayName, out num2) ? num2 : (-1));
			bool flag = num >= 0 && num < this.trafficPresets.Length && this.trafficPresets[num] != null && this.trafficPresets[num].HasPlayableCue;
			int num3 = 0;
			foreach (PoiArrivalCatalogSO.TrafficPreset trafficPreset in this.trafficPresets)
			{
				if (trafficPreset != null && trafficPreset.HasPlayableCue)
				{
					num3++;
				}
			}
			if (num3 == 0)
			{
				return null;
			}
			bool flag2 = flag && num3 > 1;
			int num4 = Random.Range(0, num3 - (flag2 ? 1 : 0));
			for (int j = 0; j < this.trafficPresets.Length; j++)
			{
				PoiArrivalCatalogSO.TrafficPreset trafficPreset2 = this.trafficPresets[j];
				if (trafficPreset2 != null && trafficPreset2.HasPlayableCue && (!flag2 || j != num) && num4-- == 0)
				{
					PoiArrivalPresentation.LastTrafficPresetByZone[this.zoneDisplayName] = j;
					return trafficPreset2;
				}
			}
			return null;
		}

		// Token: 0x060040CE RID: 16590 RVA: 0x00180758 File Offset: 0x0017E958
		private void StartTrafficAudio()
		{
			this.StopTrafficAudio();
			AmbientAudioController ambientAudioController = Object.FindObjectOfType<AmbientAudioController>();
			if (ambientAudioController != null && ambientAudioController.IsNight)
			{
				return;
			}
			this._trafficAudioActive = true;
			this._trafficCuesAllowed = true;
			this._trafficPresentationWeight = 0f;
			PoiArrivalCatalogSO.TrafficPreset trafficPreset = this.PickTrafficPreset();
			if (trafficPreset != null)
			{
				foreach (PoiArrivalCatalogSO.TrafficCue trafficCue in trafficPreset.cues)
				{
					if (((trafficCue != null) ? trafficCue.clip : null) != null)
					{
						base.StartCoroutine(this.PlayTrafficCue(trafficCue.clip, trafficCue.delay, trafficCue.volume, trafficCue.startPan, trafficCue.endPan, trafficCue.passingCar, trafficCue.pitch));
					}
				}
				return;
			}
			base.StartCoroutine(this.PlayTrafficCue(this.trafficPassClip, 0f, 0.3f, -0.75f, 0.75f, true, 1f));
			base.StartCoroutine(this.PlayTrafficCue(this.trafficHornOneClip, 0.35f, 0.22f, -0.45f, -0.25f, false, 1f));
			base.StartCoroutine(this.PlayTrafficCue(this.trafficHornTwoClip, 1.85f, 0.17f, 0.55f, 0.65f, false, 1f));
		}

		// Token: 0x060040CF RID: 16591 RVA: 0x0018089C File Offset: 0x0017EA9C
		private IEnumerator PlayTrafficCue(AudioClip clip, float delay, float volume, float startPan, float endPan, bool passingCar = false, float pitch = 1f)
		{
			if (clip == null || !this._trafficAudioActive || !this._trafficCuesAllowed)
			{
				yield break;
			}
			if (delay > 0f)
			{
				yield return new WaitForSecondsRealtime(delay);
			}
			if (!this._trafficAudioActive || !this._trafficCuesAllowed || !base.isActiveAndEnabled || SceneManager.GetActiveScene() != base.gameObject.scene)
			{
				yield break;
			}
			GameObject gameObject = new GameObject("Arrival Traffic " + clip.name);
			gameObject.transform.SetParent(base.transform, false);
			AudioSource source = gameObject.AddComponent<AudioSource>();
			this._trafficSources.Add(source);
			source.playOnAwake = false;
			source.loop = false;
			source.spatialBlend = 0f;
			source.panStereo = startPan;
			source.volume = 0f;
			float basePitch = Mathf.Clamp(pitch, 0.5f, 2f);
			source.pitch = basePitch * (passingCar ? 1.08f : 1f);
			source.clip = clip;
			GameAudioRouter.Route(source, GameAudioRouter.Channel.Sfx);
			source.Play();
			while (this._trafficAudioActive && source != null && source.isPlaying && SceneManager.GetActiveScene() == base.gameObject.scene)
			{
				float num = Mathf.Clamp01(source.time / Mathf.Max(0.01f, clip.length));
				source.panStereo = Mathf.Lerp(startPan, endPan, num);
				float num2 = Mathf.Min(num / 0.12f, (1f - num) / 0.3f);
				source.volume = volume * Mathf.SmoothStep(0f, 1f, num2) * this._trafficPresentationWeight;
				if (passingCar)
				{
					source.pitch = basePitch * Mathf.Lerp(1.08f, 0.94f, num);
				}
				yield return null;
			}
			this._trafficSources.Remove(source);
			if (source != null)
			{
				source.Stop();
				Object.Destroy(source.gameObject);
			}
			yield break;
		}

		// Token: 0x060040D0 RID: 16592 RVA: 0x001808EC File Offset: 0x0017EAEC
		private void StopTrafficAudio()
		{
			this._trafficAudioActive = false;
			this._trafficCuesAllowed = false;
			this._trafficPresentationWeight = 0f;
			for (int i = 0; i < this._trafficSources.Count; i++)
			{
				AudioSource audioSource = this._trafficSources[i];
				if (!(audioSource == null))
				{
					audioSource.Stop();
					Object.Destroy(audioSource.gameObject);
				}
			}
			this._trafficSources.Clear();
		}

		// Token: 0x060040D1 RID: 16593 RVA: 0x0018095A File Offset: 0x0017EB5A
		private IEnumerator AnimatePhase(float from, float to, float duration, bool entering, bool updatePostFx = true)
		{
			if (!entering)
			{
				this._trafficCuesAllowed = false;
			}
			if (duration <= 0f)
			{
				this.ApplyPresentationWeight(to, entering ? 1f : 0f, updatePostFx);
				yield break;
			}
			float elapsed = 0f;
			while (elapsed < duration)
			{
				elapsed += Time.unscaledDeltaTime;
				float num = PoiArrivalPresentation.SmoothStep(Mathf.Clamp01(elapsed / duration));
				float num2 = Mathf.Lerp(from, to, num);
				this.ApplyPresentationWeight(num2, entering ? num : (1f - num), updatePostFx);
				this._uiSuppression.Refresh();
				yield return null;
			}
			this.ApplyPresentationWeight(to, entering ? 1f : 0f, updatePostFx);
			yield break;
		}

		// Token: 0x060040D2 RID: 16594 RVA: 0x00180990 File Offset: 0x0017EB90
		private void ApplyPresentationWeight(float weight, float entranceProgress, bool updatePostFx = true)
		{
			this._trafficPresentationWeight = Mathf.Clamp01(weight);
			if (this._titleGroup != null)
			{
				this._titleGroup.alpha = weight;
			}
			if (updatePostFx && this._arrivalVolume != null)
			{
				this._arrivalVolume.weight = Mathf.Clamp01(weight);
			}
			if (this._titleCard != null)
			{
				float num = Mathf.Lerp(0.975f, 1f, entranceProgress);
				this._titleCard.localScale = new Vector3(num, num, 1f);
			}
		}

		// Token: 0x060040D3 RID: 16595 RVA: 0x00180A1C File Offset: 0x0017EC1C
		private CielVoiceSO PickVoiceLine()
		{
			if (this.voiceLines == null || this.voiceLines.Length == 0)
			{
				return null;
			}
			List<int> list = new List<int>(this.voiceLines.Length);
			for (int i = 0; i < this.voiceLines.Length; i++)
			{
				if (this.voiceLines[i] != null && this.voiceLines[i].HasClips)
				{
					list.Add(i);
				}
			}
			if (list.Count == 0)
			{
				return null;
			}
			string text = (string.IsNullOrWhiteSpace(this.zoneDisplayName) ? base.gameObject.scene.name : this.zoneDisplayName);
			int num;
			bool flag = PoiArrivalPresentation.LastVoiceByZone.TryGetValue(text, out num);
			int num2 = list[Random.Range(0, list.Count)];
			if (list.Count > 1 && flag && num2 == num)
			{
				int num3 = list.IndexOf(num2);
				num2 = list[(num3 + Random.Range(1, list.Count)) % list.Count];
			}
			PoiArrivalPresentation.LastVoiceByZone[text] = num2;
			return this.voiceLines[num2];
		}

		// Token: 0x060040D4 RID: 16596 RVA: 0x00180B30 File Offset: 0x0017ED30
		private void BuildPostProcessing()
		{
			this._arrivalVolume = base.gameObject.AddComponent<Volume>();
			this._arrivalVolume.isGlobal = true;
			this._arrivalVolume.priority = 80f;
			this._arrivalVolume.weight = 0f;
			this._runtimeProfile = ScriptableObject.CreateInstance<VolumeProfile>();
			this._runtimeProfile.name = this.zoneDisplayName + " Arrival (Runtime)";
			Vignette vignette = this._runtimeProfile.Add<Vignette>(true);
			vignette.color.Override(new Color(0.08f, 0.025f, 0.13f));
			vignette.intensity.Override(this.vignetteIntensity);
			vignette.smoothness.Override(0.72f);
			this._runtimeProfile.Add<ChromaticAberration>(true).intensity.Override(this.chromaticAberration);
			ColorAdjustments colorAdjustments = this._runtimeProfile.Add<ColorAdjustments>(true);
			colorAdjustments.saturation.Override(this.saturation);
			colorAdjustments.postExposure.Override(-0.08f);
			this._arrivalVolume.profile = this._runtimeProfile;
		}

		// Token: 0x060040D5 RID: 16597 RVA: 0x00180C44 File Offset: 0x0017EE44
		private void BuildTitleUI()
		{
			GameObject gameObject = new GameObject("POI Arrival Title", new Type[]
			{
				typeof(RectTransform),
				typeof(Canvas),
				typeof(CanvasScaler)
			});
			gameObject.transform.SetParent(base.transform, false);
			this._titleCanvas = gameObject.transform;
			Canvas component = gameObject.GetComponent<Canvas>();
			component.renderMode = RenderMode.ScreenSpaceOverlay;
			component.sortingOrder = 90;
			GameplayHudVisibility.RegisterCanvas(component);
			CanvasScaler component2 = gameObject.GetComponent<CanvasScaler>();
			component2.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			component2.referenceResolution = new Vector2(1920f, 1080f);
			component2.matchWidthOrHeight = 0.5f;
			GameObject gameObject2 = new GameObject("Title Card", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasGroup)
			});
			gameObject2.transform.SetParent(gameObject.transform, false);
			this._titleCard = gameObject2.GetComponent<RectTransform>();
			this._titleCard.anchorMin = new Vector2(0.5f, 1f);
			this._titleCard.anchorMax = new Vector2(0.5f, 1f);
			this._titleCard.pivot = new Vector2(0.5f, 1f);
			this._titleCard.anchoredPosition = new Vector2(0f, -58f);
			this._titleCard.sizeDelta = new Vector2(1120f, 126f);
			this._titleGroup = gameObject2.GetComponent<CanvasGroup>();
			this._titleGroup.alpha = 0f;
			this._titleGroup.interactable = false;
			this._titleGroup.blocksRaycasts = false;
			DAGradient dagradient = PoiArrivalPresentation.CreateImage(this._titleCard, "Contrast Scrim", new Vector2(0f, -44f), new Vector2(1040f, 116f), Color.white).AddComponent<DAGradient>();
			Gradient gradient = new Gradient();
			Color color = new Color(0.035f, 0.008f, 0.055f, 1f);
			gradient.SetKeys(new GradientColorKey[]
			{
				new GradientColorKey(color, 0f),
				new GradientColorKey(color, 0.5f),
				new GradientColorKey(color, 1f)
			}, new GradientAlphaKey[]
			{
				new GradientAlphaKey(0f, 0f),
				new GradientAlphaKey(0.68f, 0.22f),
				new GradientAlphaKey(0.84f, 0.5f),
				new GradientAlphaKey(0.68f, 0.78f),
				new GradientAlphaKey(0f, 1f)
			});
			dagradient.Gradient = gradient;
			dagradient.BlendMode = DAColorBlendMode.Overlay;
			dagradient.Intensity = 1f;
			dagradient.Angle = 90f;
			this._zoneTitleText = this.CreateText(this._titleCard, "Zone Name", this.zoneDisplayName, 70f, FontStyles.Bold, new Color(0.86f, 0.64f, 1f, 1f), new Vector2(0f, -38f), new Vector2(1050f, 92f), 3f);
			Shadow shadow = this._zoneTitleText.gameObject.AddComponent<Shadow>();
			shadow.effectColor = new Color(0.025f, 0.008f, 0.04f, 0.98f);
			shadow.effectDistance = new Vector2(0f, -4f);
			Outline outline = this._zoneTitleText.gameObject.AddComponent<Outline>();
			outline.effectColor = new Color(0.055f, 0.015f, 0.08f, 0.9f);
			outline.effectDistance = new Vector2(1.5f, -1.5f);
			PoiArrivalPresentation.CreateImage(this._titleCard, "Accent", new Vector2(0f, -96f), new Vector2(148f, 3f), new Color(0.75f, 0.42f, 1f, 0.95f));
			gameObject2.SetActive(false);
		}

		// Token: 0x060040D6 RID: 16598 RVA: 0x00181058 File Offset: 0x0017F258
		private static GameObject CreateImage(RectTransform parent, string objectName, Vector2 position, Vector2 size, Color color)
		{
			GameObject gameObject = new GameObject(objectName, new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			gameObject.transform.SetParent(parent, false);
			RectTransform component = gameObject.GetComponent<RectTransform>();
			Vector2 vector = new Vector2(0.5f, 0.5f);
			component.anchorMax = vector;
			component.anchorMin = vector;
			component.anchoredPosition = position;
			component.sizeDelta = size;
			Image component2 = gameObject.GetComponent<Image>();
			component2.color = color;
			component2.raycastTarget = false;
			return gameObject;
		}

		// Token: 0x060040D7 RID: 16599 RVA: 0x001810E0 File Offset: 0x0017F2E0
		private TextMeshProUGUI CreateText(RectTransform parent, string objectName, string content, float fontSize, FontStyles style, Color color, Vector2 position, Vector2 size, float characterSpacing)
		{
			GameObject gameObject = new GameObject(objectName, new Type[]
			{
				typeof(RectTransform),
				typeof(TextMeshProUGUI)
			});
			gameObject.transform.SetParent(parent, false);
			RectTransform component = gameObject.GetComponent<RectTransform>();
			Vector2 vector = new Vector2(0.5f, 0.5f);
			component.anchorMax = vector;
			component.anchorMin = vector;
			component.anchoredPosition = position;
			component.sizeDelta = size;
			TextMeshProUGUI component2 = gameObject.GetComponent<TextMeshProUGUI>();
			component2.text = content;
			component2.fontSize = fontSize;
			component2.fontStyle = style;
			component2.color = color;
			component2.alignment = TextAlignmentOptions.Center;
			component2.characterSpacing = characterSpacing;
			component2.enableWordWrapping = false;
			component2.raycastTarget = false;
			if (this.titleFont != null)
			{
				component2.font = this.titleFont;
			}
			LocalizedGameText.BindSource(component2, "poi_arrival", content, null).FitTranslationToRectangle(false);
			return component2;
		}

		// Token: 0x060040D8 RID: 16600 RVA: 0x001811C8 File Offset: 0x0017F3C8
		private static float SmoothStep(float value)
		{
			value = Mathf.Clamp01(value);
			return value * value * (3f - 2f * value);
		}

		// Token: 0x060040D9 RID: 16601 RVA: 0x001811E3 File Offset: 0x0017F3E3
		private static float SmootherStep(float value)
		{
			value = Mathf.Clamp01(value);
			return value * value * value * (value * (value * 6f - 15f) + 10f);
		}

		// Token: 0x060040DA RID: 16602 RVA: 0x00181208 File Offset: 0x0017F408
		public static PoiArrivalPresentation CreateTitleOnly(Transform parent, string displayName, TMP_FontAsset font = null, float authoredTitleHoldDuration = 0f)
		{
			GameObject gameObject = new GameObject("Arrival Title - " + displayName);
			gameObject.SetActive(false);
			gameObject.transform.SetParent(parent, false);
			PoiArrivalPresentation poiArrivalPresentation = gameObject.AddComponent<PoiArrivalPresentation>();
			poiArrivalPresentation._titleOnly = true;
			poiArrivalPresentation.Configure(displayName, null, font, 0f, false, default(Vector2), 1.5f, 0f, authoredTitleHoldDuration);
			return poiArrivalPresentation;
		}

		// Token: 0x060040DB RID: 16603 RVA: 0x0018126A File Offset: 0x0017F46A
		public void ShowTitleOnly()
		{
			if (!this._titleOnly || this._hasStarted)
			{
				return;
			}
			this._hasStarted = true;
			this._voicePlayRequestFinished = true;
			base.gameObject.SetActive(true);
			base.StartCoroutine(this.TitleOnlyRoutine());
		}

		// Token: 0x060040DC RID: 16604 RVA: 0x001812A4 File Offset: 0x0017F4A4
		private IEnumerator TitleOnlyRoutine()
		{
			yield return this.AnimateTitleAndPostFx();
			Object.Destroy(base.gameObject);
			yield break;
		}

		// Token: 0x060040DD RID: 16605 RVA: 0x001812B4 File Offset: 0x0017F4B4
		public void Configure(string displayName, CielVoiceSO[] lines, TMP_FontAsset font = null, float cameraYawOffset = 0f, bool useGameplayCameraOrbitOverride = false, Vector2 authoredGameplayCameraOrbit = default(Vector2), float authoredGameplayCameraDistance = 1.5f, float authoredCameraReturnDuration = 0f, float authoredTitleHoldDuration = 0f)
		{
			this.zoneDisplayName = (string.IsNullOrWhiteSpace(displayName) ? "POI" : displayName);
			this.voiceLines = lines;
			this.frontYawOffset = cameraYawOffset;
			this.overrideGameplayCameraOrbit = useGameplayCameraOrbitOverride;
			this.gameplayCameraOrbit = authoredGameplayCameraOrbit;
			this.gameplayCameraDistance = Mathf.Max(0.5f, authoredGameplayCameraDistance);
			if (authoredCameraReturnDuration > 0f)
			{
				this.cameraReturnDuration = authoredCameraReturnDuration;
			}
			if (authoredTitleHoldDuration > 0f)
			{
				this.titleHold = authoredTitleHoldDuration;
			}
			if (font != null)
			{
				this.titleFont = font;
			}
			if (this._zoneTitleText != null)
			{
				LocalizedGameText.BindSource(this._zoneTitleText, "poi_arrival", this.zoneDisplayName, null).SetSourceFont(this.titleFont);
			}
			if (this._runtimeProfile != null)
			{
				this._runtimeProfile.name = this.zoneDisplayName + " Arrival (Runtime)";
			}
		}

		// Token: 0x04004080 RID: 16512
		private const int TitleCanvasSortingOrder = 90;

		// Token: 0x04004081 RID: 16513
		private static readonly float[] FrontCameraSearchAngles = new float[] { 0f, -30f, 30f, -60f, 60f, -90f, 90f, 180f };

		// Token: 0x04004082 RID: 16514
		private static readonly Dictionary<string, int> LastVoiceByZone = new Dictionary<string, int>();

		// Token: 0x04004083 RID: 16515
		private static readonly Dictionary<string, int> LastTrafficPresetByZone = new Dictionary<string, int>();

		// Token: 0x04004084 RID: 16516
		private static readonly HashSet<PoiArrivalPresentation> EnabledPresentations = new HashSet<PoiArrivalPresentation>();

		// Token: 0x04004085 RID: 16517
		[Header("Zone")]
		[SerializeField]
		private string zoneDisplayName = "POI";

		// Token: 0x04004086 RID: 16518
		[Header("Ciel voice")]
		[SerializeField]
		private CielVoiceSO[] voiceLines;

		// Token: 0x04004087 RID: 16519
		[SerializeField]
		[Min(0f)]
		private float voiceDelay = 1.05f;

		// Token: 0x04004088 RID: 16520
		[Header("Arrival traffic")]
		[SerializeField]
		private AudioClip trafficPassClip;

		// Token: 0x04004089 RID: 16521
		[SerializeField]
		private AudioClip trafficHornOneClip;

		// Token: 0x0400408A RID: 16522
		[SerializeField]
		private AudioClip trafficHornTwoClip;

		// Token: 0x0400408B RID: 16523
		[SerializeField]
		private PoiArrivalCatalogSO.TrafficPreset[] trafficPresets;

		// Token: 0x0400408C RID: 16524
		private readonly List<AudioSource> _trafficSources = new List<AudioSource>(8);

		// Token: 0x0400408D RID: 16525
		private bool _trafficAudioActive;

		// Token: 0x0400408E RID: 16526
		private bool _trafficCuesAllowed;

		// Token: 0x0400408F RID: 16527
		private float _trafficPresentationWeight;

		// Token: 0x04004090 RID: 16528
		[Header("Reveal")]
		[SerializeField]
		[Min(0.05f)]
		private float revealDuration = 0.9f;

		// Token: 0x04004091 RID: 16529
		[SerializeField]
		[Min(0f)]
		private float titleFadeIn = 0.3f;

		// Token: 0x04004092 RID: 16530
		[SerializeField]
		[Min(0f)]
		private float titleHold = 2.15f;

		// Token: 0x04004093 RID: 16531
		[SerializeField]
		[Min(0f)]
		private float titleFadeOut = 0.65f;

		// Token: 0x04004094 RID: 16532
		[Header("Cinematic camera")]
		[SerializeField]
		[Min(0f)]
		private float frontShotDuration = 2.35f;

		// Token: 0x04004095 RID: 16533
		[SerializeField]
		[Min(0.05f)]
		private float cameraReturnDuration = 1.75f;

		// Token: 0x04004096 RID: 16534
		[SerializeField]
		[Min(0.5f)]
		private float frontDistance = 3.1f;

		// Token: 0x04004097 RID: 16535
		[SerializeField]
		private float frontSideOffset = 0.55f;

		// Token: 0x04004098 RID: 16536
		[SerializeField]
		[Range(-180f, 180f)]
		private float frontYawOffset;

		// Token: 0x04004099 RID: 16537
		[SerializeField]
		private bool overrideGameplayCameraOrbit;

		// Token: 0x0400409A RID: 16538
		[SerializeField]
		private Vector2 gameplayCameraOrbit;

		// Token: 0x0400409B RID: 16539
		[SerializeField]
		[Min(0.5f)]
		private float gameplayCameraDistance = 1.5f;

		// Token: 0x0400409C RID: 16540
		[SerializeField]
		[Min(0.5f)]
		private float cameraHeight = 1.62f;

		// Token: 0x0400409D RID: 16541
		[SerializeField]
		[Min(0.5f)]
		private float lookAtHeight = 1.32f;

		// Token: 0x0400409E RID: 16542
		[SerializeField]
		[Min(0f)]
		private float frontPushIn = 0.28f;

		// Token: 0x0400409F RID: 16543
		[SerializeField]
		[Min(0f)]
		private float orbitLift = 0.28f;

		// Token: 0x040040A0 RID: 16544
		[SerializeField]
		private bool orbitClockwise = true;

		// Token: 0x040040A1 RID: 16545
		[SerializeField]
		[Min(0.05f)]
		private float collisionRadius = 0.2f;

		// Token: 0x040040A2 RID: 16546
		[SerializeField]
		[Min(0f)]
		private float collisionPadding = 0.12f;

		// Token: 0x040040A3 RID: 16547
		[SerializeField]
		[Min(0.5f)]
		private float minimumCameraDistance = 1.15f;

		// Token: 0x040040A4 RID: 16548
		[SerializeField]
		[Min(0f)]
		private float cameraFinishBeforeVoiceEnd = 0.35f;

		// Token: 0x040040A5 RID: 16549
		[Header("Detection")]
		[SerializeField]
		[Min(0f)]
		private float detectionEnableDelay = 10f;

		// Token: 0x040040A6 RID: 16550
		[Header("Post processing")]
		[SerializeField]
		[Range(0f, 1f)]
		private float vignetteIntensity = 0.3f;

		// Token: 0x040040A7 RID: 16551
		[SerializeField]
		[Range(0f, 1f)]
		private float chromaticAberration = 0.16f;

		// Token: 0x040040A8 RID: 16552
		[SerializeField]
		[Range(-100f, 100f)]
		private float saturation = -12f;

		// Token: 0x040040A9 RID: 16553
		[Header("Debug")]
		[SerializeField]
		private bool enableDebugLogs;

		// Token: 0x040040AA RID: 16554
		[Header("Presentation style")]
		[SerializeField]
		private TMP_FontAsset titleFont;

		// Token: 0x040040AB RID: 16555
		private readonly SceneUiSuppressionScope _uiSuppression = new SceneUiSuppressionScope();

		// Token: 0x040040AC RID: 16556
		private CanvasGroup _titleGroup;

		// Token: 0x040040AD RID: 16557
		private RectTransform _titleCard;

		// Token: 0x040040AE RID: 16558
		private Transform _titleCanvas;

		// Token: 0x040040AF RID: 16559
		private TextMeshProUGUI _zoneTitleText;

		// Token: 0x040040B0 RID: 16560
		private Volume _arrivalVolume;

		// Token: 0x040040B1 RID: 16561
		private VolumeProfile _runtimeProfile;

		// Token: 0x040040B2 RID: 16562
		private PlayerActivityLease _activityLease;

		// Token: 0x040040B3 RID: 16563
		private vThirdPersonInput _playerInput;

		// Token: 0x040040B4 RID: 16564
		private bool _inputStateCaptured;

		// Token: 0x040040B5 RID: 16565
		private bool _previousInputLock;

		// Token: 0x040040B6 RID: 16566
		private bool _previousCameraInputLock;

		// Token: 0x040040B7 RID: 16567
		private vThirdPersonCamera _thirdPersonCamera;

		// Token: 0x040040B8 RID: 16568
		private Transform _cameraTarget;

		// Token: 0x040040B9 RID: 16569
		private bool _cameraStateCaptured;

		// Token: 0x040040BA RID: 16570
		private bool _previousCameraFreeze;

		// Token: 0x040040BB RID: 16571
		private bool _cameraFrozenByPresentation;

		// Token: 0x040040BC RID: 16572
		private float _gameplayCameraMouseX;

		// Token: 0x040040BD RID: 16573
		private float _gameplayCameraMouseY;

		// Token: 0x040040BE RID: 16574
		private Vector3 _gameplayCameraPosition;

		// Token: 0x040040BF RID: 16575
		private Quaternion _gameplayCameraRotation;

		// Token: 0x040040C0 RID: 16576
		private SensoryManager _pausedSensoryManager;

		// Token: 0x040040C1 RID: 16577
		private bool _ownsDetectionPause;

		// Token: 0x040040C2 RID: 16578
		private CielVoiceController _arrivalVoiceController;

		// Token: 0x040040C3 RID: 16579
		private CielVoiceSubtitleUI _arrivalSubtitleUi;

		// Token: 0x040040C4 RID: 16580
		private CielVoiceSO _arrivalVoice;

		// Token: 0x040040C5 RID: 16581
		private bool _voicePlayRequestFinished;

		// Token: 0x040040C6 RID: 16582
		private bool _arrivalVoiceStarted;

		// Token: 0x040040C7 RID: 16583
		private bool _hasStarted;

		// Token: 0x040040C8 RID: 16584
		private bool _presentationActive;

		// Token: 0x040040C9 RID: 16585
		private bool _destinationRevealPending;

		// Token: 0x040040CA RID: 16586
		private bool _titleOnly;
	}
}
