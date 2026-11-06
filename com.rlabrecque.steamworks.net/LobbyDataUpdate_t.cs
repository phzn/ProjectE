using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000078 RID: 120
	[CallbackIdentity(505)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyDataUpdate_t
	{
		// Token: 0x04000141 RID: 321
		public const int k_iCallback = 505;

		// Token: 0x04000142 RID: 322
		public ulong m_ulSteamIDLobby;

		// Token: 0x04000143 RID: 323
		public ulong m_ulSteamIDMember;

		// Token: 0x04000144 RID: 324
		public byte m_bSuccess;
	}
}
