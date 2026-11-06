using System;
using System.Collections.Generic;
using System.Globalization;
using Features.CensorBars;
using Features.CielOS;
using Features.CielOS.UI;
using Features.CielOS.WindowManager;
using Features.Inventory;
using Features.Localization;
using Features.MainMenu;
using Features.Rank;
using ProjectExhibitionist.CharacterCustomization.Core.Storage;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.RunModes
{
	// Token: 0x02000C85 RID: 3205
	public sealed class CielOSRunModesApp : MonoBehaviour
	{
		// Token: 0x06005D12 RID: 23826 RVA: 0x002494F4 File Offset: 0x002476F4
		private void BuildClassicStartTimeSelector(RectTransform parent)
		{
			if (this._classic == null)
			{
				this._classic = RunModeService.LoadClassicSettings();
			}
			RectTransform rectTransform = this.BuildSelectionControl("ClassicStartTime", parent, "LEAVE AT", new Vector2(-292f, 18f), 150f);
			this.ChoiceButton(rectTransform, "Day", "DAY", 0, 2, !this._classic.startAtNight, delegate
			{
				this.SaveClassicStartTime(false);
			}, false);
			this.ChoiceButton(rectTransform, "Night", "NIGHT", 1, 2, this._classic.startAtNight, delegate
			{
				this.SaveClassicStartTime(true);
			}, false);
		}

		// Token: 0x06005D13 RID: 23827 RVA: 0x00249593 File Offset: 0x00247793
		private void SaveClassicStartTime(bool night)
		{
			this._classic.startAtNight = night;
			RunModeService.SaveClassicSettings(this._classic);
			this.Build();
		}

		// Token: 0x06005D14 RID: 23828 RVA: 0x002495B2 File Offset: 0x002477B2
		private void Update()
		{
			this.SetControllerLayout(ControllerInput.IsActive);
		}

		// Token: 0x06005D15 RID: 23829 RVA: 0x002495BF File Offset: 0x002477BF
		private void SetControllerLayout(bool controller)
		{
			if (this._controllerLayout == controller)
			{
				return;
			}
			this._controllerLayout = controller;
			this._controllerConfigurationOpen = this.ConfigurationOpen;
			this._focusControllerPage = this._controllerLayout;
			this.Build();
		}

		// Token: 0x06005D16 RID: 23830 RVA: 0x002495F0 File Offset: 0x002477F0
		private void ChooseControllerMode(RunModeKind mode)
		{
			this._selected = mode;
			if (this._controllerLayout)
			{
				this._controllerConfigurationOpen = true;
				this._freeConfigurationOpen = mode == RunModeKind.Free;
				this._longWayHomeConfigurationOpen = mode == RunModeKind.LongWayHome;
				this._focusControllerPage = true;
			}
			this.Build();
		}

		// Token: 0x06005D17 RID: 23831 RVA: 0x0024962C File Offset: 0x0024782C
		public bool ControllerBack()
		{
			if (!this._controllerLayout || (!this._controllerConfigurationOpen && !this.ConfigurationOpen))
			{
				return false;
			}
			this._controllerConfigurationOpen = (this._freeConfigurationOpen = (this._longWayHomeConfigurationOpen = false));
			this._focusControllerPage = true;
			this.Build();
			return true;
		}

		// Token: 0x06005D18 RID: 23832 RVA: 0x0024967A File Offset: 0x0024787A
		public bool ControllerStartRun(EventSystem events)
		{
			if (!this._controllerLayout || (!this._controllerConfigurationOpen && !this.ConfigurationOpen) || this._controllerStart == null)
			{
				return false;
			}
			ControllerUINavigation.Activate(this._controllerStart.gameObject, events);
			return true;
		}

		// Token: 0x06005D19 RID: 23833 RVA: 0x002496B8 File Offset: 0x002478B8
		private void BuildControllerConfiguration(RectTransform body)
		{
			this.BuildModeSummary(body, RunModeService.ModesUnlocked);
			RectTransform rectTransform = body.Find("ModeDetail") as RectTransform;
			if (rectTransform != null)
			{
				rectTransform.anchorMin = new Vector2(rectTransform.anchorMin.x, 0.5f);
				rectTransform.anchorMax = new Vector2(rectTransform.anchorMax.x, 0.5f);
				rectTransform.anchoredPosition = Vector2.zero;
			}
			this.TextActionButton("BackToModes", body, "BACK TO MODES", Vector2.up, new Vector2(0f, -8f), new Vector2(220f, 44f), false).onClick.AddListener(delegate
			{
				this.ControllerBack();
			});
		}

		// Token: 0x06005D1A RID: 23834 RVA: 0x00249778 File Offset: 0x00247978
		private void FinishControllerLayout()
		{
			if (!this._controllerLayout || !this._focusControllerPage)
			{
				return;
			}
			this._focusControllerPage = false;
			GameObject gameObject = null;
			if (!this._controllerConfigurationOpen && !this.ConfigurationOpen)
			{
				string text = ((this._selected == RunModeKind.Free) ? "FREEMODE" : ((this._selected == RunModeKind.LongWayHome) ? "LONGWAYHOME" : "CLASSIC"));
				Transform transform = this._content.Find("Body/ModeGallery/" + text);
				if (transform != null)
				{
					gameObject = transform.gameObject;
				}
			}
			if (gameObject == null)
			{
				foreach (Selectable selectable in this._content.GetComponentsInChildren<Selectable>())
				{
					if (!(selectable == this._controllerStart) && !(selectable.name == "BackToModes") && !(selectable is Scrollbar) && ControllerUINavigation.IsFocusable(selectable.gameObject))
					{
						gameObject = selectable.gameObject;
						break;
					}
				}
			}
			if (gameObject == null && this._controllerStart != null)
			{
				gameObject = this._controllerStart.gameObject;
			}
			ControllerUINavigation.RequestFocus(gameObject);
		}

		// Token: 0x06005D1B RID: 23835 RVA: 0x00249895 File Offset: 0x00247A95
		public void ConfigureBanners(Sprite classic, Sprite free, Sprite longWayHome, Sprite background = null)
		{
			this.classicBanner = classic;
			this.freeBanner = free;
			this.longWayHomeBanner = longWayHome;
			this.heartPattern = background;
		}

		// Token: 0x06005D1C RID: 23836 RVA: 0x002498B4 File Offset: 0x00247AB4
		private void Awake()
		{
			this.ApplyFullscreenWindow();
			this._controllerLayout = ControllerInput.IsActive;
			this._focusControllerPage = this._controllerLayout;
			this._free = RunModeService.LoadFreeSettings();
			this._classic = RunModeService.LoadClassicSettings();
			this._longWayHome = RunModeService.LoadLongWayHomeSettings();
			this._selected = RunModeKind.Classic;
			this._classicRunKind = RunModeService.LoadPreferredClassicRunKind();
			this.ResolveWeatherNames();
			this.Build();
		}

		// Token: 0x06005D1D RID: 23837 RVA: 0x0024991D File Offset: 0x00247B1D
		private void Start()
		{
			this.ApplyFullscreenWindow();
		}

		// Token: 0x06005D1E RID: 23838 RVA: 0x00249928 File Offset: 0x00247B28
		private void ApplyFullscreenWindow()
		{
			CielOSWindow componentInParent = base.GetComponentInParent<CielOSWindow>();
			if (componentInParent == null || componentInParent.Rect == null)
			{
				return;
			}
			componentInParent.Rect.anchorMin = Vector2.zero;
			componentInParent.Rect.anchorMax = Vector2.one;
			componentInParent.Rect.pivot = new Vector2(0.5f, 0.5f);
			componentInParent.Rect.offsetMin = Vector2.zero;
			componentInParent.Rect.offsetMax = Vector2.zero;
			componentInParent.Rect.anchoredPosition = Vector2.zero;
			componentInParent.Rect.sizeDelta = Vector2.zero;
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

		// Token: 0x06005D1F RID: 23839 RVA: 0x00249A14 File Offset: 0x00247C14
		private void OnEnable()
		{
			GameLocalization.LocaleChanged += this.HandleLocaleChanged;
			RankProgressionService.Changed += this.HandleRankChanged;
			RankedLeaderboardService.AvailabilityChanged += this.HandleRankedAvailabilityChanged;
			NpcRosterPreferences.Changed += this.HandleRosterPreferencesChanged;
			RunModeService.Changed += this.HandleRunSettingsChanged;
			this.HandleRosterPreferencesChanged();
			this.HandleRunSettingsChanged();
			this.HandleLocaleChanged();
		}

		// Token: 0x06005D20 RID: 23840 RVA: 0x00249A88 File Offset: 0x00247C88
		private void OnDisable()
		{
			GameLocalization.LocaleChanged -= this.HandleLocaleChanged;
			RankProgressionService.Changed -= this.HandleRankChanged;
			RankedLeaderboardService.AvailabilityChanged -= this.HandleRankedAvailabilityChanged;
			NpcRosterPreferences.Changed -= this.HandleRosterPreferencesChanged;
			RunModeService.Changed -= this.HandleRunSettingsChanged;
		}

		// Token: 0x06005D21 RID: 23841 RVA: 0x00249AEC File Offset: 0x00247CEC
		private void HandleLocaleChanged()
		{
			if (this._content == null)
			{
				return;
			}
			string noticeEnglish = this._noticeEnglish;
			this.Build();
			if (noticeEnglish != null)
			{
				this.SetNotice(noticeEnglish);
			}
		}

		// Token: 0x06005D22 RID: 23842 RVA: 0x00249B20 File Offset: 0x00247D20
		private void HandleRunSettingsChanged()
		{
			ClassicRunSettings classicRunSettings = RunModeService.LoadClassicSettings();
			LongWayHomeRunSettings longWayHomeRunSettings = RunModeService.LoadLongWayHomeSettings();
			bool flag = this._classic == null || this._classic.startAtNight != classicRunSettings.startAtNight || this._longWayHome == null || this._longWayHome.startingLootEnabled != longWayHomeRunSettings.startingLootEnabled || this._longWayHome.startAtNight != longWayHomeRunSettings.startAtNight || this._longWayHome.prologue != longWayHomeRunSettings.prologue;
			this._longWayHome = longWayHomeRunSettings;
			this._classic = classicRunSettings;
			if (flag && this._content != null)
			{
				this.Build();
			}
		}

		// Token: 0x06005D23 RID: 23843 RVA: 0x00249BBF File Offset: 0x00247DBF
		private void HandleRosterPreferencesChanged()
		{
			if (this._content != null && this._displayedPopulationMode != NpcRosterPreferences.PopulationMode)
			{
				this.Build();
			}
		}

		// Token: 0x06005D24 RID: 23844 RVA: 0x00249BE2 File Offset: 0x00247DE2
		private void HandleRankChanged()
		{
			if (!RunModeService.ModesUnlocked && this._selected == RunModeKind.Free)
			{
				this._selected = RunModeKind.Classic;
			}
			if (!RunModeService.ModesUnlocked)
			{
				this._classicRunKind = ClassicRunKind.Normal;
			}
			this.Build();
		}

		// Token: 0x06005D25 RID: 23845 RVA: 0x00249C0F File Offset: 0x00247E0F
		private void HandleRankedAvailabilityChanged()
		{
			if (!RankedLeaderboardService.IsAvailable)
			{
				this._classicRunKind = ClassicRunKind.Normal;
			}
			this.Build();
		}

		// Token: 0x06005D26 RID: 23846 RVA: 0x00249C28 File Offset: 0x00247E28
		private void Build()
		{
			if (this._isBuilding)
			{
				return;
			}
			this._isBuilding = true;
			try
			{
				if (this._content != null)
				{
					this._content.gameObject.SetActive(false);
					if (Application.isPlaying)
					{
						Object.Destroy(this._content.gameObject);
					}
					else
					{
						Object.DestroyImmediate(this._content.gameObject);
					}
				}
				this._controllerStart = null;
				this._notice = null;
				this._noticeEnglish = null;
				this._displayedPopulationMode = NpcRosterPreferences.PopulationMode;
				this._content = CielOSRunModesApp.Rect("RunModesLayout", base.transform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
				CielOSRunModesApp.AddImage(this._content, CielOSRunModesApp.Overlay).raycastTarget = false;
				if (this.heartPattern != null)
				{
					RectTransform rectTransform = CielOSRunModesApp.Rect("HeartPattern", this._content, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(this.heartPattern.rect.width, this.heartPattern.rect.height));
					Image image = CielOSRunModesApp.AddImage(rectTransform, new Color(0.78f, 0.62f, 0.84f, this.ConfigurationOpen ? 0.2f : 0.56f));
					image.sprite = this.heartPattern;
					image.type = global::UnityEngine.UI.Image.Type.Simple;
					image.preserveAspect = true;
					image.raycastTarget = false;
					AspectRatioFitter aspectRatioFitter = rectTransform.gameObject.AddComponent<AspectRatioFitter>();
					aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
					aspectRatioFitter.aspectRatio = this.heartPattern.rect.width / this.heartPattern.rect.height;
				}
				CielOSRunModesApp.Image("BackgroundScrim", this._content, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero, new Color(CielOSRunModesApp.Overlay.r, CielOSRunModesApp.Overlay.g, CielOSRunModesApp.Overlay.b, 0.58f)).raycastTarget = false;
				this.BuildHeader();
				RectTransform rectTransform2 = CielOSRunModesApp.Rect("Body", this._content, Vector2.zero, Vector2.one, new Vector2(34f, 28f), new Vector2(-34f, -92f));
				if (this._freeConfigurationOpen)
				{
					this.BuildFreeConfiguration(rectTransform2);
				}
				else if (this._longWayHomeConfigurationOpen)
				{
					this.BuildLongWayHomeConfiguration(rectTransform2);
				}
				else if (this._controllerLayout && this._controllerConfigurationOpen)
				{
					this.BuildControllerConfiguration(rectTransform2);
				}
				else
				{
					this.BuildModeGallery(rectTransform2);
				}
				foreach (TMP_Text tmp_Text in this._content.GetComponentsInChildren<TMP_Text>(true))
				{
					(tmp_Text.GetComponent<LocalizedGameText>() ?? LocalizedGameText.BindFontOnly(tmp_Text)).FitTranslationToRectangle(false);
				}
			}
			finally
			{
				this._isBuilding = false;
			}
			this.FinishControllerLayout();
		}

		// Token: 0x06005D27 RID: 23847 RVA: 0x00249F20 File Offset: 0x00248120
		private void BuildHeader()
		{
			RectTransform rectTransform = CielOSRunModesApp.Rect("Header", this._content, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(34f, -12f), new Vector2(-34f, 66f));
			this.Text("Title", rectTransform, "RUN MODES", this.ConfigurationOpen ? 22f : 46f, FontStyles.Bold, new Vector2(0f, 0f), new Vector2(0f, 0f), Vector2.zero, new Vector2(520f, 54f), CielOSRunModesApp.White, TextAlignmentOptions.BottomLeft);
			CielOSRunModesApp.Image("Divider", rectTransform, new Vector2(0f, 0f), new Vector2(1f, 0f), Vector2.zero, new Vector2(0f, 2f), new Color(CielOSRunModesApp.Pink.r, CielOSRunModesApp.Pink.g, CielOSRunModesApp.Pink.b, 0.42f)).raycastTarget = false;
			if (!this.ConfigurationOpen && this._selected == RunModeKind.Classic && RankedLeaderboardService.IsSupported)
			{
				this.TextActionButton("ViewLeaderboard", rectTransform, "LEADERBOARDS", new Vector2(1f, 0.5f), new Vector2(0f, 4f), new Vector2(220f, 42f), false).onClick.AddListener(delegate
				{
					CielOSWindowManager cielOSWindowManager = base.GetComponentInParent<CielOSWindowManager>();
					if (cielOSWindowManager == null)
					{
						cielOSWindowManager = Object.FindObjectOfType<CielOSWindowManager>();
					}
					if (cielOSWindowManager != null)
					{
						cielOSWindowManager.LaunchApp("leaderboard");
					}
				});
			}
		}

		// Token: 0x06005D28 RID: 23848 RVA: 0x0024A0B8 File Offset: 0x002482B8
		private void BuildModeGallery(RectTransform parent)
		{
			bool modesUnlocked = RunModeService.ModesUnlocked;
			int num = 0;
			float num2 = 320f;
			float num3 = 82f;
			if (this._controllerLayout)
			{
				num2 = Mathf.Min(360f, parent.rect.height * 0.8f);
				num3 = 0f;
			}
			else if (this._selected == RunModeKind.Classic && RunModeService.ClassicStartTimeUnlocked)
			{
				num2 = Mathf.Min(num2, Mathf.Max(140f, parent.rect.height - 260f));
				num3 = Mathf.Max(num3, 240f + num2 * 0.5f - parent.rect.height * 0.5f);
			}
			RectTransform rectTransform = CielOSRunModesApp.Rect("ModeGallery", parent, new Vector2(0.08f, 0.5f), new Vector2(0.92f, 0.5f), new Vector2(0f, num3), new Vector2(0f, num2));
			this.ModeButton(rectTransform, RunModeKind.Classic, "CLASSIC", null, num++, 3);
			this.ModeButton(rectTransform, RunModeKind.Free, "FREE MODE", modesUnlocked ? null : "LOCKED", num++, 3);
			this.ModeButton(rectTransform, RunModeKind.LongWayHome, "LONG WAY HOME", RunModeService.LongWayHomeUnlocked ? null : "LOCKED", num, 3);
			if (!this._controllerLayout)
			{
				this.BuildModeSummary(parent, modesUnlocked);
			}
		}

		// Token: 0x06005D29 RID: 23849 RVA: 0x0024A210 File Offset: 0x00248410
		private void ModeButton(RectTransform parent, RunModeKind mode, string label, string state, int index, int count)
		{
			bool flag = this._selected == mode;
			float num = (float)index / (float)count;
			float num2 = ((float)index + 1f) / (float)count;
			float num3 = ((index == 0) ? 0f : 10f);
			float num4 = ((index == count - 1) ? 0f : 10f);
			RectTransform rectTransform = CielOSRunModesApp.Rect(label.Replace(" ", string.Empty), parent, new Vector2(num, 0f), new Vector2(num2, 1f), new Vector2(num3, 0f), new Vector2(-num4, 0f));
			Image image = CielOSRunModesApp.AddImage(rectTransform, flag ? CielOSRunModesApp.Pink : new Color(CielOSRunModesApp.PalePink.r, CielOSRunModesApp.PalePink.g, CielOSRunModesApp.PalePink.b, 0.22f));
			CielOSTheme.Round(image);
			Shadow shadow = rectTransform.gameObject.AddComponent<Shadow>();
			shadow.effectColor = new Color(0f, 0f, 0f, flag ? 0.56f : 0.38f);
			shadow.effectDistance = new Vector2(0f, -8f);
			shadow.useGraphicAlpha = true;
			float num5 = (flag ? 4f : 1f);
			RectTransform rectTransform2 = CielOSRunModesApp.Rect("Card", rectTransform, Vector2.zero, Vector2.one, new Vector2(num5, num5), new Vector2(-num5, -num5));
			CielOSTheme.Round(CielOSRunModesApp.AddImage(rectTransform2, CielOSRunModesApp.Row));
			rectTransform2.gameObject.AddComponent<Mask>().showMaskGraphic = true;
			Sprite sprite = this.BannerFor(mode);
			if (sprite != null)
			{
				RectTransform rectTransform3 = CielOSRunModesApp.Rect("Banner", rectTransform2, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
				Image image2 = CielOSRunModesApp.AddImage(rectTransform3, flag ? Color.white : new Color(0.72f, 0.62f, 0.7f, 1f));
				StreamerArtworkImage.Bind(image2, sprite);
				image2.type = global::UnityEngine.UI.Image.Type.Simple;
				image2.preserveAspect = false;
				image2.raycastTarget = false;
				AspectRatioFitter aspectRatioFitter = rectTransform3.gameObject.AddComponent<AspectRatioFitter>();
				aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
				aspectRatioFitter.aspectRatio = sprite.rect.width / sprite.rect.height;
			}
			CielOSRunModesApp.Image("Shade", rectTransform2, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero, new Color(CielOSRunModesApp.Overlay.r, CielOSRunModesApp.Overlay.g, CielOSRunModesApp.Overlay.b, flag ? 0.12f : 0.42f)).raycastTarget = false;
			if (flag)
			{
				CielOSRunModesApp.Image("SelectionAccent", rectTransform2, new Vector2(0f, 1f), new Vector2(0.34f, 1f), Vector2.zero, new Vector2(0f, 4f), CielOSRunModesApp.Pink).raycastTarget = false;
			}
			this.Text("ModeNumber", rectTransform2, "0" + (index + 1).ToString(), 12f, FontStyles.Bold, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(18f, -18f), new Vector2(44f, 22f), flag ? CielOSRunModesApp.Pink : CielOSRunModesApp.Muted, TextAlignmentOptions.TopLeft).characterSpacing = 1.8f;
			Image image3 = CielOSRunModesApp.AddImage(CielOSRunModesApp.Rect("TitleFade", rectTransform2, new Vector2(0f, 0f), new Vector2(1f, 0.62f), Vector2.zero, Vector2.zero), new Color(CielOSRunModesApp.Overlay.r, CielOSRunModesApp.Overlay.g, CielOSRunModesApp.Overlay.b, 0.94f));
			image3.sprite = CielOSRunModesApp.CardFadeSprite();
			image3.type = global::UnityEngine.UI.Image.Type.Simple;
			image3.raycastTarget = false;
			Button button = rectTransform.gameObject.AddComponent<Button>();
			button.targetGraphic = image;
			button.colors = CielOSRunModesApp.ControlColors(CielOSRunModesApp.RowHover);
			button.interactable = !this._controllerLayout || mode == RunModeKind.Classic || ((mode == RunModeKind.Free) ? RunModeService.ModesUnlocked : RunModeService.LongWayHomeUnlocked);
			CielOSSfx.Attach(button);
			rectTransform.gameObject.AddComponent<MainMenuButtonFx>().SetTintChildGraphics(false);
			button.onClick.AddListener(delegate
			{
				this.ChooseControllerMode(mode);
			});
			TMP_Text tmp_Text = this.Text("Label", rectTransform2, label, 25f, FontStyles.Bold, new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(22f, 14f), new Vector2(-22f, 70f), flag ? CielOSRunModesApp.White : CielOSRunModesApp.PalePink, TextAlignmentOptions.BottomLeft);
			tmp_Text.enableWordWrapping = true;
			tmp_Text.characterSpacing = 1.4f;
			if (state != null)
			{
				RectTransform rectTransform4 = CielOSRunModesApp.Rect("StateBadge", rectTransform2, Vector2.one, Vector2.one, new Vector2(-14f, -14f), new Vector2(136f, 30f));
				Image image4 = CielOSRunModesApp.AddImage(rectTransform4, new Color(CielOSRunModesApp.Overlay.r, CielOSRunModesApp.Overlay.g, CielOSRunModesApp.Overlay.b, 0.86f));
				CielOSTheme.Round(image4);
				image4.raycastTarget = false;
				this.Text("State", rectTransform4, state, 13f, FontStyles.Bold, Vector2.zero, Vector2.one, new Vector2(8f, 0f), new Vector2(-8f, 0f), CielOSRunModesApp.PalePink, TextAlignmentOptions.Center);
			}
		}

		// Token: 0x06005D2A RID: 23850 RVA: 0x0024A7C8 File Offset: 0x002489C8
		private void BuildModeSummary(RectTransform parent, bool unlocked)
		{
			if ((!unlocked || !RankedLeaderboardService.IsAvailable) && this._classicRunKind == ClassicRunKind.Ranked)
			{
				this._classicRunKind = ClassicRunKind.Normal;
			}
			bool flag = true;
			RunModeKind selected = this._selected;
			bool flag2 = false;
			bool flag3 = false;
			string text;
			string text2;
			string text3;
			string text4;
			if (!unlocked && this._selected == RunModeKind.Free)
			{
				text = "FREE MODE";
				text2 = "Work up the nerve";
				text3 = "Earn your Certified Exhibitionist rank, then plan an outing of your own.";
				text4 = "LOCKED";
				flag = false;
			}
			else
			{
				RunModeKind selected2 = this._selected;
				if (selected2 != RunModeKind.Free)
				{
					if (selected2 != RunModeKind.LongWayHome)
					{
						text = ((!unlocked) ? "CLASSIC" : "CLASSIC RUN");
						if (!unlocked)
						{
							text2 = "A little more daring";
							text3 = "Explore and grow Ciel's rank. Return home anytime; reach 100 m for a distance bonus.";
							text4 = "START CLASSIC";
							flag2 = true;
						}
						else
						{
							flag3 = true;
							bool flag4 = this._classicRunKind == ClassicRunKind.Ranked;
							text2 = (flag4 ? "Go further than the rest" : "How far will you go?");
							text3 = (flag4 ? CielOSRunModesApp.RankedStartNotice() : "Head out with nothing to wear. Return home anytime; reach 100 m for a distance bonus.");
							text4 = (flag4 ? "START RANKED" : "START NORMAL");
						}
					}
					else
					{
						text = "LONG WAY HOME";
						flag = RunModeService.LongWayHomeUnlocked;
						text2 = (flag ? "Find your way back" : "The long way can wait");
						text3 = (flag ? "Home is a long way off. Follow Maps, keep a low profile and don't let the cops catch you." : "Become a Certified Exhibitionist before taking the long way home.");
						text4 = (flag ? "PLAN YOUR RUN" : "LOCKED");
					}
				}
				else
				{
					text = "FREE MODE";
					text2 = "Your kind of journey";
					text3 = "Pick Ciel's look, the weather and how much trouble she can get into.";
					text4 = "PLAN YOUR RUN";
				}
			}
			bool flag5 = flag && selected == RunModeKind.Classic && unlocked;
			float num = ((parent.rect.width < 1050f) ? 0.08f : 0.18f);
			RectTransform rectTransform = CielOSRunModesApp.Rect("ModeDetail", parent, new Vector2(num, 0f), new Vector2(1f - num, 0f), new Vector2(0f, 22f), new Vector2(0f, flag5 ? 200f : 172f));
			Image image = CielOSRunModesApp.AddImage(rectTransform, new Color(CielOSRunModesApp.Window.r, CielOSRunModesApp.Window.g, CielOSRunModesApp.Window.b, 0.94f));
			CielOSTheme.Round(image);
			CielOSTheme.Outline(image, new Color?(new Color(CielOSRunModesApp.PalePink.r, CielOSRunModesApp.PalePink.g, CielOSRunModesApp.PalePink.b, 0.14f)), 1f);
			Shadow shadow = rectTransform.gameObject.AddComponent<Shadow>();
			shadow.effectColor = new Color(0f, 0f, 0f, 0.42f);
			shadow.effectDistance = new Vector2(0f, -7f);
			shadow.useGraphicAlpha = true;
			Image image2 = CielOSRunModesApp.Image("Accent", rectTransform, Vector2.zero, new Vector2(0f, 1f), new Vector2(0f, 12f), new Vector2(6f, -12f), flag ? CielOSRunModesApp.Pink : CielOSRunModesApp.Muted);
			CielOSTheme.Round(image2);
			image2.raycastTarget = false;
			this.Text("Eyebrow", rectTransform, text, 12f, FontStyles.Bold, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(28f, -17f), new Vector2(-304f, 18f), flag ? CielOSRunModesApp.Pink : CielOSRunModesApp.Muted, TextAlignmentOptions.TopLeft).characterSpacing = 1.8f;
			TMP_Text tmp_Text = this.Text("Title", rectTransform, CielOSRunModesApp.Localize(text2).ToUpperInvariant(), 23f, FontStyles.Bold, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(28f, -43f), new Vector2(-304f, 30f), CielOSRunModesApp.White, TextAlignmentOptions.TopLeft);
			tmp_Text.enableAutoSizing = true;
			tmp_Text.fontSizeMin = 18f;
			tmp_Text.fontSizeMax = 23f;
			TMP_Text tmp_Text2 = this.Text("Description", rectTransform, text3, 15f, FontStyles.Normal, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(28f, -78f), new Vector2(-304f, 38f), flag ? CielOSRunModesApp.PalePink : CielOSRunModesApp.Muted, TextAlignmentOptions.TopLeft);
			tmp_Text2.enableWordWrapping = true;
			tmp_Text2.overflowMode = TextOverflowModes.Ellipsis;
			tmp_Text2.richText = false;
			if (this._selected == RunModeKind.Classic && this._classicRunKind == ClassicRunKind.Ranked)
			{
				tmp_Text2.enableAutoSizing = true;
				tmp_Text2.fontSizeMin = 12f;
				tmp_Text2.fontSizeMax = 15f;
			}
			if (flag2)
			{
				this.BuildClassicJourneyProgress(rectTransform);
			}
			else if (flag3)
			{
				this.BuildClassicRunChoice(rectTransform);
			}
			if (text4 == null)
			{
				this.Text("ComingSoon", rectTransform, "COMING SOON", 14f, FontStyles.Bold, new Vector2(1f, 0.5f), new Vector2(1f, 0.5f), new Vector2(-28f, 0f), new Vector2(220f, 44f), CielOSRunModesApp.Muted, TextAlignmentOptions.Center).characterSpacing = 2f;
				return;
			}
			bool flag6 = flag && (selected == RunModeKind.Classic || selected == RunModeKind.Free);
			Button button = this.TextActionButton("ModeAction", rectTransform, text4, new Vector2(1f, 0.5f), new Vector2(-28f, flag6 ? 40f : 0f), new Vector2(248f, 54f), flag);
			button.interactable = flag;
			if (this._controllerLayout && selected == RunModeKind.Classic)
			{
				this._controllerStart = button;
				ControllerButtonHint.Bind(button.gameObject, "Menu");
			}
			if (!flag)
			{
				return;
			}
			if (flag6)
			{
				this.BuildPopulationSelector(rectTransform, new Vector2(-28f, 18f), 248f);
			}
			if (flag5)
			{
				this.BuildClassicStartTimeSelector(rectTransform);
			}
			RunModeKind capturedMode = selected;
			button.onClick.AddListener(delegate
			{
				if (capturedMode == RunModeKind.Free)
				{
					this._freeConfigurationOpen = true;
					this.Build();
					return;
				}
				if (capturedMode == RunModeKind.LongWayHome)
				{
					this._longWayHomeConfigurationOpen = true;
					this.Build();
					return;
				}
				this.StartRun(capturedMode);
			});
		}

		// Token: 0x06005D2B RID: 23851 RVA: 0x0024ADB4 File Offset: 0x00248FB4
		private void BuildClassicJourneyProgress(RectTransform parent)
		{
			RankProgressionConfigSO config = RankProgressionService.Config;
			int num = ((config != null && config.ranksInOrder != null && config.ranksInOrder.Count > 0) ? config.ranksInOrder.Count : 6);
			int num2 = Mathf.Clamp(RankProgressionService.RankIndex, 0, num - 1);
			int num3 = Mathf.Max(0, num - 1 - num2);
			float num4 = Mathf.Clamp01(((float)num2 + 1f) / (float)num);
			string text = ((num3 == 1) ? CielOSRunModesApp.Localize("1 RANK UNTIL FREE MODE") : CielOSRunModesApp.Format("{0} RANKS UNTIL FREE MODE", new object[] { num3 }));
			this.Text("ClassicJourney", parent, text, 13f, FontStyles.Bold, new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(28f, 29f), new Vector2(-304f, 18f), CielOSRunModesApp.Muted, TextAlignmentOptions.Left).characterSpacing = 1.1f;
			RectTransform rectTransform = CielOSRunModesApp.Rect("ClassicJourneyTrack", parent, new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(28f, 18f), new Vector2(-304f, 7f));
			Image image = CielOSRunModesApp.AddImage(rectTransform, new Color(CielOSRunModesApp.Track.r, CielOSRunModesApp.Track.g, CielOSRunModesApp.Track.b, 0.88f));
			CielOSTheme.Round(image);
			image.raycastTarget = false;
			Image image2 = CielOSRunModesApp.AddImage(CielOSRunModesApp.Rect("Fill", rectTransform, Vector2.zero, new Vector2(num4, 1f), Vector2.zero, Vector2.zero), CielOSRunModesApp.Pink);
			CielOSTheme.Round(image2);
			image2.raycastTarget = false;
			RectTransform rectTransform2 = CielOSRunModesApp.Rect("Milestone", rectTransform, new Vector2(num4, 0.5f), new Vector2(num4, 0.5f), Vector2.zero, new Vector2(13f, 13f));
			rectTransform2.pivot = new Vector2(0.5f, 0.5f);
			Image image3 = CielOSRunModesApp.AddImage(rectTransform2, CielOSRunModesApp.PalePink);
			CielOSTheme.Circle(image3);
			image3.raycastTarget = false;
		}

		// Token: 0x06005D2C RID: 23852 RVA: 0x0024AFD8 File Offset: 0x002491D8
		private void BuildClassicRunChoice(RectTransform parent)
		{
			if (!RankedLeaderboardService.IsAvailable)
			{
				RectTransform rectTransform = CielOSRunModesApp.Rect("NormalOnly", parent, new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(28f, 18f), new Vector2(142f, 34f));
				Image image = CielOSRunModesApp.AddImage(rectTransform, CielOSRunModesApp.Surface);
				CielOSTheme.Round(image);
				CielOSTheme.Outline(image, new Color?(new Color(CielOSRunModesApp.PalePink.r, CielOSRunModesApp.PalePink.g, CielOSRunModesApp.PalePink.b, 0.16f)), 1f);
				this.Text("Label", rectTransform, "NORMAL RUN", 12f, FontStyles.Bold, Vector2.zero, Vector2.one, new Vector2(12f, 0f), new Vector2(-12f, 0f), CielOSRunModesApp.PalePink, TextAlignmentOptions.Center).characterSpacing = 1.2f;
				return;
			}
			RectTransform rectTransform2 = CielOSRunModesApp.Rect("ClassicRunKind", parent, new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(28f, 18f), new Vector2(Mathf.Clamp(parent.rect.width - 498f, 150f, 300f), 38f));
			Image image2 = CielOSRunModesApp.AddImage(rectTransform2, new Color(CielOSRunModesApp.Track.r, CielOSRunModesApp.Track.g, CielOSRunModesApp.Track.b, 0.84f));
			CielOSTheme.Round(image2);
			CielOSTheme.Outline(image2, new Color?(new Color(CielOSRunModesApp.PalePink.r, CielOSRunModesApp.PalePink.g, CielOSRunModesApp.PalePink.b, 0.12f)), 1f);
			this.ClassicRunChoiceButton(rectTransform2, ClassicRunKind.Normal, "NORMAL", 0f, 0.5f);
			this.ClassicRunChoiceButton(rectTransform2, ClassicRunKind.Ranked, "RANKED", 0.5f, 1f);
		}

		// Token: 0x06005D2D RID: 23853 RVA: 0x0024B1D2 File Offset: 0x002493D2
		private static string RankedStartNotice()
		{
			if (RankProgressionService.IsAdminRankActive && RankedLeaderboardService.IsAvailable)
			{
				return "Admin review: Ranked gameplay is available. Progress, achievements and leaderboard submission are disabled for this run.";
			}
			if (!RankedLeaderboardService.CanSubmitScores)
			{
				return "Ranked runs are unavailable right now. Choose Normal to head out.";
			}
			return "Go further or return home anytime. Reach 100 m for a return bonus. Both count on the leaderboard. No cheats or mods.";
		}

		// Token: 0x06005D2E RID: 23854 RVA: 0x0024B1FC File Offset: 0x002493FC
		private void ClassicRunChoiceButton(RectTransform parent, ClassicRunKind kind, string label, float min, float max)
		{
			bool flag = this._classicRunKind == kind;
			RectTransform rectTransform = CielOSRunModesApp.Rect(label, parent, new Vector2(min, 0f), new Vector2(max, 1f), new Vector2(3f, 3f), new Vector2(-3f, -3f));
			Image image = CielOSRunModesApp.AddImage(rectTransform, flag ? CielOSRunModesApp.Pink : Color.clear);
			CielOSTheme.Round(image);
			Button button = rectTransform.gameObject.AddComponent<Button>();
			button.targetGraphic = image;
			button.colors = CielOSRunModesApp.ControlColors(flag ? CielOSRunModesApp.PalePink : CielOSRunModesApp.RowHover);
			CielOSSfx.Attach(button);
			this.Text("Label", rectTransform, label, 12f, FontStyles.Bold, Vector2.zero, Vector2.one, new Vector2(8f, 0f), new Vector2(-8f, 0f), flag ? CielOSRunModesApp.Window : CielOSRunModesApp.Muted, TextAlignmentOptions.Center).characterSpacing = 1.2f;
			button.onClick.AddListener(delegate
			{
				if (this._classicRunKind == kind)
				{
					return;
				}
				this._classicRunKind = kind;
				RunModeService.SavePreferredClassicRunKind(kind);
				this.Build();
			});
		}

		// Token: 0x06005D2F RID: 23855 RVA: 0x0024B327 File Offset: 0x00249527
		private Sprite BannerFor(RunModeKind mode)
		{
			if (mode == RunModeKind.Free)
			{
				return this.freeBanner;
			}
			if (mode != RunModeKind.LongWayHome)
			{
				return this.classicBanner;
			}
			return this.longWayHomeBanner;
		}

		// Token: 0x06005D30 RID: 23856 RVA: 0x0024B348 File Offset: 0x00249548
		private void BuildFreeConfiguration(RectTransform parent)
		{
			this._freeViewport = parent;
			this._freeViewportSize = parent.rect.size;
			RectTransform rectTransform = CielOSRunModesApp.Rect("FreeConfiguration", parent, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
			RectTransform rectTransform2 = CielOSRunModesApp.Rect("Inner", rectTransform, Vector2.zero, Vector2.one, new Vector2(8f, 0f), new Vector2(-8f, 0f));
			float num = Mathf.Min(1.25f, Mathf.Min(rectTransform2.rect.width / 1100f, rectTransform2.rect.height / 625f));
			if (num > 0f)
			{
				Vector2 vector = rectTransform2.rect.size / num;
				RectTransform rectTransform3 = rectTransform2;
				RectTransform rectTransform4 = rectTransform2;
				Vector2 vector2 = new Vector2(0.5f, 0.5f);
				rectTransform4.anchorMax = vector2;
				rectTransform3.anchorMin = vector2;
				rectTransform2.sizeDelta = vector;
				rectTransform2.anchoredPosition = Vector2.zero;
				rectTransform2.localScale = Vector3.one * num;
			}
			this.BuildFree(rectTransform2);
		}

		// Token: 0x06005D31 RID: 23857 RVA: 0x0024B468 File Offset: 0x00249668
		private void BuildStage(RectTransform parent)
		{
			bool modesUnlocked = RunModeService.ModesUnlocked;
			if (!modesUnlocked && this._selected == RunModeKind.Free)
			{
				this.BuildLocked(parent);
				return;
			}
			RunModeKind selected = this._selected;
			if (selected == RunModeKind.Free)
			{
				this.BuildFree(parent);
				return;
			}
			if (selected != RunModeKind.LongWayHome)
			{
				this.BuildClassic(parent);
				return;
			}
			this.BuildModeSummary(parent, modesUnlocked);
		}

		// Token: 0x06005D32 RID: 23858 RVA: 0x0024B4B8 File Offset: 0x002496B8
		private void BuildClassic(RectTransform parent)
		{
			this.PageTitle(parent, "CLASSIC");
			bool flag = !RunModeService.ModesUnlocked;
			TMP_Text tmp_Text = this.Text("Description", parent, flag ? "Explore and grow Ciel's rank. Return home anytime; reach 100 m to earn a bonus based on your furthest distance." : "Push as far as you dare, or return home anytime to end the run. Reach 100 m to earn 1 bonus point per metre of your furthest distance; returning earlier grants no return bonus.", 22f, FontStyles.Normal, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0f, -105f), new Vector2(0f, 170f), CielOSRunModesApp.PalePink, TextAlignmentOptions.TopLeft);
			tmp_Text.enableWordWrapping = true;
			tmp_Text.overflowMode = TextOverflowModes.Overflow;
			tmp_Text.lineSpacing = 8f;
			this.Footer(parent, "START CLASSIC", RunModeKind.Classic);
		}

		// Token: 0x06005D33 RID: 23859 RVA: 0x0024B568 File Offset: 0x00249768
		private void OpenInventoryPicker(RectTransform parent, int slot)
		{
			RectTransform overlay = CielOSRunModesApp.Rect("InventoryPicker", parent, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
			Image image = CielOSRunModesApp.AddImage(overlay, new Color(CielOSRunModesApp.Overlay.r, CielOSRunModesApp.Overlay.g, CielOSRunModesApp.Overlay.b, 0.88f));
			Button button = overlay.gameObject.AddComponent<Button>();
			button.targetGraphic = image;
			button.onClick.AddListener(delegate
			{
				Object.Destroy(overlay.gameObject);
			});
			RectTransform rectTransform = CielOSRunModesApp.Rect("PickerDialog", overlay, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(Mathf.Min(520f, parent.rect.width - 44f), Mathf.Min(440f, parent.rect.height - 50f)));
			Image image2 = CielOSRunModesApp.AddImage(rectTransform, CielOSRunModesApp.Window);
			CielOSTheme.Round(image2);
			CielOSTheme.Outline(image2, new Color?(CielOSRunModesApp.Pink), 1f);
			Button button2 = rectTransform.gameObject.AddComponent<Button>();
			button2.targetGraphic = image2;
			button2.transition = Selectable.Transition.None;
			this.Text("PickerTitle", rectTransform, CielOSRunModesApp.Format("SLOT {0}", new object[] { slot + 1 }), 22f, FontStyles.Bold, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(22f, -58f), new Vector2(-70f, 34f), CielOSRunModesApp.White, TextAlignmentOptions.Left);
			Button button3 = this.TextActionButton("Close", rectTransform, "X", new Vector2(1f, 1f), new Vector2(-46f, -48f), new Vector2(32f, 32f), false);
			TMP_Text componentInChildren = button3.GetComponentInChildren<TMP_Text>();
			componentInChildren.rectTransform.offsetMin = Vector2.zero;
			componentInChildren.rectTransform.offsetMax = Vector2.zero;
			componentInChildren.fontSize = 18f;
			componentInChildren.characterSpacing = 0f;
			button3.onClick.AddListener(delegate
			{
				Object.Destroy(overlay.gameObject);
			});
			RectTransform rectTransform2 = CielOSRunModesApp.Rect("PickerViewport", rectTransform, Vector2.zero, Vector2.one, new Vector2(20f, 20f), new Vector2(-32f, -100f));
			CielOSRunModesApp.AddImage(rectTransform2, Color.clear);
			rectTransform2.gameObject.AddComponent<RectMask2D>();
			IEnumerable<InventoryItemSO> unlockedChoices = FreeRunLoadoutCatalog.GetUnlockedChoices();
			List<InventoryItemSO> list = new List<InventoryItemSO> { null };
			foreach (InventoryItemSO inventoryItemSO in unlockedChoices)
			{
				if (!(inventoryItemSO == null))
				{
					bool flag = false;
					for (int i = 0; i < 3; i++)
					{
						if (i != slot && this._free.startingItemIds[i] == inventoryItemSO.itemId)
						{
							flag = true;
						}
					}
					if (!flag)
					{
						list.Add(inventoryItemSO);
					}
				}
			}
			RectTransform rectTransform3 = CielOSRunModesApp.Rect("PickerList", rectTransform2, new Vector2(0f, 1f), new Vector2(1f, 1f), Vector2.zero, new Vector2(0f, (float)list.Count * 48f));
			rectTransform3.pivot = new Vector2(0.5f, 1f);
			rectTransform3.anchoredPosition = Vector2.zero;
			ScrollRect scrollRect = rectTransform2.gameObject.AddComponent<ScrollRect>();
			scrollRect.viewport = rectTransform2;
			scrollRect.content = rectTransform3;
			scrollRect.horizontal = false;
			scrollRect.vertical = true;
			scrollRect.movementType = ScrollRect.MovementType.Clamped;
			rectTransform2.gameObject.AddComponent<SmoothCollectionScroll>().Configure(48f, 0.16f);
			RectTransform rectTransform4 = CielOSRunModesApp.Rect("PickerScrollbar", rectTransform, new Vector2(1f, 0f), new Vector2(1f, 1f), new Vector2(-16f, 20f), new Vector2(6f, -100f));
			CielOSRunModesApp.AddImage(rectTransform4, CielOSRunModesApp.Track);
			RectTransform rectTransform5 = CielOSRunModesApp.Rect("Handle", rectTransform4, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
			Image image3 = CielOSRunModesApp.AddImage(rectTransform5, CielOSRunModesApp.Pink);
			Scrollbar scrollbar = rectTransform4.gameObject.AddComponent<Scrollbar>();
			scrollbar.direction = Scrollbar.Direction.BottomToTop;
			scrollbar.handleRect = rectTransform5;
			scrollbar.targetGraphic = image3;
			scrollbar.SetValueWithoutNotify(1f);
			scrollRect.verticalScrollbar = scrollbar;
			scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
			string text = this._free.startingItemIds[slot];
			for (int j = 0; j < list.Count; j++)
			{
				InventoryItemSO inventoryItemSO2 = list[j];
				string id = ((inventoryItemSO2 != null) ? inventoryItemSO2.itemId : string.Empty);
				bool flag2 = text == id;
				RectTransform rectTransform6 = CielOSRunModesApp.Rect("Choice_" + j.ToString(), rectTransform3, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0f, (float)(-(float)j) * 48f), new Vector2(0f, 44f));
				Image image4 = CielOSRunModesApp.AddImage(rectTransform6, flag2 ? CielOSRunModesApp.RowHover : CielOSRunModesApp.Row);
				CielOSTheme.Round(image4);
				Button button4 = rectTransform6.gameObject.AddComponent<Button>();
				button4.targetGraphic = image4;
				button4.colors = CielOSRunModesApp.ControlColors(CielOSRunModesApp.RowHover);
				CielOSSfx.Attach(button4);
				if (inventoryItemSO2 != null && inventoryItemSO2.icon != null)
				{
					Image image5 = CielOSRunModesApp.AddImage(CielOSRunModesApp.Rect("Icon", rectTransform6, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(8f, 0f), new Vector2(32f, 32f)), CielOSRunModesApp.White);
					image5.sprite = inventoryItemSO2.icon;
					StreamerCensorImage.BindItem(image5, inventoryItemSO2);
					image5.preserveAspect = true;
					image5.raycastTarget = false;
				}
				this.Text("ItemName", rectTransform6, (inventoryItemSO2 == null) ? "EMPTY" : InteractionLocalization.ObjectName(string.IsNullOrWhiteSpace(inventoryItemSO2.itemName) ? inventoryItemSO2.name : inventoryItemSO2.itemName), 16f, FontStyles.Bold, Vector2.zero, Vector2.one, new Vector2(48f, 0f), new Vector2(-88f, 0f), CielOSRunModesApp.White, TextAlignmentOptions.Left);
				if (flag2)
				{
					this.Text("Selected", rectTransform6, "ON", 13f, FontStyles.Bold, new Vector2(1f, 0f), new Vector2(1f, 1f), new Vector2(-42f, 0f), new Vector2(34f, 0f), CielOSRunModesApp.Pink, TextAlignmentOptions.Center);
				}
				button4.onClick.AddListener(delegate
				{
					this._free.startingItemIds[slot] = id;
					this.SaveAndBuild();
				});
			}
			foreach (TMP_Text tmp_Text in overlay.GetComponentsInChildren<TMP_Text>(true))
			{
				(tmp_Text.GetComponent<LocalizedGameText>() ?? LocalizedGameText.BindFontOnly(tmp_Text)).FitTranslationToRectangle(false);
			}
		}

		// Token: 0x06005D34 RID: 23860 RVA: 0x0024BD20 File Offset: 0x00249F20
		private void EnsureStartWeatherAllowed()
		{
			if (this._free.allowedWeatherPresetNames == null || this._free.allowedWeatherPresetNames.Length == 0)
			{
				return;
			}
			string[] allowedWeatherPresetNames = this._free.allowedWeatherPresetNames;
			for (int i = 0; i < allowedWeatherPresetNames.Length; i++)
			{
				if (string.Equals(allowedWeatherPresetNames[i], this._free.weatherPresetName, StringComparison.OrdinalIgnoreCase))
				{
					return;
				}
			}
			List<string> list = new List<string>(this._free.allowedWeatherPresetNames) { this._free.weatherPresetName };
			this._free.allowedWeatherPresetNames = list.ToArray();
		}

		// Token: 0x06005D35 RID: 23861 RVA: 0x0024BDB0 File Offset: 0x00249FB0
		private void ToggleWeatherInPool(string name)
		{
			IEnumerable<string> enumerable2;
			if (this._free.allowedWeatherPresetNames != null && this._free.allowedWeatherPresetNames.Length != 0)
			{
				IEnumerable<string> enumerable = this._free.allowedWeatherPresetNames;
				enumerable2 = enumerable;
			}
			else
			{
				IEnumerable<string> enumerable = this._weatherNames;
				enumerable2 = enumerable;
			}
			List<string> list = new List<string>(enumerable2);
			int num = list.FindIndex((string value) => string.Equals(value, name, StringComparison.OrdinalIgnoreCase));
			if (num >= 0)
			{
				if (list.Count == 1)
				{
					return;
				}
				list.RemoveAt(num);
			}
			else
			{
				list.Add(name);
			}
			if (!list.Exists((string value) => string.Equals(value, this._free.weatherPresetName, StringComparison.OrdinalIgnoreCase)))
			{
				this._free.weatherPresetName = list[0];
				this._free.weatherIndex = this._weatherNames.FindIndex((string value) => string.Equals(value, this._free.weatherPresetName, StringComparison.OrdinalIgnoreCase));
			}
			this._free.allowedWeatherPresetNames = ((list.Count == this._weatherNames.Count) ? Array.Empty<string>() : list.ToArray());
			this.SaveAndBuild();
		}

		// Token: 0x06005D36 RID: 23862 RVA: 0x0024BEC0 File Offset: 0x0024A0C0
		private void BuildLocked(RectTransform parent)
		{
			this.PageTitle(parent, "FREE MODE");
			this.Text("Description", parent, "Earn your Certified Exhibitionist rank, then plan an outing of your own.", 22f, FontStyles.Normal, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0f, -105f), new Vector2(0f, 90f), CielOSRunModesApp.PalePink, TextAlignmentOptions.TopLeft).enableWordWrapping = true;
			this.TextActionButton("Locked", parent, "LOCKED", new Vector2(1f, 0f), Vector2.zero, new Vector2(248f, 56f), false).interactable = false;
		}

		// Token: 0x06005D37 RID: 23863 RVA: 0x0024BF77 File Offset: 0x0024A177
		private void BuildComingSoon(RectTransform parent)
		{
			this.PageTitle(parent, "LONG WAY HOME");
			this.ReadOnlyRow(parent, 0, "STATUS", "FUTURE UPDATE");
		}

		// Token: 0x06005D38 RID: 23864 RVA: 0x0024BF98 File Offset: 0x0024A198
		private void PageTitle(RectTransform parent, string value)
		{
			this.Text("PageTitle", parent, value, 32f, FontStyles.Bold, new Vector2(0f, 1f), new Vector2(1f, 1f), Vector2.zero, new Vector2(-196f, 48f), CielOSRunModesApp.White, TextAlignmentOptions.Left);
		}

		// Token: 0x06005D39 RID: 23865 RVA: 0x0024BFF8 File Offset: 0x0024A1F8
		private void ReadOnlyRow(RectTransform parent, int index, string label, string value)
		{
			RectTransform rectTransform = this.SettingsRow(parent, index, true);
			this.RowLabel(rectTransform, label, true);
			this.Text("Value", rectTransform, value, 17f, FontStyles.Bold, new Vector2(0.46f, 0f), Vector2.one, new Vector2(8f, 0f), new Vector2(-24f, 0f), CielOSRunModesApp.PalePink, TextAlignmentOptions.Right);
		}

		// Token: 0x06005D3A RID: 23866 RVA: 0x0024C06C File Offset: 0x0024A26C
		private void SwitchRow(RectTransform parent, int index, string label, bool isOn, Action<bool> setter)
		{
			RectTransform rectTransform = this.SettingsRow(parent, index, true);
			this.RowLabel(rectTransform, label, true);
			Toggle toggle = rectTransform.gameObject.AddComponent<Toggle>();
			toggle.targetGraphic = rectTransform.GetComponent<Image>();
			toggle.isOn = isOn;
			toggle.colors = CielOSRunModesApp.ControlColors(CielOSRunModesApp.RowHover);
			RectTransform rectTransform2 = CielOSRunModesApp.Rect("Switch", rectTransform, new Vector2(1f, 0.5f), new Vector2(1f, 0.5f), new Vector2(-24f, 0f), new Vector2(68f, 32f));
			Image image = CielOSRunModesApp.AddImage(rectTransform2, CielOSRunModesApp.Track);
			CielOSTheme.Round(image);
			image.raycastTarget = false;
			RectTransform rectTransform3 = CielOSRunModesApp.Rect("Knob", rectTransform2, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(4f, 0f), new Vector2(24f, 24f));
			Image image2 = CielOSRunModesApp.AddImage(rectTransform3, CielOSRunModesApp.Muted);
			CielOSTheme.Circle(image2);
			image2.raycastTarget = false;
			rectTransform.gameObject.AddComponent<MainMenuSwitchVisual>().Configure(rectTransform3, image);
			toggle.onValueChanged.AddListener(delegate(bool value)
			{
				setter(value);
				this.SaveAndBuild();
			});
		}

		// Token: 0x06005D3B RID: 23867 RVA: 0x0024C1BC File Offset: 0x0024A3BC
		private void SelectorRow(RectTransform parent, int index, string label, IEnumerable<string> options, int selectedIndex, Action<int> setter)
		{
			RectTransform rectTransform = this.SettingsRow(parent, index, true);
			this.Text("Label", rectTransform, label, 19f, FontStyles.Bold, Vector2.zero, new Vector2(0.42f, 1f), new Vector2(22f, 0f), new Vector2(-12f, 0f), CielOSRunModesApp.White, TextAlignmentOptions.Left);
			RectTransform rectTransform2 = CielOSRunModesApp.Rect("Selector", rectTransform, new Vector2(0.5f, 0f), Vector2.one, Vector2.zero, new Vector2(-18f, 0f));
			Button button = this.ArrowButton(rectTransform2, "Previous", "<", new Vector2(0f, 0.5f));
			TMP_Text tmp_Text = this.Text("Value", rectTransform2, "-", 19f, FontStyles.Bold, new Vector2(0f, 0.5f), new Vector2(1f, 0.5f), new Vector2(44f, 0f), new Vector2(-44f, 62f), CielOSRunModesApp.White, TextAlignmentOptions.Center);
			tmp_Text.enableAutoSizing = true;
			tmp_Text.fontSizeMin = 14f;
			tmp_Text.fontSizeMax = 19f;
			tmp_Text.margin = new Vector4(4f, 0f, 4f, 0f);
			tmp_Text.overflowMode = TextOverflowModes.Ellipsis;
			Button button2 = this.ArrowButton(rectTransform2, "Next", ">", new Vector2(1f, 0.5f));
			MainMenuArrowSelector mainMenuArrowSelector = rectTransform.gameObject.AddComponent<MainMenuArrowSelector>();
			mainMenuArrowSelector.Configure(tmp_Text, button, button2);
			mainMenuArrowSelector.SetOptions(options, "cielos_settings");
			mainMenuArrowSelector.SetIndexWithoutNotify(selectedIndex);
			mainMenuArrowSelector.OnUserChangedIndex += setter;
		}

		// Token: 0x06005D3C RID: 23868 RVA: 0x0024C36C File Offset: 0x0024A56C
		private Button ArrowButton(RectTransform parent, string name, string label, Vector2 anchor)
		{
			RectTransform rectTransform = CielOSRunModesApp.Rect(name, parent, anchor, anchor, Vector2.zero, new Vector2(40f, 40f));
			CielOSRunModesApp.AddImage(rectTransform, Color.clear);
			Image image = CielOSRunModesApp.Image("Glyph", rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(24f, 24f), Color.white);
			CielOSIcon.Apply(image, CielOSIconId.ChevronDown, Color.white, 64);
			image.rectTransform.localRotation = Quaternion.Euler(0f, 0f, (label == "<") ? (-90f) : 90f);
			image.raycastTarget = false;
			Button button = rectTransform.gameObject.AddComponent<Button>();
			button.targetGraphic = image;
			button.colors = new ColorBlock
			{
				normalColor = CielOSRunModesApp.Pink,
				highlightedColor = CielOSRunModesApp.White,
				pressedColor = CielOSRunModesApp.PalePink,
				selectedColor = CielOSRunModesApp.White,
				disabledColor = new Color(CielOSRunModesApp.Muted.r, CielOSRunModesApp.Muted.g, CielOSRunModesApp.Muted.b, 0.42f),
				colorMultiplier = 1f,
				fadeDuration = 0.1f
			};
			CielOSSfx.Attach(button);
			return button;
		}

		// Token: 0x06005D3D RID: 23869 RVA: 0x0024C4D0 File Offset: 0x0024A6D0
		private void SliderRow(RectTransform parent, int index, string label, float value, float min, float max, Action<float> setter, Func<float, string> formatter, bool enabled, float defaultValue)
		{
			RectTransform rectTransform = this.SettingsRow(parent, index, enabled);
			this.Text("Label", rectTransform, label, 19f, FontStyles.Bold, Vector2.zero, new Vector2(0.42f, 1f), new Vector2(22f, 0f), new Vector2(-12f, 0f), enabled ? CielOSRunModesApp.White : CielOSRunModesApp.Muted, TextAlignmentOptions.Left);
			RectTransform rectTransform2 = CielOSRunModesApp.Rect("Slider", rectTransform, new Vector2(0.42f, 0.5f), new Vector2(1f, 0.5f), Vector2.zero, new Vector2(-88f, 30f));
			Slider slider = rectTransform2.gameObject.AddComponent<Slider>();
			slider.minValue = min;
			slider.maxValue = max;
			slider.value = value;
			slider.interactable = enabled;
			slider.direction = Slider.Direction.LeftToRight;
			CielOSTheme.Round(CielOSRunModesApp.AddImage(CielOSRunModesApp.Rect("Background", rectTransform2, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero), CielOSRunModesApp.Track));
			RectTransform fill = CielOSRunModesApp.Rect("Fill", rectTransform2, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
			Image image = CielOSRunModesApp.AddImage(fill, enabled ? CielOSRunModesApp.Pink : CielOSRunModesApp.Muted);
			CielOSTheme.Round(image);
			image.raycastTarget = false;
			RectTransform rectTransform3 = CielOSRunModesApp.Rect("Handle Slide Area", rectTransform2, Vector2.zero, Vector2.one, new Vector2(35f, 0f), new Vector2(-35f, 0f));
			RectTransform rectTransform4 = CielOSRunModesApp.Rect("Handle", rectTransform3, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), Vector2.zero, new Vector2(70f, 32f));
			rectTransform4.pivot = new Vector2(0.5f, 0.5f);
			Image image2 = CielOSRunModesApp.AddImage(rectTransform4, enabled ? CielOSRunModesApp.Window : CielOSRunModesApp.Surface);
			CielOSTheme.Round(image2);
			TMP_Text valueText = this.Text("Value", rectTransform4, formatter(value), 17f, FontStyles.Bold, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero, enabled ? CielOSRunModesApp.PalePink : CielOSRunModesApp.Muted, TextAlignmentOptions.Center);
			valueText.enableAutoSizing = true;
			valueText.fontSizeMin = 11f;
			valueText.fontSizeMax = 17f;
			valueText.overflowMode = TextOverflowModes.Ellipsis;
			slider.handleRect = rectTransform4;
			slider.targetGraphic = image2;
			rectTransform2.gameObject.AddComponent<MainMenuUiSoundRelay>();
			CielOSRunModesApp.UpdateSliderFill(fill, value, min, max);
			RectTransform rectTransform5 = CielOSRunModesApp.Rect("Reset", rectTransform, new Vector2(1f, 0.5f), new Vector2(1f, 0.5f), new Vector2(-28f, 0f), new Vector2(40f, 40f));
			Image image3 = CielOSRunModesApp.AddImage(rectTransform5, CielOSRunModesApp.Surface);
			CielOSTheme.Circle(image3);
			Button button = rectTransform5.gameObject.AddComponent<Button>();
			button.targetGraphic = image3;
			button.colors = CielOSRunModesApp.ControlColors(CielOSRunModesApp.RowHover);
			button.interactable = enabled;
			CielOSSfx.Attach(button);
			this.Text("Icon", rectTransform5, "R", 17f, FontStyles.Bold, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero, enabled ? CielOSRunModesApp.PalePink : CielOSRunModesApp.Muted, TextAlignmentOptions.Center);
			button.onClick.AddListener(delegate
			{
				slider.value = defaultValue;
			});
			slider.onValueChanged.AddListener(delegate(float current)
			{
				setter(current);
				valueText.text = formatter(current);
				CielOSRunModesApp.UpdateSliderFill(fill, current, min, max);
				RunModeService.SaveFreeSettings(this._free);
			});
		}

		// Token: 0x06005D3E RID: 23870 RVA: 0x0024C8FC File Offset: 0x0024AAFC
		private static void UpdateSliderFill(RectTransform fill, float value, float min, float max)
		{
			if (fill == null)
			{
				return;
			}
			float num = (Mathf.Approximately(min, max) ? 0f : Mathf.Clamp01(Mathf.InverseLerp(min, max, value)));
			float num2 = 35f;
			fill.anchorMin = Vector2.zero;
			fill.anchorMax = new Vector2(num, 1f);
			fill.offsetMin = new Vector2(3f, 3f);
			fill.offsetMax = new Vector2(Mathf.Lerp(num2, -num2, num), -3f);
		}

		// Token: 0x06005D3F RID: 23871 RVA: 0x0024C984 File Offset: 0x0024AB84
		private void ActionRow(RectTransform parent, int index, string label, CielOSIconId icon, Action action)
		{
			RectTransform rectTransform = this.SettingsRow(parent, index, true);
			Button button = rectTransform.gameObject.AddComponent<Button>();
			button.targetGraphic = rectTransform.GetComponent<Image>();
			button.colors = CielOSRunModesApp.ControlColors(CielOSRunModesApp.RowHover);
			CielOSSfx.Attach(button);
			button.onClick.AddListener(delegate
			{
				action();
			});
			this.RowLabel(rectTransform, label, true);
			Image image = CielOSRunModesApp.Image("Icon", rectTransform, new Vector2(1f, 0.5f), new Vector2(1f, 0.5f), new Vector2(-28f, 0f), new Vector2(30f, 30f), Color.white);
			CielOSIcon.Apply(image, icon, CielOSRunModesApp.Pink, 80);
			image.raycastTarget = false;
		}

		// Token: 0x06005D40 RID: 23872 RVA: 0x0024CA58 File Offset: 0x0024AC58
		private RectTransform SettingsRow(RectTransform parent, int index, bool enabled)
		{
			float num = -132f - (float)index * 72f;
			RectTransform rectTransform = CielOSRunModesApp.Rect("Row" + index.ToString(), parent, new Vector2(0.04f, 1f), new Vector2(0.96f, 1f), new Vector2(0f, num), new Vector2(0f, 62f));
			CielOSTheme.Round(CielOSRunModesApp.AddImage(rectTransform, enabled ? CielOSRunModesApp.Row : new Color(CielOSRunModesApp.Row.r, CielOSRunModesApp.Row.g, CielOSRunModesApp.Row.b, 0.42f)));
			return rectTransform;
		}

		// Token: 0x06005D41 RID: 23873 RVA: 0x0024CB00 File Offset: 0x0024AD00
		private void RowLabel(RectTransform row, string label, bool enabled)
		{
			this.Text("Label", row, label, 19f, FontStyles.Bold, Vector2.zero, Vector2.one, new Vector2(22f, 0f), new Vector2(-310f, 0f), enabled ? CielOSRunModesApp.White : CielOSRunModesApp.Muted, TextAlignmentOptions.Left);
		}

		// Token: 0x06005D42 RID: 23874 RVA: 0x0024CB60 File Offset: 0x0024AD60
		private void Footer(RectTransform parent, string label, RunModeKind mode)
		{
			RectTransform rectTransform = CielOSRunModesApp.Rect("Footer", parent, new Vector2(0.04f, 0f), new Vector2(0.96f, 0f), Vector2.zero, new Vector2(0f, 56f));
			this.ActionButton("Start", rectTransform, label, CielOSIconId.Play, new Vector2(1f, 0f), new Vector2(1f, 0f), Vector2.zero, new Vector2(248f, 56f), CielOSRunModesApp.Pink, CielOSRunModesApp.Window).onClick.AddListener(delegate
			{
				this.StartRun(mode);
			});
			bool flag = mode == RunModeKind.Classic || mode == RunModeKind.Free;
			if (flag)
			{
				this.BuildPopulationSelector(rectTransform, new Vector2(-272f, 0f), 300f);
			}
			this._notice = this.Text("Notice", rectTransform, string.Empty, 15f, FontStyles.Bold, new Vector2(0f, 0f), new Vector2(1f, 0f), Vector2.zero, new Vector2(flag ? (-596f) : (-270f), 52f), CielOSRunModesApp.PalePink, TextAlignmentOptions.Left);
		}

		// Token: 0x06005D43 RID: 23875 RVA: 0x0024CCB8 File Offset: 0x0024AEB8
		private void BuildPopulationSelector(RectTransform parent, Vector2 position, float width)
		{
			RectTransform rectTransform = this.BuildSelectionControl("NpcPopulation", parent, "THE CROWD", position, width);
			this.PopulationChoiceButton(rectTransform, NpcPopulationMode.Mixed, "EVERYONE", 0);
			this.PopulationChoiceButton(rectTransform, NpcPopulationMode.MaleOnly, "MEN", 1);
			this.PopulationChoiceButton(rectTransform, NpcPopulationMode.FemaleOnly, "WOMEN", 2);
		}

		// Token: 0x06005D44 RID: 23876 RVA: 0x0024CD03 File Offset: 0x0024AF03
		private void SaveLongWayHomeAndBuild()
		{
			RunModeService.SaveLongWayHomeSettings(this._longWayHome);
			this.Build();
		}

		// Token: 0x06005D45 RID: 23877 RVA: 0x0024CD18 File Offset: 0x0024AF18
		private RectTransform BuildSelectionControl(string name, RectTransform parent, string label, Vector2 position, float width)
		{
			RectTransform rectTransform = CielOSRunModesApp.Rect(name, parent, new Vector2(1f, 0f), new Vector2(1f, 0f), position, new Vector2(width, 60f));
			this.Text("Label", rectTransform, label, 11f, FontStyles.Bold, new Vector2(0f, 1f), new Vector2(1f, 1f), Vector2.zero, new Vector2(0f, 16f), CielOSRunModesApp.Muted, TextAlignmentOptions.Left).characterSpacing = 1.1f;
			RectTransform rectTransform2 = CielOSRunModesApp.Rect("Selector", rectTransform, Vector2.zero, new Vector2(1f, 0f), Vector2.zero, new Vector2(0f, 38f));
			Image image = CielOSRunModesApp.AddImage(rectTransform2, new Color(CielOSRunModesApp.Track.r, CielOSRunModesApp.Track.g, CielOSRunModesApp.Track.b, 0.84f));
			CielOSTheme.Round(image);
			CielOSTheme.Outline(image, new Color?(new Color(CielOSRunModesApp.PalePink.r, CielOSRunModesApp.PalePink.g, CielOSRunModesApp.PalePink.b, 0.12f)), 1f);
			return rectTransform2;
		}

		// Token: 0x06005D46 RID: 23878 RVA: 0x0024CE50 File Offset: 0x0024B050
		private void PopulationChoiceButton(RectTransform parent, NpcPopulationMode mode, string label, int index)
		{
			this.ChoiceButton(parent, mode.ToString(), label, index, 3, this._displayedPopulationMode == mode, delegate
			{
				NpcRosterPreferences.PopulationMode = mode;
			}, true);
		}

		// Token: 0x06005D47 RID: 23879 RVA: 0x0024CEA4 File Offset: 0x0024B0A4
		private void ChoiceButton(RectTransform parent, string name, string label, int index, int count, bool selected, Action onClick, bool wrap = false)
		{
			RectTransform rectTransform = CielOSRunModesApp.Rect(name, parent, new Vector2((float)index / (float)count, 0f), new Vector2(((float)index + 1f) / (float)count, 1f), new Vector2(3f, 3f), new Vector2(-3f, -3f));
			Image image = CielOSRunModesApp.AddImage(rectTransform, selected ? CielOSRunModesApp.Pink : Color.clear);
			CielOSTheme.Round(image);
			Button button = rectTransform.gameObject.AddComponent<Button>();
			button.targetGraphic = image;
			button.colors = CielOSRunModesApp.ControlColors(selected ? CielOSRunModesApp.PalePink : CielOSRunModesApp.RowHover);
			CielOSSfx.Attach(button);
			TMP_Text tmp_Text = this.Text("Label", rectTransform, label, 11f, FontStyles.Bold, Vector2.zero, Vector2.one, new Vector2(4f, 0f), new Vector2(-4f, 0f), selected ? CielOSRunModesApp.Window : CielOSRunModesApp.Muted, TextAlignmentOptions.Center);
			tmp_Text.enableAutoSizing = true;
			tmp_Text.fontSizeMin = 9f;
			tmp_Text.fontSizeMax = 11f;
			tmp_Text.enableWordWrapping = wrap;
			button.onClick.AddListener(delegate
			{
				onClick();
			});
		}

		// Token: 0x06005D48 RID: 23880 RVA: 0x0024CFE8 File Offset: 0x0024B1E8
		private void StartRun(RunModeKind mode)
		{
			RunModeService.SaveFreeSettings(this._free);
			if (mode == RunModeKind.LongWayHome)
			{
				RunModeService.SaveLongWayHomeSettings(this._longWayHome);
			}
			if (CielOSAppLauncher.Instance == null)
			{
				this.SetNotice("Can't head out right now. Close this window and try again.");
				return;
			}
			ClassicRunKind classicRunKind = ((mode == RunModeKind.Classic) ? this._classicRunKind : ClassicRunKind.Normal);
			string text;
			if (!CielOSAppLauncher.Instance.StartRun(mode, this._free, classicRunKind, out text))
			{
				this.SetNotice(text);
				ToastService.Show(text, "Ciel couldn't head out", null, 6f, "cielos_settings");
			}
		}

		// Token: 0x06005D49 RID: 23881 RVA: 0x0024D06C File Offset: 0x0024B26C
		private void OpenCustomizationApp()
		{
			RunModeService.SaveFreeSettings(this._free);
			CielOSWindowManager cielOSWindowManager = Object.FindObjectOfType<CielOSWindowManager>(true);
			if (cielOSWindowManager == null || cielOSWindowManager.LaunchApp("customization") == null)
			{
				this.SetNotice("Can't change Ciel's look right now. Please try again.");
			}
		}

		// Token: 0x06005D4A RID: 23882 RVA: 0x0024D0B2 File Offset: 0x0024B2B2
		private void SaveAndBuild()
		{
			RunModeService.SaveFreeSettings(this._free);
			if (this._isBuilding || CanvasUpdateRegistry.IsRebuildingLayout() || CanvasUpdateRegistry.IsRebuildingGraphics())
			{
				return;
			}
			this.Build();
		}

		// Token: 0x06005D4B RID: 23883 RVA: 0x0024D0DC File Offset: 0x0024B2DC
		private void ResolveWeatherNames()
		{
			this._weatherNames.Clear();
			foreach (string text in DynamicWeatherPolicy.AuthoredPresetNames)
			{
				this._weatherNames.Add(CielOSRunModesApp.CleanWeatherName(text));
			}
			if (this._free.allowedWeatherPresetNames != null && this._free.allowedWeatherPresetNames.Length != 0)
			{
				List<string> list = new List<string>();
				string[] array = this._free.allowedWeatherPresetNames;
				for (int i = 0; i < array.Length; i++)
				{
					string allowed = array[i];
					string text2 = this._weatherNames.Find((string name) => DynamicWeatherPolicy.NormalizeName(name) == DynamicWeatherPolicy.NormalizeName(allowed));
					if (text2 != null && !list.Contains(text2))
					{
						list.Add(text2);
					}
				}
				this._free.allowedWeatherPresetNames = list.ToArray();
			}
			int num = this._weatherNames.FindIndex((string name) => string.Equals(name, this._free.weatherPresetName, StringComparison.OrdinalIgnoreCase));
			this._free.weatherIndex = ((num >= 0) ? num : Mathf.Clamp(this._free.weatherIndex, 0, this._weatherNames.Count - 1));
			this._free.weatherPresetName = this._weatherNames[this._free.weatherIndex];
			this.EnsureStartWeatherAllowed();
		}

		// Token: 0x06005D4C RID: 23884 RVA: 0x0024D220 File Offset: 0x0024B420
		private static string CleanWeatherName(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return "DEFAULT";
			}
			string text = value.Replace('_', ' ').Replace('-', ' ').Trim()
				.ToUpperInvariant();
			foreach (string text2 in new string[] { "WEATHER PRESET ", "WEATHER ", "PRESET ", "WP " })
			{
				if (text.StartsWith(text2, StringComparison.Ordinal))
				{
					return text.Substring(text2.Length).Trim();
				}
			}
			return text;
		}

		// Token: 0x06005D4D RID: 23885 RVA: 0x0024D2AF File Offset: 0x0024B4AF
		private IEnumerable<string> WeatherDisplayNames()
		{
			foreach (string text in this._weatherNames)
			{
				yield return CielOSRunModesApp.WeatherDisplayName(text);
			}
			List<string>.Enumerator enumerator = default(List<string>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06005D4E RID: 23886 RVA: 0x0024D2C0 File Offset: 0x0024B4C0
		private static string WeatherDisplayName(string name)
		{
			string text = DynamicWeatherPolicy.NormalizeName(name);
			if (text == "CALMRAIN")
			{
				return "LIGHT RAIN";
			}
			if (text == "FOGNESS")
			{
				return "FOG";
			}
			if (!(text == "SUNNYCONSTELLATION"))
			{
				return name;
			}
			return "STARRY SKY";
		}

		// Token: 0x06005D4F RID: 23887 RVA: 0x0024D310 File Offset: 0x0024B510
		private static string MultiplierLabel(float value)
		{
			return "x" + value.ToString("0.00", CultureInfo.InvariantCulture);
		}

		// Token: 0x06005D50 RID: 23888 RVA: 0x0024D330 File Offset: 0x0024B530
		private static List<string> HourOptions()
		{
			List<string> list = new List<string>(24);
			for (int i = 0; i < 24; i++)
			{
				list.Add(i.ToString("00", CultureInfo.InvariantCulture) + ":00");
			}
			return list;
		}

		// Token: 0x06005D51 RID: 23889 RVA: 0x0024D374 File Offset: 0x0024B574
		private static string CurrentRankName()
		{
			if (!(RankProgressionService.CurrentRank != null))
			{
				return CielOSRunModesApp.Localize("UNKNOWN");
			}
			return GameLocalization.GetSource("rank", RankProgressionService.CurrentRank.displayName).ToUpperInvariant();
		}

		// Token: 0x06005D52 RID: 23890 RVA: 0x0024D3A7 File Offset: 0x0024B5A7
		private void SetNotice(string message)
		{
			this._noticeEnglish = message;
			if (this._notice != null)
			{
				this._notice.text = CielOSRunModesApp.Localize(message ?? string.Empty).ToUpperInvariant();
			}
		}

		// Token: 0x06005D53 RID: 23891 RVA: 0x0024D3DD File Offset: 0x0024B5DD
		private static string Localize(string english)
		{
			return GameLocalization.GetSource("cielos_settings", english);
		}

		// Token: 0x06005D54 RID: 23892 RVA: 0x0024D3EC File Offset: 0x0024B5EC
		private static string Format(string english, params object[] arguments)
		{
			string text;
			GameLocalization.TryGetSourceKey("cielos_settings", english, out text);
			return GameLocalization.Format(text, english, arguments);
		}

		// Token: 0x06005D55 RID: 23893 RVA: 0x0024D410 File Offset: 0x0024B610
		private TMP_Text Text(string name, Transform parent, string value, float size, FontStyles style, Vector2 anchorMin, Vector2 anchorMax, Vector2 position, Vector2 dimensions, Color color, TextAlignmentOptions alignment)
		{
			TextMeshProUGUI textMeshProUGUI = CielOSRunModesApp.Rect(name, parent, anchorMin, anchorMax, position, dimensions).gameObject.AddComponent<TextMeshProUGUI>();
			textMeshProUGUI.text = CielOSRunModesApp.Localize(value);
			textMeshProUGUI.fontSize = size;
			textMeshProUGUI.fontStyle = style;
			textMeshProUGUI.color = color;
			textMeshProUGUI.alignment = alignment;
			textMeshProUGUI.enableWordWrapping = false;
			textMeshProUGUI.raycastTarget = false;
			textMeshProUGUI.overflowMode = TextOverflowModes.Ellipsis;
			if (CielOSTheme.Active != null && CielOSTheme.Active.uiFont != null)
			{
				textMeshProUGUI.font = CielOSTheme.Active.uiFont;
			}
			return textMeshProUGUI;
		}

		// Token: 0x06005D56 RID: 23894 RVA: 0x0024D4A8 File Offset: 0x0024B6A8
		private Button ActionButton(string name, Transform parent, string label, CielOSIconId icon, Vector2 anchorMin, Vector2 anchorMax, Vector2 position, Vector2 dimensions, Color background, Color textColor)
		{
			RectTransform rectTransform = CielOSRunModesApp.Rect(name, parent, anchorMin, anchorMax, position, dimensions);
			Image image = CielOSRunModesApp.AddImage(rectTransform, background);
			CielOSTheme.Round(image);
			Button button = rectTransform.gameObject.AddComponent<Button>();
			button.targetGraphic = image;
			button.colors = CielOSRunModesApp.ControlColors(CielOSRunModesApp.PalePink);
			CielOSSfx.Attach(button);
			Image image2 = CielOSRunModesApp.Image("Icon", rectTransform, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(18f, 0f), new Vector2(22f, 22f), Color.white);
			CielOSIcon.Apply(image2, icon, textColor, 64);
			image2.raycastTarget = false;
			this.Text("Label", rectTransform, label, 17f, FontStyles.Bold, Vector2.zero, Vector2.one, new Vector2(48f, 0f), new Vector2(-16f, 0f), textColor, TextAlignmentOptions.Center);
			return button;
		}

		// Token: 0x06005D57 RID: 23895 RVA: 0x0024D5A0 File Offset: 0x0024B7A0
		private Button TextActionButton(string name, Transform parent, string label, Vector2 anchor, Vector2 position, Vector2 dimensions, bool primary)
		{
			RectTransform rectTransform = CielOSRunModesApp.Rect(name, parent, anchor, anchor, position, dimensions);
			Image image = CielOSRunModesApp.AddImage(rectTransform, primary ? CielOSRunModesApp.Pink : new Color(CielOSRunModesApp.Row.r, CielOSRunModesApp.Row.g, CielOSRunModesApp.Row.b, 0.94f));
			CielOSTheme.Round(image);
			if (!primary)
			{
				CielOSTheme.Outline(image, new Color?(new Color(CielOSRunModesApp.PalePink.r, CielOSRunModesApp.PalePink.g, CielOSRunModesApp.PalePink.b, 0.2f)), 1f);
			}
			Button button = rectTransform.gameObject.AddComponent<Button>();
			button.targetGraphic = image;
			button.colors = CielOSRunModesApp.ControlColors(primary ? CielOSRunModesApp.PalePink : CielOSRunModesApp.RowHover);
			CielOSSfx.Attach(button);
			this.Text("Label", rectTransform, label, primary ? 17f : 14f, FontStyles.Bold, Vector2.zero, Vector2.one, new Vector2(16f, 0f), new Vector2(-16f, 0f), primary ? CielOSRunModesApp.Window : CielOSRunModesApp.PalePink, TextAlignmentOptions.Center).characterSpacing = (primary ? 0.8f : 1.2f);
			return button;
		}

		// Token: 0x06005D58 RID: 23896 RVA: 0x0024D6DC File Offset: 0x0024B8DC
		private static Sprite CardFadeSprite()
		{
			if (CielOSRunModesApp._cardFadeSprite != null)
			{
				return CielOSRunModesApp._cardFadeSprite;
			}
			Texture2D texture2D = new Texture2D(1, 96, TextureFormat.RGBA32, false)
			{
				name = "RunModesCardFade",
				filterMode = FilterMode.Bilinear,
				wrapMode = TextureWrapMode.Clamp,
				hideFlags = HideFlags.HideAndDontSave
			};
			for (int i = 0; i < 96; i++)
			{
				float num = (float)i / 95f;
				float num2 = 1f - num * num * (3f - 2f * num);
				texture2D.SetPixel(0, i, new Color(1f, 1f, 1f, num2));
			}
			texture2D.Apply(false, true);
			CielOSRunModesApp._cardFadeSprite = Sprite.Create(texture2D, new Rect(0f, 0f, 1f, 96f), new Vector2(0.5f, 0.5f), 100f);
			CielOSRunModesApp._cardFadeSprite.name = "RunModesCardFade";
			CielOSRunModesApp._cardFadeSprite.hideFlags = HideFlags.HideAndDontSave;
			return CielOSRunModesApp._cardFadeSprite;
		}

		// Token: 0x06005D59 RID: 23897 RVA: 0x0024D7D4 File Offset: 0x0024B9D4
		private static ColorBlock ControlColors(Color highlight)
		{
			return new ColorBlock
			{
				normalColor = Color.white,
				highlightedColor = new Color(1.08f, 1.08f, 1.08f, 1f),
				pressedColor = new Color(0.84f, 0.84f, 0.84f, 1f),
				selectedColor = Color.white,
				disabledColor = new Color(1f, 1f, 1f, 0.42f),
				colorMultiplier = 1f,
				fadeDuration = 0.08f
			};
		}

		// Token: 0x06005D5A RID: 23898 RVA: 0x0015358E File Offset: 0x0015178E
		private static Image AddImage(RectTransform target, Color color)
		{
			Image image = target.gameObject.AddComponent<Image>();
			image.color = color;
			return image;
		}

		// Token: 0x06005D5B RID: 23899 RVA: 0x0024D87A File Offset: 0x0024BA7A
		private static Image Image(string name, Transform parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 position, Vector2 dimensions, Color color)
		{
			return CielOSRunModesApp.AddImage(CielOSRunModesApp.Rect(name, parent, anchorMin, anchorMax, position, dimensions), color);
		}

		// Token: 0x06005D5C RID: 23900 RVA: 0x0024D890 File Offset: 0x0024BA90
		private static RectTransform Rect(string name, Transform parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 position, Vector2 dimensions)
		{
			RectTransform rectTransform = (RectTransform)new GameObject(name, new Type[] { typeof(RectTransform) }).transform;
			rectTransform.SetParent(parent, false);
			rectTransform.anchorMin = anchorMin;
			rectTransform.anchorMax = anchorMax;
			bool flag = !Mathf.Approximately(anchorMin.x, anchorMax.x);
			bool flag2 = !Mathf.Approximately(anchorMin.y, anchorMax.y);
			rectTransform.pivot = new Vector2(flag ? 0.5f : anchorMin.x, flag2 ? 0.5f : anchorMin.y);
			if (!flag && !flag2)
			{
				rectTransform.anchoredPosition = position;
				rectTransform.sizeDelta = dimensions;
			}
			else if (flag && flag2)
			{
				rectTransform.offsetMin = position;
				rectTransform.offsetMax = dimensions;
			}
			else if (flag)
			{
				rectTransform.offsetMin = new Vector2(position.x, rectTransform.offsetMin.y);
				rectTransform.offsetMax = new Vector2(dimensions.x, rectTransform.offsetMax.y);
				rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, position.y);
				rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, dimensions.y);
			}
			else
			{
				rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, position.y);
				rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, dimensions.y);
				rectTransform.anchoredPosition = new Vector2(position.x, rectTransform.anchoredPosition.y);
				rectTransform.sizeDelta = new Vector2(dimensions.x, rectTransform.sizeDelta.y);
			}
			return rectTransform;
		}

		// Token: 0x06005D5D RID: 23901 RVA: 0x0024DA50 File Offset: 0x0024BC50
		private void LateUpdate()
		{
			if (this.ConfigurationOpen && !this._isBuilding && this._freeViewport != null && (this._freeViewport.rect.size - this._freeViewportSize).sqrMagnitude > 0.25f)
			{
				this.Build();
			}
		}

		// Token: 0x06005D5E RID: 23902 RVA: 0x0024DAB0 File Offset: 0x0024BCB0
		private void BuildFree(RectTransform parent)
		{
			this._freePageRoot = parent;
			Image image = CielOSRunModesApp.Image("FreeEmblem", parent, Vector2.up, Vector2.up, new Vector2(0f, -2f), new Vector2(58f, 58f), CielOSRunModesApp.White);
			CielOSIcon.Apply(image, CielOSIconId.FreeMode, CielOSRunModesApp.White, 128);
			image.raycastTarget = false;
			this.Text("PageTitle", parent, "FREE MODE", 34f, FontStyles.Bold, Vector2.up, Vector2.one, new Vector2(76f, 0f), new Vector2(-200f, 40f), CielOSRunModesApp.White, TextAlignmentOptions.Left);
			this.Text("Subtitle", parent, "Make the city your own.", 16f, FontStyles.Normal, Vector2.up, Vector2.one, new Vector2(78f, -44f), new Vector2(-200f, 22f), CielOSRunModesApp.Muted, TextAlignmentOptions.Left);
			this.TextActionButton("BackToModes", parent, "BACK TO MODES", Vector2.one, new Vector2(0f, -8f), new Vector2(178f, 42f), false).onClick.AddListener(delegate
			{
				if (this.ControllerBack())
				{
					return;
				}
				this._freeConfigurationOpen = false;
				this.Build();
			});
			RectTransform rectTransform = CielOSRunModesApp.Rect("Categories", parent, Vector2.zero, Vector2.up, new Vector2(0f, 94f), new Vector2(200f, -90f));
			this.Text("Heading", rectTransform, "YOUR RULES", 11f, FontStyles.Bold, Vector2.up, Vector2.one, new Vector2(16f, 0f), new Vector2(0f, 20f), CielOSRunModesApp.Muted, TextAlignmentOptions.Left).characterSpacing = 2f;
			this.FreeTab(rectTransform, CielOSRunModesApp.FreePage.Start, "GETTING READY", CielOSIconId.Avatar, 0);
			this.FreeTab(rectTransform, CielOSRunModesApp.FreePage.Inventory, "INVENTORY", CielOSIconId.Files, 1);
			this.FreeTab(rectTransform, CielOSRunModesApp.FreePage.World, "WORLD", CielOSIconId.Weather, 2);
			this.FreeTab(rectTransform, CielOSRunModesApp.FreePage.Rules, "RULES", CielOSIconId.Settings, 3);
			this.FreeTab(rectTransform, CielOSRunModesApp.FreePage.BodyPaint, "BODYPAINT", CielOSIconId.Paint, 4);
			this.FreeTab(rectTransform, CielOSRunModesApp.FreePage.Vehicles, "VEHICLES", CielOSIconId.Maps, 5);
			RectTransform rectTransform2 = CielOSRunModesApp.Rect("SettingsPanel", parent, Vector2.zero, Vector2.one, new Vector2(220f, 90f), new Vector2(0f, -86f));
			Image image2 = CielOSRunModesApp.AddImage(rectTransform2, CielOSRunModesApp.FreeSurface);
			CielOSTheme.Round(image2);
			CielOSTheme.Outline(image2, new Color?(new Color(CielOSRunModesApp.PalePink.r, CielOSRunModesApp.PalePink.g, CielOSRunModesApp.PalePink.b, 0.12f)), 1f);
			string text;
			string text2;
			this.FreePageCopy(out text, out text2);
			this.Text("SectionTitle", rectTransform2, text, 22f, FontStyles.Bold, Vector2.up, Vector2.one, new Vector2(24f, -20f), new Vector2(-203f, 28f), CielOSRunModesApp.White, TextAlignmentOptions.Left);
			this.Text("SectionDescription", rectTransform2, text2, 14f, FontStyles.Normal, Vector2.up, Vector2.one, new Vector2(24f, -54f), new Vector2(-24f, 22f), CielOSRunModesApp.Muted, TextAlignmentOptions.Left);
			Button button = this.TextActionButton("ResetPage", rectTransform2, "RESET PAGE", Vector2.one, new Vector2(-20f, -18f), new Vector2(174f, 32f), false);
			TMP_Text componentInChildren = button.GetComponentInChildren<TMP_Text>();
			componentInChildren.fontSize = 11f;
			componentInChildren.enableAutoSizing = true;
			componentInChildren.fontSizeMin = 9f;
			componentInChildren.fontSizeMax = 11f;
			componentInChildren.enableWordWrapping = true;
			componentInChildren.characterSpacing = 0f;
			componentInChildren.rectTransform.offsetMin = new Vector2(8f, 0f);
			componentInChildren.rectTransform.offsetMax = new Vector2(-8f, 0f);
			button.onClick.AddListener(new UnityAction(this.ConfirmResetFreePage));
			RectTransform rectTransform3 = CielOSRunModesApp.Rect("SettingsViewport", rectTransform2, Vector2.zero, Vector2.one, new Vector2(18f, 18f), new Vector2(-28f, -90f));
			CielOSRunModesApp.AddImage(rectTransform3, Color.clear);
			rectTransform3.gameObject.AddComponent<RectMask2D>();
			RectTransform rectTransform4 = CielOSRunModesApp.Rect("SettingsList", rectTransform3, Vector2.up, Vector2.one, Vector2.zero, Vector2.zero);
			this._freeRowY = 0f;
			switch (this._freePage)
			{
			case CielOSRunModesApp.FreePage.Inventory:
				this.BuildInventoryRules(rectTransform4);
				break;
			case CielOSRunModesApp.FreePage.World:
				this.BuildWorldRules(rectTransform4);
				break;
			case CielOSRunModesApp.FreePage.Rules:
				this.BuildItemRules(rectTransform4);
				break;
			case CielOSRunModesApp.FreePage.Vehicles:
				this.BuildVehicleRules(rectTransform4);
				break;
			case CielOSRunModesApp.FreePage.BodyPaint:
				this.BuildBodyPaintRules(rectTransform4);
				break;
			default:
				this.BuildStartRules(rectTransform4);
				break;
			}
			rectTransform4.sizeDelta = new Vector2(0f, Mathf.Max(0f, this._freeRowY - 10f));
			ScrollRect scrollRect = rectTransform3.gameObject.AddComponent<ScrollRect>();
			scrollRect.viewport = rectTransform3;
			scrollRect.content = rectTransform4;
			scrollRect.horizontal = false;
			scrollRect.movementType = ScrollRect.MovementType.Clamped;
			rectTransform3.gameObject.AddComponent<SmoothCollectionScroll>().Configure(48f, 0.16f);
			RectTransform rectTransform5 = CielOSRunModesApp.Rect("SettingsScrollbar", rectTransform2, Vector2.right, Vector2.one, new Vector2(-11f, 18f), new Vector2(4f, -90f));
			CielOSRunModesApp.AddImage(rectTransform5, CielOSRunModesApp.Track);
			RectTransform rectTransform6 = CielOSRunModesApp.Rect("Handle", rectTransform5, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
			Image image3 = CielOSRunModesApp.AddImage(rectTransform6, new Color(CielOSRunModesApp.Pink.r, CielOSRunModesApp.Pink.g, CielOSRunModesApp.Pink.b, 0.7f));
			CielOSTheme.Round(image3);
			Scrollbar scrollbar = rectTransform5.gameObject.AddComponent<Scrollbar>();
			scrollbar.direction = Scrollbar.Direction.BottomToTop;
			scrollbar.handleRect = rectTransform6;
			scrollbar.targetGraphic = image3;
			scrollRect.verticalScrollbar = scrollbar;
			scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
			float num2;
			float num = (this._freeScrollPositions.TryGetValue(this._freePage, out num2) ? num2 : 1f);
			rectTransform4.anchoredPosition = new Vector2(0f, (1f - num) * Mathf.Max(0f, rectTransform4.rect.height - rectTransform3.rect.height));
			scrollbar.SetValueWithoutNotify(num);
			CielOSRunModesApp.FreePage current = this._freePage;
			scrollRect.onValueChanged.AddListener(delegate(Vector2 value)
			{
				this._freeScrollPositions[current] = Mathf.Clamp01(value.y);
			});
			this.BuildFreeFooter(parent);
		}

		// Token: 0x06005D5F RID: 23903 RVA: 0x0024E16C File Offset: 0x0024C36C
		private void FreeTab(RectTransform parent, CielOSRunModesApp.FreePage page, string label, CielOSIconId icon, int index)
		{
			bool flag = this._freePage == page;
			RectTransform rectTransform = CielOSRunModesApp.Rect(page.ToString(), parent, Vector2.up, Vector2.one, new Vector2(0f, -32f - (float)index * 51f), new Vector2(0f, 44f));
			Image image = CielOSRunModesApp.AddImage(rectTransform, flag ? new Color(CielOSRunModesApp.Pink.r, CielOSRunModesApp.Pink.g, CielOSRunModesApp.Pink.b, 0.14f) : Color.clear);
			CielOSTheme.Round(image);
			Button button = rectTransform.gameObject.AddComponent<Button>();
			button.targetGraphic = image;
			button.colors = CielOSRunModesApp.ControlColors(CielOSRunModesApp.RowHover);
			CielOSSfx.Attach(button);
			Image image2 = CielOSRunModesApp.Image("Icon", rectTransform, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(16f, 0f), new Vector2(22f, 22f), flag ? CielOSRunModesApp.Pink : CielOSRunModesApp.Muted);
			CielOSIcon.Apply(image2, icon, flag ? CielOSRunModesApp.Pink : CielOSRunModesApp.Muted, 64);
			image2.raycastTarget = false;
			this.Text("Label", rectTransform, label, 14f, FontStyles.Bold, Vector2.zero, Vector2.one, new Vector2(50f, 0f), new Vector2(-12f, 0f), flag ? CielOSRunModesApp.White : CielOSRunModesApp.Muted, TextAlignmentOptions.Left);
			if (flag)
			{
				CielOSRunModesApp.Image("Selected", rectTransform, Vector2.zero, Vector2.up, new Vector2(0f, 10f), new Vector2(3f, -10f), CielOSRunModesApp.Pink).raycastTarget = false;
			}
			button.onClick.AddListener(delegate
			{
				if (this._freePage == page)
				{
					return;
				}
				this._freePage = page;
				this.Build();
			});
		}

		// Token: 0x06005D60 RID: 23904 RVA: 0x0024E36C File Offset: 0x0024C56C
		private void FreePageCopy(out string title, out string description)
		{
			switch (this._freePage)
			{
			case CielOSRunModesApp.FreePage.Inventory:
				title = "STARTING GEAR";
				description = "Make room for your favourite finds.";
				return;
			case CielOSRunModesApp.FreePage.World:
				title = "WORLD";
				description = "Choose your map, time and weather.";
				return;
			case CielOSRunModesApp.FreePage.Rules:
				title = "RULES";
				description = "Decide how the city reacts and how long your gear lasts.";
				return;
			case CielOSRunModesApp.FreePage.Vehicles:
				title = "VEHICLES";
				description = "Take your time getting around.";
				return;
			case CielOSRunModesApp.FreePage.BodyPaint:
				title = "PAINT & PROTECTION";
				description = "Choose how long your bodypaint lasts.";
				return;
			default:
				title = "GETTING READY";
				description = "Choose your look and when to head out.";
				return;
			}
		}

		// Token: 0x06005D61 RID: 23905 RVA: 0x0024E3F8 File Offset: 0x0024C5F8
		private void BuildFreeFooter(RectTransform parent)
		{
			CielOSRunModesApp.Image("FooterDivider", parent, Vector2.zero, Vector2.right, new Vector2(0f, 78f), new Vector2(0f, 1f), new Color(CielOSRunModesApp.PalePink.r, CielOSRunModesApp.PalePink.g, CielOSRunModesApp.PalePink.b, 0.16f)).raycastTarget = false;
			this.Text("AutosaveHint", parent, "SETTINGS SAVED AUTOMATICALLY", 10f, FontStyles.Bold, Vector2.zero, Vector2.zero, new Vector2(2f, 5f), new Vector2(370f, 18f), CielOSRunModesApp.Muted, TextAlignmentOptions.Left);
			this._freeSummary = this.Text("RunSummary", parent, string.Empty, 18f, FontStyles.Bold, Vector2.zero, Vector2.right, new Vector2(2f, 30f), new Vector2(-574f, 28f), CielOSRunModesApp.White, TextAlignmentOptions.Left);
			this.UpdateFreeSummary();
			this.BuildPopulationSelector(parent, new Vector2(-272f, 0f), 280f);
			Button button = this.ActionButton("Start", parent, "START FREE MODE", CielOSIconId.Play, Vector2.right, Vector2.right, Vector2.zero, new Vector2(248f, 56f), CielOSRunModesApp.Pink, CielOSRunModesApp.Window);
			button.onClick.AddListener(delegate
			{
				this.StartRun(RunModeKind.Free);
			});
			if (this._controllerLayout)
			{
				this._controllerStart = button;
				ControllerButtonHint.Bind(button.gameObject, "Menu");
			}
			this._notice = this.Text("Notice", parent, string.Empty, 13f, FontStyles.Bold, Vector2.zero, Vector2.right, new Vector2(2f, 60f), new Vector2(-574f, 18f), CielOSRunModesApp.PalePink, TextAlignmentOptions.Left);
		}

		// Token: 0x06005D62 RID: 23906 RVA: 0x0024E5E4 File Offset: 0x0024C7E4
		private void UpdateFreeSummary()
		{
			if (this._freeSummary != null)
			{
				this._freeSummary.text = this._free.startHour.ToString("00") + ":00  /  " + CielOSRunModesApp.Localize(CielOSRunModesApp.WeatherDisplayName(this._free.weatherPresetName));
			}
		}

		// Token: 0x06005D63 RID: 23907 RVA: 0x0024E640 File Offset: 0x0024C840
		private RectTransform FreeSettingRow(RectTransform parent, string name, string title, string description, float height = 96f)
		{
			RectTransform rectTransform = CielOSRunModesApp.Rect(name, parent, Vector2.up, Vector2.one, new Vector2(0f, -this._freeRowY), new Vector2(0f, height));
			this._freeRowY += height + 10f;
			CielOSTheme.Round(CielOSRunModesApp.AddImage(rectTransform, CielOSRunModesApp.FreeRowColor));
			this.Text("Title", rectTransform, title, 16f, FontStyles.Bold, Vector2.up, new Vector2(0.49f, 1f), new Vector2(20f, -17f), new Vector2(-12f, 24f), CielOSRunModesApp.White, TextAlignmentOptions.Left);
			this.Text("Description", rectTransform, description, 13f, FontStyles.Normal, Vector2.zero, new Vector2(0.49f, 1f), new Vector2(20f, 12f), new Vector2(-16f, -46f), CielOSRunModesApp.Muted, TextAlignmentOptions.TopLeft).enableWordWrapping = true;
			return rectTransform;
		}

		// Token: 0x06005D64 RID: 23908 RVA: 0x0024E748 File Offset: 0x0024C948
		private RectTransform FreeControl(RectTransform row, string name = "Control", float height = 40f, float y = 0f)
		{
			return CielOSRunModesApp.Rect(name, row, new Vector2(0.51f, 0.5f), new Vector2(1f, 0.5f), new Vector2(0f, y), new Vector2(-18f, height));
		}

		// Token: 0x06005D65 RID: 23909 RVA: 0x0024E788 File Offset: 0x0024C988
		private void FreeChoices(RectTransform row, string[] labels, int selected, Action<int> setter, float y = 0f)
		{
			RectTransform rectTransform = this.FreeControl(row, "Choices", 40f, y);
			CielOSTheme.Round(CielOSRunModesApp.AddImage(rectTransform, CielOSRunModesApp.Track));
			for (int i = 0; i < labels.Length; i++)
			{
				int choice = i;
				this.ChoiceButton(rectTransform, "Choice_" + i.ToString(), labels[i], i, labels.Length, selected == i, delegate
				{
					if (choice == selected)
					{
						return;
					}
					setter(choice);
					this.SaveAndBuild();
				}, false);
				TMP_Text componentInChildren = rectTransform.GetChild(i).GetComponentInChildren<TMP_Text>();
				componentInChildren.fontSize = (componentInChildren.fontSizeMax = 12f);
			}
		}

		// Token: 0x06005D66 RID: 23910 RVA: 0x0024E854 File Offset: 0x0024CA54
		private void FreeSelector(RectTransform row, IEnumerable<string> options, int selected, Action<int> setter, bool enabled = true)
		{
			RectTransform rectTransform = this.FreeControl(row, "Selector", 40f, 0f);
			CielOSTheme.Round(CielOSRunModesApp.AddImage(rectTransform, CielOSRunModesApp.Track));
			Button button = this.ArrowButton(rectTransform, "Previous", "<", new Vector2(0f, 0.5f));
			Button button2 = this.ArrowButton(rectTransform, "Next", ">", new Vector2(1f, 0.5f));
			TMP_Text tmp_Text = this.Text("Value", rectTransform, "-", 18f, FontStyles.Bold, Vector2.zero, Vector2.one, new Vector2(44f, 0f), new Vector2(-44f, 0f), enabled ? CielOSRunModesApp.PalePink : CielOSRunModesApp.Muted, TextAlignmentOptions.Center);
			tmp_Text.enableAutoSizing = true;
			tmp_Text.fontSizeMin = 12f;
			tmp_Text.fontSizeMax = 18f;
			MainMenuArrowSelector selector = rectTransform.gameObject.AddComponent<MainMenuArrowSelector>();
			selector.Configure(tmp_Text, button, button2);
			selector.SetOptions(options, "cielos_settings");
			selector.SetIndexWithoutNotify(selected);
			button.interactable = (button2.interactable = enabled);
			selector.OnUserChangedIndex += delegate(int index)
			{
				if (!enabled)
				{
					selector.SetIndexWithoutNotify(selected);
					return;
				}
				setter(index);
				this.SaveAndBuild();
			};
		}

		// Token: 0x06005D67 RID: 23911 RVA: 0x0024E9D8 File Offset: 0x0024CBD8
		private void FreeSlider(RectTransform row, float value, float min, float max, float step, bool enabled, Action<float> setter, Func<float, string> format, bool compact = false)
		{
			RectTransform rectTransform = this.FreeControl(row, "DurationControl", compact ? 56f : 64f, 0f);
			TMP_Text number = this.Text("Value", rectTransform, format(value), compact ? 17f : 19f, FontStyles.Bold, Vector2.up, Vector2.one, Vector2.zero, new Vector2(0f, compact ? 26f : 25f), enabled ? CielOSRunModesApp.PalePink : CielOSRunModesApp.Muted, TextAlignmentOptions.Right);
			RectTransform rectTransform2 = CielOSRunModesApp.Rect("Slider", rectTransform, Vector2.zero, Vector2.right, new Vector2(8f, 0f), new Vector2(-8f, 28f));
			Slider slider = rectTransform2.gameObject.AddComponent<Slider>();
			slider.minValue = 0f;
			slider.maxValue = Mathf.Round((max - min) / step);
			slider.wholeNumbers = true;
			slider.SetValueWithoutNotify(Mathf.Round((value - min) / step));
			slider.interactable = enabled;
			CielOSTheme.Round(CielOSRunModesApp.Image("Track", rectTransform2, new Vector2(0f, 0.5f), new Vector2(1f, 0.5f), Vector2.zero, new Vector2(0f, 5f), CielOSRunModesApp.Track));
			CielOSRunModesApp.AddImage(rectTransform2, Color.clear);
			RectTransform rectTransform3 = CielOSRunModesApp.Rect("FillArea", rectTransform2, new Vector2(0f, 0.5f), new Vector2(1f, 0.5f), Vector2.zero, new Vector2(0f, 5f));
			Image image = CielOSRunModesApp.Image("Fill", rectTransform3, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero, enabled ? CielOSRunModesApp.Pink : CielOSRunModesApp.Muted);
			CielOSTheme.Round(image);
			image.raycastTarget = false;
			RectTransform rectTransform4 = CielOSRunModesApp.Rect("HandleArea", rectTransform2, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
			RectTransform rectTransform5 = CielOSRunModesApp.Rect("Handle", rectTransform4, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), Vector2.zero, new Vector2(18f, 18f));
			rectTransform5.pivot = new Vector2(0.5f, 0.5f);
			Image image2 = CielOSRunModesApp.AddImage(rectTransform5, enabled ? CielOSRunModesApp.PalePink : CielOSRunModesApp.Muted);
			CielOSTheme.Circle(image2);
			slider.handleRect = rectTransform5;
			slider.fillRect = image.rectTransform;
			slider.targetGraphic = image2;
			slider.colors = CielOSRunModesApp.ControlColors(CielOSRunModesApp.PalePink);
			rectTransform2.gameObject.AddComponent<MainMenuUiSoundRelay>();
			rectTransform.gameObject.AddComponent<CanvasGroup>().alpha = (enabled ? 1f : 0.4f);
			slider.onValueChanged.AddListener(delegate(float index)
			{
				if (this._isBuilding || CanvasUpdateRegistry.IsRebuildingLayout() || CanvasUpdateRegistry.IsRebuildingGraphics())
				{
					return;
				}
				float num = min + index * step;
				setter(num);
				number.text = format(num);
				RunModeService.SaveFreeSettings(this._free);
				this.UpdateFreeSummary();
			});
		}

		// Token: 0x06005D68 RID: 23912 RVA: 0x0024ED08 File Offset: 0x0024CF08
		private void BuildStartRules(RectTransform parent)
		{
			RectTransform rectTransform = this.FreeSettingRow(parent, "PresetCard", "SAVED OUTFIT", "Your saved outfit, or nothing at all.", 96f);
			this.FreeChoices(rectTransform, new string[] { "WEAR IT", "LEAVE IT" }, this._free.usePresetOutfit ? 0 : 1, delegate(int i)
			{
				this._free.usePresetOutfit = i == 0;
			}, 0f);
			RectTransform rectTransform2 = this.FreeSettingRow(parent, "TimeCard", "TIME TO LEAVE", "An early start or a late night?", 96f);
			this.FreeSelector(rectTransform2, CielOSRunModesApp.HourOptions(), this._free.startHour, delegate(int i)
			{
				this._free.startHour = i;
			}, true);
			RectTransform rectTransform3 = this.FreeSettingRow(parent, "CharacterCard", "CIEL'S LOOK", "A fresh look before heading out.", 96f);
			RectTransform rectTransform4 = this.FreeControl(rectTransform3, "Control", 40f, 0f);
			this.TextActionButton("Customize", rectTransform4, "CHANGE HER LOOK", new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(Mathf.Min(240f, rectTransform4.rect.width), 40f), false).onClick.AddListener(new UnityAction(this.OpenCustomizationApp));
		}

		// Token: 0x06005D69 RID: 23913 RVA: 0x0024EE44 File Offset: 0x0024D044
		private void BuildInventoryRules(RectTransform parent)
		{
			IReadOnlyList<InventoryItemSO> unlockedChoices = FreeRunLoadoutCatalog.GetUnlockedChoices();
			for (int i = 0; i < 3; i++)
			{
				int captured = i;
				InventoryItemSO inventoryItemSO = FreeRunLoadoutCatalog.ResolveEligible(this._free.startingItemIds[i]);
				RectTransform rectTransform = this.FreeSettingRow(parent, "StartingItem" + i.ToString(), CielOSRunModesApp.Format("SLOT {0}", new object[] { i + 1 }), (unlockedChoices.Count == 0) ? "Unlock items by playing to fill these slots." : "Choose one unlocked item to carry at the start.", 96f);
				RectTransform rectTransform2 = this.FreeControl(rectTransform, "ItemControl", 56f, 0f);
				Image image = CielOSRunModesApp.AddImage(rectTransform2, CielOSRunModesApp.Track);
				CielOSTheme.Round(image);
				Button button = rectTransform2.gameObject.AddComponent<Button>();
				button.targetGraphic = image;
				button.colors = CielOSRunModesApp.ControlColors(CielOSRunModesApp.RowHover);
				CielOSSfx.Attach(button);
				if (inventoryItemSO != null && inventoryItemSO.icon != null)
				{
					Image image2 = CielOSRunModesApp.Image("ItemIcon", rectTransform2, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(10f, 0f), new Vector2(38f, 38f), CielOSRunModesApp.White);
					image2.sprite = inventoryItemSO.icon;
					StreamerCensorImage.BindItem(image2, inventoryItemSO);
					image2.preserveAspect = true;
					image2.raycastTarget = false;
				}
				this.Text("ItemName", rectTransform2, (inventoryItemSO == null) ? "EMPTY" : InteractionLocalization.ObjectName(string.IsNullOrWhiteSpace(inventoryItemSO.itemName) ? inventoryItemSO.name : inventoryItemSO.itemName), 15f, FontStyles.Bold, Vector2.zero, Vector2.one, new Vector2((inventoryItemSO == null) ? 18f : 58f, 10f), new Vector2(-34f, -2f), (inventoryItemSO == null) ? CielOSRunModesApp.Muted : CielOSRunModesApp.White, TextAlignmentOptions.Left);
				this.Text("BrowseLabel", rectTransform2, "BROWSE ITEMS", 10f, FontStyles.Bold, Vector2.zero, Vector2.one, new Vector2((inventoryItemSO == null) ? 18f : 58f, 1f), new Vector2(-34f, -34f), CielOSRunModesApp.Pink, TextAlignmentOptions.Left);
				this.Text("Arrow", rectTransform2, ">", 22f, FontStyles.Normal, Vector2.right, Vector2.one, new Vector2(-24f, 0f), new Vector2(18f, 0f), CielOSRunModesApp.Pink, TextAlignmentOptions.Center);
				button.onClick.AddListener(delegate
				{
					this.OpenInventoryPicker(this._freePageRoot, captured);
				});
			}
		}

		// Token: 0x06005D6A RID: 23914 RVA: 0x0024F10C File Offset: 0x0024D30C
		private void BuildWorldRules(RectTransform parent)
		{
			this.BuildWorldSeedRules(parent);
			RectTransform rectTransform = this.FreeSettingRow(parent, "TimeFlowCard", "LET TIME PASS", "Let day turn to night, or hold on to your favourite hour.", 96f);
			this.FreeChoices(rectTransform, new string[] { "FLOWING", "FROZEN" }, this._free.timeProgressionEnabled ? 0 : 1, delegate(int i)
			{
				this._free.timeProgressionEnabled = i == 0;
			}, 0f);
			RectTransform rectTransform2 = this.FreeSettingRow(parent, "DayLength", "A DAY LASTS", "Real minutes for a full day and night.", 96f);
			this.FreeSlider(rectTransform2, this._free.dayLengthMinutes, 5f, 120f, 1f, this._free.timeProgressionEnabled, delegate(float v)
			{
				this._free.dayLengthMinutes = v;
			}, (float v) => CielOSRunModesApp.Format("{0} MIN", new object[] { Mathf.RoundToInt(v) }), false);
			RectTransform rectTransform3 = this.FreeSettingRow(parent, "WeatherCard", "CHANGING SKIES", "Let the skies change, or keep one forecast.", 96f);
			this.FreeChoices(rectTransform3, new string[] { "CHANGING", "FIXED" }, this._free.variableWeather ? 0 : 1, delegate(int i)
			{
				this._free.variableWeather = i == 0;
			}, 0f);
			RectTransform rectTransform4 = this.FreeSettingRow(parent, "StartWeather", "START WITH", "The sky at the start of your run.", 96f);
			this.FreeSelector(rectTransform4, this.WeatherDisplayNames(), this._free.weatherIndex, delegate(int i)
			{
				this._free.weatherIndex = i;
				this._free.weatherPresetName = this._weatherNames[i];
				this.EnsureStartWeatherAllowed();
			}, true);
			RectTransform rectTransform5 = this.FreeSettingRow(parent, "WeatherInterval", "NEW WEATHER EVERY", "Real minutes between weather changes.", 96f);
			this.FreeSlider(rectTransform5, this._free.weatherChangeIntervalMinutes, 1f, 20f, 1f, this._free.variableWeather, delegate(float v)
			{
				this._free.weatherChangeIntervalMinutes = v;
			}, (float v) => CielOSRunModesApp.Format("{0} MIN", new object[] { Mathf.RoundToInt(v) }), false);
			this.BuildWeatherPool(parent);
			this.FreeInfo(parent, "The same seed recreates the same city layout in the same game version.");
		}

		// Token: 0x06005D6B RID: 23915 RVA: 0x0024F314 File Offset: 0x0024D514
		private void BuildWeatherPool(RectTransform parent)
		{
			int count = this._weatherNames.Count;
			int num = Mathf.CeilToInt((float)count / 3f);
			RectTransform rectTransform = this.FreeSettingRow(parent, "WeatherPoolCard", "WEATHER POOL", "Choose the weather types that can appear. Keep at least one selected.", 108f + (float)num * 40f);
			rectTransform.Find("Title").GetComponent<RectTransform>().anchorMax = Vector2.one;
			RectTransform component = rectTransform.Find("Description").GetComponent<RectTransform>();
			component.anchorMin = Vector2.up;
			component.anchorMax = Vector2.one;
			component.pivot = Vector2.up;
			component.offsetMin = new Vector2(20f, -86f);
			component.offsetMax = new Vector2(-20f, -46f);
			bool flag = this._free.allowedWeatherPresetNames == null || this._free.allowedWeatherPresetNames.Length == 0;
			for (int i = 0; i < count; i++)
			{
				string name = this._weatherNames[i];
				bool flag2 = flag || Array.Exists<string>(this._free.allowedWeatherPresetNames, (string n) => string.Equals(n, name, StringComparison.OrdinalIgnoreCase));
				int num2 = i % 3;
				RectTransform rectTransform2 = CielOSRunModesApp.Rect("Weather_" + i.ToString(), rectTransform, new Vector2((float)num2 / 3f, 1f), new Vector2(((float)num2 + 1f) / 3f, 1f), new Vector2(18f, -96f - (float)(i / 3) * 40f), new Vector2(-10f, 32f));
				Image image = CielOSRunModesApp.AddImage(rectTransform2, flag2 ? CielOSRunModesApp.RowHover : CielOSRunModesApp.Track);
				CielOSTheme.Round(image);
				Button button = rectTransform2.gameObject.AddComponent<Button>();
				button.targetGraphic = image;
				button.colors = CielOSRunModesApp.ControlColors(CielOSRunModesApp.PalePink);
				button.interactable = this._free.variableWeather;
				CielOSSfx.Attach(button);
				Image image2 = CielOSRunModesApp.Image("Check", rectTransform2, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(10f, 0f), new Vector2(15f, 15f), flag2 ? CielOSRunModesApp.Pink : CielOSRunModesApp.Muted);
				CielOSIcon.Apply(image2, flag2 ? CielOSIconId.Check : CielOSIconId.Weather, flag2 ? CielOSRunModesApp.Pink : CielOSRunModesApp.Muted, 48);
				image2.raycastTarget = false;
				this.Text("Name", rectTransform2, CielOSRunModesApp.WeatherDisplayName(name), 12f, FontStyles.Bold, Vector2.zero, Vector2.one, new Vector2(34f, 0f), new Vector2(-8f, 0f), flag2 ? CielOSRunModesApp.White : CielOSRunModesApp.Muted, TextAlignmentOptions.Left);
				button.onClick.AddListener(delegate
				{
					this.ToggleWeatherInPool(name);
				});
			}
		}

		// Token: 0x06005D6C RID: 23916 RVA: 0x0024F618 File Offset: 0x0024D818
		private void BuildItemRules(RectTransform parent)
		{
			RectTransform rectTransform = this.FreeSettingRow(parent, "HomeReturnCard", "RETURN HOME", "Allow returning home through the front door to end this run.", 96f);
			this.FreeChoices(rectTransform, new string[] { "YES", "NO" }, this._free.homeReturnEnabled ? 0 : 1, delegate(int i)
			{
				this._free.homeReturnEnabled = i == 0;
			}, 0f);
			RectTransform rectTransform2 = this.FreeSettingRow(parent, "DetectionCard", "NPC REACTIONS", "Choose how passers-by respond to Ciel.", 96f);
			this.FreeChoices(rectTransform2, new string[] { "IGNORE CIEL", "REACT ONLY", "ENCOUNTERS" }, (!this._free.detectionEnabled) ? 0 : (this._free.encountersEnabled ? 2 : 1), delegate(int i)
			{
				this._free.detectionEnabled = i != 0;
				this._free.encountersEnabled = i == 2;
			}, 0f);
			FreeNpcDifficulty[] difficultyLevels = new FreeNpcDifficulty[]
			{
				FreeNpcDifficulty.VeryEasy,
				FreeNpcDifficulty.Easy,
				FreeNpcDifficulty.Default,
				FreeNpcDifficulty.Hard,
				FreeNpcDifficulty.Extreme
			};
			int num = Array.IndexOf<FreeNpcDifficulty>(difficultyLevels, this._free.npcDifficulty);
			RectTransform rectTransform3 = this.FreeSettingRow(parent, "NpcDifficultyCard", "NPC AGGRESSIVENESS", "Adjust how far NPCs can see Ciel.", 96f);
			this.FreeSelector(rectTransform3, new string[] { "VERY EASY", "EASY", "DEFAULT", "HARD", "EXTREME" }, (num < 0) ? 2 : num, delegate(int i)
			{
				this._free.npcDifficulty = difficultyLevels[i];
			}, this._free.detectionEnabled);
			RectTransform rectTransform4 = this.FreeSettingRow(parent, "StaminaCard", "STAMINA", "Use stamina normally, or keep it full for movement and actions.", 96f);
			this.FreeChoices(rectTransform4, new string[] { "NORMAL", "INFINITE" }, this._free.infiniteStamina ? 1 : 0, delegate(int i)
			{
				this._free.infiniteStamina = i == 1;
			}, 0f);
			RectTransform rectTransform5 = this.FreeSettingRow(parent, "LootCard", "TRASHCAN LOOT", "Choose how much loot you find in trashcans.", 96f);
			this.FreeChoices(rectTransform5, new string[] { "SCARCE", "NORMAL", "ABUNDANT" }, (int)this._free.lootAmount, delegate(int i)
			{
				this._free.lootAmount = (FreeLootAmount)i;
			}, 0f);
			RectTransform rectTransform6 = this.FreeSettingRow(parent, "ClothingCard", "CLOTHING DURATION", "Choose how long clothes last. 2× doubles the duration.", 112f);
			this.FreeChoices(rectTransform6, new string[] { "TIMED", "INFINITE" }, this._free.clothingDurabilityEnabled ? 0 : 1, delegate(int i)
			{
				this._free.clothingDurabilityEnabled = i == 0;
			}, 28f);
			this.FreeSlider(rectTransform6, this._free.clothingDurationMultiplier, 0.25f, 5f, 0.25f, this._free.clothingDurabilityEnabled, delegate(float v)
			{
				this._free.clothingDurationMultiplier = v;
			}, new Func<float, string>(CielOSRunModesApp.MultiplierLabel), true);
			rectTransform6.Find("DurationControl").GetComponent<RectTransform>().anchoredPosition += new Vector2(0f, -20f);
			RectTransform rectTransform7 = this.FreeSettingRow(parent, "ItemsCard", "ITEM DURATION & USES", "Items can run out of time or uses. 2× doubles timers.", 112f);
			this.FreeChoices(rectTransform7, new string[] { "LIMITED", "INFINITE" }, this._free.itemDurabilityEnabled ? 0 : 1, delegate(int i)
			{
				this._free.itemDurabilityEnabled = i == 0;
			}, 28f);
			this.FreeSlider(rectTransform7, this._free.itemDurationMultiplier, 0.25f, 5f, 0.25f, this._free.itemDurabilityEnabled, delegate(float v)
			{
				this._free.itemDurationMultiplier = v;
			}, new Func<float, string>(CielOSRunModesApp.MultiplierLabel), true);
			rectTransform7.Find("DurationControl").GetComponent<RectTransform>().anchoredPosition += new Vector2(0f, -20f);
		}

		// Token: 0x06005D6D RID: 23917 RVA: 0x0024FA04 File Offset: 0x0024DC04
		private void BuildBodyPaintRules(RectTransform parent)
		{
			RectTransform rectTransform = this.FreeSettingRow(parent, "BodyPaintPenUses", "BODYPAINT PEN USES", "Choose limited or infinite uses for preset and custom bodypaint pens.", 96f);
			this.FreeChoices(rectTransform, new string[] { "LIMITED", "INFINITE" }, this._free.infiniteBodyPaintPenUses ? 1 : 0, delegate(int i)
			{
				this._free.infiniteBodyPaintPenUses = i == 1;
			}, 0f);
			RectTransform rectTransform2 = this.FreeSettingRow(parent, "BodyPaintMode", "BODYPAINT DURATION", "Use each paint’s usual duration, choose a timer, or keep it indefinitely.", 106f);
			this.FreeChoices(rectTransform2, new string[] { "DEFAULT", "TIMED", "INFINITE" }, (int)this._free.bodyPaintDurationMode, delegate(int i)
			{
				this._free.bodyPaintDurationMode = (BodyPaintDurationMode)i;
			}, 0f);
			RectTransform rectTransform3 = this.FreeSettingRow(parent, "BodyPaintTimer", "PAINT LASTS FOR", "Applies to mud, tint, preset and custom bodypaint.", 96f);
			this.FreeSlider(rectTransform3, this._free.bodyPaintDurationSeconds, 30f, 1800f, 30f, this._free.bodyPaintDurationMode == BodyPaintDurationMode.Custom, delegate(float v)
			{
				this._free.bodyPaintDurationSeconds = v;
			}, delegate(float v)
			{
				if (this._free.bodyPaintDurationMode != BodyPaintDurationMode.Custom)
				{
					return CielOSRunModesApp.Localize((this._free.bodyPaintDurationMode == BodyPaintDurationMode.Infinite) ? "INFINITE" : "DEFAULT");
				}
				return CielOSRunModesApp.BodyPaintTimeLabel(v);
			}, false);
			RectTransform rectTransform4 = this.FreeSettingRow(parent, "BodyPaintRain", "RAIN WASHES PAINT", "Let rain wash bodypaint away, or protect it from the weather.", 96f);
			this.FreeChoices(rectTransform4, new string[] { "WASHES OFF", "PROTECTED" }, this._free.bodyPaintRainWashEnabled ? 0 : 1, delegate(int i)
			{
				this._free.bodyPaintRainWashEnabled = i == 0;
			}, 0f);
			this.FreeInfo(parent, "Infinite duration stops the timer. Rain can still wash paint away when enabled above.");
		}

		// Token: 0x06005D6E RID: 23918 RVA: 0x0024FB94 File Offset: 0x0024DD94
		private static string BodyPaintTimeLabel(float seconds)
		{
			int num = Mathf.RoundToInt(seconds);
			if (num < 60)
			{
				return CielOSRunModesApp.Format("{0} SEC", new object[] { num });
			}
			if (num % 60 != 0)
			{
				return (num / 60).ToString() + ":" + (num % 60).ToString("00");
			}
			return CielOSRunModesApp.Format("{0} MIN", new object[] { num / 60 });
		}

		// Token: 0x06005D6F RID: 23919 RVA: 0x0024FC14 File Offset: 0x0024DE14
		private void BuildVehicleRules(RectTransform parent)
		{
			RectTransform rectTransform = this.FreeSettingRow(parent, "VehiclesCard", "VEHICLE DURATION", "Vehicle and traffic rides use their normal time limits, or last until you choose to leave.", 106f);
			this.FreeChoices(rectTransform, new string[] { "TIMED RIDES", "UNLIMITED" }, this._free.vehicleDurationEnabled ? 0 : 1, delegate(int i)
			{
				this._free.vehicleDurationEnabled = i == 0;
			}, 0f);
			RectTransform rectTransform2 = this.FreeSettingRow(parent, "VehicleSpawnRate", "VEHICLE SPAWN RATE", "How often rideable vehicles appear. 0× none, 1× normal.", 96f);
			this.FreeSlider(rectTransform2, this._free.vehicleSpawnMultiplier, 0f, 5f, 0.25f, true, delegate(float v)
			{
				this._free.vehicleSpawnMultiplier = v;
			}, new Func<float, string>(CielOSRunModesApp.MultiplierLabel), false);
			this.FreeInfo(parent, "Clothing and item timers are configured in Rules.");
		}

		// Token: 0x06005D70 RID: 23920 RVA: 0x0024FCE0 File Offset: 0x0024DEE0
		private void FreeInfo(RectTransform parent, string message)
		{
			RectTransform rectTransform = CielOSRunModesApp.Rect("Info", parent, Vector2.up, Vector2.one, new Vector2(14f, -this._freeRowY), new Vector2(-14f, 60f));
			this._freeRowY += 70f;
			Image image = CielOSRunModesApp.Image("Icon", rectTransform, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), Vector2.zero, new Vector2(20f, 20f), CielOSRunModesApp.Pink);
			CielOSIcon.Apply(image, CielOSIconId.Info, CielOSRunModesApp.Pink, 64);
			image.raycastTarget = false;
			this.Text("Text", rectTransform, message, 13f, FontStyles.Normal, Vector2.zero, Vector2.one, new Vector2(34f, 0f), Vector2.zero, CielOSRunModesApp.Muted, TextAlignmentOptions.Left).enableWordWrapping = true;
		}

		// Token: 0x06005D71 RID: 23921 RVA: 0x0024FDD0 File Offset: 0x0024DFD0
		private void ConfirmResetFreePage()
		{
			MenuDialogShell shell = MenuDialogShell.Create(this._freePageRoot, "ResetConfirmation", (CielOSTheme.Active != null) ? CielOSTheme.Active.uiFont : null, new Vector2(510f, 230f));
			RectTransform card = shell.Card;
			card.name = "Dialog";
			this.Text("Title", card, "RESTORE DEFAULTS?", 24f, FontStyles.Bold, Vector2.up, Vector2.one, new Vector2(28f, -25f), new Vector2(-28f, 36f), CielOSRunModesApp.White, TextAlignmentOptions.Left);
			this.Text("Description", card, "Reset the settings on this page? Your other pages will stay as they are.", 17f, FontStyles.Normal, Vector2.up, Vector2.one, new Vector2(28f, -78f), new Vector2(-28f, 66f), CielOSRunModesApp.Muted, TextAlignmentOptions.TopLeft).enableWordWrapping = true;
			Button button = this.TextActionButton("Cancel", card, "CANCEL", Vector2.zero, new Vector2(28f, 24f), new Vector2(210f, 44f), false);
			Button button2 = this.TextActionButton("Reset", card, "RESET", Vector2.right, new Vector2(-28f, 24f), new Vector2(210f, 44f), true);
			Action close = delegate
			{
				shell.Close(true);
				if (Application.isPlaying)
				{
					Object.Destroy(shell.gameObject);
					return;
				}
				Object.DestroyImmediate(shell.gameObject);
			};
			button.onClick.AddListener(delegate
			{
				close();
			});
			button2.onClick.AddListener(new UnityAction(this.ResetFreePage));
			MenuDialogShell.SetNavigation(new Button[] { button, button2 });
			foreach (TMP_Text tmp_Text in card.GetComponentsInChildren<TMP_Text>(true))
			{
				(tmp_Text.GetComponent<LocalizedGameText>() ?? LocalizedGameText.BindFontOnly(tmp_Text)).FitTranslationToRectangle(false);
			}
			Transform transform = this._freePageRoot.Find("SettingsPanel/ResetPage");
			shell.Show(button, (transform != null) ? transform.gameObject : null, close);
		}

		// Token: 0x06005D72 RID: 23922 RVA: 0x00250004 File Offset: 0x0024E204
		private void ResetFreePage()
		{
			FreeRunSettings freeRunSettings = new FreeRunSettings();
			switch (this._freePage)
			{
			case CielOSRunModesApp.FreePage.Start:
				this._free.usePresetOutfit = freeRunSettings.usePresetOutfit;
				this._free.startHour = freeRunSettings.startHour;
				break;
			case CielOSRunModesApp.FreePage.Inventory:
				this._free.startingItemIds = freeRunSettings.startingItemIds;
				break;
			case CielOSRunModesApp.FreePage.World:
				this._free.useCustomWorldSeed = freeRunSettings.useCustomWorldSeed;
				this._free.worldSeed = freeRunSettings.worldSeed;
				this._free.timeProgressionEnabled = freeRunSettings.timeProgressionEnabled;
				this._free.dayLengthMinutes = freeRunSettings.dayLengthMinutes;
				this._free.variableWeather = freeRunSettings.variableWeather;
				this._free.weatherPresetName = freeRunSettings.weatherPresetName;
				this._free.allowedWeatherPresetNames = freeRunSettings.allowedWeatherPresetNames;
				this._free.weatherChangeIntervalMinutes = freeRunSettings.weatherChangeIntervalMinutes;
				this.ResolveWeatherNames();
				break;
			case CielOSRunModesApp.FreePage.Rules:
				this._free.homeReturnEnabled = freeRunSettings.homeReturnEnabled;
				this._free.detectionEnabled = freeRunSettings.detectionEnabled;
				this._free.encountersEnabled = freeRunSettings.encountersEnabled;
				this._free.npcDifficulty = freeRunSettings.npcDifficulty;
				this._free.infiniteStamina = freeRunSettings.infiniteStamina;
				this._free.lootAmount = freeRunSettings.lootAmount;
				this._free.clothingDurabilityEnabled = freeRunSettings.clothingDurabilityEnabled;
				this._free.clothingDurationMultiplier = freeRunSettings.clothingDurationMultiplier;
				this._free.itemDurabilityEnabled = freeRunSettings.itemDurabilityEnabled;
				this._free.itemDurationMultiplier = freeRunSettings.itemDurationMultiplier;
				break;
			case CielOSRunModesApp.FreePage.Vehicles:
				this._free.vehicleDurationEnabled = freeRunSettings.vehicleDurationEnabled;
				this._free.vehicleSpawnMultiplier = freeRunSettings.vehicleSpawnMultiplier;
				break;
			case CielOSRunModesApp.FreePage.BodyPaint:
				this._free.infiniteBodyPaintPenUses = freeRunSettings.infiniteBodyPaintPenUses;
				this._free.bodyPaintDurationMode = freeRunSettings.bodyPaintDurationMode;
				this._free.bodyPaintDurationSeconds = freeRunSettings.bodyPaintDurationSeconds;
				this._free.bodyPaintRainWashEnabled = freeRunSettings.bodyPaintRainWashEnabled;
				break;
			}
			this._freeScrollPositions[this._freePage] = 1f;
			this.SaveAndBuild();
		}

		// Token: 0x17000F32 RID: 3890
		// (get) Token: 0x06005D73 RID: 23923 RVA: 0x00250241 File Offset: 0x0024E441
		private bool ConfigurationOpen
		{
			get
			{
				return this._freeConfigurationOpen || this._longWayHomeConfigurationOpen;
			}
		}

		// Token: 0x06005D74 RID: 23924 RVA: 0x00250254 File Offset: 0x0024E454
		private void BuildLongWayHomeConfiguration(RectTransform parent)
		{
			this._freeViewport = parent;
			this._freeViewportSize = parent.rect.size;
			RectTransform rectTransform = CielOSRunModesApp.Rect("LongWayHomeConfiguration", parent, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
			RectTransform rectTransform2 = CielOSRunModesApp.Rect("Inner", rectTransform, Vector2.zero, Vector2.one, new Vector2(8f, 0f), new Vector2(-8f, 0f));
			float num = Mathf.Min(1.25f, Mathf.Min(rectTransform2.rect.width / 1100f, rectTransform2.rect.height / 625f));
			if (num > 0f)
			{
				Vector2 vector = rectTransform2.rect.size / num;
				RectTransform rectTransform3 = rectTransform2;
				RectTransform rectTransform4 = rectTransform2;
				Vector2 vector2 = new Vector2(0.5f, 0.5f);
				rectTransform4.anchorMax = vector2;
				rectTransform3.anchorMin = vector2;
				rectTransform2.sizeDelta = vector;
				rectTransform2.anchoredPosition = Vector2.zero;
				rectTransform2.localScale = Vector3.one * num;
			}
			this.Text("PageTitle", rectTransform2, "LONG WAY HOME", 34f, FontStyles.Bold, Vector2.up, Vector2.one, Vector2.zero, new Vector2(-200f, 40f), CielOSRunModesApp.White, TextAlignmentOptions.Left);
			this.Text("Subtitle", rectTransform2, "Choose where Ciel's journey home begins.", 16f, FontStyles.Normal, Vector2.up, Vector2.one, new Vector2(2f, -44f), new Vector2(-200f, 22f), CielOSRunModesApp.Muted, TextAlignmentOptions.Left);
			this.TextActionButton("BackToModes", rectTransform2, "BACK TO MODES", Vector2.one, new Vector2(0f, -8f), new Vector2(178f, 42f), false).onClick.AddListener(delegate
			{
				if (this.ControllerBack())
				{
					return;
				}
				this._longWayHomeConfigurationOpen = (this._controllerConfigurationOpen = false);
				this.Build();
			});
			this.Text("SceneHeading", rectTransform2, "STARTING SCENE", 12f, FontStyles.Bold, Vector2.up, Vector2.one, new Vector2(2f, -78f), new Vector2(0f, 20f), CielOSRunModesApp.Muted, TextAlignmentOptions.Left).characterSpacing = 1.8f;
			RectTransform rectTransform5 = CielOSRunModesApp.Rect("StartScenes", rectTransform2, Vector2.zero, Vector2.one, new Vector2(0f, 294f), new Vector2(0f, -112f));
			this.BuildLongWayHomeStartCard(rectTransform5, LongWayHomePrologue.Restaurant, 0, "CITY", "RESTAURANT", "After dinner with friends, Ciel wakes up a long way from home.", (this.restaurantStartBanner != null) ? this.restaurantStartBanner : Resources.Load<Sprite>("LongWayHome/StartPreviews/Restaurant"));
			this.BuildLongWayHomeStartCard(rectTransform5, LongWayHomePrologue.GymShower, 1, "MALL", "GYM SHOWERS", "Someone steals Ciel's clothes while she's in the shower.", (this.gymShowerStartBanner != null) ? this.gymShowerStartBanner : Resources.Load<Sprite>("LongWayHome/StartPreviews/GymShower"));
			this.BuildLongWayHomeStartCard(rectTransform5, LongWayHomePrologue.University, 2, "UNIVERSITY", "LECTURE HALL", "Ciel falls asleep alone on campus and wakes up surrounded by students.", (this.universityStartBanner != null) ? this.universityStartBanner : Resources.Load<Sprite>("LongWayHome/StartPreviews/University"));
			this.BuildLongWayHomeStartCard(rectTransform5, LongWayHomePrologue.Stadium, 3, "STADIUM", "THE PITCH", "Ciel streaks across a full stadium with only a bag over her head.", (this.stadiumStartBanner != null) ? this.stadiumStartBanner : Resources.Load<Sprite>("LongWayHome/StartPreviews/Stadium"));
			this.BuildLongWayHomeUpcomingCard(rectTransform2);
			RectTransform rectTransform6 = CielOSRunModesApp.Rect("Options", rectTransform2, Vector2.zero, Vector2.right, new Vector2(0f, 92f), new Vector2(0f, 110f));
			this.BuildLongWayHomeSelectors(rectTransform6);
			CielOSRunModesApp.Image("FooterDivider", rectTransform2, Vector2.zero, Vector2.right, new Vector2(0f, 78f), new Vector2(0f, 1f), new Color(CielOSRunModesApp.PalePink.r, CielOSRunModesApp.PalePink.g, CielOSRunModesApp.PalePink.b, 0.16f)).raycastTarget = false;
			string text = ((this._longWayHome.prologue == LongWayHomePrologue.GymShower) ? "MALL" : ((this._longWayHome.prologue == LongWayHomePrologue.University) ? "UNIVERSITY" : ((this._longWayHome.prologue == LongWayHomePrologue.Stadium) ? "STADIUM" : "CITY")));
			string text2 = ((this._longWayHome.prologue == LongWayHomePrologue.University) ? "NIGHT TO MORNING" : ((this._longWayHome.prologue == LongWayHomePrologue.Stadium) ? "DAY" : (this._longWayHome.startAtNight ? "NIGHT" : "DAY")));
			this.Text("RunSummary", rectTransform2, CielOSRunModesApp.Localize(text) + "  /  " + CielOSRunModesApp.Localize(text2), 18f, FontStyles.Bold, Vector2.zero, Vector2.right, new Vector2(2f, 30f), new Vector2(-280f, 28f), CielOSRunModesApp.White, TextAlignmentOptions.Left);
			this.Text("AutosaveHint", rectTransform2, "SETTINGS SAVED AUTOMATICALLY", 10f, FontStyles.Bold, Vector2.zero, Vector2.right, new Vector2(2f, 5f), new Vector2(-280f, 18f), CielOSRunModesApp.Muted, TextAlignmentOptions.Left);
			Button button = this.ActionButton("Start", rectTransform2, "START RUN", CielOSIconId.Play, Vector2.right, Vector2.right, Vector2.zero, new Vector2(248f, 56f), CielOSRunModesApp.Pink, CielOSRunModesApp.Window);
			button.onClick.AddListener(delegate
			{
				this.StartRun(RunModeKind.LongWayHome);
			});
			if (this._controllerLayout)
			{
				this._controllerStart = button;
				ControllerButtonHint.Bind(button.gameObject, "Menu");
			}
			this._notice = this.Text("Notice", rectTransform2, string.Empty, 13f, FontStyles.Bold, Vector2.zero, Vector2.right, new Vector2(2f, 60f), new Vector2(-280f, 18f), CielOSRunModesApp.PalePink, TextAlignmentOptions.Left);
		}

		// Token: 0x06005D75 RID: 23925 RVA: 0x00250848 File Offset: 0x0024EA48
		private void BuildLongWayHomeStartCard(RectTransform parent, LongWayHomePrologue prologue, int index, string location, string title, string description, Sprite preview)
		{
			bool flag = this._longWayHome.prologue == prologue;
			bool flag2 = index == 0;
			bool flag3 = index == 3;
			RectTransform rectTransform = CielOSRunModesApp.Rect(prologue.ToString(), parent, new Vector2((float)index * 0.25f, 0f), new Vector2((float)(index + 1) * 0.25f, 1f), new Vector2(flag2 ? 0f : 7f, 0f), new Vector2(flag3 ? 0f : (-7f), 0f));
			Image image = CielOSRunModesApp.AddImage(rectTransform, flag ? CielOSRunModesApp.Pink : new Color(CielOSRunModesApp.PalePink.r, CielOSRunModesApp.PalePink.g, CielOSRunModesApp.PalePink.b, 0.24f));
			CielOSTheme.Round(image);
			float num = (flag ? 3f : 1f);
			RectTransform rectTransform2 = CielOSRunModesApp.Rect("Card", rectTransform, Vector2.zero, Vector2.one, Vector2.one * num, -Vector2.one * num);
			CielOSTheme.Round(CielOSRunModesApp.AddImage(rectTransform2, CielOSRunModesApp.FreeSurface));
			rectTransform2.gameObject.AddComponent<Mask>().showMaskGraphic = true;
			RectTransform rectTransform3 = CielOSRunModesApp.Rect("Banner", rectTransform2, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
			Image image2 = CielOSRunModesApp.AddImage(rectTransform3, Color.white);
			StreamerArtworkImage.Bind(image2, preview);
			image2.raycastTarget = false;
			if (preview != null)
			{
				AspectRatioFitter aspectRatioFitter = rectTransform3.gameObject.AddComponent<AspectRatioFitter>();
				aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
				aspectRatioFitter.aspectRatio = preview.rect.width / preview.rect.height;
			}
			CielOSRunModesApp.Image("Shade", rectTransform2, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero, new Color(CielOSRunModesApp.Overlay.r, CielOSRunModesApp.Overlay.g, CielOSRunModesApp.Overlay.b, flag ? 0.04f : 0.22f)).raycastTarget = false;
			Image image3 = CielOSRunModesApp.Image("TitleFade", rectTransform2, Vector2.zero, new Vector2(1f, 0.66f), Vector2.zero, Vector2.zero, new Color(CielOSRunModesApp.Overlay.r, CielOSRunModesApp.Overlay.g, CielOSRunModesApp.Overlay.b, 0.98f));
			image3.sprite = CielOSRunModesApp.CardFadeSprite();
			image3.raycastTarget = false;
			RectTransform rectTransform4 = CielOSRunModesApp.Rect("LocationBadge", rectTransform2, Vector2.up, Vector2.up, new Vector2(16f, -14f), new Vector2(124f, 28f));
			CielOSTheme.Round(CielOSRunModesApp.AddImage(rectTransform4, flag ? CielOSRunModesApp.Pink : CielOSRunModesApp.Window));
			this.Text("Label", rectTransform4, location, 12f, FontStyles.Bold, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero, flag ? CielOSRunModesApp.Window : CielOSRunModesApp.White, TextAlignmentOptions.Center);
			if (flag)
			{
				this.Text("Selected", rectTransform2, "SELECTED", 11f, FontStyles.Bold, Vector2.one, Vector2.one, new Vector2(-14f, -14f), new Vector2(96f, 28f), CielOSRunModesApp.White, TextAlignmentOptions.Right);
			}
			this.Text("Title", rectTransform2, title, 22f, FontStyles.Bold, Vector2.zero, Vector2.right, new Vector2(18f, 62f), new Vector2(-18f, 32f), CielOSRunModesApp.White, TextAlignmentOptions.Left);
			this.Text("Description", rectTransform2, description, 12f, FontStyles.Normal, Vector2.zero, Vector2.right, new Vector2(18f, 10f), new Vector2(-18f, 50f), CielOSRunModesApp.PalePink, TextAlignmentOptions.TopLeft).enableWordWrapping = true;
			Button button = rectTransform.gameObject.AddComponent<Button>();
			button.targetGraphic = image;
			button.colors = CielOSRunModesApp.ControlColors(CielOSRunModesApp.RowHover);
			CielOSSfx.Attach(button);
			rectTransform.gameObject.AddComponent<MainMenuButtonFx>().SetTintChildGraphics(false);
			button.onClick.AddListener(delegate
			{
				if (this._longWayHome.prologue == prologue)
				{
					return;
				}
				this._longWayHome.prologue = prologue;
				this.SaveLongWayHomeAndBuild();
			});
		}

		// Token: 0x06005D76 RID: 23926 RVA: 0x00250CA4 File Offset: 0x0024EEA4
		private void BuildLongWayHomeUpcomingCard(RectTransform parent)
		{
			RectTransform rectTransform = CielOSRunModesApp.Rect("WorkInProgress", parent, Vector2.zero, Vector2.right, new Vector2(0f, 216f), new Vector2(0f, 62f));
			Image image = CielOSRunModesApp.AddImage(rectTransform, CielOSRunModesApp.FreeRowColor);
			image.raycastTarget = false;
			CielOSTheme.Round(image);
			CielOSTheme.Outline(image, new Color?(new Color(CielOSRunModesApp.PalePink.r, CielOSRunModesApp.PalePink.g, CielOSRunModesApp.PalePink.b, 0.18f)), 1f);
			RectTransform rectTransform2 = CielOSRunModesApp.Rect("Preview", rectTransform, Vector2.zero, Vector2.zero, new Vector2(12f, 8f), new Vector2(82f, 46f));
			Image image2 = CielOSRunModesApp.AddImage(rectTransform2, CielOSRunModesApp.Track);
			image2.raycastTarget = false;
			CielOSTheme.Round(image2);
			this.Text("QuestionMark", rectTransform2, "?", 36f, FontStyles.Bold, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero, CielOSRunModesApp.PalePink, TextAlignmentOptions.Center);
			this.Text("Title", rectTransform, "WORK IN PROGRESS", 16f, FontStyles.Bold, Vector2.zero, Vector2.right, new Vector2(112f, 31f), new Vector2(-20f, 22f), CielOSRunModesApp.PalePink, TextAlignmentOptions.Left);
			this.Text("Description", rectTransform, "More starting scenes are in development.", 13f, FontStyles.Normal, Vector2.zero, Vector2.right, new Vector2(112f, 9f), new Vector2(-20f, 20f), CielOSRunModesApp.Muted, TextAlignmentOptions.Left);
		}

		// Token: 0x06005D77 RID: 23927 RVA: 0x00250E4C File Offset: 0x0024F04C
		private void BuildLongWayHomeSelectors(RectTransform parent)
		{
			bool flag = this._longWayHome.prologue == LongWayHomePrologue.Restaurant;
			int num = (flag ? 3 : 2);
			if (flag)
			{
				RectTransform rectTransform = this.LongWayHomeOption(parent, "InitialLoot", "SOME SUPPLIES", "Start with a few clothing finds nearby.", 0, num);
				this.ChoiceButton(rectTransform, "On", "YES", 0, 2, this._longWayHome.startingLootEnabled, delegate
				{
					this._longWayHome.startingLootEnabled = true;
					this.SaveLongWayHomeAndBuild();
				}, false);
				this.ChoiceButton(rectTransform, "Off", "NO", 1, 2, !this._longWayHome.startingLootEnabled, delegate
				{
					this._longWayHome.startingLootEnabled = false;
					this.SaveLongWayHomeAndBuild();
				}, false);
			}
			if (this._longWayHome.prologue == LongWayHomePrologue.University)
			{
				RectTransform rectTransform2 = this.LongWayHomeOption(parent, "RunStartTime", "LEAVE AT", "You wake up when the morning bell rings.", 0, num);
				this.ChoiceButton(rectTransform2, "Morning", "NIGHT TO MORNING", 0, 1, true, delegate
				{
				}, false);
			}
			else if (this._longWayHome.prologue == LongWayHomePrologue.Stadium)
			{
				RectTransform rectTransform3 = this.LongWayHomeOption(parent, "RunStartTime", "LEAVE AT", "The match is played in broad daylight.", 0, num);
				this.ChoiceButton(rectTransform3, "Day", "DAY", 0, 1, true, delegate
				{
				}, false);
			}
			else
			{
				RectTransform rectTransform4 = this.LongWayHomeOption(parent, "RunStartTime", "LEAVE AT", "Choose the time of day for your journey.", flag ? 1 : 0, num);
				this.ChoiceButton(rectTransform4, "Day", "DAY", 0, 2, !this._longWayHome.startAtNight, delegate
				{
					this._longWayHome.startAtNight = false;
					this.SaveLongWayHomeAndBuild();
				}, false);
				this.ChoiceButton(rectTransform4, "Night", "NIGHT", 1, 2, this._longWayHome.startAtNight, delegate
				{
					this._longWayHome.startAtNight = true;
					this.SaveLongWayHomeAndBuild();
				}, false);
			}
			RectTransform rectTransform5 = this.LongWayHomeOption(parent, "NpcPopulation", "THE CROWD", "Choose who you meet along the way.", num - 1, num);
			this.PopulationChoiceButton(rectTransform5, NpcPopulationMode.Mixed, "EVERYONE", 0);
			this.PopulationChoiceButton(rectTransform5, NpcPopulationMode.MaleOnly, "MEN", 1);
			this.PopulationChoiceButton(rectTransform5, NpcPopulationMode.FemaleOnly, "WOMEN", 2);
		}

		// Token: 0x06005D78 RID: 23928 RVA: 0x00251068 File Offset: 0x0024F268
		private RectTransform LongWayHomeOption(RectTransform parent, string name, string title, string description, int index, int count)
		{
			RectTransform rectTransform = CielOSRunModesApp.Rect(name, parent, new Vector2((float)index / (float)count, 0f), new Vector2(((float)index + 1f) / (float)count, 1f), new Vector2((index == 0) ? 0f : 7f, 0f), new Vector2((index == count - 1) ? 0f : (-7f), 0f));
			CielOSTheme.Round(CielOSRunModesApp.AddImage(rectTransform, CielOSRunModesApp.FreeRowColor));
			this.Text("Label", rectTransform, title, 13f, FontStyles.Bold, Vector2.up, Vector2.one, new Vector2(20f, -10f), new Vector2(-20f, 20f), CielOSRunModesApp.White, TextAlignmentOptions.Left);
			this.Text("Description", rectTransform, description, 12f, FontStyles.Normal, Vector2.zero, Vector2.right, new Vector2(20f, 7f), new Vector2(-20f, 28f), CielOSRunModesApp.Muted, TextAlignmentOptions.TopLeft).enableWordWrapping = true;
			RectTransform rectTransform2 = CielOSRunModesApp.Rect("Selector", rectTransform, Vector2.zero, Vector2.right, new Vector2(18f, 40f), new Vector2(-18f, 32f));
			CielOSTheme.Round(CielOSRunModesApp.AddImage(rectTransform2, CielOSRunModesApp.Track));
			return rectTransform2;
		}

		// Token: 0x06005D79 RID: 23929 RVA: 0x002511C4 File Offset: 0x0024F3C4
		private void BuildWorldSeedRules(RectTransform parent)
		{
			RectTransform rectTransform = this.FreeSettingRow(parent, "WorldSeedNumber", "WORLD SEED", "Leave empty for a random world, or enter a shared seed.", 118f);
			RectTransform rectTransform2 = this.FreeControl(rectTransform, "SeedInput", 38f, 22f);
			rectTransform2.gameObject.SetActive(false);
			Image image = CielOSRunModesApp.AddImage(rectTransform2, CielOSRunModesApp.Track);
			CielOSTheme.Round(image);
			TMP_InputField input = rectTransform2.gameObject.AddComponent<TMP_InputField>();
			input.targetGraphic = image;
			input.lineType = TMP_InputField.LineType.SingleLine;
			input.characterLimit = 64;
			input.interactable = true;
			input.customCaretColor = true;
			input.caretColor = CielOSRunModesApp.PalePink;
			input.selectionColor = new Color(CielOSRunModesApp.Pink.r, CielOSRunModesApp.Pink.g, CielOSRunModesApp.Pink.b, 0.35f);
			RectTransform rectTransform3 = CielOSRunModesApp.Rect("Viewport", rectTransform2, Vector2.zero, Vector2.one, new Vector2(12f, 4f), new Vector2(-12f, -4f));
			rectTransform3.gameObject.AddComponent<RectMask2D>();
			TMP_Text tmp_Text = this.Text("Value", rectTransform3, string.Empty, 18f, FontStyles.Bold, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero, CielOSRunModesApp.PalePink, TextAlignmentOptions.Left);
			tmp_Text.overflowMode = TextOverflowModes.Overflow;
			tmp_Text.richText = false;
			LocalizedGameText.BindFontOnly(tmp_Text);
			input.textViewport = rectTransform3;
			input.textComponent = tmp_Text;
			TMP_Text tmp_Text2 = this.Text("Placeholder", rectTransform3, "Random world", 16f, FontStyles.Normal, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero, CielOSRunModesApp.Muted, TextAlignmentOptions.Left);
			input.placeholder = tmp_Text2;
			input.SetTextWithoutNotify(this._free.useCustomWorldSeed ? this._free.worldSeed.ToString(CultureInfo.InvariantCulture) : string.Empty);
			input.onValueChanged.AddListener(delegate(string raw)
			{
				if (!this._isBuilding)
				{
					this.SaveWorldSeedInput(raw);
				}
			});
			input.onEndEdit.AddListener(delegate(string raw)
			{
				if (this._isBuilding)
				{
					return;
				}
				if (this.SaveWorldSeedInput(raw))
				{
					this.SetNotice(string.Empty);
				}
				else
				{
					this.SetNotice("Enter a valid seed number.");
				}
				input.SetTextWithoutNotify(this._free.useCustomWorldSeed ? this._free.worldSeed.ToString(CultureInfo.InvariantCulture) : string.Empty);
			});
			RectTransform rectTransform4 = this.FreeControl(rectTransform, "SeedActions", 32f, -24f);
			Button button = this.TextActionButton("PasteSeed", rectTransform4, "PASTE SEED", new Vector2(1f, 0.5f), Vector2.zero, new Vector2(Mathf.Min(180f, rectTransform4.rect.width), 32f), false);
			button.GetComponentInChildren<TMP_Text>().fontSize = 12f;
			button.onClick.AddListener(delegate
			{
				int num;
				if (!CielOSRunModesApp.TryParseWorldSeed(GUIUtility.systemCopyBuffer, out num))
				{
					this.SetNotice("Clipboard does not contain a valid seed.");
					return;
				}
				input.text = num.ToString(CultureInfo.InvariantCulture);
				this.SaveWorldSeedInput(input.text);
				this.SetNotice(string.Empty);
			});
			rectTransform2.gameObject.SetActive(true);
		}

		// Token: 0x06005D7A RID: 23930 RVA: 0x002514B4 File Offset: 0x0024F6B4
		private bool SaveWorldSeedInput(string raw)
		{
			bool flag = !string.IsNullOrWhiteSpace(raw);
			int num = 0;
			if (flag && !CielOSRunModesApp.TryParseWorldSeed(raw, out num))
			{
				return false;
			}
			if (this._free.useCustomWorldSeed != flag || this._free.worldSeed != num)
			{
				this._free.useCustomWorldSeed = flag;
				this._free.worldSeed = num;
				RunModeService.SaveFreeSettings(this._free);
			}
			return true;
		}

		// Token: 0x06005D7B RID: 23931 RVA: 0x0025151C File Offset: 0x0024F71C
		internal static bool TryParseWorldSeed(string raw, out int seed)
		{
			return int.TryParse((raw != null) ? raw.Trim() : null, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out seed);
		}

		// Token: 0x04005E72 RID: 24178
		private bool _controllerLayout;

		// Token: 0x04005E73 RID: 24179
		private bool _controllerConfigurationOpen;

		// Token: 0x04005E74 RID: 24180
		private bool _focusControllerPage;

		// Token: 0x04005E75 RID: 24181
		private Button _controllerStart;

		// Token: 0x04005E76 RID: 24182
		private static readonly Color Overlay = new Color(0.012f, 0.006f, 0.016f, 0.96f);

		// Token: 0x04005E77 RID: 24183
		private static readonly Color Window = new Color(0.075f, 0.035f, 0.075f, 0.99f);

		// Token: 0x04005E78 RID: 24184
		private static readonly Color Surface = new Color(0.21f, 0.095f, 0.19f, 0.98f);

		// Token: 0x04005E79 RID: 24185
		private static readonly Color Row = new Color(0.165f, 0.118f, 0.2f, 0.9f);

		// Token: 0x04005E7A RID: 24186
		private static readonly Color RowHover = new Color(0.243f, 0.165f, 0.298f, 1f);

		// Token: 0x04005E7B RID: 24187
		private static readonly Color Track = new Color(0.035f, 0.015f, 0.035f, 1f);

		// Token: 0x04005E7C RID: 24188
		private static readonly Color Pink = new Color(1f, 0.31f, 0.61f, 1f);

		// Token: 0x04005E7D RID: 24189
		private static readonly Color PalePink = new Color(1f, 0.78f, 0.89f, 1f);

		// Token: 0x04005E7E RID: 24190
		private static readonly Color White = new Color(1f, 0.97f, 1f, 1f);

		// Token: 0x04005E7F RID: 24191
		private static readonly Color Muted = new Color(0.78f, 0.67f, 0.76f, 1f);

		// Token: 0x04005E80 RID: 24192
		private const float ModeCardGap = 20f;

		// Token: 0x04005E81 RID: 24193
		private const float SliderHandleWidth = 70f;

		// Token: 0x04005E82 RID: 24194
		private static Sprite _cardFadeSprite;

		// Token: 0x04005E83 RID: 24195
		private FreeRunSettings _free;

		// Token: 0x04005E84 RID: 24196
		private ClassicRunSettings _classic;

		// Token: 0x04005E85 RID: 24197
		private LongWayHomeRunSettings _longWayHome;

		// Token: 0x04005E86 RID: 24198
		private RunModeKind _selected;

		// Token: 0x04005E87 RID: 24199
		private ClassicRunKind _classicRunKind;

		// Token: 0x04005E88 RID: 24200
		private CielOSRunModesApp.FreePage _freePage;

		// Token: 0x04005E89 RID: 24201
		private bool _freeConfigurationOpen;

		// Token: 0x04005E8A RID: 24202
		[SerializeField]
		private Sprite classicBanner;

		// Token: 0x04005E8B RID: 24203
		[SerializeField]
		private Sprite freeBanner;

		// Token: 0x04005E8C RID: 24204
		[SerializeField]
		private Sprite longWayHomeBanner;

		// Token: 0x04005E8D RID: 24205
		[SerializeField]
		private Sprite heartPattern;

		// Token: 0x04005E8E RID: 24206
		private RectTransform _content;

		// Token: 0x04005E8F RID: 24207
		private TMP_Text _notice;

		// Token: 0x04005E90 RID: 24208
		private string _noticeEnglish;

		// Token: 0x04005E91 RID: 24209
		private NpcPopulationMode _displayedPopulationMode;

		// Token: 0x04005E92 RID: 24210
		private readonly List<string> _weatherNames = new List<string>();

		// Token: 0x04005E93 RID: 24211
		private bool _isBuilding;

		// Token: 0x04005E94 RID: 24212
		private float _freeRowY;

		// Token: 0x04005E95 RID: 24213
		private RectTransform _freePageRoot;

		// Token: 0x04005E96 RID: 24214
		private RectTransform _freeViewport;

		// Token: 0x04005E97 RID: 24215
		private Vector2 _freeViewportSize;

		// Token: 0x04005E98 RID: 24216
		private TMP_Text _freeSummary;

		// Token: 0x04005E99 RID: 24217
		private readonly Dictionary<CielOSRunModesApp.FreePage, float> _freeScrollPositions = new Dictionary<CielOSRunModesApp.FreePage, float>();

		// Token: 0x04005E9A RID: 24218
		private static readonly Color FreeSurface = new Color(0.04f, 0.019f, 0.054f, 0.98f);

		// Token: 0x04005E9B RID: 24219
		private static readonly Color FreeRowColor = new Color(0.08f, 0.046f, 0.099f, 1f);

		// Token: 0x04005E9C RID: 24220
		private bool _longWayHomeConfigurationOpen;

		// Token: 0x04005E9D RID: 24221
		[SerializeField]
		private Sprite restaurantStartBanner;

		// Token: 0x04005E9E RID: 24222
		[SerializeField]
		private Sprite gymShowerStartBanner;

		// Token: 0x04005E9F RID: 24223
		[SerializeField]
		private Sprite universityStartBanner;

		// Token: 0x04005EA0 RID: 24224
		[SerializeField]
		private Sprite stadiumStartBanner;

		// Token: 0x04005EA1 RID: 24225
		private const int LongWayHomeStartCardCount = 4;

		// Token: 0x02000C86 RID: 3206
		private enum FreePage
		{
			// Token: 0x04005EA3 RID: 24227
			Start,
			// Token: 0x04005EA4 RID: 24228
			Inventory,
			// Token: 0x04005EA5 RID: 24229
			World,
			// Token: 0x04005EA6 RID: 24230
			Rules,
			// Token: 0x04005EA7 RID: 24231
			Vehicles,
			// Token: 0x04005EA8 RID: 24232
			BodyPaint
		}
	}
}
