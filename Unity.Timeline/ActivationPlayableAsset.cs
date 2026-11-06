using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x02000004 RID: 4
	internal class ActivationPlayableAsset : PlayableAsset, ITimelineClipAsset
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000009 RID: 9 RVA: 0x000021D7 File Offset: 0x000003D7
		public ClipCaps clipCaps
		{
			get
			{
				return ClipCaps.None;
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000021DA File Offset: 0x000003DA
		public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
		{
			return Playable.Create(graph, 0);
		}
	}
}
