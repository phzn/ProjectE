using System;
using ExhibitionistDemo.Movement;
using ProjectExhibitionist.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Vehicles.Spawning
{
	// Token: 0x02000B87 RID: 2951
	public sealed class VehicleLifetimeHUD : MonoBehaviour
	{
		// Token: 0x17000E09 RID: 3593
		// (get) Token: 0x06005426 RID: 21542 RVA: 0x0020A999 File Offset: 0x00208B99
		// (set) Token: 0x06005427 RID: 21543 RVA: 0x0020A9A0 File Offset: 0x00208BA0
		public static VehicleLifetimeHUD Instance { get; private set; }

		// Token: 0x17000E0A RID: 3594
		// (get) Token: 0x06005428 RID: 21544 RVA: 0x0020A9A8 File Offset: 0x00208BA8
		public int OwnerInstanceId
		{
			get
			{
				return this._ownerInstanceId;
			}
		}

		// Token: 0x17000E0B RID: 3595
		// (get) Token: 0x06005429 RID: 21545 RVA: 0x0020A9B0 File Offset: 0x00208BB0
		public bool IsVisible
		{
			get
			{
				return this._canvasGroup != null && this._canvasGroup.alpha > 0.001f;
			}
		}

		// Token: 0x17000E0C RID: 3596
		// (get) Token: 0x0600542A RID: 21546 RVA: 0x0020A9D4 File Offset: 0x00208BD4
		public bool IsPaused
		{
			get
			{
				return this._isPaused;
			}
		}

		// Token: 0x17000E0D RID: 3597
		// (get) Token: 0x0600542B RID: 21547 RVA: 0x0020A9DC File Offset: 0x00208BDC
		public float NormalizedTime
		{
			get
			{
				if (!(this._progressRing != null))
				{
					return 0f;
				}
				return this._progressRing.FillAmount;
			}
		}

		// Token: 0x17000E0E RID: 3598
		// (get) Token: 0x0600542C RID: 21548 RVA: 0x0020A9FD File Offset: 0x00208BFD
		public VehicleLifetimeGlyph CurrentGlyph
		{
			get
			{
				return this._currentGlyph;
			}
		}

		// Token: 0x17000E0F RID: 3599
		// (get) Token: 0x0600542D RID: 21549 RVA: 0x0020AA05 File Offset: 0x00208C05
		public bool UsesSourceIcon
		{
			get
			{
				return this._sourceIcon != null && this._sourceIcon.enabled && this._sourceIcon.sprite != null;
			}
		}

		// Token: 0x17000E10 RID: 3600
		// (get) Token: 0x0600542E RID: 21550 RVA: 0x0020AA35 File Offset: 0x00208C35
		public Canvas ScreenCanvas
		{
			get
			{
				return base.GetComponentInParent<Canvas>();
			}
		}

		// Token: 0x17000E11 RID: 3601
		// (get) Token: 0x0600542F RID: 21551 RVA: 0x0020AA3D File Offset: 0x00208C3D
		public static float RenderedDiameter
		{
			get
			{
				return 61.6f;
			}
		}

		// Token: 0x06005430 RID: 21552 RVA: 0x0020AA44 File Offset: 0x00208C44
		public static Vector2 GetPreferredTopLeftPosition()
		{
			float num;
			return VehicleLifetimeHUD.CalculateTopLeftPosition(AutoLocomotionHUD.TryGetVisibleIndicatorBottom(out num), num);
		}

		// Token: 0x06005431 RID: 21553 RVA: 0x0020AA5E File Offset: 0x00208C5E
		private void Awake()
		{
			if (VehicleLifetimeHUD.Instance != null && VehicleLifetimeHUD.Instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			VehicleLifetimeHUD.Instance = this;
			this.EnsureView();
		}

		// Token: 0x06005432 RID: 21554 RVA: 0x0020AA92 File Offset: 0x00208C92
		private void Update()
		{
			if (this._ownerInstanceId != 0 && this._owner == null)
			{
				this.HideAndClear();
			}
		}

		// Token: 0x06005433 RID: 21555 RVA: 0x0020AAB0 File Offset: 0x00208CB0
		private void LateUpdate()
		{
			this.UpdateScreenPosition();
		}

		// Token: 0x06005434 RID: 21556 RVA: 0x0020AAB8 File Offset: 0x00208CB8
		private void OnDestroy()
		{
			if (VehicleLifetimeHUD.Instance == this)
			{
				VehicleLifetimeHUD.Instance = null;
			}
		}

		// Token: 0x06005435 RID: 21557 RVA: 0x0020AAD0 File Offset: 0x00208CD0
		public static VehicleLifetimeHUD GetOrCreate()
		{
			if (VehicleLifetimeHUD.Instance != null)
			{
				VehicleLifetimeHUD.Instance.EnsureView();
				return VehicleLifetimeHUD.Instance;
			}
			VehicleLifetimeHUD vehicleLifetimeHUD = Object.FindObjectOfType<VehicleLifetimeHUD>(true);
			if (vehicleLifetimeHUD != null)
			{
				VehicleLifetimeHUD.Instance = vehicleLifetimeHUD;
				vehicleLifetimeHUD.EnsureView();
				return vehicleLifetimeHUD;
			}
			Canvas canvas = VehicleLifetimeHUD.FindBestScreenCanvas();
			if (canvas == null)
			{
				canvas = VehicleLifetimeHUD.CreateFallbackCanvas();
			}
			GameObject gameObject = new GameObject("Vehicle Lifetime HUD", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasGroup)
			});
			gameObject.layer = VehicleLifetimeHUD.ResolveUiLayer();
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			rectTransform.SetParent(canvas.transform, false);
			Vector2 vector = new Vector2(0f, 1f);
			rectTransform.anchorMax = vector;
			rectTransform.anchorMin = vector;
			rectTransform.pivot = new Vector2(0f, 1f);
			rectTransform.anchoredPosition = VehicleLifetimeHUD.TopLeftPreferredPosition;
			rectTransform.sizeDelta = Vector2.one * 88f;
			rectTransform.SetAsLastSibling();
			VehicleLifetimeHUD vehicleLifetimeHUD2 = (VehicleLifetimeHUD.Instance = gameObject.AddComponent<VehicleLifetimeHUD>());
			vehicleLifetimeHUD2.EnsureView();
			return vehicleLifetimeHUD2;
		}

		// Token: 0x06005436 RID: 21558 RVA: 0x0020ABE4 File Offset: 0x00208DE4
		public void Claim(SpawnedVehicleLifetime owner, Sprite sourceIcon, VehicleLifetimeGlyph fallbackGlyph)
		{
			if (owner == null)
			{
				return;
			}
			int instanceID = owner.GetInstanceID();
			if (this._ownerInstanceId != instanceID)
			{
				this._owner = owner;
				this._ownerInstanceId = instanceID;
				this.ConfigureIcon(sourceIcon, fallbackGlyph);
			}
			if (this._canvasGroup != null)
			{
				this._canvasGroup.alpha = 1f;
			}
			this.UpdateScreenPosition();
			base.transform.SetAsLastSibling();
		}

		// Token: 0x06005437 RID: 21559 RVA: 0x0020AC50 File Offset: 0x00208E50
		public bool RefreshOwner(SpawnedVehicleLifetime owner, float remainingSeconds, float totalSeconds, bool paused)
		{
			if (owner == null || owner.GetInstanceID() != this._ownerInstanceId)
			{
				return false;
			}
			float num = ((totalSeconds > 0f) ? Mathf.Clamp01(remainingSeconds / totalSeconds) : 0f);
			VehicleLifetimeRadialGraphic progressRing = this._progressRing;
			if (progressRing != null)
			{
				progressRing.SetFill(num);
			}
			this.SetPausedVisual(paused);
			if (num <= 0f)
			{
				this.HideAndClear();
			}
			return true;
		}

		// Token: 0x06005438 RID: 21560 RVA: 0x0020ACB7 File Offset: 0x00208EB7
		public bool Release(SpawnedVehicleLifetime owner)
		{
			if (owner == null || owner.GetInstanceID() != this._ownerInstanceId)
			{
				return false;
			}
			this.HideAndClear();
			return true;
		}

		// Token: 0x06005439 RID: 21561 RVA: 0x0020ACDC File Offset: 0x00208EDC
		private void BuildView()
		{
			this._canvasGroup = base.GetComponent<CanvasGroup>() ?? base.gameObject.AddComponent<CanvasGroup>();
			this._canvasGroup.interactable = false;
			this._canvasGroup.blocksRaycasts = false;
			VehicleLifetimeHUD.CreateRadial("Center", base.transform, new Vector2(59f, 59f), VehicleLifetimeHUD.CenterBackground, 0f).SetFill(1f);
			VehicleLifetimeHUD.CreateRadial("Clock Background", base.transform, Vector2.one * 88f, VehicleLifetimeHUD.RingBackground, 0.78f).SetFill(1f);
			this._progressRing = VehicleLifetimeHUD.CreateRadial("Clock", base.transform, Vector2.one * 88f, VehicleLifetimeHUD.ActiveAccent, 0.78f);
			this._progressRing.SetFill(1f);
			GameObject gameObject = VehicleLifetimeHUD.CreateUiObject("Vehicle Glyph", base.transform, new Vector2(49f, 49f));
			this._fallbackGlyph = gameObject.AddComponent<VehicleLifetimeGlyphGraphic>();
			this._fallbackGlyph.raycastTarget = false;
			this._fallbackGlyph.color = VehicleLifetimeHUD.ActiveIcon;
			GameObject gameObject2 = VehicleLifetimeHUD.CreateUiObject("Vehicle Icon", base.transform, new Vector2(45f, 45f));
			this._sourceIcon = gameObject2.AddComponent<Image>();
			this._sourceIcon.raycastTarget = false;
			this._sourceIcon.preserveAspect = true;
			this._sourceIcon.color = VehicleLifetimeHUD.ActiveIcon;
			this._sourceIcon.enabled = false;
		}

		// Token: 0x0600543A RID: 21562 RVA: 0x0020AE6C File Offset: 0x0020906C
		private void EnsureView()
		{
			base.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
			if (this._canvasGroup != null && this._progressRing != null && this._fallbackGlyph != null && this._sourceIcon != null)
			{
				return;
			}
			this.BuildView();
			this.HideAndClear();
		}

		// Token: 0x0600543B RID: 21563 RVA: 0x0020AEE0 File Offset: 0x002090E0
		private void ConfigureIcon(Sprite sourceIcon, VehicleLifetimeGlyph fallbackGlyph)
		{
			this._currentGlyph = fallbackGlyph;
			bool flag = sourceIcon != null;
			if (this._sourceIcon != null)
			{
				this._sourceIcon.sprite = sourceIcon;
				this._sourceIcon.enabled = flag;
			}
			if (this._fallbackGlyph != null)
			{
				this._fallbackGlyph.SetGlyph(fallbackGlyph);
				this._fallbackGlyph.enabled = !flag;
			}
		}

		// Token: 0x0600543C RID: 21564 RVA: 0x0020AF4C File Offset: 0x0020914C
		private void SetPausedVisual(bool paused)
		{
			this._isPaused = paused;
			if (this._progressRing != null)
			{
				this._progressRing.color = (paused ? VehicleLifetimeHUD.PausedAccent : VehicleLifetimeHUD.ActiveAccent);
			}
			if (this._fallbackGlyph != null)
			{
				this._fallbackGlyph.color = (paused ? VehicleLifetimeHUD.PausedIcon : VehicleLifetimeHUD.ActiveIcon);
			}
			if (this._sourceIcon != null)
			{
				this._sourceIcon.color = (paused ? VehicleLifetimeHUD.PausedIcon : VehicleLifetimeHUD.ActiveIcon);
			}
		}

		// Token: 0x0600543D RID: 21565 RVA: 0x0020AFD8 File Offset: 0x002091D8
		private void UpdateScreenPosition()
		{
			RectTransform rectTransform = base.transform as RectTransform;
			if (rectTransform == null)
			{
				return;
			}
			rectTransform.anchoredPosition = VehicleLifetimeHUD.GetPreferredTopLeftPosition();
		}

		// Token: 0x0600543E RID: 21566 RVA: 0x0020B006 File Offset: 0x00209206
		private static Vector2 CalculateTopLeftPosition(bool hasVisibleAutoIndicators, float occupiedBottom)
		{
			if (!hasVisibleAutoIndicators)
			{
				return VehicleLifetimeHUD.TopLeftPreferredPosition;
			}
			return new Vector2(VehicleLifetimeHUD.TopLeftPreferredPosition.x, Mathf.Min(VehicleLifetimeHUD.TopLeftPreferredPosition.y, occupiedBottom - 12f));
		}

		// Token: 0x0600543F RID: 21567 RVA: 0x0020B038 File Offset: 0x00209238
		private void HideAndClear()
		{
			this._owner = null;
			this._ownerInstanceId = 0;
			this._isPaused = false;
			if (this._canvasGroup != null)
			{
				this._canvasGroup.alpha = 0f;
			}
			VehicleLifetimeRadialGraphic progressRing = this._progressRing;
			if (progressRing == null)
			{
				return;
			}
			progressRing.SetFill(0f);
		}

		// Token: 0x06005440 RID: 21568 RVA: 0x0020B08D File Offset: 0x0020928D
		private static VehicleLifetimeRadialGraphic CreateRadial(string name, Transform parent, Vector2 size, Color color, float innerRadius)
		{
			VehicleLifetimeRadialGraphic vehicleLifetimeRadialGraphic = VehicleLifetimeHUD.CreateUiObject(name, parent, size).AddComponent<VehicleLifetimeRadialGraphic>();
			vehicleLifetimeRadialGraphic.raycastTarget = false;
			vehicleLifetimeRadialGraphic.color = color;
			vehicleLifetimeRadialGraphic.SetInnerRadius(innerRadius);
			return vehicleLifetimeRadialGraphic;
		}

		// Token: 0x06005441 RID: 21569 RVA: 0x0020B0B4 File Offset: 0x002092B4
		private static GameObject CreateUiObject(string name, Transform parent, Vector2 size)
		{
			GameObject gameObject = new GameObject(name, new Type[] { typeof(RectTransform) });
			gameObject.layer = VehicleLifetimeHUD.ResolveUiLayer();
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			rectTransform.SetParent(parent, false);
			Vector2 vector = new Vector2(0.5f, 0.5f);
			rectTransform.anchorMax = vector;
			rectTransform.anchorMin = vector;
			rectTransform.pivot = new Vector2(0.5f, 0.5f);
			rectTransform.anchoredPosition = Vector2.zero;
			rectTransform.sizeDelta = size;
			return gameObject;
		}

		// Token: 0x06005442 RID: 21570 RVA: 0x0020B140 File Offset: 0x00209340
		private static Canvas FindBestScreenCanvas()
		{
			Canvas[] array = Object.FindObjectsOfType<Canvas>(true);
			Canvas canvas = null;
			int num = int.MinValue;
			foreach (Canvas canvas2 in array)
			{
				if (!(canvas2 == null) && canvas2.renderMode != RenderMode.WorldSpace)
				{
					Canvas rootCanvas = canvas2.rootCanvas;
					if (!(rootCanvas == null) && rootCanvas.renderMode != RenderMode.WorldSpace)
					{
						int num2 = (rootCanvas.isActiveAndEnabled ? 1000 : 0);
						string name = rootCanvas.gameObject.name;
						if (name.IndexOf("HUD", StringComparison.OrdinalIgnoreCase) >= 0)
						{
							num2 += 100;
						}
						if (name.IndexOf("Auto Locomotion HUD", StringComparison.OrdinalIgnoreCase) >= 0)
						{
							num2 += 500;
						}
						if (rootCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
						{
							num2 += 5;
						}
						if (num2 > num)
						{
							canvas = rootCanvas;
							num = num2;
						}
					}
				}
			}
			return canvas;
		}

		// Token: 0x06005443 RID: 21571 RVA: 0x0020B214 File Offset: 0x00209414
		private static Canvas CreateFallbackCanvas()
		{
			GameObject gameObject = new GameObject("Vehicle Lifetime HUD Canvas", new Type[]
			{
				typeof(RectTransform),
				typeof(Canvas),
				typeof(CanvasScaler)
			});
			gameObject.layer = VehicleLifetimeHUD.ResolveUiLayer();
			Canvas component = gameObject.GetComponent<Canvas>();
			component.renderMode = RenderMode.ScreenSpaceOverlay;
			component.overrideSorting = true;
			component.sortingOrder = 145;
			GameplayHudVisibility.RegisterCanvas(component);
			CanvasScaler component2 = gameObject.GetComponent<CanvasScaler>();
			component2.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			component2.referenceResolution = new Vector2(1920f, 1080f);
			component2.matchWidthOrHeight = 0.5f;
			if (Application.isPlaying)
			{
				Object.DontDestroyOnLoad(gameObject);
			}
			return component;
		}

		// Token: 0x06005444 RID: 21572 RVA: 0x0020B2C4 File Offset: 0x002094C4
		private static int ResolveUiLayer()
		{
			int num = LayerMask.NameToLayer("UI");
			if (num < 0)
			{
				return 0;
			}
			return num;
		}

		// Token: 0x04005489 RID: 21641
		private static readonly Color RingBackground = new Color(1f, 1f, 1f, 0.16f);

		// Token: 0x0400548A RID: 21642
		private static readonly Color CenterBackground = new Color(0.1f, 0.105f, 0.14f, 0.82f);

		// Token: 0x0400548B RID: 21643
		private static readonly Color ActiveAccent = new Color32(237, 82, 125, byte.MaxValue);

		// Token: 0x0400548C RID: 21644
		private static readonly Color PausedAccent = new Color(0.93f, 0.6f, 0.69f, 0.72f);

		// Token: 0x0400548D RID: 21645
		private static readonly Color ActiveIcon = Color.white;

		// Token: 0x0400548E RID: 21646
		private static readonly Color PausedIcon = new Color(1f, 1f, 1f, 0.66f);

		// Token: 0x0400548F RID: 21647
		private const float HudDiameter = 88f;

		// Token: 0x04005490 RID: 21648
		private const float HudScale = 0.7f;

		// Token: 0x04005491 RID: 21649
		private const float NearbyIndicatorGap = 12f;

		// Token: 0x04005492 RID: 21650
		private static readonly Vector2 TopLeftPreferredPosition = new Vector2(210f, -22f);

		// Token: 0x04005494 RID: 21652
		private SpawnedVehicleLifetime _owner;

		// Token: 0x04005495 RID: 21653
		private int _ownerInstanceId;

		// Token: 0x04005496 RID: 21654
		private CanvasGroup _canvasGroup;

		// Token: 0x04005497 RID: 21655
		private VehicleLifetimeRadialGraphic _progressRing;

		// Token: 0x04005498 RID: 21656
		private VehicleLifetimeGlyphGraphic _fallbackGlyph;

		// Token: 0x04005499 RID: 21657
		private Image _sourceIcon;

		// Token: 0x0400549A RID: 21658
		private bool _isPaused;

		// Token: 0x0400549B RID: 21659
		private VehicleLifetimeGlyph _currentGlyph;
	}
}
