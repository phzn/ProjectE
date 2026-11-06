using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000A0 RID: 160
	[CallbackIdentity(1320)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageGetPublishedItemVoteDetailsResult_t
	{
		// Token: 0x040001DD RID: 477
		public const int k_iCallback = 1320;

		// Token: 0x040001DE RID: 478
		public EResult m_eResult;

		// Token: 0x040001DF RID: 479
		public PublishedFileId_t m_unPublishedFileId;

		// Token: 0x040001E0 RID: 480
		public int m_nVotesFor;

		// Token: 0x040001E1 RID: 481
		public int m_nVotesAgainst;

		// Token: 0x040001E2 RID: 482
		public int m_nReports;

		// Token: 0x040001E3 RID: 483
		public float m_fScore;
	}
}
