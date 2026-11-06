using System;
using Features.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.OxGram
{
	// Token: 0x02000EB4 RID: 3764
	public static class OxGramUI
	{
		// Token: 0x060070E2 RID: 28898 RVA: 0x002DD477 File Offset: 0x002DB677
		public static RectTransform Rect(string name, Transform parent)
		{
			RectTransform rectTransform = (RectTransform)new GameObject(name, new Type[] { typeof(RectTransform) }).transform;
			rectTransform.SetParent(parent, false);
			return rectTransform;
		}

		// Token: 0x060070E3 RID: 28899 RVA: 0x002DD4A4 File Offset: 0x002DB6A4
		public static RectTransform Stretch(RectTransform rt, float left = 0f, float right = 0f, float top = 0f, float bottom = 0f)
		{
			rt.anchorMin = Vector2.zero;
			rt.anchorMax = Vector2.one;
			rt.offsetMin = new Vector2(left, bottom);
			rt.offsetMax = new Vector2(-right, -top);
			return rt;
		}

		// Token: 0x060070E4 RID: 28900 RVA: 0x002DD4DC File Offset: 0x002DB6DC
		public static Image Image(string name, Transform parent, Color color, Sprite sprite = null)
		{
			Image image = OxGramUI.Rect(name, parent).gameObject.AddComponent<Image>();
			image.color = color;
			if (sprite != null)
			{
				image.sprite = sprite;
				image.type = global::UnityEngine.UI.Image.Type.Sliced;
			}
			return image;
		}

		// Token: 0x060070E5 RID: 28901 RVA: 0x002DD51C File Offset: 0x002DB71C
		public static TMP_Text Text(string name, Transform parent, string content, TMP_FontAsset font, float size, Color color, TextAlignmentOptions align = TextAlignmentOptions.TopLeft)
		{
			TextMeshProUGUI textMeshProUGUI = OxGramUI.Rect(name, parent).gameObject.AddComponent<TextMeshProUGUI>();
			textMeshProUGUI.text = content;
			if (font != null)
			{
				textMeshProUGUI.font = font;
			}
			textMeshProUGUI.fontSize = size;
			textMeshProUGUI.color = color;
			textMeshProUGUI.alignment = align;
			textMeshProUGUI.richText = true;
			textMeshProUGUI.overflowMode = TextOverflowModes.Overflow;
			textMeshProUGUI.raycastTarget = false;
			LocalizedGameText.BindFontOnly(textMeshProUGUI);
			return textMeshProUGUI;
		}

		// Token: 0x060070E6 RID: 28902 RVA: 0x002DD587 File Offset: 0x002DB787
		public static TMP_Text LocalizedText(string name, Transform parent, string content, TMP_FontAsset font, float size, Color color, TextAlignmentOptions align = TextAlignmentOptions.TopLeft)
		{
			TMP_Text tmp_Text = OxGramUI.Text(name, parent, content, font, size, color, align);
			LocalizedGameText.BindSource(tmp_Text, "phone_story", content, null).FitTranslationToRectangle(false);
			return tmp_Text;
		}

		// Token: 0x060070E7 RID: 28903 RVA: 0x002DD5AC File Offset: 0x002DB7AC
		public static Button Button(GameObject target)
		{
			Button button = target.GetComponent<Button>();
			if (button == null)
			{
				button = target.AddComponent<Button>();
			}
			button.targetGraphic = target.GetComponent<Graphic>();
			if (button.targetGraphic != null)
			{
				button.targetGraphic.raycastTarget = true;
			}
			button.transition = Selectable.Transition.ColorTint;
			ColorBlock colors = button.colors;
			colors.highlightedColor = new Color(0.92f, 0.92f, 1f);
			colors.pressedColor = new Color(0.72f, 0.72f, 0.85f);
			colors.selectedColor = Color.white;
			colors.disabledColor = new Color(0.55f, 0.55f, 0.65f, 0.55f);
			colors.fadeDuration = 0.12f;
			button.colors = colors;
			return button;
		}

		// Token: 0x060070E8 RID: 28904 RVA: 0x002DD67A File Offset: 0x002DB87A
		public static T Add<T>(Component c) where T : Component
		{
			return c.gameObject.AddComponent<T>();
		}

		// Token: 0x060070E9 RID: 28905 RVA: 0x002DD688 File Offset: 0x002DB888
		public static GameObject BackButton(Transform header, TMP_FontAsset font, Color tint, float yOffset, Action onTap, string label = "Back", Sprite nativeSprite = null)
		{
			if (nativeSprite != null)
			{
				RectTransform rectTransform = OxGramUI.Rect("BackButton", header);
				rectTransform.anchorMin = new Vector2(0f, 0.5f);
				rectTransform.anchorMax = new Vector2(0f, 0.5f);
				rectTransform.pivot = new Vector2(0f, 0.5f);
				rectTransform.sizeDelta = new Vector2(82f, 39f);
				rectTransform.anchoredPosition = new Vector2(10f, yOffset);
				Image img = rectTransform.gameObject.AddComponent<Image>();
				img.sprite = nativeSprite;
				img.color = Color.white;
				img.preserveAspect = true;
				Button btn = rectTransform.gameObject.AddComponent<Button>();
				btn.transition = Selectable.Transition.ColorTint;
				btn.targetGraphic = img;
				ColorBlock colors = btn.colors;
				colors.normalColor = Color.white;
				colors.highlightedColor = new Color(0.9607843f, 0.9607843f, 0.9607843f, 1f);
				colors.pressedColor = new Color(0.78431374f, 0.78431374f, 0.78431374f, 1f);
				colors.selectedColor = Color.white;
				colors.fadeDuration = 0.1f;
				btn.colors = colors;
				btn.onClick.AddListener(delegate
				{
					Action onTap2 = onTap;
					if (onTap2 == null)
					{
						return;
					}
					onTap2();
				});
				TMP_Text nativeLabel = OxGramUI.Text("LocalizedBack", rectTransform, "", font, 21f, tint, TextAlignmentOptions.Center);
				OxGramUI.Stretch(nativeLabel.rectTransform, 0f, 0f, 0f, 0f);
				LocalizedGameText.BindComputed(nativeLabel, delegate
				{
					bool flag = GameLocalization.CurrentCode == "en";
					img.sprite = (flag ? nativeSprite : null);
					img.color = (flag ? Color.white : Color.clear);
					btn.targetGraphic = (flag ? img : nativeLabel);
					if (!flag)
					{
						return "‹ " + GameLocalization.GetSource("phone_story", label);
					}
					return "";
				}).FitTranslationToRectangle(false);
				return rectTransform.gameObject;
			}
			RectTransform rectTransform2 = OxGramUI.Rect("BackButton", header);
			rectTransform2.anchorMin = new Vector2(0f, 0.5f);
			rectTransform2.anchorMax = new Vector2(0f, 0.5f);
			rectTransform2.pivot = new Vector2(0f, 0.5f);
			rectTransform2.sizeDelta = new Vector2(120f, 44f);
			rectTransform2.anchoredPosition = new Vector2(4f, yOffset);
			TMP_Text tmp_Text = OxGramUI.Text("Chevron", rectTransform2, "‹", font, 38f, tint, TextAlignmentOptions.Left);
			tmp_Text.rectTransform.anchorMin = new Vector2(0f, 0.5f);
			tmp_Text.rectTransform.anchorMax = new Vector2(0f, 0.5f);
			tmp_Text.rectTransform.pivot = new Vector2(0f, 0.5f);
			tmp_Text.rectTransform.sizeDelta = new Vector2(18f, 44f);
			tmp_Text.rectTransform.anchoredPosition = new Vector2(2f, 2f);
			tmp_Text.raycastTarget = false;
			TMP_Text tmp_Text2 = OxGramUI.LocalizedText("Label", rectTransform2, label, font, 21f, tint, TextAlignmentOptions.Left);
			tmp_Text2.rectTransform.anchorMin = new Vector2(0f, 0.5f);
			tmp_Text2.rectTransform.anchorMax = new Vector2(0f, 0.5f);
			tmp_Text2.rectTransform.pivot = new Vector2(0f, 0.5f);
			tmp_Text2.rectTransform.sizeDelta = new Vector2(90f, 44f);
			tmp_Text2.rectTransform.anchoredPosition = new Vector2(20f, 0f);
			tmp_Text2.raycastTarget = false;
			rectTransform2.gameObject.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
			OxGramUI.Button(rectTransform2.gameObject).onClick.AddListener(delegate
			{
				Action onTap3 = onTap;
				if (onTap3 == null)
				{
					return;
				}
				onTap3();
			});
			return rectTransform2.gameObject;
		}
	}
}
