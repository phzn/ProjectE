using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000BD RID: 189
	[CallbackIdentity(3412)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct AddUGCDependencyResult_t
	{
		// Token: 0x0400024D RID: 589
		public const int k_iCallback = 3412;

		// Token: 0x0400024E RID: 590
		public EResult m_eResult;

		// Token: 0x0400024F RID: 591
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x04000250 RID: 592
		public PublishedFileId_t m_nChildPublishedFileId;
	}
}
