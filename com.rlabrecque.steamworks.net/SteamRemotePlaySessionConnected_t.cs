using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000091 RID: 145
	[CallbackIdentity(5701)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamRemotePlaySessionConnected_t
	{
		// Token: 0x0400018D RID: 397
		public const int k_iCallback = 5701;

		// Token: 0x0400018E RID: 398
		public RemotePlaySessionID_t m_unSessionID;
	}
}
