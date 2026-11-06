using System;

namespace Features.RunModes
{
	// Token: 0x02000D56 RID: 3414
	public interface IRankedLeaderboardRecordReset
	{
		// Token: 0x17001057 RID: 4183
		// (get) Token: 0x06006454 RID: 25684
		bool CanResetOwnRecord { get; }

		// Token: 0x17001058 RID: 4184
		// (get) Token: 0x06006455 RID: 25685
		bool IsResettingOwnRecord { get; }

		// Token: 0x17001059 RID: 4185
		// (get) Token: 0x06006456 RID: 25686
		bool HasPendingRecordReset { get; }

		// Token: 0x06006457 RID: 25687
		void ResetOwnRecord(Action<RankedRecordResetResult> completed);
	}
}
