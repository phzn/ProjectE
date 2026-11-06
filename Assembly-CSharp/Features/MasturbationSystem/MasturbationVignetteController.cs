using System;
using ProjectExhibitionist.Core.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Features.MasturbationSystem
{
	// Token: 0x02000FF7 RID: 4087
	[DisallowMultipleComponent]
	public class MasturbationVignetteController : MonoBehaviour
	{
		// Token: 0x1700140E RID: 5134
		// (get) Token: 0x06007BE5 RID: 31717 RVA: 0x0032970C File Offset: 0x0032790C
		public bool IsOverriding
		{
			get
			{
				return this._override;
			}
		}

		// Token: 0x06007BE6 RID: 31718 RVA: 0x00329714 File Offset: 0x00327914
		private void Start()
		{
			if (this.meter == null)
			{
				this.meter = OrgasmMeterController.Instance;
			}
			if (this.heartbeat == null)
			{
				this.heartbeat = Object.FindObjectOfType<HeartbeatController>();
			}
			this.ResolveVolume();
			this.ResolveVignette();
			this.HookMeter();
			this.ResumeMeter();
		}

		// Token: 0x06007BE7 RID: 31719 RVA: 0x0032976B File Offset: 0x0032796B
		private void OnEnable()
		{
			this.HookMeter();
			this.ResumeMeter();
		}

		// Token: 0x06007BE8 RID: 31720 RVA: 0x00329779 File Offset: 0x00327979
		private void ResumeMeter()
		{
			if (this.meter != null && this.meter.State != OrgasmMeterController.MeterState.Resting)
			{
				this.HandleFillingStart();
			}
		}

		// Token: 0x06007BE9 RID: 31721 RVA: 0x0032979C File Offset: 0x0032799C
		private void OnDestroy()
		{
			this.UnhookMeter();
			this.RestoreBaseline();
		}

		// Token: 0x06007BEA RID: 31722 RVA: 0x0032979C File Offset: 0x0032799C
		private void OnDisable()
		{
			this.UnhookMeter();
			this.RestoreBaseline();
		}

		// Token: 0x06007BEB RID: 31723 RVA: 0x003297AA File Offset: 0x003279AA
		private void ResolveVolume()
		{
			this.targetVolume = GameplayVignetteVolumeResolver.Resolve(this.targetVolume);
			if (this.targetVolume != null)
			{
				return;
			}
			if (this.debugMode)
			{
				Debug.LogWarning("[MasturbationVignette] No global Volume found in scene; vignette will be inactive.");
			}
		}

		// Token: 0x06007BEC RID: 31724 RVA: 0x003297E0 File Offset: 0x003279E0
		private void ResolveVignette()
		{
			if (this.targetVolume == null || !this.targetVolume.isActiveAndEnabled || this.targetVolume.profile == null)
			{
				return;
			}
			this._resolvedProfile = this.targetVolume.profile;
			if (this._resolvedProfile.TryGet<Vignette>(out this._vignette))
			{
				if (this.debugMode)
				{
					Debug.Log("[MasturbationVignette] Found Vignette override in volume profile.");
				}
				return;
			}
			this._vignette = this._resolvedProfile.Add<Vignette>(true);
			if (this.debugMode)
			{
				Debug.Log("[MasturbationVignette] Added runtime Vignette override.");
			}
		}

		// Token: 0x06007BED RID: 31725 RVA: 0x00329877 File Offset: 0x00327A77
		private void RestoreBaseline()
		{
			GameplayVignetteMixer.Release(this._vignette, this);
			this._override = false;
			this._meterActive = false;
		}

		// Token: 0x06007BEE RID: 31726 RVA: 0x00329894 File Offset: 0x00327A94
		private void HookMeter()
		{
			if (this.meter == null || this._meterHooked)
			{
				return;
			}
			this.meter.OnFillingStart += this.HandleFillingStart;
			this.meter.OnEmptied += this.HandleEmptied;
			this._meterHooked = true;
		}

		// Token: 0x06007BEF RID: 31727 RVA: 0x003298F0 File Offset: 0x00327AF0
		private void UnhookMeter()
		{
			if (this.meter != null && this._meterHooked)
			{
				this.meter.OnFillingStart -= this.HandleFillingStart;
				this.meter.OnEmptied -= this.HandleEmptied;
			}
			this._meterHooked = false;
		}

		// Token: 0x06007BF0 RID: 31728 RVA: 0x00329948 File Offset: 0x00327B48
		private void HandleFillingStart()
		{
			if (!base.isActiveAndEnabled || this._vignette == null)
			{
				return;
			}
			if (!this._override)
			{
				GameplayVignetteMixer.Set(this._vignette, this, 20, 0f, new Color?(this.restingColor), new float?(this.smoothness));
			}
			this._override = true;
			this._meterActive = true;
			this._displayedValue = 0f;
			this._phase = 0f;
			if (this.debugMode)
			{
				Debug.Log("[MasturbationVignette] Activated (FillingStart)");
			}
		}

		// Token: 0x06007BF1 RID: 31729 RVA: 0x003299D4 File Offset: 0x00327BD4
		private void HandleEmptied()
		{
			this._meterActive = false;
			if (this._displayedValue <= this.deactivateThreshold)
			{
				this.RestoreBaseline();
				if (this.debugMode)
				{
					Debug.Log("[MasturbationVignette] Deactivated (local tail already empty)");
					return;
				}
			}
			else if (this.debugMode)
			{
				Debug.Log("[MasturbationVignette] Meter emptied - starting local fade-out");
			}
		}

		// Token: 0x06007BF2 RID: 31730 RVA: 0x00329A24 File Offset: 0x00327C24
		private void Update()
		{
			bool flag = this._resolvedProfile != null && (this.targetVolume == null || !this.targetVolume.isActiveAndEnabled || this.targetVolume.profile != this._resolvedProfile);
			if ((this._vignette == null || flag) && (flag || Time.unscaledTime >= this._nextVolumeResolveTime))
			{
				this._nextVolumeResolveTime = Time.unscaledTime + 0.5f;
				this.RestoreBaseline();
				this._vignette = null;
				this._resolvedProfile = null;
				this.ResolveVolume();
				this.ResolveVignette();
				this.ResumeMeter();
			}
			if (this.meter == null)
			{
				this._meterHooked = false;
				this.meter = OrgasmMeterController.Instance;
				if (this.meter != null)
				{
					this.HookMeter();
					this.ResumeMeter();
				}
			}
			if (this.heartbeat == null)
			{
				this.heartbeat = Object.FindObjectOfType<HeartbeatController>();
			}
			if (this._vignette == null)
			{
				return;
			}
			if (!this._override)
			{
				return;
			}
			float num = ((this.meter != null) ? Mathf.Clamp01(this.meter.Value) : ((this.heartbeat != null) ? this.heartbeat.GetSourceIntensity(this.heartbeatSourceId) : 0f));
			if (num >= this._displayedValue)
			{
				if (this.valueSmoothing >= 1f)
				{
					this._displayedValue = num;
				}
				else
				{
					float num2 = 1f - Mathf.Pow(1f - this.valueSmoothing, Time.unscaledDeltaTime * 60f);
					this._displayedValue = Mathf.Lerp(this._displayedValue, num, num2);
				}
			}
			else
			{
				this._displayedValue = Mathf.MoveTowards(this._displayedValue, num, Time.unscaledDeltaTime / Mathf.Max(0.1f, this.fadeOutDuration));
			}
			if (!this._meterActive && this._displayedValue <= this.deactivateThreshold)
			{
				if (this.debugMode)
				{
					Debug.Log(string.Format("[MasturbationVignette] Fade complete (intensity={0:F3}) — restoring baseline", this._displayedValue));
				}
				this.RestoreBaseline();
				return;
			}
			float displayedValue = this._displayedValue;
			float num3 = ((this.bpmCurve != null) ? this.bpmCurve.Evaluate(displayedValue) : displayedValue);
			float num4 = Mathf.Lerp(this.restingBpm, this.peakBpm, num3);
			float num5 = Mathf.Max(0.1f, num4 / 60f);
			this._phase += Time.unscaledDeltaTime * num5;
			float num6 = 0.5f + 0.5f * Mathf.Sin(this._phase * 3.1415927f * 2f);
			float num7 = Mathf.Clamp01(displayedValue * Mathf.Lerp(this.baseIntensityAtRest, this.baseIntensityAtPeak, displayedValue) + num6 * this.pulseAmplitude * displayedValue);
			Color color = Color.Lerp(this.restingColor, this.peakColor, displayedValue);
			GameplayVignetteMixer.Set(this._vignette, this, 20, num7, new Color?(color), new float?(this.smoothness));
		}

		// Token: 0x04007F86 RID: 32646
		[Header("References")]
		[Tooltip("Controller logico del meter. Determina QUANDO la vignette si attiva (OnFillingStart) e quando torna a baseline (OnEmptied). Se null, viene risolto da OrgasmMeterController.Instance allo Start.")]
		[SerializeField]
		private OrgasmMeterController meter;

		// Token: 0x04007F87 RID: 32647
		[Tooltip("HeartbeatController usato solo come fallback per leggere la sorgente orgasm quando il meter non e' disponibile. Non viene mai letta l'intensita globale adattiva.")]
		[SerializeField]
		private HeartbeatController heartbeat;

		// Token: 0x04007F88 RID: 32648
		[Tooltip("ID della sorgente scritta da OrgasmHeartbeatDriver.")]
		[SerializeField]
		private string heartbeatSourceId = "orgasm";

		// Token: 0x04007F89 RID: 32649
		[Tooltip("Volume URP da pilotare. Se null, viene cercato il primo Volume globale nella scena.")]
		[SerializeField]
		private Volume targetVolume;

		// Token: 0x04007F8A RID: 32650
		[Header("Vignette Color")]
		[Tooltip("Colore della vignette quando il meter e' a 0 (battito a riposo).")]
		[SerializeField]
		private Color restingColor = new Color(0.85f, 0.3f, 0.45f, 1f);

		// Token: 0x04007F8B RID: 32651
		[Tooltip("Colore della vignette quando il meter e' a 1 (peak).")]
		[SerializeField]
		private Color peakColor = new Color(0.9f, 0.1f, 0.25f, 1f);

		// Token: 0x04007F8C RID: 32652
		[Header("Intensity Mapping")]
		[Tooltip("Intensita base a meter=0 (vignette appena visibile).")]
		[Range(0f, 1f)]
		[SerializeField]
		private float baseIntensityAtRest = 0.15f;

		// Token: 0x04007F8D RID: 32653
		[Tooltip("Intensita base a meter=1 (vignette molto presente).")]
		[Range(0f, 1f)]
		[SerializeField]
		private float baseIntensityAtPeak = 0.55f;

		// Token: 0x04007F8E RID: 32654
		[Tooltip("Ampiezza del pulse aggiuntivo sopra l'intensita base. 0 = niente pulse, 0.2 = la vignette oscilla di +0.2 al picco.")]
		[Range(0f, 0.5f)]
		[SerializeField]
		private float pulseAmplitude = 0.12f;

		// Token: 0x04007F8F RID: 32655
		[Header("Smoothness")]
		[Tooltip("Smoothness della vignette (1 = bordi morbidi, 0 = bordi netti). Mantenuto costante durante il pulse.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float smoothness = 0.4f;

		// Token: 0x04007F90 RID: 32656
		[Header("BPM Sync")]
		[Tooltip("BPM a meter=0. Match con HeartbeatController per sincronizzare la pulsazione visiva.")]
		[Min(20f)]
		[SerializeField]
		private float restingBpm = 70f;

		// Token: 0x04007F91 RID: 32657
		[Tooltip("BPM a meter=1. Match con HeartbeatController.")]
		[Min(20f)]
		[SerializeField]
		private float peakBpm = 160f;

		// Token: 0x04007F92 RID: 32658
		[Tooltip("Curva di interpolazione del BPM in funzione di meter.Value (0..1).")]
		[SerializeField]
		private AnimationCurve bpmCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04007F93 RID: 32659
		[Header("Lerping")]
		[Tooltip("Smoothing della salita del valore usato dalla vignette (1 = istantaneo).")]
		[Range(0f, 1f)]
		[SerializeField]
		private float valueSmoothing = 0.2f;

		// Token: 0x04007F94 RID: 32660
		[Tooltip("Durata massima del tail dopo che il meter torna vuoto. Indipendente dal battito globale.")]
		[Min(0.1f)]
		[SerializeField]
		private float fadeOutDuration = 1.5f;

		// Token: 0x04007F95 RID: 32661
		[Tooltip("Soglia locale sotto cui la vignette torna a baseline e cede il controllo al profile.")]
		[Range(0f, 0.2f)]
		[SerializeField]
		private float deactivateThreshold = 0.02f;

		// Token: 0x04007F96 RID: 32662
		[Header("Debug")]
		[SerializeField]
		private bool debugMode;

		// Token: 0x04007F97 RID: 32663
		private Vignette _vignette;

		// Token: 0x04007F98 RID: 32664
		private VolumeProfile _resolvedProfile;

		// Token: 0x04007F99 RID: 32665
		private float _nextVolumeResolveTime;

		// Token: 0x04007F9A RID: 32666
		private bool _override;

		// Token: 0x04007F9B RID: 32667
		private bool _meterActive;

		// Token: 0x04007F9C RID: 32668
		private bool _meterHooked;

		// Token: 0x04007F9D RID: 32669
		private float _displayedValue;

		// Token: 0x04007F9E RID: 32670
		private float _phase;
	}
}
