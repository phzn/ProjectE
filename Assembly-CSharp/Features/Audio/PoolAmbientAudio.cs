using System;
using System.Collections;
using Features.CielVoice;
using Features.MainMenu;
using UnityEngine;

namespace Features.Audio
{
	// Token: 0x0200152A RID: 5418
	[DisallowMultipleComponent]
	public class PoolAmbientAudio : MonoBehaviour
	{
		// Token: 0x170019D4 RID: 6612
		// (get) Token: 0x0600A7BD RID: 42941 RVA: 0x00461741 File Offset: 0x0045F941
		public float ArrivalVoiceDuckGain
		{
			get
			{
				return this._arrivalDuckGain;
			}
		}

		// Token: 0x170019D5 RID: 6613
		// (get) Token: 0x0600A7BE RID: 42942 RVA: 0x00461749 File Offset: 0x0045F949
		public bool IsArrivalVoiceDucking
		{
			get
			{
				return this._arrivalDuckTarget < 0.999f || this._arrivalDuckGain < 0.999f;
			}
		}

		// Token: 0x0600A7BF RID: 42943 RVA: 0x00461767 File Offset: 0x0045F967
		private void Awake()
		{
			if (this.autoResolveClip)
			{
				this.ResolveClip();
			}
			this._source = this.CreateSource();
		}

		// Token: 0x0600A7C0 RID: 42944 RVA: 0x00461784 File Offset: 0x0045F984
		private void OnEnable()
		{
			this.MaintainVoiceBinding();
			if (this._source != null && this.ambientClip != null && !this._source.isPlaying)
			{
				this._source.Play();
			}
			this.ApplyVolume();
		}

		// Token: 0x0600A7C1 RID: 42945 RVA: 0x004617D1 File Offset: 0x0045F9D1
		private void LateUpdate()
		{
			this.MaintainVoiceBinding();
			this._arrivalDuckGain = Mathf.MoveTowards(this._arrivalDuckGain, this._arrivalDuckTarget, this._arrivalDuckSpeed * Time.unscaledDeltaTime);
			this.ApplyVolume();
		}

		// Token: 0x0600A7C2 RID: 42946 RVA: 0x00461804 File Offset: 0x0045FA04
		private void OnDisable()
		{
			if (this._voiceRestoreRoutine != null)
			{
				base.StopCoroutine(this._voiceRestoreRoutine);
				this._voiceRestoreRoutine = null;
			}
			this.UnbindVoiceController();
			this._arrivalDuckTarget = 1f;
			this._arrivalDuckGain = 1f;
			this._arrivalDuckSpeed = 0f;
			this.ApplyVolume();
			if (this._source != null)
			{
				this._source.Stop();
			}
		}

		// Token: 0x0600A7C3 RID: 42947 RVA: 0x00461874 File Offset: 0x0045FA74
		private AudioSource CreateSource()
		{
			GameObject gameObject = new GameObject("PoolAmbientSource");
			gameObject.transform.SetParent(base.transform, false);
			gameObject.transform.localPosition = Vector3.zero;
			AudioSource audioSource = gameObject.AddComponent<AudioSource>();
			audioSource.playOnAwake = false;
			audioSource.loop = true;
			audioSource.spatialBlend = this.spatialBlend;
			audioSource.rolloffMode = AudioRolloffMode.Linear;
			audioSource.minDistance = this.minDistance;
			audioSource.maxDistance = this.maxDistance;
			audioSource.dopplerLevel = 0f;
			audioSource.clip = this.ambientClip;
			audioSource.volume = Mathf.Clamp01(this.volume * this._arrivalDuckGain);
			GameAudioRouter.Route(audioSource, GameAudioRouter.Channel.Sfx);
			if (this.ambientClip != null)
			{
				audioSource.Play();
			}
			return audioSource;
		}

		// Token: 0x0600A7C4 RID: 42948 RVA: 0x00461937 File Offset: 0x0045FB37
		public void SetVolume(float v)
		{
			this.volume = Mathf.Clamp01(v);
			this.ApplyVolume();
		}

