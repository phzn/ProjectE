using System;
using System.Collections;
using System.Collections.Generic;
using Bozo.AnimeCharacters;
using Features.CielOS;
using Features.CielOS.Apps.Maps;
using Features.Detection;
using Features.GameOver;
using Features.GoldBikini;
using Features.HideFlash;
using Features.Inventory;
using Features.Inventory.Placement;
using Features.Inventory.UI;
using Features.MainMenu;
using Features.MallHack;
using Features.MasturbationSystem;
using Features.Missions;
using Features.OxGram;
using Features.PlayerEmotes;
using Features.Rank;
using Features.RapidActions;
using Features.RenderingPrecision;
using Invector.vCharacterController;
using ProjectExhibitionist.BenchSeating;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.Rendering;
using ProjectExhibitionist.FirstPerson;
using ProjectExhibitionist.Interactions;
using ProjektSumperk;
using RayznGames.VehicleSystem;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Features.PhoneCompanion
{
	// Token: 0x02000E74 RID: 3700
	public class SmartphoneController : MonoBehaviour
	{
		// Token: 0x17001217 RID: 4631
		// (get) Token: 0x06006E80 RID: 28288 RVA: 0x002C7730 File Offset: 0x002C5930
		private BenchPlayerSeating PlayerBench
		{
			get
			{
				if (!(this.playerAnimator != null))
				{
					return null;
				}
				return this.playerAnimator.GetComponent<BenchPlayerSeating>();
			}
		}

		// Token: 0x17001218 RID: 4632
		// (get) Token: 0x06006E81 RID: 28289 RVA: 0x002C774D File Offset: 0x002C594D
		public bool IsBenchBodyOwned
		{
			get
			{
				return this.PlayerBench != null && this.PlayerBench.IsBusy;
			}
		}

		// Token: 0x17001219 RID: 4633
		// (get) Token: 0x06006E82 RID: 28290 RVA: 0x002C776A File Offset: 0x002C596A
		public bool IsBenchSeated
		{
			get
			{
				return this.PlayerBench != null && this.PlayerBench.IsSeated && PlayerActivityGate.GetResourceOwner<BenchPlayerSeating>(PlayerContextResource.FullBodyAnimation) == this.PlayerBench;
			}
		}

		// Token: 0x1700121A RID: 4634
		// (get) Token: 0x06006E83 RID: 28291 RVA: 0x002C7797 File Offset: 0x002C5997
		private PlayerContextResource DesiredHandheldResources
		{
			get
			{
				if (!this.IsBenchSeated)
				{
					return PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation | PlayerContextResource.HeldItem;
				}
				return PlayerContextResource.Camera | PlayerContextResource.HeldItem;
			}
		}

		// Token: 0x06006E84 RID: 28292 RVA: 0x002C77A6 File Offset: 0x002C59A6
		public static bool IsPhoneOutFor(Animator actor)
		{
			return actor != null && SmartphoneController._activeInstance != null && SmartphoneController._activeInstance.playerAnimator == actor && SmartphoneController._activeInstance.IsPhoneOut;
		}

		// Token: 0x06006E85 RID: 28293 RVA: 0x002C77DC File Offset: 0x002C59DC
		public static bool IsHandheldPhoneOutFor(Animator actor)
		{
			return SmartphoneController.IsPhoneOutFor(actor) && !SmartphoneController._activeInstance.IsStableTripodActivityProfile;
		}

		// Token: 0x06006E86 RID: 28294 RVA: 0x002C77F5 File Offset: 0x002C59F5
		public static bool IsPlayerPoseFrozenFor(Animator actor)
		{
			return SmartphoneController.IsPhoneOutFor(actor) && SmartphoneController._activeInstance._ci_frozen;
		}

		// Token: 0x06006E87 RID: 28295 RVA: 0x002C780B File Offset: 0x002C5A0B
		public static bool IsStableTripodCameraOwnerFor(Animator actor)
		{
			return actor != null && SmartphoneController.IsStableTripodCameraOwner && SmartphoneController._activeInstance.playerAnimator == actor;
		}

		// Token: 0x06006E88 RID: 28296 RVA: 0x002C782F File Offset: 0x002C5A2F
		public static void CloseHandheldForBenchTransition(Animator actor)
		{
			if (SmartphoneController.IsHandheldPhoneOutFor(actor))
			{
				SmartphoneController._activeInstance.ExitToGameplay();
			}
		}

		// Token: 0x1700121B RID: 4635
		// (get) Token: 0x06006E89 RID: 28297 RVA: 0x002C7844 File Offset: 0x002C5A44
		public static bool ControllerSelfieOwnsMovement
		{
			get
			{
				return ControllerInput.IsActive && SmartphoneController._activeInstance != null && SmartphoneController._activeInstance._selfieActive && (SmartphoneController._activeInstance._poseMenuOpen || ControllerInput.Modifier || (SmartphoneController._activeInstance.enableSelfieHeadAdjust && ControllerInput.GetButton(ControllerButton.LeftStick)));
			}
		}

		// Token: 0x1700121C RID: 4636
		// (get) Token: 0x06006E8A RID: 28298 RVA: 0x002C789C File Offset: 0x002C5A9C
		public static bool IsControllerAdjustingPhoneView
		{
			get
			{
				return ControllerInput.IsActive && SmartphoneController._activeInstance != null && SmartphoneController._activeInstance.IsScreenInteractionActive && !PhotoPolaroidViewer.IsOpen && !PhoneVideoViewer.IsOpen && !GraphicsSettingsRuntimeUI.AnyOpen && !ControllerTextKeyboard.IsOpen && !TextInputFocus.BlocksGameplayInput && !PlayerActivityGate.IsResourceOwned(PlayerContextResource.ModalUI) && (ControllerInput.Modifier || ControllerInput.GetButton(ControllerButton.LeftStick) || ControllerInput.GetButtonDown(ControllerButton.RightStick));
			}
		}

		// Token: 0x06006E8B RID: 28299 RVA: 0x002C7910 File Offset: 0x002C5B10
		private void UpdateControllerPhoneView(float deltaTime)
		{
			if (GameplayInputGate.BlocksGameplayInput || Time.timeScale <= 0f || TextInputFocus.BlocksGameplayInput || ControllerTextKeyboard.IsOpen || GraphicsSettingsRuntimeUI.AnyOpen || PlayerActivityGate.IsResourceOwned(PlayerContextResource.ModalUI) || ControllerInput.UiInputConsumedThisFrame)
			{
				return;
			}
			if (!this._controllerPhoneViewReady)
			{
				if (!this.IsPhoneHoldReady())
				{
					return;
				}
				this.SetCursorUnlocked(true);
				this._controllerPhoneViewReady = true;
				this.CenterControllerPhone();
			}
			if (ControllerInput.GetButtonDown(ControllerButton.RightStick))
			{
				this.CenterControllerPhone();
			}
			if (ControllerInput.Modifier)
			{
				this._zoomTarget01 = Mathf.Clamp01(this._zoomTarget01 + ControllerInput.Look.y * deltaTime * 0.65f);
				return;
			}
			if (ControllerInput.GetButton(ControllerButton.LeftStick) && this.firstPersonController != null)
			{
				Vector2 vector = ControllerInput.Look * (deltaTime * 22f);
				this.firstPersonController.RotateCameraFP(vector.x, vector.y);
			}
		}

		// Token: 0x06006E8C RID: 28300 RVA: 0x002C79F8 File Offset: 0x002C5BF8
		private bool IsPhoneHoldReady()
		{
			return this.playerAnimator == null || this._holdLayerIndex < 0 || this._ci_frozen || (this.playerAnimator.GetLayerWeight(this._holdLayerIndex) >= 0.99f && !this.playerAnimator.IsInTransition(this._holdLayerIndex) && this.playerAnimator.GetCurrentAnimatorStateInfo(this._holdLayerIndex).IsName("PhoneHold"));
		}

		// Token: 0x06006E8D RID: 28301 RVA: 0x002C7A74 File Offset: 0x002C5C74
		private void CenterControllerPhone()
		{
			if (this._mainCamera == null || this.firstPersonController == null || this.phoneInstance == null)
			{
				return;
			}
			if (this._phoneCanvas == null)
			{
				foreach (Canvas canvas in this.phoneInstance.GetComponentsInChildren<Canvas>(true))
				{
					if (canvas.renderMode == RenderMode.WorldSpace)
					{
						this._phoneCanvas = canvas;
						break;
					}
				}
			}
			if (this._phoneCanvas == null)
			{
				return;
			}
			RectTransform rectTransform = this._phoneCanvas.transform as RectTransform;
			if (rectTransform == null)
			{
				return;
			}
			Vector3 vector = rectTransform.TransformPoint(rectTransform.rect.center);
			Vector3 vector2 = vector - this._mainCamera.transform.position;
			if (vector2.sqrMagnitude < 1E-06f)
			{
				return;
			}
			this.firstPersonController.FrameWorldPoint(vector);
			Quaternion quaternion = Quaternion.Inverse(Quaternion.LookRotation(vector2, Vector3.up));
			rectTransform.GetWorldCorners(this._controllerScreenCorners);
			float num = 0f;
			foreach (Vector3 vector3 in this._controllerScreenCorners)
			{
				Vector3 vector4 = quaternion * (vector3 - this._mainCamera.transform.position);
				float num2 = Mathf.Max(0.001f, vector4.z);
				num = Mathf.Max(new float[]
				{
					num,
					Mathf.Abs(vector4.y) / num2,
					Mathf.Abs(vector4.x) / (num2 * Mathf.Max(0.1f, this._mainCamera.aspect))
				});
			}
			float num3 = Mathf.Clamp(2f * Mathf.Atan(num / 0.78f) * 57.29578f, this.fovZoomedIn, this.fovZoomedOut);
			this._zoomTarget01 = Mathf.InverseLerp(this.fovZoomedOut, this.fovZoomedIn, num3);
		}

		// Token: 0x1700121D RID: 4637
		// (get) Token: 0x06006E8E RID: 28302 RVA: 0x002C7C80 File Offset: 0x002C5E80
		// (set) Token: 0x06006E8F RID: 28303 RVA: 0x002C7C88 File Offset: 0x002C5E88
		public SmartphoneController.PhoneState State { get; private set; }

		// Token: 0x1700121E RID: 4638
		// (get) Token: 0x06006E90 RID: 28304 RVA: 0x002C7C91 File Offset: 0x002C5E91
		public bool IsPhoneOut
		{
			get
			{
				return this.State > SmartphoneController.PhoneState.Gameplay;
			}
		}

		// Token: 0x06006E91 RID: 28305 RVA: 0x002C7C9C File Offset: 0x002C5E9C
		public static void SetCensorLeakMovementLock(bool locked)
		{
			if (SmartphoneController._activeInstance != null)
			{
				SmartphoneController._activeInstance.SetCensorLeakMovementLocked(locked);
			}
		}

		// Token: 0x06006E92 RID: 28306 RVA: 0x002C7CB6 File Offset: 0x002C5EB6
		public static bool RequestCensorLeakGameOver()
		{
			if (!CensorLeakState.GameOverPending || SmartphoneController._activeInstance == null)
			{
				return false;
			}
			SmartphoneController._activeInstance.SetCensorLeakMovementLocked(true);
			SmartphoneController._activeInstance._censorLeakExitRequested = true;
			return true;
		}

		// Token: 0x1700121F RID: 4639
		// (get) Token: 0x06006E93 RID: 28307 RVA: 0x002C7CE5 File Offset: 0x002C5EE5
		public static bool IsCustomAppOpen
		{
			get
			{
				return SmartphoneController._customAppCloser != null;
			}
		}

		// Token: 0x17001220 RID: 4640
		// (get) Token: 0x06006E94 RID: 28308 RVA: 0x002C7CEF File Offset: 0x002C5EEF
		public static bool IsAnyPhoneOut
		{
			get
			{
				return SmartphoneController._activeInstance != null && SmartphoneController._activeInstance.IsPhoneOut;
			}
		}

		// Token: 0x17001221 RID: 4641
		// (get) Token: 0x06006E95 RID: 28309 RVA: 0x002C7D0A File Offset: 0x002C5F0A
		public static bool IsPhonePointerActive
		{
			get
			{
				return SmartphoneController.IsCustomAppOpen || (SmartphoneController._activeInstance != null && SmartphoneController._activeInstance._cursorUnlocked);
			}
		}

		// Token: 0x17001222 RID: 4642
		// (get) Token: 0x06006E96 RID: 28310 RVA: 0x002C7D2E File Offset: 0x002C5F2E
		public bool IsScreenInteractionActive
		{
			get
			{
				return this.IsPhoneOut && this._cursorUnlocked && !this._selfieActive;
			}
		}

		// Token: 0x17001223 RID: 4643
		// (get) Token: 0x06006E97 RID: 28311 RVA: 0x002C7D4B File Offset: 0x002C5F4B
		public static bool IsAnySelfieActive
		{
			get
			{
				return SmartphoneController._activeInstance != null && SmartphoneController._activeInstance._selfieActive;
			}
		}

		// Token: 0x17001224 RID: 4644
		// (get) Token: 0x06006E98 RID: 28312 RVA: 0x002C7D66 File Offset: 0x002C5F66
		public static bool IsAnyTripodActive
		{
			get
			{
				return SmartphoneController._activeInstance != null && SmartphoneController._activeInstance._tripodActive;
			}
		}

		// Token: 0x17001225 RID: 4645
		// (get) Token: 0x06006E99 RID: 28313 RVA: 0x002C7D84 File Offset: 0x002C5F84
		public static bool IsStableTripodCameraOwner
		{
			get
			{
				return SmartphoneController._activeInstance != null && SmartphoneController._activeInstance.IsPhoneOut && SmartphoneController._activeInstance._selfieActive && SmartphoneController._activeInstance.IsStableTripodActivityProfile && SmartphoneController._activeInstance._activityLease != null && SmartphoneController._activeInstance._activityLease.IsActive && SmartphoneController._activeInstance._activityLease.Resources == PlayerContextResource.Camera && SmartphoneController._activeInstance._activityLease.Owner == SmartphoneController._activeInstance;
			}
		}

		// Token: 0x06006E9A RID: 28314 RVA: 0x002C7E09 File Offset: 0x002C6009
		public static void InterruptForDetection()
		{
			if (SmartphoneController._activeInstance == null)
			{
				return;
			}
			bool isPhoneOut = SmartphoneController._activeInstance.IsPhoneOut;
			SmartphoneController._activeInstance.ExitToGameplay();
			if (!isPhoneOut)
			{
				return;
			}
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		// Token: 0x06006E9B RID: 28315 RVA: 0x002C7E3C File Offset: 0x002C603C
		public static void NotifyCustomAppOpened(Action closeCallback)
		{
			SmartphoneController._customAppCloser = closeCallback;
		}

		// Token: 0x06006E9C RID: 28316 RVA: 0x002C7E44 File Offset: 0x002C6044
		public static void NotifyCustomAppClosed()
		{
			SmartphoneController._customAppCloser = null;
			if (SmartphoneController._activeInstance != null)
			{
				SmartphoneController._activeInstance.ResetDynamicIsland();
			}
		}

		// Token: 0x17001226 RID: 4646
		// (get) Token: 0x06006E9D RID: 28317 RVA: 0x002C7E63 File Offset: 0x002C6063
		public Transform SelfieCamAnchor
		{
			get
			{
				return this._selfieCamAnchor;
			}
		}

		// Token: 0x17001227 RID: 4647
		// (get) Token: 0x06006E9E RID: 28318 RVA: 0x002C7E6B File Offset: 0x002C606B
		public bool SelfieActive
		{
			get
			{
				return this._selfieActive;
			}
		}

		// Token: 0x17001228 RID: 4648
		// (get) Token: 0x06006E9F RID: 28319 RVA: 0x002C7E73 File Offset: 0x002C6073
		public bool TripodActive
		{
			get
			{
				return this._tripodActive;
			}
		}

		// Token: 0x17001229 RID: 4649
		// (get) Token: 0x06006EA0 RID: 28320 RVA: 0x002C7E7B File Offset: 0x002C607B
		public bool TripodTransitioning
		{
			get
			{
				return this._tripodTransitioning;
			}
		}

		// Token: 0x1700122A RID: 4650
		// (get) Token: 0x06006EA1 RID: 28321 RVA: 0x002C7E83 File Offset: 0x002C6083
		public bool TripodPlacementActive
		{
			get
			{
				return this._tripodPlacementActive;
			}
		}

		// Token: 0x1700122B RID: 4651
		// (get) Token: 0x06006EA2 RID: 28322 RVA: 0x002C7E8B File Offset: 0x002C608B
		// (set) Token: 0x06006EA3 RID: 28323 RVA: 0x002C7E93 File Offset: 0x002C6093
		public bool SuspendPhonePoseReapply { get; set; }

		// Token: 0x06006EA4 RID: 28324 RVA: 0x002C7E9C File Offset: 0x002C609C
		private void Awake()
		{
			WorldOrigin.Shifted += this.ApplyWorldOriginShift;
			if (this.playerAnimator == null)
			{
				this.playerAnimator = base.GetComponentInParent<Animator>();
			}
			if (this.playerAnimator == null)
			{
				Debug.LogError("[SmartphoneController] Nessun Animator trovato sul player. Disabilito.", this);
				base.enabled = false;
				return;
			}
			if (this.firstPersonController == null)
			{
				this.firstPersonController = base.GetComponentInParent<FirstPersonController>();
			}
			this._tpInput = base.GetComponentInParent<vThirdPersonInput>();
			this._rigController = base.GetComponentInParent<CharacterRigController>();
			this._gameplayEmoteController = base.GetComponentInParent<GameplayEmoteController>();
			this.ResolveEmoteSpeedController();
			this._holdLayerIndex = this.playerAnimator.GetLayerIndex(this.holdLayerName);
			if (this._holdLayerIndex < 0 && this.debugMode)
			{
				Debug.LogWarning("[SmartphoneController] Layer animator '" + this.holdLayerName + "' non trovato; la posa di hold non verra' pilotata.", this);
			}
			if (this.enableSelfieHeadAdjust)
			{
				GameObject gameObject = this.playerAnimator.gameObject;
				this._headAdjuster = gameObject.GetComponent<SelfieHeadAdjuster>();
				if (this._headAdjuster == null)
				{
					this._headAdjuster = gameObject.AddComponent<SelfieHeadAdjuster>();
				}
				this._headAdjuster.Initialize(this.playerAnimator);
			}
			this._cameraCapture = base.GetComponent<PhoneCameraCapture>();
			if (this._cameraCapture == null)
			{
				this._cameraCapture = base.gameObject.AddComponent<PhoneCameraCapture>();
			}
			this._posePicker = base.GetComponent<SelfiePosePicker>();
			if (this._posePicker == null)
			{
				this._posePicker = base.gameObject.AddComponent<SelfiePosePicker>();
			}
			if (this.playerAnimator != null)
			{
				this._playerEmotion = this.playerAnimator.GetComponentInParent<PlayerEmotionController>();
			}
			this._posePicker.Initialize(this.playerAnimator, this.selfiePoseConfig, new Action<bool>(this.OnPoseMenuVisibilityChanged), new Action<SelfiePoseConfigSO.PoseEntry>(this.ApplySelfiePoseOffsets), this._playerEmotion);
			this._posePicker.ConfigureBodyPoseAvailability(() => !this.IsBenchBodyOwned);
			this._posePicker.toggleMenuKey = this.selfiePoseMenuKey;
			this._posePicker.buildLegacyDropdownMenu = !this.useRadialMenu;
		}

		// Token: 0x06006EA5 RID: 28325 RVA: 0x002C80B0 File Offset: 0x002C62B0
		private SelfieRadialMenuController ResolveRadialMenu()
		{
			if (!this.useRadialMenu)
			{
				return null;
			}
			if (this._radialMenu == null)
			{
				this._radialMenu = this.selfieRadialMenu;
			}
			if (this._radialMenu == null)
			{
				this._radialMenu = Object.FindObjectOfType<SelfieRadialMenuController>(true);
			}
			if (this._radialMenu == null)
			{
				GameObject gameObject = Resources.Load<GameObject>("SelfieCameraOverlay");
				if (gameObject != null)
				{
					this._runtimeSelfieOverlay = Object.Instantiate<GameObject>(gameObject);
					this._runtimeSelfieOverlay.name = "SelfieCameraOverlay (Phone)";
					this._radialMenu = this._runtimeSelfieOverlay.GetComponentInChildren<SelfieRadialMenuController>(true);
					if (this.selfieCameraOverlay == null)
					{
						this.selfieCameraOverlay = this._runtimeSelfieOverlay.GetComponent<SelfieCameraOverlayUI>();
					}
				}
			}
			if (this._radialMenu != null && this._posePicker != null)
			{
				this._radialMenu.Initialize(this._posePicker, new Action<bool>(this.OnPoseMenuVisibilityChanged), this);
			}
			else if (this._radialMenu == null)
			{
				Debug.LogWarning("[SmartphoneController] SelfieRadialMenuController non trovato in scena. Assegna 'selfieRadialMenu' o verifica che sia presente.", this);
			}
			return this._radialMenu;
		}

		// Token: 0x06006EA6 RID: 28326 RVA: 0x002C81C8 File Offset: 0x002C63C8
		private void Start()
		{
			this.EnsurePhoneInstance();
			if (this.phoneInstance != null)
			{
				HackMePhoneInstaller.Ensure(this.phoneInstance, this);
			}
			if (this.phoneInstance != null)
			{
				MapsPhoneInstaller.Ensure(this.phoneInstance, this);
			}
			if (this.phoneInstance != null)
			{
				MissionsPhoneInstaller.Ensure(this.phoneInstance);
			}
			if (this.phoneInstance != null)
			{
				this.phoneInstance.SetActive(false);
			}
		}

		// Token: 0x06006EA7 RID: 28327 RVA: 0x002C8244 File Offset: 0x002C6444
		private void OnEnable()
		{
			PhoneSelfieNotifier.SelfieStateChanged += this.OnSelfieStateChanged;
			this._nextPhoneInvariantCheckFrame = Time.frameCount + 1;
			if (this.State == SmartphoneController.PhoneState.PhoneHeld && this.TryAcquireActivityContext())
			{
				SmartphoneController._activeInstance = this;
			}
			if (this._resumeCensorLeak)
			{
				this._resumeCensorLeak = false;
				if (CensorLeakState.GameOverPending)
				{
					SmartphoneController._activeInstance = this;
					SmartphoneController.RequestCensorLeakGameOver();
				}
			}
		}

		// Token: 0x06006EA8 RID: 28328 RVA: 0x002C82A8 File Offset: 0x002C64A8
		private void OnDisable()
		{
			PhoneSelfieNotifier.SelfieStateChanged -= this.OnSelfieStateChanged;
			this._pendingSelfieActivate = false;
			this._pendingSelfieDeactivate = false;
			if (this.State == SmartphoneController.PhoneState.PhoneHeld && base.gameObject.activeInHierarchy)
			{
				this.ExitToGameplay();
			}
			this.ApplyPhotoViewerFreeze(false);
			this.FreezeCiel(false);
			if (!base.gameObject.activeInHierarchy)
			{
				this._resumeCensorLeak = this._censorLeakMovementLocked && CensorLeakState.GameOverPending;
				if (this._censorLeakRoutine != null)
				{
					base.StopCoroutine(this._censorLeakRoutine);
				}
				this._censorLeakRoutine = null;
				this.SetCensorLeakMovementLocked(false);
			}
			if (this._cursorUnlocked)
			{
				this._cursorUnlocked = false;
				if (this._tpInput != null)
				{
					this._tpInput.lockCameraInput = this._savedLockCameraInput;
				}
				vThirdPersonInput.ForceCameraLock = false;
			}
			if (this._tripodActive || this._tripodTransitioning || this._tripodPlacementActive)
			{
				if (base.gameObject.activeInHierarchy)
				{
					this.RestorePhoneFromTripodImmediate(false);
				}
				else
				{
					this.RequestTripodRestore(false);
				}
			}
			if (SmartphoneController._activeInstance == this)
			{
				SmartphoneController._activeInstance = null;
				SmartphoneController._customAppCloser = null;
			}
			this.ReleaseActivityContext();
		}

		// Token: 0x06006EA9 RID: 28329 RVA: 0x002C83CB File Offset: 0x002C65CB
		private void ApplyWorldOriginShift(Vector3 delta)
		{
			this._tripodGroundPosition += delta;
		}

		// Token: 0x06006EAA RID: 28330 RVA: 0x002C83DF File Offset: 0x002C65DF
		private void OnDestroy()
		{
			WorldOrigin.Shifted -= this.ApplyWorldOriginShift;
			this.ReleaseActivityContext();
			if (this._runtimeSelfieOverlay != null)
			{
				Object.Destroy(this._runtimeSelfieOverlay);
			}
		}

		// Token: 0x06006EAB RID: 28331 RVA: 0x002C8414 File Offset: 0x002C6614
		private bool TryAcquireActivityContext()
		{
			if (this._activityLease != null)
			{
				if (this._activityLease.IsActive)
				{
					return true;
				}
				this._activityLease = null;
			}
			bool isStableTripodActivityProfile = this.IsStableTripodActivityProfile;
			return PlayerActivityGate.TryAcquire("Smartphone.Session", this, isStableTripodActivityProfile ? PlayerActivity.None : PlayerActivity.Phone, isStableTripodActivityProfile ? PlayerContextResource.Camera : this.DesiredHandheldResources, isStableTripodActivityProfile ? (PlayerActivity.Cover | PlayerActivity.DebugFly) : (PlayerActivity.Emote | PlayerActivity.Cover | PlayerActivity.Flash | PlayerActivity.RapidHUD | PlayerActivity.Interact | PlayerActivity.Undress | PlayerActivity.LingerieOpen | PlayerActivity.Inventory | PlayerActivity.DebugFly), out this._activityLease, false, null);
		}

		// Token: 0x1700122C RID: 4652
		// (get) Token: 0x06006EAC RID: 28332 RVA: 0x002C847F File Offset: 0x002C667F
		private bool IsStableTripodActivityProfile
		{
			get
			{
				return this._tripodActive && !this._tripodTransitioning && !this._tripodPlacementActive && !this._tripodRestoreInProgress && !this._pendingTripodRestore;
			}
		}

		// Token: 0x1700122D RID: 4653
		// (get) Token: 0x06006EAD RID: 28333 RVA: 0x002C84AC File Offset: 0x002C66AC
		private bool TripodBodyIsOwned
		{
			get
			{
				return this.IsStableTripodActivityProfile && (this.HasActiveInventoryHandItem() || PlayerActivityGate.IsResourceOwned(PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.FullBodyAnimation | PlayerContextResource.HeldItem));
			}
		}

		// Token: 0x1700122E RID: 4654
		// (get) Token: 0x06006EAE RID: 28334 RVA: 0x002C84CC File Offset: 0x002C66CC
		private bool UserCameraExitBlocked
		{
			get
			{
				if (this._cameraCapture != null && this._cameraCapture.HasPendingRunCapture)
				{
					return true;
				}
				if (!this.IsStableTripodActivityProfile)
				{
					return false;
				}
				if (PlayerActivityGate.IsResourceOwned(PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.FullBodyAnimation))
				{
					return true;
				}
				if (!PlayerActivityGate.IsResourceOwned(PlayerContextResource.HeldItem))
				{
					return false;
				}
				HideFlashController resourceOwner = PlayerActivityGate.GetResourceOwner<HideFlashController>(PlayerContextResource.HeldItem);
				return resourceOwner == null || resourceOwner.IsExposureActive;
			}
		}

		// Token: 0x06006EAF RID: 28335 RVA: 0x002C852F File Offset: 0x002C672F
		private void ResolveEmoteSpeedController()
		{
			this._emoteSpeedController = ((this.playerAnimator != null) ? this.playerAnimator.GetComponentInChildren<MasturbationSpeedController>(true) : base.GetComponentInParent<MasturbationSpeedController>());
		}

		// Token: 0x06006EB0 RID: 28336 RVA: 0x002C855C File Offset: 0x002C675C
		private bool TryApplyActivityProfile(bool stableTripod)
		{
			PlayerActivityLease activityLease = this._activityLease;
			if (activityLease == null || !activityLease.IsActive)
			{
				return false;
			}
			PlayerContextResource playerContextResource = (stableTripod ? PlayerContextResource.Camera : this.DesiredHandheldResources);
			PlayerActivity playerActivity = (stableTripod ? (PlayerActivity.Cover | PlayerActivity.DebugFly) : (PlayerActivity.Emote | PlayerActivity.Cover | PlayerActivity.Flash | PlayerActivity.RapidHUD | PlayerActivity.Interact | PlayerActivity.Undress | PlayerActivity.LingerieOpen | PlayerActivity.Inventory | PlayerActivity.DebugFly));
			return PlayerActivityGate.TryUpdate(activityLease, stableTripod ? PlayerActivity.None : PlayerActivity.Phone, playerContextResource, playerActivity) && (this._activityLease == activityLease && activityLease.IsActive && activityLease.Resources == playerContextResource) && activityLease.BlockedActivities == playerActivity;
		}

		// Token: 0x06006EB1 RID: 28337 RVA: 0x002C85D6 File Offset: 0x002C67D6
		private bool TryApplyDesiredActivityProfile()
		{
			return this.TryApplyActivityProfile(this.IsStableTripodActivityProfile);
		}

		// Token: 0x06006EB2 RID: 28338 RVA: 0x002C85E4 File Offset: 0x002C67E4
		private void ReleaseActivityContext()
		{
			PlayerActivityLease activityLease = this._activityLease;
			this._activityLease = null;
			if (activityLease == null)
			{
				return;
			}
			activityLease.Dispose();
		}

		// Token: 0x06006EB3 RID: 28339 RVA: 0x002C85FD File Offset: 0x002C67FD
		private void OnSelfieStateChanged(bool selfieActive)
		{
			if (this.State != SmartphoneController.PhoneState.PhoneHeld)
			{
				return;
			}
			if (!selfieActive)
			{
				this.RequestSelfieDeactivation();
				return;
			}
			this._pendingSelfieDeactivate = false;
			this._pendingSelfieActivate = true;
			this._pendingSelfieActivateEarliestFrame = Mathf.Max(this._pendingSelfieActivateEarliestFrame, Time.frameCount + 1);
		}

		// Token: 0x06006EB4 RID: 28340 RVA: 0x002C863C File Offset: 0x002C683C
		private void ProcessPendingSelfieActivation()
		{
			if (!this._pendingSelfieActivate || Time.frameCount < this._pendingSelfieActivateEarliestFrame || GraphicsSettingsRuntimeUI.AnyOpen)
			{
				return;
			}
			this.ResolveSelfieNotifier();
			bool flag = this._selfieNotifier != null && this._selfieNotifier.IsLogicallyOpen && this._selfieNotifier.gameObject.activeInHierarchy;
			this._pendingSelfieActivate = false;
			if (!flag || this.State != SmartphoneController.PhoneState.PhoneHeld || this._selfieActive)
			{
				return;
			}
			if (this.playerAnimator != null && !string.IsNullOrEmpty(this.selfiePoseParam))
			{
				this.playerAnimator.SetBool(this.selfiePoseParam, true);
			}
			this.ApplySelfieCamera(true);
			if (this.debugMode)
			{
				Debug.Log("[SmartphoneController] Selfie ON differito e riconciliato.", this);
			}
		}

		// Token: 0x06006EB5 RID: 28341 RVA: 0x002C86FC File Offset: 0x002C68FC
		private void RequestSelfieDeactivation()
		{
			this._pendingSelfieActivate = false;
			if (!this._selfieActive && !this._pendingSelfieDeactivate)
			{
				return;
			}
			this._pendingSelfieDeactivate = true;
			this._pendingSelfieDeactivateEarliestFrame = Mathf.Max(this._pendingSelfieDeactivateEarliestFrame, Time.frameCount + 1);
		}

		// Token: 0x06006EB6 RID: 28342 RVA: 0x002C8738 File Offset: 0x002C6938
		private void ProcessPendingSelfieDeactivation()
		{
			if (!this._pendingSelfieDeactivate || Time.frameCount < this._pendingSelfieDeactivateEarliestFrame || GraphicsSettingsRuntimeUI.AnyOpen)
			{
				return;
			}
			this.ResolveSelfieNotifier();
			if (this._selfieNotifier != null && this._selfieNotifier.IsLogicallyOpen && this._selfieNotifier.gameObject.activeInHierarchy)
			{
				this._pendingSelfieDeactivate = false;
				return;
			}
			this._pendingSelfieDeactivate = false;
			if (this.State != SmartphoneController.PhoneState.PhoneHeld || !this._selfieActive)
			{
				return;
			}
			if (this.playerAnimator != null && !string.IsNullOrEmpty(this.selfiePoseParam))
			{
				this.playerAnimator.SetBool(this.selfiePoseParam, false);
			}
			this.ApplySelfieCamera(false);
			if (this.debugMode)
			{
				Debug.Log("[SmartphoneController] Selfie OFF differito e riconciliato.", this);
			}
		}

		// Token: 0x06006EB7 RID: 28343 RVA: 0x002C8800 File Offset: 0x002C6A00
		private void ExitSelfieToPhone()
		{
			if (this.UserCameraExitBlocked)
			{
				return;
			}
			this.DumpInputState("ExitSelfieToPhone ENTER");
			if (this._tripodActive || this._tripodTransitioning || this._tripodPlacementActive)
			{
				this.RestorePhoneFromTripodImmediate(true);
			}
			PhoneSelfieNotifier active = PhoneSelfieNotifier.Active;
			if (active == null)
			{
				return;
			}
			this.ResetDynamicIsland();
			foreach (Button button in active.GetComponentsInChildren<Button>(true))
			{
				if (button != null && button.name.ToLowerInvariant().Contains("back"))
				{
					button.onClick.Invoke();
					if (this.debugMode)
					{
						Debug.Log("[SmartphoneController] Selfie: ESC -> Back dell'app camera.", this);
					}
					return;
				}
			}
			active.gameObject.SetActive(false);
			if (this.debugMode)
			{
				Debug.Log("[SmartphoneController] Selfie: ESC -> pannello camera disattivato (fallback).", this);
			}
		}

		// Token: 0x06006EB8 RID: 28344 RVA: 0x002C88D0 File Offset: 0x002C6AD0
		private void ToggleSelfieFacing()
		{
			if (this._selfieCamAnchor == null)
			{
				return;
			}
			this._selfieRearFacing = !this._selfieRearFacing;
			if (this._selfieRearFacing)
			{
				this.ComputeRearCam(out this._selfieCamPos, out this._selfieBaseLocalRot);
				this._selfieCamPosTgt = this._selfieCamPos;
				this._selfieCamBaseRotTgt = this._selfieBaseLocalRot;
			}
			else
			{
				this._selfieCamPos = this._selfieCamPosSelfie;
				this._selfieBaseLocalRot = Quaternion.Euler(this._selfieCamEulNeutral);
				this._selfieCamPosTgt = this._selfieCamPosSelfie;
				this._selfieCamBaseRotTgt = this._selfieBaseLocalRot;
			}
			this._selfiePoseBlend = 1f;
			this._selfieYaw = 0f;
			this._selfiePitch = 0f;
			this._selfieCamAnchor.localPosition = this._selfieCamPos;
			this._selfieCamAnchor.localRotation = this._selfieBaseLocalRot;
			if (this.debugMode)
			{
				Debug.Log("[SmartphoneController] Camera: " + (this._selfieRearFacing ? "ESTERNA (rear)" : "INTERNA (selfie)") + ".", this);
			}
		}

		// Token: 0x06006EB9 RID: 28345 RVA: 0x002C89D8 File Offset: 0x002C6BD8
		private void ComputeRearCam(out Vector3 localPos, out Quaternion localRot)
		{
			this.ComputeRearCam(this._selfieCamPosSelfie, this._selfieCamEulNeutral, out localPos, out localRot);
		}

		// Token: 0x06006EBA RID: 28346 RVA: 0x002C89F0 File Offset: 0x002C6BF0
		private void ComputeRearCam(Vector3 selfiePos, Vector3 selfieEul, out Vector3 localPos, out Quaternion localRot)
		{
			if (this.rearCamTuned)
			{
				localPos = this.rearCamLocalPosition;
				localRot = Quaternion.Euler(this.rearCamLocalEuler);
				return;
			}
			Quaternion quaternion = Quaternion.Euler(selfieEul);
			localRot = quaternion * Quaternion.Euler(0f, 180f, 0f);
			Vector3 vector = localRot * Vector3.forward;
			localPos = selfiePos + vector * this.rearCamForwardPush;
		}

		// Token: 0x06006EBB RID: 28347 RVA: 0x002C8A78 File Offset: 0x002C6C78
		private void ApplySelfieCamera(bool selfieActive)
		{
			if (selfieActive && this.firstPersonController == null)
			{
				return;
			}
			if (selfieActive)
			{
				this._selfiePhonePos = this.selfiePhoneLocalPosition;
				this._selfiePhoneEul = this.selfiePhoneLocalEuler;
				this._selfiePhoneScale = this.selfiePhoneLocalScale;
				this._selfiePhonePosTgt = this._selfiePhonePos;
				this._selfiePhoneEulTgt = this._selfiePhoneEul;
				this._selfiePhoneScaleTgt = this._selfiePhoneScale;
				this.ApplyPhonePose(this._selfiePhonePos, this._selfiePhoneEul, this._selfiePhoneScale);
				if (this._selfieCamAnchor == null && this.phoneInstance != null)
				{
					GameObject gameObject = new GameObject("SelfieCamAnchor");
					this._selfieCamAnchor = gameObject.transform;
					this._selfieCamAnchor.SetParent(this.phoneInstance.transform, false);
				}
				if (this._selfieCamAnchor != null)
				{
					this._selfieRearFacing = false;
					this._selfieCamPos = this.selfieCamLocalPosition;
					this._selfieCamEulNeutral = this.selfieCamLocalEuler;
					this._selfieCamPosSelfie = this.selfieCamLocalPosition;
					this._selfieCamPosTgt = this.selfieCamLocalPosition;
					this._selfieCamBaseRotTgt = Quaternion.Euler(this.selfieCamLocalEuler);
					this._selfieCamAnchor.localPosition = this._selfieCamPos;
					this._selfieBaseLocalRot = Quaternion.Euler(this.selfieCamLocalEuler);
					this._selfieCamAnchor.localRotation = this._selfieBaseLocalRot;
					this.firstPersonController.CameraAnchorOverride = this._selfieCamAnchor;
				}
				this._selfieActive = true;
				this._selfieYaw = 0f;
				this._selfiePitch = 0f;
				this._headYaw = 0f;
				this._headPitch = 0f;
				this._selfieFov = this.selfieFovDefault;
				this._zoom01 = 0f;
				this._zoomTarget01 = 0f;
				if (this._mainCamera == null)
				{
					this._mainCamera = Camera.main;
				}
				if (this._mainCamera != null)
				{
					this._mainCamera.fieldOfView = this._selfieFov;
				}
				this.LockCursorForSelfie();
				this.SuppressInvectorJump(true);
				SelfieRadialMenuController selfieRadialMenuController = this.ResolveRadialMenu();
				if (this._cameraCapture != null)
				{
					if (!this._cameraCaptureConfigured)
					{
						this._cameraCapture.overlayUI = this.selfieCameraOverlay;
						this._cameraCapture.Configure(this._phoneCanvas, () => this.selfieFovMax / Mathf.Max(1f, this._selfieFov));
						this._cameraCapture.ConfigureTripod(new Action(this.ToggleTripodMode), () => this._tripodActive, () => this._tripodTransitioning, () => this._tripodPlacementActive, delegate
						{
							if (!(this.playerAnimator != null))
							{
								return null;
							}
							return this.playerAnimator.transform;
						}, () => !this.IsBenchBodyOwned);
						this._cameraCaptureConfigured = true;
					}
					this._cameraCapture.SetCameraMode(true);
				}
				if (this._posePicker != null)
				{
					this._posePicker.SetActive(true);
				}
				if (selfieRadialMenuController != null)
				{
					selfieRadialMenuController.SetActive(true);
				}
				this._ikBlend = 0f;
				this._ikBaseCaptured = false;
				if (this.enableSelfieHandIK && this._rigController != null && !this._rigController.IsCoverRigActive)
				{
					this._rigController.ActivateCoverRig();
					this._selfieHandIKActive = true;
					return;
				}
			}
			else
			{
				this._pendingSelfieActivate = false;
				this._pendingSelfieDeactivate = false;
				if (this._tripodActive || this._tripodTransitioning || this._tripodPlacementActive)
				{
					this.RestorePhoneFromTripodImmediate(true);
				}
				this._selfieActive = false;
				this._selfiePoseStarted = false;
				this.FreezeCiel(false);
				if (this._selfieMoveLocked && this._tpInput != null)
				{
					this._tpInput.lockMoveInput = this._selfieSavedLockMove;
				}
				this._selfieMoveLocked = false;
				if (this._playerEmotion != null)
				{
					this._playerEmotion.RestoreFacialExpression();
				}
				this.SuppressInvectorJump(false);
				if (this._posePicker != null)
				{
					this._posePicker.SetActive(false);
				}
				if (this._radialMenu != null)
				{
					this._radialMenu.SetActive(false);
				}
				this._poseMenuOpen = false;
				if (this._cameraCapture != null)
				{
					this._cameraCapture.SetCameraMode(false);
				}
				this._headYaw = 0f;
				this._headPitch = 0f;
				if (this._headAdjuster != null)
				{
					this._headAdjuster.Deactivate();
				}
				if (this.firstPersonController != null)
				{
					this.firstPersonController.CameraAnchorOverride = null;
				}
				this.ApplyPhonePose(this.attachLocalPosition, this.attachLocalEuler, this.attachLocalScale);
				if (this._mainCamera != null)
				{
					this._mainCamera.fieldOfView = this.fovZoomedOut;
				}
				if (this._selfieHandIKActive && this._rigController != null)
				{
					this._rigController.ZeroCoverLimb(LimbType.RightHand);
					this._rigController.DeactivateCoverRig();
				}
				this._selfieHandIKActive = false;
				if (this.State == SmartphoneController.PhoneState.PhoneHeld)
				{
					this._controllerPhoneViewReady = false;
					this._zoom01 = 0f;
					this._zoomTarget01 = 0f;
					if (this._mainCamera != null)
					{
						this._mainCamera.fieldOfView = this.fovZoomedOut;
					}
					if (this._cursorUnlocked)
					{
						this.SetCursorUnlocked(false);
					}
					else
					{
						Cursor.lockState = CursorLockMode.Locked;
						Cursor.visible = false;
						if (this._tpInput != null)
						{
							vThirdPersonInput.ForceCameraLock = false;
							this._tpInput.lockCameraInput = this._savedLockCameraInput;
						}
					}
				}
				this.DumpInputState("ApplySelfieCamera(false) EXIT");
			}
		}

		// Token: 0x06006EBC RID: 28348 RVA: 0x002C8FDC File Offset: 0x002C71DC
		private bool EnforceTripodDistanceLimit()
		{
			if (this.State != SmartphoneController.PhoneState.PhoneHeld || !this._tripodActive)
			{
				return false;
			}
			if ((((this._tpInput != null) ? this._tpInput.transform : ((this.playerAnimator != null) ? this.playerAnimator.transform : base.transform)).position - this._tripodGroundPosition).sqrMagnitude <= 400f)
			{
				return false;
			}
			this.ExitToGameplay();
			return true;
		}

		// Token: 0x06006EBD RID: 28349 RVA: 0x002C9060 File Offset: 0x002C7260
		public void ToggleTripodMode()
		{
			if (this.IsBenchBodyOwned || !this._selfieActive || this._tripodActive || this._tripodPlacementActive || this._tripodTransitioning || this.phoneInstance == null || this._selfieCamAnchor == null || this.firstPersonController == null)
			{
				return;
			}
			if (this._cameraCapture != null && (this._cameraCapture.IsRecordingVideo || this._cameraCapture.IsFinalizingVideo))
			{
				return;
			}
			this.BeginTripodPlacement();
		}

		// Token: 0x06006EBE RID: 28350 RVA: 0x002C90F0 File Offset: 0x002C72F0
		private GameObject EnsureTripodInstance()
		{
			if (this._tripodInstance != null)
			{
				return this._tripodInstance;
			}
			if (this.tripodPrefab != null)
			{
				this._tripodInstance = Object.Instantiate<GameObject>(this.tripodPrefab);
				this._tripodInstance.name = "PhoneTripod";
			}
			else
			{
				foreach (Transform transform in Object.FindObjectsOfType<Transform>(true))
				{
					if (!(transform == null) && !(transform.name != "Tripod") && transform.gameObject.scene.IsValid())
					{
						this._tripodInstance = transform.gameObject;
						break;
					}
				}
			}
			if (this._tripodInstance != null)
			{
				this._tripodBaseScale = this._tripodInstance.transform.localScale;
				this._tripodInstance.SetActive(false);
			}
			return this._tripodInstance;
		}

		// Token: 0x06006EBF RID: 28351 RVA: 0x002C91D0 File Offset: 0x002C73D0
		private void BeginTripodPlacement()
		{
			if (this.IsBenchBodyOwned)
			{
				return;
			}
			this.PrepareForTripodStateChange(true);
			GameObject gameObject = this.EnsureTripodInstance();
			if (gameObject == null)
			{
				Debug.LogError("[SmartphoneController] Prefab/oggetto Tripod non disponibile.", this);
				return;
			}
			if (this._tripodSurfaceDetector == null)
			{
				this._tripodSurfaceDetector = base.GetComponent<PlacementSurfaceDetector>();
			}
			if (this._tripodSurfaceDetector == null)
			{
				this._tripodSurfaceDetector = base.gameObject.AddComponent<PlacementSurfaceDetector>();
			}
			if (this._tripodPlacementGhost == null)
			{
				this._tripodPlacementGhost = base.GetComponent<PlacementGhost>();
			}
			if (this._tripodPlacementGhost == null)
			{
				this._tripodPlacementGhost = base.gameObject.AddComponent<PlacementGhost>();
			}
			GameObject gameObject2 = ((this.tripodPrefab != null) ? this.tripodPrefab : gameObject);
			this._tripodPlacementGhost.Spawn(gameObject2, null, null, false);
			this._tripodPlacementResult = PlacementResult.Invalid;
			this._tripodPlacementActive = true;
			this.firstPersonController.CameraAnchorOverride = null;
			this.SetTripodPlacementCharacterOcclusion(true);
			this.LockCursorForSelfie();
			if (this._posePicker != null)
			{
				this._posePicker.SelectPose(0);
			}
			if (this._selfieMoveLocked && this._tpInput != null)
			{
				this._tpInput.lockMoveInput = this._selfieSavedLockMove;
			}
			this._selfieMoveLocked = false;
			SelfieRadialMenuController selfieRadialMenuController = this.ResolveRadialMenu();
			if (selfieRadialMenuController != null)
			{
				selfieRadialMenuController.SetActive(false);
			}
			if (this.debugMode)
			{
				Debug.Log("[SmartphoneController] Piazzamento treppiede: preview FP attiva.", this);
			}
		}

		// Token: 0x06006EC0 RID: 28352 RVA: 0x002C9348 File Offset: 0x002C7548
		private void UpdateTripodPlacement()
		{
			if (!this._tripodPlacementActive)
			{
				return;
			}
			if (this._mainCamera == null)
			{
				this._mainCamera = Camera.main;
			}
			if (Input.GetKeyDown(this.selfieExitKey) || EscapeInputCoordinator.TryConsume(this) || GameplayInput.GetMouseButtonDown(1))
			{
				this.CancelTripodPlacement(true);
				return;
			}
			if (this._mainCamera == null || this._tripodSurfaceDetector == null)
			{
				this._tripodPlacementResult = PlacementResult.Invalid;
				if (this._tripodPlacementGhost != null)
				{
					this._tripodPlacementGhost.SetVisible(false);
				}
				return;
			}
			float num = Mathf.Max(3f, this.tripodPlacementDistance);
			Transform transform = ((this.playerAnimator != null) ? this.playerAnimator.transform : base.transform.root);
			PlacementResult placementResult = this._tripodSurfaceDetector.DetectSurface(this._mainCamera, this.tripodPlacementMask, num, transform, new Vector3?(this._mainCamera.transform.position), null);
			if (placementResult.IsValid)
			{
				bool flag = placementResult.SurfaceType == PlacementSurfaceType.Floor && Vector3.Dot(placementResult.HitNormal, Vector3.up) >= 0.72f;
				Vector3 vector = transform.position - placementResult.Position;
				vector.y = 0f;
				if (vector.sqrMagnitude > 0.001f)
				{
					placementResult.Rotation = Quaternion.LookRotation(vector.normalized, Vector3.up);
				}
				placementResult.IsValid = flag && this.TripodPlacementIsClear(placementResult.Position);
				this._tripodPlacementResult = placementResult;
				this._tripodPlacementGhost.UpdateTarget(placementResult.Position, placementResult.Rotation, placementResult.IsValid, null);
			}
			else
			{
				this._tripodPlacementResult = PlacementResult.Invalid;
				if (this._tripodPlacementGhost != null)
				{
					this._tripodPlacementGhost.SetVisible(false);
				}
			}
			if (GameplayInput.GetMouseButtonDown(0) && this._tripodPlacementResult.IsValid)
			{
				PlacementResult tripodPlacementResult = this._tripodPlacementResult;
				this.EndTripodPlacementPreview(false);
				this._tripodTransition = base.StartCoroutine(this.DeployPhoneToTripod(tripodPlacementResult.Position, tripodPlacementResult.Rotation));
			}
		}

		// Token: 0x06006EC1 RID: 28353 RVA: 0x002C9570 File Offset: 0x002C7770
		private void CancelTripodPlacement(bool smoothCamera)
		{
			if (!this._tripodPlacementActive)
			{
				return;
			}
			Vector3 vector = ((this._mainCamera != null) ? this._mainCamera.transform.position : ((this._selfieCamAnchor != null) ? this._selfieCamAnchor.position : Vector3.zero));
			Quaternion quaternion = ((this._mainCamera != null) ? this._mainCamera.transform.rotation : ((this._selfieCamAnchor != null) ? this._selfieCamAnchor.rotation : Quaternion.identity));
			this.EndTripodPlacementPreview(false);
			SelfieRadialMenuController selfieRadialMenuController = this.ResolveRadialMenu();
			if (selfieRadialMenuController != null)
			{
				selfieRadialMenuController.SetPoseSlotEnabled(true);
				selfieRadialMenuController.SetActive(true);
			}
			if (smoothCamera && this._selfieCamAnchor != null && this.firstPersonController != null)
			{
				this._tripodTransition = base.StartCoroutine(this.RestoreHandheldCameraAfterPlacement(vector, quaternion));
				return;
			}
			if (this.firstPersonController != null)
			{
				this.firstPersonController.CameraAnchorOverride = this._selfieCamAnchor;
				this.SetTripodPlacementCharacterOcclusion(false);
			}
		}

		// Token: 0x06006EC2 RID: 28354 RVA: 0x002C9687 File Offset: 0x002C7887
		private void EndTripodPlacementPreview(bool restoreCharacterVisibility = true)
		{
			this._tripodPlacementActive = false;
			this._tripodPlacementResult = PlacementResult.Invalid;
			if (this._tripodPlacementGhost != null)
			{
				this._tripodPlacementGhost.Destroy();
			}
			if (restoreCharacterVisibility)
			{
				this.SetTripodPlacementCharacterOcclusion(false);
			}
		}

		// Token: 0x06006EC3 RID: 28355 RVA: 0x002C96BE File Offset: 0x002C78BE
		private IEnumerator RestoreHandheldCameraAfterPlacement(Vector3 fromPosition, Quaternion fromRotation)
		{
			this._tripodTransitioning = true;
			yield return this.BlendTripodCamera(fromPosition, fromRotation, this._selfieCamAnchor);
			this.SetTripodPlacementCharacterOcclusion(false);
			this._tripodTransitioning = false;
			this._tripodTransition = null;
			yield break;
		}

		// Token: 0x06006EC4 RID: 28356 RVA: 0x002C96DC File Offset: 0x002C78DC
		private void SetTripodPlacementCharacterOcclusion(bool hidden)
		{
			if (!hidden)
			{
				foreach (KeyValuePair<Renderer, ShadowCastingMode> keyValuePair in this._tripodPlacementHiddenRenderers)
				{
					if (keyValuePair.Key != null)
					{
						keyValuePair.Key.shadowCastingMode = keyValuePair.Value;
					}
				}
				this._tripodPlacementHiddenRenderers.Clear();
				return;
			}
			this.SetTripodPlacementCharacterOcclusion(false);
			if (this.playerAnimator == null)
			{
				return;
			}
			foreach (Renderer renderer in this.playerAnimator.GetComponentsInChildren<Renderer>(true))
			{
				if (!(renderer == null) && SmartphoneController.IsHairRenderer(renderer))
				{
					this._tripodPlacementHiddenRenderers[renderer] = renderer.shadowCastingMode;
					renderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
				}
			}
		}

		// Token: 0x06006EC5 RID: 28357 RVA: 0x002C97C0 File Offset: 0x002C79C0
		private static bool IsHairRenderer(Renderer renderer)
		{
			Outfit componentInParent = renderer.GetComponentInParent<Outfit>();
			if (componentInParent != null && componentInParent.Type != null && SmartphoneController.ContainsIgnoreCase(componentInParent.Type.TypeName, "hair"))
			{
				return true;
			}
			if (SmartphoneController.ContainsIgnoreCase(renderer.name, "hair") && !SmartphoneController.ContainsIgnoreCase(renderer.name, "pubic"))
			{
				return true;
			}
			foreach (Material material in renderer.sharedMaterials)
			{
				if (material != null && SmartphoneController.ContainsIgnoreCase(material.name, "hair") && !SmartphoneController.ContainsIgnoreCase(material.name, "pubic"))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006EC6 RID: 28358 RVA: 0x0002E9C8 File Offset: 0x0002CBC8
		private static bool ContainsIgnoreCase(string value, string token)
		{
			return !string.IsNullOrEmpty(value) && value.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x06006EC7 RID: 28359 RVA: 0x002C9874 File Offset: 0x002C7A74
		private bool TripodPlacementIsClear(Vector3 groundPoint)
		{
			Vector3 vector = groundPoint + Vector3.up * 0.38f;
			Vector3 vector2 = groundPoint + Vector3.up * 1.38f;
			foreach (Collider collider in Physics.OverlapCapsule(vector, vector2, 0.26f, this.tripodPlacementMask, QueryTriggerInteraction.Ignore))
			{
				if (!(collider == null))
				{
					Transform transform = collider.transform;
					if ((!(this.playerAnimator != null) || !transform.IsChildOf(this.playerAnimator.transform)) && (!(this._tripodInstance != null) || !transform.IsChildOf(this._tripodInstance.transform)))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06006EC8 RID: 28360 RVA: 0x002C992E File Offset: 0x002C7B2E
		private IEnumerator DeployPhoneToTripod(Vector3 tripodPosition, Quaternion tripodRotation)
		{
			this._tripodTransitioning = true;
			this._tripodActive = true;
			this._tripodGroundPosition = tripodPosition;
			this._tripodHeightScale = 1f;
			this._tripodInstance.transform.localScale = this._tripodBaseScale;
			this._tripodInstance.transform.SetPositionAndRotation(tripodPosition, tripodRotation);
			this._tripodInstance.SetActive(true);
			Vector3 vector = tripodPosition + Vector3.up * this.tripodCameraHeight;
			Vector3 vector2 = this.playerAnimator.transform.position + Vector3.up * this.tripodFramingHeight - vector;
			Quaternion quaternion = ((vector2.sqrMagnitude > 0.001f) ? Quaternion.LookRotation(vector2.normalized, Vector3.up) : tripodRotation);
			Camera camera = ((this._mainCamera != null) ? this._mainCamera : Camera.main);
			Vector3 vector3 = ((camera != null) ? camera.transform.position : this._selfieCamAnchor.position);
			Quaternion quaternion2 = ((camera != null) ? camera.transform.rotation : this._selfieCamAnchor.rotation);
			this._phoneParentBeforeTripod = this.phoneInstance.transform.parent;
			this._phoneWorldScaleOnTripod = this.phoneInstance.transform.lossyScale;
			this._selfieYaw = 0f;
			this._selfiePitch = 0f;
			this._selfieBaseLocalRot = this._selfieCamAnchor.localRotation;
			this.phoneInstance.transform.SetParent(this._tripodInstance.transform, true);
			this.CompensatePhoneScaleOnTripod();
			this.phoneInstance.transform.rotation = quaternion * Quaternion.Inverse(this._selfieBaseLocalRot);
			this.phoneInstance.transform.position = vector - this.phoneInstance.transform.TransformVector(this._selfieCamAnchor.localPosition);
			this.SetPhoneHeldPoseForTripod(true);
			yield return this.BlendTripodCamera(vector3, quaternion2, this._selfieCamAnchor);
			this.SetTripodPlacementCharacterOcclusion(false);
			this._tripodTransitioning = false;
			this._tripodTransition = null;
			if (!this.TryApplyActivityProfile(true) && this.debugMode)
			{
				Debug.LogWarning("[SmartphoneController] Impossibile applicare il profilo tripod stabile; mantengo il profilo restrittivo.", this);
			}
			if (this.debugMode)
			{
				Debug.Log(string.Format("[SmartphoneController] Treppiede piazzato a {0}.", tripodPosition), this);
			}
			yield break;
		}

		// Token: 0x06006EC9 RID: 28361 RVA: 0x002C994B File Offset: 0x002C7B4B
		private IEnumerator ReturnPhoneFromTripod()
		{
			this.PrepareForTripodStateChange(true);
			this._tripodTransitioning = true;
			Camera camera = ((this._mainCamera != null) ? this._mainCamera : Camera.main);
			Vector3 vector = ((camera != null) ? camera.transform.position : this._selfieCamAnchor.position);
			Quaternion quaternion = ((camera != null) ? camera.transform.rotation : this._selfieCamAnchor.rotation);
			if (!this.TryRestorePhoneToHandTransform())
			{
				this._tripodTransitioning = false;
				this._tripodTransition = null;
				this.RequestTripodRestore(true);
				yield break;
			}
			this._tripodActive = false;
			this.SetPhoneHeldPoseForTripod(false);
			yield return this.BlendTripodCamera(vector, quaternion, this._selfieCamAnchor);
			if (this._tripodInstance != null)
			{
				this._tripodInstance.SetActive(false);
			}
			this._tripodTransitioning = false;
			this._tripodTransition = null;
			if (this.debugMode)
			{
				Debug.Log("[SmartphoneController] Telefono ripreso dal treppiede.", this);
			}
			yield break;
		}

		// Token: 0x06006ECA RID: 28362 RVA: 0x002C995C File Offset: 0x002C7B5C
		private void UpdateTripodHeight()
		{
			if (!this._tripodActive || this._tripodTransitioning || this._tripodInstance == null || this.phoneInstance == null || this._selfieCamAnchor == null)
			{
				return;
			}
			float num = 0f;
			if (GameplayInput.GetKey(KeyCode.UpArrow))
			{
				num += 1f;
			}
			if (GameplayInput.GetKey(KeyCode.DownArrow))
			{
				num -= 1f;
			}
			if (Mathf.Approximately(num, 0f))
			{
				return;
			}
			this.AdjustTripodHeight(num * this.tripodHeightScaleSpeed * Time.deltaTime);
		}

		// Token: 0x06006ECB RID: 28363 RVA: 0x002C99F4 File Offset: 0x002C7BF4
		private void AdjustTripodHeight(float scaleDelta)
		{
			if (!this._tripodActive || this._tripodInstance == null || this.phoneInstance == null || this._selfieCamAnchor == null)
			{
				return;
			}
			float num = Mathf.Min(this.tripodMinHeightScale, this.tripodMaxHeightScale);
			float num2 = Mathf.Max(this.tripodMinHeightScale, this.tripodMaxHeightScale);
			float num3 = Mathf.Clamp(this._tripodHeightScale + scaleDelta, num, num2);
			if (Mathf.Approximately(num3, this._tripodHeightScale))
			{
				return;
			}
			Quaternion rotation = this._selfieCamAnchor.rotation;
			this._tripodHeightScale = num3;
			this._tripodInstance.transform.localScale = this._tripodBaseScale * this._tripodHeightScale;
			this.CompensatePhoneScaleOnTripod();
			Vector3 vector = this._tripodGroundPosition + Vector3.up * (this.tripodCameraHeight * this._tripodHeightScale);
			this.phoneInstance.transform.rotation = rotation * Quaternion.Inverse(this._selfieCamAnchor.localRotation);
			this.phoneInstance.transform.position = vector - this.phoneInstance.transform.TransformVector(this._selfieCamAnchor.localPosition);
		}

		// Token: 0x06006ECC RID: 28364 RVA: 0x002C9B30 File Offset: 0x002C7D30
		private void CompensatePhoneScaleOnTripod()
		{
			if (this.phoneInstance == null || this._tripodInstance == null)
			{
				return;
			}
			Vector3 lossyScale = this._tripodInstance.transform.lossyScale;
			this.phoneInstance.transform.localScale = new Vector3(SmartphoneController.SafeScaleRatio(this._phoneWorldScaleOnTripod.x, lossyScale.x), SmartphoneController.SafeScaleRatio(this._phoneWorldScaleOnTripod.y, lossyScale.y), SmartphoneController.SafeScaleRatio(this._phoneWorldScaleOnTripod.z, lossyScale.z));
		}

		// Token: 0x06006ECD RID: 28365 RVA: 0x002C9BC2 File Offset: 0x002C7DC2
		private static float SafeScaleRatio(float worldScale, float parentScale)
		{
			if (Mathf.Abs(parentScale) <= 0.0001f)
			{
				return worldScale;
			}
			return worldScale / parentScale;
		}

		// Token: 0x06006ECE RID: 28366 RVA: 0x002C9BD6 File Offset: 0x002C7DD6
		private IEnumerator BlendTripodCamera(Vector3 fromPosition, Quaternion fromRotation, Transform target)
		{
			Vector3 logicalFromPosition = WorldOrigin.ToLogical(fromPosition);
			this.EnsureTripodCameraBlendAnchor();
			this._tripodCameraBlendAnchor.SetPositionAndRotation(fromPosition, fromRotation);
			this.firstPersonController.CameraAnchorOverride = this._tripodCameraBlendAnchor;
			float duration = Mathf.Max(0.05f, this.tripodCameraTransitionDuration);
			float elapsed = 0f;
			while (elapsed < duration && target != null)
			{
				elapsed += Time.unscaledDeltaTime;
				float num = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / duration));
				this._tripodCameraBlendAnchor.position = Vector3.Lerp(WorldOrigin.ToUnity(logicalFromPosition), target.position, num);
				this._tripodCameraBlendAnchor.rotation = Quaternion.Slerp(fromRotation, target.rotation, num);
				yield return null;
			}
			if (target != null)
			{
				this._tripodCameraBlendAnchor.SetPositionAndRotation(target.position, target.rotation);
				this.firstPersonController.CameraAnchorOverride = target;
			}
			yield break;
		}

		// Token: 0x06006ECF RID: 28367 RVA: 0x002C9BFC File Offset: 0x002C7DFC
		private void EnsureTripodCameraBlendAnchor()
		{
			if (this._tripodCameraBlendAnchor != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("TripodCameraBlendAnchor");
			this._tripodCameraBlendAnchor = gameObject.transform;
		}

		// Token: 0x06006ED0 RID: 28368 RVA: 0x002C9C30 File Offset: 0x002C7E30
		private bool TryRestorePhoneToHandTransform()
		{
			if (this.phoneInstance == null)
			{
				return true;
			}
			Transform transform = ((this._phoneParentBeforeTripod != null) ? this._phoneParentBeforeTripod : this._handBone);
			if (transform == null)
			{
				if (this.debugMode)
				{
					Debug.LogWarning("[SmartphoneController] Restore tripod rinviato: parent mano non disponibile.", this);
				}
				return false;
			}
			try
			{
				if (this.phoneInstance.transform.parent != transform)
				{
					this.phoneInstance.transform.SetParent(transform, false);
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[SmartphoneController] Restore tripod rinviato: " + ex.Message, this);
				return false;
			}
			if (this.phoneInstance.transform.parent != transform)
			{
				Debug.LogWarning("[SmartphoneController] Restore tripod rinviato: SetParent non applicato.", this);
				return false;
			}
			if (this._selfiePhoneScaleTgt.sqrMagnitude < 1E-06f)
			{
				this._selfiePhonePosTgt = this.selfiePhoneLocalPosition;
				this._selfiePhoneEulTgt = this.selfiePhoneLocalEuler;
				this._selfiePhoneScaleTgt = this.selfiePhoneLocalScale;
			}
			this._selfiePhonePos = this._selfiePhonePosTgt;
			this._selfiePhoneEul = this._selfiePhoneEulTgt;
			this._selfiePhoneScale = this._selfiePhoneScaleTgt;
			this.ApplyPhonePose(this._selfiePhonePos, this._selfiePhoneEul, this._selfiePhoneScale);
			this._selfieRearFacing = false;
			this._selfieCamPos = this._selfieCamPosTgt;
			this._selfieBaseLocalRot = this._selfieCamBaseRotTgt;
			this._selfieYaw = 0f;
			this._selfiePitch = 0f;
			this._selfiePoseBlend = 1f;
			if (this._selfieCamAnchor != null)
			{
				this._selfieCamAnchor.localPosition = this._selfieCamPos;
				this._selfieCamAnchor.localRotation = this._selfieBaseLocalRot;
			}
			return true;
		}

		// Token: 0x06006ED1 RID: 28369 RVA: 0x002C9DF0 File Offset: 0x002C7FF0
		private void SetPhoneHeldPoseForTripod(bool onTripod)
		{
			if (this.playerAnimator != null && !string.IsNullOrEmpty(this.selfiePoseParam))
			{
				this.playerAnimator.SetBool(this.selfiePoseParam, !onTripod && this._selfieActive);
			}
			this._ikBlend = 0f;
			this._ikBaseCaptured = false;
			if (onTripod)
			{
				this.SetPhoneHeldParam(false);
				this._holdLayerTargetWeight = 0f;
				if (this.playerAnimator != null && this._holdLayerIndex >= 0)
				{
					this.playerAnimator.SetLayerWeight(this._holdLayerIndex, 0f);
				}
				this._owningHoldLayer = false;
				if (this._selfieHandIKActive && this._rigController != null)
				{
					this._rigController.ZeroCoverLimb(LimbType.RightHand);
					this._rigController.DeactivateCoverRig();
				}
				this._selfieHandIKActive = false;
				if (this._posePicker != null)
				{
					this._posePicker.SelectPose(0);
				}
				SelfieRadialMenuController selfieRadialMenuController = this.ResolveRadialMenu();
				if (selfieRadialMenuController != null)
				{
					selfieRadialMenuController.ResetPoseSelection();
					selfieRadialMenuController.SetPoseSlotEnabled(false);
					selfieRadialMenuController.SetActive(true);
					selfieRadialMenuController.SetPersistentHudVisible(false);
				}
				this.SetTripodMinimalHud(true);
				return;
			}
			this.SetTripodMinimalHud(false);
			this.SetConfiguredUiHidden(true);
			this.SetPhoneHeldParam(true);
			this._holdLayerTargetWeight = 1f;
			this._owningHoldLayer = true;
			SelfieRadialMenuController selfieRadialMenuController2 = this.ResolveRadialMenu();
			if (selfieRadialMenuController2 != null)
			{
				selfieRadialMenuController2.SetPersistentHudVisible(true);
				selfieRadialMenuController2.SetPoseSlotEnabled(true);
				selfieRadialMenuController2.SetActive(this._selfieActive);
			}
			if (this._selfieActive && this.enableSelfieHandIK && this._rigController != null && !this._rigController.IsCoverRigActive)
			{
				this._rigController.ActivateCoverRig();
				this._selfieHandIKActive = true;
			}
		}

		// Token: 0x06006ED2 RID: 28370 RVA: 0x002C9FA4 File Offset: 0x002C81A4
		private void RestorePhoneFromTripodImmediate(bool returnToPhoneHeld)
		{
			if (GraphicsSettingsRuntimeUI.AnyOpen)
			{
				this.RequestTripodRestore(returnToPhoneHeld);
				return;
			}
			if (this._tripodRestoreInProgress)
			{
				this.RequestTripodRestore(returnToPhoneHeld);
				return;
			}
			this._tripodRestoreInProgress = true;
			try
			{
				this.PrepareForTripodStateChange(true);
				if (returnToPhoneHeld)
				{
					HideFlashController resourceOwner = PlayerActivityGate.GetResourceOwner<HideFlashController>(PlayerContextResource.HeldItem);
					if (resourceOwner != null)
					{
						resourceOwner.InterruptForWorldInteraction();
					}
					if (this.HasActiveInventoryHandItem())
					{
						this._inventoryManager.StowHeldItemForInteraction();
					}
				}
				if (returnToPhoneHeld && !this.TryApplyActivityProfile(false))
				{
					this.RequestTripodRestore(true);
				}
				else
				{
					if (this._tripodTransition != null)
					{
						base.StopCoroutine(this._tripodTransition);
						this._tripodTransition = null;
					}
					this.EndTripodPlacementPreview(true);
					bool flag = this.phoneInstance != null && this._tripodInstance != null && this.phoneInstance.transform.IsChildOf(this._tripodInstance.transform);
					if ((this._tripodActive || flag) && !this.TryRestorePhoneToHandTransform())
					{
						this.RequestTripodRestore(returnToPhoneHeld);
					}
					else
					{
						this._pendingTripodRestore = false;
						this._tripodActive = false;
						this._tripodTransitioning = false;
						if (this._tripodInstance != null)
						{
							this._tripodInstance.SetActive(false);
						}
						this.SetTripodMinimalHud(false);
						this.SetConfiguredUiHidden(this.State == SmartphoneController.PhoneState.PhoneHeld || returnToPhoneHeld);
						if (this._radialMenu != null)
						{
							this._radialMenu.SetPersistentHudVisible(true);
							this._radialMenu.SetPoseSlotEnabled(true);
						}
						this.SetPhoneHeldParam(returnToPhoneHeld);
						this._holdLayerTargetWeight = (returnToPhoneHeld ? 1f : 0f);
						this._owningHoldLayer = returnToPhoneHeld || !this.HasActiveInventoryHandItem();
						if (this.playerAnimator != null && !string.IsNullOrEmpty(this.selfiePoseParam))
						{
							this.playerAnimator.SetBool(this.selfiePoseParam, false);
						}
						if (this._selfieHandIKActive && this._rigController != null)
						{
							this._rigController.ZeroCoverLimb(LimbType.RightHand);
							this._rigController.DeactivateCoverRig();
						}
						this._selfieHandIKActive = false;
						if (this.firstPersonController != null)
						{
							this.firstPersonController.CameraAnchorOverride = null;
						}
					}
				}
			}
			finally
			{
				this._tripodRestoreInProgress = false;
			}
		}

		// Token: 0x06006ED3 RID: 28371 RVA: 0x002CA1DC File Offset: 0x002C83DC
		private void RequestTripodRestore(bool returnToPhoneHeld)
		{
			bool pendingTripodRestore = this._pendingTripodRestore;
			this._pendingTripodRestore = true;
			this._pendingTripodReturnToPhoneHeld = (pendingTripodRestore ? (this._pendingTripodReturnToPhoneHeld && returnToPhoneHeld) : returnToPhoneHeld);
			if (this.State == SmartphoneController.PhoneState.Gameplay)
			{
				this._pendingTripodReturnToPhoneHeld = false;
			}
			this._pendingTripodRestoreEarliestFrame = Mathf.Max(this._pendingTripodRestoreEarliestFrame, Time.frameCount + 1);
		}

		// Token: 0x06006ED4 RID: 28372 RVA: 0x002CA234 File Offset: 0x002C8434
		private void ProcessPendingTripodRestore()
		{
			if (!this._pendingTripodRestore || Time.frameCount < this._pendingTripodRestoreEarliestFrame || GraphicsSettingsRuntimeUI.AnyOpen)
			{
				return;
			}
			bool flag = this.State == SmartphoneController.PhoneState.PhoneHeld && this._pendingTripodReturnToPhoneHeld;
			this._pendingTripodRestore = false;
			this._pendingTripodReturnToPhoneHeld = false;
			this.RestorePhoneFromTripodImmediate(flag);
		}

		// Token: 0x06006ED5 RID: 28373 RVA: 0x002CA288 File Offset: 0x002C8488
		public void ApplySelfiePoseOffsets(SelfiePoseConfigSO.PoseEntry entry)
		{
			if (!this._selfieActive)
			{
				return;
			}
			this.ResetSelfieHandAdjustment();
			Vector3 phoneLocalPosition = this.selfiePhoneLocalPosition;
			Vector3 phoneLocalEuler = this.selfiePhoneLocalEuler;
			Vector3 phoneLocalScale = this.selfiePhoneLocalScale;
			Vector3 camLocalPosition = this.selfieCamLocalPosition;
			Vector3 camLocalEuler = this.selfieCamLocalEuler;
			if (entry != null)
			{
				phoneLocalPosition = entry.phoneLocalPosition;
				phoneLocalEuler = entry.phoneLocalEuler;
				phoneLocalScale = entry.phoneLocalScale;
				camLocalPosition = entry.camLocalPosition;
				camLocalEuler = entry.camLocalEuler;
			}
			this._selfiePoseBlendDur = Mathf.Max(0f, this.selfiePoseOffsetBlendDuration);
			this._selfiePhonePosTgt = phoneLocalPosition;
			this._selfiePhoneEulTgt = phoneLocalEuler;
			this._selfiePhoneScaleTgt = phoneLocalScale;
			this._selfieCamEulNeutral = camLocalEuler;
			this._selfieCamPosSelfie = camLocalPosition;
			if (this._selfieRearFacing)
			{
				this.ComputeRearCam(camLocalPosition, camLocalEuler, out this._selfieCamPosTgt, out this._selfieCamBaseRotTgt);
			}
			else
			{
				this._selfieCamPosTgt = camLocalPosition;
				this._selfieCamBaseRotTgt = Quaternion.Euler(camLocalEuler);
			}
			if (this._tripodActive)
			{
				this._selfiePoseStarted = true;
				return;
			}
			if (this._selfiePoseBlendDur <= 0f || !this._selfiePoseStarted)
			{
				this._selfiePhonePos = phoneLocalPosition;
				this._selfiePhoneEul = phoneLocalEuler;
				this._selfiePhoneScale = phoneLocalScale;
				this._selfieCamPos = camLocalPosition;
				this._selfieBaseLocalRot = this._selfieCamBaseRotTgt;
				this._selfiePoseBlend = 1f;
				this.ApplyPhonePose(this._selfiePhonePos, this._selfiePhoneEul, this._selfiePhoneScale);
				if (this._selfieCamAnchor != null)
				{
					this._selfieCamAnchor.localPosition = this._selfieCamPos;
					this._selfieCamAnchor.localRotation = this._selfieBaseLocalRot * Quaternion.Euler(this._selfiePitch, this._selfieYaw, 0f);
				}
			}
			else
			{
				this._selfiePoseBlend = 0f;
			}
			this._selfiePoseStarted = true;
		}

		// Token: 0x06006ED6 RID: 28374 RVA: 0x002CA42E File Offset: 0x002C862E
		public void ResetSelfieLookForAuthoring()
		{
			this._selfieYaw = 0f;
			this._selfiePitch = 0f;
			if (this._selfieCamAnchor != null)
			{
				this._selfieCamAnchor.localRotation = this._selfieBaseLocalRot;
			}
		}

		// Token: 0x1700122F RID: 4655
		// (get) Token: 0x06006ED7 RID: 28375 RVA: 0x002CA465 File Offset: 0x002C8665
		public bool IsRearFacing
		{
			get
			{
				return this._selfieRearFacing;
			}
		}

		// Token: 0x06006ED8 RID: 28376 RVA: 0x002CA46D File Offset: 0x002C866D
		public void SetRearFacing(bool rear)
		{
			if (this._selfieRearFacing != rear)
			{
				this.ToggleSelfieFacing();
			}
		}

		// Token: 0x06006ED9 RID: 28377 RVA: 0x002CA480 File Offset: 0x002C8680
		public void LoadRearCamToLive()
		{
			if (this._selfieCamAnchor == null || !this._selfieRearFacing)
			{
				return;
			}
			this._selfieYaw = 0f;
			this._selfiePitch = 0f;
			this.ComputeRearCam(out this._selfieCamPos, out this._selfieBaseLocalRot);
			this._selfieCamPosTgt = this._selfieCamPos;
			this._selfieCamBaseRotTgt = this._selfieBaseLocalRot;
			this._selfieCamAnchor.localPosition = this._selfieCamPos;
			this._selfieCamAnchor.localRotation = this._selfieBaseLocalRot;
		}

		// Token: 0x06006EDA RID: 28378 RVA: 0x002CA508 File Offset: 0x002C8708
		public bool CaptureRearCamFromLive(out Vector3 pos, out Vector3 euler)
		{
			pos = this.rearCamLocalPosition;
			euler = this.rearCamLocalEuler;
			if (this._selfieCamAnchor == null || !this._selfieRearFacing)
			{
				return false;
			}
			this.ResetSelfieLookForAuthoring();
			pos = this._selfieCamAnchor.localPosition;
			euler = this._selfieCamAnchor.localRotation.eulerAngles;
			this.rearCamLocalPosition = pos;
			this.rearCamLocalEuler = euler;
			this.rearCamTuned = true;
			return true;
		}

		// Token: 0x06006EDB RID: 28379 RVA: 0x002CA598 File Offset: 0x002C8798
		private void OnPoseMenuVisibilityChanged(bool menuOpen)
		{
			if (!this._selfieActive)
			{
				this._poseMenuOpen = false;
				return;
			}
			this._poseMenuOpen = menuOpen;
			this.DumpInputState(menuOpen ? "PoseMenu.Open ENTER" : "PoseMenu.Close ENTER");
			if (menuOpen)
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = !ControllerInput.IsActive;
				if (this._tpInput != null)
				{
					this._tpInput.lockCameraInput = true;
					vThirdPersonInput.ForceCameraLock = true;
				}
			}
			else
			{
				if (this._tpInput != null)
				{
					vThirdPersonInput.ForceCameraLock = false;
					this._tpInput.lockCameraInput = false;
				}
				this.LockCursorForSelfie();
			}
			this.DumpInputState(menuOpen ? "PoseMenu.Open EXIT" : "PoseMenu.Close EXIT");
		}

		// Token: 0x06006EDC RID: 28380 RVA: 0x002CA644 File Offset: 0x002C8844
		private void SuppressInvectorJump(bool suppress)
		{
			if (this._tpInput == null)
			{
				return;
			}
			if (suppress)
			{
				if (this._jumpSuppressed)
				{
					return;
				}
				this._savedJumpUseInput = this._tpInput.jumpInput.useInput;
				this._tpInput.jumpInput.useInput = false;
				this._jumpSuppressed = true;
				return;
			}
			else
			{
				if (!this._jumpSuppressed)
				{
					return;
				}
				this._tpInput.jumpInput.useInput = this._savedJumpUseInput;
				this._jumpSuppressed = false;
				return;
			}
		}

		// Token: 0x06006EDD RID: 28381 RVA: 0x002CA6C1 File Offset: 0x002C88C1
		private void LockCursorForSelfie()
		{
			if (this._cursorUnlocked)
			{
				this.SetCursorUnlocked(false);
			}
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		// Token: 0x06006EDE RID: 28382 RVA: 0x002CA6E0 File Offset: 0x002C88E0
		private void UpdateSelfiePhotoMode()
		{
			if (this._mainCamera == null)
			{
				this._mainCamera = Camera.main;
			}
			if (this.SuspendPhonePoseReapply)
			{
				return;
			}
			if (this._tripodPlacementActive)
			{
				this.UpdateTripodPlacement();
				return;
			}
			if (this._tripodTransitioning)
			{
				return;
			}
			if (PhoneCameraCapture.CameraInputSuppressed)
			{
				return;
			}
			bool tripodBodyIsOwned = this.TripodBodyIsOwned;
			if (GameplayInput.GetKeyDown(KeyCode.R) && this.SelfiePoseIsDefault && !this._tripodActive)
			{
				this.ToggleSelfieFacing();
			}
			if (this._poseMenuOpen)
			{
				return;
			}
			if (this._tripodActive && this._tripodRapidActions != null && this._tripodRapidActions.IsRebindOpen)
			{
				return;
			}
			bool flag = !this.UserCameraExitBlocked && ControllerInput.IsControllerActive && ControllerInput.CancelPressed && (!this.IsStableTripodActivityProfile || !(PlayerInventoryManager.Instance != null) || !PlayerInventoryManager.Instance.CanClearControllerSelection) && !ControllerInput.UiInputConsumedThisFrame && !SelfieRadialMenuController.AnyWheelOpen && EscapeInputCoordinator.TryConsume(this);
			if ((Input.GetKeyDown(this.selfieExitKey) && (this._cameraCapture == null || !this._cameraCapture.IsGameplayKeyReserved(this.selfieExitKey))) || flag)
			{
				this.ExitSelfieToPhone();
				return;
			}
			float num = ((this._tripodActive && ((this._emoteSpeedController != null && this._emoteSpeedController.ReservesModifiedScroll) || (this._tpInput != null && this._tpInput.suppressCameraZoom))) ? 0f : GameplayInput.GetAxis("Mouse ScrollWheel"));
			if (Mathf.Abs(num) > 0.0001f)
			{
				this._selfieFov = Mathf.Clamp(this._selfieFov - num * this.selfieFovScrollSpeed, this.selfieFovMin, this.selfieFovMax);
			}
			if (this._mainCamera != null)
			{
				this._mainCamera.fieldOfView = this._selfieFov;
			}
			this.UpdateTripodHeight();
			float num2 = Mathf.InverseLerp(this.selfieFovMin, this.selfieFovMax, this._selfieFov);
			float num3 = Mathf.Lerp(1f, this.selfieLookLimitAtMaxFov, num2);
			float num4 = this.selfieYawLimit * num3;
			float num5 = this.selfiePitchLimit * num3;
			float axis = GameplayInput.GetAxis("Mouse X");
			float axis2 = GameplayInput.GetAxis("Mouse Y");
			Vector2 vector = new Vector2(GameplayInput.GetAxisRaw("Horizontal"), GameplayInput.GetAxisRaw("Vertical"));
			bool flag2 = !tripodBodyIsOwned && SmartphoneController.ShouldAdjustHead(this.enableSelfieHeadAdjust, GameplayInput.GetKey(KeyCode.LeftShift) || GameplayInput.GetKey(KeyCode.RightShift), vector);
			bool flag3 = !tripodBodyIsOwned && this._tripodActive && this.SelfiePoseIsDefault && !flag2 && vector.sqrMagnitude >= 0.0001f && (GameplayInput.GetKey(KeyCode.LeftShift) || GameplayInput.GetKey(KeyCode.RightShift)) && this._tpInput != null && this._tpInput.cc != null;
			if (flag2)
			{
				this._headYaw = Mathf.Clamp(this._headYaw - axis * this.selfieHeadSensitivity, -this.selfieHeadYawLimit, this.selfieHeadYawLimit);
				this._headPitch = Mathf.Clamp(this._headPitch - axis2 * this.selfieHeadSensitivity, -this.selfieHeadPitchLimit, this.selfieHeadPitchLimit);
			}
			else if (flag3)
			{
				this._tpInput.cc.transform.Rotate(0f, axis * this.selfieLookSensitivity, 0f, Space.World);
			}
			else
			{
				Vector2 vector2 = new Vector2(axis, axis2);
				if (!ControllerInput.IsActive)
				{
					vector2 = GameSettingsService.ApplyMouseLookInversion(vector2);
				}
				float num6 = vector2.x * this.selfieLookSensitivity;
				float num7 = vector2.y * this.selfieLookSensitivity;
				this._selfieYaw = Mathf.Clamp(this._selfieYaw + num6, -num4, num4);
				this._selfiePitch = Mathf.Clamp(this._selfiePitch - num7, -num5, num5);
			}
			if (this._selfieCamAnchor != null)
			{
				this._selfieCamAnchor.localRotation = this._selfieBaseLocalRot * Quaternion.Euler(this._selfiePitch, this._selfieYaw, 0f);
			}
			if (tripodBodyIsOwned)
			{
				return;
			}
			bool flag4 = this.IsBenchBodyOwned || (this.firstPersonController != null && this.firstPersonController.IsRidingVehicle);
			if (!flag4 && !flag2 && this.SelfiePoseIsDefault && this._tpInput != null && this._tpInput.cc != null)
			{
				vThirdPersonController cc = this._tpInput.cc;
				Vector3 vector3 = new Vector3(cc.input.x, 0f, cc.input.z);
				bool flag5 = vector3.z < -0.1f;
				if (vector3.sqrMagnitude > 0.04f && !flag5)
				{
					Vector3 vector4 = cc.transform.TransformDirection(vector3.normalized);
					vector4.y = 0f;
					if (vector4.sqrMagnitude > 0.0001f)
					{
						Quaternion quaternion = Quaternion.LookRotation(vector4, Vector3.up);
						cc.transform.rotation = Quaternion.RotateTowards(cc.transform.rotation, quaternion, this.selfieMoveTurnSpeed * Time.deltaTime);
					}
				}
			}
			float num8 = 0f;
			if (ControllerInput.IsActive && ControllerInput.Modifier && !GraphicsSettingsRuntimeUI.AnyOpen && !ControllerInput.UiInputConsumedThisFrame)
			{
				num8 = (ControllerInput.GetButton(ControllerButton.DpadRight) ? 1f : 0f) - (ControllerInput.GetButton(ControllerButton.DpadLeft) ? 1f : 0f);
			}
			else
			{
				if (GameplayInput.GetKey(KeyCode.LeftArrow))
				{
					num8 -= 1f;
				}
				if (GameplayInput.GetKey(KeyCode.RightArrow))
				{
					num8 += 1f;
				}
			}
			if (!flag4 && Mathf.Abs(num8) > 0f && this.playerAnimator != null)
			{
				this.playerAnimator.transform.Rotate(0f, num8 * this.selfiePlayerRotateSpeed * Time.deltaTime, 0f, Space.World);
			}
			if (this._selfieHandIKActive && this.SelfiePoseIsDefault && !this._tripodActive)
			{
				float num9 = 0f;
				if (GameplayInput.GetKey(KeyCode.UpArrow))
				{
					num9 += 1f;
				}
				if (GameplayInput.GetKey(KeyCode.DownArrow))
				{
					num9 -= 1f;
				}
				if (Mathf.Abs(num9) > 0f)
				{
					this._ikBlend = Mathf.Clamp(this._ikBlend + num9 * this.selfieHandIKBlendSpeed * Time.deltaTime, -1f, 1f);
				}
			}
			this.HandleSelfieCover();
			this.UpdateSelfieMoveLock();
		}

		// Token: 0x06006EDF RID: 28383 RVA: 0x002CAD66 File Offset: 0x002C8F66
		internal static bool ShouldAdjustHead(bool enabled, bool shiftHeld, Vector2 movementInput)
		{
			return enabled && shiftHeld && movementInput.sqrMagnitude < 0.0001f;
		}

		// Token: 0x06006EE0 RID: 28384 RVA: 0x002CAD80 File Offset: 0x002C8F80
		private void UpdateSelfieMoveLock()
		{
			if (this._tpInput == null || this.IsBenchBodyOwned)
			{
				return;
			}
			bool flag = !this.SelfiePoseIsDefault || this._poseMenuOpen || (this.enableSelfieHeadAdjust && ControllerInput.IsActive && (ControllerInput.Modifier || ControllerInput.GetButton(ControllerButton.LeftStick)));
			if (flag == this._selfieMoveLocked)
			{
				return;
			}
			this._selfieMoveLocked = flag;
			if (!flag)
			{
				this._tpInput.lockMoveInput = this._selfieSavedLockMove;
				return;
			}
			this._selfieSavedLockMove = this._tpInput.lockMoveInput;
			this._tpInput.lockMoveInput = true;
			vThirdPersonController cc = this._tpInput.cc;
			if (cc == null)
			{
				return;
			}
			cc.StopCharacter();
		}

		// Token: 0x17001230 RID: 4656
		// (get) Token: 0x06006EE1 RID: 28385 RVA: 0x002CAE32 File Offset: 0x002C9032
		private bool SelfiePoseIsDefault
		{
			get
			{
				return this._posePicker == null || this._posePicker.CurrentPoseIndex == 0;
			}
		}

		// Token: 0x06006EE2 RID: 28386 RVA: 0x002CAE54 File Offset: 0x002C9054
		private void HandleSelfieCover()
		{
			if (this._tripodActive || this._tripodTransitioning || this._tripodPlacementActive)
			{
				return;
			}
			CoverPoseController instance = CoverPoseController.Instance;
			if (instance == null)
			{
				return;
			}
			if (this._posePicker != null && this._posePicker.CurrentPoseIndex > 0)
			{
				if (instance.IsCovering)
				{
					instance.DeactivateCoverPose();
				}
				return;
			}
			if (!(this._tpInput != null) || this._tpInput.coverPoseInput == null || !this._tpInput.coverPoseInput.GetButtonDown())
			{
				if (instance.IsCovering && instance.CurrentCoverType != CoverType.FullCover)
				{
					instance.SetCoverType(CoverType.FullCover);
				}
				return;
			}
			if (instance.IsCovering)
			{
				instance.DeactivateCoverPose();
				return;
			}
			instance.ActivateCoverPose();
			instance.SetCoverType(CoverType.FullCover);
		}

		// Token: 0x06006EE3 RID: 28387 RVA: 0x002CAF1C File Offset: 0x002C911C
		private void ApplyPhonePose(Vector3 localPos, Vector3 localEuler, Vector3 localScale)
		{
			if (this.phoneInstance == null)
			{
				return;
			}
			this.phoneInstance.transform.localPosition = localPos;
			this.phoneInstance.transform.localRotation = Quaternion.Euler(localEuler);
			this.phoneInstance.transform.localScale = localScale;
		}

		// Token: 0x06006EE4 RID: 28388 RVA: 0x002CAF70 File Offset: 0x002C9170
		private void Update()
		{
			if (this._censorLeakMovementLocked && !CensorLeakState.GameOverPending)
			{
				this.SetCensorLeakMovementLocked(false);
			}
			if (this.ProcessPendingCensorLeakClose() || this._censorLeakRoutine != null)
			{
				return;
			}
			if (GameOverManager.Instance != null && GameOverManager.Instance.IsGameOver)
			{
				PhoneHudHints.Hide();
				return;
			}
			if (this.EnforceTripodDistanceLimit())
			{
				return;
			}
			this.ReconcilePhoneRuntimeState();
			this.ProcessPendingSelfieActivation();
			this.ProcessPendingTripodRestore();
			this.ProcessPendingSelfieDeactivation();
			if (TextInputFocus.BlocksGameplayInput)
			{
				PhoneHudHints.Hide();
				this.DriveHoldLayer();
				return;
			}
			if (GraphicsSettingsRuntimeUI.AnyOpen)
			{
				PhoneHudHints.Hide();
				return;
			}
			if (GameplayInput.GetKeyDown(this.toggleKey) && (this.State == SmartphoneController.PhoneState.PhoneHeld || !BikiniSlipDiveController.ReservesKeyboardInput(this.toggleKey)) && ((!CielOSController.AnyOpen && PlayerActivityGate.Allows(PlayerActivity.Phone)) || this.State == SmartphoneController.PhoneState.PhoneHeld))
			{
				if (this.State == SmartphoneController.PhoneState.PhoneHeld)
				{
					this.CloseOpenMediaViewers();
					this.ApplyPhotoViewerFreeze(false);
				}
				this.Toggle();
				return;
			}
			if (PhoneVideoViewer.IsOpen)
			{
				this.ApplyPhotoViewerFreeze(true);
				PhoneHudHints.Hide();
				return;
			}
			if (PhotoPolaroidViewer.IsOpen)
			{
				this.ApplyPhotoViewerFreeze(true);
				PhoneHudHints.ShowPhotoViewer();
				return;
			}
			this.ApplyPhotoViewerFreeze(false);
			if (this.State == SmartphoneController.PhoneState.PhoneHeld)
			{
				if (this._selfieActive)
				{
					PhoneHudHints.ShowSelfie(this.SelfiePoseIsDefault, this._cameraCapture != null && this._cameraCapture.IsVideoMode, this._cameraCapture != null && this._cameraCapture.IsRecordingVideo, this._tripodActive, this._tripodPlacementActive, this.TripodBodyIsOwned || this.IsBenchBodyOwned, this._cameraCapture != null && this._cameraCapture.IsGameplayKeyReserved(this._cameraCapture.filterSwitchKey), this._cameraCapture != null && this._cameraCapture.IsGameplayKeyReserved(this._cameraCapture.shutterKey), this.UserCameraExitBlocked, this._tripodActive && this._emoteSpeedController != null && this._emoteSpeedController.IsActive, this._cameraCapture != null && this._cameraCapture.IsGameplayKeyReserved(this._cameraCapture.mediaModeSwitchKey), !this.IsBenchBodyOwned, !this.IsBenchBodyOwned, this.IsBenchSeated);
					this.UpdateSelfiePhotoMode();
				}
				else
				{
					if (ControllerInput.IsControllerActive && ControllerInput.CancelPressed && !ControllerInput.UiInputConsumedThisFrame)
					{
						ControllerInput.ConsumeUIInput();
						if (!this.CloseOpenCustomApp())
						{
							this.ExitToGameplay();
						}
						return;
					}
					if (SmartphoneController.IsCustomAppOpen && EscapeInputCoordinator.TryConsume(this))
					{
						this.CloseOpenCustomApp();
					}
					else
					{
						PhoneHudHints.ShowPhoneHeld(this._cursorUnlocked, this.IsBenchSeated);
						this.UpdateZoomAndCursor();
						if (!ControllerInput.IsActive && this._cursorUnlocked && this.enableDragToAdjust)
						{
							this.UpdateDragOrClick();
						}
					}
				}
				if (this.debugMode && GameplayInput.GetKeyDown(KeyCode.G))
				{
					this.DiagnoseUIRaycast();
				}
			}
			this.DriveHoldLayer();
		}

		// Token: 0x06006EE5 RID: 28389 RVA: 0x002CB258 File Offset: 0x002C9458
		private void ReconcilePhoneRuntimeState()
		{
			if (this.IsPhoneOut && this.IsBenchBodyOwned && !this.IsBenchSeated && !this.IsStableTripodActivityProfile)
			{
				this.ExitToGameplay();
				return;
			}
			this.ResolveSelfieNotifier();
			PhoneSelfieNotifier selfieNotifier = this._selfieNotifier;
			if (selfieNotifier != null)
			{
				selfieNotifier.ReconcileLogicalState();
			}
			if (this.State == SmartphoneController.PhoneState.Gameplay)
			{
				this.ReleaseActivityContext();
				if (this._selfieActive)
				{
					this._selfieActive = false;
					this._pendingSelfieActivate = false;
					this._pendingSelfieDeactivate = false;
					if (this.playerAnimator != null && !string.IsNullOrEmpty(this.selfiePoseParam))
					{
						this.playerAnimator.SetBool(this.selfiePoseParam, false);
					}
					if (this._cameraCapture != null)
					{
						this._cameraCapture.SetCameraMode(false);
					}
					this.SuppressInvectorJump(false);
					if (this._headAdjuster != null)
					{
						this._headAdjuster.Deactivate();
					}
					if (this.firstPersonController != null)
					{
						this.firstPersonController.CameraAnchorOverride = null;
					}
				}
				return;
			}
			if ((this._activityLease == null || !this._activityLease.IsActive) && !this.TryAcquireActivityContext())
			{
				if (!GraphicsSettingsRuntimeUI.AnyOpen)
				{
					this.ExitToGameplay();
				}
				return;
			}
			if (GraphicsSettingsRuntimeUI.AnyOpen)
			{
				return;
			}
			if (!this.TryApplyDesiredActivityProfile() && this.debugMode)
			{
				Debug.LogWarning("[SmartphoneController] Profilo activity non riallineato; mantengo temporaneamente quello corrente.", this);
			}
			if (this.phoneInstance == null)
			{
				return;
			}
			if (!this.phoneInstance.activeSelf)
			{
				this.phoneInstance.SetActive(true);
			}
			bool flag = this._tripodInstance != null && this.phoneInstance.transform.IsChildOf(this._tripodInstance.transform);
			bool flag2 = flag && (!this._tripodActive || !this._tripodInstance.activeInHierarchy);
			bool flag3 = this._tripodActive && !this._tripodTransitioning && !flag && !this._tripodPlacementActive;
			if ((flag2 || flag3) && !this._pendingTripodRestore)
			{
				this.RequestTripodRestore(true);
			}
			bool flag4 = this._selfieNotifier != null && this._selfieNotifier.IsLogicallyOpen;
			if (this._selfieActive && !flag4 && !this._pendingSelfieDeactivate)
			{
				this.RequestSelfieDeactivation();
			}
			else if (!this._selfieActive && flag4 && this._selfieNotifier.gameObject.activeInHierarchy && !this._pendingSelfieActivate)
			{
				this._pendingSelfieActivate = true;
				this._pendingSelfieActivateEarliestFrame = Time.frameCount + 1;
			}
			if (Time.frameCount >= this._nextPhoneInvariantCheckFrame)
			{
				this._nextPhoneInvariantCheckFrame = Time.frameCount + 30;
				this.AssignPhoneCanvasEventCamera();
				this.RaisePhoneCanvasSorting(true);
				this.RepairStalePhoneRaycastBlockers();
			}
		}

		// Token: 0x06006EE6 RID: 28390 RVA: 0x002CB4EC File Offset: 0x002C96EC
		private void RepairStalePhoneRaycastBlockers()
		{
			if (this.phoneInstance == null)
			{
				return;
			}
			this.ResolvePhonePanel();
			if (this._phonePanel != null)
			{
				foreach (object obj in this._phonePanel)
				{
					Transform transform = (Transform)obj;
					if (!(transform == null) && transform.gameObject.activeInHierarchy)
					{
						Animator component = transform.GetComponent<Animator>();
						if (!(component == null) && component.isActiveAndEnabled && component.isInitialized && component.layerCount != 0)
						{
							AnimatorStateInfo currentAnimatorStateInfo = component.GetCurrentAnimatorStateInfo(0);
							if (currentAnimatorStateInfo.IsName("BackPanelAnimation") && currentAnimatorStateInfo.normalizedTime >= 0.98f)
							{
								PhoneAppAutoHide component2 = transform.GetComponent<PhoneAppAutoHide>();
								if (component2 == null || !component2.Pending)
								{
									if (this.debugMode)
									{
										Debug.LogWarning("[SmartphoneController] Riparato blocker UI invisibile: '" + transform.name + "' era ancora raycastabile dopo Back.", transform);
									}
									PhoneAppAutoHide.Schedule(transform.gameObject, 0f);
								}
							}
						}
					}
				}
			}
			PhotoGallery componentInChildren = this.phoneInstance.GetComponentInChildren<PhotoGallery>(true);
			if (componentInChildren != null && componentInChildren.FullScreenPanel != null && componentInChildren.FullScreenPanel.activeInHierarchy && !PhotoPolaroidViewer.IsOpen)
			{
				Image component3 = componentInChildren.FullScreenPanel.GetComponent<Image>();
				if (component3 != null && !component3.enabled)
				{
					component3.enabled = true;
					componentInChildren.FullScreenPanel.SetActive(false);
					if (this.debugMode)
					{
						Debug.LogWarning("[SmartphoneController] Riparato FullscreenImage invisibile rimasto sopra la galleria.", componentInChildren);
					}
				}
			}
		}

		// Token: 0x06006EE7 RID: 28391 RVA: 0x002CB6AC File Offset: 0x002C98AC
		private void ResolveSelfieNotifier()
		{
			if (this._selfieNotifier != null && this.phoneInstance != null && this._selfieNotifier.transform.IsChildOf(this.phoneInstance.transform))
			{
				return;
			}
			this._selfieNotifier = ((this.phoneInstance != null) ? this.phoneInstance.GetComponentInChildren<PhoneSelfieNotifier>(true) : null);
		}

		// Token: 0x06006EE8 RID: 28392 RVA: 0x002CB718 File Offset: 0x002C9918
		private void LateUpdate()
		{
			this.ReconcilePhoneCursor();
			if (this._selfieHandIKActive && this._rigController != null)
			{
				this.ApplySelfieHandIK();
			}
			if (this._selfieActive && this.enableSelfieHeadAdjust && this._headAdjuster != null)
			{
				if (this.TripodBodyIsOwned)
				{
					this._headAdjuster.Deactivate();
				}
				else
				{
					this._headAdjuster.SetOffset(this._headYaw, this._headPitch, this.selfieHeadSmoothSpeed);
				}
			}
			if (this._selfieActive && !this._tripodActive && !this.SuspendPhonePoseReapply)
			{
				this.TickSelfiePoseOffsetBlend();
			}
		}

		// Token: 0x06006EE9 RID: 28393 RVA: 0x002CB7B8 File Offset: 0x002C99B8
		private void TickSelfiePoseOffsetBlend()
		{
			if (this._selfiePoseBlend < 1f && this._selfiePoseBlendDur > 0f)
			{
				this._selfiePoseBlend = Mathf.Min(1f, this._selfiePoseBlend + Time.deltaTime / this._selfiePoseBlendDur);
				float num = Mathf.SmoothStep(0f, 1f, this._selfiePoseBlend);
				this._selfiePhonePos = Vector3.Lerp(this._selfiePhonePos, this._selfiePhonePosTgt, num);
				this._selfiePhoneEul = SmartphoneController.LerpEuler(this._selfiePhoneEul, this._selfiePhoneEulTgt, num);
				this._selfiePhoneScale = Vector3.Lerp(this._selfiePhoneScale, this._selfiePhoneScaleTgt, num);
				this._selfieCamPos = Vector3.Lerp(this._selfieCamPos, this._selfieCamPosTgt, num);
				this._selfieBaseLocalRot = Quaternion.Slerp(this._selfieBaseLocalRot, this._selfieCamBaseRotTgt, num);
				if (this._selfiePoseBlend >= 1f)
				{
					this._selfiePhonePos = this._selfiePhonePosTgt;
					this._selfiePhoneEul = this._selfiePhoneEulTgt;
					this._selfiePhoneScale = this._selfiePhoneScaleTgt;
					this._selfieCamPos = this._selfieCamPosTgt;
					this._selfieBaseLocalRot = this._selfieCamBaseRotTgt;
				}
			}
			this.ApplyPhonePose(this._selfiePhonePos, this._selfiePhoneEul, this._selfiePhoneScale);
			if (this._selfieCamAnchor != null)
			{
				this._selfieCamAnchor.localPosition = this._selfieCamPos;
				this._selfieCamAnchor.localRotation = this._selfieBaseLocalRot * Quaternion.Euler(this._selfiePitch, this._selfieYaw, 0f);
			}
		}

		// Token: 0x06006EEA RID: 28394 RVA: 0x002CB942 File Offset: 0x002C9B42
		private static Vector3 LerpEuler(Vector3 a, Vector3 b, float t)
		{
			return new Vector3(Mathf.LerpAngle(a.x, b.x, t), Mathf.LerpAngle(a.y, b.y, t), Mathf.LerpAngle(a.z, b.z, t));
		}

		// Token: 0x06006EEB RID: 28395 RVA: 0x002CB980 File Offset: 0x002C9B80
		private void ApplySelfieHandIK()
		{
			if (!this._selfieActive || this._tripodActive || !this.SelfiePoseIsDefault || (this._posePicker != null && this._posePicker.HasPoseLayerInfluence))
			{
				this.ResetSelfieHandAdjustment();
				return;
			}
			if (this._handBone == null && this.playerAnimator != null)
			{
				this._handBone = this.playerAnimator.GetBoneTransform(HumanBodyBones.RightHand);
			}
			if (this._handBone == null)
			{
				return;
			}
			if (this._selfiePivotRef == null && this.playerAnimator != null)
			{
				this._selfiePivotRef = this.playerAnimator.GetBoneTransform(HumanBodyBones.RightLowerArm);
			}
			Transform transform = ((this._selfiePivotRef != null) ? this._selfiePivotRef : this._handBone.parent);
			if (!this._ikBaseCaptured)
			{
				if (transform == null)
				{
					return;
				}
				if (!this.IsSelfieStateSettled())
				{
					return;
				}
				this._ikBasePos = transform.InverseTransformPoint(this._handBone.position);
				this._ikBaseRot = Quaternion.Inverse(transform.rotation) * this._handBone.rotation;
				this._ikBaseCaptured = true;
			}
			if (transform == null)
			{
				return;
			}
			Vector3 vector = transform.TransformPoint(this._ikBasePos);
			Quaternion quaternion = transform.rotation * this._ikBaseRot;
			float num = Mathf.Sign((this._ikBlend == 0f) ? 1f : this._ikBlend);
			Vector3 vector2 = vector + Vector3.up * (this.selfieHandIKArc * num);
			float num2 = Mathf.Abs(this._ikBlend) * this.selfieHandIKMaxWeight;
			this._rigController.SetCoverLimbPoseWeighted(LimbType.RightHand, vector2, quaternion, num2, 1f, 1f, vector, 0f);
		}

		// Token: 0x06006EEC RID: 28396 RVA: 0x002CBB48 File Offset: 0x002C9D48
		private void ResetSelfieHandAdjustment()
		{
			this._ikBlend = 0f;
			this._ikBaseCaptured = false;
			if (this._selfieHandIKActive && this._rigController != null)
			{
				this._rigController.ZeroCoverLimb(LimbType.RightHand);
			}
		}

		// Token: 0x06006EED RID: 28397 RVA: 0x002CBB80 File Offset: 0x002C9D80
		private bool IsSelfieStateSettled()
		{
			return this.playerAnimator == null || this._holdLayerIndex < 0 || (!this.playerAnimator.IsInTransition(this._holdLayerIndex) && this.playerAnimator.GetCurrentAnimatorStateInfo(this._holdLayerIndex).IsName("Selfie"));
		}

		// Token: 0x06006EEE RID: 28398 RVA: 0x002CBBDC File Offset: 0x002C9DDC
		private void DiagnoseUIRaycast()
		{
			EventSystem current = EventSystem.current;
			if (current == null)
			{
				Debug.LogWarning("[SmartphoneController][DIAG] EventSystem.current e' NULL: nessun EventSystem attivo in scena.", this);
				return;
			}
			Debug.Log(string.Format("[SmartphoneController][DIAG] EventSystem='{0}', module='{1}', cursorPos={2}", current.name, current.currentInputModule, GameplayInput.mousePosition), this);
			PointerEventData pointerEventData = new PointerEventData(current)
			{
				position = GameplayInput.mousePosition
			};
			List<RaycastResult> list = new List<RaycastResult>();
			current.RaycastAll(pointerEventData, list);
			if (list.Count == 0)
			{
				Debug.LogWarning("[SmartphoneController][DIAG] Nessun hit UI sotto il puntatore. Il raggio non raggiunge lo schermo del telefono.", this);
				Camera main = Camera.main;
				Canvas[] array = ((this.phoneInstance != null) ? this.phoneInstance.GetComponentsInChildren<Canvas>(true) : null);
				if (array != null)
				{
					foreach (Canvas canvas in array)
					{
						Debug.Log(string.Format("[SmartphoneController][DIAG] Canvas '{0}' renderMode={1}, worldCamera={2}, hasRaycaster={3}", new object[]
						{
							canvas.name,
							canvas.renderMode,
							canvas.worldCamera ? canvas.worldCamera.name : "NULL",
							canvas.GetComponent<GraphicRaycaster>() != null
						}), canvas);
					}
				}
				return;
			}
			Debug.Log(string.Format("[SmartphoneController][DIAG] {0} hit UI:", list.Count), this);
			foreach (RaycastResult raycastResult in list)
			{
				Debug.Log(string.Format("[SmartphoneController][DIAG]   -> {0} (module={1}, dist={2})", raycastResult.gameObject.name, raycastResult.module, raycastResult.distance), raycastResult.gameObject);
			}
		}

		// Token: 0x06006EEF RID: 28399 RVA: 0x002CBDA4 File Offset: 0x002C9FA4
		private void EnsurePhotoViewerNotifier()
		{
			if (this._photoViewerEnsured || this.phoneInstance == null || !this.rotatePhotoForPhotoView)
			{
				return;
			}
			PhotoGallery componentInChildren = this.phoneInstance.GetComponentInChildren<PhotoGallery>(true);
			if (componentInChildren == null || componentInChildren.FullScreenPanel == null)
			{
				return;
			}
			if (componentInChildren.FullScreenPanel.GetComponent<PhotoPolaroidViewer>() == null)
			{
				componentInChildren.FullScreenPanel.AddComponent<PhotoPolaroidViewer>();
			}
			ScrollRect[] componentsInChildren = componentInChildren.GetComponentsInChildren<ScrollRect>(true);
			ScrollRect[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].scrollSensitivity = 12f;
			}
			if (this.debugMode && componentsInChildren.Length != 0)
			{
				Debug.Log(string.Format("[SmartphoneController] Scroll galleria abilitato su {0} ScrollRect.", componentsInChildren.Length), this);
			}
			this._photoViewerEnsured = true;
			if (this.debugMode)
			{
				Debug.Log("[SmartphoneController] PhotoPolaroidViewer agganciato al FullScreenPanel.", this);
			}
		}

		// Token: 0x06006EF0 RID: 28400 RVA: 0x002CBE78 File Offset: 0x002CA078
		private void UpdateDragOrClick()
		{
			if (this.firstPersonController == null)
			{
				return;
			}
			float num = (GameplayInput.GetMouseButton(2) ? GameplayInput.GetAxis("Mouse Y") : 0f);
			if (!ControllerInput.IsActive)
			{
				num = GameSettingsService.ApplyMouseLookInversion(new Vector2(0f, num)).y;
			}
			float num2 = num * this.dragLookSensitivity;
			this._dragVel = Mathf.Lerp(this._dragVel, num2, 1f - Mathf.Exp(-this.dragSmoothSpeed * Time.deltaTime));
			if (Mathf.Abs(this._dragVel) > 0.0001f)
			{
				this.firstPersonController.RotateCameraFP(0f, this._dragVel);
			}
		}

		// Token: 0x06006EF1 RID: 28401 RVA: 0x002CBF28 File Offset: 0x002CA128
		private void UpdateZoomAndCursor()
		{
			if (this._mainCamera == null)
			{
				this._mainCamera = Camera.main;
			}
			bool isActive = ControllerInput.IsActive;
			if (isActive)
			{
				this.UpdateControllerPhoneView(Time.unscaledDeltaTime);
			}
			else
			{
				this._controllerPhoneViewReady = false;
			}
			float num = ((!isActive && (GameplayInput.GetKey(KeyCode.LeftShift) || GameplayInput.GetKey(KeyCode.RightShift))) ? GameplayInput.GetAxis("Mouse ScrollWheel") : 0f);
			if (Mathf.Abs(num) > 0.0001f)
			{
				float num2 = Mathf.Max(0.0001f, this.fovZoomedOut - this.fovZoomedIn);
				this._zoomTarget01 = Mathf.Clamp01(this._zoomTarget01 + num * (this.zoomScrollSpeed / num2));
			}
			this._zoom01 = Mathf.MoveTowards(this._zoom01, this._zoomTarget01, this.zoomSmoothSpeed * Time.deltaTime);
			if (this._mainCamera != null)
			{
				this._mainCamera.fieldOfView = Mathf.Lerp(this.fovZoomedOut, this.fovZoomedIn, this._zoom01);
			}
			bool flag = (isActive ? this._controllerPhoneViewReady : (this._zoom01 >= this.cursorUnlockThreshold));
			if (flag && !this._cursorUnlocked)
			{
				this.SetCursorUnlocked(true);
				return;
			}
			if (!flag && this._cursorUnlocked)
			{
				this.SetCursorUnlocked(false);
			}
		}

		// Token: 0x06006EF2 RID: 28402 RVA: 0x002CC074 File Offset: 0x002CA274
		private void ReconcilePhoneCursor()
		{
			if (Application.isPlaying && !Application.isFocused)
			{
				return;
			}
			if (this.State != SmartphoneController.PhoneState.PhoneHeld || this.SuspendPhonePoseReapply || PlayerActivityGate.IsResourceOwned(PlayerContextResource.ModalUI) || GraphicsSettingsRuntimeUI.AnyOpen || CielOSController.AnyOpen || (GameOverManager.Instance != null && GameOverManager.Instance.IsGameOver) || this._photoViewerFrozen || PhotoPolaroidViewer.IsOpen || PhoneVideoViewer.IsOpen || this._poseMenuOpen || (this._tripodActive && this._tripodRapidActions != null && this._tripodRapidActions.IsRebindOpen))
			{
				return;
			}
			if (this.IsScreenInteractionActive)
			{
				if (Cursor.lockState != CursorLockMode.None)
				{
					Cursor.lockState = CursorLockMode.None;
				}
				bool flag = !ControllerInput.IsActive;
				if (Cursor.visible != flag)
				{
					Cursor.visible = flag;
				}
				return;
			}
			if (Cursor.lockState != CursorLockMode.Locked)
			{
				Cursor.lockState = CursorLockMode.Locked;
			}
			if (Cursor.visible)
			{
				Cursor.visible = false;
			}
		}

		// Token: 0x06006EF3 RID: 28403 RVA: 0x002CC15C File Offset: 0x002CA35C
		private void SetCursorUnlocked(bool unlocked)
		{
			if (this._cursorUnlocked == unlocked)
			{
				return;
			}
			if (unlocked)
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = !ControllerInput.IsActive;
				if (this._tpInput != null)
				{
					this._savedLockCameraInput = this._tpInput.lockCameraInput;
					this._tpInput.lockCameraInput = true;
					vThirdPersonInput.ForceCameraLock = true;
				}
				this.FreezeCiel(true);
				this._cursorUnlocked = true;
				if (this.debugMode)
				{
					Debug.Log("[SmartphoneController] Cursore sbloccato + camera/Ciel congelati.", this);
					return;
				}
			}
			else
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
				if (this._tpInput != null)
				{
					vThirdPersonInput.ForceCameraLock = false;
					this._tpInput.lockCameraInput = this._savedLockCameraInput;
				}
				this.FreezeCiel(false);
				this._cursorUnlocked = false;
				if (this.debugMode)
				{
					Debug.Log("[SmartphoneController] Cursore ri-nascosto + camera/Ciel riattivi.", this);
				}
			}
		}

		// Token: 0x06006EF4 RID: 28404 RVA: 0x002CC22E File Offset: 0x002CA42E
		private void FreezeCiel(bool frozen)
		{
			this._ci_freezeRequested = frozen;
			this.ApplyCielFreeze(frozen || this._censorLeakMovementLocked);
		}

		// Token: 0x06006EF5 RID: 28405 RVA: 0x002CC24C File Offset: 0x002CA44C
		private void SetCensorLeakMovementLocked(bool locked)
		{
			this._censorLeakMovementLocked = locked;
			this.ApplyCielFreeze(this._ci_freezeRequested || locked);
			if (locked)
			{
				if (this._tpInput != null)
				{
					this._tpInput.lockMoveInput = true;
				}
				if (this._frozenMotor != null)
				{
					this._frozenMotor.lockMovement = true;
					this._frozenMotor.lockRotation = true;
				}
			}
		}

		// Token: 0x06006EF6 RID: 28406 RVA: 0x002CC2B4 File Offset: 0x002CA4B4
		private void ApplyCielFreeze(bool frozen)
		{
			if (frozen == this._ci_frozen)
			{
				return;
			}
			this._ci_frozen = frozen;
			if (frozen)
			{
				if (this.playerAnimator != null)
				{
					this._savedAnimatorSpeed = this.playerAnimator.speed;
					this._savedAnimatorEnabled = this.playerAnimator.enabled;
					this._savedKeepAnimatorStateOnDisable = this.playerAnimator.keepAnimatorStateOnDisable;
					this._frozenAnimatorGraph = this.playerAnimator.playableGraph;
					this._resumeAnimatorGraph = this._frozenAnimatorGraph.IsValid() && this._frozenAnimatorGraph.IsPlaying();
					if (this._resumeAnimatorGraph)
					{
						this._frozenAnimatorGraph.Stop();
					}
					RigBuilder component = this.playerAnimator.GetComponent<RigBuilder>();
					this._frozenRigGraph = ((component != null) ? component.graph : default(PlayableGraph));
					this._resumeRigGraph = this._frozenRigGraph.IsValid() && this._frozenRigGraph.IsPlaying();
					if (this._resumeRigGraph)
					{
						this._frozenRigGraph.Stop();
					}
					this.playerAnimator.keepAnimatorStateOnDisable = true;
					this.playerAnimator.speed = 0f;
					this.playerAnimator.enabled = false;
				}
				if (this._tpInput != null)
				{
					this._savedLockMoveInput = this._tpInput.lockMoveInput;
					this._tpInput.lockMoveInput = true;
					this._frozenMotor = this._tpInput.cc;
					if (this._frozenMotor != null)
					{
						this._savedMotorLockMovement = this._frozenMotor.lockMovement;
						this._savedMotorLockRotation = this._frozenMotor.lockRotation;
						this._frozenBody = this._frozenMotor._rigidbody;
						if (this._frozenBody != null)
						{
							Vector3 position = this._frozenBody.transform.position;
							Quaternion rotation = this._frozenBody.transform.rotation;
							this._savedBodyConstraints = this._frozenBody.constraints;
							this._savedBodyInterpolation = this._frozenBody.interpolation;
							this._savedBodyKinematic = this._frozenBody.isKinematic;
							this._frozenBody.interpolation = RigidbodyInterpolation.None;
							this._frozenBody.position = position;
							this._frozenBody.rotation = rotation;
							this._frozenBody.transform.SetPositionAndRotation(position, rotation);
							if (!this._frozenBody.isKinematic)
							{
								this._frozenBody.velocity = Vector3.zero;
								this._frozenBody.angularVelocity = Vector3.zero;
							}
							this._frozenBody.isKinematic = true;
							this._frozenBody.constraints = RigidbodyConstraints.FreezeAll;
						}
						this._frozenMotor.StopCharacter();
						this._frozenMotor.lockMovement = true;
						this._frozenMotor.lockRotation = true;
						return;
					}
				}
			}
			else
			{
				if (this._tpInput != null)
				{
					this._tpInput.lockMoveInput = this._savedLockMoveInput;
				}
				if (this._frozenMotor != null)
				{
					this._frozenMotor.lockMovement = this._savedMotorLockMovement;
					this._frozenMotor.lockRotation = this._savedMotorLockRotation;
				}
				if (this._frozenBody != null)
				{
					this._frozenBody.isKinematic = this._savedBodyKinematic;
					if (!this._frozenBody.isKinematic)
					{
						this._frozenBody.velocity = Vector3.zero;
						this._frozenBody.angularVelocity = Vector3.zero;
					}
					this._frozenBody.constraints = this._savedBodyConstraints;
					this._frozenBody.interpolation = this._savedBodyInterpolation;
				}
				if (this.playerAnimator != null)
				{
					this.playerAnimator.speed = this._savedAnimatorSpeed;
					this.playerAnimator.enabled = this._savedAnimatorEnabled;
					this.playerAnimator.keepAnimatorStateOnDisable = this._savedKeepAnimatorStateOnDisable;
				}
				if (this._resumeAnimatorGraph && this._frozenAnimatorGraph.IsValid())
				{
					this._frozenAnimatorGraph.Play();
				}
				if (this._resumeRigGraph && this._frozenRigGraph.IsValid())
				{
					this._frozenRigGraph.Play();
				}
				this._frozenAnimatorGraph = (this._frozenRigGraph = default(PlayableGraph));
				this._resumeAnimatorGraph = (this._resumeRigGraph = false);
				this._frozenMotor = null;
				this._frozenBody = null;
			}
		}

		// Token: 0x06006EF7 RID: 28407 RVA: 0x002CC6E8 File Offset: 0x002CA8E8
		private void DumpInputState(string tag)
		{
			if (!this.debugMode)
			{
				return;
			}
			bool forceCameraLock = vThirdPersonInput.ForceCameraLock;
			string text = ((this._tpInput != null) ? this._tpInput.lockCameraInput.ToString() : "n/a");
			string text2 = ((this._tpInput != null) ? this._tpInput.lockMoveInput.ToString() : "n/a");
			Debug.Log(string.Format("[Phone/state] {0} | State={1} selfie={2} poseMenu={3} ", new object[] { tag, this.State, this._selfieActive, this._poseMenuOpen }) + string.Format("cursorUnlocked={0} Cursor.lockState={1} visible={2} ", this._cursorUnlocked, Cursor.lockState, Cursor.visible) + string.Format("ForceCameraLock={0} lockCameraInput={1} lockMoveInput={2} ", forceCameraLock, text, text2) + string.Format("frozen={0} zoom01={1:0.00} customApp={2}", this._ci_frozen, this._zoom01, SmartphoneController.IsCustomAppOpen), this);
		}

		// Token: 0x06006EF8 RID: 28408 RVA: 0x002CC800 File Offset: 0x002CAA00
		private void ApplyPhotoViewerFreeze(bool frozen)
		{
			if (frozen == this._photoViewerFrozen)
			{
				return;
			}
			this._photoViewerFrozen = frozen;
			if (frozen)
			{
				if (!CielOSController.AnyOpen)
				{
					this.FreezeCiel(true);
				}
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = !ControllerInput.IsActive;
				if (this._tpInput != null)
				{
					this._pv_savedLockCameraInput = this._tpInput.lockCameraInput;
					this._tpInput.lockCameraInput = true;
					vThirdPersonInput.ForceCameraLock = true;
					return;
				}
			}
			else
			{
				if (CielOSController.AnyOpen)
				{
					if (this._tpInput != null)
					{
						this._tpInput.lockCameraInput = this._pv_savedLockCameraInput;
					}
					Cursor.lockState = CursorLockMode.None;
					Cursor.visible = !ControllerInput.IsActive;
					return;
				}
				bool cursorUnlocked = this._cursorUnlocked;
				if (!cursorUnlocked)
				{
					this.FreezeCiel(false);
				}
				if (this._tpInput != null)
				{
					vThirdPersonInput.ForceCameraLock = cursorUnlocked;
					this._tpInput.lockCameraInput = cursorUnlocked || this._pv_savedLockCameraInput;
				}
				if (cursorUnlocked)
				{
					Cursor.lockState = CursorLockMode.None;
					Cursor.visible = !ControllerInput.IsActive;
					return;
				}
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
		}

		// Token: 0x06006EF9 RID: 28409 RVA: 0x002CC910 File Offset: 0x002CAB10
		private bool CloseOpenCustomApp()
		{
			Action customAppCloser = SmartphoneController._customAppCloser;
			if (customAppCloser == null)
			{
				return false;
			}
			customAppCloser();
			if (SmartphoneController._customAppCloser != null)
			{
				SmartphoneController._customAppCloser = null;
				this.ResetDynamicIsland();
			}
			return true;
		}

		// Token: 0x06006EFA RID: 28410 RVA: 0x002CC944 File Offset: 0x002CAB44
		private void ResetDynamicIsland()
		{
			if (!this._dynamicIslandResolved)
			{
				if (this.phoneInstance != null)
				{
					this._dynamicIsland = this.phoneInstance.GetComponentInChildren<DynamicIslandController>(true);
				}
				this._dynamicIslandResolved = true;
			}
			if (this._dynamicIsland != null)
			{
				this._dynamicIsland.ResetToIdle();
			}
		}

		// Token: 0x06006EFB RID: 28411 RVA: 0x002CC999 File Offset: 0x002CAB99
		private void CloseOpenMediaViewers()
		{
			PhoneVideoViewer.CloseIfOpen();
			PhotoPolaroidViewer.CloseIfOpen();
		}

		// Token: 0x06006EFC RID: 28412 RVA: 0x002CC9A8 File Offset: 0x002CABA8
		private void ForcePhoneHome()
		{
			this.ResolvePhonePanel();
			if (this._phonePanel == null)
			{
				return;
			}
			PhotoGallery componentInChildren = this.phoneInstance.GetComponentInChildren<PhotoGallery>(true);
			if (componentInChildren != null)
			{
				componentInChildren.ResetToDefault(false);
			}
			foreach (object obj in this._phonePanel)
			{
				Transform transform = (Transform)obj;
				if (!(transform.name == "Home") && !(transform.name == "Wallpaper") && !(transform.name == "TopBar") && (transform.name.StartsWith("m", StringComparison.Ordinal) || transform.GetComponent<Animator>() != null || transform.GetComponent<PhotoGallery>() != null || transform.GetComponent<OxGramApp>() != null))
				{
					Animator component = transform.GetComponent<Animator>();
					if (component != null)
					{
						component.keepAnimatorStateOnDisable = false;
					}
					if (transform.gameObject.activeSelf)
					{
						transform.gameObject.SetActive(false);
					}
				}
			}
			Transform transform2 = this._phonePanel.Find("Home");
			if (transform2 != null && !transform2.gameObject.activeSelf)
			{
				transform2.gameObject.SetActive(true);
			}
		}

		// Token: 0x06006EFD RID: 28413 RVA: 0x002CCB18 File Offset: 0x002CAD18
		private void ResolvePhonePanel()
		{
			if (this._phonePanelResolved && this._phonePanel != null && this.phoneInstance != null && this._phonePanel.IsChildOf(this.phoneInstance.transform))
			{
				return;
			}
			this._phonePanel = null;
			if (this.phoneInstance != null)
			{
				foreach (Transform transform in this.phoneInstance.GetComponentsInChildren<Transform>(true))
				{
					if (!(transform.name != "Home"))
					{
						this._phonePanel = transform.parent;
						break;
					}
				}
			}
			this._phonePanelResolved = true;
		}

		// Token: 0x06006EFE RID: 28414 RVA: 0x002CCBC0 File Offset: 0x002CADC0
		private void EnsureNativeCameraBackGuard()
		{
			if (this._nativeCameraBackGuarded || this.phoneInstance == null)
			{
				return;
			}
			PhoneSelfieNotifier componentInChildren = this.phoneInstance.GetComponentInChildren<PhoneSelfieNotifier>(true);
			if (componentInChildren == null)
			{
				return;
			}
			Button button = null;
			foreach (Button button2 in componentInChildren.GetComponentsInChildren<Button>(true))
			{
				if (button2 != null && button2.name.IndexOf("back", StringComparison.OrdinalIgnoreCase) >= 0)
				{
					button = button2;
					break;
				}
			}
			if (button == null)
			{
				return;
			}
			button.onClick.AddListener(new UnityAction(this.HandleNativeCameraBack));
			this._nativeCameraBackGuarded = true;
		}

		// Token: 0x06006EFF RID: 28415 RVA: 0x002CCC64 File Offset: 0x002CAE64
		private void HandleNativeCameraBack()
		{
			this.ForcePhoneHome();
			this.ResetDynamicIsland();
		}

		// Token: 0x06006F00 RID: 28416 RVA: 0x002CCC74 File Offset: 0x002CAE74
		public void Toggle()
		{
			if (this.State == SmartphoneController.PhoneState.Gameplay)
			{
				this.EnterPhoneHeld();
				return;
			}
			if (PhoneCameraCapture.CameraInputSuppressed)
			{
				return;
			}
			if (this._selfieActive && this._cameraToggleConsumedFrame == Time.frameCount)
			{
				return;
			}
			if (this._selfieActive && this._cameraCapture != null && (this._cameraCapture.IsRecordingVideo || this._cameraCapture.IsGameplayKeyReserved(this._cameraCapture.shutterKey)))
			{
				this._cameraToggleConsumedFrame = Time.frameCount;
				if (this._cameraCapture.IsRecordingVideo)
				{
					this._cameraCapture.StopVideoRecording();
				}
				else
				{
					this._cameraCapture.HandleShutterPressed();
				}
				if (ControllerInput.IsActive)
				{
					ControllerInput.ConsumeUIInput();
				}
				return;
			}
			if (this.UserCameraExitBlocked)
			{
				return;
			}
			this.ExitToGameplay();
		}

		// Token: 0x06006F01 RID: 28417 RVA: 0x002CCD38 File Offset: 0x002CAF38
		private static bool SmartphoneBlockedByRank()
		{
			if (RankRuntimeController.Current == null)
			{
				return false;
			}
			RankDefinitionSO currentRank = RankProgressionService.CurrentRank;
			return currentRank != null && !currentRank.allowSmartphone;
		}

		// Token: 0x06006F02 RID: 28418 RVA: 0x002CCD70 File Offset: 0x002CAF70
		public void EnterPhoneHeld()
		{
			if (this._censorLeakExitRequested || this._censorLeakRoutine != null)
			{
				return;
			}
			if (this.State == SmartphoneController.PhoneState.PhoneHeld)
			{
				if (this.TryAcquireActivityContext())
				{
					SmartphoneController._activeInstance = this;
				}
				return;
			}
			if (SmartphoneController.SmartphoneBlockedByRank())
			{
				BlockedActionFeedback.Show(null);
				return;
			}
			if (this.HasActiveInventoryHandItem())
			{
				if (base.isActiveAndEnabled)
				{
					BlockedActionFeedback.Show("Put away the item first");
				}
				return;
			}
			if (!this.TryAcquireActivityContext())
			{
				if (base.isActiveAndEnabled)
				{
					BlockedActionFeedback.Show(null);
				}
				return;
			}
			SmartphoneController._activeInstance = this;
			this.EnsurePhoneInstance();
			this.AttachPhoneToHand();
			this.ForcePhoneHome();
			if (this.phoneInstance != null)
			{
				this.phoneInstance.SetActive(true);
			}
			this.ResetDynamicIsland();
			this.SetPhoneHeldParam(true);
			this._holdLayerTargetWeight = 1f;
			this._owningHoldLayer = true;
			this.EnterAndLockFirstPerson();
			this._savedCursorLock = Cursor.lockState;
			this._savedCursorVisible = Cursor.visible;
			this._savedLockCameraInput = this._tpInput != null && this._tpInput.lockCameraInput;
			this._cursorUnlocked = false;
			this._zoom01 = 0f;
			this._zoomTarget01 = 0f;
			this._mainCamera = Camera.main;
			this._controllerPhoneViewReady = false;
			if (this._mainCamera != null)
			{
				this._savedFov = this._mainCamera.fieldOfView;
			}
			this.AssignPhoneCanvasEventCamera();
			this.RaisePhoneCanvasSorting(true);
			this.SetConfiguredUiHidden(true);
			this.EnableTapSound(true);
			this.EnsurePhotoViewerNotifier();
			this.State = SmartphoneController.PhoneState.PhoneHeld;
			if (this.debugMode)
			{
				Debug.Log("[SmartphoneController] -> PhoneHeld (FP locked)", this);
			}
		}

		// Token: 0x06006F03 RID: 28419 RVA: 0x002CCF00 File Offset: 0x002CB100
		private bool HasActiveInventoryHandItem()
		{
			if (this._inventoryManager == null)
			{
				this._inventoryManager = PlayerInventoryManager.Instance ?? base.GetComponentInParent<PlayerInventoryManager>();
			}
			return this._inventoryManager != null && this._inventoryManager.GetActiveItem() != null;
		}

		// Token: 0x06006F04 RID: 28420 RVA: 0x002CCF54 File Offset: 0x002CB154
		public void ExitToGameplay()
		{
			if (this.State == SmartphoneController.PhoneState.Gameplay)
			{
				if (this._selfieActive || this._selfieMoveLocked || this._poseMenuOpen || this._ci_frozen)
				{
					this.ApplySelfieCamera(false);
				}
				this._pendingSelfieActivate = false;
				this._pendingSelfieDeactivate = false;
				SelfieRadialMenuController radialMenu = this._radialMenu;
				if (radialMenu != null)
				{
					radialMenu.SetActive(false);
				}
				this.ReleaseActivityContext();
				this.QueueCensorLeakGameOver();
				return;
			}
			this.CloseOpenMediaViewers();
			this.ApplyPhotoViewerFreeze(false);
			this.CloseOpenCustomApp();
			this.ResetDynamicIsland();
			if (this._tripodActive || this._tripodTransitioning || this._tripodPlacementActive)
			{
				this.RestorePhoneFromTripodImmediate(false);
			}
			this.ApplySelfieCamera(false);
			this.ForcePhoneHome();
			if (this.phoneInstance != null)
			{
				this.phoneInstance.SetActive(false);
			}
			this.SetPhoneHeldParam(false);
			if (this.playerAnimator != null && !string.IsNullOrEmpty(this.selfiePoseParam))
			{
				this.playerAnimator.SetBool(this.selfiePoseParam, false);
			}
			if (this.firstPersonController != null)
			{
				this.firstPersonController.CameraAnchorOverride = null;
			}
			this._holdLayerTargetWeight = 0f;
			if (this._mainCamera != null)
			{
				this._mainCamera.fieldOfView = this._savedFov;
			}
			this._zoom01 = 0f;
			this._zoomTarget01 = 0f;
			if (this._cursorUnlocked)
			{
				this.SetCursorUnlocked(false);
			}
			Cursor.lockState = this._savedCursorLock;
			Cursor.visible = this._savedCursorVisible;
			vThirdPersonInput.ForceCameraLock = false;
			if (this._tpInput != null)
			{
				this._tpInput.lockCameraInput = this._savedLockCameraInput;
			}
			this.RaisePhoneCanvasSorting(false);
			this.SetConfiguredUiHidden(false);
			this.EnableTapSound(false);
			this.UnlockAndExitFirstPerson();
			PhoneHudHints.Hide();
			this.State = SmartphoneController.PhoneState.Gameplay;
			if (this.debugMode)
			{
				Debug.Log("[SmartphoneController] -> Gameplay", this);
			}
			this.ReleaseActivityContext();
			this.QueueCensorLeakGameOver();
		}

		// Token: 0x06006F05 RID: 28421 RVA: 0x002CD13B File Offset: 0x002CB33B
		private bool ProcessPendingCensorLeakClose()
		{
			if (!this._censorLeakExitRequested)
			{
				return false;
			}
			this._censorLeakExitRequested = false;
			if (!CensorLeakState.GameOverPending)
			{
				return false;
			}
			this.ExitToGameplay();
			return true;
		}

		// Token: 0x06006F06 RID: 28422 RVA: 0x002CD160 File Offset: 0x002CB360
		private void QueueCensorLeakGameOver()
		{
			this._censorLeakExitRequested = false;
			if (!CensorLeakState.GameOverPending)
			{
				return;
			}
			this.FreezeCiel(false);
			this.SetCensorLeakMovementLocked(true);
			if (this._censorLeakRoutine == null && base.gameObject.activeInHierarchy)
			{
				this._censorLeakRoutine = base.StartCoroutine(this.TriggerCensorLeakNextFrame());
			}
		}

		// Token: 0x06006F07 RID: 28423 RVA: 0x002CD1B1 File Offset: 0x002CB3B1
		private IEnumerator TriggerCensorLeakNextFrame()
		{
			yield return null;
			yield return new WaitForEndOfFrame();
			while (CensorLeakState.GameOverPending && GameOverManager.Instance == null)
			{
				yield return null;
			}
			bool gameOverPending = CensorLeakState.GameOverPending;
			CensorLeakState.Reset();
			this.SetCensorLeakMovementLocked(false);
			this._censorLeakRoutine = null;
			if (gameOverPending)
			{
				if (this.debugMode)
				{
					Debug.Log("[SmartphoneController] Censor leak pending -> TriggerCensorLeak()", this);
				}
				GameOverManager.Instance.TriggerCensorLeak();
			}
			yield break;
		}

		// Token: 0x06006F08 RID: 28424 RVA: 0x002CD1C0 File Offset: 0x002CB3C0
		private void EnterAndLockFirstPerson()
		{
			if (this.firstPersonController == null)
			{
				return;
			}
			this.firstPersonController.ExternalLock = false;
			this.firstPersonController.EnterFirstPerson();
			this.firstPersonController.ExternalLock = true;
		}

		// Token: 0x06006F09 RID: 28425 RVA: 0x002CD1F4 File Offset: 0x002CB3F4
		private void UnlockAndExitFirstPerson()
		{
			if (this.firstPersonController == null)
			{
				return;
			}
			this.firstPersonController.ExternalLock = false;
			this.firstPersonController.ExitFirstPerson();
		}

		// Token: 0x06006F0A RID: 28426 RVA: 0x002CD21C File Offset: 0x002CB41C
		private void AttachPhoneToHand()
		{
			if (this.phoneInstance == null)
			{
				return;
			}
			this.SanitizeToolkitPhone();
			if (this._handBone == null && this.playerAnimator != null)
			{
				this._handBone = this.playerAnimator.GetBoneTransform(HumanBodyBones.RightHand);
			}
			if (this._handBone == null)
			{
				if (this.debugMode)
				{
					Debug.LogWarning("[SmartphoneController] RightHand bone non risolto; telefono non agganciato.", this);
				}
				return;
			}
			if (this.phoneInstance.transform.parent != this._handBone)
			{
				this.phoneInstance.transform.SetParent(this._handBone, false);
			}
			this.ApplyPhonePose(this.attachLocalPosition, this.attachLocalEuler, this.attachLocalScale);
		}

		// Token: 0x06006F0B RID: 28427 RVA: 0x002CD2DC File Offset: 0x002CB4DC
		private void EnsurePhoneInstance()
		{
			if (this.phoneInstance != null)
			{
				return;
			}
			if (!string.IsNullOrEmpty(this.childPhoneName))
			{
				Transform transform = base.transform.Find(this.childPhoneName);
				if (transform != null)
				{
					this.phoneInstance = transform.gameObject;
					return;
				}
			}
			if (this.phonePrefab == null)
			{
				return;
			}
			this.phoneInstance = Object.Instantiate<GameObject>(this.phonePrefab);
			this.phoneInstance.name = this.phonePrefab.name + "_Runtime";
		}

		// Token: 0x06006F0C RID: 28428 RVA: 0x002CD370 File Offset: 0x002CB570
		private void SanitizeToolkitPhone()
		{
			if (this._toolkitSanitized || this.phoneInstance == null)
			{
				return;
			}
			this._toolkitSanitized = true;
			Canvas[] componentsInChildren = this.phoneInstance.GetComponentsInChildren<Canvas>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				GameplayHudVisibility.RegisterCanvas(componentsInChildren[i]);
			}
			if (this.disableToolkitEventSystem)
			{
				EventSystem componentInChildren = this.phoneInstance.GetComponentInChildren<EventSystem>(true);
				if (componentInChildren != null)
				{
					componentInChildren.gameObject.SetActive(false);
					if (this.debugMode)
					{
						Debug.Log("[SmartphoneController] EventSystem del toolkit disattivato (uso quello di scena).", this);
					}
				}
			}
			if (this.disableToolkitCameras)
			{
				global::ProjektSumperk.CameraController[] componentsInChildren2 = this.phoneInstance.GetComponentsInChildren<global::ProjektSumperk.CameraController>(true);
				for (int i = 0; i < componentsInChildren2.Length; i++)
				{
					componentsInChildren2[i].enabled = false;
				}
				Camera[] componentsInChildren3 = this.phoneInstance.GetComponentsInChildren<Camera>(true);
				foreach (Camera camera in componentsInChildren3)
				{
					camera.enabled = false;
					camera.gameObject.SetActive(false);
				}
				if (this.debugMode && componentsInChildren3.Length != 0)
				{
					Debug.Log(string.Format("[SmartphoneController] {0} camera/e embedded del toolkit disattivate.", componentsInChildren3.Length), this);
				}
			}
			this.EnsureNativeCameraBackGuard();
			PhoneExperienceCoordinator.Ensure(this.phoneInstance);
			PhoneScreenRaycaster.EnsureInstalled(this.phoneInstance, this);
		}

		// Token: 0x06006F0D RID: 28429 RVA: 0x002CD4A4 File Offset: 0x002CB6A4
		private void AssignPhoneCanvasEventCamera()
		{
			if (this.phoneInstance == null)
			{
				return;
			}
			PhoneScreenRaycaster.EnsureInstalled(this.phoneInstance, this);
			Camera main = Camera.main;
			if (main == null)
			{
				return;
			}
			Canvas[] componentsInChildren = this.phoneInstance.GetComponentsInChildren<Canvas>(true);
			int num = 0;
			foreach (Canvas canvas in componentsInChildren)
			{
				if (canvas.renderMode == RenderMode.WorldSpace && canvas.worldCamera != main)
				{
					canvas.worldCamera = main;
					num++;
				}
			}
			if (this.debugMode && num > 0)
			{
				Debug.Log(string.Format("[SmartphoneController] Event camera riallineata su {0} canvas del telefono.", num), this);
			}
		}

		// Token: 0x06006F0E RID: 28430 RVA: 0x002CD548 File Offset: 0x002CB748
		private void RaisePhoneCanvasSorting(bool raise)
		{
			if (!this.raisePhoneCanvasSorting)
			{
				return;
			}
			if (this._phoneCanvas == null && this.phoneInstance != null)
			{
				Canvas[] componentsInChildren = this.phoneInstance.GetComponentsInChildren<Canvas>(true);
				foreach (Canvas canvas in componentsInChildren)
				{
					if (canvas.renderMode == RenderMode.WorldSpace)
					{
						this._phoneCanvas = canvas;
						break;
					}
				}
				if (this._phoneCanvas == null && componentsInChildren.Length != 0)
				{
					this._phoneCanvas = componentsInChildren[0];
				}
			}
			if (this._phoneCanvas == null)
			{
				return;
			}
			if (raise)
			{
				if (!this._phoneCanvasSortingRaised)
				{
					this._savedOverrideSorting = this._phoneCanvas.overrideSorting;
					this._savedSortingOrder = this._phoneCanvas.sortingOrder;
					this._phoneCanvasSortingRaised = true;
				}
				this._phoneCanvas.overrideSorting = true;
				this._phoneCanvas.sortingOrder = this.phoneSortingOrder;
				return;
			}
			if (!this._phoneCanvasSortingRaised)
			{
				return;
			}
			this._phoneCanvas.overrideSorting = this._savedOverrideSorting;
			this._phoneCanvas.sortingOrder = this._savedSortingOrder;
			this._phoneCanvasSortingRaised = false;
		}

		// Token: 0x06006F0F RID: 28431 RVA: 0x002CD65C File Offset: 0x002CB85C
		private void EnableTapSound(bool enable)
		{
			if (this.tapSound == null)
			{
				return;
			}
			if (this._phoneCanvas == null && this.phoneInstance != null)
			{
				Canvas[] componentsInChildren = this.phoneInstance.GetComponentsInChildren<Canvas>(true);
				foreach (Canvas canvas in componentsInChildren)
				{
					if (canvas.renderMode == RenderMode.WorldSpace)
					{
						this._phoneCanvas = canvas;
						break;
					}
				}
				if (this._phoneCanvas == null && componentsInChildren.Length != 0)
				{
					this._phoneCanvas = componentsInChildren[0];
				}
			}
			if (this._tapSoundPlayer == null)
			{
				this._tapSoundPlayer = base.GetComponent<PhoneTapSound>();
				if (this._tapSoundPlayer == null)
				{
					this._tapSoundPlayer = base.gameObject.AddComponent<PhoneTapSound>();
				}
			}
			this._tapSoundPlayer.Configure(this.tapSound, this.tapVolume, this._phoneCanvas);
			this._tapSoundPlayer.SetActive(enable);
		}

		// Token: 0x06006F10 RID: 28432 RVA: 0x002CD744 File Offset: 0x002CB944
		private void SetConfiguredUiHidden(bool hidden)
		{
			if (hidden)
			{
				if (this._configuredUiHidden)
				{
					return;
				}
				this._configuredUiHidden = true;
				this._hiddenUi.Clear();
				if (this.uiToHideWhilePhoneOut == null)
				{
					return;
				}
				foreach (GameObject gameObject in this.uiToHideWhilePhoneOut)
				{
					if (gameObject != null && gameObject.activeSelf)
					{
						gameObject.SetActive(false);
						this._hiddenUi.Add(gameObject);
					}
				}
				return;
			}
			else
			{
				if (!this._configuredUiHidden)
				{
					return;
				}
				foreach (GameObject gameObject2 in this._hiddenUi)
				{
					if (gameObject2 != null)
					{
						gameObject2.SetActive(true);
					}
				}
				this._hiddenUi.Clear();
				this._configuredUiHidden = false;
				return;
			}
		}

		// Token: 0x06006F11 RID: 28433 RVA: 0x002CD824 File Offset: 0x002CBA24
		private void SetTripodMinimalHud(bool active)
		{
			if (this._tripodMinimalHudActive == active)
			{
				return;
			}
			if (active)
			{
				this.SetConfiguredUiHidden(false);
				this.ResolveTripodRapidActions();
				this._tripodUiSnapshots.Clear();
				this._tripodUiSnapshotTargets.Clear();
				if (this.uiToHideWhilePhoneOut != null)
				{
					GameObject[] array = this.uiToHideWhilePhoneOut;
					for (int i = 0; i < array.Length; i++)
					{
						GameObject root = array[i];
						if (!(root == null))
						{
							List<Transform> list = new List<Transform>();
							if (this._tripodRapidActions != null)
							{
								list.Add(this._tripodRapidActions.transform);
							}
							InteractionHUDController instance = InteractionHUDController.Instance;
							if (instance != null)
							{
								list.Add(instance.transform);
							}
							foreach (InventoryHUD inventoryHUD in root.GetComponentsInChildren<InventoryHUD>(true))
							{
								list.Add(inventoryHUD.transform);
							}
							foreach (ClothHUDController clothHUDController in root.GetComponentsInChildren<ClothHUDController>(true))
							{
								list.Add(clothHUDController.transform);
							}
							this.CaptureTripodUiTree(root, list);
							if (list.Exists((Transform branch) => SmartphoneController.ContainsBranch(root.transform, branch)))
							{
								root.SetActive(true);
								SmartphoneController.ActivateOnlyBranches(root.transform, list);
							}
							else
							{
								root.SetActive(false);
							}
						}
					}
				}
				RapidActionsHUDController tripodRapidActions = this._tripodRapidActions;
				if (tripodRapidActions != null)
				{
					tripodRapidActions.SetTripodInputMode(true);
				}
				if (this._radialMenu != null)
				{
					this._radialMenu.SetPersistentHudVisible(false);
				}
				this._tripodMinimalHudActive = true;
				return;
			}
			RapidActionsHUDController tripodRapidActions2 = this._tripodRapidActions;
			if (tripodRapidActions2 != null)
			{
				tripodRapidActions2.CancelTransientInput();
			}
			RapidActionsHUDController tripodRapidActions3 = this._tripodRapidActions;
			if (tripodRapidActions3 != null)
			{
				tripodRapidActions3.SetTripodInputMode(false);
			}
			if (this._radialMenu != null)
			{
				this._radialMenu.SetPersistentHudVisible(true);
			}
			for (int k = this._tripodUiSnapshots.Count - 1; k >= 0; k--)
			{
				SmartphoneController.TripodUiSnapshot tripodUiSnapshot = this._tripodUiSnapshots[k];
				if (tripodUiSnapshot.target != null && tripodUiSnapshot.target.activeSelf != tripodUiSnapshot.activeSelf)
				{
					tripodUiSnapshot.target.SetActive(tripodUiSnapshot.activeSelf);
				}
			}
			this._tripodUiSnapshots.Clear();
			this._tripodUiSnapshotTargets.Clear();
			this._tripodMinimalHudActive = false;
		}

		// Token: 0x06006F12 RID: 28434 RVA: 0x002CDA93 File Offset: 0x002CBC93
		private void ResolveTripodRapidActions()
		{
			if (this._tripodRapidActions != null)
			{
				return;
			}
			this._tripodRapidActions = Object.FindObjectOfType<RapidActionsHUDController>(true);
		}

		// Token: 0x06006F13 RID: 28435 RVA: 0x002CDAB0 File Offset: 0x002CBCB0
		private void CaptureTripodUiTree(GameObject root, List<Transform> keepBranches)
		{
			Transform[] componentsInChildren = root.GetComponentsInChildren<Transform>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Transform item = componentsInChildren[i];
				if (!(item == null) && this._tripodUiSnapshotTargets.Add(item.gameObject) && !keepBranches.Exists((Transform branch) => item != branch && item.IsChildOf(branch)))
				{
					this._tripodUiSnapshots.Add(new SmartphoneController.TripodUiSnapshot
					{
						target = item.gameObject,
						activeSelf = item.gameObject.activeSelf
					});
				}
			}
		}

		// Token: 0x06006F14 RID: 28436 RVA: 0x002CDB5F File Offset: 0x002CBD5F
		private static bool ContainsBranch(Transform root, Transform keep)
		{
			return keep != null && (keep == root || keep.IsChildOf(root));
		}

		// Token: 0x06006F15 RID: 28437 RVA: 0x002CDB80 File Offset: 0x002CBD80
		private static void ActivateOnlyBranches(Transform root, List<Transform> keepBranches)
		{
			if (keepBranches.Contains(root))
			{
				return;
			}
			for (int i = 0; i < root.childCount; i++)
			{
				Transform child = root.GetChild(i);
				bool flag = keepBranches.Exists((Transform branch) => SmartphoneController.ContainsBranch(child, branch));
				if (child.gameObject.activeSelf != flag)
				{
					child.gameObject.SetActive(flag);
				}
				if (flag)
				{
					SmartphoneController.ActivateOnlyBranches(child, keepBranches);
				}
			}
		}

		// Token: 0x06006F16 RID: 28438 RVA: 0x002CDC04 File Offset: 0x002CBE04
		private void PrepareForTripodStateChange(bool stopCharacterAction)
		{
			this.ResolveTripodRapidActions();
			RapidActionsHUDController tripodRapidActions = this._tripodRapidActions;
			if (tripodRapidActions != null)
			{
				tripodRapidActions.CancelTransientInput();
			}
			SelfieRadialMenuController selfieRadialMenuController = this.ResolveRadialMenu();
			if (selfieRadialMenuController != null)
			{
				selfieRadialMenuController.CancelTransientInput();
			}
			if (!stopCharacterAction)
			{
				return;
			}
			if (this._gameplayEmoteController == null)
			{
				this._gameplayEmoteController = base.GetComponentInParent<GameplayEmoteController>(true);
			}
			GameplayEmoteController gameplayEmoteController = this._gameplayEmoteController;
			if (gameplayEmoteController != null)
			{
				gameplayEmoteController.ForceEndEmoteImmediate();
			}
			CoverPoseController instance = CoverPoseController.Instance;
			if (instance != null && instance.IsCovering)
			{
				instance.DeactivateCoverPose();
			}
		}

		// Token: 0x06006F17 RID: 28439 RVA: 0x002CDC86 File Offset: 0x002CBE86
		private void SetPhoneHeldParam(bool value)
		{
			if (this.playerAnimator == null || string.IsNullOrEmpty(this.phoneHeldParam))
			{
				return;
			}
			this.playerAnimator.SetBool(this.phoneHeldParam, value);
		}

		// Token: 0x06006F18 RID: 28440 RVA: 0x002CDCB8 File Offset: 0x002CBEB8
		private void DriveHoldLayer()
		{
			if (this.playerAnimator == null || this._holdLayerIndex < 0)
			{
				return;
			}
			if (this._ci_frozen)
			{
				return;
			}
			if (!this._owningHoldLayer)
			{
				return;
			}
			float layerWeight = this.playerAnimator.GetLayerWeight(this._holdLayerIndex);
			if (Mathf.Approximately(layerWeight, this._holdLayerTargetWeight))
			{
				if (this._holdLayerTargetWeight <= 0f)
				{
					this._owningHoldLayer = false;
				}
				return;
			}
			float num = Mathf.MoveTowards(layerWeight, this._holdLayerTargetWeight, this.holdLayerBlendSpeed * Time.deltaTime);
			this.playerAnimator.SetLayerWeight(this._holdLayerIndex, num);
		}

		// Token: 0x04007136 RID: 28982
		private bool _controllerPhoneViewReady;

		// Token: 0x04007137 RID: 28983
		private readonly Vector3[] _controllerScreenCorners = new Vector3[4];

		// Token: 0x04007138 RID: 28984
		[Header("Input")]
		[Tooltip("Tasto globale che tira fuori / ripone il telefono. Default P (come Tab per CCTV, I per inventario).")]
		public KeyCode toggleKey = KeyCode.P;

		// Token: 0x04007139 RID: 28985
		[Tooltip("Tasto che apre/chiude il menu pose/facce in modalita' selfie (e sblocca il cursore per cliccare i dropdown). Default Tab.")]
		public KeyCode selfiePoseMenuKey = KeyCode.Tab;

		// Token: 0x0400713A RID: 28986
		[Tooltip("Tasto per USCIRE dalla modalita' selfie e tornare alla schermata del telefono. Default X (non Escape: in editor Escape sblocca il cursore di Play, interferendo con la gestione locked/unlocked del telefono).")]
		public KeyCode selfieExitKey = KeyCode.X;

		// Token: 0x0400713B RID: 28987
		[Header("Refs")]
		[Tooltip("Animator del player. Se null, risolto via GetComponentInParent in Awake.")]
		public Animator playerAnimator;

		// Token: 0x0400713C RID: 28988
		[Tooltip("Istanza del telefono (mesh + schermo World-Space del toolkit) da agganciare alla mano. Puo' essere un figlio gia' presente nel prefab oppure un prefab istanziato a runtime se phonePrefab e' valorizzato.")]
		public GameObject phoneInstance;

		// Token: 0x0400713D RID: 28989
		[Tooltip("Prefab del telefono da istanziare a runtime sotto il bone della mano destra, se phoneInstance non e' assegnato.")]
		public GameObject phonePrefab;

		// Token: 0x0400713E RID: 28990
		[Tooltip("Se phoneInstance e phonePrefab sono entrambi nulli, cerca a runtime un figlio con questo nome (il Phone fisso parcheggiato sotto SmartphoneSystem). Evita di dover serializzare una reference interna al prefab.")]
		public string childPhoneName = "Phone";

		// Token: 0x0400713F RID: 28991
		[Header("Attach in mano (offset locali rispetto al bone RightHand)")]
		[Tooltip("Posizione locale del telefono quando agganciato alla mano destra. Tarata a mano in Play.")]
		public Vector3 attachLocalPosition = new Vector3(-0.0423f, 0.0234f, -0.0146f);

		// Token: 0x04007140 RID: 28992
		[Tooltip("Rotazione locale (euler) del telefono quando agganciato alla mano destra. Tarata a mano in Play.")]
		public Vector3 attachLocalEuler = new Vector3(28.344f, 189.572f, 57.375f);

		// Token: 0x04007141 RID: 28993
		[Tooltip("Scala locale del telefono quando agganciato alla mano destra. Tarata a mano in Play.")]
		public Vector3 attachLocalScale = new Vector3(0.1f, 0.1f, 0.1f);

		// Token: 0x04007142 RID: 28994
		[Header("Attach in modalita' selfie (override della posa telefono)")]
		[Tooltip("Posizione locale del telefono mentre si e' in selfie (override della posa default). Tarata in Play.")]
		public Vector3 selfiePhoneLocalPosition = new Vector3(0.0248f, 0.043f, -0.0264f);

		// Token: 0x04007143 RID: 28995
		[Tooltip("Rotazione locale (euler) del telefono in selfie. Tarata in Play.")]
		public Vector3 selfiePhoneLocalEuler = new Vector3(33.111f, -179.861f, -67.906f);

		// Token: 0x04007144 RID: 28996
		[Tooltip("Scala locale del telefono in selfie. Tarata in Play.")]
		public Vector3 selfiePhoneLocalScale = new Vector3(0.1f, 0.1f, 0.1f);

		// Token: 0x04007145 RID: 28997
		[Header("UI / priorita' canvas")]
		[Tooltip("Mentre il telefono e' in mano, alza il sortingOrder del suo Canvas World-Space sopra l'HUD (stesso pattern di InventoryDialogController). Senza questo, i Canvas dell'HUD coprono lo schermo e ne mangiano i click, rendendolo non interagibile.")]
		public bool raisePhoneCanvasSorting = true;

		// Token: 0x04007146 RID: 28998
		[Tooltip("sortingOrder applicato al Canvas del telefono mentre e' attivo. L'HUD arriva a ~200, quindi tienilo comodamente piu' alto. Default 500.")]
		public int phoneSortingOrder = 500;

		// Token: 0x04007147 RID: 28999
		[Tooltip("Pezzi di UI (GameObject) da spegnere mentre il telefono e' in mano, e riaccendere quando si ripone. Configurabili a mano: trascina qui gli oggetti HUD/overlay che danno fastidio o coprono lo schermo.")]
		public GameObject[] uiToHideWhilePhoneOut;

		// Token: 0x04007148 RID: 29000
		[Header("Toolkit cleanup")]
		[Tooltip("Disattiva l'EventSystem che il Phone.prefab del toolkit porta dietro. Unity ammette un solo EventSystem attivo: lasciamo quello della scena e il Canvas World-Space del telefono ci si appoggia. Tieni ON salvo tu non abbia rimosso a mano l'EventSystem dal Phone.")]
		public bool disableToolkitEventSystem = true;

		// Token: 0x04007149 RID: 29001
		[Tooltip("Disattiva le camere embedded del toolkit (cFront/cBack). Non le usiamo: in selfie e' la MainCamera (prima persona) a trasformarsi in camera selfie davanti a Ciel, ancorata al telefono. Quindi le camere del toolkit restano spente.")]
		public bool disableToolkitCameras = true;

		// Token: 0x0400714A RID: 29002
		[Header("Audio")]
		[Tooltip("Suono di 'tap' riprodotto a ogni click su un elemento UI dello schermo del telefono (qualsiasi app). Se null, nessun suono.")]
		public AudioClip tapSound;

		// Token: 0x0400714B RID: 29003
		[Tooltip("Volume del suono di tap [0..1].")]
		[Range(0f, 1f)]
		public float tapVolume = 1f;

		// Token: 0x0400714C RID: 29004
		[Header("Animator")]
		[Tooltip("Nome del layer animator usato per la posa base 'reggere il telefono'. Default 'HoldingItem' (esistente).")]
		public string holdLayerName = "HoldingItem";

		// Token: 0x0400714D RID: 29005
		[Tooltip("Bool che attiva lo stato dedicato 'PhoneHold' dentro il layer HoldingItem (stato ad hoc, separato dallo stato 'Holding' usato dall'inventario). Settato true mentre il telefono e' in mano.")]
		public string phoneHeldParam = "PhoneHeld";

		// Token: 0x0400714E RID: 29006
		[Tooltip("Bool che attiva lo stato 'Selfie' (clip Pose20) sul layer HoldingItem quando il player apre la fotocamera del telefono in modalita' frontale (selfie verso Ciel).")]
		public string selfiePoseParam = "SelfiePose";

		// Token: 0x0400714F RID: 29007
		[Header("Fotocamera - treppiede remoto")]
		[Tooltip("Prefab visivo del treppiede. Se null, il sistema prova a riusare un oggetto di scena chiamato 'Tripod'.")]
		public GameObject tripodPrefab;

		// Token: 0x04007150 RID: 29008
		[Tooltip("Portata massima indicativa del ray di piazzamento davanti a Ciel.")]
		[Range(2.5f, 7f)]
		public float tripodPlacementDistance = 5f;

		// Token: 0x04007151 RID: 29009
		[Tooltip("Altezza dell'obiettivo del telefono rispetto al terreno.")]
		[Range(0.8f, 2f)]
		public float tripodCameraHeight = 1.28f;

		// Token: 0x04007152 RID: 29010
		[Tooltip("Altezza del punto su Ciel inquadrato al momento del piazzamento.")]
		[Range(0.5f, 1.8f)]
		public float tripodFramingHeight = 1.08f;

		// Token: 0x04007153 RID: 29011
		[Tooltip("Durata della transizione tra telefono in mano e POV del treppiede.")]
		[Range(0.15f, 1.2f)]
		public float tripodCameraTransitionDuration = 0.48f;

		// Token: 0x04007154 RID: 29012
		[Tooltip("Layer considerati terreno/ostacoli durante la preview di piazzamento.")]
		public LayerMask tripodPlacementMask = -1;

		// Token: 0x04007155 RID: 29013
		[Tooltip("Scala minima del treppiede (freccia giu').")]
		[Range(0.6f, 1f)]
		public float tripodMinHeightScale = 0.78f;

		// Token: 0x04007156 RID: 29014
		[Tooltip("Scala massima del treppiede (freccia su').")]
		[Range(1f, 1.6f)]
		public float tripodMaxHeightScale = 1.28f;

		// Token: 0x04007157 RID: 29015
		[Tooltip("Velocita' di regolazione dell'altezza del treppiede.")]
		[Range(0.1f, 1f)]
		public float tripodHeightScaleSpeed = 0.36f;

		// Token: 0x04007158 RID: 29016
		[Header("Selfie - ancoraggio camera al telefono")]
		[Tooltip("Offset locale (rispetto al telefono in mano) dove ancorare la MainCamera in selfie. La camera si mette qui e guarda verso il volto di Ciel. Tarata in Play.")]
		public Vector3 selfieCamLocalPosition = new Vector3(0.044f, 0.92f, 0.198f);

		// Token: 0x04007159 RID: 29017
		[Tooltip("Rotazione locale (euler, rispetto al telefono) della camera selfie. Tarata in Play per puntarla verso il volto di Ciel.")]
		public Vector3 selfieCamLocalEuler = new Vector3(14.18f, 14.486f, 93.783f);

		// Token: 0x0400715A RID: 29018
		[Header("Fotocamera frontale/esterna (tasto R)")]
		[Tooltip("Offset locale (rispetto al telefono) dove ancorare la MainCamera quando si passa alla fotocamera ESTERNA (rear, punta davanti a Ciel come la cam posteriore di un telefono). Indipendente dalla camera selfie: tarala in Play col Selfie Pose Author.")]
		public Vector3 rearCamLocalPosition = new Vector3(0.044f, 0.92f, 0.198f);

		// Token: 0x0400715B RID: 29019
		[Tooltip("Rotazione locale (euler, rispetto al telefono) della fotocamera esterna. Tarata in Play per puntarla davanti a Ciel.")]
		public Vector3 rearCamLocalEuler = new Vector3(14.18f, 194.486f, 93.783f);

		// Token: 0x0400715C RID: 29020
		[Tooltip("Spunta automatica quando la fotocamera esterna e' stata TARATA (cattura dal tool). Finche' e' false, la camera esterna usa il fallback storico (offset selfie + flip 180' sull'asse verticale) cosi' punta comunque davanti a Ciel invece che sul telefono.")]
		public bool rearCamTuned;

		// Token: 0x0400715D RID: 29021
		[Tooltip("Config per-posa (offset telefono+camera per ogni posa selfie). Ogni posa muove il braccio diversamente, quindi telefono e camera vanno riposizionati. Se null o la posa non ha entry, si usano gli offset selfie di default qui sopra. Create > Phone Companion.")]
		public SelfiePoseConfigSO selfiePoseConfig;

		// Token: 0x0400715E RID: 29022
		[Tooltip("Overlay UI della fotocamera selfie GIA' IN SCENA (mirino + shutter + zoom + flash), editabile nell'editor. Assegna il GameObject 'SelfieCameraOverlay'. Se null, l'overlay viene costruito da codice (legacy).")]
		public SelfieCameraOverlayUI selfieCameraOverlay;

		// Token: 0x0400715F RID: 29023
		[Tooltip("Se true, il menu pose/facce usa il RADIALE (hold 1/2) invece del dropdown TAB legacy.")]
		public bool useRadialMenu = true;

		// Token: 0x04007160 RID: 29024
		[Tooltip("Controller del menu radiale pose/facce in scena. Se null (controller su prefab), auto-find in scena. Usato solo se useRadialMenu = true.")]
		public SelfieRadialMenuController selfieRadialMenu;

		// Token: 0x04007161 RID: 29025
		[Tooltip("Durata (s) dell'interpolazione degli offset telefono/camera al cambio posa: telefono e camera 'seguono' il braccio mentre la posa fa blending. Allineala al blend del layer SelfiePose dell'Animator (~0.65s).")]
		public float selfiePoseOffsetBlendDuration = 0.65f;

		// Token: 0x04007162 RID: 29026
		[Header("Selfie - photo mode (stile Stellar Blade)")]
		[Tooltip("FOV della camera selfie a zoom 'wide' (rotellina giu'). Stellar Blade usa ~100.")]
		public float selfieFovMax = 100f;

		// Token: 0x04007163 RID: 29027
		[Tooltip("FOV della camera selfie a zoom 'narrow' (rotellina su'). Stellar Blade usa ~17.")]
		public float selfieFovMin = 17f;

		// Token: 0x04007164 RID: 29028
		[Tooltip("FOV iniziale della camera selfie all'ingresso (reset). Buon default ~60.")]
		public float selfieFovDefault = 60f;

		// Token: 0x04007165 RID: 29029
		[Tooltip("Gradi di FOV per tacca di rotellina in selfie.")]
		public float selfieFovScrollSpeed = 8f;

		// Token: 0x04007166 RID: 29030
		[Tooltip("Sensibilita' del mouse sulla direzione di puntamento della camera selfie.")]
		public float selfieLookSensitivity = 2f;

		// Token: 0x04007167 RID: 29031
		[Tooltip("Limite orizzontale (gradi, +/-) di quanto il mouse puo' ruotare la direzione camera selfie (a FOV wide).")]
		public float selfieYawLimit = 50f;

		// Token: 0x04007168 RID: 29032
		[Tooltip("Limite verticale (gradi, +/-) di quanto il mouse puo' inclinare la direzione camera selfie (a FOV wide).")]
		public float selfiePitchLimit = 42f;

		// Token: 0x04007169 RID: 29033
		[Tooltip("Frazione del limite visuale usata a FOV massimo (wide). A FOV narrow si usa il limite pieno. Piu' basso = a FOV alto la visuale si muove meno (cosi' il telefono non entra in campo).")]
		[Range(0.1f, 1f)]
		public float selfieLookLimitAtMaxFov = 0.35f;

		// Token: 0x0400716A RID: 29034
		[Tooltip("Velocita' di rotazione del player sul posto con le frecce sinistra/destra in selfie (gradi/sec).")]
		public float selfiePlayerRotateSpeed = 50f;

		// Token: 0x0400716B RID: 29035
		[Tooltip("Velocita' (gradi/sec) con cui il player curva verso la direzione di movimento WASD in selfie.")]
		public float selfieMoveTurnSpeed = 140f;

		// Token: 0x0400716C RID: 29036
		[Header("Selfie - IK mano destra (frecce su/giu')")]
		[Tooltip("Usa il Cover Rig del CharacterRigController per modulare leggermente la posa della mano destra in selfie, cambiando il POV della foto con un piccolo movimento ad arco.")]
		public bool enableSelfieHandIK = true;

		// Token: 0x0400716D RID: 29037
		[Tooltip("Ampiezza dell'arco (m): di quanto il target IK e' spostato sopra/sotto la posa originale. Il movimento reale e' una frazione di questo, modulata dal weight.")]
		public float selfieHandIKArc = 0.07f;

		// Token: 0x0400716E RID: 29038
		[Tooltip("Velocita' con cui le frecce su/giu' alzano/abbassano il blend del weight IK [0..1] al secondo.")]
		public float selfieHandIKBlendSpeed = 2.2f;

		// Token: 0x0400716F RID: 29039
		[Tooltip("Peso IK massimo del constraint mano a blend pieno (1 = pieno; piu' basso = effetto piu' sottile).")]
		[Range(0f, 1f)]
		public float selfieHandIKMaxWeight = 0.75f;

		// Token: 0x04007170 RID: 29040
		[Header("Selfie - aggiusta testa (SHIFT + mouse)")]
		[Tooltip("In selfie, tenendo premuto SHIFT il movimento del mouse riposiziona la testa di Ciel (destra/sinistra = yaw, su/giu' = pitch) invece di muovere la camera. Molto smooth e lento per posizionarla con precisione. Clampato cosi' la posa non diventa brutta.")]
		public bool enableSelfieHeadAdjust = true;

		// Token: 0x04007171 RID: 29041
		[Tooltip("Sensibilita' (lenta) del mouse sull'aggiustamento testa con SHIFT. Bassa di proposito per un controllo fine.")]
		public float selfieHeadSensitivity = 0.45f;

		// Token: 0x04007172 RID: 29042
		[Tooltip("Limite orizzontale (gradi, +/-) di rotazione testa con SHIFT. Tienilo contenuto per non rompere la posa.")]
		public float selfieHeadYawLimit = 32f;

		// Token: 0x04007173 RID: 29043
		[Tooltip("Limite verticale (gradi, +/-) di inclinazione testa con SHIFT (su/giu').")]
		public float selfieHeadPitchLimit = 22f;

		// Token: 0x04007174 RID: 29044
		[Tooltip("Velocita' di smoothing con cui la testa raggiunge l'offset target (piu' basso = piu' morbido/lento).")]
		public float selfieHeadSmoothSpeed = 6f;

		// Token: 0x04007175 RID: 29045
		[Tooltip("Quanto velocemente il layer di hold sale/scende di weight (0->1) quando si apre/chiude il telefono.")]
		public float holdLayerBlendSpeed = 6f;

		// Token: 0x04007176 RID: 29046
		[Header("First Person")]
		[Tooltip("FirstPersonController del player. Se null, risolto via GetComponentInParent in Awake. Col telefono in mano la FP viene forzata e bloccata (non togglabile col tasto V) finche' non si ripone il telefono.")]
		public FirstPersonController firstPersonController;

		// Token: 0x04007177 RID: 29047
		[Header("Zoom verso lo schermo (rotellina)")]
		[Tooltip("FOV della MainCamera quando lo zoom e' completamente indietro (nessuno zoom). Tipicamente il FOV FP base.")]
		public float fovZoomedOut = 70f;

		// Token: 0x04007178 RID: 29048
		[Tooltip("FOV minimo a zoom massimo: piu' basso = zoom piu' potente / piu' vicino allo schermo.")]
		public float fovZoomedIn = 16f;

		// Token: 0x04007179 RID: 29049
		[Tooltip("Sensibilita' della rotellina sullo zoom (quanto FOV per tacca di scroll).")]
		public float zoomScrollSpeed = 30f;

		// Token: 0x0400717A RID: 29050
		[Tooltip("Velocita' di interpolazione smooth del FOV verso il target.")]
		public float zoomSmoothSpeed = 10f;

		// Token: 0x0400717B RID: 29051
		[Tooltip("Soglia di zoom normalizzata [0..1] oltre la quale lo schermo e' ben visibile e si sblocca il cursore. 0 = nessuno zoom, 1 = zoom massimo. Es. 0.6 = serve il 60% dello zoom per interagire.")]
		[Range(0f, 1f)]
		public float cursorUnlockThreshold = 0.6f;

		// Token: 0x0400717C RID: 29052
		[Header("Aggiusta visuale (drag tasto centrale / rotellina)")]
		[Tooltip("Mentre il cursore e' sbloccato: tieni premuto il tasto centrale (rotellina) e trascina su/giu' per aggiustare l'inquadratura del telefono. Il sinistro resta libero per interagire con le app.")]
		public bool enableDragToAdjust = true;

		// Token: 0x0400717D RID: 29053
		[Tooltip("Sensibilita' del drag (tasto centrale) sull'aggiustamento verticale della visuale. Piu' alto = drag piu' veloce.")]
		public float dragLookSensitivity = 1.5f;

		// Token: 0x0400717E RID: 29054
		[Tooltip("Smoothing del drag col tasto centrale: piu' BASSO = piu' fluido/morbido (il movimento scivola dopo aver fermato il mouse), piu' alto = piu' reattivo/rigido.")]
		public float dragSmoothSpeed = 8f;

		// Token: 0x0400717F RID: 29055
		[Header("Debug")]
		public bool debugMode;

		// Token: 0x04007181 RID: 29057
		private const string ActivityGateReason = "Smartphone.Session";

		// Token: 0x04007182 RID: 29058
		private const PlayerActivity SessionRequiredActivities = PlayerActivity.Phone;

		// Token: 0x04007183 RID: 29059
		private const PlayerContextResource TripodResources = PlayerContextResource.Camera;

		// Token: 0x04007184 RID: 29060
		private const PlayerActivity TripodBlockedActivities = PlayerActivity.Cover | PlayerActivity.DebugFly;

		// Token: 0x04007185 RID: 29061
		private const PlayerContextResource HandheldResources = PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation | PlayerContextResource.HeldItem;

		// Token: 0x04007186 RID: 29062
		private const PlayerActivity HandheldBlockedActivities = PlayerActivity.Emote | PlayerActivity.Cover | PlayerActivity.Flash | PlayerActivity.RapidHUD | PlayerActivity.Interact | PlayerActivity.Undress | PlayerActivity.LingerieOpen | PlayerActivity.Inventory | PlayerActivity.DebugFly;

		// Token: 0x04007187 RID: 29063
		private PlayerActivityLease _activityLease;

		// Token: 0x04007188 RID: 29064
		private int _cameraToggleConsumedFrame = -1;

		// Token: 0x04007189 RID: 29065
		private static Action _customAppCloser;

		// Token: 0x0400718A RID: 29066
		private static SmartphoneController _activeInstance;

		// Token: 0x0400718B RID: 29067
		private Transform _handBone;

		// Token: 0x0400718C RID: 29068
		private int _holdLayerIndex = -1;

		// Token: 0x0400718D RID: 29069
		private float _holdLayerTargetWeight;

		// Token: 0x0400718E RID: 29070
		private bool _owningHoldLayer;

		// Token: 0x0400718F RID: 29071
		private bool _toolkitSanitized;

		// Token: 0x04007190 RID: 29072
		private PlayerInventoryManager _inventoryManager;

		// Token: 0x04007191 RID: 29073
		private Camera _mainCamera;

		// Token: 0x04007192 RID: 29074
		private vThirdPersonInput _tpInput;

		// Token: 0x04007193 RID: 29075
		private float _zoom01;

		// Token: 0x04007194 RID: 29076
		private float _zoomTarget01;

		// Token: 0x04007195 RID: 29077
		private float _savedFov;

		// Token: 0x04007196 RID: 29078
		private bool _cursorUnlocked;

		// Token: 0x04007197 RID: 29079
		private CursorLockMode _savedCursorLock;

		// Token: 0x04007198 RID: 29080
		private bool _savedCursorVisible;

		// Token: 0x04007199 RID: 29081
		private bool _savedLockCameraInput;

		// Token: 0x0400719A RID: 29082
		private bool _ci_frozen;

		// Token: 0x0400719B RID: 29083
		private bool _ci_freezeRequested;

		// Token: 0x0400719C RID: 29084
		private bool _censorLeakMovementLocked;

		// Token: 0x0400719D RID: 29085
		private bool _censorLeakExitRequested;

		// Token: 0x0400719E RID: 29086
		private bool _resumeCensorLeak;

		// Token: 0x0400719F RID: 29087
		private Coroutine _censorLeakRoutine;

		// Token: 0x040071A0 RID: 29088
		private float _savedAnimatorSpeed = 1f;

		// Token: 0x040071A1 RID: 29089
		private bool _savedAnimatorEnabled;

		// Token: 0x040071A2 RID: 29090
		private bool _savedKeepAnimatorStateOnDisable;

		// Token: 0x040071A3 RID: 29091
		private PlayableGraph _frozenAnimatorGraph;

		// Token: 0x040071A4 RID: 29092
		private PlayableGraph _frozenRigGraph;

		// Token: 0x040071A5 RID: 29093
		private bool _resumeAnimatorGraph;

		// Token: 0x040071A6 RID: 29094
		private bool _resumeRigGraph;

		// Token: 0x040071A7 RID: 29095
		private bool _savedLockMoveInput;

		// Token: 0x040071A8 RID: 29096
		private bool _savedMotorLockMovement;

		// Token: 0x040071A9 RID: 29097
		private bool _savedMotorLockRotation;

		// Token: 0x040071AA RID: 29098
		private vThirdPersonController _frozenMotor;

		// Token: 0x040071AB RID: 29099
		private Rigidbody _frozenBody;

		// Token: 0x040071AC RID: 29100
		private RigidbodyConstraints _savedBodyConstraints;

		// Token: 0x040071AD RID: 29101
		private RigidbodyInterpolation _savedBodyInterpolation;

		// Token: 0x040071AE RID: 29102
		private bool _savedBodyKinematic;

		// Token: 0x040071AF RID: 29103
		private Transform _selfieCamAnchor;

		// Token: 0x040071B0 RID: 29104
		private bool _selfieActive;

		// Token: 0x040071B1 RID: 29105
		private GameObject _tripodInstance;

		// Token: 0x040071B2 RID: 29106
		private bool _tripodActive;

		// Token: 0x040071B3 RID: 29107
		private bool _tripodTransitioning;

		// Token: 0x040071B4 RID: 29108
		private bool _tripodPlacementActive;

		// Token: 0x040071B5 RID: 29109
		private bool _tripodRestoreInProgress;

		// Token: 0x040071B6 RID: 29110
		private bool _pendingTripodRestore;

		// Token: 0x040071B7 RID: 29111
		private bool _pendingTripodReturnToPhoneHeld;

		// Token: 0x040071B8 RID: 29112
		private int _pendingTripodRestoreEarliestFrame;

		// Token: 0x040071B9 RID: 29113
		private PlacementSurfaceDetector _tripodSurfaceDetector;

		// Token: 0x040071BA RID: 29114
		private PlacementGhost _tripodPlacementGhost;

		// Token: 0x040071BB RID: 29115
		private PlacementResult _tripodPlacementResult;

		// Token: 0x040071BC RID: 29116
		private Transform _tripodCameraBlendAnchor;

		// Token: 0x040071BD RID: 29117
		private Transform _phoneParentBeforeTripod;

		// Token: 0x040071BE RID: 29118
		private Coroutine _tripodTransition;

		// Token: 0x040071BF RID: 29119
		private Vector3 _tripodBaseScale = Vector3.one;

		// Token: 0x040071C0 RID: 29120
		private Vector3 _tripodGroundPosition;

		// Token: 0x040071C1 RID: 29121
		private Vector3 _phoneWorldScaleOnTripod = Vector3.one;

		// Token: 0x040071C2 RID: 29122
		private float _tripodHeightScale = 1f;

		// Token: 0x040071C3 RID: 29123
		private readonly Dictionary<Renderer, ShadowCastingMode> _tripodPlacementHiddenRenderers = new Dictionary<Renderer, ShadowCastingMode>();

		// Token: 0x040071C5 RID: 29125
		private float _selfieFov;

		// Token: 0x040071C6 RID: 29126
		private float _selfieYaw;

		// Token: 0x040071C7 RID: 29127
		private float _selfiePitch;

		// Token: 0x040071C8 RID: 29128
		private Quaternion _selfieBaseLocalRot;

		// Token: 0x040071C9 RID: 29129
		private bool _selfieRearFacing;

		// Token: 0x040071CA RID: 29130
		private Vector3 _selfiePhonePos;

		// Token: 0x040071CB RID: 29131
		private Vector3 _selfiePhoneEul;

		// Token: 0x040071CC RID: 29132
		private Vector3 _selfiePhoneScale;

		// Token: 0x040071CD RID: 29133
		private Vector3 _selfiePhonePosTgt;

		// Token: 0x040071CE RID: 29134
		private Vector3 _selfiePhoneEulTgt;

		// Token: 0x040071CF RID: 29135
		private Vector3 _selfiePhoneScaleTgt;

		// Token: 0x040071D0 RID: 29136
		private Vector3 _selfieCamPos;

		// Token: 0x040071D1 RID: 29137
		private Vector3 _selfieCamPosTgt;

		// Token: 0x040071D2 RID: 29138
		private Quaternion _selfieCamBaseRotTgt;

		// Token: 0x040071D3 RID: 29139
		private Vector3 _selfieCamEulNeutral;

		// Token: 0x040071D4 RID: 29140
		private Vector3 _selfieCamPosSelfie;

		// Token: 0x040071D5 RID: 29141
		private float _selfiePoseBlend = 1f;

		// Token: 0x040071D6 RID: 29142
		private float _selfiePoseBlendDur;

		// Token: 0x040071D7 RID: 29143
		private CharacterRigController _rigController;

		// Token: 0x040071D8 RID: 29144
		private bool _selfieHandIKActive;

		// Token: 0x040071D9 RID: 29145
		private float _ikBlend;

		// Token: 0x040071DA RID: 29146
		private bool _ikBaseCaptured;

		// Token: 0x040071DB RID: 29147
		private Vector3 _ikBasePos;

		// Token: 0x040071DC RID: 29148
		private Quaternion _ikBaseRot;

		// Token: 0x040071DD RID: 29149
		private Transform _selfiePivotRef;

		// Token: 0x040071DE RID: 29150
		private float _headYaw;

		// Token: 0x040071DF RID: 29151
		private float _headPitch;

		// Token: 0x040071E0 RID: 29152
		private SelfieHeadAdjuster _headAdjuster;

		// Token: 0x040071E1 RID: 29153
		private Canvas _phoneCanvas;

		// Token: 0x040071E2 RID: 29154
		private PhoneTapSound _tapSoundPlayer;

		// Token: 0x040071E3 RID: 29155
		private bool _savedOverrideSorting;

		// Token: 0x040071E4 RID: 29156
		private int _savedSortingOrder;

		// Token: 0x040071E5 RID: 29157
		private bool _phoneCanvasSortingRaised;

		// Token: 0x040071E6 RID: 29158
		private bool _configuredUiHidden;

		// Token: 0x040071E7 RID: 29159
		private readonly List<GameObject> _hiddenUi = new List<GameObject>();

		// Token: 0x040071E8 RID: 29160
		private RapidActionsHUDController _tripodRapidActions;

		// Token: 0x040071E9 RID: 29161
		private GameplayEmoteController _gameplayEmoteController;

		// Token: 0x040071EA RID: 29162
		private MasturbationSpeedController _emoteSpeedController;

		// Token: 0x040071EB RID: 29163
		private bool _tripodMinimalHudActive;

		// Token: 0x040071EC RID: 29164
		private readonly List<SmartphoneController.TripodUiSnapshot> _tripodUiSnapshots = new List<SmartphoneController.TripodUiSnapshot>();

		// Token: 0x040071ED RID: 29165
		private readonly HashSet<GameObject> _tripodUiSnapshotTargets = new HashSet<GameObject>();

		// Token: 0x040071EE RID: 29166
		private PhoneCameraCapture _cameraCapture;

		// Token: 0x040071EF RID: 29167
		private bool _cameraCaptureConfigured;

		// Token: 0x040071F0 RID: 29168
		private PhoneSelfieNotifier _selfieNotifier;

		// Token: 0x040071F1 RID: 29169
		private bool _pendingSelfieActivate;

		// Token: 0x040071F2 RID: 29170
		private int _pendingSelfieActivateEarliestFrame;

		// Token: 0x040071F3 RID: 29171
		private bool _pendingSelfieDeactivate;

		// Token: 0x040071F4 RID: 29172
		private int _pendingSelfieDeactivateEarliestFrame;

		// Token: 0x040071F5 RID: 29173
		private int _nextPhoneInvariantCheckFrame;

		// Token: 0x040071F6 RID: 29174
		private SelfiePosePicker _posePicker;

		// Token: 0x040071F7 RID: 29175
		private bool _poseMenuOpen;

		// Token: 0x040071F8 RID: 29176
		[Header("Galleria - viewer foto ingrandita")]
		[Tooltip("Cliccando una foto in galleria, la mostra ingrandita e dritta (polaroid) davanti al player invece del piccolo viewer fullscreen del toolkit. Click ovunque per chiudere.")]
		public bool rotatePhotoForPhotoView = true;

		// Token: 0x040071F9 RID: 29177
		private PlayerEmotionController _playerEmotion;

		// Token: 0x040071FA RID: 29178
		private SelfieRadialMenuController _radialMenu;

		// Token: 0x040071FB RID: 29179
		private GameObject _runtimeSelfieOverlay;

		// Token: 0x040071FC RID: 29180
		[Tooltip("Quanto spingere AVANTI (lungo lo sguardo della camera) la fotocamera esterna nel fallback non-tarato, cosi' non inquadra il telefono da troppo vicino. In metri.")]
		public float rearCamForwardPush = 0.35f;

		// Token: 0x040071FD RID: 29181
		private const float TripodMaxPlayerDistance = 20f;

		// Token: 0x040071FE RID: 29182
		private bool _selfiePoseStarted;

		// Token: 0x040071FF RID: 29183
		private bool _jumpSuppressed;

		// Token: 0x04007200 RID: 29184
		private bool _savedJumpUseInput;

		// Token: 0x04007201 RID: 29185
		private bool _selfieMoveLocked;

		// Token: 0x04007202 RID: 29186
		private bool _selfieSavedLockMove;

		// Token: 0x04007203 RID: 29187
		private bool _photoViewerEnsured;

		// Token: 0x04007204 RID: 29188
		private float _dragVel;

		// Token: 0x04007205 RID: 29189
		private bool _photoViewerFrozen;

		// Token: 0x04007206 RID: 29190
		private bool _pv_savedLockCameraInput;

		// Token: 0x04007207 RID: 29191
		private DynamicIslandController _dynamicIsland;

		// Token: 0x04007208 RID: 29192
		private bool _dynamicIslandResolved;

		// Token: 0x04007209 RID: 29193
		private Transform _phonePanel;

		// Token: 0x0400720A RID: 29194
		private bool _phonePanelResolved;

		// Token: 0x0400720B RID: 29195
		private bool _nativeCameraBackGuarded;

		// Token: 0x02000E75 RID: 3701
		public enum PhoneState
		{
			// Token: 0x0400720D RID: 29197
			Gameplay,
			// Token: 0x0400720E RID: 29198
			PhoneHeld
		}

		// Token: 0x02000E76 RID: 3702
		private struct TripodUiSnapshot
		{
			// Token: 0x0400720F RID: 29199
			public GameObject target;

			// Token: 0x04007210 RID: 29200
			public bool activeSelf;
		}
	}
}
