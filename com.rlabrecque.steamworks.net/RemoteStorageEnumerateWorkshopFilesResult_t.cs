using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200009F RID: 159
	[CallbackIdentity(1319)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageEnumerateWorkshopFilesResult_t
	{
		// Token: 0x040001D5 RID: 469
		public const int k_iCallback = 1319;

		// Token: 0x040001D6 RID: 470
		public EResult m_eResult;

		// Token: 0x040001D7 RID: 471
		public int m_nResultsReturned;

		// Token: 0x040001D8 RID: 472
		public int m_nTotalResultCount;

		// Token: 0x040001D9 RID: 473
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public PublishedFileId_t[] m_rgPublishedFileId;

		// Token: 0x040001DA RID: 474
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public float[] m_rgScore;

		// Token: 0x040001DB RID: 475
		public AppId_t m_nAppId;

		// Token: 0x040001DC RID: 476
		public uint m_unStartIndex;
	}
}
