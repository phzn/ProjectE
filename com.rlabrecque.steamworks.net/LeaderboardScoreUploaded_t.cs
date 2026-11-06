using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000D9 RID: 217
	[CallbackIdentity(1106)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LeaderboardScoreUploaded_t
	{
		// Token: 0x040002B7 RID: 695
		public const int k_iCallback = 1106;

		// Token: 0x040002B8 RID: 696
		public byte m_bSuccess;

		// Token: 0x040002B9 RID: 697
		public SteamLeaderboard_t m_hSteamLeaderboard;

		// Token: 0x040002BA RID: 698
		public int m_nScore;

		// Token: 0x040002BB RID: 699
		public byte m_bScoreChanged;

		// Token: 0x040002BC RID: 700
		public int m_nGlobalRankNew;

		// Token: 0x040002BD RID: 701
		public int m_nGlobalRankPrevious;
	}
}
