using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Cinemachine;
using Features.CensorBars;
using Features.Detection;
using Features.Localization;
using Invector.vCamera;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.Rendering;
using ProjectExhibitionist.Interactions;
using ProjectExhibitionist.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace Features.SecurityCamera
{
	// Token: 0x02000C5E RID: 3166
	[DisallowMultipleComponent]
	public class SecurityCameraController : MonoBehaviour
	{
		// Token: 0x17000F04 RID: 3844
		// (get) Token: 0x06005B99 RID: 23449 RVA: 0x0023EC0C File Offset: 0x0023CE0C
		public bool IsViewing
		{
			get
			{
				return this._activeIndex >= 0;
			}
		}

		// Token: 0x17000F05 RID: 3845
		// (get) Token: 0x06005B9A RID: 23450 RVA: 0x0023EC1A File Offset: 0x0023CE1A
		public int ActiveIndex
		{
			get
			{
				return this._activeIndex;
			}
		}

		// Token: 0x17000F06 RID: 3846
		// (get) Token: 0x06005B9B RID: 23451 RVA: 0x0023EC22 File Offset: 0x0023CE22
		public int CameraCount
		{
			get
			{
				return this.securityCams.Count;
			}
		}

		// Token: 0x17000F07 RID: 3847
		// (get) Token: 0x06005B9C RID: 23452 RVA: 0x0023EC2F File Offset: 0x0023CE2F
		public bool IsLookingAtCamera
		{
			get
			{
				return this._lookingAtCamera;
			}
		}

		// Token: 0x06005B9D RID: 23453 RVA: 0x0023EC38 File Offset: 0x0023CE38
		private void Start()
		{
			this.ResolveReferences();
			this.CollectVCamsIfNeeded();
			foreach (CinemachineVirtualCamera cinemachineVirtualCamera in this.securityCams)
			{
				if (cinemachineVirtualCamera != null)
				{
					cinemachineVirtualCamera.Priority = this.idlePriority;
				}
			}
			this.DisableBrain();
			this.SetCctvFxActive(false);
			this.CaptureMainCameraLocal();
			this.SetupAudio();
			this.SetupInteractionUi();
		}

		// Token: 0x06005B9E RID: 23454 RVA: 0x0023ECC4 File Offset: 0x0023CEC4
		private void SetupAudio()
		{
			this._audioH = this.CreateMotorSource(this.horizontalMoveClip);
			this._audioV = this.CreateMotorSource(this.verticalMoveClip);
			this._audioZoom = this.CreateMotorSource(this.zoomClip);
		}

		// Token: 0x06005B9F RID: 23455 RVA: 0x0023ECFC File Offset: 0x0023CEFC
		private AudioSource CreateMotorSource(AudioClip clip)
		{
			if (clip == null)
			{
				return null;
			}
			AudioSource audioSource = base.gameObject.AddComponent<AudioSource>();
			audioSource.clip = clip;
			audioSource.loop = true;
			audioSource.playOnAwake = false;
			audioSource.spatialBlend = 0f;
			audioSource.volume = 0f;
			return audioSource;
		}

		// Token: 0x06005BA0 RID: 23456 RVA: 0x0023ED4A File Offset: 0x0023CF4A
		private void OnDisable()
		{
			if (this.IsViewing)
			{
				this.ExitToGameplay();
			}
			this.HideProximityPrompt();
			this.ReleaseViewingGate();
			this.CancelCapture();
		}

		// Token: 0x06005BA1 RID: 23457 RVA: 0x0023ED6C File Offset: 0x0023CF6C
		private void OnDestroy()
		{
			this.ReleaseViewingGate();
			if (this._proximityConfig != null)
			{
				Object.Destroy(this._proximityConfig);
			}
			if (this._viewActionsConfig != null)
			{
				Object.Destroy(this._viewActionsConfig);
			}
		}

		// Token: 0x06005BA2 RID: 23458 RVA: 0x0023EDA8 File Offset: 0x0023CFA8
		private void Update()
		{
			if (GraphicsSettingsRuntimeUI.AnyOpen || TextInputFocus.BlocksGameplayInput || PlayerActivityGate.IsResourceOwned(PlayerContextResource.ModalUI))
			{
				this.HideProximityPrompt();
				this._nearestInteractionIndex = -1;
				this._wantH = (this._wantV = false);
				this._zoomAudioHold = 0f;
				this.UpdateMotorAudio();
				return;
			}
			this.UpdateProximityInteraction();
			if (!this.IsViewing)
			{
				if (this.enterKey != KeyCode.None && GameplayInput.GetKeyDown(this.enterKey))
				{
					this.TryEnterNearestCamera();
				}
			}
			else if (this.exitKey != KeyCode.None && (ControllerInput.IsActive ? ControllerWorldPointer.Down(ControllerButton.East) : GameplayInput.GetKeyDown(this.exitKey)))
			{
				this.ExitToGameplay();
			}
			else if (this.cycleKey != KeyCode.None && (ControllerInput.IsActive ? ControllerWorldPointer.Down(ControllerButton.RightShoulder) : GameplayInput.GetKeyDown(this.cycleKey)))
			{
				this.SwitchToNextCamera();
			}
			this._wantH = (this._wantV = false);
			if (this._zoomAudioHold > 0f)
			{
				this._zoomAudioHold -= Time.unscaledDeltaTime;
			}
			if (!this.IsViewing)
			{
				this.UpdateMotorAudio();
				return;
			}
			this.ShowViewingActions();
			if (ControllerInput.IsActive ? ControllerWorldPointer.Down(ControllerButton.Select) : (this.captureKey != KeyCode.None && GameplayInput.GetKeyDown(this.captureKey)))
			{
				if (ControllerInput.IsActive)
				{
					ControllerInput.ConsumeUIInput();
				}
				this.CapturePhoto();
			}
			if (this.lookAtCameraInput != null && this.lookAtCameraInput.GetButtonDown())
			{
				this.ToggleLookAtCamera();
			}
			if (this.enableScrollZoom)
			{
				this.HandleScrollZoom();
			}
			if (this.enablePanTilt)
			{
				this.HandlePanTilt();
			}
			this.UpdateMotorAudio();
		}

		// Token: 0x06005BA3 RID: 23459 RVA: 0x0023EF38 File Offset: 0x0023D138
		private void LateUpdate()
		{
			if (!this.IsViewing)
			{
				return;
			}
			CinemachineVirtualCamera currentCam = this.CurrentCam;
			if (currentCam == null || !this._hasZoomState)
			{
				return;
			}
			float num = 1f - Mathf.Exp(-this.zoomLerpSpeed * Time.unscaledDeltaTime);
			float num2 = Mathf.Lerp(currentCam.m_Lens.FieldOfView, this._targetFov, num);
			currentCam.m_Lens.FieldOfView = num2;
			this.ApplyPanTilt();
		}

		// Token: 0x06005BA4 RID: 23460 RVA: 0x0023EFAA File Offset: 0x0023D1AA
		public void Advance()
		{
			if (this.IsViewing)
			{
				this.SwitchToNextCamera();
				return;
			}
			this.TryEnterNearestCamera();
		}

		// Token: 0x06005BA5 RID: 23461 RVA: 0x0023EFC4 File Offset: 0x0023D1C4
		public bool TryEnterNearestCamera()
		{
			if (!this.CanBeginViewing())
			{
				return false;
			}
			if (this.requireProximityToEnter)
			{
				this.UpdateProximityInteraction();
			}
			if (this.securityCams.Count == 0)
			{
				if (this.debugLogs)
				{
					Debug.LogWarning("[SecurityCam] Nessuna VCam configurata.");
				}
				return false;
			}
			if (InteractablePromptUI.HasVisiblePromptOtherThan(this._proximityPrompt))
			{
				return false;
			}
			if (this.requireProximityToEnter && this._nearestInteractionIndex < 0)
			{
				return false;
			}
			int num = ((this._nearestInteractionIndex >= 0) ? this._nearestInteractionIndex : 0);
			this.EnterCamera(num);
			return this.IsViewing;
		}

		// Token: 0x06005BA6 RID: 23462 RVA: 0x0023F04C File Offset: 0x0023D24C
		public void SwitchToNextCamera()
		{
			if (!this.IsViewing || this.securityCams.Count == 0)
			{
				return;
			}
			this.EnterCamera((this._activeIndex + 1) % this.securityCams.Count);
		}

		// Token: 0x06005BA7 RID: 23463 RVA: 0x0023F080 File Offset: 0x0023D280
		public void EnterCamera(int index)
		{
			if (index < 0 || index >= this.securityCams.Count)
			{
				return;
			}
			bool flag = !this.IsViewing;
			if (flag && !this.CanBeginViewing())
			{
				return;
			}
			if (this.IsViewing && this._activeIndex < this.securityCams.Count && this.securityCams[this._activeIndex] != null)
			{
				this.securityCams[this._activeIndex].Priority = this.idlePriority;
				this.RestorePanRotation();
			}
			this._activeIndex = index;
			if (flag)
			{
				this.HideProximityPrompt();
				this.PushViewingGate();
				this.CaptureMainCameraLocal();
				this.EnableBrainForCctv();
				this.LockPlayerForCctv(true);
				this.SetCctvFxActive(true);
				this.ShowViewingActions();
			}
			CinemachineVirtualCamera cinemachineVirtualCamera = this.securityCams[this._activeIndex];
			if (cinemachineVirtualCamera != null)
			{
				cinemachineVirtualCamera.gameObject.SetActive(true);
				cinemachineVirtualCamera.Priority = this.activePriority;
				this._targetFov = Mathf.Clamp(cinemachineVirtualCamera.m_Lens.FieldOfView, this.minFov, this.maxFov);
				this._restoreFov = cinemachineVirtualCamera.m_Lens.FieldOfView;
				this._hasZoomState = true;
				this._camT = cinemachineVirtualCamera.transform;
				this._baseLocalRot = this._camT.localRotation;
				this._yaw = (this._pitch = (this._targetYaw = (this._targetPitch = 0f)));
				this._hasPanState = true;
			}
			if (this._lookingAtCamera)
			{
				this.ApplyLookAt(true);
			}
			this.UpdateCameraLabel();
			if (this.debugLogs)
			{
				Debug.Log(string.Concat(new string[]
				{
					"[SecurityCam] Enter CAM ",
					this._activeIndex.ToString(),
					" ('",
					this.ResolveLabel(this._activeIndex),
					"')"
				}));
			}
		}

		// Token: 0x06005BA8 RID: 23464 RVA: 0x0023F260 File Offset: 0x0023D460
		public void ExitToGameplay()
		{
			if (!this.IsViewing)
			{
				return;
			}
			this.CancelCapture();
			this.ApplyLookAt(false);
			this._lookingAtCamera = false;
			CinemachineVirtualCamera currentCam = this.CurrentCam;
			if (currentCam != null && this._hasZoomState)
			{
				currentCam.m_Lens.FieldOfView = this._restoreFov;
			}
			this._hasZoomState = false;
			this.RestorePanRotation();
			foreach (CinemachineVirtualCamera cinemachineVirtualCamera in this.securityCams)
			{
				if (cinemachineVirtualCamera != null)
				{
					cinemachineVirtualCamera.Priority = this.idlePriority;
				}
			}
			this._activeIndex = -1;
			this.SetCctvFxActive(false);
			this.LockPlayerForCctv(false);
			this.HideViewingActions();
			this.ReleaseViewingGate();
			this.DisableBrain();
			if (this._hasMainCamLocal && this._mainCamT != null)
			{
				this._mainCamT.localPosition = this._mainCamLocalPos;
				this._mainCamT.localRotation = this._mainCamLocalRot;
			}
			if (this._tpCamera == null)
			{
				this._tpCamera = Object.FindObjectOfType<vThirdPersonCamera>();
			}
			if (this._tpCamera != null)
			{
				this._tpCamera.RemoveLockTarget();
				this._tpCamera.ResetAngle();
			}
			if (this.debugLogs)
			{
				Debug.Log("[SecurityCam] Exit -> handoff a vThirdPersonCamera.");
			}
		}

		// Token: 0x06005BA9 RID: 23465 RVA: 0x0023F3C4 File Offset: 0x0023D5C4
		private void SetupInteractionUi()
		{
			if (this._proximityConfig == null)
			{
				this._proximityConfig = ScriptableObject.CreateInstance<InteractionConfigSO>();
				this._proximityConfig.name = "CCTV Proximity (Runtime)";
				this._proximityConfig.objectName = "Surveillance Camera";
				this._proximityConfig.mainActionName = "View cameras";
				this._proximityConfig.mainKeyDisplayLabel = ((this.enterKey == KeyCode.None) ? string.Empty : this.enterKey.ToString());
			}
			if (this._viewActionsConfig == null)
			{
				this._viewActionsConfig = ScriptableObject.CreateInstance<InteractionConfigSO>();
				this._viewActionsConfig.name = "CCTV Actions (Runtime)";
				if (this.captureKey != KeyCode.None)
				{
					this._viewActionsConfig.usableActions.Add(new UsableAction
					{
						keyDisplayLabel = this.captureKey.ToString(),
						actionName = "Take photo"
					});
				}
				if (this.enablePanTilt)
				{
					this._viewActionsConfig.usableActions.Add(new UsableAction
					{
						keyDisplayLabel = "Arrow keys",
						actionName = "Move camera"
					});
				}
				if (this.enableScrollZoom)
				{
					this._viewActionsConfig.usableActions.Add(new UsableAction
					{
						keyDisplayLabel = "Scroll",
						actionName = "Zoom"
					});
				}
				this._viewActionsConfig.usableActions.Add(new UsableAction
				{
					keyDisplayLabel = this.cycleKey.ToString(),
					actionName = "Switch camera"
				});
				this._viewActionsConfig.usableActions.Add(new UsableAction
				{
					keyDisplayLabel = "N",
					actionName = "Look at camera"
				});
				this._viewActionsConfig.usableActions.Add(new UsableAction
				{
					keyDisplayLabel = this.exitKey.ToString(),
					actionName = "Exit CCTV"
				});
			}
			ControllerContextBindings.SetLabel(this._viewActionsConfig, this.cycleKey.ToString(), "RB", false);
			ControllerContextBindings.SetLabel(this._viewActionsConfig, this.exitKey.ToString(), "B", false);
			ControllerContextBindings.SetLabel(this._viewActionsConfig, "N", ControllerInput.ForKey(KeyCode.Space), false);
			ControllerContextBindings.SetLabel(this._viewActionsConfig, this.captureKey.ToString(), "View", false);
			ControllerContextBindings.SetLabel(this._viewActionsConfig, "Arrow keys", "RS", false);
			ControllerContextBindings.SetLabel(this._viewActionsConfig, "Scroll", "LT + RS", false);
			if (this._proximityPrompt == null && this.proximityPromptPrefab != null)
			{
				this._proximityPrompt = Object.Instantiate<InteractablePromptUI>(this.proximityPromptPrefab, base.transform);
				this._proximityPrompt.name = "CCTV Interaction Prompt (Runtime)";
				this._proximityPrompt.Hide();
			}
		}

		// Token: 0x06005BAA RID: 23466 RVA: 0x0023F6A8 File Offset: 0x0023D8A8
		private void UpdateProximityInteraction()
		{
			this._nearestInteractionIndex = -1;
			if (this.IsViewing || !this.requireProximityToEnter)
			{
				this.HideProximityPrompt();
				return;
			}
			if (this._playerInput == null)
			{
				this._playerInput = Object.FindObjectOfType<vThirdPersonInput>();
			}
			if (!this.CanBeginViewing())
			{
				this.HideProximityPrompt();
				return;
			}
			float num = this.interactionDistance * this.interactionDistance;
			Vector3 position = this._playerInput.transform.position;
			for (int i = 0; i < this.securityCams.Count; i++)
			{
				CinemachineVirtualCamera cinemachineVirtualCamera = this.securityCams[i];
				if (!(cinemachineVirtualCamera == null) && cinemachineVirtualCamera.gameObject.activeInHierarchy)
				{
					Vector3 vector = cinemachineVirtualCamera.transform.position - position;
					if (Mathf.Abs(vector.y) <= this.maxVerticalInteractionDistance)
					{
						vector.y = 0f;
						float sqrMagnitude = vector.sqrMagnitude;
						if (sqrMagnitude <= num)
						{
							num = sqrMagnitude;
							this._nearestInteractionIndex = i;
						}
					}
				}
			}
			if (this._nearestInteractionIndex < 0)
			{
				this.HideProximityPrompt();
				return;
			}
			this.SetupInteractionUi();
			if (this._proximityPrompt == null)
			{
				return;
			}
			if (InteractablePromptUI.HasVisiblePromptOtherThan(this._proximityPrompt))
			{
				this._nearestInteractionIndex = -1;
				this.HideProximityPrompt();
				return;
			}
			Vector3 vector2 = this.securityCams[this._nearestInteractionIndex].transform.position + this.promptWorldOffset;
			vector2.y = Mathf.Min(vector2.y, position.y + 1.6f);
			this._proximityPrompt.transform.position = vector2;
			if (this._promptCameraIndex != this._nearestInteractionIndex)
			{
				this._proximityPrompt.Show(this._proximityConfig);
				this._promptCameraIndex = this._nearestInteractionIndex;
			}
		}

		// Token: 0x06005BAB RID: 23467 RVA: 0x0023F866 File Offset: 0x0023DA66
		private void HideProximityPrompt()
		{
			if (this._proximityPrompt != null)
			{
				this._proximityPrompt.Hide();
			}
			this._promptCameraIndex = -1;
		}

		// Token: 0x06005BAC RID: 23468 RVA: 0x0023F888 File Offset: 0x0023DA88
		private bool CanBeginViewing()
		{
			return this._playerInput != null && !this._playerInput.lockInput && !GraphicsSettingsRuntimeUI.AnyOpen && PlayerActivityGate.CanStartWorldInteraction && !PlayerActivityGate.IsResourceOwned(PlayerContextResource.Camera | PlayerContextResource.ModalUI);
		}

		// Token: 0x06005BAD RID: 23469 RVA: 0x0023F8C0 File Offset: 0x0023DAC0
		private void ShowViewingActions()
		{
			this.SetupInteractionUi();
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (this._viewActionsHud != null && this._viewActionsHud != instance)
			{
				this._viewActionsHud.HideSupplemental(this);
			}
			this._viewActionsHud = instance;
			if (instance != null)
			{
				instance.ShowSupplemental(this, this._viewActionsConfig);
			}
		}

		// Token: 0x06005BAE RID: 23470 RVA: 0x0023F91E File Offset: 0x0023DB1E
		private void HideViewingActions()
		{
			if (this._viewActionsHud != null)
			{
				this._viewActionsHud.HideSupplemental(this);
			}
			this._viewActionsHud = null;
		}

		// Token: 0x06005BAF RID: 23471 RVA: 0x0023F944 File Offset: 0x0023DB44
		public void CapturePhoto()
		{
			if (!this.IsViewing || !base.isActiveAndEnabled || this._captureRoutine != null || Camera.main == null || Time.unscaledTime < this._nextCaptureTime || GameplayInputGate.BlocksGameplayInput || TextInputFocus.BlocksGameplayInput || PlayerActivityGate.IsResourceOwned(PlayerContextResource.ModalUI))
			{
				return;
			}
			this._nextCaptureTime = Time.unscaledTime + 0.4f;
			this._captureRoutine = base.StartCoroutine(this.CaptureAtEndOfFrame());
		}

		// Token: 0x06005BB0 RID: 23472 RVA: 0x0023F9BE File Offset: 0x0023DBBE
		private IEnumerator CaptureAtEndOfFrame()
		{
			yield return new WaitForEndOfFrame();
			try
			{
				if (!this.IsViewing || Camera.main == null)
				{
					yield break;
				}
				string text = this.SaveCctvFrame(Camera.main);
				if (!string.IsNullOrEmpty(text))
				{
					ToastService.Show("Photo saved: Media / CCTV / Photos", "CCTV", null, -1f, "prison_ui");
					if (this.debugLogs)
					{
						Debug.Log("[SecurityCam] Photo saved: " + text, this);
					}
				}
				yield break;
			}
			finally
			{
				this._captureRoutine = null;
			}
			yield break;
		}

		// Token: 0x06005BB1 RID: 23473 RVA: 0x0023F9CD File Offset: 0x0023DBCD
		private void CancelCapture()
		{
			if (this._captureRoutine == null)
			{
				return;
			}
			base.StopCoroutine(this._captureRoutine);
			this._captureRoutine = null;
		}

		// Token: 0x06005BB2 RID: 23474 RVA: 0x0023F9EC File Offset: 0x0023DBEC
		private string SaveCctvFrame(Camera camera)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(Mathf.Max(1, Screen.width), Mathf.Max(1, Screen.height), 24, RenderTextureFormat.ARGB32);
			RenderTexture targetTexture = camera.targetTexture;
			RenderTexture active = RenderTexture.active;
			UniversalAdditionalCameraData universalAdditionalCameraData = camera.GetUniversalAdditionalCameraData();
			AntialiasingMode antialiasing = universalAdditionalCameraData.antialiasing;
			Texture2D texture2D = null;
			string text;
			try
			{
				universalAdditionalCameraData.antialiasing = AntialiasingMode.None;
				camera.targetTexture = temporary;
				SecurityCameraController.RenderCctvFrame(camera, temporary, (this.cctvOverlay != null) ? this.cctvOverlay.GetComponentInChildren<CctvOverlayHud>(true) : null);
				RenderTexture.active = temporary;
				texture2D = new Texture2D(temporary.width, temporary.height, TextureFormat.RGB24, false);
				texture2D.ReadPixels(new Rect(0f, 0f, (float)temporary.width, (float)temporary.height), 0, 0, false);
				texture2D.Apply(false, false);
				text = PlayerMediaExport.WriteCctvPhoto(texture2D.EncodeToPNG(), "CCTV_" + DateTime.Now.ToString("yyyyMMdd_HHmmss_fff") + ".png");
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[SecurityCam] Could not save photo: " + ex.Message, this);
				ToastService.Show("Photo could not be saved", "CCTV", null, -1f, "prison_ui");
				text = null;
			}
			finally
			{
				camera.targetTexture = targetTexture;
				universalAdditionalCameraData.antialiasing = antialiasing;
				RenderTexture.active = active;
				RenderTexture.ReleaseTemporary(temporary);
				if (texture2D != null)
				{
					Object.Destroy(texture2D);
				}
			}
			return text;
		}

		// Token: 0x06005BB3 RID: 23475 RVA: 0x0023FB6C File Offset: 0x0023DD6C
		private static void RenderCctvFrame(Camera camera, RenderTexture target, CctvOverlayHud overlay)
		{
			Action <>9__1;
			SecurityCameraController.WithCaptureUiHidden(delegate
			{
				Camera camera2 = camera;
				Action action;
				if ((action = <>9__1) == null)
				{
					action = (<>9__1 = delegate
					{
						using (StreamerCensorRenderFeature.BeginCapture())
						{
							UniversalRenderPipeline.SingleCameraRequest singleCameraRequest = new UniversalRenderPipeline.SingleCameraRequest
							{
								destination = target
							};
							if (RenderPipeline.SupportsRenderRequest<UniversalRenderPipeline.SingleCameraRequest>(camera, singleCameraRequest))
							{
								RenderPipeline.SubmitRenderRequest<UniversalRenderPipeline.SingleCameraRequest>(camera, singleCameraRequest);
							}
							else
							{
								camera.Render();
							}
						}
					});
				}
				SecurityCameraController.WithCaptureVolumeStack(camera2, action);
			});
			if (overlay != null)
			{
				overlay.RenderCaptureOverlay(target, camera);
			}
		}

		// Token: 0x06005BB4 RID: 23476 RVA: 0x0023FBBC File Offset: 0x0023DDBC
		private static void WithCaptureVolumeStack(Camera camera, Action capture)
		{
			UniversalAdditionalCameraData universalAdditionalCameraData = camera.GetUniversalAdditionalCameraData();
			VolumeManager instance = VolumeManager.instance;
			VolumeStack stack = instance.stack;
			VolumeStack volumeStack = instance.CreateStack();
			try
			{
				instance.Update(volumeStack, (universalAdditionalCameraData.volumeTrigger != null) ? universalAdditionalCameraData.volumeTrigger : camera.transform, universalAdditionalCameraData.volumeLayerMask);
				instance.stack = volumeStack;
				capture();
			}
			finally
			{
				instance.stack = stack;
				instance.DestroyStack(volumeStack);
			}
		}

		// Token: 0x06005BB5 RID: 23477 RVA: 0x0023FC3C File Offset: 0x0023DE3C
		private static void WithCaptureUiHidden(Action capture)
		{
			List<Canvas> list = new List<Canvas>();
			try
			{
				foreach (Canvas canvas in Object.FindObjectsOfType<Canvas>(true))
				{
					if (!(canvas == null) && canvas.enabled)
					{
						list.Add(canvas);
						canvas.enabled = false;
					}
				}
				capture();
			}
			finally
			{
				foreach (Canvas canvas2 in list)
				{
					if (canvas2 != null)
					{
						canvas2.enabled = true;
					}
				}
			}
		}

		// Token: 0x06005BB6 RID: 23478 RVA: 0x0023FCEC File Offset: 0x0023DEEC
		private void PushViewingGate()
		{
			if (this._viewingGatePushed)
			{
				return;
			}
			PlayerActivityGate.Push("CCTV.Viewing", PlayerActivity.Interact | PlayerActivity.StanceSwitch | PlayerActivity.Inventory | PlayerActivity.Phone | PlayerActivity.DebugFly);
			this._viewingGatePushed = true;
		}

		// Token: 0x06005BB7 RID: 23479 RVA: 0x0023FD0D File Offset: 0x0023DF0D
		private void ReleaseViewingGate()
		{
			if (!this._viewingGatePushed)
			{
				return;
			}
			PlayerActivityGate.Pop("CCTV.Viewing");
			this._viewingGatePushed = false;
		}

		// Token: 0x06005BB8 RID: 23480 RVA: 0x0023FD2C File Offset: 0x0023DF2C
		private void ResolveReferences()
		{
			if (this.brain == null && Camera.main != null)
			{
				this.brain = Camera.main.GetComponent<CinemachineBrain>();
			}
			if (this._tpCamera == null)
			{
				this._tpCamera = Object.FindObjectOfType<vThirdPersonCamera>();
			}
			if (this._playerInput == null)
			{
				this._playerInput = Object.FindObjectOfType<vThirdPersonInput>();
			}
			if (this.playerHeadAim == null && this._playerInput != null)
			{
				this.playerHeadAim = this._playerInput.GetComponentInChildren<PlayerHeadAim>(true);
			}
			if (this.playerHeadAim == null)
			{
				this.playerHeadAim = Object.FindObjectOfType<PlayerHeadAim>();
			}
			if (this.playerEyeLookAt == null && this._playerInput != null)
			{
				this.playerEyeLookAt = this._playerInput.GetComponentInChildren<PlayerEyeLookAt>(true);
			}
			if (this.playerEyeLookAt == null)
			{
				this.playerEyeLookAt = Object.FindObjectOfType<PlayerEyeLookAt>();
			}
		}

		// Token: 0x17000F08 RID: 3848
		// (get) Token: 0x06005BB9 RID: 23481 RVA: 0x0023FE24 File Offset: 0x0023E024
		private CinemachineVirtualCamera CurrentCam
		{
			get
			{
				if (this._activeIndex < 0 || this._activeIndex >= this.securityCams.Count)
				{
					return null;
				}
				return this.securityCams[this._activeIndex];
			}
		}

		// Token: 0x06005BBA RID: 23482 RVA: 0x0023FE55 File Offset: 0x0023E055
		public void ToggleLookAtCamera()
		{
			this.SetLookAtCamera(!this._lookingAtCamera);
		}

		// Token: 0x06005BBB RID: 23483 RVA: 0x0023FE68 File Offset: 0x0023E068
		public void SetLookAtCamera(bool on)
		{
			if (!this.IsViewing)
			{
				on = false;
			}
			if (on)
			{
				Transform transform = this.ResolveLookTarget();
				if (this.playerHeadAim != null && transform != null && !this.playerHeadAim.IsTargetInFront(transform))
				{
					if (this.debugLogs)
					{
						Debug.Log("[SecurityCam] Look-at negato: Player non rivolto verso la cam.");
					}
					return;
				}
			}
			this._lookingAtCamera = on;
			this.ApplyLookAt(on);
			if (this.debugLogs)
			{
				Debug.Log("[SecurityCam] LookAtCamera = " + on.ToString());
			}
		}

		// Token: 0x06005BBC RID: 23484 RVA: 0x0023FEF0 File Offset: 0x0023E0F0
		private void ApplyLookAt(bool on)
		{
			if (on)
			{
				Transform transform = this.ResolveLookTarget();
				if (transform == null)
				{
					return;
				}
				if (this.playerHeadAim != null)
				{
					this.playerHeadAim.SetTarget(transform);
				}
				if (this.playerEyeLookAt != null)
				{
					this.playerEyeLookAt.ForceLookAt(transform);
					return;
				}
			}
			else
			{
				if (this.playerHeadAim != null)
				{
					this.playerHeadAim.ClearTarget();
				}
				if (this.playerEyeLookAt != null)
				{
					this.playerEyeLookAt.ClearForcedTarget();
				}
			}
		}

		// Token: 0x06005BBD RID: 23485 RVA: 0x0023FF78 File Offset: 0x0023E178
		private Transform ResolveLookTarget()
		{
			if (this.lookAtTargetOverride != null)
			{
				return this.lookAtTargetOverride;
			}
			if (!(Camera.main != null))
			{
				return null;
			}
			return Camera.main.transform;
		}

		// Token: 0x06005BBE RID: 23486 RVA: 0x0023FFA8 File Offset: 0x0023E1A8
		private void HandleScrollZoom()
		{
			float axis = GameplayInput.GetAxis("Mouse ScrollWheel");
			if (Mathf.Abs(axis) < 0.0001f)
			{
				return;
			}
			float targetFov = this._targetFov;
			this._targetFov = Mathf.Clamp(this._targetFov - axis * this.zoomStep * 10f, this.minFov, this.maxFov);
			if (!Mathf.Approximately(targetFov, this._targetFov))
			{
				this._zoomAudioHold = 0.18f;
			}
			if (this.debugLogs)
			{
				Debug.Log("[SecurityCam] Zoom targetFov=" + this._targetFov.ToString("F1"));
			}
		}

		// Token: 0x06005BBF RID: 23487 RVA: 0x00240040 File Offset: 0x0023E240
		private void HandlePanTilt()
		{
			if (!this._hasPanState)
			{
				return;
			}
			float num = 0f;
			float num2 = 0f;
			if (GameplayInput.GetKey(KeyCode.LeftArrow))
			{
				num -= 1f;
			}
			if (GameplayInput.GetKey(KeyCode.RightArrow))
			{
				num += 1f;
			}
			if (GameplayInput.GetKey(KeyCode.UpArrow))
			{
				num2 += 1f;
			}
			if (GameplayInput.GetKey(KeyCode.DownArrow))
			{
				num2 -= 1f;
			}
			if (ControllerInput.IsActive && !ControllerWorldPointer.BlocksContextInput)
			{
				Vector2 vector = (ControllerInput.GetButton(ControllerButton.LeftTrigger) ? Vector2.zero : ControllerInput.Look);
				num = vector.x;
				num2 = vector.y;
			}
			if (this.invertVertical)
			{
				num2 = -num2;
			}
			if (num != 0f)
			{
				float targetYaw = this._targetYaw;
				this._targetYaw = Mathf.Clamp(this._targetYaw + num * this.panSpeed * Time.unscaledDeltaTime, -this.maxYaw, this.maxYaw);
				if (!Mathf.Approximately(targetYaw, this._targetYaw))
				{
					this._wantH = true;
				}
			}
			if (num2 != 0f)
			{
				float targetPitch = this._targetPitch;
				this._targetPitch = Mathf.Clamp(this._targetPitch + num2 * this.panSpeed * Time.unscaledDeltaTime, -this.maxPitchDown, this.maxPitchUp);
				if (!Mathf.Approximately(targetPitch, this._targetPitch))
				{
					this._wantV = true;
				}
			}
		}

		// Token: 0x06005BC0 RID: 23488 RVA: 0x0024018C File Offset: 0x0023E38C
		private void ApplyPanTilt()
		{
			if (!this.enablePanTilt || !this._hasPanState || this._camT == null)
			{
				return;
			}
			float num = 1f - Mathf.Exp(-this.panLerpSpeed * Time.unscaledDeltaTime);
			this._yaw = Mathf.Lerp(this._yaw, this._targetYaw, num);
			this._pitch = Mathf.Lerp(this._pitch, this._targetPitch, num);
			this._camT.localRotation = this._baseLocalRot * Quaternion.Euler(-this._pitch, this._yaw, 0f);
		}

		// Token: 0x06005BC1 RID: 23489 RVA: 0x00240230 File Offset: 0x0023E430
		private void RestorePanRotation()
		{
			if (this._hasPanState && this._camT != null)
			{
				this._camT.localRotation = this._baseLocalRot;
			}
			this._yaw = (this._pitch = (this._targetYaw = (this._targetPitch = 0f)));
			this._hasPanState = false;
			this._camT = null;
		}

		// Token: 0x06005BC2 RID: 23490 RVA: 0x00240298 File Offset: 0x0023E498
		private void UpdateMotorAudio()
		{
			bool isViewing = this.IsViewing;
			this.FadeSource(this._audioH, isViewing && this._wantH);
			this.FadeSource(this._audioV, isViewing && this._wantV);
			this.FadeSource(this._audioZoom, isViewing && this._zoomAudioHold > 0f);
		}

		// Token: 0x06005BC3 RID: 23491 RVA: 0x002402FC File Offset: 0x0023E4FC
		private void FadeSource(AudioSource src, bool on)
		{
			if (src == null)
			{
				return;
			}
			float num = (on ? this.motorVolume : 0f);
			if (on && !src.isPlaying)
			{
				src.Play();
			}
			float num2 = 1f - Mathf.Exp(-this.audioFadeSpeed * Time.unscaledDeltaTime);
			src.volume = Mathf.Lerp(src.volume, num, num2);
			if (!on && src.isPlaying && src.volume < 0.001f)
			{
				src.volume = 0f;
				src.Stop();
			}
		}

		// Token: 0x06005BC4 RID: 23492 RVA: 0x0024038C File Offset: 0x0023E58C
		private void CollectVCamsIfNeeded()
		{
			if (this.securityCams.Count > 0 || !this.autoCollectChildVCams)
			{
				return;
			}
			CinemachineVirtualCamera[] componentsInChildren = base.GetComponentsInChildren<CinemachineVirtualCamera>(true);
			this.securityCams.AddRange(componentsInChildren);
			if (this.debugLogs)
			{
				Debug.Log("[SecurityCam] Auto-collected " + this.securityCams.Count.ToString() + " VCam figlie.");
			}
		}

		// Token: 0x06005BC5 RID: 23493 RVA: 0x002403F4 File Offset: 0x0023E5F4
		private void CaptureMainCameraLocal()
		{
			if (this._hasMainCamLocal)
			{
				return;
			}
			if (Camera.main == null)
			{
				return;
			}
			this._mainCamT = Camera.main.transform;
			this._mainCamLocalPos = this._mainCamT.localPosition;
			this._mainCamLocalRot = this._mainCamT.localRotation;
			this._hasMainCamLocal = true;
		}

		// Token: 0x06005BC6 RID: 23494 RVA: 0x00240454 File Offset: 0x0023E654
		private void EnableBrainForCctv()
		{
			if (this.brain == null && Camera.main != null)
			{
				this.brain = Camera.main.GetComponent<CinemachineBrain>();
			}
			if (this.brain == null)
			{
				return;
			}
			this.brain.m_DefaultBlend.m_Style = ((this.blendDuration > 0f) ? CinemachineBlendDefinition.Style.EaseInOut : CinemachineBlendDefinition.Style.Cut);
			this.brain.m_DefaultBlend.m_Time = this.blendDuration;
			this.brain.enabled = true;
		}

		// Token: 0x06005BC7 RID: 23495 RVA: 0x002404E0 File Offset: 0x0023E6E0
		private void DisableBrain()
		{
			if (this.brain == null && Camera.main != null)
			{
				this.brain = Camera.main.GetComponent<CinemachineBrain>();
			}
			if (this.brain != null)
			{
				this.brain.enabled = false;
			}
		}

		// Token: 0x06005BC8 RID: 23496 RVA: 0x00240534 File Offset: 0x0023E734
		private void LockPlayerForCctv(bool locked)
		{
			if (this._playerInput == null)
			{
				this._playerInput = Object.FindObjectOfType<vThirdPersonInput>();
			}
			if (this._playerInput == null)
			{
				return;
			}
			this._playerInput.SetLockCameraInput(locked);
			if (!this.allowMovementWhileViewing)
			{
				this._playerInput.SetLockAllInput(locked);
			}
		}

		// Token: 0x06005BC9 RID: 23497 RVA: 0x00240589 File Offset: 0x0023E789
		private void SetCctvFxActive(bool active)
		{
			if (this.cctvOverlay != null)
			{
				this.cctvOverlay.SetActive(active);
			}
			if (this.cctvVolumeObject != null)
			{
				this.cctvVolumeObject.SetActive(active);
			}
			this.SetGameplayHudHidden(active);
		}

		// Token: 0x06005BCA RID: 23498 RVA: 0x002405C8 File Offset: 0x0023E7C8
		private void SetGameplayHudHidden(bool hide)
		{
			if (this.hudToHide == null || this.hudToHide.Count == 0)
			{
				return;
			}
			if (hide)
			{
				this._hudPrevActive.Clear();
				using (List<GameObject>.Enumerator enumerator = this.hudToHide.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						GameObject gameObject = enumerator.Current;
						this._hudPrevActive.Add(gameObject != null && gameObject.activeSelf);
						if (gameObject != null)
						{
							gameObject.SetActive(false);
						}
					}
					return;
				}
			}
			if (this._hudPrevActive.Count == 0)
			{
				return;
			}
			for (int i = 0; i < this.hudToHide.Count; i++)
			{
				GameObject gameObject2 = this.hudToHide[i];
				if (!(gameObject2 == null))
				{
					bool flag = i >= this._hudPrevActive.Count || this._hudPrevActive[i];
					gameObject2.SetActive(flag);
				}
			}
			this._hudPrevActive.Clear();
		}

		// Token: 0x06005BCB RID: 23499 RVA: 0x002406D4 File Offset: 0x0023E8D4
		private void UpdateCameraLabel()
		{
			if (this.cameraLabelTarget == null)
			{
				return;
			}
			Text component = this.cameraLabelTarget.GetComponent<Text>();
			if (component != null)
			{
				LocalizedLegacyGameText.BindComputed(component, () => this.CameraLabelPresentation(this._activeIndex)).FitTranslationToRectangle();
				return;
			}
			TMP_Text component2 = this.cameraLabelTarget.GetComponent<TMP_Text>();
			if (component2 != null)
			{
				LocalizedGameText.BindComputed(component2, () => this.CameraLabelPresentation(this._activeIndex)).FitTranslationToRectangle(false);
				return;
			}
			string text = this.ResolveLabel(this._activeIndex);
			foreach (Component component3 in this.cameraLabelTarget.GetComponents<Component>())
			{
				if (!(component3 == null))
				{
					PropertyInfo property = component3.GetType().GetProperty("text");
					if (property != null && property.PropertyType == typeof(string) && property.CanWrite)
					{
						property.SetValue(component3, text);
						return;
					}
				}
			}
		}

		// Token: 0x06005BCC RID: 23500 RVA: 0x002407D0 File Offset: 0x0023E9D0
		private string CameraLabelPresentation(int index)
		{
			if (index >= 0 && index < this.cameraLabels.Count && !string.IsNullOrEmpty(this.cameraLabels[index]))
			{
				return this.cameraLabels[index];
			}
			return GameLocalization.FormatSource("prison_ui", "CAM {0}", new object[] { (index + 1).ToString("00") });
		}

		// Token: 0x06005BCD RID: 23501 RVA: 0x00240838 File Offset: 0x0023EA38
		private string ResolveLabel(int index)
		{
			if (index >= 0 && index < this.cameraLabels.Count && !string.IsNullOrEmpty(this.cameraLabels[index]))
			{
				return this.cameraLabels[index];
			}
			return "CAM " + (index + 1).ToString("00");
		}

		// Token: 0x04005CCA RID: 23754
		[Header("Security VCams")]
		[Tooltip("Le VCam delle telecamere di sicurezza, nell'ordine di ciclo. Se vuoto, vengono raccolte automaticamente tra i figli (in ordine di gerarchia) allo Start.")]
		[SerializeField]
		private List<CinemachineVirtualCamera> securityCams = new List<CinemachineVirtualCamera>();

		// Token: 0x04005CCB RID: 23755
		[Tooltip("Se true e la lista sopra e' vuota, raccoglie le VCam figlie automaticamente.")]
		[SerializeField]
		private bool autoCollectChildVCams = true;

		// Token: 0x04005CCC RID: 23756
		[Header("Input")]
		[Tooltip("Tasto per passare alla CCTV successiva mentre la modalita' e' gia' attiva.")]
		[SerializeField]
		private KeyCode cycleKey = KeyCode.Tab;

		// Token: 0x04005CCD RID: 23757
		[Tooltip("Tasto di interazione per entrare nella CCTV quando Ciel e' vicina a una camera.")]
		[SerializeField]
		private KeyCode enterKey = KeyCode.E;

		// Token: 0x04005CCE RID: 23758
		[Tooltip("Tasto per uscire dalla modalita' CCTV e tornare al gameplay.")]
		[SerializeField]
		private KeyCode exitKey = KeyCode.E;

		// Token: 0x04005CCF RID: 23759
		[Tooltip("Salva su disco il feed CCTV con cornice e dati camera, senza HUD di gameplay o import nelle gallerie.")]
		[SerializeField]
		private KeyCode captureKey = KeyCode.P;

		// Token: 0x04005CD0 RID: 23760
		[Header("Proximity Interaction")]
		[SerializeField]
		private bool requireProximityToEnter = true;

		// Token: 0x04005CD1 RID: 23761
		[Min(0.5f)]
		[SerializeField]
		private float interactionDistance = 1.35f;

		// Token: 0x04005CD2 RID: 23762
		[Min(0.5f)]
		[SerializeField]
		private float maxVerticalInteractionDistance = 3f;

		// Token: 0x04005CD3 RID: 23763
		[Tooltip("Prefab del prompt world-space condiviso. Una sola istanza runtime segue la camera CCTV piu' vicina.")]
		[SerializeField]
		private InteractablePromptUI proximityPromptPrefab;

		// Token: 0x04005CD4 RID: 23764
		[SerializeField]
		private Vector3 promptWorldOffset = new Vector3(0f, 0.35f, 0f);

		// Token: 0x04005CD5 RID: 23765
		[Tooltip("Se true il Player puo' continuare a muoversi mentre guarda la CCTV (viene bloccato solo l'input camera). Se false viene bloccato tutto l'input.")]
		[SerializeField]
		private bool allowMovementWhileViewing = true;

		// Token: 0x04005CD6 RID: 23766
		[Header("Cinemachine")]
		[Tooltip("Brain sulla MainCamera. Se null viene preso da Camera.main. Tenuto disabilitato in gameplay (Invector controlla la MainCamera) e abilitato durante la CCTV.")]
		[SerializeField]
		private CinemachineBrain brain;

		// Token: 0x04005CD7 RID: 23767
		[Tooltip("Priority assegnata alla VCam attualmente live (deve battere ogni altra VCam).")]
		[SerializeField]
		private int activePriority = 100;

		// Token: 0x04005CD8 RID: 23768
		[Tooltip("Priority delle VCam non attive (e di tutte all'uscita).")]
		[SerializeField]
		private int idlePriority;

		// Token: 0x04005CD9 RID: 23769
		[Tooltip("Durata del blend tra una camera e l'altra (e in ingresso). 0 = taglio netto.")]
		[SerializeField]
		private float blendDuration = 0.4f;

		// Token: 0x04005CDA RID: 23770
		[Header("CCTV FX (opzionali)")]
		[Tooltip("Root dell'overlay UI CCTV (Canvas con bordo, CAM xx, REC, scanline). Attivato all'ingresso, disattivato all'uscita.")]
		[SerializeField]
		private GameObject cctvOverlay;

		// Token: 0x04005CDB RID: 23771
		[Tooltip("GameObject che porta il Volume URP con gli effetti CCTV (grana, color adjust, vignette...). Attivato all'ingresso, disattivato all'uscita.")]
		[SerializeField]
		private GameObject cctvVolumeObject;

		// Token: 0x04005CDC RID: 23772
		[Tooltip("Componente Text/TMP (qualsiasi) il cui testo viene impostato all'etichetta della camera corrente. Opzionale: lasciare null se l'overlay e' statico.")]
		[SerializeField]
		private GameObject cameraLabelTarget;

		// Token: 0x04005CDD RID: 23773
		[Tooltip("Etichette per ogni camera (es. 'CAM 01'). Se la lista e' piu' corta delle camere, le mancanti usano 'CAM {indice+1:00}'.")]
		[SerializeField]
		private List<string> cameraLabels = new List<string>();

		// Token: 0x04005CDE RID: 23774
		[Tooltip("GameObject dell'HUD di gameplay da NASCONDERE mentre si guarda una CCTV (BPM, minimappa, prompt azioni...). Vengono disattivati all'ingresso e ripristinati allo stato che avevano all'uscita.")]
		[SerializeField]
		private List<GameObject> hudToHide = new List<GameObject>();

		// Token: 0x04005CDF RID: 23775
		[Header("Look At Camera (solo in CCTV)")]
		[Tooltip("Input per attivare/disattivare lo sguardo del Player verso la camera CCTV attiva (toggle). Funziona solo mentre si e' in CCTV e il Player e' rivolto verso la cam.")]
		[SerializeField]
		private GenericInput lookAtCameraInput = new GenericInput("Space", "Y", "Y");

		// Token: 0x04005CE0 RID: 23776
		[Tooltip("Componente che ruota la testa del Player verso un target. Se null, viene cercato sul Player (vThirdPersonInput) allo Start.")]
		[SerializeField]
		private PlayerHeadAim playerHeadAim;

		// Token: 0x04005CE1 RID: 23777
		[Tooltip("Eye-look del Player (occhi). Quando si guarda la cam, gli occhi la seguono via ForceLookAt. Se null, viene cercato sul Player.")]
		[SerializeField]
		private PlayerEyeLookAt playerEyeLookAt;

		// Token: 0x04005CE2 RID: 23778
		[Tooltip("Transform usato come bersaglio dello sguardo. Se null usa la MainCamera (cioe' il punto di vista della VCam CCTV live).")]
		[SerializeField]
		private Transform lookAtTargetOverride;

		// Token: 0x04005CE3 RID: 23779
		[Header("Zoom (rotella mouse, solo in CCTV)")]
		[Tooltip("Se true, la rotella del mouse regola il FOV della VCam CCTV attiva (zoom in/out).")]
		[SerializeField]
		private bool enableScrollZoom = true;

		// Token: 0x04005CE4 RID: 23780
		[Tooltip("FOV minimo (zoom IN massimo).")]
		[Range(5f, 60f)]
		[SerializeField]
		private float minFov = 25f;

		// Token: 0x04005CE5 RID: 23781
		[Tooltip("FOV massimo (zoom OUT massimo).")]
		[Range(40f, 120f)]
		[SerializeField]
		private float maxFov = 90f;

		// Token: 0x04005CE6 RID: 23782
		[Tooltip("Gradi di FOV per tacca di rotella (negativo perche' scroll su = zoom in).")]
		[SerializeField]
		private float zoomStep = 8f;

		// Token: 0x04005CE7 RID: 23783
		[Tooltip("Velocita' di smoothing dello zoom (piu' alto = piu' rapido).")]
		[Range(1f, 20f)]
		[SerializeField]
		private float zoomLerpSpeed = 10f;

		// Token: 0x04005CE8 RID: 23784
		[Header("Pan/Tilt (frecce tastiera, solo in CCTV)")]
		[Tooltip("Se true, le frecce della tastiera permettono di orientare la VCam CCTV attiva entro limiti molto stringenti (panoramica orizzontale e verticale).")]
		[SerializeField]
		private bool enablePanTilt = true;

		// Token: 0x04005CE9 RID: 23785
		[Tooltip("Velocita' di rotazione in gradi/secondo quando si tiene premuta una freccia.")]
		[Range(5f, 120f)]
		[SerializeField]
		private float panSpeed = 35f;

		// Token: 0x04005CEA RID: 23786
		[Tooltip("Escursione orizzontale massima (in gradi) a SINISTRA e a DESTRA rispetto all'orientamento di base della camera. Vincolo molto stretto di default.")]
		[Range(0f, 60f)]
		[SerializeField]
		private float maxYaw = 20f;

		// Token: 0x04005CEB RID: 23787
		[Tooltip("Escursione verticale massima verso l'ALTO (in gradi) rispetto all'orientamento di base.")]
		[Range(0f, 45f)]
		[SerializeField]
		private float maxPitchUp = 8f;

		// Token: 0x04005CEC RID: 23788
		[Tooltip("Escursione verticale massima verso il BASSO (in gradi) rispetto all'orientamento di base.")]
		[Range(0f, 45f)]
		[SerializeField]
		private float maxPitchDown = 12f;

		// Token: 0x04005CED RID: 23789
		[Tooltip("Velocita' di smoothing del pan/tilt (piu' alto = piu' reattivo). Da' il classico movimento 'pesante' delle telecamere motorizzate.")]
		[Range(1f, 20f)]
		[SerializeField]
		private float panLerpSpeed = 8f;

		// Token: 0x04005CEE RID: 23790
		[Tooltip("Se true inverte l'asse verticale (freccia su = guarda giu').")]
		[SerializeField]
		private bool invertVertical;

		// Token: 0x04005CEF RID: 23791
		[Header("Audio (solo in CCTV)")]
		[Tooltip("Suono del motore mentre si pana in orizzontale (frecce sinistra/destra). Riprodotto in loop a basso volume mentre si tiene premuta una freccia orizzontale.")]
		[SerializeField]
		private AudioClip horizontalMoveClip;

		// Token: 0x04005CF0 RID: 23792
		[Tooltip("Suono del motore mentre si inclina in verticale (frecce su/giu'). Riprodotto in loop a basso volume mentre si tiene premuta una freccia verticale.")]
		[SerializeField]
		private AudioClip verticalMoveClip;

		// Token: 0x04005CF1 RID: 23793
		[Tooltip("Suono del motore di zoom (rotella mouse). Riprodotto in loop a basso volume mentre si sta zoomando, si ferma poco dopo l'ultima tacca di rotella.")]
		[SerializeField]
		private AudioClip zoomClip;

		// Token: 0x04005CF2 RID: 23794
		[Tooltip("Volume dei suoni del motore (tenuto basso: non devono essere invadenti).")]
		[Range(0f, 1f)]
		[SerializeField]
		private float motorVolume = 0.25f;

		// Token: 0x04005CF3 RID: 23795
		[Tooltip("Velocita' di fade-in/out del volume dei motori (piu' alto = piu' rapido). Evita stacchi secchi quando si preme/rilascia una freccia.")]
		[Range(1f, 30f)]
		[SerializeField]
		private float audioFadeSpeed = 12f;

		// Token: 0x04005CF4 RID: 23796
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04005CF5 RID: 23797
		private int _activeIndex = -1;

		// Token: 0x04005CF6 RID: 23798
		private vThirdPersonCamera _tpCamera;

		// Token: 0x04005CF7 RID: 23799
		private vThirdPersonInput _playerInput;

		// Token: 0x04005CF8 RID: 23800
		private Transform _mainCamT;

		// Token: 0x04005CF9 RID: 23801
		private Vector3 _mainCamLocalPos;

		// Token: 0x04005CFA RID: 23802
		private Quaternion _mainCamLocalRot;

		// Token: 0x04005CFB RID: 23803
		private bool _hasMainCamLocal;

		// Token: 0x04005CFC RID: 23804
		private bool _lookingAtCamera;

		// Token: 0x04005CFD RID: 23805
		private float _targetFov;

		// Token: 0x04005CFE RID: 23806
		private float _restoreFov;

		// Token: 0x04005CFF RID: 23807
		private bool _hasZoomState;

		// Token: 0x04005D00 RID: 23808
		private Transform _camT;

		// Token: 0x04005D01 RID: 23809
		private Quaternion _baseLocalRot;

		// Token: 0x04005D02 RID: 23810
		private float _yaw;

		// Token: 0x04005D03 RID: 23811
		private float _pitch;

		// Token: 0x04005D04 RID: 23812
		private float _targetYaw;

		// Token: 0x04005D05 RID: 23813
		private float _targetPitch;

		// Token: 0x04005D06 RID: 23814
		private bool _hasPanState;

		// Token: 0x04005D07 RID: 23815
		private AudioSource _audioH;

		// Token: 0x04005D08 RID: 23816
		private AudioSource _audioV;

		// Token: 0x04005D09 RID: 23817
		private AudioSource _audioZoom;

		// Token: 0x04005D0A RID: 23818
		private bool _wantH;

		// Token: 0x04005D0B RID: 23819
		private bool _wantV;

		// Token: 0x04005D0C RID: 23820
		private bool _wantZoom;

		// Token: 0x04005D0D RID: 23821
		private float _zoomAudioHold;

		// Token: 0x04005D0E RID: 23822
		private readonly List<bool> _hudPrevActive = new List<bool>();

		// Token: 0x04005D0F RID: 23823
		private const string ViewingGateReason = "CCTV.Viewing";

		// Token: 0x04005D10 RID: 23824
		private InteractablePromptUI _proximityPrompt;

		// Token: 0x04005D11 RID: 23825
		private InteractionConfigSO _proximityConfig;

		// Token: 0x04005D12 RID: 23826
		private InteractionConfigSO _viewActionsConfig;

		// Token: 0x04005D13 RID: 23827
		private int _nearestInteractionIndex = -1;

		// Token: 0x04005D14 RID: 23828
		private int _promptCameraIndex = -1;

		// Token: 0x04005D15 RID: 23829
		private bool _viewingGatePushed;

		// Token: 0x04005D16 RID: 23830
		private Coroutine _captureRoutine;

		// Token: 0x04005D17 RID: 23831
		private float _nextCaptureTime;

		// Token: 0x04005D18 RID: 23832
		private InteractionHUDController _viewActionsHud;
	}
}
