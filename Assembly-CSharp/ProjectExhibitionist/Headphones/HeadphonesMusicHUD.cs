using System;
using System.IO;
using Features.CielOS;
using Features.CielOS.UI;
using Features.Localization;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectExhibitionist.Headphones
{
	// Token: 0x020009D6 RID: 2518
	[DisallowMultipleComponent]
	[RequireComponent(typeof(RectTransform))]
	[RequireComponent(typeof(Canvas))]
	public sealed class HeadphonesMusicHUD : MonoBehaviour
	{
		// Token: 0x060046EC RID: 18156 RVA: 0x001ACB2C File Offset: 0x001AAD2C
		public static HeadphonesMusicHUD GetOrAdd(Transform hudParent)
		{
			if (hudParent == null)
			{
				return null;
			}
			HeadphonesMusicHUD componentInChildren = hudParent.GetComponentInChildren<HeadphonesMusicHUD>(true);
			if (componentInChildren != null)
			{
				return componentInChildren;
			}
			GameObject gameObject = new GameObject("Headphones Music HUD", new Type[]
			{
				typeof(RectTransform),
				typeof(Canvas)
			});
			gameObject.layer = hudParent.gameObject.layer;
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.SetParent(hudParent, false);
			component.anchorMin = Vector2.zero;
			component.anchorMax = Vector2.zero;
			component.pivot = Vector2.zero;
			component.anchoredPosition = HeadphonesMusicHUD.DefaultOffset;
			component.sizeDelta = HeadphonesMusicHUD.DefaultSize;
			return gameObject.AddComponent<HeadphonesMusicHUD>();
		}

		// Token: 0x060046ED RID: 18157 RVA: 0x001ACBDB File Offset: 0x001AADDB
		private void Awake()
		{
			this.BuildVisuals();
		}

		// Token: 0x060046EE RID: 18158 RVA: 0x001ACBE3 File Offset: 0x001AADE3
		private void OnEnable()
		{
			this.BuildVisuals();
			HeadphonesEquipment.WearStateChanged += this.HandleWearStateChanged;
			this.SetWornState(HeadphonesEquipment.IsWorn, true);
		}

		// Token: 0x060046EF RID: 18159 RVA: 0x001ACC08 File Offset: 0x001AAE08
		private void OnDisable()
		{
			HeadphonesEquipment.WearStateChanged -= this.HandleWearStateChanged;
			this.BindAudioService(null);
			this._worn = false;
			this.SetVisualVisible(false);
		}

		// Token: 0x060046F0 RID: 18160 RVA: 0x001ACC30 File Offset: 0x001AAE30
		private void Update()
		{
			bool isWorn = HeadphonesEquipment.IsWorn;
			if (isWorn != this._worn)
			{
				this.SetWornState(isWorn, false);
			}
			if (!this._worn)
			{
				return;
			}
			bool isActive = ControllerInput.IsActive;
			if (isActive != this._controllerLegend)
			{
				this._controllerLegend = isActive;
				LocalizedGameText shortcutBinding = this._shortcutBinding;
				if (shortcutBinding != null)
				{
					shortcutBinding.Refresh();
				}
			}
			CielOSAudioService instance = CielOSAudioService.Instance;
			if (this._audioService == null || instance == null || this._audioService != instance)
			{
				this.BindAudioService(CielOSAudioService.Ensure(null));
				this.Refresh();
			}
			if (Time.unscaledTime >= this._nextProgressRefreshTime)
			{
				this._nextProgressRefreshTime = Time.unscaledTime + 0.25f;
				this.RefreshProgress(false);
			}
		}

		// Token: 0x060046F1 RID: 18161 RVA: 0x001ACCE6 File Offset: 0x001AAEE6
		private void HandleWearStateChanged(bool isWorn)
		{
			this.SetWornState(isWorn, false);
		}

		// Token: 0x060046F2 RID: 18162 RVA: 0x001ACCF0 File Offset: 0x001AAEF0
		private void SetWornState(bool isWorn, bool force)
		{
			if (!force && this._worn == isWorn)
			{
				return;
			}
			this._worn = isWorn;
			this.SetVisualVisible(this._worn);
			if (!this._worn)
			{
				this.BindAudioService(null);
				return;
			}
			this.BindAudioService(CielOSAudioService.Ensure(null));
			this.Refresh();
		}

		// Token: 0x060046F3 RID: 18163 RVA: 0x001ACD40 File Offset: 0x001AAF40
		private void BindAudioService(CielOSAudioService service)
		{
			if (this._audioService == service)
			{
				return;
			}
			if (this._audioService != null)
			{
				this._audioService.TrackChanged -= this.Refresh;
				this._audioService.StateChanged -= this.Refresh;
			}
			this._audioService = service;
			if (this._audioService != null)
			{
				this._audioService.TrackChanged += this.Refresh;
				this._audioService.StateChanged += this.Refresh;
			}
		}

		// Token: 0x060046F4 RID: 18164 RVA: 0x001ACDDC File Offset: 0x001AAFDC
		public void Refresh()
		{
			if (!this._worn || this._transportStateIcon == null)
			{
				return;
			}
			string text = ((this._audioService != null) ? this._audioService.CurrentName : null);
			string text2 = HeadphonesMusicHUD.DisplayNameFor(text);
			bool flag = !string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(Path.GetFileNameWithoutExtension(text));
			if (this._trackTitle != null && (this._shownTrackTitle != text2 || this._shownTrackHasName != flag))
			{
				this._shownTrackTitle = text2;
				this._shownTrackHasName = flag;
				LocalizedGameText trackTitleBinding = this._trackTitleBinding;
				if (trackTitleBinding != null)
				{
					trackTitleBinding.Refresh();
				}
			}
			CielOSIconId cielOSIconId = ((this._audioService != null && (this._audioService.IsPlaying || this._audioService.IsLoading)) ? CielOSIconId.Pause : CielOSIconId.Play);
			CielOSIconId? shownTransportIcon = this._shownTransportIcon;
			CielOSIconId cielOSIconId2 = cielOSIconId;
			if (!((shownTransportIcon.GetValueOrDefault() == cielOSIconId2) & (shownTransportIcon != null)))
			{
				CielOSIcon.Apply(this._transportStateIcon, cielOSIconId, this.centerGlyphColor, 64);
				this._transportStateIcon.raycastTarget = false;
				this._shownTransportIcon = new CielOSIconId?(cielOSIconId);
			}
			this.RefreshProgress(true);
		}

		// Token: 0x060046F5 RID: 18165 RVA: 0x001ACF0C File Offset: 0x001AB10C
		private static string DisplayNameFor(string currentName)
		{
			if (string.IsNullOrWhiteSpace(currentName))
			{
				return "No track";
			}
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(currentName);
			if (!string.IsNullOrWhiteSpace(fileNameWithoutExtension))
			{
				return fileNameWithoutExtension;
			}
			return "No track";
		}

		// Token: 0x060046F6 RID: 18166 RVA: 0x001ACF40 File Offset: 0x001AB140
		private void RefreshProgress(bool force = false)
		{
			float num = ((this._audioService != null) ? this._audioService.PlaybackProgress : 0f);
			num = Mathf.Clamp01(num);
			if (!force && Mathf.Abs(num - this._shownProgress) < 0.0036764706f)
			{
				return;
			}
			this._shownProgress = num;
			if (this._progressFill != null)
			{
				this._progressFill.anchorMax = new Vector2(num, 1f);
				this._progressFill.offsetMin = Vector2.zero;
				this._progressFill.offsetMax = Vector2.zero;
			}
			if (this._progressKnob != null)
			{
				RectTransform progressKnob = this._progressKnob;
				RectTransform progressKnob2 = this._progressKnob;
				Vector2 vector = new Vector2(num, 0.5f);
				progressKnob2.anchorMax = vector;
				progressKnob.anchorMin = vector;
				this._progressKnob.anchoredPosition = Vector2.zero;
			}
		}

		// Token: 0x060046F7 RID: 18167 RVA: 0x001AD01C File Offset: 0x001AB21C
		private void BuildVisuals()
		{
			if (this._visualRoot != null)
			{
				return;
			}
			((RectTransform)base.transform).sizeDelta = HeadphonesMusicHUD.DefaultSize;
			Canvas canvas = base.GetComponent<Canvas>();
			if (canvas == null)
			{
				canvas = base.gameObject.AddComponent<Canvas>();
			}
			canvas.overrideSorting = false;
			GameplayHudVisibility.RegisterCanvas(canvas);
			GameObject gameObject = this.NewUIObject("Visual", base.transform, new Type[]
			{
				typeof(CanvasRenderer),
				typeof(Image),
				typeof(CanvasGroup)
			});
			RectTransform component = gameObject.GetComponent<RectTransform>();
			HeadphonesMusicHUD.Stretch(component);
			Image component2 = gameObject.GetComponent<Image>();
			component2.color = this.backgroundColor;
			component2.sprite = CielOSShapes.RoundedRect(28);
			component2.type = Image.Type.Sliced;
			component2.pixelsPerUnitMultiplier = 1f;
			component2.raycastTarget = false;
			CanvasGroup component3 = gameObject.GetComponent<CanvasGroup>();
			component3.interactable = false;
			component3.blocksRaycasts = false;
			component3.ignoreParentGroups = false;
			this._visualRoot = gameObject;
			RectTransform rectTransform = this.CreateImage(component, "Progress Track", this.railColor);
			rectTransform.anchorMin = new Vector2(0f, 1f);
			rectTransform.anchorMax = new Vector2(1f, 1f);
			rectTransform.pivot = new Vector2(0.5f, 0.5f);
			rectTransform.anchoredPosition = new Vector2(0f, -16f);
			rectTransform.sizeDelta = new Vector2(-48f, 2.5f);
			this._progressFill = this.CreateImage(rectTransform, "Progress Fill", this.progressColor);
			this._progressFill.anchorMin = Vector2.zero;
			this._progressFill.anchorMax = new Vector2(0f, 1f);
			this._progressFill.pivot = new Vector2(0f, 0.5f);
			this._progressFill.offsetMin = Vector2.zero;
			this._progressFill.offsetMax = Vector2.zero;
			this._progressKnob = this.CreateImage(rectTransform, "Progress Knob", this.progressColor);
			RectTransform progressKnob = this._progressKnob;
			RectTransform progressKnob2 = this._progressKnob;
			Vector2 vector = new Vector2(0f, 0.5f);
			progressKnob2.anchorMax = vector;
			progressKnob.anchorMin = vector;
			this._progressKnob.pivot = new Vector2(0.5f, 0.5f);
			this._progressKnob.anchoredPosition = Vector2.zero;
			this._progressKnob.sizeDelta = new Vector2(9f, 9f);
			Image component4 = this._progressKnob.GetComponent<Image>();
			component4.sprite = CielOSShapes.Circle(64);
			component4.type = Image.Type.Simple;
			component4.preserveAspect = true;
			GameObject gameObject2 = this.NewUIObject("Track Name", component, new Type[] { typeof(TextMeshProUGUI) });
			RectTransform component5 = gameObject2.GetComponent<RectTransform>();
			component5.anchorMin = new Vector2(0f, 1f);
			component5.anchorMax = new Vector2(1f, 1f);
			component5.pivot = new Vector2(0.5f, 0.5f);
			component5.anchoredPosition = new Vector2(0f, -34f);
			component5.sizeDelta = new Vector2(-48f, 18f);
			this._trackTitle = gameObject2.GetComponent<TextMeshProUGUI>();
			this._trackTitle.text = "No track";
			this._trackTitle.fontSize = 13f;
			this._trackTitle.fontStyle = FontStyles.Normal;
			this._trackTitle.color = this.titleColor;
			this._trackTitle.alignment = TextAlignmentOptions.Center;
			this._trackTitle.enableWordWrapping = false;
			this._trackTitle.overflowMode = TextOverflowModes.Ellipsis;
			this._trackTitle.raycastTarget = false;
			this._shownTrackTitle = this._trackTitle.text;
			this._shownTrackHasName = false;
			this._trackTitleBinding = LocalizedGameText.BindComputed(this._trackTitle, delegate
			{
				if (!this._shownTrackHasName)
				{
					return GameLocalization.GetSource("phone_story", "No track");
				}
				return this._shownTrackTitle;
			});
			LocalizedGameText trackTitleBinding = this._trackTitleBinding;
			if (trackTitleBinding != null)
			{
				trackTitleBinding.FitTranslationToRectangle(false);
			}
			GameObject gameObject3 = this.NewUIObject("Keyboard Shortcuts", component, new Type[] { typeof(TextMeshProUGUI) });
			RectTransform component6 = gameObject3.GetComponent<RectTransform>();
			component6.anchorMin = new Vector2(0f, 0f);
			component6.anchorMax = new Vector2(1f, 0f);
			component6.pivot = new Vector2(0.5f, 0f);
			component6.anchoredPosition = new Vector2(0f, 6f);
			component6.sizeDelta = new Vector2(-32f, 22f);
			TextMeshProUGUI component7 = gameObject3.GetComponent<TextMeshProUGUI>();
			component7.text = "F1  PREVIOUS   |   F2  PLAY / PAUSE   |   F3  NEXT\nPAGE UP  VOLUME +        PAGE DOWN  VOLUME -";
			component7.fontSize = 8.5f;
			component7.fontStyle = FontStyles.Normal;
			component7.color = this.shortcutColor;
			component7.alignment = TextAlignmentOptions.Center;
			component7.enableWordWrapping = false;
			component7.overflowMode = TextOverflowModes.Ellipsis;
			component7.raycastTarget = false;
			this._shortcutBinding = LocalizedGameText.BindComputed(component7, delegate
			{
				if (!ControllerInput.IsActive)
				{
					return GameLocalization.GetSource("world_ui", "F1  PREVIOUS   |   F2  PLAY / PAUSE   |   F3  NEXT\nPAGE UP  VOLUME +        PAGE DOWN  VOLUME -");
				}
				return ControllerGlyphs.Format("D-Pad Up") + "  " + GameLocalization.GetSource("inventory_ui", "Select headphones in inventory");
			});
			this._shortcutBinding.FitTranslationToRectangle(false);
			this.CreateTransportIcon(component, "Previous", CielOSIconId.SkipPrev, new Vector2(0.5f, 1f), new Vector2(-70f, -66f), new Vector2(26f, 26f));
			RectTransform rectTransform2 = this.CreateImage(component, "Center Control", this.centerButtonColor);
			RectTransform rectTransform3 = rectTransform2;
			RectTransform rectTransform4 = rectTransform2;
			vector = new Vector2(0.5f, 1f);
			rectTransform4.anchorMax = vector;
			rectTransform3.anchorMin = vector;
			rectTransform2.pivot = new Vector2(0.5f, 0.5f);
			rectTransform2.anchoredPosition = new Vector2(0f, -66f);
			rectTransform2.sizeDelta = new Vector2(40f, 40f);
			Image component8 = rectTransform2.GetComponent<Image>();
			component8.sprite = CielOSShapes.Circle(64);
			component8.type = Image.Type.Simple;
			component8.preserveAspect = true;
			RectTransform rectTransform5 = this.CreateImage(rectTransform2, "Transport State", Color.white);
			RectTransform rectTransform6 = rectTransform5;
			RectTransform rectTransform7 = rectTransform5;
			vector = new Vector2(0.5f, 0.5f);
			rectTransform7.anchorMax = vector;
			rectTransform6.anchorMin = vector;
			rectTransform5.pivot = new Vector2(0.5f, 0.5f);
			rectTransform5.anchoredPosition = Vector2.zero;
			rectTransform5.sizeDelta = new Vector2(18f, 18f);
			this._transportStateIcon = rectTransform5.GetComponent<Image>();
			CielOSIcon.Apply(this._transportStateIcon, CielOSIconId.Play, this.centerGlyphColor, 64);
			this._transportStateIcon.raycastTarget = false;
			this._shownTransportIcon = new CielOSIconId?(CielOSIconId.Play);
			this.CreateTransportIcon(component, "Next", CielOSIconId.SkipNext, new Vector2(0.5f, 1f), new Vector2(70f, -66f), new Vector2(26f, 26f));
			this.RefreshProgress(true);
			this.SetVisualVisible(false);
		}

		// Token: 0x060046F8 RID: 18168 RVA: 0x001AD6FC File Offset: 0x001AB8FC
		private RectTransform CreateTransportIcon(RectTransform parent, string name, CielOSIconId icon, Vector2 anchor, Vector2 position, Vector2 size)
		{
			RectTransform rectTransform = this.CreateImage(parent, name, Color.white);
			rectTransform.anchorMax = anchor;
			rectTransform.anchorMin = anchor;
			rectTransform.pivot = new Vector2(0.5f, 0.5f);
			rectTransform.anchoredPosition = position;
			rectTransform.sizeDelta = size;
			Image component = rectTransform.GetComponent<Image>();
			CielOSIcon.Apply(component, icon, this.controlIconColor, 64);
			component.raycastTarget = false;
			return rectTransform;
		}

		// Token: 0x060046F9 RID: 18169 RVA: 0x001AD768 File Offset: 0x001AB968
		private RectTransform CreateImage(Transform parent, string name, Color color)
		{
			GameObject gameObject = this.NewUIObject(name, parent, new Type[]
			{
				typeof(CanvasRenderer),
				typeof(Image)
			});
			Image component = gameObject.GetComponent<Image>();
			component.color = color;
			component.raycastTarget = false;
			return gameObject.GetComponent<RectTransform>();
		}

		// Token: 0x060046FA RID: 18170 RVA: 0x001AD7B8 File Offset: 0x001AB9B8
		private GameObject NewUIObject(string name, Transform parent, params Type[] components)
		{
			GameObject gameObject = new GameObject(name, new Type[] { typeof(RectTransform) });
			gameObject.layer = base.gameObject.layer;
			gameObject.transform.SetParent(parent, false);
			foreach (Type type in components)
			{
				if (type != typeof(RectTransform) && gameObject.GetComponent(type) == null)
				{
					gameObject.AddComponent(type);
				}
			}
			return gameObject;
		}

		// Token: 0x060046FB RID: 18171 RVA: 0x001AD839 File Offset: 0x001ABA39
		private void SetVisualVisible(bool visible)
		{
			if (this._visualRoot != null && this._visualRoot.activeSelf != visible)
			{
				this._visualRoot.SetActive(visible);
			}
		}

		// Token: 0x060046FC RID: 18172 RVA: 0x001A38AD File Offset: 0x001A1AAD
		private static void Stretch(RectTransform rect)
		{
			rect.anchorMin = Vector2.zero;
			rect.anchorMax = Vector2.one;
			rect.offsetMin = Vector2.zero;
			rect.offsetMax = Vector2.zero;
		}

		// Token: 0x04004676 RID: 18038
		public const string RuntimeObjectName = "Headphones Music HUD";

		// Token: 0x04004677 RID: 18039
		public const string ShortcutLegend = "F1  PREVIOUS   |   F2  PLAY / PAUSE   |   F3  NEXT\nPAGE UP  VOLUME +        PAGE DOWN  VOLUME -";

		// Token: 0x04004678 RID: 18040
		private static readonly Vector2 DefaultSize = new Vector2(320f, 128f);

		// Token: 0x04004679 RID: 18041
		private static readonly Vector2 DefaultOffset = new Vector2(24f, 84f);

		// Token: 0x0400467A RID: 18042
		private const float ProgressRefreshInterval = 0.25f;

		// Token: 0x0400467B RID: 18043
		private const float ProgressPixelStep = 0.0036764706f;

		// Token: 0x0400467C RID: 18044
		[Header("Pill palette")]
		[SerializeField]
		private Color backgroundColor = new Color(0.055f, 0.045f, 0.06f, 0.72f);

		// Token: 0x0400467D RID: 18045
		[SerializeField]
		private Color railColor = new Color(1f, 1f, 1f, 0.42f);

		// Token: 0x0400467E RID: 18046
		[SerializeField]
		private Color progressColor = new Color(0.98f, 0.97f, 0.99f, 1f);

		// Token: 0x0400467F RID: 18047
		[SerializeField]
		private Color titleColor = new Color(0.98f, 0.97f, 0.99f, 0.92f);

		// Token: 0x04004680 RID: 18048
		[SerializeField]
		private Color shortcutColor = new Color(0.98f, 0.97f, 0.99f, 0.62f);

		// Token: 0x04004681 RID: 18049
		[SerializeField]
		private Color controlIconColor = new Color(0.98f, 0.97f, 0.99f, 1f);

		// Token: 0x04004682 RID: 18050
		[SerializeField]
		private Color centerButtonColor = new Color(0.98f, 0.97f, 0.99f, 1f);

		// Token: 0x04004683 RID: 18051
		[SerializeField]
		private Color centerGlyphColor = new Color(0.075f, 0.06f, 0.08f, 1f);

		// Token: 0x04004684 RID: 18052
		private GameObject _visualRoot;

		// Token: 0x04004685 RID: 18053
		private RectTransform _progressFill;

		// Token: 0x04004686 RID: 18054
		private RectTransform _progressKnob;

		// Token: 0x04004687 RID: 18055
		private TMP_Text _trackTitle;

		// Token: 0x04004688 RID: 18056
		private Image _transportStateIcon;

		// Token: 0x04004689 RID: 18057
		private CielOSAudioService _audioService;

		// Token: 0x0400468A RID: 18058
		private CielOSIconId? _shownTransportIcon;

		// Token: 0x0400468B RID: 18059
		private string _shownTrackTitle;

		// Token: 0x0400468C RID: 18060
		private bool _shownTrackHasName;

		// Token: 0x0400468D RID: 18061
		private LocalizedGameText _trackTitleBinding;

		// Token: 0x0400468E RID: 18062
		private float _shownProgress = -1f;

		// Token: 0x0400468F RID: 18063
		private float _nextProgressRefreshTime;

		// Token: 0x04004690 RID: 18064
		private bool _worn;

		// Token: 0x04004691 RID: 18065
		private bool _controllerLegend;

		// Token: 0x04004692 RID: 18066
		private LocalizedGameText _shortcutBinding;
	}
}
