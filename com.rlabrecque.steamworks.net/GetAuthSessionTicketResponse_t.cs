using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000CE RID: 206
	[CallbackIdentity(163)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GetAuthSessionTicketResponse_t
	{
		// Token: 0x04000288 RID: 648
		public const int k_iCallback = 163;

		// Token: 0x04000289 RID: 649
		public HAuthTicket m_hAuthTicket;

		// Token: 0x0400028A RID: 650
		public EResult m_eResult;
	}
}