		// Token: 0x0600A7C5 RID: 42949 RVA: 0x0046194B File Offset: 0x0045FB4B
		private void ApplyVolume()
		{
			if (this._source != null)
			{
				this._source.volume = Mathf.Clamp01(this.volume * this._arrivalDuckGain);
			}
		}

		// Token: 0x0600A7C6 RID: 42950 RVA: 0x00461978 File Offset: 0x0045FB78
		private void MaintainVoiceBinding()
		{
			CielVoiceController instance = CielVoiceController.Instance;
			if (instance == null)
			{
				this.UnbindVoiceController();
				if (this._arrivalDuckTarget < 1f)
				{
					this.SetArrivalDuckTarget(1f, this.arrivalVoiceRestore);
				}
				return;
			}
			if (this._voiceController == instance)
			{
				return;
			}
			this.UnbindVoiceController();
			this.BindVoiceController(instance);
			if (instance.IsSpeaking && PoolAmbientAudio.IsPoolArrivalVoice(instance.CurrentVoice))
			{
				this.BeginArrivalVoiceDuck();
				return;
			}
			if (this._arrivalDuckTarget < 1f)
			{
				this.CancelPendingRestore();
				this.SetArrivalDuckTarget(1f, this.arrivalVoiceRestore);
			}
		}

		// Token: 0x0600A7C7 RID: 42951 RVA: 0x00461A18 File Offset: 0x0045FC18
		private void BindVoiceController(CielVoiceController controller)
		{
			if (this._voiceController == controller)
			{
				return;
			}
			this.UnbindVoiceController();
			this._voiceController = controller;
			this._voiceController.OnVoiceStarted += this.HandleVoiceStarted;
			this._voiceController.OnVoiceEnded += this.HandleVoiceEnded;
		}

		// Token: 0x0600A7C8 RID: 42952 RVA: 0x00461A70 File Offset: 0x0045FC70
		private void UnbindVoiceController()
		{
			if (this._voiceController == null)
			{
				return;
			}
			this._voiceController.OnVoiceStarted -= this.HandleVoiceStarted;
			this._voiceController.OnVoiceEnded -= this.HandleVoiceEnded;
			this._voiceController = null;
		}

		// Token: 0x0600A7C9 RID: 42953 RVA: 0x00461AC1 File Offset: 0x0045FCC1
		private void HandleVoiceStarted(CielVoiceSO voice)
		{
			if (PoolAmbientAudio.IsPoolArrivalVoice(voice))
			{
				this.BeginArrivalVoiceDuck();
			}
		}

		// Token: 0x0600A7CA RID: 42954 RVA: 0x00461AD1 File Offset: 0x0045FCD1
		private void BeginArrivalVoiceDuck()
		{
			this.CancelPendingRestore();
			this.SetArrivalDuckTarget(this.arrivalVoiceAmbientMultiplier, this.arrivalVoiceDuckIn);
		}

		// Token: 0x0600A7CB RID: 42955 RVA: 0x00461AEB File Offset: 0x0045FCEB
		private void HandleVoiceEnded(CielVoiceSO voice)
		{
			if (!PoolAmbientAudio.IsPoolArrivalVoice(voice))
			{
				return;
			}
			this.CancelPendingRestore();
			this._voiceRestoreRoutine = base.StartCoroutine(this.RestoreAfterVoicePlayback());
		}

		// Token: 0x0600A7CC RID: 42956 RVA: 0x00461B0E File Offset: 0x0045FD0E
		private IEnumerator RestoreAfterVoicePlayback()
		{
			yield return null;
			while (this._voiceController != null && this._voiceController.CurrentVoice == null && this._voiceController.VoiceSource != null && this._voiceController.VoiceSource.isPlaying)
			{
				yield return null;
			}
			this.SetArrivalDuckTarget(1f, this.arrivalVoiceRestore);
			this._voiceRestoreRoutine = null;
			yield break;
		}

