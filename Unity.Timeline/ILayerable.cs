using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x02000031 RID: 49
	public interface ILayerable
	{
		// Token: 0x06000265 RID: 613
		Playable CreateLayerMixer(PlayableGraph graph, GameObject go, int inputCount);
	}
}
