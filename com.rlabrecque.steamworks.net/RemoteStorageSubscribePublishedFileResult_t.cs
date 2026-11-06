using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000099 RID: 153
	[CallbackIdentity(1313)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageSubscribePublishedFileResult_t
	{
		// Token: 0x040001A8 RID: 424
		public const int k_iCallback = 1313;

		// Token: 0x040001A9 RID: 425
		public EResult m_eResult;

		// Token: 0x040001AA RID: 426
		public PublishedFileId_t m_nPublishedFileId;
	}
}
