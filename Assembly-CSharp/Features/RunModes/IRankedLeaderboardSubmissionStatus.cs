using System;

namespace Features.RunModes
{
	// Token: 0x02000D63 RID: 3427
	public interface IRankedLeaderboardSubmissionStatus
	{
		// Token: 0x17001075 RID: 4213
		// (get) Token: 0x060064A2 RID: 25762
		bool CanSubmitScores { get; }

		// Token: 0x17001076 RID: 4214
		// (get) Token: 0x060064A3 RID: 25763
		string SubmissionDisabledReason { get; }
	}
}
