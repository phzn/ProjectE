using System;

namespace Features.PlayerEmotions
{
	// Token: 0x02000E13 RID: 3603
	public interface IContinuousEmotionSource : IEmotionSource
	{
		// Token: 0x06006B36 RID: 27446
		EmotionImpactData GetContinuousImpact();
	}
}
