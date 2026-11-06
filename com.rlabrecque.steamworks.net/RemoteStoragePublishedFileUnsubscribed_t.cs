using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000A2 RID: 162
	[CallbackIdentity(1322)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStoragePublishedFileUnsubscribed_t
	{
		// Token: 0x040001E7 RID: 487
		public const int k_iCallback = 1322;

		// Token: 0x040001E8 RID: 488
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x040001E9 RID: 489
		public AppId_t m_nAppID;
	}
}
