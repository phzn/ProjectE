using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000082 RID: 130
	[CallbackIdentity(5303)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct ReservationNotificationCallback_t
	{
		// Token: 0x04000167 RID: 359
		public const int k_iCallback = 5303;

		// Token: 0x04000168 RID: 360
		public PartyBeaconID_t m_ulBeaconID;

		// Token: 0x04000169 RID: 361
		public CSteamID m_steamIDJoiner;
	}
}
