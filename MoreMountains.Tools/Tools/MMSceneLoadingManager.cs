using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MoreMountains.Tools
{
	// Token: 0x0200014F RID: 335
	public class MMSceneLoadingManager : MonoBehaviour
	{
		// Token: 0x06000768 RID: 1896 RVA: 0x00023C2D File Offset: 0x00021E2D
		public static void LoadScene(string sceneToLoad)
		{
			MMSceneLoadingManager._sceneToLoad = sceneToLoad;
			Application.backgroundLoadingPriority = ThreadPriority.High;
			if (MMSceneLoadingManager.LoadingScreenSceneName != null)
			{
				MMSceneLoadingManager.LoadingSceneEvent.Trigger(sceneToLoad, MMSceneLoadingManager.LoadingStatus.LoadStarted);
				SceneManager.LoadScene(MMSceneLoadingManager.LoadingScreenSceneName);
			}
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x00023C53 File Offset: 0x00021E53
		public static void LoadScene(string sceneToLoad, string loadingSceneName)
		{
			MMSceneLoadingManager._sceneToLoad = sceneToLoad;
			Application.backgroundLoadingPriority = ThreadPriority.High;
			SceneManager.LoadScene(loadingSceneName);
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x00023C68 File Offset: 0x00021E68
		protected virtual void Start()
		{
			MMSceneLoadingManager._tween = new MMTweenType(MMTween.MMTweenCurve.EaseOutCubic);
			this._progressBarImage = this.LoadingProgressBar.GetComponent<Image>();
			this._loadingTextValue = this.LoadingText.text;
			if (!string.IsNullOrEmpty(MMSceneLoadingManager._sceneToLoad))
			{
				base.StartCoroutine(this.LoadAsynchronously());
			}
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x00023CBB File Offset: 0x00021EBB
		protected virtual void Update()
		{
			Time.timeScale = 1f;
			this._progressBarImage.fillAmount = MMMaths.Approach(this._progressBarImage.fillAmount, this._fillTarget, Time.deltaTime * this.ProgressBarSpeed);
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x00023CF4 File Offset: 0x00021EF4
		protected virtual IEnumerator LoadAsynchronously()
		{
			this.LoadingSetup();
			MMFadeOutEvent.Trigger(this.StartFadeDuration, MMSceneLoadingManager._tween, 0, true, default(Vector3));
			yield return new WaitForSeconds(this.StartFadeDuration);
			this._asyncOperation = SceneManager.LoadSceneAsync(MMSceneLoadingManager._sceneToLoad, LoadSceneMode.Single);
			this._asyncOperation.allowSceneActivation = false;
			while (this._asyncOperation.progress < 0.9f)
			{
				this._fillTarget = this._asyncOperation.progress;
				yield return null;
			}
			this._fillTarget = 1f;
			while (this._progressBarImage.fillAmount != this._fillTarget)
			{
				yield return null;
			}
			this.LoadingComplete();
			yield return new WaitForSeconds(this.LoadCompleteDelay);
			MMFadeInEvent.Trigger(this.ExitFadeDuration, MMSceneLoadingManager._tween, 0, true, default(Vector3));
			yield return new WaitForSeconds(this.ExitFadeDuration);
			this._asyncOperation.allowSceneActivation = true;
			MMSceneLoadingManager.LoadingSceneEvent.Trigger(MMSceneLoadingManager._sceneToLoad, MMSceneLoadingManager.LoadingStatus.LoadTransitionComplete);
			yield break;
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x00023D03 File Offset: 0x00021F03
		protected virtual void LoadingSetup()
		{
			this.LoadingCompleteAnimation.alpha = 0f;
			this._progressBarImage.fillAmount = 0f;
			this.LoadingText.text = this._loadingTextValue;
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x00023D38 File Offset: 0x00021F38
		protected virtual void LoadingComplete()
		{
			MMSceneLoadingManager.LoadingSceneEvent.Trigger(MMSceneLoadingManager._sceneToLoad, MMSceneLoadingManager.LoadingStatus.InterpolatedLoadProgressComplete);
			this.LoadingCompleteAnimation.gameObject.SetActive(true);
			base.StartCoroutine(MMFade.FadeCanvasGroup(this.LoadingProgressBar, 0.1f, 0f, true));
			base.StartCoroutine(MMFade.FadeCanvasGroup(this.LoadingAnimation, 0.1f, 0f, true));
			base.StartCoroutine(MMFade.FadeCanvasGroup(this.LoadingCompleteAnimation, 0.1f, 1f, true));
		}

		// Token: 0x04000912 RID: 2322
		[Header("Binding")]
		public static string LoadingScreenSceneName = "LoadingScreen";

		// Token: 0x04000913 RID: 2323
		[Header("GameObjects")]
		public Text LoadingText;

		// Token: 0x04000914 RID: 2324
		public CanvasGroup LoadingProgressBar;

		// Token: 0x04000915 RID: 2325
		public CanvasGroup LoadingAnimation;

		// Token: 0x04000916 RID: 2326
		public CanvasGroup LoadingCompleteAnimation;

		// Token: 0x04000917 RID: 2327
		[Header("Time")]
		public float StartFadeDuration = 0.2f;

		// Token: 0x04000918 RID: 2328
		public float ProgressBarSpeed = 2f;

		// Token: 0x04000919 RID: 2329
		public float ExitFadeDuration = 0.2f;

		// Token: 0x0400091A RID: 2330
		public float LoadCompleteDelay = 0.5f;

		// Token: 0x0400091B RID: 2331
		protected AsyncOperation _asyncOperation;

		// Token: 0x0400091C RID: 2332
		protected static string _sceneToLoad = "";

		// Token: 0x0400091D RID: 2333
		protected float _fadeDuration = 0.5f;

		// Token: 0x0400091E RID: 2334
		protected float _fillTarget;

		// Token: 0x0400091F RID: 2335
		protected string _loadingTextValue;

		// Token: 0x04000920 RID: 2336
		protected Image _progressBarImage;

		// Token: 0x04000921 RID: 2337
		protected static MMTweenType _tween;

		// Token: 0x02000455 RID: 1109
		public enum LoadingStatus
		{
			// Token: 0x0400200C RID: 8204
			LoadStarted,
			// Token: 0x0400200D RID: 8205
			BeforeEntryFade,
			// Token: 0x0400200E RID: 8206
			EntryFade,
			// Token: 0x0400200F RID: 8207
			AfterEntryFade,
			// Token: 0x04002010 RID: 8208
			UnloadOriginScene,
			// Token: 0x04002011 RID: 8209
			LoadDestinationScene,
			// Token: 0x04002012 RID: 8210
			LoadProgressComplete,
			// Token: 0x04002013 RID: 8211
			InterpolatedLoadProgressComplete,
			// Token: 0x04002014 RID: 8212
			BeforeExitFade,
			// Token: 0x04002015 RID: 8213
			ExitFade,
			// Token: 0x04002016 RID: 8214
			DestinationSceneActivation,
			// Token: 0x04002017 RID: 8215
			UnloadSceneLoader,
			// Token: 0x04002018 RID: 8216
			LoadTransitionComplete
		}

		// Token: 0x02000456 RID: 1110
		public struct LoadingSceneEvent
		{
			// Token: 0x06001A73 RID: 6771 RVA: 0x0006D6D8 File Offset: 0x0006B8D8
			public LoadingSceneEvent(string sceneName, MMSceneLoadingManager.LoadingStatus status)
			{
				this.Status = status;
				this.SceneName = sceneName;
			}

			// Token: 0x06001A74 RID: 6772 RVA: 0x0006D6E8 File Offset: 0x0006B8E8
			public static void Trigger(string sceneName, MMSceneLoadingManager.LoadingStatus status)
			{
				MMSceneLoadingManager.LoadingSceneEvent.e.Status = status;
				MMSceneLoadingManager.LoadingSceneEvent.e.SceneName = sceneName;
				MMEventManager.TriggerEvent<MMSceneLoadingManager.LoadingSceneEvent>(MMSceneLoadingManager.LoadingSceneEvent.e);
			}

			// Token: 0x04002019 RID: 8217
			public MMSceneLoadingManager.LoadingStatus Status;

			// Token: 0x0400201A RID: 8218
			public string SceneName;

			// Token: 0x0400201B RID: 8219
			private static MMSceneLoadingManager.LoadingSceneEvent e;
		}
	}
}
