using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200008D RID: 141
	[CallbackIdentity(1221)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamNetConnectionStatusChangedCallback_t
	{
		// Token: 0x0400017F RID: 383
		public const int k_iCallback = 1221;

		// Token: 0x04000180 RID: 384
		public HSteamNetConnection m_hConn;

		// Token: 0x04000181 RID: 385
		public SteamNetConnectionInfo_t m_info;

		// Token: 0x04000182 RID: 386
		public ESteamNetworkingConnectionState m_eOldState;
	}
}
