using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Features.CensorBars;
using Features.CielOS.UI;
using Features.CielOS.WindowManager;
using Features.Localization;
using Features.OxGram;
using Features.Rank;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.CielOS.Apps.Social
{
	// Token: 0x0200143F RID: 5183
	public class CielXApp : MonoBehaviour
	{
		// Token: 0x0600A00A RID: 40970 RVA: 0x00420D34 File Offset: 0x0041EF34
		private void Awake()
		{
			this._rng = (int)(Time.realtimeSinceStartup * 1000f) | 1;
			CielOSTheme active = CielOSTheme.Active;
			if (active != null)
			{
				this._font = active.uiFont;
				if (active.accent.a > 0f)
				{
					this._accent = active.accent;
				}
			}
			this._self = (RectTransform)base.transform;
			foreach (object obj in base.transform)
			{
				((Transform)obj).gameObject.SetActive(false);
			}
			CielXApp.Stretch(this._self);
			Image image = this._self.GetComponent<Image>();
			if (image == null)
			{
				image = this._self.gameObject.AddComponent<Image>();
			}
			image.color = CielXApp.Bg;
			this.BuildLoginScreen();
			this.BuildFeedScreen();
			if (CielXState.IsLoggedIn)
			{
				this.ShowFeed();
			}
			else
			{
				this.ShowLogin();
			}
			this.MaximizeWindow();
		}

		// Token: 0x0600A00B RID: 40971 RVA: 0x00420E54 File Offset: 0x0041F054
		private void OnEnable()
		{
			RankProgressionService.Changed += this.RefreshPhotoPrivacy;
			this.RefreshPhotoPrivacy();
			this.MaximizeWindow();
			this.RefreshRightColumnVisibility();
			if (this._picker != null)
			{
				OxGramGallery.DecodeWhileOpen(this._picker.gameObject);
			}
		}

		// Token: 0x0600A00C RID: 40972 RVA: 0x00420EA2 File Offset: 0x0041F0A2
		private void OnRectTransformDimensionsChange()
		{
			this.RefreshRightColumnVisibility();
			this.RefreshPhotoPickerLayout();
		}

		// Token: 0x0600A00D RID: 40973 RVA: 0x00420EB0 File Offset: 0x0041F0B0
		private void RefreshRightColumnVisibility()
		{
			if (this._self == null || this._rightColumn == null)
			{
				return;
			}
			bool flag = this._self.rect.width - 268f - 920f - 48f >= 260f;
			if (this._rightColumn.gameObject.activeSelf != flag)
			{
				this._rightColumn.gameObject.SetActive(flag);
			}
		}

		// Token: 0x0600A00E RID: 40974 RVA: 0x00420F30 File Offset: 0x0041F130
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

		// Token: 0x0600A00F RID: 40975 RVA: 0x00420FF9 File Offset: 0x0041F1F9
		private void OnDisable()
		{
			RankProgressionService.Changed -= this.RefreshPhotoPrivacy;
			if (this._ticker != null)
			{
				base.StopCoroutine(this._ticker);
				this._ticker = null;
			}
		}

		// Token: 0x0600A010 RID: 40976 RVA: 0x00421027 File Offset: 0x0041F227
		private void ShowLogin()
		{
			this._loginScreen.gameObject.SetActive(true);
			this._feedScreen.gameObject.SetActive(false);
			if (this._ticker != null)
			{
				base.StopCoroutine(this._ticker);
				this._ticker = null;
			}
		}

		// Token: 0x0600A011 RID: 40977 RVA: 0x00421068 File Offset: 0x0041F268
		private void ShowFeed()
		{
			this._loginScreen.gameObject.SetActive(false);
			this._feedScreen.gameObject.SetActive(true);
			if (this._railHandleLabel != null)
			{
				this._railHandleLabel.text = CielXState.DisplayHandle;
			}
			this.RebuildFeed();
			this.RefreshFollowerLabel();
			if (this._ticker == null && base.isActiveAndEnabled)
			{
				this._ticker = base.StartCoroutine(this.LiveTicker());
			}
		}

		// Token: 0x0600A012 RID: 40978 RVA: 0x004210E4 File Offset: 0x0041F2E4
		private void BuildLoginScreen()
		{
			this._loginScreen = this.NewChild("LoginScreen", this._self);
			CielXApp.Stretch(this._loginScreen);
			RectTransform rectTransform = this.NewChild("LoginCard", this._loginScreen);
			RectTransform rectTransform2 = rectTransform;
			RectTransform rectTransform3 = rectTransform;
			Vector2 vector = new Vector2(0.5f, 0.5f);
			rectTransform3.anchorMax = vector;
			rectTransform2.anchorMin = vector;
			rectTransform.pivot = new Vector2(0.5f, 0.5f);
			rectTransform.sizeDelta = new Vector2(380f, 440f);
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = CielXApp.Card;
			CielXApp.RoundOpaque(image);
			VerticalLayoutGroup verticalLayoutGroup = rectTransform.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.padding = new RectOffset(30, 30, 30, 30);
			verticalLayoutGroup.spacing = 14f;
			verticalLayoutGroup.childAlignment = TextAnchor.UpperCenter;
			verticalLayoutGroup.childControlWidth = true;
			verticalLayoutGroup.childForceExpandWidth = true;
			verticalLayoutGroup.childControlHeight = true;
			verticalLayoutGroup.childForceExpandHeight = false;
			RectTransform rectTransform4 = this.NewChild("LogoRow", rectTransform);
			this.FixedHeight(rectTransform4, 60f);
			RectTransform rectTransform5 = (RectTransform)this.AppLogo(rectTransform4, 56f).transform;
			vector = new Vector2(0.5f, 0.5f);
			rectTransform5.anchorMax = vector;
			rectTransform5.anchorMin = vector;
			rectTransform5.pivot = new Vector2(0.5f, 0.5f);
			rectTransform5.anchoredPosition = Vector2.zero;
			TMP_Text tmp_Text = this.MakeLabel(rectTransform, "People", 32f, FontStyles.Bold, CielXApp.TextMain, TextAlignmentOptions.Center, false);
			this.FixedHeight((RectTransform)tmp_Text.transform, 40f);
			TMP_Text tmp_Text2 = this.MakeLocalizedLabel(rectTransform, "see what people are up to.", 14f, FontStyles.Normal, CielXApp.TextDim, TextAlignmentOptions.Center, false);
			this.FixedHeight((RectTransform)tmp_Text2.transform, 22f);
			this.Spacer(rectTransform, 6f);
			TMP_InputField handle = this.MakeInput(rectTransform, "@handle", false);
			this.FixedHeight((RectTransform)handle.transform, 46f);
			TMP_InputField pass = this.MakeInput(rectTransform, "password", true);
			this.FixedHeight((RectTransform)pass.transform, 46f);
			TMP_Text errorLabel = this.MakeLabel(rectTransform, "", 12f, FontStyles.Normal, CielXApp.LikeColor, TextAlignmentOptions.Center, false);
			this.FixedHeight((RectTransform)errorLabel.transform, 16f);
			RectTransform rectTransform6 = this.MakeButton(rectTransform, "Log in", this._accent, Color.white);
			this.FixedHeight((RectTransform)rectTransform6.transform, 48f);
			rectTransform6.GetComponent<Button>().onClick.AddListener(delegate
			{
				string text = handle.text;
				string text2 = pass.text;
				if (string.IsNullOrWhiteSpace(text) || string.IsNullOrEmpty(text2))
				{
					LocalizedGameText.BindSource(errorLabel, "cielx_ui", "enter a handle and any password", null);
					return;
				}
				if (!CielXState.LogIn(text))
				{
					LocalizedGameText.BindSource(errorLabel, "cielx_ui", "that handle has no usable characters", null);
					return;
				}
				LocalizedGameText.BindSource(errorLabel, "cielx_ui", "", null);
				this.ShowFeed();
			});
			TMP_Text tmp_Text3 = this.MakeLocalizedLabel(rectTransform, "(any password works — it's not that kind of app)", 11f, FontStyles.Italic, CielXApp.TextDim, TextAlignmentOptions.Center, false);
			this.FixedHeight((RectTransform)tmp_Text3.transform, 16f);
			if (CielXState.HasAccount)
			{
				handle.text = CielXState.Handle;
			}
		}

		// Token: 0x0600A013 RID: 40979 RVA: 0x004213FC File Offset: 0x0041F5FC
		private void BuildFeedScreen()
		{
			this._feedScreen = this.NewChild("FeedScreen", this._self);
			CielXApp.Stretch(this._feedScreen);
			this._feedScreen.gameObject.SetActive(false);
			this.BuildLeftRail(this._feedScreen);
			this.BuildCenterColumn(this._feedScreen);
			this.BuildRightColumn(this._feedScreen);
			this.RefreshRightColumnVisibility();
		}

		// Token: 0x0600A014 RID: 40980 RVA: 0x00421468 File Offset: 0x0041F668
		private void BuildRightColumn(RectTransform parent)
		{
			float num = 1188f;
			RectTransform rectTransform = this.NewChild("RightColumn", parent);
			this._rightColumn = rectTransform;
			rectTransform.anchorMin = new Vector2(0f, 0f);
			rectTransform.anchorMax = new Vector2(1f, 1f);
			rectTransform.offsetMin = new Vector2(num + 24f, 0f);
			rectTransform.offsetMax = new Vector2(-24f, 0f);
			RectTransform rectTransform2 = this.NewChild("Search", rectTransform);
			rectTransform2.anchorMin = new Vector2(0f, 1f);
			rectTransform2.anchorMax = new Vector2(1f, 1f);
			rectTransform2.pivot = new Vector2(0.5f, 1f);
			rectTransform2.sizeDelta = new Vector2(0f, 44f);
			rectTransform2.anchoredPosition = new Vector2(0f, -12f);
			Image image = rectTransform2.gameObject.AddComponent<Image>();
			image.color = CielXApp.Bg;
			CielXApp.RoundOpaque(image);
			this.HairlineBox(rectTransform2);
			Image image2 = this.MakeIcon(rectTransform2, CielOSIconId.Search, 18, CielXApp.TextDim);
			RectTransform rectTransform3 = image2.rectTransform;
			RectTransform rectTransform4 = image2.rectTransform;
			Vector2 vector = new Vector2(0f, 0.5f);
			rectTransform4.anchorMax = vector;
			rectTransform3.anchorMin = vector;
			image2.rectTransform.pivot = new Vector2(0f, 0.5f);
			image2.rectTransform.anchoredPosition = new Vector2(16f, 0f);
			image2.rectTransform.sizeDelta = new Vector2(18f, 18f);
			RectTransform rectTransform5 = (RectTransform)this.MakeLocalizedLabel(rectTransform2, "Search", 15f, FontStyles.Normal, CielXApp.TextDim, TextAlignmentOptions.Left, false).transform;
			rectTransform5.anchorMin = new Vector2(0f, 0f);
			rectTransform5.anchorMax = new Vector2(1f, 1f);
			rectTransform5.offsetMin = new Vector2(44f, 0f);
			rectTransform5.offsetMax = new Vector2(-12f, 0f);
			float num2 = -72f;
			RectTransform rectTransform6 = this.TopCard(rectTransform, "StatsCard", num2, 226f);
			float num3 = -18f;
			this.CardLabel(rectTransform6, "Your stats", 20f, FontStyles.Bold, CielXApp.TextMain, num3, 28f, 18f, TextAlignmentOptions.Center);
			num3 -= 30f;
			this.CardLabel(rectTransform6, "all-time, across every post", 12f, FontStyles.Italic, CielXApp.TextDim, num3, 16f, 18f, TextAlignmentOptions.Center);
			num3 -= 28f;
			this._statLikes = this.AddStatRow(rectTransform6, CielOSIconId.Heart, CielXApp.LikeColor, "likes", num3, 18f);
			num3 -= 44f;
			this._statReposts = this.AddStatRow(rectTransform6, CielOSIconId.RepostArrows, CielXApp.RepostColor, "reposts", num3, 18f);
			num3 -= 44f;
			this._statReplies = this.AddStatRow(rectTransform6, CielOSIconId.ReplyBubble, this._accent, "comments", num3, 18f);
			CielXSeed.Trend[] array = CielXSeed.Trending();
			float num4 = 56f + (float)array.Length * 72f + 12f;
			RectTransform rectTransform7 = this.TopCard(rectTransform, "TrendCard", num2 - 226f - 16f, num4);
			float num5 = -18f;
			this.CardLabel(rectTransform7, "Trends for you", 20f, FontStyles.Bold, CielXApp.TextMain, num5, 28f, 18f, TextAlignmentOptions.Left);
			num5 -= 44f;
			for (int i = 0; i < array.Length; i++)
			{
				this.AddTrendRow(rectTransform7, array[i], num5, 72f, 18f);
				if (i < array.Length - 1)
				{
					this.AddRowSeparator(rectTransform7, num5 - 72f, 18f);
				}
				num5 -= 72f;
			}
			this.RefreshLifetimePanel();
		}

		// Token: 0x0600A015 RID: 40981 RVA: 0x0042185C File Offset: 0x0041FA5C
		private RectTransform TopCard(RectTransform col, string name, float y, float height)
		{
			RectTransform rectTransform = this.NewChild(name, col);
			rectTransform.anchorMin = new Vector2(0f, 1f);
			rectTransform.anchorMax = new Vector2(1f, 1f);
			rectTransform.pivot = new Vector2(0.5f, 1f);
			rectTransform.sizeDelta = new Vector2(0f, height);
			rectTransform.anchoredPosition = new Vector2(0f, y);
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = CielXApp.Bg;
			CielXApp.RoundOpaque(image);
			this.HairlineBox(rectTransform);
			return rectTransform;
		}

		// Token: 0x0600A016 RID: 40982 RVA: 0x004218F8 File Offset: 0x0041FAF8
		private void HairlineBox(RectTransform target)
		{
			CielXApp.<>c__DisplayClass59_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.target = target;
			CS$<>8__locals1.c = new Color(1f, 1f, 1f, 0.12f);
			this.<HairlineBox>g__Edge|59_0(new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0f, -1f), new Vector2(0f, 0f), ref CS$<>8__locals1);
			this.<HairlineBox>g__Edge|59_0(new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(0f, 0f), new Vector2(0f, 1f), ref CS$<>8__locals1);
			this.<HairlineBox>g__Edge|59_0(new Vector2(0f, 0f), new Vector2(0f, 1f), new Vector2(0f, 0f), new Vector2(1f, 0f), ref CS$<>8__locals1);
			this.<HairlineBox>g__Edge|59_0(new Vector2(1f, 0f), new Vector2(1f, 1f), new Vector2(-1f, 0f), new Vector2(0f, 0f), ref CS$<>8__locals1);
		}

		// Token: 0x0600A017 RID: 40983 RVA: 0x00421A48 File Offset: 0x0041FC48
		private void AddRowSeparator(RectTransform card, float y, float pad)
		{
			RectTransform rectTransform = this.NewChild("Sep", card);
			rectTransform.anchorMin = new Vector2(0f, 1f);
			rectTransform.anchorMax = new Vector2(1f, 1f);
			rectTransform.pivot = new Vector2(0.5f, 1f);
			rectTransform.offsetMin = new Vector2(pad, 0f);
			rectTransform.offsetMax = new Vector2(-pad, 0f);
			rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 1f);
			rectTransform.anchoredPosition = new Vector2(0f, y);
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = new Color(1f, 1f, 1f, 0.08f);
			CielXApp.RoundOpaque(image);
			image.raycastTarget = false;
		}

		// Token: 0x0600A018 RID: 40984 RVA: 0x00421B24 File Offset: 0x0041FD24
		private TMP_Text CardLabel(RectTransform card, string text, float size, FontStyles style, Color color, float y, float h, float pad, TextAlignmentOptions align = TextAlignmentOptions.Left)
		{
			TMP_Text tmp_Text = this.MakeLocalizedLabel(card, text, size, style, color, align, false);
			RectTransform rectTransform = (RectTransform)tmp_Text.transform;
			rectTransform.anchorMin = new Vector2(0f, 1f);
			rectTransform.anchorMax = new Vector2(1f, 1f);
			rectTransform.pivot = new Vector2(0.5f, 1f);
			rectTransform.offsetMin = new Vector2(pad, 0f);
			rectTransform.offsetMax = new Vector2(-pad, 0f);
			rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, h);
			rectTransform.anchoredPosition = new Vector2(0f, y);
			return tmp_Text;
		}

		// Token: 0x0600A019 RID: 40985 RVA: 0x00421BDC File Offset: 0x0041FDDC
		private TMP_Text AddStatRow(RectTransform card, CielOSIconId icon, Color color, string label, float y, float pad)
		{
			RectTransform rectTransform = this.NewChild("Stat_" + label, card);
			rectTransform.anchorMin = new Vector2(0f, 1f);
			rectTransform.anchorMax = new Vector2(1f, 1f);
			rectTransform.pivot = new Vector2(0.5f, 1f);
			rectTransform.offsetMin = new Vector2(pad, 0f);
			rectTransform.offsetMax = new Vector2(-pad, 0f);
			rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 40f);
			rectTransform.anchoredPosition = new Vector2(0f, y);
			Image image = this.MakeIcon(rectTransform, icon, 24, color);
			RectTransform rectTransform2 = image.rectTransform;
			RectTransform rectTransform3 = image.rectTransform;
			Vector2 vector = new Vector2(0f, 0.5f);
			rectTransform3.anchorMax = vector;
			rectTransform2.anchorMin = vector;
			image.rectTransform.pivot = new Vector2(0f, 0.5f);
			image.rectTransform.anchoredPosition = new Vector2(0f, 0f);
			image.rectTransform.sizeDelta = new Vector2(24f, 24f);
			TMP_Text tmp_Text = this.MakeLabel(rectTransform, "0", 24f, FontStyles.Bold, CielXApp.TextMain, TextAlignmentOptions.Left, false);
			RectTransform rectTransform4 = (RectTransform)tmp_Text.transform;
			rectTransform4.anchorMin = new Vector2(0f, 0f);
			rectTransform4.anchorMax = new Vector2(1f, 1f);
			rectTransform4.offsetMin = new Vector2(38f, 0f);
			rectTransform4.offsetMax = new Vector2(-100f, 0f);
			RectTransform rectTransform5 = (RectTransform)this.MakeLocalizedLabel(rectTransform, label, 14f, FontStyles.Normal, CielXApp.TextDim, TextAlignmentOptions.Right, false).transform;
			rectTransform5.anchorMin = new Vector2(1f, 0f);
			rectTransform5.anchorMax = new Vector2(1f, 1f);
			rectTransform5.pivot = new Vector2(1f, 0.5f);
			rectTransform5.sizeDelta = new Vector2(94f, 0f);
			rectTransform5.anchoredPosition = new Vector2(0f, 0f);
			return tmp_Text;
		}

		// Token: 0x0600A01A RID: 40986 RVA: 0x00421E18 File Offset: 0x00420018
		private void AddTrendRow(RectTransform card, CielXSeed.Trend t, float y, float rowH, float pad)
		{
			RectTransform rectTransform = this.NewChild("Trend", card);
			rectTransform.anchorMin = new Vector2(0f, 1f);
			rectTransform.anchorMax = new Vector2(1f, 1f);
			rectTransform.pivot = new Vector2(0.5f, 1f);
			rectTransform.offsetMin = new Vector2(6f, 0f);
			rectTransform.offsetMax = new Vector2(-6f, 0f);
			rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rowH);
			rectTransform.anchoredPosition = new Vector2(0f, y);
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = new Color(0f, 0f, 0f, 0f);
			CielXApp.RoundOpaque(image);
			Button button = rectTransform.gameObject.AddComponent<Button>();
			button.targetGraphic = image;
			ColorBlock colors = button.colors;
			colors.normalColor = new Color(0f, 0f, 0f, 0f);
			colors.highlightedColor = CielXApp.CardHover;
			colors.pressedColor = CielXApp.CardHover;
			colors.selectedColor = new Color(0f, 0f, 0f, 0f);
			button.colors = colors;
			float num = pad - 6f;
			this.CardLabelAt(rectTransform, t.topic, 12f, FontStyles.Normal, CielXApp.TextDim, -14f, 15f, num);
			this.CardLabelAt(rectTransform, t.tag, 16f, FontStyles.Bold, CielXApp.TextMain, -31f, 20f, num);
			this.CardLabelAt(rectTransform, t.posts, 12f, FontStyles.Normal, CielXApp.TextDim, -52f, 15f, num);
		}

		// Token: 0x0600A01B RID: 40987 RVA: 0x00421FE0 File Offset: 0x004201E0
		private void CardLabelAt(RectTransform parent, string text, float size, FontStyles style, Color color, float y, float h, float leftInset)
		{
			TMP_Text tmp_Text = this.MakeLabel(parent, text, size, style, color, TextAlignmentOptions.Left, false);
			LocalizedGameText.BindSource(tmp_Text, "phone_story", text, null).FitTranslationToRectangle(false);
			RectTransform rectTransform = (RectTransform)tmp_Text.transform;
			rectTransform.anchorMin = new Vector2(0f, 1f);
			rectTransform.anchorMax = new Vector2(1f, 1f);
			rectTransform.pivot = new Vector2(0.5f, 1f);
			rectTransform.offsetMin = new Vector2(leftInset, 0f);
			rectTransform.offsetMax = new Vector2(-8f, 0f);
			rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, h);
			rectTransform.anchoredPosition = new Vector2(0f, y);
		}

		// Token: 0x0600A01C RID: 40988 RVA: 0x004220AC File Offset: 0x004202AC
		private void RefreshLifetimePanel()
		{
			if (this._statLikes != null)
			{
				this._statLikes.text = CielXFollowerModel.Compact(CielXState.TotalLikes);
			}
			if (this._statReposts != null)
			{
				this._statReposts.text = CielXFollowerModel.Compact(CielXState.TotalReposts);
			}
			if (this._statReplies != null)
			{
				this._statReplies.text = CielXFollowerModel.Compact(CielXState.TotalReplies);
			}
		}

		// Token: 0x0600A01D RID: 40989 RVA: 0x00422124 File Offset: 0x00420324
		private void BuildLeftRail(RectTransform parent)
		{
			RectTransform rectTransform = this.NewChild("LeftRail", parent);
			rectTransform.anchorMin = new Vector2(0f, 0f);
			rectTransform.anchorMax = new Vector2(0f, 1f);
			rectTransform.pivot = new Vector2(0f, 0.5f);
			rectTransform.sizeDelta = new Vector2(268f, 0f);
			rectTransform.anchoredPosition = Vector2.zero;
			rectTransform.gameObject.AddComponent<Image>().color = CielXApp.RailBg;
			RectTransform rectTransform2 = this.NewChild("RailDivider", rectTransform);
			rectTransform2.anchorMin = new Vector2(1f, 0f);
			rectTransform2.anchorMax = new Vector2(1f, 1f);
			rectTransform2.pivot = new Vector2(1f, 0.5f);
			rectTransform2.sizeDelta = new Vector2(1f, 0f);
			rectTransform2.gameObject.AddComponent<Image>().color = CielXApp.Divider;
			RectTransform rectTransform3 = (RectTransform)this.AppLogo(rectTransform, 36f).transform;
			Vector2 vector = new Vector2(0f, 1f);
			rectTransform3.anchorMax = vector;
			rectTransform3.anchorMin = vector;
			rectTransform3.pivot = new Vector2(0f, 1f);
			rectTransform3.anchoredPosition = new Vector2(24f, -20f);
			string[] array = new string[] { "Home", "Explore", "Notifications", "Messages", "Bookmarks", "Profile" };
			CielOSIconId[] array2 = new CielOSIconId[]
			{
				CielOSIconId.Grid,
				CielOSIconId.Search,
				CielOSIconId.Bell,
				CielOSIconId.ReplyBubble,
				CielOSIconId.Files,
				CielOSIconId.Avatar
			};
			float num = -78f;
			for (int i = 0; i < array.Length; i++)
			{
				this.BuildNavItem(rectTransform, array2[i], array[i], num, i == 0);
				num -= 54f;
			}
			RectTransform rectTransform4 = this.MakeButton(rectTransform, "Post", this._accent, Color.white);
			RectTransform rectTransform5 = (RectTransform)rectTransform4.transform;
			vector = new Vector2(0f, 1f);
			rectTransform5.anchorMax = vector;
			rectTransform5.anchorMin = vector;
			rectTransform5.pivot = new Vector2(0f, 1f);
			rectTransform5.sizeDelta = new Vector2(216f, 50f);
			rectTransform5.anchoredPosition = new Vector2(26f, num - 8f);
			rectTransform4.GetComponentInChildren<TMP_Text>().fontSize = 18f;
			rectTransform4.GetComponent<Button>().onClick.AddListener(delegate
			{
				if (this._composerInput != null)
				{
					this._composerInput.Select();
				}
			});
			this.BuildRailProfile(rectTransform);
		}

		// Token: 0x0600A01E RID: 40990 RVA: 0x004223BC File Offset: 0x004205BC
		private void BuildNavItem(RectTransform rail, CielOSIconId icon, string label, float y, bool active)
		{
			RectTransform rectTransform = this.NewChild("Nav_" + label, rail);
			RectTransform rectTransform2 = rectTransform;
			RectTransform rectTransform3 = rectTransform;
			Vector2 vector = new Vector2(0f, 1f);
			rectTransform3.anchorMax = vector;
			rectTransform2.anchorMin = vector;
			rectTransform.pivot = new Vector2(0f, 1f);
			rectTransform.sizeDelta = new Vector2(232f, 46f);
			rectTransform.anchoredPosition = new Vector2(18f, y);
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = new Color(0f, 0f, 0f, 0f);
			CielXApp.Round(image);
			RectTransform rectTransform4 = this.MakeIcon(rectTransform, icon, 24, active ? CielXApp.TextMain : new Color(0.85f, 0.88f, 0.92f, 1f)).rectTransform;
			vector = new Vector2(0f, 0.5f);
			rectTransform4.anchorMax = vector;
			rectTransform4.anchorMin = vector;
			rectTransform4.pivot = new Vector2(0f, 0.5f);
			rectTransform4.anchoredPosition = new Vector2(12f, 0f);
			rectTransform4.sizeDelta = new Vector2(24f, 24f);
			RectTransform rectTransform5 = (RectTransform)this.MakeLocalizedLabel(rectTransform, label, 19f, active ? FontStyles.Bold : FontStyles.Normal, CielXApp.TextMain, TextAlignmentOptions.Left, false).transform;
			rectTransform5.anchorMin = new Vector2(0f, 0f);
			rectTransform5.anchorMax = new Vector2(1f, 1f);
			rectTransform5.offsetMin = new Vector2(50f, 0f);
			rectTransform5.offsetMax = new Vector2(-8f, 0f);
			Button button = rectTransform.gameObject.AddComponent<Button>();
			button.targetGraphic = image;
			ColorBlock colors = button.colors;
			colors.highlightedColor = CielXApp.Hover;
			colors.normalColor = new Color(0f, 0f, 0f, 0f);
			colors.pressedColor = CielXApp.Hover;
			colors.selectedColor = new Color(0f, 0f, 0f, 0f);
			button.colors = colors;
		}

		// Token: 0x0600A01F RID: 40991 RVA: 0x004225EC File Offset: 0x004207EC
		private void BuildRailProfile(RectTransform rail)
		{
			RectTransform rectTransform = this.NewChild("RailProfile", rail);
			rectTransform.anchorMin = new Vector2(0f, 0f);
			rectTransform.anchorMax = new Vector2(1f, 0f);
			rectTransform.pivot = new Vector2(0.5f, 0f);
			rectTransform.sizeDelta = new Vector2(-20f, 64f);
			rectTransform.anchoredPosition = new Vector2(0f, 12f);
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = new Color(0f, 0f, 0f, 0f);
			CielXApp.Round(image);
			this.MakeAvatar(rectTransform, CielXAvatars.ForMe(this._accent, 96), 40f, new Vector2(14f, 0f), new Vector2(0f, 0.5f));
			this._railHandleLabel = this.MakeLabel(rectTransform, CielXState.DisplayHandle, 15f, FontStyles.Bold, CielXApp.TextMain, TextAlignmentOptions.Left, false);
			RectTransform rectTransform2 = (RectTransform)this._railHandleLabel.transform;
			rectTransform2.anchorMin = new Vector2(0f, 0.5f);
			rectTransform2.anchorMax = new Vector2(1f, 1f);
			rectTransform2.offsetMin = new Vector2(64f, 0f);
			rectTransform2.offsetMax = new Vector2(-8f, -6f);
			this._railFollowerLabel = this.MakeLabel(rectTransform, "", 12f, FontStyles.Normal, CielXApp.TextDim, TextAlignmentOptions.Left, false);
			RectTransform rectTransform3 = (RectTransform)this._railFollowerLabel.transform;
			rectTransform3.anchorMin = new Vector2(0f, 0f);
			rectTransform3.anchorMax = new Vector2(1f, 0.5f);
			rectTransform3.offsetMin = new Vector2(64f, 8f);
			rectTransform3.offsetMax = new Vector2(-8f, 0f);
			this.AddClick(rectTransform.gameObject, delegate
			{
				CielXState.LogOut();
				this.ShowLogin();
			});
		}

		// Token: 0x0600A020 RID: 40992 RVA: 0x004227FC File Offset: 0x004209FC
		private void BuildCenterColumn(RectTransform parent)
		{
			RectTransform rectTransform = this.NewChild("Center", parent);
			rectTransform.anchorMin = new Vector2(0f, 0f);
			rectTransform.anchorMax = new Vector2(1f, 1f);
			rectTransform.offsetMin = new Vector2(268f, 0f);
			rectTransform.offsetMax = Vector2.zero;
			RectTransform rectTransform2 = this.NewChild("FeedColumn", rectTransform);
			rectTransform2.anchorMin = new Vector2(0f, 0f);
			rectTransform2.anchorMax = new Vector2(0f, 1f);
			rectTransform2.pivot = new Vector2(0f, 0.5f);
			rectTransform2.sizeDelta = new Vector2(920f, 0f);
			rectTransform2.anchoredPosition = Vector2.zero;
			this.AddSideDivider(rectTransform2, 0.5f);
			RectTransform rectTransform3 = this.NewChild("Header", rectTransform2);
			rectTransform3.anchorMin = new Vector2(0f, 1f);
			rectTransform3.anchorMax = new Vector2(1f, 1f);
			rectTransform3.pivot = new Vector2(0.5f, 1f);
			rectTransform3.sizeDelta = new Vector2(0f, 92f);
			rectTransform3.anchoredPosition = Vector2.zero;
			rectTransform3.gameObject.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.85f);
			RectTransform rectTransform4 = (RectTransform)this.MakeLocalizedLabel(rectTransform3, "Home", 20f, FontStyles.Bold, CielXApp.TextMain, TextAlignmentOptions.Left, false).transform;
			rectTransform4.anchorMin = new Vector2(0f, 1f);
			rectTransform4.anchorMax = new Vector2(1f, 1f);
			rectTransform4.pivot = new Vector2(0.5f, 1f);
			rectTransform4.sizeDelta = new Vector2(-32f, 40f);
			rectTransform4.anchoredPosition = new Vector2(16f, -10f);
			this.BuildTab(rectTransform3, "For you", 0.25f, true);
			this.BuildTab(rectTransform3, "Following", 0.75f, false);
			RectTransform rectTransform5 = this.NewChild("HeaderDiv", rectTransform3);
			rectTransform5.anchorMin = new Vector2(0f, 0f);
			rectTransform5.anchorMax = new Vector2(1f, 0f);
			rectTransform5.pivot = new Vector2(0.5f, 0f);
			rectTransform5.sizeDelta = new Vector2(0f, 1f);
			rectTransform5.gameObject.AddComponent<Image>().color = CielXApp.Divider;
			RectTransform rectTransform6 = this.NewChild("FeedScroll", rectTransform2);
			rectTransform6.anchorMin = Vector2.zero;
			rectTransform6.anchorMax = Vector2.one;
			rectTransform6.offsetMin = Vector2.zero;
			rectTransform6.offsetMax = new Vector2(0f, -92f);
			this._scroll = rectTransform6.gameObject.AddComponent<CielXWheelScroll>();
			this._scroll.horizontal = false;
			this._scroll.vertical = true;
			this._scroll.movementType = ScrollRect.MovementType.Clamped;
			this._scroll.inertia = false;
			this._scroll.notchFraction = 0.04f;
			this._scroll.smoothing = 8f;
			rectTransform6.gameObject.AddComponent<RectMask2D>();
			RectTransform rectTransform7 = this.NewChild("Viewport", rectTransform6);
			CielXApp.Stretch(rectTransform7);
			this._scroll.viewport = rectTransform7;
			this._feedContent = this.NewChild("Content", rectTransform7);
			this._feedContent.anchorMin = new Vector2(0f, 1f);
			this._feedContent.anchorMax = new Vector2(1f, 1f);
			this._feedContent.pivot = new Vector2(0.5f, 1f);
			this._feedContent.sizeDelta = Vector2.zero;
			VerticalLayoutGroup verticalLayoutGroup = this._feedContent.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.spacing = 0f;
			verticalLayoutGroup.childControlWidth = true;
			verticalLayoutGroup.childForceExpandWidth = true;
			verticalLayoutGroup.childControlHeight = true;
			verticalLayoutGroup.childForceExpandHeight = false;
			this._feedContent.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			this._scroll.content = this._feedContent;
		}

		// Token: 0x0600A021 RID: 40993 RVA: 0x00422C34 File Offset: 0x00420E34
		private void AddSideDivider(RectTransform col, float side)
		{
			RectTransform rectTransform = this.NewChild("SideDiv", col);
			rectTransform.anchorMin = new Vector2(0.5f + side, 0f);
			rectTransform.anchorMax = new Vector2(0.5f + side, 1f);
			rectTransform.pivot = new Vector2(0.5f, 0.5f);
			rectTransform.sizeDelta = new Vector2(1f, 0f);
			rectTransform.anchoredPosition = Vector2.zero;
			rectTransform.gameObject.AddComponent<Image>().color = CielXApp.Divider;
		}

		// Token: 0x0600A022 RID: 40994 RVA: 0x00422CC4 File Offset: 0x00420EC4
		private void BuildTab(RectTransform header, string label, float xFrac, bool active)
		{
			RectTransform rectTransform = this.NewChild("Tab_" + label, header);
			rectTransform.anchorMin = new Vector2(xFrac, 0f);
			rectTransform.anchorMax = new Vector2(xFrac, 0f);
			rectTransform.pivot = new Vector2(0.5f, 0f);
			rectTransform.sizeDelta = new Vector2(160f, 44f);
			rectTransform.anchoredPosition = new Vector2(0f, 1f);
			CielXApp.Stretch((RectTransform)this.MakeLocalizedLabel(rectTransform, label, 16f, active ? FontStyles.Bold : FontStyles.Normal, active ? CielXApp.TextMain : CielXApp.TextDim, TextAlignmentOptions.Center, false).transform);
			if (active)
			{
				RectTransform rectTransform2 = this.NewChild("Underline", rectTransform);
				rectTransform2.anchorMin = new Vector2(0.5f, 0f);
				rectTransform2.anchorMax = new Vector2(0.5f, 0f);
				rectTransform2.pivot = new Vector2(0.5f, 0f);
				rectTransform2.sizeDelta = new Vector2(64f, 4f);
				rectTransform2.anchoredPosition = new Vector2(0f, 0f);
				Image image = rectTransform2.gameObject.AddComponent<Image>();
				image.color = this._accent;
				CielXApp.Round(image);
			}
		}

		// Token: 0x0600A023 RID: 40995 RVA: 0x00422E16 File Offset: 0x00421016
		private void RefreshFollowerLabel()
		{
			if (this._railFollowerLabel != null)
			{
				LocalizedGameText.BindFormat(this._railFollowerLabel, "cielx_ui", "{0} followers", new object[] { CielXFollowerModel.Compact(CielXState.Followers) });
			}
		}

		// Token: 0x0600A024 RID: 40996 RVA: 0x00422E50 File Offset: 0x00421050
		private void RebuildFeed()
		{
			if (this._feedContent == null)
			{
				return;
			}
			this._liveWidgets.Clear();
			for (int i = this._feedContent.childCount - 1; i >= 0; i--)
			{
				CielXApp.DestroyUi(this._feedContent.GetChild(i).gameObject);
			}
			this.BuildComposer(this._feedContent);
			this.Divider1(this._feedContent);
			this._feed.Clear();
			List<CielXPost> list = CielXState.LoadMyPosts();
			foreach (CielXPost cielXPost in list)
			{
				this.ApplyTimeGrowth(cielXPost);
			}
			this._feed.AddRange(list);
			this._feed.AddRange(CielXSeed.BuildFeed());
			foreach (CielXPost cielXPost2 in this._feed)
			{
				this.BuildPostRow(this._feedContent, cielXPost2);
			}
			if (Application.isPlaying && base.isActiveAndEnabled)
			{
				base.StartCoroutine(this.SnapToTopNextFrame());
			}
		}

		// Token: 0x0600A025 RID: 40997 RVA: 0x00422F94 File Offset: 0x00421194
		private IEnumerator SnapToTopNextFrame()
		{
			yield return null;
			if (this._feedContent != null)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(this._feedContent);
			}
			if (this._scroll != null)
			{
				this._scroll.SnapTop();
			}
			yield break;
		}

		// Token: 0x0600A026 RID: 40998 RVA: 0x00422FA4 File Offset: 0x004211A4
		private void BuildComposer(RectTransform parent)
		{
			this.RefreshPendingPhoto();
			this._showFaceToggle = null;
			this._photoPrivacyHint = null;
			RectTransform rectTransform = this.NewChild("Composer", parent);
			this._composerLayout = rectTransform.gameObject.AddComponent<LayoutElement>();
			bool flag = !string.IsNullOrEmpty(this._pendingPhotoName) && this._pendingPhoto != null;
			float num = 128f;
			this._composerLayout.minHeight = num;
			this._composerLayout.preferredHeight = num;
			rectTransform.gameObject.AddComponent<Image>().color = CielXApp.Bg;
			this.MakeAvatar(rectTransform, CielXAvatars.ForMe(this._accent, 96), 44f, new Vector2(14f, -14f), new Vector2(0f, 1f));
			RectTransform rectTransform2 = this.NewChild("ComposerInput", rectTransform);
			rectTransform2.anchorMin = new Vector2(0f, 1f);
			rectTransform2.anchorMax = new Vector2(1f, 1f);
			rectTransform2.pivot = new Vector2(0.5f, 1f);
			rectTransform2.offsetMin = new Vector2(70f, -62f);
			rectTransform2.offsetMax = new Vector2(-16f, -12f);
			this._composerInput = this.BuildMultilineInput(rectTransform2, "What's happening?");
			if (!string.IsNullOrEmpty(this._composerDraft))
			{
				this._composerInput.text = this._composerDraft;
			}
			this._composerInput.onValueChanged.AddListener(delegate(string v)
			{
				this._composerDraft = v;
			});
			if (flag && this._pendingPhoto != null && this._pendingPhoto.texture != null)
			{
				Texture2D texture = this._pendingPhoto.texture;
				float num2 = ((texture.height > 0) ? ((float)texture.width / (float)texture.height) : 1.5f);
				float num3 = Mathf.Min(420f, 240f * num2);
				float num4 = num3 / num2;
				this._composerLayout.minHeight = (this._composerLayout.preferredHeight = 128f + num4 + 30f);
				this._composerPreviewWrap = this.NewChild("PreviewWrap", rectTransform);
				this._composerPreviewWrap.anchorMin = new Vector2(0f, 1f);
				this._composerPreviewWrap.anchorMax = new Vector2(0f, 1f);
				this._composerPreviewWrap.pivot = new Vector2(0f, 1f);
				this._composerPreviewWrap.sizeDelta = new Vector2(num3, num4);
				this._composerPreviewWrap.anchoredPosition = new Vector2(70f, -66f);
				Image image = this._composerPreviewWrap.gameObject.AddComponent<Image>();
				image.color = new Color(0.02f, 0.02f, 0.03f, 1f);
				CielXApp.RoundOpaque(image);
				image.raycastTarget = false;
				RectTransform rectTransform3 = this.NewChild("Img", this._composerPreviewWrap);
				RectTransform rectTransform4 = rectTransform3;
				RectTransform rectTransform5 = rectTransform3;
				Vector2 vector = new Vector2(0.5f, 0.5f);
				rectTransform5.anchorMax = vector;
				rectTransform4.anchorMin = vector;
				rectTransform3.sizeDelta = new Vector2(num3, num4);
				rectTransform3.anchoredPosition = Vector2.zero;
				this._composerPreview = rectTransform3.gameObject.AddComponent<RawImage>();
				this._composerPreview.texture = texture;
				StreamerCensorImage.Bind(this._composerPreview, false);
				this._composerPreview.raycastTarget = false;
				RectTransform rectTransform6 = this.NewChild("Remove", this._composerPreviewWrap);
				RectTransform rectTransform7 = rectTransform6;
				RectTransform rectTransform8 = rectTransform6;
				vector = new Vector2(1f, 1f);
				rectTransform8.anchorMax = vector;
				rectTransform7.anchorMin = vector;
				rectTransform6.pivot = new Vector2(1f, 1f);
				rectTransform6.sizeDelta = new Vector2(24f, 24f);
				rectTransform6.anchoredPosition = new Vector2(-5f, -5f);
				Image image2 = rectTransform6.gameObject.AddComponent<Image>();
				image2.color = new Color(0f, 0f, 0f, 0.7f);
				CielXApp.Circle(image2);
				Image image3 = this.MakeIcon(rectTransform6, CielOSIconId.Close, 14, Color.white);
				image3.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
				image3.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
				image3.rectTransform.anchoredPosition = Vector2.zero;
				image3.rectTransform.sizeDelta = new Vector2(14f, 14f);
				this.AddClick(rectTransform6.gameObject, delegate
				{
					this.ClearPendingPhoto();
					this.RebuildComposerOnly();
				});
				this.BuildPhotoPrivacyControl(rectTransform, 66f + num4 + 6f);
			}
			RectTransform rectTransform9 = this.MakeButton(rectTransform, "", new Color(0f, 0f, 0f, 0f), this._accent);
			RectTransform rectTransform10 = (RectTransform)rectTransform9.transform;
			rectTransform10.anchorMin = new Vector2(0f, 0f);
			rectTransform10.anchorMax = new Vector2(0f, 0f);
			rectTransform10.pivot = new Vector2(0f, 0f);
			rectTransform10.sizeDelta = new Vector2(40f, 40f);
			rectTransform10.anchoredPosition = new Vector2(66f, 12f);
			Image image4 = this.MakeIcon(rectTransform9, CielOSIconId.Files, 22, this._accent);
			image4.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
			image4.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
			image4.rectTransform.anchoredPosition = Vector2.zero;
			image4.rectTransform.sizeDelta = new Vector2(22f, 22f);
			rectTransform9.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OpenPhotoPicker));
			RectTransform rectTransform11 = this.MakeButton(rectTransform, "Post", this._accent, Color.white);
			RectTransform rectTransform12 = (RectTransform)rectTransform11.transform;
			rectTransform12.anchorMin = new Vector2(1f, 0f);
			rectTransform12.anchorMax = new Vector2(1f, 0f);
			rectTransform12.pivot = new Vector2(1f, 0f);
			rectTransform12.sizeDelta = new Vector2(110f, 40f);
			rectTransform12.anchoredPosition = new Vector2(-16f, 12f);
			rectTransform11.GetComponent<Button>().onClick.AddListener(new UnityAction(this.PublishPost));
		}

		// Token: 0x0600A027 RID: 40999 RVA: 0x00423630 File Offset: 0x00421830
		private void RebuildComposerOnly()
		{
			this.RebuildFeed();
		}

		// Token: 0x0600A028 RID: 41000 RVA: 0x00423638 File Offset: 0x00421838
		private void BuildPostRow(RectTransform parent, CielXPost post)
		{
			RectTransform rectTransform = this.NewChild("Post", parent);
			VerticalLayoutGroup verticalLayoutGroup = rectTransform.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.padding = new RectOffset(64, 16, 12, 8);
			verticalLayoutGroup.spacing = 6f;
			verticalLayoutGroup.childControlWidth = true;
			verticalLayoutGroup.childForceExpandWidth = true;
			verticalLayoutGroup.childControlHeight = true;
			verticalLayoutGroup.childForceExpandHeight = false;
			rectTransform.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = CielXApp.Bg;
			if (post.byMe && !string.IsNullOrEmpty(post.id))
			{
				CielXRightClick cielXRightClick = rectTransform.gameObject.AddComponent<CielXRightClick>();
				CielXPost capturedPost = post;
				cielXRightClick.onRightClick = delegate(Vector2 screenPos)
				{
					this.ShowDeletePopup(capturedPost, screenPos);
				};
			}
			Sprite sprite = (post.byMe ? CielXAvatars.ForMe(this._accent, 96) : CielXAvatars.ForHandle(post.authorHandle, 96));
			this.MakeAvatar(rectTransform, sprite, 40f, new Vector2(14f, -12f), new Vector2(0f, 1f));
			string aName = (post.byMe ? CielXState.Me.displayName : post.authorName);
			string aHandle = (post.byMe ? CielXState.DisplayHandle : post.authorHandle);
			string verified = (post.authorVerified ? ("  <color=#" + CielXApp.ToHex(this._accent) + ">[v]</color>") : "");
			string text = CielXApp.RelativeAge(post.ageMinutes);
			TMP_Text tmp_Text = this.MakeLabel(rectTransform, string.Concat(new string[]
			{
				"<b>",
				aName,
				"</b>",
				verified,
				"  <color=#",
				CielXApp.ToHex(CielXApp.TextDim),
				">",
				aHandle,
				" · ",
				text,
				"</color>"
			}), 15f, FontStyles.Normal, CielXApp.TextMain, TextAlignmentOptions.Left, false);
			tmp_Text.richText = true;
			tmp_Text.enableWordWrapping = false;
			LocalizedGameText.BindComputed(tmp_Text, () => string.Concat(new string[]
			{
				"<b>",
				aName,
				"</b>",
				verified,
				"  <color=#",
				CielXApp.ToHex(CielXApp.TextDim),
				">",
				aHandle,
				" · ",
				CielXApp.RelativeAge(post.ageMinutes),
				"</color>"
			}));
			if (!string.IsNullOrEmpty(post.text))
			{
				TMP_Text tmp_Text2 = this.MakeLabel(rectTransform, post.text, 16f, FontStyles.Normal, CielXApp.TextMain, TextAlignmentOptions.Left, false);
				tmp_Text2.enableWordWrapping = true;
				if (!post.byMe)
				{
					LocalizedGameText.BindSource(tmp_Text2, "phone_story", post.text, null);
				}
			}
			if (post.HasPhoto)
			{
				Sprite sprite2 = this.ResolvePostPhoto(post);
				if (sprite2 != null && sprite2.texture != null)
				{
					RectTransform rectTransform2 = this.NewChild("Photo", rectTransform);
					LayoutElement layoutElement = rectTransform2.gameObject.AddComponent<LayoutElement>();
					Rect rect = sprite2.rect;
					float num = ((rect.height > 0f) ? (rect.width / rect.height) : 1f);
					float num2 = Mathf.Clamp(824f / Mathf.Max(0.0001f, num), 200f, 520f);
					layoutElement.minHeight = num2;
					layoutElement.preferredHeight = num2;
					rectTransform2.gameObject.AddComponent<RectMask2D>();
					RawImage rawImage = rectTransform2.gameObject.AddComponent<RawImage>();
					rawImage.texture = sprite2.texture;
					StreamerCensorImage.Bind(rawImage, false);
					rawImage.color = Color.white;
				}
			}
			this.BuildActionBar(rectTransform, post);
			Button button = rectTransform.gameObject.AddComponent<Button>();
			button.targetGraphic = image;
			button.transition = Selectable.Transition.None;
			CielXPost capturedForOpen = post;
			button.onClick.AddListener(delegate
			{
				this.OpenPostDetail(capturedForOpen);
			});
			this.Divider1(parent);
		}

		// Token: 0x0600A029 RID: 41001 RVA: 0x00423A60 File Offset: 0x00421C60
		private void BuildActionBar(RectTransform parent, CielXPost post)
		{
			RectTransform rectTransform = this.NewChild("Actions", parent);
			LayoutElement layoutElement = rectTransform.gameObject.AddComponent<LayoutElement>();
			layoutElement.minHeight = 32f;
			layoutElement.preferredHeight = 32f;
			HorizontalLayoutGroup horizontalLayoutGroup = rectTransform.gameObject.AddComponent<HorizontalLayoutGroup>();
			horizontalLayoutGroup.spacing = 70f;
			horizontalLayoutGroup.childControlWidth = false;
			horizontalLayoutGroup.childForceExpandWidth = false;
			horizontalLayoutGroup.childControlHeight = true;
			horizontalLayoutGroup.childForceExpandHeight = true;
			horizontalLayoutGroup.childAlignment = TextAnchor.MiddleLeft;
			CielXApp.PostWidgets postWidgets = new CielXApp.PostWidgets
			{
				post = post
			};
			Image image;
			TMP_Text tmp_Text;
			this.MakeAction(rectTransform, CielOSIconId.ReplyBubble, post.replyCount, CielXApp.TextDim, null, out image, out tmp_Text);
			this.MakeAction(rectTransform, CielOSIconId.RepostArrows, post.reposts, post.repostedByMe ? CielXApp.RepostColor : CielXApp.TextDim, delegate(Image icon, TMP_Text label)
			{
				post.repostedByMe = !post.repostedByMe;
				post.reposts += (post.repostedByMe ? 1 : (-1));
				Color color = (post.repostedByMe ? CielXApp.RepostColor : CielXApp.TextDim);
				icon.color = color;
				label.color = color;
				label.text = CielXFollowerModel.Compact(post.reposts);
				this.PersistIfMine(post);
			}, out postWidgets.repostIcon, out postWidgets.repost);
			this.MakeAction(rectTransform, CielOSIconId.Heart, post.likes, post.likedByMe ? CielXApp.LikeColor : CielXApp.TextDim, delegate(Image icon, TMP_Text label)
			{
				post.likedByMe = !post.likedByMe;
				post.likes += (post.likedByMe ? 1 : (-1));
				Color color2 = (post.likedByMe ? CielXApp.LikeColor : CielXApp.TextDim);
				icon.color = color2;
				label.color = color2;
				label.text = CielXFollowerModel.Compact(post.likes);
				this.PersistIfMine(post);
			}, out postWidgets.likeIcon, out postWidgets.like);
			this._liveWidgets.Add(postWidgets);
		}

		// Token: 0x0600A02A RID: 41002 RVA: 0x00423BAC File Offset: 0x00421DAC
		private void MakeAction(Transform parent, CielOSIconId id, int count, Color color, Action<Image, TMP_Text> onClick, out Image iconOut, out TMP_Text labelOut)
		{
			RectTransform rectTransform = this.NewChild("Action", parent);
			LayoutElement layoutElement = rectTransform.gameObject.AddComponent<LayoutElement>();
			layoutElement.minWidth = 90f;
			layoutElement.preferredWidth = 90f;
			HorizontalLayoutGroup horizontalLayoutGroup = rectTransform.gameObject.AddComponent<HorizontalLayoutGroup>();
			horizontalLayoutGroup.spacing = 8f;
			horizontalLayoutGroup.childAlignment = TextAnchor.MiddleLeft;
			horizontalLayoutGroup.childControlWidth = false;
			horizontalLayoutGroup.childForceExpandWidth = false;
			horizontalLayoutGroup.childControlHeight = false;
			horizontalLayoutGroup.childForceExpandHeight = false;
			RectTransform rectTransform2 = this.NewChild("Icon", rectTransform);
			rectTransform2.sizeDelta = new Vector2(19f, 19f);
			Image icon = rectTransform2.gameObject.AddComponent<Image>();
			icon.sprite = CielOSIconLibrary.GetSprite(id, 19);
			icon.color = color;
			icon.preserveAspect = true;
			icon.raycastTarget = false;
			TMP_Text label = this.MakeLabel(rectTransform, CielXFollowerModel.Compact(count), 14f, FontStyles.Normal, color, TextAlignmentOptions.Left, false);
			label.raycastTarget = false;
			LayoutElement layoutElement2 = label.gameObject.AddComponent<LayoutElement>();
			layoutElement2.minWidth = 48f;
			layoutElement2.preferredWidth = 48f;
			if (onClick != null)
			{
				this.AddClick(rectTransform.gameObject, delegate
				{
					onClick(icon, label);
				});
			}
			iconOut = icon;
			labelOut = label;
		}

		// Token: 0x0600A02B RID: 41003 RVA: 0x00423D1C File Offset: 0x00421F1C
		private void PersistIfMine(CielXPost post)
		{
			if (!post.byMe)
			{
				return;
			}
			List<CielXPost> list = new List<CielXPost>();
			foreach (CielXPost cielXPost in this._feed)
			{
				if (cielXPost.byMe)
				{
					list.Add(cielXPost);
				}
			}
			CielXState.SaveMyPosts(list);
		}

		// Token: 0x0600A02C RID: 41004 RVA: 0x00423D8C File Offset: 0x00421F8C
		private IEnumerator LiveTicker()
		{
			yield return new WaitForSecondsRealtime(2f);
			for (;;)
			{
				foreach (CielXApp.PostWidgets postWidgets in this._liveWidgets)
				{
					if (postWidgets != null && postWidgets.post != null && postWidgets.post.byMe)
					{
						int likes = postWidgets.post.likes;
						int reposts = postWidgets.post.reposts;
						this.ApplyTimeGrowth(postWidgets.post);
						if (postWidgets.post.likes != likes && postWidgets.like != null)
						{
							postWidgets.like.text = CielXFollowerModel.Compact(postWidgets.post.likes);
						}
						if (postWidgets.post.reposts != reposts && postWidgets.repost != null)
						{
							postWidgets.repost.text = CielXFollowerModel.Compact(postWidgets.post.reposts);
						}
						int num = postWidgets.post.likes - likes;
						int num2 = postWidgets.post.reposts - reposts;
						if (num > 0 || num2 > 0)
						{
							CielXState.AddLifetime(Mathf.Max(0, num), Mathf.Max(0, num2), 0);
						}
					}
				}
				if (this._liveWidgets.Count > 0)
				{
					CielXApp.PostWidgets postWidgets2 = this._liveWidgets[this.NextRange(0, this._liveWidgets.Count - 1)];
					if (postWidgets2 != null && postWidgets2.post != null && !postWidgets2.post.byMe && this.NextRange(0, 1) == 0)
					{
						postWidgets2.post.likes++;
						if (postWidgets2.like != null)
						{
							postWidgets2.like.text = CielXFollowerModel.Compact(postWidgets2.post.likes);
						}
					}
				}
				if (this.NextRange(0, 3) == 0)
				{
					int num3 = CielXState.AddFollowers(1);
					if (this._railFollowerLabel != null)
					{
						LocalizedGameText.BindFormat(this._railFollowerLabel, "cielx_ui", "{0} followers", new object[] { CielXFollowerModel.Compact(num3) });
					}
				}
				this.RefreshLifetimePanel();
				yield return new WaitForSecondsRealtime(2f);
			}
			yield break;
		}

		// Token: 0x0600A02D RID: 41005 RVA: 0x00423D9C File Offset: 0x00421F9C
		private void ApplyTimeGrowth(CielXPost post)
		{
			if (post == null || !post.byMe || post.createdAtTicks <= 0L)
			{
				return;
			}
			double num = (double)(DateTime.UtcNow.Ticks - post.createdAtTicks) / 600000000.0;
			int num2 = CielXFollowerModel.LikesOverTime(post.baseLikes, post.likeRatePerMinute, num);
			if (post.HasPhoto && !OxGramPhotoTag.ShowsCiel(post.photoName))
			{
				num2 = Mathf.Min(num2, 5);
			}
			int num3 = CielXFollowerModel.RepostsOverTime(post.baseReposts, num2, post.baseLikes);
			int num4 = ((post.HasPhoto && !OxGramPhotoTag.ShowsCiel(post.photoName)) ? 0 : CielXFollowerModel.RepliesOverTime(post.baseReplies, num2, post.baseLikes));
			post.likes = num2 + (post.likedByMe ? 1 : 0);
			post.reposts = num3 + (post.repostedByMe ? 1 : 0);
			post.replyCount = num4;
		}

		// Token: 0x0600A02E RID: 41006 RVA: 0x00423E80 File Offset: 0x00422080
		private void PublishPost()
		{
			this.RefreshPendingPhoto();
			string text = ((this._composerInput != null) ? this._composerInput.text.Trim() : "");
			if (string.IsNullOrEmpty(text) && string.IsNullOrEmpty(this._pendingPhotoName))
			{
				return;
			}
			CielXAccount me = CielXState.Me;
			CielXPost cielXPost = new CielXPost
			{
				id = Guid.NewGuid().ToString("N"),
				authorHandle = me.handle,
				authorName = me.displayName,
				authorVerified = false,
				byMe = true,
				text = text,
				photoName = this._pendingPhotoName,
				ageMinutes = 0L,
				faceVisible = this.ShouldShowPendingFace
			};
			int followers = CielXState.Followers;
			CielXFollowerModel.Reward reward = CielXFollowerModel.RewardFor(this._pendingPhotoName, followers);
			cielXPost.likes = reward.likes;
			cielXPost.reposts = reward.reposts;
			cielXPost.replyCount = reward.replies;
			cielXPost.createdAtTicks = DateTime.UtcNow.Ticks;
			cielXPost.likeRatePerMinute = reward.likeRatePerMinute;
			cielXPost.baseLikes = reward.likes;
			cielXPost.baseReposts = reward.reposts;
			cielXPost.baseReplies = reward.replies;
			if (this._pendingPhoto != null)
			{
				this.PhotoArchive.SavePublished(cielXPost, this._pendingPhoto);
			}
			CielXState.PrependMyPost(cielXPost);
			CielXState.AddFollowers(reward.followers);
			CielXState.AddLifetime(reward.likes, reward.reposts, cielXPost.replyCount);
			if (this._pendingPhoto != null && !string.IsNullOrEmpty(this._pendingPhotoName) && !OxGramGallery.IsPrivacyCover(this._pendingPhoto))
			{
				this._sessionPhotos[CielXApp.PostPhotoCacheKey(cielXPost)] = this._pendingPhoto;
			}
			this._composerDraft = "";
			if (this._composerInput != null)
			{
				this._composerInput.text = "";
			}
			this.ClearPendingPhoto();
			this.RebuildFeed();
			if (Application.isPlaying && base.isActiveAndEnabled)
			{
				base.StartCoroutine(this.AnimateFollowerGain(followers, followers + reward.followers));
			}
		}

		// Token: 0x0600A02F RID: 41007 RVA: 0x004240AA File Offset: 0x004222AA
		private IEnumerator AnimateFollowerGain(int from, int to)
		{
			float t = 0f;
			float dur = 1f;
			while (t < dur)
			{
				t += Time.unscaledDeltaTime;
				int num = Mathf.RoundToInt(Mathf.Lerp((float)from, (float)to, t / dur));
				if (this._railFollowerLabel != null)
				{
					LocalizedGameText.BindFormat(this._railFollowerLabel, "cielx_ui", "{0} followers  +{1}", new object[]
					{
						CielXFollowerModel.Compact(num),
						to - from
					});
				}
				yield return null;
			}
			this.RefreshFollowerLabel();
			yield break;
		}

		// Token: 0x0600A030 RID: 41008 RVA: 0x004240C8 File Offset: 0x004222C8
		private void OpenPostDetail(CielXPost post)
		{
			if (post == null)
			{
				return;
			}
			if (this._detail != null)
			{
				CielXApp.DestroyUi(this._detail.gameObject);
			}
			this._detail = this.NewChild("PostDetail", this._self);
			CielXApp.Stretch(this._detail);
			Image image = this._detail.gameObject.AddComponent<Image>();
			image.color = CielXApp.Bg;
			CielXApp.RoundOpaque(image);
			image.raycastTarget = true;
			RectTransform rectTransform = this.NewChild("Header", this._detail);
			rectTransform.anchorMin = new Vector2(0f, 1f);
			rectTransform.anchorMax = new Vector2(1f, 1f);
			rectTransform.pivot = new Vector2(0.5f, 1f);
			rectTransform.sizeDelta = new Vector2(0f, 54f);
			Image image2 = rectTransform.gameObject.AddComponent<Image>();
			image2.color = new Color(0f, 0f, 0f, 0.92f);
			CielXApp.RoundOpaque(image2);
			RectTransform rectTransform2 = this.NewChild("Back", rectTransform);
			rectTransform2.anchorMin = new Vector2(0f, 0.5f);
			rectTransform2.anchorMax = new Vector2(0f, 0.5f);
			rectTransform2.pivot = new Vector2(0f, 0.5f);
			rectTransform2.sizeDelta = new Vector2(40f, 40f);
			rectTransform2.anchoredPosition = new Vector2(14f, 0f);
			rectTransform2.gameObject.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
			Image image3 = this.MakeIcon(rectTransform2, CielOSIconId.BackArrow, 22, CielXApp.TextMain);
			image3.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
			image3.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
			image3.rectTransform.anchoredPosition = Vector2.zero;
			image3.rectTransform.sizeDelta = new Vector2(22f, 22f);
			this.AddClick(rectTransform2.gameObject, new UnityAction(this.ClosePostDetail));
			ControllerNavigationScope.Configure(this._detail.gameObject, new Action(this.ClosePostDetail), rectTransform2.gameObject, 80, false);
			RectTransform rectTransform3 = (RectTransform)this.MakeLocalizedLabel(rectTransform, "Post", 19f, FontStyles.Bold, CielXApp.TextMain, TextAlignmentOptions.Left, false).transform;
			rectTransform3.anchorMin = new Vector2(0f, 0f);
			rectTransform3.anchorMax = new Vector2(1f, 1f);
			rectTransform3.offsetMin = new Vector2(64f, 0f);
			rectTransform3.offsetMax = new Vector2(-16f, 0f);
			RectTransform rectTransform4 = this.NewChild("Scroll", this._detail);
			rectTransform4.anchorMin = Vector2.zero;
			rectTransform4.anchorMax = Vector2.one;
			rectTransform4.offsetMin = Vector2.zero;
			rectTransform4.offsetMax = new Vector2(0f, -54f);
			CielXWheelScroll cielXWheelScroll = rectTransform4.gameObject.AddComponent<CielXWheelScroll>();
			cielXWheelScroll.horizontal = false;
			cielXWheelScroll.vertical = true;
			cielXWheelScroll.movementType = ScrollRect.MovementType.Clamped;
			cielXWheelScroll.inertia = false;
			cielXWheelScroll.notchFraction = 0.04f;
			cielXWheelScroll.smoothing = 8f;
			rectTransform4.gameObject.AddComponent<RectMask2D>();
			if (this._scroll != null)
			{
				this._scroll.enabled = false;
			}
			RectTransform rectTransform5 = this.NewChild("VP", rectTransform4);
			CielXApp.Stretch(rectTransform5);
			cielXWheelScroll.viewport = rectTransform5;
			float num = Mathf.Min(920f, 760f);
			RectTransform rectTransform6 = this.NewChild("Content", rectTransform5);
			rectTransform6.anchorMin = new Vector2(0.5f, 1f);
			rectTransform6.anchorMax = new Vector2(0.5f, 1f);
			rectTransform6.pivot = new Vector2(0.5f, 1f);
			rectTransform6.sizeDelta = new Vector2(num, 0f);
			VerticalLayoutGroup verticalLayoutGroup = rectTransform6.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.spacing = 0f;
			verticalLayoutGroup.childControlWidth = true;
			verticalLayoutGroup.childForceExpandWidth = true;
			verticalLayoutGroup.childControlHeight = true;
			verticalLayoutGroup.childForceExpandHeight = false;
			rectTransform6.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			cielXWheelScroll.content = rectTransform6;
			this.BuildDetailPost(rectTransform6, post, num);
			this.BuildCommentList(rectTransform6, post, num);
			if (base.isActiveAndEnabled)
			{
				base.StartCoroutine(this.SnapDetailTop(cielXWheelScroll, rectTransform6));
			}
		}

		// Token: 0x0600A031 RID: 41009 RVA: 0x00424551 File Offset: 0x00422751
		private IEnumerator SnapDetailTop(CielXWheelScroll scroll, RectTransform content)
		{
			yield return null;
			if (content != null)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(content);
			}
			if (scroll != null)
			{
				scroll.SnapTop();
			}
			yield break;
		}

		// Token: 0x0600A032 RID: 41010 RVA: 0x00424568 File Offset: 0x00422768
		private void BuildDetailPost(RectTransform parent, CielXPost post, float colW)
		{
			RectTransform rectTransform = this.NewChild("DetailPost", parent);
			VerticalLayoutGroup verticalLayoutGroup = rectTransform.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.padding = new RectOffset(18, 18, 16, 12);
			verticalLayoutGroup.spacing = 12f;
			verticalLayoutGroup.childControlWidth = true;
			verticalLayoutGroup.childForceExpandWidth = true;
			verticalLayoutGroup.childControlHeight = true;
			verticalLayoutGroup.childForceExpandHeight = false;
			rectTransform.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			string text = (post.byMe ? CielXState.Me.displayName : post.authorName);
			string text2 = (post.byMe ? CielXState.DisplayHandle : post.authorHandle);
			RectTransform rectTransform2 = this.NewChild("Head", rectTransform);
			this.AddHeight(rectTransform2.gameObject, 52f);
			Sprite sprite = (post.byMe ? CielXAvatars.ForMe(this._accent, 96) : CielXAvatars.ForHandle(post.authorHandle, 96));
			this.MakeAvatar(rectTransform2, sprite, 48f, new Vector2(0f, 0f), new Vector2(0f, 0.5f));
			RectTransform rectTransform3 = this.NewChild("Name", rectTransform2);
			rectTransform3.anchorMin = new Vector2(0f, 0f);
			rectTransform3.anchorMax = new Vector2(1f, 1f);
			rectTransform3.offsetMin = new Vector2(60f, 0f);
			rectTransform3.offsetMax = new Vector2(0f, 0f);
			VerticalLayoutGroup verticalLayoutGroup2 = rectTransform3.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup2.childAlignment = TextAnchor.MiddleLeft;
			verticalLayoutGroup2.spacing = 0f;
			verticalLayoutGroup2.childControlWidth = true;
			verticalLayoutGroup2.childForceExpandWidth = true;
			verticalLayoutGroup2.childControlHeight = true;
			verticalLayoutGroup2.childForceExpandHeight = false;
			string text3 = (post.authorVerified ? ("  <color=#" + CielXApp.ToHex(this._accent) + ">[v]</color>") : "");
			TMP_Text tmp_Text = this.MakeLabel(rectTransform3, "<b>" + text + "</b>" + text3, 16f, FontStyles.Normal, CielXApp.TextMain, TextAlignmentOptions.Left, false);
			tmp_Text.richText = true;
			this.AddHeight(tmp_Text.gameObject, 22f);
			TMP_Text tmp_Text2 = this.MakeLabel(rectTransform3, text2, 14f, FontStyles.Normal, CielXApp.TextDim, TextAlignmentOptions.Left, false);
			this.AddHeight(tmp_Text2.gameObject, 20f);
			if (!string.IsNullOrEmpty(post.text))
			{
				TMP_Text tmp_Text3 = this.MakeLabel(rectTransform, post.text, 20f, FontStyles.Normal, CielXApp.TextMain, TextAlignmentOptions.Left, false);
				tmp_Text3.enableWordWrapping = true;
				if (!post.byMe)
				{
					LocalizedGameText.BindSource(tmp_Text3, "phone_story", post.text, null);
				}
			}
			if (post.HasPhoto)
			{
				Sprite sprite2 = this.ResolvePostPhoto(post);
				if (sprite2 != null && sprite2.texture != null)
				{
					RectTransform rectTransform4 = this.NewChild("Photo", rectTransform);
					LayoutElement layoutElement = rectTransform4.gameObject.AddComponent<LayoutElement>();
					Rect rect = sprite2.rect;
					float num = ((rect.height > 0f) ? (rect.width / rect.height) : 1f);
					float num2 = Mathf.Clamp((colW - 36f) / Mathf.Max(0.0001f, num), 240f, 680f);
					layoutElement.minHeight = num2;
					layoutElement.preferredHeight = num2;
					rectTransform4.gameObject.AddComponent<RectMask2D>();
					RawImage rawImage = rectTransform4.gameObject.AddComponent<RawImage>();
					rawImage.texture = sprite2.texture;
					StreamerCensorImage.Bind(rawImage, false);
					rawImage.color = Color.white;
				}
			}
			RectTransform rectTransform5 = this.NewChild("Stats", rectTransform);
			this.AddHeight(rectTransform5.gameObject, 56f);
			rectTransform5.gameObject.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
			this.StatCell(rectTransform5, 0f, CielXFollowerModel.Compact(post.replyCount), "Comments");
			this.StatCell(rectTransform5, 0.33333334f, CielXFollowerModel.Compact(post.reposts), "Reposts");
			this.StatCell(rectTransform5, 0.6666667f, CielXFollowerModel.Compact(post.likes), "Likes");
			this.StatSep(rectTransform5, 0.33333334f);
			this.StatSep(rectTransform5, 0.6666667f);
			this.Divider1(parent);
		}

		// Token: 0x0600A033 RID: 41011 RVA: 0x004249AC File Offset: 0x00422BAC
		private void StatCell(RectTransform parent, float frac, string value, string label)
		{
			RectTransform rectTransform = this.NewChild("Cell", parent);
			rectTransform.anchorMin = new Vector2(frac, 0f);
			rectTransform.anchorMax = new Vector2(frac + 0.33333334f, 1f);
			rectTransform.offsetMin = new Vector2(14f, 0f);
			rectTransform.offsetMax = new Vector2(-14f, 0f);
			RectTransform rectTransform2 = (RectTransform)this.MakeLabel(rectTransform, value, 20f, FontStyles.Bold, CielXApp.TextMain, TextAlignmentOptions.Left, false).transform;
			rectTransform2.anchorMin = new Vector2(0f, 0f);
			rectTransform2.anchorMax = new Vector2(1f, 1f);
			rectTransform2.offsetMin = new Vector2(0f, 22f);
			rectTransform2.offsetMax = Vector2.zero;
			RectTransform rectTransform3 = (RectTransform)this.MakeLocalizedLabel(rectTransform, label, 12f, FontStyles.Normal, CielXApp.TextDim, TextAlignmentOptions.Left, false).transform;
			rectTransform3.anchorMin = new Vector2(0f, 0f);
			rectTransform3.anchorMax = new Vector2(1f, 0f);
			rectTransform3.pivot = new Vector2(0f, 0f);
			rectTransform3.sizeDelta = new Vector2(0f, 18f);
			rectTransform3.anchoredPosition = new Vector2(0f, 6f);
		}

		// Token: 0x0600A034 RID: 41012 RVA: 0x00424B10 File Offset: 0x00422D10
		private void StatSep(RectTransform parent, float frac)
		{
			RectTransform rectTransform = this.NewChild("StatSep", parent);
			rectTransform.anchorMin = new Vector2(frac, 0f);
			rectTransform.anchorMax = new Vector2(frac, 1f);
			rectTransform.pivot = new Vector2(0.5f, 0.5f);
			rectTransform.sizeDelta = new Vector2(1f, -16f);
			rectTransform.anchoredPosition = Vector2.zero;
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = new Color(1f, 1f, 1f, 0.1f);
			CielXApp.RoundOpaque(image);
			image.raycastTarget = false;
		}

		// Token: 0x0600A035 RID: 41013 RVA: 0x00424BB8 File Offset: 0x00422DB8
		private void BuildCommentList(RectTransform parent, CielXPost post, float colW)
		{
			RectTransform rectTransform = this.NewChild("CommentsHdr", parent);
			this.AddHeight(rectTransform.gameObject, 38f);
			RectTransform rectTransform2 = (RectTransform)this.MakeLocalizedLabel(rectTransform, "Comments", 16f, FontStyles.Bold, CielXApp.TextMain, TextAlignmentOptions.Left, false).transform;
			rectTransform2.anchorMin = new Vector2(0f, 0f);
			rectTransform2.anchorMax = new Vector2(1f, 1f);
			rectTransform2.offsetMin = new Vector2(18f, 0f);
			rectTransform2.offsetMax = new Vector2(-18f, 0f);
			int num = Mathf.Max(1, Mathf.RoundToInt((float)post.likes * 0.3f));
			string text = (post.HasPhoto ? OxGramPhotoTag.FromName(post.photoName) : null);
			int num2 = Mathf.Clamp(post.replyCount, 3, 30);
			foreach (CielXReply cielXReply in CielXSeed.RepliesFor(text, num2, new Func<int, int, int>(this.NextRange), num, post.faceVisible))
			{
				this.BuildCommentRow(parent, cielXReply, colW);
			}
		}

		// Token: 0x0600A036 RID: 41014 RVA: 0x00424CF8 File Offset: 0x00422EF8
		private void BuildCommentRow(RectTransform parent, CielXReply c, float colW)
		{
			RectTransform rectTransform = this.NewChild("Comment", parent);
			VerticalLayoutGroup verticalLayoutGroup = rectTransform.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.padding = new RectOffset(64, 18, 10, 10);
			verticalLayoutGroup.spacing = 3f;
			verticalLayoutGroup.childControlWidth = true;
			verticalLayoutGroup.childForceExpandWidth = true;
			verticalLayoutGroup.childControlHeight = true;
			verticalLayoutGroup.childForceExpandHeight = false;
			rectTransform.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = CielXApp.Bg;
			CielXApp.RoundOpaque(image);
			this.MakeAvatar(rectTransform, CielXAvatars.ForHandle(c.handle, 96), 36f, new Vector2(14f, -10f), new Vector2(0f, 1f));
			TMP_Text tmp_Text = this.MakeLabel(rectTransform, string.Concat(new string[]
			{
				"<b>",
				c.displayName,
				"</b>  <color=#",
				CielXApp.ToHex(CielXApp.TextDim),
				">",
				c.handle,
				"</color>"
			}), 14f, FontStyles.Normal, CielXApp.TextMain, TextAlignmentOptions.Left, false);
			tmp_Text.richText = true;
			tmp_Text.enableWordWrapping = false;
			TMP_Text tmp_Text2 = this.MakeLabel(rectTransform, c.text, 15f, FontStyles.Normal, CielXApp.TextMain, TextAlignmentOptions.Left, false);
			tmp_Text2.enableWordWrapping = true;
			LocalizedGameText.BindComputed(tmp_Text2, () => CielXApp.ReplyText(c));
			TMP_Text tmp_Text3 = this.MakeLabel(rectTransform, CielXFollowerModel.Compact(c.likes) + " likes", 12f, FontStyles.Normal, CielXApp.TextDim, TextAlignmentOptions.Left, false);
			LocalizedGameText.BindFormat(tmp_Text3, "cielx_ui", "{0} likes", new object[] { CielXFollowerModel.Compact(c.likes) });
			this.AddHeight(tmp_Text3.gameObject, 16f);
			this.Divider1(parent);
		}

		// Token: 0x0600A037 RID: 41015 RVA: 0x00424EF8 File Offset: 0x004230F8
		private void ClosePostDetail()
		{
			if (this._detail != null)
			{
				CielXApp.DestroyUi(this._detail.gameObject);
				this._detail = null;
			}
			if (this._scroll != null)
			{
				this._scroll.enabled = true;
			}
		}

		// Token: 0x0600A038 RID: 41016 RVA: 0x00424F44 File Offset: 0x00423144
		private void ShowDeletePopup(CielXPost post, Vector2 screenPos)
		{
			if (this._deletePopup != null)
			{
				CielXApp.DestroyUi(this._deletePopup.gameObject);
			}
			RectTransform rectTransform = this.NewChild("DeleteBackdrop", this._self);
			CielXApp.Stretch(rectTransform);
			rectTransform.gameObject.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.01f);
			this.AddClick(rectTransform.gameObject, new UnityAction(this.CloseDeletePopup));
			this._deletePopup = rectTransform;
			RectTransform rectTransform2 = this.NewChild("DeleteCard", rectTransform);
			Image image = rectTransform2.gameObject.AddComponent<Image>();
			image.color = CielXApp.Card;
			CielXApp.RoundOpaque(image);
			rectTransform2.sizeDelta = new Vector2(220f, 96f);
			Camera canvasCamera = this.GetCanvasCamera();
			Vector2 vector;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this._self, screenPos, canvasCamera, out vector);
			RectTransform rectTransform3 = rectTransform2;
			RectTransform rectTransform4 = rectTransform2;
			Vector2 vector2 = new Vector2(0.5f, 0.5f);
			rectTransform4.anchorMax = vector2;
			rectTransform3.anchorMin = vector2;
			rectTransform2.pivot = new Vector2(0f, 1f);
			rectTransform2.anchoredPosition = vector + new Vector2(6f, -6f);
			RectTransform rectTransform5 = (RectTransform)this.MakeLocalizedLabel(rectTransform2, "Delete this post?", 15f, FontStyles.Bold, CielXApp.TextMain, TextAlignmentOptions.Center, false).transform;
			rectTransform5.anchorMin = new Vector2(0f, 1f);
			rectTransform5.anchorMax = new Vector2(1f, 1f);
			rectTransform5.pivot = new Vector2(0.5f, 1f);
			rectTransform5.sizeDelta = new Vector2(-16f, 26f);
			rectTransform5.anchoredPosition = new Vector2(0f, -12f);
			RectTransform rectTransform6 = this.MakeButton(rectTransform2, "Delete", CielXApp.LikeColor, Color.white);
			RectTransform rectTransform7 = (RectTransform)rectTransform6.transform;
			rectTransform7.anchorMin = new Vector2(0f, 0f);
			rectTransform7.anchorMax = new Vector2(0.5f, 0f);
			rectTransform7.pivot = new Vector2(0f, 0f);
			rectTransform7.offsetMin = new Vector2(12f, 12f);
			rectTransform7.offsetMax = new Vector2(-6f, 44f);
			rectTransform6.GetComponent<Button>().onClick.AddListener(delegate
			{
				if (CielXState.DeleteMyPost(post.id))
				{
					this.PhotoArchive.DeletePublished(post);
				}
				this._sessionPhotos.Remove(CielXApp.PostPhotoCacheKey(post));
				this.CloseDeletePopup();
				this.RebuildFeed();
			});
			RectTransform rectTransform8 = this.MakeButton(rectTransform2, "Cancel", new Color(0.2f, 0.22f, 0.26f, 1f), CielXApp.TextMain);
			RectTransform rectTransform9 = (RectTransform)rectTransform8.transform;
			rectTransform9.anchorMin = new Vector2(0.5f, 0f);
			rectTransform9.anchorMax = new Vector2(1f, 0f);
			rectTransform9.pivot = new Vector2(1f, 0f);
			rectTransform9.offsetMin = new Vector2(6f, 12f);
			rectTransform9.offsetMax = new Vector2(-12f, 44f);
			rectTransform8.GetComponent<Button>().onClick.AddListener(new UnityAction(this.CloseDeletePopup));
			ControllerNavigationScope.Configure(rectTransform.gameObject, new Action(this.CloseDeletePopup), rectTransform8.gameObject, 110, false);
		}

		// Token: 0x0600A039 RID: 41017 RVA: 0x0042529B File Offset: 0x0042349B
		private void CloseDeletePopup()
		{
			if (this._deletePopup != null)
			{
				CielXApp.DestroyUi(this._deletePopup.gameObject);
				this._deletePopup = null;
			}
		}

		// Token: 0x0600A03A RID: 41018 RVA: 0x004252C4 File Offset: 0x004234C4
		private Camera GetCanvasCamera()
		{
			Canvas componentInParent = base.GetComponentInParent<Canvas>();
			if (componentInParent == null)
			{
				return null;
			}
			if (componentInParent.renderMode != RenderMode.ScreenSpaceOverlay)
			{
				return componentInParent.worldCamera;
			}
			return null;
		}

		// Token: 0x0600A03B RID: 41019 RVA: 0x004252F3 File Offset: 0x004234F3
		private void OpenPhotoPicker()
		{
			this.BuildPhotoPicker(OxGramGallery.Reload());
		}

		// Token: 0x0600A03C RID: 41020 RVA: 0x00425300 File Offset: 0x00423500
		private void BuildPhotoPicker(IReadOnlyList<Sprite> photos)
		{
			this.ClosePicker();
			this._picker = this.NewChild("PhotoPicker", this._self);
			CielXApp.Stretch(this._picker);
			OxGramGallery.DecodeWhileOpen(this._picker.gameObject);
			this._picker.gameObject.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.7f);
			this.AddClick(this._picker.gameObject, new UnityAction(this.ClosePicker));
			RectTransform rectTransform = this.NewChild("Sheet", this._picker);
			this._pickerSheet = rectTransform;
			RectTransform rectTransform2 = rectTransform;
			RectTransform rectTransform3 = rectTransform;
			Vector2 vector = new Vector2(0.5f, 0.5f);
			rectTransform3.anchorMax = vector;
			rectTransform2.anchorMin = vector;
			rectTransform.pivot = new Vector2(0.5f, 0.5f);
			rectTransform.sizeDelta = new Vector2(560f, 540f);
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = CielXApp.Card;
			CielXApp.RoundOpaque(image);
			image.raycastTarget = true;
			TMP_Text tmp_Text = this.MakeLocalizedLabel(rectTransform, "Pick a photo to post", 17f, FontStyles.Bold, CielXApp.TextMain, TextAlignmentOptions.Center, true);
			tmp_Text.name = "Title";
			RectTransform rectTransform4 = (RectTransform)tmp_Text.transform;
			rectTransform4.anchorMin = new Vector2(0f, 1f);
			rectTransform4.anchorMax = new Vector2(1f, 1f);
			rectTransform4.pivot = new Vector2(0.5f, 1f);
			rectTransform4.sizeDelta = new Vector2(-32f, 30f);
			rectTransform4.anchoredPosition = new Vector2(0f, -14f);
			TMP_Text tmp_Text2 = this.MakeLocalizedLabel(rectTransform, "Your face stays blurred unless you choose to show it at Certified Exhibitionist.", 12f, FontStyles.Italic, CielXApp.TextDim, TextAlignmentOptions.Center, true);
			tmp_Text2.name = "PrivacyNote";
			RectTransform rectTransform5 = (RectTransform)tmp_Text2.transform;
			rectTransform5.anchorMin = new Vector2(0f, 1f);
			rectTransform5.anchorMax = new Vector2(1f, 1f);
			rectTransform5.pivot = new Vector2(0.5f, 1f);
			rectTransform5.sizeDelta = new Vector2(-32f, 48f);
			rectTransform5.anchoredPosition = new Vector2(0f, -46f);
			RectTransform rectTransform6 = this.NewChild("Scroll", rectTransform);
			rectTransform6.anchorMin = Vector2.zero;
			rectTransform6.anchorMax = Vector2.one;
			rectTransform6.offsetMin = new Vector2(12f, 12f);
			rectTransform6.offsetMax = new Vector2(-12f, -102f);
			CielXWheelScroll cielXWheelScroll = rectTransform6.gameObject.AddComponent<CielXWheelScroll>();
			cielXWheelScroll.horizontal = false;
			cielXWheelScroll.vertical = true;
			cielXWheelScroll.movementType = ScrollRect.MovementType.Clamped;
			cielXWheelScroll.inertia = false;
			cielXWheelScroll.notchFraction = 0.04f;
			cielXWheelScroll.smoothing = 8f;
			rectTransform6.gameObject.AddComponent<RectMask2D>();
			RectTransform rectTransform7 = this.NewChild("VP", rectTransform6);
			CielXApp.Stretch(rectTransform7);
			cielXWheelScroll.viewport = rectTransform7;
			RectTransform rectTransform8 = this.NewChild("Content", rectTransform7);
			rectTransform8.anchorMin = new Vector2(0f, 1f);
			rectTransform8.anchorMax = new Vector2(1f, 1f);
			rectTransform8.pivot = new Vector2(0.5f, 1f);
			rectTransform8.sizeDelta = Vector2.zero;
			GridLayoutGroup gridLayoutGroup = rectTransform8.gameObject.AddComponent<GridLayoutGroup>();
			this._pickerGrid = gridLayoutGroup;
			gridLayoutGroup.cellSize = new Vector2(120f, 120f);
			gridLayoutGroup.spacing = new Vector2(10f, 10f);
			gridLayoutGroup.padding = new RectOffset(6, 6, 6, 6);
			gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
			gridLayoutGroup.constraintCount = 4;
			rectTransform8.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			cielXWheelScroll.content = rectTransform8;
			this.RefreshPhotoPickerLayout();
			if (photos.Count == 0)
			{
				TMP_Text tmp_Text3 = this.MakeLocalizedLabel(rectTransform7, "No photos in your gallery yet.\nTake some with the phone camera.", 13f, FontStyles.Normal, CielXApp.TextDim, TextAlignmentOptions.Center, true);
				tmp_Text3.name = "EmptyState";
				RectTransform rectTransform9 = (RectTransform)tmp_Text3.transform;
				CielXApp.Stretch(rectTransform9);
				rectTransform9.offsetMin = new Vector2(16f, 16f);
				rectTransform9.offsetMax = new Vector2(-16f, -16f);
				cielXWheelScroll.vertical = false;
				return;
			}
			foreach (Sprite sprite in photos)
			{
				RectTransform rectTransform10 = this.NewChild("Thumb", rectTransform8);
				rectTransform10.gameObject.AddComponent<RectMask2D>();
				rectTransform10.gameObject.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.25f);
				RectTransform rectTransform11 = this.NewChild("Img", rectTransform10);
				Image image2 = rectTransform11.gameObject.AddComponent<Image>();
				image2.sprite = sprite;
				image2.preserveAspect = true;
				image2.raycastTarget = false;
				StreamerCensorImage.Bind(image2, false);
				Rect rect = sprite.rect;
				float num = ((rect.height > 0f) ? (rect.width / rect.height) : 1f);
				AspectRatioFitter aspectRatioFitter = rectTransform11.gameObject.AddComponent<AspectRatioFitter>();
				aspectRatioFitter.aspectRatio = num;
				aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
				Sprite captured = sprite;
				this.AddClick(rectTransform10.gameObject, delegate
				{
					this.SelectPhoto(captured);
				});
			}
		}

		// Token: 0x0600A03D RID: 41021 RVA: 0x0042588C File Offset: 0x00423A8C
		private void RefreshPhotoPickerLayout()
		{
			if (this._self == null || this._pickerSheet == null)
			{
				return;
			}
			this._pickerSheet.sizeDelta = new Vector2(Mathf.Min(560f, Mathf.Max(1f, this._self.rect.width - 24f)), Mathf.Min(540f, Mathf.Max(1f, this._self.rect.height - 24f)));
			if (this._pickerGrid == null)
			{
				return;
			}
			float num = Mathf.Max(1f, this._pickerSheet.rect.width - 24f - (float)this._pickerGrid.padding.horizontal);
			float num2 = Mathf.Min(120f, num);
			this._pickerGrid.cellSize = new Vector2(num2, num2);
			this._pickerGrid.constraintCount = Mathf.Clamp(Mathf.FloorToInt((num + this._pickerGrid.spacing.x) / (num2 + this._pickerGrid.spacing.x)), 1, 4);
		}

		// Token: 0x0600A03E RID: 41022 RVA: 0x004259BF File Offset: 0x00423BBF
		private void SelectPhoto(Sprite gallerySprite)
		{
			if (gallerySprite == null)
			{
				return;
			}
			this._pendingOriginalPhoto = gallerySprite;
			this._pendingPhotoName = CielXApp.NormalizePhotoName(gallerySprite.name);
			this._showFace = false;
			this.RefreshPendingPhoto();
			this.ClosePicker();
			this.RebuildComposerOnly();
		}

		// Token: 0x170018F3 RID: 6387
		// (get) Token: 0x0600A03F RID: 41023 RVA: 0x004259FC File Offset: 0x00423BFC
		private bool ShouldShowPendingFace
		{
			get
			{
				return this._pendingOriginalPhoto != null && this._showFace && CielXPhotoPrivacy.CanShowFace && OxGramPhotoTag.ShowsFace(this._pendingPhotoName);
			}
		}

		// Token: 0x0600A040 RID: 41024 RVA: 0x00425A28 File Offset: 0x00423C28
		private void RefreshPendingPhoto()
		{
			if (!CielXPhotoPrivacy.CanShowFace || !OxGramPhotoTag.ShowsFace(this._pendingPhotoName))
			{
				this._showFace = false;
			}
			if (this._pendingOriginalPhoto == null)
			{
				return;
			}
			this._pendingPhoto = (this.ShouldShowPendingFace ? OxGramGallery.OriginalOf(this._pendingOriginalPhoto) : OxGramGallery.CensoredSiblingOf(this._pendingOriginalPhoto, true));
		}

		// Token: 0x0600A041 RID: 41025 RVA: 0x00425A88 File Offset: 0x00423C88
		private void BuildPhotoPrivacyControl(RectTransform composer, float top)
		{
			RectTransform rectTransform = this.NewChild("PhotoPrivacy", composer);
			rectTransform.anchorMin = new Vector2(0f, 1f);
			rectTransform.anchorMax = Vector2.one;
			rectTransform.pivot = new Vector2(0f, 1f);
			rectTransform.offsetMin = new Vector2(70f, -top - 24f);
			rectTransform.offsetMax = new Vector2(-16f, -top);
			RectTransform rectTransform2 = this.NewChild("ShowFace", rectTransform);
			RectTransform rectTransform3 = rectTransform2;
			RectTransform rectTransform4 = rectTransform2;
			Vector2 vector = new Vector2(0f, 0.5f);
			rectTransform4.anchorMax = vector;
			rectTransform3.anchorMin = vector;
			rectTransform2.pivot = new Vector2(0f, 0.5f);
			rectTransform2.anchoredPosition = Vector2.zero;
			rectTransform2.sizeDelta = new Vector2(150f, 24f);
			rectTransform2.gameObject.AddComponent<Image>().color = Color.clear;
			RectTransform rectTransform5 = this.NewChild("Checkbox", rectTransform2);
			RectTransform rectTransform6 = rectTransform5;
			RectTransform rectTransform7 = rectTransform5;
			vector = new Vector2(0f, 0.5f);
			rectTransform7.anchorMax = vector;
			rectTransform6.anchorMin = vector;
			rectTransform5.pivot = new Vector2(0f, 0.5f);
			rectTransform5.anchoredPosition = Vector2.zero;
			rectTransform5.sizeDelta = new Vector2(14f, 14f);
			this._photoPrivacyBox = rectTransform5.gameObject.AddComponent<Image>();
			this._photoPrivacyBox.raycastTarget = false;
			RectTransform rectTransform8 = this.NewChild("Inset", rectTransform5);
			CielXApp.Stretch(rectTransform8);
			rectTransform8.offsetMin = Vector2.one;
			rectTransform8.offsetMax = -Vector2.one;
			Image image = rectTransform8.gameObject.AddComponent<Image>();
			image.color = CielXApp.Bg;
			image.raycastTarget = false;
			RectTransform rectTransform9 = this.NewChild("Check", rectTransform5);
			CielXApp.Stretch(rectTransform9);
			OxGramVectorIcon oxGramVectorIcon = rectTransform9.gameObject.AddComponent<OxGramVectorIcon>();
			oxGramVectorIcon.symbol = OxGramVectorIcon.Symbol.Check;
			oxGramVectorIcon.color = CielXApp.TextMain;
			oxGramVectorIcon.raycastTarget = false;
			this._photoPrivacyLabel = this.MakeLocalizedLabel(rectTransform2, "Show my face", 12f, FontStyles.Normal, CielXApp.TextDim, TextAlignmentOptions.Left, false);
			CielXApp.Stretch(this._photoPrivacyLabel.rectTransform);
			this._photoPrivacyLabel.rectTransform.offsetMin = new Vector2(22f, 0f);
			this._photoPrivacyLabel.raycastTarget = false;
			this._photoPrivacyHint = this.MakeLocalizedLabel(rectTransform, "", 11f, FontStyles.Normal, new Color(0.4f, 0.45f, 0.5f), TextAlignmentOptions.Left, false);
			CielXApp.Stretch(this._photoPrivacyHint.rectTransform);
			this._photoPrivacyHint.rectTransform.offsetMin = new Vector2(166f, 0f);
			this._photoPrivacyHint.raycastTarget = false;
			this._showFaceToggle = rectTransform2.gameObject.AddComponent<Toggle>();
			this._showFaceToggle.targetGraphic = this._photoPrivacyBox;
			this._showFaceToggle.graphic = oxGramVectorIcon;
			this._showFaceToggle.transition = Selectable.Transition.None;
			this._showFaceToggle.toggleTransition = Toggle.ToggleTransition.None;
			this._showFaceToggle.onValueChanged.AddListener(delegate(bool value)
			{
				this._showFace = value;
				this.RefreshPhotoPrivacy();
			});
			this.RefreshPhotoPrivacy();
		}

		// Token: 0x0600A042 RID: 41026 RVA: 0x00425DAC File Offset: 0x00423FAC
		private void RefreshPhotoPrivacy()
		{
			this.RefreshPendingPhoto();
			if (this._showFaceToggle == null || this._pendingOriginalPhoto == null)
			{
				return;
			}
			this._showFaceToggle.SetIsOnWithoutNotify(this.ShouldShowPendingFace);
			this._showFaceToggle.interactable = CielXPhotoPrivacy.CanShowFace && OxGramPhotoTag.ShowsFace(this._pendingPhotoName);
			bool shouldShowPendingFace = this.ShouldShowPendingFace;
			bool interactable = this._showFaceToggle.interactable;
			this._photoPrivacyBox.color = (shouldShowPendingFace ? this._accent : new Color(0.38f, 0.43f, 0.48f));
			this._photoPrivacyLabel.color = (interactable ? new Color(0.7f, 0.74f, 0.78f) : CielXApp.TextDim);
			StreamerCensorImage.Bind(this._composerPreview, false);
			if (this._composerPreview != null)
			{
				this._composerPreview.texture = ((this._pendingPhoto != null) ? this._pendingPhoto.texture : null);
			}
			string text = ((!OxGramPhotoTag.ShowsFace(this._pendingPhotoName)) ? "No face in this photo." : ((!CielXPhotoPrivacy.CanShowFace) ? "Visible at Certified Exhibitionist." : (this.ShouldShowPendingFace ? "People may recognize you." : "Your identity stays private.")));
			if (!this.ShouldShowPendingFace && OxGramGallery.IsPrivacyCover(this._pendingPhoto))
			{
				text = "Face blur unavailable. Photo hidden.";
			}
			LocalizedGameText.BindSource(this._photoPrivacyHint, "cielx_ui", text, null).FitTranslationToRectangle(false);
			this._photoPrivacyHint.gameObject.SetActive(!interactable || OxGramGallery.IsPrivacyCover(this._pendingPhoto));
		}

		// Token: 0x0600A043 RID: 41027 RVA: 0x00425F40 File Offset: 0x00424140
		private void ClearPendingPhoto()
		{
			this._pendingPhoto = null;
			this._pendingPhotoName = null;
			this._pendingOriginalPhoto = null;
			this._showFace = false;
			this._showFaceToggle = null;
			this._photoPrivacyHint = null;
			if (this._composerPreviewWrap != null)
			{
				this._composerPreviewWrap.gameObject.SetActive(false);
			}
		}

		// Token: 0x0600A044 RID: 41028 RVA: 0x00425F96 File Offset: 0x00424196
		private void ClosePicker()
		{
			if (this._picker != null)
			{
				CielXApp.DestroyUi(this._picker.gameObject);
				this._picker = null;
			}
			this._pickerSheet = null;
			this._pickerGrid = null;
		}

		// Token: 0x170018F4 RID: 6388
		// (get) Token: 0x0600A045 RID: 41029 RVA: 0x00425FCC File Offset: 0x004241CC
		private CielXPhotoArchive PhotoArchive
		{
			get
			{
				CielXPhotoArchive cielXPhotoArchive;
				if ((cielXPhotoArchive = this._photoArchive) == null)
				{
					cielXPhotoArchive = (this._photoArchive = CielXPhotoArchive.CreateDefault());
				}
				return cielXPhotoArchive;
			}
		}

		// Token: 0x0600A046 RID: 41030 RVA: 0x00425FF1 File Offset: 0x004241F1
		private static string PhotoCacheKey(string photoName, bool faceVisible)
		{
			return CielXApp.NormalizePhotoName(photoName) + (faceVisible ? ":visible" : ":protected");
		}

		// Token: 0x0600A047 RID: 41031 RVA: 0x0042600D File Offset: 0x0042420D
		private static string PostPhotoCacheKey(CielXPost post)
		{
			if (!string.IsNullOrEmpty(post.id))
			{
				return post.id + (post.faceVisible ? ":visible" : ":protected");
			}
			return CielXApp.PhotoCacheKey(post.photoName, post.faceVisible);
		}

		// Token: 0x0600A048 RID: 41032 RVA: 0x0042604D File Offset: 0x0042424D
		private static string NormalizePhotoName(string name)
		{
			return CielXPhotoArchive.NormalizeName(name);
		}

		// Token: 0x0600A049 RID: 41033 RVA: 0x00426058 File Offset: 0x00424258
		private Sprite ResolvePostPhoto(CielXPost post)
		{
			if (post.photoResolved && post.cachedPhoto != null)
			{
				return post.cachedPhoto;
			}
			if (string.IsNullOrEmpty(post.photoName))
			{
				return null;
			}
			string text = CielXApp.PostPhotoCacheKey(post);
			Sprite sprite;
			if (this._sessionPhotos.TryGetValue(text, out sprite) && sprite != null)
			{
				post.cachedPhoto = sprite;
				post.photoResolved = true;
				return sprite;
			}
			post.cachedPhoto = this.PhotoArchive.LoadPublished(post);
			bool flag = post.cachedPhoto != null;
			if (!flag)
			{
				post.cachedPhoto = this.LoadCensoredOrNormal(post.photoName, post.faceVisible);
			}
			post.photoResolved = post.cachedPhoto != null && !OxGramGallery.IsPrivacyCover(post.cachedPhoto);
			if (post.photoResolved)
			{
				if (!flag)
				{
					this.PhotoArchive.SavePublished(post, post.cachedPhoto);
				}
				this._sessionPhotos[text] = post.cachedPhoto;
			}
			return post.cachedPhoto;
		}

		// Token: 0x0600A04A RID: 41034 RVA: 0x00426155 File Offset: 0x00424355
		private Sprite LoadCensoredOrNormal(string baseName, bool faceVisible)
		{
			return this.PhotoArchive.LoadSource(baseName, faceVisible);
		}

		// Token: 0x0600A04B RID: 41035 RVA: 0x00027EE2 File Offset: 0x000260E2
		private static void DestroyPhotoObject(Object value)
		{
			if (Application.isPlaying)
			{
				Object.Destroy(value);
				return;
			}
			Object.DestroyImmediate(value);
		}

		// Token: 0x0600A04C RID: 41036 RVA: 0x00426164 File Offset: 0x00424364
		private static void DestroyUi(GameObject value)
		{
			if (value == null)
			{
				return;
			}
			value.SetActive(false);
			CielXApp.DestroyPhotoObject(value);
		}

		// Token: 0x0600A04D RID: 41037 RVA: 0x00426180 File Offset: 0x00424380
		private int NextRange(int minInclusive, int maxInclusive)
		{
			if (maxInclusive <= minInclusive)
			{
				return minInclusive;
			}
			uint num = (uint)this._rng;
			num ^= num << 13;
			num ^= num >> 17;
			num ^= num << 5;
			this._rng = (int)num;
			return minInclusive + (int)(num % (uint)(maxInclusive - minInclusive + 1));
		}

		// Token: 0x0600A04E RID: 41038 RVA: 0x004261C0 File Offset: 0x004243C0
		private static string RelativeAge(long minutes)
		{
			if (minutes < 1L)
			{
				return GameLocalization.GetSource("cielx_ui", "now");
			}
			if (minutes < 60L)
			{
				return GameLocalization.FormatSource("cielx_ui", "{0}m", new object[] { minutes });
			}
			long num = minutes / 60L;
			if (num < 24L)
			{
				return GameLocalization.FormatSource("cielx_ui", "{0}h", new object[] { num });
			}
			return GameLocalization.FormatSource("cielx_ui", "{0}d", new object[] { num / 24L });
		}

		// Token: 0x0600A04F RID: 41039 RVA: 0x00426254 File Offset: 0x00424454
		private static string ToHex(Color c)
		{
			return ColorUtility.ToHtmlStringRGB(c);
		}

		// Token: 0x0600A050 RID: 41040 RVA: 0x0042625C File Offset: 0x0042445C
		private RawImage AppLogo(Transform parent, float size)
		{
			RectTransform rectTransform = this.NewChild("Logo", parent);
			RawImage rawImage = rectTransform.gameObject.AddComponent<RawImage>();
			rawImage.raycastTarget = false;
			Texture texture = null;
			Sprite sprite = Resources.Load<Sprite>("Icons/CielX_Logo");
			if (sprite != null && sprite.texture != null)
			{
				texture = sprite.texture;
			}
			else
			{
				Sprite sprite2 = CielOSIconLibrary.GetSprite(CielOSIconId.CielX, (int)size);
				if (sprite2 != null)
				{
					texture = sprite2.texture;
				}
			}
			rawImage.texture = texture;
			StreamerCensorImage.Bind(rawImage, false);
			float num = ((texture != null && texture.height > 0) ? ((float)texture.width / (float)texture.height) : 1f);
			float num2 = size;
			float num3 = size;
			if (num >= 1f)
			{
				num3 = size / num;
			}
			else
			{
				num2 = size * num;
			}
			rectTransform.sizeDelta = new Vector2(num2, num3);
			return rawImage;
		}

		// Token: 0x0600A051 RID: 41041 RVA: 0x003FDF19 File Offset: 0x003FC119
		private RectTransform NewChild(string name, Transform parent)
		{
			GameObject gameObject = new GameObject(name, new Type[] { typeof(RectTransform) });
			gameObject.transform.SetParent(parent, false);
			return (RectTransform)gameObject.transform;
		}

		// Token: 0x0600A052 RID: 41042 RVA: 0x00426334 File Offset: 0x00424534
		private TMP_Text MakeLabel(Transform parent, string text, float size, FontStyles style, Color color, TextAlignmentOptions align, bool wrap = false)
		{
			TextMeshProUGUI textMeshProUGUI = this.NewChild("Label", parent).gameObject.AddComponent<TextMeshProUGUI>();
			textMeshProUGUI.text = text;
			textMeshProUGUI.fontSize = size;
			textMeshProUGUI.fontStyle = style;
			textMeshProUGUI.color = color;
			textMeshProUGUI.alignment = align;
			textMeshProUGUI.enableWordWrapping = wrap;
			if (this._font != null)
			{
				textMeshProUGUI.font = this._font;
			}
			LocalizedGameText.BindFontOnly(textMeshProUGUI);
			return textMeshProUGUI;
		}

		// Token: 0x0600A053 RID: 41043 RVA: 0x004263A8 File Offset: 0x004245A8
		private TMP_Text MakeLocalizedLabel(Transform parent, string text, float size, FontStyles style, Color color, TextAlignmentOptions align, bool wrap = false)
		{
			TMP_Text tmp_Text = this.MakeLabel(parent, text, size, style, color, align, wrap);
			LocalizedGameText.BindSource(tmp_Text, "cielx_ui", text, null).FitTranslationToRectangle(false);
			return tmp_Text;
		}

		// Token: 0x0600A054 RID: 41044 RVA: 0x004263D0 File Offset: 0x004245D0
		private static string ReplyText(CielXReply reply)
		{
			if (GameLocalization.CurrentCode == "en")
			{
				return reply.text;
			}
			string sourceOpener = reply.sourceOpener;
			string sourceCore = reply.sourceCore;
			string sourceCloser = reply.sourceCloser;
			bool capitalizeSource = reply.capitalizeSource;
			if (string.IsNullOrEmpty(sourceCore) && !CielXSeed.TryResolveReplySource(reply.text, out sourceOpener, out sourceCore, out sourceCloser, out capitalizeSource))
			{
				return reply.text;
			}
			string source = GameLocalization.GetSource("phone_story", sourceCore);
			string source2 = GameLocalization.GetSource("phone_story", sourceOpener ?? "");
			string source3 = GameLocalization.GetSource("phone_story", sourceCloser ?? "");
			string text = ((GameLocalization.CurrentCode == "ja") ? "、" : ", ");
			if (GameLocalization.CurrentCode == "zh-Hans" || GameLocalization.CurrentCode == "zh-Hant")
			{
				text = "，";
			}
			string text2 = (string.IsNullOrEmpty(source2) ? source : (source2.Trim() + text + source));
			if (capitalizeSource && text2.Length > 0)
			{
				text2 = char.ToUpperInvariant(text2[0]).ToString() + text2.Substring(1);
			}
			return text2 + (string.IsNullOrEmpty(source3) ? "" : (" " + source3.TrimStart()));
		}

		// Token: 0x0600A055 RID: 41045 RVA: 0x00426530 File Offset: 0x00424730
		private RawImage MakeAvatar(Transform parent, Sprite avatar, float size, Vector2 pos, Vector2 anchor)
		{
			RectTransform rectTransform = this.NewChild("Avatar", parent);
			rectTransform.gameObject.AddComponent<LayoutElement>().ignoreLayout = true;
			rectTransform.anchorMax = anchor;
			rectTransform.anchorMin = anchor;
			rectTransform.pivot = anchor;
			rectTransform.sizeDelta = new Vector2(size, size);
			rectTransform.anchoredPosition = pos;
			RawImage rawImage = rectTransform.gameObject.AddComponent<RawImage>();
			rawImage.texture = ((avatar != null) ? avatar.texture : null);
			rawImage.color = Color.white;
			rawImage.raycastTarget = false;
			return rawImage;
		}

		// Token: 0x0600A056 RID: 41046 RVA: 0x004265BC File Offset: 0x004247BC
		private Image MakeIcon(Transform parent, CielOSIconId id, int size, Color color)
		{
			RectTransform rectTransform = this.NewChild("Icon", parent);
			rectTransform.sizeDelta = new Vector2((float)size, (float)size);
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.sprite = CielOSIconLibrary.GetSprite(id, size);
			image.color = color;
			image.preserveAspect = true;
			return image;
		}

		// Token: 0x0600A057 RID: 41047 RVA: 0x0042660C File Offset: 0x0042480C
		private TMP_InputField MakeInput(Transform parent, string placeholder, bool isPassword)
		{
			RectTransform rectTransform = this.NewChild("Input", parent);
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = new Color(0.13f, 0.14f, 0.16f, 1f);
			CielXApp.RoundOpaque(image);
			TMP_InputField tmp_InputField = rectTransform.gameObject.AddComponent<TMP_InputField>();
			tmp_InputField.lineType = TMP_InputField.LineType.SingleLine;
			if (isPassword)
			{
				tmp_InputField.contentType = TMP_InputField.ContentType.Password;
				tmp_InputField.asteriskChar = '*';
			}
			RectTransform rectTransform2 = this.NewChild("TextArea", rectTransform);
			rectTransform2.anchorMin = Vector2.zero;
			rectTransform2.anchorMax = Vector2.one;
			rectTransform2.offsetMin = new Vector2(12f, 4f);
			rectTransform2.offsetMax = new Vector2(-12f, -4f);
			rectTransform2.gameObject.AddComponent<RectMask2D>();
			tmp_InputField.textViewport = rectTransform2;
			TMP_Text tmp_Text = this.MakeLocalizedLabel(rectTransform2, placeholder, 15f, FontStyles.Normal, CielXApp.TextDim, TextAlignmentOptions.Left, false);
			CielXApp.Stretch((RectTransform)tmp_Text.transform);
			tmp_InputField.placeholder = tmp_Text;
			TMP_Text tmp_Text2 = this.MakeLabel(rectTransform2, "", 15f, FontStyles.Normal, CielXApp.TextMain, TextAlignmentOptions.Left, false);
			CielXApp.Stretch((RectTransform)tmp_Text2.transform);
			tmp_InputField.textComponent = (TextMeshProUGUI)tmp_Text2;
			return tmp_InputField;
		}

		// Token: 0x0600A058 RID: 41048 RVA: 0x0042674C File Offset: 0x0042494C
		private TMP_InputField BuildMultilineInput(RectTransform rt, string placeholder)
		{
			TMP_InputField tmp_InputField = rt.gameObject.AddComponent<TMP_InputField>();
			tmp_InputField.lineType = TMP_InputField.LineType.MultiLineNewline;
			tmp_InputField.characterLimit = 280;
			RectTransform rectTransform = this.NewChild("TextArea", rt);
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.offsetMin = new Vector2(2f, 2f);
			rectTransform.offsetMax = new Vector2(-2f, -2f);
			rectTransform.gameObject.AddComponent<RectMask2D>();
			tmp_InputField.textViewport = rectTransform;
			TMP_Text tmp_Text = this.MakeLocalizedLabel(rectTransform, placeholder, 18f, FontStyles.Normal, CielXApp.TextDim, TextAlignmentOptions.TopLeft, false);
			tmp_Text.enableWordWrapping = true;
			CielXApp.Stretch((RectTransform)tmp_Text.transform);
			tmp_InputField.placeholder = tmp_Text;
			TMP_Text tmp_Text2 = this.MakeLabel(rectTransform, "", 18f, FontStyles.Normal, CielXApp.TextMain, TextAlignmentOptions.TopLeft, false);
			tmp_Text2.enableWordWrapping = true;
			CielXApp.Stretch((RectTransform)tmp_Text2.transform);
			tmp_InputField.textComponent = (TextMeshProUGUI)tmp_Text2;
			return tmp_InputField;
		}

		// Token: 0x0600A059 RID: 41049 RVA: 0x00426850 File Offset: 0x00424A50
		private RectTransform MakeButton(Transform parent, string label, Color bgColor, Color textColor)
		{
			RectTransform rectTransform = this.NewChild("Button", parent);
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = bgColor;
			CielXApp.Round(image);
			rectTransform.gameObject.AddComponent<Button>().targetGraphic = image;
			if (!string.IsNullOrEmpty(label))
			{
				CielXApp.Stretch((RectTransform)this.MakeLocalizedLabel(rectTransform, label, 15f, FontStyles.Bold, textColor, TextAlignmentOptions.Center, false).transform);
			}
			return rectTransform;
		}

		// Token: 0x0600A05A RID: 41050 RVA: 0x004268C4 File Offset: 0x00424AC4
		private void Divider1(Transform parent)
		{
			RectTransform rectTransform = this.NewChild("Divider", parent);
			LayoutElement layoutElement = rectTransform.gameObject.AddComponent<LayoutElement>();
			layoutElement.minHeight = 1f;
			layoutElement.preferredHeight = 1f;
			rectTransform.gameObject.AddComponent<Image>().color = CielXApp.Divider;
		}

		// Token: 0x0600A05B RID: 41051 RVA: 0x00426911 File Offset: 0x00424B11
		private void Spacer(Transform parent, float hgt)
		{
			LayoutElement layoutElement = this.NewChild("Spacer", parent).gameObject.AddComponent<LayoutElement>();
			layoutElement.minHeight = hgt;
			layoutElement.preferredHeight = hgt;
		}

		// Token: 0x0600A05C RID: 41052 RVA: 0x00426936 File Offset: 0x00424B36
		private void FixedHeight(RectTransform rt, float hgt)
		{
			LayoutElement layoutElement = rt.gameObject.GetComponent<LayoutElement>() ?? rt.gameObject.AddComponent<LayoutElement>();
			layoutElement.minHeight = hgt;
			layoutElement.preferredHeight = hgt;
		}

		// Token: 0x0600A05D RID: 41053 RVA: 0x0042695F File Offset: 0x00424B5F
		private void AddHeight(GameObject go, float hgt)
		{
			LayoutElement layoutElement = go.GetComponent<LayoutElement>() ?? go.AddComponent<LayoutElement>();
			layoutElement.minHeight = hgt;
			layoutElement.preferredHeight = hgt;
		}

		// Token: 0x0600A05E RID: 41054 RVA: 0x00426980 File Offset: 0x00424B80
		private void AddClick(GameObject go, UnityAction action)
		{
			object obj = go.GetComponent<Button>() ?? go.AddComponent<Button>();
			if (go.GetComponent<Graphic>() == null)
			{
				go.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
			}
			object obj2 = obj;
			obj2.transition = Selectable.Transition.None;
			obj2.onClick.AddListener(action);
		}

		// Token: 0x0600A05F RID: 41055 RVA: 0x001A38AD File Offset: 0x001A1AAD
		private static void Stretch(RectTransform rt)
		{
			rt.anchorMin = Vector2.zero;
			rt.anchorMax = Vector2.one;
			rt.offsetMin = Vector2.zero;
			rt.offsetMax = Vector2.zero;
		}

		// Token: 0x0600A060 RID: 41056 RVA: 0x004269E1 File Offset: 0x00424BE1
		private static void Round(Image img)
		{
			img.sprite = CielOSShapes.RoundedRect(10);
			img.type = Image.Type.Sliced;
			img.pixelsPerUnitMultiplier = 1f;
		}

		// Token: 0x0600A061 RID: 41057 RVA: 0x00426A02 File Offset: 0x00424C02
		private static void RoundOpaque(Image img)
		{
			img.sprite = null;
			img.type = Image.Type.Simple;
		}

		// Token: 0x0600A062 RID: 41058 RVA: 0x00426A12 File Offset: 0x00424C12
		private static void Circle(Image img)
		{
			img.sprite = CielOSShapes.Circle(64);
			img.type = Image.Type.Simple;
			img.preserveAspect = true;
		}

		// Token: 0x0600A065 RID: 41061 RVA: 0x00426BD8 File Offset: 0x00424DD8
		[CompilerGenerated]
		private void <HairlineBox>g__Edge|59_0(Vector2 aMin, Vector2 aMax, Vector2 oMin, Vector2 oMax, ref CielXApp.<>c__DisplayClass59_0 A_5)
		{
			RectTransform rectTransform = this.NewChild("Edge", A_5.target);
			rectTransform.anchorMin = aMin;
			rectTransform.anchorMax = aMax;
			rectTransform.offsetMin = oMin;
			rectTransform.offsetMax = oMax;
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = A_5.c;
			CielXApp.RoundOpaque(image);
			image.raycastTarget = false;
		}

		// Token: 0x0400A3E2 RID: 41954
		private static readonly Color Bg = new Color(0f, 0f, 0f, 1f);

		// Token: 0x0400A3E3 RID: 41955
		private static readonly Color RailBg = new Color(0f, 0f, 0f, 1f);

		// Token: 0x0400A3E4 RID: 41956
		private static readonly Color Card = new Color(0.086f, 0.094f, 0.106f, 1f);

		// Token: 0x0400A3E5 RID: 41957
		private static readonly Color CardHover = new Color(0.12f, 0.13f, 0.15f, 1f);

		// Token: 0x0400A3E6 RID: 41958
		private static readonly Color Hover = new Color(1f, 1f, 1f, 0.06f);

		// Token: 0x0400A3E7 RID: 41959
		private static readonly Color Divider = new Color(1f, 1f, 1f, 0.1f);

		// Token: 0x0400A3E8 RID: 41960
		private static readonly Color TextMain = new Color(0.92f, 0.94f, 0.96f, 1f);

		// Token: 0x0400A3E9 RID: 41961
		private static readonly Color TextDim = new Color(0.44f, 0.5f, 0.56f, 1f);

		// Token: 0x0400A3EA RID: 41962
		private Color _accent = new Color(0.11f, 0.63f, 0.95f, 1f);

		// Token: 0x0400A3EB RID: 41963
		private static readonly Color LikeColor = new Color(0.97f, 0.22f, 0.44f, 1f);

		// Token: 0x0400A3EC RID: 41964
		private static readonly Color RepostColor = new Color(0f, 0.73f, 0.46f, 1f);

		// Token: 0x0400A3ED RID: 41965
		private const string AppName = "People";

		// Token: 0x0400A3EE RID: 41966
		private const string LogoResource = "Icons/CielX_Logo";

		// Token: 0x0400A3EF RID: 41967
		private const float RailWidth = 268f;

		// Token: 0x0400A3F0 RID: 41968
		private const float FeedMaxWidth = 920f;

		// Token: 0x0400A3F1 RID: 41969
		private const float ScrollNotchFraction = 0.04f;

		// Token: 0x0400A3F2 RID: 41970
		private const float ScrollSmoothing = 8f;

		// Token: 0x0400A3F3 RID: 41971
		private TMP_FontAsset _font;

		// Token: 0x0400A3F4 RID: 41972
		private RectTransform _self;

		// Token: 0x0400A3F5 RID: 41973
		private RectTransform _loginScreen;

		// Token: 0x0400A3F6 RID: 41974
		private RectTransform _feedScreen;

		// Token: 0x0400A3F7 RID: 41975
		private RectTransform _rightColumn;

		// Token: 0x0400A3F8 RID: 41976
		private CielXWheelScroll _scroll;

		// Token: 0x0400A3F9 RID: 41977
		private RectTransform _feedContent;

		// Token: 0x0400A3FA RID: 41978
		private TMP_Text _railFollowerLabel;

		// Token: 0x0400A3FB RID: 41979
		private TMP_Text _railHandleLabel;

		// Token: 0x0400A3FC RID: 41980
		private TMP_InputField _composerInput;

		// Token: 0x0400A3FD RID: 41981
		private RawImage _composerPreview;

		// Token: 0x0400A3FE RID: 41982
		private RectTransform _composerPreviewWrap;

		// Token: 0x0400A3FF RID: 41983
		private LayoutElement _composerLayout;

		// Token: 0x0400A400 RID: 41984
		private Sprite _pendingPhoto;

		// Token: 0x0400A401 RID: 41985
		private Sprite _pendingOriginalPhoto;

		// Token: 0x0400A402 RID: 41986
		private bool _showFace;

		// Token: 0x0400A403 RID: 41987
		private Toggle _showFaceToggle;

		// Token: 0x0400A404 RID: 41988
		private TMP_Text _photoPrivacyHint;

		// Token: 0x0400A405 RID: 41989
		private Image _photoPrivacyBox;

		// Token: 0x0400A406 RID: 41990
		private TMP_Text _photoPrivacyLabel;

		// Token: 0x0400A407 RID: 41991
		private string _pendingPhotoName;

		// Token: 0x0400A408 RID: 41992
		private string _composerDraft = "";

		// Token: 0x0400A409 RID: 41993
		private readonly List<CielXPost> _feed = new List<CielXPost>();

		// Token: 0x0400A40A RID: 41994
		private readonly List<CielXApp.PostWidgets> _liveWidgets = new List<CielXApp.PostWidgets>();

		// Token: 0x0400A40B RID: 41995
		private int _rng = 439041101;

		// Token: 0x0400A40C RID: 41996
		private Coroutine _ticker;

		// Token: 0x0400A40D RID: 41997
		private TMP_Text _statLikes;

		// Token: 0x0400A40E RID: 41998
		private TMP_Text _statReposts;

		// Token: 0x0400A40F RID: 41999
		private TMP_Text _statReplies;

		// Token: 0x0400A410 RID: 42000
		private const float ComposerBaseHeight = 128f;

		// Token: 0x0400A411 RID: 42001
		private const float ComposerPreviewHeight = 240f;

		// Token: 0x0400A412 RID: 42002
		private RectTransform _detail;

		// Token: 0x0400A413 RID: 42003
		private RectTransform _deletePopup;

		// Token: 0x0400A414 RID: 42004
		private RectTransform _picker;

		// Token: 0x0400A415 RID: 42005
		private RectTransform _pickerSheet;

		// Token: 0x0400A416 RID: 42006
		private GridLayoutGroup _pickerGrid;

		// Token: 0x0400A417 RID: 42007
		private readonly Dictionary<string, Sprite> _sessionPhotos = new Dictionary<string, Sprite>();

		// Token: 0x0400A418 RID: 42008
		private CielXPhotoArchive _photoArchive;

		// Token: 0x02001440 RID: 5184
		private class PostWidgets
		{
			// Token: 0x0400A419 RID: 42009
			public CielXPost post;

			// Token: 0x0400A41A RID: 42010
			public TMP_Text like;

			// Token: 0x0400A41B RID: 42011
			public Image likeIcon;

			// Token: 0x0400A41C RID: 42012
			public TMP_Text repost;

			// Token: 0x0400A41D RID: 42013
			public Image repostIcon;
		}
	}
}
