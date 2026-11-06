using System;
using System.Collections.Generic;
using Features.CielOS;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

namespace Features.MasturbationSystem
{
	// Token: 0x02000FF0 RID: 4080
	[DisallowMultipleComponent]
	public class HeartbeatController : MonoBehaviour
	{
		// Token: 0x170013FB RID: 5115
		// (get) Token: 0x06007B8F RID: 31631 RVA: 0x00327AFE File Offset: 0x00325CFE
		public float CurrentIntensity
		{
			get
			{
				return this._currentIntensity;
			}
		}

		// Token: 0x170013FC RID: 5116
		// (get) Token: 0x06007B90 RID: 31632 RVA: 0x00327B06 File Offset: 0x00325D06
		public float TargetIntensity
		{
			get
			{
				return this._targetIntensity;
			}
		}

		// Token: 0x170013FD RID: 5117
		// (get) Token: 0x06007B91 RID: 31633 RVA: 0x00327B0E File Offset: 0x00325D0E
		public float RestingBpm
		{
			get
			{
				return this.restingBpm;
			}
		}

		// Token: 0x170013FE RID: 5118
		// (get) Token: 0x06007B92 RID: 31634 RVA: 0x00327B16 File Offset: 0x00325D16
		public float PeakBpm
		{
			get
			{
				return this.peakBpm;
			}
		}

		// Token: 0x170013FF RID: 5119
		// (get) Token: 0x06007B93 RID: 31635 RVA: 0x00327B1E File Offset: 0x00325D1E
		public float CurrentBpm
		{
			get
			{
				return this.BpmForIntensity(this._currentIntensity);
			}
		}

		// Token: 0x17001400 RID: 5120
		// (get) Token: 0x06007B94 RID: 31636 RVA: 0x00327B2C File Offset: 0x00325D2C
		public float ClockTime
		{
			get
			{
				return this._clockTime;
			}
		}

		// Token: 0x17001401 RID: 5121
		// (get) Token: 0x06007B95 RID: 31637 RVA: 0x00327B34 File Offset: 0x00325D34
		public float ClockDeltaTime
		{
			get
			{
				return this._clockDeltaTime;
			}
		}

		// Token: 0x17001402 RID: 5122
		// (get) Token: 0x06007B96 RID: 31638 RVA: 0x00327B3C File Offset: 0x00325D3C
		public bool IsAudioSuppressed
		{
			get
			{
				return CielOSController.AnyOpen;
			}
		}

		// Token: 0x06007B97 RID: 31639 RVA: 0x00327B43 File Offset: 0x00325D43
		public static float ResolveClockDeltaTime(float captureDeltaTime, float unscaledDeltaTime)
		{
			if (captureDeltaTime <= 0f)
			{
				return Mathf.Max(0f, unscaledDeltaTime);
			}
			return captureDeltaTime;
		}

		// Token: 0x06007B98 RID: 31640 RVA: 0x00327B5C File Offset: 0x00325D5C
		public float BpmForIntensity(float value)
		{
			float num = Mathf.Clamp01(value);
			float num2 = ((this.bpmCurve != null) ? this.bpmCurve.Evaluate(num) : num);
			return Mathf.Lerp(this.restingBpm, this.peakBpm, num2);
		}

		// Token: 0x06007B99 RID: 31641 RVA: 0x00327B9C File Offset: 0x00325D9C
		public float IntensityForBpm(float bpm)
		{
			float num = Mathf.InverseLerp(this.restingBpm, this.peakBpm, bpm);
			if (this.bpmCurve == null)
			{
				return num;
			}
			float num2 = 0f;
			float num3 = 1f;
			for (int i = 0; i < 18; i++)
			{
				float num4 = (num2 + num3) * 0.5f;
				if (Mathf.Clamp01(this.bpmCurve.Evaluate(num4)) < num)
				{
					num2 = num4;
				}
				else
				{
					num3 = num4;
				}
			}
			return (num2 + num3) * 0.5f;
		}

