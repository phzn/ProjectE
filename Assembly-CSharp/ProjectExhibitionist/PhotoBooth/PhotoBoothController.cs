using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Features.CensorBars;
using Features.Detection;
using Features.HideFlash;
using Features.MainMenu;
using Features.PlayerEmotes;
using Features.Rank;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.FirstPerson;
using ProjectExhibitionist.Interactions;
using RayznGames.VehicleSystem;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

namespace ProjectExhibitionist.PhotoBooth
{
	// Token: 0x0200096C RID: 2412
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(-100)]
	public sealed class PhotoBoothController : MonoBehaviour, IInteractable, IInteractionHandoffCandidate, IInteractionCandidate
	{
		// Token: 0x17000B37 RID: 2871
		// (get) Token: 0x06004207 RID: 16903 RVA: 0x00188D1B File Offset: 0x00186F1B
		public static bool AnySessionActive
		{
			get
			{
				return PhotoBoothController._activeSession != null && PhotoBoothController._activeSession._sessionActive;
			}
		}

		// Token: 0x06004208 RID: 16904 RVA: 0x00188D36 File Offset: 0x00186F36
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetSessionOwner()
		{
			PhotoBoothController._activeSession = null;
		}

		// Token: 0x17000B38 RID: 2872
		// (get) Token: 0x06004209 RID: 16905 RVA: 0x00188D3E File Offset: 0x00186F3E
		public InteractionConfigSO Config
		{
			get
			{
				return this.interactionConfig;
			}
		}

		// Token: 0x17000B39 RID: 2873
		// (get) Token: 0x0600420A RID: 16906 RVA: 0x00188D46 File Offset: 0x00186F46
		public bool IsBeingUsed
		{
			get
			{
				return this._sessionActive || this._isEjectingPrints;
			}
		}

		// Token: 0x17000B3A RID: 2874
		// (get) Token: 0x0600420B RID: 16907 RVA: 0x00188D58 File Offset: 0x00186F58
		public bool IsSessionActive
		{
			get
			{
				return this._sessionActive;
			}
		}

		// Token: 0x17000B3B RID: 2875
		// (get) Token: 0x0600420C RID: 16908 RVA: 0x00188D60 File Offset: 0x00186F60
		public bool HasResolvedPlayer
		{
			get
			{
				return this._player != null;
			}
		}

		// Token: 0x17000B3C RID: 2876
		// (get) Token: 0x0600420D RID: 16909 RVA: 0x00188D6E File Offset: 0x00186F6E
		public bool IsPlayerInRange
		{
			get
			{
				return this._playerInRange;
			}
		}

		// Token: 0x17000B3D RID: 2877
		// (get) Token: 0x0600420E RID: 16910 RVA: 0x00188D76 File Offset: 0x00186F76
		public bool CanPrepareInteraction
		{
			get
			{
				return this.CanAcquireSessionContext;
			}
		}

		// Token: 0x17000B3E RID: 2878
		// (get) Token: 0x0600420F RID: 16911 RVA: 0x000FCE88 File Offset: 0x000FB088
		public int CandidateInstanceId
		{
			get
			{
				return base.GetInstanceID();
			}
		}

		// Token: 0x17000B3F RID: 2879
		// (get) Token: 0x06004210 RID: 16912 RVA: 0x00188D7E File Offset: 0x00186F7E
		public float CandidateSqrDistance
		{
			get
			{
				return this._candidateSqrDistance;
			}
		}

		// Token: 0x17000B40 RID: 2880
		// (get) Token: 0x06004211 RID: 16913 RVA: 0x00188D88 File Offset: 0x00186F88
		public bool IsCandidateEligible
		{
			get
			{
				return !this._sessionActive && this._playerInRange && this._player != null && this.CanAcquireSessionContext && (this.GetNextAvailablePrint() != null || !this._isEjectingPrints);
			}
		}

		// Token: 0x17000B41 RID: 2881
		// (get) Token: 0x06004212 RID: 16914 RVA: 0x00188DD6 File Offset: 0x00186FD6
		public int SelectedSetting
		{
			get
			{
				return this._selectedSetting;
			}
		}

		// Token: 0x17000B42 RID: 2882
		// (get) Token: 0x06004213 RID: 16915 RVA: 0x00188DDE File Offset: 0x00186FDE
		public float LightIntensityMultiplier
		{
			get
			{
				return this._lightIntensityMultiplier;
			}
		}

		// Token: 0x17000B43 RID: 2883
		// (get) Token: 0x06004214 RID: 16916 RVA: 0x00188DE8 File Offset: 0x00186FE8
		public string IntensityLabel
		{
			get
			{
				return Mathf.RoundToInt(this._lightIntensityMultiplier * 100f).ToString() + "%";
			}
		}

		// Token: 0x17000B44 RID: 2884
		// (get) Token: 0x06004215 RID: 16917 RVA: 0x00188E18 File Offset: 0x00187018
		public string LightColorLabel
		{
			get
			{
				return PhotoBoothController.LightColorNames[this._lightColorIndex];
			}
		}

		// Token: 0x17000B45 RID: 2885
		// (get) Token: 0x06004216 RID: 16918 RVA: 0x00188E26 File Offset: 0x00187026
		public string BackdropLabel
		{
			get
			{
				return PhotoBoothController.Backdrops[this._backdropIndex].label;
			}
		}

		// Token: 0x17000B46 RID: 2886
		// (get) Token: 0x06004217 RID: 16919 RVA: 0x00188E3D File Offset: 0x0018703D
		public string EffectLabel
		{
			get
			{
				if (!(this.effectOverlay != null))
				{
					return "NONE";
				}
				return this.effectOverlay.CurrentEffectName;
			}
		}

		// Token: 0x17000B47 RID: 2887
		// (get) Token: 0x06004218 RID: 16920 RVA: 0x00188E5E File Offset: 0x0018705E
		public string FilterLabel
		{
			get
			{
				return PhotoBoothController.Filters[this._filterIndex].label;
			}
		}

		// Token: 0x17000B48 RID: 2888
		// (get) Token: 0x06004219 RID: 16921 RVA: 0x00188E75 File Offset: 0x00187075
		public string PoseLabel
		{
			get
			{
				if (this._poseIndex != 0)
				{
					return "POSE " + this._poseIndex.ToString("00");
				}
				return "NONE";
			}
		}

		// Token: 0x17000B49 RID: 2889
		// (get) Token: 0x0600421A RID: 16922 RVA: 0x00188E9F File Offset: 0x0018709F
		public string FaceLabel
		{
			get
			{
				return PhotoBoothController.Faces[this._faceIndex].label;
			}
		}

		// Token: 0x17000B4A RID: 2890
		// (get) Token: 0x0600421B RID: 16923 RVA: 0x00188EB6 File Offset: 0x001870B6
		public string LayoutLabel
		{
			get
			{
				return PhotoBoothPhotoLayout.GetName(this._layoutIndex);
			}
		}

		// Token: 0x17000B4B RID: 2891
		// (get) Token: 0x0600421C RID: 16924 RVA: 0x00188EC3 File Offset: 0x001870C3
		public float PhotoFieldOfView
		{
			get
			{
				if (!(this.photoCamera != null))
				{
					return this.cameraFieldOfView;
				}
				return this.photoCamera.fieldOfView;
			}
		}

		// Token: 0x17000B4C RID: 2892
		// (get) Token: 0x0600421D RID: 16925 RVA: 0x00188EE5 File Offset: 0x001870E5
		public float PhotoTargetFieldOfView
		{
			get
			{
				return this._photoCameraTargetFieldOfView;
			}
		}

		// Token: 0x17000B4D RID: 2893
		// (get) Token: 0x0600421E RID: 16926 RVA: 0x00188EED File Offset: 0x001870ED
		public bool IsSettingsPanelVisible
		{
			get
			{
				return this.settingsPanel != null && this.settingsPanel.SettingsVisible;
			}
		}

		// Token: 0x17000B4E RID: 2894
		// (get) Token: 0x0600421F RID: 16927 RVA: 0x00188F0A File Offset: 0x0018710A
		public bool IsPreviewPresenting
		{
			get
			{
				return this._filteredPreview != null && this._filteredPreview.IsPresenting;
			}
		}

		// Token: 0x17000B4F RID: 2895
		// (get) Token: 0x06004220 RID: 16928 RVA: 0x00188F27 File Offset: 0x00187127
		public float PhotoPitchOffset
		{
			get
			{
				return this._photoCameraPitchOffset;
			}
		}

		// Token: 0x17000B50 RID: 2896
		// (get) Token: 0x06004221 RID: 16929 RVA: 0x00188F2F File Offset: 0x0018712F
		public float ActivePostExposure
		{
			get
			{
				if (!(this._colorAdjustments != null))
				{
					return 0f;
				}
				return this._colorAdjustments.postExposure.value;
			}
		}

		// Token: 0x17000B51 RID: 2897
		// (get) Token: 0x06004222 RID: 16930 RVA: 0x00188F55 File Offset: 0x00187155
		public float ActiveSaturation
		{
			get
			{
				if (!(this._colorAdjustments != null))
				{
					return 0f;
				}
				return this._colorAdjustments.saturation.value;
			}
		}

		// Token: 0x17000B52 RID: 2898
		// (get) Token: 0x06004223 RID: 16931 RVA: 0x00188F7B File Offset: 0x0018717B
		public float FilterVolumeWeight
		{
			get
			{
				if (!(this._filterVolume != null))
				{
					return 0f;
				}
				return this._filterVolume.weight;
			}
		}

		// Token: 0x17000B53 RID: 2899
		// (get) Token: 0x06004224 RID: 16932 RVA: 0x00188F9C File Offset: 0x0018719C
		public int PendingPrintCount
		{
			get
			{
				return this._pendingPrintTextures.Count;
			}
		}

		// Token: 0x17000B54 RID: 2900
		// (get) Token: 0x06004225 RID: 16933 RVA: 0x00188FA9 File Offset: 0x001871A9
		public bool IsEjectingPrints
		{
			get
			{
				return this._isEjectingPrints;
			}
		}

		// Token: 0x17000B55 RID: 2901
		// (get) Token: 0x06004226 RID: 16934 RVA: 0x00188FB1 File Offset: 0x001871B1
		public int PrintedPhotoCount
		{
			get
			{
				return this._printedPhotoCount;
			}
		}

		// Token: 0x17000B56 RID: 2902
		// (get) Token: 0x06004227 RID: 16935 RVA: 0x00188FB9 File Offset: 0x001871B9
		public bool HasAvailablePrint
		{
			get
			{
				return this.GetNextAvailablePrint() != null;
			}
		}

		// Token: 0x17000B57 RID: 2903
		// (get) Token: 0x06004228 RID: 16936 RVA: 0x00188FC7 File Offset: 0x001871C7
		public bool IsPrintPickupPromptVisible
		{
			get
			{
				return this._promptVisible && (this._activePromptConfig == this.printInteractionConfig || this._activePromptConfig == this._combinedBoothPrintConfig);
			}
		}

		// Token: 0x17000B58 RID: 2904
		// (get) Token: 0x06004229 RID: 16937 RVA: 0x00188FF9 File Offset: 0x001871F9
		public float PrintPickupHoldProgress
		{
			get
			{
				return Mathf.Clamp01(this._printPickupHoldTime / 0.65f);
			}
		}

		// Token: 0x0600422A RID: 16938 RVA: 0x0018900C File Offset: 0x0018720C
		private void Awake()
		{
			this._gateReason = "PhotoBooth#" + base.GetInstanceID().ToString();
			this.NormalizeRuntimeSetupTransform();
			if (this.interactionAnchor == null)
			{
				this.interactionAnchor = base.transform;
			}
			this.PrepareInteractionPrompt();
			if (this.photoCamera != null)
			{
				this.photoCamera.enabled = false;
			}
			if (this.entranceGate != null)
			{
				Object.Destroy(this.entranceGate.gameObject);
				this.entranceGate = null;
			}
			Transform transform = ((this.runtimeSetupRoot != null) ? this.runtimeSetupRoot.Find("PhotoBooth_EntranceGate") : null);
			if (transform == null)
			{
				transform = base.transform.Find("PhotoBooth_RuntimeSetup/PhotoBooth_EntranceGate");
			}
			if (transform != null)
			{
				Object.Destroy(transform.gameObject);
			}
			if (this.mirrorSwitch == null)
			{
				this.mirrorSwitch = base.GetComponentInChildren<PhotoBoothMirrorSwitch>(true);
			}
			if (this.mirrorSwitch != null)
			{
				this.mirrorSwitch.ForceOff();
			}
			this.SetPhotoBackdropVisible(false);
			this._propertyBlock = new MaterialPropertyBlock();
			this.CacheLightDefaults();
			this.PreparePrintOutput();
			this.ApplyCameraFraming();
			this.CacheCameraControlDefaults();
			this.PrepareCaptureAudio();
			this.EnsureFilterRuntime();
			this._filteredPreview = base.GetComponent<PhotoBoothFilteredPreview>();
			if (this._filteredPreview == null)
			{
				this._filteredPreview = base.gameObject.AddComponent<PhotoBoothFilteredPreview>();
			}
			this._filteredPreview.Configure(this.photoCamera, (float)this.captureWidth / (float)this.captureHeight);
			if (this.settingsPanel == null)
			{
				this.settingsPanel = base.GetComponent<PhotoBoothPanel>();
			}
			if (this.settingsPanel == null)
			{
				this.settingsPanel = base.gameObject.AddComponent<PhotoBoothPanel>();
			}
			this.settingsPanel.Initialize(this);
			if (this.effectOverlay != null)
			{
				this.effectOverlay.Configure(this.photoCamera);
				this.effectOverlay.SetEffect(0);
			}
			this.ApplyAllVisualSettings();
		}

		// Token: 0x0600422B RID: 16939 RVA: 0x000FCE90 File Offset: 0x000FB090
		private void OnEnable()
		{
			InteractionCandidateRegistry.Register(this);
		}

		// Token: 0x0600422C RID: 16940 RVA: 0x00189217 File Offset: 0x00187417
		private void OnDisable()
		{
			InteractionCandidateRegistry.Unregister(this);
			this.ResetPrintPickupHold();
			this.InterruptForEncounter();
			this.SetPromptVisible(false);
			this.SetPhotoBackdropVisible(false);
		}

		// Token: 0x0600422D RID: 16941 RVA: 0x00189239 File Offset: 0x00187439
		private void Update()
		{
			this.ResolvePlayer();
			if (this._sessionActive)
			{
				this.MaintainPhotoBoothMovementReference();
				this.UpdateSessionInput();
				this.UpdatePhotoBoothPose();
				return;
			}
			this.UpdateInteraction();
		}

		// Token: 0x0600422E RID: 16942 RVA: 0x00189262 File Offset: 0x00187462
		private void LateUpdate()
		{
			if (this._sessionActive)
			{
				this.KeepPlayerInside();
			}
		}

		// Token: 0x0600422F RID: 16943 RVA: 0x00189274 File Offset: 0x00187474
		private void ResolvePlayer()
		{
			if (this._player != null)
			{
				return;
			}
			if (PlayerRegistry.Player != null)
			{
				this._player = PlayerRegistry.Player.GetComponentInChildren<vThirdPersonController>(true);
			}
			if (this._player == null)
			{
				this._player = Object.FindObjectOfType<vThirdPersonController>();
			}
			if (this._player != null)
			{
				this._playerBody = this._player.GetComponent<Rigidbody>();
				this.CachePlayerSessionComponents();
			}
		}

		// Token: 0x06004230 RID: 16944 RVA: 0x001892EC File Offset: 0x001874EC
		private void UpdateInteraction()
		{
			if (this._printPickupConsumed)
			{
				if (!this.IsInteractPhysicallyHeld())
				{
					this.ResetPrintPickupHold();
				}
				return;
			}
			if (this._player == null)
			{
				this._playerInRange = false;
				this._candidateSqrDistance = float.MaxValue;
				this.SetPromptVisible(false);
				this.ResetPrintPickupHold();
				return;
			}
			Vector3 vector = ((this.interactionAnchor != null) ? this.interactionAnchor.position : base.transform.position);
			this._candidateSqrDistance = Vector3.ProjectOnPlane(this._player.transform.position - vector, base.transform.up).sqrMagnitude;
			this._playerInRange = this._candidateSqrDistance <= this.interactionDistance * this.interactionDistance;
			bool flag = this.IsCandidateEligible && InteractionCandidateRegistry.IsWinner(this);
			PhotoBoothPrint nextAvailablePrint = this.GetNextAvailablePrint();
			if (!flag)
			{
				this.SetPromptVisible(false);
				this.ResetPrintPickupHold();
				return;
			}
			if (!(nextAvailablePrint != null))
			{
				this.ResetPrintPickupHold();
				this.SetPromptVisible(true, this.interactionConfig, false);
				if (this.IsInteractPressedThisFrame() && this.CanInteract(this._player.gameObject))
				{
					InteractionCandidateRegistry.MarkInteract();
					this.OnInteract(this._player.gameObject);
				}
				return;
			}
			if (!PlayerActivityGate.CanStartWorldInteraction)
			{
				this.SetPromptVisible(true, this.interactionConfig, false);
				if (this.IsInteractPressedThisFrame())
				{
					this.OnInteract(this._player.gameObject);
				}
				return;
			}
			this.SetPromptVisible(true, this.GetCombinedBoothPrintConfig(), this._printInteractionHeld);
			this.UpdatePrintPickup(nextAvailablePrint);
		}

		// Token: 0x06004231 RID: 16945 RVA: 0x00189478 File Offset: 0x00187678
		private void UpdatePrintPickup(PhotoBoothPrint print)
		{
			if (print == null || !print.IsAvailable)
			{
				this.ResetPrintPickupHold();
				return;
			}
			PhotoBoothController.PrintPickupAction printPickupAction = this.AdvancePrintPickup(this.IsInteractPressedThisFrame(), this.IsInteractHeld(), this.IsInteractReleasedThisFrame(), Time.deltaTime);
			if (this.promptUI != null)
			{
				this.promptUI.SetHoldProgress(this.PrintPickupHoldProgress, this._printInteractionHeld && !this._printPickupConsumed);
			}
			if (printPickupAction == PhotoBoothController.PrintPickupAction.Collect)
			{
				InteractionCandidateRegistry.MarkInteract();
				this.CollectAllAvailablePrints();
				this.SetPromptVisible(false);
				return;
			}
			if (printPickupAction == PhotoBoothController.PrintPickupAction.UseBooth && this.CanInteract(this._player.gameObject))
			{
				InteractionCandidateRegistry.MarkInteract();
				this.OnInteract(this._player.gameObject);
			}
		}

		// Token: 0x06004232 RID: 16946 RVA: 0x00189534 File Offset: 0x00187734
		internal PhotoBoothController.PrintPickupAction AdvancePrintPickup(bool pressed, bool held, bool released, float deltaTime)
		{
			if (this._printPickupConsumed)
			{
				if (!held)
				{
					this.ResetPrintPickupHold();
				}
				return PhotoBoothController.PrintPickupAction.None;
			}
			if (pressed && !this._printInteractionHeld)
			{
				this._printInteractionHeld = true;
				this._printPickupHoldTime = 0f;
				this._printPickupConsumed = false;
			}
			if (!this._printInteractionHeld)
			{
				return PhotoBoothController.PrintPickupAction.None;
			}
			if (held)
			{
				this._printPickupHoldTime += Mathf.Max(0f, deltaTime);
				if (this._printPickupHoldTime < 0.65f)
				{
					return PhotoBoothController.PrintPickupAction.None;
				}
				this._printPickupConsumed = true;
				return PhotoBoothController.PrintPickupAction.Collect;
			}
			else
			{
				if (!released)
				{
					return PhotoBoothController.PrintPickupAction.None;
				}
				this.ResetPrintPickupHold();
				return PhotoBoothController.PrintPickupAction.UseBooth;
			}
		}

		// Token: 0x06004233 RID: 16947 RVA: 0x001895C2 File Offset: 0x001877C2
		private void ResetPrintPickupHold()
		{
			this._printPickupHoldTime = 0f;
			this._printPickupConsumed = false;
			this._printInteractionHeld = false;
			if (this.promptUI != null)
			{
				this.promptUI.SetHoldProgress(0f, false);
			}
		}

		// Token: 0x06004234 RID: 16948 RVA: 0x001895FC File Offset: 0x001877FC
		private InteractionConfigSO GetCombinedBoothPrintConfig()
		{
			if (this._combinedBoothPrintConfig != null)
			{
				return this._combinedBoothPrintConfig;
			}
			InteractionConfigSO interactionConfigSO = ((this.interactionConfig != null) ? this.interactionConfig : this.printInteractionConfig);
			if (interactionConfigSO == null)
			{
				return null;
			}
			this._combinedBoothPrintConfig = Object.Instantiate<InteractionConfigSO>(interactionConfigSO);
			this._combinedBoothPrintConfig.name = "Photo Booth + Print (Runtime)";
			this._combinedBoothPrintConfig.hideFlags = HideFlags.DontSave;
			this._combinedBoothPrintConfig.mainActionName = "USE PHOTOBOOTH";
			this._combinedBoothPrintConfig.mainKeyDisplayLabel = "E";
			this._combinedBoothPrintConfig.secondaryActionName = "(HOLD) TAKE PHOTOS";
			this._combinedBoothPrintConfig.secondaryKeyDisplayLabel = "E";
			return this._combinedBoothPrintConfig;
		}

		// Token: 0x06004235 RID: 16949 RVA: 0x001896B5 File Offset: 0x001878B5
		private bool IsInteractHeld()
		{
			return !TextInputFocus.BlocksGameplayInput && (GameplayInput.GetKey(KeyCode.E) || this.interactInput.GetButton());
		}

		// Token: 0x06004236 RID: 16950 RVA: 0x001896D8 File Offset: 0x001878D8
		private bool IsInteractPhysicallyHeld()
		{
			if (Input.GetKey(KeyCode.E) || ControllerInput.GetButton(ControllerButton.West))
			{
				return true;
			}
			if (this.interactInput == null)
			{
				return false;
			}
			KeyCode keyCode;
			bool flag = ControllerGameplayBridge.TryKey(this.interactInput.keyboard, out keyCode);
			if (flag && Input.GetKey(keyCode))
			{
				return true;
			}
			if (!flag && !string.IsNullOrEmpty(this.interactInput.keyboard))
			{
				try
				{
					if (Input.GetButton(this.interactInput.keyboard))
					{
						return true;
					}
				}
				catch (ArgumentException)
				{
				}
				catch (UnityException)
				{
				}
			}
			if (this.interactInput.preserveJoystickBinding || !flag || !ControllerBindings.HasKey(keyCode))
			{
				ControllerButton controllerButton;
				return ControllerBindings.TryLegacyButton(this.interactInput.joystick, out controllerButton) && ControllerInput.GetButton(controllerButton);
			}
			ControllerButton controllerButton2;
			bool flag2;
			if (ControllerBindings.TryButton(keyCode, out controllerButton2, out flag2))
			{
				return ControllerInput.GetButton(controllerButton2);
			}
			if (keyCode == KeyCode.Return)
			{
				return ControllerInput.GetButton(ControllerButton.South);
			}
			if (keyCode == KeyCode.Escape)
			{
				return ControllerInput.GetButton(ControllerButton.East) || ControllerInput.GetButton(ControllerButton.Start);
			}
			return ControllerInput.GetKey(keyCode);
		}

		// Token: 0x06004237 RID: 16951 RVA: 0x001897E8 File Offset: 0x001879E8
		private bool IsInteractPressedThisFrame()
		{
			return !TextInputFocus.BlocksGameplayInput && (GameplayInput.GetKeyDown(KeyCode.E) || this.interactInput.GetButtonDown());
		}

		// Token: 0x06004238 RID: 16952 RVA: 0x00189809 File Offset: 0x00187A09
		private bool IsInteractReleasedThisFrame()
		{
			return !TextInputFocus.BlocksGameplayInput && (GameplayInput.GetKeyUp(KeyCode.E) || this.interactInput.GetButtonUp());
		}

		// Token: 0x06004239 RID: 16953 RVA: 0x0018982C File Offset: 0x00187A2C
		private void UpdateSessionInput()
		{
			if (ControllerInput.IsActive)
			{
				if (ControllerWorldPointer.Down(ControllerButton.North) && this.settingsPanel != null)
				{
					this.settingsPanel.ToggleSettingsVisibility();
				}
				if (ControllerWorldPointer.Down(ControllerButton.DpadUp))
				{
					this.SelectSetting(this._selectedSetting - 1);
				}
				if (ControllerWorldPointer.Down(ControllerButton.DpadDown))
				{
					this.SelectSetting(this._selectedSetting + 1);
				}
				if (ControllerWorldPointer.Down(ControllerButton.DpadLeft))
				{
					this.AdjustSelectedSetting(-1);
				}
				if (ControllerWorldPointer.Down(ControllerButton.DpadRight))
				{
					this.AdjustSelectedSetting(1);
				}
				this.UpdatePhotoCameraControls();
				if (ControllerWorldPointer.Down(ControllerButton.South))
				{
					this.CapturePhoto();
				}
				if (ControllerWorldPointer.Down(ControllerButton.East))
				{
					this.EndSession();
				}
				return;
			}
			if (GameplayInput.GetKeyDown(KeyCode.Tab) && this.settingsPanel != null)
			{
				this.settingsPanel.ToggleSettingsVisibility();
			}
			if (GameplayInput.GetKeyDown(KeyCode.UpArrow))
			{
				this.SelectSetting(this._selectedSetting - 1);
			}
			if (GameplayInput.GetKeyDown(KeyCode.DownArrow))
			{
				this.SelectSetting(this._selectedSetting + 1);
			}
			if (GameplayInput.GetKeyDown(KeyCode.LeftArrow))
			{
				this.AdjustSelectedSetting(-1);
			}
			if (GameplayInput.GetKeyDown(KeyCode.RightArrow))
			{
				this.AdjustSelectedSetting(1);
			}
			this.UpdatePhotoCameraControls();
			if (GameplayInput.GetKeyDown(KeyCode.Space))
			{
				this.CapturePhoto();
			}
			if (this.IsInteractPressedThisFrame())
			{
				this.EndSession();
			}
		}

		// Token: 0x0600423A RID: 16954 RVA: 0x00189974 File Offset: 0x00187B74
		public bool CanInteract(GameObject player)
		{
			bool flag = !this._isEjectingPrints || this.GetNextAvailablePrint() != null;
			return !this._sessionActive && flag && player != null && this.photoCamera != null && this.CanAcquireSessionContext;
		}

		// Token: 0x17000B59 RID: 2905
		// (get) Token: 0x0600423B RID: 16955 RVA: 0x001899C8 File Offset: 0x00187BC8
		private bool CanAcquireSessionContext
		{
			get
			{
				if (!TextInputFocus.BlocksGameplayInput && this._player != null && !this._player.ragdolled && !this._player.isJumping && !this._player.inJumpStarted && !this._player.isRolling && !this._player.isSliding)
				{
					CielRagdollRecovery component = this._player.GetComponent<CielRagdollRecovery>();
					if (component == null || !component.IsRecovering)
					{
						return PlayerActivityGate.CanAcquireAfterReleasing(PlayerActivity.Interact, PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation | PlayerContextResource.ModalUI | PlayerContextResource.HeldItem, new Predicate<Object>(this.CanInterruptSessionOwner));
					}
				}
				return false;
			}
		}

		// Token: 0x0600423C RID: 16956 RVA: 0x00189A5C File Offset: 0x00187C5C
		private bool CanInterruptSessionOwner(Object owner)
		{
			if (this._player == null)
			{
				return false;
			}
			GameplayEmoteController gameplayEmoteController = owner as GameplayEmoteController;
			if (gameplayEmoteController != null)
			{
				return gameplayEmoteController.transform == this._player.transform || gameplayEmoteController.transform.IsChildOf(this._player.transform);
			}
			HideFlashController hideFlashController = owner as HideFlashController;
			if (hideFlashController != null)
			{
				return hideFlashController.transform == this._player.transform || hideFlashController.transform.IsChildOf(this._player.transform);
			}
			vVehicleInteractable vVehicleInteractable = owner as vVehicleInteractable;
			if (vVehicleInteractable != null)
			{
				return vVehicleInteractable.CurrentRider == this._player;
			}
			vBikeInteractable vBikeInteractable = owner as vBikeInteractable;
			if (vBikeInteractable != null)
			{
				return vBikeInteractable.CurrentRider == this._player;
			}
			SkateboardInteractable skateboardInteractable = owner as SkateboardInteractable;
			if (skateboardInteractable != null)
			{
				return skateboardInteractable.CurrentRider == this._player;
			}
			SimpleRollerSkatesInteractable simpleRollerSkatesInteractable = owner as SimpleRollerSkatesInteractable;
			return simpleRollerSkatesInteractable != null && simpleRollerSkatesInteractable.CurrentRider == this._player;
		}

		// Token: 0x0600423D RID: 16957 RVA: 0x00189B68 File Offset: 0x00187D68
		private bool PreparePlayerForSession()
		{
			foreach (vVehicleController vVehicleController in Object.FindObjectsOfType<vVehicleController>())
			{
				if (!(vVehicleController.CurrentRider != this._player) && (!vVehicleController.Dismount(true) || !this.ParkVehicleOutside(vVehicleController.transform)))
				{
					return false;
				}
			}
			foreach (vBikeController vBikeController in Object.FindObjectsOfType<vBikeController>())
			{
				if (!(vBikeController.CurrentRider != this._player))
				{
					vBikeController.ForceDismount();
					if (vBikeController.IsBeingRidden || !this.ParkVehicleOutside(vBikeController.transform))
					{
						return false;
					}
				}
			}
			foreach (SkateboardInteractable skateboardInteractable in Object.FindObjectsOfType<SkateboardInteractable>())
			{
				if (!(skateboardInteractable.CurrentRider != this._player))
				{
					skateboardInteractable.ForceCrashDismount();
					if (skateboardInteractable.IsBeingWorn || !this.ParkVehicleOutside(skateboardInteractable.transform))
					{
						return false;
					}
				}
			}
			foreach (SimpleRollerSkatesInteractable simpleRollerSkatesInteractable in Object.FindObjectsOfType<SimpleRollerSkatesInteractable>())
			{
				if (!(simpleRollerSkatesInteractable.CurrentRider != this._player))
				{
					simpleRollerSkatesInteractable.ForceCrashDismount();
					if (simpleRollerSkatesInteractable.IsBeingWorn || !this.ParkVehicleOutside(simpleRollerSkatesInteractable.transform))
					{
						return false;
					}
				}
			}
			GameplayEmoteController[] componentsInChildren = this._player.GetComponentsInChildren<GameplayEmoteController>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].ForceEndEmoteImmediate();
			}
			HideFlashController[] componentsInChildren2 = this._player.GetComponentsInChildren<HideFlashController>(true);
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				componentsInChildren2[i].InterruptForWorldInteraction();
			}
			CoverPoseController componentInChildren = this._player.GetComponentInChildren<CoverPoseController>(true);
			if (componentInChildren != null && componentInChildren.IsCovering)
			{
				componentInChildren.DeactivateCoverPose();
			}
			PlayerActionAnimator componentInChildren2 = this._player.GetComponentInChildren<PlayerActionAnimator>(true);
			if (componentInChildren2 != null && componentInChildren2.GenericAction != null && componentInChildren2.GenericAction.doingAction)
			{
				componentInChildren2.CancelCurrentAction();
			}
			this._player.isCrouching = false;
			this._player.input = Vector3.zero;
			this._player.inputSmooth = Vector3.zero;
			this._player.moveDirection = Vector3.zero;
			this.CrossFadeToLocomotion(0f);
			return true;
		}

		// Token: 0x0600423E RID: 16958 RVA: 0x00189DA0 File Offset: 0x00187FA0
		private bool ParkVehicleOutside(Transform vehicle)
		{
			if (vehicle == null)
			{
				return false;
			}
			Bounds bounds = new Bounds(vehicle.position + Vector3.up * 0.5f, Vector3.one * 0.5f);
			foreach (Collider collider in vehicle.GetComponentsInChildren<Collider>())
			{
				if (collider.enabled && !collider.isTrigger && !(collider is WheelCollider))
				{
					bounds.Encapsulate(collider.bounds);
				}
			}
			foreach (Renderer renderer in vehicle.GetComponentsInChildren<Renderer>())
			{
				if (renderer.enabled && (renderer is MeshRenderer || renderer is SkinnedMeshRenderer))
				{
					bounds.Encapsulate(renderer.bounds);
				}
			}
			Vector3 extents = bounds.extents;
			Vector3 vector = bounds.center - vehicle.position;
			Vector3 vector2 = ((this.interiorBounds != null) ? this.interiorBounds.bounds.center : base.transform.position);
			PhotoBoothCurtainPhysics componentInChildren = base.GetComponentInChildren<PhotoBoothCurtainPhysics>(true);
			Vector3 vector3 = ((componentInChildren != null) ? Vector3.ProjectOnPlane(componentInChildren.transform.position - vector2, Vector3.up) : base.transform.forward);
			if (vector3.sqrMagnitude < 0.01f)
			{
				vector3 = base.transform.forward;
			}
			vector3 = Vector3.ProjectOnPlane(vector3, Vector3.up).normalized;
			Vector3 vector4 = Vector3.Cross(Vector3.up, vector3);
			float num = ((this.interiorBounds != null) ? this.interiorBounds.bounds.extents.magnitude : 2f);
			float num2 = Mathf.Max(extents.x, extents.z);
			float num3 = ((this.playerStartMarker != null) ? this.playerStartMarker.position.y : this._player.transform.position.y);
			for (int j = 0; j < 5; j++)
			{
				for (int k = 0; k < 5; k++)
				{
					int num4 = ((k == 0) ? 0 : ((k + 1) / 2 * ((k % 2 == 0) ? (-1) : 1)));
					Vector3 vector5 = vector2 + vector3 * (num + num2 + 0.6f + (float)j) + vector4 * ((float)num4 * (num2 * 2f + 0.3f));
					vector5.y = num3 + 2f;
					RaycastHit raycastHit;
					if (Physics.Raycast(vector5, Vector3.down, out raycastHit, 4f, -1, QueryTriggerInteraction.Ignore) && raycastHit.normal.y >= 0.65f && !raycastHit.collider.transform.IsChildOf(vehicle) && !raycastHit.collider.transform.IsChildOf(this._player.transform))
					{
						Vector3 vector6 = new Vector3(vector5.x, raycastHit.point.y + extents.y + 0.05f, vector5.z);
						if (this.HasParkingFootprintSupport(vehicle, vector6, extents, raycastHit.point.y))
						{
							bool flag = false;
							foreach (Collider collider2 in Physics.OverlapBox(vector6, extents, Quaternion.identity, -1, QueryTriggerInteraction.Ignore))
							{
								if (!collider2.transform.IsChildOf(vehicle) && !collider2.transform.IsChildOf(this._player.transform))
								{
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								Vector3 vector7 = vector6 - vector;
								vehicle.position = vector7;
								Rigidbody component = vehicle.GetComponent<Rigidbody>();
								if (component != null)
								{
									if (!component.isKinematic)
									{
										component.velocity = Vector3.zero;
										component.angularVelocity = Vector3.zero;
									}
									component.position = vector7;
								}
								Physics.SyncTransforms();
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600423F RID: 16959 RVA: 0x0018A1BC File Offset: 0x001883BC
		private bool HasParkingFootprintSupport(Transform vehicle, Vector3 center, Vector3 extents, float floorY)
		{
			for (int i = -1; i <= 1; i += 2)
			{
				for (int j = -1; j <= 1; j += 2)
				{
					RaycastHit raycastHit;
					if (!Physics.Raycast(new Vector3(center.x + (float)i * extents.x * 0.85f, floorY + 1f, center.z + (float)j * extents.z * 0.85f), Vector3.down, out raycastHit, 1.3f, -1, QueryTriggerInteraction.Ignore) || raycastHit.normal.y < 0.65f || Mathf.Abs(raycastHit.point.y - floorY) > 0.25f || raycastHit.collider.transform.IsChildOf(vehicle) || raycastHit.collider.transform.IsChildOf(this._player.transform))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06004240 RID: 16960 RVA: 0x0018A29E File Offset: 0x0018849E
		private bool TryAcquireSessionContext()
		{
			if (this._sessionLease != null)
			{
				if (this._sessionLease.IsActive)
				{
					return true;
				}
				this._sessionLease = null;
			}
			return PlayerActivityGate.TryAcquire(this._gateReason, this, PlayerActivity.Interact, PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation | PlayerContextResource.ModalUI | PlayerContextResource.HeldItem, PlayerActivity.AllPlayerActions, out this._sessionLease, false, null);
		}

		// Token: 0x06004241 RID: 16961 RVA: 0x0018A2DB File Offset: 0x001884DB
		private void ReleaseSessionContext()
		{
			PlayerActivityLease sessionLease = this._sessionLease;
			this._sessionLease = null;
			if (sessionLease == null)
			{
				return;
			}
			sessionLease.Dispose();
		}

		// Token: 0x06004242 RID: 16962 RVA: 0x0018A2F4 File Offset: 0x001884F4
		public void OnInteract(GameObject player)
		{
			if (!this.CanInteract(player))
			{
				return;
			}
			this._player = player.GetComponent<vThirdPersonController>();
			if (this._player == null)
			{
				this._player = player.GetComponentInChildren<vThirdPersonController>(true);
			}
			if (this._player == null)
			{
				this._player = player.GetComponentInParent<vThirdPersonController>();
			}
			if (this._player == null)
			{
				return;
			}
			this._playerBody = this._player.GetComponent<Rigidbody>();
			this.CachePlayerSessionComponents();
			this.StartSession();
		}

		// Token: 0x06004243 RID: 16963 RVA: 0x00004F31 File Offset: 0x00003131
		public void OnActionExecuted(string actionName, GameObject player)
		{
		}

		// Token: 0x06004244 RID: 16964 RVA: 0x0018A378 File Offset: 0x00188578
		private void StartSession()
		{
			if (this._sessionActive || this.photoCamera == null)
			{
				return;
			}
			this.NormalizeRuntimeSetupTransform();
			if (!this.CanAcquireSessionContext || !this.PreparePlayerForSession() || !this.TryAcquireSessionContext())
			{
				BlockedActionFeedback.Show("Stop what you're doing first");
				return;
			}
			this.NormalizeRuntimeSetupTransform();
			this._sessionActive = true;
			PhotoBoothController._activeSession = this;
			InteractionCandidateRegistry.MarkInteract();
			this.SetPromptVisible(false);
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (instance != null)
			{
				instance.Hide();
			}
			this.PositionPlayerForPhoto();
			this.KeepPlayerInside();
			this.ResetPhotoCameraControls();
			this.BeginPhotoBoothMovement();
			this.BeginPhotoBoothFace();
			this.BeginPhotoBoothPose();
			this._disabledCameras.Clear();
			foreach (Camera camera in Object.FindObjectsOfType<Camera>(true))
			{
				if (!(camera == null) && !(camera == this.photoCamera) && camera.enabled && camera.gameObject.activeInHierarchy && !(camera.targetTexture != null))
				{
					this._disabledCameras.Add(camera);
					camera.enabled = false;
				}
			}
			this.SuppressGameplayHUD();
			this.SetPhotoBackdropVisible(true);
			if (this._filteredPreview != null)
			{
				this._filteredPreview.Begin();
			}
			this.photoCamera.enabled = true;
			if (this._photoCameraData != null)
			{
				this._photoCameraData.renderPostProcessing = false;
			}
			this.ApplyFilter();
			if (this.effectOverlay != null)
			{
				this.effectOverlay.SetEffect(this._effectIndex);
			}
			this.settingsPanel.Show();
			if (this.debugLogs)
			{
				Debug.Log("[PhotoBooth] Session started.", this);
			}
		}

		// Token: 0x06004245 RID: 16965 RVA: 0x0018A518 File Offset: 0x00188718
		public void InterruptForEncounter()
		{
			this.CancelPendingCapture();
			this.EndSession();
			this.SetPromptVisible(false);
		}

		// Token: 0x06004246 RID: 16966 RVA: 0x0018A52D File Offset: 0x0018872D
		private void CancelPendingCapture()
		{
			if (!this._captureInProgress)
			{
				return;
			}
			this._captureGeneration++;
			this._captureInProgress = false;
			if (this.settingsPanel != null)
			{
				this.settingsPanel.SetCaptureVisibility(true);
			}
		}

		// Token: 0x06004247 RID: 16967 RVA: 0x0018A568 File Offset: 0x00188768
		public void EndSession()
		{
			if (PhotoBoothController._activeSession == this)
			{
				PhotoBoothController._activeSession = null;
			}
			if (!this._sessionActive && !this._movementReferenceOverridden && this._disabledCameras.Count == 0 && this._suppressedGameplayCanvases.Count == 0)
			{
				if (this._photoBoothActionActive || this._emotionLayersSuppressed)
				{
					this.EndPhotoBoothPose();
				}
				this.EndPhotoBoothFace();
				this.ReleaseSessionContext();
				return;
			}
			this._sessionActive = false;
			if (this.entranceGate != null)
			{
				this.entranceGate.enabled = false;
			}
			if (this.mirrorSwitch != null)
			{
				this.mirrorSwitch.ForceOff();
			}
			if (this.photoCamera != null)
			{
				this.photoCamera.enabled = false;
			}
			if (this._filteredPreview != null)
			{
				this._filteredPreview.End();
			}
			this.SetPhotoBackdropVisible(false);
			foreach (Camera camera in this._disabledCameras)
			{
				if (camera != null)
				{
					camera.enabled = true;
				}
			}
			this._disabledCameras.Clear();
			if (this._filterVolume != null)
			{
				this._filterVolume.weight = 0f;
			}
			if (this._photoCameraData != null)
			{
				this._photoCameraData.renderPostProcessing = this._savedPostProcessing;
			}
			if (this.effectOverlay != null)
			{
				this.effectOverlay.SetEffect(0);
			}
			this.EndPhotoBoothPose();
			this.EndPhotoBoothFace();
			this.EndPhotoBoothMovement();
			if (this.settingsPanel != null)
			{
				this.settingsPanel.Hide();
			}
			this.RestoreGameplayHUD();
			this.ReleaseSessionContext();
			this.BeginPrintEjectionIfReady();
			if (this.debugLogs)
			{
				Debug.Log("[PhotoBooth] Session ended.", this);
			}
		}

		// Token: 0x06004248 RID: 16968 RVA: 0x0018A74C File Offset: 0x0018894C
		public void SelectSetting(int index)
		{
			this._selectedSetting = (index % 8 + 8) % 8;
			if (this.settingsPanel != null)
			{
				this.settingsPanel.Refresh();
			}
		}

		// Token: 0x06004249 RID: 16969 RVA: 0x0018A774 File Offset: 0x00188974
		public void AdjustSelectedSetting(int direction)
		{
			direction = ((direction < 0) ? (-1) : 1);
			switch (this._selectedSetting)
			{
			case 0:
				this._lightIntensityMultiplier = Mathf.Clamp(Mathf.Round((this._lightIntensityMultiplier + (float)direction * 0.25f) * 4f) / 4f, 0.25f, 2f);
				this.ApplyLights();
				this.ApplyFilter();
				break;
			case 1:
				this._lightColorIndex = PhotoBoothController.Wrap(this._lightColorIndex + direction, PhotoBoothController.LightColors.Length);
				this.ApplyLights();
				break;
			case 2:
				this._backdropIndex = PhotoBoothController.Wrap(this._backdropIndex + direction, PhotoBoothController.Backdrops.Length);
				this.ApplyBackdrop();
				break;
			case 3:
				if (this.effectOverlay != null)
				{
					this._effectIndex = PhotoBoothController.Wrap(this._effectIndex + direction, this.effectOverlay.EffectCount);
					this.effectOverlay.SetEffect(this._effectIndex);
				}
				break;
			case 4:
				this._filterIndex = PhotoBoothController.Wrap(this._filterIndex + direction, PhotoBoothController.Filters.Length);
				this.ApplyFilter();
				break;
			case 5:
				this._poseIndex = PhotoBoothController.Wrap(this._poseIndex + direction, 27);
				this.ApplyPhotoBoothPose();
				break;
			case 6:
				this._faceIndex = PhotoBoothController.Wrap(this._faceIndex + direction, PhotoBoothController.Faces.Length);
				this.ApplyPhotoBoothFace();
				break;
			case 7:
				this._layoutIndex = PhotoBoothController.Wrap(this._layoutIndex + direction, PhotoBoothPhotoLayout.Count);
				if (this._filteredPreview != null)
				{
					this._filteredPreview.ApplyLayout(this._layoutIndex);
				}
				break;
			}
			if (this.settingsPanel != null)
			{
				this.settingsPanel.Refresh();
			}
		}

		// Token: 0x0600424A RID: 16970 RVA: 0x0018A944 File Offset: 0x00188B44
		public void CapturePhoto()
		{
			if (!this._sessionActive || this._captureInProgress || this.photoCamera == null)
			{
				return;
			}
			if (this._captureAudioSource != null && this._flashAudioClip != null)
			{
				this._captureAudioSource.PlayOneShot(this._flashAudioClip, 0.92f);
			}
			base.StartCoroutine(this.CaptureAtEndOfFrame());
		}

		// Token: 0x0600424B RID: 16971 RVA: 0x0018A9AF File Offset: 0x00188BAF
		private IEnumerator CaptureAtEndOfFrame()
		{
			int generation = this._captureGeneration;
			this._captureInProgress = true;
			if (this.settingsPanel != null)
			{
				this.settingsPanel.SetCaptureVisibility(false);
			}
			yield return new WaitForEndOfFrame();
			if (generation != this._captureGeneration)
			{
				yield break;
			}
			if (this.photoCamera == null)
			{
				this.CancelPendingCapture();
				yield break;
			}
			RenderTexture renderTexture = null;
			RenderTexture renderTexture2 = null;
			Texture2D texture2D = null;
			Texture2D texture2D2 = null;
			Texture2D texture2D3 = null;
			RenderTexture active = RenderTexture.active;
			RenderTexture targetTexture = this.photoCamera.targetTexture;
			string text = null;
			try
			{
				renderTexture = RenderTexture.GetTemporary(this.captureWidth, this.captureHeight, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
				this.RenderPhotoCamera(renderTexture);
				renderTexture2 = RenderTexture.GetTemporary(this.captureWidth, this.captureHeight, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
				if (this._filteredPreview != null)
				{
					this._filteredPreview.Process(renderTexture, renderTexture2);
				}
				else
				{
					Graphics.Blit(renderTexture, renderTexture2);
				}
				RenderTexture.active = renderTexture2;
				texture2D = new Texture2D(this.captureWidth, this.captureHeight, TextureFormat.RGB24, false, false);
				texture2D.ReadPixels(new Rect(0f, 0f, (float)this.captureWidth, (float)this.captureHeight), 0, 0, false);
				texture2D.Apply(false, false);
				PhotoBoothPhotoLayout.ApplyToTexture(texture2D, this._layoutIndex);
				string text2 = "PHOTOBOOTH_" + DateTime.Now.ToString("yyyyMMdd_HHmmss_fff") + ".png";
				text = PlayerMediaExport.WritePhotoBooth(texture2D.EncodeToPNG(), text2);
				texture2D.name = text2;
				texture2D2 = texture2D;
				texture2D3 = PhotoBoothController.RotatePortrait180(texture2D);
				texture2D3.name = Path.GetFileNameWithoutExtension(text2) + "_PRINT";
				texture2D = null;
			}
			catch (Exception ex)
			{
				Debug.LogError("[PhotoBooth] Capture failed: " + ex.Message, this);
			}
			finally
			{
				this.photoCamera.targetTexture = targetTexture;
				RenderTexture.active = active;
				if (renderTexture != null)
				{
					RenderTexture.ReleaseTemporary(renderTexture);
				}
				if (renderTexture2 != null)
				{
					RenderTexture.ReleaseTemporary(renderTexture2);
				}
				if (texture2D != null)
				{
					Object.Destroy(texture2D);
				}
				if (this.settingsPanel != null)
				{
					this.settingsPanel.SetCaptureVisibility(true);
				}
				this._captureInProgress = false;
			}
			if (!string.IsNullOrEmpty(text) && this.settingsPanel != null)
			{
				this.settingsPanel.ShowCaptureFeedback(Path.GetFileName(text));
			}
			if (texture2D2 != null && texture2D3 != null)
			{
				this.QueuePrint(new PhotoBoothController.CapturedPhoto(texture2D2, texture2D3, PhotoBoothPhotoArchive.ArchiveIdFromPath(text)));
				PhotoBoothPhotoArchive.NotifyChanged();
				if (!this._sessionActive)
				{
					this.BeginPrintEjectionIfReady();
				}
			}
			else
			{
				if (texture2D2 != null)
				{
					Object.Destroy(texture2D2);
				}
				if (texture2D3 != null && texture2D3 != texture2D2)
				{
					Object.Destroy(texture2D3);
				}
			}
			if (this.debugLogs && !string.IsNullOrEmpty(text))
			{
				Debug.Log("[PhotoBooth] Photo saved to " + text, this);
			}
			yield break;
		}

		// Token: 0x0600424C RID: 16972 RVA: 0x0018A9C0 File Offset: 0x00188BC0
		private void RenderPhotoCamera(RenderTexture destination)
		{
			RenderTexture targetTexture = this.photoCamera.targetTexture;
			using (StreamerCensorRenderFeature.BeginCapture())
			{
				try
				{
					this.photoCamera.targetTexture = destination;
					UniversalRenderPipeline.SingleCameraRequest singleCameraRequest = new UniversalRenderPipeline.SingleCameraRequest();
					if (RenderPipeline.SupportsRenderRequest<UniversalRenderPipeline.SingleCameraRequest>(this.photoCamera, singleCameraRequest))
					{
						singleCameraRequest.destination = destination;
						RenderPipeline.SubmitRenderRequest<UniversalRenderPipeline.SingleCameraRequest>(this.photoCamera, singleCameraRequest);
					}
					else
					{
						this.photoCamera.Render();
					}
				}
				finally
				{
					this.photoCamera.targetTexture = targetTexture;
				}
			}
		}

		// Token: 0x0600424D RID: 16973 RVA: 0x0018AA54 File Offset: 0x00188C54
		public void QueuePrintForDiagnostics(Texture2D texture)
		{
			if (texture == null)
			{
				return;
			}
			Texture2D texture2D = ((texture.height > texture.width) ? texture : PhotoBoothController.RotateLandscapeCounterClockwise(texture));
			PhotoBoothController.CapturedPhoto capturedPhoto = new PhotoBoothController.CapturedPhoto(texture2D, PhotoBoothController.RotatePortrait180(texture2D), null);
			this.QueuePrint(capturedPhoto);
			if (!this._sessionActive)
			{
				this.BeginPrintEjectionIfReady();
			}
		}

		// Token: 0x0600424E RID: 16974 RVA: 0x0018AAA4 File Offset: 0x00188CA4
		private void PreparePrintOutput()
		{
			if (this.photoPrintTemplate == null)
			{
				return;
			}
			Renderer component = this.photoPrintTemplate.GetComponent<Renderer>();
			this.EnsurePortraitCaptureDimensions(component);
			this._printEjectDirection = -base.transform.right.normalized;
			this.photoPrintTemplate.gameObject.SetActive(false);
			this._printerAudioSource = base.GetComponent<AudioSource>();
			if (this._printerAudioSource == null)
			{
				this._printerAudioSource = base.gameObject.AddComponent<AudioSource>();
			}
			this._printerAudioSource.playOnAwake = false;
			this._printerAudioSource.loop = false;
			this._printerAudioSource.spatialBlend = 1f;
			this._printerAudioSource.minDistance = 0.6f;
			this._printerAudioSource.maxDistance = 8f;
			this._printerAudioSource.volume = 0.72f;
			this._printerAudioClip = PhotoBoothController.CreatePrinterAudioClip();
		}

		// Token: 0x0600424F RID: 16975 RVA: 0x0018AB90 File Offset: 0x00188D90
		private void ApplyCameraFraming()
		{
			if (this.photoCamera == null)
			{
				return;
			}
			this.photoCamera.fieldOfView = this.cameraFieldOfView;
			this.photoCamera.nearClipPlane = this.cameraNearClipPlane;
			this.photoCamera.farClipPlane = this.cameraFarClipPlane;
			if (!this.autoAimPhotoCamera)
			{
				return;
			}
			if (this.photoTarget == null)
			{
				foreach (Transform transform in base.GetComponentsInChildren<Transform>(true))
				{
					if (!(transform.name != "PhotoBooth_PhotoTarget"))
					{
						this.photoTarget = transform;
						break;
					}
				}
			}
			if (this.photoTarget == null)
			{
				return;
			}
			Vector3 vector = this.photoTarget.position + base.transform.up * this.cameraAimHeightOffset - this.photoCamera.transform.position;
			if (vector.sqrMagnitude > 0.0001f)
			{
				this.photoCamera.transform.rotation = Quaternion.LookRotation(vector, base.transform.up);
			}
		}

		// Token: 0x06004250 RID: 16976 RVA: 0x0018ACA8 File Offset: 0x00188EA8
		private void NormalizeRuntimeSetupTransform()
		{
			if (this.runtimeSetupRoot == null)
			{
				Transform transform = base.transform.Find("PhotoBooth_RuntimeSetup");
				this.runtimeSetupRoot = ((transform != null) ? transform : ((this.photoCamera != null) ? this.photoCamera.transform.parent : null));
			}
			if (this.runtimeSetupRoot == null)
			{
				return;
			}
			this.runtimeSetupRoot.localPosition = Vector3.zero;
			this.runtimeSetupRoot.localRotation = Quaternion.identity;
			Vector3 lossyScale = this.runtimeSetupRoot.lossyScale;
			Vector3 localScale = this.runtimeSetupRoot.localScale;
			this.runtimeSetupRoot.localScale = new Vector3(PhotoBoothController.DivideScale(localScale.x, lossyScale.x), PhotoBoothController.DivideScale(localScale.y, lossyScale.y), PhotoBoothController.DivideScale(localScale.z, lossyScale.z));
		}

		// Token: 0x06004251 RID: 16977 RVA: 0x0018AD94 File Offset: 0x00188F94
		private static float DivideScale(float localScale, float worldScale)
		{
			float num = Mathf.Abs(worldScale);
			if (num <= 0.0001f)
			{
				return localScale;
			}
			return localScale / num;
		}

		// Token: 0x06004252 RID: 16978 RVA: 0x0018ADB8 File Offset: 0x00188FB8
		private void CacheCameraControlDefaults()
		{
			this.UpgradeCameraControlTuning();
			if (this.photoCamera == null)
			{
				return;
			}
			this._photoCameraBaseLocalEuler = this.photoCamera.transform.localEulerAngles;
			this._photoCameraBaseLocalEuler.z = 0f;
			this._photoCameraBaseLocalRotation = Quaternion.Euler(this._photoCameraBaseLocalEuler);
		}

		// Token: 0x06004253 RID: 16979 RVA: 0x0018AE14 File Offset: 0x00189014
		private void UpgradeCameraControlTuning()
		{
			this.minimumZoomFieldOfView = Mathf.Min(new float[] { this.minimumZoomFieldOfView, 10f, this.cameraFieldOfView });
			this.maximumZoomFieldOfView = Mathf.Max(new float[] { this.maximumZoomFieldOfView, 86f, this.cameraFieldOfView });
			this.zoomDegreesPerWheelStep = Mathf.Max(this.zoomDegreesPerWheelStep, 11f);
			this.zoomInDegreesPerWheelStep = ((this.zoomInDegreesPerWheelStep <= 0f) ? 6f : Mathf.Min(this.zoomInDegreesPerWheelStep, 6f));
			this.zoomSmoothTime = Mathf.Clamp(this.zoomSmoothTime, 0.08f, 0.24f);
			this.cameraPitchLimit = Mathf.Max(this.cameraPitchLimit, 36f);
			this.cameraYawLimit = Mathf.Max(this.cameraYawLimit, 50f);
			this.cameraDragSensitivity = Mathf.Max(this.cameraDragSensitivity, 0.55f);
		}

		// Token: 0x06004254 RID: 16980 RVA: 0x0018AF14 File Offset: 0x00189114
		private void ResetPhotoCameraControls()
		{
			this.UpgradeCameraControlTuning();
			if (this.photoCamera == null)
			{
				return;
			}
			this._photoCameraPitchOffset = 0f;
			this._photoCameraYawOffset = 0f;
			this.photoCamera.transform.localRotation = this._photoCameraBaseLocalRotation;
			this.photoCamera.fieldOfView = Mathf.Clamp(this.cameraFieldOfView, this.minimumZoomFieldOfView, this.maximumZoomFieldOfView);
			this._photoCameraTargetFieldOfView = this.photoCamera.fieldOfView;
			this._photoCameraZoomVelocity = 0f;
		}

		// Token: 0x06004255 RID: 16981 RVA: 0x0018AFA0 File Offset: 0x001891A0
		private void UpdatePhotoCameraControls()
		{
			if (ControllerInput.IsActive && ControllerWorldPointer.BlocksContextInput)
			{
				return;
			}
			if (this.photoCamera == null)
			{
				return;
			}
			float y = GameplayInput.mouseScrollDelta.y;
			if (Mathf.Abs(y) > 0.001f)
			{
				float num = ((y > 0f) ? this.zoomInDegreesPerWheelStep : this.zoomDegreesPerWheelStep);
				this._photoCameraTargetFieldOfView = Mathf.Clamp(this._photoCameraTargetFieldOfView - y * num, this.minimumZoomFieldOfView, this.maximumZoomFieldOfView);
			}
			this.photoCamera.fieldOfView = Mathf.SmoothDamp(this.photoCamera.fieldOfView, this._photoCameraTargetFieldOfView, ref this._photoCameraZoomVelocity, this.zoomSmoothTime, float.PositiveInfinity, Time.unscaledDeltaTime);
			if (GameplayInput.GetMouseButton(2) || (ControllerInput.IsActive && !ControllerInput.GetButton(ControllerButton.LeftTrigger)))
			{
				Vector2 vector = new Vector2(GameplayInput.GetAxisRaw("Mouse X"), GameplayInput.GetAxisRaw("Mouse Y"));
				if (!ControllerInput.IsActive)
				{
					vector = GameSettingsService.ApplyMouseLookInversion(vector);
				}
				this._photoCameraYawOffset = Mathf.Clamp(this._photoCameraYawOffset + vector.x * this.cameraDragSensitivity, -this.cameraYawLimit, this.cameraYawLimit);
				this._photoCameraPitchOffset = Mathf.Clamp(this._photoCameraPitchOffset - vector.y * this.cameraDragSensitivity, -this.cameraPitchLimit, this.cameraPitchLimit);
				this.photoCamera.transform.localRotation = Quaternion.Euler(this._photoCameraBaseLocalEuler.x + this._photoCameraPitchOffset, this._photoCameraBaseLocalEuler.y + this._photoCameraYawOffset, 0f);
			}
		}

		// Token: 0x06004256 RID: 16982 RVA: 0x0018B130 File Offset: 0x00189330
		private void PrepareCaptureAudio()
		{
			GameObject gameObject = new GameObject("PhotoBooth Capture Audio");
			gameObject.transform.SetParent(base.transform, false);
			this._captureAudioSource = gameObject.AddComponent<AudioSource>();
			this._captureAudioSource.playOnAwake = false;
			this._captureAudioSource.loop = false;
			this._captureAudioSource.spatialBlend = 0f;
			this._captureAudioSource.volume = 0.82f;
			this._flashAudioClip = PhotoBoothController.CreateFlashAudioClip();
		}

		// Token: 0x06004257 RID: 16983 RVA: 0x0018B1AC File Offset: 0x001893AC
		private void QueuePrint(PhotoBoothController.CapturedPhoto captured)
		{
			if (captured == null || captured.portraitTexture == null || captured.printTexture == null)
			{
				return;
			}
			captured.portraitTexture.wrapMode = TextureWrapMode.Clamp;
			captured.portraitTexture.filterMode = FilterMode.Bilinear;
			captured.printTexture.wrapMode = TextureWrapMode.Clamp;
			captured.printTexture.filterMode = FilterMode.Bilinear;
			this._pendingPrintTextures.Enqueue(captured);
		}

		// Token: 0x06004258 RID: 16984 RVA: 0x0018B218 File Offset: 0x00189418
		private void EnsurePortraitCaptureDimensions(Renderer printRenderer)
		{
			float num = 1.5f;
			float num2;
			if (PhotoBoothController.TryGetPrintFacePortraitAspect(printRenderer, out num2))
			{
				num = num2;
			}
			int num3 = Mathf.Clamp(Mathf.Max(this.captureWidth, this.captureHeight), 1280, 2560);
			int num4 = Mathf.RoundToInt((float)num3 / Mathf.Max(1.1f, num));
			num4 = Mathf.Clamp(num4, 640, num3 - 2);
			this.captureWidth = num4 + (num4 & 1);
			this.captureHeight = num3 + (num3 & 1);
		}

		// Token: 0x06004259 RID: 16985 RVA: 0x0018B294 File Offset: 0x00189494
		private static bool TryGetPrintFacePortraitAspect(Renderer printRenderer, out float portraitAspect)
		{
			portraitAspect = 1.5f;
			if (printRenderer == null)
			{
				return false;
			}
			SkinnedMeshRenderer skinnedMeshRenderer = printRenderer as SkinnedMeshRenderer;
			Mesh mesh;
			if (skinnedMeshRenderer != null)
			{
				mesh = skinnedMeshRenderer.sharedMesh;
			}
			else
			{
				MeshFilter component = printRenderer.GetComponent<MeshFilter>();
				mesh = ((component != null) ? component.sharedMesh : null);
			}
			if (mesh == null || mesh.subMeshCount == 0 || mesh.uv == null || mesh.uv.Length != mesh.vertexCount)
			{
				return false;
			}
			Vector2[] uv = mesh.uv;
			Vector3[] vertices = mesh.vertices;
			int[] triangles = mesh.GetTriangles(0);
			if (triangles == null || triangles.Length < 3)
			{
				return false;
			}
			Vector2 vector = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
			Vector2 vector2 = new Vector2(float.NegativeInfinity, float.NegativeInfinity);
			for (int i = 0; i < triangles.Length; i++)
			{
				Vector2 vector3 = uv[triangles[i]];
				vector = Vector2.Min(vector, vector3);
				vector2 = Vector2.Max(vector2, vector3);
			}
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			for (int j = 0; j <= triangles.Length - 3; j += 3)
			{
				int num4 = triangles[j];
				int num5 = triangles[j + 1];
				int num6 = triangles[j + 2];
				Vector2 vector4 = uv[num5] - uv[num4];
				Vector2 vector5 = uv[num6] - uv[num4];
				float num7 = vector4.x * vector5.y - vector4.y * vector5.x;
				if (Mathf.Abs(num7) > 1E-06f)
				{
					Vector3 vector6 = vertices[num5] - vertices[num4];
					Vector3 vector7 = vertices[num6] - vertices[num4];
					float sqrMagnitude = Vector3.Cross(vector6, vector7).sqrMagnitude;
					if (sqrMagnitude > num3)
					{
						Vector3 vector8 = (vector6 * vector5.y - vector7 * vector4.y) / num7;
						Vector3 vector9 = (vector7 * vector4.x - vector6 * vector5.x) / num7;
						vector8 = printRenderer.transform.TransformVector(vector8);
						vector9 = printRenderer.transform.TransformVector(vector9);
						if (vector8.sqrMagnitude > 1E-06f && vector9.sqrMagnitude > 1E-06f)
						{
							num3 = sqrMagnitude;
							num = vector8.magnitude;
							num2 = vector9.magnitude;
						}
					}
				}
			}
			Vector2 vector10 = vector2 - vector;
			float num8 = vector10.x * num;
			float num9 = vector10.y * num2;
			if (num8 <= 0.0001f || num9 <= 0.0001f)
			{
				return false;
			}
			portraitAspect = num9 / num8;
			return portraitAspect > 1.01f && portraitAspect < 4f;
		}

		// Token: 0x0600425A RID: 16986 RVA: 0x0018B568 File Offset: 0x00189768
		private static Texture2D RotateLandscapeCounterClockwise(Texture2D source)
		{
			Color32[] pixels = source.GetPixels32();
			int width = source.width;
			int height = source.height;
			int num = height;
			int num2 = width;
			Color32[] array = new Color32[pixels.Length];
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					int num3 = i;
					int num4 = width - 1 - j;
					array[num4 * num + num3] = pixels[i * width + j];
				}
			}
			Texture2D texture2D = new Texture2D(num, num2, TextureFormat.RGB24, false, false);
			texture2D.SetPixels32(array);
			texture2D.Apply(false, false);
			return texture2D;
		}

		// Token: 0x0600425B RID: 16987 RVA: 0x0018B5FC File Offset: 0x001897FC
		private static Texture2D RotatePortrait180(Texture2D source)
		{
			Color32[] pixels = source.GetPixels32();
			Color32[] array = new Color32[pixels.Length];
			int num = pixels.Length - 1;
			for (int i = 0; i < pixels.Length; i++)
			{
				array[num - i] = pixels[i];
			}
			Texture2D texture2D = new Texture2D(source.width, source.height, TextureFormat.RGB24, false, false);
			texture2D.SetPixels32(array);
			texture2D.Apply(false, false);
			return texture2D;
		}

		// Token: 0x0600425C RID: 16988 RVA: 0x0018B660 File Offset: 0x00189860
		private void BeginPrintEjectionIfReady()
		{
			if (!base.isActiveAndEnabled || this._sessionActive || this._captureInProgress || this._pendingPrintTextures.Count == 0 || this.photoPrintTemplate == null || this._printEjectionRoutine != null)
			{
				return;
			}
			this._printEjectionRoutine = base.StartCoroutine(this.EjectQueuedPrints());
		}

		// Token: 0x0600425D RID: 16989 RVA: 0x0018B6BB File Offset: 0x001898BB
		private IEnumerator EjectQueuedPrints()
		{
			this._isEjectingPrints = true;
			this.ResolvePlayer();
			PhotoBoothPrintHolder holder = PhotoBoothPrintHolder.GetOrCreate(this._player);
			while (this._pendingPrintTextures.Count > 0)
			{
				this._spawnedPrints.RemoveAll((PhotoBoothPrint existing) => existing == null || existing.IsCollected);
				this.ReflowEjectedPrintStack(true);
				PhotoBoothController.CapturedPhoto capturedPhoto = this._pendingPrintTextures.Dequeue();
				Transform transform = Object.Instantiate<Transform>(this.photoPrintTemplate, this.photoPrintTemplate.parent);
				Object @object = transform;
				string text = "Photo Print ";
				int num = this._printedPhotoCount + 1;
				this._printedPhotoCount = num;
				@object.name = text + num.ToString("00");
				PhotoBoothPrint print = transform.GetComponent<PhotoBoothPrint>();
				if (print == null)
				{
					print = transform.gameObject.AddComponent<PhotoBoothPrint>();
				}
				print.Initialize(capturedPhoto.portraitTexture, capturedPhoto.printTexture, this.printInteractionConfig, holder, this.photoPrintTemplate.gameObject, this._printedPhotoCount, capturedPhoto.archiveId);
				this._spawnedPrints.Add(print);
				print.PrepareForEjection();
				print.SetHidePhotoWeight(100f);
				transform.gameObject.SetActive(true);
				if (this._printerAudioSource != null && this._printerAudioClip != null)
				{
					this._printerAudioSource.PlayOneShot(this._printerAudioClip);
				}
				float ejectDuration = Mathf.Max(0.95f, this.printEjectDuration);
				float elapsed = 0f;
				while (elapsed < ejectDuration)
				{
					elapsed += Time.deltaTime;
					float num2 = Mathf.Clamp01(elapsed / ejectDuration);
					float num3 = 1f - Mathf.Pow(1f - num2, 3f);
					print.SetHidePhotoWeight(Mathf.Lerp(100f, 0f, num3));
					yield return null;
				}
				print.SetHidePhotoWeight(0f);
				print.FinishEjection();
				this.ReflowEjectedPrintStack(false);
				yield return new WaitForSeconds(0.24f);
				print = null;
			}
			this._isEjectingPrints = false;
			this._printEjectionRoutine = null;
			yield break;
		}

		// Token: 0x0600425E RID: 16990 RVA: 0x0018B6CC File Offset: 0x001898CC
		private void ReflowEjectedPrintStack(bool reserveOutputSlot)
		{
			int num = (reserveOutputSlot ? 1 : 0);
			for (int i = this._spawnedPrints.Count - 1; i >= 0; i--)
			{
				PhotoBoothPrint photoBoothPrint = this._spawnedPrints[i];
				if (!(photoBoothPrint == null) && photoBoothPrint.IsAvailable)
				{
					photoBoothPrint.SetOutputStackLevel(num++, base.transform.up, base.transform.forward);
				}
			}
		}

		// Token: 0x0600425F RID: 16991 RVA: 0x0018B738 File Offset: 0x00189938
		private static AudioClip CreatePrinterAudioClip()
		{
			int num = Mathf.CeilToInt(34398f);
			float[] array = new float[num];
			Random random = new Random(7319);
			for (int i = 0; i < num; i++)
			{
				float num2 = (float)i / 44100f;
				float num3 = Mathf.Sin(Mathf.Clamp01(num2 / 0.78f) * 3.1415927f);
				float num4 = Mathf.Sin(num2 * 3.1415927f * 2f * 82f) * 0.13f + Mathf.Sin(num2 * 3.1415927f * 2f * 164f) * 0.045f;
				float num5 = ((float)random.NextDouble() * 2f - 1f) * 0.055f;
				float num6 = Mathf.Exp(-num2 * 52f) * Mathf.Sin(num2 * 3.1415927f * 2f * 620f) * 0.22f;
				float num7 = Mathf.Max(0f, num2 - 0.62f);
				float num8 = ((num7 > 0f) ? (Mathf.Exp(-num7 * 45f) * Mathf.Sin(num7 * 3.1415927f * 2f * 410f) * 0.18f) : 0f);
				array[i] = (num4 + num5) * num3 + num6 + num8;
			}
			AudioClip audioClip = AudioClip.Create("PhotoBooth Printer", num, 1, 44100, false);
			audioClip.SetData(array, 0);
			return audioClip;
		}

		// Token: 0x06004260 RID: 16992 RVA: 0x0018B8A4 File Offset: 0x00189AA4
		private static AudioClip CreateFlashAudioClip()
		{
			int num = Mathf.CeilToInt(8379f);
			float[] array = new float[num];
			Random random = new Random(9241);
			for (int i = 0; i < num; i++)
			{
				float num2 = (float)i / 44100f;
				float num3 = (float)random.NextDouble() * 2f - 1f;
				float num4 = Mathf.Exp(-num2 * 72f) * (num3 * 0.52f + Mathf.Sin(num2 * 3.1415927f * 2f * 1180f) * 0.34f);
				float num5 = Mathf.Max(0f, num2 - 0.072f);
				float num6 = ((num2 >= 0.072f) ? Mathf.Exp(-num5 * 82f) : 0f) * (num3 * 0.38f + Mathf.Sin(num5 * 3.1415927f * 2f * 860f) * 0.3f);
				float num7 = Mathf.Sin(num2 * 3.1415927f * 2f * 135f) * Mathf.Exp(-num2 * 25f) * 0.09f;
				array[i] = Mathf.Clamp((num4 + num6 + num7) * 0.78f, -1f, 1f);
			}
			AudioClip audioClip = AudioClip.Create("PhotoBooth Flash Shutter", num, 1, 44100, false);
			audioClip.SetData(array, 0);
			return audioClip;
		}

		// Token: 0x06004261 RID: 16993 RVA: 0x0018BA00 File Offset: 0x00189C00
		private PhotoBoothPrint GetNextAvailablePrint()
		{
			if (this._isEjectingPrints)
			{
				return null;
			}
			for (int i = this._spawnedPrints.Count - 1; i >= 0; i--)
			{
				if (this._spawnedPrints[i] == null)
				{
					this._spawnedPrints.RemoveAt(i);
				}
			}
			for (int j = 0; j < this._spawnedPrints.Count; j++)
			{
				PhotoBoothPrint photoBoothPrint = this._spawnedPrints[j];
				if (photoBoothPrint != null && photoBoothPrint.IsAvailable)
				{
					return photoBoothPrint;
				}
			}
			return null;
		}

		// Token: 0x06004262 RID: 16994 RVA: 0x0018BA88 File Offset: 0x00189C88
		private int CollectAllAvailablePrints()
		{
			this.ResolvePlayer();
			if (this._player == null)
			{
				return 0;
			}
			PhotoBoothPrintHolder orCreate = PhotoBoothPrintHolder.GetOrCreate(this._player);
			if (orCreate == null)
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < this._spawnedPrints.Count; i++)
			{
				PhotoBoothPrint photoBoothPrint = this._spawnedPrints[i];
				if (!(photoBoothPrint == null) && photoBoothPrint.IsAvailable)
				{
					if (!orCreate.StoreInInventory(photoBoothPrint))
					{
						break;
					}
					num++;
				}
			}
			this._spawnedPrints.RemoveAll((PhotoBoothPrint existing) => existing == null || existing.IsCollected);
			return num;
		}

		// Token: 0x06004263 RID: 16995 RVA: 0x0018BB34 File Offset: 0x00189D34
		private void CachePlayerSessionComponents()
		{
			if (this._player == null)
			{
				return;
			}
			if (this._playerInput == null)
			{
				this._playerInput = this._player.GetComponent<vThirdPersonInput>();
				if (this._playerInput == null)
				{
					this._playerInput = this._player.GetComponentInParent<vThirdPersonInput>();
				}
			}
			if (this._firstPersonController == null)
			{
				this._firstPersonController = this._player.GetComponent<FirstPersonController>();
				if (this._firstPersonController == null)
				{
					this._firstPersonController = this._player.GetComponentInParent<FirstPersonController>();
				}
			}
			if (this._playerAnimator == null)
			{
				this._playerAnimator = this._player.animator;
				if (this._playerAnimator == null)
				{
					this._playerAnimator = this._player.GetComponentInChildren<Animator>(true);
				}
			}
			this._photoBoothPoseParameterHash = Animator.StringToHash("PhotoBoothPose");
			this._faceExpressionParameterHash = Animator.StringToHash("FaceExpressionIndex");
			this._actionStateParameterHash = Animator.StringToHash("ActionState");
			if (this._playerEmotionController == null)
			{
				this._playerEmotionController = this._player.GetComponent<PlayerEmotionController>();
				if (this._playerEmotionController == null)
				{
					this._playerEmotionController = this._player.GetComponentInParent<PlayerEmotionController>();
				}
				if (this._playerEmotionController == null)
				{
					this._playerEmotionController = this._player.GetComponentInChildren<PlayerEmotionController>(true);
				}
				if (this._playerEmotionController == null)
				{
					this._playerEmotionController = PlayerEmotionController.Instance;
				}
			}
		}

		// Token: 0x06004264 RID: 16996 RVA: 0x0018BCB4 File Offset: 0x00189EB4
		private void BeginPhotoBoothFace()
		{
			this.CachePlayerSessionComponents();
			if (this._playerAnimator == null)
			{
				return;
			}
			if (!this._hasSavedFaceExpression)
			{
				this._savedFaceExpression = this._playerAnimator.GetInteger(this._faceExpressionParameterHash);
				this._hasSavedFaceExpression = true;
			}
			if (this._playerEmotionController != null && !this._facialExpressionSuppressed)
			{
				this._playerEmotionController.DisableFacialExpressionForAction();
				this._facialExpressionSuppressed = true;
			}
			this.ApplyPhotoBoothFace();
		}

		// Token: 0x06004265 RID: 16997 RVA: 0x0018BD2C File Offset: 0x00189F2C
		private void ApplyPhotoBoothFace()
		{
			if (this._playerAnimator == null || PhotoBoothController.Faces.Length == 0)
			{
				return;
			}
			this._faceIndex = PhotoBoothController.Wrap(this._faceIndex, PhotoBoothController.Faces.Length);
			int animatorIndex = PhotoBoothController.Faces[this._faceIndex].animatorIndex;
			if (this._playerEmotionController != null)
			{
				this._playerEmotionController.SetExpression(animatorIndex);
				return;
			}
			this._playerAnimator.SetInteger(this._faceExpressionParameterHash, animatorIndex);
		}

		// Token: 0x06004266 RID: 16998 RVA: 0x0018BDAC File Offset: 0x00189FAC
		private void EndPhotoBoothFace()
		{
			if (this._hasSavedFaceExpression && this._playerAnimator != null)
			{
				this._playerAnimator.SetInteger(this._faceExpressionParameterHash, this._savedFaceExpression);
			}
			this._hasSavedFaceExpression = false;
			if (this._facialExpressionSuppressed && this._playerEmotionController != null)
			{
				this._playerEmotionController.RestoreFacialExpression();
			}
			this._facialExpressionSuppressed = false;
		}

		// Token: 0x06004267 RID: 16999 RVA: 0x0018BE18 File Offset: 0x0018A018
		private void BeginPhotoBoothMovement()
		{
			this.CachePlayerSessionComponents();
			if (this._playerInput == null || this._player == null || this.photoCamera == null)
			{
				return;
			}
			this._savedMovementCamera = this._playerInput.cameraMain;
			this._savedRotateTarget = this._player.rotateTarget;
			this._savedLockCameraInput = this._playerInput.lockCameraInput;
			this._savedSuppressCameraZoom = this._playerInput.suppressCameraZoom;
			this._savedJumpInputEnabled = this._playerInput.jumpInput.useInput;
			this._savedRollInputEnabled = this._playerInput.rollInput.useInput;
			this._playerInput.jumpInput.useInput = false;
			this._playerInput.rollInput.useInput = false;
			if (this._firstPersonController != null)
			{
				this._restoreFirstPersonOnExit = this._firstPersonController.IsFirstPerson;
				this._savedFirstPersonExternalLock = this._firstPersonController.ExternalLock;
				this._firstPersonController.ExternalLock = false;
				if (this._restoreFirstPersonOnExit)
				{
					this._firstPersonController.ExitFirstPerson();
				}
				this._firstPersonController.ExternalLock = true;
			}
			this._playerInput.cameraMain = this.photoCamera;
			this._player.rotateTarget = this.photoCamera.transform;
			this._playerInput.lockCameraInput = true;
			this._playerInput.suppressCameraZoom = true;
			this._movementReferenceOverridden = true;
		}

		// Token: 0x06004268 RID: 17000 RVA: 0x0018BF8C File Offset: 0x0018A18C
		private void EndPhotoBoothMovement()
		{
			if (!this._movementReferenceOverridden)
			{
				return;
			}
			this._movementReferenceOverridden = false;
			if (this._playerInput != null)
			{
				this._playerInput.cameraMain = this._savedMovementCamera;
				this._playerInput.lockCameraInput = this._savedLockCameraInput;
				this._playerInput.suppressCameraZoom = this._savedSuppressCameraZoom;
				this._playerInput.jumpInput.useInput = this._savedJumpInputEnabled;
				this._playerInput.rollInput.useInput = this._savedRollInputEnabled;
			}
			if (this._player != null)
			{
				this._player.rotateTarget = ((this._savedRotateTarget != null) ? this._savedRotateTarget : ((this._savedMovementCamera != null) ? this._savedMovementCamera.transform : null));
			}
			if (this._firstPersonController != null)
			{
				this._firstPersonController.ExternalLock = this._savedFirstPersonExternalLock;
				if (this._restoreFirstPersonOnExit)
				{
					this._firstPersonController.EnterFirstPerson();
				}
			}
			this._restoreFirstPersonOnExit = false;
		}

		// Token: 0x06004269 RID: 17001 RVA: 0x0018C09C File Offset: 0x0018A29C
		private void MaintainPhotoBoothMovementReference()
		{
			if (!this._movementReferenceOverridden || this._playerInput == null || this._player == null || this.photoCamera == null)
			{
				return;
			}
			if (this._playerInput.cameraMain != this.photoCamera)
			{
				this._playerInput.cameraMain = this.photoCamera;
			}
			if (this._player.rotateTarget != this.photoCamera.transform)
			{
				this._player.rotateTarget = this.photoCamera.transform;
			}
			this._playerInput.lockCameraInput = true;
			this._playerInput.suppressCameraZoom = true;
		}

		// Token: 0x0600426A RID: 17002 RVA: 0x0018C150 File Offset: 0x0018A350
		private void BeginPhotoBoothPose()
		{
			this.CachePlayerSessionComponents();
			if (this._playerAnimator == null)
			{
				return;
			}
			int num = Animator.StringToHash("Base Layer.Actions.Photo Booth.Pose 01");
			if (!this._playerAnimator.HasState(0, num))
			{
				if (!this._poseActionWarningIssued)
				{
					Debug.LogWarning("[PhotoBooth] Base Layer action 'Actions/Photo Booth' is missing. Run Tools/Photo Booth/Setup Booth Action once.", this);
					this._poseActionWarningIssued = true;
				}
				return;
			}
			int integer = this._playerAnimator.GetInteger(this._actionStateParameterHash);
			this._savedActionState = ((integer == 5) ? 0 : integer);
			this._photoBoothActionActive = false;
			this._emotionLayersSuppressed = false;
			this._playerAnimator.SetInteger(this._photoBoothPoseParameterHash, 0);
			this.ApplyPhotoBoothPose();
		}

		// Token: 0x0600426B RID: 17003 RVA: 0x0018C1F0 File Offset: 0x0018A3F0
		private void ApplyPhotoBoothPose()
		{
			if (this._playerAnimator == null)
			{
				return;
			}
			if (this._poseIndex <= 0)
			{
				this._playerAnimator.SetInteger(this._photoBoothPoseParameterHash, 0);
				this._playerAnimator.SetInteger(this._actionStateParameterHash, this._savedActionState);
				this.RestorePoseOverrideLayers();
				if (this._photoBoothActionActive)
				{
					this.CrossFadeToLocomotion(0.42f);
				}
				this._photoBoothActionActive = false;
				return;
			}
			this.SuppressPoseOverrideLayers();
			this._playerAnimator.SetInteger(this._actionStateParameterHash, 5);
			this._playerAnimator.SetInteger(this._photoBoothPoseParameterHash, this._poseIndex);
			string text = "Pose " + this._poseIndex.ToString("00");
			int num = Animator.StringToHash("Base Layer.Actions.Photo Booth." + text);
			if (!this._playerAnimator.HasState(0, num))
			{
				return;
			}
			this._playerAnimator.CrossFadeInFixedTime(num, 0.42f, 0, 0f);
			this._photoBoothActionActive = true;
		}

		// Token: 0x0600426C RID: 17004 RVA: 0x0018C2E8 File Offset: 0x0018A4E8
		private void UpdatePhotoBoothPose()
		{
			if (this._playerAnimator == null || !this._photoBoothActionActive || this._poseIndex <= 0)
			{
				return;
			}
			this._playerAnimator.SetInteger(this._actionStateParameterHash, 5);
			this._playerAnimator.SetInteger(this._photoBoothPoseParameterHash, this._poseIndex);
		}

		// Token: 0x0600426D RID: 17005 RVA: 0x0018C340 File Offset: 0x0018A540
		private void EndPhotoBoothPose()
		{
			if (this._playerAnimator == null)
			{
				this.RestorePoseOverrideLayers();
				this._photoBoothActionActive = false;
				return;
			}
			this._playerAnimator.SetInteger(this._photoBoothPoseParameterHash, 0);
			this._playerAnimator.SetInteger(this._actionStateParameterHash, this._savedActionState);
			if (this._photoBoothActionActive)
			{
				this.CrossFadeToLocomotion(0.42f);
			}
			this.RestorePoseOverrideLayers();
			this._photoBoothActionActive = false;
		}

		// Token: 0x0600426E RID: 17006 RVA: 0x0018C3B4 File Offset: 0x0018A5B4
		private void SuppressPoseOverrideLayers()
		{
			if (this._emotionLayersSuppressed)
			{
				return;
			}
			PlayerEmotionController playerEmotionController = ((this._playerEmotionController != null) ? this._playerEmotionController : PlayerEmotionController.Instance);
			if (playerEmotionController == null)
			{
				return;
			}
			playerEmotionController.DisableEmotionLayersForAction();
			this._emotionLayersSuppressed = true;
		}

		// Token: 0x0600426F RID: 17007 RVA: 0x0018C400 File Offset: 0x0018A600
		private void RestorePoseOverrideLayers()
		{
			if (!this._emotionLayersSuppressed)
			{
				return;
			}
			PlayerEmotionController playerEmotionController = ((this._playerEmotionController != null) ? this._playerEmotionController : PlayerEmotionController.Instance);
			if (playerEmotionController != null)
			{
				playerEmotionController.RestoreEmotionLayers();
			}
			this._emotionLayersSuppressed = false;
		}

		// Token: 0x06004270 RID: 17008 RVA: 0x0018C448 File Offset: 0x0018A648
		private void CrossFadeToLocomotion(float duration)
		{
			if (this._playerAnimator == null)
			{
				return;
			}
			int num = Animator.StringToHash("Base Layer.Locomotion.Free Locomotion.Free Movement");
			if (!this._playerAnimator.HasState(0, num))
			{
				Debug.LogError("[PhotoBooth] Missing locomotion exit state: Base Layer.Locomotion.Free Locomotion.Free Movement", this);
				return;
			}
			this._playerAnimator.CrossFadeInFixedTime(num, duration, 0);
		}

		// Token: 0x06004271 RID: 17009 RVA: 0x0018C498 File Offset: 0x0018A698
		private void KeepPlayerInside()
		{
			if (this._player == null || this.interiorBounds == null)
			{
				return;
			}
			Transform transform = this.interiorBounds.transform;
			Vector3 vector = transform.InverseTransformPoint(this._player.transform.position);
			Vector3 center = this.interiorBounds.center;
			Vector3 vector2 = this.interiorBounds.size * 0.5f;
			float num = Mathf.Max(0.001f, Mathf.Abs(transform.lossyScale.x));
			float num2 = Mathf.Max(0.001f, Mathf.Abs(transform.lossyScale.z));
			float num3 = this.playerBoundaryPadding / num;
			float num4 = this.playerBoundaryPadding / num2;
			vector.x = Mathf.Clamp(vector.x, center.x - vector2.x + num3, center.x + vector2.x - num3);
			vector.z = Mathf.Clamp(vector.z, center.z - vector2.z + num4, center.z + vector2.z - num4);
			Vector3 vector3 = transform.TransformPoint(vector);
			if ((this._player.transform.position - vector3).sqrMagnitude > 1E-06f)
			{
				this._player.transform.position = vector3;
				if (this._playerBody != null)
				{
					this._playerBody.position = vector3;
				}
				Physics.SyncTransforms();
			}
		}

		// Token: 0x06004272 RID: 17010 RVA: 0x0018C61C File Offset: 0x0018A81C
		private void PositionPlayerForPhoto()
		{
			if (this._player == null || this.playerStartMarker == null)
			{
				return;
			}
			Vector3 vector = this.playerStartMarker.forward;
			vector = Vector3.ProjectOnPlane(vector, Vector3.up);
			Quaternion quaternion = ((vector.sqrMagnitude > 0.001f) ? Quaternion.LookRotation(vector.normalized, Vector3.up) : this._player.transform.rotation);
			this._player.transform.SetPositionAndRotation(this.playerStartMarker.position, quaternion);
			if (this._playerBody != null)
			{
				this._playerBody.position = this.playerStartMarker.position;
				this._playerBody.rotation = quaternion;
				this._playerBody.velocity = Vector3.zero;
				this._playerBody.angularVelocity = Vector3.zero;
			}
			Physics.SyncTransforms();
		}

		// Token: 0x06004273 RID: 17011 RVA: 0x0018C701 File Offset: 0x0018A901
		private void SetPromptVisible(bool visible)
		{
			this.SetPromptVisible(visible, this.interactionConfig, false);
		}

		// Token: 0x06004274 RID: 17012 RVA: 0x0018C714 File Offset: 0x0018A914
		private void PrepareInteractionPrompt()
		{
			if (this.promptUI == null)
			{
				return;
			}
			PhotoBoothController.SetUniformWorldScale(this.promptUI.transform, 0.0025f);
			this._promptDefaultLocalPosition = this.promptUI.transform.localPosition;
			this._promptDefaultLocalRotation = this.promptUI.transform.localRotation;
		}

		// Token: 0x06004275 RID: 17013 RVA: 0x0018C774 File Offset: 0x0018A974
		private void SetPromptVisible(bool visible, InteractionConfigSO config, bool showHoldBar)
		{
			if (!visible)
			{
				if (!this._promptVisible)
				{
					return;
				}
				this._promptVisible = false;
				this._activePromptConfig = null;
				if (this.promptUI != null)
				{
					this.promptUI.Hide();
				}
				return;
			}
			else
			{
				bool flag = !this._promptVisible || this._activePromptConfig != config || (this.promptUI != null && !this.promptUI.IsVisible);
				this._promptVisible = true;
				this._activePromptConfig = config;
				if (this.promptUI == null)
				{
					return;
				}
				if (flag)
				{
					this.promptUI.transform.localPosition = this._promptDefaultLocalPosition;
					this.promptUI.transform.localRotation = this._promptDefaultLocalRotation;
					PhotoBoothController.SetUniformWorldScale(this.promptUI.transform, 0.0025f);
					this.promptUI.Show(config);
				}
				this.promptUI.SetHoldProgress(this._printPickupHoldTime / 0.65f, showHoldBar);
				return;
			}
		}

		// Token: 0x06004276 RID: 17014 RVA: 0x0018C874 File Offset: 0x0018AA74
		private static void SetUniformWorldScale(Transform target, float worldScale)
		{
			if (target == null)
			{
				return;
			}
			Vector3 vector = ((target.parent != null) ? target.parent.lossyScale : Vector3.one);
			target.localScale = new Vector3(worldScale / Mathf.Max(0.0001f, Mathf.Abs(vector.x)), worldScale / Mathf.Max(0.0001f, Mathf.Abs(vector.y)), worldScale / Mathf.Max(0.0001f, Mathf.Abs(vector.z)));
		}

		// Token: 0x06004277 RID: 17015 RVA: 0x0018C8FC File Offset: 0x0018AAFC
		private void SetPhotoBackdropVisible(bool visible)
		{
			if (this.photoBackdropExtension != null)
			{
				this.photoBackdropExtension.enabled = visible;
			}
		}

		// Token: 0x06004278 RID: 17016 RVA: 0x0018C918 File Offset: 0x0018AB18
		private void SuppressGameplayHUD()
		{
			this.RestoreGameplayHUD();
			foreach (Canvas canvas in Object.FindObjectsOfType<Canvas>(true))
			{
				if (!(canvas == null) && canvas.enabled && canvas.gameObject.activeInHierarchy && !canvas.transform.IsChildOf(base.transform))
				{
					this._suppressedGameplayCanvases.Add(new PhotoBoothController.CanvasState
					{
						canvas = canvas,
						enabled = canvas.enabled
					});
					canvas.enabled = false;
				}
			}
		}

		// Token: 0x06004279 RID: 17017 RVA: 0x0018C9A8 File Offset: 0x0018ABA8
		private void RestoreGameplayHUD()
		{
			for (int i = 0; i < this._suppressedGameplayCanvases.Count; i++)
			{
				PhotoBoothController.CanvasState canvasState = this._suppressedGameplayCanvases[i];
				if (canvasState.canvas != null)
				{
					canvasState.canvas.enabled = canvasState.enabled;
				}
			}
			this._suppressedGameplayCanvases.Clear();
		}

		// Token: 0x0600427A RID: 17018 RVA: 0x0018CA04 File Offset: 0x0018AC04
		private void CacheLightDefaults()
		{
			if (this.atmosphereLights == null)
			{
				this.atmosphereLights = new Light[0];
			}
			this._baseLightIntensities = new float[this.atmosphereLights.Length];
			for (int i = 0; i < this.atmosphereLights.Length; i++)
			{
				this._baseLightIntensities[i] = ((this.atmosphereLights[i] != null) ? this.atmosphereLights[i].intensity : 0f);
			}
		}

		// Token: 0x0600427B RID: 17019 RVA: 0x0018CA77 File Offset: 0x0018AC77
		private void ApplyAllVisualSettings()
		{
			this.ApplyLights();
			this.ApplyBackdrop();
			this.ApplyFilter();
			if (this._filteredPreview != null)
			{
				this._filteredPreview.ApplyLayout(this._layoutIndex);
			}
		}

		// Token: 0x0600427C RID: 17020 RVA: 0x0018CAAC File Offset: 0x0018ACAC
		private void ApplyLights()
		{
			Color color = PhotoBoothController.LightColors[this._lightColorIndex];
			float num = Mathf.InverseLerp(0.25f, 2f, this._lightIntensityMultiplier);
			if (this._filteredPreview != null)
			{
				this._filteredPreview.ApplyLighting(color, num);
			}
			for (int i = 0; i < this.atmosphereLights.Length; i++)
			{
				Light light = this.atmosphereLights[i];
				if (!(light == null))
				{
					light.color = color;
					float num2 = ((i < this._baseLightIntensities.Length) ? this._baseLightIntensities[i] : light.intensity);
					light.intensity = num2 * Mathf.Lerp(0.08f, 1.55f, num);
				}
			}
			if (this.glowRenderers == null)
			{
				return;
			}
			Color color2 = color * Mathf.Lerp(0.04f, 1.35f, num);
			Color color3 = color * Mathf.Lerp(0.14f, 0.68f, num);
			foreach (Renderer renderer in this.glowRenderers)
			{
				if (!(renderer == null))
				{
					renderer.GetPropertyBlock(this._propertyBlock);
					this._propertyBlock.SetColor("_EmissionColor", color2);
					this._propertyBlock.SetColor("_Color", color3);
					this._propertyBlock.SetColor("_BaseColor", color3);
					renderer.SetPropertyBlock(this._propertyBlock);
					this._propertyBlock.Clear();
				}
			}
		}

		// Token: 0x0600427D RID: 17021 RVA: 0x0018CC24 File Offset: 0x0018AE24
		private void ApplyBackdrop()
		{
			PhotoBoothController.BackdropPreset backdropPreset = PhotoBoothController.Backdrops[this._backdropIndex];
			if (this.backdropRenderers == null)
			{
				return;
			}
			foreach (Renderer renderer in this.backdropRenderers)
			{
				if (!(renderer == null))
				{
					renderer.GetPropertyBlock(this._propertyBlock);
					this._propertyBlock.SetColor("_TopColor", backdropPreset.top);
					this._propertyBlock.SetColor("_BottomColor", backdropPreset.bottom);
					this._propertyBlock.SetColor("_AccentColor", backdropPreset.accent);
					this._propertyBlock.SetFloat("_Pattern", backdropPreset.pattern);
					renderer.SetPropertyBlock(this._propertyBlock);
					this._propertyBlock.Clear();
				}
			}
		}

		// Token: 0x0600427E RID: 17022 RVA: 0x0018CCF0 File Offset: 0x0018AEF0
		private void EnsureFilterRuntime()
		{
			if (this._filterVolume != null || this.photoCamera == null)
			{
				return;
			}
			GameObject gameObject = new GameObject("PhotoBooth Filter Volume");
			gameObject.transform.SetParent(base.transform, false);
			this._filterVolume = gameObject.AddComponent<Volume>();
			this._filterVolume.isGlobal = true;
			this._filterVolume.priority = 1000000f;
			this._filterVolume.weight = 0f;
			this._filterProfile = ScriptableObject.CreateInstance<VolumeProfile>();
			this._filterProfile.name = "PhotoBooth Filter Profile (Runtime)";
			this._filterProfile.hideFlags = HideFlags.DontSave;
			this._filterVolume.sharedProfile = this._filterProfile;
			this._colorAdjustments = this._filterProfile.Add<ColorAdjustments>(true);
			this._whiteBalance = this._filterProfile.Add<WhiteBalance>(true);
			this._colorAdjustments.SetAllOverridesTo(true);
			this._whiteBalance.SetAllOverridesTo(true);
			this._photoCameraData = this.photoCamera.GetComponent<UniversalAdditionalCameraData>();
			if (this._photoCameraData == null)
			{
				this._photoCameraData = this.photoCamera.gameObject.AddComponent<UniversalAdditionalCameraData>();
			}
			UniversalAdditionalCameraData photoCameraData = this._photoCameraData;
			photoCameraData.volumeLayerMask |= 1 << base.gameObject.layer;
			this._photoCameraData.volumeTrigger = this.photoCamera.transform;
			this._savedPostProcessing = this._photoCameraData.renderPostProcessing;
		}

		// Token: 0x0600427F RID: 17023 RVA: 0x0018CE6C File Offset: 0x0018B06C
		private void ApplyFilter()
		{
			this.EnsureFilterRuntime();
			if (this._filterVolume == null || this._colorAdjustments == null || this._whiteBalance == null)
			{
				return;
			}
			PhotoBoothController.FilterPreset filterPreset = PhotoBoothController.Filters[this._filterIndex];
			float num = Mathf.InverseLerp(0.25f, 2f, this._lightIntensityMultiplier);
			float num2 = Mathf.Lerp(-2f, 0.25f, Mathf.Pow(num, 0.8f));
			Color color = filterPreset.color;
			this._colorAdjustments.postExposure.value = filterPreset.exposure + num2;
			this._colorAdjustments.contrast.value = filterPreset.contrast;
			this._colorAdjustments.hueShift.value = filterPreset.hue;
			this._colorAdjustments.saturation.value = filterPreset.saturation;
			this._colorAdjustments.colorFilter.value = color;
			this._whiteBalance.temperature.value = filterPreset.temperature;
			this._whiteBalance.tint.value = filterPreset.tint;
			this._filterVolume.weight = 0f;
			if (this._filteredPreview != null)
			{
				this._filteredPreview.Apply(this._colorAdjustments.postExposure.value, filterPreset.contrast, filterPreset.hue, filterPreset.saturation, this._whiteBalance.temperature.value, this._whiteBalance.tint.value, color);
			}
		}

		// Token: 0x06004280 RID: 17024 RVA: 0x0018CFF6 File Offset: 0x0018B1F6
		private static int Wrap(int value, int count)
		{
			return (value % count + count) % count;
		}

		// Token: 0x06004281 RID: 17025 RVA: 0x0018D000 File Offset: 0x0018B200
		private void OnDestroy()
		{
			this.InterruptForEncounter();
			while (this._pendingPrintTextures.Count > 0)
			{
				PhotoBoothController.CapturedPhoto capturedPhoto = this._pendingPrintTextures.Dequeue();
				if (capturedPhoto != null)
				{
					capturedPhoto.DestroyTextures();
				}
			}
			if (this._printerAudioClip != null)
			{
				Object.Destroy(this._printerAudioClip);
			}
			if (this._flashAudioClip != null)
			{
				Object.Destroy(this._flashAudioClip);
			}
			if (this._filterProfile != null)
			{
				Object.Destroy(this._filterProfile);
			}
			if (this._combinedBoothPrintConfig != null)
			{
				Object.Destroy(this._combinedBoothPrintConfig);
			}
		}

		// Token: 0x06004282 RID: 17026 RVA: 0x0018D0A0 File Offset: 0x0018B2A0
		private void OnDrawGizmosSelected()
		{
			Transform transform = ((this.interactionAnchor != null) ? this.interactionAnchor : base.transform);
			Gizmos.color = (this._playerInRange ? Color.green : new Color(1f, 0.45f, 0.72f, 1f));
			Gizmos.DrawWireSphere(transform.position, this.interactionDistance);
		}

		// Token: 0x0400420B RID: 16907
		private const int SettingCount = 8;

		// Token: 0x0400420C RID: 16908
		private const int PhotoBoothPoseCount = 26;

		// Token: 0x0400420D RID: 16909
		private const int PhotoBoothActionState = 5;

		// Token: 0x0400420E RID: 16910
		private const string PhotoBoothPoseParameterName = "PhotoBoothPose";

		// Token: 0x0400420F RID: 16911
		private const string FaceExpressionParameterName = "FaceExpressionIndex";

		// Token: 0x04004210 RID: 16912
		private const string PhotoBoothActionPath = "Base Layer.Actions.Photo Booth.";

		// Token: 0x04004211 RID: 16913
		private const string FreeMovementPath = "Base Layer.Locomotion.Free Locomotion.Free Movement";

		// Token: 0x04004212 RID: 16914
		private const float PhotoBoothPoseBlendDuration = 0.42f;

		// Token: 0x04004213 RID: 16915
		private const float PrintPickupHoldDuration = 0.65f;

		// Token: 0x04004214 RID: 16916
		private const float BoothPromptWorldScale = 0.0025f;

		// Token: 0x04004215 RID: 16917
		private const string RuntimeSetupRootName = "PhotoBooth_RuntimeSetup";

		// Token: 0x04004216 RID: 16918
		[Header("Interaction")]
		[SerializeField]
		private InteractionConfigSO interactionConfig;

		// Token: 0x04004217 RID: 16919
		[SerializeField]
		private InteractablePromptUI promptUI;

		// Token: 0x04004218 RID: 16920
		[SerializeField]
		private Transform interactionAnchor;

		// Token: 0x04004219 RID: 16921
		[SerializeField]
		[Min(0.5f)]
		private float interactionDistance = 1.85f;

		// Token: 0x0400421A RID: 16922
		public GenericInput interactInput = new GenericInput("E", "A", "A");

		// Token: 0x0400421B RID: 16923
		[Header("Session")]
		[SerializeField]
		private Transform runtimeSetupRoot;

		// Token: 0x0400421C RID: 16924
		[SerializeField]
		private Camera photoCamera;

		// Token: 0x0400421D RID: 16925
		[SerializeField]
		private Transform photoTarget;

		// Token: 0x0400421E RID: 16926
		[SerializeField]
		private Transform playerStartMarker;

		// Token: 0x0400421F RID: 16927
		[SerializeField]
		private BoxCollider interiorBounds;

		// Token: 0x04004220 RID: 16928
		[SerializeField]
		private Collider entranceGate;

		// Token: 0x04004221 RID: 16929
		[SerializeField]
		[Range(0.05f, 0.45f)]
		private float playerBoundaryPadding = 0.2f;

		// Token: 0x04004222 RID: 16930
		[Header("Look")]
		[SerializeField]
		private Light[] atmosphereLights;

		// Token: 0x04004223 RID: 16931
		[SerializeField]
		private Renderer[] glowRenderers;

		// Token: 0x04004224 RID: 16932
		[FormerlySerializedAs("backdropRenderer")]
		[SerializeField]
		private Renderer[] backdropRenderers;

		// Token: 0x04004225 RID: 16933
		[Tooltip("Wide backdrop used only by the booth camera. It stays hidden from gameplay cameras.")]
		[SerializeField]
		private Renderer photoBackdropExtension;

		// Token: 0x04004226 RID: 16934
		[SerializeField]
		private PhotoBoothEffectOverlay effectOverlay;

		// Token: 0x04004227 RID: 16935
		[SerializeField]
		private PhotoBoothPanel settingsPanel;

		// Token: 0x04004228 RID: 16936
		[SerializeField]
		private PhotoBoothMirrorSwitch mirrorSwitch;

		// Token: 0x04004229 RID: 16937
		[Header("Photo Prints")]
		[SerializeField]
		private Transform photoPrintTemplate;

		// Token: 0x0400422A RID: 16938
		[SerializeField]
		private InteractionConfigSO printInteractionConfig;

		// Token: 0x0400422B RID: 16939
		[SerializeField]
		[Min(0.05f)]
		private float printRetractionDistance = 0.18f;

		// Token: 0x0400422C RID: 16940
		[SerializeField]
		[Min(0.15f)]
		private float printEjectDuration = 0.72f;

		// Token: 0x0400422D RID: 16941
		[Header("Capture")]
		[SerializeField]
		[Min(640f)]
		private int captureWidth = 1280;

		// Token: 0x0400422E RID: 16942
		[SerializeField]
		[Min(360f)]
		private int captureHeight = 1920;

		// Token: 0x0400422F RID: 16943
		[SerializeField]
		[Range(38f, 70f)]
		private float cameraFieldOfView = 46.6f;

		// Token: 0x04004230 RID: 16944
		[Tooltip("When disabled, the authored PhotoBooth_PhotoCamera rotation is preserved.")]
		[SerializeField]
		private bool autoAimPhotoCamera;

		// Token: 0x04004231 RID: 16945
		[SerializeField]
		[Range(-0.65f, 0.35f)]
		private float cameraAimHeightOffset = -0.3f;

		// Token: 0x04004232 RID: 16946
		[SerializeField]
		[Range(0.005f, 0.15f)]
		private float cameraNearClipPlane = 0.01f;

		// Token: 0x04004233 RID: 16947
		[SerializeField]
		[Min(25f)]
		private float cameraFarClipPlane = 250f;

		// Token: 0x04004234 RID: 16948
		[Header("Camera Controls")]
		[SerializeField]
		[Range(6f, 45f)]
		private float minimumZoomFieldOfView = 10f;

		// Token: 0x04004235 RID: 16949
		[SerializeField]
		[Range(50f, 95f)]
		private float maximumZoomFieldOfView = 86f;

		// Token: 0x04004236 RID: 16950
		[Tooltip("Zoom-out strength. Kept separate so zooming back remains controlled.")]
		[SerializeField]
		[Range(2f, 18f)]
		private float zoomDegreesPerWheelStep = 11f;

		// Token: 0x04004237 RID: 16951
		[Tooltip("Wheel-up zoom strength. Kept deliberately fine-grained for precise framing.")]
		[SerializeField]
		[Range(2f, 18f)]
		private float zoomInDegreesPerWheelStep = 6f;

		// Token: 0x04004238 RID: 16952
		[SerializeField]
		[Range(0.05f, 0.4f)]
		private float zoomSmoothTime = 0.14f;

		// Token: 0x04004239 RID: 16953
		[SerializeField]
		[Range(10f, 50f)]
		private float cameraPitchLimit = 36f;

		// Token: 0x0400423A RID: 16954
		[SerializeField]
		[Range(10f, 65f)]
		private float cameraYawLimit = 50f;

		// Token: 0x0400423B RID: 16955
		[SerializeField]
		[Range(0.05f, 1.5f)]
		private float cameraDragSensitivity = 0.55f;

		// Token: 0x0400423C RID: 16956
		[SerializeField]
		private bool debugLogs;

		// Token: 0x0400423D RID: 16957
		private static readonly string[] LightColorNames = new string[] { "WARM", "PINK", "COOL BLUE", "WHITE", "AMBER" };

		// Token: 0x0400423E RID: 16958
		private static readonly Color[] LightColors = new Color[]
		{
			new Color(1f, 0.74f, 0.56f),
			new Color(1f, 0.36f, 0.64f),
			new Color(0.42f, 0.76f, 1f),
			new Color(1f, 0.96f, 0.9f),
			new Color(1f, 0.52f, 0.2f)
		};

		// Token: 0x0400423F RID: 16959
		private static readonly PhotoBoothController.BackdropPreset[] Backdrops = new PhotoBoothController.BackdropPreset[]
		{
			new PhotoBoothController.BackdropPreset("CLASSIC", new Color(0.87f, 0.72f, 0.83f), new Color(0.34f, 0.17f, 0.3f), new Color(1f, 0.74f, 0.87f), 0f),
			new PhotoBoothController.BackdropPreset("PINK STRIPES", new Color(1f, 0.42f, 0.69f), new Color(0.25f, 0.035f, 0.2f), new Color(1f, 0.82f, 0.92f), 1f),
			new PhotoBoothController.BackdropPreset("NEON BLUE", new Color(0.12f, 0.45f, 0.78f), new Color(0.025f, 0.055f, 0.19f), new Color(0.28f, 0.93f, 1f), 2f),
			new PhotoBoothController.BackdropPreset("GOLD DOTS", new Color(0.42f, 0.2f, 0.08f), new Color(0.1f, 0.035f, 0.02f), new Color(1f, 0.73f, 0.22f), 3f)
		};

		// Token: 0x04004240 RID: 16960
		private static readonly PhotoBoothController.FacePreset[] Faces = new PhotoBoothController.FacePreset[]
		{
			new PhotoBoothController.FacePreset("NEUTRAL", 0),
			new PhotoBoothController.FacePreset("EMBARRASSED", 1),
			new PhotoBoothController.FacePreset("HORNY", 2),
			new PhotoBoothController.FacePreset("HORNY SUPREME", 3),
			new PhotoBoothController.FacePreset("EMBARRASSED SUPREME", 4),
			new PhotoBoothController.FacePreset("ORGASM", 5),
			new PhotoBoothController.FacePreset("HAPPY WINK", 8),
			new PhotoBoothController.FacePreset("HORNY LIGHT", 9),
			new PhotoBoothController.FacePreset("HORNY HEAVY", 10),
			new PhotoBoothController.FacePreset("AHEGAO", 11),
			new PhotoBoothController.FacePreset("EYES CLOSED", 12),
			new PhotoBoothController.FacePreset(">_<", 13)
		};

		// Token: 0x04004241 RID: 16961
		private static readonly PhotoBoothController.FilterPreset[] Filters = new PhotoBoothController.FilterPreset[]
		{
			new PhotoBoothController.FilterPreset("NATURAL", 0f, 0f, 0f, 0f, 0f, 0f, Color.white),
			new PhotoBoothController.FilterPreset("WARM", 0.1f, 12f, -3f, 14f, 36f, 8f, new Color(1f, 0.9f, 0.76f)),
			new PhotoBoothController.FilterPreset("COOL", 0.02f, 14f, 1f, 8f, -42f, -5f, new Color(0.76f, 0.9f, 1f)),
			new PhotoBoothController.FilterPreset("FILM", -0.12f, -12f, -8f, -32f, 20f, 13f, new Color(1f, 0.85f, 0.64f)),
			new PhotoBoothController.FilterPreset("MONO", 0.04f, 34f, 0f, -100f, 0f, 0f, Color.white),
			new PhotoBoothController.FilterPreset("DREAM", 0.22f, -24f, -7f, 20f, 12f, 30f, new Color(1f, 0.76f, 0.96f))
		};

		// Token: 0x04004242 RID: 16962
		private vThirdPersonController _player;

		// Token: 0x04004243 RID: 16963
		private vThirdPersonInput _playerInput;

		// Token: 0x04004244 RID: 16964
		private Rigidbody _playerBody;

		// Token: 0x04004245 RID: 16965
		private Animator _playerAnimator;

		// Token: 0x04004246 RID: 16966
		private Camera _savedMovementCamera;

		// Token: 0x04004247 RID: 16967
		private Transform _savedRotateTarget;

		// Token: 0x04004248 RID: 16968
		private bool _savedLockCameraInput;

		// Token: 0x04004249 RID: 16969
		private bool _savedSuppressCameraZoom;

		// Token: 0x0400424A RID: 16970
		private bool _savedJumpInputEnabled;

		// Token: 0x0400424B RID: 16971
		private bool _savedRollInputEnabled;

		// Token: 0x0400424C RID: 16972
		private bool _movementReferenceOverridden;

		// Token: 0x0400424D RID: 16973
		private FirstPersonController _firstPersonController;

		// Token: 0x0400424E RID: 16974
		private bool _restoreFirstPersonOnExit;

		// Token: 0x0400424F RID: 16975
		private bool _playerInRange;

		// Token: 0x04004250 RID: 16976
		private bool _promptVisible;

		// Token: 0x04004251 RID: 16977
		private bool _sessionActive;

		// Token: 0x04004252 RID: 16978
		private bool _captureInProgress;

		// Token: 0x04004253 RID: 16979
		private int _captureGeneration;

		// Token: 0x04004254 RID: 16980
		private PlayerActivityLease _sessionLease;

		// Token: 0x04004255 RID: 16981
		private float _candidateSqrDistance = float.MaxValue;

		// Token: 0x04004256 RID: 16982
		private readonly List<Camera> _disabledCameras = new List<Camera>(4);

		// Token: 0x04004257 RID: 16983
		private readonly List<PhotoBoothController.CanvasState> _suppressedGameplayCanvases = new List<PhotoBoothController.CanvasState>(24);

		// Token: 0x04004258 RID: 16984
		private readonly Queue<PhotoBoothController.CapturedPhoto> _pendingPrintTextures = new Queue<PhotoBoothController.CapturedPhoto>();

		// Token: 0x04004259 RID: 16985
		private readonly List<PhotoBoothPrint> _spawnedPrints = new List<PhotoBoothPrint>(8);

		// Token: 0x0400425A RID: 16986
		private string _gateReason;

		// Token: 0x0400425B RID: 16987
		private const PlayerContextResource SessionResources = PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation | PlayerContextResource.ModalUI | PlayerContextResource.HeldItem;

		// Token: 0x0400425C RID: 16988
		private int _selectedSetting;

		// Token: 0x0400425D RID: 16989
		private int _lightColorIndex;

		// Token: 0x0400425E RID: 16990
		private int _backdropIndex;

		// Token: 0x0400425F RID: 16991
		private int _effectIndex;

		// Token: 0x04004260 RID: 16992
		private int _filterIndex;

		// Token: 0x04004261 RID: 16993
		private int _poseIndex;

		// Token: 0x04004262 RID: 16994
		private int _faceIndex;

		// Token: 0x04004263 RID: 16995
		private int _layoutIndex;

		// Token: 0x04004264 RID: 16996
		private int _photoBoothPoseParameterHash;

		// Token: 0x04004265 RID: 16997
		private int _faceExpressionParameterHash;

		// Token: 0x04004266 RID: 16998
		private int _actionStateParameterHash;

		// Token: 0x04004267 RID: 16999
		private int _savedActionState;

		// Token: 0x04004268 RID: 17000
		private bool _photoBoothActionActive;

		// Token: 0x04004269 RID: 17001
		private bool _emotionLayersSuppressed;

		// Token: 0x0400426A RID: 17002
		private bool _poseActionWarningIssued;

		// Token: 0x0400426B RID: 17003
		private PlayerEmotionController _playerEmotionController;

		// Token: 0x0400426C RID: 17004
		private bool _facialExpressionSuppressed;

		// Token: 0x0400426D RID: 17005
		private bool _hasSavedFaceExpression;

		// Token: 0x0400426E RID: 17006
		private int _savedFaceExpression;

		// Token: 0x0400426F RID: 17007
		private float _lightIntensityMultiplier = 1f;

		// Token: 0x04004270 RID: 17008
		private float[] _baseLightIntensities;

		// Token: 0x04004271 RID: 17009
		private MaterialPropertyBlock _propertyBlock;

		// Token: 0x04004272 RID: 17010
		private Volume _filterVolume;

		// Token: 0x04004273 RID: 17011
		private VolumeProfile _filterProfile;

		// Token: 0x04004274 RID: 17012
		private ColorAdjustments _colorAdjustments;

		// Token: 0x04004275 RID: 17013
		private WhiteBalance _whiteBalance;

		// Token: 0x04004276 RID: 17014
		private UniversalAdditionalCameraData _photoCameraData;

		// Token: 0x04004277 RID: 17015
		private bool _savedPostProcessing;

		// Token: 0x04004278 RID: 17016
		private PhotoBoothFilteredPreview _filteredPreview;

		// Token: 0x04004279 RID: 17017
		private AudioSource _printerAudioSource;

		// Token: 0x0400427A RID: 17018
		private AudioClip _printerAudioClip;

		// Token: 0x0400427B RID: 17019
		private AudioSource _captureAudioSource;

		// Token: 0x0400427C RID: 17020
		private AudioClip _flashAudioClip;

		// Token: 0x0400427D RID: 17021
		private Quaternion _photoCameraBaseLocalRotation;

		// Token: 0x0400427E RID: 17022
		private Vector3 _photoCameraBaseLocalEuler;

		// Token: 0x0400427F RID: 17023
		private float _photoCameraPitchOffset;

		// Token: 0x04004280 RID: 17024
		private float _photoCameraYawOffset;

		// Token: 0x04004281 RID: 17025
		private float _photoCameraTargetFieldOfView;

		// Token: 0x04004282 RID: 17026
		private float _photoCameraZoomVelocity;

		// Token: 0x04004283 RID: 17027
		private Vector3 _promptDefaultLocalPosition;

		// Token: 0x04004284 RID: 17028
		private Quaternion _promptDefaultLocalRotation;

		// Token: 0x04004285 RID: 17029
		private Coroutine _printEjectionRoutine;

		// Token: 0x04004286 RID: 17030
		private Vector3 _printEjectDirection;

		// Token: 0x04004287 RID: 17031
		private int _printedPhotoCount;

		// Token: 0x04004288 RID: 17032
		private bool _isEjectingPrints;

		// Token: 0x04004289 RID: 17033
		private InteractionConfigSO _activePromptConfig;

		// Token: 0x0400428A RID: 17034
		private InteractionConfigSO _combinedBoothPrintConfig;

		// Token: 0x0400428B RID: 17035
		private float _printPickupHoldTime;

		// Token: 0x0400428C RID: 17036
		private bool _printPickupConsumed;

		// Token: 0x0400428D RID: 17037
		private bool _printInteractionHeld;

		// Token: 0x0400428E RID: 17038
		private static PhotoBoothController _activeSession;

		// Token: 0x0400428F RID: 17039
		private bool _savedFirstPersonExternalLock;

		// Token: 0x0200096D RID: 2413
		private struct FilterPreset
		{
			// Token: 0x06004285 RID: 17029 RVA: 0x0018D6D4 File Offset: 0x0018B8D4
			public FilterPreset(string label, float exposure, float contrast, float hue, float saturation, float temperature, float tint, Color color)
			{
				this.label = label;
				this.exposure = exposure;
				this.contrast = contrast;
				this.hue = hue;
				this.saturation = saturation;
				this.temperature = temperature;
				this.tint = tint;
				this.color = color;
			}

			// Token: 0x04004290 RID: 17040
			public string label;

			// Token: 0x04004291 RID: 17041
			public float exposure;

			// Token: 0x04004292 RID: 17042
			public float contrast;

			// Token: 0x04004293 RID: 17043
			public float hue;

			// Token: 0x04004294 RID: 17044
			public float saturation;

			// Token: 0x04004295 RID: 17045
			public float temperature;

			// Token: 0x04004296 RID: 17046
			public float tint;

			// Token: 0x04004297 RID: 17047
			public Color color;
		}

		// Token: 0x0200096E RID: 2414
		private struct BackdropPreset
		{
			// Token: 0x06004286 RID: 17030 RVA: 0x0018D713 File Offset: 0x0018B913
			public BackdropPreset(string label, Color top, Color bottom, Color accent, float pattern)
			{
				this.label = label;
				this.top = top;
				this.bottom = bottom;
				this.accent = accent;
				this.pattern = pattern;
			}

			// Token: 0x04004298 RID: 17048
			public string label;

			// Token: 0x04004299 RID: 17049
			public Color top;

			// Token: 0x0400429A RID: 17050
			public Color bottom;

			// Token: 0x0400429B RID: 17051
			public Color accent;

			// Token: 0x0400429C RID: 17052
			public float pattern;
		}

		// Token: 0x0200096F RID: 2415
		private struct FacePreset
		{
			// Token: 0x06004287 RID: 17031 RVA: 0x0018D73A File Offset: 0x0018B93A
			public FacePreset(string label, int animatorIndex)
			{
				this.label = label;
				this.animatorIndex = animatorIndex;
			}

			// Token: 0x0400429D RID: 17053
			public string label;

			// Token: 0x0400429E RID: 17054
			public int animatorIndex;
		}

		// Token: 0x02000970 RID: 2416
		private struct CanvasState
		{
			// Token: 0x0400429F RID: 17055
			public Canvas canvas;

			// Token: 0x040042A0 RID: 17056
			public bool enabled;
		}

		// Token: 0x02000971 RID: 2417
		private sealed class CapturedPhoto
		{
			// Token: 0x06004288 RID: 17032 RVA: 0x0018D74A File Offset: 0x0018B94A
			public CapturedPhoto(Texture2D portraitTexture, Texture2D printTexture, string persistentArchiveId = null)
			{
				this.portraitTexture = portraitTexture;
				this.printTexture = printTexture;
				this.archiveId = persistentArchiveId;
			}

			// Token: 0x06004289 RID: 17033 RVA: 0x0018D768 File Offset: 0x0018B968
			public void DestroyTextures()
			{
				if (this.portraitTexture != null)
				{
					Object.Destroy(this.portraitTexture);
				}
				if (this.printTexture != null && this.printTexture != this.portraitTexture)
				{
					Object.Destroy(this.printTexture);
				}
				this.portraitTexture = null;
				this.printTexture = null;
			}

			// Token: 0x040042A1 RID: 17057
			public Texture2D portraitTexture;

			// Token: 0x040042A2 RID: 17058
			public Texture2D printTexture;

			// Token: 0x040042A3 RID: 17059
			public string archiveId;
		}

		// Token: 0x02000972 RID: 2418
		internal enum PrintPickupAction
		{
			// Token: 0x040042A5 RID: 17061
			None,
			// Token: 0x040042A6 RID: 17062
			Collect,
			// Token: 0x040042A7 RID: 17063
			UseBooth
		}
	}
}
