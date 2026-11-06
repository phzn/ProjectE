using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Features.Detection
{
	// Token: 0x020012F8 RID: 4856
	public class TVNewsIntroController : MonoBehaviour
	{
		// Token: 0x060096B9 RID: 38585 RVA: 0x003DE140 File Offset: 0x003DC340
		private void OnEnable()
		{
			if (this.canvasGroup != null)
			{
				this.canvasGroup.alpha = 0f;
			}
			if (this.displayImage != null)
			{
				this.displayImage.gameObject.SetActive(false);
			}
			if (this.videoDisplayImage != null)
			{
				this.videoDisplayImage.gameObject.SetActive(false);
			}
		}

		// Token: 0x060096BA RID: 38586 RVA: 0x003DE1A9 File Offset: 0x003DC3A9
		private void OnDestroy()
		{
			this.ReleaseVideoCallbacks();
			if (this._createdRT != null)
			{
				this._createdRT.Release();
				Object.Destroy(this._createdRT);
			}
		}

		// Token: 0x060096BB RID: 38587 RVA: 0x003DE1D8 File Offset: 0x003DC3D8
		public void CancelPlayback()
		{
			base.StopAllCoroutines();
			this.ReleaseVideoCallbacks();
			if (this.videoPlayer != null)
			{
				this.videoPlayer.Stop();
			}
			if (this.audioSource != null)
			{
				this.audioSource.Stop();
			}
			if (this.canvasGroup != null)
			{
				this.canvasGroup.alpha = 0f;
			}
			this.HideDisplayObjects();
		}

		// Token: 0x060096BC RID: 38588 RVA: 0x003DE248 File Offset: 0x003DC448
		private void ReleaseVideoCallbacks()
		{
			if (this._observedPlayer != null)
			{
				this._observedPlayer.loopPointReached -= this._videoEndHandler;
				this._observedPlayer.errorReceived -= this._videoErrorHandler;
			}
			this._observedPlayer = null;
			this._videoEndHandler = null;
			this._videoErrorHandler = null;
		}

		// Token: 0x060096BD RID: 38589 RVA: 0x003DE29A File Offset: 0x003DC49A
		public IEnumerator Play(bool includeFadeOut = true)
		{
			bool flag = this.introVideoClip != null && this.videoPlayer != null;
			bool flag2 = !flag && this.frames != null && this.frames.Length != 0;
			if (!flag && !flag2)
			{
				Debug.LogWarning("[TVNewsIntro] Nessun VideoClip né frame assegnati, intro saltato.");
				yield break;
			}
			if (this.canvasGroup != null)
			{
				this.canvasGroup.alpha = 0f;
			}
			if (this.audioSource != null && this.introAudio != null && this.audioSource.enabled)
			{
				this.audioSource.clip = this.introAudio;
				this.audioSource.loop = false;
				this.audioSource.volume = this.introVolume;
				this.audioSource.Play();
			}
			if (flag)
			{
				yield return this.PlayVideo(includeFadeOut);
			}
			else
			{
				yield return this.PlayFrames(includeFadeOut);
			}
			yield break;
		}

		// Token: 0x060096BE RID: 38590 RVA: 0x003DE2B0 File Offset: 0x003DC4B0
		public IEnumerator FadeOutOverlay()
		{
			yield return this.SetAlpha((this.canvasGroup != null) ? this.canvasGroup.alpha : 1f, 0f, this.fadeOutDuration);
			this.HideDisplayObjects();
			yield break;
		}

		// Token: 0x060096BF RID: 38591 RVA: 0x003DE2BF File Offset: 0x003DC4BF
		public IEnumerator PrepareVideoAsync()
		{
			if (this.introVideoClip == null || this.videoPlayer == null)
			{
				yield break;
			}
			this.videoPlayer.gameObject.SetActive(true);
			this.videoPlayer.enabled = true;
			this.videoPlayer.isLooping = false;
			this.videoPlayer.Stop();
			if (this.videoPlayer.targetTexture == null)
			{
				this._createdRT = new RenderTexture(1920, 1080, 0);
				this.videoPlayer.targetTexture = this._createdRT;
			}
			if (this.videoDisplayImage != null)
			{
				this.videoDisplayImage.texture = this.videoPlayer.targetTexture;
				this.videoDisplayImage.gameObject.SetActive(true);
			}
			this.videoPlayer.clip = this.introVideoClip;
			this.videoPlayer.Prepare();
			float timeout = 5f;
			float elapsed = 0f;
			while (!this.videoPlayer.isPrepared && elapsed < timeout)
			{
				elapsed += Time.unscaledDeltaTime;
				yield return null;
			}
			if (!this.videoPlayer.isPrepared)
			{
				Debug.LogWarning("[TVNewsIntro] VideoPlayer non pronto entro il timeout (PrepareVideoAsync).");
			}
			yield break;
		}

		// Token: 0x060096C0 RID: 38592 RVA: 0x003DE2CE File Offset: 0x003DC4CE
		public IEnumerator PlayPrepared(bool includeFadeOut = false)
		{
			if (this.videoPlayer == null || this.videoPlayer.clip == null || !this.videoPlayer.isPrepared)
			{
				yield return this.Play(includeFadeOut);
				yield break;
			}
			if (this.audioSource != null && this.introAudio != null && this.audioSource.enabled)
			{
				this.audioSource.clip = this.introAudio;
				this.audioSource.loop = false;
				this.audioSource.volume = this.introVolume;
				this.audioSource.Play();
			}
			if (this.videoDisplayImage != null && !this.videoDisplayImage.gameObject.activeSelf)
			{
				this.videoDisplayImage.gameObject.SetActive(true);
			}
			if (this.canvasGroup != null)
			{
				this.canvasGroup.alpha = 1f;
			}
			yield return this.PlayVideoToCompletion(false);
			if (this.audioSource != null && this.audioSource.isPlaying)
			{
				this.audioSource.Stop();
			}
			if (includeFadeOut)
			{
				yield return this.SetAlpha(1f, 0f, this.fadeOutDuration);
				this.HideDisplayObjects();
				this.videoPlayer.Stop();
			}
			yield break;
		}

		// Token: 0x060096C1 RID: 38593 RVA: 0x003DE2E4 File Offset: 0x003DC4E4
		private IEnumerator PlayVideo(bool includeFadeOut)
		{
			this.videoPlayer.gameObject.SetActive(true);
			this.videoPlayer.enabled = true;
			this.videoPlayer.isLooping = false;
			this.videoPlayer.Stop();
			if (this.videoPlayer.targetTexture == null)
			{
				this._createdRT = new RenderTexture(1920, 1080, 0);
				this.videoPlayer.targetTexture = this._createdRT;
			}
			if (this.videoDisplayImage != null)
			{
				this.videoDisplayImage.texture = this.videoPlayer.targetTexture;
				this.videoDisplayImage.gameObject.SetActive(true);
			}
			this.videoPlayer.clip = this.introVideoClip;
			this.videoPlayer.Prepare();
			float timeout = 5f;
			float elapsed = 0f;
			while (!this.videoPlayer.isPrepared && elapsed < timeout)
			{
				elapsed += Time.unscaledDeltaTime;
				yield return null;
			}
			if (!this.videoPlayer.isPrepared)
			{
				Debug.LogWarning("[TVNewsIntro] VideoPlayer non pronto entro il timeout.");
				if (this.frames != null && this.frames.Length != 0)
				{
					yield return this.PlayFrames(includeFadeOut);
				}
				yield break;
			}
			yield return this.PlayVideoToCompletion(true);
			if (this.audioSource != null && this.audioSource.isPlaying)
			{
				this.audioSource.Stop();
			}
			if (includeFadeOut)
			{
				yield return this.SetAlpha(1f, 0f, this.fadeOutDuration);
				this.HideDisplayObjects();
				this.videoPlayer.Stop();
			}
			yield break;
		}

		// Token: 0x060096C2 RID: 38594 RVA: 0x003DE2FA File Offset: 0x003DC4FA
		private IEnumerator PlayVideoToCompletion(bool fadeIn = false)
		{
			VideoPlayer player = this.videoPlayer;
			bool finished = false;
			bool failed = false;
			VideoPlayer.EventHandler onEnd = delegate(VideoPlayer _)
			{
				finished = true;
			};
			VideoPlayer.ErrorEventHandler onError = delegate(VideoPlayer _, string message)
			{
				failed = true;
				Debug.LogWarning("[TVNewsIntro] " + message);
			};
			this.ReleaseVideoCallbacks();
			this._observedPlayer = player;
			this._videoEndHandler = onEnd;
			this._videoErrorHandler = onError;
			player.loopPointReached += onEnd;
			player.errorReceived += onError;
			try
			{
				player.isLooping = false;
				player.Play();
				if (fadeIn)
				{
					yield return this.SetAlpha(0f, 1f, this.fadeInDuration);
				}
				double length = player.length;
				float deadline = Time.realtimeSinceStartup + (float)((length > 0.0) ? length : 30.0) + 10f;
				bool started = false;
				while (player != null && !finished && !failed && Time.realtimeSinceStartup < deadline)
				{
					started |= player.isPlaying;
					if (started && !player.isPlaying && !player.isPaused)
					{
						break;
					}
					yield return null;
				}
				if (!finished && !failed && Time.realtimeSinceStartup >= deadline)
				{
					Debug.LogWarning("[TVNewsIntro] Video playback timed out; continuing to the studio.");
				}
			}
			finally
			{
				if (player != null)
				{
					player.loopPointReached -= onEnd;
					player.errorReceived -= onError;
					if (this._observedPlayer == player && this._videoEndHandler == onEnd)
					{
						this.ReleaseVideoCallbacks();
						player.Pause();
					}
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x060096C3 RID: 38595 RVA: 0x003DE310 File Offset: 0x003DC510
		private IEnumerator PlayFrames(bool includeFadeOut)
		{
			if (this.displayImage != null)
			{
				this.displayImage.gameObject.SetActive(true);
			}
			yield return this.SetAlpha(0f, 1f, this.fadeInDuration);
			float frameDuration = ((this.fps > 0f) ? (1f / this.fps) : 0.041666668f);
			foreach (Sprite sprite in this.frames)
			{
				if (this.displayImage != null)
				{
					this.displayImage.sprite = sprite;
				}
				yield return new WaitForSeconds(frameDuration);
			}
			Sprite[] array = null;
			if (this.audioSource != null && this.audioSource.isPlaying)
			{
				this.audioSource.Stop();
			}
			if (includeFadeOut)
			{
				yield return this.SetAlpha(1f, 0f, this.fadeOutDuration);
				this.HideDisplayObjects();
				if (this.audioSource != null)
				{
					this.audioSource.Stop();
				}
			}
			yield break;
		}

		// Token: 0x060096C4 RID: 38596 RVA: 0x003DE326 File Offset: 0x003DC526
		private void HideDisplayObjects()
		{
			if (this.displayImage != null)
			{
				this.displayImage.gameObject.SetActive(false);
			}
			if (this.videoDisplayImage != null)
			{
				this.videoDisplayImage.gameObject.SetActive(false);
			}
		}

		// Token: 0x060096C5 RID: 38597 RVA: 0x003DE366 File Offset: 0x003DC566
		private IEnumerator SetAlpha(float from, float to, float duration)
		{
			if (this.canvasGroup == null)
			{
				yield break;
			}
			float elapsed = 0f;
			this.canvasGroup.alpha = from;
			while (elapsed < duration)
			{
				elapsed += Time.unscaledDeltaTime;
				this.canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
				yield return null;
			}
			this.canvasGroup.alpha = to;
			yield break;
		}

		// Token: 0x04009BD1 RID: 39889
		[Header("Video Clip (consigliato — MP4)")]
		[Tooltip("VideoClip da riprodurre. Se assegnato, ha la precedenza sullo spritesheet.")]
		public VideoClip introVideoClip;

		// Token: 0x04009BD2 RID: 39890
		[Tooltip("VideoPlayer component (stesso GO o child). Assegna targetTexture = una RenderTexture del progetto.")]
		public VideoPlayer videoPlayer;

		// Token: 0x04009BD3 RID: 39891
		[Tooltip("RawImage su cui proiettare il video. Il codice associa automaticamente videoPlayer.targetTexture.")]
		public RawImage videoDisplayImage;

		// Token: 0x04009BD4 RID: 39892
		[Header("Spritesheet Frames (fallback se introVideoClip = null)")]
		[Tooltip("Sprite sliciati dalla spritesheet in ordine di riproduzione.")]
		public Sprite[] frames;

		// Token: 0x04009BD5 RID: 39893
		[Tooltip("Fotogrammi al secondo per lo spritesheet.")]
		public float fps = 24f;

		// Token: 0x04009BD6 RID: 39894
		[Tooltip("Image su cui proiettare i frame (spritesheet only).")]
		public Image displayImage;

		// Token: 0x04009BD7 RID: 39895
		[Header("Fade")]
		[Tooltip("Durata fade-in dell'overlay all'inizio.")]
		public float fadeInDuration = 0.3f;

		// Token: 0x04009BD8 RID: 39896
		[Tooltip("Durata fade-out dell'overlay al termine. Ignorata se includeFadeOut=false.")]
		public float fadeOutDuration = 0.3f;

		// Token: 0x04009BD9 RID: 39897
		[Header("UI References")]
		[Tooltip("CanvasGroup del GO root per alpha fade.")]
		public CanvasGroup canvasGroup;

		// Token: 0x04009BDA RID: 39898
		[Header("Audio (opzionale — se l'MP4 non ha traccia audio propria)")]
		[Tooltip("AudioClip jingle/sigla da riprodurre in parallelo al video.")]
		public AudioClip introAudio;

		// Token: 0x04009BDB RID: 39899
		public AudioSource audioSource;

		// Token: 0x04009BDC RID: 39900
		[Tooltip("Volume forzato sull'AudioSource ad ogni Play() della sigla. Necessario perché la sorgente è spesso condivisa con il background audio dello studio: senza un reset esplicito, un eventuale fade precedente che ha portato il volume a 0 lascia la sigla muta.")]
		[Range(0f, 1f)]
		public float introVolume = 1f;

		// Token: 0x04009BDD RID: 39901
		private RenderTexture _createdRT;

		// Token: 0x04009BDE RID: 39902
		private VideoPlayer _observedPlayer;

		// Token: 0x04009BDF RID: 39903
		private VideoPlayer.EventHandler _videoEndHandler;

		// Token: 0x04009BE0 RID: 39904
		private VideoPlayer.ErrorEventHandler _videoErrorHandler;
	}
}
