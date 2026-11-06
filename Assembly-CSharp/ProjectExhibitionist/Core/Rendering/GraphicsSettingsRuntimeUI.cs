using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Features.CielOS;
using Features.DebugTools;
using Features.Detection;
using Features.GameOver;
using Features.Home;
using Features.Loading;
using Features.Localization;
using Features.MainMenu;
using Features.PhoneCompanion;
using Features.Police;
using Features.PrisonIntro;
using Features.Rank;
using Features.RunModes;
using Features.Tutorials;
using Michsky.MUIP;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.PoiTravel;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ProjectExhibitionist.Core.Rendering
{
	// Token: 0x02000AA7 RID: 2727
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(10000)]
	public class GraphicsSettingsRuntimeUI : MonoBehaviour
	{
		// Token: 0x17000D13 RID: 3347
		// (get) Token: 0x06004D97 RID: 19863 RVA: 0x001E1952 File Offset: 0x001DFB52
		// (set) Token: 0x06004D98 RID: 19864 RVA: 0x001E1959 File Offset: 0x001DFB59
		public static bool AnyOpen { get; private set; }

		// Token: 0x17000D14 RID: 3348
		// (get) Token: 0x06004D99 RID: 19865 RVA: 0x001E1961 File Offset: 0x001DFB61
		public static bool ConsumedToggleInputThisFrame
		{
			get
			{
				return GraphicsSettingsRuntimeUI._toggleInputFrame == Time.frameCount;
			}
		}

		// Token: 0x17000D15 RID: 3349
		// (get) Token: 0x06004D9A RID: 19866 RVA: 0x001E196F File Offset: 0x001DFB6F
		public bool IsOpen
		{
			get
			{
				return this._isOpen;
			}
		}

		// Token: 0x17000D16 RID: 3350
		// (get) Token: 0x06004D9B RID: 19867 RVA: 0x001E1978 File Offset: 0x001DFB78
		public bool CanOpen
		{
			get
			{
				return !GameplayInputGate.BlocksGameplayInput && !LongWayHomeRuntimeController.IsReturningHome && !CielHomeIntroSequence.IsAnyRunning && !PrisonIntroSequence.IsAnyRunning && (GameOverManager.Instance == null || (!GameOverManager.Instance.IsGameOver && !GameOverManager.Instance.HasPendingCapture)) && (PerpWalkController.Instance == null || !PerpWalkController.Instance.IsActive) && (ExhibitionEventDirector.Instance == null || !ExhibitionEventDirector.Instance.IsEventActive) && !PoiArrivalPresentation.IsPresentingInActiveScene;
			}
		}

		// Token: 0x17000D17 RID: 3351
		// (get) Token: 0x06004D9C RID: 19868 RVA: 0x001E1A02 File Offset: 0x001DFC02
		private GraphicsSettingsController Ctrl
		{
			get
			{
				if (!(this.controller != null))
				{
					return GraphicsSettingsController.Instance;
				}
				return this.controller;
			}
		}

		// Token: 0x06004D9D RID: 19869 RVA: 0x001E1A20 File Offset: 0x001DFC20
		private void Start()
		{
			if (this.controller == null)
			{
				this.controller = GraphicsSettingsController.Instance;
			}
			this.BuildShell();
			this._isOpen = false;
			this._canvas.gameObject.SetActive(false);
			if (this.pauseGameWhenOpen && Time.timeScale == 0f)
			{
				Time.timeScale = 1f;
			}
			TutorialFirstUseController tutorialFirstUseController = base.GetComponent<TutorialFirstUseController>();
			if (tutorialFirstUseController == null)
			{
				tutorialFirstUseController = base.gameObject.AddComponent<TutorialFirstUseController>();
			}
			tutorialFirstUseController.Initialize(this);
		}

		// Token: 0x06004D9E RID: 19870 RVA: 0x001E1AA8 File Offset: 0x001DFCA8
		private void Update()
		{
			if (EscapeInputCoordinator.TryConsumeForSettings(this.toggleKey, this))
			{
				GraphicsSettingsRuntimeUI._toggleInputFrame = Time.frameCount;
				if (!this._isOpen && !this.CanOpen)
				{
					if (!GameSettingsService.ShowGameplayUI)
					{
						GameSettingsService.SetShowGameplayUI(true);
						return;
					}
					BlockedActionFeedback.Show("Pause unavailable during this encounter");
					return;
				}
				else
				{
					if (this._isOpen && this._premiumSettingsPanel != null && this._premiumSettingsPanel.TryHandleBack())
					{
						return;
					}
					this.SetOpen(!this._isOpen);
				}
			}
			if (!this._isOpen)
			{
				return;
			}
			float unscaledDeltaTime = Time.unscaledDeltaTime;
			this._fpsSmoothed = ((this._fpsSmoothed <= 0f) ? (1f / unscaledDeltaTime) : Mathf.Lerp(this._fpsSmoothed, 1f / unscaledDeltaTime, 0.08f));
			this._fpsTimer += unscaledDeltaTime;
			if (this._fpsTimer >= 0.25f && this._titleInfo != null)
			{
				this._fpsTimer = 0f;
				TMP_Text titleInfo = this._titleInfo;
				string text = "{0}   |   {1} FPS";
				GraphicsSettingsController ctrl = this.Ctrl;
				titleInfo.text = string.Format(text, ((ctrl != null) ? ctrl.CurrentPresetName : null) ?? "-", Mathf.RoundToInt(this._fpsSmoothed));
			}
		}

		// Token: 0x06004D9F RID: 19871 RVA: 0x001E1BE0 File Offset: 0x001DFDE0
		private void LateUpdate()
		{
			if (this._isOpen)
			{
				this._sceneUiSuppression.Refresh();
			}
			if (this._isOpen && this.overrideCursorWhenOpen && (!Application.isPlaying || Application.isFocused))
			{
				if (Cursor.lockState != CursorLockMode.None)
				{
					Cursor.lockState = CursorLockMode.None;
				}
				bool flag = !ControllerInput.IsActive;
				if (Cursor.visible != flag)
				{
					Cursor.visible = flag;
				}
			}
		}

		// Token: 0x06004DA0 RID: 19872 RVA: 0x001E1C42 File Offset: 0x001DFE42
		private void OnDestroy()
		{
			if (this._isOpen)
			{
				this.RestoreGameState();
			}
			this._sceneUiSuppression.Restore();
			GraphicsSettingsRuntimeUI.AnyOpen = false;
		}

		// Token: 0x06004DA1 RID: 19873 RVA: 0x001E1C63 File Offset: 0x001DFE63
		private void OnDisable()
		{
			if (this._isOpen)
			{
				this.SetOpen(false);
			}
		}

		// Token: 0x06004DA2 RID: 19874 RVA: 0x001E1C74 File Offset: 0x001DFE74
		public void SetOpen(bool open)
		{
			if (open == this._isOpen)
			{
				return;
			}
			if (open && !this.CanOpen)
			{
				return;
			}
			if (open)
			{
				FlyCamera.StopForContextChange();
			}
			this._isOpen = open;
			GraphicsSettingsRuntimeUI.AnyOpen = open;
			if (!open)
			{
				if (this._canvas != null)
				{
					this._canvas.gameObject.SetActive(false);
				}
				this.HideConfirmBar(false);
				this._sceneUiSuppression.Restore();
				TutorialFirstUseController component = base.GetComponent<TutorialFirstUseController>();
				if (component != null)
				{
					component.NotifyDismissed();
				}
				this.RestoreGameState();
				return;
			}
			this.CaptureAndOverrideGameState();
			this._sceneUiSuppression.Suppress(new Transform[] { (this._canvas != null) ? this._canvas.transform : null });
			if (this._canvas != null)
			{
				this._canvas.gameObject.SetActive(true);
			}
			MainMenuSettingsPanel premiumSettingsPanel = this._premiumSettingsPanel;
			if (premiumSettingsPanel == null)
			{
				return;
			}
			premiumSettingsPanel.RefreshFromCurrentSettings();
		}

		// Token: 0x06004DA3 RID: 19875 RVA: 0x001E1D60 File Offset: 0x001DFF60
		public bool ShowTutorial(string topicId, bool automatic)
		{
			if (this._premiumSettingsPanel == null || this._isOpen || !this.CanOpen)
			{
				return false;
			}
			this.SetOpen(true);
			if (!this._isOpen)
			{
				return false;
			}
			if (this._premiumSettingsPanel.OpenTutorial(topicId, automatic))
			{
				return true;
			}
			this.SetOpen(false);
			return false;
		}

		// Token: 0x06004DA4 RID: 19876 RVA: 0x001E1DB7 File Offset: 0x001DFFB7
		public bool ShowNextTutorial(string topicId)
		{
			return this._isOpen && this._premiumSettingsPanel != null && this._premiumSettingsPanel.OpenTutorial(topicId, true);
		}

		// Token: 0x06004DA5 RID: 19877 RVA: 0x001E1DE0 File Offset: 0x001DFFE0
		private void CaptureAndOverrideGameState()
		{
			if (this.pauseGameWhenOpen)
			{
				this._previousTimeScale = Time.timeScale;
				Time.timeScale = 0f;
				this._audioPause = GameplayAudioPause.Acquire();
			}
			if (this.overrideCursorWhenOpen)
			{
				this._previousCursorLock = Cursor.lockState;
				this._previousCursorVisible = Cursor.visible;
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = !ControllerInput.IsActive;
			}
		}

		// Token: 0x06004DA6 RID: 19878 RVA: 0x001E1E48 File Offset: 0x001E0048
		private void RestoreGameState()
		{
			IDisposable audioPause = this._audioPause;
			if (audioPause != null)
			{
				audioPause.Dispose();
			}
			this._audioPause = null;
			if (this.pauseGameWhenOpen)
			{
				Time.timeScale = this._previousTimeScale;
			}
			if (this.overrideCursorWhenOpen)
			{
				if (this._returningToMainMenu)
				{
					Cursor.lockState = CursorLockMode.None;
					Cursor.visible = !ControllerInput.IsActive;
					return;
				}
				bool flag = CielOSController.AnyOpen || SmartphoneController.IsPhonePointerActive || PlayerActivityGate.IsResourceOwned(PlayerContextResource.ModalUI) || TextInputFocus.BlocksGameplayInput;
				bool flag2 = PlayerRegistry.Player != null && !GameplayInputGate.BlocksGameplayInput;
				Cursor.lockState = ((flag2 && !flag) ? CursorLockMode.Locked : this._previousCursorLock);
				Cursor.visible = (!flag2 || flag) && this._previousCursorVisible;
			}
		}

		// Token: 0x06004DA7 RID: 19879 RVA: 0x001E1F04 File Offset: 0x001E0104
		private void BuildShell()
		{
			GameObject gameObject = new GameObject("Settings_Canvas", new Type[]
			{
				typeof(Canvas),
				typeof(CanvasScaler),
				typeof(GraphicRaycaster)
			});
			gameObject.transform.SetParent(base.transform, false);
			this._canvas = gameObject.GetComponent<Canvas>();
			this._canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			this._canvas.sortingOrder = this.sortOrder;
			GameplayHudVisibility.RegisterCanvas(this._canvas);
			CanvasScaler component = gameObject.GetComponent<CanvasScaler>();
			component.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			component.referenceResolution = new Vector2(1980f, 1080f);
			component.matchWidthOrHeight = 0.5f;
			GraphicsSettingsRuntimeUI.EnsureEventSystem();
			this._premiumSettingsPanel = MainMenuSettingsRuntimeFactory.Build(gameObject.transform, this.premiumFont, delegate
			{
				this.SetOpen(false);
			}, new Action(this.ReturnToMainMenu), new Action(this.UnstuckPlayer));
		}

		// Token: 0x06004DA8 RID: 19880 RVA: 0x001E1FFC File Offset: 0x001E01FC
		private void UnstuckPlayer()
		{
			if (!PlayerUnstuckService.IsRecoveryAllowed)
			{
				return;
			}
			this.SetOpen(false);
			PlayerUnstuckService.RequestPlayerRecovery(delegate(bool recovered, string failureReason)
			{
				if (recovered)
				{
					return;
				}
				if (failureReason != null && failureReason.StartsWith("Recovery cancelled", StringComparison.Ordinal))
				{
					return;
				}
				Debug.LogWarning("[Unstuck] " + failureReason);
				if (!GameplayInputGate.IsLoading && PlayerRegistry.Player != null)
				{
					BlockedActionFeedback.Show("Unable to find a safe spot. Please try again.");
				}
			});
			if (PlayerUnstuckService.IsRecoveryPending)
			{
				BlockedActionFeedback.Show("Repositioning...");
			}
		}

		// Token: 0x06004DA9 RID: 19881 RVA: 0x001E2050 File Offset: 0x001E0250
		private void ReturnToMainMenu()
		{
			if (this._returningToMainMenu)
			{
				return;
			}
			if (!Application.CanStreamedLevelBeLoaded("MainMenu"))
			{
				Debug.LogError("[Settings] Scene 'MainMenu' is not available in Build Settings.");
				return;
			}
			LoadingScreenController instance = LoadingScreenController.Instance;
			if (instance == null)
			{
				Debug.LogError("[Settings] LoadingScreenController could not be created.");
				return;
			}
			this._returningToMainMenu = true;
			this.SetOpen(false);
			Time.timeScale = 1f;
			instance.LoadSceneAsync("MainMenu");
		}

		// Token: 0x06004DAA RID: 19882 RVA: 0x001E20BC File Offset: 0x001E02BC
		private void BuildLegacyShell()
		{
			GameObject gameObject = new GameObject("GraphicsSettings_Canvas", new Type[]
			{
				typeof(Canvas),
				typeof(CanvasScaler),
				typeof(GraphicRaycaster)
			});
			gameObject.transform.SetParent(base.transform, false);
			this._canvas = gameObject.GetComponent<Canvas>();
			this._canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			this._canvas.sortingOrder = this.sortOrder;
			GameplayHudVisibility.RegisterCanvas(this._canvas);
			CanvasScaler component = gameObject.GetComponent<CanvasScaler>();
			component.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			component.referenceResolution = new Vector2(1920f, 1080f);
			component.matchWidthOrHeight = 0.5f;
			GraphicsSettingsRuntimeUI.EnsureEventSystem();
			GraphicsSettingsRuntimeUI.Stretch(GraphicsSettingsRuntimeUI.CreateImage(gameObject.transform, "InputBlocker", new Color(0f, 0f, 0f, 0.55f)).rectTransform);
			this._panel = new GameObject("Panel", new Type[] { typeof(Image) });
			this._panel.transform.SetParent(gameObject.transform, false);
			this._panel.GetComponent<Image>().color = GraphicsSettingsRuntimeUI.PanelColor;
			RectTransform component2 = this._panel.GetComponent<RectTransform>();
			Vector2 vector = new Vector2(0.5f, 0.5f);
			component2.pivot = vector;
			component2.anchorMin = (component2.anchorMax = vector);
			component2.sizeDelta = GraphicsSettingsRuntimeUI.PanelSize;
			RectTransform rectTransform = GraphicsSettingsRuntimeUI.CreateText(this._panel.transform, "SETTINGS", 26f, FontStyles.Bold, TextAlignmentOptions.Left).rectTransform;
			rectTransform.anchorMin = new Vector2(0f, 1f);
			rectTransform.anchorMax = new Vector2(0.5f, 1f);
			rectTransform.pivot = new Vector2(0f, 1f);
			rectTransform.anchoredPosition = new Vector2(28f, -18f);
			rectTransform.sizeDelta = new Vector2(0f, 36f);
			this._titleInfo = GraphicsSettingsRuntimeUI.CreateText(this._panel.transform, "", 18f, FontStyles.Normal, TextAlignmentOptions.Right);
			RectTransform rectTransform2 = this._titleInfo.rectTransform;
			rectTransform2.anchorMin = new Vector2(0.5f, 1f);
			rectTransform2.anchorMax = new Vector2(1f, 1f);
			rectTransform2.pivot = new Vector2(1f, 1f);
			rectTransform2.anchoredPosition = new Vector2(-28f, -22f);
			rectTransform2.sizeDelta = new Vector2(0f, 30f);
			this._titleInfo.color = new Color(1f, 1f, 1f, 0.65f);
			GameObject gameObject2 = new GameObject("TabBar", new Type[]
			{
				typeof(RectTransform),
				typeof(HorizontalLayoutGroup)
			});
			gameObject2.transform.SetParent(this._panel.transform, false);
			RectTransform component3 = gameObject2.GetComponent<RectTransform>();
			component3.anchorMin = new Vector2(0f, 1f);
			component3.anchorMax = new Vector2(1f, 1f);
			component3.pivot = new Vector2(0.5f, 1f);
			component3.anchoredPosition = new Vector2(0f, -62f);
			component3.sizeDelta = new Vector2(-56f, 42f);
			HorizontalLayoutGroup component4 = gameObject2.GetComponent<HorizontalLayoutGroup>();
			component4.spacing = 8f;
			component4.childControlWidth = true;
			component4.childControlHeight = true;
			component4.childForceExpandWidth = true;
			component4.childForceExpandHeight = true;
			string[] array = new string[] { "AUDIO", "GAME", "GRAPHICS", "ADVANCED" };
			for (int i = 0; i < array.Length; i++)
			{
				int captured = i;
				ButtonManager buttonManager = Object.Instantiate<ButtonManager>(this.buttonPrefab, gameObject2.transform);
				buttonManager.gameObject.name = "Tab_" + array[i];
				buttonManager.buttonText = array[i];
				buttonManager.UpdateUI();
				buttonManager.onClick.AddListener(delegate
				{
					this.SelectTab(captured);
				});
				if (buttonManager.GetComponent<CanvasGroup>() == null)
				{
					buttonManager.gameObject.AddComponent<CanvasGroup>();
				}
				GraphicsSettingsRuntimeUI.NormalizeLayout(buttonManager.gameObject, 1f, -1f, 0f);
				GameObject gameObject3 = this.BuildScrollArea(this._panel.transform, "Content_" + array[i]);
				this._tabs.Add(new ValueTuple<ButtonManager, GameObject, string>(buttonManager, gameObject3, array[i]));
			}
			GameObject gameObject4 = new GameObject("Footer", new Type[]
			{
				typeof(RectTransform),
				typeof(HorizontalLayoutGroup)
			});
			gameObject4.transform.SetParent(this._panel.transform, false);
			RectTransform component5 = gameObject4.GetComponent<RectTransform>();
			component5.anchorMin = new Vector2(0f, 0f);
			component5.anchorMax = new Vector2(1f, 0f);
			component5.pivot = new Vector2(0.5f, 0f);
			component5.anchoredPosition = new Vector2(0f, 14f);
			component5.sizeDelta = new Vector2(-56f, 44f);
			HorizontalLayoutGroup component6 = gameObject4.GetComponent<HorizontalLayoutGroup>();
			component6.spacing = 10f;
			component6.childControlWidth = true;
			component6.childControlHeight = true;
			component6.childForceExpandWidth = true;
			component6.childForceExpandHeight = true;
			ButtonManager buttonManager2 = Object.Instantiate<ButtonManager>(this.buttonPrefab, gameObject4.transform);
			buttonManager2.buttonText = "RESET DEFAULTS";
			buttonManager2.UpdateUI();
			GraphicsSettingsRuntimeUI.NormalizeLayout(buttonManager2.gameObject, 1f, -1f, 0f);
			buttonManager2.onClick.AddListener(delegate
			{
				GraphicsSettingsController ctrl = this.Ctrl;
				if (ctrl != null)
				{
					ctrl.ResetToDefaults();
				}
				GameSettingsService.ResetAudioAndGameplay();
				this.RebuildAllTabs();
			});
			ButtonManager buttonManager3 = Object.Instantiate<ButtonManager>(this.buttonPrefab, gameObject4.transform);
			buttonManager3.buttonText = string.Format("CLOSE ({0})", this.toggleKey);
			buttonManager3.UpdateUI();
			buttonManager3.onClick.AddListener(delegate
			{
				this.SetOpen(false);
			});
			GraphicsSettingsRuntimeUI.NormalizeLayout(buttonManager3.gameObject, 1f, -1f, 0f);
			this.BuildConfirmBar();
		}

		// Token: 0x06004DAB RID: 19883 RVA: 0x001E2718 File Offset: 0x001E0918
		private GameObject BuildScrollArea(Transform parent, string name)
		{
			GameObject gameObject = new GameObject(name + "_Scroll", new Type[]
			{
				typeof(Image),
				typeof(ScrollRect)
			});
			gameObject.transform.SetParent(parent, false);
			gameObject.GetComponent<Image>().color = Color.clear;
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.anchorMin = new Vector2(0f, 0f);
			component.anchorMax = new Vector2(1f, 1f);
			component.offsetMin = new Vector2(28f, 70f);
			component.offsetMax = new Vector2(-28f, -112f);
			GameObject gameObject2 = new GameObject("Viewport", new Type[]
			{
				typeof(Image),
				typeof(Mask)
			});
			gameObject2.transform.SetParent(gameObject.transform, false);
			gameObject2.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.01f);
			gameObject2.GetComponent<Mask>().showMaskGraphic = false;
			RectTransform component2 = gameObject2.GetComponent<RectTransform>();
			GraphicsSettingsRuntimeUI.Stretch(component2);
			component2.pivot = new Vector2(0f, 1f);
			GameObject gameObject3 = new GameObject("Content", new Type[]
			{
				typeof(RectTransform),
				typeof(VerticalLayoutGroup),
				typeof(ContentSizeFitter)
			});
			gameObject3.transform.SetParent(gameObject2.transform, false);
			RectTransform component3 = gameObject3.GetComponent<RectTransform>();
			component3.anchorMin = new Vector2(0f, 1f);
			component3.anchorMax = new Vector2(1f, 1f);
			component3.pivot = new Vector2(0.5f, 1f);
			component3.offsetMin = Vector2.zero;
			component3.offsetMax = Vector2.zero;
			VerticalLayoutGroup component4 = gameObject3.GetComponent<VerticalLayoutGroup>();
			component4.spacing = 6f;
			component4.childControlWidth = true;
			component4.childControlHeight = true;
			component4.childForceExpandWidth = true;
			component4.childForceExpandHeight = false;
			gameObject3.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			ScrollRect component5 = gameObject.GetComponent<ScrollRect>();
			component5.viewport = component2;
			component5.content = component3;
			component5.horizontal = false;
			component5.movementType = ScrollRect.MovementType.Clamped;
			component5.scrollSensitivity = 25f;
			gameObject.SetActive(false);
			return gameObject;
		}

		// Token: 0x06004DAC RID: 19884 RVA: 0x001E296C File Offset: 0x001E0B6C
		private void BuildConfirmBar()
		{
			this._confirmBar = new GameObject("ConfirmBar", new Type[]
			{
				typeof(Image),
				typeof(HorizontalLayoutGroup)
			});
			this._confirmBar.transform.SetParent(this._panel.transform, false);
			this._confirmBar.GetComponent<Image>().color = new Color(0.9f, 0.6f, 0.1f, 0.18f);
			RectTransform component = this._confirmBar.GetComponent<RectTransform>();
			component.anchorMin = new Vector2(0f, 0f);
			component.anchorMax = new Vector2(1f, 0f);
			component.pivot = new Vector2(0.5f, 0f);
			component.anchoredPosition = new Vector2(0f, 66f);
			component.sizeDelta = new Vector2(-56f, 46f);
			HorizontalLayoutGroup component2 = this._confirmBar.GetComponent<HorizontalLayoutGroup>();
			component2.spacing = 10f;
			component2.padding = new RectOffset(12, 12, 5, 5);
			component2.childControlWidth = true;
			component2.childControlHeight = true;
			component2.childForceExpandWidth = false;
			component2.childForceExpandHeight = true;
			this._confirmText = GraphicsSettingsRuntimeUI.CreateText(this._confirmBar.transform, "", 17f, FontStyles.Normal, TextAlignmentOptions.Left);
			this._confirmText.gameObject.AddComponent<LayoutElement>().flexibleWidth = 1f;
			ButtonManager buttonManager = Object.Instantiate<ButtonManager>(this.buttonPrefab, this._confirmBar.transform);
			buttonManager.buttonText = "KEEP";
			buttonManager.UpdateUI();
			buttonManager.gameObject.AddComponent<LayoutElement>().preferredWidth = 140f;
			buttonManager.onClick.AddListener(delegate
			{
				this.HideConfirmBar(false);
			});
			ButtonManager buttonManager2 = Object.Instantiate<ButtonManager>(this.buttonPrefab, this._confirmBar.transform);
			buttonManager2.buttonText = "REVERT";
			buttonManager2.UpdateUI();
			buttonManager2.gameObject.AddComponent<LayoutElement>().preferredWidth = 140f;
			buttonManager2.onClick.AddListener(delegate
			{
				this.HideConfirmBar(true);
			});
			this._confirmBar.SetActive(false);
		}

		// Token: 0x06004DAD RID: 19885 RVA: 0x001E2B93 File Offset: 0x001E0D93
		private void ShowConfirmBar()
		{
			this._confirmBar.SetActive(true);
			if (this._confirmCountdown != null)
			{
				base.StopCoroutine(this._confirmCountdown);
			}
			this._confirmCountdown = base.StartCoroutine(this.ConfirmCountdown());
		}

		// Token: 0x06004DAE RID: 19886 RVA: 0x001E2BC7 File Offset: 0x001E0DC7
		private IEnumerator ConfirmCountdown()
		{
			int num;
			for (int s = 10; s > 0; s = num - 1)
			{
				this._confirmText.text = string.Format("Keep these display settings?  Reverting in {0}s...", s);
				yield return new WaitForSecondsRealtime(1f);
				num = s;
			}
			this.HideConfirmBar(true);
			yield break;
		}

		// Token: 0x06004DAF RID: 19887 RVA: 0x001E2BD8 File Offset: 0x001E0DD8
		private void HideConfirmBar(bool revert)
		{
			if (this._confirmCountdown != null)
			{
				base.StopCoroutine(this._confirmCountdown);
				this._confirmCountdown = null;
			}
			if (this._confirmBar == null || !this._confirmBar.activeSelf)
			{
				return;
			}
			this._confirmBar.SetActive(false);
			if (revert)
			{
				GraphicsSettingsController ctrl = this.Ctrl;
				if (ctrl != null)
				{
					ctrl.RevertDisplay();
				}
				this.RebuildTab(2);
				return;
			}
			GraphicsSettingsController ctrl2 = this.Ctrl;
			if (ctrl2 == null)
			{
				return;
			}
			ctrl2.CommitDisplay();
		}

		// Token: 0x06004DB0 RID: 19888 RVA: 0x001E2C58 File Offset: 0x001E0E58
		private void SelectTab(int index)
		{
			this._activeTab = Mathf.Clamp(index, 0, this._tabs.Count - 1);
			for (int i = 0; i < this._tabs.Count; i++)
			{
				this._tabs[i].Item2.SetActive(i == this._activeTab);
				CanvasGroup component = this._tabs[i].Item1.GetComponent<CanvasGroup>();
				if (component != null)
				{
					component.alpha = ((i == this._activeTab) ? 1f : 0.5f);
				}
			}
		}

		// Token: 0x06004DB1 RID: 19889 RVA: 0x001E2CF0 File Offset: 0x001E0EF0
		private void RebuildAllTabs()
		{
			for (int i = 0; i < this._tabs.Count; i++)
			{
				this.RebuildTab(i);
			}
		}

		// Token: 0x06004DB2 RID: 19890 RVA: 0x001E2D1C File Offset: 0x001E0F1C
		private void RebuildTab(int index)
		{
			if (this.Ctrl == null)
			{
				return;
			}
			this._suppress = true;
			try
			{
				Transform transform = this._tabs[index].Item2.GetComponentInChildren<VerticalLayoutGroup>(true).transform;
				for (int i = transform.childCount - 1; i >= 0; i--)
				{
					Object.Destroy(transform.GetChild(i).gameObject);
				}
				switch (index)
				{
				case 0:
					this.BuildAudioTab(transform);
					break;
				case 1:
					this.BuildGamePreferencesTab(transform);
					break;
				case 2:
					this.BuildGraphicsTab(transform);
					break;
				case 3:
					this.BuildAdvancedGraphicsTab(transform);
					break;
				}
				Debug.Log(string.Format("[GraphicsSettingsUI] Tab {0}: {1} elementi costruiti.", this._tabs[index].Item3, transform.childCount));
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
			finally
			{
				this._suppress = false;
			}
		}

		// Token: 0x06004DB3 RID: 19891 RVA: 0x001E2E14 File Offset: 0x001E1014
		private void BuildGraphicsTab(Transform parent)
		{
			GraphicsSettingsModel model = this.Ctrl.Model;
			this.AddSelector(parent, "Quality Preset", GameSettingsUiSchema.QualityLabels, this.Ctrl.CurrentQualityIndex, delegate(int index)
			{
				if (index < 0 || index > 3)
				{
					this.ScheduleRebuild(2);
					return;
				}
				this.Ctrl.SetQualityLevel((GraphicsSettingsSO.QualityLevel)Mathf.Clamp(index, 0, 3));
				this.ScheduleRebuild(2);
			});
			List<string> list = new List<string>();
			GameSettingsUiSchema.BuildResolutionOptions(this._resolutions, list);
			int num = ((model.resolutionWidth > 0) ? model.resolutionWidth : Screen.width);
			int num2 = ((model.resolutionHeight > 0) ? model.resolutionHeight : Screen.height);
			int num3 = GameSettingsUiSchema.FindResolutionIndex(this._resolutions, num, num2);
			this._pendingResolutionIndex = num3;
			this.AddSelector(parent, "Resolution", list.ToArray(), num3, delegate(int index)
			{
				this._pendingResolutionIndex = Mathf.Clamp(index, 0, this._resolutions.Count - 1);
			});
			this._pendingWindowModeIndex = GameSettingsUiSchema.ModeToIndex((FullScreenMode)model.fullScreenMode);
			this.AddSelector(parent, "Window Mode", GameSettingsUiSchema.WindowModeLabels, this._pendingWindowModeIndex, delegate(int index)
			{
				this._pendingWindowModeIndex = Mathf.Clamp(index, 0, GameSettingsUiSchema.WindowModeLabels.Length - 1);
			});
			this.AddActionButton(parent, "Display Changes", "APPLY DISPLAY", new Action(this.ApplyPendingDisplay));
			LocalizedGameText.BindSource(this.AddSlider(parent, "BRIGHTNESS", 50f, 200f, GraphicsSettingsModel.SanitizeBrightness(model.brightness) * 100f, true, delegate(float value)
			{
				this.Ctrl.UpdateModel(delegate(GraphicsSettingsModel x)
				{
					x.brightness = value / 100f;
				}, false);
			}, true).GetComponentInChildren<TMP_Text>(), "settings", "BRIGHTNESS", null);
			TextMeshProUGUI textMeshProUGUI = GraphicsSettingsRuntimeUI.CreateText(parent, "Adjust brightness to make dark areas easier to see. Default: 100%.", 17f, FontStyles.Normal, TextAlignmentOptions.TopLeft);
			LocalizedGameText.BindSource(textMeshProUGUI, "settings", textMeshProUGUI.text, null);
			textMeshProUGUI.gameObject.AddComponent<LayoutElement>().preferredHeight = 60f;
			SwitchManager vSyncControl = null;
			HorizontalSelector fpsControl = this.AddSelector(parent, "FPS Cap", GameSettingsUiSchema.FpsLabels, GameSettingsUiSchema.FindFpsIndex(model.fpsCap), delegate(int index)
			{
				this.Ctrl.SetTargetFrameRate(GameSettingsUiSchema.FpsValues[Mathf.Clamp(index, 0, GameSettingsUiSchema.FpsValues.Length - 1)]);
				if (vSyncControl != null)
				{
					vSyncControl.isOn = this.Ctrl.Model.vSync;
					if (vSyncControl.gameObject.activeInHierarchy)
					{
						vSyncControl.UpdateUI();
					}
				}
			});
			vSyncControl = this.AddSwitch(parent, "VSync", model.vSync, delegate(bool value)
			{
				this.Ctrl.SetVSync(value);
				fpsControl.index = GameSettingsUiSchema.FindFpsIndex(this.Ctrl.Model.fpsCap);
				fpsControl.UpdateUI();
			}, null);
		}

		// Token: 0x06004DB4 RID: 19892 RVA: 0x001E3018 File Offset: 0x001E1218
		private void BuildAdvancedGraphicsTab(Transform parent)
		{
			GraphicsSettingsModel model = this.Ctrl.Model;
			this.AddSelector(parent, "Map Lighting (Next Run)", GameSettingsUiSchema.MapLightingLabels, (int)model.mapLighting, delegate(int i)
			{
				this.QualityChanged(delegate(GraphicsSettingsModel x)
				{
					x.mapLighting = (GraphicsSettingsModel.MapLightingQuality)Mathf.Clamp(i, 0, 2);
				});
			});
			GraphicsSettingsRuntimeUI.CreateText(parent, "Map lighting applies to a new run. Return to the main menu and start a new run after changing it. Quality presets can also change this setting.", 17f, FontStyles.Normal, TextAlignmentOptions.TopLeft).gameObject.AddComponent<LayoutElement>().preferredHeight = 85f;
			this.AddSelector(parent, "Render Scale", GameSettingsUiSchema.RenderScaleLabels, GameSettingsUiSchema.FindClosest(GameSettingsUiSchema.RenderScales, model.renderScale), delegate(int index)
			{
				this.Ctrl.UpdateModel(delegate(GraphicsSettingsModel x)
				{
					x.renderScale = GameSettingsUiSchema.RenderScales[Mathf.Clamp(index, 0, GameSettingsUiSchema.RenderScales.Length - 1)];
				}, false);
			});
			this.AddSelector(parent, "Shadow Quality", GameSettingsUiSchema.ShadowQualityLabels, GameSettingsUiSchema.ShadowQualityIndex(model), delegate(int index)
			{
				this.QualityChanged(delegate(GraphicsSettingsModel x)
				{
					GameSettingsUiSchema.ApplyShadowQuality(x, index);
				});
			});
			this.AddSelector(parent, "View Distance", GameSettingsUiSchema.ViewDistanceLabels, GameSettingsUiSchema.FindClosest(GameSettingsUiSchema.ViewDistanceScales, model.viewDistanceScale), delegate(int index)
			{
				this.QualityChanged(delegate(GraphicsSettingsModel x)
				{
					x.viewDistanceScale = GameSettingsUiSchema.ViewDistanceScales[Mathf.Clamp(index, 0, GameSettingsUiSchema.ViewDistanceScales.Length - 1)];
				});
			});
			this.AddSelector(parent, "Texture Quality", GameSettingsUiSchema.TextureQualityLabels, Mathf.Clamp(model.textureQuality, 0, GameSettingsUiSchema.TextureQualityLabels.Length - 1), delegate(int index)
			{
				this.QualityChanged(delegate(GraphicsSettingsModel x)
				{
					x.textureQuality = Mathf.Clamp(index, 0, 3);
				});
			});
			this.AddSwitch(parent, "SSAO (Ambient Occlusion)", model.ssao, delegate(bool value)
			{
				this.QualityChanged(delegate(GraphicsSettingsModel x)
				{
					x.ssao = value;
				});
			}, null);
			this.AddSwitch(parent, "Post Processing", model.postProcessing, delegate(bool value)
			{
				this.QualityChanged(delegate(GraphicsSettingsModel x)
				{
					x.postProcessing = value;
				});
			}, null);
			this.AddSwitch(parent, "Magica Physics", model.magicaClothEnabled, delegate(bool value)
			{
				this.Ctrl.UpdateModel(delegate(GraphicsSettingsModel x)
				{
					x.magicaClothEnabled = value;
				}, false);
			}, null);
			this.AddSelector(parent, "Color Vision Filter", GameSettingsUiSchema.ColorVisionLabels, (int)model.colorVision, delegate(int index)
			{
				this.Ctrl.UpdateModel(delegate(GraphicsSettingsModel x)
				{
					x.colorVision = (GraphicsSettingsModel.ColorVisionMode)Mathf.Clamp(index, 0, 3);
				}, false);
			});
		}

		// Token: 0x06004DB5 RID: 19893 RVA: 0x001E31B8 File Offset: 0x001E13B8
		private void ApplyPendingDisplay()
		{
			if (this._resolutions.Count == 0)
			{
				return;
			}
			Vector2Int vector2Int = this._resolutions[Mathf.Clamp(this._pendingResolutionIndex, 0, this._resolutions.Count - 1)];
			this.Ctrl.ApplyDisplayWithBackup(vector2Int.x, vector2Int.y, GameSettingsUiSchema.IndexToMode(this._pendingWindowModeIndex));
			this.ShowConfirmBar();
		}

		// Token: 0x06004DB6 RID: 19894 RVA: 0x001E3224 File Offset: 0x001E1424
		private void BuildAudioTab(Transform parent)
		{
			this.AddSlider(parent, "Master", 0f, 1f, GameSettingsService.MasterVolume, false, new Action<float>(GameSettingsService.SetMasterVolume), false);
			this.AddSlider(parent, "Heartbeat", 0f, 1f, GameSettingsService.HeartbeatVolume, false, new Action<float>(GameSettingsService.SetHeartbeatVolume), false);
			this.AddSlider(parent, "Ciel Voice", 0f, 1f, GameSettingsService.CielVoiceVolume, false, new Action<float>(GameSettingsService.SetCielVoiceVolume), false);
			this.AddSlider(parent, "NPC Reactions", 0f, 1f, GameSettingsService.NpcReactionVolume, false, new Action<float>(GameSettingsService.SetNpcReactionVolume), false);
			this.AddSlider(parent, "Music", 0f, 1f, GameSettingsService.MusicVolume, false, new Action<float>(GameSettingsService.SetMusicVolume), false);
			this.AddSlider(parent, "SFX", 0f, 1f, GameSettingsService.SfxVolume, false, new Action<float>(GameSettingsService.SetSfxVolume), false);
			this.AddSwitch(parent, "Mute All", GameSettingsService.Muted, new Action<bool>(GameSettingsService.SetMuted), null);
		}

		// Token: 0x06004DB7 RID: 19895 RVA: 0x001E334C File Offset: 0x001E154C
		private void BuildGamePreferencesTab(Transform parent)
		{
			this.AddSlider(parent, "Mouse Sensitivity", 0.1f, 3f, GameSettingsService.LookSensitivity, false, new Action<float>(GameSettingsService.SetLookSensitivity), false);
			this.AddSwitch(parent, "INVERT MOUSE HORIZONTALLY", GameSettingsService.InvertMouseX, new Action<bool>(GameSettingsService.SetInvertMouseX), "settings.game.invert_mouse_x");
			this.AddSwitch(parent, "INVERT MOUSE VERTICALLY", GameSettingsService.InvertMouseY, new Action<bool>(GameSettingsService.SetInvertMouseY), "settings.game.invert_mouse_y");
			this.AddSwitch(parent, "Ciel Voice Subtitles", GameSettingsService.CielVoiceSubtitles, new Action<bool>(GameSettingsService.SetCielVoiceSubtitles), null);
			this.AddSwitch(parent, "Camera Shake (Subway / Events)", GameSettingsService.CameraShake, new Action<bool>(GameSettingsService.SetCameraShake), null);
			this.AddSwitch(parent, "Mud from Trashcans / Dumpsters", GameSettingsService.GarbageMudBodyPaint, new Action<bool>(GameSettingsService.SetGarbageMudBodyPaint), null);
			this.AddSwitch(parent, "Streamer Mode (SFW)", GameSettingsService.StreamerMode, new Action<bool>(GameSettingsService.SetStreamerMode), null);
			this.AddSlider(parent, "Streamer Pixel Size", 12f, 80f, (float)GameSettingsService.StreamerPixelSize, true, new Action<float>(GameSettingsService.SetStreamerPixelSize), false);
			this.AddSwitch(parent, "Show FPS on HUD", GameSettingsService.ShowFps, new Action<bool>(GameSettingsService.SetShowFps), null);
			this.AddSelector(parent, "Phone Video Resolution", GameSettingsUiSchema.PhoneVideoResolutionLabels, GameSettingsUiSchema.FindClosest(GameSettingsUiSchema.PhoneVideoHeights, GameSettingsService.PhoneVideoHeight), delegate(int i)
			{
				GameSettingsService.SetPhoneVideoHeight(GameSettingsUiSchema.PhoneVideoHeights[Mathf.Clamp(i, 0, GameSettingsUiSchema.PhoneVideoHeights.Length - 1)]);
			});
			this.AddSelector(parent, "Phone Video FPS", GameSettingsUiSchema.PhoneVideoFpsLabels, GameSettingsUiSchema.FindClosest(GameSettingsUiSchema.PhoneVideoFpsValues, GameSettingsService.PhoneVideoFps), delegate(int i)
			{
				GameSettingsService.SetPhoneVideoFps(GameSettingsUiSchema.PhoneVideoFpsValues[Mathf.Clamp(i, 0, GameSettingsUiSchema.PhoneVideoFpsValues.Length - 1)]);
			});
		}

		// Token: 0x06004DB8 RID: 19896 RVA: 0x001E3510 File Offset: 0x001E1710
		private void QualityChanged(Action<GraphicsSettingsModel> mutate)
		{
			this.Ctrl.UpdateModel(mutate, false);
			if (this._titleInfo != null)
			{
				this._titleInfo.text = string.Format("{0}   |   {1} FPS", this.Ctrl.CurrentPresetName, Mathf.RoundToInt(this._fpsSmoothed));
			}
		}

		// Token: 0x06004DB9 RID: 19897 RVA: 0x001E3568 File Offset: 0x001E1768
		private void ScheduleRebuild(int tabIndex)
		{
			base.StartCoroutine(this.RebuildNextFrame(tabIndex));
		}

		// Token: 0x06004DBA RID: 19898 RVA: 0x001E3578 File Offset: 0x001E1778
		private IEnumerator RebuildNextFrame(int tabIndex)
		{
			yield return null;
			this.RebuildTab(tabIndex);
			yield break;
		}

		// Token: 0x06004DBB RID: 19899 RVA: 0x001E3590 File Offset: 0x001E1790
		private Transform AddRow(Transform parent, string label)
		{
			GameObject gameObject = new GameObject("Row_" + label, new Type[]
			{
				typeof(Image),
				typeof(HorizontalLayoutGroup),
				typeof(LayoutElement)
			});
			gameObject.transform.SetParent(parent, false);
			gameObject.GetComponent<Image>().color = GraphicsSettingsRuntimeUI.RowColor;
			LayoutElement component = gameObject.GetComponent<LayoutElement>();
			component.preferredHeight = 56f;
			component.minHeight = 56f;
			HorizontalLayoutGroup component2 = gameObject.GetComponent<HorizontalLayoutGroup>();
			component2.padding = new RectOffset(18, 18, 6, 6);
			component2.spacing = 16f;
			component2.childAlignment = TextAnchor.MiddleLeft;
			component2.childControlWidth = true;
			component2.childControlHeight = true;
			component2.childForceExpandWidth = false;
			component2.childForceExpandHeight = false;
			LayoutElement layoutElement = GraphicsSettingsRuntimeUI.CreateText(gameObject.transform, label, 19f, FontStyles.Normal, TextAlignmentOptions.MidlineLeft).gameObject.AddComponent<LayoutElement>();
			layoutElement.flexibleWidth = 1f;
			layoutElement.minWidth = 260f;
			layoutElement.preferredHeight = 44f;
			return gameObject.transform;
		}

		// Token: 0x06004DBC RID: 19900 RVA: 0x001E369C File Offset: 0x001E189C
		private GameObject WrapControl(Transform row, float width, float height)
		{
			GameObject gameObject = new GameObject("Control", new Type[]
			{
				typeof(RectTransform),
				typeof(LayoutElement)
			});
			gameObject.transform.SetParent(row, false);
			LayoutElement component = gameObject.GetComponent<LayoutElement>();
			component.preferredWidth = width;
			component.minWidth = width;
			component.preferredHeight = height;
			return gameObject;
		}

		// Token: 0x06004DBD RID: 19901 RVA: 0x001E36FC File Offset: 0x001E18FC
		private void AddActionButton(Transform parent, string label, string buttonText, Action onClick)
		{
			Transform transform = this.AddRow(parent, label);
			GameObject gameObject = this.WrapControl(transform, 360f, 44f);
			ButtonManager buttonManager = Object.Instantiate<ButtonManager>(this.buttonPrefab, gameObject.transform);
			buttonManager.buttonText = buttonText;
			buttonManager.UpdateUI();
			GraphicsSettingsRuntimeUI.Stretch((RectTransform)buttonManager.transform);
			GraphicsSettingsRuntimeUI.NormalizeLayout(buttonManager.gameObject, 0f, -1f, 0f);
			buttonManager.onClick.AddListener(delegate
			{
				Action onClick2 = onClick;
				if (onClick2 == null)
				{
					return;
				}
				onClick2();
			});
		}

		// Token: 0x06004DBE RID: 19902 RVA: 0x001E3790 File Offset: 0x001E1990
		private HorizontalSelector AddSelector(Transform parent, string label, string[] options, int index, Action<int> onChange)
		{
			Transform transform = this.AddRow(parent, label);
			GameObject gameObject = this.WrapControl(transform, 360f, 44f);
			HorizontalSelector horizontalSelector = Object.Instantiate<HorizontalSelector>(this.selectorPrefab, gameObject.transform);
			GraphicsSettingsRuntimeUI.Stretch((RectTransform)horizontalSelector.transform);
			horizontalSelector.saveSelected = false;
			horizontalSelector.invokeAtStart = false;
			horizontalSelector.loopSelection = true;
			horizontalSelector.enableIcon = false;
			horizontalSelector.enableIndicators = false;
			horizontalSelector.items.Clear();
			foreach (string text in options)
			{
				horizontalSelector.items.Add(new HorizontalSelector.Item
				{
					itemTitle = text
				});
			}
			horizontalSelector.defaultIndex = Mathf.Clamp(index, 0, options.Length - 1);
			horizontalSelector.SetupSelector();
			horizontalSelector.UpdateContentLayout();
			horizontalSelector.onValueChanged.AddListener(delegate(int i)
			{
				if (!this._suppress)
				{
					Action<int> onChange2 = onChange;
					if (onChange2 == null)
					{
						return;
					}
					onChange2(i);
				}
			});
			return horizontalSelector;
		}

		// Token: 0x06004DBF RID: 19903 RVA: 0x001E388C File Offset: 0x001E1A8C
		private SwitchManager AddSwitch(Transform parent, string label, bool value, Action<bool> onChange, string localizationKey = null)
		{
			Transform transform = this.AddRow(parent, label);
			if (localizationKey != null)
			{
				LocalizedGameText.Bind(transform.Find("Text").GetComponent<TMP_Text>(), localizationKey, label, null);
			}
			GameObject gameObject = this.WrapControl(transform, 84f, 40f);
			SwitchManager switchManager = Object.Instantiate<SwitchManager>(this.switchPrefab, gameObject.transform);
			GraphicsSettingsRuntimeUI.Stretch((RectTransform)switchManager.transform);
			switchManager.saveValue = false;
			switchManager.invokeAtStart = false;
			switchManager.isOn = value;
			if (switchManager.gameObject.activeInHierarchy)
			{
				switchManager.UpdateUI();
			}
			switchManager.onValueChanged.AddListener(delegate(bool v)
			{
				if (!this._suppress)
				{
					Action<bool> onChange2 = onChange;
					if (onChange2 == null)
					{
						return;
					}
					onChange2(v);
				}
			});
			return switchManager;
		}

		// Token: 0x06004DC0 RID: 19904 RVA: 0x001E394C File Offset: 0x001E1B4C
		private Transform AddSlider(Transform parent, string label, float min, float max, float value, bool whole, Action<float> onChange, bool percent = false)
		{
			Transform transform = this.AddRow(parent, label);
			GameObject gameObject = this.WrapControl(transform, 360f, 36f);
			SliderManager sliderManager = Object.Instantiate<SliderManager>(this.sliderPrefab, gameObject.transform);
			RectTransform rectTransform = (RectTransform)sliderManager.transform;
			GraphicsSettingsRuntimeUI.Stretch(rectTransform);
			rectTransform.offsetMax = new Vector2(-64f, 0f);
			sliderManager.enableSaving = false;
			sliderManager.invokeOnAwake = false;
			sliderManager.useRoundValue = whole;
			if (percent)
			{
				sliderManager.usePercent = true;
			}
			sliderManager.mainSlider.wholeNumbers = whole;
			sliderManager.mainSlider.minValue = min;
			sliderManager.mainSlider.maxValue = max;
			sliderManager.mainSlider.value = value;
			sliderManager.UpdateUI();
			sliderManager.mainSlider.onValueChanged.AddListener(delegate(float v)
			{
				if (!this._suppress)
				{
					Action<float> onChange2 = onChange;
					if (onChange2 == null)
					{
						return;
					}
					onChange2(v);
				}
			});
			return transform;
		}

		// Token: 0x06004DC1 RID: 19905 RVA: 0x001E3A37 File Offset: 0x001E1C37
		private static Image CreateImage(Transform parent, string name, Color color)
		{
			GameObject gameObject = new GameObject(name, new Type[] { typeof(Image) });
			gameObject.transform.SetParent(parent, false);
			Image component = gameObject.GetComponent<Image>();
			component.color = color;
			return component;
		}

		// Token: 0x06004DC2 RID: 19906 RVA: 0x001E3A6C File Offset: 0x001E1C6C
		private static TextMeshProUGUI CreateText(Transform parent, string text, float size, FontStyles style, TextAlignmentOptions align)
		{
			GameObject gameObject = new GameObject("Text", new Type[] { typeof(TextMeshProUGUI) });
			gameObject.transform.SetParent(parent, false);
			TextMeshProUGUI component = gameObject.GetComponent<TextMeshProUGUI>();
			component.text = text;
			component.fontSize = size;
			component.fontStyle = style;
			component.alignment = align;
			component.color = Color.white;
			component.enableWordWrapping = true;
			component.overflowMode = TextOverflowModes.Ellipsis;
			return component;
		}

		// Token: 0x06004DC3 RID: 19907 RVA: 0x001E3AE0 File Offset: 0x001E1CE0
		private static void NormalizeLayout(GameObject go, float flexible = 0f, float preferred = -1f, float min = 0f)
		{
			LayoutElement layoutElement = go.GetComponent<LayoutElement>();
			if (layoutElement == null)
			{
				layoutElement = go.AddComponent<LayoutElement>();
			}
			layoutElement.minWidth = min;
			layoutElement.preferredWidth = preferred;
			layoutElement.flexibleWidth = flexible;
		}

		// Token: 0x06004DC4 RID: 19908 RVA: 0x001A38AD File Offset: 0x001A1AAD
		private static void Stretch(RectTransform rt)
		{
			rt.anchorMin = Vector2.zero;
			rt.anchorMax = Vector2.one;
			rt.offsetMin = Vector2.zero;
			rt.offsetMax = Vector2.zero;
		}

		// Token: 0x06004DC5 RID: 19909 RVA: 0x001E3B19 File Offset: 0x001E1D19
		private static void EnsureEventSystem()
		{
			if (Object.FindObjectOfType<EventSystem>() != null)
			{
				return;
			}
			Object.DontDestroyOnLoad(new GameObject("EventSystem", new Type[]
			{
				typeof(EventSystem),
				typeof(StandaloneInputModule)
			}));
		}

		// Token: 0x04004F15 RID: 20245
		private static int _toggleInputFrame = -1;

		// Token: 0x04004F17 RID: 20247
		[Header("References")]
		[Tooltip("Se null, viene usato GraphicsSettingsController.Instance")]
		[SerializeField]
		private GraphicsSettingsController controller;

		// Token: 0x04004F18 RID: 20248
		[Header("MUIP Prefabs")]
		[SerializeField]
		private HorizontalSelector selectorPrefab;

		// Token: 0x04004F19 RID: 20249
		[SerializeField]
		private SwitchManager switchPrefab;

		// Token: 0x04004F1A RID: 20250
		[SerializeField]
		private SliderManager sliderPrefab;

		// Token: 0x04004F1B RID: 20251
		[SerializeField]
		private ButtonManager buttonPrefab;

		// Token: 0x04004F1C RID: 20252
		[Header("Toggle Menu")]
		[SerializeField]
		private KeyCode toggleKey = KeyCode.Escape;

		// Token: 0x04004F1D RID: 20253
		[SerializeField]
		private bool pauseGameWhenOpen = true;

		// Token: 0x04004F1E RID: 20254
		[SerializeField]
		private bool overrideCursorWhenOpen = true;

		// Token: 0x04004F1F RID: 20255
		[Header("Style")]
		[SerializeField]
		private int sortOrder = 1000;

		// Token: 0x04004F20 RID: 20256
		[SerializeField]
		private TMP_FontAsset premiumFont;

		// Token: 0x04004F21 RID: 20257
		private static readonly Vector2 PanelSize = new Vector2(1140f, 780f);

		// Token: 0x04004F22 RID: 20258
		private static readonly Color PanelColor = new Color(0.06f, 0.07f, 0.1f, 0.97f);

		// Token: 0x04004F23 RID: 20259
		private static readonly Color RowColor = new Color(1f, 1f, 1f, 0.035f);

		// Token: 0x04004F24 RID: 20260
		private const float RowHeight = 56f;

		// Token: 0x04004F25 RID: 20261
		private const float ControlWidth = 360f;

		// Token: 0x04004F26 RID: 20262
		private const float LabelMinWidth = 260f;

		// Token: 0x04004F27 RID: 20263
		private Canvas _canvas;

		// Token: 0x04004F28 RID: 20264
		private GameObject _panel;

		// Token: 0x04004F29 RID: 20265
		private TextMeshProUGUI _titleInfo;

		// Token: 0x04004F2A RID: 20266
		private GameObject _confirmBar;

		// Token: 0x04004F2B RID: 20267
		private TextMeshProUGUI _confirmText;

		// Token: 0x04004F2C RID: 20268
		private Coroutine _confirmCountdown;

		// Token: 0x04004F2D RID: 20269
		private MainMenuSettingsPanel _premiumSettingsPanel;

		// Token: 0x04004F2E RID: 20270
		[TupleElementNames(new string[] { "button", "content", "title" })]
		private readonly List<ValueTuple<ButtonManager, GameObject, string>> _tabs = new List<ValueTuple<ButtonManager, GameObject, string>>();

		// Token: 0x04004F2F RID: 20271
		private int _activeTab;

		// Token: 0x04004F30 RID: 20272
		private bool _isOpen;

		// Token: 0x04004F31 RID: 20273
		private bool _suppress;

		// Token: 0x04004F32 RID: 20274
		private bool _returningToMainMenu;

		// Token: 0x04004F33 RID: 20275
		private float _previousTimeScale = 1f;

		// Token: 0x04004F34 RID: 20276
		private IDisposable _audioPause;

		// Token: 0x04004F35 RID: 20277
		private CursorLockMode _previousCursorLock;

		// Token: 0x04004F36 RID: 20278
		private bool _previousCursorVisible;

		// Token: 0x04004F37 RID: 20279
		private float _fpsSmoothed;

		// Token: 0x04004F38 RID: 20280
		private float _fpsTimer;

		// Token: 0x04004F39 RID: 20281
		private readonly List<Vector2Int> _resolutions = new List<Vector2Int>();

		// Token: 0x04004F3A RID: 20282
		private readonly SceneUiSuppressionScope _sceneUiSuppression = new SceneUiSuppressionScope();

		// Token: 0x04004F3B RID: 20283
		private int _pendingResolutionIndex;

		// Token: 0x04004F3C RID: 20284
		private int _pendingWindowModeIndex;
	}
}
