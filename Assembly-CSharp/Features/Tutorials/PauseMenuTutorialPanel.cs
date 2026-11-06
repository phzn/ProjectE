using System;
using System.Collections.Generic;
using DA_Assets.CR;
using Features.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.Tutorials
{
	// Token: 0x02000BC6 RID: 3014
	[DisallowMultipleComponent]
	public sealed class PauseMenuTutorialPanel : MonoBehaviour
	{
		// Token: 0x17000E61 RID: 3681
		// (get) Token: 0x06005699 RID: 22169 RVA: 0x0021AA5C File Offset: 0x00218C5C
		public bool IsShowingDetail
		{
			get
			{
				return !string.IsNullOrEmpty(this._openTopicId);
			}
		}

		// Token: 0x17000E62 RID: 3682
		// (get) Token: 0x0600569A RID: 22170 RVA: 0x0021AA6C File Offset: 0x00218C6C
		public bool IsAutomatic
		{
			get
			{
				return this._automatic;
			}
		}

		// Token: 0x17000E63 RID: 3683
		// (get) Token: 0x0600569B RID: 22171 RVA: 0x0021AA74 File Offset: 0x00218C74
		public string OpenTopicId
		{
			get
			{
				return this._openTopicId;
			}
		}

		// Token: 0x0600569C RID: 22172 RVA: 0x0021AA7C File Offset: 0x00218C7C
		public void Initialize(TMP_FontAsset font, Action backToPauseRoot, Action continueGame)
		{
			this._font = font;
			this._backToPauseRoot = backToPauseRoot;
			this._continueGame = continueGame;
			this.Build();
		}

		// Token: 0x0600569D RID: 22173 RVA: 0x0021AA99 File Offset: 0x00218C99
		private void OnEnable()
		{
			TutorialProgress.Changed -= this.RefreshProgress;
			TutorialProgress.Changed += this.RefreshProgress;
		}

		// Token: 0x0600569E RID: 22174 RVA: 0x0021AABD File Offset: 0x00218CBD
		private void OnDisable()
		{
			TutorialProgress.Changed -= this.RefreshProgress;
		}

		// Token: 0x0600569F RID: 22175 RVA: 0x0021AAD0 File Offset: 0x00218CD0
		public void ShowLibrary()
		{
			this.Build();
			this._automatic = false;
			TutorialTopic tutorialTopic = this.ResolveDefaultTopic();
			if (tutorialTopic != null)
			{
				this.DisplayTopic(tutorialTopic, false, false);
			}
			this._continueButton.SetActive(false);
			this.SetFooterBack("BACK TO PAUSE", delegate
			{
				Action backToPauseRoot = this._backToPauseRoot;
				if (backToPauseRoot == null)
				{
					return;
				}
				backToPauseRoot();
			});
		}

		// Token: 0x060056A0 RID: 22176 RVA: 0x0021AB20 File Offset: 0x00218D20
		public bool OpenTopic(string topicId, bool automatic)
		{
			this.Build();
			TutorialTopic tutorialTopic;
			if (!TutorialCatalog.TryGetTopic(topicId, out tutorialTopic))
			{
				return false;
			}
			this.DisplayTopic(tutorialTopic, automatic, true);
			return true;
		}

		// Token: 0x060056A1 RID: 22177 RVA: 0x0021AB49 File Offset: 0x00218D49
		public bool TryHandleBack()
		{
			if (this._automatic)
			{
				this.ContinueGame();
			}
			else
			{
				Action backToPauseRoot = this._backToPauseRoot;
				if (backToPauseRoot != null)
				{
					backToPauseRoot();
				}
			}
			return true;
		}

		// Token: 0x060056A2 RID: 22178 RVA: 0x0021AB70 File Offset: 0x00218D70
		private void Build()
		{
			if (this._built)
			{
				return;
			}
			this._built = true;
			RectTransform rectTransform = base.transform as RectTransform;
			if (rectTransform != null)
			{
				PauseMenuTutorialPanel.Stretch(rectTransform, Vector2.zero, Vector2.zero);
			}
			Image image = base.gameObject.GetComponent<Image>() ?? base.gameObject.AddComponent<Image>();
			image.color = PauseMenuTutorialPanel.Background;
			image.raycastTarget = true;
			PauseMenuTutorialPanel.SetRect(this.CreateText("Tutorial Heading", base.transform, "TUTORIAL", 32f, FontStyles.Bold, PauseMenuTutorialPanel.TextPrimary, TextAlignmentOptions.Left, false, null).rectTransform, new Vector2(0f, 1f), Vector2.one, new Vector2(54f, -54f), new Vector2(-54f, -12f));
			TMP_Text tmp_Text = this.CreateText("Tutorial Kicker", base.transform, "A CLEAR GUIDE TO THE GAME", 12f, FontStyles.Bold, PauseMenuTutorialPanel.TextQuiet, TextAlignmentOptions.Left, false, null);
			tmp_Text.characterSpacing = 1.8f;
			PauseMenuTutorialPanel.SetRect(tmp_Text.rectTransform, new Vector2(0f, 1f), Vector2.one, new Vector2(54f, -88f), new Vector2(-54f, -60f));
			RectTransform rectTransform2 = PauseMenuTutorialPanel.CreateRect("Tutorial Wiki", base.transform);
			PauseMenuTutorialPanel.SetRect(rectTransform2, Vector2.zero, Vector2.one, new Vector2(54f, 78f), new Vector2(-54f, -105f));
			this.BuildSidebar(rectTransform2);
			this.BuildArticle(rectTransform2);
			this.BuildFooter();
			foreach (object obj in base.transform)
			{
				Transform transform = (Transform)obj;
				this._libraryChrome.Add(transform.gameObject);
			}
		}

		// Token: 0x060056A3 RID: 22179 RVA: 0x0021AD5C File Offset: 0x00218F5C
		private void BuildSidebar(RectTransform parent)
		{
			RectTransform rectTransform = this.CreatePanel("Wiki Sidebar", parent, PauseMenuTutorialPanel.Sidebar, 8f);
			PauseMenuTutorialPanel.SetRect(rectTransform, Vector2.zero, new Vector2(0f, 1f), Vector2.zero, new Vector2(350f, 0f));
			TMP_Text tmp_Text = this.CreateText("Sidebar Heading", rectTransform, "CONTENTS", 11f, FontStyles.Bold, PauseMenuTutorialPanel.TextQuiet, TextAlignmentOptions.Left, false, null);
			tmp_Text.characterSpacing = 2f;
			PauseMenuTutorialPanel.SetRect(tmp_Text.rectTransform, new Vector2(0f, 1f), Vector2.one, new Vector2(24f, -48f), new Vector2(-18f, -16f));
			RectTransform rectTransform2 = PauseMenuTutorialPanel.CreateRect("Sidebar Viewport", rectTransform);
			PauseMenuTutorialPanel.SetRect(rectTransform2, Vector2.zero, Vector2.one, new Vector2(12f, 12f), new Vector2(-10f, -58f));
			Image image = rectTransform2.gameObject.AddComponent<Image>();
			image.color = Color.clear;
			image.raycastTarget = true;
			rectTransform2.gameObject.AddComponent<RectMask2D>();
			RectTransform rectTransform3 = PauseMenuTutorialPanel.CreateRect("Sidebar Content", rectTransform2);
			rectTransform3.anchorMin = new Vector2(0f, 1f);
			rectTransform3.anchorMax = new Vector2(1f, 1f);
			rectTransform3.pivot = new Vector2(0.5f, 1f);
			rectTransform3.anchoredPosition = Vector2.zero;
			rectTransform3.sizeDelta = Vector2.zero;
			VerticalLayoutGroup verticalLayoutGroup = rectTransform3.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.padding = new RectOffset(8, 8, 2, 18);
			verticalLayoutGroup.spacing = 2f;
			verticalLayoutGroup.childAlignment = TextAnchor.UpperLeft;
			verticalLayoutGroup.childControlWidth = true;
			verticalLayoutGroup.childForceExpandWidth = true;
			verticalLayoutGroup.childControlHeight = true;
			verticalLayoutGroup.childForceExpandHeight = false;
			rectTransform3.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			SmoothTutorialScrollRect smoothTutorialScrollRect = rectTransform.gameObject.AddComponent<SmoothTutorialScrollRect>();
			smoothTutorialScrollRect.viewport = rectTransform2;
			smoothTutorialScrollRect.content = rectTransform3;
			smoothTutorialScrollRect.horizontal = false;
			smoothTutorialScrollRect.vertical = true;
			smoothTutorialScrollRect.movementType = ScrollRect.MovementType.Clamped;
			smoothTutorialScrollRect.inertia = true;
			smoothTutorialScrollRect.scrollSensitivity = 0f;
			smoothTutorialScrollRect.Configure(42f, 0.14f);
			for (int i = 0; i < TutorialCatalog.Categories.Count; i++)
			{
				TutorialCategory tutorialCategory = TutorialCatalog.Categories[i];
				this.BuildSidebarCategory(rectTransform3, tutorialCategory, i > 0);
				foreach (TutorialTopic tutorialTopic in TutorialCatalog.GetTopicsInCategory(tutorialCategory.Id))
				{
					this.BuildSidebarTopic(rectTransform3, tutorialTopic);
				}
			}
		}

		// Token: 0x060056A4 RID: 22180 RVA: 0x0021B000 File Offset: 0x00219200
		private void BuildSidebarCategory(RectTransform parent, TutorialCategory category, bool addSpace)
		{
			Button button = this.CreateFlatButton("Category - " + category.Title, parent);
			button.gameObject.AddComponent<LayoutElement>().preferredHeight = (addSpace ? 42f : 34f);
			PauseMenuTutorialPanel.SetRect(this.CreateText("Label", button.transform, category.Title, 12f, FontStyles.Bold, PauseMenuTutorialPanel.TextSecondary, TextAlignmentOptions.Left, false, (string s) => s.ToUpperInvariant()).rectTransform, Vector2.zero, Vector2.one, new Vector2(12f, 0f), new Vector2(-12f, addSpace ? (-8f) : 0f));
			string categoryId = category.Id;
			button.onClick.AddListener(delegate
			{
				this.OpenFirstTopic(categoryId);
			});
		}

		// Token: 0x060056A5 RID: 22181 RVA: 0x0021B0F8 File Offset: 0x002192F8
		private void BuildSidebarTopic(RectTransform parent, TutorialTopic topic)
		{
			Button button = this.CreateFlatButton("Topic - " + topic.Id, parent);
			button.gameObject.AddComponent<LayoutElement>().preferredHeight = 38f;
			Image image = button.targetGraphic as Image;
			Image image2 = PauseMenuTutorialPanel.CreateRect("Active Indicator", button.transform).gameObject.AddComponent<Image>();
			image2.color = Color.clear;
			image2.raycastTarget = false;
			PauseMenuTutorialPanel.SetRect(image2.rectTransform, Vector2.zero, new Vector2(0f, 1f), new Vector2(0f, 5f), new Vector2(2f, -5f));
			TMP_Text tmp_Text = this.CreateText("Label", button.transform, topic.Title, 14f, FontStyles.Normal, PauseMenuTutorialPanel.TextSecondary, TextAlignmentOptions.Left, false, null);
			PauseMenuTutorialPanel.SetRect(tmp_Text.rectTransform, Vector2.zero, Vector2.one, new Vector2(22f, 0f), new Vector2(-36f, 0f));
			Image image3 = PauseMenuTutorialPanel.CreateRect("Seen", button.transform).gameObject.AddComponent<Image>();
			image3.raycastTarget = false;
			image3.color = (TutorialProgress.HasSeen(topic.Id) ? PauseMenuTutorialPanel.TextQuiet : PauseMenuTutorialPanel.Accent);
			RectTransform rectTransform = image3.rectTransform;
			Vector2 vector = new Vector2(1f, 0.5f);
			rectTransform.anchorMax = vector;
			rectTransform.anchorMin = vector;
			rectTransform.pivot = new Vector2(0.5f, 0.5f);
			rectTransform.anchoredPosition = new Vector2(-18f, 0f);
			rectTransform.sizeDelta = new Vector2(5f, 5f);
			PauseMenuTutorialPanel.Round(image3.gameObject, 3f);
			string topicId = topic.Id;
			button.onClick.AddListener(delegate
			{
				this.OpenTopic(topicId, this._automatic);
			});
			this._sidebarEntries[topic.Id] = new PauseMenuTutorialPanel.SidebarEntry
			{
				Button = button,
				Background = image,
				Indicator = image2,
				SeenDot = image3,
				Label = tmp_Text
			};
		}

		// Token: 0x060056A6 RID: 22182 RVA: 0x0021B330 File Offset: 0x00219530
		private void BuildArticle(RectTransform parent)
		{
			RectTransform rectTransform = PauseMenuTutorialPanel.CreateRect("Tutorial Article", parent);
			PauseMenuTutorialPanel.SetRect(rectTransform, Vector2.zero, Vector2.one, new Vector2(382f, 0f), Vector2.zero);
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = PauseMenuTutorialPanel.ArticleSurface;
			image.raycastTarget = false;
			PauseMenuTutorialPanel.Round(rectTransform.gameObject, 8f);
			Image image2 = PauseMenuTutorialPanel.CreateRect("Sidebar Divider", rectTransform).gameObject.AddComponent<Image>();
			image2.color = PauseMenuTutorialPanel.Divider;
			image2.raycastTarget = false;
			PauseMenuTutorialPanel.SetRect(image2.rectTransform, Vector2.zero, new Vector2(0f, 1f), Vector2.zero, new Vector2(1f, 0f));
			this._breadcrumb = PauseMenuTutorialPanel.CreateRect("Tutorial Breadcrumb", rectTransform);
			Image image3 = this._breadcrumb.gameObject.AddComponent<Image>();
			image3.color = PauseMenuTutorialPanel.ArticleHeader;
			image3.raycastTarget = false;
			PauseMenuTutorialPanel.Round(this._breadcrumb.gameObject, 5f);
			PauseMenuTutorialPanel.SetRect(this._breadcrumb, new Vector2(0f, 1f), Vector2.one, new Vector2(28f, -50f), new Vector2(-18f, -10f));
			HorizontalLayoutGroup horizontalLayoutGroup = this._breadcrumb.gameObject.AddComponent<HorizontalLayoutGroup>();
			horizontalLayoutGroup.spacing = 8f;
			horizontalLayoutGroup.padding = new RectOffset(16, 16, 0, 0);
			horizontalLayoutGroup.childAlignment = TextAnchor.MiddleLeft;
			horizontalLayoutGroup.childControlWidth = true;
			horizontalLayoutGroup.childForceExpandWidth = false;
			horizontalLayoutGroup.childControlHeight = true;
			RectTransform rectTransform2 = PauseMenuTutorialPanel.CreateRect("Article Viewport", rectTransform);
			PauseMenuTutorialPanel.SetRect(rectTransform2, Vector2.zero, Vector2.one, new Vector2(28f, 12f), new Vector2(-18f, -58f));
			Image image4 = rectTransform2.gameObject.AddComponent<Image>();
			image4.color = Color.clear;
			image4.raycastTarget = true;
			rectTransform2.gameObject.AddComponent<RectMask2D>();
			this._articleContent = PauseMenuTutorialPanel.CreateRect("Article Content", rectTransform2);
			this._articleContent.anchorMin = new Vector2(0f, 1f);
			this._articleContent.anchorMax = new Vector2(1f, 1f);
			this._articleContent.pivot = new Vector2(0.5f, 1f);
			this._articleContent.anchoredPosition = Vector2.zero;
			this._articleContent.sizeDelta = Vector2.zero;
			VerticalLayoutGroup verticalLayoutGroup = this._articleContent.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.padding = new RectOffset(22, 30, 18, 42);
			verticalLayoutGroup.spacing = 14f;
			verticalLayoutGroup.childAlignment = TextAnchor.UpperLeft;
			verticalLayoutGroup.childControlWidth = true;
			verticalLayoutGroup.childForceExpandWidth = true;
			verticalLayoutGroup.childControlHeight = true;
			verticalLayoutGroup.childForceExpandHeight = false;
			this._articleContent.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			Scrollbar scrollbar = PauseMenuTutorialPanel.BuildScrollbar(rectTransform);
			SmoothTutorialScrollRect smoothTutorialScrollRect = rectTransform.gameObject.AddComponent<SmoothTutorialScrollRect>();
			this._articleScroll = smoothTutorialScrollRect;
			this._articleScroll.viewport = rectTransform2;
			this._articleScroll.content = this._articleContent;
			this._articleScroll.horizontal = false;
			this._articleScroll.vertical = true;
			this._articleScroll.movementType = ScrollRect.MovementType.Clamped;
			this._articleScroll.inertia = true;
			this._articleScroll.decelerationRate = 0.12f;
			this._articleScroll.scrollSensitivity = 0f;
			smoothTutorialScrollRect.Configure(60f, 0.17f);
			this._articleScroll.verticalScrollbar = scrollbar;
			this._articleScroll.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.Permanent;
		}

		// Token: 0x060056A7 RID: 22183 RVA: 0x0021B6B4 File Offset: 0x002198B4
		private void BuildFooter()
		{
			this._footerBackButton = this.CreateFooterButton("Tutorial Back", base.transform, "BACK TO PAUSE", false);
			PauseMenuTutorialPanel.SetRect(this._footerBackButton.GetComponent<RectTransform>(), Vector2.zero, Vector2.zero, new Vector2(54f, 18f), new Vector2(290f, 62f));
			this._footerBackLabel = this._footerBackButton.GetComponentInChildren<TMP_Text>();
			Button button = this.CreateFooterButton("Continue", base.transform, "CONTINUE", true);
			PauseMenuTutorialPanel.SetRect(button.GetComponent<RectTransform>(), new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(-246f, 18f), new Vector2(-54f, 62f));
			button.onClick.AddListener(new UnityAction(this.ContinueGame));
			this._continueButton = button.gameObject;
			this._continueButton.SetActive(false);
		}

		// Token: 0x060056A8 RID: 22184 RVA: 0x0021B7B8 File Offset: 0x002199B8
		private void DisplayTopic(TutorialTopic topic, bool automatic, bool markSeen)
		{
			this._automatic = automatic;
			this._selectedTopicId = topic.Id;
			this._openTopicId = topic.Id;
			bool flag = automatic && topic.Id == "introduction";
			foreach (GameObject gameObject in this._libraryChrome)
			{
				gameObject.SetActive(!flag);
			}
			if (flag && this._introductionPage == null)
			{
				this._introductionPage = PauseMenuTutorialPanel.CreateRect("First Visit Introduction", base.transform);
				PauseMenuTutorialPanel.Stretch(this._introductionPage, Vector2.zero, Vector2.zero);
				TutorialIntroductionPage.Build(this._introductionPage, this._font, new UnityAction(this.ContinueGame));
			}
			if (this._introductionPage != null)
			{
				this._introductionPage.gameObject.SetActive(flag);
			}
			if (!flag)
			{
				this.PopulateArticle(topic);
				this.BuildBreadcrumb(topic);
			}
			this.RefreshSidebarSelection();
			this._continueButton.SetActive(automatic && !flag);
			this.SetFooterBack(automatic ? "CONTINUE" : "BACK TO PAUSE", automatic ? new UnityAction(this.ContinueGame) : delegate
			{
				Action backToPauseRoot = this._backToPauseRoot;
				if (backToPauseRoot == null)
				{
					return;
				}
				backToPauseRoot();
			});
			if (markSeen || !TutorialProgress.HasSeen(topic.Id))
			{
				TutorialProgress.MarkSeen(topic.Id);
			}
		}

		// Token: 0x060056A9 RID: 22185 RVA: 0x0021B934 File Offset: 0x00219B34
		private void PopulateArticle(TutorialTopic topic)
		{
			PauseMenuTutorialPanel.ClearChildren(this._articleContent);
			if (topic.Id == "introduction")
			{
				RectTransform rectTransform = PauseMenuTutorialPanel.CreateRect("Gameplay Screenshot 1", this._articleContent);
				rectTransform.gameObject.AddComponent<PauseMenuTutorialPanel.GuideImageLayout>();
				TutorialIntroductionPage.Build(rectTransform, this._font, new UnityAction(this.ContinueGame));
				this.ResetArticleScroll();
				return;
			}
			TMP_Text tmp_Text = this.CreateFlowText("Article Category", this._articleContent, PauseMenuTutorialPanel.CategoryTitle(topic.CategoryId), 12f, FontStyles.Bold, PauseMenuTutorialPanel.Accent, (string s) => s.ToUpperInvariant());
			tmp_Text.characterSpacing = 2f;
			tmp_Text.gameObject.AddComponent<LayoutElement>().minHeight = 22f;
			TMP_Text tmp_Text2 = this.CreateFlowText("Article Title", this._articleContent, topic.Title, 34f, FontStyles.Bold, PauseMenuTutorialPanel.TextPrimary, null);
			tmp_Text2.lineSpacing = 2f;
			tmp_Text2.gameObject.AddComponent<LayoutElement>().minHeight = 48f;
			TMP_Text tmp_Text3 = this.CreateFlowText("Article Summary", this._articleContent, topic.Summary, 18f, FontStyles.Normal, PauseMenuTutorialPanel.TextSecondary, null);
			tmp_Text3.lineSpacing = 8f;
			tmp_Text3.gameObject.AddComponent<LayoutElement>().minHeight = 34f;
			this.BuildDivider(this._articleContent);
			bool flag = topic.Id == "cielhome_pc";
			if (!flag)
			{
				this.BuildTopicReferences(topic, "Controls at a glance");
			}
			this.BuildGallery(topic);
			if (flag)
			{
				this.BuildTopicReferences(topic, "Important apps & controls");
			}
			this.BuildSectionHeading(this._articleContent, flag ? "How CielOS works" : "Overview");
			TMP_Text tmp_Text4 = this.CreateFlowText("Description", this._articleContent, topic.Description, 16f, FontStyles.Normal, PauseMenuTutorialPanel.TextPrimary, null);
			tmp_Text4.lineSpacing = 12f;
			tmp_Text4.paragraphSpacing = 10f;
			this.BuildSectionHeading(this._articleContent, "In practice");
			this.BuildCallout(this._articleContent, "WHAT TO DO", topic.PlayerAction, PauseMenuTutorialPanel.Accent);
			this.BuildCallout(this._articleContent, "FEEDBACK FROM THE GAME", topic.ExpectedFeedback, PauseMenuTutorialPanel.TextSecondary);
			if (topic.Tips.Count > 0)
			{
				this.BuildSectionHeading(this._articleContent, "Useful to know");
				for (int i = 0; i < topic.Tips.Count; i++)
				{
					this.BuildBullet(this._articleContent, topic.Tips[i]);
				}
			}
			this.ResetArticleScroll();
		}

		// Token: 0x060056AA RID: 22186 RVA: 0x0021BBB4 File Offset: 0x00219DB4
		private void ResetArticleScroll()
		{
			Canvas.ForceUpdateCanvases();
			SmoothTutorialScrollRect smoothTutorialScrollRect = this._articleScroll as SmoothTutorialScrollRect;
			if (smoothTutorialScrollRect != null)
			{
				smoothTutorialScrollRect.JumpTo(1f);
				return;
			}
			this._articleScroll.verticalNormalizedPosition = 1f;
		}

		// Token: 0x060056AB RID: 22187 RVA: 0x0021BBF4 File Offset: 0x00219DF4
		private void BuildTopicReferences(TutorialTopic topic, string heading)
		{
			this.BuildSectionHeading(this._articleContent, heading);
			if (topic.Bindings.Count == 0)
			{
				this.BuildCommandRow(this._articleContent, topic.Glyph, topic.PlayerAction);
				return;
			}
			RectTransform rectTransform = null;
			for (int i = 0; i < topic.Bindings.Count; i++)
			{
				if (i % 2 == 0)
				{
					rectTransform = PauseMenuTutorialPanel.CreateRect("Control Pair " + (i / 2 + 1).ToString(), this._articleContent);
					HorizontalLayoutGroup horizontalLayoutGroup = rectTransform.gameObject.AddComponent<HorizontalLayoutGroup>();
					horizontalLayoutGroup.spacing = 12f;
					horizontalLayoutGroup.childControlWidth = (horizontalLayoutGroup.childControlHeight = true);
					horizontalLayoutGroup.childForceExpandWidth = (horizontalLayoutGroup.childForceExpandHeight = true);
				}
				this.BuildCommandRow(rectTransform, PauseMenuTutorialPanel.BindingKey(topic.Bindings[i]), topic.Bindings[i]);
			}
		}

		// Token: 0x060056AC RID: 22188 RVA: 0x0021BCD4 File Offset: 0x00219ED4
		private void BuildGallery(TutorialTopic topic)
		{
			int num = ((topic.VisualResources.Count > 0) ? topic.VisualResources.Count : 6);
			List<ValueTuple<Texture2D, int>> list = new List<ValueTuple<Texture2D, int>>();
			for (int i = 0; i < num; i++)
			{
				Texture2D texture2D = Resources.Load<Texture2D>(topic.GetVisualResource(i));
				if (i == 0 && texture2D == null && !string.IsNullOrEmpty(topic.VisualResource))
				{
					texture2D = Resources.Load<Texture2D>(topic.VisualResource);
				}
				if (texture2D != null)
				{
					list.Add(new ValueTuple<Texture2D, int>(texture2D, i));
				}
			}
			if (list.Count == 0)
			{
				return;
			}
			this.BuildSectionHeading(this._articleContent, "Visual guide");
			for (int j = 0; j < list.Count; j++)
			{
				this.BuildGuideImage(this._articleContent, topic, list[j].Item1, list[j].Item2, j + 1, list.Count);
			}
		}

		// Token: 0x060056AD RID: 22189 RVA: 0x0021BDB8 File Offset: 0x00219FB8
		private void BuildGuideImage(RectTransform parent, TutorialTopic topic, Texture2D texture, int index, int number, int count)
		{
			float aspectRatio = TutorialGuideMedia.GetAspectRatio(topic, texture, index);
			RectTransform rectTransform = this.CreatePanel("Gameplay Screenshot " + (index + 1).ToString(), parent, PauseMenuTutorialPanel.Surface, 7f);
			VerticalLayoutGroup verticalLayoutGroup = rectTransform.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.padding = new RectOffset(1, 1, 1, 1);
			verticalLayoutGroup.childControlWidth = (verticalLayoutGroup.childControlHeight = true);
			verticalLayoutGroup.childForceExpandWidth = true;
			verticalLayoutGroup.childForceExpandHeight = false;
			RectTransform rectTransform2 = this.CreatePanel("Image", rectTransform, PauseMenuTutorialPanel.Background, 6f);
			rectTransform2.gameObject.AddComponent<PauseMenuTutorialPanel.GuideImageLayout>().AspectRatio = aspectRatio;
			rectTransform2.gameObject.AddComponent<RectMask2D>();
			if (!TutorialGuideMedia.Build(rectTransform2, topic, texture, index))
			{
				RawImage rawImage = PauseMenuTutorialPanel.CreateRect("Mechanic Screenshot", rectTransform2).gameObject.AddComponent<RawImage>();
				rawImage.texture = texture;
				rawImage.color = Color.white;
				rawImage.uvRect = new Rect(0f, 0f, 1f, 1f);
				PauseMenuTutorialPanel.CenterImage(rawImage, aspectRatio);
				TutorialImageTextLocalization.Bind(rawImage);
			}
			TMP_Text tmp_Text = this.CreateText("Caption", rectTransform, topic.GetVisualCaption(index), 14f, FontStyles.Normal, PauseMenuTutorialPanel.TextSecondary, TextAlignmentOptions.Left, true, null);
			tmp_Text.margin = new Vector4(86f, 14f, 18f, 14f);
			tmp_Text.lineSpacing = 4f;
			tmp_Text.gameObject.AddComponent<LayoutElement>().minHeight = 48f;
			TMP_Text tmp_Text2 = this.CreateText("Image Number", tmp_Text.transform, number.ToString("00") + " / " + count.ToString("00"), 11f, FontStyles.Bold, PauseMenuTutorialPanel.Accent, TextAlignmentOptions.TopLeft, false, null);
			tmp_Text2.characterSpacing = 1f;
			PauseMenuTutorialPanel.SetRect(tmp_Text2.rectTransform, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(18f, -34f), new Vector2(80f, -14f));
		}

		// Token: 0x060056AE RID: 22190 RVA: 0x0021BFC0 File Offset: 0x0021A1C0
		private void BuildCommandRow(RectTransform parent, string key, string action)
		{
			RectTransform rectTransform = this.CreatePanel("Command - " + key, parent, PauseMenuTutorialPanel.Surface, 6f);
			LayoutElement layoutElement = rectTransform.gameObject.AddComponent<LayoutElement>();
			layoutElement.minHeight = 46f;
			layoutElement.minWidth = 0f;
			layoutElement.preferredWidth = 0f;
			layoutElement.flexibleWidth = 1f;
			HorizontalLayoutGroup horizontalLayoutGroup = rectTransform.gameObject.AddComponent<HorizontalLayoutGroup>();
			horizontalLayoutGroup.padding = new RectOffset(8, 14, 8, 8);
			horizontalLayoutGroup.spacing = 12f;
			horizontalLayoutGroup.childAlignment = TextAnchor.MiddleLeft;
			horizontalLayoutGroup.childControlWidth = (horizontalLayoutGroup.childControlHeight = true);
			horizontalLayoutGroup.childForceExpandWidth = (horizontalLayoutGroup.childForceExpandHeight = false);
			RectTransform rectTransform2 = this.CreatePanel("Key", rectTransform, PauseMenuTutorialPanel.KeySurface, 5f);
			LayoutElement layoutElement2 = rectTransform2.gameObject.AddComponent<LayoutElement>();
			layoutElement2.minHeight = (layoutElement2.preferredHeight = 30f);
			TMP_Text tmp_Text = this.CreateText("Key Label", rectTransform2, key, 12f, FontStyles.Bold, PauseMenuTutorialPanel.Accent, TextAlignmentOptions.Center, false, null);
			layoutElement2.minWidth = (layoutElement2.preferredWidth = Mathf.Clamp(tmp_Text.preferredWidth + 16f, 112f, 190f));
			tmp_Text.enableAutoSizing = true;
			tmp_Text.fontSizeMin = 10f;
			tmp_Text.fontSizeMax = 12f;
			PauseMenuTutorialPanel.Stretch(tmp_Text.rectTransform, new Vector2(8f, 0f), new Vector2(-8f, 0f));
			TMP_Text tmp_Text2 = this.CreateText("Action Label", rectTransform, action, 14f, FontStyles.Normal, PauseMenuTutorialPanel.TextPrimary, TextAlignmentOptions.Left, true, new Func<string, string>(PauseMenuTutorialPanel.BindingAction));
			tmp_Text2.lineSpacing = 3f;
			LayoutElement layoutElement3 = tmp_Text2.gameObject.AddComponent<LayoutElement>();
			layoutElement3.minWidth = 0f;
			layoutElement3.flexibleWidth = 1f;
		}

		// Token: 0x060056AF RID: 22191 RVA: 0x0021C18C File Offset: 0x0021A38C
		private void BuildCallout(RectTransform parent, string label, string value, Color accent)
		{
			RectTransform rectTransform = this.CreatePanel("Callout - " + label, parent, PauseMenuTutorialPanel.Surface, 6f);
			rectTransform.gameObject.AddComponent<LayoutElement>().minHeight = 82f;
			VerticalLayoutGroup verticalLayoutGroup = rectTransform.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.padding = new RectOffset(20, 20, 14, 14);
			verticalLayoutGroup.spacing = 8f;
			verticalLayoutGroup.childControlWidth = (verticalLayoutGroup.childControlHeight = true);
			verticalLayoutGroup.childForceExpandWidth = true;
			verticalLayoutGroup.childForceExpandHeight = false;
			Image image = PauseMenuTutorialPanel.CreateRect("Accent", rectTransform).gameObject.AddComponent<Image>();
			image.color = accent;
			image.raycastTarget = false;
			image.gameObject.AddComponent<LayoutElement>().ignoreLayout = true;
			PauseMenuTutorialPanel.SetRect(image.rectTransform, Vector2.zero, new Vector2(0f, 1f), Vector2.zero, new Vector2(2f, 0f));
			this.CreateText("Label", rectTransform, label, 11f, FontStyles.Bold, PauseMenuTutorialPanel.TextQuiet, TextAlignmentOptions.Left, false, null).characterSpacing = 1.5f;
			this.CreateText("Body", rectTransform, value, 15f, FontStyles.Normal, PauseMenuTutorialPanel.TextPrimary, TextAlignmentOptions.Left, true, null).lineSpacing = 4f;
		}

		// Token: 0x060056B0 RID: 22192 RVA: 0x0021C2CC File Offset: 0x0021A4CC
		private void BuildBullet(RectTransform parent, string value)
		{
			RectTransform rectTransform = PauseMenuTutorialPanel.CreateRect("Bullet", parent);
			HorizontalLayoutGroup horizontalLayoutGroup = rectTransform.gameObject.AddComponent<HorizontalLayoutGroup>();
			horizontalLayoutGroup.spacing = 8f;
			horizontalLayoutGroup.padding = new RectOffset(2, 0, 3, 3);
			horizontalLayoutGroup.childAlignment = TextAnchor.UpperLeft;
			horizontalLayoutGroup.childControlWidth = (horizontalLayoutGroup.childControlHeight = true);
			horizontalLayoutGroup.childForceExpandWidth = (horizontalLayoutGroup.childForceExpandHeight = false);
			LayoutElement layoutElement = this.CreateText("Marker", rectTransform, "-", 17f, FontStyles.Bold, PauseMenuTutorialPanel.Accent, TextAlignmentOptions.TopLeft, false, null).gameObject.AddComponent<LayoutElement>();
			layoutElement.minWidth = (layoutElement.preferredWidth = 16f);
			TMP_Text tmp_Text = this.CreateText("Text", rectTransform, value, 15f, FontStyles.Normal, PauseMenuTutorialPanel.TextSecondary, TextAlignmentOptions.TopLeft, true, null);
			tmp_Text.gameObject.AddComponent<LayoutElement>().flexibleWidth = 1f;
			tmp_Text.lineSpacing = 4f;
		}

		// Token: 0x060056B1 RID: 22193 RVA: 0x0021C3B0 File Offset: 0x0021A5B0
		private void BuildSectionHeading(RectTransform parent, string value)
		{
			RectTransform rectTransform = PauseMenuTutorialPanel.CreateRect("Section - " + value, parent);
			LayoutElement layoutElement = rectTransform.gameObject.AddComponent<LayoutElement>();
			layoutElement.minHeight = 38f;
			layoutElement.preferredHeight = 42f;
			Image image = PauseMenuTutorialPanel.CreateRect("Marker", rectTransform).gameObject.AddComponent<Image>();
			image.color = PauseMenuTutorialPanel.Accent;
			image.raycastTarget = false;
			PauseMenuTutorialPanel.SetRect(image.rectTransform, Vector2.zero, new Vector2(0f, 1f), new Vector2(0f, 7f), new Vector2(3f, -7f));
			PauseMenuTutorialPanel.SetRect(this.CreateText("Label", rectTransform, value, 20f, FontStyles.Bold, PauseMenuTutorialPanel.TextPrimary, TextAlignmentOptions.Left, false, null).rectTransform, Vector2.zero, Vector2.one, new Vector2(16f, 0f), Vector2.zero);
			Image image2 = PauseMenuTutorialPanel.CreateRect("Rule", rectTransform).gameObject.AddComponent<Image>();
			image2.color = PauseMenuTutorialPanel.Divider;
			image2.raycastTarget = false;
			PauseMenuTutorialPanel.SetRect(image2.rectTransform, new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(0f, 0f), new Vector2(0f, 1f));
		}

		// Token: 0x060056B2 RID: 22194 RVA: 0x0021C506 File Offset: 0x0021A706
		private void BuildDivider(RectTransform parent)
		{
			RectTransform rectTransform = PauseMenuTutorialPanel.CreateRect("Article Divider", parent);
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = PauseMenuTutorialPanel.Divider;
			image.raycastTarget = false;
			rectTransform.gameObject.AddComponent<LayoutElement>().preferredHeight = 1f;
		}

		// Token: 0x060056B3 RID: 22195 RVA: 0x0021C544 File Offset: 0x0021A744
		private void BuildBreadcrumb(TutorialTopic topic)
		{
			for (int i = 0; i < this._dynamicBreadcrumbs.Count; i++)
			{
				PauseMenuTutorialPanel.DestroyUiObject(this._dynamicBreadcrumbs[i]);
			}
			this._dynamicBreadcrumbs.Clear();
			this.AddBreadcrumb("HOW TO PLAY", PauseMenuTutorialPanel.TextQuiet, 0f, null);
			this.AddBreadcrumb("/", PauseMenuTutorialPanel.TextQuiet, 14f, null);
			this.AddBreadcrumb(PauseMenuTutorialPanel.CategoryTitle(topic.CategoryId), PauseMenuTutorialPanel.TextSecondary, 0f, (string s) => s.ToUpperInvariant());
			this.AddBreadcrumb("/", PauseMenuTutorialPanel.TextQuiet, 14f, null);
			this.AddBreadcrumb(topic.Title, PauseMenuTutorialPanel.TextPrimary, 0f, (string s) => s.ToUpperInvariant());
		}

		// Token: 0x060056B4 RID: 22196 RVA: 0x0021C634 File Offset: 0x0021A834
		private void AddBreadcrumb(string value, Color color, float width = 0f, Func<string, string> transform = null)
		{
			TMP_Text tmp_Text = this.CreateText("Breadcrumb - " + value, this._breadcrumb, value, 11f, FontStyles.Bold, color, TextAlignmentOptions.Left, false, transform);
			tmp_Text.gameObject.AddComponent<LayoutElement>().preferredWidth = ((width > 0f) ? width : Mathf.Clamp(Mathf.Max(tmp_Text.preferredWidth + 12f, (float)value.Length * 8f + 12f), 36f, 310f));
			this._dynamicBreadcrumbs.Add(tmp_Text.gameObject);
		}

		// Token: 0x060056B5 RID: 22197 RVA: 0x0021C6C8 File Offset: 0x0021A8C8
		private void OpenFirstTopic(string categoryId)
		{
			using (IEnumerator<TutorialTopic> enumerator = TutorialCatalog.GetTopicsInCategory(categoryId).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					TutorialTopic tutorialTopic = enumerator.Current;
					this.OpenTopic(tutorialTopic.Id, this._automatic);
				}
			}
		}

		// Token: 0x060056B6 RID: 22198 RVA: 0x0021C724 File Offset: 0x0021A924
		private TutorialTopic ResolveDefaultTopic()
		{
			TutorialTopic tutorialTopic;
			if (!string.IsNullOrEmpty(this._selectedTopicId) && TutorialCatalog.TryGetTopic(this._selectedTopicId, out tutorialTopic))
			{
				return tutorialTopic;
			}
			TutorialTopic tutorialTopic2;
			if (TutorialCatalog.TryGetTopic("introduction", out tutorialTopic2))
			{
				return tutorialTopic2;
			}
			if (TutorialCatalog.Topics.Count <= 0)
			{
				return null;
			}
			return TutorialCatalog.Topics[0];
		}

		// Token: 0x060056B7 RID: 22199 RVA: 0x0021C77C File Offset: 0x0021A97C
		private void RefreshProgress()
		{
			foreach (KeyValuePair<string, PauseMenuTutorialPanel.SidebarEntry> keyValuePair in this._sidebarEntries)
			{
				if (keyValuePair.Value.SeenDot != null)
				{
					keyValuePair.Value.SeenDot.color = (TutorialProgress.HasSeen(keyValuePair.Key) ? PauseMenuTutorialPanel.TextQuiet : PauseMenuTutorialPanel.Accent);
				}
			}
			this.RefreshSidebarSelection();
		}

		// Token: 0x060056B8 RID: 22200 RVA: 0x0021C810 File Offset: 0x0021AA10
		private void RefreshSidebarSelection()
		{
			foreach (KeyValuePair<string, PauseMenuTutorialPanel.SidebarEntry> keyValuePair in this._sidebarEntries)
			{
				bool flag = keyValuePair.Key == this._selectedTopicId;
				PauseMenuTutorialPanel.SidebarEntry value = keyValuePair.Value;
				if (value.Background != null)
				{
					value.Background.color = (flag ? PauseMenuTutorialPanel.SurfaceActive : Color.clear);
				}
				if (value.Indicator != null)
				{
					value.Indicator.color = (flag ? PauseMenuTutorialPanel.Accent : Color.clear);
				}
				if (value.Label != null)
				{
					value.Label.color = (flag ? PauseMenuTutorialPanel.TextPrimary : PauseMenuTutorialPanel.TextSecondary);
					value.Label.fontStyle = (flag ? FontStyles.Bold : FontStyles.Normal);
				}
				if (value.Button != null)
				{
					PauseMenuTutorialPanel.ApplyTint(value.Button, flag ? PauseMenuTutorialPanel.SurfaceActive : Color.clear, PauseMenuTutorialPanel.SurfaceHover);
				}
			}
		}

		// Token: 0x060056B9 RID: 22201 RVA: 0x0021C934 File Offset: 0x0021AB34
		private void SetFooterBack(string label, UnityAction action)
		{
			LocalizedGameText.BindSource(this._footerBackLabel, "tutorial", label, (string s) => "<  " + s);
			this._footerBackButton.onClick.RemoveAllListeners();
			if (action != null)
			{
				this._footerBackButton.onClick.AddListener(action);
			}
		}

		// Token: 0x060056BA RID: 22202 RVA: 0x0021C998 File Offset: 0x0021AB98
		private void ContinueGame()
		{
			if (this._automatic && TutorialFirstUseController.Instance != null && TutorialFirstUseController.Instance.TryContinuePresentation())
			{
				return;
			}
			TutorialFirstUseController instance = TutorialFirstUseController.Instance;
			if (instance != null)
			{
				instance.NotifyDismissed();
			}
			Action continueGame = this._continueGame;
			if (continueGame == null)
			{
				return;
			}
			continueGame();
		}

		// Token: 0x060056BB RID: 22203 RVA: 0x0021C9E8 File Offset: 0x0021ABE8
		private Button CreateFlatButton(string name, Transform parent)
		{
			RectTransform rectTransform = PauseMenuTutorialPanel.CreateRect(name, parent);
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = Color.clear;
			Button button = rectTransform.gameObject.AddComponent<Button>();
			button.targetGraphic = image;
			PauseMenuTutorialPanel.ApplyTint(button, Color.clear, PauseMenuTutorialPanel.SurfaceHover);
			return button;
		}

		// Token: 0x060056BC RID: 22204 RVA: 0x0021CA34 File Offset: 0x0021AC34
		private Button CreateFooterButton(string name, Transform parent, string label, bool primary)
		{
			RectTransform rectTransform = PauseMenuTutorialPanel.CreateRect(name, parent);
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = Color.white;
			if (primary)
			{
				PauseMenuTutorialPanel.Round(rectTransform.gameObject, 5f);
			}
			Button button = rectTransform.gameObject.AddComponent<Button>();
			button.targetGraphic = image;
			PauseMenuTutorialPanel.ApplyTint(button, primary ? PauseMenuTutorialPanel.Accent : Color.clear, primary ? new Color(1f, 0.49f, 0.7f, 1f) : PauseMenuTutorialPanel.SurfaceHover);
			TMP_Text tmp_Text = this.CreateText("Label", rectTransform, label, 12f, FontStyles.Bold, primary ? PauseMenuTutorialPanel.Background : PauseMenuTutorialPanel.TextPrimary, TextAlignmentOptions.Center, false, null);
			tmp_Text.characterSpacing = 1f;
			PauseMenuTutorialPanel.Stretch(tmp_Text.rectTransform, new Vector2(8f, 0f), new Vector2(-8f, 0f));
			return button;
		}

		// Token: 0x060056BD RID: 22205 RVA: 0x0021CB20 File Offset: 0x0021AD20
		private RectTransform CreatePanel(string name, Transform parent, Color color, float radius)
		{
			RectTransform rectTransform = PauseMenuTutorialPanel.CreateRect(name, parent);
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = color;
			image.raycastTarget = false;
			if (radius > 0f)
			{
				PauseMenuTutorialPanel.Round(rectTransform.gameObject, radius);
			}
			return rectTransform;
		}

		// Token: 0x060056BE RID: 22206 RVA: 0x0021CB64 File Offset: 0x0021AD64
		private TMP_Text CreateFlowText(string name, Transform parent, string value, float size, FontStyles style, Color color, Func<string, string> transform = null)
		{
			TMP_Text tmp_Text = this.CreateText(name, parent, value, size, style, color, TextAlignmentOptions.Left, true, transform);
			tmp_Text.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			return tmp_Text;
		}

		// Token: 0x060056BF RID: 22207 RVA: 0x0021CB9C File Offset: 0x0021AD9C
		private TMP_Text CreateText(string name, Transform parent, string value, float size, FontStyles style, Color color, TextAlignmentOptions alignment, bool wrap, Func<string, string> transform = null)
		{
			TextMeshProUGUI textMeshProUGUI = PauseMenuTutorialPanel.CreateRect(name, parent).gameObject.AddComponent<TextMeshProUGUI>();
			textMeshProUGUI.fontSize = size;
			textMeshProUGUI.fontStyle = style;
			textMeshProUGUI.color = color;
			textMeshProUGUI.alignment = alignment;
			textMeshProUGUI.raycastTarget = false;
			textMeshProUGUI.enableWordWrapping = wrap;
			textMeshProUGUI.overflowMode = (wrap ? TextOverflowModes.Overflow : TextOverflowModes.Ellipsis);
			textMeshProUGUI.font = ((this._font != null) ? this._font : TMP_Settings.defaultFontAsset);
			LocalizedGameText localizedGameText = LocalizedGameText.BindSource(textMeshProUGUI, "tutorial", value, transform);
			if (!wrap)
			{
				localizedGameText.FitTranslationToRectangle(false);
			}
			return textMeshProUGUI;
		}

		// Token: 0x060056C0 RID: 22208 RVA: 0x0021CC34 File Offset: 0x0021AE34
		private static void CenterImage(RawImage image, float ratio)
		{
			RectTransform rectTransform = image.rectTransform;
			Vector2 vector = new Vector2(0.5f, 0.5f);
			rectTransform.pivot = vector;
			rectTransform.anchorMin = (rectTransform.anchorMax = vector);
			rectTransform.anchoredPosition = Vector2.zero;
			rectTransform.sizeDelta = new Vector2(1280f, 720f);
			image.raycastTarget = false;
			AspectRatioFitter aspectRatioFitter = image.gameObject.AddComponent<AspectRatioFitter>();
			aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
			aspectRatioFitter.aspectRatio = ratio;
		}

		// Token: 0x060056C1 RID: 22209 RVA: 0x0021CCB0 File Offset: 0x0021AEB0
		private static Scrollbar BuildScrollbar(RectTransform parent)
		{
			RectTransform rectTransform = PauseMenuTutorialPanel.CreateRect("Scrollbar", parent);
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = PauseMenuTutorialPanel.ScrollTrack;
			rectTransform.anchorMin = new Vector2(1f, 0f);
			rectTransform.anchorMax = Vector2.one;
			rectTransform.pivot = new Vector2(1f, 0.5f);
			rectTransform.offsetMin = new Vector2(-5f, 12f);
			rectTransform.offsetMax = new Vector2(-1f, -58f);
			PauseMenuTutorialPanel.Round(image.gameObject, 2f);
			RectTransform rectTransform2 = PauseMenuTutorialPanel.CreateRect("Sliding Area", rectTransform);
			PauseMenuTutorialPanel.Stretch(rectTransform2, Vector2.zero, Vector2.zero);
			RectTransform rectTransform3 = PauseMenuTutorialPanel.CreateRect("Handle", rectTransform2);
			PauseMenuTutorialPanel.Stretch(rectTransform3, Vector2.zero, Vector2.zero);
			Image image2 = rectTransform3.gameObject.AddComponent<Image>();
			image2.color = PauseMenuTutorialPanel.ScrollHandle;
			PauseMenuTutorialPanel.Round(image2.gameObject, 2f);
			Scrollbar scrollbar = image.gameObject.AddComponent<Scrollbar>();
			scrollbar.handleRect = rectTransform3;
			scrollbar.targetGraphic = image2;
			scrollbar.direction = Scrollbar.Direction.BottomToTop;
			return scrollbar;
		}

		// Token: 0x060056C2 RID: 22210 RVA: 0x0021CDCC File Offset: 0x0021AFCC
		private static void ApplyTint(Selectable selectable, Color normal, Color hover)
		{
			ColorBlock colors = selectable.colors;
			colors.normalColor = normal;
			colors.highlightedColor = hover;
			colors.selectedColor = hover;
			colors.pressedColor = new Color(1f, 1f, 1f, 0.1f);
			colors.disabledColor = new Color(normal.r, normal.g, normal.b, normal.a * 0.55f);
			colors.colorMultiplier = 1f;
			colors.fadeDuration = 0.06f;
			selectable.colors = colors;
		}

		// Token: 0x060056C3 RID: 22211 RVA: 0x0021CE64 File Offset: 0x0021B064
		private static TutorialCategory FindCategory(string categoryId)
		{
			for (int i = 0; i < TutorialCatalog.Categories.Count; i++)
			{
				if (TutorialCatalog.Categories[i].Id == categoryId)
				{
					return TutorialCatalog.Categories[i];
				}
			}
			return null;
		}

		// Token: 0x060056C4 RID: 22212 RVA: 0x0021CEAC File Offset: 0x0021B0AC
		private static string CategoryTitle(string categoryId)
		{
			TutorialCategory tutorialCategory = PauseMenuTutorialPanel.FindCategory(categoryId);
			if (tutorialCategory == null)
			{
				return "TUTORIAL";
			}
			return tutorialCategory.Title;
		}

		// Token: 0x060056C5 RID: 22213 RVA: 0x0021CED0 File Offset: 0x0021B0D0
		private static string BindingKey(string binding)
		{
			int num2;
			int num = PauseMenuTutorialPanel.FindBindingSeparator(binding, out num2);
			if (num <= 0)
			{
				return binding.Trim();
			}
			return binding.Substring(0, num).Trim();
		}

		// Token: 0x060056C6 RID: 22214 RVA: 0x0021CF00 File Offset: 0x0021B100
		private static string BindingAction(string binding)
		{
			int num2;
			int num = PauseMenuTutorialPanel.FindBindingSeparator(binding, out num2);
			if (num < 0 || num + num2 >= binding.Length)
			{
				return binding.Trim();
			}
			return binding.Substring(num + num2).Trim();
		}

		// Token: 0x060056C7 RID: 22215 RVA: 0x0021CF3C File Offset: 0x0021B13C
		private static int FindBindingSeparator(string binding, out int length)
		{
			int num = binding.IndexOf('•');
			int num2 = binding.IndexOf(" - ", StringComparison.Ordinal);
			if (num2 >= 0 && (num < 0 || num2 < num))
			{
				length = 3;
				return num2;
			}
			length = 1;
			return num;
		}

		// Token: 0x060056C8 RID: 22216 RVA: 0x0021CF77 File Offset: 0x0021B177
		private static RectTransform CreateRect(string name, Transform parent)
		{
			RectTransform component = new GameObject(name, new Type[] { typeof(RectTransform) }).GetComponent<RectTransform>();
			component.SetParent(parent, false);
			component.localScale = Vector3.one;
			return component;
		}

		// Token: 0x060056C9 RID: 22217 RVA: 0x0018F85B File Offset: 0x0018DA5B
		private static void SetRect(RectTransform rect, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax)
		{
			rect.anchorMin = anchorMin;
			rect.anchorMax = anchorMax;
			rect.offsetMin = offsetMin;
			rect.offsetMax = offsetMax;
		}

		// Token: 0x060056CA RID: 22218 RVA: 0x0021CFAA File Offset: 0x0021B1AA
		private static void Stretch(RectTransform rect, Vector2 offsetMin, Vector2 offsetMax)
		{
			PauseMenuTutorialPanel.SetRect(rect, Vector2.zero, Vector2.one, offsetMin, offsetMax);
		}

		// Token: 0x060056CB RID: 22219 RVA: 0x001A3764 File Offset: 0x001A1964
		private static void Round(GameObject target, float radius)
		{
			target.AddComponent<CornerRounder>().SetRadii(new Vector4(radius, radius, radius, radius));
		}

		// Token: 0x060056CC RID: 22220 RVA: 0x0021CFC0 File Offset: 0x0021B1C0
		private static void ClearChildren(Transform parent)
		{
			if (parent == null)
			{
				return;
			}
			for (int i = parent.childCount - 1; i >= 0; i--)
			{
				PauseMenuTutorialPanel.DestroyUiObject(parent.GetChild(i).gameObject);
			}
		}

		// Token: 0x060056CD RID: 22221 RVA: 0x000A4FF8 File Offset: 0x000A31F8
		private static void DestroyUiObject(GameObject value)
		{
			if (value == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				Object.Destroy(value);
				return;
			}
			Object.DestroyImmediate(value);
		}

		// Token: 0x04005713 RID: 22291
		private static readonly Color Background = new Color(0.025f, 0.018f, 0.029f, 1f);

		// Token: 0x04005714 RID: 22292
		private static readonly Color Sidebar = new Color(0.075f, 0.056f, 0.082f, 1f);

		// Token: 0x04005715 RID: 22293
		private static readonly Color ArticleSurface = new Color(0.07f, 0.052f, 0.078f, 1f);

		// Token: 0x04005716 RID: 22294
		private static readonly Color ArticleHeader = new Color(0.095f, 0.069f, 0.104f, 1f);

		// Token: 0x04005717 RID: 22295
		private static readonly Color Surface = new Color(0.125f, 0.094f, 0.132f, 1f);

		// Token: 0x04005718 RID: 22296
		private static readonly Color SurfaceHover = new Color(0.175f, 0.13f, 0.178f, 1f);

		// Token: 0x04005719 RID: 22297
		private static readonly Color SurfaceActive = new Color(0.205f, 0.143f, 0.194f, 1f);

		// Token: 0x0400571A RID: 22298
		private static readonly Color Divider = new Color(0.31f, 0.245f, 0.302f, 0.72f);

		// Token: 0x0400571B RID: 22299
		private static readonly Color Accent = new Color(1f, 0.3f, 0.58f, 1f);

		// Token: 0x0400571C RID: 22300
		private static readonly Color TextPrimary = new Color(1f, 0.975f, 0.99f, 1f);

		// Token: 0x0400571D RID: 22301
		private static readonly Color TextSecondary = new Color(0.84f, 0.78f, 0.82f, 1f);

		// Token: 0x0400571E RID: 22302
		private static readonly Color TextQuiet = new Color(0.62f, 0.55f, 0.6f, 1f);

		// Token: 0x0400571F RID: 22303
		private static readonly Color KeySurface = new Color(0.24f, 0.15f, 0.22f, 1f);

		// Token: 0x04005720 RID: 22304
		private static readonly Color ScrollTrack = new Color(0.12f, 0.09f, 0.13f, 1f);

		// Token: 0x04005721 RID: 22305
		private static readonly Color ScrollHandle = new Color(0.9f, 0.49f, 0.67f, 0.82f);

		// Token: 0x04005722 RID: 22306
		private readonly Dictionary<string, PauseMenuTutorialPanel.SidebarEntry> _sidebarEntries = new Dictionary<string, PauseMenuTutorialPanel.SidebarEntry>();

		// Token: 0x04005723 RID: 22307
		private readonly List<GameObject> _dynamicBreadcrumbs = new List<GameObject>();

		// Token: 0x04005724 RID: 22308
		private TMP_FontAsset _font;

		// Token: 0x04005725 RID: 22309
		private Action _backToPauseRoot;

		// Token: 0x04005726 RID: 22310
		private Action _continueGame;

		// Token: 0x04005727 RID: 22311
		private RectTransform _articleContent;

		// Token: 0x04005728 RID: 22312
		private RectTransform _breadcrumb;

		// Token: 0x04005729 RID: 22313
		private ScrollRect _articleScroll;

		// Token: 0x0400572A RID: 22314
		private Button _footerBackButton;

		// Token: 0x0400572B RID: 22315
		private TMP_Text _footerBackLabel;

		// Token: 0x0400572C RID: 22316
		private GameObject _continueButton;

		// Token: 0x0400572D RID: 22317
		private string _selectedTopicId;

		// Token: 0x0400572E RID: 22318
		private string _openTopicId;

		// Token: 0x0400572F RID: 22319
		private bool _automatic;

		// Token: 0x04005730 RID: 22320
		private bool _built;

		// Token: 0x04005731 RID: 22321
		private RectTransform _introductionPage;

		// Token: 0x04005732 RID: 22322
		private readonly List<GameObject> _libraryChrome = new List<GameObject>();

		// Token: 0x02000BC7 RID: 3015
		private sealed class SidebarEntry
		{
			// Token: 0x04005733 RID: 22323
			public Button Button;

			// Token: 0x04005734 RID: 22324
			public Image Background;

			// Token: 0x04005735 RID: 22325
			public Image Indicator;

			// Token: 0x04005736 RID: 22326
			public Image SeenDot;

			// Token: 0x04005737 RID: 22327
			public TMP_Text Label;
		}

		// Token: 0x02000BC8 RID: 3016
		private sealed class GuideImageLayout : UIBehaviour, ILayoutElement
		{
			// Token: 0x17000E64 RID: 3684
			// (get) Token: 0x060056D3 RID: 22227 RVA: 0x0008D6F5 File Offset: 0x0008B8F5
			public float minWidth
			{
				get
				{
					return 0f;
				}
			}

			// Token: 0x17000E65 RID: 3685
			// (get) Token: 0x060056D4 RID: 22228 RVA: 0x0021D205 File Offset: 0x0021B405
			public float preferredWidth
			{
				get
				{
					return -1f;
				}
			}

			// Token: 0x17000E66 RID: 3686
			// (get) Token: 0x060056D5 RID: 22229 RVA: 0x0021D20C File Offset: 0x0021B40C
			public float flexibleWidth
			{
				get
				{
					return 1f;
				}
			}

			// Token: 0x17000E67 RID: 3687
			// (get) Token: 0x060056D6 RID: 22230 RVA: 0x0008D6F5 File Offset: 0x0008B8F5
			public float minHeight
			{
				get
				{
					return 0f;
				}
			}

			// Token: 0x17000E68 RID: 3688
			// (get) Token: 0x060056D7 RID: 22231 RVA: 0x0021D214 File Offset: 0x0021B414
			public float preferredHeight
			{
				get
				{
					return Mathf.Max(0f, ((RectTransform)base.transform).rect.width) / Mathf.Max(0.01f, this.AspectRatio);
				}
			}

			// Token: 0x17000E69 RID: 3689
			// (get) Token: 0x060056D8 RID: 22232 RVA: 0x0008D6F5 File Offset: 0x0008B8F5
			public float flexibleHeight
			{
				get
				{
					return 0f;
				}
			}

			// Token: 0x17000E6A RID: 3690
			// (get) Token: 0x060056D9 RID: 22233 RVA: 0x0002AE8C File Offset: 0x0002908C
			public int layoutPriority
			{
				get
				{
					return 1;
				}
			}

			// Token: 0x060056DA RID: 22234 RVA: 0x00004F31 File Offset: 0x00003131
			public void CalculateLayoutInputHorizontal()
			{
			}

			// Token: 0x060056DB RID: 22235 RVA: 0x00004F31 File Offset: 0x00003131
			public void CalculateLayoutInputVertical()
			{
			}

			// Token: 0x04005738 RID: 22328
			public float AspectRatio = 1.7777778f;
		}
	}
}
