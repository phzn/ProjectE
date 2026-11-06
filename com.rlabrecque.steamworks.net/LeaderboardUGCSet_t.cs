using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000DE RID: 222
	[CallbackIdentity(1111)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LeaderboardUGCSet_t
	{
		// Token: 0x040002CB RID: 715
		public const int k_iCallback = 1111;

		// Token: 0x040002CC RID: 716
		public EResult m_eResult;

		// Token: 0x040002CD RID: 717
		public SteamLeaderboard_t m_hSteamLeaderboard;
	}
}
