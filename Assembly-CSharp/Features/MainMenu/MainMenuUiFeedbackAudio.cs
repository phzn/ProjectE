using System;
using System.Collections;
using UnityEngine;

namespace Features.MainMenu
{
	// Token: 0x020010A0 RID: 4256
	[DisallowMultipleComponent]
	public sealed class MainMenuUiFeedbackAudio : MonoBehaviour
	{
		// Token: 0x17001488 RID: 5256
		// (get) Token: 0x060080DF RID: 32991 RVA: 0x00349072 File Offset: 0x00347272
		private static MainMenuUiFeedbackAudio Instance
		{
			get
			{
				if (MainMenuUiFeedbackAudio._instance != null)
				{
					return MainMenuUiFeedbackAudio._instance;
				}
				GameObject gameObject = new GameObject("Main Menu UI Feedback Audio");
				Object.DontDestroyOnLoad(gameObject);
				MainMenuUiFeedbackAudio._instance = gameObject.AddComponent<MainMenuUiFeedbackAudio>();
				return MainMenuUiFeedbackAudio._instance;
			}
		}

		// Token: 0x060080E0 RID: 32992 RVA: 0x003490A6 File Offset: 0x003472A6
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStaticState()
		{
			MainMenuUiFeedbackAudio._instance = null;
		}

		// Token: 0x060080E1 RID: 32993 RVA: 0x003490AE File Offset: 0x003472AE
		public static void PlayHover()
		{
			MainMenuUiFeedbackAudio.Instance.PlayHoverInternal();
		}

		// Token: 0x060080E2 RID: 32994 RVA: 0x003490BA File Offset: 0x003472BA
		public static void PlayClick()
		{
			MainMenuUiFeedbackAudio.Instance.PlayClickInternal();
		}

		// Token: 0x060080E3 RID: 32995 RVA: 0x003490C8 File Offset: 0x003472C8
		public static void PlayDenied()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			MainMenuUiFeedbackAudio instance = MainMenuUiFeedbackAudio.Instance;
			if (Time.unscaledTime - instance._lastDenied < 0.12f)
			{
				return;
			}
			instance._lastDenied = Time.unscaledTime;
			instance._denied.volume = 0.4f * GameSettingsService.SfxVolume;
			instance._denied.PlayOneShot(instance._deniedClip);
		}

		// Token: 0x060080E4 RID: 32996 RVA: 0x0034912C File Offset: 0x0034732C
		private static AudioClip BuildDeniedTone()
		{
			float[] array = new float[8379];
			for (int i = 0; i < array.Length; i++)
			{
				float num = (float)i / 44100f;
				float num2 = ((num < 0.085f) ? num : (num - 0.105f));
				if (num2 >= 0f)
				{
					float num3 = Mathf.Clamp01(num2 / 0.008f) * Mathf.Clamp01((0.085f - num2) / 0.025f);
					float num4 = ((num < 0.085f) ? 240f : 165f);
					array[i] = Mathf.Sin(6.2831855f * num4 * num2) * num3 * 0.45f;
				}
			}
			AudioClip audioClip = AudioClip.Create("UI unavailable", array.Length, 1, 44100, false);
			audioClip.SetData(array, 0);
			return audioClip;
		}

