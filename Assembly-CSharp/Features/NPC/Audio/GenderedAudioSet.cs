using System;
using System.Collections.Generic;
using Bozo.AnimeCharacters;
using UnityEngine;

namespace Features.NPC.Audio
{
	// Token: 0x02000F4F RID: 3919
	[Serializable]
	public class GenderedAudioSet
	{
		// Token: 0x06007570 RID: 30064 RVA: 0x002F9140 File Offset: 0x002F7340
		public WeightedAudioClip GetRandomClip(Gender gender)
		{
			List<WeightedAudioClip> list = null;
			if (gender == Gender.Male && this.maleClips.Count > 0)
			{
				list = this.maleClips;
			}
			else if (gender == Gender.Female && this.femaleClips.Count > 0)
			{
				list = this.femaleClips;
			}
			if (list == null || list.Count == 0)
			{
				list = this.neutralClips;
			}
			if (list == null || list.Count == 0)
			{
				return null;
			}
			return this.GetWeightedRandom(list);
		}

		// Token: 0x06007571 RID: 30065 RVA: 0x002F91A8 File Offset: 0x002F73A8
		private WeightedAudioClip GetWeightedRandom(List<WeightedAudioClip> clips)
		{
			int num = 0;
			foreach (WeightedAudioClip weightedAudioClip in clips)
			{
				if (weightedAudioClip.clip != null)
				{
					num += weightedAudioClip.weight;
				}
			}
			if (num != 0)
			{
				int num2 = Random.Range(0, num);
				int num3 = 0;
				foreach (WeightedAudioClip weightedAudioClip2 in clips)
				{
					if (!(weightedAudioClip2.clip == null))
					{
						num3 += weightedAudioClip2.weight;
						if (num2 < num3)
						{
							return weightedAudioClip2;
						}
					}
				}
				return clips[0];
			}
			if (clips.Count <= 0)
			{
				return null;
			}
			return clips[0];
		}

		// Token: 0x04007846 RID: 30790
		[Tooltip("Audio clips for male NPCs")]
		public List<WeightedAudioClip> maleClips = new List<WeightedAudioClip>();

		// Token: 0x04007847 RID: 30791
		[Tooltip("Audio clips for female NPCs")]
		public List<WeightedAudioClip> femaleClips = new List<WeightedAudioClip>();

		// Token: 0x04007848 RID: 30792
		[Tooltip("Audio clips for any gender (fallback)")]
		public List<WeightedAudioClip> neutralClips = new List<WeightedAudioClip>();
	}
}
