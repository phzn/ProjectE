using System;
using System.Collections;
using System.Collections.Generic;
using Features.MainMenu;
using ProjectExhibitionist.Core.SceneManagement;
using UnityEngine;

namespace Features.CielVoice
{
	// Token: 0x0200135F RID: 4959
	[DisallowMultipleComponent]
	public class CielVoiceController : MonoBehaviour
	{
		// Token: 0x17001866 RID: 6246
		// (get) Token: 0x0600996F RID: 39279 RVA: 0x003EE4C8 File Offset: 0x003EC6C8
		public static CielVoiceController Instance
		{
			get
			{
				Transform player = PlayerRegistry.Player;
				bool flag = CielVoiceController._instance != null && CielVoiceController._instance.isActiveAndEnabled;
				if (flag && (player == null || CielVoiceController._instance.transform.IsChildOf(player)))
				{
					return CielVoiceController._instance;
				}
				CielVoiceController cielVoiceController = (flag ? CielVoiceController._instance : null);
				for (int i = 0; i < CielVoiceController.EnabledControllers.Count; i++)
				{
					CielVoiceController cielVoiceController2 = CielVoiceController.EnabledControllers[i];
					if (!(cielVoiceController2 == null) && cielVoiceController2.isActiveAndEnabled)
					{
						if (player != null && cielVoiceController2.transform.IsChildOf(player))
						{
							return CielVoiceController._instance = cielVoiceController2;
						}
						if (cielVoiceController == null)
						{
							cielVoiceController = cielVoiceController2;
						}
					}
				}
				return CielVoiceController._instance = ((player != null) ? null : cielVoiceController);
			}
		}

		// Token: 0x17001867 RID: 6247
		// (get) Token: 0x06009970 RID: 39280 RVA: 0x003EE59B File Offset: 0x003EC79B
		public bool IsSpeaking
		{
			get
			{
				return this.voiceSource != null && this.voiceSource.isPlaying;
			}
		}

		// Token: 0x17001868 RID: 6248
		// (get) Token: 0x06009971 RID: 39281 RVA: 0x003EE5B8 File Offset: 0x003EC7B8
		public CielVoiceSO CurrentVoice
		{
			get
			{
				return this._current;
			}
		}

		// Token: 0x17001869 RID: 6249
		// (get) Token: 0x06009972 RID: 39282 RVA: 0x003EE5C0 File Offset: 0x003EC7C0
		public AudioSource VoiceSource
		{
			get
			{
				return this.voiceSource;
			}
		}

		// Token: 0x1700186A RID: 6250
		// (get) Token: 0x06009973 RID: 39283 RVA: 0x003EE5C8 File Offset: 0x003EC7C8
		public bool IsReadyToPlay
		{
			get
			{
				return base.isActiveAndEnabled && this.voiceSource != null && this.voiceSource.isActiveAndEnabled && (!AudioListener.pause || this.voiceSource.ignoreListenerPause);
			}
		}

		// Token: 0x1700186B RID: 6251
		// (get) Token: 0x06009974 RID: 39284 RVA: 0x003EE603 File Offset: 0x003EC803
		public CielVoiceSubtitleUI ActiveSubtitleUI
		{
			get
			{
				return this.ResolveSubtitleUI(false);
			}
		}

		// Token: 0x1700186C RID: 6252
		// (get) Token: 0x06009975 RID: 39285 RVA: 0x003EE60C File Offset: 0x003EC80C
		public float SubtitleTypewriterSpeed
		{
			get
			{
				return this.subtitleTypewriterSpeed;
			}
		}

		// Token: 0x1400010F RID: 271
		// (add) Token: 0x06009976 RID: 39286 RVA: 0x003EE614 File Offset: 0x003EC814
		// (remove) Token: 0x06009977 RID: 39287 RVA: 0x003EE64C File Offset: 0x003EC84C
		public event Action<CielVoiceSO> OnVoiceStarted;

		// Token: 0x14000110 RID: 272
		// (add) Token: 0x06009978 RID: 39288 RVA: 0x003EE684 File Offset: 0x003EC884
		// (remove) Token: 0x06009979 RID: 39289 RVA: 0x003EE6BC File Offset: 0x003EC8BC
		public event Action<CielVoiceSO> OnVoiceEnded;

