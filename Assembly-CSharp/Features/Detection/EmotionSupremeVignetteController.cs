using System;
using Features.MasturbationSystem;
using Features.PlayerEmotions;
using ProjectExhibitionist.Core.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Features.Detection
{
	// Token: 0x02001287 RID: 4743
	[DisallowMultipleComponent]
	public class EmotionSupremeVignetteController : MonoBehaviour
	{
		// Token: 0x06009332 RID: 37682 RVA: 0x003C71BD File Offset: 0x003C53BD
		private void Start()
		{
			if (this.heartbeat == null)
			{
				this.heartbeat = Object.FindObjectOfType<HeartbeatController>();
			}
			this.ResolveVolume();
			this.ResolveVignette();
		}

		// Token: 0x06009333 RID: 37683 RVA: 0x003C71E4 File Offset: 0x003C53E4
		private void OnDisable()
		{
			this.RestoreBaseline();
			this.ReleaseHeartbeat();
		}

		// Token: 0x06009334 RID: 37684 RVA: 0x003C71E4 File Offset: 0x003C53E4
		private void OnDestroy()
		{
			this.RestoreBaseline();
			this.ReleaseHeartbeat();
		}

		// Token: 0x06009335 RID: 37685 RVA: 0x003C71F2 File Offset: 0x003C53F2
		private void ReleaseHeartbeat()
		{
			if (!this._heartbeatHeld)
			{
				return;
			}
			if (this.heartbeat != null)
			{
				this.heartbeat.ClearSource(this.heartbeatSourceId);
			}
			this._heartbeatHeld = false;
		}

		// Token: 0x06009336 RID: 37686 RVA: 0x003C7223 File Offset: 0x003C5423
		private void ResolveVolume()
		{
			this.targetVolume = GameplayVignetteVolumeResolver.Resolve(this.targetVolume);
			if (this.targetVolume != null)
			{
				return;
			}
			if (this.debugMode)
			{
				Debug.LogWarning("[EmotionSupremeVignette] No global Volume found in scene; vignette will be inactive.");
			}
		}

		// Token: 0x06009337 RID: 37687 RVA: 0x003C7258 File Offset: 0x003C5458
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
					Debug.Log("[EmotionSupremeVignette] Found Vignette override in volume profile.");
				}
				return;
			}
			this._vignette = this._resolvedProfile.Add<Vignette>(true);
			if (this.debugMode)
			{
				Debug.Log("[EmotionSupremeVignette] Added runtime Vignette override.");
			}
		}

		// Token: 0x06009338 RID: 37688 RVA: 0x003C72EF File Offset: 0x003C54EF
		private void RestoreBaseline()
		{
			GameplayVignetteMixer.Release(this._vignette, this);
			this._override = false;
		}

		// Token: 0x06009339 RID: 37689 RVA: 0x003C7304 File Offset: 0x003C5504
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
			}
			if (this.heartbeat == null)
			{
				this.heartbeat = Object.FindObjectOfType<HeartbeatController>();
			}
			EmotionManager instance = EmotionManager.Instance;
			float num = ((instance != null) ? Mathf.Abs(instance.EmotionBalance) : 0f);
			bool flag2 = instance != null && num > this.supremeThreshold;
			float num2 = (flag2 ? Mathf.Clamp01((num - this.supremeThreshold) / Mathf.Max(0.01f, 100f - this.supremeThreshold)) : 0f);
			if (this.heartbeat != null)
			{
				AdaptiveHeartbeatDriver component = this.heartbeat.GetComponent<AdaptiveHeartbeatDriver>();
				if (component != null && component.isActiveAndEnabled)
				{
					this.ReleaseHeartbeat();
				}
				else if (flag2)
				{
					float num3 = Mathf.Lerp(this.heartbeatAtThreshold, this.heartbeatAtPeak, num2);
					this.heartbeat.SetSourceIntensity(this.heartbeatSourceId, num3);
					this._heartbeatHeld = true;
				}
				else if (this._heartbeatHeld)
				{
					this.ReleaseHeartbeat();
				}
			}
			if (this._vignette == null)
			{
				return;
			}
			if (!flag2)
			{
				if (this._override)
				{
					this.RestoreBaseline();
				}
				return;
			}
			if (!this._override)
			{
				this._override = true;
				if (this.debugMode)
				{
					Debug.Log(string.Format("[EmotionSupremeVignette] Activated (|balance|={0:F1})", num));
				}
			}
			float num4 = ((this.intensityCurve != null) ? this.intensityCurve.Evaluate(num2) : num2);
			float num5 = Mathf.Clamp01(this.peakIntensity * num4);
			GameplayVignetteMixer.Set(this._vignette, this, 10, num5, new Color?(this.supremeColor), new float?(this.smoothness));
		}

		// Token: 0x04009806 RID: 38918
		[Header("References")]
		[Tooltip("Volume URP da pilotare. Se null, viene cercato il primo Volume globale nella scena.")]
		[SerializeField]
		private Volume targetVolume;

		// Token: 0x04009807 RID: 38919
		[Tooltip("HeartbeatController da pilotare in stato Supreme: il battito sale vertiginosamente. Il contributo e' max-wins con le altre sorgenti (es. orgasm), e viene rilasciato all'uscita dallo stato cosi' il cuore si calma col fall smoothing. Se null, risolto allo Start.")]
		[SerializeField]
		private HeartbeatController heartbeat;

		// Token: 0x04009808 RID: 38920
		[Header("Supreme Threshold")]
		[Tooltip("Soglia su |EmotionBalance| (0-100) oltre cui scatta lo stato Supreme e la vignette si attiva. Match con la soglia usata da PlayerEmotionController per HornySupreme/EmbSupreme (70).")]
		[Range(0f, 100f)]
		[SerializeField]
		private float supremeThreshold = 70f;

		// Token: 0x04009809 RID: 38921
		[Header("Vignette Look")]
		[Tooltip("Colore unico della vignette Supreme (sia Emb che Horny).")]
		[SerializeField]
		private Color supremeColor = new Color(0.7f, 0.15f, 0.35f, 1f);

		// Token: 0x0400980A RID: 38922
		[Tooltip("Intensita' al picco (|balance| = 100). Piu' lieve del peak dell'orgasm.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float peakIntensity = 0.32f;

		// Token: 0x0400980B RID: 38923
		[Tooltip("Smoothness della vignette (1 = bordi morbidi, 0 = bordi netti).")]
		[Range(0f, 1f)]
		[SerializeField]
		private float smoothness = 0.5f;

		// Token: 0x0400980C RID: 38924
		[Tooltip("Curva di mapping da 'quanto sopra soglia' (0..1) a fattore intensita' (0..1). Default EaseInOut per un ingresso/uscita morbido.")]
		[SerializeField]
		private AnimationCurve intensityCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		// Token: 0x0400980D RID: 38925
		[Header("Heartbeat (Supreme)")]
		[Tooltip("Contributo di battito appena si ENTRA in stato Supreme (over=0). Alto di default cosi' il cuore parte gia' concitato (sale vertiginosamente). Max-wins con le altre sorgenti.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float heartbeatAtThreshold = 0.8f;

		// Token: 0x0400980E RID: 38926
		[Tooltip("Contributo di battito al picco Supreme (|balance| = 100). 1 = battito massimo.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float heartbeatAtPeak = 1f;

		// Token: 0x0400980F RID: 38927
		[Tooltip("Id della sorgente registrata nell'HeartbeatController. Univoco per non collidere con altre sorgenti.")]
		[SerializeField]
		private string heartbeatSourceId = "EmotionSupreme";

		// Token: 0x04009810 RID: 38928
		[Header("Debug")]
		[SerializeField]
		private bool debugMode;

		// Token: 0x04009811 RID: 38929
		private Vignette _vignette;

		// Token: 0x04009812 RID: 38930
		private VolumeProfile _resolvedProfile;

		// Token: 0x04009813 RID: 38931
		private float _nextVolumeResolveTime;

		// Token: 0x04009814 RID: 38932
		private bool _override;

		// Token: 0x04009815 RID: 38933
		private bool _heartbeatHeld;
	}
}
