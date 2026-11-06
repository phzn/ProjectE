using System;
using Features.ExposeLingerie;
using Features.Flash;
using Features.MasturbationSystem;
using Features.RunModes;
using ProjectExhibitionist;
using ProjectExhibitionist.Core;
using UnityEngine;

namespace Features.CielVoice
{
	// Token: 0x02001352 RID: 4946
	[DisallowMultipleComponent]
	public class CielAmbientBreathing : MonoBehaviour
	{
		// Token: 0x1700185A RID: 6234
		// (get) Token: 0x0600990F RID: 39183 RVA: 0x003EC34D File Offset: 0x003EA54D
		private bool IsRunningActive
		{
			get
			{
				return this.runningNude != null && this.runningNude.IsActive;
			}
		}

		// Token: 0x1700185B RID: 6235
		// (get) Token: 0x06009910 RID: 39184 RVA: 0x003EC36A File Offset: 0x003EA56A
		private static bool ModalContextActive
		{
			get
			{
				return (PlayerActivityGate.BlockedMask & PlayerActivity.AllPlayerActions) == PlayerActivity.AllPlayerActions;
			}
		}

		// Token: 0x1700185C RID: 6236
		// (get) Token: 0x06009911 RID: 39185 RVA: 0x003EC37E File Offset: 0x003EA57E
		private bool LewdRequested
		{
			get
			{
				return this._flashActive || this._emoteLewdActive;
			}
		}

		// Token: 0x1700185D RID: 6237
		// (get) Token: 0x06009912 RID: 39186 RVA: 0x003EC390 File Offset: 0x003EA590
		private bool ShouldPlayLewd
		{
			get
			{
				return !RunModeService.IsAtHome && this.LewdRequested && !this._voiceDucking && !this._uiContextDucking && !this._masturbationActive && !this.IsRunningActive && this.lewdBreathingClip != null;
			}
		}

		// Token: 0x1700185E RID: 6238
		// (get) Token: 0x06009913 RID: 39187 RVA: 0x003EC3CF File Offset: 0x003EA5CF
		private bool ShouldPlay
		{
			get
			{
				return !RunModeService.IsAtHome && this._isNaked && !this._voiceDucking && !this._uiContextDucking && !this.LewdRequested && !this._masturbationActive && !this.IsRunningActive;
			}
		}

		// Token: 0x06009914 RID: 39188 RVA: 0x003EC40C File Offset: 0x003EA60C
		private void Awake()
		{
			this.EnsureSources();
			if (this.clothing == null)
			{
				this.clothing = Object.FindObjectOfType<ClothingToggleController>();
			}
			if (this.voiceController == null)
			{
				this.voiceController = CielVoiceController.Instance;
			}
			if (this.runningNude == null)
			{
				this.runningNude = base.GetComponent<CielRunningNude>();
			}
			this._loop = new CrossfadeLoopPlayer(this.breathingSourceA, this.breathingSourceB, this.breathingClip, this.loopCrossfade, 0f, 0f);
			if (this.lewdBreathingClip != null)
			{
				this._lewdLoop = new CrossfadeLoopPlayer(this.lewdSourceA, this.lewdSourceB, this.lewdBreathingClip, this.loopCrossfade, 0f, 0f);
			}
			this._playerRoot = ((this.clothing != null) ? this.clothing.transform.root : base.transform.root);
			this.RescanFlashGates();
		}

		// Token: 0x06009915 RID: 39189 RVA: 0x003EC50C File Offset: 0x003EA70C
		private void EnsureSources()
		{
			this.breathingSourceA = this.EnsureOne(this.breathingSourceA, "CielBreathingAudioSourceA", null);
			this.breathingSourceB = this.EnsureOne(this.breathingSourceB, "CielBreathingAudioSourceB", null);
			if (this.lewdBreathingClip != null)
			{
				this.lewdSourceA = this.EnsureOne(this.lewdSourceA, "CielLewdBreathingSourceA", this.lewdBreathingClip);
				this.lewdSourceB = this.EnsureOne(this.lewdSourceB, "CielLewdBreathingSourceB", this.lewdBreathingClip);
			}
		}

