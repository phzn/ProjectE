using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000B7 RID: 183
	[CallbackIdentity(3406)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct DownloadItemResult_t
	{
		// Token: 0x04000237 RID: 567
		public const int k_iCallback = 3406;

		// Token: 0x04000238 RID: 568
		public AppId_t m_unAppID;

		// Token: 0x04000239 RID: 569
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x0400023A RID: 570
		public EResult m_eResult;
	}
}
