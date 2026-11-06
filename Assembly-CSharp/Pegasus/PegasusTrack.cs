using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Pegasus
{
	// Token: 0x020001BD RID: 445
	[TrackColor(0.09f, 0.45f, 0.8f)]
	[TrackClipType(typeof(PegasusClip))]
	public class PegasusTrack : TrackAsset
	{
		// Token: 0x06000D40 RID: 3392 RVA: 0x0005BAD8 File Offset: 0x00059CD8
		public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
		{
			foreach (TimelineClip timelineClip in base.GetClips())
			{
				PegasusManager pegasusManager = ((PegasusClip)timelineClip.asset).PegasusManager.Resolve(graph.GetResolver());
				timelineClip.displayName = ((pegasusManager == null) ? "Pegasus" : pegasusManager.name);
			}
			ScriptPlayable<PegasusMixerBehaviour> scriptPlayable = ScriptPlayable<PegasusMixerBehaviour>.Create(graph, 0);
			scriptPlayable.SetInputCount(inputCount);
			return scriptPlayable;
		}
	}
}