		// Token: 0x06009916 RID: 39190 RVA: 0x003EC594 File Offset: 0x003EA794
		private AudioSource EnsureOne(AudioSource src, string childName, AudioClip clip = null)
		{
			if (src == null)
			{
				GameObject gameObject = new GameObject(childName);
				gameObject.transform.SetParent(base.transform, false);
				src = gameObject.AddComponent<AudioSource>();
				src.spatialBlend = 0f;
				if (this.debugLog)
				{
					Debug.LogWarning("[CielBreathing] " + childName + " non cablato: creato fallback runtime.");
				}
			}
			src.loop = false;
			src.playOnAwake = false;
			AudioClip audioClip = ((clip != null) ? clip : this.breathingClip);
			if (audioClip != null)
			{
				src.clip = audioClip;
			}
			src.volume = 0f;
			return src;
		}

		// Token: 0x06009917 RID: 39191 RVA: 0x003EC630 File Offset: 0x003EA830
		private void RescanFlashGates()
		{
			if (this._playerRoot == null)
			{
				this._playerRoot = base.transform.root;
			}
			if (this._playerRoot != null)
			{
				this._flashGates = this._playerRoot.GetComponentsInChildren<IFlashGate>(true);
			}
		}

		// Token: 0x06009918 RID: 39192 RVA: 0x003EC67C File Offset: 0x003EA87C
		private bool AnyLewdExposureEngaged()
		{
			IFlashGate[] flashGates = this._flashGates;
			for (int i = 0; i < flashGates.Length; i++)
			{
				if (FlashGateRegistry.IsFlashEngaged(flashGates[i]))
				{
					return true;
				}
				ExposeLingerieController exposeLingerieController = flashGates[i] as ExposeLingerieController;
				if (exposeLingerieController != null && exposeLingerieController != null && exposeLingerieController.isActiveAndEnabled && exposeLingerieController.IsEngaged)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009919 RID: 39193 RVA: 0x003EC6D4 File Offset: 0x003EA8D4
		private void OnEnable()
		{
			if (this.voiceController == null)
			{
				this.voiceController = CielVoiceController.Instance;
			}
			if (this.clothing != null)
			{
				this.clothing.OnCoverageChanged += this.HandleCoverageChanged;
				this._isNaked = !this.clothing.IsFullyClothed;
			}
			if (this.voiceController != null)
			{
				this.voiceController.OnVoiceStarted += this.HandleVoiceStarted;
				this.voiceController.OnVoiceEnded += this.HandleVoiceEnded;
				this._voiceDucking = this.voiceController.IsSpeaking;
			}
			if (this.duckOnUiContext)
			{
				PlayerActivityGate.Changed += this.HandleGateChanged;
				this._uiContextDucking = CielAmbientBreathing.ModalContextActive;
			}
			MasturbationVoiceController.OnMasturbationVoiceActiveChanged += this.HandleMasturbationVoiceChanged;
			this._masturbationActive = MasturbationVoiceController.IsMasturbationVoiceActive;
			if (this.ShouldPlay && this._loop != null)
			{
				this._loop.Play();
			}
			if (this.ShouldPlayLewd && this._lewdLoop != null)
			{
				this._lewdLoop.Play();
			}
		}

		// Token: 0x0600991A RID: 39194 RVA: 0x003EC7F8 File Offset: 0x003EA9F8
		private void OnDisable()
		{
			if (this.clothing != null)
			{
				this.clothing.OnCoverageChanged -= this.HandleCoverageChanged;
			}
			if (this.voiceController != null)
			{
				this.voiceController.OnVoiceStarted -= this.HandleVoiceStarted;
				this.voiceController.OnVoiceEnded -= this.HandleVoiceEnded;
			}
			if (this.duckOnUiContext)
			{
				PlayerActivityGate.Changed -= this.HandleGateChanged;
			}
			MasturbationVoiceController.OnMasturbationVoiceActiveChanged -= this.HandleMasturbationVoiceChanged;
			CrossfadeLoopPlayer loop = this._loop;
			if (loop != null)
			{
				loop.Stop();
			}
			CrossfadeLoopPlayer lewdLoop = this._lewdLoop;
			if (lewdLoop != null)
			{
				lewdLoop.Stop();
			}
			this._currentVol = 0f;
			this._lewdVol = 0f;
		}

		// Token: 0x0600991B RID: 39195 RVA: 0x003EC8C8 File Offset: 0x003EAAC8
		private void HandleMasturbationVoiceChanged(bool active)
		{
			this._masturbationActive = active;
			if (this.debugLog)
			{
				Debug.Log("[CielBreathing] masturbation voice " + (active ? "attiva → duck respiro" : "finita → respiro puo' riprendere"));
			}
			this.SyncPlayState();
		}

		// Token: 0x0600991C RID: 39196 RVA: 0x003EC8FD File Offset: 0x003EAAFD
		private void HandleCoverageChanged(bool isFullyClothed)
		{
			this._isNaked = !isFullyClothed;
			if (this.debugLog)
			{
				Debug.Log(string.Format("[CielBreathing] coverage changed → naked={0}", this._isNaked));
			}
			this.SyncPlayState();
		}

		// Token: 0x0600991D RID: 39197 RVA: 0x003EC931 File Offset: 0x003EAB31
		private void HandleVoiceStarted(CielVoiceSO voice)
		{
			this._voiceDucking = true;
			if (this.debugLog)
			{
				Debug.Log("[CielBreathing] voice started → duck respiro");
			}
			this.SyncPlayState();
		}

		// Token: 0x0600991E RID: 39198 RVA: 0x003EC952 File Offset: 0x003EAB52
		private void HandleVoiceEnded(CielVoiceSO voice)
		{
			this._voiceDucking = false;
			if (this.debugLog)
			{
				Debug.Log("[CielBreathing] voice ended → respiro puo' riprendere");
			}
			this.SyncPlayState();
		}

		// Token: 0x0600991F RID: 39199 RVA: 0x003EC974 File Offset: 0x003EAB74
		private void HandleGateChanged(PlayerActivity _)
		{
			bool modalContextActive = CielAmbientBreathing.ModalContextActive;
			if (this._uiContextDucking == modalContextActive)
			{
				return;
			}
			this._uiContextDucking = modalContextActive;
			if (this.debugLog)
			{
				Debug.Log("[CielBreathing] contesto UI → " + (modalContextActive ? "duck respiro" : "respiro puo' riprendere"));
			}
			this.SyncPlayState();
		}

		// Token: 0x06009920 RID: 39200 RVA: 0x003EC9C4 File Offset: 0x003EABC4
		public void SetEmoteLewdBreathing(bool active)
		{
			if (this._emoteLewdActive == active)
			{
				return;
			}
			this._emoteLewdActive = active;
			if (this.debugLog)
			{
				Debug.Log("[CielBreathing] emote lewd breathing → " + (active ? "ATTIVO" : "finito"));
			}
			this.SyncPlayState();
		}

		// Token: 0x06009921 RID: 39201 RVA: 0x003ECA03 File Offset: 0x003EAC03
		private void SyncPlayState()
		{
			if (this._loop == null)
			{
				return;
			}
			if (this.ShouldPlay && !this._loop.IsPlaying)
			{
				this._loop.Resume();
			}
		}

		// Token: 0x06009922 RID: 39202 RVA: 0x003ECA30 File Offset: 0x003EAC30
		private void Update()
		{
			if (RunModeService.IsAtHome)
			{
				CrossfadeLoopPlayer loop = this._loop;
				if (loop != null)
				{
					loop.Pause();
				}
				CrossfadeLoopPlayer lewdLoop = this._lewdLoop;
				if (lewdLoop != null)
				{
					lewdLoop.Pause();
				}
				this._currentVol = (this._lewdVol = 0f);
				return;
			}
			if (this.flashRescanInterval <= 0f || Time.unscaledTime >= this._nextFlashRescanAt)
			{
				this.RescanFlashGates();
				this._nextFlashRescanAt = Time.unscaledTime + Mathf.Max(0f, this.flashRescanInterval);
			}
			bool flag = this.AnyLewdExposureEngaged();
			if (flag != this._flashActive)
			{
				this._flashActive = flag;
				if (this.debugLog)
				{
					Debug.Log("[CielBreathing] flash " + (this._flashActive ? "ATTIVO → lewd breathing" : "finito → respiro normale"));
				}
			}
			if (this._loop != null)
			{
				if (this.ShouldPlay && !this._loop.IsPlaying)
				{
					this._loop.Resume();
				}
				float num = (this.ShouldPlay ? this.breathingVolume : 0f);
				float num2 = ((num > this._currentVol) ? this.fadeInDuration : this.fadeOutDuration);
				this._currentVol = ((num2 <= 0f) ? num : Mathf.MoveTowards(this._currentVol, num, this.breathingVolume / num2 * Time.unscaledDeltaTime));
				this._loop.Tick(this._currentVol);
				if (!this.ShouldPlay && this._currentVol <= 0.001f && this._loop.IsPlaying)
				{
					this._loop.Pause();
					if (this.debugLog)
					{
						Debug.Log("[CielBreathing] respiro in pausa (fade-out completo)");
					}
				}
			}
			if (this._lewdLoop != null)
			{
				if (this.ShouldPlayLewd && !this._lewdLoop.IsPlaying)
				{
					this._lewdLoop.Resume();
				}
				float num3 = (this.ShouldPlayLewd ? this.breathingVolume : 0f);
				float num4 = ((num3 > this._lewdVol) ? this.fadeInDuration : this.fadeOutDuration);
				this._lewdVol = ((num4 <= 0f) ? num3 : Mathf.MoveTowards(this._lewdVol, num3, this.breathingVolume / num4 * Time.unscaledDeltaTime));
				this._lewdLoop.Tick(this._lewdVol);
				if (!this.ShouldPlayLewd && this._lewdVol <= 0.001f && this._lewdLoop.IsPlaying)
				{
					this._lewdLoop.Pause();
					if (this.debugLog)
					{
						Debug.Log("[CielBreathing] lewd breathing in pausa (fade-out completo)");
					}
				}
			}
		}

		// Token: 0x04009E1B RID: 40475
		[Header("References")]
		[Tooltip("Primo AudioSource dedicato al respiro (per il crossfade loop). GO sotto Ciel_Invector/Audio.")]
		[SerializeField]
		private AudioSource breathingSourceA;

		// Token: 0x04009E1C RID: 40476
		[Tooltip("Secondo AudioSource dedicato al respiro (crossfade loop). GO sotto Ciel_Invector/Audio.")]
		[SerializeField]
		private AudioSource breathingSourceB;

		// Token: 0x04009E1D RID: 40477
		[Tooltip("Clip del respiro in loop.")]
		[SerializeField]
		private AudioClip breathingClip;

		// Token: 0x04009E1E RID: 40478
		[Header("Lewd Breathing (durante il FLASH)")]
		[Tooltip("Clip del respiro 'lewd' in loop, usata AL POSTO del respiro normale mentre un'azione di flash e' ATTIVA (towel, gold bikini, expose bra/panties, swimsuit). Stesso volume del breathing normale. Finito il flash, torna il respiro classico.")]
		[SerializeField]
		private AudioClip lewdBreathingClip;

		// Token: 0x04009E1F RID: 40479
		[Tooltip("Primo AudioSource dedicato al lewd breathing (crossfade loop). GO sotto Ciel_Invector/Audio.")]
		[SerializeField]
		private AudioSource lewdSourceA;

		// Token: 0x04009E20 RID: 40480
		[Tooltip("Secondo AudioSource dedicato al lewd breathing (crossfade loop). GO sotto Ciel_Invector/Audio.")]
		[SerializeField]
		private AudioSource lewdSourceB;

		// Token: 0x04009E21 RID: 40481
		[Tooltip("Ogni quanti secondi ri-scansionare i componenti di flash (IFlashGate) sotto il player: i capi worn possono essere aggiunti/rimossi a runtime (pickup). 0 = ogni frame.")]
		[Min(0f)]
		[SerializeField]
		private float flashRescanInterval = 1f;

		// Token: 0x04009E22 RID: 40482
		[Tooltip("ClothingToggleController da osservare. Auto-risolto in scena se null.")]
		[SerializeField]
		private ClothingToggleController clothing;

		// Token: 0x04009E23 RID: 40483
		[Tooltip("CielVoiceController da osservare per il ducking. Se null usa l'Instance.")]
		[SerializeField]
		private CielVoiceController voiceController;

		// Token: 0x04009E24 RID: 40484
		[Tooltip("CielRunningNude: quando il running e' attivo il breathing si ducka (running ha precedenza). Auto-risolto sullo stesso GO se null.")]
		[SerializeField]
		private CielRunningNude runningNude;

		// Token: 0x04009E25 RID: 40485
		[Header("Volume")]
		[Tooltip("Volume del respiro quando pienamente udibile (basso: e' un sottofondo).")]
		[Range(0f, 1f)]
		[SerializeField]
		private float breathingVolume = 0.18f;

		// Token: 0x04009E26 RID: 40486
		[Header("Loop")]
		[Tooltip("Durata (s) del crossfade al giro del loop. Piu' lungo = giuntura piu' morbida.")]
		[Min(0.02f)]
		[SerializeField]
		private float loopCrossfade = 0.25f;

		// Token: 0x04009E27 RID: 40487
		[Header("Fade")]
		[Tooltip("Durata (s) del fade-in quando il respiro parte/riprende.")]
		[Min(0f)]
		[SerializeField]
		private float fadeInDuration = 0.6f;

		// Token: 0x04009E28 RID: 40488
		[Tooltip("Durata (s) del fade-out quando il respiro si ferma o viene ducked da una voce.")]
		[Min(0f)]
		[SerializeField]
		private float fadeOutDuration = 0.4f;

		// Token: 0x04009E29 RID: 40489
		[Header("UI Context")]
		[Tooltip("Se true, il respiro si ducka (fade-out + pausa) mentre un contesto UI del player è attivo sul PlayerActivityGate (es. PC/CielOS aperto → la Ciel di gioco tace, resta solo quella della customizzazione). Metti false su una Ciel che NON deve mai zittirsi per il contesto UI (es. un'anteprima).")]
		[SerializeField]
		private bool duckOnUiContext = true;

		// Token: 0x04009E2A RID: 40490
		[Header("Debug")]
		[SerializeField]
		private bool debugLog;

		// Token: 0x04009E2B RID: 40491
		private bool _isNaked;

		// Token: 0x04009E2C RID: 40492
		private bool _voiceDucking;

		// Token: 0x04009E2D RID: 40493
		private bool _uiContextDucking;

		// Token: 0x04009E2E RID: 40494
		private bool _flashActive;

		// Token: 0x04009E2F RID: 40495
		private bool _emoteLewdActive;

		// Token: 0x04009E30 RID: 40496
		private bool _masturbationActive;

		// Token: 0x04009E31 RID: 40497
		private float _currentVol;

		// Token: 0x04009E32 RID: 40498
		private float _lewdVol;

		// Token: 0x04009E33 RID: 40499
		private CrossfadeLoopPlayer _loop;

		// Token: 0x04009E34 RID: 40500
		private CrossfadeLoopPlayer _lewdLoop;

		// Token: 0x04009E35 RID: 40501
		private IFlashGate[] _flashGates = Array.Empty<IFlashGate>();

		// Token: 0x04009E36 RID: 40502
		private Transform _playerRoot;

		// Token: 0x04009E37 RID: 40503
		private float _nextFlashRescanAt;
	}
}
