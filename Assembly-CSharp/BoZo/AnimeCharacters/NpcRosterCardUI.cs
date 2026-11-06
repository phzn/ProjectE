using System;
using System.Collections.Generic;
using Features.Localization;
using ProjectExhibitionist.CharacterCustomization.Core.Presets;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Bozo.AnimeCharacters
{
	// Token: 0x02000898 RID: 2200
	internal sealed class NpcRosterCardUI
	{
		// Token: 0x06003B35 RID: 15157 RVA: 0x0015FD18 File Offset: 0x0015DF18
		public NpcRosterCardUI(Transform card, Image portrait, TextMeshProUGUI name, Sprite maleIcon, Sprite femaleIcon, Action toggle)
		{
			this.portrait = portrait;
			this.name = name;
			this.maleIcon = maleIcon;
			this.femaleIcon = femaleIcon;
			this.originalPortraitColor = ((portrait != null) ? portrait.color : Color.white);
			this.originalNameColor = ((name != null) ? name.color : Color.white);
			this.originalPreserveAspect = portrait != null && portrait.preserveAspect;
			this.originalNameFontSize = ((name != null) ? name.fontSize : 24f);
			this.originalNameWrapping = name != null && name.enableWordWrapping;
			this.originalNameOverflow = ((name != null) ? name.overflowMode : TextOverflowModes.Overflow);
			Transform transform = card.Find("InUse/InUseBg");
			Image image = ((transform != null) ? transform.GetComponent<Image>() : null);
			this.accent = ((image != null) ? image.color : new Color(0.8941177f, 0.34901962f, 0.4666667f, 1f));
			this.accent.a = 1f;
			this.Remember(card.Find("Normal"));
			this.Remember(card.Find("Normal/Bg"));
			this.Remember(card.Find("Normal/Frame"));
			this.Remember(card.Find("Selected"));
			this.Remember(card.Find("Selected/SelectedBorder"));
			this.Remember(card.Find("Content"));
			if (portrait != null)
			{
				this.Remember(portrait.transform);
			}
			if (name != null)
			{
				this.Remember(name.transform);
			}
			if (portrait != null)
			{
				this.originalPortraitParent = portrait.transform.parent;
				this.originalPortraitSibling = portrait.transform.GetSiblingIndex();
				GameObject gameObject = new GameObject("NpcPortraitViewport", new Type[]
				{
					typeof(RectTransform),
					typeof(RectMask2D)
				});
				gameObject.layer = portrait.gameObject.layer;
				this.portraitViewport = (RectTransform)gameObject.transform;
				this.portraitViewport.SetParent(this.originalPortraitParent, false);
				this.portraitViewport.SetSiblingIndex(this.originalPortraitSibling);
				NpcRosterCardUI.Stretch(this.portraitViewport, new Vector2(16f, 45f), new Vector2(-16f, -42f));
				gameObject.SetActive(false);
				this.portraitFitter = portrait.GetComponent<AspectRatioFitter>();
				if (this.portraitFitter != null)
				{
					this.originalFitterEnabled = this.portraitFitter.enabled;
					this.originalFitterMode = this.portraitFitter.aspectMode;
					this.originalFitterRatio = this.portraitFitter.aspectRatio;
				}
				else
				{
					this.portraitFitter = portrait.gameObject.AddComponent<AspectRatioFitter>();
					this.portraitFitter.enabled = false;
				}
			}
			this.controls = new GameObject("NpcRosterControls", new Type[] { typeof(RectTransform) });
			this.controls.layer = card.gameObject.layer;
			this.controls.transform.SetParent(card, false);
			NpcRosterCardUI.Stretch((RectTransform)this.controls.transform, default(Vector2), default(Vector2));
			this.metadata = this.MakeText("Origin", this.controls.transform, 14f, NpcRosterCardUI.MutedText);
			this.metadata.rectTransform.anchorMin = (this.metadata.rectTransform.anchorMax = Vector2.up);
			this.metadata.rectTransform.pivot = Vector2.up;
			this.metadata.rectTransform.anchoredPosition = new Vector2(14f, -11f);
			this.metadata.rectTransform.sizeDelta = new Vector2(74f, 24f);
			this.metadata.alignment = TextAlignmentOptions.MidlineLeft;
			this.metadata.characterSpacing = 0.6f;
			this.genderIcon = NpcRosterCardUI.MakeImage("Gender", this.controls.transform, NpcRosterCardUI.MutedText);
			this.genderIcon.rectTransform.anchorMin = (this.genderIcon.rectTransform.anchorMax = Vector2.up);
			this.genderIcon.rectTransform.pivot = Vector2.up;
			this.genderIcon.rectTransform.anchoredPosition = new Vector2(94f, -14f);
			this.genderIcon.rectTransform.sizeDelta = new Vector2(19f, 19f);
			this.genderIcon.preserveAspect = true;
			GameObject gameObject2 = new GameObject("NpcEnabledToggle", new Type[]
			{
				typeof(RectTransform),
				typeof(Image),
				typeof(NpcRosterToggleButton)
			});
			gameObject2.layer = this.controls.layer;
			gameObject2.transform.SetParent(this.controls.transform, false);
			this.toggleRect = (RectTransform)gameObject2.transform;
			this.toggleRect.anchorMin = (this.toggleRect.anchorMax = Vector2.one);
			this.toggleRect.pivot = Vector2.one;
			this.toggleRect.anchoredPosition = new Vector2(-7f, -5f);
			this.toggleRect.sizeDelta = new Vector2(36f, 36f);
			gameObject2.GetComponent<Image>().color = Color.clear;
			this.toggleBackground = NpcRosterCardUI.MakeImage("Border", this.toggleRect, NpcRosterCardUI.MutedText);
			NpcRosterCardUI.Centre(this.toggleBackground.rectTransform, new Vector2(24f, 24f));
			NpcRosterCardUI.Stretch(NpcRosterCardUI.MakeImage("Inset", this.toggleBackground.transform, NpcRosterCardUI.Panel).rectTransform, new Vector2(2f, 2f), new Vector2(-2f, -2f));
			this.toggleButton = gameObject2.GetComponent<NpcRosterToggleButton>();
			this.toggleButton.targetGraphic = this.toggleBackground;
			ColorBlock colors = this.toggleButton.colors;
			colors.normalColor = Color.white;
			colors.highlightedColor = new Color(1.15f, 1.15f, 1.15f, 1f);
			colors.selectedColor = colors.highlightedColor;
			colors.pressedColor = new Color(0.85f, 0.85f, 0.85f, 1f);
			colors.fadeDuration = 0.1f;
			this.toggleButton.colors = colors;
			this.toggleButton.onClick.AddListener(delegate
			{
				Action toggle2 = toggle;
				if (toggle2 == null)
				{
					return;
				}
				toggle2();
			});
			this.indicator = new GameObject("Checkmark", new Type[] { typeof(RectTransform) });
			this.indicator.layer = this.controls.layer;
			this.indicator.transform.SetParent(this.toggleRect, false);
			NpcRosterCardUI.Centre((RectTransform)this.indicator.transform, new Vector2(24f, 24f));
			this.MakeCheckStroke(this.indicator.transform, new Vector2(-4f, -1f), 8f, -45f);
			this.MakeCheckStroke(this.indicator.transform, new Vector2(3f, 1f), 14f, 45f);
			Image image2 = NpcRosterCardUI.MakeImage("Toggle Hint", this.controls.transform, NpcRosterCardUI.Panel);
			NpcRosterCardUI.PlaceFromTop(image2.rectTransform, 8f, 42f, 48f);
			TextMeshProUGUI textMeshProUGUI = this.MakeText("Label", image2.transform, 15f, NpcRosterCardUI.Text);
			NpcRosterCardUI.Stretch(textMeshProUGUI.rectTransform, new Vector2(7f, 3f), new Vector2(-7f, -3f));
			textMeshProUGUI.enableWordWrapping = true;
			textMeshProUGUI.alignment = TextAlignmentOptions.Center;
			this.toggleButton.ConfigureHint(image2.gameObject, textMeshProUGUI);
			this.controls.SetActive(false);
		}

		// Token: 0x06003B36 RID: 15158 RVA: 0x001605A4 File Offset: 0x0015E7A4
		public void Show(CustomizationPresetData preset, bool enabled, bool eligible)
		{
			if (!this.visible)
			{
				foreach (NpcRosterCardUI.RectState rectState in this.originalRects)
				{
					NpcRosterCardUI.Stretch(rectState.Rect, default(Vector2), default(Vector2));
				}
				if (this.portrait != null)
				{
					this.portrait.transform.SetParent(this.portraitViewport, false);
					this.portrait.rectTransform.pivot = new Vector2(0.5f, 0.5f);
					NpcRosterCardUI.Stretch(this.portrait.rectTransform, default(Vector2), default(Vector2));
					this.portraitViewport.gameObject.SetActive(true);
					this.portrait.preserveAspect = true;
					this.portraitFitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
					this.portraitFitter.enabled = true;
				}
				if (this.name != null)
				{
					NpcRosterCardUI.PlaceFromBottom(this.name.rectTransform, 12f, 10f, 29f);
					this.name.fontSize = 22f;
					this.name.enableWordWrapping = false;
					this.name.overflowMode = TextOverflowModes.Ellipsis;
				}
			}
			this.visible = true;
			this.RefreshPortraitAspect();
			this.controls.SetActive(true);
			LocalizedGameText localizedGameText = LocalizedGameText.BindSource(this.metadata, "cielos_apps", preset.isSystemPreset ? "SYSTEM" : "CUSTOM", null);
			if (localizedGameText != null)
			{
				localizedGameText.FitTranslationToRectangle(false);
			}
			this.genderIcon.sprite = ((preset.gender == Gender.Female) ? this.femaleIcon : this.maleIcon);
			this.genderIcon.gameObject.SetActive(this.genderIcon.sprite != null);
			this.Refresh(enabled, eligible);
		}

		// Token: 0x06003B37 RID: 15159 RVA: 0x001607A0 File Offset: 0x0015E9A0
		public void Refresh(bool enabled, bool eligible)
		{
			if (!this.visible)
			{
				return;
			}
			bool flag = enabled && !eligible;
			this.toggleBackground.color = (enabled ? this.accent : NpcRosterCardUI.MutedText);
			this.indicator.SetActive(enabled);
			this.toggleButton.SetHint(enabled ? (flag ? "Disable in game\nOutside current population" : "Disable in game") : "Enable in game");
			if (this.portrait != null)
			{
				Color color = this.originalPortraitColor;
				color.a *= (enabled ? 1f : 0.48f);
				this.portrait.color = color;
			}
			if (this.name != null)
			{
				Color color2 = this.originalNameColor;
				color2.a *= (enabled ? 1f : 0.72f);
				this.name.color = color2;
			}
		}

		// Token: 0x06003B38 RID: 15160 RVA: 0x00160884 File Offset: 0x0015EA84
		public void RefreshPortraitAspect()
		{
			if (!this.visible || this.portraitFitter == null || this.portrait.sprite == null)
			{
				return;
			}
			Rect rect = this.portrait.sprite.rect;
			if (rect.height > 0f)
			{
				this.portraitFitter.aspectRatio = rect.width / rect.height;
			}
		}

		// Token: 0x06003B39 RID: 15161 RVA: 0x001608F4 File Offset: 0x0015EAF4
		public void Hide()
		{
			if (!this.visible)
			{
				return;
			}
			this.visible = false;
			this.controls.SetActive(false);
			if (this.portrait != null)
			{
				this.portraitFitter.enabled = false;
				this.portrait.transform.SetParent(this.originalPortraitParent, false);
				this.portrait.transform.SetSiblingIndex(this.originalPortraitSibling);
				this.portraitViewport.gameObject.SetActive(false);
				this.portrait.color = this.originalPortraitColor;
				this.portrait.preserveAspect = this.originalPreserveAspect;
			}
			foreach (NpcRosterCardUI.RectState rectState in this.originalRects)
			{
				rectState.Restore();
			}
			if (this.portraitFitter != null)
			{
				this.portraitFitter.aspectMode = this.originalFitterMode;
				this.portraitFitter.aspectRatio = this.originalFitterRatio;
				this.portraitFitter.enabled = this.originalFitterEnabled;
			}
			if (this.name != null)
			{
				this.name.fontSize = this.originalNameFontSize;
				this.name.color = this.originalNameColor;
				this.name.enableWordWrapping = this.originalNameWrapping;
				this.name.overflowMode = this.originalNameOverflow;
			}
		}

		// Token: 0x06003B3A RID: 15162 RVA: 0x00160A70 File Offset: 0x0015EC70
		public bool OwnsPointer(PointerEventData eventData)
		{
			if (!this.visible || eventData == null)
			{
				return false;
			}
			GameObject gameObject = eventData.pointerPressRaycast.gameObject;
			if (gameObject == null)
			{
				gameObject = eventData.pointerCurrentRaycast.gameObject;
			}
			return gameObject != null && gameObject.transform.IsChildOf(this.toggleRect);
		}

		// Token: 0x06003B3B RID: 15163 RVA: 0x00160AD0 File Offset: 0x0015ECD0
		private TextMeshProUGUI MakeText(string objectName, Transform parent, float size, Color color)
		{
			GameObject gameObject = new GameObject(objectName, new Type[]
			{
				typeof(RectTransform),
				typeof(TextMeshProUGUI)
			});
			gameObject.layer = parent.gameObject.layer;
			gameObject.transform.SetParent(parent, false);
			TextMeshProUGUI component = gameObject.GetComponent<TextMeshProUGUI>();
			if (this.name != null)
			{
				component.font = this.name.font;
				component.fontSharedMaterial = this.name.fontSharedMaterial;
			}
			component.fontSize = size;
			component.color = color;
			component.alignment = TextAlignmentOptions.Midline;
			component.enableWordWrapping = false;
			component.overflowMode = TextOverflowModes.Ellipsis;
			component.raycastTarget = false;
			return component;
		}

		// Token: 0x06003B3C RID: 15164 RVA: 0x00160B88 File Offset: 0x0015ED88
		private static Image MakeImage(string objectName, Transform parent, Color color)
		{
			GameObject gameObject = new GameObject(objectName, new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			gameObject.layer = parent.gameObject.layer;
			gameObject.transform.SetParent(parent, false);
			Image component = gameObject.GetComponent<Image>();
			component.color = color;
			component.raycastTarget = false;
			return component;
		}

		// Token: 0x06003B3D RID: 15165 RVA: 0x00160BEC File Offset: 0x0015EDEC
		private void MakeCheckStroke(Transform parent, Vector2 position, float length, float angle)
		{
			Image image = NpcRosterCardUI.MakeImage("Stroke", parent, this.accent);
			NpcRosterCardUI.Centre(image.rectTransform, new Vector2(length, 3f));
			image.rectTransform.anchoredPosition = position;
			image.rectTransform.localRotation = Quaternion.Euler(0f, 0f, angle);
		}

		// Token: 0x06003B3E RID: 15166 RVA: 0x00160C48 File Offset: 0x0015EE48
		private static void Centre(RectTransform rect, Vector2 size)
		{
			Vector2 vector = new Vector2(0.5f, 0.5f);
			rect.pivot = vector;
			rect.anchorMin = (rect.anchorMax = vector);
			rect.anchoredPosition = Vector2.zero;
			rect.sizeDelta = size;
		}

		// Token: 0x06003B3F RID: 15167 RVA: 0x00160C90 File Offset: 0x0015EE90
		private void Remember(Transform target)
		{
			RectTransform rectTransform = target as RectTransform;
			if (rectTransform != null)
			{
				this.originalRects.Add(new NpcRosterCardUI.RectState(rectTransform));
			}
		}

		// Token: 0x06003B40 RID: 15168 RVA: 0x0013F62D File Offset: 0x0013D82D
		private static void Stretch(RectTransform rect, Vector2 min = default(Vector2), Vector2 max = default(Vector2))
		{
			rect.anchorMin = Vector2.zero;
			rect.anchorMax = Vector2.one;
			rect.offsetMin = min;
			rect.offsetMax = max;
		}

		// Token: 0x06003B41 RID: 15169 RVA: 0x00160CB8 File Offset: 0x0015EEB8
		private static void PlaceFromTop(RectTransform rect, float inset, float top, float height)
		{
			rect.anchorMin = new Vector2(0f, 1f);
			rect.anchorMax = Vector2.one;
			rect.offsetMin = new Vector2(inset, -top - height);
			rect.offsetMax = new Vector2(-inset, -top);
		}

		// Token: 0x06003B42 RID: 15170 RVA: 0x00160D04 File Offset: 0x0015EF04
		private static void PlaceFromBottom(RectTransform rect, float inset, float bottom, float height)
		{
			rect.anchorMin = Vector2.zero;
			rect.anchorMax = new Vector2(1f, 0f);
			rect.offsetMin = new Vector2(inset, bottom);
			rect.offsetMax = new Vector2(-inset, bottom + height);
		}

		// Token: 0x04003B61 RID: 15201
		private readonly List<NpcRosterCardUI.RectState> originalRects = new List<NpcRosterCardUI.RectState>();

		// Token: 0x04003B62 RID: 15202
		private readonly Image portrait;

		// Token: 0x04003B63 RID: 15203
		private readonly RectTransform portraitViewport;

		// Token: 0x04003B64 RID: 15204
		private readonly Transform originalPortraitParent;

		// Token: 0x04003B65 RID: 15205
		private readonly int originalPortraitSibling;

		// Token: 0x04003B66 RID: 15206
		private readonly AspectRatioFitter portraitFitter;

		// Token: 0x04003B67 RID: 15207
		private readonly bool originalFitterEnabled;

		// Token: 0x04003B68 RID: 15208
		private readonly AspectRatioFitter.AspectMode originalFitterMode;

		// Token: 0x04003B69 RID: 15209
		private readonly float originalFitterRatio = 1f;

		// Token: 0x04003B6A RID: 15210
		private readonly TextMeshProUGUI name;

		// Token: 0x04003B6B RID: 15211
		private readonly GameObject controls;

		// Token: 0x04003B6C RID: 15212
		private readonly RectTransform toggleRect;

		// Token: 0x04003B6D RID: 15213
		private readonly Image toggleBackground;

		// Token: 0x04003B6E RID: 15214
		private readonly GameObject indicator;

		// Token: 0x04003B6F RID: 15215
		private readonly TextMeshProUGUI metadata;

		// Token: 0x04003B70 RID: 15216
		private readonly Image genderIcon;

		// Token: 0x04003B71 RID: 15217
		private readonly Sprite maleIcon;

		// Token: 0x04003B72 RID: 15218
		private readonly Sprite femaleIcon;

		// Token: 0x04003B73 RID: 15219
		private readonly NpcRosterToggleButton toggleButton;

		// Token: 0x04003B74 RID: 15220
		private readonly Color accent;

		// Token: 0x04003B75 RID: 15221
		private readonly Color originalPortraitColor;

		// Token: 0x04003B76 RID: 15222
		private readonly Color originalNameColor;

		// Token: 0x04003B77 RID: 15223
		private readonly bool originalPreserveAspect;

		// Token: 0x04003B78 RID: 15224
		private readonly float originalNameFontSize;

		// Token: 0x04003B79 RID: 15225
		private readonly bool originalNameWrapping;

		// Token: 0x04003B7A RID: 15226
		private readonly TextOverflowModes originalNameOverflow;

		// Token: 0x04003B7B RID: 15227
		private bool visible;

		// Token: 0x04003B7C RID: 15228
		private static readonly Color Panel = new Color(0.18f, 0.105f, 0.235f, 1f);

		// Token: 0x04003B7D RID: 15229
		private static readonly Color Text = new Color(0.97f, 0.94f, 0.96f, 1f);

		// Token: 0x04003B7E RID: 15230
		private static readonly Color MutedText = new Color(0.79f, 0.69f, 0.8f, 1f);

		// Token: 0x02000899 RID: 2201
		private readonly struct RectState
		{
			// Token: 0x06003B44 RID: 15172 RVA: 0x00160DAC File Offset: 0x0015EFAC
			public RectState(RectTransform rect)
			{
				this.Rect = rect;
				this.anchorMin = rect.anchorMin;
				this.anchorMax = rect.anchorMax;
				this.position = rect.anchoredPosition;
				this.size = rect.sizeDelta;
				this.pivot = rect.pivot;
			}

			// Token: 0x06003B45 RID: 15173 RVA: 0x00160DFC File Offset: 0x0015EFFC
			public void Restore()
			{
				this.Rect.anchorMin = this.anchorMin;
				this.Rect.anchorMax = this.anchorMax;
				this.Rect.pivot = this.pivot;
				this.Rect.anchoredPosition = this.position;
				this.Rect.sizeDelta = this.size;
			}

			// Token: 0x04003B7F RID: 15231
			public readonly RectTransform Rect;

			// Token: 0x04003B80 RID: 15232
			private readonly Vector2 anchorMin;

			// Token: 0x04003B81 RID: 15233
			private readonly Vector2 anchorMax;

			// Token: 0x04003B82 RID: 15234
			private readonly Vector2 position;

			// Token: 0x04003B83 RID: 15235
			private readonly Vector2 size;

			// Token: 0x04003B84 RID: 15236
			private readonly Vector2 pivot;
		}
	}
}
