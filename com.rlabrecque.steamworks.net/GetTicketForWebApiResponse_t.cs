using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000D3 RID: 211
	[CallbackIdentity(168)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GetTicketForWebApiResponse_t
	{
		// Token: 0x0400029E RID: 670
		public const int k_iCallback = 168;

		// Token: 0x0400029F RID: 671
		public HAuthTicket m_hAuthTicket;

		// Token: 0x040002A0 RID: 672
		public EResult m_eResult;

		// Token: 0x040002A1 RID: 673
		public int m_cubTicket;

		// Token: 0x040002A2 RID: 674
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2560)]
		public byte[] m_rgubTicket;
	}
}
