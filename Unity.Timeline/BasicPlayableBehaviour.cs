using System;
using System.Collections.Generic;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x02000033 RID: 51
	[Obsolete("For best performance use PlayableAsset and PlayableBehaviour.")]
	[Serializable]
	public class BasicPlayableBehaviour : ScriptableObject, IPlayableAsset, IPlayableBehaviour
	{
		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600026D RID: 621 RVA: 0x00008C2F File Offset: 0x00006E2F
		public virtual double duration
		{
			get
			{
				return PlayableBinding.DefaultDuration;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600026E RID: 622 RVA: 0x00008C36 File Offset: 0x00006E36
		public virtual IEnumerable<PlayableBinding> outputs
		{
			get
			{
				return PlayableBinding.None;
			}
		}

		// Token: 0x0600026F RID: 623 RVA: 0x00008C3D File Offset: 0x00006E3D
		public virtual void OnGraphStart(Playable playable)
		{
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00008C3F File Offset: 0x00006E3F
		public virtual void OnGraphStop(Playable playable)
		{
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00008C41 File Offset: 0x00006E41
		public virtual void OnPlayableCreate(Playable playable)
		{
		}

		// Token: 0x06000272 RID: 626 RVA: 0x00008C43 File Offset: 0x00006E43
		public virtual void OnPlayableDestroy(Playable playable)
		{
		}

		// Token: 0x06000273 RID: 627 RVA: 0x00008C45 File Offset: 0x00006E45
		public virtual void OnBehaviourPlay(Playable playable, FrameData info)
		{
		}

		// Token: 0x06000274 RID: 628 RVA: 0x00008C47 File Offset: 0x00006E47
		public virtual void OnBehaviourPause(Playable playable, FrameData info)
		{
		}

		// Token: 0x06000275 RID: 629 RVA: 0x00008C49 File Offset: 0x00006E49
		public virtual void PrepareFrame(Playable playable, FrameData info)
		{
		}

		// Token: 0x06000276 RID: 630 RVA: 0x00008C4B File Offset: 0x00006E4B
		public virtual void ProcessFrame(Playable playable, FrameData info, object playerData)
		{
		}

		// Token: 0x06000277 RID: 631 RVA: 0x00008C4D File Offset: 0x00006E4D
		public virtual Playable CreatePlayable(PlayableGraph graph, GameObject owner)
		{
			return ScriptPlayable<BasicPlayableBehaviour>.Create(graph, this, 0);
		}
	}
}
