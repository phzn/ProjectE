using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000092 RID: 146
	[CallbackIdentity(5702)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamRemotePlaySessionDisconnected_t
	{
		// Token: 0x0400018F RID: 399
		public const int k_iCallback = 5702;

		// Token: 0x04000190 RID: 400
		public RemotePlaySessionID_t m_unSessionID;
	}
}
