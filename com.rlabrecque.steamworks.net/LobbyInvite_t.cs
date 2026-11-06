using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000076 RID: 118
	[CallbackIdentity(503)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyInvite_t
	{
		// Token: 0x04000138 RID: 312
		public const int k_iCallback = 503;

		// Token: 0x04000139 RID: 313
		public ulong m_ulSteamIDUser;

		// Token: 0x0400013A RID: 314
		public ulong m_ulSteamIDLobby;

		// Token: 0x0400013B RID: 315
		public ulong m_ulGameID;
	}
}
