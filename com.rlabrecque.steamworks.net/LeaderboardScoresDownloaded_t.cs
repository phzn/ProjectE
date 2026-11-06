using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000D8 RID: 216
	[CallbackIdentity(1105)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LeaderboardScoresDownloaded_t
	{
		// Token: 0x040002B3 RID: 691
		public const int k_iCallback = 1105;

		// Token: 0x040002B4 RID: 692
		public SteamLeaderboard_t m_hSteamLeaderboard;

		// Token: 0x040002B5 RID: 693
		public SteamLeaderboardEntries_t m_hSteamLeaderboardEntries;

		// Token: 0x040002B6 RID: 694
		public int m_cEntryCount;
	}
}
