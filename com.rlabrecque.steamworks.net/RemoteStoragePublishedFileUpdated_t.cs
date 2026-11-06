using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000AA RID: 170
	[CallbackIdentity(1330)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStoragePublishedFileUpdated_t
	{
		// Token: 0x04000207 RID: 519
		public const int k_iCallback = 1330;

		// Token: 0x04000208 RID: 520
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x04000209 RID: 521
		public AppId_t m_nAppID;

		// Token: 0x0400020A RID: 522
		public ulong m_ulUnused;
	}
}
