using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200008C RID: 140
	[CallbackIdentity(1252)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamNetworkingMessagesSessionFailed_t
	{
		// Token: 0x0400017D RID: 381
		public const int k_iCallback = 1252;

		// Token: 0x0400017E RID: 382
		public SteamNetConnectionInfo_t m_info;
	}
}
