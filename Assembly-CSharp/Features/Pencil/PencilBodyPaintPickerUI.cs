using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Features.Localization;
using ProjectExhibitionist;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.Pencil
{
	// Token: 0x02000E83 RID: 3715
	public sealed class PencilBodyPaintPickerUI : MonoBehaviour
	{
		// Token: 0x140000C1 RID: 193
		// (add) Token: 0x06006F53 RID: 28499 RVA: 0x002CEC84 File Offset: 0x002CCE84
		// (remove) Token: 0x06006F54 RID: 28500 RVA: 0x002CECBC File Offset: 0x002CCEBC
		public event Action<SkinDecal> ApplyRequested;

		// Token: 0x140000C2 RID: 194
		// (add) Token: 0x06006F55 RID: 28501 RVA: 0x002CECF4 File Offset: 0x002CCEF4
		// (remove) Token: 0x06006F56 RID: 28502 RVA: 0x002CED2C File Offset: 0x002CCF2C
		public event Action CancelRequested;

		// Token: 0x1700123F RID: 4671
		// (get) Token: 0x06006F57 RID: 28503 RVA: 0x002CED61 File Offset: 0x002CCF61
		public bool IsOpen
		{
			get
			{
				return this._canvasRoot != null && this._canvasRoot.activeSelf;
			}
		}

		// Token: 0x17001240 RID: 4672
		// (get) Token: 0x06006F58 RID: 28504 RVA: 0x002CED7E File Offset: 0x002CCF7E
		public int CurrentIndex
		{
			get
			{
				return this._currentIndex;
			}
		}

		// Token: 0x17001241 RID: 4673
		// (get) Token: 0x06006F59 RID: 28505 RVA: 0x002CED86 File Offset: 0x002CCF86
		public int OptionCount
		{
			get
			{
				return this._options.Count;
			}
		}

		// Token: 0x17001242 RID: 4674
		// (get) Token: 0x06006F5A RID: 28506 RVA: 0x002CED93 File Offset: 0x002CCF93
		public int SuspendedCanvasCount
		{
			get
			{
				return this._suspendedCanvases.Count;
			}
		}

		// Token: 0x06006F5B RID: 28507 RVA: 0x002CEDA0 File Offset: 0x002CCFA0
		public void Show(PencilBodyPaintCatalogSO catalog, SkinDecal current)
		{
			this._font = ((catalog != null) ? catalog.uiFont : null);
			this.EnsureBuilt();
			this.RebuildOptions((catalog != null) ? catalog.options : null);
			if (this._itemIcon != null)
			{
				this._itemIcon.sprite = ((catalog != null && catalog.pencilItem != null) ? catalog.pencilItem.icon : null);
				this._itemIcon.enabled = this._itemIcon.sprite != null;
			}
			int num = 0;
			for (int i = 0; i < this._options.Count; i++)
			{
				if (this._options[i].decal == current)
				{
					num = i;
					break;
				}
			}
			this._canvasRoot.SetActive(true);
			this.SuspendBackgroundCanvases();
			this.SelectIndex(num, 0, false);
			if (this._openTransition != null)
			{
				base.StopCoroutine(this._openTransition);
			}
			this._openTransition = base.StartCoroutine(this.AnimateIn());
		}

		// Token: 0x06006F5C RID: 28508 RVA: 0x002CEEB4 File Offset: 0x002CD0B4
		public void HideImmediate()
		{
			if (this._openTransition != null)
			{
				base.StopCoroutine(this._openTransition);
			}
			if (this._previewTransition != null)
			{
				base.StopCoroutine(this._previewTransition);
			}
			this._openTransition = null;
			this._previewTransition = null;
			if (this._canvasRoot != null)
			{
				this._canvasRoot.SetActive(false);
			}
			this.RestoreBackgroundCanvases();
		}

		// Token: 0x06006F5D RID: 28509 RVA: 0x002CEF18 File Offset: 0x002CD118
		private void Update()
		{
			if (!this.IsOpen)
			{
				return;
			}
			if (GameplayInput.GetKeyDown(KeyCode.LeftArrow) || GameplayInput.GetKeyDown(KeyCode.A))
			{
				this.MoveSelection(-1);
				return;
			}
			if (GameplayInput.GetKeyDown(KeyCode.RightArrow) || GameplayInput.GetKeyDown(KeyCode.D))
			{
				this.MoveSelection(1);
				return;
			}
			if (GameplayInput.GetKeyDown(KeyCode.Return) || GameplayInput.GetKeyDown(KeyCode.KeypadEnter))
			{
				this.RequestApply();
			}
		}

		// Token: 0x06006F5E RID: 28510 RVA: 0x002CEF83 File Offset: 0x002CD183
		private void OnDestroy()
		{
			this.RestoreBackgroundCanvases();
			if (this._roundedSprite != null)
			{
				Object.Destroy(this._roundedSprite);
			}
			if (this._roundedTexture != null)
			{
				Object.Destroy(this._roundedTexture);
			}
		}

		// Token: 0x06006F5F RID: 28511 RVA: 0x002CEFBD File Offset: 0x002CD1BD
		private void OnDisable()
		{
			if (this._canvasRoot != null && this._canvasRoot.activeSelf)
			{
				this._canvasRoot.SetActive(false);
			}
			this.RestoreBackgroundCanvases();
		}

		// Token: 0x06006F60 RID: 28512 RVA: 0x002CEFEC File Offset: 0x002CD1EC
		private void RebuildOptions(IReadOnlyList<PencilBodyPaintCatalogSO.Option> source)
		{
			this._options.Clear();
			if (source == null)
			{
				return;
			}
			for (int i = 0; i < source.Count; i++)
			{
				PencilBodyPaintCatalogSO.Option option = source[i];
				if (option != null && option.decal != null)
				{
					this._options.Add(option);
				}
			}
		}

		// Token: 0x06006F61 RID: 28513 RVA: 0x002CF040 File Offset: 0x002CD240
		private void MoveSelection(int direction)
		{
			if (this._options.Count == 0)
			{
				return;
			}
			int num = (this._currentIndex + direction) % this._options.Count;
			if (num < 0)
			{
				num += this._options.Count;
			}
			this.SelectIndex(num, direction, true);
		}

		// Token: 0x06006F62 RID: 28514 RVA: 0x002CF08C File Offset: 0x002CD28C
		private void SelectIndex(int index, int direction, bool animate)
		{
			if (this._options.Count == 0)
			{
				this._currentIndex = -1;
				this._selected = null;
				this.RefreshSelectionVisuals();
				return;
			}
			this._currentIndex = Mathf.Clamp(index, 0, this._options.Count - 1);
			this._selected = this._options[this._currentIndex];
			this.RefreshSelectionVisuals();
			if (this._previewTransition != null)
			{
				base.StopCoroutine(this._previewTransition);
			}
			if (animate)
			{
				this._previewTransition = base.StartCoroutine(this.AnimatePreview(direction));
				return;
			}
			this._previewGroup.alpha = 1f;
			this._previewPanel.anchoredPosition = new Vector2(0f, -12f);
		}

		// Token: 0x06006F63 RID: 28515 RVA: 0x002CF148 File Offset: 0x002CD348
		private void RefreshSelectionVisuals()
		{
			bool flag = this._selected != null && this._selected.decal != null;
			if (this._portrait != null)
			{
				this._portrait.sprite = (flag ? this._selected.preview : null);
				this._portrait.enabled = this._portrait.sprite != null;
			}
			if (this._selectedName != null)
			{
				LocalizedGameText localizedGameText = LocalizedGameText.BindSource(this._selectedName, flag ? "timed_effects" : "pencil_vegetables", flag ? this._selected.DisplayName : "NO LOOK AVAILABLE", null);
				if (localizedGameText != null)
				{
					localizedGameText.FitTranslationToRectangle(false);
				}
			}
			if (this._counter != null)
			{
				this._counter.text = (flag ? ((this._currentIndex + 1).ToString("00") + "  /  " + this._options.Count.ToString("00")) : "00 / 00");
			}
			if (this._modeLabel != null)
			{
				LocalizedGameText localizedGameText2 = LocalizedGameText.BindSource(this._modeLabel, "pencil_vegetables", (flag && this._selected.decal.HasHeadTexture && !this._selected.decal.HasBodyTexture) ? "FACE LOOK" : "FULL BODY LOOK", null);
				if (localizedGameText2 != null)
				{
					localizedGameText2.FitTranslationToRectangle(false);
				}
			}
			if (this._dots != null)
			{
				this._dots.text = this.BuildDots();
			}
			if (this._applyButton != null)
			{
				this._applyButton.interactable = flag;
			}
			bool flag2 = this._options.Count > 1;
			if (this._previousButton != null)
			{
				this._previousButton.interactable = flag2;
			}
			if (this._nextButton != null)
			{
				this._nextButton.interactable = flag2;
			}
		}

		// Token: 0x06006F64 RID: 28516 RVA: 0x002CF338 File Offset: 0x002CD538
		private string BuildDots()
		{
			if (this._options.Count == 0)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder(this._options.Count * 22);
			for (int i = 0; i < this._options.Count; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append("   ");
				}
				stringBuilder.Append((i == this._currentIndex) ? "<color=#FF4F9C>●</color>" : "<color=#80657C>•</color>");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06006F65 RID: 28517 RVA: 0x002CF3B4 File Offset: 0x002CD5B4
		private void EnsureBuilt()
		{
			if (this._canvasRoot != null)
			{
				this.ApplyFontToExistingText();
				return;
			}
			PencilBodyPaintPickerUI.EnsureEventSystem();
			this._roundedSprite = PencilBodyPaintPickerUI.CreateRoundedSprite(out this._roundedTexture);
			this._canvasRoot = new GameObject("PencilBodyPaintCarousel", new Type[]
			{
				typeof(RectTransform),
				typeof(Canvas),
				typeof(CanvasScaler),
				typeof(GraphicRaycaster),
				typeof(CanvasGroup)
			});
			this._canvasRoot.layer = 5;
			this._canvasRoot.transform.SetParent(base.transform, false);
			Canvas component = this._canvasRoot.GetComponent<Canvas>();
			component.renderMode = RenderMode.ScreenSpaceOverlay;
			component.sortingOrder = 4500;
			GameplayHudVisibility.RegisterCanvas(component);
			CanvasScaler component2 = this._canvasRoot.GetComponent<CanvasScaler>();
			component2.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			component2.referenceResolution = new Vector2(1920f, 1080f);
			component2.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
			component2.matchWidthOrHeight = 0.5f;
			this._canvasGroup = this._canvasRoot.GetComponent<CanvasGroup>();
			RectTransform rectTransform = PencilBodyPaintPickerUI.CreateRect("Backdrop", this._canvasRoot.transform);
			PencilBodyPaintPickerUI.Stretch(rectTransform);
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = PencilBodyPaintPickerUI.BackdropColor;
			Button button = rectTransform.gameObject.AddComponent<Button>();
			PencilBodyPaintPickerUI.ConfigureButton(button, image, PencilBodyPaintPickerUI.BackdropColor, PencilBodyPaintPickerUI.BackdropColor, PencilBodyPaintPickerUI.BackdropColor);
			button.onClick.AddListener(new UnityAction(this.RequestCancel));
			this._content = PencilBodyPaintPickerUI.CreateRect("Content", rectTransform);
			RectTransform content = this._content;
			RectTransform content2 = this._content;
			RectTransform content3 = this._content;
			Vector2 vector = new Vector2(0.5f, 0.5f);
			content3.pivot = vector;
			content.anchorMin = (content2.anchorMax = vector);
			this._content.sizeDelta = new Vector2(1040f, 900f);
			Image image2 = this._content.gameObject.AddComponent<Image>();
			image2.color = Color.clear;
			image2.raycastTarget = true;
			RectTransform rectTransform2 = PencilBodyPaintPickerUI.CreateRect("InterfacePanel", this._content);
			vector = new Vector2(0.5f, 0.5f);
			rectTransform2.pivot = vector;
			rectTransform2.anchorMin = (rectTransform2.anchorMax = vector);
			rectTransform2.anchoredPosition = Vector2.zero;
			rectTransform2.sizeDelta = new Vector2(1024f, 884f);
			Image image3 = rectTransform2.gameObject.AddComponent<Image>();
			image3.sprite = this._roundedSprite;
			image3.type = Image.Type.Sliced;
			image3.color = new Color(0.045f, 0.022f, 0.06f, 0.82f);
			image3.raycastTarget = false;
			Outline outline = rectTransform2.gameObject.AddComponent<Outline>();
			outline.effectColor = new Color(PencilBodyPaintPickerUI.AccentColor.r, PencilBodyPaintPickerUI.AccentColor.g, PencilBodyPaintPickerUI.AccentColor.b, 0.24f);
			outline.effectDistance = new Vector2(1f, -1f);
			this.BuildHeader();
			this.BuildCarousel();
			this.BuildFooter();
			this._canvasRoot.SetActive(false);
		}

		// Token: 0x06006F66 RID: 28518 RVA: 0x002CF6C4 File Offset: 0x002CD8C4
		private void BuildHeader()
		{
			RectTransform rectTransform = PencilBodyPaintPickerUI.CreateRect("Accent", this._content);
			PencilBodyPaintPickerUI.SetTopLeft(rectTransform, new Vector2(18f, -8f), new Vector2(64f, 4f));
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.sprite = this._roundedSprite;
			image.type = Image.Type.Sliced;
			image.color = PencilBodyPaintPickerUI.AccentColor;
			RectTransform rectTransform2 = PencilBodyPaintPickerUI.CreateRect("PenIconFrame", this._content);
			PencilBodyPaintPickerUI.SetTopLeft(rectTransform2, new Vector2(18f, -38f), new Vector2(62f, 62f));
			Image image2 = rectTransform2.gameObject.AddComponent<Image>();
			image2.sprite = this._roundedSprite;
			image2.type = Image.Type.Sliced;
			image2.color = PencilBodyPaintPickerUI.CardColor;
			RectTransform rectTransform3 = PencilBodyPaintPickerUI.CreateRect("PenIcon", rectTransform2);
			PencilBodyPaintPickerUI.SetInset(rectTransform3, 8f);
			this._itemIcon = rectTransform3.gameObject.AddComponent<Image>();
			this._itemIcon.preserveAspect = true;
			this._itemIcon.raycastTarget = false;
			PencilBodyPaintPickerUI.SetTopLeft(this.CreateText("Kicker", this._content, "PENCIL  /  BODY PAINT", 16f, FontStyles.Bold, PencilBodyPaintPickerUI.AccentColor, TextAlignmentOptions.Left).rectTransform, new Vector2(98f, -34f), new Vector2(430f, 25f));
			PencilBodyPaintPickerUI.SetTopLeft(this.CreateText("Title", this._content, "CHOOSE A BODY PAINT", 42f, FontStyles.Bold, PencilBodyPaintPickerUI.MainTextColor, TextAlignmentOptions.Left).rectTransform, new Vector2(98f, -57f), new Vector2(650f, 56f));
			PencilBodyPaintPickerUI.SetTopLeft(this.CreateText("Subtitle", this._content, "Choose one look. Applying consumes 1 pen and lasts for 3 minutes.", 15f, FontStyles.Normal, PencilBodyPaintPickerUI.MainTextColor, TextAlignmentOptions.Left).rectTransform, new Vector2(98f, -111f), new Vector2(700f, 28f));
			TMP_Text tmp_Text = this.CreateText("EscapeHint", this._content, "ESC   CLOSE", 13f, FontStyles.Bold, PencilBodyPaintPickerUI.MainTextColor, TextAlignmentOptions.Right);
			RectTransform rectTransform4 = tmp_Text.rectTransform;
			RectTransform rectTransform5 = tmp_Text.rectTransform;
			RectTransform rectTransform6 = tmp_Text.rectTransform;
			Vector2 vector = new Vector2(1f, 1f);
			rectTransform6.pivot = vector;
			rectTransform4.anchorMin = (rectTransform5.anchorMax = vector);
			tmp_Text.rectTransform.anchoredPosition = new Vector2(-26f, -40f);
			tmp_Text.rectTransform.sizeDelta = new Vector2(150f, 28f);
			this.CreateLine("HeaderLine", this._content, new Vector2(18f, -148f), new Vector2(-18f, -146f), true);
		}

		// Token: 0x06006F67 RID: 28519 RVA: 0x002CF978 File Offset: 0x002CDB78
		private void BuildCarousel()
		{
			RectTransform rectTransform = PencilBodyPaintPickerUI.CreateRect("PreviewShadow", this._content);
			Vector2 vector = new Vector2(0.5f, 0.5f);
			rectTransform.pivot = vector;
			rectTransform.anchorMin = (rectTransform.anchorMax = vector);
			rectTransform.anchoredPosition = new Vector2(0f, -20f);
			rectTransform.sizeDelta = new Vector2(692f, 622f);
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.sprite = this._roundedSprite;
			image.type = Image.Type.Sliced;
			image.color = new Color(0f, 0f, 0f, 0.34f);
			image.raycastTarget = false;
			this._previewPanel = PencilBodyPaintPickerUI.CreateRect("LargePreview", this._content);
			RectTransform previewPanel = this._previewPanel;
			RectTransform previewPanel2 = this._previewPanel;
			RectTransform previewPanel3 = this._previewPanel;
			vector = new Vector2(0.5f, 0.5f);
			previewPanel3.pivot = vector;
			previewPanel.anchorMin = (previewPanel2.anchorMax = vector);
			this._previewPanel.anchoredPosition = new Vector2(0f, -12f);
			this._previewPanel.sizeDelta = new Vector2(680f, 610f);
			Image image2 = this._previewPanel.gameObject.AddComponent<Image>();
			image2.sprite = this._roundedSprite;
			image2.type = Image.Type.Sliced;
			image2.color = PencilBodyPaintPickerUI.CardColor;
			Outline outline = this._previewPanel.gameObject.AddComponent<Outline>();
			outline.effectColor = new Color(PencilBodyPaintPickerUI.AccentColor.r, PencilBodyPaintPickerUI.AccentColor.g, PencilBodyPaintPickerUI.AccentColor.b, 0.32f);
			outline.effectDistance = new Vector2(1f, -1f);
			this._previewGroup = this._previewPanel.gameObject.AddComponent<CanvasGroup>();
			RectTransform rectTransform2 = PencilBodyPaintPickerUI.CreateRect("PortraitFrame", this._previewPanel);
			PencilBodyPaintPickerUI.SetInset(rectTransform2, 12f);
			Image image3 = rectTransform2.gameObject.AddComponent<Image>();
			image3.sprite = this._roundedSprite;
			image3.type = Image.Type.Sliced;
			image3.color = PencilBodyPaintPickerUI.PreviewColor;
			image3.raycastTarget = false;
			RectTransform rectTransform3 = PencilBodyPaintPickerUI.CreateRect("CielPortrait", rectTransform2);
			PencilBodyPaintPickerUI.SetInset(rectTransform3, 8f);
			this._portrait = rectTransform3.gameObject.AddComponent<Image>();
			this._portrait.preserveAspect = true;
			this._portrait.raycastTarget = false;
			this._modeLabel = this.CreateText("Mode", this._previewPanel, "FULL BODY LOOK", 12f, FontStyles.Bold, PencilBodyPaintPickerUI.AccentColor, TextAlignmentOptions.Left);
			PencilBodyPaintPickerUI.SetTopLeft(this._modeLabel.rectTransform, new Vector2(24f, -20f), new Vector2(190f, 24f));
			this._counter = this.CreateText("Counter", this._previewPanel, "01 / 10", 14f, FontStyles.Bold, PencilBodyPaintPickerUI.MainTextColor, TextAlignmentOptions.Right);
			RectTransform rectTransform4 = this._counter.rectTransform;
			RectTransform rectTransform5 = this._counter.rectTransform;
			RectTransform rectTransform6 = this._counter.rectTransform;
			vector = new Vector2(1f, 1f);
			rectTransform6.pivot = vector;
			rectTransform4.anchorMin = (rectTransform5.anchorMax = vector);
			this._counter.rectTransform.anchoredPosition = new Vector2(-24f, -20f);
			this._counter.rectTransform.sizeDelta = new Vector2(130f, 24f);
			this._previousButton = this.CreateArrowButton("Previous", this._content, -1, new Vector2(-382f, -12f));
			this._previousButton.onClick.AddListener(delegate
			{
				this.MoveSelection(-1);
			});
			this._nextButton = this.CreateArrowButton("Next", this._content, 1, new Vector2(382f, -12f));
			this._nextButton.onClick.AddListener(delegate
			{
				this.MoveSelection(1);
			});
			this._dots = this.CreateText("PageDots", this._content, string.Empty, 15f, FontStyles.Bold, PencilBodyPaintPickerUI.MutedTextColor, TextAlignmentOptions.Center);
			this._dots.richText = true;
			RectTransform rectTransform7 = this._dots.rectTransform;
			RectTransform rectTransform8 = this._dots.rectTransform;
			RectTransform rectTransform9 = this._dots.rectTransform;
			vector = new Vector2(0.5f, 0f);
			rectTransform9.pivot = vector;
			rectTransform7.anchorMin = (rectTransform8.anchorMax = vector);
			this._dots.rectTransform.anchoredPosition = new Vector2(0f, 112f);
			this._dots.rectTransform.sizeDelta = new Vector2(600f, 30f);
		}

		// Token: 0x06006F68 RID: 28520 RVA: 0x002CFE18 File Offset: 0x002CE018
		private void BuildFooter()
		{
			this.CreateLine("FooterLine", this._content, new Vector2(18f, 96f), new Vector2(-18f, 98f), false);
			PencilBodyPaintPickerUI.SetBottomLeft(this.CreateText("SelectedPrefix", this._content, "SELECTED LOOK", 11f, FontStyles.Bold, PencilBodyPaintPickerUI.AccentColor, TextAlignmentOptions.Left).rectTransform, new Vector2(18f, 57f), new Vector2(190f, 20f));
			this._selectedName = this.CreateText("SelectedName", this._content, "-", 22f, FontStyles.Bold, PencilBodyPaintPickerUI.MainTextColor, TextAlignmentOptions.Left);
			PencilBodyPaintPickerUI.SetBottomLeft(this._selectedName.rectTransform, new Vector2(18f, 20f), new Vector2(460f, 38f));
			TMP_Text tmp_Text = this.CreateText("BrowseHint", this._content, "A / D   OR   ← / →   BROWSE", 12f, FontStyles.Bold, PencilBodyPaintPickerUI.MainTextColor, TextAlignmentOptions.Center);
			RectTransform rectTransform = tmp_Text.rectTransform;
			RectTransform rectTransform2 = tmp_Text.rectTransform;
			RectTransform rectTransform3 = tmp_Text.rectTransform;
			Vector2 vector = new Vector2(0.5f, 0f);
			rectTransform3.pivot = vector;
			rectTransform.anchorMin = (rectTransform2.anchorMax = vector);
			tmp_Text.rectTransform.anchoredPosition = new Vector2(45f, 25f);
			tmp_Text.rectTransform.sizeDelta = new Vector2(280f, 30f);
			this._applyButton = this.CreateActionButton("Apply", this._content, "APPLY  •  USE 1 PEN", PencilBodyPaintPickerUI.AccentColor, new Vector2(-18f, 22f), new Vector2(190f, 44f));
			this._applyButton.onClick.AddListener(new UnityAction(this.RequestApply));
			this.CreateActionButton("Back", this._content, "CANCEL", PencilBodyPaintPickerUI.CardColor, new Vector2(-220f, 22f), new Vector2(98f, 44f)).onClick.AddListener(new UnityAction(this.RequestCancel));
		}

		// Token: 0x06006F69 RID: 28521 RVA: 0x002D003C File Offset: 0x002CE23C
		private Button CreateArrowButton(string name, Transform parent, int direction, Vector2 position)
		{
			RectTransform rectTransform = PencilBodyPaintPickerUI.CreateRect(name, parent);
			RectTransform rectTransform2 = rectTransform;
			RectTransform rectTransform3 = rectTransform;
			RectTransform rectTransform4 = rectTransform;
			Vector2 vector = new Vector2(0.5f, 0.5f);
			rectTransform4.pivot = vector;
			rectTransform2.anchorMin = (rectTransform3.anchorMax = vector);
			rectTransform.anchoredPosition = position;
			rectTransform.sizeDelta = new Vector2(64f, 64f);
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.sprite = this._roundedSprite;
			image.type = Image.Type.Sliced;
			image.color = PencilBodyPaintPickerUI.CardColor;
			Outline outline = rectTransform.gameObject.AddComponent<Outline>();
			outline.effectColor = new Color(PencilBodyPaintPickerUI.AccentColor.r, PencilBodyPaintPickerUI.AccentColor.g, PencilBodyPaintPickerUI.AccentColor.b, 0.2f);
			outline.effectDistance = new Vector2(1f, -1f);
			Button button = rectTransform.gameObject.AddComponent<Button>();
			PencilBodyPaintPickerUI.ConfigureButton(button, image, PencilBodyPaintPickerUI.CardColor, PencilBodyPaintPickerUI.CardHoverColor, PencilBodyPaintPickerUI.CardPressedColor);
			this.CreateChevronStroke("UpperStroke", rectTransform, direction, 7.5f, true);
			this.CreateChevronStroke("LowerStroke", rectTransform, direction, -7.5f, false);
			return button;
		}

		// Token: 0x06006F6A RID: 28522 RVA: 0x002D0154 File Offset: 0x002CE354
		private void CreateChevronStroke(string name, Transform parent, int direction, float y, bool upper)
		{
			RectTransform rectTransform = PencilBodyPaintPickerUI.CreateRect(name, parent);
			Vector2 vector = new Vector2(0.5f, 0.5f);
			rectTransform.pivot = vector;
			rectTransform.anchorMin = (rectTransform.anchorMax = vector);
			rectTransform.anchoredPosition = new Vector2(0f, y);
			rectTransform.sizeDelta = new Vector2(4f, 24f);
			float num = (upper ? ((float)direction * 45f) : ((float)direction * -45f));
			rectTransform.localRotation = Quaternion.Euler(0f, 0f, num);
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.sprite = this._roundedSprite;
			image.type = Image.Type.Sliced;
			image.color = PencilBodyPaintPickerUI.MainTextColor;
			image.raycastTarget = false;
		}

		// Token: 0x06006F6B RID: 28523 RVA: 0x002D0214 File Offset: 0x002CE414
		private void SuspendBackgroundCanvases()
		{
			if (this._suspendedCanvases.Count > 0 || this._canvasRoot == null)
			{
				return;
			}
			Canvas component = this._canvasRoot.GetComponent<Canvas>();
			foreach (Canvas canvas in Object.FindObjectsOfType<Canvas>(true))
			{
				if (!(canvas == null) && !(canvas == component) && canvas.enabled && canvas.gameObject.activeInHierarchy && !canvas.transform.IsChildOf(this._canvasRoot.transform))
				{
					this._suspendedCanvases.Add(canvas);
				}
			}
			for (int j = 0; j < this._suspendedCanvases.Count; j++)
			{
				if (this._suspendedCanvases[j] != null)
				{
					this._suspendedCanvases[j].enabled = false;
				}
			}
		}

		// Token: 0x06006F6C RID: 28524 RVA: 0x002D02F0 File Offset: 0x002CE4F0
		private void RestoreBackgroundCanvases()
		{
			for (int i = 0; i < this._suspendedCanvases.Count; i++)
			{
				if (this._suspendedCanvases[i] != null)
				{
					this._suspendedCanvases[i].enabled = true;
				}
			}
			this._suspendedCanvases.Clear();
		}

		// Token: 0x06006F6D RID: 28525 RVA: 0x002D0344 File Offset: 0x002CE544
		private Button CreateActionButton(string name, Transform parent, string label, Color color, Vector2 position, Vector2 size)
		{
			RectTransform rectTransform = PencilBodyPaintPickerUI.CreateRect(name, parent);
			RectTransform rectTransform2 = rectTransform;
			RectTransform rectTransform3 = rectTransform;
			RectTransform rectTransform4 = rectTransform;
			Vector2 vector = new Vector2(1f, 0f);
			rectTransform4.pivot = vector;
			rectTransform2.anchorMin = (rectTransform3.anchorMax = vector);
			rectTransform.anchoredPosition = position;
			rectTransform.sizeDelta = size;
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.sprite = this._roundedSprite;
			image.type = Image.Type.Sliced;
			image.color = color;
			Button button = rectTransform.gameObject.AddComponent<Button>();
			PencilBodyPaintPickerUI.ConfigureButton(button, image, color, Color.Lerp(color, Color.white, 0.12f), Color.Lerp(color, Color.black, 0.16f));
			PencilBodyPaintPickerUI.Stretch(this.CreateText("Label", rectTransform, label, 13f, FontStyles.Bold, Color.white, TextAlignmentOptions.Center).rectTransform);
			return button;
		}

		// Token: 0x06006F6E RID: 28526 RVA: 0x002D0415 File Offset: 0x002CE615
		private IEnumerator AnimateIn()
		{
			float elapsed = 0f;
			this._canvasGroup.alpha = 0f;
			this._content.anchoredPosition = new Vector2(0f, -16f);
			while (elapsed < 0.2f)
			{
				elapsed += Time.unscaledDeltaTime;
				float num = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / 0.2f));
				this._canvasGroup.alpha = num;
				this._content.anchoredPosition = new Vector2(0f, Mathf.Lerp(-16f, 0f, num));
				yield return null;
			}
			this._canvasGroup.alpha = 1f;
			this._content.anchoredPosition = Vector2.zero;
			this._openTransition = null;
			yield break;
		}

		// Token: 0x06006F6F RID: 28527 RVA: 0x002D0424 File Offset: 0x002CE624
		private IEnumerator AnimatePreview(int direction)
		{
			float elapsed = 0f;
			float startX = Mathf.Sign((float)direction) * 24f;
			this._previewGroup.alpha = 0.2f;
			while (elapsed < 0.16f)
			{
				elapsed += Time.unscaledDeltaTime;
				float num = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / 0.16f));
				this._previewGroup.alpha = Mathf.Lerp(0.2f, 1f, num);
				this._previewPanel.anchoredPosition = new Vector2(Mathf.Lerp(startX, 0f, num), -12f);
				yield return null;
			}
			this._previewGroup.alpha = 1f;
			this._previewPanel.anchoredPosition = new Vector2(0f, -12f);
			this._previewTransition = null;
			yield break;
		}

		// Token: 0x06006F70 RID: 28528 RVA: 0x002D043A File Offset: 0x002CE63A
		private void RequestApply()
		{
			if (this._selected != null && this._selected.decal != null)
			{
				Action<SkinDecal> applyRequested = this.ApplyRequested;
				if (applyRequested == null)
				{
					return;
				}
				applyRequested(this._selected.decal);
			}
		}

		// Token: 0x06006F71 RID: 28529 RVA: 0x002D0472 File Offset: 0x002CE672
		private void RequestCancel()
		{
			Action cancelRequested = this.CancelRequested;
			if (cancelRequested == null)
			{
				return;
			}
			cancelRequested();
		}

		// Token: 0x06006F72 RID: 28530 RVA: 0x002D0484 File Offset: 0x002CE684
		private void CreateLine(string name, Transform parent, Vector2 minOffset, Vector2 maxOffset, bool top)
		{
			RectTransform rectTransform = PencilBodyPaintPickerUI.CreateRect(name, parent);
			rectTransform.anchorMin = new Vector2(0f, top ? 1f : 0f);
			rectTransform.anchorMax = new Vector2(1f, top ? 1f : 0f);
			rectTransform.offsetMin = minOffset;
			rectTransform.offsetMax = maxOffset;
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = new Color(PencilBodyPaintPickerUI.MutedTextColor.r, PencilBodyPaintPickerUI.MutedTextColor.g, PencilBodyPaintPickerUI.MutedTextColor.b, 0.2f);
			image.raycastTarget = false;
		}

		// Token: 0x06006F73 RID: 28531 RVA: 0x002D0528 File Offset: 0x002CE728
		private static void ConfigureButton(Button button, Graphic target, Color normal, Color highlighted, Color pressed)
		{
			button.targetGraphic = target;
			button.transition = Selectable.Transition.ColorTint;
			button.navigation = new Navigation
			{
				mode = Navigation.Mode.None
			};
			ColorBlock colors = button.colors;
			colors.normalColor = normal;
			colors.highlightedColor = highlighted;
			colors.selectedColor = highlighted;
			colors.pressedColor = pressed;
			colors.disabledColor = new Color(normal.r, normal.g, normal.b, 0.35f);
			colors.colorMultiplier = 1f;
			colors.fadeDuration = 0.08f;
			button.colors = colors;
		}

		// Token: 0x06006F74 RID: 28532 RVA: 0x002D05C4 File Offset: 0x002CE7C4
		private TMP_Text CreateText(string name, Transform parent, string value, float size, FontStyles style, Color color, TextAlignmentOptions alignment)
		{
			TextMeshProUGUI textMeshProUGUI = PencilBodyPaintPickerUI.CreateRect(name, parent).gameObject.AddComponent<TextMeshProUGUI>();
			if (this._font != null)
			{
				textMeshProUGUI.font = this._font;
			}
			textMeshProUGUI.text = value;
			textMeshProUGUI.fontSize = size;
			textMeshProUGUI.fontStyle = style;
			textMeshProUGUI.color = color;
			textMeshProUGUI.alignment = alignment;
			textMeshProUGUI.raycastTarget = false;
			textMeshProUGUI.enableWordWrapping = false;
			textMeshProUGUI.overflowMode = TextOverflowModes.Ellipsis;
			if (name == "Counter" || name == "PageDots" || name == "SelectedName")
			{
				LocalizedGameText.BindFontOnly(textMeshProUGUI);
			}
			else
			{
				LocalizedGameText localizedGameText = LocalizedGameText.BindSource(textMeshProUGUI, "pencil_vegetables", value, null);
				if (localizedGameText != null)
				{
					localizedGameText.FitTranslationToRectangle(false);
				}
			}
			return textMeshProUGUI;
		}

		// Token: 0x06006F75 RID: 28533 RVA: 0x002D0684 File Offset: 0x002CE884
		private void ApplyFontToExistingText()
		{
			if (this._font == null || this._canvasRoot == null)
			{
				return;
			}
			TMP_Text[] componentsInChildren = this._canvasRoot.GetComponentsInChildren<TMP_Text>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				LocalizedGameText component = componentsInChildren[i].GetComponent<LocalizedGameText>();
				if (component != null)
				{
					component.SetSourceFont(this._font);
				}
				else
				{
					componentsInChildren[i].font = this._font;
				}
			}
		}

		// Token: 0x06006F76 RID: 28534 RVA: 0x002D06F7 File Offset: 0x002CE8F7
		private static RectTransform CreateRect(string name, Transform parent)
		{
			RectTransform component = new GameObject(name, new Type[] { typeof(RectTransform) })
			{
				layer = 5
			}.GetComponent<RectTransform>();
			component.SetParent(parent, false);
			component.localScale = Vector3.one;
			return component;
		}

		// Token: 0x06006F77 RID: 28535 RVA: 0x001A38AD File Offset: 0x001A1AAD
		private static void Stretch(RectTransform rect)
		{
			rect.anchorMin = Vector2.zero;
			rect.anchorMax = Vector2.one;
			rect.offsetMin = Vector2.zero;
			rect.offsetMax = Vector2.zero;
		}

		// Token: 0x06006F78 RID: 28536 RVA: 0x002D0731 File Offset: 0x002CE931
		private static void SetInset(RectTransform rect, float inset)
		{
			rect.anchorMin = Vector2.zero;
			rect.anchorMax = Vector2.one;
			rect.offsetMin = new Vector2(inset, inset);
			rect.offsetMax = new Vector2(-inset, -inset);
		}

		// Token: 0x06006F79 RID: 28537 RVA: 0x002D0768 File Offset: 0x002CE968
		private static void SetTopLeft(RectTransform rect, Vector2 position, Vector2 size)
		{
			Vector2 vector = new Vector2(0f, 1f);
			rect.anchorMax = vector;
			rect.anchorMin = vector;
			rect.pivot = new Vector2(0f, 1f);
			rect.anchoredPosition = position;
			rect.sizeDelta = size;
		}

		// Token: 0x06006F7A RID: 28538 RVA: 0x002D07B8 File Offset: 0x002CE9B8
		private static void SetBottomLeft(RectTransform rect, Vector2 position, Vector2 size)
		{
			rect.anchorMin = (rect.anchorMax = Vector2.zero);
			rect.pivot = Vector2.zero;
			rect.anchoredPosition = position;
			rect.sizeDelta = size;
		}

		// Token: 0x06006F7B RID: 28539 RVA: 0x002D07F4 File Offset: 0x002CE9F4
		private static Sprite CreateRoundedSprite(out Texture2D texture)
		{
			texture = new Texture2D(48, 48, TextureFormat.RGBA32, false, true)
			{
				name = "PencilUI_SettingsRoundedRect",
				filterMode = FilterMode.Bilinear,
				wrapMode = TextureWrapMode.Clamp,
				hideFlags = HideFlags.DontSave
			};
			Color32[] array = new Color32[2304];
			for (int i = 0; i < 48; i++)
			{
				for (int j = 0; j < 48; j++)
				{
					float num = Mathf.Clamp((float)j, 13f, 34f);
					float num2 = Mathf.Clamp((float)i, 13f, 34f);
					float num3 = Vector2.Distance(new Vector2((float)j, (float)i), new Vector2(num, num2));
					byte b = (byte)Mathf.RoundToInt(255f * Mathf.Clamp01(13.5f - num3));
					array[i * 48 + j] = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, b);
				}
			}
			texture.SetPixels32(array);
			texture.Apply(false, true);
			Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, 48f, 48f), new Vector2(0.5f, 0.5f), 100f, 0U, SpriteMeshType.FullRect, new Vector4(14f, 14f, 14f, 14f));
			sprite.name = "PencilUI_SettingsRoundedRect";
			sprite.hideFlags = HideFlags.DontSave;
			return sprite;
		}

		// Token: 0x06006F7C RID: 28540 RVA: 0x002D094C File Offset: 0x002CEB4C
		private static void EnsureEventSystem()
		{
			if (EventSystem.current != null || Object.FindObjectOfType<EventSystem>() != null)
			{
				return;
			}
			Object.DontDestroyOnLoad(new GameObject("EventSystem", new Type[]
			{
				typeof(EventSystem),
				typeof(StandaloneInputModule)
			}));
		}

		// Token: 0x04007243 RID: 29251
		private const string LocalizationScope = "pencil_vegetables";

		// Token: 0x04007244 RID: 29252
		private static readonly Color BackdropColor = new Color(0.025f, 0.012f, 0.03f, 0.82f);

		// Token: 0x04007245 RID: 29253
		private static readonly Color CardColor = new Color(0.165f, 0.118f, 0.2f, 0.96f);

		// Token: 0x04007246 RID: 29254
		private static readonly Color CardHoverColor = new Color(0.243f, 0.165f, 0.298f, 1f);

		// Token: 0x04007247 RID: 29255
		private static readonly Color CardPressedColor = new Color(0.41f, 0.349f, 0.452f, 1f);

		// Token: 0x04007248 RID: 29256
		private static readonly Color PreviewColor = new Color(0.055f, 0.025f, 0.065f, 1f);

		// Token: 0x04007249 RID: 29257
		private static readonly Color AccentColor = new Color(1f, 0.31f, 0.61f, 1f);

		// Token: 0x0400724A RID: 29258
		private static readonly Color MainTextColor = new Color(1f, 0.97f, 1f, 1f);

		// Token: 0x0400724B RID: 29259
		private static readonly Color MutedTextColor = new Color(0.78f, 0.67f, 0.76f, 1f);

		// Token: 0x0400724E RID: 29262
		private readonly List<PencilBodyPaintCatalogSO.Option> _options = new List<PencilBodyPaintCatalogSO.Option>(10);

		// Token: 0x0400724F RID: 29263
		private readonly List<Canvas> _suspendedCanvases = new List<Canvas>(16);

		// Token: 0x04007250 RID: 29264
		private GameObject _canvasRoot;

		// Token: 0x04007251 RID: 29265
		private CanvasGroup _canvasGroup;

		// Token: 0x04007252 RID: 29266
		private RectTransform _content;

		// Token: 0x04007253 RID: 29267
		private RectTransform _previewPanel;

		// Token: 0x04007254 RID: 29268
		private CanvasGroup _previewGroup;

		// Token: 0x04007255 RID: 29269
		private Image _itemIcon;

		// Token: 0x04007256 RID: 29270
		private Image _portrait;

		// Token: 0x04007257 RID: 29271
		private TMP_Text _selectedName;

		// Token: 0x04007258 RID: 29272
		private TMP_Text _counter;

		// Token: 0x04007259 RID: 29273
		private TMP_Text _dots;

		// Token: 0x0400725A RID: 29274
		private TMP_Text _modeLabel;

		// Token: 0x0400725B RID: 29275
		private Button _previousButton;

		// Token: 0x0400725C RID: 29276
		private Button _nextButton;

		// Token: 0x0400725D RID: 29277
		private Button _applyButton;

		// Token: 0x0400725E RID: 29278
		private TMP_FontAsset _font;

		// Token: 0x0400725F RID: 29279
		private Sprite _roundedSprite;

		// Token: 0x04007260 RID: 29280
		private Texture2D _roundedTexture;

		// Token: 0x04007261 RID: 29281
		private Coroutine _openTransition;

		// Token: 0x04007262 RID: 29282
		private Coroutine _previewTransition;

		// Token: 0x04007263 RID: 29283
		private int _currentIndex = -1;

		// Token: 0x04007264 RID: 29284
		private PencilBodyPaintCatalogSO.Option _selected;
	}
}
