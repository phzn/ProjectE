using System;
using System.Collections.Generic;
using Features.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ProjectExhibitionist.Core
{
	// Token: 0x02000A3B RID: 2619
	[DefaultExecutionOrder(-700)]
	public sealed class ControllerTextKeyboard : MonoBehaviour
	{
		// Token: 0x17000C8E RID: 3214
		// (get) Token: 0x06004A58 RID: 19032 RVA: 0x001CD027 File Offset: 0x001CB227
		public static bool IsOpen
		{
			get
			{
				return ControllerTextKeyboard._instance != null && ControllerTextKeyboard._instance.gameObject.activeInHierarchy;
			}
		}

		// Token: 0x17000C8F RID: 3215
		// (get) Token: 0x06004A59 RID: 19033 RVA: 0x001CD047 File Offset: 0x001CB247
		// (set) Token: 0x06004A5A RID: 19034 RVA: 0x001CD04E File Offset: 0x001CB24E
		public static bool IsCommitting { get; private set; }

		// Token: 0x17000C90 RID: 3216
		// (get) Token: 0x06004A5B RID: 19035 RVA: 0x001CD056 File Offset: 0x001CB256
		public static Transform Root
		{
			get
			{
				if (!ControllerTextKeyboard.IsOpen)
				{
					return null;
				}
				return ControllerTextKeyboard._instance.transform;
			}
		}

		// Token: 0x06004A5C RID: 19036 RVA: 0x001CD06C File Offset: 0x001CB26C
		public static void Open(GameObject target)
		{
			if (target == null || ControllerTextKeyboard.IsOpen)
			{
				return;
			}
			TMP_InputField component = target.GetComponent<TMP_InputField>();
			InputField component2 = target.GetComponent<InputField>();
			if ((component == null && component2 == null) || (component != null && component.readOnly) || (component2 != null && component2.readOnly))
			{
				return;
			}
			ControllerTextKeyboard._instance = new GameObject("Controller Text Keyboard", new Type[] { typeof(RectTransform) }).AddComponent<ControllerTextKeyboard>();
			ControllerTextKeyboard._instance._target = target;
			ControllerTextKeyboard._instance._tmp = component;
			ControllerTextKeyboard._instance._legacy = component2;
			ControllerTextKeyboard._instance._draft = ((component != null) ? component.text : component2.text);
			ControllerTextKeyboard._instance._caret = ControllerTextKeyboard._instance._draft.Length;
			ControllerTextKeyboard._instance._openedFrame = Time.frameCount;
			if (component != null)
			{
				component.DeactivateInputField(false);
			}
			if (component2 != null)
			{
				component2.DeactivateInputField();
			}
			ControllerTextKeyboard._instance.Build();
		}

		// Token: 0x06004A5D RID: 19037 RVA: 0x001CD187 File Offset: 0x001CB387
		public static void Cancel()
		{
			if (ControllerTextKeyboard._instance != null)
			{
				ControllerTextKeyboard._instance.Close(false);
			}
		}

		// Token: 0x06004A5E RID: 19038 RVA: 0x001CD1A1 File Offset: 0x001CB3A1
		private void OnDestroy()
		{
			if (ControllerTextKeyboard._instance == this)
			{
				ControllerTextKeyboard._instance = null;
			}
		}

		// Token: 0x06004A5F RID: 19039 RVA: 0x001CD1B8 File Offset: 0x001CB3B8
		private void Update()
		{
			if (this._target == null || !this._target.activeInHierarchy || GameplayInputGate.IsLoading)
			{
				this.Close(false);
				return;
			}
			if (Time.frameCount == this._openedFrame || !ControllerInput.IsControllerActive)
			{
				return;
			}
			if (ControllerInput.CancelPressed || Input.GetKeyDown(KeyCode.Escape))
			{
				ControllerInput.ConsumeUIInput();
				this.Close(false);
				return;
			}
			if (ControllerInput.GetButtonDown(ControllerButton.Start))
			{
				ControllerInput.ConsumeUIInput();
				this.Close(true);
				return;
			}
			if (ControllerInput.GetButtonDown(ControllerButton.West))
			{
				this.Backspace();
			}
			if (ControllerInput.GetButtonDown(ControllerButton.North))
			{
				this.ToggleCase();
			}
			if (ControllerInput.GetButtonDown(ControllerButton.LeftShoulder))
			{
				this._caret = Mathf.Max(0, this._caret - 1);
			}
			if (ControllerInput.GetButtonDown(ControllerButton.RightShoulder))
			{
				this._caret = Mathf.Min(this._draft.Length, this._caret + 1);
			}
			this.RefreshPreview();
		}

		// Token: 0x06004A60 RID: 19040 RVA: 0x001CD29C File Offset: 0x001CB49C
		private void Build()
		{
			Canvas canvas = base.gameObject.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			canvas.sortingOrder = 31000;
			CanvasScaler canvasScaler = base.gameObject.AddComponent<CanvasScaler>();
			canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			canvasScaler.referenceResolution = new Vector2(1920f, 1080f);
			canvasScaler.matchWidthOrHeight = 0.5f;
			base.gameObject.AddComponent<GraphicRaycaster>();
			base.gameObject.AddComponent<Image>().color = new Color(0.018f, 0.01f, 0.03f, 0.96f);
			RectTransform rectTransform = ControllerTextKeyboard.Rect("Keyboard", base.transform, new Vector2(0.5f, 0.5f), new Vector2(1000f, 650f), Vector2.zero);
			TMP_Text tmp_Text = this.Text(ControllerTextKeyboard.Rect("Title", rectTransform, new Vector2(0.5f, 1f), new Vector2(940f, 40f), new Vector2(0f, -35f)), "", 28f);
			this.BindCaption(tmp_Text, "Enter text", null);
			this._preview = this.Text(ControllerTextKeyboard.Rect("Text preview", rectTransform, new Vector2(0.5f, 1f), new Vector2(940f, 85f), new Vector2(0f, -105f)), "", 25f);
			this._preview.overflowMode = TextOverflowModes.Ellipsis;
			this._preview.richText = false;
			LocalizedGameText.BindFontOnly(this._preview);
			for (int i = 0; i < "qwertyuiopasdfghjkl'zxcvbnm,.-".Length; i++)
			{
				int index = i;
				int num = i / 10;
				int num2 = i % 10;
				Button button = this.Button(rectTransform, "Key " + i.ToString(), new Vector2(-423f + (float)num2 * 94f, 115f - (float)num * 76f), new Vector2(86f, 64f), "", delegate
				{
					this.Insert(this._keyValues[index]);
				}, null);
				this._keyLabels.Add(button.GetComponentInChildren<TMP_Text>());
				this._keyValues.Add("qwertyuiopasdfghjkl'zxcvbnm,.-"[i]);
			}
			this.Button(rectTransform, "Symbols", new Vector2(-380f, -150f), new Vector2(160f, 64f), "123 / ABC", delegate
			{
				this._symbols = !this._symbols;
				this.RefreshKeys();
			}, null);
			this.Button(rectTransform, "Case", new Vector2(-200f, -150f), new Vector2(160f, 64f), "Shift", new UnityAction(this.ToggleCase), "Y");
			this.Button(rectTransform, "Space", new Vector2(20f, -150f), new Vector2(240f, 64f), "Space", delegate
			{
				this.Insert(' ');
			}, null);
			this.Button(rectTransform, "Delete", new Vector2(260f, -150f), new Vector2(200f, 64f), "Delete", new UnityAction(this.Backspace), "X");
			this.Button(rectTransform, "Cancel", new Vector2(-190f, -238f), new Vector2(250f, 64f), "Cancel", delegate
			{
				this.Close(false);
			}, "B");
			this.Button(rectTransform, "Done", new Vector2(190f, -238f), new Vector2(250f, 64f), "Done", delegate
			{
				this.Close(true);
			}, "Menu");
			TMP_Text tmp_Text2 = this.Text(ControllerTextKeyboard.Rect("Help", rectTransform, new Vector2(0.5f, 0f), new Vector2(940f, 35f), new Vector2(0f, 22f)), "", 18f);
			tmp_Text2.richText = true;
			this._localizedCaptions.Add(LocalizedGameText.BindComputed(tmp_Text2, () => GameLocalization.FormatSource("controller_ui", "{0}: select     {1}: enter     {2}: move caret", new object[]
			{
				ControllerGlyphs.Format("LS / D-pad"),
				ControllerGlyphs.Format("A"),
				ControllerGlyphs.Format("LB / RB")
			})));
			this._localizedCaptions[this._localizedCaptions.Count - 1].FitTranslationToRectangle(false);
			this.RefreshKeys();
			this.RefreshPreview();
			if (EventSystem.current != null)
			{
				EventSystem.current.SetSelectedGameObject(this._keyLabels[0].transform.parent.gameObject);
			}
		}

		// Token: 0x06004A61 RID: 19041 RVA: 0x001CD748 File Offset: 0x001CB948
		private TMP_Text Text(RectTransform rect, string value, float size)
		{
			TextMeshProUGUI textMeshProUGUI = rect.gameObject.AddComponent<TextMeshProUGUI>();
			textMeshProUGUI.text = value;
			textMeshProUGUI.fontSize = size;
			textMeshProUGUI.color = Color.white;
			textMeshProUGUI.alignment = TextAlignmentOptions.Center;
			textMeshProUGUI.raycastTarget = false;
			textMeshProUGUI.richText = false;
			if (this._tmp != null && this._tmp.textComponent != null)
			{
				textMeshProUGUI.font = this._tmp.textComponent.font;
			}
			else if (TMP_Settings.defaultFontAsset != null)
			{
				textMeshProUGUI.font = TMP_Settings.defaultFontAsset;
			}
			return textMeshProUGUI;
		}

		// Token: 0x06004A62 RID: 19042 RVA: 0x001CD7E8 File Offset: 0x001CB9E8
		private Button Button(Transform parent, string name, Vector2 position, Vector2 size, string label, UnityAction click, string controllerKey = null)
		{
			RectTransform rectTransform = ControllerTextKeyboard.Rect(name, parent, Vector2.one * 0.5f, size, position);
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = new Color(0.18f, 0.12f, 0.24f, 1f);
			Button button = rectTransform.gameObject.AddComponent<Button>();
			button.targetGraphic = image;
			button.onClick.AddListener(click);
			TMP_Text tmp_Text = this.Text(ControllerTextKeyboard.Rect("Label", rectTransform, Vector2.one * 0.5f, size - new Vector2(6f, 6f), Vector2.zero), label, 23f);
			if (!string.IsNullOrEmpty(label) && label != "123 / ABC")
			{
				this.BindCaption(tmp_Text, label, controllerKey);
			}
			return button;
		}

		// Token: 0x06004A63 RID: 19043 RVA: 0x001CD8C0 File Offset: 0x001CBAC0
		private void BindCaption(TMP_Text label, string english, string controllerKey = null)
		{
			label.richText = true;
			LocalizedGameText localizedGameText = LocalizedGameText.BindComputed(label, () => ((controllerKey == null) ? "" : (ControllerGlyphs.Format(controllerKey) + "  ")) + GameLocalization.GetSource("controller_ui", english));
			localizedGameText.FitTranslationToRectangle(false);
			this._localizedCaptions.Add(localizedGameText);
		}

		// Token: 0x06004A64 RID: 19044 RVA: 0x001CD910 File Offset: 0x001CBB10
		private void LateUpdate()
		{
			if (this._captionGlyphVersion == ControllerGlyphs.Version)
			{
				return;
			}
			this._captionGlyphVersion = ControllerGlyphs.Version;
			foreach (LocalizedGameText localizedGameText in this._localizedCaptions)
			{
				if (localizedGameText != null)
				{
					localizedGameText.Refresh();
				}
			}
		}

		// Token: 0x06004A65 RID: 19045 RVA: 0x001CD984 File Offset: 0x001CBB84
		private static RectTransform Rect(string name, Transform parent, Vector2 anchor, Vector2 size, Vector2 position)
		{
			RectTransform rectTransform = (RectTransform)new GameObject(name, new Type[] { typeof(RectTransform) }).transform;
			rectTransform.SetParent(parent, false);
			rectTransform.anchorMax = anchor;
			rectTransform.anchorMin = anchor;
			rectTransform.sizeDelta = size;
			rectTransform.anchoredPosition = position;
			return rectTransform;
		}

		// Token: 0x06004A66 RID: 19046 RVA: 0x001CD9DB File Offset: 0x001CBBDB
		private void ToggleCase()
		{
			this._upper = !this._upper;
			this.RefreshKeys();
		}

		// Token: 0x06004A67 RID: 19047 RVA: 0x001CD9F4 File Offset: 0x001CBBF4
		private void RefreshKeys()
		{
			string text = (this._symbols ? "1234567890!@#$%^&*()_+=/\\:;?" : "qwertyuiopasdfghjkl'zxcvbnm,.-");
			for (int i = 0; i < this._keyLabels.Count; i++)
			{
				char c = ((i < text.Length) ? text[i] : ' ');
				if (this._upper && !this._symbols)
				{
					c = char.ToUpperInvariant(c);
				}
				this._keyValues[i] = c;
				this._keyLabels[i].text = c.ToString();
			}
		}

		// Token: 0x06004A68 RID: 19048 RVA: 0x001CDA80 File Offset: 0x001CBC80
		private void Insert(char character)
		{
			int num = ((this._tmp != null) ? this._tmp.characterLimit : this._legacy.characterLimit);
			if (num > 0 && this._draft.Length >= num)
			{
				return;
			}
			if (this._tmp != null && this._tmp.onValidateInput != null)
			{
				character = this._tmp.onValidateInput(this._draft, this._caret, character);
			}
			else if (this._legacy != null && this._legacy.onValidateInput != null)
			{
				character = this._legacy.onValidateInput(this._draft, this._caret, character);
			}
			else
			{
				string text = ((this._tmp != null) ? this._tmp.characterValidation.ToString() : this._legacy.characterValidation.ToString());
				character = ControllerTextKeyboard.ValidateCharacter(this._draft, this._caret, character, text);
			}
			if (character == '\0')
			{
				return;
			}
			string draft = this._draft;
			int caret = this._caret;
			this._caret = caret + 1;
			this._draft = draft.Insert(caret, character.ToString());
			this.RefreshPreview();
		}

		// Token: 0x06004A69 RID: 19049 RVA: 0x001CDBD0 File Offset: 0x001CBDD0
		public static char ValidateCharacter(string text, int position, char character, string validation)
		{
			if (validation == "Integer" || validation == "Decimal" || validation == "Digit")
			{
				if (character >= '0' && character <= '9')
				{
					return character;
				}
				if (character == '-' && position == 0 && !text.Contains("-") && validation != "Digit")
				{
					return character;
				}
				if (character == '.' && validation == "Decimal" && !text.Contains("."))
				{
					return character;
				}
				return '\0';
			}
			else
			{
				if (validation == "Alphanumeric" && !char.IsLetterOrDigit(character))
				{
					return '\0';
				}
				if (validation == "Name" && !char.IsLetter(character) && character != ' ' && character != '\'')
				{
					return '\0';
				}
				if (validation == "EmailAddress" && (character == ' ' || (character == '@' && text.Contains("@"))))
				{
					return '\0';
				}
				return character;
			}
		}

		// Token: 0x06004A6A RID: 19050 RVA: 0x001CDCB8 File Offset: 0x001CBEB8
		private void Backspace()
		{
			if (this._caret == 0)
			{
				return;
			}
			string draft = this._draft;
			int num = this._caret - 1;
			this._caret = num;
			this._draft = draft.Remove(num, 1);
			this.RefreshPreview();
		}

		// Token: 0x06004A6B RID: 19051 RVA: 0x001CDCF8 File Offset: 0x001CBEF8
		private void RefreshPreview()
		{
			if (this._preview == null)
			{
				return;
			}
			string text = (((this._tmp != null && this._tmp.inputType == TMP_InputField.InputType.Password) || (this._legacy != null && this._legacy.inputType == InputField.InputType.Password)) ? new string('*', this._draft.Length) : this._draft);
			this._preview.text = text.Insert(this._caret, "|");
			this.EnsurePreviewFont(text);
		}

		// Token: 0x06004A6C RID: 19052 RVA: 0x001CDD94 File Offset: 0x001CBF94
		private void EnsurePreviewFont(string display)
		{
			if (this._previewFontText == display && this._previewFontLocale == GameLocalization.CurrentCode)
			{
				return;
			}
			this._previewFontText = display;
			this._previewFontLocale = GameLocalization.CurrentCode;
			LocalizedGameText component = this._preview.GetComponent<LocalizedGameText>();
			if (component == null)
			{
				return;
			}
			TMP_FontAsset font = GameLocalization.GetFont(LocalizedGameText.SourceFont(this._preview));
			uint[] array;
			if (font != null && font.HasCharacters(display, out array, true, false))
			{
				component.SetFontOverride(null);
				return;
			}
			GameLocalizationConfig config = GameLocalization.Config;
			if (config == null)
			{
				return;
			}
			foreach (GameLocalizationConfig.Language language in config.languages)
			{
				uint[] array2;
				if (language.font != null && language.font.HasCharacters(display, out array2, true, false))
				{
					component.SetFontOverride(language.font);
					return;
				}
			}
		}

		// Token: 0x06004A6D RID: 19053 RVA: 0x001CDE7C File Offset: 0x001CC07C
		private void Close(bool commit)
		{
			GameObject target = this._target;
			TMP_InputField tmp = this._tmp;
			InputField legacy = this._legacy;
			base.gameObject.SetActive(false);
			ControllerTextKeyboard._instance = null;
			if (target != null && target.activeInHierarchy && commit)
			{
				if (tmp != null)
				{
					tmp.text = this._draft;
					tmp.caretPosition = this._caret;
					tmp.onEndEdit.Invoke(tmp.text);
					if (!(tmp != null) || !tmp.gameObject.activeInHierarchy)
					{
						goto IL_00DA;
					}
					ControllerTextKeyboard.IsCommitting = true;
					try
					{
						tmp.onSubmit.Invoke(tmp.text);
						goto IL_00DA;
					}
					finally
					{
						ControllerTextKeyboard.IsCommitting = false;
					}
				}
				if (legacy != null)
				{
					legacy.text = this._draft;
					legacy.caretPosition = this._caret;
					legacy.onEndEdit.Invoke(legacy.text);
				}
			}
			IL_00DA:
			if (target != null && target.activeInHierarchy && EventSystem.current != null)
			{
				EventSystem.current.SetSelectedGameObject(target);
			}
			Object.Destroy(base.gameObject);
		}

		// Token: 0x04004C8E RID: 19598
		private static ControllerTextKeyboard _instance;

		// Token: 0x04004C90 RID: 19600
		private GameObject _target;

		// Token: 0x04004C91 RID: 19601
		private TMP_InputField _tmp;

		// Token: 0x04004C92 RID: 19602
		private InputField _legacy;

		// Token: 0x04004C93 RID: 19603
		private TMP_Text _preview;

		// Token: 0x04004C94 RID: 19604
		private string _draft;

		// Token: 0x04004C95 RID: 19605
		private string _previewFontText;

		// Token: 0x04004C96 RID: 19606
		private string _previewFontLocale;

		// Token: 0x04004C97 RID: 19607
		private int _caret;

		// Token: 0x04004C98 RID: 19608
		private bool _upper;

		// Token: 0x04004C99 RID: 19609
		private bool _symbols;

		// Token: 0x04004C9A RID: 19610
		private int _openedFrame;

		// Token: 0x04004C9B RID: 19611
		private readonly List<TMP_Text> _keyLabels = new List<TMP_Text>();

		// Token: 0x04004C9C RID: 19612
		private readonly List<char> _keyValues = new List<char>();

		// Token: 0x04004C9D RID: 19613
		private readonly List<LocalizedGameText> _localizedCaptions = new List<LocalizedGameText>();

		// Token: 0x04004C9E RID: 19614
		private int _captionGlyphVersion = -1;

		// Token: 0x04004C9F RID: 19615
		private const string Letters = "qwertyuiopasdfghjkl'zxcvbnm,.-";

		// Token: 0x04004CA0 RID: 19616
		private const string Symbols = "1234567890!@#$%^&*()_+=/\\:;?";
	}
}
