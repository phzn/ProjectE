using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000036 RID: 54
	[CallbackIdentity(339)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GameConnectedChatJoin_t
	{
		// Token: 0x04000036 RID: 54
		public const int k_iCallback = 339;

		// Token: 0x04000037 RID: 55
		public CSteamID m_steamIDClanChat;

		// Token: 0x04000038 RID: 56
		public CSteamID m_steamIDUser;
	}
}
