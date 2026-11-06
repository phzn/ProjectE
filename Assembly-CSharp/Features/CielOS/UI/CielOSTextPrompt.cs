using System;
using Features.Localization;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.CielOS.UI
{
	// Token: 0x020013CD RID: 5069
	public class CielOSTextPrompt : MonoBehaviour
	{
		// Token: 0x170018D4 RID: 6356
		// (get) Token: 0x06009C9A RID: 40090 RVA: 0x00402F28 File Offset: 0x00401128
		// (set) Token: 0x06009C9B RID: 40091 RVA: 0x00402F2F File Offset: 0x0040112F
		public static CielOSTextPrompt Instance { get; private set; }

		// Token: 0x06009C9C RID: 40092 RVA: 0x00402F38 File Offset: 0x00401138
		public static CielOSTextPrompt Ensure(Transform canvas)
		{
			if (CielOSTextPrompt.Instance != null)
			{
				return CielOSTextPrompt.Instance;
			}
			GameObject gameObject = new GameObject("CielOSTextPrompt", new Type[] { typeof(RectTransform) });
			gameObject.transform.SetParent(canvas, false);
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = Vector2.zero;
			CielOSTextPrompt.Instance = gameObject.AddComponent<CielOSTextPrompt>();
			CielOSTextPrompt.Instance.Build();
			return CielOSTextPrompt.Instance;
		}

		// Token: 0x06009C9D RID: 40093 RVA: 0x00402FD4 File Offset: 0x004011D4
		private void Build()
		{
			CielOSTheme active = CielOSTheme.Active;
			this._group = base.gameObject.AddComponent<CanvasGroup>();
			GameObject gameObject = new GameObject("Dim", new Type[]
			{
				typeof(RectTransform),
				typeof(Image),
				typeof(Button)
			});
			gameObject.transform.SetParent(base.transform, false);
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = Vector2.zero;
			gameObject.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.5f);
			gameObject.GetComponent<Button>().onClick.AddListener(new UnityAction(this.Cancel));
			GameObject gameObject2 = new GameObject("Card", new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			gameObject2.transform.SetParent(base.transform, false);
			RectTransform rectTransform2 = (RectTransform)gameObject2.transform;
			RectTransform rectTransform3 = rectTransform2;
			RectTransform rectTransform4 = rectTransform2;
			RectTransform rectTransform5 = rectTransform2;
			Vector2 vector = new Vector2(0.5f, 0.5f);
			rectTransform5.pivot = vector;
			rectTransform3.anchorMin = (rectTransform4.anchorMax = vector);
			rectTransform2.sizeDelta = new Vector2(360f, 150f);
			Image component = gameObject2.GetComponent<Image>();
			component.color = ((active != null) ? active.windowBackground : new Color(0.12f, 0.1f, 0.15f, 1f));
			CielOSTheme.Round(component);
			CielOSTheme.AddBorder(rectTransform2, null, 1f);
			this._title = this.MakeText(rectTransform2, "Title", "", 15f, (active != null) ? active.titleText : Color.white, FontStyles.Bold);
			RectTransform rectTransform6 = (RectTransform)this._title.transform;
			rectTransform6.anchorMin = new Vector2(0f, 1f);
			rectTransform6.anchorMax = new Vector2(1f, 1f);
			rectTransform6.pivot = new Vector2(0.5f, 1f);
			rectTransform6.sizeDelta = new Vector2(0f, 26f);
			rectTransform6.offsetMin = new Vector2(16f, rectTransform6.offsetMin.y);
			rectTransform6.offsetMax = new Vector2(-16f, -14f);
			this._title.alignment = TextAlignmentOptions.MidlineLeft;
			GameObject gameObject3 = new GameObject("Input", new Type[]
			{
				typeof(RectTransform),
				typeof(Image),
				typeof(TMP_InputField)
			});
			gameObject3.transform.SetParent(rectTransform2, false);
			RectTransform rectTransform7 = (RectTransform)gameObject3.transform;
			rectTransform7.anchorMin = new Vector2(0f, 0.5f);
			rectTransform7.anchorMax = new Vector2(1f, 0.5f);
			rectTransform7.pivot = new Vector2(0.5f, 0.5f);
			rectTransform7.sizeDelta = new Vector2(-32f, 34f);
			rectTransform7.anchoredPosition = new Vector2(0f, 2f);
			Image component2 = gameObject3.GetComponent<Image>();
			component2.color = ((active != null) ? active.controlBackground : new Color(0.18f, 0.14f, 0.23f, 1f));
			CielOSTheme.Round(component2);
			GameObject gameObject4 = new GameObject("Text", new Type[]
			{
				typeof(RectTransform),
				typeof(TextMeshProUGUI)
			});
			gameObject4.transform.SetParent(gameObject3.transform, false);
			RectTransform rectTransform8 = (RectTransform)gameObject4.transform;
			rectTransform8.anchorMin = Vector2.zero;
			rectTransform8.anchorMax = Vector2.one;
			rectTransform8.offsetMin = new Vector2(10f, 2f);
			rectTransform8.offsetMax = new Vector2(-10f, -2f);
			TextMeshProUGUI component3 = gameObject4.GetComponent<TextMeshProUGUI>();
			component3.fontSize = 14f;
			component3.color = ((active != null) ? active.controlText : Color.white);
			component3.alignment = TextAlignmentOptions.MidlineLeft;
			if (active != null && active.uiFont != null)
			{
				component3.font = active.uiFont;
			}
			LocalizedGameText.BindFontOnly(component3);
			GameObject gameObject5 = new GameObject("Placeholder", new Type[]
			{
				typeof(RectTransform),
				typeof(TextMeshProUGUI)
			});
			gameObject5.transform.SetParent(gameObject3.transform, false);
			RectTransform rectTransform9 = (RectTransform)gameObject5.transform;
			rectTransform9.anchorMin = Vector2.zero;
			rectTransform9.anchorMax = Vector2.one;
			rectTransform9.offsetMin = new Vector2(10f, 2f);
			rectTransform9.offsetMax = new Vector2(-10f, -2f);
			TextMeshProUGUI component4 = gameObject5.GetComponent<TextMeshProUGUI>();
			component4.text = "Name…";
			component4.fontSize = 14f;
			component4.fontStyle = FontStyles.Italic;
			component4.color = new Color(1f, 1f, 1f, 0.4f);
			component4.alignment = TextAlignmentOptions.MidlineLeft;
			if (active != null && active.uiFont != null)
			{
				component4.font = active.uiFont;
			}
			LocalizedGameText.BindSource(component4, "cielos_shell", "Name…", null).FitTranslationToRectangle(false);
			this._input = gameObject3.GetComponent<TMP_InputField>();
			this._input.textViewport = rectTransform7;
			this._input.textComponent = component3;
			this._input.placeholder = component4;
			this._input.lineType = TMP_InputField.LineType.SingleLine;
			this._input.onSubmit.AddListener(delegate(string _)
			{
				this.Confirm();
			});
			this.MakeButton(rectTransform2, "Cancel", new Vector2(1f, 0f), new Vector2(-94f, 14f), new Action(this.Cancel), (active != null) ? active.controlBackground : new Color(0.2f, 0.16f, 0.26f, 1f));
			this.MakeButton(rectTransform2, "OK", new Vector2(1f, 0f), new Vector2(-16f, 14f), new Action(this.Confirm), (active != null) ? active.accent : new Color(0.66f, 0.38f, 0.79f, 1f));
			base.gameObject.SetActive(false);
		}

		// Token: 0x06009C9E RID: 40094 RVA: 0x0040368C File Offset: 0x0040188C
		public void Show(string title, string initial, Action<string> onConfirm)
		{
			ControllerNavigationScope.Configure(base.gameObject, new Action(this.Cancel), (this._input != null) ? this._input.gameObject : null, 100, false);
			this._onConfirm = onConfirm;
			if (this._title != null)
			{
				LocalizedGameText.BindComputed(this._title, () => CielOSContentText.Preferred("cielos_shell", title)).FitTranslationToRectangle(false);
			}
			base.gameObject.SetActive(true);
			base.transform.SetAsLastSibling();
			if (this._input != null)
			{
				this._input.text = initial ?? "";
				this._input.ActivateInputField();
				this._input.caretPosition = this._input.text.Length;
			}
		}

		// Token: 0x06009C9F RID: 40095 RVA: 0x00403770 File Offset: 0x00401970
		private void Confirm()
		{
			string text = ((this._input != null) ? this._input.text : "");
			Action<string> onConfirm = this._onConfirm;
			this._onConfirm = null;
			base.gameObject.SetActive(false);
			if (!string.IsNullOrWhiteSpace(text) && onConfirm != null)
			{
				onConfirm(text.Trim());
			}
		}

		// Token: 0x06009CA0 RID: 40096 RVA: 0x004037CF File Offset: 0x004019CF
		private void Cancel()
		{
			this._onConfirm = null;
			base.gameObject.SetActive(false);
		}

		// Token: 0x06009CA1 RID: 40097 RVA: 0x004037E4 File Offset: 0x004019E4
		private TMP_Text MakeText(Transform parent, string name, string content, float size, Color color, FontStyles style)
		{
			GameObject gameObject = new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(TextMeshProUGUI)
			});
			gameObject.transform.SetParent(parent, false);
			TextMeshProUGUI component = gameObject.GetComponent<TextMeshProUGUI>();
			component.text = content;
			component.fontSize = size;
			component.color = color;
			component.fontStyle = style;
			component.raycastTarget = false;
			if (CielOSTheme.Active != null && CielOSTheme.Active.uiFont != null)
			{
				component.font = CielOSTheme.Active.uiFont;
			}
			return component;
		}

		// Token: 0x06009CA2 RID: 40098 RVA: 0x00403880 File Offset: 0x00401A80
		private void MakeButton(Transform parent, string label, Vector2 anchor, Vector2 pos, Action onClick, Color color)
		{
			GameObject gameObject = new GameObject(label, new Type[]
			{
				typeof(RectTransform),
				typeof(Image),
				typeof(Button)
			});
			gameObject.transform.SetParent(parent, false);
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			rectTransform.anchorMax = anchor;
			rectTransform.anchorMin = anchor;
			rectTransform.pivot = new Vector2(1f, 0f);
			rectTransform.sizeDelta = new Vector2(72f, 28f);
			rectTransform.anchoredPosition = pos;
			Image component = gameObject.GetComponent<Image>();
			component.color = color;
			CielOSTheme.Round(component);
			gameObject.GetComponent<Button>().onClick.AddListener(delegate
			{
				onClick();
			});
			TMP_Text tmp_Text = this.MakeText(gameObject.transform, "L", label, 13f, Color.white, FontStyles.Normal);
			LocalizedGameText.BindSource(tmp_Text, "cielos_shell", label, null).FitTranslationToRectangle(false);
			RectTransform rectTransform2 = (RectTransform)tmp_Text.transform;
			rectTransform2.anchorMin = Vector2.zero;
			rectTransform2.anchorMax = Vector2.one;
			rectTransform2.offsetMin = Vector2.zero;
			rectTransform2.offsetMax = Vector2.zero;
			tmp_Text.alignment = TextAlignmentOptions.Center;
		}

		// Token: 0x0400A136 RID: 41270
		private CanvasGroup _group;

		// Token: 0x0400A137 RID: 41271
		private TMP_InputField _input;

		// Token: 0x0400A138 RID: 41272
		private TMP_Text _title;

		// Token: 0x0400A139 RID: 41273
		private Action<string> _onConfirm;
	}
}
