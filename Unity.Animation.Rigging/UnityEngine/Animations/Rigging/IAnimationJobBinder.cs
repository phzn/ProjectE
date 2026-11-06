using System;
using UnityEngine.Playables;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x0200001A RID: 26
	public interface IAnimationJobBinder
	{
		// Token: 0x06000065 RID: 101
		IAnimationJob Create(Animator animator, IAnimationJobData data, Component component = null);

		// Token: 0x06000066 RID: 102
		void Destroy(IAnimationJob job);

		// Token: 0x06000067 RID: 103
		void Update(IAnimationJob job, IAnimationJobData data);

		// Token: 0x06000068 RID: 104
		AnimationScriptPlayable CreatePlayable(PlayableGraph graph, IAnimationJob job);
	}
}
