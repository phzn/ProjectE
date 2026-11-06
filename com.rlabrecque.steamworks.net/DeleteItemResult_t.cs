using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000C2 RID: 194
	[CallbackIdentity(3417)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct DeleteItemResult_t
	{
		// Token: 0x04000263 RID: 611
		public const int k_iCallback = 3417;

		// Token: 0x04000264 RID: 612
		public EResult m_eResult;

		// Token: 0x04000265 RID: 613
		public PublishedFileId_t m_nPublishedFileId;
	}
}
