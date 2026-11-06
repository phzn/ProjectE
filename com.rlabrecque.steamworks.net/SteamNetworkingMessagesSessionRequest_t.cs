using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200008B RID: 139
	[CallbackIdentity(1251)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamNetworkingMessagesSessionRequest_t
	{
		// Token: 0x0400017B RID: 379
		public const int k_iCallback = 1251;

		// Token: 0x0400017C RID: 380
		public SteamNetworkingIdentity m_identityRemote;
	}
}
