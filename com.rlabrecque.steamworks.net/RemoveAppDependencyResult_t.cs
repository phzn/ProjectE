using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000C0 RID: 192
	[CallbackIdentity(3415)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoveAppDependencyResult_t
	{
		// Token: 0x04000259 RID: 601
		public const int k_iCallback = 3415;

		// Token: 0x0400025A RID: 602
		public EResult m_eResult;

		// Token: 0x0400025B RID: 603
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x0400025C RID: 604
		public AppId_t m_nAppID;
	}
}
