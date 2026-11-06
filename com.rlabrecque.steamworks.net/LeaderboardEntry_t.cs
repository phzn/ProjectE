using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200016B RID: 363
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LeaderboardEntry_t
	{
		// Token: 0x04000AD3 RID: 2771
		public CSteamID m_steamIDUser;

		// Token: 0x04000AD4 RID: 2772
		public int m_nGlobalRank;

		// Token: 0x04000AD5 RID: 2773
		public int m_nScore;

		// Token: 0x04000AD6 RID: 2774
		public int m_cDetails;

		// Token: 0x04000AD7 RID: 2775
		public UGCHandle_t m_hUGC;
	}
}
