using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200009C RID: 156
	[CallbackIdentity(1316)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageUpdatePublishedFileResult_t
	{
		// Token: 0x040001B4 RID: 436
		public const int k_iCallback = 1316;

		// Token: 0x040001B5 RID: 437
		public EResult m_eResult;

		// Token: 0x040001B6 RID: 438
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x040001B7 RID: 439
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bUserNeedsToAcceptWorkshopLegalAgreement;
	}
}
