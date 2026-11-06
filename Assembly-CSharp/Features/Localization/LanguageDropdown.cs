using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.Localization
{
	// Token: 0x020010CA RID: 4298
	public sealed class LanguageDropdown : TMP_Dropdown
	{
		// Token: 0x0600824C RID: 33356 RVA: 0x003538BC File Offset: 0x00351ABC
		public void SetLanguages(GameLocalizationConfig.Language[] languages, TMP_FontAsset sourceFont)
		{
			this._languages = languages ?? Array.Empty<GameLocalizationConfig.Language>();
			this._sourceFont = ((sourceFont != null) ? sourceFont : TMP_Settings.defaultFontAsset);
			base.ClearOptions();
			base.AddOptions(this._languages.Select<GameLocalizationConfig.Language, string>((GameLocalizationConfig.Language language) => language.nativeName).ToList<string>());
			base.onValueChanged.RemoveListener(new UnityAction<int>(this.HandleValueChanged));
			base.onValueChanged.AddListener(new UnityAction<int>(this.HandleValueChanged));
			this.SetIndexWithoutNotify(base.value);
		}

		// Token: 0x0600824D RID: 33357 RVA: 0x00353965 File Offset: 0x00351B65
		public void SetIndexWithoutNotify(int index)
		{
			base.SetValueWithoutNotify(index);
			this.RefreshCaptionFont(base.value);
		}

		// Token: 0x0600824E RID: 33358 RVA: 0x0035397A File Offset: 0x00351B7A
		private void RefreshCaptionFont(int index)
		{
			this.ApplyFont(base.captionText, index);
		}

		// Token: 0x0600824F RID: 33359 RVA: 0x00353989 File Offset: 0x00351B89
		private void HandleValueChanged(int index)
		{
			this._valueChangedVersion++;
			this.RefreshCaptionFont(index);
		}

		// Token: 0x06008250 RID: 33360 RVA: 0x003539A0 File Offset: 0x00351BA0
		internal void NotifyItemActivated(int index, int versionAtOpen)
		{
			if (index != base.value || versionAtOpen != this._valueChangedVersion || !this.IsActive() || !this.IsInteractable())
			{
				return;
			}
			base.onValueChanged.Invoke(index);
		}

		// Token: 0x06008251 RID: 33361 RVA: 0x003539D4 File Offset: 0x00351BD4
		private void ApplyFont(TMP_Text text, int index)
		{
			if (text == null)
			{
				return;
			}
			TMP_FontAsset tmp_FontAsset = ((index >= 0 && index < this._languages.Length) ? this._languages[index].font : null);
			text.font = ((tmp_FontAsset != null) ? tmp_FontAsset : this._sourceFont);
			if (text.font != null)
			{
				text.fontSharedMaterial = text.font.material;
			}
		}

		// Token: 0x06008252 RID: 33362 RVA: 0x00353A44 File Offset: 0x00351C44
		protected override GameObject CreateDropdownList(GameObject template)
		{
			this._nextItem = 0;
			this._popupValueChangedVersion = this._valueChangedVersion;
			GameObject gameObject = base.CreateDropdownList(template);
			Canvas componentInParent = base.GetComponentInParent<Canvas>();
			if (componentInParent != null)
			{
				gameObject.GetComponent<Canvas>().sortingOrder = Mathf.Clamp(Mathf.Max(30000, componentInParent.rootCanvas.sortingOrder + 2), 0, 32767);
			}
			return gameObject;
		}

		// Token: 0x06008253 RID: 33363 RVA: 0x00353AAC File Offset: 0x00351CAC
		protected override TMP_Dropdown.DropdownItem CreateItem(TMP_Dropdown.DropdownItem itemTemplate)
		{
			TMP_Dropdown.DropdownItem dropdownItem = base.CreateItem(itemTemplate);
			int nextItem = this._nextItem;
			this._nextItem = nextItem + 1;
			int num = nextItem;
			this.ApplyFont(dropdownItem.text, num);
			dropdownItem.gameObject.AddComponent<LanguageDropdownSelectionRelay>().Initialize(this, num, this._popupValueChangedVersion);
			Material fontSharedMaterial = dropdownItem.text.fontSharedMaterial;
			if (fontSharedMaterial != null)
			{
				Material material;
				if (!this._popupMaterials.TryGetValue(fontSharedMaterial, out material))
				{
					material = new Material(fontSharedMaterial)
					{
						name = fontSharedMaterial.name + " (Language Popup)",
						hideFlags = HideFlags.HideAndDontSave
					};
					this._popupMaterials.Add(fontSharedMaterial, material);
				}
				dropdownItem.text.fontSharedMaterial = material;
			}
			return dropdownItem;
		}

		// Token: 0x06008254 RID: 33364 RVA: 0x00353B60 File Offset: 0x00351D60
		protected override void OnDestroy()
		{
			base.OnDestroy();
			foreach (Material material in this._popupMaterials.Values)
			{
				if (Application.isPlaying)
				{
					Object.Destroy(material);
				}
				else
				{
					Object.DestroyImmediate(material);
				}
			}
			this._popupMaterials.Clear();
		}

		// Token: 0x06008255 RID: 33365 RVA: 0x00353BD8 File Offset: 0x00351DD8
		public static LanguageDropdown Create(Transform parent, TMP_FontAsset font)
		{
			RectTransform rectTransform = LanguageDropdown.Rect("Language Dropdown", parent, Vector2.zero, Vector2.one);
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = new Color(0.12f, 0.06f, 0.14f, 1f);
			LanguageDropdown languageDropdown = rectTransform.gameObject.AddComponent<LanguageDropdown>();
			languageDropdown.targetGraphic = image;
			ColorBlock colors = languageDropdown.colors;
			colors.highlightedColor = new Color(1f, 0.75f, 0.87f);
			colors.selectedColor = colors.highlightedColor;
			languageDropdown.colors = colors;
			languageDropdown.captionText = LanguageDropdown.Label("Selected Language", rectTransform, font, 22f);
			languageDropdown.captionText.rectTransform.offsetMin = new Vector2(18f, 4f);
			languageDropdown.captionText.rectTransform.offsetMax = new Vector2(-45f, -4f);
			TMP_Text tmp_Text = LanguageDropdown.Label("Open List", rectTransform, font, 22f);
			tmp_Text.text = "v";
			tmp_Text.alignment = TextAlignmentOptions.Center;
			tmp_Text.rectTransform.anchorMin = new Vector2(1f, 0f);
			tmp_Text.rectTransform.offsetMin = new Vector2(-42f, 0f);
			tmp_Text.rectTransform.offsetMax = Vector2.zero;
			RectTransform rectTransform2 = LanguageDropdown.Rect("Template", rectTransform, Vector2.zero, Vector2.right);
			rectTransform2.pivot = new Vector2(0.5f, 1f);
			rectTransform2.sizeDelta = new Vector2(0f, 320f);
			rectTransform2.anchoredPosition = new Vector2(0f, -4f);
			rectTransform2.gameObject.AddComponent<Image>().color = new Color(0.09f, 0.04f, 0.11f, 1f);
			RectTransform rectTransform3 = LanguageDropdown.Rect("Viewport", rectTransform2, Vector2.zero, Vector2.one);
			rectTransform3.offsetMin = new Vector2(4f, 4f);
			rectTransform3.offsetMax = new Vector2(-18f, -4f);
			rectTransform3.gameObject.AddComponent<RectMask2D>();
			RectTransform rectTransform4 = LanguageDropdown.Rect("Content", rectTransform3, Vector2.up, Vector2.one);
			rectTransform4.pivot = new Vector2(0.5f, 1f);
			rectTransform4.sizeDelta = new Vector2(0f, 52f);
			RectTransform rectTransform5 = LanguageDropdown.Rect("Item", rectTransform4, new Vector2(0f, 0.5f), new Vector2(1f, 0.5f));
			rectTransform5.sizeDelta = new Vector2(0f, 48f);
			Image image2 = rectTransform5.gameObject.AddComponent<Image>();
			image2.color = Color.white;
			Toggle toggle = rectTransform5.gameObject.AddComponent<Toggle>();
			toggle.targetGraphic = image2;
			ColorBlock colors2 = toggle.colors;
			colors2.normalColor = new Color(0.16f, 0.09f, 0.18f, 1f);
			colors2.highlightedColor = new Color(0.35f, 0.16f, 0.29f, 1f);
			colors2.selectedColor = colors2.highlightedColor;
			toggle.colors = colors2;
			RectTransform rectTransform6 = LanguageDropdown.Rect("Selected", rectTransform5, Vector2.zero, Vector2.up);
			rectTransform6.sizeDelta = new Vector2(4f, 0f);
			rectTransform6.anchoredPosition = new Vector2(2f, 0f);
			toggle.graphic = rectTransform6.gameObject.AddComponent<Image>();
			toggle.graphic.color = new Color(1f, 0.31f, 0.61f);
			languageDropdown.itemText = LanguageDropdown.Label("Native Name", rectTransform5, font, 22f);
			languageDropdown.itemText.rectTransform.offsetMin = new Vector2(18f, 2f);
			languageDropdown.itemText.rectTransform.offsetMax = new Vector2(-12f, -2f);
			ScrollRect scrollRect = rectTransform2.gameObject.AddComponent<ScrollRect>();
			scrollRect.content = rectTransform4;
			scrollRect.viewport = rectTransform3;
			scrollRect.horizontal = false;
			scrollRect.movementType = ScrollRect.MovementType.Clamped;
			scrollRect.scrollSensitivity = 32f;
			RectTransform rectTransform7 = LanguageDropdown.Rect("Scrollbar", rectTransform2, Vector2.right, Vector2.one);
			rectTransform7.offsetMin = new Vector2(-14f, 4f);
			rectTransform7.offsetMax = new Vector2(-4f, -4f);
			rectTransform7.gameObject.AddComponent<Image>().color = new Color(0.04f, 0.02f, 0.05f);
			RectTransform rectTransform8 = LanguageDropdown.Rect("Handle", rectTransform7, Vector2.zero, Vector2.one);
			Image image3 = rectTransform8.gameObject.AddComponent<Image>();
			image3.color = new Color(1f, 0.31f, 0.61f);
			Scrollbar scrollbar = rectTransform7.gameObject.AddComponent<Scrollbar>();
			scrollbar.handleRect = rectTransform8;
			scrollbar.targetGraphic = image3;
			scrollbar.direction = Scrollbar.Direction.BottomToTop;
			scrollbar.navigation = new Navigation
			{
				mode = Navigation.Mode.None
			};
			scrollRect.verticalScrollbar = scrollbar;
			scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
			languageDropdown.template = rectTransform2;
			rectTransform2.gameObject.SetActive(false);
			return languageDropdown;
		}

		// Token: 0x06008256 RID: 33366 RVA: 0x00354118 File Offset: 0x00352318
		internal static RectTransform Rect(string name, Transform parent, Vector2 min, Vector2 max)
		{
			RectTransform component = new GameObject(name, new Type[] { typeof(RectTransform) }).GetComponent<RectTransform>();
			component.SetParent(parent, false);
			component.anchorMin = min;
			component.anchorMax = max;
			component.offsetMin = (component.offsetMax = Vector2.zero);
			return component;
		}

		// Token: 0x06008257 RID: 33367 RVA: 0x00354170 File Offset: 0x00352370
		internal static TMP_Text Label(string name, Transform parent, TMP_FontAsset font, float size)
		{
			TextMeshProUGUI textMeshProUGUI = LanguageDropdown.Rect(name, parent, Vector2.zero, Vector2.one).gameObject.AddComponent<TextMeshProUGUI>();
			textMeshProUGUI.font = ((font != null) ? font : TMP_Settings.defaultFontAsset);
			textMeshProUGUI.fontSize = size;
			textMeshProUGUI.fontSizeMin = size * 0.7f;
			textMeshProUGUI.fontSizeMax = size;
			textMeshProUGUI.enableAutoSizing = true;
			textMeshProUGUI.enableWordWrapping = false;
			textMeshProUGUI.alignment = TextAlignmentOptions.MidlineLeft;
			textMeshProUGUI.color = new Color(1f, 0.97f, 1f);
			textMeshProUGUI.raycastTarget = false;
			return textMeshProUGUI;
		}

		// Token: 0x0400857D RID: 34173
		private GameLocalizationConfig.Language[] _languages = Array.Empty<GameLocalizationConfig.Language>();

		// Token: 0x0400857E RID: 34174
		private TMP_FontAsset _sourceFont;

		// Token: 0x0400857F RID: 34175
		private int _nextItem;

		// Token: 0x04008580 RID: 34176
		private int _valueChangedVersion;

		// Token: 0x04008581 RID: 34177
		private int _popupValueChangedVersion;

		// Token: 0x04008582 RID: 34178
		private readonly Dictionary<Material, Material> _popupMaterials = new Dictionary<Material, Material>();
	}
}
