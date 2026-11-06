using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Features.CensorBars;
using Features.Localization;
using Features.OxGram;
using Features.PhoneCompanion;
using ProjektSumperk;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Features.MallHack
{
	// Token: 0x0200100A RID: 4106
	[RequireComponent(typeof(RectTransform))]
	public sealed class HackMePhoneApp : MonoBehaviour
	{
		// Token: 0x1700141F RID: 5151
		// (get) Token: 0x06007C9D RID: 31901 RVA: 0x0032E920 File Offset: 0x0032CB20
		public static bool IsAnyHackInProgress
		{
			get
			{
				return HackMePhoneApp._activeHackCount > 0;
			}
		}

		// Token: 0x17001420 RID: 5152
		// (get) Token: 0x06007C9E RID: 31902 RVA: 0x0032E92A File Offset: 0x0032CB2A
		public bool IsHackInProgress
		{
			get
			{
				return this._hackInProgress;
			}
		}

		// Token: 0x140000E1 RID: 225
		// (add) Token: 0x06007C9F RID: 31903 RVA: 0x0032E934 File Offset: 0x0032CB34
		// (remove) Token: 0x06007CA0 RID: 31904 RVA: 0x0032E968 File Offset: 0x0032CB68
		public static event Action<bool> OnHackActivityChanged;

		// Token: 0x06007CA1 RID: 31905 RVA: 0x0032E99B File Offset: 0x0032CB9B
		internal void Initialize(SmartphoneController owner, TMP_FontAsset font)
		{
			this._owner = owner;
			this._font = ((font != null) ? font : TMP_Settings.defaultFontAsset);
			this.ResolveNativeShell();
			this.BuildIfNeeded();
		}

		// Token: 0x06007CA2 RID: 31906 RVA: 0x0032E9C7 File Offset: 0x0032CBC7
		private void Awake()
		{
			this.SetHackInProgress(false);
			this.ResolveNativeShell();
			this.BuildIfNeeded();
		}

		// Token: 0x06007CA3 RID: 31907 RVA: 0x0032E9DC File Offset: 0x0032CBDC
		private void OnDisable()
		{
			this.CancelTransientUi();
			this._loadGeneration++;
			this._loadRoutine = null;
			this._galleryLoading = false;
			if (this._registeredAsOpen)
			{
				SmartphoneController.NotifyCustomAppClosed();
				this._registeredAsOpen = false;
			}
		}

		// Token: 0x06007CA4 RID: 31908 RVA: 0x0032EA14 File Offset: 0x0032CC14
		private void OnDestroy()
		{
			this.SetHackInProgress(false);
			this.ClearLoadedPhotos();
			this.DestroyGeneratedSounds();
		}

		// Token: 0x06007CA5 RID: 31909 RVA: 0x0032EA29 File Offset: 0x0032CC29
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetHackActivity()
		{
			HackMePhoneApp._activeHackCount = 0;
			HackMePhoneApp.OnHackActivityChanged = null;
		}

		// Token: 0x06007CA6 RID: 31910 RVA: 0x0032EA38 File Offset: 0x0032CC38
		private void SetHackInProgress(bool active)
		{
			bool isAnyHackInProgress = HackMePhoneApp.IsAnyHackInProgress;
			this._hackInProgress = active;
			if (active && !this._registeredAsHacking)
			{
				this._registeredAsHacking = true;
				HackMePhoneApp._activeHackCount++;
			}
			else if (!active && this._registeredAsHacking)
			{
				this._registeredAsHacking = false;
				HackMePhoneApp._activeHackCount = Mathf.Max(0, HackMePhoneApp._activeHackCount - 1);
			}
			if (isAnyHackInProgress != HackMePhoneApp.IsAnyHackInProgress)
			{
				Action<bool> onHackActivityChanged = HackMePhoneApp.OnHackActivityChanged;
				if (onHackActivityChanged == null)
				{
					return;
				}
				onHackActivityChanged(HackMePhoneApp.IsAnyHackInProgress);
			}
		}

		// Token: 0x06007CA7 RID: 31911 RVA: 0x0032EAB0 File Offset: 0x0032CCB0
		public void OpenApp()
		{
			if (!PhoneAppAccessPolicy.TryOpen("HackMe"))
			{
				return;
			}
			this.BuildIfNeeded();
			if (!base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(true);
			}
			this.CancelTransientUi();
			this.EnsureHomeUnderneath();
			if (this._animator != null && this._animator.isActiveAndEnabled)
			{
				this._animator.Play("BackIdleAnimation", -1, 0f);
			}
			SmartphoneController.NotifyCustomAppOpened(new Action(this.CloseApp));
			this._registeredAsOpen = true;
			HackMeStoreRegistry.EnsureLoadedScenesConfigured();
			this.RefreshStoreSignal();
			this.ReloadGallery();
		}

		// Token: 0x06007CA8 RID: 31912 RVA: 0x0032EB50 File Offset: 0x0032CD50
		public void CloseApp()
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			this.CancelTransientUi();
			if (this._registeredAsOpen)
			{
				SmartphoneController.NotifyCustomAppClosed();
				this._registeredAsOpen = false;
			}
			if (this._island != null && this._animator != null)
			{
				this._island.UniversalBack(this._animator);
				return;
			}
			base.gameObject.SetActive(false);
		}

		// Token: 0x06007CA9 RID: 31913 RVA: 0x0032EBC0 File Offset: 0x0032CDC0
		private void ResolveNativeShell()
		{
			if (this._animator == null)
			{
				this._animator = base.GetComponent<Animator>();
			}
			if (this._island == null)
			{
				this._island = base.GetComponentInParent<DynamicIslandController>();
				if (this._island == null && base.transform.root != null)
				{
					this._island = base.transform.root.GetComponentInChildren<DynamicIslandController>(true);
				}
			}
		}

		// Token: 0x06007CAA RID: 31914 RVA: 0x0032EC3C File Offset: 0x0032CE3C
		private void BuildIfNeeded()
		{
			if (this._built)
			{
				return;
			}
			this._built = true;
			RectTransform rectTransform = (RectTransform)base.transform;
			this._content = OxGramUI.Rect("Content", rectTransform);
			RectTransform content = this._content;
			RectTransform content2 = this._content;
			Vector2 vector = new Vector2(0.5f, 0.5f);
			content2.anchorMax = vector;
			content.anchorMin = vector;
			this._content.pivot = new Vector2(0.5f, 0.5f);
			this._content.sizeDelta = new Vector2(480f, 1014f);
			this._content.anchoredPosition = Vector2.zero;
			LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
			float num = ((Mathf.Abs(rectTransform.rect.height) > 0.0001f) ? Mathf.Abs(rectTransform.rect.height) : 5.4411f) / 1014f;
			this._content.localScale = new Vector3(num, num, 1f);
			Image image = OxGramUI.Image("Background", this._content, HackMePhoneApp.Background, null);
			OxGramUI.Stretch(image.rectTransform, 0f, 0f, 0f, 0f);
			image.transform.SetAsFirstSibling();
			this.BuildHeader();
			this.BuildSignalPanel();
			this.BuildGallery();
			this.EnsureAudioSource();
		}

		// Token: 0x06007CAB RID: 31915 RVA: 0x0032ED94 File Offset: 0x0032CF94
		private void BuildHeader()
		{
			Image image = OxGramUI.Image("Header", this._content, new Color(0.022f, 0.037f, 0.047f, 1f), null);
			RectTransform rectTransform = image.rectTransform;
			rectTransform.anchorMin = new Vector2(0f, 1f);
			rectTransform.anchorMax = new Vector2(1f, 1f);
			rectTransform.pivot = new Vector2(0.5f, 1f);
			rectTransform.sizeDelta = new Vector2(0f, 94f);
			rectTransform.anchoredPosition = Vector2.zero;
			TMP_Text tmp_Text = this.CreateText("Title", image.transform, "HACKME", 26f, Color.white, TextAlignmentOptions.Center);
			OxGramUI.Stretch(tmp_Text.rectTransform, 112f, 74f, 15f, 34f);
			tmp_Text.fontStyle = FontStyles.Bold;
			tmp_Text.raycastTarget = false;
			TMP_Text tmp_Text2 = this.CreateLocalizedText("Subtitle", image.transform, "STORECAST", 10f, HackMePhoneApp.Neon, TextAlignmentOptions.Center);
			OxGramUI.Stretch(tmp_Text2.rectTransform, 112f, 74f, 57f, 15f);
			tmp_Text2.characterSpacing = 8f;
			tmp_Text2.raycastTarget = false;
			TMP_Text tmp_Text3 = this.CreateText("Terminal", image.transform, ">_", 22f, HackMePhoneApp.Neon, TextAlignmentOptions.Center);
			RectTransform rectTransform2 = tmp_Text3.rectTransform;
			Vector2 vector = new Vector2(1f, 0.5f);
			rectTransform2.anchorMax = vector;
			rectTransform2.anchorMin = vector;
			rectTransform2.pivot = new Vector2(1f, 0.5f);
			rectTransform2.anchoredPosition = new Vector2(-18f, -4f);
			rectTransform2.sizeDelta = new Vector2(48f, 46f);
			tmp_Text3.fontStyle = FontStyles.Bold;
			tmp_Text3.raycastTarget = false;
			Sprite nativeBackSprite = this.FindNativeBackSprite();
			if (nativeBackSprite != null)
			{
				Image backImage = OxGramUI.Image("Back", image.transform, Color.white, null);
				RectTransform rectTransform3 = backImage.rectTransform;
				RectTransform rectTransform4 = rectTransform3;
				RectTransform rectTransform5 = rectTransform3;
				vector = new Vector2(0f, 0.5f);
				rectTransform5.anchorMax = vector;
				rectTransform4.anchorMin = vector;
				rectTransform3.pivot = new Vector2(0f, 0.5f);
				rectTransform3.anchoredPosition = new Vector2(12f, -4f);
				rectTransform3.sizeDelta = new Vector2(104f, 50f);
				backImage.sprite = nativeBackSprite;
				backImage.type = Image.Type.Simple;
				backImage.preserveAspect = true;
				Button backButton = OxGramUI.Button(backImage.gameObject);
				backButton.transition = Selectable.Transition.ColorTint;
				backButton.targetGraphic = backImage;
				backButton.onClick.AddListener(new UnityAction(this.CloseApp));
				TMP_Text localizedBack = this.CreateText("LocalizedBack", rectTransform3, "", 18f, HackMePhoneApp.LinkBlue, TextAlignmentOptions.Center);
				OxGramUI.Stretch(localizedBack.rectTransform, 0f, 0f, 0f, 0f);
				LocalizedGameText.BindComputed(localizedBack, delegate
				{
					bool flag = GameLocalization.CurrentCode == "en";
					backImage.sprite = (flag ? nativeBackSprite : null);
					backImage.color = (flag ? Color.white : Color.clear);
					backButton.targetGraphic = (flag ? backImage : localizedBack);
					if (!flag)
					{
						return GameLocalization.GetSource("hackme_ui", "< Back");
					}
					return "";
				}).FitTranslationToRectangle(false);
			}
			else
			{
				Image image2 = OxGramUI.Image("Back", image.transform, new Color(0f, 0f, 0f, 0f), null);
				RectTransform rectTransform6 = image2.rectTransform;
				RectTransform rectTransform7 = rectTransform6;
				RectTransform rectTransform8 = rectTransform6;
				vector = new Vector2(0f, 0.5f);
				rectTransform8.anchorMax = vector;
				rectTransform7.anchorMin = vector;
				rectTransform6.pivot = new Vector2(0f, 0.5f);
				rectTransform6.anchoredPosition = new Vector2(12f, -4f);
				rectTransform6.sizeDelta = new Vector2(104f, 50f);
				TMP_Text tmp_Text4 = this.CreateLocalizedText("Label", rectTransform6, "< Back", 18f, HackMePhoneApp.LinkBlue, TextAlignmentOptions.Center);
				OxGramUI.Stretch(tmp_Text4.rectTransform, 0f, 0f, 0f, 0f);
				tmp_Text4.raycastTarget = false;
				OxGramUI.Button(image2.gameObject).onClick.AddListener(new UnityAction(this.CloseApp));
			}
			Image image3 = OxGramUI.Image("Divider", image.transform, HackMePhoneApp.LinkBlue, null);
			RectTransform rectTransform9 = image3.rectTransform;
			rectTransform9.anchorMin = new Vector2(0f, 0f);
			rectTransform9.anchorMax = new Vector2(1f, 0f);
			rectTransform9.pivot = new Vector2(0.5f, 0f);
			rectTransform9.sizeDelta = new Vector2(0f, 2f);
			rectTransform9.anchoredPosition = Vector2.zero;
			image3.raycastTarget = false;
		}

		// Token: 0x06007CAC RID: 31916 RVA: 0x0032F278 File Offset: 0x0032D478
		private Sprite FindNativeBackSprite()
		{
			Transform parent = base.transform.parent;
			if (parent == null)
			{
				return null;
			}
			foreach (Image image in parent.GetComponentsInChildren<Image>(true))
			{
				if (!(image == null) && !(image.transform == base.transform) && !(image.name != "Back") && !(image.sprite == null))
				{
					return image.sprite;
				}
			}
			return null;
		}

		// Token: 0x06007CAD RID: 31917 RVA: 0x0032F2FC File Offset: 0x0032D4FC
		private void BuildSignalPanel()
		{
			Image image = OxGramUI.Image("Signal", this._content, HackMePhoneApp.Panel, PhoneUiSpriteFactory.Rounded);
			RectTransform rectTransform = image.rectTransform;
			rectTransform.anchorMin = new Vector2(0f, 1f);
			rectTransform.anchorMax = new Vector2(1f, 1f);
			rectTransform.pivot = new Vector2(0.5f, 1f);
			rectTransform.anchoredPosition = new Vector2(0f, -112f);
			rectTransform.sizeDelta = new Vector2(-32f, 104f);
			HackMePhoneApp.AddOutline(image.gameObject, new Color(0.08f, 0.55f, 1f, 0.28f), 1.5f);
			this._signalDot = OxGramUI.Image("SignalDot", rectTransform, HackMePhoneApp.Neon, PhoneUiSpriteFactory.Circle);
			RectTransform rectTransform2 = this._signalDot.rectTransform;
			Vector2 vector = new Vector2(0f, 1f);
			rectTransform2.anchorMax = vector;
			rectTransform2.anchorMin = vector;
			rectTransform2.pivot = new Vector2(0f, 1f);
			rectTransform2.anchoredPosition = new Vector2(18f, -19f);
			rectTransform2.sizeDelta = new Vector2(16f, 16f);
			this._signalDot.raycastTarget = false;
			this._signalText = this.CreateLocalizedText("StoreSignal", rectTransform, "SCANNING STORE...", 19f, HackMePhoneApp.Neon, TextAlignmentOptions.TopLeft);
			OxGramUI.Stretch(this._signalText.rectTransform, 46f, 18f, 13f, 54f);
			this._signalText.fontStyle = FontStyles.Bold;
			this._signalText.raycastTarget = false;
			this._resultText = this.CreateLocalizedText("Result", rectTransform, "Select gallery media to overwrite nearby displays.", 14f, HackMePhoneApp.Muted, TextAlignmentOptions.BottomLeft);
			OxGramUI.Stretch(this._resultText.rectTransform, 18f, 18f, 53f, 12f);
			this._resultText.raycastTarget = false;
		}

		// Token: 0x06007CAE RID: 31918 RVA: 0x0032F504 File Offset: 0x0032D704
		private void BuildGallery()
		{
			TMP_Text tmp_Text = this.CreateLocalizedText("GalleryHeading", this._content, "GALLERY PAYLOADS", 17f, Color.white, TextAlignmentOptions.Left);
			RectTransform rectTransform = tmp_Text.rectTransform;
			rectTransform.anchorMin = new Vector2(0f, 1f);
			rectTransform.anchorMax = new Vector2(1f, 1f);
			rectTransform.pivot = new Vector2(0.5f, 1f);
			rectTransform.anchoredPosition = new Vector2(0f, -230f);
			rectTransform.sizeDelta = new Vector2(-32f, 32f);
			tmp_Text.fontStyle = FontStyles.Bold;
			tmp_Text.raycastTarget = false;
			TMP_Text tmp_Text2 = this.CreateLocalizedText("GalleryHint", this._content, "CHOOSE A TYPE", 11f, HackMePhoneApp.LinkBlue, TextAlignmentOptions.Right);
			RectTransform rectTransform2 = tmp_Text2.rectTransform;
			rectTransform2.anchorMin = new Vector2(0f, 1f);
			rectTransform2.anchorMax = new Vector2(1f, 1f);
			rectTransform2.pivot = new Vector2(0.5f, 1f);
			rectTransform2.anchoredPosition = new Vector2(0f, -233f);
			rectTransform2.sizeDelta = new Vector2(-32f, 28f);
			tmp_Text2.fontStyle = FontStyles.Bold;
			tmp_Text2.raycastTarget = false;
			RectTransform rectTransform3 = OxGramUI.Rect("MediaTabs", this._content);
			RectTransform rectTransform4 = rectTransform3;
			RectTransform rectTransform5 = rectTransform3;
			Vector2 vector = new Vector2(0.5f, 1f);
			rectTransform5.anchorMax = vector;
			rectTransform4.anchorMin = vector;
			rectTransform3.pivot = new Vector2(0.5f, 1f);
			rectTransform3.anchoredPosition = new Vector2(0f, -266f);
			rectTransform3.sizeDelta = new Vector2(448f, 42f);
			this._photoTab = this.BuildMediaTab(rectTransform3, "PhotosTab", "PHOTOS", true, delegate
			{
				this.SelectMediaTab(HackMePhoneApp.PayloadKind.Photo);
			}, out this._photoTabText);
			this._videoTab = this.BuildMediaTab(rectTransform3, "VideosTab", "VIDEOS", false, delegate
			{
				this.SelectMediaTab(HackMePhoneApp.PayloadKind.Video);
			}, out this._videoTabText);
			RectTransform rectTransform6 = OxGramUI.Rect("Gallery", this._content);
			OxGramUI.Stretch(rectTransform6, 16f, 16f, 320f, 18f);
			this._galleryScroll = rectTransform6.gameObject.AddComponent<ScrollRect>();
			this._galleryScroll.horizontal = false;
			this._galleryScroll.vertical = true;
			this._galleryScroll.movementType = ScrollRect.MovementType.Clamped;
			this._galleryScroll.scrollSensitivity = 24f;
			Image image = OxGramUI.Image("Viewport", rectTransform6, new Color(0f, 0f, 0f, 0f), null);
			OxGramUI.Stretch(image.rectTransform, 0f, 0f, 0f, 0f);
			image.gameObject.AddComponent<RectMask2D>();
			this._galleryScroll.viewport = image.rectTransform;
			this._photoGrid = this.BuildPayloadGrid("PhotoGrid", image.transform);
			this._videoGrid = this.BuildPayloadGrid("VideoGrid", image.transform);
			this._photoEmptyState = this.CreateLocalizedText("NoPhotos", image.transform, "NO PHOTOS FOUND\nTake a photo with Camera first.", 19f, HackMePhoneApp.Muted, TextAlignmentOptions.Center).gameObject;
			OxGramUI.Stretch((RectTransform)this._photoEmptyState.transform, 30f, 30f, 120f, 120f);
			this._videoEmptyState = this.CreateLocalizedText("NoVideos", image.transform, "NO VIDEOS FOUND\nRecord a video with Camera first.", 19f, HackMePhoneApp.Muted, TextAlignmentOptions.Center).gameObject;
			OxGramUI.Stretch((RectTransform)this._videoEmptyState.transform, 30f, 30f, 120f, 120f);
			this._photoEmptyState.SetActive(false);
			this._videoEmptyState.SetActive(false);
			this.SelectMediaTab(HackMePhoneApp.PayloadKind.Photo);
		}

		// Token: 0x06007CAF RID: 31919 RVA: 0x0032F8EC File Offset: 0x0032DAEC
		private RectTransform BuildPayloadGrid(string name, Transform parent)
		{
			RectTransform rectTransform = OxGramUI.Rect(name, parent);
			rectTransform.anchorMin = new Vector2(0f, 1f);
			rectTransform.anchorMax = new Vector2(1f, 1f);
			rectTransform.pivot = new Vector2(0.5f, 1f);
			rectTransform.sizeDelta = Vector2.zero;
			GridLayoutGroup gridLayoutGroup = rectTransform.gameObject.AddComponent<GridLayoutGroup>();
			gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
			gridLayoutGroup.constraintCount = 2;
			gridLayoutGroup.cellSize = new Vector2(216f, 198f);
			gridLayoutGroup.spacing = new Vector2(10f, 12f);
			gridLayoutGroup.padding = new RectOffset(3, 3, 0, 8);
			gridLayoutGroup.childAlignment = TextAnchor.UpperCenter;
			rectTransform.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			return rectTransform;
		}

		// Token: 0x06007CB0 RID: 31920 RVA: 0x0032F9B4 File Offset: 0x0032DBB4
		private Image BuildMediaTab(Transform parent, string name, string label, bool left, Action onClick, out TMP_Text tabText)
		{
			Image image = OxGramUI.Image(name, parent, HackMePhoneApp.PanelRaised, null);
			RectTransform rectTransform = image.rectTransform;
			rectTransform.anchorMin = new Vector2(left ? 0f : 0.5f, 0f);
			rectTransform.anchorMax = new Vector2(left ? 0.5f : 1f, 1f);
			rectTransform.offsetMin = new Vector2(left ? 0f : 3f, 0f);
			rectTransform.offsetMax = new Vector2(left ? (-3f) : 0f, 0f);
			image.type = Image.Type.Simple;
			tabText = this.CreateLocalizedText("Label", image.transform, label, 14f, HackMePhoneApp.Muted, TextAlignmentOptions.Center);
			OxGramUI.Stretch(tabText.rectTransform, 6f, 6f, 4f, 4f);
			tabText.fontStyle = FontStyles.Bold;
			tabText.raycastTarget = false;
			Button button = OxGramUI.Button(image.gameObject);
			button.targetGraphic = image;
			button.transition = Selectable.Transition.ColorTint;
			button.onClick.AddListener(delegate
			{
				onClick();
			});
			return image;
		}

		// Token: 0x06007CB1 RID: 31921 RVA: 0x0032FAF4 File Offset: 0x0032DCF4
		private void SelectMediaTab(HackMePhoneApp.PayloadKind kind)
		{
			this._selectedPayloadKind = kind;
			bool flag = kind == HackMePhoneApp.PayloadKind.Photo;
			if (this._photoGrid != null)
			{
				this._photoGrid.gameObject.SetActive(flag);
			}
			if (this._videoGrid != null)
			{
				this._videoGrid.gameObject.SetActive(!flag);
			}
			if (this._galleryScroll != null)
			{
				this._galleryScroll.content = (flag ? this._photoGrid : this._videoGrid);
				this._galleryScroll.verticalNormalizedPosition = 1f;
			}
			if (this._photoTab != null)
			{
				this._photoTab.color = (flag ? HackMePhoneApp.Neon : HackMePhoneApp.PanelRaised);
			}
			if (this._videoTab != null)
			{
				this._videoTab.color = (flag ? HackMePhoneApp.PanelRaised : HackMePhoneApp.Neon);
			}
			this.UpdateTabLabels();
			this.UpdateEmptyStates();
		}

		// Token: 0x06007CB2 RID: 31922 RVA: 0x0032FBE4 File Offset: 0x0032DDE4
		private void UpdateTabLabels()
		{
			if (this._photoTabText != null)
			{
				HackMePhoneApp.SetFormat(this._photoTabText, "PHOTOS  {0}", new object[] { this._loadedPhotoCount });
				this._photoTabText.color = ((this._selectedPayloadKind == HackMePhoneApp.PayloadKind.Photo) ? HackMePhoneApp.Background : HackMePhoneApp.Muted);
			}
			if (this._videoTabText != null)
			{
				HackMePhoneApp.SetFormat(this._videoTabText, "VIDEOS  {0}", new object[] { this._loadedVideoCount });
				this._videoTabText.color = ((this._selectedPayloadKind == HackMePhoneApp.PayloadKind.Video) ? HackMePhoneApp.Background : HackMePhoneApp.Muted);
			}
		}

		// Token: 0x06007CB3 RID: 31923 RVA: 0x0032FC94 File Offset: 0x0032DE94
		private void UpdateEmptyStates()
		{
			bool flag = !this._galleryLoading;
			if (this._photoEmptyState != null)
			{
				this._photoEmptyState.SetActive(this._selectedPayloadKind == HackMePhoneApp.PayloadKind.Photo && flag && this._loadedPhotoCount == 0);
			}
			if (this._videoEmptyState != null)
			{
				this._videoEmptyState.SetActive(this._selectedPayloadKind == HackMePhoneApp.PayloadKind.Video && flag && this._loadedVideoCount == 0);
			}
		}

		// Token: 0x06007CB4 RID: 31924 RVA: 0x0032FD0F File Offset: 0x0032DF0F
		private TMP_Text CreateText(string name, Transform parent, string value, float size, Color color, TextAlignmentOptions alignment)
		{
			return OxGramUI.Text(name, parent, value, this._font, size, color, alignment);
		}

		// Token: 0x06007CB5 RID: 31925 RVA: 0x0032FD25 File Offset: 0x0032DF25
		private TMP_Text CreateLocalizedText(string name, Transform parent, string english, float size, Color color, TextAlignmentOptions alignment)
		{
			TMP_Text tmp_Text = this.CreateText(name, parent, english, size, color, alignment);
			HackMePhoneApp.SetSource(tmp_Text, english);
			return tmp_Text;
		}

		// Token: 0x06007CB6 RID: 31926 RVA: 0x0032FD3D File Offset: 0x0032DF3D
		private static void SetSource(TMP_Text label, string english)
		{
			LocalizedGameText localizedGameText = LocalizedGameText.BindSource(label, "hackme_ui", english, null);
			if (localizedGameText == null)
			{
				return;
			}
			localizedGameText.FitTranslationToRectangle(false);
		}

		// Token: 0x06007CB7 RID: 31927 RVA: 0x0032FD57 File Offset: 0x0032DF57
		private static void SetFormat(TMP_Text label, string english, params object[] arguments)
		{
			LocalizedGameText localizedGameText = LocalizedGameText.BindFormat(label, "hackme_ui", english, arguments);
			if (localizedGameText == null)
			{
				return;
			}
			localizedGameText.FitTranslationToRectangle(false);
		}

		// Token: 0x06007CB8 RID: 31928 RVA: 0x0032FD71 File Offset: 0x0032DF71
		private static string StoreLabel(string source)
		{
			if (source == "Area_Electronic" || source == "Area_Electronic (1)" || source == "Area_ElectronicR")
			{
				return GameLocalization.GetSource("hackme_ui", source);
			}
			return source;
		}

		// Token: 0x06007CB9 RID: 31929 RVA: 0x0032FDA8 File Offset: 0x0032DFA8
		private static void SetStoreLabel(TMP_Text label, string englishTemplate, string storeName)
		{
			LocalizedGameText localizedGameText = LocalizedGameText.BindComputed(label, () => GameLocalization.FormatSource("hackme_ui", englishTemplate, new object[] { HackMePhoneApp.StoreLabel(storeName).ToUpperInvariant() }));
			if (localizedGameText == null)
			{
				return;
			}
			localizedGameText.FitTranslationToRectangle(false);
		}

		// Token: 0x06007CBA RID: 31930 RVA: 0x0032FDE8 File Offset: 0x0032DFE8
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
					else if ((transform.name.StartsWith("m", StringComparison.Ordinal) || transform.GetComponent<Animator>() != null || transform.GetComponent<PhotoGallery>() != null || transform.GetComponent<OxGramApp>() != null) && transform.gameObject.activeSelf)
					{
						transform.gameObject.SetActive(false);
					}
				}
			}
		}

		// Token: 0x17001421 RID: 5153
		// (get) Token: 0x06007CBB RID: 31931 RVA: 0x0032FEE8 File Offset: 0x0032E0E8
		private Vector3 PlayerPosition
		{
			get
			{
				if (this._owner == null)
				{
					return base.transform.root.position;
				}
				if (this._owner.playerAnimator != null)
				{
					return this._owner.playerAnimator.transform.position;
				}
				return this._owner.transform.position;
			}
		}

		// Token: 0x06007CBC RID: 31932 RVA: 0x0032FF50 File Offset: 0x0032E150
		private HackMeStoreZone RefreshStoreSignal()
		{
			HackMeStoreZone hackMeStoreZone = HackMeStoreRegistry.FindStoreContaining(this.PlayerPosition);
			if (this._signalText == null)
			{
				return hackMeStoreZone;
			}
			if (hackMeStoreZone == null)
			{
				HackMePhoneApp.SetSource(this._signalText, "NO COMPATIBLE STORE SIGNAL");
				this._signalText.color = HackMePhoneApp.Error;
				if (this._signalDot != null)
				{
					this._signalDot.color = HackMePhoneApp.Error;
				}
				HackMePhoneApp.SetSource(this._resultText, "Enter an electronics store before selecting a payload.");
				this._resultText.color = HackMePhoneApp.Muted;
			}
			else
			{
				HackMePhoneApp.SetStoreLabel(this._signalText, "CONNECTED // {0}", hackMeStoreZone.DisplayName);
				this._signalText.color = HackMePhoneApp.Neon;
				if (this._signalDot != null)
				{
					this._signalDot.color = HackMePhoneApp.Neon;
				}
				HackMePhoneApp.SetSource(this._resultText, hackMeStoreZone.IsSingleScreen ? "Target locked. Only this screen will be affected." : "Target locked. Only this store will be affected.");
				this._resultText.color = HackMePhoneApp.Muted;
			}
			return hackMeStoreZone;
		}

		// Token: 0x06007CBD RID: 31933 RVA: 0x00330058 File Offset: 0x0032E258
		private void ShowPayloadPreview(HackMePhoneApp.GalleryPayload payload)
		{
			if (payload == null || payload.thumbnail == null || this._hackInProgress)
			{
				return;
			}
			this.ClosePhotoPreview();
			HackMeStoreZone hackMeStoreZone = this.RefreshStoreSignal();
			this.PlayCue(HackMePhoneApp.CyberCue.Preview);
			this._previewOverlay = OxGramUI.Rect("PayloadPreview", this._content);
			OxGramUI.Stretch(this._previewOverlay, 0f, 0f, 0f, 0f);
			this._previewOverlay.SetAsLastSibling();
			CanvasGroup canvasGroup = this._previewOverlay.gameObject.AddComponent<CanvasGroup>();
			canvasGroup.alpha = 0f;
			Image image = OxGramUI.Image("Dim", this._previewOverlay, new Color(0f, 0.015f, 0.025f, 0.88f), null);
			OxGramUI.Stretch(image.rectTransform, 0f, 0f, 0f, 0f);
			OxGramUI.Button(image.gameObject).onClick.AddListener(new UnityAction(this.ClosePhotoPreview));
			Image image2 = OxGramUI.Image("PreviewCard", this._previewOverlay, HackMePhoneApp.PanelRaised, PhoneUiSpriteFactory.Rounded);
			RectTransform rectTransform = image2.rectTransform;
			RectTransform rectTransform2 = rectTransform;
			RectTransform rectTransform3 = rectTransform;
			Vector2 vector = new Vector2(0.5f, 0.5f);
			rectTransform3.anchorMax = vector;
			rectTransform2.anchorMin = vector;
			rectTransform.pivot = new Vector2(0.5f, 0.5f);
			rectTransform.sizeDelta = new Vector2(432f, 800f);
			HackMePhoneApp.AddOutline(image2.gameObject, new Color(0.24f, 1f, 0.53f, 0.62f), 2f);
			TMP_Text tmp_Text = this.CreateLocalizedText("Title", image2.transform, payload.IsVideo ? "VIDEO PAYLOAD" : "PHOTO PAYLOAD", 22f, Color.white, TextAlignmentOptions.Center);
			HackMePhoneApp.SetTopRect(tmp_Text.rectTransform, 18f, 36f, 20f);
			tmp_Text.fontStyle = FontStyles.Bold;
			tmp_Text.raycastTarget = false;
			string text = ((hackMeStoreZone != null) ? ("TARGET  //  " + hackMeStoreZone.DisplayName.ToUpperInvariant()) : "TARGET  //  NO STORE SIGNAL");
			TMP_Text tmp_Text2 = this.CreateText("Target", image2.transform, text, 12f, (hackMeStoreZone != null) ? HackMePhoneApp.Neon : HackMePhoneApp.Error, TextAlignmentOptions.Center);
			if (hackMeStoreZone != null)
			{
				HackMePhoneApp.SetStoreLabel(tmp_Text2, "TARGET  //  {0}", hackMeStoreZone.DisplayName);
			}
			else
			{
				HackMePhoneApp.SetSource(tmp_Text2, "TARGET  //  NO STORE SIGNAL");
			}
			HackMePhoneApp.SetTopRect(tmp_Text2.rectTransform, 60f, 28f, 20f);
			tmp_Text2.fontStyle = FontStyles.Bold;
			tmp_Text2.raycastTarget = false;
			Image image3 = OxGramUI.Image("MediaFrame", image2.transform, new Color(0.008f, 0.012f, 0.014f, 1f), PhoneUiSpriteFactory.Rounded);
			RectTransform rectTransform4 = image3.rectTransform;
			vector = new Vector2(0.5f, 1f);
			rectTransform4.anchorMax = vector;
			rectTransform4.anchorMin = vector;
			rectTransform4.pivot = new Vector2(0.5f, 1f);
			rectTransform4.anchoredPosition = new Vector2(0f, -98f);
			rectTransform4.sizeDelta = new Vector2(392f, 520f);
			HackMePhoneApp.AddOutline(image3.gameObject, new Color(0.08f, 0.55f, 1f, 0.42f), 1f);
			image3.gameObject.AddComponent<RectMask2D>();
			if (payload.IsVideo)
			{
				this.BuildVideoPreview(image3.transform, payload);
			}
			else
			{
				this.BuildPhotoPreview(image3.transform, payload.thumbnail);
			}
			string text2 = ((payload.displayName.Length > 34) ? (payload.displayName.Substring(0, 34) + "...") : payload.displayName);
			TMP_Text tmp_Text3 = this.CreateText("FileName", image2.transform, text2, 12f, HackMePhoneApp.Muted, TextAlignmentOptions.Center);
			HackMePhoneApp.SetTopRect(tmp_Text3.rectTransform, 624f, 24f, 24f);
			tmp_Text3.raycastTarget = false;
			TMP_Text tmp_Text4 = this.CreateLocalizedText("Confirmation", image2.transform, (hackMeStoreZone != null) ? (hackMeStoreZone.IsSingleScreen ? ("Broadcast this " + (payload.IsVideo ? "video" : "photo") + " to the connected screen?") : ("Broadcast this " + (payload.IsVideo ? "video" : "photo") + " to every screen in the connected store?")) : "Move inside a compatible electronics store before starting the hack.", 15f, (hackMeStoreZone != null) ? Color.white : HackMePhoneApp.Error, TextAlignmentOptions.Center);
			HackMePhoneApp.SetTopRect(tmp_Text4.rectTransform, 652f, 58f, 26f);
			tmp_Text4.raycastTarget = false;
			this.BuildModalButton(image2.transform, "Cancel", "CANCEL", new Vector2(-101f, 22f), new Vector2(190f, 54f), new Color(1f, 1f, 1f, 0.09f), Color.white, new Action(this.ClosePhotoPreview), true);
			this.BuildModalButton(image2.transform, "Hack", (hackMeStoreZone != null && hackMeStoreZone.IsSingleScreen) ? "HACK SCREEN" : "HACK STORE", new Vector2(101f, 22f), new Vector2(190f, 54f), (hackMeStoreZone != null) ? HackMePhoneApp.Neon : new Color(0.16f, 0.25f, 0.2f, 1f), (hackMeStoreZone != null) ? HackMePhoneApp.Background : HackMePhoneApp.Muted, delegate
			{
				this.BeginHack(payload);
			}, hackMeStoreZone != null);
			base.StartCoroutine(HackMePhoneApp.AnimateModal(canvasGroup, rectTransform));
		}

		// Token: 0x06007CBE RID: 31934 RVA: 0x00330674 File Offset: 0x0032E874
		private void BuildPhotoPreview(Transform frame, Sprite sprite)
		{
			Image image = OxGramUI.Image("Photo", frame, Color.white, null);
			OxGramUI.Stretch(image.rectTransform, 8f, 8f, 8f, 8f);
			image.sprite = sprite;
			StreamerCensorImage.Bind(image, false);
			image.preserveAspect = true;
			image.raycastTarget = false;
		}

		// Token: 0x06007CBF RID: 31935 RVA: 0x003306D0 File Offset: 0x0032E8D0
		private void BuildVideoPreview(Transform frame, HackMePhoneApp.GalleryPayload payload)
		{
			RectTransform rectTransform = OxGramUI.Rect("VideoViewport", frame);
			OxGramUI.Stretch(rectTransform, 8f, 8f, 8f, 8f);
			RawImage video = rectTransform.gameObject.AddComponent<RawImage>();
			video.color = Color.white;
			video.texture = payload.thumbnail.texture;
			StreamerCensorImage.Bind(video, false);
			video.raycastTarget = false;
			AspectRatioFitter aspect = rectTransform.gameObject.AddComponent<AspectRatioFitter>();
			aspect.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
			aspect.aspectRatio = payload.thumbnail.rect.width / Mathf.Max(1f, payload.thumbnail.rect.height);
			TMP_Text loading = this.CreateLocalizedText("Loading", frame, "LOADING VIDEO...", 15f, HackMePhoneApp.Neon, TextAlignmentOptions.Center);
			OxGramUI.Stretch(loading.rectTransform, 20f, 20f, 20f, 20f);
			loading.fontStyle = FontStyles.Bold;
			loading.raycastTarget = false;
			TMP_Text tmp_Text = this.CreateText("VideoBadge", frame, "VIDEO  //  " + HackMePhoneApp.FormatDuration(payload.durationSeconds), 11f, Color.white, TextAlignmentOptions.Center);
			HackMePhoneApp.SetFormat(tmp_Text, "VIDEO  //  {0}", new object[] { HackMePhoneApp.FormatDuration(payload.durationSeconds) });
			RectTransform rectTransform2 = tmp_Text.rectTransform;
			Vector2 vector = new Vector2(1f, 1f);
			rectTransform2.anchorMax = vector;
			rectTransform2.anchorMin = vector;
			rectTransform2.pivot = new Vector2(1f, 1f);
			rectTransform2.anchoredPosition = new Vector2(-14f, -14f);
			rectTransform2.sizeDelta = new Vector2(126f, 25f);
			tmp_Text.fontStyle = FontStyles.Bold;
			tmp_Text.raycastTarget = false;
			VideoPlayer videoPlayer = this._previewOverlay.gameObject.AddComponent<VideoPlayer>();
			videoPlayer.playOnAwake = false;
			videoPlayer.source = VideoSource.Url;
			videoPlayer.renderMode = VideoRenderMode.APIOnly;
			videoPlayer.audioOutputMode = VideoAudioOutputMode.None;
			videoPlayer.waitForFirstFrame = true;
			videoPlayer.skipOnDrop = true;
			videoPlayer.isLooping = true;
			videoPlayer.url = payload.path;
			videoPlayer.prepareCompleted += delegate(VideoPlayer source)
			{
				if (video == null || loading == null || source.texture == null)
				{
					return;
				}
				video.texture = source.texture;
				aspect.aspectRatio = ((source.height > 0U) ? Mathf.Max(0.1f, source.width / source.height) : aspect.aspectRatio);
				loading.gameObject.SetActive(false);
				source.Play();
			};
			videoPlayer.errorReceived += delegate(VideoPlayer source, string message)
			{
				if (loading == null)
				{
					return;
				}
				HackMePhoneApp.SetSource(loading, "VIDEO UNAVAILABLE");
				loading.color = HackMePhoneApp.Error;
				Debug.LogWarning("[HackMe] Could not preview '" + payload.path + "': " + message, this);
			};
			videoPlayer.Prepare();
		}

		// Token: 0x06007CC0 RID: 31936 RVA: 0x0033097C File Offset: 0x0032EB7C
		private void BeginHack(HackMePhoneApp.GalleryPayload payload)
		{
			if (this._hackInProgress || payload == null || payload.thumbnail == null)
			{
				return;
			}
			if (payload.IsVideo && !File.Exists(payload.path))
			{
				this.PlayCue(HackMePhoneApp.CyberCue.Failure);
				return;
			}
			if (!payload.IsVideo && payload.thumbnail.texture == null)
			{
				return;
			}
			HackMeStoreZone hackMeStoreZone = this.RefreshStoreSignal();
			if (hackMeStoreZone == null)
			{
				this.PlayCue(HackMePhoneApp.CyberCue.Failure);
				return;
			}
			this.ClosePhotoPreview();
			this._hackRoutine = base.StartCoroutine(this.RunHackSequence(hackMeStoreZone, payload));
		}

		// Token: 0x06007CC1 RID: 31937 RVA: 0x00330A0E File Offset: 0x0032EC0E
		private IEnumerator RunHackSequence(HackMeStoreZone store, HackMePhoneApp.GalleryPayload payload)
		{
			this.SetHackInProgress(true);
			TMP_Text status;
			TMP_Text detail;
			Image progress;
			this.ShowHackOverlay(out status, out detail, out progress);
			HackMePhoneApp.SetSource(this._resultText, "INJECTING PAYLOAD...");
			this._resultText.color = HackMePhoneApp.Neon;
			this.PlayCue(HackMePhoneApp.CyberCue.Hack);
			string[] steps = new string[] { "ESTABLISHING HANDSHAKE...", "BYPASSING DISPLAY BUS...", "UPLOADING PAYLOAD...", "SYNCING ALL PANELS..." };
			int num;
			for (int step = 0; step < steps.Length; step = num + 1)
			{
				if (status != null)
				{
					HackMePhoneApp.SetSource(status, steps[step]);
				}
				if (detail != null)
				{
					HackMePhoneApp.SetFormat(detail, "PACKET {0:00}/{1:00}", new object[]
					{
						step + 1,
						steps.Length
					});
				}
				float from = (float)step / (float)steps.Length;
				float to = ((float)step + 1f) / (float)steps.Length;
				float elapsed = 0f;
				while (elapsed < 0.24f)
				{
					elapsed += Time.unscaledDeltaTime;
					if (progress != null)
					{
						HackMePhoneApp.SetHackProgress(progress, Mathf.Lerp(from, to, Mathf.Clamp01(elapsed / 0.24f)));
					}
					yield return null;
				}
				num = step;
			}
			int num2 = 0;
			if (store != null && payload != null)
			{
				num2 = (payload.IsVideo ? store.ApplyVideo(payload.path) : store.ApplyWallpaper(payload.thumbnail.texture, payload.path));
			}
			if (num2 > 0)
			{
				if (status != null)
				{
					HackMePhoneApp.SetSource(status, "HACK COMPLETE");
					status.color = HackMePhoneApp.Neon;
				}
				if (detail != null)
				{
					HackMePhoneApp.SetFormat(detail, "{0} SCREENS UPDATED", new object[] { num2 });
				}
				if (progress != null)
				{
					HackMePhoneApp.SetHackProgress(progress, 1f);
					progress.color = HackMePhoneApp.Neon;
				}
				HackMePhoneApp.SetFormat(this._resultText, "HACK COMPLETE // {0} SCREENS UPDATED", new object[] { num2 });
				this._resultText.color = HackMePhoneApp.Neon;
				this.PlayCue(HackMePhoneApp.CyberCue.Success);
			}
			else
			{
				if (status != null)
				{
					HackMePhoneApp.SetSource(status, "INJECTION FAILED");
					status.color = HackMePhoneApp.Error;
				}
				if (detail != null)
				{
					HackMePhoneApp.SetSource(detail, "NO COMPATIBLE DISPLAYS FOUND");
				}
				if (progress != null)
				{
					progress.color = HackMePhoneApp.Error;
				}
				HackMePhoneApp.SetSource(this._resultText, "FAILED // NO COMPATIBLE DISPLAYS FOUND");
				this._resultText.color = HackMePhoneApp.Error;
				this.PlayCue(HackMePhoneApp.CyberCue.Failure);
			}
			yield return new WaitForSecondsRealtime(1.15f);
			this.CloseHackOverlay();
			this.SetHackInProgress(false);
			this._hackRoutine = null;
			yield break;
		}

		// Token: 0x06007CC2 RID: 31938 RVA: 0x00330A2C File Offset: 0x0032EC2C
		private void ShowHackOverlay(out TMP_Text status, out TMP_Text detail, out Image progress)
		{
			this.CloseHackOverlay();
			this._hackOverlay = OxGramUI.Rect("HackProgress", this._content);
			OxGramUI.Stretch(this._hackOverlay, 0f, 0f, 0f, 0f);
			this._hackOverlay.SetAsLastSibling();
			Image image = OxGramUI.Image("Dim", this._hackOverlay, new Color(0f, 0.01f, 0.018f, 0.9f), null);
			OxGramUI.Stretch(image.rectTransform, 0f, 0f, 0f, 0f);
			OxGramUI.Button(image.gameObject);
			Image image2 = OxGramUI.Image("Card", this._hackOverlay, HackMePhoneApp.PanelRaised, PhoneUiSpriteFactory.Rounded);
			RectTransform rectTransform = image2.rectTransform;
			Vector2 vector = new Vector2(0.5f, 0.5f);
			rectTransform.anchorMax = vector;
			rectTransform.anchorMin = vector;
			rectTransform.pivot = new Vector2(0.5f, 0.5f);
			rectTransform.sizeDelta = new Vector2(402f, 330f);
			HackMePhoneApp.AddOutline(image2.gameObject, new Color(0.24f, 1f, 0.53f, 0.72f), 2f);
			TMP_Text tmp_Text = this.CreateText("Glyph", image2.transform, ">_", 50f, HackMePhoneApp.Neon, TextAlignmentOptions.Center);
			HackMePhoneApp.SetTopRect(tmp_Text.rectTransform, 24f, 68f, 20f);
			tmp_Text.fontStyle = FontStyles.Bold;
			tmp_Text.raycastTarget = false;
			status = this.CreateLocalizedText("Status", image2.transform, "ESTABLISHING HANDSHAKE...", 19f, Color.white, TextAlignmentOptions.Center);
			HackMePhoneApp.SetTopRect(status.rectTransform, 106f, 38f, 24f);
			status.fontStyle = FontStyles.Bold;
			status.raycastTarget = false;
			detail = this.CreateText("Detail", image2.transform, "PACKET 01/04", 12f, HackMePhoneApp.LinkBlue, TextAlignmentOptions.Center);
			HackMePhoneApp.SetFormat(detail, "PACKET {0:00}/{1:00}", new object[] { 1, 4 });
			HackMePhoneApp.SetTopRect(detail.rectTransform, 151f, 28f, 24f);
			detail.fontStyle = FontStyles.Bold;
			detail.raycastTarget = false;
			Image image3 = OxGramUI.Image("ProgressTrack", image2.transform, new Color(0f, 0f, 0f, 0.64f), null);
			RectTransform rectTransform2 = image3.rectTransform;
			vector = new Vector2(0.5f, 0f);
			rectTransform2.anchorMax = vector;
			rectTransform2.anchorMin = vector;
			rectTransform2.pivot = new Vector2(0.5f, 0f);
			rectTransform2.anchoredPosition = new Vector2(0f, 76f);
			rectTransform2.sizeDelta = new Vector2(342f, 16f);
			image3.type = Image.Type.Simple;
			image3.raycastTarget = false;
			HackMePhoneApp.AddOutline(image3.gameObject, new Color(HackMePhoneApp.LinkBlue.r, HackMePhoneApp.LinkBlue.g, HackMePhoneApp.LinkBlue.b, 0.72f), 1f);
			progress = OxGramUI.Image("Fill", image3.transform, HackMePhoneApp.LinkBlue, null);
			progress.type = Image.Type.Simple;
			HackMePhoneApp.SetHackProgress(progress, 0f);
			progress.raycastTarget = false;
			TMP_Text tmp_Text2 = this.CreateLocalizedText("Warning", image2.transform, "DO NOT CLOSE HACKME DURING TRANSMISSION", 10f, HackMePhoneApp.Muted, TextAlignmentOptions.Center);
			RectTransform rectTransform3 = tmp_Text2.rectTransform;
			rectTransform3.anchorMin = new Vector2(0f, 0f);
			rectTransform3.anchorMax = new Vector2(1f, 0f);
			rectTransform3.pivot = new Vector2(0.5f, 0f);
			rectTransform3.anchoredPosition = new Vector2(0f, 30f);
			rectTransform3.sizeDelta = new Vector2(-30f, 24f);
			tmp_Text2.raycastTarget = false;
		}

		// Token: 0x06007CC3 RID: 31939 RVA: 0x00330E1C File Offset: 0x0032F01C
		private static void SetHackProgress(Image progress, float value)
		{
			if (progress == null)
			{
				return;
			}
			RectTransform rectTransform = progress.rectTransform;
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = new Vector2(Mathf.Clamp01(value), 1f);
			rectTransform.pivot = new Vector2(0f, 0.5f);
			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = Vector2.zero;
		}

		// Token: 0x06007CC4 RID: 31940 RVA: 0x00330E84 File Offset: 0x0032F084
		private void ClosePhotoPreview()
		{
			if (this._previewOverlay == null)
			{
				return;
			}
			this._previewOverlay.gameObject.SetActive(false);
			Object.Destroy(this._previewOverlay.gameObject);
			this._previewOverlay = null;
		}

		// Token: 0x06007CC5 RID: 31941 RVA: 0x00330EBD File Offset: 0x0032F0BD
		private void CloseHackOverlay()
		{
			if (this._hackOverlay == null)
			{
				return;
			}
			this._hackOverlay.gameObject.SetActive(false);
			Object.Destroy(this._hackOverlay.gameObject);
			this._hackOverlay = null;
		}

		// Token: 0x06007CC6 RID: 31942 RVA: 0x00330EF8 File Offset: 0x0032F0F8
		private void CancelTransientUi()
		{
			if (this._hackRoutine != null)
			{
				base.StopCoroutine(this._hackRoutine);
				this._hackRoutine = null;
			}
			this.SetHackInProgress(false);
			if (this._audioSource != null)
			{
				this._audioSource.Stop();
			}
			this.ClosePhotoPreview();
			this.CloseHackOverlay();
		}

		// Token: 0x06007CC7 RID: 31943 RVA: 0x00330F4C File Offset: 0x0032F14C
		private void ReloadGallery()
		{
			if (this._loadRoutine != null)
			{
				base.StopCoroutine(this._loadRoutine);
			}
			int num = this._loadGeneration + 1;
			this._loadGeneration = num;
			int num2 = num;
			this._galleryLoading = true;
			this.ClearPhotoGrid();
			this._loadedPhotoCount = 0;
			this._loadedVideoCount = 0;
			this.UpdateTabLabels();
			this.UpdateEmptyStates();
			this._loadRoutine = base.StartCoroutine(this.LoadGallery(num2));
		}

		// Token: 0x06007CC8 RID: 31944 RVA: 0x00330FB9 File Offset: 0x0032F1B9
		private IEnumerator LoadGallery(int generation)
		{
			List<HackMePhoneApp.MediaFile> list = new List<HackMePhoneApp.MediaFile>();
			List<HackMePhoneApp.MediaFile> list2 = new List<HackMePhoneApp.MediaFile>();
			string imagesFolder = PhonePhotoStorage.ImagesFolder;
			if (Directory.Exists(imagesFolder))
			{
				string[] files2 = Directory.GetFiles(imagesFolder, "*.png");
				for (int j = 0; j < files2.Length; j++)
				{
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(files2[j]);
					int num;
					if (!int.TryParse(fileNameWithoutExtension, out num) && !PhonePhotoStorage.IsExcludedFromGallery(files2[j]) && !fileNameWithoutExtension.EndsWith(".censored", StringComparison.OrdinalIgnoreCase))
					{
						list.Add(new HackMePhoneApp.MediaFile(files2[j], HackMePhoneApp.PayloadKind.Photo));
					}
				}
			}
			string videosFolder = PhoneVideoStorage.VideosFolder;
			if (Directory.Exists(videosFolder))
			{
				string[] files3 = Directory.GetFiles(videosFolder);
				for (int k = 0; k < files3.Length; k++)
				{
					if (HackMePhoneApp.IsSupportedVideo(files3[k]))
					{
						list2.Add(new HackMePhoneApp.MediaFile(files3[k], HackMePhoneApp.PayloadKind.Video));
					}
				}
			}
			list.Sort((HackMePhoneApp.MediaFile left, HackMePhoneApp.MediaFile right) => right.modifiedUtc.CompareTo(left.modifiedUtc));
			list2.Sort((HackMePhoneApp.MediaFile left, HackMePhoneApp.MediaFile right) => right.modifiedUtc.CompareTo(left.modifiedUtc));
			List<HackMePhoneApp.MediaFile> files = new List<HackMePhoneApp.MediaFile>(list.Count + list2.Count);
			int num2 = Mathf.Max(list.Count, list2.Count);
			for (int l = 0; l < num2; l++)
			{
				if (l < list.Count)
				{
					files.Add(list[l]);
				}
				if (l < list2.Count)
				{
					files.Add(list2[l]);
				}
			}
			int i = 0;
			while (i < files.Count)
			{
				if (generation != this._loadGeneration)
				{
					yield break;
				}
				HackMePhoneApp.MediaFile media = files[i];
				string thumbnailPath = ((media.kind == HackMePhoneApp.PayloadKind.Video) ? PhoneVideoStorage.ThumbnailPathFor(media.path) : media.path);
				Sprite sprite = null;
				if (File.Exists(thumbnailPath))
				{
					goto IL_025B;
				}
				if (media.kind == HackMePhoneApp.PayloadKind.Video)
				{
					sprite = this.CreateVideoFallbackSprite(media.path);
					goto IL_025B;
				}
				IL_0473:
				int num = i;
				i = num + 1;
				continue;
				IL_025B:
				if (sprite == null)
				{
					string absoluteUri = new Uri(thumbnailPath).AbsoluteUri;
					using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(absoluteUri, true))
					{
						yield return request.SendWebRequest();
						if (generation != this._loadGeneration)
						{
							yield break;
						}
						if (request.result != UnityWebRequest.Result.Success)
						{
							Debug.LogWarning("[HackMe] Could not load media thumbnail '" + thumbnailPath + "': " + request.error, this);
							if (media.kind != HackMePhoneApp.PayloadKind.Video)
							{
								goto IL_037A;
							}
							sprite = this.CreateVideoFallbackSprite(media.path);
						}
						if (sprite == null)
						{
							Texture2D content = DownloadHandlerTexture.GetContent(request);
							if (content == null)
							{
								goto IL_037A;
							}
							sprite = this.CreateRuntimeSprite(content, media.path);
						}
					}
					goto JumpOutOfTryFinally-3;
					IL_037A:
					goto IL_0473;
					JumpOutOfTryFinally-3:
					UnityWebRequest request = null;
				}
				if (!(sprite == null))
				{
					PhoneVideoMetadata phoneVideoMetadata = ((media.kind == HackMePhoneApp.PayloadKind.Video) ? PhoneVideoStorage.LoadMetadata(media.path) : null);
					this.AddMediaCell(new HackMePhoneApp.GalleryPayload
					{
						kind = media.kind,
						path = media.path,
						displayName = Path.GetFileNameWithoutExtension(media.path),
						thumbnail = sprite,
						durationSeconds = ((phoneVideoMetadata != null) ? phoneVideoMetadata.durationSeconds : 0f)
					});
					if (media.kind == HackMePhoneApp.PayloadKind.Video)
					{
						this._loadedVideoCount++;
					}
					else
					{
						this._loadedPhotoCount++;
					}
					this.UpdateTabLabels();
					media = default(HackMePhoneApp.MediaFile);
					thumbnailPath = null;
					sprite = null;
					goto IL_0473;
				}
				goto IL_0473;
			}
			this._loadRoutine = null;
			this._galleryLoading = false;
			this.UpdateEmptyStates();
			yield break;
			yield break;
		}

		// Token: 0x06007CC9 RID: 31945 RVA: 0x00330FD0 File Offset: 0x0032F1D0
		private void AddMediaCell(HackMePhoneApp.GalleryPayload payload)
		{
			string displayName = payload.displayName;
			RectTransform rectTransform = (payload.IsVideo ? this._videoGrid : this._photoGrid);
			if (rectTransform == null)
			{
				return;
			}
			Image image = OxGramUI.Image((payload.IsVideo ? "Video_" : "Photo_") + displayName, rectTransform, HackMePhoneApp.Panel, PhoneUiSpriteFactory.Rounded);
			HackMePhoneApp.AddOutline(image.gameObject, new Color(0.08f, 0.55f, 1f, 0.2f), 1f);
			Image image2 = OxGramUI.Image("ThumbnailFrame", image.transform, new Color(0.006f, 0.01f, 0.012f, 1f), PhoneUiSpriteFactory.Rounded);
			RectTransform rectTransform2 = image2.rectTransform;
			rectTransform2.anchorMin = new Vector2(0f, 1f);
			rectTransform2.anchorMax = new Vector2(1f, 1f);
			rectTransform2.pivot = new Vector2(0.5f, 1f);
			rectTransform2.anchoredPosition = new Vector2(0f, -6f);
			rectTransform2.sizeDelta = new Vector2(-12f, 154f);
			image2.gameObject.AddComponent<RectMask2D>();
			image2.raycastTarget = false;
			Image image3 = OxGramUI.Image("Thumbnail", image2.transform, Color.white, null);
			RectTransform rectTransform3 = image3.rectTransform;
			Vector2 vector = new Vector2(0.5f, 0.5f);
			rectTransform3.anchorMax = vector;
			rectTransform3.anchorMin = vector;
			rectTransform3.pivot = new Vector2(0.5f, 0.5f);
			rectTransform3.anchoredPosition = Vector2.zero;
			float num = ((payload.thumbnail.rect.height > 0f) ? (payload.thumbnail.rect.width / payload.thumbnail.rect.height) : 1f);
			float num2 = 1.3246753f;
			rectTransform3.sizeDelta = ((num >= num2) ? new Vector2(154f * num, 154f) : new Vector2(204f, 204f / Mathf.Max(0.01f, num)));
			image3.sprite = payload.thumbnail;
			StreamerCensorImage.Bind(image3, false);
			image3.preserveAspect = false;
			image3.raycastTarget = false;
			if (payload.IsVideo)
			{
				Image image4 = OxGramUI.Image("PlayDisc", image2.transform, new Color(0f, 0f, 0f, 0.64f), PhoneUiSpriteFactory.Circle);
				RectTransform rectTransform4 = image4.rectTransform;
				vector = new Vector2(0.5f, 0.5f);
				rectTransform4.anchorMax = vector;
				rectTransform4.anchorMin = vector;
				rectTransform4.pivot = new Vector2(0.5f, 0.5f);
				rectTransform4.anchoredPosition = Vector2.zero;
				rectTransform4.sizeDelta = new Vector2(54f, 54f);
				image4.raycastTarget = false;
				TMP_Text tmp_Text = this.CreateText("Glyph", image4.transform, "▶", 24f, Color.white, TextAlignmentOptions.Center);
				OxGramUI.Stretch(tmp_Text.rectTransform, 5f, 2f, 4f, 4f);
				tmp_Text.raycastTarget = false;
				TMP_Text tmp_Text2 = this.CreateText("Duration", image2.transform, HackMePhoneApp.FormatDuration(payload.durationSeconds), 10f, Color.white, TextAlignmentOptions.Center);
				RectTransform rectTransform5 = tmp_Text2.rectTransform;
				vector = new Vector2(1f, 0f);
				rectTransform5.anchorMax = vector;
				rectTransform5.anchorMin = vector;
				rectTransform5.pivot = new Vector2(1f, 0f);
				rectTransform5.anchoredPosition = new Vector2(-7f, 7f);
				rectTransform5.sizeDelta = new Vector2(58f, 22f);
				tmp_Text2.fontStyle = FontStyles.Bold;
				tmp_Text2.raycastTarget = false;
			}
			string text = ((displayName.Length > 22) ? (displayName.Substring(0, 22) + "...") : displayName);
			TMP_Text tmp_Text3 = this.CreateText("Name", image.transform, text, 12f, Color.white, TextAlignmentOptions.Left);
			RectTransform rectTransform6 = tmp_Text3.rectTransform;
			rectTransform6.anchorMin = new Vector2(0f, 0f);
			rectTransform6.anchorMax = new Vector2(1f, 0f);
			rectTransform6.pivot = new Vector2(0.5f, 0f);
			rectTransform6.anchoredPosition = new Vector2(0f, 7f);
			rectTransform6.sizeDelta = new Vector2(-74f, 28f);
			rectTransform6.offsetMin = new Vector2(10f, rectTransform6.offsetMin.y);
			tmp_Text3.raycastTarget = false;
			TMP_Text tmp_Text4 = this.CreateLocalizedText("PreviewLabel", image.transform, payload.IsVideo ? "VIDEO" : "PHOTO", 10f, HackMePhoneApp.LinkBlue, TextAlignmentOptions.Right);
			RectTransform rectTransform7 = tmp_Text4.rectTransform;
			rectTransform7.anchorMin = new Vector2(1f, 0f);
			rectTransform7.anchorMax = new Vector2(1f, 0f);
			rectTransform7.pivot = new Vector2(1f, 0f);
			rectTransform7.anchoredPosition = new Vector2(-10f, 8f);
			rectTransform7.sizeDelta = new Vector2(52f, 26f);
			tmp_Text4.fontStyle = FontStyles.Bold;
			tmp_Text4.raycastTarget = false;
			Button button = OxGramUI.Button(image.gameObject);
			button.targetGraphic = image;
			button.transition = Selectable.Transition.ColorTint;
			button.onClick.AddListener(delegate
			{
				this.ShowPayloadPreview(payload);
			});
		}

		// Token: 0x06007CCA RID: 31946 RVA: 0x003315A8 File Offset: 0x0032F7A8
		private Sprite CreateRuntimeSprite(Texture2D texture, string sourcePath)
		{
			if (texture == null)
			{
				return null;
			}
			texture.name = Path.GetFileNameWithoutExtension(sourcePath);
			texture.wrapMode = TextureWrapMode.Clamp;
			Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, (float)texture.width, (float)texture.height), new Vector2(0.5f, 0.5f), 100f);
			sprite.name = sourcePath;
			this._runtimeTextures.Add(texture);
			this._runtimeSprites.Add(sprite);
			return sprite;
		}

		// Token: 0x06007CCB RID: 31947 RVA: 0x0033162C File Offset: 0x0032F82C
		private Sprite CreateVideoFallbackSprite(string videoPath)
		{
			Texture2D texture2D = new Texture2D(16, 16, TextureFormat.RGB24, false);
			for (int i = 0; i < texture2D.height; i++)
			{
				for (int j = 0; j < texture2D.width; j++)
				{
					float num = (float)i / (float)(texture2D.height - 1);
					float num2 = Mathf.Repeat((float)(j + i) * 0.16f, 1f);
					texture2D.SetPixel(j, i, Color.Lerp(HackMePhoneApp.Panel, HackMePhoneApp.PanelRaised, Mathf.Clamp01(num * 0.75f + num2 * 0.25f)));
				}
			}
			texture2D.Apply(false, false);
			return this.CreateRuntimeSprite(texture2D, videoPath);
		}

		// Token: 0x06007CCC RID: 31948 RVA: 0x003316C8 File Offset: 0x0032F8C8
		private static bool IsSupportedVideo(string path)
		{
			string text = Path.GetExtension(path).ToLowerInvariant();
			return text == ".avi" || text == ".mp4" || text == ".mov" || text == ".m4v" || text == ".webm";
		}

		// Token: 0x06007CCD RID: 31949 RVA: 0x00331724 File Offset: 0x0032F924
		private static string FormatDuration(float seconds)
		{
			int num = Mathf.Max(0, Mathf.RoundToInt(seconds));
			return string.Format("{0:00}:{1:00}", num / 60, num % 60);
		}

		// Token: 0x06007CCE RID: 31950 RVA: 0x0033175C File Offset: 0x0032F95C
		private Button BuildModalButton(Transform parent, string name, string label, Vector2 position, Vector2 size, Color background, Color foreground, Action onClick, bool interactable)
		{
			Image image = OxGramUI.Image(name, parent, background, PhoneUiSpriteFactory.Rounded);
			RectTransform rectTransform = image.rectTransform;
			Vector2 vector = new Vector2(0.5f, 0f);
			rectTransform.anchorMax = vector;
			rectTransform.anchorMin = vector;
			rectTransform.pivot = new Vector2(0.5f, 0f);
			rectTransform.anchoredPosition = position;
			rectTransform.sizeDelta = size;
			TMP_Text tmp_Text = this.CreateLocalizedText("Label", image.transform, label, 15f, foreground, TextAlignmentOptions.Center);
			OxGramUI.Stretch(tmp_Text.rectTransform, 6f, 6f, 4f, 4f);
			tmp_Text.fontStyle = FontStyles.Bold;
			tmp_Text.raycastTarget = false;
			Button button = OxGramUI.Button(image.gameObject);
			button.targetGraphic = image;
			button.transition = Selectable.Transition.ColorTint;
			button.interactable = interactable;
			ColorBlock colors = button.colors;
			colors.normalColor = Color.white;
			colors.highlightedColor = new Color(1f, 1f, 1f, 0.84f);
			colors.pressedColor = new Color(0.68f, 0.75f, 0.72f, 1f);
			colors.disabledColor = new Color(0.42f, 0.48f, 0.45f, 0.6f);
			colors.fadeDuration = 0.08f;
			button.colors = colors;
			if (onClick != null)
			{
				button.onClick.AddListener(delegate
				{
					onClick();
				});
			}
			return button;
		}

		// Token: 0x06007CCF RID: 31951 RVA: 0x003318E4 File Offset: 0x0032FAE4
		private static void SetTopRect(RectTransform rt, float top, float height, float horizontalInset)
		{
			rt.anchorMin = new Vector2(0f, 1f);
			rt.anchorMax = new Vector2(1f, 1f);
			rt.pivot = new Vector2(0.5f, 1f);
			rt.anchoredPosition = new Vector2(0f, -top);
			rt.sizeDelta = new Vector2(-horizontalInset * 2f, height);
		}

		// Token: 0x06007CD0 RID: 31952 RVA: 0x00331958 File Offset: 0x0032FB58
		private static void AddOutline(GameObject target, Color color, float distance)
		{
			if (target == null)
			{
				return;
			}
			Outline outline = target.GetComponent<Outline>();
			if (outline == null)
			{
				outline = target.AddComponent<Outline>();
			}
			outline.effectColor = color;
			outline.effectDistance = new Vector2(distance, -distance);
			outline.useGraphicAlpha = true;
		}

		// Token: 0x06007CD1 RID: 31953 RVA: 0x003319A2 File Offset: 0x0032FBA2
		private static IEnumerator AnimateModal(CanvasGroup group, RectTransform card)
		{
			if (group == null || card == null)
			{
				yield break;
			}
			card.localScale = Vector3.one * 0.92f;
			float elapsed = 0f;
			while (elapsed < 0.16f)
			{
				if (group == null || card == null)
				{
					yield break;
				}
				elapsed += Time.unscaledDeltaTime;
				float num = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / 0.16f));
				group.alpha = num;
				card.localScale = Vector3.one * Mathf.Lerp(0.92f, 1f, num);
				yield return null;
			}
			if (group != null)
			{
				group.alpha = 1f;
			}
			if (card != null)
			{
				card.localScale = Vector3.one;
			}
			yield break;
		}

		// Token: 0x06007CD2 RID: 31954 RVA: 0x003319B8 File Offset: 0x0032FBB8
		private void EnsureAudioSource()
		{
			if (this._audioSource != null)
			{
				return;
			}
			this._audioSource = base.GetComponent<AudioSource>();
			if (this._audioSource == null)
			{
				this._audioSource = base.gameObject.AddComponent<AudioSource>();
			}
			this._audioSource.playOnAwake = false;
			this._audioSource.loop = false;
			this._audioSource.spatialBlend = 0f;
			this._audioSource.ignoreListenerPause = true;
		}

		// Token: 0x06007CD3 RID: 31955 RVA: 0x00331A34 File Offset: 0x0032FC34
		private void PlayCue(HackMePhoneApp.CyberCue cue)
		{
			this.EnsureAudioSource();
			AudioClip cue2 = this.GetCue(cue);
			if (this._audioSource == null || cue2 == null)
			{
				return;
			}
			float num = ((cue == HackMePhoneApp.CyberCue.Hack) ? 0.72f : 0.82f);
			this._audioSource.PlayOneShot(cue2, num);
		}

		// Token: 0x06007CD4 RID: 31956 RVA: 0x00331A88 File Offset: 0x0032FC88
		private AudioClip GetCue(HackMePhoneApp.CyberCue cue)
		{
			switch (cue)
			{
			case HackMePhoneApp.CyberCue.Preview:
				if (!(this._previewSound != null))
				{
					return this._previewSound = HackMePhoneApp.CreateCyberSound("HackMe Preview", cue, 0.13f);
				}
				return this._previewSound;
			case HackMePhoneApp.CyberCue.Hack:
				if (!(this._hackSound != null))
				{
					return this._hackSound = HackMePhoneApp.CreateCyberSound("HackMe Transmission", cue, 1.05f);
				}
				return this._hackSound;
			case HackMePhoneApp.CyberCue.Success:
				if (!(this._successSound != null))
				{
					return this._successSound = HackMePhoneApp.CreateCyberSound("HackMe Success", cue, 0.48f);
				}
				return this._successSound;
			default:
				if (!(this._failureSound != null))
				{
					return this._failureSound = HackMePhoneApp.CreateCyberSound("HackMe Failure", cue, 0.36f);
				}
				return this._failureSound;
			}
		}

		// Token: 0x06007CD5 RID: 31957 RVA: 0x00331B68 File Offset: 0x0032FD68
		private static AudioClip CreateCyberSound(string name, HackMePhoneApp.CyberCue cue, float duration)
		{
			int num = Mathf.CeilToInt(44100f * duration);
			float[] array = new float[num];
			for (int i = 0; i < num; i++)
			{
				float num2 = (float)i / 44100f;
				float num3 = num2 / duration;
				float num5;
				switch (cue)
				{
				case HackMePhoneApp.CyberCue.Preview:
				{
					float num4 = Mathf.Sin(3.1415927f * Mathf.Clamp01(num3));
					num5 = Mathf.Sin(6.2831855f * (520f * num2 + 250f * num2 * num2)) * num4 * 0.38f;
					break;
				}
				case HackMePhoneApp.CyberCue.Hack:
				{
					int num6 = Mathf.FloorToInt(num2 * 18f);
					float num7 = 210f + (float)(num6 * 173 % 850);
					float num8 = ((num6 % 4 == 3) ? 0.28f : 1f);
					float num9 = Mathf.Min(Mathf.Clamp01(num2 / 0.025f), Mathf.Clamp01((duration - num2) / 0.08f));
					float num10 = Mathf.Repeat(Mathf.Sin((float)i * 12.9898f) * 43758.547f, 1f) * 2f - 1f;
					num5 = (Mathf.Sin(6.2831855f * num7 * num2) * 0.24f + Mathf.Sign(Mathf.Sin(6.2831855f * num7 * 0.5f * num2)) * 0.07f + num10 * 0.035f) * num8 * num9;
					break;
				}
				case HackMePhoneApp.CyberCue.Success:
				{
					int num11 = Mathf.Min(3, Mathf.FloorToInt(num3 * 4f));
					float num12 = ((num11 == 0) ? 523.25f : ((num11 == 1) ? 659.25f : ((num11 == 2) ? 783.99f : 1046.5f)));
					float num13 = Mathf.Repeat(num3 * 4f, 1f);
					float num14 = Mathf.Sin(3.1415927f * num13) * (1f - num3 * 0.25f);
					num5 = (Mathf.Sin(6.2831855f * num12 * num2) + Mathf.Sin(6.2831855f * num12 * 2f * num2) * 0.18f) * num14 * 0.28f;
					break;
				}
				default:
				{
					float num15 = Mathf.Sin(3.1415927f * Mathf.Clamp01(num3));
					float num16 = Mathf.Lerp(180f, 82f, num3);
					num5 = (Mathf.Sin(6.2831855f * num16 * num2) * 0.3f + Mathf.Sign(Mathf.Sin(6.2831855f * num16 * 2.03f * num2)) * 0.09f) * num15;
					break;
				}
				}
				array[i] = Mathf.Clamp(num5, -0.75f, 0.75f);
			}
			AudioClip audioClip = AudioClip.Create(name, num, 1, 44100, false);
			audioClip.SetData(array, 0);
			return audioClip;
		}

		// Token: 0x06007CD6 RID: 31958 RVA: 0x00331E08 File Offset: 0x00330008
		private void DestroyGeneratedSounds()
		{
			if (this._previewSound != null)
			{
				Object.Destroy(this._previewSound);
			}
			if (this._hackSound != null)
			{
				Object.Destroy(this._hackSound);
			}
			if (this._successSound != null)
			{
				Object.Destroy(this._successSound);
			}
			if (this._failureSound != null)
			{
				Object.Destroy(this._failureSound);
			}
			this._previewSound = null;
			this._hackSound = null;
			this._successSound = null;
			this._failureSound = null;
		}

		// Token: 0x06007CD7 RID: 31959 RVA: 0x00331E98 File Offset: 0x00330098
		private void ClearPhotoGrid()
		{
			HackMePhoneApp.ClearGridChildren(this._photoGrid);
			HackMePhoneApp.ClearGridChildren(this._videoGrid);
			this.ClearLoadedPhotos();
			if (this._photoEmptyState != null)
			{
				this._photoEmptyState.SetActive(false);
			}
			if (this._videoEmptyState != null)
			{
				this._videoEmptyState.SetActive(false);
			}
		}

		// Token: 0x06007CD8 RID: 31960 RVA: 0x00331EF8 File Offset: 0x003300F8
		private static void ClearGridChildren(RectTransform grid)
		{
			if (grid == null)
			{
				return;
			}
			for (int i = grid.childCount - 1; i >= 0; i--)
			{
				GameObject gameObject = grid.GetChild(i).gameObject;
				gameObject.SetActive(false);
				Object.Destroy(gameObject);
			}
		}

		// Token: 0x06007CD9 RID: 31961 RVA: 0x00331F3C File Offset: 0x0033013C
		private void ClearLoadedPhotos()
		{
			for (int i = 0; i < this._runtimeSprites.Count; i++)
			{
				if (this._runtimeSprites[i] != null)
				{
					Object.Destroy(this._runtimeSprites[i]);
				}
			}
			for (int j = 0; j < this._runtimeTextures.Count; j++)
			{
				if (this._runtimeTextures[j] != null)
				{
					Object.Destroy(this._runtimeTextures[j]);
				}
			}
			this._runtimeSprites.Clear();
			this._runtimeTextures.Clear();
		}

		// Token: 0x0400806F RID: 32879
		private const string LocalizationScope = "hackme_ui";

		// Token: 0x04008070 RID: 32880
		private const float ContentWidth = 480f;

		// Token: 0x04008071 RID: 32881
		private const float ContentHeight = 1014f;

		// Token: 0x04008072 RID: 32882
		private static readonly Color Background = new Color(0.018f, 0.027f, 0.024f, 1f);

		// Token: 0x04008073 RID: 32883
		private static readonly Color Panel = new Color(0.035f, 0.065f, 0.052f, 1f);

		// Token: 0x04008074 RID: 32884
		private static readonly Color PanelRaised = new Color(0.052f, 0.088f, 0.072f, 1f);

		// Token: 0x04008075 RID: 32885
		private static readonly Color Neon = new Color(0.24f, 1f, 0.53f, 1f);

		// Token: 0x04008076 RID: 32886
		private static readonly Color LinkBlue = new Color(0.08f, 0.55f, 1f, 1f);

		// Token: 0x04008077 RID: 32887
		private static readonly Color Muted = new Color(0.55f, 0.69f, 0.61f, 1f);

		// Token: 0x04008078 RID: 32888
		private static readonly Color Error = new Color(1f, 0.34f, 0.38f, 1f);

		// Token: 0x04008079 RID: 32889
		private static int _activeHackCount;

		// Token: 0x0400807B RID: 32891
		private SmartphoneController _owner;

		// Token: 0x0400807C RID: 32892
		private TMP_FontAsset _font;

		// Token: 0x0400807D RID: 32893
		private RectTransform _content;

		// Token: 0x0400807E RID: 32894
		private TMP_Text _signalText;

		// Token: 0x0400807F RID: 32895
		private TMP_Text _resultText;

		// Token: 0x04008080 RID: 32896
		private Image _signalDot;

		// Token: 0x04008081 RID: 32897
		private RectTransform _photoGrid;

		// Token: 0x04008082 RID: 32898
		private RectTransform _videoGrid;

		// Token: 0x04008083 RID: 32899
		private ScrollRect _galleryScroll;

		// Token: 0x04008084 RID: 32900
		private Image _photoTab;

		// Token: 0x04008085 RID: 32901
		private Image _videoTab;

		// Token: 0x04008086 RID: 32902
		private TMP_Text _photoTabText;

		// Token: 0x04008087 RID: 32903
		private TMP_Text _videoTabText;

		// Token: 0x04008088 RID: 32904
		private GameObject _photoEmptyState;

		// Token: 0x04008089 RID: 32905
		private GameObject _videoEmptyState;

		// Token: 0x0400808A RID: 32906
		private RectTransform _previewOverlay;

		// Token: 0x0400808B RID: 32907
		private RectTransform _hackOverlay;

		// Token: 0x0400808C RID: 32908
		private DynamicIslandController _island;

		// Token: 0x0400808D RID: 32909
		private Animator _animator;

		// Token: 0x0400808E RID: 32910
		private AudioSource _audioSource;

		// Token: 0x0400808F RID: 32911
		private AudioClip _previewSound;

		// Token: 0x04008090 RID: 32912
		private AudioClip _hackSound;

		// Token: 0x04008091 RID: 32913
		private AudioClip _successSound;

		// Token: 0x04008092 RID: 32914
		private AudioClip _failureSound;

		// Token: 0x04008093 RID: 32915
		private Coroutine _loadRoutine;

		// Token: 0x04008094 RID: 32916
		private Coroutine _hackRoutine;

		// Token: 0x04008095 RID: 32917
		private int _loadGeneration;

		// Token: 0x04008096 RID: 32918
		private bool _built;

		// Token: 0x04008097 RID: 32919
		private bool _registeredAsOpen;

		// Token: 0x04008098 RID: 32920
		private bool _registeredAsHacking;

		// Token: 0x04008099 RID: 32921
		private bool _hackInProgress;

		// Token: 0x0400809A RID: 32922
		private bool _galleryLoading;

		// Token: 0x0400809B RID: 32923
		private HackMePhoneApp.PayloadKind _selectedPayloadKind;

		// Token: 0x0400809C RID: 32924
		private int _loadedPhotoCount;

		// Token: 0x0400809D RID: 32925
		private int _loadedVideoCount;

		// Token: 0x0400809E RID: 32926
		private readonly List<Sprite> _runtimeSprites = new List<Sprite>();

		// Token: 0x0400809F RID: 32927
		private readonly List<Texture2D> _runtimeTextures = new List<Texture2D>();

		// Token: 0x0200100B RID: 4107
		private enum PayloadKind
		{
			// Token: 0x040080A1 RID: 32929
			Photo,
			// Token: 0x040080A2 RID: 32930
			Video
		}

		// Token: 0x0200100C RID: 4108
		private sealed class GalleryPayload
		{
			// Token: 0x17001422 RID: 5154
			// (get) Token: 0x06007CDE RID: 31966 RVA: 0x003320E5 File Offset: 0x003302E5
			public bool IsVideo
			{
				get
				{
					return this.kind == HackMePhoneApp.PayloadKind.Video;
				}
			}

			// Token: 0x040080A3 RID: 32931
			public HackMePhoneApp.PayloadKind kind;

			// Token: 0x040080A4 RID: 32932
			public string path;

			// Token: 0x040080A5 RID: 32933
			public string displayName;

			// Token: 0x040080A6 RID: 32934
			public Sprite thumbnail;

			// Token: 0x040080A7 RID: 32935
			public float durationSeconds;
		}

		// Token: 0x0200100D RID: 4109
		private readonly struct MediaFile
		{
			// Token: 0x06007CE0 RID: 31968 RVA: 0x003320F0 File Offset: 0x003302F0
			public MediaFile(string path, HackMePhoneApp.PayloadKind kind)
			{
				this.path = path;
				this.kind = kind;
				this.modifiedUtc = File.GetLastWriteTimeUtc(path);
			}

			// Token: 0x040080A8 RID: 32936
			public readonly string path;

			// Token: 0x040080A9 RID: 32937
			public readonly HackMePhoneApp.PayloadKind kind;

			// Token: 0x040080AA RID: 32938
			public readonly DateTime modifiedUtc;
		}

		// Token: 0x0200100E RID: 4110
		private enum CyberCue
		{
			// Token: 0x040080AC RID: 32940
			Preview,
			// Token: 0x040080AD RID: 32941
			Hack,
			// Token: 0x040080AE RID: 32942
			Success,
			// Token: 0x040080AF RID: 32943
			Failure
		}
	}
}
