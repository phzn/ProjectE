using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200003B RID: 59
	[CallbackIdentity(344)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct FriendsGetFollowerCount_t
	{
		// Token: 0x04000046 RID: 70
		public const int k_iCallback = 344;

		// Token: 0x04000047 RID: 71
		public EResult m_eResult;

		// Token: 0x04000048 RID: 72
		public CSteamID m_steamID;

		// Token: 0x04000049 RID: 73
		public int m_nCount;
	}
}
