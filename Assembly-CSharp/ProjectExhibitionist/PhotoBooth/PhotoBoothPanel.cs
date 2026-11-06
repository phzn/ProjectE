using System;
using System.Collections;
using System.Collections.Generic;
using Features.Localization;
using ProjectExhibitionist.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ProjectExhibitionist.PhotoBooth
{
	// Token: 0x02000984 RID: 2436
	[DisallowMultipleComponent]
	public sealed class PhotoBoothPanel : MonoBehaviour
	{
		// Token: 0x17000B78 RID: 2936
		// (get) Token: 0x0600432D RID: 17197 RVA: 0x00191BE0 File Offset: 0x0018FDE0
		public bool SettingsVisible
		{
			get
			{
				return this._settingsVisible;
			}
		}

		// Token: 0x0600432E RID: 17198 RVA: 0x00191BE8 File Offset: 0x0018FDE8
		private void LateUpdate()
		{
			bool isActive = ControllerInput.IsActive;
			if (this._controls == null || isActive == this._controllerControls)
			{
				return;
			}
			this._controllerControls = isActive;
			this.RefreshControls();
		}

		// Token: 0x0600432F RID: 17199 RVA: 0x00191C20 File Offset: 0x0018FE20
		private void RefreshControls()
		{
			LocalizedLegacyGameText.BindComputed(this._controls, () => GameLocalization.GetSource("photobooth_ui", ControllerInput.IsActive ? "D-PAD  SELECT / CHANGE\nLT + RS  ZOOM          RS  MOVE VIEW\nA  TAKE PHOTO       Y  HIDE UI\nB  EXIT" : this._keyboardControls));
		}

		// Token: 0x06004330 RID: 17200 RVA: 0x00191C3A File Offset: 0x0018FE3A
		public void Initialize(PhotoBoothController controller)
		{
			this._controller = controller;
			this.Hide();
		}

		// Token: 0x06004331 RID: 17201 RVA: 0x00191C49 File Offset: 0x0018FE49
		public void Show()
		{
			this.EnsureUI();
			if (this._canvasObject != null)
			{
				this._canvasObject.SetActive(true);
			}
			this.SetSettingsVisible(true, true);
			this.Refresh();
		}

		// Token: 0x06004332 RID: 17202 RVA: 0x00191C79 File Offset: 0x0018FE79
		public void Hide()
		{
			if (this._settingsVisibilityRoutine != null)
			{
				base.StopCoroutine(this._settingsVisibilityRoutine);
				this._settingsVisibilityRoutine = null;
			}
			if (this._canvasObject != null)
			{
				this._canvasObject.SetActive(false);
			}
		}

		// Token: 0x06004333 RID: 17203 RVA: 0x00191CB0 File Offset: 0x0018FEB0
		public void ToggleSettingsVisibility()
		{
			this.SetSettingsVisible(!this._settingsVisible, false);
		}

		// Token: 0x06004334 RID: 17204 RVA: 0x00191CC4 File Offset: 0x0018FEC4
		public void SetSettingsVisible(bool visible, bool immediate = false)
		{
			this.EnsureUI();
			this._settingsVisible = visible;
			if (this._settingsPanelRect == null || this._settingsCanvasGroup == null)
			{
				return;
			}
			if (this._settingsVisibilityRoutine != null)
			{
				base.StopCoroutine(this._settingsVisibilityRoutine);
				this._settingsVisibilityRoutine = null;
			}
			if (immediate || this._canvasObject == null || !this._canvasObject.activeInHierarchy)
			{
				this.ApplySettingsVisibility(visible ? 1f : 0f);
				return;
			}
			this._settingsVisibilityRoutine = base.StartCoroutine(this.AnimateSettingsVisibility(visible));
		}

		// Token: 0x06004335 RID: 17205 RVA: 0x00191D5D File Offset: 0x0018FF5D
		public void SetCaptureVisibility(bool visible)
		{
			if (this._canvasGroup != null)
			{
				this._canvasGroup.alpha = (visible ? 1f : 0f);
			}
		}

		// Token: 0x06004336 RID: 17206 RVA: 0x00191D88 File Offset: 0x0018FF88
		public void ShowCaptureFeedback(string fileName)
		{
			if (this._status != null)
			{
				LocalizedLegacyGameText.BindFormat(this._status, "photobooth_ui", "SAVED  -  {0}", new object[] { fileName });
			}
			if (this._flashRoutine != null)
			{
				base.StopCoroutine(this._flashRoutine);
			}
			this._flashRoutine = base.StartCoroutine(this.FlashRoutine());
		}

		// Token: 0x06004337 RID: 17207 RVA: 0x00191DEC File Offset: 0x0018FFEC
		public void Refresh()
		{
			if (this._controller == null || this._canvasObject == null)
			{
				return;
			}
			for (int i = 0; i < this._rowBackgrounds.Count; i++)
			{
				this._rowBackgrounds[i].color = ((i == this._controller.SelectedSetting) ? PhotoBoothPanel.SelectedColor : PhotoBoothPanel.RowColor);
			}
			if (this._valueLabels.Count >= 8)
			{
				for (int j = 0; j < this._valueLabels.Count; j++)
				{
					int setting = j;
					LocalizedLegacyGameText.BindComputed(this._valueLabels[j], () => this.SettingValue(setting));
				}
			}
			if (this._intensityFill != null)
			{
				this._intensityFill.fillAmount = Mathf.InverseLerp(0.25f, 2f, this._controller.LightIntensityMultiplier);
			}
		}

		// Token: 0x06004338 RID: 17208 RVA: 0x00191EE0 File Offset: 0x001900E0
		private string SettingValue(int setting)
		{
			if (this._controller == null)
			{
				return "-";
			}
			string text;
			switch (setting)
			{
			case 0:
				return this._controller.IntensityLabel;
			case 1:
				text = this._controller.LightColorLabel;
				break;
			case 2:
				text = this._controller.BackdropLabel;
				break;
			case 3:
				text = this._controller.EffectLabel;
				break;
			case 4:
				text = this._controller.FilterLabel;
				break;
			case 5:
				text = this._controller.PoseLabel;
				if (!text.StartsWith("POSE ", StringComparison.Ordinal))
				{
					return GameLocalization.GetSource("photobooth_ui", text);
				}
				return GameLocalization.FormatSource("photobooth_ui", "POSE {0}", new object[] { text.Substring(5) });
			case 6:
				text = this._controller.FaceLabel;
				break;
			case 7:
				text = this._controller.LayoutLabel;
				break;
			default:
				return "-";
			}
			return GameLocalization.GetSource("photobooth_ui", text);
		}

		// Token: 0x06004339 RID: 17209 RVA: 0x00191FEC File Offset: 0x001901EC
		private void EnsureUI()
		{
			if (this._canvasObject != null)
			{
				return;
			}
			this._canvasObject = new GameObject("PhotoBooth Settings UI", new Type[]
			{
				typeof(RectTransform),
				typeof(Canvas),
				typeof(CanvasScaler),
				typeof(GraphicRaycaster),
				typeof(CanvasGroup)
			});
			this._canvasObject.transform.SetParent(base.transform, false);
			Vector3 lossyScale = this._canvasObject.transform.lossyScale;
			this._canvasObject.transform.localScale = new Vector3(this._canvasObject.transform.localScale.x / Mathf.Max(0.0001f, Mathf.Abs(lossyScale.x)), this._canvasObject.transform.localScale.y / Mathf.Max(0.0001f, Mathf.Abs(lossyScale.y)), this._canvasObject.transform.localScale.z / Mathf.Max(0.0001f, Mathf.Abs(lossyScale.z)));
			Canvas component = this._canvasObject.GetComponent<Canvas>();
			component.renderMode = RenderMode.ScreenSpaceOverlay;
			component.sortingOrder = 1000;
			GameplayHudVisibility.RegisterCanvas(component);
			CanvasScaler component2 = this._canvasObject.GetComponent<CanvasScaler>();
			component2.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			component2.referenceResolution = new Vector2(1920f, 1080f);
			component2.matchWidthOrHeight = 0.5f;
			this._canvasGroup = this._canvasObject.GetComponent<CanvasGroup>();
			this._canvasGroup.blocksRaycasts = true;
			this._canvasGroup.interactable = true;
			RectTransform rectTransform = (RectTransform)this._canvasObject.transform;
			this._flash = PhotoBoothPanel.NewImage(rectTransform, "Capture Flash", Color.clear);
			PhotoBoothPanel.Stretch(this._flash.rectTransform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
			this._flash.raycastTarget = false;
			Image image = PhotoBoothPanel.NewImage(rectTransform, "Settings Panel", PhotoBoothPanel.PanelColor);
			RectTransform rectTransform2 = image.rectTransform;
			rectTransform2.anchorMin = new Vector2(1f, 0f);
			rectTransform2.anchorMax = new Vector2(1f, 1f);
			rectTransform2.pivot = new Vector2(1f, 0.5f);
			rectTransform2.sizeDelta = new Vector2(520f, 0f);
			rectTransform2.anchoredPosition = Vector2.zero;
			this._settingsPanelRect = rectTransform2;
			this._settingsCanvasGroup = image.gameObject.AddComponent<CanvasGroup>();
			this._settingsCanvasGroup.alpha = 1f;
			this._settingsCanvasGroup.interactable = true;
			this._settingsCanvasGroup.blocksRaycasts = true;
			Image image2 = PhotoBoothPanel.NewImage(rectTransform2, "Header", new Color(0.16f, 0.045f, 0.105f, 1f));
			image2.raycastTarget = false;
			image2.rectTransform.anchorMin = new Vector2(0f, 1f);
			image2.rectTransform.anchorMax = new Vector2(1f, 1f);
			image2.rectTransform.pivot = new Vector2(0.5f, 1f);
			image2.rectTransform.sizeDelta = new Vector2(0f, 102f);
			image2.rectTransform.anchoredPosition = Vector2.zero;
			PhotoBoothPanel.SetRect(PhotoBoothPanel.NewImage(rectTransform2, "Accent", PhotoBoothPanel.AccentColor).rectTransform, new Vector2(0f, 0f), new Vector2(0f, 1f), new Vector2(0f, 0.5f), new Vector2(7f, 0f), new Vector2(3.5f, 0f));
			Text text = PhotoBoothPanel.NewText(rectTransform2, "Title", "PHOTO BOOTH", 34, FontStyle.Bold, TextAnchor.MiddleLeft);
			PhotoBoothPanel.Anchor(text.rectTransform, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(450f, 44f), new Vector2(28f, -20f));
			text.color = Color.white;
			Text text2 = PhotoBoothPanel.NewText(rectTransform2, "Subtitle", "CREATE YOUR SHOT", 15, FontStyle.Bold, TextAnchor.MiddleLeft);
			PhotoBoothPanel.Anchor(text2.rectTransform, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(450f, 26f), new Vector2(29f, -70f));
			text2.color = new Color(1f, 0.6f, 0.78f, 0.9f);
			string[] array = new string[] { "BRIGHTNESS", "LIGHT COLOR", "BACKGROUND", "EFFECT", "FILTER", "POSE", "FACE", "PHOTO LAYOUT" };
			for (int i = 0; i < array.Length; i++)
			{
				this.BuildRow(rectTransform2, i, array[i], -112f - (float)i * 74f);
			}
			this._status = PhotoBoothPanel.NewText(rectTransform2, "Status", "READY", 15, FontStyle.Bold, TextAnchor.MiddleLeft);
			PhotoBoothPanel.Anchor(this._status.rectTransform, Vector2.zero, Vector2.zero, new Vector2(460f, 24f), new Vector2(28f, 98f));
			this._status.color = new Color(0.58f, 1f, 0.76f, 0.92f);
			Text text3 = PhotoBoothPanel.NewText(rectTransform2, "Controls", "UP/DOWN  SELECT       LEFT/RIGHT  CHANGE\nWHEEL  ZOOM          MMB + DRAG  MOVE VIEW\nSPACE  TAKE PHOTO       TAB  HIDE UI\nE  EXIT", 13, FontStyle.Normal, TextAnchor.MiddleLeft);
			PhotoBoothPanel.Anchor(text3.rectTransform, Vector2.zero, Vector2.zero, new Vector2(465f, 84f), new Vector2(28f, 10f));
			text3.color = new Color(1f, 1f, 1f, 0.72f);
			this._controls = text3;
			this._keyboardControls = "UP/DOWN  SELECT       LEFT/RIGHT  CHANGE\nWHEEL  ZOOM          MMB + DRAG  MOVE VIEW\nSPACE  TAKE PHOTO       TAB  HIDE UI\nE  EXIT";
			this._controllerControls = ControllerInput.IsActive;
			this.RefreshControls();
		}

		// Token: 0x0600433A RID: 17210 RVA: 0x00192608 File Offset: 0x00190808
		private void BuildRow(RectTransform parent, int index, string label, float y)
		{
			Image image = PhotoBoothPanel.NewImage(parent, "Row " + label, PhotoBoothPanel.RowColor);
			RectTransform rectTransform = image.rectTransform;
			rectTransform.anchorMin = new Vector2(0f, 1f);
			rectTransform.anchorMax = new Vector2(1f, 1f);
			rectTransform.pivot = new Vector2(0.5f, 1f);
			rectTransform.sizeDelta = new Vector2(-32f, 66f);
			rectTransform.anchoredPosition = new Vector2(0f, y);
			this._rowBackgrounds.Add(image);
			Button button = image.gameObject.AddComponent<Button>();
			button.targetGraphic = image;
			int capturedIndex = index;
			button.onClick.AddListener(delegate
			{
				if (this._controller != null)
				{
					this._controller.SelectSetting(capturedIndex);
				}
			});
			Text text = PhotoBoothPanel.NewText(rectTransform, "Name", label, 13, FontStyle.Bold, TextAnchor.MiddleLeft);
			PhotoBoothPanel.Anchor(text.rectTransform, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(300f, 18f), new Vector2(20f, -5f));
			text.color = new Color(1f, 1f, 1f, 0.64f);
			Text text2 = PhotoBoothPanel.NewText(rectTransform, "Value", "-", 22, FontStyle.Bold, TextAnchor.MiddleLeft);
			PhotoBoothPanel.Anchor(text2.rectTransform, Vector2.zero, Vector2.zero, new Vector2(330f, 34f), new Vector2(20f, (index == 0) ? 14f : 8f));
			text2.color = Color.white;
			this._valueLabels.Add(text2);
			this.NewButton(rectTransform, "Previous", "<", -82f, delegate
			{
				if (this._controller == null)
				{
					return;
				}
				this._controller.SelectSetting(capturedIndex);
				this._controller.AdjustSelectedSetting(-1);
			}).transform.SetAsLastSibling();
			this.NewButton(rectTransform, "Next", ">", -26f, delegate
			{
				if (this._controller == null)
				{
					return;
				}
				this._controller.SelectSetting(capturedIndex);
				this._controller.AdjustSelectedSetting(1);
			}).transform.SetAsLastSibling();
			if (index != 0)
			{
				return;
			}
			Image image2 = PhotoBoothPanel.NewImage(rectTransform, "Intensity Track", new Color(1f, 1f, 1f, 0.14f));
			PhotoBoothPanel.Anchor(image2.rectTransform, Vector2.zero, Vector2.zero, new Vector2(310f, 6f), new Vector2(20f, 8f));
			this._intensityFill = PhotoBoothPanel.NewImage(image2.rectTransform, "Fill", PhotoBoothPanel.AccentColor);
			PhotoBoothPanel.Stretch(this._intensityFill.rectTransform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
			this._intensityFill.type = Image.Type.Filled;
			this._intensityFill.fillMethod = Image.FillMethod.Horizontal;
		}

		// Token: 0x0600433B RID: 17211 RVA: 0x001928D0 File Offset: 0x00190AD0
		private Button NewButton(RectTransform parent, string name, string label, float anchoredX, UnityAction action)
		{
			Image image = PhotoBoothPanel.NewImage(parent, name, new Color(1f, 1f, 1f, 0.12f));
			RectTransform rectTransform = image.rectTransform;
			rectTransform.anchorMin = new Vector2(1f, 0.5f);
			rectTransform.anchorMax = new Vector2(1f, 0.5f);
			rectTransform.pivot = new Vector2(0.5f, 0.5f);
			rectTransform.sizeDelta = new Vector2(48f, 50f);
			rectTransform.anchoredPosition = new Vector2(anchoredX, 0f);
			Button button = image.gameObject.AddComponent<Button>();
			button.targetGraphic = image;
			button.onClick.AddListener(action);
			PhotoBoothPanel.Stretch(PhotoBoothPanel.NewText(rectTransform, "Label", label, 24, FontStyle.Bold, TextAnchor.MiddleCenter).rectTransform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
			return button;
		}

		// Token: 0x0600433C RID: 17212 RVA: 0x001929B8 File Offset: 0x00190BB8
		private IEnumerator FlashRoutine()
		{
			if (this._flash == null)
			{
				yield break;
			}
			this._flash.transform.SetAsLastSibling();
			for (float t = 0f; t < 0.34f; t += Time.unscaledDeltaTime)
			{
				float num = t / 0.34f;
				float num2 = ((num < 0.18f) ? (num / 0.18f) : (1f - (num - 0.18f) / 0.82f));
				this._flash.color = new Color(1f, 0.94f, 0.98f, Mathf.Clamp01(num2) * 0.92f);
				yield return null;
			}
			this._flash.color = Color.clear;
			this._flashRoutine = null;
			yield break;
		}

		// Token: 0x0600433D RID: 17213 RVA: 0x001929C7 File Offset: 0x00190BC7
		private IEnumerator AnimateSettingsVisibility(bool visible)
		{
			float startAlpha = this._settingsCanvasGroup.alpha;
			Vector2 startPosition = this._settingsPanelRect.anchoredPosition;
			float targetAlpha = (visible ? 1f : 0f);
			Vector2 targetPosition = (visible ? PhotoBoothPanel.SettingsShownPosition : PhotoBoothPanel.SettingsHiddenPosition);
			this._settingsCanvasGroup.interactable = false;
			this._settingsCanvasGroup.blocksRaycasts = false;
			float elapsed = 0f;
			while (elapsed < 0.24f)
			{
				elapsed += Time.unscaledDeltaTime;
				float num = Mathf.Clamp01(elapsed / 0.24f);
				float num2 = num * num * (3f - 2f * num);
				this._settingsCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, num2);
				this._settingsPanelRect.anchoredPosition = Vector2.LerpUnclamped(startPosition, targetPosition, num2);
				yield return null;
			}
			this.ApplySettingsVisibility(targetAlpha);
			this._settingsVisibilityRoutine = null;
			yield break;
		}

		// Token: 0x0600433E RID: 17214 RVA: 0x001929E0 File Offset: 0x00190BE0
		private void ApplySettingsVisibility(float visibility)
		{
			bool flag = visibility > 0.5f;
			this._settingsCanvasGroup.alpha = (flag ? 1f : 0f);
			this._settingsCanvasGroup.interactable = flag;
			this._settingsCanvasGroup.blocksRaycasts = flag;
			this._settingsPanelRect.anchoredPosition = (flag ? PhotoBoothPanel.SettingsShownPosition : PhotoBoothPanel.SettingsHiddenPosition);
		}

		// Token: 0x0600433F RID: 17215 RVA: 0x00192A44 File Offset: 0x00190C44
		private static Image NewImage(Transform parent, string name, Color color)
		{
			GameObject gameObject = new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Image)
			});
			gameObject.transform.SetParent(parent, false);
			Image component = gameObject.GetComponent<Image>();
			component.color = color;
			return component;
		}

		// Token: 0x06004340 RID: 17216 RVA: 0x00192AA0 File Offset: 0x00190CA0
		private static Text NewText(Transform parent, string name, string content, int size, FontStyle style, TextAnchor alignment)
		{
			GameObject gameObject = new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Text)
			});
			gameObject.transform.SetParent(parent, false);
			Text component = gameObject.GetComponent<Text>();
			component.text = content;
			component.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
			component.fontSize = size;
			component.fontStyle = style;
			component.alignment = alignment;
			component.horizontalOverflow = HorizontalWrapMode.Overflow;
			component.verticalOverflow = VerticalWrapMode.Overflow;
			component.raycastTarget = false;
			((content == "-" || content == "<" || content == ">") ? LocalizedLegacyGameText.BindFontOnly(component) : LocalizedLegacyGameText.BindSource(component, "photobooth_ui", content)).FitTranslationToRectangle();
			return component;
		}

		// Token: 0x06004341 RID: 17217 RVA: 0x0018F85B File Offset: 0x0018DA5B
		private static void Stretch(RectTransform rect, Vector2 min, Vector2 max, Vector2 offsetMin, Vector2 offsetMax)
		{
			rect.anchorMin = min;
			rect.anchorMax = max;
			rect.offsetMin = offsetMin;
			rect.offsetMax = offsetMax;
		}

		// Token: 0x06004342 RID: 17218 RVA: 0x00192B77 File Offset: 0x00190D77
		private static void SetRect(RectTransform rect, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector2 size, Vector2 position)
		{
			rect.anchorMin = anchorMin;
			rect.anchorMax = anchorMax;
			rect.pivot = pivot;
			rect.sizeDelta = size;
			rect.anchoredPosition = position;
		}

		// Token: 0x06004343 RID: 17219 RVA: 0x00192B9E File Offset: 0x00190D9E
		private static void Anchor(RectTransform rect, Vector2 anchor, Vector2 pivot, Vector2 size, Vector2 position)
		{
			rect.anchorMin = anchor;
			rect.anchorMax = anchor;
			rect.pivot = pivot;
			rect.sizeDelta = size;
			rect.anchoredPosition = position;
		}

		// Token: 0x0400432C RID: 17196
		private static readonly Color PanelColor = new Color(0.025f, 0.032f, 0.052f, 0.975f);

		// Token: 0x0400432D RID: 17197
		private static readonly Color RowColor = new Color(0.13f, 0.16f, 0.22f, 0.96f);

		// Token: 0x0400432E RID: 17198
		private static readonly Color SelectedColor = new Color(0.48f, 0.08f, 0.28f, 0.98f);

		// Token: 0x0400432F RID: 17199
		private static readonly Color AccentColor = new Color(1f, 0.34f, 0.64f, 1f);

		// Token: 0x04004330 RID: 17200
		private PhotoBoothController _controller;

		// Token: 0x04004331 RID: 17201
		private GameObject _canvasObject;

		// Token: 0x04004332 RID: 17202
		private CanvasGroup _canvasGroup;

		// Token: 0x04004333 RID: 17203
		private RectTransform _settingsPanelRect;

		// Token: 0x04004334 RID: 17204
		private CanvasGroup _settingsCanvasGroup;

		// Token: 0x04004335 RID: 17205
		private readonly List<Image> _rowBackgrounds = new List<Image>(8);

		// Token: 0x04004336 RID: 17206
		private readonly List<Text> _valueLabels = new List<Text>(8);

		// Token: 0x04004337 RID: 17207
		private Image _intensityFill;

		// Token: 0x04004338 RID: 17208
		private Image _flash;

		// Token: 0x04004339 RID: 17209
		private Text _status;

		// Token: 0x0400433A RID: 17210
		private Text _controls;

		// Token: 0x0400433B RID: 17211
		private string _keyboardControls;

		// Token: 0x0400433C RID: 17212
		private bool _controllerControls;

		// Token: 0x0400433D RID: 17213
		private Coroutine _flashRoutine;

		// Token: 0x0400433E RID: 17214
		private Coroutine _settingsVisibilityRoutine;

		// Token: 0x0400433F RID: 17215
		private bool _settingsVisible = true;

		// Token: 0x04004340 RID: 17216
		private static readonly Vector2 SettingsShownPosition = Vector2.zero;

		// Token: 0x04004341 RID: 17217
		private static readonly Vector2 SettingsHiddenPosition = new Vector2(92f, 0f);

		// Token: 0x04004342 RID: 17218
		private const float SettingsTransitionDuration = 0.24f;
	}
}
