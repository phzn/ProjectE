using System;
using System.Collections.Generic;
using Features.Localization;
using Features.Missions;
using Features.Tutorials;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.Rendering;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.MainMenu
{
	// Token: 0x02001075 RID: 4213
	public sealed class MainMenuSettingsPanel : MonoBehaviour
	{
		// Token: 0x17001485 RID: 5253
		// (get) Token: 0x06007FF3 RID: 32755 RVA: 0x00343C34 File Offset: 0x00341E34
		public bool IsOnSubPage
		{
			get
			{
				return MainMenuSettingsPanel.IsActive(this.audioPage) || MainMenuSettingsPanel.IsActive(this.gamePage) || MainMenuSettingsPanel.IsActive(this.graphicsPage) || MainMenuSettingsPanel.IsActive(this.advancedGraphicsPage) || MainMenuSettingsPanel.IsActive(this.missionsPage) || MainMenuSettingsPanel.IsActive(this.controlsPage) || MainMenuSettingsPanel.IsActive(this.tutorialsPage) || MainMenuSettingsPanel.IsActive(this._hudPage) || MainMenuSettingsPanel.IsActive(this._worldGraphicsPage);
			}
		}

		// Token: 0x06007FF4 RID: 32756 RVA: 0x00343CB8 File Offset: 0x00341EB8
		public void ConfigureRuntime(MainMenuSettingsPanel.RuntimeBindings bindings)
		{
			if (bindings == null)
			{
				return;
			}
			this.rootPage = bindings.rootPage;
			this.audioPage = bindings.audioPage;
			this.gamePage = bindings.gamePage;
			this.graphicsPage = bindings.graphicsPage;
			this.advancedGraphicsPage = bindings.advancedGraphicsPage;
			this.missionsPage = bindings.missionsPage;
			this.controlsPage = bindings.controlsPage;
			this.tutorialsPage = bindings.tutorialsPage;
			this.tutorialPanel = bindings.tutorialPanel;
			this.progressionPanel = bindings.progressionPanel;
			this._unstuckButton = bindings.unstuckButton;
			this.RefreshUnstuckAvailability();
			this.pageTitle = bindings.pageTitle;
			this.pageKicker = bindings.pageKicker;
			if (!string.IsNullOrWhiteSpace(bindings.rootKicker))
			{
				this._rootKicker = bindings.rootKicker;
			}
			this._contextHint = bindings.contextHint;
			this.masterSlider = bindings.masterSlider;
			this.masterValue = bindings.masterValue;
			this.heartbeatSlider = bindings.heartbeatSlider;
			this.heartbeatValue = bindings.heartbeatValue;
			this.cielVoiceSlider = bindings.cielVoiceSlider;
			this.cielVoiceValue = bindings.cielVoiceValue;
			this.npcReactionSlider = bindings.npcReactionSlider;
			this.npcReactionValue = bindings.npcReactionValue;
			this.musicSlider = bindings.musicSlider;
			this.musicValue = bindings.musicValue;
			this.sfxSlider = bindings.sfxSlider;
			this.sfxValue = bindings.sfxValue;
			this.muteToggle = bindings.muteToggle;
			this.sensitivitySlider = bindings.sensitivitySlider;
			this.sensitivityValue = bindings.sensitivityValue;
			this.cielVoiceSubtitlesToggle = bindings.cielVoiceSubtitlesToggle;
			this.cameraShakeToggle = bindings.cameraShakeToggle;
			this.streamerModeToggle = bindings.streamerModeToggle;
			this.showFpsToggle = bindings.showFpsToggle;
			this.phoneVideoResolutionSelector = bindings.phoneVideoResolutionSelector;
			this.phoneVideoFpsSelector = bindings.phoneVideoFpsSelector;
			this.qualitySelector = bindings.qualitySelector;
			this.resolutionSelector = bindings.resolutionSelector;
			this.windowModeSelector = bindings.windowModeSelector;
			this.fpsSelector = bindings.fpsSelector;
			this.vSyncToggle = bindings.vSyncToggle;
			this.renderScaleSelector = bindings.renderScaleSelector;
			this.shadowQualitySelector = bindings.shadowQualitySelector;
			this.viewDistanceSelector = bindings.viewDistanceSelector;
			this.textureQualitySelector = bindings.textureQualitySelector;
			this.ssaoToggle = bindings.ssaoToggle;
			this.postProcessingToggle = bindings.postProcessingToggle;
			this.magicaClothToggle = bindings.magicaClothToggle;
			this.colorVisionSelector = bindings.colorVisionSelector;
		}

		// Token: 0x06007FF5 RID: 32757 RVA: 0x00343F28 File Offset: 0x00342128
		private void Awake()
		{
			this._intro = base.GetComponent<MainMenuPanelIntroAnimator>();
			this.EnsureHudSettings();
			this.EnsureWorldGraphicsSettings();
			this.EnsureBrightnessSettings();
			this.EnsureGarbageMudSettings();
			this.EnsureBodyPhysicsSettings();
			this.EnsureMouseInversionSettings();
			MainMenuSettingsRuntimeFactory.ApplyLocalizedTextLayout(base.transform);
			this.BuildStaticOptions();
			this.BindEvents();
		}

		// Token: 0x06007FF6 RID: 32758 RVA: 0x00343F7C File Offset: 0x0034217C
		private void OnEnable()
		{
			this.RefreshUnstuckAvailability();
			this.EnsureHudSettings();
			this.EnsureWorldGraphicsSettings();
			this.EnsureBrightnessSettings();
			this.EnsureLanguageSettings();
			this.EnsureStreamerPixelSettings();
			this.EnsureBodyPhysicsSettings();
			this.EnsureMouseInversionSettings();
			MainMenuSettingsRuntimeFactory.FitGameSettingsRows((this.gamePage != null) ? this.gamePage.transform : null);
			MainMenuSettingsRuntimeFactory.ApplyLocalizedTextLayout(base.transform);
			GameLocalization.LocaleChanged += this.RefreshLanguageSettings;
			GameLocalization.LoadingChanged += this.RefreshLanguageSettings;
			this.ShowPage(this.rootPage, false);
			this.RefreshFromCurrentSettings();
		}

		// Token: 0x06007FF7 RID: 32759 RVA: 0x0034401A File Offset: 0x0034221A
		private void OnDisable()
		{
			GameLocalization.LocaleChanged -= this.RefreshLanguageSettings;
			GameLocalization.LoadingChanged -= this.RefreshLanguageSettings;
		}

		// Token: 0x06007FF8 RID: 32760 RVA: 0x0034403E File Offset: 0x0034223E
		private void Update()
		{
			this.RefreshUnstuckAvailability();
		}

		// Token: 0x06007FF9 RID: 32761 RVA: 0x00344046 File Offset: 0x00342246
		private void RefreshUnstuckAvailability()
		{
			if (this._unstuckButton != null)
			{
				this._unstuckButton.interactable = PlayerUnstuckService.IsRecoveryAllowed;
			}
		}

		// Token: 0x06007FFA RID: 32762 RVA: 0x00344068 File Offset: 0x00342268
		private void EnsureLanguageSettings()
		{
			if (this._languageSelector != null || this.gamePage == null)
			{
				return;
			}
			GameLocalizationConfig config = GameLocalization.Config;
			if (config == null)
			{
				return;
			}
			this._languageSelector = MainMenuSettingsRuntimeFactory.BuildLanguageSelector(this.gamePage.transform, LocalizedGameText.SourceFont(this.pageTitle));
			this._languageSelector.SetLanguages(config.languages, LocalizedGameText.SourceFont(this.pageTitle));
			this._languageSelector.onValueChanged.AddListener(new UnityAction<int>(this.SelectLanguage));
			MainMenuSettingsRuntimeFactory.FitGameSettingsRows(this.gamePage.transform);
		}

		// Token: 0x06007FFB RID: 32763 RVA: 0x0034410C File Offset: 0x0034230C
		private void EnsureMouseInversionSettings()
		{
			if (this.gamePage == null)
			{
				return;
			}
			if (this._invertMouseX == null)
			{
				this._invertMouseX = MainMenuSettingsRuntimeFactory.BuildMouseInversionSetting(this.gamePage.transform, LocalizedGameText.SourceFont(this.pageTitle), true);
				this._invertMouseX.onValueChanged.AddListener(delegate(bool value)
				{
					this.Change(delegate
					{
						GameSettingsService.SetInvertMouseX(value);
					});
				});
			}
			if (this._invertMouseY == null)
			{
				this._invertMouseY = MainMenuSettingsRuntimeFactory.BuildMouseInversionSetting(this.gamePage.transform, LocalizedGameText.SourceFont(this.pageTitle), false);
				this._invertMouseY.onValueChanged.AddListener(delegate(bool value)
				{
					this.Change(delegate
					{
						GameSettingsService.SetInvertMouseY(value);
					});
				});
			}
		}

		// Token: 0x06007FFC RID: 32764 RVA: 0x003441C0 File Offset: 0x003423C0
		private void EnsureBrightnessSettings()
		{
			if (this._brightnessSlider != null || this.graphicsPage == null)
			{
				return;
			}
			this._brightnessSlider = MainMenuSettingsRuntimeFactory.BuildBrightnessSetting(this.graphicsPage.transform, LocalizedGameText.SourceFont(this.pageTitle), out this._brightnessValue);
			this.Bind(this._brightnessSlider, delegate(float value)
			{
				this.ChangeGraphics(delegate(GraphicsSettingsModel model)
				{
					model.brightness = value;
				});
			});
		}

		// Token: 0x06007FFD RID: 32765 RVA: 0x0034422C File Offset: 0x0034242C
		private void SelectLanguage(int index)
		{
			GameLocalizationConfig config = GameLocalization.Config;
			if (config == null || index < 0 || index >= config.languages.Length)
			{
				return;
			}
			GameLocalization.SetLocale(config.languages[index].code, true);
		}

		// Token: 0x06007FFE RID: 32766 RVA: 0x0034426C File Offset: 0x0034246C
		private void EnsureStreamerPixelSettings()
		{
			if (this._streamerPixelSize != null || this.gamePage == null)
			{
				return;
			}
			this._streamerPixelSize = MainMenuSettingsRuntimeFactory.BuildStreamerPixelSetting(this.gamePage.transform, LocalizedGameText.SourceFont(this.pageTitle), out this._streamerPixelValue);
			this._streamerPixelSize.onValueChanged.AddListener(delegate(float v)
			{
				this.Change(delegate
				{
					GameSettingsService.SetStreamerPixelSize(v);
				});
				MainMenuSettingsPanel.SetNumericLabel(this._streamerPixelValue, GameSettingsService.StreamerPixelSize.ToString());
			});
		}

		// Token: 0x06007FFF RID: 32767 RVA: 0x003442DC File Offset: 0x003424DC
		private void EnsureBodyPhysicsSettings()
		{
			if (this._bodyPhysics != null || this.gamePage == null)
			{
				return;
			}
			this._bodyPhysics = MainMenuSettingsRuntimeFactory.BuildBodyPhysicsSetting(this.gamePage.transform, LocalizedGameText.SourceFont(this.pageTitle));
			this._bodyPhysics.SetOptions(GameSettingsUiSchema.BodyPhysicsLabels, "settings.body_physics");
			MainMenuSettingsPanel.Bind(this._bodyPhysics, delegate(int i)
			{
				this.Change(delegate
				{
					GameSettingsService.SetBodyPhysics((GameSettingsService.BodyPhysicsStyle)Mathf.Clamp(i, 0, GameSettingsUiSchema.BodyPhysicsLabels.Length - 1));
				});
			});
		}

		// Token: 0x06008000 RID: 32768 RVA: 0x00344354 File Offset: 0x00342554
		private void EnsureGarbageMudSettings()
		{
			if (this._garbageMudBodyPaint != null || this.gamePage == null)
			{
				return;
			}
			this._garbageMudBodyPaint = MainMenuSettingsRuntimeFactory.BuildGarbageMudSetting(this.gamePage.transform, LocalizedGameText.SourceFont(this.pageTitle));
			this._garbageMudBodyPaint.onValueChanged.AddListener(delegate(bool v)
			{
				this.Change(delegate
				{
					GameSettingsService.SetGarbageMudBodyPaint(v);
				});
			});
		}

		// Token: 0x06008001 RID: 32769 RVA: 0x003443BC File Offset: 0x003425BC
		private void RefreshLanguageSettings()
		{
			this.EnsureLanguageSettings();
			if (this._languageSelector == null)
			{
				return;
			}
			GameLocalizationConfig config = GameLocalization.Config;
			if (config == null)
			{
				return;
			}
			for (int i = 0; i < config.languages.Length; i++)
			{
				if (config.languages[i].code == GameLocalization.RequestedCode)
				{
					this._languageSelector.SetIndexWithoutNotify(i);
				}
			}
			this._languageSelector.interactable = config.languages.Length != 0;
		}

		// Token: 0x06008002 RID: 32770 RVA: 0x0034443B File Offset: 0x0034263B
		public void OpenAudioSettings()
		{
			this.ShowPage(this.audioPage, true);
		}

		// Token: 0x06008003 RID: 32771 RVA: 0x0034444A File Offset: 0x0034264A
		public void OpenGameSettings()
		{
			this.ShowPage(this.gamePage, true);
		}

		// Token: 0x06008004 RID: 32772 RVA: 0x00344459 File Offset: 0x00342659
		public void OpenHudSettings()
		{
			this.ShowPage(this._hudPage, true);
		}

		// Token: 0x06008005 RID: 32773 RVA: 0x00344468 File Offset: 0x00342668
		private void EnsureHudSettings()
		{
			if (this._hudPage != null || this.graphicsPage == null)
			{
				return;
			}
			this._hudPage = MainMenuSettingsRuntimeFactory.BuildHudSettings(this, this.graphicsPage.transform, LocalizedGameText.SourceFont(this.pageTitle), out this._showFaceCamera, out this._showMinimap, out this._showDistanceAndScore);
			this._showFaceCamera.onValueChanged.AddListener(delegate(bool v)
			{
				this.Change(delegate
				{
					GameSettingsService.SetShowFaceCamera(v);
				});
			});
			this._showMinimap.onValueChanged.AddListener(delegate(bool v)
			{
				this.Change(delegate
				{
					GameSettingsService.SetShowMinimap(v);
				});
			});
			this._showDistanceAndScore.onValueChanged.AddListener(delegate(bool v)
			{
				this.Change(delegate
				{
					GameSettingsService.SetShowDistanceAndScore(v);
				});
			});
		}

		// Token: 0x06008006 RID: 32774 RVA: 0x0034451A File Offset: 0x0034271A
		public void OpenGraphics()
		{
			this.ShowPage(this.graphicsPage, true);
		}

		// Token: 0x06008007 RID: 32775 RVA: 0x00344529 File Offset: 0x00342729
		public void OpenAdvancedGraphics()
		{
			this.ShowPage(this.advancedGraphicsPage, true);
		}

		// Token: 0x06008008 RID: 32776 RVA: 0x00344538 File Offset: 0x00342738
		public void OpenMissions()
		{
			GameObject gameObject = this.missionsPage;
			if (gameObject != null)
			{
				PauseMissionTabs component = gameObject.GetComponent<PauseMissionTabs>();
				if (component != null)
				{
					component.ShowDefault();
				}
			}
			this.ShowPage(this.missionsPage, true);
		}

		// Token: 0x06008009 RID: 32777 RVA: 0x00344563 File Offset: 0x00342763
		public void OpenControls()
		{
			this.ShowPage(this.controlsPage, true);
		}

		// Token: 0x0600800A RID: 32778 RVA: 0x00344572 File Offset: 0x00342772
		public void OpenTutorials()
		{
			PauseMenuTutorialPanel pauseMenuTutorialPanel = this.tutorialPanel;
			if (pauseMenuTutorialPanel != null)
			{
				pauseMenuTutorialPanel.ShowLibrary();
			}
			this.ShowPage(this.tutorialsPage, true);
		}

		// Token: 0x0600800B RID: 32779 RVA: 0x00344592 File Offset: 0x00342792
		public bool OpenTutorial(string topicId, bool automatic)
		{
			if (this.tutorialPanel == null || !this.tutorialPanel.OpenTopic(topicId, automatic))
			{
				return false;
			}
			this.ShowPage(this.tutorialsPage, !MainMenuSettingsPanel.IsActive(this.tutorialsPage));
			return true;
		}

		// Token: 0x0600800C RID: 32780 RVA: 0x003445CE File Offset: 0x003427CE
		public void BackToRoot()
		{
			this.ShowPage(this.rootPage, true);
		}

		// Token: 0x0600800D RID: 32781 RVA: 0x0034451A File Offset: 0x0034271A
		public void BackToGraphics()
		{
			this.ShowPage(this.graphicsPage, true);
		}

		// Token: 0x0600800E RID: 32782 RVA: 0x003445E0 File Offset: 0x003427E0
		public bool TryHandleBack()
		{
			if (this._languageSelector != null && this._languageSelector.IsExpanded)
			{
				this._languageSelector.Hide();
				return true;
			}
			if (MainMenuSettingsPanel.IsActive(this._hudPage))
			{
				this.OpenGraphics();
				return true;
			}
			if (MainMenuSettingsPanel.IsActive(this._worldGraphicsPage))
			{
				this.OpenGraphics();
				return true;
			}
			if (MainMenuSettingsPanel.IsActive(this.tutorialsPage) && this.tutorialPanel != null && this.tutorialPanel.TryHandleBack())
			{
				return true;
			}
			if (MainMenuSettingsPanel.IsActive(this.advancedGraphicsPage))
			{
				this.BackToGraphics();
				return true;
			}
			if (!this.IsOnSubPage)
			{
				return false;
			}
			this.BackToRoot();
			return true;
		}

		// Token: 0x0600800F RID: 32783 RVA: 0x00344690 File Offset: 0x00342890
		private void ShowPage(GameObject page, bool animate)
		{
			this.PrepareControllerPage(page);
			MainMenuSettingsPanel.SetActive(this.rootPage, page == this.rootPage);
			MainMenuSettingsPanel.SetActive(this.audioPage, page == this.audioPage);
			MainMenuSettingsPanel.SetActive(this.gamePage, page == this.gamePage);
			MainMenuSettingsPanel.SetActive(this._hudPage, page == this._hudPage);
			MainMenuSettingsPanel.SetActive(this._worldGraphicsPage, page == this._worldGraphicsPage);
			MainMenuSettingsPanel.SetActive(this.graphicsPage, page == this.graphicsPage);
			MainMenuSettingsPanel.SetActive(this.advancedGraphicsPage, page == this.advancedGraphicsPage);
			MainMenuSettingsPanel.SetActive(this.missionsPage, page == this.missionsPage);
			MainMenuSettingsPanel.SetActive(this.controlsPage, page == this.controlsPage);
			MainMenuSettingsPanel.SetActive(this.tutorialsPage, page == this.tutorialsPage);
			MainMenuSettingsPanel.SetActive(this.progressionPanel, page != this.tutorialsPage);
			if (this.pageTitle != null)
			{
				if (page == this.audioPage)
				{
					this.pageTitle.text = "AUDIO";
				}
				else if (page == this.gamePage)
				{
					this.pageTitle.text = "GAME SETTINGS";
				}
				else if (page == this._hudPage)
				{
					this.pageTitle.text = "HUD DISPLAY";
				}
				else if (page == this._worldGraphicsPage)
				{
					this.pageTitle.text = "MAP & LIGHTING";
				}
				else if (page == this.graphicsPage || page == this.advancedGraphicsPage)
				{
					this.pageTitle.text = "GRAPHICS";
				}
				else if (page == this.missionsPage)
				{
					this.pageTitle.text = "MISSIONS";
				}
				else if (page == this.controlsPage)
				{
					this.pageTitle.text = "CONTROLS";
				}
				else if (page == this.tutorialsPage)
				{
					this.pageTitle.text = "TUTORIAL";
				}
				else
				{
					this.pageTitle.text = "PAUSE MENU";
				}
				LocalizedGameText.BindSource(this.pageTitle, "settings", this.pageTitle.text, null);
			}
			if (this.pageKicker != null)
			{
				if (page == this.audioPage)
				{
					this.pageKicker.text = "PAUSE MENU / AUDIO";
				}
				else if (page == this.gamePage)
				{
					this.pageKicker.text = "PAUSE MENU / GAME";
				}
				else if (page == this._hudPage)
				{
					this.pageKicker.text = "PAUSE MENU / GRAPHICS / HUD";
				}
				else if (page == this._worldGraphicsPage)
				{
					this.pageKicker.text = "PAUSE MENU / GRAPHICS / MAP";
				}
				else if (page == this.graphicsPage)
				{
					this.pageKicker.text = "PAUSE MENU / GRAPHICS";
				}
				else if (page == this.advancedGraphicsPage)
				{
					this.pageKicker.text = "PAUSE MENU / GRAPHICS / ADVANCED";
				}
				else if (page == this.missionsPage)
				{
					this.pageKicker.text = "PAUSE MENU";
				}
				else if (page == this.controlsPage)
				{
					this.pageKicker.text = "PAUSE MENU / CONTROLS";
				}
				else if (page == this.tutorialsPage)
				{
					this.pageKicker.text = "PAUSE MENU / HOW TO PLAY";
				}
				else
				{
					this.pageKicker.text = this._rootKicker;
				}
				LocalizedGameText.BindSource(this.pageKicker, "settings", this.pageKicker.text, null);
			}
			MainMenuSettingsPanel.SetActive(this._contextHint, page != this.missionsPage && page != this.controlsPage && page != this.tutorialsPage);
			if (animate && this._intro != null && page != null)
			{
				this._intro.PlayRows(page.transform as RectTransform);
			}
		}

		// Token: 0x06008010 RID: 32784 RVA: 0x00344AD0 File Offset: 0x00342CD0
		private void PrepareControllerPage(GameObject page)
		{
			if (page == null)
			{
				return;
			}
			Button button = null;
			foreach (Transform transform in page.GetComponentsInChildren<Transform>(true))
			{
				if (transform.name.StartsWith("Row - ", StringComparison.Ordinal))
				{
					if (transform.GetComponent<ControllerNavigationRow>() == null)
					{
						transform.gameObject.AddComponent<ControllerNavigationRow>();
					}
					if (transform.name.IndexOf("BACK", StringComparison.OrdinalIgnoreCase) >= 0 || ((page == this._hudPage || page == this._worldGraphicsPage) && transform.name.IndexOf("GRAPHICS", StringComparison.OrdinalIgnoreCase) >= 0))
					{
						button = transform.GetComponentInChildren<Button>(true);
					}
				}
			}
			if (page.GetComponent<ControllerNavigationScope>() != null)
			{
				return;
			}
			GameObject initial = ((button != null) ? button.gameObject : null);
			ControllerNavigationScope.Configure(page, delegate
			{
				if (!this.TryHandleBack() && initial != null)
				{
					ControllerUINavigation.Activate(initial, EventSystem.current);
				}
			}, initial, -100, true);
		}

		// Token: 0x06008011 RID: 32785 RVA: 0x00344BDC File Offset: 0x00342DDC
		public void RefreshFromCurrentSettings()
		{
			this._refreshing = true;
			GameSettingsService.ApplyAll();
			this.RefreshLanguageSettings();
			MainMenuSettingsPanel.SetSlider(this.masterSlider, GameSettingsService.MasterVolume);
			MainMenuSettingsPanel.SetSlider(this.heartbeatSlider, GameSettingsService.HeartbeatVolume);
			MainMenuSettingsPanel.SetSlider(this.cielVoiceSlider, GameSettingsService.CielVoiceVolume);
			MainMenuSettingsPanel.SetSlider(this.npcReactionSlider, GameSettingsService.NpcReactionVolume);
			MainMenuSettingsPanel.SetSlider(this.musicSlider, GameSettingsService.MusicVolume);
			MainMenuSettingsPanel.SetSlider(this.sfxSlider, GameSettingsService.SfxVolume);
			MainMenuSettingsPanel.SetToggle(this.muteToggle, GameSettingsService.Muted);
			MainMenuSettingsPanel.SetSlider(this.sensitivitySlider, GameSettingsService.LookSensitivity);
			MainMenuSettingsPanel.SetToggle(this._invertMouseX, GameSettingsService.InvertMouseX);
			MainMenuSettingsPanel.SetToggle(this._invertMouseY, GameSettingsService.InvertMouseY);
			MainMenuSettingsPanel.SetToggle(this.cielVoiceSubtitlesToggle, GameSettingsService.CielVoiceSubtitles);
			MainMenuSettingsPanel.SetToggle(this.cameraShakeToggle, GameSettingsService.CameraShake);
			MainMenuSettingsPanel.SetToggle(this._garbageMudBodyPaint, GameSettingsService.GarbageMudBodyPaint);
			MainMenuSettingsPanel.SetSelector(this._bodyPhysics, (int)GameSettingsService.BodyPhysics);
			MainMenuSettingsPanel.SetToggle(this.streamerModeToggle, GameSettingsService.StreamerMode);
			MainMenuSettingsPanel.SetSlider(this._streamerPixelSize, (float)GameSettingsService.StreamerPixelSize);
			if (this._streamerPixelSize != null)
			{
				this._streamerPixelSize.interactable = GameSettingsService.StreamerMode;
			}
			MainMenuSettingsPanel.SetNumericLabel(this._streamerPixelValue, GameSettingsService.StreamerPixelSize.ToString());
			MainMenuSettingsPanel.SetToggle(this.showFpsToggle, GameSettingsService.ShowFps);
			MainMenuSettingsPanel.SetToggle(this._showFaceCamera, GameSettingsService.ShowFaceCamera);
			MainMenuSettingsPanel.SetToggle(this._showMinimap, GameSettingsService.ShowMinimap);
			MainMenuSettingsPanel.SetToggle(this._showDistanceAndScore, GameSettingsService.ShowDistanceAndScore);
			MainMenuSettingsPanel.SetSelector(this.phoneVideoResolutionSelector, GameSettingsUiSchema.FindClosest(GameSettingsUiSchema.PhoneVideoHeights, GameSettingsService.PhoneVideoHeight));
			MainMenuSettingsPanel.SetSelector(this.phoneVideoFpsSelector, GameSettingsUiSchema.FindClosest(GameSettingsUiSchema.PhoneVideoFpsValues, GameSettingsService.PhoneVideoFps));
			GraphicsSettingsController instance = GraphicsSettingsController.Instance;
			GraphicsSettingsModel graphicsSettingsModel = ((instance != null) ? instance.Model : this.LoadStoredModel());
			MainMenuSettingsPanel.SetSlider(this._brightnessSlider, GraphicsSettingsModel.SanitizeBrightness(graphicsSettingsModel.brightness));
			MainMenuSettingsPanel.SetSelector(this.qualitySelector, (instance != null) ? instance.CurrentQualityIndex : this.MatchProfileIndex(graphicsSettingsModel));
			MainMenuSettingsPanel.SetToggle(this.vSyncToggle, graphicsSettingsModel.vSync);
			MainMenuSettingsPanel.SetSelector(this.windowModeSelector, MainMenuSettingsPanel.ModeToIndex((FullScreenMode)graphicsSettingsModel.fullScreenMode));
			MainMenuSettingsPanel.SetSelector(this.resolutionSelector, this.FindResolutionIndex((graphicsSettingsModel.resolutionWidth > 0) ? graphicsSettingsModel.resolutionWidth : Screen.width, (graphicsSettingsModel.resolutionHeight > 0) ? graphicsSettingsModel.resolutionHeight : Screen.height));
			MainMenuSettingsPanel.SetSelector(this.fpsSelector, MainMenuSettingsPanel.FindFpsIndex(graphicsSettingsModel.fpsCap));
			MainMenuSettingsPanel.SetSelector(this.renderScaleSelector, MainMenuSettingsPanel.FindClosest(GameSettingsUiSchema.RenderScales, graphicsSettingsModel.renderScale));
			MainMenuSettingsPanel.SetSelector(this.shadowQualitySelector, MainMenuSettingsPanel.ShadowQualityIndex(graphicsSettingsModel));
			MainMenuSettingsPanel.SetSelector(this.viewDistanceSelector, MainMenuSettingsPanel.FindClosest(GameSettingsUiSchema.ViewDistanceScales, graphicsSettingsModel.viewDistanceScale));
			MainMenuSettingsPanel.SetSelector(this.textureQualitySelector, Mathf.Clamp(graphicsSettingsModel.textureQuality, 0, 3));
			MainMenuSettingsPanel.SetToggle(this.ssaoToggle, graphicsSettingsModel.ssao);
			MainMenuSettingsPanel.SetToggle(this.postProcessingToggle, graphicsSettingsModel.postProcessing);
			MainMenuSettingsPanel.SetToggle(this.magicaClothToggle, graphicsSettingsModel.magicaClothEnabled);
			MainMenuSettingsPanel.SetSelector(this.colorVisionSelector, (int)graphicsSettingsModel.colorVision);
			this.RefreshWorldGraphics(graphicsSettingsModel);
			this.RefreshValueLabels();
			this._refreshing = false;
		}

		// Token: 0x06008012 RID: 32786 RVA: 0x00344F30 File Offset: 0x00343130
		private void RefreshValueLabels()
		{
			MainMenuSettingsPanel.SetVolumeLabel(this.masterValue, this.masterSlider);
			MainMenuSettingsPanel.SetVolumeLabel(this.heartbeatValue, this.heartbeatSlider);
			MainMenuSettingsPanel.SetVolumeLabel(this.cielVoiceValue, this.cielVoiceSlider);
			MainMenuSettingsPanel.SetVolumeLabel(this.npcReactionValue, this.npcReactionSlider);
			MainMenuSettingsPanel.SetVolumeLabel(this.musicValue, this.musicSlider);
			MainMenuSettingsPanel.SetVolumeLabel(this.sfxValue, this.sfxSlider);
			if (this.sensitivityValue != null && this.sensitivitySlider != null)
			{
				MainMenuSettingsPanel.SetNumericLabel(this.sensitivityValue, this.sensitivitySlider.value.ToString("0.0"));
			}
			if (this._brightnessValue != null && this._brightnessSlider != null)
			{
				MainMenuSettingsPanel.SetNumericLabel(this._brightnessValue, Mathf.RoundToInt(this._brightnessSlider.value * 100f).ToString() + "%");
			}
		}

		// Token: 0x06008013 RID: 32787 RVA: 0x00345034 File Offset: 0x00343234
		public void ApplyDisplay()
		{
			if (this._resolutions.Count == 0 || this.resolutionSelector == null)
			{
				return;
			}
			Vector2Int resolution = this._resolutions[Mathf.Clamp(this.resolutionSelector.Index, 0, this._resolutions.Count - 1)];
			FullScreenMode mode = MainMenuSettingsPanel.IndexToMode((this.windowModeSelector != null) ? this.windowModeSelector.Index : 1);
			GraphicsSettingsController instance = GraphicsSettingsController.Instance;
			if (instance != null)
			{
				instance.ApplyDisplayWithBackup(resolution.x, resolution.y, mode);
				instance.CommitDisplay();
				return;
			}
			Screen.SetResolution(resolution.x, resolution.y, mode);
			this.MutateStoredModel(delegate(GraphicsSettingsModel m)
			{
				m.resolutionWidth = resolution.x;
				m.resolutionHeight = resolution.y;
				m.fullScreenMode = (int)mode;
			});
		}

		// Token: 0x06008014 RID: 32788 RVA: 0x00345124 File Offset: 0x00343324
		public void ResetDefaults()
		{
			GameSettingsService.ResetAudioAndGameplay();
			if (GraphicsSettingsController.Instance != null)
			{
				GraphicsSettingsController.Instance.ResetToDefaults();
			}
			else
			{
				PlayerPrefs.DeleteKey("Graphics_ModelJson_v2");
				PlayerPrefs.Save();
			}
			this.RefreshFromCurrentSettings();
		}

		// Token: 0x06008015 RID: 32789 RVA: 0x00345159 File Offset: 0x00343359
		public void ResetMasterRow()
		{
			this.ResetAudio(delegate
			{
				GameSettingsService.SetMasterVolume(0.9f);
			});
		}

		// Token: 0x06008016 RID: 32790 RVA: 0x00345180 File Offset: 0x00343380
		public void ResetHeartbeatRow()
		{
			this.ResetAudio(delegate
			{
				GameSettingsService.SetHeartbeatVolume(0.9f);
			});
		}

		// Token: 0x06008017 RID: 32791 RVA: 0x003451A7 File Offset: 0x003433A7
		public void ResetCielVoiceRow()
		{
			this.ResetAudio(delegate
			{
				GameSettingsService.SetCielVoiceVolume(1f);
			});
		}

		// Token: 0x06008018 RID: 32792 RVA: 0x003451CE File Offset: 0x003433CE
		public void ResetNpcReactionRow()
		{
			this.ResetAudio(delegate
			{
				GameSettingsService.SetNpcReactionVolume(1f);
			});
		}

		// Token: 0x06008019 RID: 32793 RVA: 0x003451F5 File Offset: 0x003433F5
		public void ResetMusicRow()
		{
			this.ResetAudio(delegate
			{
				GameSettingsService.SetMusicVolume(0.72f);
			});
		}

		// Token: 0x0600801A RID: 32794 RVA: 0x0034521C File Offset: 0x0034341C
		public void ResetSfxRow()
		{
			this.ResetAudio(delegate
			{
				GameSettingsService.SetSfxVolume(0.82f);
			});
		}

		// Token: 0x0600801B RID: 32795 RVA: 0x00345243 File Offset: 0x00343443
		public void ResetSensitivityRow()
		{
			GameSettingsService.SetLookSensitivity(1f);
			this.RefreshFromCurrentSettings();
		}

		// Token: 0x0600801C RID: 32796 RVA: 0x00345255 File Offset: 0x00343455
		private void ResetAudio(Action reset)
		{
			if (reset != null)
			{
				reset();
			}
			this.RefreshFromCurrentSettings();
		}

		// Token: 0x0600801D RID: 32797 RVA: 0x00345268 File Offset: 0x00343468
		private void BindEvents()
		{
			this.Bind(this.masterSlider, delegate(float v)
			{
				GameSettingsService.SetMasterVolume(v);
			});
			this.Bind(this.heartbeatSlider, delegate(float v)
			{
				GameSettingsService.SetHeartbeatVolume(v);
			});
			this.Bind(this.cielVoiceSlider, delegate(float v)
			{
				GameSettingsService.SetCielVoiceVolume(v);
			});
			this.Bind(this.npcReactionSlider, delegate(float v)
			{
				GameSettingsService.SetNpcReactionVolume(v);
			});
			this.Bind(this.musicSlider, delegate(float v)
			{
				GameSettingsService.SetMusicVolume(v);
			});
			this.Bind(this.sfxSlider, delegate(float v)
			{
				GameSettingsService.SetSfxVolume(v);
			});
			this.Bind(this.sensitivitySlider, delegate(float v)
			{
				GameSettingsService.SetLookSensitivity(v);
			});
			if (this.muteToggle != null)
			{
				this.muteToggle.onValueChanged.AddListener(delegate(bool v)
				{
					this.Change(delegate
					{
						GameSettingsService.SetMuted(v);
					});
				});
			}
			if (this.cielVoiceSubtitlesToggle != null)
			{
				this.cielVoiceSubtitlesToggle.onValueChanged.AddListener(delegate(bool v)
				{
					this.Change(delegate
					{
						GameSettingsService.SetCielVoiceSubtitles(v);
					});
				});
			}
			if (this.cameraShakeToggle != null)
			{
				this.cameraShakeToggle.onValueChanged.AddListener(delegate(bool v)
				{
					this.Change(delegate
					{
						GameSettingsService.SetCameraShake(v);
					});
				});
			}
			if (this.streamerModeToggle != null)
			{
				this.streamerModeToggle.onValueChanged.AddListener(delegate(bool v)
				{
					this.Change(delegate
					{
						GameSettingsService.SetStreamerMode(v);
					});
					if (this._streamerPixelSize != null)
					{
						this._streamerPixelSize.interactable = v;
					}
				});
			}
			if (this.showFpsToggle != null)
			{
				this.showFpsToggle.onValueChanged.AddListener(delegate(bool v)
				{
					this.Change(delegate
					{
						GameSettingsService.SetShowFps(v);
					});
				});
			}
			MainMenuSettingsPanel.Bind(this.phoneVideoResolutionSelector, delegate(int i)
			{
				this.Change(delegate
				{
					GameSettingsService.SetPhoneVideoHeight(GameSettingsUiSchema.PhoneVideoHeights[Mathf.Clamp(i, 0, GameSettingsUiSchema.PhoneVideoHeights.Length - 1)]);
				});
			});
			MainMenuSettingsPanel.Bind(this.phoneVideoFpsSelector, delegate(int i)
			{
				this.Change(delegate
				{
					GameSettingsService.SetPhoneVideoFps(GameSettingsUiSchema.PhoneVideoFpsValues[Mathf.Clamp(i, 0, GameSettingsUiSchema.PhoneVideoFpsValues.Length - 1)]);
				});
			});
			if (this.vSyncToggle != null)
			{
				this.vSyncToggle.onValueChanged.AddListener(delegate(bool v)
				{
					this.ChangeGraphics(delegate(GraphicsSettingsModel m)
					{
						m.SetVerticalSync(v);
					});
				});
			}
			if (this.ssaoToggle != null)
			{
				this.ssaoToggle.onValueChanged.AddListener(delegate(bool v)
				{
					this.ChangeGraphics(delegate(GraphicsSettingsModel m)
					{
						m.ssao = v;
					});
				});
			}
			if (this.postProcessingToggle != null)
			{
				this.postProcessingToggle.onValueChanged.AddListener(delegate(bool v)
				{
					this.ChangeGraphics(delegate(GraphicsSettingsModel m)
					{
						m.postProcessing = v;
					});
				});
			}
			if (this.magicaClothToggle != null)
			{
				this.magicaClothToggle.onValueChanged.AddListener(delegate(bool v)
				{
					this.ChangeGraphics(delegate(GraphicsSettingsModel m)
					{
						m.magicaClothEnabled = v;
					});
				});
			}
			MainMenuSettingsPanel.Bind(this.qualitySelector, new Action<int>(this.OnQualityChanged));
			MainMenuSettingsPanel.Bind(this.fpsSelector, delegate(int i)
			{
				this.ChangeGraphics(delegate(GraphicsSettingsModel m)
				{
					m.SetFrameRateLimit(GameSettingsUiSchema.FpsValues[Mathf.Clamp(i, 0, GameSettingsUiSchema.FpsValues.Length - 1)]);
				});
			});
			MainMenuSettingsPanel.Bind(this.renderScaleSelector, delegate(int i)
			{
				this.ChangeGraphics(delegate(GraphicsSettingsModel m)
				{
					m.renderScale = GameSettingsUiSchema.RenderScales[Mathf.Clamp(i, 0, GameSettingsUiSchema.RenderScales.Length - 1)];
				});
			});
			MainMenuSettingsPanel.Bind(this.viewDistanceSelector, delegate(int i)
			{
				this.ChangeGraphics(delegate(GraphicsSettingsModel m)
				{
					m.viewDistanceScale = GameSettingsUiSchema.ViewDistanceScales[Mathf.Clamp(i, 0, GameSettingsUiSchema.ViewDistanceScales.Length - 1)];
				});
			});
			MainMenuSettingsPanel.Bind(this.textureQualitySelector, delegate(int i)
			{
				this.ChangeGraphics(delegate(GraphicsSettingsModel m)
				{
					m.textureQuality = Mathf.Clamp(i, 0, 3);
				});
			});
			MainMenuSettingsPanel.Bind(this.colorVisionSelector, delegate(int i)
			{
				this.ChangeGraphics(delegate(GraphicsSettingsModel m)
				{
					m.colorVision = (GraphicsSettingsModel.ColorVisionMode)Mathf.Clamp(i, 0, 3);
				});
			});
			MainMenuSettingsPanel.Bind(this.shadowQualitySelector, new Action<int>(this.OnShadowQualityChanged));
		}

		// Token: 0x0600801E RID: 32798 RVA: 0x003455EC File Offset: 0x003437EC
		private void Bind(Slider slider, Action<float> setter)
		{
			if (slider == null)
			{
				return;
			}
			slider.onValueChanged.AddListener(delegate(float value)
			{
				if (!this._refreshing)
				{
					setter(value);
				}
				this.RefreshValueLabels();
			});
		}

		// Token: 0x0600801F RID: 32799 RVA: 0x0034562E File Offset: 0x0034382E
		private static void Bind(MainMenuArrowSelector selector, Action<int> setter)
		{
			if (selector != null)
			{
				selector.OnUserChangedIndex += setter;
			}
		}

		// Token: 0x06008020 RID: 32800 RVA: 0x00345640 File Offset: 0x00343840
		private void Change(Action action)
		{
			if (!this._refreshing)
			{
				action();
			}
		}

		// Token: 0x06008021 RID: 32801 RVA: 0x00345650 File Offset: 0x00343850
		private void ChangeGraphics(Action<GraphicsSettingsModel> mutate)
		{
			if (this._refreshing || mutate == null)
			{
				return;
			}
			GraphicsSettingsController instance = GraphicsSettingsController.Instance;
			if (instance != null)
			{
				instance.UpdateModel(mutate, false);
				MainMenuSettingsPanel.SetSelector(this.qualitySelector, instance.CurrentQualityIndex);
			}
			else
			{
				this.MutateStoredModel(mutate);
				MainMenuSettingsPanel.SetSelector(this.qualitySelector, this.MatchProfileIndex(this.LoadStoredModel()));
			}
			GraphicsSettingsModel graphicsSettingsModel = ((instance != null) ? instance.Model : this.LoadStoredModel());
			MainMenuSettingsPanel.SetToggle(this.vSyncToggle, graphicsSettingsModel.vSync);
			MainMenuSettingsPanel.SetSelector(this.fpsSelector, MainMenuSettingsPanel.FindFpsIndex(graphicsSettingsModel.fpsCap));
			this.RefreshWorldGraphics(graphicsSettingsModel);
		}

		// Token: 0x06008022 RID: 32802 RVA: 0x003456F8 File Offset: 0x003438F8
		private void OnQualityChanged(int index)
		{
			if (this._refreshing)
			{
				return;
			}
			if (index < 0 || index > 3)
			{
				this.RefreshFromCurrentSettings();
				return;
			}
			GraphicsSettingsController instance = GraphicsSettingsController.Instance;
			if (instance != null)
			{
				instance.SetQualityLevel((GraphicsSettingsSO.QualityLevel)Mathf.Clamp(index, 0, 3));
			}
			else
			{
				GraphicsSettingsSO profile = this.FindProfile(index);
				if (profile != null)
				{
					this.MutateStoredModel(delegate(GraphicsSettingsModel m)
					{
						m.CopyQualityFrom(profile.model);
					});
				}
			}
			this.RefreshFromCurrentSettings();
		}

		// Token: 0x06008023 RID: 32803 RVA: 0x00345778 File Offset: 0x00343978
		private void OnShadowQualityChanged(int index)
		{
			this.ChangeGraphics(delegate(GraphicsSettingsModel m)
			{
				GameSettingsUiSchema.ApplyShadowQuality(m, index);
			});
		}

		// Token: 0x06008024 RID: 32804 RVA: 0x003457A4 File Offset: 0x003439A4
		private void BuildStaticOptions()
		{
			MainMenuArrowSelector mainMenuArrowSelector = this.qualitySelector;
			if (mainMenuArrowSelector != null)
			{
				mainMenuArrowSelector.SetOptions(GameSettingsUiSchema.QualityLabels, "settings");
			}
			MainMenuArrowSelector mainMenuArrowSelector2 = this.windowModeSelector;
			if (mainMenuArrowSelector2 != null)
			{
				mainMenuArrowSelector2.SetOptions(GameSettingsUiSchema.WindowModeLabels, "settings");
			}
			MainMenuArrowSelector mainMenuArrowSelector3 = this.fpsSelector;
			if (mainMenuArrowSelector3 != null)
			{
				mainMenuArrowSelector3.SetOptions(GameSettingsUiSchema.FpsLabels, "settings");
			}
			MainMenuArrowSelector mainMenuArrowSelector4 = this.renderScaleSelector;
			if (mainMenuArrowSelector4 != null)
			{
				mainMenuArrowSelector4.SetOptions(GameSettingsUiSchema.RenderScaleLabels, "settings");
			}
			MainMenuArrowSelector mainMenuArrowSelector5 = this.shadowQualitySelector;
			if (mainMenuArrowSelector5 != null)
			{
				mainMenuArrowSelector5.SetOptions(GameSettingsUiSchema.ShadowQualityLabels, "settings");
			}
			MainMenuArrowSelector mainMenuArrowSelector6 = this.viewDistanceSelector;
			if (mainMenuArrowSelector6 != null)
			{
				mainMenuArrowSelector6.SetOptions(GameSettingsUiSchema.ViewDistanceLabels, "settings.distance");
			}
			MainMenuArrowSelector mainMenuArrowSelector7 = this.textureQualitySelector;
			if (mainMenuArrowSelector7 != null)
			{
				mainMenuArrowSelector7.SetOptions(GameSettingsUiSchema.TextureQualityLabels, "settings");
			}
			MainMenuArrowSelector mainMenuArrowSelector8 = this.colorVisionSelector;
			if (mainMenuArrowSelector8 != null)
			{
				mainMenuArrowSelector8.SetOptions(GameSettingsUiSchema.ColorVisionLabels, "settings");
			}
			MainMenuArrowSelector mainMenuArrowSelector9 = this.phoneVideoResolutionSelector;
			if (mainMenuArrowSelector9 != null)
			{
				mainMenuArrowSelector9.SetOptions(GameSettingsUiSchema.PhoneVideoResolutionLabels, "settings");
			}
			MainMenuArrowSelector mainMenuArrowSelector10 = this.phoneVideoFpsSelector;
			if (mainMenuArrowSelector10 != null)
			{
				mainMenuArrowSelector10.SetOptions(GameSettingsUiSchema.PhoneVideoFpsLabels, "settings");
			}
			List<string> list = new List<string>();
			GameSettingsUiSchema.BuildResolutionOptions(this._resolutions, list);
			MainMenuArrowSelector mainMenuArrowSelector11 = this.resolutionSelector;
			if (mainMenuArrowSelector11 == null)
			{
				return;
			}
			mainMenuArrowSelector11.SetOptions(list, "settings");
		}

		// Token: 0x06008025 RID: 32805 RVA: 0x003458E7 File Offset: 0x00343AE7
		private GraphicsSettingsModel LoadStoredModel()
		{
			return GraphicsSettingsModel.FromJson(PlayerPrefs.GetString("Graphics_ModelJson_v2", null)) ?? new GraphicsSettingsModel();
		}

		// Token: 0x06008026 RID: 32806 RVA: 0x00345904 File Offset: 0x00343B04
		private void MutateStoredModel(Action<GraphicsSettingsModel> mutate)
		{
			GraphicsSettingsModel graphicsSettingsModel = this.LoadStoredModel();
			mutate(graphicsSettingsModel);
			GraphicsSettingsApplier.ApplyFramePacing(graphicsSettingsModel, GraphicsSettingsController.IsStartupPresentationActive);
			PlayerPrefs.SetString("Graphics_ModelJson_v2", graphicsSettingsModel.ToJson());
			PlayerPrefs.Save();
		}

		// Token: 0x06008027 RID: 32807 RVA: 0x00345940 File Offset: 0x00343B40
		private GraphicsSettingsSO FindProfile(int index)
		{
			if (this.qualityProfiles == null)
			{
				return null;
			}
			foreach (GraphicsSettingsSO graphicsSettingsSO in this.qualityProfiles)
			{
				if (graphicsSettingsSO != null && graphicsSettingsSO.qualityLevel == (GraphicsSettingsSO.QualityLevel)index)
				{
					return graphicsSettingsSO;
				}
			}
			return null;
		}

		// Token: 0x06008028 RID: 32808 RVA: 0x00345988 File Offset: 0x00343B88
		private int MatchProfileIndex(GraphicsSettingsModel model)
		{
			if (this.qualityProfiles != null)
			{
				foreach (GraphicsSettingsSO graphicsSettingsSO in this.qualityProfiles)
				{
					if (graphicsSettingsSO != null && model.QualityEquals(graphicsSettingsSO.model))
					{
						return (int)graphicsSettingsSO.qualityLevel;
					}
				}
			}
			return 4;
		}

		// Token: 0x06008029 RID: 32809 RVA: 0x003459D5 File Offset: 0x00343BD5
		private int FindResolutionIndex(int width, int height)
		{
			return GameSettingsUiSchema.FindResolutionIndex(this._resolutions, width, height);
		}

		// Token: 0x0600802A RID: 32810 RVA: 0x003459E4 File Offset: 0x00343BE4
		private static int FindFpsIndex(int fps)
		{
			return GameSettingsUiSchema.FindFpsIndex(fps);
		}

		// Token: 0x0600802B RID: 32811 RVA: 0x003459EC File Offset: 0x00343BEC
		private static int FindClosest(float[] values, float target)
		{
			return GameSettingsUiSchema.FindClosest(values, target);
		}

		// Token: 0x0600802C RID: 32812 RVA: 0x003459F5 File Offset: 0x00343BF5
		private static int ShadowQualityIndex(GraphicsSettingsModel model)
		{
			return GameSettingsUiSchema.ShadowQualityIndex(model);
		}

		// Token: 0x0600802D RID: 32813 RVA: 0x003459FD File Offset: 0x00343BFD
		private static int ModeToIndex(FullScreenMode mode)
		{
			return GameSettingsUiSchema.ModeToIndex(mode);
		}

		// Token: 0x0600802E RID: 32814 RVA: 0x00345A05 File Offset: 0x00343C05
		private static FullScreenMode IndexToMode(int index)
		{
			return GameSettingsUiSchema.IndexToMode(index);
		}

		// Token: 0x0600802F RID: 32815 RVA: 0x00345A10 File Offset: 0x00343C10
		private static void SetVolumeLabel(TMP_Text label, Slider slider)
		{
			if (label != null && slider != null)
			{
				MainMenuSettingsPanel.SetNumericLabel(label, Mathf.RoundToInt(slider.value * 100f).ToString());
			}
		}

		// Token: 0x06008030 RID: 32816 RVA: 0x00345A4E File Offset: 0x00343C4E
		private static void SetNumericLabel(TMP_Text label, string value)
		{
			if (label == null)
			{
				return;
			}
			label.text = value;
			LocalizedGameText.BindFontOnly(label);
		}

		// Token: 0x06008031 RID: 32817 RVA: 0x003410B1 File Offset: 0x0033F2B1
		private static bool IsActive(GameObject target)
		{
			return target != null && target.activeSelf;
		}

		// Token: 0x06008032 RID: 32818 RVA: 0x003410C4 File Offset: 0x0033F2C4
		private static void SetActive(GameObject target, bool value)
		{
			if (target != null && target.activeSelf != value)
			{
				target.SetActive(value);
			}
		}

		// Token: 0x06008033 RID: 32819 RVA: 0x00345A68 File Offset: 0x00343C68
		private static void SetSlider(Slider slider, float value)
		{
			if (slider != null)
			{
				slider.SetValueWithoutNotify(value);
			}
		}

		// Token: 0x06008034 RID: 32820 RVA: 0x00345A7C File Offset: 0x00343C7C
		private static void SetToggle(Toggle toggle, bool value)
		{
			if (toggle == null)
			{
				return;
			}
			toggle.SetIsOnWithoutNotify(value);
			MainMenuSwitchVisual component = toggle.GetComponent<MainMenuSwitchVisual>();
			if (component != null)
			{
				component.Snap(value);
			}
		}

		// Token: 0x06008035 RID: 32821 RVA: 0x00345AB1 File Offset: 0x00343CB1
		private static void SetSelector(MainMenuArrowSelector selector, int value)
		{
			if (selector != null)
			{
				selector.SetIndexWithoutNotify(value);
			}
		}

		// Token: 0x06008036 RID: 32822 RVA: 0x00345AC3 File Offset: 0x00343CC3
		public void OpenWorldGraphics()
		{
			this.ShowPage(this._worldGraphicsPage, true);
		}

		// Token: 0x06008037 RID: 32823 RVA: 0x00345AD4 File Offset: 0x00343CD4
		private void EnsureWorldGraphicsSettings()
		{
			if (this._worldGraphicsPage != null || this.graphicsPage == null)
			{
				return;
			}
			this._worldGraphicsPage = MainMenuSettingsRuntimeFactory.BuildWorldGraphics(this, this.graphicsPage.transform, LocalizedGameText.SourceFont(this.pageTitle), out this._mapLightingSelector, out this._worldRestartStatus, out this._graphicsRestartStatus);
			this._mapLightingSelector.SetOptions(GameSettingsUiSchema.MapLightingLabels, "settings");
			MainMenuSettingsPanel.Bind(this._mapLightingSelector, delegate(int i)
			{
				this.ChangeGraphics(delegate(GraphicsSettingsModel m)
				{
					m.mapLighting = (GraphicsSettingsModel.MapLightingQuality)Mathf.Clamp(i, 0, 2);
				});
			});
		}

		// Token: 0x06008038 RID: 32824 RVA: 0x00345B60 File Offset: 0x00343D60
		private void RefreshWorldGraphics(GraphicsSettingsModel model)
		{
			MainMenuSettingsPanel.SetSelector(this._mapLightingSelector, (int)model.mapLighting);
			ChunkManager instance = ChunkManager.Instance;
			string pending = ((instance != null && instance.SpawnedChunkCount > 0) ? MapGraphicsSelection.PendingChanges(model, instance.ActiveMapCatalogResource) : "");
			if (this._worldRestartStatus != null)
			{
				LocalizedGameText.BindComputed(this._worldRestartStatus, () => MainMenuSettingsPanel.WorldGraphicsNotice(pending, false)).FitTranslationToRectangle(false);
			}
			if (this._graphicsRestartStatus != null)
			{
				LocalizedGameText.BindComputed(this._graphicsRestartStatus, () => MainMenuSettingsPanel.WorldGraphicsNotice(pending, true)).FitTranslationToRectangle(false);
			}
		}

		// Token: 0x06008039 RID: 32825 RVA: 0x00345C0C File Offset: 0x00343E0C
		private static string WorldGraphicsNotice(string pending, bool includeInstructions)
		{
			string text = (string.IsNullOrEmpty(pending) ? GameLocalization.GetSource("settings", "Map lighting applies to a new run. Quality presets can also change this setting.") : GameLocalization.FormatSource("settings", "NEW RUN REQUIRED\n{0} changed. The current map keeps its previous settings.", new object[] { GameLocalization.GetSource("settings", pending) }));
			if (!includeInstructions)
			{
				return text;
			}
			return GameLocalization.FormatSource("settings", "{0}\n\nReturn to the main menu and start a new run to apply it. See MAP & LIGHTING for details.", new object[] { text });
		}

		// Token: 0x0400834F RID: 33615
		[Header("Pages")]
		[SerializeField]
		private GameObject rootPage;

		// Token: 0x04008350 RID: 33616
		[SerializeField]
		private GameObject audioPage;

		// Token: 0x04008351 RID: 33617
		[SerializeField]
		private GameObject gamePage;

		// Token: 0x04008352 RID: 33618
		[SerializeField]
		private GameObject graphicsPage;

		// Token: 0x04008353 RID: 33619
		[SerializeField]
		private GameObject advancedGraphicsPage;

		// Token: 0x04008354 RID: 33620
		[SerializeField]
		private GameObject missionsPage;

		// Token: 0x04008355 RID: 33621
		[SerializeField]
		private GameObject controlsPage;

		// Token: 0x04008356 RID: 33622
		[SerializeField]
		private GameObject tutorialsPage;

		// Token: 0x04008357 RID: 33623
		[SerializeField]
		private PauseMenuTutorialPanel tutorialPanel;

		// Token: 0x04008358 RID: 33624
		[SerializeField]
		private GameObject progressionPanel;

		// Token: 0x04008359 RID: 33625
		[SerializeField]
		private TMP_Text pageTitle;

		// Token: 0x0400835A RID: 33626
		[SerializeField]
		private TMP_Text pageKicker;

		// Token: 0x0400835B RID: 33627
		private string _rootKicker = "MAIN MENU";

		// Token: 0x0400835C RID: 33628
		private GameObject _contextHint;

		// Token: 0x0400835D RID: 33629
		private GameObject _hudPage;

		// Token: 0x0400835E RID: 33630
		private Toggle _showFaceCamera;

		// Token: 0x0400835F RID: 33631
		private Toggle _showMinimap;

		// Token: 0x04008360 RID: 33632
		private Toggle _showDistanceAndScore;

		// Token: 0x04008361 RID: 33633
		private Toggle _garbageMudBodyPaint;

		// Token: 0x04008362 RID: 33634
		private Toggle _invertMouseX;

		// Token: 0x04008363 RID: 33635
		private Toggle _invertMouseY;

		// Token: 0x04008364 RID: 33636
		private Slider _brightnessSlider;

		// Token: 0x04008365 RID: 33637
		private TMP_Text _brightnessValue;

		// Token: 0x04008366 RID: 33638
		[Header("Audio")]
		[SerializeField]
		private Slider masterSlider;

		// Token: 0x04008367 RID: 33639
		[SerializeField]
		private TMP_Text masterValue;

		// Token: 0x04008368 RID: 33640
		[SerializeField]
		private Slider heartbeatSlider;

		// Token: 0x04008369 RID: 33641
		[SerializeField]
		private TMP_Text heartbeatValue;

		// Token: 0x0400836A RID: 33642
		[SerializeField]
		private Slider cielVoiceSlider;

		// Token: 0x0400836B RID: 33643
		[SerializeField]
		private TMP_Text cielVoiceValue;

		// Token: 0x0400836C RID: 33644
		[SerializeField]
		private Slider npcReactionSlider;

		// Token: 0x0400836D RID: 33645
		[SerializeField]
		private TMP_Text npcReactionValue;

		// Token: 0x0400836E RID: 33646
		[SerializeField]
		private Slider musicSlider;

		// Token: 0x0400836F RID: 33647
		[SerializeField]
		private TMP_Text musicValue;

		// Token: 0x04008370 RID: 33648
		[SerializeField]
		private Slider sfxSlider;

		// Token: 0x04008371 RID: 33649
		[SerializeField]
		private TMP_Text sfxValue;

		// Token: 0x04008372 RID: 33650
		[SerializeField]
		private Toggle muteToggle;

		// Token: 0x04008373 RID: 33651
		[Header("Game")]
		[SerializeField]
		private Slider sensitivitySlider;

		// Token: 0x04008374 RID: 33652
		[SerializeField]
		private TMP_Text sensitivityValue;

		// Token: 0x04008375 RID: 33653
		[SerializeField]
		private Toggle cielVoiceSubtitlesToggle;

		// Token: 0x04008376 RID: 33654
		[SerializeField]
		private Toggle cameraShakeToggle;

		// Token: 0x04008377 RID: 33655
		[SerializeField]
		private Toggle streamerModeToggle;

		// Token: 0x04008378 RID: 33656
		[SerializeField]
		private Toggle showFpsToggle;

		// Token: 0x04008379 RID: 33657
		[SerializeField]
		private MainMenuArrowSelector phoneVideoResolutionSelector;

		// Token: 0x0400837A RID: 33658
		[SerializeField]
		private MainMenuArrowSelector phoneVideoFpsSelector;

		// Token: 0x0400837B RID: 33659
		[Header("Graphics")]
		[SerializeField]
		private MainMenuArrowSelector qualitySelector;

		// Token: 0x0400837C RID: 33660
		[SerializeField]
		private MainMenuArrowSelector resolutionSelector;

		// Token: 0x0400837D RID: 33661
		[SerializeField]
		private MainMenuArrowSelector windowModeSelector;

		// Token: 0x0400837E RID: 33662
		[SerializeField]
		private MainMenuArrowSelector fpsSelector;

		// Token: 0x0400837F RID: 33663
		[SerializeField]
		private Toggle vSyncToggle;

		// Token: 0x04008380 RID: 33664
		[Header("Advanced graphics")]
		[SerializeField]
		private MainMenuArrowSelector renderScaleSelector;

		// Token: 0x04008381 RID: 33665
		[SerializeField]
		private MainMenuArrowSelector shadowQualitySelector;

		// Token: 0x04008382 RID: 33666
		[SerializeField]
		private MainMenuArrowSelector viewDistanceSelector;

		// Token: 0x04008383 RID: 33667
		[SerializeField]
		private MainMenuArrowSelector textureQualitySelector;

		// Token: 0x04008384 RID: 33668
		[SerializeField]
		private Toggle ssaoToggle;

		// Token: 0x04008385 RID: 33669
		[SerializeField]
		private Toggle postProcessingToggle;

		// Token: 0x04008386 RID: 33670
		[SerializeField]
		private Toggle magicaClothToggle;

		// Token: 0x04008387 RID: 33671
		[SerializeField]
		private MainMenuArrowSelector colorVisionSelector;

		// Token: 0x04008388 RID: 33672
		[SerializeField]
		private GraphicsSettingsSO[] qualityProfiles;

		// Token: 0x04008389 RID: 33673
		private const string GraphicsModelPrefsKey = "Graphics_ModelJson_v2";

		// Token: 0x0400838A RID: 33674
		private readonly List<Vector2Int> _resolutions = new List<Vector2Int>();

		// Token: 0x0400838B RID: 33675
		private bool _refreshing;

		// Token: 0x0400838C RID: 33676
		private MainMenuPanelIntroAnimator _intro;

		// Token: 0x0400838D RID: 33677
		private LanguageDropdown _languageSelector;

		// Token: 0x0400838E RID: 33678
		private Button _unstuckButton;

		// Token: 0x0400838F RID: 33679
		private Slider _streamerPixelSize;

		// Token: 0x04008390 RID: 33680
		private TMP_Text _streamerPixelValue;

		// Token: 0x04008391 RID: 33681
		private MainMenuArrowSelector _bodyPhysics;

		// Token: 0x04008392 RID: 33682
		private GameObject _worldGraphicsPage;

		// Token: 0x04008393 RID: 33683
		private MainMenuArrowSelector _mapLightingSelector;

		// Token: 0x04008394 RID: 33684
		private TMP_Text _worldRestartStatus;

		// Token: 0x04008395 RID: 33685
		private TMP_Text _graphicsRestartStatus;

		// Token: 0x02001076 RID: 4214
		public sealed class RuntimeBindings
		{
			// Token: 0x04008396 RID: 33686
			public GameObject rootPage;

			// Token: 0x04008397 RID: 33687
			public GameObject audioPage;

			// Token: 0x04008398 RID: 33688
			public GameObject gamePage;

			// Token: 0x04008399 RID: 33689
			public GameObject graphicsPage;

			// Token: 0x0400839A RID: 33690
			public GameObject advancedGraphicsPage;

			// Token: 0x0400839B RID: 33691
			public GameObject missionsPage;

			// Token: 0x0400839C RID: 33692
			public GameObject controlsPage;

			// Token: 0x0400839D RID: 33693
			public GameObject tutorialsPage;

			// Token: 0x0400839E RID: 33694
			public PauseMenuTutorialPanel tutorialPanel;

			// Token: 0x0400839F RID: 33695
			public GameObject progressionPanel;

			// Token: 0x040083A0 RID: 33696
			public Button unstuckButton;

			// Token: 0x040083A1 RID: 33697
			public TMP_Text pageTitle;

			// Token: 0x040083A2 RID: 33698
			public TMP_Text pageKicker;

			// Token: 0x040083A3 RID: 33699
			public string rootKicker;

			// Token: 0x040083A4 RID: 33700
			public GameObject contextHint;

			// Token: 0x040083A5 RID: 33701
			public Slider masterSlider;

			// Token: 0x040083A6 RID: 33702
			public TMP_Text masterValue;

			// Token: 0x040083A7 RID: 33703
			public Slider heartbeatSlider;

			// Token: 0x040083A8 RID: 33704
			public TMP_Text heartbeatValue;

			// Token: 0x040083A9 RID: 33705
			public Slider cielVoiceSlider;

			// Token: 0x040083AA RID: 33706
			public TMP_Text cielVoiceValue;

			// Token: 0x040083AB RID: 33707
			public Slider npcReactionSlider;

			// Token: 0x040083AC RID: 33708
			public TMP_Text npcReactionValue;

			// Token: 0x040083AD RID: 33709
			public Slider musicSlider;

			// Token: 0x040083AE RID: 33710
			public TMP_Text musicValue;

			// Token: 0x040083AF RID: 33711
			public Slider sfxSlider;

			// Token: 0x040083B0 RID: 33712
			public TMP_Text sfxValue;

			// Token: 0x040083B1 RID: 33713
			public Toggle muteToggle;

			// Token: 0x040083B2 RID: 33714
			public Slider sensitivitySlider;

			// Token: 0x040083B3 RID: 33715
			public TMP_Text sensitivityValue;

			// Token: 0x040083B4 RID: 33716
			public Toggle cielVoiceSubtitlesToggle;

			// Token: 0x040083B5 RID: 33717
			public Toggle cameraShakeToggle;

			// Token: 0x040083B6 RID: 33718
			public Toggle streamerModeToggle;

			// Token: 0x040083B7 RID: 33719
			public Toggle showFpsToggle;

			// Token: 0x040083B8 RID: 33720
			public MainMenuArrowSelector phoneVideoResolutionSelector;

			// Token: 0x040083B9 RID: 33721
			public MainMenuArrowSelector phoneVideoFpsSelector;

			// Token: 0x040083BA RID: 33722
			public MainMenuArrowSelector qualitySelector;

			// Token: 0x040083BB RID: 33723
			public MainMenuArrowSelector resolutionSelector;

			// Token: 0x040083BC RID: 33724
			public MainMenuArrowSelector windowModeSelector;

			// Token: 0x040083BD RID: 33725
			public MainMenuArrowSelector fpsSelector;

			// Token: 0x040083BE RID: 33726
			public Toggle vSyncToggle;

			// Token: 0x040083BF RID: 33727
			public MainMenuArrowSelector renderScaleSelector;

			// Token: 0x040083C0 RID: 33728
			public MainMenuArrowSelector shadowQualitySelector;

			// Token: 0x040083C1 RID: 33729
			public MainMenuArrowSelector viewDistanceSelector;

			// Token: 0x040083C2 RID: 33730
			public MainMenuArrowSelector textureQualitySelector;

			// Token: 0x040083C3 RID: 33731
			public Toggle ssaoToggle;

			// Token: 0x040083C4 RID: 33732
			public Toggle postProcessingToggle;

			// Token: 0x040083C5 RID: 33733
			public Toggle magicaClothToggle;

			// Token: 0x040083C6 RID: 33734
			public MainMenuArrowSelector colorVisionSelector;
		}
	}
}
