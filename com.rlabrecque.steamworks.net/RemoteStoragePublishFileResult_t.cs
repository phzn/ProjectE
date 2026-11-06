using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000096 RID: 150
	[CallbackIdentity(1309)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStoragePublishFileResult_t
	{
		// Token: 0x0400019C RID: 412
		public const int k_iCallback = 1309;

		// Token: 0x0400019D RID: 413
		public EResult m_eResult;

		// Token: 0x0400019E RID: 414
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x0400019F RID: 415
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bUserNeedsToAcceptWorkshopLegalAgreement;
	}
}
