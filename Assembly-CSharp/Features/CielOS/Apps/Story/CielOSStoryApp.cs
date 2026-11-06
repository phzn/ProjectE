using System;
using System.Collections.Generic;
using System.Text;
using Features.CensorBars;
using Features.CielOS.UI;
using Features.CielOS.WindowManager;
using Features.Localization;
using Features.PhoneCompanion;
using Features.Rank;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.Rendering;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.CielOS.Apps.Story
{
	// Token: 0x0200141D RID: 5149
	public class CielOSStoryApp : MonoBehaviour, ICielOSWindowCloseGuard
	{
		// Token: 0x170018E6 RID: 6374
		// (get) Token: 0x06009EE1 RID: 40673 RVA: 0x00416414 File Offset: 0x00414614
		private bool UseProgression
		{
			get
			{
				return Application.isPlaying;
			}
		}

		// Token: 0x170018E7 RID: 6375
		// (get) Token: 0x06009EE2 RID: 40674 RVA: 0x0041641C File Offset: 0x0041461C
		private int AuthoredPageCount
		{
			get
			{
				if (!(this.storyBook != null) || this.storyBook.pages == null || this.storyBook.pages.Count <= 0)
				{
					return 1;
				}
				return this.storyBook.pages.Count;
			}
		}

		// Token: 0x170018E8 RID: 6376
		// (get) Token: 0x06009EE3 RID: 40675 RVA: 0x00416469 File Offset: 0x00414669
		private int AccessibleAuthoredPageCount
		{
			get
			{
				return CielOSStoryApp.ResolveAccessiblePageCount(this.AuthoredPageCount, this.UseProgression ? RankStoryProgression.UnlockedPageCount : this.AuthoredPageCount, this.UseProgression);
			}
		}

		// Token: 0x170018E9 RID: 6377
		// (get) Token: 0x06009EE4 RID: 40676 RVA: 0x00416491 File Offset: 0x00414691
		private int PageCount
		{
			get
			{
				return this.AccessibleAuthoredPageCount + this.PersonalPageCount;
			}
		}

		// Token: 0x06009EE5 RID: 40677 RVA: 0x004164A0 File Offset: 0x004146A0
		private void Awake()
		{
			if (this.UseProgression)
			{
				RankStoryProgression.EnsureInitialized();
			}
			CielOSTheme active = CielOSTheme.Active;
			this._font = ((this.bodyFont != null) ? this.bodyFont : this.monoFont);
			if (this._font == null && active != null)
			{
				this._font = active.uiFont;
			}
			this._self = (RectTransform)base.transform;
			CielOSStoryApp.Stretch(this._self);
			Image image = this._self.gameObject.GetComponent<Image>();
			if (image == null)
			{
				image = this._self.gameObject.AddComponent<Image>();
			}
			image.color = CielOSStoryApp.Paper;
			this.BuildScrollArea();
			this.BuildNav();
			this.BuildDiaryActions();
			this.ShowPage(0);
		}

		// Token: 0x06009EE6 RID: 40678 RVA: 0x00416570 File Offset: 0x00414770
		private void Start()
		{
			this.MaximizeWindow();
		}

		// Token: 0x06009EE7 RID: 40679 RVA: 0x00416578 File Offset: 0x00414778
		private void OnEnable()
		{
			RankProgressionService.Changed += this.HandleProgressionChanged;
			GameLocalization.LocaleChanged += this.RefreshLocale;
			this.MaximizeWindow();
			this.RefreshLocale();
			if (this._scroll != null && (this._diaryEditor != null || !this.ShowPendingChapter()))
			{
				this.RefreshNavigation();
			}
		}

		// Token: 0x06009EE8 RID: 40680 RVA: 0x004165DD File Offset: 0x004147DD
		private void OnDisable()
		{
			this.FlushDiaryChanges();
			RankProgressionService.Changed -= this.HandleProgressionChanged;
			GameLocalization.LocaleChanged -= this.RefreshLocale;
		}

		// Token: 0x06009EE9 RID: 40681 RVA: 0x00416608 File Offset: 0x00414808
		private void RefreshLocale()
		{
			if (this._scroll == null || this._sheet == null)
			{
				return;
			}
			if (this._diaryEditor != null)
			{
				this.RebuildDiaryEditor();
			}
			float verticalNormalizedPosition = this._scroll.verticalNormalizedPosition;
			bool pageViewRecorded = this._pageViewRecorded;
			int pageShownFrame = this._pageShownFrame;
			this.ShowPage(this._page);
			this._pageViewRecorded = pageViewRecorded;
			this._pageShownFrame = pageShownFrame;
			this._scroll.verticalNormalizedPosition = verticalNormalizedPosition;
		}

		// Token: 0x06009EEA RID: 40682 RVA: 0x00416686 File Offset: 0x00414886
		private void HandleProgressionChanged()
		{
			if (this._scroll == null)
			{
				return;
			}
			if (this._page >= this.PageCount)
			{
				this.ShowPage(this.PageCount - 1);
				return;
			}
			this.RefreshNavigation();
		}

		// Token: 0x06009EEB RID: 40683 RVA: 0x004166BC File Offset: 0x004148BC
		private void MaximizeWindow()
		{
			CielOSWindow componentInParent = base.GetComponentInParent<CielOSWindow>();
			if (componentInParent == null || componentInParent.Rect == null)
			{
				return;
			}
			RectTransform rect = componentInParent.Rect;
			rect.anchorMin = new Vector2(0f, 0f);
			rect.anchorMax = new Vector2(1f, 1f);
			rect.pivot = new Vector2(0.5f, 0.5f);
			rect.offsetMin = Vector2.zero;
			rect.offsetMax = Vector2.zero;
			CielOSWindowResizeHandle[] componentsInChildren = componentInParent.GetComponentsInChildren<CielOSWindowResizeHandle>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.SetActive(false);
			}
			CielOSWindowDragHandle[] componentsInChildren2 = componentInParent.GetComponentsInChildren<CielOSWindowDragHandle>(true);
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				componentsInChildren2[i].enabled = false;
			}
		}

		// Token: 0x06009EEC RID: 40684 RVA: 0x00416788 File Offset: 0x00414988
		private void Update()
		{
			this.UpdateDiaryAutosave();
			bool flag = this.IsReaderVisible();
			if (flag && !this.DiaryOwnsInput && !PhotoPolaroidViewer.IsOpen)
			{
				if (GameplayInput.GetKeyDown(KeyCode.RightArrow) || GameplayInput.GetKeyDown(KeyCode.PageDown))
				{
					this.StepPage(1);
				}
				else if (GameplayInput.GetKeyDown(KeyCode.LeftArrow) || GameplayInput.GetKeyDown(KeyCode.PageUp))
				{
					this.StepPage(-1);
				}
			}
			if (this._sheet != null && this._scroll != null && this._scroll.viewport != null)
			{
				Vector2 size = this._scroll.viewport.rect.size;
				if (size.x > 0f && (size - this._lastViewport).sqrMagnitude > 1f)
				{
					this._lastViewport = size;
					this.ShowPage(this._page);
					this.RefreshDiaryEditorLayout();
				}
			}
			this.UpdateReadingProgress(flag);
		}

		// Token: 0x06009EED RID: 40685 RVA: 0x00416884 File Offset: 0x00414A84
		private void UpdateReadingProgress(bool readerVisible)
		{
			if (!this.UseProgression || this._page >= this.AccessibleAuthoredPageCount || this.DiaryOwnsInput || this._pageViewRecorded || !readerVisible || PhotoPolaroidViewer.IsOpen || Time.frameCount <= this._pageShownFrame || this._sheet == null || this._scroll == null || this._scroll.viewport == null || this._scroll.viewport.rect.height <= 1f)
			{
				return;
			}
			this._pageViewRecorded = true;
			RankStoryProgression.MarkPageRead(this._page);
			this.RefreshNavigation();
		}

		// Token: 0x06009EEE RID: 40686 RVA: 0x0041693C File Offset: 0x00414B3C
		private void BuildScrollArea()
		{
			RectTransform rectTransform = CielOSStoryApp.NewChild("ScrollArea", this._self);
			CielOSStoryApp.Stretch(rectTransform);
			rectTransform.offsetMin = new Vector2(0f, 120f);
			rectTransform.gameObject.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.001f);
			this._scroll = rectTransform.gameObject.AddComponent<ScrollRect>();
			this._scroll.horizontal = false;
			this._scroll.vertical = true;
			this._scroll.movementType = ScrollRect.MovementType.Clamped;
			RectTransform rectTransform2 = CielOSStoryApp.NewChild("Viewport", rectTransform);
			CielOSStoryApp.Stretch(rectTransform2);
			rectTransform2.offsetMax = new Vector2(-20f, 0f);
			rectTransform2.gameObject.AddComponent<RectMask2D>();
			rectTransform2.gameObject.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.001f);
			this._content = CielOSStoryApp.NewChild("Content", rectTransform2);
			this._content.anchorMin = new Vector2(0f, 1f);
			this._content.anchorMax = new Vector2(1f, 1f);
			this._content.pivot = new Vector2(0.5f, 1f);
			this._content.sizeDelta = new Vector2(0f, 600f);
			this._scroll.viewport = rectTransform2;
			this._scroll.content = this._content;
			this._smoothScroll = CielOSStoryApp.SmoothWheelScroll.Attach(this._scroll);
			this.CreatePageScrollbar(this._scroll, rectTransform, new Action(this._smoothScroll.Cancel));
		}

		// Token: 0x06009EEF RID: 40687 RVA: 0x00416AF4 File Offset: 0x00414CF4
		private void BuildNav()
		{
			RectTransform rectTransform = CielOSStoryApp.NewChild("ReaderFooter", this._self);
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = new Vector2(1f, 0f);
			rectTransform.pivot = new Vector2(0.5f, 0f);
			rectTransform.sizeDelta = new Vector2(0f, 120f);
			rectTransform.gameObject.AddComponent<Image>().color = CielOSStoryApp.Paper;
			this._prevButton = this.MakeNavButton(rectTransform, "<  prev", new Vector2(0f, 0f), new Vector2(26f, 20f), delegate
			{
				this.StepPage(-1);
			});
			this._nextButton = this.MakeNavButton(rectTransform, "next  >", new Vector2(1f, 0f), new Vector2(-26f, 20f), delegate
			{
				this.StepPage(1);
			});
			this._pageLabel = CielOSStoryApp.MakeLabel(rectTransform, "", 19f, FontStyles.Normal, CielOSStoryApp.InkDim, TextAlignmentOptions.Center, this._font);
			RectTransform rectTransform2 = (RectTransform)this._pageLabel.transform;
			Vector2 vector = new Vector2(0.5f, 0f);
			rectTransform2.anchorMax = vector;
			rectTransform2.anchorMin = vector;
			rectTransform2.pivot = new Vector2(0.5f, 0f);
			rectTransform2.sizeDelta = new Vector2(260f, 32f);
			rectTransform2.anchoredPosition = new Vector2(0f, 22f);
			this._chapterHint = CielOSStoryApp.MakeLabel(rectTransform, "", 18f, FontStyles.Normal, CielOSStoryApp.InkDim, TextAlignmentOptions.MidlineLeft, this._font);
			RectTransform rectTransform3 = (RectTransform)this._chapterHint.transform;
			rectTransform3.anchorMin = Vector2.zero;
			rectTransform3.anchorMax = new Vector2(1f, 0f);
			rectTransform3.pivot = new Vector2(0.5f, 0f);
			rectTransform3.offsetMin = new Vector2(30f, 77f);
			rectTransform3.offsetMax = new Vector2(-30f, 111f);
		}

		// Token: 0x06009EF0 RID: 40688 RVA: 0x00416D14 File Offset: 0x00414F14
		private Button MakeNavButton(RectTransform parent, string label, Vector2 anchor, Vector2 offset, UnityAction onClick)
		{
			RectTransform rectTransform = CielOSStoryApp.NewChild("Nav_" + label, parent);
			RectTransform rectTransform2 = rectTransform;
			rectTransform.anchorMax = anchor;
			rectTransform2.anchorMin = anchor;
			rectTransform.pivot = anchor;
			rectTransform.sizeDelta = new Vector2(150f, 46f);
			rectTransform.anchoredPosition = offset;
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = new Color(0.925f, 0.9f, 0.85f, 1f);
			CielOSTheme.Round(image);
			CielOSTheme.AddBorder(rectTransform, new Color?(new Color(CielOSStoryApp.Ink.r, CielOSStoryApp.Ink.g, CielOSStoryApp.Ink.b, 0.3f)), 1f);
			Button button = rectTransform.gameObject.AddComponent<Button>();
			button.targetGraphic = image;
			ColorBlock colors = button.colors;
			colors.highlightedColor = new Color(0.94f, 0.94f, 0.94f, 1f);
			colors.pressedColor = new Color(0.85f, 0.85f, 0.85f, 1f);
			button.colors = colors;
			button.onClick.AddListener(onClick);
			CielOSSfx.Attach(button);
			CielOSStoryApp.Stretch((RectTransform)CielOSStoryApp.MakeLabel(rectTransform, label, 19f, FontStyles.Normal, CielOSStoryApp.Ink, TextAlignmentOptions.Center, this._font).transform);
			return button;
		}

		// Token: 0x06009EF1 RID: 40689 RVA: 0x00416E70 File Offset: 0x00415070
		private void StepPage(int delta)
		{
			if (this.IsReaderVisible() && !this.DiaryOwnsInput)
			{
				this.ShowPage(this._page + delta);
			}
		}

		// Token: 0x06009EF2 RID: 40690 RVA: 0x00416E90 File Offset: 0x00415090
		public bool ShowPendingChapter()
		{
			if (!this.UseProgression || this._scroll == null)
			{
				return false;
			}
			int pendingPageIndex = RankStoryProgression.PendingPageIndex;
			if (pendingPageIndex < 0 || pendingPageIndex >= this.PageCount || pendingPageIndex >= this.AuthoredPageCount)
			{
				return false;
			}
			if (this._page != pendingPageIndex || this._sheet == null)
			{
				this.ShowPage(pendingPageIndex);
			}
			else
			{
				this.RefreshNavigation();
			}
			return true;
		}

		// Token: 0x06009EF3 RID: 40691 RVA: 0x00416EFC File Offset: 0x004150FC
		private void ShowPage(int index)
		{
			bool flag = this._page != Mathf.Clamp(index, 0, this.PageCount - 1);
			this._page = Mathf.Clamp(index, 0, this.PageCount - 1);
			this._pageViewRecorded = false;
			this._pageShownFrame = Time.frameCount;
			if (this._sheet != null)
			{
				this._sheet.gameObject.SetActive(false);
				if (Application.isPlaying)
				{
					Object.Destroy(this._sheet.gameObject);
				}
				else
				{
					Object.DestroyImmediate(this._sheet.gameObject);
				}
			}
			if (flag && this._diaryEditor == null && !PhotoPolaroidViewer.IsOpen)
			{
				this.ReleaseDiarySprites();
			}
			this._sheet = (this.IsPersonalPage ? this.BuildPersonalDiarySheet(this.CurrentPersonalPage) : this.BuildSheet(this.GetPage(this._page)));
			this.RefreshNavigation();
			this._scroll.verticalNormalizedPosition = 1f;
			if (this._smoothScroll != null)
			{
				this._smoothScroll.Cancel();
			}
		}

		// Token: 0x06009EF4 RID: 40692 RVA: 0x0041700C File Offset: 0x0041520C
		private void RefreshNavigation()
		{
			int pageCount = this.PageCount;
			int num = (this.UseProgression ? RankStoryProgression.PendingPageIndex : (-1));
			bool flag = num == this._page;
			LocalizedGameText localizedGameText = LocalizedGameText.BindFormat(this._pageLabel, "phone_story", (this.UseProgression && !this.CanEditPersonalDiary) ? "page {0} / {1} unlocked" : "page {0} / {1}", new object[]
			{
				this._page + 1,
				this.UseProgression ? pageCount : this.AuthoredPageCount
			});
			if (localizedGameText != null)
			{
				localizedGameText.FitTranslationToRectangle(false);
			}
			if (this._prevButton != null)
			{
				this._prevButton.interactable = this._page > 0;
			}
			if (this._nextButton != null)
			{
				this._nextButton.interactable = this._page < pageCount - 1;
			}
			if (this._chapterHint != null)
			{
				if (this.UseProgression && !flag && num >= 0)
				{
					LocalizedGameText.BindFormat(this._chapterHint, "phone_story", "A new chapter is waiting on page {0}.", new object[] { num + 1 });
				}
				else
				{
					LocalizedGameText.BindSource(this._chapterHint, "phone_story", (!this.UseProgression) ? "Author preview: all pages are available." : (flag ? "New chapter" : CielOSStoryApp.GetNextChapterHint(pageCount)), null);
				}
				this._chapterHint.GetComponent<LocalizedGameText>().FitTranslationToRectangle(false);
			}
			this.RefreshDiaryActions();
		}

		// Token: 0x06009EF5 RID: 40693 RVA: 0x00417178 File Offset: 0x00415378
		private bool IsReaderVisible()
		{
			if (!base.isActiveAndEnabled || !base.gameObject.activeInHierarchy)
			{
				return false;
			}
			if (GraphicsSettingsRuntimeUI.AnyOpen)
			{
				return false;
			}
			if (Application.isPlaying && this.UseProgression && !CielOSController.AnyOpen)
			{
				return false;
			}
			if (this._window == null)
			{
				this._window = base.GetComponentInParent<CielOSWindow>();
			}
			if (this._window != null)
			{
				if (!this._window.IsOpen || this._window.IsMinimized)
				{
					return false;
				}
				Transform parent = this._window.transform.parent;
				if (parent != null)
				{
					for (int i = this._window.transform.GetSiblingIndex() + 1; i < parent.childCount; i++)
					{
						CielOSWindow component = parent.GetChild(i).GetComponent<CielOSWindow>();
						if (component != null && component.isActiveAndEnabled && component.IsOpen && !component.IsMinimized)
						{
							return false;
						}
					}
				}
			}
			this._visibilityGroups.Clear();
			base.GetComponentsInParent<CanvasGroup>(false, this._visibilityGroups);
			foreach (CanvasGroup canvasGroup in this._visibilityGroups)
			{
				if (canvasGroup.alpha < 0.99f || !canvasGroup.interactable || !canvasGroup.blocksRaycasts)
				{
					return false;
				}
			}
			this._parentCanvases.Clear();
			base.GetComponentsInParent<Canvas>(false, this._parentCanvases);
			using (List<Canvas>.Enumerator enumerator2 = this._parentCanvases.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (!enumerator2.Current.isActiveAndEnabled)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06009EF6 RID: 40694 RVA: 0x00417350 File Offset: 0x00415550
		public static int ResolveAccessiblePageCount(int authoredCount, int unlockedCount, bool runtime)
		{
			int num = Mathf.Max(1, authoredCount);
			if (!runtime)
			{
				return num;
			}
			return Mathf.Clamp(unlockedCount, 1, num);
		}

		// Token: 0x06009EF7 RID: 40695 RVA: 0x00417374 File Offset: 0x00415574
		public static string GetNextChapterHint(int unlockedCount)
		{
			switch (unlockedCount)
			{
			case 1:
				return "Next chapter: finish your first run.";
			case 2:
				return "Next chapter: finish your first run as CURIOUS.";
			case 3:
				return "Next chapter: finish your first run as AROUSED.";
			case 4:
				return "Next chapter: finish your first run as HORNY.";
			case 5:
				return "Next chapter: finish your first run as HORNY SLUT.";
			case 6:
				return "Next chapter: finish your first run as CERTIFIED EXHIBITIONIST.";
			default:
				return "All chapters unlocked.";
			}
		}

		// Token: 0x06009EF8 RID: 40696 RVA: 0x004173CC File Offset: 0x004155CC
		private CielOSStoryBookSO.Page GetPage(int index)
		{
			if (this.storyBook != null && this.storyBook.pages != null && index >= 0 && index < this.storyBook.pages.Count)
			{
				return this.storyBook.pages[index];
			}
			return new CielOSStoryBookSO.Page
			{
				title = "Empty notebook",
				body = "No StoryBook assigned.\nTools > ProjectExhibitionist > CielOS > Setup Story App"
			};
		}

		// Token: 0x06009EF9 RID: 40697 RVA: 0x00417438 File Offset: 0x00415638
		private RectTransform BuildSheet(CielOSStoryBookSO.Page page)
		{
			CielOSTheme active = CielOSTheme.Active;
			Color color = Color.Lerp((active != null) ? active.accent : new Color(0.663f, 0.376f, 0.788f, 1f), Color.black, 0.25f);
			RectTransform rectTransform = CielOSStoryApp.NewChild("Sheet", this._content);
			rectTransform.anchorMin = new Vector2(0f, 1f);
			rectTransform.anchorMax = new Vector2(1f, 1f);
			rectTransform.pivot = new Vector2(0.5f, 1f);
			rectTransform.anchoredPosition = Vector2.zero;
			float num = ((this._scroll != null && this._scroll.viewport != null) ? this._scroll.viewport.rect.width : 0f);
			if (num <= 0f)
			{
				num = 1640f;
			}
			float num2 = Mathf.Clamp((num - 80f) / 1560f, 0.55f, 1.25f);
			RectTransform rectTransform2 = CielOSStoryApp.NewChild("Column", rectTransform);
			RectTransform rectTransform3 = rectTransform2;
			RectTransform rectTransform4 = rectTransform2;
			Vector2 vector = new Vector2(0.5f, 1f);
			rectTransform4.anchorMax = vector;
			rectTransform3.anchorMin = vector;
			rectTransform2.pivot = new Vector2(0.5f, 1f);
			rectTransform2.anchoredPosition = Vector2.zero;
			rectTransform2.localScale = new Vector3(num2, num2, 1f);
			string source = GameLocalization.GetSource("phone_story", page.title ?? "");
			float num3 = 1360f;
			TMP_Text tmp_Text = CielOSStoryApp.MakeLabel(rectTransform2, source, 48f, FontStyles.Normal, color, TextAlignmentOptions.TopLeft, this._font);
			float num4 = (string.IsNullOrEmpty(source) ? 0f : (tmp_Text.GetPreferredValues(source, num3, 0f).y + 26f));
			List<CielOSStoryApp.BodySegment> list = CielOSStoryApp.ParseBodySegments(GameLocalization.GetSource("phone_story", page.body ?? ""));
			List<TMP_Text> list2 = new List<TMP_Text>(list.Count);
			List<float> list3 = new List<float>(list.Count);
			float num5 = 0f;
			for (int i = 0; i < list.Count; i++)
			{
				CielOSStoryApp.BodySegment bodySegment = list[i];
				TMP_Text tmp_Text2;
				float num6;
				if (bodySegment.isHeading)
				{
					tmp_Text2 = CielOSStoryApp.MakeLabel(rectTransform2, bodySegment.text, 48f, FontStyles.Normal, color, TextAlignmentOptions.TopLeft, this._font);
					num6 = tmp_Text2.GetPreferredValues(bodySegment.text, num3, 0f).y + 26f;
					if (i > 0)
					{
						num5 += 10f;
					}
				}
				else
				{
					tmp_Text2 = CielOSStoryApp.MakeLabel(rectTransform2, bodySegment.text, 27f, FontStyles.Normal, CielOSStoryApp.Ink, TextAlignmentOptions.TopLeft, this._font);
					tmp_Text2.lineSpacing = 6f;
					num6 = tmp_Text2.GetPreferredValues(bodySegment.text, num3, 0f).y;
				}
				list2.Add(tmp_Text2);
				list3.Add(num6);
				num5 += num6;
			}
			float num7 = 64f + num4 + num5 + 96f;
			if (page.stickers != null)
			{
				foreach (CielOSStoryBookSO.Sticker sticker in page.stickers)
				{
					if (sticker != null && !(sticker.sprite == null))
					{
						float num8 = this.StickerFrameHeight(sticker);
						num7 = Mathf.Max(num7, sticker.position.y + num8 * 0.5f + sticker.width * 0.12f + 40f);
					}
				}
			}
			float num9 = ((this._scroll != null && this._scroll.viewport != null) ? this._scroll.viewport.rect.height : 0f);
			float num10 = ((num9 > 0f) ? (num9 / num2) : 620f);
			num7 = Mathf.Max(new float[] { num7, page.minHeight, num10, 620f });
			rectTransform2.sizeDelta = new Vector2(1560f, num7);
			rectTransform.sizeDelta = new Vector2(0f, num7 * num2);
			this._content.sizeDelta = new Vector2(0f, num7 * num2);
			RectTransform rectTransform5 = CielOSStoryApp.NewChild("Paper", rectTransform);
			CielOSStoryApp.Stretch(rectTransform5);
			Image image = rectTransform5.gameObject.AddComponent<Image>();
			image.color = CielOSStoryApp.Paper;
			image.raycastTarget = false;
			rectTransform5.SetSiblingIndex(0);
			RectTransform rectTransform6 = CielOSStoryApp.NewChild("Grid", rectTransform);
			CielOSStoryApp.Stretch(rectTransform6);
			Image image2 = rectTransform6.gameObject.AddComponent<Image>();
			image2.sprite = CielOSStoryApp.GridSprite();
			image2.type = Image.Type.Tiled;
			image2.pixelsPerUnitMultiplier = 0.75f;
			image2.color = CielOSStoryApp.GridInk;
			image2.raycastTarget = false;
			rectTransform6.SetSiblingIndex(1);
			rectTransform2.SetAsLastSibling();
			RectTransform rectTransform7 = CielOSStoryApp.NewChild("MarginLine", rectTransform2);
			rectTransform7.anchorMin = new Vector2(0f, 0f);
			rectTransform7.anchorMax = new Vector2(0f, 1f);
			rectTransform7.pivot = new Vector2(0.5f, 0.5f);
			rectTransform7.sizeDelta = new Vector2(3f, 0f);
			rectTransform7.anchoredPosition = new Vector2(102f, 0f);
			Image image3 = rectTransform7.gameObject.AddComponent<Image>();
			image3.color = CielOSStoryApp.MarginRed;
			image3.raycastTarget = false;
			rectTransform7.SetAsFirstSibling();
			CielOSStoryApp.PlaceTopLeft((RectTransform)tmp_Text.transform, 120f, 64f, num3, Mathf.Max(num4, 1f));
			if (!string.IsNullOrEmpty(source))
			{
				this.DrawPenUnderline(rectTransform2, tmp_Text, source, num3, color, 64f + num4 - 16f);
			}
			float num11 = 64f + num4;
			for (int j = 0; j < list.Count; j++)
			{
				if (list[j].isHeading && j > 0)
				{
					num11 += 10f;
				}
				CielOSStoryApp.PlaceTopLeft((RectTransform)list2[j].transform, 120f, num11, num3, Mathf.Max(list3[j], 1f));
				if (list[j].isHeading)
				{
					this.DrawPenUnderline(rectTransform2, list2[j], list[j].text, num3, color, num11 + list3[j] - 16f);
				}
				num11 += list3[j];
			}
			RectTransform rectTransform8 = (RectTransform)CielOSStoryApp.MakeLabel(rectTransform2, "· " + (this._page + 1).ToString() + " ·", 19f, FontStyles.Normal, CielOSStoryApp.InkDim, TextAlignmentOptions.Center, this._font).transform;
			vector = new Vector2(1f, 0f);
			rectTransform8.anchorMax = vector;
			rectTransform8.anchorMin = vector;
			rectTransform8.pivot = new Vector2(1f, 0f);
			rectTransform8.sizeDelta = new Vector2(110f, 32f);
			rectTransform8.anchoredPosition = new Vector2(-28f, 18f);
			List<RectTransform> list4 = new List<RectTransform>();
			if (page.stickers != null)
			{
				foreach (CielOSStoryBookSO.Sticker sticker2 in page.stickers)
				{
					if (sticker2 != null && sticker2.sprite != null)
					{
						list4.Add(this.BuildSticker(rectTransform2, sticker2));
					}
				}
			}
			if (GameLocalization.CurrentCode != "en" && list4.Count > 0)
			{
				num7 = CielOSStoryApp.ArrangeTranslatedStickers(rectTransform2, tmp_Text, list2, list4, num7);
				rectTransform2.sizeDelta = new Vector2(1560f, num7);
				rectTransform.sizeDelta = new Vector2(0f, num7 * num2);
				this._content.sizeDelta = new Vector2(0f, num7 * num2);
			}
			return rectTransform;
		}

		// Token: 0x06009EFA RID: 40698 RVA: 0x00417C84 File Offset: 0x00415E84
		private static float ArrangeTranslatedStickers(RectTransform column, TMP_Text title, List<TMP_Text> body, List<RectTransform> stickers, float sheetHeight)
		{
			List<Rect> list = new List<Rect>();
			CielOSStoryApp.AddWritingBounds(column, title, list);
			foreach (TMP_Text tmp_Text in body)
			{
				CielOSStoryApp.AddWritingBounds(column, tmp_Text, list);
			}
			stickers.Sort((RectTransform a, RectTransform b) => b.anchoredPosition.y.CompareTo(a.anchoredPosition.y));
			float num = float.PositiveInfinity;
			foreach (RectTransform rectTransform in stickers)
			{
				Rect rect = CielOSStoryApp.StickerBounds(column, rectTransform);
				float num2 = Mathf.Max(0f, rect.center.y - num);
				rect.y -= num2;
				for (int i = 0; i <= list.Count; i++)
				{
					float num3 = 0f;
					foreach (Rect rect2 in list)
					{
						Rect rect3 = Rect.MinMaxRect(rect2.xMin - 24f, rect2.yMin - 24f, rect2.xMax + 24f, rect2.yMax + 24f);
						if (rect.Overlaps(rect3))
						{
							num3 = Mathf.Max(num3, rect.yMax - rect3.yMin + 0.5f);
						}
					}
					if (num3 <= 0f)
					{
						break;
					}
					rect.y -= num3;
					num2 += num3;
				}
				rectTransform.anchoredPosition += Vector2.down * num2;
				num = rect.center.y;
				list.Add(rect);
				sheetHeight = Mathf.Max(sheetHeight, -rect.yMin + 96f);
			}
			return sheetHeight;
		}

		// Token: 0x06009EFB RID: 40699 RVA: 0x00417ECC File Offset: 0x004160CC
		private static void AddWritingBounds(RectTransform column, TMP_Text text, List<Rect> bounds)
		{
			text.ForceMeshUpdate(true, true);
			for (int i = 0; i < text.textInfo.characterCount; i++)
			{
				TMP_CharacterInfo tmp_CharacterInfo = text.textInfo.characterInfo[i];
				if (tmp_CharacterInfo.isVisible)
				{
					Vector3 vector = column.InverseTransformPoint(text.transform.TransformPoint(tmp_CharacterInfo.bottomLeft));
					Vector3 vector2 = column.InverseTransformPoint(text.transform.TransformPoint(tmp_CharacterInfo.topRight));
					bounds.Add(Rect.MinMaxRect(vector.x, vector.y, vector2.x, vector2.y));
				}
			}
			if (text.textInfo.characterCount == 0 && !string.IsNullOrEmpty(text.text))
			{
				bounds.Add(CielOSStoryApp.RectInColumn(column, text.rectTransform));
			}
		}

		// Token: 0x06009EFC RID: 40700 RVA: 0x00417F90 File Offset: 0x00416190
		private static Rect StickerBounds(RectTransform column, RectTransform sticker)
		{
			Rect rect = CielOSStoryApp.RectInColumn(column, sticker);
			foreach (RectTransform rectTransform in sticker.GetComponentsInChildren<RectTransform>(true))
			{
				Rect rect2 = CielOSStoryApp.RectInColumn(column, rectTransform);
				rect = Rect.MinMaxRect(Mathf.Min(rect.xMin, rect2.xMin), Mathf.Min(rect.yMin, rect2.yMin), Mathf.Max(rect.xMax, rect2.xMax), Mathf.Max(rect.yMax, rect2.yMax));
			}
			return rect;
		}

		// Token: 0x06009EFD RID: 40701 RVA: 0x0041801C File Offset: 0x0041621C
		private static Rect RectInColumn(RectTransform column, RectTransform rect)
		{
			Vector3[] array = new Vector3[4];
			rect.GetWorldCorners(array);
			Vector2 vector = column.InverseTransformPoint(array[0]);
			Vector2 vector2 = vector;
			for (int i = 1; i < array.Length; i++)
			{
				Vector2 vector3 = column.InverseTransformPoint(array[i]);
				vector = Vector2.Min(vector, vector3);
				vector2 = Vector2.Max(vector2, vector3);
			}
			return Rect.MinMaxRect(vector.x, vector.y, vector2.x, vector2.y);
		}

		// Token: 0x06009EFE RID: 40702 RVA: 0x004180A0 File Offset: 0x004162A0
		private static List<CielOSStoryApp.BodySegment> ParseBodySegments(string body)
		{
			List<CielOSStoryApp.BodySegment> list = new List<CielOSStoryApp.BodySegment>();
			if (string.IsNullOrEmpty(body))
			{
				return list;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string text in body.Split('\n', StringSplitOptions.None))
			{
				string text2 = text.TrimStart();
				if (text2.StartsWith("# "))
				{
					if (stringBuilder.Length > 0)
					{
						list.Add(new CielOSStoryApp.BodySegment
						{
							isHeading = false,
							text = stringBuilder.ToString()
						});
						stringBuilder.Length = 0;
					}
					list.Add(new CielOSStoryApp.BodySegment
					{
						isHeading = true,
						text = text2.Substring(2).Trim()
					});
				}
				else
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append('\n');
					}
					stringBuilder.Append(text);
				}
			}
			if (stringBuilder.Length > 0)
			{
				list.Add(new CielOSStoryApp.BodySegment
				{
					isHeading = false,
					text = stringBuilder.ToString()
				});
			}
			return list;
		}

		// Token: 0x06009EFF RID: 40703 RVA: 0x004181AC File Offset: 0x004163AC
		private void DrawPenUnderline(RectTransform column, TMP_Text tmp, string text, float textWidth, Color color, float y)
		{
			float num = Mathf.Min(tmp.GetPreferredValues(text, textWidth, 0f).x, textWidth);
			RectTransform rectTransform = CielOSStoryApp.NewChild("PenUnderline", column);
			Vector2 vector = new Vector2(0f, 1f);
			rectTransform.anchorMax = vector;
			rectTransform.anchorMin = vector;
			rectTransform.pivot = new Vector2(0f, 0.5f);
			rectTransform.sizeDelta = new Vector2(num + 22f, 4f);
			rectTransform.anchoredPosition = new Vector2(114f, -y);
			rectTransform.localEulerAngles = new Vector3(0f, 0f, 0.7f);
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = new Color(color.r, color.g, color.b, 0.45f);
			image.raycastTarget = false;
		}

		// Token: 0x06009F00 RID: 40704 RVA: 0x0041828C File Offset: 0x0041648C
		private float StickerPhotoHeight(CielOSStoryBookSO.Sticker s)
		{
			float num = s.sprite.rect.height / Mathf.Max(1f, s.sprite.rect.width);
			return s.width * num;
		}

		// Token: 0x06009F01 RID: 40705 RVA: 0x004182D3 File Offset: 0x004164D3
		private float StickerFrameHeight(CielOSStoryBookSO.Sticker s)
		{
			return this.StickerPhotoHeight(s) + 12f;
		}

		// Token: 0x06009F02 RID: 40706 RVA: 0x004182E4 File Offset: 0x004164E4
		private RectTransform BuildSticker(RectTransform sheet, CielOSStoryBookSO.Sticker s)
		{
			float num = this.StickerPhotoHeight(s);
			float num2 = s.width + 12f;
			float num3 = num + 12f;
			RectTransform rectTransform = CielOSStoryApp.NewChild("Sticker", sheet);
			RectTransform rectTransform2 = rectTransform;
			RectTransform rectTransform3 = rectTransform;
			Vector2 vector = new Vector2(0f, 1f);
			rectTransform3.anchorMax = vector;
			rectTransform2.anchorMin = vector;
			rectTransform.pivot = new Vector2(0.5f, 0.5f);
			rectTransform.sizeDelta = new Vector2(num2, num3);
			rectTransform.anchoredPosition = new Vector2(s.position.x, -s.position.y);
			rectTransform.localEulerAngles = new Vector3(0f, 0f, -s.rotation);
			RectTransform rectTransform4 = CielOSStoryApp.NewChild("Shadow", rectTransform);
			CielOSStoryApp.Stretch(rectTransform4);
			rectTransform4.offsetMin = new Vector2(-1f, -8f);
			rectTransform4.offsetMax = new Vector2(6f, -3f);
			Image image = rectTransform4.gameObject.AddComponent<Image>();
			image.color = CielOSStoryApp.ShadowColor;
			image.raycastTarget = false;
			CielOSTheme.Round(image);
			RectTransform rectTransform5 = CielOSStoryApp.NewChild("Frame", rectTransform);
			CielOSStoryApp.Stretch(rectTransform5);
			Image image2 = rectTransform5.gameObject.AddComponent<Image>();
			image2.color = new Color(0.99f, 0.99f, 0.985f, 1f);
			image2.raycastTarget = true;
			CielOSTheme.Round(image2);
			rectTransform5.gameObject.AddComponent<Mask>().showMaskGraphic = true;
			Button button = rectTransform5.gameObject.AddComponent<Button>();
			button.targetGraphic = image2;
			ColorBlock colors = button.colors;
			colors.highlightedColor = new Color(0.93f, 0.93f, 0.93f, 1f);
			colors.pressedColor = new Color(0.82f, 0.82f, 0.82f, 1f);
			button.colors = colors;
			Sprite captured = s.sprite;
			button.onClick.AddListener(delegate
			{
				this.OpenPhotoViewer(captured);
			});
			CielOSSfx.Attach(button);
			RectTransform rectTransform6 = CielOSStoryApp.NewChild("Photo", rectTransform5);
			CielOSStoryApp.Stretch(rectTransform6);
			rectTransform6.offsetMin = new Vector2(6f, 6f);
			rectTransform6.offsetMax = new Vector2(-6f, -6f);
			Image image3 = rectTransform6.gameObject.AddComponent<Image>();
			image3.sprite = s.sprite;
			StreamerCensorImage.Bind(image3, false);
			image3.raycastTarget = false;
			if (!string.IsNullOrEmpty(s.caption))
			{
				TMP_Text tmp_Text = CielOSStoryApp.MakeLabel(rectTransform, s.caption, 25f, FontStyles.Normal, Color.white, TextAlignmentOptions.Center, (this.handFont != null) ? this.handFont : this._font);
				RectTransform rectTransform7 = (RectTransform)tmp_Text.transform;
				rectTransform7.anchorMin = new Vector2(0f, 0f);
				rectTransform7.anchorMax = new Vector2(1f, 0f);
				rectTransform7.pivot = new Vector2(0.5f, 0f);
				rectTransform7.sizeDelta = new Vector2(0f, 38f);
				rectTransform7.anchoredPosition = new Vector2(0f, 10f);
				Shadow shadow = tmp_Text.gameObject.AddComponent<Shadow>();
				shadow.effectColor = new Color(0f, 0f, 0f, 0.65f);
				shadow.effectDistance = new Vector2(2f, -2f);
			}
			return rectTransform;
		}

		// Token: 0x06009F03 RID: 40707 RVA: 0x00418649 File Offset: 0x00416849
		private void OpenPhotoViewer(Sprite sprite)
		{
			if (sprite == null)
			{
				return;
			}
			PhotoPolaroidViewer.Show(sprite);
		}

		// Token: 0x06009F04 RID: 40708 RVA: 0x0041865C File Offset: 0x0041685C
		private static Sprite GridSprite()
		{
			if (CielOSStoryApp._gridSprite != null)
			{
				return CielOSStoryApp._gridSprite;
			}
			Texture2D texture2D = new Texture2D(32, 32, TextureFormat.RGBA32, false);
			Color32[] array = new Color32[1024];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
			}
			for (int j = 0; j < 32; j++)
			{
				array[992 + j] = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			}
			for (int k = 0; k < 32; k++)
			{
				array[k * 32] = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			}
			texture2D.SetPixels32(array);
			texture2D.filterMode = FilterMode.Point;
			texture2D.Apply(false, true);
			CielOSStoryApp._gridSprite = Sprite.Create(texture2D, new Rect(0f, 0f, 32f, 32f), new Vector2(0.5f, 0.5f), 100f);
			CielOSStoryApp._gridSprite.name = "CielOSStory_GridTile";
			return CielOSStoryApp._gridSprite;
		}

		// Token: 0x06009F05 RID: 40709 RVA: 0x002DD477 File Offset: 0x002DB677
		private static RectTransform NewChild(string name, RectTransform parent)
		{
			RectTransform rectTransform = (RectTransform)new GameObject(name, new Type[] { typeof(RectTransform) }).transform;
			rectTransform.SetParent(parent, false);
			return rectTransform;
		}

		// Token: 0x06009F06 RID: 40710 RVA: 0x001A38AD File Offset: 0x001A1AAD
		private static void Stretch(RectTransform rt)
		{
			rt.anchorMin = Vector2.zero;
			rt.anchorMax = Vector2.one;
			rt.offsetMin = Vector2.zero;
			rt.offsetMax = Vector2.zero;
		}

		// Token: 0x06009F07 RID: 40711 RVA: 0x00418788 File Offset: 0x00416988
		private static void PlaceTopLeft(RectTransform rt, float x, float y, float w, float h)
		{
			Vector2 vector = new Vector2(0f, 1f);
			rt.anchorMax = vector;
			rt.anchorMin = vector;
			rt.pivot = new Vector2(0f, 1f);
			rt.sizeDelta = new Vector2(w, h);
			rt.anchoredPosition = new Vector2(x, -y);
		}

		// Token: 0x06009F08 RID: 40712 RVA: 0x004187E8 File Offset: 0x004169E8
		private static TMP_Text MakeLabel(RectTransform parent, string text, float size, FontStyles style, Color color, TextAlignmentOptions align, TMP_FontAsset font)
		{
			TextMeshProUGUI textMeshProUGUI = CielOSStoryApp.NewChild("Label", parent).gameObject.AddComponent<TextMeshProUGUI>();
			if (font != null)
			{
				textMeshProUGUI.font = font;
			}
			textMeshProUGUI.text = text;
			textMeshProUGUI.fontSize = size;
			textMeshProUGUI.fontStyle = style;
			textMeshProUGUI.color = color;
			textMeshProUGUI.alignment = align;
			textMeshProUGUI.overflowMode = TextOverflowModes.Overflow;
			textMeshProUGUI.raycastTarget = false;
			LocalizedGameText.BindComputed(textMeshProUGUI, () => GameLocalization.GetSource("phone_story", text));
			return textMeshProUGUI;
		}

		// Token: 0x06009F09 RID: 40713 RVA: 0x00418878 File Offset: 0x00416A78
		private void CaptureDiarySelection()
		{
			if (this._activeDiaryInput == null || !this._activeDiaryInput.isFocused)
			{
				return;
			}
			if (this._diaryAnchor != this._activeDiaryInput.selectionStringAnchorPosition || this._diaryFocus != this._activeDiaryInput.selectionStringFocusPosition)
			{
				this._diaryTypingMask = -1;
			}
			this._diaryAnchor = this._activeDiaryInput.selectionStringAnchorPosition;
			this._diaryFocus = this._activeDiaryInput.selectionStringFocusPosition;
		}

		// Token: 0x06009F0A RID: 40714 RVA: 0x004188F0 File Offset: 0x00416AF0
		private void RebuildDiaryEditor()
		{
			if (this._diaryDraft == null || !this.CanEditPersonalDiary)
			{
				return;
			}
			this.CaptureDiarySelection();
			float num = ((this._diaryEditorScroll != null && this._diaryEditorScroll.content.rect.height * this._diaryEditorScroll.content.localScale.y > this._diaryEditorScroll.viewport.rect.height + 1f) ? this._diaryEditorScroll.verticalNormalizedPosition : 1f);
			this._activeDiaryInput = null;
			this._diaryInputs.Clear();
			this._diaryRows.Clear();
			this._diaryPhotoRoots.Clear();
			this._diaryFormattingButtons.Clear();
			CielOSStoryApp.RemoveDiaryObject(this._diaryEditor);
			this._diaryEditor = CielOSStoryApp.NewChild("PersonalDiaryEditor", this._self);
			CielOSStoryApp.Stretch(this._diaryEditor);
			this._diaryEditor.gameObject.AddComponent<Image>().color = CielOSStoryApp.Paper;
			ControllerNavigationScope.Configure(this._diaryEditor.gameObject, new Action(this.CloseDiaryEditor), null, 100, false);
			RectTransform rectTransform = CielOSStoryApp.NewChild("EditorHeader", this._diaryEditor);
			this._diaryHeader = rectTransform;
			rectTransform.anchorMin = new Vector2(0f, 1f);
			rectTransform.anchorMax = Vector2.one;
			rectTransform.pivot = new Vector2(0.5f, 1f);
			rectTransform.sizeDelta = new Vector2(-48f, 56f);
			TMP_Text tmp_Text = this.DiaryLabel(rectTransform, "Your story continues", 26f, CielOSStoryApp.DiaryAccent, true);
			tmp_Text.rectTransform.anchorMin = Vector2.zero;
			tmp_Text.rectTransform.anchorMax = new Vector2(0.48f, 1f);
			tmp_Text.rectTransform.offsetMin = (tmp_Text.rectTransform.offsetMax = Vector2.zero);
			this._diaryStatus = this.DiaryLabel(rectTransform, this._diaryDirty ? "Saving..." : "All changes saved", 17f, CielOSStoryApp.InkDim, true);
			this._diaryStatus.rectTransform.anchorMin = new Vector2(0.5f, 0f);
			this._diaryStatus.rectTransform.anchorMax = new Vector2(0.81f, 1f);
			this._diaryStatus.rectTransform.offsetMin = (this._diaryStatus.rectTransform.offsetMax = Vector2.zero);
			if (!string.IsNullOrEmpty(this._diaryStatusMessage))
			{
				this.SetDiaryStatus(this._diaryStatusMessage, this._diaryStatusIsError);
			}
			RectTransform rectTransform2 = (RectTransform)this.DiaryButton(rectTransform, "Save & read", new UnityAction(this.CloseDiaryEditor)).transform;
			rectTransform2.anchorMin = new Vector2(0.83f, 0.12f);
			rectTransform2.anchorMax = new Vector2(1f, 0.88f);
			Vector2 vector = (rectTransform2.offsetMax = Vector2.zero);
			rectTransform2.offsetMin = vector;
			RectTransform rectTransform3 = CielOSStoryApp.NewChild("FormattingToolbar", this._diaryEditor);
			this._diaryToolbar = rectTransform3;
			rectTransform3.anchorMin = new Vector2(0f, 1f);
			rectTransform3.anchorMax = Vector2.one;
			rectTransform3.pivot = new Vector2(0.5f, 1f);
			rectTransform3.sizeDelta = new Vector2(-48f, 42f);
			rectTransform3.anchoredPosition = new Vector2(0f, -60f);
			this.DiaryButton(rectTransform3, "Text", delegate
			{
				this.ApplyDiaryParagraphStyle(PersonalDiaryStore.BlockType.Paragraph);
			});
			this.DiaryButton(rectTransform3, "H1", delegate
			{
				this.ApplyDiaryParagraphStyle(PersonalDiaryStore.BlockType.Heading1);
			});
			this.DiaryButton(rectTransform3, "H2", delegate
			{
				this.ApplyDiaryParagraphStyle(PersonalDiaryStore.BlockType.Heading2);
			});
			this.DiaryButton(rectTransform3, "B", delegate
			{
				this.FormatDiarySelection("b");
			}).GetComponentInChildren<TMP_Text>().fontStyle = FontStyles.Bold;
			this.DiaryButton(rectTransform3, "I", delegate
			{
				this.FormatDiarySelection("i");
			}).GetComponentInChildren<TMP_Text>().fontStyle = FontStyles.Italic;
			this.DiaryButton(rectTransform3, "U", delegate
			{
				this.FormatDiarySelection("u");
			}).GetComponentInChildren<TMP_Text>().fontStyle = FontStyles.Underline;
			this.DiaryButton(rectTransform3, "S", delegate
			{
				this.FormatDiarySelection("s");
			}).GetComponentInChildren<TMP_Text>().fontStyle = FontStyles.Strikethrough;
			this.DiaryButton(rectTransform3, "•", delegate
			{
				this.AddDiaryList(false);
			});
			this.DiaryButton(rectTransform3, "1.", delegate
			{
				this.AddDiaryList(true);
			});
			this.DiaryButton(rectTransform3, "Left", delegate
			{
				this.AlignDiaryText(0);
			});
			this.DiaryButton(rectTransform3, "Center", delegate
			{
				this.AlignDiaryText(1);
			});
			this.DiaryButton(rectTransform3, "Right", delegate
			{
				this.AlignDiaryText(2);
			});
			this._diaryFormattingButtons.AddRange(rectTransform3.GetComponentsInChildren<Button>());
			this.DiaryButton(rectTransform3, "Phone photo", delegate
			{
				this.CaptureDiarySelection();
				this.PickDiaryPhoto(true);
			});
			this.DiaryButton(rectTransform3, "Import image", delegate
			{
				this.CaptureDiarySelection();
				this.PickDiaryPhoto(false);
			});
			RectTransform rectTransform4 = CielOSStoryApp.NewChild("EditorScroll", this._diaryEditor);
			CielOSStoryApp.Stretch(rectTransform4);
			rectTransform4.offsetMin = new Vector2(24f, 60f);
			rectTransform4.offsetMax = new Vector2(-24f, -114f);
			rectTransform4.gameObject.AddComponent<Image>().color = CielOSStoryApp.Paper;
			rectTransform4.gameObject.AddComponent<RectMask2D>();
			this._diaryEditorScroll = rectTransform4.gameObject.AddComponent<ScrollRect>();
			this._diaryEditorScroll.horizontal = false;
			this._diaryEditorScroll.movementType = ScrollRect.MovementType.Clamped;
			this._diarySmoothScroll = CielOSStoryApp.SmoothWheelScroll.Attach(this._diaryEditorScroll);
			RectTransform rectTransform5 = CielOSStoryApp.NewChild("Grid", rectTransform4);
			CielOSStoryApp.Stretch(rectTransform5);
			Image image = rectTransform5.gameObject.AddComponent<Image>();
			image.sprite = CielOSStoryApp.GridSprite();
			image.type = Image.Type.Tiled;
			image.pixelsPerUnitMultiplier = 0.75f;
			image.color = CielOSStoryApp.GridInk;
			Button button = rectTransform5.gameObject.AddComponent<Button>();
			button.targetGraphic = image;
			button.transition = Selectable.Transition.None;
			button.onClick.AddListener(new UnityAction(this.FocusDiaryDocumentEnd));
			RectTransform rectTransform6 = CielOSStoryApp.NewChild("Blocks", rectTransform4);
			this._diaryDocument = rectTransform6;
			RectTransform rectTransform7 = rectTransform6;
			RectTransform rectTransform8 = rectTransform6;
			vector = new Vector2(0.5f, 1f);
			rectTransform8.anchorMax = vector;
			rectTransform7.anchorMin = vector;
			rectTransform6.pivot = new Vector2(0.5f, 1f);
			rectTransform6.sizeDelta = Vector2.zero;
			this._diaryEditorScroll.viewport = rectTransform4;
			this._diaryEditorScroll.content = rectTransform6;
			RectTransform rectTransform9 = CielOSStoryApp.NewChild("Margin", rectTransform6);
			rectTransform9.anchorMin = Vector2.zero;
			rectTransform9.anchorMax = new Vector2(0f, 1f);
			rectTransform9.sizeDelta = new Vector2(2f, 0f);
			rectTransform9.anchoredPosition = new Vector2(102f, 0f);
			rectTransform9.gameObject.AddComponent<Image>().color = CielOSStoryApp.MarginRed;
			rectTransform9.GetComponent<Image>().raycastTarget = false;
			TMP_InputField tmp_InputField = this.DiaryInput(rectTransform6, "Page title", this._diaryDraft.title, 48f, true, 160, delegate(string value)
			{
				this._diaryDraft.title = value;
				this.DiaryChanged();
				this.ReflowDiaryDocument();
			});
			this._diaryTitleInput = tmp_InputField;
			CielOSStoryApp.SetDiaryDocumentField(tmp_InputField);
			tmp_InputField.gameObject.name = "PageTitle";
			tmp_InputField.GetComponent<Image>().color = Color.clear;
			tmp_InputField.textComponent.color = CielOSStoryApp.DiaryAccent;
			tmp_InputField.onSelect.AddListener(delegate(string _)
			{
				this._activeDiaryBlock = null;
				this._activeDiaryInput = null;
				this._diaryTypingMask = -1;
				this.RefreshDiaryPhotoTools();
			});
			if (this._diaryDraft.blocks.Count == 0)
			{
				this._diaryDraft.blocks.Add(CielOSStoryApp.NewDiaryBlock(PersonalDiaryStore.BlockType.Paragraph));
			}
			CielOSStoryApp.DiaryLayout diaryLayout = this.CalculateDiaryLayout(this._diaryDraft, 620f);
			foreach (CielOSStoryApp.DiaryPlacement diaryPlacement in diaryLayout.items)
			{
				if (diaryPlacement.block.type != PersonalDiaryStore.BlockType.Photo)
				{
					this.BuildDiaryDocumentBlock(rectTransform6, diaryPlacement.block);
				}
			}
			foreach (CielOSStoryApp.DiaryPlacement diaryPlacement2 in diaryLayout.items)
			{
				if (diaryPlacement2.block.type == PersonalDiaryStore.BlockType.Photo)
				{
					this._diaryPhotoRoots[diaryPlacement2.block.id] = this.BuildDiaryPhotoVisual(rectTransform6, diaryPlacement2, true);
				}
			}
			RectTransform rectTransform10 = CielOSStoryApp.NewChild("EditorFooter", this._diaryEditor);
			rectTransform10.anchorMin = Vector2.zero;
			rectTransform10.anchorMax = new Vector2(1f, 0f);
			rectTransform10.pivot = new Vector2(0.5f, 0f);
			rectTransform10.sizeDelta = new Vector2(-48f, 44f);
			rectTransform10.anchoredPosition = new Vector2(0f, 8f);
			this._diaryEditorHint = this.DiaryLabel(rectTransform10, "Select text to format it. Drag photos anywhere on the page.", 18f, CielOSStoryApp.InkDim, true);
			CielOSStoryApp.Stretch(this._diaryEditorHint.rectTransform);
			this._diaryEditorHint.GetComponent<LocalizedGameText>().SetSourceLayout(true, true, 12f, 18f);
			this._diaryPhotoTools = CielOSStoryApp.NewChild("PhotoTools", this._diaryEditor);
			this._diaryPhotoTools.anchorMin = new Vector2(0f, 1f);
			this._diaryPhotoTools.anchorMax = Vector2.one;
			this._diaryPhotoTools.pivot = new Vector2(0.5f, 1f);
			this._diaryPhotoTools.sizeDelta = new Vector2(-48f, 36f);
			this._diaryPhotoTools.anchoredPosition = new Vector2(0f, -108f);
			this.DiaryButton(this._diaryPhotoTools, "Smaller photo", delegate
			{
				this.ChangeDiaryPhoto(-120f, 0f);
			});
			this.DiaryButton(this._diaryPhotoTools, "Larger photo", delegate
			{
				this.ChangeDiaryPhoto(120f, 0f);
			});
			this.DiaryButton(this._diaryPhotoTools, "Tilt left", delegate
			{
				this.ChangeDiaryPhoto(0f, -2f);
			});
			this.DiaryButton(this._diaryPhotoTools, "Tilt right", delegate
			{
				this.ChangeDiaryPhoto(0f, 2f);
			});
			this.DiaryButton(this._diaryPhotoTools, "Remove photo", new UnityAction(this.RemoveSelectedDiaryPhoto));
			if (this._activeDiaryBlock == null || !this._diaryDraft.blocks.Contains(this._activeDiaryBlock))
			{
				this._activeDiaryBlock = this._diaryDraft.blocks.Find((PersonalDiaryStore.Block block) => block.type != PersonalDiaryStore.BlockType.Photo);
			}
			if (this._activeDiaryBlock != null)
			{
				this._diaryInputs.TryGetValue(this._activeDiaryBlock.id, out this._activeDiaryInput);
			}
			this.CreatePageScrollbar(this._diaryEditorScroll, this._diaryEditor, new Action(this._diarySmoothScroll.Cancel));
			this.LayoutDiaryEditorChrome();
			this.RefreshDiaryPhotoTools();
			Canvas.ForceUpdateCanvases();
			this.ReflowDiaryDocument();
			this._diaryEditorScroll.verticalNormalizedPosition = num;
		}

		// Token: 0x06009F0B RID: 40715 RVA: 0x00419448 File Offset: 0x00417648
		private void BuildDiaryDocumentBlock(RectTransform content, PersonalDiaryStore.Block block)
		{
			RectTransform rectTransform = CielOSStoryApp.NewChild("Block_" + block.id, content);
			this._diaryRows[block.id] = rectTransform;
			float num = CielOSStoryApp.DiaryFontSize(block);
			TMP_InputField field = null;
			field = this.DiaryInput(rectTransform, "Write here...", block.text, num, true, 12000, delegate(string value)
			{
				this.OnDiaryTextChanged(block, field, value);
			});
			field.GetComponent<Image>().color = Color.clear;
			CielOSStoryApp.Stretch((RectTransform)field.transform);
			CielOSStoryApp.SetDiaryDocumentField(field);
			field.richText = block.richText;
			field.isRichTextEditingAllowed = false;
			field.characterLimit = (block.richText ? 384000 : 12000);
			field.textComponent.richText = block.richText;
			field.textComponent.color = ((block.type == PersonalDiaryStore.BlockType.Paragraph) ? CielOSStoryApp.Ink : CielOSStoryApp.DiaryAccent);
			field.textComponent.alignment = ((block.alignment == 1) ? TextAlignmentOptions.Top : ((block.alignment == 2) ? TextAlignmentOptions.TopRight : TextAlignmentOptions.TopLeft));
			field.onSelect.AddListener(delegate(string _)
			{
				if (this._activeDiaryBlock != block)
				{
					this._diaryTypingMask = -1;
				}
				this._activeDiaryBlock = block;
				this._activeDiaryInput = field;
				this.RefreshDiaryPhotoTools();
			});
			field.onDeselect.AddListener(delegate(string _)
			{
				if (this._activeDiaryInput != field)
				{
					return;
				}
				this._diaryAnchor = field.selectionStringAnchorPosition;
				this._diaryFocus = field.selectionStringFocusPosition;
			});
			this._diaryInputs[block.id] = field;
		}

		// Token: 0x06009F0C RID: 40716 RVA: 0x00419634 File Offset: 0x00417834
		private void OnDiaryTextChanged(PersonalDiaryStore.Block block, TMP_InputField field, string value)
		{
			if (this._changingDiaryText || field == null)
			{
				return;
			}
			string text = block.text;
			if ((block.richText ? PersonalDiaryTextFormatting.ToPlainText(value) : value).Length > 12000)
			{
				this._changingDiaryText = true;
				try
				{
					field.SetTextWithoutNotify(text);
				}
				finally
				{
					this._changingDiaryText = false;
				}
				return;
			}
			block.text = value;
			if (block.richText)
			{
				string text2 = value;
				if (this._diaryTypingMask >= 0 && this._diaryTypingBlock == block.id)
				{
					int num = 0;
					while (num < text.Length && num < value.Length && text[num] == value[num])
					{
						num++;
					}
					int num2 = 0;
					while (num2 < text.Length - num && num2 < value.Length - num && text[text.Length - num2 - 1] == value[value.Length - num2 - 1])
					{
						num2++;
					}
					if (value.Length - num2 > num)
					{
						text2 = PersonalDiaryTextFormatting.SetStyles(value, num, value.Length - num2, this._diaryTypingMask);
					}
				}
				string text3 = PersonalDiaryTextFormatting.Normalize(text2);
				if (text3 != value)
				{
					int num3 = PersonalDiaryTextFormatting.RawToPlainIndex(value, field.selectionStringFocusPosition);
					block.text = text3;
					this._changingDiaryText = true;
					try
					{
						field.SetTextWithoutNotify(text3);
						field.ForceLabelUpdate();
						field.selectionStringAnchorPosition = (field.selectionStringFocusPosition = PersonalDiaryTextFormatting.PlainToRawIndex(text3, num3));
					}
					finally
					{
						this._changingDiaryText = false;
					}
				}
			}
			this._diaryAnchor = field.selectionStringAnchorPosition;
			this._diaryFocus = field.selectionStringFocusPosition;
			this.ResizeDiaryText(field);
			this.DiaryChanged();
		}

		// Token: 0x06009F0D RID: 40717 RVA: 0x004197F8 File Offset: 0x004179F8
		private void ResizeDiaryText(TMP_InputField field)
		{
			this.ReflowDiaryDocument();
		}

		// Token: 0x06009F0E RID: 40718 RVA: 0x00419800 File Offset: 0x00417A00
		private void RefreshDiaryEditorLayout()
		{
			if (this._diaryEditor == null || this._diaryEditorScroll == null)
			{
				return;
			}
			CielOSStoryApp.SmoothWheelScroll diarySmoothScroll = this._diarySmoothScroll;
			if (diarySmoothScroll != null)
			{
				diarySmoothScroll.Cancel();
			}
			float verticalNormalizedPosition = this._diaryEditorScroll.verticalNormalizedPosition;
			this.LayoutDiaryEditorChrome();
			Canvas.ForceUpdateCanvases();
			this.ReflowDiaryDocument();
			this._diaryEditorScroll.verticalNormalizedPosition = verticalNormalizedPosition;
		}

		// Token: 0x06009F0F RID: 40719 RVA: 0x00419864 File Offset: 0x00417A64
		private bool SelectDiaryWriting()
		{
			this.CaptureDiarySelection();
			return this._diaryDraft != null && this.CanEditPersonalDiary && (this._activeDiaryInput != null && this._activeDiaryBlock != null) && this._activeDiaryBlock.type != PersonalDiaryStore.BlockType.Photo;
		}

		// Token: 0x06009F10 RID: 40720 RVA: 0x004198B4 File Offset: 0x00417AB4
		private void FormatDiarySelection(string tag)
		{
			if (!this.SelectDiaryWriting())
			{
				return;
			}
			PersonalDiaryStore.Block activeDiaryBlock = this._activeDiaryBlock;
			int num = this._diaryAnchor;
			int num2 = this._diaryFocus;
			if (!activeDiaryBlock.richText)
			{
				num = PersonalDiaryTextFormatting.PlainToRichIndex(activeDiaryBlock.text, num);
				num2 = PersonalDiaryTextFormatting.PlainToRichIndex(activeDiaryBlock.text, num2);
				activeDiaryBlock.text = PersonalDiaryTextFormatting.Escape(activeDiaryBlock.text);
				activeDiaryBlock.richText = true;
			}
			if (num == num2)
			{
				if (this._diaryTypingMask < 0 || this._diaryTypingBlock != activeDiaryBlock.id)
				{
					this._diaryTypingMask = PersonalDiaryTextFormatting.StyleAt(activeDiaryBlock.text, num2);
				}
				this._diaryTypingBlock = activeDiaryBlock.id;
				this._diaryTypingMask ^= ((tag == "b") ? 1 : ((tag == "i") ? 2 : ((tag == "u") ? 4 : 8)));
				this._diaryAnchor = num;
				this._diaryFocus = num2;
				this.RefreshActiveDiaryText();
				return;
			}
			this._diaryTypingMask = -1;
			activeDiaryBlock.text = PersonalDiaryTextFormatting.Toggle(activeDiaryBlock.text, num, num2, tag, out this._diaryAnchor, out this._diaryFocus);
			this.RefreshActiveDiaryText();
			this.DiaryChanged();
		}

		// Token: 0x06009F11 RID: 40721 RVA: 0x004199E0 File Offset: 0x00417BE0
		private void RefreshActiveDiaryText()
		{
			if (this._activeDiaryInput == null || this._activeDiaryBlock == null)
			{
				return;
			}
			this._changingDiaryText = true;
			try
			{
				this._activeDiaryInput.richText = this._activeDiaryBlock.richText;
				this._activeDiaryInput.characterLimit = (this._activeDiaryBlock.richText ? 384000 : 12000);
				this._activeDiaryInput.textComponent.richText = this._activeDiaryBlock.richText;
				this._activeDiaryInput.SetTextWithoutNotify(this._activeDiaryBlock.text);
				this._activeDiaryInput.ForceLabelUpdate();
				this._activeDiaryInput.textComponent.ForceMeshUpdate(false, false);
				this.RestoreDiaryCaret();
				this.ResizeDiaryText(this._activeDiaryInput);
			}
			finally
			{
				this._changingDiaryText = false;
			}
		}

		// Token: 0x06009F12 RID: 40722 RVA: 0x00419AC0 File Offset: 0x00417CC0
		private void RestoreDiaryCaret()
		{
			if (this._activeDiaryInput == null)
			{
				return;
			}
			if (EventSystem.current != null)
			{
				EventSystem.current.SetSelectedGameObject(this._activeDiaryInput.gameObject);
			}
			this._activeDiaryInput.ActivateInputField();
			this._activeDiaryInput.selectionStringAnchorPosition = Mathf.Clamp(this._diaryAnchor, 0, this._activeDiaryInput.text.Length);
			this._activeDiaryInput.selectionStringFocusPosition = Mathf.Clamp(this._diaryFocus, 0, this._activeDiaryInput.text.Length);
		}

		// Token: 0x06009F13 RID: 40723 RVA: 0x00419B57 File Offset: 0x00417D57
		private string DiaryPlain(PersonalDiaryStore.Block block)
		{
			if (!block.richText)
			{
				return block.text;
			}
			return PersonalDiaryTextFormatting.ToPlainText(block.text);
		}

		// Token: 0x06009F14 RID: 40724 RVA: 0x00419B73 File Offset: 0x00417D73
		private int DiaryRawIndex(PersonalDiaryStore.Block block, int plain)
		{
			if (!block.richText)
			{
				return plain;
			}
			return PersonalDiaryTextFormatting.PlainToRawIndex(block.text, plain);
		}

		// Token: 0x06009F15 RID: 40725 RVA: 0x00419B8B File Offset: 0x00417D8B
		private string DiarySlice(PersonalDiaryStore.Block block, int start, int end)
		{
			if (!block.richText)
			{
				return block.text.Substring(start, end - start);
			}
			return PersonalDiaryTextFormatting.Slice(block.text, start, end);
		}

		// Token: 0x06009F16 RID: 40726 RVA: 0x00419BB4 File Offset: 0x00417DB4
		private void ApplyDiaryParagraphStyle(PersonalDiaryStore.BlockType type)
		{
			if (!this.SelectDiaryWriting())
			{
				return;
			}
			PersonalDiaryStore.Block activeDiaryBlock = this._activeDiaryBlock;
			string text = this.DiaryPlain(activeDiaryBlock);
			int num = (activeDiaryBlock.richText ? PersonalDiaryTextFormatting.RawToPlainIndex(activeDiaryBlock.text, this._diaryFocus) : this._diaryFocus);
			num = Mathf.Clamp(num, 0, text.Length);
			int num2 = ((num > 0) ? (text.LastIndexOf('\n', num - 1) + 1) : 0);
			int num3 = text.IndexOf('\n', num);
			if (num3 < 0)
			{
				num3 = text.Length;
			}
			if (num2 == 0 && num3 == text.Length)
			{
				activeDiaryBlock.type = type;
			}
			else
			{
				if (this._diaryDraft.blocks.Count + 2 > 100)
				{
					return;
				}
				int num4 = this._diaryDraft.blocks.IndexOf(activeDiaryBlock);
				List<PersonalDiaryStore.Block> list = new List<PersonalDiaryStore.Block>();
				if (num2 > 0)
				{
					list.Add(this.CopyDiaryTextPart(activeDiaryBlock, 0, this.DiaryRawIndex(activeDiaryBlock, num2 - 1), activeDiaryBlock.type));
				}
				PersonalDiaryStore.Block block = this.CopyDiaryTextPart(activeDiaryBlock, this.DiaryRawIndex(activeDiaryBlock, num2), this.DiaryRawIndex(activeDiaryBlock, num3), type);
				list.Add(block);
				if (num3 < text.Length)
				{
					list.Add(this.CopyDiaryTextPart(activeDiaryBlock, this.DiaryRawIndex(activeDiaryBlock, num3 + 1), activeDiaryBlock.text.Length, activeDiaryBlock.type));
				}
				this._diaryDraft.blocks.RemoveAt(num4);
				this._diaryDraft.blocks.InsertRange(num4, list);
				this._activeDiaryBlock = block;
			}
			this._diaryAnchor = (this._diaryFocus = this._activeDiaryBlock.text.Length);
			this.DiaryChanged();
			this.RebuildDiaryEditor();
			this.RestoreDiaryCaret();
		}

		// Token: 0x06009F17 RID: 40727 RVA: 0x00419D5C File Offset: 0x00417F5C
		private PersonalDiaryStore.Block CopyDiaryTextPart(PersonalDiaryStore.Block source, int start, int end, PersonalDiaryStore.BlockType type)
		{
			return new PersonalDiaryStore.Block
			{
				type = type,
				text = this.DiarySlice(source, start, end),
				richText = source.richText,
				alignment = source.alignment
			};
		}

		// Token: 0x06009F18 RID: 40728 RVA: 0x00419D94 File Offset: 0x00417F94
		private void AddDiaryList(bool numbered)
		{
			if (!this.SelectDiaryWriting())
			{
				return;
			}
			PersonalDiaryStore.Block activeDiaryBlock = this._activeDiaryBlock;
			string text = this.DiaryPlain(activeDiaryBlock);
			int num = (activeDiaryBlock.richText ? PersonalDiaryTextFormatting.RawToPlainIndex(activeDiaryBlock.text, this._diaryFocus) : this._diaryFocus);
			num = Mathf.Clamp(num, 0, text.Length);
			int num2 = ((num > 0) ? (text.LastIndexOf('\n', num - 1) + 1) : 0);
			int num3 = this.DiaryRawIndex(activeDiaryBlock, num2);
			string text2 = (numbered ? "1. " : "• ");
			activeDiaryBlock.text = this.DiarySlice(activeDiaryBlock, 0, num3) + text2 + this.DiarySlice(activeDiaryBlock, num3, activeDiaryBlock.text.Length);
			this._diaryAnchor = (this._diaryFocus = this.DiaryRawIndex(activeDiaryBlock, num + text2.Length));
			this.RefreshActiveDiaryText();
			this.DiaryChanged();
		}

		// Token: 0x06009F19 RID: 40729 RVA: 0x00419E71 File Offset: 0x00418071
		private void AlignDiaryText(int alignment)
		{
			if (!this.SelectDiaryWriting())
			{
				return;
			}
			this._activeDiaryBlock.alignment = alignment;
			this.DiaryChanged();
			this.RebuildDiaryEditor();
			this.RestoreDiaryCaret();
		}

		// Token: 0x06009F1A RID: 40730 RVA: 0x00419E9C File Offset: 0x0041809C
		private void InsertDiaryPhotoBlock(PersonalDiaryStore.Block photo)
		{
			if (this._diaryDraft == null)
			{
				return;
			}
			if (this._activeDiaryBlock != null && this._activeDiaryBlock.type != PersonalDiaryStore.BlockType.Photo && this._diaryDraft.blocks.Contains(this._activeDiaryBlock) && this._diaryDraft.blocks.Count + 2 <= 100)
			{
				PersonalDiaryStore.Block activeDiaryBlock = this._activeDiaryBlock;
				int num = Mathf.Clamp(this._diaryFocus, 0, activeDiaryBlock.text.Length);
				int num2 = this._diaryDraft.blocks.IndexOf(activeDiaryBlock);
				PersonalDiaryStore.Block block = this.CopyDiaryTextPart(activeDiaryBlock, 0, num, activeDiaryBlock.type);
				PersonalDiaryStore.Block block2 = this.CopyDiaryTextPart(activeDiaryBlock, num, activeDiaryBlock.text.Length, PersonalDiaryStore.BlockType.Paragraph);
				this._diaryDraft.blocks.RemoveAt(num2);
				if (!string.IsNullOrEmpty(this.DiaryPlain(block)))
				{
					this._diaryDraft.blocks.Insert(num2++, block);
				}
				this._diaryDraft.blocks.Insert(num2++, photo);
				this._diaryDraft.blocks.Insert(num2, block2);
				this._activeDiaryBlock = block2;
				this._diaryAnchor = (this._diaryFocus = 0);
				return;
			}
			this._diaryDraft.blocks.Add(photo);
			if (this._diaryDraft.blocks.Count < 100)
			{
				this._diaryDraft.blocks.Add(CielOSStoryApp.NewDiaryBlock(PersonalDiaryStore.BlockType.Paragraph));
			}
			this._activeDiaryBlock = photo;
		}

		// Token: 0x06009F1B RID: 40731 RVA: 0x0041A014 File Offset: 0x00418214
		private void SelectDiaryPhoto(PersonalDiaryStore.Block block)
		{
			this.CaptureDiarySelection();
			this._activeDiaryBlock = block;
			this._activeDiaryInput = null;
			this.RefreshDiaryPhotoTools();
		}

		// Token: 0x06009F1C RID: 40732 RVA: 0x0041A030 File Offset: 0x00418230
		private void RefreshDiaryPhotoTools()
		{
			bool flag = this._activeDiaryBlock != null && this._activeDiaryBlock.type == PersonalDiaryStore.BlockType.Photo;
			if (this._diaryPhotoTools != null)
			{
				Button[] componentsInChildren = this._diaryPhotoTools.GetComponentsInChildren<Button>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].interactable = flag;
				}
			}
			foreach (Button button in this._diaryFormattingButtons)
			{
				button.interactable = this._activeDiaryInput != null;
			}
			foreach (KeyValuePair<string, RectTransform> keyValuePair in this._diaryPhotoRoots)
			{
				if (!(keyValuePair.Value == null))
				{
					Outline componentInChildren = keyValuePair.Value.GetComponentInChildren<Outline>();
					if (componentInChildren != null)
					{
						componentInChildren.enabled = flag && keyValuePair.Key == this._activeDiaryBlock.id;
					}
				}
			}
		}

		// Token: 0x06009F1D RID: 40733 RVA: 0x0041A160 File Offset: 0x00418360
		private void FocusDiaryDocumentEnd()
		{
			if (this._diaryDraft == null)
			{
				return;
			}
			PersonalDiaryStore.Block block = ((this._diaryDraft.blocks.Count > 0) ? this._diaryDraft.blocks[this._diaryDraft.blocks.Count - 1] : null);
			if (block == null || block.type == PersonalDiaryStore.BlockType.Photo)
			{
				this.AddDiaryBlock(PersonalDiaryStore.BlockType.Paragraph);
				return;
			}
			this._activeDiaryBlock = block;
			this._diaryInputs.TryGetValue(block.id, out this._activeDiaryInput);
			this._diaryAnchor = (this._diaryFocus = block.text.Length);
			this.RestoreDiaryCaret();
		}

		// Token: 0x06009F1E RID: 40734 RVA: 0x0041A204 File Offset: 0x00418404
		private void ChangeDiaryPhoto(float size, float rotation)
		{
			if (this._activeDiaryBlock == null || this._activeDiaryBlock.type != PersonalDiaryStore.BlockType.Photo)
			{
				return;
			}
			this._activeDiaryBlock.imageWidth = Mathf.Clamp(this._activeDiaryBlock.imageWidth + size, 240f, 900f);
			this._activeDiaryBlock.rotation = Mathf.Clamp(this._activeDiaryBlock.rotation + rotation, -12f, 12f);
			this.DiaryChanged();
			this.RebuildDiaryEditor();
		}

		// Token: 0x06009F1F RID: 40735 RVA: 0x0041A284 File Offset: 0x00418484
		private void RemoveSelectedDiaryPhoto()
		{
			if (this._activeDiaryBlock == null || this._activeDiaryBlock.type != PersonalDiaryStore.BlockType.Photo)
			{
				return;
			}
			PersonalDiaryStore.Block photo = this._activeDiaryBlock;
			this.ConfirmDiaryAction("Remove this block?", delegate
			{
				this._diaryDraft.blocks.Remove(photo);
				this._activeDiaryBlock = null;
				this.DiaryChanged();
				this.RebuildDiaryEditor();
			});
		}

		// Token: 0x06009F20 RID: 40736 RVA: 0x0041A2D8 File Offset: 0x004184D8
		private void DropDiaryPhoto(string id, int index)
		{
			if (this._diaryDraft == null || !this.CanEditPersonalDiary)
			{
				return;
			}
			PersonalDiaryStore.Block block = this._diaryDraft.blocks.Find((PersonalDiaryStore.Block item) => item.id == id);
			if (block == null)
			{
				return;
			}
			this._diaryDraft.blocks.Remove(block);
			this._diaryDraft.blocks.Insert(Mathf.Clamp(index, 0, this._diaryDraft.blocks.Count), block);
			this._activeDiaryBlock = block;
			this.DiaryChanged();
			this.RebuildDiaryEditor();
		}

		// Token: 0x06009F21 RID: 40737 RVA: 0x0041A374 File Offset: 0x00418574
		private void AddDiaryBlock(PersonalDiaryStore.BlockType type)
		{
			if (this._diaryDraft == null || !this.CanEditPersonalDiary || this._diaryDraft.blocks.Count >= 100)
			{
				return;
			}
			this._activeDiaryBlock = CielOSStoryApp.NewDiaryBlock(type);
			this._diaryDraft.blocks.Add(this._activeDiaryBlock);
			this._diaryAnchor = (this._diaryFocus = 0);
			this.DiaryChanged();
			this.RebuildDiaryEditor();
			this.RestoreDiaryCaret();
			this._diaryEditorScroll.verticalNormalizedPosition = 0f;
		}

		// Token: 0x06009F22 RID: 40738 RVA: 0x0041A3FC File Offset: 0x004185FC
		private void MoveDiaryBlock(int index, int delta)
		{
			if (this._diaryDraft == null || index < 0 || index >= this._diaryDraft.blocks.Count || index + delta < 0 || index + delta >= this._diaryDraft.blocks.Count)
			{
				return;
			}
			this.DropDiaryPhoto(this._diaryDraft.blocks[index].id, index + delta);
		}

		// Token: 0x06009F23 RID: 40739 RVA: 0x0041A463 File Offset: 0x00418663
		private static float DiaryScale(float width)
		{
			return Mathf.Max(1f, width) / 1560f;
		}

		// Token: 0x06009F24 RID: 40740 RVA: 0x0041A476 File Offset: 0x00418676
		private static float DiaryFontSize(PersonalDiaryStore.Block block)
		{
			if (block.type == PersonalDiaryStore.BlockType.Heading1)
			{
				return 48f;
			}
			if (block.type != PersonalDiaryStore.BlockType.Heading2)
			{
				return 27f;
			}
			return 36f;
		}

		// Token: 0x06009F25 RID: 40741 RVA: 0x0041A49C File Offset: 0x0041869C
		private float MeasureDiaryText(string text, float size, float width, bool rich)
		{
			if (this._diaryMeasure == null)
			{
				this._diaryMeasure = this.DiaryLabel(this._self, "", size, CielOSStoryApp.Ink, false);
				this._diaryMeasure.name = "DiaryMeasurement";
				this._diaryMeasure.gameObject.SetActive(false);
			}
			this._diaryMeasure.GetComponent<LocalizedGameText>().Refresh();
			this._diaryMeasure.fontSize = size;
			this._diaryMeasure.richText = rich;
			this._diaryMeasure.lineSpacing = 6f;
			return Mathf.Max(size * 1.25f, this._diaryMeasure.GetPreferredValues(string.IsNullOrEmpty(text) ? " " : text, width, 0f).y);
		}

		// Token: 0x06009F26 RID: 40742 RVA: 0x0041A564 File Offset: 0x00418764
		private CielOSStoryApp.DiaryLayout CalculateDiaryLayout(PersonalDiaryStore.Page page, float minimumHeight)
		{
			CielOSStoryApp.DiaryLayout diaryLayout = new CielOSStoryApp.DiaryLayout();
			float num = 1360f;
			diaryLayout.titleHeight = this.MeasureDiaryText(page.title, 48f, num, false) + 26f;
			float num2 = 64f + diaryLayout.titleHeight;
			float num3 = num2;
			foreach (PersonalDiaryStore.Block block in page.blocks)
			{
				CielOSStoryApp.DiaryPlacement diaryPlacement = new CielOSStoryApp.DiaryPlacement
				{
					block = block
				};
				if (block.type == PersonalDiaryStore.BlockType.Photo)
				{
					Sprite sprite = this.LoadDiarySprite(block.imageFileName);
					float num4 = Mathf.Clamp(block.imageWidth, 240f, 900f);
					float num5 = ((sprite != null) ? (num4 * sprite.rect.height / Mathf.Max(1f, sprite.rect.width)) : (num4 * 0.6f));
					if (num5 > 900f)
					{
						num4 *= 900f / num5;
						num5 = 900f;
					}
					diaryPlacement.photoHeight = num5;
					diaryPlacement.captionHeight = Mathf.Max(48f, this.MeasureDiaryText(block.caption, 24f, num4, false) + 12f);
					diaryPlacement.size = new Vector2(num4 + 12f, num5 + 12f + diaryPlacement.captionHeight + 8f);
					Vector2 vector = PersonalDiaryPhotoLayout.RotatedHalfExtents(diaryPlacement.size, block.rotation);
					diaryPlacement.center = (block.freePosition ? block.photoPosition : new Vector2(120f + num * 0.5f, num2 + 26f + vector.y));
					diaryPlacement.center = PersonalDiaryPhotoLayout.ClampPosition(diaryPlacement.center, diaryPlacement.size, block.rotation, 1560f, 12f, 12f, 100000f);
					float num6 = diaryPlacement.center.y + vector.y;
					num3 = Mathf.Max(num3, num6);
					if (!block.freePosition)
					{
						num2 = num6 + 26f + 18f;
					}
				}
				else
				{
					bool flag = block.type > PersonalDiaryStore.BlockType.Paragraph;
					float num7 = this.MeasureDiaryText(block.text, CielOSStoryApp.DiaryFontSize(block), num, block.richText) + (flag ? 26f : 6f);
					diaryPlacement.textRect = new Rect(120f, num2, num, num7);
					num2 += num7 + 18f;
					num3 = Mathf.Max(num3, num2);
				}
				diaryLayout.items.Add(diaryPlacement);
			}
			diaryLayout.height = Mathf.Max(minimumHeight, num3 + 96f);
			return diaryLayout;
		}

		// Token: 0x06009F27 RID: 40743 RVA: 0x0041A844 File Offset: 0x00418A44
		private RectTransform BuildDiaryPhotoVisual(RectTransform parent, CielOSStoryApp.DiaryPlacement item, bool editor)
		{
			PersonalDiaryStore.Block block = item.block;
			RectTransform rectTransform = CielOSStoryApp.NewChild(editor ? ("Block_" + block.id) : "Sticker", parent);
			RectTransform rectTransform2 = rectTransform;
			RectTransform rectTransform3 = rectTransform;
			Vector2 vector = new Vector2(0f, 1f);
			rectTransform3.anchorMax = vector;
			rectTransform2.anchorMin = vector;
			rectTransform.pivot = new Vector2(0.5f, 0.5f);
			rectTransform.sizeDelta = item.size;
			rectTransform.anchoredPosition = new Vector2(item.center.x, -item.center.y);
			rectTransform.localEulerAngles = new Vector3(0f, 0f, -block.rotation);
			RectTransform rectTransform4 = CielOSStoryApp.NewChild("Frame", rectTransform);
			rectTransform4.anchorMin = new Vector2(0f, 1f);
			rectTransform4.anchorMax = Vector2.one;
			rectTransform4.pivot = new Vector2(0.5f, 1f);
			rectTransform4.sizeDelta = new Vector2(0f, item.photoHeight + 12f);
			Image image2 = rectTransform4.gameObject.AddComponent<Image>();
			image2.color = new Color(0.99f, 0.99f, 0.985f, 1f);
			CielOSTheme.Round(image2);
			Shadow shadow = rectTransform4.gameObject.AddComponent<Shadow>();
			shadow.effectColor = CielOSStoryApp.ShadowColor;
			shadow.effectDistance = new Vector2(4f, -5f);
			rectTransform4.gameObject.AddComponent<Mask>().showMaskGraphic = true;
			RectTransform rectTransform5 = CielOSStoryApp.NewChild("Photo", rectTransform4);
			CielOSStoryApp.Stretch(rectTransform5);
			rectTransform5.offsetMin = new Vector2(6f, 6f);
			rectTransform5.offsetMax = new Vector2(-6f, -6f);
			Image image = rectTransform5.gameObject.AddComponent<Image>();
			image.sprite = this.LoadDiarySprite(block.imageFileName);
			image.raycastTarget = false;
			StreamerCensorImage.Bind(image, block.sensitiveImage);
			if (image.sprite == null)
			{
				TMP_Text tmp_Text = this.DiaryLabel(rectTransform4, "Image unavailable", 24f, CielOSStoryApp.InkDim, true);
				CielOSStoryApp.Stretch(tmp_Text.rectTransform);
				tmp_Text.alignment = TextAlignmentOptions.Center;
			}
			if (editor)
			{
				Button button = rectTransform.gameObject.AddComponent<Button>();
				button.targetGraphic = image2;
				button.onClick.AddListener(delegate
				{
					this.SelectDiaryPhoto(block);
				});
				PersonalDiaryPhotoDrag.Configure(rectTransform, parent, this._diaryEditorScroll, delegate(Vector2 position)
				{
					this.MoveDiaryPhotoFreely(block, position);
				}, delegate
				{
					this.BeginDiaryPhotoDrag(block);
				}, new Action(this.EndDiaryPhotoDrag), 12f, 12f);
				Outline outline = rectTransform4.gameObject.AddComponent<Outline>();
				outline.effectDistance = new Vector2(2f, -2f);
				outline.effectColor = CielOSStoryApp.DiaryAccent;
				outline.enabled = this._activeDiaryBlock == block;
			}
			else
			{
				Button button2 = rectTransform4.gameObject.AddComponent<Button>();
				button2.targetGraphic = image2;
				button2.onClick.AddListener(delegate
				{
					this.OpenPhotoViewer(image.sprite);
				});
			}
			if (editor)
			{
				TMP_InputField tmp_InputField = this.DiaryInput(rectTransform, "Photo caption", block.caption, 24f, true, 500, delegate(string value)
				{
					block.caption = value;
					this.DiaryChanged();
					this.ReflowDiaryDocument();
				});
				tmp_InputField.gameObject.name = "Caption";
				tmp_InputField.GetComponent<Image>().color = Color.clear;
				CielOSStoryApp.SetDiaryDocumentField(tmp_InputField);
				tmp_InputField.textComponent.alignment = TextAlignmentOptions.Top;
				CielOSStoryApp.PlaceTopLeft((RectTransform)tmp_InputField.transform, 6f, item.photoHeight + 12f + 8f, item.size.x - 12f, item.captionHeight);
				tmp_InputField.onSelect.AddListener(delegate(string _)
				{
					this.SelectDiaryPhoto(block);
				});
			}
			else if (!string.IsNullOrEmpty(block.caption))
			{
				TMP_Text tmp_Text2 = this.DiaryLabel(rectTransform, block.caption, 24f, CielOSStoryApp.InkDim, false);
				tmp_Text2.alignment = TextAlignmentOptions.Top;
				tmp_Text2.lineSpacing = 6f;
				CielOSStoryApp.PlaceTopLeft(tmp_Text2.rectTransform, 6f, item.photoHeight + 12f + 8f, item.size.x - 12f, item.captionHeight);
			}
			return rectTransform;
		}

		// Token: 0x06009F28 RID: 40744 RVA: 0x0041ACC0 File Offset: 0x00418EC0
		private void MoveDiaryPhotoFreely(PersonalDiaryStore.Block block, Vector2 position)
		{
			if (this._diaryDraft == null || !this.CanEditPersonalDiary || !this._diaryDraft.blocks.Contains(block))
			{
				return;
			}
			block.freePosition = true;
			block.photoPosition = position;
			RectTransform rectTransform;
			if (this._diaryPhotoRoots.TryGetValue(block.id, out rectTransform) && rectTransform != null)
			{
				block.photoPosition = PersonalDiaryPhotoLayout.ClampPosition(position, rectTransform.sizeDelta, block.rotation, 1560f, 12f, 12f, 100000f);
			}
			this.DiaryChanged();
			this.ReflowDiaryDocument();
		}

		// Token: 0x06009F29 RID: 40745 RVA: 0x0041AD58 File Offset: 0x00418F58
		private void BeginDiaryPhotoDrag(PersonalDiaryStore.Block block)
		{
			CielOSStoryApp.SmoothWheelScroll diarySmoothScroll = this._diarySmoothScroll;
			if (diarySmoothScroll != null)
			{
				diarySmoothScroll.Cancel();
			}
			this._diaryPhotoDragging = true;
			if (this._diaryDocument != null)
			{
				this._diaryRetainedDragHeight = Mathf.Max(this._diaryRetainedDragHeight, this._diaryDocument.rect.height);
			}
			this.SelectDiaryPhoto(block);
		}

		// Token: 0x06009F2A RID: 40746 RVA: 0x0041ADB6 File Offset: 0x00418FB6
		private void EndDiaryPhotoDrag()
		{
			this._diaryPhotoDragging = false;
			this.ReflowDiaryDocument();
			if (this._diaryDirty)
			{
				this.FlushDiaryChanges();
			}
		}

		// Token: 0x06009F2B RID: 40747 RVA: 0x0041ADD4 File Offset: 0x00418FD4
		private void ReflowDiaryDocument()
		{
			if (this._diaryDocument == null || this._diaryEditorScroll == null || this._diaryDraft == null)
			{
				return;
			}
			if (this._diaryHeightDocument != this._diaryDocument)
			{
				this._diaryHeightDocument = this._diaryDocument;
				this._diaryRetainedDragHeight = 0f;
				this._diaryPhotoDragging = false;
			}
			float num = CielOSStoryApp.DiaryScale(this._diaryEditorScroll.viewport.rect.width);
			this._diaryDocument.localScale = Vector3.one * num;
			int num2 = Mathf.Clamp(Mathf.CeilToInt(2f / num), 2, 5);
			foreach (TMP_InputField tmp_InputField in this._diaryInputs.Values)
			{
				if (tmp_InputField != null)
				{
					tmp_InputField.caretWidth = num2;
				}
			}
			if (this._diaryTitleInput != null)
			{
				this._diaryTitleInput.caretWidth = num2;
			}
			CielOSStoryApp.DiaryLayout diaryLayout = this.CalculateDiaryLayout(this._diaryDraft, this._diaryEditorScroll.viewport.rect.height / num);
			if (this._diaryPhotoDragging)
			{
				this._diaryRetainedDragHeight = Mathf.Max(new float[]
				{
					this._diaryRetainedDragHeight,
					this._diaryDocument.rect.height,
					diaryLayout.height
				});
			}
			diaryLayout.height = Mathf.Max(diaryLayout.height, this._diaryRetainedDragHeight);
			this._diaryDocument.sizeDelta = new Vector2(1560f, diaryLayout.height);
			if (this._diaryTitleInput != null)
			{
				CielOSStoryApp.PlaceTopLeft((RectTransform)this._diaryTitleInput.transform, 120f, 64f, 1360f, diaryLayout.titleHeight);
			}
			foreach (CielOSStoryApp.DiaryPlacement diaryPlacement in diaryLayout.items)
			{
				RectTransform rectTransform3;
				if (diaryPlacement.block.type == PersonalDiaryStore.BlockType.Photo)
				{
					RectTransform rectTransform;
					if (this._diaryPhotoRoots.TryGetValue(diaryPlacement.block.id, out rectTransform) && rectTransform != null)
					{
						rectTransform.anchoredPosition = new Vector2(diaryPlacement.center.x, -diaryPlacement.center.y);
						rectTransform.sizeDelta = diaryPlacement.size;
						RectTransform rectTransform2 = rectTransform.Find("Caption") as RectTransform;
						if (rectTransform2 != null)
						{
							CielOSStoryApp.PlaceTopLeft(rectTransform2, 6f, diaryPlacement.photoHeight + 12f + 8f, diaryPlacement.size.x - 12f, diaryPlacement.captionHeight);
						}
					}
				}
				else if (this._diaryRows.TryGetValue(diaryPlacement.block.id, out rectTransform3) && rectTransform3 != null)
				{
					CielOSStoryApp.PlaceTopLeft(rectTransform3, diaryPlacement.textRect.x, diaryPlacement.textRect.y, diaryPlacement.textRect.width, diaryPlacement.textRect.height);
				}
			}
		}

		// Token: 0x06009F2C RID: 40748 RVA: 0x0041B144 File Offset: 0x00419344
		private static void SetDiaryDocumentField(TMP_InputField field)
		{
			field.textViewport.offsetMin = (field.textViewport.offsetMax = Vector2.zero);
			field.textComponent.lineSpacing = 6f;
			field.textComponent.alignment = TextAlignmentOptions.TopLeft;
			TMP_Text tmp_Text = field.placeholder as TMP_Text;
			if (tmp_Text != null)
			{
				tmp_Text.alignment = TextAlignmentOptions.TopLeft;
			}
		}

		// Token: 0x06009F2D RID: 40749 RVA: 0x0041B1AC File Offset: 0x004193AC
		private void LayoutDiaryEditorChrome()
		{
			if (this._diaryEditor == null || this._diaryToolbar == null)
			{
				return;
			}
			float num = Mathf.Max(240f, this._self.rect.width);
			float num2 = ((num < 800f) ? 16f : 24f);
			float num3 = num - num2 * 2f;
			bool flag = num < 760f;
			float num4 = (flag ? 76f : 56f);
			this._diaryHeader.sizeDelta = new Vector2(-num2 * 2f, num4);
			TMP_Text[] componentsInChildren = this._diaryHeader.GetComponentsInChildren<TMP_Text>();
			RectTransform rectTransform = componentsInChildren[0].rectTransform;
			rectTransform.anchorMin = new Vector2(0f, flag ? 0.42f : 0f);
			rectTransform.anchorMax = new Vector2(flag ? 0.64f : 0.46f, 1f);
			rectTransform.offsetMin = (rectTransform.offsetMax = Vector2.zero);
			componentsInChildren[0].GetComponent<LocalizedGameText>().SetSourceLayout(true, true, 15f, 26f);
			this._diaryStatus.rectTransform.anchorMin = (flag ? Vector2.zero : new Vector2(0.48f, 0f));
			this._diaryStatus.rectTransform.anchorMax = new Vector2(flag ? 0.66f : 0.8f, flag ? 0.42f : 1f);
			this._diaryStatus.rectTransform.offsetMin = (this._diaryStatus.rectTransform.offsetMax = Vector2.zero);
			this._diaryStatus.GetComponent<LocalizedGameText>().SetSourceLayout(true, true, 12f, 17f);
			RectTransform rectTransform2 = (RectTransform)this._diaryHeader.Find("Save & read");
			rectTransform2.anchorMin = new Vector2(flag ? 0.7f : 0.83f, 0.14f);
			rectTransform2.anchorMax = new Vector2(1f, 0.88f);
			rectTransform2.offsetMin = (rectTransform2.offsetMax = Vector2.zero);
			float num5 = num4 + 4f;
			num5 += CielOSStoryApp.WrapDiaryToolbar(this._diaryToolbar, num3, num2, num5, false) + 6f;
			num5 += CielOSStoryApp.WrapDiaryToolbar(this._diaryPhotoTools, num3, num2, num5, true) + 10f;
			this._diaryChromeBottom = num5;
			RectTransform rectTransform3 = (RectTransform)this._diaryEditorScroll.transform;
			rectTransform3.offsetMin = new Vector2(num2, 48f);
			rectTransform3.offsetMax = new Vector2(-num2, -num5);
			if (this._diaryEditorScroll.verticalScrollbar != null)
			{
				RectTransform rectTransform4 = (RectTransform)this._diaryEditorScroll.verticalScrollbar.transform;
				rectTransform4.offsetMin = new Vector2(-14f, 48f);
				rectTransform4.offsetMax = new Vector2(-2f, -num5);
			}
		}

		// Token: 0x06009F2E RID: 40750 RVA: 0x0041B488 File Offset: 0x00419688
		private static float WrapDiaryToolbar(RectTransform toolbar, float width, float padding, float top, bool photo)
		{
			if (toolbar == null)
			{
				return 0f;
			}
			float num = (photo ? 32f : 38f);
			float num2 = 0f;
			float num3 = 0f;
			toolbar.sizeDelta = new Vector2(-padding * 2f, num);
			toolbar.anchoredPosition = new Vector2(0f, -top);
			for (int i = 0; i < toolbar.childCount; i++)
			{
				RectTransform rectTransform = toolbar.GetChild(i) as RectTransform;
				float num4 = (photo ? Mathf.Min(128f, Mathf.Max(90f, (width - 20f) / 5f)) : ((i >= 12) ? 136f : ((i >= 9) ? 80f : ((i <= 2) ? 62f : 46f))));
				num4 = Mathf.Min(num4, width);
				if (num2 > 0f && num2 + num4 > width + 0.1f)
				{
					num2 = 0f;
					num3 += num + 5f;
				}
				CielOSStoryApp.PlaceTopLeft(rectTransform, num2, num3, num4, num);
				num2 += num4 + 5f;
			}
			float num5 = num3 + num;
			toolbar.sizeDelta = new Vector2(-padding * 2f, num5);
			return num5;
		}

		// Token: 0x170018EA RID: 6378
		// (get) Token: 0x06009F2F RID: 40751 RVA: 0x0041B5C0 File Offset: 0x004197C0
		private bool CanEditPersonalDiary
		{
			get
			{
				return this.UseProgression && RankStoryProgression.IsPersonalDiaryUnlocked;
			}
		}

		// Token: 0x170018EB RID: 6379
		// (get) Token: 0x06009F30 RID: 40752 RVA: 0x0041B5D1 File Offset: 0x004197D1
		private bool DiaryOwnsInput
		{
			get
			{
				return this._diaryEditor != null || this._diaryConfirmation != null || (CielOSFileBrowser.Instance != null && CielOSFileBrowser.Instance.gameObject.activeInHierarchy);
			}
		}

		// Token: 0x170018EC RID: 6380
		// (get) Token: 0x06009F31 RID: 40753 RVA: 0x0041B60F File Offset: 0x0041980F
		private int PersonalPageCount
		{
			get
			{
				if (!this.CanEditPersonalDiary || !this.EnsureDiaryLoaded())
				{
					return 0;
				}
				return this._diaryStore.PageCount;
			}
		}

		// Token: 0x170018ED RID: 6381
		// (get) Token: 0x06009F32 RID: 40754 RVA: 0x0041B62E File Offset: 0x0041982E
		private bool IsPersonalPage
		{
			get
			{
				return this._page >= this.AccessibleAuthoredPageCount && this._page < this.PageCount;
			}
		}

		// Token: 0x170018EE RID: 6382
		// (get) Token: 0x06009F33 RID: 40755 RVA: 0x0041B64E File Offset: 0x0041984E
		private PersonalDiaryStore.Page CurrentPersonalPage
		{
			get
			{
				if (!this.IsPersonalPage)
				{
					return null;
				}
				return this._diaryStore.GetPage(this._page - this.AccessibleAuthoredPageCount);
			}
		}

		// Token: 0x06009F34 RID: 40756 RVA: 0x0041B674 File Offset: 0x00419874
		private bool EnsureDiaryLoaded()
		{
			if (this._diaryLoaded)
			{
				return string.IsNullOrEmpty(this._diaryLoadError);
			}
			this._diaryLoaded = true;
			if (this._diaryStore == null)
			{
				this._diaryStore = new PersonalDiaryStore(null);
			}
			if (!this._diaryStore.TryLoad(out this._diaryLoadError))
			{
				return false;
			}
			this._diaryRecoveredBackup = !string.IsNullOrEmpty(this._diaryStore.LastLoadWarning);
			return true;
		}

		// Token: 0x06009F35 RID: 40757 RVA: 0x0041B6E0 File Offset: 0x004198E0
		private void BuildDiaryActions()
		{
			RectTransform rectTransform = (RectTransform)this._self.Find("ReaderFooter");
			this._diaryActions = CielOSStoryApp.NewChild("PersonalDiaryActions", rectTransform);
			this._diaryActions.anchorMin = new Vector2(0.5f, 0f);
			this._diaryActions.anchorMax = new Vector2(1f, 0f);
			this._diaryActions.offsetMin = new Vector2(0f, 70f);
			this._diaryActions.offsetMax = new Vector2(-26f, 114f);
			CielOSStoryApp.DiaryRow(this._diaryActions, 8f);
			this._newDiaryButton = this.DiaryButton(this._diaryActions, "New page", new UnityAction(this.BeginNewDiaryPage));
			this._editDiaryButton = this.DiaryButton(this._diaryActions, "Edit page", new UnityAction(this.EditPersonalPage));
			this._deleteDiaryButton = this.DiaryButton(this._diaryActions, "Delete page", new UnityAction(this.ConfirmDeletePersonalPage));
		}

		// Token: 0x06009F36 RID: 40758 RVA: 0x0041B7F8 File Offset: 0x004199F8
		private void RefreshDiaryActions()
		{
			if (this._diaryActions == null)
			{
				return;
			}
			bool canEditPersonalDiary = this.CanEditPersonalDiary;
			this._diaryActions.gameObject.SetActive(canEditPersonalDiary);
			RectTransform rectTransform = ((this._chapterHint != null) ? this._chapterHint.rectTransform : null);
			if (rectTransform != null)
			{
				rectTransform.anchorMax = new Vector2(canEditPersonalDiary ? 0.49f : 1f, 0f);
			}
			if (!canEditPersonalDiary)
			{
				CielOSStoryApp.RemoveDiaryObject(this._diaryEditor);
				CielOSStoryApp.RemoveDiaryObject(this._diaryConfirmation);
				this._diaryEditor = null;
				this._diaryConfirmation = null;
				this._diaryEditorScroll = null;
				this._diaryDraft = null;
				this._diaryDirty = false;
				return;
			}
			bool flag = this.EnsureDiaryLoaded();
			this._newDiaryButton.interactable = flag && this._diaryStore.PageCount < 200;
			this._editDiaryButton.gameObject.SetActive(this.IsPersonalPage);
			this._deleteDiaryButton.gameObject.SetActive(this.IsPersonalPage);
			if (this._chapterHint != null && (this.IsPersonalPage || RankStoryProgression.PendingPageIndex < 0 || !flag || this._diaryRecoveredBackup))
			{
				LocalizedGameText localizedGameText = LocalizedGameText.BindSource(this._chapterHint, "phone_story", (!flag) ? "Your diary could not be loaded. Reopen Story to retry." : (this._diaryRecoveredBackup ? "Your diary was restored from its previous save." : (this.IsPersonalPage ? "Your personal diary" : "Your story continues. Add your own pages.")), null);
				if (localizedGameText == null)
				{
					return;
				}
				localizedGameText.FitTranslationToRectangle(false);
			}
		}

		// Token: 0x06009F37 RID: 40759 RVA: 0x0041B978 File Offset: 0x00419B78
		private void BeginNewDiaryPage()
		{
			if (!this.CanEditPersonalDiary || !this.EnsureDiaryLoaded() || this.DiaryOwnsInput || this._diaryStore.PageCount >= 200)
			{
				return;
			}
			this._diaryDraft = this._diaryStore.CreateDraft();
			this._diaryStatusMessage = null;
			this._diaryDraft.blocks.Add(CielOSStoryApp.NewDiaryBlock(PersonalDiaryStore.BlockType.Paragraph));
			this._diaryDirty = true;
			this.FlushDiaryChanges();
			this._page = this.AccessibleAuthoredPageCount + this._diaryStore.PageCount - 1;
			this.RebuildDiaryEditor();
		}

		// Token: 0x06009F38 RID: 40760 RVA: 0x0041BA0C File Offset: 0x00419C0C
		private void EditPersonalPage()
		{
			if (!this.CanEditPersonalDiary || !this.IsPersonalPage || this.DiaryOwnsInput)
			{
				return;
			}
			this._diaryDraft = PersonalDiaryStore.ClonePage(this.CurrentPersonalPage);
			this._diaryStatusMessage = null;
			this._diaryDirty = false;
			for (int i = 0; i < this._diaryStore.PageCount; i++)
			{
				if (this._diaryStore.GetPage(i).id == this._diaryDraft.id)
				{
					this._page = this.AccessibleAuthoredPageCount + i;
					break;
				}
			}
			this.RebuildDiaryEditor();
		}

		// Token: 0x06009F39 RID: 40761 RVA: 0x0041BAA0 File Offset: 0x00419CA0
		private static PersonalDiaryStore.Block NewDiaryBlock(PersonalDiaryStore.BlockType type)
		{
			return new PersonalDiaryStore.Block
			{
				id = Guid.NewGuid().ToString("N"),
				type = type,
				text = ""
			};
		}

		// Token: 0x06009F3A RID: 40762 RVA: 0x0041BADC File Offset: 0x00419CDC
		private void DiaryChanged()
		{
			this._diaryDirty = true;
			this._diarySaveAt = Time.unscaledTime + 0.8f;
			this.SetDiaryStatus("Saving...", false);
		}

		// Token: 0x06009F3B RID: 40763 RVA: 0x0041BB02 File Offset: 0x00419D02
		private void UpdateDiaryAutosave()
		{
			this.CaptureDiarySelection();
			if (this._diaryDirty && this._diaryDraft != null && Time.unscaledTime >= this._diarySaveAt)
			{
				this._diarySaveAt = float.PositiveInfinity;
				this.FlushDiaryChanges();
			}
		}

		// Token: 0x06009F3C RID: 40764 RVA: 0x0041BB3C File Offset: 0x00419D3C
		private bool FlushDiaryChanges()
		{
			if (!this._diaryDirty || this._diaryDraft == null || this._diaryStore == null)
			{
				return true;
			}
			if (!this.CanEditPersonalDiary)
			{
				return false;
			}
			string text;
			if (!this._diaryStore.TrySavePage(this._diaryDraft, out text))
			{
				this.SetDiaryFailure("Your diary could not be saved. Keep this page open and try again.", text);
				return false;
			}
			this._diaryDirty = false;
			this.SetDiaryStatus("All changes saved", false);
			return true;
		}

		// Token: 0x06009F3D RID: 40765 RVA: 0x0041BBA4 File Offset: 0x00419DA4
		public bool CanCloseWindow()
		{
			return this.FlushDiaryChanges();
		}

		// Token: 0x06009F3E RID: 40766 RVA: 0x0041BBAC File Offset: 0x00419DAC
		private void SetDiaryFailure(string source, string diagnostic)
		{
			if (!string.IsNullOrEmpty(diagnostic))
			{
				Debug.LogWarning("Personal diary: " + diagnostic, this);
			}
			this.SetDiaryStatus(source, true);
		}

		// Token: 0x06009F3F RID: 40767 RVA: 0x0041BBD0 File Offset: 0x00419DD0
		private void SetDiaryStatus(string text, bool isError = true)
		{
			this._diaryStatusMessage = text;
			this._diaryStatusIsError = isError;
			if (this._diaryStatus == null)
			{
				if (isError && this._chapterHint != null)
				{
					LocalizedGameText localizedGameText = LocalizedGameText.BindSource(this._chapterHint, "phone_story", text, null);
					if (localizedGameText == null)
					{
						return;
					}
					localizedGameText.FitTranslationToRectangle(false);
				}
				return;
			}
			this._diaryStatus.color = (isError ? new Color(0.65f, 0.15f, 0.16f) : CielOSStoryApp.InkDim);
			LocalizedGameText.BindSource(this._diaryStatus, "phone_story", text, null);
		}

		// Token: 0x06009F40 RID: 40768 RVA: 0x0041BC64 File Offset: 0x00419E64
		private void CloseDiaryEditor()
		{
			if (!this.FlushDiaryChanges())
			{
				return;
			}
			CielOSStoryApp.RemoveDiaryObject(this._diaryEditor);
			this._diaryEditor = null;
			this._diaryEditorScroll = null;
			this._diaryStatus = null;
			this._diaryDraft = null;
			this._diaryStatusMessage = null;
			this.ShowPage(this._page);
		}

		// Token: 0x06009F41 RID: 40769 RVA: 0x0041BCB4 File Offset: 0x00419EB4
		private void ConfirmDeletePersonalPage()
		{
			if (!this.CanEditPersonalDiary || !this.IsPersonalPage || this.DiaryOwnsInput)
			{
				return;
			}
			string id = this.CurrentPersonalPage.id;
			this.ConfirmDiaryAction("Delete this page? This cannot be undone.", delegate
			{
				string text;
				if (!this._diaryStore.TryDeletePage(id, out text))
				{
					this.SetDiaryFailure("This page could not be deleted. Try again.", text);
					return;
				}
				this.ShowPage(Mathf.Min(this._page, this.PageCount - 1));
			});
		}

		// Token: 0x06009F42 RID: 40770 RVA: 0x0041BD10 File Offset: 0x00419F10
		private void ConfirmDiaryAction(string message, UnityAction action)
		{
			if (this._diaryConfirmation != null)
			{
				return;
			}
			this._diaryConfirmation = CielOSStoryApp.NewChild("DiaryConfirmation", this._self);
			CielOSStoryApp.Stretch(this._diaryConfirmation);
			this._diaryConfirmation.gameObject.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.4f);
			UnityAction cancel = delegate
			{
				CielOSStoryApp.RemoveDiaryObject(this._diaryConfirmation);
				this._diaryConfirmation = null;
			};
			ControllerNavigationScope.Configure(this._diaryConfirmation.gameObject, delegate
			{
				cancel();
			}, null, 100, false);
			RectTransform rectTransform = CielOSStoryApp.NewChild("Card", this._diaryConfirmation);
			rectTransform.anchorMin = new Vector2(0.2f, 0.32f);
			rectTransform.anchorMax = new Vector2(0.8f, 0.68f);
			rectTransform.offsetMin = (rectTransform.offsetMax = Vector2.zero);
			rectTransform.gameObject.AddComponent<Image>().color = CielOSStoryApp.Paper;
			TMP_Text tmp_Text = this.DiaryLabel(rectTransform, message, 26f, CielOSStoryApp.Ink, true);
			CielOSStoryApp.Stretch(tmp_Text.rectTransform);
			tmp_Text.rectTransform.offsetMin = new Vector2(24f, 74f);
			tmp_Text.rectTransform.offsetMax = new Vector2(-24f, -20f);
			RectTransform rectTransform2 = CielOSStoryApp.NewChild("Actions", rectTransform);
			rectTransform2.anchorMin = Vector2.zero;
			rectTransform2.anchorMax = new Vector2(1f, 0f);
			rectTransform2.pivot = new Vector2(0.5f, 0f);
			rectTransform2.sizeDelta = new Vector2(-48f, 46f);
			rectTransform2.anchoredPosition = new Vector2(0f, 20f);
			CielOSStoryApp.DiaryRow(rectTransform2, 12f);
			this.DiaryButton(rectTransform2, "Keep", cancel);
			this.DiaryButton(rectTransform2, "Delete", delegate
			{
				cancel();
				if (this.CanEditPersonalDiary)
				{
					action();
				}
			});
		}

		// Token: 0x06009F43 RID: 40771 RVA: 0x0041BF1C File Offset: 0x0041A11C
		private RectTransform BuildPersonalDiarySheet(PersonalDiaryStore.Page page)
		{
			RectTransform rectTransform = this.BuildSheet(new CielOSStoryBookSO.Page());
			RectTransform rectTransform2 = (RectTransform)rectTransform.Find("Column");
			float num = CielOSStoryApp.DiaryScale(this._scroll.viewport.rect.width);
			rectTransform2.localScale = Vector3.one * num;
			CielOSStoryApp.DiaryLayout diaryLayout = this.CalculateDiaryLayout(page, this._scroll.viewport.rect.height / num);
			float num2 = 1360f;
			TMP_Text tmp_Text = this.DiaryLabel(rectTransform2, string.IsNullOrWhiteSpace(page.title) ? CielOSStoryApp.DiaryText("Untitled page") : page.title, 48f, CielOSStoryApp.DiaryAccent, false);
			tmp_Text.alignment = TextAlignmentOptions.TopLeft;
			tmp_Text.lineSpacing = 6f;
			CielOSStoryApp.PlaceTopLeft(tmp_Text.rectTransform, 120f, 64f, num2, diaryLayout.titleHeight);
			this.DrawPenUnderline(rectTransform2, tmp_Text, tmp_Text.text, num2, CielOSStoryApp.DiaryAccent, 64f + diaryLayout.titleHeight - 16f);
			foreach (CielOSStoryApp.DiaryPlacement diaryPlacement in diaryLayout.items)
			{
				PersonalDiaryStore.Block block = diaryPlacement.block;
				if (block.type != PersonalDiaryStore.BlockType.Photo)
				{
					bool flag = block.type > PersonalDiaryStore.BlockType.Paragraph;
					string text = (block.richText ? PersonalDiaryTextFormatting.Normalize(block.text) : block.text);
					TMP_Text tmp_Text2 = this.DiaryLabel(rectTransform2, text, CielOSStoryApp.DiaryFontSize(block), flag ? CielOSStoryApp.DiaryAccent : CielOSStoryApp.Ink, false);
					tmp_Text2.richText = block.richText;
					tmp_Text2.lineSpacing = 6f;
					tmp_Text2.alignment = ((block.alignment == 1) ? TextAlignmentOptions.Top : ((block.alignment == 2) ? TextAlignmentOptions.TopRight : TextAlignmentOptions.TopLeft));
					CielOSStoryApp.PlaceTopLeft(tmp_Text2.rectTransform, diaryPlacement.textRect.x, diaryPlacement.textRect.y, diaryPlacement.textRect.width, diaryPlacement.textRect.height);
					if (flag)
					{
						this.DrawPenUnderline(rectTransform2, tmp_Text2, text, num2, CielOSStoryApp.DiaryAccent, diaryPlacement.textRect.yMax - 16f);
					}
				}
			}
			foreach (CielOSStoryApp.DiaryPlacement diaryPlacement2 in diaryLayout.items)
			{
				if (diaryPlacement2.block.type == PersonalDiaryStore.BlockType.Photo)
				{
					this.BuildDiaryPhotoVisual(rectTransform2, diaryPlacement2, false);
				}
			}
			rectTransform2.sizeDelta = new Vector2(1560f, diaryLayout.height);
			rectTransform.sizeDelta = new Vector2(0f, diaryLayout.height * num);
			this._content.sizeDelta = rectTransform.sizeDelta;
			return rectTransform;
		}

		// Token: 0x06009F44 RID: 40772 RVA: 0x0041C23C File Offset: 0x0041A43C
		private void AddDiaryWriting(RectTransform column, string text, float size, Color color, float width, ref float y, bool underline, bool richText = false, int alignment = 0)
		{
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			TMP_Text tmp_Text = this.DiaryLabel(column, text, size, color, false);
			tmp_Text.richText = richText;
			tmp_Text.alignment = ((alignment == 1) ? TextAlignmentOptions.Top : ((alignment == 2) ? TextAlignmentOptions.TopRight : TextAlignmentOptions.TopLeft));
			tmp_Text.lineSpacing = 6f;
			float num = tmp_Text.GetPreferredValues(text, width, 0f).y + (underline ? 26f : 6f);
			CielOSStoryApp.PlaceTopLeft(tmp_Text.rectTransform, 120f, y, width, num);
			if (underline)
			{
				this.DrawPenUnderline(column, tmp_Text, text, width, color, y + num - 16f);
			}
			y += num;
		}

		// Token: 0x06009F45 RID: 40773 RVA: 0x0041C2F4 File Offset: 0x0041A4F4
		private static string DiaryText(string source)
		{
			return GameLocalization.GetSource("phone_story", source);
		}

		// Token: 0x06009F46 RID: 40774 RVA: 0x0041C304 File Offset: 0x0041A504
		private TMP_Text DiaryLabel(RectTransform parent, string text, float size, Color color, bool localized)
		{
			TextMeshProUGUI textMeshProUGUI = CielOSStoryApp.NewChild("Label", parent).gameObject.AddComponent<TextMeshProUGUI>();
			if (this._font != null)
			{
				textMeshProUGUI.font = this._font;
			}
			textMeshProUGUI.text = text;
			textMeshProUGUI.fontSize = size;
			textMeshProUGUI.color = color;
			textMeshProUGUI.richText = false;
			textMeshProUGUI.alignment = TextAlignmentOptions.MidlineLeft;
			textMeshProUGUI.raycastTarget = false;
			textMeshProUGUI.overflowMode = TextOverflowModes.Overflow;
			if (localized)
			{
				LocalizedGameText.BindSource(textMeshProUGUI, "phone_story", text, null);
			}
			else
			{
				LocalizedGameText.BindFontOnly(textMeshProUGUI);
			}
			return textMeshProUGUI;
		}

		// Token: 0x06009F47 RID: 40775 RVA: 0x0041C394 File Offset: 0x0041A594
		private Button DiaryButton(RectTransform parent, string text, UnityAction action)
		{
			RectTransform rectTransform = CielOSStoryApp.NewChild(text, parent);
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = new Color(0.925f, 0.9f, 0.85f);
			CielOSTheme.Round(image);
			CielOSTheme.AddBorder(rectTransform, new Color?(new Color(CielOSStoryApp.Ink.r, CielOSStoryApp.Ink.g, CielOSStoryApp.Ink.b, 0.2f)), 1f);
			Button button = rectTransform.gameObject.AddComponent<Button>();
			button.targetGraphic = image;
			button.onClick.AddListener(action);
			CielOSSfx.Attach(button);
			TMP_Text tmp_Text = this.DiaryLabel(rectTransform, text, 20f, CielOSStoryApp.Ink, true);
			CielOSStoryApp.Stretch(tmp_Text.rectTransform);
			tmp_Text.rectTransform.offsetMin = new Vector2(8f, 2f);
			tmp_Text.rectTransform.offsetMax = new Vector2(-8f, -2f);
			tmp_Text.alignment = TextAlignmentOptions.Center;
			tmp_Text.enableAutoSizing = true;
			tmp_Text.fontSizeMin = 12f;
			tmp_Text.fontSizeMax = 20f;
			tmp_Text.GetComponent<LocalizedGameText>().SetSourceLayout(true, true, 12f, 20f);
			rectTransform.gameObject.AddComponent<LayoutElement>().flexibleWidth = 1f;
			return button;
		}

		// Token: 0x06009F48 RID: 40776 RVA: 0x0041C4D8 File Offset: 0x0041A6D8
		private TMP_InputField DiaryInput(RectTransform parent, string placeholder, string text, float size, bool multiline, int limit, UnityAction<string> changed)
		{
			RectTransform rectTransform = CielOSStoryApp.NewChild("Input", parent);
			rectTransform.gameObject.SetActive(false);
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = new Color(1f, 1f, 1f, 0.42f);
			image.raycastTarget = true;
			PersonalDiaryInputField personalDiaryInputField = rectTransform.gameObject.AddComponent<PersonalDiaryInputField>();
			personalDiaryInputField.targetGraphic = image;
			personalDiaryInputField.onFocusSelectAll = false;
			personalDiaryInputField.resetOnDeActivation = false;
			personalDiaryInputField.restoreOriginalTextOnEscape = false;
			personalDiaryInputField.customCaretColor = true;
			personalDiaryInputField.caretColor = CielOSStoryApp.Ink;
			personalDiaryInputField.caretWidth = 2;
			personalDiaryInputField.selectionColor = new Color(CielOSStoryApp.DiaryAccent.r, CielOSStoryApp.DiaryAccent.g, CielOSStoryApp.DiaryAccent.b, 0.32f);
			personalDiaryInputField.lineType = (multiline ? TMP_InputField.LineType.MultiLineNewline : TMP_InputField.LineType.SingleLine);
			personalDiaryInputField.characterLimit = limit;
			personalDiaryInputField.richText = false;
			RectTransform rectTransform2 = CielOSStoryApp.NewChild("TextArea", rectTransform);
			CielOSStoryApp.Stretch(rectTransform2);
			rectTransform2.offsetMin = new Vector2(12f, 8f);
			rectTransform2.offsetMax = new Vector2(-12f, -8f);
			rectTransform2.gameObject.AddComponent<RectMask2D>();
			personalDiaryInputField.textViewport = rectTransform2;
			TMP_Text tmp_Text = this.DiaryLabel(rectTransform2, "", size, CielOSStoryApp.Ink, false);
			CielOSStoryApp.Stretch(tmp_Text.rectTransform);
			tmp_Text.alignment = (multiline ? TextAlignmentOptions.TopLeft : TextAlignmentOptions.MidlineLeft);
			personalDiaryInputField.textComponent = (TextMeshProUGUI)tmp_Text;
			TMP_Text tmp_Text2 = this.DiaryLabel(rectTransform2, placeholder, size, CielOSStoryApp.InkDim, true);
			CielOSStoryApp.Stretch(tmp_Text2.rectTransform);
			tmp_Text2.alignment = tmp_Text.alignment;
			personalDiaryInputField.placeholder = tmp_Text2;
			personalDiaryInputField.text = text ?? "";
			personalDiaryInputField.onValueChanged.AddListener(changed);
			rectTransform.gameObject.SetActive(true);
			return personalDiaryInputField;
		}

		// Token: 0x06009F49 RID: 40777 RVA: 0x0041C6A8 File Offset: 0x0041A8A8
		private static void DiaryRow(RectTransform rt, float spacing)
		{
			HorizontalLayoutGroup horizontalLayoutGroup = rt.gameObject.AddComponent<HorizontalLayoutGroup>();
			horizontalLayoutGroup.spacing = spacing;
			horizontalLayoutGroup.childControlHeight = (horizontalLayoutGroup.childControlWidth = true);
			horizontalLayoutGroup.childForceExpandHeight = (horizontalLayoutGroup.childForceExpandWidth = true);
		}

		// Token: 0x06009F4A RID: 40778 RVA: 0x0041C6E8 File Offset: 0x0041A8E8
		private static void DiaryHeight(RectTransform rt, float height)
		{
			LayoutElement layoutElement = rt.gameObject.AddComponent<LayoutElement>();
			layoutElement.minHeight = height;
			layoutElement.preferredHeight = height;
		}

		// Token: 0x06009F4B RID: 40779 RVA: 0x0041C70F File Offset: 0x0041A90F
		private static void RemoveDiaryObject(RectTransform rt)
		{
			if (rt == null)
			{
				return;
			}
			rt.gameObject.SetActive(false);
			if (Application.isPlaying)
			{
				Object.Destroy(rt.gameObject);
				return;
			}
			Object.DestroyImmediate(rt.gameObject);
		}

		// Token: 0x06009F4C RID: 40780 RVA: 0x0041C745 File Offset: 0x0041A945
		private void OnDestroy()
		{
			this.FlushDiaryChanges();
			this.ReleaseDiarySprites();
		}

		// Token: 0x06009F4D RID: 40781 RVA: 0x0041C754 File Offset: 0x0041A954
		private void PickDiaryPhoto(bool phoneGallery)
		{
			if (!this.CanEditPersonalDiary || this._diaryDraft == null || this._diaryEditor == null || this._diaryStore == null)
			{
				return;
			}
			if (this._diaryDraft.blocks.Count >= 100)
			{
				this.SetDiaryStatus("This page is full. Add a new page to keep writing.", true);
				return;
			}
			Canvas componentInParent = base.GetComponentInParent<Canvas>();
			if (componentInParent == null)
			{
				return;
			}
			PersonalDiaryStore.Page draft = this._diaryDraft;
			try
			{
				string text = (phoneGallery ? PhonePhotoStorage.ImagesFolder : null);
				CielOSFileBrowser browser = CielOSFileBrowser.Ensure(componentInParent.transform);
				browser.Open(phoneGallery ? "Phone photos" : "Import photo", new string[] { ".png", ".jpg", ".jpeg" }, delegate(string path)
				{
					if (this == null || !this.isActiveAndEnabled || this._diaryEditor == null || this._diaryDraft != draft || !this.CanEditPersonalDiary || string.IsNullOrWhiteSpace(path))
					{
						return;
					}
					if (draft.blocks.Count >= 100)
					{
						this.SetDiaryStatus("This page is full. Add a new page to keep writing.", true);
						return;
					}
					string text2;
					string text3;
					if (!this._diaryStore.TryImportImage(path, out text2, out text3))
					{
						this.SetDiaryPhotoFailure(text3);
						return;
					}
					this.InsertDiaryPhotoBlock(new PersonalDiaryStore.Block
					{
						id = Guid.NewGuid().ToString("N"),
						type = PersonalDiaryStore.BlockType.Photo,
						imageFileName = text2,
						caption = "",
						sensitiveImage = false
					});
					this.DiaryChanged();
					this.RebuildDiaryEditor();
					browser.RememberSuccessfulDirectory(path);
				}, text, "phone_story", phoneGallery);
			}
			catch (Exception ex)
			{
				this.SetDiaryFailure("Photos could not be opened. Check that the folder is available and try again.", ex.Message);
			}
		}

		// Token: 0x06009F4E RID: 40782 RVA: 0x0041C864 File Offset: 0x0041AA64
		private void SetDiaryPhotoFailure(string diagnostic)
		{
			string text = "The photo could not be added. Check that the file is available and try again.";
			if (!string.IsNullOrEmpty(diagnostic))
			{
				if (diagnostic.Contains("Choose a photo up to 8192 pixels per side and 32 megapixels."))
				{
					text = "Choose a photo up to 8192 pixels per side and 32 megapixels.";
				}
				else if (diagnostic.Contains("Choose a PNG or JPEG photo smaller than 20 MB."))
				{
					text = "Choose a PNG or JPEG photo smaller than 20 MB.";
				}
				else if (diagnostic.Contains("Choose a valid PNG or JPEG photo.") || diagnostic.Contains("This image could not be decoded."))
				{
					text = "Choose a valid PNG or JPEG photo.";
				}
			}
			this.SetDiaryFailure(text, diagnostic);
		}

		// Token: 0x06009F4F RID: 40783 RVA: 0x0041C8D4 File Offset: 0x0041AAD4
		private Sprite LoadDiarySprite(string fileName)
		{
			if (string.IsNullOrEmpty(fileName) || this._diaryStore == null)
			{
				return null;
			}
			Sprite sprite;
			if (this._diarySprites.TryGetValue(fileName, out sprite) && sprite != null)
			{
				return sprite;
			}
			if (this._unavailableDiaryImages.Contains(fileName))
			{
				return null;
			}
			Texture2D texture2D = null;
			Sprite sprite2;
			try
			{
				byte[] array;
				string text;
				if (!this._diaryStore.TryLoadImageBytes(fileName, out array, out text))
				{
					this._unavailableDiaryImages.Add(fileName);
					sprite2 = null;
				}
				else
				{
					texture2D = new Texture2D(2, 2, TextureFormat.RGBA32, false);
					if (!texture2D.LoadImage(array, true))
					{
						CielOSStoryApp.DestroyDiaryImageObject(texture2D);
						this._unavailableDiaryImages.Add(fileName);
						sprite2 = null;
					}
					else
					{
						texture2D = CielOSStoryApp.LimitDiaryPhotoDisplaySize(texture2D);
						texture2D.name = "Personal diary photo";
						texture2D.filterMode = FilterMode.Bilinear;
						texture2D.wrapMode = TextureWrapMode.Clamp;
						Sprite sprite3 = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f), 100f);
						sprite3.name = "PersonalDiary_" + fileName;
						this._diarySprites[fileName] = sprite3;
						sprite2 = sprite3;
					}
				}
			}
			catch (Exception)
			{
				if (texture2D != null)
				{
					CielOSStoryApp.DestroyDiaryImageObject(texture2D);
				}
				this._unavailableDiaryImages.Add(fileName);
				sprite2 = null;
			}
			return sprite2;
		}

		// Token: 0x06009F50 RID: 40784 RVA: 0x0041CA2C File Offset: 0x0041AC2C
		private static Texture2D LimitDiaryPhotoDisplaySize(Texture2D original)
		{
			int num = Mathf.Max(original.width, original.height);
			if (num <= 1024)
			{
				return original;
			}
			float num2 = 1024f / (float)num;
			int num3 = Mathf.Max(1, Mathf.RoundToInt((float)original.width * num2));
			int num4 = Mathf.Max(1, Mathf.RoundToInt((float)original.height * num2));
			RenderTexture active = RenderTexture.active;
			RenderTexture renderTexture = null;
			Texture2D texture2D = null;
			Texture2D texture2D2;
			try
			{
				renderTexture = RenderTexture.GetTemporary(num3, num4, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
				Graphics.Blit(original, renderTexture);
				RenderTexture.active = renderTexture;
				texture2D = new Texture2D(num3, num4, TextureFormat.RGBA32, false);
				texture2D.ReadPixels(new Rect(0f, 0f, (float)num3, (float)num4), 0, 0, false);
				texture2D.Apply(false, true);
				CielOSStoryApp.DestroyDiaryImageObject(original);
				texture2D2 = texture2D;
			}
			catch
			{
				if (texture2D != null)
				{
					CielOSStoryApp.DestroyDiaryImageObject(texture2D);
				}
				throw;
			}
			finally
			{
				RenderTexture.active = active;
				if (renderTexture != null)
				{
					RenderTexture.ReleaseTemporary(renderTexture);
				}
			}
			return texture2D2;
		}

		// Token: 0x06009F51 RID: 40785 RVA: 0x0041CB38 File Offset: 0x0041AD38
		private void ReleaseDiarySprites()
		{
			if (this._diarySprites.Count > 0)
			{
				List<Sprite> list = new List<Sprite>(this._diarySprites.Values);
				if (Application.isPlaying && PhotoPolaroidViewer.IsOpen)
				{
					GameObject gameObject = new GameObject("Personal diary photo cleanup");
					Object.DontDestroyOnLoad(gameObject);
					gameObject.AddComponent<PersonalDiaryPhotoCleanup>().Sprites = list;
				}
				else
				{
					CielOSStoryApp.DestroyDiarySprites(list);
				}
			}
			this._diarySprites.Clear();
			this._unavailableDiaryImages.Clear();
		}

		// Token: 0x06009F52 RID: 40786 RVA: 0x0041CBAC File Offset: 0x0041ADAC
		internal static void DestroyDiarySprites(List<Sprite> sprites)
		{
			foreach (Sprite sprite in sprites)
			{
				if (!(sprite == null))
				{
					Texture2D texture = sprite.texture;
					CielOSStoryApp.DestroyDiaryImageObject(sprite);
					if (texture != null)
					{
						CielOSStoryApp.DestroyDiaryImageObject(texture);
					}
				}
			}
		}

		// Token: 0x06009F53 RID: 40787 RVA: 0x00027EE2 File Offset: 0x000260E2
		private static void DestroyDiaryImageObject(Object item)
		{
			if (Application.isPlaying)
			{
				Object.Destroy(item);
				return;
			}
			Object.DestroyImmediate(item);
		}

		// Token: 0x06009F54 RID: 40788 RVA: 0x0041CC18 File Offset: 0x0041AE18
		private void CreatePageScrollbar(ScrollRect scroll, RectTransform parent, Action cancelGlide = null)
		{
			RectTransform rectTransform = CielOSStoryApp.NewChild("PageScrollbar", parent);
			rectTransform.anchorMin = new Vector2(1f, 0f);
			rectTransform.anchorMax = Vector2.one;
			rectTransform.offsetMin = new Vector2(-14f, 0f);
			rectTransform.offsetMax = new Vector2(-2f, 0f);
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = new Color(CielOSStoryApp.DiaryAccent.r, CielOSStoryApp.DiaryAccent.g, CielOSStoryApp.DiaryAccent.b, 0.12f);
			CielOSTheme.Round(image);
			RectTransform rectTransform2 = CielOSStoryApp.NewChild("Handle", rectTransform);
			CielOSStoryApp.Stretch(rectTransform2);
			Image image2 = rectTransform2.gameObject.AddComponent<Image>();
			image2.color = new Color(CielOSStoryApp.DiaryAccent.r, CielOSStoryApp.DiaryAccent.g, CielOSStoryApp.DiaryAccent.b, 0.65f);
			CielOSTheme.Round(image2);
			Scrollbar scrollbar = rectTransform.gameObject.AddComponent<Scrollbar>();
			scrollbar.direction = Scrollbar.Direction.BottomToTop;
			scrollbar.handleRect = rectTransform2;
			scrollbar.targetGraphic = image2;
			scroll.verticalScrollbar = scrollbar;
			scroll.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
			CielOSStoryApp.PageScrollbarPointer pageScrollbarPointer = rectTransform.gameObject.AddComponent<CielOSStoryApp.PageScrollbarPointer>();
			pageScrollbarPointer.scroll = scroll;
			pageScrollbarPointer.cancelGlide = cancelGlide;
		}

		// Token: 0x0400A307 RID: 41735
		public const float SheetWidth = 1560f;

		// Token: 0x0400A308 RID: 41736
		[Tooltip("The notebook content. Assigned by the setup tool; swappable for other books.")]
		[SerializeField]
		private CielOSStoryBookSO storyBook;

		// Token: 0x0400A309 RID: 41737
		[Tooltip("Main notebook font (title/body/nav): simple handwriting, like a real notepad — deliberately NOT code-like.")]
		[SerializeField]
		private TMP_FontAsset bodyFont;

		// Token: 0x0400A30A RID: 41738
		[Tooltip("Legacy fallback font if bodyFont is missing.")]
		[SerializeField]
		private TMP_FontAsset monoFont;

		// Token: 0x0400A30B RID: 41739
		[Tooltip("Marker-style font for sticker captions (writing on the polaroid strip).")]
		[SerializeField]
		private TMP_FontAsset handFont;

		// Token: 0x0400A30C RID: 41740
		private TMP_FontAsset _font;

		// Token: 0x0400A30D RID: 41741
		private static readonly Color Paper = new Color(0.953f, 0.933f, 0.89f, 1f);

		// Token: 0x0400A30E RID: 41742
		private static readonly Color Ink = new Color(0.169f, 0.165f, 0.2f, 1f);

		// Token: 0x0400A30F RID: 41743
		private static readonly Color InkDim = new Color(0.169f, 0.165f, 0.2f, 0.55f);

		// Token: 0x0400A310 RID: 41744
		private static readonly Color GridInk = new Color(0.25f, 0.35f, 0.55f, 0.1f);

		// Token: 0x0400A311 RID: 41745
		private static readonly Color MarginRed = new Color(0.85f, 0.42f, 0.42f, 0.45f);

		// Token: 0x0400A312 RID: 41746
		private static readonly Color ShadowColor = new Color(0f, 0f, 0f, 0.3f);

		// Token: 0x0400A313 RID: 41747
		private const float SheetPadTop = 64f;

		// Token: 0x0400A314 RID: 41748
		private const float SheetPadBottom = 96f;

		// Token: 0x0400A315 RID: 41749
		private const float MarginX = 120f;

		// Token: 0x0400A316 RID: 41750
		private const float TextRightPad = 80f;

		// Token: 0x0400A317 RID: 41751
		private const float ReaderFooterHeight = 120f;

		// Token: 0x0400A318 RID: 41752
		private RectTransform _self;

		// Token: 0x0400A319 RID: 41753
		private ScrollRect _scroll;

		// Token: 0x0400A31A RID: 41754
		private CielOSStoryApp.SmoothWheelScroll _smoothScroll;

		// Token: 0x0400A31B RID: 41755
		private RectTransform _content;

		// Token: 0x0400A31C RID: 41756
		private RectTransform _sheet;

		// Token: 0x0400A31D RID: 41757
		private TMP_Text _pageLabel;

		// Token: 0x0400A31E RID: 41758
		private TMP_Text _chapterHint;

		// Token: 0x0400A31F RID: 41759
		private Button _prevButton;

		// Token: 0x0400A320 RID: 41760
		private Button _nextButton;

		// Token: 0x0400A321 RID: 41761
		private CielOSWindow _window;

		// Token: 0x0400A322 RID: 41762
		private readonly List<CanvasGroup> _visibilityGroups = new List<CanvasGroup>();

		// Token: 0x0400A323 RID: 41763
		private readonly List<Canvas> _parentCanvases = new List<Canvas>();

		// Token: 0x0400A324 RID: 41764
		private bool _pageViewRecorded;

		// Token: 0x0400A325 RID: 41765
		private int _pageShownFrame;

		// Token: 0x0400A326 RID: 41766
		private int _page;

		// Token: 0x0400A327 RID: 41767
		private Vector2 _lastViewport;

		// Token: 0x0400A328 RID: 41768
		private static Sprite _gridSprite;

		// Token: 0x0400A329 RID: 41769
		private const float HeadingTopGap = 10f;

		// Token: 0x0400A32A RID: 41770
		private const float StickerEdge = 6f;

		// Token: 0x0400A32B RID: 41771
		private PersonalDiaryStore.Block _activeDiaryBlock;

		// Token: 0x0400A32C RID: 41772
		private TMP_InputField _activeDiaryInput;

		// Token: 0x0400A32D RID: 41773
		private int _diaryAnchor;

		// Token: 0x0400A32E RID: 41774
		private int _diaryFocus;

		// Token: 0x0400A32F RID: 41775
		private bool _changingDiaryText;

		// Token: 0x0400A330 RID: 41776
		private int _diaryTypingMask = -1;

		// Token: 0x0400A331 RID: 41777
		private string _diaryTypingBlock;

		// Token: 0x0400A332 RID: 41778
		private readonly Dictionary<string, TMP_InputField> _diaryInputs = new Dictionary<string, TMP_InputField>();

		// Token: 0x0400A333 RID: 41779
		private RectTransform _diaryPhotoTools;

		// Token: 0x0400A334 RID: 41780
		private CielOSStoryApp.SmoothWheelScroll _diarySmoothScroll;

		// Token: 0x0400A335 RID: 41781
		private TMP_Text _diaryEditorHint;

		// Token: 0x0400A336 RID: 41782
		private RectTransform _diaryHeader;

		// Token: 0x0400A337 RID: 41783
		private RectTransform _diaryToolbar;

		// Token: 0x0400A338 RID: 41784
		private readonly List<Button> _diaryFormattingButtons = new List<Button>();

		// Token: 0x0400A339 RID: 41785
		private TMP_Text _diaryMeasure;

		// Token: 0x0400A33A RID: 41786
		private RectTransform _diaryDocument;

		// Token: 0x0400A33B RID: 41787
		private TMP_InputField _diaryTitleInput;

		// Token: 0x0400A33C RID: 41788
		private readonly Dictionary<string, RectTransform> _diaryRows = new Dictionary<string, RectTransform>();

		// Token: 0x0400A33D RID: 41789
		private readonly Dictionary<string, RectTransform> _diaryPhotoRoots = new Dictionary<string, RectTransform>();

		// Token: 0x0400A33E RID: 41790
		private float _diaryChromeBottom;

		// Token: 0x0400A33F RID: 41791
		private bool _diaryPhotoDragging;

		// Token: 0x0400A340 RID: 41792
		private RectTransform _diaryHeightDocument;

		// Token: 0x0400A341 RID: 41793
		private float _diaryRetainedDragHeight;

		// Token: 0x0400A342 RID: 41794
		private PersonalDiaryStore _diaryStore;

		// Token: 0x0400A343 RID: 41795
		private PersonalDiaryStore.Page _diaryDraft;

		// Token: 0x0400A344 RID: 41796
		private RectTransform _diaryEditor;

		// Token: 0x0400A345 RID: 41797
		private ScrollRect _diaryEditorScroll;

		// Token: 0x0400A346 RID: 41798
		private RectTransform _diaryActions;

		// Token: 0x0400A347 RID: 41799
		private RectTransform _diaryConfirmation;

		// Token: 0x0400A348 RID: 41800
		private TMP_Text _diaryStatus;

		// Token: 0x0400A349 RID: 41801
		private Button _newDiaryButton;

		// Token: 0x0400A34A RID: 41802
		private Button _editDiaryButton;

		// Token: 0x0400A34B RID: 41803
		private Button _deleteDiaryButton;

		// Token: 0x0400A34C RID: 41804
		private bool _diaryLoaded;

		// Token: 0x0400A34D RID: 41805
		private bool _diaryDirty;

		// Token: 0x0400A34E RID: 41806
		private float _diarySaveAt;

		// Token: 0x0400A34F RID: 41807
		private string _diaryLoadError;

		// Token: 0x0400A350 RID: 41808
		private string _diaryStatusMessage;

		// Token: 0x0400A351 RID: 41809
		private bool _diaryStatusIsError;

		// Token: 0x0400A352 RID: 41810
		private bool _diaryRecoveredBackup;

		// Token: 0x0400A353 RID: 41811
		private static readonly Color DiaryAccent = new Color(0.44f, 0.24f, 0.57f);

		// Token: 0x0400A354 RID: 41812
		private const int DiaryPhotoDisplaySize = 1024;

		// Token: 0x0400A355 RID: 41813
		private readonly Dictionary<string, Sprite> _diarySprites = new Dictionary<string, Sprite>(StringComparer.Ordinal);

		// Token: 0x0400A356 RID: 41814
		private readonly HashSet<string> _unavailableDiaryImages = new HashSet<string>(StringComparer.Ordinal);

		// Token: 0x0200141E RID: 5150
		private class SmoothWheelScroll : MonoBehaviour, IScrollHandler, IEventSystemHandler, IBeginDragHandler, IControllerScrollSync
		{
			// Token: 0x06009F6D RID: 40813 RVA: 0x0041CFD5 File Offset: 0x0041B1D5
			public static CielOSStoryApp.SmoothWheelScroll Attach(ScrollRect scroll)
			{
				CielOSStoryApp.SmoothWheelScroll smoothWheelScroll = scroll.gameObject.AddComponent<CielOSStoryApp.SmoothWheelScroll>();
				smoothWheelScroll._scroll = scroll;
				scroll.scrollSensitivity = 0f;
				return smoothWheelScroll;
			}

			// Token: 0x06009F6E RID: 40814 RVA: 0x0041CFF4 File Offset: 0x0041B1F4
			public void Cancel()
			{
				this._active = false;
			}

			// Token: 0x06009F6F RID: 40815 RVA: 0x0041CFFD File Offset: 0x0041B1FD
			public void SyncControllerScroll()
			{
				this.Cancel();
			}

			// Token: 0x06009F70 RID: 40816 RVA: 0x0041CFFD File Offset: 0x0041B1FD
			private void OnDisable()
			{
				this.Cancel();
			}

			// Token: 0x06009F71 RID: 40817 RVA: 0x0041CFFD File Offset: 0x0041B1FD
			public void OnBeginDrag(PointerEventData eventData)
			{
				this.Cancel();
			}

			// Token: 0x06009F72 RID: 40818 RVA: 0x0041D008 File Offset: 0x0041B208
			public void OnScroll(PointerEventData eventData)
			{
				if (this._scroll == null || this._scroll.content == null || this._scroll.viewport == null)
				{
					return;
				}
				float y = eventData.scrollDelta.y;
				if (Mathf.Approximately(y, 0f))
				{
					return;
				}
				if (!this._active)
				{
					this._target = this._scroll.content.anchoredPosition.y;
				}
				float num = this.MaximumScroll();
				this._target = Mathf.Clamp(this._target - Mathf.Sign(y) * 90f, 0f, num);
				this._active = true;
			}

			// Token: 0x06009F73 RID: 40819 RVA: 0x0041D0BC File Offset: 0x0041B2BC
			private float MaximumScroll()
			{
				this._scroll.content.GetWorldCorners(this._corners);
				float num = float.PositiveInfinity;
				float num2 = float.NegativeInfinity;
				foreach (Vector3 vector in this._corners)
				{
					float y = this._scroll.viewport.InverseTransformPoint(vector).y;
					num = Mathf.Min(num, y);
					num2 = Mathf.Max(num2, y);
				}
				return Mathf.Max(0f, num2 - num - this._scroll.viewport.rect.height);
			}

			// Token: 0x06009F74 RID: 40820 RVA: 0x0041D15C File Offset: 0x0041B35C
			private void Update()
			{
				if (!this._active || this._scroll == null || this._scroll.content == null || this._scroll.viewport == null)
				{
					return;
				}
				this._target = Mathf.Clamp(this._target, 0f, this.MaximumScroll());
				Vector2 anchoredPosition = this._scroll.content.anchoredPosition;
				float num = 1f - Mathf.Exp(-10f * Time.unscaledDeltaTime);
				float num2 = Mathf.Lerp(anchoredPosition.y, this._target, num);
				if (Mathf.Abs(num2 - this._target) < 0.5f)
				{
					num2 = this._target;
					this._active = false;
				}
				this._scroll.velocity = Vector2.zero;
				this._scroll.content.anchoredPosition = new Vector2(anchoredPosition.x, num2);
			}

			// Token: 0x0400A357 RID: 41815
			private const float PixelsPerNotch = 90f;

			// Token: 0x0400A358 RID: 41816
			private const float EaseSpeed = 10f;

			// Token: 0x0400A359 RID: 41817
			private ScrollRect _scroll;

			// Token: 0x0400A35A RID: 41818
			private float _target;

			// Token: 0x0400A35B RID: 41819
			private bool _active;

			// Token: 0x0400A35C RID: 41820
			private readonly Vector3[] _corners = new Vector3[4];
		}

		// Token: 0x0200141F RID: 5151
		private struct BodySegment
		{
			// Token: 0x0400A35D RID: 41821
			public bool isHeading;

			// Token: 0x0400A35E RID: 41822
			public string text;
		}

		// Token: 0x02001420 RID: 5152
		private sealed class DiaryPlacement
		{
			// Token: 0x0400A35F RID: 41823
			public PersonalDiaryStore.Block block;

			// Token: 0x0400A360 RID: 41824
			public Rect textRect;

			// Token: 0x0400A361 RID: 41825
			public Vector2 center;

			// Token: 0x0400A362 RID: 41826
			public Vector2 size;

			// Token: 0x0400A363 RID: 41827
			public float photoHeight;

			// Token: 0x0400A364 RID: 41828
			public float captionHeight;
		}

		// Token: 0x02001421 RID: 5153
		private sealed class DiaryLayout
		{
			// Token: 0x0400A365 RID: 41829
			public float titleHeight;

			// Token: 0x0400A366 RID: 41830
			public float height;

			// Token: 0x0400A367 RID: 41831
			public readonly List<CielOSStoryApp.DiaryPlacement> items = new List<CielOSStoryApp.DiaryPlacement>();
		}

		// Token: 0x02001422 RID: 5154
		private sealed class PageScrollbarPointer : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IScrollHandler, IMoveHandler
		{
			// Token: 0x06009F78 RID: 40824 RVA: 0x0041D272 File Offset: 0x0041B472
			public void OnPointerDown(PointerEventData eventData)
			{
				if (eventData.button != PointerEventData.InputButton.Left)
				{
					return;
				}
				Action action = this.cancelGlide;
				if (action != null)
				{
					action();
				}
				if (this.scroll != null)
				{
					this.scroll.StopMovement();
				}
			}

			// Token: 0x06009F79 RID: 40825 RVA: 0x0041D2A7 File Offset: 0x0041B4A7
			public void OnMove(AxisEventData eventData)
			{
				Action action = this.cancelGlide;
				if (action != null)
				{
					action();
				}
				if (this.scroll != null)
				{
					this.scroll.StopMovement();
				}
			}

			// Token: 0x06009F7A RID: 40826 RVA: 0x0041D2D3 File Offset: 0x0041B4D3
			public void OnScroll(PointerEventData eventData)
			{
				if (this.scroll != null && this.scroll.isActiveAndEnabled)
				{
					ExecuteEvents.Execute<IScrollHandler>(this.scroll.gameObject, eventData, ExecuteEvents.scrollHandler);
				}
			}

			// Token: 0x0400A368 RID: 41832
			public ScrollRect scroll;

			// Token: 0x0400A369 RID: 41833
			public Action cancelGlide;
		}
	}
}
