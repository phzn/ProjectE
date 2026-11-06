using System;

namespace Features.PlayerEmotions
{
	// Token: 0x02000E12 RID: 3602
	public interface IEmotionSource
	{
		// Token: 0x170011A1 RID: 4513
		// (get) Token: 0x06006B32 RID: 27442
		string SourceId { get; }

		// Token: 0x170011A2 RID: 4514
		// (get) Token: 0x06006B33 RID: 27443
		EmotionSourceCategory Category { get; }

		// Token: 0x170011A3 RID: 4515
		// (get) Token: 0x06006B34 RID: 27444
		EmotionPriority Priority { get; }

		// Token: 0x170011A4 RID: 4516
		// (get) Token: 0x06006B35 RID: 27445
		bool IsActive { get; }
	}
}
