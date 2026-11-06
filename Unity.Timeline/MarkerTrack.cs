using System;
using System.Collections.Generic;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x02000029 RID: 41
	[TrackBindingType(typeof(GameObject))]
	[HideInMenu]
	[ExcludeFromPreset]
	[Serializable]
	public class MarkerTrack : TrackAsset
	{
		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000241 RID: 577 RVA: 0x000085E0 File Offset: 0x000067E0
		public override IEnumerable<PlayableBinding> outputs
		{
			get
			{
				TimelineAsset timelineAsset = base.timelineAsset;
				if (!(this == ((timelineAsset != null) ? timelineAsset.markerTrack : null)))
				{
					return base.outputs;
				}
				return new List<PlayableBinding> { ScriptPlayableBinding.Create(base.name, null, typeof(GameObject)) };
			}
		}
	}
}
