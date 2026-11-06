using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200009B RID: 155
	[CallbackIdentity(1315)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageUnsubscribePublishedFileResult_t
	{
		// Token: 0x040001B1 RID: 433
		public const int k_iCallback = 1315;

		// Token: 0x040001B2 RID: 434
		public EResult m_eResult;

		// Token: 0x040001B3 RID: 435
		public PublishedFileId_t m_nPublishedFileId;
	}
}
