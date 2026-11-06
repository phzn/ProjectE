using System;
using Features.Police;
using UnityEngine;

namespace Features.CielVoice
{
	// Token: 0x0200135B RID: 4955
	[DisallowMultipleComponent]
	public class CielPerpWalkVoiceListener : MonoBehaviour
	{
		// Token: 0x0600994B RID: 39243 RVA: 0x003ED824 File Offset: 0x003EBA24
		private void Awake()
		{
			this.EnsureSources();
			if (this.nervousWhineClip != null)
			{
				this._loop = new CrossfadeLoopPlayer(this.sourceA, this.sourceB, this.nervousWhineClip, this.loopCrossfade, 0f, 0f);
			}
		}

		// Token: 0x0600994C RID: 39244 RVA: 0x003ED872 File Offset: 0x003EBA72
		private void EnsureSources()
		{
			this.sourceA = this.EnsureOne(this.sourceA, "CielNervousWhineSourceA");
			this.sourceB = this.EnsureOne(this.sourceB, "CielNervousWhineSourceB");
		}

		// Token: 0x0600994D RID: 39245 RVA: 0x003ED8A4 File Offset: 0x003EBAA4
		private AudioSource EnsureOne(AudioSource src, string childName)
		{
			if (src == null)
			{
				GameObject gameObject = new GameObject(childName);
				gameObject.transform.SetParent(base.transform, false);
				src = gameObject.AddComponent<AudioSource>();
				src.spatialBlend = 0f;
				if (this.debugLog)
				{
					Debug.LogWarning("[CielPerpWalkVoiceListener] " + childName + " non cablato: creato fallback runtime.");
				}
			}
			src.loop = false;
			src.playOnAwake = false;
			if (this.nervousWhineClip != null)
			{
				src.clip = this.nervousWhineClip;
			}
			src.volume = 0f;
			return src;
		}

		// Token: 0x0600994E RID: 39246 RVA: 0x003ED938 File Offset: 0x003EBB38
		private void OnEnable()
		{
			PerpWalkController.OnPerpWalkStarted += this.HandlePerpWalkStarted;
			PerpWalkController.OnPerpWalkEnded += this.HandlePerpWalkEnded;
			this._shouldPlay = PerpWalkController.Instance != null && PerpWalkController.Instance.IsActive;
		}

		// Token: 0x0600994F RID: 39247 RVA: 0x003ED988 File Offset: 0x003EBB88
		private void OnDisable()
		{
			PerpWalkController.OnPerpWalkStarted -= this.HandlePerpWalkStarted;
			PerpWalkController.OnPerpWalkEnded -= this.HandlePerpWalkEnded;
			this._shouldPlay = false;
			CrossfadeLoopPlayer loop = this._loop;
			if (loop != null)
			{
				loop.Stop();
			}
			this._currentVol = 0f;
		}

		// Token: 0x06009950 RID: 39248 RVA: 0x003ED9DC File Offset: 0x003EBBDC
		private void HandlePerpWalkStarted(bool isRunMode)
		{
			if (this._loop == null)
			{
				if (this.debugLog)
				{
					Debug.Log("[CielPerpWalkVoiceListener] nervousWhineClip non assegnata — loop non avviato.");
				}
				return;
			}
			this._shouldPlay = true;
			if (!this._loop.IsPlaying)
			{
				this._loop.Play();
			}
			if (this.debugLog)
			{
				Debug.Log("[CielPerpWalkVoiceListener] Nervous whine LOOP start (" + (isRunMode ? "PerpRun (censura)" : "PerpWalk (Polizia)") + ").");
			}
		}

		// Token: 0x06009951 RID: 39249 RVA: 0x003EDA4E File Offset: 0x003EBC4E
		private void HandlePerpWalkEnded()
		{
			this._shouldPlay = false;
			if (this.debugLog)
			{
				Debug.Log("[CielPerpWalkVoiceListener] Nervous whine LOOP stop (perp walk ended).");
			}
		}

		// Token: 0x06009952 RID: 39250 RVA: 0x003EDA6C File Offset: 0x003EBC6C
		private void Update()
		{
			if (this._loop == null)
			{
				return;
			}
			if (this._shouldPlay && !this._loop.IsPlaying)
			{
				this._loop.Resume();
			}
			float num = (this._shouldPlay ? this.volume : 0f);
			float num2 = ((num > this._currentVol) ? this.fadeInDuration : this.fadeOutDuration);
			this._currentVol = ((num2 <= 0f) ? num : Mathf.MoveTowards(this._currentVol, num, Mathf.Max(0.0001f, this.volume) / num2 * Time.unscaledDeltaTime));
			this._loop.Tick(this._currentVol);
			if (!this._shouldPlay && this._currentVol <= 0.001f && this._loop.IsPlaying)
			{
				this._loop.Stop();
				if (this.debugLog)
				{
					Debug.Log("[CielPerpWalkVoiceListener] loop fermato (fade-out completo).");
				}
			}
		}

		// Token: 0x04009E5F RID: 40543
		[Header("Loop clip")]
		[Tooltip("Clip del verso nervoso in loop (Nervous whine). Vale per entrambi gli ending.")]
		[SerializeField]
		private AudioClip nervousWhineClip;

		// Token: 0x04009E60 RID: 40544
		[Tooltip("Primo AudioSource dedicato al loop (crossfade). GO sotto Ciel_Invector/Audio.")]
		[SerializeField]
		private AudioSource sourceA;

		// Token: 0x04009E61 RID: 40545
		[Tooltip("Secondo AudioSource dedicato al loop (crossfade). GO sotto Ciel_Invector/Audio.")]
		[SerializeField]
		private AudioSource sourceB;

		// Token: 0x04009E62 RID: 40546
		[Header("Volume / Fade")]
		[Tooltip("Volume del loop quando pienamente udibile.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float volume = 0.7f;

		// Token: 0x04009E63 RID: 40547
		[Tooltip("Durata (s) del crossfade al giro del loop. Piu' lungo = giuntura piu' morbida.")]
		[Min(0.02f)]
		[SerializeField]
		private float loopCrossfade = 0.3f;

		// Token: 0x04009E64 RID: 40548
		[Tooltip("Durata (s) del fade-in quando il loop parte.")]
		[Min(0f)]
		[SerializeField]
		private float fadeInDuration = 0.4f;

		// Token: 0x04009E65 RID: 40549
		[Tooltip("Durata (s) del fade-out quando il loop si ferma.")]
		[Min(0f)]
		[SerializeField]
		private float fadeOutDuration = 0.5f;

		// Token: 0x04009E66 RID: 40550
		[Header("Debug")]
		[SerializeField]
		private bool debugLog;

		// Token: 0x04009E67 RID: 40551
		private CrossfadeLoopPlayer _loop;

		// Token: 0x04009E68 RID: 40552
		private bool _shouldPlay;

		// Token: 0x04009E69 RID: 40553
		private float _currentVol;
	}
}
