using System;

namespace UnityEngine.Timeline
{
	// Token: 0x0200003B RID: 59
	[Serializable]
	public class PlayableTrack : TrackAsset
	{
		// Token: 0x060002AE RID: 686 RVA: 0x00009A02 File Offset: 0x00007C02
		protected override void OnCreateClip(TimelineClip clip)
		{
			if (clip.asset != null)
			{
				clip.displayName = clip.asset.GetType().Name;
			}
		}
	}
}
