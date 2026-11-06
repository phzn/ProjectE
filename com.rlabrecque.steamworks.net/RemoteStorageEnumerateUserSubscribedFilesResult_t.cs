using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200009A RID: 154
	[CallbackIdentity(1314)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageEnumerateUserSubscribedFilesResult_t
	{
		// Token: 0x040001AB RID: 427
		public const int k_iCallback = 1314;

		// Token: 0x040001AC RID: 428
		public EResult m_eResult;

		// Token: 0x040001AD RID: 429
		public int m_nResultsReturned;

		// Token: 0x040001AE RID: 430
		public int m_nTotalResultCount;

		// Token: 0x040001AF RID: 431
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public PublishedFileId_t[] m_rgPublishedFileId;

		// Token: 0x040001B0 RID: 432
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public uint[] m_rgRTimeSubscribed;
	}
}
