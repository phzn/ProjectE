using System;

namespace Features.RunModes
{
	// Token: 0x02000D59 RID: 3417
	public interface IRankedLeaderboardBackend
	{
		// Token: 0x1700105B RID: 4187
		// (get) Token: 0x0600645A RID: 25690
		string PlatformName { get; }

		// Token: 0x1700105C RID: 4188
		// (get) Token: 0x0600645B RID: 25691
		bool IsAvailable { get; }

		// Token: 0x0600645C RID: 25692
		bool TrySubmitRun(RankedRunResult result);
	}
}
