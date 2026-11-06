using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000D7 RID: 215
	[CallbackIdentity(1104)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LeaderboardFindResult_t
	{
		// Token: 0x040002B0 RID: 688
		public const int k_iCallback = 1104;

		// Token: 0x040002B1 RID: 689
		public SteamLeaderboard_t m_hSteamLeaderboard;

		// Token: 0x040002B2 RID: 690
		public byte m_bLeaderboardFound;
	}
}