		// Token: 0x06007B9A RID: 31642 RVA: 0x00327C10 File Offset: 0x00325E10
		public void SetSourceIntensity(string sourceId, float value)
		{
			if (string.IsNullOrEmpty(sourceId))
			{
				return;
			}
			this._sources[sourceId] = Mathf.Clamp01(value);
		}

		// Token: 0x06007B9B RID: 31643 RVA: 0x00327C2D File Offset: 0x00325E2D
		public void SetSourceBpm(string sourceId, float bpm)
		{
			this.SetSourceIntensity(sourceId, this.IntensityForBpm(bpm));
		}

		// Token: 0x06007B9C RID: 31644 RVA: 0x00327C40 File Offset: 0x00325E40
		public float GetSourceIntensity(string sourceId)
		{
			if (string.IsNullOrEmpty(sourceId))
			{
				return 0f;
			}
			float num;
			if (!this._sources.TryGetValue(sourceId, out num))
			{
				return 0f;
			}
			return num;
		}

		// Token: 0x06007B9D RID: 31645 RVA: 0x00327C72 File Offset: 0x00325E72
		public void ClearSource(string sourceId)
		{
			if (string.IsNullOrEmpty(sourceId))
			{
				return;
			}
			this._sources.Remove(sourceId);
		}

		// Token: 0x06007B9E RID: 31646 RVA: 0x00327C8A File Offset: 0x00325E8A
		private void Awake()
		{
			this.CacheScaleFeedback();
		}

		// Token: 0x06007B9F RID: 31647 RVA: 0x00327C92 File Offset: 0x00325E92
		private void OnEnable()
		{
			this.CacheScaleFeedback();
			this.RestorePulseVisual();
			this.StopSuppressedAudio();
			this._nextBeatTime = this._clockTime + this.IntervalForValue(this._currentIntensity);
		}

		// Token: 0x06007BA0 RID: 31648 RVA: 0x00327CC0 File Offset: 0x00325EC0
		private void OnDisable()
		{
			if (this.pulseFeedback != null)
			{
				this.pulseFeedback.StopFeedbacks();
				this.pulseFeedback.RestoreInitialValues();
			}
			if (this.audioSource != null)
			{
				this.audioSource.Stop();
			}
			this.RestorePulseVisual();
		}

		// Token: 0x06007BA1 RID: 31649 RVA: 0x00327D10 File Offset: 0x00325F10
		private void Start()
		{
			this.CacheScaleFeedback();
			this._nextBeatTime = this._clockTime + this.IntervalForValue(this._currentIntensity);
			this.UpdateBpmLabel(this._currentIntensity);
		}

		// Token: 0x06007BA2 RID: 31650 RVA: 0x00327D40 File Offset: 0x00325F40
		private void CacheScaleFeedback()
		{
			if (this.pulseFeedback == null)
			{
				return;
			}
			this.pulseFeedback.ForceTimescaleMode = true;
			this.pulseFeedback.ForcedTimescaleMode = TimescaleModes.Unscaled;
			if (this._cachedScaleFeedback == null)
			{
				for (int i = 0; i < this.pulseFeedback.FeedbacksList.Count; i++)
				{
					MMF_Scale mmf_Scale = this.pulseFeedback.FeedbacksList[i] as MMF_Scale;
					if (mmf_Scale != null)
					{
						this._cachedScaleFeedback = mmf_Scale;
						break;
					}
				}
			}
			if (this._cachedScaleFeedback != null)
			{
				this._cachedScaleFeedback.Timing.TimescaleMode = TimescaleModes.Unscaled;
				this._cachedScaleFeedback.Timing.UseScriptDrivenTimescale = true;
				this._cachedScaleFeedback.Timing.ScriptDrivenTime = this._clockTime;
				this._cachedScaleFeedback.Timing.ScriptDrivenDeltaTime = this._clockDeltaTime;
				if (!this._hasPulseRestScale && this._cachedScaleFeedback.AnimateScaleTarget != null)
				{
					this._pulseScaleTarget = this._cachedScaleFeedback.AnimateScaleTarget;
					this._pulseRestScale = this._pulseScaleTarget.localScale;
					this._hasPulseRestScale = true;
				}
			}
			if (this._cachedScaleFeedback == null && this.driveAmplitudeFromValue && this.debugMode)
			{
				Debug.LogWarning("[Heartbeat] driveAmplitudeFromValue=true but no MMF_Scale found in pulseFeedback.");
			}
		}

