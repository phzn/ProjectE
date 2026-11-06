using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000050 RID: 80
	[CallbackIdentity(1108)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GSStatsUnloaded_t
	{
		// Token: 0x04000094 RID: 148
		public const int k_iCallback = 1108;

		// Token: 0x04000095 RID: 149
		public CSteamID m_steamIDUser;
	}
}
