using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000079 RID: 121
	[CallbackIdentity(506)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyChatUpdate_t
	{
		// Token: 0x04000145 RID: 325
		public const int k_iCallback = 506;

		// Token: 0x04000146 RID: 326
		public ulong m_ulSteamIDLobby;

		// Token: 0x04000147 RID: 327
		public ulong m_ulSteamIDUserChanged;

		// Token: 0x04000148 RID: 328
		public ulong m_ulSteamIDMakingChange;

		// Token: 0x04000149 RID: 329
		public uint m_rgfChatMemberStateChange;
	}
}
