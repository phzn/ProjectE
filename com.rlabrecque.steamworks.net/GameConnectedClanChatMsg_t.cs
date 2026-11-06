using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000035 RID: 53
	[CallbackIdentity(338)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct GameConnectedClanChatMsg_t
	{
		// Token: 0x04000032 RID: 50
		public const int k_iCallback = 338;

		// Token: 0x04000033 RID: 51
		public CSteamID m_steamIDClanChat;

		// Token: 0x04000034 RID: 52
		public CSteamID m_steamIDUser;

		// Token: 0x04000035 RID: 53
		public int m_iMessageID;
	}
}
