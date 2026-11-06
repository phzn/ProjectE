using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000A3 RID: 163
	[CallbackIdentity(1323)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStoragePublishedFileDeleted_t
	{
		// Token: 0x040001EA RID: 490
		public const int k_iCallback = 1323;

		// Token: 0x040001EB RID: 491
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x040001EC RID: 492
		public AppId_t m_nAppID;
	}
}
