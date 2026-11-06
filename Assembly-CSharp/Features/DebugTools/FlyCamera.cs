using System;
using System.Collections.Generic;
using Features.CielOS;
using Features.Detection;
using Features.GameOver;
using Features.GameOver.Newspaper;
using Features.MainMenu;
using Features.MatchRecap;
using Features.Police;
using Invector;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.Rendering;
using ProjectExhibitionist.FirstPerson;
using RayznGames.VehicleSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Features.DebugTools
{
	// Token: 0x02001341 RID: 4929
	[DefaultExecutionOrder(1000)]
	public class FlyCamera : MonoBehaviour
	{
		// Token: 0x1700183D RID: 6205
		// (get) Token: 0x0600980E RID: 38926 RVA: 0x003E4C62 File Offset: 0x003E2E62
		// (set) Token: 0x0600980F RID: 38927 RVA: 0x003E4C69 File Offset: 0x003E2E69
		public static bool IsActive { get; private set; }

		// Token: 0x1700183E RID: 6206
		// (get) Token: 0x06009810 RID: 38928 RVA: 0x003E4C71 File Offset: 0x003E2E71
		public static bool IsExerciseCameraActive
		{
			get
			{
				return FlyCamera.Instance != null && FlyCamera.Instance._active && FlyCamera.Instance._exerciseMode;
			}
		}

		// Token: 0x1700183F RID: 6207
		// (get) Token: 0x06009811 RID: 38929 RVA: 0x003E4C98 File Offset: 0x003E2E98
		internal static bool IsGlobalPauseActive
		{
			get
			{
				return FlyCamera.Instance != null && FlyCamera.Instance._active && !FlyCamera.Instance._exerciseMode && FlyCamera.Instance._frozeTime;
			}
		}

		// Token: 0x17001840 RID: 6208
		// (get) Token: 0x06009812 RID: 38930 RVA: 0x003E4CCB File Offset: 0x003E2ECB
		// (set) Token: 0x06009813 RID: 38931 RVA: 0x003E4CD2 File Offset: 0x003E2ED2
		public static FlyCamera Instance { get; private set; }

		// Token: 0x17001841 RID: 6209
		// (get) Token: 0x06009814 RID: 38932 RVA: 0x003E4CDC File Offset: 0x003E2EDC
		public static bool CanEnterPhotoMode
		{
			get
			{
				return !GameplayInputGate.BlocksGameplayInput && !GraphicsSettingsRuntimeUI.AnyOpen && !TextInputFocus.BlocksGameplayInput && !CielOSController.AnyOpen && (!(NewspaperController.Instance != null) || !NewspaperController.Instance.IsShowing) && (!(NpcPhotoGalleryController.Instance != null) || !NpcPhotoGalleryController.Instance.IsShowing) && !FlyCameraPhotoMode.AnyOpen && (!(ExhibitionEventDirector.Instance != null) || !ExhibitionEventDirector.Instance.OwnsPhotoModeInput) && (PlayerActivityGate.Allows(PlayerActivity.DebugFly) || FlyCamera.CanPhotographPerpWalk());
			}
		}

		// Token: 0x06009815 RID: 38933 RVA: 0x003E4D6C File Offset: 0x003E2F6C
		private static bool CanPhotographPerpWalk()
		{
			PerpWalkController instance = PerpWalkController.Instance;
			GameOverManager instance2 = GameOverManager.Instance;
			return instance != null && instance.IsActive && instance2 != null && instance2.IsGameOver && PlayerActivityGate.CanAcquireAlongside(PlayerActivity.DebugFly, PlayerContextResource.None, instance2, "GameOver");
		}

		// Token: 0x06009816 RID: 38934 RVA: 0x003E4DBA File Offset: 0x003E2FBA
		internal static void StopGlobalPause()
		{
			if (!FlyCamera.IsGlobalPauseActive || FlyCamera.Instance == null)
			{
				return;
			}
			FlyCamera.Instance._toggleConsumedFrame = Time.frameCount;
			FlyCamera.Instance.Deactivate();
		}

		// Token: 0x06009817 RID: 38935 RVA: 0x003E4DEA File Offset: 0x003E2FEA
		internal static void StopForContextChange()
		{
			if (FlyCamera.Instance == null)
			{
				return;
			}
			FlyCamera.Instance._toggleConsumedFrame = Time.frameCount;
			if (FlyCamera.Instance._active)
			{
				FlyCamera.Instance.Deactivate();
			}
		}

		// Token: 0x06009818 RID: 38936 RVA: 0x003E4E20 File Offset: 0x003E3020
		internal static void StopForGameplayCamera(Camera camera)
		{
			if (FlyCamera.Instance == null || !FlyCamera.Instance._active || FlyCamera.Instance._gameplayCam != camera)
			{
				return;
			}
			FlyCamera.Instance._toggleConsumedFrame = Time.frameCount;
			FlyCamera.Instance.Deactivate();
		}

		// Token: 0x06009819 RID: 38937 RVA: 0x003E4E74 File Offset: 0x003E3074
		internal static void StopForActivityLease(PlayerActivityLease lease)
		{
			if (lease == null || !lease.IsActive || (lease.Resources & PlayerContextResource.Camera) == PlayerContextResource.None || FlyCamera.Instance == null || !FlyCamera.Instance._active || FlyCamera.Instance._exerciseMode)
			{
				return;
			}
			FlyCamera.Instance._toggleConsumedFrame = Time.frameCount;
			FlyCamera.Instance.Deactivate();
		}

		// Token: 0x0600981A RID: 38938 RVA: 0x003E4ED8 File Offset: 0x003E30D8
		internal static void StopForRagdollRecovery(CielRagdollRecovery owner)
		{
			if (owner == null || FlyCamera.Instance == null || !FlyCamera.Instance._active || FlyCamera.Instance._exerciseMode || FlyCamera.Instance._ragdollRecovery != owner)
			{
				return;
			}
			FlyCamera.Instance._toggleConsumedFrame = Time.frameCount;
			FlyCamera.Instance.Deactivate();
		}

		// Token: 0x0600981B RID: 38939 RVA: 0x003E4F3F File Offset: 0x003E313F
		private void Awake()
		{
			if (FlyCamera.Instance != null && FlyCamera.Instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			FlyCamera.Instance = this;
		}

		// Token: 0x0600981C RID: 38940 RVA: 0x003E4F6D File Offset: 0x003E316D
		private void OnEnable()
		{
			SceneManager.activeSceneChanged += this.OnActiveSceneChanged;
		}

		// Token: 0x0600981D RID: 38941 RVA: 0x003E4F80 File Offset: 0x003E3180
		private void OnActiveSceneChanged(Scene previous, Scene current)
		{
			if (this._active)
			{
				FlyCamera.StopForContextChange();
			}
		}

		// Token: 0x0600981E RID: 38942 RVA: 0x003E4F8F File Offset: 0x003E318F
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void Bootstrap()
		{
			if (Object.FindObjectOfType<FlyCamera>() != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("[FlyCamera]");
			gameObject.AddComponent<FlyCamera>();
			Object.DontDestroyOnLoad(gameObject);
		}

		// Token: 0x0600981F RID: 38943 RVA: 0x003E4FB8 File Offset: 0x003E31B8
		private void Update()
		{
			if (this._active && (GameplayInputGate.BlocksGameplayInput || GraphicsSettingsRuntimeUI.AnyOpen || this._flyCam == null || this._gameplayCam == null || !this._gameplayCam.gameObject.activeInHierarchy || (this._exerciseMode && this._exerciseFocus == null)))
			{
				FlyCamera.StopForContextChange();
				return;
			}
			if (GraphicsSettingsRuntimeUI.AnyOpen)
			{
				return;
			}
			if (TextInputFocus.BlocksGameplayInput)
			{
				return;
			}
			if (this._active && EscapeInputCoordinator.TryConsumeForPhotoMode(this))
			{
				FlyCamera.StopForContextChange();
				return;
			}
			if (this.GetToggleKeyDown() && this._toggleConsumedFrame != Time.frameCount)
			{
				this.TogglePhotoMode();
			}
			if (!this._active)
			{
				return;
			}
			this.HandleLook();
			this.HandleMove();
			if (FlyCamera.GetShutterKeyDown() && this._photoMode != null)
			{
				this._photoMode.CapturePhoto();
			}
		}

		// Token: 0x06009820 RID: 38944 RVA: 0x003E509B File Offset: 0x003E329B
		public void TogglePhotoMode()
		{
			if (GraphicsSettingsRuntimeUI.AnyOpen)
			{
				return;
			}
			if (TextInputFocus.BlocksGameplayInput)
			{
				return;
			}
			this._toggleConsumedFrame = Time.frameCount;
			if (this._active)
			{
				this.Deactivate();
				return;
			}
			if (FlyCamera.CanEnterPhotoMode)
			{
				this.Activate();
			}
		}

		// Token: 0x06009821 RID: 38945 RVA: 0x003E50D4 File Offset: 0x003E32D4
		internal static void ConsumeToggleForCurrentFrame()
		{
			if (FlyCamera.Instance != null)
			{
				FlyCamera.Instance._toggleConsumedFrame = Time.frameCount;
			}
		}

		// Token: 0x06009822 RID: 38946 RVA: 0x003E50F4 File Offset: 0x003E32F4
		public static bool ToggleExerciseCamera(Transform focus, Object owner)
		{
			FlyCamera flyCamera = FlyCamera.EnsureInstance();
			if (flyCamera == null || focus == null || owner == null)
			{
				return false;
			}
			flyCamera._toggleConsumedFrame = Time.frameCount;
			if (!flyCamera._active)
			{
				return flyCamera.ActivateExerciseCamera(focus, owner);
			}
			if (!flyCamera._exerciseMode || flyCamera._exerciseOwner != owner)
			{
				return false;
			}
			flyCamera.Deactivate();
			return false;
		}

		// Token: 0x06009823 RID: 38947 RVA: 0x003E515F File Offset: 0x003E335F
		public static bool IsExerciseCameraOwnedBy(Object owner)
		{
			return FlyCamera.IsExerciseCameraActive && FlyCamera.Instance._exerciseOwner == owner;
		}

		// Token: 0x06009824 RID: 38948 RVA: 0x003E517A File Offset: 0x003E337A
		public static void StopExerciseCamera(Object owner)
		{
			if (!FlyCamera.IsExerciseCameraActive || (owner != null && FlyCamera.Instance._exerciseOwner != owner))
			{
				return;
			}
			FlyCamera.Instance.Deactivate();
		}

		// Token: 0x06009825 RID: 38949 RVA: 0x003E51AC File Offset: 0x003E33AC
		private static FlyCamera EnsureInstance()
		{
			if (FlyCamera.Instance != null)
			{
				return FlyCamera.Instance;
			}
			FlyCamera.Instance = Object.FindObjectOfType<FlyCamera>();
			if (FlyCamera.Instance != null)
			{
				return FlyCamera.Instance;
			}
			GameObject gameObject = new GameObject("[FlyCamera]");
			FlyCamera.Instance = gameObject.AddComponent<FlyCamera>();
			Object.DontDestroyOnLoad(gameObject);
			return FlyCamera.Instance;
		}

		// Token: 0x06009826 RID: 38950 RVA: 0x003E5208 File Offset: 0x003E3408
		private bool ActivateExerciseCamera(Transform focus, Object owner)
		{
			if (FlyCameraPhotoMode.AnyOpen)
			{
				return false;
			}
			this._exerciseMode = true;
			this._exerciseSubjectRoot = focus;
			this._exerciseFocus = FlyCamera.ResolveExerciseFocus(focus);
			this._exerciseOwner = owner;
			this._exerciseFirstPerson = focus.GetComponent<FirstPersonController>();
			if (this._exerciseFirstPerson == null)
			{
				this._exerciseFirstPerson = focus.GetComponentInChildren<FirstPersonController>(true);
			}
			this._restoreFirstPerson = this._exerciseFirstPerson != null && this._exerciseFirstPerson.IsFirstPerson;
			if (this._restoreFirstPerson)
			{
				this._exerciseFirstPerson.ExitFirstPerson();
			}
			this.DisableCullingFade(focus);
			this.Activate();
			return this._active && this._exerciseMode;
		}

		// Token: 0x06009827 RID: 38951 RVA: 0x003E52B8 File Offset: 0x003E34B8
		private void Activate()
		{
			if (!this._exerciseMode && !FlyCamera.CanEnterPhotoMode)
			{
				return;
			}
			this._ragdollRecovery = (this._exerciseMode ? null : FlyCamera.FindRecoveringPlayer());
			this._gameplayCam = FlyCamera.FindActiveGameplayCamera();
			if (this._flyCam == null)
			{
				GameObject gameObject = new GameObject("[FlyCamera] Cam");
				gameObject.transform.SetParent(base.transform, false);
				this._flyCam = gameObject.AddComponent<Camera>();
				this._flyListener = gameObject.AddComponent<AudioListener>();
			}
			if (this._gameplayCam != null)
			{
				this._flyCam.transform.SetPositionAndRotation(this._gameplayCam.transform.position, this._gameplayCam.transform.rotation);
				this._flyCam.fieldOfView = this._gameplayCam.fieldOfView;
				this._flyCam.farClipPlane = this._gameplayCam.farClipPlane;
				this._flyCam.cullingMask = this._gameplayCam.cullingMask;
				this._gameplayCam.enabled = false;
				this._savedGameplayCameraTag = this._gameplayCam.gameObject.tag;
				this._savedFlyCameraTag = this._flyCam.gameObject.tag;
				if (this._gameplayCam.CompareTag("MainCamera"))
				{
					this._gameplayCam.gameObject.tag = "Untagged";
					this._flyCam.gameObject.tag = "MainCamera";
					this._cameraTagsSwapped = true;
				}
			}
			if (this._exerciseMode)
			{
				this._flyCam.nearClipPlane = Mathf.Clamp(this.ExerciseNearClipPlane, 0.001f, 0.05f);
				this.PlaceExerciseCameraNearCiel();
			}
			else
			{
				this._flyCam.nearClipPlane = 0.001f;
				this.DisableCullingFade(null);
			}
			this._flyCam.useOcclusionCulling = false;
			Vector3 eulerAngles = this._flyCam.transform.eulerAngles;
			this._yaw = (this._targetYaw = eulerAngles.y);
			this._pitch = (this._targetPitch = FlyCamera.NormalizePitch(eulerAngles.x));
			this._currentVelocity = Vector3.zero;
			this._velocityRef = Vector3.zero;
			this._flyCam.enabled = true;
			this.SuspendGameplayAudioListeners();
			if (this._flyListener != null)
			{
				this._flyListener.enabled = true;
			}
			this._restoreTerminalCursor = !this._exerciseMode && PerpWalkController.Instance != null && PerpWalkController.Instance.IsActive;
			this._terminalCursorLock = Cursor.lockState;
			this._terminalCursorVisible = Cursor.visible;
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			this.LockPlayerInput();
			this.HideAllUI();
			if (!this._exerciseMode && (this.FreezeGameWhileActive || this._restoreTerminalCursor || this._ragdollRecovery != null))
			{
				this._prevTimeScale = Time.timeScale;
				Time.timeScale = 0f;
				this._audioPause = GameplayAudioPause.Acquire();
				this._frozeTime = true;
			}
			this._active = true;
			FlyCamera.IsActive = true;
			if (this._photoMode == null)
			{
				this._photoMode = base.gameObject.AddComponent<FlyCameraPhotoMode>();
			}
			this._photoMode.Enter(this._flyCam, this._gameplayCam, "K", true);
		}

		// Token: 0x06009828 RID: 38952 RVA: 0x003E55F8 File Offset: 0x003E37F8
		private void Deactivate()
		{
			if (this._photoMode != null)
			{
				this._photoMode.Exit();
			}
			bool exerciseMode = this._exerciseMode;
			this._active = false;
			FlyCamera.IsActive = false;
			if (this._frozeTime)
			{
				if (Mathf.Approximately(Time.timeScale, 0f) && !GraphicsSettingsRuntimeUI.AnyOpen)
				{
					Time.timeScale = this._prevTimeScale;
				}
				IDisposable audioPause = this._audioPause;
				if (audioPause != null)
				{
					audioPause.Dispose();
				}
				this._audioPause = null;
				this._frozeTime = false;
			}
			this.UnlockPlayerInput();
			this.RestoreUI();
			if (this._flyCam != null)
			{
				this._flyCam.enabled = false;
			}
			if (this._flyListener != null)
			{
				this._flyListener.enabled = false;
			}
			this.RestoreGameplayAudioListeners();
			if (this._gameplayCam != null)
			{
				this._gameplayCam.enabled = true;
			}
			if (this._cameraTagsSwapped)
			{
				if (this._flyCam != null)
				{
					this._flyCam.gameObject.tag = this._savedFlyCameraTag;
				}
				if (this._gameplayCam != null)
				{
					this._gameplayCam.gameObject.tag = this._savedGameplayCameraTag;
				}
				this._cameraTagsSwapped = false;
			}
			bool flag = GraphicsSettingsRuntimeUI.AnyOpen || (Application.isPlaying && GameplayInputGate.IsFrontend);
			Cursor.lockState = (flag ? CursorLockMode.None : (this._restoreTerminalCursor ? this._terminalCursorLock : CursorLockMode.Locked));
			Cursor.visible = flag || (this._restoreTerminalCursor && this._terminalCursorVisible);
			this._restoreTerminalCursor = false;
			if (exerciseMode && this._restoreFirstPerson && this._exerciseFirstPerson != null)
			{
				this._exerciseFirstPerson.EnterFirstPerson();
			}
			this.RestoreCullingFade();
			this._exerciseMode = false;
			this._exerciseFocus = null;
			this._exerciseSubjectRoot = null;
			this._exerciseOwner = null;
			this._exerciseFirstPerson = null;
			this._restoreFirstPerson = false;
			this._ragdollRecovery = null;
		}

		// Token: 0x06009829 RID: 38953 RVA: 0x003E57E4 File Offset: 0x003E39E4
		private static CielRagdollRecovery FindRecoveringPlayer()
		{
			foreach (CielRagdollRecovery cielRagdollRecovery in Object.FindObjectsOfType<CielRagdollRecovery>())
			{
				if (cielRagdollRecovery.isActiveAndEnabled && cielRagdollRecovery.IsRecovering)
				{
					return cielRagdollRecovery;
				}
			}
			return null;
		}

		// Token: 0x0600982A RID: 38954 RVA: 0x003E581C File Offset: 0x003E3A1C
		private void DisableCullingFade(Transform focus)
		{
			FlyCamera.DisableCullingFade(focus, this._disabledCullingFades);
		}

		// Token: 0x0600982B RID: 38955 RVA: 0x003E582C File Offset: 0x003E3A2C
		internal static void DisableCullingFade(Transform focus, List<vCullingFadeControl> disabledFades)
		{
			disabledFades.Clear();
			foreach (vCullingFadeControl vCullingFadeControl in (focus != null) ? focus.GetComponentsInChildren<vCullingFadeControl>(true) : Object.FindObjectsOfType<vCullingFadeControl>())
			{
				if (!(vCullingFadeControl == null) && vCullingFadeControl.enabled)
				{
					FlyCamera.RestoreFadeMaterials(vCullingFadeControl.fadeMeshRenderers);
					FlyCamera.RestoreFadeMaterials(vCullingFadeControl.fadeSkinnedMeshRenderes);
					vCullingFadeControl.usingTransp = false;
					vCullingFadeControl.enabled = false;
					disabledFades.Add(vCullingFadeControl);
				}
			}
		}

		// Token: 0x0600982C RID: 38956 RVA: 0x003E58A8 File Offset: 0x003E3AA8
		private static void RestoreFadeMaterials(List<FadeMaterials> groups)
		{
			if (groups == null)
			{
				return;
			}
			for (int i = 0; i < groups.Count; i++)
			{
				FadeMaterials fadeMaterials = groups[i];
				if (fadeMaterials != null && fadeMaterials.renderer != null && fadeMaterials.originalMaterials != null)
				{
					fadeMaterials.renderer.sharedMaterials = fadeMaterials.originalMaterials;
				}
			}
		}

		// Token: 0x0600982D RID: 38957 RVA: 0x003E58FC File Offset: 0x003E3AFC
		private void RestoreCullingFade()
		{
			FlyCamera.RestoreCullingFade(this._disabledCullingFades);
		}

		// Token: 0x0600982E RID: 38958 RVA: 0x003E590C File Offset: 0x003E3B0C
		internal static void RestoreCullingFade(List<vCullingFadeControl> disabledFades)
		{
			for (int i = 0; i < disabledFades.Count; i++)
			{
				vCullingFadeControl vCullingFadeControl = disabledFades[i];
				if (vCullingFadeControl != null)
				{
					vCullingFadeControl.enabled = true;
				}
			}
			disabledFades.Clear();
		}

		// Token: 0x0600982F RID: 38959 RVA: 0x003E5948 File Offset: 0x003E3B48
		private void SuspendGameplayAudioListeners()
		{
			this._disabledGameplayListeners.Clear();
			foreach (AudioListener audioListener in Object.FindObjectsOfType<AudioListener>())
			{
				if (!(audioListener == this._flyListener) && audioListener.enabled)
				{
					audioListener.enabled = false;
					this._disabledGameplayListeners.Add(audioListener);
				}
			}
		}

		// Token: 0x06009830 RID: 38960 RVA: 0x003E59A4 File Offset: 0x003E3BA4
		private void RestoreGameplayAudioListeners()
		{
			foreach (AudioListener audioListener in this._disabledGameplayListeners)
			{
				if (audioListener != null)
				{
					audioListener.enabled = true;
				}
			}
			this._disabledGameplayListeners.Clear();
		}

		// Token: 0x06009831 RID: 38961 RVA: 0x003E5A0C File Offset: 0x003E3C0C
		private void LockPlayerInput()
		{
			this._lockedPlayerInputs.Clear();
			foreach (vThirdPersonInput vThirdPersonInput in Object.FindObjectsOfType<vThirdPersonInput>())
			{
				if (!(vThirdPersonInput == null) && vThirdPersonInput.enabled && !vThirdPersonInput.lockInput)
				{
					vThirdPersonInput.SetLockAllInput(true);
					CielRagdollRecovery component = vThirdPersonInput.GetComponent<CielRagdollRecovery>();
					if (vThirdPersonInput.cc != null && !vThirdPersonInput.cc.ragdolled && (component == null || !component.IsRecovering))
					{
						vThirdPersonInput.cc.StopCharacter();
					}
					this._lockedPlayerInputs.Add(vThirdPersonInput);
				}
			}
		}

		// Token: 0x06009832 RID: 38962 RVA: 0x003E5AA8 File Offset: 0x003E3CA8
		private void UnlockPlayerInput()
		{
			foreach (vThirdPersonInput vThirdPersonInput in this._lockedPlayerInputs)
			{
				if (vThirdPersonInput != null)
				{
					vThirdPersonInput.SetLockAllInput(false);
				}
			}
			this._lockedPlayerInputs.Clear();
		}

		// Token: 0x06009833 RID: 38963 RVA: 0x003E5B10 File Offset: 0x003E3D10
		private void HideAllUI()
		{
			this._hiddenCanvases.Clear();
			foreach (Canvas canvas in Object.FindObjectsOfType<Canvas>(false))
			{
				if (!(canvas == null) && canvas.enabled && !ActionUiSuppressionScope.IsProtectedCanvas(canvas) && !canvas.transform.IsChildOf(base.transform))
				{
					canvas.enabled = false;
					this._hiddenCanvases.Add(canvas);
				}
			}
		}

		// Token: 0x06009834 RID: 38964 RVA: 0x003E5B80 File Offset: 0x003E3D80
		private void RestoreUI()
		{
			foreach (Canvas canvas in this._hiddenCanvases)
			{
				if (canvas != null)
				{
					canvas.enabled = true;
				}
			}
			this._hiddenCanvases.Clear();
		}

		// Token: 0x06009835 RID: 38965 RVA: 0x003E5BE8 File Offset: 0x003E3DE8
		private void HandleLook()
		{
			Vector2 vector = FlyCamera.GetMouseDelta() * this.LookSensitivity;
			this._targetYaw += vector.x;
			this._targetPitch -= vector.y;
			this._targetPitch = Mathf.Clamp(this._targetPitch, -this.MaxPitch, this.MaxPitch);
			float num = 1f - Mathf.Exp(-this.LookSmoothing * Time.unscaledDeltaTime);
			this._yaw = Mathf.LerpAngle(this._yaw, this._targetYaw, num);
			this._pitch = Mathf.Lerp(this._pitch, this._targetPitch, num);
			this._flyCam.transform.rotation = Quaternion.Euler(this._pitch, this._yaw, 0f);
		}

		// Token: 0x06009836 RID: 38966 RVA: 0x003E5CBC File Offset: 0x003E3EBC
		private void HandleMove()
		{
			if (!this._exerciseMode)
			{
				float scroll = FlyCamera.GetScroll();
				if (Mathf.Abs(scroll) > 0.01f)
				{
					this.MoveSpeed = FlyCamera.AdjustMoveSpeed(this.MoveSpeed, scroll, this.MinSpeed, this.MaxSpeed);
				}
			}
			Vector3 moveInput = FlyCamera.GetMoveInput();
			float num = (this._exerciseMode ? this.ExerciseMoveSpeed : this.MoveSpeed);
			float num2 = (this._exerciseMode ? this.ExerciseBoostMultiplier : this.BoostMultiplier);
			float num3 = num * (FlyCamera.GetBoost() ? num2 : 1f);
			Vector3 vector = this._flyCam.transform.TransformDirection(moveInput) * num3;
			this._currentVelocity = Vector3.SmoothDamp(this._currentVelocity, vector, ref this._velocityRef, this.MoveSmoothTime, float.PositiveInfinity, Time.unscaledDeltaTime);
			this._flyCam.transform.position += this._currentVelocity * Time.unscaledDeltaTime;
			if (this._exerciseMode)
			{
				this._flyCam.transform.position = FlyCamera.ClampToExerciseRadius(this._flyCam.transform.position, this.ExerciseFocusPoint(), this.ExerciseMaxRadius);
			}
		}

		// Token: 0x06009837 RID: 38967 RVA: 0x003E5DED File Offset: 0x003E3FED
		private Vector3 ExerciseFocusPoint()
		{
			if (this._exerciseFocus == null)
			{
				return Vector3.zero;
			}
			return this._exerciseFocus.position;
		}

		// Token: 0x06009838 RID: 38968 RVA: 0x003E5E10 File Offset: 0x003E4010
		private void PlaceExerciseCameraNearCiel()
		{
			Vector3 vector = this.ExerciseFocusPoint();
			Vector3 vector2 = ((this._gameplayCam != null) ? (this._gameplayCam.transform.position - vector) : Vector3.zero);
			if (!FlyCamera.IsFinite(vector2) || vector2.sqrMagnitude < 0.01f)
			{
				vector2 = -((this._exerciseSubjectRoot != null) ? this._exerciseSubjectRoot.forward : Vector3.forward) + Vector3.up * 0.2f;
			}
			float num = Mathf.Clamp(this.ExerciseStartDistance, 0.35f, Mathf.Max(0.35f, this.ExerciseMaxRadius));
			this._flyCam.transform.position = vector + vector2.normalized * num;
			Vector3 vector3 = vector - this._flyCam.transform.position;
			if (vector3.sqrMagnitude > 0.0001f)
			{
				this._flyCam.transform.rotation = Quaternion.LookRotation(vector3, Vector3.up);
			}
		}

		// Token: 0x06009839 RID: 38969 RVA: 0x003E5F24 File Offset: 0x003E4124
		private static Transform ResolveExerciseFocus(Transform subjectRoot)
		{
			if (subjectRoot == null)
			{
				return null;
			}
			Animator componentInChildren = subjectRoot.GetComponentInChildren<Animator>(true);
			if (componentInChildren != null && componentInChildren.isHuman)
			{
				Transform transform;
				if ((transform = componentInChildren.GetBoneTransform(HumanBodyBones.UpperChest)) == null)
				{
					transform = componentInChildren.GetBoneTransform(HumanBodyBones.Chest) ?? componentInChildren.GetBoneTransform(HumanBodyBones.Hips);
				}
				Transform transform2 = transform;
				if (transform2 != null)
				{
					return transform2;
				}
			}
			return subjectRoot;
		}

		// Token: 0x0600983A RID: 38970 RVA: 0x003E5F80 File Offset: 0x003E4180
		private static Camera FindActiveGameplayCamera()
		{
			Camera main = Camera.main;
			if (FlyCamera.IsUsableGameplayCamera(main))
			{
				return main;
			}
			Camera camera = null;
			foreach (Camera camera2 in Camera.allCameras)
			{
				if (FlyCamera.IsUsableGameplayCamera(camera2) && (camera == null || camera2.depth > camera.depth))
				{
					camera = camera2;
				}
			}
			return camera;
		}

		// Token: 0x0600983B RID: 38971 RVA: 0x003E5FE0 File Offset: 0x003E41E0
		private static bool IsUsableGameplayCamera(Camera camera)
		{
			return camera != null && camera.enabled && camera.gameObject.activeInHierarchy && camera.targetTexture == null && camera.cameraType == CameraType.Game && (FlyCamera.Instance == null || camera != FlyCamera.Instance._flyCam);
		}

		// Token: 0x0600983C RID: 38972 RVA: 0x003E6044 File Offset: 0x003E4244
		private static bool IsFinite(Vector3 value)
		{
			return !float.IsNaN(value.x) && !float.IsInfinity(value.x) && !float.IsNaN(value.y) && !float.IsInfinity(value.y) && !float.IsNaN(value.z) && !float.IsInfinity(value.z);
		}

		// Token: 0x0600983D RID: 38973 RVA: 0x003E60A4 File Offset: 0x003E42A4
		public static Vector3 ClampToExerciseRadius(Vector3 position, Vector3 focus, float maxRadius)
		{
			float num = Mathf.Max(0.25f, maxRadius);
			Vector3 vector = position - focus;
			if (vector.sqrMagnitude <= num * num)
			{
				return position;
			}
			return focus + vector.normalized * num;
		}

		// Token: 0x0600983E RID: 38974 RVA: 0x003E60E6 File Offset: 0x003E42E6
		private void OnDestroy()
		{
			if (this._active)
			{
				this.Deactivate();
			}
			if (FlyCamera.Instance == this)
			{
				FlyCamera.Instance = null;
			}
		}

		// Token: 0x0600983F RID: 38975 RVA: 0x003E6109 File Offset: 0x003E4309
		private void OnDisable()
		{
			SceneManager.activeSceneChanged -= this.OnActiveSceneChanged;
			if (this._active)
			{
				this.Deactivate();
			}
		}

		// Token: 0x06009840 RID: 38976 RVA: 0x003E612A File Offset: 0x003E432A
		private static float NormalizePitch(float x)
		{
			if (x <= 180f)
			{
				return x;
			}
			return x - 360f;
		}

		// Token: 0x06009841 RID: 38977 RVA: 0x003E6140 File Offset: 0x003E4340
		private bool GetToggleKeyDown()
		{
			if (!ControllerInput.IsActive)
			{
				Keyboard current = Keyboard.current;
				return current != null && current.kKey.wasPressedThisFrame;
			}
			bool flag = FlyCamera.CanEnterPhotoMode && ((MatchRecapController.Instance != null && MatchRecapController.Instance.IsShowing) || (GameOverManager.Instance != null && GameOverManager.Instance.IsGameOver));
			if (ControllerWorldPointer.BlocksContextInput)
			{
				return false;
			}
			if (!flag)
			{
				return GameplayInput.GetKeyDown(this.ToggleKey);
			}
			return ControllerInput.GetKeyDown(this.ToggleKey);
		}

		// Token: 0x06009842 RID: 38978 RVA: 0x003E61CF File Offset: 0x003E43CF
		internal static bool GetShutterKeyDown()
		{
			if (ControllerInput.IsActive)
			{
				return ControllerWorldPointer.Down(ControllerButton.South);
			}
			return Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;
		}

		// Token: 0x06009843 RID: 38979 RVA: 0x003E61F8 File Offset: 0x003E43F8
		internal static Vector3 GetMoveInput()
		{
			if (ControllerInput.IsActive)
			{
				if (ControllerWorldPointer.BlocksContextInput)
				{
					return Vector3.zero;
				}
				Vector2 move = ControllerInput.Move;
				float num = (ControllerInput.GetButton(ControllerButton.RightShoulder) ? 1f : 0f) - (ControllerInput.GetButton(ControllerButton.LeftShoulder) ? 1f : 0f);
				return Vector3.ClampMagnitude(new Vector3(move.x, num, move.y), 1f);
			}
			else
			{
				float num2 = 0f;
				float num3 = 0f;
				float num4 = 0f;
				Keyboard current = Keyboard.current;
				if (current != null)
				{
					if (current.aKey.isPressed || current.leftArrowKey.isPressed)
					{
						num2 -= 1f;
					}
					if (current.dKey.isPressed || current.rightArrowKey.isPressed)
					{
						num2 += 1f;
					}
					if (current.wKey.isPressed || current.upArrowKey.isPressed)
					{
						num4 += 1f;
					}
					if (current.sKey.isPressed || current.downArrowKey.isPressed)
					{
						num4 -= 1f;
					}
					if (current.eKey.isPressed)
					{
						num3 += 1f;
					}
					if (current.qKey.isPressed)
					{
						num3 -= 1f;
					}
				}
				Vector3 vector = new Vector3(num2, num3, num4);
				if (vector.sqrMagnitude <= 1f)
				{
					return vector;
				}
				return vector.normalized;
			}
		}

		// Token: 0x06009844 RID: 38980 RVA: 0x003E6360 File Offset: 0x003E4560
		internal static bool GetBoost()
		{
			if (ControllerInput.IsActive)
			{
				return ControllerWorldPointer.Held(ControllerButton.LeftStick);
			}
			Keyboard current = Keyboard.current;
			return current != null && current.leftShiftKey.isPressed;
		}

		// Token: 0x06009845 RID: 38981 RVA: 0x003E6394 File Offset: 0x003E4594
		internal static Vector2 GetMouseDelta()
		{
			if (ControllerInput.IsActive)
			{
				if (!ControllerWorldPointer.BlocksContextInput && !ControllerInput.Modifier)
				{
					return ControllerInput.Look * (60f * Time.unscaledDeltaTime);
				}
				return Vector2.zero;
			}
			else
			{
				Mouse current = Mouse.current;
				if (current == null)
				{
					return Vector2.zero;
				}
				return GameSettingsService.ApplyMouseLookInversion(current.delta.ReadValue() * 0.05f);
			}
		}

		// Token: 0x06009846 RID: 38982 RVA: 0x003E63FC File Offset: 0x003E45FC
		internal static float GetScroll()
		{
			if (ControllerInput.IsActive)
			{
				if (!ControllerWorldPointer.BlocksContextInput)
				{
					return ControllerInput.Scroll + (ControllerInput.GetButtonDown(ControllerButton.DpadUp) ? 1f : 0f) - (ControllerInput.GetButtonDown(ControllerButton.DpadDown) ? 1f : 0f);
				}
				return 0f;
			}
			else
			{
				Mouse current = Mouse.current;
				if (current == null)
				{
					return 0f;
				}
				return current.scroll.ReadValue().y * 0.01f;
			}
		}

		// Token: 0x06009847 RID: 38983 RVA: 0x003E6474 File Offset: 0x003E4674
		internal static bool GetActionSpeedModifier()
		{
			if (ControllerInput.IsActive)
			{
				return ControllerWorldPointer.Held(ControllerButton.LeftTrigger);
			}
			Keyboard current = Keyboard.current;
			return current != null && (current.leftCtrlKey.isPressed || current.rightCtrlKey.isPressed);
		}

		// Token: 0x06009848 RID: 38984 RVA: 0x003E64B4 File Offset: 0x003E46B4
		internal static float AdjustMoveSpeed(float current, float scroll, float minimum, float maximum)
		{
			return Mathf.Clamp(current + scroll * current * 0.1f, minimum, maximum);
		}

		// Token: 0x04009D35 RID: 40245
		internal const float PhotoNearClipPlane = 0.001f;

		// Token: 0x04009D36 RID: 40246
		[Header("Activation")]
		[Tooltip("Key that toggles the fly camera on/off.")]
		public KeyCode ToggleKey = KeyCode.K;

		// Token: 0x04009D37 RID: 40247
		[Header("Movement")]
		[Tooltip("Base movement speed in units/second.")]
		public float MoveSpeed = 8f;

		// Token: 0x04009D38 RID: 40248
		[Tooltip("Multiplier applied while holding Shift.")]
		public float BoostMultiplier = 4f;

		// Token: 0x04009D39 RID: 40249
		[Tooltip("How quickly the camera accelerates/decelerates. Lower = floatier.")]
		public float MoveSmoothTime = 0.12f;

		// Token: 0x04009D3A RID: 40250
		public float MinSpeed = 1f;

		// Token: 0x04009D3B RID: 40251
		public float MaxSpeed = 80f;

		// Token: 0x04009D3C RID: 40252
		[Header("Time")]
		[Tooltip("If true, the game is frozen (Time.timeScale = 0, audio paused) while the fly camera is active.")]
		public bool FreezeGameWhileActive = true;

		// Token: 0x04009D3D RID: 40253
		[Header("Look")]
		[Tooltip("Mouse look sensitivity.")]
		public float LookSensitivity = 2f;

		// Token: 0x04009D3E RID: 40254
		[Tooltip("How quickly the look rotation catches up. Lower = smoother/laggier.")]
		public float LookSmoothing = 12f;

		// Token: 0x04009D3F RID: 40255
		public float MaxPitch = 89f;

		// Token: 0x04009D40 RID: 40256
		[Header("Exercise Camera")]
		[Tooltip("Maximum distance the exercise camera may move from Ciel's visible body centre.")]
		public float ExerciseMaxRadius = 2.75f;

		// Token: 0x04009D41 RID: 40257
		[Tooltip("Distance from Ciel used when exercise-camera mode starts.")]
		public float ExerciseStartDistance = 1.65f;

		// Token: 0x04009D42 RID: 40258
		[Tooltip("Movement speed used by the close-range exercise camera.")]
		public float ExerciseMoveSpeed = 2.25f;

		// Token: 0x04009D43 RID: 40259
		[Tooltip("Shift multiplier used by the exercise camera.")]
		public float ExerciseBoostMultiplier = 2f;

		// Token: 0x04009D44 RID: 40260
		[Tooltip("Very small near plane so Ciel remains visible during extreme close-ups.")]
		public float ExerciseNearClipPlane = 0.005f;

		// Token: 0x04009D47 RID: 40263
		private Camera _flyCam;

		// Token: 0x04009D48 RID: 40264
		private Camera _gameplayCam;

		// Token: 0x04009D49 RID: 40265
		private FlyCameraPhotoMode _photoMode;

		// Token: 0x04009D4A RID: 40266
		private CielRagdollRecovery _ragdollRecovery;

		// Token: 0x04009D4B RID: 40267
		private AudioListener _flyListener;

		// Token: 0x04009D4C RID: 40268
		private string _savedGameplayCameraTag;

		// Token: 0x04009D4D RID: 40269
		private string _savedFlyCameraTag;

		// Token: 0x04009D4E RID: 40270
		private bool _cameraTagsSwapped;

		// Token: 0x04009D4F RID: 40271
		private bool _active;

		// Token: 0x04009D50 RID: 40272
		private bool _exerciseMode;

		// Token: 0x04009D51 RID: 40273
		private Transform _exerciseFocus;

		// Token: 0x04009D52 RID: 40274
		private Object _exerciseOwner;

		// Token: 0x04009D53 RID: 40275
		private Transform _exerciseSubjectRoot;

		// Token: 0x04009D54 RID: 40276
		private FirstPersonController _exerciseFirstPerson;

		// Token: 0x04009D55 RID: 40277
		private bool _restoreFirstPerson;

		// Token: 0x04009D56 RID: 40278
		private readonly List<vCullingFadeControl> _disabledCullingFades = new List<vCullingFadeControl>();

		// Token: 0x04009D57 RID: 40279
		private int _toggleConsumedFrame = -1;

		// Token: 0x04009D58 RID: 40280
		private Vector3 _currentVelocity;

		// Token: 0x04009D59 RID: 40281
		private Vector3 _velocityRef;

		// Token: 0x04009D5A RID: 40282
		private float _yaw;

		// Token: 0x04009D5B RID: 40283
		private float _pitch;

		// Token: 0x04009D5C RID: 40284
		private float _targetYaw;

		// Token: 0x04009D5D RID: 40285
		private float _targetPitch;

		// Token: 0x04009D5E RID: 40286
		private bool _frozeTime;

		// Token: 0x04009D5F RID: 40287
		private bool _restoreTerminalCursor;

		// Token: 0x04009D60 RID: 40288
		private CursorLockMode _terminalCursorLock;

		// Token: 0x04009D61 RID: 40289
		private bool _terminalCursorVisible;

		// Token: 0x04009D62 RID: 40290
		private float _prevTimeScale = 1f;

		// Token: 0x04009D63 RID: 40291
		private IDisposable _audioPause;

		// Token: 0x04009D64 RID: 40292
		private readonly List<vThirdPersonInput> _lockedPlayerInputs = new List<vThirdPersonInput>();

		// Token: 0x04009D65 RID: 40293
		private readonly List<AudioListener> _disabledGameplayListeners = new List<AudioListener>();

		// Token: 0x04009D66 RID: 40294
		private readonly List<Canvas> _hiddenCanvases = new List<Canvas>();
	}
}
