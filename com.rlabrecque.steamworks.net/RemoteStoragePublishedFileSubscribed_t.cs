using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000A1 RID: 161
	[CallbackIdentity(1321)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStoragePublishedFileSubscribed_t
	{
		// Token: 0x040001E4 RID: 484
		public const int k_iCallback = 1321;

		// Token: 0x040001E5 RID: 485
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x040001E6 RID: 486
		public AppId_t m_nAppID;
	}
}
