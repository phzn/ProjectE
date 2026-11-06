using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000CB RID: 203
	[CallbackIdentity(143)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ValidateAuthTicketResponse_t
	{
		// Token: 0x0400027E RID: 638
		public const int k_iCallback = 143;

		// Token: 0x0400027F RID: 639
		public CSteamID m_SteamID;

		// Token: 0x04000280 RID: 640
		public EAuthSessionResponse m_eAuthSessionResponse;

		// Token: 0x04000281 RID: 641
		public CSteamID m_OwnerSteamID;
	}
}