		// Token: 0x06007BA3 RID: 31651 RVA: 0x00327E79 File Offset: 0x00326079
		private void RestorePulseVisual()
		{
			if (this._hasPulseRestScale && this._pulseScaleTarget != null)
			{
				this._pulseScaleTarget.localScale = this._pulseRestScale;
			}
		}

		// Token: 0x06007BA4 RID: 31652 RVA: 0x00327EA4 File Offset: 0x003260A4
		private void Update()
		{
			this.StopSuppressedAudio();
			this._clockDeltaTime = HeartbeatController.ResolveClockDeltaTime(Time.captureDeltaTime, Time.unscaledDeltaTime);
			this._clockTime += this._clockDeltaTime;
			if (this._cachedScaleFeedback != null)
			{
				this._cachedScaleFeedback.Timing.ScriptDrivenTime = this._clockTime;
				this._cachedScaleFeedback.Timing.ScriptDrivenDeltaTime = this._clockDeltaTime;
			}
			float num = 0f;
			foreach (KeyValuePair<string, float> keyValuePair in this._sources)
			{
				if (keyValuePair.Value > num)
				{
					num = keyValuePair.Value;
				}
			}
			this._targetIntensity = num;
			if (this._targetIntensity > this._currentIntensity + 0.05f)
			{
				float num2 = this._clockTime + this.IntervalForValue(this._targetIntensity);
				this._nextBeatTime = Mathf.Min(this._nextBeatTime, num2);
			}
			float num3 = ((this._targetIntensity > this._currentIntensity) ? this.riseSmoothTime : this.fallSmoothTime);
			this._currentIntensity = Mathf.SmoothDamp(this._currentIntensity, this._targetIntensity, ref this._velocity, num3, float.PositiveInfinity, this._clockDeltaTime);
			if (Mathf.Abs(this._currentIntensity - this._targetIntensity) < 0.001f)
			{
				this._currentIntensity = this._targetIntensity;
				this._velocity = 0f;
			}
			if (this._clockTime < this._nextBeatTime)
			{
				return;
			}
			if (this.driveAmplitudeFromValue && this._cachedScaleFeedback != null)
			{
				this.ApplyAmplitudeFromValue(this._currentIntensity);
			}
			if (this.pulseFeedback != null)
			{
				this.pulseFeedback.PlayFeedbacks();
			}
			this._nextBeatTime = this._clockTime + this.IntervalForValue(this._currentIntensity);
			this.UpdateBpmLabel(this._currentIntensity);
			this.PlayHeartbeatAudio(this._currentIntensity);
			if (this.debugMode)
			{
				Debug.Log(string.Format("[Heartbeat] beat (cur={0:F2}, tgt={1:F2}, next in {2:F2}s)", this._currentIntensity, this._targetIntensity, this.IntervalForValue(this._currentIntensity)));
			}
		}

		// Token: 0x06007BA5 RID: 31653 RVA: 0x003280D8 File Offset: 0x003262D8
		private void PlayHeartbeatAudio(float value)
		{
			if (!this.enableHeartbeatAudio || this.IsAudioSuppressed)
			{
				return;
			}
			if (this.audioSource == null || this.heartbeatClip == null)
			{
				return;
			}
			float num = Mathf.Clamp01(value);
			this.audioSource.pitch = Mathf.Lerp(this.audioPitchAtRest, this.audioPitchAtPeak, num);
			float num2 = Mathf.Lerp(this.audioVolumeAtRest, this.audioVolumeAtPeak, num);
			this.audioSource.Stop();
			this.audioSource.clip = this.heartbeatClip;
			this.audioSource.loop = false;
			this.audioSource.volume = num2;
			this.audioSource.Play();
		}

