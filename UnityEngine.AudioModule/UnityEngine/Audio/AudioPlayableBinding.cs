using System;
using UnityEngine.Playables;

namespace UnityEngine.Audio
{
	// Token: 0x02000032 RID: 50
	public static class AudioPlayableBinding
	{
		// Token: 0x06000213 RID: 531 RVA: 0x00003BB4 File Offset: 0x00001DB4
		public static PlayableBinding Create(string name, Object key)
		{
			return PlayableBinding.CreateInternal(name, key, typeof(AudioSource), new PlayableBinding.CreateOutputMethod(AudioPlayableBinding.CreateAudioOutput));
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00003BE4 File Offset: 0x00001DE4
		private static PlayableOutput CreateAudioOutput(PlayableGraph graph, string name)
		{
			return AudioPlayableOutput.Create(graph, name, null);
		}
	}
}
