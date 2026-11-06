using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace MoreMountains.Tools
{
	// Token: 0x0200014D RID: 333
	public class MMAdditiveSceneLoadingManager : MMMonoBehaviour
	{
		// Token: 0x06000748 RID: 1864 RVA: 0x000233F8 File Offset: 0x000215F8
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		protected static void InitializeStatics()
		{
			MMAdditiveSceneLoadingManager._loadingInProgress = false;
			MMAdditiveSceneLoadingManager._interpolateProgress = false;
			MMAdditiveSceneLoadingManager._progressInterpolationSpeed = 0f;
			MMAdditiveSceneLoadingManager._speedIntervals = new List<MMSceneLoadingSpeedInterval>();
			MMAdditiveSceneLoadingManager._beforeEntryFadeDelay = 0f;
			MMAdditiveSceneLoadingManager._entryFadeTween = null;
			MMAdditiveSceneLoadingManager._entryFadeDuration = 0f;
			MMAdditiveSceneLoadingManager._afterEntryFadeDelay = 0f;
			MMAdditiveSceneLoadingManager._beforeExitFadeDelay = 0f;
			MMAdditiveSceneLoadingManager._exitFadeTween = null;
			MMAdditiveSceneLoadingManager._exitFadeDuration = 0f;
			MMAdditiveSceneLoadingManager._sceneToLoadName = "";
			MMAdditiveSceneLoadingManager._loadingScreenSceneName = "";
			MMAdditiveSceneLoadingManager._scenesInBuild = new List<string>();
			MMAdditiveSceneLoadingManager._initialScenes = null;
			MMAdditiveSceneLoadingManager._antiSpillSceneName = "";
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x00023494 File Offset: 0x00021694
		public static void LoadScene(string sceneToLoadName, MMAdditiveSceneLoadingManagerSettings settings)
		{
			MMAdditiveSceneLoadingManager.LoadScene(sceneToLoadName, settings.LoadingSceneName, settings.ThreadPriority, settings.SecureLoad, settings.InterpolateProgress, settings.BeforeEntryFadeDelay, settings.EntryFadeDuration, settings.AfterEntryFadeDelay, settings.BeforeExitFadeDelay, settings.ExitFadeDuration, settings.EntryFadeTween, settings.ExitFadeTween, settings.ProgressBarSpeed, settings.FadeMode, settings.UnloadMethod, settings.AntiSpillSceneName, settings.SpeedIntervals);
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x00023508 File Offset: 0x00021708
		public static void LoadScene(string sceneToLoadName, string loadingSceneName = "MMAdditiveLoadingScreen", ThreadPriority threadPriority = ThreadPriority.High, bool secureLoad = true, bool interpolateProgress = true, float beforeEntryFadeDelay = 0f, float entryFadeDuration = 0.25f, float afterEntryFadeDelay = 0.1f, float beforeExitFadeDelay = 0.25f, float exitFadeDuration = 0.2f, MMTweenType entryFadeTween = null, MMTweenType exitFadeTween = null, float progressBarSpeed = 5f, MMAdditiveSceneLoadingManager.FadeModes fadeMode = MMAdditiveSceneLoadingManager.FadeModes.FadeInThenOut, MMAdditiveSceneLoadingManagerSettings.UnloadMethods unloadMethod = MMAdditiveSceneLoadingManagerSettings.UnloadMethods.AllScenes, string antiSpillSceneName = "", List<MMSceneLoadingSpeedInterval> speedIntervals = null)
		{
			if (MMAdditiveSceneLoadingManager._loadingInProgress)
			{
				Debug.LogError("MMLoadingSceneManagerAdditive : a request to load a new scene was emitted while a scene load was already in progress");
				return;
			}
			if (entryFadeTween == null)
			{
				entryFadeTween = new MMTweenType(MMTween.MMTweenCurve.EaseInOutCubic);
			}
			if (exitFadeTween == null)
			{
				exitFadeTween = new MMTweenType(MMTween.MMTweenCurve.EaseInOutCubic);
			}
			if (secureLoad)
			{
				MMAdditiveSceneLoadingManager._scenesInBuild = MMScene.GetScenesInBuild();
				if (!MMAdditiveSceneLoadingManager._scenesInBuild.Contains(sceneToLoadName))
				{
					Debug.LogError("MMLoadingSceneManagerAdditive : impossible to load the '" + sceneToLoadName + "' scene, there is no such scene in the project's build settings.");
					return;
				}
				if (!MMAdditiveSceneLoadingManager._scenesInBuild.Contains(loadingSceneName))
				{
					Debug.LogError("MMLoadingSceneManagerAdditive : impossible to load the '" + loadingSceneName + "' scene, there is no such scene in the project's build settings.");
					return;
				}
			}
			MMAdditiveSceneLoadingManager._loadingInProgress = true;
			MMAdditiveSceneLoadingManager._initialScenes = MMAdditiveSceneLoadingManager.GetScenesToUnload(unloadMethod);
			Application.backgroundLoadingPriority = threadPriority;
			MMAdditiveSceneLoadingManager._sceneToLoadName = sceneToLoadName;
			MMAdditiveSceneLoadingManager._loadingScreenSceneName = loadingSceneName;
			MMAdditiveSceneLoadingManager._beforeEntryFadeDelay = beforeEntryFadeDelay;
			MMAdditiveSceneLoadingManager._entryFadeDuration = entryFadeDuration;
			MMAdditiveSceneLoadingManager._entryFadeTween = entryFadeTween;
			MMAdditiveSceneLoadingManager._afterEntryFadeDelay = afterEntryFadeDelay;
			MMAdditiveSceneLoadingManager._progressInterpolationSpeed = progressBarSpeed;
			MMAdditiveSceneLoadingManager._beforeExitFadeDelay = beforeExitFadeDelay;
			MMAdditiveSceneLoadingManager._exitFadeDuration = exitFadeDuration;
			MMAdditiveSceneLoadingManager._exitFadeTween = exitFadeTween;
			MMAdditiveSceneLoadingManager._fadeMode = fadeMode;
			MMAdditiveSceneLoadingManager._interpolateProgress = interpolateProgress;
			MMAdditiveSceneLoadingManager._antiSpillSceneName = antiSpillSceneName;
			MMAdditiveSceneLoadingManager._speedIntervals = speedIntervals;
			SceneManager.LoadScene(MMAdditiveSceneLoadingManager._loadingScreenSceneName, LoadSceneMode.Additive);
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x00023618 File Offset: 0x00021818
		private static Scene[] GetScenesToUnload(MMAdditiveSceneLoadingManagerSettings.UnloadMethods unloaded)
		{
			switch (unloaded)
			{
			case MMAdditiveSceneLoadingManagerSettings.UnloadMethods.None:
				MMAdditiveSceneLoadingManager._initialScenes = new Scene[0];
				goto IL_0044;
			case MMAdditiveSceneLoadingManagerSettings.UnloadMethods.ActiveScene:
				MMAdditiveSceneLoadingManager._initialScenes = new Scene[] { SceneManager.GetActiveScene() };
				goto IL_0044;
			}
			MMAdditiveSceneLoadingManager._initialScenes = MMScene.GetLoadedScenes();
			IL_0044:
			return MMAdditiveSceneLoadingManager._initialScenes;
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x0002366E File Offset: 0x0002186E
		protected virtual void Awake()
		{
			this.Initialization();
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x00023678 File Offset: 0x00021878
		protected virtual void Initialization()
		{
			this.MMLoadingSceneDebug("MMLoadingSceneManagerAdditive : Initialization");
			if (this.DebugMode)
			{
				foreach (Scene scene in MMAdditiveSceneLoadingManager._initialScenes)
				{
					this.MMLoadingSceneDebug("MMLoadingSceneManagerAdditive : Initial scene : " + scene.name);
				}
			}
			this._setRealtimeProgressValueIsNull = this.SetRealtimeProgressValue == null;
			this._setInterpolatedProgressValueIsNull = this.SetInterpolatedProgressValue == null;
			Time.timeScale = 1f;
			if (MMAdditiveSceneLoadingManager._sceneToLoadName == "" || MMAdditiveSceneLoadingManager._loadingScreenSceneName == "")
			{
				return;
			}
			base.StartCoroutine(this.LoadSequence());
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x00023723 File Offset: 0x00021923
		protected virtual void Update()
		{
			this.UpdateProgress();
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x0002372C File Offset: 0x0002192C
		protected virtual void UpdateProgress()
		{
			if (!this._setRealtimeProgressValueIsNull)
			{
				this.SetRealtimeProgressValue.Invoke(this._loadProgress);
			}
			if (MMAdditiveSceneLoadingManager._interpolateProgress)
			{
				this._interpolatedLoadProgress = MMMaths.Approach(this._interpolatedLoadProgress, this._loadProgress, Time.unscaledDeltaTime * MMAdditiveSceneLoadingManager.ComputeInterpolationSpeed(this._interpolatedLoadProgress));
				if (!this._setInterpolatedProgressValueIsNull)
				{
					this.SetInterpolatedProgressValue.Invoke(this._interpolatedLoadProgress);
					return;
				}
			}
			else
			{
				this.SetInterpolatedProgressValue.Invoke(this._loadProgress);
			}
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x000237AC File Offset: 0x000219AC
		public static float ComputeInterpolationSpeed(float t)
		{
			if (MMAdditiveSceneLoadingManager._speedIntervals != null && MMAdditiveSceneLoadingManager._speedIntervals.Count > 0)
			{
				foreach (MMSceneLoadingSpeedInterval mmsceneLoadingSpeedInterval in MMAdditiveSceneLoadingManager._speedIntervals)
				{
					if (mmsceneLoadingSpeedInterval.Interval.Contains(t))
					{
						return mmsceneLoadingSpeedInterval.Speed;
					}
				}
			}
			return MMAdditiveSceneLoadingManager._progressInterpolationSpeed;
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x0002382C File Offset: 0x00021A2C
		protected virtual IEnumerator LoadSequence()
		{
			MMSceneLoadingAntiSpill antiSpill = this._antiSpill;
			if (antiSpill != null)
			{
				antiSpill.PrepareAntiFill(MMAdditiveSceneLoadingManager._sceneToLoadName, MMAdditiveSceneLoadingManager._antiSpillSceneName);
			}
			this.InitiateLoad();
			yield return this.ProcessDelayBeforeEntryFade();
			yield return this.EntryFade();
			yield return this.ProcessDelayAfterEntryFade();
			yield return this.UnloadOriginScenes();
			yield return this.LoadDestinationScene();
			yield return this.ProcessDelayBeforeExitFade();
			yield return this.DestinationSceneActivation();
			yield return this.ExitFade();
			yield return this.UnloadSceneLoader();
			yield break;
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x0002383C File Offset: 0x00021A3C
		protected virtual void InitiateLoad()
		{
			this._loadProgress = 0f;
			this._interpolatedLoadProgress = 0f;
			Time.timeScale = 1f;
			this.SetAudioListener(false);
			this.MMLoadingSceneDebug("MMLoadingSceneManagerAdditive : Initiate Load");
			MMSceneLoadingManager.LoadingSceneEvent.Trigger(MMAdditiveSceneLoadingManager._sceneToLoadName, MMSceneLoadingManager.LoadingStatus.LoadStarted);
			UnityEvent onLoadStarted = this.OnLoadStarted;
			if (onLoadStarted == null)
			{
				return;
			}
			onLoadStarted.Invoke();
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x00023896 File Offset: 0x00021A96
		protected virtual IEnumerator ProcessDelayBeforeEntryFade()
		{
			if (MMAdditiveSceneLoadingManager._beforeEntryFadeDelay > 0f)
			{
				this.MMLoadingSceneDebug("MMLoadingSceneManagerAdditive : delay before entry fade, duration : " + MMAdditiveSceneLoadingManager._beforeEntryFadeDelay.ToString());
				MMSceneLoadingManager.LoadingSceneEvent.Trigger(MMAdditiveSceneLoadingManager._sceneToLoadName, MMSceneLoadingManager.LoadingStatus.BeforeEntryFade);
				UnityEvent onBeforeEntryFade = this.OnBeforeEntryFade;
				if (onBeforeEntryFade != null)
				{
					onBeforeEntryFade.Invoke();
				}
				yield return MMCoroutine.WaitForUnscaled(MMAdditiveSceneLoadingManager._beforeEntryFadeDelay);
			}
			yield break;
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x000238A5 File Offset: 0x00021AA5
		protected virtual IEnumerator EntryFade()
		{
			if (MMAdditiveSceneLoadingManager._entryFadeDuration > 0f)
			{
				this.MMLoadingSceneDebug("MMLoadingSceneManagerAdditive : entry fade, duration : " + MMAdditiveSceneLoadingManager._entryFadeDuration.ToString());
				MMSceneLoadingManager.LoadingSceneEvent.Trigger(MMAdditiveSceneLoadingManager._sceneToLoadName, MMSceneLoadingManager.LoadingStatus.EntryFade);
				UnityEvent onEntryFade = this.OnEntryFade;
				if (onEntryFade != null)
				{
					onEntryFade.Invoke();
				}
				if (MMAdditiveSceneLoadingManager._fadeMode == MMAdditiveSceneLoadingManager.FadeModes.FadeOutThenIn)
				{
					yield return null;
					MMFadeOutEvent.Trigger(MMAdditiveSceneLoadingManager._entryFadeDuration, MMAdditiveSceneLoadingManager._entryFadeTween, this.FaderID, true, default(Vector3));
				}
				else
				{
					yield return null;
					MMFadeInEvent.Trigger(MMAdditiveSceneLoadingManager._entryFadeDuration, MMAdditiveSceneLoadingManager._entryFadeTween, this.FaderID, true, default(Vector3));
				}
				yield return MMCoroutine.WaitForUnscaled(MMAdditiveSceneLoadingManager._entryFadeDuration);
			}
			yield break;
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x000238B4 File Offset: 0x00021AB4
		protected virtual IEnumerator ProcessDelayAfterEntryFade()
		{
			if (MMAdditiveSceneLoadingManager._afterEntryFadeDelay > 0f)
			{
				this.MMLoadingSceneDebug("MMLoadingSceneManagerAdditive : delay after entry fade, duration : " + MMAdditiveSceneLoadingManager._afterEntryFadeDelay.ToString());
				MMSceneLoadingManager.LoadingSceneEvent.Trigger(MMAdditiveSceneLoadingManager._sceneToLoadName, MMSceneLoadingManager.LoadingStatus.AfterEntryFade);
				UnityEvent onAfterEntryFade = this.OnAfterEntryFade;
				if (onAfterEntryFade != null)
				{
					onAfterEntryFade.Invoke();
				}
				yield return MMCoroutine.WaitForUnscaled(MMAdditiveSceneLoadingManager._afterEntryFadeDelay);
			}
			yield break;
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x000238C3 File Offset: 0x00021AC3
		protected virtual IEnumerator UnloadOriginScenes()
		{
			foreach (Scene scene in MMAdditiveSceneLoadingManager._initialScenes)
			{
				this.MMLoadingSceneDebug("MMLoadingSceneManagerAdditive : unload scene " + scene.name);
				MMSceneLoadingManager.LoadingSceneEvent.Trigger(MMAdditiveSceneLoadingManager._sceneToLoadName, MMSceneLoadingManager.LoadingStatus.UnloadOriginScene);
				UnityEvent onUnloadOriginScene = this.OnUnloadOriginScene;
				if (onUnloadOriginScene != null)
				{
					onUnloadOriginScene.Invoke();
				}
				if (!scene.IsValid() || !scene.isLoaded)
				{
					Debug.LogWarning("MMLoadingSceneManagerAdditive : invalid scene : " + scene.name);
				}
				else
				{
					this._unloadOriginAsyncOperation = SceneManager.UnloadSceneAsync(scene);
					this.SetAudioListener(true);
					while (this._unloadOriginAsyncOperation.progress < 0.9f)
					{
						yield return null;
					}
				}
			}
			Scene[] array = null;
			yield break;
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x000238D2 File Offset: 0x00021AD2
		protected virtual IEnumerator LoadDestinationScene()
		{
			this.MMLoadingSceneDebug("MMLoadingSceneManagerAdditive : load destination scene");
			MMSceneLoadingManager.LoadingSceneEvent.Trigger(MMAdditiveSceneLoadingManager._sceneToLoadName, MMSceneLoadingManager.LoadingStatus.LoadDestinationScene);
			UnityEvent onLoadDestinationScene = this.OnLoadDestinationScene;
			if (onLoadDestinationScene != null)
			{
				onLoadDestinationScene.Invoke();
			}
			this._loadDestinationAsyncOperation = SceneManager.LoadSceneAsync(MMAdditiveSceneLoadingManager._sceneToLoadName, LoadSceneMode.Additive);
			this._loadDestinationAsyncOperation.completed += this.OnLoadOperationComplete;
			this._loadDestinationAsyncOperation.allowSceneActivation = false;
			while (this._loadDestinationAsyncOperation.progress < 0.9f)
			{
				this._loadProgress = this._loadDestinationAsyncOperation.progress;
				yield return null;
			}
			this.MMLoadingSceneDebug("MMLoadingSceneManagerAdditive : load progress complete");
			MMSceneLoadingManager.LoadingSceneEvent.Trigger(MMAdditiveSceneLoadingManager._sceneToLoadName, MMSceneLoadingManager.LoadingStatus.LoadProgressComplete);
			UnityEvent onLoadProgressComplete = this.OnLoadProgressComplete;
			if (onLoadProgressComplete != null)
			{
				onLoadProgressComplete.Invoke();
			}
			this._loadProgress = 1f;
			if (MMAdditiveSceneLoadingManager._interpolateProgress)
			{
				while (this._interpolatedLoadProgress < 1f)
				{
					yield return null;
				}
			}
			this.MMLoadingSceneDebug("MMLoadingSceneManagerAdditive : interpolated load complete");
			MMSceneLoadingManager.LoadingSceneEvent.Trigger(MMAdditiveSceneLoadingManager._sceneToLoadName, MMSceneLoadingManager.LoadingStatus.InterpolatedLoadProgressComplete);
			UnityEvent onInterpolatedLoadProgressComplete = this.OnInterpolatedLoadProgressComplete;
			if (onInterpolatedLoadProgressComplete != null)
			{
				onInterpolatedLoadProgressComplete.Invoke();
			}
			yield break;
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x000238E1 File Offset: 0x00021AE1
		protected virtual IEnumerator ProcessDelayBeforeExitFade()
		{
			if (MMAdditiveSceneLoadingManager._beforeExitFadeDelay > 0f)
			{
				this.MMLoadingSceneDebug("MMLoadingSceneManagerAdditive : delay before exit fade, duration : " + MMAdditiveSceneLoadingManager._beforeExitFadeDelay.ToString());
				MMSceneLoadingManager.LoadingSceneEvent.Trigger(MMAdditiveSceneLoadingManager._sceneToLoadName, MMSceneLoadingManager.LoadingStatus.BeforeExitFade);
				UnityEvent onBeforeExitFade = this.OnBeforeExitFade;
				if (onBeforeExitFade != null)
				{
					onBeforeExitFade.Invoke();
				}
				yield return MMCoroutine.WaitForUnscaled(MMAdditiveSceneLoadingManager._beforeExitFadeDelay);
			}
			yield break;
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x000238F0 File Offset: 0x00021AF0
		protected virtual IEnumerator ExitFade()
		{
			this.SetAudioListener(false);
			if (MMAdditiveSceneLoadingManager._exitFadeDuration > 0f)
			{
				this.MMLoadingSceneDebug("MMLoadingSceneManagerAdditive : exit fade, duration : " + MMAdditiveSceneLoadingManager._exitFadeDuration.ToString());
				MMSceneLoadingManager.LoadingSceneEvent.Trigger(MMAdditiveSceneLoadingManager._sceneToLoadName, MMSceneLoadingManager.LoadingStatus.ExitFade);
				UnityEvent onExitFade = this.OnExitFade;
				if (onExitFade != null)
				{
					onExitFade.Invoke();
				}
				if (MMAdditiveSceneLoadingManager._fadeMode == MMAdditiveSceneLoadingManager.FadeModes.FadeOutThenIn)
				{
					MMFadeInEvent.Trigger(MMAdditiveSceneLoadingManager._exitFadeDuration, MMAdditiveSceneLoadingManager._exitFadeTween, this.FaderID, true, default(Vector3));
				}
				else
				{
					MMFadeOutEvent.Trigger(MMAdditiveSceneLoadingManager._exitFadeDuration, MMAdditiveSceneLoadingManager._exitFadeTween, this.FaderID, true, default(Vector3));
				}
				yield return MMCoroutine.WaitForUnscaled(MMAdditiveSceneLoadingManager._exitFadeDuration);
			}
			yield break;
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x000238FF File Offset: 0x00021AFF
		protected virtual IEnumerator DestinationSceneActivation()
		{
			yield return MMCoroutine.WaitForFrames(1);
			this._loadDestinationAsyncOperation.allowSceneActivation = true;
			while (this._loadDestinationAsyncOperation.progress < 1f)
			{
				yield return null;
			}
			this.MMLoadingSceneDebug("MMLoadingSceneManagerAdditive : activating destination scene");
			MMSceneLoadingManager.LoadingSceneEvent.Trigger(MMAdditiveSceneLoadingManager._sceneToLoadName, MMSceneLoadingManager.LoadingStatus.DestinationSceneActivation);
			UnityEvent onDestinationSceneActivation = this.OnDestinationSceneActivation;
			if (onDestinationSceneActivation != null)
			{
				onDestinationSceneActivation.Invoke();
			}
			yield break;
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x0002390E File Offset: 0x00021B0E
		protected virtual void OnLoadOperationComplete(AsyncOperation obj)
		{
			SceneManager.SetActiveScene(SceneManager.GetSceneByName(MMAdditiveSceneLoadingManager._sceneToLoadName));
			this.MMLoadingSceneDebug("MMLoadingSceneManagerAdditive : set active scene to " + MMAdditiveSceneLoadingManager._sceneToLoadName);
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x00023935 File Offset: 0x00021B35
		protected virtual IEnumerator UnloadSceneLoader()
		{
			this.MMLoadingSceneDebug("MMLoadingSceneManagerAdditive : unloading scene loader");
			MMSceneLoadingManager.LoadingSceneEvent.Trigger(MMAdditiveSceneLoadingManager._sceneToLoadName, MMSceneLoadingManager.LoadingStatus.UnloadSceneLoader);
			UnityEvent onUnloadSceneLoader = this.OnUnloadSceneLoader;
			if (onUnloadSceneLoader != null)
			{
				onUnloadSceneLoader.Invoke();
			}
			yield return null;
			this._unloadLoadingAsyncOperation = SceneManager.UnloadSceneAsync(MMAdditiveSceneLoadingManager._loadingScreenSceneName);
			while (this._unloadLoadingAsyncOperation.progress < 0.9f)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x00023944 File Offset: 0x00021B44
		protected virtual void SetAudioListener(bool state)
		{
			this.LoadingAudioListener != null;
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x00023953 File Offset: 0x00021B53
		protected virtual void OnDestroy()
		{
			MMAdditiveSceneLoadingManager._loadingInProgress = false;
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x0002395C File Offset: 0x00021B5C
		protected virtual void MMLoadingSceneDebug(string message)
		{
			if (!this.DebugMode)
			{
				return;
			}
			MMDebug.DebugLogInfo("" + "<color=#82d3f9>[" + Time.frameCount.ToString() + "]</color> " + "<color=#f9a682>[" + MMTime.FloatToTimeString(Time.time, false, true, true, true) + "]</color> " + message, "", 3, true);
		}

		// Token: 0x040008E0 RID: 2272
		[MMInspectorGroup("Audio Listener", true, 3, false)]
		public AudioListener LoadingAudioListener;

		// Token: 0x040008E1 RID: 2273
		[MMInspectorGroup("Settings", true, 10, false)]
		[Tooltip("the ID on which to trigger a fade, has to match the ID on the fader in your scene")]
		public int FaderID = 500;

		// Token: 0x040008E2 RID: 2274
		[Tooltip("whether or not to output debug messages to the console")]
		public bool DebugMode;

		// Token: 0x040008E3 RID: 2275
		[MMInspectorGroup("Progress Events", true, 11, false)]
		[Tooltip("an event used to update progress")]
		public ProgressEvent SetRealtimeProgressValue;

		// Token: 0x040008E4 RID: 2276
		[Tooltip("an event used to update progress with interpolation")]
		public ProgressEvent SetInterpolatedProgressValue;

		// Token: 0x040008E5 RID: 2277
		[MMInspectorGroup("State Events", true, 12, false)]
		[Tooltip("an event that will be invoked when the load starts")]
		public UnityEvent OnLoadStarted;

		// Token: 0x040008E6 RID: 2278
		[Tooltip("an event that will be invoked when the delay before the entry fade starts")]
		public UnityEvent OnBeforeEntryFade;

		// Token: 0x040008E7 RID: 2279
		[Tooltip("an event that will be invoked when the entry fade starts")]
		public UnityEvent OnEntryFade;

		// Token: 0x040008E8 RID: 2280
		[Tooltip("an event that will be invoked when the delay after the entry fade starts")]
		public UnityEvent OnAfterEntryFade;

		// Token: 0x040008E9 RID: 2281
		[Tooltip("an event that will be invoked when the origin scene gets unloaded")]
		public UnityEvent OnUnloadOriginScene;

		// Token: 0x040008EA RID: 2282
		[Tooltip("an event that will be invoked when the destination scene starts loading")]
		public UnityEvent OnLoadDestinationScene;

		// Token: 0x040008EB RID: 2283
		[Tooltip("an event that will be invoked when the load of the destination scene is complete")]
		public UnityEvent OnLoadProgressComplete;

		// Token: 0x040008EC RID: 2284
		[Tooltip("an event that will be invoked when the interpolated load of the destination scene is complete")]
		public UnityEvent OnInterpolatedLoadProgressComplete;

		// Token: 0x040008ED RID: 2285
		[Tooltip("an event that will be invoked when the delay before the exit fade starts")]
		public UnityEvent OnBeforeExitFade;

		// Token: 0x040008EE RID: 2286
		[Tooltip("an event that will be invoked when the exit fade starts")]
		public UnityEvent OnExitFade;

		// Token: 0x040008EF RID: 2287
		[Tooltip("an event that will be invoked when the destination scene gets activated")]
		public UnityEvent OnDestinationSceneActivation;

		// Token: 0x040008F0 RID: 2288
		[Tooltip("an event that will be invoked when the scene loader gets unloaded")]
		public UnityEvent OnUnloadSceneLoader;

		// Token: 0x040008F1 RID: 2289
		protected static bool _interpolateProgress;

		// Token: 0x040008F2 RID: 2290
		protected static float _progressInterpolationSpeed;

		// Token: 0x040008F3 RID: 2291
		protected static List<MMSceneLoadingSpeedInterval> _speedIntervals;

		// Token: 0x040008F4 RID: 2292
		protected static float _beforeEntryFadeDelay;

		// Token: 0x040008F5 RID: 2293
		protected static MMTweenType _entryFadeTween;

		// Token: 0x040008F6 RID: 2294
		protected static float _entryFadeDuration;

		// Token: 0x040008F7 RID: 2295
		protected static float _afterEntryFadeDelay;

		// Token: 0x040008F8 RID: 2296
		protected static float _beforeExitFadeDelay;

		// Token: 0x040008F9 RID: 2297
		protected static MMTweenType _exitFadeTween;

		// Token: 0x040008FA RID: 2298
		protected static float _exitFadeDuration;

		// Token: 0x040008FB RID: 2299
		protected static MMAdditiveSceneLoadingManager.FadeModes _fadeMode;

		// Token: 0x040008FC RID: 2300
		protected static string _sceneToLoadName = "";

		// Token: 0x040008FD RID: 2301
		protected static string _loadingScreenSceneName;

		// Token: 0x040008FE RID: 2302
		protected static List<string> _scenesInBuild;

		// Token: 0x040008FF RID: 2303
		protected static Scene[] _initialScenes;

		// Token: 0x04000900 RID: 2304
		protected float _loadProgress;

		// Token: 0x04000901 RID: 2305
		protected float _interpolatedLoadProgress;

		// Token: 0x04000902 RID: 2306
		protected static bool _loadingInProgress = false;

		// Token: 0x04000903 RID: 2307
		protected AsyncOperation _unloadOriginAsyncOperation;

		// Token: 0x04000904 RID: 2308
		protected AsyncOperation _loadDestinationAsyncOperation;

		// Token: 0x04000905 RID: 2309
		protected AsyncOperation _unloadLoadingAsyncOperation;

		// Token: 0x04000906 RID: 2310
		protected bool _setRealtimeProgressValueIsNull;

		// Token: 0x04000907 RID: 2311
		protected bool _setInterpolatedProgressValueIsNull;

		// Token: 0x04000908 RID: 2312
		protected const float _asyncProgressLimit = 0.9f;

		// Token: 0x04000909 RID: 2313
		protected MMSceneLoadingAntiSpill _antiSpill = new MMSceneLoadingAntiSpill();

		// Token: 0x0400090A RID: 2314
		protected static string _antiSpillSceneName = "";

		// Token: 0x0200044A RID: 1098
		public enum FadeModes
		{
			// Token: 0x04001FE9 RID: 8169
			FadeInThenOut,
			// Token: 0x04001FEA RID: 8170
			FadeOutThenIn
		}
	}
}