		// Token: 0x06007BA6 RID: 31654 RVA: 0x00328189 File Offset: 0x00326389
		private void StopSuppressedAudio()
		{
			if (this.IsAudioSuppressed && this.audioSource != null && this.audioSource.isPlaying)
			{
				this.audioSource.Stop();
			}
		}

		// Token: 0x06007BA7 RID: 31655 RVA: 0x003281BC File Offset: 0x003263BC
		private void UpdateBpmLabel(float value)
		{
			if (this.bpmLabel == null)
			{
				return;
			}
			float num = this.BpmForIntensity(value);
			this.bpmLabel.text = num.ToString(string.IsNullOrEmpty(this.bpmLabelFormat) ? "0" : this.bpmLabelFormat);
		}

		// Token: 0x06007BA8 RID: 31656 RVA: 0x0032820C File Offset: 0x0032640C
		private float IntervalForValue(float value)
		{
			return 60f / Mathf.Max(20f, this.BpmForIntensity(value));
		}

		// Token: 0x06007BA9 RID: 31657 RVA: 0x00328228 File Offset: 0x00326428
		private void ApplyAmplitudeFromValue(float value)
		{
			if (this._cachedScaleFeedback == null)
			{
				return;
			}
			float num = Mathf.Clamp01(value);
			float num2 = Mathf.Lerp(this.restingAmplitude, this.peakAmplitude, num);
			float num3 = 1f + num2;
			this._cachedScaleFeedback.RemapCurveZero = 1f;
			this._cachedScaleFeedback.RemapCurveOne = num3;
			this._cachedScaleFeedback.AnimateY = true;
			this._cachedScaleFeedback.AnimateZ = true;
		}

		// Token: 0x04007F20 RID: 32544
		[Header("References")]
		[Tooltip("MMF_Player con dentro un MMF_Scale (single shot). PlayFeedbacks() viene chiamato a ritmo del BPM corrente.")]
		[SerializeField]
		private MMF_Player pulseFeedback;

		// Token: 0x04007F21 RID: 32545
		[Header("BPM Mapping")]
		[Tooltip("Battiti al minuto a intensity=0 (Resting). Esempio: 60 = un beat al secondo.")]
		[Min(20f)]
		[SerializeField]
		private float restingBpm = 70f;

		// Token: 0x04007F22 RID: 32546
		[Tooltip("Battiti al minuto a intensity=1 (max). Esempio: 180 = tre beat al secondo.")]
		[Min(20f)]
		[SerializeField]
		private float peakBpm = 160f;

		// Token: 0x04007F23 RID: 32547
		[Tooltip("Curva di interpolazione del BPM in funzione di intensity (0..1).")]
		[SerializeField]
		private AnimationCurve bpmCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04007F24 RID: 32548
		[Header("Intensity Lerp (asymmetric)")]
		[Tooltip("Secondi per SmoothDamp quando _target SALE (rise). Basso = il cuore reagisce subito a stimoli forti.")]
		[Min(0.01f)]
		[SerializeField]
		private float riseSmoothTime = 0.4f;

		// Token: 0x04007F25 RID: 32549
		[Tooltip("Secondi per SmoothDamp quando _target SCENDE (fall). Alto = il cuore si calma lentamente, verosimile post-orgasm.")]
		[Min(0.1f)]
		[SerializeField]
		private float fallSmoothTime = 2.5f;

