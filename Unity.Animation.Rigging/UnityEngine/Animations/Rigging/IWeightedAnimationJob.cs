using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x0200001D RID: 29
	public interface IWeightedAnimationJob : IAnimationJob
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000073 RID: 115
		// (set) Token: 0x06000074 RID: 116
		FloatProperty jobWeight { get; set; }
	}
}
