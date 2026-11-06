using System;
using System.Collections.Generic;
using DA_Assets.CR;
using Features.Localization;
using Features.Missions;
using Features.Rank;
using Features.Tutorials;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.MainMenu
{
	// Token: 0x02001098 RID: 4248
	public static class MainMenuSettingsRuntimeFactory
	{
		// Token: 0x060080A6 RID: 32934 RVA: 0x0034647C File Offset: 0x0034467C
		public static GameObject BuildHudSettings(MainMenuSettingsPanel controller, Transform graphicsPage, TMP_FontAsset font, out Toggle face, out Toggle minimap, out Toggle distanceAndScore)
		{
			RectTransform rectTransform = MainMenuSettingsRuntimeFactory.CreatePage(graphicsPage.parent, "PageHudDisplay");
			rectTransform.gameObject.SetActive(false);
			MainMenuSettingsRuntimeFactory.CreateNavRow(graphicsPage, 8, "HUD DISPLAY", ">", font).onClick.AddListener(new UnityAction(controller.OpenHudSettings));
			MainMenuSettingsRuntimeFactory.CreateNavRow(rectTransform, 0, "<   GRAPHICS", null, font).onClick.AddListener(new UnityAction(controller.OpenGraphics));
			face = MainMenuSettingsRuntimeFactory.CreateSwitchRow(rectTransform, 1, "SHOW FACE CAMERA", font);
			minimap = MainMenuSettingsRuntimeFactory.CreateSwitchRow(rectTransform, 2, "SHOW MINIMAP", font);
			distanceAndScore = MainMenuSettingsRuntimeFactory.CreateSwitchRow(rectTransform, 3, "SHOW DISTANCE & POINTS", font);
			return rectTransform.gameObject;
		}

		// Token: 0x060080A7 RID: 32935 RVA: 0x00346528 File Offset: 0x00344728
		public static MainMenuSettingsPanel Build(Transform parent, TMP_FontAsset font, Action closePanel, Action returnToMainMenu = null, Action unstuckPlayer = null)
		{
			GameObject gameObject = MainMenuSettingsRuntimeFactory.CreateRect("SettingsPanel", parent, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
			gameObject.SetActive(false);
			Image image = gameObject.AddComponent<Image>();
			image.color = MainMenuSettingsRuntimeFactory.Overlay;
			image.raycastTarget = true;
			gameObject.AddComponent<CanvasGroup>();
			MainMenuSettingsPanel mainMenuSettingsPanel = gameObject.AddComponent<MainMenuSettingsPanel>();
			MainMenuPanelIntroAnimator mainMenuPanelIntroAnimator = gameObject.AddComponent<MainMenuPanelIntroAnimator>();
			TMP_Text tmp_Text;
			TMP_Text tmp_Text2;
			RectTransform rectTransform = MainMenuSettingsRuntimeFactory.BuildTitleBlock(gameObject.transform, font, out tmp_Text, out tmp_Text2);
			GameObject gameObject2 = MainMenuSettingsRuntimeFactory.CreateRect("Pause Progression", gameObject.transform, Vector2.one, Vector2.one, new Vector2(-470f, -226f), new Vector2(-60f, -58f));
			gameObject2.AddComponent<PauseMenuProgressionPanel>().Initialize(font);
			RectTransform rectTransform2 = MainMenuSettingsRuntimeFactory.CreatePage(gameObject.transform, "PageRoot");
			RectTransform rectTransform3 = MainMenuSettingsRuntimeFactory.CreatePage(gameObject.transform, "PageAudio");
			RectTransform rectTransform4 = MainMenuSettingsRuntimeFactory.CreatePage(gameObject.transform, "PageGame");
			RectTransform rectTransform5 = MainMenuSettingsRuntimeFactory.CreatePage(gameObject.transform, "PageGraphics");
			RectTransform rectTransform6 = MainMenuSettingsRuntimeFactory.CreatePage(gameObject.transform, "PageGraphicsAdvanced");
			RectTransform rectTransform7 = MainMenuSettingsRuntimeFactory.CreatePage(gameObject.transform, "PageMissions");
			RectTransform rectTransform8 = MainMenuSettingsRuntimeFactory.CreatePage(gameObject.transform, "PageControls");
			RectTransform rectTransform9 = MainMenuSettingsRuntimeFactory.CreateWidePage(gameObject.transform, "PageTutorials");
			Button button = MainMenuSettingsRuntimeFactory.CreateNavRow(rectTransform2, 0, "<   BACK", null, font);
			Button button2 = MainMenuSettingsRuntimeFactory.CreateNavRow(rectTransform2, 1, "TUTORIAL", ">", font);
			Button button3 = MainMenuSettingsRuntimeFactory.CreateNavRow(rectTransform2, 2, "AUDIO", ">", font);
			Button button4 = MainMenuSettingsRuntimeFactory.CreateNavRow(rectTransform2, 3, "GAME SETTINGS", ">", font);
			Button button5 = MainMenuSettingsRuntimeFactory.CreateNavRow(rectTransform2, 4, "GRAPHICS", ">", font);
			Button button6 = MainMenuSettingsRuntimeFactory.CreateNavRow(rectTransform2, 5, "MISSIONS", ">", font);
			Button button7 = MainMenuSettingsRuntimeFactory.CreateNavRow(rectTransform2, 6, "CONTROLS", ">", font);
			Button button8 = MainMenuSettingsRuntimeFactory.CreateActionRow(rectTransform2, 7, "RESET ALL DEFAULTS", font);
			Button button9 = MainMenuSettingsRuntimeFactory.CreateActionRow(rectTransform2, 8, "RETURN TO MAIN MENU", font);
			Button button10 = MainMenuSettingsRuntimeFactory.CreateActionRow(MainMenuSettingsRuntimeFactory.CreateRect("Unstuck Action", gameObject2.transform, Vector2.zero, Vector2.right, new Vector2(0f, -88f), new Vector2(0f, -26f)).GetComponent<RectTransform>(), 0, "UNSTUCK", font);
			PauseMenuTutorialPanel pauseMenuTutorialPanel = rectTransform9.gameObject.AddComponent<PauseMenuTutorialPanel>();
			pauseMenuTutorialPanel.Initialize(font, new Action(mainMenuSettingsPanel.BackToRoot), closePanel);
			Button button11 = MainMenuSettingsRuntimeFactory.CreateNavRow(rectTransform7, 0, "<   BACK", null, font);
			Button button12 = MainMenuSettingsRuntimeFactory.CreateNavRow(rectTransform7, 1, "MISSION TYPE", ">", font);
			GameObject gameObject3 = MainMenuSettingsRuntimeFactory.CreateRect("Global Missions", rectTransform7, Vector2.zero, Vector2.one, new Vector2(0f, 0f), new Vector2(0f, -162f));
			gameObject3.AddComponent<GlobalMissionsPanel>().InitializeForPauseMenu(font);
			GameObject gameObject4 = MainMenuSettingsRuntimeFactory.CreateRect("Run Missions", rectTransform7, Vector2.zero, Vector2.one, Vector2.zero, new Vector2(0f, -162f));
			gameObject4.AddComponent<RunMissionsPanel>().Initialize(font);
			rectTransform7.gameObject.AddComponent<PauseMissionTabs>().Initialize(gameObject4, gameObject3, button12);
			Button button13 = MainMenuSettingsRuntimeFactory.CreateNavRow(rectTransform8, 0, "<   BACK", null, font);
			MainMenuSettingsRuntimeFactory.CreateRect("Controls List", rectTransform8, Vector2.zero, Vector2.one, Vector2.zero, new Vector2(0f, -86f)).AddComponent<PauseMenuControlsPanel>().Initialize(font);
			Button button14 = MainMenuSettingsRuntimeFactory.CreateNavRow(rectTransform3, 0, "<   BACK", null, font);
			MainMenuSettingsRuntimeFactory.SliderRow sliderRow = MainMenuSettingsRuntimeFactory.CreateSliderRow(rectTransform3, 1, "MASTER", 0f, 1f, font);
			MainMenuSettingsRuntimeFactory.SliderRow sliderRow2 = MainMenuSettingsRuntimeFactory.CreateSliderRow(rectTransform3, 2, "HEARTBEAT", 0f, 1f, font);
			MainMenuSettingsRuntimeFactory.SliderRow sliderRow3 = MainMenuSettingsRuntimeFactory.CreateSliderRow(rectTransform3, 3, "CIEL VOICE", 0f, 1f, font);
			MainMenuSettingsRuntimeFactory.SliderRow sliderRow4 = MainMenuSettingsRuntimeFactory.CreateSliderRow(rectTransform3, 4, "NPC REACTIONS", 0f, 1f, font);
			MainMenuSettingsRuntimeFactory.SliderRow sliderRow5 = MainMenuSettingsRuntimeFactory.CreateSliderRow(rectTransform3, 5, "MUSIC", 0f, 1f, font);
			MainMenuSettingsRuntimeFactory.SliderRow sliderRow6 = MainMenuSettingsRuntimeFactory.CreateSliderRow(rectTransform3, 6, "SFX", 0f, 1f, font);
			Toggle toggle = MainMenuSettingsRuntimeFactory.CreateSwitchRow(rectTransform3, 7, "MUTE ALL", font);
			Button button15 = MainMenuSettingsRuntimeFactory.CreateNavRow(rectTransform4, 0, "<   BACK", null, font);
			MainMenuSettingsRuntimeFactory.SliderRow sliderRow7 = MainMenuSettingsRuntimeFactory.CreateSliderRow(rectTransform4, 1, "MOUSE SENSITIVITY", 0.1f, 3f, font);
			Toggle toggle2 = MainMenuSettingsRuntimeFactory.CreateSwitchRow(rectTransform4, 2, "CIEL VOICE SUBTITLES", font);
			Toggle toggle3 = MainMenuSettingsRuntimeFactory.CreateSwitchRow(rectTransform4, 3, "CAMERA SHAKE (SUBWAY / EVENTS)", font);
			Toggle toggle4 = MainMenuSettingsRuntimeFactory.CreateSwitchRow(rectTransform4, 4, "STREAMER MODE (SFW)", font);
			Toggle toggle5 = MainMenuSettingsRuntimeFactory.CreateSwitchRow(rectTransform4, 5, "SHOW FPS ON HUD", font);
			MainMenuArrowSelector mainMenuArrowSelector = MainMenuSettingsRuntimeFactory.CreateSelectorRow(rectTransform4, 6, "PHONE VIDEO RESOLUTION", font);
			MainMenuArrowSelector mainMenuArrowSelector2 = MainMenuSettingsRuntimeFactory.CreateSelectorRow(rectTransform4, 7, "PHONE VIDEO FPS", font);
			Button button16 = MainMenuSettingsRuntimeFactory.CreateNavRow(rectTransform5, 0, "<   BACK", null, font);
			MainMenuArrowSelector mainMenuArrowSelector3 = MainMenuSettingsRuntimeFactory.CreateSelectorRow(rectTransform5, 1, "QUALITY", font);
			MainMenuArrowSelector mainMenuArrowSelector4 = MainMenuSettingsRuntimeFactory.CreateSelectorRow(rectTransform5, 2, "RESOLUTION", font);
			MainMenuArrowSelector mainMenuArrowSelector5 = MainMenuSettingsRuntimeFactory.CreateSelectorRow(rectTransform5, 3, "WINDOW MODE", font);
			MainMenuArrowSelector mainMenuArrowSelector6 = MainMenuSettingsRuntimeFactory.CreateSelectorRow(rectTransform5, 4, "FPS CAP", font);
			Toggle toggle6 = MainMenuSettingsRuntimeFactory.CreateSwitchRow(rectTransform5, 5, "VSYNC", font);
			Button button17 = MainMenuSettingsRuntimeFactory.CreateNavRow(rectTransform5, 6, "ADVANCED GRAPHICS", ">", font);
			Button button18 = MainMenuSettingsRuntimeFactory.CreateActionRow(rectTransform5, 7, "APPLY DISPLAY", font);
			Button button19 = MainMenuSettingsRuntimeFactory.CreateNavRow(rectTransform6, 0, "<   GRAPHICS", null, font);
			MainMenuArrowSelector mainMenuArrowSelector7 = MainMenuSettingsRuntimeFactory.CreateSelectorRow(rectTransform6, 1, "3D RESOLUTION", font);
			MainMenuArrowSelector mainMenuArrowSelector8 = MainMenuSettingsRuntimeFactory.CreateSelectorRow(rectTransform6, 2, "SHADOW QUALITY", font);
			MainMenuArrowSelector mainMenuArrowSelector9 = MainMenuSettingsRuntimeFactory.CreateSelectorRow(rectTransform6, 3, "VIEW DISTANCE", font);
			MainMenuArrowSelector mainMenuArrowSelector10 = MainMenuSettingsRuntimeFactory.CreateSelectorRow(rectTransform6, 4, "TEXTURE QUALITY", font);
			Toggle toggle7 = MainMenuSettingsRuntimeFactory.CreateSwitchRow(rectTransform6, 5, "AMBIENT OCCLUSION", font);
			Toggle toggle8 = MainMenuSettingsRuntimeFactory.CreateSwitchRow(rectTransform6, 6, "POST PROCESSING", font);
			MainMenuArrowSelector mainMenuArrowSelector11 = MainMenuSettingsRuntimeFactory.CreateSelectorRow(rectTransform6, 7, "COLOR VISION FILTER", font);
			Toggle toggle9 = MainMenuSettingsRuntimeFactory.CreateSwitchRow(rectTransform6, 8, "MAGICA PHYSICS", font);
			TMP_Text tmp_Text3 = MainMenuSettingsRuntimeFactory.CreateText("Hint", gameObject.transform, "Changes are saved automatically", 15f, FontStyles.Normal, TextAlignmentOptions.MidlineLeft, MainMenuSettingsRuntimeFactory.Muted, new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(120f, 26f), new Vector2(760f, 54f), font, true, 0f);
			tmp_Text3.raycastTarget = false;
			button.onClick.AddListener(delegate
			{
				Action closePanel2 = closePanel;
				if (closePanel2 == null)
				{
					return;
				}
				closePanel2();
			});
			button10.onClick.AddListener(delegate
			{
				if (PlayerUnstuckService.IsRecoveryAllowed)
				{
					Action unstuckPlayer2 = unstuckPlayer;
					if (unstuckPlayer2 == null)
					{
						return;
					}
					unstuckPlayer2();
				}
			});
			button3.onClick.AddListener(new UnityAction(mainMenuSettingsPanel.OpenAudioSettings));
			button4.onClick.AddListener(new UnityAction(mainMenuSettingsPanel.OpenGameSettings));
			button5.onClick.AddListener(new UnityAction(mainMenuSettingsPanel.OpenGraphics));
			button6.onClick.AddListener(new UnityAction(mainMenuSettingsPanel.OpenMissions));
			button7.onClick.AddListener(new UnityAction(mainMenuSettingsPanel.OpenControls));
			button2.onClick.AddListener(new UnityAction(mainMenuSettingsPanel.OpenTutorials));
			button14.onClick.AddListener(new UnityAction(mainMenuSettingsPanel.BackToRoot));
			button15.onClick.AddListener(new UnityAction(mainMenuSettingsPanel.BackToRoot));
			button16.onClick.AddListener(new UnityAction(mainMenuSettingsPanel.BackToRoot));
			button11.onClick.AddListener(new UnityAction(mainMenuSettingsPanel.BackToRoot));
			button13.onClick.AddListener(new UnityAction(mainMenuSettingsPanel.BackToRoot));
			button17.onClick.AddListener(new UnityAction(mainMenuSettingsPanel.OpenAdvancedGraphics));
			button19.onClick.AddListener(new UnityAction(mainMenuSettingsPanel.BackToGraphics));
			button18.onClick.AddListener(new UnityAction(mainMenuSettingsPanel.ApplyDisplay));
			button8.onClick.AddListener(new UnityAction(mainMenuSettingsPanel.ResetDefaults));
			button9.onClick.AddListener(delegate
			{
				Action returnToMainMenu2 = returnToMainMenu;
				if (returnToMainMenu2 == null)
				{
					return;
				}
				returnToMainMenu2();
			});
			sliderRow.reset.onClick.AddListener(new UnityAction(mainMenuSettingsPanel.ResetMasterRow));
			sliderRow2.reset.onClick.AddListener(new UnityAction(mainMenuSettingsPanel.ResetHeartbeatRow));
			sliderRow3.reset.onClick.AddListener(new UnityAction(mainMenuSettingsPanel.ResetCielVoiceRow));
			sliderRow4.reset.onClick.AddListener(new UnityAction(mainMenuSettingsPanel.ResetNpcReactionRow));
			sliderRow5.reset.onClick.AddListener(new UnityAction(mainMenuSettingsPanel.ResetMusicRow));
			sliderRow6.reset.onClick.AddListener(new UnityAction(mainMenuSettingsPanel.ResetSfxRow));
			sliderRow7.reset.onClick.AddListener(new UnityAction(mainMenuSettingsPanel.ResetSensitivityRow));
			mainMenuSettingsPanel.ConfigureRuntime(new MainMenuSettingsPanel.RuntimeBindings
			{
				rootPage = rectTransform2.gameObject,
				audioPage = rectTransform3.gameObject,
				gamePage = rectTransform4.gameObject,
				graphicsPage = rectTransform5.gameObject,
				advancedGraphicsPage = rectTransform6.gameObject,
				missionsPage = rectTransform7.gameObject,
				controlsPage = rectTransform8.gameObject,
				tutorialsPage = rectTransform9.gameObject,
				tutorialPanel = pauseMenuTutorialPanel,
				progressionPanel = gameObject2,
				unstuckButton = button10,
				pageTitle = tmp_Text,
				pageKicker = tmp_Text2,
				rootKicker = "GAME PAUSED",
				contextHint = tmp_Text3.gameObject,
				masterSlider = sliderRow.slider,
				masterValue = sliderRow.value,
				heartbeatSlider = sliderRow2.slider,
				heartbeatValue = sliderRow2.value,
				cielVoiceSlider = sliderRow3.slider,
				cielVoiceValue = sliderRow3.value,
				npcReactionSlider = sliderRow4.slider,
				npcReactionValue = sliderRow4.value,
				musicSlider = sliderRow5.slider,
				musicValue = sliderRow5.value,
				sfxSlider = sliderRow6.slider,
				sfxValue = sliderRow6.value,
				muteToggle = toggle,
				sensitivitySlider = sliderRow7.slider,
				sensitivityValue = sliderRow7.value,
				cielVoiceSubtitlesToggle = toggle2,
				cameraShakeToggle = toggle3,
				streamerModeToggle = toggle4,
				showFpsToggle = toggle5,
				phoneVideoResolutionSelector = mainMenuArrowSelector,
				phoneVideoFpsSelector = mainMenuArrowSelector2,
				qualitySelector = mainMenuArrowSelector3,
				resolutionSelector = mainMenuArrowSelector4,
				windowModeSelector = mainMenuArrowSelector5,
				fpsSelector = mainMenuArrowSelector6,
				vSyncToggle = toggle6,
				renderScaleSelector = mainMenuArrowSelector7,
				shadowQualitySelector = mainMenuArrowSelector8,
				viewDistanceSelector = mainMenuArrowSelector9,
				textureQualitySelector = mainMenuArrowSelector10,
				ssaoToggle = toggle7,
				postProcessingToggle = toggle8,
				magicaClothToggle = toggle9,
				colorVisionSelector = mainMenuArrowSelector11
			});
			mainMenuPanelIntroAnimator.Configure(new RectTransform[] { rectTransform2, rectTransform3, rectTransform4, rectTransform5, rectTransform6, rectTransform7, rectTransform8, rectTransform9 }, rectTransform);
			rectTransform3.gameObject.SetActive(false);
			rectTransform4.gameObject.SetActive(false);
			rectTransform5.gameObject.SetActive(false);
			rectTransform6.gameObject.SetActive(false);
			rectTransform7.gameObject.SetActive(false);
			rectTransform8.gameObject.SetActive(false);
			rectTransform9.gameObject.SetActive(false);
			MainMenuSettingsRuntimeFactory.ApplyLocalizedTextLayout(gameObject.transform);
			gameObject.SetActive(true);
			return mainMenuSettingsPanel;
		}

		// Token: 0x060080A8 RID: 32936 RVA: 0x00347098 File Offset: 0x00345298
		public static void ApplyLocalizedTextLayout(Transform panel)
		{
			if (panel == null)
			{
				return;
			}
			RectTransform rectTransform = panel.Find("TitleBlock") as RectTransform;
			if (rectTransform != null)
			{
				rectTransform.offsetMax = new Vector2(rectTransform.offsetMin.x + 1120f, rectTransform.offsetMax.y);
				Transform transform = rectTransform.Find("Title");
				TMP_Text tmp_Text = ((transform != null) ? transform.GetComponent<TMP_Text>() : null);
				if (tmp_Text != null)
				{
					MainMenuSettingsRuntimeFactory.ConfigureTextLayout(tmp_Text, LocalizedGameText.SourceFontSize(tmp_Text), false, 40f);
				}
			}
			foreach (object obj in panel)
			{
				Transform transform2 = (Transform)obj;
				string name = transform2.name;
				uint num = <PrivateImplementationDetails>.ComputeStringHash(name);
				if (num <= 1535075344U)
				{
					if (num != 1234681613U)
					{
						if (num != 1534246836U)
						{
							if (num != 1535075344U)
							{
								continue;
							}
							if (!(name == "PageAudio"))
							{
								continue;
							}
						}
						else if (!(name == "PageMapLighting"))
						{
							continue;
						}
					}
					else if (!(name == "PageGraphicsAdvanced"))
					{
						continue;
					}
				}
				else if (num <= 2193019586U)
				{
					if (num != 1799287996U)
					{
						if (num != 2193019586U)
						{
							continue;
						}
						if (!(name == "PageRoot"))
						{
							continue;
						}
					}
					else if (!(name == "PageGame"))
					{
						continue;
					}
				}
				else if (num != 3206315907U)
				{
					if (num != 3225602059U)
					{
						continue;
					}
					if (!(name == "PageHudDisplay"))
					{
						continue;
					}
				}
				else if (!(name == "PageGraphics"))
				{
					continue;
				}
				foreach (Transform transform3 in transform2.GetComponentsInChildren<Transform>(true))
				{
					if (transform3.name.StartsWith("Row - ", StringComparison.Ordinal))
					{
						Transform transform4 = transform3.Find("Label");
						TMP_Text tmp_Text2 = ((transform4 != null) ? transform4.GetComponent<TMP_Text>() : null);
						if (!(tmp_Text2 == null))
						{
							float num2 = LocalizedGameText.SourceFontSize(tmp_Text2);
							MainMenuSettingsRuntimeFactory.ConfigureTextLayout(tmp_Text2, num2, true, 0f);
							if (transform3.GetComponent<MainMenuArrowSelector>() != null)
							{
								tmp_Text2.rectTransform.anchorMax = Vector2.one;
								tmp_Text2.rectTransform.offsetMax = new Vector2(-350f, tmp_Text2.rectTransform.offsetMax.y);
								Transform transform5 = transform3.Find("Value");
								TMP_Text tmp_Text3 = ((transform5 != null) ? transform5.GetComponent<TMP_Text>() : null);
								if (tmp_Text3 != null)
								{
									MainMenuSettingsRuntimeFactory.ConfigureTextLayout(tmp_Text3, LocalizedGameText.SourceFontSize(tmp_Text3), true, 0f);
								}
							}
							else if (transform3.GetComponent<Toggle>() != null)
							{
								tmp_Text2.rectTransform.anchorMax = Vector2.one;
								tmp_Text2.rectTransform.offsetMax = new Vector2(-104f, tmp_Text2.rectTransform.offsetMax.y);
							}
						}
					}
				}
			}
		}

		// Token: 0x060080A9 RID: 32937 RVA: 0x003473BC File Offset: 0x003455BC
		private static void ConfigureTextLayout(TMP_Text text, float size, bool wrap, float minimumSize = 0f)
		{
			float num = ((minimumSize > 0f) ? minimumSize : (size * 0.75f));
			LocalizedGameText component = text.GetComponent<LocalizedGameText>();
			if (component != null)
			{
				component.SetSourceLayout(true, wrap, num, size);
				return;
			}
			text.enableAutoSizing = true;
			text.fontSizeMin = num;
			text.fontSizeMax = size;
			text.enableWordWrapping = wrap;
		}

		// Token: 0x060080AA RID: 32938 RVA: 0x00347414 File Offset: 0x00345614
		private static RectTransform BuildTitleBlock(Transform parent, TMP_FontAsset font, out TMP_Text title, out TMP_Text kicker)
		{
			RectTransform component = MainMenuSettingsRuntimeFactory.CreateRect("TitleBlock", parent, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(120f, -206f), new Vector2(1240f, -52f)).GetComponent<RectTransform>();
			title = MainMenuSettingsRuntimeFactory.CreateText("Title", component, "PAUSE MENU", 74f, FontStyles.Bold, TextAlignmentOptions.BottomLeft, MainMenuSettingsRuntimeFactory.White, new Vector2(0f, 0.3f), Vector2.one, Vector2.zero, Vector2.zero, font, false, 40f);
			kicker = MainMenuSettingsRuntimeFactory.CreateText("Kicker", component, "GAME PAUSED", 21f, FontStyles.Bold, TextAlignmentOptions.TopLeft, MainMenuSettingsRuntimeFactory.Pink, new Vector2(0f, 0f), new Vector2(1f, 0.3f), new Vector2(4f, 0f), Vector2.zero, font, true, 0f);
			return component;
		}

		// Token: 0x060080AB RID: 32939 RVA: 0x00347514 File Offset: 0x00345714
		private static RectTransform CreatePage(Transform parent, string name)
		{
			return MainMenuSettingsRuntimeFactory.CreateRect(name, parent, new Vector2(0f, 0f), new Vector2(0f, 1f), new Vector2(120f, 40f), new Vector2(760f, -252f)).GetComponent<RectTransform>();
		}

		// Token: 0x060080AC RID: 32940 RVA: 0x00347569 File Offset: 0x00345769
		private static RectTransform CreateWidePage(Transform parent, string name)
		{
			return MainMenuSettingsRuntimeFactory.CreateRect(name, parent, Vector2.zero, Vector2.one, new Vector2(120f, 40f), new Vector2(-120f, -252f)).GetComponent<RectTransform>();
		}

		// Token: 0x060080AD RID: 32941 RVA: 0x003475A0 File Offset: 0x003457A0
		private static RectTransform CreateRow(Transform page, string name, int index)
		{
			float num = -((float)index * 76f);
			GameObject gameObject = MainMenuSettingsRuntimeFactory.CreateRect(name, page, new Vector2(0f, 1f), Vector2.one, new Vector2(0f, num - 62f), new Vector2(0f, num));
			Image image = gameObject.AddComponent<Image>();
			gameObject.AddComponent<ControllerNavigationRow>();
			image.color = MainMenuSettingsRuntimeFactory.RowColor;
			MainMenuSettingsRuntimeFactory.Round(gameObject, 14f);
			return gameObject.GetComponent<RectTransform>();
		}

		// Token: 0x060080AE RID: 32942 RVA: 0x00347618 File Offset: 0x00345818
		private static Button CreateNavRow(Transform page, int index, string label, string chevron, TMP_FontAsset font)
		{
			RectTransform rectTransform = MainMenuSettingsRuntimeFactory.CreateRow(page, "Row - " + label, index);
			Button button = MainMenuSettingsRuntimeFactory.AddRowButton(rectTransform);
			MainMenuSettingsRuntimeFactory.CreateText("Label", rectTransform, label, 23f, FontStyles.Bold, TextAlignmentOptions.MidlineLeft, MainMenuSettingsRuntimeFactory.White, Vector2.zero, Vector2.one, new Vector2(28f, 0f), new Vector2(-56f, 0f), font, true, 0f);
			if (!string.IsNullOrEmpty(chevron))
			{
				MainMenuSettingsRuntimeFactory.CreateText("Chevron", rectTransform, chevron, 24f, FontStyles.Bold, TextAlignmentOptions.Center, MainMenuSettingsRuntimeFactory.Pink, new Vector2(1f, 0f), Vector2.one, new Vector2(-52f, 0f), new Vector2(-18f, 0f), font, true, 0f);
			}
			rectTransform.gameObject.AddComponent<MainMenuButtonFx>();
			return button;
		}

		// Token: 0x060080AF RID: 32943 RVA: 0x003476F8 File Offset: 0x003458F8
		private static Button CreateActionRow(Transform page, int index, string label, TMP_FontAsset font)
		{
			RectTransform rectTransform = MainMenuSettingsRuntimeFactory.CreateRow(page, "Row - " + label, index);
			Button button = MainMenuSettingsRuntimeFactory.AddRowButton(rectTransform);
			MainMenuSettingsRuntimeFactory.CreateText("Label", rectTransform, label, 23f, FontStyles.Bold, TextAlignmentOptions.Center, MainMenuSettingsRuntimeFactory.Pink, Vector2.zero, Vector2.one, new Vector2(28f, 0f), new Vector2(-28f, 0f), font, true, 0f);
			rectTransform.gameObject.AddComponent<MainMenuButtonFx>();
			return button;
		}

		// Token: 0x060080B0 RID: 32944 RVA: 0x00347778 File Offset: 0x00345978
		private static MainMenuSettingsRuntimeFactory.SliderRow CreateSliderRow(Transform page, int index, string label, float min, float max, TMP_FontAsset font)
		{
			RectTransform rectTransform = MainMenuSettingsRuntimeFactory.CreateRow(page, "Row - " + label, index);
			MainMenuSettingsRuntimeFactory.CreateText("Label", rectTransform, label, 21f, FontStyles.Bold, TextAlignmentOptions.MidlineLeft, MainMenuSettingsRuntimeFactory.White, Vector2.zero, new Vector2(0.42f, 1f), new Vector2(28f, 0f), Vector2.zero, font, true, 0f);
			GameObject gameObject = MainMenuSettingsRuntimeFactory.CreateRect("Slider", rectTransform, new Vector2(0.42f, 0.5f), new Vector2(1f, 0.5f), new Vector2(0f, -15f), new Vector2(-68f, 15f));
			Slider slider = gameObject.AddComponent<Slider>();
			slider.minValue = min;
			slider.maxValue = max;
			GameObject gameObject2 = MainMenuSettingsRuntimeFactory.CreateRect("Background", gameObject.transform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
			gameObject2.AddComponent<Image>().color = MainMenuSettingsRuntimeFactory.TrackColor;
			MainMenuSettingsRuntimeFactory.Round(gameObject2, 15f);
			RectTransform component = MainMenuSettingsRuntimeFactory.CreateRect("Fill Area", gameObject.transform, Vector2.zero, Vector2.one, new Vector2(3f, 3f), new Vector2(-3f, -3f)).GetComponent<RectTransform>();
			GameObject gameObject3 = MainMenuSettingsRuntimeFactory.CreateRect("Fill", component, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
			Image image = gameObject3.AddComponent<Image>();
			image.color = MainMenuSettingsRuntimeFactory.Pink;
			MainMenuSettingsRuntimeFactory.Round(gameObject3, 12f);
			RectTransform component2 = MainMenuSettingsRuntimeFactory.CreateRect("Handle Slide Area", gameObject.transform, Vector2.zero, Vector2.one, new Vector2(30f, 0f), new Vector2(-30f, 0f)).GetComponent<RectTransform>();
			GameObject gameObject4 = MainMenuSettingsRuntimeFactory.CreateRect("Handle", component2, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(-29f, -16f), new Vector2(29f, 16f));
			Image image2 = gameObject4.AddComponent<Image>();
			image2.color = MainMenuSettingsRuntimeFactory.Window;
			MainMenuSettingsRuntimeFactory.Round(gameObject4, 16f);
			TMP_Text tmp_Text = MainMenuSettingsRuntimeFactory.CreateText("Value", gameObject4.transform, "100", 17f, FontStyles.Bold, TextAlignmentOptions.Center, MainMenuSettingsRuntimeFactory.PalePink, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero, font, true, 0f);
			LocalizedGameText.BindFontOnly(tmp_Text);
			slider.fillRect = image.rectTransform;
			slider.handleRect = image2.rectTransform;
			slider.targetGraphic = image2;
			slider.direction = Slider.Direction.LeftToRight;
			gameObject.AddComponent<MainMenuUiSoundRelay>();
			return new MainMenuSettingsRuntimeFactory.SliderRow(slider, tmp_Text, MainMenuSettingsRuntimeFactory.CreateResetCircle(rectTransform, font));
		}

		// Token: 0x060080B1 RID: 32945 RVA: 0x00347A30 File Offset: 0x00345C30
		private static Button CreateResetCircle(RectTransform row, TMP_FontAsset font)
		{
			GameObject gameObject = MainMenuSettingsRuntimeFactory.CreateRect("Reset", row, new Vector2(1f, 0.5f), new Vector2(1f, 0.5f), new Vector2(-58f, -20f), new Vector2(-18f, 20f));
			Image image = gameObject.AddComponent<Image>();
			MainMenuSettingsRuntimeFactory.Round(gameObject, 20f);
			Button button = gameObject.AddComponent<Button>();
			button.targetGraphic = image;
			MainMenuSettingsRuntimeFactory.ApplyRowTint(button, MainMenuSettingsRuntimeFactory.SurfaceLight, MainMenuSettingsRuntimeFactory.RowHover);
			MainMenuSettingsRuntimeFactory.CreateText("Icon", gameObject.transform, "R", 17f, FontStyles.Bold, TextAlignmentOptions.Center, MainMenuSettingsRuntimeFactory.PalePink, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero, font, true, 0f);
			gameObject.AddComponent<MainMenuButtonFx>();
			return button;
		}

		// Token: 0x060080B2 RID: 32946 RVA: 0x00347AFC File Offset: 0x00345CFC
		private static Toggle CreateSwitchRow(Transform page, int index, string label, TMP_FontAsset font)
		{
			RectTransform rectTransform = MainMenuSettingsRuntimeFactory.CreateRow(page, "Row - " + label, index);
			Toggle toggle = rectTransform.gameObject.AddComponent<Toggle>();
			toggle.transition = Selectable.Transition.ColorTint;
			toggle.targetGraphic = rectTransform.GetComponent<Image>();
			MainMenuSettingsRuntimeFactory.ApplyRowTint(toggle, MainMenuSettingsRuntimeFactory.RowColor, MainMenuSettingsRuntimeFactory.RowHover);
			toggle.graphic = null;
			MainMenuSettingsRuntimeFactory.CreateText("Label", rectTransform, label, 21f, FontStyles.Bold, TextAlignmentOptions.MidlineLeft, MainMenuSettingsRuntimeFactory.White, Vector2.zero, Vector2.one, new Vector2(28f, 0f), new Vector2(-104f, 0f), font, true, 0f);
			GameObject gameObject = MainMenuSettingsRuntimeFactory.CreateRect("Track", rectTransform, new Vector2(1f, 0.5f), new Vector2(1f, 0.5f), new Vector2(-86f, -16f), new Vector2(-18f, 16f));
			Image image = gameObject.AddComponent<Image>();
			image.color = MainMenuSettingsRuntimeFactory.TrackColor;
			image.raycastTarget = false;
			MainMenuSettingsRuntimeFactory.Round(gameObject, 16f);
			GameObject gameObject2 = MainMenuSettingsRuntimeFactory.CreateRect("Knob", gameObject.transform, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0f, -12f), new Vector2(24f, 12f));
			Image image2 = gameObject2.AddComponent<Image>();
			image2.color = MainMenuSettingsRuntimeFactory.Muted;
			image2.raycastTarget = false;
			MainMenuSettingsRuntimeFactory.Round(gameObject2, 12f);
			rectTransform.gameObject.AddComponent<MainMenuSwitchVisual>().Configure(gameObject2.GetComponent<RectTransform>(), image);
			rectTransform.gameObject.AddComponent<MainMenuUiSoundRelay>();
			return toggle;
		}

		// Token: 0x060080B3 RID: 32947 RVA: 0x00347C9C File Offset: 0x00345E9C
		private static MainMenuArrowSelector CreateSelectorRow(Transform page, int index, string label, TMP_FontAsset font)
		{
			RectTransform rectTransform = MainMenuSettingsRuntimeFactory.CreateRow(page, "Row - " + label, index);
			MainMenuSettingsRuntimeFactory.CreateText("Label", rectTransform, label, 21f, FontStyles.Bold, TextAlignmentOptions.MidlineLeft, MainMenuSettingsRuntimeFactory.White, Vector2.zero, Vector2.one, new Vector2(28f, 0f), new Vector2(-350f, 0f), font, true, 0f);
			Button button = MainMenuSettingsRuntimeFactory.CreateArrowButton(rectTransform, "Prev", "<", new Vector2(-334f, -18f), new Vector2(-298f, 18f), font);
			TMP_Text tmp_Text = MainMenuSettingsRuntimeFactory.CreateText("Value", rectTransform, "-", 19f, FontStyles.Bold, TextAlignmentOptions.Center, MainMenuSettingsRuntimeFactory.White, new Vector2(1f, 0f), Vector2.one, new Vector2(-292f, 0f), new Vector2(-60f, 0f), font, true, 0f);
			Button button2 = MainMenuSettingsRuntimeFactory.CreateArrowButton(rectTransform, "Next", ">", new Vector2(-54f, -18f), new Vector2(-18f, 18f), font);
			MainMenuArrowSelector mainMenuArrowSelector = rectTransform.gameObject.AddComponent<MainMenuArrowSelector>();
			mainMenuArrowSelector.Configure(tmp_Text, button, button2);
			return mainMenuArrowSelector;
		}

		// Token: 0x060080B4 RID: 32948 RVA: 0x00347DD8 File Offset: 0x00345FD8
		public static Slider BuildStreamerPixelSetting(Transform gamePage, TMP_FontAsset font, out TMP_Text value)
		{
			MainMenuSettingsRuntimeFactory.SliderRow row = MainMenuSettingsRuntimeFactory.CreateSliderRow(gamePage, 10, "STREAMER PIXEL SIZE", 12f, 80f, font);
			row.slider.wholeNumbers = true;
			value = row.value;
			row.reset.onClick.AddListener(delegate
			{
				row.slider.value = 32f;
			});
			return row.slider;
		}

		// Token: 0x060080B5 RID: 32949 RVA: 0x00347E54 File Offset: 0x00346054
		public static Slider BuildBrightnessSetting(Transform graphicsPage, TMP_FontAsset font, out TMP_Text value)
		{
			foreach (object obj in graphicsPage)
			{
				Transform transform = (Transform)obj;
				RectTransform rectTransform = transform as RectTransform;
				if (rectTransform != null && transform.name.StartsWith("Row - "))
				{
					int num = Mathf.RoundToInt(-rectTransform.offsetMax.y / 76f);
					if (num >= 4)
					{
						num++;
					}
					float num2 = (float)(-(float)num) * 68f;
					rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, num2 - 62f);
					rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, num2);
				}
			}
			MainMenuSettingsRuntimeFactory.SliderRow row = MainMenuSettingsRuntimeFactory.CreateSliderRow(graphicsPage, 4, "BRIGHTNESS", 0.5f, 2f, font);
			RectTransform rectTransform2 = (RectTransform)row.slider.transform.parent;
			float num3 = -272f;
			rectTransform2.offsetMin = new Vector2(rectTransform2.offsetMin.x, num3 - 62f);
			rectTransform2.offsetMax = new Vector2(rectTransform2.offsetMax.x, num3);
			rectTransform2.SetSiblingIndex(4);
			row.slider.SetValueWithoutNotify(1f);
			value = row.value;
			value.text = "100%";
			row.reset.onClick.AddListener(delegate
			{
				row.slider.value = 1f;
			});
			TMP_Text tmp_Text = MainMenuSettingsRuntimeFactory.CreateText("Brightness help", graphicsPage, "Adjust brightness to make dark areas easier to see. Default: 100%.", 22f, FontStyles.Normal, TextAlignmentOptions.TopLeft, MainMenuSettingsRuntimeFactory.Muted, Vector2.one, Vector2.one, new Vector2(50f, -520f), new Vector2(620f, -410f), font, true, 0f);
			tmp_Text.enableWordWrapping = true;
			tmp_Text.raycastTarget = false;
			return row.slider;
		}

		// Token: 0x060080B6 RID: 32950 RVA: 0x00348068 File Offset: 0x00346268
		public static void FitGameSettingsRows(Transform gamePage)
		{
			if (gamePage == null)
			{
				return;
			}
			Transform transform = gamePage.Find("Game Settings Scroll");
			ScrollRect scrollRect = ((transform != null) ? transform.GetComponent<ScrollRect>() : null);
			bool flag = scrollRect == null;
			if (flag)
			{
				scrollRect = MainMenuSettingsRuntimeFactory.CreateGameSettingsScroll(gamePage);
			}
			List<KeyValuePair<int, RectTransform>> list = new List<KeyValuePair<int, RectTransform>>();
			int num = 0;
			foreach (Transform transform2 in gamePage.GetComponentsInChildren<Transform>(true))
			{
				RectTransform rectTransform = transform2 as RectTransform;
				if (rectTransform != null && transform2.name.StartsWith("Row - "))
				{
					int num2 = Mathf.RoundToInt(-rectTransform.offsetMax.y / 76f);
					StreamerSettingsRowIndex streamerSettingsRowIndex = transform2.GetComponent<StreamerSettingsRowIndex>();
					if (streamerSettingsRowIndex == null)
					{
						streamerSettingsRowIndex = transform2.gameObject.AddComponent<StreamerSettingsRowIndex>();
						streamerSettingsRowIndex.index = num2;
					}
					Transform transform3 = transform2.Find("Label");
					TMP_Text tmp_Text = ((transform3 != null) ? transform3.GetComponent<TMP_Text>() : null);
					if (tmp_Text != null && tmp_Text.GetComponent<LocalizedGameText>() == null)
					{
						LocalizedGameText.BindSource(tmp_Text, "settings", tmp_Text.text, null);
					}
					int num3 = streamerSettingsRowIndex.index;
					if (num3 == 12)
					{
						num3 = 2;
					}
					else if (num3 == 13)
					{
						num3 = 3;
					}
					else if (num3 >= 2)
					{
						num3 += 2;
					}
					list.Add(new KeyValuePair<int, RectTransform>(num3, rectTransform));
				}
			}
			list.Sort((KeyValuePair<int, RectTransform> left, KeyValuePair<int, RectTransform> right) => left.Key.CompareTo(right.Key));
			foreach (KeyValuePair<int, RectTransform> keyValuePair in list)
			{
				RectTransform value = keyValuePair.Value;
				bool flag2 = keyValuePair.Key == 0;
				if (!flag2)
				{
					num++;
				}
				Transform transform4 = (flag2 ? gamePage : scrollRect.content);
				if (value.parent != transform4)
				{
					value.SetParent(transform4, false);
				}
				value.anchorMin = new Vector2(0f, 1f);
				value.anchorMax = Vector2.one;
				float num4 = (float)(-(float)(flag2 ? 0 : (num - 1))) * 64f;
				value.offsetMin = new Vector2(0f, num4 - 58f);
				value.offsetMax = new Vector2(0f, num4);
				value.SetSiblingIndex(flag2 ? 0 : (num - 1));
			}
			scrollRect.content.sizeDelta = new Vector2(0f, Mathf.Max(0f, (float)num * 64f - 6f));
			if (flag)
			{
				scrollRect.verticalNormalizedPosition = 1f;
			}
		}

		// Token: 0x060080B7 RID: 32951 RVA: 0x00348320 File Offset: 0x00346520
		private static ScrollRect CreateGameSettingsScroll(Transform gamePage)
		{
			GameObject gameObject = MainMenuSettingsRuntimeFactory.CreateRect("Game Settings Scroll", gamePage, Vector2.zero, Vector2.one, new Vector2(0f, 24f), new Vector2(0f, -64f));
			ScrollRect scrollRect = gameObject.AddComponent<ScrollRect>();
			scrollRect.horizontal = false;
			scrollRect.vertical = true;
			scrollRect.movementType = ScrollRect.MovementType.Clamped;
			scrollRect.inertia = false;
			scrollRect.scrollSensitivity = 32f;
			GameObject gameObject2 = MainMenuSettingsRuntimeFactory.CreateRect("Viewport", gameObject.transform, Vector2.zero, Vector2.one, Vector2.zero, new Vector2(-18f, 0f));
			gameObject2.AddComponent<Image>().color = Color.clear;
			gameObject2.AddComponent<RectMask2D>();
			scrollRect.viewport = (RectTransform)gameObject2.transform;
			scrollRect.content = (RectTransform)MainMenuSettingsRuntimeFactory.CreateRect("Content", gameObject2.transform, Vector2.up, Vector2.one, Vector2.zero, Vector2.zero).transform;
			scrollRect.content.pivot = new Vector2(0.5f, 1f);
			GameObject gameObject3 = MainMenuSettingsRuntimeFactory.CreateRect("Scrollbar", gameObject.transform, Vector2.right, Vector2.one, new Vector2(-8f, 0f), Vector2.zero);
			gameObject3.AddComponent<Image>().color = MainMenuSettingsRuntimeFactory.TrackColor;
			GameObject gameObject4 = MainMenuSettingsRuntimeFactory.CreateRect("Handle", gameObject3.transform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
			Image image = gameObject4.AddComponent<Image>();
			image.color = MainMenuSettingsRuntimeFactory.Pink;
			Scrollbar scrollbar = gameObject3.AddComponent<Scrollbar>();
			scrollbar.handleRect = (RectTransform)gameObject4.transform;
			scrollbar.targetGraphic = image;
			scrollbar.direction = Scrollbar.Direction.BottomToTop;
			scrollbar.navigation = new Navigation
			{
				mode = Navigation.Mode.None
			};
			scrollRect.verticalScrollbar = scrollbar;
			scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
			return scrollRect;
		}

		// Token: 0x060080B8 RID: 32952 RVA: 0x003484FE File Offset: 0x003466FE
		public static Toggle BuildMouseInversionSetting(Transform gamePage, TMP_FontAsset font, bool horizontal)
		{
			return MainMenuSettingsRuntimeFactory.CreateSwitchRow(gamePage, horizontal ? 12 : 13, horizontal ? "INVERT MOUSE HORIZONTALLY" : "INVERT MOUSE VERTICALLY", font);
		}

		// Token: 0x060080B9 RID: 32953 RVA: 0x0034851F File Offset: 0x0034671F
		public static Toggle BuildGarbageMudSetting(Transform gamePage, TMP_FontAsset font)
		{
			return MainMenuSettingsRuntimeFactory.CreateSwitchRow(gamePage, 9, "MUD FROM TRASHCANS / DUMPSTERS", font);
		}

		// Token: 0x060080BA RID: 32954 RVA: 0x0034852F File Offset: 0x0034672F
		public static MainMenuArrowSelector BuildBodyPhysicsSetting(Transform gamePage, TMP_FontAsset font)
		{
			return MainMenuSettingsRuntimeFactory.CreateSelectorRow(gamePage, 11, "BODY PHYSICS", font);
		}

		// Token: 0x060080BB RID: 32955 RVA: 0x00348540 File Offset: 0x00346740
		public static LanguageDropdown BuildLanguageSelector(Transform gamePage, TMP_FontAsset font)
		{
			RectTransform rectTransform = MainMenuSettingsRuntimeFactory.CreateRow(gamePage, "Row - LANGUAGE", 8);
			MainMenuSettingsRuntimeFactory.CreateText("Label", rectTransform, "LANGUAGE", 21f, FontStyles.Bold, TextAlignmentOptions.MidlineLeft, MainMenuSettingsRuntimeFactory.White, Vector2.zero, new Vector2(0.36f, 1f), new Vector2(28f, 0f), Vector2.zero, font, true, 0f);
			LanguageDropdown languageDropdown = LanguageDropdown.Create(rectTransform, font);
			RectTransform rectTransform2 = (RectTransform)languageDropdown.transform;
			rectTransform2.anchorMin = new Vector2(0.37f, 0f);
			rectTransform2.offsetMin = new Vector2(0f, 6f);
			rectTransform2.offsetMax = new Vector2(-12f, -6f);
			return languageDropdown;
		}

		// Token: 0x060080BC RID: 32956 RVA: 0x003485FC File Offset: 0x003467FC
		private static Button CreateArrowButton(RectTransform row, string name, string glyph, Vector2 offsetMin, Vector2 offsetMax, TMP_FontAsset font)
		{
			GameObject gameObject = MainMenuSettingsRuntimeFactory.CreateRect(name, row, new Vector2(1f, 0.5f), new Vector2(1f, 0.5f), offsetMin, offsetMax);
			Image image = gameObject.AddComponent<Image>();
			MainMenuSettingsRuntimeFactory.Round(gameObject, 18f);
			Button button = gameObject.AddComponent<Button>();
			button.targetGraphic = image;
			MainMenuSettingsRuntimeFactory.ApplyRowTint(button, MainMenuSettingsRuntimeFactory.SurfaceLight, MainMenuSettingsRuntimeFactory.RowHover);
			MainMenuSettingsRuntimeFactory.CreateText("Glyph", gameObject.transform, glyph, 21f, FontStyles.Bold, TextAlignmentOptions.Center, MainMenuSettingsRuntimeFactory.Pink, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero, font, true, 0f);
			gameObject.AddComponent<MainMenuButtonFx>();
			return button;
		}

		// Token: 0x060080BD RID: 32957 RVA: 0x003486A6 File Offset: 0x003468A6
		private static Button AddRowButton(RectTransform row)
		{
			Button button = row.gameObject.AddComponent<Button>();
			button.targetGraphic = row.GetComponent<Image>();
			MainMenuSettingsRuntimeFactory.ApplyRowTint(button, MainMenuSettingsRuntimeFactory.RowColor, MainMenuSettingsRuntimeFactory.RowHover);
			return button;
		}

		// Token: 0x060080BE RID: 32958 RVA: 0x003486D0 File Offset: 0x003468D0
		private static void ApplyRowTint(Selectable selectable, Color normal, Color highlighted)
		{
			if (selectable.targetGraphic != null)
			{
				selectable.targetGraphic.color = Color.white;
			}
			ColorBlock colors = selectable.colors;
			colors.normalColor = normal;
			colors.highlightedColor = highlighted;
			colors.pressedColor = Color.Lerp(highlighted, Color.white, 0.22f);
			colors.selectedColor = normal;
			colors.disabledColor = new Color(normal.r, normal.g, normal.b, 0.35f);
			colors.colorMultiplier = 1f;
			colors.fadeDuration = 0.08f;
			selectable.colors = colors;
		}

		// Token: 0x060080BF RID: 32959 RVA: 0x00348774 File Offset: 0x00346974
		private static TMP_Text CreateText(string name, Transform parent, string value, float size, FontStyles style, TextAlignmentOptions alignment, Color color, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax, TMP_FontAsset font, bool wrap = true, float minimumSize = 0f)
		{
			TextMeshProUGUI textMeshProUGUI = MainMenuSettingsRuntimeFactory.CreateRect(name, parent, anchorMin, anchorMax, offsetMin, offsetMax).AddComponent<TextMeshProUGUI>();
			textMeshProUGUI.fontSize = size;
			textMeshProUGUI.fontStyle = style;
			textMeshProUGUI.alignment = alignment;
			textMeshProUGUI.color = color;
			textMeshProUGUI.raycastTarget = false;
			textMeshProUGUI.overflowMode = TextOverflowModes.Ellipsis;
			MainMenuSettingsRuntimeFactory.ConfigureTextLayout(textMeshProUGUI, size, wrap, minimumSize);
			if (font != null)
			{
				textMeshProUGUI.font = font;
			}
			LocalizedGameText.BindSource(textMeshProUGUI, "settings", value, null);
			return textMeshProUGUI;
		}

		// Token: 0x060080C0 RID: 32960 RVA: 0x003487F0 File Offset: 0x003469F0
		private static GameObject CreateRect(string name, Transform parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax)
		{
			GameObject gameObject = new GameObject(name, new Type[] { typeof(RectTransform) });
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.SetParent(parent, false);
			component.anchorMin = anchorMin;
			component.anchorMax = anchorMax;
			component.offsetMin = offsetMin;
			component.offsetMax = offsetMax;
			component.localScale = Vector3.one;
			return gameObject;
		}

		// Token: 0x060080C1 RID: 32961 RVA: 0x001A3764 File Offset: 0x001A1964
		private static void Round(GameObject target, float radius)
		{
			target.AddComponent<CornerRounder>().SetRadii(new Vector4(radius, radius, radius, radius));
		}

		// Token: 0x060080C2 RID: 32962 RVA: 0x0034884C File Offset: 0x00346A4C
		public static GameObject BuildWorldGraphics(MainMenuSettingsPanel controller, Transform graphicsPage, TMP_FontAsset font, out MainMenuArrowSelector lighting, out TMP_Text status, out TMP_Text graphicsStatus)
		{
			RectTransform rectTransform = MainMenuSettingsRuntimeFactory.CreatePage(graphicsPage.parent, "PageMapLighting");
			rectTransform.gameObject.SetActive(false);
			MainMenuSettingsRuntimeFactory.CreateNavRow(graphicsPage, 9, "MAP & LIGHTING", ">", font).onClick.AddListener(new UnityAction(controller.OpenWorldGraphics));
			MainMenuSettingsRuntimeFactory.CreateNavRow(rectTransform, 0, "<   GRAPHICS", null, font).onClick.AddListener(new UnityAction(controller.OpenGraphics));
			lighting = MainMenuSettingsRuntimeFactory.CreateSelectorRow(rectTransform, 1, "MAP LIGHTING *", font);
			MainMenuSettingsRuntimeFactory.CreateText("Lighting details", rectTransform, "PERFORMANCE: fewer lights, lowest cost.\nBALANCED: more lights, wider coverage.\nORIGINAL: the full original lighting.\n\nMap optimization is enabled for every graphics preset.", 20f, FontStyles.Normal, TextAlignmentOptions.TopLeft, MainMenuSettingsRuntimeFactory.Muted, new Vector2(0f, 1f), Vector2.one, new Vector2(0f, -395f), new Vector2(0f, -168f), font, true, 0f).enableWordWrapping = true;
			status = MainMenuSettingsRuntimeFactory.CreateText("Restart status", rectTransform, "", 21f, FontStyles.Bold, TextAlignmentOptions.TopLeft, MainMenuSettingsRuntimeFactory.PalePink, new Vector2(0f, 1f), Vector2.one, new Vector2(0f, -500f), new Vector2(0f, -410f), font, true, 0f);
			status.enableWordWrapping = true;
			MainMenuSettingsRuntimeFactory.CreateText("Restart instructions", rectTransform, "* Return to the main menu and start a new run to apply lighting changes. Other graphics settings apply immediately.", 19f, FontStyles.Normal, TextAlignmentOptions.TopLeft, MainMenuSettingsRuntimeFactory.Muted, new Vector2(0f, 1f), Vector2.one, new Vector2(0f, -620f), new Vector2(0f, -515f), font, true, 0f).enableWordWrapping = true;
			graphicsStatus = MainMenuSettingsRuntimeFactory.CreateText("Map restart notice", graphicsPage, "", 22f, FontStyles.Normal, TextAlignmentOptions.TopLeft, MainMenuSettingsRuntimeFactory.Muted, Vector2.one, Vector2.one, new Vector2(50f, -370f), new Vector2(620f, -140f), font, true, 0f);
			graphicsStatus.enableWordWrapping = true;
			return rectTransform.gameObject;
		}

		// Token: 0x040083F8 RID: 33784
		private static readonly Color Overlay = new Color(0.012f, 0.006f, 0.016f, 0.95f);

		// Token: 0x040083F9 RID: 33785
		private static readonly Color Window = new Color(0.075f, 0.035f, 0.075f, 0.985f);

		// Token: 0x040083FA RID: 33786
		private static readonly Color SurfaceLight = new Color(0.21f, 0.095f, 0.19f, 0.98f);

		// Token: 0x040083FB RID: 33787
		private static readonly Color Pink = new Color(1f, 0.31f, 0.61f, 1f);

		// Token: 0x040083FC RID: 33788
		private static readonly Color PalePink = new Color(1f, 0.78f, 0.89f, 1f);

		// Token: 0x040083FD RID: 33789
		private static readonly Color White = new Color(1f, 0.97f, 1f, 1f);

		// Token: 0x040083FE RID: 33790
		private static readonly Color Muted = new Color(0.78f, 0.67f, 0.76f, 1f);

		// Token: 0x040083FF RID: 33791
		private static readonly Color RowColor = new Color(0.165f, 0.118f, 0.2f, 0.9f);

		// Token: 0x04008400 RID: 33792
		private static readonly Color RowHover = new Color(0.243f, 0.165f, 0.298f, 1f);

		// Token: 0x04008401 RID: 33793
		private static readonly Color TrackColor = new Color(0.035f, 0.015f, 0.035f, 1f);

		// Token: 0x04008402 RID: 33794
		private const float ColumnX = 120f;

		// Token: 0x04008403 RID: 33795
		private const float ColumnWidth = 640f;

		// Token: 0x04008404 RID: 33796
		private const float ColumnTop = -252f;

		// Token: 0x04008405 RID: 33797
		private const float RowHeight = 62f;

		// Token: 0x04008406 RID: 33798
		private const float RowPitch = 76f;

		// Token: 0x04008407 RID: 33799
		private const float RowRadius = 14f;

		// Token: 0x02001099 RID: 4249
		private readonly struct SliderRow
		{
			// Token: 0x060080C4 RID: 32964 RVA: 0x00348B91 File Offset: 0x00346D91
			public SliderRow(Slider slider, TMP_Text value, Button reset)
			{
				this.slider = slider;
				this.value = value;
				this.reset = reset;
			}

			// Token: 0x04008408 RID: 33800
			public readonly Slider slider;

			// Token: 0x04008409 RID: 33801
			public readonly TMP_Text value;

			// Token: 0x0400840A RID: 33802
			public readonly Button reset;
		}
	}
}
