using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200008A RID: 138
	[CallbackIdentity(1201)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct SocketStatusCallback_t
	{
		// Token: 0x04000176 RID: 374
		public const int k_iCallback = 1201;

		// Token: 0x04000177 RID: 375
		public SNetSocket_t m_hSocket;

		// Token: 0x04000178 RID: 376
		public SNetListenSocket_t m_hListenSocket;

		// Token: 0x04000179 RID: 377
		public CSteamID m_steamIDRemote;

		// Token: 0x0400017A RID: 378
		public int m_eSNetSocketState;
	}
}
