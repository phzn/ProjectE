using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Features.DebugTools;
using Features.Detection;
using Features.Localization;
using Features.Rank;
using Features.RenderingPrecision;
using Features.RunModes;
using Invector.vCharacterController;
using MoreMountains.Feedbacks;
using Pathfinding;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.Rendering;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.Diagnostics;
using ProjectExhibitionist.Minimap;
using TMPro;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Features.Loading
{
	// Token: 0x020010D8 RID: 4312
	[DisallowMultipleComponent]
	public class LoadingScreenController : MonoBehaviour
	{
		// Token: 0x0600829F RID: 33439 RVA: 0x00355777 File Offset: 0x00353977
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetRenderWarmup()
		{
			LoadingScreenController._cityRenderWarmed = false;
		}

		// Token: 0x170014B4 RID: 5300
		// (get) Token: 0x060082A0 RID: 33440 RVA: 0x0035577F File Offset: 0x0035397F
		public static LoadingScreenController ExistingInstance
		{
			get
			{
				return LoadingScreenController._instance;
			}
		}

		// Token: 0x170014B5 RID: 5301
		// (get) Token: 0x060082A1 RID: 33441 RVA: 0x00355788 File Offset: 0x00353988
		public static LoadingScreenController Instance
		{
			get
			{
				if (LoadingScreenController._instance != null)
				{
					return LoadingScreenController._instance;
				}
				LoadingScreenController._instance = Object.FindObjectOfType<LoadingScreenController>();
				if (LoadingScreenController._instance != null)
				{
					return LoadingScreenController._instance;
				}
				GameObject gameObject = Resources.Load<GameObject>("LoadingScreen");
				if (gameObject == null)
				{
					Debug.LogError("[LoadingScreen] Prefab non trovato in Resources/'LoadingScreen'. Assicurati che LoadingScreen.prefab sia sotto una cartella Resources.");
					return null;
				}
				GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject);
				gameObject2.name = gameObject.name;
				if (!(LoadingScreenController._instance != null))
				{
					return gameObject2.GetComponentInChildren<LoadingScreenController>(true);
				}
				return LoadingScreenController._instance;
			}
		}

		// Token: 0x170014B6 RID: 5302
		// (get) Token: 0x060082A2 RID: 33442 RVA: 0x00355812 File Offset: 0x00353A12
		// (set) Token: 0x060082A3 RID: 33443 RVA: 0x0035581A File Offset: 0x00353A1A
		public bool IsManualLoadActive { get; private set; }

		// Token: 0x170014B7 RID: 5303
		// (get) Token: 0x060082A4 RID: 33444 RVA: 0x00355823 File Offset: 0x00353A23
		// (set) Token: 0x060082A5 RID: 33445 RVA: 0x0035582B File Offset: 0x00353A2B
		public bool IsLoadingActive { get; private set; }

		// Token: 0x170014B8 RID: 5304
		// (get) Token: 0x060082A6 RID: 33446 RVA: 0x00355834 File Offset: 0x00353A34
		// (set) Token: 0x060082A7 RID: 33447 RVA: 0x0035583C File Offset: 0x00353A3C
		public bool IsPreparingReveal { get; private set; }

		// Token: 0x170014B9 RID: 5305
		// (get) Token: 0x060082A8 RID: 33448 RVA: 0x00355845 File Offset: 0x00353A45
		// (set) Token: 0x060082A9 RID: 33449 RVA: 0x0035584D File Offset: 0x00353A4D
		public bool HasLoadFailure { get; private set; }

		// Token: 0x170014BA RID: 5306
		// (get) Token: 0x060082AA RID: 33450 RVA: 0x00355856 File Offset: 0x00353A56
		// (set) Token: 0x060082AB RID: 33451 RVA: 0x0035585E File Offset: 0x00353A5E
		public bool IsSlowLoadPromptVisible { get; private set; }

		// Token: 0x060082AC RID: 33452 RVA: 0x00355868 File Offset: 0x00353A68
		private void Update()
		{
			if (!this.IsLoadingActive)
			{
				return;
			}
			this.EnsurePlayerInputLocked();
			if (this._leavingSlowLoad)
			{
				if (Time.frameCount - this._leaveRequestedFrame >= 2)
				{
					this._leavingSlowLoad = false;
					this.ReturnToMenuAfterLoadFailure();
				}
				return;
			}
			if (this.HasLoadFailure)
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
				if (this.ReadPromptChoice(1) == 0)
				{
					this.ReturnToMenuAfterLoadFailure();
				}
				return;
			}
			this.UpdateSlowLoadPrompt();
			float num = ((this.progressLerpSpeed > 0f) ? Mathf.MoveTowards(this._displayedProgress, this._targetProgress, Time.unscaledDeltaTime * this.progressLerpSpeed) : this._targetProgress);
			if (Mathf.Approximately(num, this._displayedProgress))
			{
				return;
			}
			this._displayedProgress = num;
			this.UpdateProgressVisuals(this._displayedProgress);
		}

		// Token: 0x060082AD RID: 33453 RVA: 0x00355928 File Offset: 0x00353B28
		private void Awake()
		{
			if (!(LoadingScreenController._instance != null) || !(LoadingScreenController._instance != this))
			{
				LoadingScreenController._instance = this;
				if (this.progressLabel != null)
				{
					LocalizedGameText.BindFontOnly(this.progressLabel);
				}
				this.InitializeProgressMaterial();
				if (this.persistAcrossScenes)
				{
					Canvas componentInParent = base.GetComponentInParent<Canvas>();
					if (componentInParent != null)
					{
						Transform transform = componentInParent.transform;
						if (transform.parent != null)
						{
							transform.SetParent(null, false);
						}
						Object.DontDestroyOnLoad(transform.gameObject);
					}
					else
					{
						if (base.transform.parent != null)
						{
							base.transform.SetParent(null, false);
						}
						Object.DontDestroyOnLoad(base.gameObject);
					}
				}
				this.HideImmediate();
				return;
			}
			Canvas componentInParent2 = base.GetComponentInParent<Canvas>();
			if (componentInParent2 != null)
			{
				Object.Destroy(componentInParent2.gameObject);
				return;
			}
			Object.Destroy(base.gameObject);
		}

		// Token: 0x060082AE RID: 33454 RVA: 0x00355A10 File Offset: 0x00353C10
		private void OnDestroy()
		{
			this.HideSlowLoadPrompt();
			this.ClearLoadFailure();
			this.RestorePlayerInput();
			this.RestoreGameplayAudio();
			this.RestoreLoadPriority();
			if (this._progressMaterial != null)
			{
				Object.Destroy(this._progressMaterial);
			}
			if (LoadingScreenController._instance == this)
			{
				LoadingScreenController._instance = null;
			}
		}

		// Token: 0x060082AF RID: 33455 RVA: 0x00355A68 File Offset: 0x00353C68
		private void InitializeProgressMaterial()
		{
			if (this.progressFill == null || this.progressFill.material == null || !this.progressFill.material.HasProperty(LoadingScreenController.ProgressFillAmount))
			{
				return;
			}
			this._progressMaterial = new Material(this.progressFill.material)
			{
				name = this.progressFill.material.name + " (Instance)"
			};
			this._progressMaterial.SetFloat(LoadingScreenController.ProgressFillAmount, 0f);
			this.progressFill.material = this._progressMaterial;
			this.progressFill.type = Image.Type.Simple;
			this.progressFill.fillAmount = 1f;
			this.progressFill.color = Color.white;
		}

		// Token: 0x060082B0 RID: 33456 RVA: 0x00355B38 File Offset: 0x00353D38
		public void BeginManualLoad()
		{
			if (this.IsManualLoadActive)
			{
				return;
			}
			this.ClearLoadFailure();
			this.ResetSlowLoadState();
			this._recoveryLoadPending = false;
			FlyCamera.StopForContextChange();
			this.CancelManualCompletion();
			this.IsManualLoadActive = true;
			this.IsLoadingActive = true;
			this.BoostLoadPriority();
			this.EnsureActiveForCoroutine();
			this.RandomizeOnce();
			this.ShowImmediate();
			this._displayedProgress = 0f;
			this._targetProgress = 0f;
			this.UpdateProgressVisuals(0f);
			this.PauseGameplayAudio();
			this.EnsurePlayerInputLocked();
			if (this.fadeInFeedbacks != null)
			{
				this.fadeInFeedbacks.PlayFeedbacks();
			}
		}

		// Token: 0x060082B1 RID: 33457 RVA: 0x00355BD8 File Offset: 0x00353DD8
		public void ReportProgress(float normalized01)
		{
			this._targetProgress = Mathf.Max(this._targetProgress, Mathf.Clamp01(normalized01));
		}

		// Token: 0x060082B2 RID: 33458 RVA: 0x00355BF4 File Offset: 0x00353DF4
		public void EndManualLoad()
		{
			if (this.HasLoadFailure || GameSystemsBootstrapper.GameplayReadinessFailed)
			{
				this.ShowLoadFailure();
				return;
			}
			if (!this.IsManualLoadActive)
			{
				return;
			}
			this.IsManualLoadActive = false;
			if (!base.gameObject.activeInHierarchy)
			{
				this.HideImmediate();
				return;
			}
			this.IsPreparingReveal = true;
			this._manualEndRoutine = base.StartCoroutine(this.EndManualLoadRoutine());
		}

		// Token: 0x060082B3 RID: 33459 RVA: 0x00355C54 File Offset: 0x00353E54
		public void ForceHideImmediate()
		{
			this.ClearLoadFailure();
			this.CancelManualCompletion();
			this.IsManualLoadActive = false;
			this.HideImmediate();
		}

		// Token: 0x060082B4 RID: 33460 RVA: 0x00355C70 File Offset: 0x00353E70
		public void ShowLoadFailure()
		{
			if (this.HasLoadFailure || this._recoveryLoadPending)
			{
				return;
			}
			this.HideSlowLoadPrompt();
			this._promptSelection = 0;
			this._promptInputReadyAt = Time.unscaledTime + 0.5f;
			this.CancelManualCompletion();
			this.EnsureActiveForCoroutine();
			this.HasLoadFailure = true;
			this.IsLoadingActive = true;
			this._failureCursorLock = Cursor.lockState;
			this._failureCursorVisible = Cursor.visible;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			if (this.fadeInFeedbacks != null)
			{
				this.fadeInFeedbacks.StopFeedbacks();
			}
			this.ShowImmediate();
			this.PauseGameplayAudio();
			this.EnsurePlayerInputLocked();
			this.RestoreLoadPriority();
		}

		// Token: 0x060082B5 RID: 33461 RVA: 0x00355D19 File Offset: 0x00353F19
		private void ClearLoadFailure()
		{
			if (!this.HasLoadFailure)
			{
				return;
			}
			this.HasLoadFailure = false;
			Cursor.lockState = this._failureCursorLock;
			Cursor.visible = this._failureCursorVisible;
		}

		// Token: 0x060082B6 RID: 33462 RVA: 0x00355D44 File Offset: 0x00353F44
		private void OnGUI()
		{
			if (this._leavingSlowLoad)
			{
				return;
			}
			if (this.HasLoadFailure)
			{
				if (this.DrawPrompt(GameLocalization.Get("loading.failed", "Unable to finish loading. Return to the main menu to try again."), new string[] { GameLocalization.Get("loading.return_menu", "Return to Main Menu") }) == 0)
				{
					this.ReturnToMenuAfterLoadFailure();
					return;
				}
			}
			else if (this.IsSlowLoadPromptVisible)
			{
				int num = this.DrawPrompt(GameLocalization.Get("loading.slow", "Loading is taking longer than usual. You can keep waiting or return to the main menu."), new string[]
				{
					GameLocalization.Get("loading.wait", "Keep Waiting"),
					GameLocalization.Get("loading.return_menu", "Return to Main Menu")
				});
				if (num == 0)
				{
					this.KeepWaitingForSlowLoad();
					return;
				}
				if (num == 1)
				{
					this.LeaveSlowLoadToMenu();
				}
			}
		}

		// Token: 0x060082B7 RID: 33463 RVA: 0x00355DF4 File Offset: 0x00353FF4
		private int DrawPrompt(string message, params string[] choices)
		{
			int depth = GUI.depth;
			Color backgroundColor = GUI.backgroundColor;
			GUI.depth = -10000;
			Font font = this.ResolvePromptFont();
			float num = Mathf.Min(560f, (float)Screen.width - 32f);
			float num2 = ((choices.Length > 1) ? 210f : 190f);
			Rect rect = new Rect(((float)Screen.width - num) * 0.5f, ((float)Screen.height - num2) * 0.5f, num, num2);
			GUI.Box(rect, GUIContent.none);
			GUIStyle guistyle = new GUIStyle(GUI.skin.label)
			{
				alignment = TextAnchor.MiddleCenter,
				wordWrap = true,
				fontSize = 20,
				font = font
			};
			GUI.Label(new Rect(rect.x + 20f, rect.y + 20f, num - 40f, num2 - 105f), message, guistyle);
			int num3 = -1;
			float num4 = (num - 80f - 16f * (float)(choices.Length - 1)) / (float)choices.Length;
			for (int i = 0; i < choices.Length; i++)
			{
				bool flag = i == this._promptSelection;
				GUIStyle guistyle2 = new GUIStyle(GUI.skin.button)
				{
					fontSize = 18,
					font = font,
					fontStyle = (flag ? FontStyle.Bold : FontStyle.Normal)
				};
				GUI.backgroundColor = (flag ? new Color(0.72f, 0.5f, 1f) : backgroundColor);
				if (GUI.Button(new Rect(rect.x + 40f + (float)i * (num4 + 16f), rect.yMax - 68f, num4, 44f), choices[i], guistyle2))
				{
					num3 = i;
				}
			}
			GUI.backgroundColor = backgroundColor;
			GUI.depth = depth;
			return num3;
		}

		// Token: 0x060082B8 RID: 33464 RVA: 0x00355FC4 File Offset: 0x003541C4
		private Font ResolvePromptFont()
		{
			string currentCode = GameLocalization.CurrentCode;
			if (this._promptFontCode == currentCode)
			{
				return this._promptFont;
			}
			this._promptFontCode = currentCode;
			TMP_FontAsset font = GameLocalization.GetFont(null);
			this._promptFont = ((font != null) ? font.sourceFontFile : null);
			return this._promptFont;
		}

		// Token: 0x060082B9 RID: 33465 RVA: 0x00356018 File Offset: 0x00354218
		private void UpdateSlowLoadPrompt()
		{
			if (!LoadingStallMonitor.IsStalled)
			{
				this._slowLoadPromptHiddenUntil = 0.0;
				this.HideSlowLoadPrompt();
				return;
			}
			if (Time.realtimeSinceStartupAsDouble < this._slowLoadPromptHiddenUntil)
			{
				this.HideSlowLoadPrompt();
				return;
			}
			this.ShowSlowLoadPrompt();
			int num = this.ReadPromptChoice(2);
			if (num == 0)
			{
				this.KeepWaitingForSlowLoad();
				return;
			}
			if (num == 1)
			{
				this.LeaveSlowLoadToMenu();
			}
		}

		// Token: 0x060082BA RID: 33466 RVA: 0x00356078 File Offset: 0x00354278
		private void ShowSlowLoadPrompt()
		{
			if (!this.IsSlowLoadPromptVisible)
			{
				this.IsSlowLoadPromptVisible = true;
				this._promptCursorLock = Cursor.lockState;
				this._promptCursorVisible = Cursor.visible;
				this._promptSelection = 0;
				this._promptInputReadyAt = Time.unscaledTime + 0.5f;
				Debug.LogWarning("[LoadingScreen] Loading is slow; offering to keep waiting or return to the main menu. " + LoadingStallMonitor.LastDiagnostic);
			}
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}

		// Token: 0x060082BB RID: 33467 RVA: 0x003560E2 File Offset: 0x003542E2
		private void HideSlowLoadPrompt()
		{
			if (!this.IsSlowLoadPromptVisible)
			{
				return;
			}
			this.IsSlowLoadPromptVisible = false;
			Cursor.lockState = this._promptCursorLock;
			Cursor.visible = this._promptCursorVisible;
		}

		// Token: 0x060082BC RID: 33468 RVA: 0x0035610A File Offset: 0x0035430A
		private void ResetSlowLoadState()
		{
			this.HideSlowLoadPrompt();
			this._slowLoadPromptHiddenUntil = 0.0;
			this._leavingSlowLoad = false;
			LoadingStallMonitor.Clear();
		}

		// Token: 0x060082BD RID: 33469 RVA: 0x0035612D File Offset: 0x0035432D
		public void KeepWaitingForSlowLoad()
		{
			if (!this.IsSlowLoadPromptVisible)
			{
				return;
			}
			this._slowLoadPromptHiddenUntil = Time.realtimeSinceStartupAsDouble + (double)this.slowLoadPromptSnoozeSeconds;
			this.HideSlowLoadPrompt();
			Debug.Log("[LoadingScreen] The player chose to keep waiting for a slow load.");
		}

		// Token: 0x060082BE RID: 33470 RVA: 0x0035615C File Offset: 0x0035435C
		public void LeaveSlowLoadToMenu()
		{
			if (!this.IsLoadingActive || this.HasLoadFailure || this._leavingSlowLoad)
			{
				return;
			}
			string lastDiagnostic = LoadingStallMonitor.LastDiagnostic;
			this.HideSlowLoadPrompt();
			GameSystemsBootstrapper.FailGameplayReadiness("The player returned to the main menu from a slow load. " + lastDiagnostic);
			this.ShowLoadFailure();
			this._leavingSlowLoad = true;
			this._leaveRequestedFrame = Time.frameCount;
		}

		// Token: 0x060082BF RID: 33471 RVA: 0x003561B8 File Offset: 0x003543B8
		private int ReadPromptChoice(int choiceCount)
		{
			if (Time.unscaledTime < this._promptInputReadyAt)
			{
				return -1;
			}
			int num = this.ReadPromptNavigation();
			if (num != 0)
			{
				this._promptSelection = Mathf.Clamp(this._promptSelection + num, 0, choiceCount - 1);
			}
			this._promptSelection = Mathf.Clamp(this._promptSelection, 0, choiceCount - 1);
			Keyboard current = Keyboard.current;
			if (ControllerInput.SubmitPressed || (current != null && (current.enterKey.wasPressedThisFrame || current.numpadEnterKey.wasPressedThisFrame || current.spaceKey.wasPressedThisFrame)))
			{
				return this._promptSelection;
			}
			if ((!ControllerInput.CancelPressed && (current == null || !current.escapeKey.wasPressedThisFrame)) || choiceCount <= 1)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x060082C0 RID: 33472 RVA: 0x0035626C File Offset: 0x0035446C
		private int ReadPromptNavigation()
		{
			int num = 0;
			if (ControllerInput.GetButtonDown(ControllerButton.DpadLeft) || ControllerInput.GetButtonDown(ControllerButton.DpadUp))
			{
				num = -1;
			}
			else if (ControllerInput.GetButtonDown(ControllerButton.DpadRight) || ControllerInput.GetButtonDown(ControllerButton.DpadDown))
			{
				num = 1;
			}
			Vector2 move = ControllerInput.Move;
			float num2 = ((Mathf.Abs(move.x) >= Mathf.Abs(move.y)) ? move.x : (-move.y));
			if (Mathf.Abs(num2) < 0.35f)
			{
				this._promptStickNeutral = true;
			}
			else if (this._promptStickNeutral)
			{
				this._promptStickNeutral = false;
				if (num == 0)
				{
					num = ((num2 > 0f) ? 1 : (-1));
				}
			}
			Keyboard current = Keyboard.current;
			if (current != null && num == 0)
			{
				if (current.leftArrowKey.wasPressedThisFrame || current.upArrowKey.wasPressedThisFrame)
				{
					num = -1;
				}
				else if (current.rightArrowKey.wasPressedThisFrame || current.downArrowKey.wasPressedThisFrame || current.tabKey.wasPressedThisFrame)
				{
					num = 1;
				}
			}
			return num;
		}

		// Token: 0x060082C1 RID: 33473 RVA: 0x0035635C File Offset: 0x0035455C
		public void ReturnToMenuAfterLoadFailure()
		{
			if (!this.HasLoadFailure || !Application.CanStreamedLevelBeLoaded("MainMenu"))
			{
				return;
			}
			base.StopAllCoroutines();
			this._manualEndRoutine = null;
			this.ClearLoadFailure();
			this.ResetSlowLoadState();
			this._recoveryLoadPending = true;
			this.IsManualLoadActive = false;
			Time.timeScale = 1f;
			this.LoadSceneAsync("MainMenu");
		}

		// Token: 0x060082C2 RID: 33474 RVA: 0x003563BA File Offset: 0x003545BA
		private IEnumerator EndManualLoadRoutine()
		{
			yield return this.WaitForInitialPresentation();
			yield return this.WaitForGameplayWarmup();
			this.IsPreparingReveal = false;
			this.ReportProgress(1f);
			while (this._displayedProgress < 0.999f)
			{
				yield return null;
			}
			if (this.fadeOutFeedbacks != null)
			{
				this.fadeOutFeedbacks.PlayFeedbacks();
				float totalDuration = this.fadeOutFeedbacks.TotalDuration;
				if (totalDuration > 0f)
				{
					yield return new WaitForSecondsRealtime(totalDuration);
				}
			}
			this._manualEndRoutine = null;
			this.HideImmediate();
			yield break;
		}

		// Token: 0x060082C3 RID: 33475 RVA: 0x003563C9 File Offset: 0x003545C9
		private void CancelManualCompletion()
		{
			this.IsPreparingReveal = false;
			if (this._manualEndRoutine != null)
			{
				base.StopCoroutine(this._manualEndRoutine);
				this._manualEndRoutine = null;
			}
			if (this.fadeOutFeedbacks != null)
			{
				this.fadeOutFeedbacks.StopFeedbacks();
			}
		}

		// Token: 0x060082C4 RID: 33476 RVA: 0x00356406 File Offset: 0x00354606
		private IEnumerator WaitForInitialPresentation()
		{
			LoadingReadinessWatchdog watchdog = new LoadingReadinessWatchdog(Mathf.Max(5f, this.readinessSafetyTimeout));
			while (!LoadingScreenController.IsInitialPresentationReady())
			{
				int num = (RankRuntimeController.InitialPresentationReady ? 1 : 0) | (EnvironmentLightingRefresh.IsReady ? 2 : 0);
				if (watchdog.Tick(Time.realtimeSinceStartupAsDouble, GameSystemsBootstrapper.InitializationProgress, GameSystemsBootstrapper.InitializationWorkVersion, num))
				{
					Debug.LogError("[LoadingScreen] Initial presentation stopped making progress; releasing loading through the safety fallback.");
					yield break;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x060082C5 RID: 33477 RVA: 0x00356415 File Offset: 0x00354615
		private IEnumerator WaitForGameplayWarmup()
		{
			GameSystemsBootstrapper instance = GameSystemsBootstrapper.Instance;
			if (instance == null || !GameSystemsBootstrapper.IsRunReady(false) || !instance.GetFeatureEnabled(GameSystemsBootstrapper.RuntimeFeature.ProceduralMap))
			{
				yield break;
			}
			this.RestoreLoadPriority();
			SensoryManager instance2 = SensoryManager.Instance;
			if (instance2 != null)
			{
				instance2.PrewarmDetectionJob();
			}
			double num = 0.03666666666666667;
			if (Application.targetFrameRate > 0)
			{
				num = Math.Max(num, 1.2 / (double)Application.targetFrameRate);
			}
			if (QualitySettings.vSyncCount > 0 && Screen.currentResolution.refreshRate > 0)
			{
				num = Math.Max(num, 1.2 * (double)QualitySettings.vSyncCount / (double)Screen.currentResolution.refreshRate);
			}
			LoadingFrameStability stability = new LoadingFrameStability(LoadingScreenController._cityRenderWarmed ? 1.0 : 6.0, 15.0, 0.75, num);
			while (!stability.Tick(Time.realtimeSinceStartupAsDouble, (double)Time.unscaledDeltaTime, LoadingScreenController.IsInitialPresentationReady() && ProceduralMiniMapController.InitialRenderReady))
			{
				this.ReportProgress(Mathf.Lerp(0.97f, 0.99f, (float)(stability.ElapsedSeconds / 15.0)));
				yield return null;
			}
			LoadingScreenController._cityRenderWarmed = !stability.TimedOut;
			if (stability.TimedOut)
			{
				Debug.LogWarning("[LoadingScreen] City render warmup reached 15s without stable frames; revealing gameplay.");
			}
			yield break;
		}

		// Token: 0x060082C6 RID: 33478 RVA: 0x00356424 File Offset: 0x00354624
		public void LoadSceneAsync(int buildIndex)
		{
			this.EnsureActiveForCoroutine();
			base.StartCoroutine(this.LoadSceneRoutine(buildIndex, null, null));
		}

		// Token: 0x060082C7 RID: 33479 RVA: 0x0035643C File Offset: 0x0035463C
		public void LoadSceneAsync(string sceneName)
		{
			this.EnsureActiveForCoroutine();
			base.StartCoroutine(this.LoadSceneRoutine(-1, sceneName, null));
		}

		// Token: 0x060082C8 RID: 33480 RVA: 0x00356454 File Offset: 0x00354654
		public void ActivatePreloadedSceneAsync(string sceneName, AsyncOperation preloaded)
		{
			if (preloaded == null)
			{
				this.LoadSceneAsync(sceneName);
				return;
			}
			this.EnsureActiveForCoroutine();
			base.StartCoroutine(this.LoadSceneRoutine(-1, sceneName, preloaded));
		}

		// Token: 0x060082C9 RID: 33481 RVA: 0x00356478 File Offset: 0x00354678
		private void EnsureActiveForCoroutine()
		{
			if (this.rootObject != null && !this.rootObject.activeSelf)
			{
				this.rootObject.SetActive(true);
			}
			if (!base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(true);
			}
		}

		// Token: 0x060082CA RID: 33482 RVA: 0x003564C5 File Offset: 0x003546C5
		private IEnumerator LoadSceneRoutine(int buildIndex, string sceneName, AsyncOperation preloaded = null)
		{
			this.ClearLoadFailure();
			this.ResetSlowLoadState();
			GameSystemsBootstrapper.InvalidateForSceneTransition();
			FlyCamera.StopForContextChange();
			this.CancelManualCompletion();
			this.IsManualLoadActive = false;
			this.IsLoadingActive = true;
			this.BoostLoadPriority();
			this.PauseGameplayAudio();
			this.EnsurePlayerInputLocked();
			this.RandomizeOnce();
			this.ShowImmediate();
			this._displayedProgress = 0f;
			this._targetProgress = 0f;
			this.UpdateProgressVisuals(0f);
			if (this.fadeInFeedbacks != null)
			{
				this.fadeInFeedbacks.PlayFeedbacks();
			}
			HashSet<int> hashSet = new HashSet<int>();
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				hashSet.Add(SceneManager.GetSceneAt(i).handle);
			}
			AsyncOperation op = preloaded;
			if (op == null)
			{
				op = ((!string.IsNullOrEmpty(sceneName)) ? SceneManager.LoadSceneAsync(sceneName) : SceneManager.LoadSceneAsync(buildIndex));
			}
			if (op == null)
			{
				Debug.LogWarning("[LoadingScreen] LoadSceneAsync returned null for '" + (sceneName ?? buildIndex.ToString()) + "', fallback to sync load.");
				HeavyZoneStreamingProxy.ReleaseCachedPayloadAssets(true);
				if (!string.IsNullOrEmpty(sceneName))
				{
					SceneManager.LoadScene(sceneName);
				}
				else
				{
					SceneManager.LoadScene(buildIndex);
				}
				this._recoveryLoadPending = false;
				yield break;
			}
			op.allowSceneActivation = false;
			float startTime = Time.unscaledTime;
			LoadingScreenController.RetireSingleLoadSourceNavigation(hashSet);
			yield return GameplayTeardownService.FastTeardownRoutine();
			while (!WorldOrigin.TryResetForSceneExit())
			{
				yield return null;
			}
			while (op.progress < 0.9f || Time.unscaledTime - startTime < this.minDisplayDuration)
			{
				float num = Mathf.Clamp01(op.progress / 0.9f);
				float num2 = ((this.minDisplayDuration > 0f) ? Mathf.Clamp01((Time.unscaledTime - startTime) / this.minDisplayDuration) : 1f);
				float num3 = Mathf.Min(num, num2);
				this.ReportProgress(num3 * 0.25f);
				yield return null;
			}
			this.ReportProgress(0.25f);
			float fileLoadSeconds = Time.unscaledTime - startTime;
			HeavyZoneStreamingProxy.ReleaseCachedPayloadAssets(true);
			op.allowSceneActivation = true;
			while (!op.isDone)
			{
				yield return null;
			}
			this._recoveryLoadPending = false;
			this.ReportProgress(0.28f);
			Debug.Log(string.Concat(new string[]
			{
				"[LoadingScreen] '",
				sceneName ?? buildIndex.ToString(),
				"': file load ",
				string.Format("{0:0.0}s, activation ", fileLoadSeconds),
				string.Format("{0:0.0}s.", Time.unscaledTime - startTime - fileLoadSeconds)
			}));
			MemoryReport.Log(string.Format("load-scene-activated t={0:F1}s", Time.realtimeSinceStartup));
			if (LoadingScreenController.SceneHasBootLoader())
			{
				double waitStart = Time.realtimeSinceStartupAsDouble;
				float hardDeadline = Mathf.Max(5f, this.readinessSafetyTimeout);
				LoadingReadinessWatchdog watchdog = new LoadingReadinessWatchdog(hardDeadline);
				bool gameplayReadyReported = false;
				bool degraded = false;
				while (!SceneBootLoader.CoreReady || !GameSystemsBootstrapper.GameplayReady || !LoadingScreenController.IsInitialPresentationReady())
				{
					if (GameSystemsBootstrapper.GameplayReadinessFailed)
					{
						this.ShowLoadFailure();
						yield break;
					}
					if (!gameplayReadyReported && GameSystemsBootstrapper.GameplayReady)
					{
						gameplayReadyReported = true;
						MemoryReport.Log(string.Format("load-gameplay-ready t={0:F1}s", Time.realtimeSinceStartup));
					}
					float num4 = ((!GameSystemsBootstrapper.GameplayReady) ? (0.28f + GameSystemsBootstrapper.InitializationProgress * 0.66f) : 0.97f);
					this.ReportProgress(num4);
					int num5 = (SceneBootLoader.CoreReady ? 1 : 0) | (GameSystemsBootstrapper.GameplayReady ? 2 : 0) | (LoadingScreenController.IsInitialPresentationReady() ? 4 : 0);
					if (watchdog.Tick(Time.realtimeSinceStartupAsDouble, GameSystemsBootstrapper.InitializationProgress, GameSystemsBootstrapper.InitializationWorkVersion, num5))
					{
						string stall = string.Format("LoadingScreenController: no progress for {0:0.#}s ", hardDeadline) + string.Format("(total {0:0.#}s)", Time.realtimeSinceStartupAsDouble - waitStart);
						bool flag = GameSystemsBootstrapper.ForceCompleteGameplayReadiness(stall);
						if (!flag && !GameSystemsBootstrapper.GameplayReadinessFailed)
						{
							LoadingStallMonitor.Report(this, true, () => stall);
							yield return null;
						}
						else
						{
							if (!flag || !SceneBootLoader.CoreReady)
							{
								GameSystemsBootstrapper.FailGameplayReadiness("Core or required gameplay dependencies remain unavailable.");
								this.ShowLoadFailure();
								yield break;
							}
							LongWayHomeRestaurantSequence presenterForScene = LongWayHomeRestaurantSequence.GetPresenterForScene(SceneManager.GetActiveScene());
							if (presenterForScene != null && presenterForScene.OwnsDestinationReveal && !presenterForScene.IsPreparedForReveal)
							{
								presenterForScene.FailPreparation("The restaurant presentation did not finish before the scene loading deadline.");
							}
							ILongWayHomePoiProloguePresenter presenterForScene2 = LongWayHomePoiPrologue.GetPresenterForScene(SceneManager.GetActiveScene());
							if (presenterForScene2 != null && presenterForScene2.OwnsDestinationReveal && !presenterForScene2.IsPreparedForReveal)
							{
								presenterForScene2.FailPreparation("The opening presentation did not finish before the scene loading deadline.");
							}
							if (!LoadingScreenController.IsInitialPresentationReady())
							{
								Debug.LogError("[LoadingScreen] Initial presentation missed its safety deadline; releasing the loading screen in degraded mode.");
							}
							degraded = true;
							break;
						}
					}
					else
					{
						LoadingStallMonitor.Report(this, false, null);
						yield return null;
					}
				}
				LoadingStallMonitor.Report(this, false, null);
				MemoryReport.Log(string.Format("{0} t={1:F1}s", degraded ? "load-ready-degraded" : "load-ready", Time.realtimeSinceStartup));
				yield return this.WaitForLongWayHomeOpening();
				watchdog = null;
			}
			this.IsPreparingReveal = true;
			yield return this.WaitForGameplayWarmup();
			this.IsPreparingReveal = false;
			this.ReportProgress(1f);
			while (this._displayedProgress < 0.999f)
			{
				yield return null;
			}
			if (this.fadeOutFeedbacks != null)
			{
				this.fadeOutFeedbacks.PlayFeedbacks();
				float totalDuration = this.fadeOutFeedbacks.TotalDuration;
				if (totalDuration > 0f)
				{
					yield return new WaitForSecondsRealtime(totalDuration);
				}
			}
			this.HideImmediate();
			yield break;
		}

		// Token: 0x060082CB RID: 33483 RVA: 0x003564EC File Offset: 0x003546EC
		private static void RetireSingleLoadSourceNavigation(HashSet<int> sourceScenes)
		{
			World defaultGameObjectInjectionWorld = World.DefaultGameObjectInjectionWorld;
			if (defaultGameObjectInjectionWorld != null && defaultGameObjectInjectionWorld.IsCreated)
			{
				defaultGameObjectInjectionWorld.EntityManager.CompleteAllTrackedJobs();
			}
			foreach (FollowerEntity followerEntity in Object.FindObjectsOfType<FollowerEntity>(true))
			{
				if (sourceScenes.Contains(followerEntity.gameObject.scene.handle))
				{
					followerEntity.enabled = false;
				}
			}
			foreach (CinemachineBrain cinemachineBrain in Object.FindObjectsOfType<CinemachineBrain>(true))
			{
				if (sourceScenes.Contains(cinemachineBrain.gameObject.scene.handle))
				{
					cinemachineBrain.enabled = false;
				}
			}
			AStarGraphManager instance = AStarGraphManager.Instance;
			if (instance != null && sourceScenes.Contains(instance.gameObject.scene.handle))
			{
				instance.RetireForSceneUnload();
				return;
			}
			foreach (ProceduralGraphMover proceduralGraphMover in Object.FindObjectsOfType<ProceduralGraphMover>(true))
			{
				if (sourceScenes.Contains(proceduralGraphMover.gameObject.scene.handle))
				{
					proceduralGraphMover.BindTarget(null);
					proceduralGraphMover.enabled = false;
				}
			}
			AstarPath active = AstarPath.active;
			if (active != null && sourceScenes.Contains(active.gameObject.scene.handle))
			{
				active.enabled = false;
			}
		}

		// Token: 0x060082CC RID: 33484 RVA: 0x0035665A File Offset: 0x0035485A
		public IEnumerator WaitForLongWayHomeOpening()
		{
			float deadline = Time.realtimeSinceStartup + 180f;
			while (LongWayHomeRuntimeController.IsPreparing && !LongWayHomeRuntimeController.Instance.IsOpeningPrepared)
			{
				this.ReportProgress(Mathf.Lerp(0.96f, 0.99f, LongWayHomeRuntimeController.Instance.PreparationProgress));
				if (Time.realtimeSinceStartup >= deadline)
				{
					LongWayHomeRuntimeController.Instance.FailPreparation("The Long Way Home opening could not be prepared in time.");
					yield break;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x060082CD RID: 33485 RVA: 0x0035666C File Offset: 0x0035486C
		private void UpdateProgressVisuals(float t)
		{
			if (this._progressMaterial != null)
			{
				this._progressMaterial.SetFloat(LoadingScreenController.ProgressFillAmount, t);
			}
			else if (this.progressFill != null)
			{
				this.progressFill.fillAmount = t;
			}
			if (this.progressSlider != null)
			{
				this.progressSlider.value = t;
			}
			if (this.progressLabel != null)
			{
				this.progressLabel.text = string.Format("{0}%", Mathf.RoundToInt(t * 100f));
			}
		}

		// Token: 0x060082CE RID: 33486 RVA: 0x00356704 File Offset: 0x00354904
		private void ShowImmediate()
		{
			if (this.rootObject != null)
			{
				this.rootObject.SetActive(true);
			}
			if (this.rootGroup != null)
			{
				this.rootGroup.alpha = 1f;
				this.rootGroup.interactable = true;
				this.rootGroup.blocksRaycasts = true;
			}
		}

		// Token: 0x060082CF RID: 33487 RVA: 0x00356764 File Offset: 0x00354964
		private void RandomizeOnce()
		{
			LoadingScreenRandomizer component = base.GetComponent<LoadingScreenRandomizer>();
			if (component != null)
			{
				component.Randomize();
			}
		}

		// Token: 0x060082D0 RID: 33488 RVA: 0x00356788 File Offset: 0x00354988
		private void HideImmediate()
		{
			this.IsLoadingActive = false;
			this.ResetSlowLoadState();
			if (this.rootGroup != null)
			{
				this.rootGroup.alpha = 0f;
				this.rootGroup.interactable = false;
				this.rootGroup.blocksRaycasts = false;
			}
			if (this.rootObject != null)
			{
				this.rootObject.SetActive(false);
			}
			this.RestorePlayerInput();
			this.RestoreGameplayAudio();
			this.RestoreLoadPriority();
		}

		// Token: 0x060082D1 RID: 33489 RVA: 0x00356804 File Offset: 0x00354A04
		private void BoostLoadPriority()
		{
			if (this._loadPriorityBoosted)
			{
				return;
			}
			this._savedLoadPriority = Application.backgroundLoadingPriority;
			Application.backgroundLoadingPriority = ThreadPriority.High;
			this._loadPriorityBoosted = true;
		}

		// Token: 0x060082D2 RID: 33490 RVA: 0x00356827 File Offset: 0x00354A27
		private void RestoreLoadPriority()
		{
			if (!this._loadPriorityBoosted)
			{
				return;
			}
			Application.backgroundLoadingPriority = this._savedLoadPriority;
			this._loadPriorityBoosted = false;
		}

		// Token: 0x060082D3 RID: 33491 RVA: 0x00356844 File Offset: 0x00354A44
		private void EnsurePlayerInputLocked()
		{
			if (this._lockedPlayerInput != null)
			{
				return;
			}
			this._lockedPlayerInput = Object.FindObjectOfType<vThirdPersonInput>();
			if (this._lockedPlayerInput == null)
			{
				return;
			}
			this._playerInputWasLocked = this._lockedPlayerInput.lockInput;
			this._cameraInputWasLocked = this._lockedPlayerInput.lockCameraInput;
			this._lockedPlayerInput.SetLockAllInput(true);
			this._lockedPlayerInput.SetLockCameraInput(true);
			if (this._lockedPlayerInput.cc != null)
			{
				this._lockedPlayerInput.cc.input = Vector2.zero;
				this._lockedPlayerInput.cc.isSprinting = false;
			}
		}

		// Token: 0x060082D4 RID: 33492 RVA: 0x003568F4 File Offset: 0x00354AF4
		public bool TryGetPreLoadingInputLocks(vThirdPersonInput input, out bool inputLocked, out bool cameraLocked)
		{
			inputLocked = (cameraLocked = false);
			if (input == null || this._lockedPlayerInput != input)
			{
				return false;
			}
			inputLocked = this._playerInputWasLocked;
			cameraLocked = this._cameraInputWasLocked;
			return true;
		}

		// Token: 0x060082D5 RID: 33493 RVA: 0x00356933 File Offset: 0x00354B33
		public bool TryReleaseCapturedPlayerInputLock(vThirdPersonInput input, bool previousOwnerLock)
		{
			if (!this.IsLoadingActive || input == null || this._lockedPlayerInput != input)
			{
				return false;
			}
			this._playerInputWasLocked = this._playerInputWasLocked && previousOwnerLock;
			input.SetLockAllInput(true);
			return true;
		}

		// Token: 0x060082D6 RID: 33494 RVA: 0x0035696C File Offset: 0x00354B6C
		private void RestorePlayerInput()
		{
			if (this._lockedPlayerInput == null)
			{
				return;
			}
			this._lockedPlayerInput.SetLockAllInput(this._playerInputWasLocked);
			this._lockedPlayerInput.SetLockCameraInput(this._cameraInputWasLocked);
			this._lockedPlayerInput = null;
		}

		// Token: 0x060082D7 RID: 33495 RVA: 0x003569A6 File Offset: 0x00354BA6
		private void PauseGameplayAudio()
		{
			if (this._audioPause != null)
			{
				return;
			}
			this._audioPause = GameplayAudioPause.Acquire();
		}

		// Token: 0x060082D8 RID: 33496 RVA: 0x003569BC File Offset: 0x00354BBC
		private void RestoreGameplayAudio()
		{
			IDisposable audioPause = this._audioPause;
			if (audioPause != null)
			{
				audioPause.Dispose();
			}
			this._audioPause = null;
		}

		// Token: 0x060082D9 RID: 33497 RVA: 0x003569D6 File Offset: 0x00354BD6
		private static bool SceneHasBootLoader()
		{
			return Object.FindObjectOfType<SceneBootLoader>() != null;
		}

		// Token: 0x060082DA RID: 33498 RVA: 0x003569E4 File Offset: 0x00354BE4
		private static bool IsInitialPresentationReady()
		{
			if (!EnvironmentLightingRefresh.IsReady)
			{
				return false;
			}
			if (RankRuntimeController.Current != null && !RankRuntimeController.InitialPresentationReady)
			{
				return false;
			}
			LongWayHomeRestaurantSequence presenterForScene = LongWayHomeRestaurantSequence.GetPresenterForScene(SceneManager.GetActiveScene());
			if (presenterForScene != null && presenterForScene.OwnsDestinationReveal && !presenterForScene.IsPreparedForReveal)
			{
				return false;
			}
			ILongWayHomePoiProloguePresenter presenterForScene2 = LongWayHomePoiPrologue.GetPresenterForScene(SceneManager.GetActiveScene());
			return presenterForScene2 == null || !presenterForScene2.OwnsDestinationReveal || presenterForScene2.IsPreparedForReveal;
		}

		// Token: 0x040085CE RID: 34254
		private static LoadingScreenController _instance;

		// Token: 0x040085CF RID: 34255
		private static bool _cityRenderWarmed;

		// Token: 0x040085D0 RID: 34256
		private const string ResourcePath = "LoadingScreen";

		// Token: 0x040085D5 RID: 34261
		[Header("Root")]
		[Tooltip("CanvasGroup del pannello loading (pannello + progress bar). Lo abilitiamo/disabilitiamo via alpha + interactable + blocksRaycasts.")]
		[SerializeField]
		private CanvasGroup rootGroup;

		// Token: 0x040085D6 RID: 34262
		[Tooltip("GameObject root da SetActive(false) quando hidden.")]
		[SerializeField]
		private GameObject rootObject;

		// Token: 0x040085D7 RID: 34263
		[Header("Progress")]
		[Tooltip("Image (fillAmount) o Slider per il progresso. Riempire 0→1 mentre la scena carica.")]
		[SerializeField]
		private Image progressFill;

		// Token: 0x040085D8 RID: 34264
		[Tooltip("Slider opzionale come alternativa a progressFill.")]
		[SerializeField]
		private Slider progressSlider;

		// Token: 0x040085D9 RID: 34265
		[Tooltip("Label progresso testuale (es. '47%'). Aggiornata ogni frame.")]
		[SerializeField]
		private TMP_Text progressLabel;

		// Token: 0x040085DA RID: 34266
		[Header("Behavior")]
		[Tooltip("Durata minima della schermata loading (s). Anche se la scena carica in 0.1s, il pannello resta visibile per questo tempo cosi' la transizione non lampeggia.")]
		[Min(0f)]
		[SerializeField]
		private float minDisplayDuration = 0.6f;

		// Token: 0x040085DB RID: 34267
		[Tooltip("Velocita' di lerp del progresso visualizzato verso il valore reale. Smooth out per evitare jump a scatti. 0 = instant.")]
		[Min(0f)]
		[SerializeField]
		private float progressLerpSpeed = 3f;

		// Token: 0x040085DC RID: 34268
		[Tooltip("Seconds without preparation progress before the stalled-load safety fallback.")]
		[SerializeField]
		[Min(5f)]
		private float readinessSafetyTimeout = 45f;

		// Token: 0x040085DD RID: 34269
		[Tooltip("Seconds the slow-load choice stays hidden after the player chooses to keep waiting.")]
		[SerializeField]
		[Min(5f)]
		private float slowLoadPromptSnoozeSeconds = 30f;

		// Token: 0x040085DE RID: 34270
		[Header("Feel")]
		[Tooltip("Feedback MMF al fade-in del pannello loading (es. alpha 0→1).")]
		[SerializeField]
		private MMF_Player fadeInFeedbacks;

		// Token: 0x040085DF RID: 34271
		[Tooltip("Feedback MMF al fade-out (post-load, prima di SetActive(false)).")]
		[SerializeField]
		private MMF_Player fadeOutFeedbacks;

		// Token: 0x040085E0 RID: 34272
		[Tooltip("Se true, il GameObject sopravvive ai reload scena (DontDestroyOnLoad). Necessario per il suo ruolo.")]
		[SerializeField]
		private bool persistAcrossScenes = true;

		// Token: 0x040085E1 RID: 34273
		private float _displayedProgress;

		// Token: 0x040085E2 RID: 34274
		private float _targetProgress;

		// Token: 0x040085E3 RID: 34275
		private ThreadPriority _savedLoadPriority;

		// Token: 0x040085E4 RID: 34276
		private bool _loadPriorityBoosted;

		// Token: 0x040085E5 RID: 34277
		private IDisposable _audioPause;

		// Token: 0x040085E6 RID: 34278
		private Material _progressMaterial;

		// Token: 0x040085E7 RID: 34279
		private static readonly int ProgressFillAmount = Shader.PropertyToID("_FillAmount");

		// Token: 0x040085E8 RID: 34280
		private vThirdPersonInput _lockedPlayerInput;

		// Token: 0x040085E9 RID: 34281
		private bool _playerInputWasLocked;

		// Token: 0x040085EA RID: 34282
		private bool _cameraInputWasLocked;

		// Token: 0x040085EB RID: 34283
		private Coroutine _manualEndRoutine;

		// Token: 0x040085EC RID: 34284
		private CursorLockMode _failureCursorLock;

		// Token: 0x040085ED RID: 34285
		private bool _failureCursorVisible;

		// Token: 0x040085EE RID: 34286
		private const string FailedKey = "loading.failed";

		// Token: 0x040085EF RID: 34287
		private const string FailedEnglish = "Unable to finish loading. Return to the main menu to try again.";

		// Token: 0x040085F0 RID: 34288
		private const string SlowKey = "loading.slow";

		// Token: 0x040085F1 RID: 34289
		private const string SlowEnglish = "Loading is taking longer than usual. You can keep waiting or return to the main menu.";

		// Token: 0x040085F2 RID: 34290
		private const string WaitKey = "loading.wait";

		// Token: 0x040085F3 RID: 34291
		private const string WaitEnglish = "Keep Waiting";

		// Token: 0x040085F4 RID: 34292
		private const string ReturnKey = "loading.return_menu";

		// Token: 0x040085F5 RID: 34293
		private const string ReturnEnglish = "Return to Main Menu";

		// Token: 0x040085F6 RID: 34294
		private const float PromptInputDelay = 0.5f;

		// Token: 0x040085F8 RID: 34296
		private double _slowLoadPromptHiddenUntil;

		// Token: 0x040085F9 RID: 34297
		private float _promptInputReadyAt;

		// Token: 0x040085FA RID: 34298
		private int _promptSelection;

		// Token: 0x040085FB RID: 34299
		private bool _promptStickNeutral = true;

		// Token: 0x040085FC RID: 34300
		private CursorLockMode _promptCursorLock;

		// Token: 0x040085FD RID: 34301
		private bool _promptCursorVisible;

		// Token: 0x040085FE RID: 34302
		private bool _leavingSlowLoad;

		// Token: 0x040085FF RID: 34303
		private int _leaveRequestedFrame;

		// Token: 0x04008600 RID: 34304
		private bool _recoveryLoadPending;

		// Token: 0x04008601 RID: 34305
		private string _promptFontCode;

		// Token: 0x04008602 RID: 34306
		private Font _promptFont;
	}
}
