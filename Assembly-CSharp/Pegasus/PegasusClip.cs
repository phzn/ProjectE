using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Pegasus
{
	// Token: 0x020001A3 RID: 419
	[Serializable]
	public class PegasusClip : PlayableAsset, ITimelineClipAsset
	{
		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000CDA RID: 3290 RVA: 0x00057FFB File Offset: 0x000561FB
		public ClipCaps clipCaps
		{
			get
			{
				return ClipCaps.Blending;
			}
		}

		// Token: 0x06000CDB RID: 3291 RVA: 0x00058000 File Offset: 0x00056200
		public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
		{
			ScriptPlayable<PegasusBehaviour> scriptPlayable = ScriptPlayable<PegasusBehaviour>.Create(graph, this.template, 0);
			scriptPlayable.GetBehaviour().pegasusManager = this.PegasusManager.Resolve(graph.GetResolver());
			scriptPlayable.GetBehaviour().pegasusProgress = this.PegasusProgress;
			return scriptPlayable;
		}

		// Token: 0x04000CC9 RID: 3273
		public ExposedReference<PegasusManager> PegasusManager;

		// Token: 0x04000CCA RID: 3274
		[Range(0f, 1f)]
		public float PegasusProgress;

		// Token: 0x04000CCB RID: 3275
		[HideInInspector]
		public PegasusBehaviour template = new PegasusBehaviour();
	}
}
