using System;
using Features.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.CielOS.UI
{
	// Token: 0x020013B3 RID: 5043
	public class CielOSBarMeter : MonoBehaviour
	{
		// Token: 0x06009C0D RID: 39949 RVA: 0x003FBE40 File Offset: 0x003FA040
		public void Build(string label)
		{
			CielOSTheme active = CielOSTheme.Active;
			Color color = ((active != null) ? active.controlText : Color.white);
			Color color2 = ((active != null) ? active.barTrack : new Color(0.2f, 0.15f, 0.26f, 1f));
			Color color3 = ((active != null) ? active.barFill : new Color(0.66f, 0.38f, 0.79f, 1f));
			LayoutElement layoutElement = base.GetComponent<LayoutElement>();
			if (layoutElement == null)
			{
				layoutElement = base.gameObject.AddComponent<LayoutElement>();
			}
			layoutElement.minHeight = (layoutElement.preferredHeight = 20f);
			this._label = this.MakeText("Label", label, 12f, color, TextAlignmentOptions.MidlineLeft);
			LocalizedGameText.BindSource(this._label, "cielos_shell", label, null).FitTranslationToRectangle(false);
			RectTransform rectTransform = (RectTransform)this._label.transform;
			rectTransform.anchorMin = new Vector2(0f, 0f);
			rectTransform.anchorMax = new Vector2(0f, 1f);
			rectTransform.pivot = new Vector2(0f, 0.5f);
			rectTransform.sizeDelta = new Vector2(54f, 0f);
			rectTransform.anchoredPosition = Vector2.zero;
			GameObject gameObject = new GameObject("Track", new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			gameObject.transform.SetParent(base.transform, false);
			RectTransform rectTransform2 = (RectTransform)gameObject.transform;
			rectTransform2.anchorMin = new Vector2(0f, 0.5f);
			rectTransform2.anchorMax = new Vector2(1f, 0.5f);
			rectTransform2.pivot = new Vector2(0f, 0.5f);
			rectTransform2.offsetMin = new Vector2(58f, -5f);
			rectTransform2.offsetMax = new Vector2(-84f, 5f);
			Image component = gameObject.GetComponent<Image>();
			component.color = color2;
			CielOSTheme.Round(component);
			GameObject gameObject2 = new GameObject("Fill", new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			gameObject2.transform.SetParent(gameObject.transform, false);
			this._fill = (RectTransform)gameObject2.transform;
			this._fill.anchorMin = new Vector2(0f, 0f);
			this._fill.anchorMax = new Vector2(0f, 1f);
			this._fill.pivot = new Vector2(0f, 0.5f);
			this._fill.offsetMin = Vector2.zero;
			this._fill.offsetMax = Vector2.zero;
			this._fill.sizeDelta = new Vector2(0f, 0f);
			Image component2 = gameObject2.GetComponent<Image>();
			component2.color = color3;
			component2.raycastTarget = false;
			CielOSTheme.Round(component2);
			this._value = this.MakeText("Value", "", 11f, color, TextAlignmentOptions.MidlineRight);
			RectTransform rectTransform3 = (RectTransform)this._value.transform;
			rectTransform3.anchorMin = new Vector2(1f, 0f);
			rectTransform3.anchorMax = new Vector2(1f, 1f);
			rectTransform3.pivot = new Vector2(1f, 0.5f);
			rectTransform3.sizeDelta = new Vector2(80f, 0f);
			rectTransform3.anchoredPosition = Vector2.zero;
		}

		// Token: 0x06009C0E RID: 39950 RVA: 0x003FC1E3 File Offset: 0x003FA3E3
		public void SetValue01(float v)
		{
			if (this._fill == null)
			{
				return;
			}
			v = Mathf.Clamp01(v);
			this._fill.anchorMax = new Vector2(v, 1f);
			this._fill.offsetMax = Vector2.zero;
		}

		// Token: 0x06009C0F RID: 39951 RVA: 0x003FC222 File Offset: 0x003FA422
		public void SetValueText(string text)
		{
			if (this._value != null)
			{
				this._value.text = text;
			}
		}

		// Token: 0x06009C10 RID: 39952 RVA: 0x003FC240 File Offset: 0x003FA440
		private TMP_Text MakeText(string name, string content, float size, Color color, TextAlignmentOptions align)
		{
			GameObject gameObject = new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(TextMeshProUGUI)
			});
			gameObject.transform.SetParent(base.transform, false);
			TextMeshProUGUI component = gameObject.GetComponent<TextMeshProUGUI>();
			component.text = content;
			component.fontSize = size;
			component.color = color;
			component.alignment = align;
			component.raycastTarget = false;
			component.enableWordWrapping = false;
			component.overflowMode = TextOverflowModes.Ellipsis;
			component.enableAutoSizing = true;
			component.fontSizeMin = 9f;
			component.fontSizeMax = size;
			if (CielOSTheme.Active != null)
			{
				if (CielOSTheme.Active.monospaceFont != null)
				{
					component.font = CielOSTheme.Active.monospaceFont;
				}
				else if (CielOSTheme.Active.uiFont != null)
				{
					component.font = CielOSTheme.Active.uiFont;
				}
			}
			return component;
		}

		// Token: 0x0400A098 RID: 41112
		private TMP_Text _label;

		// Token: 0x0400A099 RID: 41113
		private TMP_Text _value;

		// Token: 0x0400A09A RID: 41114
		private RectTransform _fill;
	}
}
