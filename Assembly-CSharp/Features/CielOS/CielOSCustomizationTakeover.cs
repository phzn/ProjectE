using System;
using System.Collections.Generic;
using Bozo.AnimeCharacters;
using Features.Rank;
using Features.RenderingPrecision;
using Pathfinding;
using Pathfinding.RVO;
using ProjectExhibitionist.CharacterCustomization.Core;
using ProjectExhibitionist.CharacterCustomization.Core.Runtime;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Features.CielOS
{
	// Token: 0x02001384 RID: 4996
	[DefaultExecutionOrder(-300)]
	public class CielOSCustomizationTakeover : MonoBehaviour
	{
		// Token: 0x170018AE RID: 6318
		// (get) Token: 0x06009ABE RID: 39614 RVA: 0x003F48C2 File Offset: 0x003F2AC2
		// (set) Token: 0x06009ABF RID: 39615 RVA: 0x003F48CA File Offset: 0x003F2ACA
		public bool IsActive { get; private set; }

		// Token: 0x170018AF RID: 6319
		// (get) Token: 0x06009AC0 RID: 39616 RVA: 0x003F48D3 File Offset: 0x003F2AD3
		// (set) Token: 0x06009AC1 RID: 39617 RVA: 0x003F48DB File Offset: 0x003F2ADB
		public bool IsWindowedActive { get; private set; }

		// Token: 0x14000113 RID: 275
		// (add) Token: 0x06009AC2 RID: 39618 RVA: 0x003F48E4 File Offset: 0x003F2AE4
		// (remove) Token: 0x06009AC3 RID: 39619 RVA: 0x003F491C File Offset: 0x003F2B1C
		public event Action WindowCloseRequested;

		// Token: 0x06009AC4 RID: 39620 RVA: 0x003F4951 File Offset: 0x003F2B51
		private void OnEnable()
		{
			WorldOrigin.Shifted += this.ApplyWorldOriginShift;
			if (this.returnNavigator != null)
			{
				this.returnNavigator.ReturnRequested += this.HandleReturnRequested;
			}
		}

		// Token: 0x06009AC5 RID: 39621 RVA: 0x003F498C File Offset: 0x003F2B8C
		private void OnDisable()
		{
			WorldOrigin.Shifted -= this.ApplyWorldOriginShift;
			if (this.returnNavigator != null)
			{
				this.returnNavigator.ReturnRequested -= this.HandleReturnRequested;
			}
			this.RestoreAuxiliaryCustomizationCameras();
			this.RestoreStageCamera();
			this.RestoreStageAudioListener();
			this.RestoreSuspendedSceneRoots();
			this.RestorePlayerMotion();
		}

		// Token: 0x06009AC6 RID: 39622 RVA: 0x003F49ED File Offset: 0x003F2BED
		private void ApplyWorldOriginShift(Vector3 delta)
		{
			if (this._playerMotionFrozen)
			{
				this._playerPosition += delta;
			}
		}

		// Token: 0x06009AC7 RID: 39623 RVA: 0x003F4A09 File Offset: 0x003F2C09
		private void Update()
		{
			this.MaintainFrozenPlayerPose();
			if (!this.IsActive)
			{
				return;
			}
			if (this.returnNavigator != null && EscapeInputCoordinator.TryConsume(this.exitKey, this))
			{
				this.returnNavigator.ReturnToSourceScene();
			}
		}

		// Token: 0x06009AC8 RID: 39624 RVA: 0x003F4A44 File Offset: 0x003F2C44
		public bool OpenWindowed(RectTransform uiHost, RawImage stageView)
		{
			if (this.IsActive || this.IsWindowedActive)
			{
				return false;
			}
			if (this.customizationRig == null || this.uiRoot == null || this.stageCamera == null || uiHost == null)
			{
				Debug.LogWarning("[CielOSCustomizationTakeover] OpenWindowed: riferimenti mancanti " + string.Format("(rig:{0} uiRoot:{1} stageCamera:{2} host:{3}).", new object[]
				{
					this.customizationRig != null,
					this.uiRoot != null,
					this.stageCamera != null,
					uiHost != null
				}), this);
				return false;
			}
			this.IsWindowedActive = true;
			this.FreezePlayerMotion();
			this.SuspendHeavySceneRoots();
			this.customizationRig.SetActive(true);
			this.PrepareCustomizationRuntime();
			this._stageView = stageView;
			if (this._stageView != null)
			{
				this._stageViewTextureWas = this._stageView.texture;
				this._stageViewColorWas = this._stageView.color;
				this._stageViewRaycastWas = this._stageView.raycastTarget;
				this._stageViewStateCaptured = true;
				this._stageView.texture = null;
				this._stageView.color = Color.clear;
				this._stageView.raycastTarget = false;
			}
			this._uiHost = uiHost;
			this._uiOriginalParent = this.uiRoot.parent;
			this._uiOriginalSiblingIndex = this.uiRoot.GetSiblingIndex();
			this._savedRects.Clear();
			this._windowedCanvases.Clear();
			this.uiRoot.SetParent(uiHost, false);
			this.uiRoot.localPosition = Vector3.zero;
			this.uiRoot.localRotation = Quaternion.identity;
			this.uiRoot.localScale = Vector3.one;
			int num = 0;
			foreach (Canvas canvas in this.uiRoot.GetComponentsInChildren<Canvas>(true))
			{
				if (CielOSCustomizationTakeover.HasCanvasAncestorBelow(canvas.transform, this.uiRoot))
				{
					num++;
				}
				else
				{
					RectTransform rectTransform = canvas.transform as RectTransform;
					if (rectTransform != null && !this.IsAlreadySaved(rectTransform))
					{
						this.Save(rectTransform, canvas);
						CanvasScaler component = canvas.GetComponent<CanvasScaler>();
						if (component != null)
						{
							component.enabled = false;
						}
						Vector2 vector = this.windowedDesignSize;
						if (vector.x <= 0f || vector.y <= 0f)
						{
							vector = ((rectTransform.rect.size.x > 0f && rectTransform.rect.size.y > 0f) ? rectTransform.rect.size : new Vector2(1920f, 1080f));
						}
						rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
						rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
						rectTransform.pivot = new Vector2(0.5f, 0.5f);
						rectTransform.localScale = Vector3.one;
						this._windowedCanvases.Add(new CielOSCustomizationTakeover.WindowedCanvas
						{
							rect = rectTransform,
							designSize = vector
						});
					}
				}
			}
			if (num > 0)
			{
				Debug.Log(string.Format("[CielOSCustomizationTakeover] OpenWindowed: ignorati {0} canvas annidati ", num) + "(runtime, es. liste dropdown MUIP) — gestiti dai loro canvas di design.");
			}
			this.SyncWindowedLayout();
			return true;
		}

		// Token: 0x06009AC9 RID: 39625 RVA: 0x003F4DC0 File Offset: 0x003F2FC0
		private static bool HasCanvasAncestorBelow(Transform t, Transform stopAt)
		{
			Transform transform = t.parent;
			while (transform != null && transform != stopAt)
			{
				if (transform.GetComponent<Canvas>() != null)
				{
					return true;
				}
				transform = transform.parent;
			}
			return false;
		}

		// Token: 0x06009ACA RID: 39626 RVA: 0x003F4E00 File Offset: 0x003F3000
		public void SyncWindowedLayout()
		{
			if (!this.IsWindowedActive || this._uiHost == null)
			{
				return;
			}
			Rect rect = this._uiHost.rect;
			Vector3 vector = this._uiHost.TransformPoint(rect.center);
			Vector2 size = rect.size;
			for (int i = 0; i < this._windowedCanvases.Count; i++)
			{
				CielOSCustomizationTakeover.WindowedCanvas windowedCanvas = this._windowedCanvases[i];
				if (!(windowedCanvas.rect == null))
				{
					windowedCanvas.rect.sizeDelta = windowedCanvas.designSize;
					windowedCanvas.rect.localScale = new Vector3(size.x / windowedCanvas.designSize.x, size.y / windowedCanvas.designSize.y, 1f);
					windowedCanvas.rect.position = vector;
				}
			}
		}

		// Token: 0x06009ACB RID: 39627 RVA: 0x003F4EE4 File Offset: 0x003F30E4
		public void CloseWindowed()
		{
			if (!this.IsWindowedActive)
			{
				return;
			}
			this.IsWindowedActive = false;
			for (int i = this._savedRects.Count - 1; i >= 0; i--)
			{
				CielOSCustomizationTakeover.RectState rectState = this._savedRects[i];
				if (!(rectState.rect == null))
				{
					if (rectState.canvas != null)
					{
						rectState.canvas.enabled = rectState.canvasEnabled;
					}
					if (rectState.scaler != null)
					{
						rectState.scaler.enabled = rectState.scalerEnabled;
					}
					if (rectState.raycaster != null)
					{
						rectState.raycaster.enabled = rectState.raycasterEnabled;
					}
					rectState.rect.pivot = rectState.pivot;
					rectState.rect.anchorMin = rectState.anchorMin;
					rectState.rect.anchorMax = rectState.anchorMax;
					rectState.rect.offsetMin = rectState.offsetMin;
					rectState.rect.offsetMax = rectState.offsetMax;
					rectState.rect.anchoredPosition3D = rectState.anchoredPosition3D;
					rectState.rect.localScale = rectState.localScale;
				}
			}
			this._savedRects.Clear();
			this._windowedCanvases.Clear();
			this._uiHost = null;
			if (this.uiRoot != null && this._uiOriginalParent != null)
			{
				this.uiRoot.SetParent(this._uiOriginalParent, false);
				this.uiRoot.SetSiblingIndex(this._uiOriginalSiblingIndex);
			}
			if (this._stageView != null)
			{
				if (this._stageViewStateCaptured)
				{
					this._stageView.texture = this._stageViewTextureWas;
					this._stageView.color = this._stageViewColorWas;
					this._stageView.raycastTarget = this._stageViewRaycastWas;
				}
				this._stageView = null;
			}
			this._stageViewStateCaptured = false;
			if (this.hubController != null)
			{
				this.hubController.PrepareForTakeoverClose();
			}
			else if (this.coreOrchestrator != null)
			{
				this.coreOrchestrator.CloseSession();
			}
			if (this.customizationRig != null)
			{
				this.customizationRig.SetActive(false);
			}
			this.RestoreAuxiliaryCustomizationCameras();
			this.RestoreStageCamera();
			this.RestoreStageAudioListener();
			this.RestoreSuspendedSceneRoots();
			this.RestorePlayerMotion();
		}

		// Token: 0x06009ACC RID: 39628 RVA: 0x003F5130 File Offset: 0x003F3330
		private void Save(RectTransform rt, Canvas canvas)
		{
			CanvasScaler canvasScaler = ((canvas != null) ? canvas.GetComponent<CanvasScaler>() : null);
			GraphicRaycaster graphicRaycaster = ((canvas != null) ? canvas.GetComponent<GraphicRaycaster>() : null);
			this._savedRects.Add(new CielOSCustomizationTakeover.RectState
			{
				rect = rt,
				anchorMin = rt.anchorMin,
				anchorMax = rt.anchorMax,
				offsetMin = rt.offsetMin,
				offsetMax = rt.offsetMax,
				anchoredPosition3D = rt.anchoredPosition3D,
				localScale = rt.localScale,
				pivot = rt.pivot,
				canvas = canvas,
				canvasEnabled = (canvas != null && canvas.enabled),
				scaler = canvasScaler,
				scalerEnabled = (canvasScaler != null && canvasScaler.enabled),
				raycaster = graphicRaycaster,
				raycasterEnabled = (graphicRaycaster != null && graphicRaycaster.enabled)
			});
		}

		// Token: 0x06009ACD RID: 39629 RVA: 0x003F5240 File Offset: 0x003F3440
		private bool IsAlreadySaved(RectTransform rt)
		{
			for (int i = 0; i < this._savedRects.Count; i++)
			{
				if (this._savedRects[i].rect == rt)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009ACE RID: 39630 RVA: 0x003F5280 File Offset: 0x003F3480
		public void Open()
		{
			if (this.IsActive || this.IsWindowedActive)
			{
				return;
			}
			if (this.customizationRig == null)
			{
				Debug.LogWarning("[CielOSCustomizationTakeover] CustomizationRig non assegnato.", this);
				return;
			}
			this.IsActive = true;
			if (this.osController != null)
			{
				this.osController.enabled = false;
			}
			this.SetOsDesktopVisible(false);
			this.FreezePlayerMotion();
			this.SuspendHeavySceneRoots();
			this.customizationRig.SetActive(true);
			this.PrepareCustomizationRuntime();
		}

		// Token: 0x06009ACF RID: 39631 RVA: 0x003F52FE File Offset: 0x003F34FE
		private void HandleReturnRequested(string _)
		{
			if (!this.IsWindowedActive)
			{
				this.CloseFullscreenInternal();
				return;
			}
			Action windowCloseRequested = this.WindowCloseRequested;
			if (windowCloseRequested == null)
			{
				return;
			}
			windowCloseRequested();
		}

		// Token: 0x06009AD0 RID: 39632 RVA: 0x003F5320 File Offset: 0x003F3520
		private void CloseFullscreenInternal()
		{
			if (!this.IsActive)
			{
				return;
			}
			this.IsActive = false;
			if (this.hubController != null)
			{
				this.hubController.PrepareForTakeoverClose();
			}
			else if (this.coreOrchestrator != null)
			{
				this.coreOrchestrator.CloseSession();
			}
			if (this.customizationRig != null)
			{
				this.customizationRig.SetActive(false);
			}
			this.RestoreAuxiliaryCustomizationCameras();
			this.RestoreStageCamera();
			this.RestoreStageAudioListener();
			this.RestoreSuspendedSceneRoots();
			this.RestorePlayerMotion();
			this.SetOsDesktopVisible(true);
			if (this.osController != null)
			{
				this.osController.enabled = true;
			}
		}

		// Token: 0x06009AD1 RID: 39633 RVA: 0x003F53CC File Offset: 0x003F35CC
		private void SetOsDesktopVisible(bool visible)
		{
			if (this.osCanvasGroup == null)
			{
				return;
			}
			this.osCanvasGroup.alpha = (visible ? 1f : 0f);
			this.osCanvasGroup.interactable = visible;
			this.osCanvasGroup.blocksRaycasts = visible;
		}

		// Token: 0x06009AD2 RID: 39634 RVA: 0x003F541C File Offset: 0x003F361C
		private void PrepareCustomizationRuntime()
		{
			if (this.customizationRig == null)
			{
				return;
			}
			if (this.hubController == null)
			{
				this.hubController = this.customizationRig.GetComponentInChildren<CustomizationHubController>(true);
			}
			CustomizationPreviewPhysicsGuard[] componentsInChildren = this.customizationRig.GetComponentsInChildren<CustomizationPreviewPhysicsGuard>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Stabilize();
			}
			foreach (Transform transform in this.customizationRig.GetComponentsInChildren<Transform>(true))
			{
				if (!(transform == null) && !(transform.name != "CielPreview"))
				{
					CustomizationPreviewPhysicsGuard customizationPreviewPhysicsGuard = transform.GetComponent<CustomizationPreviewPhysicsGuard>();
					if (customizationPreviewPhysicsGuard == null)
					{
						customizationPreviewPhysicsGuard = transform.gameObject.AddComponent<CustomizationPreviewPhysicsGuard>();
					}
					customizationPreviewPhysicsGuard.Stabilize();
					break;
				}
			}
			CustomizationUiPolishController customizationUiPolishController = this.customizationRig.GetComponent<CustomizationUiPolishController>();
			if (customizationUiPolishController == null)
			{
				customizationUiPolishController = this.customizationRig.AddComponent<CustomizationUiPolishController>();
			}
			customizationUiPolishController.Configure((this.uiRoot != null) ? this.uiRoot : this.customizationRig.transform);
			Camera camera = this.FindPrimaryPreviewCamera();
			this.ConfigureCustomizationCameras(camera);
			this.EnsureStageCamera(camera);
			this.EnsureStageAudioListener();
		}

		// Token: 0x06009AD3 RID: 39635 RVA: 0x003F5550 File Offset: 0x003F3750
		private Camera FindPrimaryPreviewCamera()
		{
			if (this.customizationRig == null)
			{
				return null;
			}
			RawImage[] componentsInChildren = this.customizationRig.GetComponentsInChildren<RawImage>(true);
			Camera[] componentsInChildren2 = this.customizationRig.GetComponentsInChildren<Camera>(true);
			foreach (RawImage rawImage in componentsInChildren)
			{
				if (!(rawImage == null) && !(rawImage.name != "Character_Preview") && !(rawImage.texture == null))
				{
					foreach (Camera camera in componentsInChildren2)
					{
						if (camera != null && camera.targetTexture == rawImage.texture)
						{
							return camera;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06009AD4 RID: 39636 RVA: 0x003F55FC File Offset: 0x003F37FC
		private void EnsureStageCamera(Camera previewCamera)
		{
			if (this.stageCamera == null)
			{
				return;
			}
			if (!this._stageCameraStateCaptured)
			{
				this._stageCameraWasEnabled = this.stageCamera.enabled;
				this._stageCameraTargetWas = this.stageCamera.targetTexture;
				this._stageCameraStateCaptured = true;
			}
			this.stageCamera.targetTexture = null;
			this.stageCamera.enabled = true;
			CameraController[] componentsInChildren = this.customizationRig.GetComponentsInChildren<CameraController>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i] != null)
				{
					componentsInChildren[i].SetCamera((previewCamera != null) ? previewCamera : this.stageCamera);
				}
			}
		}

		// Token: 0x06009AD5 RID: 39637 RVA: 0x003F56A4 File Offset: 0x003F38A4
		private void RestoreStageCamera()
		{
			if (this._stageCameraStateCaptured && this.stageCamera != null)
			{
				this.stageCamera.enabled = this._stageCameraWasEnabled;
				this.stageCamera.targetTexture = this._stageCameraTargetWas;
			}
			this._stageCameraStateCaptured = false;
			this._stageCameraTargetWas = null;
		}

		// Token: 0x06009AD6 RID: 39638 RVA: 0x003F56F8 File Offset: 0x003F38F8
		private void EnsureStageAudioListener()
		{
			if (this.stageCamera == null)
			{
				return;
			}
			this._stageAudioListener = this.stageCamera.GetComponent<AudioListener>();
			AudioListener[] array = Object.FindObjectsOfType<AudioListener>();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != null && array[i] != this._stageAudioListener && array[i].isActiveAndEnabled)
				{
					this._stageAudioListener = null;
					return;
				}
			}
			if (this._stageAudioListener == null)
			{
				this._stageAudioListener = this.stageCamera.gameObject.AddComponent<AudioListener>();
			}
			if (!this._stageAudioListenerStateCaptured)
			{
				this._stageAudioListenerWasEnabled = this._stageAudioListener.enabled;
				this._stageAudioListenerStateCaptured = true;
			}
			this._stageAudioListener.enabled = true;
		}

		// Token: 0x06009AD7 RID: 39639 RVA: 0x003F57B6 File Offset: 0x003F39B6
		private void RestoreStageAudioListener()
		{
			if (this._stageAudioListenerStateCaptured && this._stageAudioListener != null)
			{
				this._stageAudioListener.enabled = this._stageAudioListenerWasEnabled;
			}
			this._stageAudioListener = null;
			this._stageAudioListenerStateCaptured = false;
		}

		// Token: 0x06009AD8 RID: 39640 RVA: 0x003F57F0 File Offset: 0x003F39F0
		private void ConfigureCustomizationCameras(Camera previewCamera)
		{
			this.RestoreAuxiliaryCustomizationCameras();
			if (this.customizationRig == null)
			{
				return;
			}
			foreach (Camera camera in this.customizationRig.GetComponentsInChildren<Camera>(true))
			{
				if (!(camera == null) && !(camera == this.stageCamera))
				{
					this._customizationCameraStates.Add(new CielOSCustomizationTakeover.CameraState
					{
						camera = camera,
						enabled = camera.enabled
					});
					camera.enabled = camera == previewCamera;
				}
			}
		}

		// Token: 0x06009AD9 RID: 39641 RVA: 0x003F5880 File Offset: 0x003F3A80
		private void RestoreAuxiliaryCustomizationCameras()
		{
			for (int i = this._customizationCameraStates.Count - 1; i >= 0; i--)
			{
				CielOSCustomizationTakeover.CameraState cameraState = this._customizationCameraStates[i];
				if (cameraState.camera != null)
				{
					cameraState.camera.enabled = cameraState.enabled;
				}
			}
			this._customizationCameraStates.Clear();
		}

		// Token: 0x06009ADA RID: 39642 RVA: 0x003F58DC File Offset: 0x003F3ADC
		private void SuspendHeavySceneRoots()
		{
			this.RestoreSuspendedSceneRoots();
			if (this.suspendedSceneRootNames == null || this.suspendedSceneRootNames.Length == 0)
			{
				return;
			}
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				Scene sceneAt = SceneManager.GetSceneAt(i);
				if (sceneAt.isLoaded)
				{
					foreach (GameObject gameObject in sceneAt.GetRootGameObjects())
					{
						if (!(gameObject == null) && !(gameObject == this.customizationRig) && this.ShouldSuspendRoot(gameObject.name) && !(gameObject.GetComponentInChildren<RankRuntimeController>(true) != null) && !(gameObject.GetComponentInChildren<AstarPath>(true) != null) && !(gameObject.GetComponentInChildren<ProceduralGraphMover>(true) != null) && !(gameObject.GetComponentInChildren<RVOSimulator>(true) != null))
						{
							this._suspendedRoots.Add(new CielOSCustomizationTakeover.GameObjectState
							{
								gameObject = gameObject,
								activeSelf = gameObject.activeSelf
							});
							if (gameObject.activeSelf)
							{
								gameObject.SetActive(false);
							}
						}
					}
				}
			}
		}

		// Token: 0x06009ADB RID: 39643 RVA: 0x003F59F8 File Offset: 0x003F3BF8
		private bool ShouldSuspendRoot(string rootName)
		{
			for (int i = 0; i < this.suspendedSceneRootNames.Length; i++)
			{
				if (string.Equals(rootName, this.suspendedSceneRootNames[i], StringComparison.Ordinal))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009ADC RID: 39644 RVA: 0x003F5A2C File Offset: 0x003F3C2C
		private void RestoreSuspendedSceneRoots()
		{
			for (int i = this._suspendedRoots.Count - 1; i >= 0; i--)
			{
				CielOSCustomizationTakeover.GameObjectState gameObjectState = this._suspendedRoots[i];
				if (gameObjectState.gameObject != null)
				{
					gameObjectState.gameObject.SetActive(gameObjectState.activeSelf);
				}
			}
			this._suspendedRoots.Clear();
		}

		// Token: 0x06009ADD RID: 39645 RVA: 0x003F5A88 File Offset: 0x003F3C88
		private void FreezePlayerMotion()
		{
			this.RestorePlayerMotion();
			this.ResolvePlayerApplier();
			if (this.playerApplier == null)
			{
				return;
			}
			Rigidbody componentInParent = this.playerApplier.GetComponentInParent<Rigidbody>();
			this._playerMotionRoot = ((componentInParent != null) ? componentInParent.transform : this.playerApplier.transform);
			this._playerPosition = this._playerMotionRoot.position;
			this._playerRotation = this._playerMotionRoot.rotation;
			foreach (Rigidbody rigidbody in this._playerMotionRoot.GetComponentsInChildren<Rigidbody>(true))
			{
				if (!(rigidbody == null))
				{
					this._playerRigidbodies.Add(new CielOSCustomizationTakeover.RigidbodyState
					{
						body = rigidbody,
						isKinematic = rigidbody.isKinematic,
						useGravity = rigidbody.useGravity,
						constraints = rigidbody.constraints,
						velocity = rigidbody.velocity,
						angularVelocity = rigidbody.angularVelocity
					});
					if (!rigidbody.isKinematic)
					{
						rigidbody.velocity = Vector3.zero;
						rigidbody.angularVelocity = Vector3.zero;
					}
					rigidbody.useGravity = false;
					rigidbody.isKinematic = true;
				}
			}
			foreach (global::CharacterController characterController in this._playerMotionRoot.GetComponentsInChildren<global::CharacterController>(true))
			{
				if (!(characterController == null))
				{
					this._playerCharacterControllers.Add(new CielOSCustomizationTakeover.CharacterControllerState
					{
						controller = characterController,
						enabled = characterController.enabled
					});
					characterController.enabled = false;
				}
			}
			this._playerMotionFrozen = true;
			this.MaintainFrozenPlayerPose();
		}

		// Token: 0x06009ADE RID: 39646 RVA: 0x003F5C38 File Offset: 0x003F3E38
		private void ResolvePlayerApplier()
		{
			if (this.playerApplier != null)
			{
				return;
			}
			GameObject playerObject = PlayerRegistry.PlayerObject;
			if (playerObject != null)
			{
				this.playerApplier = playerObject.GetComponentInChildren<MainCharacterAppearanceApplier>(true);
			}
		}

		// Token: 0x06009ADF RID: 39647 RVA: 0x003F5C70 File Offset: 0x003F3E70
		private void MaintainFrozenPlayerPose()
		{
			if (!this._playerMotionFrozen || this._playerMotionRoot == null)
			{
				return;
			}
			this._playerMotionRoot.SetPositionAndRotation(this._playerPosition, this._playerRotation);
		}

		// Token: 0x06009AE0 RID: 39648 RVA: 0x003F5CA0 File Offset: 0x003F3EA0
		private void RestorePlayerMotion()
		{
			if (!this._playerMotionFrozen)
			{
				this._playerRigidbodies.Clear();
				this._playerCharacterControllers.Clear();
				return;
			}
			if (this._playerMotionRoot != null)
			{
				this._playerMotionRoot.SetPositionAndRotation(this._playerPosition, this._playerRotation);
			}
			for (int i = this._playerCharacterControllers.Count - 1; i >= 0; i--)
			{
				CielOSCustomizationTakeover.CharacterControllerState characterControllerState = this._playerCharacterControllers[i];
				if (characterControllerState.controller != null)
				{
					characterControllerState.controller.enabled = characterControllerState.enabled;
				}
			}
			for (int j = this._playerRigidbodies.Count - 1; j >= 0; j--)
			{
				CielOSCustomizationTakeover.RigidbodyState rigidbodyState = this._playerRigidbodies[j];
				if (!(rigidbodyState.body == null))
				{
					rigidbodyState.body.isKinematic = rigidbodyState.isKinematic;
					rigidbodyState.body.useGravity = rigidbodyState.useGravity;
					rigidbodyState.body.constraints = rigidbodyState.constraints;
					if (!rigidbodyState.isKinematic)
					{
						rigidbodyState.body.velocity = rigidbodyState.velocity;
						rigidbodyState.body.angularVelocity = rigidbodyState.angularVelocity;
					}
				}
			}
			Physics.SyncTransforms();
			this._playerRigidbodies.Clear();
			this._playerCharacterControllers.Clear();
			this._playerMotionRoot = null;
			this._playerMotionFrozen = false;
		}

		// Token: 0x04009F94 RID: 40852
		[Header("Rig")]
		[Tooltip("Istanza (disattivata) del prefab CustomizationRig nella scena.")]
		[SerializeField]
		private GameObject customizationRig;

		// Token: 0x04009F95 RID: 40853
		[Tooltip("ReturnNavigator dentro il rig (CustomizationSystem/ReturnNavigator).")]
		[SerializeField]
		private CustomizationReturnNavigator returnNavigator;

		// Token: 0x04009F96 RID: 40854
		[Tooltip("Orchestrator del rig: CloseSession() chiude la sessione di preview.")]
		[SerializeField]
		private CustomizationCoreOrchestrator coreOrchestrator;

		// Token: 0x04009F97 RID: 40855
		[Tooltip("Hub controller used to reset editor/session state when the app window closes.")]
		[SerializeField]
		private CustomizationHubController hubController;

		// Token: 0x04009F98 RID: 40856
		[Header("Windowed mode")]
		[Tooltip("Root UI del rig ('---UI'): contiene hub, editor e shared UI. Viene ri-parentato nella finestra.")]
		[SerializeField]
		private Transform uiRoot;

		// Token: 0x04009F99 RID: 40857
		[Tooltip("Camera principale dello stage del rig: in windowed renderizza su RenderTexture.")]
		[SerializeField]
		private Camera stageCamera;

		// Token: 0x04009F9A RID: 40858
		[Tooltip("Risoluzione di design della UI del rig (frame Figma). In windowed le canvas restano a questa dimensione e vengono scalate sul content della finestra. NB: NON la referenceResolution del CanvasScaler (1980×1080, refuso non 16:9).")]
		[SerializeField]
		private Vector2 windowedDesignSize = new Vector2(1920f, 1080f);

		// Token: 0x04009F9B RID: 40859
		[Tooltip("Render resolution of the 3D preview inside the CielOS window.")]
		[SerializeField]
		private Vector2Int windowedRenderResolution = new Vector2Int(1280, 720);

		// Token: 0x04009F9C RID: 40860
		[Header("Performance")]
		[Tooltip("CielHome roots suspended while the customization app owns the screen.")]
		[SerializeField]
		private string[] suspendedSceneRootNames = new string[] { "---MAP", "---ENVIRONMENT", "ThirdPersonCamera" };

		// Token: 0x04009F9D RID: 40861
		[Header("CielOS")]
		[Tooltip("Controller del desktop: disabilitato durante il takeover FULLSCREEN (sopprime l'ESC del desktop).")]
		[SerializeField]
		private CielOSController osController;

		// Token: 0x04009F9E RID: 40862
		[Tooltip("CanvasGroup del desktop: nascosto durante il takeover FULLSCREEN.")]
		[SerializeField]
		private CanvasGroup osCanvasGroup;

		// Token: 0x04009F9F RID: 40863
		[Header("Player")]
		[Tooltip("Applier sul player di CielHome, usato per risolvere e sospendere il movimento del player.")]
		[SerializeField]
		private MainCharacterAppearanceApplier playerApplier;

		// Token: 0x04009FA0 RID: 40864
		[Header("Input (solo fullscreen)")]
		[Tooltip("Tasto che esce dal takeover fullscreen. In windowed si usa la X della finestra.")]
		[SerializeField]
		private KeyCode exitKey = KeyCode.Escape;

		// Token: 0x04009FA4 RID: 40868
		private readonly List<CielOSCustomizationTakeover.RectState> _savedRects = new List<CielOSCustomizationTakeover.RectState>();

		// Token: 0x04009FA5 RID: 40869
		private readonly List<CielOSCustomizationTakeover.WindowedCanvas> _windowedCanvases = new List<CielOSCustomizationTakeover.WindowedCanvas>();

		// Token: 0x04009FA6 RID: 40870
		private readonly List<CielOSCustomizationTakeover.GameObjectState> _suspendedRoots = new List<CielOSCustomizationTakeover.GameObjectState>();

		// Token: 0x04009FA7 RID: 40871
		private readonly List<CielOSCustomizationTakeover.CameraState> _customizationCameraStates = new List<CielOSCustomizationTakeover.CameraState>();

		// Token: 0x04009FA8 RID: 40872
		private readonly List<CielOSCustomizationTakeover.RigidbodyState> _playerRigidbodies = new List<CielOSCustomizationTakeover.RigidbodyState>();

		// Token: 0x04009FA9 RID: 40873
		private readonly List<CielOSCustomizationTakeover.CharacterControllerState> _playerCharacterControllers = new List<CielOSCustomizationTakeover.CharacterControllerState>();

		// Token: 0x04009FAA RID: 40874
		private Transform _uiOriginalParent;

		// Token: 0x04009FAB RID: 40875
		private int _uiOriginalSiblingIndex;

		// Token: 0x04009FAC RID: 40876
		private RectTransform _uiHost;

		// Token: 0x04009FAD RID: 40877
		private RawImage _stageView;

		// Token: 0x04009FAE RID: 40878
		private Texture _stageViewTextureWas;

		// Token: 0x04009FAF RID: 40879
		private Color _stageViewColorWas;

		// Token: 0x04009FB0 RID: 40880
		private bool _stageViewRaycastWas;

		// Token: 0x04009FB1 RID: 40881
		private bool _stageViewStateCaptured;

		// Token: 0x04009FB2 RID: 40882
		private AudioListener _stageAudioListener;

		// Token: 0x04009FB3 RID: 40883
		private bool _stageAudioListenerWasEnabled;

		// Token: 0x04009FB4 RID: 40884
		private bool _stageAudioListenerStateCaptured;

		// Token: 0x04009FB5 RID: 40885
		private bool _stageCameraWasEnabled;

		// Token: 0x04009FB6 RID: 40886
		private bool _stageCameraStateCaptured;

		// Token: 0x04009FB7 RID: 40887
		private RenderTexture _stageCameraTargetWas;

		// Token: 0x04009FB8 RID: 40888
		private Transform _playerMotionRoot;

		// Token: 0x04009FB9 RID: 40889
		private Vector3 _playerPosition;

		// Token: 0x04009FBA RID: 40890
		private Quaternion _playerRotation;

		// Token: 0x04009FBB RID: 40891
		private bool _playerMotionFrozen;

		// Token: 0x02001385 RID: 4997
		private struct RectState
		{
			// Token: 0x04009FBC RID: 40892
			public RectTransform rect;

			// Token: 0x04009FBD RID: 40893
			public Vector2 anchorMin;

			// Token: 0x04009FBE RID: 40894
			public Vector2 anchorMax;

			// Token: 0x04009FBF RID: 40895
			public Vector2 offsetMin;

			// Token: 0x04009FC0 RID: 40896
			public Vector2 offsetMax;

			// Token: 0x04009FC1 RID: 40897
			public Vector3 anchoredPosition3D;

			// Token: 0x04009FC2 RID: 40898
			public Vector3 localScale;

			// Token: 0x04009FC3 RID: 40899
			public Vector2 pivot;

			// Token: 0x04009FC4 RID: 40900
			public Canvas canvas;

			// Token: 0x04009FC5 RID: 40901
			public bool canvasEnabled;

			// Token: 0x04009FC6 RID: 40902
			public CanvasScaler scaler;

			// Token: 0x04009FC7 RID: 40903
			public bool scalerEnabled;

			// Token: 0x04009FC8 RID: 40904
			public GraphicRaycaster raycaster;

			// Token: 0x04009FC9 RID: 40905
			public bool raycasterEnabled;
		}

		// Token: 0x02001386 RID: 4998
		private struct WindowedCanvas
		{
			// Token: 0x04009FCA RID: 40906
			public RectTransform rect;

			// Token: 0x04009FCB RID: 40907
			public Vector2 designSize;
		}

		// Token: 0x02001387 RID: 4999
		private struct GameObjectState
		{
			// Token: 0x04009FCC RID: 40908
			public GameObject gameObject;

			// Token: 0x04009FCD RID: 40909
			public bool activeSelf;
		}

		// Token: 0x02001388 RID: 5000
		private struct CameraState
		{
			// Token: 0x04009FCE RID: 40910
			public Camera camera;

			// Token: 0x04009FCF RID: 40911
			public bool enabled;
		}

		// Token: 0x02001389 RID: 5001
		private struct RigidbodyState
		{
			// Token: 0x04009FD0 RID: 40912
			public Rigidbody body;

			// Token: 0x04009FD1 RID: 40913
			public bool isKinematic;

			// Token: 0x04009FD2 RID: 40914
			public bool useGravity;

			// Token: 0x04009FD3 RID: 40915
			public RigidbodyConstraints constraints;

			// Token: 0x04009FD4 RID: 40916
			public Vector3 velocity;

			// Token: 0x04009FD5 RID: 40917
			public Vector3 angularVelocity;
		}

		// Token: 0x0200138A RID: 5002
		private struct CharacterControllerState
		{
			// Token: 0x04009FD6 RID: 40918
			public global::CharacterController controller;

			// Token: 0x04009FD7 RID: 40919
			public bool enabled;
		}
	}
}