		// Token: 0x060080E5 RID: 32997 RVA: 0x003491E8 File Offset: 0x003473E8
		private void Awake()
		{
			if (MainMenuUiFeedbackAudio._instance != null && MainMenuUiFeedbackAudio._instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			MainMenuUiFeedbackAudio._instance = this;
			this._hoverA = this.CreateSource("Creamy UI Hover A");
			this._hoverB = this.CreateSource("Creamy UI Hover B");
			this._click = this.CreateSource("Creamy UI Click");
			this._denied = this.CreateSource("UI unavailable");
			this._deniedClip = MainMenuUiFeedbackAudio.BuildDeniedTone();
			this._hoverClip = MainMenuPresentationDirector.BuildUiTone("Creamy Hover", 0.105f, false);
			this._clickClip = MainMenuPresentationDirector.BuildUiTone("Creamy Click", 0.17f, true);
		}

		// Token: 0x060080E6 RID: 32998 RVA: 0x0034929C File Offset: 0x0034749C
		private AudioSource CreateSource(string sourceName)
		{
			GameObject gameObject = new GameObject(sourceName);
			gameObject.transform.SetParent(base.transform, false);
			AudioSource audioSource = gameObject.AddComponent<AudioSource>();
			audioSource.playOnAwake = false;
			audioSource.loop = false;
			audioSource.spatialBlend = 0f;
			audioSource.ignoreListenerPause = true;
			return audioSource;
		}

		// Token: 0x060080E7 RID: 32999 RVA: 0x003492DC File Offset: 0x003474DC
		private void PlayHoverInternal()
		{
			if (Time.unscaledTime - this._lastHoverTime < 0.045f || this._hoverClip == null)
			{
				return;
			}
			this._lastHoverTime = Time.unscaledTime;
			AudioSource audioSource = (this._useHoverA ? this._hoverA : this._hoverB);
			AudioSource audioSource2 = (this._useHoverA ? this._hoverB : this._hoverA);
			this._useHoverA = !this._useHoverA;
			audioSource.Stop();
			audioSource.clip = this._hoverClip;
			audioSource.pitch = Random.Range(0.992f, 1.008f);
			audioSource.volume = 0f;
			audioSource.Play();
			if (this._hoverMixRoutine != null)
			{
				base.StopCoroutine(this._hoverMixRoutine);
			}
			this._hoverMixRoutine = base.StartCoroutine(this.CrossfadeHover(audioSource2, audioSource));
		}

		// Token: 0x060080E8 RID: 33000 RVA: 0x003493B4 File Offset: 0x003475B4
		private void PlayClickInternal()
		{
			if (this._clickClip == null)
			{
				return;
			}
			this._click.Stop();
			this._click.clip = this._clickClip;
			this._click.pitch = Random.Range(0.995f, 1.006f);
			this._click.volume = 0.39f * GameSettingsService.SfxVolume;
			this._click.Play();
		}

		// Token: 0x060080E9 RID: 33001 RVA: 0x00349427 File Offset: 0x00347627
		private IEnumerator CrossfadeHover(AudioSource outgoing, AudioSource incoming)
		{
			float outgoingStart = ((outgoing != null) ? outgoing.volume : 0f);
			float elapsed = 0f;
			while (elapsed < 0.038f)
			{
				elapsed += Time.unscaledDeltaTime;
				float num = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / 0.038f));
				if (outgoing != null)
				{
					outgoing.volume = Mathf.Lerp(outgoingStart, 0f, num);
				}
				if (incoming != null)
				{
					incoming.volume = Mathf.Lerp(0f, 0.28f * GameSettingsService.SfxVolume, num);
				}
				yield return null;
			}
			if (outgoing != null)
			{
				outgoing.Stop();
				outgoing.volume = 0f;
			}
			if (incoming != null)
			{
				incoming.volume = 0.28f * GameSettingsService.SfxVolume;
			}
			this._hoverMixRoutine = null;
			yield break;
		}

		// Token: 0x04008424 RID: 33828
		private static MainMenuUiFeedbackAudio _instance;

		// Token: 0x04008425 RID: 33829
		private AudioSource _hoverA;

		// Token: 0x04008426 RID: 33830
		private AudioSource _hoverB;

		// Token: 0x04008427 RID: 33831
		private AudioSource _click;

		// Token: 0x04008428 RID: 33832
		private AudioClip _hoverClip;

		// Token: 0x04008429 RID: 33833
		private AudioClip _clickClip;

		// Token: 0x0400842A RID: 33834
		private AudioSource _denied;

		// Token: 0x0400842B RID: 33835
		private AudioClip _deniedClip;

		// Token: 0x0400842C RID: 33836
		private float _lastDenied = -10f;

		// Token: 0x0400842D RID: 33837
		private bool _useHoverA = true;

		// Token: 0x0400842E RID: 33838
		private float _lastHoverTime = -10f;

		// Token: 0x0400842F RID: 33839
		private Coroutine _hoverMixRoutine;
	}
}
