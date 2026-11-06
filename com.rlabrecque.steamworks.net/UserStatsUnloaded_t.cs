using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000DB RID: 219
	[CallbackIdentity(1108)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct UserStatsUnloaded_t
	{
		// Token: 0x040002C1 RID: 705
		public const int k_iCallback = 1108;

		// Token: 0x040002C2 RID: 706
		public CSteamID m_steamIDUser;
	}
}