		// Token: 0x0600997A RID: 39290 RVA: 0x003EE6F1 File Offset: 0x003EC8F1
		private void Awake()
		{
			this.EnsureVoiceSource();
			this.showSubtitles = GameSettingsService.CielVoiceSubtitles;
			GameAudioRouter.Route(this.voiceSource, GameAudioRouter.Channel.CielVoice);
		}

		// Token: 0x0600997B RID: 39291 RVA: 0x003EE710 File Offset: 0x003EC910
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetInstances()
		{
			CielVoiceController.EnabledControllers.Clear();
			CielVoiceController._instance = null;
		}

		// Token: 0x0600997C RID: 39292 RVA: 0x003EE722 File Offset: 0x003EC922
		private void OnEnable()
		{
			if (!CielVoiceController.EnabledControllers.Contains(this))
			{
				CielVoiceController.EnabledControllers.Add(this);
			}
		}

		// Token: 0x0600997D RID: 39293 RVA: 0x003EE73C File Offset: 0x003EC93C
		private void OnDisable()
		{
			CielVoiceController.EnabledControllers.Remove(this);
			if (CielVoiceController._instance == this)
			{
				CielVoiceController._instance = null;
			}
			bool flag = this._current != null || this._playRoutine != null || this._fadeOutRoutine != null;
			if (this._fadeOutRoutine != null)
			{
				base.StopCoroutine(this._fadeOutRoutine);
				this._fadeOutRoutine = null;
			}
			if (flag)
			{
				this.StopInternal(true, true);
			}
		}

		// Token: 0x0600997E RID: 39294 RVA: 0x003EE7B0 File Offset: 0x003EC9B0
		private void EnsureVoiceSource()
		{
			if (this.voiceSource != null)
			{
				this.voiceSource.playOnAwake = false;
				this.voiceSource.loop = false;
				return;
			}
			GameObject gameObject = new GameObject("CielVoiceAudioSource");
			gameObject.transform.SetParent(base.transform, false);
			this.voiceSource = gameObject.AddComponent<AudioSource>();
			this.voiceSource.playOnAwake = false;
			this.voiceSource.loop = false;
			this.voiceSource.spatialBlend = 0f;
			if (this.debugLog)
			{
				Debug.LogWarning("[CielVoice] voiceSource non cablato: creato fallback runtime.");
			}
		}

		// Token: 0x0600997F RID: 39295 RVA: 0x003EE847 File Offset: 0x003ECA47
		public bool Play(CielVoiceSO voice)
		{
			return this.Play(voice, true, -1f);
		}

		// Token: 0x06009980 RID: 39296 RVA: 0x003EE856 File Offset: 0x003ECA56
		public bool Play(CielVoiceSO voice, bool showThisSubtitle)
		{
			return this.Play(voice, showThisSubtitle, -1f);
		}

		// Token: 0x06009981 RID: 39297 RVA: 0x003EE868 File Offset: 0x003ECA68
		public bool PlayWithSubtitleSyncedToAudio(CielVoiceSO voice, float minimumPostTypewriterHold = 0.75f)
		{
			this._syncNextSubtitleToAudio = true;
			this._syncedSubtitleMinimumHold = Mathf.Max(0f, minimumPostTypewriterHold);
			bool flag;
			try
			{
				flag = this.Play(voice, true, -1f);
			}
			finally
			{
				this._syncNextSubtitleToAudio = false;
				this._syncedSubtitleMinimumHold = 0f;
			}
			return flag;
		}

