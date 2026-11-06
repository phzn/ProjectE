using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000C6 RID: 198
	[CallbackIdentity(102)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamServerConnectFailure_t
	{
		// Token: 0x04000270 RID: 624
		public const int k_iCallback = 102;

		// Token: 0x04000271 RID: 625
		public EResult m_eResult;

		// Token: 0x04000272 RID: 626
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bStillRetrying;
	}
}
