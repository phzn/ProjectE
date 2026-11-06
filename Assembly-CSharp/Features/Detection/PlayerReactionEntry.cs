using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x020012D7 RID: 4823
	[Serializable]
	public class PlayerReactionEntry
	{
		// Token: 0x0600957A RID: 38266 RVA: 0x003D5C6D File Offset: 0x003D3E6D
		public DialogueLine ToDialogueLine(string defaultSpeakerName = "")
		{
			return new DialogueLine
			{
				speakerName = ((!string.IsNullOrEmpty(this.speakerNameOverride)) ? this.speakerNameOverride : defaultSpeakerName),
				text = this.text,
				localizationScope = "dialogue"
			};
		}

		// Token: 0x170017DD RID: 6109
		// (get) Token: 0x0600957B RID: 38267 RVA: 0x003D5CA7 File Offset: 0x003D3EA7
		public bool HasDialogue
		{
			get
			{
				return !string.IsNullOrEmpty(this.text);
			}
		}

		// Token: 0x170017DE RID: 6110
		// (get) Token: 0x0600957C RID: 38268 RVA: 0x003D5CB7 File Offset: 0x003D3EB7
		public bool HasAudio
		{
			get
			{
				return this.audioClips != null && this.audioClips.Count > 0;
			}
		}

		// Token: 0x04009A9D RID: 39581
		[Header("Dialogue")]
		[Tooltip("Speaker name (leave empty to use profile's playerDisplayName)")]
		public string speakerNameOverride;

		// Token: 0x04009A9E RID: 39582
		[TextArea(2, 5)]
		[Tooltip("The reaction text/thought")]
		public string text;

		// Token: 0x04009A9F RID: 39583
		[Header("Audio")]
		[Tooltip("List of audio clips to play during this reaction (played with delay between each)")]
		public List<ReactionAudioClip> audioClips = new List<ReactionAudioClip>();

		// Token: 0x04009AA0 RID: 39584
		[Tooltip("Delay between each audio clip playing (in seconds)")]
		[Range(0f, 1f)]
		public float audioDelay = 0.1f;

		// Token: 0x04009AA1 RID: 39585
		[Tooltip("If true, randomize the order of audio playback")]
		public bool randomizeAudioOrder;

		// Token: 0x04009AA2 RID: 39586
		[Header("Timing")]
		[Tooltip("Delay before showing dialogue (allows audio to start first)")]
		public float dialogueDelay;
	}
}
