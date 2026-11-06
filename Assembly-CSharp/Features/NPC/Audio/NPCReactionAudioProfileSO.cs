using System;
using Bozo.AnimeCharacters;
using UnityEngine;

namespace Features.NPC.Audio
{
	// Token: 0x02000F50 RID: 3920
	[CreateAssetMenu(fileName = "New NPC Reaction Audio Profile", menuName = "NPCs/NPC Reaction Audio Profile")]
	public class NPCReactionAudioProfileSO : ScriptableObject
	{
		// Token: 0x06007573 RID: 30067 RVA: 0x002F92B9 File Offset: 0x002F74B9
		public WeightedAudioClip GetStopAndStareAudio(Gender gender)
		{
			return this.stopAndStareAudio.GetRandomClip(gender);
		}

		// Token: 0x06007574 RID: 30068 RVA: 0x002F92C7 File Offset: 0x002F74C7
		public WeightedAudioClip GetStumbleAudio(Gender gender)
		{
			return this.stumbleAudio.GetRandomClip(gender);
		}

		// Token: 0x06007575 RID: 30069 RVA: 0x002F92D5 File Offset: 0x002F74D5
		public WeightedAudioClip GetSearchingAudio(Gender gender)
		{
			return this.searchingAudio.GetRandomClip(gender);
		}

		// Token: 0x06007576 RID: 30070 RVA: 0x002F92E3 File Offset: 0x002F74E3
		public WeightedAudioClip GetConfusionAudio(Gender gender)
		{
			return this.confusionAudio.GetRandomClip(gender);
		}

		// Token: 0x04007849 RID: 30793
		[Header("Stop & Stare Reactions")]
		[Tooltip("Audio clips played when NPC enters Stop&Stare state (surprised gasp, 'huh?', etc.)")]
		public GenderedAudioSet stopAndStareAudio = new GenderedAudioSet();

		// Token: 0x0400784A RID: 30794
		[Header("Stumble Reactions")]
		[Tooltip("Audio clips played when NPC stumbles (grunt, 'woah!', etc.)")]
		public GenderedAudioSet stumbleAudio = new GenderedAudioSet();

		// Token: 0x0400784B RID: 30795
		[Header("Searching Reactions")]
		[Tooltip("Audio clips played when NPC enters Searching state ('where did they go?', etc.)")]
		public GenderedAudioSet searchingAudio = new GenderedAudioSet();

		// Token: 0x0400784C RID: 30796
		[Header("Confusion Reactions")]
		[Tooltip("Audio clips played when NPC enters Confusion state ('hm?', 'strange...', etc.)")]
		public GenderedAudioSet confusionAudio = new GenderedAudioSet();

		// Token: 0x0400784D RID: 30797
		[Header("Audio Settings")]
		[Tooltip("Base volume for all reaction audio")]
		[Range(0f, 1f)]
		public float baseVolume = 1f;

		// Token: 0x0400784E RID: 30798
		[Tooltip("Random pitch variation (+/- this value)")]
		[Range(0f, 0.3f)]
		public float pitchVariation = 0.1f;
	}
}
