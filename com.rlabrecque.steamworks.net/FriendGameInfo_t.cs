using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200015F RID: 351
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct FriendGameInfo_t
	{
		// Token: 0x04000A89 RID: 2697
		public CGameID m_gameID;

		// Token: 0x04000A8A RID: 2698
		public uint m_unGameIP;

		// Token: 0x04000A8B RID: 2699
		public ushort m_usGamePort;

		// Token: 0x04000A8C RID: 2700
		public ushort m_usQueryPort;

		// Token: 0x04000A8D RID: 2701
		public CSteamID m_steamIDLobby;
	}
}