		// Token: 0x06009982 RID: 39298 RVA: 0x003EE8C4 File Offset: 0x003ECAC4
		public bool Play(CielVoiceSO voice, bool showThisSubtitle, float subtitleHoldDurationOverride)
		{
			if (voice == null || !base.isActiveAndEnabled)
			{
				return false;
			}
			if (this.voiceSource == null)
			{
				this.EnsureVoiceSource();
			}
			if (this.voiceSource == null || !this.voiceSource.isActiveAndEnabled)
			{
				return false;
			}
			if (!voice.HasClips)
			{
				if (this.debugLog)
				{
					Debug.LogWarning("[CielVoice] '" + voice.ResolvedId + "' non ha clip valide.");
				}
				return false;
			}
			if (this.IsSpeaking)
			{
				if (this.interruptPolicy == CielVoiceController.InterruptPolicy.Ignore)
				{
					if (this.debugLog)
					{
						string[] array = new string[5];
						array[0] = "[CielVoice] '";
						array[1] = voice.ResolvedId;
						array[2] = "' ignorata: '";
						int num = 3;
						CielVoiceSO current = this._current;
						array[num] = ((current != null) ? current.ResolvedId : null);
						array[4] = "' in corso.";
						Debug.Log(string.Concat(array));
					}
					return false;
				}
				this.StopImmediate();
			}
			if (this._fadeOutRoutine != null)
			{
				base.StopCoroutine(this._fadeOutRoutine);
				this._fadeOutRoutine = null;
				if (this.voiceSource != null)
				{
					this.voiceSource.Stop();
				}
			}
			AudioClip audioClip = voice.PickClip();
			if (audioClip == null)
			{
				return false;
			}
			this._current = voice;
			this.voiceSource.clip = audioClip;
			this.voiceSource.volume = Mathf.Clamp01(this.masterVolume * voice.Volume);
			this.voiceSource.pitch = voice.ResolvePitch();
			if (voice.MixerGroup != null)
			{
				this.voiceSource.outputAudioMixerGroup = voice.MixerGroup;
			}
			this.voiceSource.loop = false;
			this.voiceSource.Play();
			CielVoiceSubtitleUI cielVoiceSubtitleUI = ((showThisSubtitle && this.showSubtitles && !string.IsNullOrWhiteSpace(voice.Subtitle)) ? this.ResolveSubtitleUI(true) : null);
			if (cielVoiceSubtitleUI != null)
			{
				cielVoiceSubtitleUI.SetTypewriterSpeed(this.subtitleTypewriterSpeed);
				float num2 = Mathf.Max(0.01f, Mathf.Abs(this.voiceSource.pitch));
				float num3 = audioClip.length / num2;
				float num7;
				if (this._syncNextSubtitleToAudio)
				{
					float num4 = num3 + voice.SubtitleExtraHold;
					float num5 = (float)voice.Subtitle.Length * this.subtitleTypewriterSpeed;
					float num6 = cielVoiceSubtitleUI.FadeInDuration + cielVoiceSubtitleUI.FadeOutDuration;
					num7 = Mathf.Max(this._syncedSubtitleMinimumHold, num4 - num5 - num6);
				}
				else
				{
					num7 = ((subtitleHoldDurationOverride >= 0f) ? subtitleHoldDurationOverride : (num3 + voice.SubtitleExtraHold));
				}
				cielVoiceSubtitleUI.Show(voice.Subtitle, num7, this.subtitleSpeakerName);
			}
			if (this.debugLog)
			{
				Debug.Log(string.Format("[CielVoice] Play '{0}' clip='{1}' vol={2:F2} pitch={3:F2} skippableSub={4}", new object[]
				{
					voice.ResolvedId,
					audioClip.name,
					this.voiceSource.volume,
					this.voiceSource.pitch,
					voice.SkippableSubtitle
				}));
			}
			Action<CielVoiceSO> onVoiceStarted = this.OnVoiceStarted;
			if (onVoiceStarted != null)
			{
				onVoiceStarted(voice);
			}
			if (this._playRoutine != null)
			{
				base.StopCoroutine(this._playRoutine);
			}
			this._playRoutine = base.StartCoroutine(this.WatchPlaybackEnd());
			return true;
		}

		// Token: 0x06009983 RID: 39299 RVA: 0x003EEBDC File Offset: 0x003ECDDC
		public void Stop()
		{
			this.StopInternal(true, false);
		}

		// Token: 0x06009984 RID: 39300 RVA: 0x003EEBE6 File Offset: 0x003ECDE6
		private void StopImmediate()
		{
			this.StopInternal(true, true);
		}

