using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000DF RID: 223
	[CallbackIdentity(1112)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GlobalStatsReceived_t
	{
		// Token: 0x040002CE RID: 718
		public const int k_iCallback = 1112;

		// Token: 0x040002CF RID: 719
		public ulong m_nGameID;

		// Token: 0x040002D0 RID: 720
		public EResult m_eResult;
	}
}
