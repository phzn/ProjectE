using System;
using Features.CielOS.Desktop;
using Features.CielOS.WindowManager;
using Features.Home;
using Features.Loading;
using Features.PhoneCompanion;
using Features.Rank;
using ProjectExhibitionist.Core.Rendering;
using ProjectExhibitionist.Core.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Features.CielOS.Apps.Story
{
	// Token: 0x02001431 RID: 5169
	[DisallowMultipleComponent]
	public sealed class CielOSStoryFlow : MonoBehaviour
	{
		// Token: 0x06009FA1 RID: 40865 RVA: 0x0041D797 File Offset: 0x0041B997
		private void Awake()
		{
			this._controller = base.GetComponent<CielOSController>();
		}

		// Token: 0x06009FA2 RID: 40866 RVA: 0x0041D7A8 File Offset: 0x0041B9A8
		private void Update()
		{
			if (this._controller == null || !this._controller.IsOpen)
			{
				this.HideGuidance();
				return;
			}
			if (!Application.isPlaying || Time.unscaledTime < this._nextAttempt)
			{
				return;
			}
			this._nextAttempt = Time.unscaledTime + 0.1f;
			this.RefreshGuidance();
		}

		// Token: 0x06009FA3 RID: 40867 RVA: 0x0041D803 File Offset: 0x0041BA03
		private void OnDisable()
		{
			this.HideGuidance();
		}

		// Token: 0x06009FA4 RID: 40868 RVA: 0x0041D80B File Offset: 0x0041BA0B
		public void HideGuidance()
		{
			if (this._spotlight != null)
			{
				this._spotlight.Hide();
			}
		}

		// Token: 0x06009FA5 RID: 40869 RVA: 0x0041D826 File Offset: 0x0041BA26
		public void RefreshGuidance()
		{
			if (this._controller == null || !this._controller.IsOpen || !CielOSStoryFlow.RequiresReadingInCurrentScene() || !this.TryPresentPending())
			{
				this.HideGuidance();
			}
		}

		// Token: 0x06009FA6 RID: 40870 RVA: 0x0041D858 File Offset: 0x0041BA58
		public static bool RequiresReading(string sceneName, bool isAdmin, bool hasPendingChapter)
		{
			return sceneName == "CielHome" && !isAdmin && hasPendingChapter;
		}

		// Token: 0x06009FA7 RID: 40871 RVA: 0x0041D870 File Offset: 0x0041BA70
		private static bool RequiresReadingInCurrentScene()
		{
			string name = SceneManager.GetActiveScene().name;
			return !(name != "CielHome") && CielOSStoryFlow.RequiresReading(name, RankProgressionService.IsAdminRankActive, RankStoryProgression.HasPendingChapter);
		}

		// Token: 0x06009FA8 RID: 40872 RVA: 0x0041D8AC File Offset: 0x0041BAAC
		public static bool BeforeStartRun(out string error)
		{
			error = null;
			if (!Application.isPlaying || !CielOSStoryFlow.RequiresReadingInCurrentScene())
			{
				return true;
			}
			error = "Open the new chapter in Story before starting a run.";
			CielOSController instance = CielOSController.Instance;
			if (instance != null)
			{
				CielOSStoryFlow component = instance.GetComponent<CielOSStoryFlow>();
				if (component != null)
				{
					component.RefreshGuidance();
				}
			}
			return false;
		}

		// Token: 0x06009FA9 RID: 40873 RVA: 0x0041D8F9 File Offset: 0x0041BAF9
		public static bool AllowsAppLaunch(string appId)
		{
			if (!Application.isPlaying || appId == "story" || !CielOSStoryFlow.RequiresReadingInCurrentScene())
			{
				return true;
			}
			CielOSController instance = CielOSController.Instance;
			if (instance != null)
			{
				CielOSStoryFlow component = instance.GetComponent<CielOSStoryFlow>();
				if (component != null)
				{
					component.RefreshGuidance();
				}
			}
			return false;
		}

		// Token: 0x06009FAA RID: 40874 RVA: 0x0041D934 File Offset: 0x0041BB34
		private bool IsPresentationReady()
		{
			if (this._controller == null || !this._controller.isActiveAndEnabled || !this._controller.IsOpen || Time.timeScale <= 0.0001f || !SceneBootLoader.CoreReady || !GameSystemsBootstrapper.GameplayReady || GraphicsSettingsRuntimeUI.AnyOpen || PhotoPolaroidViewer.IsOpen)
			{
				return false;
			}
			SceneTransitionManager instance = SceneTransitionManager.Instance;
			if (instance != null && instance.IsTransitioning)
			{
				return false;
			}
			LoadingScreenController existingInstance = LoadingScreenController.ExistingInstance;
			if (existingInstance != null && existingInstance.IsLoadingActive)
			{
				return false;
			}
			Scene activeScene = SceneManager.GetActiveScene();
			foreach (CielHomeIntroSequence cielHomeIntroSequence in Object.FindObjectsOfType<CielHomeIntroSequence>())
			{
				if (cielHomeIntroSequence.gameObject.scene == activeScene && cielHomeIntroSequence.OwnsDestinationReveal)
				{
					return false;
				}
			}
			foreach (CielOSCustomizationTakeover cielOSCustomizationTakeover in Object.FindObjectsOfType<CielOSCustomizationTakeover>())
			{
				if (cielOSCustomizationTakeover.IsActive || cielOSCustomizationTakeover.IsWindowedActive)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06009FAB RID: 40875 RVA: 0x0041DA40 File Offset: 0x0041BC40
		private bool TryPresentPending()
		{
			if (!this.IsPresentationReady())
			{
				return false;
			}
			if (this._windows == null)
			{
				this._windows = this._controller.GetComponentInChildren<CielOSWindowManager>(true);
			}
			if (this._windows == null)
			{
				this._windows = Object.FindObjectOfType<CielOSWindowManager>(true);
			}
			CielOSAppDefinition cielOSAppDefinition = ((this._windows != null && this._windows.Catalog != null) ? this._windows.Catalog.FindById("story") : null);
			if (cielOSAppDefinition == null || !cielOSAppDefinition.IsUnlocked || cielOSAppDefinition.windowContentPrefab == null || cielOSAppDefinition.windowContentPrefab.GetComponentInChildren<CielOSStoryApp>(true) == null)
			{
				if (!this._warnedMissingApp)
				{
					this._warnedMissingApp = true;
					Debug.LogWarning("[CielOSStoryFlow] Pending chapter needs the Story app in the home PC catalog.", this);
				}
				return false;
			}
			CielOSWindow cielOSWindow = null;
			foreach (CielOSWindow cielOSWindow2 in this._windows.Windows)
			{
				if (!(cielOSWindow2 == null) && cielOSWindow2.IsOpen && !cielOSWindow2.IsMinimized)
				{
					if (cielOSWindow2.AppId == "story")
					{
						cielOSWindow = cielOSWindow2;
					}
					else
					{
						cielOSWindow2.Minimize();
					}
				}
			}
			if (cielOSWindow != null)
			{
				this.HideGuidance();
				return true;
			}
			if (this._storyIcon == null || !this._storyIcon.isActiveAndEnabled)
			{
				foreach (CielOSDesktopIcon cielOSDesktopIcon in this._controller.GetComponentsInChildren<CielOSDesktopIcon>())
				{
					if (cielOSDesktopIcon.AppId == "story")
					{
						this._storyIcon = cielOSDesktopIcon;
						break;
					}
				}
			}
			if (this._storyIcon == null)
			{
				return false;
			}
			if (this._spotlight == null)
			{
				this._spotlight = base.GetComponent<CielOSStorySpotlight>() ?? base.gameObject.AddComponent<CielOSStorySpotlight>();
			}
			if (!this._spotlight.IsVisible)
			{
				if (EventSystem.current != null)
				{
					EventSystem.current.SetSelectedGameObject(null);
				}
				this._spotlight.Show(this._storyIcon, new UnityAction(this.OpenStoryFromPlayerClick));
			}
			return true;
		}

		// Token: 0x06009FAC RID: 40876 RVA: 0x0041DC7C File Offset: 0x0041BE7C
		private void OpenStoryFromPlayerClick()
		{
			if (!CielOSStoryFlow.RequiresReadingInCurrentScene() || !this.IsPresentationReady() || this._windows == null)
			{
				return;
			}
			CielOSAppDefinition cielOSAppDefinition = ((this._windows.Catalog != null) ? this._windows.Catalog.FindById("story") : null);
			if (cielOSAppDefinition == null)
			{
				return;
			}
			CielOSWindow cielOSWindow = this._windows.LaunchApp(cielOSAppDefinition);
			CielOSStoryApp cielOSStoryApp = ((cielOSWindow != null) ? cielOSWindow.GetComponentInChildren<CielOSStoryApp>(true) : null);
			if (cielOSStoryApp != null && cielOSStoryApp.ShowPendingChapter())
			{
				this.HideGuidance();
			}
		}

		// Token: 0x0400A38C RID: 41868
		private CielOSController _controller;

		// Token: 0x0400A38D RID: 41869
		private CielOSWindowManager _windows;

		// Token: 0x0400A38E RID: 41870
		private CielOSStorySpotlight _spotlight;

		// Token: 0x0400A38F RID: 41871
		private CielOSDesktopIcon _storyIcon;

		// Token: 0x0400A390 RID: 41872
		private float _nextAttempt;

		// Token: 0x0400A391 RID: 41873
		private bool _warnedMissingApp;
	}
}
