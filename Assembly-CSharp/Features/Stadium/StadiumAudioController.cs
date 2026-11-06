using System;
using System.Collections;
using Features.CielVoice;
using UnityEngine;

namespace Features.Stadium
{
	// Token: 0x02000C37 RID: 3127
	public class StadiumAudioController : MonoBehaviour
	{
		// Token: 0x14000094 RID: 148
		// (add) Token: 0x06005A1C RID: 23068 RVA: 0x00234A18 File Offset: 0x00232C18
		// (remove) Token: 0x06005A1D RID: 23069 RVA: 0x00234A50 File Offset: 0x00232C50
		public event Action HypeStarted;

		// Token: 0x14000095 RID: 149
		// (add) Token: 0x06005A1E RID: 23070 RVA: 0x00234A88 File Offset: 0x00232C88
		// (remove) Token: 0x06005A1F RID: 23071 RVA: 0x00234AC0 File Offset: 0x00232CC0
		public event Action HypeEnded;

		// Token: 0x17000ED4 RID: 3796
		// (get) Token: 0x06005A20 RID: 23072 RVA: 0x00234AF5 File Offset: 0x00232CF5
		public bool IsHypePlaying
		{
			get
			{
				return this._hypeActive;
			}
		}

		// Token: 0x17000ED5 RID: 3797
		// (get) Token: 0x06005A21 RID: 23073 RVA: 0x00234AFD File Offset: 0x00232CFD
		public float HypeStartDuration
		{
			get
			{
				if (!(this.hypeStartSource != null) || !(this.hypeStartSource.clip != null))
				{
					return 0f;
				}
				return this.hypeStartSource.clip.length;
			}
		}

		// Token: 0x17000ED6 RID: 3798
		// (get) Token: 0x06005A22 RID: 23074 RVA: 0x00234B36 File Offset: 0x00232D36
		public float ArrivalVoiceDuckGain
		{
			get
			{
				return this._arrivalDuckGain;
			}
		}

		// Token: 0x17000ED7 RID: 3799
		// (get) Token: 0x06005A23 RID: 23075 RVA: 0x00234B3E File Offset: 0x00232D3E
		public bool IsArrivalVoiceDucking
		{
			get
			{
				return this._arrivalDuckTarget < 0.999f || this._arrivalDuckGain < 0.999f;
			}
		}

		// Token: 0x06005A24 RID: 23076 RVA: 0x00234B5C File Offset: 0x00232D5C
		private void OnEnable()
		{
			this.MaintainVoiceBinding();
		}

		// Token: 0x06005A25 RID: 23077 RVA: 0x00234B64 File Offset: 0x00232D64
		private void Start()
		{
			this.InitAmbient();
			this.InitHype();
		}

		// Token: 0x06005A26 RID: 23078 RVA: 0x00234B72 File Offset: 0x00232D72
		private void LateUpdate()
		{
			this.MaintainVoiceBinding();
			this._arrivalDuckGain = Mathf.MoveTowards(this._arrivalDuckGain, this._arrivalDuckTarget, this._arrivalDuckSpeed * Time.unscaledDeltaTime);
			this.ApplyCrowdVolumes();
		}

		// Token: 0x06005A27 RID: 23079 RVA: 0x00234BA4 File Offset: 0x00232DA4
		private void OnDisable()
		{
			if (this._hypeRoutine != null)
			{
				base.StopCoroutine(this._hypeRoutine);
				this._hypeRoutine = null;
			}
			this._hypeHeld = false;
			if (this.hypeStartSource != null)
			{
				this.hypeStartSource.Stop();
			}
			if (this.hypeLoopSource != null)
			{
				this.hypeLoopSource.Stop();
			}
			this._hypeStartEnvelope = 0f;
			this._hypeLoopEnvelope = 0f;
			if (this._hypeActive)
			{
				this._hypeActive = false;
				Action hypeEnded = this.HypeEnded;
				if (hypeEnded != null)
				{
					hypeEnded();
				}
			}
			if (this._voiceRestoreRoutine != null)
			{
				base.StopCoroutine(this._voiceRestoreRoutine);
				this._voiceRestoreRoutine = null;
			}
			this.UnbindVoiceController();
			this._arrivalDuckTarget = 1f;
			this._arrivalDuckGain = 1f;
			this._arrivalDuckSpeed = 0f;
			this.ApplyCrowdVolumes();
		}

