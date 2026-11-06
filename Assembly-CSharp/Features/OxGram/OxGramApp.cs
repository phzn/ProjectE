using System;
using System.Collections.Generic;
using Features.Localization;
using Features.PhoneCompanion;
using ProjektSumperk;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.OxGram
{
	// Token: 0x02000E8A RID: 3722
	[RequireComponent(typeof(RectTransform))]
	public class OxGramApp : MonoBehaviour
	{
		// Token: 0x17001250 RID: 4688
		// (get) Token: 0x06006FC9 RID: 28617 RVA: 0x002D2331 File Offset: 0x002D0531
		// (set) Token: 0x06006FCA RID: 28618 RVA: 0x002D2339 File Offset: 0x002D0539
		public string HistoryDirectoryOverride { get; set; }

		// Token: 0x06006FCB RID: 28619 RVA: 0x002D2344 File Offset: 0x002D0544
		private void Awake()
		{
			this._root = (RectTransform)base.transform;
			if (this.theme == null)
			{
				Debug.LogError("[OxGram] No theme assigned; assign an OxGramTheme asset.");
				base.enabled = false;
				return;
			}
			this.ConfigureNativeShell();
			this._chatStore = new OxGramChatStore(this.HistoryDirectoryOverride);
			this._chats = this._chatStore.Load();
			this.BuildBackground();
			this.BuildOnboarding();
			this.BuildListView();
			this.BuildChatView();
			if (OxGramUser.HasUsername)
			{
				this.ShowList();
			}
			else
			{
				this.ShowOnboarding();
			}
			if (this.debugOpenChatIndex >= 0 && this.debugOpenChatIndex < this._chats.Count)
			{
				this.OpenChat(this._chats[this.debugOpenChatIndex]);
			}
		}

		// Token: 0x06006FCC RID: 28620 RVA: 0x002D240C File Offset: 0x002D060C
		private void ConfigureNativeShell()
		{
			this._root.anchorMin = Vector2.zero;
			this._root.anchorMax = Vector2.one;
			this._root.pivot = new Vector2(0.5f, 0.5f);
			this._root.sizeDelta = new Vector2(2.0443f, 4.3207f);
			this._root.anchoredPosition3D = Vector3.zero;
			this._root.localScale = new Vector3(0.20591347f, 0.20591347f, 0.20591347f);
			Image image = base.GetComponent<Image>();
			if (image == null)
			{
				image = base.gameObject.AddComponent<Image>();
			}
			image.color = this.theme.appBackground;
			image.raycastTarget = true;
			if (base.GetComponent<Mask>() == null)
			{
				base.gameObject.AddComponent<Mask>();
			}
			this._animator = base.GetComponent<Animator>();
			if (this._animator == null)
			{
				this._animator = base.gameObject.AddComponent<Animator>();
			}
			this._content = OxGramUI.Rect("Content", this._root);
			this._content.anchorMin = new Vector2(0.5f, 0.5f);
			this._content.anchorMax = new Vector2(0.5f, 0.5f);
			this._content.pivot = new Vector2(0.5f, 0.5f);
			this._content.sizeDelta = new Vector2(480f, 1014f);
			this._content.anchoredPosition = Vector2.zero;
			LayoutRebuilder.ForceRebuildLayoutImmediate(this._root);
			Rect rect = this._root.rect;
			float num = ((Mathf.Abs(rect.height) > 0.0001f) ? Mathf.Abs(rect.height) : 5.4411f) / 1014f;
			this._content.localScale = new Vector3(num, num, 1f);
			this._island = base.GetComponentInParent<DynamicIslandController>();
			if (this._island == null)
			{
				Transform root = base.transform.root;
				this._island = ((root != null) ? root.GetComponentInChildren<DynamicIslandController>(true) : null);
			}
		}

		// Token: 0x06006FCD RID: 28621 RVA: 0x002D2640 File Offset: 0x002D0840
		private void BuildBackground()
		{
			Image image = OxGramUI.Image("Background", this._content, this.theme.appBackground, null);
			OxGramUI.Stretch(image.rectTransform, 0f, 0f, 0f, 0f);
			image.transform.SetAsFirstSibling();
		}

		// Token: 0x06006FCE RID: 28622 RVA: 0x002D2694 File Offset: 0x002D0894
		private void BuildChatView()
		{
			RectTransform rectTransform = OxGramUI.Rect("ChatView", this._content);
			OxGramUI.Stretch(rectTransform, 0f, 0f, 0f, 0f);
			this._chatView = rectTransform.gameObject.AddComponent<OxGramChatView>();
			this._chatView.Build(this.theme, new Action(this.OnBackToList), new Action(this.CloseApp), this.reactionPool, this.censorBugChance);
			this._chatView.ChatChanged += delegate(OxGramChat _)
			{
				this.SaveChats();
			};
			this._chatView.RenameRequested += this.ShowRenameChat;
			rectTransform.gameObject.SetActive(false);
		}

		// Token: 0x06006FCF RID: 28623 RVA: 0x002D2750 File Offset: 0x002D0950
		private void BuildBackButton(Transform parent, float yOffset)
		{
			OxGramUI.BackButton(parent, (this.theme.fontBold != null) ? this.theme.fontBold : this.theme.font, this.theme.headerForeground, yOffset, new Action(this.CloseApp), "Back", this.theme.backSprite);
		}

		// Token: 0x06006FD0 RID: 28624 RVA: 0x002D27B8 File Offset: 0x002D09B8
		public void OpenApp()
		{
			if (!PhoneAppAccessPolicy.TryOpen("CielGram"))
			{
				return;
			}
			this._ignoreCloseUntil = Time.unscaledTime + 0.3f;
			if (!base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(true);
			}
			this.EnsureHomeUnderneath();
			if (this._animator != null && this._animator.isActiveAndEnabled)
			{
				this._animator.Play("BackIdleAnimation", -1, 0f);
			}
			if (OxGramUser.HasUsername)
			{
				this.ShowList();
				return;
			}
			this.ShowOnboarding();
		}

		// Token: 0x06006FD1 RID: 28625 RVA: 0x002D2848 File Offset: 0x002D0A48
		private void EnsureHomeUnderneath()
		{
			Transform parent = base.transform.parent;
			if (parent == null)
			{
				return;
			}
			foreach (object obj in parent)
			{
				Transform transform = (Transform)obj;
				if (!(transform == base.transform))
				{
					if (transform.name == "Home")
					{
						if (!transform.gameObject.activeSelf)
						{
							transform.gameObject.SetActive(true);
						}
					}
					else if (transform.name.Length > 1 && transform.name[0] == 'm' && char.IsUpper(transform.name[1]) && transform.gameObject.activeSelf)
					{
						transform.gameObject.SetActive(false);
					}
				}
			}
		}

		// Token: 0x06006FD2 RID: 28626 RVA: 0x002D2938 File Offset: 0x002D0B38
		public void CloseApp()
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			if (SmartphoneController.RequestCensorLeakGameOver())
			{
				return;
			}
			if (Time.unscaledTime < this._ignoreCloseUntil)
			{
				return;
			}
			if (this._island != null && this._animator != null)
			{
				this._island.UniversalBack(this._animator);
				return;
			}
			base.gameObject.SetActive(false);
		}

		// Token: 0x06006FD3 RID: 28627 RVA: 0x002D29A4 File Offset: 0x002D0BA4
		public void ShowList()
		{
			this.CloseDirectChatDialog();
			this._ignoreCloseUntil = Time.unscaledTime + 0.3f;
			if (this._onboarding != null)
			{
				this._onboarding.gameObject.SetActive(false);
			}
			if (this._listView != null)
			{
				this._listView.gameObject.SetActive(true);
			}
			if (this._chatView != null)
			{
				this._chatView.gameObject.SetActive(false);
			}
			this.RefreshList();
		}

		// Token: 0x06006FD4 RID: 28628 RVA: 0x002D2A2C File Offset: 0x002D0C2C
		private void OpenChat(OxGramChat chat)
		{
			this.CloseDirectChatDialog();
			this._ignoreCloseUntil = Time.unscaledTime + 0.3f;
			this._listView.gameObject.SetActive(false);
			this._chatView.gameObject.SetActive(true);
			this._chatView.Open(chat);
		}

		// Token: 0x06006FD5 RID: 28629 RVA: 0x002D2A7E File Offset: 0x002D0C7E
		private void OnBackToList()
		{
			this.ShowList();
		}

		// Token: 0x06006FD6 RID: 28630 RVA: 0x002D2A86 File Offset: 0x002D0C86
		private void SaveChats()
		{
			this._historyDirty = true;
			if (this._chatStore != null && this._chatStore.Save(this._chats))
			{
				this._historyDirty = false;
			}
		}

		// Token: 0x06006FD7 RID: 28631 RVA: 0x002D2AB1 File Offset: 0x002D0CB1
		private void OnDisable()
		{
			this.CloseDirectChatDialog();
			if (this._historyDirty)
			{
				this.SaveChats();
			}
			SmartphoneController.RequestCensorLeakGameOver();
		}

		// Token: 0x06006FD8 RID: 28632 RVA: 0x002D2ACD File Offset: 0x002D0CCD
		private void OnApplicationPause(bool paused)
		{
			if (paused && this._historyDirty)
			{
				this.SaveChats();
			}
		}

		// Token: 0x06006FD9 RID: 28633 RVA: 0x002D2AE0 File Offset: 0x002D0CE0
		private void OnDestroy()
		{
			if (this._historyDirty)
			{
				this.SaveChats();
			}
			OxGramChatStore chatStore = this._chatStore;
			if (chatStore == null)
			{
				return;
			}
			chatStore.Dispose();
		}

		// Token: 0x06006FDA RID: 28634 RVA: 0x002D2B00 File Offset: 0x002D0D00
		private void LogOut()
		{
			OxGramUser.Clear();
			if (this._usernameInput != null)
			{
				this._usernameInput.text = "";
			}
			this.ShowOnboarding();
		}

		// Token: 0x06006FDB RID: 28635 RVA: 0x002D2B2C File Offset: 0x002D0D2C
		private void CloseDirectChatDialog()
		{
			if (this._directChatDialog == null)
			{
				return;
			}
			this._directChatDialog.gameObject.SetActive(false);
			if (Application.isPlaying)
			{
				Object.Destroy(this._directChatDialog.gameObject);
			}
			else
			{
				Object.DestroyImmediate(this._directChatDialog.gameObject);
			}
			this._directChatDialog = null;
		}

		// Token: 0x06006FDC RID: 28636 RVA: 0x002D2B8C File Offset: 0x002D0D8C
		private RectTransform NewDirectChatDialog()
		{
			this.CloseDirectChatDialog();
			this._directChatDialog = OxGramUI.Rect("DirectChatDialog", this._content);
			OxGramUI.Stretch(this._directChatDialog, 0f, 0f, 0f, 0f);
			this._directChatDialog.SetAsLastSibling();
			Image image = OxGramUI.Image("Shade", this._directChatDialog, new Color(0f, 0f, 0f, 0.72f), null);
			OxGramUI.Stretch(image.rectTransform, 0f, 0f, 0f, 0f);
			OxGramUI.Button(image.gameObject).onClick.AddListener(new UnityAction(this.CloseDirectChatDialog));
			return this._directChatDialog;
		}

		// Token: 0x06006FDD RID: 28637 RVA: 0x002D2C50 File Offset: 0x002D0E50
		private void ShowNewChat()
		{
			RectTransform rectTransform = this.NewDirectChatDialog();
			Image image = this.Card("ContactCard", rectTransform, this.theme.headerColor, 24f, 155f, 432f, 704f);
			this.LocalizedLabel("Title", image.transform, "New direct chat", 22f, 24f, 388f, 40f, 27f, this.theme.textPrimary, true);
			this.LocalizedLabel("Hint", image.transform, "Choose someone from your groups.", 22f, 73f, 388f, 50f, 17f, this.theme.textSecondary, false);
			RectTransform rectTransform2 = OxGramUI.Rect("Contacts", image.transform);
			OxGramApp.Place(rectTransform2, 14f, 135f, 404f, 476f);
			ScrollRect scrollRect = rectTransform2.gameObject.AddComponent<ScrollRect>();
			scrollRect.horizontal = false;
			scrollRect.movementType = ScrollRect.MovementType.Clamped;
			scrollRect.scrollSensitivity = 28f;
			Image image2 = OxGramUI.Image("Viewport", rectTransform2, Color.clear, null);
			OxGramUI.Stretch(image2.rectTransform, 0f, 0f, 0f, 0f);
			image2.gameObject.AddComponent<RectMask2D>();
			scrollRect.viewport = image2.rectTransform;
			RectTransform rectTransform3 = OxGramUI.Rect("Rows", image2.transform);
			rectTransform3.anchorMin = new Vector2(0f, 1f);
			rectTransform3.anchorMax = Vector2.one;
			rectTransform3.pivot = new Vector2(0.5f, 1f);
			rectTransform3.sizeDelta = Vector2.zero;
			VerticalLayoutGroup verticalLayoutGroup = rectTransform3.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.childControlWidth = (verticalLayoutGroup.childControlHeight = true);
			verticalLayoutGroup.childForceExpandWidth = true;
			verticalLayoutGroup.childForceExpandHeight = false;
			verticalLayoutGroup.padding = new RectOffset(8, 8, 0, 8);
			verticalLayoutGroup.spacing = 8f;
			rectTransform3.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			scrollRect.content = rectTransform3;
			using (IEnumerator<OxGramContacts.Contact> enumerator = OxGramContacts.All.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					OxGramContacts.Contact contact = enumerator.Current;
					OxGramChat oxGramChat = this._chats.Find((OxGramChat chat) => !chat.isGroup && chat.contactId == contact.id);
					Image image3 = this.Card("Contact_" + contact.id, rectTransform3, this.theme.bubbleOther, 0f, 0f, 388f, 76f);
					image3.gameObject.AddComponent<LayoutElement>().preferredHeight = 76f;
					TMP_Text tmp_Text = this.Label("Name", image3.transform, (oxGramChat != null) ? oxGramChat.title : contact.name, 16f, 10f, 319f, 30f, 21f, this.theme.textPrimary, true);
					tmp_Text.richText = false;
					LocalizedGameText.BindFontOnly(tmp_Text);
					TMP_Text tmp_Text2 = this.Label("Group", image3.transform, "", 16f, 43f, 319f, 23f, 12f, this.theme.textSecondary, false);
					tmp_Text2.richText = false;
					LocalizedGameText.BindComputed(tmp_Text2, () => contact.name + " / " + GameLocalization.GetSource("phone_story", contact.group));
					this.Icon("Open", image3.transform, OxGramVectorIcon.Symbol.Chevron, this.theme.authorAccent, 348f, 27f, 20f);
					OxGramUI.Button(image3.gameObject).onClick.AddListener(delegate
					{
						OxGramChat orCreate = OxGramContacts.GetOrCreate(this._chats, contact);
						this.SaveChats();
						this.OpenChat(orCreate);
					});
				}
			}
			this.LocalizedActionButton("Cancel", image.transform, "Cancel", 22f, 634f, 388f, 48f, this.theme.bubbleOther, new UnityAction(this.CloseDirectChatDialog));
		}

		// Token: 0x06006FDE RID: 28638 RVA: 0x002D3070 File Offset: 0x002D1270
		private void ShowRenameChat(OxGramChat chat)
		{
			if (chat == null || chat.isGroup)
			{
				return;
			}
			RectTransform rectTransform = this.NewDirectChatDialog();
			Image image = this.Card("RenameCard", rectTransform, this.theme.headerColor, 24f, 284f, 432f, 432f);
			this.LocalizedLabel("Title", image.transform, "Rename contact", 24f, 24f, 384f, 44f, 27f, this.theme.textPrimary, true);
			this.LocalizedLabel("Hint", image.transform, "Choose the name shown in this chat.", 24f, 80f, 384f, 48f, 17f, this.theme.textSecondary, false);
			TMP_InputField input = this.Input("ContactName", image.transform, "Contact name", 24f, 151f, 384f, 58f, 18f);
			input.characterLimit = 24;
			input.SetTextWithoutNotify(chat.title);
			TMP_Text feedback = this.LocalizedLabel("NameFeedback", image.transform, "Saved on this phone.", 24f, 225f, 384f, 62f, 15f, this.theme.textSecondary, false);
			feedback.enableWordWrapping = true;
			Button save = null;
			Action validate = delegate
			{
				string text;
				string text2;
				bool flag = OxGramContacts.TryNormalizeName(input.text, out text, out text2);
				if (save != null)
				{
					save.interactable = flag;
				}
				feedback.color = (flag ? this.theme.textSecondary : OxGramApp.Warning);
				OxGramApp.SetLocalized(feedback, flag ? "Saved on this phone." : text2);
			};
			UnityAction apply = delegate
			{
				string text3;
				if (!OxGramContacts.TryRename(chat, input.text, out text3))
				{
					validate();
					return;
				}
				this.SaveChats();
				this.OpenChat(chat);
			};
			save = this.LocalizedActionButton("SaveName", image.transform, "Save name", 222f, 329f, 186f, 56f, this.theme.accent, apply).GetComponent<Button>();
			this.LocalizedActionButton("Cancel", image.transform, "Cancel", 24f, 329f, 186f, 56f, this.theme.bubbleOther, new UnityAction(this.CloseDirectChatDialog));
			input.onValueChanged.AddListener(delegate(string _)
			{
				validate();
			});
			input.onSubmit.AddListener(delegate(string _)
			{
				apply();
			});
			validate();
		}

		// Token: 0x17001251 RID: 4689
		// (get) Token: 0x06006FDF RID: 28639 RVA: 0x002D32ED File Offset: 0x002D14ED
		private TMP_FontAsset Bold
		{
			get
			{
				if (!(this.theme.fontBold != null))
				{
					return this.theme.font;
				}
				return this.theme.fontBold;
			}
		}

		// Token: 0x06006FE0 RID: 28640 RVA: 0x002D331C File Offset: 0x002D151C
		private static void Place(RectTransform rect, float x, float y, float width, float height)
		{
			Vector2 vector = new Vector2(0f, 1f);
			rect.anchorMax = vector;
			rect.anchorMin = vector;
			rect.pivot = new Vector2(0f, 1f);
			rect.anchoredPosition = new Vector2(x, -y);
			rect.sizeDelta = new Vector2(width, height);
		}

		// Token: 0x06006FE1 RID: 28641 RVA: 0x002D337C File Offset: 0x002D157C
		private TMP_Text Label(string name, Transform parent, string text, float x, float y, float width, float height, float size, Color color, bool bold = false)
		{
			TMP_Text tmp_Text = OxGramUI.Text(name, parent, text, bold ? this.Bold : this.theme.font, size, color, TextAlignmentOptions.Left);
			OxGramApp.Place(tmp_Text.rectTransform, x, y, width, height);
			tmp_Text.overflowMode = TextOverflowModes.Ellipsis;
			return tmp_Text;
		}

		// Token: 0x06006FE2 RID: 28642 RVA: 0x002D33CC File Offset: 0x002D15CC
		private TMP_Text LocalizedLabel(string name, Transform parent, string text, float x, float y, float width, float height, float size, Color color, bool bold = false)
		{
			TMP_Text tmp_Text = this.Label(name, parent, text, x, y, width, height, size, color, bold);
			LocalizedGameText.BindSource(tmp_Text, "phone_story", text, null).FitTranslationToRectangle(false);
			return tmp_Text;
		}

		// Token: 0x06006FE3 RID: 28643 RVA: 0x002D3403 File Offset: 0x002D1603
		private static void SetLocalized(TMP_Text label, string source)
		{
			if (label != null)
			{
				LocalizedGameText.BindSource(label, "phone_story", source, null);
			}
		}

		// Token: 0x06006FE4 RID: 28644 RVA: 0x002D341C File Offset: 0x002D161C
		private Image Card(string name, Transform parent, Color color, float x, float y, float width, float height)
		{
			Image image = OxGramUI.Image(name, parent, color, OxGramBubbleSprite.Body);
			OxGramApp.Place(image.rectTransform, x, y, width, height);
			return image;
		}

		// Token: 0x06006FE5 RID: 28645 RVA: 0x002D343E File Offset: 0x002D163E
		private OxGramVectorIcon Icon(string name, Transform parent, OxGramVectorIcon.Symbol symbol, Color color, float x, float y, float size)
		{
			RectTransform rectTransform = OxGramUI.Rect(name, parent);
			OxGramApp.Place(rectTransform, x, y, size, size);
			OxGramVectorIcon oxGramVectorIcon = rectTransform.gameObject.AddComponent<OxGramVectorIcon>();
			oxGramVectorIcon.symbol = symbol;
			oxGramVectorIcon.color = color;
			oxGramVectorIcon.raycastTarget = false;
			return oxGramVectorIcon;
		}

		// Token: 0x06006FE6 RID: 28646 RVA: 0x002D3478 File Offset: 0x002D1678
		private Image ActionButton(string name, Transform parent, string text, float x, float y, float width, float height, Color color, UnityAction action)
		{
			Image image = this.Card(name, parent, color, x, y, width, height);
			OxGramUI.Stretch(OxGramUI.Text("Label", image.transform, text, this.Bold, 17f, this.theme.textPrimary, TextAlignmentOptions.Center).rectTransform, 10f, 10f, 2f, 2f);
			OxGramUI.Button(image.gameObject).onClick.AddListener(action);
			return image;
		}

		// Token: 0x06006FE7 RID: 28647 RVA: 0x002D34FC File Offset: 0x002D16FC
		private Image LocalizedActionButton(string name, Transform parent, string text, float x, float y, float width, float height, Color color, UnityAction action)
		{
			Image image = this.ActionButton(name, parent, text, x, y, width, height, color, action);
			TMP_Text componentInChildren = image.GetComponentInChildren<TMP_Text>();
			if (componentInChildren != null)
			{
				LocalizedGameText.BindSource(componentInChildren, "phone_story", text, null).FitTranslationToRectangle(false);
			}
			return image;
		}

		// Token: 0x06006FE8 RID: 28648 RVA: 0x002D3544 File Offset: 0x002D1744
		private TMP_InputField Input(string name, Transform parent, string hint, float x, float y, float width, float height, float inset = 18f)
		{
			Image image = this.Card(name, parent, this.theme.bubbleOther, x, y, width, height);
			RectTransform rectTransform = OxGramUI.Rect("TextArea", image.transform);
			OxGramUI.Stretch(rectTransform, inset, 40f, 8f, 8f);
			rectTransform.gameObject.AddComponent<RectMask2D>();
			TMP_Text tmp_Text = OxGramUI.Text("Placeholder", rectTransform, hint, this.theme.font, 18f, this.theme.textSecondary, TextAlignmentOptions.Left);
			LocalizedGameText.BindSource(tmp_Text, "phone_story", hint, null).FitTranslationToRectangle(false);
			OxGramUI.Stretch(tmp_Text.rectTransform, 0f, 0f, 0f, 0f);
			TMP_Text tmp_Text2 = OxGramUI.Text("Text", rectTransform, "", this.theme.font, 18f, this.theme.textPrimary, TextAlignmentOptions.Left);
			tmp_Text2.richText = false;
			LocalizedGameText.BindFontOnly(tmp_Text2);
			OxGramUI.Stretch(tmp_Text2.rectTransform, 0f, 0f, 0f, 0f);
			TMP_InputField tmp_InputField = image.gameObject.AddComponent<TMP_InputField>();
			tmp_InputField.textViewport = rectTransform;
			tmp_InputField.textComponent = tmp_Text2;
			tmp_InputField.placeholder = tmp_Text;
			tmp_InputField.lineType = TMP_InputField.LineType.SingleLine;
			tmp_InputField.customCaretColor = true;
			tmp_InputField.caretColor = this.theme.authorAccent;
			tmp_InputField.selectionColor = new Color(0.49f, 0.39f, 0.84f, 0.4f);
			return tmp_InputField;
		}

		// Token: 0x06006FE9 RID: 28649 RVA: 0x002D36C0 File Offset: 0x002D18C0
		private void BuildOnboarding()
		{
			this._onboarding = OxGramUI.Rect("Onboarding", this._content);
			OxGramUI.Stretch(this._onboarding, 0f, 0f, 0f, 0f);
			OxGramUI.Stretch(OxGramUI.Image("OnbBg", this._onboarding, this.theme.appBackground, null).rectTransform, 0f, 0f, 0f, 0f);
			OxGramUI.BackButton(this.Card("Header", this._onboarding, this.theme.headerColor, 0f, 0f, 480f, 92f).transform, this.Bold, this.theme.authorAccent, -14f, delegate
			{
				if (OxGramUser.HasUsername)
				{
					this.ShowList();
					return;
				}
				this.CloseApp();
			}, "Back", this.theme.backSprite);
			Image image = this.Card("IdentityMark", this._onboarding, this.theme.bubbleOther, 36f, 140f, 88f, 88f);
			this.Icon("Shield", image.transform, OxGramVectorIcon.Symbol.Shield, this.theme.authorAccent, 20f, 18f, 48f);
			this.LocalizedLabel("Edition", this._onboarding, "CIELGRAM / ALIAS MODE", 36f, 254f, 408f, 24f, 13f, OxGramApp.Mint, true);
			this._usernameHeading = this.LocalizedLabel("Welcome", this._onboarding, "A new name.\nYour own space.", 36f, 290f, 408f, 105f, 38f, this.theme.textPrimary, true);
			this.LocalizedLabel("Sub", this._onboarding, "Join the conversation with an alias.\nThis is the name everyone will reply to.", 36f, 410f, 408f, 58f, 18f, this.theme.textSecondary, false);
			this.LocalizedLabel("UsernameLabel", this._onboarding, "YOUR ALIAS", 36f, 505f, 408f, 22f, 13f, this.theme.authorAccent, true);
			this._usernameInput = this.Input("Field", this._onboarding, "Choose an alias", 36f, 542f, 408f, 60f, 42f);
			this.Label("At", this._usernameInput.transform, "@", 16f, 13f, 26f, 34f, 22f, this.theme.authorAccent, true);
			this._usernameInput.characterLimit = 20;
			this._usernameInput.onSubmit.AddListener(delegate(string _)
			{
				this.ConfirmUsername();
			});
			this._usernameInput.onValueChanged.AddListener(delegate(string _)
			{
				this.ValidateUsername(false);
			});
			this._usernameFeedback = this.LocalizedLabel("Validation", this._onboarding, "Keep your identity private. Your real name, Ciel, is reserved.", 38f, 615f, 404f, 56f, 15f, this.theme.textSecondary, false);
			this._usernameFeedback.enableWordWrapping = true;
			Image image2 = this.LocalizedActionButton("Confirm", this._onboarding, "Start chatting", 36f, 688f, 408f, 58f, this.theme.accent, new UnityAction(this.ConfirmUsername));
			this._usernameConfirm = image2.GetComponent<Button>();
			this._usernameConfirmLabel = image2.GetComponentInChildren<TMP_Text>();
			Image image3 = this.Card("DisclaimerBg", this._onboarding, this.theme.bubbleOther, 36f, 787f, 408f, 117f);
			this.LocalizedLabel("NoteHeading", image3.transform, "UNOFFICIAL CLIENT", 18f, 16f, 372f, 20f, 12f, new Color(0.93f, 0.73f, 0.47f), true);
			this.LocalizedLabel("Disclaimer", image3.transform, "An anonymous, modified build of CielGram.\nThis client is unstable; privacy tools may fail.", 18f, 46f, 372f, 54f, 15f, this.theme.textSecondary, false);
			this._logoutButton = this.LocalizedActionButton("Logout", this._onboarding, "Log out", 160f, 923f, 160f, 44f, this.theme.bubbleOther, new UnityAction(this.LogOut)).gameObject;
			this._onboarding.gameObject.SetActive(false);
		}

		// Token: 0x06006FEA RID: 28650 RVA: 0x002D3B6C File Offset: 0x002D1D6C
		private void ValidateUsername(bool submitted)
		{
			string text = this._usernameInput.text;
			string text2;
			bool flag = OxGramUser.IsValidDisplayName(text, out text2);
			this._usernameConfirm.interactable = flag;
			bool flag2 = !flag && (submitted || !string.IsNullOrEmpty(text));
			this._usernameFeedback.color = (flag2 ? OxGramApp.Warning : this.theme.textSecondary);
			OxGramApp.SetLocalized(this._usernameFeedback, flag2 ? text2 : "Keep your identity private. Your real name, Ciel, is reserved.");
		}

		// Token: 0x06006FEB RID: 28651 RVA: 0x002D3BE8 File Offset: 0x002D1DE8
		private void ConfirmUsername()
		{
			string text;
			if (!OxGramUser.TrySetDisplayName(this._usernameInput.text, out text))
			{
				OxGramApp.SetLocalized(this._usernameFeedback, text);
				this._usernameFeedback.color = OxGramApp.Warning;
				this.ValidateUsername(true);
				return;
			}
			this.ShowList();
		}

		// Token: 0x06006FEC RID: 28652 RVA: 0x002D3C34 File Offset: 0x002D1E34
		public void ShowOnboarding()
		{
			this.CloseDirectChatDialog();
			this._ignoreCloseUntil = Time.unscaledTime + 0.3f;
			if (this._onboarding != null)
			{
				this._onboarding.gameObject.SetActive(true);
			}
			if (this._listView != null)
			{
				this._listView.gameObject.SetActive(false);
			}
			if (this._chatView != null)
			{
				this._chatView.gameObject.SetActive(false);
			}
			bool hasUsername = OxGramUser.HasUsername;
			OxGramApp.SetLocalized(this._usernameHeading, hasUsername ? "Your alias.\nYour own space." : "A new name.\nYour own space.");
			this._usernameInput.SetTextWithoutNotify(hasUsername ? OxGramUser.DisplayName : "");
			OxGramApp.SetLocalized(this._usernameConfirmLabel, hasUsername ? "Save alias" : "Start chatting");
			this._logoutButton.SetActive(hasUsername);
			this.ValidateUsername(false);
		}

		// Token: 0x06006FED RID: 28653 RVA: 0x002D3D1C File Offset: 0x002D1F1C
		private void BuildListView()
		{
			this._listView = OxGramUI.Rect("ListView", this._content);
			OxGramUI.Stretch(this._listView, 0f, 0f, 0f, 0f);
			Image image = this.Card("Header", this._listView, this.theme.headerColor, 0f, 0f, 480f, 92f);
			this.Label("Title", image.transform, "CielGram", 130f, 28f, 220f, 52f, 27f, this.theme.headerForeground, true).alignment = TextAlignmentOptions.Center;
			this.BuildBackButton(image.transform, -14f);
			Image image2 = this.Card("Profile", image.transform, this.theme.bubbleOther, 416f, 39f, 40f, 40f);
			this.Icon("Shield", image2.transform, OxGramVectorIcon.Symbol.Shield, this.theme.authorAccent, 9f, 8f, 22f);
			OxGramUI.Button(image2.gameObject).onClick.AddListener(new UnityAction(this.ShowOnboarding));
			Image image3 = this.Card("AliasCard", this._listView, new Color(0.15f, 0.14f, 0.25f), 24f, 116f, 432f, 137f);
			Image image4 = this.Card("Badge", image3.transform, new Color(0.23f, 0.21f, 0.37f), 18f, 20f, 56f, 56f);
			this.Icon("Shield", image4.transform, OxGramVectorIcon.Symbol.Shield, this.theme.authorAccent, 13f, 12f, 30f);
			this.LocalizedLabel("Status", image3.transform, "YOUR ANONYMOUS PROFILE", 91f, 18f, 318f, 21f, 12f, OxGramApp.Mint, true);
			this._profileAlias = this.Label("Alias", image3.transform, "", 91f, 40f, 294f, 48f, 26f, this.theme.textPrimary, true);
			this._profileAlias.richText = false;
			this.LocalizedLabel("Hint", image3.transform, "A little mystery. A lot to share.", 20f, 98f, 367f, 23f, 16f, this.theme.textSecondary, false);
			this.Icon("Edit", image3.transform, OxGramVectorIcon.Symbol.Chevron, this.theme.authorAccent, 393f, 99f, 20f);
			OxGramUI.Button(image3.gameObject).onClick.AddListener(new UnityAction(this.ShowOnboarding));
			this.LocalizedLabel("Heading", this._listView, "Conversations", 26f, 277f, 340f, 54f, 30f, this.theme.textPrimary, true);
			this._groupCount = this.Label("Count", this._listView, "", 354f, 292f, 98f, 26f, 13f, this.theme.textSecondary, false);
			this._groupCount.alignment = TextAlignmentOptions.Right;
			this._searchInput = this.Input("Search", this._listView, "Search chats or messages", 24f, 339f, 432f, 54f, 50f);
			this._searchInput.characterLimit = 80;
			this.Icon("SearchIcon", this._searchInput.transform, OxGramVectorIcon.Symbol.Search, this.theme.textSecondary, 16f, 15f, 24f);
			Image image5 = this.Card("Clear", this._searchInput.transform, Color.clear, 390f, 7f, 36f, 40f);
			this.Icon("Cross", image5.transform, OxGramVectorIcon.Symbol.Close, this.theme.textSecondary, 7f, 9f, 22f);
			OxGramUI.Button(image5.gameObject).onClick.AddListener(delegate
			{
				this._searchInput.text = "";
			});
			this._searchInput.onValueChanged.AddListener(delegate(string _)
			{
				this.RefreshList();
			});
			this._allFilter = this.LocalizedActionButton("AllGroups", this._listView, "All chats", 24f, 410f, 136f, 38f, this.theme.accent, delegate
			{
				this.SetFavoritesFilter(false);
			});
			this._favoritesFilter = this.LocalizedActionButton("Favorites", this._listView, "Favorites", 170f, 410f, 136f, 38f, this.theme.bubbleOther, delegate
			{
				this.SetFavoritesFilter(true);
			});
			this.LocalizedActionButton("NewChat", this._listView, "+ New chat", 316f, 410f, 140f, 38f, new Color(0.23f, 0.21f, 0.37f), new UnityAction(this.ShowNewChat));
			RectTransform rectTransform = OxGramUI.Rect("ListBody", this._listView);
			OxGramUI.Stretch(rectTransform, 0f, 0f, 467f, 102f);
			this._listScroll = rectTransform.gameObject.AddComponent<ScrollRect>();
			this._listScroll.horizontal = false;
			this._listScroll.movementType = ScrollRect.MovementType.Clamped;
			this._listScroll.scrollSensitivity = 28f;
			Image image6 = OxGramUI.Image("Viewport", rectTransform, Color.clear, null);
			OxGramUI.Stretch(image6.rectTransform, 0f, 0f, 0f, 0f);
			image6.gameObject.AddComponent<RectMask2D>();
			this._listScroll.viewport = image6.rectTransform;
			this._rows = OxGramUI.Rect("Content", image6.transform);
			this._rows.anchorMin = new Vector2(0f, 1f);
			this._rows.anchorMax = Vector2.one;
			this._rows.pivot = new Vector2(0.5f, 1f);
			this._rows.sizeDelta = Vector2.zero;
			VerticalLayoutGroup verticalLayoutGroup = this._rows.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.childControlWidth = (verticalLayoutGroup.childControlHeight = true);
			verticalLayoutGroup.childForceExpandWidth = true;
			verticalLayoutGroup.childForceExpandHeight = false;
			verticalLayoutGroup.spacing = 12f;
			verticalLayoutGroup.padding = new RectOffset(24, 24, 0, 12);
			this._rows.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			this._listScroll.content = this._rows;
			Image image7 = this.Card("Navigation", this._listView, this.theme.headerColor, 0f, 924f, 480f, 90f);
			OxGramApp.Place(OxGramUI.Image("Divider", image7.transform, this.theme.listRowDivider, null).rectTransform, 24f, 0f, 432f, 1f);
			Image image8 = this.Card("Chats", image7.transform, Color.clear, 24f, 10f, 204f, 48f);
			this.LocalizedLabel("Label", image8.transform, "Chats", 0f, 6f, 204f, 35f, 17f, this.theme.authorAccent, true).alignment = TextAlignmentOptions.Center;
			OxGramUI.Button(image8.gameObject).onClick.AddListener(new UnityAction(this.ShowList));
			Image image9 = this.Card("MyAlias", image7.transform, Color.clear, 252f, 10f, 204f, 48f);
			this.LocalizedLabel("Label", image9.transform, "My alias", 0f, 6f, 204f, 35f, 17f, this.theme.textSecondary, false).alignment = TextAlignmentOptions.Center;
			OxGramUI.Button(image9.gameObject).onClick.AddListener(new UnityAction(this.ShowOnboarding));
			this.RefreshList();
		}

		// Token: 0x06006FEE RID: 28654 RVA: 0x002D45C1 File Offset: 0x002D27C1
		private static string FavoriteKey(OxGramChat chat)
		{
			return "OxGram.Favorite." + (string.IsNullOrEmpty(chat.id) ? chat.title : chat.id);
		}

		// Token: 0x06006FEF RID: 28655 RVA: 0x002D45E8 File Offset: 0x002D27E8
		private static bool IsFavorite(OxGramChat chat)
		{
			return PlayerPrefs.GetInt(OxGramApp.FavoriteKey(chat), chat.isGroup ? PlayerPrefs.GetInt("OxGram.Favorite." + chat.title, 0) : 0) == 1;
		}

		// Token: 0x06006FF0 RID: 28656 RVA: 0x002D4619 File Offset: 0x002D2819
		private void SetFavoritesFilter(bool favorites)
		{
			this._favoritesOnly = favorites;
			this.RefreshList();
		}

		// Token: 0x06006FF1 RID: 28657 RVA: 0x002D4628 File Offset: 0x002D2828
		private void ToggleFavorite(OxGramChat chat)
		{
			PlayerPrefs.SetInt(OxGramApp.FavoriteKey(chat), OxGramApp.IsFavorite(chat) ? 0 : 1);
			PlayerPrefs.Save();
			this.RefreshList();
		}

		// Token: 0x06006FF2 RID: 28658 RVA: 0x0002E9C8 File Offset: 0x0002CBC8
		private static bool Contains(string text, string query)
		{
			return !string.IsNullOrEmpty(text) && text.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x06006FF3 RID: 28659 RVA: 0x002D464C File Offset: 0x002D284C
		private static bool MatchesSearch(OxGramChat chat, string query)
		{
			if (string.IsNullOrEmpty(query) || OxGramApp.Contains(chat.title, query) || OxGramApp.Contains(chat.subtitle, query) || OxGramApp.Contains(GameLocalization.GetSource("phone_story", chat.title), query) || OxGramApp.Contains(GameLocalization.GetSource("phone_story", chat.subtitle), query))
			{
				return true;
			}
			foreach (OxGramMessage oxGramMessage in chat.messages)
			{
				if (OxGramApp.Contains(oxGramMessage.text, query) || OxGramApp.Contains(OxGramPresentation.MessageBody(oxGramMessage), query) || OxGramApp.Contains(oxGramMessage.authorName, query))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006FF4 RID: 28660 RVA: 0x002D4720 File Offset: 0x002D2920
		private void RefreshList()
		{
			if (this._rows == null)
			{
				return;
			}
			this._profileAlias.text = OxGramUser.Mention;
			this._allFilter.color = (this._favoritesOnly ? this.theme.bubbleOther : this.theme.accent);
			this._favoritesFilter.color = (this._favoritesOnly ? this.theme.accent : this.theme.bubbleOther);
			for (int i = this._rows.childCount - 1; i >= 0; i--)
			{
				GameObject gameObject = this._rows.GetChild(i).gameObject;
				gameObject.SetActive(false);
				if (Application.isPlaying)
				{
					Object.Destroy(gameObject);
				}
				else
				{
					Object.DestroyImmediate(gameObject);
				}
			}
			string text = ((this._searchInput != null) ? this._searchInput.text.Trim() : "");
			List<OxGramChat> list = new List<OxGramChat>();
			foreach (OxGramChat oxGramChat in this._chats)
			{
				if ((!this._favoritesOnly || OxGramApp.IsFavorite(oxGramChat)) && OxGramApp.MatchesSearch(oxGramChat, text))
				{
					list.Add(oxGramChat);
				}
			}
			list.Sort(delegate(OxGramChat a, OxGramChat b)
			{
				int num = OxGramApp.IsFavorite(b).CompareTo(OxGramApp.IsFavorite(a));
				if (num == 0)
				{
					return this._chats.IndexOf(a).CompareTo(this._chats.IndexOf(b));
				}
				return num;
			});
			foreach (OxGramChat oxGramChat2 in list)
			{
				this.BuildChatRow(this._rows, oxGramChat2);
			}
			LocalizedGameText.BindFormat(this._groupCount, "phone_story", (list.Count == 1) ? "{0} CHAT" : "{0} CHATS", new object[] { list.Count });
			if (list.Count == 0)
			{
				Image image = this.Card("Empty", this._rows, this.theme.bubbleOther, 0f, 0f, 432f, 155f);
				image.gameObject.AddComponent<LayoutElement>().preferredHeight = 155f;
				this.LocalizedLabel("Title", image.transform, (this._favoritesOnly && text.Length == 0) ? "Keep your people close" : "No conversations found", 22f, 26f, 388f, 32f, 22f, this.theme.textPrimary, true);
				this.LocalizedLabel("Hint", image.transform, (this._favoritesOnly && text.Length == 0) ? "Tap the star on a chat to add it here.\nYour favorites stay at the top." : "Try another name or a word from a message.", 22f, 72f, 388f, 62f, 17f, this.theme.textSecondary, false);
			}
			this._listScroll.StopMovement();
			this._listScroll.verticalNormalizedPosition = 1f;
		}

		// Token: 0x06006FF5 RID: 28661 RVA: 0x002D4A20 File Offset: 0x002D2C20
		private void BuildChatRow(RectTransform parent, OxGramChat chat)
		{
			bool flag = OxGramApp.IsFavorite(chat);
			Color color = this.theme.AvatarColor(string.IsNullOrEmpty(chat.id) ? chat.title : chat.id);
			Image image = OxGramUI.Image("Row_" + chat.title, parent, Color.Lerp(this.theme.bubbleOther, color, flag ? 0.4f : 0.18f), OxGramBubbleSprite.Body);
			LayoutElement layoutElement = image.gameObject.AddComponent<LayoutElement>();
			layoutElement.minHeight = (layoutElement.preferredHeight = 126f);
			Image image2 = OxGramUI.Image("Surface", image.transform, this.theme.bubbleOther, OxGramBubbleSprite.Body);
			OxGramUI.Stretch(image2.rectTransform, 1f, 1f, 1f, 1f);
			image2.raycastTarget = false;
			OxGramUI.Button(image.gameObject).onClick.AddListener(delegate
			{
				this.OpenChat(chat);
			});
			Image image3 = this.Card("Avatar", image.transform, Color.Lerp(this.theme.bubbleOther, color, 0.28f), 16f, 17f, 58f, 58f);
			image3.raycastTarget = false;
			string text = (chat.title.StartsWith("Oxpath", StringComparison.Ordinal) ? "CS" : ((chat.title == "Night Owls") ? "NO" : ((chat.title == "Tokyo") ? "TK" : chat.title.Substring(0, 1))));
			this.Label("Initial", image3.transform, text, 0f, 0f, 58f, 58f, 21f, Color.Lerp(color, Color.white, 0.4f), true).alignment = TextAlignmentOptions.Center;
			TMP_Text tmp_Text = (chat.isGroup ? this.LocalizedLabel("Title", image.transform, chat.title, 89f, 15f, 283f, 31f, 21f, this.theme.textPrimary, true) : this.Label("Title", image.transform, chat.title, 89f, 15f, 283f, 31f, 21f, this.theme.textPrimary, true));
			tmp_Text.richText = false;
			if (!chat.isGroup)
			{
				LocalizedGameText.BindFontOnly(tmp_Text);
			}
			string text2 = ((!chat.isGroup) ? "DIRECT CHAT" : (chat.title.StartsWith("Oxpath", StringComparison.Ordinal) ? "CAMPUS / COMPUTER SCIENCE" : ((chat.title == "Tokyo") ? "CITY LIFE / TOKYO" : ((chat.title == "Night Owls") ? "AFTER HOURS / COMMUNITY" : "PUBLIC GROUP"))));
			this.LocalizedLabel("Topic", image.transform, text2, 90f, 49f, 272f, 24f, 11f, Color.Lerp(color, Color.white, 0.35f), true);
			Image image4 = this.Card("Favorite", image.transform, flag ? new Color(0.27f, 0.23f, 0.4f) : Color.clear, 376f, 14f, 40f, 42f);
			this.Icon("Star", image4.transform, OxGramVectorIcon.Symbol.Star, flag ? this.theme.authorAccent : this.theme.textSecondary, 9f, 10f, 22f);
			OxGramUI.Button(image4.gameObject).onClick.AddListener(delegate
			{
				this.ToggleFavorite(chat);
			});
			OxGramMessage last = chat.LastMessage;
			string text3 = ((last == null) ? "" : ((last.sender == OxGramSender.Me) ? "You" : last.authorName));
			string text4 = ((last == null) ? "Be the first to say something." : ((last.kind == OxGramMessageKind.Photo) ? (text3 + " shared a photo") : (text3 + ": " + last.text)));
			TMP_Text tmp_Text2 = this.Label("Preview", image.transform, text4, 18f, 89f, 330f, 24f, 16f, this.theme.textSecondary, false);
			LocalizedGameText.BindComputed(tmp_Text2, () => OxGramPresentation.Preview(last));
			tmp_Text2.richText = false;
			tmp_Text2.enableWordWrapping = false;
			this.Label("Time", image.transform, (last != null) ? last.time : "", 355f, 88f, 60f, 24f, 12f, this.theme.textSecondary, false).alignment = TextAlignmentOptions.Right;
		}

		// Token: 0x0400729B RID: 29339
		[SerializeField]
		private OxGramTheme theme;

		// Token: 0x0400729C RID: 29340
		[Tooltip("Pool di reazioni NPC alle foto (per parte del corpo). Se null, usa i default builtin.")]
		[SerializeField]
		private OxGramReactionPool reactionPool;

		// Token: 0x0400729D RID: 29341
		[Header("Censorship glitch")]
		[Tooltip("Solo durante le run e prima del rank Certified Exhibitionist; a CielHome il blur non fallisce. Probabilita' (%) che la censura del volto 'fallisca' (bug simulato): al posto della versione censurata viene inviata la foto ORIGINALE non censurata. Compare un popup di errore CRITICO e gli NPC reagiscono in modo esagerato perche' hanno scoperto chi e'. 0 = la censura non fallisce mai.")]
		[Range(0f, 100f)]
		[SerializeField]
		private float censorBugChance = 5f;

		// Token: 0x0400729E RID: 29342
		[Tooltip("Debug only: auto-open this chat index on start (-1 = stay on the list).")]
		[SerializeField]
		private int debugOpenChatIndex = -1;

		// Token: 0x0400729F RID: 29343
		private RectTransform _root;

		// Token: 0x040072A0 RID: 29344
		private RectTransform _listView;

		// Token: 0x040072A1 RID: 29345
		private RectTransform _onboarding;

		// Token: 0x040072A2 RID: 29346
		private TMP_InputField _usernameInput;

		// Token: 0x040072A3 RID: 29347
		private OxGramChatView _chatView;

		// Token: 0x040072A4 RID: 29348
		private List<OxGramChat> _chats;

		// Token: 0x040072A5 RID: 29349
		private OxGramChatStore _chatStore;

		// Token: 0x040072A6 RID: 29350
		private bool _historyDirty;

		// Token: 0x040072A8 RID: 29352
		private float _ignoreCloseUntil;

		// Token: 0x040072A9 RID: 29353
		private const float ScreenSwitchGuard = 0.3f;

		// Token: 0x040072AA RID: 29354
		private Animator _animator;

		// Token: 0x040072AB RID: 29355
		private DynamicIslandController _island;

		// Token: 0x040072AC RID: 29356
		private const float ContentWidthPx = 480f;

		// Token: 0x040072AD RID: 29357
		private const float ContentHeightPx = 1014f;

		// Token: 0x040072AE RID: 29358
		private RectTransform _content;

		// Token: 0x040072AF RID: 29359
		private RectTransform _directChatDialog;

		// Token: 0x040072B0 RID: 29360
		private const float TopInset = 28f;

		// Token: 0x040072B1 RID: 29361
		private const float HeaderBar = 92f;

		// Token: 0x040072B2 RID: 29362
		private static readonly Color Mint = new Color(0.48f, 0.87f, 0.76f);

		// Token: 0x040072B3 RID: 29363
		private static readonly Color Warning = new Color(1f, 0.65f, 0.63f);

		// Token: 0x040072B4 RID: 29364
		private TMP_Text _usernameFeedback;

		// Token: 0x040072B5 RID: 29365
		private TMP_Text _usernameHeading;

		// Token: 0x040072B6 RID: 29366
		private TMP_Text _usernameConfirmLabel;

		// Token: 0x040072B7 RID: 29367
		private Button _usernameConfirm;

		// Token: 0x040072B8 RID: 29368
		private GameObject _logoutButton;

		// Token: 0x040072B9 RID: 29369
		private TMP_Text _profileAlias;

		// Token: 0x040072BA RID: 29370
		private TMP_Text _groupCount;

		// Token: 0x040072BB RID: 29371
		private TMP_InputField _searchInput;

		// Token: 0x040072BC RID: 29372
		private ScrollRect _listScroll;

		// Token: 0x040072BD RID: 29373
		private RectTransform _rows;

		// Token: 0x040072BE RID: 29374
		private Image _allFilter;

		// Token: 0x040072BF RID: 29375
		private Image _favoritesFilter;

		// Token: 0x040072C0 RID: 29376
		private bool _favoritesOnly;
	}
}
