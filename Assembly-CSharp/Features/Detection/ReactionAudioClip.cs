using System;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x020012D8 RID: 4824
	[Serializable]
	public class ReactionAudioClip
	{
		// Token: 0x04009AA3 RID: 39587
		[Tooltip("Audio clip to play")]
		public AudioClip clip;

		// Token: 0x04009AA4 RID: 39588
		[Range(0f, 1f)]
		[Tooltip("Volume for this audio clip")]
		public float volume = 1f;
	}
}
