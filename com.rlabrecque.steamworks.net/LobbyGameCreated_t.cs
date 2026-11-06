using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200007B RID: 123
	[CallbackIdentity(509)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyGameCreated_t
	{
		// Token: 0x0400014F RID: 335
		public const int k_iCallback = 509;

		// Token: 0x04000150 RID: 336
		public ulong m_ulSteamIDLobby;

		// Token: 0x04000151 RID: 337
		public ulong m_ulSteamIDGameServer;

		// Token: 0x04000152 RID: 338
		public uint m_unIP;

		// Token: 0x04000153 RID: 339
		public ushort m_usPort;
	}
}
