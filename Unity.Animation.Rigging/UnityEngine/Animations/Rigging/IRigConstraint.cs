using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000058 RID: 88
	public interface IRigConstraint
	{
		// Token: 0x06000230 RID: 560
		bool IsValid();

		// Token: 0x06000231 RID: 561
		IAnimationJob CreateJob(Animator animator);

		// Token: 0x06000232 RID: 562
		void UpdateJob(IAnimationJob job);

		// Token: 0x06000233 RID: 563
		void DestroyJob(IAnimationJob job);

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000234 RID: 564
		IAnimationJobData data { get; }

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000235 RID: 565
		IAnimationJobBinder binder { get; }

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000236 RID: 566
		Component component { get; }

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000237 RID: 567
		// (set) Token: 0x06000238 RID: 568
		float weight { get; set; }
	}
}
