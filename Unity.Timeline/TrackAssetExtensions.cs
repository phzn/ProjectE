using System;

namespace UnityEngine.Timeline
{
	// Token: 0x0200002F RID: 47
	public static class TrackAssetExtensions
	{
		// Token: 0x06000260 RID: 608 RVA: 0x000089FA File Offset: 0x00006BFA
		public static GroupTrack GetGroup(this TrackAsset asset)
		{
			if (asset == null)
			{
				return null;
			}
			return asset.parent as GroupTrack;
		}

		// Token: 0x06000261 RID: 609 RVA: 0x00008A14 File Offset: 0x00006C14
		public static void SetGroup(this TrackAsset asset, GroupTrack group)
		{
			if (asset == null || asset == group || asset.parent == group)
			{
				return;
			}
			if (group != null && asset.timelineAsset != group.timelineAsset)
			{
				throw new InvalidOperationException("Cannot assign to a group in a different timeline");
			}
			TimelineAsset timelineAsset = asset.timelineAsset;
			TrackAsset trackAsset = asset.parent as TrackAsset;
			TimelineAsset timelineAsset2 = asset.parent as TimelineAsset;
			if (trackAsset != null || timelineAsset2 != null)
			{
				if (timelineAsset2 != null)
				{
					timelineAsset2.RemoveTrack(asset);
				}
				else
				{
					trackAsset.RemoveSubTrack(asset);
				}
			}
			if (group == null)
			{
				asset.parent = asset.timelineAsset;
				timelineAsset.AddTrackInternal(asset);
				return;
			}
			group.AddChild(asset);
		}
	}
}
