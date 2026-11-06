using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000A4 RID: 164
	[CallbackIdentity(1324)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageUpdateUserPublishedItemVoteResult_t
	{
		// Token: 0x040001ED RID: 493
		public const int k_iCallback = 1324;

		// Token: 0x040001EE RID: 494
		public EResult m_eResult;

		// Token: 0x040001EF RID: 495
		public PublishedFileId_t m_nPublishedFileId;
	}
}
