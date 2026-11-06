using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000BE RID: 190
	[CallbackIdentity(3413)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoveUGCDependencyResult_t
	{
		// Token: 0x04000251 RID: 593
		public const int k_iCallback = 3413;

		// Token: 0x04000252 RID: 594
		public EResult m_eResult;

		// Token: 0x04000253 RID: 595
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x04000254 RID: 596
		public PublishedFileId_t m_nChildPublishedFileId;
	}
}
