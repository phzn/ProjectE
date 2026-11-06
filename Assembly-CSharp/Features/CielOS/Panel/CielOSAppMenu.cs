using System;
using System.Collections.Generic;
using Features.CielOS.UI;
using Features.CielOS.WindowManager;
using Features.Localization;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.CielOS.Panel
{
	// Token: 0x020013D4 RID: 5076
	public class CielOSAppMenu : MonoBehaviour
	{
		// Token: 0x170018D6 RID: 6358
		// (get) Token: 0x06009CB8 RID: 40120 RVA: 0x004047BF File Offset: 0x004029BF
		public bool IsOpen
		{
			get
			{
				return this.root != null && this.root.activeSelf;
			}
		}

		// Token: 0x06009CB9 RID: 40121 RVA: 0x004047DC File Offset: 0x004029DC
		private void Awake()
		{
			if (this.root != null)
			{
				this.root.SetActive(false);
			}
		}

		// Token: 0x06009CBA RID: 40122 RVA: 0x004047F8 File Offset: 0x004029F8
		private void OnEnable()
		{
			GameLocalization.LocaleChanged += this.RefreshLocalizedFilter;
		}

		// Token: 0x06009CBB RID: 40123 RVA: 0x0040480B File Offset: 0x00402A0B
		private void OnDisable()
		{
			GameLocalization.LocaleChanged -= this.RefreshLocalizedFilter;
		}

		// Token: 0x06009CBC RID: 40124 RVA: 0x0040481E File Offset: 0x00402A1E
		private void RefreshLocalizedFilter()
		{
			if (this._built)
			{
				this.ApplyFilter();
			}
		}

		// Token: 0x06009CBD RID: 40125 RVA: 0x00404830 File Offset: 0x00402A30
		private void OnValidate()
		{
			this.preferredMenuSize.x = Mathf.Max(180f, this.preferredMenuSize.x);
			this.preferredMenuSize.y = Mathf.Max(240f, this.preferredMenuSize.y);
			this.edgePadding = Mathf.Clamp(this.edgePadding, 4f, 24f);
		}

		// Token: 0x06009CBE RID: 40126 RVA: 0x00404898 File Offset: 0x00402A98
		private void OnDestroy()
		{
			if (this.searchField != null)
			{
				this.searchField.onValueChanged.RemoveListener(new UnityAction<string>(this.OnSearchChanged));
			}
		}

		// Token: 0x06009CBF RID: 40127 RVA: 0x004048C4 File Offset: 0x00402AC4
		private void LateUpdate()
		{
			if (!this._built || !this.IsOpen)
			{
				return;
			}
			if (this._boundsRect != null && this._boundsRect.rect != this._lastBoundsRect)
			{
				this.RefreshLayout(false);
			}
			this.KeepSelectionVisible();
		}

		// Token: 0x06009CC0 RID: 40128 RVA: 0x00404915 File Offset: 0x00402B15
		public void Toggle()
		{
			if (this.IsOpen)
			{
				this.Close();
				return;
			}
			this.Open();
		}

		// Token: 0x06009CC1 RID: 40129 RVA: 0x0040492C File Offset: 0x00402B2C
		public void Open()
		{
			this.EnsureBuilt();
			if (!this._built)
			{
				return;
			}
			ControllerNavigationScope.Configure(this.root, new Action(this.Close), (this.searchField != null) ? this.searchField.gameObject : null, 90, false);
			this.root.SetActive(true);
			this.root.transform.SetAsLastSibling();
			this._query = "";
			this._activeCategory = "All";
			if (this.searchField != null)
			{
				this.searchField.SetTextWithoutNotify("");
			}
			this.RefreshAvailability();
			this.HighlightCategory();
			if (this.searchField != null)
			{
				this.searchField.ActivateInputField();
			}
		}

		// Token: 0x06009CC2 RID: 40130 RVA: 0x004049F4 File Offset: 0x00402BF4
		public void Close()
		{
			if (this._scrollRect != null)
			{
				this._scrollRect.StopMovement();
			}
			if (this.root != null)
			{
				this.root.SetActive(false);
			}
			this._lastSelection = null;
		}

		// Token: 0x06009CC3 RID: 40131 RVA: 0x00404A30 File Offset: 0x00402C30
		public void RefreshAvailability()
		{
			if (!this._built || this.catalog == null)
			{
				return;
			}
			List<string> list = new List<string>();
			using (IEnumerator<CielOSAppDefinition> enumerator = this.catalog.GetMenuApps().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CielOSAppDefinition app = enumerator.Current;
					string text = CielOSAppIcons.CategoryForApp(app.appId);
					if (!list.Contains(text))
					{
						list.Add(text);
					}
					if (!this._entries.Exists((CielOSAppMenuEntry entry) => entry != null && entry.AppId == app.appId))
					{
						this.AddEntry(app);
					}
				}
			}
			if (string.Join("|", this._categories) != string.Join("|", list))
			{
				this._categories.Clear();
				this._categories.AddRange(list);
				if (!this._categories.Contains(this._activeCategory))
				{
					this._activeCategory = "All";
				}
				this.BuildCategoryRail();
			}
			this.ApplyFilter();
		}

		// Token: 0x06009CC4 RID: 40132 RVA: 0x00404B4C File Offset: 0x00402D4C
		private void EnsureBuilt()
		{
			if (this._built)
			{
				return;
			}
			if (this.catalog == null || this.root == null || !(this.root.transform is RectTransform) || this.entryParent == null || this.entryPrefab == null)
			{
				Debug.LogWarning("[CielOSAppMenu] Missing catalog, popup RectTransform, entryParent, or entryPrefab.", this);
				return;
			}
			this.BuildChrome();
			this._built = true;
			this.RefreshAvailability();
		}

		// Token: 0x06009CC5 RID: 40133 RVA: 0x00404BCC File Offset: 0x00402DCC
		private void AddEntry(CielOSAppDefinition app)
		{
			CielOSAppMenuEntry cielOSAppMenuEntry = Object.Instantiate<CielOSAppMenuEntry>(this.entryPrefab, this.entryParent);
			cielOSAppMenuEntry.Bind(app.appId, app.displayName, app.icon, new Action<string>(this.OnEntryClicked), !app.IsPlatformSupported);
			LayoutElement layoutElement = cielOSAppMenuEntry.GetComponent<LayoutElement>();
			if (layoutElement == null)
			{
				layoutElement = cielOSAppMenuEntry.gameObject.AddComponent<LayoutElement>();
			}
			layoutElement.minHeight = (layoutElement.preferredHeight = 40f);
			layoutElement.flexibleHeight = 0f;
			foreach (TMP_Text tmp_Text in cielOSAppMenuEntry.GetComponentsInChildren<TMP_Text>(true))
			{
				tmp_Text.enableWordWrapping = false;
				tmp_Text.overflowMode = TextOverflowModes.Ellipsis;
			}
			this._entries.Add(cielOSAppMenuEntry);
		}

		// Token: 0x06009CC6 RID: 40134 RVA: 0x00404C8A File Offset: 0x00402E8A
		private void OnSearchChanged(string value)
		{
			this._query = (value ?? "").Trim();
			this.ApplyFilter();
		}

		// Token: 0x06009CC7 RID: 40135 RVA: 0x00404CA7 File Offset: 0x00402EA7
		private void OnCategorySelected(string category)
		{
			this._activeCategory = category;
			this.HighlightCategory();
			this.ApplyFilter();
		}

		// Token: 0x06009CC8 RID: 40136 RVA: 0x00404CBC File Offset: 0x00402EBC
		private void HighlightCategory()
		{
			CielOSTheme active = CielOSTheme.Active;
			Color color = ((active != null) ? active.accent : new Color(0.66f, 0.38f, 0.79f, 1f));
			Color color2 = ((active != null) ? active.controlBackground : new Color(0.18f, 0.14f, 0.23f, 1f));
			foreach (Image image in this._categoryChips)
			{
				if (image != null)
				{
					image.color = ((image.name == this._activeCategory) ? color : color2);
				}
			}
		}

		// Token: 0x06009CC9 RID: 40137 RVA: 0x00404D90 File Offset: 0x00402F90
		private void ApplyFilter()
		{
			this._visibleEntryCount = 0;
			foreach (CielOSAppMenuEntry cielOSAppMenuEntry in this._entries)
			{
				if (!(cielOSAppMenuEntry == null))
				{
					CielOSAppDefinition cielOSAppDefinition = this.catalog.FindById(cielOSAppMenuEntry.AppId);
					string text = ((cielOSAppDefinition != null) ? cielOSAppDefinition.displayName : cielOSAppMenuEntry.AppId);
					string source = GameLocalization.GetSource("cielos_shell", text);
					bool flag = string.IsNullOrEmpty(this._query) || (text != null && text.IndexOf(this._query, StringComparison.OrdinalIgnoreCase) >= 0) || (source != null && source.IndexOf(this._query, StringComparison.OrdinalIgnoreCase) >= 0);
					bool flag2 = this._activeCategory == "All" || CielOSAppIcons.CategoryForApp(cielOSAppMenuEntry.AppId) == this._activeCategory;
					bool flag3 = cielOSAppDefinition != null && !cielOSAppDefinition.hidden && cielOSAppDefinition.IsVisible && cielOSAppDefinition.showInAppMenu && flag && flag2;
					cielOSAppMenuEntry.gameObject.SetActive(flag3);
					if (flag3)
					{
						this._visibleEntryCount++;
					}
				}
			}
			if (this._emptyState != null)
			{
				this._emptyState.gameObject.SetActive(this._visibleEntryCount == 0);
			}
			this.RefreshLayout(true);
		}

		// Token: 0x06009CCA RID: 40138 RVA: 0x00404F1C File Offset: 0x0040311C
		private void KeepSelectionVisible()
		{
			GameObject gameObject = ((EventSystem.current != null) ? EventSystem.current.currentSelectedGameObject : null);
			if (gameObject == this._lastSelection)
			{
				return;
			}
			this._lastSelection = gameObject;
			if (!(gameObject == null) && gameObject.transform.IsChildOf(this.entryParent))
			{
				RectTransform rectTransform = gameObject.transform as RectTransform;
				if (rectTransform != null)
				{
					Bounds bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(this._viewport, rectTransform);
					float num = ((bounds.min.y < this._viewport.rect.yMin) ? (this._viewport.rect.yMin - bounds.min.y) : ((bounds.max.y > this._viewport.rect.yMax) ? (this._viewport.rect.yMax - bounds.max.y) : 0f));
					this._scrollRect.StopMovement();
					Vector2 anchoredPosition = this.entryParent.anchoredPosition;
					anchoredPosition.y = Mathf.Clamp(anchoredPosition.y + num, 0f, Mathf.Max(0f, this.entryParent.rect.height - this._viewport.rect.height));
					this.entryParent.anchoredPosition = anchoredPosition;
					return;
				}
			}
		}

		// Token: 0x06009CCB RID: 40139 RVA: 0x00405095 File Offset: 0x00403295
		private void OnEntryClicked(string appId)
		{
			if (this.windowManager != null)
			{
				this.windowManager.LaunchApp(appId);
			}
			this.Close();
		}

		// Token: 0x06009CCC RID: 40140 RVA: 0x004050B8 File Offset: 0x004032B8
		private void BuildChrome()
		{
			this._popupRect = (RectTransform)this.root.transform;
			this._authoredPosition = this._popupRect.anchoredPosition;
			Canvas componentInParent = this.root.GetComponentInParent<Canvas>(true);
			this._boundsRect = ((componentInParent != null) ? (componentInParent.transform as RectTransform) : ((this._popupRect.parent != null) ? ((this._popupRect.parent.parent as RectTransform) ?? (this._popupRect.parent as RectTransform)) : null));
			LayoutGroup[] components = this.root.GetComponents<LayoutGroup>();
			for (int i = 0; i < components.Length; i++)
			{
				components[i].enabled = false;
			}
			ContentSizeFitter contentSizeFitter;
			if (this.root.TryGetComponent<ContentSizeFitter>(out contentSizeFitter))
			{
				contentSizeFitter.enabled = false;
			}
			RectMask2D rectMask2D;
			if (!this.root.TryGetComponent<RectMask2D>(out rectMask2D))
			{
				this.root.AddComponent<RectMask2D>();
			}
			CielOSTheme active = CielOSTheme.Active;
			Image image;
			if (this.root.TryGetComponent<Image>(out image))
			{
				image.color = ((active != null) ? active.panelBackground : new Color(0.09f, 0.06f, 0.12f, 0.98f));
				CielOSTheme.Round(image);
				CielOSTheme.AddBorder(this._popupRect, null, 1f);
			}
			if (this.searchField == null)
			{
				this.BuildSearchField();
			}
			LocalizedGameText.BindFontOnly(this.searchField.textComponent);
			TMP_Text tmp_Text = this.searchField.placeholder as TMP_Text;
			if (tmp_Text != null && tmp_Text.GetComponent<LocalizedGameText>() == null)
			{
				LocalizedGameText.BindSource(tmp_Text, "cielos_shell", tmp_Text.text, null).FitTranslationToRectangle(false);
			}
			this.searchField.transform.SetParent(this.root.transform, false);
			this.searchField.onValueChanged.RemoveListener(new UnityAction<string>(this.OnSearchChanged));
			this.searchField.onValueChanged.AddListener(new UnityAction<string>(this.OnSearchChanged));
			this.BuildScrollView();
		}

		// Token: 0x06009CCD RID: 40141 RVA: 0x004052D4 File Offset: 0x004034D4
		private void BuildSearchField()
		{
			RectTransform rectTransform = CielOSAppMenu.CreateRect("SearchField", this.root.transform);
			Image image = CielOSAppMenu.AddBackground(rectTransform);
			this.searchField = rectTransform.gameObject.AddComponent<TMP_InputField>();
			this.searchField.targetGraphic = image;
			this.searchField.lineType = TMP_InputField.LineType.SingleLine;
			RectTransform rectTransform2 = CielOSAppMenu.CreateRect("TextArea", rectTransform);
			CielOSAppMenu.Stretch(rectTransform2, new Vector2(28f, 2f), new Vector2(-8f, -2f));
			rectTransform2.gameObject.AddComponent<RectMask2D>();
			TextMeshProUGUI textMeshProUGUI = CielOSAppMenu.CreateText("Text", rectTransform2, "", 13f, TextAlignmentOptions.MidlineLeft);
			TextMeshProUGUI textMeshProUGUI2 = CielOSAppMenu.CreateText("Placeholder", rectTransform2, "Search apps...", 13f, TextAlignmentOptions.MidlineLeft);
			Color color = textMeshProUGUI2.color;
			color.a = 0.55f;
			textMeshProUGUI2.color = color;
			this.searchField.textViewport = rectTransform2;
			this.searchField.textComponent = textMeshProUGUI;
			this.searchField.placeholder = textMeshProUGUI2;
			RectTransform rectTransform3 = CielOSAppMenu.CreateRect("Glyph", rectTransform);
			Vector2 vector = new Vector2(0f, 0.5f);
			rectTransform3.anchorMax = vector;
			rectTransform3.anchorMin = vector;
			rectTransform3.sizeDelta = new Vector2(16f, 16f);
			rectTransform3.anchoredPosition = new Vector2(14f, 0f);
			Image image2 = rectTransform3.gameObject.AddComponent<Image>();
			image2.raycastTarget = false;
			CielOSIcon.Apply(image2, CielOSIconId.Search, (CielOSTheme.Active != null) ? CielOSTheme.Active.trayIconTint : Color.white, 64);
		}

		// Token: 0x06009CCE RID: 40142 RVA: 0x0040546C File Offset: 0x0040366C
		private void BuildScrollView()
		{
			this._scrollRoot = CielOSAppMenu.CreateRect("AppScrollView", this.root.transform);
			this._scrollRoot.gameObject.AddComponent<Image>().color = Color.clear;
			this._scrollRect = this._scrollRoot.gameObject.AddComponent<ScrollRect>();
			this._scrollRect.horizontal = false;
			this._scrollRect.vertical = true;
			this._scrollRect.movementType = ScrollRect.MovementType.Clamped;
			this._scrollRect.scrollSensitivity = 40f;
			this._viewport = CielOSAppMenu.CreateRect("Viewport", this._scrollRoot);
			CielOSAppMenu.Stretch(this._viewport, Vector2.zero, new Vector2(-16f, 0f));
			this._viewport.gameObject.AddComponent<Image>().color = Color.clear;
			this._viewport.gameObject.AddComponent<RectMask2D>();
			this.entryParent.SetParent(this._viewport, false);
			this.entryParent.anchorMin = new Vector2(0f, 1f);
			this.entryParent.anchorMax = Vector2.one;
			this.entryParent.pivot = new Vector2(0.5f, 1f);
			this.entryParent.anchoredPosition = Vector2.zero;
			this.entryParent.sizeDelta = Vector2.zero;
			VerticalLayoutGroup verticalLayoutGroup = this.entryParent.GetComponent<VerticalLayoutGroup>();
			if (verticalLayoutGroup == null)
			{
				verticalLayoutGroup = this.entryParent.gameObject.AddComponent<VerticalLayoutGroup>();
			}
			verticalLayoutGroup.padding = new RectOffset();
			verticalLayoutGroup.spacing = 2f;
			verticalLayoutGroup.childAlignment = TextAnchor.UpperLeft;
			verticalLayoutGroup.childControlWidth = (verticalLayoutGroup.childControlHeight = true);
			verticalLayoutGroup.childForceExpandWidth = true;
			verticalLayoutGroup.childForceExpandHeight = false;
			ContentSizeFitter contentSizeFitter = this.entryParent.GetComponent<ContentSizeFitter>();
			if (contentSizeFitter == null)
			{
				contentSizeFitter = this.entryParent.gameObject.AddComponent<ContentSizeFitter>();
			}
			contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
			contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			this._scrollRect.viewport = this._viewport;
			this._scrollRect.content = this.entryParent;
			this.BuildScrollbar();
			this._emptyState = CielOSAppMenu.CreateText("EmptyState", this._viewport, "No apps found", 13f, TextAlignmentOptions.Center);
			this._emptyState.gameObject.SetActive(false);
		}

		// Token: 0x06009CCF RID: 40143 RVA: 0x004056C4 File Offset: 0x004038C4
		private void BuildScrollbar()
		{
			RectTransform rectTransform = CielOSAppMenu.CreateRect("Scrollbar", this._scrollRoot);
			rectTransform.anchorMin = new Vector2(1f, 0f);
			rectTransform.anchorMax = Vector2.one;
			rectTransform.pivot = new Vector2(1f, 0.5f);
			rectTransform.sizeDelta = new Vector2(12f, 0f);
			CielOSAppMenu.AddBackground(rectTransform);
			RectTransform rectTransform2 = CielOSAppMenu.CreateRect("SlidingArea", rectTransform);
			CielOSAppMenu.Stretch(rectTransform2, new Vector2(2f, 2f), new Vector2(-2f, -2f));
			RectTransform rectTransform3 = CielOSAppMenu.CreateRect("Handle", rectTransform2);
			CielOSAppMenu.Stretch(rectTransform3, Vector2.zero, Vector2.zero);
			Image image = CielOSAppMenu.AddBackground(rectTransform3);
			image.color = ((CielOSTheme.Active != null) ? CielOSTheme.Active.accent : new Color(0.66f, 0.38f, 0.79f, 1f));
			Scrollbar scrollbar = rectTransform.gameObject.AddComponent<Scrollbar>();
			scrollbar.handleRect = rectTransform3;
			scrollbar.targetGraphic = image;
			scrollbar.direction = Scrollbar.Direction.BottomToTop;
			this._scrollRect.verticalScrollbar = scrollbar;
			this._scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
		}

		// Token: 0x06009CD0 RID: 40144 RVA: 0x004057FC File Offset: 0x004039FC
		private void BuildCategoryRail()
		{
			if (this._categoryGrid != null)
			{
				this._categoryGrid.gameObject.SetActive(false);
				if (Application.isPlaying)
				{
					Object.Destroy(this._categoryGrid.gameObject);
				}
				else
				{
					Object.DestroyImmediate(this._categoryGrid.gameObject);
				}
			}
			this._categoryChips.Clear();
			RectTransform rectTransform = CielOSAppMenu.CreateRect("CategoryRail", this.root.transform);
			this._categoryGrid = rectTransform.gameObject.AddComponent<GridLayoutGroup>();
			this._categoryGrid.spacing = new Vector2(4f, 4f);
			this._categoryGrid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
			this._categoryGrid.startAxis = GridLayoutGroup.Axis.Horizontal;
			this._categoryGrid.childAlignment = TextAnchor.UpperLeft;
			for (int i = 0; i <= this._categories.Count; i++)
			{
				string category = ((i == 0) ? "All" : this._categories[i - 1]);
				RectTransform rectTransform2 = CielOSAppMenu.CreateRect(category, rectTransform);
				Image image = CielOSAppMenu.AddBackground(rectTransform2);
				this._categoryChips.Add(image);
				CielOSAppMenu.Stretch(CielOSAppMenu.CreateText("Label", rectTransform2, category, 11f, TextAlignmentOptions.Center).rectTransform, new Vector2(4f, 0f), new Vector2(-4f, 0f));
				Button button = rectTransform2.gameObject.AddComponent<Button>();
				button.targetGraphic = image;
				button.onClick.AddListener(delegate
				{
					this.OnCategorySelected(category);
				});
				CielOSSfx.Attach(button);
			}
			this.HighlightCategory();
		}

		// Token: 0x06009CD1 RID: 40145 RVA: 0x004059A4 File Offset: 0x00403BA4
		private void RefreshLayout(bool resetScroll)
		{
			if (this._popupRect == null || this._scrollRect == null)
			{
				return;
			}
			Rect rect = this.BoundsInPopupParent();
			this._lastBoundsRect = ((this._boundsRect != null) ? this._boundsRect.rect : default(Rect));
			this._popupRect.anchoredPosition = this._authoredPosition;
			float num = Mathf.Max(0.001f, Mathf.Abs(this._popupRect.localScale.x));
			float num2 = Mathf.Max(0.001f, Mathf.Abs(this._popupRect.localScale.y));
			float num3 = Mathf.Min(this.preferredMenuSize.x, Mathf.Max(1f, rect.width - this.edgePadding * 2f) / num);
			float num4 = Mathf.Max(1f, num3 - this.edgePadding * 2f);
			int num5 = Mathf.Clamp(Mathf.FloorToInt((num4 + 4f) / 88f), 1, 3);
			int num6 = Mathf.CeilToInt((float)this._categoryChips.Count / (float)num5);
			float num7 = ((num6 == 0) ? 0f : ((float)num6 * 30f - 4f));
			float num8 = this.edgePadding + 34f + 6f + num7 + 6f;
			float num9 = ((this._visibleEntryCount == 0) ? 80f : ((float)this._visibleEntryCount * 42f - 2f));
			float num10 = Mathf.Min(this.preferredMenuSize.y, num8 + num9 + this.edgePadding);
			this._popupRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, num3);
			this._popupRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, num10);
			float num11 = Mathf.Min(this.GetRectInParent(this._popupRect).yMax, rect.yMax - this.edgePadding);
			float num12 = Mathf.Min(num10, Mathf.Max(1f, num11 - rect.yMin - this.edgePadding) / num2);
			this._popupRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, num12);
			Rect rectInParent = this.GetRectInParent(this._popupRect);
			float num13 = Mathf.Clamp(rectInParent.xMin, rect.xMin + this.edgePadding, Mathf.Max(rect.xMin + this.edgePadding, rect.xMax - this.edgePadding - rectInParent.width));
			this._popupRect.anchoredPosition += new Vector2(num13 - rectInParent.xMin, num11 - rectInParent.yMax);
			this.PositionHeader((RectTransform)this.searchField.transform, this.edgePadding, 34f);
			if (this._categoryGrid != null)
			{
				this._categoryGrid.constraintCount = num5;
				this._categoryGrid.cellSize = new Vector2((num4 - (float)(num5 - 1) * 4f) / (float)num5, 26f);
				this.PositionHeader((RectTransform)this._categoryGrid.transform, this.edgePadding + 34f + 6f, num7);
				LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this._categoryGrid.transform);
			}
			CielOSAppMenu.Stretch(this._scrollRoot, new Vector2(this.edgePadding, this.edgePadding), new Vector2(-this.edgePadding, -num8));
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.entryParent);
			this._scrollRect.StopMovement();
			if (resetScroll)
			{
				this._scrollRect.verticalNormalizedPosition = 1f;
			}
			Vector2 anchoredPosition = this.entryParent.anchoredPosition;
			anchoredPosition.x = 0f;
			anchoredPosition.y = (resetScroll ? 0f : Mathf.Clamp(anchoredPosition.y, 0f, Mathf.Max(0f, this.entryParent.rect.height - this._viewport.rect.height)));
			this.entryParent.anchoredPosition = anchoredPosition;
			this._lastSelection = null;
		}

		// Token: 0x06009CD2 RID: 40146 RVA: 0x00405DBD File Offset: 0x00403FBD
		private Rect BoundsInPopupParent()
		{
			if (this._boundsRect == null)
			{
				return new Rect(-this.preferredMenuSize, this.preferredMenuSize * 3f);
			}
			return this.GetRectInParent(this._boundsRect);
		}

		// Token: 0x06009CD3 RID: 40147 RVA: 0x00405DFC File Offset: 0x00403FFC
		private Rect GetRectInParent(RectTransform rect)
		{
			rect.GetWorldCorners(this._corners);
			Vector2 vector = new Vector2(float.MaxValue, float.MaxValue);
			Vector2 vector2 = new Vector2(float.MinValue, float.MinValue);
			foreach (Vector3 vector3 in this._corners)
			{
				Vector2 vector4 = ((this._popupRect.parent != null) ? this._popupRect.parent.InverseTransformPoint(vector3) : vector3);
				vector = Vector2.Min(vector, vector4);
				vector2 = Vector2.Max(vector2, vector4);
			}
			return Rect.MinMaxRect(vector.x, vector.y, vector2.x, vector2.y);
		}

		// Token: 0x06009CD4 RID: 40148 RVA: 0x00405EB4 File Offset: 0x004040B4
		private void PositionHeader(RectTransform rect, float top, float height)
		{
			rect.anchorMin = new Vector2(0f, 1f);
			rect.anchorMax = Vector2.one;
			rect.pivot = new Vector2(0.5f, 1f);
			rect.sizeDelta = new Vector2(-this.edgePadding * 2f, height);
			rect.anchoredPosition = new Vector2(0f, -top);
		}

		// Token: 0x06009CD5 RID: 40149 RVA: 0x00405F24 File Offset: 0x00404124
		private static RectTransform CreateRect(string name, Transform parent)
		{
			RectTransform rectTransform = (RectTransform)new GameObject(name, new Type[] { typeof(RectTransform) }).transform;
			rectTransform.SetParent(parent, false);
			rectTransform.gameObject.layer = parent.gameObject.layer;
			return rectTransform;
		}

		// Token: 0x06009CD6 RID: 40150 RVA: 0x0013F62D File Offset: 0x0013D82D
		private static void Stretch(RectTransform rect, Vector2 minimum, Vector2 maximum)
		{
			rect.anchorMin = Vector2.zero;
			rect.anchorMax = Vector2.one;
			rect.offsetMin = minimum;
			rect.offsetMax = maximum;
		}

		// Token: 0x06009CD7 RID: 40151 RVA: 0x00405F74 File Offset: 0x00404174
		private static Image AddBackground(RectTransform rect)
		{
			Image image = rect.gameObject.AddComponent<Image>();
			image.color = ((CielOSTheme.Active != null) ? CielOSTheme.Active.controlBackground : new Color(0.18f, 0.14f, 0.23f, 1f));
			CielOSTheme.Round(image);
			return image;
		}

		// Token: 0x06009CD8 RID: 40152 RVA: 0x00405FCC File Offset: 0x004041CC
		private static TextMeshProUGUI CreateText(string name, Transform parent, string value, float size, TextAlignmentOptions alignment)
		{
			RectTransform rectTransform = CielOSAppMenu.CreateRect(name, parent);
			CielOSAppMenu.Stretch(rectTransform, Vector2.zero, Vector2.zero);
			TextMeshProUGUI textMeshProUGUI = rectTransform.gameObject.AddComponent<TextMeshProUGUI>();
			textMeshProUGUI.text = value;
			textMeshProUGUI.fontSize = size;
			textMeshProUGUI.alignment = alignment;
			textMeshProUGUI.enableWordWrapping = false;
			textMeshProUGUI.overflowMode = TextOverflowModes.Ellipsis;
			textMeshProUGUI.raycastTarget = false;
			textMeshProUGUI.color = ((CielOSTheme.Active != null) ? CielOSTheme.Active.controlText : Color.white);
			if (CielOSTheme.Active != null && CielOSTheme.Active.uiFont != null)
			{
				textMeshProUGUI.font = CielOSTheme.Active.uiFont;
			}
			if (!string.IsNullOrEmpty(value))
			{
				LocalizedGameText.BindSource(textMeshProUGUI, "cielos_shell", value, null).FitTranslationToRectangle(false);
			}
			return textMeshProUGUI;
		}

		// Token: 0x0400A147 RID: 41287
		[SerializeField]
		private CielOSAppCatalogSO catalog;

		// Token: 0x0400A148 RID: 41288
		[SerializeField]
		private CielOSWindowManager windowManager;

		// Token: 0x0400A149 RID: 41289
		[Tooltip("Root object toggled on/off (the popup panel).")]
		[SerializeField]
		private GameObject root;

		// Token: 0x0400A14A RID: 41290
		[Tooltip("Entry content, reparented into a masked scroll viewport on first open.")]
		[SerializeField]
		private RectTransform entryParent;

		// Token: 0x0400A14B RID: 41291
		[Tooltip("Prefab of a single menu entry (Button with an icon Image + TMP label).")]
		[SerializeField]
		private CielOSAppMenuEntry entryPrefab;

		// Token: 0x0400A14C RID: 41292
		[Tooltip("Optional search field. Its existing listeners are preserved.")]
		[SerializeField]
		private TMP_InputField searchField;

		// Token: 0x0400A14D RID: 41293
		[Tooltip("Maximum popup size in canvas units; 300 x 520 is recommended. Shrinks to available space and results.")]
		[SerializeField]
		private Vector2 preferredMenuSize = new Vector2(300f, 520f);

		// Token: 0x0400A14E RID: 41294
		[Tooltip("Inset from the popup and canvas edges in canvas units. Recommended: 8.")]
		[SerializeField]
		[Min(4f)]
		private float edgePadding = 8f;

		// Token: 0x0400A14F RID: 41295
		private readonly List<CielOSAppMenuEntry> _entries = new List<CielOSAppMenuEntry>();

		// Token: 0x0400A150 RID: 41296
		private readonly List<Image> _categoryChips = new List<Image>();

		// Token: 0x0400A151 RID: 41297
		private readonly List<string> _categories = new List<string>();

		// Token: 0x0400A152 RID: 41298
		private bool _built;

		// Token: 0x0400A153 RID: 41299
		private string _query = "";

		// Token: 0x0400A154 RID: 41300
		private string _activeCategory = "All";

		// Token: 0x0400A155 RID: 41301
		private int _visibleEntryCount;

		// Token: 0x0400A156 RID: 41302
		private GameObject _lastSelection;

		// Token: 0x0400A157 RID: 41303
		private const float EntryHeight = 40f;

		// Token: 0x0400A158 RID: 41304
		private const float EntrySpacing = 2f;

		// Token: 0x0400A159 RID: 41305
		private const float SearchHeight = 34f;

		// Token: 0x0400A15A RID: 41306
		private const float CategoryHeight = 26f;

		// Token: 0x0400A15B RID: 41307
		private const float HeaderSpacing = 6f;

		// Token: 0x0400A15C RID: 41308
		private const float ScrollbarWidth = 12f;

		// Token: 0x0400A15D RID: 41309
		private readonly Vector3[] _corners = new Vector3[4];

		// Token: 0x0400A15E RID: 41310
		private RectTransform _popupRect;

		// Token: 0x0400A15F RID: 41311
		private RectTransform _boundsRect;

		// Token: 0x0400A160 RID: 41312
		private RectTransform _viewport;

		// Token: 0x0400A161 RID: 41313
		private RectTransform _scrollRoot;

		// Token: 0x0400A162 RID: 41314
		private ScrollRect _scrollRect;

		// Token: 0x0400A163 RID: 41315
		private GridLayoutGroup _categoryGrid;

		// Token: 0x0400A164 RID: 41316
		private TMP_Text _emptyState;

		// Token: 0x0400A165 RID: 41317
		private Vector2 _authoredPosition;

		// Token: 0x0400A166 RID: 41318
		private Rect _lastBoundsRect;
	}
}
