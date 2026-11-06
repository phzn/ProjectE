using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000D5 RID: 213
	[CallbackIdentity(1102)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct UserStatsStored_t
	{
		// Token: 0x040002A7 RID: 679
		public const int k_iCallback = 1102;

		// Token: 0x040002A8 RID: 680
		public ulong m_nGameID;

		// Token: 0x040002A9 RID: 681
		public EResult m_eResult;
	}
}
