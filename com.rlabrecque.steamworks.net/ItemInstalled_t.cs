using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000B6 RID: 182
	[CallbackIdentity(3405)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct ItemInstalled_t
	{
		// Token: 0x04000232 RID: 562
		public const int k_iCallback = 3405;

		// Token: 0x04000233 RID: 563
		public AppId_t m_unAppID;

		// Token: 0x04000234 RID: 564
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x04000235 RID: 565
		public UGCHandle_t m_hLegacyContent;

		// Token: 0x04000236 RID: 566
		public ulong m_unManifestID;
	}
}
