using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000A5 RID: 165
	[CallbackIdentity(1325)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageUserVoteDetails_t
	{
		// Token: 0x040001F0 RID: 496
		public const int k_iCallback = 1325;

		// Token: 0x040001F1 RID: 497
		public EResult m_eResult;

		// Token: 0x040001F2 RID: 498
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x040001F3 RID: 499
		public EWorkshopVote m_eVote;
	}
}