		// Token: 0x06009985 RID: 39301 RVA: 0x003EEBF0 File Offset: 0x003ECDF0
		private void StopInternal(bool fireEnded, bool immediate)
		{
			if (this._playRoutine != null)
			{
				base.StopCoroutine(this._playRoutine);
				this._playRoutine = null;
			}
			CielVoiceSO current = this._current;
			this._current = null;
			CielVoiceSubtitleUI cielVoiceSubtitleUI = this.ResolveSubtitleUI(false);
			if (cielVoiceSubtitleUI != null)
			{
				cielVoiceSubtitleUI.Hide();
			}
			if (this.voiceSource != null && this.voiceSource.isPlaying)
			{
				if (immediate || this.stopFadeDuration <= 0f)
				{
					this.voiceSource.Stop();
				}
				else
				{
					if (this._fadeOutRoutine != null)
					{
						base.StopCoroutine(this._fadeOutRoutine);
					}
					this._fadeOutRoutine = base.StartCoroutine(this.FadeOutAndStop(this.voiceSource));
				}
			}
			if (fireEnded && current != null)
			{
				Action<CielVoiceSO> onVoiceEnded = this.OnVoiceEnded;
				if (onVoiceEnded == null)
				{
					return;
				}
				onVoiceEnded(current);
			}
		}

		// Token: 0x06009986 RID: 39302 RVA: 0x003EECBE File Offset: 0x003ECEBE
		private IEnumerator FadeOutAndStop(AudioSource src)
		{
			float startVol = src.volume;
			float elapsed = 0f;
			while (elapsed < this.stopFadeDuration && src != null && src.isPlaying)
			{
				elapsed += Time.unscaledDeltaTime;
				float num = Mathf.Clamp01(elapsed / this.stopFadeDuration);
				src.volume = Mathf.Lerp(startVol, 0f, num);
				yield return null;
			}
			if (src != null)
			{
				src.Stop();
				src.volume = startVol;
			}
			this._fadeOutRoutine = null;
			yield break;
		}

		// Token: 0x06009987 RID: 39303 RVA: 0x003EECD4 File Offset: 0x003ECED4
		private IEnumerator WatchPlaybackEnd()
		{
			yield return null;
			while (this.voiceSource != null && this.voiceSource.isPlaying)
			{
				yield return null;
			}
			CielVoiceSO current = this._current;
			this._current = null;
			this._playRoutine = null;
			if (this.debugLog && current != null)
			{
				Debug.Log("[CielVoice] '" + current.ResolvedId + "' finita.");
			}
			if (current != null)
			{
				Action<CielVoiceSO> onVoiceEnded = this.OnVoiceEnded;
				if (onVoiceEnded != null)
				{
					onVoiceEnded(current);
				}
			}
			yield break;
		}

		// Token: 0x06009988 RID: 39304 RVA: 0x003EECE4 File Offset: 0x003ECEE4
		public void SetMasterVolume(float v)
		{
			this.masterVolume = Mathf.Clamp01(v);
			if (this.voiceSource != null && this.voiceSource.isPlaying && this._current != null)
			{
				this.voiceSource.volume = Mathf.Clamp01(this.masterVolume * this._current.Volume);
			}
		}

		// Token: 0x06009989 RID: 39305 RVA: 0x003EED48 File Offset: 0x003ECF48
		public void SetSubtitlesEnabled(bool enabled)
		{
			this.showSubtitles = enabled;
			if (!enabled)
			{
				CielVoiceSubtitleUI cielVoiceSubtitleUI = this.ResolveSubtitleUI(false);
				if (cielVoiceSubtitleUI != null)
				{
					cielVoiceSubtitleUI.Hide();
				}
			}
		}

		// Token: 0x0600998A RID: 39306 RVA: 0x003EED78 File Offset: 0x003ECF78
		private CielVoiceSubtitleUI ResolveSubtitleUI(bool createFallback)
		{
			if (this.subtitleUI != null)
			{
				return this.subtitleUI;
			}
			if (CielVoiceSubtitleUI.Instance != null)
			{
				return CielVoiceSubtitleUI.Instance;
			}
			CielVoiceSubtitleUI cielVoiceSubtitleUI = Object.FindObjectOfType<CielVoiceSubtitleUI>(true);
			if (cielVoiceSubtitleUI != null)
			{
				return cielVoiceSubtitleUI;
			}
			if (!createFallback || this.subtitlePrefab == null)
			{
				return null;
			}
			this.subtitleUI = Object.Instantiate<CielVoiceSubtitleUI>(this.subtitlePrefab);
			this.subtitleUI.gameObject.name = this.subtitlePrefab.gameObject.name;
			if (this.debugLog)
			{
				Debug.Log(string.Concat(new string[]
				{
					"[CielVoice] Creato fallback sottotitoli '",
					this.subtitleUI.name,
					"' per la scena standalone '",
					base.gameObject.scene.name,
					"'."
				}));
			}
			return this.subtitleUI;
		}

