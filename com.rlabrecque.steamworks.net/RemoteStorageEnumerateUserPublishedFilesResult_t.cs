using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000098 RID: 152
	[CallbackIdentity(1312)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageEnumerateUserPublishedFilesResult_t
	{
		// Token: 0x040001A3 RID: 419
		public const int k_iCallback = 1312;

		// Token: 0x040001A4 RID: 420
		public EResult m_eResult;

		// Token: 0x040001A5 RID: 421
		public int m_nResultsReturned;

		// Token: 0x040001A6 RID: 422
		public int m_nTotalResultCount;

		// Token: 0x040001A7 RID: 423
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public PublishedFileId_t[] m_rgPublishedFileId;
	}
}
