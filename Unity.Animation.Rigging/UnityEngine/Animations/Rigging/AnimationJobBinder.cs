using System;
using UnityEngine.Playables;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x0200001B RID: 27
	public abstract class AnimationJobBinder<TJob, TData> : IAnimationJobBinder where TJob : struct, IAnimationJob where TData : struct, IAnimationJobData
	{
		// Token: 0x06000069 RID: 105
		public abstract TJob Create(Animator animator, ref TData data, Component component);

		// Token: 0x0600006A RID: 106
		public abstract void Destroy(TJob job);

		// Token: 0x0600006B RID: 107 RVA: 0x000037E1 File Offset: 0x000019E1
		public virtual void Update(TJob job, ref TData data)
		{
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000037E4 File Offset: 0x000019E4
		IAnimationJob IAnimationJobBinder.Create(Animator animator, IAnimationJobData data, Component component)
		{
			TData tdata = (TData)((object)data);
			return this.Create(animator, ref tdata, component);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003807 File Offset: 0x00001A07
		void IAnimationJobBinder.Destroy(IAnimationJob job)
		{
			this.Destroy((TJob)((object)job));
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003818 File Offset: 0x00001A18
		void IAnimationJobBinder.Update(IAnimationJob job, IAnimationJobData data)
		{
			TData tdata = (TData)((object)data);
			this.Update((TJob)((object)job), ref tdata);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x0000383A File Offset: 0x00001A3A
		AnimationScriptPlayable IAnimationJobBinder.CreatePlayable(PlayableGraph graph, IAnimationJob job)
		{
			return AnimationScriptPlayable.Create<TJob>(graph, (TJob)((object)job), 0);
		}
	}
}