		// Token: 0x0600A7CD RID: 42957 RVA: 0x00461B1D File Offset: 0x0045FD1D
		private void CancelPendingRestore()
		{
			if (this._voiceRestoreRoutine == null)
			{
				return;
			}
			base.StopCoroutine(this._voiceRestoreRoutine);
			this._voiceRestoreRoutine = null;
		}

		// Token: 0x0600A7CE RID: 42958 RVA: 0x00461B3C File Offset: 0x0045FD3C
		private void SetArrivalDuckTarget(float target, float duration)
		{
			this._arrivalDuckTarget = Mathf.Clamp01(target);
			float num = Mathf.Abs(this._arrivalDuckTarget - this._arrivalDuckGain);
			this._arrivalDuckSpeed = ((num <= Mathf.Epsilon) ? 0f : (num / Mathf.Max(0.01f, duration)));
		}

		// Token: 0x0600A7CF RID: 42959 RVA: 0x00461B8A File Offset: 0x0045FD8A
		private static bool IsPoolArrivalVoice(CielVoiceSO voice)
		{
			return voice != null && voice.ResolvedId.StartsWith("poi_pool_", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x0600A7D0 RID: 42960 RVA: 0x00004F31 File Offset: 0x00003131
		private void ResolveClip()
		{
		}

		// Token: 0x0400AC70 RID: 44144
		[Header("Clip")]
		[Tooltip("Looping pool ambience. Invector waterSound.")]
		[SerializeField]
		private AudioClip ambientClip;

		// Token: 0x0400AC71 RID: 44145
		[Header("Volume")]
		[SerializeField]
		[Range(0f, 1f)]
		private float volume = 0.16f;

		// Token: 0x0400AC72 RID: 44146
		[Header("Spatial 3D")]
		[SerializeField]
		[Range(0f, 1f)]
		private float spatialBlend = 1f;

		// Token: 0x0400AC73 RID: 44147
		[Tooltip("Distance (m) at which the ambience is at full volume — roughly the pool footprint.")]
		[SerializeField]
		private float minDistance = 3f;

		// Token: 0x0400AC74 RID: 44148
		[Tooltip("Distance (m) beyond which the ambience is fully attenuated.")]
		[SerializeField]
		private float maxDistance = 26f;

		// Token: 0x0400AC75 RID: 44149
		[Header("Auto-resolve clip")]
		[SerializeField]
		private bool autoResolveClip = true;

		// Token: 0x0400AC76 RID: 44150
		[Header("Ciel Arrival Voice Ducking")]
		[Tooltip("Moltiplicatore dell'ambiente piscina durante la voce di arrivo di Ciel (0.1 = -20 dB circa).")]
		[SerializeField]
		[Range(0f, 1f)]
		private float arrivalVoiceAmbientMultiplier = 0.1f;

		// Token: 0x0400AC77 RID: 44151
		[Tooltip("Tempo impiegato dall'ambiente piscina per abbassarsi quando Ciel inizia a parlare.")]
		[SerializeField]
		[Min(0.01f)]
		private float arrivalVoiceDuckIn = 0.2f;

		// Token: 0x0400AC78 RID: 44152
		[Tooltip("Tempo impiegato dall'ambiente piscina per tornare gradualmente al volume normale.")]
		[SerializeField]
		[Min(0.05f)]
		private float arrivalVoiceRestore = 2.8f;

		// Token: 0x0400AC79 RID: 44153
		private AudioSource _source;

		// Token: 0x0400AC7A RID: 44154
		private CielVoiceController _voiceController;

		// Token: 0x0400AC7B RID: 44155
		private Coroutine _voiceRestoreRoutine;

		// Token: 0x0400AC7C RID: 44156
		private float _arrivalDuckGain = 1f;

		// Token: 0x0400AC7D RID: 44157
		private float _arrivalDuckTarget = 1f;

		// Token: 0x0400AC7E RID: 44158
		private float _arrivalDuckSpeed;
	}
}
