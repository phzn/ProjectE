using System;
using System.Collections;
using Features.CielOS.Apps.Story;
using Features.Detection;
using Features.Loading;
using Features.Localization;
using Features.RunModes;
using ProjectExhibitionist.CharacterCustomization.Core.Runtime;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.PoiTravel;
using ProjectExhibitionist.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.Home
{
	// Token: 0x0200119A RID: 4506
	public class HomeManager : MonoBehaviour
	{
		// Token: 0x060089F3 RID: 35315 RVA: 0x003860B8 File Offset: 0x003842B8
		public void OnStartGame()
		{
			string text;
			if (!this.TryStartGame(null, out text) && !string.IsNullOrEmpty(text))
			{
				ToastService.Show(text, "Start run", null, -1f, "cielos_settings");
			}
		}

		// Token: 0x060089F4 RID: 35316 RVA: 0x003860F0 File Offset: 0x003842F0
		public bool CanStartGame(bool isLongWayHome, out string error)
		{
			error = null;
			SceneTransitionManager instance = SceneTransitionManager.Instance;
			if (this._startingGame || (instance != null && instance.IsTransitioning) || (LoadingScreenController.ExistingInstance != null && LoadingScreenController.ExistingInstance.IsLoadingActive))
			{
				error = "A scene transition is already in progress.";
				return false;
			}
			string text = HomeManager.ResolveRunEntryScene(this.gameSceneName, isLongWayHome);
			if (!Application.CanStreamedLevelBeLoaded(this.gameSceneName) || !Application.CanStreamedLevelBeLoaded(text))
			{
				string text2;
				GameLocalization.TryGetSourceKey("cielos_settings", "Run entry '{0}' or gameplay scene '{1}' cannot be loaded. Check Build Settings.", out text2);
				error = GameLocalization.Format(text2, "Run entry '{0}' or gameplay scene '{1}' cannot be loaded. Check Build Settings.", new object[] { text, this.gameSceneName });
				return false;
			}
			if (SceneManager.GetActiveScene().name == text)
			{
				error = "The requested run scene is already active.";
				return false;
			}
			if ((instance != null) ? (!instance.isActiveAndEnabled) : (!Application.CanStreamedLevelBeLoaded(this.coreSceneName)))
			{
				error = "The scene transition system is unavailable.";
				return false;
			}
			return true;
		}

		// Token: 0x060089F5 RID: 35317 RVA: 0x003861E4 File Offset: 0x003843E4
		public bool TryStartGame(Action closeDesktop, out string error)
		{
			if (!this.CanStartGame(RunModeService.IsLongWayHome, out error))
			{
				return false;
			}
			if (!CielOSStoryFlow.BeforeStartRun(out error))
			{
				return false;
			}
			if (!RunModeService.HasPreparedRun && !RunModeService.TryPrepareRun(RunModeKind.Classic, null, out error))
			{
				return false;
			}
			this._startingGame = true;
			string text = HomeManager.ResolveRunEntryScene(this.gameSceneName, RunModeService.IsLongWayHome);
			bool flag = false;
			bool flag2 = false;
			ScreenFadeController screenFadeController = null;
			LoadingScreenController loadingScreenController = null;
			string pendingGameplayScene = SceneTransitionManager.PendingGameplayScene;
			bool flag3;
			try
			{
				if (RunModeService.IsLongWayHome)
				{
					screenFadeController = ScreenFadeController.Instance;
					loadingScreenController = LoadingScreenController.Instance;
					if (screenFadeController == null || loadingScreenController == null)
					{
						error = "The restaurant loading presentation is unavailable.";
						return false;
					}
					flag2 = true;
					HomeManager.BeginRestaurantLoading(screenFadeController, loadingScreenController, closeDesktop);
				}
				else if (closeDesktop != null)
				{
					closeDesktop();
				}
				PoiTravelState.ClearPendingReturn();
				SceneTransitionManager instance = SceneTransitionManager.Instance;
				if (instance != null)
				{
					PoiArrivalPresentationRequest.RequestForScene(text);
					Debug.Log("HomeManager: Starting game via additive swap -> '" + text + "'.");
					instance.LoadGameplayScene(text);
					flag = instance.IsTransitioning;
					if (!flag)
					{
						error = "The run scene transition could not start.";
						flag3 = false;
					}
					else
					{
						base.StartCoroutine(this.ResetStartGuardWhenTransitionEnds(instance));
						flag3 = true;
					}
				}
				else
				{
					SceneTransitionManager.PendingGameplayScene = text;
					PoiArrivalPresentationRequest.RequestForScene(text);
					loadingScreenController = ((loadingScreenController != null) ? loadingScreenController : LoadingScreenController.Instance);
					flag2 = true;
					if (loadingScreenController != null)
					{
						loadingScreenController.BeginManualLoad();
					}
					Debug.LogWarning("HomeManager: _Core non presente; fallback Single load di '" + this.coreSceneName + "'.");
					SceneManager.LoadScene(this.coreSceneName);
					flag = true;
					flag3 = true;
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex, this);
				error = "The run could not start. Please try again.";
				flag3 = false;
			}
			finally
			{
				if (!flag)
				{
					this._startingGame = false;
					SceneTransitionManager.PendingGameplayScene = pendingGameplayScene;
					if (flag2)
					{
						try
						{
							if (loadingScreenController != null)
							{
								loadingScreenController.ForceHideImmediate();
							}
						}
						finally
						{
							if (screenFadeController != null)
							{
								screenFadeController.SetClear();
							}
						}
					}
				}
			}
			return flag3;
		}

		// Token: 0x060089F6 RID: 35318 RVA: 0x003863E4 File Offset: 0x003845E4
		private static void BeginRestaurantLoading(ScreenFadeController cover, LoadingScreenController loading, Action closeDesktop)
		{
			cover.SetBlack();
			if (closeDesktop != null)
			{
				closeDesktop();
			}
			loading.BeginManualLoad();
		}

		// Token: 0x060089F7 RID: 35319 RVA: 0x003863FB File Offset: 0x003845FB
		private IEnumerator ResetStartGuardWhenTransitionEnds(SceneTransitionManager transition)
		{
			while (transition != null && transition.IsTransitioning)
			{
				yield return null;
			}
			this._startingGame = false;
			yield break;
		}

		// Token: 0x060089F8 RID: 35320 RVA: 0x00386411 File Offset: 0x00384611
		private static string ResolveRunEntryScene(string gameplaySceneName, bool isLongWayHome)
		{
			return HomeManager.ResolveRunEntryScene(gameplaySceneName, isLongWayHome, RunModeService.CurrentLongWayHomePrologue);
		}

		// Token: 0x060089F9 RID: 35321 RVA: 0x0038641F File Offset: 0x0038461F
		public static string ResolveRunEntryScene(string gameplaySceneName, bool isLongWayHome, LongWayHomePrologue prologue)
		{
			string text;
			if (isLongWayHome)
			{
				if ((text = LongWayHomeRuntimeController.ProloguePoiScene(prologue)) == null)
				{
					return "LongWayHome_RestaurantIntro";
				}
			}
			else
			{
				text = gameplaySceneName;
			}
			return text;
		}

		// Token: 0x060089FA RID: 35322 RVA: 0x00386435 File Offset: 0x00384635
		public void OpenMainCharacterCustomization()
		{
			if (this.customizationSceneNavigator == null)
			{
				Debug.LogError("HomeManager: CustomizationSceneNavigator reference is missing.");
				return;
			}
			this.customizationSceneNavigator.OpenMainCharacterCustomization(this.mainCharacterId, this.mainCharacterPresetIsDefault, string.Empty);
		}

		// Token: 0x060089FB RID: 35323 RVA: 0x0038646C File Offset: 0x0038466C
		public void OpenCustomizationHub()
		{
			if (this.customizationSceneNavigator == null)
			{
				Debug.LogError("HomeManager: CustomizationSceneNavigator reference is missing.");
				return;
			}
			this.customizationSceneNavigator.OpenCustomizationHub();
		}

		// Token: 0x060089FC RID: 35324 RVA: 0x00386492 File Offset: 0x00384692
		public void OpenNpcCustomization(string npcCharacterId)
		{
			if (this.customizationSceneNavigator == null)
			{
				Debug.LogError("HomeManager: CustomizationSceneNavigator reference is missing.");
				return;
			}
			this.customizationSceneNavigator.OpenNpcCustomization(npcCharacterId, false, string.Empty);
		}

		// Token: 0x04008CA8 RID: 36008
		[Header("Scene Transition")]
		[Tooltip("The name of the scene to load when starting the game")]
		public string gameSceneName = "Game - Current";

		// Token: 0x04008CA9 RID: 36009
		[Header("Customization Entry")]
		[SerializeField]
		private CustomizationSceneNavigator customizationSceneNavigator;

		// Token: 0x04008CAA RID: 36010
		[SerializeField]
		private string mainCharacterId = "Ciel";

		// Token: 0x04008CAB RID: 36011
		[SerializeField]
		private bool mainCharacterPresetIsDefault = true;

		// Token: 0x04008CAC RID: 36012
		[Tooltip("Scena _Core persistente. Usata solo dal fallback difensivo se CielHome viene avviata senza il suo SceneBootLoader.")]
		public string coreSceneName = "_Core";

		// Token: 0x04008CAD RID: 36013
		private bool _startingGame;
	}
}
