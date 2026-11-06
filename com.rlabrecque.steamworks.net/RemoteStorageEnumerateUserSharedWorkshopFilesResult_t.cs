using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000A6 RID: 166
	[CallbackIdentity(1326)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageEnumerateUserSharedWorkshopFilesResult_t
	{
		// Token: 0x040001F4 RID: 500
		public const int k_iCallback = 1326;

		// Token: 0x040001F5 RID: 501
		public EResult m_eResult;

		// Token: 0x040001F6 RID: 502
		public int m_nResultsReturned;

		// Token: 0x040001F7 RID: 503
		public int m_nTotalResultCount;

		// Token: 0x040001F8 RID: 504
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public PublishedFileId_t[] m_rgPublishedFileId;
	}
}
