using System;
using ProjectExhibitionist.Core;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace Features.BoobTouch
{
	// Token: 0x020014E5 RID: 5349
	[DefaultExecutionOrder(120)]
	[DisallowMultipleComponent]
	public sealed class GropingFaceCameraOverlay : MonoBehaviour
	{
		// Token: 0x17001957 RID: 6487
		// (get) Token: 0x0600A4B7 RID: 42167 RVA: 0x0044AE0E File Offset: 0x0044900E
		public bool IsRequestedVisible
		{
			get
			{
				return this._requestedVisible;
			}
		}

		// Token: 0x17001958 RID: 6488
		// (get) Token: 0x0600A4B8 RID: 42168 RVA: 0x0044AE16 File Offset: 0x00449016
		public float CurrentAlpha
		{
			get
			{
				return this._currentAlpha;
			}
		}

		// Token: 0x17001959 RID: 6489
		// (get) Token: 0x0600A4B9 RID: 42169 RVA: 0x0044AE1E File Offset: 0x0044901E
		public bool IsRendering
		{
			get
			{
				return this._faceCamera != null && this._faceCamera.enabled;
			}
		}

		// Token: 0x1700195A RID: 6490
		// (get) Token: 0x0600A4BA RID: 42170 RVA: 0x0044AE3C File Offset: 0x0044903C
		public bool IsCanvasEnabled
		{
			get
			{
				if (this._canvasRoot == null || !this._canvasRoot.activeInHierarchy)
				{
					return false;
				}
				Canvas component = this._canvasRoot.GetComponent<Canvas>();
				return component != null && component.enabled;
			}
		}

		// Token: 0x1700195B RID: 6491
		// (get) Token: 0x0600A4BB RID: 42171 RVA: 0x0044AE83 File Offset: 0x00449083
		public bool IsActuallyVisible
		{
			get
			{
				return this.IsCanvasEnabled && this.IsRendering && this._currentAlpha > 0.001f;
			}
		}

		// Token: 0x0600A4BC RID: 42172 RVA: 0x0044AEA4 File Offset: 0x004490A4
		public bool OwnsCanvas(Canvas canvas)
		{
			return canvas != null && this._canvasRoot != null && canvas.gameObject == this._canvasRoot;
		}

		// Token: 0x0600A4BD RID: 42173 RVA: 0x0044AED0 File Offset: 0x004490D0
		public void BeginSession(Camera sourceCamera, Animator characterAnimator = null)
		{
			this._requestedVisible = false;
			this._currentAlpha = 0f;
			this._hasLockedCameraPose = false;
			this.Bind(sourceCamera, characterAnimator, true);
			if (this._canvasRoot != null)
			{
				this._canvasRoot.SetActive(true);
				Canvas component = this._canvasRoot.GetComponent<Canvas>();
				if (component != null)
				{
					component.enabled = true;
				}
			}
			if (this._canvasGroup != null)
			{
				this._canvasGroup.alpha = 0f;
			}
			if (this._faceCamera != null)
			{
				this._faceCamera.enabled = false;
			}
		}

		// Token: 0x0600A4BE RID: 42174 RVA: 0x0044AF6D File Offset: 0x0044916D
		public void Bind(Camera sourceCamera, Animator characterAnimator = null, bool forceRigRefresh = false)
		{
			this._sourceCamera = ((sourceCamera != null) ? sourceCamera : Camera.main);
			this.ResolveRig(characterAnimator, forceRigRefresh);
			this.EnsureVisuals();
			this.SyncSourceCameraSettings();
		}

		// Token: 0x0600A4BF RID: 42175 RVA: 0x0044AF9C File Offset: 0x0044919C
		public void SetVisible(bool visible, Camera sourceCamera = null)
		{
			if (sourceCamera != null && sourceCamera != this._sourceCamera)
			{
				this.Bind(sourceCamera, null, false);
			}
			else if (this._sourceCamera == null)
			{
				this.Bind(Camera.main, null, false);
			}
			if (visible && this._head == null)
			{
				this.ResolveRig(null, false);
				this.EnsureVisuals();
			}
			bool requestedVisible = this._requestedVisible;
			this._requestedVisible = visible && this._head != null;
			if (this._requestedVisible)
			{
				this.EnsureVisuals();
				if (!requestedVisible)
				{
					this._hasLockedCameraPose = false;
					this.LockFaceCameraPose();
				}
			}
		}

		// Token: 0x0600A4C0 RID: 42176 RVA: 0x0044B044 File Offset: 0x00449244
		private void LateUpdate()
		{
			float num = (this._requestedVisible ? 1f : 0f);
			this._currentAlpha = Mathf.MoveTowards(this._currentAlpha, num, Mathf.Max(0.1f, this.fadeSpeed) * Time.unscaledDeltaTime);
			if (this._canvasGroup != null)
			{
				this._canvasGroup.alpha = this._currentAlpha;
				this._canvasGroup.blocksRaycasts = false;
				this._canvasGroup.interactable = false;
			}
			if (this._panelRect != null)
			{
				float num2 = Mathf.Lerp(0.92f, 1f, Mathf.SmoothStep(0f, 1f, this._currentAlpha));
				this._panelRect.localScale = new Vector3(num2, num2, 1f);
			}
			bool flag = this._requestedVisible || this._currentAlpha > 0.001f;
			if (this._faceCamera != null)
			{
				this._faceCamera.enabled = flag && this._head != null;
				if (this._faceCamera.enabled && !this._hasLockedCameraPose)
				{
					this.SyncSourceCameraSettings();
					this.LockFaceCameraPose();
				}
			}
			if (!this._requestedVisible && this._currentAlpha <= 0.001f)
			{
				this._hasLockedCameraPose = false;
			}
		}

		// Token: 0x0600A4C1 RID: 42177 RVA: 0x0044B190 File Offset: 0x00449390
		private void ResolveRig(Animator preferredAnimator = null, bool forceRefresh = false)
		{
			if (forceRefresh)
			{
				this._characterAnimator = null;
				this._characterRoot = null;
				this._head = null;
				this._leftEye = null;
				this._rightEye = null;
			}
			if (preferredAnimator != null && preferredAnimator.isHuman)
			{
				this._characterAnimator = preferredAnimator;
			}
			if (this._characterAnimator == null || !this._characterAnimator.isHuman)
			{
				Animator componentInParent = base.GetComponentInParent<Animator>(true);
				if (componentInParent != null && componentInParent.isHuman)
				{
					this._characterAnimator = componentInParent;
				}
			}
			if (this._characterAnimator == null || !this._characterAnimator.isHuman)
			{
				foreach (Animator animator in base.GetComponentsInChildren<Animator>(true))
				{
					if (!(animator == null) && animator.isHuman)
					{
						this._characterAnimator = animator;
						break;
					}
				}
			}
			if (this._characterAnimator == null || !this._characterAnimator.isHuman)
			{
				this._characterRoot = null;
				this._head = null;
				this._leftEye = null;
				this._rightEye = null;
				return;
			}
			this._characterRoot = this._characterAnimator.transform;
			this._head = this._characterAnimator.GetBoneTransform(HumanBodyBones.Head);
			this._leftEye = this._characterAnimator.GetBoneTransform(HumanBodyBones.LeftEye);
			this._rightEye = this._characterAnimator.GetBoneTransform(HumanBodyBones.RightEye);
		}

		// Token: 0x0600A4C2 RID: 42178 RVA: 0x0044B2E8 File Offset: 0x004494E8
		private void EnsureVisuals()
		{
			if (this._head == null)
			{
				this.ResolveRig(null, false);
			}
			this.EnsureRenderTexture();
			if (this._canvasRoot != null && !this._canvasRoot.activeSelf)
			{
				this._canvasRoot.SetActive(true);
			}
			if (this._canvasRoot != null)
			{
				Canvas component = this._canvasRoot.GetComponent<Canvas>();
				if (component != null)
				{
					component.enabled = true;
				}
			}
			if (this._faceCamera != null)
			{
				this._faceCamera.targetTexture = this._renderTexture;
			}
			if (this._feedImage == null && this._canvasRoot != null)
			{
				this._feedImage = this._canvasRoot.GetComponentInChildren<RawImage>(true);
			}
			if (this._feedImage != null)
			{
				this._feedImage.texture = this._renderTexture;
			}
			if (this._canvasRoot != null && this._faceCamera != null)
			{
				return;
			}
			if (this._canvasRoot != null || this._faceCamera != null)
			{
				if (this._faceCamera != null)
				{
					Object.Destroy(this._faceCamera.gameObject);
				}
				if (this._canvasRoot != null)
				{
					Object.Destroy(this._canvasRoot);
				}
				this._faceCamera = null;
				this._canvasRoot = null;
				this._canvasGroup = null;
				this._panelRect = null;
				this._feedImage = null;
			}
			GameObject gameObject = new GameObject("Groping Face Camera");
			this._faceCamera = gameObject.AddComponent<Camera>();
			this._faceCamera.enabled = false;
			this._faceCamera.targetTexture = this._renderTexture;
			this._faceCamera.clearFlags = CameraClearFlags.Color;
			this._faceCamera.backgroundColor = new Color(0f, 0f, 0f, 0f);
			this._faceCamera.nearClipPlane = 0.025f;
			this._faceCamera.farClipPlane = 100f;
			this._faceCamera.fieldOfView = this.fieldOfView;
			this._faceCamera.allowHDR = false;
			this._faceCamera.allowMSAA = true;
			this._faceCamera.useOcclusionCulling = false;
			this._canvasRoot = new GameObject("Groping Face Reaction Overlay", new Type[]
			{
				typeof(RectTransform),
				typeof(Canvas),
				typeof(CanvasScaler),
				typeof(CanvasGroup)
			});
			this._canvasRoot.transform.SetParent(base.transform, false);
			Canvas component2 = this._canvasRoot.GetComponent<Canvas>();
			component2.renderMode = RenderMode.ScreenSpaceOverlay;
			component2.overrideSorting = true;
			component2.sortingOrder = 85;
			GameplayHudVisibility.RegisterCanvas(component2);
			CanvasScaler component3 = this._canvasRoot.GetComponent<CanvasScaler>();
			component3.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			component3.referenceResolution = new Vector2(1920f, 1080f);
			component3.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
			component3.matchWidthOrHeight = 0.5f;
			this._canvasGroup = this._canvasRoot.GetComponent<CanvasGroup>();
			this._canvasGroup.alpha = 0f;
			this._canvasGroup.blocksRaycasts = false;
			this._canvasGroup.interactable = false;
			GameObject gameObject2 = new GameObject("Cloud Face Feed", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(RawImage)
			});
			gameObject2.transform.SetParent(this._canvasRoot.transform, false);
			this._panelRect = gameObject2.GetComponent<RectTransform>();
			this._panelRect.anchorMin = new Vector2(0f, 0.5f);
			this._panelRect.anchorMax = new Vector2(0f, 0.5f);
			this._panelRect.pivot = new Vector2(0.5f, 0.5f);
			this._panelRect.sizeDelta = this.overlaySize;
			this._panelRect.anchoredPosition = this.overlayAnchoredPosition;
			this._panelRect.localRotation = Quaternion.Euler(0f, 0f, -1.5f);
			this._feedImage = gameObject2.GetComponent<RawImage>();
			this._feedImage.texture = this._renderTexture;
			this._feedImage.color = Color.white;
			this._feedImage.raycastTarget = false;
			Shader shader = ((this.faceCloudShader != null) ? this.faceCloudShader : Shader.Find("Hidden/Groping/FaceCloud"));
			if (shader != null)
			{
				this._cloudMaterial = new Material(shader)
				{
					name = "Groping Face Cloud (Runtime)"
				};
				this._cloudMaterial.SetFloat("_Opacity", Mathf.Clamp01(this.feedOpacity));
				this._cloudMaterial.SetColor("_HaloColor", this.haloColor);
				this._feedImage.material = this._cloudMaterial;
			}
			this.SyncSourceCameraSettings();
		}

		// Token: 0x0600A4C3 RID: 42179 RVA: 0x0044B7C0 File Offset: 0x004499C0
		private void EnsureRenderTexture()
		{
			int num = Mathf.Clamp(this.renderTextureSize, 128, 1024);
			if ((this._renderTexture == null || this._renderTexture.width != num || this._renderTexture.height != num) && this._renderTexture != null)
			{
				if (this._faceCamera != null)
				{
					this._faceCamera.targetTexture = null;
				}
				if (this._feedImage != null)
				{
					this._feedImage.texture = null;
				}
				this._renderTexture.Release();
				Object.Destroy(this._renderTexture);
				this._renderTexture = null;
			}
			if (this._renderTexture == null)
			{
				this._renderTexture = new RenderTexture(num, num, 24, RenderTextureFormat.ARGB32)
				{
					name = "Groping Face Camera RT",
					antiAliasing = 1,
					useMipMap = false,
					autoGenerateMips = false
				};
			}
			if (!this._renderTexture.IsCreated())
			{
				this._renderTexture.Create();
			}
			if (this._faceCamera != null)
			{
				this._faceCamera.targetTexture = this._renderTexture;
			}
			if (this._feedImage != null)
			{
				this._feedImage.texture = this._renderTexture;
			}
		}

		// Token: 0x0600A4C4 RID: 42180 RVA: 0x0044B90C File Offset: 0x00449B0C
		private void SyncSourceCameraSettings()
		{
			if (this._faceCamera == null)
			{
				return;
			}
			this._faceCamera.fieldOfView = this.fieldOfView;
			this._faceCamera.targetTexture = this._renderTexture;
			if (this._sourceCamera != null)
			{
				this._faceCamera.cullingMask = this._sourceCamera.cullingMask;
				this._faceCamera.depthTextureMode = DepthTextureMode.None;
			}
			UniversalAdditionalCameraData universalAdditionalCameraData = this._faceCamera.GetUniversalAdditionalCameraData();
			if (universalAdditionalCameraData != null)
			{
				universalAdditionalCameraData.renderType = CameraRenderType.Base;
				universalAdditionalCameraData.renderPostProcessing = false;
				universalAdditionalCameraData.renderShadows = true;
				universalAdditionalCameraData.requiresColorTexture = false;
				universalAdditionalCameraData.requiresDepthTexture = false;
				universalAdditionalCameraData.allowXRRendering = false;
			}
		}

		// Token: 0x0600A4C5 RID: 42181 RVA: 0x0044B9BC File Offset: 0x00449BBC
		private void LockFaceCameraPose()
		{
			if (this._faceCamera == null || this._head == null)
			{
				return;
			}
			Vector3 vector = ((this._characterRoot != null) ? this._characterRoot.up.normalized : Vector3.up);
			Vector3 vector2;
			if (this._leftEye != null && this._rightEye != null)
			{
				vector2 = (this._leftEye.position + this._rightEye.position) * 0.5f;
			}
			else
			{
				vector2 = this._head.position + vector * 0.075f;
			}
			vector2 += vector * this.faceVerticalOffset;
			Vector3 vector3 = ((this._characterRoot != null) ? Vector3.ProjectOnPlane(this._characterRoot.forward, vector) : Vector3.forward);
			if (vector3.sqrMagnitude < 0.0001f)
			{
				vector3 = Vector3.forward;
			}
			vector3.Normalize();
			Vector3 vector4 = vector2 + vector3 * Mathf.Max(0.25f, this.cameraDistance) + vector * this.cameraVerticalOffset;
			this._faceCamera.transform.SetPositionAndRotation(vector4, Quaternion.LookRotation(vector2 - vector4, vector));
			this._hasLockedCameraPose = true;
		}

		// Token: 0x0600A4C6 RID: 42182 RVA: 0x0044BB18 File Offset: 0x00449D18
		private void OnDisable()
		{
			this._requestedVisible = false;
			this._currentAlpha = 0f;
			this._hasLockedCameraPose = false;
			if (this._faceCamera != null)
			{
				this._faceCamera.enabled = false;
			}
			if (this._canvasGroup != null)
			{
				this._canvasGroup.alpha = 0f;
			}
		}

		// Token: 0x0600A4C7 RID: 42183 RVA: 0x0044BB78 File Offset: 0x00449D78
		private void OnDestroy()
		{
			if (this._faceCamera != null)
			{
				Object.Destroy(this._faceCamera.gameObject);
			}
			if (this._canvasRoot != null)
			{
				Object.Destroy(this._canvasRoot);
			}
			if (this._cloudMaterial != null)
			{
				Object.Destroy(this._cloudMaterial);
			}
			if (this._renderTexture != null)
			{
				this._renderTexture.Release();
				Object.Destroy(this._renderTexture);
			}
		}

		// Token: 0x0400A8F7 RID: 43255
		[Header("Look")]
		[SerializeField]
		private Shader faceCloudShader;

		// Token: 0x0400A8F8 RID: 43256
		[SerializeField]
		private Vector2 overlaySize = new Vector2(420f, 420f);

		// Token: 0x0400A8F9 RID: 43257
		[SerializeField]
		private Vector2 overlayAnchoredPosition = new Vector2(255f, 0f);

		// Token: 0x0400A8FA RID: 43258
		[SerializeField]
		[Range(0.1f, 1f)]
		private float feedOpacity = 0.82f;

		// Token: 0x0400A8FB RID: 43259
		[SerializeField]
		private Color haloColor = new Color(1f, 0.52f, 0.75f, 0.82f);

		// Token: 0x0400A8FC RID: 43260
		[SerializeField]
		[Min(0.1f)]
		private float fadeSpeed = 5.5f;

		// Token: 0x0400A8FD RID: 43261
		[Header("Face camera")]
		[SerializeField]
		[Range(128f, 1024f)]
		private int renderTextureSize = 512;

		// Token: 0x0400A8FE RID: 43262
		[SerializeField]
		[Range(10f, 50f)]
		private float fieldOfView = 26f;

		// Token: 0x0400A8FF RID: 43263
		[SerializeField]
		[Range(0.25f, 1.5f)]
		private float cameraDistance = 0.6f;

		// Token: 0x0400A900 RID: 43264
		[SerializeField]
		private float faceVerticalOffset = -0.018f;

		// Token: 0x0400A901 RID: 43265
		[SerializeField]
		private float cameraVerticalOffset = 0.015f;

		// Token: 0x0400A902 RID: 43266
		private Camera _sourceCamera;

		// Token: 0x0400A903 RID: 43267
		private Camera _faceCamera;

		// Token: 0x0400A904 RID: 43268
		private Animator _characterAnimator;

		// Token: 0x0400A905 RID: 43269
		private Transform _characterRoot;

		// Token: 0x0400A906 RID: 43270
		private Transform _head;

		// Token: 0x0400A907 RID: 43271
		private Transform _leftEye;

		// Token: 0x0400A908 RID: 43272
		private Transform _rightEye;

		// Token: 0x0400A909 RID: 43273
		private RenderTexture _renderTexture;

		// Token: 0x0400A90A RID: 43274
		private Material _cloudMaterial;

		// Token: 0x0400A90B RID: 43275
		private RectTransform _panelRect;

		// Token: 0x0400A90C RID: 43276
		private CanvasGroup _canvasGroup;

		// Token: 0x0400A90D RID: 43277
		private RawImage _feedImage;

		// Token: 0x0400A90E RID: 43278
		private GameObject _canvasRoot;

		// Token: 0x0400A90F RID: 43279
		private bool _requestedVisible;

		// Token: 0x0400A910 RID: 43280
		private float _currentAlpha;

		// Token: 0x0400A911 RID: 43281
		private bool _hasLockedCameraPose;
	}
}
