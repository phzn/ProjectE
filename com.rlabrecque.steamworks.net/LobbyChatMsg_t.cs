using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200007A RID: 122
	[CallbackIdentity(507)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyChatMsg_t
	{
		// Token: 0x0400014A RID: 330
		public const int k_iCallback = 507;

		// Token: 0x0400014B RID: 331
		public ulong m_ulSteamIDLobby;

		// Token: 0x0400014C RID: 332
		public ulong m_ulSteamIDUser;

		// Token: 0x0400014D RID: 333
		public byte m_eChatEntryType;

		// Token: 0x0400014E RID: 334
		public uint m_iChatID;
	}
}