		// Token: 0x06005A28 RID: 23080 RVA: 0x00234C84 File Offset: 0x00232E84
		public void StartHype()
		{
			if (!base.isActiveAndEnabled || this.hypeStartSource == null || this.hypeLoopSource == null || this.hypeLoopSource.clip == null)
			{
				return;
			}
			this._hypeHeld = true;
			if (this._hypeRoutine != null)
			{
				return;
			}
			this._hypeRoutine = base.StartCoroutine(this.HypeRoutine());
		}

		// Token: 0x06005A29 RID: 23081 RVA: 0x00234CEB File Offset: 0x00232EEB
		public void HoldHype()
		{
			this._hypeHeld = true;
		}

		// Token: 0x06005A2A RID: 23082 RVA: 0x00234CF4 File Offset: 0x00232EF4
		public void ReleaseHype()
		{
			this._hypeHeld = false;
		}

		// Token: 0x06005A2B RID: 23083 RVA: 0x00234D00 File Offset: 0x00232F00
		private void InitAmbient()
		{
			if (this.ambientSource == null || this.ambientSource.clip == null)
			{
				Debug.LogWarning("[StadiumAudio] Ambient source / clip missing — ambient disabled.", this);
				return;
			}
			this.ambientSource.loop = true;
			this._ambientEnvelope = 0f;
			this.ApplyCrowdVolumes();
			this.ambientSource.Play();
			base.StartCoroutine(this.FadeAmbientEnvelope(0f, this.ambientBaseVolume, this.ambientStartupFadeIn));
		}

		// Token: 0x06005A2C RID: 23084 RVA: 0x00234D80 File Offset: 0x00232F80
		private void InitHype()
		{
			if (this.hypeStartSource != null)
			{
				this.hypeStartSource.loop = false;
				this.hypeStartSource.playOnAwake = false;
				this._hypeStartEnvelope = this.hypeStartVolume;
				this.hypeStartSource.Stop();
				if (this.hypeStartSource.clip != null)
				{
					this.hypeStartSource.clip.LoadAudioData();
				}
			}
			if (this.hypeLoopSource != null)
			{
				this.hypeLoopSource.loop = true;
				this.hypeLoopSource.playOnAwake = false;
				this._hypeLoopEnvelope = 0f;
				this.hypeLoopSource.Stop();
				if (this.hypeLoopSource.clip != null)
				{
					this.hypeLoopSource.clip.LoadAudioData();
				}
			}
			this.ApplyCrowdVolumes();
		}

		// Token: 0x06005A2D RID: 23085 RVA: 0x00234E54 File Offset: 0x00233054
		private IEnumerator HypeRoutine()
		{
			this._hypeActive = true;
			Action hypeStarted = this.HypeStarted;
			if (hypeStarted != null)
			{
				hypeStarted();
			}
			this._hypeStartEnvelope = this.hypeStartVolume;
			this._hypeLoopEnvelope = 0f;
			this.ApplyCrowdVolumes();
			float startLen = ((this.hypeStartSource.clip != null) ? (this.hypeStartSource.clip.length / Mathf.Max(0.01f, Mathf.Abs(this.hypeStartSource.pitch))) : 0f);
			if (startLen > 0f)
			{
				this.hypeStartSource.Play();
			}
			double startDspTime = AudioSettings.dspTime;
			float overlap = Mathf.Min(this.hypeStartToLoopCrossfade, startLen);
			double transitionStart = 0.0;
			float transitionDuration = 0f;
			bool loopStarted = false;
			while (AudioSettings.dspTime - startDspTime < (double)startLen)
			{
				float num = (float)(AudioSettings.dspTime - startDspTime);
				if (!loopStarted && this._hypeHeld && num >= startLen - overlap)
				{
					this.hypeLoopSource.Play();
					loopStarted = true;
					transitionStart = AudioSettings.dspTime;
					transitionDuration = Mathf.Max(0.01f, startLen - num);
				}
				if (loopStarted)
				{
					float num2 = Mathf.Clamp01((float)(AudioSettings.dspTime - transitionStart) / transitionDuration);
					this._hypeStartEnvelope = this.hypeStartVolume * Mathf.Cos(num2 * 3.1415927f * 0.5f);
					this._hypeLoopEnvelope = this.hypeLoopVolume * Mathf.Sin(num2 * 3.1415927f * 0.5f);
					this.ApplyCrowdVolumes();
				}
				yield return null;
			}
			this.hypeStartSource.Stop();
			this._hypeStartEnvelope = 0f;
			if (loopStarted)
			{
				this._hypeLoopEnvelope = this.hypeLoopVolume;
			}
			if (!loopStarted && this._hypeHeld)
			{
				this.hypeLoopSource.Play();
				loopStarted = true;
			}
			while (loopStarted && (this._hypeHeld || this._hypeLoopEnvelope > 0f))
			{
				float num3 = (this._hypeHeld ? this.hypeLoopVolume : 0f);
				float num4 = (this._hypeHeld ? this.hypeLoopFadeIn : this.hypeLoopFadeOut);
				this._hypeLoopEnvelope = Mathf.MoveTowards(this._hypeLoopEnvelope, num3, this.hypeLoopVolume * Time.unscaledDeltaTime / Mathf.Max(0.01f, num4));
				this.ApplyCrowdVolumes();
				yield return null;
			}
			this.hypeLoopSource.Stop();
			this._hypeLoopEnvelope = 0f;
			this.ApplyCrowdVolumes();
			this._hypeActive = false;
			Action hypeEnded = this.HypeEnded;
			if (hypeEnded != null)
			{
				hypeEnded();
			}
			this._hypeRoutine = null;
			yield break;
		}

