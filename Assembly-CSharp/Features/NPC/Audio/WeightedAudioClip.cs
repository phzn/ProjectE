using System;
using UnityEngine;

namespace Features.NPC.Audio
{
	// Token: 0x02000F4E RID: 3918
	[Serializable]
	public class WeightedAudioClip
	{
		// Token: 0x04007843 RID: 30787
		[Tooltip("The audio clip to play")]
		public AudioClip clip;

		// Token: 0x04007844 RID: 30788
		[Tooltip("Weight for random selection (higher = more likely)")]
		[Range(1f, 100f)]
		public int weight = 10;

		// Token: 0x04007845 RID: 30789
		[Tooltip("Volume multiplier for this clip")]
		[Range(0f, 1f)]
		public float volume = 1f;
	}
}
