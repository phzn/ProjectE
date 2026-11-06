using System;
using System.Collections;
using Features.Localization;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectExhibitionist.UI
{
	// Token: 0x0200091B RID: 2331
	public class ToastService : MonoBehaviour
	{
		// Token: 0x17000AA0 RID: 2720
		// (get) Token: 0x06003F24 RID: 16164 RVA: 0x00176B4E File Offset: 0x00174D4E
		// (set) Token: 0x06003F25 RID: 16165 RVA: 0x00176B55 File Offset: 0x00174D55
		public static ToastService Instance { get; private set; }

		// Token: 0x06003F26 RID: 16166 RVA: 0x00176B60 File Offset: 0x00174D60
		private void Awake()
		{
			if (ToastService.Instance == null)
			{
				ToastService.Instance = this;
				if (this.persistAcrossScenes)
				{
					base.transform.SetParent(null, false);
					Object.DontDestroyOnLoad(base.gameObject);
					return;
				}
			}
			else
			{
				Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x06003F27 RID: 16167 RVA: 0x00176BAC File Offset: 0x00174DAC
		private void OnDestroy()
		{
			if (ToastService.Instance == this)
			{
				ToastService.Instance = null;
			}
		}

		// Token: 0x06003F28 RID: 16168 RVA: 0x00176BC4 File Offset: 0x00174DC4
		public static void Show(string message, string title = null, Sprite icon = null, float duration = -1f, string localizationScope = null)
		{
			ToastService toastService = ToastService.EnsureInstance();
			if (toastService == null)
			{
				Debug.Log("[Toast] " + (title ?? "Info") + ": " + message);
				return;
			}
			toastService.ShowInternal(message, title, duration, localizationScope, null, null, null);
		}

		// Token: 0x06003F29 RID: 16169 RVA: 0x00176C10 File Offset: 0x00174E10
		public static void ShowFormat(string scope, string english, object[] arguments, string title = null, Sprite icon = null, float duration = -1f)
		{
			ToastService toastService = ToastService.EnsureInstance();
			if (toastService == null)
			{
				return;
			}
			toastService.ShowInternal(english, title, duration, scope, arguments ?? Array.Empty<object>(), null, null);
		}

		// Token: 0x06003F2A RID: 16170 RVA: 0x00176C44 File Offset: 0x00174E44
		public static void ShowComputed(Func<string> message, string title = null, float duration = -1f, string localizationScope = null, Func<string> titleResolver = null)
		{
			ToastService toastService = ToastService.EnsureInstance();
			if (toastService == null || message == null)
			{
				return;
			}
			toastService.ShowInternal(message(), title, duration, localizationScope, null, message, titleResolver);
		}

		// Token: 0x06003F2B RID: 16171 RVA: 0x00176C78 File Offset: 0x00174E78
		private void ShowInternal(string message, string title, float duration, string localizationScope, object[] arguments = null, Func<string> resolver = null, Func<string> titleResolver = null)
		{
			this.EnsureUiBuilt();
			this.SetText(message, title, localizationScope, arguments, resolver, titleResolver);
			float num = ((duration > 0f) ? duration : this.defaultDuration);
			if (this._activeRoutine != null)
			{
				base.StopCoroutine(this._activeRoutine);
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(this._panelRect);
			Canvas.ForceUpdateCanvases();
			this._activeRoutine = base.StartCoroutine(this.RunToast(num));
		}

		// Token: 0x06003F2C RID: 16172 RVA: 0x00176CE4 File Offset: 0x00174EE4
		private void SetText(string message, string title, string localizationScope, object[] arguments, Func<string> resolver, Func<string> titleResolver)
		{
			this._titleText.text = (string.IsNullOrWhiteSpace(title) ? this.defaultTitle : title);
			this._descriptionText.text = message;
			if (localizationScope != null)
			{
				LocalizedGameText.BindSource(this._titleText, localizationScope, this._titleText.text, null);
				if (arguments == null)
				{
					LocalizedGameText.BindSource(this._descriptionText, localizationScope, message, null);
				}
				else
				{
					LocalizedGameText.BindFormat(this._descriptionText, localizationScope, message, arguments);
				}
			}
			else
			{
				LocalizedGameText.BindFontOnly(this._titleText);
				LocalizedGameText.BindFontOnly(this._descriptionText);
			}
			if (resolver != null)
			{
				LocalizedGameText.BindComputed(this._descriptionText, resolver);
			}
			if (titleResolver != null)
			{
				LocalizedGameText.BindComputed(this._titleText, titleResolver);
			}
		}

		// Token: 0x06003F2D RID: 16173 RVA: 0x00176D98 File Offset: 0x00174F98
		private IEnumerator RunToast(float visibleSeconds)
		{
			this._overlayCanvas.gameObject.SetActive(true);
			yield return this.Fade(0f, 1f, this.fadeInSeconds);
			yield return new WaitForSecondsRealtime(visibleSeconds);
			yield return this.Fade(1f, 0f, this.fadeOutSeconds);
			this._overlayCanvas.gameObject.SetActive(false);
			this._activeRoutine = null;
			yield break;
		}

		// Token: 0x06003F2E RID: 16174 RVA: 0x00176DAE File Offset: 0x00174FAE
		private IEnumerator Fade(float from, float to, float seconds)
		{
			if (seconds <= 0f)
			{
				this._panelGroup.alpha = to;
				yield break;
			}
			float t = 0f;
			while (t < seconds)
			{
				t += Time.unscaledDeltaTime;
				this._panelGroup.alpha = Mathf.Lerp(from, to, Mathf.Clamp01(t / seconds));
				yield return null;
			}
			this._panelGroup.alpha = to;
			yield break;
		}

		// Token: 0x06003F2F RID: 16175 RVA: 0x00176DD4 File Offset: 0x00174FD4
		private void EnsureUiBuilt()
		{
			if (this._overlayCanvas != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("ToastOverlayCanvas", new Type[]
			{
				typeof(Canvas),
				typeof(CanvasScaler),
				typeof(GraphicRaycaster)
			});
			gameObject.transform.SetParent(base.transform, false);
			gameObject.layer = LayerMask.NameToLayer("UI");
			this._overlayCanvas = gameObject.GetComponent<Canvas>();
			this._overlayCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
			this._overlayCanvas.sortingOrder = 32000;
			GameplayHudVisibility.RegisterCanvas(this._overlayCanvas);
			CanvasScaler component = gameObject.GetComponent<CanvasScaler>();
			component.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			component.referenceResolution = new Vector2(1920f, 1080f);
			component.matchWidthOrHeight = 0.5f;
			GameObject gameObject2 = new GameObject("ToastPanel", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasGroup),
				typeof(Image),
				typeof(HorizontalLayoutGroup),
				typeof(ContentSizeFitter)
			});
			gameObject2.transform.SetParent(gameObject.transform, false);
			this._panelRect = gameObject2.GetComponent<RectTransform>();
			this._panelRect.anchorMin = new Vector2(0.5f, 1f);
			this._panelRect.anchorMax = new Vector2(0.5f, 1f);
			this._panelRect.pivot = new Vector2(0.5f, 1f);
			this._panelRect.anchoredPosition = new Vector2(0f, -40f);
			gameObject2.GetComponent<Image>().color = this.backdropColor;
			this._panelGroup = gameObject2.GetComponent<CanvasGroup>();
			this._panelGroup.alpha = 0f;
			this._panelGroup.interactable = false;
			this._panelGroup.blocksRaycasts = false;
			HorizontalLayoutGroup component2 = gameObject2.GetComponent<HorizontalLayoutGroup>();
			component2.padding = new RectOffset(0, 18, 0, 0);
			component2.spacing = 14f;
			component2.childAlignment = TextAnchor.MiddleLeft;
			component2.childControlWidth = true;
			component2.childControlHeight = true;
			component2.childForceExpandWidth = false;
			component2.childForceExpandHeight = false;
			ContentSizeFitter component3 = gameObject2.GetComponent<ContentSizeFitter>();
			component3.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
			component3.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			GameObject gameObject3 = new GameObject("Accent", new Type[]
			{
				typeof(RectTransform),
				typeof(Image),
				typeof(LayoutElement)
			});
			gameObject3.transform.SetParent(gameObject2.transform, false);
			gameObject3.GetComponent<Image>().color = this.accentColor;
			LayoutElement component4 = gameObject3.GetComponent<LayoutElement>();
			component4.minWidth = 6f;
			component4.preferredWidth = 6f;
			component4.flexibleHeight = 1f;
			GameObject gameObject4 = new GameObject("Text", new Type[]
			{
				typeof(RectTransform),
				typeof(VerticalLayoutGroup),
				typeof(LayoutElement)
			});
			gameObject4.transform.SetParent(gameObject2.transform, false);
			VerticalLayoutGroup component5 = gameObject4.GetComponent<VerticalLayoutGroup>();
			component5.padding = new RectOffset(0, 0, 14, 14);
			component5.spacing = 4f;
			component5.childAlignment = TextAnchor.MiddleLeft;
			component5.childControlWidth = true;
			component5.childControlHeight = true;
			component5.childForceExpandWidth = false;
			component5.childForceExpandHeight = false;
			LayoutElement component6 = gameObject4.GetComponent<LayoutElement>();
			component6.minWidth = 220f;
			component6.preferredWidth = 360f;
			this._titleText = ToastService.CreateText(gameObject4.transform, "Title", 22f, FontStyles.Bold, this.titleColor);
			this._descriptionText = ToastService.CreateText(gameObject4.transform, "Description", 17f, FontStyles.Normal, this.descriptionColor);
			this._overlayCanvas.gameObject.SetActive(false);
		}

		// Token: 0x06003F30 RID: 16176 RVA: 0x0017719C File Offset: 0x0017539C
		private static TextMeshProUGUI CreateText(Transform parent, string name, float size, FontStyles style, Color color)
		{
			GameObject gameObject = new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(TextMeshProUGUI)
			});
			gameObject.transform.SetParent(parent, false);
			TextMeshProUGUI component = gameObject.GetComponent<TextMeshProUGUI>();
			component.fontSize = size;
			component.fontStyle = style;
			component.color = color;
			component.enableWordWrapping = true;
			component.overflowMode = TextOverflowModes.Overflow;
			component.alignment = TextAlignmentOptions.MidlineLeft;
			return component;
		}

		// Token: 0x06003F31 RID: 16177 RVA: 0x00177210 File Offset: 0x00175410
		private static ToastService EnsureInstance()
		{
			if (ToastService.Instance != null && ToastService.Instance.isActiveAndEnabled)
			{
				return ToastService.Instance;
			}
			ToastService toastService = Object.FindObjectOfType<ToastService>(false);
			if (toastService != null && toastService.isActiveAndEnabled)
			{
				ToastService.Instance = toastService;
				return ToastService.Instance;
			}
			ToastService.Instance = new GameObject("[ToastService Runtime]").AddComponent<ToastService>();
			return ToastService.Instance;
		}

		// Token: 0x04003F07 RID: 16135
		private const string RuntimeServiceName = "[ToastService Runtime]";

		// Token: 0x04003F08 RID: 16136
		[Header("Defaults")]
		[SerializeField]
		private string defaultTitle = "Info";

		// Token: 0x04003F09 RID: 16137
		[SerializeField]
		private float defaultDuration = 2.5f;

		// Token: 0x04003F0A RID: 16138
		[Header("Appearance")]
		[Tooltip("Backdrop color of the toast panel.")]
		[SerializeField]
		private Color backdropColor = new Color(0.16f, 0.12f, 0.22f, 0.96f);

		// Token: 0x04003F0B RID: 16139
		[Tooltip("Accent strip color on the left of the panel.")]
		[SerializeField]
		private Color accentColor = new Color(0.62f, 0.36f, 0.88f, 1f);

		// Token: 0x04003F0C RID: 16140
		[SerializeField]
		private Color titleColor = new Color(1f, 1f, 1f, 1f);

		// Token: 0x04003F0D RID: 16141
		[SerializeField]
		private Color descriptionColor = new Color(0.85f, 0.85f, 0.9f, 1f);

		// Token: 0x04003F0E RID: 16142
		[SerializeField]
		private float fadeInSeconds = 0.18f;

		// Token: 0x04003F0F RID: 16143
		[SerializeField]
		private float fadeOutSeconds = 0.25f;

		// Token: 0x04003F10 RID: 16144
		[Header("Runtime")]
		[SerializeField]
		private bool persistAcrossScenes = true;

		// Token: 0x04003F11 RID: 16145
		private Canvas _overlayCanvas;

		// Token: 0x04003F12 RID: 16146
		private CanvasGroup _panelGroup;

		// Token: 0x04003F13 RID: 16147
		private RectTransform _panelRect;

		// Token: 0x04003F14 RID: 16148
		private TextMeshProUGUI _titleText;

		// Token: 0x04003F15 RID: 16149
		private TextMeshProUGUI _descriptionText;

		// Token: 0x04003F16 RID: 16150
		private Coroutine _activeRoutine;
	}
}
