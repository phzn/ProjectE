using System;
using System.Collections.Generic;
using Features.Detection;
using UnityEngine;

namespace Features.CielVoice
{
	// Token: 0x02001357 RID: 4951
	[CreateAssetMenu(fileName = "CielDetectionVoiceSet", menuName = "Ciel Voice/Detection Voice Set")]
	public class CielDetectionVoiceSet : ScriptableObject
	{
		// Token: 0x0600993C RID: 39228 RVA: 0x003ED3E0 File Offset: 0x003EB5E0
		public CielReactionMood ResolveMood(float shameLevel, float exhibitionLevel)
		{
			if (shameLevel > exhibitionLevel && shameLevel > this.moodDominanceThreshold)
			{
				return CielReactionMood.Shame;
			}
			if (exhibitionLevel > shameLevel && exhibitionLevel > this.moodDominanceThreshold)
			{
				return CielReactionMood.Exhibition;
			}
			return CielReactionMood.Neutral;
		}

		// Token: 0x0600993D RID: 39229 RVA: 0x003ED404 File Offset: 0x003EB604
		public CielVoiceSO ResolveVoice(DetectionActorType actorType, CielReactionMood mood)
		{
			CielDetectionVoiceSet.ActorVoiceEntry actorVoiceEntry = this.FindEntry(actorType);
			CielVoiceSO cielVoiceSO = ((actorVoiceEntry != null) ? actorVoiceEntry.Resolve(mood) : null);
			if (cielVoiceSO != null)
			{
				return cielVoiceSO;
			}
			if (actorType != DetectionActorType.Generic)
			{
				CielDetectionVoiceSet.ActorVoiceEntry actorVoiceEntry2 = this.FindEntry(DetectionActorType.Generic);
				if (actorVoiceEntry2 != null)
				{
					cielVoiceSO = actorVoiceEntry2.Resolve(mood);
				}
			}
			return cielVoiceSO;
		}

		// Token: 0x0600993E RID: 39230 RVA: 0x003ED44C File Offset: 0x003EB64C
		private CielDetectionVoiceSet.ActorVoiceEntry FindEntry(DetectionActorType actorType)
		{
			for (int i = 0; i < this.entries.Count; i++)
			{
				if (this.entries[i] != null && this.entries[i].actorType == actorType)
				{
					return this.entries[i];
				}
			}
			return null;
		}

		// Token: 0x04009E50 RID: 40528
		[Header("Voci per attore")]
		[Tooltip("Una entry per ciascun tipo di attore. L'attore non presente ripiega sull'entry 'Generic' (se configurata).")]
		public List<CielDetectionVoiceSet.ActorVoiceEntry> entries = new List<CielDetectionVoiceSet.ActorVoiceEntry>();

		// Token: 0x04009E51 RID: 40529
		[Header("Soglie stato emotivo")]
		[Tooltip("Soglia minima del livello dominante (Shame o Exhibition) sotto la quale si considera lo stato Neutral. Rispecchia il valore 10 usato da PlayerReactionProfileSO.")]
		public float moodDominanceThreshold = 10f;

		// Token: 0x02001358 RID: 4952
		[Serializable]
		public class ActorVoiceEntry
		{
			// Token: 0x06009940 RID: 39232 RVA: 0x003ED4C0 File Offset: 0x003EB6C0
			public CielVoiceSO Resolve(CielReactionMood mood)
			{
				if (mood != CielReactionMood.Shame)
				{
					if (mood != CielReactionMood.Exhibition)
					{
						return this.neutralVoice;
					}
					if (!(this.exhibitionVoice != null))
					{
						return this.neutralVoice;
					}
					return this.exhibitionVoice;
				}
				else
				{
					if (!(this.shameVoice != null))
					{
						return this.neutralVoice;
					}
					return this.shameVoice;
				}
			}

			// Token: 0x17001861 RID: 6241
			// (get) Token: 0x06009941 RID: 39233 RVA: 0x003ED515 File Offset: 0x003EB715
			public bool HasAnyVoice
			{
				get
				{
					return this.shameVoice != null || this.exhibitionVoice != null || this.neutralVoice != null;
				}
			}

			// Token: 0x04009E52 RID: 40530
			[Tooltip("Tipo di attore a cui si applica questa entry.")]
			public DetectionActorType actorType;

			// Token: 0x04009E53 RID: 40531
			[Tooltip("Voce quando Ciel e' in stato Shame (vergogna dominante).")]
			public CielVoiceSO shameVoice;

			// Token: 0x04009E54 RID: 40532
			[Tooltip("Voce quando Ciel e' in stato Exhibition (esibizione dominante).")]
			public CielVoiceSO exhibitionVoice;

			// Token: 0x04009E55 RID: 40533
			[Tooltip("Voce quando nessuno stato domina (Neutral). Usata anche come fallback se la voce dello stato specifico e' mancante.")]
			public CielVoiceSO neutralVoice;
		}
	}
}
