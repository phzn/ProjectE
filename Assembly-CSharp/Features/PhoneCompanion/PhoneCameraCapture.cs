using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Features.CensorBars;
using Features.DebugTools;
using Features.Localization;
using Features.MainMenu;
using Features.RapidActions;
using Features.TrashcanLoot;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.Rendering;
using ProjectExhibitionist.Interactions;
using ProjektSumperk;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace Features.PhoneCompanion
{
	// Token: 0x02000E33 RID: 3635
	[DisallowMultipleComponent]
	public class PhoneCameraCapture : MonoBehaviour
	{
		// Token: 0x170011CE RID: 4558
		// (get) Token: 0x06006C62 RID: 27746 RVA: 0x002B5B6C File Offset: 0x002B3D6C
		public PhoneCameraCapture.CameraMediaMode MediaMode
		{
			get
			{
				return this._mediaMode;
			}
		}

		// Token: 0x170011CF RID: 4559
		// (get) Token: 0x06006C63 RID: 27747 RVA: 0x002B5B74 File Offset: 0x002B3D74
		public bool IsVideoMode
		{
			get
			{
				return this._mediaMode == PhoneCameraCapture.CameraMediaMode.Video;
			}
		}

		// Token: 0x170011D0 RID: 4560
		// (get) Token: 0x06006C64 RID: 27748 RVA: 0x002B5B7F File Offset: 0x002B3D7F
		public bool IsRecordingVideo
		{
			get
			{
				return this._videoRecordingRequested;
			}
		}

		// Token: 0x170011D1 RID: 4561
		// (get) Token: 0x06006C65 RID: 27749 RVA: 0x002B5B87 File Offset: 0x002B3D87
		public bool IsFinalizingVideo
		{
			get
			{
				return this._videoFinalizing;
			}
		}

		// Token: 0x170011D2 RID: 4562
		// (get) Token: 0x06006C66 RID: 27750 RVA: 0x002B5B8F File Offset: 0x002B3D8F
		public bool HasPendingRunCapture
		{
			get
			{
				return this._captureInProgress || this._videoRecordingRequested || this._videoFinalizing;
			}
		}

		// Token: 0x170011D3 RID: 4563
		// (get) Token: 0x06006C67 RID: 27751 RVA: 0x002B5BA9 File Offset: 0x002B3DA9
		public int CurrentFilterIndex
		{
			get
			{
				return this._filterIndex;
			}
		}

		// Token: 0x170011D4 RID: 4564
		// (get) Token: 0x06006C68 RID: 27752 RVA: 0x002B5BB1 File Offset: 0x002B3DB1
		public int FilterCount
		{
			get
			{
				return PhoneCameraCapture.FilterPresets.Length;
			}
		}

		// Token: 0x170011D5 RID: 4565
		// (get) Token: 0x06006C69 RID: 27753 RVA: 0x002B5BBA File Offset: 0x002B3DBA
		public string CurrentFilterName
		{
			get
			{
				return PhoneCameraCapture.FilterPresets[Mathf.Clamp(this._filterIndex, 0, PhoneCameraCapture.FilterPresets.Length - 1)].Label;
			}
		}

		// Token: 0x170011D6 RID: 4566
		// (get) Token: 0x06006C6A RID: 27754 RVA: 0x002B5BE0 File Offset: 0x002B3DE0
		public float RecordingDuration
		{
			get
			{
				if (!this._videoRecordingRequested)
				{
					return 0f;
				}
				return Mathf.Max(0f, (float)(Time.realtimeSinceStartupAsDouble - this._videoStartedAt));
			}
		}

		// Token: 0x06006C6B RID: 27755 RVA: 0x002B5C07 File Offset: 0x002B3E07
		public void Configure(Canvas canvas, Func<float> zoomLabelProvider)
		{
			this.phoneCanvas = canvas;
			this._zoomLabelProvider = zoomLabelProvider;
			this.ResolveToolkitGallery();
			this.EnsureAudio();
			this.ResolveFaceCensor();
		}

		// Token: 0x06006C6C RID: 27756 RVA: 0x002B5C29 File Offset: 0x002B3E29
		public void ConfigureTripod(Action toggleHandler, Func<bool> activeProvider, Func<bool> busyProvider, Func<bool> placementProvider, Func<Transform> audioSubjectProvider = null, Func<bool> placementAllowedProvider = null)
		{
			this._tripodToggleHandler = toggleHandler;
			this._tripodActiveProvider = activeProvider;
			this._tripodBusyProvider = busyProvider;
			this._tripodPlacementProvider = placementProvider;
			this._tripodPlacementAllowedProvider = placementAllowedProvider;
			this._tripodAudioSubjectProvider = audioSubjectProvider;
			this.UpdateTripodUi();
		}

		// Token: 0x06006C6D RID: 27757 RVA: 0x002B5C60 File Offset: 0x002B3E60
		public void SetCameraMode(bool active)
		{
			if (!active && (this._videoRecordingRequested || this._videoFinalizing))
			{
				this.StopVideoRecording();
			}
			this._cameraModeActive = active;
			this.SyncTripodSpatialAudio();
			if (active)
			{
				this.ApplyRecordingPreferences();
				this.EnsureOverlay();
				this._mediaMode = this.defaultMediaMode;
				this.ApplyMediaModeVisualsImmediate();
				this._disableFilterAfterVideo = false;
				this.ResetCameraFilterImmediate();
			}
			if (this._usingSceneOverlay && this.overlayUI != null)
			{
				this.overlayUI.SetVisible(active);
			}
			else if (this._overlayCanvas != null)
			{
				this._overlayCanvas.gameObject.SetActive(active);
			}
			else if (this._overlayRoot != null)
			{
				this._overlayRoot.SetActive(active);
			}
			if (!active)
			{
				if (this._videoFinalizing)
				{
					this._disableFilterAfterVideo = true;
				}
				else
				{
					this.DisableCameraFilterImmediate();
				}
			}
			if (this.debugLogs)
			{
				Debug.Log("[PhoneCameraCapture] Camera mode " + (active ? "ON" : "OFF") + ".", this);
			}
		}

		// Token: 0x06006C6E RID: 27758 RVA: 0x002B5D68 File Offset: 0x002B3F68
		private void EnsureOverlay()
		{
			if (this._overlayBound)
			{
				return;
			}
			if (this.overlayUI == null)
			{
				this.overlayUI = Object.FindObjectOfType<SelfieCameraOverlayUI>(true);
				if (this.overlayUI != null && this.debugLogs)
				{
					Debug.Log("[PhoneCameraCapture] Overlay in scena trovato via auto-find.", this);
				}
			}
			if (this.overlayUI != null)
			{
				this._usingSceneOverlay = true;
				this._overlayRoot = this.overlayUI.ActiveRoot;
				this._zoomLabel = this.overlayUI.zoomLabel;
				this._flashImage = this.overlayUI.flashImage;
				this._galleryAnchor = this.overlayUI.galleryAnchor;
				this._overlayCanvas = this.overlayUI.GetComponentInParent<Canvas>();
				this._shutterButton = this.overlayUI.shutterButton;
				if (this.clickToShoot && this.overlayUI.shutterButton != null)
				{
					this.overlayUI.shutterButton.onClick.RemoveListener(new UnityAction(this.CapturePhoto));
					this.overlayUI.shutterButton.onClick.RemoveListener(new UnityAction(this.HandleShutterPressed));
					this.overlayUI.shutterButton.onClick.AddListener(new UnityAction(this.HandleShutterPressed));
				}
				this._overlayBound = true;
				this.EnsureMediaModeUi();
				if (this.debugLogs)
				{
					Debug.Log("[PhoneCameraCapture] Overlay in scena collegato.", this);
					return;
				}
			}
			else if (this._overlayRoot == null)
			{
				this.BuildOverlay();
				this._overlayBound = true;
				this.EnsureMediaModeUi();
			}
		}

		// Token: 0x06006C6F RID: 27759 RVA: 0x002B5EF8 File Offset: 0x002B40F8
		private void Update()
		{
			this.SyncTripodSpatialAudio();
			if (!this._cameraModeActive)
			{
				return;
			}
			PhoneCameraCapture.RefreshKeyLabel(this._filterKeyLabel, this.filterSwitchKey, ref this._filterKeyState);
			PhoneCameraCapture.RefreshKeyLabel(this._mediaModeKeyLabel, this.mediaModeSwitchKey, ref this._mediaModeKeyState);
			bool flag = this._tripodPlacementProvider != null && this._tripodPlacementProvider();
			if (!flag && !this._videoRecordingRequested && !this._videoFinalizing && this.TryReadCameraKey(this.mediaModeSwitchKey))
			{
				this.SetMediaMode((this._mediaMode == PhoneCameraCapture.CameraMediaMode.Photo) ? PhoneCameraCapture.CameraMediaMode.Video : PhoneCameraCapture.CameraMediaMode.Photo);
			}
			if (!flag && !this._videoFinalizing && this.TryReadCameraKey(this.filterSwitchKey))
			{
				this.CycleFilter();
			}
			if (this.TryReadCameraKey(this.tripodToggleKey))
			{
				this.RequestTripodToggle();
			}
			if (this.TryReadCameraKey(this.shutterKey))
			{
				this.HandleShutterPressed();
			}
			this.UpdateZoomLabel();
			this.UpdateVideoHud();
			this.UpdateTripodUi();
		}

		// Token: 0x170011D7 RID: 4567
		// (get) Token: 0x06006C70 RID: 27760 RVA: 0x002B5FE3 File Offset: 0x002B41E3
		internal static bool CameraInputSuppressed
		{
			get
			{
				return GameplayInputGate.BlocksGameplayInput || TextInputFocus.BlocksGameplayInput || GraphicsSettingsRuntimeUI.AnyOpen || PlayerActivityGate.IsResourceOwned(PlayerContextResource.ModalUI) || RadialControllerNavigation.AnyWheelOpen || (ControllerInput.IsActive && ControllerInput.UiInputConsumedThisFrame);
			}
		}

		// Token: 0x06006C71 RID: 27761 RVA: 0x002B601C File Offset: 0x002B421C
		private GenericInput CameraInput(KeyCode key)
		{
			GenericInput genericInput;
			if (!this._cameraInputs.TryGetValue(key, out genericInput))
			{
				this._cameraInputs.Add(key, genericInput = new GenericInput(key.ToString(), ControllerInput.ForKey(key), key.ToString()));
			}
			return genericInput;
		}

		// Token: 0x06006C72 RID: 27762 RVA: 0x002B6070 File Offset: 0x002B4270
		internal bool IsGameplayKeyReserved(KeyCode key)
		{
			if (this._tripodActiveProvider == null || !this._tripodActiveProvider())
			{
				return false;
			}
			GenericInput genericInput = this.CameraInput(key);
			if (InteractionCandidateRegistry.IsContextInputReserved(genericInput))
			{
				return true;
			}
			if (WasteCanHideInteractable.ReservesContextInput(genericInput))
			{
				return true;
			}
			InteractionConfigSO interactionConfigSO = ((InteractionHUDController.Instance != null) ? InteractionHUDController.Instance.CurrentConfig : null);
			if (interactionConfigSO == null || PhoneHudHints.IsPhoneConfig(interactionConfigSO) || interactionConfigSO.usableActions == null)
			{
				return false;
			}
			string text = InteractionCandidateRegistry.ActiveBinding(genericInput);
			foreach (UsableAction usableAction in interactionConfigSO.usableActions)
			{
				if (usableAction != null)
				{
					string text2 = ControllerContextBindings.ResolveLabel(interactionConfigSO, usableAction.keyDisplayLabel, false, ControllerInput.IsActive);
					if (string.Equals(text, text2, StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06006C73 RID: 27763 RVA: 0x002B615C File Offset: 0x002B435C
		private bool TryReadCameraKey(KeyCode key)
		{
			if (PhoneCameraCapture.CameraInputSuppressed || this.IsGameplayKeyReserved(key) || !GameplayInput.GetKeyDown(key))
			{
				return false;
			}
			InteractionCandidateRegistry.MarkContextInputConsumed(this.CameraInput(key));
			return true;
		}

		// Token: 0x06006C74 RID: 27764 RVA: 0x002B6185 File Offset: 0x002B4385
		private static string CurrentKeyLabel(KeyCode key)
		{
			if (!ControllerInput.IsActive)
			{
				return key.ToString();
			}
			return ControllerGlyphs.Plain(ControllerInput.ForKey(key), ControllerGlyphs.Family);
		}

		// Token: 0x06006C75 RID: 27765 RVA: 0x002B61AC File Offset: 0x002B43AC
		private static void RefreshKeyLabel(Text text, KeyCode key, ref PhoneCameraCapture.KeyLabelState state)
		{
			if (text == null)
			{
				return;
			}
			bool isActive = ControllerInput.IsActive;
			int version = ControllerGlyphs.Version;
			if (state.target == text && state.key == key && state.controller == isActive && state.glyphVersion == version)
			{
				return;
			}
			state = new PhoneCameraCapture.KeyLabelState
			{
				target = text,
				key = key,
				controller = isActive,
				glyphVersion = version
			};
			string text2 = PhoneCameraCapture.CurrentKeyLabel(key);
			if (text.text != text2)
			{
				text.text = text2;
			}
		}

		// Token: 0x06006C76 RID: 27766 RVA: 0x002B6244 File Offset: 0x002B4444
		private string TripodButtonCaption()
		{
			if (this._tripodBusyProvider != null && this._tripodBusyProvider())
			{
				return GameLocalization.GetSource("phone_story", "MOVING...");
			}
			if (!ControllerInput.IsActive)
			{
				return GameLocalization.GetSource("phone_story", "T   PLACE TRIPOD");
			}
			return PhoneCameraCapture.CurrentKeyLabel(this.tripodToggleKey) + "   " + InteractionLocalization.ActionName("Place tripod");
		}

		// Token: 0x06006C77 RID: 27767 RVA: 0x002B62AC File Offset: 0x002B44AC
		private void RequestTripodToggle()
		{
			if (PhoneCameraCapture.CameraInputSuppressed || !this._cameraModeActive || this._tripodToggleHandler == null || (this._tripodPlacementAllowedProvider != null && !this._tripodPlacementAllowedProvider()) || (this._tripodBusyProvider != null && this._tripodBusyProvider()) || (this._tripodActiveProvider != null && this._tripodActiveProvider()) || (this._tripodPlacementProvider != null && this._tripodPlacementProvider()))
			{
				return;
			}
			if (this._videoRecordingRequested || this._videoFinalizing)
			{
				return;
			}
			this._tripodToggleHandler();
			this.UpdateTripodUi();
		}

		// Token: 0x06006C78 RID: 27768 RVA: 0x002B6348 File Offset: 0x002B4548
		public void HandleShutterPressed()
		{
			if (PhoneCameraCapture.CameraInputSuppressed)
			{
				return;
			}
			if (FlyCamera.IsActive || FlyCameraPhotoMode.AnyOpen)
			{
				return;
			}
			if (this._videoFinalizing || (this._tripodBusyProvider != null && this._tripodBusyProvider()) || (this._tripodPlacementProvider != null && this._tripodPlacementProvider()))
			{
				return;
			}
			if (this._mediaMode != PhoneCameraCapture.CameraMediaMode.Video)
			{
				this.CapturePhoto();
				return;
			}
			if (this._videoRecordingRequested)
			{
				this.StopVideoRecording();
				return;
			}
			this.StartVideoRecording();
		}

		// Token: 0x06006C79 RID: 27769 RVA: 0x002B63C4 File Offset: 0x002B45C4
		public void SetMediaMode(PhoneCameraCapture.CameraMediaMode mode)
		{
			if (this._videoRecordingRequested || this._videoFinalizing || this._mediaMode == mode)
			{
				return;
			}
			PhoneCameraCapture.CameraMediaMode mediaMode = this._mediaMode;
			this._mediaMode = mode;
			this.EnsureAudio();
			this.PlayVideoUiSound(this.mediaModeSwitchClip);
			if (this._mediaModeTransition != null)
			{
				base.StopCoroutine(this._mediaModeTransition);
			}
			this._mediaModeTransition = base.StartCoroutine(this.AnimateMediaMode(mediaMode, mode));
		}

		// Token: 0x06006C7A RID: 27770 RVA: 0x002B6433 File Offset: 0x002B4633
		public void CycleFilter()
		{
			if (!this._cameraModeActive || this._videoFinalizing || PhoneCameraCapture.FilterPresets.Length <= 1)
			{
				return;
			}
			this.SelectFilter((this._filterIndex + 1) % PhoneCameraCapture.FilterPresets.Length, true);
		}

		// Token: 0x06006C7B RID: 27771 RVA: 0x002B6468 File Offset: 0x002B4668
		public void SelectFilter(int index, bool animate = true)
		{
			if (!this._cameraModeActive || this._videoFinalizing || PhoneCameraCapture.FilterPresets.Length == 0)
			{
				return;
			}
			int num = (index % PhoneCameraCapture.FilterPresets.Length + PhoneCameraCapture.FilterPresets.Length) % PhoneCameraCapture.FilterPresets.Length;
			if (num == this._filterIndex && animate)
			{
				return;
			}
			this.EnsureFilterRuntime();
			this._filterIndex = num;
			PhoneCameraCapture.CameraFilterPreset cameraFilterPreset = PhoneCameraCapture.FilterPresets[this._filterIndex];
			if (this._filterIndex != 0)
			{
				this.EnsureFilterPostProcessing();
			}
			this.UpdateFilterUiImmediate();
			if (this._filterTransition != null)
			{
				base.StopCoroutine(this._filterTransition);
				this._filterTransition = null;
			}
			if (!animate || !base.isActiveAndEnabled)
			{
				this.ApplyFilterPreset(cameraFilterPreset);
				if (this._filterVolume != null)
				{
					this._filterVolume.weight = ((this._filterIndex == 0) ? 0f : 1f);
				}
				if (this._filterIndex == 0)
				{
					this.RestoreFilterPostProcessing();
				}
				this.SetFilterUiRestState();
				return;
			}
			this._filterTransition = base.StartCoroutine(this.AnimateCameraFilter(cameraFilterPreset, this._filterIndex != 0));
		}

		// Token: 0x06006C7C RID: 27772 RVA: 0x002B6574 File Offset: 0x002B4774
		private void EnsureFilterRuntime()
		{
			if (this._filterVolume != null && this._filterProfile != null && this._filterColorAdjustments != null && this._filterWhiteBalance != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("PhoneCameraFilterVolume");
			gameObject.transform.SetParent(base.transform, false);
			gameObject.hideFlags = HideFlags.DontSave;
			this._filterVolume = gameObject.AddComponent<Volume>();
			this._filterVolume.isGlobal = true;
			this._filterVolume.priority = 10000f;
			this._filterVolume.weight = 0f;
			this._filterProfile = ScriptableObject.CreateInstance<VolumeProfile>();
			this._filterProfile.name = "PhoneCameraFilterProfile (Runtime)";
			this._filterProfile.hideFlags = HideFlags.DontSave;
			this._filterVolume.sharedProfile = this._filterProfile;
			this._filterColorAdjustments = this._filterProfile.Add<ColorAdjustments>(true);
			this._filterWhiteBalance = this._filterProfile.Add<WhiteBalance>(true);
			this.ApplyFilterPreset(PhoneCameraCapture.FilterPresets[0]);
		}

		// Token: 0x06006C7D RID: 27773 RVA: 0x002B6688 File Offset: 0x002B4888
		private void EnsureFilterPostProcessing()
		{
			if (this._mainCamera == null)
			{
				this._mainCamera = Camera.main;
			}
			if (this._mainCamera == null)
			{
				return;
			}
			UniversalAdditionalCameraData universalAdditionalCameraData = this._mainCamera.GetComponent<UniversalAdditionalCameraData>();
			if (universalAdditionalCameraData == null)
			{
				universalAdditionalCameraData = this._mainCamera.gameObject.AddComponent<UniversalAdditionalCameraData>();
			}
			if (!this._filterCameraPostProcessingOverridden || this._filterCameraData != universalAdditionalCameraData)
			{
				this.RestoreFilterPostProcessing();
				this._filterCameraData = universalAdditionalCameraData;
				this._filterCameraPostProcessingSaved = universalAdditionalCameraData.renderPostProcessing;
				this._filterCameraPostProcessingOverridden = true;
			}
			universalAdditionalCameraData.renderPostProcessing = true;
			if (this._filterVolume != null)
			{
				int value = universalAdditionalCameraData.volumeLayerMask.value;
				for (int i = 0; i < 32; i++)
				{
					if ((value & (1 << i)) != 0)
					{
						this._filterVolume.gameObject.layer = i;
						return;
					}
				}
			}
		}

		// Token: 0x06006C7E RID: 27774 RVA: 0x002B6766 File Offset: 0x002B4966
		private void RestoreFilterPostProcessing()
		{
			if (this._filterCameraPostProcessingOverridden && this._filterCameraData != null)
			{
				this._filterCameraData.renderPostProcessing = this._filterCameraPostProcessingSaved;
			}
			this._filterCameraData = null;
			this._filterCameraPostProcessingOverridden = false;
		}

		// Token: 0x06006C7F RID: 27775 RVA: 0x002B679D File Offset: 0x002B499D
		private IEnumerator AnimateCameraFilter(PhoneCameraCapture.CameraFilterPreset target, bool targetEnabled)
		{
			float duration = Mathf.Max(0.05f, this.filterTransitionDuration);
			float elapsed = 0f;
			float startExposure = this._filterColorAdjustments.postExposure.value;
			float startContrast = this._filterColorAdjustments.contrast.value;
			float startHue = this._filterColorAdjustments.hueShift.value;
			float startSaturation = this._filterColorAdjustments.saturation.value;
			Color startColor = this._filterColorAdjustments.colorFilter.value;
			float startTemperature = this._filterWhiteBalance.temperature.value;
			float startTint = this._filterWhiteBalance.tint.value;
			float startWeight = this._filterVolume.weight;
			float targetWeight = (targetEnabled ? 1f : 0f);
			while (elapsed < duration)
			{
				elapsed += Time.unscaledDeltaTime;
				float num = Mathf.Clamp01(elapsed / duration);
				float num2 = Mathf.SmoothStep(0f, 1f, num);
				this._filterColorAdjustments.postExposure.value = Mathf.Lerp(startExposure, target.Exposure, num2);
				this._filterColorAdjustments.contrast.value = Mathf.Lerp(startContrast, target.Contrast, num2);
				this._filterColorAdjustments.hueShift.value = Mathf.Lerp(startHue, target.Hue, num2);
				this._filterColorAdjustments.saturation.value = Mathf.Lerp(startSaturation, target.Saturation, num2);
				this._filterColorAdjustments.colorFilter.value = Color.Lerp(startColor, target.ColorFilter, num2);
				this._filterWhiteBalance.temperature.value = Mathf.Lerp(startTemperature, target.Temperature, num2);
				this._filterWhiteBalance.tint.value = Mathf.Lerp(startTint, target.Tint, num2);
				this._filterVolume.weight = Mathf.Lerp(startWeight, targetWeight, num2);
				float num3 = Mathf.Sin(num * 3.1415927f);
				if (this._filterPanel != null)
				{
					this._filterPanel.rectTransform.localScale = Vector3.one * (1f + num3 * 0.055f);
				}
				if (this._filterPanelGroup != null)
				{
					this._filterPanelGroup.alpha = Mathf.Lerp(0.58f, 1f, num3);
				}
				yield return null;
			}
			this.ApplyFilterPreset(target);
			this._filterVolume.weight = targetWeight;
			if (!targetEnabled)
			{
				this.RestoreFilterPostProcessing();
			}
			this.SetFilterUiRestState();
			this._filterTransition = null;
			yield break;
		}

		// Token: 0x06006C80 RID: 27776 RVA: 0x002B67BC File Offset: 0x002B49BC
		private void ApplyFilterPreset(PhoneCameraCapture.CameraFilterPreset preset)
		{
			if (this._filterColorAdjustments == null || this._filterWhiteBalance == null)
			{
				return;
			}
			this._filterColorAdjustments.postExposure.value = preset.Exposure;
			this._filterColorAdjustments.contrast.value = preset.Contrast;
			this._filterColorAdjustments.hueShift.value = preset.Hue;
			this._filterColorAdjustments.saturation.value = preset.Saturation;
			this._filterColorAdjustments.colorFilter.value = preset.ColorFilter;
			this._filterWhiteBalance.temperature.value = preset.Temperature;
			this._filterWhiteBalance.tint.value = preset.Tint;
		}

		// Token: 0x06006C81 RID: 27777 RVA: 0x002B6880 File Offset: 0x002B4A80
		private void ResetCameraFilterImmediate()
		{
			this.EnsureFilterRuntime();
			if (this._filterTransition != null)
			{
				base.StopCoroutine(this._filterTransition);
				this._filterTransition = null;
			}
			this._filterIndex = 0;
			this.ApplyFilterPreset(PhoneCameraCapture.FilterPresets[0]);
			this._filterVolume.weight = 0f;
			this.RestoreFilterPostProcessing();
			this.UpdateFilterUiImmediate();
			this.SetFilterUiRestState();
		}

		// Token: 0x06006C82 RID: 27778 RVA: 0x002B68E8 File Offset: 0x002B4AE8
		private void DisableCameraFilterImmediate()
		{
			if (this._filterTransition != null)
			{
				base.StopCoroutine(this._filterTransition);
				this._filterTransition = null;
			}
			if (this._filterVolume != null)
			{
				this._filterVolume.weight = 0f;
			}
			this._filterIndex = 0;
			if (this._filterColorAdjustments != null && this._filterWhiteBalance != null)
			{
				this.ApplyFilterPreset(PhoneCameraCapture.FilterPresets[0]);
			}
			this.RestoreFilterPostProcessing();
			this.UpdateFilterUiImmediate();
			this.SetFilterUiRestState();
			this._disableFilterAfterVideo = false;
		}

		// Token: 0x06006C83 RID: 27779 RVA: 0x002B697B File Offset: 0x002B4B7B
		private void CompleteDeferredFilterShutdown()
		{
			if (this._disableFilterAfterVideo && !this._cameraModeActive)
			{
				this.DisableCameraFilterImmediate();
			}
		}

		// Token: 0x06006C84 RID: 27780 RVA: 0x002B6994 File Offset: 0x002B4B94
		public void CapturePhoto()
		{
			if (!this._cameraModeActive || this._mediaMode != PhoneCameraCapture.CameraMediaMode.Photo || this._videoFinalizing)
			{
				return;
			}
			if (this._captureInProgress)
			{
				return;
			}
			if (this._mainCamera == null)
			{
				this._mainCamera = Camera.main;
			}
			if (this._mainCamera == null)
			{
				if (this.debugLogs)
				{
					Debug.LogWarning("[PhoneCameraCapture] Camera.main null, scatto annullato.");
				}
				return;
			}
			base.StartCoroutine(this.CaptureAtEndOfFrame());
		}

		// Token: 0x06006C85 RID: 27781 RVA: 0x002B6A0A File Offset: 0x002B4C0A
		private IEnumerator CaptureAtEndOfFrame()
		{
			this._captureInProgress = true;
			yield return new WaitForEndOfFrame();
			try
			{
				this.DoCapture();
				yield break;
			}
			finally
			{
				this._captureInProgress = false;
			}
			yield break;
		}

		// Token: 0x06006C86 RID: 27782 RVA: 0x002B6A1C File Offset: 0x002B4C1C
		private void DoCapture()
		{
			this.EnsureCaptureRT();
			bool flag;
			if (this._usingSceneOverlay && this.overlayUI != null)
			{
				flag = true;
				this.overlayUI.SetVisible(false);
			}
			else
			{
				flag = this._overlayRoot != null && this._overlayRoot.activeSelf;
				if (flag)
				{
					this._overlayRoot.SetActive(false);
				}
			}
			string text = this.ResolveBodyTag();
			bool flag2 = this.ResolveFaceInFrame();
			Texture2D texture2D = null;
			Texture2D texture2D2 = null;
			Sprite sprite = null;
			bool flag3 = false;
			try
			{
				try
				{
					this.EnsureFaceCensorOff();
					if (!this.RenderToRT())
					{
						return;
					}
					texture2D = this.ReadRTToTexture(this._captureRT);
					if (this.produceCensoredVersion && this.SetFaceCensorActive(true))
					{
						if (this.RenderToRT())
						{
							texture2D2 = this.ReadRTToTexture(this._captureRT);
						}
						this.SetFaceCensorActive(false);
					}
				}
				catch (Exception ex)
				{
					Debug.LogError("[PhoneCameraCapture] Cattura fallita: " + ex.Message);
					this.EnsureFaceCensorOff();
					return;
				}
				finally
				{
					if (flag)
					{
						if (this._usingSceneOverlay && this.overlayUI != null)
						{
							this.overlayUI.SetVisible(true);
						}
						else if (this._overlayRoot != null)
						{
							this._overlayRoot.SetActive(true);
						}
					}
				}
				if (!(texture2D == null))
				{
					byte[] array = texture2D.EncodeToPNG();
					string text2 = this.SavePngToGallery(array, text, flag2);
					if (texture2D2 != null && !string.IsNullOrEmpty(text2))
					{
						string text3 = PhoneCameraCapture.CensoredPathFor(text2);
						try
						{
							File.WriteAllBytes(text3, texture2D2.EncodeToPNG());
						}
						catch (Exception ex2)
						{
							Debug.LogError("[PhoneCameraCapture] Salvataggio versione censurata fallito: " + ex2.Message);
						}
					}
					texture2D.Apply(false, true);
					sprite = this.SpriteFromTexture(texture2D);
					if (sprite != null && !string.IsNullOrEmpty(text2))
					{
						sprite.name = text2;
					}
					flag3 = true;
					this.InsertIntoGalleryGrid(sprite);
					this.PlayShutterSound();
					base.StartCoroutine(this.FlashRoutine());
					if (sprite != null)
					{
						base.StartCoroutine(this.FlyThumbnailRoutine(sprite));
					}
					if (this.debugLogs)
					{
						Debug.Log("[PhoneCameraCapture] Foto scattata e salvata: " + text2, this);
					}
				}
			}
			finally
			{
				if (texture2D2 != null)
				{
					Object.Destroy(texture2D2);
				}
				if (!flag3)
				{
					if (sprite != null)
					{
						Object.Destroy(sprite);
					}
					if (texture2D != null)
					{
						Object.Destroy(texture2D);
					}
				}
			}
		}

		// Token: 0x06006C87 RID: 27783 RVA: 0x002B6CD4 File Offset: 0x002B4ED4
		private bool RenderToRT()
		{
			return this.RenderCameraTo(this._captureRT);
		}

		// Token: 0x06006C88 RID: 27784 RVA: 0x002B6CE4 File Offset: 0x002B4EE4
		private bool RenderCameraTo(RenderTexture destination)
		{
			if (this._mainCamera == null || destination == null)
			{
				return false;
			}
			bool flag;
			using (StreamerCensorRenderFeature.BeginCapture())
			{
				int cullingMask = this._mainCamera.cullingMask;
				RenderTexture targetTexture = this._mainCamera.targetTexture;
				try
				{
					this._mainCamera.targetTexture = destination;
					UniversalRenderPipeline.SingleCameraRequest singleCameraRequest = new UniversalRenderPipeline.SingleCameraRequest();
					if (RenderPipeline.SupportsRenderRequest<UniversalRenderPipeline.SingleCameraRequest>(this._mainCamera, singleCameraRequest))
					{
						singleCameraRequest.destination = destination;
						RenderPipeline.SubmitRenderRequest<UniversalRenderPipeline.SingleCameraRequest>(this._mainCamera, singleCameraRequest);
					}
					else
					{
						this._mainCamera.Render();
					}
					flag = true;
				}
				catch (Exception ex)
				{
					Debug.LogError("[PhoneCameraCapture] Render fallito: " + ex.Message);
					flag = false;
				}
				finally
				{
					this._mainCamera.cullingMask = cullingMask;
					this._mainCamera.targetTexture = targetTexture;
				}
			}
			return flag;
		}

		// Token: 0x06006C89 RID: 27785 RVA: 0x002B6DD8 File Offset: 0x002B4FD8
		public static string CensoredPathFor(string normalPath)
		{
			if (string.IsNullOrEmpty(normalPath))
			{
				return normalPath;
			}
			string directoryName = Path.GetDirectoryName(normalPath);
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(normalPath);
			string extension = Path.GetExtension(normalPath);
			string text = fileNameWithoutExtension + ".censored" + extension;
			if (!string.IsNullOrEmpty(directoryName))
			{
				return Path.Combine(directoryName, text);
			}
			return text;
		}

		// Token: 0x06006C8A RID: 27786 RVA: 0x002B6E20 File Offset: 0x002B5020
		private void ResolveFaceCensor()
		{
			if (this._faceCensorResolved)
			{
				return;
			}
			this._faceCensorResolved = true;
			if (this.faceCensor == null)
			{
				Transform transform = this.ResolvePlayerHeadBone();
				if (transform != null)
				{
					Transform transform2 = PhoneCameraCapture.FindChildByName(transform, this.faceCensorObjectName);
					if (transform2 != null)
					{
						this.faceCensor = transform2.gameObject;
					}
				}
			}
			if (this.faceCensor == null && this.debugLogs)
			{
				Debug.LogWarning("[PhoneCameraCapture] FaceCensor non trovato: le foto non avranno la versione censurata. Assegna 'faceCensor' o verifica il nome.", this);
			}
			if (this.faceCensor != null && this.faceCensor.activeSelf)
			{
				this.faceCensor.SetActive(false);
			}
		}

		// Token: 0x06006C8B RID: 27787 RVA: 0x002B6EC8 File Offset: 0x002B50C8
		private Transform ResolvePlayerHeadBone()
		{
			if (this._frameTagger == null)
			{
				this._frameTagger = base.GetComponentInParent<SelfieFrameTagger>() ?? Object.FindObjectOfType<SelfieFrameTagger>();
			}
			Animator animator = ((this._frameTagger != null) ? this._frameTagger.GetComponentInParent<Animator>() : null);
			if (animator == null)
			{
				animator = Object.FindObjectOfType<Animator>();
			}
			if (animator != null && animator.isHuman)
			{
				return animator.GetBoneTransform(HumanBodyBones.Head);
			}
			return null;
		}

		// Token: 0x06006C8C RID: 27788 RVA: 0x002B6F40 File Offset: 0x002B5140
		private static Transform FindChildByName(Transform root, string name)
		{
			if (root == null || string.IsNullOrEmpty(name))
			{
				return null;
			}
			foreach (Transform transform in root.GetComponentsInChildren<Transform>(true))
			{
				if (transform.name == name)
				{
					return transform;
				}
			}
			return null;
		}

		// Token: 0x06006C8D RID: 27789 RVA: 0x002B6F8B File Offset: 0x002B518B
		private bool SetFaceCensorActive(bool active)
		{
			if (!this._faceCensorResolved)
			{
				this.ResolveFaceCensor();
			}
			if (this.faceCensor == null)
			{
				return false;
			}
			this.faceCensor.SetActive(active);
			return true;
		}

		// Token: 0x06006C8E RID: 27790 RVA: 0x002B6FB8 File Offset: 0x002B51B8
		private void EnsureFaceCensorOff()
		{
			if (!this._faceCensorResolved)
			{
				this.ResolveFaceCensor();
			}
			if (this.faceCensor != null && this.faceCensor.activeSelf)
			{
				this.faceCensor.SetActive(false);
			}
		}

		// Token: 0x06006C8F RID: 27791 RVA: 0x002B6FF0 File Offset: 0x002B51F0
		private Texture2D ReadRTToTexture(RenderTexture rt)
		{
			RenderTexture active = RenderTexture.active;
			RenderTexture renderTexture = null;
			Texture2D texture2D = null;
			bool flag = false;
			Texture2D texture2D2;
			try
			{
				renderTexture = RenderTexture.GetTemporary(rt.width, rt.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
				Graphics.Blit(rt, renderTexture);
				RenderTexture.active = renderTexture;
				texture2D = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false, false);
				texture2D.ReadPixels(new Rect(0f, 0f, (float)rt.width, (float)rt.height), 0, 0);
				texture2D.Apply();
				flag = true;
				texture2D2 = texture2D;
			}
			finally
			{
				RenderTexture.active = active;
				if (renderTexture != null)
				{
					RenderTexture.ReleaseTemporary(renderTexture);
				}
				if (!flag && texture2D != null)
				{
					Object.Destroy(texture2D);
				}
			}
			return texture2D2;
		}

		// Token: 0x06006C90 RID: 27792 RVA: 0x002B70AC File Offset: 0x002B52AC
		private void EnsureCaptureRT()
		{
			if (this._captureRT != null)
			{
				return;
			}
			int num = Mathf.Max(256, this.captureWidth);
			int num2 = Mathf.Max(256, this.captureHeight);
			this._captureRT = new RenderTexture(num, num2, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB)
			{
				name = "PhoneCameraCapture RT",
				antiAliasing = Mathf.Max(1, this.captureMsaa)
			};
			this._captureRT.Create();
		}

		// Token: 0x06006C91 RID: 27793 RVA: 0x002B7124 File Offset: 0x002B5324
		private void ApplyRecordingPreferences()
		{
			if (this._videoRecordingRequested || this._videoFinalizing)
			{
				return;
			}
			int num = Mathf.Clamp(GameSettingsService.PhoneVideoHeight, 360, 2160);
			this.videoHeight = num;
			this.videoWidth = Mathf.RoundToInt((float)num * 1.7777778f);
			this.videoFrameRate = Mathf.Clamp(GameSettingsService.PhoneVideoFps, 12, 60);
		}

		// Token: 0x06006C92 RID: 27794 RVA: 0x002B7188 File Offset: 0x002B5388
		public void StartVideoRecording()
		{
			if (!this._cameraModeActive || this._mediaMode != PhoneCameraCapture.CameraMediaMode.Video || this._videoRecordingRequested || this._videoFinalizing)
			{
				return;
			}
			if (this._mainCamera == null)
			{
				this._mainCamera = Camera.main;
			}
			if (this._mainCamera == null)
			{
				this.ShowVideoStatus("NO CAMERA", 2f);
				return;
			}
			try
			{
				this.ApplyRecordingPreferences();
				this.EnsureFaceCensorOff();
				this.EnsureVideoResources();
				this.ClearQueuedAudio();
				this._videoFinalPath = PhoneVideoStorage.CreateVideoPath();
				this._videoTemporaryPath = this._videoFinalPath + ".recording";
				this._videoThumbnailPath = PhoneVideoStorage.ThumbnailPathFor(this._videoFinalPath);
				this._videoThumbnailJpeg = null;
				this._lastEncodedVideoJpeg = null;
				this._videoDroppedCaptures = 0;
				Interlocked.Exchange(ref this._pendingVideoReadbacks, 0);
				this.StopVideoFrameEncoder();
				if (SystemInfo.supportsAsyncGPUReadback)
				{
					this._videoFrameEncoder = new PhoneVideoFrameEncoder(this._videoRT.width, this._videoRT.height, Mathf.Clamp(this.videoJpegQuality, 35, 95), Mathf.Clamp(this.videoEncoderQueueDepth, 1, 6));
				}
				this._audioCapture = null;
				this._videoHasAudio = false;
				if (this.recordGameAudio)
				{
					AudioListener audioListener = Object.FindObjectOfType<AudioListener>(true);
					if (audioListener != null)
					{
						this._audioCapture = audioListener.GetComponent<PhoneVideoAudioCapture>();
						if (this._audioCapture == null)
						{
							this._audioCapture = audioListener.gameObject.AddComponent<PhoneVideoAudioCapture>();
						}
						this._videoHasAudio = true;
					}
				}
				this._aviWriter = new RuntimeMjpegAviWriter(this._videoTemporaryPath, Mathf.Max(320, this.videoWidth), Mathf.Max(180, this.videoHeight), Mathf.Clamp(this.videoFrameRate, 12, 60), this._videoHasAudio, AudioSettings.outputSampleRate, 2);
				this._acceptAudio = this._videoHasAudio;
				if (this._audioCapture != null)
				{
					this._audioCapture.Begin(new Action<float[], int>(this.QueueMixedAudio));
				}
				this._videoStartedAt = Time.realtimeSinceStartupAsDouble;
				this._videoStoppedAt = this._videoStartedAt;
				this._videoRecordingRequested = true;
				this._videoFinalizing = false;
				this._acceptVideoReadbacks = true;
				this._videoStatusOverride = null;
				this.AnimateRecordingShutter(true);
				this.PlayVideoUiSound(this.videoRecordingStartClip);
				this._videoRoutine = base.StartCoroutine(this.VideoRecordingRoutine());
				if (this.debugLogs)
				{
					Debug.Log("[PhoneCameraCapture] Video REC: " + this._videoFinalPath, this);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("[PhoneCameraCapture] Impossibile avviare il video: " + ex.Message, this);
				this.AbortVideoRecording();
				this.ShowVideoStatus("REC ERROR", 2.5f);
			}
		}

		// Token: 0x06006C93 RID: 27795 RVA: 0x002B7444 File Offset: 0x002B5644
		public void StopVideoRecording()
		{
			if (!this._videoRecordingRequested)
			{
				bool videoFinalizing = this._videoFinalizing;
				return;
			}
			this._videoRecordingRequested = false;
			this._videoFinalizing = true;
			this._videoStoppedAt = Time.realtimeSinceStartupAsDouble;
			this._acceptAudio = false;
			if (this._audioCapture != null)
			{
				this._audioCapture.End();
			}
			this.PlayVideoUiSound(this.videoRecordingStopClip);
			this.ShowVideoStatus("SAVING...", 10f);
			this.AnimateRecordingShutter(false);
		}

		// Token: 0x06006C94 RID: 27796 RVA: 0x002B74BF File Offset: 0x002B56BF
		private IEnumerator VideoRecordingRoutine()
		{
			WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();
			double nextCaptureAt = this._videoStartedAt;
			bool failed = false;
			int fps = Mathf.Clamp(this.videoFrameRate, 12, 60);
			double frameInterval = 1.0 / (double)fps;
			while (this._videoRecordingRequested)
			{
				yield return endOfFrame;
				this.DrainEncodedVideoFrames();
				this.DrainQueuedAudio();
				double realtimeSinceStartupAsDouble = Time.realtimeSinceStartupAsDouble;
				if (this._aviWriter == null)
				{
					failed = true;
					break;
				}
				if (this._videoFrameEncoder != null && !string.IsNullOrEmpty(this._videoFrameEncoder.Error))
				{
					Debug.LogError("[PhoneCameraCapture] Encoder video: " + this._videoFrameEncoder.Error, this);
					failed = true;
					break;
				}
				if (realtimeSinceStartupAsDouble + 0.0001 >= nextCaptureAt)
				{
					try
					{
						int num = Math.Max(0, (int)Math.Floor((realtimeSinceStartupAsDouble - this._videoStartedAt) * (double)fps));
						bool flag;
						if (this._videoFrameEncoder != null)
						{
							flag = this.ScheduleAsyncVideoFrame(num);
						}
						else
						{
							byte[] array;
							flag = this.CaptureVideoJpeg(num, out array);
							if (flag)
							{
								this.CommitEncodedVideoFrame(num, array);
							}
						}
						if (!flag)
						{
							this._videoDroppedCaptures++;
						}
					}
					catch (Exception ex)
					{
						Debug.LogError("[PhoneCameraCapture] Errore durante REC: " + ex.Message, this);
						failed = true;
						break;
					}
					nextCaptureAt += frameInterval;
					if (nextCaptureAt < realtimeSinceStartupAsDouble - frameInterval)
					{
						nextCaptureAt = realtimeSinceStartupAsDouble + frameInterval;
					}
				}
				float num2 = (float)(realtimeSinceStartupAsDouble - this._videoStartedAt);
				if (num2 >= Mathf.Max(1f, this.maxVideoDurationSeconds) || this._aviWriter.LengthBytes >= 1800000000L)
				{
					this._videoRecordingRequested = false;
					this._videoFinalizing = true;
					this._videoStoppedAt = realtimeSinceStartupAsDouble;
					this._acceptAudio = false;
					if (this._audioCapture != null)
					{
						this._audioCapture.End();
					}
					this.PlayVideoUiSound(this.videoRecordingStopClip);
					this.ShowVideoStatus((num2 >= this.maxVideoDurationSeconds) ? "MAX TIME" : "FILE LIMIT", 2f);
					this.AnimateRecordingShutter(false);
				}
			}
			if (this._videoStoppedAt <= this._videoStartedAt)
			{
				this._videoStoppedAt = Time.realtimeSinceStartupAsDouble;
			}
			double drainDeadline = Time.realtimeSinceStartupAsDouble + 5.0;
			while (!failed && (Volatile.Read(ref this._pendingVideoReadbacks) > 0 || (this._videoFrameEncoder != null && this._videoFrameEncoder.HasPendingWork)) && Time.realtimeSinceStartupAsDouble < drainDeadline)
			{
				yield return null;
				this.DrainEncodedVideoFrames();
				this.DrainQueuedAudio();
				if (this._videoFrameEncoder != null && !string.IsNullOrEmpty(this._videoFrameEncoder.Error))
				{
					Debug.LogError("[PhoneCameraCapture] Encoder video: " + this._videoFrameEncoder.Error, this);
					failed = true;
				}
			}
			this._acceptVideoReadbacks = false;
			this.StopVideoFrameEncoder();
			this.DrainQueuedAudio();
			if (!failed && this._aviWriter != null && this._aviWriter.VideoFrameCount == 0)
			{
				byte[] array2;
				if (this.CaptureVideoJpeg(0, out array2))
				{
					this.CommitEncodedVideoFrame(0, array2);
				}
				else
				{
					failed = true;
				}
			}
			if (!failed)
			{
				this.PadVideoToRecordingDuration();
			}
			this.FinishVideoRecording(!failed, true);
			this._videoRoutine = null;
			yield break;
		}

		// Token: 0x06006C95 RID: 27797 RVA: 0x002B74D0 File Offset: 0x002B56D0
		private bool ScheduleAsyncVideoFrame(int targetFrameIndex)
		{
			if (this._videoFrameEncoder == null || !this._acceptVideoReadbacks)
			{
				return false;
			}
			if (Volatile.Read(ref this._pendingVideoReadbacks) + this._videoFrameEncoder.Backlog >= Mathf.Clamp(this.videoEncoderQueueDepth, 1, 6))
			{
				return false;
			}
			if (!this.RenderCameraTo(this._videoRT))
			{
				return false;
			}
			Interlocked.Increment(ref this._pendingVideoReadbacks);
			AsyncGPUReadback.Request(this._videoRT, 0, TextureFormat.RGB24, delegate(AsyncGPUReadbackRequest request)
			{
				this.CompleteVideoReadback(request, targetFrameIndex);
			});
			return true;
		}

		// Token: 0x06006C96 RID: 27798 RVA: 0x002B7564 File Offset: 0x002B5764
		private void CompleteVideoReadback(AsyncGPUReadbackRequest request, int targetFrameIndex)
		{
			Interlocked.Decrement(ref this._pendingVideoReadbacks);
			if (!this._acceptVideoReadbacks || request.hasError || this._videoFrameEncoder == null)
			{
				Interlocked.Increment(ref this._videoDroppedCaptures);
				return;
			}
			try
			{
				byte[] array = request.GetData<byte>(0).ToArray();
				this.BurnRecordingTimerIntoFrame(array, targetFrameIndex);
				if (!this._videoFrameEncoder.TryEnqueue(targetFrameIndex, array))
				{
					Interlocked.Increment(ref this._videoDroppedCaptures);
				}
			}
			catch (Exception ex)
			{
				Interlocked.Increment(ref this._videoDroppedCaptures);
				if (this.debugLogs)
				{
					Debug.LogWarning("[PhoneCameraCapture] Readback video scartato: " + ex.Message, this);
				}
			}
		}

		// Token: 0x06006C97 RID: 27799 RVA: 0x002B761C File Offset: 0x002B581C
		private void DrainEncodedVideoFrames()
		{
			if (this._videoFrameEncoder == null)
			{
				return;
			}
			PhoneVideoFrameEncoder.EncodedFrame encodedFrame;
			while (this._videoFrameEncoder.TryDequeue(out encodedFrame))
			{
				this.CommitEncodedVideoFrame(encodedFrame.targetFrameIndex, encodedFrame.jpeg);
			}
		}

		// Token: 0x06006C98 RID: 27800 RVA: 0x002B7658 File Offset: 0x002B5858
		private void CommitEncodedVideoFrame(int targetFrameIndex, byte[] jpeg)
		{
			if (this._aviWriter == null || jpeg == null || jpeg.Length <= 4)
			{
				return;
			}
			int num = Mathf.Clamp(this.videoFrameRate, 12, 60);
			int num2 = Math.Min(Math.Max(0, targetFrameIndex), this._aviWriter.VideoFrameCount + num * 2);
			if (num2 < this._aviWriter.VideoFrameCount)
			{
				return;
			}
			byte[] array = this._lastEncodedVideoJpeg ?? jpeg;
			while (this._aviWriter.VideoFrameCount < num2)
			{
				this._aviWriter.WriteVideoFrame(array);
			}
			this._aviWriter.WriteVideoFrame(jpeg);
			this._lastEncodedVideoJpeg = jpeg;
			if (this._videoThumbnailJpeg == null)
			{
				this._videoThumbnailJpeg = jpeg;
				File.WriteAllBytes(this._videoThumbnailPath, jpeg);
			}
		}

		// Token: 0x06006C99 RID: 27801 RVA: 0x002B7708 File Offset: 0x002B5908
		private void PadVideoToRecordingDuration()
		{
			if (this._aviWriter == null || this._lastEncodedVideoJpeg == null)
			{
				return;
			}
			int num = Mathf.Clamp(this.videoFrameRate, 12, 60);
			int num2 = Math.Max(1, (int)Math.Ceiling(Math.Max(0.0, this._videoStoppedAt - this._videoStartedAt) * (double)num));
			num2 = Math.Min(num2, this._aviWriter.VideoFrameCount + num * 2);
			while (this._aviWriter.VideoFrameCount < num2)
			{
				this._aviWriter.WriteVideoFrame(this._lastEncodedVideoJpeg);
			}
		}

		// Token: 0x06006C9A RID: 27802 RVA: 0x002B7798 File Offset: 0x002B5998
		private void StopVideoFrameEncoder()
		{
			if (this._videoFrameEncoder == null)
			{
				return;
			}
			this._videoFrameEncoder.Complete();
			this._videoFrameEncoder.Dispose();
			this.DrainEncodedVideoFrames();
			this._videoFrameEncoder = null;
		}

		// Token: 0x06006C9B RID: 27803 RVA: 0x002B77C8 File Offset: 0x002B59C8
		private bool CaptureVideoJpeg(int targetFrameIndex, out byte[] jpeg)
		{
			jpeg = null;
			this.EnsureVideoResources();
			if (!this.RenderCameraTo(this._videoRT))
			{
				return false;
			}
			RenderTexture active = RenderTexture.active;
			bool flag;
			try
			{
				RenderTexture.active = this._videoRT;
				this._videoFrameTexture.ReadPixels(new Rect(0f, 0f, (float)this._videoRT.width, (float)this._videoRT.height), 0, 0, false);
				this._videoFrameTexture.Apply(false, false);
				if (this.burnRecordingTimerIntoVideo)
				{
					byte[] array = this._videoFrameTexture.GetRawTextureData<byte>().ToArray();
					this.BurnRecordingTimerIntoFrame(array, targetFrameIndex);
					this._videoFrameTexture.LoadRawTextureData(array);
					this._videoFrameTexture.Apply(false, false);
				}
				jpeg = this._videoFrameTexture.EncodeToJPG(Mathf.Clamp(this.videoJpegQuality, 35, 95));
				flag = jpeg != null && jpeg.Length > 4;
			}
			finally
			{
				RenderTexture.active = active;
			}
			return flag;
		}

		// Token: 0x06006C9C RID: 27804 RVA: 0x002B78C4 File Offset: 0x002B5AC4
		private void BurnRecordingTimerIntoFrame(byte[] rgb24, int targetFrameIndex)
		{
			if (!this.burnRecordingTimerIntoVideo || this._videoRT == null)
			{
				return;
			}
			PhoneVideoTimerBurnIn.Draw(rgb24, this._videoRT.width, this._videoRT.height, targetFrameIndex, Mathf.Clamp(this.videoFrameRate, 12, 60));
		}

		// Token: 0x06006C9D RID: 27805 RVA: 0x002B7914 File Offset: 0x002B5B14
		private void EnsureVideoResources()
		{
			int num = Mathf.Max(320, this.videoWidth);
			int num2 = Mathf.Max(180, this.videoHeight);
			if ((num & 1) != 0)
			{
				num--;
			}
			if ((num2 & 1) != 0)
			{
				num2--;
			}
			if (this._videoRT != null && (this._videoRT.width != num || this._videoRT.height != num2))
			{
				this._videoRT.Release();
				Object.Destroy(this._videoRT);
				this._videoRT = null;
			}
			if (this._videoFrameTexture != null && (this._videoFrameTexture.width != num || this._videoFrameTexture.height != num2))
			{
				Object.Destroy(this._videoFrameTexture);
				this._videoFrameTexture = null;
			}
			if (this._videoRT == null)
			{
				this._videoRT = new RenderTexture(num, num2, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB)
				{
					name = "Phone Video Recording RT",
					antiAliasing = 1,
					useMipMap = false,
					autoGenerateMips = false
				};
				this._videoRT.Create();
			}
			if (this._videoFrameTexture == null)
			{
				this._videoFrameTexture = new Texture2D(num, num2, TextureFormat.RGB24, false, false)
				{
					name = "Phone Video Frame Buffer"
				};
			}
		}

		// Token: 0x06006C9E RID: 27806 RVA: 0x002B7A4C File Offset: 0x002B5C4C
		private void QueueMixedAudio(float[] data, int inputChannels)
		{
			if (!this._acceptAudio || data == null || data.Length == 0 || inputChannels <= 0)
			{
				return;
			}
			if (Volatile.Read(ref this._queuedAudioBytes) > 16777216)
			{
				return;
			}
			int num = data.Length / inputChannels;
			byte[] array = new byte[num * 4];
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				int num3 = i * inputChannels;
				float num4 = data[num3];
				float num5 = ((inputChannels > 1) ? data[num3 + 1] : num4);
				PhoneCameraCapture.WritePcm16(array, ref num2, num4);
				PhoneCameraCapture.WritePcm16(array, ref num2, num5);
			}
			this._audioQueue.Enqueue(array);
			Interlocked.Add(ref this._queuedAudioBytes, array.Length);
		}

		// Token: 0x06006C9F RID: 27807 RVA: 0x002B7AEC File Offset: 0x002B5CEC
		private static void WritePcm16(byte[] target, ref int index, float sample)
		{
			if (sample > 1f)
			{
				sample = 1f;
			}
			else if (sample < -1f)
			{
				sample = -1f;
			}
			short num = (short)Math.Round((double)(sample * ((sample < 0f) ? 32768f : 32767f)));
			int num2 = index;
			index = num2 + 1;
			target[num2] = (byte)(num & 255);
			num2 = index;
			index = num2 + 1;
			target[num2] = (byte)((num >> 8) & 255);
		}

		// Token: 0x06006CA0 RID: 27808 RVA: 0x002B7B60 File Offset: 0x002B5D60
		private void DrainQueuedAudio()
		{
			byte[] array;
			while (this._audioQueue.TryDequeue(out array))
			{
				Interlocked.Add(ref this._queuedAudioBytes, -array.Length);
				if (this._aviWriter != null)
				{
					this._aviWriter.WriteAudioPcm(array);
				}
			}
		}

		// Token: 0x06006CA1 RID: 27809 RVA: 0x002B7BA4 File Offset: 0x002B5DA4
		private void ClearQueuedAudio()
		{
			byte[] array;
			while (this._audioQueue.TryDequeue(out array))
			{
			}
			Interlocked.Exchange(ref this._queuedAudioBytes, 0);
		}

		// Token: 0x06006CA2 RID: 27810 RVA: 0x002B7BD0 File Offset: 0x002B5DD0
		private void FinishVideoRecording(bool keepFile, bool refreshGallery = true)
		{
			this._acceptVideoReadbacks = false;
			this.StopVideoFrameEncoder();
			this._acceptAudio = false;
			if (this._audioCapture != null)
			{
				this._audioCapture.End();
			}
			this.DrainQueuedAudio();
			try
			{
				if (!keepFile || this._aviWriter == null || this._aviWriter.VideoFrameCount == 0)
				{
					this.AbortVideoRecording();
					this.ShowVideoStatus("REC FAILED", 2.2f);
				}
				else
				{
					int videoFrameCount = this._aviWriter.VideoFrameCount;
					int num = Mathf.Clamp(this.videoFrameRate, 12, 60);
					this._aviWriter.FinalizeFile();
					this._aviWriter = null;
					if (File.Exists(this._videoFinalPath))
					{
						File.Delete(this._videoFinalPath);
					}
					File.Move(this._videoTemporaryPath, this._videoFinalPath);
					if (!File.Exists(this._videoThumbnailPath) && this._videoThumbnailJpeg != null)
					{
						File.WriteAllBytes(this._videoThumbnailPath, this._videoThumbnailJpeg);
					}
					PhoneVideoStorage.SaveMetadata(this._videoFinalPath, new PhoneVideoMetadata
					{
						createdUtc = DateTime.UtcNow.ToString("o"),
						durationSeconds = (float)videoFrameCount / (float)num,
						width = ((this._videoRT != null) ? this._videoRT.width : this.videoWidth),
						height = ((this._videoRT != null) ? this._videoRT.height : this.videoHeight),
						frameRate = num,
						hasAudio = this._videoHasAudio
					});
					this.ShowVideoStatus("SAVED", 1.8f);
					if (this.debugLogs)
					{
						Debug.Log("[PhoneCameraCapture] Video salvato: " + this._videoFinalPath, this);
					}
					if (refreshGallery)
					{
						PhotoGallery photoGallery = Object.FindObjectOfType<PhotoGallery>(true);
						if (photoGallery != null)
						{
							photoGallery.RefreshVideos();
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("[PhoneCameraCapture] Finalizzazione video fallita: " + ex.Message, this);
				this.AbortVideoRecording();
				this.ShowVideoStatus("SAVE ERROR", 2.5f);
			}
			finally
			{
				this._videoRecordingRequested = false;
				this._videoFinalizing = false;
				this._audioCapture = null;
				this._videoThumbnailJpeg = null;
				this._lastEncodedVideoJpeg = null;
				Interlocked.Exchange(ref this._pendingVideoReadbacks, 0);
				this.ClearQueuedAudio();
				this.CompleteDeferredFilterShutdown();
			}
		}

		// Token: 0x06006CA3 RID: 27811 RVA: 0x002B7E44 File Offset: 0x002B6044
		private void AbortVideoRecording()
		{
			this._acceptVideoReadbacks = false;
			this.StopVideoFrameEncoder();
			this._acceptAudio = false;
			if (this._audioCapture != null)
			{
				this._audioCapture.End();
			}
			if (this._aviWriter != null)
			{
				try
				{
					this._aviWriter.Abort();
				}
				catch
				{
				}
				this._aviWriter = null;
			}
			PhoneCameraCapture.DeleteRecordingArtifact(this._videoTemporaryPath);
			PhoneCameraCapture.DeleteRecordingArtifact(this._videoThumbnailPath);
			this._videoRecordingRequested = false;
			this._videoFinalizing = false;
			this._audioCapture = null;
			this._videoThumbnailJpeg = null;
			this._lastEncodedVideoJpeg = null;
			Interlocked.Exchange(ref this._pendingVideoReadbacks, 0);
			this.ClearQueuedAudio();
			this.CompleteDeferredFilterShutdown();
		}

		// Token: 0x06006CA4 RID: 27812 RVA: 0x002B7F04 File Offset: 0x002B6104
		private void SyncTripodSpatialAudio()
		{
			if (!this._cameraModeActive || this._tripodActiveProvider == null || !this._tripodActiveProvider())
			{
				this.EndTripodSpatialAudio();
				return;
			}
			if (this._tripodSpatialAudioScope.IsActive)
			{
				if (Time.unscaledTime >= this._nextTripodAudioRefreshAt)
				{
					this._tripodSpatialAudioScope.Refresh();
					this._nextTripodAudioRefreshAt = Time.unscaledTime + Mathf.Max(0.1f, this.tripodAudioRefreshInterval);
				}
				return;
			}
			Transform transform = ((this._tripodAudioSubjectProvider != null) ? this._tripodAudioSubjectProvider() : null);
			if (transform == null)
			{
				return;
			}
			this._tripodSpatialAudioScope.Begin(transform, this.tripodAudioMinDistance, this.tripodAudioMaxDistance, new Func<AudioSource, bool>(this.IsPhoneUiAudioSource));
			this._nextTripodAudioRefreshAt = Time.unscaledTime + Mathf.Max(0.1f, this.tripodAudioRefreshInterval);
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[PhoneCameraCapture] Tripod POV: {0} ", this._tripodSpatialAudioScope.AdjustedSourceCount) + "sorgenti di Ciel convertite in audio 3D.", this);
			}
		}

		// Token: 0x06006CA5 RID: 27813 RVA: 0x002B8010 File Offset: 0x002B6210
		private void EndTripodSpatialAudio()
		{
			if (this._tripodSpatialAudioScope.IsActive)
			{
				this._tripodSpatialAudioScope.End();
			}
		}

		// Token: 0x06006CA6 RID: 27814 RVA: 0x002B802C File Offset: 0x002B622C
		private bool IsPhoneUiAudioSource(AudioSource source)
		{
			return source == null || source == this.shutterAudio || (this.phoneCanvas != null && source.transform.IsChildOf(this.phoneCanvas.transform));
		}

		// Token: 0x06006CA7 RID: 27815 RVA: 0x002B807C File Offset: 0x002B627C
		private static void DeleteRecordingArtifact(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return;
			}
			try
			{
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
			catch
			{
			}
		}

		// Token: 0x06006CA8 RID: 27816 RVA: 0x002B80B8 File Offset: 0x002B62B8
		private void ShowVideoStatus(string status, float duration)
		{
			this._videoStatusOverride = status;
			this._videoStatusUntil = Time.unscaledTime + duration;
		}

		// Token: 0x06006CA9 RID: 27817 RVA: 0x002B80D0 File Offset: 0x002B62D0
		private void ResolveToolkitGallery()
		{
			this._toolkitCamController = Object.FindObjectOfType<CameraController>(true);
			if (this._toolkitCamController != null)
			{
				this._galleryImagePrefab = this._toolkitCamController.imagePrefab;
				this._galleryImageParent = this._toolkitCamController.imageParent;
			}
			this._galleryFolder = PhonePhotoStorage.ImagesFolder;
		}

		// Token: 0x06006CAA RID: 27818 RVA: 0x002B8124 File Offset: 0x002B6324
		private string ResolveBodyTag()
		{
			if (this._frameTagger == null)
			{
				this._frameTagger = base.GetComponentInParent<SelfieFrameTagger>() ?? Object.FindObjectOfType<SelfieFrameTagger>();
			}
			Camera camera = ((this._mainCamera != null) ? this._mainCamera : Camera.main);
			if (this._frameTagger == null || camera == null)
			{
				return "none";
			}
			return this._frameTagger.ResolveTag(camera);
		}

		// Token: 0x06006CAB RID: 27819 RVA: 0x002B819C File Offset: 0x002B639C
		private bool ResolveFaceInFrame()
		{
			if (this._posePicker == null)
			{
				this._posePicker = base.GetComponent<SelfiePosePicker>();
			}
			if (this._posePicker != null && this._posePicker.CurrentPoseHidesFace)
			{
				return false;
			}
			if (this._frameTagger == null)
			{
				this._frameTagger = base.GetComponentInParent<SelfieFrameTagger>() ?? Object.FindObjectOfType<SelfieFrameTagger>();
			}
			Camera camera = ((this._mainCamera != null) ? this._mainCamera : Camera.main);
			return !(this._frameTagger == null) && !(camera == null) && this._frameTagger.IsFaceInFrame(camera);
		}

		// Token: 0x06006CAC RID: 27820 RVA: 0x002B8244 File Offset: 0x002B6444
		private string SavePngToGallery(byte[] png, string bodyTag = null, bool faceInFrame = false)
		{
			string text5;
			try
			{
				if (!Directory.Exists(this._galleryFolder))
				{
					Directory.CreateDirectory(this._galleryFolder);
				}
				int num = Random.Range(100000, 999999);
				string text = PhoneCameraCapture.SanitizeTag(bodyTag);
				string text2 = (faceInFrame ? "_face" : "");
				string text3 = (string.IsNullOrEmpty(text) ? string.Format("IMG_{0}{1}.png", num, text2) : string.Format("IMG_{0}_{1}{2}.png", text, num, text2));
				string text4 = Path.Combine(this._galleryFolder, text3);
				File.WriteAllBytes(text4, png);
				text5 = text4;
			}
			catch (Exception ex)
			{
				Debug.LogError("[PhoneCameraCapture] Salvataggio PNG fallito: " + ex.Message);
				text5 = null;
			}
			return text5;
		}

		// Token: 0x06006CAD RID: 27821 RVA: 0x002B8308 File Offset: 0x002B6508
		private static string SanitizeTag(string tag)
		{
			if (string.IsNullOrEmpty(tag))
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder(tag.Length);
			foreach (char c in tag)
			{
				if (c >= 'A' && c <= 'Z')
				{
					stringBuilder.Append(c + ' ');
				}
				else if (c >= 'a' && c <= 'z')
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06006CAE RID: 27822 RVA: 0x002B837C File Offset: 0x002B657C
		private void InsertIntoGalleryGrid(Sprite sprite)
		{
			if (this._galleryImagePrefab == null || this._galleryImageParent == null || sprite == null)
			{
				return;
			}
			GameObject gameObject = Object.Instantiate<GameObject>(this._galleryImagePrefab);
			gameObject.transform.SetParent(this._galleryImageParent, false);
			gameObject.transform.SetAsFirstSibling();
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localEulerAngles = Vector3.zero;
			Image image = ((gameObject.transform.childCount > 0) ? gameObject.transform.GetChild(0).GetComponent<Image>() : gameObject.GetComponentInChildren<Image>());
			if (image != null)
			{
				image.sprite = sprite;
			}
			StreamerCensorImage.Bind(image, false);
			PhonePhotoGalleryItem phonePhotoGalleryItem = gameObject.GetComponent<PhonePhotoGalleryItem>();
			if (phonePhotoGalleryItem == null)
			{
				phonePhotoGalleryItem = gameObject.AddComponent<PhonePhotoGalleryItem>();
			}
			phonePhotoGalleryItem.runtimeSprite = sprite;
		}

		// Token: 0x06006CAF RID: 27823 RVA: 0x002B8458 File Offset: 0x002B6658
		private Sprite SpriteFromTexture(Texture2D tex)
		{
			if (tex == null)
			{
				return null;
			}
			return Sprite.Create(tex, new Rect(0f, 0f, (float)tex.width, (float)tex.height), new Vector2(0.5f, 0.5f), 100f);
		}

		// Token: 0x06006CB0 RID: 27824 RVA: 0x002B84A8 File Offset: 0x002B66A8
		private void UpdateZoomLabel()
		{
			if (this._zoomLabel == null || this._zoomLabelProvider == null)
			{
				return;
			}
			float num = this._zoomLabelProvider();
			if (this._displayedZoomTarget == this._zoomLabel && this._displayedZoom == num)
			{
				return;
			}
			this._displayedZoomTarget = this._zoomLabel;
			this._displayedZoom = num;
			this._zoomLabel.text = string.Format("{0:0.#}x", num);
		}

		// Token: 0x06006CB1 RID: 27825 RVA: 0x002B8524 File Offset: 0x002B6724
		private void EnsureMediaModeUi()
		{
			if (this._shutterFill != null || this._overlayRoot == null)
			{
				return;
			}
			RectTransform rectTransform = this._overlayRoot.transform as RectTransform;
			if (rectTransform == null)
			{
				return;
			}
			this._displayedVideoSeconds = (this._displayedVideoHeight = (this._displayedVideoFrameRate = -1));
			this._displayedVideoAudioState = -1;
			if (this._shutterButton == null && this.overlayUI != null)
			{
				this._shutterButton = this.overlayUI.shutterButton;
			}
			if (this._shutterButton == null)
			{
				this._shutterButton = this._overlayRoot.GetComponentInChildren<Button>(true);
			}
			if (this._shutterButton != null)
			{
				GameObject gameObject = new GameObject("MediaModeFill", new Type[]
				{
					typeof(RectTransform),
					typeof(CanvasRenderer),
					typeof(Image)
				});
				this._shutterFillRect = (RectTransform)gameObject.transform;
				this._shutterFillRect.SetParent(this._shutterButton.transform, false);
				RectTransform shutterFillRect = this._shutterFillRect;
				RectTransform shutterFillRect2 = this._shutterFillRect;
				Vector2 vector = new Vector2(0.5f, 0.5f);
				shutterFillRect2.anchorMax = vector;
				shutterFillRect.anchorMin = vector;
				this._shutterFillRect.pivot = new Vector2(0.5f, 0.5f);
				this._shutterFillRect.anchoredPosition = Vector2.zero;
				this._shutterFillRect.sizeDelta = new Vector2(78f, 78f);
				this._shutterFill = gameObject.GetComponent<Image>();
				this._shutterFill.sprite = PhoneCameraCapture.CircleSprite();
				this._shutterFill.color = PhoneCameraCapture.PhotoShutterColor;
				this._shutterFill.raycastTarget = false;
			}
			RectTransform rectTransform2 = PhoneCameraCapture.NewOverlayRect(rectTransform, "MediaModeStrip");
			PhoneCameraCapture.SetOverlayRect(rectTransform2, new Vector2(0.5f, 0f), new Vector2(0f, 310f), new Vector2(340f, 54f));
			this._videoModeLabel = PhoneCameraCapture.NewOverlayText(rectTransform2, "VideoMode", "VIDEO", 25, TextAnchor.MiddleCenter);
			LocalizedLegacyGameText.BindSource(this._videoModeLabel, "phone_story", this._videoModeLabel.text);
			PhoneCameraCapture.SetOverlayRect(this._videoModeLabel.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(-70f, 0f), new Vector2(120f, 44f));
			this._photoModeLabel = PhoneCameraCapture.NewOverlayText(rectTransform2, "PhotoMode", "PHOTOS", 25, TextAnchor.MiddleCenter);
			LocalizedLegacyGameText.BindSource(this._photoModeLabel, "phone_story", this._photoModeLabel.text);
			PhoneCameraCapture.SetOverlayRect(this._photoModeLabel.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(70f, 0f), new Vector2(120f, 44f));
			Image image = PhoneCameraCapture.NewOverlayImage(rectTransform2, "ModeUnderline", PhoneCameraCapture.CameraModeAccent);
			this._modeUnderline = image;
			PhoneCameraCapture.SetOverlayRect(image.rectTransform, new Vector2(0.5f, 0f), new Vector2(70f, 1f), new Vector2(64f, 4f));
			Text text = PhoneCameraCapture.NewOverlayText(rectTransform2, "ModeHint", "H", 17, TextAnchor.MiddleCenter);
			this._mediaModeKeyLabel = text;
			PhoneCameraCapture.SetOverlayRect(text.rectTransform, new Vector2(0.5f, 0f), new Vector2(0f, -18f), new Vector2(180f, 30f));
			Outline outline = text.gameObject.AddComponent<Outline>();
			outline.effectColor = new Color(0f, 0f, 0f, 0.8f);
			outline.effectDistance = new Vector2(1f, -1f);
			this._tripodButtonImage = PhoneCameraCapture.NewOverlayImage(rectTransform, "TripodButton", new Color(0f, 0f, 0f, 0.58f));
			this._tripodButtonImage.sprite = PhoneCameraCapture.UISprite();
			this._tripodButtonImage.type = Image.Type.Sliced;
			this._tripodButtonImage.raycastTarget = true;
			PhoneCameraCapture.SetOverlayRect(this._tripodButtonImage.rectTransform, new Vector2(1f, 0f), new Vector2(-190f, 92f), new Vector2(270f, 58f));
			this._tripodButton = this._tripodButtonImage.gameObject.AddComponent<Button>();
			this._tripodButton.targetGraphic = this._tripodButtonImage;
			this._tripodButton.onClick.AddListener(new UnityAction(this.RequestTripodToggle));
			ColorBlock colors = this._tripodButton.colors;
			colors.normalColor = Color.white;
			colors.highlightedColor = new Color(1f, 1f, 1f, 0.92f);
			colors.pressedColor = new Color(0.82f, 0.82f, 0.82f, 1f);
			colors.disabledColor = new Color(1f, 1f, 1f, 0.42f);
			colors.fadeDuration = 0.12f;
			this._tripodButton.colors = colors;
			this._tripodButtonLabel = PhoneCameraCapture.NewOverlayText(this._tripodButtonImage.rectTransform, "Label", "T   TRIPOD", 21, TextAnchor.MiddleCenter);
			PhoneCameraCapture.StretchFull(this._tripodButtonLabel.rectTransform);
			PhoneCameraCapture.AddHudTextOutline(this._tripodButtonLabel);
			GameObject gameObject2 = new GameObject("VideoOnlyHud", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasGroup)
			});
			RectTransform rectTransform3 = (RectTransform)gameObject2.transform;
			rectTransform3.SetParent(rectTransform, false);
			PhoneCameraCapture.StretchFull(rectTransform3);
			this._videoHudGroup = gameObject2.GetComponent<CanvasGroup>();
			this._videoHudGroup.alpha = 0f;
			this._videoHudGroup.blocksRaycasts = false;
			this._videoHudGroup.interactable = false;
			PhoneCameraCapture.CreateGridLine(rectTransform3, new Vector2(0.33333334f, 0.5f), new Vector2(2f, 1080f));
			PhoneCameraCapture.CreateGridLine(rectTransform3, new Vector2(0.6666667f, 0.5f), new Vector2(2f, 1080f));
			PhoneCameraCapture.CreateGridLine(rectTransform3, new Vector2(0.5f, 0.33333334f), new Vector2(1920f, 2f));
			PhoneCameraCapture.CreateGridLine(rectTransform3, new Vector2(0.5f, 0.6666667f), new Vector2(1920f, 2f));
			this._recordingBadge = PhoneCameraCapture.NewOverlayImage(rectTransform3, "RecordingTimer", new Color(PhoneCameraCapture.VideoShutterColor.r, PhoneCameraCapture.VideoShutterColor.g, PhoneCameraCapture.VideoShutterColor.b, 0.9f));
			this._recordingBadge.sprite = PhoneCameraCapture.UISprite();
			this._recordingBadge.type = Image.Type.Sliced;
			PhoneCameraCapture.SetOverlayRect(this._recordingBadge.rectTransform, new Vector2(1f, 1f), new Vector2(-402f, -62f), new Vector2(164f, 44f));
			this._videoTimerLabel = PhoneCameraCapture.NewOverlayText(this._recordingBadge.rectTransform, "Timer", "00:00:00", 23, TextAnchor.MiddleCenter);
			PhoneCameraCapture.StretchFull(this._videoTimerLabel.rectTransform);
			this._videoQualityLabel = PhoneCameraCapture.NewOverlayText(rectTransform3, "Quality", "", 20, TextAnchor.MiddleRight);
			PhoneCameraCapture.SetOverlayRect(this._videoQualityLabel.rectTransform, new Vector2(1f, 1f), new Vector2(-450f, -104f), new Vector2(260f, 30f));
			PhoneCameraCapture.AddHudTextOutline(this._videoQualityLabel);
			this._videoAudioLabel = PhoneCameraCapture.NewOverlayText(rectTransform3, "Audio", "", 17, TextAnchor.MiddleRight);
			PhoneCameraCapture.SetOverlayRect(this._videoAudioLabel.rectTransform, new Vector2(1f, 1f), new Vector2(-450f, -138f), new Vector2(260f, 26f));
			PhoneCameraCapture.AddHudTextOutline(this._videoAudioLabel);
			this.EnsureFilterUi(rectTransform);
			this.UpdateTripodUi();
		}

		// Token: 0x06006CB2 RID: 27826 RVA: 0x002B8D48 File Offset: 0x002B6F48
		private void EnsureFilterUi(RectTransform root)
		{
			if (this._filterPanel != null || root == null)
			{
				return;
			}
			this._filterPanel = PhoneCameraCapture.NewOverlayImage(root, "CameraFilter", new Color(0.015f, 0.018f, 0.025f, 0.64f));
			this._filterPanel.sprite = PhoneCameraCapture.UISprite();
			this._filterPanel.type = Image.Type.Sliced;
			this._filterPanel.raycastTarget = false;
			PhoneCameraCapture.SetOverlayRect(this._filterPanel.rectTransform, new Vector2(0f, 1f), new Vector2(166f, -168f), new Vector2(272f, 54f));
			this._filterPanelGroup = this._filterPanel.gameObject.AddComponent<CanvasGroup>();
			this._filterPanelGroup.blocksRaycasts = false;
			this._filterPanelGroup.interactable = false;
			Image image = PhoneCameraCapture.NewOverlayImage(this._filterPanel.rectTransform, "Key", new Color(1f, 1f, 1f, 0.13f));
			image.sprite = PhoneCameraCapture.UISprite();
			image.type = Image.Type.Sliced;
			PhoneCameraCapture.SetOverlayRect(image.rectTransform, new Vector2(0f, 0.5f), new Vector2(29f, 0f), new Vector2(40f, 36f));
			Text text = PhoneCameraCapture.NewOverlayText(image.rectTransform, "KeyLabel", "F", 18, TextAnchor.MiddleCenter);
			this._filterKeyLabel = text;
			PhoneCameraCapture.StretchFull(text.rectTransform);
			PhoneCameraCapture.AddHudTextOutline(text);
			this._filterAccent = PhoneCameraCapture.NewOverlayImage(this._filterPanel.rectTransform, "Accent", PhoneCameraCapture.FilterPresets[0].Accent);
			this._filterAccent.sprite = PhoneCameraCapture.UISprite();
			this._filterAccent.type = Image.Type.Sliced;
			PhoneCameraCapture.SetOverlayRect(this._filterAccent.rectTransform, new Vector2(0f, 0.5f), new Vector2(57f, 0f), new Vector2(4f, 30f));
			this._filterLabel = PhoneCameraCapture.NewOverlayText(this._filterPanel.rectTransform, "FilterName", PhoneCameraCapture.FilterPresets[0].Label, 19, TextAnchor.MiddleLeft);
			PhoneCameraCapture.SetOverlayRect(this._filterLabel.rectTransform, new Vector2(0f, 0.5f), new Vector2(164f, 0f), new Vector2(188f, 40f));
			PhoneCameraCapture.AddHudTextOutline(this._filterLabel);
			this.UpdateFilterUiImmediate();
			this.SetFilterUiRestState();
		}

		// Token: 0x06006CB3 RID: 27827 RVA: 0x002B8FD8 File Offset: 0x002B71D8
		private void UpdateFilterUiImmediate()
		{
			if (PhoneCameraCapture.FilterPresets.Length == 0)
			{
				return;
			}
			PhoneCameraCapture.CameraFilterPreset cameraFilterPreset = PhoneCameraCapture.FilterPresets[Mathf.Clamp(this._filterIndex, 0, PhoneCameraCapture.FilterPresets.Length - 1)];
			if (this._filterLabel != null)
			{
				LocalizedLegacyGameText.BindSource(this._filterLabel, "phone_story", cameraFilterPreset.Label).FitTranslationToRectangle();
				this._filterLabel.color = cameraFilterPreset.Accent;
			}
			if (this._filterAccent != null)
			{
				this._filterAccent.color = cameraFilterPreset.Accent;
			}
		}

		// Token: 0x06006CB4 RID: 27828 RVA: 0x002B9068 File Offset: 0x002B7268
		private void SetFilterUiRestState()
		{
			if (this._filterPanel != null)
			{
				this._filterPanel.rectTransform.localScale = Vector3.one;
			}
			if (this._filterPanelGroup != null)
			{
				this._filterPanelGroup.alpha = ((this._filterIndex == 0) ? 0.7f : 0.88f);
			}
		}

		// Token: 0x06006CB5 RID: 27829 RVA: 0x002B90C8 File Offset: 0x002B72C8
		private void UpdateTripodUi()
		{
			if (this._tripodButton == null || this._tripodButtonLabel == null || this._tripodButtonImage == null)
			{
				return;
			}
			bool flag = this._tripodActiveProvider != null && this._tripodActiveProvider();
			bool flag2 = this._tripodBusyProvider != null && this._tripodBusyProvider();
			bool flag3 = this._tripodPlacementProvider != null && this._tripodPlacementProvider();
			bool flag4 = flag2 || this._videoRecordingRequested || this._videoFinalizing || this._tripodToggleHandler == null;
			bool flag5 = !flag && !flag3 && (this._tripodPlacementAllowedProvider == null || this._tripodPlacementAllowedProvider());
			if (this._tripodButtonImage.gameObject.activeSelf != flag5)
			{
				this._tripodButtonImage.gameObject.SetActive(flag5);
			}
			if (!flag5)
			{
				return;
			}
			this._tripodButton.interactable = !flag4;
			int num = (flag2 ? 1 : 0) | (ControllerInput.IsActive ? 2 : 0);
			int version = ControllerGlyphs.Version;
			if (this._tripodLabelState != num || this._tripodLabelGlyphVersion != version)
			{
				this._tripodLabelState = num;
				this._tripodLabelGlyphVersion = version;
				LocalizedLegacyGameText.BindComputed(this._tripodButtonLabel, new Func<string>(this.TripodButtonCaption)).FitTranslationToRectangle();
			}
			this._tripodButtonLabel.color = ((flag || flag3) ? new Color(1f, 0.86f, 0.24f, 1f) : Color.white);
			this._tripodButtonImage.color = ((flag || flag3) ? new Color(0.16f, 0.13f, 0.03f, 0.84f) : new Color(0f, 0f, 0f, flag4 ? 0.36f : 0.58f));
		}

		// Token: 0x06006CB6 RID: 27830 RVA: 0x002B929C File Offset: 0x002B749C
		private void ApplyMediaModeVisualsImmediate()
		{
			this.EnsureMediaModeUi();
			bool flag = this._mediaMode == PhoneCameraCapture.CameraMediaMode.Video;
			if (this._photoModeLabel != null)
			{
				this._photoModeLabel.color = (flag ? PhoneCameraCapture.ModeLabelIdle : PhoneCameraCapture.CameraModeAccent);
			}
			if (this._videoModeLabel != null)
			{
				this._videoModeLabel.color = (flag ? PhoneCameraCapture.CameraModeAccent : PhoneCameraCapture.ModeLabelIdle);
			}
			if (this._modeUnderline != null)
			{
				Vector2 anchoredPosition = this._modeUnderline.rectTransform.anchoredPosition;
				anchoredPosition.x = (flag ? (-70f) : 70f);
				this._modeUnderline.rectTransform.anchoredPosition = anchoredPosition;
			}
			if (this._videoHudGroup != null)
			{
				this._videoHudGroup.alpha = (flag ? 1f : 0f);
			}
			if (this._shutterFill != null)
			{
				this._shutterFill.sprite = PhoneCameraCapture.CircleSprite();
				this._shutterFill.type = Image.Type.Simple;
				this._shutterFill.color = (flag ? PhoneCameraCapture.VideoShutterColor : PhoneCameraCapture.PhotoShutterColor);
				this._shutterFillRect.sizeDelta = new Vector2(78f, 78f);
				this._shutterFillRect.localScale = Vector3.one;
			}
			this.UpdateVideoHud();
		}

		// Token: 0x06006CB7 RID: 27831 RVA: 0x002B93EB File Offset: 0x002B75EB
		private IEnumerator AnimateMediaMode(PhoneCameraCapture.CameraMediaMode previous, PhoneCameraCapture.CameraMediaMode next)
		{
			this.EnsureMediaModeUi();
			bool toVideo = next == PhoneCameraCapture.CameraMediaMode.Video;
			float duration = Mathf.Max(0.05f, this.mediaModeTransitionDuration);
			float elapsed = 0f;
			float underlineFrom = ((this._modeUnderline != null) ? this._modeUnderline.rectTransform.anchoredPosition.x : ((previous == PhoneCameraCapture.CameraMediaMode.Video) ? (-70f) : 70f));
			float underlineTo = (toVideo ? (-70f) : 70f);
			Color fillFrom = ((this._shutterFill != null) ? this._shutterFill.color : ((previous == PhoneCameraCapture.CameraMediaMode.Video) ? PhoneCameraCapture.VideoShutterColor : PhoneCameraCapture.PhotoShutterColor));
			Color fillTo = (toVideo ? PhoneCameraCapture.VideoShutterColor : PhoneCameraCapture.PhotoShutterColor);
			float hudFrom = ((this._videoHudGroup != null) ? this._videoHudGroup.alpha : (toVideo ? 0f : 1f));
			while (elapsed < duration)
			{
				elapsed += Time.unscaledDeltaTime;
				float num = Mathf.Clamp01(elapsed / duration);
				float num2 = Mathf.SmoothStep(0f, 1f, num);
				if (this._modeUnderline != null)
				{
					Vector2 anchoredPosition = this._modeUnderline.rectTransform.anchoredPosition;
					anchoredPosition.x = Mathf.Lerp(underlineFrom, underlineTo, num2);
					this._modeUnderline.rectTransform.anchoredPosition = anchoredPosition;
				}
				if (this._shutterFill != null)
				{
					this._shutterFill.color = Color.Lerp(fillFrom, fillTo, num2);
					float num3 = 1f - Mathf.Sin(num * 3.1415927f) * 0.16f;
					this._shutterFillRect.localScale = Vector3.one * num3;
				}
				if (this._videoHudGroup != null)
				{
					this._videoHudGroup.alpha = Mathf.Lerp(hudFrom, toVideo ? 1f : 0f, num2);
				}
				if (this._photoModeLabel != null)
				{
					this._photoModeLabel.color = Color.Lerp((previous == PhoneCameraCapture.CameraMediaMode.Photo) ? PhoneCameraCapture.CameraModeAccent : PhoneCameraCapture.ModeLabelIdle, toVideo ? PhoneCameraCapture.ModeLabelIdle : PhoneCameraCapture.CameraModeAccent, num2);
				}
				if (this._videoModeLabel != null)
				{
					this._videoModeLabel.color = Color.Lerp((previous == PhoneCameraCapture.CameraMediaMode.Video) ? PhoneCameraCapture.CameraModeAccent : PhoneCameraCapture.ModeLabelIdle, toVideo ? PhoneCameraCapture.CameraModeAccent : PhoneCameraCapture.ModeLabelIdle, num2);
				}
				yield return null;
			}
			this.ApplyMediaModeVisualsImmediate();
			this._mediaModeTransition = null;
			yield break;
		}

		// Token: 0x06006CB8 RID: 27832 RVA: 0x002B9408 File Offset: 0x002B7608
		private void AnimateRecordingShutter(bool recording)
		{
			if (this._shutterFill == null)
			{
				return;
			}
			if (this._shutterTransition != null)
			{
				base.StopCoroutine(this._shutterTransition);
			}
			this._shutterTransition = base.StartCoroutine(this.AnimateRecordingShutterRoutine(recording));
		}

		// Token: 0x06006CB9 RID: 27833 RVA: 0x002B9440 File Offset: 0x002B7640
		private IEnumerator AnimateRecordingShutterRoutine(bool recording)
		{
			Vector2 startSize = this._shutterFillRect.sizeDelta;
			Vector2 targetSize = (recording ? new Vector2(48f, 48f) : new Vector2(78f, 78f));
			if (recording)
			{
				this._shutterFill.sprite = PhoneCameraCapture.UISprite();
				this._shutterFill.type = Image.Type.Sliced;
			}
			float elapsed = 0f;
			while (elapsed < 0.22f)
			{
				elapsed += Time.unscaledDeltaTime;
				float num = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / 0.22f));
				this._shutterFillRect.sizeDelta = Vector2.Lerp(startSize, targetSize, num);
				this._shutterFillRect.localRotation = Quaternion.Euler(0f, 0f, recording ? Mathf.Lerp(0f, 90f, num) : Mathf.Lerp(90f, 0f, num));
				yield return null;
			}
			this._shutterFillRect.sizeDelta = targetSize;
			this._shutterFillRect.localRotation = Quaternion.identity;
			if (!recording)
			{
				this._shutterFill.sprite = PhoneCameraCapture.CircleSprite();
				this._shutterFill.type = Image.Type.Simple;
			}
			this._shutterTransition = null;
			yield break;
		}

		// Token: 0x06006CBA RID: 27834 RVA: 0x002B9458 File Offset: 0x002B7658
		private void UpdateVideoHud()
		{
			if (this._videoTimerLabel == null || (!this.IsVideoMode && !this._videoRecordingRequested && !this._videoFinalizing))
			{
				return;
			}
			double num = (this._videoRecordingRequested ? Math.Max(0.0, Time.realtimeSinceStartupAsDouble - this._videoStartedAt) : ((this._aviWriter != null) ? this._aviWriter.DurationSeconds : 0.0));
			int num2 = Math.Max(0, (int)Math.Floor(num));
			if (this._displayedVideoSeconds != num2)
			{
				this._displayedVideoSeconds = num2;
				this._videoTimerLabel.text = PhoneCameraCapture.FormatVideoTime((double)num2);
			}
			if (this._recordingBadge != null)
			{
				float num3 = (this._videoRecordingRequested ? 0.96f : (this._videoFinalizing ? 0.72f : 0.62f));
				this._recordingBadge.color = new Color(PhoneCameraCapture.VideoShutterColor.r, PhoneCameraCapture.VideoShutterColor.g, PhoneCameraCapture.VideoShutterColor.b, num3);
			}
			int num4 = Mathf.Max(180, this.videoHeight);
			int num5 = Mathf.Clamp(this.videoFrameRate, 12, 60);
			if (this._videoQualityLabel != null && (this._displayedVideoHeight != num4 || this._displayedVideoFrameRate != num5))
			{
				this._displayedVideoHeight = num4;
				this._displayedVideoFrameRate = num5;
				this._videoQualityLabel.text = string.Format("{0}p  •  {1} FPS", num4, num5);
			}
			if (this._videoAudioLabel != null)
			{
				bool flag = (this._videoRecordingRequested ? this._videoHasAudio : this.recordGameAudio);
				int num6 = (flag ? 1 : 0);
				if (this._displayedVideoAudioState != num6)
				{
					this._displayedVideoAudioState = num6;
					LocalizedLegacyGameText.BindSource(this._videoAudioLabel, "phone_story", flag ? "●  AUDIO" : "AUDIO OFF").FitTranslationToRectangle();
					this._videoAudioLabel.color = (flag ? new Color(1f, 1f, 1f, 0.78f) : new Color(1f, 0.45f, 0.45f, 0.78f));
				}
			}
		}

		// Token: 0x06006CBB RID: 27835 RVA: 0x002B9680 File Offset: 0x002B7880
		private static string FormatVideoTime(double seconds)
		{
			int num = Math.Max(0, (int)Math.Floor(seconds));
			return string.Format("{0:00}:{1:00}:{2:00}", num / 3600, num / 60 % 60, num % 60);
		}

		// Token: 0x06006CBC RID: 27836 RVA: 0x002B96C6 File Offset: 0x002B78C6
		private static void CreateGridLine(RectTransform parent, Vector2 anchor, Vector2 size)
		{
			Image image = PhoneCameraCapture.NewOverlayImage(parent, "GridLine", new Color(1f, 1f, 1f, 0.16f));
			PhoneCameraCapture.SetOverlayRect(image.rectTransform, anchor, Vector2.zero, size);
			image.raycastTarget = false;
		}

		// Token: 0x06006CBD RID: 27837 RVA: 0x002B9704 File Offset: 0x002B7904
		private static RectTransform NewOverlayRect(Transform parent, string name)
		{
			GameObject gameObject = new GameObject(name, new Type[] { typeof(RectTransform) });
			gameObject.transform.SetParent(parent, false);
			return (RectTransform)gameObject.transform;
		}

		// Token: 0x06006CBE RID: 27838 RVA: 0x002B9738 File Offset: 0x002B7938
		private static Image NewOverlayImage(Transform parent, string name, Color color)
		{
			GameObject gameObject = new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Image)
			});
			gameObject.transform.SetParent(parent, false);
			Image component = gameObject.GetComponent<Image>();
			component.color = color;
			component.raycastTarget = false;
			return component;
		}

		// Token: 0x06006CBF RID: 27839 RVA: 0x002B9798 File Offset: 0x002B7998
		private static Text NewOverlayText(Transform parent, string name, string value, int fontSize, TextAnchor alignment)
		{
			GameObject gameObject = new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Text)
			});
			gameObject.transform.SetParent(parent, false);
			Text component = gameObject.GetComponent<Text>();
			component.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
			component.text = value;
			component.fontSize = fontSize;
			component.alignment = alignment;
			component.color = Color.white;
			component.raycastTarget = false;
			LocalizedLegacyGameText.BindFontOnly(component);
			return component;
		}

		// Token: 0x06006CC0 RID: 27840 RVA: 0x002B982C File Offset: 0x002B7A2C
		private static void AddHudTextOutline(Text text)
		{
			if (text == null)
			{
				return;
			}
			Outline outline = text.gameObject.AddComponent<Outline>();
			outline.effectColor = new Color(0f, 0f, 0f, 0.78f);
			outline.effectDistance = new Vector2(1.2f, -1.2f);
			outline.useGraphicAlpha = true;
		}

		// Token: 0x06006CC1 RID: 27841 RVA: 0x002B9888 File Offset: 0x002B7A88
		private static void SetOverlayRect(RectTransform rt, Vector2 anchor, Vector2 position, Vector2 size)
		{
			rt.anchorMax = anchor;
			rt.anchorMin = anchor;
			rt.pivot = new Vector2(0.5f, 0.5f);
			rt.anchoredPosition = position;
			rt.sizeDelta = size;
		}

		// Token: 0x06006CC2 RID: 27842 RVA: 0x002B98C8 File Offset: 0x002B7AC8
		private void BuildOverlay()
		{
			GameObject gameObject = new GameObject("PhoneCameraOverlayCanvas", new Type[]
			{
				typeof(Canvas),
				typeof(CanvasScaler)
			});
			this._overlayCanvas = gameObject.GetComponent<Canvas>();
			this._overlayCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
			this._overlayCanvas.sortingOrder = 1000;
			GameplayHudVisibility.RegisterCanvas(this._overlayCanvas);
			CanvasScaler component = gameObject.GetComponent<CanvasScaler>();
			component.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			component.referenceResolution = new Vector2(1920f, 1080f);
			component.matchWidthOrHeight = 0.5f;
			this._overlayRoot = new GameObject("PhoneCameraOverlay", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasGroup)
			});
			RectTransform rectTransform = (RectTransform)this._overlayRoot.transform;
			rectTransform.SetParent(this._overlayCanvas.transform, false);
			PhoneCameraCapture.StretchFull(rectTransform);
			this._overlayGroup = this._overlayRoot.GetComponent<CanvasGroup>();
			this._overlayGroup.blocksRaycasts = false;
			this._overlayGroup.interactable = false;
			float num = 36f;
			float num2 = 90f;
			float num3 = 5f;
			this.CreateCorner(rectTransform, new Vector2(0f, 1f), new Vector2(num, -num), num2, num3, true, true);
			this.CreateCorner(rectTransform, new Vector2(1f, 1f), new Vector2(-num, -num), num2, num3, false, true);
			this.CreateCorner(rectTransform, new Vector2(0f, 0f), new Vector2(num, num), num2, num3, true, false);
			this.CreateCorner(rectTransform, new Vector2(1f, 0f), new Vector2(-num, num), num2, num3, false, false);
			this.BuildShutterButton(rectTransform);
			this._zoomLabel = this.CreateLabel(rectTransform, new Vector2(0.5f, 0f), new Vector2(0f, 170f), "1.0x");
			this._galleryAnchor = this.CreatePoint(rectTransform, new Vector2(0f, 0f), new Vector2(80f, 80f));
			GameObject gameObject2 = new GameObject("Flash", new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			RectTransform rectTransform2 = (RectTransform)gameObject2.transform;
			rectTransform2.SetParent(rectTransform, false);
			PhoneCameraCapture.StretchFull(rectTransform2);
			this._flashImage = gameObject2.GetComponent<Image>();
			this._flashImage.color = new Color(1f, 1f, 1f, 0f);
			this._flashImage.raycastTarget = false;
		}

		// Token: 0x06006CC3 RID: 27843 RVA: 0x002B9B64 File Offset: 0x002B7D64
		private void BuildShutterButton(RectTransform parent)
		{
			GameObject gameObject = new GameObject("Shutter", new Type[] { typeof(RectTransform) });
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			rectTransform.SetParent(parent, false);
			RectTransform rectTransform2 = rectTransform;
			RectTransform rectTransform3 = rectTransform;
			Vector2 vector = new Vector2(0.5f, 0f);
			rectTransform3.anchorMax = vector;
			rectTransform2.anchorMin = vector;
			rectTransform.pivot = new Vector2(0.5f, 0.5f);
			rectTransform.anchoredPosition = new Vector2(0f, 88f);
			rectTransform.sizeDelta = new Vector2(116f, 116f);
			GameObject gameObject2 = new GameObject("ShutterBg", new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			RectTransform rectTransform4 = (RectTransform)gameObject2.transform;
			rectTransform4.SetParent(rectTransform, false);
			vector = new Vector2(0.5f, 0.5f);
			rectTransform4.anchorMax = vector;
			rectTransform4.anchorMin = vector;
			rectTransform4.pivot = new Vector2(0.5f, 0.5f);
			rectTransform4.sizeDelta = new Vector2(95.12f, 95.12f);
			Image component = gameObject2.GetComponent<Image>();
			component.sprite = PhoneCameraCapture.CircleSprite();
			component.color = new Color(0f, 0f, 0f, 0.55f);
			component.raycastTarget = false;
			GameObject gameObject3 = new GameObject("ShutterRing", new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			RectTransform rectTransform5 = (RectTransform)gameObject3.transform;
			rectTransform5.SetParent(rectTransform, false);
			vector = new Vector2(0.5f, 0.5f);
			rectTransform5.anchorMax = vector;
			rectTransform5.anchorMin = vector;
			rectTransform5.pivot = new Vector2(0.5f, 0.5f);
			rectTransform5.sizeDelta = new Vector2(116f, 116f);
			Image component2 = gameObject3.GetComponent<Image>();
			component2.sprite = PhoneCameraCapture.ShutterSprite();
			component2.color = this.overlayColor;
			component2.preserveAspect = true;
			component2.raycastTarget = false;
			if (component2.sprite == null)
			{
				component2.sprite = PhoneCameraCapture.CircleSprite();
				component2.color = new Color(this.overlayColor.r, this.overlayColor.g, this.overlayColor.b, 0.85f);
			}
			this._shutterButton = gameObject.AddComponent<Button>();
			this._shutterButton.targetGraphic = component2;
			if (this.clickToShoot)
			{
				this._shutterButton.onClick.AddListener(new UnityAction(this.HandleShutterPressed));
			}
		}

		// Token: 0x06006CC4 RID: 27844 RVA: 0x002B9DF1 File Offset: 0x002B7FF1
		private static Sprite ShutterSprite()
		{
			if (PhoneCameraCapture._shutterLoaded)
			{
				return PhoneCameraCapture._shutterSprite;
			}
			PhoneCameraCapture._shutterLoaded = true;
			PhoneCameraCapture._shutterSprite = Resources.Load<Sprite>("ShutterRing");
			return PhoneCameraCapture._shutterSprite;
		}

		// Token: 0x06006CC5 RID: 27845 RVA: 0x002B9E1C File Offset: 0x002B801C
		private void CreateCorner(RectTransform parent, Vector2 anchor, Vector2 pos, float len, float thick, bool left, bool top)
		{
			this.CreateBar(parent, anchor, pos, new Vector2(len, thick), left ? 0.5f : (-0.5f), 0f);
			this.CreateBar(parent, anchor, pos, new Vector2(thick, len), 0f, top ? (-0.5f) : 0.5f);
		}

		// Token: 0x06006CC6 RID: 27846 RVA: 0x002B9E78 File Offset: 0x002B8078
		private void CreateBar(RectTransform parent, Vector2 anchor, Vector2 pos, Vector2 size, float hShift, float vShift)
		{
			GameObject gameObject = new GameObject("Bar", new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			rectTransform.SetParent(parent, false);
			rectTransform.anchorMax = anchor;
			rectTransform.anchorMin = anchor;
			rectTransform.pivot = new Vector2(0.5f, 0.5f);
			rectTransform.anchoredPosition = pos + new Vector2(size.x * hShift, size.y * vShift);
			rectTransform.sizeDelta = size;
			Image component = gameObject.GetComponent<Image>();
			component.color = this.overlayColor;
			component.raycastTarget = false;
		}

		// Token: 0x06006CC7 RID: 27847 RVA: 0x002B9F2C File Offset: 0x002B812C
		private Text CreateLabel(RectTransform parent, Vector2 anchor, Vector2 pos, string text)
		{
			GameObject gameObject = new GameObject("ZoomLabel", new Type[]
			{
				typeof(RectTransform),
				typeof(Text)
			});
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			rectTransform.SetParent(parent, false);
			rectTransform.anchorMax = anchor;
			rectTransform.anchorMin = anchor;
			rectTransform.pivot = new Vector2(0.5f, 0.5f);
			rectTransform.anchoredPosition = pos;
			rectTransform.sizeDelta = new Vector2(120f, 40f);
			Text component = gameObject.GetComponent<Text>();
			component.text = text;
			component.alignment = TextAnchor.MiddleCenter;
			component.color = this.overlayColor;
			component.fontSize = 28;
			component.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
			component.raycastTarget = false;
			return component;
		}

		// Token: 0x06006CC8 RID: 27848 RVA: 0x002B9FF8 File Offset: 0x002B81F8
		private RectTransform CreatePoint(RectTransform parent, Vector2 anchor, Vector2 pos)
		{
			RectTransform rectTransform = (RectTransform)new GameObject("GalleryAnchor", new Type[] { typeof(RectTransform) }).transform;
			rectTransform.SetParent(parent, false);
			rectTransform.anchorMax = anchor;
			rectTransform.anchorMin = anchor;
			rectTransform.pivot = new Vector2(0.5f, 0.5f);
			rectTransform.anchoredPosition = pos;
			rectTransform.sizeDelta = new Vector2(70f, 70f);
			return rectTransform;
		}

		// Token: 0x06006CC9 RID: 27849 RVA: 0x001A38AD File Offset: 0x001A1AAD
		private static void StretchFull(RectTransform rt)
		{
			rt.anchorMin = Vector2.zero;
			rt.anchorMax = Vector2.one;
			rt.offsetMin = Vector2.zero;
			rt.offsetMax = Vector2.zero;
		}

		// Token: 0x06006CCA RID: 27850 RVA: 0x002BA075 File Offset: 0x002B8275
		private static Sprite UISprite()
		{
			if (PhoneCameraCapture._uiSprite != null)
			{
				return PhoneCameraCapture._uiSprite;
			}
			PhoneCameraCapture._uiSprite = PhoneUiSpriteFactory.Rounded;
			return PhoneCameraCapture._uiSprite;
		}

		// Token: 0x06006CCB RID: 27851 RVA: 0x002BA099 File Offset: 0x002B8299
		private static Sprite CircleSprite()
		{
			if (PhoneCameraCapture._circleSprite == null)
			{
				PhoneCameraCapture._circleSprite = PhoneUiSpriteFactory.Circle;
			}
			return PhoneCameraCapture._circleSprite;
		}

		// Token: 0x06006CCC RID: 27852 RVA: 0x002BA0B8 File Offset: 0x002B82B8
		private void EnsureAudio()
		{
			if (this.shutterAudio == null)
			{
				this.shutterAudio = base.gameObject.GetComponent<AudioSource>();
				if (this.shutterAudio == null)
				{
					this.shutterAudio = base.gameObject.AddComponent<AudioSource>();
				}
			}
			this.shutterAudio.playOnAwake = false;
			this.shutterAudio.spatialBlend = 0f;
			if (this.shutterClip == null)
			{
				this.shutterClip = this.GenerateShutterClip();
			}
			if (this.mediaModeSwitchClip == null)
			{
				this.mediaModeSwitchClip = PhoneCameraCapture.GenerateSoftVideoUiCue("CameraModeSwitch", PhoneCameraCapture.VideoUiCueStyle.ModeSwitch);
			}
			if (this.videoRecordingStartClip == null)
			{
				this.videoRecordingStartClip = PhoneCameraCapture.GenerateSoftVideoUiCue("VideoRecordingStart", PhoneCameraCapture.VideoUiCueStyle.RecordingStart);
			}
			if (this.videoRecordingStopClip == null)
			{
				this.videoRecordingStopClip = PhoneCameraCapture.GenerateSoftVideoUiCue("VideoRecordingStop", PhoneCameraCapture.VideoUiCueStyle.RecordingStop);
			}
		}

		// Token: 0x06006CCD RID: 27853 RVA: 0x002BA196 File Offset: 0x002B8396
		private void PlayShutterSound()
		{
			if (this.shutterAudio != null && this.shutterClip != null)
			{
				this.shutterAudio.PlayOneShot(this.shutterClip);
			}
		}

		// Token: 0x06006CCE RID: 27854 RVA: 0x002BA1C8 File Offset: 0x002B83C8
		private void PlayVideoUiSound(AudioClip clip)
		{
			if (this.shutterAudio == null)
			{
				this.EnsureAudio();
			}
			if (this.shutterAudio != null && clip != null && this.videoUiSoundVolume > 0f)
			{
				this.shutterAudio.PlayOneShot(clip, Mathf.Clamp01(this.videoUiSoundVolume));
			}
		}

		// Token: 0x06006CCF RID: 27855 RVA: 0x002BA224 File Offset: 0x002B8424
		private static AudioClip GenerateSoftVideoUiCue(string name, PhoneCameraCapture.VideoUiCueStyle style)
		{
			float num = ((style == PhoneCameraCapture.VideoUiCueStyle.ModeSwitch) ? 0.07f : ((style == PhoneCameraCapture.VideoUiCueStyle.RecordingStart) ? 0.115f : 0.13f));
			int num2 = Mathf.Max(1, Mathf.RoundToInt(44100f * num));
			float[] array = new float[num2];
			uint num3 = (uint)(-1847521883 + style * (PhoneCameraCapture.VideoUiCueStyle)1831565813);
			float num4 = 0f;
			for (int i = 0; i < num2; i++)
			{
				float num5 = (float)i / 44100f;
				num3 = num3 * 1664525U + 1013904223U;
				float num6 = (num3 >> 8) / 8388607.5f - 1f;
				num4 += (num6 - num4) * 0.075f;
				float num7;
				if (style != PhoneCameraCapture.VideoUiCueStyle.ModeSwitch)
				{
					if (style != PhoneCameraCapture.VideoUiCueStyle.RecordingStart)
					{
						num7 = PhoneCameraCapture.SoftUiPulse(num5, 0f, 385f, 44f, 0.115f) + PhoneCameraCapture.SoftUiPulse(num5, 0.046f, 275f, 47f, 0.092f) + PhoneCameraCapture.SoftUiTap(num5, 0f, num4, 96f, 0.014f);
					}
					else
					{
						num7 = PhoneCameraCapture.SoftUiPulse(num5, 0f, 325f, 42f, 0.14f) + PhoneCameraCapture.SoftUiPulse(num5, 0.043f, 395f, 50f, 0.062f) + PhoneCameraCapture.SoftUiTap(num5, 0f, num4, 92f, 0.014f);
					}
				}
				else
				{
					num7 = PhoneCameraCapture.SoftUiPulse(num5, 0f, 285f, 64f, 0.12f) + PhoneCameraCapture.SoftUiPulse(num5, 0.028f, 335f, 72f, 0.058f) + PhoneCameraCapture.SoftUiTap(num5, 0f, num4, 105f, 0.018f);
				}
				float num8 = Mathf.Clamp01((num - num5) / 0.012f);
				array[i] = Mathf.Clamp(num7 * num8, -0.22f, 0.22f);
			}
			AudioClip audioClip = AudioClip.Create(name, num2, 1, 44100, false);
			audioClip.SetData(array, 0);
			return audioClip;
		}

		// Token: 0x06006CD0 RID: 27856 RVA: 0x002BA418 File Offset: 0x002B8618
		private static float SoftUiPulse(float time, float startTime, float frequency, float decay, float level)
		{
			float num = time - startTime;
			if (num < 0f)
			{
				return 0f;
			}
			float num2 = (1f - Mathf.Exp(-num * 850f)) * Mathf.Exp(-num * decay);
			return Mathf.Sin(6.2831855f * frequency * num) * num2 * level;
		}

		// Token: 0x06006CD1 RID: 27857 RVA: 0x002BA468 File Offset: 0x002B8668
		private static float SoftUiTap(float time, float startTime, float filteredNoise, float decay, float level)
		{
			float num = time - startTime;
			if (num < 0f)
			{
				return 0f;
			}
			return filteredNoise * Mathf.Exp(-num * decay) * level;
		}

		// Token: 0x06006CD2 RID: 27858 RVA: 0x002BA498 File Offset: 0x002B8698
		private AudioClip GenerateShutterClip()
		{
			int num = 44100;
			int num2 = num / 12;
			float[] array = new float[num2];
			for (int i = 0; i < num2; i++)
			{
				float num3 = (float)i / (float)num;
				float num4 = Mathf.Exp(-num3 * 90f);
				float num5 = Mathf.Exp(-(num3 - 0.03f) * 90f) * ((num3 > 0.03f) ? 1f : 0f);
				float num6 = Random.value * 2f - 1f;
				array[i] = Mathf.Clamp(num6 * (num4 + num5) * 0.6f, -1f, 1f);
			}
			AudioClip audioClip = AudioClip.Create("ShutterClick", num2, 1, num, false);
			audioClip.SetData(array, 0);
			return audioClip;
		}

		// Token: 0x06006CD3 RID: 27859 RVA: 0x002BA550 File Offset: 0x002B8750
		private IEnumerator FlashRoutine()
		{
			if (this._flashImage == null)
			{
				yield break;
			}
			float up = 0.05f;
			float down = 0.22f;
			float t = 0f;
			while (t < up)
			{
				t += Time.deltaTime;
				this._flashImage.color = new Color(1f, 1f, 1f, Mathf.Lerp(0f, 0.85f, t / up));
				yield return null;
			}
			t = 0f;
			while (t < down)
			{
				t += Time.deltaTime;
				this._flashImage.color = new Color(1f, 1f, 1f, Mathf.Lerp(0.85f, 0f, t / down));
				yield return null;
			}
			this._flashImage.color = new Color(1f, 1f, 1f, 0f);
			yield break;
		}

		// Token: 0x06006CD4 RID: 27860 RVA: 0x002BA55F File Offset: 0x002B875F
		private IEnumerator FlyThumbnailRoutine(Sprite sprite)
		{
			if (this._overlayRoot == null || this._galleryAnchor == null)
			{
				yield break;
			}
			GameObject go = new GameObject("FlyingThumb", new Type[]
			{
				typeof(RectTransform),
				typeof(Image),
				typeof(CanvasGroup)
			});
			RectTransform rt = (RectTransform)go.transform;
			rt.SetParent(this._overlayRoot.transform, false);
			RectTransform rectTransform = rt;
			RectTransform rectTransform2 = rt;
			Vector2 vector = new Vector2(0.5f, 0.5f);
			rectTransform2.anchorMax = vector;
			rectTransform.anchorMin = vector;
			rt.pivot = new Vector2(0.5f, 0.5f);
			Image component = go.GetComponent<Image>();
			component.sprite = sprite;
			StreamerCensorImage.Bind(component, false);
			component.preserveAspect = true;
			component.raycastTarget = false;
			CanvasGroup cg = go.GetComponent<CanvasGroup>();
			Vector2 startPos = Vector2.zero;
			Vector2 startSize = new Vector2(360f, 360f);
			Vector2 endPos = this._galleryAnchor.anchoredPosition - new Vector2(((RectTransform)this._overlayRoot.transform).rect.width * 0.5f - this._galleryAnchor.rect.width * 0.5f, ((RectTransform)this._overlayRoot.transform).rect.height * 0.5f - this._galleryAnchor.rect.height * 0.5f);
			Vector2 endSize = new Vector2(70f, 70f);
			float dur = 0.55f;
			float t = 0f;
			while (t < dur)
			{
				t += Time.deltaTime;
				float num = Mathf.SmoothStep(0f, 1f, t / dur);
				rt.anchoredPosition = Vector2.Lerp(startPos, endPos, num);
				rt.sizeDelta = Vector2.Lerp(startSize, endSize, num);
				cg.alpha = Mathf.Lerp(1f, 0.2f, num);
				yield return null;
			}
			Object.Destroy(go);
			yield break;
		}

		// Token: 0x06006CD5 RID: 27861 RVA: 0x002BA575 File Offset: 0x002B8775
		private void OnApplicationPause(bool paused)
		{
			if (paused && this._videoRecordingRequested)
			{
				this.StopVideoRecording();
			}
		}

		// Token: 0x06006CD6 RID: 27862 RVA: 0x002BA588 File Offset: 0x002B8788
		private void OnDisable()
		{
			this.EndTripodSpatialAudio();
		}

		// Token: 0x06006CD7 RID: 27863 RVA: 0x002BA590 File Offset: 0x002B8790
		private void OnDestroy()
		{
			this.EndTripodSpatialAudio();
			if (this._shutterButton != null)
			{
				this._shutterButton.onClick.RemoveListener(new UnityAction(this.HandleShutterPressed));
			}
			if (this._filterTransition != null)
			{
				base.StopCoroutine(this._filterTransition);
				this._filterTransition = null;
			}
			this.RestoreFilterPostProcessing();
			if (this._filterProfile != null)
			{
				Object.Destroy(this._filterProfile);
				this._filterProfile = null;
			}
			if (this._filterVolume != null)
			{
				Object.Destroy(this._filterVolume.gameObject);
				this._filterVolume = null;
			}
			this._acceptVideoReadbacks = false;
			this.StopVideoFrameEncoder();
			if (this._videoRoutine != null)
			{
				base.StopCoroutine(this._videoRoutine);
				this._videoRoutine = null;
			}
			if (this._aviWriter != null)
			{
				this._videoRecordingRequested = false;
				this._videoFinalizing = true;
				this.FinishVideoRecording(this._aviWriter.VideoFrameCount > 0, false);
			}
			if (this._captureRT != null)
			{
				this._captureRT.Release();
				Object.Destroy(this._captureRT);
				this._captureRT = null;
			}
			if (this._videoRT != null)
			{
				this._videoRT.Release();
				Object.Destroy(this._videoRT);
				this._videoRT = null;
			}
			if (this._videoFrameTexture != null)
			{
				Object.Destroy(this._videoFrameTexture);
				this._videoFrameTexture = null;
			}
		}

		// Token: 0x04006ED3 RID: 28371
		[Header("Input")]
		[Tooltip("Tasto per scattare la foto mentre si e' in modalita' fotocamera selfie.")]
		public KeyCode shutterKey = KeyCode.Space;

		// Token: 0x04006ED4 RID: 28372
		[Tooltip("Passa in modo fluido tra Foto e Video.")]
		public KeyCode mediaModeSwitchKey = KeyCode.H;

		// Token: 0x04006ED5 RID: 28373
		[Tooltip("Piazza/riprende il telefono sul treppiede. Disponibile sia in Foto sia in Video.")]
		public KeyCode tripodToggleKey = KeyCode.T;

		// Token: 0x04006ED6 RID: 28374
		[Tooltip("Alterna i filtri fotografici live. Il look selezionato viene incluso sia nelle foto sia nei frame video.")]
		public KeyCode filterSwitchKey = KeyCode.F;

		// Token: 0x04006ED7 RID: 28375
		[Header("Filtri fotocamera")]
		[Tooltip("Durata del blend tra un filtro e il successivo.")]
		[Range(0.12f, 0.8f)]
		public float filterTransitionDuration = 0.26f;

		// Token: 0x04006ED8 RID: 28376
		[Header("Qualita' cattura")]
		[Tooltip("Larghezza (px) della foto. Landscape 16:9: 2048x1152 = ottima qualita' orizzontale.")]
		public int captureWidth = 2048;

		// Token: 0x04006ED9 RID: 28377
		[Tooltip("Altezza (px) della foto. Landscape 16:9: 2048x1152.")]
		public int captureHeight = 1152;

		// Token: 0x04006EDA RID: 28378
		[Tooltip("Campioni MSAA della RT di cattura (1/2/4/8). Anti-alias dei bordi -> foto piu' pulita.")]
		[Range(1f, 8f)]
		public int captureMsaa = 8;

		// Token: 0x04006EDB RID: 28379
		[Header("Registrazione video runtime")]
		[Tooltip("Risoluzione del file video. Sovrascritta dalle preferenze giocatore (Settings > Game > Phone Video Resolution): il valore serializzato e' solo il fallback.")]
		public int videoWidth = 1920;

		// Token: 0x04006EDC RID: 28380
		public int videoHeight = 1080;

		// Token: 0x04006EDD RID: 28381
		[Tooltip("Frame rate nominale del file. Sovrascritto dalle preferenze giocatore (Settings > Game > Phone Video FPS). Se il gioco rallenta, l'ultimo frame viene duplicato per mantenere il timer e l'audio sincronizzati.")]
		[Range(12f, 60f)]
		public int videoFrameRate = 30;

		// Token: 0x04006EDE RID: 28382
		[Tooltip("Qualita' JPEG dei frame MJPEG (1-100). 85 conserva bene i dettagli senza gonfiare eccessivamente file e carico CPU.")]
		[Range(35f, 95f)]
		public int videoJpegQuality = 85;

		// Token: 0x04006EDF RID: 28383
		[Tooltip("Numero massimo di frame raw in attesa dell'encoder asincrono. Una coda corta protegge RAM e frame time: se e' piena, il video duplica l'ultimo frame invece di far laggare il gioco.")]
		[Range(1f, 6f)]
		public int videoEncoderQueueDepth = 2;

		// Token: 0x04006EE0 RID: 28384
		[Tooltip("Limite di sicurezza di una singola registrazione.")]
		[Range(10f, 900f)]
		public float maxVideoDurationSeconds = 300f;

		// Token: 0x04006EE1 RID: 28385
		[Tooltip("Registra l'audio mixato che arriva all'AudioListener. Se non esiste un listener, il video viene comunque salvato senza traccia audio.")]
		public bool recordGameAudio = true;

		// Token: 0x04006EE2 RID: 28386
		[Tooltip("Imprime nel file video un piccolo timer rosso nell'angolo in alto a destra. Non e' solo UI: resta visibile anche riproducendo o esportando il video.")]
		public bool burnRecordingTimerIntoVideo = true;

		// Token: 0x04006EE3 RID: 28387
		[Header("Audio visuale da treppiede")]
		[Tooltip("Distanza entro cui voce, respiri e suoni corporei di Ciel restano a volume pieno quando la visuale usa il microfono virtuale del treppiede, anche fuori da REC.")]
		[Range(0.25f, 3f)]
		public float tripodAudioMinDistance = 0.8f;

		// Token: 0x04006EE4 RID: 28388
		[Tooltip("Distanza massima dei suoni di Ciel nella visuale dal treppiede. Gli NPC mantengono le proprie curve 3D gia' configurate.")]
		[Range(8f, 50f)]
		public float tripodAudioMaxDistance = 24f;

		// Token: 0x04006EE5 RID: 28389
		[Tooltip("Intervallo di scansione per includere anche AudioSource creati mentre il POV e' attivo.")]
		[Range(0.1f, 2f)]
		public float tripodAudioRefreshInterval = 0.4f;

		// Token: 0x04006EE6 RID: 28390
		[Tooltip("Modalita' selezionata ogni volta che si apre la fotocamera.")]
		public PhoneCameraCapture.CameraMediaMode defaultMediaMode;

		// Token: 0x04006EE7 RID: 28391
		[Tooltip("Durata della transizione Foto/Video.")]
		[Range(0.12f, 0.8f)]
		public float mediaModeTransitionDuration = 0.28f;

		// Token: 0x04006EE8 RID: 28392
		[Header("Riferimenti (auto-risolti se nulli)")]
		[Tooltip("Canvas World-Space del telefono sotto cui creare l'overlay fotocamera. Se null, risolto dal SmartphoneController al setup.")]
		public Canvas phoneCanvas;

		// Token: 0x04006EE9 RID: 28393
		[Tooltip("AudioSource per lo scatto. Se null ne viene aggiunto uno a runtime.")]
		public AudioSource shutterAudio;

		// Token: 0x04006EEA RID: 28394
		[Tooltip("Clip suono otturatore. Se null, viene generato un click sintetico.")]
		public AudioClip shutterClip;

		// Token: 0x04006EEB RID: 28395
		[Header("Audio feedback video")]
		[Tooltip("Cue breve durante lo switch Foto/Video. Se null viene generato a runtime.")]
		public AudioClip mediaModeSwitchClip;

		// Token: 0x04006EEC RID: 28396
		[Tooltip("Conferma morbida quando parte la registrazione. Se null viene generata a runtime.")]
		public AudioClip videoRecordingStartClip;

		// Token: 0x04006EED RID: 28397
		[Tooltip("Conferma morbida quando termina la registrazione. Se null viene generata a runtime.")]
		public AudioClip videoRecordingStopClip;

		// Token: 0x04006EEE RID: 28398
		[Tooltip("Volume dei feedback Foto/Video. Il valore predefinito e' volutamente discreto.")]
		[Range(0f, 1f)]
		public float videoUiSoundVolume = 0.34f;

		// Token: 0x04006EEF RID: 28399
		[Header("Overlay UI (in scena)")]
		[Tooltip("Overlay fotocamera selfie GIA' IN SCENA (Canvas dedicato disattivato). Se assegnato, viene acceso/spento e i suoi riferimenti (zoom/flash/gallery) usati al posto di quelli generati da codice. Cosi' la UI si modifica nell'editor. Se null, l'overlay viene costruito da codice (comportamento legacy).")]
		public SelfieCameraOverlayUI overlayUI;

		// Token: 0x04006EF0 RID: 28400
		[Tooltip("Se l'overlay in scena ha un bottone shutter assegnato, cliccarlo scatta la foto (oltre al tasto SPACE).")]
		public bool clickToShoot = true;

		// Token: 0x04006EF1 RID: 28401
		[Header("Overlay - stile (solo overlay generato da codice)")]
		[Tooltip("Colore degli elementi dell'overlay (angoli, shutter). Usato solo se overlayUI e' null.")]
		public Color overlayColor = new Color(1f, 1f, 1f, 0.92f);

		// Token: 0x04006EF2 RID: 28402
		[Header("Face censor (CielGram)")]
		[Tooltip("Every shot also produces a hidden 'censored' twin: a second render with the FaceCensor object enabled (blurs/covers the face). Saved next to the normal photo as IMG_<tag>_<rand>.censored.png. The gallery hides it; CielGram sends it in chat.")]
		public bool produceCensoredVersion = true;

		// Token: 0x04006EF3 RID: 28403
		[Tooltip("The FaceCensor object on the player's head (child of the Head bone). If null, it's auto-resolved at runtime by name from the player's Head bone. Kept disabled except during the censored capture pass.")]
		public GameObject faceCensor;

		// Token: 0x04006EF4 RID: 28404
		[Tooltip("Name of the FaceCensor child under the player's Head bone, used for auto-resolution when 'faceCensor' is not assigned.")]
		public string faceCensorObjectName = "FaceCensor";

		// Token: 0x04006EF5 RID: 28405
		[Header("Debug")]
		public bool debugLogs;

		// Token: 0x04006EF6 RID: 28406
		private bool _cameraModeActive;

		// Token: 0x04006EF7 RID: 28407
		private Camera _mainCamera;

		// Token: 0x04006EF8 RID: 28408
		private RenderTexture _captureRT;

		// Token: 0x04006EF9 RID: 28409
		private bool _faceCensorResolved;

		// Token: 0x04006EFA RID: 28410
		private PhoneCameraCapture.CameraMediaMode _mediaMode;

		// Token: 0x04006EFB RID: 28411
		private bool _videoRecordingRequested;

		// Token: 0x04006EFC RID: 28412
		private bool _videoFinalizing;

		// Token: 0x04006EFD RID: 28413
		private double _videoStartedAt;

		// Token: 0x04006EFE RID: 28414
		private string _videoFinalPath;

		// Token: 0x04006EFF RID: 28415
		private string _videoTemporaryPath;

		// Token: 0x04006F00 RID: 28416
		private string _videoThumbnailPath;

		// Token: 0x04006F01 RID: 28417
		private byte[] _videoThumbnailJpeg;

		// Token: 0x04006F02 RID: 28418
		private byte[] _lastEncodedVideoJpeg;

		// Token: 0x04006F03 RID: 28419
		private RuntimeMjpegAviWriter _aviWriter;

		// Token: 0x04006F04 RID: 28420
		private RenderTexture _videoRT;

		// Token: 0x04006F05 RID: 28421
		private Texture2D _videoFrameTexture;

		// Token: 0x04006F06 RID: 28422
		private PhoneVideoFrameEncoder _videoFrameEncoder;

		// Token: 0x04006F07 RID: 28423
		private int _pendingVideoReadbacks;

		// Token: 0x04006F08 RID: 28424
		private int _videoDroppedCaptures;

		// Token: 0x04006F09 RID: 28425
		private volatile bool _acceptVideoReadbacks;

		// Token: 0x04006F0A RID: 28426
		private double _videoStoppedAt;

		// Token: 0x04006F0B RID: 28427
		private PhoneVideoAudioCapture _audioCapture;

		// Token: 0x04006F0C RID: 28428
		private readonly ConcurrentQueue<byte[]> _audioQueue = new ConcurrentQueue<byte[]>();

		// Token: 0x04006F0D RID: 28429
		private int _queuedAudioBytes;

		// Token: 0x04006F0E RID: 28430
		private volatile bool _acceptAudio;

		// Token: 0x04006F0F RID: 28431
		private bool _videoHasAudio;

		// Token: 0x04006F10 RID: 28432
		private Coroutine _videoRoutine;

		// Token: 0x04006F11 RID: 28433
		private string _videoStatusOverride;

		// Token: 0x04006F12 RID: 28434
		private float _videoStatusUntil;

		// Token: 0x04006F13 RID: 28435
		private CameraController _toolkitCamController;

		// Token: 0x04006F14 RID: 28436
		private GameObject _galleryImagePrefab;

		// Token: 0x04006F15 RID: 28437
		private Transform _galleryImageParent;

		// Token: 0x04006F16 RID: 28438
		private string _galleryFolder;

		// Token: 0x04006F17 RID: 28439
		private GameObject _overlayRoot;

		// Token: 0x04006F18 RID: 28440
		private Image _flashImage;

		// Token: 0x04006F19 RID: 28441
		private RectTransform _galleryAnchor;

		// Token: 0x04006F1A RID: 28442
		private CanvasGroup _overlayGroup;

		// Token: 0x04006F1B RID: 28443
		private Button _shutterButton;

		// Token: 0x04006F1C RID: 28444
		private Image _shutterFill;

		// Token: 0x04006F1D RID: 28445
		private RectTransform _shutterFillRect;

		// Token: 0x04006F1E RID: 28446
		private Text _photoModeLabel;

		// Token: 0x04006F1F RID: 28447
		private Text _videoModeLabel;

		// Token: 0x04006F20 RID: 28448
		private Image _modeUnderline;

		// Token: 0x04006F21 RID: 28449
		private CanvasGroup _videoHudGroup;

		// Token: 0x04006F22 RID: 28450
		private Image _recordingBadge;

		// Token: 0x04006F23 RID: 28451
		private Text _videoTimerLabel;

		// Token: 0x04006F24 RID: 28452
		private Text _videoQualityLabel;

		// Token: 0x04006F25 RID: 28453
		private Text _videoAudioLabel;

		// Token: 0x04006F26 RID: 28454
		private int _displayedVideoSeconds = -1;

		// Token: 0x04006F27 RID: 28455
		private int _displayedVideoHeight = -1;

		// Token: 0x04006F28 RID: 28456
		private int _displayedVideoFrameRate = -1;

		// Token: 0x04006F29 RID: 28457
		private int _displayedVideoAudioState = -1;

		// Token: 0x04006F2A RID: 28458
		private Button _tripodButton;

		// Token: 0x04006F2B RID: 28459
		private Image _tripodButtonImage;

		// Token: 0x04006F2C RID: 28460
		private Text _tripodButtonLabel;

		// Token: 0x04006F2D RID: 28461
		private Coroutine _mediaModeTransition;

		// Token: 0x04006F2E RID: 28462
		private Coroutine _shutterTransition;

		// Token: 0x04006F2F RID: 28463
		private Coroutine _filterTransition;

		// Token: 0x04006F30 RID: 28464
		private Image _filterPanel;

		// Token: 0x04006F31 RID: 28465
		private Image _filterAccent;

		// Token: 0x04006F32 RID: 28466
		private Text _filterLabel;

		// Token: 0x04006F33 RID: 28467
		private Text _filterKeyLabel;

		// Token: 0x04006F34 RID: 28468
		private Text _mediaModeKeyLabel;

		// Token: 0x04006F35 RID: 28469
		private PhoneCameraCapture.KeyLabelState _filterKeyState;

		// Token: 0x04006F36 RID: 28470
		private PhoneCameraCapture.KeyLabelState _mediaModeKeyState;

		// Token: 0x04006F37 RID: 28471
		private int _tripodLabelState = -1;

		// Token: 0x04006F38 RID: 28472
		private int _tripodLabelGlyphVersion = -1;

		// Token: 0x04006F39 RID: 28473
		private CanvasGroup _filterPanelGroup;

		// Token: 0x04006F3A RID: 28474
		private static readonly PhoneCameraCapture.CameraFilterPreset[] FilterPresets = new PhoneCameraCapture.CameraFilterPreset[]
		{
			new PhoneCameraCapture.CameraFilterPreset("NATURAL", 0f, 0f, 0f, 0f, 0f, 0f, Color.white, new Color(1f, 1f, 1f, 0.92f)),
			new PhoneCameraCapture.CameraFilterPreset("WARM", 0.08f, 7f, -2f, 10f, 22f, 4f, new Color(1f, 0.96f, 0.88f, 1f), new Color(1f, 0.66f, 0.3f, 1f)),
			new PhoneCameraCapture.CameraFilterPreset("COOL", 0.02f, 9f, 1f, 6f, -18f, 3f, new Color(0.9f, 0.96f, 1f, 1f), new Color(0.35f, 0.78f, 1f, 1f)),
			new PhoneCameraCapture.CameraFilterPreset("FILM", -0.08f, -4f, -6f, -20f, 12f, 7f, new Color(1f, 0.93f, 0.78f, 1f), new Color(0.93f, 0.72f, 0.42f, 1f)),
			new PhoneCameraCapture.CameraFilterPreset("MONO", 0.05f, 24f, 0f, -100f, 0f, 0f, Color.white, new Color(0.82f, 0.82f, 0.82f, 1f)),
			new PhoneCameraCapture.CameraFilterPreset("DREAM", 0.18f, -14f, -5f, 14f, 8f, 12f, new Color(1f, 0.9f, 0.98f, 1f), new Color(1f, 0.52f, 0.82f, 1f))
		};

		// Token: 0x04006F3B RID: 28475
		private int _filterIndex;

		// Token: 0x04006F3C RID: 28476
		private Volume _filterVolume;

		// Token: 0x04006F3D RID: 28477
		private VolumeProfile _filterProfile;

		// Token: 0x04006F3E RID: 28478
		private ColorAdjustments _filterColorAdjustments;

		// Token: 0x04006F3F RID: 28479
		private WhiteBalance _filterWhiteBalance;

		// Token: 0x04006F40 RID: 28480
		private UniversalAdditionalCameraData _filterCameraData;

		// Token: 0x04006F41 RID: 28481
		private bool _filterCameraPostProcessingSaved;

		// Token: 0x04006F42 RID: 28482
		private bool _filterCameraPostProcessingOverridden;

		// Token: 0x04006F43 RID: 28483
		private bool _disableFilterAfterVideo;

		// Token: 0x04006F44 RID: 28484
		private static readonly Color PhotoShutterColor = new Color(1f, 1f, 1f, 0.98f);

		// Token: 0x04006F45 RID: 28485
		private static readonly Color VideoShutterColor = new Color(1f, 0.105f, 0.12f, 1f);

		// Token: 0x04006F46 RID: 28486
		private static readonly Color CameraModeAccent = new Color(1f, 0.82f, 0.12f, 1f);

		// Token: 0x04006F47 RID: 28487
		private static readonly Color ModeLabelIdle = new Color(1f, 1f, 1f, 0.58f);

		// Token: 0x04006F48 RID: 28488
		private Func<float> _zoomLabelProvider;

		// Token: 0x04006F49 RID: 28489
		private Action _tripodToggleHandler;

		// Token: 0x04006F4A RID: 28490
		private Func<bool> _tripodActiveProvider;

		// Token: 0x04006F4B RID: 28491
		private Func<bool> _tripodBusyProvider;

		// Token: 0x04006F4C RID: 28492
		private Func<bool> _tripodPlacementProvider;

		// Token: 0x04006F4D RID: 28493
		private Func<bool> _tripodPlacementAllowedProvider;

		// Token: 0x04006F4E RID: 28494
		private Func<Transform> _tripodAudioSubjectProvider;

		// Token: 0x04006F4F RID: 28495
		private readonly TripodRecordingAudioScope _tripodSpatialAudioScope = new TripodRecordingAudioScope();

		// Token: 0x04006F50 RID: 28496
		private float _nextTripodAudioRefreshAt;

		// Token: 0x04006F51 RID: 28497
		private bool _usingSceneOverlay;

		// Token: 0x04006F52 RID: 28498
		private bool _overlayBound;

		// Token: 0x04006F53 RID: 28499
		private readonly Dictionary<KeyCode, GenericInput> _cameraInputs = new Dictionary<KeyCode, GenericInput>();

		// Token: 0x04006F54 RID: 28500
		private bool _captureInProgress;

		// Token: 0x04006F55 RID: 28501
		private SelfieFrameTagger _frameTagger;

		// Token: 0x04006F56 RID: 28502
		private SelfiePosePicker _posePicker;

		// Token: 0x04006F57 RID: 28503
		public const string FaceInFrameFlag = "face";

		// Token: 0x04006F58 RID: 28504
		private Text _zoomLabel;

		// Token: 0x04006F59 RID: 28505
		private Text _displayedZoomTarget;

		// Token: 0x04006F5A RID: 28506
		private float _displayedZoom = float.NaN;

		// Token: 0x04006F5B RID: 28507
		private Canvas _overlayCanvas;

		// Token: 0x04006F5C RID: 28508
		private static Sprite _shutterSprite;

		// Token: 0x04006F5D RID: 28509
		private static bool _shutterLoaded;

		// Token: 0x04006F5E RID: 28510
		private static Sprite _uiSprite;

		// Token: 0x04006F5F RID: 28511
		private static Sprite _circleSprite;

		// Token: 0x02000E34 RID: 3636
		public enum CameraMediaMode
		{
			// Token: 0x04006F61 RID: 28513
			Photo,
			// Token: 0x04006F62 RID: 28514
			Video
		}

		// Token: 0x02000E35 RID: 3637
		private enum VideoUiCueStyle
		{
			// Token: 0x04006F64 RID: 28516
			ModeSwitch,
			// Token: 0x04006F65 RID: 28517
			RecordingStart,
			// Token: 0x04006F66 RID: 28518
			RecordingStop
		}

		// Token: 0x02000E36 RID: 3638
		private struct CameraFilterPreset
		{
			// Token: 0x06006CDA RID: 27866 RVA: 0x002BAB18 File Offset: 0x002B8D18
			public CameraFilterPreset(string label, float exposure, float contrast, float hue, float saturation, float temperature, float tint, Color colorFilter, Color accent)
			{
				this.Label = label;
				this.Exposure = exposure;
				this.Contrast = contrast;
				this.Hue = hue;
				this.Saturation = saturation;
				this.Temperature = temperature;
				this.Tint = tint;
				this.ColorFilter = colorFilter;
				this.Accent = accent;
			}

			// Token: 0x04006F67 RID: 28519
			public readonly string Label;

			// Token: 0x04006F68 RID: 28520
			public readonly float Exposure;

			// Token: 0x04006F69 RID: 28521
			public readonly float Contrast;

			// Token: 0x04006F6A RID: 28522
			public readonly float Hue;

			// Token: 0x04006F6B RID: 28523
			public readonly float Saturation;

			// Token: 0x04006F6C RID: 28524
			public readonly float Temperature;

			// Token: 0x04006F6D RID: 28525
			public readonly float Tint;

			// Token: 0x04006F6E RID: 28526
			public readonly Color ColorFilter;

			// Token: 0x04006F6F RID: 28527
			public readonly Color Accent;
		}

		// Token: 0x02000E37 RID: 3639
		private struct KeyLabelState
		{
			// Token: 0x04006F70 RID: 28528
			public Text target;

			// Token: 0x04006F71 RID: 28529
			public KeyCode key;

			// Token: 0x04006F72 RID: 28530
			public bool controller;

			// Token: 0x04006F73 RID: 28531
			public int glyphVersion;
		}
	}
}
