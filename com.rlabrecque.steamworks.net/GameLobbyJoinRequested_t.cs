using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000030 RID: 48
	[CallbackIdentity(333)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GameLobbyJoinRequested_t
	{
		// Token: 0x04000020 RID: 32
		public const int k_iCallback = 333;

		// Token: 0x04000021 RID: 33
		public CSteamID m_steamIDLobby;

		// Token: 0x04000022 RID: 34
		public CSteamID m_steamIDFriend;
	}
}
