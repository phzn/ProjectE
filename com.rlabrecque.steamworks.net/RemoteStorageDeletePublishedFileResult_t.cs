using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000097 RID: 151
	[CallbackIdentity(1311)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageDeletePublishedFileResult_t
	{
		// Token: 0x040001A0 RID: 416
		public const int k_iCallback = 1311;

		// Token: 0x040001A1 RID: 417
		public EResult m_eResult;

		// Token: 0x040001A2 RID: 418
		public PublishedFileId_t m_nPublishedFileId;
	}
}
