using System;
using Features.Localization;
using ProjectExhibitionist.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Features.DebugTools
{
	// Token: 0x02001345 RID: 4933
	[DisallowMultipleComponent]
	public sealed class FlyCameraPhotoViewfinder : MonoBehaviour
	{
		// Token: 0x0600986A RID: 39018 RVA: 0x003E7104 File Offset: 0x003E5304
		public void Initialize(Camera camera, string exitKey = "K", bool showSpeedControl = true)
		{
			this._camera = camera;
			if (this._canvas == null)
			{
				this.Build();
			}
			this._exitKey = exitKey;
			this._showSpeedControl = showSpeedControl;
			this.RefreshInputHints();
			this.RefreshReadout();
		}

		// Token: 0x0600986B RID: 39019 RVA: 0x003E713C File Offset: 0x003E533C
		public void SetVisible(bool visible)
		{
			if (this._canvas == null && visible)
			{
				this.Build();
			}
			if (this._canvas == null)
			{
				return;
			}
			this._canvas.enabled = visible;
			if (visible)
			{
				this.RefreshSafeArea();
				this.RefreshReadout();
			}
		}

		// Token: 0x0600986C RID: 39020 RVA: 0x003E7189 File Offset: 0x003E5389
		public void SetCaptureInProgress(bool inProgress)
		{
			this._captureInProgress = inProgress;
			if (this._captureHint == null)
			{
				return;
			}
			this.RefreshInputHints();
			this._captureHint.color = (inProgress ? FlyCameraPhotoViewfinder.Muted : FlyCameraPhotoViewfinder.White);
		}

		// Token: 0x0600986D RID: 39021 RVA: 0x003E71C4 File Offset: 0x003E53C4
		public void ShowShutter()
		{
			if (this._canvas == null)
			{
				return;
			}
			this._shutterStart = Time.unscaledTime;
			this._shutter.color = new Color(0f, 0f, 0f, 1f);
			this._shutter.enabled = true;
		}

		// Token: 0x0600986E RID: 39022 RVA: 0x003E721C File Offset: 0x003E541C
		public void ShowSaved(string fileName)
		{
			if (this._canvas == null)
			{
				return;
			}
			this._photosTaken++;
			LocalizedLegacyGameText.BindComputed(this._counter, () => GameLocalization.FormatSource("camera_ui", "PHOTO  {0}", new object[] { this._photosTaken.ToString("0000") }));
			this.SetCaptureInProgress(false);
			this.ShowToast("PHOTO SAVED", fileName, FlyCameraPhotoViewfinder.Focused, false);
		}

		// Token: 0x0600986F RID: 39023 RVA: 0x003E7278 File Offset: 0x003E5478
		public void ShowError()
		{
			if (this._canvas == null)
			{
				return;
			}
			this.SetCaptureInProgress(false);
			this.ShowToast("COULD NOT SAVE PHOTO", "Check the game log for details, then try again.", new Color(1f, 0.71f, 0.62f, 1f), true);
		}

		// Token: 0x06009870 RID: 39024 RVA: 0x003E72C8 File Offset: 0x003E54C8
		private void Update()
		{
			if (this._canvas == null || !this._canvas.enabled)
			{
				return;
			}
			if (this._hintController != ControllerInput.IsActive || this._hintVersion != ControllerGlyphs.Version)
			{
				this.RefreshInputHints();
			}
			this.RefreshSafeArea();
			if (Time.unscaledTime >= this._nextReadout)
			{
				this._nextReadout = Time.unscaledTime + 0.25f;
				this.RefreshReadout();
			}
			float num = Time.unscaledTime - this._shutterStart;
			if (num < 0.055f)
			{
				this._shutter.color = new Color(0f, 0f, 0f, 1f);
			}
			else if (num < 0.29f)
			{
				this._shutter.color = new Color(1f, 1f, 1f, 0.28f * (1f - Mathf.InverseLerp(0.055f, 0.29f, num)));
			}
			else
			{
				this._shutter.enabled = false;
			}
			Color color = ((num < 0.65f) ? FlyCameraPhotoViewfinder.Focused : FlyCameraPhotoViewfinder.White);
			for (int i = 0; i < this._focusCorners.Length; i++)
			{
				this._focusCorners[i].color = color;
			}
			float num2 = Time.unscaledTime - this._toastStart;
			this._toast.alpha = Mathf.Clamp01(num2 / 0.16f) * (1f - Mathf.InverseLerp(3.6f, 4.1f, num2));
		}

		// Token: 0x06009871 RID: 39025 RVA: 0x003E7438 File Offset: 0x003E5638
		private void RefreshInputHints()
		{
			this._hintController = ControllerInput.IsActive;
			this._hintVersion = ControllerGlyphs.Version;
			if (this._exitHint == null || this._movementHint == null || this._captureHint == null)
			{
				return;
			}
			string text = (this._hintController ? ControllerGlyphs.Plain("B", ControllerGlyphs.Family) : this._exitKey);
			LocalizedLegacyGameText.BindFormat(this._exitHint, "camera_ui", "[ {0} ]  EXIT", new object[] { text }).FitTranslationToRectangle();
			string text2 = (this._hintController ? ControllerGlyphs.Plain("LS  MOVE     RS  LOOK     LB / RB  HEIGHT     L3  BOOST" + (this._showSpeedControl ? "     D-Pad Up / Down  SPEED" : string.Empty), ControllerGlyphs.Family) : ("WASD  MOVE     Q / E  HEIGHT     SHIFT  BOOST" + (this._showSpeedControl ? "     SCROLL  SPEED" : string.Empty)));
			LocalizedLegacyGameText.BindSource(this._movementHint, "camera_ui", text2).FitTranslationToRectangle();
			if (this._captureInProgress)
			{
				LocalizedLegacyGameText.BindSource(this._captureHint, "camera_ui", "SAVING PHOTO...");
				return;
			}
			LocalizedLegacyGameText.BindFormat(this._captureHint, "camera_ui", "[ {0} ]  TAKE PHOTO", new object[] { this._hintController ? ControllerGlyphs.Plain("A", ControllerGlyphs.Family) : "SPACE" });
		}

		// Token: 0x06009872 RID: 39026 RVA: 0x003E7590 File Offset: 0x003E5790
		private void Build()
		{
			this._font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
			GameObject gameObject = new GameObject("Photo Viewfinder", new Type[]
			{
				typeof(RectTransform),
				typeof(Canvas),
				typeof(CanvasScaler),
				typeof(CanvasGroup)
			});
			gameObject.transform.SetParent(base.transform, false);
			this._canvas = gameObject.GetComponent<Canvas>();
			this._canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			this._canvas.sortingOrder = 32000;
			GameplayHudVisibility.RegisterCanvas(this._canvas);
			CanvasScaler component = gameObject.GetComponent<CanvasScaler>();
			component.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			component.referenceResolution = new Vector2(1920f, 1080f);
			component.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
			component.matchWidthOrHeight = 0.5f;
			CanvasGroup component2 = gameObject.GetComponent<CanvasGroup>();
			component2.interactable = false;
			component2.blocksRaycasts = false;
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			FlyCameraPhotoViewfinder.Place(FlyCameraPhotoViewfinder.AddImage("Top rail", rectTransform, new Color(0.02f, 0.025f, 0.03f, 0.34f)).rectTransform, new Vector2(0f, 1f), Vector2.one, new Vector2(0f, 1f), Vector2.zero, new Vector2(0f, 86f));
			FlyCameraPhotoViewfinder.Place(FlyCameraPhotoViewfinder.AddImage("Bottom rail", rectTransform, new Color(0.02f, 0.025f, 0.03f, 0.38f)).rectTransform, Vector2.zero, Vector2.right, Vector2.zero, Vector2.zero, new Vector2(0f, 106f));
			this._safeArea = FlyCameraPhotoViewfinder.AddRect("Safe area", rectTransform);
			RectTransform rectTransform2 = FlyCameraPhotoViewfinder.AddRect("Composition frame", this._safeArea);
			FlyCameraPhotoViewfinder.Stretch(rectTransform2, new Vector2(0.055f, 0.145f), new Vector2(0.945f, 0.865f));
			this.AddGrid(rectTransform2);
			FlyCameraPhotoViewfinder.AddCorners(rectTransform2, 42f, 2f, FlyCameraPhotoViewfinder.White, null);
			RectTransform rectTransform3 = FlyCameraPhotoViewfinder.AddRect("Focus brackets", rectTransform2);
			FlyCameraPhotoViewfinder.Place(rectTransform3, Vector2.one * 0.5f, Vector2.one * 0.5f, Vector2.one * 0.5f, Vector2.zero, new Vector2(118f, 82f));
			FlyCameraPhotoViewfinder.AddCorners(rectTransform3, 19f, 2f, FlyCameraPhotoViewfinder.White, this._focusCorners);
			FlyCameraPhotoViewfinder.Place(FlyCameraPhotoViewfinder.AddImage("Focus point", rectTransform3, FlyCameraPhotoViewfinder.Muted).rectTransform, Vector2.one * 0.5f, Vector2.one * 0.5f, Vector2.one * 0.5f, Vector2.zero, new Vector2(3f, 3f));
			FlyCameraPhotoViewfinder.Place(this.AddText("Mode", this._safeArea, "PHOTO MODE", 23, TextAnchor.MiddleLeft, FlyCameraPhotoViewfinder.White).rectTransform, Vector2.up, Vector2.up, Vector2.up, new Vector2(64f, -23f), new Vector2(250f, 38f));
			this._counter = this.AddText("Counter", this._safeArea, "PHOTO  0000", 19, TextAnchor.MiddleRight, FlyCameraPhotoViewfinder.White);
			LocalizedLegacyGameText.BindComputed(this._counter, () => GameLocalization.FormatSource("camera_ui", "PHOTO  {0}", new object[] { this._photosTaken.ToString("0000") }));
			FlyCameraPhotoViewfinder.Place(this._counter.rectTransform, Vector2.one, Vector2.one, Vector2.one, new Vector2(-64f, -23f), new Vector2(240f, 38f));
			this._readout = this.AddText("Camera readout", this._safeArea, "", 17, TextAnchor.MiddleCenter, FlyCameraPhotoViewfinder.Muted);
			FlyCameraPhotoViewfinder.Place(this._readout.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -23f), new Vector2(470f, 38f));
			this._captureHint = this.AddText("Capture", this._safeArea, "[ SPACE ]  TAKE PHOTO", 21, TextAnchor.MiddleLeft, FlyCameraPhotoViewfinder.White);
			FlyCameraPhotoViewfinder.Place(this._captureHint.rectTransform, Vector2.zero, Vector2.zero, Vector2.zero, new Vector2(64f, 47f), new Vector2(390f, 33f));
			Text text = this.AddText("Exit", this._safeArea, "[ K ]  EXIT", 20, TextAnchor.MiddleRight, FlyCameraPhotoViewfinder.White);
			this._exitHint = text;
			FlyCameraPhotoViewfinder.Place(text.rectTransform, Vector2.right, Vector2.right, Vector2.right, new Vector2(-64f, 47f), new Vector2(210f, 33f));
			Text text2 = this.AddText("Movement", this._safeArea, "WASD  MOVE     Q / E  HEIGHT     SHIFT  BOOST     SCROLL  SPEED", 15, TextAnchor.MiddleLeft, FlyCameraPhotoViewfinder.Muted);
			this._movementHint = text2;
			FlyCameraPhotoViewfinder.Place(text2.rectTransform, Vector2.zero, Vector2.zero, Vector2.zero, new Vector2(64f, 17f), new Vector2(960f, 27f));
			this.BuildToast(this._safeArea);
			this._shutter = FlyCameraPhotoViewfinder.AddImage("Shutter", rectTransform, Color.clear);
			FlyCameraPhotoViewfinder.Stretch(this._shutter.rectTransform, Vector2.zero, Vector2.one);
			this._shutter.enabled = false;
			this._canvas.enabled = false;
			this.RefreshSafeArea();
			this.SetCaptureInProgress(this._captureInProgress);
		}

		// Token: 0x06009873 RID: 39027 RVA: 0x003E7B44 File Offset: 0x003E5D44
		private void BuildToast(RectTransform parent)
		{
			RectTransform rectTransform = FlyCameraPhotoViewfinder.AddRect("Save confirmation", parent);
			FlyCameraPhotoViewfinder.Place(rectTransform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0f, 128f), new Vector2(640f, 88f));
			this._toast = rectTransform.gameObject.AddComponent<CanvasGroup>();
			this._toast.interactable = false;
			this._toast.blocksRaycasts = false;
			this._toast.alpha = 0f;
			FlyCameraPhotoViewfinder.Stretch(FlyCameraPhotoViewfinder.AddImage("Background", rectTransform, new Color(0.015f, 0.023f, 0.024f, 0.86f)).rectTransform, Vector2.zero, Vector2.one);
			this._toastTitle = this.AddText("Title", rectTransform, "", 20, TextAnchor.MiddleCenter, FlyCameraPhotoViewfinder.Focused);
			FlyCameraPhotoViewfinder.Place(this._toastTitle.rectTransform, Vector2.up, Vector2.one, Vector2.up, new Vector2(20f, -10f), new Vector2(-40f, 31f));
			this._toastDetail = this.AddText("File", rectTransform, "", 16, TextAnchor.MiddleCenter, FlyCameraPhotoViewfinder.White);
			FlyCameraPhotoViewfinder.Place(this._toastDetail.rectTransform, Vector2.zero, Vector2.right, Vector2.zero, new Vector2(20f, 13f), new Vector2(-40f, 28f));
		}

		// Token: 0x06009874 RID: 39028 RVA: 0x003E7CD8 File Offset: 0x003E5ED8
		private void AddGrid(RectTransform parent)
		{
			Color color = new Color(1f, 1f, 1f, 0.14f);
			for (int i = 1; i <= 2; i++)
			{
				float num = (float)i / 3f;
				FlyCameraPhotoViewfinder.Place(FlyCameraPhotoViewfinder.AddImage("Third vertical " + i.ToString(), parent, color).rectTransform, new Vector2(num, 0f), new Vector2(num, 1f), Vector2.one * 0.5f, Vector2.zero, new Vector2(1f, 0f));
				FlyCameraPhotoViewfinder.Place(FlyCameraPhotoViewfinder.AddImage("Third horizontal " + i.ToString(), parent, color).rectTransform, new Vector2(0f, num), new Vector2(1f, num), Vector2.one * 0.5f, Vector2.zero, new Vector2(0f, 1f));
			}
		}

		// Token: 0x06009875 RID: 39029 RVA: 0x003E7DD4 File Offset: 0x003E5FD4
		private static void AddCorners(RectTransform parent, float length, float thickness, Color color, Image[] store)
		{
			for (int i = 0; i < 4; i++)
			{
				Vector2 vector = new Vector2((float)(i % 2), (float)(i / 2));
				Image image = FlyCameraPhotoViewfinder.AddImage("Corner " + i.ToString() + " horizontal", parent, color);
				FlyCameraPhotoViewfinder.Place(image.rectTransform, vector, vector, vector, Vector2.zero, new Vector2(length, thickness));
				Image image2 = FlyCameraPhotoViewfinder.AddImage("Corner " + i.ToString() + " vertical", parent, color);
				FlyCameraPhotoViewfinder.Place(image2.rectTransform, vector, vector, vector, Vector2.zero, new Vector2(thickness, length));
				if (store != null)
				{
					store[i * 2] = image;
					store[i * 2 + 1] = image2;
				}
			}
		}

		// Token: 0x06009876 RID: 39030 RVA: 0x003E7E88 File Offset: 0x003E6088
		private void ShowToast(string title, string detail, Color color, bool authoredDetail = false)
		{
			LocalizedLegacyGameText.BindSource(this._toastTitle, "camera_ui", title).FitTranslationToRectangle();
			this._toastTitle.color = color;
			if (authoredDetail)
			{
				LocalizedLegacyGameText.BindSource(this._toastDetail, "camera_ui", detail ?? string.Empty).FitTranslationToRectangle();
			}
			else
			{
				LocalizedLegacyGameText.BindFontOnly(this._toastDetail);
				this._toastDetail.text = detail ?? string.Empty;
			}
			this._toastStart = Time.unscaledTime;
			this._toast.alpha = 0f;
		}

		// Token: 0x06009877 RID: 39031 RVA: 0x003E7F18 File Offset: 0x003E6118
		private void RefreshReadout()
		{
			if (this._readout == null)
			{
				return;
			}
			this._readout.text = ((this._camera != null) ? string.Concat(new string[]
			{
				"PNG    |    FOV ",
				Mathf.RoundToInt(this._camera.fieldOfView).ToString(),
				"°    |    ",
				Screen.width.ToString(),
				" x ",
				Screen.height.ToString()
			}) : ("PNG    |    " + Screen.width.ToString() + " x " + Screen.height.ToString()));
		}

		// Token: 0x06009878 RID: 39032 RVA: 0x003E7FD8 File Offset: 0x003E61D8
		private void RefreshSafeArea()
		{
			Vector2Int vector2Int = new Vector2Int(Screen.width, Screen.height);
			Rect safeArea = Screen.safeArea;
			if (vector2Int.x <= 0 || vector2Int.y <= 0 || (safeArea == this._previousSafeArea && vector2Int == this._previousScreenSize))
			{
				return;
			}
			this._previousSafeArea = safeArea;
			this._previousScreenSize = vector2Int;
			this._safeArea.anchorMin = new Vector2(safeArea.xMin / (float)vector2Int.x, safeArea.yMin / (float)vector2Int.y);
			this._safeArea.anchorMax = new Vector2(safeArea.xMax / (float)vector2Int.x, safeArea.yMax / (float)vector2Int.y);
			this._safeArea.offsetMin = Vector2.zero;
			this._safeArea.offsetMax = Vector2.zero;
		}

		// Token: 0x06009879 RID: 39033 RVA: 0x003E80BB File Offset: 0x003E62BB
		private static RectTransform AddRect(string name, Transform parent)
		{
			GameObject gameObject = new GameObject(name, new Type[] { typeof(RectTransform) });
			gameObject.transform.SetParent(parent, false);
			return (RectTransform)gameObject.transform;
		}

		// Token: 0x0600987A RID: 39034 RVA: 0x003E80ED File Offset: 0x003E62ED
		private static Image AddImage(string name, Transform parent, Color color)
		{
			Image image = FlyCameraPhotoViewfinder.AddRect(name, parent).gameObject.AddComponent<Image>();
			image.color = color;
			image.raycastTarget = false;
			return image;
		}

		// Token: 0x0600987B RID: 39035 RVA: 0x003E8110 File Offset: 0x003E6310
		private Text AddText(string name, Transform parent, string value, int fontSize, TextAnchor alignment, Color color)
		{
			RectTransform rectTransform = FlyCameraPhotoViewfinder.AddRect(name, parent);
			Text text = rectTransform.gameObject.AddComponent<Text>();
			text.font = this._font;
			text.text = value;
			text.fontSize = fontSize;
			text.alignment = alignment;
			text.color = color;
			text.raycastTarget = false;
			text.supportRichText = false;
			text.horizontalOverflow = HorizontalWrapMode.Overflow;
			text.verticalOverflow = VerticalWrapMode.Truncate;
			if (string.IsNullOrEmpty(value))
			{
				LocalizedLegacyGameText.BindFontOnly(text);
			}
			else
			{
				LocalizedLegacyGameText.BindSource(text, "camera_ui", value).FitTranslationToRectangle();
			}
			Shadow shadow = rectTransform.gameObject.AddComponent<Shadow>();
			shadow.effectColor = new Color(0f, 0f, 0f, 0.6f);
			shadow.effectDistance = new Vector2(0f, -1f);
			return text;
		}

		// Token: 0x0600987C RID: 39036 RVA: 0x003E81D7 File Offset: 0x003E63D7
		private static void Place(RectTransform rect, Vector2 min, Vector2 max, Vector2 pivot, Vector2 position, Vector2 size)
		{
			rect.anchorMin = min;
			rect.anchorMax = max;
			rect.pivot = pivot;
			rect.anchoredPosition = position;
			rect.sizeDelta = size;
		}

		// Token: 0x0600987D RID: 39037 RVA: 0x003E81FE File Offset: 0x003E63FE
		private static void Stretch(RectTransform rect, Vector2 min, Vector2 max)
		{
			rect.anchorMin = min;
			rect.anchorMax = max;
			rect.offsetMin = Vector2.zero;
			rect.offsetMax = Vector2.zero;
		}

		// Token: 0x04009D7D RID: 40317
		private static readonly Color White = new Color(0.97f, 0.97f, 0.94f, 0.9f);

		// Token: 0x04009D7E RID: 40318
		private static readonly Color Muted = new Color(0.97f, 0.97f, 0.94f, 0.6f);

		// Token: 0x04009D7F RID: 40319
		private static readonly Color Focused = new Color(0.61f, 0.94f, 0.79f, 0.95f);

		// Token: 0x04009D80 RID: 40320
		private Camera _camera;

		// Token: 0x04009D81 RID: 40321
		private Canvas _canvas;

		// Token: 0x04009D82 RID: 40322
		private RectTransform _safeArea;

		// Token: 0x04009D83 RID: 40323
		private Font _font;

		// Token: 0x04009D84 RID: 40324
		private Text _readout;

		// Token: 0x04009D85 RID: 40325
		private Text _counter;

		// Token: 0x04009D86 RID: 40326
		private Text _captureHint;

		// Token: 0x04009D87 RID: 40327
		private Text _exitHint;

		// Token: 0x04009D88 RID: 40328
		private Text _movementHint;

		// Token: 0x04009D89 RID: 40329
		private Text _toastTitle;

		// Token: 0x04009D8A RID: 40330
		private Text _toastDetail;

		// Token: 0x04009D8B RID: 40331
		private CanvasGroup _toast;

		// Token: 0x04009D8C RID: 40332
		private Image _shutter;

		// Token: 0x04009D8D RID: 40333
		private readonly Image[] _focusCorners = new Image[8];

		// Token: 0x04009D8E RID: 40334
		private Rect _previousSafeArea;

		// Token: 0x04009D8F RID: 40335
		private Vector2Int _previousScreenSize;

		// Token: 0x04009D90 RID: 40336
		private float _shutterStart = float.NegativeInfinity;

		// Token: 0x04009D91 RID: 40337
		private float _toastStart = float.NegativeInfinity;

		// Token: 0x04009D92 RID: 40338
		private float _nextReadout;

		// Token: 0x04009D93 RID: 40339
		private int _photosTaken;

		// Token: 0x04009D94 RID: 40340
		private bool _captureInProgress;

		// Token: 0x04009D95 RID: 40341
		private string _exitKey = "K";

		// Token: 0x04009D96 RID: 40342
		private bool _showSpeedControl = true;

		// Token: 0x04009D97 RID: 40343
		private bool _hintController;

		// Token: 0x04009D98 RID: 40344
		private int _hintVersion = -1;
	}
}
