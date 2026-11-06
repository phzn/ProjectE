using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000A7 RID: 167
	[CallbackIdentity(1327)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageSetUserPublishedFileActionResult_t
	{
		// Token: 0x040001F9 RID: 505
		public const int k_iCallback = 1327;

		// Token: 0x040001FA RID: 506
		public EResult m_eResult;

		// Token: 0x040001FB RID: 507
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x040001FC RID: 508
		public EWorkshopFileAction m_eAction;
	}
}
