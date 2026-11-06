using System;
using System.Collections;
using System.Collections.Generic;
using Features.Achievements;
using Features.CensorBars;
using Features.Localization;
using Features.PhoneCompanion;
using Features.Rank;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.OxGram
{
	// Token: 0x02000E95 RID: 3733
	public class OxGramChatView : MonoBehaviour
	{
		// Token: 0x140000C3 RID: 195
		// (add) Token: 0x06007023 RID: 28707 RVA: 0x002D5DA4 File Offset: 0x002D3FA4
		// (remove) Token: 0x06007024 RID: 28708 RVA: 0x002D5DDC File Offset: 0x002D3FDC
		public event Action<OxGramChat> ChatChanged;

		// Token: 0x140000C4 RID: 196
		// (add) Token: 0x06007025 RID: 28709 RVA: 0x002D5E14 File Offset: 0x002D4014
		// (remove) Token: 0x06007026 RID: 28710 RVA: 0x002D5E4C File Offset: 0x002D404C
		public event Action<OxGramChat> RenameRequested;

		// Token: 0x06007027 RID: 28711 RVA: 0x002D5E84 File Offset: 0x002D4084
		private void OnEnable()
		{
			GameLocalization.LocaleChanged += this.RefreshLocaleText;
			RankProgressionService.Changed += this.RefreshPhotoPrivacy;
			this.RefreshPhotoPrivacy();
			if (this._photoPicker != null && this._photoPicker.gameObject.activeSelf)
			{
				OxGramGallery.DecodeWhileOpen(this._photoPicker.gameObject);
			}
		}

		// Token: 0x06007028 RID: 28712 RVA: 0x002D5EE9 File Offset: 0x002D40E9
		private void OnDisable()
		{
			GameLocalization.LocaleChanged -= this.RefreshLocaleText;
			RankProgressionService.Changed -= this.RefreshPhotoPrivacy;
		}

		// Token: 0x06007029 RID: 28713 RVA: 0x002D5F10 File Offset: 0x002D4110
		private void RefreshLocaleText()
		{
			if (this._composerStatus != null)
			{
				this.SetTyping(this._typingAuthor);
			}
			this.RefreshPhotoPrivacy();
			if (this._scroll != null && base.isActiveAndEnabled)
			{
				base.StartCoroutine(this.RestoreLocaleScroll(this._scroll.verticalNormalizedPosition));
			}
		}

		// Token: 0x0600702A RID: 28714 RVA: 0x002D5F6B File Offset: 0x002D416B
		private IEnumerator RestoreLocaleScroll(float position)
		{
			yield return null;
			if (this._scroll == null || this._content == null)
			{
				yield break;
			}
			Canvas.ForceUpdateCanvases();
			LayoutRebuilder.ForceRebuildLayoutImmediate(this._content);
			this._scroll.verticalNormalizedPosition = position;
			yield break;
		}

		// Token: 0x17001255 RID: 4693
		// (get) Token: 0x0600702B RID: 28715 RVA: 0x002D5F81 File Offset: 0x002D4181
		public OxGramChat Chat
		{
			get
			{
				return this._chat;
			}
		}

		// Token: 0x0600702C RID: 28716 RVA: 0x002D5F8C File Offset: 0x002D418C
		public void Build(OxGramTheme theme, Action onBack, Action onClose, OxGramReactionPool reactionPool = null, float censorBugChance = 0f)
		{
			this._theme = theme;
			this._onBack = onBack;
			this._onClose = onClose;
			this._reactionPool = reactionPool;
			this._censorBugChance = censorBugChance;
			this._root = (RectTransform)base.transform;
			OxGramUI.Stretch(OxGramUI.Image("ChatBg", this._root, theme.appBackground, null).rectTransform, 0f, 0f, 0f, 0f);
			this.BuildHeader();
			this.BuildMessageScroll();
			this.BuildInputBar();
			this._life = base.gameObject.AddComponent<OxGramLifeSimulator>();
			this._life.Init(this, theme, this._reactionPool);
		}

		// Token: 0x0600702D RID: 28717 RVA: 0x002D603C File Offset: 0x002D423C
		private void BuildHeader()
		{
			Image image = OxGramUI.Image("Header", this._root, this._theme.headerColor, null);
			image.rectTransform.anchorMin = new Vector2(0f, 1f);
			image.rectTransform.anchorMax = new Vector2(1f, 1f);
			image.rectTransform.pivot = new Vector2(0.5f, 1f);
			image.rectTransform.sizeDelta = new Vector2(0f, 104f);
			image.rectTransform.anchoredPosition = Vector2.zero;
			this.BuildHeaderIconButton(image.transform, "BackButton", OxGramVectorIcon.Symbol.Back, false, delegate
			{
				Action onBack = this._onBack;
				if (onBack == null)
				{
					return;
				}
				onBack();
			});
			this.BuildHeaderIconButton(image.transform, "GroupDetailsButton", OxGramVectorIcon.Symbol.Info, true, new UnityAction(this.ShowGroupDetails));
			Image image2 = OxGramUI.Image("GroupIdentity", image.transform, Color.clear, null);
			OxGramUI.Stretch(image2.rectTransform, 84f, 84f, 36f, 12f);
			OxGramUI.Button(image2.gameObject).onClick.AddListener(new UnityAction(this.ShowGroupDetails));
			this._titleText = OxGramUI.Text("Title", image2.transform, "", (this._theme.fontBold != null) ? this._theme.fontBold : this._theme.font, 21f, this._theme.headerForeground, TextAlignmentOptions.Center);
			OxGramUI.Stretch(this._titleText.rectTransform, 0f, 0f, 0f, 20f);
			this._titleText.richText = false;
			this._titleText.enableWordWrapping = false;
			this._titleText.enableAutoSizing = true;
			this._titleText.fontSizeMin = 17f;
			this._titleText.fontSizeMax = 21f;
			this._titleText.overflowMode = TextOverflowModes.Ellipsis;
			this._titleText.raycastTarget = false;
			this._subtitleText = OxGramUI.Text("Subtitle", image2.transform, "", this._theme.font, 12f, this._theme.textSecondary, TextAlignmentOptions.Center);
			OxGramUI.Stretch(this._subtitleText.rectTransform, 0f, 0f, 34f, 0f);
			this._subtitleText.enableWordWrapping = false;
			this._subtitleText.overflowMode = TextOverflowModes.Ellipsis;
			this._subtitleText.raycastTarget = false;
			Image image3 = OxGramUI.Image("Divider", image.transform, this._theme.listRowDivider, null);
			image3.rectTransform.anchorMin = Vector2.zero;
			image3.rectTransform.anchorMax = new Vector2(1f, 0f);
			image3.rectTransform.sizeDelta = new Vector2(0f, 1f);
			image3.raycastTarget = false;
		}

		// Token: 0x0600702E RID: 28718 RVA: 0x002D633C File Offset: 0x002D453C
		private void BuildHeaderIconButton(Transform header, string name, OxGramVectorIcon.Symbol symbol, bool right, UnityAction onClick)
		{
			Image image = OxGramUI.Image(name, header, Color.clear, null);
			RectTransform rectTransform = image.rectTransform;
			RectTransform rectTransform2 = image.rectTransform;
			Vector2 vector = new Vector2(right ? 1f : 0f, 1f);
			rectTransform2.anchorMax = vector;
			rectTransform.anchorMin = vector;
			image.rectTransform.pivot = new Vector2(right ? 1f : 0f, 1f);
			image.rectTransform.sizeDelta = new Vector2(44f, 44f);
			image.rectTransform.anchoredPosition = new Vector2(right ? (-24f) : 24f, -42f);
			RectTransform rectTransform3 = OxGramUI.Rect("Icon", image.transform);
			OxGramUI.Stretch(rectTransform3, 10f, 10f, 10f, 10f);
			OxGramVectorIcon oxGramVectorIcon = rectTransform3.gameObject.AddComponent<OxGramVectorIcon>();
			oxGramVectorIcon.symbol = symbol;
			oxGramVectorIcon.color = this._theme.authorAccent;
			oxGramVectorIcon.raycastTarget = false;
			Button button = OxGramUI.Button(image.gameObject);
			button.targetGraphic = oxGramVectorIcon;
			button.onClick.AddListener(onClick);
		}

		// Token: 0x0600702F RID: 28719 RVA: 0x002D6464 File Offset: 0x002D4664
		private void BuildMessageScroll()
		{
			RectTransform rectTransform = OxGramUI.Rect("Messages", this._root);
			OxGramUI.Stretch(rectTransform, 0f, 0f, 104f, 108f);
			this._scroll = rectTransform.gameObject.AddComponent<ScrollRect>();
			this._scroll.horizontal = false;
			this._scroll.vertical = true;
			this._scroll.movementType = ScrollRect.MovementType.Clamped;
			this._scroll.scrollSensitivity = 20f;
			this._scroll.onValueChanged.AddListener(delegate(Vector2 _)
			{
				if (this.IsNearBottom() && this._newMessages != null)
				{
					this._newMessages.gameObject.SetActive(false);
				}
			});
			Image image = OxGramUI.Image("Viewport", rectTransform, new Color(0f, 0f, 0f, 0f), null);
			OxGramUI.Stretch(image.rectTransform, 0f, 0f, 0f, 0f);
			image.gameObject.AddComponent<RectMask2D>();
			this._scroll.viewport = image.rectTransform;
			this._content = OxGramUI.Rect("Content", image.transform);
			this._content.anchorMin = new Vector2(0f, 0f);
			this._content.anchorMax = new Vector2(1f, 0f);
			this._content.pivot = new Vector2(0.5f, 0f);
			this._content.sizeDelta = new Vector2(0f, 0f);
			VerticalLayoutGroup verticalLayoutGroup = this._content.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.childControlWidth = true;
			verticalLayoutGroup.childControlHeight = true;
			verticalLayoutGroup.childForceExpandWidth = true;
			verticalLayoutGroup.childForceExpandHeight = false;
			verticalLayoutGroup.spacing = 12f;
			verticalLayoutGroup.padding = new RectOffset(20, 20, 20, 20);
			this._content.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			this._scroll.content = this._content;
			Image image2 = OxGramUI.Image("NewMessages", this._root, this._theme.accent, OxGramBubbleSprite.Body);
			this._newMessages = image2.rectTransform;
			this._newMessages.anchorMin = new Vector2(0.5f, 0f);
			this._newMessages.anchorMax = new Vector2(0.5f, 0f);
			this._newMessages.pivot = new Vector2(0.5f, 0f);
			this._newMessages.anchoredPosition = new Vector2(0f, 120f);
			this._newMessages.sizeDelta = new Vector2(160f, 36f);
			TMP_Text tmp_Text = OxGramUI.LocalizedText("Label", image2.transform, "New messages", this._theme.font, 14f, Color.white, TextAlignmentOptions.Center);
			OxGramUI.Stretch(tmp_Text.rectTransform, 0f, 0f, 0f, 0f);
			tmp_Text.raycastTarget = false;
			OxGramUI.Button(image2.gameObject).onClick.AddListener(new UnityAction(this.ScrollToBottomNextFrame));
			this._newMessages.gameObject.SetActive(false);
		}

		// Token: 0x06007030 RID: 28720 RVA: 0x002D6788 File Offset: 0x002D4988
		private void BuildInputBar()
		{
			Image image = OxGramUI.Image("InputBar", this._root, this._theme.inputBar, null);
			image.rectTransform.anchorMin = new Vector2(0f, 0f);
			image.rectTransform.anchorMax = new Vector2(1f, 0f);
			image.rectTransform.pivot = new Vector2(0.5f, 0f);
			image.rectTransform.sizeDelta = new Vector2(0f, 108f);
			image.rectTransform.anchoredPosition = Vector2.zero;
			this._composerStatus = OxGramUI.Text("PostingAs", image.transform, "", this._theme.font, 12f, this._theme.textSecondary, TextAlignmentOptions.Left);
			this._composerStatus.rectTransform.anchorMin = new Vector2(0f, 1f);
			this._composerStatus.rectTransform.anchorMax = Vector2.one;
			this._composerStatus.rectTransform.pivot = new Vector2(0.5f, 1f);
			this._composerStatus.rectTransform.sizeDelta = new Vector2(-44f, 22f);
			this._composerStatus.rectTransform.anchoredPosition = new Vector2(0f, -2f);
			this._composerStatus.richText = false;
			this._composerStatus.enableWordWrapping = false;
			this._composerStatus.overflowMode = TextOverflowModes.Ellipsis;
			this._composerStatus.raycastTarget = false;
			float num = -4f;
			Image image2 = OxGramUI.Image("AttachPhoto", image.transform, Color.clear, null);
			image2.rectTransform.anchorMin = new Vector2(0f, 0.5f);
			image2.rectTransform.anchorMax = new Vector2(0f, 0.5f);
			image2.rectTransform.pivot = new Vector2(0f, 0.5f);
			image2.rectTransform.sizeDelta = new Vector2(44f, 44f);
			image2.rectTransform.anchoredPosition = new Vector2(18f, num);
			RectTransform rectTransform = OxGramUI.Rect("Paperclip", image2.transform);
			OxGramUI.Stretch(rectTransform, 8f, 8f, 8f, 8f);
			OxGramVectorIcon oxGramVectorIcon = rectTransform.gameObject.AddComponent<OxGramVectorIcon>();
			oxGramVectorIcon.symbol = OxGramVectorIcon.Symbol.Paperclip;
			oxGramVectorIcon.color = this._theme.textSecondary;
			oxGramVectorIcon.raycastTarget = false;
			Button button = OxGramUI.Button(image2.gameObject);
			button.targetGraphic = oxGramVectorIcon;
			button.onClick.AddListener(new UnityAction(this.OnAttachPhoto));
			Image image3 = OxGramUI.Image("Field", image.transform, this._theme.bubbleOther, OxGramBubbleSprite.Body);
			image3.rectTransform.anchorMin = new Vector2(0f, 0.5f);
			image3.rectTransform.anchorMax = new Vector2(1f, 0.5f);
			image3.rectTransform.pivot = new Vector2(0.5f, 0.5f);
			image3.rectTransform.offsetMin = new Vector2(72f, -22f + num);
			image3.rectTransform.offsetMax = new Vector2(-72f, 22f + num);
			RectTransform rectTransform2 = OxGramUI.Rect("TextArea", image3.transform);
			OxGramUI.Stretch(rectTransform2, 12f, 12f, 4f, 4f);
			rectTransform2.gameObject.AddComponent<RectMask2D>();
			TMP_Text tmp_Text = OxGramUI.LocalizedText("Placeholder", rectTransform2, "Write a message...", this._theme.font, 17f, this._theme.textSecondary, TextAlignmentOptions.Left);
			OxGramUI.Stretch(tmp_Text.rectTransform, 0f, 0f, 0f, 0f);
			TMP_Text tmp_Text2 = OxGramUI.Text("Text", rectTransform2, "", this._theme.font, 17f, this._theme.textPrimary, TextAlignmentOptions.Left);
			OxGramUI.Stretch(tmp_Text2.rectTransform, 0f, 0f, 0f, 0f);
			tmp_Text2.richText = false;
			this._input = image3.gameObject.AddComponent<TMP_InputField>();
			this._input.textViewport = rectTransform2;
			this._input.textComponent = tmp_Text2;
			this._input.placeholder = tmp_Text;
			this._input.lineType = TMP_InputField.LineType.SingleLine;
			this._input.characterLimit = 500;
			this._input.onSubmit.AddListener(delegate(string _)
			{
				this.OnSend();
			});
			this._input.onValueChanged.AddListener(delegate(string _)
			{
				this.RefreshSendButton();
			});
			Image image4 = OxGramUI.Image("Send", image.transform, this._theme.accent, this._theme.circleSprite);
			image4.rectTransform.anchorMin = new Vector2(1f, 0.5f);
			image4.rectTransform.anchorMax = new Vector2(1f, 0.5f);
			image4.rectTransform.pivot = new Vector2(1f, 0.5f);
			image4.rectTransform.sizeDelta = new Vector2(44f, 44f);
			image4.rectTransform.anchoredPosition = new Vector2(-18f, num);
			if (this._theme.sendSprite != null)
			{
				Image image5 = OxGramUI.Image("Icon", image4.transform, Color.white, this._theme.sendSprite);
				image5.type = Image.Type.Simple;
				image5.preserveAspect = true;
				image5.raycastTarget = false;
				OxGramUI.Stretch(image5.rectTransform, 10f, 10f, 10f, 10f);
			}
			else
			{
				TMP_Text tmp_Text3 = OxGramUI.Text("Arrow", image4.transform, "âž¤", this._theme.font, 20f, Color.white, TextAlignmentOptions.Center);
				OxGramUI.Stretch(tmp_Text3.rectTransform, 0f, 0f, 0f, 0f);
				tmp_Text3.raycastTarget = false;
			}
			this._sendButton = OxGramUI.Button(image4.gameObject);
			this._sendButton.onClick.AddListener(new UnityAction(this.OnSend));
			this._sendVisuals = image4.gameObject.AddComponent<CanvasGroup>();
			this.RefreshSendButton();
		}

		// Token: 0x06007031 RID: 28721 RVA: 0x002D6E18 File Offset: 0x002D5018
		private void RefreshSendButton()
		{
			if (this._sendButton == null)
			{
				return;
			}
			bool flag = this._input != null && !string.IsNullOrWhiteSpace(this._input.text);
			this._sendButton.interactable = flag;
			this._sendVisuals.alpha = (flag ? 1f : 0.4f);
		}

		// Token: 0x06007032 RID: 28722 RVA: 0x002D6E80 File Offset: 0x002D5080
		public void SetTyping(string author)
		{
			if (this._composerStatus == null)
			{
				return;
			}
			this._typingAuthor = author;
			if (string.IsNullOrEmpty(author))
			{
				LocalizedGameText.BindFormat(this._composerStatus, "phone_story", "Posting as {0}", new object[] { OxGramUser.Mention });
			}
			else
			{
				LocalizedGameText.BindFormat(this._composerStatus, "phone_story", "{0} is typing…", new object[] { author });
			}
			this._composerStatus.color = (string.IsNullOrEmpty(author) ? this._theme.textSecondary : this._theme.accent);
		}

		// Token: 0x06007033 RID: 28723 RVA: 0x002D6F1C File Offset: 0x002D511C
		public void Open(OxGramChat chat)
		{
			this._chat = chat;
			if (chat.isGroup)
			{
				LocalizedGameText.BindSource(this._titleText, "phone_story", chat.title, null);
			}
			else
			{
				this._titleText.text = chat.title;
				LocalizedGameText.BindFontOnly(this._titleText);
			}
			LocalizedGameText.BindSource(this._subtitleText, "phone_story", chat.isGroup ? "Public group" : "Direct chat - tap to rename", null);
			OxGramVectorIcon component = this._root.Find("Header/GroupDetailsButton/Icon").GetComponent<OxGramVectorIcon>();
			component.symbol = (chat.isGroup ? OxGramVectorIcon.Symbol.Info : OxGramVectorIcon.Symbol.Edit);
			component.SetVerticesDirty();
			this._input.text = "";
			this.SetTyping(null);
			this.CloseGroupDetails();
			this.CloseSendConfirm();
			if (this._photoPicker != null)
			{
				this._photoPicker.gameObject.SetActive(false);
			}
			if (this._newMessages != null)
			{
				this._newMessages.gameObject.SetActive(false);
			}
			for (int i = this._content.childCount - 1; i >= 0; i--)
			{
				this._content.GetChild(i).gameObject.SetActive(false);
				OxGramChatView.DestroyViewObject(this._content.GetChild(i).gameObject);
			}
			TMP_Text tmp_Text = OxGramUI.LocalizedText("ConversationContext", this._content, chat.isGroup ? "GROUP CONVERSATION" : "CONVERSATION", this._theme.font, 11f, this._theme.textSecondary, TextAlignmentOptions.Center);
			tmp_Text.characterSpacing = 2f;
			tmp_Text.gameObject.AddComponent<LayoutElement>().preferredHeight = 30f;
			foreach (OxGramMessage oxGramMessage in chat.messages)
			{
				this.AddBubble(oxGramMessage, false);
			}
			this.ScrollToBottomNextFrame();
			if (Application.isPlaying)
			{
				this._life.OnChatOpened(chat);
			}
		}

		// Token: 0x06007034 RID: 28724 RVA: 0x002D712C File Offset: 0x002D532C
		public void AppendMessage(OxGramMessage message)
		{
			if (this._chat == null)
			{
				return;
			}
			bool flag = message.sender == OxGramSender.Me || this.IsNearBottom();
			float height = this._content.rect.height;
			Vector2 anchoredPosition = this._content.anchoredPosition;
			this._chat.messages.Add(message);
			Action<OxGramChat> chatChanged = this.ChatChanged;
			if (chatChanged != null)
			{
				chatChanged(this._chat);
			}
			this.AddBubble(message, flag);
			if (!flag)
			{
				if (this._scrollRoutine != null)
				{
					base.StopCoroutine(this._scrollRoutine);
					this._scrollRoutine = null;
				}
				Canvas.ForceUpdateCanvases();
				LayoutRebuilder.ForceRebuildLayoutImmediate(this._content);
				this._content.anchoredPosition = anchoredPosition - Vector2.up * (this._content.rect.height - height);
				this._newMessages.gameObject.SetActive(true);
			}
		}

		// Token: 0x06007035 RID: 28725 RVA: 0x002D7214 File Offset: 0x002D5414
		private void ShowGroupDetails()
		{
			if (this._chat == null || this._groupDetails != null)
			{
				return;
			}
			if (this._chat.isGroup)
			{
				this._groupDetails = OxGramUI.Rect("GroupDetails", this._root);
				OxGramUI.Stretch(this._groupDetails, 0f, 0f, 0f, 0f);
				this._groupDetails.SetAsLastSibling();
				Image image = OxGramUI.Image("Dim", this._groupDetails, new Color(0f, 0f, 0f, 0.65f), null);
				OxGramUI.Stretch(image.rectTransform, 0f, 0f, 0f, 0f);
				OxGramUI.Button(image.gameObject).onClick.AddListener(new UnityAction(this.CloseGroupDetails));
				Image image2 = OxGramUI.Image("Card", this._groupDetails, this._theme.headerColor, OxGramBubbleSprite.Body);
				image2.rectTransform.anchorMin = new Vector2(0f, 0.5f);
				image2.rectTransform.anchorMax = new Vector2(1f, 0.5f);
				image2.rectTransform.sizeDelta = new Vector2(-40f, 440f);
				Image image3 = OxGramUI.Image("Avatar", image2.transform, this._theme.AvatarColor(this._chat.title), this._theme.circleSprite);
				RectTransform rectTransform = image3.rectTransform;
				RectTransform rectTransform2 = image3.rectTransform;
				Vector2 vector = new Vector2(0.5f, 1f);
				rectTransform2.anchorMax = vector;
				rectTransform.anchorMin = vector;
				image3.rectTransform.pivot = new Vector2(0.5f, 1f);
				image3.rectTransform.anchoredPosition = new Vector2(0f, -24f);
				image3.rectTransform.sizeDelta = new Vector2(62f, 62f);
				TMP_Text tmp_Text = OxGramUI.Text("Initial", image3.transform, string.IsNullOrEmpty(this._chat.title) ? "?" : this._chat.title.Substring(0, 1).ToUpperInvariant(), (this._theme.fontBold != null) ? this._theme.fontBold : this._theme.font, 28f, Color.white, TextAlignmentOptions.Center);
				OxGramUI.Stretch(tmp_Text.rectTransform, 0f, 0f, 0f, 0f);
				tmp_Text.richText = false;
				TMP_Text tmp_Text2 = OxGramUI.LocalizedText("Title", image2.transform, this._chat.title, (this._theme.fontBold != null) ? this._theme.fontBold : this._theme.font, 23f, this._theme.textPrimary, TextAlignmentOptions.Center);
				tmp_Text2.rectTransform.anchorMin = new Vector2(0f, 1f);
				tmp_Text2.rectTransform.anchorMax = Vector2.one;
				tmp_Text2.rectTransform.pivot = new Vector2(0.5f, 1f);
				tmp_Text2.rectTransform.anchoredPosition = new Vector2(0f, -100f);
				tmp_Text2.rectTransform.sizeDelta = new Vector2(-40f, 42f);
				tmp_Text2.richText = false;
				tmp_Text2.enableAutoSizing = true;
				tmp_Text2.fontSizeMin = 18f;
				tmp_Text2.fontSizeMax = 23f;
				TMP_Text tmp_Text3 = OxGramUI.LocalizedText("Description", image2.transform, this._chat.subtitle, this._theme.font, 14f, this._theme.textSecondary, TextAlignmentOptions.Center);
				tmp_Text3.rectTransform.anchorMin = new Vector2(0f, 1f);
				tmp_Text3.rectTransform.anchorMax = Vector2.one;
				tmp_Text3.rectTransform.pivot = new Vector2(0.5f, 1f);
				tmp_Text3.rectTransform.anchoredPosition = new Vector2(0f, -148f);
				tmp_Text3.rectTransform.sizeDelta = new Vector2(-48f, 30f);
				tmp_Text3.richText = false;
				tmp_Text3.enableWordWrapping = false;
				tmp_Text3.overflowMode = TextOverflowModes.Ellipsis;
				TMP_Text tmp_Text4 = OxGramUI.LocalizedText("ParticipantsLabel", image2.transform, "IN THIS CONVERSATION", this._theme.font, 11f, this._theme.accent, TextAlignmentOptions.Center);
				tmp_Text4.characterSpacing = 1.5f;
				tmp_Text4.rectTransform.anchorMin = new Vector2(0f, 1f);
				tmp_Text4.rectTransform.anchorMax = Vector2.one;
				tmp_Text4.rectTransform.pivot = new Vector2(0.5f, 1f);
				tmp_Text4.rectTransform.anchoredPosition = new Vector2(0f, -194f);
				tmp_Text4.rectTransform.sizeDelta = new Vector2(-48f, 26f);
				string text = ((this._chat.members == null) ? "" : string.Join("  ·  ", this._chat.members));
				TMP_Text tmp_Text5 = OxGramUI.Text("Participants", image2.transform, text, this._theme.font, 17f, this._theme.textPrimary, TextAlignmentOptions.Top);
				OxGramUI.Stretch(tmp_Text5.rectTransform, 28f, 28f, 230f, 110f);
				tmp_Text5.richText = false;
				tmp_Text5.overflowMode = TextOverflowModes.Ellipsis;
				TMP_Text tmp_Text6 = OxGramUI.Text("YourAlias", image2.transform, "You appear as " + OxGramUser.Mention, this._theme.font, 13f, this._theme.textSecondary, TextAlignmentOptions.Center);
				LocalizedGameText.BindFormat(tmp_Text6, "phone_story", "You appear as {0}", new object[] { OxGramUser.Mention });
				tmp_Text6.rectTransform.anchorMin = Vector2.zero;
				tmp_Text6.rectTransform.anchorMax = new Vector2(1f, 0f);
				tmp_Text6.rectTransform.pivot = new Vector2(0.5f, 0f);
				tmp_Text6.rectTransform.anchoredPosition = new Vector2(0f, 80f);
				tmp_Text6.rectTransform.sizeDelta = new Vector2(-40f, 26f);
				tmp_Text6.richText = false;
				Image image4 = OxGramUI.Image("Done", image2.transform, this._theme.bubbleMine, OxGramBubbleSprite.Body);
				RectTransform rectTransform3 = image4.rectTransform;
				RectTransform rectTransform4 = image4.rectTransform;
				vector = new Vector2(0.5f, 0f);
				rectTransform4.anchorMax = vector;
				rectTransform3.anchorMin = vector;
				image4.rectTransform.pivot = new Vector2(0.5f, 0f);
				image4.rectTransform.anchoredPosition = new Vector2(0f, 22f);
				image4.rectTransform.sizeDelta = new Vector2(180f, 44f);
				OxGramUI.Stretch(OxGramUI.LocalizedText("Label", image4.transform, "Back to chat", this._theme.font, 16f, Color.white, TextAlignmentOptions.Center).rectTransform, 0f, 0f, 0f, 0f);
				OxGramUI.Button(image4.gameObject).onClick.AddListener(new UnityAction(this.CloseGroupDetails));
				return;
			}
			Action<OxGramChat> renameRequested = this.RenameRequested;
			if (renameRequested == null)
			{
				return;
			}
			renameRequested(this._chat);
		}

		// Token: 0x06007036 RID: 28726 RVA: 0x002D79AC File Offset: 0x002D5BAC
		private void CloseGroupDetails()
		{
			if (this._groupDetails == null)
			{
				return;
			}
			OxGramChatView.DestroyViewObject(this._groupDetails.gameObject);
			this._groupDetails = null;
		}

		// Token: 0x06007037 RID: 28727 RVA: 0x002D79D4 File Offset: 0x002D5BD4
		private void AddBubble(OxGramMessage m, bool scrollToBottom)
		{
			bool flag = m.sender == OxGramSender.Me;
			bool showAuthor = this._chat != null && this._chat.isGroup && !flag && !string.IsNullOrEmpty(m.authorName);
			RectTransform rectTransform = OxGramUI.Rect("Row", this._content);
			HorizontalLayoutGroup horizontalLayoutGroup = rectTransform.gameObject.AddComponent<HorizontalLayoutGroup>();
			horizontalLayoutGroup.childControlWidth = true;
			horizontalLayoutGroup.childControlHeight = true;
			horizontalLayoutGroup.childForceExpandWidth = false;
			horizontalLayoutGroup.childForceExpandHeight = false;
			horizontalLayoutGroup.childAlignment = (flag ? TextAnchor.LowerRight : TextAnchor.LowerLeft);
			horizontalLayoutGroup.spacing = 9f;
			rectTransform.gameObject.AddComponent<LayoutElement>().flexibleWidth = 1f;
			if (showAuthor)
			{
				Image image = OxGramUI.Image("Avatar", rectTransform, this._theme.AvatarColor(m.authorName), this._theme.circleSprite);
				LayoutElement layoutElement = image.gameObject.AddComponent<LayoutElement>();
				layoutElement.preferredWidth = 32f;
				layoutElement.preferredHeight = 32f;
				layoutElement.minWidth = 32f;
				OxGramUI.Stretch(OxGramUI.Text("Ini", image.transform, m.authorName.Substring(0, 1).ToUpper(), (this._theme.fontBold != null) ? this._theme.fontBold : this._theme.font, 16f, Color.white, TextAlignmentOptions.Center).rectTransform, 0f, 0f, 0f, 0f);
			}
			int padLeft = 14;
			int padRight = 14;
			float num = ((this._root.rect.width > 0f) ? (this._root.rect.width - 40f) : 440f);
			if (showAuthor)
			{
				num -= 32f + horizontalLayoutGroup.spacing;
			}
			float maxContent = Mathf.Min(330f, num) - (float)padLeft - (float)padRight;
			float num2 = 0f;
			string authorName = m.authorName;
			if (showAuthor)
			{
				num2 = Mathf.Max(num2, this.MeasureWidth(authorName, 14f, true, maxContent));
			}
			if (m.kind == OxGramMessageKind.Photo && m.photo != null)
			{
				num2 = maxContent;
			}
			else
			{
				num2 = Mathf.Max(num2, this.MeasureWidth(OxGramPresentation.MessageBody(m), 18f, false, maxContent) + 52f);
			}
			num2 = Mathf.Clamp(num2, 40f, maxContent);
			bool flag2 = m.kind == OxGramMessageKind.Photo && m.photo != null;
			Image image2 = OxGramUI.Image("Bubble", rectTransform, flag2 ? new Color(0f, 0f, 0f, 0f) : (flag ? this._theme.bubbleMine : this._theme.bubbleOther), flag2 ? null : OxGramBubbleSprite.Body);
			VerticalLayoutGroup verticalLayoutGroup = image2.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.childControlWidth = true;
			verticalLayoutGroup.childControlHeight = true;
			verticalLayoutGroup.childForceExpandWidth = true;
			verticalLayoutGroup.childForceExpandHeight = false;
			verticalLayoutGroup.padding = (flag2 ? new RectOffset(0, 0, 0, 0) : new RectOffset(padLeft, padRight, 10, 21));
			verticalLayoutGroup.spacing = 1f;
			ContentSizeFitter contentSizeFitter = image2.gameObject.AddComponent<ContentSizeFitter>();
			contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
			contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			LayoutElement bubbleLe = image2.gameObject.AddComponent<LayoutElement>();
			bubbleLe.preferredWidth = num2 + (float)padLeft + (float)padRight;
			bubbleLe.flexibleWidth = 0f;
			if (showAuthor)
			{
				TMP_Text tmp_Text = OxGramUI.Text("Author", image2.transform, authorName, (this._theme.fontBold != null) ? this._theme.fontBold : this._theme.font, 14f, this._theme.AvatarColor(authorName), TextAlignmentOptions.TopLeft);
				tmp_Text.richText = false;
				this.HeightOnly(tmp_Text);
			}
			if (flag2)
			{
				Image image3 = OxGramUI.Image("Photo", image2.transform, Color.white, null);
				image3.sprite = m.photo;
				StreamerCensorImage.Bind(image3, false);
				image3.type = Image.Type.Simple;
				image3.preserveAspect = false;
				Rect rect = m.photo.rect;
				float num3 = ((rect.width > 0f) ? (rect.height / rect.width) : 1f);
				float num4 = Mathf.Clamp(num2 * num3, num2 * 0.4f, num2 * 1.7f);
				image3.gameObject.AddComponent<LayoutElement>().preferredHeight = num4;
				Sprite sprite = m.photo;
				Button button = OxGramUI.Button(image3.gameObject);
				button.transition = Selectable.Transition.None;
				button.onClick.AddListener(delegate
				{
					PhotoPolaroidViewer.Show(sprite);
				});
			}
			else
			{
				TMP_Text tmp_Text2 = (flag ? OxGramUI.Text("Body", image2.transform, m.text, this._theme.font, 18f, Color.white, TextAlignmentOptions.TopLeft) : OxGramUI.Text("Body", image2.transform, m.text, this._theme.font, 18f, this._theme.textPrimary, TextAlignmentOptions.TopLeft));
				LocalizedGameText.BindComputed(tmp_Text2, delegate
				{
					string text = OxGramPresentation.MessageBody(m);
					float num5 = Mathf.Max(showAuthor ? this.MeasureWidth(authorName, 14f, true, maxContent) : 0f, this.MeasureWidth(text, 18f, false, maxContent) + 52f);
					bubbleLe.preferredWidth = Mathf.Clamp(num5, 40f, maxContent) + (float)padLeft + (float)padRight;
					return text;
				});
				tmp_Text2.richText = false;
				tmp_Text2.lineSpacing = 3f;
				this.HeightOnly(tmp_Text2);
			}
			if (!flag2)
			{
				this.AddCornerTime(image2, m, flag, padRight);
			}
			if (scrollToBottom)
			{
				this.ScrollToBottomNextFrame();
			}
		}

		// Token: 0x06007038 RID: 28728 RVA: 0x002D8018 File Offset: 0x002D6218
		private void AddCornerTime(Image bubble, OxGramMessage m, bool mine, int padRight)
		{
			if (string.IsNullOrEmpty(m.time))
			{
				return;
			}
			TMP_Text tmp_Text = OxGramUI.Text("Time", bubble.transform, m.time, this._theme.font, 11f, mine ? new Color(1f, 1f, 1f, 0.65f) : this._theme.textSecondary, TextAlignmentOptions.BottomRight);
			tmp_Text.gameObject.AddComponent<LayoutElement>().ignoreLayout = true;
			RectTransform rectTransform = tmp_Text.rectTransform;
			rectTransform.anchorMin = new Vector2(1f, 0f);
			rectTransform.anchorMax = new Vector2(1f, 0f);
			rectTransform.pivot = new Vector2(1f, 0f);
			rectTransform.sizeDelta = new Vector2(52f, 15f);
			rectTransform.anchoredPosition = new Vector2((float)(-(float)padRight) + 2f, 8f);
		}

		// Token: 0x06007039 RID: 28729 RVA: 0x002D810C File Offset: 0x002D630C
		private string BodyWithInlineTime(OxGramMessage m, bool mine)
		{
			if (string.IsNullOrEmpty(m.time))
			{
				return m.text;
			}
			string text = (mine ? "C8E6D8" : "9AA6B2");
			string text2 = string.Format("<size={0}><color=#{1}>\u2002\u2002{2}</color></size>", 11f, text, m.time);
			return m.text + text2;
		}

		// Token: 0x0600703A RID: 28730 RVA: 0x002D8168 File Offset: 0x002D6368
		private void AddTimeRow(Transform bubble, OxGramMessage m, bool mine)
		{
			if (string.IsNullOrEmpty(m.time))
			{
				return;
			}
			string time = m.time;
			OxGramUI.Text("Time", bubble, time, this._theme.font, 11f, mine ? new Color(1f, 1f, 1f, 0.6f) : this._theme.textSecondary, TextAlignmentOptions.BottomRight).gameObject.AddComponent<LayoutElement>().preferredHeight = 15f;
		}

		// Token: 0x0600703B RID: 28731 RVA: 0x002D81E8 File Offset: 0x002D63E8
		private float MeasureWidth(string text, float fontSize, bool bold, float maxWidth)
		{
			TMP_Text tmp_Text = ((this._measureProbe != null) ? this._measureProbe : (this._measureProbe = this.MakeProbe()));
			tmp_Text.font = GameLocalization.GetFont((bold && this._theme.fontBold != null) ? this._theme.fontBold : this._theme.font);
			tmp_Text.fontSize = fontSize;
			tmp_Text.text = text;
			float x = tmp_Text.GetPreferredValues(text, 100000f, 0f).x;
			return Mathf.Min(maxWidth, x);
		}

		// Token: 0x0600703C RID: 28732 RVA: 0x002D8280 File Offset: 0x002D6480
		private TMP_Text MakeProbe()
		{
			GameObject gameObject = new GameObject("OxGramMeasure", new Type[] { typeof(RectTransform) });
			gameObject.transform.SetParent(base.transform, false);
			TextMeshProUGUI textMeshProUGUI = gameObject.AddComponent<TextMeshProUGUI>();
			textMeshProUGUI.enableWordWrapping = false;
			textMeshProUGUI.richText = false;
			textMeshProUGUI.raycastTarget = false;
			textMeshProUGUI.color = new Color(0f, 0f, 0f, 0f);
			((RectTransform)gameObject.transform).sizeDelta = new Vector2(1f, 1f);
			return textMeshProUGUI;
		}

		// Token: 0x0600703D RID: 28733 RVA: 0x002D8316 File Offset: 0x002D6516
		private void HeightOnly(TMP_Text t)
		{
			t.enableWordWrapping = true;
			ContentSizeFitter contentSizeFitter = t.gameObject.AddComponent<ContentSizeFitter>();
			contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
			contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
		}

		// Token: 0x0600703E RID: 28734 RVA: 0x002D8338 File Offset: 0x002D6538
		private void OnSend()
		{
			if (this._input == null || string.IsNullOrWhiteSpace(this._input.text))
			{
				return;
			}
			OxGramMessage oxGramMessage = OxGramMessage.Text(OxGramSender.Me, OxGramUser.DisplayName, this._input.text.Trim(), DateTime.Now.ToString("HH:mm"));
			this.AppendMessage(oxGramMessage);
			this._input.text = "";
			this._input.ActivateInputField();
		}

		// Token: 0x0600703F RID: 28735 RVA: 0x002D83B8 File Offset: 0x002D65B8
		private void OnAttachPhoto()
		{
			if (this._photoPicker != null)
			{
				OxGramChatView.DestroyViewObject(this._photoPicker.gameObject);
			}
			OxGramGallery.Reload();
			this.BuildPhotoPicker();
			this._photoPicker.gameObject.SetActive(true);
			this._photoPicker.SetAsLastSibling();
			OxGramGallery.DecodeWhileOpen(this._photoPicker.gameObject);
		}

		// Token: 0x06007040 RID: 28736 RVA: 0x002D841C File Offset: 0x002D661C
		private void BuildPhotoPicker()
		{
			this._photoPicker = OxGramUI.Rect("PhotoPicker", this._root);
			OxGramUI.Stretch(this._photoPicker, 0f, 0f, 0f, 0f);
			Image image = OxGramUI.Image("Dim", this._photoPicker, new Color(0f, 0f, 0f, 0.55f), null);
			OxGramUI.Stretch(image.rectTransform, 0f, 0f, 0f, 0f);
			OxGramUI.Button(image.gameObject).onClick.AddListener(delegate
			{
				this._photoPicker.gameObject.SetActive(false);
			});
			Image image2 = OxGramUI.Image("Sheet", this._photoPicker, this._theme.headerColor, OxGramBubbleSprite.Body);
			image2.rectTransform.anchorMin = new Vector2(0f, 0f);
			image2.rectTransform.anchorMax = new Vector2(1f, 0f);
			image2.rectTransform.pivot = new Vector2(0.5f, 0f);
			image2.rectTransform.sizeDelta = new Vector2(-32f, 440f);
			image2.rectTransform.anchoredPosition = new Vector2(0f, 24f);
			TMP_Text tmp_Text = OxGramUI.LocalizedText("PickTitle", image2.transform, "Send a photo", (this._theme.fontBold != null) ? this._theme.fontBold : this._theme.font, 22f, this._theme.textPrimary, TextAlignmentOptions.Center);
			tmp_Text.rectTransform.anchorMin = new Vector2(0f, 1f);
			tmp_Text.rectTransform.anchorMax = new Vector2(1f, 1f);
			tmp_Text.rectTransform.pivot = new Vector2(0.5f, 1f);
			tmp_Text.rectTransform.sizeDelta = new Vector2(0f, 40f);
			tmp_Text.rectTransform.anchoredPosition = new Vector2(0f, -10f);
			TMP_Text tmp_Text2 = OxGramUI.Text("Identity", image2.transform, "Share as " + OxGramUser.Mention, this._theme.font, 13f, this._theme.textSecondary, TextAlignmentOptions.Center);
			LocalizedGameText.BindFormat(tmp_Text2, "phone_story", "Share as {0}", new object[] { OxGramUser.Mention });
			tmp_Text2.rectTransform.anchorMin = new Vector2(0f, 1f);
			tmp_Text2.rectTransform.anchorMax = Vector2.one;
			tmp_Text2.rectTransform.pivot = new Vector2(0.5f, 1f);
			tmp_Text2.rectTransform.sizeDelta = new Vector2(-32f, 24f);
			tmp_Text2.rectTransform.anchoredPosition = new Vector2(0f, -48f);
			tmp_Text2.richText = false;
			RectTransform rectTransform = OxGramUI.Rect("PickBody", image2.transform);
			OxGramUI.Stretch(rectTransform, 16f, 16f, 86f, 54f);
			ScrollRect scrollRect = rectTransform.gameObject.AddComponent<ScrollRect>();
			scrollRect.horizontal = false;
			scrollRect.vertical = true;
			scrollRect.movementType = ScrollRect.MovementType.Clamped;
			scrollRect.scrollSensitivity = 24f;
			Image image3 = OxGramUI.Image("PickViewport", rectTransform, new Color(0f, 0f, 0f, 0f), null);
			OxGramUI.Stretch(image3.rectTransform, 0f, 0f, 0f, 0f);
			image3.gameObject.AddComponent<RectMask2D>();
			scrollRect.viewport = image3.rectTransform;
			RectTransform rectTransform2 = OxGramUI.Rect("PickContent", image3.transform);
			rectTransform2.anchorMin = new Vector2(0f, 1f);
			rectTransform2.anchorMax = new Vector2(1f, 1f);
			rectTransform2.pivot = new Vector2(0.5f, 1f);
			rectTransform2.sizeDelta = new Vector2(0f, 0f);
			GridLayoutGroup gridLayoutGroup = rectTransform2.gameObject.AddComponent<GridLayoutGroup>();
			float num = Mathf.Max(64f, (this._root.rect.width - 92f) / 3f);
			gridLayoutGroup.cellSize = new Vector2(num, num);
			gridLayoutGroup.spacing = new Vector2(8f, 8f);
			gridLayoutGroup.padding = new RectOffset(4, 4, 4, 4);
			gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
			gridLayoutGroup.constraintCount = 3;
			rectTransform2.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			scrollRect.content = rectTransform2;
			List<Sprite> photos = OxGramGallery.Photos;
			if (photos.Count == 0)
			{
				TMP_Text tmp_Text3 = OxGramUI.LocalizedText("Empty", rectTransform, "Your gallery is empty.\nTake a photo with the camera to share it here.", this._theme.font, 17f, this._theme.textSecondary, TextAlignmentOptions.Center);
				OxGramUI.Stretch(tmp_Text3.rectTransform, 18f, 18f, 0f, 0f);
				tmp_Text3.raycastTarget = false;
			}
			else
			{
				foreach (Sprite sprite in photos)
				{
					Image image4 = OxGramUI.Image("Thumb", rectTransform2, new Color(0f, 0f, 0f, 0.25f), null);
					image4.gameObject.AddComponent<RectMask2D>();
					Image image5 = OxGramUI.Image("Img", image4.transform, Color.white, null);
					image5.sprite = sprite;
					StreamerCensorImage.Bind(image5, false);
					image5.preserveAspect = true;
					image5.raycastTarget = false;
					Rect rect = sprite.rect;
					float num2 = ((rect.height > 0f) ? (rect.width / rect.height) : 1f);
					RectTransform rectTransform3 = image5.rectTransform;
					rectTransform3.anchorMin = new Vector2(0.5f, 0.5f);
					rectTransform3.anchorMax = new Vector2(0.5f, 0.5f);
					rectTransform3.pivot = new Vector2(0.5f, 0.5f);
					rectTransform3.anchoredPosition = Vector2.zero;
					float num3 = num;
					if (num2 >= 1f)
					{
						rectTransform3.sizeDelta = new Vector2(num3 * num2, num3);
					}
					else
					{
						rectTransform3.sizeDelta = new Vector2(num3, num3 / num2);
					}
					Sprite captured = sprite;
					OxGramUI.Button(image4.gameObject).onClick.AddListener(delegate
					{
						this.ShowSendConfirm(captured);
					});
				}
			}
			Image image6 = OxGramUI.Image("Cancel", image2.transform, Color.clear, null);
			image6.rectTransform.anchorMin = Vector2.zero;
			image6.rectTransform.anchorMax = new Vector2(1f, 0f);
			image6.rectTransform.pivot = new Vector2(0.5f, 0f);
			image6.rectTransform.sizeDelta = new Vector2(-32f, 44f);
			image6.rectTransform.anchoredPosition = new Vector2(0f, 4f);
			TMP_Text tmp_Text4 = OxGramUI.LocalizedText("Label", image6.transform, "Cancel", this._theme.font, 16f, this._theme.accent, TextAlignmentOptions.Center);
			OxGramUI.Stretch(tmp_Text4.rectTransform, 0f, 0f, 0f, 0f);
			tmp_Text4.raycastTarget = false;
			OxGramUI.Button(image6.gameObject).onClick.AddListener(delegate
			{
				this._photoPicker.gameObject.SetActive(false);
			});
		}

		// Token: 0x06007041 RID: 28737 RVA: 0x002D8BFC File Offset: 0x002D6DFC
		private void ShowSendConfirm(Sprite original)
		{
			if (original == null)
			{
				return;
			}
			if (this._sendConfirm != null)
			{
				OxGramChatView.DestroyViewObject(this._sendConfirm.gameObject);
			}
			this._selectedPhoto = original;
			this._blurFace = true;
			this._sendConfirm = OxGramUI.Rect("SendConfirm", this._root);
			OxGramUI.Stretch(this._sendConfirm, 0f, 0f, 0f, 0f);
			this._sendConfirm.SetAsLastSibling();
			Image image = OxGramUI.Image("Dim", this._sendConfirm, new Color(0f, 0f, 0f, 0.6f), null);
			OxGramUI.Stretch(image.rectTransform, 0f, 0f, 0f, 0f);
			OxGramUI.Button(image.gameObject).onClick.AddListener(new UnityAction(this.CloseSendConfirm));
			Image image2 = OxGramUI.Image("Card", this._sendConfirm, this._theme.headerColor, OxGramBubbleSprite.Body);
			image2.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
			image2.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
			image2.rectTransform.pivot = new Vector2(0.5f, 0.5f);
			image2.rectTransform.sizeDelta = new Vector2(400f, 610f);
			TMP_Text tmp_Text = OxGramUI.Text("SendingAs", image2.transform, "Send as " + OxGramUser.Mention, (this._theme.fontBold != null) ? this._theme.fontBold : this._theme.font, 17f, this._theme.textPrimary, TextAlignmentOptions.Center);
			LocalizedGameText.BindFormat(tmp_Text, "phone_story", "Send as {0}", new object[] { OxGramUser.Mention });
			tmp_Text.rectTransform.anchorMin = new Vector2(0f, 1f);
			tmp_Text.rectTransform.anchorMax = Vector2.one;
			tmp_Text.rectTransform.pivot = new Vector2(0.5f, 1f);
			tmp_Text.rectTransform.sizeDelta = new Vector2(-32f, 34f);
			tmp_Text.rectTransform.anchoredPosition = new Vector2(0f, -12f);
			tmp_Text.richText = false;
			tmp_Text.enableWordWrapping = false;
			tmp_Text.overflowMode = TextOverflowModes.Ellipsis;
			Image image3 = OxGramUI.Image("Preview", image2.transform, Color.white, null);
			this._photoPreview = image3;
			image3.preserveAspect = true;
			image3.rectTransform.anchorMin = new Vector2(0.5f, 1f);
			image3.rectTransform.anchorMax = new Vector2(0.5f, 1f);
			image3.rectTransform.pivot = new Vector2(0.5f, 1f);
			image3.rectTransform.sizeDelta = new Vector2(352f, 248f);
			image3.rectTransform.anchoredPosition = new Vector2(0f, -54f);
			Image image4 = OxGramUI.Image("FaceBlur", image2.transform, this._theme.bubbleOther, OxGramBubbleSprite.Body);
			RectTransform rectTransform = image4.rectTransform;
			RectTransform rectTransform2 = image4.rectTransform;
			Vector2 vector = new Vector2(0.5f, 1f);
			rectTransform2.anchorMax = vector;
			rectTransform.anchorMin = vector;
			image4.rectTransform.pivot = new Vector2(0.5f, 1f);
			image4.rectTransform.anchoredPosition = new Vector2(0f, -320f);
			image4.rectTransform.sizeDelta = new Vector2(352f, 62f);
			Image image5 = OxGramUI.Image("Checkbox", image4.transform, this._theme.accent, OxGramBubbleSprite.Body);
			RectTransform rectTransform3 = image5.rectTransform;
			RectTransform rectTransform4 = image5.rectTransform;
			vector = new Vector2(0f, 0.5f);
			rectTransform4.anchorMax = vector;
			rectTransform3.anchorMin = vector;
			image5.rectTransform.anchoredPosition = new Vector2(30f, 0f);
			image5.rectTransform.sizeDelta = new Vector2(28f, 28f);
			RectTransform rectTransform5 = OxGramUI.Rect("Check", image5.transform);
			OxGramUI.Stretch(rectTransform5, 3f, 3f, 3f, 3f);
			OxGramVectorIcon oxGramVectorIcon = rectTransform5.gameObject.AddComponent<OxGramVectorIcon>();
			oxGramVectorIcon.symbol = OxGramVectorIcon.Symbol.Check;
			oxGramVectorIcon.color = Color.white;
			oxGramVectorIcon.raycastTarget = false;
			TMP_Text tmp_Text2 = OxGramUI.LocalizedText("Label", image4.transform, "Auto face blur", (this._theme.fontBold != null) ? this._theme.fontBold : this._theme.font, 18f, this._theme.textPrimary, TextAlignmentOptions.Left);
			OxGramUI.Stretch(tmp_Text2.rectTransform, 56f, 12f, 0f, 0f);
			tmp_Text2.raycastTarget = false;
			this._faceBlurToggle = image4.gameObject.AddComponent<Toggle>();
			this._faceBlurToggle.targetGraphic = image5;
			this._faceBlurToggle.graphic = oxGramVectorIcon;
			this._faceBlurToggle.toggleTransition = Toggle.ToggleTransition.None;
			this._faceBlurToggle.onValueChanged.AddListener(delegate(bool value)
			{
				this._blurFace = !OxGramPhotoPrivacy.CanSendUncensored || value;
				this.RefreshPhotoPrivacy();
			});
			this._privacyHint = OxGramUI.Text("PrivacyHint", image2.transform, "", this._theme.font, 15f, this._theme.textSecondary, TextAlignmentOptions.Top);
			this._privacyHint.rectTransform.anchorMin = new Vector2(0f, 1f);
			this._privacyHint.rectTransform.anchorMax = Vector2.one;
			this._privacyHint.rectTransform.pivot = new Vector2(0.5f, 1f);
			this._privacyHint.rectTransform.sizeDelta = new Vector2(-48f, 100f);
			this._privacyHint.rectTransform.anchoredPosition = new Vector2(0f, -396f);
			this._rewardHint = OxGramUI.Text("RewardHint", image2.transform, "", this._theme.font, 14f, this._theme.authorAccent, TextAlignmentOptions.Center);
			this._rewardHint.rectTransform.anchorMin = new Vector2(0f, 1f);
			this._rewardHint.rectTransform.anchorMax = Vector2.one;
			this._rewardHint.rectTransform.pivot = new Vector2(0.5f, 1f);
			this._rewardHint.rectTransform.sizeDelta = new Vector2(-48f, 38f);
			this._rewardHint.rectTransform.anchoredPosition = new Vector2(0f, -502f);
			this.RefreshPhotoPrivacy();
			this.BuildConfirmButton(image2.transform, "Cancel", new Vector2(0.5f, -0.5f), new Color(1f, 1f, 1f, 0.1f), this._theme.textPrimary, new UnityAction(this.CloseSendConfirm));
			this.BuildConfirmButton(image2.transform, "Send", new Vector2(0.5f, 0.5f), this._theme.accent, Color.white, delegate
			{
				this.SendPhoto(original);
			});
		}

		// Token: 0x06007042 RID: 28738 RVA: 0x002D93BC File Offset: 0x002D75BC
		private void RefreshPhotoPrivacy()
		{
			if (!OxGramPhotoPrivacy.CanSendUncensored)
			{
				this._blurFace = true;
			}
			if (this._faceBlurToggle == null || this._sendConfirm == null || this._selectedPhoto == null)
			{
				return;
			}
			bool canSendUncensored = OxGramPhotoPrivacy.CanSendUncensored;
			this._faceBlurToggle.SetIsOnWithoutNotify(this._blurFace);
			this._faceBlurToggle.interactable = canSendUncensored;
			StreamerCensorImage.Bind(this._photoPreview, false);
			this._photoPreview.sprite = (this._blurFace ? OxGramGallery.CensoredSiblingOf(this._selectedPhoto, true) : OxGramGallery.OriginalOf(this._selectedPhoto));
			string text = (canSendUncensored ? (this._blurFace ? "Face blur is on. You can turn it off.\nSending photos cannot end your run." : "Face blur is off. The original photo will be sent.\nSending photos cannot end your run.") : (OxGramPhotoPrivacy.CanLeakInCurrentScene ? "Required until Certified Exhibitionist.\nA client glitch can reveal your face and end the run." : "Required until Certified Exhibitionist.\nYour face stays protected outside a run."));
			if (this._blurFace && !OxGramGallery.HasCensoredSibling(this._selectedPhoto))
			{
				text = "Face blur is unavailable for this photo.\nA privacy cover will be sent instead.";
			}
			LocalizedGameText.BindSource(this._privacyHint, "phone_story", text, null);
			if (canSendUncensored)
			{
				LocalizedGameText.BindSource(this._rewardHint, "phone_story", "Certified Exhibitionist: photos give no points.", null);
			}
			else if (OxGramPhotoPrivacy.IsInActiveRun && OxGramPhotoPrivacy.RewardsPhotos)
			{
				LocalizedGameText.BindFormat(this._rewardHint, "phone_story", "+{0} points per photo sent during a run", new object[] { 5 });
			}
			else
			{
				LocalizedGameText.BindSource(this._rewardHint, "phone_story", "Photo points are earned during a run.", null);
			}
			this._privacyHint.GetComponent<LocalizedGameText>().FitTranslationToRectangle(false);
			this._rewardHint.GetComponent<LocalizedGameText>().FitTranslationToRectangle(false);
		}

		// Token: 0x06007043 RID: 28739 RVA: 0x002D9544 File Offset: 0x002D7744
		private void BuildConfirmButton(Transform parent, string label, Vector2 sidePivot, Color bg, Color fg, UnityAction onClick)
		{
			Image image = OxGramUI.Image(label, parent, bg, OxGramBubbleSprite.Body);
			image.rectTransform.anchorMin = new Vector2(0.5f, 0f);
			image.rectTransform.anchorMax = new Vector2(0.5f, 0f);
			image.rectTransform.pivot = new Vector2(0.5f, 0f);
			image.rectTransform.sizeDelta = new Vector2(128f, 44f);
			image.rectTransform.anchoredPosition = new Vector2(sidePivot.y * 140f, 16f);
			OxGramUI.Stretch(OxGramUI.LocalizedText("Label", image.transform, label, (this._theme.fontBold != null) ? this._theme.fontBold : this._theme.font, 16f, fg, TextAlignmentOptions.Center).rectTransform, 0f, 0f, 0f, 0f);
			OxGramUI.Button(image.gameObject).onClick.AddListener(onClick);
		}

		// Token: 0x06007044 RID: 28740 RVA: 0x002D9668 File Offset: 0x002D7868
		private void CloseSendConfirm()
		{
			if (this._sendConfirm != null)
			{
				OxGramChatView.DestroyViewObject(this._sendConfirm.gameObject);
				this._sendConfirm = null;
			}
			this._selectedPhoto = null;
			this._faceBlurToggle = null;
			this._photoPreview = null;
			this._privacyHint = null;
			this._rewardHint = null;
		}

		// Token: 0x06007045 RID: 28741 RVA: 0x002D96C0 File Offset: 0x002D78C0
		private void SendPhoto(Sprite original)
		{
			if (original == null || this._chat == null)
			{
				return;
			}
			bool flag = !OxGramPhotoPrivacy.CanSendUncensored || this._blurFace;
			this.CloseSendConfirm();
			if (this._photoPicker != null)
			{
				this._photoPicker.gameObject.SetActive(false);
			}
			if (!OxGramPhotoPrivacy.CanLeakInCurrentScene)
			{
				CensorLeakState.Reset();
				if (flag)
				{
					this.SendProtectedPhoto(original);
					return;
				}
				this.SendNormalPhoto(OxGramGallery.OriginalOf(original));
				return;
			}
			else
			{
				bool flag2 = OxGramPhotoTag.ShowsFace(original.name);
				bool flag3 = OxGramGallery.HasCensoredSibling(original);
				bool canLeakInCurrentScene = OxGramPhotoPrivacy.CanLeakInCurrentScene;
				float num = ((canLeakInCurrentScene && flag2 && flag3 && this._censorBugChance > 0f) ? Random.Range(0f, 100f) : 100f);
				if (OxGramPhotoPrivacy.ShouldLeak(canLeakInCurrentScene, flag2, flag3, this._censorBugChance, num))
				{
					CensorLeakState.GameOverPending = true;
					this.ShowCensorErrorPopup(delegate
					{
						this.LeakUncensored(original);
					});
					return;
				}
				this.SendProtectedPhoto(original);
				return;
			}
		}

		// Token: 0x06007046 RID: 28742 RVA: 0x002D97E0 File Offset: 0x002D79E0
		private void SendProtectedPhoto(Sprite original)
		{
			this.SendNormalPhoto(OxGramGallery.CensoredSiblingOf(original, true));
		}

		// Token: 0x06007047 RID: 28743 RVA: 0x002D97F0 File Offset: 0x002D79F0
		private void SendNormalPhoto(Sprite toSend)
		{
			if (toSend == null || this._chat == null)
			{
				return;
			}
			OxGramMessage oxGramMessage = OxGramMessage.Photo(OxGramSender.Me, OxGramUser.DisplayName, toSend, DateTime.Now.ToString("HH:mm"));
			this.AppendMessage(oxGramMessage);
			AchievementTrackingService.RecordGroupPhoto(this._chat != null && this._chat.isGroup, oxGramMessage.photo != null);
			OxGramPhotoPrivacy.AwardPhotoPoints();
			if (this._life != null)
			{
				this._life.ReactToPhoto(oxGramMessage);
			}
		}

		// Token: 0x06007048 RID: 28744 RVA: 0x002D987C File Offset: 0x002D7A7C
		private void LeakUncensored(Sprite original)
		{
			if (original == null || this._chat == null)
			{
				return;
			}
			if (!OxGramPhotoPrivacy.CanLeakInCurrentScene)
			{
				CensorLeakState.Reset();
				this.SendProtectedPhoto(original);
				return;
			}
			OxGramMessage oxGramMessage = OxGramMessage.Photo(OxGramSender.Me, OxGramUser.DisplayName, OxGramGallery.OriginalOf(original), DateTime.Now.ToString("HH:mm"));
			this.AppendMessage(oxGramMessage);
			AchievementTrackingService.RecordGroupPhoto(this._chat != null && this._chat.isGroup, oxGramMessage.photo != null);
			if (this._life != null)
			{
				this._life.ReactToExposure(oxGramMessage, OxGramUser.DisplayName);
			}
			CensorLeakState.GameOverPending = true;
		}

		// Token: 0x06007049 RID: 28745 RVA: 0x002D9928 File Offset: 0x002D7B28
		private void ShowCensorErrorPopup(UnityAction onAcknowledged)
		{
			if (this._errorPopup != null)
			{
				OxGramChatView.DestroyViewObject(this._errorPopup.gameObject);
			}
			if (this._theme.errorSound != null)
			{
				if (this._errorAudio == null)
				{
					this._errorAudio = base.gameObject.AddComponent<AudioSource>();
					this._errorAudio.playOnAwake = false;
					this._errorAudio.spatialBlend = 0f;
					this._errorAudio.ignoreListenerPause = true;
				}
				this._errorAudio.PlayOneShot(this._theme.errorSound);
			}
			this._errorPopup = OxGramUI.Rect("CensorError", this._root);
			OxGramUI.Stretch(this._errorPopup, 0f, 0f, 0f, 0f);
			this._errorPopup.SetAsLastSibling();
			Image image = OxGramUI.Image("Dim", this._errorPopup, new Color(0f, 0f, 0f, 0.78f), null);
			OxGramUI.Stretch(image.rectTransform, 0f, 0f, 0f, 0f);
			OxGramUI.Button(image.gameObject);
			Image image2 = OxGramUI.Image("Card", this._errorPopup, new Color(0.16f, 0.06f, 0.06f, 1f), OxGramBubbleSprite.Body);
			image2.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
			image2.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
			image2.rectTransform.pivot = new Vector2(0.5f, 0.5f);
			image2.rectTransform.sizeDelta = new Vector2(300f, 300f);
			Color color = new Color(0.92f, 0.26f, 0.26f, 1f);
			if (this._theme.errorIcon != null)
			{
				Image image3 = OxGramUI.Image("Icon", image2.transform, color, null);
				image3.sprite = this._theme.errorIcon;
				image3.preserveAspect = true;
				image3.rectTransform.anchorMin = new Vector2(0.5f, 1f);
				image3.rectTransform.anchorMax = new Vector2(0.5f, 1f);
				image3.rectTransform.pivot = new Vector2(0.5f, 1f);
				image3.rectTransform.sizeDelta = new Vector2(72f, 72f);
				image3.rectTransform.anchoredPosition = new Vector2(0f, -18f);
			}
			else
			{
				TMP_Text tmp_Text = OxGramUI.Text("Icon", image2.transform, "âš\u00a0", (this._theme.fontBold != null) ? this._theme.fontBold : this._theme.font, 56f, color, TextAlignmentOptions.Center);
				tmp_Text.rectTransform.anchorMin = new Vector2(0.5f, 1f);
				tmp_Text.rectTransform.anchorMax = new Vector2(0.5f, 1f);
				tmp_Text.rectTransform.pivot = new Vector2(0.5f, 1f);
				tmp_Text.rectTransform.sizeDelta = new Vector2(80f, 80f);
				tmp_Text.rectTransform.anchoredPosition = new Vector2(0f, -18f);
			}
			TMP_Text tmp_Text2 = OxGramUI.LocalizedText("Title", image2.transform, "CRITICAL ERROR", (this._theme.fontBold != null) ? this._theme.fontBold : this._theme.font, 22f, color, TextAlignmentOptions.Center);
			tmp_Text2.rectTransform.anchorMin = new Vector2(0f, 1f);
			tmp_Text2.rectTransform.anchorMax = new Vector2(1f, 1f);
			tmp_Text2.rectTransform.pivot = new Vector2(0.5f, 1f);
			tmp_Text2.rectTransform.sizeDelta = new Vector2(-24f, 32f);
			tmp_Text2.rectTransform.anchoredPosition = new Vector2(0f, -104f);
			TMP_Text tmp_Text3 = OxGramUI.LocalizedText("Body", image2.transform, "The face censorship process failed unexpectedly.\nThe photo was sent <b>without censoring</b>.", this._theme.font, 15f, this._theme.textPrimary, TextAlignmentOptions.Center);
			tmp_Text3.rectTransform.anchorMin = new Vector2(0f, 1f);
			tmp_Text3.rectTransform.anchorMax = new Vector2(1f, 1f);
			tmp_Text3.rectTransform.pivot = new Vector2(0.5f, 1f);
			tmp_Text3.rectTransform.sizeDelta = new Vector2(-32f, 80f);
			tmp_Text3.rectTransform.anchoredPosition = new Vector2(0f, -148f);
			Image image4 = OxGramUI.Image("Ok", image2.transform, color, OxGramBubbleSprite.Body);
			image4.rectTransform.anchorMin = new Vector2(0.5f, 0f);
			image4.rectTransform.anchorMax = new Vector2(0.5f, 0f);
			image4.rectTransform.pivot = new Vector2(0.5f, 0f);
			image4.rectTransform.sizeDelta = new Vector2(180f, 44f);
			image4.rectTransform.anchoredPosition = new Vector2(0f, 20f);
			OxGramUI.Stretch(OxGramUI.LocalizedText("Label", image4.transform, "Dismiss", (this._theme.fontBold != null) ? this._theme.fontBold : this._theme.font, 16f, Color.white, TextAlignmentOptions.Center).rectTransform, 0f, 0f, 0f, 0f);
			OxGramUI.Button(image4.gameObject).onClick.AddListener(delegate
			{
				this.CloseErrorPopup();
				UnityAction onAcknowledged2 = onAcknowledged;
				if (onAcknowledged2 == null)
				{
					return;
				}
				onAcknowledged2();
			});
		}

		// Token: 0x0600704A RID: 28746 RVA: 0x002D9F5B File Offset: 0x002D815B
		private void CloseErrorPopup()
		{
			if (this._errorPopup != null)
			{
				OxGramChatView.DestroyViewObject(this._errorPopup.gameObject);
				this._errorPopup = null;
			}
		}

		// Token: 0x0600704B RID: 28747 RVA: 0x00027EE2 File Offset: 0x000260E2
		private static void DestroyViewObject(GameObject target)
		{
			if (Application.isPlaying)
			{
				Object.Destroy(target);
				return;
			}
			Object.DestroyImmediate(target);
		}

		// Token: 0x0600704C RID: 28748 RVA: 0x002D9F84 File Offset: 0x002D8184
		private bool IsNearBottom()
		{
			return this._scroll == null || this._content == null || this._scroll.viewport == null || this._content.rect.height <= this._scroll.viewport.rect.height + 2f || this._scroll.verticalNormalizedPosition <= 0.04f;
		}

		// Token: 0x0600704D RID: 28749 RVA: 0x002DA00C File Offset: 0x002D820C
		private void ScrollToBottomNextFrame()
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (!Application.isPlaying)
			{
				Canvas.ForceUpdateCanvases();
				LayoutRebuilder.ForceRebuildLayoutImmediate(this._content);
				this._scroll.StopMovement();
				this._scroll.verticalNormalizedPosition = 0f;
				if (this._newMessages != null)
				{
					this._newMessages.gameObject.SetActive(false);
				}
				return;
			}
			if (this._scrollRoutine != null)
			{
				base.StopCoroutine(this._scrollRoutine);
			}
			this._scrollRoutine = base.StartCoroutine(this.ScrollToBottomCo());
		}

		// Token: 0x0600704E RID: 28750 RVA: 0x002DA09A File Offset: 0x002D829A
		private IEnumerator ScrollToBottomCo()
		{
			yield return null;
			Canvas.ForceUpdateCanvases();
			if (this._scroll != null)
			{
				this._scroll.StopMovement();
				this._scroll.verticalNormalizedPosition = 0f;
			}
			if (this._newMessages != null)
			{
				this._newMessages.gameObject.SetActive(false);
			}
			this._scrollRoutine = null;
			yield break;
		}

		// Token: 0x040072E5 RID: 29413
		private OxGramTheme _theme;

		// Token: 0x040072E6 RID: 29414
		private Action _onBack;

		// Token: 0x040072E7 RID: 29415
		private Action _onClose;

		// Token: 0x040072E8 RID: 29416
		private RectTransform _root;

		// Token: 0x040072E9 RID: 29417
		private TMP_Text _titleText;

		// Token: 0x040072EA RID: 29418
		private TMP_Text _subtitleText;

		// Token: 0x040072EB RID: 29419
		private TMP_Text _composerStatus;

		// Token: 0x040072EC RID: 29420
		private RectTransform _content;

		// Token: 0x040072ED RID: 29421
		private ScrollRect _scroll;

		// Token: 0x040072EE RID: 29422
		private TMP_InputField _input;

		// Token: 0x040072EF RID: 29423
		private Button _sendButton;

		// Token: 0x040072F0 RID: 29424
		private CanvasGroup _sendVisuals;

		// Token: 0x040072F1 RID: 29425
		private RectTransform _newMessages;

		// Token: 0x040072F2 RID: 29426
		private RectTransform _groupDetails;

		// Token: 0x040072F3 RID: 29427
		private Coroutine _scrollRoutine;

		// Token: 0x040072F4 RID: 29428
		private OxGramChat _chat;

		// Token: 0x040072F5 RID: 29429
		private OxGramLifeSimulator _life;

		// Token: 0x040072F6 RID: 29430
		private OxGramReactionPool _reactionPool;

		// Token: 0x040072F7 RID: 29431
		private string _typingAuthor;

		// Token: 0x040072FA RID: 29434
		private float _censorBugChance;

		// Token: 0x040072FB RID: 29435
		private const float TopInset = 28f;

		// Token: 0x040072FC RID: 29436
		private const float BottomInset = 24f;

		// Token: 0x040072FD RID: 29437
		private const float HeaderBar = 104f;

		// Token: 0x040072FE RID: 29438
		private const float InputBar = 108f;

		// Token: 0x040072FF RID: 29439
		private const float MaxBubbleWidth = 330f;

		// Token: 0x04007300 RID: 29440
		private const float BubblePadX = 14f;

		// Token: 0x04007301 RID: 29441
		private const float BubblePadY = 10f;

		// Token: 0x04007302 RID: 29442
		private const float BodyFont = 18f;

		// Token: 0x04007303 RID: 29443
		private const float AuthorFont = 14f;

		// Token: 0x04007304 RID: 29444
		private const float TimeFont = 11f;

		// Token: 0x04007305 RID: 29445
		private const float TimeReservedWidth = 52f;

		// Token: 0x04007306 RID: 29446
		private const float AvatarSize = 32f;

		// Token: 0x04007307 RID: 29447
		private TMP_Text _measureProbe;

		// Token: 0x04007308 RID: 29448
		private RectTransform _photoPicker;

		// Token: 0x04007309 RID: 29449
		private RectTransform _sendConfirm;

		// Token: 0x0400730A RID: 29450
		private bool _blurFace = true;

		// Token: 0x0400730B RID: 29451
		private Toggle _faceBlurToggle;

		// Token: 0x0400730C RID: 29452
		private Image _photoPreview;

		// Token: 0x0400730D RID: 29453
		private Sprite _selectedPhoto;

		// Token: 0x0400730E RID: 29454
		private TMP_Text _privacyHint;

		// Token: 0x0400730F RID: 29455
		private TMP_Text _rewardHint;

		// Token: 0x04007310 RID: 29456
		private RectTransform _errorPopup;

		// Token: 0x04007311 RID: 29457
		private AudioSource _errorAudio;
	}
}