		// Token: 0x06005A2E RID: 23086 RVA: 0x00234E63 File Offset: 0x00233063
		private IEnumerator FadeAmbientEnvelope(float from, float to, float duration)
		{
			this._ambientEnvelope = from;
			float elapsed = 0f;
			while (elapsed < duration)
			{
				elapsed += Time.unscaledDeltaTime;
				this._ambientEnvelope = Mathf.Lerp(from, to, elapsed / duration);
				this.ApplyCrowdVolumes();
				yield return null;
			}
			this._ambientEnvelope = to;
			this.ApplyCrowdVolumes();
			yield break;
		}

		// Token: 0x06005A2F RID: 23087 RVA: 0x00234E88 File Offset: 0x00233088
		private void ApplyCrowdVolumes()
		{
			float num = Mathf.Clamp01(this._arrivalDuckGain);
			if (this.ambientSource != null)
			{
				this.ambientSource.volume = Mathf.Clamp01(this._ambientEnvelope * num);
			}
			if (this.hypeStartSource != null)
			{
				this.hypeStartSource.volume = Mathf.Clamp01(this._hypeStartEnvelope * num);
			}
			if (this.hypeLoopSource != null)
			{
				this.hypeLoopSource.volume = Mathf.Clamp01(this._hypeLoopEnvelope * num);
			}
		}

