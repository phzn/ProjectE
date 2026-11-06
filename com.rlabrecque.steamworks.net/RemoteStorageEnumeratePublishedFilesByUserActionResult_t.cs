using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000A8 RID: 168
	[CallbackIdentity(1328)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageEnumeratePublishedFilesByUserActionResult_t
	{
		// Token: 0x040001FD RID: 509
		public const int k_iCallback = 1328;

		// Token: 0x040001FE RID: 510
		public EResult m_eResult;

		// Token: 0x040001FF RID: 511
		public EWorkshopFileAction m_eAction;

		// Token: 0x04000200 RID: 512
		public int m_nResultsReturned;

		// Token: 0x04000201 RID: 513
		public int m_nTotalResultCount;

		// Token: 0x04000202 RID: 514
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public PublishedFileId_t[] m_rgPublishedFileId;

		// Token: 0x04000203 RID: 515
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public uint[] m_rgRTimeUpdated;
	}
}
