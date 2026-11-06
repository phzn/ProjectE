using System;
using System.Collections;
using Features.Detection;
using Features.Police;
using UnityEngine;

namespace Features.CielVoice
{
	// Token: 0x0200135D RID: 4957
	[DisallowMultipleComponent]
	public class CielSpottedVoiceListener : MonoBehaviour
	{
		// Token: 0x06009961 RID: 39265 RVA: 0x003EE191 File Offset: 0x003EC391
		private void OnEnable()
		{
			this._hookupRoutine = base.StartCoroutine(this.HookupWhenReady());
		}

		// Token: 0x06009962 RID: 39266 RVA: 0x003EE1A5 File Offset: 0x003EC3A5
		private void OnDisable()
		{
			if (this._hookupRoutine != null)
			{
				base.StopCoroutine(this._hookupRoutine);
				this._hookupRoutine = null;
			}
			this.Unsubscribe();
		}

		// Token: 0x06009963 RID: 39267 RVA: 0x003EE1C8 File Offset: 0x003EC3C8
		private IEnumerator HookupWhenReady()
		{
			float elapsed = 0f;
			while (PlayerEmotionController.Instance == null)
			{
				if (this.hookupTimeout > 0f && elapsed >= this.hookupTimeout)
				{
					if (this.debugLog)
					{
						Debug.LogWarning("[CielSpottedVoiceListener] PlayerEmotionController non trovato entro il timeout — nessun aggancio.");
					}
					yield break;
				}
				elapsed += Time.unscaledDeltaTime;
				yield return null;
			}
			this.Subscribe(PlayerEmotionController.Instance);
			this._hookupRoutine = null;
			yield break;
		}

		// Token: 0x06009964 RID: 39268 RVA: 0x003EE1D8 File Offset: 0x003EC3D8
		private void Subscribe(PlayerEmotionController emotion)
		{
			if (emotion == null || this._emotion == emotion)
			{
				return;
			}
			this.Unsubscribe();
			this._emotion = emotion;
			this._emotion.OnHighestDetectionAttentionChanged += this.HandleAttentionChanged;
			if (this.debugLog)
			{
				Debug.Log("[CielSpottedVoiceListener] Agganciato a PlayerEmotionController.OnHighestDetectionAttentionChanged.");
			}
		}

		// Token: 0x06009965 RID: 39269 RVA: 0x003EE233 File Offset: 0x003EC433
		private void Unsubscribe()
		{
			if (this._emotion != null)
			{
				this._emotion.OnHighestDetectionAttentionChanged -= this.HandleAttentionChanged;
				this._emotion = null;
			}
		}

		// Token: 0x06009966 RID: 39270 RVA: 0x003EE264 File Offset: 0x003EC464
		private void HandleAttentionChanged(float previous, float current)
		{
			if (PerpWalkController.Instance != null && PerpWalkController.Instance.IsActive)
			{
				return;
			}
			if (current <= this.rearmAttention)
			{
				this._armed = true;
				return;
			}
			if (!this._armed)
			{
				return;
			}
			bool flag = current > previous;
			bool flag2 = current >= this.triggerAttention && current < this.maxAttention;
			if (!flag || !flag2)
			{
				return;
			}
			if (Time.time - this._lastPlayTime < this.cooldownSeconds)
			{
				this._armed = false;
				return;
			}
			CielVoiceSO cielVoiceSO = this.ResolveVoiceForState();
			if (cielVoiceSO == null)
			{
				if (this.debugLog)
				{
					Debug.Log("[CielSpottedVoiceListener] Nessuna voce configurata per lo stato corrente.");
				}
				this._armed = false;
				return;
			}
			CielVoiceController instance = CielVoiceController.Instance;
			if (instance == null)
			{
				if (this.debugLog)
				{
					Debug.LogWarning("[CielSpottedVoiceListener] CielVoiceController.Instance null — verso non riprodotto.");
				}
				return;
			}
			instance.Play(cielVoiceSO);
			this._lastPlayTime = Time.time;
			this._armed = false;
			if (this.debugLog)
			{
				Debug.Log(string.Format("[CielSpottedVoiceListener] Verso avvistamento: attention={0:F2}, voice='{1}'.", current, cielVoiceSO.ResolvedId));
			}
		}

		// Token: 0x06009967 RID: 39271 RVA: 0x003EE36A File Offset: 0x003EC56A
		private CielVoiceSO ResolveVoiceForState()
		{
			if (!PlayerExposureState.IsFlashActive && (!(this._emotion != null) || !this._emotion.IsExhibitionDominant))
			{
				return this.embarrassedVoice;
			}
			return this.confidentVoice;
		}

		// Token: 0x04009E80 RID: 40576
		[Header("Voci per stato")]
		[Tooltip("Verso quando Ciel e' in stato Embarrassed (Shame dominante). Es. EncounterSouns1.")]
		[SerializeField]
		private CielVoiceSO embarrassedVoice;

		// Token: 0x04009E81 RID: 40577
		[Tooltip("Verso quando Ciel e' in stato Confident (Exhibition dominante). Es. LewdGiggle.")]
		[SerializeField]
		private CielVoiceSO confidentVoice;

		// Token: 0x04009E82 RID: 40578
		[Header("Soglie attention (0..1)")]
		[Tooltip("La barra deve superare questa soglia (salendo) perche' il verso parta: 'poco dopo che la barra inizia a salire'. Default 0.15.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float triggerAttention = 0.15f;

		// Token: 0x04009E83 RID: 40579
		[Tooltip("Il verso parte SOLO se l'attention e' ancora sotto questo valore ('prima della meta'). Default 0.5 = meta' barra.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float maxAttention = 0.5f;

		// Token: 0x04009E84 RID: 40580
		[Tooltip("Sotto questa soglia il trigger si 'riarma' (la barra e' scesa abbastanza da considerare un nuovo avvistamento). Default 0.05.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float rearmAttention = 0.05f;

		// Token: 0x04009E85 RID: 40581
		[Header("Cooldown")]
		[Tooltip("Secondi minimi tra un verso e il successivo. Default 15.")]
		[Min(0f)]
		[SerializeField]
		private float cooldownSeconds = 15f;

		// Token: 0x04009E86 RID: 40582
		[Header("Hookup")]
		[Tooltip("Secondi di retry per agganciare il PlayerEmotionController.Instance se non e' ancora pronto.")]
		[SerializeField]
		private float hookupTimeout = 10f;

		// Token: 0x04009E87 RID: 40583
		[Header("Debug")]
		[SerializeField]
		private bool debugLog;

		// Token: 0x04009E88 RID: 40584
		private PlayerEmotionController _emotion;

		// Token: 0x04009E89 RID: 40585
		private Coroutine _hookupRoutine;

		// Token: 0x04009E8A RID: 40586
		private float _lastPlayTime = float.NegativeInfinity;

		// Token: 0x04009E8B RID: 40587
		private bool _armed = true;
	}
}
