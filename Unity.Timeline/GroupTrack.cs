using System;
using System.Collections.Generic;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x02000030 RID: 48
	[TrackClipType(typeof(TrackAsset))]
	[SupportsChildTracks(null, 2147483647)]
	[ExcludeFromPreset]
	[Serializable]
	public class GroupTrack : TrackAsset
	{
		// Token: 0x06000262 RID: 610 RVA: 0x00008AD8 File Offset: 0x00006CD8
		internal override bool CanCompileClips()
		{
			return false;
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000263 RID: 611 RVA: 0x00008ADB File Offset: 0x00006CDB
		public override IEnumerable<PlayableBinding> outputs
		{
			get
			{
				return PlayableBinding.None;
			}
		}
	}
}
