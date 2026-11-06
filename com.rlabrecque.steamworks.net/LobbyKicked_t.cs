using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200007D RID: 125
	[CallbackIdentity(512)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyKicked_t
	{
		// Token: 0x04000156 RID: 342
		public const int k_iCallback = 512;

		// Token: 0x04000157 RID: 343
		public ulong m_ulSteamIDLobby;

		// Token: 0x04000158 RID: 344
		public ulong m_ulSteamIDAdmin;

		// Token: 0x04000159 RID: 345
		public byte m_bKickedDueToDisconnect;
	}
}
