using System;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Bozo.AnimeCharacters
{
	// Token: 0x02000837 RID: 2103
	public class CameraController : MonoBehaviour
	{
		// Token: 0x060037B4 RID: 14260 RVA: 0x00145FB4 File Offset: 0x001441B4
		private void Awake()
		{
			this.cam = ((this.targetCamera != null) ? this.targetCamera : Camera.main);
			if (this.slider != null)
			{
				this.slider.onValueChanged.AddListener(new UnityAction<float>(this.SetPosition));
			}
			this.CacheScrollZoneRefs();
		}

		// Token: 0x060037B5 RID: 14261 RVA: 0x00146012 File Offset: 0x00144212
		private void OnEnable()
		{
			CameraController._activeDriver = this;
		}

		// Token: 0x060037B6 RID: 14262 RVA: 0x0014601A File Offset: 0x0014421A
		private void OnDisable()
		{
			if (CameraController._activeDriver == this)
			{
				CameraController._activeDriver = null;
			}
		}

		// Token: 0x060037B7 RID: 14263 RVA: 0x0014602F File Offset: 0x0014422F
		private void OnDestroy()
		{
			if (this.slider != null)
			{
				this.slider.onValueChanged.RemoveListener(new UnityAction<float>(this.SetPosition));
			}
		}

		// Token: 0x060037B8 RID: 14264 RVA: 0x0014605C File Offset: 0x0014425C
		private void CacheScrollZoneRefs()
		{
			if (this.scrollZoneRect == null)
			{
				return;
			}
			this._scrollZoneCanvas = this.scrollZoneRect.GetComponentInParent<Canvas>();
			if (this._scrollZoneCanvas != null && this._scrollZoneCanvas.renderMode != RenderMode.ScreenSpaceOverlay)
			{
				this._scrollZoneCamera = this._scrollZoneCanvas.worldCamera;
			}
		}

		// Token: 0x060037B9 RID: 14265 RVA: 0x001460B5 File Offset: 0x001442B5
		public void SetCamera(Camera camera)
		{
			this.targetCamera = camera;
			this.cam = camera;
		}

		// Token: 0x060037BA RID: 14266 RVA: 0x001460C8 File Offset: 0x001442C8
		private void Update()
		{
			if (CameraController._activeDriver == null)
			{
				CameraController._activeDriver = this;
			}
			if (CameraController._activeDriver != this)
			{
				return;
			}
			if (this.cam == null)
			{
				this.cam = ((this.targetCamera != null) ? this.targetCamera : Camera.main);
				if (this.cam == null)
				{
					return;
				}
			}
			bool flag = this.IsPointerInScrollZone();
			bool flag2 = flag || CameraController.IsControllerInputDeviceActive();
			this.ApplyControllerPreviewInput(Time.unscaledDeltaTime);
			if (flag)
			{
				float axis = Input.GetAxis("Mouse ScrollWheel");
				if (axis > 0f)
				{
					this.position += this.scrollSpeed;
					this.tweenTimer = -1f;
				}
				else if (axis < 0f)
				{
					this.position -= this.scrollSpeed;
					this.tweenTimer = -1f;
				}
			}
			this.position = Mathf.Clamp(this.position, 0f, 1f);
			if (this.tweenTimer >= 0f)
			{
				this.position = Mathf.Lerp(this.targetPosition, this.currentPosition, this.tweenTimer);
				if (this.tweenVerticalOffset)
				{
					this._verticalOffset = Mathf.Lerp(this.targetVerticalOffset, this.currentVerticalOffset, this.tweenTimer);
				}
				this.tweenTimer -= Time.deltaTime * this.tweenSpeed;
			}
			this.UpdateVerticalPan(flag2);
			if (this.slider != null)
			{
				this._suppressSliderCallback = true;
				this.slider.value = this.position;
				this._suppressSliderCallback = false;
			}
			Vector3 vector = Vector3.Lerp(this.startPosition.position, this.endPosition.position, this.position);
			this.cam.transform.position = vector + Vector3.up * this._verticalOffset;
			this.cam.fieldOfView = Mathf.Lerp(this.fov.x, this.fov.y, this.position);
		}

		// Token: 0x060037BB RID: 14267 RVA: 0x001462D8 File Offset: 0x001444D8
		private void ApplyControllerPreviewInput(float deltaTime)
		{
			if (!ControllerInput.IsControllerActive || ControllerInput.UiInputConsumedThisFrame || !ControllerUINavigation.CanControlPreview(this) || !ControllerInput.Modifier)
			{
				return;
			}
			float y = ControllerInput.Look.y;
			if (ControllerInput.GetButton(ControllerButton.RightTrigger))
			{
				if (!this.enableVerticalPan)
				{
					return;
				}
				this._verticalOffset = Mathf.Clamp(this._verticalOffset + y * deltaTime, -this.maxVerticalOffset, this.maxVerticalOffset);
			}
			else
			{
				this.position = Mathf.Clamp01(this.position + y * deltaTime * 0.5f);
			}
			if (!Mathf.Approximately(y, 0f))
			{
				this.tweenTimer = -1f;
			}
		}

		// Token: 0x060037BC RID: 14268 RVA: 0x00146378 File Offset: 0x00144578
		private void UpdateVerticalPan(bool allowPan)
		{
			if (!this.enableVerticalPan)
			{
				this._verticalOffset = 0f;
				this._isPanning = false;
				return;
			}
			if (this.position <= this.panResetZoomThreshold)
			{
				this._verticalOffset = Mathf.MoveTowards(this._verticalOffset, 0f, this.panResetSpeed * Time.deltaTime);
				this._isPanning = false;
				return;
			}
			if (ControllerInput.IsControllerActive)
			{
				this._isPanning = false;
				return;
			}
			if (!allowPan)
			{
				this._isPanning = false;
				return;
			}
			this._isPanning = this.IsPanHoldPressed();
			if (this._isPanning)
			{
				float panVerticalAxis = this.GetPanVerticalAxis();
				this._verticalOffset += panVerticalAxis * this.panSensitivity * ((float)Screen.height * 0.1f);
				this._verticalOffset = Mathf.Clamp(this._verticalOffset, -this.maxVerticalOffset, this.maxVerticalOffset);
			}
		}

		// Token: 0x060037BD RID: 14269 RVA: 0x0014644D File Offset: 0x0014464D
		private bool IsPanHoldPressed()
		{
			if (this.panHoldInput != null)
			{
				return this.panHoldInput.GetButton();
			}
			return Input.GetMouseButton(2);
		}

		// Token: 0x060037BE RID: 14270 RVA: 0x00146469 File Offset: 0x00144669
		private float GetPanVerticalAxis()
		{
			if (this.panVerticalAxisInput != null)
			{
				return this.panVerticalAxisInput.GetAxisRaw();
			}
			return Input.GetAxisRaw("Mouse Y");
		}

		// Token: 0x060037BF RID: 14271 RVA: 0x0014648C File Offset: 0x0014468C
		private bool IsPointerInScrollZone()
		{
			if (this.scrollZoneRect != null)
			{
				return RectTransformUtility.RectangleContainsScreenPoint(this.scrollZoneRect, Input.mousePosition, this._scrollZoneCamera);
			}
			return EventSystem.current == null || !EventSystem.current.IsPointerOverGameObject();
		}

		// Token: 0x060037C0 RID: 14272 RVA: 0x001464DF File Offset: 0x001446DF
		private static bool IsControllerInputDeviceActive()
		{
			return vInput.instance != null && vInput.instance.inputDevice == InputDevice.Joystick;
		}

		// Token: 0x060037C1 RID: 14273 RVA: 0x001464FD File Offset: 0x001446FD
		public void SetPosition(float value)
		{
			if (this._suppressSliderCallback)
			{
				return;
			}
			this.position = value;
			this.tweenTimer = -1f;
		}

		// Token: 0x060037C2 RID: 14274 RVA: 0x0014651A File Offset: 0x0014471A
		public void tweenPosition(float value)
		{
			this.tweenTimer = 1f;
			this.targetPosition = Mathf.Clamp01(value);
			this.currentPosition = this.position;
			this.tweenVerticalOffset = false;
		}

		// Token: 0x060037C3 RID: 14275 RVA: 0x00146548 File Offset: 0x00144748
		public void TweenToContext(float zoom, float verticalOffset)
		{
			this.tweenTimer = 1f;
			this.targetPosition = Mathf.Clamp01(zoom);
			this.currentPosition = this.position;
			this.currentVerticalOffset = this._verticalOffset;
			this.targetVerticalOffset = Mathf.Clamp(verticalOffset, -this.maxVerticalOffset, this.maxVerticalOffset);
			this.tweenVerticalOffset = true;
		}

		// Token: 0x060037C4 RID: 14276 RVA: 0x001465A4 File Offset: 0x001447A4
		public void ResetVerticalPan()
		{
			this._verticalOffset = 0f;
			this._isPanning = false;
		}

		// Token: 0x170009EA RID: 2538
		// (get) Token: 0x060037C5 RID: 14277 RVA: 0x001465B8 File Offset: 0x001447B8
		public float VerticalOffset
		{
			get
			{
				return this._verticalOffset;
			}
		}

		// Token: 0x170009EB RID: 2539
		// (get) Token: 0x060037C6 RID: 14278 RVA: 0x001465C0 File Offset: 0x001447C0
		public float ZoomPosition
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x0400386F RID: 14447
		[Tooltip("Camera pilotata dal controller. Se nulla, fallback a Camera.main (comportamento storico della scena Customization). In app-mode (CielOS) va assegnata la camera dello stage che renderizza su RenderTexture — Camera.main lì è quella di gioco.")]
		[SerializeField]
		private Camera targetCamera;

		// Token: 0x04003870 RID: 14448
		private Camera cam;

		// Token: 0x04003871 RID: 14449
		private float position;

		// Token: 0x04003872 RID: 14450
		public float scrollSpeed;

		// Token: 0x04003873 RID: 14451
		public Transform startPosition;

		// Token: 0x04003874 RID: 14452
		public Transform endPosition;

		// Token: 0x04003875 RID: 14453
		public Vector2 fov;

		// Token: 0x04003876 RID: 14454
		public Slider slider;

		// Token: 0x04003877 RID: 14455
		[Tooltip("RectTransform that defines the scroll-wheel zoom area. When assigned, zoom only works when the mouse is inside this rect. When null, zoom works whenever the pointer is NOT over a UI element (legacy behavior).")]
		[SerializeField]
		private RectTransform scrollZoneRect;

		// Token: 0x04003878 RID: 14456
		public float tweenSpeed;

		// Token: 0x04003879 RID: 14457
		[Header("Vertical Pan (Middle Mouse)")]
		[Tooltip("Enable vertical panning while holding the middle mouse button.")]
		[SerializeField]
		private bool enableVerticalPan = true;

		// Token: 0x0400387A RID: 14458
		[Tooltip("Input hold used to enable camera pan (mouse/controller), configurable via GenericInput.")]
		[SerializeField]
		private GenericInput panHoldInput = new GenericInput("Mouse2", "RightStickClick", "RightStickClick");

		// Token: 0x0400387B RID: 14459
		[Tooltip("Input axis used for vertical pan movement.")]
		[SerializeField]
		private GenericInput panVerticalAxisInput = new GenericInput("Mouse Y", true, false, "RightAnalogVertical", true, false, "Mouse Y", true, false);

		// Token: 0x0400387C RID: 14460
		[Tooltip("Vertical pan sensitivity (world units per pixel of mouse movement).")]
		[SerializeField]
		private float panSensitivity = 0.005f;

		// Token: 0x0400387D RID: 14461
		[Tooltip("Maximum vertical offset from the zoom axis (world units).")]
		[SerializeField]
		private float maxVerticalOffset = 1.5f;

		// Token: 0x0400387E RID: 14462
		[Tooltip("Speed at which vertical offset resets to zero when zooming out fully.")]
		[SerializeField]
		private float panResetSpeed = 5f;

		// Token: 0x0400387F RID: 14463
		[Tooltip("Zoom threshold below which vertical pan resets to zero (0 = start position).")]
		[SerializeField]
		private float panResetZoomThreshold = 0.02f;

		// Token: 0x04003880 RID: 14464
		private float tweenTimer;

		// Token: 0x04003881 RID: 14465
		private float currentPosition;

		// Token: 0x04003882 RID: 14466
		private float targetPosition;

		// Token: 0x04003883 RID: 14467
		private float currentVerticalOffset;

		// Token: 0x04003884 RID: 14468
		private float targetVerticalOffset;

		// Token: 0x04003885 RID: 14469
		private bool tweenVerticalOffset;

		// Token: 0x04003886 RID: 14470
		private bool _suppressSliderCallback;

		// Token: 0x04003887 RID: 14471
		private Canvas _scrollZoneCanvas;

		// Token: 0x04003888 RID: 14472
		private Camera _scrollZoneCamera;

		// Token: 0x04003889 RID: 14473
		private float _verticalOffset;

		// Token: 0x0400388A RID: 14474
		private bool _isPanning;

		// Token: 0x0400388B RID: 14475
		private static CameraController _activeDriver;
	}
}
