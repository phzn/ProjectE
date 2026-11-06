using System;
using System.Collections;
using Features.Localization;
using Features.MainMenu;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.GameWarmUp
{
	// Token: 0x020011C9 RID: 4553
	public sealed class GameWarmUpSequenceController : MonoBehaviour
	{
		// Token: 0x1700164F RID: 5711
		// (get) Token: 0x06008BDE RID: 35806 RVA: 0x00392BC2 File Offset: 0x00390DC2
		// (set) Token: 0x06008BDF RID: 35807 RVA: 0x00392BCA File Offset: 0x00390DCA
		public string CurrentPhase { get; private set; } = "Not Started";

		// Token: 0x17001650 RID: 5712
		// (get) Token: 0x06008BE0 RID: 35808 RVA: 0x00392BD3 File Offset: 0x00390DD3
		// (set) Token: 0x06008BE1 RID: 35809 RVA: 0x00392BDB File Offset: 0x00390DDB
		public bool SequenceComplete { get; private set; }

		// Token: 0x06008BE2 RID: 35810 RVA: 0x00392BE4 File Offset: 0x00390DE4
		private void Awake()
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
			GameWarmUpSequenceController.SetGroup(this.companyScreen, 1f, true);
			GameWarmUpSequenceController.SetGroup(this.companyLogo, 0f, false);
			GameWarmUpSequenceController.SetGroup(this.disclaimerScreen, 0f, false);
			GameWarmUpSequenceController.SetGroup(this.blackCurtain, 0f, false);
			if (this.loadingLabel != null)
			{
				LocalizedGameText.BindComputed(this.loadingLabel, () => GameLocalization.FormatSource("startup_ui", "PREPARING THE STAGE{0}", new object[]
				{
					new string('.', this._loadingDots)
				}));
				this.loadingLabel.gameObject.SetActive(false);
			}
			this.PrepareStudioAudio();
		}

		// Token: 0x06008BE3 RID: 35811 RVA: 0x00392C7E File Offset: 0x00390E7E
		private IEnumerator Start()
		{
			yield return this.RunSequence();
			yield break;
		}

		// Token: 0x06008BE4 RID: 35812 RVA: 0x00392C8D File Offset: 0x00390E8D
		private IEnumerator RunSequence()
		{
			float startedAt = Time.realtimeSinceStartup;
			base.StartCoroutine(this.PreloadMainMenuWhenLocalizationReady());
			this.CurrentPhase = "JoystickyLabs";
			yield return this.RevealCompanyLogo();
			yield return new WaitForSecondsRealtime(this.logoHold);
			yield return GameWarmUpSequenceController.FadeGroup(this.companyScreen, 1f, 0f, this.companyFadeOut, false);
			if (this.studioChimeSource != null)
			{
				this.studioChimeSource.Stop();
			}
			this.CurrentPhase = "Disclaimer";
			IntroMenuMusic.Begin(this.mainMenuMusic, this.menuMusicVolume * Mathf.SmoothStep(0f, 1f, 0.35f), this.musicLoopStart, this.musicLoopEnd, this.musicLoopCrossfade);
			GameWarmUpSequenceController.SetGroup(this.companyScreen, 0f, false);
			yield return GameWarmUpSequenceController.FadeGroup(this.disclaimerScreen, 0f, 1f, this.disclaimerFadeIn, true);
			yield return new WaitForSecondsRealtime(this.disclaimerHold);
			if ((!this._menuPreloadStarted || (this._menuPreload != null && this._menuPreload.progress < 0.9f)) && this.loadingLabel != null)
			{
				this.loadingLabel.gameObject.SetActive(true);
			}
			float nextDotChange = 0f;
			int dots = 1;
			while (!this._menuPreloadStarted || (this._menuPreload != null && this._menuPreload.progress < 0.9f))
			{
				if (this.loadingLabel != null && Time.unscaledTime >= nextDotChange)
				{
					this._loadingDots = dots;
					this.loadingLabel.GetComponent<LocalizedGameText>().Refresh();
					dots = dots % 3 + 1;
					nextDotChange = Time.unscaledTime + 0.35f;
				}
				yield return null;
			}
			if (this.loadingLabel != null)
			{
				this.loadingLabel.gameObject.SetActive(false);
			}
			AsyncOperation preload = this._menuPreload;
			if (preload == null)
			{
				Debug.LogError("[GameWarmUp] Could not start loading scene '" + this.mainMenuSceneName + "'.");
				yield break;
			}
			yield return GameWarmUpSequenceController.FadeGroup(this.disclaimerScreen, 1f, 0f, this.disclaimerFadeOut, false);
			GameWarmUpSequenceController.SetGroup(this.blackCurtain, 1f, true);
			AudioListener.volume = 0f;
			preload.allowSceneActivation = true;
			while (!preload.isDone)
			{
				yield return null;
			}
			Scene sceneByName = SceneManager.GetSceneByName(this.mainMenuSceneName);
			if (sceneByName.IsValid() && sceneByName.isLoaded)
			{
				SceneManager.SetActiveScene(sceneByName);
			}
			if (this.warmUpCamera != null)
			{
				this.warmUpCamera.enabled = false;
			}
			if (this.warmUpListener != null)
			{
				this.warmUpListener.enabled = false;
			}
			this.CurrentPhase = "Preparing Ciel";
			MainMenuPresentationDirector director = null;
			float timeoutAt = Time.realtimeSinceStartup + this.menuReadyTimeout;
			while (Time.realtimeSinceStartup < timeoutAt)
			{
				if (director == null)
				{
					director = Object.FindObjectOfType<MainMenuPresentationDirector>(true);
				}
				if (director != null && director.IsPresentationReady)
				{
					break;
				}
				yield return null;
			}
			if (director == null || !director.IsPresentationReady)
			{
				Debug.LogWarning("[GameWarmUp] Main Menu reveal reached the readiness timeout.");
			}
			yield return new WaitForSecondsRealtime(0.55f);
			GameSettingsService.ApplyAudio();
			this.CurrentPhase = "Language Selection";
			yield return FirstLaunchLanguageScreen.ShowIfNeeded(base.gameObject.scene, (this.loadingLabel != null) ? LocalizedGameText.SourceFont(this.loadingLabel) : TMP_Settings.defaultFontAsset);
			this.CurrentPhase = "Menu Reveal";
			if (director != null)
			{
				director.ReleaseIntroMusic();
			}
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			if (this.baseBlack != null)
			{
				this.baseBlack.SetActive(false);
			}
			yield return GameWarmUpSequenceController.FadeGroup(this.blackCurtain, 1f, 0f, this.menuReveal, false);
			MainMenuEntranceAnimator mainMenuEntranceAnimator = Object.FindObjectOfType<MainMenuEntranceAnimator>(true);
			if (mainMenuEntranceAnimator != null)
			{
				mainMenuEntranceAnimator.PlayEntrance();
			}
			this.SequenceComplete = true;
			this.CurrentPhase = "Complete";
			Debug.Log("[GameWarmUp] PASS startup sequence completed in " + string.Format("{0:0.00}s; menuReady=", Time.realtimeSinceStartup - startedAt) + string.Format("{0}.", director != null && director.IsPresentationReady));
			Scene scene = base.gameObject.scene;
			if (scene.IsValid() && scene.isLoaded)
			{
				SceneManager.UnloadSceneAsync(scene);
			}
			yield break;
		}

		// Token: 0x06008BE5 RID: 35813 RVA: 0x00392C9C File Offset: 0x00390E9C
		private IEnumerator PreloadMainMenuWhenLocalizationReady()
		{
			yield return GameWarmUpSequenceController.WaitForInitialLanguage();
			this._menuPreloadStarted = true;
			this._menuPreload = SceneManager.LoadSceneAsync(this.mainMenuSceneName, LoadSceneMode.Additive);
			if (this._menuPreload != null)
			{
				this._menuPreload.allowSceneActivation = false;
			}
			yield break;
		}

		// Token: 0x06008BE6 RID: 35814 RVA: 0x00392CAB File Offset: 0x00390EAB
		private static IEnumerator WaitForInitialLanguage()
		{
			while (GameLocalization.IsLoading)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x06008BE7 RID: 35815 RVA: 0x00392CB4 File Offset: 0x00390EB4
		private void PrepareStudioAudio()
		{
			float num = (GameSettingsService.Muted ? 0f : GameSettingsService.MasterVolume);
			GameWarmUpSequenceController.ConfigureSource(this.studioChimeSource);
			if (this.studioChimeSource != null)
			{
				this.studioChimeSource.clip = ((this.studioLogoSting != null) ? this.studioLogoSting : GameWarmUpSequenceController.BuildStudioLogoSting());
				this.studioChimeSource.volume = 0.76f * num;
				this.studioChimeSource.loop = false;
			}
		}

		// Token: 0x06008BE8 RID: 35816 RVA: 0x00392D32 File Offset: 0x00390F32
		private static void ConfigureSource(AudioSource source)
		{
			if (source == null)
			{
				return;
			}
			source.playOnAwake = false;
			source.spatialBlend = 0f;
			source.ignoreListenerVolume = true;
			source.ignoreListenerPause = true;
			source.bypassReverbZones = true;
		}

		// Token: 0x06008BE9 RID: 35817 RVA: 0x00392D65 File Offset: 0x00390F65
		private IEnumerator RevealCompanyLogo()
		{
			if (this.companyLogo == null)
			{
				yield break;
			}
			RectTransform logoRect = this.companyLogo.transform as RectTransform;
			Vector3 finalScale = ((logoRect != null) ? logoRect.localScale : Vector3.one);
			this.companyLogo.gameObject.SetActive(true);
			this.companyLogo.alpha = 0f;
			this.companyLogo.blocksRaycasts = false;
			this.companyLogo.interactable = false;
			if (logoRect != null)
			{
				logoRect.localScale = finalScale * 0.92f;
			}
			if (this.logoCueDelay > 0f)
			{
				yield return new WaitForSecondsRealtime(this.logoCueDelay);
			}
			if (this.studioChimeSource != null && this.studioChimeSource.clip != null)
			{
				this.studioChimeSource.Play();
				float num = this.studioChimeSource.clip.length / Mathf.Max(0.01f, Mathf.Abs(this.studioChimeSource.pitch));
				yield return new WaitForSecondsRealtime(num);
				while (this.studioChimeSource != null && this.studioChimeSource.isPlaying)
				{
					yield return null;
				}
			}
			float elapsed = 0f;
			while (elapsed < this.logoFadeIn)
			{
				elapsed += Time.unscaledDeltaTime;
				float num2 = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / this.logoFadeIn));
				this.companyLogo.alpha = num2;
				if (logoRect != null)
				{
					logoRect.localScale = finalScale * Mathf.Lerp(0.92f, 1f, num2);
				}
				yield return null;
			}
			this.companyLogo.alpha = 1f;
			if (logoRect != null)
			{
				logoRect.localScale = finalScale;
			}
			yield break;
		}

		// Token: 0x06008BEA RID: 35818 RVA: 0x00392D74 File Offset: 0x00390F74
		private void OnDestroy()
		{
			IntroMenuMusic.CancelUnclaimed();
		}

		// Token: 0x06008BEB RID: 35819 RVA: 0x00392D7B File Offset: 0x00390F7B
		private static IEnumerator FadeGroup(CanvasGroup group, float from, float to, float duration, bool blocksRaycasts)
		{
			if (group == null)
			{
				yield break;
			}
			group.gameObject.SetActive(true);
			group.blocksRaycasts = true;
			group.interactable = false;
			float elapsed = 0f;
			while (elapsed < duration)
			{
				elapsed += Time.unscaledDeltaTime;
				float num = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / duration));
				group.alpha = Mathf.Lerp(from, to, num);
				yield return null;
			}
			group.alpha = to;
			group.blocksRaycasts = blocksRaycasts;
			if (to <= 0.001f)
			{
				group.gameObject.SetActive(false);
			}
			yield break;
		}

		// Token: 0x06008BEC RID: 35820 RVA: 0x00392DA7 File Offset: 0x00390FA7
		private static void SetGroup(CanvasGroup group, float alpha, bool blocksRaycasts)
		{
			if (group == null)
			{
				return;
			}
			group.gameObject.SetActive(alpha > 0.001f);
			group.alpha = alpha;
			group.interactable = false;
			group.blocksRaycasts = blocksRaycasts;
		}

		// Token: 0x06008BED RID: 35821 RVA: 0x00392DDC File Offset: 0x00390FDC
		private static AudioClip BuildStudioLogoSting()
		{
			int num = Mathf.CeilToInt(23040f);
			float[] array = new float[num];
			uint num2 = 1248819539U;
			float num3 = 0f;
			for (int i = 0; i < num; i++)
			{
				float num4 = (float)i / 32000f;
				num2 = num2 * 1664525U + 1013904223U;
				float num5 = (num2 >> 9) / 8388607f * 2f - 1f;
				num3 = Mathf.Lerp(num3, num5, 0.16f);
				float num6 = GameWarmUpSequenceController.BuildLogoClick(num4, 0.025f, 0.06f, 1450f, 760f, num5);
				float num7 = GameWarmUpSequenceController.BuildLogoClick(num4, 0.135f, 0.064f, 1690f, 840f, num5);
				float num8 = num4 - 0.305f;
				float num9 = 0f;
				if (num8 >= 0f && num8 < 0.385f)
				{
					float num10 = num8 / 0.385f;
					float num11 = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(num8 / 0.018f));
					float num12 = Mathf.Pow(1f - num10, 1.55f);
					float num13 = num11 * num12;
					float num14 = Mathf.Sin(6.2831855f * (610f * num8 - 430f * num8 * num8)) * 0.39f + Mathf.Sin(1162.3893f * num8 + 0.7f) * 0.23f + Mathf.Sin(1727.876f * num8 + 1.4f) * 0.12f;
					float num15 = Mathf.Sin(num10 * 3.1415927f * 5f) * (1f - num10) * 0.08f;
					num9 = (num14 + num3 * 0.3f + num15) * num13;
				}
				float num16 = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(num4 / 0.008f));
				array[i] = Mathf.Clamp((num6 + num7 + num9) * num16, -0.95f, 0.95f);
			}
			AudioClip audioClip = AudioClip.Create("JoystickyLabs - Click Click Splash", num, 1, 32000, false);
			audioClip.SetData(array, 0);
			return audioClip;
		}

		// Token: 0x06008BEE RID: 35822 RVA: 0x00392FE8 File Offset: 0x003911E8
		private static float BuildLogoClick(float time, float start, float length, float startFrequency, float endFrequency, float noise)
		{
			float num = time - start;
			if (num < 0f || num >= length)
			{
				return 0f;
			}
			float num2 = num / length;
			float num3 = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(num / 0.0045f));
			float num4 = Mathf.Pow(1f - num2, 3.2f);
			float num5 = endFrequency - startFrequency;
			return (Mathf.Sin(6.2831855f * (startFrequency * num + 0.5f * num5 / length * num * num)) * 0.54f + noise * 0.22f) * num3 * num4;
		}

		// Token: 0x04008ED5 RID: 36565
		[Header("Static UI")]
		[SerializeField]
		private CanvasGroup companyScreen;

		// Token: 0x04008ED6 RID: 36566
		[SerializeField]
		private CanvasGroup companyLogo;

		// Token: 0x04008ED7 RID: 36567
		[SerializeField]
		private CanvasGroup disclaimerScreen;

		// Token: 0x04008ED8 RID: 36568
		[SerializeField]
		private GameObject baseBlack;

		// Token: 0x04008ED9 RID: 36569
		[SerializeField]
		private CanvasGroup blackCurtain;

		// Token: 0x04008EDA RID: 36570
		[SerializeField]
		private TMP_Text loadingLabel;

		// Token: 0x04008EDB RID: 36571
		[Header("Warm-up Camera")]
		[SerializeField]
		private Camera warmUpCamera;

		// Token: 0x04008EDC RID: 36572
		[SerializeField]
		private AudioListener warmUpListener;

		// Token: 0x04008EDD RID: 36573
		[Header("Studio Audio")]
		[SerializeField]
		private AudioSource studioChimeSource;

		// Token: 0x04008EDE RID: 36574
		[SerializeField]
		private AudioClip studioLogoSting;

		// Token: 0x04008EDF RID: 36575
		[SerializeField]
		private AudioClip mainMenuMusic;

		// Token: 0x04008EE0 RID: 36576
		[SerializeField]
		[Range(0f, 1f)]
		private float menuMusicVolume = 0.2f;

		// Token: 0x04008EE1 RID: 36577
		[SerializeField]
		private float musicLoopStart = 0.04f;

		// Token: 0x04008EE2 RID: 36578
		[SerializeField]
		private float musicLoopEnd = 169.85f;

		// Token: 0x04008EE3 RID: 36579
		[SerializeField]
		private float musicLoopCrossfade = 0.4f;

		// Token: 0x04008EE4 RID: 36580
		[Header("Sequence")]
		[SerializeField]
		private string mainMenuSceneName = "MainMenu";

		// Token: 0x04008EE5 RID: 36581
		[SerializeField]
		[Min(0f)]
		private float logoCueDelay = 0.28f;

		// Token: 0x04008EE6 RID: 36582
		[SerializeField]
		[Min(0.01f)]
		private float logoFadeIn = 0.92f;

		// Token: 0x04008EE7 RID: 36583
		[SerializeField]
		[Min(0f)]
		private float logoHold = 2.1f;

		// Token: 0x04008EE8 RID: 36584
		[SerializeField]
		[Min(0.01f)]
		private float companyFadeOut = 0.55f;

		// Token: 0x04008EE9 RID: 36585
		[SerializeField]
		[Min(0.01f)]
		private float disclaimerFadeIn = 0.45f;

		// Token: 0x04008EEA RID: 36586
		[SerializeField]
		[Min(1f)]
		private float disclaimerHold = 10.25f;

		// Token: 0x04008EEB RID: 36587
		[SerializeField]
		[Min(0.01f)]
		private float disclaimerFadeOut = 0.55f;

		// Token: 0x04008EEC RID: 36588
		[SerializeField]
		[Min(0.01f)]
		private float menuReveal = 0.85f;

		// Token: 0x04008EED RID: 36589
		[SerializeField]
		[Min(1f)]
		private float menuReadyTimeout = 8f;

		// Token: 0x04008EF0 RID: 36592
		private int _loadingDots = 1;

		// Token: 0x04008EF1 RID: 36593
		private AsyncOperation _menuPreload;

		// Token: 0x04008EF2 RID: 36594
		private bool _menuPreloadStarted;
	}
}
