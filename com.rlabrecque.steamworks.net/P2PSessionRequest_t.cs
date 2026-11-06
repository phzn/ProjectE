using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000088 RID: 136
	[CallbackIdentity(1202)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct P2PSessionRequest_t
	{
		// Token: 0x04000171 RID: 369
		public const int k_iCallback = 1202;

		// Token: 0x04000172 RID: 370
		public CSteamID m_steamIDRemote;
	}
}