		// Token: 0x04007F26 RID: 32550
		[Header("Scale Amplitude")]
		[Tooltip("Se true, l'ampiezza del pulse (RemapZero/One dell'MMF_Scale) viene patchata in base a _currentIntensity.")]
		[SerializeField]
		private bool driveAmplitudeFromValue = true;

		// Token: 0x04007F27 RID: 32551
		[Tooltip("Ampiezza scale a intensity=0 (offset positivo dallo scale base). 0.05 = +5% al picco del beat.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float restingAmplitude = 0.05f;

		// Token: 0x04007F28 RID: 32552
		[Tooltip("Ampiezza scale a intensity=1.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float peakAmplitude = 0.25f;

		// Token: 0x04007F29 RID: 32553
		[Header("Audio (optional)")]
		[Tooltip("Se true, suona un AudioClip a ogni battito (volume e pitch scalano con _currentIntensity).")]
		[SerializeField]
		private bool enableHeartbeatAudio = true;

		// Token: 0x04007F2A RID: 32554
		[Tooltip("AudioSource usato per la riproduzione. Se null, l'audio non parte. Lascialo configurato 2D con Spatial Blend = 0.")]
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x04007F2B RID: 32555
		[Tooltip("Clip del battito. Una sola pulsazione, breve (50-200ms).")]
		[SerializeField]
		private AudioClip heartbeatClip;

		// Token: 0x04007F2C RID: 32556
		[Tooltip("Volume a intensity=0 (Resting). Super basso di default per non disturbare in stato neutro.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float audioVolumeAtRest = 0.05f;

		// Token: 0x04007F2D RID: 32557
		[Tooltip("Volume a intensity=1 (Peak). Audibile ma non saturante.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float audioVolumeAtPeak = 0.75f;

		// Token: 0x04007F2E RID: 32558
		[Tooltip("Pitch a intensity=0 (Resting). 1 = normale.")]
		[Range(0.5f, 2f)]
		[SerializeField]
		private float audioPitchAtRest = 0.9f;

		// Token: 0x04007F2F RID: 32559
		[Tooltip("Pitch a intensity=1 (Peak). >1 = piu' acuto, piu' 'concitato'.")]
		[Range(0.5f, 2f)]
		[SerializeField]
		private float audioPitchAtPeak = 1.25f;

		// Token: 0x04007F30 RID: 32560
		[Header("BPM Label (optional)")]
		[Tooltip("TMP_Text opzionale che mostra il BPM corrente come intero. Aggiornato a ogni beat. Lascia null se non vuoi un display testuale del battito.")]
		[SerializeField]
		private TMP_Text bpmLabel;

		// Token: 0x04007F31 RID: 32561
		[Tooltip("Formato C# numerico per il BPM (es. '0' per intero, 'F1' per un decimale).")]
		[SerializeField]
		private string bpmLabelFormat = "0";

		// Token: 0x04007F32 RID: 32562
		[Header("Debug")]
		[SerializeField]
		private bool debugMode;

		// Token: 0x04007F33 RID: 32563
		private readonly Dictionary<string, float> _sources = new Dictionary<string, float>();

		// Token: 0x04007F34 RID: 32564
		private float _targetIntensity;

		// Token: 0x04007F35 RID: 32565
		private float _currentIntensity;

		// Token: 0x04007F36 RID: 32566
		private float _velocity;

		// Token: 0x04007F37 RID: 32567
		private float _nextBeatTime;

		// Token: 0x04007F38 RID: 32568
		private float _clockTime;

		// Token: 0x04007F39 RID: 32569
		private float _clockDeltaTime;

		// Token: 0x04007F3A RID: 32570
		private MMF_Scale _cachedScaleFeedback;

		// Token: 0x04007F3B RID: 32571
		private Transform _pulseScaleTarget;

		// Token: 0x04007F3C RID: 32572
		private Vector3 _pulseRestScale = Vector3.one;

		// Token: 0x04007F3D RID: 32573
		private bool _hasPulseRestScale;
	}
}
