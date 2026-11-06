using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200007E RID: 126
	[CallbackIdentity(513)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyCreated_t
	{
		// Token: 0x0400015A RID: 346
		public const int k_iCallback = 513;

		// Token: 0x0400015B RID: 347
		public EResult m_eResult;

		// Token: 0x0400015C RID: 348
		public ulong m_ulSteamIDLobby;
	}
}
