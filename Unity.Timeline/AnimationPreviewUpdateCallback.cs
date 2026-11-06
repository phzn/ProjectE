using System;
using System.Collections.Generic;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x02000008 RID: 8
	internal class AnimationPreviewUpdateCallback : ITimelineEvaluateCallback
	{
		// Token: 0x0600003C RID: 60 RVA: 0x000027E4 File Offset: 0x000009E4
		public AnimationPreviewUpdateCallback(AnimationPlayableOutput output)
		{
			this.m_Output = output;
			Playable sourcePlayable = this.m_Output.GetSourcePlayable<AnimationPlayableOutput>();
			if (sourcePlayable.IsValid<Playable>())
			{
				this.m_Graph = sourcePlayable.GetGraph<Playable>();
			}
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002820 File Offset: 0x00000A20
		public void Evaluate()
		{
			if (!this.m_Graph.IsValid())
			{
				return;
			}
			if (this.m_PreviewComponents == null)
			{
				this.FetchPreviewComponents();
			}
			foreach (IAnimationWindowPreview animationWindowPreview in this.m_PreviewComponents)
			{
				if (animationWindowPreview != null)
				{
					animationWindowPreview.UpdatePreviewGraph(this.m_Graph);
				}
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002898 File Offset: 0x00000A98
		private void FetchPreviewComponents()
		{
			this.m_PreviewComponents = new List<IAnimationWindowPreview>();
			Animator target = this.m_Output.GetTarget();
			if (target == null)
			{
				return;
			}
			GameObject gameObject = target.gameObject;
			this.m_PreviewComponents.AddRange(gameObject.GetComponents<IAnimationWindowPreview>());
		}

		// Token: 0x04000015 RID: 21
		private AnimationPlayableOutput m_Output;

		// Token: 0x04000016 RID: 22
		private PlayableGraph m_Graph;

		// Token: 0x04000017 RID: 23
		private List<IAnimationWindowPreview> m_PreviewComponents;
	}
}
