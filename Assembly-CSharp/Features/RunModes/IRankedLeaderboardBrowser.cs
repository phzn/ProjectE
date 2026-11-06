using System;

namespace Features.RunModes
{
	// Token: 0x02000D57 RID: 3415
	public interface IRankedLeaderboardBrowser
	{
		// Token: 0x06006458 RID: 25688
		IDisposable RequestEntries(RankedLeaderboardScope scope, int page, int pageSize, Action<RankedLeaderboardResult> completed);
	}
}