		// Token: 0x06005A30 RID: 23088 RVA: 0x00234F14 File Offset: 0x00233114
		private void MaintainVoiceBinding()
		{
			CielVoiceController instance = CielVoiceController.Instance;
			if (instance == null)
			{
				this.UnbindVoiceController();
				this._voiceController = null;
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
			if (instance.IsSpeaking && StadiumAudioController.IsStadiumArrivalVoice(instance.CurrentVoice))
			{
				this.BeginArrivalVoiceDuck();
				return;
			}
			if (this._arrivalDuckTarget < 1f)
			{
				if (this._voiceRestoreRoutine != null)
				{
					base.StopCoroutine(this._voiceRestoreRoutine);
					this._voiceRestoreRoutine = null;
				}
				this.SetArrivalDuckTarget(1f, this.arrivalVoiceRestore);
			}
		}

		// Token: 0x06005A31 RID: 23089 RVA: 0x00234FD0 File Offset: 0x002331D0
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

		// Token: 0x06005A32 RID: 23090 RVA: 0x00235028 File Offset: 0x00233228
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

		// Token: 0x06005A33 RID: 23091 RVA: 0x00235079 File Offset: 0x00233279
		private void HandleVoiceStarted(CielVoiceSO voice)
		{
			if (StadiumAudioController.IsStadiumArrivalVoice(voice))
			{
				this.BeginArrivalVoiceDuck();
			}
		}

		// Token: 0x06005A34 RID: 23092 RVA: 0x00235089 File Offset: 0x00233289
		private void BeginArrivalVoiceDuck()
		{
			if (this._voiceRestoreRoutine != null)
			{
				base.StopCoroutine(this._voiceRestoreRoutine);
				this._voiceRestoreRoutine = null;
			}
			this.SetArrivalDuckTarget(this.arrivalVoiceCrowdMultiplier, this.arrivalVoiceDuckIn);
		}

		// Token: 0x06005A35 RID: 23093 RVA: 0x002350B8 File Offset: 0x002332B8
		private void HandleVoiceEnded(CielVoiceSO voice)
		{
			if (!StadiumAudioController.IsStadiumArrivalVoice(voice))
			{
				return;
			}
			if (this._voiceRestoreRoutine != null)
			{
				base.StopCoroutine(this._voiceRestoreRoutine);
			}
			this._voiceRestoreRoutine = base.StartCoroutine(this.RestoreAfterVoicePlayback());
		}

		// Token: 0x06005A36 RID: 23094 RVA: 0x002350E9 File Offset: 0x002332E9
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

		// Token: 0x06005A37 RID: 23095 RVA: 0x002350F8 File Offset: 0x002332F8
		private void SetArrivalDuckTarget(float target, float duration)
		{
			this._arrivalDuckTarget = Mathf.Clamp01(target);
			float num = Mathf.Abs(this._arrivalDuckTarget - this._arrivalDuckGain);
			this._arrivalDuckSpeed = ((num <= Mathf.Epsilon) ? 0f : (num / Mathf.Max(0.01f, duration)));
		}

		// Token: 0x06005A38 RID: 23096 RVA: 0x00235146 File Offset: 0x00233346
		private static bool IsStadiumArrivalVoice(CielVoiceSO voice)
		{
			return voice != null && voice.ResolvedId.StartsWith("poi_stadium_", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x04005B0D RID: 23309
		[Header("Ambient Base (3D, looped)")]
		[SerializeField]
		private AudioSource ambientSource;

		// Token: 0x04005B0E RID: 23310
		[Header("Hype Start (3D, one-shot)")]
		[SerializeField]
		private AudioSource hypeStartSource;

		// Token: 0x04005B0F RID: 23311
		[Header("Hype Loop (3D, looped while held)")]
		[SerializeField]
		private AudioSource hypeLoopSource;

		// Token: 0x04005B10 RID: 23312
		[Header("Volume Levels")]
		[Range(0f, 1f)]
		[SerializeField]
		private float ambientBaseVolume = 0.6f;

		// Token: 0x04005B11 RID: 23313
		[Range(0f, 1f)]
		[SerializeField]
		private float hypeStartVolume = 0.9f;

		// Token: 0x04005B12 RID: 23314
		[Range(0f, 1f)]
		[SerializeField]
		private float hypeLoopVolume = 0.9f;

		// Token: 0x04005B13 RID: 23315
		[Header("Ambient Fades")]
		[Min(0.05f)]
		[SerializeField]
		private float ambientStartupFadeIn = 2f;

		// Token: 0x04005B14 RID: 23316
		[Header("Hype Fades")]
		[Tooltip("Equal-power overlap from the opening cheer into the seamless sustained crowd loop.")]
		[Min(0.05f)]
		[SerializeField]
		private float hypeStartToLoopCrossfade = 1.2f;

		// Token: 0x04005B15 RID: 23317
		[Min(0.01f)]
		[SerializeField]
		private float hypeLoopFadeIn = 0.2f;

		// Token: 0x04005B16 RID: 23318
		[Min(0.05f)]
		[SerializeField]
		private float hypeLoopFadeOut = 1.2f;

		// Token: 0x04005B17 RID: 23319
		[Header("Ciel Arrival Voice Ducking")]
		[Tooltip("Moltiplicatore del pubblico durante la voce di arrivo di Ciel (0.1 = -20 dB circa).")]
		[Range(0f, 1f)]
		[SerializeField]
		private float arrivalVoiceCrowdMultiplier = 0.1f;

		// Token: 0x04005B18 RID: 23320
		[Tooltip("Tempo impiegato dal pubblico per abbassarsi quando Ciel inizia a parlare.")]
		[Min(0.01f)]
		[SerializeField]
		private float arrivalVoiceDuckIn = 0.2f;

		// Token: 0x04005B19 RID: 23321
		[Tooltip("Tempo impiegato dal pubblico per tornare gradualmente al volume normale.")]
		[Min(0.05f)]
		[SerializeField]
		private float arrivalVoiceRestore = 2.8f;

		// Token: 0x04005B1C RID: 23324
		private bool _hypeHeld;

		// Token: 0x04005B1D RID: 23325
		private bool _hypeActive;

		// Token: 0x04005B1E RID: 23326
		private Coroutine _hypeRoutine;

		// Token: 0x04005B1F RID: 23327
		private float _ambientEnvelope;

		// Token: 0x04005B20 RID: 23328
		private float _hypeStartEnvelope;

		// Token: 0x04005B21 RID: 23329
		private float _hypeLoopEnvelope;

		// Token: 0x04005B22 RID: 23330
		private float _arrivalDuckGain = 1f;

		// Token: 0x04005B23 RID: 23331
		private float _arrivalDuckTarget = 1f;

		// Token: 0x04005B24 RID: 23332
		private float _arrivalDuckSpeed;

		// Token: 0x04005B25 RID: 23333
		private CielVoiceController _voiceController;

		// Token: 0x04005B26 RID: 23334
		private Coroutine _voiceRestoreRoutine;
	}
}
