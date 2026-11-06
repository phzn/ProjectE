using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Features.CielVoice
{
	// Token: 0x02001363 RID: 4963
	[CreateAssetMenu(fileName = "Voice_", menuName = "Ciel Voice/Voice Line")]
	public class CielVoiceSO : ScriptableObject
	{
		// Token: 0x17001871 RID: 6257
		// (get) Token: 0x06009999 RID: 39321 RVA: 0x003EF0B1 File Offset: 0x003ED2B1
		public string ResolvedId
		{
			get
			{
				if (!string.IsNullOrEmpty(this.VoiceId))
				{
					return this.VoiceId;
				}
				return base.name;
			}
		}

		// Token: 0x17001872 RID: 6258
		// (get) Token: 0x0600999A RID: 39322 RVA: 0x003EF0D0 File Offset: 0x003ED2D0
		public bool HasClips
		{
			get
			{
				if (this.Clips == null)
				{
					return false;
				}
				for (int i = 0; i < this.Clips.Length; i++)
				{
					if (this.Clips[i] != null)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x0600999B RID: 39323 RVA: 0x003EF110 File Offset: 0x003ED310
		public AudioClip PickClip()
		{
			if (this.Clips == null || this.Clips.Length == 0)
			{
				return null;
			}
			int num = 0;
			for (int i = 0; i < this.Clips.Length; i++)
			{
				if (this.Clips[i] != null)
				{
					num++;
				}
			}
			if (num == 0)
			{
				return null;
			}
			if (num == 1)
			{
				for (int j = 0; j < this.Clips.Length; j++)
				{
					if (this.Clips[j] != null)
					{
						return this.Clips[j];
					}
				}
			}
			switch (this.PickMode)
			{
			case CielVoiceSO.ClipPickMode.Sequential:
				return this.NextValidFrom(ref this._seqCursor);
			case CielVoiceSO.ClipPickMode.First:
			{
				for (int k = 0; k < this.Clips.Length; k++)
				{
					if (this.Clips[k] != null)
					{
						return this.Clips[k];
					}
				}
				return null;
			}
			}
			int num2 = Random.Range(0, num);
			int num3 = 0;
			for (int l = 0; l < this.Clips.Length; l++)
			{
				if (!(this.Clips[l] == null))
				{
					if (num3 == num2)
					{
						return this.Clips[l];
					}
					num3++;
				}
			}
			return null;
		}

		// Token: 0x0600999C RID: 39324 RVA: 0x003EF238 File Offset: 0x003ED438
		private AudioClip NextValidFrom(ref int cursor)
		{
			int num = this.Clips.Length;
			for (int i = 0; i < num; i++)
			{
				int num2 = cursor % num;
				cursor = (cursor + 1) % num;
				if (this.Clips[num2] != null)
				{
					return this.Clips[num2];
				}
			}
			return null;
		}

		// Token: 0x0600999D RID: 39325 RVA: 0x003EF281 File Offset: 0x003ED481
		public float ResolvePitch()
		{
			if (this.PitchJitter <= 0f)
			{
				return this.Pitch;
			}
			return this.Pitch + Random.Range(-this.PitchJitter, this.PitchJitter);
		}

		// Token: 0x04009EAF RID: 40623
		[Header("Identity")]
		[Tooltip("Id opzionale della battuta (per lookup/debug). Se vuoto si usa il nome dell'asset.")]
		public string VoiceId;

		// Token: 0x04009EB0 RID: 40624
		[Header("Audio")]
		[Tooltip("Clip candidate. Con piu' di una clip, la scelta segue PickMode. Le entry null sono ignorate.")]
		public AudioClip[] Clips;

		// Token: 0x04009EB1 RID: 40625
		[Tooltip("Come scegliere la clip quando ce n'e' piu' di una.")]
		public CielVoiceSO.ClipPickMode PickMode;

		// Token: 0x04009EB2 RID: 40626
		[Tooltip("Mixer group opzionale su cui instradare la voce. Se null usa quello gia' sull'AudioSource.")]
		public AudioMixerGroup MixerGroup;

		// Token: 0x04009EB3 RID: 40627
		[Tooltip("Volume base della battuta (moltiplicato per il volume globale del controller).")]
		[Range(0f, 1f)]
		public float Volume = 1f;

		// Token: 0x04009EB4 RID: 40628
		[Tooltip("Pitch base della battuta.")]
		[Range(0.5f, 2f)]
		public float Pitch = 1f;

		// Token: 0x04009EB5 RID: 40629
		[Tooltip("Jitter casuale (+/-) applicato al pitch ad ogni Play, per varieta'. 0 = pitch fisso.")]
		[Range(0f, 0.3f)]
		public float PitchJitter;

		// Token: 0x04009EB6 RID: 40630
		[Header("Subtitle")]
		[Tooltip("Testo sottotitolo mostrato durante la battuta. Vuoto = nessun sottotitolo.")]
		[TextArea(1, 3)]
		public string Subtitle;

		// Token: 0x04009EB7 RID: 40631
		[Tooltip("Durata extra (s) per cui tenere il sottotitolo visibile OLTRE la fine della clip. Se <= 0 il sottotitolo dura quanto la clip.")]
		[Min(0f)]
		public float SubtitleExtraHold = 0.4f;

		// Token: 0x04009EB8 RID: 40632
		[Tooltip("Se true, il sottotitolo NON si auto-chiude a fine clip: resta visibile finche' il player non lo skippa (click/tasto avanzamento), poi fa fade-out. Usato per momenti narrativi 'click per continuare' (es. reazione di cattura). Se false (default) il sottotitolo dura quanto la clip + SubtitleExtraHold e sparisce da solo.")]
		public bool SkippableSubtitle;

		// Token: 0x04009EB9 RID: 40633
		[NonSerialized]
		private int _seqCursor;

		// Token: 0x02001364 RID: 4964
		public enum ClipPickMode
		{
			// Token: 0x04009EBB RID: 40635
			[Tooltip("Sceglie una clip a caso ad ogni Play (evita ripetizioni immediate se >1 clip).")]
			Random,
			// Token: 0x04009EBC RID: 40636
			[Tooltip("Scorre le clip in ordine, ciclando. Utile per sequenze recitate.")]
			Sequential,
			// Token: 0x04009EBD RID: 40637
			[Tooltip("Suona sempre la prima clip valida.")]
			First
		}
	}
}
