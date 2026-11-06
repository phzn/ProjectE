using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000BF RID: 191
	[CallbackIdentity(3414)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct AddAppDependencyResult_t
	{
		// Token: 0x04000255 RID: 597
		public const int k_iCallback = 3414;

		// Token: 0x04000256 RID: 598
		public EResult m_eResult;

		// Token: 0x04000257 RID: 599
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x04000258 RID: 600
		public AppId_t m_nAppID;
	}
}