		// Token: 0x04009E90 RID: 40592
		private static readonly List<CielVoiceController> EnabledControllers = new List<CielVoiceController>();

		// Token: 0x04009E91 RID: 40593
		private static CielVoiceController _instance;

		// Token: 0x04009E92 RID: 40594
		[Header("References")]
		[Tooltip("AudioSource DEDICATO alla voce, su un GO sotto Ciel_Invector/Audio. Configurato con PlayOnAwake=false. Se null viene cercato/creato a runtime come figlio 'CielVoiceAudioSource'.")]
		[SerializeField]
		private AudioSource voiceSource;

		// Token: 0x04009E93 RID: 40595
		[Tooltip("Pannello sottotitoli di scena. Se null si usa CielVoiceSubtitleUI.Instance al momento del Play.")]
		[SerializeField]
		private CielVoiceSubtitleUI subtitleUI;

		// Token: 0x04009E94 RID: 40596
		[Tooltip("Fallback istanziato soltanto quando una scena standalone/test non contiene CielVoiceSubtitleUI. Nelle scene con _Core il singleton esistente viene riusato e questo prefab non viene duplicato.")]
		[SerializeField]
		private CielVoiceSubtitleUI subtitlePrefab;

		// Token: 0x04009E95 RID: 40597
		[Header("Global")]
		[Tooltip("Volume globale applicato a tutte le battute (moltiplica CielVoiceSO.Volume).")]
		[Range(0f, 1f)]
		[SerializeField]
		private float masterVolume = 1f;

		// Token: 0x04009E96 RID: 40598
		[Tooltip("Comportamento quando arriva una nuova battuta mentre un'altra sta suonando.")]
		[SerializeField]
		private CielVoiceController.InterruptPolicy interruptPolicy;

		// Token: 0x04009E97 RID: 40599
		[Tooltip("Mostra i sottotitoli delle battute. Off = solo audio (accessibilita'/preferenza).")]
		[SerializeField]
		private bool showSubtitles = true;

		// Token: 0x04009E98 RID: 40600
		[Tooltip("Nome mostrato nel pannello detection-style dei sottotitoli.")]
		[SerializeField]
		private string subtitleSpeakerName = "CIEL";

		// Token: 0x04009E99 RID: 40601
		[Tooltip("Velocita' del typewriter dei sottotitoli di Ciel (secondi per carattere): piu' basso = testo piu' veloce. Applicata al pannello CielVoiceSubtitleUI ad ogni battuta, cosi' la velocita' e' centralizzata qui. 0 = testo istantaneo (nessun typewriter).")]
		[Min(0f)]
		[SerializeField]
		private float subtitleTypewriterSpeed = 0.045f;

		// Token: 0x04009E9A RID: 40602
		[Header("Stop")]
		[Tooltip("Durata (s) del fade-out quando una battuta viene FERMATA in anticipo (Stop). Rende l'interruzione morbida invece di un taglio secco. 0 = stop immediato.")]
		[Min(0f)]
		[SerializeField]
		private float stopFadeDuration = 0.25f;

		// Token: 0x04009E9B RID: 40603
		[Header("Debug")]
		[SerializeField]
		private bool debugLog;

		// Token: 0x04009E9C RID: 40604
		private CielVoiceSO _current;

		// Token: 0x04009E9D RID: 40605
		private Coroutine _playRoutine;

		// Token: 0x04009E9E RID: 40606
		private Coroutine _fadeOutRoutine;

		// Token: 0x04009E9F RID: 40607
		private bool _syncNextSubtitleToAudio;

		// Token: 0x04009EA0 RID: 40608
		private float _syncedSubtitleMinimumHold;

		// Token: 0x02001360 RID: 4960
		public enum InterruptPolicy
		{
			// Token: 0x04009EA4 RID: 40612
			[Tooltip("Se una voce sta gia' suonando, la nuova la interrompe e prende il suo posto.")]
			Interrupt,
			// Token: 0x04009EA5 RID: 40613
			[Tooltip("Se una voce sta gia' suonando, la nuova viene ignorata (la corrente finisce).")]
			Ignore
		}
	}
}
