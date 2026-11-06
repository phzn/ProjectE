using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x02000014 RID: 20
	[NotKeyable]
	[Serializable]
	internal class AudioClipProperties : PlayableBehaviour
	{
		// Token: 0x04000086 RID: 134
		[Range(0f, 1f)]
		public float volume = 1f;
	}
}
